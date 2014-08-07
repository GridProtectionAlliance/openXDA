//******************************************************************************************************
//  AlarmOperation.cs - Gbtc
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
//  07/23/2014 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using FaultData.DataAnalysis;
using FaultData.Database;
using FaultData.Database.AlarmDataTableAdapters;
using FaultData.Database.MeterDataTableAdapters;

namespace FaultData.DataOperations
{
    public class AlarmOperation : IDataOperation
    {
        #region [ Members ]

        // Nested Types
        private class HourlySummaryRowComparer : IEqualityComparer<MeterData.HourlyTrendingSummaryRow>
        {
            private HourlySummaryRowComparer()
            {
            }

            public bool Equals(MeterData.HourlyTrendingSummaryRow x, MeterData.HourlyTrendingSummaryRow y)
            {
                return x.ID == y.ID;
            }

            public int GetHashCode(MeterData.HourlyTrendingSummaryRow obj)
            {
                return obj.ID.GetHashCode();
            }

            public static readonly HourlySummaryRowComparer Default = new HourlySummaryRowComparer();
        }

        // Constants
        private static readonly double Sqrt3 = Math.Sqrt(3.0D);

        // Fields
        private string m_connectionString;
        private BulkLoader m_alarmLimitLoader;
        private BulkLoader m_offNormalLimitLoader;
        private BulkLoader m_alarmLogLoader;
        private BulkLoader m_offNormalLogLoader;

        #endregion

        #region [ Constructors ]

        public AlarmOperation(string connectionString)
        {
            m_connectionString = connectionString;
            m_alarmLimitLoader = new BulkLoader();
            m_offNormalLimitLoader = new BulkLoader();
            m_alarmLogLoader = new BulkLoader();
            m_offNormalLogLoader = new BulkLoader();

            m_alarmLimitLoader.ConnectionString = connectionString;
            m_offNormalLimitLoader.ConnectionString = connectionString;
            m_alarmLogLoader.ConnectionString = connectionString;
            m_offNormalLogLoader.ConnectionString = connectionString;

            m_alarmLimitLoader.CreateTableFormat = "CREATE TABLE {0} " +
                                                   "( " +
                                                   "    ID INT, " +
                                                   "    ChannelID INT, " +
                                                   "    High FLOAT, " +
                                                   "    Low FLOAT, " +
                                                   "    RangeInclusive INT, " +
                                                   "    Enabled INT " +
                                                   ")";

            m_alarmLimitLoader.MergeTableFormat = "MERGE INTO {0} AS Target " +
                                                  "USING {1} AS Source " +
                                                  "ON Source.ChannelID = Target.ChannelID " +
                                                  "WHEN NOT MATCHED THEN " +
                                                  "    INSERT (ChannelID, High, Low, RangeInclusive, Enabled) " +
                                                  "    VALUES (Source.ChannelID, Source.High, Source.Low, Source.RangeInclusive, Source.Enabled);";

            m_offNormalLimitLoader.CreateTableFormat = "CREATE TABLE {0} " +
                                                       "( " +
                                                       "    ID INT, " +
                                                       "    ChannelID INT, " +
                                                       "    HourOfWeek INT, " +
                                                       "    High FLOAT, " +
                                                       "    Low FLOAT, " +
                                                       "    Count INT, " +
                                                       "    Enabled INT, " +
                                                       ")";

            m_offNormalLimitLoader.MergeTableFormat = "MERGE INTO {0} AS Target " +
                                                      "USING {1} AS Source " +
                                                      "ON Source.ChannelID = Target.ChannelID AND Source.HourOfWeek = Target.HourOfWeek " +
                                                      "WHEN MATCHED THEN " +
                                                      "    UPDATE SET " +
                                                      "        High = Source.High, " +
                                                      "        Low = Source.Low, " +
                                                      "        Count = Source.Count " +
                                                      "WHEN NOT MATCHED THEN " +
                                                      "    INSERT (ChannelID, HourOfWeek, High, Low, Count, Enabled) " +
                                                      "    VALUES (Source.ChannelID, Source.HourOfWeek, Source.High, Source.Low, Source.Count, Source.Enabled);";

            m_alarmLogLoader.CreateTableFormat = "CREATE TABLE {0} " +
                                                 "( " +
                                                 "    ID INT, " +
                                                 "    ChannelID INT, " +
                                                 "    Time DATETIME, " +
                                                 "    LimitHigh FLOAT, " +
                                                 "    LimitLow FLOAT, " +
                                                 "    ValueHigh FLOAT, " +
                                                 "    ValueLow FLOAT " +
                                                 ")";

            m_alarmLogLoader.MergeTableFormat = "MERGE INTO {0} AS Target " +
                                                "USING {1} AS Source " +
                                                "ON Source.ChannelID = Target.ChannelID AND Source.Time = Target.Time " +
                                                "WHEN MATCHED THEN " +
                                                "    UPDATE SET " +
                                                "        LimitHigh = Source.LimitHigh, " +
                                                "        LimitLow = Source.LimitLow, " +
                                                "        ValueHigh = IIF(Source.ValueHigh > Target.ValueHigh, Source.ValueHigh, Target.ValueHigh), " +
                                                "        ValueLow = IIF(Source.ValueLow < Target.ValueLow, Source.ValueLow, Target.ValueLow) " +
                                                "WHEN NOT MATCHED THEN " +
                                                "    INSERT (ChannelID, Time, LimitHigh, LimitLow, ValueHigh, ValueLow) " +
                                                "    VALUES (Source.ChannelID, Source.Time, Source.LimitHigh, Source.LimitLow, Source.ValueHigh, Source.ValueLow);";

            m_offNormalLogLoader.CreateTableFormat = "CREATE TABLE {0} " +
                                                     "( " +
                                                     "    ID INT, " +
                                                     "    ChannelID INT, " +
                                                     "    Time DATETIME, " +
                                                     "    LimitHigh FLOAT, " +
                                                     "    LimitLow FLOAT, " +
                                                     "    ValueHigh FLOAT, " +
                                                     "    ValueLow FLOAT " +
                                                     ")";

            m_offNormalLogLoader.MergeTableFormat = "MERGE INTO {0} AS Target " +
                                                    "USING {1} AS Source " +
                                                    "ON Source.ChannelID = Target.ChannelID AND Source.Time = Target.Time " +
                                                    "WHEN MATCHED THEN " +
                                                    "    UPDATE SET " +
                                                    "        LimitHigh = Source.LimitHigh, " +
                                                    "        LimitLow = Source.LimitLow, " +
                                                    "        ValueHigh = IIF(Source.ValueHigh > Target.ValueHigh, Source.ValueHigh, Target.ValueHigh), " +
                                                    "        ValueLow = IIF(Source.ValueLow < Target.ValueLow, Source.ValueLow, Target.ValueLow) " +
                                                    "WHEN NOT MATCHED THEN " +
                                                    "    INSERT (ChannelID, Time, LimitHigh, LimitLow, ValueHigh, ValueLow) " +
                                                    "    VALUES (Source.ChannelID, Source.Time, Source.LimitHigh, Source.LimitLow, Source.ValueHigh, Source.ValueLow);";
        }

        #endregion

        #region [ Properties ]

        #endregion

        #region [ Methods ]

        public void Execute(MeterDataSet meterDataSet)
        {
            AlarmData.OffNormalLimitDataTable offNormalLimitTable;
            AlarmData.AlarmLogDataTable alarmLogTable;
            AlarmData.OffNormalLogDataTable offNormalLogTable;

            Dictionary<Channel, List<DataGroup>> trendingGroups;
            Dictionary<Channel, List<TrendingDataSummaryResource.TrendingDataSummary>> trendingDataSummaries;

            Channel channel;
            List<DataGroup> dataGroups;
            List<TrendingDataSummaryResource.TrendingDataSummary> summaryList;
            List<AlarmData.OffNormalLimitRow> channelLimits;
            List<Tuple<MeterData.HourlyTrendingSummaryRow, List<TrendingDataSummaryResource.TrendingDataSummary>, AlarmData.OffNormalLimitRow>> hourlyDataGroupings;

            offNormalLimitTable = new AlarmData.OffNormalLimitDataTable();
            offNormalLogTable = new AlarmData.OffNormalLogDataTable();
            alarmLogTable = new AlarmData.AlarmLogDataTable();

            trendingGroups = meterDataSet.GetResource<TrendingGroupsResource>().TrendingGroups;
            trendingDataSummaries = meterDataSet.GetResource<TrendingDataSummaryResource>().TrendingDataSummaries;

            UpdateAlarmLimits(trendingDataSummaries.Keys.ToList());

            foreach (KeyValuePair<Channel, List<TrendingDataSummaryResource.TrendingDataSummary>> trendingDataSummary in trendingDataSummaries)
            {
                channel = trendingDataSummary.Key;

                if (!trendingGroups.TryGetValue(channel, out dataGroups))
                    continue;

                summaryList = trendingDataSummary.Value;

                channelLimits = GetOffNormalLimits(channel, offNormalLimitTable);

                hourlyDataGroupings = GetHourlySummaries(channel, dataGroups)
                    .Distinct(HourlySummaryRowComparer.Default)
                    .Where(hourlySummary => Math.Abs(GetSamplesPerHour(dataGroups.First()) - hourlySummary.Count) < 1.0D)
                    .GroupJoin(summaryList, hourlySummary => hourlySummary.Time, trendingSummary => GetHour(trendingSummary.Time), Tuple.Create)
                    .GroupJoin(channelLimits, tuple => GetHourOfWeek(tuple.Item1.Time), offNormalLimitRow => offNormalLimitRow.HourOfWeek, (tuple, offNormalLimitRows) => Tuple.Create(tuple.Item1, tuple.Item2.ToList(), offNormalLimitRows.FirstOrDefault()))
                    .ToList();

                UpdateOffNormalLimits(hourlyDataGroupings);
                UpdateAlarmLogs(channel, hourlyDataGroupings, alarmLogTable, offNormalLogTable);

                // Now that the off-normal limits have been updated by the UpdateOffNormalLimits method,
                // ensure that the rows which do not already belong to the offNormalLimitTable are
                // imported into the offNormalLimitTable
                if (channelLimits.First().Table != offNormalLimitTable)
                {
                    foreach (AlarmData.OffNormalLimitRow row in channelLimits)
                    {
                        row.EndEdit();
                        offNormalLimitTable.ImportRow(row);
                    }
                }
            }

            m_offNormalLimitLoader.Load(offNormalLimitTable);
            m_alarmLogLoader.Load(alarmLogTable);
            m_offNormalLogLoader.Load(offNormalLogTable);
        }

        private void UpdateAlarmLimits(List<Channel> channels)
        {
            AlarmData.AlarmLimitDataTable alarmLimitTable = new AlarmData.AlarmLimitDataTable();
            AlarmData.AlarmLimitRow alarmLimits;

            int channelID;
            double nominalVoltage;

            foreach (Channel channel in channels)
            {
                channelID = channel.ID;
                nominalVoltage = channel.Line.VoltageKV * 1000.0D;

                if (channel.MeasurementType.Name != "Voltage")
                    continue;

                switch (channel.MeasurementCharacteristic.Name)
                {
                    case "AvgImbal":
                        alarmLimits = alarmLimitTable.AddAlarmLimitRow(channelID, 3.0D, 0.0D, 0, 1);
                        alarmLimits.SetLowNull();
                        break;

                    case "FlkrPLT":
                        alarmLimits = alarmLimitTable.AddAlarmLimitRow(channelID, 0.8D, 0.0D, 0, 1);
                        alarmLimits.SetLowNull();
                        break;

                    case "FlkrPST":
                        alarmLimits = alarmLimitTable.AddAlarmLimitRow(channelID, 1.0D, 0.0D, 0, 1);
                        alarmLimits.SetLowNull();
                        break;

                    case "Frequency":
                        alarmLimitTable.AddAlarmLimitRow(channelID, 60.01, 59.99, 0, 1);
                        break;

                    case "RMS":
                        alarmLimits = alarmLimitTable.AddAlarmLimitRow(channelID, 1.1D * nominalVoltage, 0.9D * nominalVoltage, 0, 1);

                        if (channel.Phase.Name.Contains("N"))
                        {
                            alarmLimits.High /= Sqrt3;
                            alarmLimits.Low /= Sqrt3;
                        }

                        break;

                    case "SNeg":
                        alarmLimits = alarmLimitTable.AddAlarmLimitRow(channelID, 0.02 * nominalVoltage, 0.0D, 0, 1);
                        alarmLimits.SetLowNull();
                        break;

                    case "SpectraHGroup":
                        if (channel.HarmonicGroup > 0)
                        {
                            alarmLimits = alarmLimitTable.AddAlarmLimitRow(channelID, 0.03 * nominalVoltage, 0.0D, 0, 1);
                            alarmLimits.SetLowNull();
                        }

                        break;

                    case "TotalTHD":
                    case "TotalTHDRMS":
                        alarmLimits = alarmLimitTable.AddAlarmLimitRow(channelID, 5.0D, 0.0D, 0, 1);
                        alarmLimits.SetLowNull();
                        break;
                }
            }

            m_alarmLimitLoader.Load(alarmLimitTable);
        }

        private void UpdateOffNormalLimits(List<Tuple<MeterData.HourlyTrendingSummaryRow, List<TrendingDataSummaryResource.TrendingDataSummary>, AlarmData.OffNormalLimitRow>> hourlyDataGroupings)
        {
            MeterData.HourlyTrendingSummaryRow hourlySummary;
            AlarmData.OffNormalLimitRow offNormalLimits;

            foreach (Tuple<MeterData.HourlyTrendingSummaryRow, List<TrendingDataSummaryResource.TrendingDataSummary>, AlarmData.OffNormalLimitRow> hourOfData in hourlyDataGroupings)
            {
                offNormalLimits = hourOfData.Item3;

                if ((object)offNormalLimits != null)
                {
                    hourlySummary = hourOfData.Item1;

                    offNormalLimits.BeginEdit();
                    offNormalLimits.Count++;
                    offNormalLimits.High = offNormalLimits.High + (hourlySummary.Maximum - offNormalLimits.High) / offNormalLimits.Count;
                    offNormalLimits.Low = offNormalLimits.Low + (hourlySummary.Minimum - offNormalLimits.Low) / offNormalLimits.Count;
                }
            }
        }

        private void UpdateAlarmLogs(Channel channel, List<Tuple<MeterData.HourlyTrendingSummaryRow, List<TrendingDataSummaryResource.TrendingDataSummary>, AlarmData.OffNormalLimitRow>> hourlyDataGroupings, AlarmData.AlarmLogDataTable alarmLogTable, AlarmData.OffNormalLogDataTable offNormalLogTable)
        {
            List<TrendingDataSummaryResource.TrendingDataSummary> trendingSummaries;
            MeterData.HourlyTrendingSummaryRow hourlySummary;
            AlarmData.AlarmLimitRow alarmLimits;
            AlarmData.OffNormalLimitRow offNormalLimits;
            AlarmData.AlarmLogRow alarmLog;

            double maxDataPoint;
            double minDataPoint;
            bool alarm;

            alarmLimits = GetAlarmLimits(channel);

            foreach (Tuple<MeterData.HourlyTrendingSummaryRow, List<TrendingDataSummaryResource.TrendingDataSummary>, AlarmData.OffNormalLimitRow> hourlyDataGrouping in hourlyDataGroupings)
            {
                hourlySummary = hourlyDataGrouping.Item1;

                // Ensure that alarm limits exist for this channel
                if ((object)alarmLimits != null && alarmLimits.Enabled != 0)
                {
                    // Determine whether the alarm limits were exceeded
                    alarm = (!alarmLimits.IsHighNull() && hourlySummary.Maximum > alarmLimits.High) ||
                            (!alarmLimits.IsLowNull() && hourlySummary.Minimum < alarmLimits.Low);

                    if (alarm)
                    {
                        // If the alarm limits were exceeded,
                        // add a row to the AlarmLog table for that hour
                        alarmLog = alarmLogTable.NewAlarmLogRow();

                        alarmLog.ChannelID = channel.ID;
                        alarmLog.Time = hourlySummary.Time;
                        alarmLog.ValueHigh = hourlySummary.Maximum;
                        alarmLog.ValueLow = hourlySummary.Minimum;

                        if (!alarmLimits.IsHighNull())
                            alarmLog.LimitHigh = alarmLimits.High;

                        if (!alarmLimits.IsLowNull())
                            alarmLog.LimitLow = alarmLimits.Low;

                        alarmLogTable.AddAlarmLogRow(alarmLog);
                    }
                }

                offNormalLimits = hourlyDataGrouping.Item3;

                // Ensure that off-normal limits exist for this channel and hour of the week
                if ((object)offNormalLimits != null && offNormalLimits.Enabled != 0)
                {
                    // Get the data points in this hour
                    trendingSummaries = hourlyDataGrouping.Item2.ToList();

                    if (trendingSummaries.Count > 0)
                    {
                        // Determine the maximum and minimum values of the data point during the hour
                        maxDataPoint = trendingSummaries.Max(trendingSummary => trendingSummary.Average);
                        minDataPoint = trendingSummaries.Min(trendingSummary => trendingSummary.Average);

                        // Determine whether the off-normal limits were exceeded
                        if (maxDataPoint > offNormalLimits.High || minDataPoint < offNormalLimits.Low)
                            offNormalLogTable.AddOffNormalLogRow(channel.ID, hourlySummary.Time, offNormalLimits.High, offNormalLimits.Low, maxDataPoint, minDataPoint);
                    }
                }
            }
        }

        private MeterData.HourlyTrendingSummaryDataTable GetHourlySummaries(Channel channel, List<DataGroup> dataGroups)
        {
            MeterData.HourlyTrendingSummaryDataTable hourlySummaryTable = new MeterData.HourlyTrendingSummaryDataTable();

            using (HourlyTrendingSummaryTableAdapter hourlySummaryAdapter = new HourlyTrendingSummaryTableAdapter())
            {
                hourlySummaryAdapter.Connection.ConnectionString = m_connectionString;
                hourlySummaryAdapter.ClearBeforeFill = false;

                hourlySummaryTable.BeginLoadData();

                foreach (DataGroup dataGroup in dataGroups)
                    hourlySummaryAdapter.FillBy(hourlySummaryTable, channel.ID, dataGroup.StartTime.AddHours(-1.0D), dataGroup.EndTime);

                hourlySummaryTable.EndLoadData();
            }

            return hourlySummaryTable;
        }

        private AlarmData.AlarmLimitRow GetAlarmLimits(Channel channel)
        {
            using (AlarmLimitTableAdapter alarmLimitAdapter = new AlarmLimitTableAdapter())
            {
                alarmLimitAdapter.Connection.ConnectionString = m_connectionString;
                return alarmLimitAdapter.GetDataBy(channel.ID).FirstOrDefault();
            }
        }

        // The goal here is to return the OffNormalLimitRows for the given channel, but also to consolidate that with the offNormalLimitTable.
        // Unfortunately, when creating new rows in a table with autoincrementing IDs, the table must maintain the uniqueness constraint and
        // will therefore assign IDs starting at -1 and decreasing. Therefore, new rows cannot be created in the queriedRows table because the
        // IDs would conflict with the ones in the offNormalLimitTable. On the other hand, if the query does return results, we cannot immediately
        // import rows into the offNormalLimitTable because the values might change in the UpdateOffNormalLimits method. As a result, the rows
        // returned by this method may belong to the queriedRows table or the offNormalLimitTable.
        private List<AlarmData.OffNormalLimitRow> GetOffNormalLimits(Channel channel, AlarmData.OffNormalLimitDataTable offNormalLimitTable)
        {
            List<AlarmData.OffNormalLimitRow> offNormalLimits;
            AlarmData.OffNormalLimitDataTable queriedRows;
            AlarmData.OffNormalLimitRow newRow;

            using (OffNormalLimitTableAdapter offNormalLimitAdapter = new OffNormalLimitTableAdapter())
            {
                offNormalLimitAdapter.Connection.ConnectionString = m_connectionString;
                queriedRows = offNormalLimitAdapter.GetDataBy(channel.ID);
            }

            if (queriedRows.Count > 0)
            {
                offNormalLimits = queriedRows.ToList();
            }
            else
            {
                offNormalLimits = new List<AlarmData.OffNormalLimitRow>();

                for (int i = 0; i < 168; i++)
                {
                    newRow = offNormalLimitTable.AddOffNormalLimitRow(channel.ID, i, 0.0D, 0.0D, 0, 1);
                    offNormalLimits.Add(newRow);
                }
            }

            return offNormalLimits;
        }

        private DateTime GetHour(DateTime time)
        {
            return new DateTime(time.Year, time.Month, time.Day, time.Hour, 0, 0);
        }

        private int GetHourOfWeek(DateTime time)
        {
            return (int)time.DayOfWeek * 24 + time.Hour;
        }

        private double GetSamplesPerHour(DataGroup dataGroup)
        {
            return dataGroup.Samples / Math.Round((dataGroup.EndTime - dataGroup.StartTime).TotalHours);
        }

        #endregion
    }
}
