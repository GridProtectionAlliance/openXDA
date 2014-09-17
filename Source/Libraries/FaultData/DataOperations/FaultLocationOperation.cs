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
using FaultLocationAlgorithm = FaultAlgorithms.FaultLocationAlgorithm;

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

        // Constants
        // TODO: Hardcoded frequency
        private const double Frequency = 60.0D;

        // Fields
        private string m_connectionString;
        private double m_prefaultMultiplier;
        private double m_ratedCurrentMultiplier;

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

        #endregion

        #region [ Methods ]

        public void Execute(MeterDataSet meterDataSet)
        {
            List<DataGroup> dataGroups = meterDataSet.GetResource<DataGroupsResource>().DataGroups;
            FaultLocationInfoDataContext faultLocationInfo = new FaultLocationInfoDataContext(m_connectionString);

            VIDataGroup viDataGroup;
            VICycleDataSet viCycleDataSet;
            List<Segment> faultDetectedSegments;
            List<Segment> faultTypeSegments;

            int eventID;
            bool prefault;
            FaultSegment faultSegment;
            FaultLocationDataSet faultLocationDataSet;

            FaultLocationData.CycleDataDataTable cycleDataTable = new FaultLocationData.CycleDataDataTable();
            FaultLocationData.FaultCurveDataTable faultCurveTable = new FaultLocationData.FaultCurveDataTable();

            foreach (DataGroup faultGroup in dataGroups.Where(dataGroup => dataGroup.Classification == DataClassification.Fault))
            {
                using (EventTableAdapter adapter = new EventTableAdapter())
                {
                    adapter.Connection.ConnectionString = m_connectionString;
                    eventID = adapter.GetEventIDBy(meterDataSet.Meter.ID, faultGroup.Line.ID, faultGroup.StartTime, faultGroup.EndTime) ?? 0;
                }

                if (eventID == 0)
                    continue;

                viDataGroup = GetVIDataGroup(faultGroup);
                viCycleDataSet = Transform.ToVICycleDataSet(viDataGroup, Frequency);
                faultDetectedSegments = DetectFaults(viDataGroup, viCycleDataSet);
                faultTypeSegments = ClassifyFaults(faultDetectedSegments, viCycleDataSet);

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

                // FAULT LOCATION
                ComplexNumber z0;
                ComplexNumber z1;
                ComplexNumber zSrc;
                ComplexNumber zRem;
                CycleData prefaultCycle;

                DataSeries faultDistanceSeries;
                LineImpedance zLine;

                int lineID = faultGroup.Line.ID;

                zLine = faultLocationInfo.LineImpedances.FirstOrDefault(impedance => impedance.LineID == lineID);

                SourceImpedance zLocal = faultGroup.Line.MeterLocationLines
                    .Where(link => link.MeterLocationID == meterDataSet.Meter.MeterLocationID)
                    .SelectMany(link => faultLocationInfo.SourceImpedances.Where(sourceImpedance => sourceImpedance.MeterLocationLineID == link.ID))
                    .FirstOrDefault();

                List<SourceImpedance> remoteImpedances = faultGroup.Line.MeterLocationLines
                    .Where(link => link.MeterLocationID != meterDataSet.Meter.MeterLocationID)
                    .SelectMany(link => faultLocationInfo.SourceImpedances.Where(sourceImpedance => sourceImpedance.MeterLocationLineID == link.ID))
                    .ToList();

                zSrc = ((object)zLocal != null)
                    ? new ComplexNumber(zLocal.RSrc, zLocal.XSrc)
                    : default(ComplexNumber);

                if (remoteImpedances.Count == 1)
                    zRem = new ComplexNumber(remoteImpedances[0].RSrc, remoteImpedances[0].XSrc);
                else
                    zRem = default(ComplexNumber);

                if ((object)zLine != null)
                {
                    prefaultCycle = null;
                    z0 = new ComplexNumber(zLine.R0, zLine.X0);
                    z1 = new ComplexNumber(zLine.R1, zLine.X1);

                    foreach (FaultLocationAlgorithm faultLocationAlgorithm in GetFaultLocationAlgorithms(faultLocationInfo))
                    {
                        faultDistanceSeries = new DataSeries();

                        foreach (Segment segment in faultTypeSegments)
                        {
                            VICycleDataSet subSet;
                            List<DataPoint> faultDataPoints = null;
                            double[] faultDistances;

                            if ((object)prefaultCycle == null && segment.FaultType == FaultType.None)
                                prefaultCycle = FirstCycle(viCycleDataSet);

                            if (segment.FaultType != FaultType.None)
                            {
                                faultLocationDataSet = new FaultLocationDataSet();
                                faultLocationDataSet.FaultType = segment.FaultType;
                                faultLocationDataSet.LineDistance = faultGroup.Line.Length;
                                faultLocationDataSet.PrefaultCycle = prefaultCycle;
                                faultLocationDataSet.Z0 = z0;
                                faultLocationDataSet.Z1 = z1;
                                faultLocationDataSet.ZSrc = zSrc;
                                faultLocationDataSet.ZRem = zRem;

                                subSet = viCycleDataSet.ToSubSet(segment.StartSample, segment.EndSample);
                                subSet.PushDataTo(faultLocationDataSet.Cycles);

                                if (TryExecute(faultLocationAlgorithm, faultLocationDataSet, out faultDistances))
                                {
                                    faultDataPoints = subSet.VA.RMS.DataPoints
                                        .Zip(faultDistances, (point, distance) => new DataPoint() { Time = point.Time, Value = distance })
                                        .ToList();
                                }
                            }

                            if ((object)faultDataPoints == null)
                            {
                                faultDataPoints = viCycleDataSet.VA.RMS.DataPoints
                                    .Skip(segment.StartSample)
                                    .Take(segment.EndSample - segment.StartSample + 1)
                                    .Select(point => new DataPoint() { Time = point.Time, Value = 0.0D })
                                    .ToList();
                            }

                            foreach (DataPoint faultDataPoint in faultDataPoints)
                                faultDistanceSeries.DataPoints.Add(faultDataPoint);
                        }

                        faultCurveTable.AddFaultCurveRow(eventID, faultLocationAlgorithm.Method.Name, Serialize(faultDistanceSeries));
                    }

                    cycleDataTable.AddCycleDataRow(eventID, viCycleDataSet.ToDataGroup().ToData());
                }
            }

            faultLocationInfo.SubmitChanges();

            if (faultCurveTable.Count > 0)
            {
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(m_connectionString))
                {
                    bulkCopy.BulkCopyTimeout = 0;

                    bulkCopy.DestinationTableName = cycleDataTable.TableName;
                    bulkCopy.WriteToServer(cycleDataTable);

                    bulkCopy.DestinationTableName = faultCurveTable.TableName;
                    bulkCopy.WriteToServer(faultCurveTable);
                }
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

                        i = Math.Min(Math.Min(iaIndex, ibIndex), icIndex);
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

                        i = Math.Max(Math.Max(iaIndex, ibIndex), icIndex);
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
            double max = Math.Max(Math.Max(ia, ib), ic);
            double min = Math.Min(Math.Min(ia, ib), ic);
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

        private byte[] Serialize(DataSeries series)
        {
            DataGroup group = new DataGroup();
            group.Add(series);
            return group.ToData();
        }

        #endregion
    }
}
