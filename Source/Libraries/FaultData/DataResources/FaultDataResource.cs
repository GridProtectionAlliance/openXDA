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
using GSF.Collections;
using log4net;
using FaultLocationAlgorithm = FaultAlgorithms.FaultLocationAlgorithm;
using Line = FaultData.Database.Line;

namespace FaultData.DataResources
{

    public class FaultDataResource : DataResourceBase<MeterDataSet>
    {
        #region [ Members ]

        // Nested Types
        public class Factory
        {
            public DbAdapterContainer DbAdapterContainer;

            public double MaxVoltage;
            public double MaxCurrent;
            public double LowVoltageThreshold;
            public double MaxLowVoltageCurrent;
            public double MaxTimeOffset;
            public double MinTimeOffset;

            public double ResidualCurrentTrigger;
            public double PhaseCurrentTrigger;
            public double PrefaultTrigger;
            public double FaultSuppressionTrigger;
            public double MaxFaultDistanceMultiplier;
            public double MinFaultDistanceMultiplier;

            public FaultDataResource Create()
            {
                return new FaultDataResource()
                {
                    m_dbAdapterContainer = DbAdapterContainer,
                    m_maxVoltage = MaxVoltage,
                    m_maxCurrent = MaxCurrent,
                    m_lowVoltageThreshold = LowVoltageThreshold,
                    m_maxLowVoltageCurrent = MaxLowVoltageCurrent,
                    m_maxTimeOffset = MaxTimeOffset,
                    m_minTimeOffset = MinTimeOffset,
                    m_residualCurrentTrigger = ResidualCurrentTrigger,
                    m_phaseCurrentTrigger = PhaseCurrentTrigger,
                    m_prefaultTrigger = PrefaultTrigger,
                    m_faultSuppressionTrigger = FaultSuppressionTrigger,
                    m_maxFaultDistanceMultiplier = MaxFaultDistanceMultiplier,
                    m_minFaultDistanceMultiplier = MinFaultDistanceMultiplier
                };
            }
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
                    int endSample = fault.Info.EndSample - SamplesPerCycle;

                    // Initialize a fault curve for each algorithm
                    fault.Curves.AddRange(FaultLocationAlgorithms
                        .Select(algorithm => new Fault.Curve(algorithm.Method.Name))
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

        private Dictionary<DataGroup, List<Fault>> m_faultLookup;

        #endregion

        #region [ Constructors ]

        private FaultDataResource()
        {
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

            bool isFault;

            int iaIndex;
            int ibIndex;
            int icIndex;

            for (int i = 0; i < iaRMS.DataPoints.Count; i++)
            {
                ia = iaRMS.DataPoints[i].Value;
                ib = ibRMS.DataPoints[i].Value;
                ic = icRMS.DataPoints[i].Value;
                ir = irRMS.DataPoints[i].Value;

                isFault = IsFaultObvious(ia, ib, ic, ir) ||
                          (IsFaultApparent(viCycleDataGroup, i) && !IsFaultSuppressed(viDataGroup, i));

                if (isFault)
                {
                    if ((object)currentFault == null)
                    {
                        iaIndex = FindFaultInception(viDataGroup.IA, i);
                        ibIndex = FindFaultInception(viDataGroup.IB, i);
                        icIndex = FindFaultInception(viDataGroup.IC, i);

                        currentFault = new Fault();
                        currentFault.Info.StartSample = Common.Min(iaIndex, ibIndex, icIndex);
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

                        currentFault.Info.EndSample = Common.Max(iaIndex, ibIndex, icIndex) - 1;
                        currentFault = null;
                    }
                }
            }

            if ((object)currentFault != null)
                currentFault.Info.EndSample = iaRMS.DataPoints.Count - 1;

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

        private bool IsFaultSuppressed(VIDataGroup viDataGroup, int index)
        {
            DataSeries[] phaseCurrents =
            {
                viDataGroup.IA,
                viDataGroup.IB,
                viDataGroup.IC
            };

            int samplesPerCycle;
            List<double> cycle;
            double dist;

            foreach (DataSeries series in phaseCurrents)
            {
                // Calculate the sample rate of the series
                samplesPerCycle = (int)Math.Round(series.SampleRate / Frequency);

                // Get the values of the data points
                // in the cycle in sorted order
                cycle = series
                    .ToSubSeries(index, index + samplesPerCycle - 1)
                    .DataPoints.Select(dataPoint => dataPoint.Value)
                    .OrderBy(value => value)
                    .ToList();

                // Distance between median and average of the cycle
                dist = Math.Abs(cycle[cycle.Count / 2] - cycle.Average());

                // If the distance exceeds the trigger, fault is suppressed
                if (dist > m_faultSuppressionTrigger)
                    return true;
            }

            return false;
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
                for (int i = fault.Info.StartSample; i <= fault.Info.EndSample; i++)
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
                    currentSegment.EndTime = viCycleDataGroup.IA.RMS[fault.Info.EndSample].Time;
                    currentSegment.EndSample = fault.Info.EndSample;
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
            int largestFaultCycle = GetLargestFaultCycle(fault, viCycleDataGroup);
            DateTime startTime = viCycleDataGroup.IA.RMS[fault.Info.StartSample].Time;
            DateTime endTime = viCycleDataGroup.IA.RMS[fault.Info.EndSample].Time;

            List<Tuple<int, string, double>> distances;

            fault.Info.CalculationCycle = largestFaultCycle;
            fault.Info.StartSample = fault.Info.StartSample;
            fault.Info.EndSample = fault.Info.EndSample;
            fault.Info.InceptionTime = startTime;
            fault.Info.ClearingTime = endTime;
            fault.Info.Duration = endTime - startTime;

            if (fault.Segments.Any())
            {
                fault.Info.Type = fault.Segments
                    .MaxBy(segment => segment.EndSample - segment.StartSample)
                    .FaultType;
            }

            if (largestFaultCycle >= 0)
            {
                distances = fault.Curves
                    .Select((curve, index) => Tuple.Create(index, curve.Algorithm, curve[largestFaultCycle].Value))
                    .Where(tuple => IsValid(tuple.Item3, dataGroup))
                    .OrderBy(tuple => tuple.Item3)
                    .ToList();

                if (distances.Any())
                {
                    fault.Info.DistanceAlgorithmIndex = distances[distances.Count / 2].Item1;
                    fault.Info.DistanceAlgorithm = distances[distances.Count / 2].Item2;
                    fault.Info.Distance = distances[distances.Count / 2].Item3;
                    fault.Info.NumberOfValidDistances = distances.Count;
                }
            }
        }

        private bool IsValid(double faultDistance, DataGroup dataGroup)
        {
            double lineLength = dataGroup.Line.Length;
            double minDistance = m_minFaultDistanceMultiplier * lineLength;
            double maxDistance = m_maxFaultDistanceMultiplier * lineLength;
            return faultDistance >= minDistance && faultDistance <= maxDistance;
        }

        private int GetLargestFaultCycle(Fault fault, VICycleDataGroup viCycleDataGroup)
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

            startSample = fault.Info.StartSample;
            endSample = startSample + fault.Curves.Min(curve => curve.Series.DataPoints.Count) - 1;

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

            return maxIndex - startSample;
        }

        #endregion

        #region [ Static ]

        // Static Fields
        private static readonly ILog Log = LogManager.GetLogger(typeof(FaultDataResource));

        #endregion
    }
}
