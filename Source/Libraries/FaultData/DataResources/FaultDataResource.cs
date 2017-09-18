//******************************************************************************************************
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
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Reflection;
using FaultAlgorithms;
using FaultData.Configuration;
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
                using (AdoDataConnection connection = Meter.ConnectionFactory())
                {
                    int lineID = Line.ID;
                    TableOperations<LineImpedance> lineImpedanceTable = new TableOperations<LineImpedance>(connection);
                    LineImpedance lineImpedance = lineImpedanceTable.QueryRecordWhere("LineID = {0}", lineID);

                    if ((object)lineImpedance != null)
                    {
                        int localMeterLocationID = Meter.MeterLocationID;

                        List<int> linkIDs = Line.MeterLocationLines
                            .Select(link => link.ID)
                            .ToList();

                        int localLinkID = Line.MeterLocationLines
                            .Where(link => link.MeterLocationID == localMeterLocationID)
                            .Select(link => link.ID)
                            .FirstOrDefault();

                        TableOperations<SourceImpedance> sourceImpedanceTable = new TableOperations<SourceImpedance>(connection);

                        List<SourceImpedance> sourceImpedances = sourceImpedanceTable
                            .QueryRecordsWhere($"MeterLocationLineID IN ({string.Join(",", linkIDs)})")
                            .ToList();

                        SourceImpedance localImpedance = sourceImpedances
                            .FirstOrDefault(impedance => impedance.MeterLocationLineID == localLinkID);

                        List<SourceImpedance> remoteImpedances = sourceImpedances
                            .Where(impedance => impedance.MeterLocationLineID != localMeterLocationID)
                            .ToList();

                        if (lineImpedance.R0 == 0.0D && lineImpedance.X0 == 0.0D && lineImpedance.R1 == 0.0D && lineImpedance.X1 == 0.0D)
                            return false;

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
                    int endSample = fault.EndSample - SamplesPerCycle;

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
                            subGroup.PushDataTo(FaultLocationDataSet.Cycles);

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

        // Fields
        private double m_systemFrequency;
        private double m_maxVoltage;
        private double m_maxCurrent;
        private FaultLocationSettings m_faultLocationSettings;
        private BreakerSettings m_breakerSettings;

        private Dictionary<DataGroup, FaultGroup> m_faultLookup;

        #endregion

        #region [ Constructors ]

        public FaultDataResource()
        {
            m_faultLocationSettings = new FaultLocationSettings();
            m_breakerSettings = new BreakerSettings();
            m_faultLookup = new Dictionary<DataGroup, FaultGroup>();
        }

        #endregion

        #region [ Properties ]

        public Dictionary<DataGroup, FaultGroup> FaultLookup
        {
            get
            {
                return m_faultLookup;
            }
        }

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

        [Setting]
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

        [Setting]
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

        [Setting]
        public double SagThreshold { get; set;}

        [Category]
        [SettingName("FaultLocation")]
        public FaultLocationSettings FaultLocationSettings
        {
            get
            {
                return m_faultLocationSettings;
            }
        }

        [Category]
        [SettingName("Breakers")]
        public BreakerSettings BreakerSettings
        {
            get
            {
                return m_breakerSettings;
            }
        }

        #endregion

        #region [ Methods ]

        public override void Initialize(MeterDataSet meterDataSet)
        {
            DataGroup dataGroup;
            VIDataGroup viDataGroup;
            VICycleDataGroup viCycleDataGroup;

            List<Fault> faults;

            List<FaultLocationAlgorithm> faultLocationAlgorithms;
            FaultLocationDataSet faultLocationDataSet;
            ImpedanceExtractor impedanceExtractor;
            FaultCurveGenerator faultCurveGenerator;

            CycleDataResource cycleDataResource;

            bool? faultDetectionLogicResult;
            bool defaultFaultDetectionLogicResult;
            bool faultValidationLogicResult;

            Stopwatch stopwatch;

            stopwatch = new Stopwatch();
            cycleDataResource = meterDataSet.GetResource<CycleDataResource>();

            using (AdoDataConnection connection = meterDataSet.CreateDbConnection())
            {
                TableOperations<openXDA.Model.FaultLocationAlgorithm> faultLocationAlgorithmTable = new TableOperations<openXDA.Model.FaultLocationAlgorithm>(connection);
                faultLocationAlgorithms = GetFaultLocationAlgorithms(faultLocationAlgorithmTable);
            }

            Log.Info(string.Format("Executing fault location analysis on {0} events.", cycleDataResource.DataGroups.Count));

            for (int i = 0; i < cycleDataResource.DataGroups.Count; i++)
            {
                dataGroup = cycleDataResource.DataGroups[i];
                viDataGroup = cycleDataResource.VIDataGroups[i];
                viCycleDataGroup = cycleDataResource.VICycleDataGroups[i];

                // Defined channel checks
                Log.Debug("Checking defined channels...");

                if (viDataGroup.DefinedNeutralVoltages != 3)
                {
                    Log.Debug($"Not enough neutral voltage channels for fault analysis: {viDataGroup.DefinedNeutralVoltages}.");
                    continue;
                }

                if (viDataGroup.DefinedCurrents < 3)
                {
                    Log.Debug($"Not enough current channels for fault analysis: {viDataGroup.DefinedNeutralVoltages}.");
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
                faultLocationDataSet = new FaultLocationDataSet();
                faultLocationDataSet.LineDistance = dataGroup.Line.Length;
                faultLocationDataSet.PrefaultCycle = FirstCycle(viCycleDataGroup);

                // Extract impedances from the database
                // and into the fault location data set
                impedanceExtractor = new ImpedanceExtractor();
                impedanceExtractor.FaultLocationDataSet = faultLocationDataSet;
                impedanceExtractor.Meter = meterDataSet.Meter;
                impedanceExtractor.Line = dataGroup.Line;

                if (!impedanceExtractor.TryExtractImpedances())
                {
                    Log.Debug("No impedances defined; skipping fault analysis.");
                    continue;
                }

                // Break into faults and segments
                Log.Debug("Classifying data into faults and segments...");

                try
                {
                    stopwatch.Restart();

                    faults = DetectFaults(viDataGroup, viCycleDataGroup);

                    if (faults.Count > 0)
                        ClassifyFaults(faults, dataGroup, viCycleDataGroup);
                }
                finally
                {
                    Log.Debug(stopwatch.Elapsed);
                }

                // Check the fault detection logic and the default fault detection logic
                faultDetectionLogicResult = CheckFaultDetectionLogic(meterDataSet, dataGroup);
                defaultFaultDetectionLogicResult = CheckDefaultFaultDetectionLogic(faults);

                // If the fault detection logic detects a fault and the default
                // logic does not agree, treat the whole waveform as a fault
                if (faultDetectionLogicResult == true && !defaultFaultDetectionLogicResult)
                {
                    faults.Add(new Fault()
                    {
                        StartSample = 0,
                        EndSample = dataGroup[0].DataPoints.Count - 1
                    });

                    ClassifyFaults(faults, dataGroup, viCycleDataGroup);
                }

                // Generate fault curves for fault analysis
                Log.Debug("Generating fault curves...");
                stopwatch.Restart();

                faultCurveGenerator = new FaultCurveGenerator();
                faultCurveGenerator.SamplesPerCycle = Transform.CalculateSamplesPerCycle(viDataGroup.VA, m_systemFrequency);
                faultCurveGenerator.CycleDataGroup = viCycleDataGroup;
                faultCurveGenerator.Faults = faults;
                faultCurveGenerator.FaultLocationDataSet = faultLocationDataSet;
                faultCurveGenerator.FaultLocationAlgorithms = faultLocationAlgorithms;
                faultCurveGenerator.GenerateFaultCurves();

                Log.Debug(stopwatch.Elapsed);

                // Gather additional info about each fault
                // based on the results of the above analysis
                foreach (Fault fault in faults)
                    PopulateFaultInfo(fault, dataGroup, viCycleDataGroup, viDataGroup);

                // Create a fault group and add it to the lookup table
                faultValidationLogicResult = CheckFaultValidationLogic(faults);
                m_faultLookup.Add(dataGroup, new FaultGroup(faults, faultDetectionLogicResult, defaultFaultDetectionLogicResult, faultValidationLogicResult));
            }
        }

        private bool? CheckFaultDetectionLogic(MeterDataSet meterDataSet, DataGroup dataGroup)
        {
            string expressionText;
            int meterLineID;

            expressionText = null;

            // Find MeterLine record corresponding to the meter that produced
            // the data and the line associated with the data group
            meterLineID = meterDataSet.Meter.MeterLines
                .Where(meterLine => meterLine.LineID == dataGroup.Line.ID)
                .Select(meterLine => (int?)meterLine.ID)
                .FirstOrDefault() ?? -1;

            if (meterLineID > 0)
            {
                using (AdoDataConnection connection = meterDataSet.CreateDbConnection())
                {
                    TableOperations<FaultDetectionLogic> faultDetectionLogicTable = new TableOperations<FaultDetectionLogic>(connection);

                    // Find fault detection logic defined for the meter and line
                    expressionText = faultDetectionLogicTable
                        .QueryRecordsWhere("MeterLineID = {0}", meterLineID)
                        .Select(logic => logic.Expression)
                        .FirstOrDefault();
                }
            }

            try
            {
                if ((object)expressionText == null)
                {
                    if (m_faultLocationSettings.WarnMissingDetectionLogic)
                        throw new Exception($"Expression text is not defined for line '{dataGroup.Line.AssetKey}'.");

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

        private List<Fault> DetectFaults(VIDataGroup viDataGroup, VICycleDataGroup viCycleDataGroup)
        {
            List<Fault> iaFaults = DetectFaults(viDataGroup.IA, viCycleDataGroup.IA.RMS);
            List<Fault> ibFaults = DetectFaults(viDataGroup.IB, viCycleDataGroup.IB.RMS);
            List<Fault> icFaults = DetectFaults(viDataGroup.IC, viCycleDataGroup.IC.RMS);

            return Merge(iaFaults, ibFaults, icFaults);
        }

        private List<Fault> DetectFaults(DataSeries waveForm, DataSeries rms)
        {
            List<Fault> faults = new List<Fault>();
            Fault currentFault = null;

            bool[] faultApparent = rms.DataPoints
                .Select(dataPoint => dataPoint.Value - m_faultLocationSettings.PrefaultTriggerAdjustment)
                .Select(value => value / rms[0].Value)
                .Select(ratio => ratio > m_faultLocationSettings.PrefaultTrigger)
                .ToArray();

            for (int i = 0; i < rms.DataPoints.Count; i++)
            {
                if (faultApparent[i])
                {
                    if ((object)currentFault == null)
                    {
                        currentFault = new Fault();
                        currentFault.StartSample = FindFaultInception(waveForm, i);
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
            double nominalVoltage = dataGroup.Line.VoltageKV * 1000.0D / Math.Sqrt(3.0D);

            DataSeries[] voltages =
            {
                viCycleDataGroup.VA.RMS.Multiply(1.0D / nominalVoltage),
                viCycleDataGroup.VB.RMS.Multiply(1.0D / nominalVoltage),
                viCycleDataGroup.VC.RMS.Multiply(1.0D / nominalVoltage)
            };

            DataSeries[] currents =
            {
                viCycleDataGroup.IA.RMS,
                viCycleDataGroup.IB.RMS,
                viCycleDataGroup.IC.RMS,
                viCycleDataGroup.IR.RMS
            };

            // Determine if any of the RMS voltages are unreasonably high
            if (voltages.Any(voltage => voltage.DataPoints.Any(dataPoint => dataPoint.Value > m_maxVoltage)))
            {
                Log.Debug("Data unreasonable: voltage > maxVoltage");
                return false;
            }

            // Determine if any of the RMS currents are unreasonably low
            if (currents.Any(voltage => voltage.DataPoints.Any(dataPoint => dataPoint.Value > m_maxCurrent)))
            {
                Log.Debug("Data unreasonable: current > maxCurrent");
                return false;
            }

            return true;
        }

        private int FindFaultInception(DataSeries waveForm, int cycleIndex)
        {
            int samplesPerCycle = Transform.CalculateSamplesPerCycle(waveForm, m_systemFrequency);
            int prefaultIndex = Math.Max(0, cycleIndex - samplesPerCycle);
            int endIndex = cycleIndex + samplesPerCycle - 1;

            double largestPrefaultPeak;
            double largestFaultCyclePeak;

            double previousValue;
            double value;
            double nextValue;

            largestPrefaultPeak = 0.0D;
            largestFaultCyclePeak = 0.0D;

            // Find the largest prefault peak as the absolute
            // peak of the cycle before the first faulted cycle
            for (int i = prefaultIndex; i < cycleIndex; i++)
            {
                value = Math.Abs(waveForm[i].Value);

                if (value > largestPrefaultPeak)
                    largestPrefaultPeak = value;
            }

            // Find the largest peak of the first faulted cycle
            for (int i = cycleIndex; i <= endIndex; i++)
            {
                value = Math.Abs(waveForm[i].Value);

                if (value > largestFaultCyclePeak)
                    largestFaultCyclePeak = value;
            }

            // Find the first point where the value exceeds a point 25%
            // of the way from the prefault peak to the fault peak
            for (int i = cycleIndex; i < endIndex; i++)
            {
                value = Math.Abs(waveForm[i].Value);

                if (value >= (largestPrefaultPeak * 0.75 + largestFaultCyclePeak * 0.25))
                    endIndex = i;
            }

            // Starting from the point found in the previous loop and
            // scanning backwards, find either the first zero crossing
            // or the first point at which the slope changes drastically
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
            int samplesPerCycle = Transform.CalculateSamplesPerCycle(waveForm, m_systemFrequency);
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

        private void ClassifyFaults(List<Fault> faults, DataGroup dataGroup, VICycleDataGroup viCycleDataGroup)
        {
            Fault.Segment currentSegment = null;

            DataSeries iaRMS = viCycleDataGroup.IA.RMS;
            DataSeries ibRMS = viCycleDataGroup.IB.RMS;
            DataSeries icRMS = viCycleDataGroup.IC.RMS;

            double iaPre = iaRMS[0].Value;
            double ibPre = ibRMS[0].Value;
            double icPre = icRMS[0].Value;

            double ia;
            double ib;
            double ic;

            int numPhases;
            FaultType faultType;

            foreach (Fault fault in faults)
            {
                for (int i = fault.StartSample; i <= fault.EndSample && i < iaRMS.DataPoints.Count; i++)
                {
                    ia = iaRMS[i].Value;
                    ib = ibRMS[i].Value;
                    ic = icRMS[i].Value;

                    numPhases = GetNumPhases(4.0D, ia, ib, ic);

                    if (numPhases == 3)
                        numPhases = GetNumPhases(1.5, ia - iaPre, ib - ibPre, ic - icPre);

                    faultType = GetFaultType(numPhases, ia, ib, ic);

                    if ((object)currentSegment == null)
                    {
                        currentSegment = new Fault.Segment(faultType);
                        currentSegment.StartTime = viCycleDataGroup.IA.RMS[i].Time;
                        currentSegment.StartSample = i;
                        fault.Segments.Add(currentSegment);
                    }
                    else if (currentSegment.FaultType != faultType)
                    {
                        currentSegment.EndTime = viCycleDataGroup.IA.RMS[i - 1].Time;
                        currentSegment.EndSample = i - 1;

                        currentSegment = new Fault.Segment(faultType);
                        currentSegment.StartTime = viCycleDataGroup.IA.RMS[i].Time;
                        currentSegment.StartSample = i;
                        fault.Segments.Add(currentSegment);
                    }
                }

                if ((object)currentSegment != null)
                {
                    currentSegment.EndTime = dataGroup[0][fault.EndSample].Time;
                    currentSegment.EndSample = fault.EndSample;
                    currentSegment = null;
                }
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

        private CycleData FirstCycle(VICycleDataGroup viCycleDataGroup)
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
                cycles[i].RMS = cycleDataGroups[i].RMS.DataPoints[0].Value;
                cycles[i].Phase = cycleDataGroups[i].Phase.DataPoints[0].Value;
                cycles[i].Peak = cycleDataGroups[i].Peak.DataPoints[0].Value;
                cycles[i].Error = cycleDataGroups[i].Error.DataPoints[0].Value;
            }

            return cycle;
        }

        private void PopulateFaultInfo(Fault fault, DataGroup dataGroup, VICycleDataGroup viCycleDataGroup, VIDataGroup viDataGroup)
        {
            int samplesPerCycle = Transform.CalculateSamplesPerCycle(dataGroup.SamplesPerSecond, m_systemFrequency);
            int calculationCycle = GetCalculationCycle(fault, viCycleDataGroup, samplesPerCycle);
            DateTime startTime = dataGroup[0][fault.StartSample].Time;
            DateTime endTime = dataGroup[0][fault.EndSample].Time;

            List<Fault.Summary> validSummaries;
            Fault.Summary summary;

            fault.CalculationCycle = calculationCycle;
            fault.InceptionTime = startTime;
            fault.ClearingTime = endTime;
            fault.Duration = endTime - startTime;
            fault.PrefaultCurrent = GetPrefaultCurrent(fault, dataGroup, viCycleDataGroup);
            fault.PostfaultCurrent = GetPostfaultCurrent(fault, dataGroup, viCycleDataGroup);
            if (fault.Segments.Any())
            {
                fault.Type = fault.Segments
                    .Where(segment => segment.StartSample <= fault.CalculationCycle)
                    .Where(segment => fault.CalculationCycle <= segment.EndSample)
                    .Select(segment => segment.FaultType)
                    .FirstOrDefault();

                fault.CurrentMagnitude = GetFaultCurrentMagnitude(viCycleDataGroup, fault.Type, calculationCycle);
                fault.CurrentLag = GetFaultCurrentLag(viCycleDataGroup, fault.Type, calculationCycle);
            }

            bool mostly60 = CheckThreePhaseFFT(fault, viDataGroup);
            bool threePhaseWithNoSag = (Common.Min(viCycleDataGroup.VA.ToSubGroup(fault.StartSample, fault.EndSample).RMS.Minimum, viCycleDataGroup.VB.ToSubGroup(fault.StartSample, fault.EndSample).RMS.Minimum, viCycleDataGroup.VC.ToSubGroup(fault.StartSample, fault.EndSample).RMS.Minimum) / 1000 / (dataGroup.Line.VoltageKV / Math.Sqrt(3)) > SagThreshold);

            fault.IsSuppressed = !mostly60 || threePhaseWithNoSag  || GetPostfaultPeak(fault, dataGroup, viCycleDataGroup) > m_breakerSettings.OpenBreakerThreshold;



            if (calculationCycle >= 0)
            {
                for (int i = 0; i < fault.Curves.Count; i++)
                {
                    summary = new Fault.Summary();
                    summary.DistanceAlgorithmIndex = i;
                    summary.DistanceAlgorithm = fault.Curves[i].Algorithm;
                    summary.Distance = fault.Curves[i][calculationCycle].Value;
                    summary.IsValid = IsValid(summary.Distance, dataGroup);

                    fault.Summaries.Add(summary);
                }

                if (fault.Summaries.Any(s => !s.IsValid))
                    fault.IsSuppressed |= fault.CurrentLag < 0;

                validSummaries = fault.Summaries
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
            double lineLength = dataGroup.Line.Length;
            double maxDistance = m_faultLocationSettings.MaxFaultDistanceMultiplier * lineLength;
            double minDistance = m_faultLocationSettings.MinFaultDistanceMultiplier * lineLength;
            return faultDistance >= minDistance && faultDistance <= maxDistance;
        }

        private double GetPrefaultCurrent(Fault fault, DataGroup dataGroup, VICycleDataGroup viCycleDataGroup)
        {
            int samplesPerCycle = Transform.CalculateSamplesPerCycle(dataGroup.SamplesPerSecond, m_systemFrequency);
            int start = Math.Max(0, fault.StartSample - samplesPerCycle);
            int end = fault.StartSample;

            double ia = viCycleDataGroup.IA.RMS.ToSubSeries(start, end).Minimum;
            double ib = viCycleDataGroup.IB.RMS.ToSubSeries(start, end).Minimum;
            double ic = viCycleDataGroup.IC.RMS.ToSubSeries(start, end).Minimum;

            return Common.Min(ia, ib, ic);
        }

        private double GetPostfaultCurrent(Fault fault, DataGroup dataGroup, VICycleDataGroup viCycleDataGroup)
        {
            int samplesPerCycle = Transform.CalculateSamplesPerCycle(dataGroup.SamplesPerSecond, m_systemFrequency);
            int start = fault.EndSample + 1;
            int end = Math.Min(start + samplesPerCycle, viCycleDataGroup.IA.RMS.DataPoints.Count) - 1;

            double ia = viCycleDataGroup.IA.RMS.ToSubSeries(start, end).Minimum;
            double ib = viCycleDataGroup.IB.RMS.ToSubSeries(start, end).Minimum;
            double ic = viCycleDataGroup.IC.RMS.ToSubSeries(start, end).Minimum;

            return Common.Min(ia, ib, ic);
        }

        private double GetPostfaultPeak(Fault fault, DataGroup dataGroup, VICycleDataGroup viCycleDataGroup)
        {
            int samplesPerCycle = Transform.CalculateSamplesPerCycle(dataGroup.SamplesPerSecond, m_systemFrequency);
            int start = fault.EndSample + 1;
            int end = Math.Min(start + 5 * samplesPerCycle, viCycleDataGroup.IA.RMS.DataPoints.Count) - 1;

            double ia = viCycleDataGroup.IA.Peak.ToSubSeries(start, end).Minimum;
            double ib = viCycleDataGroup.IB.Peak.ToSubSeries(start, end).Minimum;
            double ic = viCycleDataGroup.IC.Peak.ToSubSeries(start, end).Minimum;

            return Common.Min(ia, ib, ic);
        }

        private int GetCalculationCycle(Fault fault, VICycleDataGroup viCycleDataGroup, int samplesPerCycle)
        {
            double ia;
            double ib;
            double ic;
            double sum;

            double max;
            int maxIndex;

            int startSample;
            int endSample;

            if (!fault.Curves.Any())
                return -1;

            startSample = fault.StartSample + samplesPerCycle;
            endSample = fault.StartSample + fault.Curves.Min(curve => curve.Series.DataPoints.Count) - 1;

            if (startSample > endSample)
                startSample = fault.StartSample;

            if (startSample > endSample)
                return -1;

            max = double.MinValue;
            maxIndex = -1;

            for (int i = startSample; i <= endSample; i++)
            {
                ia = viCycleDataGroup.IA.RMS[i].Value;
                ib = viCycleDataGroup.IB.RMS[i].Value;
                ic = viCycleDataGroup.IC.RMS[i].Value;
                sum = ia + ib + ic;

                if (sum > max)
                {
                    maxIndex = i;
                    max = sum;
                }
            }

            return maxIndex;
        }

        private double GetFaultCurrentMagnitude(VICycleDataGroup viCycleDataGroup, FaultType faultType, int cycle)
        {
            FaultType viFaultType;

            double anError;
            double bnError;
            double cnError;

            ComplexNumber an;
            ComplexNumber bn;
            ComplexNumber cn;

            viFaultType = faultType;

            if (viFaultType == FaultType.ABC)
            {
                anError = viCycleDataGroup.IA.Error[cycle].Value;
                bnError = viCycleDataGroup.IB.Error[cycle].Value;
                cnError = viCycleDataGroup.IC.Error[cycle].Value;

                if (anError < bnError && anError < cnError)
                    viFaultType = FaultType.AN;
                else if (bnError < anError && bnError < cnError)
                    viFaultType = FaultType.BN;
                else
                    viFaultType = FaultType.CN;
            }

            switch (viFaultType)
            {
                case FaultType.AN:
                    return viCycleDataGroup.IA.RMS[cycle].Value;

                case FaultType.BN:
                    return viCycleDataGroup.IB.RMS[cycle].Value;

                case FaultType.CN:
                    return viCycleDataGroup.IC.RMS[cycle].Value;

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
            FaultType viFaultType;

            double anError;
            double bnError;
            double cnError;

            ComplexNumber van;
            ComplexNumber vbn;
            ComplexNumber vcn;

            ComplexNumber ian;
            ComplexNumber ibn;
            ComplexNumber icn;

            viFaultType = faultType;

            if (viFaultType == FaultType.ABC)
            {
                anError = viCycleDataGroup.IA.Error[cycle].Value;
                bnError = viCycleDataGroup.IB.Error[cycle].Value;
                cnError = viCycleDataGroup.IC.Error[cycle].Value;

                if (anError < bnError && anError < cnError)
                    viFaultType = FaultType.AN;
                else if (bnError < anError && bnError < cnError)
                    viFaultType = FaultType.BN;
                else
                    viFaultType = FaultType.CN;
            }

            switch (viFaultType)
            {
                case FaultType.AN:
                    return new Angle(viCycleDataGroup.VA.Phase[cycle].Value - viCycleDataGroup.IA.Phase[cycle].Value).ToRange(-Math.PI, false);

                case FaultType.BN:
                    return new Angle(viCycleDataGroup.VB.Phase[cycle].Value - viCycleDataGroup.IB.Phase[cycle].Value).ToRange(-Math.PI, false);

                case FaultType.CN:
                    return new Angle(viCycleDataGroup.VC.Phase[cycle].Value - viCycleDataGroup.IC.Phase[cycle].Value).ToRange(-Math.PI, false);

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

        private ComplexNumber ToComplexNumber(CycleDataGroup cycleDataGroup, int cycle)
        {
            Angle angle = cycleDataGroup.Phase[cycle].Value;
            double magnitude = cycleDataGroup.RMS[cycle].Value;
            return new ComplexNumber(angle, magnitude);
        }

        private bool CheckThreePhaseFFT(Fault fault, VIDataGroup viDataGroup)
        {
            bool returnValue = true;
            double samplesPerCycle = Math.Round(viDataGroup.IA.SampleRate / m_systemFrequency);
            int cycles = (int)Math.Ceiling(viDataGroup.IA.DataPoints.Count / samplesPerCycle);
            int arraySize = cycles*(int)samplesPerCycle;

            Complex[] iAsresult = FFT(viDataGroup.IA.DataPoints.Select(x => x.Value).Concat(Enumerable.Repeat(0.0,arraySize-viDataGroup.IA.DataPoints.Count)).ToArray());
            Complex[] iBsresult = FFT(viDataGroup.IB.DataPoints.Select(x => x.Value).Concat(Enumerable.Repeat(0.0, arraySize - viDataGroup.IA.DataPoints.Count)).ToArray());
            Complex[] iCsresult = FFT(viDataGroup.IC.DataPoints.Select(x => x.Value).Concat(Enumerable.Repeat(0.0, arraySize - viDataGroup.IA.DataPoints.Count)).ToArray());
            double ratioIa = iAsresult[cycles].Magnitude / iAsresult.Take(arraySize / 2).Sum(x => x.Magnitude);
            double ratioIb = iBsresult[cycles].Magnitude / iBsresult.Take(arraySize / 2).Sum(x => x.Magnitude);
            double ratioIc = iCsresult[cycles].Magnitude / iCsresult.Take(arraySize / 2).Sum(x => x.Magnitude);
            double ratio = ratioIa + ratioIb + ratioIc;
            if (ratio < 0.03)
                returnValue = false;

            return returnValue;
        }

        static Complex[] FFT(double[] samples)
        {
            Complex[] complexSamples = samples
                .Select(sample => new Complex(sample, 0))
                .ToArray();

            Fourier.BluesteinForward(complexSamples, FourierOptions.Default);

            return complexSamples;
        }

        #endregion

        #region [ Static ]

        // Static Fields
        private static readonly ILog Log = LogManager.GetLogger(typeof(FaultDataResource));

        #endregion
    }
}
