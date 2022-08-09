//******************************************************************************************************
//  PQIEquipment.cs - Gbtc
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
   
    public class PQIEquipment: PQIModel
    {
        /// <summary>
        /// Path to query this equipment
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Path to query the facility audit in which the equipment was audited
        /// </summary>
        public string FacilityAudit { get; set; }

        /// <summary>
        /// Path to query the areas in which the equipment resides
        /// </summary>
        public string Areas { get; set; }

        /// <summary>
        /// Path to query the audit curves assigned to the equipment
        /// </summary>
        public string AuditCurves { get; set; }

        /// <summary>
        /// Title of the equipment
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Rank of the equipment
        /// </summary>
        public int Rank { get; set; }

        /// <summary>
        /// Content of the equipment
        /// </summary>
        public string Content { get; set; }

    }
}
