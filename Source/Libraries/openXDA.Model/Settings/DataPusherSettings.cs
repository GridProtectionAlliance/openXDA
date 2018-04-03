//******************************************************************************************************
//  DataPusherSettings.cs - Gbtc
//
//  Copyright © 2018, Grid Protection Alliance.  All Rights Reserved.
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
//  04/02/2018 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************

using System.ComponentModel;
using System.Configuration;

namespace openXDA.Model
{
    public class DataPusherSettings
    {
        #region [ Members ]

        // Fields
        private bool m_enable;
        private bool m_onlyValidFaults;
        private int m_timeWindow;

        #endregion
        #region [ Properties ]


        /// <summary>
        /// Indicates whether to push on just faults
        /// </summary>
        [Setting]
        [DefaultValue(true)]
        public bool OnlyValidFaults
        {
            get
            {
                return m_onlyValidFaults;
            }
            set
            {
                m_onlyValidFaults = value;
            }
        }

        /// <summary>
        /// Indicates whether operation is enabled.
        /// </summary>
        [Setting]
        [DefaultValue(false)]
        public bool Enabled
        {
            get
            {
                return m_enable;
            }
            set
            {
                m_enable = value;
            }
        }


        /// <summary>
        /// Indicates the hours to push
        /// </summary>
        [Setting]
        [DefaultValue(72)]
        public int TimeWindow
        {
            get
            {
                return m_timeWindow;
            }
            set
            {
                m_timeWindow = value;
            }
        }

        #endregion


    }
}
