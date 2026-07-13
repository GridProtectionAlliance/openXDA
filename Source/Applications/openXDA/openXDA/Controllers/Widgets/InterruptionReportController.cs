//******************************************************************************************************
//  InterruptionReportController.cs - Gbtc
//
//  Copyright © 2023, Grid Protection Alliance.  All Rights Reserved.
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
//  08/22/2023 - Preston Crawford
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Http;
using GSF.Data;

namespace openXDA.Controllers.Widgets
{
    /// <summary>
    /// Controller that handles fetching interruption report data for an openXDA event.
    /// </summary>
    [RoutePrefix("api/Widgets/InterruptionReport")]
    public class InterruptionReportController : ApiController
    {
        private const string SettingsCategory = "interruptionReport";
        private readonly Func<AdoDataConnection> m_connectionFactory;

        public class Interruption
        {
            public DateTime? TimeOut { get; set; }
            public DateTime? TimeIn { get; set; }
            public string Class { get; set; }
            public string Area { get; set; }
            public int ReportNumber { get; set; }
            public string Explanation { get; set; }
            public string CircuitInfo { get; set; }
        }

        /// <summary>
        /// Constructor to pull a connection factory from the XDA controller activator.
        /// </summary>
        public InterruptionReportController(Func<AdoDataConnection> connectionFactory)
        {
            m_connectionFactory = connectionFactory;
        }

        [Route("GetEvents/{hour:int}/{eventID:int}"), HttpGet]
        public IHttpActionResult GetData(int hour, int eventID)
        {
            DateTime start;

            using (AdoDataConnection connection = m_connectionFactory())
                start = connection.ExecuteScalar<DateTime>("SELECT StartTime FROM Event WHERE ID = {0} ", eventID);

            List<Interruption> result = new List<Interruption>();
            DataSet ds = new DataSet();

            using (AdoDataConnection connection = CreateConnection(SettingsCategory))
            using (SqlCommand command = ((SqlConnection)connection.Connection).CreateCommand())
            using (SqlDataAdapter sda = new SqlDataAdapter(command))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "iradmin.GetIncidentsByDateTimeRange";
                command.Parameters.AddWithValue("@startDateTime", start.AddHours(-hour));
                command.Parameters.AddWithValue("@endDateTime", start.AddHours(hour));
                sda.Fill(ds);
            }

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                int recordNumber = int.Parse(dr["ReportNumber"].ToString());
                DataRow[] children = ds.Tables[1].Select(string.Format("ReportNumber = {0}", recordNumber));

                result.Add(new Interruption
                {
                    TimeOut = DateTime.Parse(dr["TimeOut"].ToString()),
                    Class = dr["ClassType"].ToString(),
                    Area = dr["Area"].ToString(),
                    ReportNumber = recordNumber,
                    Explanation = dr["Explanation"].ToString(),
                    CircuitInfo = dr["CircuitInfo"].ToString(),
                    TimeIn = null
                });

                result.AddRange(children.Select(r => new Interruption
                {
                    TimeOut = DateTime.Parse(dr["TimeOut"].ToString()),
                    Class = "",
                    Area = r["Area"].ToString(),
                    ReportNumber = recordNumber,
                    Explanation = "",
                    CircuitInfo = "",
                    TimeIn = DateTime.Parse(r["TimeIn"].ToString())
                }));
            }

            return Ok(result);
        }

        private static AdoDataConnection CreateConnection(string settingsCategory)
        {
            return new AdoDataConnection(settingsCategory);
        }
    }
}
