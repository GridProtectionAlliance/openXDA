using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GSF.Data.Model;

namespace openXDA.Model
{
    public class LineLineGroup
    {
        [PrimaryKey(true)]
        public int ID { get; set; }
        public int LineID { get; set; }
        public int LineGroupID { get; set; }
    }

    [PrimaryLabel("MeterName")]
    public class LineLineGroupView: LineLineGroup
    {
        public string LineName { get; set; }
    }
}
