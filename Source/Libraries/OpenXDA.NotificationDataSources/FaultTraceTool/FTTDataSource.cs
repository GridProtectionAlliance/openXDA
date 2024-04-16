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

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using GSF;
using GSF.Data;
using GSF.Data.Model;
using openXDA.Model;

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

            return string.Join("&", queryParts);
        }

        private string BuildFullURL(IEnumerable<FTTRecord> records)
        {
            UriBuilder builder = new UriBuilder(FTTOptions.URL);

            IEnumerable<string> queryParts = new[] { builder.Query.TrimStart('?'), $"totalLine={records.Count()}" }
                .Concat(records.Select(GetQueryString))
                .Where(queryPart => !string.IsNullOrEmpty(queryPart));

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

        #endregion
    }
}
