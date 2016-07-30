//******************************************************************************************************
//  FileUploadHandler.ashx.cs - Gbtc
//
//  Copyright © 2016, Grid Protection Alliance.  All Rights Reserved.
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
//  04/07/2016 - J. Ritchie Carroll
//       Generated original version of source code.
//
//******************************************************************************************************

using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using GSF.Data.Model;
using GSF.Web;
using GSF.Web.Hosting;
using GSF.Web.Model;
using openXDA.Model;

namespace openXDA
{
    /// <summary>
    /// Handles uploaded files.
    /// </summary>
    public class AlarmRangeLimitCSVUploadHandler : IHostedHttpHandler
    {
        ///// <summary>
        ///// Gets a value indicating whether another request can use the <see cref="IHttpHandler"/> instance.
        ///// </summary>
        ///// <returns>
        ///// <c>true</c> if the <see cref="IHttpHandler"/> instance is reusable; otherwise, <c>false</c>.
        ///// </returns>
        //public bool IsReusable => false;

        ///// <summary>
        ///// Enables processing of HTTP Web requests by a custom HttpHandler that implements the <see cref="IHttpHandler"/> interface.
        ///// </summary>
        ///// <param name="context">An <see cref="HttpContext"/> object that provides references to the intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests. </param>
        //public void ProcessRequest(HttpContext context)
        //{
        //    using (DataContext dataContext = new DataContext())
        //    {
        //        HttpPostedFile file = context.Request.Files[0];
        //        byte[] documentBlob = file.InputStream.ReadStream();
        //        string csv = System.Text.Encoding.UTF8.GetString(documentBlob);
        //        string[] csvRows = csv.Split('\n');
        //        string[] tableFields = csvRows[0].Split(',');

        //        TableOperations<AlarmRangeLimit> table = dataContext.Table<AlarmRangeLimit>();
        //        if(table.GetFieldNames() == tableFields)
        //        {
        //            for (int i = 1; i < csvRows.Length; ++i)
        //            {
        //                string[] row = csvRows[i].Split(',');
        //                AlarmRangeLimit newRecord = new AlarmRangeLimit();
        //                newRecord = dataContext.Connection.ExecuteScalar<AlarmRangeLimit>("Select * FROM AlarmRangeLimit WHERE ID ={0}", row[0]);
        //                newRecord.Severity = int.Parse(row[4]);
        //                newRecord.High = float.Parse(row[5]);
        //                newRecord.Low = float.Parse(row[6]);
        //                newRecord.RangeInclusive = int.Parse(row[7]);
        //                newRecord.PerUnit = int.Parse(row[8]);
        //                newRecord.Enabled = int.Parse(row[9]);
        //                newRecord.IsDefault = bool.Parse(row[17]);

        //                table.UpdateRecord(newRecord);
        //            }
        //        }
        //    }

        //}

        public async Task ProcessRequestAsync(HttpRequestMessage request, HttpResponseMessage response)
        {
            await request.GetPostDataAsync().ContinueWith(async postDataTask =>
            {
                await Task.WhenAll(postDataTask.Result.FileData.Select(ProcessFileAsync));
            });
        }

        private async Task ProcessFileAsync(HttpContent file)
        {
            string csvFileData = await file.ReadAsStringAsync();
            string[] csvRows = csvFileData.Split('\n');
            string[] tableFields = csvRows[0].Split(',');

            using (DataContext dataContext = new DataContext())
            {
                TableOperations<AlarmRangeLimit> table = dataContext.Table<AlarmRangeLimit>();
                if (table.GetFieldNames() == tableFields)
                {
                    for (int i = 1; i < csvRows.Length; ++i)
                    {
                        string[] row = csvRows[i].Split(',');
                        AlarmRangeLimit newRecord = new AlarmRangeLimit();
                        newRecord = dataContext.Connection.ExecuteScalar<AlarmRangeLimit>("Select * FROM AlarmRangeLimit WHERE ID ={0}", row[0]);
                        newRecord.Severity = int.Parse(row[4]);
                        newRecord.High = float.Parse(row[5]);
                        newRecord.Low = float.Parse(row[6]);
                        newRecord.RangeInclusive = int.Parse(row[7]);
                        newRecord.PerUnit = int.Parse(row[8]);
                        newRecord.Enabled = int.Parse(row[9]);
                        newRecord.IsDefault = bool.Parse(row[17]);

                        table.UpdateRecord(newRecord);
                    }
                }
            }

        }
    }
}