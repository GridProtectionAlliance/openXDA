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
using System.Configuration;
using System.Linq;
using FaultData.DataAnalysis;
using FaultData.DataSets;
using GSF.Collections;
using GSF.Data;
using GSF.Data.Model;
using log4net;
using openXDA.Model;

namespace FaultData.DataOperations
{
    public class ConfigurationOperation : DataOperationBase<MeterDataSet>
    {
        #region [ Members ]

        // Nested Types
        private class SourceIndex
        {
            public double Multiplier;
            public int ChannelIndex;

            public static SourceIndex Parse(string text)
            {
                SourceIndex sourceIndex = new SourceIndex();

                string[] parts = text.Split('*');
                string multiplier = (parts.Length > 1) ? parts[0].Trim() : "1";
                string channelIndex = (parts.Length > 1) ? parts[1].Trim() : parts[0].Trim();

                if (parts.Length > 2)
                    throw new FormatException($"Too many asterisks found in source index {text}.");

                if (!double.TryParse(multiplier, out sourceIndex.Multiplier))
                    throw new FormatException($"Incorrect format for multiplier {multiplier} found in source index {text}.");

                if (channelIndex == "NONE")
                    return null;

                if (!int.TryParse(channelIndex, out sourceIndex.ChannelIndex))
                    throw new FormatException($"Incorrect format for channel index {channelIndex} found in source index {text}.");

                if (channelIndex[0] == '-')
                {
                    sourceIndex.Multiplier *= -1.0D;
                    sourceIndex.ChannelIndex *= -1;
                }

                return sourceIndex;
            }
        }

        // Constants
        private const double Sqrt3 = 1.7320508075688772935274463415059D;

        // Fields
        private string m_filePattern;
        private double m_systemFrequency;

        #endregion

        #region [ Properties ]

        [Setting]
        public string FilePattern
        {
            get
            {
                return m_filePattern;
            }
            set
            {
                m_filePattern = value;
            }
        }

        [Setting]
        public double SystemFrequency
        {
            get
            {
                return m_systemFrequency;
            }
            set
            {
                m_systemFrequency = value;
            }
        }

        #endregion

        #region [ Methods ]

        public override void Execute(MeterDataSet meterDataSet)
        {
            Meter parsedMeter;
            Meter dbMeter;
            List<Series> seriesList;

            Log.Info("Executing operation to locate meter in database...");

            // Grab the parsed meter right away as we will be replacing it in the meter data set with the meter from the database
            parsedMeter = meterDataSet.Meter;

            // Search the database for a meter definition that matches the parsed meter
            using (AdoDataConnection connection = meterDataSet.CreateDbConnection())
            {
                TableOperations<Meter> meterTable = new TableOperations<Meter>(connection);
                dbMeter = meterTable.QueryRecordWhere("AssetKey = {0}", parsedMeter.AssetKey);
                dbMeter.ConnectionFactory = meterDataSet.CreateDbConnection;
            }

            if ((object)dbMeter == null)
            {
                Log.Info(string.Format("No existing meter found matching meter with name {0}.", parsedMeter.Name));

                // If configuration cannot be modified and existing configuration cannot be found for this meter,
                // throw an exception to indicate the operation could not be executed
                throw new InvalidOperationException("Cannot process meter - configuration does not exist");
            }

            Log.Info(string.Format("Found meter {0} in database.", dbMeter.Name));

            // Replace the parsed meter with
            // the one from the database
            meterDataSet.Meter = dbMeter;

            // Get the list of series associated with the meter in the database
            seriesList = dbMeter.Channels
                .SelectMany(channel => channel.Series)
                .ToList();

            // Create data series for series which
            // are combinations of the parsed series
            List<DataSeries> calculatedDataSeriesList = new List<DataSeries>();

            foreach (Series series in seriesList.Where(series => !string.IsNullOrEmpty(series.SourceIndexes)))
                AddCalculatedDataSeries(calculatedDataSeriesList, meterDataSet, series);

            foreach (DataSeries calculatedDataSeries in calculatedDataSeriesList)
            {
                if (calculatedDataSeries.SeriesInfo.Channel.MeasurementType.Name != "Digital")
                    meterDataSet.DataSeries.Add(calculatedDataSeries);
                else
                    meterDataSet.Digitals.Add(calculatedDataSeries);
            }

            // There may be some placeholder DataSeries objects with no data so that indexes
            // would be correct for calculating data series--now that we are finished
            // calculating data series, these need to be removed
            for (int i = meterDataSet.DataSeries.Count - 1; i >= 0; i--)
            {
                if ((object)meterDataSet.DataSeries[i].SeriesInfo == null)
                    meterDataSet.DataSeries.RemoveAt(i);
            }

            for (int i = meterDataSet.Digitals.Count - 1; i >= 0; i--)
            {
                if ((object)meterDataSet.Digitals[i].SeriesInfo == null)
                    meterDataSet.Digitals.RemoveAt(i);
            }

            // Remove data series that were not defined
            // in the configuration or the source data
            RemoveUnknownChannelTypes(meterDataSet);

            // Add channels that are not already defined in the
            // configuration by assuming the meter monitors only one line
            AddUndefinedChannels(meterDataSet);

            // Set samples per hour and enabled flags based on
            // the configuration obtained from the latest file
            FixUpdatedChannelInfo(meterDataSet, parsedMeter);

            // Update line parameters pulled from the input data
            UpdateConfigurationData(meterDataSet);
        }

        private void AddCalculatedDataSeries(List<DataSeries> calculatedDataSeriesList, MeterDataSet meterDataSet, Series series)
        {
            List<SourceIndex> sourceIndexes;
            DataSeries dataSeries;

            sourceIndexes = series.SourceIndexes.Split(',')
                .Select(SourceIndex.Parse)
                .Where(sourceIndex => (object)sourceIndex != null)
                .ToList();

            if (sourceIndexes.Count == 0)
                return;

            if (series.Channel.MeasurementType.Name != "Digital")
            {
                if (sourceIndexes.Any(sourceIndex => sourceIndex.ChannelIndex >= meterDataSet.DataSeries.Count))
                    return;

                dataSeries = sourceIndexes
                    .Select(sourceIndex => meterDataSet.DataSeries[sourceIndex.ChannelIndex].Multiply(sourceIndex.Multiplier))
                    .Aggregate((series1, series2) => series1.Add(series2));
            }
            else
            {
                if (sourceIndexes.Any(sourceIndex => Math.Abs(sourceIndex.ChannelIndex) >= meterDataSet.Digitals.Count))
                    return;

                dataSeries = sourceIndexes
                    .Select(sourceIndex => meterDataSet.Digitals[sourceIndex.ChannelIndex].Multiply(sourceIndex.Multiplier))
                    .Aggregate((series1, series2) => series1.Add(series2));
            }

            dataSeries.SeriesInfo = series;
            calculatedDataSeriesList.Add(dataSeries);
        }

        private void AddUndefinedChannels(MeterDataSet meterDataSet)
        {
            List<DataSeries> undefinedDataSeries = meterDataSet.DataSeries
                .Concat(meterDataSet.Digitals)
                .Where(dataSeries => (object)dataSeries.SeriesInfo.Channel.Line == null)
                .ToList();

            if (undefinedDataSeries.Count <= 0)
                return;

            Meter meter = meterDataSet.Meter;

            if (meter.MeterLines.Count == 0)
            {
                Log.Warn($"Unable to automatically add channels to meter {meterDataSet.Meter.Name} because there are no lines associated with that meter.");
                return;
            }

            if (meter.MeterLines.Count > 1)
            {
                Log.Warn($"Unable to automatically add channels to meter {meterDataSet.Meter.Name} because there are too many lines associated with that meter.");
                return;
            }

            Line line = meter.MeterLines
                .Select(meterLine => meterLine.Line)
                .Single();

            foreach (DataSeries series in undefinedDataSeries)
                series.SeriesInfo.Channel.LineID = line.ID;

            using (AdoDataConnection connection = meterDataSet.CreateDbConnection())
            {
                TableOperations<MeasurementType> measurementTypeTable = new TableOperations<MeasurementType>(connection);
                TableOperations<MeasurementCharacteristic> measurementCharacteristicTable = new TableOperations<MeasurementCharacteristic>(connection);
                TableOperations<Phase> phaseTable = new TableOperations<Phase>(connection);
                TableOperations<SeriesType> seriesTypeTable = new TableOperations<SeriesType>(connection);

                Dictionary<string, MeasurementType> measurementTypeLookup = undefinedDataSeries
                    .Select(dataSeries => dataSeries.SeriesInfo.Channel.MeasurementType)
                    .DistinctBy(measurementType => measurementType.Name)
                    .Select(measurementType => measurementTypeTable.GetOrAdd(measurementType.Name, measurementType.Description))
                    .ToDictionary(measurementType => measurementType.Name);

                Dictionary<string, MeasurementCharacteristic> measurementCharacteristicLookup = undefinedDataSeries
                    .Select(dataSeries => dataSeries.SeriesInfo.Channel.MeasurementCharacteristic)
                    .DistinctBy(measurementCharacteristic => measurementCharacteristic.Name)
                    .Select(measurementCharacteristic => measurementCharacteristicTable.GetOrAdd(measurementCharacteristic.Name, measurementCharacteristic.Description))
                    .ToDictionary(measurementCharacteristic => measurementCharacteristic.Name);

                Dictionary<string, Phase> phaseLookup = undefinedDataSeries
                    .Select(dataSeries => dataSeries.SeriesInfo.Channel.Phase)
                    .DistinctBy(phase => phase.Name)
                    .Select(phase => phaseTable.GetOrAdd(phase.Name, phase.Description))
                    .ToDictionary(phase => phase.Name);

                Dictionary<string, SeriesType> seriesTypeLookup = undefinedDataSeries
                    .Select(dataSeries => dataSeries.SeriesInfo.SeriesType)
                    .DistinctBy(seriesType => seriesType.Name)
                    .Select(seriesType => seriesTypeTable.GetOrAdd(seriesType.Name, seriesType.Description))
                    .ToDictionary(seriesType => seriesType.Name);

                Dictionary<ChannelKey, Channel> channelLookup = meter.Channels
                    .GroupBy(channel => new ChannelKey(channel))
                    .ToDictionary(grouping =>
                    {
                        if (grouping.Count() > 1)
                            Log.Warn($"Detected duplicate channel key: {grouping.First().ID}");

                        return grouping.Key;
                    }, grouping => grouping.First());

                List<Channel> undefinedChannels = undefinedDataSeries
                    .Select(dataSeries => dataSeries.SeriesInfo.Channel)
                    .GroupBy(channel => new ChannelKey(channel))
                    .Where(grouping => !channelLookup.ContainsKey(grouping.Key))
                    .Select(grouping => grouping.First())
                    .ToList();

                TableOperations<Channel> channelTable = new TableOperations<Channel>(connection);

                // Add all undefined channels to the database
                foreach (Channel channel in undefinedChannels)
                {
                    string measurementTypeName = channel.MeasurementType.Name;
                    string measurementCharacteristicName = channel.MeasurementCharacteristic.Name;
                    string phaseName = channel.Phase.Name;

                    channel.MeterID = meter.ID;
                    channel.LineID = line.ID;
                    channel.MeasurementTypeID = measurementTypeLookup[measurementTypeName].ID;
                    channel.MeasurementCharacteristicID = measurementCharacteristicLookup[measurementCharacteristicName].ID;
                    channel.PhaseID = phaseLookup[phaseName].ID;
                    channel.Enabled = true;

                    // If the per-unit value was not specified in the input file,
                    // we can obtain the per-unit value from the line configuration
                    // if the channel happens to be an instantaneous or RMS voltage
                    if (!channel.PerUnitValue.HasValue)
                    {
                        if (IsVoltage(channel))
                        {
                            if (IsLineToNeutral(channel))
                                channel.PerUnitValue = (line.VoltageKV * 1000.0D) / Sqrt3;
                            else if (IsLineToLine(channel))
                                channel.PerUnitValue = line.VoltageKV * 1000.0D;
                        }
                    }

                    channelTable.AddNewRecord(channel);
                }

                if (undefinedChannels.Count > 0)
                {
                    // Refresh the channel lookup to
                    // include all the new channels
                    meter.Channels = null;

                    channelLookup = meter.Channels
                        .GroupBy(channel => new ChannelKey(channel))
                        .ToDictionary(grouping => grouping.Key, grouping => grouping.First());
                }

                Dictionary<SeriesKey, Series> seriesLookup = meter.Channels
                    .SelectMany(channel => channel.Series)
                    .Where(series => series.SourceIndexes == "")
                    .GroupBy(series => new SeriesKey(series))
                    .ToDictionary(grouping =>
                    {
                        if (grouping.Count() > 1)
                            Log.Warn($"Detected duplicate series key: {grouping.First().ID}");

                        return grouping.Key;
                    }, grouping => grouping.First());

                List<Series> undefinedSeries = undefinedDataSeries
                    .SelectMany(dataSeries => dataSeries.SeriesInfo.Channel.Series)
                    .GroupBy(series => new SeriesKey(series))
                    .Where(grouping => !seriesLookup.ContainsKey(grouping.Key))
                    .Select(grouping => grouping.First())
                    .ToList();

                TableOperations<Series> seriesTable = new TableOperations<Series>(connection);

                // Add all undefined series objects to the database
                foreach (Series series in undefinedSeries)
                {
                    ChannelKey channelKey = new ChannelKey(series.Channel);
                    string seriesTypeName = series.SeriesType.Name;

                    series.ChannelID = channelLookup[channelKey].ID;
                    series.SeriesTypeID = seriesTypeLookup[seriesTypeName].ID;
                    series.SourceIndexes = "";

                    seriesTable.AddNewRecord(series);
                }

                if (undefinedSeries.Count > 0)
                {
                    // Refresh the series lookup to
                    // include all the new series
                    foreach (Channel channel in meter.Channels)
                        channel.Series = null;

                    seriesLookup = meter.Channels
                        .SelectMany(channel => channel.Series)
                        .GroupBy(series => new SeriesKey(series))
                        .ToDictionary(grouping => grouping.Key, grouping => grouping.First());
                }

                // Update all undefined data series to reference the new database objects
                foreach (DataSeries dataSeries in undefinedDataSeries)
                {
                    SeriesKey seriesKey = new SeriesKey(dataSeries.SeriesInfo);
                    Series series = seriesLookup[seriesKey];
                    dataSeries.SeriesInfo = series;
                }
            }
        }

        private void UpdateConfigurationData(MeterDataSet meterDataSet)
        {
            bool updateLineLength = meterDataSet.Configuration.LineLength.HasValue;

            bool updateLineImpedance =
                meterDataSet.Configuration.R1.HasValue &&
                meterDataSet.Configuration.X1.HasValue &&
                meterDataSet.Configuration.R0.HasValue &&
                meterDataSet.Configuration.X0.HasValue;

            if (!updateLineLength && !updateLineImpedance)
                return;

            using (AdoDataConnection connection = meterDataSet.CreateDbConnection())
            {
                TableOperations<Line> lineTable = new TableOperations<Line>(connection);

                List<Line> lines = lineTable.QueryRecordsWhere("ID IN (SELECT LineID FROM MeterLocationLine WHERE MeterLocationID = {0})", meterDataSet.Meter.MeterLocationID).ToList();

                if (lines.Count != 1)
                    return;

                Line line = lines[0];

                if (updateLineLength)
                {
                    line.Length = meterDataSet.Configuration.LineLength.GetValueOrDefault();
                    lineTable.UpdateRecord(line);
                }

                if (updateLineImpedance)
                {
                    TableOperations<LineImpedance> lineImpedanceTable = new TableOperations<LineImpedance>(connection);
                    LineImpedance lineImpedance = lineImpedanceTable.QueryRecordWhere("LineID = {0}", line.ID);

                    if ((object)lineImpedance == null)
                        lineImpedance = new LineImpedance() { ID = line.ID };

                    if (meterDataSet.Configuration.R1.HasValue)
                        lineImpedance.R1 = meterDataSet.Configuration.R1.GetValueOrDefault();

                    if (meterDataSet.Configuration.X1.HasValue)
                        lineImpedance.X1 = meterDataSet.Configuration.X1.GetValueOrDefault();

                    if (meterDataSet.Configuration.R0.HasValue)
                        lineImpedance.R0 = meterDataSet.Configuration.R0.GetValueOrDefault();

                    if (meterDataSet.Configuration.X0.HasValue)
                        lineImpedance.X0 = meterDataSet.Configuration.X0.GetValueOrDefault();

                    try
                    {
                        lineImpedanceTable.AddNewOrUpdateRecord(lineImpedance);
                    }
                    catch (Exception ex)
                    {
                        // Ignore errors regarding unique key constraints
                        // which can occur as a result of a race condition
                        bool isUniqueViolation = ExceptionHandler.IsUniqueViolation(ex);

                        if (!isUniqueViolation)
                            throw;
                    }
                }
            }
        }

        private void FixUpdatedChannelInfo(MeterDataSet meterDataSet, Meter parsedMeter)
        {
            using (AdoDataConnection connection = meterDataSet.CreateDbConnection())
            {
                TableOperations<Channel> channelTable = new TableOperations<Channel>(connection);

                foreach (DataSeries dataSeries in meterDataSet.DataSeries)
                {
                    if ((object)dataSeries.SeriesInfo != null && dataSeries.DataPoints.Count > 1)
                    {
                        double samplesPerHour = (dataSeries.DataPoints.Count - 1) / (dataSeries.Duration / 3600.0D);
                        double roundedSamplesPerHour = Math.Round(samplesPerHour);

                        if (Math.Abs(samplesPerHour - roundedSamplesPerHour) < 0.1D)
                            samplesPerHour = roundedSamplesPerHour;

                        if (samplesPerHour <= 60.0D)
                        {
                            Channel channel = dataSeries.SeriesInfo.Channel;
                            channel.SamplesPerHour = samplesPerHour;
                            channelTable.UpdateRecord(channel);
                        }
                    }
                }

                IEnumerable<ChannelKey> parsedChannelKeys = parsedMeter.Channels
                    .Concat(meterDataSet.DataSeries.Select(dataSeries => dataSeries.SeriesInfo.Channel))
                    .Where(channel => (object)channel.Line != null)
                    .Select(channel => new ChannelKey(channel));

                HashSet<ChannelKey> parsedChannelLookup = new HashSet<ChannelKey>(parsedChannelKeys);

                foreach (Channel channel in meterDataSet.Meter.Channels)
                {
                    channel.Enabled = parsedChannelLookup.Contains(new ChannelKey(channel));
                    channelTable.UpdateRecord(channel);
                }
            }
        }

        private void RemoveUnknownChannelTypes(MeterDataSet meterDataSet)
        {
            for (int i = meterDataSet.DataSeries.Count - 1; i >= 0; i--)
            {
                Series seriesInfo = meterDataSet.DataSeries[i].SeriesInfo;
                Channel channel = seriesInfo.Channel;

                string[] typeIdentifiers =
                {
                    channel.MeasurementType.Name,
                    channel.MeasurementCharacteristic.Name,
                    channel.Phase.Name,
                    seriesInfo.SeriesType.Name
                };

                if (typeIdentifiers.Contains("Unknown", StringComparer.OrdinalIgnoreCase))
                    meterDataSet.DataSeries.RemoveAt(i);
            }

            for (int i = meterDataSet.Digitals.Count - 1; i >= 0; i--)
            {
                Series seriesInfo = meterDataSet.Digitals[i].SeriesInfo;
                Channel channel = seriesInfo.Channel;

                string[] typeIdentifiers =
                {
                    channel.MeasurementType.Name,
                    channel.MeasurementCharacteristic.Name,
                    channel.Phase.Name,
                    seriesInfo.SeriesType.Name
                };

                if (typeIdentifiers.Contains("Unknown", StringComparer.OrdinalIgnoreCase))
                    meterDataSet.Digitals.RemoveAt(i);
            }
        }

        #endregion

        #region [ Static ]

        // Static Fields
        private static readonly ILog Log = LogManager.GetLogger(typeof(ConfigurationOperation));

        // Static Methods
        private static ChannelKey GetGenericChannelKey(Channel channel)
        {
            return new ChannelKey(0, channel.HarmonicGroup, channel.Name, channel.MeasurementType.Name, channel.MeasurementCharacteristic.Name, channel.Phase.Name);
        }

        private static bool IsVoltage(Channel channel)
        {
            return channel.MeasurementType.Name == "Voltage" &&
                   (channel.MeasurementCharacteristic.Name == "Instantaneous" ||
                    channel.MeasurementCharacteristic.Name == "RMS");
        }

        private static bool IsLineToNeutral(Channel channel)
        {
            return channel.Phase.Name == "AN" ||
                   channel.Phase.Name == "BN" ||
                   channel.Phase.Name == "CN" ||
                   channel.Phase.Name == "RES" ||
                   channel.Phase.Name == "NG" ||
                   channel.Phase.Name == "LineToNeutralAverage";
        }

        private static bool IsLineToLine(Channel channel)
        {
            return channel.Phase.Name == "AB" ||
                   channel.Phase.Name == "BC" ||
                   channel.Phase.Name == "CA" ||
                   channel.Phase.Name == "LineToLineAverage";
        }

        #endregion
    }
}
