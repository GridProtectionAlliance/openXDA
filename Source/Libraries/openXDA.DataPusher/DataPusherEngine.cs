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
        private bool m_disposed;
        private ScheduleManager m_scheduler;
        private bool m_running = false;
        private DataPusherSettings m_dataPusherSettings;
        #endregion

        #region [ Constructors ]

        public DataPusherEngine()
        {
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

        public static event EventHandler<EventArgs<string, string, int>> UpdateProgressForMeter;

        private static void OnUpdateProgressForMeter(string client, string meter, int update)
        {
            if (client != string.Empty)
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

        public void Initialize()
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                m_dataPusherSettings = new DataPusherSettings();
                m_dataPusherSettings.Enabled = connection.ExecuteScalar<bool?>("SELECT Value FROM Setting WHERE Name = 'DataPusher.Enabled'") ?? false;
                m_dataPusherSettings.OnlyValidFaults = connection.ExecuteScalar<bool?>("SELECT Value FROM Setting WHERE Name = 'DataPusher.OnlyValidFaults'") ?? true;
                m_dataPusherSettings.TimeWindow = connection.ExecuteScalar<int?>("SELECT Value FROM Setting WHERE Name = 'DataPusher.TimeWindow'") ?? 0;
            }
        }

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

                using (AdoDataConnection connection = new AdoDataConnection("systemSetting"))
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
        public void SyncInstanceConfiguration(string clientId, int instanceId, CancellationToken cancellationToken)
        {
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
            catch (Exception ex)
            {
                Log.Error(ex);
            }

        }

        public void SyncMeterConfigurationForInstance(string clientId, RemoteXDAInstance instance, MetersToDataPush meterToDataPush, UserAccount userAccount, CancellationToken cancellationToken)
        {

            try
            {
                using (AdoDataConnection connection = new AdoDataConnection("systemSetting"))
                {

                    IEnumerable<AssetTypes> localAssetTypes = new TableOperations<AssetTypes>(connection).QueryRecords();
                    IEnumerable<AssetTypes> remoteAssetTypes = WebAPIHub.GetRecords<AssetTypes>(instance.Address, "all", userAccount);

                    // Get local meter record
                    Meter localMeterRecord = new TableOperations<Meter>(connection).QueryRecordWhere("ID = {0}", meterToDataPush.LocalXDAMeterID);
                    // get local MeterLine table 

                    IEnumerable<MeterAsset> localMeterAssets = new TableOperations<MeterAsset>(connection).QueryRecordsWhere("MeterID = {0}", localMeterRecord.ID);


                    // update progress
                    int progressTotal = localMeterAssets.Count() + 2;
                    int progressCount = 0;

                    // Add or Get remote Asset Group
                    AssetGroup remoteAssetGroup = AddOrGetRemoteAssetGroup(instance.Address, userAccount);

                    // Add or Update remote meter location
                    Location remoteLocation = AddOrGetRemoteLocation(instance.Address, meterToDataPush, localMeterRecord, userAccount);

                    // update progress
                    OnUpdateProgressForMeter(clientId, localMeterRecord.AssetKey, (int)(100 * (progressCount) / progressTotal));

                    // if meter doesnt exist remotely add it
                    Meter remoteMeter = AddOrGetRemoteMeter(instance.Address, meterToDataPush, localMeterRecord, remoteLocation, userAccount);

                    // update progress
                    OnUpdateProgressForMeter(clientId, localMeterRecord.AssetKey, (int)(100 * (++progressCount) / progressTotal));

                    // If does not exist add link for meter to the appropriate asset group
                    AddMeterAssetGroup(instance.Address, remoteAssetGroup.ID, meterToDataPush.RemoteXDAMeterID, userAccount);

                    // update progress
                    OnUpdateProgressForMeter(clientId, localMeterRecord.AssetKey, (int)(100 * (++progressCount) / progressTotal));

                    // if there is a line for the meter ensure that its data has been uploaded remotely
                    foreach (MeterAsset localMeterAsset in localMeterAssets)
                    {
                        if (cancellationToken.IsCancellationRequested)
                            break;

                        // Add or Update Asset
                        Asset remoteAseset = AddOrGetAsset(instance.Address, localMeterAsset, localAssetTypes, remoteAssetTypes, meterToDataPush.Obsfucate, userAccount);
                        // Note that this needs to get the Asset specifics (e.g. LineAttributes) sepperatly
                        // That broke in 3.0 

                        // if MeterLine association has not been previously made, make it
                        MeterAsset remoteMeterAsset = AddMeterAsset(instance.Address, meterToDataPush, remoteAseset, userAccount, localMeterAsset);

                        // add line to meterlocationline table
                        AssetLocation remoteAssetLocation = AddOrUpdateAssetLocation(instance.Address, remoteAseset, remoteLocation, userAccount);

                        // Sync Channel and channel dependant data
                        AddOrUpdateChannelsForLine(instance.Address, localMeterAsset, remoteMeterAsset, userAccount);

                        OnUpdateProgressForMeter(clientId, localMeterRecord.AssetKey, (int)(100 * (++progressCount) / progressTotal));
                    }

                    IEnumerable<AssetConnection> localAssetConnections = new TableOperations<AssetConnection>(connection).QueryRecordsWhere("ParentID IN (SELECT AssetID FROM AssetLocation WHERE LocationID = {0}) OR ChildID IN (SELECT AssetID FROM AssetLocation WHERE LocationID = {0})", localMeterRecord.LocationID);
                    IEnumerable<AssetConnectionType> localAssetConnectionTypes = new TableOperations<AssetConnectionType>(connection).QueryRecords();
                    IEnumerable<AssetConnectionType> remoteAssetConnectionTypes = WebAPIHub.GetRecords<AssetConnectionType>(instance.Address, "all", userAccount);
                    foreach (var localAssetConnection in localAssetConnections) {
                        AddAssetConnections(instance.Address, localAssetConnection, localAssetConnectionTypes.First(x => x.ID == localAssetConnection.ID).Name, remoteAssetConnectionTypes, userAccount);
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

        private Location AddOrGetRemoteLocation(string address, MetersToDataPush meterToDataPush, Meter localMeterRecord, UserAccount userAccount)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSetting"))
            {

                Location localLocation = new TableOperations<Location>(connection).QueryRecordWhere("ID = {0}", localMeterRecord.LocationID);
                Location remoteLocation = WebAPIHub.GetRecordWhere<Location>(address, $"LocationKey='{(meterToDataPush.Obsfucate? meterToDataPush.RemoteXDAAssetKey : localLocation.LocationKey)}'", userAccount);

                // if the company meter location does not exist, create it
                if (remoteLocation == null)
                {
                    if (meterToDataPush.Obsfucate)
                    {
                        remoteLocation = new Location()
                        {
                            LocationKey = meterToDataPush.RemoteXDAAssetKey,
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
                        remoteLocation = new Location()
                        {
                            LocationKey = meterToDataPush.RemoteXDAAssetKey,
                            Name = localLocation.Name,
                            Alias = localLocation.Alias,
                            ShortName = localLocation.ShortName,
                            Description = localLocation.Description,
                            Latitude = localLocation.Latitude,
                            Longitude = localLocation.Longitude

                        };
                    }
                    remoteLocation.ID = WebAPIHub.CreateRecord(address, remoteLocation, userAccount);

                }
                else
                {
                    if (meterToDataPush.Obsfucate)
                    {
                        remoteLocation.LocationKey = meterToDataPush.RemoteXDAAssetKey;
                        remoteLocation.Name = meterToDataPush.RemoteXDAAssetKey;
                        remoteLocation.Alias = meterToDataPush.RemoteXDAAssetKey;
                        remoteLocation.ShortName = "";
                        remoteLocation.Description = "";
                        remoteLocation.Latitude = 0.0F;
                        remoteLocation.Longitude = 0.0F;

                    }
                    else
                    {
                        remoteLocation.LocationKey = meterToDataPush.RemoteXDAAssetKey;
                        remoteLocation.Name = localLocation.Name;
                        remoteLocation.Alias = localLocation.Alias;
                        remoteLocation.ShortName = localLocation.ShortName;
                        remoteLocation.Description = localLocation.Description;
                        remoteLocation.Latitude = localLocation.Latitude;
                        remoteLocation.Longitude = localLocation.Longitude;
                    }

                    WebAPIHub.UpdateRecord(address, remoteLocation, userAccount);
                }

                return remoteLocation;
            }
        }

        private Meter AddOrGetRemoteMeter(string address, MetersToDataPush meter, Meter localMeterRecord, Location remoteMeterLocation, UserAccount userAccount)
        {
            Meter remoteMeter = WebAPIHub.GetRecordWhere<Meter>(address, $"ID={meter.RemoteXDAMeterID}", userAccount);

            if (remoteMeter == null)
            {
                remoteMeter = WebAPIHub.GetRecordWhere<Meter>(address, $"AssetKey='{meter.RemoteXDAAssetKey}'", userAccount);
                if (remoteMeter != null)
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
                remoteMeter.LocationID = remoteMeterLocation.ID;
                remoteMeter.Make = localMeterRecord.Make;
                remoteMeter.Model = localMeterRecord.Model;
                remoteMeter.Description = localMeterRecord.Description;
                remoteMeter.TimeZone = localMeterRecord.TimeZone;

                remoteMeter.ID = meter.RemoteXDAMeterID = WebAPIHub.CreateRecord(address, remoteMeter, userAccount);
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
                remoteMeter.LocationID = remoteMeterLocation.ID;
                remoteMeter.Make = localMeterRecord.Make;
                remoteMeter.Model = localMeterRecord.Model;
                remoteMeter.Description = localMeterRecord.Description;
                remoteMeter.TimeZone = localMeterRecord.TimeZone;

                WebAPIHub.UpdateRecord(address, remoteMeter, userAccount);
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

                WebAPIHub.CreateRecord(address, remoteMeterAssetGroup, userAccount);
            }
        }

        private Asset AddOrGetAsset(string address, MeterAsset meterAsset, IEnumerable<AssetTypes> localAssetTypes, IEnumerable<AssetTypes> remoteAssetTypes, bool obsfucate, UserAccount userAccount)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                Asset localAsset = new TableOperations<Asset>(connection).QueryRecordWhere("ID = {0}", meterAsset.AssetID);
                AssetsToDataPush assetToDataPush = new TableOperations<AssetsToDataPush>(connection).QueryRecordWhere("LocalXDAAssetID = {0}", localAsset.ID);
                Asset remoteAsset = null;
                string assetType = localAssetTypes.First(x => x.ID == localAsset.AssetTypeID).Name;
                //if the line does not exist in the PQMarkPusher Database to allow for obsfucation add it.
                if (assetType == "Line")
                    remoteAsset = AddOrGetLine(address, localAsset, assetToDataPush, remoteAssetTypes, obsfucate, userAccount);
                else if (assetType == "LineSegment")
                    remoteAsset = AddOrGetLineSegment(address, localAsset, assetToDataPush, remoteAssetTypes, obsfucate, userAccount);
                else if (assetType == "Breaker")
                    remoteAsset = AddOrGetBreaker(address, localAsset, assetToDataPush, remoteAssetTypes, obsfucate, userAccount);
                else if (assetType == "Transformer")
                    remoteAsset = AddOrGetTransformer(address, localAsset, assetToDataPush, remoteAssetTypes, obsfucate, userAccount);
                else if (assetType == "Bus")
                    remoteAsset = AddOrGetBus(address, localAsset, assetToDataPush, remoteAssetTypes, obsfucate, userAccount);
                else if (assetType == "CapacitorBank")
                    remoteAsset = AddOrGetCapBank(address, localAsset, assetToDataPush, remoteAssetTypes, obsfucate, userAccount);

                if (remoteAsset == null) throw new Exception("Asset not defined.");
                return remoteAsset;
            }
        }

        private Line AddOrGetLine(string address, Asset localAsset, AssetsToDataPush assetToDataPush, IEnumerable<AssetTypes> remoteAssetTypes, bool obsfucate, UserAccount userAccount) {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings")) {
                Line remoteLine = null;
                Line localLine = new TableOperations<Line>(connection).QueryRecordWhere("AssetKey = {0}", localAsset.AssetKey);

                //if the line does not exist in the PQMarkPusher Database to allow for obsfucation add it.
                if (assetToDataPush == null)
                {
                    string obsfucatedAssetName = Guid.NewGuid().ToString();
                    remoteLine = WebAPIHub.GetRecordWhere<Line>(address, $"AssetKey='{localAsset.AssetKey}'", userAccount);

                    if (remoteLine == null)
                    {
                        remoteLine = localLine;
                        // need to fix obsfucation
                        if (obsfucate)
                        {
                            remoteLine.AssetKey = obsfucatedAssetName;
                            remoteLine.AssetName = obsfucatedAssetName;
                            remoteLine.Description = "";
                        }

                        remoteLine.ID = 0;
                        remoteLine.AssetTypeID = remoteAssetTypes.First(x => x.Name == "Line").ID;
                        remoteLine.ID = WebAPIHub.CreateRecord(address, remoteLine, userAccount);

                        assetToDataPush = new AssetsToDataPush()
                        {
                            LocalXDAAssetID = localAsset.ID,
                            RemoteXDAAssetID = remoteLine.ID,
                            LocalXDAAssetKey = localAsset.AssetKey,
                            RemoteXDAAssetKey = (obsfucate ? obsfucatedAssetName : localAsset.AssetKey),
                            RemoteAssetCreatedByDataPusher = true
                        };

                    }
                    else
                    {
                        assetToDataPush = new AssetsToDataPush()
                        {
                            LocalXDAAssetID = localAsset.ID,
                            RemoteXDAAssetID = remoteLine.ID,
                            LocalXDAAssetKey = localAsset.AssetKey,
                            RemoteXDAAssetKey = (obsfucate ? obsfucatedAssetName : localAsset.AssetKey),
                            RemoteAssetCreatedByDataPusher = false
                        };
                    }

                    new TableOperations<AssetsToDataPush>(connection).AddNewRecord(assetToDataPush);
                }
                else
                {
                    remoteLine = WebAPIHub.GetRecordWhere<Line>(address, $"ID={assetToDataPush.RemoteXDAAssetID}", userAccount);

                    if (remoteLine == null)
                    {
                        remoteLine = localLine;
                        if (obsfucate)
                        {
                            remoteLine.AssetKey = assetToDataPush.RemoteXDAAssetKey;
                            remoteLine.AssetName = assetToDataPush.RemoteXDAAssetKey;
                            remoteLine.Description = "";
                        }

                        remoteLine.ID = 0;
                        remoteLine.AssetTypeID = remoteAssetTypes.First(x => x.Name == "Line").ID;
                        remoteLine.ID = WebAPIHub.CreateRecord(address, remoteLine, userAccount);
                        remoteLine.ID = WebAPIHub.CreateRecord<Asset>(address, remoteLine, userAccount);

                        assetToDataPush.RemoteXDAAssetID = remoteLine.ID;
                        assetToDataPush.RemoteXDAAssetKey = remoteLine.AssetKey;
                        assetToDataPush.RemoteAssetCreatedByDataPusher = true;

                        new TableOperations<AssetsToDataPush>(connection).UpdateRecord(assetToDataPush);
                    }
                    else if (assetToDataPush.RemoteAssetCreatedByDataPusher)
                    {
                        remoteLine.AssetKey = (obsfucate ? assetToDataPush.RemoteXDAAssetKey.ToString() : localAsset.AssetKey);
                        remoteLine.AssetName = (obsfucate ? assetToDataPush.RemoteXDAAssetKey.ToString() : localAsset.AssetName);
                        remoteLine.VoltageKV = localAsset.VoltageKV;
                        remoteLine.AssetTypeID = localAsset.AssetTypeID;
                        remoteLine.Spare = localAsset.Spare;
                        remoteLine.Description = (obsfucate ? "" : localAsset.Description);
                        WebAPIHub.UpdateRecord(address, remoteLine, userAccount);
                    }
                }

                if (remoteLine == null) throw new Exception("Asset not defined.");
                return remoteLine;
            }
        }
        private Bus AddOrGetBus(string address, Asset localAsset, AssetsToDataPush assetToDataPush, IEnumerable<AssetTypes> remoteAssetTypes, bool obsfucate, UserAccount userAccount) {

            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                Bus remoteBus = null;
                Bus localBus = new TableOperations<Bus>(connection).QueryRecordWhere("AssetKey = {0}", localAsset.AssetKey);

                //if the Bus does not exist in the PQMarkPusher Database to allow for obsfucation add it.
                if (assetToDataPush == null)
                {
                    string obsfucatedAssetName = Guid.NewGuid().ToString();
                    remoteBus = WebAPIHub.GetRecordWhere<Bus>(address, $"AssetKey='{localAsset.AssetKey}'", userAccount);

                    if (remoteBus == null)
                    {
                        remoteBus = localBus;
                        // need to fix obsfucation
                        if (obsfucate)
                        {
                            remoteBus.AssetKey = obsfucatedAssetName;
                            remoteBus.AssetName = obsfucatedAssetName;
                            remoteBus.Description = "";
                        }

                        remoteBus.ID = 0;
                        remoteBus.AssetTypeID = remoteAssetTypes.First(x => x.Name == "Bus").ID;
                        remoteBus.ID = WebAPIHub.CreateRecord(address, remoteBus, userAccount);

                        assetToDataPush = new AssetsToDataPush()
                        {
                            LocalXDAAssetID = localAsset.ID,
                            RemoteXDAAssetID = remoteBus.ID,
                            LocalXDAAssetKey = localAsset.AssetKey,
                            RemoteXDAAssetKey = (obsfucate ? obsfucatedAssetName : localAsset.AssetKey),
                            RemoteAssetCreatedByDataPusher = true
                        };

                    }
                    else
                    {
                        assetToDataPush = new AssetsToDataPush()
                        {
                            LocalXDAAssetID = localAsset.ID,
                            RemoteXDAAssetID = remoteBus.ID,
                            LocalXDAAssetKey = localAsset.AssetKey,
                            RemoteXDAAssetKey = (obsfucate ? obsfucatedAssetName : localAsset.AssetKey),
                            RemoteAssetCreatedByDataPusher = false
                        };
                    }

                    new TableOperations<AssetsToDataPush>(connection).AddNewRecord(assetToDataPush);
                }
                else
                {
                    remoteBus = WebAPIHub.GetRecordWhere<Bus>(address, $"ID={assetToDataPush.RemoteXDAAssetID}", userAccount);

                    if (remoteBus == null)
                    {
                        remoteBus = localBus;
                        if (obsfucate)
                        {
                            remoteBus.AssetKey = assetToDataPush.RemoteXDAAssetKey;
                            remoteBus.AssetName = assetToDataPush.RemoteXDAAssetKey;
                            remoteBus.Description = "";
                        }

                        remoteBus.ID = 0;
                        remoteBus.AssetTypeID = remoteAssetTypes.First(x => x.Name == "Bus").ID;
                        remoteBus.ID = WebAPIHub.CreateRecord(address, remoteBus, userAccount);
                        remoteBus.ID = WebAPIHub.CreateRecord<Asset>(address, remoteBus, userAccount);

                        assetToDataPush.RemoteXDAAssetID = remoteBus.ID;
                        assetToDataPush.RemoteXDAAssetKey = remoteBus.AssetKey;
                        assetToDataPush.RemoteAssetCreatedByDataPusher = true;

                        new TableOperations<AssetsToDataPush>(connection).UpdateRecord(assetToDataPush);
                    }
                    else if (assetToDataPush.RemoteAssetCreatedByDataPusher)
                    {
                        remoteBus.AssetKey = (obsfucate ? assetToDataPush.RemoteXDAAssetKey.ToString() : localAsset.AssetKey);
                        remoteBus.AssetName = (obsfucate ? assetToDataPush.RemoteXDAAssetKey.ToString() : localAsset.AssetName);
                        remoteBus.VoltageKV = localAsset.VoltageKV;
                        remoteBus.AssetTypeID = localAsset.AssetTypeID;
                        remoteBus.Spare = localAsset.Spare;
                        remoteBus.Description = (obsfucate ? "" : localAsset.Description);
                        WebAPIHub.UpdateRecord(address, remoteBus, userAccount);
                    }
                }

                if (remoteBus == null) throw new Exception("Asset not defined.");
                return remoteBus;
            }
        }
        private LineSegment AddOrGetLineSegment(string address, Asset localAsset, AssetsToDataPush assetToDataPush, IEnumerable<AssetTypes> remoteAssetTypes, bool obsfucate, UserAccount userAccount) {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                LineSegment remoteLineSegment = null;
                LineSegment localLineSegment = new TableOperations<LineSegment>(connection).QueryRecordWhere("AssetKey = {0}", localAsset.AssetKey);

                //if the LineSegment does not exist in the PQMarkPusher Database to allow for obsfucation add it.
                if (assetToDataPush == null)
                {
                    string obsfucatedAssetName = Guid.NewGuid().ToString();
                    remoteLineSegment = WebAPIHub.GetRecordWhere<LineSegment>(address, $"AssetKey='{localAsset.AssetKey}'", userAccount);

                    if (remoteLineSegment == null)
                    {
                        remoteLineSegment = localLineSegment;
                        // need to fix obsfucation
                        if (obsfucate)
                        {
                            remoteLineSegment.AssetKey = obsfucatedAssetName;
                            remoteLineSegment.AssetName = obsfucatedAssetName;
                            remoteLineSegment.Description = "";
                        }

                        remoteLineSegment.ID = 0;
                        remoteLineSegment.AssetTypeID = remoteAssetTypes.First(x => x.Name == "LineSegment").ID;
                        remoteLineSegment.ID = WebAPIHub.CreateRecord(address, remoteLineSegment, userAccount);

                        assetToDataPush = new AssetsToDataPush()
                        {
                            LocalXDAAssetID = localAsset.ID,
                            RemoteXDAAssetID = remoteLineSegment.ID,
                            LocalXDAAssetKey = localAsset.AssetKey,
                            RemoteXDAAssetKey = (obsfucate ? obsfucatedAssetName : localAsset.AssetKey),
                            RemoteAssetCreatedByDataPusher = true
                        };

                    }
                    else
                    {
                        assetToDataPush = new AssetsToDataPush()
                        {
                            LocalXDAAssetID = localAsset.ID,
                            RemoteXDAAssetID = remoteLineSegment.ID,
                            LocalXDAAssetKey = localAsset.AssetKey,
                            RemoteXDAAssetKey = (obsfucate ? obsfucatedAssetName : localAsset.AssetKey),
                            RemoteAssetCreatedByDataPusher = false
                        };
                    }

                    new TableOperations<AssetsToDataPush>(connection).AddNewRecord(assetToDataPush);
                }
                else
                {
                    remoteLineSegment = WebAPIHub.GetRecordWhere<LineSegment>(address, $"ID={assetToDataPush.RemoteXDAAssetID}", userAccount);

                    if (remoteLineSegment == null)
                    {
                        remoteLineSegment = localLineSegment;
                        if (obsfucate)
                        {
                            remoteLineSegment.AssetKey = assetToDataPush.RemoteXDAAssetKey;
                            remoteLineSegment.AssetName = assetToDataPush.RemoteXDAAssetKey;
                            remoteLineSegment.Description = "";
                        }

                        remoteLineSegment.ID = 0;
                        remoteLineSegment.AssetTypeID = remoteAssetTypes.First(x => x.Name == "LineSegment").ID;
                        remoteLineSegment.ID = WebAPIHub.CreateRecord(address, remoteLineSegment, userAccount);
                        remoteLineSegment.ID = WebAPIHub.CreateRecord<Asset>(address, remoteLineSegment, userAccount);

                        assetToDataPush.RemoteXDAAssetID = remoteLineSegment.ID;
                        assetToDataPush.RemoteXDAAssetKey = remoteLineSegment.AssetKey;
                        assetToDataPush.RemoteAssetCreatedByDataPusher = true;

                        new TableOperations<AssetsToDataPush>(connection).UpdateRecord(assetToDataPush);
                    }
                    else if (assetToDataPush.RemoteAssetCreatedByDataPusher)
                    {
                        remoteLineSegment.AssetKey = (obsfucate ? assetToDataPush.RemoteXDAAssetKey.ToString() : localAsset.AssetKey);
                        remoteLineSegment.AssetName = (obsfucate ? assetToDataPush.RemoteXDAAssetKey.ToString() : localAsset.AssetName);
                        remoteLineSegment.VoltageKV = localAsset.VoltageKV;
                        remoteLineSegment.AssetTypeID = localAsset.AssetTypeID;
                        remoteLineSegment.Spare = localAsset.Spare;
                        remoteLineSegment.Description = (obsfucate ? "" : localAsset.Description);
                        WebAPIHub.UpdateRecord(address, remoteLineSegment, userAccount);
                    }
                }

                if (remoteLineSegment == null) throw new Exception("Asset not defined.");
                return remoteLineSegment;
            }
        }       
        private CapBank AddOrGetCapBank(string address, Asset localAsset, AssetsToDataPush assetToDataPush, IEnumerable<AssetTypes> remoteAssetTypes, bool obsfucate, UserAccount userAccount)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                CapBank remoteCapBank = null;
                CapBank localCapBank = new TableOperations<CapBank>(connection).QueryRecordWhere("AssetKey = {0}", localAsset.AssetKey);

                //if the CapBank does not exist in the PQMarkPusher Database to allow for obsfucation add it.
                if (assetToDataPush == null)
                {
                    string obsfucatedAssetName = Guid.NewGuid().ToString();
                    remoteCapBank = WebAPIHub.GetRecordWhere<CapBank>(address, $"AssetKey='{localAsset.AssetKey}'", userAccount);

                    if (remoteCapBank == null)
                    {
                        remoteCapBank = localCapBank;
                        // need to fix obsfucation
                        if (obsfucate)
                        {
                            remoteCapBank.AssetKey = obsfucatedAssetName;
                            remoteCapBank.AssetName = obsfucatedAssetName;
                            remoteCapBank.Description = "";
                        }

                        remoteCapBank.ID = 0;
                        remoteCapBank.AssetTypeID = remoteAssetTypes.First(x => x.Name == "CapBank").ID;
                        remoteCapBank.ID = WebAPIHub.CreateRecord(address, remoteCapBank, userAccount);

                        assetToDataPush = new AssetsToDataPush()
                        {
                            LocalXDAAssetID = localAsset.ID,
                            RemoteXDAAssetID = remoteCapBank.ID,
                            LocalXDAAssetKey = localAsset.AssetKey,
                            RemoteXDAAssetKey = (obsfucate ? obsfucatedAssetName : localAsset.AssetKey),
                            RemoteAssetCreatedByDataPusher = true
                        };

                    }
                    else
                    {
                        assetToDataPush = new AssetsToDataPush()
                        {
                            LocalXDAAssetID = localAsset.ID,
                            RemoteXDAAssetID = remoteCapBank.ID,
                            LocalXDAAssetKey = localAsset.AssetKey,
                            RemoteXDAAssetKey = (obsfucate ? obsfucatedAssetName : localAsset.AssetKey),
                            RemoteAssetCreatedByDataPusher = false
                        };
                    }

                    new TableOperations<AssetsToDataPush>(connection).AddNewRecord(assetToDataPush);
                }
                else
                {
                    remoteCapBank = WebAPIHub.GetRecordWhere<CapBank>(address, $"ID={assetToDataPush.RemoteXDAAssetID}", userAccount);

                    if (remoteCapBank == null)
                    {
                        remoteCapBank = localCapBank;
                        if (obsfucate)
                        {
                            remoteCapBank.AssetKey = assetToDataPush.RemoteXDAAssetKey;
                            remoteCapBank.AssetName = assetToDataPush.RemoteXDAAssetKey;
                            remoteCapBank.Description = "";
                        }

                        remoteCapBank.ID = 0;
                        remoteCapBank.AssetTypeID = remoteAssetTypes.First(x => x.Name == "CapBank").ID;
                        remoteCapBank.ID = WebAPIHub.CreateRecord(address, remoteCapBank, userAccount);
                        remoteCapBank.ID = WebAPIHub.CreateRecord<Asset>(address, remoteCapBank, userAccount);

                        assetToDataPush.RemoteXDAAssetID = remoteCapBank.ID;
                        assetToDataPush.RemoteXDAAssetKey = remoteCapBank.AssetKey;
                        assetToDataPush.RemoteAssetCreatedByDataPusher = true;

                        new TableOperations<AssetsToDataPush>(connection).UpdateRecord(assetToDataPush);
                    }
                    else if (assetToDataPush.RemoteAssetCreatedByDataPusher)
                    {
                        remoteCapBank.AssetKey = (obsfucate ? assetToDataPush.RemoteXDAAssetKey.ToString() : localAsset.AssetKey);
                        remoteCapBank.AssetName = (obsfucate ? assetToDataPush.RemoteXDAAssetKey.ToString() : localAsset.AssetName);
                        remoteCapBank.VoltageKV = localAsset.VoltageKV;
                        remoteCapBank.AssetTypeID = localAsset.AssetTypeID;
                        remoteCapBank.Spare = localAsset.Spare;
                        remoteCapBank.Description = (obsfucate ? "" : localAsset.Description);
                        WebAPIHub.UpdateRecord(address, remoteCapBank, userAccount);
                    }
                }

                if (remoteCapBank == null) throw new Exception("Asset not defined.");
                return remoteCapBank;
            }
        }
        private Breaker AddOrGetBreaker(string address, Asset localAsset, AssetsToDataPush assetToDataPush, IEnumerable<AssetTypes> remoteAssetTypes, bool obsfucate, UserAccount userAccount)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                Breaker remoteBreaker = null;
                Breaker localBreaker = new TableOperations<Breaker>(connection).QueryRecordWhere("AssetKey = {0}", localAsset.AssetKey);

                //if the Breaker does not exist in the PQMarkPusher Database to allow for obsfucation add it.
                if (assetToDataPush == null)
                {
                    string obsfucatedAssetName = Guid.NewGuid().ToString();
                    remoteBreaker = WebAPIHub.GetRecordWhere<Breaker>(address, $"AssetKey='{localAsset.AssetKey}'", userAccount);

                    if (remoteBreaker == null)
                    {
                        remoteBreaker = localBreaker;
                        // need to fix obsfucation
                        if (obsfucate)
                        {
                            remoteBreaker.AssetKey = obsfucatedAssetName;
                            remoteBreaker.AssetName = obsfucatedAssetName;
                            remoteBreaker.Description = "";
                        }

                        remoteBreaker.ID = 0;
                        remoteBreaker.AssetTypeID = remoteAssetTypes.First(x => x.Name == "Breaker").ID;
                        remoteBreaker.ID = WebAPIHub.CreateRecord(address, remoteBreaker, userAccount);

                        assetToDataPush = new AssetsToDataPush()
                        {
                            LocalXDAAssetID = localAsset.ID,
                            RemoteXDAAssetID = remoteBreaker.ID,
                            LocalXDAAssetKey = localAsset.AssetKey,
                            RemoteXDAAssetKey = (obsfucate ? obsfucatedAssetName : localAsset.AssetKey),
                            RemoteAssetCreatedByDataPusher = true
                        };

                    }
                    else
                    {
                        assetToDataPush = new AssetsToDataPush()
                        {
                            LocalXDAAssetID = localAsset.ID,
                            RemoteXDAAssetID = remoteBreaker.ID,
                            LocalXDAAssetKey = localAsset.AssetKey,
                            RemoteXDAAssetKey = (obsfucate ? obsfucatedAssetName : localAsset.AssetKey),
                            RemoteAssetCreatedByDataPusher = false
                        };
                    }

                    new TableOperations<AssetsToDataPush>(connection).AddNewRecord(assetToDataPush);
                }
                else
                {
                    remoteBreaker = WebAPIHub.GetRecordWhere<Breaker>(address, $"ID={assetToDataPush.RemoteXDAAssetID}", userAccount);

                    if (remoteBreaker == null)
                    {
                        remoteBreaker = localBreaker;
                        if (obsfucate)
                        {
                            remoteBreaker.AssetKey = assetToDataPush.RemoteXDAAssetKey;
                            remoteBreaker.AssetName = assetToDataPush.RemoteXDAAssetKey;
                            remoteBreaker.Description = "";
                        }

                        remoteBreaker.ID = 0;
                        remoteBreaker.AssetTypeID = remoteAssetTypes.First(x => x.Name == "Breaker").ID;
                        remoteBreaker.ID = WebAPIHub.CreateRecord(address, remoteBreaker, userAccount);
                        remoteBreaker.ID = WebAPIHub.CreateRecord<Asset>(address, remoteBreaker, userAccount);

                        assetToDataPush.RemoteXDAAssetID = remoteBreaker.ID;
                        assetToDataPush.RemoteXDAAssetKey = remoteBreaker.AssetKey;
                        assetToDataPush.RemoteAssetCreatedByDataPusher = true;

                        new TableOperations<AssetsToDataPush>(connection).UpdateRecord(assetToDataPush);
                    }
                    else if (assetToDataPush.RemoteAssetCreatedByDataPusher)
                    {
                        remoteBreaker.AssetKey = (obsfucate ? assetToDataPush.RemoteXDAAssetKey.ToString() : localAsset.AssetKey);
                        remoteBreaker.AssetName = (obsfucate ? assetToDataPush.RemoteXDAAssetKey.ToString() : localAsset.AssetName);
                        remoteBreaker.VoltageKV = localAsset.VoltageKV;
                        remoteBreaker.AssetTypeID = localAsset.AssetTypeID;
                        remoteBreaker.Spare = localAsset.Spare;
                        remoteBreaker.Description = (obsfucate ? "" : localAsset.Description);
                        WebAPIHub.UpdateRecord(address, remoteBreaker, userAccount);
                    }
                }

                if (remoteBreaker == null) throw new Exception("Asset not defined.");
                return remoteBreaker;
            }
        }
        private Transformer AddOrGetTransformer(string address, Asset localAsset, AssetsToDataPush assetToDataPush, IEnumerable<AssetTypes> remoteAssetTypes, bool obsfucate, UserAccount userAccount)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                Transformer remoteTransformer = null;
                Transformer localTransformer = new TableOperations<Transformer>(connection).QueryRecordWhere("AssetKey = {0}", localAsset.AssetKey);

                //if the Transformer does not exist in the PQMarkPusher Database to allow for obsfucation add it.
                if (assetToDataPush == null)
                {
                    string obsfucatedAssetName = Guid.NewGuid().ToString();
                    remoteTransformer = WebAPIHub.GetRecordWhere<Transformer>(address, $"AssetKey='{localAsset.AssetKey}'", userAccount);

                    if (remoteTransformer == null)
                    {
                        remoteTransformer = localTransformer;
                        // need to fix obsfucation
                        if (obsfucate)
                        {
                            remoteTransformer.AssetKey = obsfucatedAssetName;
                            remoteTransformer.AssetName = obsfucatedAssetName;
                            remoteTransformer.Description = "";
                        }

                        remoteTransformer.ID = 0;
                        remoteTransformer.AssetTypeID = remoteAssetTypes.First(x => x.Name == "Transformer").ID;
                        remoteTransformer.ID = WebAPIHub.CreateRecord(address, remoteTransformer, userAccount);

                        assetToDataPush = new AssetsToDataPush()
                        {
                            LocalXDAAssetID = localAsset.ID,
                            RemoteXDAAssetID = remoteTransformer.ID,
                            LocalXDAAssetKey = localAsset.AssetKey,
                            RemoteXDAAssetKey = (obsfucate ? obsfucatedAssetName : localAsset.AssetKey),
                            RemoteAssetCreatedByDataPusher = true
                        };

                    }
                    else
                    {
                        assetToDataPush = new AssetsToDataPush()
                        {
                            LocalXDAAssetID = localAsset.ID,
                            RemoteXDAAssetID = remoteTransformer.ID,
                            LocalXDAAssetKey = localAsset.AssetKey,
                            RemoteXDAAssetKey = (obsfucate ? obsfucatedAssetName : localAsset.AssetKey),
                            RemoteAssetCreatedByDataPusher = false
                        };
                    }

                    new TableOperations<AssetsToDataPush>(connection).AddNewRecord(assetToDataPush);
                }
                else
                {
                    remoteTransformer = WebAPIHub.GetRecordWhere<Transformer>(address, $"ID={assetToDataPush.RemoteXDAAssetID}", userAccount);

                    if (remoteTransformer == null)
                    {
                        remoteTransformer = localTransformer;
                        if (obsfucate)
                        {
                            remoteTransformer.AssetKey = assetToDataPush.RemoteXDAAssetKey;
                            remoteTransformer.AssetName = assetToDataPush.RemoteXDAAssetKey;
                            remoteTransformer.Description = "";
                        }

                        remoteTransformer.ID = 0;
                        remoteTransformer.AssetTypeID = remoteAssetTypes.First(x => x.Name == "Transformer").ID;
                        remoteTransformer.ID = WebAPIHub.CreateRecord(address, remoteTransformer, userAccount);
                        remoteTransformer.ID = WebAPIHub.CreateRecord<Asset>(address, remoteTransformer, userAccount);

                        assetToDataPush.RemoteXDAAssetID = remoteTransformer.ID;
                        assetToDataPush.RemoteXDAAssetKey = remoteTransformer.AssetKey;
                        assetToDataPush.RemoteAssetCreatedByDataPusher = true;

                        new TableOperations<AssetsToDataPush>(connection).UpdateRecord(assetToDataPush);
                    }
                    else if (assetToDataPush.RemoteAssetCreatedByDataPusher)
                    {
                        remoteTransformer.AssetKey = (obsfucate ? assetToDataPush.RemoteXDAAssetKey.ToString() : localAsset.AssetKey);
                        remoteTransformer.AssetName = (obsfucate ? assetToDataPush.RemoteXDAAssetKey.ToString() : localAsset.AssetName);
                        remoteTransformer.VoltageKV = localAsset.VoltageKV;
                        remoteTransformer.AssetTypeID = localAsset.AssetTypeID;
                        remoteTransformer.Spare = localAsset.Spare;
                        remoteTransformer.Description = (obsfucate ? "" : localAsset.Description);
                        WebAPIHub.UpdateRecord(address, remoteTransformer, userAccount);
                    }
                }

                if (remoteTransformer == null) throw new Exception("Asset not defined.");
                return remoteTransformer;
            }
        }

        private MeterAsset AddMeterAsset(string address, MetersToDataPush meter, Asset remoteLine, UserAccount userAccount, MeterAsset localMeterLine)
        {
            MeterAsset remoteMeterLine = WebAPIHub.GetRecordWhere<MeterAsset>(address, $"MeterID = {meter.RemoteXDAMeterID} AND AssetID = {remoteLine.ID}", userAccount);

            // if MeterLine association has not been previously made, make it
            if (remoteMeterLine == null)
            {
                remoteMeterLine = new MeterAsset()
                {
                    MeterID = meter.RemoteXDAMeterID,
                    AssetID = remoteLine.ID,
                };

                remoteMeterLine.ID = WebAPIHub.CreateRecord(address, remoteMeterLine, userAccount);
            }
            else
            {
                WebAPIHub.UpdateRecord(address, remoteMeterLine, userAccount);
            }

            AddFaultDetectionLogic(address, localMeterLine, remoteMeterLine, userAccount);

            return remoteMeterLine;
        }

        private void AddFaultDetectionLogic(string address, MeterAsset localMeterLine, MeterAsset remoteMeterLine, UserAccount userAccount)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                FaultDetectionLogic localFaultDetectionLogic = new TableOperations<FaultDetectionLogic>(connection).QueryRecordWhere("MeterAssetID = {0}", localMeterLine.ID);
                if (localFaultDetectionLogic == null) return;

                FaultDetectionLogic remoteFaultDetectionLogic = WebAPIHub.GetRecordWhere<FaultDetectionLogic>(address, $"MeterAssetID = {remoteMeterLine.ID}", userAccount);


                if (remoteFaultDetectionLogic == null)
                {
                    remoteFaultDetectionLogic = new FaultDetectionLogic()
                    {
                        MeterAssetID = remoteMeterLine.ID,
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

        private AssetLocation AddOrUpdateAssetLocation(string address, Asset remoteAsset, Location remoteLocation, UserAccount userAccount)
        {
            // add line to meterlocationline table
            AssetLocation remoteMeterLocationLine = WebAPIHub.GetRecordWhere<AssetLocation>(address, $"LocationID = {remoteLocation.ID} AND AssetID = {remoteAsset.ID}", userAccount);

            if (remoteMeterLocationLine == null)
            {
                remoteMeterLocationLine = new AssetLocation();
                remoteMeterLocationLine.AssetID = remoteAsset.ID;
                remoteMeterLocationLine.LocationID = remoteLocation.ID;

                WebAPIHub.CreateRecord(address, remoteMeterLocationLine, userAccount);
            }

            return remoteMeterLocationLine;
        }

        private void AddOrUpdateChannelsForLine(string address, MeterAsset localMeterAsset, MeterAsset remoteMeterAsset, UserAccount userAccount)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                // ensure remote and local line impedance matches
                IEnumerable<ChannelDetail> localChannels = new TableOperations<ChannelDetail>(connection).QueryRecordsWhere("MeterID = {0} AND AssetID = {1}", localMeterAsset.MeterID, localMeterAsset.AssetID);
                List<ChannelDetail> remoteChannels = WebAPIHub.GetChannels(address, $"MeterID = {remoteMeterAsset.MeterID} AND AssetID = {remoteMeterAsset.AssetID}", userAccount).ToList();

                // if there is a local record but not a remote record
                foreach (ChannelDetail localChannelDetail in localChannels)
                {
                    ChannelDetail remoteChannelDetail = remoteChannels.Find(x => x.MeasurementType == localChannelDetail.MeasurementType && x.MeasurementCharacteristic == localChannelDetail.MeasurementCharacteristic && x.Phase == localChannelDetail.Phase && x.Name == localChannelDetail.Name);
                    if (remoteChannelDetail == null)
                    {
                        remoteChannelDetail = new ChannelDetail();
                        remoteChannelDetail.MeterID = remoteMeterAsset.MeterID;
                        remoteChannelDetail.AssetID = remoteMeterAsset.AssetID;
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

        private void AddAssetConnections(string address, AssetConnection assetConnection, string connectionType, IEnumerable<AssetConnectionType> remoteAssetConnectionTypes,UserAccount userAccount) {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings")) {
                int remoteXDAAssetID1 = connection.ExecuteScalar<int>("SELECT RemoteXDAAssetID FROM AssetsToDataPush WHERE LocalXDAAssetID = {0}", assetConnection.ParentID);
                int remoteXDAAssetID2 = connection.ExecuteScalar<int>("SELECT RemoteXDAAssetID FROM AssetsToDataPush WHERE LocalXDAAssetID = {0}", assetConnection.ParentID);
                AssetConnection remoteAssetConnection = WebAPIHub.GetRecordWhere<AssetConnection>(address, $"ParentID = {remoteXDAAssetID1}  AND ChildID = {remoteXDAAssetID2}", userAccount);
                if(remoteAssetConnection == null)
                {
                    remoteAssetConnection = new AssetConnection();
                    remoteAssetConnection.ParentID = remoteXDAAssetID1;
                    remoteAssetConnection.ChildID = remoteXDAAssetID2;
                    remoteAssetConnection.AssetRelationshipTypeID = remoteAssetConnectionTypes.First(x => x.Name == connectionType).ID;

                    WebAPIHub.CreateRecord(address, remoteAssetConnection, userAccount);
                }

            }
        }


        #endregion

        #region [ File Sync Functions ]

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

                        Log.Info($"Processing Remote data push for {meterToDataPush.LocalXDAAssetKey}:Filegroup:{fileGroup.ID}...");

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
                                MeterID = meterToDataPush.RemoteXDAMeterID
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
                            MeterID = meterToDataPush.RemoteXDAMeterID
                        };
                        int remoteFileGroupId = WebAPIHub.CreateRecord(instance.Address, fg, userAccount);
                        fileGroupLocalToRemote = new FileGroupLocalToRemote()
                        {
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

        #endregion

        #endregion
    }
}