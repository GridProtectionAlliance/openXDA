﻿//******************************************************************************************************
//  SystemCenterController.cs - Gbtc
//
//  Copyright © 2024, Grid Protection Alliance.  All Rights Reserved.
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
//  10/08/2024 - G Santos
//       Generated original version of source code.
//
//******************************************************************************************************

using System.Threading.Tasks;
using System.Web.Http;
using log4net;
using openXDA.Nodes;

namespace openXDA.Controllers.Config
{
    [RoutePrefix("api/SystemCenter")]
    public class SystemCenterController : ApiController
    {
        #region [ Constructors ]

        public SystemCenterController(Host nodeHost)
        {
            NodeHost = nodeHost;
            ServiceConnection = ServiceConnection.Default;
        }

        #endregion

        #region [ Properties ]

        private Host NodeHost { get; }
        private ServiceConnection ServiceConnection { get; }

        #endregion

        #region [ Methods ]

        [Route("Alive"), HttpGet]
        public int Alive()
        {
            return 1;
        }

        [Route("SystemHealth"), HttpGet]
        public Task<string> QuerySystemHealth()
        {
            return ServiceConnection.Host.QueryEngineStatusAsync();
        }

        #endregion

        #region [ Static ]

        // Static Fields
        private static readonly ILog Log = LogManager.GetLogger(typeof(DataFileController));

        #endregion
    }
}