//******************************************************************************************************
//  DERAnalyticResult.cs - Gbtc
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
//  11/09/2021 - Billy Ernest
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
    public class DERAnalyticResult
    {
        [PrimaryKey(true)]
        public int ID { get; set; }
        public int? EventID { get; set; }
        public int MeterID { get; set; }
        public int AssetID { get; set; }
        public int ChannelID { get; set; }

        public string Regulation { get; set; }
        public string Parameter { get; set; }
        public double Threshold { get; set; }
        public double Value { get; set; }
        [UseEscapedName]
        public DateTime Time { get; set; }
        public string DataType { get; set; }

    }
}
