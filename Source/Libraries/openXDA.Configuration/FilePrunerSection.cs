//******************************************************************************************************
//  FilePrunerSection.cs - Gbtc
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
//  02/08/2021 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.ComponentModel;
using System.Configuration;
using GSF.Configuration;

namespace openXDA.Configuration
{
    public class FilePrunerSection
    {
        public const string CategoryName = "FilePruner";

        [Setting]
        [DefaultValue("* 0 * * *")]
        public string Schedule { get; set; }

        /// <summary>
        /// Gets or sets the amount of time, in days,
        /// the file pruner should keep files in openXDA's
        /// watch directories.
        /// </summary>
        [Setting]
        [SettingName(nameof(RetentionPeriod))]
        [DefaultValue(0)]
        public int RetentionPeriodDays
        {
            get => (int)RetentionPeriod.TotalDays;
            set => RetentionPeriod = TimeSpan.FromDays(value);
        }

        /// <summary>
        /// Gets or sets the amount of time the file pruner
        /// should keep files in openXDA's watch directories.
        /// </summary>
        public TimeSpan RetentionPeriod { get; set; }
    }
}
