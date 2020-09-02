//******************************************************************************************************
//  CBCapBankResult.cs - Gbtc
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
    public class CBCapBankResult
    {
        [PrimaryKey(true)]
        public int ID { get; set; }

        /// <summary>
        /// Corresponding <see cref="CBAnalyticResult"/>
        /// </summary>
        public int CBResultID { get; set; }

        /// <summary>
        /// A flag for denoting an abnormal health condition of capacitor and relay (see <see cref="CBBankHealth"/>for more details)
        /// </summary>
        public int CBBankHealthID {get; set;}

        /// <summary>
        /// The operation type of the capacitor switching event, <see cref="CBOperation"/> for more details
        /// </summary>
        public int CBOperationID { get; set; }

        /// <summary>
        /// Identifier of a bank in service alone, i.e., no other bank is in service except BankInsvcAlone. 
        /// For example, BankInsvcAlone = n where n = 1, 2, …, numBanks. For n = 2, it means bank 2 is in service alone.
        /// Note that this is the only Bank that is being analyzed 
        /// </summary>
        public int BankInService { get; set; }

     
        /// <summary>
        /// Voltage of the relay protecting bank <see cref="BankInService"/> when it is in service alone
        /// </summary>
        public double? Vrelay { get; set; }

        /// <summary>
        /// The neutral current magnitude of bank <see cref="BankInService"/> when it is in service alone
        /// </summary>
        public double? In { get; set; }

        /// <summary>
        /// Zero-sequence voltage of the bus when bank <see cref="BankInService"/> in service alone
        /// </summary>
        public double? V0 { get; set; }

        /// <summary>
        /// Zero-sequence impedance of the bus when bank <see cref="BankInService"/> is in service alonedouble
        /// </summary>
        public double? Z0 { get; set; }

        /// <summary>
        /// Reactance of the LV capacitor of the relay protecting bank <see cref="BankInService"/>
        /// </summary>
        public double? XLV { get; set; }

        /// <summary>
        /// Reactance of bank <see cref="BankInService"/>
        /// </summary>
        public double? X { get; set; }

        /// <summary>
        /// Compensation factor of the relay protection cap bank <see cref="BankInService"/>
        /// </summary>
        public double? Kfactor { get; set; }

        /// <summary>
        /// Differential voltage of the relay protection cap bank <see cref="BankInService"/>
        /// /// </summary>
        public double? dV { get; set; }

        /// <summary>
        /// Reactance of the upper groups of bank <see cref="BankInService"/>
        /// </summary>
        public double? XUG { get; set; }

        /// <summary>
        /// Reactance of the lower groups of bank <see cref="BankInService"/>
        /// </summary>
        public double? XLG { get; set; }

        /// <summary>
        /// Ratio of mismatch between V and X for bank <see cref="BankInService"/>
        /// </summary>
        public double? XVmiss { get; set; }

        /// <summary>
        /// IEC voltage unbalanced factor of cap bank <see cref="BankInService"/>
        /// </summary>
        public double? VUIEC { get; set; }

        /// <summary>
        /// Voltage unbalanced factor (IEEE) when bank <see cref="BankInService"/> is in service alone
        /// </summary>
        public double? VUIEEE { get; set; }
    } 
}
