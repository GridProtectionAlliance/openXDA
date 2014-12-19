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

            FaultLocationData.FaultSummaryDataTable faultTable;
            List<FaultLocationData.FaultSummaryRow> faultSummaries;

            using (MeterInfoDataContext meterInfo = new MeterInfoDataContext(m_connectionString))
            using (EventTableAdapter eventAdapter = new EventTableAdapter())
            using (FaultSummaryTableAdapter faultSummaryAdapter = new FaultSummaryTableAdapter())
            {
                // Set the connection strings of the LINQ-to-DataSet adapters
                eventAdapter.Connection.ConnectionString = m_connectionString;
                faultSummaryAdapter.Connection.ConnectionString = m_connectionString;

                // Get the event for the given event ID
                evt = eventAdapter.GetDataByID(eventID).Single();

                // Get the line on which the event occurred
                line = meterInfo.Lines.Single(l => evt.LineID == l.ID);

                // Load fault summary records from the database
                faultTable = faultSummaryAdapter.GetDataBy(evt.ID);

                // Get the valid fault summary records for this event
                faultSummaries = faultTable
                    .Where(faultSummary => faultSummary.DistanceDeviation < 0.5D * line.Length)
                    .Where(faultSummary => faultSummary.LargestCurrentDistance >= MinFaultDistanceMultiplier * line.Length)
                    .Where(faultSummary => faultSummary.LargestCurrentDistance <= MaxFaultDistanceMultiplier * line.Length)
                    .ToList();

                if (faultSummaries.Count > 0)
                {
                    // If any fault summaries are valid, use the median of the
                    // largest current distances for the first fault in the record
                    faultSummaries = faultSummaries
                        .GroupBy(faultSummary => faultSummary.FaultNumber)
                        .MinBy(grouping => grouping.Key)
                        .OrderBy(faultSummary => faultSummary.LargestCurrentDistance)
                        .ToList();

                    m_faultDistance = faultSummaries[faultSummaries.Count / 2].LargestCurrentDistance;
                    m_isFaultDistanceValid = true;
                }
                else if (faultTable.Count > 0)
                {
                    // If no fault distances are valid, simply pick the first one and mark as invalid
                    m_faultDistance = faultTable[0].LargestCurrentDistance;
                    m_isFaultDistanceValid = false;
                }
                else
                {
                    // There are no fault distances to choose from indicated by
                    // setting fault distance to not-a-number and marking as invalid
                    m_faultDistance = double.NaN;
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
