﻿//******************************************************************************************************
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

using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using GSF.Data.Model;
using GSF.Web;
using GSF.Web.Hosting;
using GSF.Web.Model;
using log4net.Core;
using openXDA.Model;

namespace openXDA
{
    /// <summary>
    /// Handles uploaded files.
    /// </summary>
    public class AlarmRangeLimitCSVUploadHandler : IHostedHttpHandler
    {
        public bool UseClientCache
        {
            get
            {
                return false;
            }
        }

        public long GetContentHash(HttpRequestMessage request)
        {
            throw new NotImplementedException();
        }

        ///// <summary>
        ///// Gets a value indicating whether another request can use the <see cref="IHttpHandler"/> instance.
        ///// </summary>
        ///// <returns>
        ///// <c>true</c> if the <see cref="IHttpHandler"/> instance is reusable; otherwise, <c>false</c>.
        ///// </returns>

        public async Task ProcessRequestAsync(HttpRequestMessage request, HttpResponseMessage response, CancellationToken cancellationToken)
        {
            await request.GetPostDataAsync(cancellationToken).ContinueWith(async postDataTask =>
            {
                await Task.WhenAll(postDataTask.Result.FileData.Select(ProcessFileAsync));
            });
            response.Headers.Location = request.Headers.Referrer;
            response.StatusCode = HttpStatusCode.Moved;
        }

        private async Task ProcessFileAsync(HttpContent file)
        {
            string csvFileData = await file.ReadAsStringAsync();

            try
            {
                string[] csvRows = csvFileData.Split('\n');
                string[] tableFields = csvRows[0].Split(',');

                using (DataContext dataContext = new DataContext())
                {
                    TableOperations<AlarmRangeLimit> table = dataContext.Table<AlarmRangeLimit>();

                    for (int i = 1; i < csvRows.Length; ++i)
                    {
                        string[] row = csvRows[i].Split(',');
                        AlarmRangeLimit newRecord = new AlarmRangeLimit();
                        newRecord.ID = int.Parse(row[0]);
                        newRecord.ChannelID = int.Parse(row[1]);
                        newRecord.Severity = int.Parse(row[4]);
                        newRecord.High = double.Parse(row[5]);
                        newRecord.Low = double.Parse(row[6]);
                        newRecord.RangeInclusive = bool.Parse(row[7]);
                        newRecord.PerUnit = bool.Parse(row[8]);
                        newRecord.Enabled = bool.Parse(row[9]);
                        newRecord.IsDefault = bool.Parse(row[17]);

                        table.UpdateRecord(newRecord);
                    }

                }
            }
            catch (LogException ex)
            {
                Debug.WriteLine(ex.Message);
            }

        }
    }
}