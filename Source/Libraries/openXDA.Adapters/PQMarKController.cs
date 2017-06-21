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

using GSF.Web.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using GSF.Web;
using openXDA.Model;
using Newtonsoft.Json.Linq;
using System.Web.Http;

namespace openXDA.Adapters
{
    /// <summary>
    /// This class will be used to form a Restful HTTP API that will be interfaced using the PQMarkPusher.
    /// </summary>
    public class PQMarkController : ApiController
    {
        #region [ GET Operations ]

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
                    record = dataContext.Table(typeof(Meter).Assembly.GetType("openXDA.Model." + modelName)).QueryRecordWhere("ID = {0}", id);
                }
                catch (Exception ex)
                {
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
                return BadRequest("The id field must be a comma separated integer list.");
            }

            using (DataContext dataContext = new DataContext("systemSettings"))
            {
                try
                {
                    if (idList.Length == 0)
                        record = dataContext.Table(typeof(Meter).Assembly.GetType("openXDA.Model." + modelName)).QueryRecords();
                    else
                        record = dataContext.Table(typeof(Meter).Assembly.GetType("openXDA.Model." + modelName)).QueryRecordsWhere(idList);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.ToString());
                }
            }

            return Ok(record);
        }

        [HttpGet]
        public IHttpActionResult GetChannels(string id)
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
                return BadRequest("The id field must be a comma separated integer list.");
            }

            using (DataContext dataContext = new DataContext("systemSettings"))
            {
                try
                {
                    if (idList.Length == 0)
                        record = dataContext.Table<ChannelDetail>().QueryRecords();
                    else
                        record = dataContext.Table<ChannelDetail>().QueryRecordsWhere(idList);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.ToString());
                }
            }

            return Ok(record);

        }
        #endregion

        #region [ PUT Operations ]
        [HttpPut]
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
                    return BadRequest(ex.ToString());
                }
            }

            return Ok();
        }
        #endregion

        #region [ POST Operations]
        [HttpPost]
        public IHttpActionResult CreateRecord( string modelName, [FromBody]JObject record)
        {
            using (DataContext dataContext = new DataContext("systemSettings"))
            {
                try
                {
                    Type type = typeof(Meter).Assembly.GetType("openXDA.Model." + modelName);
                    object obj = record.ToObject(type);
                    dataContext.Table(typeof(Meter).Assembly.GetType("openXDA.Model." + modelName)).AddNewRecord(obj);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.ToString());
                }
            }

            return Ok();
        }

        [HttpPost]
        public IHttpActionResult CreateChannel([FromBody]JObject record)
        {
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
                    channel.MeasurementTypeID = measurementCharacteristicID;
                    channel.PhaseID = phaseID;
                    channel.Name = record["Name"].Value<string>();
                    channel.SamplesPerHour = record["SamplesPerHour"].Value<float>();
                    channel.PerUnitValue = record["PerUnitValue"].Value<float>();
                    channel.HarmonicGroup = record["HarmonicGroup"].Value<int>();
                    channel.Description = record["Description"].Value<string>();
                    channel.Enabled = record["Enabled"].Value<bool>();

                    dataContext.Table<Channel>().AddNewRecord(channel);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.ToString());
                }
            }

            return Ok();
        }

        #endregion

        #region [ DELETE Operations]

        [HttpDelete]
        public IHttpActionResult DeleteRecord(int id, string modelName)
        {
            using (DataContext dataContext = new DataContext("systemSettings"))
            {
                dataContext.Table(typeof(Meter).Assembly.GetType("openXDA.Model." + modelName)).DeleteRecordWhere("ID = {0}", id);
            }

            return Ok();
        }

        #endregion
    }
}
