//******************************************************************************************************
//  DataPusherController.cs - Gbtc
//
//  Copyright © 2019, Grid Protection Alliance.  All Rights Reserved.
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
//  12/13/2019 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using GSF.Data;
using GSF.Data.Model;
using log4net;
using openXDA.Configuration;
using openXDA.DataPusher;
using openXDA.Model;
using openXDA.Nodes;
using openXDA.XMLConfig;

namespace openXDA.Controllers.Config
{
    [RoutePrefix("api/DataPusher")]
    public class DataPusherController : ApiController
    {
        private Func<AdoDataConnection> ConnectionFactory { get; }
        private Host NodeHost { get; }

        public DataPusherController(Host host)
        {
            ConnectionFactory = () => host.CreateDbConnection();
            NodeHost = host;
        }

        // TODO: Misspelled route is deprecated and should be removed when typos are fixed downstream
        [Route("Recieve/XML"), Route("Receive/XML"), HttpPost, HttpEditionFilter(Edition.Enterprise)]
        public async Task<IHttpActionResult> ReceiveXML()
        {
            using (AdoDataConnection connection = ConnectionFactory())
            {
                try
                {
                    Stream stream = await Request.Content.ReadAsStreamAsync();
                    Loader loader = new Loader(connection.Connection.ConnectionString, $"AssemblyName={{{connection.AdapterType.Assembly.FullName}}}; ConnectionType={connection.Connection.GetType().FullName}; AdapterType={connection.AdapterType.FullName}");
                    await loader.LoadAsync(stream);
                    return Ok(loader.ConnectionID);
                }
                catch (Exception ex)
                {
                    Log.Error(ex.Message, ex);
                    return InternalServerError(ex);
                }
            }
        }

        [Route("Send/XML/{instanceId:int}/{meterId:int}"), HttpGet, HttpEditionFilter(Edition.Enterprise)]
        public async Task<IHttpActionResult> SendMeterConfigurationForInstance(int instanceId, int meterId)
        {
            using (AdoDataConnection connection = ConnectionFactory())
            {
                try
                {
                    DataPusherRequester requester = new DataPusherRequester(instanceId, connection);
                    MetersToDataPush meter = new TableOperations<MetersToDataPush>(connection).QueryRecordWhere("ID = {0}", meterId);
                    Producer producer = new Producer(connection.Connection.ConnectionString, $"AssemblyName={{{connection.AdapterType.Assembly.FullName}}}; ConnectionType={connection.Connection.GetType().FullName}; AdapterType={connection.AdapterType.FullName}");
                    Stream stream = producer.Get(instanceId, new List<int> { meter.LocalXDAMeterID });
                    stream.Seek(0, SeekOrigin.Begin);
                    HttpContent httpContent = new StreamContent(stream);

                    using (HttpResponseMessage response = await requester.SendRequestAsync("api/DataPusher/Receive/XML", HttpMethod.Post, httpContent, "application/text"))
                    {
                        response.EnsureSuccessStatusCode();
                        string connectionID = await response.Content.ReadAsStringAsync();
                        connectionID = connectionID.Replace("\"", "");
                        connection.ExecuteNonQuery("UPDATE MetersToDataPush SET Synced = 1 WHERE ID = {0}", meterId);
                        return Ok(connectionID);
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex.Message, ex);
                    return InternalServerError(ex);
                }
            }
        }

        [Route("Send/XML/{instanceId:int}"), HttpGet, HttpEditionFilter(Edition.Enterprise)]
        public async Task<IHttpActionResult> SendMeterConfigurationForInstance(int instanceId)
        {
            using (AdoDataConnection connection = ConnectionFactory())
            {
                try
                {
                    DataPusherRequester requester = new DataPusherRequester(instanceId, connection);
                    Producer producer = new Producer(connection.Connection.ConnectionString, $"AssemblyName={{{connection.AdapterType.Assembly.FullName}}}; ConnectionType={connection.Connection.GetType().FullName}; AdapterType={connection.AdapterType.FullName}");
                    Stream stream = producer.Get(instanceId, new List<int> { });
                    stream.Seek(0, SeekOrigin.Begin);
                    HttpContent httpContent = new StreamContent(stream);

                    using (HttpResponseMessage response = await requester.SendRequestAsync("api/DataPusher/Receive/XML", HttpMethod.Post, httpContent, "application/text"))
                    {
                        response.EnsureSuccessStatusCode();
                        string connectionID = await response.Content.ReadAsStringAsync();
                        connectionID = connectionID.Replace("\"", "");
                        connection.ExecuteNonQuery("UPDATE MetersToDataPush SET Synced = 1 WHERE RemoteXDAInstanceID = {0}", instanceId);
                        return Ok(connectionID);
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex.Message, ex);
                    return InternalServerError(ex);

                }
            }

        }

        [Route("LoaderStatus/XML/{connectionID}"), HttpGet, HttpEditionFilter(Edition.Enterprise)]
        public IHttpActionResult GetStatus(string connectionID)
        {
            try
            {
                Guid guid = Guid.Parse(connectionID);
                LoaderStatus status = Loader.RetrieveStatus(guid);
                return Ok(status);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                return InternalServerError(ex);
            }
        }

        [Route("LoaderStatus/XML/{instanceId:int}/{connectionID}"), HttpGet, HttpEditionFilter(Edition.Enterprise)]
        public async Task<IHttpActionResult> GetStatus(int instanceId, string connectionID)
        {
            using (AdoDataConnection connection = ConnectionFactory())
            {
                try
                {
                    DataPusherRequester requester = new DataPusherRequester(instanceId, connection);

                    using (HttpResponseMessage response = await requester.SendRequestAsync($"api/DataPusher/LoaderStatus/XML/{connectionID}", HttpMethod.Get))
                    {
                        response.EnsureSuccessStatusCode();
                        string result = await response.Content.ReadAsStringAsync();
                        return Ok(result);
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex.Message, ex);
                    return InternalServerError(ex);
                }
            }

        }

        [Route("LoaderStatus/XML"), HttpGet, HttpEditionFilter(Edition.Enterprise)]
        public IHttpActionResult InstantiateStatus()
        {
            try
            {
                Guid guid = Loader.InstantiateConnection();
                return Ok(guid);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                return InternalServerError(ex);
            }
        }

        [Route("Send/Files/{instanceId:int}/{meterId:int}/{fileGroupID:int}"), HttpGet, HttpEditionFilter(Edition.Enterprise)]
        public async Task<IHttpActionResult> SendFiles(int instanceId, int meterId, int fileGroupID)
        {
            try
            {
                DataPusherEngine engine = new DataPusherEngine(ConnectionFactory);
                await engine.SendFilesAsync(instanceId, meterId, fileGroupID);
                return Ok("Completed sycning file.");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        private IDbDataParameter ToDateTime2(AdoDataConnection connection, DateTime dateTime)
        {
            using (IDbCommand command = connection.Connection.CreateCommand())
            {
                IDbDataParameter parameter = command.CreateParameter();
                parameter.DbType = DbType.DateTime2;
                parameter.Value = dateTime;
                return parameter;
            }
        }

        // TODO: Misspelled route is deprecated and should be removed when typos are fixed downstream
        [Route("Recieve/Files"), Route("Receive/Files"), HttpPost, HttpEditionFilter(Edition.Enterprise)]
        public async Task<IHttpActionResult> ReceiveFiles()
        {
            using (AdoDataConnection connection = ConnectionFactory())
            {
                try
                {
                    Stream stream = await Request.Content.ReadAsStreamAsync();
                    FileGroupPost fileGroupPost = JsonSerializer.Deserialize<FileGroupPost>(stream);

                    Meter meter = new TableOperations<Meter>(connection).QueryRecordWhere("AssetKey = {0}", fileGroupPost.MeterKey)
                        ?? throw new Exception($"{fileGroupPost.MeterKey} is not defined in database");

                    FileGroup fileGroup = new TableOperations<FileGroup>(connection).QueryRecordWhere("MeterID = {0} AND DataStartTime = {1} AND DataEndtime = {2}", meter.ID, ToDateTime2(connection, fileGroupPost.FileGroup.DataStartTime), ToDateTime2(connection, fileGroupPost.FileGroup.DataEndTime));
                    if (fileGroup is null)
                    {
                        fileGroup = fileGroupPost.FileGroup;
                        fileGroup.ID = 0;
                        fileGroup.MeterID = meter.ID;

                        new TableOperations<FileGroup>(connection).AddNewRecord(fileGroup);
                        fileGroup = new TableOperations<FileGroup>(connection).QueryRecordWhere("MeterID = {0} AND DataStartTime = {1} AND DataEndtime = {2}", meter.ID, ToDateTime2(connection, fileGroupPost.FileGroup.DataStartTime), ToDateTime2(connection, fileGroupPost.FileGroup.DataEndTime));
                    }

                    foreach (DataFile file in fileGroupPost.DataFiles)
                    {
                        DataFile dataFile = new TableOperations<DataFile>(connection).QueryRecordWhere("FileGroupID = {0} AND FilePath = {1} AND FilePathHash = {2} AND FileSize = {3}", fileGroup.ID, file.FilePath, file.FilePathHash, file.FileSize);
                        FileBlob blob = fileGroupPost.FileBlobs.Find(x => x.DataFileID == file.ID);

                        if (dataFile is null)
                        {
                            dataFile = file;
                            dataFile.ID = 0;
                            dataFile.FileGroupID = fileGroup.ID;
                            new TableOperations<DataFile>(connection).AddNewRecord(dataFile);
                            dataFile = new TableOperations<DataFile>(connection).QueryRecordWhere("FileGroupID = {0} AND FilePath = {1} AND FilePathHash = {2} AND FileSize = {3}", fileGroup.ID, file.FilePath, file.FilePathHash, file.FileSize);
                        }

                        FileBlob fileBlob = new TableOperations<FileBlob>(connection).QueryRecordWhere("DataFileID = {0}", dataFile.ID);
                        if (fileBlob is null)
                        {
                            fileBlob = blob;
                            fileBlob.ID = 0;
                            fileBlob.DataFileID = dataFile.ID;

                            new TableOperations<FileBlob>(connection).AddNewRecord(fileBlob);
                        }
                    }

                    DataFileController dataFileController = new DataFileController(NodeHost);
                    await dataFileController.ReprocessFile(fileGroup.ID);

                    return Ok(fileGroup.ID);
                }
                catch (Exception ex)
                {
                    Log.Error(ex.Message, ex);
                    return InternalServerError(ex);
                }
            }
        }

        [Route("SyncMeterConfig/{connectionId}/{instanceId:int}/{meterId:int}"), HttpGet, HttpEditionFilter(Edition.Enterprise)]
        public Task SyncMeterConfigurationForInstance(string connectionId, int instanceId, int meterId, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                using (AdoDataConnection connection = ConnectionFactory())
                {
                    try
                    {
                        // for now, create new instance of DataPusherEngine.  Later have one running in XDA ServiceHost and tie to it to ensure multiple updates arent happening simultaneously
                        DataPusherEngine engine = new DataPusherEngine(() => new AdoDataConnection("systemSettings"));
                        RemoteXDAInstance instance = new TableOperations<RemoteXDAInstance>(connection).QueryRecordWhere("ID = {0}", instanceId);
                        MetersToDataPush meter = new TableOperations<MetersToDataPush>(connection).QueryRecordWhere("ID = {0}", meterId);
                        DataPusherRequester requester = new DataPusherRequester(instance);
                        engine.SyncMeterConfigurationForInstance(connectionId, instance, meter, requester, cancellationToken);
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex.Message, ex);
                    }
                }
            }, cancellationToken);
        }

        [Route("SyncMeterFiles/{connectionId}/{instanceId:int}/{meterId:int}"), HttpGet, HttpEditionFilter(Edition.Enterprise)]
        public Task SyncMeterFilesForInstance(string connectionId, int instanceId, int meterId, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                using (AdoDataConnection connection = ConnectionFactory())
                {
                    try
                    {
                        // for now, create new instance of DataPusherEngine.  Later have one running in XDA ServiceHost and tie to it to ensure multiple updates arent happening simultaneously
                        DataPusherEngine engine = new DataPusherEngine(() => new AdoDataConnection("systemSettings"));
                        RemoteXDAInstance instance = new TableOperations<RemoteXDAInstance>(connection).QueryRecordWhere("ID = {0}", instanceId);
                        engine.SyncMeterFilesForInstance(connectionId, instance, meterId, cancellationToken);
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex.Message, ex);
                    }
                }
            }, cancellationToken);
        }

        [Route("SyncInstanceConfig/{connectionId}/{instanceId:int}"), HttpGet, HttpEditionFilter(Edition.Enterprise)]
        public Task SyncInstanceConfiguration(string connectionId, int instanceId, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                // for now, create new instance of DataPusherEngine.  Later have one running in XDA ServiceHost and tie to it to ensure multiple updates arent happening simultaneously
                DataPusherEngine engine = new DataPusherEngine(() => new AdoDataConnection("systemSettings"));
                engine.SyncInstanceConfiguration(connectionId, instanceId, cancellationToken);
            }, cancellationToken);
        }

        [Route("TestConnection/{instanceId:int}"), HttpGet, HttpEditionFilter(Edition.Enterprise)]
        public IHttpActionResult TestRemoteInstanceConnection(int instanceId)
        {
            // for now, create new instance of DataPusherEngine.  Later have one running in XDA ServiceHost and tie to it to ensure multiple updates arent happening simultaneously
            DataPusherEngine engine = new DataPusherEngine(() => new AdoDataConnection("systemSettings"));
            (bool success, Exception ex) = engine.TestInstance(instanceId);

            return Ok(new
            {
                Success = success,
                ErrorMessage = GetErrorMessage(ex)
            });
        }

        private static string GetErrorMessage(Exception ex)
        {
            IEnumerable<Exception> unwrappedExceptions = Unwrap(ex);
            IEnumerable<string> messages = unwrappedExceptions.Select((e, l) => l == 0 ? e.Message : $"([{l}] {e.Message})");
            return string.Join(" ", messages);
        }

        private static IEnumerable<Exception> Unwrap(Exception ex)
        {
            while (!(ex is null))
            {
                yield return ex;
                ex = ex.InnerException;
            }
        }

        private static readonly ILog Log = LogManager.GetLogger(typeof(DataPusherController));
    }
}