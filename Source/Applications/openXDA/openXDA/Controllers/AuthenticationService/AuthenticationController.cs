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
        public IHttpActionResult BaseLoginRequest(CancellationToken token)
        {
            try
            {
                // Redirect to the Correct URI
                Dictionary<string, string> query = Request.QueryParameters();

                string redirectUri;
                string nonce;
                string responseType;
                string scope;
                string clientId;

                if (!query.TryGetValue("redirect_uri", out redirectUri))
                    throw new Exception("GET Parameter 'redirect_uri' is required");
                if (!query.TryGetValue("nonce", out nonce))
                    throw new Exception("GET Parameter 'nonce' is required");
                if (!query.TryGetValue("response_type", out responseType))
                    throw new Exception("GET Parameter 'responseType' is required");
                if (!query.TryGetValue("scope", out scope))
                    throw new Exception("GET Parameter 'scope' is required");
                if (!query.TryGetValue("client_id", out clientId))
                    throw new Exception("GET Parameter 'clientId' is required");

                if (responseType != "code")
                    throw new Exception("Only ResponseType code is supported");

                //Validate clientID is in the Node Table
                using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
                    if (connection.ExecuteScalar<int>("SELECT COUNT(*) FROM ApplicationNode WHERE ID={0}", clientId) == 0)
                        throw new Exception($"Invalid NodeID '{clientId}'");

                UserData user = ((SecurityPrincipal)Request.GetRequestContext().Principal).Identity.Provider.UserData;

                void ConfigureRequest(HttpRequestMessage request)
                {
                    Type nodeType = typeof(AuthenticationProviderNode);
                    string action = "AuthorizeCode";
                    NameValueCollection queryParameters = new NameValueCollection();
                    queryParameters.Add("appId", clientId ?? "") ;
                    queryParameters.Add("userId", user.LoginID ?? "");
                    queryParameters.Add("userName", user.Username ?? "");
                    queryParameters.Add("userGivenName", user.FirstName ?? "");
                    queryParameters.Add("userLastName", user.LastName ?? "");
                    queryParameters.Add("userPhone", user.PhoneNumber ?? "");
                    queryParameters.Add("userEmail", user.EmailAddress ?? "");
                    queryParameters.Add("userRoles", string.Join(",", user.Roles) ?? "");

                    string url = Host.BuildURL(nodeType, action, queryParameters);
                    request.Method = HttpMethod.Post;
                    request.RequestUri = new Uri(url);
                }

                string code;
                using (HttpResponseMessage response = Host.SendWebRequestAsync(ConfigureRequest).Result)
                {
                    if (response.IsSuccessStatusCode)
                        code = response.Content.ReadAsStringAsync().Result.Trim('"');
                    else
                        throw new Exception($"Unable to get code from Authentication Node: {response.Content.ReadAsStringAsync().Result}");
                }

                return Redirect($"{redirectUri}?code={code}");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("token"), HttpPost]
        public IHttpActionResult RequestUserInfo([FromBody] JObject data)
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

                void ConfigureRequest(HttpRequestMessage request)
                {
                    Type nodeType = typeof(AuthenticationProviderNode);
                    string action = "GetToken";
                    NameValueCollection queryParameters = new NameValueCollection();
                    queryParameters.Add("appId", clientId.ToString() ?? "");
                    queryParameters.Add("code", code.ToString());

                    string url = Host.BuildURL(nodeType, action, queryParameters);
                    request.Method = HttpMethod.Post;
                    request.RequestUri = new Uri(url);
                }

                using (HttpResponseMessage response = Host.SendWebRequestAsync(ConfigureRequest).Result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        JObject token = JObject.Parse(response.Content.ReadAsStringAsync().Result);
                        
                        if (!token.TryGetValue("access_token", out JToken accessToken) || string.IsNullOrEmpty(accessToken.ToString()))
                            throw new Exception("Token not available for this code and clientId");

                        return Ok(token);
                    }
                        
                    else
                        throw new Exception($"Unable to get code from Authentication Node: {response.Content.ReadAsStringAsync().Result}");
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
