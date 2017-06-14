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
