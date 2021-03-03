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
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using GSF.Data;
using openXDA.Nodes;

namespace openXDA.Adapters
{
    public class NodeController : ApiController
    {
        private Host Host { get; }

        public NodeController(Host host) =>
            Host = host;

        [HttpGet, HttpPost, HttpPut, HttpDelete, HttpPatch]
        public Task<HttpResponseMessage> HandleRequestAsync(string node, string action, CancellationToken cancellationToken)
        {
            if (int.TryParse(node, out int nodeID))
                return ForwardAsync(nodeID, action, cancellationToken);

            return ForwardAsync(node, action, cancellationToken);
        }

        private async Task<HttpResponseMessage> ForwardAsync(string nodeType, string action, CancellationToken cancellationToken)
        {
            async Task<string> MessageNodeAsync(int nodeID)
            {
                using (HttpResponseMessage messageResponse = await ForwardAsync(nodeID, action, cancellationToken))
                {
                    if (!messageResponse.IsSuccessStatusCode)
                        return $"[Node {nodeID}] ERROR: {messageResponse.StatusCode} {messageResponse.ReasonPhrase}";

                    HttpContent content = messageResponse.Content;

                    Task<string> readResponseTask =
                        content?.ReadAsStringAsync() ??
                        Task.FromResult(string.Empty);

                    string responseMessage = await readResponseTask;

                    if (string.IsNullOrEmpty(responseMessage))
                        return $"[Node {nodeID}] Success";

                    return $"[Node {nodeID}] {responseMessage}";
                }
            }

            List<int> nodeIDs = QueryNodeIDs(nodeType);

            if (!nodeIDs.Any())
            {
                NotFoundResult notFoundResult = NotFound();
                return await notFoundResult.ExecuteAsync(cancellationToken);
            }

            IEnumerable<Task<string>> messageTasks = nodeIDs.Select(MessageNodeAsync);
            string[] results = await Task.WhenAll(messageTasks);
            string responseData = string.Join(Environment.NewLine, results);
            HttpResponseMessage response = new HttpResponseMessage();
            response.Content = new StringContent(responseData);
            return response;
        }

        private async Task<HttpResponseMessage> ForwardAsync(int nodeID, string action, CancellationToken cancellationToken)
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
                }, cancellationToken);
            }
            catch (InvalidOperationException)
            {
                if (Request.ShouldIncludeErrorDetail())
                    throw;

                IHttpActionResult notFound = NotFound();
                return await notFound.ExecuteAsync(cancellationToken);
            }
        }

        private List<int> QueryNodeIDs(string nodeType)
        {
            const string QueryFormat =
                "SELECT Node.ID " +
                "FROM " +
                "    Node JOIN " +
                "    NodeType ON Node.NodeTypeID = NodeType.ID " +
                "WHERE NodeType.Name = {0}";

            using (AdoDataConnection connection = Host.CreateDbConnection())
            using (DataTable result = connection.RetrieveData(QueryFormat, nodeType))
            {
                return result
                    .AsEnumerable()
                    .Select(row => row.ConvertField<int>("ID"))
                    .ToList();
            }
        }
    }
}
