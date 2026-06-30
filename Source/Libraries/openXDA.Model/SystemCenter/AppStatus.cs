//******************************************************************************************************
//  AppStatus.cs - Gbtc
//
//  Copyright © 2026, Grid Protection Alliance.  All Rights Reserved.
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
//  03/19/2026 - Natalie Beatty
//       Generated original version of source code.
//
//******************************************************************************************************

using GSF.Configuration;
using GSF.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;

namespace openXDA.Model.SystemCenter
{
    public class StatusItem
    {
        public string Status { get; set; }
        public string Description { get; set; }
    }

    public class AppStatus
    {

        public string Status { get; set; }

        public List<StatusItem> Details { get; set; }

        public static AppStatus CheckConnectivity(string settingsCategory)
        {
            AppStatus status = new AppStatus()
            {
                Status = "N/A",
                Details = new List<StatusItem>()
            };

            if (string.IsNullOrWhiteSpace(settingsCategory))
                return status;


            // Only need to establish data types and load settings once per defined section since they are being loaded from config file
            string connectionString, dataProviderString;

            // Load connection settings from the system settings category				
            ConfigurationFile config = ConfigurationFile.Current;
            CategorizedSettingsElementCollection configSettings = config.Settings[settingsCategory];

            if (configSettings.Count == 0)
            {
                return status;
            }


            connectionString = configSettings["ConnectionString"]?.Value ?? "";
            dataProviderString = configSettings["DataProviderString"]?.Value ?? "";

            return CheckConnectivity(connectionString, dataProviderString);
        }

        public static AppStatus CheckConnectivity(string connectionString, string dataProviderString)
        {
            AppStatus status = new AppStatus()
            {
                Status = "Error",
                Details = new List<StatusItem>()
            };

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                status.Status = "Error";
                status.Details.Add(new StatusItem()
                {
                    Status = "Error",
                    Description = "Connection string is not defined."
                });
                return status;
            }

            if (string.IsNullOrWhiteSpace(dataProviderString))
            {
                status.Status = "Error";
                status.Details.Add(new StatusItem()
                {
                    Status = "Error",
                    Description = "Data provider is not defined."
                });
                return status;
            }

            try
            {
                using (AdoDataConnection connection = new AdoDataConnection(connectionString, dataProviderString))
                    status = CheckConnectivity(connection);
            }
            catch (Exception e)
            {
                status.Status = "Error";
                if (e.InnerException is KeyNotFoundException) 
                {
                    status.Details.Add(new StatusItem()
                    {
                        Status = "Error",
                        Description = "Could not parse the data provider string."
                    });
                }

                if (e.InnerException is FileNotFoundException)
                {
                    status.Details.Add(new StatusItem()
                    {
                        Status = "Error",
                        Description = "Missing file or dependency."
                    });
                }

                if (e is System.IO.FileLoadException)
                {
                    status.Details.Add(new StatusItem()
                    {
                        Status = "Error",
                        Description = "Mismatched file or dependency."
                    });
                }

                if (e is InvalidOperationException ioe)
                {
                    status.Details.AddRange(GetInvalidOperationStatusItems(ioe));
                }
            }

            return status;
        }

        public static AppStatus CheckConnectivity(String connectionString, Type connectionType, Type adapterType)
        {
            AppStatus status = new AppStatus()
            {
                Status = "Error",
                Details = new List<StatusItem>()
            }; 

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                status.Status = "Error";
                status.Details.Add(new StatusItem()
                {
                    Status = "Error",
                    Description = "Connection string is not defined."
                });
                return status;
            }

            try
            {
                using (AdoDataConnection connection = new AdoDataConnection(connectionString, connectionType, adapterType))
                    status = CheckConnectivity(connection);
            }
            catch (Exception e)
            {
                status.Status = "Error";
                if (e.InnerException is KeyNotFoundException)
                {
                    status.Details.Add(new StatusItem()
                    {
                        Status = "Error",
                        Description = "Could not parse the data provider string."
                    });
                }

                if (e.InnerException is FileNotFoundException)
                {
                    status.Details.Add(new StatusItem()
                    {
                        Status = "Error",
                        Description = "Missing file or dependency."
                    });
                }

                if (e is System.IO.FileLoadException)
                {
                    status.Details.Add(new StatusItem()
                    {
                        Status = "Error",
                        Description = "Mismatched file or dependency."
                    });
                }

                if (e is InvalidOperationException ioe)
                {
                    status.Details.AddRange(GetInvalidOperationStatusItems(ioe));
                }
            }
            return status;
        }

        public static AppStatus CheckConnectivity(AdoDataConnection connection)
        {
            AppStatus testDatabaseStatus = new AppStatus()
            {
                Status = "Success",
                Details = new List<StatusItem>()
            };
            try
            {

                string query;

                if (connection.IsOracle)
                    query = "SELECT 0 FROM dual"; // oracle adds the semicolon for you as a way to keep you from delimiting multiple statements.
                else
                    query = "SELECT 0;";

                int result = connection.ExecuteScalar<int>(query);

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
            catch (InvalidOperationException e)
            {
                testDatabaseStatus.Status = "Error";
                testDatabaseStatus.Details.AddRange(GetInvalidOperationStatusItems(e));
            }
            return testDatabaseStatus;
        }
   
        public static List<StatusItem> GetInvalidOperationStatusItems(InvalidOperationException e)
        {
            List<StatusItem> items = new List<StatusItem>();

            if (e.InnerException is ArgumentException)
            {
                items.Add(new StatusItem()
                {
                    Status = "Error",
                    Description = "Connection string contains errors."
                });
            }
            if (e.InnerException is FileNotFoundException)
            {
                items.Add(new StatusItem()
                {
                    Status = "Error",
                    Description = "Missing file or dependency."
                });
            }
            if (e.InnerException is NullReferenceException)
            {
                items.Add(new StatusItem()
                {
                    Status = "Error",
                    Description = "Could not load connection settings."
                });
            }
            if (e.InnerException is SqlException s)
            {
                int number = s.Number;

                // data provider string
                if (number == 4060)
                {
                    items.Add(new StatusItem()
                    {
                        Status = "Success",
                        Description = "Successfully reached SQL server."
                    });
                    items.Add(new StatusItem()
                    {
                        Status = "Error",
                        Description = "Failed to open database."
                    });
                }

                // failed to open an ADO connection - "a network-related or instance-specific error"
                if (number == 53)
                {
                    items.Add(new StatusItem()
                    {
                        Status = "Error",
                        Description = "Failed to reach the server. Check that the connection string is correct and the server is accessible over network."
                    });
                }

                // failed for user permissions.
                if (number == 18456)
                {
                    items.Add(new StatusItem()
                    {
                        Status = "Success",
                        Description = "Successfully reached server."
                    });
                    items.Add(new StatusItem()
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
                    items.Add(new StatusItem()
                    {
                        Status = "Success",
                        Description = "Successfully reached Oracle server."
                    });
                    items.Add(new StatusItem()
                    {
                        Status = "Error",
                        Description = "Could not authenticate with the database. Please check username and password."
                    });
                }

                // no listener - port 
                if (o.Number == 12541)
                {
                    items.Add(new StatusItem()
                    {
                        Status = "Error",
                        Description = "Found no listener on given port."
                    });
                }

                // cannot resolve hostname
                if (o.Number == 12545)
                {
                    items.Add(new StatusItem()
                    {
                        Status = "Error",
                        Description = "Could not resolve hostname."
                    });
                }

                // failed to connect to server or parse connection string
                if (o.Number == -6001)
                {
                    items.Add(new StatusItem()
                    {
                        Status = "Error",
                        Description = "Failed to connect to server or parse connection string."
                    });
                }

                // invalid transport address (like 'TCAP')
                if (o.Number == 12533)
                {
                    items.Add(new StatusItem()
                    {
                        Status = "Error",
                        Description = "Invalid transport address syntax."
                    });
                }
            }
            return items;
        }
    }

    public class NamedAppStatus : AppStatus
    {
        public string Name { get; set; }
    }
}
