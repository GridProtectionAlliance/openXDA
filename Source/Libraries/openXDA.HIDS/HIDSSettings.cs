﻿//******************************************************************************************************
//  HIDSSettings.cs - Gbtc
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
//  10/27/2020 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System.ComponentModel;
using System.Configuration;

namespace openXDA.HIDS
{
    public class HIDSSettings
    {
        public const string CategoryName = "HIDS";

        [Setting]
        [DefaultValue("")]
        public string Host { get; set; }

        [Setting]
        [DefaultValue("")]
        public string TokenID { get; set; }

        [Setting]
        [DefaultValue("point_bucket")]
        public string PointBucket { get; set; }

        [Setting]
        [DefaultValue("gpa")]
        public string OrganizationID { get; set; }
    }
}
