//******************************************************************************************************
//  NodeController.cs - Gbtc
//
//  Copyright © 2021, Grid Protection Alliance.  All Rights Reserved.
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
//  02/13/2021 - Stephen C. Wills
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
using openXDA.Nodes;

namespace openXDA.Adapters
{
    public class NodeController : ApiController
    {
        private Host Host { get; }

        public NodeController(Host host) =>
            Host = host;

        [HttpGet, HttpPost, HttpPut, HttpDelete, HttpPatch]
        public Task<HttpResponseMessage> HandleRequest(int node, string action, CancellationToken cancellationToken) =>
            Forward(node, action, cancellationToken);

        private async Task<HttpResponseMessage> Forward(int nodeID, string action, CancellationToken cancellationToken)
        {
            INode node = Host.GetNode(nodeID);

            try
            {
                return await Host.SendWebRequestAsync(request =>
                {
                    request.Method = Request.Method;
                    request.Version = Request.Version;

                    NameValueCollection queryParameters = Request.RequestUri.ParseQueryString();
                    string url = Host.BuildURL(nodeID, action, queryParameters);
                    request.RequestUri = new Uri(url);
                    request.Headers.Host = request.RequestUri.Authority;

                    if (request.Method != HttpMethod.Get)
                        request.Content = Request.Content;

                    foreach (KeyValuePair<string, object> property in Request.Properties)
                        request.Properties.Add(property.Key, property.Value);

                    foreach (KeyValuePair<string, IEnumerable<string>> header in Request.Headers)
                        request.Headers.TryAddWithoutValidation(header.Key, header.Value);
                });
            }
            catch (InvalidOperationException)
            {
                if (Request.ShouldIncludeErrorDetail())
                    throw;

                IHttpActionResult notFound = NotFound();
                return await notFound.ExecuteAsync(cancellationToken);
            }
        }
    }
}
