//******************************************************************************************************
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
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Web.Http;
using GSF;
using GSF.Data;
using GSF.Data.Model;
using GSF.Reflection;
using GSF.Web.Model;
using GSF.Web.Security;
using log4net;
using Newtonsoft.Json.Linq;
using openXDA.Model;

namespace openXDA.Adapters
{
    /// <summary>
    /// This class will be used to form a Restful HTTP API that will be interfaced using the PQMarkPusher.
    /// </summary>
    public class PQMarkController : ApiController
    {
        #region [ Static Event Handlers ]
        public static event EventHandler<EventArgs<int, int>> ReprocessFilesEvent;
        private static void OnReprocessFiles(int fileGroupID, int meterID)
        {
            ReprocessFilesEvent?.Invoke(new object(), new EventArgs<int, int>(fileGroupID, meterID));
        }
        #endregion

        #region [ Static ]

        // Static Fields
        private static readonly ILog Log = LogManager.GetLogger(typeof(PQMarkController));

        #endregion

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
        public IHttpActionResult GetRecordIDWhere(string id, string modelName)
        {
            object result;

            using (DataContext dataContext = new DataContext("systemSettings"))
            {
                try
                {
                    Type type = typeof(Meter).Assembly.GetType($"openXDA.Model.{modelName}");
                    ITableOperations tableOperations = dataContext.Table(type);

                    string query =
                        $"SELECT ID " +
                        $"FROM {tableOperations.TableName} " +
                        $"WHERE ({id})";

                    object[] queryParameters = new object[0];

                    if (type.TryGetAttribute(out PQMarkRestrictedAttribute thing))
                    {
                        query += " AND ID IN (SELECT PrimaryID FROM PQMarkRestrictedTableUserAccount WHERE TableName = {0} AND UserAccount = {1})";
                        queryParameters = new object[] { modelName, User.Identity.Name };
                    }

                    result = dataContext.Connection.ExecuteScalar(query, queryParameters);
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
        public IHttpActionResult GetRecordIDsWhere(string id, string modelName)
        {
            object collection;

            using (DataContext dataContext = new DataContext("systemSettings"))
            {
                try
                {
                    Type type = typeof(Meter).Assembly.GetType($"openXDA.Model.{modelName}");
                    ITableOperations tableOperations = dataContext.Table(type);

                    string query =
                        $"SELECT ID " +
                        $"FROM {tableOperations.TableName} " +
                        $"WHERE ({id})";

                    object[] queryParameters = new object[0];

                    if (type.TryGetAttribute(out PQMarkRestrictedAttribute thing))
                    {
                        query += " AND ID IN (SELECT PrimaryID FROM PQMarkRestrictedTableUserAccount WHERE TableName = {0} AND UserAccount = {1})";
                        queryParameters = new object[] { modelName, User.Identity.Name };
                    }

                    Type fieldType = tableOperations.GetFieldType("ID");

                    collection = dataContext.Connection.RetrieveData(query, queryParameters).Select()
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

            using (DataContext dataContext = new DataContext("systemSettings"))
            {
                try
                {
                    Type type = typeof(Meter).Assembly.GetType("openXDA.Model." + modelName);
                    PQMarkRestrictedAttribute thing;

                    if(type.TryGetAttribute(out thing))
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
        public IHttpActionResult GetRecordWhere(string id, string modelName)
        {
            object record;

            using (DataContext dataContext = new DataContext("systemSettings"))
            {
                try
                {
                    Type type = typeof(Meter).Assembly.GetType("openXDA.Model." + modelName);
                    PQMarkRestrictedAttribute thing;

                    if (type.TryGetAttribute(out thing))
                    {
                        record = dataContext.Table(type).QueryRecordWhere(id + " AND ID IN (SELECT PrimaryID FROM PQMarkRestrictedTableUserAccount WHERE TableName = {0} AND UserAccount = {1})", modelName, Thread.CurrentPrincipal.Identity.Name);
                    }
                    else
                    {
                        record = dataContext.Table(type).QueryRecordWhere(id);
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
        public IHttpActionResult GetRecordsWhere(string id, string modelName)
        {
            object record;

            using (DataContext dataContext = new DataContext("systemSettings"))
            {
                try
                {
                    Type type = typeof(Meter).Assembly.GetType("openXDA.Model." + modelName);
                    PQMarkRestrictedAttribute thing;

                    if (type.TryGetAttribute(out thing))
                    {
                        record = dataContext.Table(type).QueryRecordsWhere( id + " AND ID IN (SELECT PrimaryID FROM PQMarkRestrictedTableUserAccount WHERE TableName = {0} AND UserAccount = {1})", modelName, Thread.CurrentPrincipal.Identity.Name);
                    }
                    else
                    {
                        record = dataContext.Table(type).QueryRecordsWhere(id);
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

            using (DataContext dataContext = new DataContext("systemSettings"))
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


            using (DataContext dataContext = new DataContext("systemSettings"))
            {
                try
                {
                    Type type = typeof(Channel);
                    PQMarkRestrictedAttribute thing;

                    if (id == "all")
                    {
                        if (type.TryGetAttribute(out thing))
                            record = dataContext.Table<ChannelDetail>().QueryRecordWhere("ID IN (SELECT PrimaryID FROM PQMarkRestrictedTableUserAccount WHERE TableName = 'Channel' AND UserAccount = {0})", Thread.CurrentPrincipal.Identity.Name);
                        else
                            record = dataContext.Table<ChannelDetail>().QueryRecords();
                    }
                    else
                    {
                        if (type.TryGetAttribute(out thing))
                            record = dataContext.Table<ChannelDetail>().QueryRecordWhere("ID = {0} AND ID IN (SELECT PrimaryID FROM PQMarkRestrictedTableUserAccount WHERE TableName = 'Channel' AND UserAccount = {1})", id, Thread.CurrentPrincipal.Identity.Name);
                        else
                            record = dataContext.Table<ChannelDetail>().QueryRecordsWhere(id);
                    }
                    record = dataContext.Table<ChannelDetail>().QueryRecordsWhere(id);
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                    return BadRequest(ex.ToString());
                }
            }

            return Ok(record);

        }
        #endregion

        #region [ PUT Operations ]

        [HttpPut]
        [ValidateRequestVerificationToken, SuppressMessage("Security", "SG0016", Justification = "CSRF vulnerability handled via ValidateRequestVerificationToken.")]
        public IHttpActionResult UpdateRecord(string modelName, [FromBody]JObject record)
        {
            using (DataContext dataContext = new DataContext("systemSettings"))
            {
                try
                {
                    Type type = typeof(Meter).Assembly.GetType("openXDA.Model." + modelName);
                    object obj = record.ToObject(type);
                    dataContext.Table(typeof(Meter).Assembly.GetType("openXDA.Model." + modelName)).UpdateRecord(obj);
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
        [ValidateRequestVerificationToken, SuppressMessage("Security", "SG0016", Justification = "CSRF vulnerability handled via ValidateRequestVerificationToken.")]
        public IHttpActionResult AppendToFileBlob([FromBody]JObject record)
        {
            using (DataContext dataContext = new DataContext("systemSettings"))
            {
                try
                {
                    FileBlob fileBlob = record.ToObject<FileBlob>();
                    dataContext.Connection.ExecuteNonQuery("UPDATE FileBlob SET Blob = Blob + {0} WHERE ID = {1}", fileBlob.Blob, fileBlob.ID);
                    dataContext.Connection.ExecuteNonQuery("UPDATE DataFile SET FileSize = FileSize + {0} WHERE ID = {1}", fileBlob.Blob.Length, fileBlob.DataFileID);
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
        [ValidateRequestVerificationToken, SuppressMessage("Security", "SG0016", Justification = "CSRF vulnerability handled via ValidateRequestVerificationToken.")]
        public IHttpActionResult CreateRecord( string modelName, [FromBody]JObject record)
        {
            int recordId;

            using (DataContext dataContext = new DataContext("systemSettings"))
            {
                try
                {
                    Type type = typeof(Meter).Assembly.GetType("openXDA.Model." + modelName);
                    object obj = record.ToObject(type);
                    PQMarkRestrictedAttribute attribute;

                    dataContext.Table(typeof(Meter).Assembly.GetType("openXDA.Model." + modelName)).AddNewRecord(obj);

                    if(modelName == "Meter")
                        recordId = dataContext.Table<Meter>().QueryRecordWhere("AssetKey = {0}", ((Meter)obj).AssetKey).ID;
                    else
                        recordId = dataContext.Connection.ExecuteScalar<int>("SELECT @@Identity");

                    if (type.TryGetAttribute(out attribute))
                        dataContext.Connection.ExecuteNonQuery("INSERT INTO [PQMarkRestrictedTableUserAccount] (PrimaryID, TableName, UserAccount) VALUES ({0}, {1}, {2})", recordId, modelName, Thread.CurrentPrincipal.Identity.Name);
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
        [ValidateRequestVerificationToken, SuppressMessage("Security", "SG0016", Justification = "CSRF vulnerability handled via ValidateRequestVerificationToken.")]
        public IHttpActionResult CreateChannel([FromBody]JObject record)
        {
            int channelId;
            using (DataContext dataContext = new DataContext("systemSettings"))
            {
                try
                {
                    int measurementCharacteristicID = dataContext.Table<MeasurementCharacteristic>().QueryRecordWhere("Name = {0}", record["MeasurementCharacteristic"].Value<string>())?.ID ?? -1;
                    int measurementTypeID = dataContext.Table<MeasurementType>().QueryRecordWhere("Name = {0}", record["MeasurementType"].Value<string>())?.ID ?? -1;
                    int phaseID = dataContext.Table<Phase>().QueryRecordWhere("Name = {0}", record["Phase"].Value<string>())?.ID ?? -1;

                    if(measurementCharacteristicID == -1)
                    {
                        dataContext.Table<MeasurementCharacteristic>().AddNewRecord(new MeasurementCharacteristic() { Name = record["MeasurementCharacteristic"].Value<string>(), Description = "", Display = false });
                        measurementCharacteristicID = dataContext.Connection.ExecuteScalar<int>("SELECT @@IDENTITY");
                    }

                    if(measurementTypeID == -1)
                    {
                        dataContext.Table<MeasurementType>().AddNewRecord(new MeasurementType() { Name = record["MeasurementType"].Value<string>(), Description = "" });
                        measurementTypeID = dataContext.Connection.ExecuteScalar<int>("SELECT @@IDENTITY");
                    }

                    if (phaseID == -1)
                    {
                        dataContext.Table<Phase>().AddNewRecord(new Phase() { Name = record["Phase"].Value<string>(), Description = "" });
                        phaseID = dataContext.Connection.ExecuteScalar<int>("SELECT @@IDENTITY");
                    }

                    Channel channel = new Channel();
                    channel.MeterID = record["MeterID"].Value<int>();
                    channel.LineID = record["LineID"].Value<int>();
                    channel.MeasurementTypeID = measurementTypeID;
                    channel.MeasurementCharacteristicID = measurementCharacteristicID;
                    channel.PhaseID = phaseID;
                    channel.Name = record["Name"].Value<string>();
                    channel.SamplesPerHour = record["SamplesPerHour"].Value<float>();
                    channel.PerUnitValue = record["PerUnitValue"].Value<double?>();
                    channel.HarmonicGroup = record["HarmonicGroup"].Value<int>();
                    channel.Description = record["Description"].Value<string>();
                    channel.Enabled = record["Enabled"].Value<bool>();

                    dataContext.Table<Channel>().AddNewRecord(channel);
                    channelId = dataContext.Connection.ExecuteScalar<int>("SELECT @@Identity");

                    PQMarkRestrictedAttribute attribute;
                    if (typeof(Channel).TryGetAttribute(out attribute))
                        dataContext.Connection.ExecuteNonQuery("INSERT INTO [PQMarkRestrictedTableUserAccount] (PrimaryID, TableName, UserAccount) VALUES ({0}, 'Channel', {1})", channel, Thread.CurrentPrincipal.Identity.Name);

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
        [ValidateRequestVerificationToken, SuppressMessage("Security", "SG0016", Justification = "CSRF vulnerability handled via ValidateRequestVerificationToken.")]
        public IHttpActionResult UpdateChannel([FromBody]JObject record)
        {
            using (DataContext dataContext = new DataContext("systemSettings"))
            {
                try
                {
                    Channel channel = dataContext.Table<Channel>().QueryRecordWhere("ID = {0}", record["ID"].Value<int>());
                    channel.Name = record["Name"].Value<string>();
                    channel.SamplesPerHour = record["SamplesPerHour"].Value<float>();
                    channel.PerUnitValue = record["PerUnitValue"].Value<double?>();
                    channel.HarmonicGroup = record["HarmonicGroup"].Value<int>();
                    channel.Description = record["Description"].Value<string>();
                    channel.Enabled = record["Enabled"].Value<bool>();

                    int measurementCharacteristicID = dataContext.Table<MeasurementCharacteristic>().QueryRecordWhere("Name = {0}", record["MeasurementCharacteristic"].Value<string>())?.ID ?? -1;
                    int measurementTypeID = dataContext.Table<MeasurementType>().QueryRecordWhere("Name = {0}", record["MeasurementType"].Value<string>())?.ID ?? -1;
                    int phaseID = dataContext.Table<Phase>().QueryRecordWhere("Name = {0}", record["Phase"].Value<string>())?.ID ?? -1;

                    if (measurementCharacteristicID == -1)
                    {
                        dataContext.Table<MeasurementCharacteristic>().AddNewRecord(new MeasurementCharacteristic() { Name = record["MeasurementCharacteristic"].Value<string>(), Description = "", Display = false });
                        measurementCharacteristicID = dataContext.Connection.ExecuteScalar<int>("SELECT @@IDENTITY");
                    }

                    if (measurementTypeID == -1)
                    {
                        dataContext.Table<MeasurementType>().AddNewRecord(new MeasurementType() { Name = record["MeasurementType"].Value<string>(), Description = "" });
                        measurementTypeID = dataContext.Connection.ExecuteScalar<int>("SELECT @@IDENTITY");
                    }

                    if (phaseID == -1)
                    {
                        dataContext.Table<Phase>().AddNewRecord(new Phase() { Name = record["Phase"].Value<string>(), Description = "" });
                        phaseID = dataContext.Connection.ExecuteScalar<int>("SELECT @@IDENTITY");
                    }

                    channel.MeasurementTypeID = measurementTypeID;
                    channel.MeasurementCharacteristicID = measurementCharacteristicID;
                    channel.PhaseID = phaseID;

                    dataContext.Table<Channel>().UpdateRecord(channel);

                    PQMarkRestrictedAttribute attribute;
                    if (typeof(Channel).TryGetAttribute(out attribute))
                        dataContext.Connection.ExecuteNonQuery("INSERT INTO [PQMarkRestrictedTableUserAccount] (PrimaryID, TableName, UserAccount) VALUES ({0}, 'Channel', {1})", channel, Thread.CurrentPrincipal.Identity.Name);

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
        [ValidateRequestVerificationToken, SuppressMessage("Security", "SG0016", Justification = "CSRF vulnerability handled via ValidateRequestVerificationToken.")]
        public IHttpActionResult ProcessFileGroup([FromBody]JObject record)
        {
            try
            {
                OnReprocessFiles(record["FileGroupID"].Value<int>(), record["MeterID"].Value<int>());
            }
            catch (Exception ex) {
                Log.Error(ex);
                return BadRequest("Failed to process file group.");
            }

            Log.Info($"Processed file group {record["FileGroupID"].Value<int>()} for meter {record["MeterID"].Value<int>()}");
            return Ok();
        }

        #endregion

        #region [ DELETE Operations ]

        [HttpDelete]
        [ValidateRequestVerificationToken, SuppressMessage("Security", "SG0016", Justification = "CSRF vulnerability handled via ValidateRequestVerificationToken.")]
        public IHttpActionResult DeleteRecord(int id, string modelName)
        {
            try
            {
                using (DataContext dataContext = new DataContext("systemSettings"))
                {
                    Type type = typeof(Meter).Assembly.GetType("openXDA.Model." + modelName);
                    PQMarkRestrictedAttribute attribute;

                    if (type.TryGetAttribute(out attribute))
                    {
                        dataContext.Table(type).DeleteRecordWhere("ID = {0} AND ID IN (SELECT PrimaryID FROM PQMarkRestrictedTableUserAccount WHERE TableName = {1} AND UserAccount = {2})", id, modelName, Thread.CurrentPrincipal.Identity.Name);
                        dataContext.Connection.ExecuteNonQuery("DELETE FROM [PQMarkRestrictedTableUserAccount] WHERE PrimaryID = {0} AND TableName = {1} AND UserAccount = {2}", id, modelName, Thread.CurrentPrincipal.Identity.Name);
                    }
                    else
                        dataContext.Table(type).DeleteRecordWhere("ID = {0}", id);
                }
            }
            catch (Exception ex) {
                Log.Error(ex);
                return BadRequest("Failed delete.");
            }

            Log.Info($"Deleted {id} from {modelName} table");

            return Ok();
        }

        #endregion
    }
}
