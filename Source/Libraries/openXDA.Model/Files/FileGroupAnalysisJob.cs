//******************************************************************************************************
//  FileGroupAnalysisJob.cs - Gbtc
//
//  Copyright © 2026, Grid Protection Alliance.  All Rights Reserved.
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
//  06/24/2026 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using GSF.Data;
using GSF.Data.Model;

namespace openXDA.Model
{
    public class FileGroupAnalysisJob
    {
        [PrimaryKey(true)]
        public int ID { get; set; }

        [ParentKey(typeof(FileGroup))]
        public int FileGroupID { get; set; }

        [FieldDataType(System.Data.DbType.DateTime2, DatabaseType.SQLServer)]
        public DateTime TaskQueuedTime { get; set; }

        public int TaskPriority { get; set; }

        [FieldDataType(System.Data.DbType.DateTime2, DatabaseType.SQLServer)]
        public DateTime ProcessingStartTime { get; set; }

        [FieldDataType(System.Data.DbType.DateTime2, DatabaseType.SQLServer)]
        public DateTime ProcessingEndTime { get; set; }

        public int ProcessingVersion { get; set; }

        [NonRecordField]
        public FileGroup FileGroup { get; set; }
    }
}
