//******************************************************************************************************
//  LSCVSEvent.cs - Gbtc
//
//  Copyright © 2022, Grid Protection Alliance.  All Rights Reserved.
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
//  04/01/2022 - C. Lackner
//       Generated original version of source code.
//  04/11/2022 - G. Santos
//       Added new fields.
//
//******************************************************************************************************

using GSF.Data.Model;
using System;
using System.ComponentModel.DataAnnotations;

namespace LSCVS.Model
{
    /// <summary>
    /// LSCVS Event Model. Models an Event from the openXDA that is relevant for LSCVS Computation
    /// </summary>
    /// <remarks>
    /// This will need to be extended with anything coming from the OpenXDA.
    /// </remarks>
    [PatchRoles("Administrator, API Account")]
    [PostRoles("Administrator, API Account")]
    [DeleteRoles("Administrator, API Account")]
    public class LSCVSEvent
    {
        [PrimaryKey(true)]
        public int ID { get; set; }
        public int OpenXDAID { get; set; }
        public DateTime EventStart { get; set; }
        public double Duration { get; set; }
        public double Magnitude { get; set; }
        public int MeterID { get; set; }
        public int CustomerID { get; set; }
        public int EventType { get; set; }
        public bool InsideCurve { get; set; }
        public double IntialMW { get; set; }
        public double FinalMW { get; set; }
        [NonRecordField]
        public bool SARFI80Flag { get; set; }
    }
}