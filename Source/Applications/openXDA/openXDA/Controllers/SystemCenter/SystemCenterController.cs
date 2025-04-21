//******************************************************************************************************
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

using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using FaultData.DataResources;
using FaultData.DataSets;
using GSF.Data;
using log4net;
using Newtonsoft.Json.Linq;
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

        //Note: if we make a SCADA point model controller, we may want move this to that
        [Route("SCADAPoint/SCADAPointSearch"), HttpPost]
        public IHttpActionResult QuerySCADADataPoints([FromBody] JObject query, CancellationToken token)
        {
            try
            {
                MeterDataSet meterDataSet = new MeterDataSet();
                Action<object> configurator = GetConfigurator();
                meterDataSet.Configure = configurator;
                SCADADataResource scadaDataResource = meterDataSet.GetResource<SCADADataResource>();
                scadaDataResource.Initialize(meterDataSet);
                if (!query.ContainsKey("TagSearch"))
                {
                    return BadRequest("Missing query parameter: StartTime");
                }
                string tagSearch = query.Value<string>("TagSearch");

                int take;
                if (query.ContainsKey("Take"))
                {
                    take = query.Value<int>("Take");
                }
                else take = 50;
                return Ok(scadaDataResource.QuerySCADADataPoints(tagSearch, take));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        private Action<object> GetConfigurator()
        {
            ConfigurationLoader configurationLoader = new ConfigurationLoader(NodeHost.ID, CreateDbConnection);
            return configurationLoader.Configure;
        }

        private AdoDataConnection CreateDbConnection()
        {
            AdoDataConnection connection = NodeHost.CreateDbConnection();
            connection.DefaultTimeout = DataExtensions.DefaultTimeoutDuration;
            return connection;
        }

        #endregion

        #region [ Static ]

        // Static Fields
        private static readonly ILog Log = LogManager.GetLogger(typeof(DataFileController));

        #endregion
    }
}