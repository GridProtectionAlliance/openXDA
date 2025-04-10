//******************************************************************************************************
//  OSIPISection.cs - Gbtc
//
//  Copyright © 2025, Grid Protection Alliance.  All Rights Reserved.
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
//  03/21/2025 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System.ComponentModel;
using System.Configuration;

namespace openXDA.Configuration
{
    public class OSIPISection
    {
        #region [ Members ]

        // Constants
        public const string CategoryName = "OSIPI";

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets or sets name of the PI historian server to connect to.
        /// </summary>
        [Setting]
        [DefaultValue("")]
        public string ServerName { get; set; }

        /// <summary>
        /// Gets or sets user name for logging into PI historian server.
        /// </summary>
        [Setting]
        [DefaultValue("")]
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets password for logging into PI historian server.
        /// </summary>
        [Setting]
        [DefaultValue("")]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets time to wait when establishing
        /// connection to PI server before failing.
        /// </summary>
        [Setting]
        [DefaultValue(30000)]
        public int ConnectTimeout { get; set; }

        #endregion
    }
}
