//******************************************************************************************************
//  BreakerRestrike.cs - Gbtc
//
//  Copyright © 2019, Grid Protection Alliance.  All Rights Reserved.
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
//  08/30/2019 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Data;
using GSF.Data;
using GSF.Data.Model;

namespace openXDA.Model
{
    public class BreakerRestrike
    {
        [PrimaryKey(true)]
        public int ID { get; set; }

        public int EventID { get; set; }

        public int PhaseID { get; set; }

        public int Sample { get; set; }

        [FieldDataType(DbType.DateTime2, DatabaseType.SQLServer)]
        public DateTime Timestamp { get; set; }

        public int InitialExtinguishSample { get; set; }

        [FieldDataType(DbType.DateTime2, DatabaseType.SQLServer)]
        public DateTime InitialExtinguishTime { get; set; }
        public float InitialExtinguishVoltage { get; set; }
        public int RestrikeSample { get; set; }

        [FieldDataType(DbType.DateTime2, DatabaseType.SQLServer)]
        public DateTime RestrikeTime { get; set; }
        public float RestrikeVoltage { get; set; }
        public float RestrikeCurrentPeak { get; set; }
        public float RestrikeVoltageDip { get; set; }
        public int TransientPeakSample { get; set; }

        [FieldDataType(DbType.DateTime2, DatabaseType.SQLServer)]
        public DateTime TransientPeakTime { get; set; }
        public float TransientPeakVoltage { get; set; }
        public float PerUnitTransientPeakVoltage { get; set; }
        public int FinalExtinguishSample { get; set; }

        [FieldDataType(DbType.DateTime2, DatabaseType.SQLServer)]
        public DateTime FinalExtinguishTime { get; set; }
        public float FinalExtinguishVoltage { get; set; }
        public float I2t { get; set; }

    }
}
