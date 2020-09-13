//******************************************************************************************************
//  CBSwitchHealthAnalytic.cs - Gbtc
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
    public class CBSwitchHealthAnalytic
    {
        [PrimaryKey(true)]
        public int ID { get; set; }

        /// <summary>
        /// References an associated <see cref="CBAnalyticResult"/>
        /// </summary>
        public int CBResultID { get; set; }

        /// <summary>
        /// A flag for denoting an abnormal pre-insertion closing condition, <see cref="CBSwitchingCondition"/> for more details
        /// </summary>
        public int? CBSwitchingConditionID { get; set; }

        /// <summary>
        /// The estimated resistive element of the pre-insertion switch
        /// </summary>
        public double R { get; set; }

        /// <summary>
        /// The estimated reactance element of the pre-insertion switch
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// Time between the first closing and the switching out of the pre-insertion R and X branch
        /// </summary>
        public double Duration { get; set; }

        /// <summary>
        /// The change of current magnitude with and without the pre-insertion during the energizing
        /// </summary>
        public double I { get; set; }
    } 
}
