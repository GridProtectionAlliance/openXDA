//******************************************************************************************************
//  DefaultAlarmRangeLimit.cs - Gbtc
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
    [TableName("DefaultAlarmRangeLimit")]
    public class DefaultAlarmRangeLimit
    {
        [PrimaryKey(true)]
        public int ID { get; set; }

        [Required]
        public int MeasurementTypeID { get; set; }

        [Required]
        public int MeasurementCharacteristicID { get; set; }

        [Required]
        public int AlarmTypeID { get; set; }

        [Required]
        public int Severity { get; set; }

        public float High { get; set; }

        public float Low { get; set; }

        [Required]
        public int RangeInclusive { get; set; }

        [Required]
        public int PerUnit { get; set; }
    }

    [TableName("DefaultAlarmRangeLimitView")]
    public class DefaultAlarmRangeLimitView : DefaultAlarmRangeLimit
    {
        [Searchable]
        public string AlarmType { get; set; }

        [Searchable]
        public string MeasurementCharacteristic { get; set; }

        [Searchable]
        public string MeasurementType { get; set; }
    }
}
