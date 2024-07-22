//******************************************************************************************************
//  HIDSController.cs - Gbtc
//
//  Copyright © 2022, Grid Protection Alliance.  All Rights Reserved.
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
//  06/01/2022 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using GSF.Configuration;
using GSF.Data;
using GSF.Web.Security;
using HIDS;
using Newtonsoft.Json.Linq;
using openXDA.HIDS;
using openXDA.HIDS.APIExtensions;
using openXDA.Nodes;

namespace openXDA.Adapters
{
    public class HIDSPost
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string By { get; set; }
        public List<int> IDs { get; set; }
        public List<int> Phases { get; set; }
        public List<int> Groups { get; set; }
        public List<int> Types { get; set; }
        public ulong Hours { get; set; }
        public ulong Days { get; set; }
        public ulong Weeks { get; set; }
        public ulong Months { get; set; }
        public string Aggregate { get; set; }
    }

    public class HIDSController : ApiController
    {
        public class HistogramMetadata
        {
            public int ChannelID { get; set; }
            public int FundamentalFrequency { get; set; }
            public int SamplingRate { get; set; }
            public DateTime StartTime { get; set; }
            public DateTime EndTime { get; set; }
            public int TotalCapturedCycles { get; set; }
            public double CyclesMax { get; set; }
            public double CyclesMin { get; set; }
            public double ResidualMax { get; set; }
            public double ResidualMin { get; set; }
            public double FrequencyMax { get; set; }
            public double FrequencyMin { get; set; }
            public double RMSMax { get; set; }
            public double RMSMin { get; set; }
            public int CyclicHistogramBins { get; set; }
            public int ResidualHistogramBins { get; set; }
            public int FrequencyHistogramBins { get; set; }
            public int RMSHistogramBins { get; set; }
        }

        private class Settings
        {
            [Category]
            [SettingName(HIDSSettings.CategoryName)]
            public HIDSSettings HIDSSettings { get; }
                = new HIDSSettings();
        }

        private Host Host { get; }

        public HIDSController(Host host) =>
            Host = host;

        [HttpPost]
        [ValidateRequestVerificationToken]
        public async Task<HttpResponseMessage> QueryPoints([FromBody] JObject query, CancellationToken cancellationToken)
        {
            void BuildQuery(IQueryBuilder builder)
            {
                if (query["Channels"] is JArray channels)
                {
                    IEnumerable<string> tags = channels
                        .ToObject<List<int>>()
                        .Select(channelID => APIExtensions.ToTag(null, channelID));

                    builder.FilterTags(tags);
                }

                string startTime = ReadDate(query, "StartTime");
                string stopTime = ReadDate(query, "StopTime");

                if (!string.IsNullOrEmpty(stopTime))
                    builder.Range(startTime, stopTime);
                else
                    builder.Range(startTime);

                if (query.ContainsKey("InvalidFlags"))
                {
                    uint invalidFlags = query.Value<uint>("InvalidFlags");
                    builder.TestQuality(invalidFlags);
                }

                if (query.ContainsKey("AggregateDuration"))
                {
                    string aggregateDuration = query.Value<string>("AggregateDuration");
                    builder.Aggregate(aggregateDuration);
                }

                if (query["TimeFilters"] is JArray timeFilters)
                {
                    List<TimeFilter> filters = timeFilters
                        .ToObject<List<TimeFilter>>();

                    builder.FilterTime(filters);
                }
            }

            if (!query.ContainsKey("StartTime"))
            {
                BadRequestErrorMessageResult result = BadRequest("Missing query parameter: StartTime");
                return await result.ExecuteAsync(cancellationToken);
            }

            MediaTypeHeaderValue contentType = new MediaTypeHeaderValue("text/plain");
            contentType.CharSet = "utf-8";

            Stream pointStream = PointStream.QueryPoints(CreateHIDSConnectionAsync, BuildQuery);
            StreamContent content = new StreamContent(pointStream);
            content.Headers.ContentType = contentType;

            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = content;
            return response;
        }

        [HttpPost]
        [ValidateRequestVerificationToken]
        public async Task<HttpResponseMessage> QueryPointsByTimeSpans([FromBody] JObject query, CancellationToken cancellationToken)
        {
            if (!query.ContainsKey("TimeSpans"))
            {
                BadRequestErrorMessageResult result = BadRequest("Missing query parameter: TimeSpans");
                return await result.ExecuteAsync(cancellationToken);
            }

            IEnumerable<GSF.Range<DateTime>> spans = query["TimeSpans"].ToObject<IEnumerable<JToken>>().Select((spanToken) =>
            {
                JArray bounds = (JArray)spanToken;

                return new GSF.Range<DateTime>(bounds[0].Value<DateTime>(), bounds[1].Value<DateTime>());
            });
            spans = GSF.Range<DateTime>.MergeAllOverlapping(spans).OrderBy(span => span.Start);
            if (spans.Count() == 0)
            {
                BadRequestErrorMessageResult result = BadRequest("No timespans specified in query parameter");
                return await result.ExecuteAsync(cancellationToken);
            }

            IAsyncEnumerable<Point> hidsCaller(API instance, CancellationToken token = default)
            {
                IEnumerable<string> tags = Enumerable.Empty<string>();
                if (query["Channels"] is JArray channels)
                {
                    tags = channels
                        .ToObject<List<int>>()
                        .Select(channelID => APIExtensions.ToTag(null, channelID));
                }

                uint invalidFlags = 0;
                if (query.ContainsKey("InvalidFlags"))
                {
                    invalidFlags = query.Value<uint>("InvalidFlags");
                }

                string aggregateDuration = "";
                if (query.ContainsKey("AggregateDuration"))
                {
                    aggregateDuration = query.Value<string>("AggregateDuration");
                }

                return spans
                    .ToAsyncEnumerable()
                    .SelectMany(span =>
                    {
                        void BuildQuery(IQueryBuilder builder)
                        {
                            builder.Range(span.Start, span.End);
                            if (tags.Count() != 0) builder.FilterTags(tags);
                            if (query.ContainsKey("InvalidFlags")) builder.TestQuality(invalidFlags);
                            if (query.ContainsKey("AggregateDuration")) builder.Aggregate(aggregateDuration);
                        }
                        return instance.ReadPointsAsync(BuildQuery, cancellationToken);
                    });
            }

            MediaTypeHeaderValue contentType = new MediaTypeHeaderValue("text/plain");
            contentType.CharSet = "utf-8";

            Stream pointStream = PointStream.QueryPoints(CreateHIDSConnectionAsync, hidsCaller);
            StreamContent content = new StreamContent(pointStream);
            content.Headers.ContentType = contentType;

            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = content;
            return response;
        }

        [HttpPost]
        [ValidateRequestVerificationToken]
        public async Task<List<HistogramMetadata>> QueryHistogramMetadata([FromBody] JObject query, CancellationToken cancellationToken)
        {
            using (API hids = await CreateHIDSConnectionAsync())
            {
                HistogramMetadata ToHistogramMetadata(Histogram.Metadata metadata) => new HistogramMetadata()
                {
                    ChannelID = hids.ToChannelID(metadata.Tag),
                    FundamentalFrequency = metadata.FundamentalFrequency,
                    SamplingRate = metadata.SamplingRate,
                    StartTime = metadata.StartTime,
                    EndTime = metadata.EndTime,
                    TotalCapturedCycles = metadata.TotalCapturedCycles,
                    CyclesMax = metadata.CyclesMax,
                    CyclesMin = metadata.CyclesMin,
                    ResidualMax = metadata.ResidualMax,
                    ResidualMin = metadata.ResidualMin,
                    FrequencyMax = metadata.FrequencyMax,
                    FrequencyMin = metadata.FrequencyMin,
                    RMSMax = metadata.RMSMax,
                    RMSMin = metadata.RMSMin,
                    CyclicHistogramBins = metadata.CyclicHistogramBins,
                    ResidualHistogramBins = metadata.ResidualHistogramBins,
                    FrequencyHistogramBins = metadata.FrequencyHistogramBins,
                    RMSHistogramBins = metadata.RMSHistogramBins
                };

                IEnumerable<string> tags = Enumerable.Empty<string>();
                string startTime = ReadDate(query, "StartTime");
                string stopTime = ReadDate(query, "StopTime");

                if (query["Channels"] is JArray channels)
                {
                    tags = channels
                        .ToObject<List<int>>()
                        .Select(hids.ToTag);
                }

                return await hids
                    .ReadHistogramMetadataAsync(tags, startTime, stopTime, cancellationToken)
                    .Select(ToHistogramMetadata)
                    .ToListAsync();
            }
        }

        [HttpPost]
        [ValidateRequestVerificationToken]
        public async Task<List<Histogram.Point>> QueryCyclicHistogramData([FromBody] JObject query, CancellationToken cancellationToken)
        {
            using (API hids = await CreateHIDSConnectionAsync())
            {
                int channelID = query.Value<int>("Channel");
                string tag = hids.ToTag(channelID);
                DateTime timestamp = query.Value<DateTime>("Timestamp");
                return await hids.ReadCyclicHistogramAsync(tag, timestamp, cancellationToken);
            }
        }

        [HttpPost]
        [ValidateRequestVerificationToken]
        public async Task<List<Histogram.Point>> QueryResidualHistogramData([FromBody] JObject query, CancellationToken cancellationToken)
        {
            using (API hids = await CreateHIDSConnectionAsync())
            {
                int channelID = query.Value<int>("Channel");
                string tag = hids.ToTag(channelID);
                DateTime timestamp = query.Value<DateTime>("Timestamp");
                return await hids.ReadResidualHistogramAsync(tag, timestamp, cancellationToken);
            }
        }

        [HttpPost]
        [ValidateRequestVerificationToken]
        public async Task<List<Histogram.Point>> QueryFrequencyHistogramData([FromBody] JObject query, CancellationToken cancellationToken)
        {
            using (API hids = await CreateHIDSConnectionAsync())
            {
                int channelID = query.Value<int>("Channel");
                string tag = hids.ToTag(channelID);
                DateTime timestamp = query.Value<DateTime>("Timestamp");
                return await hids.ReadFrequencyHistogramAsync(tag, timestamp, cancellationToken);
            }
        }

        [HttpPost]
        [ValidateRequestVerificationToken]
        public async Task<List<Histogram.Point>> QueryRMSHistogramData([FromBody] JObject query, CancellationToken cancellationToken)
        {
            using (API hids = await CreateHIDSConnectionAsync())
            {
                int channelID = query.Value<int>("Channel");
                string tag = hids.ToTag(channelID);
                DateTime timestamp = query.Value<DateTime>("Timestamp");
                return await hids.ReadRMSHistogramAsync(tag, timestamp, cancellationToken);
            }
        }

        [HttpPost]
        [ValidateRequestVerificationToken]
        public async Task<HttpResponseMessage> LegacyPost([FromBody] HIDSPost post, CancellationToken cancellationToken)
        {
            async Task<HttpResponseMessage> FailAsync(Exception ex)
            {
                BadRequestErrorMessageResult result = BadRequest(ex.Message);
                return await result.ExecuteAsync(cancellationToken);
            }

            DataTable table;
            try { table = GetTable(Host, post); }
            catch (ArgumentException ex) { return await FailAsync(ex); }

            async Task<List<Point>> QueryHIDSAsync()
            {
                using (API hids = await CreateHIDSConnectionAsync())
                {
                    IEnumerable<string> tags = table
                        .AsEnumerable()
                        .Select(row => row.ConvertField<int>("ID"))
                        .Select(hids.ToTag);

                    IEnumerable<TimeFilter> ReadTimeFilters(ulong flags, TimeFilter baseFilter, int size) => Enumerable
                        .Range(0, size)
                        .Where(index => (~flags & (1Lu << index)) > 0)
                        .Select(index => baseFilter + index);

                    IEnumerable<TimeFilter> timeFilters = Enumerable.Empty<TimeFilter>()
                        .Concat(ReadTimeFilters(post.Hours, TimeFilter.Hour00, 24))
                        .Concat(ReadTimeFilters(post.Days, TimeFilter.Sunday, 7))
                        .Concat(ReadTimeFilters(post.Weeks, TimeFilter.Week00, 53))
                        .Concat(ReadTimeFilters(post.Months, TimeFilter.January, 12));

                    void BuildQuery(IQueryBuilder builder) => builder
                        .Range(post.StartTime, post.EndTime)
                        .FilterTags(tags)
                        .Aggregate(post.Aggregate)
                        .FilterTime(timeFilters);

                    return await hids
                        .ReadPointsAsync(BuildQuery, cancellationToken)
                        .ToListAsync();
                }
            }

            List<Point> points = QueryHIDSAsync().GetAwaiter().GetResult();
            MemoryStream stream = new MemoryStream();
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, points);
            stream.Position = 0;

            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StreamContent(stream);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            return response;
        }

        [HttpPost]
        [ValidateRequestVerificationToken]
        [ActionName("Table")]
        public async Task<HttpResponseMessage> LegacyPostTable([FromBody] HIDSPost post, CancellationToken cancellationToken)
        {
            async Task<HttpResponseMessage> FailAsync(Exception ex)
            {
                BadRequestErrorMessageResult result = BadRequest(ex.Message);
                return await result.ExecuteAsync(cancellationToken);
            }

            DataTable table;
            try { table = GetTable(Host, post); }
            catch (ArgumentException ex) { return await FailAsync(ex); }

            MemoryStream stream = new MemoryStream();
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, table);
            stream.Position = 0;

            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StreamContent(stream);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            return response;
        }

        private async Task<API> CreateHIDSConnectionAsync()
        {
            ConfigurationLoader configurationLoader = new ConfigurationLoader(Host.ID, Host.CreateDbConnection);
            Settings settings = new Settings();
            configurationLoader.Configure(settings);

            API hids = new API();
            await hids.ConfigureAsync(settings.HIDSSettings);
            return hids;
        }

        public static DataTable GetTable(Host host, HIDSPost post)
        {
            string[] validFilters = { "Meter", "Asset" };

            if (!validFilters.Contains(post.By))
                throw new ArgumentException($"Invalid parameter value: {{\"By\":\"{post.By}\"}}");

            using (AdoDataConnection connection = host.CreateDbConnection())
            {
                return connection.RetrieveData(@"
                    SELECT
	                    DISTINCT Channel.ID, Meter.Name as Meter, Asset.AssetName as Asset, Phase.Name as Phase,
                        Channel.Name, MeasurementCharacteristic.Name as Characteristic, MeasurementType.Name as Type,
                        Channel.HarmonicGroup as Harmonic, Location.Name as Station, Location.Latitude, Location.Longitude, COALESCE(ValueList.Value, 'Unknown') as Unit
                    FROM
	                    Channel JOIN
	                    Meter ON Meter.ID = Channel.MeterID JOIN
	                    Asset ON Asset.ID = Channel.AssetID JOIN
	                    Phase ON Phase.ID = Channel.PhaseID JOIN
                        Location ON Location.ID = Meter.LocationID JOIN
	                    MeasurementCharacteristic ON MeasurementCharacteristic.ID = Channel.MeasurementCharacteristicID JOIN
	                    MeasurementType ON MeasurementType.ID = Channel.MeasurementTypeID JOIN
	                    ChannelGroupType ON ChannelGroupType.MeasurementCharacteristicID = Channel.MeasurementCharacteristicID AND ChannelGroupType.MeasurementTypeID = Channel.MeasurementTypeID LEFT JOIN
                        ValueList ON ChannelGroupType.UnitID = ValueList.ID
                    WHERE
	                    " + post.By + @".ID IN (" + string.Join(",", post.IDs) + @") AND
	                    Phase.ID IN (" + string.Join(",", post.Phases) + @") AND
	                    ChannelGroupType.ID IN (" + string.Join(",", post.Types) + @")
                ");
            }
        }

        private static string ReadDate(JObject obj, string key)
        {
            JToken token = obj[key];

            if (token is null)
                return null;

            if (token.Type != JTokenType.Date)
                return token.Value<string>();

            DateTime dt = token.Value<DateTime>();
            return API.FormatTimestamp(dt);
        }
    }
}
