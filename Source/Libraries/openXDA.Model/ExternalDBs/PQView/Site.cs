﻿//******************************************************************************************************
//  Site.cs - Gbtc
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
//  06/15/2021 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************

using GSF.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PQView.Model
{
    [TableName("Site"), SettingsCategory("dbPQView")]
    public class Site
    {
        [PrimaryKey(true)]
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int rank { get; set; }
        public int parentID { get; set; }
        public int uctOffset { get; set; }
        public int dst { get; set; }
        public int connectionTypeID { get; set; }
        public double nominalBaseV { get; set; }
        public int nominalFundFreq { get; set; }
        //public DateTime upsize_ts { get; set; }

    }
}
