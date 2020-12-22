//******************************************************************************************************
//  EventEmailSection.cs - Gbtc
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
//  02/13/2021 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.ComponentModel;
using System.Configuration;
using GSF.Configuration;

namespace openXDA.Configuration
{
    public class EventEmailSection
    {
        public const string CategoryName = "EventEmail";

        [Setting]
        [DefaultValue(false)]
        public bool Enabled { get; set; }

        [Setting]
        [DefaultValue(0)]
        public int MaxEmailCount { get; set; }

        [Setting]
        [SettingName(nameof(MaxEmailSpan))]
        [DefaultValue(0.0D)]
        public double MaxEmailSeconds
        {
            get => MaxEmailSpan.TotalSeconds;
            set => MaxEmailSpan = TimeSpan.FromSeconds(value);
        }

        public TimeSpan MaxEmailSpan { get; set; }
    }
}
