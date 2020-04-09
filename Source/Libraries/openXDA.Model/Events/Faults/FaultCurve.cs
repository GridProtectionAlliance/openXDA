//******************************************************************************************************
//  FaultCurve.cs - Gbtc
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
//  09/06/2017 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System.ComponentModel.DataAnnotations;
using GSF.Data.Model;

namespace openXDA.Model
{
    public class FaultCurve
    {
        [PrimaryKey(true)]
        public int ID { get; set; }

        public int EventID { get; set; }

        [StringLength(200)]
        public string Algorithm { get; set; }

        public byte[] Data { get; set; }

        public byte[] AngleData { get; set; }
    }

    public class FaultCurveStatistic
    {
        [PrimaryKey(true)]
        public int ID { get; set; }

        public int FaultCurveID { get; set; }

        public int FaultNumber { get; set; }

        public double Maximum { get; set; }

        public double Minimum { get; set; }
        
        public double Average { get; set; }

        public double StandardDeviation { get; set; }
    }
}
