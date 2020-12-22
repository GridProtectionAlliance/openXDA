//******************************************************************************************************
//  DataAnalysisSection.cs - Gbtc
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
//  01/16/2021 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System.ComponentModel;
using System.Configuration;

namespace openXDA.Configuration
{
    public class DataAnalysisSection
    {
        public const string CategoryName = "DataAnalysis";

        /// <summary>
        /// Gets or sets the threshold, in amps, at which the
        /// current exceeds engineering reasonableness.
        /// </summary>
        [Setting]
        [DefaultValue(0.1D)]
        public double InterruptionThreshold { get; set; }

        /// <summary>
        /// Gets or sets the units of measure to use
        /// for lengths (line length and fault distance).
        /// </summary>
        [Setting]
        [DefaultValue("miles")]
        public string LengthUnits { get; set; }

        /// <summary>
        /// Gets or sets the threshold, in amps, at which the
        /// current exceeds engineering reasonableness.
        /// </summary>
        [Setting]
        [DefaultValue(1000000.0D)]
        public double MaxCurrent { get; set; }

        /// <summary>
        /// Gets or sets the maximum duration, in seconds,
        /// of the events processed by openXDA.
        /// </summary>
        [Setting]
        [DefaultValue(0.0D)]
        public double MaxEventDuration { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of hours beyond the current system time
        /// before the time of the record indicates that the data is unreasonable.
        /// </summary>
        [Setting]
        [DefaultValue(0.0D)]
        public double MaxTimeOffset { get; set; }

        /// <summary>
        /// Gets or sets the per-unit threshold at which the
        /// voltage exceeds engineering reasonableness.
        /// </summary>
        [Setting]
        [DefaultValue(2.0D)]
        public double MaxVoltage { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of hours prior to the current system time
        /// before the time of the record indicates that the data is unreasonable.
        /// </summary>
        [Setting]
        [DefaultValue(0.0D)]
        public double MinTimeOffset { get; set; }

        /// <summary>
        /// Gets or sets the threshold, in amps, at which the
        /// current exceeds engineering reasonableness.
        /// </summary>
        [Setting]
        [DefaultValue(0.9D)]
        public double SagThreshold { get; set; }

        /// <summary>
        /// Gets or sets the threshold, in amps, at which the
        /// current exceeds engineering reasonableness.
        /// </summary>
        [Setting]
        [DefaultValue(1.1D)]
        public double SwellThreshold { get; set; }

        /// <summary>
        /// Gets or sets the system frequency.
        /// </summary>
        [Setting]
        [DefaultValue(60.0D)]
        public double SystemFrequency { get; set; }

        /// <summary>
        /// Gets or sets the maximum distance, in seconds,
        /// between a meter's clock and real time.
        /// </summary>
        [Setting]
        [DefaultValue(0.5D)]
        public double TimeTolerance { get; set; }
    }
}
