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
using System.Reflection;
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
using log4net;
using Newtonsoft.Json.Linq;
using openXDA.Model;

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

        private class Settings
        {
            public Settings(Action<object> configure) =>
                configure(this);

            [Category]
            [SettingName(DataPusherSettings.CategoryName)]
            public DataPusherSettings DataPusherSettings { get; } = new DataPusherSettings();
        }


        #endregion

        #region [ Constructors ]

        public DataPusherEngine(Func<AdoDataConnection> connectionFactory)
        {
            ConnectionFactory = connectionFactory;
        }

        #endregion

        #region [ Properties ]
        private ScheduleManager Scheduler => m_scheduler ?? (m_scheduler = new ScheduleManager());
        public bool Running => m_running;

        private Func<AdoDataConnection> ConnectionFactory { get; }

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

        public static event EventHandler<EventArgs<string, string, int>> UpdateProgressForAssets;

        private static void OnUpdateProgressForAssets(string client, string asset, int update)
        {
            if (client != string.Empty)
                UpdateProgressForAssets?.Invoke(new object(), new EventArgs<string, string, int>(client, asset, update));
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

                using (AdoDataConnection connection = ConnectionFactory())
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
            using (AdoDataConnection connection = ConnectionFactory())
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
            using (AdoDataConnection connection = ConnectionFactory())
            {
                Log.Info(string.Format("DataPusher Scheduler: {0} schedule is due...", e.Argument.Name));
                RemoteXDAInstance instance = new TableOperations<RemoteXDAInstance>(connection).QueryRecordWhere("Name = {0}", e.Argument.Name);
                SyncInstanceFiles(string.Empty, instance, new CancellationToken());
            }
        }

        protected Action<object> GetConfigurator()
        {
            ConfigurationLoader configurationLoader = new ConfigurationLoader(ConnectionFactory);
            return configurationLoader.Configure;
        }

        #endregion

        #region [ Configuration Sync Functions ]
        public void SyncInstanceConfiguration(string clientId, int instanceId, CancellationToken cancellationToken)
        {
            try
            {
                using (AdoDataConnection connection = ConnectionFactory())
                {
                    IEnumerable<MetersToDataPush> meters = new TableOperations<MetersToDataPush>(connection).QueryRecordsWhere("RemoteXDAInstanceID = {0}", instanceId);
                    IEnumerable<AssetsToDataPush> assets = new TableOperations<AssetsToDataPush>(connection).QueryRecordsWhere("RemoteXDAInstanceID = {0}", instanceId);
                    RemoteXDAInstance instance = new TableOperations<RemoteXDAInstance>(connection).QueryRecordWhere("ID = {0}", instanceId);


                    // Setup Progress
                    int progressTotal = 3 + meters.Count();
                    int progressCount = 0;
                    OnUpdateProgressForInstance(clientId, instance.Name, (int)(100 * (progressCount) / progressTotal));

                    // Do other operations needed
                    DatapusherRequester requester = new DatapusherRequester(instance, connection);

                    List<Location> remoteLocations = new List<Location>();
                    IEnumerable<AssetTypes> localAssetTypes = new TableOperations<AssetTypes>(connection).QueryRecords();

                    IEnumerable<AssetTypes> remoteAssetTypes = WebAPIHub.GetRecords<AssetTypes>("all", requester);
                    OnUpdateProgressForInstance(clientId, instance.Name, (int)(100 * (++progressCount) / progressTotal));

                    AssetGroup remoteAssetGroup = AddOrGetRemoteAssetGroup(requester);
                    OnUpdateProgressForInstance(clientId, instance.Name, (int)(100 * (++progressCount) / progressTotal));

                    foreach (MetersToDataPush meter in meters)
                    {
                        if (cancellationToken.IsCancellationRequested)
                            break;

                        // Locations are attached to meters on remote pushes, therefore we only have locations if there is a meter for them
                        remoteLocations.Add(SyncMeterConfigurationForInstance(clientId, meter, remoteAssetGroup, requester, cancellationToken));
                        OnUpdateProgressForInstance(clientId, instance.Name, (int)(100 * (++progressCount) / progressTotal));
                    }

                    SyncAssetConfigurationForInstance(clientId, instance, assets, localAssetTypes, remoteAssetTypes, remoteLocations, requester, cancellationToken);
                    OnUpdateProgressForInstance(clientId, instance.Name, (int)(100 * (++progressCount) / progressTotal));
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        public Location SyncMeterConfigurationForInstance(string clientId, MetersToDataPush meterToDataPush, AssetGroup remoteAssetGroup, DatapusherRequester requester, CancellationToken cancellationToken)
        {
            try
            {
                using (AdoDataConnection connection = ConnectionFactory())
                {
                    // Get local meter record
                    Meter localMeterRecord = new TableOperations<Meter>(connection).QueryRecordWhere("ID = {0}", meterToDataPush.LocalXDAMeterID);

                    // update progress
                    int progressTotal = 3;
                    int progressCount = 0;

                    // Add or Update remote meter location
                    Location remoteLocation = AddOrGetRemoteLocation(meterToDataPush, localMeterRecord, requester);

                    // update progress
                    OnUpdateProgressForMeter(clientId, localMeterRecord.AssetKey, (int)(100 * (++progressCount) / progressTotal));

                    // if meter doesnt exist remotely add it
                    AddOrGetRemoteMeter(meterToDataPush, localMeterRecord, remoteLocation, requester);

                    // update progress
                    OnUpdateProgressForMeter(clientId, localMeterRecord.AssetKey, (int)(100 * (++progressCount) / progressTotal));

                    // If does not exist add link for meter to the appropriate asset group
                    AddMeterAssetGroup(remoteAssetGroup.ID, meterToDataPush.RemoteXDAMeterID, requester);

                    // update progress
                    OnUpdateProgressForMeter(clientId, localMeterRecord.AssetKey, (int)(100 * (++progressCount) / progressTotal));

                    connection.ExecuteNonQuery("UPDATE MetersToDataPush SET Synced = 1 WHERE ID = {0}", meterToDataPush.ID);

                    return remoteLocation;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return null;
            }
        }

        public void SyncMeterConfigurationForInstance(string clientId, RemoteXDAInstance instance, MetersToDataPush meterToDataPush, DatapusherRequester requester, CancellationToken cancellationToken)
        {
            try
            {
                using (AdoDataConnection connection = ConnectionFactory())
                {
                    IEnumerable<AssetTypes> localAssetTypes = new TableOperations<AssetTypes>(connection).QueryRecords();
                    IEnumerable<AssetTypes> remoteAssetTypes = WebAPIHub.GetRecords<AssetTypes>("all", requester);
                    AssetGroup remoteAssetGroup = AddOrGetRemoteAssetGroup(requester);
                    IEnumerable<Location> remoteLocation = Enumerable.Empty<Location>().Append(SyncMeterConfigurationForInstance(clientId, meterToDataPush, remoteAssetGroup, requester, cancellationToken));

                    // Meter Updates every asset push associated with the meter
                    IEnumerable<AssetsToDataPush> assets = new TableOperations<AssetsToDataPush>(connection).
                        QueryRecordsWhere(@"
                        RemoteXDAInstanceID = {0} AND
                        LocalXDAAssetID IN (Select AssetID From MeterAsset Where MeterID = {1}) ", instance.ID, meterToDataPush.LocalXDAMeterID);

                    SyncAssetConfigurationForInstance(clientId, instance, assets, localAssetTypes, remoteAssetTypes, remoteLocation, requester, cancellationToken);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        public void SyncAssetConfigurationForInstance(
            string clientId, RemoteXDAInstance instance, IEnumerable<AssetsToDataPush> assets,
            IEnumerable<AssetTypes> localAssetTypes, IEnumerable<AssetTypes> remoteAssetTypes, IEnumerable<Location> remoteLocations,
            DatapusherRequester requester, CancellationToken cancellationToken)
        {
            try
            {
                using (AdoDataConnection connection = ConnectionFactory())
                {
                    // By default, line segments are not associated with meters, so they often get overlooked. Decision was made to always add them if the associated line is here
                    TableOperations<LineSegment> lineSegmentTable = new TableOperations<LineSegment>(connection);
                    TableOperations<AssetsToDataPush> assetPushTable = new TableOperations<AssetsToDataPush>(connection);
                    List<AssetsToDataPush> lineSegments = new TableOperations<Line>(connection).
                        QueryRecordsWhere("ID in (SELECT LocalXDAAssetID FROM AssetsToDataPush WHERE RemoteXDAInstanceID = {0})", instance.ID).
                        SelectMany(line =>
                        {
                            AssetsToDataPush lineAssetPush = assets.Where(asset => asset.LocalXDAAssetID == line.ID).First();
                            IEnumerable<AssetsToDataPush> newPushes = lineSegmentTable.QueryRecordsWhere(@"
                                ID in
                                    (SELECT ParentID as AssetID FROM AssetRelationship WHERE ChildID = {0} UNION
                                    SELECT ChildID as AssetID FROM AssetRelationship WHERE ParentID = {0})
                                AND ID not in 
                                    (SELECT LocalXDAAssetID FROM AssetsToDataPush WHERE RemoteXDAInstanceID = {1})", line.ID, instance.ID).
                                Select((segment, index) => {
                                    AssetsToDataPush lineSegment = new AssetsToDataPush()
                            {
                                ID = -1,
                                RemoteXDAInstanceID = instance.ID,
                                LocalXDAAssetID = segment.ID,
                                RemoteXDAAssetID = -1,
                                RemoteXDAAssetKey = $"{lineAssetPush.RemoteXDAAssetKey}-S{index}",
                                Obsfucate = lineAssetPush.Obsfucate,
                                Synced = false,
                                RemoteAssetCreatedByDataPusher = false
                                    };
                                    assetPushTable.AddNewRecord(lineSegment);
                                    lineSegment.ID = connection.ExecuteScalar<int>("SELECT @@IDENTITY");
                                    return lineSegment;
                            });
                            return newPushes;
                        }).ToList();

                    IEnumerable<AssetsToDataPush> assetsToDataPushes = assets.Concat(lineSegments);

                    TableOperations<MetersToDataPush> meterPushTable = new TableOperations<MetersToDataPush>(connection);
                    IEnumerable<MetersToDataPush> meterDataPushes = meterPushTable.
                        QueryRecordsWhere("SELECT LocalXDAMeterID FROM MetersToDataPush WHERE RemoteXDAInstanceID = {0} AND Synced = 1)", instance.ID);

                    // update progress
                    int stepsPerAsset = 3;
                    int progressTotal = assetsToDataPushes.Count() * stepsPerAsset + 2;
                    int progressCount = 0;

                    // if there is a line for the meter ensure that its data has been uploaded remotely
                    foreach (AssetsToDataPush assetToDataPush in assetsToDataPushes)
                    {
                        if (cancellationToken.IsCancellationRequested)
                            break;

                        // Add or Update Asset
                        Asset localAsset = new TableOperations<Asset>(connection).QueryRecordWhere("ID = {0}", assetToDataPush.LocalXDAAssetID);
                        // Is null if this fails, error logged in func
                        Asset remoteAsset = AddOrGetRemoteAsset(assetToDataPush, localAsset, localAssetTypes, remoteAssetTypes, requester);

                        // update progress
                        OnUpdateProgressForAssets(clientId, instance.Name + " Assets", (int)(100 * (++progressCount) / progressTotal));

                        if (remoteAsset is null)
                        {
                            progressCount += (stepsPerAsset - 1);
                            OnUpdateProgressForAssets(clientId, instance.Name + " Assets", (int)(100 * (progressCount) / progressTotal));
                            continue;
                        }

                        // if MeterAsset association has not been previously made, make it
                        IEnumerable<MeterAsset> localMeterAssets = new TableOperations<MeterAsset>(connection).
                            QueryRecordsWhere(@"
                                AssetID = {0} AND 
                                MeterID in (SELECT LocalXDAMeterID FROM MetersToDataPush WHERE RemoteXDAInstanceID = {1} AND Synced = 1)", assetToDataPush.LocalXDAAssetID, assetToDataPush.RemoteXDAInstanceID);
                        foreach(MeterAsset meterAsset in localMeterAssets)
                        {
                            IEnumerable<MetersToDataPush> meterToDataPush = meterDataPushes.Where(meter => meter.LocalXDAMeterID == meterAsset.MeterID);
                            if (!meterToDataPush.Any()) continue;

                            MeterAsset remoteMeterAsset = AddMeterAsset(meterToDataPush.First(), assetToDataPush, requester, meterAsset);
                            // Sync Channel and channel dependant data
                            AddOrUpdateChannelsForLine(meterAsset, remoteMeterAsset, requester);
                        }
                        OnUpdateProgressForAssets(clientId, instance.Name + " Assets", (int)(100 * (++progressCount) / progressTotal));

                        // if AssetLocation association has not been previously made, make it
                        IEnumerable<AssetLocation> localAssetLocations = new TableOperations<AssetLocation>(connection).
                            QueryRecordsWhere(@"
                                AssetID = {0} AND 
                                LocationID in (
                                    SELECT LocationID
                                    FROM Meter
                                    WHERE MeterID IN (SELECT LocalXDAMeterID FROM MetersToDataPush WHERE RemoteXDAInstanceID = {1} AND Synced = 1)
                                )", assetToDataPush.LocalXDAAssetID, assetToDataPush.RemoteXDAInstanceID);
                        foreach (AssetLocation assetLocation in localAssetLocations)
                        {
                            IEnumerable<MetersToDataPush> meterToDataPush = meterPushTable.
                                QueryRecordsWhere(@"
                                    LocalXDAMeterID in (
                                        SELECT ID 
                                        FROM Meter
                                        WHERE LocationID = {0} AND ID in (
                                            SELECT LocalXDAMeterID
                                            FROM MetersToDataPush
                                            Where RemoteXDAInstanceID = {1}
                                        )
                                    )", assetLocation.ID, assetToDataPush.RemoteXDAAssetKey);
                            if (!meterToDataPush.Any()) continue;

                            IEnumerable<Location> remoteLocation = remoteLocations.Where(remote => meterPushMatches.Where(meterPush => meterPush.RemoteXDAAssetKey == remote.LocationKey).Any());
                            if (!remoteLocation.Any()) continue;

                            AddOrUpdateAssetLocation(remoteAsset, remoteLocation.First(), requester);
                        }
                        OnUpdateProgressForAssets(clientId, instance.Name + " Assets", (int)(100 * (++progressCount) / progressTotal));


                        connection.ExecuteNonQuery("UPDATE AssetsToDataPush SET Synced = 1 where ID = {0}", assetToDataPush.ID);
                    }

                    IEnumerable<AssetConnection> localAssetConnections = new TableOperations<AssetConnection>(connection).QueryRecordsWhere(@"
                        ParentID IN (SELECT LocalXDAAssetID FROM AssetsToDataPush WHERE RemoteXDAInstanceID = {0})
                        AND ChildID IN (SELECT LocalXDAAssetID FROM AssetsToDataPush WHERE RemoteXDAInstanceID = {0})",
                        instance.ID);
                    IEnumerable<AssetConnectionType> localAssetConnectionTypes = new TableOperations<AssetConnectionType>(connection).QueryRecords();
                    IEnumerable<AssetConnectionType> remoteAssetConnectionTypes = WebAPIHub.GetRecords<AssetConnectionType>("all", requester);
                    OnUpdateProgressForAssets(clientId, instance.Name + " Assets", (int)(100 * (++progressCount) / progressTotal));

                    foreach (AssetConnection localAssetConnection in localAssetConnections)
                    {
                        AddAssetConnections(localAssetConnection, localAssetConnectionTypes.First(x => x.ID == localAssetConnection.ID).Name, remoteAssetConnectionTypes, requester);
                    }
                    OnUpdateProgressForAssets(clientId, instance.Name + " Assets", (int)(100 * (++progressCount) / progressTotal));
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        private AssetGroup AddOrGetRemoteAssetGroup(DatapusherRequester requester)
        {
            AssetGroup remoteAssetGroup = WebAPIHub.GetRecordWhere<AssetGroup>($"Name = '{WebAPIHub.CompanyName}'", requester);
            // if the company meter location does not exist, create it
            if (remoteAssetGroup == null)
            {
                remoteAssetGroup = new AssetGroup()
                {
                    Name = WebAPIHub.CompanyName
                };

                remoteAssetGroup.ID = WebAPIHub.CreateRecord<AssetGroup>(remoteAssetGroup, requester);
            }

            // There is nothing to update so just return group if found
            return remoteAssetGroup;
        }

        private Location AddOrGetRemoteLocation(MetersToDataPush meterToDataPush, Meter localMeterRecord, DatapusherRequester requester)
        {
            using (AdoDataConnection connection = ConnectionFactory())
            {

                Location localLocation = new TableOperations<Location>(connection).QueryRecordWhere("ID = {0}", localMeterRecord.LocationID);
                Location remoteLocation = WebAPIHub.GetRecordWhere<Location>($"LocationKey='{meterToDataPush.RemoteXDAAssetKey}'", requester);

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
                    remoteLocation.ID = WebAPIHub.CreateRecord(remoteLocation, requester);

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

                    WebAPIHub.UpdateRecord(remoteLocation, requester);
                }

                return remoteLocation;
            }
        }

        private Meter AddOrGetRemoteMeter(MetersToDataPush meter, Meter localMeterRecord, Location remoteMeterLocation, DatapusherRequester requester)
        {
            Meter remoteMeter = WebAPIHub.GetRecordWhere<Meter>($"ID={meter.RemoteXDAMeterID}", requester);

            if (remoteMeter == null)
            {
                remoteMeter = WebAPIHub.GetRecordWhere<Meter>($"AssetKey='{meter.RemoteXDAAssetKey}'", requester);
                if (remoteMeter != null)
                    throw new Exception($"A meter with this Asset Key ({meter.RemoteXDAAssetKey}) already exists.  Please update the Remote Asset Key field in the data pusher.");
            }

            // if meter doesnt exist remotely create the meter record
            if (remoteMeter == null)
            {

                if (meter.Obsfucate)
                {
                    string newShortName;
                    if (meter.RemoteXDAName.Length > 20)
                        newShortName = meter.RemoteXDAName.Substring(0, 20);
                    else
                        newShortName = meter.RemoteXDAName;
                        
                    remoteMeter = new Meter()
                    {
                        Alias = meter.RemoteXDAName,
                        ShortName = newShortName,
                        Name = meter.RemoteXDAName
                    };

                }
                else
                {
                    remoteMeter = new Meter()
                    {
                        Alias = localMeterRecord.Alias,
                        ShortName = localMeterRecord.ShortName,
                        Name = localMeterRecord.Name
                    };
                }
                remoteMeter.AssetKey = meter.RemoteXDAAssetKey.ToString();
                remoteMeter.LocationID = remoteMeterLocation.ID;
                remoteMeter.Make = localMeterRecord.Make;
                remoteMeter.Model = localMeterRecord.Model;
                remoteMeter.Description = localMeterRecord.Description;
                remoteMeter.TimeZone = localMeterRecord.TimeZone;

                remoteMeter.ID = meter.RemoteXDAMeterID = WebAPIHub.CreateRecord(remoteMeter, requester);
            }
            else
            {
                if (meter.Obsfucate)
                {
                    if (meter.RemoteXDAName.Length > 20)
                        remoteMeter.ShortName = meter.RemoteXDAName.Substring(0, 20);
                    else
                        remoteMeter.ShortName = meter.RemoteXDAName;
                    remoteMeter.Alias = meter.RemoteXDAName;
                    remoteMeter.Name = meter.RemoteXDAName;
                }
                else
                {
                    remoteMeter.Alias = localMeterRecord.Alias;
                    remoteMeter.ShortName = localMeterRecord.ShortName;
                    remoteMeter.Name = localMeterRecord.Name;
                }

                remoteMeter.AssetKey = meter.RemoteXDAAssetKey.ToString();
                remoteMeter.LocationID = remoteMeterLocation.ID;
                remoteMeter.Make = localMeterRecord.Make;
                remoteMeter.Model = localMeterRecord.Model;
                remoteMeter.Description = localMeterRecord.Description;
                remoteMeter.TimeZone = localMeterRecord.TimeZone;

                WebAPIHub.UpdateRecord<Meter>(remoteMeter, requester);
                meter.RemoteXDAMeterID = remoteMeter.ID;

            }

            using (AdoDataConnection connection = ConnectionFactory())
            {
                new TableOperations<MetersToDataPush>(connection).UpdateRecord(meter);
            }


            return remoteMeter;
        }

        private void AddMeterAssetGroup(int assetGroupId, int meterId, DatapusherRequester requester)
        {
            MeterAssetGroup remoteMeterAssetGroup = WebAPIHub.GetRecordWhere<MeterAssetGroup>($"MeterID = {meterId} AND AssetGroupID = {assetGroupId}", requester);

            // if MeterLine association has not been previously made, make it
            if (remoteMeterAssetGroup == null)
            {
                remoteMeterAssetGroup = new MeterAssetGroup()
                {
                    MeterID = meterId,
                    AssetGroupID = assetGroupId
                };

                WebAPIHub.CreateRecord(remoteMeterAssetGroup, requester);
            }
        }

        private Asset AddOrGetRemoteAsset(AssetsToDataPush assetToDataPush, Asset localAsset, IEnumerable<AssetTypes> localAssetTypes, IEnumerable<AssetTypes> remoteAssetTypes, DatapusherRequester requester)
        {
            try
            {
                using (AdoDataConnection connection = ConnectionFactory())
                {
                    Asset remoteAsset = null;

                    string assetTypeText = localAssetTypes.First(x => x.ID == localAsset.AssetTypeID).Name;
                    Type type;
                    // Check is needed since there is a disconnect between the typename and the model name for capbanks
                    if (assetTypeText == "CapacitorBankRelay")
                        type = typeof(CapBankRelay);
                    else if (assetTypeText == "CapacitorBank")
                        type = typeof(CapBank);
                    else
                        type = typeof(Asset).Assembly.GetType("openXDA.Model." + assetTypeText);

                    if (type is null || (type != typeof(Asset) && !type.IsSubclassOf(typeof(Asset))))
                        throw new Exception($"Non-valid asset type {assetTypeText} defined for asset with asset key {localAsset.AssetKey}.");
                    var assetAnyMethod = typeof(DataPusherEngine).GetMethod("AddOrGetAssetAny", BindingFlags.Instance | BindingFlags.NonPublic);
                    var assetTypedMethod = assetAnyMethod.MakeGenericMethod(new[] { type });

                    remoteAsset = (Asset)assetTypedMethod.Invoke(this, new object[] { assetTypeText, localAsset, assetToDataPush, remoteAssetTypes, requester });
                    return remoteAsset;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return null;
            }
        }

        private Asset AddOrGetAssetAny<T>(string assetTypeText, Asset localAsset, AssetsToDataPush assetToDataPush, IEnumerable<AssetTypes> remoteAssetTypes, DatapusherRequester requester) where T : Asset, new()
        {
            using (AdoDataConnection connection = ConnectionFactory())
            {
                T remoteTypedAsset = null;
                T localTypedAsset = new TableOperations<T>(connection).QueryRecordWhere("AssetKey = {0}", localAsset.AssetKey);

                // Case where the typed asset doesn't exist locally, just do regular asset
                if (localTypedAsset is null)
                    if (typeof(Asset) == typeof(T)) throw new NullReferenceException($"Unable to resolve local asset with key {localAsset.AssetKey}");
                    else return AddOrGetAssetAny<Asset>(assetTypeText, localAsset, assetToDataPush, remoteAssetTypes, requester);

                //if the asset does not exist in the PQMarkPusher Database to allow for obsfucation add it.
                remoteTypedAsset = WebAPIHub.GetRecordWhere<T>($"ID={assetToDataPush.RemoteXDAAssetID}", requester);

                if (remoteTypedAsset is null)
                {
                    Asset checkKey = WebAPIHub.GetRecordWhere<Asset>($"AssetKey='{(assetToDataPush.Obsfucate ? assetToDataPush.RemoteXDAAssetKey : localTypedAsset.AssetKey)}'", requester);
                    if (!(checkKey is null))
                        throw new Exception($"An asset with this Asset Key ({(assetToDataPush.Obsfucate ? assetToDataPush.RemoteXDAAssetKey : localTypedAsset.AssetKey)}) already exists.  Please update the Remote Asset Key field in the data pusher.");
                }

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
                    remoteTypedAsset.AssetTypeID = remoteAssetTypes.First(x => x.Name == assetTypeText).ID; //Can't find based on T since T might be different than type text
                    remoteTypedAsset.ID = WebAPIHub.CreateRecord<T>(remoteTypedAsset, requester);

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
                    WebAPIHub.UpdateRecord(remoteTypedAsset, requester);
                }

                if (remoteTypedAsset == null) throw new Exception("Asset not defined.");
                return remoteTypedAsset;
            }
        }

        private MeterAsset AddMeterAsset(MetersToDataPush meter, AssetsToDataPush asset, DatapusherRequester requester, MeterAsset localMeterLine)
        {
            MeterAsset remoteMeterLine = WebAPIHub.GetRecordWhere<MeterAsset>($"MeterID = {meter.RemoteXDAMeterID} AND AssetID = {asset.RemoteXDAAssetID}", requester);

            // if MeterLine association has not been previously made, make it
            if (remoteMeterLine == null)
            {
                remoteMeterLine = new MeterAsset()
                {
                    MeterID = meter.RemoteXDAMeterID,
                    AssetID = asset.RemoteXDAAssetID,
                };

                remoteMeterLine.ID = WebAPIHub.CreateRecord(remoteMeterLine, requester);
            }
            else
            {
                WebAPIHub.UpdateRecord(remoteMeterLine, requester);
            }

            AddFaultDetectionLogic(localMeterLine, remoteMeterLine, requester);

            return remoteMeterLine;
        }

        private void AddFaultDetectionLogic(MeterAsset localMeterLine, MeterAsset remoteMeterLine, DatapusherRequester requester)
        {
            using (AdoDataConnection connection = ConnectionFactory())
            {
                FaultDetectionLogic localFaultDetectionLogic = new TableOperations<FaultDetectionLogic>(connection).QueryRecordWhere("MeterAssetID = {0}", localMeterLine.ID);
                if (localFaultDetectionLogic == null) return;

                FaultDetectionLogic remoteFaultDetectionLogic = WebAPIHub.GetRecordWhere<FaultDetectionLogic>($"MeterAssetID = {remoteMeterLine.ID}", requester);


                if (remoteFaultDetectionLogic == null)
                {
                    remoteFaultDetectionLogic = new FaultDetectionLogic()
                    {
                        MeterAssetID = remoteMeterLine.ID,
                        Expression = localFaultDetectionLogic.Expression
                    };
                    WebAPIHub.CreateRecord<FaultDetectionLogic>(remoteFaultDetectionLogic, requester);
                }
                else
                {
                    remoteFaultDetectionLogic.Expression = localFaultDetectionLogic.Expression;
                    WebAPIHub.UpdateRecord<FaultDetectionLogic>(remoteFaultDetectionLogic, requester);
                }
            }
        }

        private AssetLocation AddOrUpdateAssetLocation(Asset remoteAsset, Location remoteLocation, DatapusherRequester requester)
        {
            // add line to meterlocationline table
            AssetLocation remoteMeterLocationLine = WebAPIHub.GetRecordWhere<AssetLocation>($"LocationID = {remoteLocation.ID} AND AssetID = {remoteAsset.ID}", requester);

            if (remoteMeterLocationLine == null)
            {
                remoteMeterLocationLine = new AssetLocation();
                remoteMeterLocationLine.AssetID = remoteAsset.ID;
                remoteMeterLocationLine.LocationID = remoteLocation.ID;

                WebAPIHub.CreateRecord(remoteMeterLocationLine, requester);
            }

            return remoteMeterLocationLine;
        }

        private void AddOrUpdateChannelsForLine(MeterAsset localMeterAsset, MeterAsset remoteMeterAsset, DatapusherRequester requester)
        {
            using (AdoDataConnection connection = ConnectionFactory())
            {
                // ensure remote and local line impedance matches
                IEnumerable<ChannelDetail> localChannels = new TableOperations<ChannelDetail>(connection).QueryRecordsWhere("MeterID = {0} AND AssetID = {1}", localMeterAsset.MeterID, localMeterAsset.AssetID);
                List<ChannelDetail> remoteChannels = WebAPIHub.GetChannels($"MeterID = {remoteMeterAsset.MeterID} AND AssetID = {remoteMeterAsset.AssetID}", requester).ToList();

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

                        remoteChannelDetail.ID = WebAPIHub.CreateChannel(JObject.FromObject(remoteChannelDetail), requester);

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

                        WebAPIHub.UpdateChannel(JObject.FromObject(remoteChannelDetail), requester);
                    }

                    AddOrGetSeries(localChannelDetail, remoteChannelDetail, requester);
                }
            }
        }

        private void AddOrGetSeries(ChannelDetail localChannel, ChannelDetail remoteChannel, DatapusherRequester requester)
        {
            using (AdoDataConnection connection = ConnectionFactory())
            {

                IEnumerable<Series> local = new TableOperations<Series>(connection).QueryRecordsWhere("ChannelID = {0}", localChannel.ID);
                List<Series> remote = WebAPIHub.GetRecordsWhere<Series>($"ChannelID = {remoteChannel.ID}", requester).ToList();

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

                        remoteSeries.ID = WebAPIHub.CreateRecord(remoteSeries, requester);

                    }
                    else
                    {
                        remoteSeries.SeriesTypeID = localSeries.SeriesTypeID;
                        remoteSeries.SourceIndexes = localSeries.SourceIndexes;
                        WebAPIHub.UpdateRecord(remoteSeries, requester);
                    }
                }
            }

        }

        private void AddAssetConnections(AssetConnection assetConnection, string connectionType, IEnumerable<AssetConnectionType> remoteAssetConnectionTypes, DatapusherRequester requester) {
            using (AdoDataConnection connection = ConnectionFactory()) {
                int remoteXDAAssetID1 = connection.ExecuteScalar<int>("SELECT RemoteXDAAssetID FROM AssetsToDataPush WHERE LocalXDAAssetID = {0}", assetConnection.ParentID);
                int remoteXDAAssetID2 = connection.ExecuteScalar<int>("SELECT RemoteXDAAssetID FROM AssetsToDataPush WHERE LocalXDAAssetID = {0}", assetConnection.ChildID);
                if ((remoteXDAAssetID1 <= 0) || (remoteXDAAssetID2 <= 0))
                    return;
                AssetConnection remoteAssetConnection = WebAPIHub.GetRecordWhere<AssetConnection>($"ParentID = {remoteXDAAssetID1}  AND ChildID = {remoteXDAAssetID2}", requester);
                if(remoteAssetConnection == null)
                {
                    remoteAssetConnection = new AssetConnection();
                    remoteAssetConnection.ParentID = remoteXDAAssetID1;
                    remoteAssetConnection.ChildID = remoteXDAAssetID2;
                    remoteAssetConnection.AssetRelationshipTypeID = remoteAssetConnectionTypes.First(x => x.Name == connectionType).ID;

                    WebAPIHub.CreateRecord(remoteAssetConnection, requester);
                }

            }
        }

        public (bool, Exception) TestInstance(int instanceId)
        {
            using (AdoDataConnection connection = ConnectionFactory())
            {
                RemoteXDAInstance instance = new TableOperations<RemoteXDAInstance>(connection).QueryRecordWhere("ID = {0}", instanceId);
                if (instance is null) throw new Exception($"No remote XDA instance found with this instance ID${instanceId}");
                DatapusherRequester requester = new DatapusherRequester(instance, connection);
                return WebAPIHub.TestConnection(requester).GetAwaiter().GetResult();
            }
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
                    if (progressTotal == 0) return;
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

                    DatapusherRequester requester = new DatapusherRequester(instance, connection);
                    Settings settings = new Settings(GetConfigurator());

                    DateTime timeWindowStartDate = DateTime.UtcNow.AddHours(settings.DataPusherSettings.TimeWindow * -1);

                    MetersToDataPush meterToDataPush = new TableOperations<MetersToDataPush>(connection).QueryRecordWhere("ID = {0}", meterId);
                    if (meterToDataPush.RemoteXDAMeterID <= 0)
                        throw new Exception($"Meter (ID:{meterId}) that was to have its files synced has not yet been pushed to instance (ID:{instance.ID}).");

                    string localAssetKey = new TableOperations<Meter>(connection).QueryRecordWhere("ID={0}", meterToDataPush.LocalXDAMeterID).AssetKey;

                    if (settings.DataPusherSettings.TimeWindow != 0)
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
                                ProcessingStatus = (int)FileGroupProcessingStatus.Created,
                                MeterID = meterToDataPush.RemoteXDAMeterID
                            };
                            int remoteFileGroupId = WebAPIHub.CreateRecord<FileGroup>(fg, requester);
                            fileGroupLocalToRemote = new FileGroupLocalToRemote()
                            {
                                RemoteXDAInstanceID = instance.ID,
                                LocalFileGroupID = fileGroup.ID,
                                RemoteFileGroupID = remoteFileGroupId
                            };
                            new TableOperations<FileGroupLocalToRemote>(connection).AddNewRecord(fileGroupLocalToRemote);
                        }

                        IEnumerable<DataFile> localDataFiles = new TableOperations<DataFile>(connection).QueryRecordsWhere("FileGroupID = {0}", fileGroupLocalToRemote.LocalFileGroupID);
                        IEnumerable<DataFile> remoteDataFiles = WebAPIHub.GetRecordsWhere<DataFile>($"FileGroupID = {fileGroupLocalToRemote.RemoteFileGroupID}", requester);

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
                                remoteDataFileId = WebAPIHub.CreateRecord(df, requester);
                                process = true;
                            }
                            else
                                remoteDataFileId = remoteDataFiles.Where(x => x.FilePath == localDataFile.FilePath).First().ID;

                            FileBlob remoteFileBlob = WebAPIHub.GetRecordWhere<FileBlob>($"DataFileID = {remoteDataFileId}", requester);

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
                                WebAPIHub.CreateRecord(new FileBlob() { DataFileID = remoteDataFileId, Blob = localFileBlob.Blob }, requester);

                            }
                        }

                        if (process)
                        {
                            Dictionary<string, int> dictionary = new Dictionary<string, int>();
                            dictionary.Add("FileGroupID", fileGroupLocalToRemote.RemoteFileGroupID);
                            dictionary.Add("MeterID", meterToDataPush.RemoteXDAMeterID);
                            WebAPIHub.ProcessFileGroup(JObject.FromObject(dictionary), requester);

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
                    DatapusherRequester requester = new DatapusherRequester(instance, connection);

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
                            ProcessingStatus = (int)FileGroupProcessingStatus.Created,
                            MeterID = meterToDataPush.RemoteXDAMeterID
                        };
                        int remoteFileGroupId = WebAPIHub.CreateRecord(fg, requester);
                        fileGroupLocalToRemote = new FileGroupLocalToRemote()
                        {
                            LocalFileGroupID = fileGroup.ID,
                            RemoteFileGroupID = remoteFileGroupId
                        };
                        new TableOperations<FileGroupLocalToRemote>(connection).AddNewRecord(fileGroupLocalToRemote);
                    }

                    IEnumerable<DataFile> localDataFiles = new TableOperations<DataFile>(connection).QueryRecordsWhere("FileGroupID = {0}", fileGroupLocalToRemote.LocalFileGroupID);
                    IEnumerable<DataFile> remoteDataFiles = WebAPIHub.GetRecordsWhere<DataFile>($"FileGroupID = {fileGroupLocalToRemote.RemoteFileGroupID}", requester);

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
                            remoteDataFileId = WebAPIHub.CreateRecord(df, requester);
                            process = true;
                        }
                        else
                            remoteDataFileId = remoteDataFiles.Where(x => x.FilePath == localDataFile.FilePath).First().ID;

                        FileBlob remoteFileBlob = WebAPIHub.GetRecordWhere<FileBlob>($"DataFileID = {remoteDataFileId}", requester);

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
                            WebAPIHub.CreateRecord(new FileBlob() { DataFileID = remoteDataFileId, Blob = localFileBlob.Blob }, requester);
                        }
                    }

                    if (process)
                    {
                        Dictionary<string, int> dictionary = new Dictionary<string, int>();
                        dictionary.Add("FileGroupID", fileGroupLocalToRemote.RemoteFileGroupID);
                        dictionary.Add("MeterID", meterToDataPush.RemoteXDAMeterID);
                        WebAPIHub.ProcessFileGroup(JObject.FromObject(dictionary), requester);

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
                MetersToDataPush meter = new TableOperations<MetersToDataPush>(connection).QueryRecordWhere("ID = {0}", meterId);
                FileGroup fileGroup = new TableOperations<FileGroup>(connection).QueryRecordWhere("ID = {0}", fileGroupID);
                List<DataFile> files = new TableOperations<DataFile>(connection).QueryRecordsWhere("FileGroupID = {0}", fileGroupID).ToList();
                List<FileBlob> fileBlobs = new TableOperations<FileBlob>(connection).QueryRecordsWhere("DataFileID IN (SELECT ID FROM DataFile WHERE FileGroupID = {0})", fileGroupID).ToList();
                FileGroupPost post = new FileGroupPost();
                post.MeterKey = meter.RemoteXDAAssetKey;
                post.FileGroup = fileGroup;
                post.DataFiles = files;
                post.FileBlobs = fileBlobs;

                DatapusherRequester requester = new DatapusherRequester(instanceId, connection);
                int id = SendFiles(post, requester).Result;
                connection.ExecuteNonQuery("INSERT INTO FileGroupLocalToRemote (RemoteXDAInstanceID, LocalFileGroupID, remoteFileGroupID) VALUES ({0},{1},{2}) ", instanceId, fileGroupID, id);

            }

        }

        public Task<int> SendFiles(FileGroupPost post, DatapusherRequester requester)
        {
            return Task.Run(() =>
            {

                MemoryStream stream = new MemoryStream();
                BinaryFormatter binaryFormater = new BinaryFormatter();
                binaryFormater.Serialize(stream, post);
                stream.Seek(0, SeekOrigin.Begin);
                HttpContent httpContent = new StreamContent(stream);

                using (HttpResponseMessage response = requester.SendRequestAsync("api/DataPusher/Recieve/Files", HttpMethod.Post, httpContent, "application/text").Result)
                {
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
        #endregion
    }
}