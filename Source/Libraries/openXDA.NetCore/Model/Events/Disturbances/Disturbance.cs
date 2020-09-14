//******************************************************************************************************
//  Disturbance.cs - Gbtc
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
using Gemstone.Data.Model;

namespace OpenXDA.Model
{
    public class Disturbance
    {
        [PrimaryKey(true)]
        public int ID { get; set; }
        public int EventID { get; set; }
        public int EventTypeID { get; set; }
        public int PhaseID { get; set; }
        public double Magnitude { get; set; }
        public double PerUnitMagnitude { get; set; }

        [FieldDataType(System.Data.DbType.DateTime2, Gemstone.Data.DatabaseType.SQLServer)]
        public DateTime StartTime { get; set; }

        [FieldDataType(System.Data.DbType.DateTime2, Gemstone.Data.DatabaseType.SQLServer)]
        public DateTime EndTime { get; set; }

        public double DurationSeconds { get; set; }
        public double DurationCycles { get; set; }
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
        public string UpdatedBy { get; set; }
    }

    [TableName("DisturbanceView")]
    public class DisturbanceView: Disturbance
    {
        public int MeterID { get; set; }
        public int LineID { get; set; }
        public int? SeverityCode { get; set; }

        [Searchable]
        public string MeterName { get; set; }
        [Searchable]
        public string PhaseName { get; set; }
    }

    [TableName("DisturbanceView")]
    public class DisturbancesForDay : DisturbanceView { }

    [TableName("DisturbanceView")]
    public class DisturbancesForMeter : DisturbanceView { }
}