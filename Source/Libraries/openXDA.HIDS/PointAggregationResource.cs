//******************************************************************************************************
//  PointAggregationResource.cs - Gbtc
//
//  Copyright © 2022, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the MIT License (MIT), the "License"; you may not use this
//  file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://opensource.org/licenses/MIT
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  06/11/2022 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using FaultData.DataAnalysis;
using FaultData.DataResources;
using FaultData.DataSets;
using GSF.Collections;
using GSF.Configuration;
using HIDS;
using openXDA.HIDS.APIExtensions;
using openXDA.Model;

namespace openXDA.HIDS
{
    public class PointAggregate
    {
        public Channel Channel { get; set; }
        public DateTime Date { get; set; }

        public double Maximum { get; set; }
        public double Minimum { get; set; }
        public double TotalAverage { get; set; }

        public int ExpectedPoints { get; set; }
        public int ReceivedPoints { get; set; }
        public int ValidPoints { get; set; }

        public int GoodPoints { get; set; }
        public int UnreasonablePoints { get; set; }
        public int LatchedPoints { get; set; }
        public int NoncongruentPoints { get; set; }
    }

    public class PointAggregationResource : DataResourceBase<MeterDataSet>
    {
        #region [ Properties ]

        [Category]
        [SettingName(HIDSSettings.CategoryName)]
        public HIDSSettings HIDSSettings { get; } = new HIDSSettings();

        public List<PointAggregate> PointAggregates { get; private set; }

        #endregion

        #region [ Methods ]

        public override void Initialize(MeterDataSet meterDataSet)
        {
            Task<List<PointAggregate>> task = QuerySummariesAsync(meterDataSet);
            PointAggregates = task.GetAwaiter().GetResult();
        }

        private async Task<List<PointAggregate>> QuerySummariesAsync(MeterDataSet meterDataSet)
        {
            TrendingGroupsResource trendingGroupsResource = meterDataSet.GetResource<TrendingGroupsResource>();
            Dictionary<Channel, List<DataGroup>> trendingGroups = trendingGroupsResource.TrendingGroups;

            using API hids = new API();
            hids.Configure(HIDSSettings);

            IEnumerable<string> tags = meterDataSet.Meter.Channels
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

            Dictionary<int, Channel> channelLookup = trendingGroups.Keys.ToDictionary(channel => channel.ID);

            var aggregateLookup = Enumerable
                .Empty<PointAggregate>()
                .ToDictionary(aggregate => new { ChannelID = default(int), Date = default(DateTime) });

            await foreach (Point point in hids.ReadPointsAsync(QueryAllPoints))
            {
                int channelID = hids.ToChannelID(point.Tag);
                DateTime date = point.Timestamp.Date;

                if (!channelLookup.TryGetValue(channelID, out Channel channel))
                    continue;

                int expectedPoints = (int)(24.0D * channel.SamplesPerHour);

                PointAggregate NewAggregate() => new PointAggregate()
                {
                    Channel = channel,
                    Date = date,
                    ExpectedPoints = expectedPoints
                };

                var key = new { ChannelID = channelID, Date = date };
                PointAggregate aggregate = aggregateLookup.GetOrAdd(key, _ => NewAggregate());
                UpdateValueAggregates(aggregate, point);
                UpdateSummaryCounts(aggregate, point);
                UpdateQualityCounts(aggregate, point);
            }

            return aggregateLookup.Values.ToList();
        }

        private void UpdateValueAggregates(PointAggregate aggregate, Point point)
        {
            if (aggregate.ReceivedPoints == 0 || point.Maximum > aggregate.Maximum)
                aggregate.Maximum = point.Maximum;

            if (aggregate.ReceivedPoints == 0 || point.Minimum < aggregate.Minimum)
                aggregate.Minimum = point.Minimum;

            aggregate.TotalAverage += point.Average;
        }

        private void UpdateSummaryCounts(PointAggregate aggregate, Point point)
        {
            aggregate.ReceivedPoints++;

            if (point.QualityFlags == 0)
                aggregate.ValidPoints++;
        }

        private void UpdateQualityCounts(PointAggregate aggregate, Point point)
        {
            QualityFlags badFlags =
                QualityFlags.Unreasonable |
                QualityFlags.Latched |
                QualityFlags.Noncongruent;

            if ((point.QualityFlags & (uint)badFlags) == 0)
                aggregate.GoodPoints++;

            if ((point.QualityFlags & (uint)QualityFlags.Unreasonable) != 0)
                aggregate.UnreasonablePoints++;

            if ((point.QualityFlags & (uint)QualityFlags.Latched) != 0)
                aggregate.LatchedPoints++;

            if ((point.QualityFlags & (uint)QualityFlags.Noncongruent) != 0)
                aggregate.NoncongruentPoints++;
        }

        #endregion
    }
}
