//******************************************************************************************************
//  AssetHistoryTableController.cs - Gbtc
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
    /// Controller that handles fetching the event history of the asset associated with an openXDA event.
    /// </summary>
    [RoutePrefix("api/Widgets/AssetHistoryTable")]
    public class AssetHistoryTableController : ApiController
    {
        private readonly Func<AdoDataConnection> m_connectionFactory;

        /// <summary>
        /// Constructor to pull a connection factory from the XDA controller activator.
        /// </summary>
        public AssetHistoryTableController(Func<AdoDataConnection> connectionFactory)
        {
            m_connectionFactory = connectionFactory;
        }

        [Route("{EventID:int}/{count:int}"), HttpGet]
        public DataTable GetAssetHistory(int EventID, int count = 10)
        {
            using (AdoDataConnection connection = m_connectionFactory())
            {
                return connection.RetrieveData(@"
                    SELECT
                        TOP " + count.ToString() + @"
                        EventType.Name as EventType,
                        Event.StartTime,
                        Event.ID,
                        Asset.AssetName
                    FROM
                        Event JOIN
                        Asset ON Event.AssetID = Asset.ID JOIN
                        EventType ON Event.EventTypeID = EventType.ID JOIN
                        Event as OrgEvt ON Event.MeterID = OrgEvt.MeterID AND Event.AssetID = OrgEvt.AssetID AND Event.ID != OrgEvt.ID
                    WHERE
                        OrgEvt.ID = {0}
                    ORDER BY
                        Event.StartTime DESC
                    ", EventID);
            }
        }
    }
}
