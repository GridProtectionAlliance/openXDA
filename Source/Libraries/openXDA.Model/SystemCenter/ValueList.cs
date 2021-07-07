﻿//******************************************************************************************************
//  ValueList.cs - Gbtc
//
//  Copyright © 2018, Grid Protection Alliance.  All Rights Reserved.
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
//  09/10/2018 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************

using GSF.ComponentModel.DataAnnotations;
using GSF.Data.Model;

namespace SystemCenter.Model
{
    [ TableName("ValueList"), UseEscapedName, PrimaryLabel("Text"), SettingsCategory("systemSettings")]
    public class ValueList
    {

        [PrimaryKey(true)]
        public int ID { get; set; }

        [ParentKey(typeof(ValueListGroup))]
        public int GroupID { get; set; }
        public string Value { get; set; }
        public string AltValue { get; set; }
        public int SortOrder { get; set; }

    }
}