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
using System.Linq;
using openXDA.Model;
using GSF.Web.Model;
using Newtonsoft.Json.Linq;
using GSF;
using System.IO;
using GSF.Scheduling;

namespace openXDA.DataPusher
{
    public class DataPusherEngine : IDisposable
    {
        #region [ Members ]

        // Fields
        private string m_dbConnectionString;
        private DataContext m_dataContext;
        private bool m_disposed;
        private ScheduleManager m_scheduler;
        private bool m_running = false;

        #endregion

        #region [ Constructors ]

        #endregion

        #region [ Properties ]
        private DataContext DataContext => m_dataContext ?? (m_dataContext = new DataContext("systemSettings"));
        private ScheduleManager Scheduler => m_scheduler ?? (m_scheduler = new ScheduleManager());
        public bool Running => m_running;
        #endregion

        // Client-side script functionality
        #region [ Static ]

        public static event EventHandler<EventArgs<string>> LogStatusMessageEvent;

        private static void OnLogStatusMessage(string message)
        {
            LogStatusMessageEvent?.Invoke(new object(), new EventArgs<string>(message));
        }

        public static event EventHandler<EventArgs<string>> LogExceptionMessageEvent;

        private static void OnLogExceptionMessage(string message)
        {
            LogExceptionMessageEvent?.Invoke(new object(), new EventArgs<string>(message));
        }

        public static event EventHandler<EventArgs<Dictionary<int, int>>> ReprocessFilesEvent;

        private static void OnReprocessFiles(Dictionary<int, int> fileGroups)
        {
            ReprocessFilesEvent?.Invoke(new object(), new EventArgs<Dictionary<int, int>>(fileGroups));
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
        public void Dispose()
        {
            if (!m_disposed)
            {
                try
                {
                    Stop();
                    m_dataContext.Dispose();
                    m_disposed = true;
                }
                catch (Exception ex)
                {
                    OnLogStatusMessage(ex.ToString());
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

                IEnumerable<RemoteXDAInstance> instances = DataContext.Table<RemoteXDAInstance>().QueryRecords();
                foreach (RemoteXDAInstance instance in instances)
                {
                    if (instance.Frequency != string.Empty)
                        Scheduler.AddSchedule(instance.Name, instance.Frequency);
                }

                Scheduler.Start();  
                m_running = true;
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
            IEnumerable<RemoteXDAInstance> instances = DataContext.Table<RemoteXDAInstance>().QueryRecords();
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

        // The process of syncing the local database with the remote database
        public void SyncDatabases()
        {
            //Stopwatch stopwatch = new Stopwatch();
            //stopwatch.Start();
            //int remoteMeterLocationId = 1;
            //int meterGroupId = AddMeterGroup();

            //IEnumerable<MetersToDataPush> selectedMeters = m_dataContext.Table<MetersToDataPush>().QueryRecords("LocalXDAAssetKey");

            //foreach (MetersToDataPush meter in selectedMeters)
            //{

            //    // if meter doesnt exist remotely add it
            //    //AddMeter(meter, remoteMeterLocationId);
            //    AddMeterMeterGroup(meterGroupId, meter.RemoteXDAMeterID);

            //    // get MeterLine table 
            //    List<MeterLine> localMeterLines = WebAPIHub.GetRecords("local", "MeterLine", "all").Where(x => x.MeterID == meter.LocalXDAMeterID).Select(x => (MeterLine)x).ToList();

            //    // if there is a line for the meter ensure that its data has been uploaded remotely
            //    foreach (MeterLine meterLine in localMeterLines)
            //    {

            //        LinesToDataPush selectedLine = AddLine(meterLine);

            //        // if MeterLine association has not been previously made, make it
            //        AddMeterLine(meter, selectedLine);

            //        // ensure remote and local line impedance matches
            //        SyncLineImpedances(selectedLine);

            //        // add line to meterlocationline table
            //        int meterLocationLineID = SyncMeterLocationLines(selectedLine.RemoteXDALineID, remoteMeterLocationId);

            //        // ensure remote and local Source Impedance records match for the current meter line location
            //        SyncSourceImpedance(meterLocationLineID);

            //        // Sync Channel and channel dependant data
            //        SyncChannel(meter, selectedLine);

            //        SyncEvents(meter.LocalXDAMeterID, meter.RemoteXDAMeterID, selectedLine.LocalXDALineID, selectedLine.RemoteXDALineID);
            //    }

            //    //Get Meter Alarm Summary Data
            //    SyncMeterAlarmSummary(meter);

            //    //Get Meter Data Quality Summary Data
            //    SyncMeterDataQualitySummary(meter);

            //    //Get Facility Data
            //    SyncMeterFacility(meter);

            //    Console.Write("Running Time: " + stopwatch.Elapsed);
            //    stopwatch.Stop();
            //}
        }

        public void SyncInstanceConfiguration(string clientId, int instanceId) {
            IEnumerable<int> meters = DataContext.Table<MetersToDataPush>().QueryRecordsWhere("ID IN (SELECT MetersToDataPushID FROM RemoteXDAInstanceMeter WHERE RemoteXDAInstanceID = {0})", instanceId).Select(x => x.ID);
            string instance = DataContext.Table<RemoteXDAInstance>().QueryRecordWhere("ID = {0}", instanceId).Name;
            int progressTotal = meters.Count();
            int progressCount = 0;
            OnUpdateProgressForInstance(clientId, instance, (int)(100 * (progressCount) / progressTotal));
            foreach (int meter in meters)
            {
                SyncMeterConfigurationForInstance(null, instanceId, meter);
                OnUpdateProgressForInstance(clientId, instance, (int)(100 * (++progressCount) / progressTotal));

            }
        }

        public void SyncInstanceFiles(string clientId, int instanceId)
        {
            IEnumerable<int> meters = DataContext.Table<MetersToDataPush>().QueryRecordsWhere("ID IN (SELECT MetersToDataPushID FROM RemoteXDAInstanceMeter WHERE RemoteXDAInstanceID = {0})", instanceId).Select(x => x.ID);
            int progressTotal = meters.Count();
            int progressCount = 0;
            string instance = DataContext.Table<RemoteXDAInstance>().QueryRecordWhere("ID = {0}", instanceId).Name;
            OnUpdateProgressForInstance(clientId, instance, (int)(100 * (progressCount) / progressTotal));
            foreach (int meter in meters)
            {
                SyncMeterFilesForInstance(clientId, instanceId, meter);
                OnUpdateProgressForInstance(clientId, instance, (int)(100 * (++progressCount) / progressTotal));
            }
        }

        public void SyncMeterConfigurationForInstance(string clientId, int instanceId, int meterId) {

            RemoteXDAInstance instance = DataContext.Table<RemoteXDAInstance>().QueryRecordWhere("ID = {0}", instanceId);
            MetersToDataPush meterToDataPush = DataContext.Table<MetersToDataPush>().QueryRecordWhere("ID = {0}", meterId);
            Meter localMeterRecord = DataContext.Table<Meter>().QueryRecordWhere("ID = {0}", meterToDataPush.LocalXDAMeterID);
            // get MeterLine table 
            IEnumerable<MeterLine> localMeterLines = DataContext.Table<MeterLine>().QueryRecordsWhere("MeterID = {0}", localMeterRecord.ID);
            int progressTotal = localMeterLines.Count() + 2;
            int progressCount = 0;
            int remoteMeterLocationId = SyncMeterLocations(instance.Address, meterToDataPush, localMeterRecord);
            int meterGroupId = AddMeterGroup(instance.Address);
            OnUpdateProgressForMeter(clientId, localMeterRecord.AssetKey, (int)(100 * (progressCount) / progressTotal));

            // if meter doesnt exist remotely add it
            AddMeter(instance.Address, meterToDataPush, localMeterRecord, remoteMeterLocationId);
            OnUpdateProgressForMeter(clientId, localMeterRecord.AssetKey,(int)(100*(++progressCount)/progressTotal));
            AddMeterMeterGroup(instance.Address, meterGroupId, meterToDataPush.RemoteXDAMeterID);
            OnUpdateProgressForMeter(clientId, localMeterRecord.AssetKey, (int)(100 * (++progressCount) / progressTotal));

            // if there is a line for the meter ensure that its data has been uploaded remotely
            foreach (MeterLine meterLine in localMeterLines)
            {

                LinesToDataPush selectedLine = AddLine(instance.Address, meterLine, meterToDataPush.Obsfucate);

                // if MeterLine association has not been previously made, make it
                AddMeterLine(instance.Address, meterToDataPush, selectedLine);

                // ensure remote and local line impedance matches
                SyncLineImpedances(instance.Address, selectedLine);

                // add line to meterlocationline table
                int meterLocationLineID = SyncMeterLocationLines(instance.Address, selectedLine.RemoteXDALineID, remoteMeterLocationId);

                // ensure remote and local Source Impedance records match for the current meter line location
                SyncSourceImpedance(instance.Address, meterLocationLineID);

                // Sync Channel and channel dependant data
                SyncChannel(instance.Address, meterToDataPush, selectedLine);

                OnUpdateProgressForMeter(clientId, localMeterRecord.AssetKey, (int)(100 * (++progressCount) / progressTotal));
            }
        }

        public void SyncMeterFilesForInstance(string clientId, int instanceId, int meterId, DateTime? startTime = null, DateTime? endTime = null)
        {
            RemoteXDAInstance instance = DataContext.Table<RemoteXDAInstance>().QueryRecordWhere("ID = {0}", instanceId);
            MetersToDataPush meterToDataPush = DataContext.Table<MetersToDataPush>().QueryRecordWhere("ID = {0}", meterId);
            IEnumerable<FileGroup> localFileGroups = DataContext.Table<FileGroup>().QueryRecordsWhere("ID IN (SELECT FileGroupID From Event WHERE MeterID = {0})", meterToDataPush.LocalXDAMeterID);
            int progressTotal = (localFileGroups.Count() > 0 ? localFileGroups.Count() : 1 );
            int progressCount = 0;
            OnUpdateProgressForMeter(clientId, meterToDataPush.LocalXDAAssetKey, (int)(100 * (progressCount) / progressTotal));

            foreach (FileGroup fileGroup in localFileGroups)
            {
                FileGroupLocalToRemote fileGroupLocalToRemote = DataContext.Table<FileGroupLocalToRemote>().QueryRecordWhere("LocalFileGroupID = {0}", fileGroup.ID);

                if (fileGroupLocalToRemote == null)
                {
                    FileGroup fg = new FileGroup()
                    {
                        ProcessingEndTime = fileGroup.ProcessingEndTime,
                        ProcessingStartTime = fileGroup.ProcessingStartTime,
                        DataEndTime = fileGroup.DataEndTime,
                        DataStartTime = fileGroup.DataStartTime,
                        Error = fileGroup.Error
                    };
                    int remoteFileGroupId = WebAPIHub.CreateRecord(instance.Address, "FileGroup", JObject.FromObject(fg));
                    fileGroupLocalToRemote = new FileGroupLocalToRemote()
                    {
                        LocalFileGroupID = fileGroup.ID,
                        RemoteFileGroupID = remoteFileGroupId
                    };
                    DataContext.Table<FileGroupLocalToRemote>().AddNewRecord(fileGroupLocalToRemote);
                }

                IEnumerable<DataFile> localDataFiles = DataContext.Table<DataFile>().QueryRecordsWhere("FileGroupID = {0}", fileGroupLocalToRemote.LocalFileGroupID);
                IEnumerable<DataFile> remoteDataFiles = WebAPIHub.GetRecordsWhere(instance.Address, "DataFile", $"FileGroupID = {fileGroupLocalToRemote.RemoteFileGroupID}").Select(x => (DataFile)x);

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
                        remoteDataFileId = WebAPIHub.CreateRecord(instance.Address, "DataFile", JObject.FromObject(df));
                        process = true;
                    }
                    else
                        remoteDataFileId = remoteDataFiles.Where(x => x.FilePath == localDataFile.FilePath).First().ID;

                    FileBlob remoteFileBlob = (FileBlob)WebAPIHub.GetRecordsWhere(instance.Address, "FileBlob", $"DataFileID = {remoteDataFileId}").FirstOrDefault();

                    if (remoteFileBlob == null)
                    {
                        FileBlob localFileBlob = DataContext.Table<FileBlob>().QueryRecordWhere("DataFileID = {0}", localDataFile.ID);

                        try
                        {
                            if (localFileBlob == null)
                            {
                                localFileBlob = new FileBlob() { DataFileID = localDataFile.ID, Blob = File.ReadAllBytes(localDataFile.FilePath) };
                                DataContext.Table<FileBlob>().AddNewRecord(localFileBlob);
                            }
                        }
                        catch (Exception ex)
                        {
                            OnLogExceptionMessage(ex.ToString());
                            process = false;
                        }
                        localFileBlob.DataFileID = remoteDataFileId;
                        WebAPIHub.CreateRecord(instance.Address, "FileBlob", JObject.FromObject(new FileBlob() { DataFileID = remoteDataFileId, Blob = localFileBlob.Blob }));

                    }
                }

                if (process)
                {
                    Dictionary<string, int> dictionary = new Dictionary<string, int>();
                    dictionary.Add("FileGroupID", fileGroupLocalToRemote.RemoteFileGroupID);
                    dictionary.Add("MeterID", meterToDataPush.RemoteXDAMeterID);
                    WebAPIHub.ProcessFileGroup(instance.Address, JObject.FromObject(dictionary));

                }

                OnUpdateProgressForMeter(clientId,meterToDataPush.LocalXDAAssetKey, (int)(100 * (++progressCount) / progressTotal));

            }
        }

        private void AddMeter(string address, MetersToDataPush meter, Meter localMeterRecord,int remoteMeterLocationId)
        {
            List<Meter> remoteMeters = WebAPIHub.GetRecords(address, "Meter", "all").Select(x => (Meter)x).ToList();

            // if meter doesnt exist remotely create the meter record
            if (!remoteMeters.Where(x => x.AssetKey.Equals(meter.RemoteXDAAssetKey.ToString())).Any())
            {
                if (meter.Obsfucate)
                {
                    Meter record = new Meter()
                    {
                        AssetKey = meter.RemoteXDAAssetKey.ToString(),
                        MeterLocationID = remoteMeterLocationId,
                        Name = meter.RemoteXDAName,
                        Alias = meter.RemoteXDAName,
                        ShortName = "",
                        Make = localMeterRecord.Make,
                        Model = localMeterRecord.Model,
                        Description = localMeterRecord.Description,
                        MeterTypeID = localMeterRecord.MeterTypeID,
                        TimeZone = localMeterRecord.TimeZone
                    };

                    meter.RemoteXDAMeterID = WebAPIHub.CreateRecord(address, "Meter", JObject.FromObject(record));
                }
                else
                {
                    localMeterRecord.MeterLocationID = remoteMeterLocationId; 
                    meter.RemoteXDAMeterID = WebAPIHub.CreateRecord(address, "Meter", JObject.FromObject(localMeterRecord));
                }

                DataContext.Table<MetersToDataPush>().UpdateRecord(meter);
            }
            else
            {
                meter.RemoteXDAMeterID = remoteMeters.Where(x => x.AssetKey.Equals(meter.RemoteXDAAssetKey.ToString())).First().ID;
            }
        }

        private LinesToDataPush AddLine(string address, MeterLine meterLine, bool obsfucate)
        {
            Line localLine = DataContext.Table<Line>().QueryRecordWhere("ID = {0}", meterLine.LineID);
            List<LinesToDataPush> selectedLines = DataContext.Table<LinesToDataPush>().QueryRecords("LocalXDAAssetKey").ToList();
            List<Line> remoteLines = WebAPIHub.GetRecords(address, "Line", "all").Select(x => (Line)x).ToList();

            //if the line does not exist in the PQMarkPusher Database to allow for obsfucation add it.
            if (!selectedLines.Where(x => x.LocalXDALineID == meterLine.LineID).Any())
            {
                LinesToDataPush record = new LinesToDataPush()
                {
                    LocalXDALineID = localLine.ID,
                    RemoteXDALineID = 0,
                    LocalXDAAssetKey = localLine.AssetKey,
                    RemoteXDAAssetKey = (obsfucate ? Guid.NewGuid().ToString(): localLine.AssetKey)
                };

                Line newRecord = new Line()
                {
                    AssetKey = (obsfucate? record.RemoteXDAAssetKey.ToString() : localLine.AssetKey),
                    VoltageKV = localLine.VoltageKV,
                    ThermalRating = localLine.ThermalRating,
                    Length = localLine.Length,
                    Description = (obsfucate? "" : localLine.Description)
                };

                record.RemoteXDALineID = WebAPIHub.CreateRecord(address, "Line", JObject.FromObject(newRecord));
                DataContext.Table<LinesToDataPush>().AddNewRecord(record);
                return record;
            }
            else
            {
                return DataContext.Table<LinesToDataPush>().QueryRecordWhere("LocalXDALineID = {0}", meterLine.LineID);
            }

        }

        private int SyncMeterLocations(string address, MetersToDataPush meterToDataPush, Meter localMeterRecord)
        {
            List<MeterLocation> remoteMeterLocations = WebAPIHub.GetRecords(address, "MeterLocation", "all").Select(x => (MeterLocation)x).ToList();

            if (meterToDataPush.Obsfucate)
            {
                // if the company meter location does not exist, create it
                if (!remoteMeterLocations.Where(x => x.AssetKey == WebAPIHub.CompanyName).Any())
                {
                    MeterLocation record = new MeterLocation()
                    {
                        AssetKey = WebAPIHub.CompanyName,
                        Name = WebAPIHub.CompanyName,
                        Alias = WebAPIHub.CompanyName,
                        ShortName = "",
                        Description = "",
                        Latitude = 0.0F,
                        Longitude = 0.0F
                    };

                    return WebAPIHub.CreateRecord(address, "MeterLocation", JObject.FromObject(record));
                }
                else
                {
                    return remoteMeterLocations.Where(x => x.AssetKey == WebAPIHub.CompanyName).First().ID;
                }

            }
            else
            {
                MeterLocation meterLocation = DataContext.Table<MeterLocation>().QueryRecordWhere("ID = {0}", localMeterRecord.MeterLocationID);
                
                if(!remoteMeterLocations.Where(x => x.AssetKey == meterLocation.AssetKey).Any())
                    return WebAPIHub.CreateRecord(address, "MeterLocation", JObject.FromObject(meterLocation));
                else
                    return remoteMeterLocations.Where(x => x.AssetKey == meterLocation.AssetKey).First().ID;
            }
        }

        private int AddMeterGroup(string address)
        {
            List<MeterGroup> remote = WebAPIHub.GetRecords(address, "MeterGroup", "all").Select(x => (MeterGroup)x).ToList();
            // if the company meter location does not exist, create it
            if (!remote.Where(x => x.Name == WebAPIHub.CompanyName).Any())
            {
                MeterGroup record = new MeterGroup()
                {
                    Name = WebAPIHub.CompanyName
                };

                return WebAPIHub.CreateRecord(address, "MeterGroup", JObject.FromObject(record));
            }
            else
            {
                return remote.Where(x => x.Name == WebAPIHub.CompanyName).First().ID;
            }
        }

        private void AddMeterMeterGroup(string address, int meterGroupId, int meterId)
        {
            List<MeterMeterGroup> remote = WebAPIHub.GetRecordsWhere(address, "MeterMeterGroup", $"MeterID = {meterId} AND MeterGroupID = {meterGroupId}").Select(x => (MeterMeterGroup)x).ToList();

            // if MeterLine association has not been previously made, make it
            if (!remote.Any())
            {
                MeterMeterGroup record = new MeterMeterGroup()
                {
                    MeterID = meterId,
                    MeterGroupID = meterGroupId
                };

                WebAPIHub.CreateRecord(address, "MeterMeterGroup", JObject.FromObject(record));
            }
        }

        private void AddMeterLine(string address, MetersToDataPush meter, LinesToDataPush selectedLine)
        {
            List<MeterLine> remoteMeterLines = WebAPIHub.GetRecordsWhere(address, "MeterLine", $"MeterID = {meter.RemoteXDAMeterID} AND LineID = {selectedLine.RemoteXDALineID}").Select(x => (MeterLine)x).ToList();

            // if MeterLine association has not been previously made, make it
            if (!remoteMeterLines.Any())
            {
                MeterLine record = new MeterLine()
                {
                    MeterID = meter.RemoteXDAMeterID,
                    LineID = selectedLine.RemoteXDALineID,
                    LineName = selectedLine.RemoteXDAAssetKey.ToString()
                };

                WebAPIHub.CreateRecord(address, "MeterLine", JObject.FromObject(record));
            }
        }

        private void SyncLineImpedances(string address, LinesToDataPush selectedLine)
        {
            // ensure remote and local line impedance matches
            LineImpedance localLineImpedance = DataContext.Table<LineImpedance>().QueryRecordWhere("LineID = {0}",selectedLine.LocalXDALineID);
            LineImpedance remoteLineImpedance = (LineImpedance)WebAPIHub.GetRecordsWhere(address, "LineImpedance", $"LineID = {selectedLine.RemoteXDALineID}").FirstOrDefault();

            // if there is a local record but not a remote record
            if (localLineImpedance != null && remoteLineImpedance == null)
            {
                JObject record = new JObject();
                record.Add("LineID", selectedLine.RemoteXDALineID);
                record.Add("R0", localLineImpedance.R0);
                record.Add("R1", localLineImpedance.R1);
                record.Add("X0", localLineImpedance.X0);
                record.Add("X1", localLineImpedance.X1);

                WebAPIHub.CreateRecord(address, "LineImpedance", record);
            }
        }

        private int SyncMeterLocationLines(string address, int lineID, int meterLocationID)
        {
            // add line to meterlocationline table
            dynamic remoteMeterLocationLine = WebAPIHub.GetRecordsWhere(address, "MeterLocationLine", $"LineID = {lineID}").FirstOrDefault();
            if (remoteMeterLocationLine == null)
            {
                JObject record = new JObject();
                record.Add("LineID", lineID);
                record.Add("MeterLocationID", meterLocationID);

                WebAPIHub.CreateRecord(address, "MeterLocationLine", record);
            }

            return WebAPIHub.GetRecords(address, "MeterLocationLine", "all").Where(x => x.LineID == lineID).First().ID;
        }

        private void SyncSourceImpedance(string address, int meterLocationLineID)
        {
            // ensure remote and local line impedance matches
            SourceImpedance local = DataContext.Table<SourceImpedance>().QueryRecordWhere("MeterLocationLineID = {0}", meterLocationLineID);
            SourceImpedance remote = (SourceImpedance)WebAPIHub.GetRecordsWhere(address, "SourceImpedance", $"MeterLocationLineID = {meterLocationLineID}").FirstOrDefault();

            // if there is a local record but not a remote record
            if (local != null && remote == null)
            {
                JObject record = new JObject();
                record.Add("MeterLocationLineID", meterLocationLineID);
                record.Add("RSrc", local.RSrc);
                record.Add("XSrc", local.XSrc);

                WebAPIHub.CreateRecord(address, "LineImpedance", record);
            }

        }

        private void SyncMeterAlarmSummary(string address, MetersToDataPush meter)
        {
            // ensure remote and local line impedance matches
            IEnumerable<MeterAlarmSummary> local = DataContext.Table<MeterAlarmSummary>().QueryRecordsWhere("MeterID = {0}", meter.LocalXDAMeterID);
            List<MeterAlarmSummary> remote = WebAPIHub.GetRecordsWhere(address, "MeterAlarmSummary", $"MeterID = {meter.RemoteXDAMeterID}").Select(x => (MeterAlarmSummary)x).ToList();

            // if there is a local record but not a remote record
            foreach (MeterAlarmSummary summary in local)
            {
                if (!remote.Where(x => x.Date == summary.Date).Any())
                {
                    JObject record = new JObject();
                    record.Add("MeterID", meter.RemoteXDAMeterID);
                    record.Add("AlarmTypeID", summary.AlarmTypeID);
                    record.Add("Date", summary.Date);
                    record.Add("AlarmPoints", summary.AlarmPoints);

                    WebAPIHub.CreateRecord(address, "MeterAlarmSummary", record);
                }
            }
        }

        private void SyncMeterDataQualitySummary(string address, MetersToDataPush meter)
        {
            // ensure remote and local line impedance matches
            IEnumerable<MeterDataQualitySummary> local = DataContext.Table<MeterDataQualitySummary>().QueryRecordsWhere("MeterID = {0}", meter.LocalXDAMeterID);
            List<MeterDataQualitySummary> remote = WebAPIHub.GetRecordsWhere(address, "MeterDataQualitySummary", $"MeterID = {meter.RemoteXDAMeterID}").Select(x => (MeterDataQualitySummary)x).ToList();

            // if there is a local record but not a remote record
            foreach (MeterDataQualitySummary summary in local)
            {
                if (!remote.Where(x => x.Date == summary.Date).Any())
                {
                    JObject record = new JObject();
                    record.Add("MeterID", meter.RemoteXDAMeterID);
                    record.Add("Date", summary.Date);
                    record.Add("ExpectedPoints", summary.ExpectedPoints);
                    record.Add("GoodPoints", summary.GoodPoints);
                    record.Add("LatchedPoints", summary.LatchedPoints);
                    record.Add("UnreasonablePoints", summary.UnreasonablePoints);
                    record.Add("NoncongruentPoints", summary.NoncongruentPoints);
                    record.Add("DuplicatePoints", summary.DuplicatePoints);

                    WebAPIHub.CreateRecord(address, "MeterDataQualitySummary", record);
                }
            }

        }

        private void SyncMeterFacility(string address, MetersToDataPush meter)
        {
            // ensure remote and local line impedance matches
            MeterFacility local = DataContext.Table<MeterFacility>().QueryRecordWhere("MeterID = {0}", meter.LocalXDAMeterID);
            MeterFacility remote = WebAPIHub.GetRecordsWhere(address, "MeterFacility", $"MeterID = {meter.RemoteXDAMeterID}").Select(x => (MeterFacility)x).FirstOrDefault();

            // if there is a local record but not a remote record
            if (local != null && remote == null)
            {
                JObject record = new JObject();
                record.Add("MeterID", meter.RemoteXDAMeterID);
                record.Add("FacilityID", local.FacilityID);

                WebAPIHub.CreateRecord(address, "MeterFacility", record);
            }
        }

        private void SyncChannel(string address, MetersToDataPush meter, LinesToDataPush line)
        {
            // ensure remote and local line impedance matches
            IEnumerable<ChannelDetail> local = DataContext.Table<ChannelDetail>().QueryRecordsWhere("MeterID = {0} AND LineID = {1}",meter.LocalXDAMeterID,line.LocalXDALineID);
            List<ChannelDetail> remote = WebAPIHub.GetChannels(address, $"MeterID = {meter.RemoteXDAMeterID} AND LineID = {line.RemoteXDALineID}").ToList();

            // if there is a local record but not a remote record
            foreach (ChannelDetail summary in local)
            {
                if (!remote.Where(x => x.MeasurementType == summary.MeasurementType && x.MeasurementCharacteristic == summary.MeasurementCharacteristic && x.Phase == summary.Phase && x.Name == summary.Name).Any())
                {
                    JObject record = new JObject();
                    record.Add("MeterID", meter.RemoteXDAMeterID);
                    record.Add("LineID", line.RemoteXDALineID);
                    record.Add("MeasurementType", summary.MeasurementType);
                    record.Add("MeasurementCharacteristic", summary.MeasurementCharacteristic);
                    record.Add("Phase", summary.Phase);
                    record.Add("Name", summary.Name);
                    record.Add("SamplesPerHour", summary.SamplesPerHour);
                    record.Add("PerUnitValue", summary.PerUnitValue);
                    record.Add("HarmonicGroup", summary.HarmonicGroup);
                    record.Add("Description", summary.Description);
                    record.Add("Enabled", summary.Enabled);

                    int remoteChannelId = WebAPIHub.CreateChannel(address, record);

                    SyncSeries(address, summary.ID, remoteChannelId);
                    //SyncAlarmLogs(address, summary.ID, remoteChannelId);
                    SyncAlarmRangeLimit(address, summary.ID, remoteChannelId);
                    SyncBreakerChannel(address, summary.ID, remoteChannelId);
                    //SyncChannelAlarmSummary(address, summary.ID, remoteChannelId);
                    //SyncChannelDataQualitySummary(address, summary.ID, remoteChannelId);
                    //SyncDailyTrendingSummary(address, summary.ID, remoteChannelId);
                    SyncDailyQualityRangeLimit(address, summary.ID, remoteChannelId);
                }
            }
        }

        private void SyncSeries(string address, int localChannelId, int remoteChannelId)
        {
            IEnumerable<Series> local = DataContext.Table<Series>().QueryRecordsWhere("ChannelID = {0}", localChannelId);
            List<Series> remote = WebAPIHub.GetRecordsWhere(address, "Series", $"ChannelID = {remoteChannelId}").Select(x => (Series)x).ToList();

            // if there is a local record but not a remote record
            foreach (Series localSeries in local)
            {
                if (!remote.Where(x => x.SeriesTypeID == localSeries.SeriesTypeID && x.SourceIndexes == localSeries.SourceIndexes).Any())
                {
                    Series record = new Series()
                    {
                        ChannelID = remoteChannelId,
                        SeriesTypeID = localSeries.SeriesTypeID,
                        SourceIndexes = localSeries.SourceIndexes
                    };

                    int remoteSeriesId = WebAPIHub.CreateRecord(address, "Series", JObject.FromObject(record));
                    SynceOutputChannels(address, localSeries.ID, remoteSeriesId);

                }
                else
                {
                    int remoteSeriesId = remote.Where(x => x.SeriesTypeID == localSeries.SeriesTypeID && x.SourceIndexes == localSeries.SourceIndexes).FirstOrDefault().ID;
                    SynceOutputChannels(address, localSeries.ID, remoteSeriesId);
                }

            }

        }

        private void SynceOutputChannels(string address, int localSeriesId, int remoteSeriesId)
        {
            IEnumerable<OutputChannel> local = DataContext.Table<OutputChannel>().QueryRecordsWhere("SeriesID = {0}",localSeriesId);
            List<OutputChannel> remote = WebAPIHub.GetRecordsWhere(address, "OutputChannel", $"SeriesID = {remoteSeriesId}").Select(x => (OutputChannel)x).ToList();

            // if there is a local record but not a remote record
            foreach (OutputChannel localOutputChannel in local)
            {
                if (!remote.Where(x => x.ChannelKey == localOutputChannel.ChannelKey && x.LoadOrder == localOutputChannel.LoadOrder).Any())
                {
                    OutputChannel record = new OutputChannel()
                    {
                        SeriesID = remoteSeriesId,
                        ChannelKey = localOutputChannel.ChannelKey,
                        LoadOrder = localOutputChannel.LoadOrder
                    };
                    WebAPIHub.CreateRecord(address, "OutputChannel", JObject.FromObject(record));
                }
            }
        }

        private void SyncAlarmLogs(string address, int localChannelId, int remoteChannelId)
        {
            IEnumerable<AlarmLog> local = DataContext.Table<AlarmLog>().QueryRecordsWhere("ChannelID = {0}", localChannelId);
            List<AlarmLog> remote = WebAPIHub.GetRecordsWhere(address, "AlarmLog", $"ChannelID = {remoteChannelId}").Select(x => (AlarmLog)x).ToList();

            // if there is a local record but not a remote record
            foreach (AlarmLog localRecord in local)
            {
                if (!remote.Where(x => x.Time == localRecord.Time).Any())
                {
                    AlarmLog record = new AlarmLog()
                    {
                        ChannelID = remoteChannelId,
                        AlarmTypeID = localRecord.AlarmTypeID,
                        Time = localRecord.Time,
                        Severity = localRecord.Severity,
                        LimitHigh = localRecord.LimitHigh,
                        LimitLow = localRecord.LimitLow,
                        Value = localRecord.Value,
                    };
                    WebAPIHub.CreateRecord(address, "AlarmLog", JObject.FromObject(record));
                }
            }
        }

        private void SyncAlarmRangeLimit(string address, int localChannelId, int remoteChannelId)
        {
            IEnumerable<AlarmRangeLimit> local = DataContext.Table<AlarmRangeLimit>().QueryRecordsWhere("ChannelID = {0}", localChannelId);
            List<AlarmRangeLimit> remote = WebAPIHub.GetRecordsWhere(address, "AlarmRangeLimit", $"ChannelID = {remoteChannelId}").Select(x => (AlarmRangeLimit)x).ToList();

            // if there is a local record but not a remote record
            foreach (AlarmRangeLimit localRecord in local)
            {
                if (!remote.Where(x => x.AlarmTypeID == localRecord.AlarmTypeID).Any())
                {
                    AlarmRangeLimit record = new AlarmRangeLimit()
                    {
                        ChannelID = remoteChannelId,
                        AlarmTypeID = localRecord.AlarmTypeID,
                        Severity = localRecord.Severity,
                        High = localRecord.High,
                        Low = localRecord.Low,
                        RangeInclusive = localRecord.RangeInclusive,
                        PerUnit = localRecord.PerUnit,
                        Enabled = localRecord.Enabled,
                        IsDefault = localRecord.IsDefault
                    };
                    WebAPIHub.CreateRecord(address, "AlarmRangeLimit", JObject.FromObject(record));
                }
            }
        }

        private void SyncBreakerChannel(string address, int localChannelId, int remoteChannelId)
        {
            IEnumerable<BreakerChannel> local = DataContext.Table<BreakerChannel>().QueryRecordsWhere("ChannelID = {0}", localChannelId);
            List<BreakerChannel> remote = WebAPIHub.GetRecordsWhere(address, "BreakerChannel", $"ChannelID = {remoteChannelId}").Select(x => (BreakerChannel)x).ToList();

            // if there is a local record but not a remote record
            foreach (BreakerChannel localRecord in local)
            {
                if (!remote.Where(x => x.BreakerNumber == localRecord.BreakerNumber).Any())
                {
                    BreakerChannel record = new BreakerChannel()
                    {
                        ChannelID = remoteChannelId,
                        BreakerNumber = localRecord.BreakerNumber
                    };
                    WebAPIHub.CreateRecord(address, "BreakerChannel", JObject.FromObject(record));
                }
            }
        }

        private void SyncChannelAlarmSummary(string address, int localChannelId, int remoteChannelId)
        {
            IEnumerable<ChannelAlarmSummary> local = DataContext.Table<ChannelAlarmSummary>().QueryRecordsWhere("ChannelID = {0}", localChannelId);
            List<ChannelAlarmSummary> remote = WebAPIHub.GetRecordsWhere(address, "ChannelAlarmSummary", $"ChannelID = {remoteChannelId}").Select(x => (ChannelAlarmSummary)x).ToList();

            // if there is a local record but not a remote record
            foreach (ChannelAlarmSummary localRecord in local)
            {
                if (!remote.Where(x => x.AlarmTypeID == localRecord.AlarmTypeID && x.Date == localRecord.Date).Any())
                {
                    ChannelAlarmSummary record = new ChannelAlarmSummary()
                    {
                        ChannelID = remoteChannelId,
                        AlarmTypeID = localRecord.AlarmTypeID,
                        Date = localRecord.Date,
                        AlarmPoints = localRecord.AlarmPoints
                    };
                    WebAPIHub.CreateRecord(address, "ChannelAlarmSummary", JObject.FromObject(record));
                }
            }
        }

        private void SyncChannelDataQualitySummary(string address, int localChannelId, int remoteChannelId)
        {
            IEnumerable<ChannelDataQualitySummary> local = DataContext.Table<ChannelDataQualitySummary>().QueryRecordsWhere("ChannelID = {0}", localChannelId);
            List<ChannelDataQualitySummary> remote = WebAPIHub.GetRecordsWhere(address, "ChannelDataQualitySummary", $"ChannelID = {remoteChannelId}").Select(x => (ChannelDataQualitySummary)x).ToList();

            // if there is a local record but not a remote record
            foreach (ChannelDataQualitySummary localRecord in local)
            {
                if (!remote.Where(x => x.Date == localRecord.Date).Any())
                {
                    ChannelDataQualitySummary record = new ChannelDataQualitySummary()
                    {
                        ChannelID = remoteChannelId,
                        Date = localRecord.Date,
                        ExpectedPoints = localRecord.ExpectedPoints,
                        GoodPoints = localRecord.GoodPoints,
                        LatchedPoints = localRecord.LatchedPoints,
                        UnreasonablePoints = localRecord.UnreasonablePoints,
                        NoncongruentPoints = localRecord.NoncongruentPoints,
                        DuplicatePoints = localRecord.DuplicatePoints,
                    };
                    WebAPIHub.CreateRecord(address, "ChannelDataQualitySummary", JObject.FromObject(record));
                }
            }
        }

        private void SyncDailyTrendingSummary(string address, int localChannelId, int remoteChannelId)
        {
            IEnumerable<DailyTrendingSummary> local = DataContext.Table<DailyTrendingSummary>().QueryRecordsWhere("ChannelID = {0}", localChannelId);
            List<DailyTrendingSummary> remote = WebAPIHub.GetRecordsWhere(address, "DailyTrendingSummary", $"ChannelID = {remoteChannelId}").Select(x => (DailyTrendingSummary)x).ToList();

            // if there is a local record but not a remote record
            foreach (DailyTrendingSummary localRecord in local)
            {
                if (!remote.Where(x => x.Date == localRecord.Date).Any())
                {
                    DailyTrendingSummary record = new DailyTrendingSummary()
                    {
                        ChannelID = remoteChannelId,
                        Date = localRecord.Date,
                        Minimum = localRecord.Minimum,
                        Average = localRecord.Average,
                        Maximum = localRecord.Maximum,
                        ValidCount = localRecord.ValidCount,
                        InvalidCount = localRecord.InvalidCount
                    };
                    WebAPIHub.CreateRecord(address, "DailyTrendingSummary", JObject.FromObject(record));
                }
            }
        }

        private void SyncDailyQualityRangeLimit(string address, int localChannelId, int remoteChannelId)
        {
            // ensure remote and local line impedance matches
            DataQualityRangeLimit local = DataContext.Table<DataQualityRangeLimit>().QueryRecordWhere("ChannelID = {0}", localChannelId);
            DataQualityRangeLimit remote = WebAPIHub.GetRecordsWhere(address, "DataQualityRangeLimit", $"ChannelID = {remoteChannelId}").Select(x => (DataQualityRangeLimit)x).FirstOrDefault();

            // if there is a local record but not a remote record
            if (local != null && remote == null)
            {
                DataQualityRangeLimit record = new DataQualityRangeLimit()
                {
                    ChannelID = remoteChannelId,
                    High = local.High,
                    Low = local.Low,
                    RangeInclusive = local.RangeInclusive,
                    PerUnit = local.PerUnit,
                    Enabled = local.Enabled
                };

                WebAPIHub.CreateRecord(address, "DataQualityRangeLimit", JObject.FromObject(record));
            }
        }

        private void SyncEvents(string address, int localMeterId, int remoteMeterId, int localLineId, int remoteLineId)
        {
            // ensure remote and local line impedance matches
            IEnumerable<Event> local = DataContext.Table<Event>().QueryRecordsWhere("MeterID = {0} AND LineID = {1}", localMeterId, localLineId);
            List<Event> remote = WebAPIHub.GetRecordsWhere(address, "Event", $"MeterID = {remoteMeterId} AND LineID = {remoteLineId}").Select(x => (Event)x).ToList();

            // if there is a local record but not a remote record
            foreach (Event summary in local)
            {
                int eventTypeId = GetRemoteEventTypeId(address, summary.EventTypeID);
                int fileGroupId = GetRemoteFileGroup(address, summary.FileGroupID);
                int eventDataId = GetRemoteEventData(address, summary.EventDataID, fileGroupId);

                if (!remote.Where(x => x.EventTypeID == eventTypeId && x.StartTime.Equals(summary.StartTime) && x.EndTime.Equals(summary.EndTime)).Any())
                {
                    Event record = new Event()
                    {
                        FileGroupID = fileGroupId,
                        MeterID = remoteMeterId,
                        LineID = remoteLineId,
                        EventTypeID = eventTypeId,
                        EventDataID = eventDataId,
                        Name = summary.Name,
                        Alias = summary.Alias,
                        ShortName = summary.ShortName,
                        StartTime = summary.StartTime,
                        EndTime = summary.EndTime,
                        Samples = summary.Samples,
                        TimeZoneOffset = summary.TimeZoneOffset,
                        SamplesPerSecond = summary.SamplesPerSecond,
                        SamplesPerCycle = summary.SamplesPerCycle,
                        Description = summary.Description,
                        UpdatedBy = summary.UpdatedBy
                    };

                    int eventId = WebAPIHub.CreateRecord(address, "Event", JObject.FromObject(record));

                    GetBreakerOperation(address, summary.ID, eventId);
                    //GetCSAResult(summary.ID, eventId);
                    GetDisturbances(address, summary.ID, eventId);
                    GetFaultGroup(address, summary.ID, eventId);
                    GetFaultSegments(address, summary.ID, eventId);
                    GetFaultSummary(address, summary.ID, eventId);
                }
            }

        }

        private int GetRemoteEventTypeId(string address, int localEventTypeId)
        {
            IEnumerable<EventType> localTypes = DataContext.Table<EventType>().QueryRecords();
            List<EventType> remoteTypes = WebAPIHub.GetRecords(address, "EventType", "all").Select(x => (EventType)x).ToList();

            if (!remoteTypes.Where(x => x.Name == localTypes.Where(y => y.ID == localEventTypeId).First().Name).Any())
            {
                EventType record = new EventType()
                {
                    Name = localTypes.Where(y => y.ID == localEventTypeId).First().Name,
                    Description = localTypes.Where(y => y.ID == localEventTypeId).First().Description
                };
                return WebAPIHub.CreateRecord(address, "EventType", JObject.FromObject(record));
            }
            else
            {
                return remoteTypes.Where(x => x.Name == localTypes.Where(y => y.ID == localEventTypeId).First().Name).First().ID;
            }
        }

        private int GetRemoteFileGroup(string address, int localFileGroupId)
        {
            FileGroup local = DataContext.Table<FileGroup>().QueryRecordWhere("ID = {0}", localFileGroupId);
            List<FileGroup> remote = WebAPIHub.GetRecords(address, "FileGroup", "all").Select(x => (FileGroup)x).ToList();

            int id;
            if (local != null && !remote.Where(x => x.DataStartTime.Equals(local.DataStartTime) && x.DataEndTime.Equals(local.DataEndTime) && x.ProcessingStartTime.Equals(local.ProcessingStartTime) && x.ProcessingEndTime.Equals(local.ProcessingEndTime)).Any())
            {
                FileGroup record = new FileGroup()
                {
                    DataStartTime = local.DataStartTime,
                    DataEndTime = local.DataEndTime,
                    ProcessingStartTime = local.ProcessingStartTime,
                    ProcessingEndTime = local.ProcessingEndTime,
                    Error = local.Error
                };
                id = WebAPIHub.CreateRecord(address, "FileGroup", JObject.FromObject(record));
            }
            else
            {
                id = remote.Where(x => x.DataStartTime.Equals(local.DataStartTime) && x.DataEndTime.Equals(local.DataEndTime) && x.ProcessingStartTime.Equals(local.ProcessingStartTime) && x.ProcessingEndTime.Equals(local.ProcessingEndTime)).First().ID;
            }

            SyncDataFiles(address, localFileGroupId, id);

            return id;
        }

        private void SyncDataFiles(string address, int localFileGroupId, int remoteFileGroupId)
        {
            IEnumerable<DataFile> local = DataContext.Table<DataFile>().QueryRecordsWhere("FileGroupID = {0}", localFileGroupId);
            List<DataFile> remote = WebAPIHub.GetRecordsWhere(address, "DataFile", $"FileGroupID = {remoteFileGroupId}").Select(x => (DataFile)x).ToList();

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
                    int id = WebAPIHub.CreateRecord(address, "DataFile", JObject.FromObject(record));

                    SyncFileBlobs(address, localRecord.ID, id);
                }
            }
        }

        private void SyncFileBlobs(string address, int localDataFileId, int remoteDataFileId)
        {
            FileBlob local = DataContext.Table<FileBlob>().QueryRecordWhere("DataFileID = {0}", localDataFileId);
            FileBlob remote = (FileBlob)WebAPIHub.GetRecordsWhere(address, "FileBlob", $"DataFileID = {remoteDataFileId}").FirstOrDefault();

            if (local != null && remote == null)
            {
                FileBlob record = new FileBlob()
                {
                    DataFileID = remoteDataFileId,
                    Blob = local.Blob
                };
                WebAPIHub.CreateRecord(address, "FileBlob", JObject.FromObject(record));
            }
        }

        private int GetRemoteEventData(string address, int localEventDataId, int remoteFileGroup)
        {
            EventData local = DataContext.Table<EventData>().QueryRecordWhere("ID = {0}", localEventDataId);
            List<EventData> remote = WebAPIHub.GetRecordsWhere(address, "EventData", $"FileGroupID = {remoteFileGroup}").Select(x => (EventData)x).ToList();


            if (local != null && !remote.Where(x => x.RunTimeID == local.RunTimeID).Any())
            {
                EventData record = new EventData()
                {
                    FileGroupID = remoteFileGroup,
                    RunTimeID = local.RunTimeID,
                    TimeDomainData = local.TimeDomainData,
                    FrequencyDomainData = local.FrequencyDomainData,
                    MarkedForDeletion = local.MarkedForDeletion
                };
                return WebAPIHub.CreateRecord(address, "EventData", JObject.FromObject(record));
            }
            else
                return remote.Where(x => x.RunTimeID == local.RunTimeID).FirstOrDefault().ID;
        }

        private void GetBreakerOperation(string address, int localEventId, int remoteEventId)
        {
            IEnumerable<BreakerOperation> local = DataContext.Table<BreakerOperation>().QueryRecordsWhere("EventID = {0}", localEventId);
            List<BreakerOperation> remote = WebAPIHub.GetRecordsWhere(address, "BreakerOperation", $"EventID = {remoteEventId}").Select(x => (BreakerOperation)x).ToList();

            // if there is a local record but not a remote record
            foreach (BreakerOperation localRecord in local)
            {
                if (!remote.Where(x => x.TripCoilEnergized.Equals(localRecord.TripCoilEnergized)).Any())
                {
                    BreakerOperation record = new BreakerOperation()
                    {
                        EventID = remoteEventId,
                        PhaseID = GetRemotePhaseId(address, localRecord.PhaseID),
                        BreakerOperationTypeID = GetRemoteBreakerOperationType(address, localRecord.BreakerOperationTypeID),
                        BreakerNumber = localRecord.BreakerNumber,
                        TripCoilEnergized = localRecord.TripCoilEnergized,
                        StatusBitSet = localRecord.StatusBitSet,
                        APhaseCleared = localRecord.APhaseCleared,
                        BPhaseCleared = localRecord.BPhaseCleared,
                        CPhaseCleared = localRecord.CPhaseCleared,
                        BreakerTiming = localRecord.BreakerTiming,
                        StatusTiming = localRecord.StatusTiming,
                        APhaseBreakerTiming = localRecord.APhaseBreakerTiming,
                        BPhaseBreakerTiming = localRecord.BPhaseBreakerTiming,
                        CPhaseBreakerTiming = localRecord.CPhaseBreakerTiming,
                        BreakerSpeed = localRecord.BreakerSpeed,
                        UpdatedBy = localRecord.UpdatedBy,
                        StatusBitChatter = localRecord.StatusBitChatter
                    };
                    int id = WebAPIHub.CreateRecord(address, "BreakerOperation", JObject.FromObject(record));

                }
            }

        }

        private int GetRemotePhaseId(string address, int localPhaseId)
        {
            List<Phase> remote = WebAPIHub.GetRecords(address, "Phase", "all").Select(x => (Phase)x).ToList();
            Phase local = DataContext.Table<Phase>().QueryRecordWhere("ID = {0}", localPhaseId);
            if (remote.Where(x => x.Name == local.Name).Any())
                return remote.Where(x => x.Name == local.Name).First().ID;
            else
            {
                Phase record = new Phase()
                {
                    Name = local.Name,
                    Description = local.Description
                };

                return WebAPIHub.CreateRecord(address, "Phase", JObject.FromObject(record));
            }
        }

        private int GetRemoteBreakerOperationType(string address, int localBreakerOperationTypeId)
        {
            List<BreakerOperationType> remote = WebAPIHub.GetRecords(address, "BreakerOperationType", "all").Select(x => (BreakerOperationType)x).ToList();
            BreakerOperationType local = DataContext.Table<BreakerOperationType>().QueryRecordWhere("ID = {0}", localBreakerOperationTypeId);
            if (remote.Where(x => x.Name == local.Name).Any())
                return remote.Where(x => x.Name == local.Name).First().ID;
            else
            {
                BreakerOperationType record = new BreakerOperationType()
                {
                    Name = local.Name,
                    Description = local.Description
                };

                return WebAPIHub.CreateRecord(address, "BreakerOperationType", JObject.FromObject(record));
            }

        }

        private void GetCSAResult(string address, int localEventId, int remoteEventId)
        {
            CSAResult local = DataContext.Table<CSAResult>().QueryRecordWhere("EventID = {0}", localEventId);
            CSAResult remote = (CSAResult)WebAPIHub.GetRecordsWhere(address, "CSAResult", $"EventID = {remoteEventId}").FirstOrDefault();

            // if there is a local record but not a remote record
            if (remote == null && local != null)
            {
                CSAResult record = new CSAResult()
                {
                    EventID = remoteEventId,
                    IsDataError = local.IsDataError,
                    IsCapSwitch = local.IsCapSwitch,
                    IsCapSwitchCondL = local.IsCapSwitchCondL,
                    OutFrequency = local.OutFrequency,
                    OutVoltagesMax = local.OutVoltagesMax,
                    OutVoltagesMean = local.OutVoltagesMean,
                    OutQConditionRPBFlag = local.OutQConditionRPBFlag,
                    OutQConditionMRPC = local.OutQConditionMRPC,
                    OutQConditionRPCA = local.OutQConditionRPCA,
                    OutQConditionRPCB = local.OutQConditionRPCB,
                    OutQConditionRPCC = local.OutQConditionRPCC,
                    OutQConditionMPFI = local.OutQConditionMPFI,
                    OutQConditionPFA = local.OutQConditionPFA,
                    OutQConditionPFB = local.OutQConditionPFB,
                    OutQConditionPFC = local.OutQConditionPFC,
                    OutRestrikeFlag = local.OutRestrikeFlag,
                    OutRestrikeNum = local.OutRestrikeNum,
                    OutRestrikePHA = local.OutRestrikePHA,
                    OutRestrikePHB = local.OutRestrikePHB,
                    OutRestrikePHC = local.OutRestrikePHC,
                    OutVTHDFlag = local.OutVTHDFlag,
                    OutVTHDBefore = local.OutVTHDBefore,
                    OutVTHDAfter = local.OutVTHDAfter,
                    OutVTHDIncrease = local.OutVTHDIncrease
                };
                int id = WebAPIHub.CreateRecord(address, "CSAResult", JObject.FromObject(record));

            }

        }

        private void GetDisturbances(string address, int localEventId, int remoteEventId)
        {
            IEnumerable<Disturbance> local = DataContext.Table<Disturbance>().QueryRecordsWhere("EventID = {0}", localEventId);
            List<Disturbance> remote = WebAPIHub.GetRecordsWhere(address, "Disturbance", $"EventID = {remoteEventId}").Select(x => (Disturbance)x).ToList();

            // if there is a local record but not a remote record
            foreach (Disturbance localRecord in local)
            {
                int remoteEventTypeId = GetRemoteEventTypeId(address, localRecord.EventTypeID);

                if (!remote.Where(x => x.StartTime.Equals(localRecord.StartTime) && x.EndTime.Equals(localRecord.EndTime) && x.EventTypeID == remoteEventTypeId).Any())
                {
                    Disturbance record = new Disturbance()
                    {
                        EventID = remoteEventId,
                        PhaseID = GetRemotePhaseId(address, localRecord.PhaseID),
                        EventTypeID = remoteEventTypeId,
                        Magnitude = localRecord.Magnitude,
                        PerUnitMagnitude = localRecord.PerUnitMagnitude,
                        StartTime = localRecord.StartTime,
                        EndTime = localRecord.EndTime,
                        DurationSeconds = localRecord.DurationSeconds,
                        DurationCycles = localRecord.DurationCycles,
                        StartIndex = localRecord.StartIndex,
                        EndIndex = localRecord.EndIndex,
                        UpdatedBy = ""
                    };
                    int id = WebAPIHub.CreateRecord(address, "Disturbance", JObject.FromObject(record));
                    GetDisturbanceSeverity(address, localRecord.ID, id);
                }
            }
        }

        private void GetDisturbanceSeverity(string address, int localDisturbanceId, int remoteDisturbanceId)
        {
            DisturbanceSeverity local = DataContext.Table<DisturbanceSeverity>().QueryRecordWhere("DisturbanceID = {0}", localDisturbanceId);
            DisturbanceSeverity remote = (DisturbanceSeverity)WebAPIHub.GetRecordsWhere(address, "DisturbanceSeverity", $"DisturbanceID = {remoteDisturbanceId}").FirstOrDefault();

            // if there is a local record but not a remote record
            if (remote == null && local != null)
            {
                DisturbanceSeverity record = new DisturbanceSeverity()
                {
                    VoltageEnvelopeID = GetRemoteVoltageEnvelopeId(address, local.VoltageEnvelopeID),
                    DisturbanceID = remoteDisturbanceId,
                    SeverityCode = local.SeverityCode
                };
                int id = WebAPIHub.CreateRecord(address, "DisturbanceSeverity", JObject.FromObject(record));
            }
        }

        private int GetRemoteVoltageEnvelopeId(string address, int id)
        {
            VoltageEnvelope local = DataContext.Table<VoltageEnvelope>().QueryRecordWhere("ID = {0}",id);
            VoltageEnvelope remote = (VoltageEnvelope)WebAPIHub.GetRecordsWhere(address, "VoltageEnvelope", $"Name = {local.Name}").FirstOrDefault();

            if (remote == null)
            {
                VoltageEnvelope record = new VoltageEnvelope()
                {
                    Name = local.Name,
                    Description = local.Description
                };
                return WebAPIHub.CreateRecord(address, "VoltageEnvelope", JObject.FromObject(record));

            }
            return remote.ID;
        }

        private void GetFaultGroup(string address, int localEventId, int remoteEventId)
        {
            FaultGroup local = DataContext.Table<FaultGroup>().QueryRecordWhere("EventID = {0}", localEventId);
            FaultGroup remote = (FaultGroup)WebAPIHub.GetRecordsWhere(address, "FaultGroup", $"EventID = {remoteEventId}").FirstOrDefault();

            // if there is a local record but not a remote record
            if (remote == null && local != null)
            {
                FaultGroup record = new FaultGroup()
                {
                    EventID = remoteEventId,
                    FaultDetectionLogicResult = local.FaultDetectionLogicResult,
                    DefaultFaultDetectionLogicResult = local.DefaultFaultDetectionLogicResult,
                    FaultValidationLogicResult = local.FaultValidationLogicResult
                };
                int id = WebAPIHub.CreateRecord(address, "FaultGroup", JObject.FromObject(record));

            }

        }

        private void GetFaultSegments(string address, int localEventId, int remoteEventId)
        {
            IEnumerable<FaultSegment> local = DataContext.Table<FaultSegment>().QueryRecordsWhere("EventID = {0}", localEventId);
            List<FaultSegment> remote = WebAPIHub.GetRecordsWhere(address, "FaultSegment", $"EventID = {remoteEventId}").Select(x => (FaultSegment)x).ToList();

            // if there is a local record but not a remote record
            foreach (FaultSegment localRecord in local)
            {
                int remoteSegmentTypeId = GetRemoteSegmentTypeId(address, localRecord.SegmentTypeID);

                if (!remote.Where(x => x.StartTime.Equals(localRecord.StartTime) && x.EndTime.Equals(localRecord.EndTime) && x.SegmentTypeID == remoteSegmentTypeId).Any())
                {
                    FaultSegment record = new FaultSegment()
                    {
                        EventID = remoteEventId,
                        SegmentTypeID = remoteSegmentTypeId,
                        StartTime = localRecord.StartTime,
                        EndTime = localRecord.EndTime,
                        StartSample = localRecord.StartSample,
                        EndSample = localRecord.EndSample
                    };
                    int id = WebAPIHub.CreateRecord(address, "FaultSegment", JObject.FromObject(record));
                }
            }
        }

        private int GetRemoteSegmentTypeId(string address, int id)
        {
            SegmentType local = DataContext.Table<SegmentType>().QueryRecordWhere("ID = {0}", id);
            SegmentType remote = (SegmentType)WebAPIHub.GetRecordsWhere(address, "SegmentType", $"Name = '{local.Name}'").FirstOrDefault();

            if (remote == null)
            {
                SegmentType record = new SegmentType()
                {
                    Name = local.Name,
                    Description = local.Description
                };
                return WebAPIHub.CreateRecord(address, "SegmentType", JObject.FromObject(record));

            }
            return remote.ID;
        }

        private void GetFaultSummary(string address, int localEventId, int remoteEventId)
        {
            IEnumerable<FaultSummary> local = DataContext.Table<FaultSummary>().QueryRecordsWhere("EventID = {0}", localEventId);
            List<FaultSummary> remote = WebAPIHub.GetRecordsWhere(address, "FaultSummary", $"EventID = {remoteEventId}").Select(x => (FaultSummary)x).ToList();

            // if there is a local record but not a remote record
            foreach (FaultSummary localRecord in local)
            {
                if (!remote.Where(x => x.Inception.Equals(localRecord.Inception) && x.FaultType == localRecord.FaultType).Any())
                {
                    FaultSummary record = new FaultSummary()
                    {
                        EventID = remoteEventId,
                        Algorithm = localRecord.Algorithm,
                        FaultNumber = localRecord.FaultNumber,
                        CalculationCycle = localRecord.CalculationCycle,
                        Distance = localRecord.Distance,
                        CurrentMagnitude = localRecord.CurrentMagnitude,
                        CurrentLag = localRecord.CurrentLag,
                        PrefaultCurrent = localRecord.PrefaultCurrent,
                        PostfaultCurrent = localRecord.PostfaultCurrent,
                        Inception = localRecord.Inception,
                        DurationSeconds = localRecord.DurationSeconds,
                        DurationCycles = localRecord.DurationCycles,
                        FaultType = localRecord.FaultType,
                        IsSelectedAlgorithm = localRecord.IsSelectedAlgorithm,
                        IsValid = localRecord.IsValid,
                        IsSuppressed = localRecord.IsSuppressed
                    };
                    int id = WebAPIHub.CreateRecord(address, "FaultSummary", JObject.FromObject(record));
                }
            }
        }

        private void GetICFResult(string address, int localEventId, int remoteEventId)
        {
            ICFResult local = DataContext.Table<ICFResult>().QueryRecordWhere("EventID = {0}", localEventId);
            ICFResult remote = (ICFResult)WebAPIHub.GetRecordsWhere(address, "ICFResult", $"EventID = {remoteEventId}").FirstOrDefault();

            // if there is a local record but not a remote record
            if (remote == null && local != null)
            {
                ICFResult record = new ICFResult()
                {
                    EventID = remoteEventId,
                    IsDataError = local.IsDataError,
                    ICFState = local.ICFState,
                    ConfidenceLevel = local.ConfidenceLevel,
                    Phase = local.Phase,
                    EventCount = local.EventCount
                };
                int id = WebAPIHub.CreateRecord(address, "ICFResult", JObject.FromObject(record));

            }

        }

        private void GetICFEvent(string address, int localEventId, int remoteEventId)
        {
            ICFEvent local = DataContext.Table<ICFEvent>().QueryRecordWhere("ICFResultID = {0}", localEventId);
            ICFEvent remote = (ICFEvent)WebAPIHub.GetRecordsWhere(address, "ICFEvent", $"ICFResultID = {remoteEventId}").FirstOrDefault();

            // if there is a local record but not a remote record
            if (remote == null && local != null)
            {
                ICFEvent record = new ICFEvent()
                {
                    ICFResultID = remoteEventId,
                    PhaseAngle = local.PhaseAngle,
                    Time = local.Time,
                    DurationCycles = local.DurationCycles,
                    PeakFaultCurrent = local.PeakFaultCurrent
                };
                int id = WebAPIHub.CreateRecord(address, "ICFEvent", JObject.FromObject(record));

            }

        }

        private void Scheduler_Started(object sender, EventArgs e)
        {
            OnLogStatusMessage("DataPusher Scheduler has started successfully...");
        }

        private void Scheduler_Starting(object sender, EventArgs e)
        {
            OnLogStatusMessage("DataPusher Scheduler is starting...");
        }

        private void Scheduler_Disposed(object sender, EventArgs e)
        {
            OnLogStatusMessage("DataPusher Scheduler is disposed...");
        }


        private void Scheduler_ScheduleDue(object sender, EventArgs<Schedule> e)
        {
            OnLogStatusMessage(string.Format("DataPusher Scheduler: {0} schedule is due...", e.Argument.Name));
            int id = DataContext.Table<RemoteXDAInstance>().QueryRecordWhere("Name = {0}", e.Argument.Name).ID;
            SyncInstanceFiles(string.Empty, id);
        }
        #endregion
    }
}
