//******************************************************************************************************
//  FileDownloadHandler.ashx.cs - Gbtc
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

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading;
using System.Web;
using GSF.Collections;
using GSF.Data;
using GSF.Data.Model;
using GSF.Security;
using GSF.Web.Model;
using openXDA.Model;

namespace openXDA
{
    /// <summary>
    /// Handles downloading files.
    /// </summary>
    public class FileDownloadHandler : IHttpHandler
    {
        /// <summary>
        /// Gets a value indicating whether another request can use the <see cref="IHttpHandler"/> instance.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the <see cref="IHttpHandler"/> instance is reusable; otherwise, <c>false</c>.
        /// </returns>
        public bool IsReusable => false;

        /// <summary>
        /// Enables processing of HTTP Web requests by a custom HttpHandler that implements the <see cref="IHttpHandler" /> interface.
        /// </summary>
        /// <param name="context">An <see cref="HttpContext" /> object that provides references to the intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests. </param>
        public void ProcessRequest(HttpContext context)
        {
            HttpResponse response = HttpContext.Current.Response;
            response.ClearContent();
            response.Clear();

            SecurityProviderCache.ValidateCurrentProvider();
            NameValueCollection parameters = context.Request.QueryString;
            string modelName = parameters["ModelName"]; // If provided, must include namespace
               
            //Type associatedModel = Type.GetType(modelName);

            using (DataContext dataContext = new DataContext())
            {
                using (DataTable table = dataContext.Connection.RetrieveData("Select * FROM {0}", modelName))
                {
                    StringBuilder sb = new StringBuilder();

                    IEnumerable<string> columnNames = table.Columns.Cast<DataColumn>().
                                                      Select(column => column.ColumnName);
                    sb.AppendLine(string.Join(",", columnNames));

                    foreach (DataRow row in table.Rows)
                    {
                        IEnumerable<string> fields = row.ItemArray.Select(field => field.ToString());
                        sb.AppendLine(string.Join(",", fields));
                    }


                    //Download the CSV file.
                    response.Clear();
                    response.Buffer = true;
                    response.AddHeader("content-disposition", "attachment;filename=SqlExport.csv");
                    response.Charset = "";
                    response.ContentType = "application/text";
                    response.Output.Write(sb.ToString());
                    response.Flush();
                    response.End();
                

                }
            }

            response.End();
        }
    }
}