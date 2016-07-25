using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GSF.Data.Model;

namespace openXDA.Model
{
    [TableName("DefaultAlarmRangeLimitView")]
    public class DefaultAlarmRangeLimitView: DefaultAlarmRangeLimit
    {
        public string AlarmType { get; set; }
        public string MeasurementCharacteristic { get; set; }
        public string MeasurementType { get; set; }
    }
}
