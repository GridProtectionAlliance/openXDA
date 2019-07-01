//******************************************************************************************************
//  ReportsSettings.cs - Gbtc
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
//  06/14/2018 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************

using System.ComponentModel;
using System.Configuration;

namespace openXDA.Model
{
    public class ReportsSettings
    {
        /// <summary>
        /// Indicates whether operation is enabled.
        /// </summary>
        [Setting]
        [DefaultValue(false)]
        public bool Enabled { get; set; }

        /// <summary>
        /// Indicates whether operation is enabled.
        /// </summary>
        [Setting]
        [DefaultValue("0 0 2 * *")]  // Runs on second day of month to ensure all data is in
        public string Schedule { get; set; }

        [Setting]
        [DefaultValue(99.0D)]
        public double FirstFrequencyPercentile { get; set; }

        [Setting]
        [DefaultValue(0.3D)]
        public double FirstFrequencyDeviationLimit { get; set; }

        [Setting]
        [DefaultValue(99.95D)]
        public double SecondFrequencyPercentile { get; set; }

        [Setting]
        [DefaultValue(0.5D)]
        public double SecondFrequencyDeviationLimit { get; set; }

        [Setting]
        [DefaultValue(99.0D)]
        public double FirstVoltagePercentile { get; set; }

        [Setting]
        [DefaultValue(3.0D)]
        public double FirstVoltageDeviationLimit { get; set; }

        [Setting]
        [DefaultValue(99.95D)]
        public double SecondVoltagePercentile { get; set; }

        [Setting]
        [DefaultValue(5.0D)]
        public double SecondVoltageDeviationLimit { get; set; }

        [Setting]
        [DefaultValue(98.5D)]
        public double FlickerPercentile { get; set; }

        [Setting]
        [DefaultValue(1.0D)]
        public double FlickerHighLimit { get; set; }

        [Setting]
        [DefaultValue(99.95D)]
        public double VoltageUnbalancePercentile { get; set; }

        [Setting]
        [DefaultValue(2.0D)]
        public double VoltageUnbalanceHighLimit { get; set; }

        [Setting]
        [DefaultValue(0.0D)]
        public double VoltageUnbalanceLowLimit { get; set; }

        [Setting]
        [DefaultValue(99.95D)]
        public double VoltageTHDPercentile { get; set; }

        [Setting]
        [DefaultValue(8.0D)]
        public double VoltageTHDHighLimit { get; set; }
    }
}
