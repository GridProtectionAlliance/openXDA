using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GSF.Data.Model;

namespace openXDA.Model
{
    public class BreakerOperation
    {
        [PrimaryKey(true)]
        public int ID { get; set; }
        public int EventID { get; set; }
        public int PhaseID { get; set; }
        public int BreakerOperationTypeID { get; set; }
        public string BreakerNumber { get; set; }
        public DateTime TripCoilEnergized { get; set; }
        public DateTime StatusBitSet { get; set; }
        public DateTime APhaseCleared { get; set; }
        public DateTime BPhaseCleared { get; set; }
        public DateTime CPhaseCleared { get; set; }
        public double BreakerTiming { get; set; }
        public double APhaseBreakerTiming { get; set; }
        public double BPhaseBreakerTiming { get; set; }
        public double CPhaseBreakerTiming { get; set; }
        public double BreakerSpeed { get; set; }
        public string UpdatedBy { get; set; }
    }

    public class BreakerView
    {
        [PrimaryKey(true)]
        public int ID { get; set; }
        public int MeterID { get; set; }
        public int EventID { get; set; }
        public string EventType { get; set; }
        public string Energized { get; set; }
        public int BreakerNumber { get; set; }
        public string LineName { get; set; }
        public string PhaseName { get; set; }
        public double Timing { get; set; }
        public int Speed { get; set; }
        public string OperationType { get; set; }
        public string UpdatedBy { get; set; }

    }

    public class BreakerOperationType
    {
        [PrimaryKey(true)]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

    }
}