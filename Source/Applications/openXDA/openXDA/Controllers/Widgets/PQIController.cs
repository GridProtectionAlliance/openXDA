//******************************************************************************************************
//  PQIController.cs - Gbtc
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
//  10/23/2025 - G. Santos
//       Migrated code from EventWidgets to here.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using GSF.Data;
using openXDA.PQI;
using openXDA.Model;

namespace openXDA.Controllers.Widgets
{
    /// <summary>
    /// Controller that handles fetching PQI information for an openXDA <see cref="Event"/>s
    /// </summary>
    [RoutePrefix("api/Widgets/PQI")]
    public class PQIController : ApiController
    {
        Func<AdoDataConnection> m_connectionFactory;

        /// <summary>
        /// Constructor to pull  a connection factory from the XDA controller activator.
        /// </summary> 
        public PQIController(Func<AdoDataConnection> connectionFactory)
        {
            m_connectionFactory = connectionFactory;
        }

        public string ClientID
        {
            get
            {
                using (AdoDataConnection connection = m_connectionFactory())
                    return connection.ExecuteScalar<string>($"SELECT Value From Setting Where Name = 'PQI.ClientID'") ?? "";
            }
        }

        public string ClientSecret
        {
            get
            {
                using (AdoDataConnection connection = m_connectionFactory())
                    return connection.ExecuteScalar<string>($"SELECT Value From Setting Where Name = 'PQI.ClientSecret'") ?? "";
            }
        }

        public string Username
        {
            get
            {
                using (AdoDataConnection connection = m_connectionFactory())
                    return connection.ExecuteScalar<string>($"SELECT Value From Setting Where Name = 'PQI.Username'") ?? "";
            }
        }

        public string Password
        {
            get
            {
                using (AdoDataConnection connection = m_connectionFactory())
                    return connection.ExecuteScalar<string>($"SELECT Value From Setting Where Name = 'PQI.Password'") ?? "";
            }
        }
        public string PingURL
        {
            get
            {
                using (AdoDataConnection connection = m_connectionFactory())
                    return connection.ExecuteScalar<string>($"SELECT Value From Setting Where Name = 'PQI.PingURL'") ?? "";
            }
        }
        public string BaseURL
        {
            get
            {
                using (AdoDataConnection connection = m_connectionFactory())
                    return connection.ExecuteScalar<string>($"SELECT Value From Setting Where Name = 'PQI.BaseURL'") ?? "";
            }
        }

        /// <summary>
        /// Retrieves a list of impacted <see cref="Equipment"/> based on an openXDA <see cref="Event"/> ID.
        /// </summary>
        /// <param name="eventID">ID of an openXDA <see cref="Event"/></param>
        /// <returns>List of impacted <see cref="Equipment"/></returns>
        [Route("GetEquipment/{eventID:int}"), HttpGet]
        public async Task<IHttpActionResult> GetEquipment(int eventID)
        {
            PQIWSClient pqiwsClient = new PQIWSClient(BaseURL, FetchAccessToken);
            PQIWSQueryHelper pqiwsQueryHelper = new PQIWSQueryHelper(() => m_connectionFactory(), pqiwsClient);
            return Ok(await pqiwsQueryHelper.GetAllImpactedEquipmentAsync(eventID));
        }

        /// <summary>
        /// Retrieves a list of test curves <see cref="Equipment"/> based on an openXDA <see cref="Event"/> ID.
        /// </summary>
        /// <param name="eventID">ID of an openXDA <see cref="Event"/></param>
        /// <returns>
        /// List of <see cref="List{T}"/> test curves, each one of which is a <see cref="Tuple{T1,T2}"/> of 
        /// <see cref="TestCurve"/>s and a <see cref="List{T}"/> of <see cref="TestCurvePoint"/>s.
        /// </returns>
        [Route("GetCurves/{eventID:int}"), HttpGet]
        public async Task<IHttpActionResult> GetCurves(int eventID)
        {
            PQIWSClient pqiwsClient = new PQIWSClient(BaseURL, FetchAccessToken);
            PQIWSQueryHelper pqiwsQueryHelper = new PQIWSQueryHelper(() => m_connectionFactory(), pqiwsClient);
            List<Tuple<TestCurve, List<TestCurvePoint>>> r = await pqiwsQueryHelper.GetAllTestCurvesAsync(new List<int>() { eventID });
            return Ok(r);
        }

        private string FetchAccessToken()
        {
            NetworkCredential clientCredential = new NetworkCredential(ClientID, ClientSecret);
            NetworkCredential userCredential = new NetworkCredential(Username, Password);
            PingClient pingClient = new PingClient(PingURL);
            Task exchangeTask = pingClient.ExchangeAsync(clientCredential, userCredential);
            exchangeTask.GetAwaiter().GetResult();
            return pingClient.AccessToken;
        }
    }
}