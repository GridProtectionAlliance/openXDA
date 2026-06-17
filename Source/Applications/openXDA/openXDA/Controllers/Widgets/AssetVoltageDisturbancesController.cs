//******************************************************************************************************
//  AssetVoltageDisturbancesController.cs - Gbtc
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
//  08/21/2023 - Preston Crawford
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Web.Http;
using GSF.Data;

namespace openXDA.Controllers.Widgets
{
    /// <summary>
    /// Controller that handles fetching voltage disturbances for an openXDA event.
    /// </summary>
    [RoutePrefix("api/Widgets/AssetVoltageDisturbances")]
    public class AssetVoltageDisturbancesController : ApiController
    {
        private readonly Func<AdoDataConnection> m_connectionFactory;

        /// <summary>
        /// Constructor to pull a connection factory from the XDA controller activator.
        /// </summary>
        public AssetVoltageDisturbancesController(Func<AdoDataConnection> connectionFactory)
        {
            m_connectionFactory = connectionFactory;
        }

        [Route("{EventID:int}"), HttpGet]
        public IHttpActionResult GetAssetVoltageDisturbances(int EventID)
        {
            using (AdoDataConnection connection = m_connectionFactory())
            {
                const string SQL = @"
                    SELECT
                        EventType.Name as EventType,
                        Phase.Name as Phase,
                        Disturbance.ID,
                        Disturbance.PerUnitMagnitude,
                        Disturbance.DurationSeconds,
                        Disturbance.StartTime,
                        DisturbanceSeverity.SeverityCode,
                        CASE
                            WHEN Disturbance.ID in (SELECT WorstDisturbanceID FROM EventWorstDisturbance WHERE EventWorstDisturbance.EventID = {0}) THEN 1
                            ELSE 0
                        END as IsWorstDisturbance
                    FROM
                        Disturbance
                        JOIN Phase ON Disturbance.PhaseID = Phase.ID
                        JOIN EventType ON Disturbance.EventTypeID = EventType.ID
                        JOIN DisturbanceSeverity ON Disturbance.ID = DisturbanceSeverity.DisturbanceID
                    WHERE
                        Phase.Name != 'WORST' AND
                        Disturbance.EventID = {0}
                    ORDER BY Disturbance.StartTime
                ";

                return Ok(connection.RetrieveData(SQL, EventID));
            }
        }
    }
}
