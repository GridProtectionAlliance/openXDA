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
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

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
        /// <param name="host">Semicolon-separated list of hosts providing access to the API.</param>
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

        /// <summary>
        /// The CSRF token used to validate POST requests.
        /// </summary>
        public string AntiForgeryToken { get; private set; }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Sends a web request to the host using the credentials for API authentication.
        /// </summary>
        /// <param name="configure">Action that configures the HTTP request.</param>
        /// <param name="path">Path to the API endpoint locating the resource to be requested.</param>
        /// <param name="cancellationToken">Token used to cancel the request before it has completed.</param>
        /// <returns>The HTTP response returned by the host that handled the request.</returns>
        public async Task<HttpResponseMessage> SendWebRequestAsync(Action<HttpRequestMessage> configure, string path, GSF.Threading.CancellationToken cancellationToken = default)
        {
            string[] urls = HostURL.Split(';');
            bool success = false;
            int i = 0;

            while (i < urls.Length && !success)
            {
                using (HttpRequestMessage request = new HttpRequestMessage())
                {
                    string cleanHostURL = urls[i].Trim().TrimEnd('/');
                    string fullurl = $"{cleanHostURL}/{path.Trim().TrimStart('/')}";

                    request.RequestUri = new Uri(fullurl);
                    configure(request);

                    if (request.Method != HttpMethod.Get && AntiForgeryToken == null)
                        AntiForgeryToken = await GenerateAntiForgeryToken();

                    if (request.Method != HttpMethod.Get)
                        HttpClient.DefaultRequestHeaders.Add("X-GSF-Verify", AntiForgeryToken);

                    const string type = "XDA-API";
                    string decode = $"{APIKey}:{APIToken}";
                    Encoding utf8 = new UTF8Encoding(false);
                    byte[] credentialData = utf8.GetBytes(decode);
                    string credentials = Convert.ToBase64String(credentialData);
                    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(type, credentials);

                    HttpResponseMessage response;
                    try
                    {
                        response = await HttpClient.SendAsync(request, cancellationToken);
                    }
                    catch (HttpRequestException ex)
                    {
                        if (!IsUnreachableException(ex))
                            new Exception($"Unhandled Exception sending request to: {fullurl}", ex);
                    }
                    catch (Exception ex)
                    {
                        new Exception($"Unhandled Exception sending request to: {fullurl}",ex);
                    }
                    finally
                    {
                        i++;
                    }
                }
            }

            return new HttpResponseMessage(System.Net.HttpStatusCode.NotFound);
        }

        /// <summary>
        /// Recursively search for a <see cref="SocketException"/> that would indicate the host is currently unreachable.
        /// </summary>
        /// <param name="ex">The ancestor of the socket exception.</param>
        /// <returns><c>true</c> if there exists an socket exception with <see cref="SocketError.TimedOut"/> or <see cref="SocketError.ConnectionRefused"/></returns>
        private bool IsUnreachableException(Exception ex)
        {
            if (ex.GetType() == typeof(SocketException))
                return ((SocketException)ex).SocketErrorCode == SocketError.ConnectionRefused || ((SocketException)ex).SocketErrorCode == SocketError.TimedOut;
            if (ex.InnerException is null)
                return false;
            return IsUnreachableException(ex.InnerException);
        }

        /// <summary>
        /// Gets a CSRF token from the host for validating POST requests.
        /// </summary>
        /// <returns>The CSRF token.</returns>
        public async Task<string> GenerateAntiForgeryToken()
        {
            Action<HttpRequestMessage> tokenRequest = (HttpRequestMessage request) => {
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                request.Method = HttpMethod.Get;
            };

           using (HttpResponseMessage response = await SendWebRequestAsync(tokenRequest, "/api/rvht"))
           {
                if (!response.IsSuccessStatusCode)
                    throw new Exception($"Unable to get Anti Forger Token: {response.StatusCode} {response.ReasonPhrase}");
                return response.Content.ReadAsStringAsync().Result;
            }
        }

        #endregion

        #region [ Static ]

        private static HttpClient HttpClient { get; }
            = new HttpClient();

        #endregion
    }
}
