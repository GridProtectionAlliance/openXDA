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
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using FaultData.Configuration;
using FaultData.DataAnalysis;
using FaultData.Database;
using FaultData.Database.AlarmDataTableAdapters;
using FaultData.DataResources;
using FaultData.DataSets;
using GSF.Configuration;
using log4net;
using openHistorian.XDALink;
using static FaultData.Database.AlarmData;

namespace FaultData.DataOperations
{
    public class AlarmOperation : DataOperationBase<MeterDataSet>
    {
        #region [ Members ]

        // Fields
        private HistorianSettings m_historianSettings;
        private DbAdapterContainer m_dbAdapterContainer;

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Creates a new <see cref="AlarmOperation"/>.
        /// </summary>
        public AlarmOperation()
        {
            m_historianSettings = new HistorianSettings();
        }

        #endregion

        #region [ Properties ]

        [Category]
        [SettingName("Historian")]
        public HistorianSettings HistorianSettings
        {
            get
            {
                return m_historianSettings;
            }
        }

        #endregion

        #region [ Methods ]

        public override void Prepare(DbAdapterContainer dbAdapterContainer)
        {
            m_dbAdapterContainer = dbAdapterContainer;
        }

        public override void Execute(MeterDataSet meterDataSet)
        {
            Log.Info("Executing operation to load alarm data into the database...");
            LoadRangeLimits(meterDataSet);
            LoadHourOfWeekLimits(meterDataSet);
        }

        public override void Load(DbAdapterContainer dbAdapterContainer)
        {
        }

        private void LoadRangeLimits(MeterDataSet meterDataSet)
        {
            MeterAlarmSummaryTableAdapter meterAlarmSummaryAdapter;
            ChannelAlarmSummaryTableAdapter channelAlarmSummaryAdapter;
            HourOfWeekLimitTableAdapter hourlyLimitAdapter;

            MeterAlarmSummaryDataTable meterAlarmSummaryTable;
            ChannelAlarmSummaryDataTable channelAlarmSummaryTable;

            Dictionary<Channel, List<DataGroup>> trendingGroups;

            DateTime startTime;
            DateTime endTime;
            int days;

            meterAlarmSummaryAdapter = m_dbAdapterContainer.GetAdapter<MeterAlarmSummaryTableAdapter>();
            channelAlarmSummaryAdapter = m_dbAdapterContainer.GetAdapter<ChannelAlarmSummaryTableAdapter>();
            hourlyLimitAdapter = m_dbAdapterContainer.GetAdapter<HourOfWeekLimitTableAdapter>();

            meterAlarmSummaryTable = new MeterAlarmSummaryDataTable();
            channelAlarmSummaryTable = new ChannelAlarmSummaryDataTable();

            trendingGroups = meterDataSet.GetResource<TrendingGroupsResource>().TrendingGroups;

            startTime = trendingGroups
                .SelectMany(kvp => kvp.Value)
                .Select(dataGroup => dataGroup.StartTime)
                .DefaultIfEmpty(DateTime.MaxValue)
                .Min().Date;

            endTime = trendingGroups
                .SelectMany(kvp => kvp.Value)
                .Select(dataGroup => dataGroup.EndTime)
                .DefaultIfEmpty(DateTime.MinValue)
                .Max();

            if (endTime != endTime.Date)
                endTime = endTime.Date.AddDays(1.0D);

            days = (int)Math.Ceiling((endTime - startTime).TotalDays);

            if (days < 0)
                return;

            using (Historian historian = new Historian(m_historianSettings.Server, m_historianSettings.InstanceName))
            {
                for (int i = 0; i < days; i++)
                {
                    DateTime queryStart = startTime.AddDays(i);
                    DateTime queryEnd = queryStart.AddDays(1.0D).AddTicks(-1L);

                    List<ChannelAlarmSummaryRow> channelAlarmSummaries = historian.Read(meterDataSet.Meter.Channels.Select(channel => channel.ID), queryStart, queryEnd)
                        .Where(trendingPoint => trendingPoint.SeriesID == SeriesID.Average)
                        .GroupBy(trendingPoint => trendingPoint.ChannelID)
                        .SelectMany(channelGroup =>
                        {
                            Channel channel = m_dbAdapterContainer.GetAdapter<MeterInfoDataContext>().Channels.Single(ch => ch.ID == channelGroup.Key);
                            AlarmRangeLimitDataTable rangeLimitTable = InitializeRangeLimitTable(channel);

                            return channelGroup
                                .SelectMany(TrendingPoint => rangeLimitTable.Where(rangeLimit => rangeLimit.Enabled != 0).Select(RangeLimit => new { TrendingPoint, RangeLimit }))
                                .Where(obj => CheckAlarm(channel, obj.TrendingPoint, obj.RangeLimit))
                                .GroupBy(obj => obj.RangeLimit.AlarmTypeID)
                                .Select(alarmTypeGroup => CreateSummaryRow(channelAlarmSummaryTable, channelGroup.Key, alarmTypeGroup.Key, queryStart, alarmTypeGroup.Count()));
                        })
                        .ToList();

                    IEnumerable<MeterAlarmSummaryRow> meterAlarmSummaries = channelAlarmSummaries
                        .GroupBy(channelAlarmSummary => channelAlarmSummary.AlarmTypeID)
                        .Select(channelSummaryGroup => CreateSummaryRow(meterAlarmSummaryTable, meterDataSet.Meter.ID, channelSummaryGroup.Key, queryStart, channelSummaryGroup.Sum(channelAlarmSummary => channelAlarmSummary.AlarmPoints)));

                    foreach (MeterAlarmSummaryRow meterAlarmSummary in meterAlarmSummaries)
                        meterAlarmSummaryAdapter.Upsert(meterAlarmSummary);

                    foreach (ChannelAlarmSummaryRow channelAlarmSummary in channelAlarmSummaries)
                        channelAlarmSummaryAdapter.Upsert(channelAlarmSummary);
                }
            }
        }

        private void LoadHourOfWeekLimits(MeterDataSet meterDataSet)
        {
            MeterAlarmSummaryTableAdapter meterAlarmSummaryAdapter;
            ChannelAlarmSummaryTableAdapter channelAlarmSummaryAdapter;
            HourOfWeekLimitTableAdapter hourlyLimitAdapter;

            MeterAlarmSummaryDataTable meterAlarmSummaryTable;
            ChannelAlarmSummaryDataTable channelAlarmSummaryTable;

            Dictionary<Channel, List<DataGroup>> trendingGroups;

            DateTime startTime;
            DateTime endTime;
            int days;

            meterAlarmSummaryAdapter = m_dbAdapterContainer.GetAdapter<MeterAlarmSummaryTableAdapter>();
            channelAlarmSummaryAdapter = m_dbAdapterContainer.GetAdapter<ChannelAlarmSummaryTableAdapter>();
            hourlyLimitAdapter = m_dbAdapterContainer.GetAdapter<HourOfWeekLimitTableAdapter>();

            meterAlarmSummaryTable = new MeterAlarmSummaryDataTable();
            channelAlarmSummaryTable = new ChannelAlarmSummaryDataTable();

            trendingGroups = meterDataSet.GetResource<TrendingGroupsResource>().TrendingGroups;

            startTime = trendingGroups
                .SelectMany(kvp => kvp.Value)
                .Select(dataGroup => dataGroup.StartTime)
                .DefaultIfEmpty(DateTime.MaxValue)
                .Min();

            endTime = trendingGroups
                .SelectMany(kvp => kvp.Value)
                .Select(dataGroup => dataGroup.EndTime)
                .DefaultIfEmpty(DateTime.MinValue)
                .Max();

            days = (int)Math.Ceiling((endTime - startTime).TotalDays);

            if (days < 0)
                return;

            using (Historian historian = new Historian(m_historianSettings.Server, m_historianSettings.InstanceName))
            {
                for (int i = 0; i < days; i++)
                {
                    DateTime queryStart = startTime.AddDays(days);
                    DateTime queryEnd = queryStart.AddDays(1.0D).AddTicks(-1L);

                    List<ChannelAlarmSummaryRow> channelAlarmSummaries = historian.Read(meterDataSet.Meter.Channels.Select(channel => channel.ID), queryStart, queryEnd)
                        .Where(trendingPoint => trendingPoint.SeriesID == SeriesID.Average)
                        .GroupBy(trendingPoint => trendingPoint.ChannelID)
                        .SelectMany(channelGroup =>
                        {
                            return channelGroup
                                .Join(hourlyLimitAdapter.GetDataBy(channelGroup.Key), trendingPoint => GetHourOfWeek(trendingPoint.Timestamp), hourlyLimit => hourlyLimit.HourOfWeek, (TrendingPoint, HourlyLimit) => new { TrendingPoint, HourlyLimit })
                                .Where(obj => obj.TrendingPoint.Value < obj.HourlyLimit.Low || obj.TrendingPoint.Value > obj.HourlyLimit.High)
                                .GroupBy(obj => obj.HourlyLimit.AlarmTypeID)
                                .Select(alarmTypeGroup => CreateSummaryRow(channelAlarmSummaryTable, channelGroup.Key, alarmTypeGroup.Key, queryStart, alarmTypeGroup.Count()));
                        })
                        .ToList();

                    IEnumerable<MeterAlarmSummaryRow> meterAlarmSummaries = channelAlarmSummaries
                        .GroupBy(channelAlarmSummary => channelAlarmSummary.AlarmTypeID)
                        .Select(channelSummaryGroup => CreateSummaryRow(meterAlarmSummaryTable, meterDataSet.Meter.ID, channelSummaryGroup.Key, queryStart, channelSummaryGroup.Sum(channelAlarmSummary => channelAlarmSummary.AlarmPoints)));

                    foreach (MeterAlarmSummaryRow meterAlarmSummary in meterAlarmSummaries)
                        meterAlarmSummaryAdapter.Upsert(meterAlarmSummary);

                    foreach (ChannelAlarmSummaryRow channelAlarmSummary in channelAlarmSummaries)
                        channelAlarmSummaryAdapter.Upsert(channelAlarmSummary);
                }
            }
        }

        private AlarmRangeLimitDataTable InitializeRangeLimitTable(Channel channel)
        {
            AlarmRangeLimitTableAdapter rangeLimitAdapter;
            AlarmRangeLimitDataTable rangeLimitTable;
            AlarmRangeLimitRow alarmRangeLimitRow;

            DefaultAlarmRangeLimitTableAdapter defaultRangeLimitAdapter;
            DefaultAlarmRangeLimitDataTable defaultRangeLimitTable;

            // Fill the range limit table with range limits for the given channel
            rangeLimitAdapter = m_dbAdapterContainer.GetAdapter<AlarmRangeLimitTableAdapter>();
            rangeLimitTable = rangeLimitAdapter.GetDataBy(channel.ID);

            // If limits exist for the given channel,
            // range limit table has been successfully initialized
            if (rangeLimitTable.Count != 0)
                return rangeLimitTable;

            // Get the default range limits for the measurement type and characteristic of this channel
            defaultRangeLimitAdapter = m_dbAdapterContainer.GetAdapter<DefaultAlarmRangeLimitTableAdapter>();
            defaultRangeLimitTable = defaultRangeLimitAdapter.GetDataBy(channel.MeasurementTypeID, channel.MeasurementCharacteristicID);

            // If there are no default limits for the channel,
            // then the range limit table has been successfully initialized
            if (defaultRangeLimitTable.Count == 0)
                return rangeLimitTable;

            lock (RangeLimitLock)
            {
                // Fill the range limit table one more time inside the lock to
                // ensure that no other threads have written limits for this channel
                rangeLimitAdapter.FillBy(rangeLimitTable, channel.ID);

                // If there are still no limits defined for this channel,
                // update the table to include this channel's default limits
                if (rangeLimitTable.Count == 0)
                {
                    foreach (DefaultAlarmRangeLimitRow row in defaultRangeLimitTable)
                    {
                        alarmRangeLimitRow = rangeLimitTable.NewAlarmRangeLimitRow();

                        alarmRangeLimitRow.ChannelID = channel.ID;
                        alarmRangeLimitRow.AlarmTypeID = row.AlarmTypeID;
                        alarmRangeLimitRow.Severity = row.Severity;
                        alarmRangeLimitRow.RangeInclusive = row.RangeInclusive;
                        alarmRangeLimitRow.PerUnit = row.PerUnit;
                        alarmRangeLimitRow.IsDefault = 1;
                        alarmRangeLimitRow.Enabled = 1;

                        if (!row.IsHighNull())
                            alarmRangeLimitRow.High = row.High;

                        if (!row.IsLowNull())
                            alarmRangeLimitRow.Low = row.Low;

                        rangeLimitTable.AddAlarmRangeLimitRow(alarmRangeLimitRow);
                    }

                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(m_dbAdapterContainer.Connection))
                    {
                        bulkCopy.BulkCopyTimeout = 0;
                        bulkCopy.DestinationTableName = rangeLimitTable.TableName;
                        bulkCopy.WriteToServer(rangeLimitTable);
                    }
                }

                return rangeLimitTable;
            }
        }

        private bool CheckAlarm(Channel channel, TrendingDataPoint trendingPoint, AlarmRangeLimitRow rangeLimit)
        {
            double perUnitValue;

            double highLimit = 0.0D;
            double lowLimit = 0.0D;
            bool highValid = true;
            bool lowValid = true;

            perUnitValue = channel.PerUnitValue ?? 1.0D;

            if (!rangeLimit.IsHighNull())
            {
                highLimit = Convert.ToBoolean(rangeLimit.PerUnit) ? (rangeLimit.High * perUnitValue) : rangeLimit.High;
                highValid = Convert.ToBoolean(rangeLimit.RangeInclusive) ^ (trendingPoint.Value <= highLimit);
            }

            if (!rangeLimit.IsLowNull())
            {
                lowLimit = Convert.ToBoolean(rangeLimit.PerUnit) ? (rangeLimit.Low * perUnitValue) : rangeLimit.Low;
                lowValid = Convert.ToBoolean(rangeLimit.RangeInclusive) ^ (trendingPoint.Value >= lowLimit);
            }

            return !lowValid || !highValid;
        }

        private int GetHourOfWeek(DateTime time)
        {
            return (int)time.DayOfWeek * 24 + time.Hour;
        }

        private MeterAlarmSummaryRow CreateSummaryRow(MeterAlarmSummaryDataTable meterAlarmSummaryTable, int meterID, int alarmTypeID, DateTime date, int alarmPoints)
        {
            MeterAlarmSummaryRow row = meterAlarmSummaryTable.NewMeterAlarmSummaryRow();
            row.MeterID = meterID;
            row.AlarmTypeID = alarmTypeID;
            row.Date = date;
            row.AlarmPoints = alarmPoints;
            return row;
        }

        private ChannelAlarmSummaryRow CreateSummaryRow(ChannelAlarmSummaryDataTable channelAlarmSummaryTable, int channelID, int alarmTypeID, DateTime date, int alarmPoints)
        {
            ChannelAlarmSummaryRow row = channelAlarmSummaryTable.NewChannelAlarmSummaryRow();
            row.ChannelID = channelID;
            row.AlarmTypeID = alarmTypeID;
            row.Date = date;
            row.AlarmPoints = alarmPoints;
            return row;
        }

        #endregion

        #region [ Static ]

        // Static Fields
        private static readonly object RangeLimitLock = new object();
        private static readonly ILog Log = LogManager.GetLogger(typeof(AlarmOperation));

        #endregion
    }
}
