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

        // Constants
        public const string CategoryName = "Breakers";

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets or sets the flag that determines whether to apply additional logic
        /// to help obtain more accurate breaker timing results in cases where DC
        /// current gradually drains from the line after the breaker is open.
        /// </summary>
        [Setting]
        [DefaultValue(false)]
        public bool ApplyDCOffsetLogic { get; set; }

        /// <summary>
        /// Gets or sets the size of the window, in cycles,
        /// to use when applying the DC offset logic.
        /// </summary>
        [Setting]
        [DefaultValue(9.0D / 8.0D)]
        public double DCOffsetWindowSize { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of cycles that a breaker
        /// operation's timing can exceed the configured breaker speed.
        /// </summary>
        [Setting]
        [DefaultValue(0.0D)]
        public double LateBreakerThreshold { get; set; }

        /// <summary>
        /// Gets or sets the minimum number of cycles that the breaker is expected
        /// to remain closed after receiving the trip coil energized signal.
        /// </summary>
        /// <remarks>
        /// This value helps to prevent phase timing calculations when the current
        /// signal is not large enough to detect the point at which the breaker opened.
        /// </remarks>
        [Setting]
        [DefaultValue(0.0D)]
        public double MinCyclesBeforeOpen { get; set; }

        /// <summary>
        /// Gets or sets the minimum amount of time, in cycles, the system must wait
        /// before automatically reclosing after a breaker operation has occurred.
        /// </summary>
        [Setting]
        [DefaultValue(15.0D)]
        public double MinWaitBeforeReclose { get; set; }

        /// <summary>
        /// Gets or sets the maximum RMS current, in amps,
        /// at which the breaker can be considered open.
        /// </summary>
        [Setting]
        [DefaultValue(20.0D)]
        public double OpenBreakerThreshold { get; set; }

        /// <summary>
        /// Gets or sets the minimum duration, in cycles, for which the current must remain
        /// at zero in order for a subsequent current spike to be considered a restrike.
        /// </summary>
        [Setting]
        [DefaultValue(0.125D)]
        public double MinCyclesBeforeRestrike { get; set; }

        /// <summary>
        /// Gets or sets the maximum duration, in cycles, for which the current must remain
        /// at zero in order for a subsequent current spike to be considered a restrike.
        /// </summary>
        [Setting]
        [DefaultValue(2.0D)]
        public double MaxCyclesBeforeRestrike { get; set; }

        #endregion
    }
}
