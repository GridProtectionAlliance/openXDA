using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GSF.Data.Model;

namespace openXDA.Model
{
    [TableName("CycleDataSOEPointView")]
    public class CycleDataSOEPointView
    {
        [PrimaryKey(true)]
        public int ID { get; set; }
        public string Name { get; set; }
        public int MeterID { get; set; }
        public DateTime Timestamp { get; set; }
        public float Vmin { get; set; }
        public float Imax { get; set; }
        public string StatusElement { get; set; }
        public string BreakerElementA { get; set; }
        public string BreakerElementB { get; set; }
        public string BreakerElementC { get; set; }
        public string UpState { get; set; }
        public string DownState { get; set; }
        public string Phasing { get; set; }
        public int IncidentID { get; set; }
        public int ParentID { get; set; }
        public int EventID { get; set; }

    }
}