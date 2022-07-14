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
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace openXDA.APIAuthentication
{
    /// <summary>
    /// Issues requests to the API using API authentication.
    /// This class handles the GSF token and authentication
    /// </summary>
    public class APIQuery
    {
        #region [ Members ]

        // Nested Types
        private class Host
        {
            public Host(string url) =>
                URL = url;

            public string URL { get; }
            public string AntiForgeryToken { get; set; }
        }

        private class HostUnreachableException : Exception
        {
        }

        // Fields
        private int m_hostIndex;

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Creates a new instance of the <see cref="APIQuery"/> class.
        /// </summary>
        /// <param name="apiKey">The API key used to identify the user of the API.</param>
        /// <param name="apiToken">The token used to authenticate the user of the API.</param>
        /// <param name="hostURL">URL that locates the host providing access to the API.</param>
        public APIQuery(string apiKey, string apiToken, string hostURL)
            : this(apiKey, apiToken, new[] { hostURL })
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="APIQuery"/> class.
        /// </summary>
        /// <param name="apiKey">The API key used to identify the user of the API.</param>
        /// <param name="apiToken">The token used to authenticate the user of the API.</param>
        /// <param name="hostURLs">List of URLs that locate the hosts providing access to the API.</param>
        public APIQuery(string apiKey, string apiToken, IEnumerable<string> hostURLs)
        {
            APIKey = apiKey;
            APIToken = apiToken;

            Hosts = hostURLs
                .Select(url => new Host(url))
                .ToList();

            // Select a random host for the first API call attempt
            if (Hosts.Count > 1)
                Interlocked.Exchange(ref m_hostIndex, InitialHostIndex);
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
        /// The list of URLs that locate the hosts providing access to the API.
        /// </summary>
        public IEnumerable<string> HostsURLs => Hosts
            .Select(host => host.URL);

        private List<Host> Hosts { get; }

        private int InitialHostIndex => Tuple
            .Create(Environment.TickCount, Environment.MachineName)
            .GetHashCode() % Hosts.Count;

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Sends a web request to the host using the credentials for API authentication.
        /// </summary>
        /// <param name="configure">Action that configures the HTTP request.</param>
        /// <param name="path">Path to the API endpoint locating the resource to be requested.</param>
        /// <param name="cancellationToken">Token used to cancel the request before it has completed.</param>
        /// <returns>The HTTP response returned by the host that handled the request.</returns>
        public async Task<HttpResponseMessage> SendWebRequestAsync(Action<HttpRequestMessage> configure, string path, CancellationToken cancellationToken = default)
        {
            int initialHostIndex = Interlocked.CompareExchange(ref m_hostIndex, 0, 0);

            void UpdateHostIndex(int hostIndex) =>
                Interlocked.CompareExchange(ref m_hostIndex, hostIndex, initialHostIndex);

            for (int i = 0; i < Hosts.Count; i++)
            {
                int hostIndex = (initialHostIndex + i) % Hosts.Count;
                Host host = Hosts[hostIndex];

                try
                {
                    HttpResponseMessage response = await SendWebRequestToAsync(host, configure, path, cancellationToken);
                    UpdateHostIndex(hostIndex);
                    return response;
                }
                catch (HostUnreachableException)
                {
                    continue;
                }
                catch
                {
                    UpdateHostIndex(hostIndex);
                    throw;
                }
            }

            return new HttpResponseMessage(HttpStatusCode.NotFound);
        }

        private async Task<HttpResponseMessage> SendWebRequestToAsync(Host host, Action<HttpRequestMessage> configure, string path, CancellationToken cancellationToken)
        {
            using (HttpRequestMessage request = BuildRequest(host, path, configure))
            {
                if (request.Method != HttpMethod.Get)
                {
                    if (host.AntiForgeryToken == null)
                        host.AntiForgeryToken = await GetAntiForgeryTokenAsync(host, cancellationToken);

                    request.Headers.Add("X-GSF-Verify", host.AntiForgeryToken);
                }

                return await CallAPIAsync(request, cancellationToken);
            }
        }

        private async Task<string> GetAntiForgeryTokenAsync(Host host, CancellationToken cancellationToken)
        {
            void ConfigureTokenRequest(HttpRequestMessage tokenRequest)
            {
                MediaTypeWithQualityHeaderValue mediaType = new MediaTypeWithQualityHeaderValue("text/plain");
                tokenRequest.Headers.Accept.Add(mediaType);
                tokenRequest.Method = HttpMethod.Get;
            }

            using (HttpRequestMessage tokenRequest = BuildRequest(host, "api/rvht", ConfigureTokenRequest))
            using (HttpResponseMessage tokenResponse = await CallAPIAsync(tokenRequest, cancellationToken))
            {
                tokenResponse.EnsureSuccessStatusCode();
                return await tokenResponse.Content.ReadAsStringAsync();
            }
        }

        private async Task<HttpResponseMessage> CallAPIAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                return await HttpClient.SendAsync(request, cancellationToken);
            }
            catch (HttpRequestException ex)
            {
                if (IndicatesHostIsUnreachable(ex))
                    throw new HostUnreachableException();

                throw;
            }
        }

        private HttpRequestMessage BuildRequest(Host host, string path, Action<HttpRequestMessage> configure)
        {
            HttpRequestMessage request = new HttpRequestMessage();

            try
            {
                string cleanHostURL = host.URL.Trim().TrimEnd('/');
                string cleanPath = path.Trim().TrimStart('/');
                string fullurl = $"{cleanHostURL}/{cleanPath}";
                request.RequestUri = new Uri(fullurl);
                configure(request);

                const string type = "XDA-API";
                string decode = $"{APIKey}:{APIToken}";
                Encoding utf8 = new UTF8Encoding(false);
                byte[] credentialData = utf8.GetBytes(decode);
                string credentials = Convert.ToBase64String(credentialData);
                request.Headers.Authorization = new AuthenticationHeaderValue(type, credentials);

                return request;
            }
            catch
            {
                request.Dispose();
                throw;
            }
        }

        private bool IndicatesHostIsUnreachable(Exception ex)
        {
            if (ex is SocketException socketException)
                return socketException.SocketErrorCode == SocketError.ConnectionRefused || socketException.SocketErrorCode == SocketError.TimedOut;
            if (ex.InnerException is null)
                return false;
            return IndicatesHostIsUnreachable(ex.InnerException);
        }

        #endregion

        #region [ Static ]

        private static HttpClient HttpClient { get; }
            = new HttpClient();

        #endregion
    }
}
