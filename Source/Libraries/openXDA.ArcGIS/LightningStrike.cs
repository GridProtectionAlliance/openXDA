//******************************************************************************************************
//  LightningStrike.cs - Gbtc
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
//  04/30/2019 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using Esri.ArcGISRuntime.Data;
using FaultData.DataResources;

namespace openXDA.ArcGIS
{
    public class LightningStrike : ILightningStrike
    {
        private const string UTCTimeField = "EVENTUTCTIME";
        private const string DisplayTimeField = "DISPLAYTIME";
        private const string AmplitudeField = "AMPLITUDE";
        private const string LatitudeField = "LATITUDE";
        private const string LongitudeField = "LONGITUDE";

        public LightningStrike(string service, Feature feature)
        {
            Service = service;
            DisplayTime = feature.Attributes[DisplayTimeField] as string;
            Amplitude = (feature.Attributes[AmplitudeField] as double?) ?? double.NaN;
            Latitude = (feature.Attributes[LatitudeField] as double?) ?? double.NaN;
            Longitude = (feature.Attributes[LongitudeField] as double?) ?? double.NaN;

            DateTimeOffset? utcTimeOffset = feature.Attributes[UTCTimeField] as DateTimeOffset?;

            if (utcTimeOffset != null)
            {
                DateTime utcTime = utcTimeOffset.GetValueOrDefault().UtcDateTime;
                long subsecond = utcTime.Ticks % TimeSpan.TicksPerSecond;

                if (subsecond == 0 && DateTime.TryParse(DisplayTime, out DateTime displayTime))
                {
                    double timeZoneOffset = Math.Round(displayTime.Subtract(utcTime).TotalMinutes);
                    utcTime = displayTime.AddMinutes(-timeZoneOffset);
                }

                UTCTime = utcTime;
            }
        }

        public string Service { get; }
        public DateTime UTCTime { get; }
        public string DisplayTime { get; }
        public double Amplitude { get; }
        public double Latitude { get; }
        public double Longitude { get; }
    }
}
