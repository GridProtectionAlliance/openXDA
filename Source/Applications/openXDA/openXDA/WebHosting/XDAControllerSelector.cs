//******************************************************************************************************
//  XDAControllerSelector.cs - Gbtc
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
//  02/17/2021 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Concurrent;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using System.Web.Http.Routing;
using openXDA.Nodes;

namespace openXDA.WebHosting
{
    public class XDAControllerSelector : DefaultHttpControllerSelector
    {
        #region [ Members ]

        // Nested Types
        private class NodeControllerDescriptor
        {
            #region [ Constructors ]

            public NodeControllerDescriptor(INode node, HttpControllerDescriptor httpControllerDescriptor)
            {
                NodeReference = new WeakReference<INode>(node);
                HttpControllerDescriptor = httpControllerDescriptor;
            }

            #endregion

            #region [ Properties ]

            public INode Node =>
                NodeReference.TryGetTarget(out INode node)
                    ? node
                    : null;

            public HttpControllerDescriptor HttpControllerDescriptor { get; }

            private WeakReference<INode> NodeReference { get; }

            #endregion
        }

        #endregion

        #region [ Constructors ]

        public XDAControllerSelector(HttpConfiguration configuration, Func<int, INode> nodeLookup)
            : base(configuration)
        {
            Configuration = configuration;
            NodeLookup = nodeLookup;
            NodeControllerDescriptorLookup = new ConcurrentDictionary<int, NodeControllerDescriptor>();
        }

        #endregion

        #region [ Properties ]

        private HttpConfiguration Configuration { get; }
        private Func<int, INode> NodeLookup { get; }
        private ConcurrentDictionary<int, NodeControllerDescriptor> NodeControllerDescriptorLookup { get; }

        #endregion

        #region [ Methods ]

        public override HttpControllerDescriptor SelectController(HttpRequestMessage request)
        {
            INode node = TryGetNode(request);

            if (node is null)
                return base.SelectController(request);

            int nodeID = node.Definition.ID;

            if (NodeControllerDescriptorLookup.TryGetValue(nodeID, out NodeControllerDescriptor lookupDescriptor))
            {
                // Node type can change at runtime based on changes to
                // configuration so a weak reference is used to verify
                // that the cached descriptor is still valid for this node ID
                if (lookupDescriptor.Node == node)
                    return lookupDescriptor.HttpControllerDescriptor;
            }

            IHttpController nodeController = node.CreateWebController();

            if (nodeController is null)
                return base.SelectController(request);

            Type controllerType = nodeController.GetType();
            string controllerName = controllerType.Name;
            HttpControllerDescriptor httpControllerDescriptor = new HttpControllerDescriptor(Configuration, controllerName, controllerType);
            NodeControllerDescriptor nodeControllerDescriptor = new NodeControllerDescriptor(node, httpControllerDescriptor);
            NodeControllerDescriptorLookup[nodeID] = nodeControllerDescriptor;
            return httpControllerDescriptor;
        }

        private INode TryGetNode(HttpRequestMessage request)
        {
            IHttpRouteData routeData = request.GetRouteData();

            if (!routeData.Values.TryGetValue("node", out object value))
                return null;

            if (!int.TryParse($"{value}", out int nodeID))
                return null;

            return NodeLookup(nodeID);
        }

        #endregion
    }
}