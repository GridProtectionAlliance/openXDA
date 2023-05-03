//******************************************************************************************************
//  SubscribeScheduledEmails.cs - Gbtc
//
//  Copyright © 2023, Grid Protection Alliance.  All Rights Reserved.
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
//  04/17/2023 - C. Lackner
//       Generated original version of source code.
//
//******************************************************************************************************

using GSF.Data.Model;

namespace openXDA.Model
{
    [AllowSearch]
    [DeleteRoles("Administrator")]
    [PatchRoles("Administrator")]
    [PostRoles("Administrator")]
    [TableName("SubscribeScheduledEmails")]
    [UseEscapedName]
    public class SubscribeScheduledEmails
    {
        [PrimaryKey()]
        public int ID { get; set; }

        public bool Approved { get; set; }

        [ParentKey(typeof(ScheduledEmailType))]
        public int ScheduledEmailID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string AssetGroup { get; set; }
    }
}
