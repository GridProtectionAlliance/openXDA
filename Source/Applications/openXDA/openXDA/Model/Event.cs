using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GSF.Data.Model;

namespace openXDA.Model
{
    public class Event
    {
        [PrimaryKey(true)]
        public int ID { get; set; }
        public int FileGroupID { get; set; }
        public int MeterID { get; set; }
        public int LineID { get; set; }
        public int EventTypeID { get; set; }
        public int EventDataID { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public string ShortName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Samples { get; set; }
        public int TimeZoneOffset { get; set; }
        public int SamplesPerSecond { get; set; }
        public string Description { get; set; }
    }

    public class EventView : Event
    {
        public string LineName { get; set; }
        public string MeterName { get; set; }
        public double Length { get; set; }
        public string EventTypeName { get; set; }
    }
}