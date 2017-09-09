//******************************************************************************************************
//  BreakerOperation.cs - Gbtc
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
    public class BreakerOperation
    {
        [PrimaryKey(true)]
        public int ID { get; set; }

        public int EventID { get; set; }

        public int PhaseID { get; set; }

        public int BreakerOperationTypeID { get; set; }

        public string BreakerNumber { get; set; }

        public DateTime TripCoilEnergized { get; set; }

        public DateTime StatusBitSet { get; set; }

        public bool StatusBitChatter { get; set; }

        public DateTime APhaseCleared { get; set; }

        public DateTime BPhaseCleared { get; set; }

        public DateTime CPhaseCleared { get; set; }

        public double BreakerTiming { get; set; }

        public double StatusTiming { get; set; }

        public double APhaseBreakerTiming { get; set; }

        public double BPhaseBreakerTiming { get; set; }

        public double CPhaseBreakerTiming { get; set; }

        public bool DcOffsetDetected { get; set; }

        public double BreakerSpeed { get; set; }

        public string UpdatedBy { get; set; }
    }

    [TableName("BreakerOperation")]
    public class BreakersForDay : BreakerOperation { }

    public class BreakerView
    {
        [PrimaryKey(true)]
        public int ID { get; set; }

        public int MeterID { get; set; }

        public int EventID { get; set; }

        public string EventType { get; set; }

        public string Energized { get; set; }

        [Searchable]
        public int BreakerNumber { get; set; }

        [Searchable]
        public string LineName { get; set; }

        public string PhaseName { get; set; }

        public double Timing { get; set; }

        public int Speed { get; set; }

        [Searchable]
        public string OperationType { get; set; }

        public string UpdatedBy { get; set; }
    }
}