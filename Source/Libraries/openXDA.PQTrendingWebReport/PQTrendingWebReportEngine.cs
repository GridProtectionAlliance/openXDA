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
using GSF.Threading;
using GSF.Web.Model;
using log4net;
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
using System.Threading;
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
        private int m_runningCount;
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
        private int RunningCount => m_runningCount;
        private bool IsProcessing => RunningCount > 0;

        [Category]
        [SettingName("PQTrendingWebReport")]
        public PQTrendingWebReportSettings PQTrendingWebReportSettings => m_pqTrendingWebReportSettings;
        #endregion

        #region [ Static ]
        // Static Fields
        private static readonly ILog Log = LogManager.GetLogger(typeof(PQTrendingWebReportEngine));
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

        private void Scheduler_Started(object sender, EventArgs e)
        {
            if (PQTrendingWebReportSettings.Verbose)
                Log.Info("PQTrendingWebReport Engine has started successfully...");
        }

        private void Scheduler_Starting(object sender, EventArgs e){}

        private void Scheduler_Disposed(object sender, EventArgs e)
        {
            if (PQTrendingWebReportSettings.Verbose)
                Log.Info("PQTrendingWebReport Engine is disposed...");
        }


        private void Scheduler_ScheduleDue(object sender, EventArgs<Schedule> e)
        {
            if (PQTrendingWebReportSettings.Verbose)
                Log.Info(string.Format("Processing pq web reports..."));

            if (!IsProcessing)
                ProcessPQWebReport();
            else
            {
                if (PQTrendingWebReportSettings.Verbose)
                    Log.Info("PQ Trending Web Reports is currently processing.");
            }
        }

        public void ProcessPQWebReport(DateTime? date = null, int? meterID = null)
        {
            DateTime processDate = date ?? DateTime.Now.AddDays(-1);
            DateTime startDay = processDate.Date;
            DateTime endDay = startDay.AddDays(1).AddTicks(-1);
            LogicalThreadScheduler logicalThreadScheduler = new LogicalThreadScheduler();
            logicalThreadScheduler.UnhandledException += (sender, args) => Log.Error(args.Argument.Message, args.Argument);

            using (DataContext dataContext = new DataContext("systemSettings"))
            {
                logicalThreadScheduler.MaxThreadCount = dataContext.Connection.ExecuteScalar<int?>("SELECT Value FROM Setting WHERE Name = 'MaxThreadCount'") ?? 1;
                string historianServer = dataContext.Connection.ExecuteScalar<string>("SELECT Value FROM Setting WHERE Name = 'Historian.Server'") ?? "127.0.0.1";
                string historianInstance = dataContext.Connection.ExecuteScalar<string>("SELECT Value FROM Setting WHERE Name = 'Historian.Instance'") ?? "XDA";

                IEnumerable<Meter> meters;
                
                if(meterID == null)
                    meters = dataContext.Table<Meter>().QueryRecordsWhere("ID IN (SELECT MeterID FROM MeterDataQualitySummary)");
                else
                    meters = dataContext.Table<Meter>().QueryRecordsWhere("ID = {0}", meterID);

                IEnumerable<PQMeasurement> pQMeasurements = dataContext.Table<PQMeasurement>().QueryRecords();

                int returnVal = Interlocked.CompareExchange(ref m_runningCount, meters.Count() * pQMeasurements.Count(), 0);

                if (returnVal != 0)
                {
                    Log.Error("PQ Treding Web Report aggregtaion process may already be running. Aborting....");
                    return;
                }

                foreach (Meter meter in meters) {
                    foreach (PQMeasurement measurement in pQMeasurements) {
                        try
                        {
                            logicalThreadScheduler.CreateThread().Push(() => {
                                try
                                {
                                    ProcessMeasurement(meter, measurement, historianServer, historianInstance, startDay, endDay);
                                }
                                finally {
                                    Interlocked.Decrement(ref m_runningCount);
                                    if (RunningCount == 0)
                                        Log.Info("PQ Trend Web Report has completed aggregating data.");
                                }

                            });
                        }
                        catch (Exception ex)
                        {
                            Log.Error(ex.Message, ex);
                        }
                    }
                }
            }
        }

        private void ProcessMeasurement(Meter meter, PQMeasurement measurement, string historianServer, string historianInstance, DateTime startTime, DateTime endTime)
        {
            Channel channel;
            PQTrendStat record;

            using (DataContext dataContext = new DataContext("systemSettings"))
            {
                channel = dataContext.Table<Channel>().QueryRecordWhere("MeasurementTypeID = {0} AND MeasurementCharacteristicID = {1} AND PhaseID = {2} AND HarmonicGroup = {3} AND MeterID = {4}", measurement.MeasurementTypeID, measurement.MeasurementCharacteristicID, measurement.PhaseID, measurement.HarmonicGroup, meter.ID);
                record = dataContext.Table<PQTrendStat>().QueryRecordWhere("MeterID = {0} AND Date = {1} AND PQMeasurementTypeID = {2}", meter.ID, startTime, measurement.ID);
            }


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

            if (channel != null)
            {
                using (Historian historian = new Historian(historianServer, historianInstance))
                {
                    IEnumerable<openHistorian.XDALink.TrendingDataPoint> data = historian.Read(new[] { channel.ID }, startTime, endTime).ToList();
                    if (!data.Any()) return;

                    IEnumerable<openHistorian.XDALink.TrendingDataPoint> avgList = data.Where(x => x.SeriesID == SeriesID.Average).OrderBy(x => x.Value);

                    try
                    {
                        record.Max = data.Where(x => x.SeriesID == SeriesID.Maximum).Select(x => x.Value).Max();
                        record.Min = data.Where(x => x.SeriesID == SeriesID.Minimum).Select(x => x.Value).Min();
                        record.Avg = avgList.Select(x => x.Value).Average();

                    }
                    catch (Exception ex)
                    {
                        if (PQTrendingWebReportSettings.Verbose)
                            Log.Error(ex.Message, ex);
                    }

                    try
                    {
                        int index99 = (int)(avgList.Count() * (0.995));
                        int index01 = (int)(avgList.Count() * (0.005));
                        record.CP99 = avgList.Where((x, i) => i >= index01 && i <= index99).Select(x => x.Value).Max();
                        record.CP01 = avgList.Where((x, i) => i >= index01 && i <= index99).Select(x => x.Value).Min();
                    }
                    catch (Exception ex)
                    {
                        record.CP99 = null;
                        record.CP01 = null;

                        if (PQTrendingWebReportSettings.Verbose)
                            Log.Error(ex.Message, ex);
                    }

                    try
                    {
                        int index95 = (int)(avgList.Count() * (0.975));
                        int index05 = (int)(avgList.Count() * (0.025));
                        record.CP95 = avgList.Where((x, i) => i >= index05 && i <= index95).Select(x => x.Value).Max();
                        record.CP05 = avgList.Where((x, i) => i >= index05 && i <= index95).Select(x => x.Value).Min();
                    }
                    catch (Exception ex)
                    {
                        record.CP95 = null;
                        record.CP05 = null;

                        if (PQTrendingWebReportSettings.Verbose)
                            Log.Error(ex.Message, ex);

                    }

                }

                using (DataContext dataContext = new DataContext("systemSettings"))
                {
                    if(record.Avg != null && record.Max != null && record.Min != null)
                        dataContext.Table<PQTrendStat>().AddNewOrUpdateRecord(record);
                }
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
            helpMessage.Append("       " + command + " -date=MM/DD/YYYY -meter=meterID");
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
