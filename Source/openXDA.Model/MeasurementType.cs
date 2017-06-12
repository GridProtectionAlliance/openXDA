using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GSF.Data.Model;

namespace openXDA.Model
{
    public class MeasurementType
    {
        [PrimaryKey(true)]
        public int ID { get; set; }
        [StringLength(200)]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
