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
using System.Text.RegularExpressions;
using FaultData.DataAnalysis;
using FaultData.Database;
using FaultData.DataResources;
using FaultData.DataSets;
using log4net;
using ChannelKey = System.Tuple<int, string, string, string, string>;
using SeriesKey = System.Tuple<int, string, string, string, string, string>;

namespace FaultData.DataOperations
{
    public class ConfigurationOperation : DataOperationBase<MeterDataSet>
    {
        #region [ Members ]

        // Fields
        private string m_filePattern;

        private MeterInfoDataContext m_meterInfo;

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

        #endregion

        #region [ Methods ]

        public override void Prepare(DbAdapterContainer dbAdapterContainer)
        {
            m_meterInfo = dbAdapterContainer.GetAdapter<MeterInfoDataContext>();
        }

        public override void Execute(MeterDataSet meterDataSet)
        {
            Meter meter;
            string meterKey;

            List<Series> seriesList;
            Dictionary<SeriesKey, Series> seriesLookup;
            DataSeries dataSeries;
            Series seriesInfo;

            Log.Info("Executing operation to locate meter in database...");

            // Try to parse the name of the meter from the file path
            if (!string.IsNullOrEmpty(FilePattern) && TryParseFilePath(meterDataSet.FilePath, out meterKey))
                meterDataSet.Meter.AssetKey = meterKey;

            // Search the database for a meter definition that matches the parsed meter
            meter = m_meterInfo.Meters.SingleOrDefault(m => m.AssetKey == meterDataSet.Meter.AssetKey);

            if ((object)meter != null)
            {
                Log.Info(string.Format("Found meter {0} in database.", meter.Name));

                // Get the list of series associated with the meter in the database
                seriesList = m_meterInfo.Series
                    .Where(series => series.Channel.MeterID == meter.ID)
                    .ToList();

                // Match the parsed series with the ones associated with the meter in the database
                seriesLookup = seriesList
                    .Where(series => string.IsNullOrEmpty(series.SourceIndexes))
                    .ToDictionary(GetSeriesKey);

                foreach (DataSeries series in meterDataSet.DataSeries)
                {
                    if ((object)series.SeriesInfo == null)
                        continue;

                    if (seriesLookup.TryGetValue(GetSeriesKey(series.SeriesInfo), out seriesInfo))
                        series.SeriesInfo = seriesInfo;
                }

                // Create data series for series which
                // are combinations of the parsed series
                foreach (Series series in seriesList.Where(series => !string.IsNullOrEmpty(series.SourceIndexes)))
                    AddCalculatedDataSeries(meterDataSet, series);

                // There may be some placeholder DataSeries objects with no data so that indexes
                // would be correct for calculating data series--now that we are finished
                // calculating data series, these need to be removed
                for (int i = meterDataSet.DataSeries.Count - 1; i >= 0; i--)
                {
                    if ((object)meterDataSet.DataSeries[i].SeriesInfo == null)
                        meterDataSet.DataSeries.RemoveAt(i);
                }

                // There may be some placeholder DataSeries objects with no data so that indexes
                // would be correct for calculating data series--now that we are finished
                // calculating data series, these need to be removed
                for (int i = meterDataSet.Digitals.Count - 1; i >= 0; i--)
                {
                    if ((object)meterDataSet.Digitals[i].SeriesInfo == null)
                        meterDataSet.Digitals.RemoveAt(i);
                }

                // Replace the parsed meter with
                // the one from the database
                meterDataSet.Meter = meter;
            }
            else
            {
                Log.Info(string.Format("No existing meter found matching meter with name {0}.", meterDataSet.Meter.Name));

                // If configuration cannot be modified and existing configuration cannot be found for this meter,
                // throw an exception to indicate the operation could not be executed
                throw new InvalidOperationException("Cannot process meter - configuration does not exist");
            }

            if (meterDataSet.FilePath.EndsWith(".pqd", StringComparison.OrdinalIgnoreCase))
            {
                // Add channels that are not already defined in the
                // configuration by assuming the meter monitors only one line
                AddUndefinedChannels(meterDataSet);
                FixUpdatedChannelInfo(meterDataSet);
            }
            else
            {
                // Remove data series that were not defined in the configuration
                // since configuration information cannot be added for it
                RemoveUndefinedDataSeries(meterDataSet);
            }

            foreach (DataGroup dataGroup in meterDataSet.GetResource<DataGroupsResource>().DataGroups)
            {
                if (dataGroup.Classification != DataClassification.Event)
                    continue;

                // Add missing current series based on IR = IA + IB + IC
                dataSeries = VIDataGroup.AddMissingCurrentSeries(m_meterInfo, meterDataSet.Meter, dataGroup);

                if ((object)dataSeries != null)
                    meterDataSet.DataSeries.Add(dataSeries);
            }
        }

        public override void Load(DbAdapterContainer dbAdapterContainer)
        {
        }

        private bool TryParseFilePath(string fileName, out string meterKey)
        {
            Match match = Regex.Match(fileName, FilePattern);
            Group meterKeyGroup;

            if (match.Success)
            {
                meterKeyGroup = match.Groups["AssetKey"];

                if ((object)meterKeyGroup != null)
                {
                    meterKey = meterKeyGroup.Value;
                    return true;
                }
            }

            meterKey = null;
            return false;
        }

        private void AddCalculatedDataSeries(MeterDataSet meterDataSet, Series series)
        {
            int sourceIndex;
            List<Tuple<char, int>> sourceIndexes;
            DataSeries dataSeries;

            sourceIndex = 0;

            sourceIndexes = series.SourceIndexes.Split(',')
                .Where(str => int.TryParse(str, out sourceIndex))
                .Select(str => Tuple.Create(str[0], sourceIndex))
                .ToList();

            if (sourceIndexes.Count == 0)
                return;

            if (series.Channel.MeasurementType.Name == "Digital")
            {
                dataSeries = sourceIndexes
                    .Select(tuple => (tuple.Item1 != '-') ? meterDataSet.Digitals[tuple.Item2].Copy() : meterDataSet.Digitals[Math.Abs(tuple.Item2)].Negate())
                    .Aggregate((series1, series2) => series1.Add(series2));
            }
            else
            {
                dataSeries = sourceIndexes
                    .Select(tuple => (tuple.Item1 != '-') ? meterDataSet.DataSeries[tuple.Item2].Copy() : meterDataSet.DataSeries[Math.Abs(tuple.Item2)].Negate())
                    .Aggregate((series1, series2) => series1.Add(series2));
            }

            dataSeries.SeriesInfo = series;

            if (!meterDataSet.DataSeries.Contains(dataSeries))
                meterDataSet.DataSeries.Add(dataSeries);
        }

        private void AddUndefinedChannels(MeterDataSet meterDataSet)
        {
            DataContextLookup<SeriesKey, Series> seriesLookup;
            DataContextLookup<ChannelKey, Channel> channelLookup;
            DataContextLookup<string, MeasurementType> measurementTypeLookup;
            DataContextLookup<string, MeasurementCharacteristic> measurementCharacteristicLookup;
            DataContextLookup<string, SeriesType> seriesTypeLookup;
            DataContextLookup<string, Phase> phaseLookup;

            List<DataSeries> undefinedDataSeries;

            Line line;

            undefinedDataSeries = meterDataSet.DataSeries
                .Where(dataSeries => (object)dataSeries.SeriesInfo.Channel.Line == null)
                .ToList();

            if (undefinedDataSeries.Count <= 0)
                return;

            line = meterDataSet.Meter.MeterLines
                .Select(meterLine => meterLine.Line)
                .Single();

            seriesLookup = new DataContextLookup<SeriesKey, Series>(m_meterInfo, GetSeriesKey)
                .WithFilterExpression(series => series.Channel.MeterID == meterDataSet.Meter.ID)
                .WithFilterExpression(series => series.SourceIndexes == "");

            channelLookup = new DataContextLookup<ChannelKey, Channel>(m_meterInfo, GetChannelKey)
                .WithFilterExpression(channel => channel.MeterID == meterDataSet.Meter.ID);

            measurementTypeLookup = new DataContextLookup<string, MeasurementType>(m_meterInfo, type => type.Name);
            measurementCharacteristicLookup = new DataContextLookup<string, MeasurementCharacteristic>(m_meterInfo, characteristic => characteristic.Name);
            seriesTypeLookup = new DataContextLookup<string, SeriesType>(m_meterInfo, type => type.Name);
            phaseLookup = new DataContextLookup<string, Phase>(m_meterInfo, phase => phase.Name);

            for (int i = 0; i < undefinedDataSeries.Count; i++)
            {
                DataSeries dataSeries = undefinedDataSeries[i];

                // Search for an existing series info object
                dataSeries.SeriesInfo = seriesLookup.GetOrAdd(GetSeriesKey(dataSeries.SeriesInfo), seriesKey =>
                {
                    Series clonedSeries = dataSeries.SeriesInfo.Clone();

                    // Search for an existing series type object to associate with the new series
                    SeriesType seriesType = seriesTypeLookup.GetOrAdd(dataSeries.SeriesInfo.SeriesType.Name, name => dataSeries.SeriesInfo.SeriesType.Clone());

                    // Search for an existing channel object to associate with the new series
                    Channel channel = channelLookup.GetOrAdd(GetChannelKey(dataSeries.SeriesInfo.Channel), channelKey =>
                    {
                        Channel clonedChannel = dataSeries.SeriesInfo.Channel.Clone();

                        // Search for an existing measurement type object to associate with the new channel
                        MeasurementType measurementType = measurementTypeLookup.GetOrAdd(dataSeries.SeriesInfo.Channel.MeasurementType.Name, name => dataSeries.SeriesInfo.Channel.MeasurementType.Clone());

                        // Search for an existing measurement characteristic object to associate with the new channel
                        MeasurementCharacteristic measurementCharacteristic = measurementCharacteristicLookup.GetOrAdd(dataSeries.SeriesInfo.Channel.MeasurementCharacteristic.Name, name => dataSeries.SeriesInfo.Channel.MeasurementCharacteristic.Clone());

                        // Search for an existing phase object to associate with the new channel
                        Phase phase = phaseLookup.GetOrAdd(dataSeries.SeriesInfo.Channel.Phase.Name, name => dataSeries.SeriesInfo.Channel.Phase.Clone());

                        // Assign the foreign keys of the channel
                        // to reference the objects from the lookup
                        clonedChannel.Meter = meterDataSet.Meter;
                        clonedChannel.Line = line;
                        clonedChannel.MeasurementType = measurementType;
                        clonedChannel.MeasurementCharacteristic = measurementCharacteristic;
                        clonedChannel.Phase = phase;
                        clonedChannel.Enabled = 1;

                        return clonedChannel;
                    });

                    // Assign the foreign keys of the series
                    // to reference the objects from the lookup
                    clonedSeries.SeriesType = seriesType;
                    clonedSeries.Channel = channel;

                    return clonedSeries;
                });
            }
        }

        private void FixUpdatedChannelInfo(MeterDataSet meterDataSet)
        {
            HashSet<int> channelLookup = new HashSet<int>(meterDataSet.DataSeries.Select(dataSeries => dataSeries.SeriesInfo.Channel.ID));

            foreach (DataSeries dataSeries in meterDataSet.DataSeries)
            {
                if ((object)dataSeries.SeriesInfo != null && dataSeries.DataPoints.Count > 1)
                    dataSeries.SeriesInfo.Channel.SamplesPerHour = (int)Math.Round((dataSeries.DataPoints.Count - 1) / (dataSeries.Duration / 3600.0D));
            }

            foreach (Channel channel in meterDataSet.Meter.Channels)
                channel.Enabled = channelLookup.Contains(channel.ID) ? 1 : 0;

            m_meterInfo.SubmitChanges();
        }

        private void RemoveUndefinedDataSeries(MeterDataSet meterDataSet)
        {
            for (int i = meterDataSet.DataSeries.Count - 1; i >= 0; i--)
            {
                if ((object)meterDataSet.DataSeries[i].SeriesInfo.Channel.Line == null)
                    meterDataSet.DataSeries.RemoveAt(i);
            }

            for (int i = meterDataSet.Digitals.Count - 1; i >= 0; i--)
            {
                if ((object)meterDataSet.Digitals[i].SeriesInfo.Channel.Line == null)
                    meterDataSet.Digitals.RemoveAt(i);
            }
        }

        #endregion

        #region [ Static ]

        // Static Fields
        private static readonly ILog Log = LogManager.GetLogger(typeof(ConfigurationOperation));

        // Static Methods
        private static ChannelKey GetChannelKey(Channel channel)
        {
            return Tuple.Create(
                channel.HarmonicGroup,
                channel.Name,
                channel.MeasurementType.Name,
                channel.MeasurementCharacteristic.Name,
                channel.Phase.Name);
        }

        private static SeriesKey GetSeriesKey(Series series)
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

        #endregion
    }
}
