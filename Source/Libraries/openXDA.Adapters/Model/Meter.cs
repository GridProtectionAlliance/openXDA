using System;
using System.Linq;
using GSF.ComponentModel.DataAnnotations;
using GSF.Data.Model;

namespace openXDA.Adapters.Model
{
    public class Meter
    {
        [PrimaryKey(true)]
        public int ID { get; set; }

        public string AssetKey { get; set; }

        [Label("Location")]
        public int MeterLocationID { get; set; }

        [Searchable]
        public string Name { get; set; }

        public string Alias { get; set; }

        public string ShortName { get; set; }

        public string Make { get; set; }

        public string Model { get; set; }

        public string TimeZone { get; set; }

        public string Description { get; set; }
    }

    public class MeterDetail : Meter
    {
        public string Location { get; set; }

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
