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
using GSF.Threading;
using GSF.Web.Model;
using log4net;
using openHistorian.XDALink;
using openXDA.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
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
        private int m_runningCount;

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
        private int RunningCount => m_runningCount;
        private bool IsProcessing => RunningCount > 0;

        [Category]
        [SettingName(StepChangeWebReportSettings.CategoryName)]
        public StepChangeWebReportSettings StepChangeWebReportSettings
        {
            get
            {
                return m_stepChangeWebReportSettings;
            }
        }
        #endregion

        #region [ Static ]
        // Static Fields
        private static readonly ILog Log = LogManager.GetLogger(typeof(StepChangeWebReportEngine));
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
                Log.Info("StepChangeWebReport Engine has started successfully...");
        }

        private void Scheduler_Starting(object sender, EventArgs e)
        {
        }

        private void Scheduler_Disposed(object sender, EventArgs e)
        {
            if (StepChangeWebReportSettings.Verbose)
                Log.Info("StepChangeWebReport Engine is disposed...");
        }


        private void Scheduler_ScheduleDue(object sender, EventArgs<Schedule> e)
        {
            if (StepChangeWebReportSettings.Verbose)
                Log.Info(string.Format("Processing Step Change web reports..."));

            ProcessStepChangeWebReport();
        }

        public void ProcessStepChangeWebReport(DateTime? date = null, int? meterID = null)
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

                if (meterID == null)
                    meters = dataContext.Table<Meter>().QueryRecordsWhere("ID IN (SELECT MeterID FROM MeterDataQualitySummary)");
                else
                    meters = dataContext.Table<Meter>().QueryRecordsWhere("ID = {0}", meterID);

                IEnumerable<StepChangeMeasurement> stepChangeMeasurements = dataContext.Table<StepChangeMeasurement>().QueryRecords();
                int returnVal = Interlocked.CompareExchange(ref m_runningCount, meters.Count() * stepChangeMeasurements.Count(), 0);

                if (returnVal != 0) {
                    Log.Error("Step Change web report agregation process may already be running. Aborting....");
                    return;
                }

                foreach (Meter meter in meters)
                {
                    foreach (StepChangeMeasurement measurement in stepChangeMeasurements)
                    {
                        try
                        {
                            logicalThreadScheduler.CreateThread().Push(() => {
                                
                                try
                                {
                                    ProcessMeasurement(meter, measurement, historianServer, historianInstance, startDay, endDay);
                                }
                                finally
                                {
                                    Interlocked.Decrement(ref m_runningCount);
                                    if (RunningCount == 0)
                                        Log.Info("Step Change Web Report has completed aggregating data.");
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

        private void ProcessMeasurement(Meter meter, StepChangeMeasurement measurement, string historianServer, string historianInstance, DateTime startTime, DateTime endTime)
        {
            using (DataContext dataContext = new DataContext("systemSettings"))
            using (Historian historian = new Historian(historianServer, historianInstance))
            {
                PQMeasurement pqMeasurement = dataContext.Table<PQMeasurement>().QueryRecordWhere("ID = {0}", measurement.PQMeasurementID);
                Channel channel = dataContext.Table<Channel>().QueryRecordWhere("MeasurementTypeID = {0} AND MeasurementCharacteristicID = {1} AND PhaseID = {2} AND HarmonicGroup = {3} AND MeterID = {4}", pqMeasurement.MeasurementTypeID, pqMeasurement.MeasurementCharacteristicID, pqMeasurement.PhaseID, pqMeasurement.HarmonicGroup, meter.ID);
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
                    IEnumerable<openHistorian.XDALink.TrendingDataPoint> data = historian.Read(new[] { channel.ID }, startTime.AddHours(-1), endTime).Where(x => x.SeriesID == SeriesID.Average).ToList();
                    
                    // no associated data for this measurement;  just return without creating the record.
                    if (!data.Any()) return;

                    try
                    {
                        foreach (openHistorian.XDALink.TrendingDataPoint point in data) {
                            if (point.Timestamp >= startTime) {
                                double lastHourAvg = data.Where(x => x.Timestamp >= point.Timestamp.AddHours(-1) && x.Timestamp < point.Timestamp).Select(x => x.Value).Average();

                                double std = data.StandardDeviation(x => x.Value);
                                double average = data.Average(x => x.Value);

                                // if value is outside 5 sigma, do not check for step change
                                if (point.Value > average + std * 5 || point.Value < average - std * 5) continue;

                                if (unit.Name == "Percent") {
                                    if (Math.Abs(point.Value - lastHourAvg) > measurement.Setting)
                                        record.Value++;
                                }
                                else {
                                    if ((Math.Abs(point.Value - lastHourAvg) * 100/lastHourAvg) > measurement.Setting)
                                        record.Value++;
                                }
                            }
                        }
                    }
                    catch (Exception)
                    {
                        // no associated data for this measurement;  just return without creating the record.
                        return;
                    }
                }

                if(record.Value > 0)
                    dataContext.Table<StepChangeStat>().AddNewOrUpdateRecord(record);
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