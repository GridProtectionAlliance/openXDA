//******************************************************************************************************
//  XDAControllerActivator.cs - Gbtc
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
using System.Linq.Expressions;
using System.Net.Http;
using System.Reflection;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using System.Web.Http.Routing;
using GSF.Data;
using openXDA.Nodes;

namespace openXDA.WebHosting
{
    public class XDAControllerActivator : IHttpControllerActivator
    {
        #region [ Constructors ]

        public XDAControllerActivator(Func<AdoDataConnection> connectionFactory, Host nodeHost)
        {
            FactoryLookup = new ConcurrentDictionary<Type, Func<IHttpController>>();
            ConnectionFactory = connectionFactory;
            NodeHost = nodeHost;
        }

        #endregion

        #region [ Properties ]

        private ConcurrentDictionary<Type, Func<IHttpController>> FactoryLookup { get; }
        private Func<AdoDataConnection> ConnectionFactory { get; }
        private Host NodeHost { get; }

        #endregion

        #region [ Methods ]

        public IHttpController Create(HttpRequestMessage request, HttpControllerDescriptor controllerDescriptor, Type controllerType)
        {
            IHttpController nodeController = LookUpNodeController(request);

            if (!(nodeController is null))
                return nodeController;

            Func<IHttpController> factory =
                LookUpFactory(controllerType) ??
                BuildFactory(controllerType) ??
                GetDefaultFactory(request, controllerDescriptor, controllerType);

            FactoryLookup.TryAdd(controllerType, factory);
            return factory();
        }

        private IHttpController LookUpNodeController(HttpRequestMessage request)
        {
            IHttpRouteData routeData = request.GetRouteData();

            if (!routeData.Values.TryGetValue("node", out object value))
                return null;

            if (!int.TryParse($"{value}", out int nodeID))
                return null;

            INode node = NodeHost.GetNode(nodeID);
            return node?.CreateWebController();
        }

        private Func<IHttpController> LookUpFactory(Type controllerType) =>
            FactoryLookup.TryGetValue(controllerType, out Func<IHttpController> factory)
                ? factory
                : null;

        private Func<IHttpController> BuildFactory(Type controllerType)
        {
            Expression GetConstructor<T>(T parameter)
            {
                Type[] types = { typeof(T) };
                ConstructorInfo constructor = controllerType.GetConstructor(types);

                if (constructor is null)
                    return null;

                Expression constantExpression = Expression.Constant(parameter);
                return Expression.New(constructor, constantExpression);
            }

            var newExpression =
                GetConstructor(ConnectionFactory) ??
                GetConstructor(NodeHost);

            if (newExpression is null)
                return null;

            var typeAsExpression = Expression.TypeAs(newExpression, typeof(IHttpController));
            var blockExpression = Expression.Block(typeAsExpression);
            var lambdaExpression = Expression.Lambda(blockExpression);
            return (Func<IHttpController>)lambdaExpression.Compile();
        }

        private Func<IHttpController> GetDefaultFactory(HttpRequestMessage request, HttpControllerDescriptor controllerDescriptor, Type controllerType) =>
            () => DefaultControllerActivator.Create(request, controllerDescriptor, controllerType);

        #endregion

        #region [ Static ]

        // Static Properties
        private static DefaultHttpControllerActivator DefaultControllerActivator { get; }
            = new DefaultHttpControllerActivator();

        #endregion
    }
}