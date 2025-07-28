//******************************************************************************************************
//  TrendingDataSection.cs - Gbtc
//
//  Copyright © 2022, Grid Protection Alliance.  All Rights Reserved.
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
//  08/26/2022 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System.ComponentModel;
using System.Configuration;

namespace openXDA.Configuration
{
    public class TrendingDataSection
    {
        #region [ Members ]

        // Nested Types
        public class RMSSubSection
        {
            [Setting]
            [DefaultValue(@"Trms.dat")]
            public string FolderPath { get; set; }
            [Setting]
            [DefaultValue(null)]
            public string DescriptionRegexMatchFilter { get; set; }
        }

        public class FlickerSubSection
        {
            [Setting]
            [DefaultValue(@"FkrR[0-9]*\.dat")]
            public string FolderPath { get; set; }
            [Setting]
            [DefaultValue(null)]
            public string DescriptionRegexMatchFilter { get; set; }
        }

        public class TriggerSubSection
        {
            [Setting]
            [DefaultValue(@"TrR[0-9]*\.dat")]
            public string FolderPath { get; set; }
            [Setting]
            [DefaultValue(@"\s-\sV\S*\sRMS\s*$")]
            public string DescriptionTriggerRMSMatch { get; set; }
            [Setting]
            [DefaultValue(@"\s-\sV\S*\sImpulse\s*$")]
            public string DescriptionTriggerImpulseMatch { get; set; }
            [Setting]
            [DefaultValue(@"\s-\s\S+\sTHD\s*$")]
            public string DescriptionTriggerTHDMatch { get; set; }
            [Setting]
            [DefaultValue(@"\s-\sUnbalance\s*$")]
            public string DescriptionTriggerUnbalanceMatch { get; set; }
            [Setting]
            [DefaultValue(@"\s-\s\S*I\S*\s*$")]
            public string DescriptionTriggerCurrentMatch { get; set; }
            [Setting]
            [DefaultValue(null)]
            public string DescriptionRegexMatchFilter { get; set; }
        }

        public class FrequencySubSection
        {
            [Setting]
            [DefaultValue(@"Tfrq[0-9]*\.dat")]
            public string FolderPath { get; set; }
            [Setting]
            [DefaultValue(null)]
            public string DescriptionRegexMatchFilter { get; set; }
        }

        // Constants
        public const string CategoryName = "TrendingData";

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets RMS folder path settings.
        /// </summary>
        [Category]
        public RMSSubSection RMS { get; } = new RMSSubSection();

        /// <summary>
        /// Gets flicker folder path settings.
        /// </summary>
        [Category]
        public FlickerSubSection Flicker { get; } = new FlickerSubSection();

        /// <summary>
        /// Gets trigger folder path settings.
        /// </summary>
        [Category]
        public TriggerSubSection Trigger { get; } = new TriggerSubSection();

        /// <summary>
        /// Gets frequency folder path settings.
        /// </summary>
        [Category]
        public FrequencySubSection Frequency { get; } = new FrequencySubSection();

        #endregion
    }
}
