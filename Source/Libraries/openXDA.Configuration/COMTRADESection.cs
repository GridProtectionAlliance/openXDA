//******************************************************************************************************
//  COMTRADESection.cs - Gbtc
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
//  06/07/2019 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.ComponentModel;
using System.Configuration;
using GSF.Configuration;

namespace openXDA.Configuration
{
    public class COMTRADESection
    {
        #region [ Members ]

        // Constants
        public const string CategoryName = "COMTRADE";

        #endregion

        #region [ Properties ]

        public TimeSpan MinWaitTime { get; set; }

        [Setting]
        [DefaultValue(false)]
        public bool WaitForINF { get; set; }

        [Setting]
        [DefaultValue(false)]
        public bool UseRelaxedValidation { get; set; }

        [Setting]
        [SettingName(nameof(MinWaitTime))]
        [DefaultValue(15.0D)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public double _MinWaitTime
        {
            get => MinWaitTime.TotalSeconds;
            set => MinWaitTime = TimeSpan.FromSeconds(value);
        }

        #endregion
    }
}
