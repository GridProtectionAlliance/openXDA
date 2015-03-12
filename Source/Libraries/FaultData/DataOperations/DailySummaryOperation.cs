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
using FaultData.DataResources;
using FaultData.DataSets;
using log4net;

namespace FaultData.DataOperations
{
    public class DailySummaryOperation : DataOperationBase<MeterDataSet>
    {
        #region [ Members ]

        // Fields
        private MeterData.DailyTrendingSummaryDataTable m_dailySummaryTable;

        #endregion

        #region [ Methods ]

        public override void Prepare(DbAdapterContainer dbAdapterContainer)
        {
            m_dailySummaryTable = new MeterData.DailyTrendingSummaryDataTable();
        }

        public override void Execute(MeterDataSet meterDataSet)
        {
            Dictionary<Channel, List<TrendingDataSummaryResource.TrendingDataSummary>> trendingDataSummaries = meterDataSet.GetResource<TrendingDataSummaryResource>().TrendingDataSummaries;
            MeterData.DailyTrendingSummaryRow row;

            List<TrendingDataSummaryResource.TrendingDataSummary> validSummaries;

            Log.Info("Executing operation to load daily summary data into the database...");

            foreach (KeyValuePair<Channel, List<TrendingDataSummaryResource.TrendingDataSummary>> channelSummaries in trendingDataSummaries)
            {
                foreach (IGrouping<DateTime, TrendingDataSummaryResource.TrendingDataSummary> dailySummary in channelSummaries.Value.GroupBy(summary => GetDate(summary.Time)))
                {
                    validSummaries = dailySummary.Where(summary => summary.IsValid).ToList();

                    row = m_dailySummaryTable.NewDailyTrendingSummaryRow();

                    row.BeginEdit();
                    row.ChannelID = channelSummaries.Key.ID;
                    row.Date = dailySummary.Key;
                    row.Minimum = validSummaries.Select(summary => summary.Minimum).DefaultIfEmpty(0.0D).Min();
                    row.Maximum = validSummaries.Select(summary => summary.Minimum).DefaultIfEmpty(0.0D).Max();
                    row.Average = validSummaries.Select(summary => summary.Minimum).DefaultIfEmpty(0.0D).Average();
                    row.ValidCount = validSummaries.Count;
                    row.InvalidCount = dailySummary.Count() - validSummaries.Count;
                    row.EndEdit();

                    m_dailySummaryTable.AddDailyTrendingSummaryRow(row);
                }
            }
        }

        public override void Load(DbAdapterContainer dbAdapterContainer)
        {
            BulkLoader bulkLoader;

            if (m_dailySummaryTable.Count == 0)
                return;

            Log.Info("Executing operation to load daily summary data into the database...");

            bulkLoader = new BulkLoader();
            bulkLoader.Connection = dbAdapterContainer.Connection;

            bulkLoader.MergeTableFormat = "MERGE INTO {0} AS Target " +
                                          "USING {1} AS Source " +
                                          "ON Source.ChannelID = Target.ChannelID AND Source.Date = Target.Date " +
                                          "WHEN MATCHED THEN " +
                                          "    UPDATE SET " +
                                          "        Maximum = CASE WHEN Source.Maximum > Target.Maximum THEN Source.Maximum ELSE Target.Maximum END, " +
                                          "        Minimum = CASE WHEN Source.Minimum < Target.Minimum THEN Source.Minimum ELSE Target.Minimum END, " +
                                          "        Average = Target.Average + (Source.Count * (Source.Average - Target.Average) / (Source.Count + Target.Count)), " +
                                          "        Count = Source.Count + Target.Count " +
                                          "WHEN NOT MATCHED THEN " +
                                          "    INSERT (ChannelID, Date, Maximum, Minimum, Average, Count) " +
                                          "    VALUES (Source.ChannelID, Source.Date, Source.Maximum, Source.Minimum, Source.Average, Source.Count);";
            
            // Bulk insert new rows
            bulkLoader.Load(m_dailySummaryTable);

            Log.Info(string.Format("Loaded {0} daily summary records into the database.", m_dailySummaryTable.Count));
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
