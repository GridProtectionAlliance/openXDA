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
using GSF;

namespace FaultData.DataOperations
{
    public class HourlySummaryOperation : IDataOperation
    {
        #region [ Members ]

        // Events
        public event EventHandler<EventArgs<string>> StatusMessage;
        public event EventHandler<EventArgs<Exception>> ProcessException;

        // Fields
        private string m_connectionString;
        private BulkLoader m_hourlySummaryLoader;
        private BulkLoader m_channelNormalLoader;

        #endregion

        #region [ Constructors ]

        public HourlySummaryOperation(string connectionString)
        {
            m_connectionString = connectionString;
            m_hourlySummaryLoader = new BulkLoader();
            m_channelNormalLoader = new BulkLoader();

            m_hourlySummaryLoader.ConnectionString = connectionString;
            m_channelNormalLoader.ConnectionString = connectionString;

            m_hourlySummaryLoader.CreateTableFormat = "CREATE TABLE {0} " +
                                                      "( " +
                                                      "    ID INT, " +
                                                      "    ChannelID INT, " +
                                                      "    Time DATETIME, " +
                                                      "    Minimum FLOAT, " +
                                                      "    Maximum FLOAT, " +
                                                      "    Average FLOAT, " +
                                                      "    ValidCount INT, " +
                                                      "    InvalidCount INT " +
                                                      ")";

            m_hourlySummaryLoader.MergeTableFormat = "MERGE INTO {0} AS Target " +
                                                     "USING {1} AS Source " +
                                                     "ON Source.ChannelID = Target.ChannelID AND Source.Time = Target.Time " +
                                                     "WHEN MATCHED THEN " +
                                                     "    UPDATE SET " +
                                                     "        Maximum = IIF(Target.ValidCount = 0 OR Source.Maximum > Target.Maximum, Source.Maximum, Target.Maximum), " +
                                                     "        Minimum = IIF(Target.ValidCount = 0 OR Source.Minimum < Target.Minimum, Source.Minimum, Target.Minimum), " +
                                                     "        Average = IIF(Target.ValidCount = 0, Source.Average, Target.Average * (CAST(Target.ValidCount AS FLOAT) / (Target.ValidCount + Source.ValidCount)) + Source.Average * (CAST(Source.ValidCount AS FLOAT) / (Target.ValidCount + Source.ValidCount))), " +
                                                     "        ValidCount = Source.ValidCount + Target.ValidCount, " +
                                                     "        InvalidCount = Source.InvalidCount + Target.InvalidCount " +
                                                     "WHEN NOT MATCHED THEN " +
                                                     "    INSERT (ChannelID, Time, Maximum, Minimum, Average, ValidCount, InvalidCount) " +
                                                     "    VALUES (Source.ChannelID, Source.Time, Source.Maximum, Source.Minimum, Source.Average, Source.ValidCount, Source.InvalidCount);";

            m_channelNormalLoader.CreateTableFormat = "CREATE TABLE {0} " +
                                                      "( " +
                                                      "	   ID INT, " +
                                                      "    ChannelID INT, " +
                                                      "	   Average FLOAT, " +
                                                      "    MeanSquare FLOAT, " +
                                                      "    StandardDeviation FLOAT, " +
                                                      "    Count INT " +
                                                      ") ";

            m_channelNormalLoader.MergeTableFormat = "MERGE INTO {0} AS Target " +
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
        }

        #endregion

        #region [ Methods ]

        public void Execute(MeterDataSet meterDataSet)
        {
            LoadDataQualityRangeLimits(meterDataSet);
            LoadHourlySummaries(meterDataSet);
            LoadChannelNormals(meterDataSet);
        }

        private void LoadDataQualityRangeLimits(MeterDataSet meterDataSet)
        {
            Dictionary<Channel, List<TrendingDataSummaryResource.TrendingDataSummary>> trendingDataSummaries = meterDataSet.GetResource<TrendingDataSummaryResource>().TrendingDataSummaries;
            DataQuality.DefaultDataQualityRangeLimitDataTable defaultRangeLimitTable = new DataQuality.DefaultDataQualityRangeLimitDataTable();
            DataQuality.DataQualityRangeLimitDataTable rangeLimitTable = new DataQuality.DataQualityRangeLimitDataTable();
            Channel channel;

            TrendingDataSummaryResource.TrendingDataSummary previousSummary = null;

            double perUnitValue;
            double lowLimit;
            double highLimit;
            bool minValid;
            bool maxValid;

            using (DefaultDataQualityRangeLimitTableAdapter defaultRangeLimitAdapter = new DefaultDataQualityRangeLimitTableAdapter())
            using (DataQualityRangeLimitTableAdapter rangeLimitAdapter = new DataQualityRangeLimitTableAdapter())
            {
                defaultRangeLimitAdapter.Connection.ConnectionString = m_connectionString;
                rangeLimitAdapter.Connection.ConnectionString = m_connectionString;

                foreach (KeyValuePair<Channel, List<TrendingDataSummaryResource.TrendingDataSummary>> keyValuePair in trendingDataSummaries)
                {
                    channel = keyValuePair.Key;
                    perUnitValue = channel.PerUnitValue ?? 1.0D;
                    rangeLimitAdapter.FillBy(rangeLimitTable, channel.ID);

                    if (rangeLimitTable.Count == 0)
                    {
                        defaultRangeLimitAdapter.FillBy(defaultRangeLimitTable, channel.MeasurementTypeID, channel.MeasurementCharacteristicID);

                        foreach (DataQuality.DefaultDataQualityRangeLimitRow row in defaultRangeLimitTable)
                            rangeLimitTable.AddDataQualityRangeLimitRow(channel.ID, row.High, row.Low, row.RangeInclusive, row.PerUnit, 1);

                        using (SqlBulkCopy bulkCopy = new SqlBulkCopy(m_connectionString))
                        {
                            bulkCopy.BulkCopyTimeout = 0;
                            bulkCopy.DestinationTableName = rangeLimitTable.TableName;
                            bulkCopy.WriteToServer(rangeLimitTable);
                        }
                    }

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
        }

        private void LoadHourlySummaries(MeterDataSet meterDataSet)
        {
            Dictionary<Channel, List<TrendingDataSummaryResource.TrendingDataSummary>> trendingDataSummaries = meterDataSet.GetResource<TrendingDataSummaryResource>().TrendingDataSummaries;
            MeterData.HourlyTrendingSummaryDataTable hourlySummaryTable = new MeterData.HourlyTrendingSummaryDataTable();
            MeterData.HourlyTrendingSummaryRow row;

            List<TrendingDataSummaryResource.TrendingDataSummary> validSummaries;

            foreach (KeyValuePair<Channel, List<TrendingDataSummaryResource.TrendingDataSummary>> channelSummaries in trendingDataSummaries)
            {
                foreach (IGrouping<DateTime, TrendingDataSummaryResource.TrendingDataSummary> hourlySummary in channelSummaries.Value.GroupBy(summary => GetHour(summary.Time)))
                {
                    validSummaries = hourlySummary.Where(summary => summary.IsValid).ToList();

                    row = hourlySummaryTable.NewHourlyTrendingSummaryRow();

                    row.BeginEdit();
                    row.ChannelID = channelSummaries.Key.ID;
                    row.Time = hourlySummary.Key;
                    row.Minimum = validSummaries.Select(summary => summary.Minimum).DefaultIfEmpty(0.0D).Min();
                    row.Maximum = validSummaries.Select(summary => summary.Minimum).DefaultIfEmpty(0.0D).Max();
                    row.Average = validSummaries.Select(summary => summary.Minimum).DefaultIfEmpty(0.0D).Average();
                    row.ValidCount = validSummaries.Count;
                    row.InvalidCount = hourlySummary.Count() - validSummaries.Count;
                    row.EndEdit();

                    hourlySummaryTable.AddHourlyTrendingSummaryRow(row);
                }
            }

            // Bulk insert new rows
            m_hourlySummaryLoader.Load(hourlySummaryTable);
        }

        private void LoadChannelNormals(MeterDataSet meterDataSet)
        {
            Dictionary<Channel, List<DataGroup>> trendingGroups = meterDataSet.GetResource<TrendingGroupsResource>().TrendingGroups;
            MeterData.HourlyTrendingSummaryDataTable hourlySummaryTable = new MeterData.HourlyTrendingSummaryDataTable();
            MeterData.ChannelNormalDataTable channelNormalTable = new MeterData.ChannelNormalDataTable();

            Channel channel;
            int channelID;
            double average;
            double meanSquare;

            using (HourlyTrendingSummaryTableAdapter hourlySummaryAdapter = new HourlyTrendingSummaryTableAdapter())
            {
                hourlySummaryAdapter.Connection.ConnectionString = m_connectionString;
                hourlySummaryAdapter.ClearBeforeFill = false;

                channelNormalTable.Clear();

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

                        channelNormalTable.AddChannelNormalRow(channelID, average, meanSquare, 0.0D, hourlySummaryTable.Count);
                    }
                }

                m_channelNormalLoader.Load(channelNormalTable);
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
