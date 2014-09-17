//******************************************************************************************************
//  ConfigurationOperation.cs - Gbtc
//
//  Copyright © 2014, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the Eclipse Public License -v 1.0 (the "License"); you may
//  not use this file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://www.opensource.org/licenses/eclipse-1.0.php
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  07/21/2014 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using GSF.Collections;
using FaultData.DataAnalysis;
using FaultData.Database;
using ChannelKey = System.Tuple<int, string, string, string, string>;
using SeriesKey = System.Tuple<int, string, string, string, string, string>;

namespace FaultData.DataOperations
{
    public class ConfigurationOperation : IDataOperation
    {
        #region [ Members ]

        // Nested Types
        private class ConfigurationHelper
        {
            public MeterInfoDataContext MeterInfo;
            public Dictionary<string, MeasurementType> MeasurementTypeLookup = new Dictionary<string, MeasurementType>();
            public Dictionary<string, MeasurementCharacteristic> MeasurementCharacteristicLookup = new Dictionary<string, MeasurementCharacteristic>();
            public Dictionary<string, Phase> PhaseLookup = new Dictionary<string, Phase>();
            public Dictionary<string, SeriesType> SeriesTypeLookup = new Dictionary<string, SeriesType>();

            public ConfigurationHelper(string connectionString)
            {
                MeterInfo = new MeterInfoDataContext(connectionString);
            }
        }

        // Constants
        private static readonly double Sqrt3 = Math.Sqrt(3.0D);
		
        // Fields
        private string m_connectionString;
        private bool m_modifyConfiguration;
        private string m_filePathPattern;
		
        #endregion

        #region [ Constructors ]

        public ConfigurationOperation(string connectionString)
        {
            m_connectionString = connectionString;
        }

        #endregion

        #region [ Properties ]

        public bool ModifyConfiguration
        {
            get
            {
                return m_modifyConfiguration;
            }
            set
            {
                m_modifyConfiguration = value;
            }
        }

        public string FilePathPattern
        {
            get
            {
                return m_filePathPattern;
            }
            set
            {
                m_filePathPattern = value;
            }
        }

        #endregion

        #region [ Methods ]

        public void Execute(MeterDataSet meterDataSet)
        {
            ConfigurationHelper configurationHelper;

            Meter meter;
            MeterFileGroup meterFileGroup;
            string meterName;

            Dictionary<SeriesKey, Series> seriesLookup;
            Series seriesInfo;

            // Try to parse the name of the meter from the file path
            if (!string.IsNullOrEmpty(FilePathPattern) && TryParseFilePath(meterDataSet.FilePath, out meterName))
                meterDataSet.Meter.Name = meterName;

            // Search the database for a meter definition that matches the parsed meter
            configurationHelper = new ConfigurationHelper(m_connectionString);
            meter = configurationHelper.MeterInfo.Meters.SingleOrDefault(m => m.Name == meterDataSet.Meter.Name);

            if ((object)meter != null)
            {
                // Match the parsed series with the ones associated with the meter in the database
                seriesLookup = meter.Channels
                    .SelectMany(channel => channel.Series)
                    .Where(series => string.IsNullOrEmpty(series.SourceIndexes))
                    .ToDictionary(GetKey);

                foreach (DataSeries dataSeries in meterDataSet.DataSeries)
                {
                    if ((object)dataSeries.SeriesInfo == null)
                        continue;

                    if (seriesLookup.TryGetValue(GetKey(dataSeries.SeriesInfo), out seriesInfo))
                        dataSeries.SeriesInfo = seriesInfo;
                }

                // Create data series for series which
                // are combinations of the parsed series
                foreach (Series series in meter.Channels.SelectMany(channel => channel.Series))
                {
                    if (!string.IsNullOrEmpty(series.SourceIndexes))
                        AddCalculatedDataSeries(meterDataSet, series);
                }

                // There may be some placeholder DataSeries objects with no data so that indexes
                // would be correct for calculating data series--now that we are finished
                // calculating data series, these need to be removed
                for (int i = meterDataSet.DataSeries.Count - 1; i >= 0; i--)
                {
                    if ((object)meterDataSet.DataSeries[i].SeriesInfo == null)
                        meterDataSet.DataSeries.RemoveAt(i);
                }

                // Replace the parsed meter with
                // the one from the database
                meterDataSet.Meter = meter;
            }
            else if (m_modifyConfiguration)
            {
                // Make sure to insert the new meter's configuration when changes are submitted
                configurationHelper.MeterInfo.Meters.InsertOnSubmit(meterDataSet.Meter);
            }
            else
            {
                // If configuration cannot be modified and existing configuration cannot be found for this meter,
                // throw an exception to indicate the operation could not be executed
                throw new InvalidOperationException("Cannot process meter - configuration does not exist");
            }

            if (m_modifyConfiguration)
            {
                // Some of the data series may still be associated with the parsed
                // meter if there was no matching series definition found in the
                // database configuration so we call this method to fix that
                FixDataSeries(configurationHelper, meterDataSet);
                FixNominalVoltage(meterDataSet);
            }
            else
            {
                // Remove data series that were not defined in the configuration
                // since configuration information cannot be added for it
                RemoveUndefinedDataSeries(meterDataSet);
            }

            meterFileGroup = new MeterFileGroup();
            meterFileGroup.Meter = meterDataSet.Meter;
            meterFileGroup.FileGroupID = meterDataSet.FileGroup.ID;

            // Submit changes to the meter configuration so
            // that they may be entered into the database
            configurationHelper.MeterInfo.SubmitChanges();
        }

        private bool TryParseFilePath(string fileName, out string meterName)
        {
            Match match = Regex.Match(fileName, FilePathPattern);
            Group meterNameGroup;

            if (match.Success)
            {
                meterNameGroup = match.Groups["MeterName"];

                if ((object)meterNameGroup != null)
                {
                    meterName = meterNameGroup.Value;
                    return true;
                }
            }

            meterName = null;
            return false;
        }

        private void AddCalculatedDataSeries(MeterDataSet meterDataSet, Series series)
        {
            int sourceIndex = 0;

            List<Tuple<char, int>> sourceIndexes = series.SourceIndexes.Split(',')
                .Where(str => int.TryParse(str, out sourceIndex))
                .Select(str => Tuple.Create(str[0], sourceIndex))
                .ToList();

            DataSeries dataSeries = sourceIndexes
                .Select(tuple => (tuple.Item1 != '-') ? meterDataSet.DataSeries[tuple.Item2].Copy() : meterDataSet.DataSeries[Math.Abs(tuple.Item2)].Negate())
                .Aggregate((series1, series2) => series1.Add(series2));

            dataSeries.SeriesInfo = series;

            if (!meterDataSet.DataSeries.Contains(dataSeries))
                meterDataSet.DataSeries.Add(dataSeries);
        }

        private void FixDataSeries(ConfigurationHelper configurationHelper, MeterDataSet meterDataSet)
        {
            Dictionary<ChannelKey, Channel> channelLookup;
            Dictionary<SeriesKey, Series> seriesLookup;
            Channel lookedUpChannel;
            Series lookedUpSeries;

            Channel channelInfo;
            ChannelKey channelKey;
            Series seriesInfo;
            SeriesKey seriesKey;

            channelLookup = meterDataSet.Meter.Channels
                .Where(channel => (object)channel.Line != null)
                .ToDictionary(GetKey);

            seriesLookup = channelLookup.Values
                .SelectMany(channel => channel.Series)
                .ToDictionary(GetKey);

            foreach (DataSeries dataSeries in meterDataSet.DataSeries)
            {
                seriesInfo = dataSeries.SeriesInfo;
                channelInfo = seriesInfo.Channel;

                if ((object)channelInfo.Line != null)
                    continue;

                channelKey = GetKey(channelInfo);
                seriesKey = GetKey(seriesInfo);

                if (seriesLookup.TryGetValue(seriesKey, out lookedUpSeries))
                {
                    // An identical series to this one already exists,
                    // so we remove this series' channel and use the existing series instead
                    meterDataSet.Meter.Channels.Remove(channelInfo);
                    dataSeries.SeriesInfo = lookedUpSeries;
                }
                else if (channelLookup.TryGetValue(channelKey, out lookedUpChannel))
                {
                    // An identical channel to this one already exists,
                    // so we remove this one and use the existing one instead
                    meterDataSet.Meter.Channels.Remove(channelInfo);
                    seriesInfo.Channel = lookedUpChannel;

                    // This fixes the series type referenced by this series to use the type that
                    // is referenced by other series to prevent duplication of database records
                    FixSeriesType(configurationHelper, seriesInfo);

                    // Make sure this series can be found on subsequent lookups
                    seriesLookup.Add(seriesKey, seriesInfo);
                }
                else
                {
                    // This channel may still be associated with the parsed meter
                    // even if the meter in the data set was extracted from the
                    // database so we fix the association here
                    channelInfo.Meter = meterDataSet.Meter;

                    // This fixes the objects referenced by the channel and series
                    // so that they match those referenced by the other channels to
                    // prevent duplication of database records
                    FixChannelProperties(configurationHelper, channelInfo);
                    FixSeriesType(configurationHelper, seriesInfo);

                    // Make sure the channel and series can
                    // both be found on subsequent lookups
                    channelLookup.Add(channelKey, channelInfo);
                    seriesLookup.Add(seriesKey, seriesInfo);
                }
            }
        }

        private void RemoveUndefinedDataSeries(MeterDataSet meterDataSet)
        {
            for (int i = meterDataSet.DataSeries.Count - 1; i >= 0; i--)
            {
                if ((object)meterDataSet.DataSeries[i].SeriesInfo.Channel.Line == null)
                    meterDataSet.DataSeries.RemoveAt(i);
            }
        }

        private void FixChannelProperties(ConfigurationHelper configurationHelper, Channel channel)
        {
            channel.Line = FindLine(channel);
            channel.MeasurementType = configurationHelper.MeasurementTypeLookup.GetOrAdd(channel.MeasurementType.Name, s => configurationHelper.MeterInfo.MeasurementTypes.SingleOrDefault(type => type.Name == channel.MeasurementType.Name) ?? channel.MeasurementType);
            channel.MeasurementCharacteristic = configurationHelper.MeasurementCharacteristicLookup.GetOrAdd(channel.MeasurementCharacteristic.Name, s => configurationHelper.MeterInfo.MeasurementCharacteristics.SingleOrDefault(characteristic => characteristic.Name == channel.MeasurementCharacteristic.Name) ?? channel.MeasurementCharacteristic);
            channel.Phase = configurationHelper.PhaseLookup.GetOrAdd(channel.Phase.Name, s => configurationHelper.MeterInfo.Phases.SingleOrDefault(phase => phase.Name == channel.Phase.Name) ?? channel.Phase);
        }

        private void FixSeriesType(ConfigurationHelper configurationHelper, Series series)
        {
            series.SeriesType = configurationHelper.SeriesTypeLookup.GetOrAdd(series.SeriesType.Name, s => configurationHelper.MeterInfo.SeriesTypes.SingleOrDefault(type => type.Name == series.SeriesType.Name) ?? series.SeriesType);
        }

        private ChannelKey GetKey(Channel channel)
        {
            return Tuple.Create(
                channel.HarmonicGroup,
                channel.Name,
                channel.MeasurementType.Name,
                channel.MeasurementCharacteristic.Name,
                channel.Phase.Name);
        }

        private SeriesKey GetKey(Series series)
        {
            Channel channel = series.Channel;

            return Tuple.Create(
                channel.HarmonicGroup,
                channel.Name,
                channel.MeasurementType.Name,
                channel.MeasurementCharacteristic.Name,
                channel.Phase.Name,
                series.SeriesType.Name);
        }

        private Line FindLine(Channel channel)
        {
            Meter meter;
            List<Line> lines;
            Line stubLine;

            meter = channel.Meter;

            lines = meter.Channels
                .Select(ch => ch.Line)
                .Where(line => (object)line != null)
                .Distinct()
                .ToList();

            if (lines.Count == 0)
                lines.AddRange(meter.MeterLocation.Lines);

            if (lines.Count == 1)
                stubLine = lines[0];
            else
                stubLine = lines.FirstOrDefault(line => line.Name.StartsWith("[STUBLINE]"));

            return stubLine ?? CreateStubLine(meter);
        }

        private Line CreateStubLine(Meter meter)
        {
            Line stubLine;
            string stubLineName;

            MeterLocationLine meterLocationLine;

            stubLine = new Line();
            stubLineName = string.Format("[STUBLINE] {0}", meter.Name);

            stubLine.Name = stubLineName;
            stubLine.AssetKey = stubLineName;
            stubLine.ShortName = meter.Name.Substring(0, Math.Min(meter.Name.Length, 50));
            stubLine.Description = string.Format("Line monitored by {0}", meter.Name);
            stubLine.VoltageKV = -1.0D;

            meterLocationLine = new MeterLocationLine();
            meterLocationLine.MeterLocation = meter.MeterLocation;
            meterLocationLine.Line = stubLine;

            return stubLine;
        }

        private void FixNominalVoltage(MeterDataSet meterDataSet)
        {
            const int LineToNeutral = 0;
            const int LineToLine = 1;
            const int Other = 2;

            Func<string, int> getPhaseGroup = phaseName =>
            {
                switch (phaseName)
                {
                    case "AN":
                    case "BN":
                    case "CN":
                        return LineToNeutral;

                    case "AB":
                    case "BC":
                    case "CA":
                        return LineToLine;

                    default:
                        return Other;
                }
            };

            List<IGrouping<int, List<DataPoint>>> rmsVoltages;
            double totalVoltage;
            int dataPointCount;
            double averageVoltage;

            foreach (Line line in meterDataSet.Meter.MeterLocation.Lines)
            {
                if (line.VoltageKV != -1.0D || !line.Name.StartsWith("[STUBLINE]"))
                    continue;

                rmsVoltages = meterDataSet.DataSeries
                    .Where(series => series.SeriesInfo.Channel.Line == line)
                    .Where(series => series.SeriesInfo.Channel.MeasurementType.Name == "Voltage")
                    .Where(series => series.SeriesInfo.Channel.MeasurementCharacteristic.Name == "RMS")
                    .Where(series => series.SeriesInfo.SeriesType.Name == "Average")
                    .GroupBy(series => getPhaseGroup(series.SeriesInfo.Channel.Phase.Name), series => series.DataPoints)
                    .Where(group => group.Key != Other)
                    .ToList();

                if (rmsVoltages.Count == 0)
                    continue;

                totalVoltage = rmsVoltages.Sum(group => group.Sum(dataPoints => dataPoints.Sum(dataPoint => dataPoint.Value)) * ((group.Key == LineToNeutral) ? Sqrt3 : 1.0D));
                dataPointCount = rmsVoltages.Sum(group => group.Sum(dataPoints => dataPoints.Count));
                averageVoltage = 0.001D * totalVoltage / dataPointCount;

                line.VoltageKV = Math.Round(averageVoltage);
            }
        }

        #endregion
    }
}
