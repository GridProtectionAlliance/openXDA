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

        private IAsyncEnumerable<ChannelDataQualitySummary> QuerySummariesAsync(Dictionary<Channel, List<DataGroup>> trendingGroups)
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

            void QueryUnreasonablePoints(IQueryBuilder queryBuilder) => queryBuilder
                .Range(startTime, endTime)
                .FilterTags(tags)
                .TestQuality((uint)QualityFlags.Unreasonable)
                .Aggregate("1d");

            void QueryLatchedPoints(IQueryBuilder queryBuilder) => queryBuilder
                .Range(startTime, endTime)
                .FilterTags(tags)
                .TestQuality((uint)QualityFlags.Latched)
                .Aggregate("1d");

            void QueryNoncongruentPoints(IQueryBuilder queryBuilder) => queryBuilder
                .Range(startTime, endTime)
                .FilterTags(tags)
                .TestQuality((uint)QualityFlags.Noncongruent)
                .Aggregate("1d");

            void QueryAllPoints(IQueryBuilder queryBuilder) => queryBuilder
                .Range(startTime, endTime)
                .FilterTags(tags)
                .Aggregate("1d");

            IAsyncEnumerable<PointCount> unreasonableCounts = hids.ReadPointCountAsync(QueryUnreasonablePoints);
            IAsyncEnumerable<PointCount> latchedCounts = hids.ReadPointCountAsync(QueryLatchedPoints);
            IAsyncEnumerable<PointCount> noncongruentCounts = hids.ReadPointCountAsync(QueryNoncongruentPoints);
            IAsyncEnumerable<PointCount> totalPointCounts = hids.ReadPointCountAsync(QueryAllPoints);

            static Func<T1, T2> ToSelector<T1, T2>(Func<T1, T2> func) => func;
            var countSelector = ToSelector((PointCount count) => new { count.Tag, count.Timestamp });

            static Task<ulong> SumAsync(IAsyncEnumerable<PointCount> counts) => counts
                .AggregateAsync(0Lu, (total, count) => total + count.Count)
                .AsTask();

            return Enumerable.Range(0, (int)endTime.Subtract(startTime).TotalDays)
                .Select(days => startTime.AddDays(days))
                .SelectMany(date => trendingGroups.Keys.Select(Channel => new { Key = new { Tag = Channel.ID.ToString("x8"), Timestamp = date }, Channel }))
                .ToAsyncEnumerable()
                .GroupJoin(unreasonableCounts, record => record.Key, countSelector, (record, counts) => new { record.Key, record.Channel, UnreasonableCountTask = SumAsync(counts) })
                .GroupJoin(latchedCounts, record => record.Key, countSelector, (record, counts) => new { record.Key, record.Channel, record.UnreasonableCountTask, LatchedCountTask = SumAsync(counts) })
                .GroupJoin(noncongruentCounts, record => record.Key, countSelector, (record, counts) => new { record.Key, record.Channel, record.UnreasonableCountTask, record.LatchedCountTask, NoncongruentCountTask = SumAsync(counts) })
                .GroupJoin(totalPointCounts, record => record.Key, countSelector, (record, counts) => new { record.Key, record.Channel, record.UnreasonableCountTask, record.LatchedCountTask, record.NoncongruentCountTask, TotalCountTask = SumAsync(counts) })
                .SelectAwait(async record =>
                {
                    ulong unreasonableCount = await record.UnreasonableCountTask;
                    ulong latchedCount = await record.LatchedCountTask;
                    ulong noncongruentCount = await record.NoncongruentCountTask;
                    ulong totalCount = await record.TotalCountTask;
                    ulong expectedCount = (ulong)(24.0D * record.Channel.SamplesPerHour);

                    return new ChannelDataQualitySummary()
                    {
                        ChannelID = Convert.ToInt32(record.Key.Tag, 16),
                        Date = record.Key.Timestamp,
                        UnreasonablePoints = (int)unreasonableCount,
                        LatchedPoints = (int)latchedCount,
                        NoncongruentPoints = (int)noncongruentCount,
                        GoodPoints = (int)(totalCount - unreasonableCount - latchedCount - noncongruentCount),
                        ExpectedPoints = (int)expectedCount
                    };
                });
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
