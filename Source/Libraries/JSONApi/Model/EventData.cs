using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openXDA.Adapters.Model
{
    public class EventData
    {
        public int SeriesID { get; set; }
        public string Characteristic { get; set; }
        public DateTime time { get; set; }
        public double Value { get; set; }
        public int ChannelID { get; set; }
        public int MeterID { get; set; }
        public int LineID { get; set; }
    }
}
