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
using GSF.Data;
using GSF.Data.Model;
using log4net;
using openXDA.Model;
using Disturbance = FaultData.DataAnalysis.Disturbance;

namespace FaultData.DataOperations
{
    public class EventOperation : DataOperationBase<MeterDataSet>
    {
        #region [ Members ]

        // Constants
        private const double Sqrt3 = 1.7320508075688772935274463415059D;

        // Fields
        private double m_systemFrequency;
        private TimeZoneInfo m_timeZone;

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

        [Setting]
        public string XDATimeZone
        {
            get
            {
                return m_timeZone.Id;
            }
            set
            {
                m_timeZone = TimeZoneInfo.FindSystemTimeZoneById(value);
            }
        }

        #endregion

        #region [ Methods ]

        public override void Execute(MeterDataSet meterDataSet)
        {
            CycleDataResource cycleDataResource;
            EventClassificationResource eventClassificationResource;

            cycleDataResource = meterDataSet.GetResource<CycleDataResource>();
            eventClassificationResource = meterDataSet.GetResource<EventClassificationResource>();
            LoadEvents(meterDataSet, cycleDataResource.DataGroups, cycleDataResource.VICycleDataGroups, eventClassificationResource.Classifications);
            LoadDisturbances(meterDataSet, cycleDataResource.DataGroups);
        }

        private void LoadEvents(MeterDataSet meterDataSet, List<DataGroup> dataGroups, List<VICycleDataGroup> viCycleDataGroups, Dictionary<DataGroup, EventClassification> eventClassifications)
        {
            int count = dataGroups
                .Where(dataGroup => dataGroup.Classification != DataClassification.Trend)
                .Where(dataGroup => dataGroup.Classification != DataClassification.Unknown)
                .Where(dataGroup => eventClassifications.ContainsKey(dataGroup))
                .Count();

            if (count == 0)
            {
                Log.Info($"No events found for file '{meterDataSet.FilePath}'.");
                return;
            }

            Log.Info(string.Format("Processing {0} events...", count));

            using (AdoDataConnection connection = meterDataSet.CreateDbConnection())
            {
                TableOperations<Event> eventTable = new TableOperations<Event>(connection);
                TableOperations<EventType> eventTypeTable = new TableOperations<EventType>(connection);
                TableOperations<EventData> eventDataTable = new TableOperations<EventData>(connection);

                for (int i = 0; i < dataGroups.Count; i++)
                {

                    DataGroup dataGroup = dataGroups[i];
                    EventClassification eventClassification;

                    if (dataGroup.Classification == DataClassification.Trend)
                        continue;

                    if (dataGroup.Classification == DataClassification.Unknown)
                        continue;

                    if (!eventClassifications.TryGetValue(dataGroup, out eventClassification))
                        continue;

                    if (eventTable.QueryRecordsWhere("StartTime = {0} AND EndTime = {1} AND Samples = {2} AND MeterID = {3} AND LineID = {4}", dataGroup.StartTime, dataGroup.EndTime, dataGroup.Samples, meterDataSet.Meter.ID, dataGroup.Line.ID).Any())
                        continue;
                        
                    EventType eventType = eventTypeTable.GetOrAdd(eventClassification.ToString());

                    EventData eventData = new EventData()
                    {
                        FileGroupID = meterDataSet.FileGroup.ID,
                        RunTimeID = i,
                        TimeDomainData = dataGroup.ToData(),
                        FrequencyDomainData = viCycleDataGroups[i].ToDataGroup().ToData(),
                        MarkedForDeletion = 0
                    };

                    eventDataTable.AddNewRecord(eventData);
                    eventData.ID = connection.ExecuteScalar<int>("SELECT @@IDENTITY");

                    Event evt = new Event()
                    {
                        FileGroupID = meterDataSet.FileGroup.ID,
                        MeterID = meterDataSet.Meter.ID,
                        LineID = dataGroup.Line.ID,
                        EventTypeID = eventType.ID,
                        EventDataID = eventData.ID,
                        Name = string.Empty,
                        StartTime = dataGroup.StartTime,
                        EndTime = dataGroup.EndTime,
                        Samples = dataGroup.Samples,
                        TimeZoneOffset = (int)m_timeZone.GetUtcOffset(dataGroup.StartTime).TotalMinutes,
                        SamplesPerSecond = (int)Math.Round(dataGroup.SamplesPerSecond),
                        SamplesPerCycle = Transform.CalculateSamplesPerCycle(dataGroup.SamplesPerSecond, m_systemFrequency)
                    };

                    eventTable.AddNewRecord(evt);
                }
            }

            Log.Info(string.Format("Finished processing {0} events.", count));
        }

        private void LoadDisturbances(MeterDataSet meterDataSet, List<DataGroup> dataGroups)
        {
            SagDataResource sagDataResource = meterDataSet.GetResource<SagDataResource>();
            SwellDataResource swellDataResource = meterDataSet.GetResource<SwellDataResource>();
            InterruptionDataResource interruptionDataResource = meterDataSet.GetResource<InterruptionDataResource>();
            List<Disturbance> disturbances;

            int count = dataGroups
                .Where(dataGroup => dataGroup.Classification != DataClassification.Trend)
                .Where(dataGroup => dataGroup.Classification != DataClassification.Unknown)
                .Count();

            if (count == 0)
                return;

            using (AdoDataConnection connection = meterDataSet.CreateDbConnection())
            {
                TableOperations<Event> eventTable = new TableOperations<Event>(connection);

                foreach (DataGroup dataGroup in dataGroups)
                {
                    if (dataGroup.Classification == DataClassification.Trend)
                        continue;

                    if (dataGroup.Classification == DataClassification.Unknown)
                        continue;

                    Event evt = eventTable.GetEvent(meterDataSet.FileGroup, dataGroup);

                    if (sagDataResource.Sags.TryGetValue(dataGroup, out disturbances))
                    {
                        foreach (Disturbance sag in disturbances)
                            AddDisturbanceRow(connection, evt.ID, sag);
                    }

                    if (swellDataResource.Swells.TryGetValue(dataGroup, out disturbances))
                    {
                        foreach (Disturbance swell in disturbances)
                            AddDisturbanceRow(connection, evt.ID, swell);
                    }

                    if (interruptionDataResource.Interruptions.TryGetValue(dataGroup, out disturbances))
                    {
                        foreach (Disturbance interruption in disturbances)
                            AddDisturbanceRow(connection, evt.ID, interruption);
                    }

                    connection.ExecuteNonQuery(@"
                        IF dbo.EventHasImpactedComponents({0}) = 1
	                        INSERT INTO PQIResult VALUES ({0})                
                    ", evt.ID);

                }

            }
        }
        
        private void AddDisturbanceRow(AdoDataConnection connection, int eventID, Disturbance disturbance)
        {
            TableOperations<EventType> eventTypeTable = new TableOperations<EventType>(connection);
            EventType eventType = eventTypeTable.GetOrAdd(disturbance.EventType.ToString());

            TableOperations<Phase> phaseTable = new TableOperations<Phase>(connection);
            Phase phase = phaseTable.GetOrAdd(disturbance.Phase.ToString());

            TableOperations<openXDA.Model.Disturbance> disturbanceTable = new TableOperations<openXDA.Model.Disturbance>(connection);
            openXDA.Model.Disturbance dbDisturbance = new openXDA.Model.Disturbance();

            dbDisturbance.EventID = eventID;
            dbDisturbance.EventTypeID = eventType.ID;
            dbDisturbance.PhaseID = phase.ID;
            dbDisturbance.Magnitude = disturbance.Magnitude;
            dbDisturbance.PerUnitMagnitude = ToDbFloat(disturbance.PerUnitMagnitude);
            dbDisturbance.StartTime = disturbance.StartTime;
            dbDisturbance.EndTime = disturbance.EndTime;
            dbDisturbance.DurationSeconds = disturbance.DurationSeconds;
            dbDisturbance.DurationCycles = disturbance.GetDurationCycles(m_systemFrequency);
            dbDisturbance.StartIndex = disturbance.StartIndex;
            dbDisturbance.EndIndex = disturbance.EndIndex;

            disturbanceTable.AddNewRecord(dbDisturbance);
        }

        private double ToDbFloat(double value)
        {
            const double Invalid = -1.0e308D;

            if (double.IsNaN(value) || double.IsInfinity(value))
                return Invalid;

            return value;
        }

        #endregion

        #region [ Static ]

        // Static Fields
        private static readonly ILog Log = LogManager.GetLogger(typeof(EventOperation));

        #endregion
    }
}
