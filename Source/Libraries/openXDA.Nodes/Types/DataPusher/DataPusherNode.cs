//******************************************************************************************************
//  DataPusherNode.cs - Gbtc
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
//  02/08/2021 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using GSF.Configuration;
using GSF.Data;
using GSF.Data.Model;
using GSF.IO;
using GSF.Threading;
using log4net;
using openXDA.Configuration;
using openXDA.DataPusher;
using openXDA.Model;

namespace openXDA.Nodes.Types.DataPusher
{
    public class DataPusherNode : NodeBase
    {
        #region [ Members ]

        // Nested Types
        private class Settings
        {
            public Settings(Action<object> configure) =>
                configure(this);

            [Category]
            [SettingName(DataPusherSection.CategoryName)]
            public DataPusherSection DataPusherNodeSettings { get; } = new DataPusherSection();

            [Category]
            [SettingName(FileWatcherSection.CategoryName)]
            public FileWatcherSection FileWatcherSettings { get; } = new FileWatcherSection();
        }

        private class DataPusherWebController : ApiController
        {
            private DataPusherNode Node { get; }

            public DataPusherWebController(DataPusherNode node) =>
                Node = node;
        }

        #endregion

        #region [ Constructors ]

        public DataPusherNode(Host host, Node definition, NodeType type)
            : base(host, definition, type)
        {
            Action<object> configurator = GetConfigurator();
            ScheduleAllInstances(configurator);
        }

        #endregion

        #region [ Properties ]

        private DataPusherEngine DataEngine { get; set; }

        #endregion

        #region [ Methods ]

        public override IHttpController CreateWebController() =>
            new DataPusherWebController(this);

        protected override void OnReconfigure(Action<object> configurator)
        {
            ScheduleAllInstances(configurator);
        }

        private void ScheduleAllInstances(Action<object> configurator)
        {
            Settings settings = new Settings(configurator);
            DataEngine = new DataPusherEngine(() => new AdoDataConnection(settings.DataPusherNodeSettings.ConnectionString));
            IEnumerable<RemoteXDAInstance> allInstances;
            using (AdoDataConnection connection = new AdoDataConnection(settings.DataPusherNodeSettings.ConnectionString))
            {
                allInstances = new TableOperations<RemoteXDAInstance>(connection).QueryRecordsWhere("Frequency != '*'");
            }
            foreach (RemoteXDAInstance instance in allInstances)
            {
                string name = $"{nameof(SyncInstance)}_ID:{instance.ID}";
                Host.RegisterScheduledProcess(this, SyncInstance(instance), name, instance.Frequency);
            }
        }

        private Action SyncInstance(RemoteXDAInstance instance)
        {
            Action<object> configurator = GetConfigurator();
            Settings settings = new Settings(configurator);
            string name = $"{nameof(DataPusherEngine.SyncInstanceFiles)}_ID:{instance.ID}";
            return () =>
            {
                DataEngine.SyncInstanceFiles(name, instance, new CancellationToken());
            };
        }

        #endregion

        #region [ Static ]

        // Static Fields
        private static readonly ILog Log = LogManager.GetLogger(typeof(DataPusherNode));

        #endregion
    }
}
