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

namespace openXDA.Controllers.Widgets
{
    [RoutePrefix("api/Widgets/PQI")]
    public class PQIController : ApiController
    {
        const string SettingsCategory = "systemSettings";

        public string ClientID
        {
            get
            {
                using (AdoDataConnection connection = new AdoDataConnection(SettingsCategory))
                    return connection.ExecuteScalar<string>($"SELECT Value From Setting Where Name = 'PQI.ClientID'") ?? "";
            }
        }

        public string ClientSecret
        {
            get
            {
                using (AdoDataConnection connection = new AdoDataConnection(SettingsCategory))
                    return connection.ExecuteScalar<string>($"SELECT Value From Setting Where Name = 'PQI.ClientSecret'") ?? "";
            }
        }

        public string Username
        {
            get
            {
                using (AdoDataConnection connection = new AdoDataConnection(SettingsCategory))
                    return connection.ExecuteScalar<string>($"SELECT Value From Setting Where Name = 'PQI.Username'") ?? "";
            }
        }

        public string Password
        {
            get
            {
                using (AdoDataConnection connection = new AdoDataConnection(SettingsCategory))
                    return connection.ExecuteScalar<string>($"SELECT Value From Setting Where Name = 'PQI.Password'") ?? "";
            }
        }
        public string PingURL
        {
            get
            {
                using (AdoDataConnection connection = new AdoDataConnection(SettingsCategory))
                    return connection.ExecuteScalar<string>($"SELECT Value From Setting Where Name = 'PQI.PingURL'") ?? "";
            }
        }
        public string BaseURL
        {
            get
            {
                using (AdoDataConnection connection = new AdoDataConnection(SettingsCategory))
                    return connection.ExecuteScalar<string>($"SELECT Value From Setting Where Name = 'PQI.BaseURL'") ?? "";
            }
        }

        [Route("GetEquipment/{eventID:int}"), HttpGet]
        public async Task<IHttpActionResult> GetEquipment(int eventID)
        {
            PQIWSClient pqiwsClient = new PQIWSClient(BaseURL, FetchAccessToken);
            PQIWSQueryHelper pqiwsQueryHelper = new PQIWSQueryHelper(() => new AdoDataConnection(SettingsCategory), pqiwsClient);
            return Ok(await pqiwsQueryHelper.GetAllImpactedEquipmentAsync(eventID));
        }

        [Route("GetCurves/{eventID:int}"), HttpGet]
        public async Task<IHttpActionResult> GetCurves(int eventID)
        {
            PQIWSClient pqiwsClient = new PQIWSClient(BaseURL, FetchAccessToken);
            PQIWSQueryHelper pqiwsQueryHelper = new PQIWSQueryHelper(() => new AdoDataConnection(SettingsCategory), pqiwsClient);
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