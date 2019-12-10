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

using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using GSF.Data;
using GSF.Data.Model;
using openXDA.Model;

namespace openXDA.Controllers.Config
{
    [RoutePrefix("api/Workbench/DataFiles")]
    public class DataFileController : ApiController
    {
        [Route("Count"), HttpPost]
        public Task<int> GetCount([FromBody]GetDataFilesPostData postData, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
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
            }, cancellationToken);
        }

        public class GetDataFilesPostData {
            public string sortField { get; set; }
            public bool ascending { get; set; }
            public int page { get; set; }
            public int pageSize { get; set; }
            public string filterString { get; set; }
        }

        [Route(""), HttpPost]
        public Task<DataTable> GetDataFiles([FromBody] GetDataFilesPostData postData, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {

                using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
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
            }, cancellationToken);

        }


    }


}