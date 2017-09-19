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
using System.Data;
using System.Linq;
using FaultData.Configuration;
using FaultData.DataAnalysis;
using FaultData.DataResources;
using FaultData.DataSets;
using GSF;
using GSF.Collections;
using GSF.Configuration;
using GSF.Data;
using GSF.Data.Model;
using log4net;
using openXDA.Model;

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
        private HashSet<string> m_breakerCurrents;

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

        public override void Execute(MeterDataSet meterDataSet)
        {
            CycleDataResource cycleDataResource = meterDataSet.GetResource<CycleDataResource>();

            using (AdoDataConnection connection = meterDataSet.CreateDbConnection())
            {
                TableOperations<BreakerChannel> breakerChannelTable = new TableOperations<BreakerChannel>(connection);

                List<int> channelsIDs = cycleDataResource.DataGroups
                    .Where(dataGroup => dataGroup.Line.AssetKey.StartsWith("BR"))
                    .SelectMany(dataGroup => dataGroup.DataSeries)
                    .Select(dataSeries => dataSeries.SeriesInfo.ChannelID)
                    .Distinct()
                    .ToList();

                // Look up the breaker numbers for the lines
                // that represent groupings of breaker current data
                m_breakerCurrents = new HashSet<string>(breakerChannelTable
                    .QueryRecordsWhere($"ChannelID IN ({string.Join(",", channelsIDs)})")
                    .Select(breakerChannel => breakerChannel.BreakerNumber));

                FileGroup fileGroup = meterDataSet.FileGroup;

                for (int i = 0; i < cycleDataResource.DataGroups.Count; i++)
                {
                    DataGroup dataGroup = cycleDataResource.DataGroups[i];
                    VIDataGroup viDataGroup = cycleDataResource.VIDataGroups[i];
                    VICycleDataGroup viCycleDataGroup = cycleDataResource.VICycleDataGroups[i];
                    IdentifyBreakerOperations(connection, fileGroup, dataGroup, viDataGroup, viCycleDataGroup);
                }
            }
        }

        private void IdentifyBreakerOperations(AdoDataConnection connection, FileGroup fileGroup, DataGroup dataGroup, VIDataGroup viDataGroup, VICycleDataGroup viCycleDataGroup)
        {
            TableOperations<Event> eventTable = new TableOperations<Event>(connection);
            TableOperations<BreakerChannel> breakerChannelTable = new TableOperations<BreakerChannel>(connection);
            TableOperations<BreakerOperation> breakerOperationTable = new TableOperations<BreakerOperation>(connection);

            List<int> channelIDs = dataGroup.DataSeries
                .Select(dataSeries => dataSeries.SeriesInfo.ChannelID)
                .Distinct()
                .ToList();

            List<string> breakerNumbers = breakerChannelTable
                .QueryRecordsWhere($"ChannelID IN ({string.Join(",", channelIDs)})")
                .Select(breakerChannel => breakerChannel.BreakerNumber)
                .Distinct()
                .ToList();

            // If the breaker currents are defined for breaker-and-a-half or ring bus configurations,
            // skip this data group so that timing is only applied to the breaker currents
            if (breakerNumbers.Count > 1 && breakerNumbers.Any(num => m_breakerCurrents.Contains(num)))
                return;

            Event evt = eventTable.GetEvent(fileGroup, dataGroup);

            foreach (string breakerNumber in breakerNumbers)
            {
                double breakerSpeed = connection.ExecuteScalar(double.NaN, "SELECT Speed FROM Breaker WHERE AssetKey = {0}", breakerNumber.TrimStart('0'));

                List<DataSeries> breakerDigitals = dataGroup.DataSeries
                    .Where(dataSeries => dataSeries.SeriesInfo.Channel.MeasurementType.Name == "Digital")
                    .Where(dataSeries => breakerChannelTable.QueryRecordCountWhere("BreakerNumber = {0}", breakerNumber) > 0)
                    .ToList();

                List<DataSeries> tripCoilEnergizedChannels = breakerDigitals
                    .Where(dataSeries => dataSeries.SeriesInfo.Channel.MeasurementCharacteristic.Name == "TCE")
                    .ToList();

                DataSeries breakerStatusChannel = breakerDigitals
                    .FirstOrDefault(dataSeries => dataSeries.SeriesInfo.Channel.MeasurementCharacteristic.Name == "BreakerStatus");

                List<XValue> tripCoilEnergizedTriggers = Range<XValue>.MergeAllOverlapping(tripCoilEnergizedChannels.SelectMany(FindTCERanges))
                    .Select(range => range.Start)
                    .ToList();

                foreach (XValue tripCoilEnergizedTrigger in tripCoilEnergizedTriggers)
                {
                    BreakerTiming breakerTiming = new BreakerTiming(tripCoilEnergizedTrigger, breakerStatusChannel, m_systemFrequency, breakerSpeed, m_breakerSettings.MinWaitBeforeReclose);

                    PhaseTiming aPhaseTiming = new PhaseTiming(viDataGroup.IA, viCycleDataGroup.IA, breakerTiming, m_breakerSettings, m_systemFrequency);
                    PhaseTiming bPhaseTiming = new PhaseTiming(viDataGroup.IB, viCycleDataGroup.IB, breakerTiming, m_breakerSettings, m_systemFrequency);
                    PhaseTiming cPhaseTiming = new PhaseTiming(viDataGroup.IC, viCycleDataGroup.IC, breakerTiming, m_breakerSettings, m_systemFrequency);

                    BreakerOperation breakerOperation = GetBreakerOperation(connection, evt.ID, breakerNumber, breakerTiming, aPhaseTiming, bPhaseTiming, cPhaseTiming);
                    breakerOperationTable.AddNewRecord(breakerOperation);
                }
            }
        }

        private BreakerOperation GetBreakerOperation(AdoDataConnection connection, int eventID, string breakerNumber, BreakerTiming breakerTiming, PhaseTiming aPhaseTiming, PhaseTiming bPhaseTiming, PhaseTiming cPhaseTiming)
        {
            TableOperations<Phase> phaseTable = new TableOperations<Phase>(connection);
            TableOperations<openXDA.Model.BreakerOperationType> breakerOperationTypeTable = new TableOperations<openXDA.Model.BreakerOperationType>(connection);

            double maxTiming = GetMaxTiming(breakerTiming, aPhaseTiming, bPhaseTiming, cPhaseTiming);
            string phase = GetLatestPhase(aPhaseTiming, bPhaseTiming, cPhaseTiming);
            BreakerOperationType type = GetBreakerOperationType(maxTiming, breakerTiming.Speed);

            return new BreakerOperation()
            {
                EventID = eventID,
                PhaseID = phaseTable.GetOrAdd(phase).ID,
                BreakerOperationTypeID = breakerOperationTypeTable.GetOrAdd(type.ToString()).ID,
                BreakerNumber = breakerNumber,
                TripCoilEnergized = breakerTiming.TimeEnergized.Time,
                StatusBitSet = breakerTiming.IsValid ? breakerTiming.TimeCleared.Time : breakerTiming.TimeEnergized.Time,
                StatusBitChatter = breakerTiming.StatusChatter,
                APhaseCleared = aPhaseTiming.IsValid ? aPhaseTiming.TimeCleared.Time : breakerTiming.TimeEnergized.Time,
                BPhaseCleared = bPhaseTiming.IsValid ? bPhaseTiming.TimeCleared.Time : breakerTiming.TimeEnergized.Time,
                CPhaseCleared = cPhaseTiming.IsValid ? cPhaseTiming.TimeCleared.Time : breakerTiming.TimeEnergized.Time,
                BreakerTiming = NotNaN(maxTiming),
                StatusTiming = NotNaN(breakerTiming.Timing),
                APhaseBreakerTiming = NotNaN(aPhaseTiming.Timing),
                BPhaseBreakerTiming = NotNaN(bPhaseTiming.Timing),
                CPhaseBreakerTiming = NotNaN(cPhaseTiming.Timing),
                DcOffsetDetected = (aPhaseTiming.DcOffsetDetected || bPhaseTiming.DcOffsetDetected || cPhaseTiming.DcOffsetDetected),
                BreakerSpeed = NotNaN(breakerTiming.Speed)
            };
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

        #endregion

        #region [ Static ]

        // Static Fields
        private static readonly ILog Log = LogManager.GetLogger(typeof(BreakerTimingOperation));

        #endregion
    }
}
