//******************************************************************************************************
//  ReportsEngine.cs - Gbtc
//
//  Copyright © 2018, Grid Protection Alliance.  All Rights Reserved.
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
//  06/14/2018 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************

using GSF;
using GSF.Configuration;
using GSF.Scheduling;
using GSF.Web.Model;
using openHistorian.XDALink;
using openXDA.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openXDA.Reports
{
    public class ReportsEngine
    {
        #region [ Members ]

        // Fields
        private ScheduleManager m_scheduler;
        private bool m_running = false;
        private ReportsSettings m_reportsSettings;

        #endregion

        #region [ Constructors ]
        public ReportsEngine()
        {
            m_reportsSettings = new ReportsSettings();
        }

        public ReportsEngine(ReportsSettings settings)
        {
            m_reportsSettings = settings;
        }

        #endregion

        #region [ Properties ]
        private ScheduleManager Scheduler => m_scheduler ?? (m_scheduler = new ScheduleManager());
        public bool Running => m_running;

        [Category]
        [SettingName("Reports")]
        public ReportsSettings ReportsSettings
        {
            get
            {
                return m_reportsSettings;
            }
        }

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

        public void Start()
        {
            if (!Running)
            {
                Scheduler.Initialize();
                Scheduler.Starting += Scheduler_Starting;
                Scheduler.Started += Scheduler_Started;
                Scheduler.ScheduleDue += Scheduler_ScheduleDue;
                Scheduler.Disposed += Scheduler_Disposed;
                Scheduler.AddSchedule("Reports", ReportsSettings.Frequency);
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
            OnLogStatusMessage("Reports Engine has started successfully...");
        }

        private void Scheduler_Starting(object sender, EventArgs e)
        {
        }

        private void Scheduler_Disposed(object sender, EventArgs e)
        {
            OnLogStatusMessage("Reports Engine is disposed...");
        }


        private void Scheduler_ScheduleDue(object sender, EventArgs<Schedule> e)
        {
            OnLogStatusMessage(string.Format("Processing monthly reports..."));
            ProcessMonthlyReports();

        }

        public void ProcessMonthlyReports()
        {
            using (DataContext dataContext = new DataContext("systemSettings"))
            {
                IEnumerable<Meter> meters = dataContext.Table<Meter>().QueryRecords();
                foreach (Meter meter in meters) {
                    DateTime today = DateTime.Now;
                    DateTime firstOfMonth = today.AddDays(1 - today.Day).AddMonths(-1);
                    DateTime endOfMonth = firstOfMonth.AddMonths(1).AddDays(-1);
                    dataContext.Table<Report>().AddNewRecord(new Report()
                    {
                        MeterID = meter.ID,
                        Month = firstOfMonth.Month,
                        Year = firstOfMonth.Year,
                        Results = ""
                    });

                    int reportID = dataContext.Table<Report>().QueryRecordWhere("MeterID = {0} AND Month = {1} AND Year = {2}", meter.ID, firstOfMonth.Month, firstOfMonth.Year).ID;

                    bool frequency = ProcessFrequency(meter, firstOfMonth, endOfMonth, dataContext, reportID);
                    bool voltage = ProcessVoltage(meter, firstOfMonth, endOfMonth, dataContext, reportID);
                    bool flicker = ProcessFlicker(meter, firstOfMonth, endOfMonth, dataContext, reportID);
                    bool imbalance = ProcessImbalance(meter, firstOfMonth, endOfMonth, dataContext, reportID);
                    bool thd = ProcessTHD(meter, firstOfMonth, endOfMonth, dataContext, reportID);
                    bool harmonics = ProcessHarmonics(meter, firstOfMonth, endOfMonth, dataContext, reportID);

                    
                    string result = (frequency && voltage && flicker && imbalance && thd && harmonics ? "Pass" : "Fail");
                    dataContext.Connection.ExecuteNonQuery("UPDATE Report SET Results = {0} WHERE ID = {1}", result, reportID);
                }
            }
        }

        private bool ProcessFrequency(Meter meter, DateTime firstOfMonth, DateTime endOfMonth, DataContext dataContext, int reportID) {
            string historianServer = dataContext.Connection.ExecuteScalar<string>("SELECT Value FROM Setting WHERE Name = 'Historian.Server'") ?? "127.0.0.1";
            string historianInstance = dataContext.Connection.ExecuteScalar<string>("SELECT Value FROM Setting WHERE Name = 'Historian.Instance'") ?? "XDA";
            IEnumerable<Channel> channels = dataContext.Table<Channel>().QueryRecordsWhere("MeterID = {0} AND MeasurementCharacteristicID = (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'Frequency') AND MeasurementTypeID = (SELECT ID FROM MeasurementType WHERE Name = 'Voltage')", meter.ID);

            if (!channels.Any()) return true;

            double nominal = 60.0D;
            double testOneMultiplier = 0.005;
            double testTwoMultiplier = 0.003;

            double lowThreshOne = nominal - (nominal * testOneMultiplier);
            double highThreshOne = nominal - (nominal * testOneMultiplier);
            double lowThreshTwo = nominal - (nominal * testTwoMultiplier);
            double highThreshTwo = nominal - (nominal * testTwoMultiplier);

            using (Historian historian = new Historian(historianServer, historianInstance))
            {
                List<double> max = new List<double>();
                List<double> min = new List<double>();
                List<double> avg = new List<double>();

                foreach (openHistorian.XDALink.TrendingDataPoint point in historian.Read(channels.Select(x => x.ID), firstOfMonth, endOfMonth))
                {
                    if (point.SeriesID == SeriesID.Minimum)
                        min.Add(point.Value);
                    else if (point.SeriesID == SeriesID.Maximum)
                        max.Add(point.Value);
                    else if (point.SeriesID == SeriesID.Average)
                        avg.Add(point.Value);
                }

                double maxValue = max.Max();
                double minValue = min.Min();
                double avgValue = avg.Average();

                bool testOne = avg.Where(x => x < lowThreshOne && x > highThreshOne).Count() / avg.Count() > 0.9995;
                bool testTwo = avg.Where(x => x < lowThreshTwo && x > highThreshTwo).Count() / avg.Count() > 0.99;

                dataContext.Table<ReportResult>().AddNewRecord(new ReportResult() {
                    ReportID = reportID,
                    ReportResultTypeID = dataContext.Connection.ExecuteScalar<int>("SELECT ID FROM ReportResultType WHERE Name = 'Frequency'"),
                    Minimum = minValue,
                    Maximum = maxValue,
                    Average = avgValue,
                    Compliance = (testOne && testTwo ? "Pass": "Fail")
                });
                return testOne && testTwo;

            }

        }

        private bool ProcessVoltage(Meter meter, DateTime firstOfMonth, DateTime endOfMonth, DataContext dataContext, int reportID) {
            string historianServer = dataContext.Connection.ExecuteScalar<string>("SELECT Value FROM Setting WHERE Name = 'Historian.Server'") ?? "127.0.0.1";
            string historianInstance = dataContext.Connection.ExecuteScalar<string>("SELECT Value FROM Setting WHERE Name = 'Historian.Instance'") ?? "XDA";
            IEnumerable<Channel> channels = dataContext.Table<Channel>().QueryRecordsWhere("MeterID = {0} AND MeasurementCharacteristicID = (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'RMS') AND MeasurementTypeID = (SELECT ID FROM MeasurementType WHERE Name = 'Voltage')", meter.ID);

            if (!channels.Any()) return true;

            double nominal = dataContext.Table<Line>().QueryRecordWhere("LineID = {0}", channels.FirstOrDefault().LineID)?.VoltageKV ?? 0;
            double testOneMultiplier = 0.005;
            double testTwoMultiplier = 0.003;

            double lowThreshOne = nominal - (nominal * testOneMultiplier);
            double highThreshOne = nominal - (nominal * testOneMultiplier);
            double lowThreshTwo = nominal - (nominal * testTwoMultiplier);
            double highThreshTwo = nominal - (nominal * testTwoMultiplier);

            using (Historian historian = new Historian(historianServer, historianInstance))
            {
                List<double> max = new List<double>();
                List<double> min = new List<double>();
                List<double> avg = new List<double>();

                foreach (openHistorian.XDALink.TrendingDataPoint point in historian.Read(channels.Select(x => x.ID), firstOfMonth, endOfMonth))
                {
                    if (point.SeriesID == SeriesID.Minimum)
                        min.Add(point.Value);
                    else if (point.SeriesID == SeriesID.Maximum)
                        max.Add(point.Value);
                    else if (point.SeriesID == SeriesID.Average)
                        avg.Add(point.Value);
                }

                double maxValue = max.Max();
                double minValue = min.Min();
                double avgValue = avg.Average();

                bool testOne = avg.Where(x => x < lowThreshOne && x > highThreshOne).Count() / avg.Count() > 0.9995;
                bool testTwo = avg.Where(x => x < lowThreshTwo && x > highThreshTwo).Count() / avg.Count() > 0.99;

                dataContext.Table<ReportResult>().AddNewRecord(new ReportResult()
                {
                    ReportID = reportID,
                    ReportResultTypeID = dataContext.Connection.ExecuteScalar<int>("SELECT ID FROM ReportResultType WHERE Name = 'Voltage'"),
                    Minimum = minValue,
                    Maximum = maxValue,
                    Average = avgValue,
                    Compliance = (testOne && testTwo ? "Pass" : "Fail")
                });
                return testOne && testTwo;

            }
        }

        private bool ProcessFlicker(Meter meter, DateTime firstOfMonth, DateTime endOfMonth, DataContext dataContext, int reportID) {
            string historianServer = dataContext.Connection.ExecuteScalar<string>("SELECT Value FROM Setting WHERE Name = 'Historian.Server'") ?? "127.0.0.1";
            string historianInstance = dataContext.Connection.ExecuteScalar<string>("SELECT Value FROM Setting WHERE Name = 'Historian.Instance'") ?? "XDA";
            IEnumerable<Channel> channels = dataContext.Table<Channel>().QueryRecordsWhere("MeterID = {0} AND MeasurementCharacteristicID = (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'FlkrPST') AND MeasurementTypeID = (SELECT ID FROM MeasurementType WHERE Name = 'Voltage')", meter.ID);

            if (!channels.Any()) return true;

            double nominal = 1;

            using (Historian historian = new Historian(historianServer, historianInstance))
            {
                List<double> max = new List<double>();
                List<double> min = new List<double>();
                List<double> avg = new List<double>();

                foreach (openHistorian.XDALink.TrendingDataPoint point in historian.Read(channels.Select(x => x.ID), firstOfMonth, endOfMonth))
                {
                    if (point.SeriesID == SeriesID.Minimum)
                        min.Add(point.Value);
                    else if (point.SeriesID == SeriesID.Maximum)
                        max.Add(point.Value);
                    else if (point.SeriesID == SeriesID.Average)
                        avg.Add(point.Value);
                }

                double maxValue = max.Max();
                double minValue = min.Min();
                double avgValue = avg.Average();
                bool test = avg.Where(x => x <= nominal).Count() / avg.Count() > 0.9850;

                dataContext.Table<ReportResult>().AddNewRecord(new ReportResult()
                {
                    ReportID = reportID,
                    ReportResultTypeID = dataContext.Connection.ExecuteScalar<int>("SELECT ID FROM ReportResultType WHERE Name = 'Flicker'"),
                    Minimum = minValue,
                    Maximum = maxValue,
                    Average = avgValue,
                    Compliance = (test ? "Pass" : "Fail")
                });
                return test;

            }
        }

        private bool ProcessImbalance(Meter meter, DateTime firstOfMonth, DateTime endOfMonth, DataContext dataContext, int reportID) {
            string historianServer = dataContext.Connection.ExecuteScalar<string>("SELECT Value FROM Setting WHERE Name = 'Historian.Server'") ?? "127.0.0.1";
            string historianInstance = dataContext.Connection.ExecuteScalar<string>("SELECT Value FROM Setting WHERE Name = 'Historian.Instance'") ?? "XDA";
            IEnumerable<Channel> channels = dataContext.Table<Channel>().QueryRecordsWhere("MeterID = {0} AND MeasurementCharacteristicID = (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'AvgImbal') AND MeasurementTypeID = (SELECT ID FROM MeasurementType WHERE Name = 'Voltage')", meter.ID);

            if (!channels.Any()) return true;

            using (Historian historian = new Historian(historianServer, historianInstance))
            {
                List<double> max = new List<double>();
                List<double> min = new List<double>();
                List<double> avg = new List<double>();

                foreach (openHistorian.XDALink.TrendingDataPoint point in historian.Read(channels.Select(x => x.ID), firstOfMonth, endOfMonth))
                {
                    if (point.SeriesID == SeriesID.Minimum)
                        min.Add(point.Value);
                    else if (point.SeriesID == SeriesID.Maximum)
                        max.Add(point.Value);
                    else if (point.SeriesID == SeriesID.Average)
                        avg.Add(point.Value);
                }

                double maxValue = max.Max();
                double minValue = min.Min();
                double avgValue = avg.Average();
                bool test = avg.Where(x => x >= 0 && x <= 2).Count() / avg.Count() > 0.9950;

                dataContext.Table<ReportResult>().AddNewRecord(new ReportResult()
                {
                    ReportID = reportID,
                    ReportResultTypeID = dataContext.Connection.ExecuteScalar<int>("SELECT ID FROM ReportResultType WHERE Name = 'Imbalance'"),
                    Minimum = minValue,
                    Maximum = maxValue,
                    Average = avgValue,
                    Compliance = (test ? "Pass" : "Fail")
                });
                return test;

            }
        }

        private bool ProcessTHD(Meter meter, DateTime firstOfMonth, DateTime endOfMonth, DataContext dataContext, int reportID) {
            string historianServer = dataContext.Connection.ExecuteScalar<string>("SELECT Value FROM Setting WHERE Name = 'Historian.Server'") ?? "127.0.0.1";
            string historianInstance = dataContext.Connection.ExecuteScalar<string>("SELECT Value FROM Setting WHERE Name = 'Historian.Instance'") ?? "XDA";
            IEnumerable<Channel> channels = dataContext.Table<Channel>().QueryRecordsWhere("MeterID = {0} AND MeasurementCharacteristicID = (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'TotalTHD') AND MeasurementTypeID = (SELECT ID FROM MeasurementType WHERE Name = 'Voltage')", meter.ID);

            if (!channels.Any()) return true;

            using (Historian historian = new Historian(historianServer, historianInstance))
            {
                List<double> max = new List<double>();
                List<double> min = new List<double>();
                List<double> avg = new List<double>();

                foreach (openHistorian.XDALink.TrendingDataPoint point in historian.Read(channels.Select(x => x.ID), firstOfMonth, endOfMonth))
                {
                    if (point.SeriesID == SeriesID.Minimum)
                        min.Add(point.Value);
                    else if (point.SeriesID == SeriesID.Maximum)
                        max.Add(point.Value);
                    else if (point.SeriesID == SeriesID.Average)
                        avg.Add(point.Value);
                }

                double maxValue = max.Max();
                double minValue = min.Min();
                double avgValue = avg.Average();
                bool test = avg.Where(x => x <= 8).Count() / avg.Count() > 0.9950;

                dataContext.Table<ReportResult>().AddNewRecord(new ReportResult()
                {
                    ReportID = reportID,
                    ReportResultTypeID = dataContext.Connection.ExecuteScalar<int>("SELECT ID FROM ReportResultType WHERE Name = 'THD'"),
                    Minimum = minValue,
                    Maximum = maxValue,
                    Average = avgValue,
                    Compliance = (test ? "Pass" : "Fail")
                });
                return test;

            }
        }

        private bool ProcessHarmonics(Meter meter, DateTime firstOfMonth, DateTime endOfMonth, DataContext dataContext, int reportID) {
            return true;
        }

        public string GetHelpMessage(string command)
        {
            StringBuilder helpMessage = new StringBuilder();

            helpMessage.Append("Processes the month PQ Reports");
            helpMessage.AppendLine();
            helpMessage.AppendLine();
            helpMessage.Append("   Usage:");
            helpMessage.AppendLine();
            helpMessage.Append("       " + command);
            helpMessage.AppendLine();
            helpMessage.Append("       " + command + " -?");
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
