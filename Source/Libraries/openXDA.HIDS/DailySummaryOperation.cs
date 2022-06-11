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

using System.Collections.Generic;
using System.ComponentModel;
using FaultData.DataAnalysis;
using FaultData.DataOperations;
using FaultData.DataResources;
using FaultData.DataSets;
using GSF.Configuration;
using GSF.Data;
using GSF.Data.Model;
using log4net;
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

            if (string.IsNullOrEmpty(HIDSSettings.Host))
            {
                Log.Debug($"No HIDS instance defined; skipping {nameof(DailySummaryOperation)}.");
                return;
            }

            PointAggregationResource pointAggregationResource = meterDataSet.GetResource<PointAggregationResource>();
            List<PointAggregate> pointAggregates = pointAggregationResource.PointAggregates;

            using AdoDataConnection connection = meterDataSet.CreateDbConnection();
            TableOperations<DailyTrendingSummary> dailyTrendingSummaryTable = new TableOperations<DailyTrendingSummary>(connection);

            foreach (PointAggregate pointAggregate in pointAggregates)
            {
                DailyTrendingSummary summary = new DailyTrendingSummary()
                {
                    ChannelID = pointAggregate.Channel.ID,
                    Date = pointAggregate.Date,
                    Maximum = pointAggregate.Maximum,
                    Minimum = pointAggregate.Minimum,
                    Average = pointAggregate.TotalAverage / pointAggregate.ReceivedPoints,
                    ValidCount = pointAggregate.ValidPoints,
                    InvalidCount = pointAggregate.ReceivedPoints - pointAggregate.ValidPoints
                };

                dailyTrendingSummaryTable.Upsert(summary);
            }
        }

        // Static Fields
        private static readonly ILog Log = LogManager.GetLogger(typeof(DailySummaryOperation));
    }
}
