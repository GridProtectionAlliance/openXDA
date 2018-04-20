//******************************************************************************************************
//  DoubleEndedFaultOperation.cs - Gbtc
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
//  04/07/2015 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using FaultAlgorithms;
using FaultData.Configuration;
using FaultData.DataAnalysis;
using FaultData.DataResources;
using FaultData.DataSets;
using GSF;
using GSF.Configuration;
using GSF.Data;
using GSF.Data.Model;
using log4net;
using openXDA.Model;
using CycleData = FaultAlgorithms.CycleData;
using Fault = FaultData.DataAnalysis.Fault;
using FaultGroup = openXDA.Model.FaultGroup;

namespace FaultData.DataOperations
{
    public class DoubleEndedFaultOperation : DataOperationBase<MeterDataSet>
    {
        #region [ Members ]

        // Nested Types
        private class FaultTimeline
        {
            public Meter Meter;
            public List<FaultSummary> Faults;
        }

        private class Mapping
        {
            public MappingNode Left;
            public MappingNode Right;

            public Mapping(FaultSummary left, FaultSummary right)
            {
                Left = new MappingNode(left);
                Right = new MappingNode(right);
            }
        }

        private class MappingNode
        {
            #region [ Members ]

            // Fields
            public FaultSummary Fault;
            public VICycleDataGroup CycleDataGroup;
            public Fault.Curve DistanceCurve;
            public Fault.Curve AngleCurve;

            public FaultType FaultType;
            public int StartSample;
            public int EndSample;

            #endregion

            #region [ Constructors ]

            public MappingNode(FaultSummary fault)
            {
                Fault = fault;
                DistanceCurve = new Fault.Curve();
                AngleCurve = new Fault.Curve();

                if (!Enum.TryParse(fault.FaultType, out FaultType))
                    FaultType = FaultType.None;
            }

            #endregion

            #region [ Methods ]

            public void Initialize(AdoDataConnection connection, VICycleDataGroup viCycleDataGroup, double systemFrequency)
            {
                int samplesPerCycle = Transform.CalculateSamplesPerCycle(viCycleDataGroup.VA.RMS, systemFrequency);

                TableOperations<FaultSegment> faultSegmentTable = new TableOperations<FaultSegment>(connection);

                RecordRestriction recordRestriction =
                    new RecordRestriction("EventID = {0}", Fault.EventID) &
                    new RecordRestriction("StartTime = {0}", ToDateTime2(connection, Fault.Inception)) &
                    new RecordRestriction("(SELECT Name FROM SegmentType WHERE ID = SegmentTypeID) = 'Fault'");

                FaultSegment faultSegment = faultSegmentTable.QueryRecord(recordRestriction);

                if ((object)faultSegment == null)
                    throw new InvalidOperationException($"Unable to find fault segment that matches fault summary for event {Fault.EventID}.");

                StartSample = faultSegment.StartSample;
                EndSample = faultSegment.EndSample - samplesPerCycle + 1;
                CycleDataGroup = Rotate(viCycleDataGroup.ToSubSet(StartSample, EndSample));

                DistanceCurve.StartIndex = StartSample;
                AngleCurve.StartIndex = StartSample;
            }

            private VICycleDataGroup Rotate(VICycleDataGroup viCycleDataGroup)
            {
                DataSeries referenceSeries = viCycleDataGroup.VA.Phase;

                DataGroup shiftedDataGroup = new DataGroup(new List<DataSeries>()
                {
                    viCycleDataGroup.VA.RMS,
                    Rotate(viCycleDataGroup.VA.Phase, referenceSeries),
                    viCycleDataGroup.VA.Peak,
                    viCycleDataGroup.VA.Error,

                    viCycleDataGroup.VB.RMS,
                    Rotate(viCycleDataGroup.VB.Phase, referenceSeries),
                    viCycleDataGroup.VB.Peak,
                    viCycleDataGroup.VB.Error,

                    viCycleDataGroup.VC.RMS,
                    Rotate(viCycleDataGroup.VC.Phase, referenceSeries),
                    viCycleDataGroup.VC.Peak,
                    viCycleDataGroup.VC.Error,

                    viCycleDataGroup.IA.RMS,
                    Rotate(viCycleDataGroup.IA.Phase, referenceSeries),
                    viCycleDataGroup.IA.Peak,
                    viCycleDataGroup.IA.Error,

                    viCycleDataGroup.IB.RMS,
                    Rotate(viCycleDataGroup.IB.Phase, referenceSeries),
                    viCycleDataGroup.IB.Peak,
                    viCycleDataGroup.IB.Error,

                    viCycleDataGroup.IC.RMS,
                    Rotate(viCycleDataGroup.IC.Phase, referenceSeries),
                    viCycleDataGroup.IC.Peak,
                    viCycleDataGroup.IC.Error,

                    viCycleDataGroup.IR.RMS,
                    Rotate(viCycleDataGroup.IR.Phase, referenceSeries),
                    viCycleDataGroup.IR.Peak,
                    viCycleDataGroup.IR.Error
                });

                return new VICycleDataGroup(shiftedDataGroup);
            }

            private DataSeries Rotate(DataSeries phaseSeries, DataSeries referenceSeries)
            {
                Func<DataPoint, DataPoint, DataPoint> shiftFunc;
                DataSeries shiftedPhaseSeries;

                shiftFunc = (phase, reference) => new DataPoint()
                {
                    Time = phase.Time,
                    Value = phase.Value - reference.Value
                };

                shiftedPhaseSeries = new DataSeries();
                shiftedPhaseSeries.SeriesInfo = phaseSeries.SeriesInfo;

                shiftedPhaseSeries.DataPoints = phaseSeries.DataPoints
                    .Zip(referenceSeries.DataPoints, shiftFunc)
                    .ToList();

                return shiftedPhaseSeries;
            }

            private IDbDataParameter ToDateTime2(AdoDataConnection connection, DateTime dateTime)
            {
                using (IDbCommand command = connection.Connection.CreateCommand())
                {
                    IDbDataParameter parameter = command.CreateParameter();
                    parameter.DbType = DbType.DateTime2;
                    parameter.Value = dateTime;
                    return parameter;
                }
            }

            #endregion
        }

        // Fields
        private double m_timeTolerance;
        private double m_systemFrequency;
        private FaultLocationSettings m_faultLocationSettings;

        #endregion

        #region [ Constructors ]

        public DoubleEndedFaultOperation()
        {
            m_faultLocationSettings = new FaultLocationSettings();
        }

        #endregion

        #region [ Properties ]

        [Setting]
        public double TimeTolerance
        {
            get
            {
                return m_timeTolerance;
            }
            set
            {
                m_timeTolerance = value;
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

        [Category]
        [SettingName("FaultLocation")]
        public FaultLocationSettings FaultLocationSettings
        {
            get
            {
                return m_faultLocationSettings;
            }
        }

        #endregion

        #region [ Methods ]

        public override void Execute(MeterDataSet meterDataSet)
        {
            // Get a time range for querying each system event that contains events in this meter data set
            SystemEventResource systemEventResource = meterDataSet.GetResource<SystemEventResource>();
            List<SystemEventResource.SystemEvent> systemEvents = systemEventResource.SystemEvents;

            if (systemEvents.Count == 0)
                return;

            using (AdoDataConnection connection = meterDataSet.CreateDbConnection())
            {
                TableOperations<openXDA.Model.Line> lineTable = new TableOperations<openXDA.Model.Line>(connection);
                TableOperations<MeterLocationLine> meterLocationLineTable = new TableOperations<MeterLocationLine>(connection);
                TableOperations<LineImpedance> lineImpedanceTable = new TableOperations<LineImpedance>(connection);
                TableOperations<Event> eventTable = new TableOperations<Event>(connection);
                TableOperations<DoubleEndedFaultDistance> doubleEndedFaultDistanceTable = new TableOperations<DoubleEndedFaultDistance>(connection);
                TableOperations<FaultCurve> faultCurveTable = new TableOperations<FaultCurve>(connection);

                List<MappingNode> processedMappingNodes = new List<MappingNode>();

                foreach (SystemEventResource.SystemEvent systemEvent in systemEvents)
                {
                    // Get the full collection of events from the database that comprise the system event that overlaps this time range
                    List<Event> dbSystemEvent = eventTable.GetSystemEvent(systemEvent.StartTime, systemEvent.EndTime, m_timeTolerance);

                    foreach (IGrouping<int, Event> lineGrouping in dbSystemEvent.GroupBy(evt => evt.LineID))
                    {
                        // Make sure this line connects two known meter locations
                        int meterLocationCount = meterLocationLineTable.QueryRecordCountWhere("LineID = {0}", lineGrouping.Key);

                        if (meterLocationCount != 2)
                            continue;

                        // Determine the length of the line
                        double lineLength = lineTable
                            .QueryRecordsWhere("ID = {0}", lineGrouping.Key)
                            .Select(line => line.Length)
                            .DefaultIfEmpty(double.NaN)
                            .First();

                        if (double.IsNaN(lineLength))
                            continue;

                        // Determine the nominal impedance of the line
                        ComplexNumber nominalImpedance = lineImpedanceTable
                            .QueryRecordsWhere("LineID = {0}", lineGrouping.Key)
                            .Select(lineImpedance => new ComplexNumber(lineImpedance.R1, lineImpedance.X1))
                            .FirstOrDefault();

                        if (!nominalImpedance.AllAssigned)
                            continue;

                        int leftEventID = 0;
                        int rightEventID = 0;
                        VICycleDataGroup leftCycleDataGroup = null;
                        VICycleDataGroup rightCycleDataGroup = null;

                        // Attempt to match faults during this system event that occurred
                        // on one end of the line with faults that occurred during this
                        // system even on the other end of the line
                        List<Mapping> mappings = GetMappings(connection, lineGrouping);

                        foreach (Mapping mapping in mappings)
                        {
                            if (mapping.Left.FaultType == FaultType.None || mapping.Right.FaultType == FaultType.None)
                                continue;

                            // Get the cycle data for each of the two mapped faults
                            if (mapping.Left.Fault.EventID != leftEventID)
                            {
                                leftEventID = mapping.Left.Fault.EventID;
                                leftCycleDataGroup = GetCycleData(connection, leftEventID);
                            }

                            if (mapping.Right.Fault.EventID != rightEventID)
                            {
                                rightEventID = mapping.Right.Fault.EventID;
                                rightCycleDataGroup = GetCycleData(connection, rightEventID);
                            }

                            if ((object)leftCycleDataGroup == null || (object)rightCycleDataGroup == null)
                                continue;

                            // Make sure double-ended distance has not already been calculated and entered into the database
                            RecordRestriction recordRestriction =
                                new RecordRestriction("LocalFaultSummaryID = {0}", mapping.Left.Fault.ID) |
                                new RecordRestriction("RemoteFaultSummaryID = {0}", mapping.Left.Fault.ID) |
                                new RecordRestriction("LocalFaultSummaryID = {0}", mapping.Right.Fault.ID) |
                                new RecordRestriction("RemoteFaultSummaryID = {0}", mapping.Right.Fault.ID);

                            if (doubleEndedFaultDistanceTable.QueryRecordCount(recordRestriction) > 0)
                                continue;

                            // Initialize the mappings with additional data needed for double-ended fault location
                            mapping.Left.Initialize(connection, leftCycleDataGroup, m_systemFrequency);
                            mapping.Right.Initialize(connection, rightCycleDataGroup, m_systemFrequency);

                            // Execute the double-ended fault location algorithm
                            ExecuteFaultLocationAlgorithm(lineLength, nominalImpedance, mapping.Left, mapping.Right);
                            ExecuteFaultLocationAlgorithm(lineLength, nominalImpedance, mapping.Right, mapping.Left);

                            try
                            {
                                // Create rows in the DoubleEndedFaultDistance table
                                DoubleEndedFaultDistance leftDistance = CreateDoubleEndedFaultDistance(lineLength, mapping.Left, mapping.Right);
                                DoubleEndedFaultDistance rightDistance = CreateDoubleEndedFaultDistance(lineLength, mapping.Right, mapping.Left);

                                doubleEndedFaultDistanceTable.AddNewRecord(leftDistance);
                                doubleEndedFaultDistanceTable.AddNewRecord(rightDistance);

                                // Add these nodes to the collection of processed mapping nodes
                                processedMappingNodes.Add(mapping.Left);
                                processedMappingNodes.Add(mapping.Right);
                            }
                            catch (Exception ex)
                            {
                                // Ignore errors regarding unique key constraints
                                // which can occur as a result of a race condition
                                bool isUniqueViolation = ExceptionHandler.IsUniqueViolation(ex);

                                if (!isUniqueViolation)
                                    throw;
                            }
                        }
                    }

                    // Create a row in the FaultCurve table for every event that now has double-ended fault distance curves
                    foreach (IGrouping<int, MappingNode> grouping in processedMappingNodes.GroupBy(node => node.Fault.EventID))
                    {
                        FaultCurve faultCurve = CreateFaultCurve(connection, grouping);
                        faultCurveTable.AddNewRecord(faultCurve);
                    }
                }
            }
        }

        private List<Mapping> GetMappings(AdoDataConnection connection, IGrouping<int, Event> lineGrouping)
        {
            TableOperations<Meter> meterTable = new TableOperations<Meter>(connection);
            TableOperations<FaultGroup> faultGroupTable = new TableOperations<FaultGroup>(connection);
            TableOperations<FaultSummary> faultSummaryTable = new TableOperations<FaultSummary>(connection);

            Func<FaultSummary, bool> filter = fault =>
            {
                if (!Convert.ToBoolean(fault.IsSelectedAlgorithm))
                    return false;

                List<FaultGroup> faultGroups = faultGroupTable
                    .QueryRecordsWhere("EventID = {0}", fault.EventID)
                    .ToList();

                foreach (FaultGroup faultGroup in faultGroups)
                {
                    bool? faultDetectionResult = faultGroup.FaultDetectionLogicResult;

                    // Fault validation (based on line length) doesn't apply because the fault may have been
                    // invalidated due to a high impedance so we only check whether it has been suppressed
                    bool faultValidationResult = !fault.IsSuppressed;

                    if (faultDetectionResult == false || (m_faultLocationSettings.UseDefaultFaultDetectionLogic && !faultValidationResult))
                        return false;
                }

                return true;
            };

            List<FaultTimeline> meterGroupings = lineGrouping
                .GroupBy(evt => evt.MeterID)
                .Select(meterGrouping => new FaultTimeline()
                {
                    Meter = meterTable.QueryRecordWhere("ID = {0}", meterGrouping.Key),
                    Faults = meterGrouping
                        .SelectMany(evt => faultSummaryTable.QueryRecordsWhere("EventID = {0}", evt.ID))
                        .Where(filter)
                        .OrderBy(fault => fault.Inception)
                        .ToList()
                })
                .Where(meterGrouping => meterGrouping.Faults.Any())
                .ToList();

            return meterGroupings
                .SelectMany(meterGrouping1 => meterGroupings.Select(meterGrouping2 => new
                {
                    Left = meterGrouping1,
                    Right = meterGrouping2
                }))
                .Where(mapping => mapping.Left.Meter.MeterLocationID < mapping.Right.Meter.MeterLocationID)
                .Where(mapping => mapping.Left.Faults.Count == mapping.Right.Faults.Count)
                .SelectMany(mapping => mapping.Left.Faults.Zip(mapping.Right.Faults, (left, right) => new Mapping(left, right)))
                .ToList();
        }

        private FaultCurve CreateFaultCurve(AdoDataConnection connection, IGrouping<int, MappingNode> grouping)
        {
            VICycleDataGroup viCycleDataGroup = GetCycleData(connection, grouping.Key);
            DataGroup faultCurveGroup = new DataGroup();

            faultCurveGroup.Add(viCycleDataGroup.VA.RMS.Multiply(double.NaN));
            faultCurveGroup.Add(faultCurveGroup[0].Copy());

            foreach (MappingNode node in grouping)
            {
                for (int i = node.StartSample; node.DistanceCurve.HasData(i); i++)
                {
                    faultCurveGroup[0][i].Value = node.DistanceCurve[i].Value;
                    faultCurveGroup[1][i].Value = node.AngleCurve[i].Value;
                }
            }

            return new FaultCurve()
            {
                EventID = grouping.Key,
                Algorithm = "DoubleEnded",
                Data = faultCurveGroup.ToData()
            };
        }

        private DoubleEndedFaultDistance CreateDoubleEndedFaultDistance(double lineLength, MappingNode local, MappingNode remote)
        {
            double distance = local.DistanceCurve[local.Fault.CalculationCycle].Value;

            return new DoubleEndedFaultDistance()
            {
                LocalFaultSummaryID = local.Fault.ID,
                RemoteFaultSummaryID = remote.Fault.ID,
                Distance = distance,
                Angle = local.AngleCurve[local.Fault.CalculationCycle].Value,
                IsValid = IsValid(distance, lineLength)
            };
        }

        private void ExecuteFaultLocationAlgorithm(double lineLength, ComplexNumber nominalImpedance, MappingNode local, MappingNode remote)
        {
            FaultLocationDataSet faultLocationDataSet;
            CycleData remoteCycle;
            ComplexNumber[] curve;

            faultLocationDataSet = new FaultLocationDataSet();
            faultLocationDataSet.FaultType = local.FaultType;
            faultLocationDataSet.LineDistance = lineLength;
            faultLocationDataSet.Z1 = nominalImpedance;
            local.CycleDataGroup.PushDataTo(faultLocationDataSet.Cycles);

            remoteCycle = GetCycleAt(remote.CycleDataGroup, remote.Fault.CalculationCycle - remote.StartSample);

            curve = FaultLocationAlgorithms.DoubleEnded(faultLocationDataSet, remoteCycle, string.Empty);
            ExtractDistanceVectors(local, curve);
        }

        private void ExtractDistanceVectors(MappingNode local, ComplexNumber[] curve)
        {
            List<DataPoint> vaRMS = local.CycleDataGroup.VA.RMS.DataPoints;
            int count = Math.Min(vaRMS.Count, curve.Length);

            for (int i = 0; i < count; i++)
            {
                local.DistanceCurve.Series.DataPoints.Add(new DataPoint()
                {
                    Time = vaRMS[i].Time,
                    Value = curve[i].Magnitude
                });

                local.AngleCurve.Series.DataPoints.Add(new DataPoint()
                {
                    Time = vaRMS[i].Time,
                    Value = curve[i].Angle
                });
            }
        }

        private VICycleDataGroup GetCycleData(AdoDataConnection connection, int eventID)
        {
            TableOperations<Event> eventTable = new TableOperations<Event>(connection);
            Event evt = eventTable.QueryRecordWhere("ID = {0}", eventID);

            if ((object)evt == null)
                return null;

            byte[] timeDomainData = connection.ExecuteScalar<byte[]>("SELECT TimeDomainData FROM EventData WHERE ID = {0}", evt.EventDataID);

            if ((object)timeDomainData == null)
                return null;

            TableOperations<Meter> meterTable = new TableOperations<Meter>(connection);
            Meter meter = meterTable.QueryRecordWhere("ID = {0}", evt.MeterID);

            if ((object)meter == null)
                return null;

            meter.ConnectionFactory = () => new AdoDataConnection(connection.Connection, connection.AdapterType, false);

            DataGroup dataGroup = new DataGroup();
            dataGroup.FromData(meter, timeDomainData);
            return Transform.ToVICycleDataGroup(new VIDataGroup(dataGroup), SystemFrequency);
        }

        private CycleData GetCycleAt(VICycleDataGroup viCycleDataGroup, int index)
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
                cycles[i].RMS = cycleDataGroups[i].RMS[index].Value;
                cycles[i].Phase = cycleDataGroups[i].Phase[index].Value;
                cycles[i].Peak = cycleDataGroups[i].Peak[index].Value;
                cycles[i].Error = cycleDataGroups[i].Error[index].Value;
            }

            return cycle;
        }

        private bool IsValid(double faultDistance, double lineLength)
        {
            double maxDistance = m_faultLocationSettings.MaxFaultDistanceMultiplier * lineLength;
            double minDistance = m_faultLocationSettings.MinFaultDistanceMultiplier * lineLength;
            return faultDistance >= minDistance && faultDistance <= maxDistance;
        }

        #endregion

        #region [ Static ]

        // Static Fields
        private static readonly ILog Log = LogManager.GetLogger(typeof(DoubleEndedFaultOperation));

        #endregion
    }
}
