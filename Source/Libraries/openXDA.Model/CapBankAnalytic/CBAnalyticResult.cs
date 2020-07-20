//******************************************************************************************************
//  CBAnalyticResult.cs - Gbtc
//
//  Copyright © 2020, Grid Protection Alliance.  All Rights Reserved.
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
//  07/20/2020 - C. Lackner
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Data;
using GSF.Data;
using GSF.Data.Model;

namespace openXDA.Model
{
    public class CBAnalyticResult
    {
        [PrimaryKey(true)]
        public int ID { get; set; }

        public int EventID { get; set; }

        public int PhaseID { get; set; }

        /// <summary>
        /// A flag summarizing the conclusion of TCSAM analysis , <see cref="CBStatus"/> for more details
        /// </summary>
        public int? CBStatusID { get; set; }

        /// <summary>
        /// A flag for input and capacitor data error, <see cref="CBDataError"/> for more details
        /// </summary>
        public int? DataErrorID { get; set; }

        /// <summary>
        /// The operation type of the capacitor switching event, <see cref="CBOperation"/> for more details
        /// </summary>
        public int? CBOperationID { get; set; }

        /// <summary>
        /// The identifier of the de-energized capacitor bank. 
        /// For example,bankDeEnerg = n where n = 1, 2, …, numBanks. For n = 2, it means bank #2 is de-energized. 
        /// (see bankDeEnerg table for more details)
        /// </summary>
        public int? DeEnergizedBanks { get; set; }

        /// <summary>
        /// The identifier of the energized capacitor bank. For example,bankEnerg = n where n = 1, 2, …, numBanks.
        /// For n = 2, it means bank #2 is energized. (see bankEnerg for more details)
        /// </summary>
        public int? EnergizedBanks { get; set; }

        /// <summary>
        /// The identifier of a bank in service alone, i.e., no other bank is in service except bankInsvcAlone.
        /// For example, bankInsvcAlone = u where u = 1, 2, …, numBanks. For u = 2, it means bank #2 is in service alone.
        /// (see bankInvsAlone table for more details)
        /// </summary>
        public int? InServiceBank { get; set; }

        /// <summary>
        /// hange in reactive power (end minus beginning) in kvar. 
        /// Positive value means a capacitor bank is de-energized. 
        /// Negative value means a capacitor bank is energized.
        /// </summary>
        public double? DeltaQ { get; set; }

        /// <summary>
        /// Current in Arms before energizing (beginning)
        /// </summary>
        public double? Ipre { get; set; }

        /// <summary>
        /// Current in Arms after energizing operation completes (end)
        /// </summary>
        public double? Ipost { get; set; }

        /// <summary>
        /// Line-to-neutral rms voltage before energizing in pu (beginning)
        /// #ToDo: Documentation not clear if [V] or [pu]. check once it is working
        /// </summary>
        public double? Vpre { get; set; }

        /// <summary>
        /// Line-to-neutral rms voltage after energizing operation completes in pu (end)
        /// #ToDo: Documentation not clear if [V] or [pu]. check once it is working
        /// </summary>
        public double? Vpost { get; set; }

        /// <summary>
        /// Short-circuit apparent power in MVA. For energized bank, its value is positive, for de-energized bank, its value is negative
        /// </summary>
        public double? MVAsc { get; set; }

        /// <summary>
        /// A flag indicating a resonance condition; 0 = no resonance, 1 = resonance exists
        /// isRes = 1 if dTHDi > iTHDLimit. isRes is invoked only when the switching operation is energizing. 
        /// For all other switching operations, isRes = NULL
        /// </summary>
        public bool? IsRes { get; set; }

        /// <summary>
        /// Frequency during the resonance condition. 
        /// It only gives a result for an energizing event only, e.g., 480 Hz, else NULL
        /// </summary>
        public double? ResFreq { get; set; }

        /// <summary>
        /// THD of current on the basis of the fundamental before the switching op.
        /// </summary>
        public double? THDpre { get; set; }

        /// <summary>
        /// THD of current on the basis of the fundamental after the switching op.
        /// </summary>
        public double? THDpost { get; set; }

        /// <summary>
        /// THD of voltage on the basis of the fundamental before the switching op.
        /// </summary>
        public double? THDVpre { get; set; }

        /// <summary>
        /// THD of voltage on the basis of the fundamental after the switching op.
        /// </summary>
        public double? THDVpost { get; set; }

        /// <summary>
        /// The number of capacitor banks in service before the switching operation
        /// </summary>
        public int? StepPre { get; set; }

        /// <summary>
        /// The number of capacitor banks in service after the switching operation
        /// </summary>
        public int? StepPost { get; set; }

        /// <summary>
        /// The public int? StepPre { get; set; }
        /// </summary>
        public double? SwitchingFreq { get; set; }

        /// <summary>
        /// The peak voltage of the voltage waveform
        /// </summary>
        public double? Vpeak { get; set; }

        /// <summary>
        /// The bank reactance before energizing
        /// </summary>
        public double? Xpre { get; set; }

        /// <summary>
        /// The bank reactance after energizing
        /// </summary>
        public double? Xpost { get; set; }

        /// <summary>
        /// The Time Stamp
        /// </summary>
        [FieldDataType(DbType.DateTime2, DatabaseType.SQLServer)]
        public DateTime Time { get; set; }

        /// <summary>
        /// The energizing time instant
        /// </summary>
        public double? Toffset { get; set; }

    } 
}
