//******************************************************************************************************
//  SOEController.cs - Gbtc
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
//  08/22/2023 - Preston Crawford
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Data;
using System.Web.Http;
using GSF.Data;

namespace openXDA.Controllers.Widgets
{
    /// <summary>
    /// Controller that handles fetching sequence-of-events (SOE) alarm data for an openXDA event.
    /// </summary>
    [RoutePrefix("api/Widgets/SOE")]
    public class SOEController : ApiController
    {
        private const string SOECategory = "dbSOE";
        private readonly Func<AdoDataConnection> m_connectionFactory;

        /// <summary>
        /// Constructor to pull a connection factory from the XDA controller activator.
        /// </summary>
        public SOEController(Func<AdoDataConnection> connectionFactory)
        {
            m_connectionFactory = connectionFactory;
        }

        [Route("{eventID:int}/{timeWindow:int}"), HttpGet]
        public IHttpActionResult Get(int eventID, int timeWindow)
        {
            DateTime eventTime;

            using (AdoDataConnection connection = m_connectionFactory())
                eventTime = connection.ExecuteScalar<DateTime>("SELECT StartTime FROM Event WHERE ID = {0}", eventID);

            using (AdoDataConnection connection = CreateConnection(SOECategory))
            {
                DataTable table = connection.RetrieveData(@"
                    SELECT
                        alarmdatetime as Time,
                        stationname + ' ' + alarmpoint as Alarm,
                        alarmstatus as Status
                    FROM soealarmdetails
                    WHERE alarmdatetime between {0} and {1}
                ", eventTime.AddSeconds(-1 * timeWindow), eventTime.AddSeconds(timeWindow));

                return Ok(table);
            }
        }

        private static AdoDataConnection CreateConnection(string settingsCategory)
        {
            return new AdoDataConnection(settingsCategory);
        }
    }
}
