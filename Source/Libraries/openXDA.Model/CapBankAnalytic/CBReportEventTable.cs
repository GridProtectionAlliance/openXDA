//******************************************************************************************************
//  CBReportEventTable.cs - Gbtc
//
//  Copyright © 2020, Grid Protection Alliance.  All Rights Reserved.
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
//  08/14/2020 - C. Lackner
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using GSF.Data;
using GSF.Data.Model;

namespace openXDA.Model
{
    public class CBReportEventTable
    {
        [PrimaryKey(true)]
        public int ID { get; set; }

        public int CapBankID { get; set; }

        public int EventID { get; set; }

        [StringLength(200)]
        public string Phase { get; set; }

        [StringLength(500)]
        public string Status { get; set; }

        public int? DataErrorID { get; set; }

        [StringLength(500)]
        public string Operation { get; set; }

        public double? DeltaQ { get; set; }

        public double? MVAsc { get; set; }

        public bool? IsRes { get; set; }


        [FieldDataType(DbType.DateTime2, DatabaseType.SQLServer)]
        public DateTime Time { get; set; }

    } 
}
