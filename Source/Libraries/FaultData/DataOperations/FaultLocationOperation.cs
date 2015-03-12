//******************************************************************************************************
//  FaultLocationOperation.cs - Gbtc
//
//  Copyright © 2014, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the Eclipse Public License -v 1.0 (the "License"); you may
//  not use this file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://www.opensource.org/licenses/eclipse-1.0.php
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  08/04/2014 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using FaultData.DataAnalysis;
using FaultData.Database;
using FaultData.DataResources;
using FaultData.DataSets;
using GSF.Collections;
using log4net;
using EventKey = System.Tuple<int, System.DateTime, System.DateTime>;

namespace FaultData.DataOperations
{
    public class FaultLocationOperation : DataOperationBase<MeterDataSet>
    {
        #region [ Members ]

        // Nested Types
        private class StatCalculator
        {
            #region [ Members ]

            // Fields
            public List<DataPoint> DataPoints;

            public double Maximum;
            public double Minimum;
            public double Average;
            public double StandardDeviation;

            #endregion

            #region [ Methods ]

            public void CalculateStatistics()
            {
                double variance;

                DataPoints.Sort((point1, point2) => point1.Value.CompareTo(point2.Value));
                Maximum = DataPoints[DataPoints.Count - 1].Value;
                Minimum = DataPoints[0].Value;
                Average = DataPoints.Average(dataPoint => dataPoint.Value);

                variance = DataPoints
                    .Select(dataPoint => dataPoint.Value - Average)
                    .Average(diff => diff * diff);

                StandardDeviation = Math.Sqrt(variance);
            }

            #endregion
        }

        private class FaultSummarizer
        {
            #region [ Members ]

            // Fields
            public List<FaultSegment> FaultSegmentTable = new List<FaultSegment>();
            public FaultLocationData.FaultCurveDataTable FaultCurveTable = new FaultLocationData.FaultCurveDataTable();
            public FaultLocationData.FaultSummaryDataTable FaultSummaryTable = new FaultLocationData.FaultSummaryDataTable();

            public MeterDataSet MeterDataSet;
            public DataGroup DataGroup;
            public List<Fault> Faults;

            private List<Tuple<EventKey, FaultSegment>> m_faultSegmentList;
            private List<Tuple<EventKey, FaultLocationData.FaultCurveRow>> m_faultCurveList;
            private List<Tuple<EventKey, FaultLocationData.FaultSummaryRow>> m_faultSummaryList;

            #endregion

            #region [ Constructors ]

            public FaultSummarizer()
            {
                m_faultSegmentList = new List<Tuple<EventKey, FaultSegment>>();
                m_faultCurveList = new List<Tuple<EventKey, FaultLocationData.FaultCurveRow>>();
                m_faultSummaryList = new List<Tuple<EventKey, FaultLocationData.FaultSummaryRow>>();
            }

            #endregion

            #region [ Methods ]

            public void SummarizeFault()
            {
                FaultSegment faultSegment;
                FaultLocationData.FaultCurveRow faultCurveRow;
                FaultLocationData.FaultSummaryRow faultSummaryRow;

                EventKey eventKey;

                int faultNumber;

                eventKey = Tuple.Create(DataGroup.Line.ID, DataGroup.StartTime, DataGroup.EndTime);
                faultNumber = 1;

                foreach (Fault fault in Faults)
                {
                    // Detemrine if the fault is valid
                    if (fault.Info.NumberOfValidDistances == 0)
                        continue;

                    // Create a fault segment for the fault itself
                    faultSegment = CreateFaultSegment(fault);

                    if ((object)faultSegment != null)
                        m_faultSegmentList.Add(Tuple.Create(eventKey, faultSegment));

                    // Create fault segments for each fault type found within the fault
                    foreach (Fault.Segment segment in fault.Segments)
                    {
                        faultSegment = CreateFaultSegment(segment);

                        if ((object)faultSegment != null)
                            m_faultSegmentList.Add(Tuple.Create(eventKey, faultSegment));
                    }

                    // Create a fault summary row for this fault
                    faultSummaryRow = CreateFaultSummaryRow(fault, faultNumber);
                    m_faultSummaryList.Add(Tuple.Create(eventKey, faultSummaryRow));

                    // Increment the fault number
                    faultNumber++;
                }

                // Generate fault curves for each algorithm used to analyze the fault
                if (Faults.Any(fault => fault.Info.NumberOfValidDistances > 0))
                {
                    for (int i = 0; i < Faults[0].Curves.Count; i++)
                    {
                        faultCurveRow = CreateFaultCurveRow(i);
                        m_faultCurveList.Add(Tuple.Create(eventKey, faultCurveRow));
                    }
                }
            }

            private FaultSegment CreateFaultSegment(Fault fault)
            {
                SegmentType segmentType;
                FaultSegment faultSegment;

                // ReSharper disable once InconsistentlySynchronizedField
                if (s_segmentTypeLookup.TryGetValue("Fault", out segmentType))
                {
                    faultSegment = new FaultSegment();
                    faultSegment.SegmentType = segmentType;
                    faultSegment.StartTime = fault.Info.InceptionTime;
                    faultSegment.EndTime = fault.Info.ClearingTime;
                    faultSegment.StartSample = fault.Info.StartSample;
                    faultSegment.EndSample = fault.Info.EndSample;

                    return faultSegment;
                }

                return null;
            }

            private FaultSegment CreateFaultSegment(Fault.Segment segment)
            {
                string segmentTypeName;
                SegmentType segmentType;
                FaultSegment faultSegment;

                segmentTypeName = string.Format("{0} Fault", segment.FaultType).Replace("ABC", "3-Phase");

                // ReSharper disable once InconsistentlySynchronizedField
                if (s_segmentTypeLookup.TryGetValue(segmentTypeName, out segmentType))
                {
                    faultSegment = new FaultSegment();
                    faultSegment.SegmentType = segmentType;
                    faultSegment.StartTime = segment.StartTime;
                    faultSegment.EndTime = segment.EndTime;
                    faultSegment.StartSample = segment.StartSample;
                    faultSegment.EndSample = segment.EndSample;

                    return faultSegment;
                }

                return null;
            }

            private FaultLocationData.FaultSummaryRow CreateFaultSummaryRow(Fault fault, int faultNumber)
            {
                FaultLocationData.FaultSummaryRow faultSummaryRow;
                StatCalculator statCalculator;
                double durationSeconds;

                // Calculate the duration of the fault in seconds
                durationSeconds = fault.Info.Duration.TotalSeconds;

                // Calculate statistics about the fault
                // using fault data from this fault curve
                statCalculator = new StatCalculator();
                statCalculator.DataPoints = fault.Curves[fault.Info.DistanceAlgorithmIndex].Series.DataPoints;
                statCalculator.CalculateStatistics();

                // Create the fault summary record to be written to the database
                faultSummaryRow = FaultSummaryTable.NewFaultSummaryRow();
                faultSummaryRow.Algorithm = fault.Info.DistanceAlgorithm;
                faultSummaryRow.FaultNumber = faultNumber;
                faultSummaryRow.LargestCurrentDistance = fault.Info.Distance;
                faultSummaryRow.MaximumDistance = statCalculator.Maximum;
                faultSummaryRow.MinimumDistance = statCalculator.Minimum;
                faultSummaryRow.AverageDistance = statCalculator.Average;
                faultSummaryRow.DistanceDeviation = statCalculator.StandardDeviation;
                faultSummaryRow.Inception = fault.Info.InceptionTime;
                faultSummaryRow.DurationSeconds = durationSeconds;
                faultSummaryRow.DurationCycles = durationSeconds * Frequency;
                faultSummaryRow.FaultType = fault.Info.Type.ToString();

                return faultSummaryRow;
            }

            private FaultLocationData.FaultCurveRow CreateFaultCurveRow(int curveIndex)
            {
                FaultLocationData.FaultCurveRow faultCurveRow;
                DataSeries series;
                int startSample;
                int endSample;

                // Create the fault curve record to be written to the database
                faultCurveRow = FaultCurveTable.NewFaultCurveRow();
                faultCurveRow.Algorithm = Faults[0].Curves[curveIndex].Algorithm;

                series = DataGroup.DataSeries[0].Multiply(double.NaN);

                foreach (Fault fault in Faults)
                {
                    startSample = fault.Info.StartSample;
                    endSample = startSample + fault.Curves.Min(curve => curve.Series.DataPoints.Count) - 1;

                    if (fault.Info.NumberOfValidDistances == 0)
                        continue;

                    for (int sample = fault.Info.StartSample; sample <= endSample; sample++)
                        series[sample].Value = fault.Curves[curveIndex].Series[sample - fault.Info.StartSample].Value;
                }

                faultCurveRow.Data = Serialize(series);

                return faultCurveRow;
            }

            public void FillTables(DbAdapterContainer dbAdapterContainer)
            {
                MeterData.EventDataTable eventTable;
                Dictionary<EventKey, MeterData.EventRow> eventLookup;
                MeterData.EventRow eventRow;

                eventTable = dbAdapterContainer.EventAdapter.GetDataByFileGroup(MeterDataSet.FileGroup.ID);
                eventLookup = eventTable.Where(evt => evt.MeterID == MeterDataSet.Meter.ID).ToDictionary(CreateEventKey);

                foreach (Tuple<EventKey, FaultSegment> faultSegment in m_faultSegmentList)
                {
                    if (eventLookup.TryGetValue(faultSegment.Item1, out eventRow))
                    {
                        faultSegment.Item2.EventID = eventRow.ID;
                        FaultSegmentTable.Add(faultSegment.Item2);
                    }
                }

                foreach (Tuple<EventKey, FaultLocationData.FaultCurveRow> faultCurve in m_faultCurveList)
                {
                    if (eventLookup.TryGetValue(faultCurve.Item1, out eventRow))
                    {
                        faultCurve.Item2.EventID = eventRow.ID;
                        FaultCurveTable.AddFaultCurveRow(faultCurve.Item2);
                    }
                }

                foreach (Tuple<EventKey, FaultLocationData.FaultSummaryRow> faultSummary in m_faultSummaryList)
                {
                    if (eventLookup.TryGetValue(faultSummary.Item1, out eventRow))
                    {
                        faultSummary.Item2.EventID = eventRow.ID;
                        FaultSummaryTable.AddFaultSummaryRow(faultSummary.Item2);
                    }
                }
            }

            private byte[] Serialize(DataSeries series)
            {
                DataGroup group = new DataGroup();
                group.Add(series);
                return group.ToData();
            }

            private EventKey CreateEventKey(MeterData.EventRow eventRow)
            {
                return Tuple.Create(eventRow.LineID, eventRow.StartTime, eventRow.EndTime);
            }

            #endregion
        }

        // Constants
        // TODO: Hardcoded frequency
        private const double Frequency = 60.0D;

        // Fields
        private DbAdapterContainer m_dbAdapterContainer;

        private double m_maxVoltage;
        private double m_maxCurrent;
        private double m_lowVoltageThreshold;
        private double m_maxLowVoltageCurrent;
        private double m_maxTimeOffset;
        private double m_minTimeOffset;

        private double m_residualCurrentTrigger;
        private double m_phaseCurrentTrigger;
        private double m_prefaultTrigger;
        private double m_faultSuppressionTrigger;
        private double m_maxFaultDistanceMultiplier;
        private double m_minFaultDistanceMultiplier;

        private FaultSummarizer m_faultSummarizer;

        #endregion

        #region [ Properties ]

        public double MaxVoltage
        {
            get
            {
                return m_maxVoltage;
            }
            set
            {
                m_maxVoltage = value;
            }
        }

        public double MaxCurrent
        {
            get
            {
                return m_maxCurrent;
            }
            set
            {
                m_maxCurrent = value;
            }
        }

        public double LowVoltageThreshold
        {
            get
            {
                return m_lowVoltageThreshold;
            }
            set
            {
                m_lowVoltageThreshold = value;
            }
        }

        public double MaxLowVoltageCurrent
        {
            get
            {
                return m_maxLowVoltageCurrent;
            }
            set
            {
                m_maxLowVoltageCurrent = value;
            }
        }

        public double MaxTimeOffset
        {
            get
            {
                return m_maxTimeOffset;
            }
            set
            {
                m_maxTimeOffset = value;
            }
        }

        public double MinTimeOffset
        {
            get
            {
                return m_minTimeOffset;
            }
            set
            {
                m_minTimeOffset = value;
            }
        }

        public double ResidualCurrentTrigger
        {
            get
            {
                return m_residualCurrentTrigger;
            }
            set
            {
                m_residualCurrentTrigger = value;
            }
        }

        public double PhaseCurrentTrigger
        {
            get
            {
                return m_phaseCurrentTrigger;
            }
            set
            {
                m_phaseCurrentTrigger = value;
            }
        }

        public double PrefaultTrigger
        {
            get
            {
                return m_prefaultTrigger;
            }
            set
            {
                m_prefaultTrigger = value;
            }
        }

        public double FaultSuppressionTrigger
        {
            get
            {
                return m_faultSuppressionTrigger;
            }
            set
            {
                m_faultSuppressionTrigger = value;
            }
        }

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

        #endregion

        #region [ Methods ]

        public override void Prepare(DbAdapterContainer dbAdapterContainer)
        {
            if ((object)s_segmentTypeLookup == null)
            {
                lock (SegmentTypeLookupLock)
                {
                    if ((object)s_segmentTypeLookup == null)
                        InitializeSegmentTypeLookup(dbAdapterContainer);
                }
            }

            m_dbAdapterContainer = dbAdapterContainer;
        }

        public override void Execute(MeterDataSet meterDataSet)
        {
            FaultDataResource.Factory faultDataResourceFactory = new FaultDataResource.Factory()
            {
                DbAdapterContainer = m_dbAdapterContainer,
                MaxVoltage = m_maxVoltage,
                MaxCurrent = m_maxCurrent,
                LowVoltageThreshold = m_lowVoltageThreshold,
                MaxLowVoltageCurrent = m_maxLowVoltageCurrent,
                MaxTimeOffset = m_maxTimeOffset,
                MinTimeOffset = m_minTimeOffset,
                ResidualCurrentTrigger = m_residualCurrentTrigger,
                PhaseCurrentTrigger = m_phaseCurrentTrigger,
                PrefaultTrigger = m_prefaultTrigger,
                FaultSuppressionTrigger = m_faultSuppressionTrigger,
                MaxFaultDistanceMultiplier = m_maxFaultDistanceMultiplier,
                MinFaultDistanceMultiplier = m_minFaultDistanceMultiplier
            };

            FaultDataResource faultDataResource = meterDataSet.GetResource(faultDataResourceFactory.Create);
            CycleDataResource cycleDataResource = meterDataSet.GetResource<CycleDataResource>();
            List<Fault> faults;

            Log.Info("Executing operation to load fault location data into the database...");

            m_faultSummarizer = new FaultSummarizer();
            m_faultSummarizer.MeterDataSet = meterDataSet;

            foreach (DataGroup dataGroup in cycleDataResource.DataGroups)
            {
                if (!faultDataResource.FaultLookup.TryGetValue(dataGroup, out faults))
                    continue;

                // Generate summary rows for the fault to be entered into the database
                m_faultSummarizer.DataGroup = dataGroup;
                m_faultSummarizer.Faults = faults;
                m_faultSummarizer.SummarizeFault();
            }
        }

        public override void Load(DbAdapterContainer dbAdapterContainer)
        {
            List<FaultSegment> faultSegmentTable;
            FaultLocationData.FaultCurveDataTable faultCurveTable;
            FaultLocationData.FaultSummaryDataTable faultSummaryTable;

            m_faultSummarizer.FillTables(dbAdapterContainer);

            faultSegmentTable = m_faultSummarizer.FaultSegmentTable;
            faultCurveTable = m_faultSummarizer.FaultCurveTable;
            faultSummaryTable = m_faultSummarizer.FaultSummaryTable;

            if (faultSegmentTable.Count == 0 && faultCurveTable.Count == 0 && faultSummaryTable.Count == 0)
                return;

            Log.Info("Loading fault data into the database...");

            // Submit fault segments to the database
            dbAdapterContainer.FaultLocationInfoAdapter.FaultSegments.InsertAllOnSubmit(faultSegmentTable);
            ClearCache(dbAdapterContainer.FaultLocationInfoAdapter);
            dbAdapterContainer.FaultLocationInfoAdapter.SubmitChanges();

            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(dbAdapterContainer.Connection))
            {
                // Set timeout to infinite
                bulkCopy.BulkCopyTimeout = 0;

                // Submit fault curves to the database
                bulkCopy.DestinationTableName = faultCurveTable.TableName;
                bulkCopy.WriteToServer(faultCurveTable);

                // Submit fault summary records to the database
                bulkCopy.DestinationTableName = faultSummaryTable.TableName;
                bulkCopy.WriteToServer(faultSummaryTable);
            }

            Log.Info(string.Format("Loaded {0} faults into the database.", faultSummaryTable.Count));
        }

        private void InitializeSegmentTypeLookup(DbAdapterContainer dbAdapterContainer)
        {
            Func<string, SegmentType> segmentTypeFactory;

            segmentTypeFactory = name =>
            {
                SegmentType segmentType = new SegmentType()
                {
                    Name = name,
                    Description = name
                };

                dbAdapterContainer.FaultLocationInfoAdapter.SegmentTypes.InsertOnSubmit(segmentType);

                return segmentType;
            };

            s_segmentTypeLookup = dbAdapterContainer.FaultLocationInfoAdapter.SegmentTypes.ToDictionary(segmentType => segmentType.Name);

            s_segmentTypeLookup.GetOrAdd("Fault", segmentTypeFactory);
            s_segmentTypeLookup.GetOrAdd("AN Fault", segmentTypeFactory);
            s_segmentTypeLookup.GetOrAdd("BN Fault", segmentTypeFactory);
            s_segmentTypeLookup.GetOrAdd("CN Fault", segmentTypeFactory);
            s_segmentTypeLookup.GetOrAdd("AB Fault", segmentTypeFactory);
            s_segmentTypeLookup.GetOrAdd("BC Fault", segmentTypeFactory);
            s_segmentTypeLookup.GetOrAdd("CA Fault", segmentTypeFactory);
            s_segmentTypeLookup.GetOrAdd("ABG Fault", segmentTypeFactory);
            s_segmentTypeLookup.GetOrAdd("BCG Fault", segmentTypeFactory);
            s_segmentTypeLookup.GetOrAdd("CAG Fault", segmentTypeFactory);
            s_segmentTypeLookup.GetOrAdd("3-Phase Fault", segmentTypeFactory);

            dbAdapterContainer.FaultLocationInfoAdapter.SubmitChanges();
        }

        private void ClearCache(FaultLocationInfoDataContext faultInfo)
        {
            // TODO: Eliminate the necessity of this using Linq-to-DataSets
            // The cache needs to be cleared before submitting changes to the
            // FaultSegment table because the faultInfo object isn't necessarily
            // aware that Events were inserted since it built its cache
            const BindingFlags Flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            MethodInfo method = faultInfo.GetType().GetMethod("ClearCache", Flags);
            method.Invoke(faultInfo, null);
        }

        #endregion

        #region [ Static ]

        // Static Fields
        private static readonly object SegmentTypeLookupLock = new object();
        private static Dictionary<string, SegmentType> s_segmentTypeLookup;
        private static readonly ILog Log = LogManager.GetLogger(typeof(FaultLocationOperation));

        #endregion
    }
}
