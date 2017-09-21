//******************************************************************************************************
//  Fault.cs - Gbtc
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
using GSF.Data.Model;

namespace openXDA.Model
{
    [TableName("FaultSummary")]
    public class Fault
    {
        [PrimaryKey(true)]
        public int ID { get; set; }

        public int EventID { get; set; }

        public string Algorithm { get; set; }

        public int FaultNumber { get; set; }

        public int CalculationCycle { get; set; }

        public double Distance { get; set; }

        public double CurrentMagnitude { get; set; }

        public double CurrentLag { get; set; }

        public double PrefaultCurrent { get; set; }

        public double PostfaultCurrent { get; set; }

        public double ReactanceRatio { get; set; }

        [FieldDataType(System.Data.DbType.DateTime2, GSF.Data.DatabaseType.SQLServer)]
        public DateTime Inception { get; set; }

        public double DurationSeconds { get; set; }

        public double DurationCycles { get; set; }

        public string FaultType { get; set; }

        public bool IsSelectedAlgorithm { get; set; }

        public bool IsValid { get; set; }

        public bool IsSuppressed { get; set; }
    }

    public class FaultSummary : Fault { } 

    [TableName("FaultView")]
    public class FaultView : Fault
    {
        [Searchable]
        public string MeterName { get; set; }

        public string ShortName { get; set; }

        [Searchable]
        public string LocationName { get; set; }

        public int MeterID { get; set; }

        public int LineID { get; set; }

        [Searchable]
        public string LineName { get; set; }

        public int Voltage { get; set; }

        public DateTime InceptionTime { get; set; }

        public double CurrentDistance { get; set; }

        public int RK { get; set; }
    }

    [TableName("FaultView")]
    public class FaultForMeter: FaultView { }

    public class FaultsDetailsByDate
    {
        public int thefaultid { get; set; }

        public string thesite { get; set; }

        public string locationname { get; set; }

        public int themeterid { get; set; }

        public int thelineid { get; set; }

        public int theeventid { get; set; }

        public string thelinename { get; set; }

        public int voltage { get; set; }

        public DateTime theinceptiontime { get; set; }

        public string thefaulttype { get; set; }

        public double thecurrentdistance { get; set; }

        public int notecount { get; set; }

        public int rk { get; set; }

        [NonRecordField]
        public string theeventtype { get; set; }
    }
}