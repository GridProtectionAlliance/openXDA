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
using System.Linq;
using FaultData.Configuration;
using FaultData.DataAnalysis;
using FaultData.DataResources;
using FaultData.DataSets;
using GSF.Configuration;
using GSF.Data;
using GSF.Data.Model;
using log4net;
using openHistorian.XDALink;
using openXDA.Model;
using TrendingDataPoint = openHistorian.XDALink.TrendingDataPoint;

namespace FaultData.DataOperations
{
    public class AlarmOperation : DataOperationBase<MeterDataSet>
    {
        #region [ Members ]

        // Fields
        private HistorianSettings m_historianSettings;

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
        [SettingName(HistorianSettings.CategoryName)]
        public HistorianSettings HistorianSettings
        {
            get
            {
                return m_historianSettings;
            }
        }

        #endregion

        #region [ Methods ]

        public override void Execute(MeterDataSet meterDataSet)
        {
            Dictionary<Channel, List<DataGroup>> trendingGroups = meterDataSet.GetResource<TrendingGroupsResource>().TrendingGroups;

            if (trendingGroups.Count == 0)
            {
                Log.Debug($"No trending data found; skipping {nameof(AlarmOperation)}.");
                return;
            }

            LoadRangeLimits(meterDataSet);
            LoadHourOfWeekLimits(meterDataSet);
        }

        private void LoadRangeLimits(MeterDataSet meterDataSet)
        {
            Dictionary<Channel, List<DataGroup>> trendingGroups = meterDataSet.GetResource<TrendingGroupsResource>().TrendingGroups;

            DateTime startTime = trendingGroups
                .SelectMany(kvp => kvp.Value)
                .Select(dataGroup => dataGroup.StartTime)
                .DefaultIfEmpty(DateTime.MaxValue)
                .Min().Date;

            DateTime endTime = trendingGroups
                .SelectMany(kvp => kvp.Value)
                .Select(dataGroup => dataGroup.EndTime)
                .DefaultIfEmpty(DateTime.MinValue)
                .Max();

            if (endTime != endTime.Date)
                endTime = endTime.Date.AddDays(1.0D);

            int days = (int)Math.Ceiling((endTime - startTime).TotalDays);

            if (days < 0)
                return;

            using (AdoDataConnection connection = meterDataSet.CreateDbConnection())
            using (Historian historian = new Historian(m_historianSettings.Server, m_historianSettings.InstanceName))
            {
                TableOperations<ChannelAlarmSummary> channelAlarmSummaryTable = new TableOperations<ChannelAlarmSummary>(connection);
                TableOperations<MeterAlarmSummary> meterAlarmSummaryTable = new TableOperations<MeterAlarmSummary>(connection);

                Dictionary<int, Channel> channelLookup = meterDataSet.Meter.Channels
                    .Where(channel => channel.ID > 0)
                    .ToDictionary(channel => channel.ID);

                for (int i = 0; i < days; i++)
                {
                    DateTime queryStart = startTime.AddDays(i);
                    DateTime queryEnd = queryStart.AddDays(1.0D).AddTicks(-1L);

                    List<ChannelAlarmSummary> channelAlarmSummaries = historian.Read(meterDataSet.Meter.Channels.Select(channel => channel.ID), queryStart, queryEnd)
                        .Where(trendingPoint => trendingPoint.SeriesID == SeriesID.Average)
                        .GroupBy(trendingPoint => trendingPoint.ChannelID)
                        .SelectMany(channelGroup =>
                        {
                            Channel channel = channelLookup[channelGroup.Key];
                            List<AlarmRangeLimit> alarmRangeLimits = InitializeRangeLimitTable(connection, channel);

                            return channelGroup
                                .SelectMany(TrendingPoint => alarmRangeLimits.Where(rangeLimit => rangeLimit.Enabled).Select(RangeLimit => new { TrendingPoint, RangeLimit }))
                                .Where(obj => CheckAlarm(channel, obj.TrendingPoint, obj.RangeLimit))
                                .GroupBy(obj => obj.RangeLimit.AlarmTypeID)
                                .Select(alarmTypeGroup => CreateChannelAlarmSummary(channelGroup.Key, alarmTypeGroup.Key, queryStart, alarmTypeGroup.Count()));
                        })
                        .ToList();

                    List<MeterAlarmSummary> meterAlarmSummaries = channelAlarmSummaries
                        .GroupBy(channelAlarmSummary => channelAlarmSummary.AlarmTypeID)
                        .Select(channelSummaryGroup => CreateMeterAlarmSummary(meterDataSet.Meter.ID, channelSummaryGroup.Key, queryStart, channelSummaryGroup.Sum(channelAlarmSummary => channelAlarmSummary.AlarmPoints)))
                        .ToList();

                    foreach (MeterAlarmSummary meterAlarmSummary in meterAlarmSummaries)
                        meterAlarmSummaryTable.Upsert(meterAlarmSummary);

                    foreach (ChannelAlarmSummary channelAlarmSummary in channelAlarmSummaries)
                        channelAlarmSummaryTable.Upsert(channelAlarmSummary);
                }
            }
        }

        private void LoadHourOfWeekLimits(MeterDataSet meterDataSet)
        {
            TrendingGroupsResource trendingGroupsResource = meterDataSet.GetResource<TrendingGroupsResource>();
            Dictionary<Channel, List<DataGroup>> trendingGroups = trendingGroupsResource.TrendingGroups;

            DateTime startTime = trendingGroups
                .SelectMany(kvp => kvp.Value)
                .Select(dataGroup => dataGroup.StartTime)
                .DefaultIfEmpty(DateTime.MaxValue)
                .Min();

            DateTime endTime = trendingGroups
                .SelectMany(kvp => kvp.Value)
                .Select(dataGroup => dataGroup.EndTime)
                .DefaultIfEmpty(DateTime.MinValue)
                .Max();

            int days = (int)Math.Ceiling((endTime - startTime).TotalDays);

            if (days < 0)
                return;

            using (Historian historian = new Historian(m_historianSettings.Server, m_historianSettings.InstanceName))
            using (AdoDataConnection connection = meterDataSet.CreateDbConnection())
            {
                TableOperations<HourOfWeekLimit> hourOfWeekLimitTable = new TableOperations<HourOfWeekLimit>(connection);
                TableOperations<MeterAlarmSummary> meterAlarmSummaryTable = new TableOperations<MeterAlarmSummary>(connection);
                TableOperations<ChannelAlarmSummary> channelAlarmSummaryTable = new TableOperations<ChannelAlarmSummary>(connection);

                for (int i = 0; i < days; i++)
                {
                    DateTime queryStart = startTime.AddDays(days);
                    DateTime queryEnd = queryStart.AddDays(1.0D).AddTicks(-1L);

                    List<ChannelAlarmSummary> channelAlarmSummaries = historian.Read(meterDataSet.Meter.Channels.Select(channel => channel.ID), queryStart, queryEnd)
                        .Where(trendingPoint => trendingPoint.SeriesID == SeriesID.Average)
                        .GroupBy(trendingPoint => trendingPoint.ChannelID)
                        .SelectMany(channelGroup =>
                        {
                            List<HourOfWeekLimit> hourOfWeekLimits = hourOfWeekLimitTable
                                .QueryRecordsWhere("ChannelID = {0}", channelGroup.Key)
                                .ToList();

                            return channelGroup
                                .Join(hourOfWeekLimits, trendingPoint => GetHourOfWeek(trendingPoint.Timestamp), hourlyLimit => hourlyLimit.HourOfWeek, (TrendingPoint, HourlyLimit) => new { TrendingPoint, HourlyLimit })
                                .Where(obj => obj.TrendingPoint.Value < obj.HourlyLimit.Low || obj.TrendingPoint.Value > obj.HourlyLimit.High)
                                .GroupBy(obj => obj.HourlyLimit.AlarmTypeID)
                                .Select(alarmTypeGroup => CreateChannelAlarmSummary(channelGroup.Key, alarmTypeGroup.Key, queryStart, alarmTypeGroup.Count()));
                        })
                        .ToList();

                    List<MeterAlarmSummary> meterAlarmSummaries = channelAlarmSummaries
                        .GroupBy(channelAlarmSummary => channelAlarmSummary.AlarmTypeID)
                        .Select(channelSummaryGroup => CreateMeterAlarmSummary(meterDataSet.Meter.ID, channelSummaryGroup.Key, queryStart, channelSummaryGroup.Sum(channelAlarmSummary => channelAlarmSummary.AlarmPoints)))
                        .ToList();

                    foreach (MeterAlarmSummary meterAlarmSummary in meterAlarmSummaries)
                        meterAlarmSummaryTable.Upsert(meterAlarmSummary);

                    foreach (ChannelAlarmSummary channelAlarmSummary in channelAlarmSummaries)
                        channelAlarmSummaryTable.Upsert(channelAlarmSummary);
                }
            }
        }

        private List<AlarmRangeLimit> InitializeRangeLimitTable(AdoDataConnection connection, Channel channel)
        {
            // Fill the range limit table with range limits for the given channel
            TableOperations<AlarmRangeLimit> alarmRangeLimitTable = new TableOperations<AlarmRangeLimit>(connection);

            List<AlarmRangeLimit> alarmRangeLimits = alarmRangeLimitTable
                .QueryRecordsWhere("ChannelID = {0}", channel.ID)
                .ToList();

            // If limits exist for the given channel,
            // range limit table has been successfully initialized
            if (alarmRangeLimits.Count != 0)
                return alarmRangeLimits;

            // Get the default range limits for the measurement type and characteristic of this channel
            TableOperations<DefaultAlarmRangeLimit> defaultAlarmRangeLimitTable = new TableOperations<DefaultAlarmRangeLimit>(connection);
            int measurementTypeID = channel.MeasurementTypeID;
            int measurementCharacteristicID = channel.MeasurementCharacteristicID;

            List<DefaultAlarmRangeLimit> defaultAlarmRangeLimits = defaultAlarmRangeLimitTable
                .QueryRecordsWhere("MeasurementTypeID = {0} AND MeasurementCharacteristicID = {1}", measurementTypeID, measurementCharacteristicID)
                .ToList();

            // If there are no default limits for the channel,
            // then the range limit table has been successfully initialized
            if (defaultAlarmRangeLimits.Count == 0)
                return alarmRangeLimits;

            foreach (DefaultAlarmRangeLimit defaultAlarmRangeLimit in defaultAlarmRangeLimits)
            {
                AlarmRangeLimit alarmRangeLimit = new AlarmRangeLimit()
                {
                    ChannelID = channel.ID,
                    AlarmTypeID = defaultAlarmRangeLimit.AlarmTypeID,
                    Severity = defaultAlarmRangeLimit.Severity,
                    High = defaultAlarmRangeLimit.High,
                    Low = defaultAlarmRangeLimit.Low,
                    RangeInclusive = defaultAlarmRangeLimit.RangeInclusive,
                    PerUnit = defaultAlarmRangeLimit.PerUnit,
                    IsDefault = true,
                    Enabled = true
                };

                alarmRangeLimitTable.AddNewRecord(alarmRangeLimit);
            }

            return alarmRangeLimits;
        }

        private bool CheckAlarm(Channel channel, TrendingDataPoint trendingPoint, AlarmRangeLimit alarmRangeLimit)
        {
            double highLimit = 0.0D;
            double lowLimit = 0.0D;
            bool highValid = true;
            bool lowValid = true;

            double perUnitValue = channel.PerUnitValue ?? 1.0D;
            double factor = alarmRangeLimit.PerUnit ? perUnitValue : 1.0D;

            if ((object)alarmRangeLimit.High != null)
            {
                highLimit = factor * alarmRangeLimit.High.GetValueOrDefault();
                highValid = Convert.ToBoolean(alarmRangeLimit.RangeInclusive) ^ (trendingPoint.Value <= highLimit);
            }

            if ((object)alarmRangeLimit.Low != null)
            {
                lowLimit = factor * alarmRangeLimit.Low.GetValueOrDefault();
                lowValid = Convert.ToBoolean(alarmRangeLimit.RangeInclusive) ^ (trendingPoint.Value >= lowLimit);
            }

            return !lowValid || !highValid;
        }

        private int GetHourOfWeek(DateTime time)
        {
            return (int)time.DayOfWeek * 24 + time.Hour;
        }

        private MeterAlarmSummary CreateMeterAlarmSummary(int meterID, int alarmTypeID, DateTime date, int alarmPoints)
        {
            return new MeterAlarmSummary()
            {
                MeterID = meterID,
                AlarmTypeID = alarmTypeID,
                Date = date,
                AlarmPoints = alarmPoints
            };
        }

        private ChannelAlarmSummary CreateChannelAlarmSummary(int channelID, int alarmTypeID, DateTime date, int alarmPoints)
        {
            return new ChannelAlarmSummary()
            {
                ChannelID = channelID,
                AlarmTypeID = alarmTypeID,
                Date = date,
                AlarmPoints = alarmPoints
        };
        }

        #endregion

        #region [ Static ]

        // Static Fields
        private static readonly ILog Log = LogManager.GetLogger(typeof(AlarmOperation));

        #endregion
    }
}
