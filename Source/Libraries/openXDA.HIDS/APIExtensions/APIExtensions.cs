//******************************************************************************************************
//  APIExtensions.cs - Gbtc
//
//  Copyright © 2020, Grid Protection Alliance.  All Rights Reserved.
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
//  11/09/2020 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using HIDS;

namespace openXDA.HIDS.APIExtensions
{
    public static class APIExtensions
    {
        public static void Configure(this API hids, HIDSSettings settings)
        {
            if (string.IsNullOrEmpty(settings.Host))
                throw new ArgumentException("Unable to configure connection to HIDS: Host not specified.", nameof(settings));

            if (!string.IsNullOrEmpty(settings.TokenID))
                hids.TokenID = settings.TokenID;

            if (!string.IsNullOrEmpty(settings.PointBucket))
                hids.PointBucket = settings.PointBucket;

            if (!string.IsNullOrEmpty(settings.OrganizationID))
                hids.OrganizationID = settings.OrganizationID;

            hids.Connect(settings.Host);
        }

        public static string ToTag(this API _, int channelID) =>
            channelID.ToString("x8");

        public static int ToChannelID(this API _, string tag) =>
            Convert.ToInt32(tag, 16);
    }
}
