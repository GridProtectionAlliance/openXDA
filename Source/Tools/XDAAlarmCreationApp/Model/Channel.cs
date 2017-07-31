using System.ComponentModel.DataAnnotations;
using GSF.Data.Model;

namespace XDAAlarmCreationApp.Model
{
    public class Channel
    {
        [PrimaryKey(true)]
        public int ID { get; set; }
        
        public int MeterID { get; set; }

        public int LineID { get; set; }

        public int MeasurementTypeID { get; set; }

        public int MeasurementCharacteristicID { get; set; }

        public int PhaseID { get; set; }

        [StringLength(200)]
        [Searchable]
        public string Name { get; set; }

        public float SamplesPerHour { get; set; }

        public float PerUnitValue { get; set; }

        public int HarmonicGroup { get; set; }

        public string Description { get; set; }

        public bool Enabled { get; set; }
    }

    public class ChannelDetail : Channel
    {
        public string MeterName { get; set; }

        public string LineKey { get; set; }

        public string LineName { get; set; }

        [Searchable]
        public string MeasurementType { get; set; }
        [Searchable]
        public string MeasurementCharacteristic { get; set; }
        [Searchable]
        public string Phase { get; set; }

        public string Mapping { get; set; }

        public int SeriesTypeID { get; set; }

        public string SeriesType { get; set; }
    }

    [TableName("ChannelInfo")]
    public class ChannelInfo
    {
        [PrimaryKey(true)]
        public int ChannelID { get; set; }
        public string ChannelName { get; set; }
        public string ChannelDescription { get; set; }
        public string MeasurementType { get; set; }
        public string MeasurementCharacteristic { get; set; }
        public string Phase { get; set; }
        public string SeriesType { get; set; }
        public string Orientation { get; set; }
        public string Phasing { get; set; }
    }

    //public class ChannelsForEvent
    //{
    //    [PrimaryKey]

    //}
}