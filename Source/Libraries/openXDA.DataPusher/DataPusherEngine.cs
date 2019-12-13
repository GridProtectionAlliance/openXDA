//******************************************************************************************************
//  DataPusherEngine.cs - Gbtc
//
//  Copyright © 2017, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the MIT License (MIT), the "License"; you may
//  not use this file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://opensource.org/licenses/MIT
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  06/30/2017 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using GSF;
using GSF.Configuration;
using GSF.Data;
using GSF.Data.Model;
using GSF.Scheduling;
using GSF.Security.Model;
using GSF.Web.Model;
using log4net;
using Newtonsoft.Json.Linq;
using openXDA.Model;

namespace openXDA.DataPusher
{
    public class DataPusherEngine : IDisposable
    {
        #region [ Members ]

        // Fields
        private DataContext m_dataContext;
        private bool m_disposed;
        private ScheduleManager m_scheduler;
        private bool m_running = false;
        private DataPusherSettings m_dataPusherSettings;
        #endregion

        #region [ Constructors ]
        public DataPusherEngine()
        {
            using(AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                m_dataPusherSettings = new DataPusherSettings();
                m_dataPusherSettings.Enabled = connection.ExecuteScalar<bool?>("SELECT Value FROM Setting WHERE Name = 'DataPusher.Enabled'") ?? false;
                m_dataPusherSettings.OnlyValidFaults = connection.ExecuteScalar<bool?>("SELECT Value FROM Setting WHERE Name = 'DataPusher.OnlyValidFaults'") ?? true;
                m_dataPusherSettings.TimeWindow = connection.ExecuteScalar<int?>("SELECT Value FROM Setting WHERE Name = 'DataPusher.TimeWindow'") ?? 0;

            }
        }

        public DataPusherEngine(DataPusherSettings settings)
        {
            m_dataPusherSettings = settings;
        }
        #endregion

        #region [ Properties ]
        private ScheduleManager Scheduler => m_scheduler ?? (m_scheduler = new ScheduleManager());
        public bool Running => m_running;

        [Category]
        [SettingName(DataPusherSettings.CategoryName)]
        public DataPusherSettings DataPusherSettings => m_dataPusherSettings;

        #endregion

        // Client-side script functionality
        #region [ Static ]
        private static readonly ILog Log = LogManager.GetLogger(typeof(DataPusherEngine));

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

        public static event EventHandler<EventArgs<string, string,int>> UpdateProgressForMeter;

        private static void OnUpdateProgressForMeter(string client, string meter, int update)
        {
            if(client != string.Empty)
                UpdateProgressForMeter?.Invoke(new object(), new EventArgs<string, string, int>(client, meter, update));
        }

        public static event EventHandler<EventArgs<string, string, int>> UpdateProgressForInstance;

        private static void OnUpdateProgressForInstance(string client, string instance, int update)
        {
            if (client != string.Empty)
                UpdateProgressForInstance?.Invoke(new object(), new EventArgs<string, string, int>(client, instance, update));
        }


        #endregion

        #region [ Methods ]

        #region [ Class Functions ]
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
                    Log.Error(ex.ToString());
                }
            }
        }


        public void Start()
        {
            if (!m_running)
            {
                Scheduler.Initialize();
                Scheduler.Starting += Scheduler_Starting;
                Scheduler.Started += Scheduler_Started;
                Scheduler.ScheduleDue += Scheduler_ScheduleDue;
                Scheduler.Disposed += Scheduler_Disposed;

                using(AdoDataConnection connection = new AdoDataConnection("systemSetting"))
                {
                    IEnumerable<RemoteXDAInstance> instances = new TableOperations<RemoteXDAInstance>(connection).QueryRecords();
                    foreach (RemoteXDAInstance instance in instances)
                    {
                        if (instance.Frequency != string.Empty && instance.Frequency != "*")
                            Scheduler.AddSchedule(instance.Name, instance.Frequency);
                    }

                    Scheduler.Start();
                    m_running = true;

                }
            }
        }

        public void Stop()
        {
            if (m_running)
            {
                Scheduler.Stop();
                m_running = false;
            }
        }

        public void ReloadSystemSettings()
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSetting"))
            {
                IEnumerable<RemoteXDAInstance> instances = new TableOperations<RemoteXDAInstance>(connection).QueryRecords();
                foreach (RemoteXDAInstance instance in instances)
                {
                    Schedule schedule = Scheduler.Schedules.FirstOrDefault(x => x.Name == instance.Name);
                    if (instance.Frequency != string.Empty && schedule != null)
                        schedule.Rule = instance.Frequency;
                    else if (instance.Frequency == string.Empty && schedule != null)
                        Scheduler.RemoveSchedule(schedule.Name);
                    else if (instance.Frequency != string.Empty && schedule == null)
                        Scheduler.AddSchedule(instance.Name, instance.Frequency);
                }
            }
        }

        private void Scheduler_Started(object sender, EventArgs e)
        {
            Log.Info("DataPusher Scheduler has started successfully...");
        }

        private void Scheduler_Starting(object sender, EventArgs e)
        {
            Log.Info("DataPusher Scheduler is starting...");
        }

        private void Scheduler_Disposed(object sender, EventArgs e)
        {
            Log.Info("DataPusher Scheduler is disposed...");
        }

        private void Scheduler_ScheduleDue(object sender, EventArgs<Schedule> e)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSetting"))
            {
                Log.Info(string.Format("DataPusher Scheduler: {0} schedule is due...", e.Argument.Name));
                RemoteXDAInstance instance = new TableOperations<RemoteXDAInstance>(connection).QueryRecordWhere("Name = {0}", e.Argument.Name);
                SyncInstanceFiles(string.Empty, instance, new CancellationToken());
            }
        }

        #endregion

        #region [ Configuration Sync Functions ]
        public void SyncInstanceConfiguration(string clientId, int instanceId, CancellationToken cancellationToken) {
            try
            {
                using (AdoDataConnection connection = new AdoDataConnection("systemSetting"))
                {

                    IEnumerable<MetersToDataPush> meters = new TableOperations<MetersToDataPush>(connection).QueryRecordsWhere("ID IN (SELECT MetersToDataPushID FROM RemoteXDAInstanceMeter WHERE RemoteXDAInstanceID = {0})", instanceId);
                    RemoteXDAInstance instance = new TableOperations<RemoteXDAInstance>(connection).QueryRecordWhere("ID = {0}", instanceId);
                    UserAccount userAccount = new TableOperations<UserAccount>(connection).QueryRecordWhere("ID = {0}", instance.UserAccountID);

                    int progressTotal = meters.Count();
                    int progressCount = 0;
                    OnUpdateProgressForInstance(clientId, instance.Name, (int)(100 * (progressCount) / progressTotal));
                    foreach (MetersToDataPush meter in meters)
                    {
                        if (cancellationToken.IsCancellationRequested)
                            break;

                        SyncMeterConfigurationForInstance(null, instance, meter, userAccount, cancellationToken);
                        OnUpdateProgressForInstance(clientId, instance.Name, (int)(100 * (++progressCount) / progressTotal));

                    }
                }
            }
            catch (Exception ex) {
                Log.Error(ex);
            }

        }

        public void SyncMeterConfigurationForInstance(string clientId, RemoteXDAInstance instance, MetersToDataPush meterToDataPush, UserAccount userAccount, CancellationToken cancellationToken)
        {

            try
            {
                using (AdoDataConnection connection = new AdoDataConnection("systemSetting"))
                {
                    // Get local meter record
                    Meter localMeterRecord = new TableOperations<Meter>(connection).QueryRecordWhere("ID = {0}", meterToDataPush.LocalXDAMeterID);
                    // get local MeterLine table 
                    IEnumerable<MeterLine> localMeterLines = new TableOperations<MeterLine>(connection).QueryRecordsWhere("MeterID = {0}", localMeterRecord.ID);

                    // update progress
                    int progressTotal = localMeterLines.Count() + 2;
                    int progressCount = 0;

                    // Add or Get remote Asset Group
                    AssetGroup remoteAssetGroup = AddOrGetRemoteAssetGroup(instance.Address, userAccount);

                    // Add or Update remote meter location
                    MeterLocation remoteMeterLocation = AddOrGetRemoteMeterLocation(instance.Address, meterToDataPush, localMeterRecord, userAccount);

                    // update progress
                    OnUpdateProgressForMeter(clientId, localMeterRecord.AssetKey, (int)(100 * (progressCount) / progressTotal));

                    // if meter doesnt exist remotely add it
                    Meter remoteMeter = AddOrGetRemoteMeter(instance.Address, meterToDataPush, localMeterRecord, remoteMeterLocation, userAccount);

                    // update progress
                    OnUpdateProgressForMeter(clientId, localMeterRecord.AssetKey, (int)(100 * (++progressCount) / progressTotal));

                    // If does not exist add link for meter to the appropriate asset group
                    AddMeterAssetGroup(instance.Address, remoteAssetGroup.ID, meterToDataPush.RemoteXDAMeterID, userAccount);

                    // update progress
                    OnUpdateProgressForMeter(clientId, localMeterRecord.AssetKey, (int)(100 * (++progressCount) / progressTotal));

                    // if there is a line for the meter ensure that its data has been uploaded remotely
                    foreach (MeterLine localMeterLine in localMeterLines)
                    {
                        if (cancellationToken.IsCancellationRequested)
                            break;

                        // Add or Update line for meter
                        Line remoteLine = AddOrGetLine(instance.Address, localMeterLine, meterToDataPush.Obsfucate, userAccount);

                        // if MeterLine association has not been previously made, make it
                        MeterLine remoteMeterLine = AddMeterLine(instance.Address, meterToDataPush, remoteLine, userAccount, localMeterLine);

                        // Add MeterLocationLine Record for double ended fault location to work
                        //AddMeterLocationLine(instance.Address, userAccount, remoteMeterLocation.ID, remoteLine.ID);

                        // ensure remote and local line impedance matches
                        AddOrUpdateLineImpedances(instance.Address, localMeterLine, remoteLine, userAccount);

                        // add line to meterlocationline table
                        MeterLocationLine remoteMeterLocationLine = AddOrUpdateMeterLocationLines(instance.Address, remoteLine, remoteMeterLocation, userAccount);

                        // ensure remote and local Source Impedance records match for the current meter line location
                        AddOrUpdateSourceImpedance(instance.Address, localMeterLine, remoteMeterLocationLine, userAccount);

                        // Sync Channel and channel dependant data
                        AddOrUpdateChannelsForLine(instance.Address, localMeterLine, remoteMeterLine, userAccount);

                        OnUpdateProgressForMeter(clientId, localMeterRecord.AssetKey, (int)(100 * (++progressCount) / progressTotal));
                    }

                    connection.ExecuteNonQuery("UPDATE MetersToDataPush SET Synced = 1 WHERE ID = {0}", meterToDataPush.ID);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

        }

        private AssetGroup AddOrGetRemoteAssetGroup(string address, UserAccount userAccount)
        {
            AssetGroup remoteAssetGroup = WebAPIHub.GetRecordWhere<AssetGroup>(address, $"Name = '{WebAPIHub.CompanyName}'", userAccount);
            // if the company meter location does not exist, create it
            if (remoteAssetGroup == null)
            {
                remoteAssetGroup = new AssetGroup()
                {
                    Name = WebAPIHub.CompanyName
                };

                remoteAssetGroup.ID = WebAPIHub.CreateRecord<AssetGroup>(address, remoteAssetGroup, userAccount);
            }

            // There is nothing to update so just return group if found
            return remoteAssetGroup;
        }

        private MeterLocation AddOrGetRemoteMeterLocation(string address, MetersToDataPush meterToDataPush, Meter localMeterRecord, UserAccount userAccount)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSetting"))
            {

                MeterLocation remoteMeterLocation = WebAPIHub.GetRecordWhere<MeterLocation>(address, $"AssetKey='{meterToDataPush.RemoteXDAAssetKey}'", userAccount);
                MeterLocation localMeterLocation = new TableOperations<MeterLocation>(connection).QueryRecordWhere("ID = {0}", localMeterRecord.MeterLocationID);

                // if the company meter location does not exist, create it
                if (remoteMeterLocation == null)
                {
                    if (meterToDataPush.Obsfucate)
                    {
                        remoteMeterLocation = new MeterLocation()
                        {
                            AssetKey = meterToDataPush.RemoteXDAAssetKey,
                            Name = meterToDataPush.RemoteXDAAssetKey,
                            Alias = meterToDataPush.RemoteXDAAssetKey,
                            ShortName = "",
                            Description = "",
                            Latitude = 0.0F,
                            Longitude = 0.0F
                        };
                    }
                    else
                    {
                        remoteMeterLocation = new MeterLocation()
                        {
                            AssetKey = meterToDataPush.RemoteXDAAssetKey,
                            Name = localMeterLocation.Name,
                            Alias = localMeterLocation.Alias,
                            ShortName = localMeterLocation.ShortName,
                            Description = localMeterLocation.Description,
                            Latitude = localMeterLocation.Latitude,
                            Longitude = localMeterLocation.Longitude

                        };
                    }
                    remoteMeterLocation.ID = WebAPIHub.CreateRecord<MeterLocation>(address, remoteMeterLocation, userAccount);

                }
                else
                {
                    if (meterToDataPush.Obsfucate)
                    {
                        remoteMeterLocation.AssetKey = meterToDataPush.RemoteXDAAssetKey;
                        remoteMeterLocation.Name = meterToDataPush.RemoteXDAAssetKey;
                        remoteMeterLocation.Alias = meterToDataPush.RemoteXDAAssetKey;
                        remoteMeterLocation.ShortName = "";
                        remoteMeterLocation.Description = "";
                        remoteMeterLocation.Latitude = 0.0F;
                        remoteMeterLocation.Longitude = 0.0F;

                    }
                    else
                    {
                        remoteMeterLocation.AssetKey = meterToDataPush.RemoteXDAAssetKey;
                        remoteMeterLocation.Name = localMeterLocation.Name;
                        remoteMeterLocation.Alias = localMeterLocation.Alias;
                        remoteMeterLocation.ShortName = localMeterLocation.ShortName;
                        remoteMeterLocation.Description = localMeterLocation.Description;
                        remoteMeterLocation.Latitude = localMeterLocation.Latitude;
                        remoteMeterLocation.Longitude = localMeterLocation.Longitude;
                    }

                    WebAPIHub.UpdateRecord<MeterLocation>(address, remoteMeterLocation, userAccount);
                }

                return remoteMeterLocation;
            }
        }

        private Meter AddOrGetRemoteMeter(string address, MetersToDataPush meter, Meter localMeterRecord, MeterLocation remoteMeterLocation, UserAccount userAccount)
        {
            Meter remoteMeter = WebAPIHub.GetRecordWhere<Meter>(address,  $"ID={meter.RemoteXDAMeterID}", userAccount);

            if (remoteMeter == null)
            {
                remoteMeter = WebAPIHub.GetRecordWhere<Meter>(address, $"AssetKey='{meter.RemoteXDAAssetKey}'", userAccount);
                if(remoteMeter != null)
                throw new Exception($"A meter with this Asset Key ({meter.RemoteXDAAssetKey}) already exists.  Please update the Remote Asset Key field in the data pusher.");
            }

            // if meter doesnt exist remotely create the meter record
            if (remoteMeter == null)
            {

                if (meter.Obsfucate)
                {
                    remoteMeter = new Meter()
                    {
                        Alias = meter.RemoteXDAName,
                        ShortName = meter.RemoteXDAName.Take(20).ToString(),
                    };

                }
                else
                {
                    remoteMeter = new Meter()
                    {
                        Alias = localMeterRecord.Alias,
                        ShortName = localMeterRecord.ShortName,
                    };
                }
                remoteMeter.AssetKey = meter.RemoteXDAAssetKey.ToString();
                remoteMeter.Name = meter.RemoteXDAName;
                remoteMeter.MeterLocationID = remoteMeterLocation.ID;
                remoteMeter.Make = localMeterRecord.Make;
                remoteMeter.Model = localMeterRecord.Model;
                remoteMeter.Description = localMeterRecord.Description;
                remoteMeter.TimeZone = localMeterRecord.TimeZone;

                remoteMeter.ID = meter.RemoteXDAMeterID = WebAPIHub.CreateRecord<Meter>(address, remoteMeter, userAccount);
            }
            else
            {
                if (meter.Obsfucate)
                {
                    remoteMeter.Alias = meter.RemoteXDAName;
                    remoteMeter.ShortName = meter.RemoteXDAName.Take(20).ToString();
                }
                else
                {
                    remoteMeter.Alias = localMeterRecord.Alias;
                    remoteMeter.ShortName = localMeterRecord.ShortName;
                }

                remoteMeter.AssetKey = meter.RemoteXDAAssetKey.ToString();
                remoteMeter.Name = meter.RemoteXDAName;
                remoteMeter.MeterLocationID = remoteMeterLocation.ID;
                remoteMeter.Make = localMeterRecord.Make;
                remoteMeter.Model = localMeterRecord.Model;
                remoteMeter.Description = localMeterRecord.Description;
                remoteMeter.TimeZone = localMeterRecord.TimeZone;

                WebAPIHub.UpdateRecord<Meter>(address, remoteMeter, userAccount);
                meter.RemoteXDAMeterID = remoteMeter.ID;

            }

            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                new TableOperations<MetersToDataPush>(connection).UpdateRecord(meter);
            }


            return remoteMeter;
        }

        private void AddMeterAssetGroup(string address, int assetGroupId, int meterId, UserAccount userAccount)
        {
            MeterAssetGroup remoteMeterAssetGroup = WebAPIHub.GetRecordWhere<MeterAssetGroup>(address, $"MeterID = {meterId} AND AssetGroupID = {assetGroupId}", userAccount);

            // if MeterLine association has not been previously made, make it
            if (remoteMeterAssetGroup == null)
            {
                remoteMeterAssetGroup = new MeterAssetGroup()
                {
                    MeterID = meterId,
                    AssetGroupID = assetGroupId
                };

                WebAPIHub.CreateRecord<MeterAssetGroup>(address, remoteMeterAssetGroup, userAccount);
            }
        }

        private Line AddOrGetLine(string address, MeterLine meterLine, bool obsfucate, UserAccount userAccount)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {


                Line localLine = new TableOperations<Line>(connection).QueryRecordWhere("ID = {0}", meterLine.LineID);
                LinesToDataPush lineToDataPush = new TableOperations<LinesToDataPush>(connection).QueryRecordWhere("LocalXDALineID = {0}", localLine.ID);
                Line remoteLine = null;

                //if the line does not exist in the PQMarkPusher Database to allow for obsfucation add it.
                if (lineToDataPush == null)
                {
                    remoteLine = WebAPIHub.GetRecordWhere<Line>(address, $"AssetKey='{localLine.AssetKey}'", userAccount);

                    if (remoteLine == null)
                    {
                        remoteLine = new Line()
                        {
                            AssetKey = (obsfucate ? remoteLine.AssetKey : localLine.AssetKey),
                            VoltageKV = localLine.VoltageKV,
                            ThermalRating = localLine.ThermalRating,
                            Length = localLine.Length,
                            Description = (obsfucate ? "" : localLine.Description)
                        };
                        remoteLine.ID = WebAPIHub.CreateRecord<Line>(address, remoteLine, userAccount);
                        lineToDataPush = new LinesToDataPush()
                        {
                            LocalXDALineID = localLine.ID,
                            RemoteXDALineID = remoteLine.ID,
                            LocalXDAAssetKey = localLine.AssetKey,
                            RemoteXDAAssetKey = (obsfucate ? Guid.NewGuid().ToString() : localLine.AssetKey),
                            RemoteLineCreatedByDataPusher = true

                        };

                    }
                    else
                    {
                        lineToDataPush = new LinesToDataPush()
                        {
                            LocalXDALineID = localLine.ID,
                            RemoteXDALineID = remoteLine.ID,
                            LocalXDAAssetKey = localLine.AssetKey,
                            RemoteXDAAssetKey = (obsfucate ? Guid.NewGuid().ToString() : localLine.AssetKey),
                            RemoteLineCreatedByDataPusher = false
                        };
                    }




                    new TableOperations<LinesToDataPush>(connection).AddNewRecord(lineToDataPush);
                }
                else
                {
                    remoteLine = WebAPIHub.GetRecordWhere<Line>(address, $"ID={lineToDataPush.RemoteXDALineID}", userAccount);

                    if (remoteLine == null)
                    {
                        remoteLine = new Line()
                        {
                            AssetKey = (obsfucate ? remoteLine.AssetKey : localLine.AssetKey),
                            VoltageKV = localLine.VoltageKV,
                            ThermalRating = localLine.ThermalRating,
                            Length = localLine.Length,
                            Description = (obsfucate ? "" : localLine.Description),
                        };
                        remoteLine.ID = WebAPIHub.CreateRecord<Line>(address, remoteLine, userAccount);

                        lineToDataPush.RemoteXDALineID = remoteLine.ID;
                        lineToDataPush.RemoteXDAAssetKey = remoteLine.AssetKey;
                        lineToDataPush.RemoteLineCreatedByDataPusher = true;

                        new TableOperations<LinesToDataPush>(connection).UpdateRecord(lineToDataPush);
                    }
                    else if (lineToDataPush.RemoteLineCreatedByDataPusher)
                    {
                        remoteLine.AssetKey = (obsfucate ? lineToDataPush.RemoteXDAAssetKey.ToString() : localLine.AssetKey);
                        remoteLine.VoltageKV = localLine.VoltageKV;
                        remoteLine.ThermalRating = localLine.ThermalRating;
                        remoteLine.Length = localLine.Length;
                        remoteLine.Description = (obsfucate ? "" : localLine.Description);
                        WebAPIHub.UpdateRecord<Line>(address, remoteLine, userAccount);
                    }
                }
                if (remoteLine == null) throw new Exception("Line not defined.");
                return remoteLine;
            }
        }

        private void AddMeterLocationLine(string address, UserAccount userAccount, int meterLocationID, int lineID) {
            MeterLocationLine remoteMeterLocationLine = WebAPIHub.GetRecordWhere<MeterLocationLine>(address, $"MeterLocationID = {meterLocationID} AND LineID = {lineID}", userAccount);

            // if MeterLineLine association has not been previously made, make it
            if (remoteMeterLocationLine == null)
                WebAPIHub.CreateRecord<MeterLocationLine>(address, new MeterLocationLine() { MeterLocationID = meterLocationID, LineID = lineID }, userAccount);
            
        }

        private MeterLine AddMeterLine(string address, MetersToDataPush meter, Line remoteLine, UserAccount userAccount, MeterLine localMeterLine)
        {
            MeterLine remoteMeterLine = WebAPIHub.GetRecordWhere<MeterLine>(address, $"MeterID = {meter.RemoteXDAMeterID} AND LineID = {remoteLine.ID}", userAccount);

            // if MeterLine association has not been previously made, make it
            if (remoteMeterLine == null)
            {
                remoteMeterLine = new MeterLine()
                {
                    MeterID = meter.RemoteXDAMeterID,
                    LineID = remoteLine.ID,
                    LineName = (meter.Obsfucate ? remoteLine.AssetKey : localMeterLine.LineName)
                };

                remoteMeterLine.ID = WebAPIHub.CreateRecord<MeterLine>(address, remoteMeterLine, userAccount);
            }
            else
            {
                remoteMeterLine.LineName = (meter.Obsfucate ? remoteLine.AssetKey : localMeterLine.LineName);
                WebAPIHub.UpdateRecord<MeterLine>(address, remoteMeterLine, userAccount);
            }

            AddFaultDetectionLogic(address, localMeterLine, remoteMeterLine, userAccount);

            return remoteMeterLine;
        }

        private void AddFaultDetectionLogic(string address, MeterLine localMeterLine,MeterLine remoteMeterLine, UserAccount userAccount)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                FaultDetectionLogic localFaultDetectionLogic = new TableOperations<FaultDetectionLogic>(connection).QueryRecordWhere("MeterLineID = {0}", localMeterLine.ID);
                if (localFaultDetectionLogic == null) return;

                FaultDetectionLogic remoteFaultDetectionLogic = WebAPIHub.GetRecordWhere<FaultDetectionLogic>(address, $"MeterLineID = {remoteMeterLine.ID}", userAccount);


                if (remoteFaultDetectionLogic == null)
                {
                    remoteFaultDetectionLogic = new FaultDetectionLogic()
                    {
                        MeterLineID = remoteMeterLine.ID,
                        Expression = localFaultDetectionLogic.Expression
                    };
                    WebAPIHub.CreateRecord<FaultDetectionLogic>(address, remoteFaultDetectionLogic, userAccount);
                }
                else
                {
                    remoteFaultDetectionLogic.Expression = localFaultDetectionLogic.Expression;
                    WebAPIHub.UpdateRecord<FaultDetectionLogic>(address, remoteFaultDetectionLogic, userAccount);
                }
            }
        }

        private void AddOrUpdateLineImpedances(string address, MeterLine localMeterLine, Line remoteLine, UserAccount userAccount)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                // ensure remote and local line impedance matches
                LineImpedance localLineImpedance = new TableOperations<LineImpedance>(connection).QueryRecordWhere("LineID = {0}", localMeterLine.LineID);
                if (localLineImpedance == null) return;

                LineImpedance remoteLineImpedance = WebAPIHub.GetRecordWhere<LineImpedance>(address, $"LineID = {remoteLine.ID}", userAccount);

                // if there is a local record but not a remote record
                if (remoteLineImpedance == null)
                {
                    remoteLineImpedance = new LineImpedance();
                    remoteLineImpedance.LineID = remoteLine.ID;
                    remoteLineImpedance.R0 = localLineImpedance.R0;
                    remoteLineImpedance.R1 = localLineImpedance.R1;
                    remoteLineImpedance.X0 = localLineImpedance.X0;
                    remoteLineImpedance.X1 = localLineImpedance.X1;

                    WebAPIHub.CreateRecord<LineImpedance>(address, remoteLineImpedance, userAccount);
                }
                else
                {
                    remoteLineImpedance.R0 = localLineImpedance.R0;
                    remoteLineImpedance.R1 = localLineImpedance.R1;
                    remoteLineImpedance.X0 = localLineImpedance.X0;
                    remoteLineImpedance.X1 = localLineImpedance.X1;
                    WebAPIHub.UpdateRecord<LineImpedance>(address, remoteLineImpedance, userAccount);
                }
            }
        }

        private MeterLocationLine AddOrUpdateMeterLocationLines(string address, Line remoteLine, MeterLocation remoteMeterLocation, UserAccount userAccount)
        {
            // add line to meterlocationline table
            MeterLocationLine remoteMeterLocationLine = WebAPIHub.GetRecordWhere<MeterLocationLine>(address, $"MeterLocationID = {remoteMeterLocation.ID} AND LineID = {remoteLine.ID}", userAccount);
            if (remoteMeterLocationLine == null)
            {
                remoteMeterLocationLine = new MeterLocationLine();
                remoteMeterLocationLine.LineID = remoteLine.ID;
                remoteMeterLocationLine.MeterLocationID = remoteMeterLocation.ID;

                WebAPIHub.CreateRecord(address, remoteMeterLocationLine, userAccount);
            }

            return remoteMeterLocationLine;
        }

        private void AddOrUpdateSourceImpedance(string address, MeterLine localMeterLine, MeterLocationLine remoteMeterLocationLine, UserAccount userAccount)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                // ensure remote and local line impedance matches
                SourceImpedance localSourceImpedance = new TableOperations<SourceImpedance>(connection).QueryRecordWhere("MeterLocationLineID  = (SELECT ID FROM MeterLocationLine WHERE MeterLocationID = ( SELECT MeterLocationID FROM Meter WHERE ID = {0}) AND LineID = {1})", localMeterLine.MeterID, localMeterLine.LineID);
                if (localSourceImpedance == null) return;

                SourceImpedance remoteSourceImpedance = WebAPIHub.GetRecordWhere<SourceImpedance>(address, $"MeterLocationLineID = {remoteMeterLocationLine.ID}", userAccount);

                if (remoteSourceImpedance == null)
                {
                    remoteSourceImpedance = new SourceImpedance();
                    remoteSourceImpedance.MeterLocationLineID = localSourceImpedance.MeterLocationLineID;
                    remoteSourceImpedance.RSrc = localSourceImpedance.RSrc;
                    remoteSourceImpedance.XSrc = localSourceImpedance.XSrc;
                    WebAPIHub.CreateRecord(address, remoteSourceImpedance, userAccount);
                }
                else
                {
                    remoteSourceImpedance.RSrc = localSourceImpedance.RSrc;
                    remoteSourceImpedance.XSrc = localSourceImpedance.XSrc;
                    WebAPIHub.UpdateRecord(address, remoteSourceImpedance, userAccount);
                }
            }

        }

        private void AddOrUpdateChannelsForLine(string address, MeterLine localMeterLine, MeterLine remoteMeterLine, UserAccount userAccount)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                // ensure remote and local line impedance matches
                IEnumerable<ChannelDetail> localChannels = new TableOperations<ChannelDetail>(connection).QueryRecordsWhere("MeterID = {0} AND LineID = {1}", localMeterLine.MeterID, localMeterLine.LineID);
                List<ChannelDetail> remoteChannels = WebAPIHub.GetChannels(address, $"MeterID = {remoteMeterLine.MeterID} AND LineID = {remoteMeterLine.LineID}", userAccount).ToList();

                // if there is a local record but not a remote record
                foreach (ChannelDetail localChannelDetail in localChannels)
                {
                    ChannelDetail remoteChannelDetail = remoteChannels.Find(x => x.MeasurementType == localChannelDetail.MeasurementType && x.MeasurementCharacteristic == localChannelDetail.MeasurementCharacteristic && x.Phase == localChannelDetail.Phase && x.Name == localChannelDetail.Name);
                    if (remoteChannelDetail == null)
                    {
                        remoteChannelDetail = new ChannelDetail();
                        remoteChannelDetail.MeterID = remoteMeterLine.MeterID;
                        remoteChannelDetail.LineID = remoteMeterLine.LineID;
                        remoteChannelDetail.MeasurementType = localChannelDetail.MeasurementType;
                        remoteChannelDetail.MeasurementCharacteristic = localChannelDetail.MeasurementCharacteristic;
                        remoteChannelDetail.Phase = localChannelDetail.Phase;
                        remoteChannelDetail.Name = localChannelDetail.Name;
                        remoteChannelDetail.SamplesPerHour = localChannelDetail.SamplesPerHour;
                        remoteChannelDetail.PerUnitValue = localChannelDetail.PerUnitValue;
                        remoteChannelDetail.HarmonicGroup = localChannelDetail.HarmonicGroup;
                        remoteChannelDetail.Description = localChannelDetail.Description;
                        remoteChannelDetail.Enabled = localChannelDetail.Enabled;

                        remoteChannelDetail.ID = WebAPIHub.CreateChannel(address, JObject.FromObject(remoteChannelDetail), userAccount);

                    }
                    else
                    {
                        remoteChannelDetail.MeasurementType = localChannelDetail.MeasurementType;
                        remoteChannelDetail.MeasurementCharacteristic = localChannelDetail.MeasurementCharacteristic;
                        remoteChannelDetail.Phase = localChannelDetail.Phase;
                        remoteChannelDetail.Name = localChannelDetail.Name;
                        remoteChannelDetail.SamplesPerHour = localChannelDetail.SamplesPerHour;
                        remoteChannelDetail.PerUnitValue = localChannelDetail.PerUnitValue;
                        remoteChannelDetail.HarmonicGroup = localChannelDetail.HarmonicGroup;
                        remoteChannelDetail.Description = localChannelDetail.Description;
                        remoteChannelDetail.Enabled = localChannelDetail.Enabled;

                        WebAPIHub.UpdateChannel(address, JObject.FromObject(remoteChannelDetail), userAccount);
                    }

                    AddOrGetSeries(address, localChannelDetail, remoteChannelDetail, userAccount);

                }
            }
        }

        private void AddOrGetSeries(string address, ChannelDetail localChannel, ChannelDetail remoteChannel, UserAccount userAccount)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                IEnumerable<Series> local = new TableOperations<Series>(connection).QueryRecordsWhere("ChannelID = {0}", localChannel.ID);
                List<Series> remote = WebAPIHub.GetRecordsWhere<Series>(address, $"ChannelID = {remoteChannel.ID}", userAccount).ToList();

                // if there is a local record but not a remote record
                foreach (Series localSeries in local)
                {
                    Series remoteSeries = remote.Find(x => x.SeriesTypeID == localSeries.SeriesTypeID && x.SourceIndexes == localSeries.SourceIndexes);
                    if (remoteSeries == null)
                    {
                        remoteSeries = new Series()
                        {
                            ChannelID = remoteChannel.ID,
                            SeriesTypeID = localSeries.SeriesTypeID,
                            SourceIndexes = localSeries.SourceIndexes
                        };

                        remoteSeries.ID = WebAPIHub.CreateRecord(address, remoteSeries, userAccount);

                    }
                    else
                    {
                        remoteSeries.SeriesTypeID = localSeries.SeriesTypeID;
                        remoteSeries.SourceIndexes = localSeries.SourceIndexes;
                        WebAPIHub.UpdateRecord(address, remoteSeries, userAccount);
                    }
                }
            }

        }

        #endregion

        #region [ File Sync Functions]
        public void SyncInstanceFiles(string clientId, RemoteXDAInstance instance, CancellationToken cancellationToken)
        {
            try
            {
                using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
                {

                    IEnumerable<int> meters = new TableOperations<MetersToDataPush>(connection).QueryRecordsWhere("ID IN (SELECT MetersToDataPushID FROM RemoteXDAInstanceMeter WHERE RemoteXDAInstanceID = {0})", instance.ID).Select(x => x.ID);
                    int progressTotal = meters.Count();
                    int progressCount = 0;
                    OnUpdateProgressForInstance(clientId, instance.Name, (int)(100 * (progressCount) / progressTotal));
                    foreach (int meter in meters)
                    {
                        if (cancellationToken.IsCancellationRequested) break;
                        SyncMeterFilesForInstance(clientId, instance, meter, cancellationToken);
                        OnUpdateProgressForInstance(clientId, instance.Name, (int)(100 * (++progressCount) / progressTotal));
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        public void SyncMeterFilesForInstance(string clientId, RemoteXDAInstance instance, int meterId, CancellationToken cancellationToken)
        {
                try
                {
                using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
                {

                    IEnumerable<FileGroup> localFileGroups;
                    UserAccount userAccount = new TableOperations<UserAccount>(connection).QueryRecordWhere("ID = {0}", instance.UserAccountID);

                    DateTime timeWindowStartDate = DateTime.UtcNow.AddHours(DataPusherSettings.TimeWindow * -1);

                    MetersToDataPush meterToDataPush = new TableOperations<MetersToDataPush>(connection).QueryRecordWhere("ID = {0}", meterId);

                    if (DataPusherSettings.TimeWindow != 0)
                        localFileGroups = new TableOperations<FileGroup>(connection).QueryRecordsWhere("ID IN (SELECT FileGroupID From Event WHERE MeterID = {0} AND StartTime >= {1})", meterToDataPush.LocalXDAMeterID, timeWindowStartDate);
                    else
                        localFileGroups = new TableOperations<FileGroup>(connection).QueryRecordsWhere("ID IN (SELECT FileGroupID From Event WHERE MeterID = {0})", meterToDataPush.LocalXDAMeterID);

                    int progressTotal = (localFileGroups.Count() > 0 ? localFileGroups.Count() : 1);
                    int progressCount = 0;

                    OnUpdateProgressForMeter(clientId, meterToDataPush.LocalXDAAssetKey, (int)(100 * (progressCount) / progressTotal));
                    Log.Info($"Processing Remote data push for {meterToDataPush.LocalXDAAssetKey}...");

                    foreach (FileGroup fileGroup in localFileGroups)
                    {
                        if (cancellationToken.IsCancellationRequested) break;

                        OnLogStatusMessage($"Processing Remote data push for {meterToDataPush.LocalXDAAssetKey}:Filegroup:{fileGroup.ID}...");

                        FileGroupLocalToRemote fileGroupLocalToRemote = new TableOperations<FileGroupLocalToRemote>(connection).QueryRecordWhere("LocalFileGroupID = {0} AND RemoteXDAInstanceID = {1}", fileGroup.ID, instance.ID);

                        if (fileGroupLocalToRemote == null)
                        {
                            FileGroup fg = new FileGroup()
                            {
                                ProcessingEndTime = fileGroup.ProcessingEndTime,
                                ProcessingStartTime = fileGroup.ProcessingStartTime,
                                DataEndTime = fileGroup.DataEndTime,
                                DataStartTime = fileGroup.DataStartTime,
                                Error = fileGroup.Error,
                                FileHash = fileGroup.FileHash
                            };
                            int remoteFileGroupId = WebAPIHub.CreateRecord(instance.Address, fg, userAccount);
                            fileGroupLocalToRemote = new FileGroupLocalToRemote()
                            {
                                RemoteXDAInstanceID = instance.ID,
                                LocalFileGroupID = fileGroup.ID,
                                RemoteFileGroupID = remoteFileGroupId
                            };
                            new TableOperations<FileGroupLocalToRemote>(connection).AddNewRecord(fileGroupLocalToRemote);
                        }

                        IEnumerable<DataFile> localDataFiles = new TableOperations<DataFile>(connection).QueryRecordsWhere("FileGroupID = {0}", fileGroupLocalToRemote.LocalFileGroupID);
                        IEnumerable<DataFile> remoteDataFiles = WebAPIHub.GetRecordsWhere<DataFile>(instance.Address, $"FileGroupID = {fileGroupLocalToRemote.RemoteFileGroupID}", userAccount);

                        bool process = false;
                        foreach (DataFile localDataFile in localDataFiles)
                        {
                            int remoteDataFileId;
                            if (!remoteDataFiles.Where(x => x.FilePath == localDataFile.FilePath).Any())
                            {
                                DataFile df = new DataFile()
                                {
                                    CreationTime = localDataFile.CreationTime,
                                    FileGroupID = fileGroupLocalToRemote.RemoteFileGroupID,
                                    FilePath = localDataFile.FilePath,
                                    FilePathHash = localDataFile.FilePathHash,
                                    FileSize = localDataFile.FileSize,
                                    LastAccessTime = localDataFile.LastAccessTime,
                                    LastWriteTime = localDataFile.LastWriteTime
                                };
                                remoteDataFileId = WebAPIHub.CreateRecord(instance.Address, df, userAccount);
                                process = true;
                            }
                            else
                                remoteDataFileId = remoteDataFiles.Where(x => x.FilePath == localDataFile.FilePath).First().ID;

                            FileBlob remoteFileBlob = WebAPIHub.GetRecordWhere<FileBlob>(instance.Address, $"DataFileID = {remoteDataFileId}", userAccount);

                            if (remoteFileBlob == null)
                            {
                                FileBlob localFileBlob = new TableOperations<FileBlob>(connection).QueryRecordWhere("DataFileID = {0}", localDataFile.ID);

                                try
                                {
                                    if (localFileBlob == null)
                                    {
                                        localFileBlob = new FileBlob() { DataFileID = localDataFile.ID, Blob = File.ReadAllBytes(localDataFile.FilePath) };
                                        new TableOperations<FileBlob>(connection).AddNewRecord(localFileBlob);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Log.Error(ex);
                                    process = false;
                                }
                                localFileBlob.DataFileID = remoteDataFileId;
                                WebAPIHub.CreateRecord(instance.Address, new FileBlob() { DataFileID = remoteDataFileId, Blob = localFileBlob.Blob }, userAccount);

                            }
                        }

                        if (process)
                        {
                            Dictionary<string, int> dictionary = new Dictionary<string, int>();
                            dictionary.Add("FileGroupID", fileGroupLocalToRemote.RemoteFileGroupID);
                            dictionary.Add("MeterID", meterToDataPush.RemoteXDAMeterID);
                            WebAPIHub.ProcessFileGroup(instance.Address, JObject.FromObject(dictionary), userAccount);

                        }

                        OnUpdateProgressForMeter(clientId, meterToDataPush.LocalXDAAssetKey, (int)(100 * (++progressCount) / progressTotal));
                        Log.Info($"Processing Remote data push for {meterToDataPush.LocalXDAAssetKey}: Completed Filegroup{fileGroup.ID}: Progress: { (int)(100 * (progressCount) / progressTotal)}");

                    }
                }

                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                }
        }

        public void SyncMeterFileForInstance(RemoteXDAInstance instance, MetersToDataPush meterToDataPush, int fileGroupId, CancellationToken cancellationToken)
        {
            try
            {
                if (cancellationToken.IsCancellationRequested) return;
                using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
                {

                    UserAccount userAccount = new TableOperations<UserAccount>(connection).QueryRecordWhere("ID = {0}", instance.UserAccountID);

                    FileGroupLocalToRemote fileGroupLocalToRemote = new TableOperations<FileGroupLocalToRemote>(connection).QueryRecordWhere("LocalFileGroupID = {0} AND RemoteXDAInstanceID = {1}", fileGroupId, instance.ID);
                    FileGroup fileGroup = new TableOperations<FileGroup>(connection).QueryRecordWhere("ID = {0}", fileGroupId);

                    if (fileGroupLocalToRemote == null)
                    {
                        FileGroup fg = new FileGroup()
                        {
                            ProcessingEndTime = fileGroup.ProcessingEndTime,
                            ProcessingStartTime = fileGroup.ProcessingStartTime,
                            DataEndTime = fileGroup.DataEndTime,
                            DataStartTime = fileGroup.DataStartTime,
                            Error = fileGroup.Error,
                            FileHash = fileGroup.FileHash
                        };
                        int remoteFileGroupId = WebAPIHub.CreateRecord(instance.Address, fg, userAccount);
                        fileGroupLocalToRemote = new FileGroupLocalToRemote()
                        {
                            RemoteXDAInstanceID = instance.ID,
                            LocalFileGroupID = fileGroup.ID,
                            RemoteFileGroupID = remoteFileGroupId
                        };
                        new TableOperations<FileGroupLocalToRemote>(connection).AddNewRecord(fileGroupLocalToRemote);
                    }

                    IEnumerable<DataFile> localDataFiles = new TableOperations<DataFile>(connection).QueryRecordsWhere("FileGroupID = {0}", fileGroupLocalToRemote.LocalFileGroupID);
                    IEnumerable<DataFile> remoteDataFiles = WebAPIHub.GetRecordsWhere<DataFile>(instance.Address, $"FileGroupID = {fileGroupLocalToRemote.RemoteFileGroupID}", userAccount);

                    bool process = false;
                    foreach (DataFile localDataFile in localDataFiles)
                    {
                        int remoteDataFileId;
                        if (!remoteDataFiles.Where(x => x.FilePath == localDataFile.FilePath).Any())
                        {
                            DataFile df = new DataFile()
                            {
                                CreationTime = localDataFile.CreationTime,
                                FileGroupID = fileGroupLocalToRemote.RemoteFileGroupID,
                                FilePath = localDataFile.FilePath,
                                FilePathHash = localDataFile.FilePathHash,
                                FileSize = localDataFile.FileSize,
                                LastAccessTime = localDataFile.LastAccessTime,
                                LastWriteTime = localDataFile.LastWriteTime
                            };
                            remoteDataFileId = WebAPIHub.CreateRecord(instance.Address, df, userAccount);
                            process = true;
                        }
                        else
                            remoteDataFileId = remoteDataFiles.Where(x => x.FilePath == localDataFile.FilePath).First().ID;

                        FileBlob remoteFileBlob = WebAPIHub.GetRecordWhere<FileBlob>(instance.Address, $"DataFileID = {remoteDataFileId}", userAccount);

                        if (remoteFileBlob == null)
                        {
                            FileBlob localFileBlob = new TableOperations<FileBlob>(connection).QueryRecordWhere("DataFileID = {0}", localDataFile.ID);

                            try
                            {
                                if (localFileBlob == null)
                                {
                                    localFileBlob = new FileBlob() { DataFileID = localDataFile.ID, Blob = File.ReadAllBytes(localDataFile.FilePath) };
                                    new TableOperations<FileBlob>(connection).AddNewRecord(localFileBlob);
                                }
                            }
                            catch (Exception ex)
                            {
                                Log.Error(ex);
                                process = false;
                            }
                            localFileBlob.DataFileID = remoteDataFileId;
                            WebAPIHub.CreateRecord(instance.Address, new FileBlob() { DataFileID = remoteDataFileId, Blob = localFileBlob.Blob }, userAccount);

                        }
                    }

                    if (process)
                    {
                        Dictionary<string, int> dictionary = new Dictionary<string, int>();
                        dictionary.Add("FileGroupID", fileGroupLocalToRemote.RemoteFileGroupID);
                        dictionary.Add("MeterID", meterToDataPush.RemoteXDAMeterID);
                        WebAPIHub.ProcessFileGroup(instance.Address, JObject.FromObject(dictionary), userAccount);

                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

        }
      
        private int GetRemoteEventTypeId(string address, int localEventTypeId, UserAccount userAccount)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                IEnumerable<EventType> localTypes = new TableOperations<EventType>(connection).QueryRecords();
                List<EventType> remoteTypes = WebAPIHub.GetRecords<EventType>(address, "all", userAccount).ToList();

                if (!remoteTypes.Where(x => x.Name == localTypes.Where(y => y.ID == localEventTypeId).First().Name).Any())
                {
                    EventType record = new EventType()
                    {
                        Name = localTypes.Where(y => y.ID == localEventTypeId).First().Name,
                        Description = localTypes.Where(y => y.ID == localEventTypeId).First().Description
                    };
                    return WebAPIHub.CreateRecord(address, record, userAccount);
                }
                else
                {
                    return remoteTypes.Where(x => x.Name == localTypes.Where(y => y.ID == localEventTypeId).First().Name).First().ID;
                }
            }
        }

        private int GetRemoteFileGroup(string address, int localFileGroupId, UserAccount userAccount)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                FileGroup local = new TableOperations<FileGroup>(connection).QueryRecordWhere("ID = {0}", localFileGroupId);
                List<FileGroup> remote = WebAPIHub.GetRecords<FileGroup>(address, "all", userAccount).ToList();

                int id;
                if (local != null && !remote.Where(x => x.DataStartTime.Equals(local.DataStartTime) && x.DataEndTime.Equals(local.DataEndTime) && x.ProcessingStartTime.Equals(local.ProcessingStartTime) && x.ProcessingEndTime.Equals(local.ProcessingEndTime)).Any())
                {
                    FileGroup record = new FileGroup()
                    {
                        DataStartTime = local.DataStartTime,
                        DataEndTime = local.DataEndTime,
                        ProcessingStartTime = local.ProcessingStartTime,
                        ProcessingEndTime = local.ProcessingEndTime,
                        Error = local.Error,
                        FileHash = local.FileHash
                    };
                    id = WebAPIHub.CreateRecord(address, record, userAccount);
                }
                else
                {
                    id = remote.Where(x => x.DataStartTime.Equals(local.DataStartTime) && x.DataEndTime.Equals(local.DataEndTime) && x.ProcessingStartTime.Equals(local.ProcessingStartTime) && x.ProcessingEndTime.Equals(local.ProcessingEndTime)).First().ID;
                }

                SyncDataFiles(address, localFileGroupId, id, userAccount);

                return id;
            }
        }

        private void SyncDataFiles(string address, int localFileGroupId, int remoteFileGroupId, UserAccount userAccount)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                IEnumerable<DataFile> local = new TableOperations<DataFile>(connection).QueryRecordsWhere("FileGroupID = {0}", localFileGroupId);
                List<DataFile> remote = WebAPIHub.GetRecordsWhere<DataFile>(address, $"FileGroupID = {remoteFileGroupId}", userAccount).ToList();

                // if there is a local record but not a remote record
                foreach (DataFile localRecord in local)
                {
                    if (!remote.Where(x => x.FilePath == localRecord.FilePath).Any())
                    {
                        DataFile record = new DataFile()
                        {
                            FileGroupID = remoteFileGroupId,
                            FilePath = localRecord.FilePath,
                            FilePathHash = localRecord.FilePathHash,
                            FileSize = localRecord.FileSize,
                            CreationTime = localRecord.CreationTime,
                            LastAccessTime = localRecord.LastAccessTime,
                            LastWriteTime = localRecord.LastWriteTime
                        };
                        int id = WebAPIHub.CreateRecord(address, record, userAccount);

                        SyncFileBlobs(address, localRecord.ID, id, userAccount);
                    }
                }
            }
        }

        private void SyncFileBlobs(string address, int localDataFileId, int remoteDataFileId, UserAccount userAccount)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                FileBlob local = new TableOperations<FileBlob>(connection).QueryRecordWhere("DataFileID = {0}", localDataFileId);
                FileBlob remote = WebAPIHub.GetRecordWhere<FileBlob>(address, $"DataFileID = {remoteDataFileId}", userAccount);

                if (local != null && remote == null)
                {
                    FileBlob record = new FileBlob()
                    {
                        DataFileID = remoteDataFileId,
                        Blob = local.Blob
                    };
                    WebAPIHub.CreateRecord(address, record, userAccount);
                }
            }
        }

        private int GetRemotePhaseId(string address, int localPhaseId, UserAccount userAccount)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                List<Phase> remote = WebAPIHub.GetRecords<Phase>(address, "all", userAccount).ToList();
                Phase local = new TableOperations<Phase>(connection).QueryRecordWhere("ID = {0}", localPhaseId);
                if (remote.Where(x => x.Name == local.Name).Any())
                    return remote.Where(x => x.Name == local.Name).First().ID;
                else
                {
                    Phase record = new Phase()
                    {
                        Name = local.Name,
                        Description = local.Description
                    };

                    return WebAPIHub.CreateRecord(address, record, userAccount);
                }
            }
        }

        #endregion

        #endregion
    }
}
