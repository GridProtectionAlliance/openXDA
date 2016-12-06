using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GSF.Data.Model;

namespace openXDA.Model
{
    [TableName("LineView")]
    public class LineView
    {
        [PrimaryKey(true)]
        public int ID { get; set; }

        [StringLength(50)]
        public string AssetKey { get; set; }

        public float VoltageKV { get; set; }

        public float ThermalRating { get; set; }

        public float Length { get; set; }

        public string Description { get; set; }

        public string TopName { get; set; }

        public int LineImpedanceID { get; set; }
        public float R0 { get; set; }
        public float R1 { get; set; }
        public float X0 { get; set; }
        public float X1 { get; set; }

    }
}
