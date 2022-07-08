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

        /// <summary>
        /// The GSF AntiForgeryToken used to secure POST Requests
        /// </summary>
        public string AntiForgeryToken { get; private set; } = null;
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
        /// Checks Recursively if an <see cref="HttpRequestException"/> is caused by the Host being unreachable.
        /// </summary>
        /// <param name="innerEx">The <see cref="Exception"/></param>
        /// <returns><see cref="true"/> if there is a <see cref="SocketException"/> with <see cref="SocketError.TimedOut"/> or <see cref="SocketError.ConnectionRefused"/></returns>
        private bool IsUnreachableException(Exception innerEx)
        {
            if (innerEx.GetType() == typeof(SocketException))
                return ((SocketException)innerEx).SocketErrorCode == SocketError.ConnectionRefused || ((SocketException)innerEx).SocketErrorCode == SocketError.TimedOut;
            if (innerEx.InnerException is null)
                return false;
            return IsUnreachableException(innerEx.InnerException);
        }

        /// <summary>
        /// Gets AntiForgeryToken from the <see cref="RequestVerificationHeaderTokenController"/>
        /// </summary>
        /// <returns>string token</returns>
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
