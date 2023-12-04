//******************************************************************************************************
//  AdditionalField.cs - Gbtc
//
//  Copyright © 2019, Grid Protection Alliance.  All Rights Reserved.
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
//  09/20/2019 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************

using GSF.Data.Model;

namespace SystemCenter.Model
{
    [UseEscapedName]
    [PatchRoles("Administrator, Transmission SME")]
    [PostRoles("Administrator, Transmission SME")]
    [DeleteRoles("Administrator, Transmission SME")]
    [AllowSearch]
    public class AdditionalField
    {
        [PrimaryKey(true)]
        public int ID { get; set; }
        public string ParentTable { get; set; }
        public string FieldName { get; set; }
        public string Type { get; set; }
        [ParentKey(typeof(extDBTables))]
        public int? ExternalDBTableID { get; set; }
        public bool IsSecure { get; set; }
        public bool IsInfo { get; set; }
        public bool IsKey { get; set; }
        public bool Searchable { get; set; }
    }

    [TableName("AdditionalField"),
    CustomView(@"
        Select
	        AdditionalField.ID,
	        AdditionalField.ParentTable,
	        AdditionalField.FieldName,
	        AdditionalField.Type,
	        AdditionalField.IsSecure,
	        AdditionalField.Searchable,
	        AdditionalField.IsInfo,
	        AdditionalField.IsKey,
            AdditionalField.ExternalDBTableID,
	        ExternalDatabases.Name as ExternalDB,
	        extDBTables.TableName as ExternalTable
        From
	        AdditionalField LEFT JOIN
	        extDBTables ON AdditionalField.ExternalDBTableID = extDBTables.ID LEFT JOIN
	        ExternalDatabases ON extDBTables.ExtDBID = ExternalDatabases.ID
    "), AllowSearch]
    public class AdditionalFieldView : AdditionalField
    {
        public string ExternalDB { get; set; }
        public string ExternalTable { get; set; }
    }
}