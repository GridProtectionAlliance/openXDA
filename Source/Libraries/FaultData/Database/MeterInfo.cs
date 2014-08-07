using System.Collections.Generic;
using System.Linq;

namespace FaultData.Database
{
    partial class Line
    {
        public IEnumerable<MeterLocation> MeterLocations
        {
            get
            {
                return MeterLocationLines.Select(mll => mll.MeterLocation);
            }
        }
    }

    partial class MeterLocation
    {
        public IEnumerable<Line> Lines
        {
            get
            {
                return MeterLocationLines.Select(mll => mll.Line);
            }
        }
    }
}
