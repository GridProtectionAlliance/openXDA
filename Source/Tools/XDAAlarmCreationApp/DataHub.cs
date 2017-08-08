//******************************************************************************************************
//  DataHub.cs - Gbtc
//
//  Copyright © 2017, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the MIT License (MIT), the "License"; you may
//  not use this file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://opensource.org/licenses/MIT
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  05/23/2017 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************

using XDAAlarmCreationApp.Model;
using GSF;
using GSF.Data;
using GSF.Data.Model;
using GSF.IO;
using GSF.Web.Hubs;
using GSF.Web.Model.HubOperations;
using GSF.Web.Security;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Web.Script;
using CsvHelper;
using Microsoft.AspNet.SignalR;
using GSF.Web.Model;
using GSF.Identity;
using openHistorian.XDALink;

namespace XDAAlarmCreationApp
{
    public class DataHub : RecordOperationsHub<DataHub>, IDirectoryBrowserOperations
    {
        #region [ Constructors ]

        public DataHub() : base(Program.LogStatusMessage, Program.LogExceptionMessage)
        {
            Action<string, UpdateType> logStatusMessage = (message, updateType) => LogStatusMessage(message, updateType);
            Action<Exception> logException = ex => LogException(ex);

        }

        #endregion

        #region [ Static ]

        // Static Properties

        /// <summary>
        /// Gets the hub connection ID for the current thread.
        /// </summary>
        public static string CurrentConnectionID => s_connectionID.Value;

        // Static Fields
        private static readonly ThreadLocal<string> s_connectionID;
        private static volatile int s_connectCount;
        private static bool s_massImportWaitFlag = false;


        // Static Constructor
        static DataHub()
        {
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string ecaClientDataPath = Path.Combine(appData, "Grid Protection Alliance", "XDA Alarm Client");
            s_connectionID = new ThreadLocal<string>();
        }

        public void ProgressUpdatedByMeter(string meterName, int value)
        {
            GlobalHost.ConnectionManager.GetHubContext<DataHub>().Clients.All.updateProgressBarForMeter(meterName, value);
        }

        public static void ProgressUpdatedOverall(string meterName, int value)
        {
            GlobalHost.ConnectionManager.GetHubContext<DataHub>().Clients.All.updateProgressBarForOverall(meterName, value);
        }


        #endregion

        #region [ Setting Table Operations ]

        [RecordOperation(typeof(Setting), RecordOperation.QueryRecordCount)]
        public int QuerySettingCount(string filterText)
        {
            return DataContext.Table<Setting>().QueryRecordCount(filterText);
        }

        [RecordOperation(typeof(Setting), RecordOperation.QueryRecords)]
        public IEnumerable<Setting> QuerySetting(string sortField, bool ascending, int page, int pageSize, string filterText)
        {
            return DataContext.Table<Setting>().QueryRecords(sortField, ascending, page, pageSize, filterText);
        }

        [AuthorizeHubRole("Administrator, Editor")]
        [RecordOperation(typeof(Setting), RecordOperation.DeleteRecord)]
        public void DeleteSetting(int id)
        {
            DataContext.Table<Setting>().DeleteRecord(id);
        }

        [RecordOperation(typeof(Setting), RecordOperation.CreateNewRecord)]
        public Setting NewSetting()
        {
            return DataContext.Table<Setting>().NewRecord();
        }

        [AuthorizeHubRole("Administrator, Editor")]
        [RecordOperation(typeof(Setting), RecordOperation.AddNewRecord)]
        public void AddNewSetting(Setting record)
        {
            DataContext.Table<Setting>().AddNewRecord(record);
        }

        [AuthorizeHubRole("Administrator, Editor")]
        [RecordOperation(typeof(Setting), RecordOperation.UpdateRecord)]
        public void UpdateSetting(Setting record)
        {
            DataContext.Table<Setting>().UpdateRecord(record);
        }

        [AuthorizeHubRole("Administrator, Editor")]
        public void AddNewOrUpdateSetting(Setting record)
        {
            DataContext.Table<Setting>().AddNewOrUpdateRecord(record);
        }

        #endregion

        #region [ MetersWithHourlyLimits Table Operations ]

        [RecordOperation(typeof(MetersWithHourlyLimits), RecordOperation.QueryRecordCount)]
        public int QueryMetersWithHourlyLimitsCount(string filterText)
        {

            return DataContext.Table<MetersWithHourlyLimits>().QueryRecordCount(filterText);
        }

        [RecordOperation(typeof(MetersWithHourlyLimits), RecordOperation.QueryRecords)]
        public IEnumerable<MetersWithHourlyLimits> QueryMetersWithHourlyLimits(string sortField, bool ascending, int page, int pageSize, string filterText)
        {
            return DataContext.Table<MetersWithHourlyLimits>().QueryRecords(sortField, ascending, page, pageSize, filterText);
        }

        [AuthorizeHubRole("Administrator, Editor")]
        [RecordOperation(typeof(MetersWithHourlyLimits), RecordOperation.DeleteRecord)]
        public void DeleteMetersWithHourlyLimits(int id)
        {
            DataContext.Table<MetersWithHourlyLimits>().DeleteRecord(id);
        }

        [RecordOperation(typeof(MetersWithHourlyLimits), RecordOperation.CreateNewRecord)]
        public MetersWithHourlyLimits NewMetersWithHourlyLimits()
        {
            return DataContext.Table<MetersWithHourlyLimits>().NewRecord();
        }

        [AuthorizeHubRole("Administrator, Editor")]
        [RecordOperation(typeof(MetersWithHourlyLimits), RecordOperation.AddNewRecord)]
        public void AddNewMetersWithHourlyLimits(MetersWithHourlyLimits record)
        {
            DataContext.Table<MetersWithHourlyLimits>().AddNewRecord(record);
        }

        [AuthorizeHubRole("Administrator, Editor")]
        [RecordOperation(typeof(MetersWithHourlyLimits), RecordOperation.UpdateRecord)]
        public void UpdateMetersWithHourlyLimits(MetersWithHourlyLimits record)
        {
            DataContext.Table<MetersWithHourlyLimits>().UpdateRecord(record);
        }

        [AuthorizeHubRole("Administrator, Editor")]
        public void AddNewOrUpdateMetersWithHourlyLimits(MetersWithHourlyLimits record)
        {
            DataContext.Table<MetersWithHourlyLimits>().AddNewOrUpdateRecord(record);
        }

        public IEnumerable<Meter> GetMetersForSelect()
        {
            return DataContext.Table<Meter>().QueryRecords("Name", new RecordRestriction("ID IN (SELECT MeterID FROM UserMeter WHERE UserName = {0})", GetCurrentUserSID()));
        }

        public IEnumerable<MeasurementCharacteristic> GetCharacteristicsForSelect()
        {
            return DataContext.Table<MeasurementCharacteristic>().QueryRecords("Name");
        }


        public class TrendingData
        {
            public int ChannelID { get; set; }
            public string SeriesType { get; set; }
            public DateTime Time { get; set; }
            public double Value { get; set; }
        }

        public class RunningAvgStdDev
        {
            public int ChannelID { get; set; }
            public int HourOfWeek { get; set; }
            public double Sum { get; set; }
            public double Count { get; set; }
            public double SumOfSquares { get; set; }
            public double FirstPassStdDev { get; set; }
        }

        public void ProcessSmartAlarms(IEnumerable<int> meterIds, IEnumerable<int> typeIds, DateTime startDate, DateTime endDate, int sigmaLevel, int decimals, bool ignoreLargeValues, bool overwriteOldAlarms, int largeValueLevel)
        {

            int progressTotal = (meterIds.Any()? meterIds.Count() : 1 );
            int progressCount = 0;
            ProgressUpdatedOverall("",(int)(100 * (progressCount) / progressTotal));

            string historianServer = DataContext.Connection.ExecuteScalar<string>("SELECT Value FROM Setting WHERE Name = 'Historian.Server'") ?? "127.0.0.1";
            string historianInstance = DataContext.Connection.ExecuteScalar<string>("SELECT Value FROM Setting WHERE Name = 'Historian.Instance'") ?? "XDA";

            foreach (int meterId in meterIds)
            {
                string characteristicList = "(" + string.Join(",", typeIds) + ")";
                IEnumerable<int> channelIds = DataContext.Table<Channel>().QueryRecordsWhere("MeterID = {0} AND MeasurementCharacteristicID IN " + characteristicList, meterId).Select(x => x.ID);
                string meterName = DataContext.Connection.ExecuteScalar<string>("Select Name from Meter where ID = {0}", meterId);
                ProgressUpdatedOverall(meterName, (int)(100 * (progressCount) / progressTotal));
                List<TrendingData> trendingData = new List<TrendingData>();
                List<RunningAvgStdDev> runningData = new List<RunningAvgStdDev>();
                ProgressUpdatedByMeter("Querying openHistorian...", 0);
                using (openHistorian.XDALink.Historian historian = new Historian(historianServer, historianInstance))
                {
                    foreach (openHistorian.XDALink.TrendingDataPoint point in historian.Read(channelIds, startDate, endDate))
                    {
                        int hourOfWeek = (int)point.Timestamp.DayOfWeek * 24 + point.Timestamp.Hour;
                        RunningAvgStdDev record = runningData.FirstOrDefault(x => x.ChannelID == point.ChannelID && x.HourOfWeek == hourOfWeek);
                        if (record == null)
                        {
                            record = new RunningAvgStdDev()
                            {
                                ChannelID = point.ChannelID,
                                Count = 0,
                                Sum = 0,
                                SumOfSquares = 0
                            };
                            runningData.Add(record);
                        }

                        if (point.SeriesID.ToString() == "Average")
                        {
                            record.Sum += point.Value;
                            record.SumOfSquares += (point.Value * point.Value);
                            ++record.Count;

                        }
                    }

                    if (ignoreLargeValues)
                    {
                        runningData = runningData.Select(x =>
                        {
                            double average = x.Sum / (x.Count != 0 ? x.Count : 1);

                            x.FirstPassStdDev = Math.Sqrt(Math.Abs((x.SumOfSquares - 2 * average * x.Sum + x.Count * average * average) / ((x.Count != 1 ? x.Count : 2) - 1)));
                            x.Sum = 0;
                            x.SumOfSquares = 0;
                            x.Count = 0;
                            return x;
                        }).ToList();

                        ProgressUpdatedByMeter("Querying openHistorian for second pass...", 0);
                        foreach (openHistorian.XDALink.TrendingDataPoint point in historian.Read(channelIds, startDate, endDate))
                        {
                            int hourOfWeek = (int)point.Timestamp.DayOfWeek * 24 + point.Timestamp.Hour;
                            RunningAvgStdDev record = runningData.FirstOrDefault(x => x.ChannelID == point.ChannelID && x.HourOfWeek == hourOfWeek);
                            if ((point.SeriesID.ToString() == "Average" && point.Value > (record.FirstPassStdDev * largeValueLevel)) || (point.SeriesID.ToString() == "Average" && point.Value < (record.FirstPassStdDev * largeValueLevel))) continue;
                            if (record == null)
                            {
                                record = new RunningAvgStdDev()
                                {
                                    ChannelID = point.ChannelID,
                                    Count = 0,
                                    Sum = 0,
                                    SumOfSquares = 0
                                };
                                runningData.Add(record);
                            }

                            if (point.SeriesID.ToString() == "Average")
                            {
                                record.Sum += point.Value;
                                record.SumOfSquares += (point.Value * point.Value);
                                ++record.Count;

                            }
                        }
                    }
                }


                int innerProgressTotal = (channelIds.Any() ? channelIds.Count() : 1);
                int innerProgressCount = 0;

                foreach (int channelId in channelIds)
                {
                    string channelName = DataContext.Connection.ExecuteScalar<string>("Select Name from Channel where ID = {0}", channelId);
                    ProgressUpdatedByMeter(channelName, (int)(100 * (innerProgressCount) / innerProgressTotal));
                    foreach (RunningAvgStdDev data in runningData.Where(x => x.ChannelID == channelId))
                    {
                        double average = data.Sum / (data.Count != 0 ? data.Count : 1);

                        double stdDev = Math.Sqrt(Math.Abs((data.SumOfSquares - 2 * average * data.Sum + data.Count * average * average) / ((data.Count != 1 ? data.Count : 2) - 1)));
                        float high = (float)Math.Round(average + stdDev * sigmaLevel, decimals);
                        float low = (float)Math.Round(average - stdDev * sigmaLevel, decimals);


                        HourOfWeekLimit hwl = DataContext.Table<HourOfWeekLimit>().QueryRecordWhere("ChannelID = {0} AND HourOfWeek = {1}", data.ChannelID, data.HourOfWeek);

                        if (hwl == null)
                        {
                            HourOfWeekLimit newrecord = new HourOfWeekLimit()
                            {
                                ChannelID = data.ChannelID,
                                AlarmTypeID = 4,
                                HourOfWeek = data.HourOfWeek,
                                Severity = 1,
                                High = high,
                                Low = low,
                                Enabled = 1
                            };
                            DataContext.Table<HourOfWeekLimit>().AddNewRecord(newrecord);
                        }
                        else if (hwl != null && overwriteOldAlarms)
                        {
                            hwl.ChannelID = data.ChannelID;
                            hwl.AlarmTypeID = 4;
                            hwl.HourOfWeek = data.HourOfWeek;
                            hwl.Severity = 1;
                            hwl.High = high;
                            hwl.Low = low;
                            hwl.Enabled = 1;
                            DataContext.Table<HourOfWeekLimit>().UpdateRecord(hwl);
                        }
                    }

                    ProgressUpdatedByMeter(channelName, (int)(100 * (++innerProgressCount) / innerProgressTotal));
                }
                ProgressUpdatedOverall(meterName, (int)(100 * (++progressCount) / progressTotal));
            }
        }

        #endregion

        #region [ ChannelsWithHourlyLimits Table Operations ]

        [RecordOperation(typeof(ChannelsWithHourlyLimits), RecordOperation.QueryRecordCount)]
        public int QueryChannelsWithHourlyLimitsCount(int meterId, string filterText)
        {
            TableOperations<ChannelsWithHourlyLimits> table = DataContext.Table<ChannelsWithHourlyLimits>();
            RecordRestriction restriction = table.GetSearchRestriction(filterText);

            return table.QueryRecordCount(new RecordRestriction("MeterID = {0}", meterId) + restriction);
        }

        [RecordOperation(typeof(ChannelsWithHourlyLimits), RecordOperation.QueryRecords)]
        public IEnumerable<ChannelsWithHourlyLimits> QueryChannelsWithHourlyLimits(int meterId, string sortField, bool ascending, int page, int pageSize, string filterText)
        {
            TableOperations<ChannelsWithHourlyLimits> table = DataContext.Table<ChannelsWithHourlyLimits>();
            RecordRestriction restriction = table.GetSearchRestriction(filterText);

            return table.QueryRecords(sortField, ascending, page, pageSize, new RecordRestriction("MeterID = {0}", meterId) + restriction);
        }

        [AuthorizeHubRole("Administrator, Editor")]
        [RecordOperation(typeof(ChannelsWithHourlyLimits), RecordOperation.DeleteRecord)]
        public void DeleteChannelsWithHourlyLimits(int id)
        {
            DataContext.Table<ChannelsWithHourlyLimits>().DeleteRecord(id);
        }

        [RecordOperation(typeof(ChannelsWithHourlyLimits), RecordOperation.CreateNewRecord)]
        public ChannelsWithHourlyLimits NewChannelsWithHourlyLimits()
        {
            return DataContext.Table<ChannelsWithHourlyLimits>().NewRecord();
        }

        [AuthorizeHubRole("Administrator, Editor")]
        [RecordOperation(typeof(ChannelsWithHourlyLimits), RecordOperation.AddNewRecord)]
        public void AddNewChannelsWithHourlyLimits(ChannelsWithHourlyLimits record)
        {
            DataContext.Table<ChannelsWithHourlyLimits>().AddNewRecord(record);
        }

        [AuthorizeHubRole("Administrator, Editor")]
        [RecordOperation(typeof(ChannelsWithHourlyLimits), RecordOperation.UpdateRecord)]
        public void UpdateChannelsWithHourlyLimits(ChannelsWithHourlyLimits record)
        {
            DataContext.Table<ChannelsWithHourlyLimits>().UpdateRecord(record);
        }

        [AuthorizeHubRole("Administrator, Editor")]
        public void AddNewOrUpdateChannelsWithHourlyLimits(ChannelsWithHourlyLimits record)
        {
            DataContext.Table<ChannelsWithHourlyLimits>().AddNewOrUpdateRecord(record);
        }

        #endregion

        #region [ HourOfWeekLimit Table Operations ]

        [RecordOperation(typeof(HourOfWeekLimit), RecordOperation.QueryRecordCount)]
        public int QueryHourOfWeekLimitCount(int channelId, string filterText)
        {
            TableOperations<HourOfWeekLimit> table = DataContext.Table<HourOfWeekLimit>();
            RecordRestriction restriction = table.GetSearchRestriction(filterText);

            return table.QueryRecordCount(new RecordRestriction("ChannelID = {0}", channelId) + restriction);
        }

        [RecordOperation(typeof(HourOfWeekLimit), RecordOperation.QueryRecords)]
        public IEnumerable<HourOfWeekLimitView> QueryHourOfWeekLimit(int channelId, string sortField, bool ascending, int page, int pageSize, string filterText)
        {
            TableOperations<HourOfWeekLimitView> table = DataContext.Table<HourOfWeekLimitView>();
            RecordRestriction restriction = table.GetSearchRestriction(filterText);

            return table.QueryRecords(sortField, ascending, page, pageSize, new RecordRestriction("ChannelID = {0}", channelId) + restriction);
        }

        [AuthorizeHubRole("Administrator, Editor")]
        [RecordOperation(typeof(HourOfWeekLimit), RecordOperation.DeleteRecord)]
        public void DeleteHourOfWeekLimit(int id)
        {
            DataContext.Table<HourOfWeekLimit>().DeleteRecord(id);
        }

        [RecordOperation(typeof(HourOfWeekLimit), RecordOperation.CreateNewRecord)]
        public HourOfWeekLimit NewHourOfWeekLimit()
        {
            return DataContext.Table<HourOfWeekLimit>().NewRecord();
        }

        [AuthorizeHubRole("Administrator, Editor")]
        [RecordOperation(typeof(HourOfWeekLimit), RecordOperation.AddNewRecord)]
        public void AddNewHourOfWeekLimit(HourOfWeekLimit record)
        {
            DataContext.Table<HourOfWeekLimit>().AddNewRecord(record);
        }

        [AuthorizeHubRole("Administrator, Editor")]
        [RecordOperation(typeof(HourOfWeekLimit), RecordOperation.UpdateRecord)]
        public void UpdateHourOfWeekLimit(HourOfWeekLimit record)
        {
            DataContext.Table<HourOfWeekLimit>().UpdateRecord(record);
        }

        [AuthorizeHubRole("Administrator, Editor")]
        public void AddNewOrUpdateHourOfWeekLimit(HourOfWeekLimit record)
        {
            DataContext.Table<HourOfWeekLimit>().AddNewOrUpdateRecord(record);
        }

        #endregion

        #region [ MetersWithNormalLimits Table Operations ]

        [RecordOperation(typeof(MetersWithNormalLimits), RecordOperation.QueryRecordCount)]
        public int QueryMetersWithNormalLimitsCount(string filterText)
        {

            return DataContext.Table<MetersWithNormalLimits>().QueryRecordCount(filterText);
        }

        [RecordOperation(typeof(MetersWithNormalLimits), RecordOperation.QueryRecords)]
        public IEnumerable<MetersWithNormalLimits> QueryMetersWithNormalLimits(string sortField, bool ascending, int page, int pageSize, string filterText)
        {
            return DataContext.Table<MetersWithNormalLimits>().QueryRecords(sortField, ascending, page, pageSize, filterText);
        }

        [AuthorizeHubRole("Administrator, Editor")]
        [RecordOperation(typeof(MetersWithNormalLimits), RecordOperation.DeleteRecord)]
        public void DeleteMetersWithNormalLimits(int id)
        {
            DataContext.Table<MetersWithNormalLimits>().DeleteRecord(id);
        }

        [RecordOperation(typeof(MetersWithNormalLimits), RecordOperation.CreateNewRecord)]
        public MetersWithNormalLimits NewMetersWithNormalLimits()
        {
            return DataContext.Table<MetersWithNormalLimits>().NewRecord();
        }

        [AuthorizeHubRole("Administrator, Editor")]
        [RecordOperation(typeof(MetersWithNormalLimits), RecordOperation.AddNewRecord)]
        public void AddNewMetersWithNormalLimits(MetersWithNormalLimits record)
        {
            DataContext.Table<MetersWithNormalLimits>().AddNewRecord(record);
        }

        [AuthorizeHubRole("Administrator, Editor")]
        [RecordOperation(typeof(MetersWithNormalLimits), RecordOperation.UpdateRecord)]
        public void UpdateMetersWithNormalLimits(MetersWithNormalLimits record)
        {
            DataContext.Table<MetersWithNormalLimits>().UpdateRecord(record);
        }

        [AuthorizeHubRole("Administrator, Editor")]
        public void AddNewOrUpdateMetersWithNormalLimits(MetersWithNormalLimits record)
        {
            DataContext.Table<MetersWithNormalLimits>().AddNewOrUpdateRecord(record);
        }

        public void ProcessSmartAlarmsNormal(IEnumerable<int> meterIds, IEnumerable<int> typeIds, DateTime startDate, DateTime endDate, int sigmaLevel, int decimals, bool ignoreLargeValues, bool overwriteOldAlarms, int largeValueLevel)
        {

            int progressTotal = (meterIds.Any() ? meterIds.Count() : 1);
            int progressCount = 0;
            ProgressUpdatedOverall("", (int)(100 * (progressCount) / progressTotal));

            string historianServer = DataContext.Connection.ExecuteScalar<string>("SELECT Value FROM Setting WHERE Name = 'Historian.Server'") ?? "127.0.0.1";
            string historianInstance = DataContext.Connection.ExecuteScalar<string>("SELECT Value FROM Setting WHERE Name = 'Historian.Instance'") ?? "XDA";

            foreach (int meterId in meterIds)
            {
                string characteristicList = "(" + string.Join(",", typeIds) + ")";
                IEnumerable<int> channelIds = DataContext.Table<Channel>().QueryRecordsWhere("MeterID = {0} AND MeasurementCharacteristicID IN " + characteristicList, meterId).Select(x => x.ID);
                string meterName = DataContext.Connection.ExecuteScalar<string>("Select Name from Meter where ID = {0}", meterId);
                ProgressUpdatedOverall(meterName, (int)(100 * (progressCount) / progressTotal));
                List<TrendingData> trendingData = new List<TrendingData>();
                List<RunningAvgStdDev> normalRunningData = new List<RunningAvgStdDev>();
                ProgressUpdatedByMeter("Querying openHistorian...", 0);
                using (openHistorian.XDALink.Historian historian = new Historian(historianServer, historianInstance))
                {
                    foreach (openHistorian.XDALink.TrendingDataPoint point in historian.Read(channelIds, startDate, endDate))
                    {
                        RunningAvgStdDev normalRecord = normalRunningData.FirstOrDefault(x => x.ChannelID == point.ChannelID);
                        if (normalRecord == null)
                        {
                            normalRecord = new RunningAvgStdDev()
                            {
                                ChannelID = point.ChannelID,
                                Count = 0,
                                Sum = 0,
                                SumOfSquares = 0
                            };
                            normalRunningData.Add(normalRecord);
                        }

                        if (point.SeriesID.ToString() == "Average")
                        {
                            normalRecord.Sum += point.Value;
                            normalRecord.SumOfSquares += (point.Value * point.Value);
                            ++normalRecord.Count;

                        }
                    }

                    if (ignoreLargeValues)
                    {

                        normalRunningData = normalRunningData.Select(x =>
                        {
                            double average = x.Sum / (x.Count != 0 ? x.Count : 1);

                            x.FirstPassStdDev = Math.Sqrt(Math.Abs((x.SumOfSquares - 2 * average * x.Sum + x.Count * average * average) / ((x.Count != 1 ? x.Count : 2) - 1)));
                            x.Count = 0;
                            x.Sum = 0;
                            x.SumOfSquares = 0;
                            return x;
                        }).ToList();



                        ProgressUpdatedByMeter("Querying openHistorian for second pass...", 0);
                        foreach (openHistorian.XDALink.TrendingDataPoint point in historian.Read(channelIds, startDate, endDate))
                        {
                            int hourOfWeek = (int)point.Timestamp.DayOfWeek * 24 + point.Timestamp.Hour;
                            RunningAvgStdDev normalRecord = normalRunningData.FirstOrDefault(x => x.ChannelID == point.ChannelID);
                            if ((point.SeriesID.ToString() == "Average" && point.Value > (normalRecord.FirstPassStdDev * largeValueLevel)) || (point.SeriesID.ToString() == "Average" && point.Value < (normalRecord.FirstPassStdDev * largeValueLevel))) continue;
                            if (point.SeriesID.ToString() == "Average")
                            {
                                normalRecord.Sum += point.Value;
                                normalRecord.SumOfSquares += (point.Value * point.Value);
                                ++normalRecord.Count;
                            }
                        }
                    }
                }

                int innerProgressTotal = (channelIds.Any() ? channelIds.Count() : 1);
                int innerProgressCount = 0;

                foreach (int channelId in channelIds)
                {
                    string channelName = DataContext.Connection.ExecuteScalar<string>("Select Name from Channel where ID = {0}", channelId);
                    ProgressUpdatedByMeter(channelName, (int)(100 * (innerProgressCount) / innerProgressTotal));
                    RunningAvgStdDev record = normalRunningData.Where(x => x.ChannelID == channelId).FirstOrDefault();
                    if (record != null)
                    {
                        double average = record.Sum / (record.Count != 0 ? record.Count : 1);

                        double stdDev = Math.Sqrt(Math.Abs((record.SumOfSquares - 2 * average * record.Sum + record.Count * average * average) / ((record.Count != 1 ? record.Count : 2) - 1)));
                        float high = (float)Math.Round(average + stdDev * sigmaLevel, decimals);
                        float low = (float)Math.Round(average - stdDev * sigmaLevel, decimals);

                        AlarmRangeLimit hwl = DataContext.Table<AlarmRangeLimit>().QueryRecordWhere("ChannelID = {0}", record.ChannelID);

                        if (hwl == null)
                        {
                            AlarmRangeLimit newRecord = new AlarmRangeLimit()
                            {
                                ChannelID = record.ChannelID,
                                AlarmTypeID = 5,
                                Severity = 1,
                                High = high,
                                Low = low,
                                RangeInclusive = 0,
                                PerUnit = 0,
                                Enabled = 1,
                                IsDefault = false
                            };
                            DataContext.Table<AlarmRangeLimit>().AddNewRecord(newRecord);
                        }
                        else if (hwl != null && overwriteOldAlarms)
                        {
                            hwl.High = high;
                            hwl.Low = low;
                            DataContext.Table<AlarmRangeLimit>().UpdateRecord(hwl);
                        }

                    }

                    ProgressUpdatedByMeter(channelName, (int)(100 * (++innerProgressCount) / innerProgressTotal));
                }
                ProgressUpdatedOverall(meterName, (int)(100 * (++progressCount) / progressTotal));
            }
        }

        #endregion

        #region [ ChannelsWithNormalLimits Table Operations ]

        [RecordOperation(typeof(ChannelsWithNormalLimits), RecordOperation.QueryRecordCount)]
        public int QueryChannelsWithNormalLimitsCount(int meterId, string filterText)
        {
            TableOperations<ChannelsWithNormalLimits> table = DataContext.Table<ChannelsWithNormalLimits>();
            RecordRestriction restriction = table.GetSearchRestriction(filterText);

            return table.QueryRecordCount(new RecordRestriction("MeterID = {0}", meterId) + restriction);
        }

        [RecordOperation(typeof(ChannelsWithNormalLimits), RecordOperation.QueryRecords)]
        public IEnumerable<ChannelsWithNormalLimits> QueryChannelsWithNormalLimits(int meterId, string sortField, bool ascending, int page, int pageSize, string filterText)
        {
            TableOperations<ChannelsWithNormalLimits> table = DataContext.Table<ChannelsWithNormalLimits>();
            RecordRestriction restriction = table.GetSearchRestriction(filterText);

            return table.QueryRecords(sortField, ascending, page, pageSize, new RecordRestriction("MeterID = {0}", meterId) + restriction);
        }

        [AuthorizeHubRole("Administrator, Editor")]
        [RecordOperation(typeof(ChannelsWithNormalLimits), RecordOperation.DeleteRecord)]
        public void DeleteChannelsWithNormalLimits(int id)
        {
            DataContext.Table<ChannelsWithNormalLimits>().DeleteRecord(id);
        }

        [RecordOperation(typeof(ChannelsWithNormalLimits), RecordOperation.CreateNewRecord)]
        public ChannelsWithNormalLimits NewChannelsWithNormalLimits()
        {
            return DataContext.Table<ChannelsWithNormalLimits>().NewRecord();
        }

        [AuthorizeHubRole("Administrator, Editor")]
        [RecordOperation(typeof(ChannelsWithNormalLimits), RecordOperation.AddNewRecord)]
        public void AddNewChannelsWithNormalLimits(ChannelsWithNormalLimits record)
        {
            DataContext.Table<ChannelsWithNormalLimits>().AddNewRecord(record);
        }

        [AuthorizeHubRole("Administrator, Editor")]
        [RecordOperation(typeof(ChannelsWithNormalLimits), RecordOperation.UpdateRecord)]
        public void UpdateChannelsWithNormalLimits(ChannelsWithNormalLimits record)
        {
            DataContext.Table<ChannelsWithNormalLimits>().UpdateRecord(record);
        }

        [AuthorizeHubRole("Administrator, Editor")]
        public void AddNewOrUpdateChannelsWithNormalLimits(ChannelsWithNormalLimits record)
        {
            DataContext.Table<ChannelsWithNormalLimits>().AddNewOrUpdateRecord(record);
        }

        #endregion

        #region [ Security Table Operations ]

        [RecordOperation(typeof(Security), RecordOperation.QueryRecordCount)]
        public int QuerySecurityCount(string filterText)
        {
            IEnumerable<Security> table = new List<Security>
            {
                new Security() {
                ID = 1,
                Name = "Users"
                },
                new Security() {
                ID = 2,
                Name = "Groups"
                }
            };

            return table.Count();
        }

        [RecordOperation(typeof(Security), RecordOperation.QueryRecords)]
        public IEnumerable<Security> QuerySecurity(string sortField, bool ascending, int page, int pageSize, string filterText)
        {
            return new List<Security>
            {
                new Security() {
                ID = 1,
                Name = "Users"
                },
                new Security() {
                ID = 2,
                Name = "Groups"
                }
            };
        }

        [AuthorizeHubRole("Administrator, Editor")]
        [RecordOperation(typeof(Security), RecordOperation.DeleteRecord)]
        public void DeleteSecurity(int id)
        {
        }

        [RecordOperation(typeof(Security), RecordOperation.CreateNewRecord)]
        public Security NewSecurity()
        {
            return new Security();
        }

        [AuthorizeHubRole("Administrator, Editor")]
        [RecordOperation(typeof(Security), RecordOperation.AddNewRecord)]
        public void AddNewSecurity(Security record)
        {
        }

        [AuthorizeHubRole("Administrator, Editor")]
        [RecordOperation(typeof(Security), RecordOperation.UpdateRecord)]
        public void UpdateSecurity(Security record)
        {
        }

        [AuthorizeHubRole("Administrator, Editor")]
        public void AddNewOrUpdateSecurity(Security record)
        {
        }

        #endregion

        #region [ DirectoryBrowser Hub Operations ]

        public IEnumerable<string> LoadDirectories(string rootFolder, bool showHidden, bool showFiles)
        {
            if (string.IsNullOrWhiteSpace(rootFolder))
                return Directory.GetLogicalDrives();

            IEnumerable<string> directories = Directory.GetDirectories(rootFolder);
            IEnumerable<string> files = Directory.GetFiles(rootFolder);
            if (!showHidden)
                directories = directories.Where(path => !new DirectoryInfo(path).Attributes.HasFlag(FileAttributes.Hidden));

            IEnumerable<string> returnString = new[] { "..\\" }.Concat(directories.Select(path => FilePath.AddPathSuffix(FilePath.GetLastDirectoryName(path))));

            if (showFiles)
                returnString = returnString.Concat(files.Select(file => file.Split('\\')[file.Split('\\').Count() - 1]));

            return returnString;
        }

        public IEnumerable<string> LoadDirectories(string rootFolder, bool showHidden) => DirectoryBrowserOperations.LoadDirectories(rootFolder, showHidden);
        public bool IsLogicalDrive(string path) => DirectoryBrowserOperations.IsLogicalDrive(path);
        public string ResolvePath(string path) => DirectoryBrowserOperations.ResolvePath(path);
        public string CombinePath(string path1, string path2) => DirectoryBrowserOperations.CombinePath(path1, path2);
        public void CreatePath(string path) => DirectoryBrowserOperations.CreatePath(path);

        #endregion

        #region [ Miscellaneous Hub Operations ]

        /// <summary>
        /// Gets UserAccount table ID for current user.
        /// </summary>
        /// <returns>UserAccount.ID for current user.</returns>
        public Guid GetCurrentUserID()
        {
            Guid userID;
            AuthorizationCache.UserIDs.TryGetValue(Thread.CurrentPrincipal.Identity.Name, out userID);
            return userID;
        }

        /// <summary>
        /// Gets the current server time.
        /// </summary>
        /// <returns>Current server time.</returns>
        public DateTime GetServerTime() => DateTime.UtcNow;

        private void CascadeDelete(string tableName, string criterion)
        {

            using (IDbCommand sc = DataContext.Connection.Connection.CreateCommand())
            {

                //sc.CommandText = "DECLARE @context VARBINARY(128)\n SELECT @context = CONVERT(VARBINARY(128), CONVERT(VARCHAR(128), @userName))\n SET CONTEXT_INFO @context";
                //IDbDataParameter param = sc.CreateParameter();
                //param.ParameterName = "@userName";
                //param.Value = GetCurrentUserName();
                //sc.Parameters.Add(param);
                //sc.ExecuteNonQuery();
                //sc.Parameters.Clear();


                sc.CommandText = "dbo.UniversalCascadeDelete";
                sc.CommandType = CommandType.StoredProcedure;
                IDbDataParameter param1 = sc.CreateParameter();
                param1.ParameterName = "@tableName";
                param1.Value = tableName;
                IDbDataParameter param2 = sc.CreateParameter();
                param2.ParameterName = "@baseCriteria";
                param2.Value = criterion;
                sc.Parameters.Add(param1);
                sc.Parameters.Add(param2);
                sc.ExecuteNonQuery();
            }
        }

        private DataTable CSVToDataTable(string fileName)
        {
            var streamReader = new StreamReader(fileName);
            var csv = new CsvParser(streamReader);
            DataTable dt = new DataTable();
            string[] headers = new string[0];
            while (true)
            {
                var row = csv.Read();
                if (row == null) break;

                if (csv.Context.Row == 1)
                {
                    foreach (string header in row)
                        dt.Columns.Add(header, typeof(string));
                    headers = row;
                }
                else
                {
                    var record = dt.NewRow();
                    for (int x = 0; x < row.Count(); ++x)
                    {
                        record[headers[x]] = row[x];
                    }
                    dt.Rows.Add(record);

                }

            }

            return dt;
        }

        /// <summary>
        /// Gets UserAccount table SID for current user.
        /// </summary>
        /// <returns>UserAccount.ID for current user.</returns>
        public static string GetCurrentUserSID()
        {
            return UserInfo.UserNameToSID(Thread.CurrentPrincipal.Identity.Name);
        }

        #endregion


    }

    public class ServiceHub: Hub
    {
        public class DateTimeArg: EventArgs
        {
            public DateTime DateTime { get; set; }
        }

        #region [ Static Event Handlers ]
        public static event EventHandler<DateTimeArg> GetServerTimeEvent;
        #endregion

        public DateTime GetServerTime()
        {
            DateTimeArg dta = new DateTimeArg();
            GetServerTimeEvent?.Invoke(new object(), dta);
            return dta.DateTime;
        }

        public string GetPerformanceStatistics()
        {
            return null;
        }

        public void SendCommand(string command)
        {
            
        }

    }

    public static class Extensions
    {
        public static double StdDev(this IEnumerable<double> values)
        {
            double ret = 0;
            int count = values.Count();
            if (count > 1)
            {
                //Compute the Average
                double avg = values.Average();

                //Perform the Sum of (value-avg)^2
                double sum = values.Sum(d => (d - avg) * (d - avg));

                //Put it all together
                ret = Math.Sqrt(sum / count);
            }
            return ret;
        }
    }
}
