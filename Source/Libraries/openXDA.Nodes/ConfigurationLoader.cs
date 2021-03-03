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
//  01/16/2021 - Stephen C. Wills
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

namespace openXDA.Nodes
{
    public class ConfigurationLoader
    {
        #region [ Constructors ]

        public ConfigurationLoader(Func<AdoDataConnection> connectionFactory)
        {
            ConnectionFactory = connectionFactory;
            LazyConfigureAction = new Lazy<Action<object>>(CreateConfigureAction);
        }

        public ConfigurationLoader(int hostID, Func<AdoDataConnection> connectionFactory)
            : this(connectionFactory)
        {
            HostID = hostID;
        }

        public ConfigurationLoader(int hostID, int nodeID, Func<AdoDataConnection> connectionFactory)
            : this(hostID, connectionFactory)
        {
            NodeID = nodeID;
        }

        #endregion

        #region [ Properties ]

        private int? HostID { get; }
        private int? NodeID { get; }
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
                var nodeSettings = LoadNodeSettings(connection);
                var hostSettings = LoadHostSettings(connection);
                var xdaSettings = LoadXDASettings(connection);

                var allSettings = nodeSettings
                    .Concat(hostSettings)
                    .Concat(xdaSettings);

                return ToConnectionString(allSettings, 0);
            }
        }

        private IEnumerable<string[]> LoadXDASettings(AdoDataConnection connection)
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

        private IEnumerable<string[]> LoadHostSettings(AdoDataConnection connection)
        {
            if (HostID is null)
                return Enumerable.Empty<string[]>();

            TableOperations<HostSetting> settingTable = new TableOperations<HostSetting>(connection);
            List<HostSetting> settingList = settingTable.QueryRecordsWhere("ID = {0}", HostID).ToList();

            foreach (IGrouping<string, Setting> grouping in settingList.GroupBy(setting => setting.Name))
            {
                if (grouping.Count() > 1)
                    Log.Warn($"Duplicate record for setting {grouping.Key} detected.");
            }

            return settingList.Select(setting => ToArray(setting.Name, setting.Value));
        }

        private IEnumerable<string[]> LoadNodeSettings(AdoDataConnection connection)
        {
            if (NodeID is null)
                return Enumerable.Empty<string[]>();

            TableOperations<NodeSetting> settingTable = new TableOperations<NodeSetting>(connection);
            List<NodeSetting> settingList = settingTable.QueryRecordsWhere("ID = {0}", NodeID).ToList();

            foreach (IGrouping<string, Setting> grouping in settingList.GroupBy(setting => setting.Name))
            {
                if (grouping.Count() > 1)
                    Log.Warn($"Duplicate record for setting {grouping.Key} detected.");
            }

            return settingList.Select(setting => ToArray(setting.Name, setting.Value));
        }

        private string[] ToArray(string key, string value) =>
            key.Split('.').Append(value).ToArray();

        #endregion

        #region [ Static ]

        // Static Fields
        private static readonly ILog Log = LogManager.GetLogger(typeof(ConfigurationLoader));

        // Static Properties
        private static ConnectionStringParser<SettingAttribute, CategoryAttribute> ConnectionStringParser { get; } =
            new ConnectionStringParser<SettingAttribute, CategoryAttribute>();

        #endregion
    }
}
