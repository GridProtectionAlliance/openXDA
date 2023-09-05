//******************************************************************************************************
//  GrafanaSection.cs - Gbtc
//
//  Copyright © 2023, Grid Protection Alliance.  All Rights Reserved.
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
//  08/18/2023 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System.ComponentModel;
using System.Configuration;

namespace openXDA.Configuration
{
    public class GrafanaSection
    {
        public const string CategoryName = "Grafana";

        /// <summary>
        /// Gets or sets the base path to the Grafana server executable.
        /// </summary>
        [Setting]
        [DefaultValue(@"Grafana\bin\grafana-server.exe")]
        public string ServerPath { get; set; }

        /// <summary>
        /// Gets or sets the base path to the
        /// folder containing the Grafana process.
        /// </summary>
        [Setting]
        [DefaultValue("Grafana")]
        public string BasePath { get; set; }

        [Setting]
        [DefaultValue("http://localhost:8185")]
        public string HostedURL { get; set; }

        [Setting]
        [DefaultValue("admin")]
        public string AdminUser { get; set; }

        [Setting]
        [DefaultValue(1)]
        public int OrganizationID { get; set; }

        [Setting]
        [DefaultValue("X-WEBAUTH-USER")]
        public string AuthProxyHeaderName { get; set; }

        [Setting]
        [DefaultValue("x-last-dashboard")]
        public string LastDashboardCookieName { get; set; }

        [Setting]
        [DefaultValue(30)]
        public int InitializationTimeout { get; set; }
    }
}
