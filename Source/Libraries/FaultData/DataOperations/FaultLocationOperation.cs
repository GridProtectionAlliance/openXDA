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

using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using FaultData.DataAnalysis;
using FaultData.DataResources;
using FaultData.DataSets;
using GSF.Collections;
using GSF.Data;
using GSF.Data.Model;
using log4net;
using openXDA.Model;
using Fault = FaultData.DataAnalysis.Fault;
using FaultGroup = FaultData.DataAnalysis.FaultGroup;

namespace FaultData.DataOperations
{
    public class FaultLocationOperation : DataOperationBase<MeterDataSet>
    {
        #region [ Members ]

        // Nested Types
        private class FaultSummarizer
        {
            #region [ Members ]

            // Fields
            public MeterDataSet MeterDataSet;
            public DataGroup DataGroup;
            public FaultGroup FaultGroup;

            private double m_systemFrequency;

            #endregion

            #region [ Constructors ]

            public FaultSummarizer(double systemFrequency)
            {
                m_systemFrequency = systemFrequency;
            }

            #endregion

            #region [ Methods ]

            public void SummarizeFault()
            {
                using (AdoDataConnection connection = MeterDataSet.CreateDbConnection())
                {
                    TableOperations<Event> eventTable = new TableOperations<Event>(connection);
                    TableOperations<SegmentType> segmentTypeTable = new TableOperations<SegmentType>(connection);

                    TableOperations<openXDA.Model.FaultGroup> faultGroupTable = new TableOperations<openXDA.Model.FaultGroup>(connection);
                    TableOperations<FaultSegment> faultSegmentTable = new TableOperations<FaultSegment>(connection);
                    TableOperations<FaultSummary> faultSummaryTable = new TableOperations<FaultSummary>(connection);

                    Event evt = eventTable.GetEvent(MeterDataSet.FileGroup, DataGroup);
                    SegmentType faultSegmentType = segmentTypeTable.GetOrAdd("Fault");

                    // Create a fault group row for the whole group of faults
                    if (FaultGroup.FaultDetectionLogicResult != false || FaultGroup.FaultValidationLogicResult != false)
                        faultGroupTable.AddNewRecord(CreateFaultGroup(evt.ID, FaultGroup));

                    for (int faultIndex = 0; faultIndex < FaultGroup.Faults.Count; faultIndex++)
                    {
                        Fault fault = FaultGroup.Faults[faultIndex];

                        // Create a fault segment for the fault itself
                        faultSegmentTable.AddNewRecord(CreateFaultSegment(evt.ID, fault, faultSegmentType));

                        // Create fault segments for each fault type found within the fault
                        foreach (Fault.Segment segment in fault.Segments)
                        {
                            string segmentTypeName = string.Format("{0} Fault", segment.FaultType).Replace("ABC", "3-Phase");
                            SegmentType segmentType = segmentTypeTable.GetOrAdd(segmentTypeName);
                            faultSegmentTable.AddNewRecord(CreateFaultSegment(evt.ID, segment, segmentType));
                        }

                        // Create the fault summary rows for this fault
                        foreach (FaultSummary faultSummary in CreateFaultSummaries(evt.ID, faultIndex + 1, fault))
                            faultSummaryTable.AddNewRecord(faultSummary);
                    }

                    // Generate fault curves for each algorithm used to analyze the fault
                    TableOperations<FaultCurve> faultCurveTable = new TableOperations<FaultCurve>(connection);
                    TableOperations<FaultCurveStatistic> faultCurveStatisticTable = new TableOperations<FaultCurveStatistic>(connection);

                    if (FaultGroup.Faults.Any())
                    {
                        for (int i = 0; i < FaultGroup.Faults[0].Curves.Count; i++)
                        {
                            FaultCurve faultCurve = CreateFaultCurve(evt.ID, i);
                            faultCurveTable.AddNewRecord(faultCurve);
                            faultCurve.ID = connection.ExecuteScalar<int>("SELECT @@IDENTITY");

                            for (int faultIndex = 0; faultIndex < FaultGroup.Faults.Count; faultIndex++)
                            {
                                Fault fault = FaultGroup.Faults[faultIndex];

                                if (fault.Curves[i].Series.DataPoints.Count == 0)
                                    continue;

                                FaultCurveStatistic faultCurveStatistic = new FaultCurveStatistic()
                                {
                                    FaultCurveID = faultCurve.ID,
                                    FaultNumber = faultIndex + 1,
                                    Maximum = ToDbFloat(fault.Curves[i].Maximum),
                                    Minimum = ToDbFloat(fault.Curves[i].Minimum),
                                    Average = ToDbFloat(fault.Curves[i].Average),
                                    StandardDeviation = ToDbFloat(fault.Curves[i].StandardDeviation)
                                };

                                faultCurveStatisticTable.AddNewRecord(faultCurveStatistic);
                            }
                        }
                    }
                }
            }

            private openXDA.Model.FaultGroup CreateFaultGroup(int eventID, FaultGroup faultGroup)
            {
                return new openXDA.Model.FaultGroup()
                {
                    EventID = eventID,
                    FaultDetectionLogicResult = faultGroup.FaultDetectionLogicResult,
                    DefaultFaultDetectionLogicResult = faultGroup.DefaultFaultDetectionLogicResult,
                    FaultValidationLogicResult = faultGroup.FaultValidationLogicResult
                };
            }

            private FaultSegment CreateFaultSegment(int eventID, Fault fault, SegmentType faultSegmentType)
            {
                return new FaultSegment()
                {
                    EventID = eventID,
                    SegmentTypeID = faultSegmentType.ID,
                    StartTime = fault.InceptionTime,
                    EndTime = fault.ClearingTime,
                    StartSample = fault.StartSample,
                    EndSample = fault.EndSample
                };
            }

            private FaultSegment CreateFaultSegment(int eventID, Fault.Segment segment, SegmentType segmentType)
            {
                return new FaultSegment()
                {
                    EventID = eventID,
                    SegmentTypeID = segmentType.ID,
                    StartTime = segment.StartTime,
                    EndTime = segment.EndTime,
                    StartSample = segment.StartSample,
                    EndSample = segment.EndSample
                };
            }

            private IEnumerable<FaultSummary> CreateFaultSummaries(int eventID, int faultNumber, Fault fault)
            {
                foreach (Fault.Summary summary in fault.Summaries)
                {
                    // Calculate the duration of the fault in seconds
                    double durationSeconds = fault.Duration.TotalSeconds;

                    // Create the fault summary record to be written to the database
                    yield return new FaultSummary()
                    {
                        EventID = eventID,
                        Algorithm = summary.DistanceAlgorithm,
                        FaultNumber = faultNumber,
                        CalculationCycle = fault.CalculationCycle,
                        Distance = ToDbFloat(summary.Distance),
                        CurrentMagnitude = ToDbFloat(fault.CurrentMagnitude),
                        CurrentLag = ToDbFloat(fault.CurrentLag),
                        PrefaultCurrent = ToDbFloat(fault.PrefaultCurrent),
                        PostfaultCurrent = ToDbFloat(fault.PostfaultCurrent),
                        ReactanceRatio = ToDbFloat(fault.ReactanceRatio),
                        Inception = fault.InceptionTime,
                        DurationSeconds = durationSeconds,
                        DurationCycles = durationSeconds * m_systemFrequency,
                        FaultType = fault.Type.ToString(),
                        IsSelectedAlgorithm = summary.IsSelectedAlgorithm,
                        IsValid = summary.IsValid,
                        IsSuppressed = fault.IsSuppressed
                    };
                }
            }

            private FaultCurve CreateFaultCurve(int eventID, int curveIndex)
            {
                DataSeries series = DataGroup.DataSeries[0].Multiply(double.NaN);

                foreach (Fault fault in FaultGroup.Faults)
                {
                    int startSample = fault.StartSample;
                    int endSample = startSample + fault.Curves.Min(curve => curve.Series.DataPoints.Count) - 1;

                    for (int sample = fault.StartSample; sample <= endSample; sample++)
                        series[sample].Value = fault.Curves[curveIndex].Series[sample - fault.StartSample].Value;
                }

                return new FaultCurve()
                {
                    EventID = eventID,
                    Algorithm = FaultGroup.Faults[0].Curves[curveIndex].Algorithm,
                    Data = Serialize(series)
                };
            }

            private double ToDbFloat(double value)
            {
                const double Invalid = -1.0e308D;

                if (double.IsNaN(value) || double.IsInfinity(value))
                    return Invalid;

                return value;
            }

            private byte[] Serialize(DataSeries series)
            {
                DataGroup group = new DataGroup();
                group.Add(series);
                return group.ToData();
            }

            #endregion
        }

        // Fields
        private FaultSummarizer m_faultSummarizer;
        private double m_systemFrequency;

        #endregion

        #region [ Properties ]

        [Setting]
        public double SystemFrequency
        {
            get
            {
                return m_systemFrequency;
            }
            set
            {
                m_systemFrequency = value;
            }
        }

        #endregion

        #region [ Methods ]

        public override void Execute(MeterDataSet meterDataSet)
        {
            FaultDataResource faultDataResource = meterDataSet.GetResource<FaultDataResource>();
            CycleDataResource cycleDataResource = meterDataSet.GetResource<CycleDataResource>();
            FaultGroup faultGroup;

            Log.Info("Executing operation to load fault location data into the database...");

            m_faultSummarizer = new FaultSummarizer(m_systemFrequency);
            m_faultSummarizer.MeterDataSet = meterDataSet;

            foreach (DataGroup dataGroup in cycleDataResource.DataGroups)
            {
                if (!faultDataResource.FaultLookup.TryGetValue(dataGroup, out faultGroup))
                    continue;

                // Generate summary rows for the fault to be entered into the database
                m_faultSummarizer.DataGroup = dataGroup;
                m_faultSummarizer.FaultGroup = faultGroup;
                m_faultSummarizer.SummarizeFault();
            }
        }

        #endregion

        #region [ Static ]

        // Static Fields
        private static readonly ILog Log = LogManager.GetLogger(typeof(FaultLocationOperation));

        #endregion
    }
}
