using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openXDA.Model
{
    public class TrendingData
    {
        public int ChannelID { get; set; }
        public string SeriesType { get; set; }
        public DateTime Time { get; set; }
        public double Value { get; set; }
    }
}
