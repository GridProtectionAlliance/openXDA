//******************************************************************************************************
//  HIDSDataSource.cs - Gbtc
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
//  05/06/2022 - Stephen Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using GSF;
using GSF.Configuration;
using GSF.Data;
using GSF.Xml;
using HIDS;
using openXDA.HIDS;
using openXDA.HIDS.APIExtensions;
using openXDA.Model;

namespace openXDA.NotificationDataSources
{
    public class HIDSDataSource : IScheduledDataSource
    {
        #region [ Members ]

        // Nested Types
        private class DataSourceSettings
        {
            const string DefaultInfluxQuery =
                "option now = () => {TimeOccurred}\n" +
                "from(bucket: {Bucket})\n" +
                "  |> range(start: -1h)\n" +
                "  |> filter(fn: (r) => r._measurement == \"point\")\n" +
                "  |> filter(fn: (r) => {TagFilter})" +
                "  |> pivot(rowKey: [\"_time\"], columnKey: [\"_field\"], valueColumn: \"_value\")\n" +
                "  |> filter(fn: (r) => r.flags == 0)";

            public DataSourceSettings(Action<object> configure) =>
                configure(this);

            [Category]
            [SettingName("HIDS")]
            public HIDSSettings HIDSSettings { get; }
                = new HIDSSettings();

            [Setting]
            [DefaultValue("SELECT NULL FOR XML PATH('Data')")]
            public string XDAQuery { get; set; }

            [Setting]
            [DefaultValue(DefaultInfluxQuery)]
            public string InfluxQuery { get; set; }

            [Setting]
            [DefaultValue(false)]
            public bool UsePointCount { get; set; }
        }

        #endregion

        #region [ Constructors ]

        public HIDSDataSource(Func<AdoDataConnection> connectionFactory) =>
            ConnectionFactory = connectionFactory;

        #endregion

        #region [ Properties ]

        private Func<AdoDataConnection> ConnectionFactory { get; }
        private DataSourceSettings Settings { get; set; }

        #endregion

        #region [ Methods ]

        public void Configure(Action<object> configurator) =>
            Settings = new DataSourceSettings(configurator);

        public XElement Process(DateTime xdaNow, DateTime xdaPrev, DateTime xdaNext)
        {
            XElement xml = QueryXDA(xdaNow);
            XDocument doc = new XDocument(xml);

            using (API hids = new API())
            {
                Task queryTask = new Func<Task>(async () =>
                {
                    await hids.ConfigureAsync(Settings.HIDSSettings);
                    await QueryInfluxAsync(hids, xdaNow, doc);
                })();
                queryTask.GetAwaiter().GetResult();
                return xml;
            }
        }

        private XElement QueryXDA(DateTime timeOccurred)
        {
            using (AdoDataConnection connection = ConnectionFactory())
            {
                string xml = connection.ExecuteScalar<string>(Settings.XDAQuery, timeOccurred);
                return XElement.Parse(xml);
            }
        }

        private async Task QueryInfluxAsync(API hids, DateTime timeOccurred, XDocument doc)
        {
            string fluxQuery = BuildFluxQuery(hids, timeOccurred, doc);

            async Task<ILookup<int, XElement>> QueryPointCountAsync() => await hids
                .ReadPointCountAsync(fluxQuery)
                .ToLookupAsync(pointCount => hids.ToChannelID(pointCount.Tag), ToPointCountElement);

            async Task<ILookup<int, XElement>> QueryPointsAsync() => await hids
                .ReadPointsAsync(fluxQuery)
                .ToLookupAsync(point => hids.ToChannelID(point.Tag), ToPointElement);

            ILookup<int, XElement> lookup = Settings.UsePointCount
                ? await QueryPointCountAsync()
                : await QueryPointsAsync();

            doc.TransformAll("TrendingData", trendingDataTag =>
            {
                int? channelID = ParseChannelID(trendingDataTag);

                IEnumerable<XElement> children = !(channelID is null)
                    ? lookup[channelID.GetValueOrDefault()]
                    : Enumerable.Empty<XElement>();

                return new XElement("TrendingData", children);
            });
        }

        private string BuildFluxQuery(API hids, DateTime timeOccurred, XDocument doc)
        {
            IEnumerable<string> tags = FindTags(hids, doc);
            IEnumerable<string> tagConditionals = tags.Select(tag => $"r.tag == \"{tag}\"");
            string tagExpression = string.Join(" or ", tagConditionals);

            if (string.IsNullOrEmpty(tagExpression))
                return string.Empty;

            string formattedTimeOccurred = API.FormatTimestamp(timeOccurred);

            var parameters = new
            {
                Bucket = hids.PointBucket,
                TimeOccurred = formattedTimeOccurred,
                TagFilter = tagExpression
            };

            return Settings.InfluxQuery.Interpolate(parameters);
        }

        private IEnumerable<string> FindTags(API hids, XDocument doc) => doc
            .Descendants("TrendingData")
            .Select(ParseChannelID)
            .Where(channelID => !(channelID is null))
            .Cast<int>()
            .Select(hids.ToTag);

        private int? ParseChannelID(XElement trendingDataElement)
        {
            string text = (string)trendingDataElement.Attribute("ChannelID");

            return int.TryParse(text, out int channelID)
                ? channelID
                : (int?)null;
        }

        private XElement ToPointElement(Point point) =>
            new XElement("Point",
                new XAttribute("Timestamp", point.Timestamp),
                new XAttribute("Maximum", point.Maximum),
                new XAttribute("Minimum", point.Minimum),
                new XAttribute("Average", point.Average));

        private XElement ToPointCountElement(PointCount pointCount) =>
            new XElement("PointCount",
                new XElement("Timestamp", pointCount.Timestamp),
                new XElement("Count", pointCount.Count));

        #endregion
    }
}
