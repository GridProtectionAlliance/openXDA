//******************************************************************************************************
//  FTTDataSource.cs - Gbtc
//
//  Copyright © 2024, Grid Protection Alliance.  All Rights Reserved.
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
//  04/01/2024 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using Azure.Core;
using GSF;
using GSF.Data;
using GSF.Data.Model;
using Newtonsoft.Json.Linq;
using openXDA.Model;
using openXDA.PQI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace openXDA.NotificationDataSources.FaultTraceTool
{
    public class FTTDataSource : ITriggeredDataSource
    {
        #region [ Constructors ]

        public FTTDataSource(Func<AdoDataConnection> xdaConnectionFactory) =>
            XDAConnectionFactory = xdaConnectionFactory;

        #endregion

        #region [ Properties ]

        private FTTOptions FTTOptions { get; } = new FTTOptions();
        private Func<AdoDataConnection> XDAConnectionFactory { get; }

        #endregion

        #region [ Methods ]

        public void Configure(Action<object> configurator) =>
            configurator(FTTOptions);

        public XElement Process(Event evt)
        {
            XElement fttElement = new XElement("ftt");
            List<FTTRecord> fttRecords = LoadFTTRecords(evt);

            foreach (IGrouping<int, FTTRecord> grouping in fttRecords.GroupBy(record => record.FaultNumber))
            {
                XElement faultElement = new XElement("fault", new XAttribute("num", grouping.Key));
                string queryString = BuildQueryString(grouping);
                string fullURL = BuildFullURL(grouping);
                faultElement.Add(new XElement("baseURL", FTTOptions.URL));
                faultElement.Add(new XElement("queryString", queryString));
                faultElement.Add(new XElement("fullURL", fullURL));

                if (!FTTOptions.URLOnly)
                    faultElement.Add(QueryImageData(fullURL));

                fttElement.Add(faultElement);
            }

            return fttElement;
        }

        private List<FTTRecord> LoadFTTRecords(Event evt)
        {
            using (AdoDataConnection connection = XDAConnectionFactory())
            using (DataTable fttRecordData = connection.RetrieveData(FTTOptions.FTTRecordSQL, evt.ID))
            {
                TableOperations<FTTRecord> fttRecordTable = new TableOperations<FTTRecord>(connection);

                return fttRecordData.Select()
                    .Select(fttRecordTable.LoadRecord)
                    .ToList();
            }
        }

        private string BuildQueryString(IEnumerable<FTTRecord> records)
        {
            IEnumerable<string> queryParts = new[] { $"totalLine={records.Count()}" }
                .Concat(records.Select(GetQueryString))
                .Where(queryPart => !string.IsNullOrEmpty(queryPart));

            if (!string.IsNullOrEmpty(FTTOptions.TokenURL))
            {
                queryParts.Append($"token={GetToken()}");
            }

            return string.Join("&", queryParts);
        }

        private string BuildFullURL(IEnumerable<FTTRecord> records)
        {
            UriBuilder builder = new UriBuilder(FTTOptions.URL);

            IEnumerable<string> queryParts = new[] { builder.Query.TrimStart('?'), $"totalLine={records.Count()}" }
                .Concat(records.Select(GetQueryString))
                .Where(queryPart => !string.IsNullOrEmpty(queryPart));

            if (!string.IsNullOrEmpty(FTTOptions.TokenURL))
            {
                queryParts.Append($"token={GetToken()}");
            }

            builder.Query = string.Join("&", queryParts);

            return builder.ToString();
        }

        private string GetQueryString(FTTRecord record, int index)
        {
            var parameters = new
            {
                index,
                station = record.StationName,
                line = record.LineKey,
                distance = record.Distance,
                eventTime = record.EventTime,
                endLatitude = record.EndLatitude,
                endLongitude = record.EndLongitude
            };

            return FTTOptions.QueryStringFormat.Interpolate(parameters);
        }

        private XElement QueryImageData(string url)
        {
            FTTImageGenerator fttImageGenerator = new FTTImageGenerator(FTTOptions);

            using (Stream imageStream = fttImageGenerator.QueryToImageStream(url))
            using (MemoryStream memoryStream = new MemoryStream())
            {
                imageStream.CopyTo(memoryStream);

                byte[] imageData = memoryStream.ToArray();
                string base64Image = Convert.ToBase64String(imageData);
                return new XElement("imageData", base64Image);
            }
        }

        /// <summary>
        /// Gets the Token using the TokenURl, User and Password
        /// </summary>
        /// <returns></returns>
        private async Task<string> GetToken()
        {
            void ConfigureRequest(HttpRequestMessage request)
            {
                request.RequestUri = new Uri(FTTOptions.TokenURL);
                request.Method = HttpMethod.Post;

                List<KeyValuePair<string, string>> body = new List<KeyValuePair<string, string>>();
                body.Add(new KeyValuePair<string, string>("username", FTTOptions.TokenUser));
                body.Add(new KeyValuePair<string, string>("password", FTTOptions.TokenPassword));
                body.Add(new KeyValuePair<string, string>("client", "referer"));
                body.Add(new KeyValuePair<string, string>("ip", ""));
                body.Add(new KeyValuePair<string, string>("referer", "https://gisdu.tva.gov/openftt/"));
                body.Add(new KeyValuePair<string, string>("expiration", "60"));
                body.Add(new KeyValuePair<string, string>("f", "json"));
                request.Content = new FormUrlEncodedContent(body);

            }

            using (HttpResponseMessage response = await HttpClient.SendRequestAsync(ConfigureRequest).ConfigureAwait(false))
            {
                response.EnsureSuccessStatusCode();

                Stopwatch stopwatch = Stopwatch.StartNew();
                JObject content = JObject.Parse(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
                int expiration = content["expires"].Value<int>();
                string accessToken = content["token"].Value<string>();
                return accessToken;
            }

        }



        private static HttpClient HttpClient =>
            HttpClientProvider.GetClient();
        #endregion
    }

    internal static class HttpClientExtensions
    {
        public static async Task<HttpResponseMessage> SendRequestAsync(this HttpClient client, Action<HttpRequestMessage> configure)
        {
            using (HttpRequestMessage request = new HttpRequestMessage())
            {
                configure(request);
                return await client.SendAsync(request).ConfigureAwait(false);
            }
        }
    }
}
