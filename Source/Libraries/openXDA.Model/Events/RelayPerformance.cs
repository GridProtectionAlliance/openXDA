//******************************************************************************************************
//  RelayPerformance.cs - Gbtc
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
//  07/10/2019 - Christoph Lackner
//       Generated original version of source code.
//  08/20/2021 - Christoph Lackner
//       Added additional Trip Coil Curve points.
//
//******************************************************************************************************

using GSF.Data;
using GSF.Data.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openXDA.Model
{
    public class RelayPerformance
    {
        [PrimaryKey(true)]
        public int ID { get; set; }
        public int EventID { get; set; }
        public int ChannelID { get; set; }
        public double? Imax1 { get; set; }
        public int? Tmax1 { get; set; }
        public double? Imax2 { get; set; }
        public int? TplungerLatch { get; set; }
        public double IplungerLatch { get; set; }
        public double? Idrop { get; set; }
        public int? TiDrop { get; set; }
        public int? Tend { get; set; }

        [FieldDataType(DbType.DateTime2, DatabaseType.SQLServer)]
        public DateTime? TripInitiate { get; set; }
        public int? TripTime { get; set; }
        public int? PickupTime { get; set; }
        public double? TripTimeCurrent { get; set;}
        public double? PickupTimeCurrent { get; set; }
        public double? TripCoilCondition { get; set; }
        public int TripCoilConditionTime { get; set; }
        public int? ExtinctionTimeA { get; set; }
        public int? ExtinctionTimeB { get; set; }
        public int? ExtinctionTimeC { get; set; }
        public double? I2CA { get; set; }
        public double? I2CB { get; set; }
        public double? I2CC { get; set; }

    }
}
