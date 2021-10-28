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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using FaultData.DataSets;
using GSF.Configuration;
using GSF.Data;
using GSF.Data.Model;
using log4net;
using openXDA.Model;

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
        [SettingName(DataPusherSettings.CategoryName)]
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
            //meterDataSet.FileGroup.DataFiles[0].FileBlob.
            if (DataPusherSettings.Enabled)
            {
                Log.Info("Executing operation to push data to remote instances...");
                PushDataToRemoteInstances(meterDataSet);
            }
            else
                Log.Info("Data Push Operation skipped because it is not enabled...");

        }

        [Serializable]
        public class FileGroupPost
        {
            public string MeterKey { get; set; }
            public FileGroup FileGroup { get; set; }
            public List<DataFile> DataFiles { get; set; }
            public List<FileBlob> FileBlobs { get; set; }
        }

        private void PushDataToRemoteInstances(MeterDataSet meterDataSet)
        {
            using (AdoDataConnection connection =  meterDataSet.CreateDbConnection())
            {

                // If only valid fault setting is set to true, count faults in file group and return if 0
                if (DataPusherSettings?.OnlyValidFaults ?? false)
                {
                    TableOperations<FaultSummary> faultSummaryTable = new TableOperations<FaultSummary>(connection);
                    int faultSummaryCount = faultSummaryTable.QueryRecordCountWhere("EventID IN (SELECT ID FROM Event WHERE FileGroupID = {0} AND FileVersion = {1}) AND IsValid = 1 AND IsSuppressed = 0", meterDataSet.FileGroup.ID, meterDataSet.FileGroup.ProcessingVersion);
                    if (faultSummaryCount == 0) return;
                }


                TableOperations<RemoteXDAInstance> instanceTable = new TableOperations<RemoteXDAInstance>(connection);
                TableOperations<MetersToDataPush> meterTable = new TableOperations<MetersToDataPush>(connection);
                IEnumerable<RemoteXDAInstance> instances = instanceTable.QueryRecordsWhere("Frequency ='*' AND ID IN (SELECT RemoteXDAInstanceID FROM RemoteXDAInstanceMeter WHERE MetersToDataPushID IN (SELECT ID FROM MetersToDataPush WHERE LocalXDAMeterID = {0}) )", meterDataSet.Meter.ID);
                FileGroupPost post = new FileGroupPost();

                post.FileGroup = meterDataSet.FileGroup;
                post.DataFiles = meterDataSet.FileGroup.DataFiles;
                post.FileBlobs = meterDataSet.FileGroup.DataFiles.Select(df => df.FileBlob).ToList();

                foreach (RemoteXDAInstance instance in instances)
                {
                    // If file group has already been pushed to a remote instance, return
                    TableOperations<FileGroupLocalToRemote> fileGroupLocalToRemoteTable = new TableOperations<FileGroupLocalToRemote>(connection);
                    FileGroupLocalToRemote fileGroup = fileGroupLocalToRemoteTable.QueryRecordWhere("LocalFileGroupID = {0} AND RemoteXDAInstanceID = {1}", meterDataSet.FileGroup.ID, instance.ID);
                    if (fileGroup != null)
                    {
                        Log.Info($"File has already been pushed previously for {instance.Name}.");
                        continue;
                    }

                    MetersToDataPush metersToDataPush = new TableOperations<MetersToDataPush>(connection).QueryRecordWhere("LocalXDAMeterID = {0} AND ID IN (SELECT MetersToDataPushID From RemoteXDAInstanceMeter WHERE RemoteXDAInstanceID = {1})", meterDataSet.Meter.ID, instance.ID);
                    post.MeterKey = metersToDataPush.RemoteXDAAssetKey;
                    Log.Info($"Sending data to intance: {instance.Name} for FileGroup: {meterDataSet.FileGroup.ID}...");


                }
                Log.Info("Sync complete...");

            }

        }

        #endregion

        #region [ Static ]

        // Static Fields
        private static readonly ILog Log = LogManager.GetLogger(typeof(DataPusherOperation));

        #endregion        
    }
}
