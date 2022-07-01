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


using GSF.Data;
using GSF.Data.Model;
using System;
using System.Data;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace openXDA.APIAuthentication
{
    /// <summary>
    /// class to create and make an API Query to another App.
    /// This class handles the GSF token and authentication
    /// </summary>
    public class APIQuery
    {
        #region[Properties]

        private Func<AdoDataConnection> ConnectionFactory { get; }
        public string APIKey { get; }
        public string APIToken { get; }
        public string HostURL { get; }

        private int DbTimeout { get; set; } =
           DataExtensions.DefaultTimeoutDuration;

        #endregion[Properties]

        #region [ Constructors ]

        public APIQuery(Func<AdoDataConnection> connectionFactory, string apiKey, string apiToken, string host)
        {
            ConnectionFactory = connectionFactory;
            APIKey = apiKey;
            APIToken = apiToken;
            HostURL = host;
        }

        #endregion [ Constructors ]

        #region [ Methods ]

        private AdoDataConnection CreateDbConnection()
        {
            AdoDataConnection connection = ConnectionFactory();
            connection.DefaultTimeout = DbTimeout;
            return connection;
        }

        #endregion [ Methods ]

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

        #region [ Static ]

        private static HttpClient HttpClient { get; }
            = new HttpClient();

        #endregion [ Static ]
    }


}
