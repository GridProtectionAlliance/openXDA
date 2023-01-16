//******************************************************************************************************
//  DataBaseMaintenanceNode.cs - Gbtc
//
//  Copyright © 2023, Grid Protection Alliance.  All Rights Reserved.
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
//  01/16/2023 - C. Lackner
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using GSF.Configuration;
using GSF.Data;
using GSF.Data.Model;
using GSF.Threading;
using log4net;
using openXDA.Configuration;
using openXDA.DataPusher;
using openXDA.Model;

namespace openXDA.Nodes.Types.DataBaseMaintenance
{
    public class DataBaseMaintenanceNode : NodeBase
    {
        #region [ Members ]

        // Nested Types
        private class DataBaseMaintenanceController : ApiController
        {
            private DataBaseMaintenanceNode Node { get; }

            public DataBaseMaintenanceController(DataBaseMaintenanceNode node) =>
                Node = node;
        }

        #endregion

        #region [ Constructors ]

        public DataBaseMaintenanceNode(Host host, Node definition, NodeType type)
            : base(host, definition, type)
        {
            ScheduleAllInstances();
        }

        #endregion

        #region [ Properties ]


        #endregion

        #region [ Methods ]

        public override IHttpController CreateWebController() =>
            new DataBaseMaintenanceController(this);

        protected override void OnReconfigure(Action<object> configurator)
        {
            ScheduleAllInstances();
        }

        private void ScheduleAllInstances()
        {            
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                IEnumerable<DBCleanup> allInstances = new TableOperations<DBCleanup>(connection).QueryRecords();
                foreach (DBCleanup instance in allInstances)
                {
                    string name = $"{nameof(RunInstance)}_ID:{instance.ID}";
                    Host.RegisterScheduledProcess(this, RunInstance(instance), name, instance.Schedule);
                }
            }
        }

        private Action RunInstance(DBCleanup instance)
        {
           Action<object> configurator = GetConfigurator();
            return () =>
            {
                using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
                {
                    connection.ExecuteNonQuery(instance.SQLCommand);
                    Log.Info($"Ran cleanup Task: {instance.Name}");
                }
            };
        }

        #endregion

        #region [ Static ]

        // Static Fields
        private static readonly ILog Log = LogManager.GetLogger(typeof(DataBaseMaintenanceNode));

        #endregion
    }
}
