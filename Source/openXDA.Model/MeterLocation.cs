using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GSF.Data.Model;

namespace openXDA.Model
{
    public class MeterLocation
    {
        [PrimaryKey(true)]
        public int ID { get; set; }

        [StringLength(50)]
        [Searchable]
        [Required]
        public string AssetKey { get; set; }

        [StringLength(200)]
        [Searchable]
        [Required]
        public string Name { get; set; }

        [StringLength(200)]
        [Searchable]
        public string Alias { get; set; }

        [StringLength(50)]
        [Searchable]
        public string ShortName { get; set; }

        [Required]
        public float Latitude { get; set; }

        [StringLength(200)]
        [Required]
        public float Longitude { get; set; }

        public string Description { get; set; }

    }
}
