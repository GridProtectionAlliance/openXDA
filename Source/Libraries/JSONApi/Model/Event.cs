using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openXDA.Adapters.Model
{
    public class Event
    {
        public int ID { get; set; }
        public int FileGroupID { get; set; }
        public int MeterID { get; set; }
        public int LineID { get; set; }
        public int EventTypeID { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public string ShortName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Samples { get; set; }
        public int TimeZoneOffset { get; set; }
        public int SamplesPerSecond { get; set; }
        public int SamplesPerCycle { get; set; }
        public string Description { get; set; }
    }
}
