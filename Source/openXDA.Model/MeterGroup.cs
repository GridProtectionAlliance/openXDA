using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GSF.Data.Model;

namespace openXDA.Model
{

    public class MeterGroup
    {
        [Required]
        [PrimaryKey(true)]
        public int ID { get; set; }

        [Required]
        [StringLength(100)]
        [Searchable]
        public string Name { get; set; }

    }
}
