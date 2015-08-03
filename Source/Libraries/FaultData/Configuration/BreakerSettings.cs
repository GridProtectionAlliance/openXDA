//******************************************************************************************************
//  BreakerSettings.cs - Gbtc
//
//  Copyright © 2015, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the MIT License (MIT), the "License"; you may
//  not use this file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://opensource.org/licenses/MIT
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  08/02/2015 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System.ComponentModel;
using System.Configuration;

namespace FaultData.Configuration
{
    public class BreakerSettings
    {
        #region [ Members ]

        // Fields
        private double m_openBreakerThreshold;
        private double m_lateBreakerThreshold;

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets or sets the maximum RMS current, in amps,
        /// at which the breaker can be considered open.
        /// </summary>
        [Setting]
        [DefaultValue(20.0D)]
        public double OpenBreakerThreshold
        {
            get
            {
                return m_openBreakerThreshold;
            }
            set
            {
                m_openBreakerThreshold = value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum number of cycles that a breaker
        /// operation's timing can exceed the configured breaker speed.
        /// </summary>
        [Setting]
        [DefaultValue(0.0D)]
        public double LateBreakerThreshold
        {
            get
            {
                return m_lateBreakerThreshold;
            }
            set
            {
                m_lateBreakerThreshold = value;
            }
        }

        #endregion
    }
}
