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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using GSF;
using GSF.Configuration;
using GSF.Data;
using GSF.Scheduling;
using GSF.Web.Model;
using log4net;
using openXDA.Model;

namespace openXDA.Reports
{
    public class ReportsEngine
    {
        #region [ Members ]
        #endregion

        #region [ Constructors ]

        public ReportsEngine()
        {
            PQReportsSettings = new PQReportsSettings();
            EmailSettings = new EmailSettings();
            BreakerReportsSettings = new BreakerReportsSettings();

            Scheduler = new ScheduleManager();
            Scheduler.Initialize();
            Scheduler.Starting += Scheduler_Starting;
            Scheduler.Started += Scheduler_Started;
            Scheduler.ScheduleDue += Scheduler_ScheduleDue;
            Scheduler.Disposed += Scheduler_Disposed;
        }

        #endregion

        #region [ Properties ]

        private ScheduleManager Scheduler { get; }

        public bool Running { get; private set; }

        [Category]
        [SettingName("PQReports")]
        public PQReportsSettings PQReportsSettings { get; }

        [Category]
        [SettingName("BreakerReports")]
        public BreakerReportsSettings BreakerReportsSettings { get; }

        [Category]
        [SettingName("Email")]
        public EmailSettings EmailSettings { get; }

        #endregion

        #region [ Methods ]

        public void ProcessMonthlyReport(Meter meter, DateTime month)
        {
            DateTime firstOfMonth = month.AddDays(1 - month.Day);
            DateTime endOfMonth = firstOfMonth.AddMonths(1).AddDays(-1);

            using (DataContext dataContext = new DataContext("systemSettings"))
            {
                ProcessMonthlyReport(meter, firstOfMonth, endOfMonth, dataContext);
            }
        }

        public bool Start()
        {
            try
            {
                if (!Running)
                {
                    bool scheduled = false;

                    if (PQReportsSettings.Enabled) {
                        Scheduler.AddSchedule("PQReports", PQReportsSettings.Schedule);
                        scheduled = true;
                    }
                    if (BreakerReportsSettings.Enabled)
                    {
                        Scheduler.AddSchedule("BreakerReports", PQReportsSettings.Schedule);
                        scheduled = true;
                    }

                    if (scheduled)
                    {
                        Scheduler.Start();
                        Running = true;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString(), ex);
                return false;
            }
        }

        public void Stop()
        {
            if (Scheduler.IsRunning)
            {
                bool scheduled1 = false;
                bool scheduled2 = false;

                if (!PQReportsSettings.Enabled)
                    scheduled1 = Scheduler.RemoveSchedule("PQReports");
                if (!BreakerReportsSettings.Enabled)
                    scheduled2 = Scheduler.RemoveSchedule("BreakerReports");

                if (scheduled1 && scheduled2 )
                {
                    Scheduler.Stop();
                    Running = false;
                }
            }
        }

        public void ReloadSystemSettings(string connectionString)
        {
            ConnectionStringParser.ParseConnectionString(connectionString, this);

            Scheduler.AddSchedule("PQReports", PQReportsSettings.Schedule, true);
            Scheduler.AddSchedule("BreakerReports", BreakerReportsSettings.Schedule, true);

            if (PQReportsSettings.Enabled || BreakerReportsSettings.Enabled)
                Start();
            else if (!PQReportsSettings.Enabled || !BreakerReportsSettings.Enabled)
                Stop();

        }

        private void Scheduler_Starting(object sender, EventArgs e)
        {
        }

        private void Scheduler_Started(object sender, EventArgs e)
        {
            Log.Info("Reports Engine has started successfully...");
        }

        private void Scheduler_Disposed(object sender, EventArgs e)
        {
            Log.Info("Reports Engine is disposed...");
        }

        private void Scheduler_ScheduleDue(object sender, EventArgs<Schedule> e)
        {
            Log.Info(string.Format($"Processing {e.Argument.Name}..."));
            if (e.Argument.Name == "PQReports")
                ProcessPQReports();
            else if (e.Argument.Name == "BreakerReports")
                ProcessBreakerReports();
        }

        private void ProcessBreakerReports() {
            DateTime today = DateTime.Now;
            DateTime firstOfMonth = today.AddDays(1 - today.Day).AddMonths(-1);
            DateTime endOfMonth = firstOfMonth.AddMonths(1).AddDays(-1);

            AllBreakersReport report = new AllBreakersReport(firstOfMonth, endOfMonth);
            byte[] pdf = report.createPDF();
            if (pdf == null) return;
            byte[] csv = ExportAllToCSV(report.DataTable, firstOfMonth, endOfMonth);
            if (csv == null) return;

            using (MemoryStream pdfStream = new MemoryStream(pdf))
            using (MemoryStream csvStream = new MemoryStream(csv))
            {
                string fileName = "AllBreakersReport_" + firstOfMonth.ToString("MM_dd_yyyy") + "_" + endOfMonth.ToString("MM_dd_yyyy");
                string pdfContentType = "application/pdf";
                string csvContentType = "text/csv";
                string pdfName = fileName + ".pdf";
                string csvName = fileName + ".csv";

                List<Attachment> attachments = new List<Attachment>() {
                    new Attachment(pdfStream, pdfName, pdfContentType),
                    new Attachment(csvStream, csvName, csvContentType)
                };

                using (DataContext dataConext = new DataContext("systemSettings"))
                {
                    EmailWriter emailWriter = new EmailWriter(dataConext, PQReportsSettings, EmailSettings);
                   
                    emailWriter.SendEmailWithAttachment(BreakerReportsSettings.EmailList.Split(',').ToList(), $"Breaker Report for {fileName}", "", attachments);
                }

            }

        }

        private void ProcessPQReports()
        {
            DateTime today = DateTime.Now;
            DateTime firstOfMonth = today.AddDays(1 - today.Day).AddMonths(-1);
            DateTime endOfMonth = firstOfMonth.AddMonths(1).AddDays(-1);

            using (DataContext dataContext = new DataContext("systemSettings"))
            {
                // TODO: There is no EmailGroupAssetGroup
                IEnumerable<Meter> meters = dataContext.Table<Meter>().QueryRecordsWhere("ID IN (SELECT MeterID FROM MeterAssetGroup WHERE AssetGroupID IN (SELECT AssetGroupID FROM EmailGroupAssetGroup WHERE EmailGroupID = (SELECT ID FROM EmailGroup WHERE Name = 'PQ Report')))");

                foreach (Meter meter in meters)
                    ProcessMonthlyReport(meter, firstOfMonth, endOfMonth, dataContext);

                EmailWriter emailWriter = new EmailWriter(dataContext, PQReportsSettings, EmailSettings);
                emailWriter.Execute(firstOfMonth.Month, firstOfMonth.Year);
            }
        }

        private void ProcessMonthlyReport(Meter meter, DateTime firstOfMonth, DateTime endOfMonth, DataContext dataContext)
        {
            Log.Info($"Starting monthly Report for {meter.Name}...");
            PQReport pQReport = new PQReport(PQReportsSettings, meter, firstOfMonth, endOfMonth, dataContext);
            byte[] pdf = pQReport.createPDF();
            Log.Info($"Completed monthly Report for {meter.Name}");

            try
            {
                Report report = dataContext.Table<Report>().QueryRecordWhere("MeterID = {0} AND Month = {1} AND Year = {2}", meter.ID, firstOfMonth.Month, firstOfMonth.Year);

                if (report != null)
                {
                    report.MeterID = meter.ID;
                    report.Month = firstOfMonth.Month;
                    report.Year = firstOfMonth.Year;
                    report.Results = pQReport.Result;
                    report.PDF = pdf;

                    dataContext.Table<Report>().UpdateRecord(report);
                }
                else
                {
                    dataContext.Table<Report>().AddNewRecord(new Report()
                    {
                        MeterID = meter.ID,
                        Month = firstOfMonth.Month,
                        Year = firstOfMonth.Year,
                        Results = pQReport.Result,
                        PDF = pdf
                    });
                }

                Log.Info($"Loaded monthly Report for {meter.Name}");
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString(), ex);
            }
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

        private DateTime GetPreviousMatch(Schedule schedule, DateTime now)
        {
            bool match = false;

            while (!match)
            {
                now = now.AddMinutes(-1);

                match = schedule.MinutePart.Matches(now);
                match &= schedule.HourPart.Matches(now);
                match &= schedule.DayPart.Matches(now);
                match &= schedule.MonthPart.Matches(now);
                match &= schedule.DaysOfWeekPart.Matches(now);
            }

            return now;
        }

        public void ProcessBreakerReportCommand() {
            ProcessBreakerReports();
        }

        public byte[] ExportAllToCSV(DataTable table, DateTime fromDate, DateTime toDate)
        {
            if (table.Rows.Count == 0) return null;

            using (MemoryStream ms = new MemoryStream())
            using (StreamWriter writer = new StreamWriter(ms))
            {
                // Write the CSV header to the file
                writer.WriteLine(GetCSVHeader(table));

                // Write data to the file
                foreach (DataRow row in table.Rows)
                    writer.WriteLine(ToCSV(table, row));

                return ms.ToArray();
            }
        }

        // Converts the data group row of CSV data.
        private string ToCSV(DataTable table, DataRow row)
        {
            IEnumerable<string> columns = table.Columns.Cast<DataColumn>().Select(x => "\"" + row[x.ColumnName] + "\"");
            return string.Join(",", columns);
        }

        // Converts the data group row of CSV data.
        private string GetCSVHeader(DataTable table)
        {
            IEnumerable<string> headers = table.Columns.Cast<DataColumn>().Select(x => "\"" + x.ColumnName + "\"");
            return string.Join(",", headers);
        }

        #endregion

        #region [ Static ]

        private static readonly ILog Log = LogManager.GetLogger(typeof(ReportsEngine));
        private static readonly ConnectionStringParser<SettingAttribute, CategoryAttribute> ConnectionStringParser = new ConnectionStringParser<SettingAttribute, CategoryAttribute>();

        #endregion
    }
}
