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
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openXDA.PQTrendingWebReport
{
    public class PQTrendingWebReportEngine
    {
        #region [ Members ]

        // Fields
        private ScheduleManager m_scheduler;
        private bool m_running = false;
        private PQTrendingWebReportSettings m_pqTrendingWebReportSettings;

        #endregion

        #region [ Constructors ]
        public PQTrendingWebReportEngine()
        {
            m_pqTrendingWebReportSettings = new PQTrendingWebReportSettings();
        }

        public PQTrendingWebReportEngine(PQTrendingWebReportSettings settings)
        {
            m_pqTrendingWebReportSettings = settings;
        }

        #endregion

        #region [ Properties ]
        private ScheduleManager Scheduler => m_scheduler ?? (m_scheduler = new ScheduleManager());
        public bool Running => m_running;

        [Category]
        [SettingName("PQTrendingWebReport")]
        public PQTrendingWebReportSettings PQTrendingWebReportSettings {
            get
            {
                return m_pqTrendingWebReportSettings;
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
            //ProcessPQWebReport();

            if (!Running)
            {
                Scheduler.Initialize();
                Scheduler.Starting += Scheduler_Starting;
                Scheduler.Started += Scheduler_Started;
                Scheduler.ScheduleDue += Scheduler_ScheduleDue;
                Scheduler.Disposed += Scheduler_Disposed;
                Scheduler.AddSchedule("PQTrendingWebReport", PQTrendingWebReportSettings.Frequency);
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
            if(PQTrendingWebReportSettings.Verbose)
                OnLogStatusMessage("PQTrendingWebReport Engine has started successfully...");
        }

        private void Scheduler_Starting(object sender, EventArgs e)
        {
        }

        private void Scheduler_Disposed(object sender, EventArgs e)
        {
            if (PQTrendingWebReportSettings.Verbose)
                OnLogStatusMessage("PQTrendingWebReport Engine is disposed...");
        }


        private void Scheduler_ScheduleDue(object sender, EventArgs<Schedule> e)
        {
            if (PQTrendingWebReportSettings.Verbose)
                OnLogStatusMessage(string.Format("Processing monthly pq web reports..."));

            ProcessPQWebReport();

        }

        private void ProcessPQWebReport()
        {
            DateTime today = DateTime.Now;
            DateTime startToday = today.Date;
            DateTime endToday = today.Date.AddDays(1).AddTicks(-1);

            using (DataContext dataContext = new DataContext("systemSettings"))
            {
                string historianServer = dataContext.Connection.ExecuteScalar<string>("SELECT Value FROM Setting WHERE Name = 'Historian.Server'") ?? "127.0.0.1";
                string historianInstance = dataContext.Connection.ExecuteScalar<string>("SELECT Value FROM Setting WHERE Name = 'Historian.Instance'") ?? "XDA";

                IEnumerable<Meter> meters = dataContext.Table<Meter>().QueryRecordsWhere("ID IN (SELECT MeterID FROM MeterDataQualitySummary)");
                IEnumerable<PQMeasurement> pQMeasurements = dataContext.Table<PQMeasurement>().QueryRecords();

                foreach (Meter meter in meters) {
                    foreach (PQMeasurement measurement in pQMeasurements) {
                        try
                        {
                            Task.Run(() =>
                            {
                                ProcessMeasurement(meter, measurement, historianServer, historianInstance, startToday, endToday);
                            });
                        }
                        catch (Exception ex)
                        {
                            OnLogExceptionMessage(ex);
                        }
                    }
                }
            }
        }

        private void ProcessMeasurement(Meter meter, PQMeasurement measurement, string historianServer, string historianInstance, DateTime startTime, DateTime endTime) {
            using( DataContext dataContext = new DataContext("systemSettings"))
            using( Historian historian = new Historian(historianServer, historianInstance))
            {
                Channel channel = dataContext.Table<Channel>().QueryRecordWhere("MeasurementTypeID = {0} AND MeasurementCharacteristicID = {1} AND PhaseID = {2} AND HarmonicGroup = {3}", measurement.MeasurementTypeID, measurement.MeasurementCharacteristicID, measurement.PhaseID, measurement.HarmonicGroup);
                PQTrendStat record = dataContext.Table<PQTrendStat>().QueryRecordWhere("MeterID = {0} AND Date = {1} AND PQMeasurementTypeID = {2}", meter.ID, startTime, measurement.ID);
                if (record == null)
                {
                    record = new PQTrendStat();
                    record.MeterID = meter.ID;
                    record.PQMeasurementTypeID = measurement.ID;
                    record.Date = startTime;
                }

                record.Max = null;
                record.Min = null;
                record.Avg = null;
                record.CP99 = null;
                record.CP95 = null;
                record.CP05 = null;
                record.CP01 = null;

                if (channel != null) {
                    IEnumerable<openHistorian.XDALink.TrendingDataPoint> data = historian.Read(new[] { channel.ID }, startTime, endTime);
                    IEnumerable<openHistorian.XDALink.TrendingDataPoint> avgList = data.Where(x => x.SeriesID == SeriesID.Average).OrderBy(x => x.Value);

                    try
                    {
                        record.Max = data.Where(x => x.SeriesID == SeriesID.Maximum).Select(x => x.Value).Max();
                    }
                    catch (Exception) {
                        record.Max = null;
                    }

                    try
                    {
                        record.Min = data.Where(x => x.SeriesID == SeriesID.Minimum).Select(x => x.Value).Min();
                    }
                    catch (Exception)
                    {
                        record.Min = null;
                    }

                    try
                    {
                        record.Avg = avgList.Select(x => x.Value).Average();
                    }
                    catch (Exception)
                    {
                        record.Avg = null;
                    }

                    try
                    {
                        int index99 = (int)(avgList.Count() - avgList.Count() * 0.99);
                        int index01 = (int)(avgList.Count() - avgList.Count() * 0.01);
                        record.CP99 = avgList.Where(x => x.ChannelID >= index01 && x.ChannelID <= index99).Select(x => x.Value).Max();
                        record.CP01 = avgList.Where(x => x.ChannelID >= index01 && x.ChannelID <= index99).Select(x => x.Value).Min();
                    }
                    catch (Exception)
                    {
                        record.CP99 = null;
                        record.CP01 = null;
                    }

                    try
                    {
                        int index95 = (int)(avgList.Count() - avgList.Count() * 0.95);
                        int index05 = (int)(avgList.Count() - avgList.Count() * 0.05);
                        record.CP95 = avgList.Where(x => x.ChannelID >= index05 && x.ChannelID <= index95).Select(x => x.Value).Max();
                        record.CP05 = avgList.Where(x => x.ChannelID >= index05 && x.ChannelID <= index95).Select(x => x.Value).Min();
                    }
                    catch (Exception)
                    {
                        record.CP95 = null;
                        record.CP05 = null;
                    }

                }

                dataContext.Table<PQTrendStat>().AddNewRecord(record);
            }
        }
        public string GetHelpMessage(string command)
        {
            StringBuilder helpMessage = new StringBuilder();

            helpMessage.Append("Processes the month PQ Trending Web Reports");
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
