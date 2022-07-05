//******************************************************************************************************
//  APIQuery.cs - Gbtc
//
//  Copyright © 2022, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the Eclipse Public License -v 1.0 (the "License"); you may
//  not use this file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://www.opensource.org/licenses/eclipse-1.0.php
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  07/01/2022 - Christoph Lackner
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using GSF.Data;

namespace openXDA.APIAuthentication
{
    /// <summary>
    /// Issues requests to the API using API authentication.
    /// This class handles the GSF token and authentication
    /// </summary>
    public class APIQuery
    {
        #region [ Constructors ]

        /// <summary>
        /// Creates a new instance of the <see cref="APIQuery"/> class.
        /// </summary>
        /// <param name="apiKey">The API key used to identify the user of the API.</param>
        /// <param name="apiToken">The token used to authenticate the user of the API.</param>
        /// <param name="host">Comma-separated list of hosts providing access to the API.</param>
        public APIQuery(string apiKey, string apiToken, string host)
        {
            APIKey = apiKey;
            APIToken = apiToken;
            HostURL = host;
        }

        #endregion

        #region[ Properties ]

        /// <summary>
        /// The API key identifying the user of the API.
        /// </summary>
        public string APIKey { get; }

        /// <summary>
        /// The API token used to authenticate the user.
        /// </summary>
        public string APIToken { get; }

        /// <summary>
        /// A comma-separated list of URLs used to locate the hosts in
        /// the cluster of systems that provide access to the API.
        /// </summary>
        public string HostURL { get; }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Sends a web request to the host using the credentials for API authentication.
        /// </summary>
        /// <param name="configure"></param>
        /// <param name="endpoint"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> SendWebRequestAsync(Action<HttpRequestMessage> configure, string endpoint, GSF.Threading.CancellationToken cancellationToken = default)
        {
            string[] urls = HostURL.Split(';');
            bool success = false;
            int i = 0;

            while (i < urls.Length && !success)
            {
                using (HttpRequestMessage request = new HttpRequestMessage())
                {
                    string cleanHostURL = urls[i].Trim().TrimEnd('/');
                    string fullurl = $"{cleanHostURL}/{endpoint.Trim().TrimStart('/')}";

                    request.RequestUri = new Uri(fullurl);
                    configure(request);

                    const string type = "XDA-API";
                    string decode = $"{APIKey}:{APIToken}";
                    Encoding utf8 = new UTF8Encoding(false);
                    byte[] credentialData = utf8.GetBytes(decode);
                    string credentials = Convert.ToBase64String(credentialData);
                    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(type, credentials);

                    HttpResponseMessage r = await HttpClient.SendAsync(request, cancellationToken);
                    i++;
                    if (r.StatusCode != System.Net.HttpStatusCode.NotFound || i == urls.Length)
                        return r;
                }
            }

            return new HttpResponseMessage(System.Net.HttpStatusCode.NotFound);
        }
      
        #endregion

        #region [ Static ]

        private static HttpClient HttpClient { get; }
            = new HttpClient();

        #endregion
    }
}
