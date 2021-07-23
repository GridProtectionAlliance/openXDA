//******************************************************************************************************
//  PingClient.cs - Gbtc
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
//  07/26/2021 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace openXDA.PQI
{
    public class PingClient
    {
        public PingClient(string tokenURL)
        {
            TokenURL = tokenURL;
            IsExpiredFunc = () => true;
        }

        private string TokenURL { get; }
        private Func<bool> IsExpiredFunc { get; set; }

        public string AccessToken { get; private set; }
        public bool IsExpired => IsExpiredFunc();

        /// <summary>
        /// Exchange credentials for a new access token.
        /// </summary>
        /// <param name="clientCredential">The client application's credentials.</param>
        /// <param name="userCredential">The user's credentials.</param>
        /// <param name="cancellationToken">Token for cancelling the exchange.</param>
        /// <returns>The task that represents the exchange of credentials for an access token.</returns>
        public async Task ExchangeAsync(NetworkCredential clientCredential, NetworkCredential userCredential, CancellationToken cancellationToken = default)
        {
            void ConfigureRequest(HttpRequestMessage request)
            {
                request.RequestUri = new Uri(TokenURL);
                request.Method = HttpMethod.Post;

                string clientID = clientCredential.UserName;
                string clientSecret = clientCredential.Password;
                string credentials = $"{clientID}:{clientSecret}";
                byte[] encodedCredentials = Encoding.UTF8.GetBytes(credentials);
                string credentials64 = Convert.ToBase64String(encodedCredentials);
                request.Headers.Authorization = new AuthenticationHeaderValue("Basic", credentials64);

                List<KeyValuePair<string, string>> body = new List<KeyValuePair<string, string>>();
                body.Add(new KeyValuePair<string, string>("username", userCredential.UserName));
                body.Add(new KeyValuePair<string, string>("password", userCredential.Password));
                body.Add(new KeyValuePair<string, string>("grant_type", "password"));
                request.Content = new FormUrlEncodedContent(body);

                MediaTypeWithQualityHeaderValue acceptHeader = new MediaTypeWithQualityHeaderValue("application/json");
                request.Headers.Accept.Add(acceptHeader);
            }

            using (HttpResponseMessage response = await HttpClient.SendRequestAsync(ConfigureRequest, cancellationToken))
            {
                response.EnsureSuccessStatusCode();

                Stopwatch stopwatch = Stopwatch.StartNew();
                JObject content = await response.Content.ReadAsAsync<JObject>(cancellationToken);
                int expiration = content["expires_in"].Value<int>();
                AccessToken = content["access_token"].Value<string>();
                IsExpiredFunc = () => stopwatch.Elapsed.TotalSeconds > expiration;
            }
        }

        private static HttpClient HttpClient =>
            HttpClientProvider.GetClient();
    }
}
