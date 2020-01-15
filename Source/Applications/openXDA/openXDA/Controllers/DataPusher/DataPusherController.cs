//******************************************************************************************************
//  DataPusherController.cs - Gbtc
//
//  Copyright © 2019, Grid Protection Alliance.  All Rights Reserved.
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
//  12/13/2019 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using GSF.Data;
using GSF.Data.Model;
using GSF.Security.Model;
using openXDA.DataPusher;
using openXDA.Model;

namespace openXDA.Controllers.Config
{
    [RoutePrefix("api/DataPusher")]
    public class DataPusherController : ApiController
    {
        [Route("SyncMeterConfig/{connectionId}/{instanceId:int}/{meterId:int}"), HttpGet]
        public Task SyncMeterConfigurationForInstance(string connectionId, int instanceId, int meterId, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
                {

                    try
                    {
                        // for now, create new instance of DataPusherEngine.  Later have one running in XDA ServiceHost and tie to it to ensure multiple updates arent happening simultaneously
                        DataPusherEngine engine = new DataPusherEngine();
                        RemoteXDAInstance instance = new TableOperations<RemoteXDAInstance>(connection).QueryRecordWhere("ID = {0}", instanceId);
                        MetersToDataPush meter = new TableOperations<MetersToDataPush>(connection).QueryRecordWhere("ID = {0}", meterId);
                        UserAccount userAccount = new TableOperations<UserAccount>(connection).QueryRecordWhere("ID = {0}", instance.UserAccountID);
                        engine.SyncMeterConfigurationForInstance(connectionId, instance, meter, userAccount, cancellationToken);
                    }
                    catch (Exception ex)
                    {
                        Program.Host.HandleException(ex);

                    }
                }
            }, cancellationToken);

        }

        [Route("SyncMeterFiles/{connectionId}/{instanceId:int}/{meterId:int}"), HttpGet]
        public Task SyncMeterFilesForInstance(string connectionId, int instanceId, int meterId, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
                {
                    try
                    {
                        // for now, create new instance of DataPusherEngine.  Later have one running in XDA ServiceHost and tie to it to ensure multiple updates arent happening simultaneously
                        DataPusherEngine engine = new DataPusherEngine();
                        RemoteXDAInstance instance = new TableOperations<RemoteXDAInstance>(connection).QueryRecordWhere("ID = {0}", instanceId);
                        engine.SyncMeterFilesForInstance(connectionId, instance, meterId, cancellationToken);
                    }
                    catch (Exception ex)
                    {
                        Program.Host.HandleException(ex);
                    }
                }
            }, cancellationToken);
        }

        [Route("SyncInstanceConfig/{connectionId}/{instanceId:int}"), HttpGet]
        public Task SyncInstanceConfiguration(string connectionId, int instanceId, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                // for now, create new instance of DataPusherEngine.  Later have one running in XDA ServiceHost and tie to it to ensure multiple updates arent happening simultaneously
                DataPusherEngine engine = new DataPusherEngine();
                engine.SyncInstanceConfiguration(connectionId, instanceId, cancellationToken);
            }, cancellationToken);

        }

        [Route("SyncInstanceFiles/{connectionId}/{instanceId:int}"), HttpGet]
        public Task SyncFilesForInstance(string connectionId, int instanceId, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
                {
                    // for now, create new instance of DataPusherEngine.  Later have one running in XDA ServiceHost and tie to it to ensure multiple updates arent happening simultaneously
                    DataPusherEngine engine = new DataPusherEngine();
                    RemoteXDAInstance instance = new TableOperations<RemoteXDAInstance>(connection).QueryRecordWhere("ID = {0}", instanceId);
                    engine.SyncInstanceFiles(connectionId, instance, cancellationToken);
                }
            }, cancellationToken);
        }
    }
}