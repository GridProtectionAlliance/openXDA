//******************************************************************************************************
//  FTTOptions.cs - Gbtc
//
//  Copyright © 2021, Grid Protection Alliance.  All Rights Reserved.
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
//  02/11/2021 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System.ComponentModel;
using System.Configuration;

namespace openXDA.NotificationDataSources.FaultTraceTool
{
    public class FTTOptions
    {
        const string DefaultFTTRecordSQL =
            "SELECT " +
            "    ROW_NUMBER() OVER(ORDER BY FaultSummary.Inception) FaultNumber, " +
            "    Location.Name StationName, " +
            "    Asset.AssetKey LineKey, " +
            "    FaultSummary.Distance, " +
            "    DATEADD(MINUTE, -Event.TimeZoneOffset, FaultSummary.Inception) EventTime, " +
            "    NULL EndLatitude, " +
            "    NULL EndLongitude " +
            "FROM " +
            "    Event JOIN " +
            "    Meter ON Event.MeterID = Meter.ID JOIN " +
            "    Location ON Meter.LocationID = Location.ID JOIN " +
            "    Asset ON Event.AssetID = Asset.ID JOIN " +
            "    FaultSummary ON " +
            "        FaultSummary.EventID = Event.ID AND " +
            "        FaultSummary.IsSelectedAlgorithm <> 0 " +
            "WHERE Event.ID = {0}";

        [Setting]
        [DefaultValue(DefaultFTTRecordSQL)]
        public string FTTRecordSQL { get; set; }

        [Setting]
        [DefaultValue("http://localhost/faulttracetool")]
        public string URL { get; set; }

        [Setting]
        [DefaultValue(false)]
        public bool URLOnly { get; set; }

        [Setting]
        [DefaultValue(@"FaultTraceTool\FaultTraceToolInterop.exe")]
        public string CLIPath { get; set; }

        [Setting]
        [DefaultValue("lineid_{index}={line}&stationname_{index}={station}&distance_{index}={distance:0.######}&eventtime_{index}={eventTime:yyyy-MM-ddTHH:mm:ss.fffffff}")]
        public string QueryStringFormat { get; set; }

        [Setting]
        [DefaultValue(60.0D)]
        public double QueryTimeout { get; set; }

        [Setting]
        [DefaultValue(false)]
        public bool IgnoreCertificateErrors { get; set; }

        [Setting]
        public int ImageWidth { get; set; }

        [Setting]
        public int ImageHeight { get; set; }

        [Setting]
        [DefaultValue("")]
        public string BrowserArguments { get; set; }
    }
}
