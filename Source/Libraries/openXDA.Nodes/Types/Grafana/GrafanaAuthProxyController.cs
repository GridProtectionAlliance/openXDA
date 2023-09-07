//******************************************************************************************************
//  GrafanaAuthProxyController.cs - Gbtc
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
//  08/28/2023 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using GSF.Collections;
using GSF.Security;
using log4net;
using Newtonsoft.Json.Linq;
using openXDA.Configuration;
using CancellationToken = System.Threading.CancellationToken;
using Http = System.Net.WebRequestMethods.Http;

namespace openXDA.Nodes.Types.Grafana
{
    /// <summary>
    /// Creates a reverse proxy to a hosted Grafana instance that handles integrated authentication.
    /// </summary>
    public class GrafanaAuthProxyController : ApiController
    {
        #region [ Constructors ]

        public GrafanaAuthProxyController(GrafanaHostingNode node) =>
            Node = node;

        #endregion

        #region [ Properties ]

        private GrafanaHostingNode Node { get; }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Handle proxy of specified Grafana URL.
        /// </summary>
        /// <param name="url">URL to proxy.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Proxied response.</returns>
        [AcceptVerbs(Http.Get, Http.Head, Http.Post, Http.Put, Http.MkCol), HttpDelete, HttpPatch]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public async Task<HttpResponseMessage> HandleRequestAsync(string action, string actionData, CancellationToken cancellationToken)
        {
            string url = action ?? string.Empty;
            GrafanaSection settings = Node.QueryGrafanaSettings();

            if (!string.IsNullOrEmpty(action) && !string.IsNullOrEmpty(actionData))
                url += $"/{actionData}";

            // Handle special URL commands
            switch (url.ToLowerInvariant())
            {
                case "syncusers":
                    return HandleSynchronizeUsersRequest(Request, RequestContext.Principal as SecurityPrincipal);
                case "servertime":
                    return HandleServerTimeRequest(Request);
                case "logout":
                    return HandleGrafanaLogoutRequest(Request, settings);
                case "api/login/ping":
                    return HandleGrafanaLoginPingRequest(Request, RequestContext.Principal as SecurityPrincipal);
            }

            if (url.StartsWith("servervar/", StringComparison.Ordinal))
                return HandleServerVarRequest(Request);

            if (url.StartsWith("avatar/", StringComparison.OrdinalIgnoreCase))
                return HandleGrafanaAvatarRequest(Request);

            // Proxy all other requests
            SecurityPrincipal securityPrincipal = RequestContext.Principal as SecurityPrincipal;

            if (securityPrincipal?.Identity is null)
                throw new SecurityException($"User \"{RequestContext.Principal?.Identity.Name}\" is unauthorized.");

            Request.Headers.Add(settings.AuthProxyHeaderName, securityPrincipal.Identity.Name);
            Request.RequestUri = new Uri($"{settings.HostedURL}/{url}{Request.RequestUri.Query}");

            if (Request.Method == HttpMethod.Get)
                Request.Content = null;

            HttpResponseMessage response = await Node.GrafanaHttpClient.SendAsync(Request, cancellationToken);
            HttpStatusCode statusCode = response.StatusCode;

            if (statusCode == HttpStatusCode.NotFound || statusCode == HttpStatusCode.Unauthorized)
            {
                // HACK: Internet Explorer sometimes applies cached authorization headers to concurrent AJAX requests
                if (!(Request.Headers.Authorization is null))
                {
                    // Clone request to allow modification
                    HttpRequestMessage retryRequest = await CloneRequest();
                    retryRequest.Headers.Authorization = null;

                    HttpResponseMessage retryResponse = await Node.GrafanaHttpClient.SendAsync(retryRequest, cancellationToken);
                    HttpStatusCode retryStatusCode = retryResponse.StatusCode;

                    if (retryStatusCode != HttpStatusCode.NotFound && retryStatusCode != HttpStatusCode.Unauthorized)
                    {
                        response = retryResponse;
                        statusCode = retryStatusCode;
                    }
                }
            }

            if (statusCode == HttpStatusCode.NotFound || statusCode == HttpStatusCode.Unauthorized)
            {
                ThreadPool.QueueUserWorkItem(state =>
                {
                    string username = (state as SecurityPrincipal)?.Identity?.Name;

                    if (string.IsNullOrEmpty(username))
                        return;

                    try
                    {
                        // Validate user has a role defined in latest security context
                        Dictionary<string, string[]> securityContext = Node.GetLatestSecurityContext(username);

                        if (securityContext is null)
                            throw new InvalidOperationException("Failed to load security context");

                        string newUserMessage = securityContext.ContainsKey(username) ? "" : $"New user \"{username}\" encountered. ";

                        Log.Info($"{newUserMessage}Security context with {securityContext.Count:N0} users and associated roles queued for Grafana user synchronization.");
                    }
                    catch (Exception ex)
                    {
                        Log.Error($"Failed while queuing Grafana user synchronization for new user \"{username}\": {ex.Message}", ex);
                    }
                },
                RequestContext.Principal);
            }

            // Always keep last visited Grafana dashboard in a client session cookie
            if (url.StartsWith("api/dashboards/", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrWhiteSpace(Request.Headers?.Referrer?.AbsolutePath))
                response.Headers.AddCookies(new[] { new CookieHeaderValue(settings.LastDashboardCookieName, $"{Request.Headers.Referrer.AbsolutePath}") { Path = "/" } });

            return response;
        }

        private HttpResponseMessage HandleSynchronizeUsersRequest(HttpRequestMessage request, SecurityPrincipal securityPrincipal)
        {
            Dictionary<string, string[]> userRoles = Node.StartUserSynchronization(securityPrincipal?.Identity?.Name);

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                RequestMessage = request,
                Content = new StringContent($@"
                    <html>
                    <head>
                        <title>Grafana User Synchronization</title>
                        <link rel=""shortcut icon"" href=""@GSF/Web/Shared/Images/Icons/favicon.ico"" />
                    </head>
                    <body>
                        Security context with {userRoles.Count:N0} users and associated roles queued for Grafana user synchronization.
                    </body>
                    </html>
                ",
                Encoding.UTF8, "text/html")
            };
        }

        private async Task<HttpRequestMessage> CloneRequest()
        {
            HttpRequestMessage clone = new HttpRequestMessage(Request.Method, Request.RequestUri)
            {
                Version = Request.Version
            };

            foreach (KeyValuePair<string, object> property in Request.Properties)
                clone.Properties.Add(property);

            foreach (KeyValuePair<string, IEnumerable<string>> header in Request.Headers)
                clone.Headers.TryAddWithoutValidation(header.Key, header.Value);

            if (!(Request.Content is null))
            {
                MemoryStream content = new MemoryStream();

                await Request.Content.CopyToAsync(content);
                content.Position = 0;

                clone.Content = new StreamContent(content);

                if (!(Request.Content.Headers is null))
                {
                    foreach (KeyValuePair<string, IEnumerable<string>> header in Request.Content.Headers)
                        clone.Content.Headers.TryAddWithoutValidation(header.Key, header.Value);
                }
            }

            return clone;
        }

        #endregion

        #region [ Static ]

        // Static Fields
        private static readonly ILog Log = LogManager.GetLogger(typeof(GrafanaAuthProxyController));

        // Static Properties

        /// <summary>
        /// Gets or sets reference top global settings.
        /// </summary>
        public static dynamic GlobalSettings { get; set; }

        // Static Methods

        private static HttpResponseMessage HandleServerTimeRequest(HttpRequestMessage request)
        {
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                RequestMessage = request,
                Content = new StringContent(JObject.FromObject(new
                {
                    serverTime = $"{DateTime.UtcNow:MM/dd/yyyy HH:mm:ss.fff} UTC"
                })
                .ToString(), Encoding.UTF8, "application/json")
            };
        }

        private static HttpResponseMessage HandleServerVarRequest(HttpRequestMessage request)
        {
            string variableValue = "NaN";

            if (!(GlobalSettings is null))
            {
                string[] parts = request.RequestUri.AbsolutePath.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length > 1)
                {
                    string variableName = parts[parts.Length - 1];

                    if (!string.IsNullOrWhiteSpace(variableValue))
                    {
                        // We tightly control which variables to expose to prevent unnecessary data leakage
                        variableValue = new Func<string>(() =>
                        {
                            switch (variableName.ToUpperInvariant())
                            {
                                case "COMPANYNAME": return GlobalSettings.CompanyName;
                                case "COMPANYACRONYM": return GlobalSettings.CompanyAcronym;
                                case "NODEID": return GlobalSettings.NodeID.ToString();
                                case "NOMINALFREQUENCY": return GlobalSettings.NominalFrequency.ToString();
                                case "SYSTEMNAME": return GlobalSettings.SystemName;
                                default: return "NaN";
                            }
                        })();
                    }
                }
            }

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                RequestMessage = request,
                Content = new StringContent(JObject.FromObject(new
                {
                    value = variableValue
                })
                .ToString(), Encoding.UTF8, "application/json")
            };
        }

        private static HttpResponseMessage HandleGrafanaLogoutRequest(HttpRequestMessage request, GrafanaSection settings)
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Redirect) { RequestMessage = request };
            Uri requestUri = request.RequestUri, referrerUri = request.Headers.Referrer;

            if (referrerUri.AbsolutePath.ToLowerInvariant().Contains("/grafana/"))
            {
                // Handle user requested logout
                response.Headers.Location = new Uri($"{requestUri.Scheme}://{requestUri.Host}:{requestUri.Port}/Logout.cshtml");
            }
            else
            {
                // Handle automated logout, returning to original Grafana page
                string lastDashboard = null;

                if (!string.IsNullOrWhiteSpace(referrerUri.Query))
                {
                    Dictionary<string, string> parameters = referrerUri.ParseQueryString().ToDictionary();

                    if (parameters.TryGetValue("referrer", out string referrer))
                    {
                        string base64Path = WebUtility.UrlDecode(referrer);
                        byte[] pathBytes = Convert.FromBase64String(base64Path);
                        referrerUri = new Uri(Encoding.UTF8.GetString(pathBytes));
                        lastDashboard = referrerUri.PathAndQuery;
                    }
                }

                if (string.IsNullOrWhiteSpace(lastDashboard))
                {
                    // Without knowing the original referrer, the best we can do is restore last screen only
                    CookieHeaderValue lastDashboardCookie = request.Headers.GetCookies(settings.LastDashboardCookieName).FirstOrDefault();
                    lastDashboard = lastDashboardCookie?[settings.LastDashboardCookieName].Value;
                }

                if (string.IsNullOrWhiteSpace(lastDashboard))
                {
                    // As a last resort (i.e., no cookie or original referrer), return to the Grafana home page
                    response.Headers.Location = new Uri($"{requestUri.Scheme}://{requestUri.Host}:{requestUri.Port}/grafana");
                }
                else
                {
                    if (!lastDashboard.Contains("?"))
                        lastDashboard += $"?orgId={settings.OrganizationID}";

                    response.Headers.Location = new Uri($"{requestUri.Scheme}://{requestUri.Host}:{requestUri.Port}{lastDashboard}");
                    Log.Info($"Reloading previous Grafana dashboard: {lastDashboard}");
                }
            }

            return response;
        }

        private static HttpResponseMessage HandleGrafanaLoginPingRequest(HttpRequestMessage request, SecurityPrincipal securityPrincipal)
        {
            string userLoginState = securityPrincipal?.Identity is null ? "Unauthorized" : "Logged in";

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                RequestMessage = request,
                Content = new StringContent(JObject.FromObject(new
                {
                    message = userLoginState
                })
                .ToString(), Encoding.UTF8, "application/json")
            };
        }

        private static HttpResponseMessage HandleGrafanaAvatarRequest(HttpRequestMessage request)
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.MovedPermanently) { RequestMessage = request };
            Uri uri = request.RequestUri;

            response.Headers.Location = new Uri($"{uri.Scheme}://{uri.Host}:{uri.Port}/Images/Icons/openXDA.png");

            return response;
        }

        #endregion
    }
}
