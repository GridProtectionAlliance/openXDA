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
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using GSF.Configuration;
using GSF.Data;
using GSF.Data.Model;
using log4net;
using Newtonsoft.Json.Linq;
using openXDA.Configuration;
using openXDA.Model;
using openXDA.Nodes;
using openXDA.Nodes.Types.FileProcessing;

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
                RecordRestriction restriction = dataFileTable.GetSearchRestriction(postData?.filterString ?? "");
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

                RecordRestriction searchRestriction = dataFileTable.GetSearchRestriction(postData?.filterString ?? "")
                    ?? new RecordRestriction("1=1");

                string searchClause = searchRestriction.FilterExpression;

                int paramIndex = searchRestriction.Parameters.Length;
                string pageClause = $"RowNumber BETWEEN {{{paramIndex}}} AND {{{paramIndex + 1}}}";

                string sortOrder = (postData?.ascending ?? true)? "ASC" : "DESC";
                string orderByClause = $"{postData?.sortField ?? "ID"} {sortOrder}";
                string query = string.Format(QueryFormat, searchClause, pageClause, orderByClause);

                int pageStart = ((postData?.page ?? 1) - 1) * (postData?.pageSize ?? 1) + 1;
                int pageEnd = pageStart + (postData?.pageSize ??1 ) - 1;

                object[] parameters = searchRestriction.Parameters
                    .Concat(new object[] { pageStart, pageEnd })
                    .ToArray();

                return connection
                    .RetrieveData(query, parameters);
            }
        }

        [Route("ReprocessFile/{fileGroupID:int}"), HttpPost]
        public async Task<int> ReprocessFile(int fileGroupID)
        {
            using (AdoDataConnection connection = NodeHost.CreateDbConnection())
            {
                CascadeDelete(connection, "Event", $"FileGroupID = {fileGroupID}");
                CascadeDelete(connection, "EventData", $"FileGroupID = {fileGroupID}");
                await ReprocessFileGroup(fileGroupID);
                return 1;
            }
        }

        [Route("ReprocessFilesByID"), HttpPost]
        public async Task<int> ReprocessFiles([FromBody] JObject fileGroupIDs)
        {
            List<int> fileGroupIDList = fileGroupIDs.ToObject<List<int>>();

            using (AdoDataConnection connection = NodeHost.CreateDbConnection())
            {
                List<Task> reprocessTasks = new List<Task>();

                foreach (int fileGroupID in fileGroupIDList)
                {
                    CascadeDelete(connection, "Event", $"FileGroupID = {fileGroupID}");
                    CascadeDelete(connection, "EventData", $"FileGroupID = {fileGroupID}");
                    reprocessTasks.Add(ReprocessFileGroup(fileGroupID));
                }

                await Task.WhenAll(reprocessTasks);

                return 1;
            }
        }

        [Route("Download/{fileID:int}"), HttpGet]
        public async Task<HttpResponseMessage> DownloadFile(int fileID)
        {
            using (AdoDataConnection connection = NodeHost.CreateDbConnection())
            {
                TableOperations<DataFile> fileGroupTable = new TableOperations<DataFile>(connection);
                DataFile file = fileGroupTable.QueryRecordWhere("ID = {0}", fileID);
                
                if (file is null)
                    return await NotFound().ExecuteAsync(default);

                file.FileBlob = new TableOperations<FileBlob>(connection).QueryRecordWhere("DataFileID = {0}", file.ID);

                byte[] data;
                if (!(file.FileBlob is null))
                    data = file.FileBlob.Blob;
                else if (File.Exists(file.FilePath))
                    data = File.ReadAllBytes(file.FilePath);
                else
                    return await NotFound().ExecuteAsync(default);

                string fileName = file.FilePath.Substring(file.FilePath.LastIndexOf("\\") + 1);
               
                var result = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ByteArrayContent(data)
                };

                result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                {
                    FileName = fileName
                };

                return result;
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

        private async Task NotifyNodes(Type nodeType, string action, NameValueCollection queryParameters = null)
        {
            async Task NotifyAsync(string url)
            {
                void ConfigureRequest(HttpRequestMessage request)
                {
                    request.Method = HttpMethod.Post;
                    request.RequestUri = new Uri(url);
                }

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
                        string url = NodeHost.BuildURL(nodeID, action, queryParameters);
                        return NotifyAsync(url);
                    })
                    .ToList();

                await Task.WhenAll(notifyTasks);
            }
        }

        private async Task ReprocessFileGroup(int id)
        {
            Type fileProcessorType = typeof(FileProcessorNode);

            await NotifyNodes(fileProcessorType, "Reprocess", new NameValueCollection
            {
                { "fileGroupID", id.ToString() }
            });
        }

        #endregion

        #region [ Static ]

        // Static Fields
        private static readonly ILog Log = LogManager.GetLogger(typeof(DataFileController));

        #endregion
    }
}