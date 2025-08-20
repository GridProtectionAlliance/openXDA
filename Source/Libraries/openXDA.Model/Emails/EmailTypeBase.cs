//******************************************************************************************************
//  EmailTypeBase.cs - Gbtc
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
//  10/03/2022 - Gabriel Santos
//       Generated original version of source code.
//
//******************************************************************************************************

using System.ComponentModel;
using GSF.Data.Model;

namespace openXDA.Model
{
    public abstract class EmailTypeBase
    {
        [PrimaryKey(true)]
        public int ID { get; set; }
        [ParentKey(typeof(EmailCategory))]
        public int EmailCategoryID { get; set; }
        [DefaultValue(false)]
        public bool RequireApproval { get; set; }
        [DefaultValue(true)]
        public bool ShowSubscription { get; set; }
        public string Name { get; set; }
        public string Template { get; set; }
        public string FilePath { get; set; }
        public bool SMS { get; set; }
        [DefaultValue("SELECT 0")]
        public string TriggerEmailSQL { get; set; }
    }
}
