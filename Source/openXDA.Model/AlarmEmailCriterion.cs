using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace openXDA.Model
{
    public class AlarmEmailCriterion
    {
        public int ID { get; set; }
        public int EmailGroupID { get; set; }
        public int MeasurementTypeID { get; set; }
        public int MeasurementCharacteristicID { get; set; }
    }
}