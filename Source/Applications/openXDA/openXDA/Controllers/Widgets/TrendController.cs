//******************************************************************************************************
//  TrendController.cs - Gbtc
//
//  Copyright � 2020, Grid Protection Alliance.  All Rights Reserved.
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
//  10/28/2025 - G. Santos
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using GSF.Configuration;
using GSF.Data;
using GSF.Data.Model;
using HIDS;
using openXDA.HIDS;
using openXDA.HIDS.APIExtensions;
using openXDA.Model;
using ConfigurationLoader = openXDA.Nodes.ConfigurationLoader;

namespace openXDA.Controllers.Widgets
{
    /// <summary>
    /// Controller that handles fetching HIDS trending data for an openXDA <see cref="Event"/>.
    /// </summary> 
    [RoutePrefix("api/Widgets/Trending")]
    public class TrendController : ApiController
    {
        /// <summary>
        /// Defines a trend post data object as an extension of an event post object.
        /// </summary>
        public class TrendPostData : EventPost
        {
            /// <summary>
            /// Hours before an event to pull data from.
            /// </summary>
            public int HoursBefore { get; set; }
            /// <summary>
            /// Hours after an event to pull data from.
            /// </summary>
            public int HoursAfter { get; set; }
        }

        private class Settings
        {
            [Category]
            [SettingName(HIDSSettings.CategoryName)]
            public HIDSSettings HIDSSettings { get; }
                = new HIDSSettings();
        }

        Func<AdoDataConnection> m_connectionFactory;

        /// <summary>
        /// Constructor to pull  a connection factory from the XDA controller activator.
        /// </summary> 
        public TrendController(Func<AdoDataConnection> connectionFactory)
        {
            m_connectionFactory = connectionFactory;
        }

        /// <summary>
        /// Searches <see cref="ChannelDetail"/> records for trend channels with an RMS <see cref="MeasurementCharacteristic"/>,
        /// line to neutral <see cref="Phase"/>, and part of the <see cref="Meter"/> associated with the <see cref="Event"/> provided.
        /// </summary>
        /// <param name="eventID">ID of the <see cref="Event"/>.</param>
        [Route("TrendChannels"), HttpPost]
        public virtual IHttpActionResult TrendChannels([FromBody] EventPost postData)
        {
            using (AdoDataConnection connection = m_connectionFactory())
            {
                if (!postData.IsCustomerAuthorized(connection))
                    return Unauthorized();

                return Ok(GetTrendChannelsByEvent(postData, connection));
            }
        }

        /// <summary>
        /// Searches <see cref="ChannelDetail"/> records for trend channels with an RMS <see cref="MeasurementCharacteristic"/>,
        /// line to neutral <see cref="Phase"/>, and part of the <see cref="Meter"/> associated with the <see cref="Event"/> provided.
        /// Will then query HIDS using these channels and the time window provided by <see cref="TrendPostData"/>.
        /// </summary>
        /// <param name="postData">Post data object containing query data.</param>
        /// <param name="token">Token to cancel the request.</param>
        [Route("QueryPoints"), HttpPost]
        public async virtual Task<HttpResponseMessage> QueryPoints([FromBody] TrendPostData postData, CancellationToken token)
        {
            IEnumerable<ChannelDetail> channels;
            Event evt;
            using (AdoDataConnection connection = m_connectionFactory())
            {
                if (!postData.IsCustomerAuthorized(connection))
                    return await Unauthorized().ExecuteAsync(token).ConfigureAwait(false);

                channels = GetTrendChannelsByEvent(postData, connection);
                evt = new TableOperations<Event>(connection).QueryRecordWhere("ID={0}", postData.EventID);
            }

            if (evt is null)
            {
                BadRequestErrorMessageResult result = BadRequest("Unable to find event.");
                return await result.ExecuteAsync(token);
            }

            void BuildQuery(IQueryBuilder builder)
            {
                IEnumerable<string> tags = channels
                    .Select(channel => APIExtensions.ToTag(null, channel.ID));

                builder.FilterTags(tags);

                string startTime = API.FormatTimestamp(evt.StartTime.AddHours(-postData.HoursBefore));
                string stopTime = API.FormatTimestamp(evt.StartTime.AddHours(postData.HoursAfter));

                builder.Range(startTime, stopTime);
            }

            if (!channels.Any())
                return await Ok("").ExecuteAsync(token).ConfigureAwait(false);

            MediaTypeHeaderValue contentType = new MediaTypeHeaderValue("text/plain");
            contentType.CharSet = "utf-8";

            Stream pointStream = PointStream.QueryPoints(CreateHIDSConnectionAsync, BuildQuery);
            StreamContent content = new StreamContent(pointStream);
            content.Headers.ContentType = contentType;

            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = content;
            return response;
        }

        private IEnumerable<ChannelDetail> GetTrendChannelsByEvent(EventPost postData, AdoDataConnection connection)
        {
            RecordRestriction baseRestriction = new RecordRestriction(@"
	            Trend = 1 AND
	            MeasurementCharacteristic = 'RMS' AND
	            Phase like '%N' AND
	            MeterID IN (SELECT MeterID FROM [Event] WHERE [Event].ID = {0})
            ", postData.EventID);

            RecordRestriction customerRestriction = postData.GetCustomerRestrictionOnChannels();

            return new TableOperations<ChannelDetail>(connection).QueryRecords(baseRestriction+customerRestriction);
        }

        private async Task<API> CreateHIDSConnectionAsync()
        {
            ConfigurationLoader configurationLoader = new ConfigurationLoader(m_connectionFactory);
            Settings settings = new Settings();
            configurationLoader.Configure(settings);

            API hids = new API();
            await hids.ConfigureAsync(settings.HIDSSettings);
            return hids;
        }
    }
}
