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
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using FaultData.Configuration;
using FaultData.DataAnalysis;
using FaultData.DataSets;
using GSF.Collections;
using GSF.Configuration;
using GSF.Data;
using GSF.Data.Model;
using log4net;
using openXDA.Configuration;
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
            public bool ByName;
            public string ChannelName;

            public bool IsRMSTrend;
            public bool IsFLKRTrend;
            public bool IsTriggerTrend;
            public bool IsTrend { get => IsRMSTrend || IsFLKRTrend || IsTriggerTrend; }

            public static SourceIndex Parse(string text)
            {
                string trim = text.Trim();

                if (trim == "NONE")
                    return null;

                if (string.IsNullOrEmpty(trim))
                    return null;

                SourceIndex sourceIndex = new SourceIndex();
                sourceIndex.IsRMSTrend = trim.StartsWith("RMS", StringComparison.OrdinalIgnoreCase);
                sourceIndex.IsFLKRTrend = trim.StartsWith("FLKR", StringComparison.OrdinalIgnoreCase);
                sourceIndex.IsTriggerTrend = trim.StartsWith("TRIGGER", StringComparison.OrdinalIgnoreCase);

                string channelMapping;

                // Deal with APP Trends before anything else since they don't have a valid Name
                if (sourceIndex.IsTrend)
                {
                    // language=regex
                    // The format is RMSAVG(n), RMSMIN(n), RMSMAX(n), FLKR(n) where n is a Channel ID (not a Series ID)
                    const string Pattern = @"(?<Name>[A-Za-z]+)\((?<Channel>[0-9]+)\)";
                    Match match = Regex.Match(trim, Pattern);

                    if (!match.Success)
                    {
                        Log.Debug($"Incorrect format for trend channel name: {trim} doesn't match format RMSAVG(n), RMSMIN(n), RMSMAX(n), FLKR(n).");
                        return null;
                    }

                    channelMapping = match.Groups["Channel"].Value;
                    sourceIndex.ChannelName = match.Groups["Name"].Value;

                    if (!int.TryParse(channelMapping, out sourceIndex.ChannelIndex))
                    {
                        Log.Debug($"Incorrect format for trend channel name: {trim} does not specify a Channel ID.");
                        return null;
                    }

                    return sourceIndex;
                }

                int asterisk = text.IndexOf('*');
                string multiplier = (asterisk >= 0) ? text.Substring(0, asterisk).Trim() : "1";
                channelMapping = (asterisk >= 0) ? text.Substring(asterisk + 1).Trim() : text.Trim();

                if (!double.TryParse(multiplier, out sourceIndex.Multiplier))
                {
                    Log.Debug($"Incorrect format for multiplier {multiplier} found in source index {text}.");
                    return null;
                }

                if (!int.TryParse(channelMapping, out sourceIndex.ChannelIndex))
                {
                    sourceIndex.ByName = true;
                    sourceIndex.ChannelName = channelMapping;
                    sourceIndex.ChannelIndex = -1;
                }
                else if (channelMapping[0] == '-')
                {
                    sourceIndex.Multiplier *= -1.0D;
                    sourceIndex.ChannelIndex *= -1;
                }

                return sourceIndex;
            }
        }

        private enum APPDataType
        {
            PointOnWave,
            RMS,
            Flicker,
            Trigger
        }

        // Constants
        private const double Sqrt3 = 1.7320508075688772935274463415059D;

        #endregion

        #region [ Properties ]

        [Category]
        [SettingName(TrendingDataSection.CategoryName)]
        public TrendingDataSection TrendingDataSettings { get; }
            = new TrendingDataSection();

        #endregion

        #region [ Methods ]

        public override void Execute(MeterDataSet meterDataSet)
        {
            // Grab the parsed meter right away as we will be replacing it in the meter data set with the meter from the database
            Meter parsedMeter = meterDataSet.Meter;

            // Search the database for a meter definition that matches the parsed meter
            Meter dbMeter = LoadMeterFromDatabase(meterDataSet);

            if (dbMeter is null)
            {
                Log.Info(string.Format("No existing meter found matching meter with name {0}.", parsedMeter.Name));

                // If configuration cannot be modified and existing configuration cannot be found for this meter,
                // throw an exception to indicate the operation could not be executed
                throw new InvalidOperationException("Cannot process meter - configuration does not exist");
            }

            if (!meterDataSet.LoadHistoricConfiguration)
                Log.Info(string.Format("Found meter {0} in database.", dbMeter.Name));
            else
                Log.Info(string.Format("Found meter {0} in configuration history.", dbMeter.Name));

            // Replace the parsed meter with
            // the one from the database
            meterDataSet.Meter = dbMeter;

            // Check if it is APP Trending Data
            APPDataType appDataType = new Func<string, APPDataType>(filePath =>
            {
                if (Regex.IsMatch(filePath, TrendingDataSettings.RMS.FolderPath))
                    return APPDataType.RMS;

                if (Regex.IsMatch(filePath, TrendingDataSettings.Flicker.FolderPath))
                    return APPDataType.Flicker;

                if (Regex.IsMatch(filePath, TrendingDataSettings.Trigger.FolderPath))
                    return APPDataType.Trigger;

                return APPDataType.PointOnWave;
            })(meterDataSet.FilePath);

            AddMissingAPPTrendChannels(meterDataSet, appDataType);

            // Create data series which are computed from parsed series
            List<DataSeries> calculatedDataSeriesList = new List<DataSeries>();
            ILookup<Series, SourceIndex> sourceIndexLookup = BuildSourceIndexLookup(meterDataSet, appDataType);

            foreach (Series series in dbMeter.Series)
                AddCalculatedDataSeries(calculatedDataSeriesList, meterDataSet, series, sourceIndexLookup);

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
                if (meterDataSet.DataSeries[i].SeriesInfo is null)
                    meterDataSet.DataSeries.RemoveAt(i);
            }

            for (int i = meterDataSet.Digitals.Count - 1; i >= 0; i--)
            {
                if (meterDataSet.Digitals[i].SeriesInfo is null)
                    meterDataSet.Digitals.RemoveAt(i);
            }

            // Remove data series that were not defined
            // in the configuration or the source data
            RemoveUnknownChannelTypes(meterDataSet);

            // Only update database configuration if NOT using historic configuration.
            if (!meterDataSet.LoadHistoricConfiguration)
            {
                // If there are no explicitly mapped channels in the meter's configuration,
                // automatically adjust meter configuration to include newly introduced channels
                if (calculatedDataSeriesList.Count == 0)
                    AddUndefinedChannels(meterDataSet);

                // If any channels could not be mapped or added to the database, remove them now
                RemoveUndefinedChannels(meterDataSet);

                FixUpdatedChannelInfo(meterDataSet, parsedMeter);
            }

            ApplySeriesAdjustments(meterDataSet.DataSeries);
        }

        private Meter LoadMeterFromDatabase(MeterDataSet meterDataSet)
        {
            using (AdoDataConnection connection = meterDataSet.CreateDbConnection())
            {
                Meter LoadCurrentMeter()
                {
                    TableOperations<Meter> meterTable = new TableOperations<Meter>(connection);
                    Meter parsedMeter = meterDataSet.Meter;
                    Meter dbMeter = meterTable.QueryRecordWhere("AssetKey = {0}", parsedMeter.AssetKey);
                    dbMeter.ConnectionFactory = meterDataSet.CreateDbConnection;
                    return dbMeter;
                }

                Meter LoadHistoricMeter()
                {
                    const string ConfigKey = "openXDA";

                    TableOperations<MeterConfiguration> meterConfigurationTable = new TableOperations<MeterConfiguration>(connection);

                    RecordRestriction recordRestriction =
                        new RecordRestriction("ConfigKey = {0}", ConfigKey) &
                        new RecordRestriction("{0} IN (SELECT FileGroupID FROM FileGroupMeterConfiguration WHERE MeterConfigurationID = MeterConfiguration.ID)", meterDataSet.FileGroup.ID);

                    MeterConfiguration meterConfiguration = meterConfigurationTable.QueryRecord("ID DESC", recordRestriction);

                    if (meterConfiguration == null)
                    {
                        // Need to find the oldest configuration record for this meter
                        Meter dbMeter = LoadCurrentMeter();
                        int? meterID = dbMeter?.ID;

                        recordRestriction =
                            new RecordRestriction("MeterID = {0}", meterID) &
                            new RecordRestriction("ConfigKey = {0}", ConfigKey) &
                            new RecordRestriction("ID NOT IN (SELECT DiffID FROM MeterConfiguration WHERE DiffID IS NOT NULL)");

                        meterConfiguration = meterConfigurationTable.QueryRecord("ID", recordRestriction);
                    }

                    if (meterConfiguration == null)
                        return null;

                    MeterSettingsSheet settingsSheet = new MeterSettingsSheet(meterConfigurationTable, meterConfiguration);
                    return settingsSheet.Meter;
                }

                Log.Info("Locating meter in database...");

                if (meterDataSet.LoadHistoricConfiguration)
                    return LoadHistoricMeter();

                return LoadCurrentMeter();
            }
        }

        private Func<SourceIndex, bool> GetSourceIndexFilter(APPDataType appDataType)
        {
            switch (appDataType)
            {
                default:
                case APPDataType.PointOnWave:
                    return sourceIndex => !sourceIndex.IsTrend;

                case APPDataType.RMS:
                    return sourceIndex => sourceIndex.IsRMSTrend;

                case APPDataType.Flicker:
                    return sourceIndex => sourceIndex.IsFLKRTrend;

                case APPDataType.Trigger:
                    return sourceIndex => sourceIndex.IsTriggerTrend;
            }
        }

        private void AddMissingAPPTrendChannels(MeterDataSet meterDataSet, APPDataType appDataType)
        {
            if (appDataType == APPDataType.PointOnWave)
                return;

            Func<SourceIndex, bool> sourceIndexFilter = GetSourceIndexFilter(appDataType);

            Func<Channel, bool> channelFilter = new Func<APPDataType, Func<Channel, bool>>(dataType =>
            {
                switch (dataType)
                {
                    case APPDataType.RMS:
                    case APPDataType.Flicker:
                        return channel =>
                            (channel.MeasurementType.Name == "Voltage" || channel.MeasurementType.Name == "Current") &&
                            channel.MeasurementCharacteristic.Name == "Instantaneous";

                    case APPDataType.Trigger:
                        string[] measurementCharacteristics =
                        {
                            "Trigger - RMS",
                            "Trigger - Impulse",
                            "Trigger - THD",
                            "Trigger - Ubal",
                            "Trigger - I"
                        };

                        return channel =>
                            channel.MeasurementType.Name == "Digital" &&
                            measurementCharacteristics.Contains(channel.MeasurementCharacteristic.Name);

                    default:
                        return channel => false;
                }
            })(appDataType);

            Action<Channel, MeterDataSet> addTrendChannel = new Func<APPDataType, Action<Channel, MeterDataSet>>(dataType =>
            {
                switch (dataType)
                {
                    case APPDataType.RMS: return AddRMSChannel;
                    case APPDataType.Flicker: return AddFlkrChannel;
                    case APPDataType.Trigger: return AddTriggerChannel;
                    default: return new Action<Channel, MeterDataSet>((_, __) => { });
                }
            })(appDataType);

            Meter meter = meterDataSet.Meter;

            IEnumerable<SourceIndex> sourceIndexes = meter.Series
                .SelectMany(series => series.SourceIndexes.Split(','))
                .Select(sourceIndex => SourceIndex.Parse(sourceIndex))
                .Where(sourceIndex => !(sourceIndex is null))
                .Where(sourceIndex => sourceIndex.ChannelIndex >= 0)
                .Where(sourceIndexFilter);

            IEnumerable<Channel> unmappedChannels = meter.Channels
                .Where(channelFilter)
                .GroupJoin(sourceIndexes, channel => channel.ID, sourceIndex => sourceIndex.ChannelIndex, (Channel, SourceIndexes) => new { Channel, SourceIndexes })
                .Where(item => !item.SourceIndexes.Any())
                .Select(item => item.Channel);

            bool refreshChannels = false;

            foreach (Channel channel in unmappedChannels)
            {
                addTrendChannel(channel, meterDataSet);
                refreshChannels = true;
            }

            if (refreshChannels)
                meter.Channels = null;
        }

        private ILookup<Series, SourceIndex> BuildSourceIndexLookup(MeterDataSet meterDataSet, APPDataType appDataType)
        {
            List<Series> seriesList = meterDataSet.Meter.Series;

            var sourceIndexMap = seriesList
                .SelectMany(series => series.SourceIndexes.Split(','), (Series, SourceIndex) => new { Series, SourceIndex })
                .Select(item => new { item.Series, SourceIndex = SourceIndex.Parse(item.SourceIndex) })
                .Where(item => !(item.SourceIndex is null))
                .ToList();

            ILookup<int, SourceIndex> sourceIndexLookup = sourceIndexMap
                .ToLookup(item => item.Series.Channel.ID, item => item.SourceIndex);

            IEnumerable<SourceIndex> Translate(SourceIndex sourceIndex)
            {
                if (!sourceIndex.IsTrend)
                    return Enumerable.Repeat(sourceIndex, 1);

                IEnumerable<SourceIndex> powIndexes = sourceIndexLookup[sourceIndex.ChannelIndex];
                bool isRMS = sourceIndex.ChannelName.StartsWith("RMS", StringComparison.OrdinalIgnoreCase);
                bool isTrigger = !isRMS && sourceIndex.ChannelName.StartsWith("TRIGGER", StringComparison.OrdinalIgnoreCase);
                bool isMax = sourceIndex.ChannelName.EndsWith("MAX", StringComparison.OrdinalIgnoreCase);
                bool isMin = !isMax && sourceIndex.ChannelName.EndsWith("MIN", StringComparison.OrdinalIgnoreCase);

                string GetIndexPrefix()
                {
                    if (isRMS)
                        return "A";
                    if (isTrigger)
                        return "T";
                    return "";
                }

                string GetAggregatePrefix(double multiplier)
                {
                    if (!isRMS && !isTrigger)
                        return "";
                    if (isMax || (isMin && multiplier < 0.0D))
                        return "Max_";
                    if (isMin || (isMax && multiplier < 0.0D))
                        return "Min_";
                    return "Avg_";
                }

                return powIndexes.Select(powIndex =>
                {
                    double multiplier = powIndex.Multiplier;
                    string channelName = powIndex.ChannelName;

                    if (!powIndex.ByName)
                    {
                        string indexPrefix = GetIndexPrefix();
                        channelName = $"{indexPrefix}{powIndex.ChannelIndex}";
                    }

                    string aggregatePrefix = GetAggregatePrefix(multiplier);
                    string translated = $"{multiplier}*{aggregatePrefix}{channelName}";

                    // RMS and Trigger channels are always analogs
                    SourceIndex newIndex = SourceIndex.Parse(translated);
                    newIndex.IsRMSTrend = sourceIndex.IsRMSTrend;
                    newIndex.IsFLKRTrend = sourceIndex.IsFLKRTrend;
                    newIndex.IsTriggerTrend = sourceIndex.IsTriggerTrend;
                    return newIndex;
                });
            }

            Func<SourceIndex, bool> sourceIndexFilter = GetSourceIndexFilter(appDataType);

            return sourceIndexMap
                .SelectMany(item => Translate(item.SourceIndex), (item, SourceIndex) => new { item.Series, SourceIndex })
                .Where(item => !(item.SourceIndex is null))
                .Where(item => sourceIndexFilter(item.SourceIndex))
                .ToLookup(item => item.Series, item => item.SourceIndex);
        }

        private void AddCalculatedDataSeries(List<DataSeries> calculatedDataSeriesList, MeterDataSet meterDataSet, Series series, ILookup<Series, SourceIndex> sourceIndexLookup)
        {
            IEnumerable<SourceIndex> sourceIndexes = sourceIndexLookup[series];

            if (!sourceIndexes.Any())
                return;

            List<DataSeries> dataSeriesList = (series.Channel.MeasurementType.Name != "Digital")
                ? meterDataSet.DataSeries
                : meterDataSet.Digitals;

            if (sourceIndexes.Any(sourceIndex => sourceIndex.ChannelIndex >= dataSeriesList.Count))
                return;

            ILookup<string, int> nameIndexLookup = dataSeriesList
                .Select((DataSeries, Index) => new { DataSeries, Index })
                .Where(item => !(item.DataSeries.SeriesInfo is null))
                .ToLookup(item => item.DataSeries.SeriesInfo.Channel.Name, item => item.Index);

            int MatchCount(DataSeries dataSeries)
            {
                int count = 0;
                if (dataSeries.SeriesInfo?.SeriesType.Name == series.SeriesType.Name)
                    count++;
                if (dataSeries.SeriesInfo?.Channel.MeasurementType.Name == series.Channel.MeasurementType.Name)
                    count++;
                if (dataSeries.SeriesInfo?.Channel.MeasurementCharacteristic.Name == series.Channel.MeasurementCharacteristic.Name)
                    count++;
                if (dataSeries.SeriesInfo?.Channel.Phase.Name == series.Channel.Phase.Name)
                    count++;
                if (dataSeries.SeriesInfo?.Channel.HarmonicGroup == series.Channel.HarmonicGroup)
                    count++;
                return count;
            }

            DataSeries FindDataSeriesByName(SourceIndex sourceIndex) => nameIndexLookup[sourceIndex.ChannelName]
                .Select(index => dataSeriesList[index])
                .OrderByDescending(dataSeries => MatchCount(dataSeries))
                .FirstOrDefault();

            DataSeries FindDataSeries(SourceIndex sourceIndex) => sourceIndex.ByName
                ? FindDataSeriesByName(sourceIndex)
                : dataSeriesList[sourceIndex.ChannelIndex];

            IEnumerable<DataSeries> seriesParts = sourceIndexes
                .Where(sourceIndex => sourceIndex.ByName || sourceIndex.ChannelIndex >= 0)
                .Select(sourceIndex => new { sourceIndex.Multiplier, DataSeries = FindDataSeries(sourceIndex) })
                .Where(obj => !(obj.DataSeries is null))
                .Select(obj => obj.DataSeries.Multiply(obj.Multiplier));

            if (!seriesParts.Any())
                return;

            DataSeries calculatedSeries = seriesParts
                .Aggregate((series1, series2) => series1.Add(series2));
              
            calculatedSeries.SeriesInfo = series;
            calculatedDataSeriesList.Add(calculatedSeries);
        }

        private void ApplySeriesAdjustments(List<DataSeries> definedSeries)
        {
            for (int i = 0; i < definedSeries.Count; i++)
            {
                DataSeries series = definedSeries[i];
                double adder = series.SeriesInfo.Channel.Adder;
                double multiplier = series.SeriesInfo.Channel.Multiplier;

                DataSeries adderSeries = series.Copy();

                foreach (DataPoint point in adderSeries.DataPoints)
                    point.Value = adder;

                definedSeries[i] = series
                    .Multiply(multiplier)
                    .Add(adderSeries);
                definedSeries[i].SeriesInfo = series.SeriesInfo;
            }
        }

        private void AddUndefinedChannels(MeterDataSet meterDataSet)
        {
            List<DataSeries> undefinedDataSeries = meterDataSet.DataSeries
                .Concat(meterDataSet.Digitals)
                .Where(dataSeries => dataSeries.SeriesInfo.Channel.Asset is null)
                .ToList();

            if (undefinedDataSeries.Count <= 0)
                return;

            Meter meter = meterDataSet.Meter;

            if (meter.MeterAssets.Count == 0)
            {
                Log.Warn($"Unable to automatically add channels to meter {meterDataSet.Meter.Name} because there are no lines associated with that meter.");
                return;
            }

            if (meter.MeterAssets.Count > 1)
            {
                Log.Warn($"Add channels to meter {meterDataSet.Meter.Name} Asset {meter.MeterAssets.First().Asset.AssetKey}. There are more than 2 assets associated with that meter.");
            }

            Asset asset = meter.MeterAssets
                .Select(meterLine => meterLine.Asset)
                .First();

            foreach (DataSeries series in undefinedDataSeries)
                series.SeriesInfo.Channel.AssetID = asset.ID;

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
                    channel.AssetID = asset.ID;
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
                                channel.PerUnitValue = (asset.VoltageKV * 1000.0D) / Sqrt3;
                            else if (IsLineToLine(channel))
                                channel.PerUnitValue = asset.VoltageKV * 1000.0D;
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

                Dictionary<SeriesKey, Series> seriesLookup = meter.Series
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

                    seriesLookup = meter.Series
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

        private void FixUpdatedChannelInfo(MeterDataSet meterDataSet, Meter parsedMeter)
        {
            using (AdoDataConnection connection = meterDataSet.CreateDbConnection())
            {
                TableOperations<Channel> channelTable = new TableOperations<Channel>(connection);

                List<DataSeries> allDataSeries = meterDataSet.DataSeries
                    .Concat(meterDataSet.Digitals)
                    .ToList();

                foreach (DataSeries dataSeries in allDataSeries)
                {
                    if ((object)dataSeries.SeriesInfo != null && dataSeries.DataPoints.Count > 1)
                    {
                        double samplesPerHour = CalculateSamplesPerHour(dataSeries);

                        if (samplesPerHour <= 60.0D)
                        {
                            Channel channel = dataSeries.SeriesInfo.Channel;
                            if (channel.SamplesPerHour == 0)
                            {
                                channel.SamplesPerHour = samplesPerHour;
                                channelTable.UpdateRecord(channel);
                            }
                        }
                    }
                }

                if (parsedMeter.Channels is null)
                    return;

                IEnumerable<ChannelKey> parsedChannelKeys = parsedMeter.Channels
                    .Concat(allDataSeries.Select(dataSeries => dataSeries.SeriesInfo.Channel))
                    .Where(channel => (object)channel.Asset != null)
                    .Select(channel => new ChannelKey(channel));

                HashSet<ChannelKey> parsedChannelLookup = new HashSet<ChannelKey>(parsedChannelKeys);

                foreach (Channel channel in meterDataSet.Meter.Channels)
                {
                    bool enabled = parsedChannelLookup.Contains(new ChannelKey(channel));

                    if (enabled == channel.Enabled)
                        continue;

                    channel.Enabled = enabled;
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

        private void RemoveUndefinedChannels(MeterDataSet meterDataSet)
        {
            for (int i = meterDataSet.DataSeries.Count - 1; i >= 0; i--)
            {
                if (meterDataSet.DataSeries[i].SeriesInfo.Channel.Asset is null)
                    meterDataSet.DataSeries.RemoveAt(i);
            }

            for (int i = meterDataSet.Digitals.Count - 1; i >= 0; i--)
            {
                if (meterDataSet.Digitals[i].SeriesInfo.Channel.Asset is null)
                    meterDataSet.Digitals.RemoveAt(i);
            }
        }

        /// <summary>
        /// Checks if the RMS Trend Channels exist and adds them if necesarry.
        /// </summary>
        /// <param name="powChannel">The point on Wave Data Channel.</param>
        /// <param name="rmsIndices">Source Indices of the existing RMS Trend Channels.</param>
        /// <param name="meterDataSet"><see cref="MeterDataSet"/></param>
        private void AddRMSChannel(Channel powChannel, MeterDataSet meterDataSet)
        {
            //Create new RMS Channel and Series
            Channel rmsChannel = new Channel()
            {
                MeterID = powChannel.MeterID,
                AssetID = powChannel.AssetID,
                MeasurementTypeID = powChannel.MeasurementTypeID,
                MeasurementCharacteristicID = powChannel.MeasurementCharacteristicID,
                PhaseID = powChannel.PhaseID,
                Name = powChannel.Name,
                SamplesPerHour = powChannel.SamplesPerHour,
                PerUnitValue = powChannel.PerUnitValue,
                HarmonicGroup = powChannel.HarmonicGroup,
                Description = powChannel.Description,
                Enabled = powChannel.Enabled,
                MeasurementType = powChannel.MeasurementType,
                Phase = powChannel.Phase
            };

            using (AdoDataConnection connection = meterDataSet.CreateDbConnection())
            {
                TableOperations<MeasurementCharacteristic> measurementCharacteristicTable = new TableOperations<MeasurementCharacteristic>(connection);
                TableOperations<SeriesType> seriesTypeTable = new TableOperations<SeriesType>(connection);
                TableOperations<Channel> channelTable = new TableOperations<Channel>(connection);
                TableOperations<Series> seriesTable = new TableOperations<Series>(connection);

                rmsChannel.MeasurementCharacteristic = measurementCharacteristicTable.QueryRecordWhere("Name = 'RMS'");
                rmsChannel.MeasurementCharacteristicID = rmsChannel.MeasurementCharacteristic.ID;

                ChannelKey key = new ChannelKey(rmsChannel);
                Channel dbChannel = key.Find(connection, meterDataSet.Meter.ID);

                if (dbChannel is null)
                {
                    channelTable.AddNewRecord(rmsChannel);
                    dbChannel = key.Find(connection, meterDataSet.Meter.ID);
                }

                //Create all 3 Series for this Channel
                Series avgSeries = new Series()
                {
                    ChannelID = dbChannel.ID,
                    SeriesTypeID = seriesTypeTable.QueryRecordWhere("Name = 'Average'").ID,
                    SourceIndexes = "RMSAVG(" + powChannel.ID + ")"
                };
                Series minSeries = new Series()
                {
                    ChannelID = dbChannel.ID,
                    SeriesTypeID = seriesTypeTable.QueryRecordWhere("Name = 'Minimum'").ID,
                    SourceIndexes = "RMSMIN(" + powChannel.ID + ")"
                };
                Series maxSeries = new Series()
                {
                    ChannelID = dbChannel.ID,
                    SeriesTypeID = seriesTypeTable.QueryRecordWhere("Name = 'Maximum'").ID,
                    SourceIndexes = "RMSMAX(" + powChannel.ID + ")"
                };

                seriesTable.AddNewRecord(avgSeries);
                seriesTable.AddNewRecord(minSeries);
                seriesTable.AddNewRecord(maxSeries);
            }
        }

        /// <summary>
        /// Checks if the Flicker Trend Channels exist and adds them if necesarry.
        /// </summary>
        /// <param name="powChannel">The point on Wave Data Channel.</param>
        /// <param name="flkrIndices">Source Indices of the existing Flicker Trend Channels.</param>
        /// <param name="meterDataSet"><see cref="MeterDataSet"/></param>
        private void AddFlkrChannel(Channel powChannel, MeterDataSet meterDataSet)
        {
            //Create new RMS Channel and Series
            Channel flkrChannel = new Channel()
            {
                MeterID = powChannel.MeterID,
                AssetID = powChannel.AssetID,
                MeasurementTypeID = powChannel.MeasurementTypeID,
                MeasurementCharacteristicID = powChannel.MeasurementCharacteristicID,
                PhaseID = powChannel.PhaseID,
                Name = powChannel.Name + "Flicker",
                SamplesPerHour = powChannel.SamplesPerHour,
                PerUnitValue = powChannel.PerUnitValue,
                HarmonicGroup = powChannel.HarmonicGroup,
                Description = powChannel.Description,
                Enabled = powChannel.Enabled,
                MeasurementType = powChannel.MeasurementType,
                Phase = powChannel.Phase
            };

            using (AdoDataConnection connection = meterDataSet.CreateDbConnection())
            {
                TableOperations<MeasurementCharacteristic> measurementCharacteristicTable = new TableOperations<MeasurementCharacteristic>(connection);
                TableOperations<SeriesType> seriesTypeTable = new TableOperations<SeriesType>(connection);
                TableOperations<Channel> channelTable = new TableOperations<Channel>(connection);
                TableOperations<Series> seriesTable = new TableOperations<Series>(connection);

                flkrChannel.MeasurementCharacteristic = measurementCharacteristicTable.QueryRecordWhere("Name = 'FlkrPST'");
                flkrChannel.MeasurementCharacteristicID = flkrChannel.MeasurementCharacteristic.ID;

                ChannelKey key = new ChannelKey(flkrChannel);
                Channel dbChannel = key.Find(connection, meterDataSet.Meter.ID);

                if (dbChannel is null)
                {
                    channelTable.AddNewRecord(flkrChannel);
                    dbChannel = key.Find(connection, meterDataSet.Meter.ID);
                }

                //Create Series for this Channel
                Series avgSeries = new Series()
                {
                    ChannelID = dbChannel.ID,
                    SeriesTypeID = seriesTypeTable.QueryRecordWhere("Name = 'Values'").ID,
                    SourceIndexes = "FLKR(" + powChannel.ID + ")"
                };

                seriesTable.AddNewRecord(avgSeries);
            }
        }

        /// <summary>
        /// Checks if the Trigger Trend Channels exist and adds them if necesarry.
        /// </summary>
        /// <param name="powChannel">The point on Wave Data Channel.</param>
        /// <param name="triggerIndices">Source Indices of the existing Trigger Trend Channels.</param>
        /// <param name="meterDataSet"><see cref="MeterDataSet"/></param>
        private void AddTriggerChannel(Channel powChannel, MeterDataSet meterDataSet)
        {
            //Create new RMS Channel and Series
            Channel trendChannel = new Channel()
            {
                MeterID = powChannel.MeterID,
                AssetID = powChannel.AssetID,
                MeasurementTypeID = powChannel.MeasurementTypeID,
                MeasurementCharacteristicID = powChannel.MeasurementCharacteristicID,
                PhaseID = powChannel.PhaseID,
                Name = powChannel.Name,
                SamplesPerHour = powChannel.SamplesPerHour,
                PerUnitValue = powChannel.PerUnitValue,
                HarmonicGroup = powChannel.HarmonicGroup,
                Description = powChannel.Description,
                Enabled = powChannel.Enabled,
                MeasurementType = powChannel.MeasurementType,
                Phase = powChannel.Phase
            };

            using (AdoDataConnection connection = meterDataSet.CreateDbConnection())
            {
                TableOperations<MeasurementCharacteristic> measurementCharacteristicTable = new TableOperations<MeasurementCharacteristic>(connection);
                TableOperations<MeasurementType> measurementTypeTable = new TableOperations<MeasurementType>(connection);
                TableOperations<SeriesType> seriesTypeTable = new TableOperations<SeriesType>(connection);
                TableOperations<Channel> channelTable = new TableOperations<Channel>(connection);
                TableOperations<Series> seriesTable = new TableOperations<Series>(connection);

                trendChannel.MeasurementType = measurementTypeTable.QueryRecordWhere("ID = {0}", trendChannel.MeasurementTypeID);
                trendChannel.MeasurementCharacteristic = measurementCharacteristicTable.QueryRecordWhere("ID = {0}", trendChannel.MeasurementCharacteristicID);

                if (trendChannel.MeasurementCharacteristic.Name == "Trigger - RMS" || trendChannel.MeasurementCharacteristic.Name == "Trigger - Impulse")
                {
                    trendChannel.MeasurementType = measurementTypeTable.QueryRecordWhere("Name = 'Voltage'");
                    trendChannel.MeasurementTypeID = trendChannel.MeasurementType.ID;
                }
                else if (trendChannel.MeasurementCharacteristic.Name == "Trigger - I")
                {
                    trendChannel.MeasurementType = measurementTypeTable.QueryRecordWhere("Name = 'Current'");
                    trendChannel.MeasurementTypeID = trendChannel.MeasurementType.ID;
                }
                else
                {
                    trendChannel.MeasurementType = measurementTypeTable.QueryRecordWhere("Name = 'Analog'");
                    trendChannel.MeasurementTypeID = trendChannel.MeasurementType.ID;
                }

                string measCharacteristic = "";

                switch (trendChannel.MeasurementCharacteristic.Name)
                {
                    case ("Trigger - RMS"):
                        measCharacteristic = "RMS";
                        break;
                    case ("Trigger - Impulse"):
                        measCharacteristic = "Instantaneous";
                        break;
                    case ("Trigger - THD"):
                        measCharacteristic = "TotalTHD";
                        break;
                    case ("Trigger - Ubal"):
                        measCharacteristic = "Instantaneous";
                        break;
                    case ("Trigger - I"):
                        measCharacteristic = "Instantaneous";
                        break;
                }

                trendChannel.MeasurementCharacteristic = measurementCharacteristicTable.QueryRecordWhere("Name ={0}", measCharacteristic);
                trendChannel.MeasurementCharacteristicID = trendChannel.MeasurementCharacteristic.ID;

                ChannelKey key = new ChannelKey(trendChannel);
                Channel dbChannel = key.Find(connection, meterDataSet.Meter.ID);

                if (dbChannel is null)
                {
                    channelTable.AddNewRecord(trendChannel);
                    dbChannel = key.Find(connection, meterDataSet.Meter.ID);
                }

                //Create all 3 Series for this Channel
                Series avgSeries = new Series()
                {
                    ChannelID = dbChannel.ID,
                    SeriesTypeID = seriesTypeTable.QueryRecordWhere("Name = 'Average'").ID,
                    SourceIndexes = "TRIGGERAVG(" + powChannel.ID + ")"
                };
                Series minSeries = new Series()
                {
                    ChannelID = dbChannel.ID,
                    SeriesTypeID = seriesTypeTable.QueryRecordWhere("Name = 'Minimum'").ID,
                    SourceIndexes = "TRIGGERMIN(" + powChannel.ID + ")"
                };
                Series maxSeries = new Series()
                {
                    ChannelID = dbChannel.ID,
                    SeriesTypeID = seriesTypeTable.QueryRecordWhere("Name = 'Maximum'").ID,
                    SourceIndexes = "TRIGGERMAX(" + powChannel.ID + ")"
                };

                seriesTable.AddNewRecord(avgSeries);
                seriesTable.AddNewRecord(minSeries);
                seriesTable.AddNewRecord(maxSeries);
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

        public static double CalculateSamplesPerHour(DataSeries dataSeries)
        {
            double[] commonSampleRates =
            {
                0.5, 1, 2, 3, 4, 6, 10, 12, 15, 20, 30, 60
            };

            double samplesPerHour = (dataSeries.DataPoints.Count - 1) / (dataSeries.Duration / 3600.0D);

            if (samplesPerHour >= 61.0D)
                return samplesPerHour;

            double nearestCommonRate = commonSampleRates.MinBy(rate => Math.Abs(samplesPerHour - rate));
            return nearestCommonRate;
        }

        #endregion
    }
}
