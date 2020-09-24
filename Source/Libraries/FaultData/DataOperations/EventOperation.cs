//******************************************************************************************************
//  EventOperation.cs - Gbtc
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
//  07/22/2014 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using FaultData.DataAnalysis;
using FaultData.DataResources;
using FaultData.DataSets;
using GSF.Collections;
using GSF.Data;
using GSF.Data.Model;
using log4net;
using openXDA.Model;
using DbDisturbance = openXDA.Model.Disturbance;
using Disturbance = FaultData.DataAnalysis.Disturbance;
using PQDPhase = GSF.PQDIF.Logical.Phase;

namespace FaultData.DataOperations
{
    public class EventOperation : DataOperationBase<MeterDataSet>
    {
        #region [ Members ]

        // Constants
        private const double Sqrt3 = 1.7320508075688772935274463415059D;

        #endregion

        #region [ Properties ]

        [Setting]
        public double MaxEventDuration
        {
            get => MaxEventDurationSpan.TotalSeconds;
            set => MaxEventDurationSpan = TimeSpan.FromSeconds(value);
        }

        [Setting]
        public double SystemFrequency { get; set; }

        [Setting]
        public string XDATimeZone
        {
            get => TimeZone.Id;
            set => TimeZone = TimeZoneInfo.FindSystemTimeZoneById(value);
        }

        private TimeSpan MaxEventDurationSpan { get; set; }
        private TimeZoneInfo TimeZone { get; set; }

        #endregion

        #region [ Methods ]

        public override void Execute(MeterDataSet meterDataSet)
        {
            FilterProcessedDataGroups(meterDataSet);

            DataGroupsResource dataGroupsResource = meterDataSet.GetResource<DataGroupsResource>();
            CycleDataResource cycleDataResource = meterDataSet.GetResource<CycleDataResource>();
            EventClassificationResource eventClassificationResource = meterDataSet.GetResource<EventClassificationResource>();

            using (AdoDataConnection connection = meterDataSet.CreateDbConnection())
            {
                List<DataGroup> dataGroups = new List<DataGroup>(cycleDataResource.DataGroups);
                dataGroups.AddRange(dataGroupsResource.DataGroups.Where(dataGroup => dataGroup.DataSeries.Count == 0));

                List<Event> events = GetEvents(connection, meterDataSet, dataGroups, cycleDataResource.VICycleDataGroups, eventClassificationResource.Classifications);
                LoadEvents(connection, events, meterDataSet);
            }
        }

        private void FilterProcessedDataGroups(MeterDataSet meterDataSet)
        {
            DataGroupsResource dataGroupsResource = meterDataSet.GetResource<DataGroupsResource>();
            List<DataGroup> dataGroups = dataGroupsResource.DataGroups;

            if (!dataGroups.Any(dataGroup => dataGroup.Classification == DataClassification.Event))
                return;

            using (AdoDataConnection connection = meterDataSet.CreateDbConnection())
            {
                TableOperations<Event> eventTable = new TableOperations<Event>(connection);

                for (int i = dataGroups.Count - 1; i >= 0; i--)
                {
                    DataGroup dataGroup = dataGroups[i];

                    if (dataGroup.Classification != DataClassification.Event)
                        continue;

                    Line line = dataGroup.Line ?? meterDataSet.Meter.MeterLocation.MeterLocationLines.Single().Line;
                    IDbDataParameter startTime2 = ToDateTime2(connection, dataGroup.StartTime);
                    IDbDataParameter endTime2 = ToDateTime2(connection, dataGroup.EndTime);

                    const string Filter =
                        "StartTime = {0} AND " +
                        "EndTime = {1} AND " +
                        "Samples = {2} AND " +
                        "MeterID = {3} AND " +
                        "LineID = {4}";

                    int count = eventTable.QueryRecordCountWhere(Filter, startTime2, endTime2, dataGroup.Samples, meterDataSet.Meter.ID, line.ID);

                    if (count > 0)
                        dataGroups.RemoveAt(i);
                }
            }
        }

        private List<Event> GetEvents(AdoDataConnection connection, MeterDataSet meterDataSet, List<DataGroup> dataGroups, List<VICycleDataGroup> viCycleDataGroups, Dictionary<DataGroup, EventClassification> eventClassifications)
        {
            int count = dataGroups
                .Where(dataGroup => dataGroup.Classification != DataClassification.Trend)
                .Where(dataGroup => dataGroup.Classification != DataClassification.Unknown)
                .Where(dataGroup => eventClassifications.ContainsKey(dataGroup))
                .Count();

            if (count == 0)
            {
                Log.Info($"No events found for file '{meterDataSet.FilePath}'.");
                return new List<Event>();
            }

            Log.Info(string.Format("Processing {0} events...", count));

            List<Event> events = new List<Event>(count);

            TableOperations<Event> eventTable = new TableOperations<Event>(connection);
            TableOperations<EventType> eventTypeTable = new TableOperations<EventType>(connection);
            TableOperations<EventData> eventDataTable = new TableOperations<EventData>(connection);

            for (int i = 0; i < dataGroups.Count; i++)
            {
                DataGroup dataGroup = dataGroups[i];
                TimeSpan duration = TimeSpan.FromSeconds(dataGroup.Duration);

                if (MaxEventDurationSpan > TimeSpan.Zero && duration > MaxEventDurationSpan)
                    continue;

                if (dataGroup.Classification == DataClassification.Trend)
                    continue;

                if (dataGroup.Classification == DataClassification.Unknown)
                    continue;

                if (!eventClassifications.TryGetValue(dataGroup, out EventClassification eventClassification))
                    continue;

                if ((object)dataGroup.Line == null && meterDataSet.Meter.MeterLocation.MeterLocationLines.Count != 1)
                    continue;

                Line line = dataGroup.Line ?? meterDataSet.Meter.MeterLocation.MeterLocationLines.Single().Line;
                IDbDataParameter startTime2 = ToDateTime2(connection, dataGroup.StartTime);
                IDbDataParameter endTime2 = ToDateTime2(connection, dataGroup.EndTime);

                if (eventTable.QueryRecordCountWhere("StartTime = {0} AND EndTime = {1} AND Samples = {2} AND MeterID = {3} AND LineID = {4}", startTime2, endTime2, dataGroup.Samples, meterDataSet.Meter.ID, line.ID) > 0)
                    continue;

                TableOperations<MaintenanceWindow> maintenanceWindowTable = new TableOperations<MaintenanceWindow>(connection);
                int maintenanceWindowCount = maintenanceWindowTable.QueryRecordCountWhere("MeterID = {0} AND (StartTime IS NULL OR StartTime <= {1}) AND (EndTime IS NULL OR EndTime >= {1})", meterDataSet.Meter.ID, DateTime.UtcNow);
                string eventTypeName = (maintenanceWindowCount == 0) ? eventClassification.ToString() : "Test";
                EventType eventType = eventTypeTable.GetOrAdd(eventTypeName);

                Event evt = new Event()
                {
                    FileGroupID = meterDataSet.FileGroup.ID,
                    MeterID = meterDataSet.Meter.ID,
                    LineID = line.ID,
                    EventTypeID = eventType.ID,
                    EventDataID = null,
                    Name = string.Empty,
                    StartTime = dataGroup.StartTime,
                    EndTime = dataGroup.EndTime,
                    Samples = dataGroup.Samples,
                    TimeZoneOffset = (int)TimeZone.GetUtcOffset(dataGroup.StartTime).TotalMinutes,
                    SamplesPerSecond = 0,
                    SamplesPerCycle = 0,
                    FileVersion = meterDataSet.FileGroup.ProcessingVersion
                };

                if (dataGroup.Samples > 0)
                {
                    evt.EventData = new EventData()
                    {
                        FileGroupID = meterDataSet.FileGroup.ID,
                        RunTimeID = i,
                        TimeDomainData = dataGroup.ToData(),
                        MarkedForDeletion = 0
                    };

                    evt.SamplesPerSecond = (int)Math.Round(dataGroup.SamplesPerSecond);
                    evt.SamplesPerCycle = Transform.CalculateSamplesPerCycle(dataGroup.SamplesPerSecond, SystemFrequency);
                }

                evt.Disturbances.AddRange(GetDisturbances(connection, meterDataSet, dataGroup));

                events.Add(evt);
            }

            Log.Info(string.Format("Finished processing {0} events.", count));

            return events;
        }

        private void LoadEvents(AdoDataConnection connection, List<Event> events, MeterDataSet meterDataSet)
        {
            TableOperations<Event> eventTable = new TableOperations<Event>(connection);
            TableOperations<EventData> eventDataTable = new TableOperations<EventData>(connection);
            TableOperations<DbDisturbance> disturbanceTable = new TableOperations<DbDisturbance>(connection);

            foreach (Event evt in events)
            {
                IDbDataParameter startTime2 = ToDateTime2(connection, evt.StartTime);
                IDbDataParameter endTime2 = ToDateTime2(connection, evt.EndTime);

                if (eventTable.QueryRecordsWhere("StartTime = {0} AND EndTime = {1} AND Samples = {2} AND MeterID = {3} AND LineID = {4}", startTime2, endTime2, evt.Samples, evt.MeterID, evt.LineID).Any())
                    continue;

                EventData eventData = evt.EventData;

                if ((object)eventData != null)
                {
                    eventDataTable.AddNewRecord(eventData);
                    eventData.ID = connection.ExecuteScalar<int>("SELECT @@IDENTITY");
                    evt.EventDataID = eventData.ID;
                }

                eventTable.AddNewRecord(evt);
                evt.ID = eventTable.QueryRecordWhere("StartTime = {0} AND EndTime = {1} AND Samples = {2} AND MeterID = {3} AND LineID = {4}", startTime2, endTime2, evt.Samples, evt.MeterID, evt.LineID).ID;

                foreach (DbDisturbance disturbance in evt.Disturbances)
                {
                    disturbance.EventID = evt.ID;
                    disturbanceTable.AddNewRecord(disturbance);
                }

                connection.ExecuteNonQuery(@"
                    IF dbo.EventHasImpactedComponents({0}) = 1
	                    INSERT INTO PQIResult VALUES ({0})                
                ".Trim(), evt.ID);

                ProcessSnapshots(meterDataSet, evt.ID, connection);
            }
        }

        private List<DbDisturbance> GetDisturbances(AdoDataConnection connection, MeterDataSet meterDataSet, DataGroup dataGroup)
        {
            SagDataResource sagDataResource = meterDataSet.GetResource<SagDataResource>();
            SwellDataResource swellDataResource = meterDataSet.GetResource<SwellDataResource>();
            InterruptionDataResource interruptionDataResource = meterDataSet.GetResource<InterruptionDataResource>();
            TransientDataResource transientDataResource = meterDataSet.GetResource<TransientDataResource>();

            List<DbDisturbance> dbDisturbances = new List<DbDisturbance>();
            List<Disturbance> disturbances;

            TableOperations<Event> eventTable = new TableOperations<Event>(connection);

            if (dataGroup.Classification == DataClassification.Trend)
                return dbDisturbances;

            if (dataGroup.Classification == DataClassification.Unknown)
                return dbDisturbances;

            if (sagDataResource.Sags.TryGetValue(dataGroup, out disturbances))
            {
                foreach (Disturbance sag in disturbances)
                    dbDisturbances.Add(GetDisturbanceRow(connection, sag));
            }

            if (swellDataResource.Swells.TryGetValue(dataGroup, out disturbances))
            {
                foreach (Disturbance swell in disturbances)
                    dbDisturbances.Add(GetDisturbanceRow(connection, swell));
            }

            if (interruptionDataResource.Interruptions.TryGetValue(dataGroup, out disturbances))
            {
                foreach (Disturbance interruption in disturbances)
                    dbDisturbances.Add(GetDisturbanceRow(connection, interruption));
            }

            if (transientDataResource.Transients.TryGetValue(dataGroup, out disturbances))
            {
                foreach (Disturbance transient in disturbances)
                    dbDisturbances.Add(GetDisturbanceRow(connection, transient));
            }


            return dbDisturbances;
        }
        
        private DbDisturbance GetDisturbanceRow(AdoDataConnection connection, Disturbance disturbance)
        {
            TableOperations<EventType> eventTypeTable = new TableOperations<EventType>(connection);
            EventType eventType = eventTypeTable.GetOrAdd(disturbance.EventType.ToString());

            TableOperations<Phase> phaseTable = new TableOperations<Phase>(connection);
            Phase phase = phaseTable.GetOrAdd(disturbance.Phase.ToString());

            DbDisturbance dbDisturbance = new DbDisturbance();

            dbDisturbance.EventTypeID = eventType.ID;
            dbDisturbance.PhaseID = phase.ID;
            dbDisturbance.Magnitude = disturbance.Magnitude;
            dbDisturbance.PerUnitMagnitude = ToDbFloat(disturbance.PerUnitMagnitude);
            dbDisturbance.StartTime = disturbance.StartTime;
            dbDisturbance.EndTime = disturbance.EndTime;
            dbDisturbance.DurationSeconds = disturbance.DurationSeconds;
            dbDisturbance.DurationCycles = disturbance.GetDurationCycles(SystemFrequency);
            dbDisturbance.StartIndex = disturbance.StartIndex;
            dbDisturbance.EndIndex = disturbance.EndIndex;

            return dbDisturbance;
        }

        private double GetPerUnitMagnitude(Line line, ReportedDisturbance disturbance)
        {
            double nominalVoltage = GetLineVoltage(line, disturbance.Phase);
            double puMax = disturbance.Maximum / nominalVoltage;
            double puMin = disturbance.Minimum / nominalVoltage;
            double maxDiff = Math.Abs(1.0D - puMax);
            double minDiff = Math.Abs(1.0D - puMin);

            if (maxDiff > minDiff)
                return puMax;

            return puMin;
        }

        private double GetLineVoltage(Line line, PQDPhase phase)
        {
            PQDPhase[] lnPhases = { PQDPhase.AN, PQDPhase.BN, PQDPhase.CN, PQDPhase.LineToNeutralAverage };
            double lineVoltage = line.VoltageKV;

            if (lnPhases.Contains(phase))
                lineVoltage /= Math.Sqrt(3.0D);

            return lineVoltage * 1000.0D;
        }

        private double ToDbFloat(double value)
        {
            const double Invalid = -1.0e308D;

            if (double.IsNaN(value) || double.IsInfinity(value))
                return Invalid;

            return value;
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

        private void ProcessSnapshots(MeterDataSet meterDataSet, int eventId, AdoDataConnection connection)
        {
            SnapshotDataResource snapshotDataResource = meterDataSet.GetResource<SnapshotDataResource>();

            TableOperations<SnapshotHarmonics> table = new TableOperations<SnapshotHarmonics>(connection);
            foreach(var kvp in snapshotDataResource.Snapshots)
            {
                table.AddNewRecord(new SnapshotHarmonics()
                {
                    EventID = eventId,
                    ChannelID = kvp.Key.ID,
                    SpectralData = kvp.Value
                });
            }
        }

        #endregion

        #region [ Static ]

        // Static Fields
        private static readonly ILog Log = LogManager.GetLogger(typeof(EventOperation));

        #endregion
    }
}
