//******************************************************************************************************
//  AuditCurve.cs - Gbtc
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

namespace openXDA.PQI
{
   
    public class AuditCurve: PQIModel
    {
        /// <summary>
        /// Path to query this audit curve
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Path to query the equipment this audit curve applies to
        /// </summary>
        public string Equipment { get; set; }

        /// <summary>
        /// Path to query the areas in which the equipment resides
        /// </summary>
        public string Curve { get; set; }

        /// <summary>
        /// Path to query the curve itself
        /// </summary>
        public string AuditCurves { get; set; }

        /// <summary>
        /// The type of the curve (TOLERANCE, PROTECTION, PQDATA)
        /// </summary>
        public string CurveType { get; set; }


    }
}
