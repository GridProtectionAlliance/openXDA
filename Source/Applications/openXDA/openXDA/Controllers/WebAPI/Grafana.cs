//******************************************************************************************************
//  WebAPI.cs - Gbtc
//
//  Copyright � 2020, Grid Protection Alliance.  All Rights Reserved.
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
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Http;
using GSF.Data;
using GSF.Data.Model;
using Microsoft.AspNet.SignalR.Infrastructure;
using Newtonsoft.Json.Linq;
using openXDA.Model;

namespace openXDA.Controllers.WebAPI
{
    [RoutePrefix("api/GrafanaData")]
    public class GrafanaDataController : ApiController
    {
        [HttpGet, Route("Subscribe")]
        public IHttpActionResult Subscribe()
        {
            return Ok();
        }


        //Returns all main data from meters
        [HttpPost, Route("Meters")]
        public IHttpActionResult GetAllMeters([FromBody] JObject query)
        {
            string filter = "";
            if (query.ContainsKey("query"))
                filter = query["query"].ToString();
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                DataTable meters = MeterTable();
              
                if (string.IsNullOrEmpty(filter))
                    return Ok(meters);

                DataRow[] results = meters.Select(filter);
                if (results.Count() == 0)
                    return Ok(new DataTable());

                return Ok(results.CopyToDataTable());
            }
        }

        // Returns trending Signals associated with a set of meters
        [HttpPost, Route("TrendingDataID")]
        public IHttpActionResult GetTrendingID([FromBody] JObject query)
        {
            string filter = "";
            if (query.ContainsKey("query"))
                filter = query["query"].ToString();
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                DataTable channelTbl = connection.RetrieveData(@"SELECT CHANNEL.ID, 
                    MeterID, 
                    AssetID, 
                    CHANNEL.Name,
                    Phase.Name AS Phase, 
                    MeasurementType.Name AS MeasurementType,
                    MeasurementCharacteristic.Name AS MeasurementCharacteristic
                FROM CHANNEL LEFT JOIN 
					Phase ON Channel.PhaseID = Phase.ID LEFT JOIN
					MeasurementType ON MeasurementType.ID = Channel.MeasurementTypeID LEFT JOIN
					MeasurementCharacteristic ON MeasurementCharacteristic.ID = Channel.MeasurementCharacteristicID
						WHERE Trend = 1");

                if (string.IsNullOrEmpty(filter))
                    return Ok(channelTbl);
                
                return Ok(channelTbl.Select(filter).CopyToDataTable());
            }
        }

        /// <summary>
        /// Returns all posible Variables to be used in Grafana
        /// </summary>
        /// <returns> Additional Fields and Columns in MeterDetails </returns>
        [HttpGet, Route("GetVariables")]
        public IHttpActionResult GetVariables()
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                DataTable AFKeys = connection.RetrieveData(@"
                    SELECT DISTINCT FieldName AS [Key] 
                    FROM AdditionalField
                    WHERE ParentTable = 'Meter'
                ");

                DataTable meterDetails = connection.RetrieveData("SELECT Top 1 * FROM MeterDetail");

                IEnumerable<string> fields = AFKeys.AsEnumerable().Select(row => row.Field<string>("Key")).Concat(
                    meterDetails.Columns.Cast<DataColumn>().Select(x => x.ColumnName)).Concat(new List<string>() { "Customer", "AssetGroup", "CustomerKey" });
                return Ok(fields);
            }
        }

        /// <summary>
        /// Returns all posible Variables to be used in Grafana
        /// </summary>
        /// <returns> Additional Fields and Columns in MeterDetails </returns>
        [HttpGet, Route("GetVariableValues/{Variable}")]
        public IHttpActionResult GetVariableValues(string Variable)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                if (string.IsNullOrEmpty(Variable))
                    return Ok(new List<string>());
                if (Variable == "Customer")
                    return Ok(new TableOperations<Customer>(connection).QueryRecords().Select(c => c.Name));
                if (Variable == "CustomerKey")
                    return Ok(new TableOperations<Customer>(connection).QueryRecords().Select(c => c.CustomerKey));
                if (Variable == "AssetGroup")
                    return Ok(new TableOperations<AssetGroup>(connection).QueryRecords().Select(g => g.Name));

                return Ok(MeterTable().AsEnumerable().Select(row => row.Field<string>(Variable)).Distinct().Where(x => !(x is null)));
            }
        }

        private DataTable MeterTable()
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                DataTable AFKeys = connection.RetrieveData(@"
                    SELECT DISTINCT FieldName AS [Key] 
                    FROM AdditionalField
                    WHERE ParentTable = 'Meter'
                ");

                string fieldNames = "";
                DataRowCollection AFRows = AFKeys.Rows;

                for (int i = 0; i < AFRows.Count; i++)
                {
                    fieldNames += $"[{AFRows[i]["Key"]}]";

                    // If it is not the last column, append a comma.
                    if (i != AFRows.Count - 1)
                    {
                        fieldNames += ", ";
                    }
                }

                DataTable dt = connection.RetrieveData(@"
                    Select * from (select 
                        MeterDetail.*, AF.Value, AF.FieldName,
                        DATEDIFF(Hour, OpenMICDailyStatistic.LastSuccessfulConnection,SYSUTCDATETIME()) as LastSuccessfulConnection
                    from 
                        MeterDetail left join 
                        OpenMICDailyStatistic ON OpenMICDailyStatistic.Meter = MeterDetail.Name
                            AND (OpenMICDailyStatistic.Date = (Select Max(Date) FROM OpenMICDailyStatistic OS WHERE OS.Meter = MeterDetail.Name))
                        LEFT JOIN (SELECT
                            AdditionalFieldValue.ID,
                            AdditionalField.FieldName,
                            AdditionalFieldValue.Value,
                            AdditionalFieldValue.ParentTableID, 
                            AdditionalField.ParentTable
                        FROM
                            AdditionalField JOIN
                            AdditionalFieldValue ON AdditionalField.ID = AdditionalFieldValue.AdditionalFieldID) as AF
	                    on AF.ParentTableID = MeterDetail.ID
                    ) as FullTbl
                    pivot(max(FullTbl.Value) for FullTbl.FieldName IN (" + fieldNames + @")) as Tbl
                ");

                return dt;
            }
        }
    }
}
