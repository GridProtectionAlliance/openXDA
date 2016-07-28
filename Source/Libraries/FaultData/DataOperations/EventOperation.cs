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
using FaultData.Database;
using FaultData.Database.MeterDataTableAdapters;
using FaultData.DataResources;
using FaultData.DataSets;
using log4net;
using EventKey = System.Tuple<int, int, System.DateTime, System.DateTime, int>;
using WaveformKey = System.Tuple<int, int>;

namespace FaultData.DataOperations
{
    public class EventOperation : DataOperationBase<MeterDataSet>
    {
        #region [ Members ]

        // Constants
        private const double Sqrt3 = 1.7320508075688772935274463415059D;

        // Fields
        private DbAdapterContainer m_dbAdapterContainer;
        private double m_systemFrequency;
        private TimeZoneInfo m_timeZone;

        private MeterDataSet m_meterDataSet;
        private MeterData.EventDataTable m_eventTable;
        private MeterData.EventDataDataTable m_eventDataTable;
        private MeterData.DisturbanceDataTable m_disturbanceTable;
        private List<Tuple<WaveformKey, MeterData.EventRow>> m_eventList;
        private List<Tuple<EventKey, MeterData.DisturbanceRow>> m_disturbanceList;

        private DataContextLookup<string, Phase> m_phaseLookup;

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

        public override void Prepare(DbAdapterContainer dbAdapterContainer)
        {
            m_dbAdapterContainer = dbAdapterContainer;
            LoadEventTypes(dbAdapterContainer);
        }

        public override void Execute(MeterDataSet meterDataSet)
        {
            CycleDataResource cycleDataResource;
            EventClassificationResource eventClassificationResource;

            Log.Info("Executing operation to load event data into the database...");

            cycleDataResource = CycleDataResource.GetResource(meterDataSet, m_dbAdapterContainer);
            eventClassificationResource = meterDataSet.GetResource(() => new EventClassificationResource(m_dbAdapterContainer));
            LoadEvents(meterDataSet, cycleDataResource.DataGroups, cycleDataResource.VICycleDataGroups, eventClassificationResource.Classifications);
            LoadDisturbances(meterDataSet, cycleDataResource.DataGroups);

            m_meterDataSet = meterDataSet;
        }

        public override void Load(DbAdapterContainer dbAdapterContainer)
        {
            BulkLoader bulkLoader;

            EventTableAdapter eventAdapter;
            EventDataTableAdapter eventDataAdapter;
            Dictionary<EventKey, MeterData.EventRow> eventLookup;
            Dictionary<WaveformKey, MeterData.EventDataRow> eventDataLookup;

            MeterData.EventRow eventRow;

            if (m_eventDataTable.Count == 0)
                return;

            Log.Info("Loading event data into the database...");

            // Create the bulk loader for loading data into the database
            bulkLoader = new BulkLoader();
            bulkLoader.Connection = dbAdapterContainer.Connection;
            bulkLoader.CommandTimeout = dbAdapterContainer.CommandTimeout;

            // Write event data to the database
            bulkLoader.Load(m_eventDataTable);

            // Query database for event data IDs and store them in a lookup table by waveform key
            eventDataAdapter = dbAdapterContainer.GetAdapter<EventDataTableAdapter>();
            eventDataAdapter.FillByFileGroup(m_eventDataTable, m_meterDataSet.FileGroup.ID);
            eventDataLookup = m_eventDataTable.ToDictionary(CreateWaveformKey);

            // Update the event rows with the IDs from the event data table
            foreach (Tuple<WaveformKey, MeterData.EventRow> tuple in m_eventList)
            {
                tuple.Item2.EventDataID = eventDataLookup[tuple.Item1].ID;
                m_eventTable.AddEventRow(tuple.Item2);
            }

            // Write events to the database
            bulkLoader.Load(m_eventTable);

            // Query database for events and store them in a lookup table by event key
            eventAdapter = dbAdapterContainer.GetAdapter<EventTableAdapter>();
            eventAdapter.FillByFileGroup(m_eventTable, m_meterDataSet.FileGroup.ID);
            eventLookup = m_eventTable.ToDictionary(CreateEventKey);

            // Update the disturbance rows with the IDs from the event table
            foreach (Tuple<EventKey, MeterData.DisturbanceRow> tuple in m_disturbanceList)
            {
                if (eventLookup.TryGetValue(tuple.Item1, out eventRow))
                {
                    tuple.Item2.EventID = eventRow.ID;
                    m_disturbanceTable.AddDisturbanceRow(tuple.Item2);
                }
            }

            // Write disturbances to the database
            bulkLoader.Load(m_disturbanceTable);

            Log.Info(string.Format("Loaded {0} events into the database.", m_eventTable.Count));
        }

        private void LoadEventTypes(DbAdapterContainer dbAdapterContainer)
        {
            if ((object)s_eventTypeLookup == null)
            {
                lock (s_eventTypeLock)
                {
                    if ((object)s_eventTypeLookup == null)
                        s_eventTypeLookup = GetEventTypeLookup(dbAdapterContainer);
                }
            }
        }

        private void LoadEvents(MeterDataSet meterDataSet, List<DataGroup> dataGroups, List<VICycleDataGroup> viCycleDataGroups, Dictionary<DataGroup, EventClassification> classifications)
        {
            DataGroup dataGroup;
            EventClassification eventClassification;
            int eventTypeID;

            MeterData.EventRow eventRow;
            MeterData.EventDataRow eventDataRow;

            m_eventTable = new MeterData.EventDataTable();
            m_eventDataTable = new MeterData.EventDataDataTable();
            m_eventList = new List<Tuple<WaveformKey, MeterData.EventRow>>();

            for (int i = 0; i < dataGroups.Count; i++)
            {
                dataGroup = dataGroups[i];

                if (dataGroup.Classification == DataClassification.Trend || dataGroup.Classification == DataClassification.Unknown)
                    continue;

                if (!classifications.TryGetValue(dataGroup, out eventClassification))
                    continue;

                if (!s_eventTypeLookup.TryGetValue(eventClassification, out eventTypeID))
                    continue;

                Log.Debug(string.Format("Processing event with event type {0}.", eventClassification));

                eventRow = m_eventTable.NewEventRow();
                eventRow.FileGroupID = meterDataSet.FileGroup.ID;
                eventRow.MeterID = meterDataSet.Meter.ID;
                eventRow.LineID = dataGroup.Line.ID;
                eventRow.EventTypeID = eventTypeID;
                eventRow.Name = string.Empty;
                eventRow.StartTime = dataGroup.StartTime;
                eventRow.EndTime = dataGroup.EndTime;
                eventRow.Samples = dataGroup.Samples;
                eventRow.TimeZoneOffset = (int)m_timeZone.GetUtcOffset(dataGroup.StartTime).TotalMinutes;
                eventRow.SamplesPerSecond = (int)Math.Round(dataGroup.SamplesPerSecond);
                eventRow.SamplesPerCycle = (int)Math.Round(dataGroup.SamplesPerSecond / m_systemFrequency);

                eventDataRow = m_eventDataTable.NewEventDataRow();
                eventDataRow.FileGroupID = meterDataSet.FileGroup.ID;
                eventDataRow.RuntimeID = i;
                eventDataRow.TimeDomainData = dataGroup.ToData();
                eventDataRow.FrequencyDomainData = viCycleDataGroups[i].ToDataGroup().ToData();
                eventDataRow.MarkedForDeletion = 0;

                m_eventDataTable.AddEventDataRow(eventDataRow);
                m_eventList.Add(Tuple.Create(CreateWaveformKey(eventDataRow), eventRow));
            }

            Log.Info(string.Format("Finished processing {0} events.", m_eventList.Count));
        }

        private void LoadDisturbances(MeterDataSet meterDataSet, List<DataGroup> dataGroups)
        {
            SagDataResource sagDataResource = SagDataResource.GetResource(meterDataSet, m_dbAdapterContainer);
            SwellDataResource swellDataResource = SwellDataResource.GetResource(meterDataSet, m_dbAdapterContainer);
            InterruptionDataResource interruptionDataResource = InterruptionDataResource.GetResource(meterDataSet, m_dbAdapterContainer);

            EventKey eventKey;
            List<Disturbance> disturbances;

            m_disturbanceTable = new MeterData.DisturbanceDataTable();
            m_disturbanceList = new List<Tuple<EventKey, MeterData.DisturbanceRow>>();

            foreach (DataGroup dataGroup in dataGroups)
            {
                if (dataGroup.Classification == DataClassification.Trend || dataGroup.Classification == DataClassification.Unknown)
                    continue;

                eventKey = CreateEventKey(meterDataSet.FileGroup, dataGroup);

                if (sagDataResource.Sags.TryGetValue(dataGroup, out disturbances))
                {
                    foreach (Disturbance sag in disturbances)
                        AddDisturbanceRow(eventKey, dataGroup, sag);
                }

                if (swellDataResource.Swells.TryGetValue(dataGroup, out disturbances))
                {
                    foreach (Disturbance swell in disturbances)
                        AddDisturbanceRow(eventKey, dataGroup, swell);
                }

                if (interruptionDataResource.Interruptions.TryGetValue(dataGroup, out disturbances))
                {
                    foreach (Disturbance interruption in disturbances)
                        AddDisturbanceRow(eventKey, dataGroup, interruption);
                }
            }
        }

        private void AddDisturbanceRow(EventKey eventKey, DataGroup dataGroup, Disturbance disturbance)
        {
            MeterData.DisturbanceRow row = m_disturbanceTable.NewDisturbanceRow();

            row.EventTypeID = s_eventTypeLookup[disturbance.EventType];
            row.PhaseID = GetPhaseID(disturbance.Phase);
            row.Magnitude = disturbance.Magnitude;
            row.PerUnitMagnitude = ToDbFloat(disturbance.PerUnitMagnitude);
            row.StartTime = disturbance.StartTime;
            row.EndTime = disturbance.EndTime;
            row.DurationSeconds = disturbance.DurationSeconds;
            row.DurationCycles = disturbance.GetDurationCycles(m_systemFrequency);
            row.StartIndex = disturbance.StartIndex;
            row.EndIndex = disturbance.EndIndex;

            m_disturbanceList.Add(Tuple.Create(eventKey, row));
        }

        private int GetPhaseID(GSF.PQDIF.Logical.Phase phase)
        {
            if ((object)m_phaseLookup == null)
                m_phaseLookup = new DataContextLookup<string, Phase>(m_dbAdapterContainer.GetAdapter<MeterInfoDataContext>(), ph => ph.Name);

            return m_phaseLookup.GetOrAdd(phase.ToString(), name => new Phase() { Name = name, Description = name }).ID;
        }

        private Dictionary<EventClassification, int> GetEventTypeLookup(DbAdapterContainer dbAdapterContainer)
        {
            MeterData.EventTypeDataTable eventTypeTable = new MeterData.EventTypeDataTable();
            EventClassification eventClassification = default(EventClassification);

            foreach (EventClassification classification in Enum.GetValues(typeof(EventClassification)))
                eventTypeTable.AddEventTypeRow(classification.ToString(), classification.ToString());

            BulkLoader bulkLoader = new BulkLoader();

            bulkLoader.Connection = dbAdapterContainer.Connection;
            bulkLoader.CommandTimeout = dbAdapterContainer.CommandTimeout;

            bulkLoader.MergeTableFormat = "MERGE INTO {0} AS Target " +
                                          "USING {1} AS Source " +
                                          "ON Source.Name = Target.Name " +
                                          "WHEN NOT MATCHED THEN " +
                                          "    INSERT (Name, Description) " +
                                          "    VALUES (Source.Name, Source.Description);";

            bulkLoader.Load(eventTypeTable);

            dbAdapterContainer.GetAdapter<EventTypeTableAdapter>().Fill(eventTypeTable);

            return Enumerable.Select(eventTypeTable
                .Where(row => Enum.TryParse(row.Name, out eventClassification)), row => Tuple.Create(eventClassification, row.ID))
                .ToDictionary(tuple => tuple.Item1, tuple => tuple.Item2);
        }

        private EventKey CreateEventKey(FileGroup fileGroup, DataGroup dataGroup)
        {
            return Tuple.Create(fileGroup.ID, dataGroup.Line.ID, dataGroup.StartTime, dataGroup.EndTime, dataGroup.Samples);
        }

        private EventKey CreateEventKey(MeterData.EventRow eventRow)
        {
            return Tuple.Create(eventRow.FileGroupID, eventRow.LineID, eventRow.StartTime, eventRow.EndTime, eventRow.Samples);
        }

        private WaveformKey CreateWaveformKey(MeterData.EventDataRow eventDataRow)
        {
            return Tuple.Create(eventDataRow.FileGroupID, eventDataRow.RuntimeID);
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
        private static object s_eventTypeLock = new object();
        private static Dictionary<EventClassification, int> s_eventTypeLookup;
        private static readonly ILog Log = LogManager.GetLogger(typeof(EventOperation));

        #endregion
    }
}
