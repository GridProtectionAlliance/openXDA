//******************************************************************************************************
//  AnalysisTaskPublisher.cs - Gbtc
//
//  Copyright © 2020, Grid Protection Alliance.  All Rights Reserved.
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
//  12/15/2020 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using GSF.Data;
using GSF.Data.Model;
using openXDA.Model;

namespace openXDA.Nodes.Types.Analysis
{
    internal class AnalysisTaskPublisher
    {
        public AnalysisTaskPublisher(Func<AdoDataConnection> connectionFactory) =>
            ConnectionFactory = connectionFactory;

        private Func<AdoDataConnection> ConnectionFactory { get; }

        public void Publish(AnalysisTask task)
        {
            using (AdoDataConnection connection = ConnectionFactory())
            {
                const string QueueQuery =
                    "INSERT INTO AnalysisTask(FileGroupID, MeterID, Priority) " +
                    "VALUES({0}, {1}, {2})";

                Upload(connection, task.FileGroup);
                connection.ExecuteNonQuery(QueueQuery, task.FileGroup.ID, task.Meter.ID, task.Priority);
            }
        }

        private void Upload(AdoDataConnection connection, FileGroup fileGroup)
        {
            TableOperations<FileGroup> fileGroupTable = new TableOperations<FileGroup>(connection);
            TableOperations<DataFile> dataFileTable = new TableOperations<DataFile>(connection);
            TableOperations<FileBlob> fileBlobTable = new TableOperations<FileBlob>(connection);

            foreach (DataFile dataFile in fileGroup.DataFiles)
            {
                DataFile dbDataFile = dataFileTable.QueryDataFile(dataFile.FilePath);

                if (dbDataFile == null)
                    continue;

                fileGroup.ID = dbDataFile.FileGroupID;
                dataFile.ID = dbDataFile.ID;

                FileBlob dbFileBlob = fileBlobTable.QueryRecordWhere("DataFileID = {0}", dbDataFile.ID);

                if (dbFileBlob != null)
                    dataFile.FileBlob.ID = dbFileBlob.ID;
            }

            if (fileGroup.ID != 0)
            {
                FileGroup dbFileGroup = fileGroupTable.QueryRecordWhere("ID = {0}", fileGroup.ID);
                fileGroup.ProcessingStartTime = dbFileGroup.ProcessingStartTime;
                fileGroup.ProcessingEndTime = dbFileGroup.ProcessingEndTime;
                fileGroup.DataStartTime = dbFileGroup.DataStartTime;
                fileGroup.DataEndTime = dbFileGroup.DataEndTime;
                fileGroup.Error = dbFileGroup.Error;
            }

            fileGroupTable.AddNewOrUpdateRecord(fileGroup);

            if (fileGroup.ID == 0)
                fileGroup.ID = connection.ExecuteScalar<int>("SELECT @@IDENTITY");

            foreach (DataFile dataFile in fileGroup.DataFiles)
            {
                dataFile.FileGroupID = fileGroup.ID;
                dataFileTable.AddNewOrUpdateRecord(dataFile);

                if (dataFile.ID == 0)
                    dataFile.ID = connection.ExecuteScalar<int>("SELECT @@IDENTITY");

                dataFile.FileBlob.DataFileID = dataFile.ID;
                fileBlobTable.AddNewOrUpdateRecord(dataFile.FileBlob);

                if (dataFile.FileBlob.ID == 0)
                    dataFile.FileBlob.ID = connection.ExecuteScalar<int>("SELECT @@IDENTITY");
            }
        }
    }
}
