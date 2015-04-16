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
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using FaultAlgorithms;
using FaultData.DataAnalysis;
using FaultData.Database;
using FaultData.DataSets;
using GSF;
using GSF.Units;
using log4net;
using FaultLocationAlgorithm = FaultAlgorithms.FaultLocationAlgorithm;
using Line = FaultData.Database.Line;

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
                                fault.Curves[i].StartIndex = segment.StartSample;
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

        private double m_openBreakerThreshold;

        private Dictionary<DataGroup, List<Fault>> m_faultLookup;

        #endregion

        #region [ Constructors ]

        public FaultDataResource(DbAdapterContainer dbAdapterContainer)
        {
            m_dbAdapterContainer = dbAdapterContainer;
            m_faultLookup = new Dictionary<DataGroup, List<Fault>>();
        }

        #endregion

        #region [ Properties ]

        public Dictionary<DataGroup, List<Fault>> FaultLookup
        {
            get
            {
                return m_faultLookup;
            }
        }

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

        public double OpenBreakerThreshold
        {
            get
            {
                return m_openBreakerThreshold;
            }
            set
            {
                m_openBreakerThreshold = value;
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

            Stopwatch stopwatch;

            stopwatch = new Stopwatch();
            cycleDataResource = meterDataSet.GetResource<CycleDataResource>();
            faultLocationAlgorithms = GetFaultLocationAlgorithms(m_dbAdapterContainer.FaultLocationInfoAdapter);

            Log.Info(string.Format("Executing fault location analysis on {0} events.", cycleDataResource.DataGroups.Count));

            for (int i = 0; i < cycleDataResource.DataGroups.Count; i++)
            {
                dataGroup = cycleDataResource.DataGroups[i];
                viDataGroup = cycleDataResource.VIDataGroups[i];
                viCycleDataGroup = cycleDataResource.VICycleDataGroups[i];

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

                // Break into faults and segments
                Log.Debug("Classifying data into faults and segments...");

                try
                {
                    stopwatch.Restart();

                    faults = DetectFaults(dataGroup, viDataGroup, viCycleDataGroup);

                    if (faults.Count == 0)
                        continue;

                    ClassifyFaults(faults, viCycleDataGroup);
                }
                finally
                {
                    Log.Debug(stopwatch.Elapsed);
                }

                // Add list of faults to lookup table
                m_faultLookup.Add(dataGroup, faults);

                // Create the fault location data set and begin populating
                // the properties necessary for calculating fault location
                faultLocationDataSet = new FaultLocationDataSet();
                faultLocationDataSet.LineDistance = dataGroup.Line.Length;
                faultLocationDataSet.PrefaultCycle = FirstCycle(viCycleDataGroup);

                // Extract impedances from the database
                // and into the fault location data set
                impedanceExtractor = new ImpedanceExtractor();
                impedanceExtractor.FaultLocationDataSet = faultLocationDataSet;
                impedanceExtractor.FaultLocationInfo = m_dbAdapterContainer.FaultLocationInfoAdapter;
                impedanceExtractor.Meter = meterDataSet.Meter;
                impedanceExtractor.Line = dataGroup.Line;

                if (!impedanceExtractor.TryExtractImpedances())
                    continue;

                // Generate fault curves for fault analysis
                Log.Debug("Generating fault curves...");
                stopwatch.Restart();

                faultCurveGenerator = new FaultCurveGenerator();
                faultCurveGenerator.SamplesPerCycle = (int)(viDataGroup.VA.SampleRate / Frequency);
                faultCurveGenerator.CycleDataGroup = viCycleDataGroup;
                faultCurveGenerator.Faults = faults;
                faultCurveGenerator.FaultLocationDataSet = faultLocationDataSet;
                faultCurveGenerator.FaultLocationAlgorithms = faultLocationAlgorithms;
                faultCurveGenerator.GenerateFaultCurves();

                Log.Debug(stopwatch.Elapsed);

                // Gather additional info about each fault
                // based on the results of the above analysis
                foreach (Fault fault in faults)
                    PopulateFaultInfo(fault, dataGroup, viCycleDataGroup);
            }
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

        private List<Fault> DetectFaults(DataGroup dataGroup, VIDataGroup viDataGroup, VICycleDataGroup viCycleDataGroup)
        {
            List<Fault> faults = new List<Fault>();
            Fault currentFault = null;

            double thermalRating = dataGroup.Line.ThermalRating;
            double inverseThermalRating = 1.0D / thermalRating;

            DataSeries iaRMS = viCycleDataGroup.IA.RMS.Multiply(inverseThermalRating);
            DataSeries ibRMS = viCycleDataGroup.IB.RMS.Multiply(inverseThermalRating);
            DataSeries icRMS = viCycleDataGroup.IC.RMS.Multiply(inverseThermalRating);
            DataSeries irRMS = viCycleDataGroup.IR.RMS.Multiply(inverseThermalRating);

            double ia;
            double ib;
            double ic;
            double ir;

            int iaIndex;
            int ibIndex;
            int icIndex;

            bool obvious;
            bool apparent;
            bool suppressed;

            for (int i = 0; i < iaRMS.DataPoints.Count; i++)
            {
                ia = iaRMS.DataPoints[i].Value;
                ib = ibRMS.DataPoints[i].Value;
                ic = icRMS.DataPoints[i].Value;
                ir = irRMS.DataPoints[i].Value;

                obvious = IsFaultObvious(ia, ib, ic, ir);
                apparent = !obvious && IsFaultApparent(viCycleDataGroup, i);
                suppressed = apparent && IsFaultSuppressed(ia, ib, ic);

                if (obvious || (apparent && !suppressed))
                {
                    if ((object)currentFault == null)
                    {
                        iaIndex = FindFaultInception(viDataGroup.IA, i);
                        ibIndex = FindFaultInception(viDataGroup.IB, i);
                        icIndex = FindFaultInception(viDataGroup.IC, i);

                        currentFault = new Fault();
                        currentFault.StartSample = Common.Min(iaIndex, ibIndex, icIndex);
                        faults.Add(currentFault);
                    }
                }
                else
                {
                    if ((object)currentFault != null)
                    {
                        iaIndex = FindFaultClearing(viDataGroup.IA, i);
                        ibIndex = FindFaultClearing(viDataGroup.IB, i);
                        icIndex = FindFaultClearing(viDataGroup.IC, i);

                        currentFault.EndSample = Common.Max(iaIndex, ibIndex, icIndex) - 1;
                        currentFault = null;
                    }
                }
            }

            if ((object)currentFault != null)
                currentFault.EndSample = iaRMS.DataPoints.Count - 1;

            return faults;
        }

        private bool IsReasonable(DataGroup dataGroup, VICycleDataGroup viCycleDataGroup)
        {
            // Get the current system time and time thresholds
            DateTime now = DateTime.Now;
            DateTime minTime = now.AddHours(-m_minTimeOffset);
            DateTime maxTime = now.AddHours(m_maxTimeOffset);

            // Get the line-to-neutral nominal voltage in volts
            double nominalVoltage = dataGroup.Line.VoltageKV * 1000.0D / Math.Sqrt(3.0D);

            // Get the thermal rating in amps
            double thermalRating = dataGroup.Line.ThermalRating;

            DataSeries[] voltages =
            {
                viCycleDataGroup.VA.RMS.Multiply(1.0D / nominalVoltage),
                viCycleDataGroup.VB.RMS.Multiply(1.0D / nominalVoltage),
                viCycleDataGroup.VC.RMS.Multiply(1.0D / nominalVoltage)
            };

            DataSeries[] currents =
            {
                viCycleDataGroup.IA.RMS.Multiply(1.0D / thermalRating),
                viCycleDataGroup.IB.RMS.Multiply(1.0D / thermalRating),
                viCycleDataGroup.IC.RMS.Multiply(1.0D / thermalRating),
                viCycleDataGroup.IR.RMS.Multiply(1.0D / thermalRating)
            };

            // Determine if the time of the record is
            // too far in the past to be reasonable
            if (dataGroup.StartTime < minTime)
            {
                Log.Debug("Data unreasonable: dataGroup.StartTime < minTime");
                return false;
            }

            // Determine if the time of the record is
            // too far in the future to be reasonable
            if (dataGroup.StartTime > maxTime)
            {
                Log.Debug("Data unreasonable: dataGroup.StartTime > maxTime");
                return false;
            }

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

            // Determine if any of the cycles suggest that the
            // voltage was too low to be able to serve the current
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < voltages[i].DataPoints.Count; j++)
                {
                    if (voltages[i][j].Value < m_lowVoltageThreshold && currents[i][j].Value > m_maxLowVoltageCurrent)
                    {
                        Log.Debug("Data unreasonable: voltage too low to serve current");
                        return false;
                    }
                }
            }

            return true;
        }

        private bool IsFaultObvious(double ia, double ib, double ic, double ir)
        {
            if (ir > m_residualCurrentTrigger)
                return true;

            if (ia > m_phaseCurrentTrigger)
                return true;

            if (ib > m_phaseCurrentTrigger)
                return true;

            if (ic > m_phaseCurrentTrigger)
                return true;

            return false;
        }

        private bool IsFaultApparent(VICycleDataGroup viCycleDataGroup, int index)
        {
            double iaPre = viCycleDataGroup.IA.RMS[0].Value;
            double ibPre = viCycleDataGroup.IB.RMS[0].Value;
            double icPre = viCycleDataGroup.IC.RMS[0].Value;

            double ia = viCycleDataGroup.IA.RMS[index].Value;
            double ib = viCycleDataGroup.IB.RMS[index].Value;
            double ic = viCycleDataGroup.IC.RMS[index].Value;

            if ((ia / iaPre) > m_prefaultTrigger)
                return true;

            if ((ib / ibPre) > m_prefaultTrigger)
                return true;

            if ((ic / icPre) > m_prefaultTrigger)
                return true;

            return false;
        }

        private bool IsFaultSuppressed(double ia, double ib, double ic)
        {
            if (ia > m_faultSuppressionTrigger)
                return false;

            if (ib > m_faultSuppressionTrigger)
                return false;

            if (ic > m_faultSuppressionTrigger)
                return false;

            return true;
        }

        private int FindFaultInception(DataSeries waveForm, int cycleIndex)
        {
            int samplesPerCycle = (int)Math.Round(waveForm.SampleRate / Frequency);
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
            int samplesPerCycle = (int)Math.Round(waveForm.SampleRate / Frequency);
            int startIndex = cycleIndex;
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

        private void ClassifyFaults(List<Fault> faults, VICycleDataGroup viCycleDataGroup)
        {
            Fault.Segment currentSegment = null;

            DataSeries iaRMS = viCycleDataGroup.IA.RMS;
            DataSeries ibRMS = viCycleDataGroup.IB.RMS;
            DataSeries icRMS = viCycleDataGroup.IC.RMS;

            double iaPre = iaRMS.DataPoints[0].Value;
            double ibPre = ibRMS.DataPoints[0].Value;
            double icPre = icRMS.DataPoints[0].Value;

            double ia;
            double ib;
            double ic;

            int numPhases;
            FaultType faultType;

            foreach (Fault fault in faults)
            {
                for (int i = fault.StartSample; i <= fault.EndSample; i++)
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
                    currentSegment.EndTime = viCycleDataGroup.IA.RMS[fault.EndSample].Time;
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

        private void PopulateFaultInfo(Fault fault, DataGroup dataGroup, VICycleDataGroup viCycleDataGroup)
        {
            int samplesPerCycle = (int)Math.Round(dataGroup.DataSeries[0].SampleRate / Frequency);
            int calculationCycle = GetCalculationCycle(fault, viCycleDataGroup, samplesPerCycle);
            DateTime startTime = viCycleDataGroup.IA.RMS[fault.StartSample].Time;
            DateTime endTime = viCycleDataGroup.IA.RMS[fault.EndSample].Time;

            List<Fault.Summary> validSummaries;
            Fault.Summary summary;

            fault.CalculationCycle = calculationCycle;
            fault.InceptionTime = startTime;
            fault.ClearingTime = endTime;
            fault.Duration = endTime - startTime;
            fault.IsSuppressed = !IsBreakerOpen(fault, dataGroup, viCycleDataGroup);

            if (fault.Segments.Any())
            {
                fault.Type = fault.Segments
                    .Where(segment => segment.StartSample <= fault.CalculationCycle)
                    .Where(segment => fault.CalculationCycle <= segment.EndSample)
                    .Select(segment => segment.FaultType)
                    .FirstOrDefault();

                fault.CurrentMagnitude = GetFaultCurrentMagnitude(viCycleDataGroup, fault.Type, calculationCycle);
            }

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

                validSummaries = fault.Summaries
                    .Where(s => s.IsValid)
                    .OrderBy(s => s.Distance)
                    .ToList();

                if (validSummaries.Any())
                    validSummaries[validSummaries.Count / 2].IsSelectedAlgorithm = true;
            }
        }

        private bool IsValid(double faultDistance, DataGroup dataGroup)
        {
            double lineLength = dataGroup.Line.Length;
            double maxDistance = m_maxFaultDistanceMultiplier * lineLength;
            double minDistance = m_minFaultDistanceMultiplier * lineLength;
            return faultDistance >= minDistance && faultDistance <= maxDistance;
        }

        private bool IsBreakerOpen(Fault fault, DataGroup dataGroup, VICycleDataGroup viCycleDataGroup)
        {
            int samplesPerCycle = (int)Math.Round(dataGroup[0].SampleRate / Frequency);
            int end = Math.Min(fault.EndSample + samplesPerCycle, viCycleDataGroup.IA.RMS.DataPoints.Count) - 1;

            double ia = viCycleDataGroup.IA.RMS.ToSubSeries(fault.EndSample, end).DataPoints.Min(dataPoint => dataPoint.Value);
            double ib = viCycleDataGroup.IB.RMS.ToSubSeries(fault.EndSample, end).DataPoints.Min(dataPoint => dataPoint.Value);
            double ic = viCycleDataGroup.IC.RMS.ToSubSeries(fault.EndSample, end).DataPoints.Min(dataPoint => dataPoint.Value);

            return (ia <= m_openBreakerThreshold) &&
                   (ib <= m_openBreakerThreshold) &&
                   (ic <= m_openBreakerThreshold);
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

        private ComplexNumber ToComplexNumber(CycleDataGroup cycleDataGroup, int cycle)
        {
            Angle angle = cycleDataGroup.Phase[cycle].Value;
            double magnitude = cycleDataGroup.RMS[cycle].Value;
            return new ComplexNumber(angle, magnitude);
        }

        #endregion

        #region [ Static ]

        // Static Fields
        private static readonly ILog Log = LogManager.GetLogger(typeof(FaultDataResource));

        #endregion
    }
}
