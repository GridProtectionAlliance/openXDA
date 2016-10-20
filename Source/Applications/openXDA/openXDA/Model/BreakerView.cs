using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GSF.Data.Model;

namespace openXDA.Model
{
    public class BreakerView
    {
        [PrimaryKey]
        public int ID { get; set; }
        public int MeterID { get; set; }
        public int EventID { get; set; }
        public string EventType { get; set; }
        public string Energized { get; set; }
        public int BreakerNumber { get; set; }
        public string LineName { get; set; }
        public string PhaseName { get; set; }
        public double Timing { get; set; }
        public int Speed { get; set; }
        public string OperationType { get; set; }

    }
}