//******************************************************************************************************
//  SQLDataSource.cs - Gbtc
//
//  Copyright © 2022, Grid Protection Alliance.  All Rights Reserved.
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
//  01/13/2022 - Christoph Lackner
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.ComponentModel;
using System.Configuration;
using System.Xml.Linq;
using GSF.Configuration;
using GSF.Data;
using GSF.Scheduling;
using openXDA.Model;

namespace openXDA.NotificationDataSources
{
    public class SQLDataSource : ITriggeredDataSource, IScheduledDataSource
    {
        #region [ Members ]

        // Nested Types
        private class DataSourceSettings
        {
            const string DefaultDataProviderString =
                "AssemblyName={System.Data, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089}; " +
                "ConnectionType=System.Data.SqlClient.SqlConnection; " +
                "AdapterType=System.Data.SqlClient.SqlDataAdapter";

            public DataSourceSettings(Action<object> configure) =>
                configure(this);

            [Setting]
            [DefaultValue(null)]
            public string ConnectionString { get; set; }

            [Setting]
            [DefaultValue(DefaultDataProviderString)]
            public string DataProviderString { get; set; }

            [Setting]
            [DefaultValue("SELECT NULL FOR XML PATH('Data')")]
            [SettingName("SQLStatement")]
            public string SQL { get; set; }

            [Setting]
            [DefaultValue(DataExtensions.DefaultTimeoutDuration)]
            public int QueryTimeout { get; set; }
        }

        #endregion

        #region [ Constructors ]

        public SQLDataSource(Func<AdoDataConnection> xdaConnectionFactory) =>
            XDAConnectionFactory = xdaConnectionFactory;

        #endregion

        #region [ Properties ]

        private Func<AdoDataConnection> XDAConnectionFactory { get; }
        private DataSourceSettings Settings { get; set; }

        #endregion

        #region [ Methods ]

        public void Configure(Action<object> configurator) =>
            Settings = new DataSourceSettings(configurator);

        XElement ITriggeredDataSource.Process(Event evt)
        {
            using (AdoDataConnection connection = CreateDBConnection())
            {
                string xml = connection.ExecuteScalar<string>(Settings.SQL, evt.ID);
                return XElement.Parse(xml);
            }
        }

        XElement IScheduledDataSource.Process(DateTime xdaNow)
        {
            using (AdoDataConnection connection = CreateDBConnection())
            {
                string xml = connection.ExecuteScalar<string>(Settings.SQL, xdaNow);
                return XElement.Parse(xml);
            }
        }

        private AdoDataConnection CreateDBConnection()
        {
            if (Settings is null)
                throw new InvalidOperationException("SQL data source must be configured before processing");

            if (string.IsNullOrEmpty(Settings.ConnectionString))
                return CreateXDAConnection();

            string connectionString = Settings.ConnectionString;
            string dataProviderString = Settings.DataProviderString;
            AdoDataConnection connection = new AdoDataConnection(connectionString, dataProviderString);
            connection.DefaultTimeout = Settings.QueryTimeout;
            return connection;
        }

        private AdoDataConnection CreateXDAConnection()
        {
            if (XDAConnectionFactory is null)
                throw new ArgumentNullException(nameof(Settings.ConnectionString));

            return XDAConnectionFactory();
        }

        #endregion
    }
}
