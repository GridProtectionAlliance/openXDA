//******************************************************************************************************
//  CBRestrikeResult.cs - Gbtc
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
    public class CBRestrikeResult
    {
        [PrimaryKey(true)]
        public int ID { get; set; }

        public int CBResultID { get; set; }

        /// <summary>
        /// A flag summarizing the conclusion of TCSAM analysis , <see cref="CBRestrikeType"/> for more details
        /// </summary>
        public int CBRestrikeTypeID { get; set; }

        /// <summary>
        /// The time instant of the first current extinction
        /// </summary>
        public double? Text { get; set; }

        /// <summary>
        /// The time instant of the first current restrike
        /// </summary>
        public double? Trest { get; set; }

        /// <summary>
        /// The time instant of the second current extinction
        /// </summary>
        public double? Text2 { get; set; }

        /// <summary>
        /// The duration of the restrike (i.e., the current zero gap) in cycle
        /// </summary>
        public double? Drest { get; set; }

        /// <summary>
        /// The instantaneous peak current in kA during the restrike
        /// </summary>
        public double? Vmax { get; set; }

        /// <summary>
        /// The instantaneous voltage in kV during the restrike
        /// </summary>
        public double? Imax { get; set; }


    } 
}
