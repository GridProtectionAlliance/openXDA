using System.Collections.Generic;
using System.Linq;

namespace FaultData.Database
{
    partial class VoltageEnvelope
    {
        public IEnumerable<VoltageCurve> VoltageCurves
        {
            get
            {
                return VoltageEnvelopeCurves.Select(link => link.VoltageCurve);
            }
        }
    }
}