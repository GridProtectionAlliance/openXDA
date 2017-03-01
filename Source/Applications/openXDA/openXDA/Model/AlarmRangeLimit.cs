using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GSF.Data.Model;

namespace openXDA.Model
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

}
