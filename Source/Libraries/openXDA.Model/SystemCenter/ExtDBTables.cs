//******************************************************************************************************
//  extDBTables.cs - Gbtc
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
//  04/10/2020 - Christoph Lackner
//       Generated original version of source code.
//
//******************************************************************************************************

using GSF.Data.Model;

namespace SystemCenter.Model
{
    /// <summary>
    /// Model used to grab tables form an external Database (e.g. FAWG or Maximo)
    /// </summary>
    [UseEscapedName,TableName("extDBTables")]
    [PostRoles("Administrator, Transmission SME")]
    [PatchRoles("Administrator, Transmission SME")]
    [GetRoles("Administrator, Transmission SME")]
    [AllowSearch]
    [CustomView(@"
    SELECT DISTINCT
	    extDBTables.ID,
	    extDBTables.TableName,
	    extDBTables.ExtDBID,
	    extDBTables.Query,
		ExternalDatabases.Name as ExternalDB,
        COUNT(DISTINCT AdditionalField.ID) as MappedFields
    FROM
	    extDBTables LEFT JOIN
	    ExternalDatabases ON ExternalDatabases.ID = extDBTables.ExtDBID LEFT JOIN
	    AdditionalField ON extDBTables.ID = AdditionalField.ExternalDBTableID
	GROUP BY
	    extDBTables.ID,
	    extDBTables.TableName,
	    extDBTables.ExtDBID,
	    extDBTables.Query,
		ExternalDatabases.Name
    ")]
    public class extDBTables
    {
        [PrimaryKey(true)]
        public int ID { get; set; }
        public string TableName { get; set; }
        [ParentKey(typeof(ExternalDatabases))]
        public int ExtDBID { get; set; }
        public string Query { get; set; }
        [NonRecordField]
        public string ExternalDB { get; set; }
        [NonRecordField]
        public int? MappedFields { get; set; }
    }
}
