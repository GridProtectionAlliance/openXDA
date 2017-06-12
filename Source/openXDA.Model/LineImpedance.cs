using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GSF.Data.Model;

namespace openXDA.Model
{
    public class LineImpedance
    {
        [PrimaryKey(true)]
        public int ID { get; set; }
        public int LineID { get; set; }
        public float R0 { get; set; }
        public float R1 { get; set; }
        public float X0 { get; set; }
        public float X1 { get; set; }
    }
}
