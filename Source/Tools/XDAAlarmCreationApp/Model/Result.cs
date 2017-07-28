//******************************************************************************************************
//  Result.cs - Gbtc
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
//  05/04/2017 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************

using GSF.Data.Model;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Data;
using System.ComponentModel;

namespace XDAAlarmCreationApp.Model
{
    public class Result
    {
        [PrimaryKey(true)]
        public int ID { get; set; }
        [Searchable]
        public string Name { get; set; }
        public int NodeRFileID { get; set; }
        public int NodeZFileID { get; set; }
        public int NodeLDGFileID { get; set; }
        public int LineFileID { get; set; }
        public bool Post { get; set; }

        [NonRecordField]
        public string Directory { get; set; }
        [NonRecordField]
        [Required]
        public string FilePrefix { get; set; }
    }
}
