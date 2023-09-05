//******************************************************************************************************
//  GrafanaQueryHelper.cs - Gbtc
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
//  08/31/2023 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using GSF.Security.Cryptography;
using log4net;
using Newtonsoft.Json.Linq;
using openXDA.Configuration;

namespace openXDA.Nodes.Types.Grafana
{
    public class GrafanaQueryHelper
    {
        #region [ Members ]

        // Nested Types
        public class UserDetail
        {
            public int id;
            public string email;
            public string name;
            public string login;
            public string theme;
            public int orgId;
            public bool isGrafanaAdmin;
        }

        public class OrgUserDetail
        {
            public int orgId;
            public int userId;
            public string email;
            public string login;
            public string role;
        }

        #endregion

        #region [ Constructors ]

        public GrafanaQueryHelper(HttpClient httpClient, GrafanaSection settings)
        {
            HttpClient = httpClient;
            Settings = settings;
        }

        #endregion

        #region [ Properties ]

        private HttpClient HttpClient { get; }
        private GrafanaSection Settings { get; }

        #endregion

        #region [ Methods ]

        public async Task<bool> CreateUserAsync(string username, int orgID, Action<UserDetail, string> userDetailAction)
        {
            try
            {
                // Create a new user
                JObject content = JObject.FromObject(new
                {
                    name = username,
                    email = "",
                    login = username,
                    password = PasswordGenerator.Default.GeneratePassword()
                });

                UserDetail userDetail = new UserDetail
                {
                    name = username,
                    login = username,
                    orgId = orgID
                };

                dynamic result = await CallAPIFunctionAsync(HttpMethod.Post, $"{Settings.HostedURL}/api/admin/users", content.ToString());

                string message = result.message;

                if (result.id is null)
                    return false;

                userDetail.id = (int)result.id;
                userDetailAction(userDetail, message);
                return true;
            }
            catch (Exception ex)
            {
                userDetailAction(null, ex.Message);
                return false;
            }
        }

        public async Task<bool> LookupUserAsync(string username, Action<UserDetail, string> userDetailAction)
        {
            try
            {
                JObject result;

                try
                {
                    result = await CallAPIFunctionAsync(HttpMethod.Get, $"{Settings.HostedURL}/api/users/lookup?loginOrEmail={username}");
                }
                catch (Exception ex)
                {
                    userDetailAction(null, GetExceptionMessage(ex));
                    return false;
                }

                if (result.TryGetValue("id", out _))
                {
                    try
                    {
                        userDetailAction(result.ToObject<UserDetail>(), null);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        userDetailAction(null, ex.Message);
                        return false;
                    }
                }

                if (result.TryGetValue("message", out JToken token))
                    userDetailAction(null, token.Value<string>());

                return false;
            }
            catch (Exception ex)
            {
                userDetailAction(null, ex.Message);
                return false;
            }
        }

        public async Task<OrgUserDetail[]> GetOrganizationUsersAsync(int orgID, Action<string> messageHandler)
        {
            try
            {
                return ((JArray)await CallAPIFunctionAsync(HttpMethod.Get, $"{Settings.HostedURL}/api/orgs/{orgID}/users", responseIsArray: true)).ToObject<OrgUserDetail[]>();
            }
            catch (Exception ex)
            {
                messageHandler(ex.Message);
                return Array.Empty<OrgUserDetail>();
            }
        }

        public async Task<bool> UpdateUserOrganizationalRoleAsync(int orgID, int userID, string organizationalRole, Action<string> messageHandler)
        {
            try
            {
                JObject content = JObject.FromObject(new
                {
                    role = organizationalRole
                });

                dynamic result = await CallAPIFunctionAsync(new HttpMethod("PATCH"), $"{Settings.HostedURL}/api/orgs/{orgID}/users/{userID}", content.ToString());
                string message = result.message;
                messageHandler(message);

                return message.Equals("Organization user updated", StringComparison.OrdinalIgnoreCase);
            }
            catch (Exception ex)
            {
                messageHandler(ex.Message);
                return false;
            }
        }

        public async Task<bool> AddUserToOrganizationAsync(int orgID, string username, string organizationalRole, Action<string> messageHandler)
        {
            try
            {
                JObject content = JObject.FromObject(new
                {
                    loginOrEmail = username,
                    role = organizationalRole
                });

                dynamic result = await CallAPIFunctionAsync(HttpMethod.Post, $"{Settings.HostedURL}/api/orgs/{orgID}/users", content.ToString());
                string message = result.message;
                messageHandler(message);

                return message.Equals("User added to organization", StringComparison.OrdinalIgnoreCase);
            }
            catch (Exception ex)
            {
                messageHandler(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Gets a flag that determines if configured Grafana server is responding.
        /// </summary>
        /// <returns><c>true</c> if Grafana server is responding; otherwise, <c>false</c>.</returns>
        public async Task<bool> IsServerRespondingAsync()
        {
            const string ServerCheckUrl = "/api/health";
            const string DebugMessage = "ServerIsResponding check for \"{0}" + ServerCheckUrl + "\" result:\r\n{1}";
            string hostedURL = Settings.HostedURL;

            try
            {
                // Test server response by hitting health page
                dynamic result = await CallAPIFunctionAsync(HttpMethod.Get, $"{hostedURL}{ServerCheckUrl}");
                Log.Debug(string.Format(DebugMessage, hostedURL, result?.ToString() ?? "null"));
                return !(result is null);
            }
            catch (Exception ex)
            {
                Log.Debug(string.Format(DebugMessage, hostedURL, ex.Message), ex);
                return false;
            }
        }

        public async Task<dynamic> CallAPIFunctionAsync(HttpMethod method, string url, string content = null, bool responseIsArray = false)
        {
            HttpRequestMessage request = new HttpRequestMessage(method, url);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Add(Settings.AuthProxyHeaderName, Settings.AdminUser);

            if (!(content is null))
                request.Content = new StringContent(content, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await HttpClient.SendAsync(request);

            content = await response.Content.ReadAsStringAsync();

            if (responseIsArray)
                return JArray.Parse(content);

            return JObject.Parse(content);
        }

        #endregion

        #region [ Static ]

        // Static Fields
        private static readonly ILog Log = LogManager.GetLogger(typeof(GrafanaQueryHelper));

        // Static Methods
        private static string GetExceptionMessage(Exception ex)
        {
            if (ex is AggregateException aggEx)
                return string.Join(", ", aggEx.InnerExceptions.Select(iex => iex.Message));

            return ex.Message;
        }

        #endregion
    }
}
