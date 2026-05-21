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

using FaultData.DataResources;
using FaultData.DataSets;
using GSF.Configuration;
using GSF.Data;
using GSF.Data.Model;
using log4net;
using Newtonsoft.Json.Linq;
using openXDA.Model;
using openXDA.Model.SystemCenter;
using openXDA.Nodes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using static FaultData.DataOperations.TVA.StructureQueryOperation;
using ConfigurationLoader = openXDA.Nodes.ConfigurationLoader;

namespace openXDA.Controllers.Config
{
    [RoutePrefix("api/SystemCenter")]
    public class SystemCenterController : ApiController
    {
        #region [ Members ]
        private class Settings
        {
            [Category]
            [SettingName(StructureQuerySection.CategoryName)]
            public StructureQuerySection StructureQuerySettings { get; } = new StructureQuerySection();
        }
        #endregion

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
                Log.Error(ex.Message, ex);
                return InternalServerError(ex);
            }
        }

        [Route("SCADAPoint/Health"), HttpGet]
        public IHttpActionResult GetSCADAPointHealth()
        {
            MeterDataSet meterDataSet = new MeterDataSet();
            Action<object> configurator = GetConfigurator();
            meterDataSet.Configure = configurator;
            SCADADataResource scadaDataResource = meterDataSet.GetResource<SCADADataResource>();
            scadaDataResource.Initialize(meterDataSet);
            return Ok(scadaDataResource.GetHistorianHealth());
        }

        [Route("StructureCrawler/Health")]
        public IHttpActionResult GetStructureCrawlerHealth()
        {

            AppStatus status = new AppStatus()
            {
                Status = "N/A"
            };

            Settings settings = new Settings();
            GetConfigurator()(settings);

            if (!settings.StructureQuerySettings.Enabled)
                return Ok(status);

            int stationKey;
            string lineKey;

            string query = @"
                SELECT TOP (1) *,
                (SELECT TOP (1) assetLocation.LocationID FROM AssetLocation assetLocation WHERE assetLocation.AssetID = asset.ID) AS LocationID
                FROM Asset asset
                    WHERE AssetTypeID = 1
            ";

            using (AdoDataConnection connection = CreateDbConnection())
            {
                DataTable result = connection.RetrieveData(query);

                if (result.Rows.Count == 0)
                    return Ok(status);

                if (result.Rows[0].IsNull("LocationID") || result.Rows[0].IsNull("AssetKey"))
                    return Ok(status);

                stationKey = result.Rows[0].Field<int>("LocationID");
                lineKey = result.Rows[0].Field<string>("AssetKey");
            }
            try
            {
                string url = string.Format(settings.StructureQuerySettings.URLFormat, stationKey, lineKey, 1);

                ICredentials credentials = null;
                if (settings.StructureQuerySettings.UserName != null && settings.StructureQuerySettings.Password != null && settings.StructureQuerySettings.Domain != null)
                {
                    NetworkCredential networkCredential = new NetworkCredential(settings.StructureQuerySettings.UserName, settings.StructureQuerySettings.Password, settings.StructureQuerySettings.Domain);
                    CredentialCache cache = new CredentialCache();
                    cache.Add(new Uri(url), "NTLM", networkCredential);
                    credentials = cache;
                }

                GetStructureInfo(url, credentials);
                status.Status = "Success";
                status.Details.Add(new StatusItem() { Status = "Success", Description = "Successful response received from Structure Crawler." });
            }
            catch(Exception ex)
            {
                status.Status = "Error";
                if (ex is HttpRequestException httpRequestException)
                    status.Details.Add(new StatusItem() { Status = "Error", Description = httpRequestException.Message });
                else if (ex is UriFormatException uriFormatException)
                    status.Details.Add(new StatusItem() { Status = "Error", Description = "Url formatting failed. Check StructureQuery.URLFormat in openXDA settings." });
                else
                {
                    Log.Error($"Unexpected exception thrown during Structure Crawler Query.", ex);
                    status.Details.Add(new StatusItem() { Status = "Error", Description = "Unexpected exception thrown during Structure Crawler query. Full exception message is available in openXDA logs." });
                }
            }

            return Ok(status);
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