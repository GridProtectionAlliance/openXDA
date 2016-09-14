//******************************************************************************************************
//  EventEmailSettings.cs - Gbtc
//
//  Copyright © 2016, Grid Protection Alliance.  All Rights Reserved.
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
//  02/19/2016 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System.ComponentModel;
using System.Configuration;

namespace FaultData.Configuration
{
    public class EventEmailSettings
    {
        #region [ Members ]

        // Fields
        private double m_waitPeriod;

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets or sets the amount of time, in seconds,
        /// between the time a file is received and the time
        /// an email should be sent out by the system.
        /// </summary>
        [Setting]
        [DefaultValue(10.0D)]
        public double WaitPeriod
        {
            get
            {
                return m_waitPeriod;
            }
            set
            {
                m_waitPeriod = value;
            }
        }

        #endregion
    }
}
