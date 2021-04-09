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
        public const string CategoryName = "Lightning";

        private const string ServiceKey = "Service";
        private const string URLKey = "URL";
        private const string LightningMapServerPattern = @"\[(?<" + ServiceKey + @">[^]]+)\]\((?<" + URLKey + @">[^)]+)\)";

        [Setting]
        public string TransmissionDataMapServer { get; set; }

        [Setting]
        [SettingName(nameof(LightningMapServers))]
        public string LightningMapServersSetting { get; set; }

        [Setting]
        [DefaultValue(5)]
        public int QueryRetries { get; set; }

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
        [SettingName(nameof(XDATimeZone))]
        public string XDATimeZoneID { get; set; }

        [Category]
        [SettingName(LightningDataProviderSettings.CategoryName)]
        public LightningDataProviderSettings LightningDataSettings { get; }

        private TimeZoneInfo XDATimeZone =>
            TimeZoneInfo.FindSystemTimeZoneById(XDATimeZoneID);

        public IEnumerable<ILightningStrike> GetLightningStrikes(string lineKey, DateTime start, DateTime end) =>
            QueryLightningStrikes(lineKey, start, end).GetAwaiter().GetResult();

        private async Task<IEnumerable<ILightningStrike>> QueryLightningStrikes(string lineKey, DateTime start, DateTime end)
        {
            List<Geometry> lineGeometry = await QueryLineGeometry(lineKey);
            Geometry lightningBufferGeometry = ToLightningBufferGeometry(lineGeometry);

            if (lightningBufferGeometry == null)
                return Enumerable.Empty<ILightningStrike>();

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

            Uri source = new Uri(transmissionDataMapServer);
            ServiceFeatureTable featureTable = new ServiceFeatureTable(source);
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

            return GeometryEngine.Buffer(lineGeometry, distances, true).SingleOrDefault();
        }

        private async Task<List<ILightningStrike>> QueryLightningData(Geometry lightningBufferGeometry, DateTimeOffset startTime, DateTimeOffset endTime)
        {
            List<LightningMapServer> lightningMapServers = LightningDataSettings.LightningMapServers;
            List<ILightningStrike> strikes = new List<ILightningStrike>();

            foreach (LightningMapServer server in lightningMapServers)
            {
                QueryParameters lightningQuery = new QueryParameters();
                lightningQuery.TimeExtent = new TimeExtent(startTime, endTime);
                lightningQuery.Geometry = lightningBufferGeometry;
                lightningQuery.SpatialRelationship = SpatialRelationship.Intersects;

                int retryCount = 0;
                int maxRetries = LightningDataSettings.QueryRetries;

                while (true)
                {
                    try
                    {
                        Uri source = new Uri(server.URL);
                        ServiceFeatureTable featureTable = new ServiceFeatureTable(source);
                        FeatureQueryResult result = await featureTable.QueryFeaturesAsync(lightningQuery, QueryFeatureFields.LoadAll);
                        IEnumerable<ILightningStrike> serverStrikes = result.Select(feature => new LightningStrike(server.Service, feature));
                        strikes.AddRange(serverStrikes);
                        break;
                    }
                    catch (ArcGISRuntimeException)
                    {
                        if (maxRetries >= 0 && retryCount >= maxRetries)
                            throw;

                        await Task.Delay(500);
                        retryCount++;
                    }
                }
            }

            return strikes;
        }
    }
}
