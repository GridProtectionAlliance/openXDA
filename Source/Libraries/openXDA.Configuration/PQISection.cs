//******************************************************************************************************
//  PQISection.cs - Gbtc
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
//  07/29/2021 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System.ComponentModel;
using System.Configuration;

namespace openXDA.Configuration
{
    public class PQISection
    {
        #region [ Members ]

        // Constants
        public const string CategoryName = "PQI";

        #endregion

        #region [ Properties ]

        [Setting]
        [DefaultValue("https://pqiws.epri.com")]
        public string BaseURL { get; set; }

        [Setting]
        [DefaultValue("https://go.epri.com")]
        public string PingURL { get; set; }

        [Setting]
        [DefaultValue(null)]
        public string ClientID { get; set; }

        [Setting]
        [DefaultValue(null)]
        public string ClientSecret { get; set; }

        [Setting]
        [DefaultValue(null)]
        public string Username { get; set; }

        [Setting]
        [DefaultValue(null)]
        public string Password { get; set; }

        #endregion
    }
}
