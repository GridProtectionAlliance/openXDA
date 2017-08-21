using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using GSF.ComponentModel.DataAnnotations;
using GSF.Data.Model;

namespace openXDA.Model
{
    public class Meter
    {
        [PrimaryKey(true)]
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string AssetKey { get; set; }

        [Required]
        [Label("Location")]
        public int MeterLocationID { get; set; }

        [Required]
        [Searchable]
        [StringLength(200)]
        public string Name { get; set; }

        [StringLength(200)]
        public string Alias { get; set; }

        [StringLength(12)]
        public string ShortName { get; set; }

        [Required]
        [Searchable]
        [StringLength(200)]
        public string Make { get; set; }

        [Required]
        [Searchable]
        [StringLength(200)]
        public string Model { get; set; }

        [Label("Meter Type")]
        public int MeterTypeID { get; set; }

        [StringLength(200)]
        public string TimeZone { get; set; }

        public string Description { get; set; }
    }

    public class MeterDetail : Meter
    {
        [Searchable]
        public string Location { get; set; }
        [Searchable]
        public string MeterType { get; set; }

        public string TimeZoneLabel
        {
            get
            {
                try
                {
                    if (TimeZone != "UTC")
                        return TimeZoneInfo.FindSystemTimeZoneById(TimeZone).ToString();
                }
                catch
                {
                    // Do not fail if the time zone cannot be found --
                    // instead, fall through to the logic below to
                    // find the label for UTC
                }

                return TimeZoneInfo.GetSystemTimeZones()
                    .Where(info => info.Id == "UTC")
                    .DefaultIfEmpty(TimeZoneInfo.Utc)
                    .First()
                    .ToString();
            }
        }
    }
}
