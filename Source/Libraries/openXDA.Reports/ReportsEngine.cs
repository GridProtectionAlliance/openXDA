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
using System.Text;
using GSF;
using GSF.Configuration;
using GSF.Scheduling;
using GSF.Web.Model;
using log4net;
using openXDA.Model;

namespace openXDA.Reports
{
    public class ReportsEngine
    {
        #region [ Members ]

        // Constants
        private const string ScheduleName = "Reports";

        #endregion

        #region [ Constructors ]

        public ReportsEngine()
        {
            ReportsSettings = new ReportsSettings();
            EmailSettings = new EmailSettings();

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
        [SettingName("Reports")]
        public ReportsSettings ReportsSettings { get; }

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
                    Scheduler.AddSchedule(ScheduleName, ReportsSettings.Frequency);
                    Scheduler.Start();
                    Running = true;
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
            if (Running)
            {
                Scheduler.RemoveSchedule(ScheduleName);
                Scheduler.Stop();
                Running = false;
            }
        }

        public void ReloadSystemSettings(string connectionString)
        {
            ConnectionStringParser.ParseConnectionString(connectionString, this);

            if (ReportsSettings.Enabled)
                Start();
            else if (!ReportsSettings.Enabled)
                Stop();

            Scheduler.AddSchedule("Reports", ReportsSettings.Frequency, true);
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
            Log.Info(string.Format("Processing monthly reports..."));
            ProcessMonthlyReports();
        }

        private void ProcessMonthlyReports()
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

                EmailWriter emailWriter = new EmailWriter(dataContext, ReportsSettings, EmailSettings);
                emailWriter.Execute(firstOfMonth.Month, firstOfMonth.Year);
            }
        }

        private void ProcessMonthlyReport(Meter meter, DateTime firstOfMonth, DateTime endOfMonth, DataContext dataContext)
        {
            Log.Info($"Starting monthly Report for {meter.Name}...");
            PQReport pQReport = new PQReport(meter, firstOfMonth, endOfMonth, dataContext);
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

        #endregion

        #region [ Static ]

        private static readonly ILog Log = LogManager.GetLogger(typeof(ReportsEngine));
        private static readonly ConnectionStringParser<SettingAttribute, CategoryAttribute> ConnectionStringParser = new ConnectionStringParser<SettingAttribute, CategoryAttribute>();

        #endregion
    }
}
