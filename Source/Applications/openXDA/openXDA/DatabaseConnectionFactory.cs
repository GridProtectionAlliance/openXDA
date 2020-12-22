//******************************************************************************************************
//  DatabaseConnectionFactory.cs - Gbtc
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
//  01/13/2021 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Data.SqlClient;
using GSF.Configuration;
using GSF.Data;

namespace openXDA
{
    public class DatabaseConnectionFactory
    {
        #region [ Members ]

        // Constants
        private const string DefaultSettingsCategory = "systemSettings";
        private const string DefaultSettingName = "ConnectionString";
        private const string DefaultSettingValue = "Data Source=localhost; Initial Catalog=openXDA; Integrated Security=SSPI";
        private const string SettingDescription = "Defines the connection to the openXDA database.";

        #endregion

        #region [ Constructors ]

        public DatabaseConnectionFactory(ConfigurationFile configurationFile)
            : this(configurationFile, DefaultSettingsCategory, DefaultSettingName)
        {
        }

        public DatabaseConnectionFactory(ConfigurationFile configurationFile, string settingsCategory)
            : this(configurationFile, settingsCategory, DefaultSettingName)
        {
        }

        public DatabaseConnectionFactory(ConfigurationFile configurationFile, string settingsCategory, string settingName)
        {
            ConfigurationFile = configurationFile;
            SettingsCategory = settingsCategory;
            SettingName = settingName;
            LoadConnectionString();
        }

        #endregion

        #region [ Properties ]

        private ConfigurationFile ConfigurationFile { get; }
        private string SettingsCategory { get; }
        private string SettingName { get; }

        private Type ConnectionType { get; } = typeof(SqlConnection);
        private Type AdapterType { get; } = typeof(SqlDataAdapter);
        private string ConnectionString { get; set; }

        #endregion

        #region [ Methods ]

        public AdoDataConnection CreateDbConnection() =>
            new AdoDataConnection(ConnectionString, ConnectionType, AdapterType);

        public void ReloadConfiguration()
        {
            AdoDataConnection.ReloadConfigurationSettings();
            LoadConnectionString();
        }

        private void LoadConnectionString()
        {
            CategorizedSettingsSection categorizedSettings = ConfigurationFile.Settings;
            CategorizedSettingsElementCollection category = categorizedSettings[SettingsCategory];
            category.Add(SettingName, DefaultSettingValue, SettingDescription);

            CategorizedSettingsElement setting = category[SettingName];
            ConnectionString = setting.Value;
        }

        #endregion
    }
}