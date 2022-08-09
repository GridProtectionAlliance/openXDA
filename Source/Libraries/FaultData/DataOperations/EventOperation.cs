﻿//******************************************************************************************************
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
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FaultData.DataAnalysis;
using FaultData.DataResources;
using FaultData.DataSets;
using GSF.Collections;
using GSF.Configuration;
using GSF.Data;
using GSF.Data.Model;
using log4net;
using openXDA.Configuration;
using openXDA.Model;
using openXDA.PQI;
using DbDisturbance = openXDA.Model.Disturbance;
using Disturbance = FaultData.DataAnalysis.Disturbance;

namespace FaultData.DataOperations
{
    public class EventOperation : DataOperationBase<MeterDataSet>
    {
        #region [ Members ]

        // Constants
        private const double Sqrt3 = 1.7320508075688772935274463415059D;

        #endregion

        #region [ Properties ]

        [Category]
        [SettingName(SystemSection.CategoryName)]
        public SystemSection SystemSettings { get; }
            = new SystemSection();

        [Category]
        [SettingName(DataAnalysisSection.CategoryName)]
        public DataAnalysisSection DataAnalysisSettings { get; }
            = new DataAnalysisSection();

        [Category]
        [SettingName(PQISection.CategoryName)]
        public PQISection PQISettings { get; }
            = new PQISection();

        private TimeSpan MaxEventDurationSpan =>
            TimeSpan.FromSeconds(DataAnalysisSettings.MaxEventDuration);

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

                List<DataGroup> processedDataGroups;
                List<Event> events = GetEvents(connection, meterDataSet, dataGroups, cycleDataResource.VICycleDataGroups, eventClassificationResource.Classifications, out processedDataGroups);
                LoadEvents(connection, events, meterDataSet, processedDataGroups);
            }

            LoadReportedEventType(meterDataSet);
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

                    Asset asset = dataGroup.Asset ?? meterDataSet.Meter.Location.AssetLocations.Single().Asset;
                    IDbDataParameter startTime2 = ToDateTime2(connection, dataGroup.StartTime);
                    IDbDataParameter endTime2 = ToDateTime2(connection, dataGroup.EndTime);

                    const string Filter =
                        "StartTime = {0} AND " +
                        "EndTime = {1} AND " +
                        "Samples = {2} AND " +
                        "MeterID = {3} AND " +
                        "AssetID = {4}";

                    int count = eventTable.QueryRecordCountWhere(Filter, startTime2, endTime2, dataGroup.Samples, meterDataSet.Meter.ID, asset.ID);

                    if (count > 0)
                        dataGroups.RemoveAt(i);
                }
            }
        }

        private List<Event> GetEvents(AdoDataConnection connection, MeterDataSet meterDataSet, List<DataGroup> dataGroups, List<VICycleDataGroup> viCycleDataGroups, Dictionary<DataGroup, EventClassification> eventClassifications, out List<DataGroup> processedDataGroups)
        {
            int count = dataGroups
                .Where(dataGroup => dataGroup.Classification != DataClassification.Trend)
                .Where(dataGroup => dataGroup.Classification != DataClassification.Unknown)
                .Where(dataGroup => eventClassifications.ContainsKey(dataGroup))
                .Count();

            processedDataGroups = new List<DataGroup>();
            if (count == 0)
            {
                Log.Info($"No events found for file '{meterDataSet.FilePath}'.");
                return new List<Event>();
            }

            Log.Info(string.Format("Processing {0} events...", count));

            List<Event> events = new List<Event>(count);

            TableOperations<Event> eventTable = new TableOperations<Event>(connection);
            TableOperations<EventType> eventTypeTable = new TableOperations<EventType>(connection);

            for (int i = 0; i < dataGroups.Count; i++)
            {
                DataGroup dataGroup = dataGroups[i];
                TimeSpan duration = !double.IsNaN(dataGroup.Duration)
                    ? TimeSpan.FromSeconds(dataGroup.Duration)
                    : TimeSpan.Zero;

                if (MaxEventDurationSpan > TimeSpan.Zero && duration > MaxEventDurationSpan)
                    continue;

                if (dataGroup.Classification == DataClassification.Trend)
                    continue;

                if (dataGroup.Classification == DataClassification.Unknown)
                    continue;

                if (!eventClassifications.TryGetValue(dataGroup, out EventClassification eventClassification))
                    continue;

                if ((object)dataGroup.Asset == null && meterDataSet.Meter.Location.AssetLocations.Count != 1)
                    continue;

                Asset asset = dataGroup.Asset ?? meterDataSet.Meter.Location.AssetLocations.Single().Asset;
                IDbDataParameter startTime2 = ToDateTime2(connection, dataGroup.StartTime);
                IDbDataParameter endTime2 = ToDateTime2(connection, dataGroup.EndTime);

                if (eventTable.QueryRecordCountWhere("StartTime = {0} AND EndTime = {1} AND Samples = {2} AND MeterID = {3} AND AssetID = {4}", startTime2, endTime2, dataGroup.Samples, meterDataSet.Meter.ID, asset.ID) > 0)
                    continue;

                TableOperations<MaintenanceWindow> maintenanceWindowTable = new TableOperations<MaintenanceWindow>(connection);
                int maintenanceWindowCount = maintenanceWindowTable.QueryRecordCountWhere("MeterID = {0} AND (StartTime IS NULL OR StartTime <= {1}) AND (EndTime IS NULL OR EndTime >= {1})", meterDataSet.Meter.ID, DateTime.UtcNow);
                string eventTypeName = (maintenanceWindowCount == 0) ? eventClassification.ToString() : "Test";
                EventType eventType = eventTypeTable.GetOrAdd(eventTypeName);
                TimeZoneInfo xdaTimeZone = SystemSettings.XDATimeZoneInfo;

                Event evt = new Event()
                {
                    FileGroupID = meterDataSet.FileGroup.ID,
                    MeterID = meterDataSet.Meter.ID,
                    AssetID = asset.ID,
                    EventTypeID = eventType.ID,
                    EventDataID = null,
                    Name = string.Empty,
                    StartTime = dataGroup.StartTime,
                    EndTime = dataGroup.EndTime,
                    Samples = dataGroup.Samples,
                    TimeZoneOffset = (int)xdaTimeZone.GetUtcOffset(dataGroup.StartTime).TotalMinutes,
                    SamplesPerSecond = 0,
                    SamplesPerCycle = 0,
                    FileVersion = meterDataSet.FileGroup.ProcessingVersion
                };

                if (dataGroup.Samples > 0)
                {
                    evt.EventData = dataGroup.ToData().Select(item =>
                   {
                       return new ChannelData()
                       {
                           SeriesID = item.Key,
                           EventID = evt.ID,
                           TimeDomainData = item.Value,
                           MarkedForDeletion = 0
                       };
                   }).ToList();

                    evt.SamplesPerSecond = (int)Math.Round(dataGroup.SamplesPerSecond);
                    evt.SamplesPerCycle = Transform.CalculateSamplesPerCycle(dataGroup.SamplesPerSecond, DataAnalysisSettings.SystemFrequency);
                }

                evt.Disturbances.AddRange(GetDisturbances(connection, meterDataSet, dataGroup));

                processedDataGroups.Add(dataGroup);
                events.Add(evt);
            }

            Log.Info(string.Format("Finished processing {0} events.", count));

            return events;
        }

        private void LoadEvents(AdoDataConnection connection, List<Event> events, MeterDataSet meterDataSet, List<DataGroup> dataGroups)
        {
            TableOperations<Event> eventTable = new TableOperations<Event>(connection);
            TableOperations<ChannelData> eventDataTable = new TableOperations<ChannelData>(connection);
            TableOperations<DbDisturbance> disturbanceTable = new TableOperations<DbDisturbance>(connection);

            int i = 0;
            foreach (Event evt in events)
            {
                IDbDataParameter startTime2 = ToDateTime2(connection, evt.StartTime);
                IDbDataParameter endTime2 = ToDateTime2(connection, evt.EndTime);

                if (eventTable.QueryRecordsWhere("StartTime = {0} AND EndTime = {1} AND Samples = {2} AND MeterID = {3} AND AssetID = {4}", startTime2, endTime2, evt.Samples, evt.MeterID, evt.AssetID).Any())
                    continue;

                eventTable.AddNewRecord(evt);
                evt.ID = eventTable.QueryRecordWhere("StartTime = {0} AND EndTime = {1} AND Samples = {2} AND MeterID = {3} AND AssetID = {4}", startTime2, endTime2, evt.Samples, evt.MeterID, evt.AssetID).ID;

                List<ChannelData> eventData = evt.EventData;

                if ((object)eventData != null)
                {
                    foreach (ChannelData channelData in eventData)
                    {

                        if ((object)channelData != null)
                        {
                            channelData.EventID = evt.ID;
                            eventDataTable.AddNewRecord(channelData);
                            channelData.ID = connection.ExecuteScalar<int>("SELECT @@IDENTITY");
                        }
                    }
                }

                foreach (DbDisturbance disturbance in evt.Disturbances)
                {
                    disturbance.EventID = evt.ID;
                    disturbanceTable.AddNewRecord(disturbance);
                }

                LoadWorstDisturbance(connection, evt.ID, GetWorstDisturbances(connection,meterDataSet,dataGroups[i]));

                CheckPQIComponents(meterDataSet, connection, evt);
                ProcessSnapshots(meterDataSet, evt.ID, connection);
                i++;
            }
        }

        private void LoadWorstDisturbance(AdoDataConnection connection, int eventID, List<Disturbance> worstDisturbances)
        {
            TableOperations<DbDisturbance> disturbanceTable = new TableOperations<DbDisturbance>(connection);

            if (worstDisturbances.Count == 0)
                return;

            foreach (IGrouping <EventClassification, Disturbance> typeGrouping in worstDisturbances.GroupBy(evt => evt.EventType))
            {
                foreach (IGrouping<int, Disturbance> overlap in typeGrouping.GroupBy(evt => evt.WorstDisturbanceGrouping))
                {
                    Disturbance worst = overlap.Where(item => item.IsTotalWorstDisturbance).FirstOrDefault();
                    if (worst == null)
                        continue;

                    Disturbance worstLL = overlap.Where(item => item.IsLLDisturbance).FirstOrDefault();
                    Disturbance worstLN = overlap.Where(item => item.IsLNDisturbance).FirstOrDefault();

                    int? worstLLID = GetDisturbanceID(connection, disturbanceTable, worstLL, eventID);
                    int? worstLNID = GetDisturbanceID(connection, disturbanceTable, worstLN, eventID);

                    int worstID = -1;
                    if (worstLL != null && worstLL.StartIndex == worst.StartIndex && worstLL.PerUnitMagnitude == worst.PerUnitMagnitude)
                        worstID = worstLLID ?? -1;
                    if (worstLN != null && worstLN.StartIndex == worst.StartIndex && worstLN.PerUnitMagnitude == worst.PerUnitMagnitude)
                        worstID = worstLNID ?? -1;

                    if (worstID == -1)
                        worstID = GetDisturbanceID(connection, disturbanceTable, worst, eventID) ?? -1;

                    if (worstID == -1)
                        continue;

                    EventWorstDisturbance worstDisturbanceRecord = new EventWorstDisturbance()
                    {
                        EventID = eventID,
                        WorstDisturbanceID = worstID,
                        WorstLLDisturbanceID = worstLLID,
                        WorstLNDisturbanceID = worstLNID
                    };

                    new TableOperations<EventWorstDisturbance>(connection).AddNewRecord(worstDisturbanceRecord);
                }
            }
        }

        private void LoadReportedEventType(MeterDataSet meterDataSet)
        {
            AnalysisDataSet faultLocationDataSet = meterDataSet.AnalysisDataSet;
            string eventType = faultLocationDataSet?.EventType;

            if (string.IsNullOrWhiteSpace(eventType))
                return;

            using (AdoDataConnection connection = meterDataSet.CreateDbConnection())
            {
                const string FieldName = "EventType";
                const string Description = "Type of event as determined by the meter.";
                meterDataSet.FileGroup.AddOrUpdateFieldValue(connection, FieldName, eventType, Description);
            }
        }

        private int? GetDisturbanceID(AdoDataConnection connection, TableOperations<DbDisturbance> disturbanceTable, Disturbance disturbance, int eventID)
        {
            if (disturbance == null)
                return null;

            DbDisturbance dbDisturbance = GetDisturbanceRow(connection, disturbance);
            
            string sql = "EventTypeID = {0} AND ";
            sql += "PhaseID = {1} AND ";
            sql += "Magnitude = {2} AND ";
            sql += "PerUnitMagnitude = {3} AND ";
            sql += "DurationSeconds = {4} AND ";
            sql += "DurationCycles = {5} AND ";
            sql += "StartIndex = {6} AND ";
            sql += "EndIndex = {7} AND ";
            sql += "EventID = {8}";

            dbDisturbance = disturbanceTable.QueryRecordWhere(sql, dbDisturbance.EventTypeID, dbDisturbance.PhaseID, dbDisturbance.Magnitude, dbDisturbance.PerUnitMagnitude,
                dbDisturbance.DurationSeconds, dbDisturbance.DurationCycles, 
                dbDisturbance.StartIndex, dbDisturbance.EndIndex, eventID);

            return dbDisturbance?.ID;
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
        
        private List<Disturbance> GetWorstDisturbances(AdoDataConnection connection, MeterDataSet meterDataSet, DataGroup dataGroup)
        {
            SagDataResource sagDataResource = meterDataSet.GetResource<SagDataResource>();
            SwellDataResource swellDataResource = meterDataSet.GetResource<SwellDataResource>();
            InterruptionDataResource interruptionDataResource = meterDataSet.GetResource<InterruptionDataResource>();
            TransientDataResource transientDataResource = meterDataSet.GetResource<TransientDataResource>();

            List<Disturbance> disturbances = new List<Disturbance>();
            if (dataGroup.Classification == DataClassification.Unknown)
                return disturbances;

            if (sagDataResource.Sags.TryGetValue(dataGroup, out List<Disturbance> sags))
                disturbances.AddRange(sags.Where(d => d.IsWorstDisturbance && d.Phase != GSF.PQDIF.Logical.Phase.Worst));
               
            if (swellDataResource.Swells.TryGetValue(dataGroup, out List<Disturbance> swells))
                disturbances.AddRange(swells.Where(d => d.IsWorstDisturbance && d.Phase != GSF.PQDIF.Logical.Phase.Worst));

            if (interruptionDataResource.Interruptions.TryGetValue(dataGroup, out List<Disturbance> interruptions))
                disturbances.AddRange(interruptions.Where(d => d.IsWorstDisturbance && d.Phase != GSF.PQDIF.Logical.Phase.Worst));

            if (transientDataResource.Transients.TryGetValue(dataGroup, out List<Disturbance> transients))
               disturbances.AddRange(transients.Where(d => d.IsWorstDisturbance && d.Phase != GSF.PQDIF.Logical.Phase.Worst));

            return disturbances;
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
            dbDisturbance.DurationCycles = disturbance.GetDurationCycles(DataAnalysisSettings.SystemFrequency);
            dbDisturbance.StartIndex = disturbance.StartIndex;
            dbDisturbance.EndIndex = disturbance.EndIndex;

            return dbDisturbance;
        }

        private void CheckPQIComponents(MeterDataSet meterDataSet, AdoDataConnection connection, Event evt)
        {
            if (string.IsNullOrEmpty(PQISettings.BaseURL))
                return;

            if (string.IsNullOrEmpty(PQISettings.PingURL))
                return;

            if (string.IsNullOrEmpty(PQISettings.ClientID))
                return;

            if (string.IsNullOrEmpty(PQISettings.ClientSecret))
                return;

            if (string.IsNullOrEmpty(PQISettings.Username))
                return;

            if (string.IsNullOrEmpty(PQISettings.Password))
                return;

            string FetchAccessToken()
            {
                NetworkCredential clientCredential = new NetworkCredential(PQISettings.ClientID, PQISettings.ClientSecret);
                NetworkCredential userCredential = new NetworkCredential(PQISettings.Username, PQISettings.Password);
                PingClient pingClient = new PingClient(PQISettings.PingURL);
                Task exchangeTask = pingClient.ExchangeAsync(clientCredential, userCredential);
                exchangeTask.GetAwaiter().GetResult();
                return pingClient.AccessToken;
            }

            PQIWSClient pqiwsClient = new PQIWSClient(PQISettings.BaseURL, FetchAccessToken);
            PQIWSQueryHelper pqiwsQueryHelper = new PQIWSQueryHelper(meterDataSet.CreateDbConnection, pqiwsClient);
            Task<bool> queryTask = pqiwsQueryHelper.HasImpactedComponentsAsync(evt.ID);

            if (queryTask.GetAwaiter().GetResult())
                connection.ExecuteNonQuery("INSERT INTO PQIResult VALUES ({0})", evt.ID);
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
