//******************************************************************************************************
//  WebAPI.cs - Gbtc
//
//  Copyright © 2020, Grid Protection Alliance.  All Rights Reserved.
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
//  10/06/2020 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using GSF.Data;
using GSF.Data.Model;
using GSF.IO;
using GSF.Web.Model;
using openXDA.Adapters;
using openXDA.APIAuthentication;
using openXDA.Model;
using openXDA.Nodes;
using SystemCenter.Model;

namespace openXDA.Controllers.WebAPI
{
    [RoutePrefix("api/GrafanaData")]
    public class GrafanaDataController : ApiController
    {
        [HttpGet, Route("AllMeters")]
        public IHttpActionResult GetAllMeters()
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                DataTable dt = connection.RetrieveData(@"
                    select 
                        *
                    from 
                        [TVAOpenXDA].[dbo].[MeterDetail]
                ");

                Dictionary<string, Dictionary<string, string>> meters = new Dictionary<string, Dictionary<string, string>>();
                foreach (DataRow row in dt.Rows)
                {
                    Dictionary<string, string> data = new Dictionary<string, string>();
                    foreach (DataColumn column in dt.Columns)
                    {
                        data[column.ColumnName] = row[column.ColumnName].ToString();
                    }
                    string id = data["ID"];
                    meters[id] = data;
                }

                DataTable additionalFieldTable = connection.RetrieveData(@"
                    select 
                        *
                    from 
                        [TVAOpenXDA].[dbo].[AdditionalField]
                    where
                        ParentTable = 'Meter'
                ");

                foreach (DataRow row in additionalFieldTable.Rows)
                {
                    string fieldName = row["FieldName"].ToString();
                    string id = row["ID"].ToString();

                    DataTable additionalFieldValueTable = connection.RetrieveData($@"
                        select 
                            *
                        from 
                            [TVAOpenXDA].[dbo].[AdditionalFieldValue]
                        where
                            AdditionalFieldID = {id}
                    ");

                    foreach (DataRow afvRow in additionalFieldValueTable.Rows)
                    {
                        string parentTableID = afvRow["ParentTableID"].ToString();
                        string value = afvRow["Value"].ToString();

                        if (meters.ContainsKey(parentTableID))
                        {
                            meters[parentTableID][fieldName] = value;
                        }
                    }
                }

                return Ok(meters);
            }
        }
    }
}

