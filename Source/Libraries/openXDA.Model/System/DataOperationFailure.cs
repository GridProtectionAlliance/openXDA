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
using System.Linq;
using System.Text;
using GSF;
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

        [ParentKey(typeof(FileGroupAnalysisJob))]
        public int FileGroupAnalysisJobID { get; set; }

        public string Log
        {
            get => Log_;
            set => Log_ = TruncateUTF8(value, 1000);
        }
        private string Log_ { get; set; }

        public string StackTrace { get; set; }

        public DateTime TimeOfFailure { get; set; }

        private static string TruncateUTF8(string str, int maxLength)
        {
            if (Encoding.UTF8.GetMaxByteCount(str.Length) <= maxLength)
                return str;

            if (Encoding.UTF8.GetByteCount(str) <= maxLength)
                return str;

            // If the first 1000 characters are all in the ASCII range,
            // there are no multibyte characters in the string
            // so the number of characters is the same as the number of bytes
            if (str.Take(1000).All(c => c <= 0x7F))
                return str.TruncateRight(maxLength);

            byte[] bytes = Encoding.UTF8.GetBytes(str);
            int end = maxLength;

            // Scan backwards looking for the first byte
            // of the first character beyond maxLength
            while ((bytes[end] & 0xC0) == 0x80)
                end--;

            return Encoding.UTF8.GetString(bytes, 0, end);
        }
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
