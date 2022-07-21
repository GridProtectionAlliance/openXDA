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
//  12/14/2020 - C. lackner
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using FaultData.DataAnalysis;
using FaultData.DataOperations;
using FaultData.DataResources;
using FaultData.DataSets;
using GSF;
using GSF.Collections;
using GSF.Configuration;
using GSF.Data;
using GSF.Data.Model;
using HIDS;
using log4net;
using openXDA.HIDS.APIExtensions;
using openXDA.Model;
using AlarmDayFilter = System.Func<System.DateTime, bool>;

namespace openXDA.HIDS
{
    public class HIDSAlarmOperation : DataOperationBase<MeterDataSet>
    {
        #region [ Members ]

        // Nested Types
        private class AlarmLoader
        {
            #region [ Constructors ]

            public AlarmLoader(AdoDataConnection connection) =>
                Connection = connection;

            #endregion

            #region [ Properties ]

            private AdoDataConnection Connection { get; }

            #endregion

            #region [ Methods ]

            public Dictionary<int, List<AlarmTester>> LoadChannelTestersLookup(Meter meter)
            {
                Dictionary<int, List<AlarmTester>> alarmDataLookup = new Dictionary<int, List<AlarmTester>>();

                const string AlarmQueryFormat =
                    "SELECT " +
                    "    Channel.ID ChannelID, " +
                    "    SeriesType.Name SeriesType, " +
                    "    AlarmType.Name AlarmType, " +
                    "    ActiveAlarm.* " +
                    "FROM " +
                    "    ActiveAlarmView ActiveAlarm JOIN " +
                    "    AlarmType ON ActiveAlarm.AlarmTypeID = AlarmType.ID JOIN " +
                    "    Series ON ActiveAlarm.SeriesID = Series.ID JOIN " +
                    "    Channel ON Series.ChannelID = Channel.ID JOIN " +
                    "    SeriesType ON Series.SeriesTypeID = SeriesType.ID " +
                    "WHERE Channel.MeterID = {0}";

                using (DataTable table = Connection.RetrieveData(AlarmQueryFormat, meter.ID))
                {
                    TableOperations<ActiveAlarm> activeAlarmTable = new TableOperations<ActiveAlarm>(Connection);

                    foreach (DataRow row in table.Rows)
                    {
                        int channelID = row.ConvertField<int>("ChannelID");
                        string seriesType = row.ConvertField<string>("SeriesType");
                        string alarmType = row.ConvertField<string>("AlarmType");
                        ActiveAlarm alarm = activeAlarmTable.LoadRecord(row);
                        AlarmData alarmData = new AlarmData(alarm, alarmType, seriesType);
                        AlarmTester alarmTester = new AlarmTester(this, alarmData);
                        List<AlarmTester> channelTesters = alarmDataLookup.GetOrAdd(channelID, _ => new List<AlarmTester>());
                        channelTesters.Add(alarmTester);
                    }
                }

                return alarmDataLookup;
            }

            public List<AlarmValue> LoadAlarmValues(ActiveAlarm alarm)
            {
                TableOperations<AlarmValue> alarmValueTable = new TableOperations<AlarmValue>(Connection);

                return alarmValueTable
                    .QueryRecordsWhere("AlarmID = {0}", alarm.AlarmID)
                    .ToList();
            }

            public List<AlarmDay> LoadAlarmDays()
            {
                TableOperations<AlarmDay> alarmDayTable = new TableOperations<AlarmDay>(Connection);

                return alarmDayTable
                    .QueryRecords()
                    .ToList();
            }

            #endregion
        }

        private class AlarmTester
        {
            #region [ Constructors ]

            public AlarmTester(AlarmLoader alarmLoader, AlarmData alarmData)
            {
                AlarmLoader = alarmLoader;
                AlarmData = alarmData;

                ValueSelector = InitializeValueSelector();
                LazyThresholdSelector = new Lazy<Func<DateTime, double>>(InitializeThresholdSelector);
                Comparer = InitializeComparer();

                LazyAlarmDayFilterLookup = new Lazy<Dictionary<int, AlarmDayFilter>>(InitializeAlarmDayFilterLookup);
            }

            #endregion

            #region [ Properties ]

            public AlarmData AlarmData { get; }
            private AlarmLoader AlarmLoader { get; }

            private Func<Point, double> ValueSelector { get; }
            private Lazy<Func<DateTime, double>> LazyThresholdSelector { get; }
            private Func<double, double, bool> Comparer { get; }

            private Lazy<Dictionary<int, AlarmDayFilter>> LazyAlarmDayFilterLookup { get; }

            #endregion

            #region [ Methods ]

            public bool Test(Point point)
            {
                double value = ValueSelector(point);
                double threshold = LazyThresholdSelector.Value(point.Timestamp);
                return Comparer(value, threshold);
            }

            private Func<Point, double> InitializeValueSelector()
            {
                switch (AlarmData.SeriesType)
                {
                    case "Maximum": return pt => pt.Maximum;
                    case "Minimum": return pt => pt.Minimum;
                    case "Average": return pt => pt.Average;
                    default: return _ => double.NaN;
                }
            }

            private Func<DateTime, double> InitializeThresholdSelector()
            {
                List<AlarmValue> alarmValues = AlarmLoader.LoadAlarmValues(AlarmData.Alarm);

                if (alarmValues.Count == 1)
                {
                    AlarmValue alarmValue = alarmValues.Single();
                    double alarmValueThreshold = alarmValue.Value * AlarmData.Alarm.Value;
                    return timestamp => alarmValueThreshold;
                }

                return timestamp => alarmValues
                    .Where(alarmValue => AlarmValueApplies(timestamp, alarmValue))
                    .Select(alarmValue => alarmValue.Value * AlarmData.Alarm.Value)
                    .DefaultIfEmpty(double.NaN)
                    .FirstOrDefault();
            }

            private Func<double, double, bool> InitializeComparer()
            {
                switch (AlarmData.AlarmType)
                {
                    case "Upper Limit": return (value, threshold) => value > threshold;
                    case "Lower Limit": return (value, threshold) => value < threshold;
                    default: return (_, __) => false;
                }
            }

            private Dictionary<int, AlarmDayFilter> InitializeAlarmDayFilterLookup() =>
                AlarmLoader.LoadAlarmDays().ToDictionary(alarmDay => alarmDay.ID, ToAlarmDayFilter);

            private AlarmDayFilter ToAlarmDayFilter(AlarmDay alarmDay)
            {
                bool IsWeekend(DayOfWeek dayOfWeek) =>
                    dayOfWeek == DayOfWeek.Saturday ||
                    dayOfWeek == DayOfWeek.Sunday;

                bool IsWeekday(DayOfWeek dayOfWeek) =>
                    !IsWeekend(dayOfWeek);

                switch (alarmDay.Name)
                {
                    case "Weekend": return dt => IsWeekend(dt.DayOfWeek);
                    case "Weekday": return dt => IsWeekday(dt.DayOfWeek);
                    case "Monday": return dt => dt.DayOfWeek == DayOfWeek.Monday;
                    case "Tuesday": return dt => dt.DayOfWeek == DayOfWeek.Tuesday;
                    case "Wednesday": return dt => dt.DayOfWeek == DayOfWeek.Wednesday;
                    case "Thursday": return dt => dt.DayOfWeek == DayOfWeek.Thursday;
                    case "Friday": return dt => dt.DayOfWeek == DayOfWeek.Friday;
                    case "Saturday": return dt => dt.DayOfWeek == DayOfWeek.Saturday;
                    case "Sunday": return dt => dt.DayOfWeek == DayOfWeek.Sunday;
                    default: return _ => true;
                }
            }

            private bool AlarmValueApplies(DateTime timestamp, AlarmValue alarmValue)
            {
                if (timestamp.Hour < alarmValue.StartHour)
                    return false;
                if (timestamp.Hour >= alarmValue.EndHour)
                    return false;

                if (alarmValue.AlarmDayID is null)
                    return true;

                int alarmDayID = alarmValue.AlarmDayID.GetValueOrDefault();
                if (!LazyAlarmDayFilterLookup.Value.TryGetValue(alarmDayID, out AlarmDayFilter alarmDayFilter))
                    return false;

                return alarmDayFilter(timestamp);
            }

            #endregion
        }

        private class AlarmData
        {
            #region [ Constructors ]

            public AlarmData(ActiveAlarm alarm, string alarmType, string seriesType)
            {
                Alarm = alarm;
                AlarmType = alarmType;
                SeriesType = seriesType;
            }

            #endregion

            #region [ Properties ]

            public ActiveAlarm Alarm { get; }
            public string AlarmType { get; }
            public string SeriesType { get; }

            #endregion
        }

        #endregion

        #region [ Properties ]

        [Category]
        [SettingName(HIDSSettings.CategoryName)]
        public HIDSSettings HIDSSettings { get; }
            = new HIDSSettings();

        #endregion

        #region [ Methods ]

        public override void Execute(MeterDataSet meterDataSet)
        {
            Log.Info("Executing operation to update trending summary alarms...");

            Dictionary<Channel, List<DataGroup>> trendingGroups = meterDataSet.GetResource<TrendingGroupsResource>().TrendingGroups;

            if (trendingGroups.Count == 0)
            {
                Log.Debug($"No trending data found; skipping {nameof(HIDSAlarmOperation)}.");
                return;
            }
            if (string.IsNullOrEmpty(HIDSSettings.Host))
            {
                Log.Debug($"No HIDS instance defined; skipping {nameof(HIDSAlarmOperation)}.");
                return;
            }

            using AdoDataConnection connection = meterDataSet.CreateDbConnection();
            AlarmLoader alarmLoader = new AlarmLoader(connection);
            Dictionary<int, List<AlarmTester>> channelTestersLookup = alarmLoader.LoadChannelTestersLookup(meterDataSet.Meter);

            if (!channelTestersLookup.Any())
            {
                Log.Debug($"No alarms defined for meter {meterDataSet.Meter.Name}; skipping {nameof(HIDSAlarmOperation)}.");
                return;
            }

            using API hids = new API();
            hids.Configure(HIDSSettings);

            CountAlarms(channelTestersLookup, meterDataSet, connection, hids);
            LogAlarms(channelTestersLookup, meterDataSet, connection, hids);
        }

        private void CountAlarms(Dictionary<int, List<AlarmTester>> channelTestersLookup, MeterDataSet meterDataSet, AdoDataConnection connection, API hids) =>
            CountAlarmsAsync(channelTestersLookup, meterDataSet, connection, hids).GetAwaiter().GetResult();

        private async Task CountAlarmsAsync(Dictionary<int, List<AlarmTester>> channelTestersLookup, MeterDataSet meterDataSet, AdoDataConnection connection, API hids)
        {
            TrendingGroupsResource trendingGroupsResource = meterDataSet.GetResource<TrendingGroupsResource>();
            Dictionary<Channel, List<DataGroup>> trendingGroups = trendingGroupsResource.TrendingGroups;

            List<Channel> trendingChannels = meterDataSet.Meter.Channels
                .Where(channel => channel.ID != 0)
                .Where(channel => channel.Enabled)
                .Where(channel => channel.MeasurementCharacteristic.Name != "Instantaneous")
                .ToList();

            IEnumerable<string> tags = trendingChannels
                .Select(channel => hids.ToTag(channel.ID));

            DateTime startTime = trendingGroups.Values
                .SelectMany(list => list)
                .Min(dataGroup => dataGroup.StartTime.Date);

            DateTime endTime = trendingGroups.Values
                .SelectMany(list => list)
                .Max(dataGroup => dataGroup.EndTime.AddDays(1.0D).AddTicks(-1L).Date);

            void QueryAllPoints(IQueryBuilder queryBuilder) => queryBuilder
                .Range(startTime, endTime)
                .FilterTags(tags);

            int totalDays = (int)(endTime - startTime).TotalDays;

            var channelCounts = Enumerable
                .Range(0, totalDays)
                .Select(day => startTime.AddDays(day))
                .SelectMany(_ => trendingChannels, (date, channel) => new { ChannelID = channel.ID, Date = date })
                .ToDictionary(key => key, _ => 0);

            Dictionary<DateTime, int> meterCounts = Enumerable
                .Range(0, totalDays)
                .Select(day => startTime.AddDays(day))
                .ToDictionary(date => date, _ => 0);

            await foreach (Point point in hids.ReadPointsAsync(QueryAllPoints))
            {
                int channelID = hids.ToChannelID(point.Tag);
                DateTime date = point.Timestamp.Date;
                var key = new { ChannelID = channelID, Date = date };

                if (!channelTestersLookup.TryGetValue(channelID, out List<AlarmTester> channelTesters))
                    continue;

                foreach (AlarmTester channelTester in channelTesters)
                {
                    if (!channelTester.Test(point))
                        continue;

                    channelCounts[key]++;
                    meterCounts[date]++;
                }
            }

            TableOperations<ChannelAlarmSummary> channelAlarmSummaryTable = new TableOperations<ChannelAlarmSummary>(connection);

            foreach (var kvp in channelCounts)
            {
                int channelID = kvp.Key.ChannelID;
                DateTime date = kvp.Key.Date;
                int count = kvp.Value;

                if (count == 0)
                {
                    string filter = "ChannelID = {0} AND Date = {1}";
                    channelAlarmSummaryTable.DeleteRecordWhere(filter, channelID, date);
                    continue;
                }

                channelAlarmSummaryTable.Upsert(new ChannelAlarmSummary()
                {
                    ChannelID = channelID,
                    Date = date,
                    AlarmPoints = count
                });
            }

            TableOperations<MeterAlarmSummary> meterAlarmSummaryTable = new TableOperations<MeterAlarmSummary>(connection);

            foreach (var kvp in meterCounts)
            {
                int meterID = meterDataSet.Meter.ID;
                DateTime date = kvp.Key.Date;
                int count = kvp.Value;

                if (count == 0)
                {
                    string filter = "MeterID = {0} AND Date = {1}";
                    meterAlarmSummaryTable.DeleteRecordWhere(filter, meterID, date);
                    continue;
                }

                meterAlarmSummaryTable.Upsert(new MeterAlarmSummary()
                {
                    MeterID = meterID,
                    Date = date,
                    AlarmPoints = count
                });
            }
        }

        private void LogAlarms(Dictionary<int, List<AlarmTester>> channelTestersLookup, MeterDataSet meterDataSet, AdoDataConnection connection, API hids)
        {
            TrendingDataSummaryResource trendingDataSummaryResource = meterDataSet.GetResource<TrendingDataSummaryResource>();
            Dictionary<Channel, List<TrendingDataSummaryResource.TrendingDataSummary>> trendingDataSummaries = trendingDataSummaryResource.TrendingDataSummaries;
            TableOperations<AlarmLog> alarmLogTable = new TableOperations<AlarmLog>(connection);

            foreach (KeyValuePair<Channel, List<TrendingDataSummaryResource.TrendingDataSummary>> channelSummaries in trendingDataSummaries)
            {
                int channelID = channelSummaries.Key.ID;
                IEnumerable<TrendingDataSummaryResource.TrendingDataSummary> summaries = channelSummaries.Value;

                IEnumerable<Point> points = summaries.Select(summary => new Point()
                {
                    Tag = hids.ToTag(channelID),
                    QualityFlags = 0u,
                    Timestamp = summary.Time,
                    Maximum = summary.Maximum,
                    Average = summary.Average,
                    Minimum = summary.Minimum
                });

                List<Point> data = points
                    .OrderBy(pt => pt.Timestamp)
                    .ToList();

                if (!channelTestersLookup.TryGetValue(channelID, out List<AlarmTester> channelTesters))
                    continue;

                foreach (AlarmTester channelTester in channelTesters)
                {
                    foreach (AlarmLog alarmLog in ProcessSeriesAlarm(channelTester, data))
                        alarmLogTable.AddNewRecord(alarmLog);
                }
            }
        }

        private List<AlarmLog> ProcessSeriesAlarm(AlarmTester alarmTester, List<Point> data)
        {
            List<AlarmLog> result = new List<AlarmLog>();

            List<Range<DateTime>> alarmRanges = new List<Range<DateTime>>();
            bool wasInAlarm = false;
            DateTime start = DateTime.MinValue;

            foreach (Point point in data)
            {
                bool isInAlarm = alarmTester.Test(point);

                if (!wasInAlarm && isInAlarm)
                    start = point.Timestamp;

                if (wasInAlarm && !isInAlarm)
                    alarmRanges.Add(new Range<DateTime>(start, point.Timestamp));

                wasInAlarm = isInAlarm;
            }

            if (wasInAlarm)
                alarmRanges.Add(new Range<DateTime>(start, data.Last().Timestamp));

            ActiveAlarm alarm = alarmTester.AlarmData.Alarm;

            return alarmRanges.Select(range => new AlarmLog()
            {
                AlarmID = alarm.AlarmID,
                AlarmFactorID = alarm.AlarmFactorID,
                StartTime = range.Start,
                EndTime = range.End
            }).ToList();
        }

        #endregion

        #region [ Static ]

        // Static Fields
        private static readonly ILog Log = LogManager.GetLogger(typeof(HIDSAlarmOperation));

        #endregion
    }
}
