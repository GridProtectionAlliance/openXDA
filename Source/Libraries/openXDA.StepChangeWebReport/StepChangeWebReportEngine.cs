//******************************************************************************************************
//  StepChangeWebReportEngine.cs - Gbtc
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
//  08/15/2018 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************

using GSF;
using GSF.Configuration;
using GSF.NumericalAnalysis;
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

namespace openXDA.StepChangeWebReport
{
    public class StepChangeWebReportEngine
    {
        #region [ Members ]

        // Fields
        private ScheduleManager m_scheduler;
        private bool m_running = false;
        private StepChangeWebReportSettings m_stepChangeWebReportSettings;

        #endregion

        #region [ Constructors ]
        public StepChangeWebReportEngine()
        {
            m_stepChangeWebReportSettings = new StepChangeWebReportSettings();
        }

        public StepChangeWebReportEngine(StepChangeWebReportSettings settings)
        {
            m_stepChangeWebReportSettings = settings;
        }

        #endregion

        #region [ Properties ]
        private ScheduleManager Scheduler => m_scheduler ?? (m_scheduler = new ScheduleManager());
        public bool Running => m_running;

        [Category]
        [SettingName("StepChangeWebReport")]
        public StepChangeWebReportSettings StepChangeWebReportSettings
        {
            get
            {
                return m_stepChangeWebReportSettings;
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
                Scheduler.AddSchedule("StepChangeWebReport", StepChangeWebReportSettings.Frequency);
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
            if (StepChangeWebReportSettings.Verbose)
                OnLogStatusMessage("StepChangeWebReport Engine has started successfully...");
        }

        private void Scheduler_Starting(object sender, EventArgs e)
        {
        }

        private void Scheduler_Disposed(object sender, EventArgs e)
        {
            if (StepChangeWebReportSettings.Verbose)
                OnLogStatusMessage("StepChangeWebReport Engine is disposed...");
        }


        private void Scheduler_ScheduleDue(object sender, EventArgs<Schedule> e)
        {
            if (StepChangeWebReportSettings.Verbose)
                OnLogStatusMessage(string.Format("Processing Step Change web reports..."));

            ProcessStepChangeWebReport();

        }

        private void ProcessStepChangeWebReport()
        {
            DateTime today = DateTime.Now;
            DateTime startDay = today.Date.AddDays(-1);
            DateTime endDay = startDay.AddDays(1).AddTicks(-1);

            using (DataContext dataContext = new DataContext("systemSettings"))
            {
                string historianServer = dataContext.Connection.ExecuteScalar<string>("SELECT Value FROM Setting WHERE Name = 'Historian.Server'") ?? "127.0.0.1";
                string historianInstance = dataContext.Connection.ExecuteScalar<string>("SELECT Value FROM Setting WHERE Name = 'Historian.Instance'") ?? "XDA";

                IEnumerable<Meter> meters = dataContext.Table<Meter>().QueryRecordsWhere("ID IN (SELECT MeterID FROM MeterDataQualitySummary)");
                IEnumerable<StepChangeMeasurement> stepChangeMeasurements = dataContext.Table<StepChangeMeasurement>().QueryRecords();

                foreach (Meter meter in meters)
                {
                    foreach (StepChangeMeasurement measurement in stepChangeMeasurements)
                    {
                        try
                        {
                            Task.Run(() =>
                            {
                                ProcessMeasurement(meter, measurement, historianServer, historianInstance, startDay, endDay);
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

        private void ProcessMeasurement(Meter meter, StepChangeMeasurement measurement, string historianServer, string historianInstance, DateTime startTime, DateTime endTime)
        {
            using (DataContext dataContext = new DataContext("systemSettings"))
            using (Historian historian = new Historian(historianServer, historianInstance))
            {
                PQMeasurement pqMeasurement = dataContext.Table<PQMeasurement>().QueryRecordWhere("ID = {0}", measurement.PQMeasurementID);
                Channel channel = dataContext.Table<Channel>().QueryRecordWhere("MeasurementTypeID = {0} AND MeasurementCharacteristicID = {1} AND PhaseID = {2} AND HarmonicGroup = {3}", pqMeasurement.MeasurementTypeID, pqMeasurement.MeasurementCharacteristicID, pqMeasurement.PhaseID, pqMeasurement.HarmonicGroup);
                StepChangeStat record = dataContext.Table<StepChangeStat>().QueryRecordWhere("MeterID = {0} AND Date = {1} AND StepChangeMeasurementID = {2}", meter.ID, startTime, measurement.ID);
                Unit unit = dataContext.Table<Unit>().QueryRecordWhere("ID = {0}", pqMeasurement.UnitID);

                if (record == null)
                {
                    record = new StepChangeStat();
                    record.MeterID = meter.ID;
                    record.StepchangeMeasurementID = measurement.ID;
                    record.Date = startTime;
                }

                record.Value = 0;

                if (channel != null)
                {
                    IEnumerable<openHistorian.XDALink.TrendingDataPoint> data = historian.Read(new[] { channel.ID }, startTime.AddHours(-1), endTime).ToList();

                    try
                    {
                        foreach (openHistorian.XDALink.TrendingDataPoint point in data) {
                            if (point.Timestamp >= startTime) {
                                double lastHourAvg = data.Where(x => x.Timestamp >= point.Timestamp.AddHours(-1) && x.Timestamp < point.Timestamp).Select(x => x.Value).Average();

                                // if value is outside 5 sigma, do not check for step change
                                double std = data.StandardDeviation(x => x.Value);
                                if (point.Value > std * 5) continue;

                                if (unit.Name == "Percent") {
                                    if (Math.Abs(point.Value - lastHourAvg) > measurement.Setting)
                                        record.Value++;
                                }
                                else {
                                    if (Math.Abs(point.Value - lastHourAvg)/lastHourAvg > measurement.Setting)
                                        record.Value++;
                                }

                            }
                        }
                        record.Value = data.Where(x => x.SeriesID == SeriesID.Average).Select(x => x.Value).Max();
                    }
                    catch (Exception)
                    {
                        // no associated data for this measurement;  just return without creating the record.
                        return;
                    }
                }

                if(record.Value > 0)
                    dataContext.Table<StepChangeStat>().AddNewRecord(record);
            }
        }
        public string GetHelpMessage(string command)
        {
            StringBuilder helpMessage = new StringBuilder();

            helpMessage.Append("Processes the Step Change Web Reports");
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