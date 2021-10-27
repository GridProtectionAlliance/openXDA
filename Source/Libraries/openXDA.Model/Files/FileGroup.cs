﻿//******************************************************************************************************
//  FileGroup.cs - Gbtc
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
using System.Collections.Generic;
using GSF.Data;
using GSF.Data.Model;

namespace openXDA.Model
{
    [Serializable]
    public class FileGroup
    {
        [PrimaryKey(true)]
        public int ID { get; set; }

        public int MeterID { get; set; }

        [FieldDataType(System.Data.DbType.DateTime2, DatabaseType.SQLServer)]
        public DateTime DataStartTime { get; set; }

        [FieldDataType(System.Data.DbType.DateTime2, DatabaseType.SQLServer)]
        public DateTime DataEndTime { get; set; }

        [FieldDataType(System.Data.DbType.DateTime2, DatabaseType.SQLServer)]
        public DateTime ProcessingStartTime { get; set; }

        [FieldDataType(System.Data.DbType.DateTime2, DatabaseType.SQLServer)]
        public DateTime ProcessingEndTime { get; set; }

        public int ProcessingVersion { get; set; }

        public int Error { get; set; }

        [NonRecordField]
        public List<DataFile> DataFiles { get; set; } = new List<DataFile>();

        public void AddFieldValue(AdoDataConnection connection, string name, string value, string description = null)
        {
            TableOperations<FileGroupField> fileGroupFieldTable = new TableOperations<FileGroupField>(connection);
            FileGroupField fileGroupField = fileGroupFieldTable.GetOrAdd(name, description);

            TableOperations<FileGroupFieldValue> fileGroupFieldValueTable = new TableOperations<FileGroupFieldValue>(connection);
            FileGroupFieldValue fileGroupFieldValue = new FileGroupFieldValue();
            fileGroupFieldValue.FileGroupID = ID;
            fileGroupFieldValue.FileGroupFieldID = fileGroupField.ID;
            fileGroupFieldValue.Value = value;
            fileGroupFieldValueTable.AddNewRecord(fileGroupFieldValue);
        }

        public void AddOrUpdateFieldValue(AdoDataConnection connection, string name, string value, string description = null)
        {
            TableOperations<FileGroupField> fileGroupFieldTable = new TableOperations<FileGroupField>(connection);
            FileGroupField fileGroupField = fileGroupFieldTable.GetOrAdd(name, description);

            TableOperations<FileGroupFieldValue> fileGroupFieldValueTable = new TableOperations<FileGroupFieldValue>(connection);
            RecordRestriction fileGroupRestriction = new RecordRestriction("FileGroupID = {0}", ID);
            RecordRestriction fileGroupFieldRestriction = new RecordRestriction("FileGroupFieldID = {0}", fileGroupField.ID);
            RecordRestriction queryRestriction = fileGroupRestriction & fileGroupFieldRestriction;

            FileGroupFieldValue fileGroupFieldValue = fileGroupFieldValueTable.QueryRecord(queryRestriction) ?? new FileGroupFieldValue()
            {
                FileGroupID = ID,
                FileGroupFieldID = fileGroupField.ID
            };

            fileGroupFieldValue.Value = value;
            fileGroupFieldValueTable.AddNewOrUpdateRecord(fileGroupFieldValue);
        }
    }
}
