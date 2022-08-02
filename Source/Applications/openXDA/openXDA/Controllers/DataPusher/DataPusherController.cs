﻿//******************************************************************************************************
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
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using GSF.Data;
using GSF.Data.Model;
using GSF.Security.Model;
using GSF.Web;
using log4net;
using openXDA.Controllers.Helpers;
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

        [Route("Recieve/XML"), HttpPost]
        public IHttpActionResult RecieveXML( CancellationToken cancellationToken)
        {
            if (User.IsInRole("DataPusher"))
            {
                using (AdoDataConnection connection = ConnectionFactory())
                {

                    try
                    {
                        Stream stream = Request.Content.ReadAsStreamAsync().Result;
                        Loader loader = new Loader(connection.Connection.ConnectionString, $"AssemblyName={{{connection.AdapterType.Assembly.FullName}}}; ConnectionType={connection.Connection.GetType().FullName}; AdapterType={connection.AdapterType.FullName}");
                        loader.Load(stream);
                        return Ok(loader.ConnectionID);
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex.Message, ex);
                        return InternalServerError(ex);
                    }
                }
            }
            else
                return Unauthorized();

        }

        [Route("Send/XML/{instanceId:int}/{meterId:int}"), HttpGet]
        public IHttpActionResult SendMeterConfigurationForInstance( int instanceId, int meterId, CancellationToken cancellationToken)
        {
            using (AdoDataConnection connection = ConnectionFactory())
            {

                try
                {
                    //// for now, create new instance of DataPusherEngine.  Later have one running in XDA ServiceHost and tie to it to ensure multiple updates arent happening simultaneously
                    RemoteXDAInstance instance = new TableOperations<RemoteXDAInstance>(connection).QueryRecordWhere("ID = {0}", instanceId);
                    MetersToDataPush meter = new TableOperations<MetersToDataPush>(connection).QueryRecordWhere("ID = {0}", meterId);
                    UserAccount userAccount = new TableOperations<UserAccount>(connection).QueryRecordWhere("ID = {0}", instance.UserAccountID);
                    string antiForgeryToken = ControllerHelpers.GenerateAntiForgeryToken(instance.Address, userAccount);

                    Producer producer = new Producer(connection.Connection.ConnectionString, $"AssemblyName={{{connection.AdapterType.Assembly.FullName}}}; ConnectionType={connection.Connection.GetType().FullName}; AdapterType={connection.AdapterType.FullName}");
                    Stream stream = producer.Get(instanceId, new List<int> { meter.LocalXDAMeterID });
                    stream.Seek(0, SeekOrigin.Begin);
                    using (WebRequestHandler handler = new WebRequestHandler())
                    using (HttpClient client = new HttpClient(handler))
                    {
                        handler.ServerCertificateValidationCallback += ControllerHelpers.HandleCertificateValidation;

                        client.BaseAddress = new Uri(instance.Address);
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/text"));
                        client.DefaultRequestHeaders.Add("X-GSF-Verify", antiForgeryToken);
                        client.AddBasicAuthenticationHeader(userAccount.AccountName, userAccount.Password);

                        HttpContent httpContent = new StreamContent(stream);
                        HttpResponseMessage response = client.PostAsync($"api/DataPusher/Recieve/XML", httpContent).Result;
                        string connectionID = response.Content.ReadAsStringAsync().Result;
                        connectionID = connectionID.Replace("\"", "");
                        if (!response.IsSuccessStatusCode)
                            throw new InvalidOperationException($"Server returned status code {response.StatusCode}: {response.ReasonPhrase}");

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

        [Route("Send/XML/{instanceId:int}"), HttpGet]
        public IHttpActionResult SendMeterConfigurationForInstance(int instanceId, CancellationToken cancellationToken)
        {
            using (AdoDataConnection connection = ConnectionFactory())
            {

                try
                {
                    //// for now, create new instance of DataPusherEngine.  Later have one running in XDA ServiceHost and tie to it to ensure multiple updates arent happening simultaneously
                    RemoteXDAInstance instance = new TableOperations<RemoteXDAInstance>(connection).QueryRecordWhere("ID = {0}", instanceId);
                    UserAccount userAccount = new TableOperations<UserAccount>(connection).QueryRecordWhere("ID = {0}", instance.UserAccountID);
                    string antiForgeryToken = ControllerHelpers.GenerateAntiForgeryToken(instance.Address, userAccount);


                    Producer producer = new Producer(connection.Connection.ConnectionString, $"AssemblyName={{{connection.AdapterType.Assembly.FullName}}}; ConnectionType={connection.Connection.GetType().FullName}; AdapterType={connection.AdapterType.FullName}");
                    Stream stream = producer.Get(instanceId, new List<int> { });
                    stream.Seek(0, SeekOrigin.Begin);
                    using (WebRequestHandler handler = new WebRequestHandler())
                    using (HttpClient client = new HttpClient(handler))
                    {
                        handler.ServerCertificateValidationCallback += ControllerHelpers.HandleCertificateValidation;

                        client.BaseAddress = new Uri(instance.Address);
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/text"));
                        client.DefaultRequestHeaders.Add("X-GSF-Verify", antiForgeryToken);
                        client.AddBasicAuthenticationHeader(userAccount.AccountName, userAccount.Password);

                        HttpContent httpContent = new StreamContent(stream);
                        HttpResponseMessage response = client.PostAsync($"api/DataPusher/Recieve/XML", httpContent).Result;

                        string connectionID = response.Content.ReadAsStringAsync().Result;
                        connectionID = connectionID.Replace("\"", "");

                        if (!response.IsSuccessStatusCode)
                            throw new InvalidOperationException($"Server returned status code {response.StatusCode}: {response.ReasonPhrase}");

                        connection.ExecuteNonQuery("UPDATE MetersToDataPush SET Synced = 1 WHERE RemoteXDAInstanceID = {0}", instance.ID);
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

        [Route("LoaderStatus/XML/{connectionID}"), HttpGet]
        public IHttpActionResult GetStatus(string connectionID) {
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

        [Route("LoaderStatus/XML/{instanceId:int}/{connectionID}"), HttpGet]
        public IHttpActionResult GetStatus(int instanceId, string connectionID)
        {
            using (AdoDataConnection connection = ConnectionFactory())
            {

                try
                {
                    //// for now, create new instance of DataPusherEngine.  Later have one running in XDA ServiceHost and tie to it to ensure multiple updates arent happening simultaneously
                    RemoteXDAInstance instance = new TableOperations<RemoteXDAInstance>(connection).QueryRecordWhere("ID = {0}", instanceId);
                    UserAccount userAccount = new TableOperations<UserAccount>(connection).QueryRecordWhere("ID = {0}", instance.UserAccountID);
                    string antiForgeryToken = ControllerHelpers.GenerateAntiForgeryToken(instance.Address, userAccount);

                    using (WebRequestHandler handler = new WebRequestHandler())
                    using (HttpClient client = new HttpClient(handler))
                    {
                        handler.ServerCertificateValidationCallback += ControllerHelpers.HandleCertificateValidation;

                        client.BaseAddress = new Uri(instance.Address);
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        client.DefaultRequestHeaders.Add("X-GSF-Verify", antiForgeryToken);
                        client.AddBasicAuthenticationHeader(userAccount.AccountName, userAccount.Password);

                        HttpResponseMessage response = client.GetAsync($"api/DataPusher/LoaderStatus/XML/{connectionID}").Result;
                        string result = response.Content.ReadAsStringAsync().Result;
                        if (!response.IsSuccessStatusCode)
                            throw new InvalidOperationException($"Server returned status code {response.StatusCode}: {response.ReasonPhrase}");

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

        [Route("LoaderStatus/XML"), HttpGet]
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

        [Route("Send/Files/{instanceId:int}/{meterId:int}/{fileGroupID:int}"), HttpGet]
        public IHttpActionResult SendFiles(int instanceId, int meterId, int fileGroupID, CancellationToken cancellationToken)
        {
            try
            {
                DataPusherEngine engine = new DataPusherEngine(ConnectionFactory);
                engine.SendFiles(instanceId, meterId, fileGroupID);
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

        [Route("Recieve/Files"), HttpPost]
        public IHttpActionResult RecieveFiles(CancellationToken cancellationToken)
        {
            if (User.IsInRole("DataPusher"))
            {
                using (AdoDataConnection connection = ConnectionFactory())
                {

                    try
                    {
                        Stream stream = Request.Content.ReadAsStreamAsync().Result;
                        BinaryFormatter binaryFormatter = new BinaryFormatter();
                        FileGroupPost fileGroupPost = (FileGroupPost)binaryFormatter.Deserialize(stream);

                        Meter meter = new TableOperations<Meter>(connection).QueryRecordWhere("AssetKey = {0}", fileGroupPost.MeterKey);
                        if (meter == null) throw new Exception($"{fileGroupPost.MeterKey} is not defined in database");

                        FileGroup fileGroup = new TableOperations<FileGroup>(connection).QueryRecordWhere("MeterID = {0} AND DataStartTime = {1} AND DataEndtime = {2}", meter.ID, ToDateTime2(connection, fileGroupPost.FileGroup.DataStartTime), ToDateTime2(connection, fileGroupPost.FileGroup.DataEndTime));
                        if (fileGroup == null) {
                            fileGroup = fileGroupPost.FileGroup;
                            fileGroup.ID = 0;
                            fileGroup.MeterID = meter.ID;

                            new TableOperations<FileGroup>(connection).AddNewRecord(fileGroup);
                            fileGroup = new TableOperations<FileGroup>(connection).QueryRecordWhere("MeterID = {0} AND DataStartTime = {1} AND DataEndtime = {2}", meter.ID, ToDateTime2(connection, fileGroupPost.FileGroup.DataStartTime), ToDateTime2(connection, fileGroupPost.FileGroup.DataEndTime));
                        }

                        foreach(DataFile file in fileGroupPost.DataFiles)
                        {
                            DataFile dataFile = new TableOperations<DataFile>(connection).QueryRecordWhere("FileGroupID = {0} AND FilePath = {1} AND FilePathHash = {2} AND FileSize = {3}", fileGroup.ID, file.FilePath, file.FilePathHash, file.FileSize);
                            FileBlob blob = fileGroupPost.FileBlobs.Find(x => x.DataFileID == file.ID);

                            if (dataFile == null)
                            {
                                dataFile = file;
                                dataFile.ID = 0;
                                dataFile.FileGroupID = fileGroup.ID;
                                new TableOperations<DataFile>(connection).AddNewRecord(dataFile);
                                dataFile = new TableOperations<DataFile>(connection).QueryRecordWhere("FileGroupID = {0} AND FilePath = {1} AND FilePathHash = {2} AND FileSize = {3}", fileGroup.ID, file.FilePath, file.FilePathHash, file.FileSize);
                            }

                            FileBlob fileBlob = new TableOperations<FileBlob>(connection).QueryRecordWhere("DataFileID = {0}", dataFile.ID);
                            if (fileBlob == null)
                            {
                                fileBlob = blob;
                                fileBlob.ID = 0;
                                fileBlob.DataFileID = dataFile.ID;

                                new TableOperations<FileBlob>(connection).AddNewRecord(fileBlob);
                            }



                        }

                        DataFileController dataFileController = new DataFileController(NodeHost);
                        dataFileController.ReprocessFile(fileGroup.ID).Wait();

                        return Ok(fileGroup.ID);
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex.Message, ex);
                        return InternalServerError(ex);
                    }
                }
            }
            else
                return Unauthorized();

        }

        [Route("SyncMeterConfig/{connectionId}/{instanceId:int}/{meterId:int}"), HttpGet]
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
                        UserAccount userAccount = new TableOperations<UserAccount>(connection).QueryRecordWhere("ID = {0}", instance.UserAccountID);
                        engine.SyncMeterConfigurationForInstance(connectionId, instance, meter, userAccount, cancellationToken);
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex.Message, ex);
                    }
                }
            }, cancellationToken);

        }

        [Route("SyncMeterFiles/{connectionId}/{instanceId:int}/{meterId:int}"), HttpGet]
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

        [Route("SyncInstanceConfig/{connectionId}/{instanceId:int}"), HttpGet]
        public Task SyncInstanceConfiguration(string connectionId, int instanceId, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                // for now, create new instance of DataPusherEngine.  Later have one running in XDA ServiceHost and tie to it to ensure multiple updates arent happening simultaneously
                DataPusherEngine engine = new DataPusherEngine(() => new AdoDataConnection("systemSettings"));
                engine.SyncInstanceConfiguration(connectionId, instanceId, cancellationToken);
            }, cancellationToken);
        }

        [Route("TestConnection/{instanceId:int}"), HttpGet]
        public IHttpActionResult TestRemoteInstanceConnection(int instanceId)
        {
            // for now, create new instance of DataPusherEngine.  Later have one running in XDA ServiceHost and tie to it to ensure multiple updates arent happening simultaneously
            DataPusherEngine engine = new DataPusherEngine(() => new AdoDataConnection("systemSettings"));
            return Ok(engine.TestInstance(instanceId) ? 1 : 0);
        }

        private static readonly ILog Log = LogManager.GetLogger(typeof(DataPusherController));
    }
}