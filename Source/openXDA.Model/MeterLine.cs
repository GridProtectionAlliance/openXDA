using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GSF.Data.Model;

namespace openXDA.Model
{
    public class MeterLine
    {
        [PrimaryKey(true)]
        public int ID { get; set; }

        public int MeterID { get; set; }

        public int LineID { get; set; }

        [StringLength(200)]
        [Searchable]
        public string LineName { get; set; }
    }

    public class MeterLineDetail : MeterLine
    {
        [Searchable]
        public string MeterName { get; set; }
        [Searchable]
        public string LineKey { get; set; }
    }
}
