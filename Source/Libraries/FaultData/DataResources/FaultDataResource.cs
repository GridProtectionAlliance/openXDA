﻿//******************************************************************************************************
//  FaultDataResource.cs - Gbtc
//
//  Copyright © 2015, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the MIT License (MIT), the "License"; you may
//  not use this file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://opensource.org/licenses/MIT
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  02/20/2015 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Reflection;
using FaultAlgorithms;
using FaultData.DataAnalysis;
using FaultData.DataSets;
using GSF;
using GSF.Collections;
using GSF.Configuration;
using GSF.Data;
using GSF.Data.Model;
using GSF.Parsing;
using GSF.Units;
using log4net;
using MathNet.Numerics.IntegralTransforms;
using openXDA.Configuration;
using openXDA.Model;
using CycleData = FaultAlgorithms.CycleData;
using Fault = FaultData.DataAnalysis.Fault;
using FaultGroup = FaultData.DataAnalysis.FaultGroup;
using FaultLocationAlgorithm = FaultAlgorithms.FaultLocationAlgorithm;
using Line = openXDA.Model.Line;

namespace FaultData.DataResources
{
    public class FaultDataResource : DataResourceBase<MeterDataSet>
    {
        #region [ Members ]

        // Nested Types
        private class ImpedanceExtractor
        {
            #region [ Members ]

            // Fields
            public FaultLocationDataSet FaultLocationDataSet;
            public Meter Meter;
            public Line Line;

            #endregion

            #region [ Methods ]

            public bool TryExtractImpedances()
            {
                if (Line.Segments != null)
                {
                    if (Line.Path[0].R0 == 0.0D && Line.Path[0].X0 == 0.0D &&
                        Line.Path[0].R1 == 0.0D && Line.Path[0].X1 == 0.0D)
                        return false;

                    FaultLocationDataSet.Z0 = new ComplexNumber(Line.Path[0].R0, Line.Path[0].X0);
                    FaultLocationDataSet.Z1 = new ComplexNumber(Line.Path[0].R1, Line.Path[0].X1);

                    SourceImpedance localImpedance = Line.AssetLocations
                        .Where(link => link.Location == Meter.Location)
                        .Where(link => link.SourceImpedance != null)
                        .Select(link => link.SourceImpedance)
                        .FirstOrDefault();

                    List<SourceImpedance> remoteImpedances = Line.AssetLocations
                        .Where(link => link.Location != Meter.Location)
                        .Where(link => link.SourceImpedance != null)
                        .Select(link => link.SourceImpedance)
                        .ToList();

                    if (localImpedance != null)
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

            public VICycleDataGroup CycleDataGroup;
            public List<Fault> Faults;
            public FaultLocationDataSet FaultLocationDataSet;
            public List<FaultLocationAlgorithm> FaultLocationAlgorithms;

            #endregion

            #region [ Methods ]

            public void GenerateFaultCurves()
            {
                VICycleDataGroup subGroup;
                List<DataPoint> faultDataPoints;
                double[] faultDistances;

                foreach (Fault fault in Faults)
                {
                    // Don't calculate fault distances for cycles at the end
                    // of a fault because these cycles include non-faulted data
                    int endSample = fault.EndSample - (SamplesPerCycle - 1);

                    // Initialize a fault curve for each algorithm
                    fault.Curves.AddRange(FaultLocationAlgorithms
                        .Select(algorithm => fault.CreateCurve(algorithm.Method.Name))
                        .ToList());

                    foreach (Fault.Segment segment in fault.Segments)
                    {
                        if (endSample > segment.StartSample)
                        {
                            // Get a subset of the cycle data that contains only the data
                            // in the current segment, up to and including the end sample
                            subGroup = CycleDataGroup.ToSubSet(segment.StartSample, Math.Min(segment.EndSample, endSample));

                            // Set the fault type of the fault location data
                            // set to the fault type of the current segment
                            FaultLocationDataSet.FaultType = segment.FaultType;

                            // Push data from the cycle data set
                            // to the fault location data set
                            FaultLocationDataSet.Cycles.Clear();
                            CopyToFaultLocationDataSet(subGroup.VA, cycleData => cycleData.AN.V);
                            CopyToFaultLocationDataSet(subGroup.VB, cycleData => cycleData.BN.V);
                            CopyToFaultLocationDataSet(subGroup.VC, cycleData => cycleData.CN.V);
                            CopyToFaultLocationDataSet(subGroup.IA, cycleData => cycleData.AN.I);
                            CopyToFaultLocationDataSet(subGroup.IB, cycleData => cycleData.BN.I);
                            CopyToFaultLocationDataSet(subGroup.IC, cycleData => cycleData.CN.I);

                            // For single-phase faults, if we don't have the appropriate
                            // phase current to execute the fault location algorithms,
                            // we can use the residual current channel instead
                            if (segment.FaultType == FaultType.AN && subGroup.IA == null)
                                CopyToFaultLocationDataSet(subGroup.IR, cycleData => cycleData.AN.I);
                            else if (segment.FaultType == FaultType.BN && subGroup.IB == null)
                                CopyToFaultLocationDataSet(subGroup.IR, cycleData => cycleData.BN.I);
                            else if (segment.FaultType == FaultType.CN && subGroup.IC == null)
                                CopyToFaultLocationDataSet(subGroup.IR, cycleData => cycleData.CN.I);

                            // Attempt to execute each fault location algorithm
                            for (int i = 0; i < FaultLocationAlgorithms.Count; i++)
                            {
                                if (TryExecute(FaultLocationAlgorithms[i], FaultLocationDataSet, out faultDistances))
                                {
                                    // Create a data point for each of the fault distances
                                    faultDataPoints = subGroup.VA.RMS.DataPoints
                                        .Zip(faultDistances, (point, distance) => new DataPoint() { Time = point.Time, Value = distance })
                                        .ToList();
                                }
                                else
                                {
                                    // Generate NaN-value data points to
                                    // fill this segment of the fault curve
                                    faultDataPoints = subGroup.VA.RMS.DataPoints
                                        .Select(point => new DataPoint() { Time = point.Time, Value = double.NaN })
                                        .ToList();
                                }

                                // Add the data points to the current fault curve
                                fault.Curves[i].Series.DataPoints.AddRange(faultDataPoints);
                            }
                        }
                    }
                }
            }

            private void CopyToFaultLocationDataSet(CycleDataGroup cycleDataGroup, Func<CycleData, Cycle> cycleSelector)
            {
                if (cycleDataGroup == null)
                    return;

                int numDataPoints = cycleDataGroup.RMS.DataPoints.Count;

                for (int i = FaultLocationDataSet.Cycles.Count; i < numDataPoints; i++)
                    FaultLocationDataSet.Cycles[i] = new CycleData();

                for (int i = 0;  i < numDataPoints; i++)
                {
                    CycleData cycleData = FaultLocationDataSet.Cycles[i];
                    Cycle cycle = cycleSelector(cycleData);
                    cycle.RMS = cycleDataGroup.RMS[i].Value;
                    cycle.Phase = cycleDataGroup.Phase[i].Value;
                    cycle.Peak = cycleDataGroup.Peak[i].Value;
                    cycle.Error = cycleDataGroup.Error[i].Value;
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

        #endregion

        #region [ Properties ]

        public Dictionary<DataGroup, FaultGroup> FaultLookup { get; }
            = new Dictionary<DataGroup, FaultGroup>();

        [Category]
        [SettingName(SystemSection.CategoryName)]
        public SystemSection SystemSettings { get; }
            = new SystemSection();

        [Category]
        [SettingName(DataAnalysisSection.CategoryName)]
        public DataAnalysisSection DataAnalysisSettings { get; }
            = new DataAnalysisSection();

        [Category]
        [SettingName(FaultLocationSection.CategoryName)]
        public FaultLocationSection FaultLocationSettings { get; }
            = new FaultLocationSection();

        [Category]
        [SettingName(BreakerSection.CategoryName)]
        public BreakerSection BreakerSettings { get; }
            = new BreakerSection();

        private Func<Asset, DateTime, bool> AskSCADAIfBreakerOpened { get; set; }

        #endregion

        #region [ Methods ]

        public override void Initialize(MeterDataSet meterDataSet)
        {
            Stopwatch stopwatch = new Stopwatch();
            CycleDataResource cycleDataResource = meterDataSet.GetResource<CycleDataResource>();
            List<FaultLocationAlgorithm> faultLocationAlgorithms;

            using (AdoDataConnection connection = meterDataSet.CreateDbConnection())
            {
                TableOperations<openXDA.Model.FaultLocationAlgorithm> faultLocationAlgorithmTable = new TableOperations<openXDA.Model.FaultLocationAlgorithm>(connection);
                faultLocationAlgorithms = GetFaultLocationAlgorithms(faultLocationAlgorithmTable);
            }

            SCADADataResource scadaDataResource = meterDataSet.GetResource<SCADADataResource>();
            AskSCADAIfBreakerOpened = scadaDataResource.DidBreakerOpen;

            Log.Info(string.Format("Executing fault location analysis on {0} events.", cycleDataResource.DataGroups.Count));

            for (int i = 0; i < cycleDataResource.DataGroups.Count; i++)
            {
                DataGroup dataGroup = cycleDataResource.DataGroups[i];
                VIDataGroup viDataGroup = cycleDataResource.VIDataGroups[i];
                VICycleDataGroup viCycleDataGroup = cycleDataResource.VICycleDataGroups[i];

                // Defined channel checks
                Log.Debug("Checking defined channels...");

                if (viDataGroup.DefinedNeutralVoltages != 3)
                {
                    Log.Debug($"Not enough neutral voltage channels for fault analysis: {viDataGroup.DefinedNeutralVoltages}.");
                    continue;
                }

                if (viDataGroup.DefinedCurrents < 3 && viDataGroup.IR == null)
                {
                    Log.Debug($"Not enough current channels for fault analysis: {viDataGroup.DefinedNeutralVoltages}.");
                    continue;
                }

                //Make Sure it is actually a Line
                Log.Debug("Checking whether Asset is a Line...");
                if (dataGroup.Asset.AssetTypeID != (int) AssetType.Line)
                {
                    Log.Debug($"Asset: {dataGroup.Asset.AssetKey} is not a Line.");
                    continue;
                }

                // Engineering reasonableness checks
                Log.Debug("Checking for engineering reasonableness...");

                try
                {
                    stopwatch.Restart();

                    if (!IsReasonable(dataGroup, viCycleDataGroup))
                        continue;
                }
                finally
                {
                    Log.Debug(stopwatch.Elapsed);
                }

                Log.Debug("Checking whether impedances are defined...");

                // Create the fault location data set and begin populating
                // the properties necessary for calculating fault location

                if (!((AssetType)dataGroup.Asset.AssetTypeID == AssetType.Line || (AssetType)dataGroup.Asset.AssetTypeID == AssetType.Transformer))
                {
                    Log.Debug("Not a Line; skipping fault analysis.");
                    continue;
                }

                Line line = Line.DetailedLine(dataGroup.Asset);

                FaultLocationDataSet faultLocationDataSet = new FaultLocationDataSet();
                faultLocationDataSet.LineDistance = line.Path[0].Length;
                faultLocationDataSet.PrefaultCycle = FirstCycle(viCycleDataGroup);

                // Extract impedances from the database
                // and into the fault location data set
                ImpedanceExtractor impedanceExtractor = new ImpedanceExtractor();
                impedanceExtractor.FaultLocationDataSet = faultLocationDataSet;
                impedanceExtractor.Meter = meterDataSet.Meter;
                impedanceExtractor.Line = line;

                if (!impedanceExtractor.TryExtractImpedances())
                {
                    Log.Debug("No impedances defined; skipping fault analysis.");
                    continue;
                }

                // Break into faults and segments
                Func<List<Fault>> detectFaultsByCurrent = () => DetectFaultsByCurrent(viDataGroup, viCycleDataGroup);
                Func<List<Fault>> detectFaultsByVoltage = () => DetectSinglePhaseFaultsByVoltage(viCycleDataGroup, viDataGroup.IR);
                Func<List<Fault>> detectFaults = (viDataGroup.DefinedCurrents >= 3) ? detectFaultsByCurrent : detectFaultsByVoltage;

                Action<List<Fault>> classifyFaultsByCurrent = faultList => ClassifyFaultsByCurrent(faultList, dataGroup, viCycleDataGroup);
                Action<List<Fault>> classifyFaultsByVoltage = faultList => ClassifyFaultsByVoltage(faultList, dataGroup, viCycleDataGroup);
                Action<List<Fault>> classifyFaults = (viDataGroup.DefinedCurrents >= 3) ? classifyFaultsByCurrent : classifyFaultsByVoltage;

                List<Fault> faults;

                Log.Debug("Classifying data into faults and segments...");

                try
                {
                    stopwatch.Restart();

                    faults = detectFaults();

                    if (faults.Count > 0)
                        classifyFaults(faults);
                }
                finally
                {
                    Log.Debug(stopwatch.Elapsed);
                }

                // Check the fault detection logic and the default fault detection logic
                bool? faultDetectionLogicResult = CheckFaultDetectionLogic(meterDataSet, dataGroup);
                bool defaultFaultDetectionLogicResult = CheckDefaultFaultDetectionLogic(faults);

                // If the fault detection logic detects a fault and the default
                // logic does not agree, treat the whole waveform as a fault
                if (faultDetectionLogicResult == true && !defaultFaultDetectionLogicResult)
                {
                    faults.Add(new Fault()
                    {
                        StartSample = 0,
                        EndSample = dataGroup[0].DataPoints.Count - 1
                    });

                    classifyFaults(faults);
                }

                // Generate fault curves for fault analysis
                Log.Debug("Generating fault curves...");
                stopwatch.Restart();

                FaultCurveGenerator faultCurveGenerator = new FaultCurveGenerator();
                faultCurveGenerator.SamplesPerCycle = Transform.CalculateSamplesPerCycle(viDataGroup.VA, DataAnalysisSettings.SystemFrequency);
                faultCurveGenerator.CycleDataGroup = viCycleDataGroup;
                faultCurveGenerator.Faults = faults;
                faultCurveGenerator.FaultLocationDataSet = faultLocationDataSet;
                faultCurveGenerator.FaultLocationAlgorithms = faultLocationAlgorithms;
                faultCurveGenerator.GenerateFaultCurves();

                Log.Debug(stopwatch.Elapsed);

                // Gather additional info about each fault
                // based on the results of the above analysis
                LightningDataResource lightningDataResource = meterDataSet.GetResource<LightningDataResource>();

                if (!lightningDataResource.LightningStrikeLookup.TryGetValue(dataGroup, out List<ILightningStrike> lightningStrikes))
                    lightningStrikes = new List<ILightningStrike>();

                using (AdoDataConnection connection = meterDataSet.CreateDbConnection())
                {
                    foreach (Fault fault in faults)
                        PopulateFaultInfo(fault, dataGroup, viCycleDataGroup, viDataGroup, lightningStrikes, connection);
                }
                // Create a fault group and add it to the lookup table
                bool faultValidationLogicResult = CheckFaultValidationLogic(faults);
                FaultLookup.Add(dataGroup, new FaultGroup(faults, faultDetectionLogicResult, defaultFaultDetectionLogicResult, faultValidationLogicResult));
            }
        }

        private bool? CheckFaultDetectionLogic(MeterDataSet meterDataSet, DataGroup dataGroup)
        {
            string expressionText;
            int meterLineID;

            expressionText = null;

            // Find MeterLine record corresponding to the meter that produced
            // the data and the line associated with the data group
            meterLineID = meterDataSet.Meter.MeterAssets
                .Where(meterLine => meterLine.AssetID == dataGroup.Asset.ID)
                .Select(meterLine => (int?)meterLine.ID)
                .FirstOrDefault() ?? -1;

            if (meterLineID > 0)
            {
                using (AdoDataConnection connection = meterDataSet.CreateDbConnection())
                {
                    TableOperations<FaultDetectionLogic> faultDetectionLogicTable = new TableOperations<FaultDetectionLogic>(connection);

                    // Find fault detection logic defined for the meter and line
                    expressionText = faultDetectionLogicTable
                        .QueryRecordsWhere("MeterAssetID = {0}", meterLineID)
                        .Select(logic => logic.Expression)
                        .FirstOrDefault();
                }
            }

            try
            {
                if ((object)expressionText == null)
                {
                    if (FaultLocationSettings.WarnMissingDetectionLogic)
                        throw new Exception($"Expression text is not defined for line '{dataGroup.Asset.AssetKey}'.");

                    return null;
                }

                // Parse fault detection logic into a boolean expression
                BooleanExpression expression = new BooleanExpression(expressionText);

                // Put digital values into a lookup table
                Dictionary<string, bool> digitalLookup = dataGroup.DataSeries
                    .Where(series => series.SeriesInfo.Channel.MeasurementType.Name.Equals("Digital", StringComparison.OrdinalIgnoreCase))
                    .GroupBy(series => series.SeriesInfo.Channel.Name)
                    .Where(grouping => grouping.Count() == 1)
                    .ToDictionary(grouping => grouping.Key, grouping => grouping.Single().DataPoints.Any(dataPoint => Convert.ToBoolean(dataPoint.Value)), StringComparer.OrdinalIgnoreCase);

                // Apply the digital values to the variables in the boolean expression
                foreach (BooleanExpression.Variable variable in expression.Variables)
                {
                    if (!digitalLookup.TryGetValue(variable.Identifier, out variable.Value))
                        Log.Warn($"Channel '{variable.Identifier}' that was required for fault detection logic was missing from the meter data set.");
                }

                // Evaluate the boolean expression
                return expression.Evaluate();
            }
            catch (Exception ex)
            {
                // Log the exception as a warning
                Log.Warn(ex.Message, ex);
                return null;
            }
        }

        private bool CheckDefaultFaultDetectionLogic(List<Fault> faults)
        {
            return faults.Any();
        }

        private bool CheckFaultValidationLogic(List<Fault> faults)
        {
            return faults.Any(fault => !fault.IsSuppressed && fault.Summaries.Any(summary => summary.IsValid));
        }

        private List<FaultLocationAlgorithm> GetFaultLocationAlgorithms(TableOperations<openXDA.Model.FaultLocationAlgorithm> faultLocationAlgorithmTable)
        {
            return faultLocationAlgorithmTable
                .QueryRecords("ExecutionOrder, ID")
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

        private List<Fault> Merge(params List<Fault>[] faultLists)
        {
            IEnumerable<Fault> allFaults = faultLists
                .SelectMany(list => list)
                .OrderBy(fault => fault.StartSample);

            List<Fault> faults = new List<Fault>();
            Fault currentFault = null;
            bool overlaps;

            foreach (Fault fault in allFaults)
            {
                overlaps = ((object)currentFault != null) &&
                           fault.StartSample <= currentFault.EndSample;

                if (overlaps)
                {
                    currentFault.EndSample = Math.Max(currentFault.EndSample, fault.EndSample);
                }
                else
                {
                    currentFault = new Fault()
                    {
                        StartSample = fault.StartSample,
                        EndSample = fault.EndSample
                    };

                    faults.Add(currentFault);
                }
            }

            return faults;
        }

        private bool IsReasonable(DataGroup dataGroup, VICycleDataGroup viCycleDataGroup)
        {
            // Get the line-to-neutral nominal voltage in volts
            double nominalVoltage = dataGroup.Asset.VoltageKV * 1000.0D / Math.Sqrt(3.0D);

            DataSeries[] voltages =
            {
                viCycleDataGroup.VA.RMS.Multiply(1.0D / nominalVoltage),
                viCycleDataGroup.VB.RMS.Multiply(1.0D / nominalVoltage),
                viCycleDataGroup.VC.RMS.Multiply(1.0D / nominalVoltage)
            };

            DataSeries[] currents =
            {
                viCycleDataGroup.IA?.RMS,
                viCycleDataGroup.IB?.RMS,
                viCycleDataGroup.IC?.RMS,
                viCycleDataGroup.IR?.RMS
            };

            // Determine if any of the RMS voltages are unreasonably high
            if (voltages.Any(voltage => voltage.DataPoints.Any(dataPoint => dataPoint.Value > DataAnalysisSettings.MaxVoltage)))
            {
                Log.Debug("Data unreasonable: voltage > maxVoltage");
                return false;
            }

            // Determine if any of the RMS currents are unreasonably high
            if (currents.Any(current => current != null && current.DataPoints.Any(dataPoint => dataPoint.Value > DataAnalysisSettings.MaxCurrent)))
            {
                Log.Debug("Data unreasonable: current > maxCurrent");
                return false;
            }

            return true;
        }

        private void Classify(Fault fault, Func<int, FaultType> getFaultType, Func<int, DateTime> getTime)
        {
            TimeSpan minSegmentDuration = TimeSpan.FromSeconds(FaultLocationSettings.MinFaultSegmentCycles / DataAnalysisSettings.SystemFrequency);

            Action<int, FaultType> UpdateSegments = new Func<Action<int, FaultType>>(() =>
            {
                var nullTransition = new { Sample = default(int), FaultType = default(FaultType) };
                var previousTransition = nullTransition;

                return (sample, faultType) =>
                {
                    var nextTransition = new { Sample = sample, FaultType = faultType };

                    if (previousTransition == nullTransition)
                    {
                        previousTransition = nextTransition;
                    }
                    else if (previousTransition.FaultType != nextTransition.FaultType)
                    {
                        DateTime start = getTime(previousTransition.Sample);
                        DateTime end = getTime(nextTransition.Sample);
                        TimeSpan duration = end - start;

                        if (duration >= minSegmentDuration)
                        {
                            fault.Segments.Add(new Fault.Segment(previousTransition.FaultType)
                            {
                                StartSample = previousTransition.Sample,
                                EndSample = nextTransition.Sample - 1
                            });
                        }

                        previousTransition = nextTransition;
                    }
                };
            })();

            for (int sample = fault.StartSample; sample <= fault.EndSample; sample++)
            {
                FaultType faultType = getFaultType(sample);
                UpdateSegments(sample, faultType);
            }

            // We found all the fault type transitions during the fault,
            // but there is always one more transition at the very end
            UpdateSegments(fault.EndSample + 1, FaultType.None);

            for (int i = 0; i < fault.Segments.Count; i++)
            {
                Fault.Segment segment = fault.Segments[i];

                // Fill gaps around segments in regions
                // where transitions happened too quickly
                if (i == 0)
                    segment.StartSample = fault.StartSample;

                if (i + 1 == fault.Segments.Count)
                    segment.EndSample = fault.EndSample;

                if (i + 1 < fault.Segments.Count)
                {
                    Fault.Segment next = fault.Segments[i + 1];
                    int mid = (segment.EndSample + next.StartSample) / 2;
                    segment.EndSample = mid;
                    next.StartSample = mid + 1;
                }

                // Don't forget to update start/end time
                // based on the latest start/end samples
                segment.StartTime = getTime(segment.StartSample);
                segment.EndTime = getTime(segment.EndSample);
            }
        }

        private int FindFaultInception(DataSeries waveForm, int cycleIndex)
        {
            int samplesPerCycle = Transform.CalculateSamplesPerCycle(waveForm, DataAnalysisSettings.SystemFrequency);
            int startIndex = cycleIndex;
            int endIndex = cycleIndex + samplesPerCycle - 1;
            int prefaultIndex = Math.Max(0, startIndex - samplesPerCycle);

            double largestPrefaultPeak;
            double largestFaultCyclePeak;

            double previousValue;
            double value;
            double nextValue;

            largestPrefaultPeak = 0.0D;
            largestFaultCyclePeak = 0.0D;

            // Find the largest prefault peak as the absolute
            // peak of the cycle before the first faulted cycle
            for (int i = prefaultIndex; i < startIndex; i++)
            {
                value = Math.Abs(waveForm[i].Value);

                if (value > largestPrefaultPeak)
                    largestPrefaultPeak = value;
            }

            // Find the largest peak of the first faulted cycle
            for (int i = startIndex; i <= endIndex; i++)
            {
                value = Math.Abs(waveForm[i].Value);

                if (value > largestFaultCyclePeak)
                    largestFaultCyclePeak = value;
            }

            // Find the first point where the value exceeds a point 25%
            // of the way from the prefault peak to the fault peak
            for (int i = startIndex; i <= endIndex; i++)
            {
                value = Math.Abs(waveForm[i].Value);

                if (value >= (largestPrefaultPeak * 0.75 + largestFaultCyclePeak * 0.25))
                    endIndex = i;
            }

            // Starting from the point found in the previous loop and
            // scanning backwards, find either the first zero crossing
            // or the first point at which the slope changes drastically
            for (int i = endIndex; i >= startIndex; i--)
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

        private int FindFaultInception(DataSeries waveForm, DataSeries rms, int cycleIndex)
        {
            int samplesPerCycle = Transform.CalculateSamplesPerCycle(waveForm, DataAnalysisSettings.SystemFrequency);
            int startIndex = -1;

            // Adjust cycleIndex using an adaptive threshhold
            double adaptiveThreshold = (rms[cycleIndex].Value - rms[0].Value)*0.25 + rms[0].Value;

          
            for (int i = cycleIndex; i > 0; i--)
            {
                if (rms[i].Value < adaptiveThreshold)
                {
                    startIndex = i;
                    break;
                }
            }

            if (adaptiveThreshold > rms[cycleIndex].Value)
            {
                startIndex = cycleIndex;
            }

            if (startIndex == -1)
            {
                startIndex = cycleIndex;
                Log.Debug("Adaptive Thersholding failed....");
                Log.Debug("Fallbvack to original Algorithm....");
            }

            int endIndex = cycleIndex + samplesPerCycle - 1;
            int prefaultIndex = Math.Max(0, startIndex - samplesPerCycle);

            double largestPrefaultPeak;
            double largestFaultCyclePeak;

            double previousValue;
            double value;
            double nextValue;

            largestPrefaultPeak = 0.0D;
            largestFaultCyclePeak = 0.0D;

            // Adjust the startIndex using an adaptive threshhold
            double adaptive_Threshhold = (waveForm[cycleIndex].Value - waveForm[cycleIndex].Value);


            // Find the largest prefault peak as the absolute
            // peak of the cycle before the first faulted cycle
            for (int i = prefaultIndex; i < startIndex; i++)
            {
                value = Math.Abs(waveForm[i].Value);

                if (value > largestPrefaultPeak)
                    largestPrefaultPeak = value;
            }

            // Find the largest peak of the first faulted cycle
            for (int i = startIndex; i <= endIndex; i++)
            {
                value = Math.Abs(waveForm[i].Value);

                if (value > largestFaultCyclePeak)
                    largestFaultCyclePeak = value;
            }

            // Find the first point where the value exceeds a point 25%
            // of the way from the prefault peak to the fault peak
            for (int i = startIndex; i <= endIndex; i++)
            {
                value = Math.Abs(waveForm[i].Value);

                if (value >= (largestPrefaultPeak * 0.75 + largestFaultCyclePeak * 0.25))
                    endIndex = i;
            }

            // Starting from the point found in the previous loop and
            // scanning backwards, find either the first zero crossing
            // or the first point at which the slope changes drastically
            for (int i = endIndex; i >= startIndex; i--)
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
            int samplesPerCycle = Transform.CalculateSamplesPerCycle(waveForm, DataAnalysisSettings.SystemFrequency);
            int startIndex = cycleIndex - 1;
            int endIndex = startIndex + samplesPerCycle - 1;
            int postfaultIndex = Math.Min(endIndex + samplesPerCycle, waveForm.DataPoints.Count - 1);

            double largestPostfaultPeak;
            double largestFaultCyclePeak;

            double previousValue;
            double value;
            double nextValue;

            largestPostfaultPeak = 0.0D;
            largestFaultCyclePeak = 0.0D;

            // Find the largest postfault peak as the absolute
            // peak of the cycle after the last faulted cycle
            for (int i = postfaultIndex; i > endIndex; i--)
            {
                value = Math.Abs(waveForm[i].Value);

                if (value > largestPostfaultPeak)
                    largestPostfaultPeak = value;
            }

            // Find the largest peak of the last faulted cycle
            for (int i = startIndex; i <= endIndex; i++)
            {
                value = Math.Abs(waveForm[i].Value);

                if (value > largestFaultCyclePeak)
                    largestFaultCyclePeak = value;
            }

            // Scanning backwards, find the first point where the value exceeds
            // a point 25% of the way from the postfault peak to the fault peak
            for (int i = endIndex; i >= startIndex; i--)
            {
                value = Math.Abs(waveForm[i].Value);

                if (value >= (largestPostfaultPeak * 0.75 + largestFaultCyclePeak * 0.25))
                    startIndex = i;
            }

            // Starting from the point found in the previous loop,
            // find either the first zero crossing or the first point
            // at which the slope changes drastically
            for (int i = startIndex; i <= endIndex; i++)
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

            return startIndex;
        }

        private CycleData GetCycle(VICycleDataGroup viCycleDataGroup, int index)
        {
            CycleData cycle = new CycleData();

            Cycle[] cycles =
            {
                cycle.AN.V,
                cycle.BN.V,
                cycle.CN.V,
                cycle.AN.I,
                cycle.BN.I,
                cycle.CN.I
            };

            CycleDataGroup[] cycleDataGroups =
            {
                viCycleDataGroup.VA,
                viCycleDataGroup.VB,
                viCycleDataGroup.VC,
                viCycleDataGroup.IA,
                viCycleDataGroup.IB,
                viCycleDataGroup.IC
            };

            for (int i = 0; i < cycles.Length; i++)
            {
                if (cycleDataGroups[i] == null)
                    continue;

                cycles[i].RMS = cycleDataGroups[i].RMS.DataPoints[index].Value;
                cycles[i].Phase = cycleDataGroups[i].Phase.DataPoints[index].Value;
                cycles[i].Peak = cycleDataGroups[i].Peak.DataPoints[index].Value;
                cycles[i].Error = cycleDataGroups[i].Error.DataPoints[index].Value;
            }

            return cycle;
        }

        private CycleData FirstCycle(VICycleDataGroup viCycleDataGroup)
        {
            return GetCycle(viCycleDataGroup, 0);
        }

        private void PopulateFaultInfo(Fault fault, DataGroup dataGroup, VICycleDataGroup viCycleDataGroup, VIDataGroup viDataGroup, List<ILightningStrike> lightningStrikes, AdoDataConnection connection)
        {
            int samplesPerCycle = Transform.CalculateSamplesPerCycle(dataGroup.SamplesPerSecond, DataAnalysisSettings.SystemFrequency);
            int calculationCycle = GetCalculationCycle(fault, viCycleDataGroup, samplesPerCycle, connection);
            DateTime startTime = dataGroup[0][fault.StartSample].Time;
            DateTime endTime = dataGroup[0][fault.EndSample].Time;
            double prefaultPeak = GetPrefaultPeak(fault, dataGroup, viCycleDataGroup);
            double postfaultPeak = GetPostfaultPeak(fault, dataGroup, viCycleDataGroup);

            fault.CalculationCycle = calculationCycle;
            fault.InceptionTime = startTime;
            fault.ClearingTime = endTime;
            fault.Duration = endTime - startTime;
            fault.PrefaultCurrent = GetPrefaultCurrent(fault, dataGroup, viCycleDataGroup);
            fault.PostfaultCurrent = GetPostfaultCurrent(fault, dataGroup, viCycleDataGroup);

            fault.IsSuppressed =
                (double.IsNaN(postfaultPeak) && viDataGroup.DefinedCurrents > 1) ||
                (!double.IsNaN(postfaultPeak) && postfaultPeak > BreakerSettings.OpenBreakerThreshold) ||
                !AskSCADAIfBreakerOpened(dataGroup.Asset, fault.ClearingTime);

            fault.IsReclose =
                !double.IsNaN(prefaultPeak) &&
                !double.IsNaN(postfaultPeak) &&
                prefaultPeak <= BreakerSettings.OpenBreakerThreshold &&
                postfaultPeak <= BreakerSettings.OpenBreakerThreshold;

            if (fault.Segments.Any())
            {
                fault.Type = fault.Segments
                    .Where(segment => segment.StartSample <= fault.CalculationCycle)
                    .Where(segment => fault.CalculationCycle <= segment.EndSample)
                    .Select(segment => segment.FaultType)
                    .FirstOrDefault();

                fault.CurrentMagnitude = GetFaultCurrentMagnitude(viCycleDataGroup, fault.Type, calculationCycle);
                fault.CurrentLag = GetFaultCurrentLag(viCycleDataGroup, fault.Type, calculationCycle);
                fault.TreeFaultResistance = GetTreeFaultResistance(fault, dataGroup, viCycleDataGroup);
                fault.LightningMilliseconds = GetLightningMilliseconds(fault, lightningStrikes);
                fault.InceptionDistanceFromPeak = GetInceptionDistanceFromPeak(fault, viCycleDataGroup, samplesPerCycle);
                fault.PrefaultThirdHarmonic = GetPrefaultThirdHarmonic(fault, viDataGroup, viCycleDataGroup, samplesPerCycle);
                fault.GroundCurrentRatio = GetGroundCurrentRatio(fault, viCycleDataGroup);
                fault.LowPrefaultCurrentRatio = GetLowPrefaultCurrentRatio(fault, viCycleDataGroup, samplesPerCycle);

                if (calculationCycle >= 0)
                {
                    CycleData reactanceRatioCycle = GetCycle(viCycleDataGroup, calculationCycle);
                    ComplexNumber voltage = FaultLocationAlgorithms.GetFaultVoltage(reactanceRatioCycle, fault.Type);
                    ComplexNumber current = FaultLocationAlgorithms.GetFaultCurrent(reactanceRatioCycle, fault.Type);

                    double impedanceMagnitude = (voltage / current).Magnitude;
                    double impedanceReactance = (voltage / current).Imaginary;
                    fault.ReactanceRatio = impedanceReactance / impedanceMagnitude;
                }
            }

            if (calculationCycle >= 0)
            {
                for (int i = 0; i < fault.Curves.Count; i++)
                {
                    Fault.Summary summary = new Fault.Summary();
                    summary.DistanceAlgorithmIndex = i;
                    summary.DistanceAlgorithm = fault.Curves[i].Algorithm;
                    summary.Distance = fault.Curves[i][calculationCycle].Value;
                    summary.IsValid = IsValid(summary.Distance, dataGroup);

                    fault.Summaries.Add(summary);
                }

                if (fault.Summaries.Any(s => !s.IsValid))
                    fault.IsSuppressed |= fault.CurrentLag < 0;

                List<Fault.Summary> validSummaries = fault.Summaries
                    .Where(s => s.IsValid)
                    .OrderBy(s => s.Distance)
                    .ToList();

                if (!validSummaries.Any())
                {
                    validSummaries = fault.Summaries
                        .Where(s => !double.IsNaN(s.Distance))
                        .OrderBy(s => s.Distance)
                        .ToList();
                }

                if (validSummaries.Any())
                    validSummaries[validSummaries.Count / 2].IsSelectedAlgorithm = true;
            }
            else
            {
                fault.IsSuppressed = true;
            }
        }

        private bool IsValid(double faultDistance, DataGroup dataGroup)
        {
            Line line = Line.DetailedLine(dataGroup.Asset);
            double maxFaultDistanceMultiplier = FaultLocationSettings.MaxFaultDistanceMultiplier;
            double minFaultDistanceMultiplier = FaultLocationSettings.MinFaultDistanceMultiplier;

            double maxDistance = line.MaxFaultDistance
                ?? maxFaultDistanceMultiplier * line.Path[0].Length;

            double minDistance = line.MinFaultDistance
                ?? minFaultDistanceMultiplier * line.Path[0].Length;

            return faultDistance >= minDistance && faultDistance <= maxDistance;
        }

        private double GetPrefaultCurrent(Fault fault, DataGroup dataGroup, VICycleDataGroup viCycleDataGroup)
        {
            int samplesPerCycle = Transform.CalculateSamplesPerCycle(dataGroup.SamplesPerSecond, DataAnalysisSettings.SystemFrequency);
            int start = fault.StartSample - samplesPerCycle;
            int end = fault.StartSample;

            double?[] minRMS =
            {
                viCycleDataGroup.IA?.RMS.ToSubSeries(start, end).Minimum,
                viCycleDataGroup.IB?.RMS.ToSubSeries(start, end).Minimum,
                viCycleDataGroup.IC?.RMS.ToSubSeries(start, end).Minimum
            };

            return minRMS
                .Where(rms => rms != null)
                .Select(rms => rms.GetValueOrDefault())
                .DefaultIfEmpty(double.NaN)
                .Min();
        }

        private double GetPostfaultCurrent(Fault fault, DataGroup dataGroup, VICycleDataGroup viCycleDataGroup)
        {
            int samplesPerCycle = Transform.CalculateSamplesPerCycle(dataGroup.SamplesPerSecond, DataAnalysisSettings.SystemFrequency);
            int start = fault.EndSample + 1;
            int end = fault.EndSample + samplesPerCycle;

            double?[] minRMS =
            {
                viCycleDataGroup.IA?.RMS.ToSubSeries(start, end).Minimum,
                viCycleDataGroup.IB?.RMS.ToSubSeries(start, end).Minimum,
                viCycleDataGroup.IC?.RMS.ToSubSeries(start, end).Minimum
            };

            return minRMS
                .Where(rms => rms != null)
                .Select(rms => rms.GetValueOrDefault())
                .DefaultIfEmpty(double.NaN)
                .Min();
        }

        private double GetPrefaultPeak(Fault fault, DataGroup dataGroup, VICycleDataGroup viCycleDataGroup)
        {
            int samplesPerCycle = Transform.CalculateSamplesPerCycle(dataGroup.SamplesPerSecond, DataAnalysisSettings.SystemFrequency);
            int start = fault.StartSample - 5 * samplesPerCycle;
            int end = fault.StartSample - samplesPerCycle - 1;

            double?[] minPeaks =
            {
                viCycleDataGroup.IA?.Peak.ToSubSeries(start, end).Minimum,
                viCycleDataGroup.IB?.Peak.ToSubSeries(start, end).Minimum,
                viCycleDataGroup.IC?.Peak.ToSubSeries(start, end).Minimum
            };

            return minPeaks
                .Where(peak => peak != null)
                .Select(peak => peak.GetValueOrDefault())
                .DefaultIfEmpty(double.NaN)
                .Min();
        }

        private double GetPostfaultPeak(Fault fault, DataGroup dataGroup, VICycleDataGroup viCycleDataGroup)
        {
            int samplesPerCycle = Transform.CalculateSamplesPerCycle(dataGroup.SamplesPerSecond, DataAnalysisSettings.SystemFrequency);
            int start = fault.EndSample + samplesPerCycle + 1;
            int end = fault.EndSample + 5 * samplesPerCycle;

            double?[] minPeaks =
            {
                viCycleDataGroup.IA?.Peak.ToSubSeries(start, end).Minimum,
                viCycleDataGroup.IB?.Peak.ToSubSeries(start, end).Minimum,
                viCycleDataGroup.IC?.Peak.ToSubSeries(start, end).Minimum
            };

            return minPeaks
                .Where(peak => peak != null)
                .Select(peak => peak.GetValueOrDefault())
                .DefaultIfEmpty(double.NaN)
                .Min();
        }

        private int GetCalculationCycle(Fault fault, VICycleDataGroup viCycleDataGroup, int samplesPerCycle, AdoDataConnection connection)
        {
            if (!fault.Curves.Any())
                return -1;

            int minFaultCurveLength = fault.Curves.Min(curve => curve.Series.DataPoints.Count);

            if (minFaultCurveLength == 0)
                return -1;

            switch (FaultLocationSettings.FaultCalculationCycleMethod)
            {
                default:
                case FaultCalculationCycleMethod.MaxCurrent:
                    return GetCycleWithMaximumCurrent(fault, viCycleDataGroup, samplesPerCycle);

                case FaultCalculationCycleMethod.LastFaultedCycle:
                    return GetLastFaultedCycle(fault, samplesPerCycle);
                case FaultCalculationCycleMethod.LastFaultedCycleExceptAirGapRes:
                    if (isAirGapResistorCurrent(viCycleDataGroup, connection))
                        return GetCycleWithMaximumCurrent(fault, viCycleDataGroup, samplesPerCycle);
                    return GetLastFaultedCycle(fault, samplesPerCycle);
            }
        }

        private bool isAirGapResistorCurrent(VICycleDataGroup viCycleDataGroup, AdoDataConnection connection)
        {
            AssetTypes breakerType = new TableOperations<AssetTypes>(connection).QueryRecordWhere("Name = 'Breaker'");
            if (breakerType == null)
                return false;

            Func<Asset, bool> isBreaker = new Func<Asset, bool>((asset) => asset.AssetTypeID == breakerType.ID);
            Func<Asset, bool> hasAirGapResistor = new Func<Asset, bool>((asset) => Breaker.DetailedBreaker(asset,connection).AirGapResistor);

            if (isBreaker(viCycleDataGroup.IA.Asset) && hasAirGapResistor(viCycleDataGroup.IA.Asset))
                return true;
            if (isBreaker(viCycleDataGroup.IB.Asset) && hasAirGapResistor(viCycleDataGroup.IB.Asset))
                return true;
            if (isBreaker(viCycleDataGroup.IC.Asset) && hasAirGapResistor(viCycleDataGroup.IC.Asset))
                return true;

            return false;
        }
        private int GetCycleWithMaximumCurrent(Fault fault, VICycleDataGroup viCycleDataGroup, int samplesPerCycle)
        {
            Func<int, double> sumPhases = i =>
            {
                double ia = viCycleDataGroup.IA.RMS[i].Value;
                double ib = viCycleDataGroup.IB.RMS[i].Value;
                double ic = viCycleDataGroup.IC.RMS[i].Value;
                return ia + ib + ic;
            };

            Func<int, double> residual = i => viCycleDataGroup.IR.RMS[i].Value;

            bool phaseCurrentsDefined =
                viCycleDataGroup.IA != null &&
                viCycleDataGroup.IB != null &&
                viCycleDataGroup.IC != null;

            Func<int, double> getCurrent = phaseCurrentsDefined ? sumPhases : residual;

            int startSample = fault.StartSample + samplesPerCycle;
            int endSample = fault.StartSample + fault.Curves.Min(curve => curve.Series.DataPoints.Count) - 1;

            if (startSample > endSample)
                startSample = fault.StartSample;

            double max = double.MinValue;
            int maxIndex = -1;

            for (int i = startSample; i <= endSample; i++)
            {
                double current = getCurrent(i);

                if (current > max)
                {
                    maxIndex = i;
                    max = current;
                }
            }

            return maxIndex;
        }

        private int GetLastFaultedCycle(Fault fault, int samplesPerCycle)
        {
            int SelectEndSample()
            {
                int GetSampleCount(Fault.Segment segment)
                {
                    // The fault type algorithm gets applied to cycles that extend beyond the end of the fault,
                    // so any segments with samples close to the end of the fault cannot be fully trusted
                    int maxEndSample = fault.EndSample - (samplesPerCycle - 1);
                    int startSample = segment.StartSample;
                    int endSample = Math.Min(segment.EndSample, maxEndSample);
                    return endSample - startSample + 1;
                };

                Fault.Segment lastValidSegment = fault.Segments
                    .Where(segment => GetSampleCount(segment) >= samplesPerCycle)
                    .LastOrDefault();

                switch (lastValidSegment?.FaultType)
                {
                    case FaultType.AN:
                    case FaultType.BN:
                    case FaultType.CN:
                        // For single-phase faults, we can simply shift the
                        // calculation cycle by a few samples to account for
                        // minor errors in the clearing time logic
                        return lastValidSegment.EndSample - FaultLocationSettings.FaultClearingAdjustmentSamples;

                    default:
                        // For multi-phase faults, the various phases will clear at different times.
                        // On a three-phase system, there ought to be a maximum of two-thirds of a
                        // cycle (240 degrees) between clearing times for individual phases.
                        // Thus, shifting by a full cycle (360 degrees) will likely adjust for the
                        // different clearing times as well as minor errors in the clearing time calculations
                        return lastValidSegment.EndSample - samplesPerCycle;

                    case null:
                        // Not enough data in any individual segment
                        // to identify a prominent fault type.
                        // Assume multi-phase (360 degree adjustment),
                        // but from the very end of the fault
                        return fault.EndSample - samplesPerCycle;
                }
            }

            // Adjustment to move from the very end of the fault
            // or segment to the beginning of the selected cycle
            int selectedCycle = SelectEndSample() - (samplesPerCycle - 1);

            int minCycle = fault.StartSample;
            int maxCycle = fault.EndSample - (samplesPerCycle - 1);

            // If the selected cycle falls outside the valid range,
            // fall back on the single-phase adjustment from the very end of the fault
            if (selectedCycle < minCycle || selectedCycle > maxCycle)
                selectedCycle = maxCycle - FaultLocationSettings.FaultClearingAdjustmentSamples;

            // If even the single-phase adjustment is invalid,
            // just pick the last valid cycle
            if (selectedCycle < minCycle || selectedCycle > maxCycle)
                selectedCycle = maxCycle;

            return selectedCycle;
        }

        private double GetFaultCurrentMagnitude(VICycleDataGroup viCycleDataGroup, FaultType faultType, int cycle)
        {
            FaultType viFaultType = faultType;

            if (viFaultType == FaultType.ABC || viFaultType == FaultType.ABCG)
            {
                double anError = viCycleDataGroup.IA.Error[cycle].Value;
                double bnError = viCycleDataGroup.IB.Error[cycle].Value;
                double cnError = viCycleDataGroup.IC.Error[cycle].Value;

                if (anError < bnError && anError < cnError)
                    viFaultType = FaultType.AN;
                else if (bnError < anError && bnError < cnError)
                    viFaultType = FaultType.BN;
                else
                    viFaultType = FaultType.CN;
            }

            CycleDataGroup cycleDataGroup;

            ComplexNumber an;
            ComplexNumber bn;
            ComplexNumber cn;

            switch (viFaultType)
            {
                case FaultType.AN:
                    cycleDataGroup = viCycleDataGroup.IA ?? viCycleDataGroup.IR;
                    return cycleDataGroup.RMS[cycle].Value;

                case FaultType.BN:
                    cycleDataGroup = viCycleDataGroup.IB ?? viCycleDataGroup.IR;
                    return cycleDataGroup.RMS[cycle].Value;

                case FaultType.CN:
                    cycleDataGroup = viCycleDataGroup.IC ?? viCycleDataGroup.IR;
                    return cycleDataGroup.RMS[cycle].Value;

                case FaultType.AB:
                case FaultType.ABG:
                    an = ToComplexNumber(viCycleDataGroup.IA, cycle);
                    bn = ToComplexNumber(viCycleDataGroup.IB, cycle);
                    return (an - bn).Magnitude;

                case FaultType.BC:
                case FaultType.BCG:
                    bn = ToComplexNumber(viCycleDataGroup.IB, cycle);
                    cn = ToComplexNumber(viCycleDataGroup.IC, cycle);
                    return (bn - cn).Magnitude;

                case FaultType.CA:
                case FaultType.CAG:
                    cn = ToComplexNumber(viCycleDataGroup.IC, cycle);
                    an = ToComplexNumber(viCycleDataGroup.IA, cycle);
                    return (cn - an).Magnitude;

                default:
                    return double.NaN;
            }
        }

        private double GetFaultCurrentLag(VICycleDataGroup viCycleDataGroup, FaultType faultType, int cycle)
        {
            FaultType viFaultType = faultType;

            if (viFaultType == FaultType.ABC || viFaultType == FaultType.ABCG)
            {
                double anError = viCycleDataGroup.IA.Error[cycle].Value;
                double bnError = viCycleDataGroup.IB.Error[cycle].Value;
                double cnError = viCycleDataGroup.IC.Error[cycle].Value;

                if (anError < bnError && anError < cnError)
                    viFaultType = FaultType.AN;
                else if (bnError < anError && bnError < cnError)
                    viFaultType = FaultType.BN;
                else
                    viFaultType = FaultType.CN;
            }

            CycleDataGroup voltageDataGroup;
            CycleDataGroup currentDataGroup;
            Angle currentLag;

            ComplexNumber van;
            ComplexNumber vbn;
            ComplexNumber vcn;
            ComplexNumber ian;
            ComplexNumber ibn;
            ComplexNumber icn;

            switch (viFaultType)
            {
                case FaultType.AN:
                    voltageDataGroup = viCycleDataGroup.VA;
                    currentDataGroup = viCycleDataGroup.IA ?? viCycleDataGroup.IR;
                    currentLag = voltageDataGroup.Phase[cycle].Value - currentDataGroup.Phase[cycle].Value;
                    return currentLag.ToRange(-Math.PI, false);

                case FaultType.BN:
                    voltageDataGroup = viCycleDataGroup.VB;
                    currentDataGroup = viCycleDataGroup.IB ?? viCycleDataGroup.IR;
                    currentLag = voltageDataGroup.Phase[cycle].Value - currentDataGroup.Phase[cycle].Value;
                    return currentLag.ToRange(-Math.PI, false);

                case FaultType.CN:
                    voltageDataGroup = viCycleDataGroup.VC;
                    currentDataGroup = viCycleDataGroup.IC ?? viCycleDataGroup.IR;
                    currentLag = voltageDataGroup.Phase[cycle].Value - currentDataGroup.Phase[cycle].Value;
                    return currentLag.ToRange(-Math.PI, false);

                case FaultType.AB:
                case FaultType.ABG:
                    van = ToComplexNumber(viCycleDataGroup.VA, cycle);
                    vbn = ToComplexNumber(viCycleDataGroup.VB, cycle);
                    ian = ToComplexNumber(viCycleDataGroup.IA, cycle);
                    ibn = ToComplexNumber(viCycleDataGroup.IB, cycle);
                    return ((van - vbn).Angle - (ian - ibn).Angle).ToRange(-Math.PI, false);

                case FaultType.BC:
                case FaultType.BCG:
                    vbn = ToComplexNumber(viCycleDataGroup.VB, cycle);
                    vcn = ToComplexNumber(viCycleDataGroup.VC, cycle);
                    ibn = ToComplexNumber(viCycleDataGroup.IB, cycle);
                    icn = ToComplexNumber(viCycleDataGroup.IC, cycle);
                    return ((vbn - vcn).Angle - (ibn - icn).Angle).ToRange(-Math.PI, false);

                case FaultType.CA:
                case FaultType.CAG:
                    vcn = ToComplexNumber(viCycleDataGroup.VC, cycle);
                    van = ToComplexNumber(viCycleDataGroup.VA, cycle);
                    icn = ToComplexNumber(viCycleDataGroup.IC, cycle);
                    ian = ToComplexNumber(viCycleDataGroup.IA, cycle);
                    return ((vcn - van).Angle - (icn - ian).Angle).ToRange(-Math.PI, false);

                default:
                    return double.NaN;
            }
        }

        // Tree in line
        private double GetTreeFaultResistance(Fault fault, DataGroup dataGroup, VICycleDataGroup viCycleDataGroup)
        {
            if (fault.CalculationCycle < 0)
                return double.NaN;

            if (!new[] { FaultType.AN, FaultType.BN, FaultType.CN }.Contains(fault.Type))
                return double.NaN;

            CycleDataGroup faultedVoltage = new Func<CycleDataGroup>(() =>
            {
                switch (fault.Type)
                {
                    case FaultType.AN: return viCycleDataGroup.VA;
                    case FaultType.BN: return viCycleDataGroup.VB;
                    case FaultType.CN: return viCycleDataGroup.VC;
                    default: return null;
                }
            })();

            CycleDataGroup faultedCurrent = new Func<CycleDataGroup>(() =>
            {
                switch (fault.Type)
                {
                    case FaultType.AN: return viCycleDataGroup.IA ?? viCycleDataGroup.IR;
                    case FaultType.BN: return viCycleDataGroup.IB ?? viCycleDataGroup.IR;
                    case FaultType.CN: return viCycleDataGroup.IC ?? viCycleDataGroup.IR;
                    default: return null;
                }
            })();

            if (faultedVoltage == null || faultedCurrent == null)
                return double.NaN;

            ComplexNumber faultVoltage = ToComplexNumber(faultedVoltage, fault.CalculationCycle);
            ComplexNumber faultCurrent = ToComplexNumber(faultedCurrent, fault.CalculationCycle);
            ComplexNumber faultImpedance = faultVoltage / faultCurrent;

            Line line = Line.DetailedLine(dataGroup.Asset);

            if (line == null)
                return double.NaN;

            ComplexNumber z0 = new ComplexNumber(line.Path[0].R0, line.Path[0].X0);
            ComplexNumber z1 = new ComplexNumber(line.Path[0].R1, line.Path[0].X1);
            ComplexNumber loopImpedance = (z0 + 2.0D * z1) / 3.0D;

            double zf = faultImpedance.Magnitude;
            double xf = faultImpedance.Imaginary;
            double rs = loopImpedance.Real;
            double xs = loopImpedance.Imaginary;

            double term1 = zf * zf;
            double term2 = xf * xf;
            double term3 = xf * rs / xs;
            return (Math.Sqrt(term1 - term2) - term3);
        }

        // Lightning
        private double GetLightningMilliseconds(Fault fault, List<ILightningStrike> lightningStrikes)
        {
            DateTime inception = TimeZoneInfo.ConvertTimeToUtc(fault.InceptionTime, SystemSettings.XDATimeZoneInfo);

            return lightningStrikes
                .Select(strike => inception - strike.UTCTime)
                .Select(span => span.TotalMilliseconds)
                .Select(new Func<double, double>(Math.Round))
                .Select(Math.Abs)
                .DefaultIfEmpty(double.NaN)
                .Min();
        }

        // Insulation contamination
        private double GetInceptionDistanceFromPeak(Fault fault, VICycleDataGroup viCycleDataGroup, int samplesPerCycle)
        {
            DataSeries angleSeries = new Func<DataSeries>(() =>
            {
                switch (fault.Type)
                {
                    case FaultType.AN: return viCycleDataGroup.VA.Phase;
                    case FaultType.BN: return viCycleDataGroup.VB.Phase;
                    case FaultType.CN: return viCycleDataGroup.VC.Phase;
                    default: return null;
                }
            })();

            if (angleSeries == null)
                return double.NaN;

            int inceptionIndex = fault.StartSample;
            double angle = angleSeries[inceptionIndex].Value * 180.0D / Math.PI;
            double positiveDistance = Math.Abs(angle - 90.0D);
            double negativeDistance = Math.Abs(angle + 90.0D);
            return Math.Min(positiveDistance, negativeDistance);
        }

        // Lightning arrester failure
        private double GetPrefaultThirdHarmonic(Fault fault, VIDataGroup viDataGroup, VICycleDataGroup viCycleDataGroup, int samplesPerCycle)
        {
            int startIndex = fault.StartSample - samplesPerCycle;

            if (startIndex < 0)
                return double.NaN;

            DataSeries PeakSeries = new Func<DataSeries>(() =>
            {
                switch (fault.Type)
                {
                    case FaultType.AN: return viCycleDataGroup.IA?.Peak;
                    case FaultType.BN: return viCycleDataGroup.IB?.Peak;
                    case FaultType.CN: return viCycleDataGroup.IC?.Peak;
                    default: return null;
                }
            })();

            if (PeakSeries == null)
                return double.NaN;

            // If there is no load current before the fault,
            // then the FFT is invalid
            if (PeakSeries[startIndex].Value <= BreakerSettings.OpenBreakerThreshold)
                return double.NaN;

            DataSeries faultedSeries = new Func<DataSeries>(() =>
            {
                switch (fault.Type)
                {
                    case FaultType.AN: return viDataGroup.IA;
                    case FaultType.BN: return viDataGroup.IB;
                    case FaultType.CN: return viDataGroup.IC;
                    default: return null;
                }
            })();

            if (faultedSeries == null)
                return double.NaN;

            int endIndex = startIndex + samplesPerCycle - 1;
            DataSeries prefaultCycle = faultedSeries.ToSubSeries(startIndex, endIndex);

            Complex[] samples = prefaultCycle.DataPoints
                .Select(dataPoint => new Complex(dataPoint.Value, 0))
                .ToArray();

            Fourier.Forward(samples);

            return samples[3].Magnitude / samples[1].Magnitude;
        }

        // Conductor slap
        private double GetGroundCurrentRatio(Fault fault, VICycleDataGroup viCycleDataGroup)
        {
            if (fault.CalculationCycle < 0)
                return double.NaN;

            if (viCycleDataGroup.IR == null)
                return double.NaN;

            double faultMagnitude = fault.CurrentMagnitude;
            double groundCurrent = viCycleDataGroup.IR.RMS[fault.CalculationCycle].Value;
            return groundCurrent / faultMagnitude;
        }

        // Conductor Break
        private double GetLowPrefaultCurrentRatio(Fault fault, VICycleDataGroup viCycleDataGroup, int samplesPerCycle)
        {
            // Back up two cycles from the beginning of the fault
            int prefaultCycleIndex = fault.StartSample - 2 * samplesPerCycle;

            if (prefaultCycleIndex < 0)
                return double.NaN;

            double[] prefaultValues = new[] { viCycleDataGroup.IA, viCycleDataGroup.IB, viCycleDataGroup.IC }
                .Where(cycleDataGroup => cycleDataGroup != null)
                .Select(cycleDataGroup => cycleDataGroup.RMS[prefaultCycleIndex].Value)
                .OrderBy(value => value)
                .ToArray();

            if (prefaultValues.Length < 2)
                return double.NaN;

            double min = prefaultValues[0];
            double nextMin = prefaultValues[1];
            return min / nextMin;
        }

        private ComplexNumber ToComplexNumber(CycleDataGroup cycleDataGroup, int cycle)
        {
            Angle angle = cycleDataGroup.Phase[cycle].Value;
            double magnitude = cycleDataGroup.RMS[cycle].Value;
            return new ComplexNumber(angle, magnitude);
        }

        #region [ Current-Based Fault Analysis ]

        private List<Fault> DetectFaultsByCurrent(VIDataGroup viDataGroup, VICycleDataGroup viCycleDataGroup)
        {
            List<Fault> iaFaults = DetectFaultsByCurrent(viDataGroup.IA, viCycleDataGroup.IA.RMS);
            List<Fault> ibFaults = DetectFaultsByCurrent(viDataGroup.IB, viCycleDataGroup.IB.RMS);
            List<Fault> icFaults = DetectFaultsByCurrent(viDataGroup.IC, viCycleDataGroup.IC.RMS);

            return Merge(iaFaults, ibFaults, icFaults);
        }

        private List<Fault> DetectFaultsByCurrent(DataSeries waveForm, DataSeries rms)
        {
            List<Fault> faults = new List<Fault>();
            Fault currentFault = null;

            bool[] faultApparent = rms.DataPoints
                .Select(dataPoint => dataPoint.Value - FaultLocationSettings.PrefaultTriggerAdjustment)
                .Select(value => value / rms[0].Value)
                .Select(ratio => ratio > FaultLocationSettings.PrefaultTrigger)
                .ToArray();

            for (int i = 0; i < rms.DataPoints.Count; i++)
            {
                if (faultApparent[i])
                {
                    if ((object)currentFault == null)
                    {
                        currentFault = new Fault();
                        currentFault.StartSample = FindFaultInception(waveForm, rms, i);
                        faults.Add(currentFault);
                    }
                }
                else
                {
                    if ((object)currentFault != null)
                    {
                        currentFault.EndSample = FindFaultClearing(waveForm, i);
                        currentFault = null;
                    }
                }
            }

            if ((object)currentFault != null)
                currentFault.EndSample = rms.DataPoints.Count - 1;

            faults.RemoveWhere(fault => fault.StartSample >= fault.EndSample);

            return faults;
        }

        private void ClassifyFaultsByCurrent(List<Fault> faults, DataGroup dataGroup, VICycleDataGroup viCycleDataGroup)
        {
            int minSample = 0;
            int maxSample = dataGroup.Samples - 1;
            int Snap(int sample) => Math.Max(Math.Min(sample, maxSample), minSample);
            DateTime GetTime(int sample) => dataGroup[0][Snap(sample)].Time;

            DataSeries iaRMS = viCycleDataGroup.IA.RMS;
            DataSeries ibRMS = viCycleDataGroup.IB.RMS;
            DataSeries icRMS = viCycleDataGroup.IC.RMS;
            DataSeries irRMS = viCycleDataGroup.IR.RMS;

            double iaPre = iaRMS[0].Value;
            double ibPre = ibRMS[0].Value;
            double icPre = icRMS[0].Value;

            int samplesPerCycle = Transform.CalculateSamplesPerCycle(dataGroup[0], DataAnalysisSettings.SystemFrequency);
            int samplesPerHalfCycle = samplesPerCycle / 2;

            foreach (Fault fault in faults)
            {
                int rmsStart = fault.StartSample;
                int rmsEnd = fault.EndSample - samplesPerCycle;

                FaultType GetFaultType(int sample)
                {
                    // Each RMS sample represents a full cycle of waveform data;
                    // in this case, we are interested in the cycle centered around
                    // the waveform data point so shift the index by half a cycle
                    int rmsIndex = sample - samplesPerHalfCycle;

                    if (rmsIndex < rmsStart)
                        rmsIndex = rmsStart;
                    else if (rmsIndex > rmsEnd)
                        rmsIndex = rmsEnd;

                    double ia = iaRMS[rmsIndex].Value;
                    double ib = ibRMS[rmsIndex].Value;
                    double ic = icRMS[rmsIndex].Value;
                    double ir = irRMS[rmsIndex].Value;

                    int numPhases = GetNumPhases(4.0D, ia, ib, ic);

                    if (numPhases == 3)
                        numPhases = GetNumPhases(1.5, ia - iaPre, ib - ibPre, ic - icPre);

                    return this.GetFaultType(numPhases, ia, ib, ic, ir);
                }

                Classify(fault, GetFaultType, GetTime);
            }
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

        private FaultType GetFaultType(int numPhases, double ia, double ib, double ic, double ir)
        {
            bool grounded =
                ir > FaultLocationSettings.PrefaultTriggerAdjustment &&
                ir > 0.1 * (ia + ib + ic);

            if (numPhases == 3)
                return grounded ? FaultType.ABCG : FaultType.ABC;

            if (numPhases == 2)
            {
                if (ic < ia && ic < ib)
                    return grounded ? FaultType.ABG : FaultType.AB;

                if (ia < ib && ia < ic)
                    return grounded ? FaultType.BCG : FaultType.BC;

                if (ib < ia && ib < ic)
                    return grounded ? FaultType.CAG : FaultType.CA;
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

        #endregion

        #region [ Voltage-Based Fault Analysis ]

        private List<Fault> DetectSinglePhaseFaultsByVoltage(VICycleDataGroup viCycleDataGroup, DataSeries irWaveform)
        {
            List<Fault> faults = new List<Fault>();
            Fault currentFault = null;

            DataSeries vaRMS = ToPerUnit(viCycleDataGroup.VA.RMS);
            DataSeries vbRMS = ToPerUnit(viCycleDataGroup.VB.RMS);
            DataSeries vcRMS = ToPerUnit(viCycleDataGroup.VC.RMS);

            double faultedPhaseThreshold = FaultLocationSettings.FaultedVoltageThreshold;

            List<bool> vaFaultedCycles = vaRMS.DataPoints
                .Select(point => point.Value < faultedPhaseThreshold)
                .ToList();

            List<bool> vbFaultedCycles = vbRMS.DataPoints
                .Select(point => point.Value < faultedPhaseThreshold)
                .ToList();

            List<bool> vcFaultedCycles = vcRMS.DataPoints
                .Select(point => point.Value < faultedPhaseThreshold)
                .ToList();

            bool[] faultApparent = Enumerable.Range(0, vaRMS.DataPoints.Count)
                .Select(index =>
                {
                    bool vaFault = vaFaultedCycles[index];
                    bool vbFault = vbFaultedCycles[index];
                    bool vcFault = vcFaultedCycles[index];

                    return
                        (vaFault && !vbFault && !vcFault) ||
                        (vbFault && !vcFault && !vaFault) ||
                        (vcFault && !vaFault && !vbFault);
                })
                .ToArray();

            for (int i = 0; i < vaRMS.DataPoints.Count; i++)
            {
                if (faultApparent[i])
                {
                    if ((object)currentFault == null)
                    {
                        currentFault = new Fault();
                        currentFault.StartSample = FindFaultInception(irWaveform, i);
                        faults.Add(currentFault);
                    }
                }
                else
                {
                    if ((object)currentFault != null)
                    {
                        currentFault.EndSample = FindFaultClearing(irWaveform, i);
                        currentFault = null;
                    }
                }
            }

            if ((object)currentFault != null)
                currentFault.EndSample = vaRMS.DataPoints.Count - 1;

            faults.RemoveWhere(fault => fault.StartSample >= fault.EndSample);

            return faults;
        }

        private void ClassifyFaultsByVoltage(List<Fault> faults, DataGroup dataGroup, VICycleDataGroup viCycleDataGroup)
        {
            int minSample = 0;
            int maxSample = dataGroup.Samples - 1;
            int Snap(int sample) => Math.Max(Math.Min(sample, maxSample), minSample);
            DateTime GetTime(int sample) => dataGroup[0][Snap(sample)].Time;

            DataSeries vaRMS = ToPerUnit(viCycleDataGroup.VA.RMS);
            DataSeries vbRMS = ToPerUnit(viCycleDataGroup.VB.RMS);
            DataSeries vcRMS = ToPerUnit(viCycleDataGroup.VC.RMS);
            double nominalLineVoltage = dataGroup.Asset.VoltageKV * 1000.0D / Math.Sqrt(3.0D);

            int samplesPerCycle = Transform.CalculateSamplesPerCycle(dataGroup[0], DataAnalysisSettings.SystemFrequency);
            int samplesPerHalfCycle = samplesPerCycle / 2;

            foreach (Fault fault in faults)
            {
                int rmsStart = fault.StartSample;
                int rmsEnd = fault.EndSample - samplesPerCycle;

                FaultType GetFaultType(int sample)
                {
                    // Each RMS sample represents a full cycle of waveform data;
                    // in this case, we are interested in the cycle centered around
                    // the waveform data point so shift the index by half a cycle
                    int rmsIndex = sample - samplesPerHalfCycle;

                    if (rmsIndex < rmsStart)
                        rmsIndex = rmsStart;
                    else if (rmsIndex > rmsEnd)
                        rmsIndex = rmsEnd;

                    double va = vaRMS[rmsIndex].Value;
                    double vb = vbRMS[rmsIndex].Value;
                    double vc = vcRMS[rmsIndex].Value;

                    CycleData cycleData = GetCycle(viCycleDataGroup, rmsIndex);
                    ComplexNumber[] sequenceComponents = CycleData.CalculateSequenceComponents(cycleData.AN.V, cycleData.BN.V, cycleData.CN.V);
                    double v0 = sequenceComponents[0].Magnitude / nominalLineVoltage;

                    return this.GetFaultType(va, vb, vc, v0);
                }

                Classify(fault, GetFaultType, GetTime);
            }
        }

        private FaultType GetFaultType(double va, double vb, double vc, double v0)
        {
            double faultedPhaseThreshold = FaultLocationSettings.FaultedVoltageThreshold;
            double groundFaultThreshold = FaultLocationSettings.GroundedFaultVoltageThreshold;

            bool vaFault = va < faultedPhaseThreshold;
            bool vbFault = vb < faultedPhaseThreshold;
            bool vcFault = vc < faultedPhaseThreshold;
            bool groundFault = v0 > groundFaultThreshold;

            if (vaFault && vbFault && vcFault)
                return groundFault ? FaultType.ABCG : FaultType.ABC;

            if (vaFault && vbFault)
                return groundFault ? FaultType.ABG : FaultType.AB;

            if (vbFault && vcFault)
                return groundFault ? FaultType.BCG : FaultType.BC;

            if (vcFault && vaFault)
                return groundFault ? FaultType.CAG : FaultType.CA;

            if (vaFault)
                return FaultType.AN;

            if (vbFault)
                return FaultType.BN;

            if (vcFault)
                return FaultType.CN;

            return FaultType.None;
        }

        private DataSeries ToPerUnit(DataSeries voltageRMS)
        {
            double? perUnitValue = voltageRMS.SeriesInfo.Channel.PerUnitValue;
            double lineVoltage = voltageRMS.SeriesInfo.Channel.Asset.VoltageKV * 1000.0D;
            string phase = voltageRMS.SeriesInfo.Channel.Phase.Name;

            if (new[] { "AN", "BN", "CN" }.Contains(phase))
                lineVoltage /= Math.Sqrt(3.0D);

            double mult = 1.0D / (perUnitValue ?? lineVoltage);
            DataSeries perUnitSeries = voltageRMS.Multiply(mult);
            perUnitSeries.SeriesInfo = voltageRMS.SeriesInfo;
            return perUnitSeries;
        }

        #endregion

        #endregion

        #region [ Static ]

        // Static Fields
        private static readonly ILog Log = LogManager.GetLogger(typeof(FaultDataResource));

        #endregion
    }
}
