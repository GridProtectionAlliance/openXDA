//******************************************************************************************************
//  extDatabases.cs - Gbtc
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
//  07/28/2023 - C. Lackner
//       Generated original version of source code.
//
//******************************************************************************************************

using GSF.Data.Model;
using System;

namespace SystemCenter.Model
{
    /// <summary>
    /// Model used to grab Database (e.g. FAWG or Maximo)
    /// </summary>
    [UseEscapedName, TableName("ExternalDatabases")]
    [PostRoles("Administrator, Transmission SME")]
    [PatchRoles("Administrator, Transmission SME")]
    [GetRoles("Administrator, Transmission SME")]
    [AllowSearch]
    public class ExternalDatabases
    {
        [PrimaryKey(true)]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Schedule { get; set; }
        public string ConnectionString { get; set; }
        public string DataProviderString { get; set; }
        public bool Encrypt { get; set; }
        public DateTime? LastDataPull { get; set; }
    }
    [CustomView(@"
    SELECT DISTINCT
	    ExternalDatabases.ID,
	    ExternalDatabases.Name,
	    ExternalDatabases.Schedule,
	    ExternalDatabases.ConnectionString,
	    ExternalDatabases.DataProviderString,
	    ExternalDatabases.Encrypt,
        ExternalDatabases.LastDataPull,
        COUNT(DISTINCT extDBTables.ID) as MappedTables,
        COUNT(DISTINCT AdditionalField.ID) as MappedFields
    FROM
	    ExternalDatabases LEFT JOIN
	    extDBTables ON ExternalDatabases.ID = extDBTables.ExtDBID LEFT JOIN
	    AdditionalField ON extDBTables.ID = AdditionalField.ExternalDBTableID
	GROUP BY
	    ExternalDatabases.ID,
	    ExternalDatabases.Name,
	    ExternalDatabases.Schedule,
	    ExternalDatabases.ConnectionString,
	    ExternalDatabases.DataProviderString,
	    ExternalDatabases.Encrypt,
        ExternalDatabases.LastDataPull
    ")]
    public class DetailedExternalDatabases : ExternalDatabases

    {
        [ViewOnlyField]
        public int? MappedTables { get; set; }
        [ViewOnlyField]
        public int? MappedFields { get; set; }
    }
}
