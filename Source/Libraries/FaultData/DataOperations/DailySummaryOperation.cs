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
using FaultData.Database;
using FaultData.Database.MeterDataTableAdapters;
using FaultData.DataResources;
using FaultData.DataSets;
using log4net;
using GSF.Web.Model;

namespace FaultData.DataOperations
{
    public class DailySummaryOperation : DataOperationBase<MeterDataSet>
    {
        #region [ Members ]

        // Fields
        private DbAdapterContainer m_dbAdapterContainer;

        #endregion

        #region [ Methods ]

        public override void Prepare(DbAdapterContainer dbAdapterContainer)
        {
            m_dbAdapterContainer = dbAdapterContainer;
        }

        public override void Prepare(DataContext dataContext)
        {
        }


        public override void Execute(MeterDataSet meterDataSet)
        {
            Dictionary<Channel, List<TrendingDataSummaryResource.TrendingDataSummary>> trendingDataSummaries = meterDataSet.GetResource<TrendingDataSummaryResource>().TrendingDataSummaries;
            List<TrendingDataSummaryResource.TrendingDataSummary> validSummaries;
            IEnumerable<IGrouping<DateTime, TrendingDataSummaryResource.TrendingDataSummary>> dailySummaryGroups;

            DailyTrendingSummaryTableAdapter dailySummaryAdapter;
            MeterData.DailyTrendingSummaryDataTable dailySummaryTable;
            MeterData.DailyTrendingSummaryRow row;

            Log.Info("Executing operation to load daily summary data into the database...");

            dailySummaryAdapter = m_dbAdapterContainer.GetAdapter<DailyTrendingSummaryTableAdapter>();
            dailySummaryTable = new MeterData.DailyTrendingSummaryDataTable();

            foreach (KeyValuePair<Channel, List<TrendingDataSummaryResource.TrendingDataSummary>> channelSummaries in trendingDataSummaries)
            {
                dailySummaryGroups = channelSummaries.Value
                    .Where(summary => !summary.IsDuplicate)
                    .GroupBy(summary => GetDate(summary.Time));

                foreach (IGrouping<DateTime, TrendingDataSummaryResource.TrendingDataSummary> dailySummaryGroup in dailySummaryGroups)
                {
                    validSummaries = dailySummaryGroup.Where(summary => summary.IsValid).ToList();

                    row = dailySummaryTable.NewDailyTrendingSummaryRow();

                    row.BeginEdit();
                    row.ChannelID = channelSummaries.Key.ID;
                    row.Date = dailySummaryGroup.Key;
                    row.Minimum = validSummaries.Select(summary => summary.Minimum).DefaultIfEmpty(0.0D).Min();
                    row.Maximum = validSummaries.Select(summary => summary.Minimum).DefaultIfEmpty(0.0D).Max();
                    row.Average = validSummaries.Select(summary => summary.Minimum).DefaultIfEmpty(0.0D).Average();
                    row.ValidCount = validSummaries.Count;
                    row.InvalidCount = dailySummaryGroup.Count() - validSummaries.Count;
                    row.EndEdit();

                    dailySummaryAdapter.Upsert(row);
                }
            }
        }

        public override void Load(DbAdapterContainer dbAdapterContainer)
        {
        }

        private DateTime GetDate(DateTime time)
        {
            return new DateTime(time.Year, time.Month, time.Day);
        }

        #endregion

        #region [ Static ]

        // Static Fields
        private static readonly ILog Log = LogManager.GetLogger(typeof(DailySummaryOperation));

        #endregion
    }
}
