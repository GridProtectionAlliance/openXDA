//******************************************************************************************************
//  HowlCSVDownloadHandler.ashx.cs - Gbtc
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
//  08/02/2017 - Stephen A. Jenks
//       Generated original version of source code.
//
//******************************************************************************************************

using System.Web;
using System;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using GSF.Data.Model;
using GSF.Web;
using GSF.Web.Hosting;
using GSF.Web.Model;
using XDAAlarmCreationApp.Model;
using GSF.Threading;
using CancellationToken = System.Threading.CancellationToken;
using GSF.Data;
using System.Collections.Specialized;
using System.IO;
using GSF;

namespace XDAAlarmCreationApp
{
    /// <summary>
    /// Handles Exporting CSV files from the MetersWithHourlyLimits, ChannelsWithHourlyLimits and Hour
    /// </summary>
    public class HowlCSVDownloadHandler : IHostedHttpHandler
    {
        #region [Members]

        // Nested Types
        private class HttpResponseCancellationToken : CompatibleCancellationToken
        {
            private readonly HttpResponse m_reponse;

            public HttpResponseCancellationToken(HttpResponse response) : base(CancellationToken.None)
            {
                m_reponse = response;
            }

            public override bool IsCancelled => !m_reponse.IsClientConnected;
        }

        const string CsvContentType = "text/csv";

        #endregion

        #region [Properties]

        /// <summary>
        /// Gets a value indicating whether another request can use the <see cref="IHttpHandler"/> instance.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the <see cref="IHttpHandler"/> instance is reusable; otherwise, <c>false</c>.
        /// </returns>
        public bool IsReusable => false;

        /// <summary>
        /// Determines if client cache should be enabled for rendered handler content.
        /// </summary>
        /// <remarks>
        /// If rendered handler content does not change often, the server and client will use the
        /// <see cref="IHostedHttpHandler.GetContentHash"/> to determine if the client needs to refresh the content.
        /// </remarks>
        public bool UseClientCache => false;

        public string Filename => "HourOfWeekAlarmLimitsExport.csv";

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

        public void ProcessRequest(HttpContext context)
        {
            HttpResponse response = HttpContext.Current.Response;
            HttpResponseCancellationToken cancellationToken = new HttpResponseCancellationToken(response);
            NameValueCollection requestParameters = context.Request.QueryString;

            response.ClearContent();
            response.Clear();
            response.AddHeader("Content-Type", CsvContentType);
            response.AddHeader("Content-Disposition", "attachment;filename=" + Filename);
            response.BufferOutput = true;

            try
            {
                WriteTableToStream(requestParameters, response.OutputStream, response.Flush, cancellationToken);
            }

            catch (Exception e)
            {
                LogExceptionHandler?.Invoke(e);
                throw;
            }
            finally
            {
                response.End();
            }
        }

        public Task ProcessRequestAsync(HttpRequestMessage request, HttpResponseMessage response, CancellationToken cancellationToken)
        {
            NameValueCollection requestParameters = request.RequestUri.ParseQueryString();

            response.Content = new PushStreamContent((stream, content, context) =>
            {
                try
                {
                    WriteTableToStream(requestParameters, stream, null, cancellationToken);
                }
                catch (Exception e)
                {
                    LogExceptionHandler?.Invoke(e);
                    throw;
                }
                finally
                {
                    stream.Close();
                }
            },
            new MediaTypeHeaderValue(CsvContentType));

            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = Filename
            };

#if MONO
            return Task.FromResult(false);
#else
            return Task.CompletedTask;
#endif
        }

        public void WriteTableToStream(NameValueCollection requestParameters, Stream responseStream, Action flushResponse, CompatibleCancellationToken cancellationToken)
        {
            string modelName = requestParameters["ModelName"];
            string hubName = requestParameters["HubName"];
            string connectionID = requestParameters["ConnectionID"];
            string callingFrom = requestParameters["CallingFrom"];
            int ID = int.Parse(requestParameters["ID"]);
            string filterText = requestParameters["FilterText"];
            string sortField = requestParameters["SortField"];
            bool sortAscending = requestParameters["SortAscending"].ParseBoolean();
            bool showDeleted = requestParameters["ShowDeleted"].ParseBoolean();

            using (StreamWriter writer = new StreamWriter(responseStream))
            {
                writer.WriteLine(GetHourOfWeekLimitCSVHeaders());
            }

            if (callingFrom == "Alarms")
                SendHourOfWeekLimitTableToCSV(responseStream, ID, filterText, cancellationToken, sortField, sortAscending);
            else if (callingFrom == "Channels")
                SendChannelsWithHourlyLimitsToCSV(responseStream, ID, filterText, cancellationToken, sortField, sortAscending);
            else
                SendMetersWithHourlyLimitsToCSV(responseStream, filterText, cancellationToken, sortField, sortAscending);
        }

        public void SendMetersWithHourlyLimitsToCSV(Stream returnStream, string searchString, CompatibleCancellationToken cancellationToken, string sortField = "Name", bool ascending = true)
        {
            List<MetersWithHourlyLimits> meters;
            using (var connection = new AdoDataConnection("systemSettings"))
            {
                TableOperations<MetersWithHourlyLimits> meterTable = new TableOperations<MetersWithHourlyLimits>(connection);
                RecordRestriction restriction = meterTable.GetSearchRestriction(searchString);
                string sortExpression = sortField != null ? sortField + (ascending == true ? " ASC" : " DESC") : null;

                meters = meterTable.QueryRecords(sortExpression, restriction).ToList();
            }

            foreach (MetersWithHourlyLimits meter in meters)
            {
                if (cancellationToken.IsCancelled)
                    return;

                SendChannelsWithHourlyLimitsToCSV(returnStream, meter.ID, "", cancellationToken);
            }
                
        }

        public void SendChannelsWithHourlyLimitsToCSV(Stream returnStream, int meterID, string searchString, CompatibleCancellationToken cancellationToken, string sortField = "Name", bool ascending = true)
        {
            MetersWithHourlyLimits meter;
            List<ChannelsWithHourlyLimits> channels;

            using (var connection = new AdoDataConnection("systemSettings"))
            {
                TableOperations<MetersWithHourlyLimits> meterTable = new TableOperations<MetersWithHourlyLimits>(connection);
                TableOperations<ChannelsWithHourlyLimits> channelTable = new TableOperations<ChannelsWithHourlyLimits>(connection);
                meter = meterTable.QueryRecordWhere("ID = {0}", meterID);

                string sortExpression = sortField != null ? sortField + (ascending == true ? " ASC" : " DESC") : null;

                RecordRestriction searchRestriction = channelTable.GetSearchRestriction(searchString);
                RecordRestriction meterRestriction = new RecordRestriction("MeterID = {0}", meterID);
                RecordRestriction finalRestriction = searchRestriction + meterRestriction;

                channels = channelTable.QueryRecords(sortExpression, finalRestriction).ToList();
            }

            foreach (var channel in channels)
            {
                if (cancellationToken.IsCancelled)
                    return;

                SendHourOfWeekLimitTableToCSV(returnStream, channel.ID, "", cancellationToken);
            }
                
        }

        public void SendHourOfWeekLimitTableToCSV(Stream returnStream, int channelID, string searchString, CompatibleCancellationToken cancellationToken, string sortField = "HourOfWeek", bool ascending=true)
        {
            MetersWithHourlyLimits meter;
            ChannelsWithHourlyLimits channel;
            List<HourOfWeekLimitView> hourlyLimits = new List<HourOfWeekLimitView>();
            using (var connection = new AdoDataConnection("systemSettings"))
            {
                TableOperations<MetersWithHourlyLimits> meterTable = new TableOperations<MetersWithHourlyLimits>(connection);
                TableOperations<ChannelsWithHourlyLimits> channelTable = new TableOperations<ChannelsWithHourlyLimits>(connection);
                TableOperations<HourOfWeekLimitView> table = new TableOperations<HourOfWeekLimitView>(connection);

                channel = channelTable.QueryRecordWhere("ID = {0}", channelID);
                meter = meterTable.QueryRecordWhere("ID = {0}", channel.MeterID);

                RecordRestriction searchRestriction = table.GetSearchRestriction(searchString);
                RecordRestriction channelRestriction = new RecordRestriction("ChannelID = {0}", channel.ID);
                RecordRestriction finalRestriction = searchRestriction + channelRestriction;

                string sortExpression = sortField != null ? sortField + (ascending == true ? " ASC" : " DESC") : null;

                hourlyLimits = table.QueryRecords(sortExpression, finalRestriction).ToList();
            }

            string meterInfo = meter.ToCSV();
            string channelInfo = channel.ToCSV();

            using (StreamWriter writer = new StreamWriter(returnStream))
                foreach (HourOfWeekLimitView limit in hourlyLimits)
                    writer.WriteLine(meterInfo + "," + channelInfo + "," + limit.ToCSV());
        }

        /// <summary>
        /// Gets the field names for the MetersWithAlarmLimits, ChannelsWithAlarmLimits, and HourOfWeekAlarmLimits tables
        /// and concatenates them together.
        /// </summary>
        /// <returns></returns>
        private string GetHourOfWeekLimitCSVHeaders()
        {
            string csv = "";

            string[] meterHeaders;
            string[] channelHeaders;
            string[] alarmHeaders;

            using (var connection = new AdoDataConnection("systemSettings"))
            {
                TableOperations<MetersWithHourlyLimits> meterTable = new TableOperations<MetersWithHourlyLimits>(connection);
                TableOperations<ChannelsWithHourlyLimits> channelTable = new TableOperations<ChannelsWithHourlyLimits>(connection);
                TableOperations<HourOfWeekLimit> alarmTable = new TableOperations<HourOfWeekLimit>(connection);

                meterHeaders = meterTable.GetFieldNames();
                channelHeaders = channelTable.GetFieldNames();
                alarmHeaders = alarmTable.GetFieldNames();
            }

            for (int i = 0; i < meterHeaders.Length; i++)
                meterHeaders[i] = "Meter" + meterHeaders[i];

            for (int i = 0; i < channelHeaders.Length; i++)
                channelHeaders[i] = "Channel" + channelHeaders[i];

            for (int i = 0; i < alarmHeaders.Length; i++)
                alarmHeaders[i] = "Alarm" + alarmHeaders[i];

            string[] headers = meterHeaders.Concat(channelHeaders).ToArray().Concat(alarmHeaders).ToArray();

            if (headers.Length > 0)
                csv += "[" + headers[0] + "]";

            for (int i = 1; i < headers.Length; i++)
                csv += ",[" + headers[i] + "]";

            return csv;
        }

        public static Action<Exception> LogExceptionHandler;

#endregion
    }
}