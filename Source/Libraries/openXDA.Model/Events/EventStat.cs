﻿//******************************************************************************************************
//  EventStat.cs - Gbtc
//
//  Copyright © 2018, Grid Protection Alliance.  All Rights Reserved.
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
//  11/07/2018 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************

using GSF.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openXDA.Model
{
    public class EventStat
    {
        [PrimaryKey(true)]
        public int ID { get; set; }
        public int EventID { get; set; }
        public double? VPeak { get; set; }
        public double? VAMax { get; set; }
        public double? VBMax { get; set; }
        public double? VCMax { get; set; }
        public double? VABMax { get; set; }
        public double? VBCMax { get; set; }
        public double? VCAMax { get; set; }
        public double? VAMin { get; set; }
        public double? VBMin { get; set; }
        public double? VCMin { get; set; }
        public double? VABMin { get; set; }
        public double? VBCMin { get; set; }
        public double? VCAMin { get; set; }
        public double? IPeak { get; set; }
        public double? IAMax { get; set; }
        public double? IBMax { get; set; }
        public double? ICMax { get; set; }
        public double? IA2t { get; set; }
        public double? IB2t { get; set; }
        public double? IC2t { get; set; }
        public double? InitialMW { get; set; }
        public double? FinalMW { get; set; }
        public int? PQViewID { get; set; }
    }
}
