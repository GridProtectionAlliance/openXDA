using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GSF.Data.Model;

namespace openXDA.Model
{
    public class MeterMeterGroup
    {
        [PrimaryKey(true)]
        public int ID { get; set; }
        public int MeterID { get; set; }
        public int MeterGroupID { get; set; }
    }

    [PrimaryLabel("MeterName")]
    public class MeterMeterGroupView
    {
        [PrimaryKey(true)]
        public int ID { get; set; }
        public int MeterID { get; set; }
        public int MeterGroupID { get; set; }

        public string MeterName { get; set; }
        public string Location { get; set; }
    }
}
