//******************************************************************************************************
//  AuditLog.cs - Gbtc
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
//  06/09/2017 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.ComponentModel.DataAnnotations;
using GSF.Data.Model;

namespace openXDA.Model
{
    [TableName("AuditLog")]
    [AmendExpression("Top {count}",TargetExpression = TargetExpression.FieldList,AffixPosition = AffixPosition.Prefix, StatementTypes = StatementTypes.SelectSet)]
    public class AuditLog
    {
        [PrimaryKey(true)]
        public int ID { get; set; }

        [StringLength(200)]
        [Searchable]
        public string TableName { get; set; }

        [StringLength(200)]
        [Searchable]
        public string PrimaryKeyColumn { get; set; }

        [Searchable]
        public string PrimaryKeyValue { get; set; }

        [StringLength(200)]
        [Searchable]
        public string ColumnName { get; set; }

        public string OriginalValue { get; set; }

        public string NewValue { get; set; }

        public bool Deleted { get; set; }

        [StringLength(200)]
        [Searchable]
        public string UpdatedBy { get; set; }

        public DateTime UpdatedOn { get; set; }

    }
}