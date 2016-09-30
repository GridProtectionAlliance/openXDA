using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GSF.Data.Model;

namespace JSONApi
{
    public class MeterLocation
    {
        [PrimaryKey(true)]
        public int ID { get; set; }

        public string AssetKey { get; set; }

        public string Name { get; set; }

        public string Alias { get; set; }

        public string ShortName { get; set; }

        public float Latitude { get; set; }

        public float Longitude { get; set; }

        public string Description { get; set; }

    }
}
