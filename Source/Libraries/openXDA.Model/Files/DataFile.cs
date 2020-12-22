//******************************************************************************************************
//  DataFile.cs - Gbtc
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
using System.Linq;
using System.Text;
using GSF.Data.Model;
using GSF.IO.Checksums;

namespace openXDA.Model
{
    [TableName("DataFile")]
    public class DataFile
    {
        [PrimaryKey(true)]
        public int ID { get; set; }

        public int FileGroupID { get; set; }

        [Searchable]
        public string FilePath { get; set; }

        public int FilePathHash { get; set; }

        public long FileSize { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime LastWriteTime { get; set; }

        public DateTime LastAccessTime { get; set; }

        [NonRecordField]
        public FileBlob FileBlob { get; set; }

        public static int GetHash(string filePath)
        {
            Encoding utf8 = new UTF8Encoding(false);
            byte[] pathData = utf8.GetBytes(filePath);
            return unchecked((int)Crc32.Compute(pathData, 0, pathData.Length));
        }
    }

    public static partial class TableOperationsExtensions
    {
        public static DataFile QueryDataFile(this TableOperations<DataFile> dataFileTable, string filePath)
        {
            int hashCode = DataFile.GetHash(filePath);
            DataFile dataFile = QueryDataFile(dataFileTable, filePath, hashCode);

            if (dataFile != null)
                return dataFile;

            int legacyHashCode = filePath.GetHashCode();
            dataFile = QueryDataFile(dataFileTable, filePath, legacyHashCode);

            if (dataFile == null)
                return null;

            dataFile.FilePathHash = hashCode;
            dataFileTable.UpdateRecord(dataFile);
            return dataFile;
        }

        private static DataFile QueryDataFile(TableOperations<DataFile> dataFileTable, string filePath, int hashCode)
        {
            IEnumerable<DataFile> dataFiles = dataFileTable.QueryRecordsWhere("FilePathHash = {0}", hashCode);
            return dataFiles.FirstOrDefault(dataFile => dataFile.FilePath == filePath);
        }
    }
}
