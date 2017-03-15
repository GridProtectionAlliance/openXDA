using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openXDA.Adapters.Model
{
    public class Disturbance
    {
        public int ID { get; set; }
        public int EventID { get; set; }
        public int EventTypeID { get; set; }
        public int PhaseID { get; set; }
        public double Magnitude { get; set; }
        public double PerUnitMagnitude { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public double DurationSeconds { get; set; }
        public double DurationCycles { get; set; }
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
    }
}
