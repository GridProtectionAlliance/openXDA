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
using GSF;

namespace FaultData.DataOperations
{
    public class DailySummaryOperation : IDataOperation
    {
        #region [ Members ]

        // Events
        public event EventHandler<EventArgs<string>> StatusMessage;
        public event EventHandler<EventArgs<Exception>> ProcessException;

        // Fields
        private BulkLoader m_bulkLoader;

        #endregion

        #region [ Constructors ]

        public DailySummaryOperation(string connectionString)
        {
            m_bulkLoader = new BulkLoader();
            m_bulkLoader.ConnectionString = connectionString;

            m_bulkLoader.CreateTableFormat = "CREATE TABLE {0} " +
                                             "( " +
                                             "    ID INT, " +
                                             "    ChannelID INT, " +
                                             "    Date DATE, " +
                                             "    Minimum FLOAT, " +
                                             "    Maximum FLOAT, " +
                                             "    Average FLOAT, " +
                                             "    Count INT " +
                                             ")";

            m_bulkLoader.MergeTableFormat = "MERGE INTO {0} AS Target " +
                                            "USING {1} AS Source " +
                                            "ON Source.ChannelID = Target.ChannelID AND Source.Date = Target.Date " +
                                            "WHEN MATCHED THEN " +
                                            "    UPDATE SET " +
                                            "        Maximum = IIF(Source.Maximum > Target.Maximum, Source.Maximum, Target.Maximum), " +
                                            "        Minimum = IIF(Source.Minimum < Target.Minimum, Source.Minimum, Target.Minimum), " +
                                            "        Average = Target.Average + (Source.Count * (Source.Average - Target.Average) / (Source.Count + Target.Count)), " +
                                            "        Count = Source.Count + Target.Count " +
                                            "WHEN NOT MATCHED THEN " +
                                            "    INSERT (ChannelID, Date, Maximum, Minimum, Average, Count) " +
                                            "    VALUES (Source.ChannelID, Source.Date, Source.Maximum, Source.Minimum, Source.Average, Source.Count);";
        }

        #endregion

        #region [ Methods ]

        public void Execute(MeterDataSet meterDataSet)
        {
            Dictionary<Channel, List<TrendingDataSummaryResource.TrendingDataSummary>> trendingDataSummaries = meterDataSet.GetResource<TrendingDataSummaryResource>().TrendingDataSummaries;
            MeterData.DailyTrendingSummaryDataTable dailySummaryTable = new MeterData.DailyTrendingSummaryDataTable();
            MeterData.DailyTrendingSummaryRow row;

            foreach (KeyValuePair<Channel, List<TrendingDataSummaryResource.TrendingDataSummary>> channelSummaries in trendingDataSummaries)
            {
                foreach (IGrouping<DateTime, TrendingDataSummaryResource.TrendingDataSummary> hourlySummary in channelSummaries.Value.GroupBy(summary => GetDate(summary.Time)))
                {
                    row = dailySummaryTable.NewDailyTrendingSummaryRow();

                    row.BeginEdit();
                    row.ChannelID = channelSummaries.Key.ID;
                    row.Date = hourlySummary.Key;
                    row.Minimum = hourlySummary.Min(summary => summary.Minimum);
                    row.Maximum = hourlySummary.Max(summary => summary.Maximum);
                    row.Average = hourlySummary.Average(summary => summary.Average);
                    row.Count = hourlySummary.Count();
                    row.EndEdit();

                    dailySummaryTable.AddDailyTrendingSummaryRow(row);
                }
            }

            // Bulk insert new rows
            m_bulkLoader.Load(dailySummaryTable);
        }

        private DateTime GetDate(DateTime time)
        {
            return new DateTime(time.Year, time.Month, time.Day);
        }

        private void OnStatusMessage(string message)
        {
            if ((object)StatusMessage != null)
                StatusMessage(this, new EventArgs<string>(message));
        }

        private void OnProcessException(Exception ex)
        {
            if ((object)ProcessException != null)
                ProcessException(this, new EventArgs<Exception>(ex));
        }

        #endregion
    }
}
