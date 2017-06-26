using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GSF.Data.Model;

namespace openXDA.Model
{
    [TableName("FaultSummary")]
    public class Fault
    {
        [PrimaryKey(true)]
        public int ID { get; set; }
        public int EventID { get; set; }
        public string Algorithm { get; set; }
        public int FaultNumber { get; set; }
        public int CalculationCycle { get; set; }
        public double Distance { get; set; }
        public double CurrentMagnitude { get; set; }
        public double CurrentLag { get; set; }
        public double PrefaultCurrent { get; set; }
        public double PostfaultCurrent { get; set; }
        [FieldDataType(System.Data.DbType.DateTime2, GSF.Data.DatabaseType.SQLServer)]
        public DateTime Inception { get; set; }
        public double DurationSeconds { get; set; }
        public double DurationCycles { get; set; }
        public string FaultType { get; set; }
        public bool IsSelectedAlgorithm { get; set; }
        public bool IsValid { get; set; }
        public bool IsSuppressed { get; set; }
    }

    public class FaultSummary : Fault { } 

    [TableName("FaultView")]
    public class FaultView : Fault
    {
        [Searchable]
        public string MeterName { get; set; }
        public string ShortName { get; set; }
        [Searchable]
        public string LocationName { get; set; }
        public int MeterID { get; set; }
        public int LineID { get; set; }
        [Searchable]
        public string LineName { get; set; }
        public int Voltage { get; set; }
        public DateTime InceptionTime { get; set; }
        public double CurrentDistance { get; set; }
        public int RK { get; set; }
    }

    [TableName("FaultView")]
    public class FaultForMeter: FaultView { }

}