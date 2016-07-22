using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GSF.Data.Model;

namespace openXDA.Model
{
    [TableName("AlarmRangeLimitView")]
    public class AlarmRangeLimitView
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

        public string MeasurementType { get; set; }
        public int MeasurementTypeID { get; set; }
        public string MeasurementCharacteristic { get; set; }
        public int MeasurementCharacteristicID { get; set; }
        public string Phase { get; set; }
        public int PhaseID { get; set; }
        public int HarmonicGroup { get; set; }
        public string Name { get; set; }

    }
}
