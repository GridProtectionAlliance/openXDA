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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaultData.Configuration
{
    class DataPusherSettings
    {
        #region [ Members ]

        // Fields
        private bool m_onlyValidFaults;

        #endregion
        #region [ Properties ]


        /// <summary>
        /// Indicates whether to use the default fault detection logic
        /// when the line-specific fault detection logic fails.
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

        #endregion


    }
}
