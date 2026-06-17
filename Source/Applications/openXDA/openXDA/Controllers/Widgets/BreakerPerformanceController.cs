//******************************************************************************************************
//  BreakerPerformanceController.cs - Gbtc
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
//  08/08/2023 - C. Lackner
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Data;
using System.Web.Http;
using GSF.Data;
using GSF.Data.Model;
using openXDA.Model;

namespace openXDA.Controllers.Widgets
{
    /// <summary>
    /// Controller that handles fetching breaker performance history for an openXDA event.
    /// </summary>
    [RoutePrefix("api/Widgets/BreakerPerformance")]
    public class BreakerPerformanceController : ApiController
    {
        private readonly Func<AdoDataConnection> m_connectionFactory;

        /// <summary>
        /// Constructor to pull a connection factory from the XDA controller activator.
        /// </summary>
        public BreakerPerformanceController(Func<AdoDataConnection> connectionFactory)
        {
            m_connectionFactory = connectionFactory;
        }

        [Route(""), HttpGet]
        public DataTable GetBreakerPerformance(int eventID)
        {
            if (eventID <= 0)
                return new DataTable();

            using (AdoDataConnection connection = m_connectionFactory())
            {
                Event evt = new TableOperations<Event>(connection).QueryRecordWhere("ID = {0}", eventID);
                return BreakerHistoryTable(evt.AssetID);
            }
        }

        private DataTable BreakerHistoryTable(int assetID)
        {
            using (AdoDataConnection connection = m_connectionFactory())
                return connection.RetrieveData("SELECT * FROM BreakerHistory WHERE BreakerId = {0}", assetID);
        }
    }
}
