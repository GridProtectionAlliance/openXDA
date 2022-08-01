﻿//******************************************************************************************************
//  PQMarKController.cs - Gbtc
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
//  06/08/2017 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using GSF.Data;
using GSF.Data.Model;
using GSF.Reflection;
using GSF.Web.Model;
using GSF.Web.Security;
using log4net;
using Newtonsoft.Json.Linq;
using openXDA.Model;
using openXDA.Nodes;
using openXDA.Nodes.Types.Analysis;

namespace openXDA.Adapters
{
    /// <summary>
    /// This class will be used to form a Restful HTTP API that will be interfaced using the PQMarkPusher.
    /// </summary>
    public class PQMarkController : ApiController
    {
        #region [ Constructors ]

        public PQMarkController(Host host) =>
            Host = host;

        #endregion

        #region [ Properties ]

        private Host Host { get; }

        #endregion

        #region [ Methods ]

        #region [ GET Operations ]

        // This generates a request verification token that will need to be added to the headers
        // of a web request before calling PUT, POST, or DELETE operations since these methods
        // validate the header token to prevent CSRF attacks in a browser. Browsers will not
        // allow this HTTP GET based method to be called from remote sites due to Same-Origin
        // policies unless CORS has been configured to explicitly to allow it; as such PUT,
        // POST, and DELETE operations (which are allowed from any site) will fail unless
        // this header token is made available. The actual header name used to store the
        // verification token is controlled by the local configuration.
        [HttpGet]
        public HttpResponseMessage GenerateRequestVerficationToken()
        {
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(Request.GenerateRequestVerficationHeaderToken(), Encoding.UTF8, "text/plain")
            };
        }

        /// <summary>
        /// Return single Record ID
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetRecordIDWhere(string modelName)
        {
            object result;

            using (AdoDataConnection connection = Host.CreateDbConnection())
            using (DataContext dataContext = new DataContext(connection))
            {
                try
                {
                    string whereClause = Request.GetQueryNameValuePairs()
                        .Where(kvp => kvp.Key == "where")
                        .Select(kvp => kvp.Value)
                        .FirstOrDefault() ?? "1=1";

                    Type type = typeof(Meter).Assembly.GetType($"openXDA.Model.{modelName}");
                    ITableOperations tableOperations = dataContext.Table(type);

                    string query =
                        $"SELECT ID " +
                        $"FROM {tableOperations.TableName} " +
                        $"WHERE ({whereClause})";

                    object[] queryParameters = new object[0];

                    if (type.TryGetAttribute(out PQMarkRestrictedAttribute thing))
                    {
                        query += " AND ID IN (SELECT PrimaryID FROM PQMarkRestrictedTableUserAccount WHERE TableName = {0} AND UserAccount = {1})";
                        queryParameters = new object[] { modelName, User.Identity.Name };
                    }

                    result = connection.ExecuteScalar(query, queryParameters);
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                    return BadRequest(ex.ToString());
                }
            }

            return Ok(result);
        }

        /// <summary>
        /// Return multiple Record IDs
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetRecordIDsWhere(string modelName)
        {
            object collection;

            using (AdoDataConnection connection = Host.CreateDbConnection())
            using (DataContext dataContext = new DataContext(connection))
            {
                try
                {
                    string whereClause = Request.GetQueryNameValuePairs()
                        .Where(kvp => kvp.Key == "where")
                        .Select(kvp => kvp.Value)
                        .FirstOrDefault() ?? "1=1";

                    Type type = typeof(Meter).Assembly.GetType($"openXDA.Model.{modelName}");
                    ITableOperations tableOperations = dataContext.Table(type);

                    string query =
                        $"SELECT ID " +
                        $"FROM {tableOperations.TableName} " +
                        $"WHERE ({whereClause})";

                    object[] queryParameters = new object[0];

                    if (type.TryGetAttribute(out PQMarkRestrictedAttribute thing))
                    {
                        query += " AND ID IN (SELECT PrimaryID FROM PQMarkRestrictedTableUserAccount WHERE TableName = {0} AND UserAccount = {1})";
                        queryParameters = new object[] { modelName, User.Identity.Name };
                    }

                    Type fieldType = tableOperations.GetFieldType("ID");

                    collection = connection.RetrieveData(query, queryParameters).Select()
                        .Select(row => row.ConvertField("ID", fieldType));
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                    return BadRequest(ex.ToString());
                }
            }

            return Ok(collection);
        }

        /// <summary>
        /// Return single Record
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetRecord(int id, string modelName)
        {
            object record;

            using (AdoDataConnection connection = Host.CreateDbConnection())
            using (DataContext dataContext = new DataContext(connection))
            {
                try
                {
                    Type type = typeof(Meter).Assembly.GetType("openXDA.Model." + modelName);
                    PQMarkRestrictedAttribute thing;

                    if (type.TryGetAttribute(out thing))
                    {
                        record = dataContext.Table(type).QueryRecordWhere("ID = {0} AND ID IN (SELECT PrimaryID FROM PQMarkRestrictedTableUserAccount WHERE TableName = {1} AND UserAccount = {2})", id, modelName, Thread.CurrentPrincipal.Identity.Name);
                    }
                    else
                    {
                        record = dataContext.Table(type).QueryRecordWhere("ID = {0}", id);
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                    return BadRequest(ex.ToString());
                }
            }

            return Ok(record);
        }

        /// <summary>
        /// Return single Record
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetRecordWhere(string modelName)
        {
            object record;

            using (AdoDataConnection connection = Host.CreateDbConnection())
            using (DataContext dataContext = new DataContext(connection))
            {
                try
                {
                    string whereClause = Request.GetQueryNameValuePairs()
                        .Where(kvp => kvp.Key == "where")
                        .Select(kvp => kvp.Value)
                        .FirstOrDefault() ?? "1=1";

                    Type type = typeof(Meter).Assembly.GetType("openXDA.Model." + modelName);
                    PQMarkRestrictedAttribute thing;

                    if (type.TryGetAttribute(out thing))
                    {
                        record = dataContext.Table(type).QueryRecordWhere($"({whereClause}) AND ID IN (SELECT PrimaryID FROM PQMarkRestrictedTableUserAccount WHERE TableName = {{0}} AND UserAccount = {{1}})", modelName, Thread.CurrentPrincipal.Identity.Name);
                    }
                    else
                    {
                        record = dataContext.Table(type).QueryRecordWhere(whereClause);
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                    return BadRequest(ex.ToString());
                }
            }

            return Ok(record);
        }

        /// <summary>
        /// Return single Record
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetRecordsWhere(string modelName)
        {
            object record;

            using (AdoDataConnection connection = Host.CreateDbConnection())
            using (DataContext dataContext = new DataContext(connection))
            {
                try
                {
                    string whereClause = Request.GetQueryNameValuePairs()
                        .Where(kvp => kvp.Key == "where")
                        .Select(kvp => kvp.Value)
                        .FirstOrDefault() ?? "1=1";

                    Type type = typeof(Meter).Assembly.GetType("openXDA.Model." + modelName);
                    PQMarkRestrictedAttribute thing;

                    if (type.TryGetAttribute(out thing))
                    {
                        record = dataContext.Table(type).QueryRecordsWhere($"({whereClause}) AND ID IN (SELECT PrimaryID FROM PQMarkRestrictedTableUserAccount WHERE TableName = {{0}} AND UserAccount = {{1}})", modelName, Thread.CurrentPrincipal.Identity.Name);
                    }
                    else
                    {
                        record = dataContext.Table(type).QueryRecordsWhere(whereClause);
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                    return BadRequest(ex.ToString());
                }
            }

            return Ok(record);
        }

        /// <summary>
        /// Returns multiple records
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetRecords(string id, string modelName)
        {

            object record;

            string idList = "";

            try
            {
                if (id != "all")
                {
                    string[] ids = id.Split(',');

                    if (ids.Count() > 0)
                        idList = $"ID IN ({ string.Join(",", ids.Select(x => int.Parse(x)))})";
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return BadRequest("The id field must be a comma separated integer list.");
            }

            using (AdoDataConnection connection = Host.CreateDbConnection())
            using (DataContext dataContext = new DataContext(connection))
            {
                try
                {
                    Type type = typeof(Meter).Assembly.GetType("openXDA.Model." + modelName);
                    PQMarkRestrictedAttribute thing;

                    if (idList.Length == 0)
                    {

                        if (type.TryGetAttribute(out thing))
                        {
                            record = dataContext.Table(type).QueryRecordsWhere("ID IN (SELECT PrimaryID FROM PQMarkRestrictedTableUserAccount WHERE TableName = {0} AND UserAccount = {1})", modelName, Thread.CurrentPrincipal.Identity.Name);
                        }
                        else
                        {
                            record = dataContext.Table(type).QueryRecords();
                        }

                    }
                    else
                    {
                        if (type.TryGetAttribute(out thing))
                        {
                            record = dataContext.Table(type).QueryRecordsWhere(idList + " AND ID IN (SELECT PrimaryID FROM PQMarkRestrictedTableUserAccount WHERE TableName = {0} AND UserAccount = {1})", modelName, Thread.CurrentPrincipal.Identity.Name);
                        }
                        else
                        {
                            record = dataContext.Table(type).QueryRecordsWhere(idList);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                    return BadRequest(ex.ToString());
                }
            }

            return Ok(record);
        }

        [HttpGet]
        public IHttpActionResult GetChannels(string id)
        {
            object record;

            using (AdoDataConnection connection = Host.CreateDbConnection())
            {
                try
                {
                    TableOperations<ChannelDetail> channelTable = new TableOperations<ChannelDetail>(connection);
                    Type type = typeof(Channel);
                    PQMarkRestrictedAttribute thing;

                    if (id == "all")
                    {
                        if (type.TryGetAttribute(out thing))
                            record = channelTable.QueryRecordWhere("ID IN (SELECT PrimaryID FROM PQMarkRestrictedTableUserAccount WHERE TableName = 'Channel' AND UserAccount = {0})", Thread.CurrentPrincipal.Identity.Name);
                        else
                            record = channelTable.QueryRecords();
                    }
                    else
                    {
                        if (type.TryGetAttribute(out thing))
                            record = channelTable.QueryRecordWhere("ID = {0} AND ID IN (SELECT PrimaryID FROM PQMarkRestrictedTableUserAccount WHERE TableName = 'Channel' AND UserAccount = {1})", id, Thread.CurrentPrincipal.Identity.Name);
                        else
                            record = channelTable.QueryRecordsWhere(id);
                    }
                    record = channelTable.QueryRecordsWhere(id);
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                    return BadRequest(ex.ToString());
                }
            }

            return Ok(record);

        }

        /// <summary>
        /// Return a 1 to request
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult Alive()
        {
            return Ok(1);
        }

        #endregion

        #region [ PUT Operations ]

        [HttpPut]
        [ValidateRequestVerificationToken]
        public IHttpActionResult UpdateRecord(string modelName, [FromBody] JObject record)
        {
            using (AdoDataConnection connection = Host.CreateDbConnection())
            using (DataContext dataContext = new DataContext(connection))
            {
                try
                {
                    Type type = typeof(Meter).Assembly.GetType("openXDA.Model." + modelName);
                    object obj = record.ToObject(type);
                    dataContext.Table(type).UpdateRecord(obj);
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                    return BadRequest(ex.ToString());
                }
            }
            Log.Info($"Updated {modelName} table with record...");
            return Ok();
        }

        [HttpPut]
        [ValidateRequestVerificationToken]
        public IHttpActionResult AppendToFileBlob([FromBody] JObject record)
        {
            using (AdoDataConnection connection = Host.CreateDbConnection())
            {
                try
                {
                    FileBlob fileBlob = record.ToObject<FileBlob>();
                    connection.ExecuteNonQuery("UPDATE FileBlob SET Blob = Blob + {0} WHERE ID = {1}", fileBlob.Blob, fileBlob.ID);
                    connection.ExecuteNonQuery("UPDATE DataFile SET FileSize = FileSize + {0} WHERE ID = {1}", fileBlob.Blob.Length, fileBlob.DataFileID);
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                    return BadRequest(ex.ToString());
                }
            }
            Log.Info($"Appended to record in FileBlob table...");
            return Ok();
        }

        #endregion

        #region [ POST Operations ]

        [HttpPost]
        [ValidateRequestVerificationToken]
        public IHttpActionResult CreateRecord(string modelName, [FromBody] JObject record)
        {
            int recordId;

            using (AdoDataConnection connection = Host.CreateDbConnection())
            using (DataContext dataContext = new DataContext(connection))
            {
                try
                {
                    Type type = typeof(Meter).Assembly.GetType("openXDA.Model." + modelName);
                    ITableOperations table = dataContext.Table(type);
                    object obj = record.ToObject(type);
                    table.AddNewRecord(obj);

                    PropertyInfo assetKeyProperty = type.GetProperty("AssetKey");

                    if (!(assetKeyProperty is null)) 
                        recordId = connection.ExecuteScalar<int>($"SELECT ID FROM {table.TableName} WHERE AssetKey = {{0}}", assetKeyProperty.GetValue(obj));
                    else
                        recordId = connection.ExecuteScalar<int>("SELECT @@Identity");

                    if (type.TryGetAttribute(out PQMarkRestrictedAttribute attribute))
                        connection.ExecuteNonQuery("INSERT INTO [PQMarkRestrictedTableUserAccount] (PrimaryID, TableName, UserAccount) VALUES ({0}, {1}, {2})", recordId, modelName, Thread.CurrentPrincipal.Identity.Name);
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                    return BadRequest(ex.ToString());
                }
            }

            Log.Info($"Added record to {modelName} table");

            return Ok(recordId);
        }


        [HttpPost]
        [ValidateRequestVerificationToken]
        public IHttpActionResult CreateChannel([FromBody] JObject record)
        {
            int channelId;

            using (AdoDataConnection connection = Host.CreateDbConnection())
            {
                try
                {
                    TableOperations<MeasurementCharacteristic> measurementCharacteristicTable = new TableOperations<MeasurementCharacteristic>(connection);
                    TableOperations<MeasurementType> measurementTypeTable = new TableOperations<MeasurementType>(connection);
                    TableOperations<Phase> phaseTable = new TableOperations<Phase>(connection);

                    int measurementCharacteristicID = measurementCharacteristicTable.QueryRecordWhere("Name = {0}", record["MeasurementCharacteristic"].Value<string>())?.ID ?? -1;
                    int measurementTypeID = measurementTypeTable.QueryRecordWhere("Name = {0}", record["MeasurementType"].Value<string>())?.ID ?? -1;
                    int phaseID = phaseTable.QueryRecordWhere("Name = {0}", record["Phase"].Value<string>())?.ID ?? -1;

                    if (measurementCharacteristicID == -1)
                    {
                        measurementCharacteristicTable.AddNewRecord(new MeasurementCharacteristic() { Name = record["MeasurementCharacteristic"].Value<string>(), Description = "", Display = false });
                        measurementCharacteristicID = connection.ExecuteScalar<int>("SELECT @@IDENTITY");
                    }

                    if (measurementTypeID == -1)
                    {
                        measurementTypeTable.AddNewRecord(new MeasurementType() { Name = record["MeasurementType"].Value<string>(), Description = "" });
                        measurementTypeID = connection.ExecuteScalar<int>("SELECT @@IDENTITY");
                    }

                    if (phaseID == -1)
                    {
                        phaseTable.AddNewRecord(new Phase() { Name = record["Phase"].Value<string>(), Description = "" });
                        phaseID = connection.ExecuteScalar<int>("SELECT @@IDENTITY");
                    }

                    Channel channel = new Channel();
                    channel.MeterID = record["MeterID"].Value<int>();
                    channel.AssetID = record["AssetID"].Value<int>();
                    channel.MeasurementTypeID = measurementTypeID;
                    channel.MeasurementCharacteristicID = measurementCharacteristicID;
                    channel.PhaseID = phaseID;
                    channel.Name = record["Name"].Value<string>();
                    channel.SamplesPerHour = record["SamplesPerHour"].Value<float>();
                    channel.PerUnitValue = record["PerUnitValue"].Value<double?>();
                    channel.HarmonicGroup = record["HarmonicGroup"].Value<int>();
                    channel.Description = record["Description"].Value<string>();
                    channel.Enabled = record["Enabled"].Value<bool>();

                    TableOperations<Channel> channelTable = new TableOperations<Channel>(connection);
                    channelTable.AddNewRecord(channel);
                    channelId = connection.ExecuteScalar<int>("SELECT @@Identity");

                    PQMarkRestrictedAttribute attribute;
                    if (typeof(Channel).TryGetAttribute(out attribute))
                        connection.ExecuteNonQuery("INSERT INTO [PQMarkRestrictedTableUserAccount] (PrimaryID, TableName, UserAccount) VALUES ({0}, 'Channel', {1})", channel, Thread.CurrentPrincipal.Identity.Name);

                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                    return BadRequest(ex.ToString());
                }
            }

            Log.Info($"Added record to Channel table");
            return Ok(channelId);
        }

        [HttpPost]
        [ValidateRequestVerificationToken]
        public IHttpActionResult UpdateChannel([FromBody] JObject record)
        {
            using (AdoDataConnection connection = Host.CreateDbConnection())
            {
                try
                {
                    TableOperations<Channel> channelTable = new TableOperations<Channel>(connection);
                    Channel channel = channelTable.QueryRecordWhere("ID = {0}", record["ID"].Value<int>());
                    channel.Name = record["Name"].Value<string>();
                    channel.SamplesPerHour = record["SamplesPerHour"].Value<float>();
                    channel.PerUnitValue = record["PerUnitValue"].Value<double?>();
                    channel.HarmonicGroup = record["HarmonicGroup"].Value<int>();
                    channel.Description = record["Description"].Value<string>();
                    channel.Enabled = record["Enabled"].Value<bool>();

                    TableOperations<MeasurementCharacteristic> measurementCharacteristicTable = new TableOperations<MeasurementCharacteristic>(connection);
                    TableOperations<MeasurementType> measurementTypeTable = new TableOperations<MeasurementType>(connection);
                    TableOperations<Phase> phaseTable = new TableOperations<Phase>(connection);

                    int measurementCharacteristicID = measurementCharacteristicTable.QueryRecordWhere("Name = {0}", record["MeasurementCharacteristic"].Value<string>())?.ID ?? -1;
                    int measurementTypeID = measurementTypeTable.QueryRecordWhere("Name = {0}", record["MeasurementType"].Value<string>())?.ID ?? -1;
                    int phaseID = phaseTable.QueryRecordWhere("Name = {0}", record["Phase"].Value<string>())?.ID ?? -1;

                    if (measurementCharacteristicID == -1)
                    {
                        measurementCharacteristicTable.AddNewRecord(new MeasurementCharacteristic() { Name = record["MeasurementCharacteristic"].Value<string>(), Description = "", Display = false });
                        measurementCharacteristicID = connection.ExecuteScalar<int>("SELECT @@IDENTITY");
                    }

                    if (measurementTypeID == -1)
                    {
                        measurementTypeTable.AddNewRecord(new MeasurementType() { Name = record["MeasurementType"].Value<string>(), Description = "" });
                        measurementTypeID = connection.ExecuteScalar<int>("SELECT @@IDENTITY");
                    }

                    if (phaseID == -1)
                    {
                        phaseTable.AddNewRecord(new Phase() { Name = record["Phase"].Value<string>(), Description = "" });
                        phaseID = connection.ExecuteScalar<int>("SELECT @@IDENTITY");
                    }

                    channel.MeasurementTypeID = measurementTypeID;
                    channel.MeasurementCharacteristicID = measurementCharacteristicID;
                    channel.PhaseID = phaseID;

                    channelTable.UpdateRecord(channel);

                    PQMarkRestrictedAttribute attribute;
                    if (typeof(Channel).TryGetAttribute(out attribute))
                        connection.ExecuteNonQuery("INSERT INTO [PQMarkRestrictedTableUserAccount] (PrimaryID, TableName, UserAccount) VALUES ({0}, 'Channel', {1})", channel, Thread.CurrentPrincipal.Identity.Name);

                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                    return BadRequest(ex.ToString());
                }
            }

            Log.Info($"Added record to Channel table");
            return Ok();
        }


        [HttpPost]
        [ValidateRequestVerificationToken]
        public async Task<IHttpActionResult> ProcessFileGroup([FromBody] JObject record, CancellationToken cancellationToken)
        {
            void ConfigureRequest(HttpRequestMessage request)
            {
                Type analysisNodeType = typeof(AnalysisNode);
                string url = Host.BuildURL(analysisNodeType, "PollTaskQueue");
                request.RequestUri = new Uri(url);
                request.Method = HttpMethod.Post;
            }

            try
            {
                using (AdoDataConnection connection = Host.CreateDbConnection())
                {
                    TableOperations<AnalysisTask> analysisTaskTable = new TableOperations<AnalysisTask>(connection);
                    AnalysisTask analysisTask = new AnalysisTask();
                    analysisTask.FileGroupID = record.Value<int>("FileGroupID");
                    analysisTask.MeterID = record.Value<int>("MeterID");
                    analysisTask.Priority = 3;
                    analysisTaskTable.AddNewRecord(analysisTask);
                }

                await Host.SendWebRequestAsync(ConfigureRequest, cancellationToken);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return BadRequest("Failed to process file group.");
            }

            Log.Info($"Processed file group {record["FileGroupID"].Value<int>()} for meter {record["MeterID"].Value<int>()}");
            return Ok();
        }

        #endregion

        #region [ DELETE Operations ]

        [HttpDelete]
        [ValidateRequestVerificationToken]
        public IHttpActionResult DeleteRecord(int id, string modelName)
        {
            try
            {
                using (AdoDataConnection connection = Host.CreateDbConnection())
                using (DataContext dataContext = new DataContext(connection))
                {
                    Type type = typeof(Meter).Assembly.GetType("openXDA.Model." + modelName);
                    PQMarkRestrictedAttribute attribute;

                    if (type.TryGetAttribute(out attribute))
                    {
                        dataContext.Table(type).DeleteRecordWhere("ID = {0} AND ID IN (SELECT PrimaryID FROM PQMarkRestrictedTableUserAccount WHERE TableName = {1} AND UserAccount = {2})", id, modelName, Thread.CurrentPrincipal.Identity.Name);
                        connection.ExecuteNonQuery("DELETE FROM [PQMarkRestrictedTableUserAccount] WHERE PrimaryID = {0} AND TableName = {1} AND UserAccount = {2}", id, modelName, Thread.CurrentPrincipal.Identity.Name);
                    }
                    else
                        dataContext.Table(type).DeleteRecordWhere("ID = {0}", id);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return BadRequest("Failed delete.");
            }

            Log.Info($"Deleted {id} from {modelName} table");

            return Ok();
        }

        #endregion

        #endregion

        #region [ Static ]

        // Static Fields
        private static readonly ILog Log = LogManager.GetLogger(typeof(PQMarkController));

        #endregion
    }
}
