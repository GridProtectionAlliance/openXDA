//******************************************************************************************************
//  BreakerTimingOperation.cs - Gbtc
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
//  06/28/2015 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using FaultData.Configuration;
using FaultData.DataAnalysis;
using FaultData.Database;
using FaultData.Database.MeterDataTableAdapters;
using FaultData.DataResources;
using FaultData.DataSets;
using GSF;
using GSF.Configuration;
using GSF.Data;
using EventKey = System.Tuple<int, System.DateTime, System.DateTime>;

namespace FaultData.DataOperations
{
    public enum BreakerOperationType
    {
        Normal,
        Late,
        Indeterminate
    }

    public class BreakerTimingOperation : DataOperationBase<MeterDataSet>
    {
        #region [ Members ]

        // Nested Types
        private class BreakerTiming
        {
            #region [ Members ]

            // Fields
            private XValue m_timeEnergized;
            private XValue m_timeCleared;
            private double m_speed;

            #endregion

            #region [ Constructors ]

            public BreakerTiming(XValue timeEnergized, DataSeries statusChannel, double speed)
            {
                m_timeEnergized = timeEnergized;

                if ((object)statusChannel != null)
                    m_timeCleared = FindStatusBitSet(statusChannel, timeEnergized.Index);

                m_speed = speed;
            }

            #endregion

            #region [ Properties ]

            public XValue TimeEnergized
            {
                get
                {
                    return m_timeEnergized;
                }
            }

            public XValue TimeCleared
            {
                get
                {
                    return m_timeCleared;
                }
            }

            public bool IsValid
            {
                get
                {
                    return (object)m_timeCleared != null;
                }
            }

            public double Speed
            {
                get
                {
                    return m_speed;
                }
            }

            #endregion

            #region [ Methods ]

            private XValue FindStatusBitSet(DataSeries statusSeries, int startIndex)
            {
                for (int i = startIndex; i < statusSeries.DataPoints.Count; i++)
                {
                    if (statusSeries[i].Value != 0.0D)
                        return new XValue(i, statusSeries[i].Time);
                }

                return null;
            }

            #endregion
        }

        private class PhaseTiming
        {
            #region [ Members ]

            // Fields
            private DataSeries m_waveform;
            private CycleDataGroup m_cycleDataGroup;
            private XValue m_timeCleared;
            private double m_timing;
            private double m_systemFrequency;

            #endregion

            #region [ Constructors ]

            public PhaseTiming(DataSeries waveform, CycleDataGroup cycleDataGroup, BreakerTiming breakerTiming, double systemFrequency, double openBreakerThreshold)
            {
                m_waveform = waveform;
                m_cycleDataGroup = cycleDataGroup;
                m_timeCleared = FindBreakerOpen(breakerTiming.TimeEnergized.Index, openBreakerThreshold);

                if ((object)m_timeCleared != null)
                    m_timing = (m_timeCleared.Time - breakerTiming.TimeEnergized.Time).TotalSeconds * systemFrequency;
                else
                    m_timing = double.NaN;

                m_systemFrequency = systemFrequency;
            }

            #endregion

            #region [ Properties ]

            public XValue TimeCleared
            {
                get
                {
                    return m_timeCleared;
                }
            }

            public double Timing
            {
                get
                {
                    return m_timing;
                }
            }

            public bool IsValid
            {
                get
                {
                    return !double.IsNaN(m_timing);
                }
            }

            #endregion

            #region [ Methods ]

            private XValue FindBreakerOpen(int startIndex, double openBreakerThreshold)
            {
                int cycleIndex;
                int sampleCleared;

                cycleIndex = m_cycleDataGroup.Peak.DataPoints
                    .Skip(startIndex)
                    .TakeWhile(dataPoint => dataPoint.Value > openBreakerThreshold)
                    .Count() + startIndex;

                if (cycleIndex == m_cycleDataGroup.Peak.DataPoints.Count)
                    return null;

                sampleCleared = FindIndexCleared(m_waveform, cycleIndex);

                return new XValue(sampleCleared, m_waveform[sampleCleared].Time);
            }

            private int FindIndexCleared(DataSeries waveform, int cycleIndex)
            {
                int samplesPerCycle = (int)Math.Round(waveform.SampleRate / m_systemFrequency);
                int startIndex = cycleIndex - 1;
                int endIndex = startIndex + samplesPerCycle - 1;
                int postfaultIndex = Math.Min(endIndex + samplesPerCycle, waveform.DataPoints.Count - 1);

                double largestPostfaultPeak;
                double largestFaultCyclePeak;

                double previousValue;
                double value;
                double nextValue;

                // If cycleIndex is zero, assume that the breaker
                // is open at the very start of the waveform
                if (cycleIndex == 0)
                    return 0;

                largestPostfaultPeak = 0.0D;
                largestFaultCyclePeak = 0.0D;

                // Find the largest postfault peak as the absolute
                // peak of the cycle after the last faulted cycle
                for (int i = postfaultIndex; i > endIndex; i--)
                {
                    value = Math.Abs(waveform[i].Value);

                    if (value > largestPostfaultPeak)
                        largestPostfaultPeak = value;
                }

                // Find the largest peak of the last faulted cycle
                for (int i = startIndex; i <= endIndex; i++)
                {
                    value = Math.Abs(waveform[i].Value);

                    if (value > largestFaultCyclePeak)
                        largestFaultCyclePeak = value;
                }

                // Scanning backwards, find the first point where the value exceeds
                // a point 25% of the way from the postfault peak to the fault peak
                for (int i = endIndex; i >= startIndex; i--)
                {
                    value = Math.Abs(waveform[i].Value);

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

                    if (i + 1 >= waveform.DataPoints.Count)
                        continue;

                    previousValue = waveform.DataPoints[i - 1].Value;
                    value = waveform.DataPoints[i].Value;
                    nextValue = waveform.DataPoints[i + 1].Value;

                    if (value * nextValue < 0.0D)
                        return i;

                    if (Math.Abs(value - previousValue) > 5.0D * Math.Abs(nextValue - value))
                        return i;
                }

                return startIndex;
            }

            #endregion
        }

        private class XValue : IComparable<XValue>
        {
            public readonly int Index;
            public readonly DateTime Time;

            public XValue(int index, DateTime time)
            {
                Index = index;
                Time = time;
            }

            public int CompareTo(XValue dataPoint)
            {
                return Index.CompareTo(dataPoint.Index);
            }
        }

        // Fields
        private double m_systemFrequency;
        private BreakerSettings m_breakerSettings;

        private DbAdapterContainer m_dbAdapterContainer;
        private MeterDataSet m_meterDataSet;

        private DataContextLookup<string, Phase> m_phaseLookup;
        private MeterData.BreakerOperationDataTable m_breakerOperationTable;
        private List<Tuple<EventKey, MeterData.BreakerOperationRow>> m_breakerOperations;

        #endregion

        #region [ Constructors ]

        public BreakerTimingOperation()
        {
            m_breakerSettings = new BreakerSettings();
        }

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

        public override void Prepare(DbAdapterContainer dbAdapterContainer)
        {
            m_dbAdapterContainer = dbAdapterContainer;

            m_phaseLookup = new DataContextLookup<string, Phase>(dbAdapterContainer.GetAdapter<MeterInfoDataContext>(), phase => phase.Name);
            m_breakerOperationTable = new MeterData.BreakerOperationDataTable();
            m_breakerOperations = new List<Tuple<EventKey, MeterData.BreakerOperationRow>>();

            LoadBreakerOperationTypes(dbAdapterContainer);
        }

        public override void Execute(MeterDataSet meterDataSet)
        {
            CycleDataResource cycleDataResource;

            DataGroup dataGroup;
            VIDataGroup viDataGroup;
            VICycleDataGroup viCycleDataGroup;

            cycleDataResource = meterDataSet.GetResource<CycleDataResource>();

            for (int i = 0; i < cycleDataResource.DataGroups.Count; i++)
            {
                dataGroup = cycleDataResource.DataGroups[i];
                viDataGroup = cycleDataResource.VIDataGroups[i];
                viCycleDataGroup = cycleDataResource.VICycleDataGroups[i];
                IdentifyBreakerOperations(dataGroup, viDataGroup, viCycleDataGroup);
            }

            m_meterDataSet = meterDataSet;
        }

        public override void Load(DbAdapterContainer dbAdapterContainer)
        {
            MeterData.EventDataTable eventTable;
            Dictionary<EventKey, MeterData.EventRow> eventLookup;
            MeterData.EventRow eventRow;
            BulkLoader bulkLoader;

            eventTable = dbAdapterContainer.GetAdapter<EventTableAdapter>().GetDataByFileGroup(m_meterDataSet.FileGroup.ID);
            eventLookup = eventTable.Where(evt => evt.MeterID == m_meterDataSet.Meter.ID).ToDictionary(CreateEventKey);

            foreach (Tuple<EventKey, MeterData.BreakerOperationRow> breakerOperation in m_breakerOperations)
            {
                if (eventLookup.TryGetValue(breakerOperation.Item1, out eventRow))
                {
                    breakerOperation.Item2.EventID = eventRow.ID;
                    m_breakerOperationTable.AddBreakerOperationRow(breakerOperation.Item2);
                }
            }

            bulkLoader = new BulkLoader();
            bulkLoader.Connection = dbAdapterContainer.Connection;
            bulkLoader.CommandTimeout = dbAdapterContainer.CommandTimeout;
            bulkLoader.Load(m_breakerOperationTable);
        }

        private void IdentifyBreakerOperations(DataGroup dataGroup, VIDataGroup viDataGroup, VICycleDataGroup viCycleDataGroup)
        {
            List<string> breakerNumbers = dataGroup.DataSeries
                .SelectMany(dataSeries => dataSeries.SeriesInfo.Channel.BreakerChannels)
                .Select(breakerChannel => breakerChannel.BreakerNumber)
                .Distinct()
                .ToList();

            foreach (string breakerNumber in breakerNumbers)
            {
                double breakerSpeed = ConvertBreakerSpeed(m_dbAdapterContainer.Connection.ExecuteScalar("SELECT BreakerSpeed FROM MaximoBreaker WHERE BreakerNum = @breakerNumber", breakerNumber));

                List<DataSeries> breakerDigitals = dataGroup.DataSeries
                    .Where(dataSeries => dataSeries.SeriesInfo.Channel.MeasurementType.Name == "Digital")
                    .Where(dataSeries => dataSeries.SeriesInfo.Channel.BreakerChannels.Any(breakerChannel => breakerChannel.BreakerNumber == breakerNumber))
                    .ToList();

                List<DataSeries> tripCoilEnergizedChannels = breakerDigitals
                    .Where(dataSeries => dataSeries.SeriesInfo.Channel.MeasurementCharacteristic.Name == "TCE")
                    .ToList();

                DataSeries breakerStatusChannel = breakerDigitals
                    .FirstOrDefault(dataSeries => dataSeries.SeriesInfo.Channel.MeasurementCharacteristic.Name == "BreakerStatus");

                List<XValue> breakerOperations = Range<XValue>.MergeAllOverlapping(tripCoilEnergizedChannels.SelectMany(FindTCERanges))
                    .Select(range => range.Start)
                    .ToList();

                foreach (XValue breakerOperation in breakerOperations)
                {
                    BreakerTiming breakerTiming = new BreakerTiming(breakerOperation, breakerStatusChannel, breakerSpeed);

                    PhaseTiming aPhaseTiming = new PhaseTiming(viDataGroup.IA, viCycleDataGroup.IA, breakerTiming, m_systemFrequency, m_breakerSettings.OpenBreakerThreshold);
                    PhaseTiming bPhaseTiming = new PhaseTiming(viDataGroup.IB, viCycleDataGroup.IB, breakerTiming, m_systemFrequency, m_breakerSettings.OpenBreakerThreshold);
                    PhaseTiming cPhaseTiming = new PhaseTiming(viDataGroup.IC, viCycleDataGroup.IC, breakerTiming, m_systemFrequency, m_breakerSettings.OpenBreakerThreshold);

                    MeterData.BreakerOperationRow breakerOperationRow = GetBreakerOperationRow(breakerNumber, breakerTiming, aPhaseTiming, bPhaseTiming, cPhaseTiming);

                    m_breakerOperations.Add(Tuple.Create(CreateEventKey(dataGroup), breakerOperationRow));
                }
            }
        }

        private MeterData.BreakerOperationRow GetBreakerOperationRow(string breakerNumber, BreakerTiming breakerTiming, PhaseTiming aPhaseTiming, PhaseTiming bPhaseTiming, PhaseTiming cPhaseTiming)
        {
            double maxTiming = GetMaxTiming(aPhaseTiming, bPhaseTiming, cPhaseTiming);
            string phase = GetLatestPhase(aPhaseTiming, bPhaseTiming, cPhaseTiming);
            BreakerOperationType type = GetBreakerOperationType(maxTiming, breakerTiming.Speed);

            MeterData.BreakerOperationRow breakerOperationRow = m_breakerOperationTable.NewBreakerOperationRow();

            breakerOperationRow.PhaseID = m_phaseLookup.GetOrAdd(phase, name => new Phase() { Name = name, Description = name }).ID;
            breakerOperationRow.BreakerOperationTypeID = s_breakerOperationTypeLookup[type];
            breakerOperationRow.BreakerNumber = breakerNumber;
            breakerOperationRow.TripCoilEnergized = breakerTiming.TimeEnergized.Time;
            breakerOperationRow.StatusBitSet = breakerTiming.IsValid ? breakerTiming.TimeCleared.Time : breakerTiming.TimeEnergized.Time;
            breakerOperationRow.APhaseCleared = aPhaseTiming.IsValid ? aPhaseTiming.TimeCleared.Time : breakerTiming.TimeEnergized.Time;
            breakerOperationRow.BPhaseCleared = bPhaseTiming.IsValid ? bPhaseTiming.TimeCleared.Time : breakerTiming.TimeEnergized.Time;
            breakerOperationRow.CPhaseCleared = cPhaseTiming.IsValid ? cPhaseTiming.TimeCleared.Time : breakerTiming.TimeEnergized.Time;
            breakerOperationRow.BreakerTiming = NotNaN(maxTiming);
            breakerOperationRow.APhaseBreakerTiming = NotNaN(aPhaseTiming.Timing);
            breakerOperationRow.BPhaseBreakerTiming = NotNaN(bPhaseTiming.Timing);
            breakerOperationRow.CPhaseBreakerTiming = NotNaN(cPhaseTiming.Timing);
            breakerOperationRow.BreakerSpeed = NotNaN(breakerTiming.Speed);

            return breakerOperationRow;
        }

        private List<Range<XValue>> FindTCERanges(DataSeries tceSeries)
        {
            List<int> stateChanges = new List<int>();
            List<Range<XValue>> ranges = new List<Range<XValue>>();
            XValue start;
            XValue end;

            for (int i = 1; i < tceSeries.DataPoints.Count; i++)
            {
                if (tceSeries[i - 1].Value != tceSeries[i].Value)
                    stateChanges.Add(i);
            }

            for (int i = 1; i < stateChanges.Count; i++)
            {
                int startIndex = stateChanges[i - 1];
                int endIndex = stateChanges[i] - 1;

                if (tceSeries[startIndex].Value != 0.0D)
                {
                    start = new XValue(startIndex, tceSeries[startIndex].Time);
                    end = new XValue(endIndex, tceSeries[endIndex].Time);
                    ranges.Add(new Range<XValue>(start, end));
                }
            }

            return ranges;
        }

        private double GetMaxTiming(PhaseTiming aPhaseTiming, PhaseTiming bPhaseTiming, PhaseTiming cPhaseTiming)
        {
            if (!aPhaseTiming.IsValid)
                return double.NaN;

            if (!bPhaseTiming.IsValid)
                return double.NaN;

            if (!cPhaseTiming.IsValid)
                return double.NaN;

            return Common.Max(aPhaseTiming.Timing, bPhaseTiming.Timing, cPhaseTiming.Timing);
        }

        private string GetLatestPhase(PhaseTiming aPhaseTiming, PhaseTiming bPhaseTiming, PhaseTiming cPhaseTiming)
        {
            int an;
            int bn;
            int cn;

            if (!aPhaseTiming.IsValid)
                return "None";

            if (!bPhaseTiming.IsValid)
                return "None";

            if (!cPhaseTiming.IsValid)
                return "None";

            an = aPhaseTiming.TimeCleared.Index;
            bn = bPhaseTiming.TimeCleared.Index;
            cn = cPhaseTiming.TimeCleared.Index;

            if (an >= bn && an >= cn)
                return "AN";

            if (bn >= an && bn >= cn)
                return "BN";

            return "CN";
        }

        private BreakerOperationType GetBreakerOperationType(double breakerTiming, double breakerSpeed)
        {
            double diff;

            if (double.IsNaN(breakerTiming))
                return BreakerOperationType.Indeterminate;

            if (double.IsNaN(breakerSpeed))
                return BreakerOperationType.Indeterminate;

            diff = breakerTiming - breakerSpeed;

            if (diff >= m_breakerSettings.LateBreakerThreshold)
                return BreakerOperationType.Late;

            return BreakerOperationType.Normal;
        }

        private double NotNaN(double value, double defaultValue = 0.0D)
        {
            return !double.IsNaN(value) ? value : defaultValue;
        }

        private double ConvertBreakerSpeed(object breakerSpeed)
        {
            if (Convert.IsDBNull(breakerSpeed))
                return double.NaN;

            return Convert.ToDouble(breakerSpeed);
        }

        private EventKey CreateEventKey(DataGroup dataGroup)
        {
            return Tuple.Create(dataGroup.Line.ID, dataGroup.StartTime, dataGroup.EndTime);
        }

        private EventKey CreateEventKey(MeterData.EventRow evt)
        {
            return Tuple.Create(evt.LineID, evt.StartTime, evt.EndTime);
        }

        private void LoadBreakerOperationTypes(DbAdapterContainer dbAdapterContainer)
        {
            if ((object)s_breakerOperationTypeLookup == null)
            {
                lock (BreakerOperationTypeLookupLock)
                {
                    if ((object)s_breakerOperationTypeLookup == null)
                        s_breakerOperationTypeLookup = GetBreakerOperationTypeLookup(dbAdapterContainer);
                }
            }
        }

        private Dictionary<BreakerOperationType, int> GetBreakerOperationTypeLookup(DbAdapterContainer dbAdapterContainer)
        {
            MeterData.BreakerOperationTypeDataTable breakerOperationTypeTable = new MeterData.BreakerOperationTypeDataTable();
            BreakerOperationType breakerOperationType = default(BreakerOperationType);

            foreach (BreakerOperationType operationType in Enum.GetValues(typeof(BreakerOperationType)))
                breakerOperationTypeTable.AddBreakerOperationTypeRow(operationType.ToString(), operationType.ToString());

            BulkLoader bulkLoader = new BulkLoader();

            bulkLoader.Connection = dbAdapterContainer.Connection;
            bulkLoader.CommandTimeout = dbAdapterContainer.CommandTimeout;

            bulkLoader.MergeTableFormat = "MERGE INTO {0} AS Target " +
                                          "USING {1} AS Source " +
                                          "ON Source.Name = Target.Name " +
                                          "WHEN NOT MATCHED THEN " +
                                          "    INSERT (Name, Description) " +
                                          "    VALUES (Source.Name, Source.Description);";

            bulkLoader.Load(breakerOperationTypeTable);

            dbAdapterContainer.GetAdapter<BreakerOperationTypeTableAdapter>().Fill(breakerOperationTypeTable);

            return breakerOperationTypeTable
                .Where(row => Enum.TryParse(row.Name, out breakerOperationType)).Select(row => Tuple.Create(breakerOperationType, row.ID))
                .ToDictionary(tuple => tuple.Item1, tuple => tuple.Item2);
        }

        #endregion

        #region [ Static ]

        // Static Fields
        private static readonly object BreakerOperationTypeLookupLock = new object();
        private static Dictionary<BreakerOperationType, int> s_breakerOperationTypeLookup;

        #endregion
    }
}
