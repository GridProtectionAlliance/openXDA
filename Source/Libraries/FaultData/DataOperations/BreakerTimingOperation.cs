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
using GSF.Collections;
using GSF.Configuration;
using GSF.Data;
using log4net;
using static FaultData.Database.MeterData;
using EventKey = System.Tuple<int, System.DateTime, System.DateTime>;
using GSF.Web.Model;

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
            private double m_timing;
            private double m_speed;
            private bool m_statusChatter;

            #endregion

            #region [ Constructors ]

            public BreakerTiming(XValue timeEnergized, DataSeries statusChannel, double systemFrequency, double speed, double minWaitBeforeReclose)
            {
                m_timeEnergized = timeEnergized;

                if ((object)statusChannel != null)
                    m_timeCleared = FindStatusBitSet(statusChannel, timeEnergized.Index);

                m_timing = double.NaN;

                if ((object)m_timeCleared != null)
                {
                    m_timing = (m_timeCleared.Time - m_timeEnergized.Time).TotalSeconds * systemFrequency;
                    m_statusChatter = FindStatusChatter(statusChannel, m_timeCleared.Index, systemFrequency, minWaitBeforeReclose);
                }

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

            public bool StatusChatter
            {
                get
                {
                    return m_statusChatter;
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

            private bool FindStatusChatter(DataSeries statusSeries, int startIndex, double systemFrequency, double minWaitBeforeReclose)
            {
                int samples = (int)Math.Round((minWaitBeforeReclose / systemFrequency) * statusSeries.SampleRate);

                for (int i = 0; i < samples; i++)
                {
                    int index = startIndex + i;

                    if (index >= statusSeries.DataPoints.Count)
                        break;

                    if (statusSeries[index - 1].Value != 0.0D && statusSeries[index].Value == 0.0D)
                        return true;
                }

                return false;
            }

            #endregion
        }

        private class PhaseTiming
        {
            #region [ Members ]

            // Fields
            private DataSeries m_waveform;
            private CycleDataGroup m_cycleDataGroup;
            private BreakerSettings m_breakerSettings;
            private XValue m_timeCleared;
            private double m_timing;
            private double m_systemFrequency;
            private bool m_dcOffsetDetected;

            #endregion

            #region [ Constructors ]

            public PhaseTiming(DataSeries waveform, CycleDataGroup cycleDataGroup, BreakerTiming breakerTiming, BreakerSettings breakerSettings, double systemFrequency)
            {
                m_waveform = waveform;
                m_cycleDataGroup = cycleDataGroup;
                m_breakerSettings = breakerSettings;
                m_systemFrequency = systemFrequency;
                m_timeCleared = FindBreakerOpen(breakerTiming.TimeEnergized.Index);

                if ((object)m_timeCleared != null)
                    m_timing = (m_timeCleared.Time - breakerTiming.TimeEnergized.Time).TotalSeconds * systemFrequency;
                else
                    m_timing = double.NaN;
                m_dcOffsetDetected = false;
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

            public bool DcOffsetDetected
            {
                get
                {
                    return m_dcOffsetDetected;
                }
            }

            #endregion

            #region [ Methods ]

            private XValue FindBreakerOpen(int startIndex)
            {
                int cycleIndex;
                int sampleCleared;
                int samplesPerCycle;
                int slidingWindowSize;
                DataSeries slidingWindow;

                if ((object)m_waveform == null || (object)m_cycleDataGroup == null)
                    return null;

                samplesPerCycle = Transform.CalculateSamplesPerCycle(m_waveform, m_systemFrequency);
                slidingWindowSize = (int)Math.Round(m_breakerSettings.DCOffsetWindowSize * samplesPerCycle);

                for (cycleIndex = startIndex; cycleIndex < m_cycleDataGroup.Peak.DataPoints.Count; cycleIndex++)
                {
                    // If the value is below the open breaker threshold,
                    // exit the loop so we can find a more accurate clearing time
                    if (m_cycleDataGroup.Peak[cycleIndex].Value <= m_breakerSettings.OpenBreakerThreshold)
                        break;

                    // If we detect DC offset (full cycle of data with no zero crossing),
                    // simply return the zero crossing as the clearing time
                    if (m_breakerSettings.ApplyDCOffsetLogic)
                    {
                        slidingWindow = m_waveform.ToSubSeries(cycleIndex, cycleIndex + slidingWindowSize - 1);

                        if (slidingWindow.Minimum >= 0 || slidingWindow.Maximum <= 0)
                        {
                            m_dcOffsetDetected = true;
                            return new XValue(cycleIndex, m_waveform[cycleIndex].Time);
                        }
                    }
                }

                // Throw out phase timing results if the
                // current channels are filled with noise
                if (cycleIndex - startIndex <= m_breakerSettings.MinCyclesBeforeOpen)
                    return null;

                if (cycleIndex == m_cycleDataGroup.Peak.DataPoints.Count)
                    return null;

                // Refine the search for the clearing time
                // by analyzing the instantaneous waveform
                sampleCleared = FindIndexCleared(m_waveform, cycleIndex);

                return new XValue(sampleCleared, m_waveform[sampleCleared].Time);
            }

            private int FindIndexCleared(DataSeries waveform, int cycleIndex)
            {
                int samplesPerCycle = Transform.CalculateSamplesPerCycle(waveform, m_systemFrequency);
                int startIndex = Math.Max(0, cycleIndex - samplesPerCycle);
                int endIndex = startIndex;
                int postclearIndex = Math.Min(endIndex + samplesPerCycle, waveform.DataPoints.Count - 1);

                double largestPostclearPeak;
                double largestPreclearPeak;

                double previousValue;
                double value;
                double nextValue;

                // If cycleIndex is zero, assume that the breaker
                // is open at the very start of the waveform
                if (cycleIndex == 0)
                    return 0;

                largestPostclearPeak = 0.0D;
                largestPreclearPeak = 0.0D;

                // Find the largest peak after the breaker is opened
                for (int i = postclearIndex; i > endIndex; i--)
                {
                    value = Math.Abs(waveform[i].Value);

                    if (value > largestPostclearPeak)
                        largestPostclearPeak = value;
                }

                // Find the largest peak of the last cycle before the breaker opened
                for (int i = startIndex; i <= endIndex; i++)
                {
                    value = Math.Abs(waveform[i].Value);

                    if (value > largestPreclearPeak)
                        largestPreclearPeak = value;
                }

                // Scanning backwards, find the first point where the value exceeds
                // a point 25% of the way from the postclear peak to the preclear peak
                for (int i = endIndex; i >= startIndex; i--)
                {
                    value = Math.Abs(waveform[i].Value);

                    if (value >= (largestPostclearPeak * 0.75 + largestPreclearPeak * 0.25))
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

        private HashSet<string> m_breakerCurrents;
        private DataContextLookup<string, Phase> m_phaseLookup;
        private BreakerOperationDataTable m_breakerOperationTable;
        private List<Tuple<EventKey, BreakerOperationRow>> m_breakerOperations;

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
            m_breakerOperationTable = new BreakerOperationDataTable();
            m_breakerOperations = new List<Tuple<EventKey, BreakerOperationRow>>();

            LoadBreakerOperationTypes(dbAdapterContainer);
        }

        public override void Prepare(DataContext dataContext)
        {
        }

        public override void Execute(MeterDataSet meterDataSet)
        {
            CycleDataResource cycleDataResource;

            DataGroup dataGroup;
            VIDataGroup viDataGroup;
            VICycleDataGroup viCycleDataGroup;

            cycleDataResource = CycleDataResource.GetResource(meterDataSet, m_dbAdapterContainer);

            m_breakerCurrents = new HashSet<string>(cycleDataResource.DataGroups
                .Where(dg => dg.Line.AssetKey.StartsWith("BR"))
                .SelectMany(dg => dg.DataSeries)
                .SelectMany(ds => ds.SeriesInfo.Channel.BreakerChannels)
                .Select(ch => ch.BreakerNumber));

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
            EventDataTable eventTable;
            Dictionary<EventKey, EventRow> eventLookup;
            EventRow eventRow;
            BulkLoader bulkLoader;

            eventTable = dbAdapterContainer.GetAdapter<EventTableAdapter>().GetDataByFileGroup(m_meterDataSet.FileGroup.ID);

            eventLookup = eventTable
                .Where(evt => evt.MeterID == m_meterDataSet.Meter.ID)
                .GroupBy(CreateEventKey)
                .ToDictionary(grouping => grouping.Key, grouping =>
                {
                    if (grouping.Count() > 1)
                        Log.Warn($"Found duplicate events for meter {m_meterDataSet.Meter.AssetKey}: {string.Join(", ", grouping.Select(evt => evt.ID))}");

                    return grouping.First();
                });

            foreach (Tuple<EventKey, BreakerOperationRow> breakerOperation in m_breakerOperations)
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

            // If the breaker currents are defined for breaker-and-a-half or ring bus configurations,
            // skip this data group so that timing is only applied to the breaker currents
            if (breakerNumbers.Count > 1 && breakerNumbers.Any(num => m_breakerCurrents.Contains(num)))
                return;

            foreach (string breakerNumber in breakerNumbers)
            {
                double breakerSpeed = ConvertBreakerSpeed(m_dbAdapterContainer.Connection.ExecuteScalar("SELECT BreakerSpeed FROM MaximoBreaker WHERE SUBSTRING(BreakerNum, PATINDEX('%[^0]%', BreakerNum + '.'), LEN(BreakerNum)) = @breakerNumber", DataExtensions.DefaultTimeoutDuration, breakerNumber.TrimStart('0')));

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
                    BreakerTiming breakerTiming = new BreakerTiming(breakerOperation, breakerStatusChannel, m_systemFrequency, breakerSpeed, m_breakerSettings.MinWaitBeforeReclose);

                    PhaseTiming aPhaseTiming = new PhaseTiming(viDataGroup.IA, viCycleDataGroup.IA, breakerTiming, m_breakerSettings, m_systemFrequency);
                    PhaseTiming bPhaseTiming = new PhaseTiming(viDataGroup.IB, viCycleDataGroup.IB, breakerTiming, m_breakerSettings, m_systemFrequency);
                    PhaseTiming cPhaseTiming = new PhaseTiming(viDataGroup.IC, viCycleDataGroup.IC, breakerTiming, m_breakerSettings, m_systemFrequency);

                    BreakerOperationRow breakerOperationRow = GetBreakerOperationRow(breakerNumber, breakerTiming, aPhaseTiming, bPhaseTiming, cPhaseTiming);

                    m_breakerOperations.Add(Tuple.Create(CreateEventKey(dataGroup), breakerOperationRow));
                }
            }
        }

        private BreakerOperationRow GetBreakerOperationRow(string breakerNumber, BreakerTiming breakerTiming, PhaseTiming aPhaseTiming, PhaseTiming bPhaseTiming, PhaseTiming cPhaseTiming)
        {
            double maxTiming = GetMaxTiming(breakerTiming, aPhaseTiming, bPhaseTiming, cPhaseTiming);
            string phase = GetLatestPhase(aPhaseTiming, bPhaseTiming, cPhaseTiming);
            BreakerOperationType type = GetBreakerOperationType(maxTiming, breakerTiming.Speed);

            BreakerOperationRow breakerOperationRow = m_breakerOperationTable.NewBreakerOperationRow();

            breakerOperationRow.PhaseID = m_phaseLookup.GetOrAdd(phase, name => new Phase() { Name = name, Description = name }).ID;
            breakerOperationRow.BreakerOperationTypeID = s_breakerOperationTypeLookup[type];
            breakerOperationRow.BreakerNumber = breakerNumber;
            breakerOperationRow.TripCoilEnergized = breakerTiming.TimeEnergized.Time;
            breakerOperationRow.StatusBitSet = breakerTiming.IsValid ? breakerTiming.TimeCleared.Time : breakerTiming.TimeEnergized.Time;
            breakerOperationRow.StatusBitChatter = breakerTiming.StatusChatter ? 1 : 0;
            breakerOperationRow.APhaseCleared = aPhaseTiming.IsValid ? aPhaseTiming.TimeCleared.Time : breakerTiming.TimeEnergized.Time;
            breakerOperationRow.BPhaseCleared = bPhaseTiming.IsValid ? bPhaseTiming.TimeCleared.Time : breakerTiming.TimeEnergized.Time;
            breakerOperationRow.CPhaseCleared = cPhaseTiming.IsValid ? cPhaseTiming.TimeCleared.Time : breakerTiming.TimeEnergized.Time;
            breakerOperationRow.BreakerTiming = NotNaN(maxTiming);
            breakerOperationRow.StatusTiming = NotNaN(breakerTiming.Timing);
            breakerOperationRow.APhaseBreakerTiming = NotNaN(aPhaseTiming.Timing);
            breakerOperationRow.BPhaseBreakerTiming = NotNaN(bPhaseTiming.Timing);
            breakerOperationRow.CPhaseBreakerTiming = NotNaN(cPhaseTiming.Timing);
            breakerOperationRow.BreakerSpeed = NotNaN(breakerTiming.Speed);
            breakerOperationRow.DcOffsetDetected = aPhaseTiming.DcOffsetDetected || bPhaseTiming.DcOffsetDetected || cPhaseTiming.DcOffsetDetected ? 1 : 0;

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

        private double GetMaxTiming(BreakerTiming breakerTiming, PhaseTiming aPhaseTiming, PhaseTiming bPhaseTiming, PhaseTiming cPhaseTiming)
        {
            if (!aPhaseTiming.IsValid)
                return breakerTiming.Timing;

            if (!bPhaseTiming.IsValid)
                return breakerTiming.Timing;

            if (!cPhaseTiming.IsValid)
                return breakerTiming.Timing;

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
            if (breakerSpeed == null || Convert.IsDBNull(breakerSpeed))
                return double.NaN;

            return Convert.ToDouble(breakerSpeed);
        }

        private EventKey CreateEventKey(DataGroup dataGroup)
        {
            return Tuple.Create(dataGroup.Line.ID, dataGroup.StartTime, dataGroup.EndTime);
        }

        private EventKey CreateEventKey(EventRow evt)
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
            BreakerOperationTypeDataTable breakerOperationTypeTable = new BreakerOperationTypeDataTable();
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

            foreach (IGrouping<string, BreakerOperationTypeRow> grouping in breakerOperationTypeTable.GroupBy(row => row.Name))
            {
                if (grouping.Count() > 1)
                    Log.Warn($"Found duplicate breaker operation type: {grouping.Key}");
            }

            return breakerOperationTypeTable
                .Where(row => Enum.TryParse(row.Name, out breakerOperationType))
                .Select(row => new { BreakerOperationType = breakerOperationType, row.ID })
                .ToList()
                .DistinctBy(obj => obj.BreakerOperationType)
                .ToDictionary(obj => obj.BreakerOperationType, obj => obj.ID);
        }

        #endregion

        #region [ Static ]

        // Static Fields
        private static readonly object BreakerOperationTypeLookupLock = new object();
        private static Dictionary<BreakerOperationType, int> s_breakerOperationTypeLookup;
        private static readonly ILog Log = LogManager.GetLogger(typeof(BreakerTimingOperation));

        #endregion
    }
}
