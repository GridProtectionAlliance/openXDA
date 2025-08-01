﻿//******************************************************************************************************
//  FaultLocationSection.cs - Gbtc
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
//  08/02/2015 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System.ComponentModel;
using System.Configuration;

namespace openXDA.Configuration
{
    public enum FaultCalculationCycleMethod
    {
        MaxCurrent,
        LastFaultedCycle,
        LastFaultedCycleExceptAirGapRes
    }

    public class FaultLocationSection
    {
        #region [ Members ]

        // Constants
        public const string CategoryName = "FaultLocation";

        #endregion

        #region [ Properties ]

        [Setting]
        [DefaultValue(5.0D)]
        public double PrefaultTrigger { get; set; }

        [Setting]
        [DefaultValue(50.0D)]
        public double PrefaultTriggerAdjustment { get; set; }

        [Setting]
        [DefaultValue(1.05D)]
        public double MaxFaultDistanceMultiplier { get; set; }

        [Setting]
        [DefaultValue(-0.05D)]
        public double MinFaultDistanceMultiplier { get; set; }

        [Setting]
        [DefaultValue(FaultCalculationCycleMethod.MaxCurrent)]
        public FaultCalculationCycleMethod FaultCalculationCycleMethod { get; set; }

        [Setting]
        [DefaultValue(1.0D)]
        public double MinFaultSegmentCycles { get; set; }

        [Setting]
        [DefaultValue(10)]
        public int FaultClearingAdjustmentSamples { get; set; }

        [Setting]
        [DefaultValue(false)]
        public bool WarnMissingDetectionLogic { get; set; }

        /// <summary>
        /// Indicates whether to use the default fault detection logic
        /// when the line-specific fault detection logic fails.
        /// </summary>
        [Setting]
        [DefaultValue(true)]
        public bool UseDefaultFaultDetectionLogic { get; set; }

        /// <summary>
        /// Indicates whether to ignore line-specific fault detection logic.
        /// </summary>
        [Setting]
        [DefaultValue(false)]
        public bool IgnoreFaultDetectionLogic { get; set; }

        [Setting]
        [DefaultValue(0.8D)]
        public double FaultedVoltageThreshold { get; set; }

        [Setting]
        [DefaultValue(0.001D)]
        public double GroundedFaultVoltageThreshold { get; set; }

        #endregion
    }
}
