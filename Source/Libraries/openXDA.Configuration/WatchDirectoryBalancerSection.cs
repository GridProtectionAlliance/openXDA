//******************************************************************************************************
//  WatchDirectoryBalancerSection.cs - Gbtc
//
//  Copyright © 2026, Grid Protection Alliance.  All Rights Reserved.
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
//  06/26/2026 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System.ComponentModel;
using System.Configuration;

namespace openXDA.Configuration
{
    public class WatchDirectoryBalancerSection
    {
        public const string CategoryName = "WatchDirectoryBalancer";

        [Setting]
        [DefaultValue("*/10 * * * *")]
        public string Schedule { get; set; }

        [Setting]
        [DefaultValue("")]
        public string BaseDirectory { get; set; }

        [Setting]
        [DefaultValue("")]
        public string SearchPattern { get; set; }

        [Setting]
        [DefaultValue(0)]
        public int SearchDepth { get; set; }

        [Setting]
        [DefaultValue(10)]
        public int BalanceThreshold { get; set; }
    }
}
