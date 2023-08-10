//******************************************************************************************************
//  MATLABAnalyticEventType.cs - Gbtc
//
//  Copyright © 2023, Grid Protection Alliance.  All Rights Reserved.
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
//  08/04/2023 - Parker Dinsdale
//       Generated original version of source code.
//
//******************************************************************************************************

using System.ComponentModel;
using GSF.Data.Model;

namespace openXDA.Model
{
    [SettingsCategory("systemSettings")]
    [AllowSearch]
    [PostRoles("Administrator")]
    [PatchRoles("Administrator")]
    [DeleteRoles("Administrator")]
    public class MATLABAnalyticEventType
    {
        [PrimaryKey(true)]
        public int ID { get; set; }

        [ParentKey(typeof(MATLABAnalytic))]
        public int MATLABAnalyticID { get; set; }

        [ParentKey(typeof(EventType))]
        public int EventTypeID { get; set; }
    }
}
