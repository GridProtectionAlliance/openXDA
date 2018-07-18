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
using Root.Reports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;
using ChartSeries = System.Windows.Forms.DataVisualization.Charting.Series;

namespace openXDA.Reports
{
    public class ReportsEngine
    {
        #region [ Members ]

        // Fields
        private ScheduleManager m_scheduler;
        private bool m_running = false;
        private ReportsSettings m_reportsSettings;
        private EmailSettings m_emailSettings;

        #endregion

        #region [ Constructors ]
        public ReportsEngine()
        {
            m_reportsSettings = new ReportsSettings();
            m_emailSettings = new EmailSettings();
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

        [Category]
        [SettingName("Email")]
        public EmailSettings EmailSettings
        {
            get
            {
                return m_emailSettings;
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
            //using (DataContext dataContext = new DataContext("systemSettings"))
            //{
            //    //DateTime today = DateTime.Parse("09/01/2016");
            //    //DateTime firstOfMonth = today.AddDays(1 - today.Day).AddMonths(-1);
            //    //DateTime endOfMonth = firstOfMonth.AddMonths(1).AddDays(-1);

            //    //Meter meter = dataContext.Table<Meter>().QueryRecordWhere("Name = 'Cal.Cal2'");

            //    //createPDF(meter, firstOfMonth, endOfMonth, dataContext);

            //    DateTime today = DateTime.Parse("08/01/2016");
            //    DateTime firstOfMonth = today.AddDays(1 - today.Day).AddMonths(-1);
            //    DateTime endOfMonth = firstOfMonth.AddMonths(1).AddDays(-1);

            //    //Meter meter = dataContext.Table<Meter>().QueryRecordWhere("Name = 'DR_Fortress'");
            //    Meter meter = dataContext.Table<Meter>().QueryRecordWhere("AssetKey = 'Enphase_SI'");
            //    PQReport pQReport = new PQReport(meter, firstOfMonth, endOfMonth, dataContext);

            //    byte[] pdf = pQReport.createPDF();

            //    try
            //    {
            //        Model.Report report = dataContext.Table<Model.Report>().QueryRecordWhere("MeterID = {0} AND Month = {1} AND Year = {2}", meter.ID, firstOfMonth.Month, firstOfMonth.Year);

            //        if (report != null)
            //        {
            //            report.MeterID = meter.ID;
            //            report.Month = firstOfMonth.Month;
            //            report.Year = firstOfMonth.Year;
            //            report.Results = pQReport.Result;
            //            report.PDF = pdf;

            //            dataContext.Table<Model.Report>().UpdateRecord(report);

            //        }
            //        else
            //        {
            //            dataContext.Table<Model.Report>().AddNewRecord(new Model.Report()
            //            {
            //                MeterID = meter.ID,
            //                Month = firstOfMonth.Month,
            //                Year = firstOfMonth.Year,
            //                Results = pQReport.Result,
            //                PDF = pdf
            //            });
            //        }

            //        EmailWriter emailWriter = new EmailWriter(dataContext, ReportsSettings, EmailSettings);
            //        emailWriter.Execute(firstOfMonth.Month, firstOfMonth.Year);

            //    }
            //    catch (Exception ex)
            //    {
            //        OnLogExceptionMessage(ex);
            //    }

            //    OnLogStatusMessage("Completed test pdf.");
            //}

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
            DateTime today = DateTime.Now;
            DateTime firstOfMonth = today.AddDays(1 - today.Day).AddMonths(-1);
            DateTime endOfMonth = firstOfMonth.AddMonths(1).AddDays(-1);

            using (DataContext dataContext = new DataContext("systemSettings"))
            {
                IEnumerable<Meter> meters = dataContext.Table<Meter>().QueryRecordsWhere("ID IN (SELECT MeterID FROM MeterMeterGroup WHERE MeterGroupID IN (SELECT MeterGroupID FROM EmailGroupMeterGroup WHERE EmailGroupID = (SELECT ID FROM EmailGroup WHERE Name = 'PQ Report')))");
                foreach (Meter meter in meters) {
                    PQReport pQReport = new PQReport(meter, firstOfMonth, endOfMonth, dataContext);

                    byte[] pdf = pQReport.createPDF();

                    try
                    {
                        Model.Report report = dataContext.Table<Model.Report>().QueryRecordWhere("MeterID = {0} AND Month = {1} AND Year = {2}", meter.ID, firstOfMonth.Month, firstOfMonth.Year);

                        if (report != null)
                        {
                            report.MeterID = meter.ID;
                            report.Month = firstOfMonth.Month;
                            report.Year = firstOfMonth.Year;
                            report.Results = pQReport.Result;
                            report.PDF = pdf;

                            dataContext.Table<Model.Report>().UpdateRecord(report);

                        }
                        else {
                            dataContext.Table<Model.Report>().AddNewRecord(new Model.Report()
                            {
                                MeterID = meter.ID,
                                Month = firstOfMonth.Month,
                                Year = firstOfMonth.Year,
                                Results = pQReport.Result,
                                PDF = pdf
                            });
                        }
                    }
                    catch (Exception ex) {
                        OnLogExceptionMessage(ex);
                    }

                }

                EmailWriter emailWriter = new EmailWriter(dataContext, ReportsSettings, EmailSettings);
                emailWriter.Execute(firstOfMonth.Month, firstOfMonth.Year);
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

    }
}
