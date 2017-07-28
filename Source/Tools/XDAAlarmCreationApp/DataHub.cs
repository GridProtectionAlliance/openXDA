//******************************************************************************************************
//  DataHub.cs - Gbtc
//
//  Copyright © 2017, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the MIT License (MIT), the "License"; you may
//  not use this file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://opensource.org/licenses/MIT
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  05/23/2017 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************

using XDAAlarmCreationApp.Model;
using GSF;
using GSF.Data;
using GSF.Data.Model;
using GSF.IO;
using GSF.Web.Hubs;
using GSF.Web.Model.HubOperations;
using GSF.Web.Security;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Web.Script;
using CsvHelper;
using Microsoft.AspNet.SignalR;
using GSF.Web.Model;

namespace XDAAlarmCreationApp
{
    public class DataHub : RecordOperationsHub<DataHub>, IDirectoryBrowserOperations
    {
        #region [ Constructors ]

        public DataHub() : base(Program.LogStatusMessage, Program.LogExceptionMessage)
        {
            Action<string, UpdateType> logStatusMessage = (message, updateType) => LogStatusMessage(message, updateType);
            Action<Exception> logException = ex => LogException(ex);

        }

        #endregion

        #region [ Static ]

        // Static Properties

        /// <summary>
        /// Gets the hub connection ID for the current thread.
        /// </summary>
        public static string CurrentConnectionID => s_connectionID.Value;

        // Static Fields
        private static readonly ThreadLocal<string> s_connectionID;
        private static volatile int s_connectCount;
        private static bool s_massImportWaitFlag = false;
        private static IEnumerable<ConflictedGroup> s_returnConflictedGroup;


        // Static Constructor
        static DataHub()
        {
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string ecaClientDataPath = Path.Combine(appData, "Grid Protection Alliance", "EPRI DRIVE Client");
            s_connectionID = new ThreadLocal<string>();
        }

        public static void ProgressUpdate(string name, int update)
        {
            GlobalHost.ConnectionManager.GetHubContext<DataHub>().Clients.All.updateProgressBar(name, update);
        }


        #endregion

        #region [ Setting Table Operations ]

        [RecordOperation(typeof(Setting), RecordOperation.QueryRecordCount)]
        public int QuerySettingCount(string filterText)
        {
            return DataContext.Table<Setting>().QueryRecordCount(filterText);
        }

        [RecordOperation(typeof(Setting), RecordOperation.QueryRecords)]
        public IEnumerable<Setting> QuerySetting(string sortField, bool ascending, int page, int pageSize, string filterText)
        {
            return DataContext.Table<Setting>().QueryRecords(sortField, ascending, page, pageSize, filterText);
        }

        [AuthorizeHubRole("Administrator, Editor")]
        [RecordOperation(typeof(Setting), RecordOperation.DeleteRecord)]
        public void DeleteSetting(int id)
        {
            DataContext.Table<Setting>().DeleteRecord(id);
        }

        [RecordOperation(typeof(Setting), RecordOperation.CreateNewRecord)]
        public Setting NewSetting()
        {
            return DataContext.Table<Setting>().NewRecord();
        }

        [AuthorizeHubRole("Administrator, Editor")]
        [RecordOperation(typeof(Setting), RecordOperation.AddNewRecord)]
        public void AddNewSetting(Setting record)
        {
            DataContext.Table<Setting>().AddNewRecord(record);
        }

        [AuthorizeHubRole("Administrator, Editor")]
        [RecordOperation(typeof(Setting), RecordOperation.UpdateRecord)]
        public void UpdateSetting(Setting record)
        {
            DataContext.Table<Setting>().UpdateRecord(record);
        }

        [AuthorizeHubRole("Administrator, Editor")]
        public void AddNewOrUpdateSetting(Setting record)
        {
            DataContext.Table<Setting>().AddNewOrUpdateRecord(record);
        }

        #endregion

        #region [ Security Table Operations ]

        [RecordOperation(typeof(Security), RecordOperation.QueryRecordCount)]
        public int QuerySecurityCount(string filterText)
        {
            IEnumerable<Security> table = new List<Security>
            {
                new Security() {
                ID = 1,
                Name = "Users"
                },
                new Security() {
                ID = 2,
                Name = "Groups"
                }
            };

            return table.Count();
        }

        [RecordOperation(typeof(Security), RecordOperation.QueryRecords)]
        public IEnumerable<Security> QuerySecurity(string sortField, bool ascending, int page, int pageSize, string filterText)
        {
            return new List<Security>
            {
                new Security() {
                ID = 1,
                Name = "Users"
                },
                new Security() {
                ID = 2,
                Name = "Groups"
                }
            };
        }

        [AuthorizeHubRole("Administrator, Editor")]
        [RecordOperation(typeof(Security), RecordOperation.DeleteRecord)]
        public void DeleteSecurity(int id)
        {
        }

        [RecordOperation(typeof(Security), RecordOperation.CreateNewRecord)]
        public Security NewSecurity()
        {
            return new Security();
        }

        [AuthorizeHubRole("Administrator, Editor")]
        [RecordOperation(typeof(Security), RecordOperation.AddNewRecord)]
        public void AddNewSecurity(Security record)
        {
        }

        [AuthorizeHubRole("Administrator, Editor")]
        [RecordOperation(typeof(Security), RecordOperation.UpdateRecord)]
        public void UpdateSecurity(Security record)
        {
        }

        [AuthorizeHubRole("Administrator, Editor")]
        public void AddNewOrUpdateSecurity(Security record)
        {
        }

        #endregion

        #region [ ColorGradient Table Operations ]

        [RecordOperation(typeof(ColorGradients), RecordOperation.QueryRecordCount)]
        public int QueryColorGradientsCount(string filterText)
        {
            return DataContext.Table<ColorGradients>().QueryRecordCount(filterText);
        }

        [RecordOperation(typeof(ColorGradients), RecordOperation.QueryRecords)]
        public IEnumerable<ColorGradients> QueryColorGradients(string sortField, bool ascending, int page, int pageSize, string filterText)
        {
            return DataContext.Table<ColorGradients>().QueryRecords(sortField, ascending, page, pageSize, filterText);
        }

        [AuthorizeHubRole("Administrator, Editor")]
        [RecordOperation(typeof(ColorGradients), RecordOperation.DeleteRecord)]
        public void DeleteColorGradients(int id)
        {
            DataContext.Table<ColorGradients>().DeleteRecord(id);
        }

        [RecordOperation(typeof(ColorGradients), RecordOperation.CreateNewRecord)]
        public ColorGradients NewColorGradients()
        {
            return DataContext.Table<ColorGradients>().NewRecord();
        }

        [AuthorizeHubRole("Administrator, Editor")]
        [RecordOperation(typeof(ColorGradients), RecordOperation.AddNewRecord)]
        public void AddNewColorGradients(ColorGradients record)
        {
            DataContext.Table<ColorGradients>().AddNewRecord(record);
        }

        [AuthorizeHubRole("Administrator, Editor")]
        [RecordOperation(typeof(ColorGradients), RecordOperation.UpdateRecord)]
        public void UpdateColorGradients(ColorGradients record)
        {
            DataContext.Table<ColorGradients>().UpdateRecord(record);
        }

        [AuthorizeHubRole("Administrator, Editor")]
        public void AddNewOrUpdateColorGradients(ColorGradients record)
        {
            DataContext.Table<ColorGradients>().AddNewOrUpdateRecord(record);
        }

        #endregion

        #region [ ResultsToDisplay Table Operations ]

        [RecordOperation(typeof(ResultsToDisplay), RecordOperation.QueryRecordCount)]
        public int QueryResultsToDisplayCount(string filterText)
        {
            return DataContext.Table<ResultsToDisplay>().QueryRecordCount(filterText);
        }

        [RecordOperation(typeof(ResultsToDisplay), RecordOperation.QueryRecords)]
        public IEnumerable<ResultsToDisplay> QueryResultsToDisplay(string sortField, bool ascending, int page, int pageSize, string filterText)
        {
            return DataContext.Table<ResultsToDisplay>().QueryRecords(sortField, ascending, page, pageSize, filterText);
        }

        [AuthorizeHubRole("Administrator, Editor")]
        [RecordOperation(typeof(ResultsToDisplay), RecordOperation.DeleteRecord)]
        public void DeleteResultsToDisplay(int id)
        {
            DataContext.Table<ResultsToDisplay>().DeleteRecord(id);
        }

        [RecordOperation(typeof(ResultsToDisplay), RecordOperation.CreateNewRecord)]
        public ResultsToDisplay NewResultsToDisplay()
        {
            return DataContext.Table<ResultsToDisplay>().NewRecord();
        }

        [AuthorizeHubRole("Administrator, Editor")]
        [RecordOperation(typeof(ResultsToDisplay), RecordOperation.AddNewRecord)]
        public void AddNewResultsToDisplay(ResultsToDisplay record)
        {
            DataContext.Table<ResultsToDisplay>().AddNewRecord(record);
        }

        [AuthorizeHubRole("Administrator, Editor")]
        [RecordOperation(typeof(ResultsToDisplay), RecordOperation.UpdateRecord)]
        public void UpdateResultsToDisplay(ResultsToDisplay record)
        {
            DataContext.Table<ResultsToDisplay>().UpdateRecord(record);
        }

        [AuthorizeHubRole("Administrator, Editor")]
        public void AddNewOrUpdateResultsToDisplay(ResultsToDisplay record)
        {
            DataContext.Table<ResultsToDisplay>().AddNewOrUpdateRecord(record);
        }

        #endregion

        #region [ LineCharacteristicsToDisplay Table Operations ]

        [RecordOperation(typeof(LineCharacteristicsToDisplay), RecordOperation.QueryRecordCount)]
        public int QueryLineCharacteristicsToDisplayCount(string filterText)
        {
            return DataContext.Table<LineCharacteristicsToDisplay>().QueryRecordCount(filterText);
        }

        [RecordOperation(typeof(LineCharacteristicsToDisplay), RecordOperation.QueryRecords)]
        public IEnumerable<LineCharacteristicsToDisplay> QueryLineCharacteristicsToDisplay(string sortField, bool ascending, int page, int pageSize, string filterText)
        {
            return DataContext.Table<LineCharacteristicsToDisplay>().QueryRecords(sortField, ascending, page, pageSize, filterText);
        }

        [AuthorizeHubRole("Administrator, Editor")]
        [RecordOperation(typeof(LineCharacteristicsToDisplay), RecordOperation.DeleteRecord)]
        public void DeleteLineCharacteristicsToDisplay(int id)
        {
            DataContext.Table<LineCharacteristicsToDisplay>().DeleteRecord(id);
        }

        [RecordOperation(typeof(LineCharacteristicsToDisplay), RecordOperation.CreateNewRecord)]
        public LineCharacteristicsToDisplay NewLineCharacteristicsToDisplay()
        {
            return DataContext.Table<LineCharacteristicsToDisplay>().NewRecord();
        }

        [AuthorizeHubRole("Administrator, Editor")]
        [RecordOperation(typeof(LineCharacteristicsToDisplay), RecordOperation.AddNewRecord)]
        public void AddNewLineCharacteristicsToDisplay(LineCharacteristicsToDisplay record)
        {
            DataContext.Table<LineCharacteristicsToDisplay>().AddNewRecord(record);
        }

        [AuthorizeHubRole("Administrator, Editor")]
        [RecordOperation(typeof(LineCharacteristicsToDisplay), RecordOperation.UpdateRecord)]
        public void UpdateLineCharacteristicsToDisplay(LineCharacteristicsToDisplay record)
        {
            DataContext.Table<LineCharacteristicsToDisplay>().UpdateRecord(record);
        }

        [AuthorizeHubRole("Administrator, Editor")]
        public void AddNewOrUpdateLineCharacteristicsToDisplay(LineCharacteristicsToDisplay record)
        {
            DataContext.Table<LineCharacteristicsToDisplay>().AddNewOrUpdateRecord(record);
        }

        #endregion

        #region [ Result Table Operations ]

        [RecordOperation(typeof(Result), RecordOperation.QueryRecordCount)]
        public int QueryResultCount(string filterText)
        {
            return DataContext.Table<Result>().QueryRecordCount(filterText);
        }

        [RecordOperation(typeof(Result), RecordOperation.QueryRecords)]
        public IEnumerable<Result> QueryResult(string sortField, bool ascending, int page, int pageSize, string filterText)
        {
            return DataContext.Table<Result>().QueryRecords(sortField, ascending, page, pageSize, filterText);
        }

        [AuthorizeHubRole("Administrator, Editor")]
        [RecordOperation(typeof(Result), RecordOperation.DeleteRecord)]
        public void DeleteResult(int id)
        {
            CascadeDelete("Result", $"ID = {id}");
        }

        [RecordOperation(typeof(Result), RecordOperation.CreateNewRecord)]
        public Result NewResult()
        {
            var temp = DataContext.Table<Result>().NewRecord();

            return DataContext.Table<Result>().NewRecord();
        }

        [AuthorizeHubRole("Administrator, Editor")]
        [RecordOperation(typeof(Result), RecordOperation.AddNewRecord)]
        public void AddNewResult(Result record)
        {
            DataTable RTable = new DataTable();
            string RTableName = "";
            DataTable ZTable = new DataTable();
            string ZTableName = "";
            DataTable LDGTable = new DataTable();
            string LDGTableName = "";
            DataTable LineTable = new DataTable();
            string LineTableName = "";
            int resultId;

            DirectoryInfo dg = new DirectoryInfo(record.Directory);
            IEnumerable<FileInfo> fileInfos = dg.GetFiles($"{record.FilePrefix}_*");

            if (fileInfos.Count() < 4) throw new Exception("You are missing 1 or more files.");
            using (DataContext dataContext = new DataContext("systemSettings"))
            {
                foreach (FileInfo fi in fileInfos)
                {
                    if (fi.Name.Equals(record.FilePrefix + "_HC-ldg.csv"))
                    {
                        LDGTableName = "LDGTable" + record.FilePrefix;
                        LDGTable = CSVToDataTable(fi.FullName);
                        dataContext.Table<Model.File>().AddNewRecord(new Model.File()
                        {
                            Name = fi.Name,
                            FileTypeID = dataContext.Connection.ExecuteScalar<int>("SELECT ID FROM FileType WHERE Name = 'NodeLDGFile'"),
                            Data = JsonConvert.SerializeObject(LDGTable)
                        });
                        record.NodeLDGFileID = dataContext.Connection.ExecuteScalar<int?>("SELECT IDENT_CURRENT('File')") ?? 0;

                    }
                    else if (fi.Name.Equals(record.FilePrefix + "_HC-R.csv"))
                    {
                        RTableName = "RTable" + record.FilePrefix;
                        RTable = CSVToDataTable(fi.FullName);
                        dataContext.Table<Model.File>().AddNewRecord(new Model.File()
                        {
                            Name = fi.Name,
                            FileTypeID = dataContext.Connection.ExecuteScalar<int>("SELECT ID FROM FileType WHERE Name = 'NodeRFile'"),
                            Data = JsonConvert.SerializeObject(RTable)
                        });
                        record.NodeRFileID = dataContext.Connection.ExecuteScalar<int?>("SELECT IDENT_CURRENT('File')") ?? 0;
                    }
                    else if (fi.Name.Equals(record.FilePrefix + "_HC-Z.csv"))
                    {
                        ZTableName = "ZTable" + record.FilePrefix;
                        ZTable = CSVToDataTable(fi.FullName);
                        dataContext.Table<Model.File>().AddNewRecord(new Model.File()
                        {
                            Name = fi.Name,
                            FileTypeID = dataContext.Connection.ExecuteScalar<int>("SELECT ID FROM FileType WHERE Name = 'NodeZFile'"),
                            Data = JsonConvert.SerializeObject(ZTable)
                        });
                        record.NodeZFileID = dataContext.Connection.ExecuteScalar<int?>("SELECT IDENT_CURRENT('File')") ?? 0;


                    }
                    else if (fi.Name.Contains(record.FilePrefix + "_segments.csv"))
                    {
                        LineTableName = "LineTable" + record.FilePrefix;
                        LineTable = CSVToDataTable(fi.FullName);
                        dataContext.Table<Model.File>().AddNewRecord(new Model.File()
                        {
                            Name = fi.Name,
                            FileTypeID = dataContext.Connection.ExecuteScalar<int>("SELECT ID FROM FileType WHERE Name = 'LineFile'"),
                            Data = JsonConvert.SerializeObject(LineTable)
                        });
                        record.LineFileID = dataContext.Connection.ExecuteScalar<int?>("SELECT IDENT_CURRENT('File')") ?? 0;

                    }
                    else
                        throw new Exception("Files have not been named correctly.");
                }



                record.Post = false;

                dataContext.Table<Result>().AddNewRecord(record);
                resultId = dataContext.Connection.ExecuteScalar<int?>("SELECT IDENT_CURRENT('Result')") ?? 0;
            }
            using (SqlConnection sqlConnection = new SqlConnection(MainWindow.SystemSettings["ConnectionString"].Value))
            using(SqlBulkCopy bulkCopy = new SqlBulkCopy(sqlConnection))
            {
                sqlConnection.Open();

                try
                {
                    bulkCopy.BulkCopyTimeout = 0;

                    sqlConnection.ExecuteNonQuery($"CREATE TABLE [" + RTableName + $"] ({string.Join(", ", RTable.Columns.Cast<DataColumn>().Select(x => "[" + x.ColumnName + "] varchar(MAX) "))})", null);
                    foreach (DataColumn column in RTable.Columns)
                        bulkCopy.ColumnMappings.Add(column.ColumnName, column.ColumnName);
                    bulkCopy.DestinationTableName = RTableName;
                    bulkCopy.WriteToServer(RTable);

                    bulkCopy.ColumnMappings.Clear();
                    sqlConnection.ExecuteNonQuery($"CREATE TABLE [" + ZTableName + $"] ({string.Join(",", ZTable.Columns.Cast<DataColumn>().Select(x => "[" + x.ColumnName + "] varchar(MAX) "))})", null);
                    foreach (DataColumn column in ZTable.Columns)
                        bulkCopy.ColumnMappings.Add(column.ColumnName, column.ColumnName);
                    bulkCopy.DestinationTableName = ZTableName;
                    bulkCopy.WriteToServer(ZTable);

                    bulkCopy.ColumnMappings.Clear();
                    sqlConnection.ExecuteNonQuery($"CREATE TABLE [" + LDGTableName + $"] ({string.Join(",", LDGTable.Columns.Cast<DataColumn>().Select(x => "[" + x.ColumnName + "] varchar(MAX) "))})", null);
                    foreach (DataColumn column in LDGTable.Columns)
                        bulkCopy.ColumnMappings.Add(column.ColumnName, column.ColumnName);
                    bulkCopy.DestinationTableName = LDGTableName;
                    bulkCopy.WriteToServer(LDGTable);

                    bulkCopy.ColumnMappings.Clear();
                    sqlConnection.ExecuteNonQuery($"CREATE TABLE [" + LineTableName + $"] ({string.Join(",", LineTable.Columns.Cast<DataColumn>().Select(x => "[" + x.ColumnName + "] varchar(MAX) "))})", null);
                    foreach (DataColumn column in LineTable.Columns)
                        bulkCopy.ColumnMappings.Add(column.ColumnName, column.ColumnName);
                    bulkCopy.DestinationTableName = LineTableName;
                    bulkCopy.WriteToServer(LineTable);

                    // Add node points
                    sqlConnection.ExecuteNonQuery($"INSERT INTO NodePoint (ResultID, Name, xCoordinate, yCoordinate, Latitude, Longitude) SELECT {resultId},Node,Xcoord, Ycoord, Drive.dbo.ReverseMercatorLat(Xcoord, Ycoord), Drive.dbo.ReverseMercatorLng(Xcoord, Ycoord) FROM [{RTableName}]", null);

                    // Add Lines Characteristics
                    sqlConnection.ExecuteNonQuery(@"INSERT INTO LineCharacteristicsToDisplay (Label, Name, FileTypeID, Display) SELECT COLUMN_NAME, COLUMN_NAME, (Select ID FROM FileType WHERE Name = TABLE_NAME), 0
                                                    FROM Drive.INFORMATION_SCHEMA.COLUMNS as ColumnTable 
                                                    WHERE (TABLE_Name = N'" + RTableName + @"' OR TABLE_NAME = N'" + ZTableName + @"' OR TABLE_NAME = N'" + LDGTableName + @"' OR TABLE_NAME = N'" + LineTableName + @"') AND COLUMN_NAME NOT LIKE N'%DER%' AND COLUMN_NAME != N'Xcoord' 
                                                    AND COLUMN_NAME != N'Ycoord' AND COLUMN_NAME NOT LIKE  N'%Node%' and COLUMN_NAME NOT IN (SELECT NAME FROM LineCharacteristicsToDisplay)", null);
                    // Add Result Characteristics
                    sqlConnection.ExecuteNonQuery(@"INSERT INTO ResultsToDisplay (Label, Name, FileTypeID, Display) SELECT COLUMN_NAME, COLUMN_NAME, (Select ID FROM FileType WHERE Name = TABLE_NAME), 0
                                                    FROM Drive.INFORMATION_SCHEMA.COLUMNS as ColumnTable 
                                                    WHERE (TABLE_Name = N'" + RTableName + @"' OR TABLE_NAME = N'" + ZTableName + @"' OR TABLE_NAME = N'" + LDGTableName + @"' OR TABLE_NAME = N'" + LineTableName + @"') AND COLUMN_NAME LIKE N'%DER%' AND COLUMN_NAME != N'Xcoord' 
                                                    AND COLUMN_NAME != N'Ycoord' AND COLUMN_NAME NOT LIKE  N'%Node%' and COLUMN_NAME NOT IN (SELECT NAME FROM ResultsToDisplay)", null);

                    // Add data to LineSegments
                    string sqlString = @"
                                DECLARE @alterAssignment nvarchar(max) = ''

                                SELECT @alterAssignment = @alterAssignment + '""' +COLUMN_NAME + '"":""'' + '  + Table_Name +'.[' +COLUMN_NAME + '] + ''"",' 
                                FROM DRIVE.INFORMATION_SCHEMA.COLUMNS
                                WHERE (TABLE_Name = N'" + RTableName + @"' OR TABLE_NAME = N'" + ZTableName + @"' OR TABLE_NAME = N'" + LDGTableName + @"' OR TABLE_NAME = N'" + LineTableName + @"') AND 
                                       COLUMN_NAME != 'FromNode' AND COLUMN_NAME != 'ToNode' AND COLUMN_NAME != 'node name' AND COLUMN_NAME != 'node' AND COLUMN_NAME != 'Xcoord' AND COLUMN_NAME != 'Ycoord' AND COLUMN_NAME NOT LIKE '%DER%'

                                DECLARE @sql1 nvarchar(max)= N'
                                    INSERT INTO LineSegment (ResultID, FromNodeID, ToNodeID, Data) 
                                    SELECT  " + resultId + @", 
                                            (SELECT ID FROM NodePoint WHERE Name = ["+ LineTableName + @"].FromNode AND ResultID=" + resultId + @"), 
                                            (SELECT ID FROM NodePoint WHERE Name = ["+ LineTableName + @"].ToNode AND ResultID=" + resultId + @"), 
                                            ''{'+ SUBSTRING(@alterAssignment, 0, len(@alterAssignment)) +'}'' AS Data '
                                DECLARE @sql2 nvarchar(max) = N'
                                     FROM [" + RTableName + @"] JOIN
                                          [" + ZTableName + @"] ON [" + RTableName + @"].Node = ["+ ZTableName + @"].Node JOIN
	                                      [" + LDGTableName + @"] ON ["+ LDGTableName + @"].[node name] = [" + RTableName + @"].Node JOIN
	                                      [" + LineTableName + @"] ON [" + LineTableName + @"].tonode = [" + RTableName + @"].Node'
                                exec (@sql1 + @sql2)";
                    sqlConnection.ExecuteNonQuery(sqlString, null);

                    //Add data to Individual Results
                    sqlString = @"                                
                                DECLARE @alterAssignment nvarchar(max) = ''

                                SELECT @alterAssignment = @alterAssignment + '""' + COLUMN_NAME + '"":""'' + '  + Table_Name +'.[' +COLUMN_NAME + '] + ''"",' 
                                FROM DRIVE.INFORMATION_SCHEMA.COLUMNS
                                WHERE (TABLE_Name = N'" + RTableName + @"' OR TABLE_NAME = N'" + ZTableName + @"' OR TABLE_NAME = N'" + LDGTableName + @"' OR TABLE_NAME = N'" + LineTableName + @"') AND 
                                       COLUMN_NAME != 'FromNode' AND COLUMN_NAME != 'ToNode' AND COLUMN_NAME != 'node name' AND COLUMN_NAME != 'node' AND COLUMN_NAME != 'Xcoord' AND COLUMN_NAME != 'Ycoord' AND COLUMN_NAME LIKE '%DER%'

                                DECLARE @sql1 nvarchar(max)= N'
                                    INSERT INTO IndividualResult (ResultID, NodePointID, Data) 
                                    SELECT  " + resultId + @", 
                                            (SELECT ID FROM NodePoint WHERE Name = [" + LineTableName + @"].ToNode AND ResultID=" + resultId + @"), 
                                            ''{'+ SUBSTRING(@alterAssignment, 0, len(@alterAssignment)) +'}'' AS Data '
                                DECLARE @sql2 nvarchar(max) = N'
                                     FROM [" + RTableName + @"] JOIN
                                          [" + ZTableName + @"] ON [" + RTableName + @"].Node = [" + ZTableName + @"].Node JOIN
	                                      [" + LDGTableName + @"] ON [" + LDGTableName + @"].[node name] = [" + RTableName + @"].Node JOIN
	                                      [" + LineTableName + @"] ON [" + LineTableName + @"].tonode = [" + RTableName + @"].Node'
                                exec (@sql1 + @sql2)";
                    sqlConnection.ExecuteNonQuery(sqlString, null);


                }
                finally
                {
                    string sql = @"DROP TABLE [" + RTableName + @"]
                                   DROP TABLE [" + ZTableName + @"]
                                   DROP TABLE [" + LDGTableName + @"]
                                   DROP TABLE [" + LineTableName + @"]";
                    sqlConnection.ExecuteNonQuery(sql, null);
                    sqlConnection.Close();
                }

            }

        }

        [AuthorizeHubRole("Administrator, Editor")]
        [RecordOperation(typeof(Result), RecordOperation.UpdateRecord)]
        public void UpdateResult(Result record)
        {
            DataContext.Table<Result>().UpdateRecord(record);
        }

        [AuthorizeHubRole("Administrator, Editor")]
        public void AddNewOrUpdateResult(Result record)
        {
            DataContext.Table<Result>().AddNewOrUpdateRecord(record);
        }

        public class ConflictedGroup
        {
            public string Name;
            public string NewName = "";
            public bool Skip = false;
            public bool Overwrite = false;
            public bool Edit = false;
        }

        public class FileInfoWrapper
        {
            public string Name;
            public IGrouping<string,FileInfo> FileInfo;
        }

        public void UpdateMassImportWaitFlag(bool value, IEnumerable<ConflictedGroup> conflictedGroups)
        {
            s_massImportWaitFlag = value;
            s_returnConflictedGroup = conflictedGroups;
        }

        [AuthorizeHubRole("Administrator, Editor")]
        public void MassAddNewResult(string directory)
        {
            s_massImportWaitFlag = false;
            DirectoryInfo df = new DirectoryInfo(directory);
            IEnumerable<FileInfo> fi = df.GetFiles();
            IEnumerable<Result> results = DataContext.Table<Result>().QueryRecords();
            List<FileInfoWrapper> groups = fi.GroupBy(x => x.Name.Split('_')[0]).Select(x => new FileInfoWrapper() { Name = x.Key, FileInfo = x}).ToList();
            IEnumerable<ConflictedGroup> conflictedGroups = groups.Where(x => results.Select(y => y.Name).Contains(x.Name)).Select(x => new ConflictedGroup() { Name = x.Name});

            if (conflictedGroups.Any())
            {
                GlobalHost.ConnectionManager.GetHubContext<DataHub>().Clients.All.notifyUserOfGroups(conflictedGroups);

                while (!s_massImportWaitFlag) { }

                foreach(ConflictedGroup group in s_returnConflictedGroup)
                {
                    if (group.Skip) groups.Remove(groups.Where(x => x.Name == group.Name).First());
                    else if (group.Overwrite)
                    {
                        CascadeDelete("Result", $"Name LIKE '{group.Name}'");
                    } 
                    else if (group.Edit)
                    {
                        groups.Where(x => x.Name == group.Name).FirstOrDefault().Name = group.NewName;
                    }
                }

            }

            int progressTotal = (groups.Count() > 0 ? groups.Count() : 1);
            int progressCount = 0;

            Parallel.ForEach(groups, group =>
            {
                ProgressUpdate(group.Name, (int)(100 * (progressCount) / progressTotal));
                try
                {
                    if (group.FileInfo.Count() == 4)
                    {
                        Result result = new Result()
                        {
                            Name = group.Name,
                            FilePrefix = group.FileInfo.Key,
                            Directory = directory
                        };

                        AddNewResult(result);
                    }
                    else
                        throw new Exception("File group with prefix " + group.Name + " does not contain correct number of files.");
                }
                catch (Exception ex)
                {
                    LogException(ex);
                }
                ProgressUpdate(group.Name, (int)(100 * (++progressCount) / progressTotal));
            });
        }

        #endregion

        #region [ Preview Operations ]

        public string GetLineSegmentsForMap(List<int> resultId, int resultSelect)
        {
            if (resultId.Count() == 0) return "";
            IEnumerable<ResultsToDisplay> fieldsToDisplay = DataContext.Table<ResultsToDisplay>().QueryRecordsWhere("Display = 1");
            IEnumerable<LineCharacteristicsToDisplay> lineCharacteristicsToDisplay = DataContext.Table<LineCharacteristicsToDisplay>().QueryRecordsWhere("Display = 1");
            List<string> fieldNames = new List<string>();
            List<string> fieldNamesForSelect = new List<string>();
            foreach (ResultsToDisplay field in fieldsToDisplay)
            {
                fieldNames.Add(@"(cast(dbo.GetJSONValueForProperty(IndividualREsult.Data, '" + field.Name + @"')as float))");
                fieldNamesForSelect.Add(@"'""" + field.Name + @""":""' + dbo.GetJSONValueForProperty(IndividualREsult.Data, '" + field.Name + @"') + '""'");
            }

            List<string> lineNames = new List<string>();
            foreach (LineCharacteristicsToDisplay lineCharacteristic in lineCharacteristicsToDisplay)
            {
                lineNames.Add(@"'""" + lineCharacteristic.Name + @""":""' + dbo.GetJSONValueForProperty(LineSegment.Data, '" + lineCharacteristic.Name + @"') + '""'");

            }

            DataTable table = new DataTable();
            using (IDbCommand sc = DataContext.Connection.Connection.CreateCommand())
            {
                string sqlCommand = @"SELECT LineSegment.ID, FromNode.Latitude as FromLatitude, FromNode.Longitude as FromLongitude, FromNode.Name as FromNodeName, ToNode.Latitude as ToLatitude, 
                                             ToNode.Longitude as ToLongitude, ToNode.Name as ToNodeName, '{' + " + string.Join("+','+", lineNames) + @"+'}' as LineData, Result, '{' + " + string.Join("+','+", fieldNamesForSelect) + @"+'}' as ResultList
                                    FROM LineSegment JOIN
	                                     NodePoint as FromNode ON FromNode.ID = LineSegment.FromNodeID JOIN
	                                     NodePoint as ToNode ON ToNode.ID = LineSegment.ToNodeID Join
	                                     IndividualResult ON IndividualResult.NodePointID = LineSegment.ToNodeID
                                    CROSS APPLY (
	                                    SELECT COALESCE(" + (resultSelect == 1 ? "MIN" : "MAX") + @"(d),0) Result 
	                                    FROM (
		                                        VALUES " + string.Join(",", fieldNames) + @"
                                             ) as A(d)) A 
                                    WHERE LineSegment.ResultID IN (" + string.Join(",", resultId) + ")";

                sc.CommandText = sqlCommand;
                sc.CommandType = CommandType.Text;

                IDataReader rdr = sc.ExecuteReader();
                table.Load(rdr);
            }


            return JsonConvert.SerializeObject(table);
        }
        #endregion

        #region [ NodePoint Table Operations ]

        [RecordOperation(typeof(NodePoint), RecordOperation.QueryRecordCount)]
        public int QueryNodePointCount(int resultId, string filterText)
        {
            TableOperations<NodePoint> table = DataContext.Table<NodePoint>();
            RecordRestriction restritcion = table.GetSearchRestriction(filterText) + new RecordRestriction("ResultID = {0}", resultId);

            return table.QueryRecordCount(restritcion);
        }

        [RecordOperation(typeof(NodePoint), RecordOperation.QueryRecords)]
        public IEnumerable<NodePoint> QueryNodePoint(int resultId, string sortField, bool ascending, int page, int pageSize, string filterText)
        {
            TableOperations<NodePoint> table = DataContext.Table<NodePoint>();
            RecordRestriction restritcion = table.GetSearchRestriction(filterText) + new RecordRestriction("ResultID = {0}", resultId);

            return table.QueryRecords(sortField, ascending, page, pageSize, restritcion);
        }

        [AuthorizeHubRole("Administrator, Editor")]
        [RecordOperation(typeof(NodePoint), RecordOperation.DeleteRecord)]
        public void DeleteNodePoint(int id)
        {
            DataContext.Table<NodePoint>().DeleteRecord(id);
        }

        [RecordOperation(typeof(NodePoint), RecordOperation.CreateNewRecord)]
        public NodePoint NewNodePoint()
        {
            return DataContext.Table<NodePoint>().NewRecord();
        }

        [AuthorizeHubRole("Administrator, Editor")]
        [RecordOperation(typeof(NodePoint), RecordOperation.AddNewRecord)]
        public void AddNewNodePoint(NodePoint line)
        {
            DataContext.Table<NodePoint>().AddNewRecord(line);
        }

        [AuthorizeHubRole("Administrator, Editor")]
        [RecordOperation(typeof(NodePoint), RecordOperation.UpdateRecord)]
        public void UpdateNodePoint(NodePoint line)
        {
            DataContext.Table<NodePoint>().UpdateRecord(line);
        }

        [AuthorizeHubRole("Administrator, Editor")]
        public void AddNewOrUpdateNodePoint(NodePoint line)
        {
            DataContext.Table<NodePoint>().AddNewOrUpdateRecord(line);
        }

        #endregion

        #region [ LineSegment Table Operations ]

        [RecordOperation(typeof(LineSegmentView), RecordOperation.QueryRecordCount)]
        public int QueryLineSegmentCount(int resultId, string filterText)
        {
            TableOperations<LineSegmentView> table = DataContext.Table<LineSegmentView>();
            RecordRestriction restritcion = table.GetSearchRestriction(filterText) + new RecordRestriction("ResultID = {0}", resultId);

            return table.QueryRecordCount(restritcion);
        }

        [RecordOperation(typeof(LineSegmentView), RecordOperation.QueryRecords)]
        public IEnumerable<LineSegmentView> QueryLineSegment(int resultId, string sortField, bool ascending, int page, int pageSize, string filterText)
        {
            TableOperations<LineSegmentView> table = DataContext.Table<LineSegmentView>();
            RecordRestriction restritcion = table.GetSearchRestriction(filterText) + new RecordRestriction("ResultID = {0}", resultId);

            return table.QueryRecords(sortField, ascending, page, pageSize, restritcion);
        }

        [AuthorizeHubRole("Administrator, Editor")]
        [RecordOperation(typeof(LineSegmentView), RecordOperation.DeleteRecord)]
        public void DeleteLineSegment(int id)
        {
            DataContext.Table<LineSegment>().DeleteRecord(id);
        }

        [RecordOperation(typeof(LineSegmentView), RecordOperation.CreateNewRecord)]
        public LineSegment NewLineSegment()
        {
            return DataContext.Table<LineSegmentView>().NewRecord();
        }

        [AuthorizeHubRole("Administrator, Editor")]
        [RecordOperation(typeof(LineSegmentView), RecordOperation.AddNewRecord)]
        public void AddNewLineSegment(LineSegmentView line)
        {
            DataContext.Table<LineSegment>().AddNewRecord(line);
        }

        [AuthorizeHubRole("Administrator, Editor")]
        [RecordOperation(typeof(LineSegmentView), RecordOperation.UpdateRecord)]
        public void UpdateLineSegment(LineSegmentView line)
        {
            DataContext.Table<LineSegment>().UpdateRecord(line);
        }

        [AuthorizeHubRole("Administrator, Editor")]
        public void AddNewOrUpdateLineSegment(LineSegmentView line)
        {
            DataContext.Table<LineSegment>().AddNewOrUpdateRecord(line);
        }

        #endregion

        #region [ IndividualResult Table Operations ]

        [RecordOperation(typeof(IndividualResult), RecordOperation.QueryRecordCount)]
        public int QueryIndividualResultCount(int resultId, string filterText)
        {
            TableOperations<IndividualResult> table = DataContext.Table<IndividualResult>();
            RecordRestriction restritcion = table.GetSearchRestriction(filterText) + new RecordRestriction("ResultID = {0}", resultId);

            return table.QueryRecordCount(restritcion);
        }

        [RecordOperation(typeof(IndividualResult), RecordOperation.QueryRecords)]
        public IEnumerable<IndividualResultView> QueryIndividualResult(int resultId, string sortField, bool ascending, int page, int pageSize, string filterText)
        {
            TableOperations<IndividualResultView> table = DataContext.Table<IndividualResultView>();
            RecordRestriction restritcion = table.GetSearchRestriction(filterText) + new RecordRestriction("ResultID = {0}", resultId);

            return table.QueryRecords(sortField, ascending, page, pageSize, restritcion);
        }

        [AuthorizeHubRole("Administrator, Editor")]
        [RecordOperation(typeof(IndividualResult), RecordOperation.DeleteRecord)]
        public void DeleteIndividualResult(int id)
        {
            DataContext.Table<IndividualResult>().DeleteRecord(id);
        }

        [RecordOperation(typeof(IndividualResult), RecordOperation.CreateNewRecord)]
        public IndividualResult NewIndividualResult()
        {
            return DataContext.Table<IndividualResult>().NewRecord();
        }

        [AuthorizeHubRole("Administrator, Editor")]
        [RecordOperation(typeof(IndividualResult), RecordOperation.AddNewRecord)]
        public void AddNewIndividualResult(IndividualResult line)
        {
            DataContext.Table<IndividualResult>().AddNewRecord(line);
        }

        [AuthorizeHubRole("Administrator, Editor")]
        [RecordOperation(typeof(IndividualResult), RecordOperation.UpdateRecord)]
        public void UpdateIndividualResult(IndividualResult line)
        {
            DataContext.Table<IndividualResult>().UpdateRecord(line);
        }

        [AuthorizeHubRole("Administrator, Editor")]
        public void AddNewOrUpdateIndividualResult(IndividualResult line)
        {
            DataContext.Table<IndividualResult>().AddNewOrUpdateRecord(line);
        }

        #endregion

        #region [ DirectoryBrowser Hub Operations ]

        public IEnumerable<string> LoadDirectories(string rootFolder, bool showHidden, bool showFiles)
        {
            if (string.IsNullOrWhiteSpace(rootFolder))
                return Directory.GetLogicalDrives();

            IEnumerable<string> directories = Directory.GetDirectories(rootFolder);
            IEnumerable<string> files = Directory.GetFiles(rootFolder);
            if (!showHidden)
                directories = directories.Where(path => !new DirectoryInfo(path).Attributes.HasFlag(FileAttributes.Hidden));

            IEnumerable<string> returnString = new[] { "..\\" }.Concat(directories.Select(path => FilePath.AddPathSuffix(FilePath.GetLastDirectoryName(path))));

            if (showFiles)
                returnString = returnString.Concat(files.Select(file => file.Split('\\')[file.Split('\\').Count() - 1]));

            return returnString;
        }

        public IEnumerable<string> LoadDirectories(string rootFolder, bool showHidden) => DirectoryBrowserOperations.LoadDirectories(rootFolder, showHidden);
        public bool IsLogicalDrive(string path) => DirectoryBrowserOperations.IsLogicalDrive(path);
        public string ResolvePath(string path) => DirectoryBrowserOperations.ResolvePath(path);
        public string CombinePath(string path1, string path2) => DirectoryBrowserOperations.CombinePath(path1, path2);
        public void CreatePath(string path) => DirectoryBrowserOperations.CreatePath(path);

        #endregion

        #region [ Miscellaneous Hub Operations ]

        /// <summary>
        /// Gets UserAccount table ID for current user.
        /// </summary>
        /// <returns>UserAccount.ID for current user.</returns>
        public Guid GetCurrentUserID()
        {
            Guid userID;
            AuthorizationCache.UserIDs.TryGetValue(Thread.CurrentPrincipal.Identity.Name, out userID);
            return userID;
        }

        /// <summary>
        /// Gets the current server time.
        /// </summary>
        /// <returns>Current server time.</returns>
        public DateTime GetServerTime() => DateTime.UtcNow;

        private void CascadeDelete(string tableName, string criterion)
        {

            using (IDbCommand sc = DataContext.Connection.Connection.CreateCommand())
            {

                //sc.CommandText = "DECLARE @context VARBINARY(128)\n SELECT @context = CONVERT(VARBINARY(128), CONVERT(VARCHAR(128), @userName))\n SET CONTEXT_INFO @context";
                //IDbDataParameter param = sc.CreateParameter();
                //param.ParameterName = "@userName";
                //param.Value = GetCurrentUserName();
                //sc.Parameters.Add(param);
                //sc.ExecuteNonQuery();
                //sc.Parameters.Clear();


                sc.CommandText = "dbo.UniversalCascadeDelete";
                sc.CommandType = CommandType.StoredProcedure;
                IDbDataParameter param1 = sc.CreateParameter();
                param1.ParameterName = "@tableName";
                param1.Value = tableName;
                IDbDataParameter param2 = sc.CreateParameter();
                param2.ParameterName = "@baseCriteria";
                param2.Value = criterion;
                sc.Parameters.Add(param1);
                sc.Parameters.Add(param2);
                sc.ExecuteNonQuery();
            }
        }

        private DataTable CSVToDataTable(string fileName)
        {
            var streamReader = new StreamReader(fileName);
            var csv = new CsvParser(streamReader);
            DataTable dt = new DataTable();
            string[] headers = new string[0];
            while (true)
            {
                var row = csv.Read();
                if (row == null) break;

                if (csv.Context.Row == 1)
                {
                    foreach (string header in row)
                        dt.Columns.Add(header, typeof(string));
                    headers = row;
                }
                else
                {
                    var record = dt.NewRow();
                    for (int x = 0; x < row.Count(); ++x)
                    {
                        record[headers[x]] = row[x];
                    }
                    dt.Rows.Add(record);

                }

            }

            return dt;
        }
        #endregion


    }

    public class ServiceHub: Hub
    {
        public class DateTimeArg: EventArgs
        {
            public DateTime DateTime { get; set; }
        }

        #region [ Static Event Handlers ]
        public static event EventHandler<DateTimeArg> GetServerTimeEvent;
        #endregion

        public DateTime GetServerTime()
        {
            DateTimeArg dta = new DateTimeArg();
            GetServerTimeEvent?.Invoke(new object(), dta);
            return dta.DateTime;
        }

        public string GetPerformanceStatistics()
        {
            return null;
        }

        public void SendCommand(string command)
        {
            
        }

    }
}
