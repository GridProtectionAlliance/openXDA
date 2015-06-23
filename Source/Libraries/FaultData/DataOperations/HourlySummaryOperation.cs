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
using System.Data.SqlClient;
using System.Linq;
using FaultData.DataAnalysis;
using FaultData.Database;
using FaultData.Database.DataQualityTableAdapters;
using FaultData.Database.MeterDataTableAdapters;
using FaultData.DataResources;
using FaultData.DataSets;
using log4net;

namespace FaultData.DataOperations
{
    public class HourlySummaryOperation : DataOperationBase<MeterDataSet>
    {
        #region [ Members ]

        // Fields
        private DbAdapterContainer m_dbAdapterContainer;
        private DataQuality.DataQualityRangeLimitDataTable m_rangeLimitTable;
        private MeterData.HourlyTrendingSummaryDataTable m_hourlySummaryTable;
        private MeterData.ChannelNormalDataTable m_channelNormalTable;

        #endregion

        #region [ Methods ]

        public override void Prepare(DbAdapterContainer dbAdapterContainer)
        {
            m_dbAdapterContainer = dbAdapterContainer;
            m_rangeLimitTable = new DataQuality.DataQualityRangeLimitDataTable();
            m_hourlySummaryTable = new MeterData.HourlyTrendingSummaryDataTable();
            m_channelNormalTable = new MeterData.ChannelNormalDataTable();
        }

        public override void Execute(MeterDataSet meterDataSet)
        {
            Log.Info("Executing operation to load hourly summary data into the database...");
            ProcessDataQualityRangeLimits(meterDataSet);
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

        private void ProcessDataQualityRangeLimits(MeterDataSet meterDataSet)
        {
            DataQuality.DataQualityRangeLimitDataTable rangeLimitTable;

            Dictionary<Channel, List<TrendingDataSummaryResource.TrendingDataSummary>> trendingDataSummaries;
            Channel channel;

            TrendingDataSummaryResource.TrendingDataSummary previousSummary = null;

            double perUnitValue;
            double lowLimit;
            double highLimit;
            bool minValid;
            bool maxValid;

            trendingDataSummaries = meterDataSet.GetResource<TrendingDataSummaryResource>().TrendingDataSummaries;
            rangeLimitTable = new DataQuality.DataQualityRangeLimitDataTable();

            foreach (KeyValuePair<Channel, List<TrendingDataSummaryResource.TrendingDataSummary>> keyValuePair in trendingDataSummaries)
            {
                channel = keyValuePair.Key;
                perUnitValue = channel.PerUnitValue ?? 1.0D;
                InitializeRangeLimitTable(rangeLimitTable, channel);

                foreach (TrendingDataSummaryResource.TrendingDataSummary trendingDataSummary in keyValuePair.Value)
                {
                    if ((object)previousSummary != null && trendingDataSummary.Minimum == previousSummary.Minimum && trendingDataSummary.Average == previousSummary.Average && trendingDataSummary.Maximum == previousSummary.Maximum)
                        trendingDataSummary.Latched = true;

                    if (trendingDataSummary.Average < trendingDataSummary.Minimum || trendingDataSummary.Average > trendingDataSummary.Maximum)
                        trendingDataSummary.NonCongruent = true;

                    foreach (DataQuality.DataQualityRangeLimitRow row in rangeLimitTable.Where(row => row.Enabled != 0))
                    {
                        highLimit = 0.0D;
                        lowLimit = 0.0D;
                        maxValid = true;
                        minValid = true;

                        if (!row.IsHighNull())
                        {
                            highLimit = Convert.ToBoolean(row.PerUnit) ? row.High * perUnitValue : row.High;
                            maxValid = Convert.ToBoolean(row.RangeInclusive) ^ (trendingDataSummary.Maximum < highLimit);
                        }

                        if (!row.IsLowNull())
                        {
                            lowLimit = Convert.ToBoolean(row.PerUnit) ? row.Low * perUnitValue : row.Low;
                            minValid = Convert.ToBoolean(row.RangeInclusive) ^ (trendingDataSummary.Minimum > lowLimit);
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

        private void InitializeRangeLimitTable(DataQuality.DataQualityRangeLimitDataTable rangeLimitTable, Channel channel)
        {
            DataQualityRangeLimitTableAdapter rangeLimitAdapter;
            DefaultDataQualityRangeLimitTableAdapter defaultRangeLimitAdapter;
            DataQuality.DefaultDataQualityRangeLimitDataTable defaultRangeLimitTable;

            // Clear existing rows from the range limit table
            rangeLimitTable.Clear();

            // Fill the range limit table with range limits for the given channel
            rangeLimitAdapter = m_dbAdapterContainer.GetAdapter<DataQualityRangeLimitTableAdapter>();
            rangeLimitAdapter.FillBy(rangeLimitTable, channel.ID);

            // If limits exist for the given channel,
            // range limit table has been successfully initialized
            if (rangeLimitTable.Count != 0)
                return;

            // Get the default range limits for the measurement type and characteristic of this channel
            defaultRangeLimitAdapter = m_dbAdapterContainer.GetAdapter<DefaultDataQualityRangeLimitTableAdapter>();
            defaultRangeLimitTable = defaultRangeLimitAdapter.GetDataBy(channel.MeasurementTypeID, channel.MeasurementCharacteristicID);

            // If there are no default limits for the channel,
            // then the range limit table has been successfully initialized
            if (defaultRangeLimitTable.Count == 0)
                return;

            lock (RangeLimitLock)
            {
                // Fill the range limit table one more time inside the lock to
                // ensure that no other threads have written limits for this channel
                rangeLimitAdapter.FillBy(rangeLimitTable, channel.ID);

                // If there are still no limits defined for this channel,
                // update the table to include this channel's default limits
                if (rangeLimitTable.Count == 0)
                {
                    foreach (DataQuality.DefaultDataQualityRangeLimitRow row in defaultRangeLimitTable)
                        rangeLimitTable.AddDataQualityRangeLimitRow(channel.ID, row.High, row.Low, row.RangeInclusive, row.PerUnit, 1);

                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(m_dbAdapterContainer.Connection))
                    {
                        bulkCopy.BulkCopyTimeout = 0;
                        bulkCopy.DestinationTableName = m_rangeLimitTable.TableName;
                        bulkCopy.WriteToServer(rangeLimitTable);
                    }
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
        private static readonly object RangeLimitLock = new object();
        private static readonly ILog Log = LogManager.GetLogger(typeof(HourlySummaryOperation));

        #endregion
    }
}
