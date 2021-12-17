//******************************************************************************************************
//  DataQualityOperation.cs - Gbtc
//
//  Copyright © 2015, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the MIT License (MIT), the "License"; you may
//  not use this file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://opensource.org/licenses/MIT
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  08/25/2015 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using FaultData.DataAnalysis;
using FaultData.DataOperations;
using FaultData.DataResources;
using FaultData.DataSets;
using GSF.Configuration;
using GSF.Data;
using GSF.Data.Model;
using HIDS;
using log4net;
using openXDA.HIDS.APIExtensions;
using openXDA.Model;

namespace openXDA.HIDS
{
    public class DataQualityOperation : DataOperationBase<MeterDataSet>
    {
        [Category]
        [SettingName(HIDSSettings.CategoryName)]
        public HIDSSettings HIDSSettings { get; } = new HIDSSettings();

        public override void Execute(MeterDataSet meterDataSet)
        {
            TrendingGroupsResource trendingGroupsResource = meterDataSet.GetResource<TrendingGroupsResource>();
            Dictionary<Channel, List<DataGroup>> trendingGroups = trendingGroupsResource.TrendingGroups;

            if (trendingGroups.Count == 0)
            {
                Log.Debug($"No trending data found; skipping {nameof(DataQualityOperation)}.");
                return;
            }

            if (string.IsNullOrEmpty(HIDSSettings.Host))
            {
                Log.Debug($"No HIDS instance defined; skipping {nameof(DataQualityOperation)}.");
                return;
            }

            // Get the total cumulative samples per hour
            // of each of the enabled channels in the meter
            double meterSamplesPerHour = meterDataSet.Meter.Channels
                .Where(channel => channel.Enabled)
                .Where(channel => channel.SamplesPerHour <= 60.0D)
                .Select(channel => channel.SamplesPerHour)
                .DefaultIfEmpty(0.0D)
                .Sum();

            using AdoDataConnection connection = meterDataSet.CreateDbConnection();
            TableOperations<ChannelDataQualitySummary> channelDataQualitySummaryTable = new TableOperations<ChannelDataQualitySummary>(connection);
            TableOperations<MeterDataQualitySummary> meterDataQualitySummaryTable = new TableOperations<MeterDataQualitySummary>(connection);

            async Task LoadSummariesAsync()
            {

                await foreach (IAsyncGrouping<DateTime, ChannelDataQualitySummary> grouping in QuerySummariesAsync(trendingGroups).GroupBy(summary => summary.Date))
                {
                    MeterDataQualitySummary meterDataQualitySummary = new MeterDataQualitySummary()
                    {
                        MeterID = meterDataSet.Meter.ID,
                        Date = grouping.Key,
                        ExpectedPoints = (int)(24.0D * meterSamplesPerHour)
                    };

                    await foreach (ChannelDataQualitySummary channelDataQualitySummary in grouping)
                    {
                        meterDataQualitySummary.UnreasonablePoints += channelDataQualitySummary.UnreasonablePoints;
                        meterDataQualitySummary.LatchedPoints += channelDataQualitySummary.LatchedPoints;
                        meterDataQualitySummary.NoncongruentPoints += channelDataQualitySummary.NoncongruentPoints;
                        meterDataQualitySummary.GoodPoints += channelDataQualitySummary.GoodPoints;
                        channelDataQualitySummaryTable.Upsert(channelDataQualitySummary);
                    }

                    meterDataQualitySummaryTable.Upsert(meterDataQualitySummary);
                }
            }

            Task loadSummariesTask = LoadSummariesAsync();
            loadSummariesTask.GetAwaiter().GetResult();
        }

        private async IAsyncEnumerable<ChannelDataQualitySummary> QuerySummariesAsync(Dictionary<Channel, List<DataGroup>> trendingGroups)
        {
            using API hids = new API();
            hids.Configure(HIDSSettings);

            IEnumerable<string> tags = trendingGroups.Keys
                .Where(channel => channel.Enabled)
                .Select(channel => hids.ToTag(channel.ID));

            DateTime startTime = trendingGroups.Values
                .SelectMany(list => list)
                .Min(dataGroup => dataGroup.StartTime.Date);

            DateTime endTime = trendingGroups.Values
                .SelectMany(list => list)
                .Max(dataGroup => dataGroup.EndTime.AddDays(1.0D).AddTicks(-1L).Date);

            void QueryAllPoints(IQueryBuilder queryBuilder) => queryBuilder
                .Range(startTime, endTime)
                .FilterTags(tags);


            var dates = Enumerable.Range(0, (int)endTime.Subtract(startTime).TotalDays).Select(days => startTime.AddDays(days));
            var channels = dates.SelectMany(date => trendingGroups.Keys.Select(Channel => new { ChannelID = Channel.ID, Date = date, ExpectedPoints = (int)(24.0D * Channel.SamplesPerHour) })).ToAsyncEnumerable();


            IAsyncEnumerable<Point> points = hids.ReadPointsAsync(QueryAllPoints);
            var latchedPoints = points.Where(x => x.QualityFlags > 1 && (x.QualityFlags & (uint)QualityFlags.Latched) == x.QualityFlags).GroupBy(x => new { Date = x.Timestamp.Date, ChannelID = Convert.ToInt32(x.Tag, 16) }).Select(x => new {Date = x.Key.Date, ChannelID = x.Key.ChannelID, Count = x.CountAsync() });
            var noncongruentPoints = points.Where(x => x.QualityFlags > 1 && (x.QualityFlags & (uint)QualityFlags.Noncongruent) == x.QualityFlags).GroupBy(x => new { Date = x.Timestamp.Date, ChannelID = Convert.ToInt32(x.Tag, 16) }).Select(x => new { Date = x.Key.Date, ChannelID = x.Key.ChannelID, Count = x.CountAsync() });
            var unreasonablePoints = points.Where(x => x.QualityFlags > 1 && (x.QualityFlags & (uint)QualityFlags.Unreasonable) == x.QualityFlags).GroupBy(x => new { Date = x.Timestamp.Date, ChannelID = Convert.ToInt32(x.Tag, 16) }).Select(x => new { Date = x.Key.Date, ChannelID = x.Key.ChannelID, Count = x.CountAsync() });
            var totalPoints = points.GroupBy(x => new { Date = x.Timestamp.Date, ChannelID = Convert.ToInt32(x.Tag, 16) }).Select(x => new { Date = x.Key.Date, ChannelID = x.Key.ChannelID, Count = x.CountAsync() });

            var records = channels.GroupJoin(unreasonablePoints, record => new { record.ChannelID, record.Date }, count => new { count.ChannelID, count.Date }, (record, counts) =>
            {
                return new { record.ChannelID, record.Date, record.ExpectedPoints, UnreasonableCount = counts.SelectAwait(x => x.Count).ToEnumerable().Sum() };
            }).GroupJoin(latchedPoints, record => new { record.ChannelID, record.Date }, count => new { count.ChannelID, count.Date }, (record, counts) =>
            {
                return new { record.ChannelID, record.Date, record.ExpectedPoints, record.UnreasonableCount, LatchedCount = counts.SelectAwait(x => x.Count).ToEnumerable().Sum() };
            }).GroupJoin(noncongruentPoints, record => new { record.ChannelID, record.Date }, count => new { count.ChannelID, count.Date }, (record, counts) =>
            {
                return new { record.ChannelID, record.Date, record.ExpectedPoints, record.UnreasonableCount, record.LatchedCount, NoncongruentCount = counts.SelectAwait(x => x.Count).ToEnumerable().Sum() };
            }).GroupJoin(totalPoints, record => new { record.ChannelID, record.Date }, count => new { count.ChannelID, count.Date }, (record, counts) =>
            {
                return new { record.ChannelID, record.Date, record.ExpectedPoints, record.UnreasonableCount, record.LatchedCount, record.NoncongruentCount, TotalCount = counts.SelectAwait(x =>x.Count).ToEnumerable().Sum() };
            });

            await foreach (var record in records)
            {
                yield return new ChannelDataQualitySummary()
                {
                    ChannelID = record.ChannelID,
                    Date = record.Date,
                    UnreasonablePoints = record.UnreasonableCount,
                    LatchedPoints = record.LatchedCount,
                    NoncongruentPoints = record.NoncongruentCount,
                    GoodPoints = record.TotalCount - record.UnreasonableCount - record.LatchedCount - record.NoncongruentCount,
                    ExpectedPoints = record.ExpectedPoints
                };
            }
        }

        private void ProcessDataQualityRangeLimits(MeterDataSet meterDataSet)
        {
            TrendingDataSummaryResource trendingDataSummaryResource = meterDataSet.GetResource<TrendingDataSummaryResource>();
            Dictionary<Channel, List<TrendingDataSummaryResource.TrendingDataSummary>> trendingDataSummaries = trendingDataSummaryResource.TrendingDataSummaries;
            TrendingDataSummaryResource.TrendingDataSummary previousSummary = null;

            if (trendingDataSummaries.Count == 0)
                return;

            using (AdoDataConnection connection = meterDataSet.CreateDbConnection())
            {
                foreach (KeyValuePair<Channel, List<TrendingDataSummaryResource.TrendingDataSummary>> keyValuePair in trendingDataSummaries)
                {
                    Channel channel = keyValuePair.Key;
                    double perUnitValue = channel.PerUnitValue ?? 1.0D;

                    List<DataQualityRangeLimit> dataQualityRangeLimits = InitializeRangeLimitTable(connection, channel);

                    foreach (TrendingDataSummaryResource.TrendingDataSummary trendingDataSummary in keyValuePair.Value)
                    {
                        if ((object)previousSummary != null && trendingDataSummary.Minimum == previousSummary.Minimum && trendingDataSummary.Average == previousSummary.Average && trendingDataSummary.Maximum == previousSummary.Maximum)
                            trendingDataSummary.Latched = true;

                        if (trendingDataSummary.Average < trendingDataSummary.Minimum || trendingDataSummary.Average > trendingDataSummary.Maximum)
                            trendingDataSummary.NonCongruent = true;

                        foreach (DataQualityRangeLimit dataQualityRangeLimit in dataQualityRangeLimits.Where(row => row.Enabled))
                        {
                            double highLimit = 0.0D;
                            double lowLimit = 0.0D;
                            bool maxValid = true;
                            bool minValid = true;

                            double factor = dataQualityRangeLimit.PerUnit ? perUnitValue : 1.0D;

                            if ((object)dataQualityRangeLimit.High != null)
                            {
                                highLimit = factor * dataQualityRangeLimit.High.GetValueOrDefault();
                                maxValid = Convert.ToBoolean(dataQualityRangeLimit.RangeInclusive) ^ (trendingDataSummary.Maximum < highLimit);
                            }

                            if ((object)dataQualityRangeLimit.Low != null)
                            {
                                lowLimit = factor * dataQualityRangeLimit.Low.GetValueOrDefault();
                                minValid = Convert.ToBoolean(dataQualityRangeLimit.RangeInclusive) ^ (trendingDataSummary.Minimum > lowLimit);
                            }

                            if (!minValid || !maxValid)
                            {
                                trendingDataSummary.Unreasonable = true;
                                trendingDataSummary.HighLimit = highLimit;
                                trendingDataSummary.LowLimit = lowLimit;

                                if (!maxValid)
                                    trendingDataSummary.UnreasonableValue = trendingDataSummary.Maximum;
                                else
                                    trendingDataSummary.UnreasonableValue = trendingDataSummary.Minimum;

                                break;
                            }
                        }

                        previousSummary = trendingDataSummary;
                    }
                }
            }
        }

        private List<DataQualityRangeLimit> InitializeRangeLimitTable(AdoDataConnection connection, Channel channel)
        {
            // Query the range limits for the given channel
            TableOperations<DataQualityRangeLimit> dataQualityRangeLimitTable = new TableOperations<DataQualityRangeLimit>(connection);

            List<DataQualityRangeLimit> dataQualityRangeLimits = dataQualityRangeLimitTable
                .QueryRecordsWhere("ChannelID = {0}", channel.ID)
                .ToList();

            // If limits exist for the given channel,
            // range limit table has been successfully initialized
            if (dataQualityRangeLimits.Count != 0)
                return dataQualityRangeLimits;

            // Get the default range limits for the measurement type and characteristic of this channel
            TableOperations<DefaultDataQualityRangeLimit> defaultDataQualityRangeLimitTable = new TableOperations<DefaultDataQualityRangeLimit>(connection);
            int measurementTypeID = channel.MeasurementTypeID;
            int measurementCharacteristicID = channel.MeasurementCharacteristicID;

            List<DefaultDataQualityRangeLimit> defaultDataQualityRangeLimits = defaultDataQualityRangeLimitTable
                .QueryRecordsWhere("MeasurementTypeID = {0} AND MeasurementCharacteristicID = {1}", measurementTypeID, measurementCharacteristicID)
                .ToList();

            // If there are no default limits for the channel,
            // then the range limit table has been successfully initialized
            if (defaultDataQualityRangeLimits.Count == 0)
                return dataQualityRangeLimits;

            // Update the table to include this channel's default limits
            foreach (DefaultDataQualityRangeLimit defaultDataQualityRangeLimit in defaultDataQualityRangeLimits)
            {
                DataQualityRangeLimit dataQualityRangeLimit = new DataQualityRangeLimit()
                {
                    ChannelID = channel.ID,
                    High = defaultDataQualityRangeLimit.High,
                    Low = defaultDataQualityRangeLimit.Low,
                    RangeInclusive = defaultDataQualityRangeLimit.RangeInclusive,
                    PerUnit = defaultDataQualityRangeLimit.PerUnit,
                    Enabled = true
                };

                dataQualityRangeLimitTable.AddNewRecord(dataQualityRangeLimit);
            }

            return dataQualityRangeLimits;
        }

        // Static Fields
        private static readonly ILog Log = LogManager.GetLogger(typeof(DataQualityOperation));
    }
}
