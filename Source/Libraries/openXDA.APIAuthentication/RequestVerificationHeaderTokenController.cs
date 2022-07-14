//******************************************************************************************************
//  RequestVerificationHeaderTokenController.cs - Gbtc
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
//  07/05/2022 - Christoph Lackner
//       Generated original version of source code.
//
//******************************************************************************************************

using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using GSF.Web.Security;

namespace openXDA.APIAuthentication
{
    /// <summary>
    /// Provides the endpoint for requests to retrieve CSRF anti-forgery tokens.
    /// This needs to be included in all apps that support APIAuthentication.
    /// </summary>
    public class RequestVerificationHeaderTokenController : ApiController
    {
        /// <summary>
        /// Generates a new request verification header token and sends it to the client.
        /// </summary>
        /// <returns>The HTTP response that contains the token.</returns>
        public HttpResponseMessage Get()
        {
            string token = Request.GenerateRequestVerficationHeaderToken();
            Encoding utf8 = new UTF8Encoding(false);
            HttpContent content = new StringContent(token, utf8, "text/plain");
            return new HttpResponseMessage(HttpStatusCode.OK) { Content = content };
        }
    }

    namespace Extensions
    {
        /// <summary>
        /// Extension methods for using request verification header tokens.
        /// </summary>
        public static class RequestVerificationHeaderTokenControllerExtensions
        {
            /// <summary>
            /// Maps the <see cref="RequestVerificationHeaderTokenController"/> to the <c>api/rvht</c> path.
            /// </summary>
            /// <param name="routes">The route collection</param>
            public static void MapRequestVerificationHeaderTokenRoute(this HttpRouteCollection routes)
            {
                routes.MapHttpRoute(
                    name: "RequestVerificationHeaderToken",
                    routeTemplate: "api/rvht",
                    defaults: new
                    {
                        controller = "RequestVerificationHeaderToken"
                    }
                );
            }
        }
    }
}
