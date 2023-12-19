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
using System.Collections.Concurrent;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Principal;
using System.Web.Http;
using GSF;
using GSF.Data;
using GSF.Data.Model;
using GSF.ServiceProcess;
using Newtonsoft.Json.Linq;
using openXDA.APIAuthentication;
using openXDA.Model;

namespace openXDA.Controllers.WebAPI
{
    [RoutePrefix("api/GrafanaData")]
    public class GrafanaDataController : ApiController
    {
        #region [ Members ]

        internal class RTSubscription
        {
            public DateTime Established { get; }
            public DateTime LastUpdate { get; private set; }


            private List<RTDataStream> m_data;
            private object m_messageLock = new object();

            public RTSubscription(string openMICKey)
            {
                Established = DateTime.UtcNow;
                LastUpdate = DateTime.UtcNow;
                m_data = new List<RTDataStream>();
            }

        
         
            public List<RTDataStream> GetData()
            {
                // Make up Data for now. Will need to add logic to connect to openMIC when Ritchie has a chance to set up 
                // Test Environment
                DateTime now = DateTime.UtcNow.Subtract(new TimeSpan(0,0,100));
                List<RTDataStream> data = new List<RTDataStream>();
                data.Add(new RTDataStream()
                {
                    Signal = "Voltage A",
                    Data = GetRandom(120, 5).Select((v, i) => new Point() { Time = now.AddSeconds(i), Value = v }).ToArray()
                }); ;
                data.Add(new RTDataStream()
                {
                    Signal = "Voltage B",
                    Data = GetRandom(120, 5).Select((v, i) => new Point() { Time = now.AddSeconds(i), Value = v }).ToArray()
                }); ;
                data.Add(new RTDataStream()
                {
                    Signal = "Voltage C",
                    Data = GetRandom(120, 10).Select((v, i) => new Point() { Time = now.AddSeconds(i), Value = v }).ToArray()
                }); ;

                data.Add(new RTDataStream()
                {
                    Signal = "Phase A",
                    Data = GetRandom(0, 10).Select((v, i) => new Point() { Time = now.AddSeconds(i), Value = v }).ToArray()
                }); ;
                data.Add(new RTDataStream()
                {
                    Signal = "Phase B",
                    Data = GetRandom(120, 10).Select((v, i) => new Point() { Time = now.AddSeconds(i), Value = v }).ToArray()
                }); ;
                data.Add(new RTDataStream()
                {
                    Signal = "Phase C",
                    Data = GetRandom(240, 10).Select((v, i) => new Point() { Time = now.AddSeconds(i), Value = v }).ToArray()
                }); ;

                return data;
            }

            // Temporary until we connect to STTP
            private double[] GetRandom(double mean, double stdev)
            {
                Random m_random = new Random();
                double[] data = new double[100];
                for (int i = 0; i < 100; i++)
                {
                    double u1 = 1.0 - m_random.NextDouble();
                    double u2 = 1.0 - m_random.NextDouble();
                    double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
                    data[i] = mean + randStdNormal * stdev;
                }
                return data;
            }


        }

        /// <summary>
        /// Represents a List of DataPoints 
        /// </summary>
        public class RTDataStream
        {
            /// <summary>
            /// Gets or sets name of the Signal
            /// </summary>
            public string Signal { get; set; }

            /// <summary>
            /// Gets or sets the data of the Signal.
            /// </summary>
            public Point[] Data { get; set; }
            
        }

        public class Point
        {
            public DateTime Time { get; set; }
            public double Value { get; set; }
        }

        /// <summary>
        /// Stores current Subscriptions to Real Time STTP data
        /// </summary>
        private static ConcurrentDictionary<string, RTSubscription> s_activeConnections = new ConcurrentDictionary<string, RTSubscription>();

        #endregion

        #region [ Http Methods ]
        /// <summary>
        /// Returns All Meter Information
        /// </summary>
        /// <param name="query"> a Query generated by the Grafana Datasource </param>
        /// <returns>a List of Meters matching the query, includign additional Fields. </returns>
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

        /// <summary>
        /// Gets all Channel IDs for trending data associated with a set of meters.
        /// </summary>
        /// <param name="query"> Query generated by the Grafana Datasource</param>
        /// <returns> a List of ChannelIDs</returns>
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

                DataRow[] results = channelTbl.Select(filter);
                if (results.Count() > 0)
                    return Ok(channelTbl.Select(filter).CopyToDataTable());
                return Ok(new DataTable());
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
        
        /// <summary>
        /// Returns the real time data associated with a certain meter
        /// </summary>
        /// <param name="query"> query to get the meters associated with the </param>
        /// <returns></returns>
        [HttpGet, Route("GetRTData")]
        public IHttpActionResult GetRealTimeData([FromBody] JObject query)
        {
           string filter = "";
            if (query.ContainsKey("query"))
                filter = query["query"].ToString();

            IEnumerable<string> openMicKeys;
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                if (string.IsNullOrEmpty(filter))
                    openMicKeys = MeterTable().Select().Select(row => row.AsString("OpenMICAcronym"));
                else
                    openMicKeys = MeterTable().Select(filter).Select(row => row.AsString("OpenMICAcronym"));
            }

            if (openMicKeys.Count() < 1)
                return Ok(1);

            foreach (string key in openMicKeys)
            {
                if (s_activeConnections.ContainsKey(key))
                    continue;
                s_activeConnections.TryAdd(key, new RTSubscription(key));
            }

            return  Ok(openMicKeys.Select(key => { s_activeConnections.TryGetValue(key, out RTSubscription sub); return sub.GetData(); }));
        }

        #endregion

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
