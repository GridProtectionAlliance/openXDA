//******************************************************************************************************
//  DataOperationFailure.cs - Gbtc
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
//  07/24/2017 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using GSF.Data.Model;

namespace openXDA.Model
{
    [PostRoles("")]
    [PatchRoles("")]
    [DeleteRoles("Administrator, Transmission SME")]
    [AllowSearch]
    public class DataOperationFailure
    {
        [PrimaryKey(true)]
        public int ID { get; set; }

        public int DataOperationID { get; set; }

        [ParentKey(typeof(FileGroup))]
        public int FileGroupID { get; set; }

        public string Log { get; set; }

        public string StackTrace { get; set; }

        public DateTime TimeOfFailure { get; set; }
    }

    [CustomView(@"
        SELECT
	        DataOperationFailure.*,
	        DataOperation.TypeName as DataOperationTypeName
        FROM
	        DataOperationFailure JOIN 
	        DataOperation ON DataOperationFailure.DataOperationID = DataOperation.ID
    ")]
    public class DataOperationFailureDetails : DataOperationFailure
    {
        public string DataOperationTypeName { get; set; }
    }
}
