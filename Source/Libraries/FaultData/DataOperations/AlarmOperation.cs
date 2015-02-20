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
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using FaultData.DataAnalysis;
using FaultData.Database;
using FaultData.Database.AlarmDataTableAdapters;
using FaultData.Database.MeterDataTableAdapters;
using FaultData.DataResources;
using FaultData.DataSets;
using GSF;

namespace FaultData.DataOperations
{
    public class AlarmOperation : IDataOperation
    {
        #region [ Members ]

        // Events
        public event EventHandler<EventArgs<string>> StatusMessage;
        public event EventHandler<EventArgs<Exception>> ProcessException;

        // Fields
        private string m_connectionString;
        private AlarmData.AlarmTypeDataTable m_alarmTypeTable;
        private AlarmData.AlarmLogDataTable m_alarmLogTable;

        #endregion

        #region [ Constructors ]

        public AlarmOperation(string connectionString)
        {
            m_connectionString = connectionString;
            m_alarmTypeTable = new AlarmData.AlarmTypeDataTable();
            m_alarmLogTable = new AlarmData.AlarmLogDataTable();
        }

        #endregion

        #region [ Methods ]

        public void Execute(MeterDataSet meterDataSet)
        {
            LoadAlarmTypes();
            LoadDataQualityAlarmLogs(meterDataSet);
            LoadRangeLimits(meterDataSet);
            LoadHourOfWeekLimits(meterDataSet);
            LoadAlarmLogs();
        }

        private void LoadAlarmTypes()
        {
            using (AlarmTypeTableAdapter alarmTypeAdapter = new AlarmTypeTableAdapter())
            {
                alarmTypeAdapter.Connection.ConnectionString = m_connectionString;
                alarmTypeAdapter.Fill(m_alarmTypeTable);
            }
        }

        private void LoadDataQualityAlarmLogs(MeterDataSet meterDataSet)
        {
            const int Latched = 0;
            const int NonCongruent = 1;
            const int Unreasonable = 2;

            Dictionary<int, int> alarmTypeLookup = new Dictionary<int, int>();
            Dictionary<Channel, List<TrendingDataSummaryResource.TrendingDataSummary>> trendingDataSummaries = meterDataSet.GetResource<TrendingDataSummaryResource>().TrendingDataSummaries;
            int channelID;

            alarmTypeLookup.Add(Latched, m_alarmTypeTable.First(alarmType => alarmType.Name == "Latched").ID);
            alarmTypeLookup.Add(NonCongruent, m_alarmTypeTable.First(alarmType => alarmType.Name == "Non-congruent").ID);
            alarmTypeLookup.Add(Unreasonable, m_alarmTypeTable.First(alarmType => alarmType.Name == "Unreasonable").ID);

            foreach (KeyValuePair<Channel, List<TrendingDataSummaryResource.TrendingDataSummary>> trendingDataSummary in trendingDataSummaries)
            {
                channelID = trendingDataSummary.Key.ID;

                foreach (TrendingDataSummaryResource.TrendingDataSummary summary in trendingDataSummary.Value)
                {
                    if (summary.IsValid)
                        continue;

                    if (summary.Latched)
                        m_alarmLogTable.AddAlarmLogRow(channelID, alarmTypeLookup[Latched], summary.Time, 0, 0.0D, 0.0D, summary.Average);

                    if (summary.NonCongruent)
                        m_alarmLogTable.AddAlarmLogRow(channelID, alarmTypeLookup[NonCongruent], summary.Time, 0, summary.Maximum, summary.Minimum, summary.Average);

                    if (summary.Unreasonable)
                        m_alarmLogTable.AddAlarmLogRow(channelID, alarmTypeLookup[Unreasonable], summary.Time, 0, summary.HighLimit, summary.LowLimit, summary.UnreasonableValue);
                }
            }
        }

        private void LoadRangeLimits(MeterDataSet meterDataSet)
        {
            Dictionary<Channel, List<DataGroup>> trendingGroups = meterDataSet.GetResource<TrendingGroupsResource>().TrendingGroups;
            MeterData.HourlyTrendingSummaryDataTable hourlySummaryTable = new MeterData.HourlyTrendingSummaryDataTable();
            AlarmData.DefaultAlarmRangeLimitDataTable defaultRangeLimitTable = new AlarmData.DefaultAlarmRangeLimitDataTable();
            AlarmData.AlarmRangeLimitDataTable rangeLimitTable = new AlarmData.AlarmRangeLimitDataTable();

            IEnumerable<AlarmData.DefaultAlarmRangeLimitRow> undefinedDefaults;
            AlarmData.AlarmRangeLimitRow limitRow;
            AlarmData.AlarmLogRow logRow;

            Channel channel;
            int channelID;

            double perUnitValue;
            double highLimit;
            double lowLimit;
            bool highValid;
            bool lowValid;

            using (DefaultAlarmRangeLimitTableAdapter defaultRangeLimitAdapter = new DefaultAlarmRangeLimitTableAdapter())
            using (AlarmRangeLimitTableAdapter rangeLimitAdapter = new AlarmRangeLimitTableAdapter())
            using (HourlyTrendingSummaryTableAdapter hourlySummaryAdapter = new HourlyTrendingSummaryTableAdapter())
            {
                defaultRangeLimitAdapter.Connection.ConnectionString = m_connectionString;
                rangeLimitAdapter.Connection.ConnectionString = m_connectionString;
                hourlySummaryAdapter.Connection.ConnectionString = m_connectionString;
                hourlySummaryAdapter.ClearBeforeFill = false;

                foreach (KeyValuePair<Channel, List<DataGroup>> channelGroups in trendingGroups)
                {
                    channel = channelGroups.Key;
                    channelID = channel.ID;
                    perUnitValue = channel.PerUnitValue ?? 1.0D;

                    defaultRangeLimitAdapter.FillBy(defaultRangeLimitTable, channel.MeasurementTypeID, channel.MeasurementCharacteristicID);
                    rangeLimitAdapter.FillBy(rangeLimitTable, channelID);

                    // Get range limits for this channel
                    undefinedDefaults = defaultRangeLimitTable
                        .GroupJoin(rangeLimitTable, row => row.AlarmTypeID, row => row.AlarmTypeID, (row, rows) => Tuple.Create(row, rows.Any()))
                        .Where(tuple => !tuple.Item2)
                        .Select(tuple => tuple.Item1);

                    foreach (AlarmData.DefaultAlarmRangeLimitRow defaultRow in undefinedDefaults)
                    {
                        limitRow = rangeLimitTable.NewAlarmRangeLimitRow();

                        limitRow.ChannelID = channelID;
                        limitRow.AlarmTypeID = defaultRow.AlarmTypeID;
                        limitRow.Severity = defaultRow.Severity;
                        limitRow.RangeInclusive = defaultRow.RangeInclusive;
                        limitRow.PerUnit = defaultRow.PerUnit;
                        limitRow.Enabled = 1;

                        if (!defaultRow.IsHighNull())
                            limitRow.High = defaultRow.High;

                        if (!defaultRow.IsLowNull())
                            limitRow.Low = defaultRow.Low;

                        rangeLimitTable.AddAlarmRangeLimitRow(limitRow);
                    }

                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(m_connectionString))
                    {
                        bulkCopy.BulkCopyTimeout = 0;
                        bulkCopy.DestinationTableName = rangeLimitTable.TableName;
                        bulkCopy.WriteToServer(Enumerable.Where(rangeLimitTable, row => row.ID <= 0).ToArray<DataRow>());
                    }

                    // Get hourly summary records for these data groups
                    hourlySummaryTable.Clear();

                    foreach (DataGroup dataGroup in channelGroups.Value)
                        hourlySummaryAdapter.FillBy(hourlySummaryTable, channelID, dataGroup.StartTime, dataGroup.EndTime);

                    for (int i = hourlySummaryTable.Count - 1; i >= 0; i--)
                    {
                        if (hourlySummaryTable[i].ValidCount + hourlySummaryTable[i].InvalidCount < channel.SamplesPerHour)
                            hourlySummaryTable.Rows.RemoveAt(i);
                    }

                    RemoveDuplicateRows(hourlySummaryTable);

                    // Update alarm log for each excursion
                    foreach (MeterData.HourlyTrendingSummaryRow hourlySummary in hourlySummaryTable)
                    {
                        foreach (AlarmData.AlarmRangeLimitRow rangeLimit in Enumerable.Where(rangeLimitTable, row => row.Enabled != 0))
                        {
                            highLimit = 0.0D;
                            lowLimit = 0.0D;
                            highValid = true;
                            lowValid = true;

                            if (!rangeLimit.IsHighNull())
                            {
                                highLimit = Convert.ToBoolean(rangeLimit.PerUnit) ? rangeLimit.High * perUnitValue : rangeLimit.High;
                                highValid = Convert.ToBoolean(rangeLimit.RangeInclusive) ^ (hourlySummary.Average < highLimit);
                            }

                            if (!rangeLimit.IsLowNull())
                            {
                                lowLimit = Convert.ToBoolean(rangeLimit.PerUnit) ? rangeLimit.Low * perUnitValue : rangeLimit.Low;
                                lowValid = Convert.ToBoolean(rangeLimit.RangeInclusive) ^ (hourlySummary.Average < lowLimit);
                            }

                            if (!lowValid || !highValid)
                            {
                                logRow = m_alarmLogTable.NewAlarmLogRow();

                                logRow.ChannelID = channelID;
                                logRow.AlarmTypeID = rangeLimit.AlarmTypeID;
                                logRow.Time = hourlySummary.Time;
                                logRow.Severity = rangeLimit.Severity;
                                logRow.Value = hourlySummary.Average;

                                if (!rangeLimit.IsHighNull())
                                    logRow.LimitHigh = highLimit;

                                if (!rangeLimit.IsLowNull())
                                    logRow.LimitLow = lowLimit;

                                m_alarmLogTable.AddAlarmLogRow(logRow);
                            }
                        }
                    }
                }
            }
        }

        private void LoadHourOfWeekLimits(MeterDataSet meterDataSet)
        {
            Dictionary<Channel, List<DataGroup>> trendingGroups = meterDataSet.GetResource<TrendingGroupsResource>().TrendingGroups;
            MeterData.HourlyTrendingSummaryDataTable hourlySummaryTable = new MeterData.HourlyTrendingSummaryDataTable();
            AlarmData.HourOfWeekLimitDataTable hourlyLimitTable = new AlarmData.HourOfWeekLimitDataTable();

            MeterData.HourlyTrendingSummaryRow hourlySummary;
            AlarmData.HourOfWeekLimitRow hourlyLimit;

            Channel channel;
            int channelID;

            using (HourOfWeekLimitTableAdapter hourlyLimitAdapter = new HourOfWeekLimitTableAdapter())
            using (HourlyTrendingSummaryTableAdapter hourlySummaryAdapter = new HourlyTrendingSummaryTableAdapter())
            {
                hourlyLimitAdapter.Connection.ConnectionString = m_connectionString;
                hourlySummaryAdapter.Connection.ConnectionString = m_connectionString;
                hourlySummaryAdapter.ClearBeforeFill = false;

                foreach (KeyValuePair<Channel, List<DataGroup>> channelGroups in trendingGroups)
                {
                    channel = channelGroups.Key;
                    channelID = channel.ID;
                    hourlyLimitAdapter.FillBy(hourlyLimitTable, channelID);

                    // Get hourly summary records for these data groups
                    hourlySummaryTable.Clear();

                    foreach (DataGroup dataGroup in channelGroups.Value)
                        hourlySummaryAdapter.FillBy(hourlySummaryTable, channelID, dataGroup.StartTime, dataGroup.EndTime);

                    for (int i = hourlySummaryTable.Count - 1; i >= 0; i--)
                    {
                        if (hourlySummaryTable[i].ValidCount + hourlySummaryTable[i].InvalidCount < channel.SamplesPerHour)
                            hourlySummaryTable.Rows.RemoveAt(i);
                    }

                    RemoveDuplicateRows(hourlySummaryTable);

                    // Update alarm log for each excursion
                    foreach (Tuple<MeterData.HourlyTrendingSummaryRow, AlarmData.HourOfWeekLimitRow> tuple in hourlySummaryTable.Join(hourlyLimitTable, row => GetHourOfWeek(row.Time), row => row.HourOfWeek, Tuple.Create))
                    {
                        hourlySummary = tuple.Item1;
                        hourlyLimit = tuple.Item2;

                        if (hourlySummary.Average < hourlyLimit.Low || hourlySummary.Average > hourlyLimit.High)
                            m_alarmLogTable.AddAlarmLogRow(channelID, hourlyLimit.AlarmTypeID, hourlySummary.Time, hourlyLimit.Severity, hourlyLimit.High, hourlyLimit.Low, hourlySummary.Average);
                    }
                }
            }
        }

        private void LoadAlarmLogs()
        {
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(m_connectionString))
            {
                bulkCopy.BulkCopyTimeout = 0;
                bulkCopy.DestinationTableName = m_alarmLogTable.TableName;
                bulkCopy.WriteToServer(m_alarmLogTable);
                m_alarmLogTable.Clear();
            }
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

        private int GetHourOfWeek(DateTime time)
        {
            return (int)time.DayOfWeek * 24 + time.Hour;
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
