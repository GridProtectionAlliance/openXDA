//******************************************************************************************************
//  Setting.cs - Gbtc
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

using System;
using System.ComponentModel.DataAnnotations;
using GSF.ComponentModel.DataAnnotations;
using GSF.Data.Model;

namespace openXDA.Model
{
    public class Setting
    {
        [PrimaryKey(true)]
        public int ID { get; set; }

        [Searchable]
        public string Name { get; set; }

        [Searchable]
        public string Value { get; set; }

        [Searchable]
        public string DefaultValue { get; set; }
    }

    [TableName("DashSettings")]
    public class DashSettings
    {
        [PrimaryKey(true)]
        public int ID { get; set; }

        [Required]
        [StringLength(500)]
        [Searchable]
        public string Name { get; set; }

        [Required]
        [StringLength(500)]
        [Searchable]
        public string Value { get; set; }

        [Required]
        public bool Enabled { get; set; }
    }

    [PrimaryLabel("Name")]
    [TableName("UserDashSettings")]
    public class UserDashSettings
    {
        [PrimaryKey(true)]
        public int ID { get; set; }

        [Required]
        [Label("User Account")]
        public Guid UserAccountID { get; set; }

        [Required]
        [Searchable]
        public string Name { get; set; }

        [Required]
        [Searchable]
        public string Value { get; set; }

        public bool Enabled { get; set; }
    }
}