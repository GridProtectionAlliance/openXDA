//******************************************************************************************************
//  AciveSubscription.cs - Gbtc
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
//  07/26/2022 - C Lackner
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using GSF.Data.Model;
using GSF.Identity;

namespace openXDA.Model
{
    [AllowSearch]
    public class ActiveSubscription
    {
        public int UserAccountEmailID { get; set; }

        [ParentKey(typeof(ConfirmableUserAccount))]
        public Guid UserAccountID { get; set; }
        public bool Approved { get; set; }
        public string AssetGroup { get; set; }
        public string EmailName { get; set; }
        public string Category { get; set; }
        public int EmailTypeID { get; set; }
        public string Subject { get; set; }
        public DateTime? LastSent { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool RequireApproval { get; set; }
    }
}
