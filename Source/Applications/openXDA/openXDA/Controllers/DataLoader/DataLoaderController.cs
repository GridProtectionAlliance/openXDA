//******************************************************************************************************
//  DataLoaderController.cs - Gbtc
//
//  Copyright © 2026, Grid Protection Alliance.  All Rights Reserved.
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
//  04/03/2026 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using GSF.Configuration;
using GSF.Data;
using GSF.Data.Model;
using openXDA.Configuration;
using openXDA.Model;
using openXDA.Model.DataLoader;
using openXDA.Nodes;
using openXDA.Nodes.Types.Analysis;
using ConfigurationLoader = openXDA.Nodes.ConfigurationLoader;

namespace openXDA.Controllers.DataLoader
{
    [Authorize(Roles = "API")]
    [RoutePrefix("api/DataLoader")]
    public class DataLoaderController : ApiController
    {
        #region [ Members ]

        // Nested Types
        private class Settings
        {
            [Category]
            [SettingName(SystemSection.CategoryName)]
            public SystemSection SystemSettings { get; } = new SystemSection();
        }

        #endregion

        #region [ Constructors ]

        public DataLoaderController(Host nodeHost) =>
            NodeHost = nodeHost;

        #endregion

        #region [ Properties ]

        private Host NodeHost { get; }

        private DateTime XDANow
        {
            get
            {
                DateTime utc = DateTime.UtcNow;
                Settings settings = new Settings();
                ConfigurationLoader configurationLoader = new ConfigurationLoader(NodeHost.ID, CreateDbConnection);
                configurationLoader.Configure(settings);

                TimeZoneInfo xdaTimeZone = settings.SystemSettings.XDATimeZoneInfo;
                return TimeZoneInfo.ConvertTimeFromUtc(utc, xdaTimeZone);
            }
        }

        #endregion

        #region [ Methods ]

        [HttpGet, Route("GetMeter/{assetKey}")]
        public IHttpActionResult GetMeter(string assetKey)
        {
            using (AdoDataConnection connection = CreateDbConnection())
            {
                TableOperations<Meter> meterTable = new TableOperations<Meter>(connection);
                Meter meter = meterTable.QueryRecordWhere("AssetKey = {0}", assetKey);
                return meter is null ? (IHttpActionResult)NotFound() : Ok(meter);
            }
        }

        [HttpPost, Route("UploadMeter")]
        public Meter UploadMeter([FromBody] MeterDescriptor descriptor)
        {
            using (AdoDataConnection connection = CreateDbConnection())
            {
                TableOperations<Meter> meterTable = new TableOperations<Meter>(connection);
                Location location = GetOrUploadLocation(connection, descriptor.Location);
                Meter meter = UploadMeter(meterTable, location, descriptor);

                foreach (BusDescriptor busDescriptor in descriptor.Buses)
                {
                    Bus bus = GetOrUploadBus(connection, busDescriptor);
                    _ = GetOrUploadAssetLocation(connection, bus, meter.Location);
                    UploadMeterAsset(connection, meter, bus);
                }

                foreach (LineDescriptor lineDescriptor in descriptor.Lines)
                {
                    Line line = GetOrUploadLine(connection, lineDescriptor);
                    _ = GetOrUploadAssetLocation(connection, line, meter.Location);
                    UploadMeterAsset(connection, meter, line);
                }

                return meter;
            }
        }

        [HttpPost, Route("GetDataFile")]
        public IHttpActionResult GetDataFile([FromBody] string filePath)
        {
            using (AdoDataConnection connection = CreateDbConnection())
            {
                TableOperations<DataFile> dataFileTable = new TableOperations<DataFile>(connection);
                DataFile dataFile = GetDataFile(dataFileTable, filePath);
                return dataFile is null ? (IHttpActionResult)NotFound() : Ok(dataFile);
            }
        }

        [HttpPost, Route("GetFileData")]
        public IHttpActionResult GetFileData([FromBody] string filePath)
        {
            using (AdoDataConnection connection = CreateDbConnection())
            {
                TableOperations<DataFile> dataFileTable = new TableOperations<DataFile>(connection);
                TableOperations<FileBlob> fileBlobTable = new TableOperations<FileBlob>(connection);
                DataFile dataFile = GetDataFile(dataFileTable, filePath);

                if (dataFile is null)
                    return NotFound();

                FileBlob fileBlob = fileBlobTable.QueryRecordWhere("DataFileID = {0}", dataFile.ID);
                return fileBlob is null ? (IHttpActionResult)NotFound() : Ok(fileBlob.Blob);
            }
        }

        [HttpPost, Route("UploadFileGroup")]
        public async Task<IHttpActionResult> UploadFileGroup([FromBody] FileGroupDescriptor descriptor)
        {
            using (AdoDataConnection connection = CreateDbConnection())
            {
                TableOperations<FileGroup> fileGroupTable = new TableOperations<FileGroup>(connection);
                TableOperations<DataFile> dataFileTable = new TableOperations<DataFile>(connection);
                TableOperations<FileBlob> fileBlobTable = new TableOperations<FileBlob>(connection);

                List<DataFile> dataFiles = descriptor.DataFiles
                    .Select(dataFileDescriptor => dataFileDescriptor.FilePath)
                    .Select(filePath => GetDataFile(dataFileTable, filePath))
                    .ToList();

                bool isInvalid = dataFiles
                    .Where(dataFile => !(dataFile is null))
                    .Select(dataFile => dataFile.FileGroupID)
                    .Distinct()
                    .Skip(1)
                    .Any();

                if (isInvalid)
                    return UnprocessableContent("Existing data files belong to different file groups");

                int? fileGroupID = dataFiles
                    .Where(dataFile => !(dataFile is null))
                    .Select(dataFile => dataFile?.FileGroupID)
                    .FirstOrDefault();

                FileGroup fileGroup = fileGroupID is null
                    ? UploadFileGroup(fileGroupTable, descriptor)
                    : fileGroupTable.QueryRecordWhere("ID = {0}", fileGroupID.GetValueOrDefault());

                DateTime now = XDANow;

                for (int i = 0; i < dataFiles.Count; i++)
                {
                    DataFileDescriptor dataFileDescriptor = descriptor.DataFiles[i];

                    DataFile dataFile = dataFiles[i]
                        ?? UploadDataFile(dataFileTable, fileGroup, now, dataFileDescriptor);

                    UploadFileBlob(fileBlobTable, dataFile, dataFileDescriptor);
                }

                UploadAnalysisTask(connection, fileGroup);
            }

            await NotifyAnalysisNodesAsync();
            return Ok();
        }

        private AdoDataConnection CreateDbConnection() =>
            NodeHost.CreateDbConnection();

        private async Task NotifyAnalysisNodesAsync()
        {
            using (HttpResponseMessage response = await NodeHost.SendWebRequestAsync(ConfigureRequest))
            {
                response.EnsureSuccessStatusCode();
            }

            void ConfigureRequest(HttpRequestMessage request)
            {
                string url = NodeHost.BuildURL(typeof(AnalysisNode), "PollTaskQueue");
                request.Method = HttpMethod.Post;
                request.RequestUri = new Uri(url);
            }
        }

        private IHttpActionResult UnprocessableContent<T>(T value) =>
            Content((HttpStatusCode)422, value);

        #endregion

        #region [ Static ]

        // Static Methods
        private static Location GetOrUploadLocation(AdoDataConnection connection, LocationDescriptor locationDescriptor)
        {
            TableOperations<Location> locationTable = new TableOperations<Location>(connection);
            Location location = locationTable.QueryRecordWhere("LocationKey = {0}", locationDescriptor.LocationKey);

            if (!(location is null))
                return location;

            location = locationTable.NewRecord();
            location.LocationKey = locationDescriptor.LocationKey;
            location.Name = locationDescriptor.Name;
            location.Description = locationDescriptor.Description;

            if (locationDescriptor.Latitude.HasValue)
                location.Latitude = locationDescriptor.Latitude.GetValueOrDefault();

            if (locationDescriptor.Longitude.HasValue)
                location.Longitude = locationDescriptor.Longitude.GetValueOrDefault();

            locationTable.AddNewRecord(location);

            string idQuery = $"SELECT ID FROM {locationTable.TableName} WHERE LocationKey = {{0}}";
            location.ID = connection.ExecuteScalar<int>(idQuery, location.LocationKey);
            return location;
        }

        private static Meter UploadMeter(TableOperations<Meter> meterTable, Location location, MeterDescriptor meterDescriptor)
        {
            Meter meter = meterTable.NewRecord();
            meter.LocationID = location.ID;
            meter.AssetKey = meterDescriptor.AssetKey;
            meter.Name = meterDescriptor.Name;
            meter.Description = meterDescriptor.Description;
            meter.Make = meterDescriptor.Make;
            meter.Model = meterDescriptor.Model;
            meter.TimeZone = meterDescriptor.TimeZone;
            meterTable.AddNewRecord(meter);

            string idQuery = $"SELECT ID FROM {meterTable.TableName} WHERE AssetKey = {{0}}";
            meter.ID = meterTable.Connection.ExecuteScalar<int>(idQuery, meterDescriptor.AssetKey);
            meter.Location = location;
            return meter;
        }

        private static Bus GetOrUploadBus(AdoDataConnection connection, BusDescriptor busDescriptor)
        {
            TableOperations<Bus> busTable = new TableOperations<Bus>(connection);
            Bus bus = busTable.QueryRecordWhere("AssetKey = {0}", busDescriptor.AssetKey);

            if (!(bus is null))
                return bus;

            bus = busTable.NewRecord();
            bus.AssetKey = busDescriptor.AssetKey;
            bus.AssetName = busDescriptor.AssetName;

            if (busDescriptor.VoltageKV.HasValue)
                bus.VoltageKV = busDescriptor.VoltageKV.GetValueOrDefault();

            busTable.AddNewRecord(bus);

            string idQuery = $"SELECT ID FROM {busTable.TableName} WHERE AssetKey = {{0}}";
            bus.ID = connection.ExecuteScalar<int>(idQuery, bus.AssetKey);
            return bus;
        }

        private static Line GetOrUploadLine(AdoDataConnection connection, LineDescriptor lineDescriptor)
        {
            TableOperations<Line> lineTable = new TableOperations<Line>(connection);
            Line line = lineTable.QueryRecordWhere("AssetKey = {0}", lineDescriptor.AssetKey);

            if (!(line is null))
                return line;

            line = lineTable.NewRecord();
            line.AssetKey = lineDescriptor.AssetKey;
            line.AssetName = lineDescriptor.AssetName;

            if (lineDescriptor.VoltageKV.HasValue)
                line.VoltageKV = lineDescriptor.VoltageKV.GetValueOrDefault();

            lineTable.AddNewRecord(line);

            string idQuery = $"SELECT ID FROM {lineTable.TableName} WHERE AssetKey = {{0}}";
            line.ID = connection.ExecuteScalar<int>(idQuery, line.AssetKey);

            LineSegment lineSegment = UploadLineSegment(connection, lineDescriptor);
            UploadAssetConnection(connection, line, lineSegment);
            return line;
        }

        private static AssetLocation GetOrUploadAssetLocation(AdoDataConnection connection, Asset asset, Location location)
        {
            TableOperations<AssetLocation> assetLocationTable = new TableOperations<AssetLocation>(connection);
            AssetLocation assetLocation = assetLocationTable.QueryRecordWhere("AssetID = {0} AND LocationID = {1}", asset.ID, location.ID);

            if (!(assetLocation is null))
                return assetLocation;

            assetLocation = assetLocationTable.NewRecord();
            assetLocation.AssetID = asset.ID;
            assetLocation.LocationID = location.ID;
            assetLocationTable.AddNewRecord(assetLocation);

            string idQuery = $"SELECT ID FROM {assetLocationTable.TableName} WHERE AssetID = {{0}} AND LocationID = {{1}}";
            assetLocation.ID = connection.ExecuteScalar<int>(idQuery, asset.ID, location.ID);
            return assetLocation;
        }

        private static void UploadMeterAsset(AdoDataConnection connection, Meter meter, Asset asset)
        {
            TableOperations<MeterAsset> meterAssetTable = new TableOperations<MeterAsset>(connection);
            MeterAsset meterAsset = meterAssetTable.NewRecord();
            meterAsset.MeterID = meter.ID;
            meterAsset.AssetID = asset.ID;
            meterAssetTable.AddNewRecord(meterAsset);
        }

        private static LineSegment UploadLineSegment(AdoDataConnection connection, LineDescriptor lineDescriptor)
        {
            TableOperations<LineSegment> lineSegmentTable = new TableOperations<LineSegment>(connection);
            LineSegment lineSegment = lineSegmentTable.NewRecord();
            lineSegment.AssetKey = $"{lineDescriptor.AssetKey}-Segment";
            lineSegment.IsEnd = true;

            if (lineDescriptor.Length.HasValue)
                lineSegment.Length = lineDescriptor.Length.GetValueOrDefault();

            if (lineDescriptor.R0.HasValue)
                lineSegment.R0 = lineDescriptor.R0.GetValueOrDefault();

            if (lineDescriptor.X0.HasValue)
                lineSegment.X0 = lineDescriptor.X0.GetValueOrDefault();

            if (lineDescriptor.R1.HasValue)
                lineSegment.R1 = lineDescriptor.R1.GetValueOrDefault();

            if (lineDescriptor.X1.HasValue)
                lineSegment.X1 = lineDescriptor.X1.GetValueOrDefault();

            if (lineDescriptor.ThermalRating.HasValue)
                lineSegment.ThermalRating = lineDescriptor.ThermalRating.GetValueOrDefault();

            lineSegmentTable.AddNewRecord(lineSegment);

            string idQuery = $"SELECT ID FROM {lineSegmentTable.TableName} WHERE AssetKey = {{0}}";
            lineSegment.ID = connection.ExecuteScalar<int>(idQuery, lineSegment.AssetKey);
            return lineSegment;
        }

        private static void UploadAssetConnection(AdoDataConnection connection, Line parent, LineSegment child)
        {
            string assetRelationshipTypeQuery = "SELECT ID FROM AssetRelationshipType WHERE Name = 'Line-LineSegment'";
            int assetRelationshipTypeID = connection.ExecuteScalar<int>(assetRelationshipTypeQuery);
            TableOperations<AssetConnection> assetConnectionTable = new TableOperations<AssetConnection>(connection);
            AssetConnection assetConnection = assetConnectionTable.NewRecord();
            assetConnection.AssetRelationshipTypeID = assetRelationshipTypeID;
            assetConnection.ParentID = parent.ID;
            assetConnection.ChildID = child.ID;
            assetConnectionTable.AddNewRecord(assetConnection);
        }

        private static DataFile GetDataFile(TableOperations<DataFile> dataFileTable, string filePath)
        {
            int hashCode = DataFile.GetHash(filePath);

            return dataFileTable
                .QueryRecordsWhere("FilePathHash = {0}", hashCode)
                .SingleOrDefault(dataFile => dataFile.FilePath == filePath);
        }

        private static FileGroup UploadFileGroup(TableOperations<FileGroup> fileGroupTable, FileGroupDescriptor fileGroupDescriptor)
        {
            FileGroup fileGroup = fileGroupTable.NewRecord();
            fileGroup.MeterID = fileGroupDescriptor.MeterID;
            fileGroupTable.AddNewRecord(fileGroup);

            string idQuery = $"SELECT @@IDENTITY";
            fileGroup.ID = fileGroupTable.Connection.ExecuteScalar<int>(idQuery);
            return fileGroup;
        }

        private static DataFile UploadDataFile(TableOperations<DataFile> dataFileTable, FileGroup fileGroup, DateTime creationTime, DataFileDescriptor dataFileDescriptor)
        {
            DataFile dataFile = dataFileTable.NewRecord();
            dataFile.FileGroupID = fileGroup.ID;
            dataFile.FilePath = dataFileDescriptor.FilePath;
            dataFile.FilePathHash = DataFile.GetHash(dataFile.FilePath);
            dataFile.FileSize = dataFileDescriptor.FileData.Length;
            dataFile.CreationTime = creationTime;
            dataFile.LastWriteTime = creationTime;
            dataFile.LastAccessTime = creationTime;
            dataFileTable.AddNewRecord(dataFile);

            string idQuery = $"SELECT @@IDENTITY";
            dataFile.ID = dataFileTable.Connection.ExecuteScalar<int>(idQuery);
            return dataFile;
        }

        private static void UploadFileBlob(TableOperations<FileBlob> fileBlobTable, DataFile dataFile, DataFileDescriptor dataFileDescriptor)
        {
            FileBlob fileBlob =
                fileBlobTable.QueryRecordWhere("DataFileID = {0}", dataFile.ID) ??
                fileBlobTable.NewRecord();

            fileBlob.DataFileID = dataFile.ID;
            fileBlob.Blob = dataFileDescriptor.FileData;
            fileBlobTable.AddNewOrUpdateRecord(fileBlob);
        }

        private static void UploadAnalysisTask(AdoDataConnection connection, FileGroup fileGroup)
        {
            const int FileWatcherPriority = 2;
            TableOperations<AnalysisTask> analysisTaskTable = new TableOperations<AnalysisTask>(connection);
            AnalysisTask analysisTask = analysisTaskTable.NewRecord();
            analysisTask.FileGroupID = fileGroup.ID;
            analysisTask.MeterID = fileGroup.MeterID;
            analysisTask.Priority = FileWatcherPriority;
            analysisTaskTable.AddNewRecord(analysisTask);
        }

        #endregion
    }
}
