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
        private DbAdapterContainer m_dbAdapterContainer;
        private AlarmData.AlarmTypeDataTable m_alarmTypeTable;
        private AlarmData.AlarmLogDataTable m_alarmLogTable;

        #endregion

        #region [ Methods ]

        public void Prepare(DbAdapterContainer dbAdapterContainer)
        {
            m_alarmTypeTable = new AlarmData.AlarmTypeDataTable();
            m_alarmLogTable = new AlarmData.AlarmLogDataTable();
            dbAdapterContainer.AlarmTypeAdapter.Fill(m_alarmTypeTable);

            m_dbAdapterContainer = dbAdapterContainer;
        }

        public void Execute(MeterDataSet meterDataSet)
        {
            LoadDataQualityAlarmLogs(meterDataSet);
            LoadRangeLimits(meterDataSet);
            LoadHourOfWeekLimits(meterDataSet);
        }

        public void Load(DbAdapterContainer dbAdapterContainer)
        {
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(dbAdapterContainer.Connection))
            {
                bulkCopy.BulkCopyTimeout = 0;
                bulkCopy.DestinationTableName = m_alarmLogTable.TableName;
                bulkCopy.WriteToServer(m_alarmLogTable);
                m_alarmLogTable.Clear();
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
            Dictionary<Channel, List<DataGroup>> trendingGroups;

            HourlyTrendingSummaryTableAdapter hourlySummaryAdapter;
            MeterData.HourlyTrendingSummaryDataTable hourlySummaryTable;
            AlarmData.AlarmRangeLimitDataTable rangeLimitTable;

            Channel channel;

            trendingGroups = meterDataSet.GetResource<TrendingGroupsResource>().TrendingGroups;

            hourlySummaryAdapter = m_dbAdapterContainer.HourlyTrendingSummaryAdapter;
            hourlySummaryTable = new MeterData.HourlyTrendingSummaryDataTable();
            rangeLimitTable = new AlarmData.AlarmRangeLimitDataTable();

            hourlySummaryAdapter.ClearBeforeFill = false;

            foreach (KeyValuePair<Channel, List<DataGroup>> channelGroups in trendingGroups)
            {
                channel = channelGroups.Key;

                InitializeRangeLimitTable(rangeLimitTable, channel);

                // Get hourly summary records for these data groups
                hourlySummaryTable.Clear();

                foreach (DataGroup dataGroup in channelGroups.Value)
                    hourlySummaryAdapter.FillBy(hourlySummaryTable, channel.ID, dataGroup.StartTime, dataGroup.EndTime);

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
                        CheckAlarm(channel, hourlySummary, rangeLimit);
                }
            }
        }

        private void LoadHourOfWeekLimits(MeterDataSet meterDataSet)
        {
            Dictionary<Channel, List<DataGroup>> trendingGroups;

            HourOfWeekLimitTableAdapter hourlyLimitAdapter;
            HourlyTrendingSummaryTableAdapter hourlySummaryAdapter;
            MeterData.HourlyTrendingSummaryDataTable hourlySummaryTable;
            AlarmData.HourOfWeekLimitDataTable hourlyLimitTable;

            MeterData.HourlyTrendingSummaryRow hourlySummary;
            AlarmData.HourOfWeekLimitRow hourlyLimit;

            Channel channel;
            int channelID;

            trendingGroups = meterDataSet.GetResource<TrendingGroupsResource>().TrendingGroups;

            hourlyLimitAdapter = m_dbAdapterContainer.HourOfWeekLimitAdapter;
            hourlySummaryAdapter = m_dbAdapterContainer.HourlyTrendingSummaryAdapter;
            hourlySummaryTable = new MeterData.HourlyTrendingSummaryDataTable();
            hourlyLimitTable = new AlarmData.HourOfWeekLimitDataTable();

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
                foreach (Tuple<MeterData.HourlyTrendingSummaryRow, AlarmData.HourOfWeekLimitRow> tuple in hourlySummaryTable.Join<MeterData.HourlyTrendingSummaryRow, AlarmData.HourOfWeekLimitRow, int, Tuple<MeterData.HourlyTrendingSummaryRow, AlarmData.HourOfWeekLimitRow>>(hourlyLimitTable, row => GetHourOfWeek(row.Time), row => row.HourOfWeek, Tuple.Create))
                {
                    hourlySummary = tuple.Item1;
                    hourlyLimit = tuple.Item2;

                    if (hourlySummary.Average < hourlyLimit.Low || hourlySummary.Average > hourlyLimit.High)
                        m_alarmLogTable.AddAlarmLogRow(channelID, hourlyLimit.AlarmTypeID, hourlySummary.Time, hourlyLimit.Severity, hourlyLimit.High, hourlyLimit.Low, hourlySummary.Average);
                }
            }
        }

        private void InitializeRangeLimitTable(AlarmData.AlarmRangeLimitDataTable rangeLimitTable, Channel channel)
        {
            AlarmRangeLimitTableAdapter rangeLimitAdapter;
            DefaultAlarmRangeLimitTableAdapter defaultRangeLimitAdapter;
            AlarmData.DefaultAlarmRangeLimitDataTable defaultRangeLimitTable;

            // Clear existing rows from the range limit table
            rangeLimitTable.Clear();

            // Fill the range limit table with range limits for the given channel
            rangeLimitAdapter = m_dbAdapterContainer.AlarmRangeLimitAdapter;
            rangeLimitAdapter.FillBy(rangeLimitTable, channel.ID);

            // If limits exist for the given channel,
            // range limit table has been successfully initialized
            if (rangeLimitTable.Count != 0)
                return;

            // Get the default range limits for the measurement type and characteristic of this channel
            defaultRangeLimitAdapter = m_dbAdapterContainer.DefaultAlarmRangeLimitAdapter;
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
                    foreach (AlarmData.DefaultAlarmRangeLimitRow row in defaultRangeLimitTable)
                        rangeLimitTable.AddAlarmRangeLimitRow(channel.ID, row.AlarmTypeID, row.Severity, row.High, row.Low, row.RangeInclusive, row.PerUnit, 1);

                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(m_dbAdapterContainer.Connection))
                    {
                        bulkCopy.BulkCopyTimeout = 0;
                        bulkCopy.DestinationTableName = rangeLimitTable.TableName;
                        bulkCopy.WriteToServer(rangeLimitTable);
                    }
                }
            }
        }

        private void CheckAlarm(Channel channel, MeterData.HourlyTrendingSummaryRow hourlySummary, AlarmData.AlarmRangeLimitRow rangeLimit)
        {
            double perUnitValue;

            var highLimit = 0.0D;
            var lowLimit = 0.0D;
            var highValid = true;
            var lowValid = true;

            perUnitValue = channel.PerUnitValue ?? 1.0D;

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
                var logRow = m_alarmLogTable.NewAlarmLogRow();

                logRow.ChannelID = channel.ID;
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

        #endregion

        #region [ Static ]

        // Static Fields
        private static readonly object RangeLimitLock = new object();

        #endregion
    }
}
