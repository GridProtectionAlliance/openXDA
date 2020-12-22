//******************************************************************************************************
//  NodeBase.cs - Gbtc
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
//  02/05/2021 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.ComponentModel;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using GSF.Configuration;
using GSF.Data;
using openXDA.Configuration;
using openXDA.Model;

namespace openXDA.Nodes
{
    public abstract class NodeBase : INode
    {
        #region [ Members ]

        // Nested Types
        private class Settings
        {
            public Settings(Action<object> configure) =>
                configure(this);

            [Category]
            [SettingName(SystemSection.CategoryName)]
            public SystemSection SystemSettings { get; } = new SystemSection();
        }

        #endregion

        #region [ Constructors ]

        public NodeBase(Host host, Node definition, NodeType type)
        {
            Host = host;
            Definition = definition;
            Type = type;

            Action<object> configurator = GetConfigurator();
            UpdateDbTimeout(configurator);
        }

        #endregion

        #region [ Properties ]

        public Host Host { get; }
        public Node Definition { get; }
        public NodeType Type { get; }

        private int DbTimeout { get; set; } =
            DataExtensions.DefaultTimeoutDuration;

        #endregion

        #region [ Methods ]

        public void Reconfigure()
        {
            Action<object> configurator = GetConfigurator();
            UpdateDbTimeout(configurator);
            OnReconfigure(configurator);
        }

        public virtual IHttpController CreateWebController() => null;

        protected Action<object> GetConfigurator()
        {
            int hostID = Host.ID;
            int nodeID = Definition.ID;
            ConfigurationLoader configurationLoader = new ConfigurationLoader(hostID, nodeID, CreateDbConnection);
            return configurationLoader.Configure;
        }

        protected AdoDataConnection CreateDbConnection()
        {
            AdoDataConnection connection = Host.CreateDbConnection();
            connection.DefaultTimeout = DbTimeout;
            return connection;
        }

        protected virtual void OnReconfigure(Action<object> configurator)
        {
        }

        private void UpdateDbTimeout(Action<object> configurator)
        {
            Settings settings = new Settings(configurator);
            DbTimeout = settings.SystemSettings.DbTimeout;
        }

        #endregion

        #region [ Static ]

        // Static Properties
        protected static HttpClient HttpClient { get; }

        #endregion
    }
}
