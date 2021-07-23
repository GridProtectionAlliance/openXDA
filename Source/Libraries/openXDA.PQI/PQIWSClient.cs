//******************************************************************************************************
//  PQIWSClient.cs - Gbtc
//
//  Copyright © 2021, Grid Protection Alliance.  All Rights Reserved.
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
//  07/22/2021 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace openXDA.PQI
{
    public class PQIWSClient
    {
        private const string BasePath = "PQDashboard";

        public PQIWSClient(string baseURL, Func<string> tokenProvider)
        {
            BaseURL = baseURL;
            TokenProvider = tokenProvider;
        }

        private string BaseURL { get; }
        private Func<string> TokenProvider { get; }

        public async Task<FacilityInfo> GetFacilityInfoAsync(int facilityID, CancellationToken cancellationToken = default)
        {
            string url = BuildURL(BaseURL, BasePath, "GetFacilityInfo");
            string queryString = $"facilityID={facilityID}";

            void ConfigureRequest(HttpRequestMessage request)
            {
                request.RequestUri = new Uri($"{url}?{queryString}");
                request.Method = HttpMethod.Get;
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", TokenProvider());

                MediaTypeWithQualityHeaderValue acceptHeader = new MediaTypeWithQualityHeaderValue("application/json");
                request.Headers.Accept.Add(acceptHeader);
            }

            using (HttpResponseMessage response = await HttpClient.SendRequestAsync(ConfigureRequest, cancellationToken))
                return await response.Content.ReadAsAsync<FacilityInfo>();
        }

        public async Task<bool> IsImpactedAsync(int facilityID, double magnitude, double duration, CancellationToken cancellationToken = default)
        {
            string url = BuildURL(BaseURL, BasePath, "IsImpacted");

            string queryString =
                $"facilityID={facilityID}&" +
                $"magnitude={magnitude}&" +
                $"duration={duration}";

            void ConfigureRequest(HttpRequestMessage request)
            {
                request.RequestUri = new Uri($"{url}?{queryString}");
                request.Method = HttpMethod.Get;
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", TokenProvider());

                MediaTypeWithQualityHeaderValue acceptHeader = new MediaTypeWithQualityHeaderValue("application/json");
                request.Headers.Accept.Add(acceptHeader);
            }

            using (HttpResponseMessage response = await HttpClient.SendRequestAsync(ConfigureRequest, cancellationToken))
                return await response.Content.ReadAsAsync<bool>();
        }

        public async Task<List<Equipment>> GetImpactedEquipmentAsync(int facilityID, double magnitude, double duration, CancellationToken cancellationToken = default)
        {
            string url = BuildURL(BaseURL, BasePath, "GetEquipmentImpacted");

            string queryString =
                $"facilityID={facilityID}&" +
                $"magnitude={magnitude}&" +
                $"duration={duration}";

            void ConfigureRequest(HttpRequestMessage request)
            {
                request.RequestUri = new Uri($"{url}?{queryString}");
                request.Method = HttpMethod.Get;
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", TokenProvider());

                MediaTypeWithQualityHeaderValue acceptHeader = new MediaTypeWithQualityHeaderValue("application/json");
                request.Headers.Accept.Add(acceptHeader);
            }

            using (HttpResponseMessage response = await HttpClient.SendRequestAsync(ConfigureRequest, cancellationToken))
                return await response.Content.ReadAsAsync<List<Equipment>>();
        }

        private static string BuildURL(params string[] parts)
        {
            const string Separator = "/";
            string combinedPath = "";

            foreach (string path in parts)
            {
                if (path == null)
                    throw new ArgumentNullException(nameof(parts), "One of the strings in the array is null.");

                if (path.Length == 0)
                    continue;

                if (combinedPath.Length == 0)
                    combinedPath = path;
                else if (path.StartsWith(Separator))
                    combinedPath = path;
                else if (combinedPath.EndsWith(Separator))
                    combinedPath += path;
                else
                    combinedPath += Separator + path;
            }

            return combinedPath;
        }

        private static HttpClient HttpClient =>
            HttpClientProvider.GetClient();
    }
}
