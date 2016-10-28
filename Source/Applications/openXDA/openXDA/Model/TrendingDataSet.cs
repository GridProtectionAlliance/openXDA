using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace openXDA.Model
{
    public class TrendingDataSet
    {
        public TrendingDataPoint[] ChannelData;
        public TrendingAlarmLimit[] AlarmLimits;
        public TrendingAlarmLimit[] OffNormalLimits;
    }

    public class TrendingDataPoint
    {
        public double Time;
        public double Maximum;
        public double Minimum;
        public double Average;
    }

    public class TrendingAlarmLimit
    {
        public double TimeStart;
        public double TimeEnd;
        public double? High;
        public double? Low;
    }
}