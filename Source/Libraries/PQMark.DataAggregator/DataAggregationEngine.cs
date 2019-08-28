//******************************************************************************************************
//  DataAggregationEngine.cs - Gbtc
//
//  Copyright © 2016, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the MIT License (MIT), the "License"; you may not use this
//  file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://opensource.org/licenses/MIT
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  08/11/2017 - Billy Ernest
//       Created class.
//
//******************************************************************************************************

using GSF;
using GSF.Configuration;
using GSF.Data;
using GSF.Data.Model;
using GSF.Scheduling;
using GSF.Web.Model;
using openHistorian.XDALink;
using openXDA.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;

namespace PQMark.DataAggregator
{
    public class DataAggregationEngine : IDisposable
    {
        #region [ Members ]

        // Fields
        private DataContext m_dataContext;
        private bool m_disposed;
        private ScheduleManager m_scheduler;
        private bool m_running = false;
        private PQMarkAggregationSettings m_pqMarkAggregationSettings;

        private class DisturbanceData
        {
            public int MeterID { get; set; }
            public double PerUnitMagnitude { get; set; }
            public DateTime StartTime { get; set; }
            public DateTime EndTime { get; set; }
            public double DurationSeconds { get; set; }
            public double DurationCycles { get; set; }
        }

        private class Curves
        {
            public class Point
            {
                public double Magnitude { get; set; }
                public double Duration { get; set; }
                public Point(double mag, double dur)
                {
                    Magnitude = mag;
                    Duration = dur;
                }
            }
            public static List<Point> IticUpperCurve { get; }
            public static List<Point> IticLowerCurve { get; }
            public static List<Point> SemiCurve { get; }

            static Curves()
            {
                IticUpperCurve = new List<Point>()
                {
                    new Point(2.0,0.001),
                    new Point(1.4,0.003),
                    new Point(1.2,0.003),
                    new Point(1.2,0.5),
                    new Point(1.1,0.5),
                };
                IticLowerCurve = new List<Point>()
                {
                    new Point(0.0,0.02),
                    new Point(0.7,0.02),
                    new Point(0.7,0.5),
                    new Point(0.8,0.5),
                    new Point(0.8,10.0),
                    new Point(0.9,10.0),
                };
                SemiCurve = new List<Point>()
                {
                    new Point(0.0,0.02),
                    new Point(0.5,0.02),
                    new Point(0.5,0.2),
                    new Point(0.7,0.2),
                    new Point(0.7,0.5),
                    new Point(0.8,0.5),
                    new Point(0.8,10.0),
                    new Point(0.9,10.0),
                };

            }
        }


        #endregion

        #region [ Constructors ]
        public DataAggregationEngine()
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                HistorianServer = connection.ExecuteScalar<string>("SELECT Value FROM Setting WHERE Name = 'Historian.Server'") ?? "127.0.0.1";
                HistorianInstance = connection.ExecuteScalar<string>("SELECT Value FROM Setting WHERE Name = 'Historian.Instance'") ?? "XDA";

                Meters = new TableOperations<PQMarkCompanyMeter>(connection).QueryRecordsWhere("Enabled = 1");

                m_pqMarkAggregationSettings = new PQMarkAggregationSettings();
            }
        }

        public DataAggregationEngine(PQMarkAggregationSettings settings)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                HistorianServer = connection.ExecuteScalar<string>("SELECT Value FROM Setting WHERE Name = 'Historian.Server'") ?? "127.0.0.1";
                HistorianInstance = connection.ExecuteScalar<string>("SELECT Value FROM Setting WHERE Name = 'Historian.Instance'") ?? "XDA";

                Meters = new TableOperations<PQMarkCompanyMeter>(connection).QueryRecordsWhere("Enabled = 1");

                m_pqMarkAggregationSettings = settings;
            }
        }

        #endregion

        #region [ Properties ]
        private ScheduleManager Scheduler => m_scheduler ?? (m_scheduler = new ScheduleManager());
        public bool Running => m_running;
        private IEnumerable<PQMarkCompanyMeter> Meters { get; set; }
        private string HistorianServer { get; set; }
        private string HistorianInstance { get; set; }

        [Category]
        [SettingName(PQMarkAggregationSettings.CategoryName)]
        public PQMarkAggregationSettings PQMarkAggregationSettings => m_pqMarkAggregationSettings;

        #endregion

        #region [ Static ]

        public static event EventHandler<EventArgs<string>> LogStatusMessageEvent;

        private static void OnLogStatusMessage(string message)
        {
            LogStatusMessageEvent?.Invoke(new object(), new EventArgs<string>(message));
        }

        public static event EventHandler<EventArgs<Exception>> LogExceptionMessage;

        private static void OnLogExceptionMessage(Exception exception)
        {
            LogExceptionMessage?.Invoke(new object(), new EventArgs<Exception>(exception));
        }

        #endregion

        #region [ Methods ]
        public void Dispose()
        {
            if (!m_disposed)
            {
                try
                {
                    Stop();
                    m_dataContext.Dispose();
                    m_disposed = true;
                }
                catch (Exception ex)
                {
                    OnLogStatusMessage(ex.ToString());
                }
            }
        }


        public void Start()
        {
            if (!Running)
            {
                Scheduler.Initialize();
                Scheduler.Starting += Scheduler_Starting;
                Scheduler.Started += Scheduler_Started;
                Scheduler.ScheduleDue += Scheduler_ScheduleDue;
                Scheduler.Disposed += Scheduler_Disposed;
                Scheduler.AddSchedule("PQMarkAggregation", PQMarkAggregationSettings.Frequency);
                Scheduler.Start();
                m_running = true;
            }
        }

        public void Stop()
        {
            if (Running)
            {
                Scheduler.Stop();
                m_running = false;
            }
        }

        public void ReloadSystemSettings()
        {
        }

        private void Scheduler_Started(object sender, EventArgs e)
        {
            OnLogStatusMessage("PQMark Data Aggregator Scheduler has started successfully...");
        }

        private void Scheduler_Starting(object sender, EventArgs e)
        {
            OnLogStatusMessage("PQMark Data Aggregator Scheduler is starting...");
        }

        private void Scheduler_Disposed(object sender, EventArgs e)
        {
            OnLogStatusMessage("PQMark Data Aggregator Scheduler is disposed...");
        }


        private void Scheduler_ScheduleDue(object sender, EventArgs<Schedule> e)
        {
            OnLogStatusMessage(string.Format("PQMark month to date data aggregation is due..."));
            ProcessMonthToDateData();

        }

        public void ProcessAllData()
        {
            using(AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                if (!Meters.Any()) return;

                DataTable table = connection.RetrieveData(
                    @"
                        SELECT	Event.MeterID, Disturbance.PerUnitMagnitude, Disturbance.StartTime, Disturbance.EndTime, Disturbance.DurationSeconds, Disturbance.DurationCycles
                        FROM  	Disturbance JOIN
		                        Event ON Event.ID = Disturbance.EventID
                        WHERE   Disturbance.PhaseID = (SELECT ID FROM Phase WHERE Name = 'Worst') AND
		                        Event.MeterID IN (" + string.Join(",", Meters.Select(x => x.MeterID)) + ")"
                );
                IEnumerable<DisturbanceData> dd = table.Select().Select(row => new TableOperations<DisturbanceData>(connection).LoadRecord(row));

                var assetGroups = dd.GroupBy(x => x.MeterID);

                foreach (var assetGroup in assetGroups)
                {
                    OnLogStatusMessage(string.Format("PQMark aggregation: Processing {0}", assetGroup.Key));
                    foreach (var yearGroup in assetGroup.GroupBy(x => x.StartTime.Year))
                    {
                        foreach (var dateGroup in yearGroup.GroupBy(x => x.StartTime.Month))
                        {
                            DateTime startDate = new DateTime(yearGroup.Key, dateGroup.Key, 1);
                            DateTime endDate = startDate.AddMonths(1).AddSeconds(-1);

                            ProcessMonthOfData(dateGroup, startDate, endDate);
                        }

                    }
                }

                OnLogStatusMessage(string.Format("PQMark data aggregation is complete..."));


            }
        }

        public void ProcessAllEmptyData()
        {
            if (!Meters.Any()) return;

            using(AdoDataConnection connection = new AdoDataConnection("systemSetting"))
            {
                DataTable table = connection.RetrieveData(
                    @"
                        SELECT	Event.MeterID, Disturbance.PerUnitMagnitude, Disturbance.StartTime, Disturbance.EndTime, Disturbance.DurationSeconds, Disturbance.DurationCycles
                        FROM  	Disturbance JOIN
		                        Event ON Event.ID = Disturbance.EventID
                        WHERE	Disturbance.PhaseID = (SELECT ID FROM Phase WHERE Name = 'Worst') AND
		                        Event.MeterID IN (" + string.Join(",", Meters.Select(x => x.MeterID)) + ")"
                );
                IEnumerable<DisturbanceData> dd = table.Select().Select(row => new TableOperations<DisturbanceData>(connection).LoadRecord(row));

                foreach (var assetGroup in dd.GroupBy(x => x.MeterID))
                {
                    OnLogStatusMessage(string.Format("PQMark aggregation: Processing {0}", assetGroup.Key));
                    foreach (var yearGroup in assetGroup.GroupBy(x => x.StartTime.Year))
                    {
                        var dateGroups = yearGroup.GroupBy(x => x.StartTime.Month);
                        foreach (var dateGroup in dateGroups)
                        {
                            PQMarkAggregate record = new TableOperations<PQMarkAggregate>(connection).QueryRecordWhere("MeterID = {0} AND Year = {1} AND Month = {2}", assetGroup.Key, yearGroup.Key, dateGroup.Key);
                            if (record != null) continue;

                            DateTime startDate = new DateTime(yearGroup.Key, dateGroup.Key, 1);
                            DateTime endDate = startDate.AddMonths(1).AddSeconds(-1);

                            ProcessMonthOfData(dateGroup, startDate, endDate);
                        }

                    }
                }


            }
            OnLogStatusMessage(string.Format("PQMark data aggregation is complete..."));

        }

        public void ProcessMonthToDateData()
        {
            if (!Meters.Any()) return;
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                DateTime endDate = DateTime.UtcNow;
                DateTime startDate = new DateTime(endDate.Year, endDate.Month, 1);


                DataTable table = connection.RetrieveData(
                    @"
                        SELECT	Event.MeterID, Disturbance.PerUnitMagnitude, Disturbance.StartTime, Disturbance.EndTime, Disturbance.DurationSeconds, Disturbance.DurationCycles
                        FROM  	Disturbance JOIN
		                        Event ON Event.ID = Disturbance.EventID
                        WHERE	Disturbance.PhaseID = (SELECT ID FROM Phase WHERE Name = 'Worst') AND
		                        Event.MeterID IN (" + string.Join(",", Meters.Select(x => x.MeterID)) + @") AND
                                Event.StartTime Between '" + startDate + @"' AND '" + endDate + @"'"
                );
                IEnumerable<DisturbanceData> dd = table.Select().Select(row => new TableOperations<DisturbanceData>(connection).LoadRecord(row));

                foreach (var assetGroup in dd.GroupBy(x => x.MeterID))
                    ProcessMonthOfData(assetGroup, startDate, endDate);
            }
            OnLogStatusMessage(string.Format("PQMark data aggregation is complete..."));

        }

        private void ProcessMonthOfData(IGrouping<int, DisturbanceData> assetGroup, DateTime startDate, DateTime endDate) {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings")) {
                OnLogStatusMessage(string.Format("PQMark aggregation: Processing {0}", assetGroup.Key));

                PQMarkAggregate record = new TableOperations<PQMarkAggregate>(connection).QueryRecordWhere("MeterID = {0} AND Year = {1} AND Month = {2}", assetGroup.Key, startDate.Year, startDate.Month);
                if (record == null) record = new PQMarkAggregate();

                record.MeterID = assetGroup.Key;
                record.Year = startDate.Year;
                record.Month = startDate.Month;
                record.ITIC = GetCounts(assetGroup, Curves.IticLowerCurve) + GetCounts(assetGroup, Curves.IticUpperCurve);
                record.SEMI = GetCounts(assetGroup, Curves.SemiCurve);
                record.SARFI90 = assetGroup.Where(x => x.PerUnitMagnitude <= 0.9).Count();
                record.SARFI70 = assetGroup.Where(x => x.PerUnitMagnitude <= 0.7).Count();
                record.SARFI50 = assetGroup.Where(x => x.PerUnitMagnitude <= 0.5).Count();
                record.SARFI10 = assetGroup.Where(x => x.PerUnitMagnitude <= 0.1).Count();
                record.THDJson = GetTHDJsonString(assetGroup, startDate, endDate);
                new TableOperations<PQMarkAggregate>(connection).AddNewOrUpdateRecord(record);
            }
        }

        private IEnumerable<DisturbanceData> GetDisturbanceDatas(DateTime startDate, DateTime endDate) {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings")) {
                IEnumerable<PQMarkCompanyMeter> meters = new TableOperations<PQMarkCompanyMeter>(connection).QueryRecordsWhere("Enabled = 1");

                if (!meters.Any()) return null;

                DataTable table = connection.RetrieveData(
                    @"SELECT	Event.MeterID, Disturbance.PerUnitMagnitude, Disturbance.StartTime, Disturbance.EndTime, Disturbance.DurationSeconds, Disturbance.DurationCycles
                  FROM  	Disturbance JOIN
		                    Event ON Event.ID = Disturbance.EventID
                  WHERE	    Disturbance.PhaseID = (SELECT ID FROM Phase WHERE Name = 'Worst') AND
		                    Event.MeterID IN (" + string.Join(",", meters.Select(x => x.MeterID)) + @") AND
                            Event.StartTime Between '" + startDate + @"' AND '" + endDate + @"'"
                );
                return table.Select().Select(row => new TableOperations<DisturbanceData>(connection).LoadRecord(row));
            }

        }

        private int GetCounts(IGrouping<int, DisturbanceData> assetGroup, List<Curves.Point> curve) {

            return assetGroup.Where(x => {
                double vc = 0.0D;

                int point1 = curve.TakeWhile(y => y.Duration < x.DurationSeconds).Count() - 1;
                if (point1 == -1) return false;
                else if (point1 + 1 == curve.Count()) vc = curve[point1].Magnitude;
                else if (curve[point1].Duration == curve[point1 + 1].Duration) vc = curve[point1 + 1].Duration;
                else
                {
                    double slope = (curve[point1 + 1].Magnitude - curve[point1].Magnitude) / (curve[point1 + 1].Duration - curve[point1].Duration);
                    vc = slope * (x.DurationSeconds - curve[point1].Duration) + curve[point1].Magnitude;
                }
                double value = (1.0D - x.PerUnitMagnitude) / (1.0D - vc);
                return value >= 1.0D;
            }).Count();

        }

        private string GetTHDJsonString(IGrouping<int, DisturbanceData> assetGroup, DateTime startDate, DateTime endDate)
        {
            using(AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                List<int> channelIds = new TableOperations<Channel>(connection).QueryRecordsWhere("MeterID = {0} AND MeasurementCharacteristicID = (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'TotalTHD')", assetGroup.Key).Select(x => x.ID).ToList();
                List<openHistorian.XDALink.TrendingDataPoint> historianPoints = new List<openHistorian.XDALink.TrendingDataPoint>();
                using (Historian historian = new Historian(HistorianServer, HistorianInstance))
                {
                    foreach (openHistorian.XDALink.TrendingDataPoint point in historian.Read(channelIds, startDate, endDate))
                    {
                        if (point.Value < 0.1)
                            point.Value *= 100;
                        if (point.SeriesID.ToString() == "Average" && point.Value >= 0 && point.Value <= 10)
                            historianPoints.Add(point);
                    }
                }

                return "{" + string.Join(",", historianPoints.GroupBy(x => (int)(x.Value / 0.1)).Where(x => x.Key > 0).OrderBy(x => x.Key).Select(x => $"\"{x.Key}\":\"{x.Count()}\"")) + "}";
            }
        }

        public string GetHelpMessage(string command)
        {
            string newString = "";
            if (command == "PQMarkProcessAllData")
                newString = "Creates aggregates for all data.";
            else if (command == "PQMarkProcessEmptyData")
                newString = "Creates aggregates for missing monthly data";
            else
                newString = "Creates aggregates for month to date data";

            StringBuilder helpMessage = new StringBuilder();

            helpMessage.Append(newString);
            helpMessage.AppendLine();
            helpMessage.AppendLine();
            helpMessage.Append("   Usage:");
            helpMessage.AppendLine();
            helpMessage.Append("       " + command);
            helpMessage.AppendLine();
            helpMessage.Append("       " + command +" -?");
            helpMessage.AppendLine();
            helpMessage.AppendLine();
            helpMessage.Append("   Options:");
            helpMessage.AppendLine();
            helpMessage.Append("       -?".PadRight(25));
            helpMessage.Append("Displays this help message");

            return helpMessage.ToString();
        }
    #endregion
    }
}
