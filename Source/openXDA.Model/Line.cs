using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GSF.Data.Model;

namespace openXDA.Model
{
    public class Line
    {
        [PrimaryKey(true)]
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        [Searchable]
        public string AssetKey { get; set; }

        [Required]
        public float VoltageKV { get; set; }

        [Required]
        public float ThermalRating { get; set; }

        [Required]
        public float Length { get; set; }

        public string Description { get; set; }
    }
}
