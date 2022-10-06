//******************************************************************************************************
//  EmailType.cs - Gbtc
//
//  Copyright © 2017, Grid Protection Alliance.  All Rights Reserved.
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
//  08/29/2017 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************

using System.ComponentModel;
using GSF.Data.Model;

namespace openXDA.Model
{
    [AllowSearch]
    [DeleteRoles("Administrator")]
    [PatchRoles("Administrator")]
    [PostRoles("Administrator")]
    [TableName("EmailType")]
    [UseEscapedName]
    public class EmailType : BaseEmailType
    {

        [ParentKey(typeof(EmailCategory))]
        public int EmailCategoryID { get; set; }

        public string TriggerEmailSQL { get; set; }

        public string CombineEventsSQL { get; set; }

        public double MinDelay { get; set; }

        public double MaxDelay { get; set; }

        public bool SMS { get; set; }

        public bool ShowSubscription { get; set; }

        [DefaultValue(false)]
        public bool RequireApproval { get; set; }
    }

    [AllowSearch]
    [DeleteRoles("Administrator")]
    [PatchRoles("Administrator")]
    [PostRoles("Administrator")]
    [TableName("EmailTypeView")]
    [UseEscapedName]
    public class EmailTypeView : EmailType
    {
        [Searchable]
        public string EmailCategory { get; set; }
    }
}
