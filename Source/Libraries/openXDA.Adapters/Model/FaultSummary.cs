using System;

namespace openXDA.Adapters.Model
{
    public class FaultSummary
    {
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
        public DateTime Inception { get; set; }
        public double DurationSeconds { get; set; }
        public double DurationCycles { get; set; }
        public string FaultType { get; set; }
        public bool IsSelectedAlgorithm { get; set; }
        public bool IsValid { get; set; }
        public bool IsSuppressed { get; set; }
    }
}
