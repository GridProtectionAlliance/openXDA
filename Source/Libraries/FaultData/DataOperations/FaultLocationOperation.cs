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
using FaultAlgorithms;
using GSF;
using FaultData.DataAnalysis;
using FaultData.Database;
using FaultData.Database.MeterDataTableAdapters;
using GSF.Collections;
using FaultLocationAlgorithm = FaultAlgorithms.FaultLocationAlgorithm;
using Line = FaultData.Database.Line;

namespace FaultData.DataOperations
{
    public class FaultLocationOperation : IDataOperation
    {
        #region [ Members ]

        // Nested Types
        private class Segment
        {
            public FaultType FaultType;
            public int StartSample;
            public int EndSample;
        }

        private class ImpedanceExtractor
        {
            #region [ Members ]

            // Fields
            public FaultLocationDataSet FaultLocationDataSet;

            public FaultLocationInfoDataContext FaultLocationInfo;
            public Meter Meter;
            public Line Line;

            #endregion

            #region [ Methods ]

            public bool TryExtractImpedances()
            {
                LineImpedance lineImpedance;
                int lineID;

                List<SourceImpedance> sourceImpedances;
                List<SourceImpedance> remoteImpedances;
                SourceImpedance localImpedance;

                List<int> linkIDs;
                int localMeterLocationID;
                int localLinkID;

                lineID = Line.ID;

                lineImpedance = FaultLocationInfo.LineImpedances
                    .FirstOrDefault(impedance => impedance.LineID == lineID);

                if ((object)lineImpedance != null)
                {
                    localMeterLocationID = Meter.MeterLocationID;

                    linkIDs = Line.MeterLocationLines
                        .Select(link => link.ID)
                        .ToList();

                    localLinkID = Line.MeterLocationLines
                        .Where(link => link.MeterLocationID == localMeterLocationID)
                        .Select(link => link.ID)
                        .FirstOrDefault();

                    sourceImpedances = FaultLocationInfo.SourceImpedances
                        .Where(impedance => linkIDs.Contains(impedance.MeterLocationLineID))
                        .ToList();

                    localImpedance = sourceImpedances
                        .FirstOrDefault(impedance => impedance.MeterLocationLineID == localLinkID);

                    remoteImpedances = sourceImpedances
                        .Where(impedance => impedance.MeterLocationLineID != localMeterLocationID)
                        .ToList();

                    FaultLocationDataSet.Z0 = new ComplexNumber(lineImpedance.R0, lineImpedance.X0);
                    FaultLocationDataSet.Z1 = new ComplexNumber(lineImpedance.R1, lineImpedance.X1);

                    if ((object)localImpedance != null)
                        FaultLocationDataSet.ZSrc = new ComplexNumber(localImpedance.RSrc, localImpedance.XSrc);

                    if (remoteImpedances.Count == 1)
                        FaultLocationDataSet.ZRem = new ComplexNumber(remoteImpedances[0].RSrc, remoteImpedances[0].XSrc);

                    return true;
                }

                return false;
            }

            #endregion
        }

        private class FaultCurveGenerator
        {
            #region [ Members ]

            // Fields
            public int SamplesPerCycle;

            public VICycleDataSet CycleDataSet;
            public List<Segment> FaultTypeSegments;
            public FaultLocationDataSet FaultLocationDataSet;
            public List<FaultLocationAlgorithm> FaultLocationAlgorithms;

            public List<DataSeries> FaultCurves;

            #endregion

            #region [ Methods ]

            public void GenerateFaultCurves()
            {
                List<List<Segment>> segmentGroups;

                VICycleDataSet subSet;
                List<DataPoint> faultDataPoints;
                double[] faultDistances;

                // Initialize a fault curve for each algorithm
                FaultCurves = FaultLocationAlgorithms
                    .Select(algorithm => new DataSeries())
                    .ToList();

                // Break the list of segments into smaller lists of segments where
                // each consecutive group of faulted segments marks the end of a list
                segmentGroups = FaultTypeSegments
                    .Aggregate(new List<List<Segment>>(), (list, segment) =>
                    {
                        if (segment.FaultType == FaultType.None || list.Count == 0)
                            list.Add(new List<Segment>());

                        list.Last().Add(segment);

                        return list;
                    });

                foreach (List<Segment> segmentGroup in segmentGroups)
                {
                    // Don't calculate fault distances for cycles at the end
                    // of a fault because these cycles include non-faulted data
                    int endSample = segmentGroup.Last().EndSample - SamplesPerCycle;

                    foreach (Segment segment in segmentGroup)
                    {
                        if (endSample > segment.StartSample)
                        {
                            // Get a subset of the cycle data that contains only the data
                            // in the current segment, up to and including the end sample
                            subSet = CycleDataSet.ToSubSet(segment.StartSample, Math.Min(segment.EndSample, endSample));

                            if (segment.FaultType != FaultType.None)
                            {
                                // Set the fault type of the fault location data
                                // set to the fault type of the current segment
                                FaultLocationDataSet.FaultType = segment.FaultType;

                                // Push data from the cycle data set
                                // to the fault location data set
                                FaultLocationDataSet.Cycles.Clear();
                                subSet.PushDataTo(FaultLocationDataSet.Cycles);

                                // Attempt to execute each fault location algorithm
                                for (int i = 0; i < FaultLocationAlgorithms.Count; i++)
                                {
                                    if (TryExecute(FaultLocationAlgorithms[i], FaultLocationDataSet, out faultDistances))
                                    {
                                        // Create a data point for each of the fault distances
                                        faultDataPoints = subSet.VA.RMS.DataPoints
                                            .Zip(faultDistances, (point, distance) => new DataPoint() { Time = point.Time, Value = distance })
                                            .ToList();
                                    }
                                    else
                                    {
                                        // Generate NaN-value data points to
                                        // fill this segment of the fault curve
                                        faultDataPoints = subSet.VA.RMS.DataPoints
                                            .Select(point => new DataPoint() { Time = point.Time, Value = double.NaN })
                                            .ToList();
                                    }

                                    // Add the data points to the current fault curve
                                    FaultCurves[i].DataPoints.AddRange(faultDataPoints);
                                }
                            }
                            else
                            {
                                foreach (DataSeries faultCurve in FaultCurves)
                                {
                                    // Generate NaN-value data points to
                                    // fill this segment of the fault curve
                                    faultDataPoints = subSet.VA.RMS.DataPoints
                                        .Select(point => new DataPoint() { Time = point.Time, Value = double.NaN })
                                        .ToList();

                                    // Add the data points to the current fault curve
                                    faultCurve.DataPoints.AddRange(faultDataPoints);
                                }
                            }
                        }

                        if (endSample < segment.EndSample)
                        {
                            // Get a subset of the cycle data that contains only
                            // the data in the current segment after the end sample
                            subSet = CycleDataSet.ToSubSet(Math.Max(segment.StartSample, endSample + 1), segment.EndSample);

                            foreach (DataSeries faultCurve in FaultCurves)
                            {
                                // Generate NaN-value data points to fill
                                // the fault curve beyond the end sample
                                faultDataPoints = subSet.VA.RMS.DataPoints
                                    .Select(point => new DataPoint() { Time = point.Time, Value = double.NaN })
                                    .ToList();

                                // Add the data points to the current fault curve
                                faultCurve.DataPoints.AddRange(faultDataPoints);
                            }
                        }
                    }
                }
            }

            private bool TryExecute(FaultLocationAlgorithm faultLocationAlgorithm, FaultLocationDataSet faultLocationDataSet, out double[] distances)
            {
                try
                {
                    distances = faultLocationAlgorithm(faultLocationDataSet, null);
                }
                catch
                {
                    distances = null;
                }

                return (object)distances != null;
            }

            #endregion
        }

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
            public FaultLocationData.FaultCurveDataTable FaultCurveTable;
            public FaultLocationData.FaultSummaryDataTable FaultSummaryTable;

            public int EventID;
            public VICycleDataSet CycleDataSet;
            public List<Segment> FaultTypeSegments;

            public List<FaultLocationAlgorithm> FaultLocationAlgorithms;
            public List<DataSeries> FaultCurves;

            #endregion

            #region [ Methods ]

            public void SummarizeFault()
            {
                StatCalculator statCalculator;

                List<List<Segment>> faults;
                List<Segment> fault;
                int largestCurrentIndex;

                DateTime startTime;
                DateTime endTime;
                double durationSeconds;

                FaultLocationData.FaultCurveRow faultCurveRow;
                FaultLocationData.FaultSummaryRow faultSummaryRow;

                statCalculator = new StatCalculator();

                // Break the list of segments into smaller lists of segments where
                // each consecutive group of faulted segments marks the end of a list
                faults = FaultTypeSegments
                    .Aggregate(new List<List<Segment>>(), (list, segment) =>
                    {
                        if (segment.FaultType == FaultType.None || list.Count == 0)
                            list.Add(new List<Segment>());

                        if (segment.FaultType != FaultType.None)
                            list.Last().Add(segment);

                        return list;
                    });

                faults.RemoveAll(list => list.Count == 0);

                for (int i = 0; i < FaultLocationAlgorithms.Count; i++)
                {
                    // Create the fault curve record to be written to the database
                    faultCurveRow = FaultCurveTable.NewFaultCurveRow();
                    faultCurveRow.EventID = EventID;
                    faultCurveRow.Algorithm = FaultLocationAlgorithms[i].Method.Name;
                    faultCurveRow.Data = Serialize(FaultCurves[i]);

                    FaultCurveTable.AddFaultCurveRow(faultCurveRow);

                    for (int faultIndex = 0; faultIndex < faults.Count; faultIndex++)
                    {
                        fault = faults[faultIndex];

                        // Get the index of the cycle where the sum of the three phases of RMS current is largest
                        largestCurrentIndex = GetLargestCurrentIndex(CycleDataSet, fault);

                        if (!double.IsNaN(FaultCurves[i].DataPoints[largestCurrentIndex].Value))
                        {
                            // Determine the start time and end time of the fault
                            startTime = CycleDataSet.VA.RMS[fault.First().StartSample].Time;
                            endTime = CycleDataSet.VA.RMS[fault.Last().EndSample].Time;

                            // Calculate the duration of the fault in seconds
                            durationSeconds = (endTime - startTime).TotalSeconds;

                            // Calculate statistics about the fault
                            // using fault data from this fault curve
                            statCalculator.DataPoints = FaultCurves[i].DataPoints
                                .Skip(fault.First().StartSample)
                                .Take(fault.Last().EndSample)
                                .Where(dataPoint => !double.IsNaN(dataPoint.Value))
                                .ToList();

                            statCalculator.CalculateStatistics();

                            // Create the fault summary record to be written to the database
                            faultSummaryRow = FaultSummaryTable.NewFaultSummaryRow();
                            faultSummaryRow.EventID = EventID;
                            faultSummaryRow.Algorithm = FaultLocationAlgorithms[i].Method.Name;
                            faultSummaryRow.FaultNumber = faultIndex + 1;
                            faultSummaryRow.LargestCurrentDistance = FaultCurves[i].DataPoints[largestCurrentIndex].Value;
                            faultSummaryRow.MaximumDistance = statCalculator.Maximum;
                            faultSummaryRow.MinimumDistance = statCalculator.Minimum;
                            faultSummaryRow.AverageDistance = statCalculator.Average;
                            faultSummaryRow.DistanceDeviation = statCalculator.StandardDeviation;
                            faultSummaryRow.Inception = startTime;
                            faultSummaryRow.DurationSeconds = durationSeconds;
                            faultSummaryRow.DurationCycles = durationSeconds * Frequency;
                            faultSummaryRow.FaultType = fault.MaxBy(segment => segment.EndSample - segment.StartSample).FaultType.ToString();

                            FaultSummaryTable.AddFaultSummaryRow(faultSummaryRow);
                        }
                    }
                }
            }

            private int GetLargestCurrentIndex(VICycleDataSet viCycleDataSet, List<Segment> faultTypeSegments)
            {
                List<double> ia = Transform.ToValues(viCycleDataSet.IA.RMS);
                List<double> ib = Transform.ToValues(viCycleDataSet.IB.RMS);
                List<double> ic = Transform.ToValues(viCycleDataSet.IC.RMS);

                int largestCurrentIndex = faultTypeSegments
                    .Where(segment => segment.FaultType != FaultType.None)
                    .SelectMany(segment => Enumerable.Range(segment.StartSample, segment.EndSample - segment.StartSample + 1))
                    .Select(i => Tuple.Create(i, ia[i] + ib[i] + ic[i]))
                    .MaxBy(tuple => tuple.Item2)
                    .Item1;

                return largestCurrentIndex;
            }

            private byte[] Serialize(DataSeries series)
            {
                DataGroup group = new DataGroup();
                group.Add(series);
                return group.ToData();
            }

            #endregion
        }

        // Constants
        // TODO: Hardcoded frequency
        private const double Frequency = 60.0D;

        // Events
        public event EventHandler<EventArgs<string>> StatusMessage;
        public event EventHandler<EventArgs<Exception>> ProcessException;

        // Fields
        private string m_connectionString;
        private double m_prefaultMultiplier;
        private double m_ratedCurrentMultiplier;
        private double m_minFaultDistanceMultiplier;
        private double m_maxFaultDistanceMultiplier;

        #endregion

        #region [ Constructors ]

        public FaultLocationOperation(string connectionString)
        {
            m_connectionString = connectionString;
            m_prefaultMultiplier = 4.0D;
            m_ratedCurrentMultiplier = 1.5D;
        }

        #endregion

        #region [ Properties ]

        public double PrefaultMultiplier
        {
            get
            {
                return m_prefaultMultiplier;
            }
            set
            {
                m_prefaultMultiplier = value;
            }
        }

        public double RatedCurrentMultiplier
        {
            get
            {
                return m_ratedCurrentMultiplier;
            }
            set
            {
                m_ratedCurrentMultiplier = value;
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

        #endregion

        #region [ Methods ]

        public void Execute(MeterDataSet meterDataSet)
        {
            List<DataGroup> dataGroups = meterDataSet.GetResource<DataGroupsResource>().DataGroups;

            int eventID;

            VIDataGroup viDataGroup;
            VICycleDataSet viCycleDataSet;
            List<Segment> faultDetectedSegments;
            List<Segment> faultTypeSegments;

            List<FaultLocationAlgorithm> faultLocationAlgorithms;
            FaultLocationDataSet faultLocationDataSet;
            ImpedanceExtractor impedanceExtractor;
            FaultCurveGenerator faultCurveGenerator;
            FaultSummarizer faultSummarizer;

            FaultLocationData.CycleDataDataTable cycleDataTable = new FaultLocationData.CycleDataDataTable();
            FaultLocationData.FaultCurveDataTable faultCurveTable = new FaultLocationData.FaultCurveDataTable();
            FaultLocationData.FaultSummaryDataTable faultSummaryTable = new FaultLocationData.FaultSummaryDataTable();

            OnStatusMessage("Executing operation to calculate fault location data and load it into the database...");

            using (FaultLocationInfoDataContext faultLocationInfo = new FaultLocationInfoDataContext(m_connectionString))
            {
                faultLocationAlgorithms = GetFaultLocationAlgorithms(faultLocationInfo);

                foreach (DataGroup dataGroup in dataGroups)
                {
                    // Get the ID of the event record for this fault
                    using (EventTableAdapter adapter = new EventTableAdapter())
                    {
                        adapter.Connection.ConnectionString = m_connectionString;
                        eventID = adapter.GetEventIDBy(meterDataSet.Meter.ID, dataGroup.Line.ID, meterDataSet.FileGroup.ID, dataGroup.StartTime, dataGroup.EndTime) ?? 0;
                    }

                    if (eventID == 0)
                        continue;

                    // Calculate cycle data
                    OnStatusMessage(string.Format("Event {0}: Transforming data to frequency domain.", eventID));
                    viDataGroup = GetVIDataGroup(dataGroup);
                    viCycleDataSet = Transform.ToVICycleDataSet(viDataGroup, Frequency);
                    cycleDataTable.AddCycleDataRow(eventID, viCycleDataSet.ToDataGroup().ToData());

                    // If the data isn't faulted, skip the fault analysis
                    if (dataGroup.Classification != DataClassification.Fault)
                        continue;

                    // Break fault into segments
                    OnStatusMessage(string.Format("Event {0}: Detecting and classifying fault segments.", eventID));
                    faultDetectedSegments = DetectFaults(viDataGroup, viCycleDataSet);
                    faultTypeSegments = ClassifyFaults(faultDetectedSegments, viCycleDataSet);
                    WriteSegmentsToDatabase(faultTypeSegments, eventID, faultLocationInfo, dataGroup);

                    // Determine the number of faults in this data group for logging
                    int faultCount = faultTypeSegments
                        .SkipWhile(segment => segment.FaultType == FaultType.None)
                        .Reverse()
                        .SkipWhile(segment => segment.FaultType == FaultType.None)
                        .Count(segment => segment.FaultType == FaultType.None) + 1;

                    OnStatusMessage(string.Format("Event {0}: Found {1} fault{2} and a total of {3} segment{4}.", eventID, faultCount, (faultCount != 1) ? "s" : "", faultTypeSegments.Count, (faultTypeSegments.Count != 1) ? "s" : ""));

                    // Create the fault location data set and begin populating
                    // the properties necessary for calculating fault location
                    faultLocationDataSet = new FaultLocationDataSet();
                    faultLocationDataSet.LineDistance = dataGroup.Line.Length;
                    faultLocationDataSet.PrefaultCycle = FirstCycle(viCycleDataSet);

                    // Extract impedances from the database
                    // and into the fault location data set
                    impedanceExtractor = new ImpedanceExtractor();
                    impedanceExtractor.FaultLocationDataSet = faultLocationDataSet;
                    impedanceExtractor.FaultLocationInfo = faultLocationInfo;
                    impedanceExtractor.Meter = meterDataSet.Meter;
                    impedanceExtractor.Line = dataGroup.Line;

                    if (!impedanceExtractor.TryExtractImpedances())
                    {
                        OnStatusMessage(string.Format("Event {0}: No line impedance found for line {1}.", eventID, dataGroup.Line.Name));
                        continue;
                    }

                    // Generate fault curves for fault analysis
                    OnStatusMessage(string.Format("Event {0}: Generating fault curves.", eventID));
                    faultCurveGenerator = new FaultCurveGenerator();
                    faultCurveGenerator.SamplesPerCycle = (int)(viDataGroup.VA.SampleRate / Frequency);
                    faultCurveGenerator.CycleDataSet = viCycleDataSet;
                    faultCurveGenerator.FaultTypeSegments = faultTypeSegments;
                    faultCurveGenerator.FaultLocationDataSet = faultLocationDataSet;
                    faultCurveGenerator.FaultLocationAlgorithms = faultLocationAlgorithms;
                    faultCurveGenerator.GenerateFaultCurves();

                    // Generate summary rows for the fault to be entered into the database
                    OnStatusMessage(string.Format("Event {0}: Summarizing fault results.", eventID));
                    faultSummarizer = new FaultSummarizer();
                    faultSummarizer.FaultCurveTable = faultCurveTable;
                    faultSummarizer.FaultSummaryTable = faultSummaryTable;
                    faultSummarizer.EventID = eventID;
                    faultSummarizer.CycleDataSet = viCycleDataSet;
                    faultSummarizer.FaultTypeSegments = faultTypeSegments;
                    faultSummarizer.FaultLocationAlgorithms = faultLocationAlgorithms;
                    faultSummarizer.FaultCurves = faultCurveGenerator.FaultCurves;
                    faultSummarizer.SummarizeFault();
                }

                OnStatusMessage("Loading fault location data into the database.");

                // Submit new fault segment rows to the database
                faultLocationInfo.SubmitChanges();
            }

            if (faultCurveTable.Count > 0)
            {
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(m_connectionString))
                {
                    bulkCopy.BulkCopyTimeout = 0;

                    // Submit cycle data to the database
                    bulkCopy.DestinationTableName = cycleDataTable.TableName;
                    bulkCopy.WriteToServer(cycleDataTable);

                    // Submit fault curves to the database
                    bulkCopy.DestinationTableName = faultCurveTable.TableName;
                    bulkCopy.WriteToServer(faultCurveTable);

                    // Submit fault summary records to the database
                    bulkCopy.DestinationTableName = faultSummaryTable.TableName;
                    bulkCopy.WriteToServer(faultSummaryTable);
                }
            }
        }

        private void WriteSegmentsToDatabase(List<Segment> faultTypeSegments, int eventID, FaultLocationInfoDataContext faultLocationInfo, DataGroup faultGroup)
        {
            FaultSegment faultSegment;
            bool prefault;

            prefault = true;

            foreach (Segment segment in faultTypeSegments)
            {
                if (segment.FaultType != FaultType.None)
                    prefault = false;

                faultSegment = new FaultSegment();
                faultSegment.EventID = eventID;
                faultSegment.SegmentType = GetSegmentType(prefault, faultLocationInfo, segment.FaultType);
                faultSegment.StartTime = faultGroup.DataSeries[0].DataPoints[segment.StartSample].Time;
                faultSegment.EndTime = faultGroup.DataSeries[0].DataPoints[segment.EndSample].Time;
                faultSegment.StartSample = segment.StartSample;
                faultSegment.EndSample = segment.EndSample;

                faultLocationInfo.FaultSegments.InsertOnSubmit(faultSegment);
            }
        }

        private VIDataGroup GetVIDataGroup(DataGroup faultGroup)
        {
            Dictionary<string, DataSeries> seriesLookup = GetSeriesLookup(faultGroup);

            DataGroup dataGroup = new DataGroup();

            dataGroup.Add(seriesLookup["VA"]);
            dataGroup.Add(seriesLookup["VB"]);
            dataGroup.Add(seriesLookup["VC"]);
            dataGroup.Add(seriesLookup["IA"]);
            dataGroup.Add(seriesLookup["IB"]);
            dataGroup.Add(seriesLookup["IC"]);

            return new VIDataGroup(dataGroup);
        }

        private Dictionary<string, DataSeries> GetSeriesLookup(DataGroup faultGroup)
        {
            string[] measurementTypes = { "Voltage", "Current" };
            string[] phases = { "AN", "BN", "CN" };

            Func<DataSeries, string> typeSelector = series =>
            {
                string measurementType = series.SeriesInfo.Channel.MeasurementType.Name;
                string phase = series.SeriesInfo.Channel.Phase.Name;

                if (measurementType == "Current")
                    measurementType = "I";

                return string.Format("{0}{1}", measurementType[0], phase[0]);
            };

            return faultGroup.DataSeries
                .Where(series => series.SeriesInfo.SeriesType.Name == "Values")
                .Where(series => series.SeriesInfo.Channel.MeasurementCharacteristic.Name == "Instantaneous")
                .Where(series => measurementTypes.Contains(series.SeriesInfo.Channel.MeasurementType.Name))
                .Where(series => phases.Contains(series.SeriesInfo.Channel.Phase.Name))
                .ToDictionary(typeSelector);
        }

        private List<Segment> DetectFaults(VIDataGroup viDataGroup, VICycleDataSet viCycleDataSet)
        {
            List<Segment> segments = new List<Segment>();
            Segment currentSegment = null;

            DataSeries iaRMS = viCycleDataSet.IA.RMS;
            DataSeries ibRMS = viCycleDataSet.IB.RMS;
            DataSeries icRMS = viCycleDataSet.IC.RMS;

            double ratedCurrentMultiplied = viDataGroup.ToDataGroup().Line.ThermalRating * m_ratedCurrentMultiplier;
            double iaFault = Math.Max(iaRMS.DataPoints[0].Value * m_prefaultMultiplier, ratedCurrentMultiplied);
            double ibFault = Math.Max(ibRMS.DataPoints[0].Value * m_prefaultMultiplier, ratedCurrentMultiplied);
            double icFault = Math.Max(icRMS.DataPoints[0].Value * m_prefaultMultiplier, ratedCurrentMultiplied);

            double ia;
            double ib;
            double ic;

            int iaIndex;
            int ibIndex;
            int icIndex;

            for (int i = 0; i < iaRMS.DataPoints.Count; i++)
            {
                ia = iaRMS.DataPoints[i].Value;
                ib = ibRMS.DataPoints[i].Value;
                ic = icRMS.DataPoints[i].Value;

                if ((object)currentSegment == null)
                {
                    currentSegment = new Segment();
                    currentSegment.FaultType = FaultType.None;
                    currentSegment.StartSample = 0;
                    segments.Add(currentSegment);
                }

                if (currentSegment.FaultType == FaultType.None)
                {
                    // Search start of fault
                    if (ia >= iaFault || ib >= ibFault || ic >= icFault)
                    {
                        iaIndex = FindFaultInception(viDataGroup.IA, i);
                        ibIndex = FindFaultInception(viDataGroup.IB, i);
                        icIndex = FindFaultInception(viDataGroup.IC, i);

                        i = Common.Min(iaIndex, ibIndex, icIndex);
                        currentSegment.EndSample = i - 1;

                        currentSegment = new Segment();
                        currentSegment.FaultType = FaultType.ABC;
                        currentSegment.StartSample = i;
                        segments.Add(currentSegment);
                    }
                }
                else
                {
                    // Search for end of fault
                    if (ia < iaFault && ib < ibFault && ic < icFault)
                    {
                        iaIndex = FindFaultClearing(viDataGroup.IA, i);
                        ibIndex = FindFaultClearing(viDataGroup.IB, i);
                        icIndex = FindFaultClearing(viDataGroup.IC, i);

                        i = Common.Max(iaIndex, ibIndex, icIndex);
                        currentSegment.EndSample = i - 1;
                        
                        currentSegment = new Segment();
                        currentSegment.FaultType = FaultType.None;
                        currentSegment.StartSample = i;
                        segments.Add(currentSegment);
                    }
                }
            }

            if ((object)currentSegment != null)
                currentSegment.EndSample = iaRMS.DataPoints.Count - 1;

            return segments;
        }

        private int FindFaultInception(DataSeries waveForm, int cycleIndex)
        {
            int samplesPerCycle = (int)Math.Round(waveForm.SampleRate / Frequency);
            int endIndex = cycleIndex + samplesPerCycle - 1;

            double previousValue;
            double value;
            double nextValue;

            for (int i = endIndex; i >= cycleIndex; i--)
            {
                if (i - 1 < 0)
                    continue;

                if (i + 1 >= waveForm.DataPoints.Count)
                    continue;

                previousValue = waveForm.DataPoints[i - 1].Value;
                value = waveForm.DataPoints[i].Value;
                nextValue = waveForm.DataPoints[i + 1].Value;

                if (value * nextValue < 0.0D)
                    return i + 1;

                if (5.0D * Math.Abs(value - previousValue) < Math.Abs(nextValue - value))
                    return i;
            }

            return endIndex;
        }

        private int FindFaultClearing(DataSeries waveForm, int cycleIndex)
        {
            int samplesPerCycle = (int)Math.Round(waveForm.SampleRate / Frequency);

            double previousValue;
            double value;
            double nextValue;

            for (int i = cycleIndex; i < cycleIndex + samplesPerCycle; i++)
            {
                if (i - 1 < 0)
                    continue;

                if (i + 1 >= waveForm.DataPoints.Count)
                    continue;

                previousValue = waveForm.DataPoints[i - 1].Value;
                value = waveForm.DataPoints[i].Value;
                nextValue = waveForm.DataPoints[i + 1].Value;

                if (value * nextValue < 0.0D)
                    return i;

                if (Math.Abs(value - previousValue) > 5.0D * Math.Abs(nextValue - value))
                    return i;
            }

            return cycleIndex;
        }

        private List<Segment> ClassifyFaults(List<Segment> faultDetectedSegments, VICycleDataSet viCycleDataSet)
        {
            List<Segment> faultTypeSegments = new List<Segment>();
            Segment currentSegment = null;

            DataSeries iaRMS = viCycleDataSet.IA.RMS;
            DataSeries ibRMS = viCycleDataSet.IB.RMS;
            DataSeries icRMS = viCycleDataSet.IC.RMS;

            double iaPre = iaRMS.DataPoints[0].Value;
            double ibPre = ibRMS.DataPoints[0].Value;
            double icPre = icRMS.DataPoints[0].Value;

            double ia;
            double ib;
            double ic;

            int numPhases;
            FaultType faultType;

            foreach (Segment segment in faultDetectedSegments)
            {
                if (segment.FaultType == FaultType.None)
                {
                    faultTypeSegments.Add(segment);
                }
                else
                {
                    for (int i = segment.StartSample; i <= segment.EndSample; i++)
                    {
                        ia = iaRMS.DataPoints[i].Value;
                        ib = ibRMS.DataPoints[i].Value;
                        ic = icRMS.DataPoints[i].Value;

                        numPhases = GetNumPhases(4.0D, ia, ib, ic);

                        if (numPhases == 3)
                            numPhases = GetNumPhases(1.5, ia - iaPre, ib - ibPre, ic - icPre);

                        faultType = GetFaultType(numPhases, ia, ib, ic);

                        if ((object)currentSegment == null)
                        {
                            currentSegment = new Segment();
                            currentSegment.FaultType = faultType;
                            currentSegment.StartSample = i;
                            faultTypeSegments.Add(currentSegment);
                        }
                        else if (currentSegment.FaultType != faultType)
                        {
                            currentSegment.EndSample = i - 1;

                            currentSegment = new Segment();
                            currentSegment.FaultType = faultType;
                            currentSegment.StartSample = i;
                            faultTypeSegments.Add(currentSegment);
                        }
                    }

                    if ((object)currentSegment != null)
                    {
                        currentSegment.EndSample = segment.EndSample;
                        currentSegment = null;
                    }
                }
            }

            return faultTypeSegments;
        }

        private int GetNumPhases(double factor, double ia, double ib, double ic)
        {
            double max = Common.Max(ia, ib, ic);
            double min = Common.Min(ia, ib, ic);
            double mid = (ia + ib + ic) - max - min;

            if (max > factor * mid)
                return 1;

            if (mid > factor * min)
                return 2;

            return 3;
        }

        private FaultType GetFaultType(int numPhases, double ia, double ib, double ic)
        {
            if (numPhases == 3)
                return FaultType.ABC;

            if (numPhases == 2)
            {
                if (ic < ia && ic < ib)
                    return FaultType.AB;

                if (ia < ib && ia < ic)
                    return FaultType.BC;

                if (ib < ia && ib < ic)
                    return FaultType.CA;
            }

            if (numPhases == 1)
            {
                if (ia > ib && ia > ic)
                    return FaultType.AN;

                if (ib > ia && ib > ic)
                    return FaultType.BN;

                if (ic > ia && ic > ib)
                    return FaultType.CN;
            }

            return FaultType.None;
        }

        private SegmentType GetSegmentType(bool prefault, FaultLocationInfoDataContext faultLocationInfo, FaultType faultType)
        {
            switch (faultType)
            {
                case FaultType.AN:
                    return faultLocationInfo.SegmentTypes.First(type => type.Name == "AN Fault");

                case FaultType.BN:
                    return faultLocationInfo.SegmentTypes.First(type => type.Name == "BN Fault");

                case FaultType.CN:
                    return faultLocationInfo.SegmentTypes.First(type => type.Name == "CN Fault");

                case FaultType.AB:
                    return faultLocationInfo.SegmentTypes.First(type => type.Name == "AB Fault");

                case FaultType.BC:
                    return faultLocationInfo.SegmentTypes.First(type => type.Name == "BC Fault");

                case FaultType.CA:
                    return faultLocationInfo.SegmentTypes.First(type => type.Name == "CA Fault");

                case FaultType.ABG:
                    return faultLocationInfo.SegmentTypes.First(type => type.Name == "ABG Fault");

                case FaultType.BCG:
                    return faultLocationInfo.SegmentTypes.First(type => type.Name == "BCG Fault");

                case FaultType.CAG:
                    return faultLocationInfo.SegmentTypes.First(type => type.Name == "CAG Fault");

                case FaultType.ABC:
                    return faultLocationInfo.SegmentTypes.First(type => type.Name == "3-Phase Fault");

                default:
                    return prefault
                        ? faultLocationInfo.SegmentTypes.First(type => type.Name == "Prefault")
                        : faultLocationInfo.SegmentTypes.First(type => type.Name == "Postfault");
            }
        }

        private CycleData FirstCycle(VICycleDataSet viCycleDataSet)
        {
            CycleData cycle = new CycleData();

            cycle.AN.V.RMS = viCycleDataSet.VA.RMS.DataPoints[0].Value;
            cycle.AN.V.Phase = viCycleDataSet.VA.Phase.DataPoints[0].Value;
            cycle.AN.V.Peak = viCycleDataSet.VA.Peak.DataPoints[0].Value;
            cycle.AN.V.Error = viCycleDataSet.VA.Error.DataPoints[0].Value;

            cycle.BN.V.RMS = viCycleDataSet.VB.RMS.DataPoints[0].Value;
            cycle.BN.V.Phase = viCycleDataSet.VB.Phase.DataPoints[0].Value;
            cycle.BN.V.Peak = viCycleDataSet.VB.Peak.DataPoints[0].Value;
            cycle.BN.V.Error = viCycleDataSet.VB.Error.DataPoints[0].Value;

            cycle.CN.V.RMS = viCycleDataSet.VC.RMS.DataPoints[0].Value;
            cycle.CN.V.Phase = viCycleDataSet.VC.Phase.DataPoints[0].Value;
            cycle.CN.V.Peak = viCycleDataSet.VC.Peak.DataPoints[0].Value;
            cycle.CN.V.Error = viCycleDataSet.VC.Error.DataPoints[0].Value;

            cycle.AN.I.RMS = viCycleDataSet.IA.RMS.DataPoints[0].Value;
            cycle.AN.I.Phase = viCycleDataSet.IA.Phase.DataPoints[0].Value;
            cycle.AN.I.Peak = viCycleDataSet.IA.Peak.DataPoints[0].Value;
            cycle.AN.I.Error = viCycleDataSet.IA.Error.DataPoints[0].Value;

            cycle.BN.I.RMS = viCycleDataSet.IB.RMS.DataPoints[0].Value;
            cycle.BN.I.Phase = viCycleDataSet.IB.Phase.DataPoints[0].Value;
            cycle.BN.I.Peak = viCycleDataSet.IB.Peak.DataPoints[0].Value;
            cycle.BN.I.Error = viCycleDataSet.IB.Error.DataPoints[0].Value;

            cycle.CN.I.RMS = viCycleDataSet.IC.RMS.DataPoints[0].Value;
            cycle.CN.I.Phase = viCycleDataSet.IC.Phase.DataPoints[0].Value;
            cycle.CN.I.Peak = viCycleDataSet.IC.Peak.DataPoints[0].Value;
            cycle.CN.I.Error = viCycleDataSet.IC.Error.DataPoints[0].Value;

            return cycle;
        }

        private List<FaultLocationAlgorithm> GetFaultLocationAlgorithms(FaultLocationInfoDataContext faultLocationInfo)
        {
            return faultLocationInfo.FaultLocationAlgorithms
                .OrderBy(dbAlgorithm => dbAlgorithm.ExecutionOrder)
                .ThenBy(dbAlgorithm => dbAlgorithm.ID)
                .Select(dbAlgorithm => LoadAlgorithm<FaultLocationAlgorithm>(dbAlgorithm.AssemblyName, dbAlgorithm.TypeName, dbAlgorithm.MethodName))
                .ToList();
        }

        private T LoadAlgorithm<T>(string assemblyName, string typeName, string methodName) where T : class
        {
            Assembly assembly;
            Type type;
            MethodInfo method;

            assembly = Assembly.LoadFrom(assemblyName);
            type = assembly.GetType(typeName);
            method = type.GetMethod(methodName, BindingFlags.IgnoreCase | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.InvokeMethod);

            return Delegate.CreateDelegate(typeof(T), method) as T;
        }

        private void OnStatusMessage(string message)
        {
            if ((object)StatusMessage != null)
                StatusMessage(this, new EventArgs<string>(message));
        }

        private void OnProcessException(Exception ex)
        {
            if ((object)ProcessException != null)
                ProcessException(this, new EventArgs<Exception>(ex));
        }

        #endregion
    }
}
