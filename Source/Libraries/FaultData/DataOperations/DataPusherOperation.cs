//******************************************************************************************************
//  DataPusherOperation.cs - Gbtc
//
//  Copyright © 2018, Grid Protection Alliance.  All Rights Reserved.
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
//  04/02/2018 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************

using FaultData.DataSets;
using GSF.Configuration;
using GSF.Data;
using GSF.Data.Model;
using log4net;
using openXDA.DataPusher;
using openXDA.Model;
using System.Collections.Generic;
using System.ComponentModel;

namespace FaultData.DataOperations
{
    public class DataPusherOperation : DataOperationBase<MeterDataSet>
    {
        #region [ Members ]
        private DataPusherSettings m_dataPusherSettings;
        #endregion

        #region [ Constructors ]

        public DataPusherOperation()
        {
            m_dataPusherSettings = new DataPusherSettings();
        }

        #endregion

        #region [ Properties ]
        [Category]
        [SettingName("DataPusher")]
        public DataPusherSettings DataPusherSettings
        {
            get
            {
                return m_dataPusherSettings;
            }
        }
        #endregion

        #region [ Methods ]
        public override void Execute(MeterDataSet meterDataSet)
        {
            if (DataPusherSettings.Enabled)
            {
                Log.Info("Executing operation to push data to remote instances...");

                using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
                {
                    if (DataPusherSettings.OnlyValidFaults)
                    {
                        TableOperations<FaultSummary> faultSummaryTable = new TableOperations<FaultSummary>(connection);
                        int faultSummaryCount = faultSummaryTable.QueryRecordCountWhere("EventID IN (SELECT ID FROM Event WHERE FileGroupID = {0}) AND IsValid = 1 AND IsSuppressed = 0", meterDataSet.FileGroup.ID);
                        if (faultSummaryCount > 0)
                            PushDataToRemoteInstances(connection, meterDataSet.FileGroup.ID, meterDataSet.Meter.ID);
                    }
                    else
                        PushDataToRemoteInstances(connection, meterDataSet.FileGroup.ID, meterDataSet.Meter.ID);
                }
            }
            else
                Log.Info("Data Push Operation skipped because it is not enabled...");

        }


        private void PushDataToRemoteInstances(AdoDataConnection connection,int fileGroupId, int meterId)
        {
            // If file group has already been pushed to a remote instance, return
            TableOperations<FileGroupLocalToRemote> fileGroupLocalToRemoteTable = new TableOperations<FileGroupLocalToRemote>(connection);
            FileGroupLocalToRemote fileGroup = fileGroupLocalToRemoteTable.QueryRecordWhere("LocalFileGroupID = {0}", fileGroupId);
            if (fileGroup != null) return;

            TableOperations<RemoteXDAInstance> instanceTable = new TableOperations<RemoteXDAInstance>(connection);
            TableOperations<MetersToDataPush> meterTable = new TableOperations<MetersToDataPush>(connection);
            IEnumerable<RemoteXDAInstance> instances = instanceTable.QueryRecordsWhere("Frequency ='*'");
            DataPusherEngine engine = new DataPusherEngine();

            foreach (RemoteXDAInstance instance in instances)
            {
                IEnumerable<MetersToDataPush> meters = meterTable.QueryRecordsWhere("LocalXDAMeterID = {0} AND ID IN (SELECT MetersToDataPushID From RemoteXDAInstanceMeter WHERE RemoteXDAInstanceID = {1})", meterId, instance.ID);
                foreach (MetersToDataPush meter in meters)
                {
                    Log.Info($"Sending data to intance: {instance.Name} for FileGroup: {fileGroupId}...");
                    engine.SyncMeterFileForInstance(instance, meter, fileGroupId);
                }
            }
        }

        #endregion

        #region [ Static ]

        // Static Fields
        private static readonly ILog Log = LogManager.GetLogger(typeof(TrendingDataSummaryOperation));

        #endregion        

    }
}
