//******************************************************************************************************
//  AnalysisTaskProcessor.cs - Gbtc
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
//  01/10/2021 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using GSF.Data;
using GSF.Data.Model;
using openXDA.Model;

namespace openXDA.Nodes.Types.Analysis
{
    internal class AnalysisTaskProcessor
    {
        #region [ Constructors ]

        public AnalysisTaskProcessor(int analysisNodeID, Func<AdoDataConnection> connectionFactory)
        {
            AnalysisNodeID = analysisNodeID;
            ConnectionFactory = connectionFactory;
        }

        #endregion

        #region [ Properties ]

        private int AnalysisNodeID { get; }
        private Func<AdoDataConnection> ConnectionFactory { get; }

        #endregion

        #region [ Methods ]

        public AnalysisTask Poll(string meterFilterQuery)
        {
            using (AdoDataConnection connection = ConnectionFactory())
            {
                string QueryFormat =
                    $"DECLARE @id INT = " +
                    $"( " +
                    $"    SELECT TOP 1 AnalysisTask.ID " +
                    $"    FROM " +
                    $"        ({meterFilterQuery}) Meter CROSS APPLY " +
                    $"        ( " +
                    $"            SELECT TOP 1 * " +
                    $"            FROM AnalysisTask " +
                    $"            WHERE MeterID = Meter.ID " +
                    $"            ORDER BY NodeID DESC, Priority DESC, ID " +
                    $"        ) AnalysisTask " +
                    $"    WHERE AnalysisTask.NodeID IS NULL " +
                    $"    ORDER BY AnalysisTask.Priority DESC, AnalysisTask.ID " +
                    $") " +
                    $"" +
                    $"UPDATE AnalysisTask " +
                    $"SET NodeID = {{0}} " +
                    $"WHERE " +
                    $"    ID = @id AND " +
                    $"    NodeID IS NULL " +
                    $"" +
                    // Check again -
                    // Extremely rare and complicated race conditions could potentially
                    // result in parallel processing of data from the same meter
                    $"UPDATE AnalysisTask " +
                    $"SET NodeID = NULL " +
                    $"WHERE " +
                    $"    ID = @id AND " +
                    $"    NodeID = {{0}} AND " +
                    $"    EXISTS " +
                    $"    ( " +
                    $"        SELECT * " +
                    $"        FROM AnalysisTask InnerTask " +
                    $"        WHERE " +
                    $"            MeterID = AnalysisTask.MeterID AND " +
                    $"            NodeID IS NOT NULL AND " +
                    $"            ID <> @id " +
                    $"    ) " +
                    $"" +
                    $"SELECT * " +
                    $"FROM AnalysisTask " +
                    $"WHERE ID = @id";

                int id;
                int fileGroupID;
                int meterID;
                int priority;

                while (true)
                {
                    using (DataTable table = connection.RetrieveData(QueryFormat, AnalysisNodeID))
                    {
                        // If there is no work to
                        // be taken, quit polling
                        if (table.Rows.Count == 0)
                            return null;

                        DataRow row = table.Rows[0];
                        int nodeID = row.ConvertField<int>("NodeID");

                        // If work was taken by another
                        // node, retry the query
                        if (nodeID != AnalysisNodeID)
                            continue;

                        id = row.ConvertField<int>("ID");
                        fileGroupID = row.ConvertField<int>("FileGroupID");
                        meterID = row.ConvertField<int>("MeterID");
                        priority = row.ConvertField<int>("Priority");
                        break;
                    }
                }

                FileGroup fileGroup = QueryFileGroup(connection, fileGroupID);
                Meter meter = QueryMeter(connection, meterID);
                meter.ConnectionFactory = ConnectionFactory;

                AnalysisTask task = new AnalysisTask(id, fileGroup, meter, priority);
                PruneDuplicateTasks(connection, task);
                return task;
            }
        }

        public void Dequeue(AnalysisTask task)
        {
            using (AdoDataConnection connection = ConnectionFactory())
            {
                const string QueryFormat =
                    "DELETE FROM AnalysisTask " +
                    "WHERE ID = {0}";

                connection.ExecuteNonQuery(QueryFormat, task.ID);
            }
        }

        private void PruneDuplicateTasks(AdoDataConnection connection, AnalysisTask task)
        {
            const string QueryFormat =
                "DELETE FROM AnalysisTask " +
                "WHERE " +
                "    ID <> {0} AND " +
                "    FileGroupID = {1} AND " +
                "    MeterID = {2}";

            int taskID = task.ID;
            int fileGroupID = task.FileGroup.ID;
            int meterID = task.Meter.ID;
            connection.ExecuteNonQuery(QueryFormat, taskID, fileGroupID, meterID);
        }

        private FileGroup QueryFileGroup(AdoDataConnection connection, int fileGroupID)
        {
            TableOperations<FileGroup> fileGroupTable = new TableOperations<FileGroup>(connection);
            FileGroup fileGroup = fileGroupTable.QueryRecordWhere("ID = {0}", fileGroupID);

            TableOperations<DataFile> dataFileTable = new TableOperations<DataFile>(connection);

            fileGroup.DataFiles = dataFileTable
                .QueryRecordsWhere("FileGroupID = {0}", fileGroupID)
                .ToList();

            TableOperations<FileBlob> fileBlobTable = new TableOperations<FileBlob>(connection);

            List<FileBlob> fileBlobs = fileBlobTable
                .QueryRecordsWhere("DataFileID IN (SELECT ID FROM DataFile WHERE FileGroupID = {0})", fileGroupID)
                .ToList();

            int SelectID(DataFile dataFile) => dataFile.ID;
            int SelectDataFileID(FileBlob fileBlob) => fileBlob.DataFileID;
            var mappings = fileGroup.DataFiles.Join(fileBlobs, SelectID, SelectDataFileID, (DataFile, FileBlob) => new { DataFile, FileBlob });

            foreach (var mapping in mappings)
                mapping.DataFile.FileBlob = mapping.FileBlob;

            return fileGroup;
        }

        private Meter QueryMeter(AdoDataConnection connection, int meterID)
        {
            TableOperations<Meter> meterTable = new TableOperations<Meter>(connection);
            return meterTable.QueryRecordWhere("ID = {0}", meterID);
        }

        #endregion
    }
}
