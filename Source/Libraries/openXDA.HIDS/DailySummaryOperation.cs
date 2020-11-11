//******************************************************************************************************
//  DailySummaryOperation.cs - Gbtc
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
//  07/22/2014 - Stephen C. Wills
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
    public class DailySummaryOperation : DataOperationBase<MeterDataSet>
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
                Log.Debug($"No trending data found; skipping {nameof(DailySummaryOperation)}.");
                return;
            }

            using AdoDataConnection connection = meterDataSet.CreateDbConnection();
            TableOperations<DailyTrendingSummary> dailyTrendingSummaryTable = new TableOperations<DailyTrendingSummary>(connection);

            async Task LoadSummariesAsync()
            {
                await foreach (DailyTrendingSummary summary in QuerySummariesAsync(trendingGroups))
                    dailyTrendingSummaryTable.Upsert(summary);
            }

            Task loadSummariesTask = LoadSummariesAsync();
            loadSummariesTask.GetAwaiter().GetResult();
        }

        private IAsyncEnumerable<DailyTrendingSummary> QuerySummariesAsync(Dictionary<Channel, List<DataGroup>> trendingGroups)
        {
            using API hids = new API();
            hids.Configure(HIDSSettings);

            IEnumerable<string> tags = trendingGroups.Keys
                .Select(channel => hids.ToTag(channel.ID));

            DateTime startTime = trendingGroups.Values
                .SelectMany(list => list)
                .Min(dataGroup => dataGroup.StartTime.Date);

            DateTime endTime = trendingGroups.Values
                .SelectMany(list => list)
                .Max(dataGroup => dataGroup.EndTime.AddDays(1.0D).AddTicks(-1L).Date);

            void QueryValidPoints(IQueryBuilder queryBuilder) => queryBuilder
                .Range(startTime, endTime)
                .FilterTags(tags)
                .TestQuality(~0u)
                .Aggregate("1d");

            void QueryAllPoints(IQueryBuilder queryBuilder) => queryBuilder
                .Range(startTime, endTime)
                .FilterTags(tags)
                .Aggregate("1d");

            IAsyncEnumerable<Point> points = hids.ReadPointsAsync(QueryValidPoints);
            IAsyncEnumerable<PointCount> validPointCounts = hids.ReadPointCountAsync(QueryValidPoints);
            IAsyncEnumerable<PointCount> totalPointCounts = hids.ReadPointCountAsync(QueryAllPoints);

            static Func<T1, T2> ToSelector<T1, T2>(Func<T1, T2> func) => func;
            var pointSelector = ToSelector((Point point) => new { point.Tag, point.Timestamp });
            var countSelector = ToSelector((PointCount count) => new { count.Tag, count.Timestamp });

            static Task<ulong> SumAsync(IAsyncEnumerable<PointCount> counts) => counts
                .AggregateAsync(0Lu, (total, count) => total + count.Count)
                .AsTask();

            return Enumerable.Range(0, (int)endTime.Subtract(startTime).TotalDays)
                .Select(days => startTime.AddDays(days))
                .SelectMany(date => trendingGroups.Keys.Select(channel => new { Tag = channel.ID.ToString("x8"), Timestamp = date }))
                .ToAsyncEnumerable()
                .GroupJoin(points, key => key, pointSelector, (Key, points) => new { Key, PointTask = points.FirstOrDefaultAsync().AsTask() })
                .GroupJoin(validPointCounts, record => record.Key, countSelector, (record, counts) => new { record.Key, record.PointTask, ValidCountTask = SumAsync(counts) })
                .GroupJoin(totalPointCounts, record => record.Key, countSelector, (record, counts) => new { record.Key, record.PointTask, record.ValidCountTask, TotalCountTask = SumAsync(counts) })
                .WhereAwait(async record => (await record.PointTask) != null)
                .SelectAwait(async record =>
                {
                    Point point = await record.PointTask;
                    ulong validCount = await record.ValidCountTask;
                    ulong totalCount = await record.TotalCountTask;

                    return new DailyTrendingSummary()
                    {
                        ChannelID = Convert.ToInt32(record.Key.Tag, 16),
                        Date = record.Key.Timestamp,
                        Maximum = point.Maximum,
                        Minimum = point.Minimum,
                        Average = point.Average,
                        ValidCount = (int)validCount,
                        InvalidCount = (int)(totalCount - validCount)
                    };
                });
        }

        // Static Fields
        private static readonly ILog Log = LogManager.GetLogger(typeof(DailySummaryOperation));
    }
}
