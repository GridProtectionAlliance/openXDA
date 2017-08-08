//******************************************************************************************************
//  AlarmRangeLimit.cs - Gbtc
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
//  06/09/2017 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************

using GSF.ComponentModel.DataAnnotations;
using GSF.Data.Model;

namespace XDAAlarmCreationApp.Model
{
    [TableName("AlarmRangeLimit")]
    public class AlarmRangeLimit
    {
        [PrimaryKey(true)]
        public int ID { get; set; }
        public int ChannelID { get; set; }
        public int AlarmTypeID { get; set; }
        public int Severity { get; set; }
        public float High { get; set; }
        public float Low { get; set; }
        public int RangeInclusive { get; set; }
        public int PerUnit { get; set; }
        public int Enabled { get; set; }
        public bool IsDefault { get; set; }

    }

    [PrimaryLabel("Name")]
    [TableName("AlarmRangeLimitView")]
    public class AlarmRangeLimitView
    {
        [PrimaryKey(true)]
        public int ID { get; set; }

        [Label("Channel ID")]
        public int ChannelID { get; set; }
        public int MeterID { get; set; }
        public int LineID { get; set; }

        [Label("Meter Name")]
        public string MeterName { get; set; }
        [Label("Channel Name")]
        public string Name { get; set; }

        [Label("Alarm Type ID")]
        public int AlarmTypeID { get; set; }
        public int Severity { get; set; }
        public float High { get; set; }
        public float Low { get; set; }

        [Label("Range Inclusive")]
        public int RangeInclusive { get; set; }

        [Label("Per Unit")]
        public int PerUnit { get; set; }
        public int Enabled { get; set; }

        [Label("Measurement Type")]
        public string MeasurementType { get; set; }
        public int MeasurementTypeID { get; set; }

        [Label("Measurement Characteristic")]
        public string MeasurementCharacteristic { get; set; }
        public int MeasurementCharacteristicID { get; set; }
        public string Phase { get; set; }
        public int PhaseID { get; set; }

        [Label("Harmonic Group")]
        public int HarmonicGroup { get; set; }


        public bool IsDefault { get; set; }


        public string csvString()
        {

            string word = ID.ToString() + ',' + ChannelID.ToString() + ',' + Name.Replace(',', '-') + ',' + AlarmTypeID.ToString() + ',' + Severity.ToString() + ',' + High.ToString() + ',' + Low.ToString() + ',' + RangeInclusive.ToString() + ','
                + PerUnit.ToString() + ',' + Enabled.ToString() + ',' + MeasurementType + ',' + MeasurementTypeID.ToString() + ',' + MeasurementCharacteristic + ',' + MeasurementCharacteristicID.ToString() + ',' +
                Phase + ',' + PhaseID.ToString() + ',' + HarmonicGroup.ToString() + ',' + IsDefault.ToString();

            return word;
        }
    }

    public class MetersWithNormalLimits
    {
        [PrimaryKey(true)]
        public int ID { get; set; }
        [Searchable]
        public string Name { get; set; }
        public int Limits { get; set; }

        public string ToCSV()
        {
            string csv = $"{ID},{Name},{Limits}";
            return csv;
        }
    }

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

        public string ToCSV()
        {
            string csv = $"{ID},{Name},{MeasurementCharacteristic},{MeasurementType},{MeterID},{HarmonicGroup},{Phase}, {High}, {Low}";
            return csv;
        }
    }

}
