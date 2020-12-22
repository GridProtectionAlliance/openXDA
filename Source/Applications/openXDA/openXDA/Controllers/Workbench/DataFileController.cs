//******************************************************************************************************
//  EmailTemplateController.cs - Gbtc
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
//  08/19/2019 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Http;
using GSF.Configuration;
using GSF.Data;
using GSF.Data.Model;
using log4net;
using openXDA.Configuration;
using openXDA.Model;
using openXDA.Nodes;
using openXDA.Nodes.Types.Analysis;
using openXDA.Nodes.Types.FileProcessing;
using openXDA.Nodes.Types.FilePruning;
using ConfigurationLoader = openXDA.Nodes.ConfigurationLoader;

namespace openXDA.Controllers.Config
{
    [RoutePrefix("api/Workbench/DataFiles")]
    public class DataFileController : ApiController
    {
        #region [ Members ]

        // Nested Types
        public class GetDataFilesPostData
        {
            public string sortField { get; set; }
            public bool ascending { get; set; }
            public int page { get; set; }
            public int pageSize { get; set; }
            public string filterString { get; set; }
        }

        private class Settings
        {
            public Settings(Action<object> configure) =>
                configure(this);

            [Category]
            [SettingName(FileWatcherSection.CategoryName)]
            public FileWatcherSection FileWatcherSettings { get; }
                = new FileWatcherSection();

            [Category]
            [SettingName(FileProcessorSection.CategoryName)]
            public FileProcessorSection FileProcessorSettings { get; }
                = new FileProcessorSection();
        }

        #endregion

        #region [ Constructors ]

        public DataFileController(Host nodeHost) =>
            NodeHost = nodeHost;

        #endregion

        #region [ Properties ]

        private Host NodeHost { get; }

        #endregion

        #region [ Methods ]

        [Route("Count"), HttpPost]
        public int GetCount([FromBody] GetDataFilesPostData postData)
        {
            using (AdoDataConnection connection = NodeHost.CreateDbConnection())
            {
                const string QueryFormat =
                    "SELECT COUNT(*) " +
                    "FROM " +
                    "( " +
                    "    SELECT DISTINCT FileGroupID " +
                    "    FROM DataFile " +
                    "    WHERE {0} " +
                    ") DataFile";

                TableOperations<DataFile> dataFileTable = new TableOperations<DataFile>(connection);
                RecordRestriction restriction = dataFileTable.GetSearchRestriction(postData.filterString);
                string query = string.Format(QueryFormat, restriction?.FilterExpression ?? "1=1");
                return connection.ExecuteScalar<int>(query, restriction?.Parameters ?? new object[0]);
            }
        }

        [Route(""), HttpPost]
        public DataTable GetDataFiles([FromBody] GetDataFilesPostData postData)
        {
            using (AdoDataConnection connection = NodeHost.CreateDbConnection())
            {
                const string QueryFormat =
                    "WITH cte AS " +
                    "( " +
                    "    SELECT * " +
                    "    FROM DataFile " +
                    "    WHERE {0} " +
                    ") " +
                    "SELECT DataFile.* " +
                    "FROM " +
                    "( " +
                    "    SELECT " +
                    "        ROW_NUMBER() OVER(ORDER BY {2}) AS RowNumber, " +
                    "        DataFile.* " +
                    "    FROM (SELECT DISTINCT FileGroupID ID FROM cte) FileGroup CROSS APPLY " +
                    "    ( " +
                    "        SELECT TOP 1 * " +
                    "        FROM cte DataFile " +
                    "        WHERE DataFile.FileGroupID = FileGroup.ID " +
                    "        ORDER BY FileSize DESC, FilePath " +
                    "    ) DataFile " +
                    ") DataFile " +
                    "WHERE {1} " +
                    "ORDER BY RowNumber";

                TableOperations<DataFile> dataFileTable = new TableOperations<DataFile>(connection);

                RecordRestriction searchRestriction = dataFileTable.GetSearchRestriction(postData.filterString)
                    ?? new RecordRestriction("1=1");

                string searchClause = searchRestriction.FilterExpression;

                int paramIndex = searchRestriction.Parameters.Length;
                string pageClause = $"RowNumber BETWEEN {{{paramIndex}}} AND {{{paramIndex + 1}}}";

                string sortOrder = postData.ascending ? "ASC" : "DESC";
                string orderByClause = $"{postData.sortField} {sortOrder}";
                string query = string.Format(QueryFormat, searchClause, pageClause, orderByClause);

                int pageStart = (postData.page - 1) * postData.pageSize + 1;
                int pageEnd = pageStart + postData.pageSize - 1;

                object[] parameters = searchRestriction.Parameters
                    .Concat(new object[] { pageStart, pageEnd })
                    .ToArray();

                return connection
                    .RetrieveData(query, parameters);
            }
        }

        [Route("ReprocessFile/{fileGroupID:int}"), HttpPost]
        public async Task ReprocessFile(int fileGroupID)
        {
            using (AdoDataConnection connection = NodeHost.CreateDbConnection())
            {
                int? meterID = connection.ExecuteScalar<int?>("SELECT MeterID FROM Event WHERE FileGroupID = {0}", fileGroupID);

                if (meterID is null)
                {
                    string[] files = new TableOperations<DataFile>(connection)
                        .QueryRecordsWhere("FileGroupID = {0}", fileGroupID)
                        .Select(dataFile => dataFile.FilePath)
                        .ToArray();

                    const string NodeQueryFormat =
                        "SELECT ID " +
                        "FROM Node " +
                        "WHERE TypeName = {0}";

                    Type fileProcessorType = typeof(FileProcessorNode);
                    string typeName = fileProcessorType.FullName;

                    using (DataTable nodeResult = connection.RetrieveData(NodeQueryFormat, typeName))
                    {
                        Func<AdoDataConnection> connectionFactory = NodeHost.CreateDbConnection;

                        string meterKey = nodeResult
                            .AsEnumerable()
                            .Select(row => row.ConvertField<int>("ID"))
                            .Select(id => new ConfigurationLoader(0, id, connectionFactory))
                            .Select(configurator => new Settings(configurator.Configure))
                            .Select(settings => new { settings.FileWatcherSettings.WatchDirectoryList, settings.FileProcessorSettings.FilePattern })
                            .SelectMany(record => record.WatchDirectoryList, (record, Directory) => new { Directory, record.FilePattern })
                            .SelectMany(record => files, (record, File) => new { File, record.Directory, record.FilePattern })
                            .Where(record => record.File.StartsWith(record.Directory, StringComparison.OrdinalIgnoreCase))
                            .Select(record => Regex.Match(record.File, record.FilePattern))
                            .Where(match => match.Success)
                            .Select(match => match.Groups["AssetKey"]?.Value)
                            .Where(assetKey => assetKey != null)
                            .FirstOrDefault();

                        if (!(meterKey is null))
                            meterID = connection.ExecuteScalar<int?>("SELECT ID FROM Meter WHERE AssetKey = {0}", meterKey);
                    }
                }

                if (meterID is null)
                    return;

                CascadeDelete(connection, "Event", $"FileGroupID = {fileGroupID}");
                CascadeDelete(connection, "EventData", $"FileGroupID = {fileGroupID}");

                TableOperations<AnalysisTask> analysisTaskTable = new TableOperations<AnalysisTask>(connection);
                AnalysisTask analysisTask = new AnalysisTask();
                analysisTask.FileGroupID = fileGroupID;
                analysisTask.MeterID = meterID.GetValueOrDefault();
                analysisTask.Priority = 3;
                analysisTaskTable.AddNewRecord(analysisTask);

                Type analysisNodeType = typeof(AnalysisNode);
                await NotifyNodes(analysisNodeType, "PollTaskQueue");
            }
        }

        [Route("ReprocessFiles"), HttpPost]
        public async Task ReprocessFiles([FromBody] List<int> meterIDs, [FromBody] DateTime startDate, [FromBody] DateTime endDate)
        {
            using (AdoDataConnection connection = NodeHost.CreateDbConnection())
            {
                IEnumerable<DataRow> QueryFileGroupIDs(IEnumerable<int> ids)
                {
                    List<int> idList = ids.ToList();

                    if (!ids.Any())
                        yield break;

                    IEnumerable<string> parameterList = idList
                        .Select((_, index) => $"{{{index + 2}}}");

                    string parameterFormats = string.Join(",", parameterList);

                    string queryFormat =
                        $"SELECT DISTINCT " +
                        $"    FileGroup.ID FileGroupID, " +
                        $"    Meter.ID MeterID " +
                        $"FROM " +
                        $"    FileGroup JOIN " +
                        $"    Event ON Event.FileGroupID = FileGroup.ID JOIN " +
                        $"    Meter ON Event.MeterID = Meter.ID " +
                        $"WHERE " +
                        $"    Meter.ID IN ({parameterFormats}) AND " +
                        $"    Event.StartTime BETWEEN {{0}} AND {{1}}";

                    object[] parameterValues = idList
                        .Cast<object>()
                        .Prepend(endDate)
                        .Prepend(startDate)
                        .ToArray();

                    using (DataTable result = connection.RetrieveData(queryFormat, parameterValues))
                    {
                        foreach (DataRow row in result.Rows)
                            yield return row;
                    }
                };

                var records = meterIDs
                    .Select((MeterID, Index) => new { MeterID, Index })
                    .GroupBy(record => record.Index / 2000, record => record.MeterID)
                    .SelectMany(QueryFileGroupIDs)
                    .Select(row => new
                    {
                        FileGroupID = row.ConvertField<int>("FileGroupID"),
                        MeterID = row.ConvertField<int>("MeterID")
                    })
                    .ToList();

                IEnumerable<int> fileGroupIDs = records
                    .Select(record => record.FileGroupID);

                string fileGroupList = string.Join(",", fileGroupIDs);
                CascadeDelete(connection, "Event", $"FileGroupID IN ({fileGroupList})");

                Type filePrunerNodeType = typeof(FilePrunerNode);
                _ = NotifyNodes(filePrunerNodeType, "PurgeOrphanData");

                var groupings = records
                    .Select((Record, Index) => new { Record, Index })
                    .GroupBy(record => record.Index / 1000, record => record.Record);

                foreach (var grouping in groupings)
                {
                    var list = grouping.ToList();

                    IEnumerable<string> parameterList = list
                        .Select((_, index) => $"SELECT {{{index * 2}}}, {{{index * 2 + 1}}}, 1");

                    string selectFormat = string.Join(" UNION ", parameterList);
                    string queryFormat = $"INSERT INTO AnalysisTask(FileGroupID, MeterID, Priority) {selectFormat}";

                    object[] parameterValues = list
                        .SelectMany(record => new[] { record.FileGroupID, record.MeterID })
                        .Cast<object>()
                        .ToArray();

                    connection.ExecuteNonQuery(queryFormat, parameterValues);
                }

                Type analysisNodeType = typeof(AnalysisNode);
                await NotifyNodes(analysisNodeType, "PollTaskQueue");
            }
        }

        private void CascadeDelete(AdoDataConnection connection, string tableName, string baseCriteria)
        {
            using (IDbCommand sc = connection.Connection.CreateCommand())
            {
                sc.CommandText = "dbo.UniversalCascadeDelete";
                sc.CommandType = CommandType.StoredProcedure;
                IDbDataParameter param1 = sc.CreateParameter();
                param1.ParameterName = "@tableName";
                param1.Value = tableName;
                IDbDataParameter param2 = sc.CreateParameter();
                param2.ParameterName = "@baseCriteria";
                param2.Value = baseCriteria;
                sc.Parameters.Add(param1);
                sc.Parameters.Add(param2);
                sc.ExecuteNonQuery();
            }
        }

        private async Task NotifyNodes(Type nodeType, string action)
        {
            async Task NotifyAsync(string url)
            {
                void ConfigureRequest(HttpRequestMessage request) =>
                    request.RequestUri = new Uri(url);

                using (HttpResponseMessage response = await NodeHost.SendWebRequestAsync(ConfigureRequest))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        string body = await response.Content.ReadAsStringAsync();

                        string logMessage = new StringBuilder()
                            .AppendLine("Analysis node notification failed.")
                            .AppendLine($"Status: {response.StatusCode}")
                            .AppendLine("Body:")
                            .Append(body)
                            .ToString();

                        Log.Debug(logMessage);
                    }
                }
            }

            const string QueryFormat =
                "SELECT Node.ID NodeID " +
                "FROM " +
                "    ActiveHost JOIN " +
                "    Node ON Node.HostRegistrationID = ActiveHost.ID JOIN " +
                "    NodeType ON Node.NodeTypeID = NodeType.ID " +
                "WHERE NodeType.TypeName = {0}";

            string typeName = nodeType.FullName;

            using (AdoDataConnection connection = NodeHost.CreateDbConnection())
            using (DataTable result = connection.RetrieveData(QueryFormat, typeName))
            {
                List<Task> notifyTasks = result
                    .AsEnumerable()
                    .Select(row =>
                    {
                        int nodeID = row.ConvertField<int>("NodeID");
                        string url = NodeHost.BuildURL(nodeID, action);
                        return NotifyAsync(url);
                    })
                    .ToList();

                await Task.WhenAll(notifyTasks);
            }
        }

        #endregion

        #region [ Static ]

        // Static Fields
        private static readonly ILog Log = LogManager.GetLogger(typeof(DataFileController));

        #endregion
    }
}