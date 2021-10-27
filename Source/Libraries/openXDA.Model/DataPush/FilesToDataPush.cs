﻿//******************************************************************************************************
//  FilesToDataPush.cs - Gbtc
//
//  Copyright © 2021, Grid Protection Alliance.  All Rights Reserved.
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
//  10/27/2021 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************


using System;
using System.ComponentModel.DataAnnotations;
using GSF.Data.Model;

namespace openXDA.Model
{
    public class FilesToDataPush
    {
        [PrimaryKey(true)]
        public int ID { get; set; }
        public DateTime DataStartTime { get; set; }
        public bool ProcessingError { get; set; }
        public string LargestFile { get; set; }
        public int FilesInGroup { get; set; }
        public int Events { get; set; }
        public bool Synced { get; set; }
    }
}
