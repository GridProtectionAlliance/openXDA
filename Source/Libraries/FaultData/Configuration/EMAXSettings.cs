//******************************************************************************************************
//  EMAXSettings.cs - Gbtc
//
//  Copyright © 2017, Grid Protection Alliance.  All Rights Reserved.
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
//  09/25/2017 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System.ComponentModel;
using System.Configuration;

namespace FaultData.Configuration
{
    public class EMAXSettings
    {
        #region [ Members ]

        // Fields
        private bool m_applyTimestampCorrection;
        private bool m_applyValueCorrection;

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets or sets the value that indicates whether timestamp
        /// correction should be applied when reading native EMAX files.
        /// </summary>
        [Setting]
        [DefaultValue(true)]
        public bool ApplyTimestampCorrection
        {
            get
            {
                return m_applyTimestampCorrection;
            }
            set
            {
                m_applyTimestampCorrection = value;
            }
        }

        /// <summary>
        /// Gets or sets the value that indicates whether value
        /// correction should be applied when reading native EMAX files.
        /// </summary>
        [Setting]
        [DefaultValue(true)]
        public bool ApplyValueCorrection
        {
            get
            {
                return m_applyValueCorrection;
            }
            set
            {
                m_applyValueCorrection = value;
            }
        }

        #endregion
    }
}
