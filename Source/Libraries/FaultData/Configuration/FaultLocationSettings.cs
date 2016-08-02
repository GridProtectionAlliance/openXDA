//******************************************************************************************************
//  FaultLocationSettings.cs - Gbtc
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
    public class FaultLocationSettings
    {
        #region [ Members ]

        // Fields
        private double m_prefaultTrigger;
        private double m_maxFaultDistanceMultiplier;
        private double m_minFaultDistanceMultiplier;
        private bool m_warnMissingDetectionLogic;
        private bool m_useDefaultFaultDetectionLogic;

        #endregion

        #region [ Properties ]

        [Setting]
        [DefaultValue(5.0D)]
        public double PrefaultTrigger
        {
            get
            {
                return m_prefaultTrigger;
            }
            set
            {
                m_prefaultTrigger = value;
            }
        }

        [Setting]
        [DefaultValue(1.05D)]
        public double MaxFaultDistanceMultiplier
        {
            get
            {
                return m_maxFaultDistanceMultiplier;
            }
            set
            {
                m_maxFaultDistanceMultiplier = value;
            }
        }

        [Setting]
        [DefaultValue(-0.05D)]
        public double MinFaultDistanceMultiplier
        {
            get
            {
                return m_minFaultDistanceMultiplier;
            }
            set
            {
                m_minFaultDistanceMultiplier = value;
            }
        }

        [Setting]
        [DefaultValue(false)]
        public bool WarnMissingDetectionLogic
        {
            get
            {
                return m_warnMissingDetectionLogic;
            }
            set
            {
                m_warnMissingDetectionLogic = value;
            }
        }

        /// <summary>
        /// Indicates whether to use the default fault detection logic
        /// when the line-specific fault detection logic fails.
        /// </summary>
        [Setting]
        [DefaultValue(true)]
        public bool UseDefaultFaultDetectionLogic
        {
            get
            {
                return m_useDefaultFaultDetectionLogic;
            }
            set
            {
                m_useDefaultFaultDetectionLogic = value;
            }
        }

        #endregion
    }
}
