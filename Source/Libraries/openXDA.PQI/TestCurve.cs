//******************************************************************************************************
//  TestCurve.cs - Gbtc
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
//  08/09/2022 - C. Lackner
//       Generated original version of source code.
//
//******************************************************************************************************

using System;

namespace openXDA.PQI
{
   
    public class TestCurve
    {
        /// <summary>
        /// Path to query this test curve (aka ride-through curve)
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Path to query the component this curve was assigned to
        /// </summary>
        public string Component { get; set; }

        /// <summary>
        /// Path to query the audit curves associated with this test curve
        /// </summary>
        public string AuditCurves { get; set; }

        /// <summary>
        /// Path to query the points in the curve
        /// </summary>
        public string Points { get; set; }

        /// <summary>
        /// Name of the curve
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Description of the curve
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Entity
        /// </summary>
        public string Entity { get; set; }

        /// <summary>
        /// Location
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// Investigator
        /// </summary>
        public string Investigator { get; set; }

        /// <summary>
        /// Date
        /// </summary>
        public DateTime? Date { get; set; }

        /// <summary>
        /// Additional notes about the curve
        /// </summary>
        public string Notes { get; set; }

        /// <summary>
        /// Frequency (Hz) of the voltage
        /// </summary>
        public double? Frequency { get; set; }

        /// <summary>
        /// Voltage level (in volts) during normal conditions
        /// </summary>
        public double? NominalVoltage { get; set; }
    }
}
