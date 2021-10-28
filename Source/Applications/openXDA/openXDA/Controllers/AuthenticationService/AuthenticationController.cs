//******************************************************************************************************
//  Config/QueryController.cs - Gbtc
//
//  Copyright © 2018, Grid Protection Alliance.  All Rights Reserved.
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
//  05/25/2018 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using GSF.Data;
using GSF.Security;
using GSF.Web;
using Newtonsoft.Json.Linq;
using openXDA.Nodes;
using openXDA.Nodes.Types.Authentication;

namespace openXDA.Controllers
{
    [RoutePrefix("api/auth")]
    public class AuthenticationController : ApiController
    {
        private Host Host { get; }

        public AuthenticationController(Host host) =>
            Host = host;

        [Route("logon"), HttpGet]
        public async Task<IHttpActionResult> BaseLoginRequestAsync(CancellationToken cancellationToken)
        {
            try
            {
                // Redirect to the Correct URI
                Dictionary<string, string> query = Request.QueryParameters();
                string redirectUri;
                if (!query.TryGetValue("redirect_uri", out redirectUri))
                    throw new Exception($"Redirect URI is required");

                try
                {


                    string nonce;
                    string responseType;
                    string scope;
                    string clientId;


                    if (!query.TryGetValue("nonce", out nonce))
                        return Redirect($"{redirectUri}?error=invalid_request&error_description=nonce%20is%20missing");
                    if (!query.TryGetValue("response_type", out responseType))
                        return Redirect($"{redirectUri}?error=invalid_request&error_description=response_type%20is%20missing");
                    if (!query.TryGetValue("scope", out scope))
                        return Redirect($"{redirectUri}?error=invalid_request&error_description=scope%20is%20missing");
                    if (!query.TryGetValue("client_id", out clientId))
                        return Redirect($"{redirectUri}?error=invalid_request&error_description=client_id%20is%20missing");

                    if (responseType != "code")
                        return Redirect($"{redirectUri}?error=unsupported_response_type&error_description=only%20code%20is%20supported");

                    int nodeID = new Func<int>(() =>
                    {
                        using (AdoDataConnection connection = Host.CreateDbConnection())
                        {
                        // Validate clientID is in the Node Table
                        if (connection.ExecuteScalar<int>("SELECT COUNT(*) FROM ApplicationNode WHERE ID={0}", clientId) == 0)
                                throw new Exception($"Invalid NodeID '{clientId}'");

                            const string NodeQueryFormat =
                                "SELECT Node.ID " +
                                "FROM " +
                                "    Node JOIN " +
                                "    NodeType ON Node.NodeTypeID = NodeType.ID " +
                                "WHERE NodeType.TypeName = {0}";

                            Type nodeType = typeof(AuthenticationProviderNode);
                            return connection.ExecuteScalar<int>(NodeQueryFormat, nodeType.FullName);
                        }
                    })();

                    UserData user = ((SecurityPrincipal)Request.GetRequestContext().Principal).Identity.Provider.UserData;

                    void ConfigureRequest(HttpRequestMessage request)
                    {
                        string action = "AuthorizeCode";
                        NameValueCollection queryParameters = new NameValueCollection();
                        queryParameters.Add("appId", clientId ?? "");
                        queryParameters.Add("userId", user.LoginID ?? "");
                        queryParameters.Add("userName", user.Username ?? "");
                        queryParameters.Add("userGivenName", user.FirstName ?? "");
                        queryParameters.Add("userLastName", user.LastName ?? "");
                        queryParameters.Add("userPhone", user.PhoneNumber ?? "");
                        queryParameters.Add("userEmail", user.EmailAddress ?? "");
                        queryParameters.Add("userRoles", string.Join(",", user.Roles) ?? "");

                        string url = Host.BuildURL(nodeID, action, queryParameters);
                        request.Method = HttpMethod.Post;
                        request.RequestUri = new Uri(url);
                    }

                    string code;
                    using (HttpResponseMessage response = await Host.SendWebRequestAsync(ConfigureRequest, cancellationToken))
                    {
                        string content = await response.Content.ReadAsStringAsync();

                        if (!response.IsSuccessStatusCode)
                            throw new Exception($"Unable to get code from Authentication Node: {content}");

                        code = content.Trim('"');
                    }

                    return Redirect($"{redirectUri}?code={code}");
                }
                catch (Exception ex)
                {
                    return Redirect($"{redirectUri}?error=server_error&error_description={ex.ToString().Replace(" ","%20")}");
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("token"), HttpPost]
        public async Task<IHttpActionResult> RequestUserInfoAsync([FromBody] JObject data, CancellationToken cancellationToken)
        {
            try
            {
                // Redirect to the Correct URI
                JToken grantType;
                JToken clientId;
                JToken code;
                JToken redirectURI;

                if (!data.TryGetValue("grant_type", out grantType))
                    throw new Exception("POST Parameter 'grant_type' is required");
                if (!data.TryGetValue("client_id", out clientId))
                    throw new Exception("POST Parameter 'client_id' is required");
                if (!data.TryGetValue("code", out code))
                    throw new Exception("POST Parameter 'code' is required");
                if (!data.TryGetValue("redirect_uri", out redirectURI))
                    throw new Exception("POST Parameter 'redirect_uri' is required");

                if (grantType.ToString().ToLower() != "authorization_code")
                    throw new Exception("Only grant type authorization_code is accepted");

                int nodeID = new Func<int>(() =>
                {
                    using (AdoDataConnection connection = Host.CreateDbConnection())
                    {
                        const string NodeQueryFormat =
                            "SELECT Node.ID " +
                            "FROM " +
                            "    Node JOIN " +
                            "    NodeType ON Node.NodeTypeID = NodeType.ID " +
                            "WHERE NodeType.TypeName = {0}";

                        Type nodeType = typeof(AuthenticationProviderNode);
                        return connection.ExecuteScalar<int>(NodeQueryFormat, nodeType.FullName);
                    }
                })();

                void ConfigureRequest(HttpRequestMessage request)
                {
                    string action = "GetToken";
                    NameValueCollection queryParameters = new NameValueCollection();
                    queryParameters.Add("appId", clientId.ToString() ?? "");
                    queryParameters.Add("code", code.ToString());

                    string url = Host.BuildURL(nodeID, action, queryParameters);
                    request.Method = HttpMethod.Post;
                    request.RequestUri = new Uri(url);
                }

                using (HttpResponseMessage response = await Host.SendWebRequestAsync(ConfigureRequest, cancellationToken))
                {
                    string content = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                        throw new Exception($"Unable to get code from Authentication Node: {content}");

                    JObject token = JObject.Parse(content);
                        
                    if (!token.TryGetValue("access_token", out JToken accessToken) || string.IsNullOrEmpty(accessToken.ToString()))
                        throw new Exception("Token not available for this code and clientId");

                    return Ok(token);
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
