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
using System.Net.Sockets;
using System.Text;

namespace PQMark.DataAggregator
{
    public class DataAggregationEngine : IDisposable
    {
        #region [ Members ]

        // Fields
        private bool m_disposed;
        private ScheduleManager m_scheduler;
 
        private class DisturbanceData
        {
            public int MeterID { get; set; }
            public double PerUnitMagnitude { get; set; }
            public DateTime StartTime { get; set; }
            public DateTime EndTime { get; set; }
            public double DurationSeconds { get; set; }
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
            }
        }

        public DataAggregationEngine(PQMarkAggregationSettings settings)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                HistorianServer = connection.ExecuteScalar<string>("SELECT Value FROM Setting WHERE Name = 'Historian.Server'") ?? "127.0.0.1";
                HistorianInstance = connection.ExecuteScalar<string>("SELECT Value FROM Setting WHERE Name = 'Historian.Instance'") ?? "XDA";
                PQMarkAggregationSettings = settings;
            }
        }

        #endregion

        #region [ Properties ]
        private ScheduleManager Scheduler => m_scheduler ?? (m_scheduler = new ScheduleManager());
        public bool Running { get; set; } = false;
        private string HistorianServer { get; set; }
        private string HistorianInstance { get; set; }

        [Category]
        [SettingName(PQMarkAggregationSettings.CategoryName)]
        public PQMarkAggregationSettings PQMarkAggregationSettings { get; set; } = new PQMarkAggregationSettings();

        private Func<bool, string> TempTableQuery = (useDates) => @"
                    SELECT 
	                    Event.*, DATEADD(MINUTE, DATEDIFF(MINUTE, CAST(StartTime as DAte), StartTime), CAST(CAST(StartTime as DAte) as datetime))as Minute 
                    into #Temp 
                    FROM 
	                    Event JOIN
	                    PQMarkCompanyMeter ON Event.MeterID = PQMarkCompanyMeter.MeterID 
					WHERE
						PQMarkCompanyMeter.Enabled = 1" + 
                        (useDates ? @" AND StartTime Between {0} AND {1}" : "" )
                    + @"
                    CREATE CLUSTERED INDEX IX_ADSFADSGASDFEWCDSVDFGWERASDFS ON #Temp(ID)
                    CREATE NONCLUSTERED INDEX IX_ADSFASDGAGDASDFAS ON #Temp(Minute)
        ";

        private const string Query = @"
                    SELECT
	                    evt.ID,
                        evt.MeterID,
	                    evt.PerUnitMagnitude,
	                    evt.DurationSeconds,
	                    evt.StartTime,
	                    evt.EndTime
                    FROM
	                    (
		                    select
			                    DISTINCT DATEADD(MINUTE, DATEDIFF(MINUTE, CAST(StartTime as DAte), StartTime), CAST(CAST(StartTime as DAte) as datetime)) as Minute
		                    from
			                    event
	                    ) as EventMINUTE OUTER APPLY
	                    (
		                    SELECT TOP 1
			                    Event.ID,
                                Event.MeterID,
			                    DurationSeconds,
			                    PerUnitMagnitude,
			                    Event.StartTime,
			                    Event.EndTime
		                    FROM
			                    #Temp as Event JOIN
			                    Disturbance ON Disturbance.EventID = Event.ID AND PhaseID = (SELECT ID FROM Phase WHERE Name = 'Worst') AND Disturbance.EventTypeID != (SELECT ID FROM EventType WHERE Name = 'Transient')
		                    WHERE
			                    Event.Minute = EventMINUTE.Minute
		                    ORDER BY 
		                    Abs(1 - PerUnitMagnitude) DESC
	                    ) as evt

                    DROP TABLE #Temp
        ";
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
                Running = true;
            }
        }

        public void Stop()
        {
            if (Running)
            {
                Scheduler.Stop();
                Running = false;
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
            //OnLogStatusMessage("PQMark Data Aggregator Scheduler is starting...");
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

        public void ProcessAllData(bool onlyEmpty = false)
        {
            using(AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                DataTable table = connection.RetrieveData(TempTableQuery(false) + Query);

                if (table.Rows.Count == 0) return;

                IEnumerable<DisturbanceData> dd = table.Select().Select(row => new TableOperations<DisturbanceData>(connection).LoadRecord(row));

                foreach (var assetGroup in dd.GroupBy(x => x.MeterID))
                {
                    int meterID = assetGroup.Key;
                    if (meterID == 0) continue;
                    OnLogStatusMessage(string.Format("PQMark aggregation: Processing {0}", assetGroup.Key));
                    foreach (var yearGroup in assetGroup.GroupBy(x => x.StartTime.Year))
                    {
                        int year = yearGroup.Key;
                        foreach (var dateGroup in yearGroup.GroupBy(x => x.StartTime.Month))
                        {
                            int month = dateGroup.Key;
                            DateTime startDate = new DateTime(yearGroup.Key, dateGroup.Key, 1);
                            DateTime endDate = startDate.AddMonths(1).AddSeconds(-1);

                            PQMarkAggregate record = new TableOperations<PQMarkAggregate>(connection).QueryRecordWhere("MeterID = {0} AND Year = {1} AND Month = {2}", meterID, year, month);
                            if (onlyEmpty && record != null) continue;

                            ProcessMonthOfData(dateGroup, meterID, year, month, startDate, endDate);
                        }

                    }


                }

                OnLogStatusMessage(string.Format("PQMark data aggregation is complete..."));


            }
        }

        public void ProcessMonthToDateData()
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                DateTime endDate = DateTime.UtcNow;
                DateTime startDate = new DateTime(endDate.Year, endDate.Month, 1);

                DataTable table = connection.RetrieveData(TempTableQuery(true) + Query, startDate, endDate);

                if (table.Rows.Count == 0) return;

                IEnumerable<DisturbanceData> dd = table.Select().Select(row => new TableOperations<DisturbanceData>(connection).LoadRecord(row));

                foreach (var assetGroup in dd.GroupBy(x => x.MeterID))
                    ProcessMonthOfData(assetGroup, assetGroup.Key, startDate.Month, startDate.Year, startDate, endDate);
            }
            OnLogStatusMessage(string.Format("PQMark data aggregation is complete..."));

        }

        private void ProcessMonthOfData(IGrouping<int, DisturbanceData> assetGroup, int meterID, int year, int month, DateTime startDate, DateTime endDate) {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings")) {
                OnLogStatusMessage(string.Format("PQMark aggregation: Processing {0}", assetGroup.Key));

                PQMarkAggregate record = new TableOperations<PQMarkAggregate>(connection).QueryRecordWhere("MeterID = {0} AND Year = {1} AND Month = {2}", meterID, year, month);
                if (record == null) record = new PQMarkAggregate();

                record.MeterID = meterID;
                record.Year = year;
                record.Month = month;
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
                try
                {
                    using (Historian historian = new Historian(HistorianServer, HistorianInstance))
                    {
                        foreach (openHistorian.XDALink.TrendingDataPoint point in historian.Read(channelIds, startDate, endDate))
                        {
                            if (point.Value < 0.1)
                                point.Value *= 100;
                            if (point.SeriesID.ToString() == "Average" && point.Value >= 0 && point.Value <= 10)
                                historianPoints.Add(point);


                        }

                        return "{" + string.Join(",", historianPoints.GroupBy(x => (int)(x.Value / 0.1)).Where(x => x.Key > 0).OrderBy(x => x.Key).Select(x => $"\"{x.Key}\":\"{x.Count()}\"")) + "}";
                    }

                }
                catch(KeyNotFoundException ex)
                {
                    return "";
                }
                catch (SocketException ex)
                {
                    return "";
                }
                catch (Exception ex)
                {
                    throw ex;
                }

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
