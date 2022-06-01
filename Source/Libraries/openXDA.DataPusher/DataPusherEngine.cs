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
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using GSF;
using GSF.Configuration;
using GSF.Data;
using GSF.Data.Model;
using GSF.Net.Security;
using GSF.Scheduling;
using GSF.Security.Model;
using GSF.Web;
using GSF.Web.Model;
using log4net;
using Newtonsoft.Json.Linq;
using openXDA.Model;
using CapacitorBank = openXDA.Model.CapBank;

namespace openXDA.DataPusher
{
    [Serializable]
    public class FileGroupPost
    {
        public string MeterKey { get; set; }
        public FileGroup FileGroup { get; set; }
        public List<DataFile> DataFiles { get; set; }
        public List<FileBlob> FileBlobs { get; set; }
    }

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

        public DataPusherEngine(Func<AdoDataConnection> connectionFactory)
        {
            ConnectionFactory = connectionFactory;
            using(AdoDataConnection connection = ConnectionFactory())
            {
                m_dataPusherSettings = new DataPusherSettings();
                m_dataPusherSettings.Enabled = connection.ExecuteScalar<string>("SELECT Value from Setting WHERE Name = 'DataPusher.Enabled'") == "True";
                m_dataPusherSettings.OnlyValidFaults = connection.ExecuteScalar<string>("SELECT Value from Setting WHERE Name = 'DataPusher.OnlyValidFaults'") == "True";
                m_dataPusherSettings.TimeWindow = int.Parse(connection.ExecuteScalar<string>("SELECT Value from Setting WHERE Name = 'DataPusher.TimeWindow'")?.ToString() ?? "1");

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

        private Func<AdoDataConnection> ConnectionFactory { get; }

        #endregion

        // Client-side script functionality
        #region [ Static ]

        private static readonly ILog Log = LogManager.GetLogger(typeof(DataPusherEngine));

        public static event EventHandler<EventArgs<string, string, int>> UpdateProgressForMeter;

        private static readonly string ConnectionString = "systemSetting";

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
            using (AdoDataConnection connection = ConnectionFactory())
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

                using (AdoDataConnection connection = new AdoDataConnection(ConnectionString))
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
            using (AdoDataConnection connection = new AdoDataConnection(ConnectionString))
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
            using (AdoDataConnection connection = new AdoDataConnection(ConnectionString))
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
                using (AdoDataConnection connection = new AdoDataConnection(ConnectionString))
                {
                    IEnumerable<MetersToDataPush> meters = new TableOperations<MetersToDataPush>(connection).QueryRecordsWhere("RemoteXDAInstanceID = {0}", instanceId);
                    RemoteXDAInstance instance = new TableOperations<RemoteXDAInstance>(connection).QueryRecordWhere("ID = {0}", instanceId);

                    // Meter Updates every asset push associated with the meter
                    IEnumerable<AssetsToDataPush> meterlessAssets = new TableOperations<AssetsToDataPush>(connection).QueryRecordsWhere("RemoteXDAInstanceID = {0} and LocalXDAAssetID in (Select AssetID From MeterAsset Where MeterID not in (Select LocalXDAMeterID From MetersToDataPush Where RemoteXDAInstanceID = {0}))", instance.ID);

                    // Setup Progress
                    int progressTotal = meters.Count() + meterlessAssets.Count()+ 1;
                    int progressCount = 0;
                    OnUpdateProgressForInstance(clientId, instance.Name, (int)(100 * (progressCount) / progressTotal));

                    // Do other operations needed
                    UserAccount userAccount = new TableOperations<UserAccount>(connection).QueryRecordWhere("ID = {0}", instance.UserAccountID);
                    IEnumerable<AssetTypes> localAssetTypes = new TableOperations<AssetTypes>(connection).QueryRecords();
                    IEnumerable<AssetTypes> remoteAssetTypes = WebAPIHub.GetRecords<AssetTypes>(instance.Address, "all", userAccount);
                    OnUpdateProgressForInstance(clientId, instance.Name, (int)(100 * (++progressCount) / progressTotal));
                    AssetGroup remoteAssetGroup = AddOrGetRemoteAssetGroup(instance.Address, userAccount);

                    foreach (MetersToDataPush meter in meters)
                    {
                        if (cancellationToken.IsCancellationRequested)
                            break;

                        SyncMeterConfigurationForInstance(null, instance, meter, remoteAssetGroup, localAssetTypes, remoteAssetTypes, userAccount, cancellationToken);
                        OnUpdateProgressForInstance(clientId, instance.Name, (int)(100 * (++progressCount) / progressTotal));
                    }
                    foreach (AssetsToDataPush asset in meterlessAssets)
                    {
                        if (cancellationToken.IsCancellationRequested)
                            break;
                        SyncAssetConfigurationOnlyForInstance(instance, asset, localAssetTypes, remoteAssetTypes, userAccount, cancellationToken);
                        OnUpdateProgressForInstance(clientId, instance.Name, (int)(100 * (++progressCount) / progressTotal));
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

        }

        public void SyncMeterConfigurationForInstance(string clientId, RemoteXDAInstance instance, MetersToDataPush meterToDataPush, AssetGroup remoteAssetGroup, IEnumerable<AssetTypes> localAssetTypes, IEnumerable<AssetTypes> remoteAssetTypes ,UserAccount userAccount, CancellationToken cancellationToken)
        {

            try
            {
                using (AdoDataConnection connection = new AdoDataConnection(ConnectionString))
                {
                    // Get local meter record
                    Meter localMeterRecord = new TableOperations<Meter>(connection).QueryRecordWhere("ID = {0}", meterToDataPush.LocalXDAMeterID);
                    // get local MeterLine table 
                    // Get Asset Datapushes assciated with this meter
                    IEnumerable<AssetsToDataPush> assetDataPushes = new TableOperations<AssetsToDataPush>(connection).QueryRecordsWhere("RemoteXDAInstanceID = {0} and LocalXDAAssetID in (Select AssetID From MeterAsset Where MeterID = {1})", instance.ID, localMeterRecord.ID);


                    // update progress
                    int progressTotal = assetDataPushes.Count() + 2;
                    int progressCount = 0;


                    // Add or Update remote meter location
                    Location remoteLocation = AddOrGetRemoteLocation(instance.Address, meterToDataPush, localMeterRecord, userAccount);

                    // update progress
                    OnUpdateProgressForMeter(clientId, localMeterRecord.AssetKey, (int)(100 * (progressCount) / progressTotal));

                    // if meter doesnt exist remotely add it
                    AddOrGetRemoteMeter(instance.Address, meterToDataPush, localMeterRecord, remoteLocation, userAccount);

                    // update progress
                    OnUpdateProgressForMeter(clientId, localMeterRecord.AssetKey, (int)(100 * (++progressCount) / progressTotal));

                    // If does not exist add link for meter to the appropriate asset group
                    AddMeterAssetGroup(instance.Address, remoteAssetGroup.ID, meterToDataPush.RemoteXDAMeterID, userAccount);

                    // update progress
                    OnUpdateProgressForMeter(clientId, localMeterRecord.AssetKey, (int)(100 * (++progressCount) / progressTotal));

                    // if there is a line for the meter ensure that its data has been uploaded remotely
                    foreach (AssetsToDataPush assetToDataPush in assetDataPushes)
                    {
                        if (cancellationToken.IsCancellationRequested)
                            break;

                        // Add or Update Asset
                        Asset localAsset = new TableOperations<Asset>(connection).QueryRecordWhere("ID = {0}", assetToDataPush.LocalXDAAssetID);
                        Asset remoteAseset = AddOrGetRemoteAsset(instance.Address, assetToDataPush, localAsset, localAssetTypes, remoteAssetTypes, userAccount);
                        // Note that this needs to get the Asset specifics (e.g. LineAttributes) sepperatly
                        // That broke in 3.0 

                        // if MeterLine association has not been previously made, make it
                        MeterAsset localMeterAsset = new TableOperations<MeterAsset>(connection).QueryRecordWhere("MeterID = {0} and AssetID = {1}", meterToDataPush.LocalXDAMeterID, assetToDataPush.LocalXDAAssetID);
                        MeterAsset remoteMeterAsset = AddMeterAsset(instance.Address, meterToDataPush, assetToDataPush, userAccount, localMeterAsset);

                        // add line to meterlocationline table
                        AssetLocation remoteAssetLocation = AddOrUpdateAssetLocation(instance.Address, remoteAseset, remoteLocation, userAccount);

                        // Sync Channel and channel dependant data
                        AddOrUpdateChannelsForLine(instance.Address, localMeterAsset, remoteMeterAsset, userAccount);

                        connection.ExecuteNonQuery("UPDATE AssetsToDataPush SET Synced = 1 where ID = {0}", assetToDataPush.ID);

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
        public void SyncMeterConfigurationForInstance(string clientId, RemoteXDAInstance instance, MetersToDataPush meterToDataPush, UserAccount userAccount, CancellationToken cancellationToken)
        {
            try
            {

                using (AdoDataConnection connection = new AdoDataConnection(ConnectionString))
                {
                    IEnumerable<AssetTypes> localAssetTypes = new TableOperations<AssetTypes>(connection).QueryRecords();
                    IEnumerable<AssetTypes> remoteAssetTypes = WebAPIHub.GetRecords<AssetTypes>(instance.Address, "all", userAccount);
                    AssetGroup remoteAssetGroup = AddOrGetRemoteAssetGroup(instance.Address, userAccount);
                    SyncMeterConfigurationForInstance(clientId, instance, meterToDataPush, remoteAssetGroup, localAssetTypes, remoteAssetTypes, userAccount, cancellationToken);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }
        public void SyncAssetConfigurationOnlyForInstance(RemoteXDAInstance instance, AssetsToDataPush assetToDataPush, IEnumerable<AssetTypes> localAssetTypes, IEnumerable<AssetTypes> remoteAssetTypes, UserAccount userAccount, CancellationToken cancellationToken)
        {

            try
            {
                using (AdoDataConnection connection = new AdoDataConnection(ConnectionString))
                {
                    // Add or Update Asset
                    Asset localAsset = new TableOperations<Asset>(connection).QueryRecordWhere("ID = {0}", assetToDataPush.LocalXDAAssetID);
                    Asset remoteAseset = AddOrGetRemoteAsset(instance.Address, assetToDataPush, localAsset, localAssetTypes, remoteAssetTypes, userAccount);
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
            using (AdoDataConnection connection = new AdoDataConnection(ConnectionString))
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

            using (AdoDataConnection connection = ConnectionFactory())
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

        private Asset AddOrGetRemoteAsset(string address, AssetsToDataPush assetToDataPush, Asset localAsset, IEnumerable<AssetTypes> localAssetTypes, IEnumerable<AssetTypes> remoteAssetTypes, UserAccount userAccount)
        {
            using (AdoDataConnection connection = ConnectionFactory())
            {
                Asset remoteAsset = null;

                // Todo, use gettype here later
                string assetTypeText = localAssetTypes.First(x => x.ID == localAsset.AssetTypeID).Name;
                //if the line does not exist in the PQMarkPusher Database to allow for obsfucation add it.
                if (assetTypeText == "Line")
                    remoteAsset = AddOrGetAssetAny<Line>(address, localAsset, assetToDataPush, remoteAssetTypes, userAccount);
                else if (assetTypeText == "LineSegment")
                    remoteAsset = AddOrGetAssetAny<LineSegment>(address, localAsset, assetToDataPush, remoteAssetTypes, userAccount);
                else if (assetTypeText == "Breaker")
                    remoteAsset = AddOrGetAssetAny<Breaker>(address, localAsset, assetToDataPush, remoteAssetTypes, userAccount);
                else if (assetTypeText == "Transformer")
                    remoteAsset = AddOrGetAssetAny<Transformer>(address, localAsset, assetToDataPush, remoteAssetTypes, userAccount);
                else if (assetTypeText == "Bus")
                    remoteAsset = AddOrGetAssetAny<Bus>(address, localAsset, assetToDataPush, remoteAssetTypes, userAccount);
                else if (assetTypeText == "CapacitorBank")
                    remoteAsset = AddOrGetAssetAny<CapacitorBank>(address, localAsset, assetToDataPush, remoteAssetTypes, userAccount);

                if (remoteAsset == null) throw new Exception("Asset not defined.");
                return remoteAsset;
            }
        }
        private T AddOrGetAssetAny<T>(string address, Asset localAsset, AssetsToDataPush assetToDataPush, IEnumerable<AssetTypes> remoteAssetTypes, UserAccount userAccount) where T : Asset, new()
        {
            using (AdoDataConnection connection = ConnectionFactory())
            {
                T remoteTypedAsset = null;
                T localTypedAsset = new TableOperations<T>(connection).QueryRecordWhere("AssetKey = {0}", localAsset.AssetKey);

                //if the asset does not exist in the PQMarkPusher Database to allow for obsfucation add it.
                remoteTypedAsset = WebAPIHub.GetRecordWhere<T>(address, $"ID={assetToDataPush.RemoteXDAAssetID}", userAccount);

                if (remoteTypedAsset == null)
                {
                    remoteTypedAsset = localTypedAsset;
                    if (assetToDataPush.Obsfucate)
                    {
                        remoteTypedAsset.AssetKey = assetToDataPush.RemoteXDAAssetKey;
                        remoteTypedAsset.AssetName = assetToDataPush.RemoteXDAAssetKey;
                        remoteTypedAsset.Description = "";
                    }

                    remoteTypedAsset.ID = 0;
                    remoteTypedAsset.AssetTypeID = remoteAssetTypes.First(x => x.Name == typeof(T).Name).ID;
                    remoteTypedAsset.ID = WebAPIHub.CreateRecord(address, remoteTypedAsset, userAccount);
                    remoteTypedAsset.ID = WebAPIHub.CreateRecord<Asset>(address, remoteTypedAsset, userAccount);

                    assetToDataPush.RemoteXDAAssetID = remoteTypedAsset.ID;
                    assetToDataPush.RemoteXDAAssetKey = remoteTypedAsset.AssetKey;
                    assetToDataPush.RemoteAssetCreatedByDataPusher = true;

                    new TableOperations<AssetsToDataPush>(connection).UpdateRecord(assetToDataPush);
                }
                else if (assetToDataPush.RemoteAssetCreatedByDataPusher)
                {
                    remoteTypedAsset.AssetKey = (assetToDataPush.Obsfucate ? assetToDataPush.RemoteXDAAssetKey.ToString() : localAsset.AssetKey);
                    remoteTypedAsset.AssetName = (assetToDataPush.Obsfucate ? assetToDataPush.RemoteXDAAssetKey.ToString() : localAsset.AssetName);
                    remoteTypedAsset.VoltageKV = localAsset.VoltageKV;
                    remoteTypedAsset.AssetTypeID = localAsset.AssetTypeID;
                    remoteTypedAsset.Spare = localAsset.Spare;
                    remoteTypedAsset.Description = (assetToDataPush.Obsfucate ? "" : localAsset.Description);
                    WebAPIHub.UpdateRecord(address, remoteTypedAsset, userAccount);
                }

                if (remoteTypedAsset == null) throw new Exception("Asset not defined.");
                return remoteTypedAsset;
            }
        }

        private MeterAsset AddMeterAsset(string address, MetersToDataPush meter, AssetsToDataPush asset, UserAccount userAccount, MeterAsset localMeterLine)
        {
            MeterAsset remoteMeterLine = WebAPIHub.GetRecordWhere<MeterAsset>(address, $"MeterID = {meter.RemoteXDAMeterID} AND AssetID = {asset.RemoteXDAAssetID}", userAccount);

            // if MeterLine association has not been previously made, make it
            if (remoteMeterLine == null)
            {
                remoteMeterLine = new MeterAsset()
                {
                    MeterID = meter.RemoteXDAMeterID,
                    AssetID = asset.RemoteXDAAssetID,
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
            using (AdoDataConnection connection = ConnectionFactory())
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
            using (AdoDataConnection connection = ConnectionFactory())
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
            using (AdoDataConnection connection = ConnectionFactory())
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
            using (AdoDataConnection connection = ConnectionFactory()) {
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

        public bool TestInstance(int instanceId)
        {
            bool connectionSuccess = false;
            try
            {
                using (AdoDataConnection connection = ConnectionFactory())
                {
                    RemoteXDAInstance instance = new TableOperations<RemoteXDAInstance>(connection).QueryRecordWhere("ID = {0}", instanceId);
                    if (instance is null) throw new Exception($"No remote XDA instance found with this instance ID${instanceId}");
                    UserAccount userAccount = new TableOperations<UserAccount>(connection).QueryRecordWhere("ID = {0}", instance.UserAccountID);
                    connectionSuccess = WebAPIHub.TestConnection(instance.Address, userAccount);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return connectionSuccess;
        }


        #endregion

        #region [ File Sync Functions ]

        public void SyncInstanceFiles(string clientId, RemoteXDAInstance instance, CancellationToken cancellationToken)
        {
            try
            {
                using (AdoDataConnection connection = ConnectionFactory())
                {

                    IEnumerable<int> meters = new TableOperations<MetersToDataPush>(connection).QueryRecordsWhere("RemoteXDAInstanceID = {0}", instance.ID).Select(x => x.ID);
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
                
                using (AdoDataConnection connection = ConnectionFactory())
                {

                    IEnumerable<FileGroup> localFileGroups;
                    UserAccount userAccount = new TableOperations<UserAccount>(connection).QueryRecordWhere("ID = {0}", instance.UserAccountID);

                    DateTime timeWindowStartDate = DateTime.UtcNow.AddHours(DataPusherSettings.TimeWindow * -1);

                    MetersToDataPush meterToDataPush = new TableOperations<MetersToDataPush>(connection).QueryRecordWhere("ID = {0}", meterId);

                    string localAssetKey = new TableOperations<Meter>(connection).QueryRecordWhere("ID={0}", meterToDataPush.LocalXDAMeterID).AssetKey;

                    if (DataPusherSettings.TimeWindow != 0)
                        localFileGroups = new TableOperations<FileGroup>(connection).QueryRecordsWhere("ID IN (SELECT FileGroupID From Event WHERE MeterID = {0} AND StartTime >= {1})", meterToDataPush.LocalXDAMeterID, timeWindowStartDate);
                    else
                        localFileGroups = new TableOperations<FileGroup>(connection).QueryRecordsWhere("ID IN (SELECT FileGroupID From Event WHERE MeterID = {0})", meterToDataPush.LocalXDAMeterID);

                    int progressTotal = (localFileGroups.Count() > 0 ? localFileGroups.Count() : 1);
                    int progressCount = 0;

                    OnUpdateProgressForMeter(clientId, localAssetKey, (int)(100 * (progressCount) / progressTotal));
                    Log.Info($"Processing Remote data push for {localAssetKey}...");

                    foreach (FileGroup fileGroup in localFileGroups)
                    {
                        if (cancellationToken.IsCancellationRequested) break;

                        Log.Info($"Processing Remote data push for {localAssetKey}:Filegroup:{fileGroup.ID}...");

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

                        OnUpdateProgressForMeter(clientId, localAssetKey, (int)(100 * (++progressCount) / progressTotal));
                        Log.Info($"Processing Remote data push for {localAssetKey}: Completed Filegroup{fileGroup.ID}: Progress: { (int)(100 * (progressCount) / progressTotal)}");

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
                using (AdoDataConnection connection = ConnectionFactory())
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


        public void SendFiles(int instanceId, int meterId, int fileGroupID)
        {
            using (AdoDataConnection connection = ConnectionFactory())
            {

                //// for now, create new instance of DataPusherEngine.  Later have one running in XDA ServiceHost and tie to it to ensure multiple updates arent happening simultaneously
                RemoteXDAInstance instance = new TableOperations<RemoteXDAInstance>(connection).QueryRecordWhere("ID = {0}", instanceId);
                MetersToDataPush meter = new TableOperations<MetersToDataPush>(connection).QueryRecordWhere("ID = {0}", meterId);
                UserAccount userAccount = new TableOperations<UserAccount>(connection).QueryRecordWhere("ID = {0}", instance.UserAccountID);
                FileGroup fileGroup = new TableOperations<FileGroup>(connection).QueryRecordWhere("ID = {0}", fileGroupID);
                List<DataFile> files = new TableOperations<DataFile>(connection).QueryRecordsWhere("FileGroupID = {0}", fileGroupID).ToList();
                List<FileBlob> fileBlobs = new TableOperations<FileBlob>(connection).QueryRecordsWhere("DataFileID IN (SELECT ID FROM DataFile WHERE FileGroupID = {0})", fileGroupID).ToList();
                FileGroupPost post = new FileGroupPost();
                post.MeterKey = meter.RemoteXDAAssetKey;
                post.FileGroup = fileGroup;
                post.DataFiles = files;
                post.FileBlobs = fileBlobs;

                int id = SendFiles(post, instance.Address, userAccount).Result;
                connection.ExecuteNonQuery("INSERT INTO FileGroupLocalToRemote (RemoteXDAInstanceID, LocalFileGroupID, remoteFileGroupID) VALUES ({0},{1},{2}) ", instanceId, fileGroupID, id);

            }

        }

        public Task<int> SendFiles(FileGroupPost post, string address, UserAccount userAccount)
        {
            return Task.Run(() =>
            {
                string antiForgeryToken = GenerateAntiForgeryToken(address, userAccount);

                MemoryStream stream = new MemoryStream();
                BinaryFormatter binaryFormater = new BinaryFormatter();
                binaryFormater.Serialize(stream, post);

                stream.Seek(0, SeekOrigin.Begin);
                using (WebRequestHandler handler = new WebRequestHandler())
                using (HttpClient client = new HttpClient(handler))
                {
                    handler.ServerCertificateValidationCallback += HandleCertificateValidation;

                    client.BaseAddress = new Uri(address);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/text"));
                    client.DefaultRequestHeaders.Add("X-GSF-Verify", antiForgeryToken);
                    client.AddBasicAuthenticationHeader(userAccount.AccountName, userAccount.Password);

                    HttpContent httpContent = new StreamContent(stream);
                    HttpResponseMessage response = client.PostAsync($"api/DataPusher/Recieve/Files", httpContent).Result;
                    string remoteFGID = response.Content.ReadAsStringAsync().Result;
                    remoteFGID = remoteFGID.Replace("\"", "");
                    int id = int.Parse(remoteFGID);
                    if (!response.IsSuccessStatusCode)
                        throw new InvalidOperationException($"Server returned status code {response.StatusCode}: {response.ReasonPhrase}");

                    return id;
                }
            });
        }


        #endregion

        #region [ Helpers ]
        public bool HandleCertificateValidation(Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            SimpleCertificateChecker simpleCertificateChecker = new SimpleCertificateChecker();

            CategorizedSettingsElementCollection systemSettings = ConfigurationFile.Current.Settings["systemSettings"];
            systemSettings.Add("CertFile", "", "This is a certfile.");
            systemSettings.Add("ValidPolicyErrors", "None", "Password for OpenXDA Web API access.");
            systemSettings.Add("ValidChainFlags", "NoError", "Password for OpenXDA Web API access.");

            try
            {
                simpleCertificateChecker.ValidPolicyErrors = (SslPolicyErrors)Enum.Parse(typeof(SslPolicyErrors), (systemSettings["ValidPolicyErrors"].Value != "All" ? systemSettings["ValidPolicyErrors"].Value : "7"));
                simpleCertificateChecker.ValidChainFlags = (X509ChainStatusFlags)Enum.Parse(typeof(X509ChainStatusFlags), (systemSettings["ValidChainFlags"].Value != "All" ? systemSettings["ValidChainFlags"].Value : (~0).ToString()));
                simpleCertificateChecker.TrustedCertificates.Add((!string.IsNullOrEmpty(systemSettings["CertFile"].Value) ? new X509Certificate2(systemSettings["CertFile"].Value) : certificate));
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }

            return simpleCertificateChecker.ValidateRemoteCertificate(sender, certificate, chain, sslPolicyErrors);
        }

        public string GenerateAntiForgeryToken(string instance, UserAccount userAccount)
        {
            using (WebRequestHandler handler = new WebRequestHandler())
            using (HttpClient client = new HttpClient(handler))
            {
                handler.ServerCertificateValidationCallback += HandleCertificateValidation;

                client.BaseAddress = new Uri(instance);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.AddBasicAuthenticationHeader(userAccount.AccountName, userAccount.Password);

                HttpResponseMessage response = client.GetAsync("api/rvht").Result;

                if (!response.IsSuccessStatusCode)
                    throw new InvalidOperationException($"Server returned status code {response.StatusCode}: {response.ReasonPhrase}");

                return response.Content.ReadAsStringAsync().Result;
            }
        }


        #endregion
        #endregion
    }
}