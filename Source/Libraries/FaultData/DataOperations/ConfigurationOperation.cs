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
using System.Data.Linq;
using System.Linq;
using System.Text.RegularExpressions;
using FaultData.DataAnalysis;
using FaultData.Database;
using FaultData.DataSets;
using GSF.Collections;
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
            MeterFileGroup meterFileGroup;
            string meterKey;

            List<Series> seriesList;
            Dictionary<SeriesKey, Series> seriesLookup;
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

                foreach (DataSeries dataSeries in meterDataSet.DataSeries)
                {
                    if ((object)dataSeries.SeriesInfo == null)
                        continue;

                    if (seriesLookup.TryGetValue(GetSeriesKey(dataSeries.SeriesInfo), out seriesInfo))
                        dataSeries.SeriesInfo = seriesInfo;
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

        private void AddUndefinedChannels(MeterDataSet meterDataSet)
        {
            Lazy<Dictionary<SeriesKey, Series>> seriesLookup;
            Lazy<Dictionary<ChannelKey, Channel>> channelLookup;
            Lazy<Dictionary<string, MeasurementType>> measurementTypeLookup;
            Lazy<Dictionary<string, MeasurementCharacteristic>> measurementCharacteristicLookup;
            Lazy<Dictionary<string, SeriesType>> seriesTypeLookup;
            Lazy<Dictionary<string, Phase>> phaseLookup;

            List<DataSeries> undefinedDataSeries;

            Line line;

            undefinedDataSeries = meterDataSet.DataSeries
                .Where(dataSeries => (object)dataSeries.SeriesInfo.Channel.Line == null)
                .ToList();

            if (undefinedDataSeries.Count <= 0)
                return;

            seriesLookup = new Lazy<Dictionary<SeriesKey, Series>>(() => m_meterInfo.Series.Where(series => series.Channel.MeterID == meterDataSet.Meter.ID).Where(series => series.SourceIndexes == "").ToDictionary(GetSeriesKey));
            channelLookup = new Lazy<Dictionary<ChannelKey, Channel>>(() => m_meterInfo.Channels.Where(channel => channel.MeterID == meterDataSet.Meter.ID).ToDictionary(GetChannelKey));
            measurementTypeLookup = new Lazy<Dictionary<string, MeasurementType>>(() => m_meterInfo.MeasurementTypes.ToDictionary(type => type.Name));
            measurementCharacteristicLookup = new Lazy<Dictionary<string, MeasurementCharacteristic>>(() => m_meterInfo.MeasurementCharacteristics.ToDictionary(type => type.Name));
            seriesTypeLookup = new Lazy<Dictionary<string, SeriesType>>(() => m_meterInfo.SeriesTypes.ToDictionary(type => type.Name));
            phaseLookup = new Lazy<Dictionary<string, Phase>>(() => m_meterInfo.Phases.ToDictionary(type => type.Name));

            line = meterDataSet.Meter.MeterLines
                .Select(meterLine => meterLine.Line)
                .Single();

            lock (AddUndefinedChannelsLock)
            {
                for (int i = 0; i < undefinedDataSeries.Count; i++)
                {
                    DataSeries dataSeries = undefinedDataSeries[i];

                    // Search for an existing series info object
                    dataSeries.SeriesInfo = seriesLookup.Value.GetOrAdd(GetSeriesKey(dataSeries.SeriesInfo), key => dataSeries.SeriesInfo);

                    // If an existing series info object was found,
                    // we don't need to do anything else for this data series
                    if ((object)dataSeries.SeriesInfo.Channel.Line != null)
                        continue;

                    // Search for an existing series type object
                    dataSeries.SeriesInfo.SeriesType = seriesTypeLookup.Value.GetOrAdd(dataSeries.SeriesInfo.SeriesType.Name, name => dataSeries.SeriesInfo.SeriesType);

                    // Search for an existing channel object
                    dataSeries.SeriesInfo.Channel = channelLookup.Value.GetOrAdd(GetChannelKey(dataSeries.SeriesInfo.Channel), key => dataSeries.SeriesInfo.Channel);

                    // If an existing channel object was found,
                    // we don't need to do anything else for this data series
                    if ((object)dataSeries.SeriesInfo.Channel.Line != null)
                        continue;

                    // Search for an existing measurement type object
                    dataSeries.SeriesInfo.Channel.MeasurementType = measurementTypeLookup.Value.GetOrAdd(dataSeries.SeriesInfo.Channel.MeasurementType.Name, name => dataSeries.SeriesInfo.Channel.MeasurementType);

                    // Search for an existing measurement characteristic object
                    dataSeries.SeriesInfo.Channel.MeasurementCharacteristic = measurementCharacteristicLookup.Value.GetOrAdd(dataSeries.SeriesInfo.Channel.MeasurementCharacteristic.Name, name => dataSeries.SeriesInfo.Channel.MeasurementCharacteristic);

                    // Search for an existing phase object
                    dataSeries.SeriesInfo.Channel.Phase = phaseLookup.Value.GetOrAdd(dataSeries.SeriesInfo.Channel.Phase.Name, name => dataSeries.SeriesInfo.Channel.Phase);

                    // Set the meter and line of the new channel
                    dataSeries.SeriesInfo.Channel.Meter = meterDataSet.Meter;
                    dataSeries.SeriesInfo.Channel.Line = line;
                }

                m_meterInfo.SubmitChanges();
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

        private ChannelKey GetChannelKey(Channel channel)
        {
            return Tuple.Create(
                channel.HarmonicGroup,
                channel.Name,
                channel.MeasurementType.Name,
                channel.MeasurementCharacteristic.Name,
                channel.Phase.Name);
        }

        private SeriesKey GetSeriesKey(Series series)
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

        #region [ Static ]

        // Static Fields
        private static readonly object AddUndefinedChannelsLock = new object();
        private static readonly ILog Log = LogManager.GetLogger(typeof(ConfigurationOperation));

        #endregion
    }
}
