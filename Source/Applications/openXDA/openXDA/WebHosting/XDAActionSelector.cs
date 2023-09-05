//******************************************************************************************************
//  XDAActionSelector.cs - Gbtc
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
//  02/24/2021 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System.Linq;
using System.Web.Http.Controllers;
using System.Web.Http.Routing;
using openXDA.Adapters;

namespace openXDA.WebHosting
{
    public class XDAActionSelector : ApiControllerActionSelector
    {
        public override HttpActionDescriptor SelectAction(HttpControllerContext controllerContext)
        {
            IHttpRouteData routeData = controllerContext.RouteData;

            bool isNodeRequest =
                routeData.Values.ContainsKey("node") ||
                routeData.Values.ContainsKey(nameof(NodeIDSelector)) ||
                routeData.Values.ContainsKey(nameof(NodeTypeSelector));

            if (!isNodeRequest)
                return base.SelectAction(controllerContext);

            ILookup<string, HttpActionDescriptor> actionMapping = GetActionMapping(controllerContext.ControllerDescriptor);

            if (routeData.Values.TryGetValue("action", out object action))
            {
                string actionName = action.ToString();
                if (actionMapping[actionName].Any())
                    return base.SelectAction(controllerContext);
            }

            HttpActionDescriptor descriptor = null;

            foreach (HttpActionDescriptor handleRequestDescriptor in actionMapping["HandleRequestAsync"])
            {
                if (!(descriptor is null))
                    return base.SelectAction(controllerContext);

                descriptor = handleRequestDescriptor;
            }

            return descriptor ?? base.SelectAction(controllerContext);
        }
    }
}