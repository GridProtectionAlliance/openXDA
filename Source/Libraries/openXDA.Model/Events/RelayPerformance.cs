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
        public double? Imax2 { get; set; }

        [FieldDataType(DbType.DateTime2, DatabaseType.SQLServer)]
        public DateTime? TripInitiate { get; set; }

        [NonRecordField]
        public int? TripTime { get { return this.TripTimeTicks / 10; } set { this.TripTimeTicks = value*10; } }

        [FieldName("TripTime")]
        public int? TripTimeTicks { get; set; }

        [NonRecordField]
        public int? PickupTime { get { return this.PickupTimeTicks / 10; } set { this.PickupTimeTicks = value * 10; } }

        [FieldName("PickupTime")]
        public int? PickupTimeTicks { get; set; }
        public double? TripCoilCondition { get; set; }
    }
}
