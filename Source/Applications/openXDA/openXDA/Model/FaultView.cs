using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GSF.Data.Model;

namespace openXDA.Model
{
    public class FaultView
    {
        [PrimaryKey]
        public int ID { get; set; }
        public string MeterName { get; set; }
        public string ShortName { get; set; }
        public string LocationName { get; set; }
        public int MeterID { get; set; }
        public int LineID { get; set; }
        public int EventID { get; set; }
        public string LineName { get; set; }
        public int Voltage { get; set; }
        public DateTime InceptionTime { get; set; }
        public string FaultType { get; set; }
        public double CurrentDistance { get; set; }
        public int RK { get; set; }
    }
}