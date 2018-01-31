﻿//******************************************************************************************************
//  FaultSegment.cs - Gbtc
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
//  06/23/2017 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using GSF.Data.Model;

namespace openXDA.Model
{
    public class FaultSegment
    {
        [PrimaryKey(true)]
        public int ID { get; set; }

        public int EventID { get; set; }

        public int SegmentTypeID { get; set; }

        [FieldDataType(System.Data.DbType.DateTime2, GSF.Data.DatabaseType.SQLServer)]
        public DateTime StartTime { get; set; }

        [FieldDataType(System.Data.DbType.DateTime2, GSF.Data.DatabaseType.SQLServer)]
        public DateTime EndTime { get; set; }

        public int StartSample { get; set; }

        public int EndSample { get; set; }
    }
}
