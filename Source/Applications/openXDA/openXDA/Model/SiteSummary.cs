using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GSF.Data.Model;

namespace openXDA.Model
{
    public class SiteSummary
    {
        [PrimaryKey]
        public int MeterID { get; set; }
        public double Completeness { get; set; }
        public double Correctness { get; set; }
        public int Events { get; set; }
        public int Disturbances { get; set; }
        public int Faults { get; set; }
        public double MaxCurrent { get; set; }
        public double MinVoltage { get; set; }

    }
}