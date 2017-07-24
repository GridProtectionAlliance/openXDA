//******************************************************************************************************
//  HourlySummaryOperation.cs - Gbtc
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
using System.Linq;
using FaultData.DataAnalysis;
using FaultData.Database;
using FaultData.Database.MeterDataTableAdapters;
using FaultData.DataResources;
using FaultData.DataSets;
using log4net;
using GSF.Web.Model;

namespace FaultData.DataOperations
{
    public class HourlySummaryOperation : DataOperationBase<MeterDataSet>
    {
        #region [ Members ]

        // Fields
        private DbAdapterContainer m_dbAdapterContainer;
        private MeterData.HourlyTrendingSummaryDataTable m_hourlySummaryTable;
        private MeterData.ChannelNormalDataTable m_channelNormalTable;

        #endregion

        #region [ Methods ]

        public override void Prepare(DbAdapterContainer dbAdapterContainer)
        {
            m_dbAdapterContainer = dbAdapterContainer;
            m_hourlySummaryTable = new MeterData.HourlyTrendingSummaryDataTable();
            m_channelNormalTable = new MeterData.ChannelNormalDataTable();
        }

        public override void Prepare(DataContext dataContext)
        {
        }

        public override void Execute(MeterDataSet meterDataSet)
        {
            Log.Info("Executing operation to load hourly summary data into the database...");
            ProcessHourlySummaries(meterDataSet);
            ProcessChannelNormals(meterDataSet);
        }

        public override void Load(DbAdapterContainer dbAdapterContainer)
        {
            BulkLoader hourlySummaryLoader;
            BulkLoader channelNormalLoader;

            if (m_hourlySummaryTable.Count == 0 && m_channelNormalTable.Count == 0)
                return;

            Log.Info("Loading hourly summary data into the database...");

            hourlySummaryLoader = new BulkLoader();
            channelNormalLoader = new BulkLoader();

            hourlySummaryLoader.Connection = dbAdapterContainer.Connection;
            hourlySummaryLoader.CommandTimeout = dbAdapterContainer.CommandTimeout;
            channelNormalLoader.Connection = dbAdapterContainer.Connection;
            channelNormalLoader.CommandTimeout = dbAdapterContainer.CommandTimeout;

            hourlySummaryLoader.MergeTableFormat = "MERGE INTO {0} WITH (TABLOCK) AS Target " +
                                                   "USING {1} AS Source " +
                                                   "ON Source.ChannelID = Target.ChannelID AND Source.Time = Target.Time " +
                                                   "WHEN MATCHED THEN " +
                                                   "    UPDATE SET " +
                                                   "        Maximum = CASE WHEN Target.ValidCount = 0 OR Source.Maximum > Target.Maximum THEN Source.Maximum ELSE Target.Maximum END, " +
                                                   "        Minimum = CASE WHEN Target.ValidCount = 0 OR Source.Minimum < Target.Minimum THEN Source.Minimum ELSE Target.Minimum END, " +
                                                   "        Average = CASE WHEN Target.ValidCount = 0 THEN Source.Average ELSE Target.Average * (CAST(Target.ValidCount AS FLOAT) / (Target.ValidCount + Source.ValidCount)) + Source.Average * (CAST(Source.ValidCount AS FLOAT) / (Target.ValidCount + Source.ValidCount)) END, " +
                                                   "        ValidCount = Source.ValidCount + Target.ValidCount, " +
                                                   "        InvalidCount = Source.InvalidCount + Target.InvalidCount " +
                                                   "WHEN NOT MATCHED THEN " +
                                                   "    INSERT (ChannelID, Time, Maximum, Minimum, Average, ValidCount, InvalidCount) " +
                                                   "    VALUES (Source.ChannelID, Source.Time, Source.Maximum, Source.Minimum, Source.Average, Source.ValidCount, Source.InvalidCount);";

            channelNormalLoader.MergeTableFormat = "MERGE INTO {0} AS Target " +
                                                   "USING {1} AS Source " +
                                                   "ON Target.ChannelID = Source.ChannelID " +
                                                   "WHEN MATCHED THEN " +
                                                   "    UPDATE SET " +
                                                   "        Average = Target.Average * (CAST(Target.Count AS FLOAT) / (Target.Count + Source.Count)) + Source.Average * (CAST(Source.Count AS FLOAT) / (Target.Count + Source.Count)), " +
                                                   "        MeanSquare = Target.MeanSquare * (CAST(Target.Count AS FLOAT) / (Target.Count + Source.Count)) + Source.MeanSquare * (CAST(Source.Count AS FLOAT) / (Target.Count + Source.Count)), " +
                                                   "        StandardDeviation = Target.MeanSquare * (CAST(Target.Count AS FLOAT) / (Target.Count + Source.Count)) + Source.MeanSquare * (CAST(Source.Count AS FLOAT) / (Target.Count + Source.Count)) - POWER(Target.Average * (CAST(Target.Count AS FLOAT) / (Target.Count + Source.Count)) + Source.Average * (CAST(Source.Count AS FLOAT) / (Target.Count + Source.Count)), 2), " +
                                                   "        Count = Target.Count + Source.Count " +
                                                   "WHEN NOT MATCHED THEN " +
                                                   "    INSERT (ChannelID, Average, MeanSquare, StandardDeviation, Count) " +
                                                   "    VALUES (Source.ChannelID, Source.Average, Source.MeanSquare, SQRT(Source.MeanSquare - Source.Average * Source.Average), Source.Count); ";

            hourlySummaryLoader.Load(m_hourlySummaryTable);
            channelNormalLoader.Load(m_channelNormalTable);

            Log.Info(string.Format("Loaded {0} hourly summary records into the database.", m_hourlySummaryTable.Count));
        }

        private void ProcessHourlySummaries(MeterDataSet meterDataSet)
        {
            Dictionary<Channel, List<TrendingDataSummaryResource.TrendingDataSummary>> trendingDataSummaries = meterDataSet.GetResource<TrendingDataSummaryResource>().TrendingDataSummaries;
            MeterData.HourlyTrendingSummaryRow row;

            List<TrendingDataSummaryResource.TrendingDataSummary> validSummaries;

            foreach (KeyValuePair<Channel, List<TrendingDataSummaryResource.TrendingDataSummary>> channelSummaries in trendingDataSummaries)
            {
                foreach (IGrouping<DateTime, TrendingDataSummaryResource.TrendingDataSummary> hourlySummary in channelSummaries.Value.GroupBy(summary => GetHour(summary.Time)))
                {
                    validSummaries = hourlySummary.Where(summary => summary.IsValid).ToList();

                    row = m_hourlySummaryTable.NewHourlyTrendingSummaryRow();

                    row.BeginEdit();
                    row.ChannelID = channelSummaries.Key.ID;
                    row.Time = hourlySummary.Key;
                    row.Minimum = validSummaries.Select(summary => summary.Minimum).DefaultIfEmpty(0.0D).Min();
                    row.Maximum = validSummaries.Select(summary => summary.Minimum).DefaultIfEmpty(0.0D).Max();
                    row.Average = validSummaries.Select(summary => summary.Minimum).DefaultIfEmpty(0.0D).Average();
                    row.ValidCount = validSummaries.Count;
                    row.InvalidCount = hourlySummary.Count() - validSummaries.Count;
                    row.EndEdit();

                    m_hourlySummaryTable.AddHourlyTrendingSummaryRow(row);
                }
            }
        }

        private void ProcessChannelNormals(MeterDataSet meterDataSet)
        {
            Dictionary<Channel, List<DataGroup>> trendingGroups;
            HourlyTrendingSummaryTableAdapter hourlySummaryAdapter;
            MeterData.HourlyTrendingSummaryDataTable hourlySummaryTable;

            Channel channel;
            int channelID;
            double average;
            double meanSquare;

            trendingGroups = meterDataSet.GetResource<TrendingGroupsResource>().TrendingGroups;
            hourlySummaryAdapter = m_dbAdapterContainer.GetAdapter<HourlyTrendingSummaryTableAdapter>();
            hourlySummaryTable = new MeterData.HourlyTrendingSummaryDataTable();

            hourlySummaryAdapter.ClearBeforeFill = false;

            foreach (KeyValuePair<Channel, List<DataGroup>> channelGroups in trendingGroups)
            {
                channel = channelGroups.Key;
                channelID = channel.ID;
                hourlySummaryTable.Clear();

                foreach (DataGroup dataGroup in channelGroups.Value)
                    hourlySummaryAdapter.FillBy(hourlySummaryTable, channelID, dataGroup.StartTime.AddHours(-1.0D), dataGroup.EndTime);

                for (int i = hourlySummaryTable.Count - 1; i >= 0; i--)
                {
                    if (hourlySummaryTable[i].ValidCount + hourlySummaryTable[i].InvalidCount < channel.SamplesPerHour)
                        hourlySummaryTable.Rows.RemoveAt(i);
                }

                RemoveDuplicateRows(hourlySummaryTable);

                if (hourlySummaryTable.Count > 0)
                {
                    average = hourlySummaryTable.Average(row => row.Average);
                    meanSquare = hourlySummaryTable.Average(row => row.Average * row.Average);

                    m_channelNormalTable.AddChannelNormalRow(channelID, average, meanSquare, 0.0D, hourlySummaryTable.Count);
                }
            }
        }

        private DateTime GetHour(DateTime time)
        {
            return new DateTime(time.Year, time.Month, time.Day, time.Hour, 0, 0);
        }

        private void RemoveDuplicateRows(MeterData.HourlyTrendingSummaryDataTable table)
        {
            HashSet<int> ids = new HashSet<int>();

            for (int i = table.Count - 1; i >= 0; i--)
            {
                if (!ids.Add(table[i].ID))
                    table.Rows.RemoveAt(i);
            }
        }

        #endregion

        #region [ Static ]

        // Static Fields
        private static readonly ILog Log = LogManager.GetLogger(typeof(HourlySummaryOperation));

        #endregion
    }
}
