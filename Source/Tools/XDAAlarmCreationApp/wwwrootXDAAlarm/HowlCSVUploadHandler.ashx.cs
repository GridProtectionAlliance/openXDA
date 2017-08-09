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
//  08/3/2017 - Stephen A. Jenks
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
using XDAAlarmCreationApp.Model;
using System.Collections.Specialized;

namespace XDAAlarmCreationApp
{
    /// <summary>
    /// Handles uploaded files.
    /// </summary>
    public class HowlCSVUploadHandler : IHostedHttpHandler
    {

        #region [Properties]

        /// <summary>
        /// Determines if client cache should be enabled for rendered handler content.
        /// </summary>
        /// <remarks>
        /// If rendered handler content does not change often, the server and client will use the
        /// <see cref="IHostedHttpHandler.GetContentHash"/> to determine if the client needs to refresh the content.
        /// </remarks>
        public bool UseClientCache => false;

        /// <summary>
        /// Gets a value indicating whether another request can use the <see cref="IHttpHandler"/> instance.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the <see cref="IHttpHandler"/> instance is reusable; otherwise, <c>false</c>.
        /// </returns>
        public bool IsReusable => false;

        #endregion

        #region [Methods]

        /// <summary>
        /// Gets hash of response content based on any <paramref name="request"/> parameters.
        /// </summary>
        /// <param name="request">HTTP request message.</param>
        /// <remarks>
        /// Value is only used when <see cref="IHostedHttpHandler.UseClientCache"/> is <c>true</c>.
        /// </remarks>
        public long GetContentHash(HttpRequestMessage request) => 0;

        public async Task ProcessRequestAsync(HttpRequestMessage request, HttpResponseMessage response, CancellationToken cancellationToken)
        {
            string referrer = request.Headers.Referrer.ToString();
            await request.GetPostDataAsync().ContinueWith(async postDataTask =>
            {
                await Task.WhenAll(postDataTask.Result.FileData.Select(file => ProcessFileAsync(file, referrer)));
            });

            response.Headers.Location = request.Headers.Referrer;
            response.StatusCode = HttpStatusCode.Moved;
        }

        private async Task ProcessFileAsync(HttpContent file, string referrer)
        {
            string csvFileData = await file.ReadAsStringAsync();

            try
            {
                string[] csvRows = csvFileData.Split('\n');
                string[] tableFields = csvRows[0].Split(',');

                for (int i = 0; i < tableFields.Length; i++)
                {
                    tableFields[i] = tableFields[i].Trim();
                    tableFields[i] = tableFields[i].Trim(new char[] { '[', ']' });
                }

                if (referrer.Contains("ChannelsWithHourlyLimits.cshtml") || referrer.Contains("MetersWithHourlyLimits.cshtml") || referrer.Contains("HourOfWeekLimits.cshtml"))
                {
                    int[] fieldIndexes = new int[8];

                    fieldIndexes[0] = Array.IndexOf(tableFields, "AlarmID");
                    fieldIndexes[1] = Array.IndexOf(tableFields, "AlarmChannelID");
                    fieldIndexes[2] = Array.IndexOf(tableFields, "AlarmAlarmTypeID");
                    fieldIndexes[3] = Array.IndexOf(tableFields, "AlarmHourOfWeek");
                    fieldIndexes[4] = Array.IndexOf(tableFields, "AlarmSeverity");
                    fieldIndexes[5] = Array.IndexOf(tableFields, "AlarmHigh");
                    fieldIndexes[6] = Array.IndexOf(tableFields, "AlarmLow");
                    fieldIndexes[7] = Array.IndexOf(tableFields, "AlarmEnabled");

                    if (!fieldIndexes.Any(n => n < 0)) // Check if any indexes are negative (missing)
                    {
                        using (DataContext dataContext = new DataContext())
                        {
                            TableOperations<HourOfWeekLimit> table = dataContext.Table<HourOfWeekLimit>();

                            for (int i = 1; i < csvRows.Length; ++i)
                            {
                                string[] row = csvRows[i].Split(',');
                                HourOfWeekLimit newRecord = new HourOfWeekLimit()
                                {
                                    ID = int.Parse(row[fieldIndexes[0]]),
                                    ChannelID = int.Parse(row[fieldIndexes[1]]),
                                    AlarmTypeID = int.Parse(row[fieldIndexes[2]]),
                                    HourOfWeek = int.Parse(row[fieldIndexes[3]]),
                                    Severity = int.Parse(row[fieldIndexes[4]]),
                                    High = float.Parse(row[fieldIndexes[5]]),
                                    Low = float.Parse(row[fieldIndexes[6]]),
                                    Enabled = int.Parse(row[fieldIndexes[7]])
                                };

                                table.UpdateRecord(newRecord);

                            }
                        }
                    }
                }
                else if(referrer.Contains("ChannelsWithLimits.cshtml") || referrer.Contains("MetersWithNormalLimits.cshtml"))
                {
                    int channelIdIndex = Array.IndexOf(tableFields, "ChannelID");
                    int highIndex = Array.IndexOf(tableFields, "ChannelHigh");
                    int lowIndex = Array.IndexOf(tableFields, "ChannelLow");

                    using (DataContext dataContext = new DataContext())
                    {
                        TableOperations<AlarmRangeLimit> table = dataContext.Table<AlarmRangeLimit>();

                        for (int i = 1; i < csvRows.Length; ++i)
                        {
                            string[] row = csvRows[i].Split(',');
                            AlarmRangeLimit record = table.QueryRecordWhere("ChannelID = {0}", int.Parse(row[channelIdIndex]));

                            if (record == null) continue;
                            record.High = float.Parse(row[highIndex]);
                            record.Low = float.Parse(row[lowIndex]);

                            table.UpdateRecord(record);

                        }
                    }

                }
            }
            catch (Exception e)
            {
                LogExceptionHandler?.Invoke(e);
                throw;
            }
        }

        public static Action<Exception> LogExceptionHandler;

        #endregion
    }
}