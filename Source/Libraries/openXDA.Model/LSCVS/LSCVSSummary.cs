//******************************************************************************************************
//  LSCVSSummary.cs - Gbtc
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
//
//******************************************************************************************************

using GSF.Data.Model;
using System;
using System.ComponentModel.DataAnnotations;

namespace LSCVS.Model
{
    /// <summary>
    /// LSCVS Summary Model. Represents a collection of Events that corresponds to a single row in the LSCVS Report.
    /// </summary>
    public class LSCVSSummary
    {
        [PrimaryKey(true)]
        public int ID { get; set; }
        public bool Reviewed { get; set; }
        public int ParentEventID { get; set; }
        public bool ParentEventITOA { get; set; }
        public bool SARFI80 { get; set; }
        public bool SelfCaused { get; set; }
        public bool ErroneouslyTriggered { get; set; }
        public double InstantenousImpact { get; set; }
        public DateTime PreEvent { get; set; }
        public DateTime PostEvent { get; set; }
        public double Impact { get; set; }


    }
}