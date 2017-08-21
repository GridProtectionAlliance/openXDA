using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GSF.Data.Model;
using System.ComponentModel.DataAnnotations;

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
