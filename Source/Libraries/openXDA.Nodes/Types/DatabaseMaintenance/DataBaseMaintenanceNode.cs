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
using System.Web.Http;
using System.Web.Http.Controllers;
using GSF.Data;
using GSF.Data.Model;
using log4net;
using openXDA.Model;

namespace openXDA.Nodes.Types.DatabaseMaintenance
{
    public class DatabaseMaintenanceNode : NodeBase
    {
        public DatabaseMaintenanceNode(Host host, Node definition, NodeType type)
            : base(host, definition, type)
        {
            ScheduleAllInstances();
        }

        private class DatabaseMaintenanceWebController : ApiController
        {
            private DatabaseMaintenanceNode Node { get; }

            public DatabaseMaintenanceWebController(DatabaseMaintenanceNode node) =>
                Node = node;

            [HttpGet]
            public void Reconfigure() =>
                Node.Reconfigure();
        }

        #region [ Methods ]

        public override IHttpController CreateWebController() =>
            new DatabaseMaintenanceWebController(this);

        protected override void OnReconfigure(Action<object> configurator) =>
            ScheduleAllInstances();

        private void ScheduleAllInstances()
        {            
            using (AdoDataConnection connection = CreateDbConnection())
            {
                IEnumerable<DBCleanup> allInstances = new TableOperations<DBCleanup>(connection).QueryRecords();

                foreach (DBCleanup instance in allInstances)
                {
                    string name = $"DBCleanup_ID:{instance.ID}";
                    // We don't need to dispose this, its handled by host
                    Host.RegisterScheduledProcess(this, CreateScheduleAction(instance), name, instance.Schedule);
                }
            }
        }

        private Action CreateScheduleAction(DBCleanup instance) => () =>
        {
            using (AdoDataConnection connection = CreateDbConnection())
            {
                connection.ExecuteNonQuery(instance.SQLCommand);
                Log.Info($"Ran cleanup Task: {instance.Name}");
            }
        };

        #endregion

        #region [ Static ]

        // Static Fields
        private static readonly ILog Log = LogManager.GetLogger(typeof(DatabaseMaintenanceNode));

        #endregion
    }
}
