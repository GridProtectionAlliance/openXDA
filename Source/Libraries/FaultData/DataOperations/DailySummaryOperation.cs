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
using System.Linq;
using FaultData.DataResources;
using FaultData.DataSets;
using GSF.Data;
using GSF.Data.Model;
using log4net;
using openXDA.Model;

namespace FaultData.DataOperations
{
    public class DailySummaryOperation : DataOperationBase<MeterDataSet>
    {
        public override void Execute(MeterDataSet meterDataSet)
        {
            Dictionary<Channel, List<TrendingDataSummaryResource.TrendingDataSummary>> trendingDataSummaries = meterDataSet.GetResource<TrendingDataSummaryResource>().TrendingDataSummaries;

            int count = trendingDataSummaries.Values
                .SelectMany(channelSummaries => channelSummaries)
                .Count(trendingDataSummary => !trendingDataSummary.IsDuplicate);

            if (count == 0)
                return;

            using (AdoDataConnection connection = meterDataSet.CreateDbConnection())
            {
                TableOperations<DailyTrendingSummary> dailyTrendingSummaryTable = new TableOperations<DailyTrendingSummary>(connection);

                foreach (KeyValuePair<Channel, List<TrendingDataSummaryResource.TrendingDataSummary>> channelSummaries in trendingDataSummaries)
                {
                    IEnumerable<IGrouping<DateTime, TrendingDataSummaryResource.TrendingDataSummary>> dailySummaryGroups = channelSummaries.Value
                        .Where(summary => !summary.IsDuplicate)
                        .GroupBy(summary => GetDate(summary.Time));

                    foreach (IGrouping<DateTime, TrendingDataSummaryResource.TrendingDataSummary> dailySummaryGroup in dailySummaryGroups)
                    {
                        DailyTrendingSummary dailyTrendingSummary = CreateDailyTrendingSummary(channelSummaries.Key.ID, dailySummaryGroup.Key, dailySummaryGroup.ToList());
                        dailyTrendingSummaryTable.Upsert(dailyTrendingSummary);
                    }
                }
            }
        }

        private DailyTrendingSummary CreateDailyTrendingSummary(int channelID, DateTime date, List<TrendingDataSummaryResource.TrendingDataSummary> trendingDataSummaries)
        {
            List<TrendingDataSummaryResource.TrendingDataSummary> validSummaries = trendingDataSummaries
                .Where(summary => summary.IsValid)
                .ToList();

            return new DailyTrendingSummary()
            {
                ChannelID = channelID,
                Date = date,
                Minimum = validSummaries.Select(summary => summary.Minimum).DefaultIfEmpty(0.0D).Min(),
                Maximum = validSummaries.Select(summary => summary.Maximum).DefaultIfEmpty(0.0D).Max(),
                Average = validSummaries.Select(summary => summary.Average).DefaultIfEmpty(0.0D).Average(),
                ValidCount = validSummaries.Count,
                InvalidCount = trendingDataSummaries.Count - validSummaries.Count
            };
        }

        private DateTime GetDate(DateTime time)
        {
            return new DateTime(time.Year, time.Month, time.Day);
        }

        // Static Fields
        private static readonly ILog Log = LogManager.GetLogger(typeof(DailySummaryOperation));
    }
}
