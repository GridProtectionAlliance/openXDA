using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GSF.Data.Model;

namespace openXDA.Model
{
    [TableName("LineView")]
    public class LineView: Lines
    {
        public string TopName { get; set; }
    }
}
