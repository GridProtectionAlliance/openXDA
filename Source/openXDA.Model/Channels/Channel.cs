//******************************************************************************************************
//  Channel.cs - Gbtc
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

using System.ComponentModel.DataAnnotations;
using GSF.Data.Model;

namespace openXDA.Model
{
    public class Channel
    {
        [PrimaryKey(true)]
        public int ID { get; set; }
        
        public int MeterID { get; set; }

        public int LineID { get; set; }

        public int MeasurementTypeID { get; set; }

        public int MeasurementCharacteristicID { get; set; }

        public int PhaseID { get; set; }

        [StringLength(200)]
        [Searchable]
        public string Name { get; set; }

        public float SamplesPerHour { get; set; }

        public float PerUnitValue { get; set; }

        public int HarmonicGroup { get; set; }

        public string Description { get; set; }

        public bool Enabled { get; set; }
    }

    public class ChannelDetail : Channel
    {
        public string MeterName { get; set; }

        public string LineKey { get; set; }

        public string LineName { get; set; }

        [Searchable]
        public string MeasurementType { get; set; }

        [Searchable]
        public string MeasurementCharacteristic { get; set; }

        [Searchable]
        public string Phase { get; set; }

        public string Mapping { get; set; }

        public int SeriesTypeID { get; set; }

        public string SeriesType { get; set; }
    }

    [TableName("ChannelInfo")]
    public class ChannelInfo
    {
        [PrimaryKey(true)]
        public int ChannelID { get; set; }

        public string ChannelName { get; set; }

        public string ChannelDescription { get; set; }

        public string MeasurementType { get; set; }

        public string MeasurementCharacteristic { get; set; }

        public string Phase { get; set; }

        public string SeriesType { get; set; }

        public string Orientation { get; set; }

        public string Phasing { get; set; }
    }
}