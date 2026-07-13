//******************************************************************************************************
//  CorrelatedSagsController.cs - Gbtc
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
using System.Data;
using System.Web.Http;
using GSF.Data;

namespace openXDA.Controllers.Widgets
{
    /// <summary>
    /// Controller that handles fetching sags that are time-correlated with an openXDA event.
    /// </summary>
    [RoutePrefix("api/Widgets/CorrelatedSags")]
    public class CorrelatedSagsController : ApiController
    {
        private readonly Func<AdoDataConnection> m_connectionFactory;

        /// <summary>
        /// Constructor to pull a connection factory from the XDA controller activator.
        /// </summary>
        public CorrelatedSagsController(Func<AdoDataConnection> connectionFactory)
        {
            m_connectionFactory = connectionFactory;
        }

        [Route(""), HttpGet]
        public DataTable GetCorrelatedSags(int eventId, string timeTolerance)
        {
            const string TimeCorrelatedSagsSQL = @"
                SELECT Disturbance.*
                INTO #sag
                FROM
                    Disturbance JOIN
                    EventType DisturbanceType ON
                        Disturbance.EventTypeID = DisturbanceType.ID AND
                        DisturbanceType.Name = 'Sag' JOIN
                    Phase ON
                        Disturbance.PhaseID = Phase.ID AND
                        Phase.Name = 'Worst'
                WHERE
                    Disturbance.StartTime <= {1} AND
                    Disturbance.EndTime >= {0}

                SELECT
                    Event.ID AS EventID,
                    EventType.Name AS EventType,
                    FORMAT(Sag.PerUnitMagnitude * 100.0, '0.#') AS SagMagnitudePercent,
                    FORMAT(Sag.DurationSeconds * 1000.0, '0') AS SagDurationMilliseconds,
                    FORMAT(Sag.DurationCycles, '0.##') AS SagDurationCycles,
                    Event.StartTime,
                    Meter.Name AS MeterName,
                    Asset.AssetName
                FROM
                    Event JOIN
                    EventType ON Event.EventTypeID = EventType.ID JOIN
                    Meter ON Event.MeterID = Meter.ID JOIN
                    MeterAsset ON
                        Event.MeterID = MeterAsset.MeterID AND
                        Event.AssetID = MeterAsset.AssetID JOIN
                    Asset ON Asset.ID = MeterAsset.AssetID CROSS APPLY
                    (
                        SELECT TOP 1
                            PerUnitMagnitude,
                            DurationSeconds,
                            DurationCycles
                        FROM #sag
                        WHERE EventID = Event.ID
                        ORDER BY PerUnitMagnitude DESC
                    ) Sag
                WHERE Event.ID IN (SELECT EventID FROM #sag)
                ORDER BY
                    Event.StartTime,
                    Sag.PerUnitMagnitude";

            if (string.IsNullOrWhiteSpace(timeTolerance))
                timeTolerance = "2.0";

            double tolerance = double.Parse(timeTolerance);

            if (eventId <= 0)
                return new DataTable();

            using (AdoDataConnection connection = m_connectionFactory())
            {
                DateTime startTime = connection.ExecuteScalar<DateTime>("SELECT StartTime FROM Event WHERE ID = {0}", eventId);
                DateTime endTime = connection.ExecuteScalar<DateTime>("SELECT EndTime FROM Event WHERE ID = {0}", eventId);
                return connection.RetrieveData(TimeCorrelatedSagsSQL, startTime.AddSeconds(-tolerance), endTime.AddSeconds(tolerance));
            }
        }
    }
}
