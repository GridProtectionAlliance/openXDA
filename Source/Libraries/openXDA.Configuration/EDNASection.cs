//******************************************************************************************************
//  EDNASection.cs - Gbtc
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
//  08/09/2018 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.ComponentModel;
using System.Configuration;

namespace openXDA.Configuration
{
    public class EDNASection
    {
        #region [ Members ]

        // Constants
        public const string CategoryName = "EDNA";

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets or sets the database query to get the list of
        /// EDNA points that indicate breaker state for the line.
        /// </summary>
        [Setting]
        [DefaultValue("SELECT NULL AS Point WHERE 1 IS NULL")]
        public string PointQuery { get; set; }

        /// <summary>
        /// Gets or sets the tolerance, in seconds, that determines the
        /// time range to be queried around the fault clearing point.
        /// </summary>
        [Setting]
        [DefaultValue(4.0D)]
        public double QueryTolerance { get; set; }

        /// <summary>
        /// Gets or sets the value of the point representing
        /// breaker state when the breaker is open.
        /// </summary>
        [Setting]
        [DefaultValue(0.0D)]
        public double BreakerOpenValue { get; set; }

        /// <summary>
        /// Gets the tolerance that determines the time range
        /// to be queried around the fault clearing point.
        /// </summary>
        public TimeSpan QueryToleranceSpan
        {
            get
            {
                return TimeSpan.FromSeconds(QueryTolerance);
            }
        }

        #endregion
    }
}
