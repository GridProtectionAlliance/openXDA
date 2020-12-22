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
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using FaultData.Configuration;
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
            public bool ByName;
            public string ChannelName;

            public bool IsRMSTrend;
            public bool IsFLKRTrend;
            public bool IsTriggerTrend;
            public bool IsTrend { get => IsRMSTrend || IsFLKRTrend || IsTriggerTrend; }


            public static SourceIndex Parse(string text, List<string> channelNames = null)
             {
                if (channelNames == null)
                    channelNames = new List<string>();

                SourceIndex sourceIndex = new SourceIndex();
                sourceIndex.ByName = false;
                sourceIndex.IsRMSTrend = false;
                sourceIndex.IsFLKRTrend = false;
                sourceIndex.IsTriggerTrend = false;

                string channelIndex;

                // Deal with RMS Trends before anythin else since they don't have a valid Name
                if (text.Trim().StartsWith("RMS", StringComparison.OrdinalIgnoreCase) || text.Trim().StartsWith("FLKR", StringComparison.OrdinalIgnoreCase) || text.Trim().StartsWith("TRIGGER", StringComparison.OrdinalIgnoreCase))
                {
                    // The format is RMSAVG(n), RMSMIN(n), RMSMAX(n), FLKR(n) Where n is a Channel ID (not a Series ID)

                    if (text.Trim().StartsWith("RMS", StringComparison.OrdinalIgnoreCase))
                        sourceIndex.IsRMSTrend = true;
                    
                    if (text.Trim().StartsWith("FLKR", StringComparison.OrdinalIgnoreCase))
                        sourceIndex.IsFLKRTrend = true;

                    if (text.Trim().StartsWith("TRIGGER", StringComparison.OrdinalIgnoreCase))
                        sourceIndex.IsTriggerTrend = true;

                    channelIndex = text.Trim().Substring(text.Trim().IndexOf("(") + 1, text.Trim().IndexOf(")") - text.Trim().IndexOf("(") - 1);
                    sourceIndex.ChannelName = text.Trim().Substring(0,text.Trim().IndexOf("("));

                    if (!int.TryParse(channelIndex, out sourceIndex.ChannelIndex))
                    {
                        throw new FormatException($"Incorrect format for channel name {channelIndex} not found.");
                    }
                    return sourceIndex;
                }
                    

                string[] parts = text.Split('*');
                string multiplier = (parts.Length > 1) ? parts[0].Trim() : "1";
                channelIndex = (parts.Length > 1) ? parts[1].Trim() : parts[0].Trim();

                if (parts.Length > 2)
                    throw new FormatException($"Too many asterisks found in source index {text}.");

                if (!double.TryParse(multiplier, out sourceIndex.Multiplier))
                    throw new FormatException($"Incorrect format for multiplier {multiplier} found in source index {text}.");

                if (channelIndex == "NONE")
                    return null;
                if (string.IsNullOrEmpty(channelIndex))
                    return null;

                if (!int.TryParse(channelIndex, out sourceIndex.ChannelIndex))
                {
                    sourceIndex.ChannelIndex = SourceIndex.ParseName(channelIndex, channelNames);
                    sourceIndex.ChannelName = (channelIndex[0] == '-' ? channelIndex.Substring(1) : channelIndex);
                    sourceIndex.ByName = true;
                }
                    

                if (channelIndex[0] == '-')
                {
                    sourceIndex.Multiplier *= -1.0D;
                    if (!sourceIndex.ByName)
                        sourceIndex.ChannelIndex *= -1;
                }

                return sourceIndex;
            }

            /// <summary>
            /// Attempts to Parse a channel Name into an Index.
            /// </summary>
            /// <param name="name"> The Channel Name</param>
            /// <param name="nameIndexPairs"> A List of Channel Names in Index Order.</param>
            /// <returns> The Channel Index associated with this channelName. Returns -1 if it is not found</returns>
            private static int ParseName(string name, List<string> nameIndexPairs)
            {
              
                string test = (name[0] == '-' ? name.Substring(1) : name);
                if (!nameIndexPairs.Contains(test))
                    return -1;
                if (nameIndexPairs.FindAll(item => item == test).Count() > 1)
                    throw new FormatException($"Incorrect format for channel name {test} duplicates found in configuration.");
               
                return nameIndexPairs.FindIndex(item => item == test);
               
            }
        }

        // Constants
        private const double Sqrt3 = 1.7320508075688772935274463415059D;

        #endregion

        #region [ Methods ]

        public override void Execute(MeterDataSet meterDataSet)
        {
            // Grab the parsed meter right away as we will be replacing it in the meter data set with the meter from the database
            Meter parsedMeter = meterDataSet.Meter;

            //Check if it is APP Trending Data
            bool isRMSTrending = false;
            string rmsTrendingMatch = null;
            bool isFlkrTrending = false;
            string flkrTrendingMatch = null;
            bool isTriggerTrending = false;
            string triggerTrendingMatch = null;

            using (AdoDataConnection connection = meterDataSet.CreateDbConnection())
            {
                rmsTrendingMatch = connection.ExecuteScalar<string>("SELECT VALUE FROM Setting WHERE Name = 'TrendingData.RMS.FolderPath'");
                flkrTrendingMatch = connection.ExecuteScalar<string>("SELECT VALUE FROM Setting WHERE Name = 'TrendingData.Flicker.FolderPath'");
                triggerTrendingMatch = connection.ExecuteScalar<string>("SELECT VALUE FROM Setting WHERE Name = 'TrendingData.Trigger.FolderPath'");
            }

            if (rmsTrendingMatch != null)
            {
                Regex rmsTrendingRegex = new Regex(rmsTrendingMatch, RegexOptions.Compiled);
                isRMSTrending = rmsTrendingRegex.Match(meterDataSet.FilePath).Success;
            }
            if (flkrTrendingMatch != null)
            {
                Regex flkrTrendingRegex = new Regex(flkrTrendingMatch, RegexOptions.Compiled);
                isFlkrTrending = flkrTrendingRegex.Match(meterDataSet.FilePath).Success;
            }
            if (triggerTrendingMatch != null)
            {
                Regex triggerTrendingRegex = new Regex(triggerTrendingMatch, RegexOptions.Compiled);
                isTriggerTrending = triggerTrendingRegex.Match(meterDataSet.FilePath).Success;
            }

            // Search the database for a meter definition that matches the parsed meter
            Meter dbMeter = LoadMeterFromDatabase(meterDataSet);

            if ((object)dbMeter == null)
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

            // Get the list of series associated with the meter in the database
            List<Series> seriesList = dbMeter.Channels
                .SelectMany(channel => channel.Series)
                .ToList();

            // Create data series for series which
            // are combinations of the parsed series
            List<DataSeries> calculatedDataSeriesList = new List<DataSeries>();

            if (isRMSTrending)
            {
                // Only get Trending Data for Voltages and Currents
                List<Channel> powChannels = dbMeter.Channels.Where(channel =>
                    (channel.MeasurementType.Name == "Voltage" || channel.MeasurementType.Name == "Current") && channel.MeasurementCharacteristic.Name == "Instantaneous")
                    .ToList();

                List<SourceIndex> rmsSourceIndices = seriesList.SelectMany(series => series.SourceIndexes.Split(',').Where(item => !string.IsNullOrEmpty(item))
                    .Select(item => SourceIndex.Parse(item))).Where(item => item.IsRMSTrend).ToList();
                
                // Add RMS Channels as neccesarry for PoW Channels
                powChannels.ForEach(item => AddRMSChannel(item, rmsSourceIndices,meterDataSet));

                //Need to update Meter here to make sure we have all Series that were generated
                dbMeter.Channels = null;

                seriesList = dbMeter.Channels
                   .SelectMany(channel => channel.Series)
                   .ToList();
                 
                //compute Series properly to account for multiply and Add (only deal with RMS here)
                foreach (Series series in seriesList.Where(series => (!string.IsNullOrEmpty(series.SourceIndexes)) &&
                    (series.SourceIndexes.Split(',').Any(item => SourceIndex.Parse(item).IsRMSTrend))))
                    AddRMSTrendSeries(calculatedDataSeriesList, meterDataSet, series);
            }
            else if (isFlkrTrending)
            {
                // Only get Trending Data for Voltages and Currents
                List<Channel> powChannels = dbMeter.Channels.Where(channel =>
                    (channel.MeasurementType.Name == "Voltage" || channel.MeasurementType.Name == "Current") && channel.MeasurementCharacteristic.Name == "Instantaneous")
                    .ToList();

                List<SourceIndex> flkrSourceIndices = seriesList.SelectMany(series => series.SourceIndexes.Split(',').Where(item => !string.IsNullOrEmpty(item))
                    .Select(item => SourceIndex.Parse(item))).Where(item => item.IsFLKRTrend).ToList();

                    // Add RMS Channels as neccesarry for PoW Channels
                    powChannels.ForEach(item => AddFlkrChannel(item, flkrSourceIndices, meterDataSet));

                    //Need to update Meter here to make sure we have all Series that were generated
                    dbMeter.Channels = null;

                    seriesList = dbMeter.Channels
                       .SelectMany(channel => channel.Series)
                       .ToList();

                    //compute Series properly to account for multiply and Add
                    foreach (Series series in seriesList.Where(series => !string.IsNullOrEmpty(series.SourceIndexes) &&
                    (series.SourceIndexes.Split(',').Any(item => SourceIndex.Parse(item).IsFLKRTrend))))
                        AddFlkrTrendSeries(calculatedDataSeriesList, meterDataSet, series);
            }
            else if (isTriggerTrending)
            {
                List<Channel> powChannels = dbMeter.Channels.Where(channel =>
                    (channel.MeasurementType.Name == "Digital") && (channel.MeasurementCharacteristic.Name == "Trigger - RMS" ||
                        channel.MeasurementCharacteristic.Name == "Trigger - Impulse" || channel.MeasurementCharacteristic.Name == "Trigger - THD" ||
                        channel.MeasurementCharacteristic.Name == "Trigger - Ubal" || channel.MeasurementCharacteristic.Name == "Trigger - I"))
                    .ToList();

                List<SourceIndex> trendSourceIndices = seriesList.SelectMany(series => series.SourceIndexes.Split(',')
                    .Where(item => !string.IsNullOrEmpty(item)).Select(item => SourceIndex.Parse(item))).Where(item => item.IsTriggerTrend).ToList();

                // Add Trend Channels as neccesarry for PoW Channels
                powChannels.ForEach(item => AddTriggerChannel(item, trendSourceIndices, meterDataSet));

                //Need to update Meter here to make sure we have all Series that were generated
                dbMeter.Channels = null;

                seriesList = dbMeter.Channels
                   .SelectMany(channel => channel.Series)
                   .ToList();

                //compute Series properly to account for multiply and Add
                foreach (Series series in seriesList.Where(series => !string.IsNullOrEmpty(series.SourceIndexes) &&
                    (series.SourceIndexes.Split(',').Any(item => SourceIndex.Parse(item).IsTriggerTrend))))
                    AddTriggerTrendSeries(calculatedDataSeriesList, meterDataSet, series);
            }
            //remove Channels with RMS Trending Data Series
            else
            {
                seriesList = seriesList.Where(series => !series.SourceIndexes.Split(',').Any(index => {
                    SourceIndex sourceIndex = SourceIndex.Parse(index);
                    return (sourceIndex == null || sourceIndex.IsTrend);
                })).ToList();


                foreach (Series series in seriesList.Where(series => !string.IsNullOrEmpty(series.SourceIndexes)))
                    AddCalculatedDataSeries(calculatedDataSeriesList, meterDataSet, series);
            }

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

            // Only update database configuration if NOT using historic configuration.
            if (!meterDataSet.LoadHistoricConfiguration)
            {
                // Add channels that are not already defined in the
                // configuration by assuming the meter monitors only one line
                AddUndefinedChannels(meterDataSet);

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

        private void AddCalculatedDataSeries(List<DataSeries> calculatedDataSeriesList, MeterDataSet meterDataSet, Series series)
        {
            List<SourceIndex> sourceIndexes;
            DataSeries dataSeries;

            List<string> channelNames = meterDataSet.DataSeries.Select(item => (item.SeriesInfo == null? "" : item.SeriesInfo.Channel.Name)).ToList();
            sourceIndexes = series.SourceIndexes.Split(',')
                .Select(item => SourceIndex.Parse(item, channelNames))
                .Where(sourceIndex => (object)sourceIndex != null && !sourceIndex.IsRMSTrend)
                .ToList();

            if (sourceIndexes.Any(index => index.ChannelIndex < 0))
            {
                Log.Error($"Incorrect format for channel name {series.SourceIndexes} Channel not found in File.");
                return;
            }

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
                .Where(dataSeries => (object)dataSeries.SeriesInfo.Channel.Asset == null)
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
                Log.Warn($"Add channels to meter {meterDataSet.Meter.Name} Asset {meter.MeterAssets.First().Asset.AssetKey}. There are too many lines associated with that meter.");
                //return;
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
                            if(channel.SamplesPerHour == 0)
                            {
                                channel.SamplesPerHour = samplesPerHour;
                                channelTable.UpdateRecord(channel);
                            }
                        }
                    }
                }

                IEnumerable<ChannelKey> parsedChannelKeys = parsedMeter.Channels
                    .Concat(allDataSeries.Select(dataSeries => dataSeries.SeriesInfo.Channel))
                    .Where(channel => (object)channel.Asset != null)
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

        /// <summary>
        /// Checks if the RMS Trend Channels exist and adds them if necesarry.
        /// </summary>
        /// <param name="powChannel">The point on Wave Data Channel.</param>
        /// <param name="rmsIndices">Source Indices of the existing RMS Trend Channels.</param>
        /// <param name="meterDataSet"><see cref="MeterDataSet"/></param>
        private void AddRMSChannel(Channel powChannel, List<SourceIndex> rmsIndices, MeterDataSet meterDataSet)
        {
            // We Assume they get set up in pairs of 3 for now.
            if (rmsIndices.Any(item => item.ChannelIndex == powChannel.ID))
                return;

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

            Dictionary<ChannelKey, Channel> channelLookup = meterDataSet.Meter.Channels
                    .GroupBy(channel => new ChannelKey(channel))
                    .ToDictionary(grouping =>
                    {
                        if (grouping.Count() > 1)
                            Log.Warn($"Detected duplicate channel key: {grouping.First().ID}");

                        return grouping.Key;
                    }, grouping => grouping.First());

            
            using (AdoDataConnection connection = meterDataSet.CreateDbConnection())
            {
                TableOperations<MeasurementCharacteristic> measurementCharacteristicTable = new TableOperations<MeasurementCharacteristic>(connection);
                TableOperations<SeriesType> seriesTypeTable = new TableOperations<SeriesType>(connection);
                TableOperations<Channel> channelTable = new TableOperations<Channel>(connection);
                TableOperations<Series> seriesTable = new TableOperations<Series>(connection);

                
                rmsChannel.MeasurementCharacteristic = measurementCharacteristicTable.QueryRecordWhere("Name = 'RMS'");
                rmsChannel.MeasurementCharacteristicID = rmsChannel.MeasurementCharacteristic.ID;

                ChannelKey key = new ChannelKey(rmsChannel);
                Channel conChannel;
                if (!channelLookup.TryGetValue(key,out conChannel))
                {
                    channelTable.AddNewRecord(rmsChannel);
                    meterDataSet.Meter.Channels = null;
                    channelLookup = meterDataSet.Meter.Channels
                    .GroupBy(channel => new ChannelKey(channel))
                    .ToDictionary(grouping => grouping.Key, grouping => grouping.First());

                    conChannel = channelLookup[key];
                }

                //Create all 3 Series for this Channel
                Series avgSeries = new Series() 
                { 
                    ChannelID = conChannel.ID,
                    SeriesTypeID = seriesTypeTable.QueryRecordWhere("Name = 'Average'").ID,
                    SourceIndexes = "RMSAVG(" + powChannel.ID +  ")"
                };
                Series minSeries = new Series()
                {
                    ChannelID = conChannel.ID,
                    SeriesTypeID = seriesTypeTable.QueryRecordWhere("Name = 'Minimum'").ID,
                    SourceIndexes = "RMSMIN(" + powChannel.ID + ")"
                };
                Series maxSeries = new Series()
                {
                    ChannelID = conChannel.ID,
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
        private void AddFlkrChannel(Channel powChannel, List<SourceIndex> flkrIndices, MeterDataSet meterDataSet)
        {
            // We Assume they get set up in pairs of 3 for now.
            if (flkrIndices.Any(item => item.ChannelIndex == powChannel.ID))
                return;

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

            Dictionary<ChannelKey, Channel> channelLookup = meterDataSet.Meter.Channels
                    .GroupBy(channel => new ChannelKey(channel))
                    .ToDictionary(grouping =>
                    {
                        if (grouping.Count() > 1)
                            Log.Warn($"Detected duplicate channel key: {grouping.First().ID}");

                        return grouping.Key;
                    }, grouping => grouping.First());


            using (AdoDataConnection connection = meterDataSet.CreateDbConnection())
            {
                TableOperations<MeasurementCharacteristic> measurementCharacteristicTable = new TableOperations<MeasurementCharacteristic>(connection);
                TableOperations<SeriesType> seriesTypeTable = new TableOperations<SeriesType>(connection);
                TableOperations<Channel> channelTable = new TableOperations<Channel>(connection);
                TableOperations<Series> seriesTable = new TableOperations<Series>(connection);


                flkrChannel.MeasurementCharacteristic = measurementCharacteristicTable.QueryRecordWhere("Name = 'FlkrPST'");
                flkrChannel.MeasurementCharacteristicID = flkrChannel.MeasurementCharacteristic.ID;

                ChannelKey key = new ChannelKey(flkrChannel);
                Channel conChannel;
                if (!channelLookup.TryGetValue(key, out conChannel))
                {
                    channelTable.AddNewRecord(flkrChannel);
                    meterDataSet.Meter.Channels = null;
                    channelLookup = meterDataSet.Meter.Channels
                    .GroupBy(channel => new ChannelKey(channel))
                    .ToDictionary(grouping => grouping.Key, grouping => grouping.First());

                    conChannel = channelLookup[key];
                }

                //Create Series for this Channel
                Series avgSeries = new Series()
                {
                    ChannelID = conChannel.ID,
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
        private void AddTriggerChannel(Channel powChannel, List<SourceIndex> triggerIndices, MeterDataSet meterDataSet)
        {
            // We Assume they get set up in pairs of 3 for now.
            if (triggerIndices.Any(item => item.ChannelIndex == powChannel.ID))
                return;

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
            

            Dictionary<ChannelKey, Channel> channelLookup = meterDataSet.Meter.Channels
                    .GroupBy(channel => new ChannelKey(channel))
                    .ToDictionary(grouping =>
                    {
                        if (grouping.Count() > 1)
                            Log.Warn($"Detected duplicate channel key: {grouping.First().ID}");

                        return grouping.Key;
                    }, grouping => grouping.First());


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
                Channel conChannel;
                if (!channelLookup.TryGetValue(key, out conChannel))
                {
                    channelTable.AddNewRecord(trendChannel);
                    meterDataSet.Meter.Channels = null;
                    channelLookup = meterDataSet.Meter.Channels
                    .GroupBy(channel => new ChannelKey(channel))
                    .ToDictionary(grouping => grouping.Key, grouping => grouping.First());

                    conChannel = channelLookup[key];
                }

                //Create all 3 Series for this Channel
                Series avgSeries = new Series()
                {
                    ChannelID = conChannel.ID,
                    SeriesTypeID = seriesTypeTable.QueryRecordWhere("Name = 'Average'").ID,
                    SourceIndexes = "TRIGGERAVG(" + powChannel.ID + ")"
                };
                Series minSeries = new Series()
                {
                    ChannelID = conChannel.ID,
                    SeriesTypeID = seriesTypeTable.QueryRecordWhere("Name = 'Minimum'").ID,
                    SourceIndexes = "TRIGGERMIN(" + powChannel.ID + ")"
                };
                Series maxSeries = new Series()
                {
                    ChannelID = conChannel.ID,
                    SeriesTypeID = seriesTypeTable.QueryRecordWhere("Name = 'Maximum'").ID,
                    SourceIndexes = "TRIGGERMAX(" + powChannel.ID + ")"
                };

                seriesTable.AddNewRecord(avgSeries);
                seriesTable.AddNewRecord(minSeries);
                seriesTable.AddNewRecord(maxSeries);


            }

        }

        /// <summary>
        /// Adds the Processed RMS Trend Data
        /// </summary>
        /// <param name="calculatedDataSeriesList">List of all processed DataSeries</param>
        /// <param name="meterDataSet"><see cref="MeterDataSet"/></param>
        /// <param name="series">The RMS Trend Series that is being processed</param>
        private void AddRMSTrendSeries(List<DataSeries> calculatedDataSeriesList, MeterDataSet meterDataSet, Series series)
        {
            List<SourceIndex> sourceIndexes;
            DataSeries dataSeries;
            Series sourceSeries;

            //If the corresponding Channel exists (and is attached to this Meter)
            if (!meterDataSet.Meter.Channels.Where(channel => channel.ID == SourceIndex.Parse(series.SourceIndexes).ChannelIndex).Any())
                return;

            //If that Channel has a Series Attached to it
            if (!meterDataSet.Meter.Channels.Where(channel => channel.ID == SourceIndex.Parse(series.SourceIndexes).ChannelIndex).First().Series.Any())
                return;

            List<string> channelNames = meterDataSet.DataSeries.Select(item => (item.SeriesInfo == null ? "" : item.SeriesInfo.Channel.Name)).ToList();

            sourceSeries = meterDataSet.Meter.Channels.Where(channel => channel.ID == SourceIndex.Parse(series.SourceIndexes).ChannelIndex).First().Series.First();
            sourceIndexes = sourceSeries.SourceIndexes.Split(',')
                .Select(item =>
                {

                    SourceIndex sourceIndex = SourceIndex.Parse(item, channelNames);
                    if ((object)sourceIndex == null)
                        return null;
                    if (!sourceIndex.ByName)
                    {
                        item = sourceIndex.Multiplier.ToString() + "*A" + sourceIndex.ChannelIndex.ToString();
                    }
                    return Translate(item, SourceIndex.Parse(series.SourceIndexes), channelNames);
                }).Where(sourceIndex => (object)sourceIndex != null && !sourceIndex.IsRMSTrend)
                .ToList();

           if (sourceIndexes.Count == 0)
                return;

            dataSeries = sourceIndexes
                    .Select(sourceIndex => meterDataSet.DataSeries[sourceIndex.ChannelIndex].Multiply(sourceIndex.Multiplier))
                    .Aggregate((series1, series2) => series1.Add(series2));

            dataSeries.SeriesInfo = series;
            calculatedDataSeriesList.Add(dataSeries);

        }

        /// <summary>
        /// Adds the Processed Trigger Trend Data
        /// </summary>
        /// <param name="calculatedDataSeriesList">List of all processed DataSeries</param>
        /// <param name="meterDataSet"><see cref="MeterDataSet"/></param>
        /// <param name="series">The Trigger Series that is being processed</param>
        private void AddTriggerTrendSeries(List<DataSeries> calculatedDataSeriesList, MeterDataSet meterDataSet, Series series)
        {
            List<SourceIndex> sourceIndexes;
            DataSeries dataSeries;
            Series sourceSeries;

            if (!meterDataSet.Meter.Channels.Where(channel => channel.ID == SourceIndex.Parse(series.SourceIndexes).ChannelIndex).Any())
                return;

            if (!meterDataSet.Meter.Channels.Where(channel => channel.ID == SourceIndex.Parse(series.SourceIndexes).ChannelIndex).First().Series.Any())
                return;

            List<string> channelNames = meterDataSet.DataSeries.Select(item => (item.SeriesInfo == null ? "" : item.SeriesInfo.Channel.Name)).ToList();

            sourceSeries = meterDataSet.Meter.Channels.Where(channel => channel.ID == SourceIndex.Parse(series.SourceIndexes).ChannelIndex).First().Series.First();
            sourceIndexes = sourceSeries.SourceIndexes.Split(',')
                .Select(item => Translate(item, SourceIndex.Parse(series.SourceIndexes), channelNames))
                .Where(sourceIndex => (object)sourceIndex != null && !sourceIndex.IsTriggerTrend)
                .ToList();

            if (sourceIndexes.Count == 0)
                return;



            dataSeries = sourceIndexes
                    .Select(sourceIndex => meterDataSet.DataSeries[sourceIndex.ChannelIndex].Multiply(sourceIndex.Multiplier))
                    .Aggregate((series1, series2) => series1.Add(series2));



            dataSeries.SeriesInfo = series;
            calculatedDataSeriesList.Add(dataSeries);

        }

        /// <summary>
        /// Adds the Processed Flicker Trend Data
        /// </summary>
        /// <param name="calculatedDataSeriesList">List of all processed DataSeries</param>
        /// <param name="meterDataSet"><see cref="MeterDataSet"/></param>
        /// <param name="series">The Flicker Trend Series that is being processed</param>
        private void AddFlkrTrendSeries(List<DataSeries> calculatedDataSeriesList, MeterDataSet meterDataSet, Series series)
        {
            List<SourceIndex> sourceIndexes;
            DataSeries dataSeries;
            Series sourceSeries;

            if (!meterDataSet.Meter.Channels.Where(channel => channel.ID == SourceIndex.Parse(series.SourceIndexes).ChannelIndex).Any())
                return;

            if (!meterDataSet.Meter.Channels.Where(channel => channel.ID == SourceIndex.Parse(series.SourceIndexes).ChannelIndex).First().Series.Any())
                return;

            List<string> channelNames = meterDataSet.DataSeries.Select(item => (item.SeriesInfo == null ? "" : item.SeriesInfo.Channel.Name)).ToList();

            sourceSeries = meterDataSet.Meter.Channels.Where(channel => channel.ID == SourceIndex.Parse(series.SourceIndexes).ChannelIndex).First().Series.First();
            sourceIndexes = sourceSeries.SourceIndexes.Split(',')
                .Select(item => {
                    SourceIndex sourceIndex = SourceIndex.Parse(item, channelNames);
                    if ((object)sourceIndex == null)
                        return null;

                    if (!sourceIndex.ByName)
                    {
                        item = sourceIndex.Multiplier.ToString() + "*A" + sourceIndex.ChannelIndex.ToString();
                        sourceIndex = SourceIndex.Parse(item, channelNames);
                    }
                    return sourceIndex; 
                })
                .Where(sourceIndex => (object)sourceIndex != null && !sourceIndex.IsFLKRTrend)
                .ToList();

            if (sourceIndexes.Count == 0)
                return;



            dataSeries = sourceIndexes
                    .Select(sourceIndex => meterDataSet.DataSeries[sourceIndex.ChannelIndex].Multiply(sourceIndex.Multiplier))
                    .Aggregate((series1, series2) => series1.Add(series2));



            dataSeries.SeriesInfo = series;
            calculatedDataSeriesList.Add(dataSeries);

        }

        private SourceIndex Translate(string original, SourceIndex functional, List<string> seriesNames)
        {
            if (!functional.IsTrend)
                return null;

            string translated = original;
            SourceIndex parsed = SourceIndex.Parse(original);

            if (!parsed.ByName)
                return null;

            if (functional.ChannelName.EndsWith("AVG", StringComparison.OrdinalIgnoreCase))
                translated = translated.Replace(parsed.ChannelName, "Avg_" + parsed.ChannelName);
            if (functional.ChannelName.EndsWith("MIN", StringComparison.OrdinalIgnoreCase))
                translated = translated.Replace(parsed.ChannelName, "Min_" + parsed.ChannelName);
            if (functional.ChannelName.EndsWith("MAX", StringComparison.OrdinalIgnoreCase))
                translated = translated.Replace(parsed.ChannelName, "Max_" + parsed.ChannelName);

            // Deal with Multiplication of negative Values
            if (parsed.Multiplier < 0)
            {
                if (functional.ChannelName.EndsWith("MIN", StringComparison.OrdinalIgnoreCase))
                    translated = translated.Replace("Min_", "Max_");                   
                
                if (functional.ChannelName.EndsWith("MAX", StringComparison.OrdinalIgnoreCase))
                    translated = translated.Replace("Max_", "Min_");
                
            }

            return SourceIndex.Parse(translated, seriesNames);
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
            double nearestCommonRate = commonSampleRates.MinBy(rate => Math.Abs(samplesPerHour - rate));
            return nearestCommonRate;
        }

        #endregion
    }
}
