﻿//******************************************************************************************************
//  ChannelsWithNormalLimits.cs - Gbtc
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
//  09/12/2017 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************

using GSF.Data.Model;

namespace openXDA.Model
{
    public class ChannelsWithNormalLimits
    {

        [PrimaryKey(true)]
        public int ID { get; set; }
        [Searchable]
        public string Name { get; set; }
        [Searchable]
        public string MeasurementCharacteristic { get; set; }
        [Searchable]
        public string MeasurementType { get; set; }
        public int MeterID { get; set; }
        public int HarmonicGroup { get; set; }
        [Searchable]
        public string Phase { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public bool RangeInclusive { get; set; }
        public bool PerUnit { get; set; }
        public bool Enabled { get; set; }
        public bool IsDefault { get; set; }

        public string ToCSV()
        {
            string csv = $"{ID},{Name},{MeasurementCharacteristic},{MeasurementType},{MeterID},{HarmonicGroup},{Phase},{High},{Low},{RangeInclusive},{PerUnit},{Enabled},{IsDefault}";
            return csv;
        }
    }

}
