//******************************************************************************************************
//  ConfigurationLoader.cs - Gbtc
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
//  01/13/2022 - C. Lackner
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using GSF;
using GSF.Configuration;
using GSF.Data;
using GSF.Data.Model;
using log4net;
using openXDA.Model;

namespace FaultData.DataWriters.Emails
{
    public class ConfigurationLoader
    {
        #region [ Constructors ]

        public ConfigurationLoader(Func<AdoDataConnection> connectionFactory)
        {
            ConnectionFactory = connectionFactory;
            LazyConfigureAction = new Lazy<Action<object>>(CreateConfigureAction);
        }

        #endregion

        #region [ Properties ]

        private Func<AdoDataConnection> ConnectionFactory { get; }
        private Lazy<Action<object>> LazyConfigureAction { get; }

        #endregion

        #region [ Methods ]

        public void Configure(object obj) => LazyConfigureAction.Value(obj);

        private Action<object> CreateConfigureAction()
        {
            string connectionString = LoadConnectionString();
            return obj => ConnectionStringParser.ParseConnectionString(connectionString, obj);
        }

        private string LoadConnectionString()
        {
            string ToConnectionString(IEnumerable<string[]> settingList, int index)
            {
                string ToValue(IEnumerable<string[]> grouping) =>
                    grouping.Any(setting => index < setting.Length - 2)
                        ? ToConnectionString(grouping, index + 1)
                        : grouping.First().Last();

                return settingList
                    .Where(setting => index < setting.Length - 1)
                    .GroupBy(setting => setting[index], StringComparer.OrdinalIgnoreCase)
                    .ToDictionary(grouping => grouping.Key, ToValue, StringComparer.OrdinalIgnoreCase)
                    .JoinKeyValuePairs();
            }

            using (AdoDataConnection connection = ConnectionFactory())
            {
                var datasourceSettings = LoadDataSourceSettings(connection);
                var xdaSettings = LoadXDASettings(connection);

                var allSettings = datasourceSettings
                    .Concat(xdaSettings);

                return ToConnectionString(allSettings, 0);
            }
        }

        protected IEnumerable<string[]> LoadXDASettings(AdoDataConnection connection)
        {
            TableOperations<Setting> settingTable = new TableOperations<Setting>(connection);
            List<Setting> settingList = settingTable.QueryRecords().ToList();

            foreach (IGrouping<string, Setting> grouping in settingList.GroupBy(setting => setting.Name))
            {
                if (grouping.Count() > 1)
                    Log.Warn($"Duplicate record for setting {grouping.Key} detected.");
            }

            return settingList.Select(setting => ToArray(setting.Name, setting.Value));
        }
        protected virtual IEnumerable<string[]> LoadDataSourceSettings(AdoDataConnection connection)
        {
            return Enumerable.Empty<string[]>();
        }

        protected string[] ToArray(string key, string value) =>
            key.Split('.').Concat(new[] { value }).ToArray();

        #endregion

        #region [ Static ]

        // Static Fields
        protected static readonly ILog Log = LogManager.GetLogger(typeof(ConfigurationLoader));

        // Static Properties
        private static ConnectionStringParser<SettingAttribute, CategoryAttribute> ConnectionStringParser { get; } =
            new ConnectionStringParser<SettingAttribute, CategoryAttribute>();

        #endregion
    }
    public class ConfigurationLoader<T> : ConfigurationLoader where T : DataSourceSetting, new()
    {
        #region [ Constructors ]

        public ConfigurationLoader(int dataSourceEmailTypeID, Func<AdoDataConnection> connectionFactory)
            : base(connectionFactory)
        {
            DataSourceEmailTypeID = dataSourceEmailTypeID;
        }

        #endregion

        #region [ Properties ]

        private int DataSourceEmailTypeID { get; }

        #endregion

        #region [ Methods ]

        protected override IEnumerable<string[]> LoadDataSourceSettings(AdoDataConnection connection)
        {
            TableOperations<T> settingTable = new TableOperations<T>(connection);
            string query;
            if (typeof(T) == typeof(ScheduledEmailDataSourceSetting))
                query = "ScheduledEmailDataSourceID = {0}";
            else
                query = "TriggeredEmailDataSourceEmailTypeID = {0}";

            List<DataSourceSetting> settingList = settingTable.QueryRecordsWhere(query, DataSourceEmailTypeID).ToList<DataSourceSetting>();

            foreach (IGrouping<string, DataSourceSetting> grouping in settingList.GroupBy(setting => setting.Name))
            {
                if (grouping.Count() > 1)
                    Log.Warn($"Duplicate record for setting {grouping.Key} detected.");
            }

            return settingList.Select(setting => ToArray(setting.Name, setting.Value));
        }
        #endregion
    }
}
