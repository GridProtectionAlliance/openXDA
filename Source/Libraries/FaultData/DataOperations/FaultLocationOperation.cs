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
using GSF.NumericalAnalysis;
using FaultData.DataAnalysis;
using FaultData.Database;
using FaultData.Database.MeterDataTableAdapters;
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

            public CycleDataGroup VA = new CycleDataGroup();
            public CycleDataGroup VB = new CycleDataGroup();
            public CycleDataGroup VC = new CycleDataGroup();
            public CycleDataGroup IA = new CycleDataGroup();
            public CycleDataGroup IB = new CycleDataGroup();
            public CycleDataGroup IC = new CycleDataGroup();
        }

        private class CycleDataGroup
        {
            public DataSeries RMSSeries = new DataSeries();
            public DataSeries PeakSeries = new DataSeries();
            public DataSeries PhaseSeries = new DataSeries();
            public DataSeries ErrorSeries = new DataSeries();
        }

        // Constants
        // TODO: Hardcoded frequency
        private const double Frequency = 60.0D;

        // Fields
        private string m_connectionString;
        private double m_prefaultMultiplier;

        #endregion

        #region [ Constructors ]

        public FaultLocationOperation(string connectionString)
        {
            m_connectionString = connectionString;
            m_prefaultMultiplier = 1.5D;
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

        #endregion

        #region [ Methods ]

        public void Execute(MeterDataSet meterDataSet)
        {
            List<DataGroup> dataGroups = meterDataSet.GetResource<DataGroupsResource>().DataGroups;
            FaultLocationInfoDataContext faultLocationInfo = new FaultLocationInfoDataContext(m_connectionString);

            Dictionary<string, DataSeries> seriesLookup;
            Dictionary<string, DataSeries> rmsLookup;
            List<FaultType> faultTypes;
            List<Segment> segments;

            int eventID;
            bool prefault;
            FaultSegment faultSegment;
            FaultLocationDataSet faultLocationDataSet;

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

                seriesLookup = GetSeriesLookup(faultGroup);
                rmsLookup = seriesLookup.ToDictionary(kvp => kvp.Key, kvp => GetRMS(kvp.Value));
                faultTypes = GetFaultTypes(rmsLookup);
                segments = GetSegments(faultTypes);

                prefault = true;

                foreach (Segment segment in segments)
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

                    if (segment.FaultType != FaultType.None)
                        FillCycleDataGroups(seriesLookup, rmsLookup, segment);
                }

                // FAULT LOCATION
                ComplexNumber z0;
                ComplexNumber z1;
                ComplexNumber? zSrc;
                CycleData prefaultCycle;

                DataSeries faultDistanceSeries;
                Impedance zRow;

                zRow = faultLocationInfo.Impedances.FirstOrDefault(impedance => impedance.LineID == faultGroup.Line.ID);

                if ((object)zRow != null)
                {
                    prefaultCycle = null;
                    z0 = new ComplexNumber(zRow.R0, zRow.X0);
                    z1 = new ComplexNumber(zRow.R1, zRow.X1);

                    zSrc = ((object)zRow.RSrc != null && (object)zRow.XSrc != null)
                        ? new ComplexNumber(zRow.RSrc.Value, zRow.XSrc.Value)
                        : (ComplexNumber?)null;

                    foreach (FaultLocationAlgorithm faultLocationAlgorithm in GetFaultLocationAlgorithms(faultLocationInfo, faultGroup.Line))
                    {
                        faultDistanceSeries = new DataSeries();
                        faultDistanceSeries.DataPoints = new List<DataPoint>();

                        foreach (Segment segment in segments)
                        {
                            List<DataPoint> faultDataPoints = null;
                            double[] faultDistances;

                            if ((object)prefaultCycle == null && segment.FaultType == FaultType.None)
                            {
                                Segment firstCycleSegment = new Segment()
                                {
                                    StartSample = 0,
                                    EndSample = 0
                                };

                                FillCycleDataGroups(seriesLookup, rmsLookup, firstCycleSegment);
                                prefaultCycle = FirstCycle(firstCycleSegment);
                            }

                            if (segment.FaultType != FaultType.None)
                            {
                                faultLocationDataSet = GetFaultLocationDataSet(segment);
                                faultLocationDataSet.LineDistance = faultGroup.Line.Length;
                                faultLocationDataSet.PrefaultCycle = prefaultCycle;
                                faultLocationDataSet.Z0 = z0;
                                faultLocationDataSet.Z1 = z1;
                                faultLocationDataSet.ZSrc = zSrc.GetValueOrDefault();

                                if (TryExecute(faultLocationAlgorithm, faultLocationDataSet, out faultDistances))
                                {
                                    faultDataPoints = segment.VA.RMSSeries.DataPoints
                                        .Zip(faultDistances, (point, distance) => new DataPoint() { Time = point.Time, Value = distance })
                                        .ToList();
                                }
                            }

                            if ((object)faultDataPoints == null)
                            {
                                faultDataPoints = rmsLookup["VA"].DataPoints
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
                }
            }

            faultLocationInfo.SubmitChanges();

            if (faultCurveTable.Count > 0)
            {
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(m_connectionString))
                {
                    bulkCopy.BulkCopyTimeout = 0;
                    bulkCopy.DestinationTableName = faultCurveTable.TableName;
                    bulkCopy.WriteToServer(faultCurveTable);
                }
            }
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

        private void FillCycleDataGroups(Dictionary<string, DataSeries> seriesLookup, Dictionary<string, DataSeries> rmsLookup, Segment segment)
        {
            FillCycleDataGroup(seriesLookup["VA"], rmsLookup["VA"], segment, segment.VA);
            FillCycleDataGroup(seriesLookup["VB"], rmsLookup["VB"], segment, segment.VB);
            FillCycleDataGroup(seriesLookup["VC"], rmsLookup["VC"], segment, segment.VC);
            FillCycleDataGroup(seriesLookup["IA"], rmsLookup["IA"], segment, segment.IA);
            FillCycleDataGroup(seriesLookup["IB"], rmsLookup["IB"], segment, segment.IB);
            FillCycleDataGroup(seriesLookup["IC"], rmsLookup["IC"], segment, segment.IC);
        }

        private void FillCycleDataGroup(DataSeries waveForm, DataSeries rmsSeries, Segment segment, CycleDataGroup cycleDataGroup)
        {
            int samplesPerCycle = (int)Math.Round(waveForm.SampleRate / Frequency);
            double[] yValues = new double[samplesPerCycle];
            double[] tValues = new double[samplesPerCycle];

            cycleDataGroup.RMSSeries.DataPoints = new List<DataPoint>();
            cycleDataGroup.PhaseSeries.DataPoints = new List<DataPoint>();
            cycleDataGroup.PeakSeries.DataPoints = new List<DataPoint>();
            cycleDataGroup.ErrorSeries.DataPoints = new List<DataPoint>();

            for (int i = segment.StartSample; i <= segment.EndSample; i++)
            {
                DateTime cycleTime = waveForm.DataPoints[i].Time;

                for (int j = 0; j < samplesPerCycle; j++)
                {
                    yValues[j] = waveForm.DataPoints[i + j].Value;
                    tValues[j] = (waveForm.DataPoints[i + j].Time - cycleTime).TotalSeconds;
                }

                SineWave sineFit = WaveFit.SineFit(yValues, tValues, Frequency);

                cycleDataGroup.RMSSeries.DataPoints.Add(rmsSeries.DataPoints[i]);

                cycleDataGroup.PhaseSeries.DataPoints.Add(new DataPoint()
                {
                    Time = cycleTime,
                    Value = sineFit.Phase
                });

                cycleDataGroup.PeakSeries.DataPoints.Add(new DataPoint()
                {
                    Time = cycleTime,
                    Value = sineFit.Amplitude
                });

                cycleDataGroup.ErrorSeries.DataPoints.Add(new DataPoint()
                {
                    Time = cycleTime,

                    Value = tValues
                        .Select(sineFit.CalculateY)
                        .Zip(yValues, (estimate, value) => Math.Abs(estimate - value))
                        .Sum()
                });
            }
        }

        private DataSeries GetRMS(DataSeries series)
        {
            return series.ToRMS((int)Math.Round(series.SampleRate / Frequency));
        }

        private List<FaultType> GetFaultTypes(Dictionary<string, DataSeries> rmsLookup)
        {
            DataSeries ia = rmsLookup["IA"];
            DataSeries ib = rmsLookup["IB"];
            DataSeries ic = rmsLookup["IC"];

            return GetFaultTypes(ia, ib, ic);
        }

        private List<FaultType> GetFaultTypes(DataSeries iaRMS, DataSeries ibRMS, DataSeries icRMS)
        {
            List<FaultType> faultTypes = new List<FaultType>();

            double anPrefault = iaRMS.DataPoints[0].Value;
            double bnPrefault = ibRMS.DataPoints[0].Value;
            double cnPrefault = icRMS.DataPoints[0].Value;

            double prefaultSum = anPrefault + bnPrefault + cnPrefault;
            double prefaultMultiplied = prefaultSum * PrefaultMultiplier;

            int numPhases;

            for (int i = 0; i < iaRMS.DataPoints.Count; i++)
            {
                double an = iaRMS.DataPoints[i].Value;
                double bn = ibRMS.DataPoints[i].Value;
                double cn = icRMS.DataPoints[i].Value;
                double sum = an + bn + cn;
                bool isFault = sum > prefaultMultiplied;

                if (isFault)
                {
                    numPhases = GetNumPhases(4.0D, an, bn, cn);

                    if (numPhases == 3)
                        numPhases = GetNumPhases(1.5D, an - anPrefault, bn - bnPrefault, cn - cnPrefault);

                    faultTypes.Add(GetFaultType(numPhases, an, bn, cn));
                }
                else
                {
                    faultTypes.Add(FaultType.None);
                }
            }

            return faultTypes;
        }

        private int GetNumPhases(double factor, double an, double bn, double cn)
        {
            double max = Math.Max(Math.Max(an, bn), cn);
            double min = Math.Min(Math.Min(an, bn), cn);
            double mid = (an + bn + cn) - max - min;

            if (max > factor * mid)
                return 1;

            if (mid > factor * min)
                return 2;

            return 3;
        }

        private FaultType GetFaultType(int numPhases, double an, double bn, double cn)
        {
            if (numPhases == 3)
                return FaultType.ABC;

            if (numPhases == 2)
            {
                if (cn < an && cn < bn)
                    return FaultType.AB;

                if (an < bn && an < cn)
                    return FaultType.BC;

                if (bn < an && bn < cn)
                    return FaultType.CA;
            }

            if (numPhases == 1)
            {
                if (an > bn && an > cn)
                    return FaultType.AN;

                if (bn > an && bn > cn)
                    return FaultType.BN;

                if (cn > an && cn > bn)
                    return FaultType.CN;
            }

            return FaultType.None;
        }

        private List<Segment> GetSegments(List<FaultType> faultTypes)
        {
            List<Segment> segments = new List<Segment>();
            Segment currentSegment = null;

            for (int i = 0; i < faultTypes.Count; i++)
            {
                if ((object)currentSegment == null || currentSegment.FaultType != faultTypes[i])
                {
                    if ((object)currentSegment != null)
                        currentSegment.EndSample = i - 1;

                    currentSegment = new Segment()
                    {
                        FaultType = faultTypes[i],
                        StartSample = i
                    };

                    segments.Add(currentSegment);
                }
            }

            if ((object)currentSegment != null)
                currentSegment.EndSample = faultTypes.Count - 1;

            return segments;
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

                case FaultType.ABC:
                    return faultLocationInfo.SegmentTypes.First(type => type.Name == "3-Phase Fault");

                default:
                    return prefault
                        ? faultLocationInfo.SegmentTypes.First(type => type.Name == "Prefault")
                        : faultLocationInfo.SegmentTypes.First(type => type.Name == "Postfault");
            }
        }

        private FaultLocationDataSet GetFaultLocationDataSet(Segment segment)
        {
            FaultLocationDataSet faultLocationDataSet;
            CycleDataSet cycleDataSet;

            faultLocationDataSet = new FaultLocationDataSet();
            faultLocationDataSet.FaultType = segment.FaultType;
            cycleDataSet = faultLocationDataSet.Cycles;

            for (int i = 0; i <= segment.EndSample - segment.StartSample; i++)
            {
                CycleData cycle = new CycleData();

                cycle.AN.V.RMS = segment.VA.RMSSeries.DataPoints[i].Value;
                cycle.AN.V.Phase = segment.VA.PhaseSeries.DataPoints[i].Value;
                cycle.AN.V.Peak = segment.VA.PeakSeries.DataPoints[i].Value;
                cycle.AN.V.Error = segment.VA.ErrorSeries.DataPoints[i].Value;

                cycle.BN.V.RMS = segment.VB.RMSSeries.DataPoints[i].Value;
                cycle.BN.V.Phase = segment.VB.PhaseSeries.DataPoints[i].Value;
                cycle.BN.V.Peak = segment.VB.PeakSeries.DataPoints[i].Value;
                cycle.BN.V.Error = segment.VB.ErrorSeries.DataPoints[i].Value;

                cycle.CN.V.RMS = segment.VC.RMSSeries.DataPoints[i].Value;
                cycle.CN.V.Phase = segment.VC.PhaseSeries.DataPoints[i].Value;
                cycle.CN.V.Peak = segment.VC.PeakSeries.DataPoints[i].Value;
                cycle.CN.V.Error = segment.VC.ErrorSeries.DataPoints[i].Value;

                cycle.AN.I.RMS = segment.IA.RMSSeries.DataPoints[i].Value;
                cycle.AN.I.Phase = segment.IA.PhaseSeries.DataPoints[i].Value;
                cycle.AN.I.Peak = segment.IA.PeakSeries.DataPoints[i].Value;
                cycle.AN.I.Error = segment.IA.ErrorSeries.DataPoints[i].Value;

                cycle.BN.I.RMS = segment.IB.RMSSeries.DataPoints[i].Value;
                cycle.BN.I.Phase = segment.IB.PhaseSeries.DataPoints[i].Value;
                cycle.BN.I.Peak = segment.IB.PeakSeries.DataPoints[i].Value;
                cycle.BN.I.Error = segment.IB.ErrorSeries.DataPoints[i].Value;

                cycle.CN.I.RMS = segment.IC.RMSSeries.DataPoints[i].Value;
                cycle.CN.I.Phase = segment.IC.PhaseSeries.DataPoints[i].Value;
                cycle.CN.I.Peak = segment.IC.PeakSeries.DataPoints[i].Value;
                cycle.CN.I.Error = segment.IC.ErrorSeries.DataPoints[i].Value;

                cycleDataSet[i] = cycle;
            }

            return faultLocationDataSet;
        }

        private CycleData FirstCycle(Segment segment)
        {
            CycleData cycle = new CycleData();

            cycle.AN.V.RMS = segment.VA.RMSSeries.DataPoints[0].Value;
            cycle.AN.V.Phase = segment.VA.PhaseSeries.DataPoints[0].Value;
            cycle.AN.V.Peak = segment.VA.PeakSeries.DataPoints[0].Value;
            cycle.AN.V.Error = segment.VA.ErrorSeries.DataPoints[0].Value;

            cycle.BN.V.RMS = segment.VB.RMSSeries.DataPoints[0].Value;
            cycle.BN.V.Phase = segment.VB.PhaseSeries.DataPoints[0].Value;
            cycle.BN.V.Peak = segment.VB.PeakSeries.DataPoints[0].Value;
            cycle.BN.V.Error = segment.VB.ErrorSeries.DataPoints[0].Value;

            cycle.CN.V.RMS = segment.VC.RMSSeries.DataPoints[0].Value;
            cycle.CN.V.Phase = segment.VC.PhaseSeries.DataPoints[0].Value;
            cycle.CN.V.Peak = segment.VC.PeakSeries.DataPoints[0].Value;
            cycle.CN.V.Error = segment.VC.ErrorSeries.DataPoints[0].Value;

            cycle.AN.I.RMS = segment.IA.RMSSeries.DataPoints[0].Value;
            cycle.AN.I.Phase = segment.IA.PhaseSeries.DataPoints[0].Value;
            cycle.AN.I.Peak = segment.IA.PeakSeries.DataPoints[0].Value;
            cycle.AN.I.Error = segment.IA.ErrorSeries.DataPoints[0].Value;

            cycle.BN.I.RMS = segment.IB.RMSSeries.DataPoints[0].Value;
            cycle.BN.I.Phase = segment.IB.PhaseSeries.DataPoints[0].Value;
            cycle.BN.I.Peak = segment.IB.PeakSeries.DataPoints[0].Value;
            cycle.BN.I.Error = segment.IB.ErrorSeries.DataPoints[0].Value;

            cycle.CN.I.RMS = segment.IC.RMSSeries.DataPoints[0].Value;
            cycle.CN.I.Phase = segment.IC.PhaseSeries.DataPoints[0].Value;
            cycle.CN.I.Peak = segment.IC.PeakSeries.DataPoints[0].Value;
            cycle.CN.I.Error = segment.IC.ErrorSeries.DataPoints[0].Value;

            return cycle;
        }

        private List<FaultLocationAlgorithm> GetFaultLocationAlgorithms(FaultLocationInfoDataContext faultLocationInfo, Line line)
        {
            return faultLocationInfo.FaultLocationAlgorithms
                .Where(dbAlgorithm => dbAlgorithm.LineID == null || dbAlgorithm.LineID == line.ID)
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
