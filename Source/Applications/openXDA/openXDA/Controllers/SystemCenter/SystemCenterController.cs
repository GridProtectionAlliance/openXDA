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

using FaultData.DataOperations.TVA;
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
using System.Data.SqlClient;
using System.IO;
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

            [Category]
            [SettingName(RealTimeLightningDataProviderSettings.CategoryName)]
            public RealTimeLightningDataProviderSettings LightningDataSettings { get; }
                = new RealTimeLightningDataProviderSettings();

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
                Status = "N/A",
                Details = new List<StatusItem>()
            };

            Settings settings = new Settings();
            GetConfigurator()(settings);

            if (!settings.StructureQuerySettings.Enabled)
                return Ok(status);

            string stationKey;
            string lineKey;
            int halfLength;

            using (AdoDataConnection connection = CreateDbConnection())
            {
                int assetID = connection.ExecuteScalar<int>("SELECT TOP (1) ID FROM Asset WHERE AssetTypeID = 1");
                int locationID = connection.ExecuteScalar<int>("SELECT TOP (1) LocationID From AssetLocation WHERE AssetID = {0}", assetID);
                lineKey = connection.ExecuteScalar<string>("SELECT AssetKey FROM Asset WHERE ID = {0}", assetID);
                stationKey = connection.ExecuteScalar<string>("SELECT LocationKey FROM Location WHERE ID = {0}", locationID);

            if (String.IsNullOrEmpty(lineKey) || String.IsNullOrEmpty(stationKey))
                    return Ok(status);

                int lineLength = connection.ExecuteScalar<int>("SELECT TOP (1) Length FROM LineView WHERE AssetKey = {0}", lineKey);
                halfLength = lineLength / 2;
            }

            try
            {
                string url = string.Format(settings.StructureQuerySettings.URLFormat, stationKey, lineKey, halfLength);

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
                    status.Details.Add(new StatusItem() { Status = "Error", Description = "Structure query received invalid response. Check the logs for full details." });
                else if (ex is UriFormatException uriFormatException)
                    status.Details.Add(new StatusItem() { Status = "Error", Description = "Url formatting failed. Check StructureQuery.URLFormat in openXDA settings." });
                else if (ex is UnauthorizedAccessException)
                    status.Details.Add(new StatusItem() { Status = "Error", Description = "Failed to authorize structure query. Check the StructureQuery.UserName and StructureQuery.Password in openXDA settings." });
                else
                {
                    status.Details.Add(new StatusItem() { Status = "Error", Description = "Unexpected exception thrown during Structure Crawler query. Full exception message is available in openXDA logs." });
                }
            }

            return Ok(status);
        }

        [Route("LightningRealTimeData/Health")]
        public IHttpActionResult GetLightningRealTimeDataHealth()
        {
            AppStatus status = new AppStatus() { Status = "N/A", Details = new List<StatusItem>()};

            Settings settings = new Settings();
            GetConfigurator()(settings);

            string connectionString = settings.LightningDataSettings.RTLightningDatabaseConnectionString;

            Type connectionType = typeof(SqlConnection);
            Type adapterType = typeof(SqlDataAdapter);

            if (!string.IsNullOrWhiteSpace(connectionString))
            {
                try
                {
                    AdoDataConnection rtLightningConnection = new AdoDataConnection(connectionString, connectionType, adapterType);
                    status = GetConnectionStatus(rtLightningConnection);
                }
                catch (InvalidOperationException ex) 
                {
                    status.Status = "Error";
                    status.Details.Add(new StatusItem() 
                        {
                        Status = "Error",
                        Description = "Failed to connect using Connection String from RTLightning.RTLightningDatabaseConnectionString."
                        }
                    );
                }
            }
            return Ok(status);
        }

        [Route("LightningStructureData/Health")]
        public IHttpActionResult GetLightningStructureDataHealth()
        {
            AppStatus status = new AppStatus() { Status = "N/A", Details = new List<StatusItem>() };

            Settings settings = new Settings();
            GetConfigurator()(settings);

            string connectionString = settings.LightningDataSettings.MaximoConnectionString;
            string dataProviderString = settings.LightningDataSettings.MaximoDataProviderString;

            if (!(String.IsNullOrWhiteSpace(connectionString) || String.IsNullOrWhiteSpace(dataProviderString)))
            {
                try
                {
                status = GetConnectionStatus(new AdoDataConnection(connectionString, dataProviderString));
                }
                catch (InvalidOperationException ex)
                {

                    status.Status = "Error";
                    status.Details.Add(new StatusItem()
                    {
                        Status = "Error",
                        Description = "Failed to connect using Connection String and Data Provider String from RTLightning.MaximoConnectionString and RTLightning.MaximoDataProviderSetting."
                    }
                    );
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

        private AppStatus GetConnectionStatus(AdoDataConnection connection)
        {
            AppStatus testDatabaseStatus = new AppStatus()
            {
                Status = "Success",
                Details = new List<StatusItem>()
            };
            try
            {
                using (AdoDataConnection extConn = connection)
                {
                    string query;

                    if (extConn.IsOracle)
                        query = "SELECT 0 FROM dual"; // oracle adds the semicolon for you as a way to keep you from delimiting multiple statements.
                    else
                        query = "SELECT 0;";

                    int result = extConn.ExecuteScalar<int>(query);

                    if (result == 0)
                    {
                        testDatabaseStatus.Details.Add(new StatusItem()
                        {
                            Status = "Success",
                            Description = "Successfully connected to database."
                        });
                    }

                    else
                    {
                        testDatabaseStatus.Status = "Warning";
                    }
                }
            }
            catch (InvalidOperationException e)
            {
                Type innerExceptionType = e.InnerException.GetType();
                testDatabaseStatus.Status = "Error";
                if (e.InnerException is ArgumentException)
                {
                    testDatabaseStatus.Details.Add(new StatusItem() { 
                        Status = "Error",
                        Description = "ConnectionString contains errors."
                    });
                }
                if (e.InnerException is FileNotFoundException)
                {
                    testDatabaseStatus.Details.Add(new StatusItem()
                    {
                        Status = "Error",
                        Description = "Missing file or dependency."
                    });
                }
                if (e.InnerException is NullReferenceException)
                {
                    testDatabaseStatus.Details.Add(new StatusItem()
                    {
                        Status = "Error",
                        Description = "Could not load connection settings from configuration file."
                    });
                }
                if (e.InnerException is SqlException s)
                {
                    int number = s.Number;

                    // data provider string
                    if (number == 4060)
                    {
                        testDatabaseStatus.Details.Add(new StatusItem()
                        {
                            Status = "Success",
                            Description = "Successfully reached SQL server."
                        });
                        testDatabaseStatus.Details.Add(new StatusItem()
                        {
                            Status = "Error",
                            Description = "Failed to open database."
                        });
                    }

                    // failed to open an ADO connection - "a network-related or instance-specific error"
                    if (number == 53)
                    {
                        testDatabaseStatus.Details.Add(new StatusItem()
                        {
                            Status = "Error",
                            Description = "Failed to reach the server. Check that the connection string is correct and the server is accessible over network."
                        });
                    }

                    // failed for user permissions.
                    if (number == 18456)
                    {
                        testDatabaseStatus.Details.Add(new StatusItem()
                        {
                            Status = "Success",
                            Description = "Successfully reached server."
                        });
                        testDatabaseStatus.Details.Add(new StatusItem()
                        {
                            Status = "Error",
                            Description = "Failed to authenticate."
                        });
                    }
                }
                if (e.InnerException is Oracle.ManagedDataAccess.Client.OracleException o)
                {
                    // authentication error
                    if (o.Number == 1017)
                    {
                        testDatabaseStatus.Details.Add(new StatusItem()
                        {
                            Status = "Success",
                            Description = "Successfully reached SQL server."
                        });
                        testDatabaseStatus.Details.Add(new StatusItem()
                        {
                            Status = "Error",
                            Description = "Could not authenticate with the database. Please check username and password."
                        });
                    }

                    // no listener - port 
                    if (o.Number == 12541)
                    {
                        testDatabaseStatus.Details.Add(new StatusItem()
                        {
                            Status = "Error",
                            Description = "Found no listener on given port."
                        });
                    }

                    // cannot resolve hostname
                    if (o.Number == 12545)
                    {
                        testDatabaseStatus.Details.Add(new StatusItem()
                        {
                            Status = "Error",
                            Description = "Could not resolve hostname."
                        });
                    }

                    // failed to connect to server or parse connection string
                    if (o.Number == -6001)
                    {
                        testDatabaseStatus.Details.Add(new StatusItem()
                        {
                            Status = "Error",
                            Description = "Failed to connect to server or parse connection string."
                        });
                    }

                    // invalid transport address (like 'TCAP')
                    if (o.Number == 12533)
                    {
                        testDatabaseStatus.Details.Add(new StatusItem()
                        {
                            Status = "Error",
                            Description = "Invalid transport address syntax."
                        });
                    }
                }
                
            }
            return testDatabaseStatus;
        }

        #endregion

        #region [ Static ]

        // Static Fields
        private static readonly ILog Log = LogManager.GetLogger(typeof(DataFileController));

        #endregion
    }
}