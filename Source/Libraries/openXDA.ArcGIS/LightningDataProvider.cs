//******************************************************************************************************
//  LightningDataProvider.cs - Gbtc
//
//  Copyright © 2019, Grid Protection Alliance.  All Rights Reserved.
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
//  04/02/2019 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Esri.ArcGISRuntime;
using Esri.ArcGISRuntime.Data;
using Esri.ArcGISRuntime.Geometry;
using FaultData.DataResources;
using GSF.Configuration;
using openXDA.Model;

namespace openXDA.ArcGIS
{
    public class LightningDataProviderSettings
    {
        private const string ServiceKey = "Service";
        private const string URLKey = "Service";
        private const string LightningMapServerPattern = @"\[(?<" + ServiceKey + @">[^]]+)\]\((?<" + URLKey + @">[^)]+)\)";

        [Setting]
        public string TransmissionDataMapServer { get; set; }

        [Setting]
        [SettingName(nameof(LightningMapServers))]
        public string LightningMapServersSetting { get; set; }

        public List<LightningMapServer> LightningMapServers => Regex.Matches(LightningMapServersSetting, LightningMapServerPattern)
            .Cast<Match>()
            .Where(match => match.Success)
            .Select(match => new LightningMapServer(match.Groups[ServiceKey].Value, match.Groups[URLKey].Value))
            .ToList();
    }

    public class LightningDataProvider : ILightningDataProvider
    {
        public LightningDataProvider() =>
            LightningDataSettings = new LightningDataProviderSettings();

        [Setting]
        [SettingName("XDATimeZone")]
        public string XDATimeZoneID { get; set; }

        [Category]
        [SettingName("Lightning")]
        public LightningDataProviderSettings LightningDataSettings { get; }

        private TimeZoneInfo XDATimeZone =>
            TimeZoneInfo.FindSystemTimeZoneById(XDATimeZoneID);

        public IEnumerable<ILightningStrike> GetLightningStrikes(string lineKey, DateTime start, DateTime end) =>
            QueryLightningStrikes(lineKey, start, end).Result;

        private async Task<IEnumerable<ILightningStrike>> QueryLightningStrikes(string lineKey, DateTime start, DateTime end)
        {
            List<Geometry> lineGeometry = await QueryLineGeometry(lineKey);
            Geometry lightningBufferGeometry = ToLightningBufferGeometry(lineGeometry);

            TimeZoneInfo xdaTimeZone = XDATimeZone;
            DateTime startUTC = TimeZoneInfo.ConvertTimeToUtc(start, xdaTimeZone);
            DateTime endUTC = TimeZoneInfo.ConvertTimeToUtc(end, xdaTimeZone);
            DateTimeOffset startOffset = new DateTimeOffset(startUTC);
            DateTimeOffset endOffset = new DateTimeOffset(endUTC);
            return await QueryLightningData(lightningBufferGeometry, startOffset, endOffset);
        }

        private async Task<List<Geometry>> QueryLineGeometry(string lineKey)
        {
            string transmissionDataMapServer = LightningDataSettings.TransmissionDataMapServer;

            QueryParameters lineQuery = new QueryParameters();
            lineQuery.ReturnGeometry = true;
            lineQuery.WhereClause = $"UPPER(LINENAME) LIKE '%{lineKey.ToUpper()}%'";

            ServiceFeatureTable featureTable = new ServiceFeatureTable(new Uri(transmissionDataMapServer));
            FeatureQueryResult result = await featureTable.QueryFeaturesAsync(lineQuery);

            return result
                .Select(feature => feature.Geometry)
                .ToList();
        }

        private Geometry ToLightningBufferGeometry(List<Geometry> lineGeometry)
        {
            const double BufferMiles = 5.0D;

            List<double> distances = lineGeometry
                .Select(geometry => geometry.SpatialReference.Unit as LinearUnit)
                .Select(unit => (unit != null) ? LinearUnits.Miles.ConvertTo(unit, BufferMiles) : BufferMiles)
                .ToList();

            return GeometryEngine.Buffer(lineGeometry, distances, true).Single();
        }

        private async Task<List<ILightningStrike>> QueryLightningData(Geometry lightningBufferGeometry, DateTimeOffset startTime, DateTimeOffset endTime)
        {
            List<LightningMapServer> lightningMapServers = LightningDataSettings.LightningMapServers;

            IEnumerable<Task<List<LightningStrike>>> queryTasks = lightningMapServers
                .Select(server =>
                {
                    QueryParameters lightningQuery = new QueryParameters();
                    lightningQuery.TimeExtent = new TimeExtent(startTime, endTime);
                    lightningQuery.Geometry = lightningBufferGeometry;
                    lightningQuery.SpatialRelationship = SpatialRelationship.Intersects;

                    ServiceFeatureTable featureTable = new ServiceFeatureTable(new Uri(server.URL));

                    return featureTable.QueryFeaturesAsync(lightningQuery, QueryFeatureFields.LoadAll)
                        .ContinueWith(task =>
                        {
                            return task.Result
                                .Select(feature => new LightningStrike(server.Service, feature))
                                .ToList();
                        });
                });

            return (await Task.WhenAll(queryTasks))
                .SelectMany(strikes => strikes.Cast<ILightningStrike>())
                .ToList();
        }
    }
}
