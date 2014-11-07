using System;
using System.Collections.Generic;
using System.Linq;
using FaultData.DataAnalysis;
using FaultData.Database;
using FaultData.Database.FaultLocationDataTableAdapters;
using FaultData.Database.MeterDataTableAdapters;
using GSF.Collections;

namespace openXDA
{
    public class FaultValidator
    {
        #region [ Members ]

        // Fields
        private string m_connectionString;
        private double m_maxFaultDistanceMultiplier;
        private double m_minFaultDistanceMultiplier;

        private double m_faultDistance;
        private bool m_isFaultDistanceValid;

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Creates a new instance of the <see cref="FaultValidator"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string used to connect to the database.</param>
        public FaultValidator(string connectionString)
        {
            m_connectionString = connectionString;
        }

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets or sets the multiplier used to determine the
        /// maximum valid fault distance based on the line length.
        /// </summary>
        public double MaxFaultDistanceMultiplier
        {
            get
            {
                return m_maxFaultDistanceMultiplier;
            }
            set
            {
                m_maxFaultDistanceMultiplier = value;
            }
        }

        /// <summary>
        /// Gets or sets the multiplier used to determine the
        /// minimum valid fault distance based on the line length.
        /// </summary>
        public double MinFaultDistanceMultiplier
        {
            get
            {
                return m_minFaultDistanceMultiplier;
            }
            set
            {
                m_minFaultDistanceMultiplier = value;
            }
        }

        /// <summary>
        /// Gets the fault distance used to validate the fault record.
        /// </summary>
        public double FaultDistance
        {
            get
            {
                return m_faultDistance;
            }
        }

        /// <summary>
        /// Gets a flag that indicates whether the fault distance used to
        /// validate the fault record indicates that the fault record is valid.
        /// </summary>
        public bool IsFaultDistanceValid
        {
            get
            {
                return m_isFaultDistanceValid;
            }
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Checks to determine whether the event identified
        /// by the given event ID has valid fault data.
        /// </summary>
        /// <param name="eventID">The identifier for the event.</param>
        /// <returns>True if the event has valid fault data; false otherwise.</returns>
        public void Validate(int eventID)
        {
            MeterData.EventRow evt;
            Line line;
            FaultLocationData.CycleDataRow cycleData;

            List<FaultSegment> faultSegments;
            List<DataSeries> faultCurves;
            DataGroup cycleDataGroup;
            VICycleDataSet cycleDataSet;

            DataSeries iSum;
            int largestCurrentIndex;
            List<double> faultDistances;

            using (MeterInfoDataContext meterInfo = new MeterInfoDataContext(m_connectionString))
            using (FaultLocationInfoDataContext faultLocationInfo = new FaultLocationInfoDataContext(m_connectionString))
            using (EventTableAdapter eventAdapter = new EventTableAdapter())
            using (CycleDataTableAdapter cycleDataAdapter = new CycleDataTableAdapter())
            using (FaultCurveTableAdapter faultCurveAdapter = new FaultCurveTableAdapter())
            {
                // Set the connection strings of the LINQ-to-DataSet adapters
                eventAdapter.Connection.ConnectionString = m_connectionString;
                cycleDataAdapter.Connection.ConnectionString = m_connectionString;
                faultCurveAdapter.Connection.ConnectionString = m_connectionString;

                // Get the event for the given event ID
                evt = eventAdapter.GetDataByID(eventID).Single();

                // Get the line on which the event occurred
                line = meterInfo.Lines.Single(l => evt.LineID == l.ID);

                // Get the cycle data and fault curves calculated by the fault location engine
                cycleData = cycleDataAdapter.GetDataBy(eventID).SingleOrDefault();

                if ((object)cycleData == null)
                {
                    m_faultDistance = double.NaN;
                    m_isFaultDistanceValid = false;
                    return;
                }

                faultCurves = faultCurveAdapter.GetDataBy(eventID)
                    .Where(curve => curve.EventID == eventID)
                    .Select(ToDataSeries)
                    .ToList();

                // Extract the cycle data from the blobs stored in the database
                cycleDataGroup = new DataGroup();
                cycleDataGroup.FromData(cycleData.Data);

                // Create a cycle data set to identify what each series is in the cycleDataGroup
                cycleDataSet = new VICycleDataSet(cycleDataGroup);

                // Get the sum of all currents
                iSum = cycleDataSet.IA.RMS
                    .Add(cycleDataSet.IB.RMS)
                    .Add(cycleDataSet.IC.RMS);

                // Find the index of the cycle with the largest current
                largestCurrentIndex = iSum.DataPoints
                    .Select((dataPoint, index) => Tuple.Create(index, dataPoint.Value))
                    .MaxBy(tuple => tuple.Item2).Item1;

                // Create the function that will determine whether a given fault distance is valid
                Func<double, bool> isValidFaultDistance = distance =>
                {
                    double maxFaultDistance = line.Length * m_maxFaultDistanceMultiplier;
                    double minFaultDistance = line.Length * m_minFaultDistanceMultiplier;
                    return (minFaultDistance <= distance) && (distance <= maxFaultDistance);
                };

                // Get a list of faulted segments invalid data
                // which is outside the fault can be excluded
                faultSegments = faultLocationInfo.FaultSegments
                    .Where(segment => segment.EventID == evt.ID)
                    .Where(segment => segment.SegmentType.Name != "Prefault")
                    .Where(segment => segment.SegmentType.Name != "Postfalt")
                    .ToList();

                // Get a list of the valid fault distances
                faultDistances = faultCurves
                    .Select(faultCurve => faultCurve.DataPoints[largestCurrentIndex].Value)
                    .Where((faultDistance, index) => faultSegments.Any(segment => segment.StartSample <= index && index <= segment.EndSample))
                    .Where(isValidFaultDistance)
                    .OrderByDescending(faultDistance => faultDistance)
                    .ToList();

                if (faultDistances.Any())
                {
                    // If any fault distances are valid, use the median value
                    m_faultDistance = faultDistances[faultDistances.Count / 2];
                    m_isFaultDistanceValid = true;
                }
                else
                {
                    // If no fault distances are valid, simply pick the first one and mark as invalid
                    m_faultDistance = faultCurves.First().DataPoints[largestCurrentIndex].Value;
                    m_isFaultDistanceValid = false;
                }
            }
        }

        // Converts the given fault curve to a DataSeries.
        private DataSeries ToDataSeries(FaultLocationData.FaultCurveRow faultCurve)
        {
            DataGroup dataGroup = new DataGroup();
            dataGroup.FromData(faultCurve.Data);
            return dataGroup.DataSeries[0];
        }

        #endregion
    }
}
