//******************************************************************************************************
//  ControllerConfig.cs - Gbtc
//
//  Copyright © 2017, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the MIT License (MIT), the "License"; you may
//  not use this file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://opensource.org/licenses/MIT
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  06/14/2017 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using GSF.Data;
using openXDA.Nodes;
using openXDA.Nodes.Types.Grafana;

namespace openXDA.Adapters
{
    public static class ControllerConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //Map custom API controllers
            config.Routes.MapHttpRoute(
                name: "JSONAPIs",
                routeTemplate: "api/JSONApi/{action}/{id}",
                defaults: new
                {
                    controller = "JSONApi",
                    id = RouteParameter.Optional
                }
            );

            config.Routes.MapHttpRoute(
                name: "GrafanaAPIs",
                routeTemplate: "api/Grafana/{action}/{id}",
                defaults: new
                {
                    controller = "Grafana",
                    id = RouteParameter.Optional
                }
            );

            config.Routes.MapHttpRoute(
                name: "PQMarkAPIs",
                routeTemplate: "api/pqmark/{action}/{modelName}/{id}",
                defaults: new
                {
                    controller = "PQMark",
                    modelName = RouteParameter.Optional,
                    id = RouteParameter.Optional
                }
            );

            config.Routes.MapHttpRoute(
                name: "OpenSTE",
                routeTemplate: "api/OpenSTE/{action}",
                defaults: new
                {
                    controller = "OpenSTE"
                }
            );

            config.Routes.MapHttpRoute(
                name: "HIDS",
                routeTemplate: "api/hids/{action}",
                defaults: new
                {
                    controller = "HIDS",
                    action = "QueryPoints"
                }
            );

            config.Routes.MapHttpRoute(
                name: "PeriodicDataDisplay",
                routeTemplate: "api/PeriodicDataDisplay/{action}",
                defaults: new
                {
                    controller = "PeriodicDataDisplay"
                }
            );

            config.Routes.MapHttpRoute(
                name: "TrendingDataDisplay",
                routeTemplate: "api/TrendingDataDisplay/{action}",
                defaults: new
                {
                    controller = "TrendingDataDisplay"
                }
            );

            config.Routes.MapHttpRoute(
                name: "DataQualitySummary",
                routeTemplate: "api/DataQualitySummary/{action}",
                defaults: new
                {
                    controller = "DataQualitySummary"
                }
            );

            config.Routes.MapHttpRoute(
                name: "ReportAPI",
                routeTemplate: "api/report/{id}/{name}",
                defaults: new
                {
                    controller = "Report"
                }
            );

            config.Routes.MapHttpRoute(
                name: "SpectralDataDisplay",
                routeTemplate: "api/SpectralDataDisplay/{action}",
                defaults: new
                {
                    controller = "SpectralDataDisplay"
                }
            );

            config.Routes.MapHttpRoute(
                name: "Nodes",
                routeTemplate: "Node/{node}/{action}/{*actionData}",
                defaults: new
                {
                    controller = "Node",
                    action = UrlParameter.Optional,
                    actionData = UrlParameter.Optional
                }
            );

            config.Routes.MapHttpRoute(
                name: "Grafana",
                routeTemplate: "grafana/{action}/{*actionData}",
                defaults: new
                {
                    controller = "Node",
                    action = UrlParameter.Optional,
                    actionData = UrlParameter.Optional,
                    nodeIDSelector = new NodeIDSelector(SelectGrafanaNode)
                }
            );
        }

        private static int SelectGrafanaNode(Host host, HttpRequestMessage request)
        {
            const string QueryFormat =
                "SELECT Node.ID " +
                "FROM " +
                "    Node JOIN " +
                "    NodeType ON Node.NodeTypeID = NodeType.ID " +
                "WHERE NodeType.TypeName = {0}";

            Type grafanaNodeType = typeof(GrafanaHostingNode);

            using (AdoDataConnection connection = host.CreateDbConnection())
            using (DataTable result = connection.RetrieveData(QueryFormat, grafanaNodeType.FullName))
            {
                IEnumerable<int> nodeIDs = result
                    .AsEnumerable()
                    .Select(row => row.ConvertField<int>("ID"));

                int? nodeID = null;

                foreach (int id in nodeIDs)
                {
                    if (!(nodeID is null))
                        throw new HttpResponseException(HttpStatusCode.BadRequest);

                    nodeID = id;
                }

                return nodeID ?? throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }
    }
}
