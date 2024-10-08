﻿//******************************************************************************************************
//  WebAPI.cs - Gbtc
//
//  Copyright © 2020, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the MIT License (MIT), the "License"; you may not use this
//  file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://opensource.org/licenses/MIT
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  10/06/2020 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using GSF.Data;
using GSF.Data.Model;
using GSF.IO;
using GSF.Web.Model;
using HIDS;
using Newtonsoft.Json.Linq;
using openXDA.APIMiddleware;
using openXDA.Model;
using SystemCenter.Model;

namespace openXDA.Controllers.WebAPI
{
    [RoutePrefix("api/Meter")]
    public class MeterController : ModelController<SystemCenter.Model.DetailedMeter> {
        [HttpGet, Route("Sapphire")]
        public IHttpActionResult GetSapphireMeters()
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                return Ok(connection.RetrieveData(@"
                select 
	                Meter.* 
                from 
	                meter JOIN 
	                AdditionalFieldValue ON Meter.ID = AdditionalFieldValue.ParentTableID AND AdditionalFieldValue.AdditionalFieldID = (SELECT TOP 1 ID FROM AdditionalField WHERE ParentTable= 'Meter' AND FieldName = 'SapphireID')

                "));
            }
        }
        [HttpGet, Route("Sapphire/{id:int}")]
        public IHttpActionResult GetSapphireIDForMeter(int id)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                return Ok(connection.ExecuteScalar<string>(@"
                    select 
	                    AdditionalFieldValue.Value 
                    from 
	                    meter JOIN 
	                    AdditionalFieldValue ON Meter.ID = AdditionalFieldValue.ParentTableID AND AdditionalFieldValue.AdditionalFieldID = (SELECT TOP 1 ID FROM AdditionalField WHERE ParentTable= 'Meter' AND FieldName = 'SapphireID')
                    WHERE Meter.ID = {0}
                ", id));
            }
        }

        [HttpGet, Route("Sapphire/Asset/{id:int}")]
        public IHttpActionResult GetAssetForSapphireMeter(int id)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                return Ok(new TableOperations<Asset>(connection).QueryRecordWhere("ID = (SELECT TOP 1 AssetID FROM MeterAsset WHERE MeterID = {0})", id));
            }
        }

        [HttpGet, Route("Sapphire/Station/{id:int}")]
        public IHttpActionResult GetStationForSapphireMeter(int id)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                return Ok(new TableOperations<Location>(connection).QueryRecordWhere("ID = (SELECT LocationID FROM Meter WHERE ID = {0})", id));
            }
        }

        [HttpGet, Route("Sapphire/Events/{id:int}/{startTime}/{endTime}")]
        public IHttpActionResult GetEventsForSapphireMeter(int id, string startTime, string endTime)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                return Ok(new TableOperations<Event>(connection).QueryRecordsWhere("MeterID ={0} AND StartTime BETWEEN {1} AND {2}", id, startTime, endTime).ToList());
            }
        }

    }

    [RoutePrefix("api/Channel")]
    public class ChannelController : ModelController<ChannelDetail>
    {
        [Route("TrenDAP"), HttpPost]
        public DataTable GetTrendSearchData([FromBody] JObject postData)
        {
            using (AdoDataConnection connection = new AdoDataConnection(Connection))
            {
                string phaseFilter = GetIDFilter((JArray) postData["Phases"], "Phase.ID");
                string channelGroupFilter = GetIDFilter((JArray) postData["ChannelGroups"], "ChannelGroup.ID");
                string assetFilter = GetIDFilter((JArray) postData["AssetList"], "Asset.ID");
                string meterFilter = GetIDFilter((JArray) postData["MeterList"], "Meter.ID");

                if (string.IsNullOrEmpty(phaseFilter) || 
                    string.IsNullOrEmpty(channelGroupFilter) || 
                    string.IsNullOrEmpty(meterFilter)) return new DataTable();

                string filters =
                    $@"Channel.Trend = 1
                    {(string.IsNullOrEmpty(phaseFilter) ? "" : $"AND ({phaseFilter})")}
                    {(string.IsNullOrEmpty(channelGroupFilter) ? "" : $"AND ({channelGroupFilter})")}
                    {(string.IsNullOrEmpty(meterFilter) ? "" : $"AND {meterFilter}")}
                    {(string.IsNullOrEmpty(assetFilter) ? "" : $"AND {assetFilter}")}";

                string query =
                    $@"SELECT
                        Channel.ID,
                        Meter.ID as MeterID,
                        Meter.Name as Meter,
                        Asset.AssetName as Asset,
                        Asset.ID as AssetID,
                        Phase.Name as Phase,
                        Channel.Name,
                        Channel.Adder,
                        Channel.Multiplier,
                        Channel.SamplesPerHour,
                        Channel.PerUnitValue,
                        Channel.HarmonicGroup,
                        Channel.Description,
                        Channel.Enabled,
                        Channel.ConnectionPriority,
                        Channel.Trend,
                        ChannelGroup.Name as ChannelGroup,
                        ChannelGroupType.Unit as Unit,
                        Location.Longitude,
                        Location.Latitude
                    FROM 
	                    Channel LEFT JOIN
	                    Phase ON Phase.ID = Channel.PhaseID LEFT JOIN
	                    Asset ON Asset.ID = Channel.AssetID LEFT JOIN
	                    Meter ON Meter.ID = Channel.MeterID LEFT JOIN
                        Location ON Meter.LocationID = Location.ID LEFT JOIN
	                    ChannelGroupType ON Channel.MeasurementCharacteristicID = ChannelGroupType.MeasurementCharacteristicID AND 
                                            Channel.MeasurementTypeID = ChannelGroupType.MeasurementTypeID LEFT JOIN
	                    ChannelGroup ON ChannelGroup.ID = ChannelGroupType.ChannelGroupID
                    WHERE
	                    {filters}";

                DataTable table = connection.RetrieveData(query);

                return table;
            }
        }

        private string GetIDFilter(JArray idObjectList, string fieldName)
        {
            if (idObjectList is null) return null;
            IEnumerable<int> ids = idObjectList.Values<int>();
            if (!ids.Any()) return null;
            return $"{fieldName} IN ({string.Join(", ", ids)})";
        }
    }

    [RoutePrefix("api/Asset")]
    public class AssetController : ModelController<Asset> { }

    [RoutePrefix("api/DetailedAsset")]
    public class DetailedAssetController : ModelController<DetailedAsset> { }

    [RoutePrefix("api/DetailedLocation")]
    public class DetailedLocationController : ModelController<DetailedLocation> { }

    [RoutePrefix("api/AssetGroup")]
    public class AssetGroupController : ModelController<AssetGroup> { }

    [RoutePrefix("api/Phase")]
    public class PhaseController : ModelController<Phase> {}

    [RoutePrefix("api/EventType")]
    public class EventTypeController : ModelController<EventType> {}

    [RoutePrefix("api/StandardMagDurCurve")]
    public class StandardMagDurCurveController : ModelController<StandardMagDurCurve> {}

    [RoutePrefix("api/Event")]
    public class EventController : ModelController<Event>
    {
        public class Phases
        {
            public bool AN { get; set; }
            public bool BN { get; set; }
            public bool CN { get; set; }
            public bool AB { get; set; }
            public bool BC { get; set; }
            public bool CA { get; set; }
            public bool ABG { get; set; }
            public bool BCG { get; set; }
            public bool ABC { get; set; }
            public bool ABCG { get; set; }
        }
        public class EventCharacteristicFilter
        {
            public Phases phases;
            public int? curveID { get; set; }
            public bool curveInside { get; set; }
            public bool curveOutside { get; set; }
            public int? durationMin { get; set; }
            public int? durationMax { get; set; }
            public int? transientMin { get; set; }
            public int? transientMax { get; set; }
            public string transientType { get; set; }
            public int? sagMin { get; set; }
            public int? sagMax { get; set; }
            public string sagType { get; set; }
            public int? swellMin { get; set; }
            public int? swellMax { get; set; }
            public string swellType { get; set; }
        }

        public class EventPost
        {
            public DateTime StartTime { get; set; }
            public DateTime EndTime { get; set; }
            public EventCharacteristicFilter EventCharacteristicFilter { get; set; }
            public List<int> Types { get; set; }
            public List<int> AssetIDs { get; set; }
            public List<int> MeterIDs { get; set; }
            public List<int> AssetGroupIDs { get; set; }
            public List<int> SubstationIDs { get; set; }
            public List<TimeFilter> TimeFilters { get; set; }
        }

        [HttpPost, Route("TrenDAP")]
        public DataTable GetEventsForTrenDAP([FromBody] EventPost postData)
        {
            using (AdoDataConnection connection = new AdoDataConnection(Connection))
            {
                string phaseFilter = getPhaseFilter(postData.EventCharacteristicFilter);
                string typeFilter = getEventTypeFilter(postData);
                string charFilter = getEventCharacteristicFilter(postData.EventCharacteristicFilter);
                string byFilter = getAssetFilters(postData);
                string timeFilter = getTimeExclusionFilter(postData);
                // Types must be selected
                if (string.IsNullOrEmpty(typeFilter)) return new DataTable();

                string filters =
                    $@"Event.StartTime BETWEEN {{0}} AND {{1}}
                    {(string.IsNullOrEmpty(phaseFilter) ? "" : $" AND ({phaseFilter})")}
                    {(string.IsNullOrEmpty(typeFilter) ? "" : $" AND ({typeFilter})")}
                    {(string.IsNullOrEmpty(byFilter) ? "" : $" AND ({byFilter})")}
                    {(string.IsNullOrEmpty(charFilter) ? "" : $" AND {charFilter}")}
                    {(string.IsNullOrEmpty(timeFilter) ? "" : $" AND {timeFilter}")}";

                string query =
                    $@"SELECT 
                        Event.ID,
                        Event.StartTime,
                        Event.EndTime,
                        Event.AssetID,
                        Event.MeterID,
                        EventType.Name as Type,
                        Asset.AssetName,
                        Meter.Name as MeterName
                    FROM 
                        Event JOIN 
                        EventType ON
                            EventType.ID = Event.EventTypeID INNER JOIN
                        Meter ON
                            Meter.ID = Event.MeterID INNER JOIN
                        Asset ON
                            Asset.ID = Event.AssetID LEFT OUTER JOIN
                        EventWorstDisturbance ON 
                            EventWorstDisturbance.EventID = Event.ID AND 
                            EventType.Name IN ('Sag', 'Swell', 'Interruption', 'Transient') LEFT OUTER JOIN 
                        FaultGroup ON 
                            FaultGroup.EventID = Event.ID AND 
                            COALESCE(FaultGroup.FaultDetectionLogicResult, 0) <> 0 LEFT OUTER JOIN 
                        FaultSummary ON 
                            FaultSummary.EventID = Event.ID AND 
                            FaultSummary.IsSelectedAlgorithm <> 0 AND 
                            ( 
                                FaultGroup.ID IS NOT NULL OR 
                                ( 
                                    FaultSummary.IsValid <> 0 AND 
                                    FaultSummary.IsSuppressed = 0 
                                ) 
                            ) AND 
                            EventType.Name IN ('Fault', 'RecloseIntoFault') 
                    WHERE 
                        ({filters}) AND 
                        ( 
                            EventWorstDisturbance.ID IS NOT NULL OR 
                            FaultSummary.ID IS NOT NULL OR 
                            EventType.Name IN ('BreakerOpen', 'Other') 
                        )";

                DataTable table = connection.RetrieveData(query, postData.StartTime, postData.EndTime);

                return table;
            }
        }

        #region [ Private Methods ]
        private string getEventTypeFilter(EventPost postData)
        {
            if (!postData.Types.Any()) return null;

            return $"Event.EventTypeID IN ({string.Join(",", postData.Types)}) OR  " +
                $"(SELECT d.EventTypeID FROM Disturbance d WHERE d.ID = EventWorstDisturbance.WorstDisturbanceID) IN ({string.Join(",", postData.Types)})";
        }

        private string getPhaseFilter(EventCharacteristicFilter postData)
        {
            Dictionary<string, bool> phases = new Dictionary<string, bool>
            {
                ["AN"] = postData.phases.AN,
                ["BN"] = postData.phases.BN,
                ["CN"] = postData.phases.CN,
                ["AB"] = postData.phases.AB,
                ["BC"] = postData.phases.BC,
                ["CA"] = postData.phases.CA,
                ["ABG"] = postData.phases.ABG,
                ["BCG"] = postData.phases.BCG,
                ["ABC"] = postData.phases.ABC,
                ["ABCG"] = postData.phases.ABCG
            };

            if (!phases.Any(item => !item.Value))   // all are true
            {
                return "";
            }

            if (!phases.Any(item => item.Value))    // all are false
            {
                return "(1=0)";
            }

            string phaseCombined = string.Join(", ", phases.Where(item => item.Value).Select(item => "\'" + item.Key + "\'"));

            return $"(EventWorstDisturbance.WorstDisturbanceID IN (SELECT Disturbance.ID FROM Disturbance WHERE Disturbance.PhaseID IN (Select Phase.ID FROM Phase Where Phase.Name IN ({phaseCombined}))) OR FaultSummary.FaultType IN ({phaseCombined}))";
        }

        private string getEventCharacteristicFilter(EventCharacteristicFilter postData)
        {
            List<string> characteristics = new List<string>();

            //Min and Max Durations
            if (!(postData.durationMin is null) && postData.durationMin > 0)
            {
                string filt = $"((SELECT d.DurationCycles FROM Disturbance d WHERE d.ID = EventWorstDisturbance.WorstDisturbanceID) >= {postData.durationMin} OR ";
                filt += $" FaultSummary.DurationCycles >= {postData.durationMin})";
                characteristics.Add(filt);
            }
            if (!(postData.durationMax is null) && postData.durationMax > 0)
            {
                string filt = $" ((SELECT d.DurationCycles FROM Disturbance d WHERE d.ID = EventWorstDisturbance.WorstDisturbanceID) <= {postData.durationMax} OR";
                filt += $" FaultSummary.DurationCycles <= {postData.durationMax})";
                characteristics.Add(filt);
            }

            // Sag Min and Max
            if (!(postData.sagMin is null) && postData.sagMin > 0)
            {
                string filt;
                if (postData.sagType == "LL")
                    filt = $"((SELECT d.PerUnitMagnitude FROM Disturbance d WHERE d.ID = EventWorstDisturbance.WorstLLDisturbanceID) >= {postData.sagMin} OR (SELECT d.EventTypeID FROM Disturbance d WHERE d.ID = EventWorstDisturbance.WorstDisturbanceID) IN (SELECT ID FROM EventType WHERE Name <> 'Sag'))";
                else if (postData.sagType == "LN")
                    filt = $"((SELECT d.PerUnitMagnitude FROM Disturbance d WHERE d.ID = EventWorstDisturbance.WorstLNDisturbanceID) >= {postData.sagMin} OR (SELECT d.EventTypeID FROM Disturbance d WHERE d.ID = EventWorstDisturbance.WorstDisturbanceID) IN (SELECT ID FROM EventType WHERE Name <> 'Sag'))";
                else
                    filt = $"((SELECT d.PerUnitMagnitude FROM Disturbance d WHERE d.ID = EventWorstDisturbance.WorstLLDisturbanceID) >= {postData.sagMin} OR (SELECT d.PerUnitMagnitude FROM Disturbance d WHERE d.ID = EventWorstDisturbance.WorstLNDisturbanceID) >= {postData.sagMin} OR (SELECT d.EventTypeID FROM Disturbance d WHERE d.ID = EventWorstDisturbance.WorstDisturbanceID) IN (SELECT ID FROM EventType WHERE Name <> 'Sag'))";
                characteristics.Add(filt);
            }
            if (!(postData.sagMax is null) && postData.sagMax > 0)
            {
                string filt;
                if (postData.sagType == "LL")
                    filt = $"((SELECT d.PerUnitMagnitude FROM Disturbance d WHERE d.ID = EventWorstDisturbance.WorstLLDisturbanceID) <= {postData.sagMax} OR (SELECT d.EventTypeID FROM Disturbance d WHERE d.ID = EventWorstDisturbance.WorstDisturbanceID) IN (SELECT ID FROM EventType WHERE Name <> 'Sag'))";
                else if (postData.sagType == "LN")
                    filt = $"((SELECT d.PerUnitMagnitude FROM Disturbance d WHERE d.ID = EventWorstDisturbance.WorstLNDisturbanceID) <= {postData.sagMax} OR (SELECT d.EventTypeID FROM Disturbance d WHERE d.ID = EventWorstDisturbance.WorstDisturbanceID) IN (SELECT ID FROM EventType WHERE Name <> 'Sag'))";
                else
                    filt = $"((SELECT d.PerUnitMagnitude FROM Disturbance d WHERE d.ID = EventWorstDisturbance.WorstLLDisturbanceID) <= {postData.sagMax} OR (SELECT d.PerUnitMagnitude FROM Disturbance d WHERE d.ID = EventWorstDisturbance.WorstLNDisturbanceID) <= {postData.sagMax} OR (SELECT d.EventTypeID FROM Disturbance d WHERE d.ID = EventWorstDisturbance.WorstDisturbanceID) IN (SELECT ID FROM EventType WHERE Name <> 'Sag'))";
                characteristics.Add(filt);
            }

            // Swell Min and Max
            if (!(postData.swellMin is null) && postData.swellMin > 0)
            {
                string filt;
                if (postData.swellType == "LL")
                    filt = $"((SELECT d.PerUnitMagnitude FROM Disturbance d WHERE d.ID = EventWorstDisturbance.WorstLLDisturbanceID) >= {postData.swellMin} OR (SELECT d.EventTypeID FROM Disturbance d WHERE d.ID = EventWorstDisturbance.WorstDisturbanceID) IN (SELECT ID FROM EventType WHERE Name <> 'Swell'))";
                else if (postData.swellType == "LN")
                    filt = $"((SELECT d.PerUnitMagnitude FROM Disturbance d WHERE d.ID = EventWorstDisturbance.WorstLNDisturbanceID) >= {postData.swellMin} OR (SELECT d.EventTypeID FROM Disturbance d WHERE d.ID = EventWorstDisturbance.WorstDisturbanceID) IN (SELECT ID FROM EventType WHERE Name <> 'Swell'))";
                else
                    filt = $"((SELECT d.PerUnitMagnitude FROM Disturbance d WHERE d.ID = EventWorstDisturbance.WorstLLDisturbanceID) >= {postData.swellMin} OR (SELECT d.PerUnitMagnitude FROM Disturbance d WHERE d.ID = EventWorstDisturbance.WorstLNDisturbanceID) >= {postData.swellMin} OR (SELECT d.EventTypeID FROM Disturbance d WHERE d.ID = EventWorstDisturbance.WorstDisturbanceID) IN (SELECT ID FROM EventType WHERE Name <> 'Swell'))";
                characteristics.Add(filt);
            }
            if (!(postData.swellMax is null) && postData.swellMax > 0)
            {
                string filt;
                if (postData.swellType == "LL")
                    filt = $"((SELECT d.PerUnitMagnitude FROM Disturbance d WHERE d.ID = EventWorstDisturbance.WorstLLDisturbanceID) <= {postData.swellMax} OR (SELECT d.EventTypeID FROM Disturbance d WHERE d.ID = EventWorstDisturbance.WorstDisturbanceID) IN (SELECT ID FROM EventType WHERE Name <> 'Swell'))";
                else if (postData.swellType == "LN")
                    filt = $"((SELECT d.PerUnitMagnitude FROM Disturbance d WHERE d.ID = EventWorstDisturbance.WorstLNDisturbanceID) <= {postData.swellMax} OR (SELECT d.EventTypeID FROM Disturbance d WHERE d.ID = EventWorstDisturbance.WorstDisturbanceID) IN (SELECT ID FROM EventType WHERE Name <> 'Swell'))";
                else
                    filt = $"((SELECT d.PerUnitMagnitude FROM Disturbance d WHERE d.ID = EventWorstDisturbance.WorstLLDisturbanceID) <= {postData.swellMax} OR (SELECT d.PerUnitMagnitude FROM Disturbance d WHERE d.ID = EventWorstDisturbance.WorstLNDisturbanceID) <= {postData.swellMax} OR (SELECT d.EventTypeID FROM Disturbance d WHERE d.ID = EventWorstDisturbance.WorstDisturbanceID) IN (SELECT ID FROM EventType WHERE Name <> 'Swell'))";
                characteristics.Add(filt);
            }

            // Transient min and max
            if (!(postData.transientMin is null) && postData.transientMin > 0)
            {
                string filt;
                if (postData.transientType == "LL")
                    filt = $"((SELECT d.PerUnitMagnitude FROM Disturbance d WHERE d.ID = EventWorstDisturbance.WorstLLDisturbanceID) >= {postData.transientMin} OR (SELECT d.EventTypeID FROM Disturbance d WHERE d.ID = EventWorstDisturbance.WorstDisturbanceID) IN (SELECT ID FROM EventType WHERE Name <> 'Transient'))";
                else if (postData.transientType == "LN")
                    filt = $"((SELECT d.PerUnitMagnitude FROM Disturbance d WHERE d.ID = EventWorstDisturbance.WorstLNDisturbanceID) >= {postData.transientMin} OR (SELECT d.EventTypeID FROM Disturbance d WHERE d.ID = EventWorstDisturbance.WorstDisturbanceID) IN (SELECT ID FROM EventType WHERE Name <> 'Transient'))";
                else
                    filt = $"((SELECT d.PerUnitMagnitude FROM Disturbance d WHERE d.ID = EventWorstDisturbance.WorstLLDisturbanceID) >= {postData.transientMin} OR (SELECT d.PerUnitMagnitude FROM Disturbance d WHERE d.ID = EventWorstDisturbance.WorstLNDisturbanceID) >= {postData.transientMin} OR (SELECT d.EventTypeID FROM Disturbance d WHERE d.ID = EventWorstDisturbance.WorstDisturbanceID) IN (SELECT ID FROM EventType WHERE Name <> 'Transient'))";
                characteristics.Add(filt);
            }
            if (!(postData.transientMax is null) && postData.transientMax > 0)
            {
                string filt;
                if (postData.transientType == "LL")
                    filt = $"((SELECT d.PerUnitMagnitude FROM Disturbance d WHERE d.ID = EventWorstDisturbance.WorstLLDisturbanceID) <= {postData.transientMax} OR (SELECT d.EventTypeID FROM Disturbance d WHERE d.ID = EventWorstDisturbance.WorstDisturbanceID) IN (SELECT ID FROM EventType WHERE Name <> 'Transient'))";
                else if (postData.transientType == "LN")
                    filt = $"((SELECT d.PerUnitMagnitude FROM Disturbance d WHERE d.ID = EventWorstDisturbance.WorstLNDisturbanceID) <= {postData.transientMax} OR (SELECT d.EventTypeID FROM Disturbance d WHERE d.ID = EventWorstDisturbance.WorstDisturbanceID) IN (SELECT ID FROM EventType WHERE Name <> 'Transient'))";
                else
                    filt = $"((SELECT d.PerUnitMagnitude FROM Disturbance d WHERE d.ID = EventWorstDisturbance.WorstLLDisturbanceID) <= {postData.transientMax} OR (SELECT d.PerUnitMagnitude FROM Disturbance d WHERE d.ID = EventWorstDisturbance.WorstLNDisturbanceID) <= {postData.transientMax} OR (SELECT d.EventTypeID FROM Disturbance d WHERE d.ID = EventWorstDisturbance.WorstDisturbanceID) IN (SELECT ID FROM EventType WHERE Name <> 'Transient'))";
                characteristics.Add(filt);
            }

            // Mag Dur Curves
            if (!(postData.curveID is null) && postData.curveID != -1)
            {
                string filt = $"( (SELECT d.DurationSeconds FROM Disturbance d WHERE d.ID = WorstDisturbanceID) IS NOT NULL AND (SELECT d.PerUnitMagnitude FROM Disturbance d WHERE d.ID = WorstDisturbanceID) IS NOT NULL AND (SELECT TOP 1 Area FROM StandardMagDurCurve WHERE ID = {postData.curveID})";
                filt += $".STContains(geometry::Point((SELECT d.DurationSeconds FROM Disturbance d WHERE d.ID = WorstDisturbanceID),(SELECT d.PerUnitMagnitude FROM Disturbance d WHERE d.ID = WorstDisturbanceID),0)) = {(postData.curveInside ? 1 : 0)})";
                characteristics.Add(filt);
            }

            return string.Join(" AND ", characteristics);
        }

        private string getAssetFilters(EventPost postData)
        {
            List<string> assets = new List<string>();

            if (postData.MeterIDs.Count() > 0)
                assets.Add($"Event.MeterID IN ({string.Join(",", postData.MeterIDs)})");

            if (postData.AssetIDs.Count() > 0)
                assets.Add($"Event.AssetID IN ({string.Join(",", postData.AssetIDs)})");

            if (postData.SubstationIDs.Count() > 0)
            {
                string filt = $"(Event.AssetID IN (SELECT AssetLocation.AssetID FROM AssetLocation WHERE AssetLocation.LocationID IN ({string.Join(",", postData.SubstationIDs)}))";
                filt += $" OR Event.MeterID IN (SELECT Meter.ID FROM Meter WHERE Meter.LocationID IN ({string.Join(",", postData.SubstationIDs)})))";
                assets.Add(filt);
            }

            if (postData.AssetGroupIDs.Count() > 0)
            {
                string filt = $"(Event.AssetID IN (SELECT AssetAssetGroup.AssetID FROM AssetAssetGroup WHERE AssetAssetGroup.AssetGroupID IN ({string.Join(",", postData.AssetGroupIDs)}))";
                filt += $" OR Event.MeterID IN (SELECT MeterAssetGroup.MeterID FROM MeterAssetGroup WHERE MeterAssetGroup.AssetGroupID IN ({string.Join(",", postData.AssetGroupIDs)})))";
                assets.Add(filt);
            }

            return string.Join(" AND ", assets);
        }

        private string getTimeExclusionFilter(EventPost postData)
        {
            IEnumerable<int> hours = postData.TimeFilters.Where(filt => filt < TimeFilter.Sunday).Select(filt => (int) filt);
            // SQL dw is 1-7, starting on sunday
            IEnumerable<int> days = postData.TimeFilters.Where(filt => filt >= TimeFilter.Sunday && filt < TimeFilter.Week00).Select(filt => filt - TimeFilter.Sunday + 1);
            // Influx uses the iso week standard, i.e. 1-53
            IEnumerable<int> weeks = postData.TimeFilters.Where(filt => filt >= TimeFilter.Week00 && filt < TimeFilter.January).Select(filt => filt - TimeFilter.Week00 + 1);
            IEnumerable<int> months = postData.TimeFilters.Where(filt => filt >= TimeFilter.January).Select(filt => filt - TimeFilter.January + 1);

            string hourFilt = hours.Any() ? $"DATEPART(hh,Event.StartTime) not in ({string.Join(",", hours)})" : null;
            string dayFilt = days.Any() ? $"DATEPART(dw, Event.StartTime) not in ({string.Join(",", days)})" : null;
            string weekFilt = weeks.Any() ? $"DATEPART(isowk, Event.StartTime) not in ({string.Join(",", weeks)})" : null;
            string monthFilt = months.Any() ? $"DATEPART(mm, Event.StartTime) not in ({string.Join(",", months)})" : null;

            string[] filters = { hourFilt, dayFilt, weekFilt, monthFilt };

            return string.Join(" AND ", filters.Where(filt => !(filt is null)));
        }
        private string getIDFilter(List<int> ids, string fieldName)
        {
            if (ids is null) return null;
            if (!ids.Any()) return null;
            return $"{fieldName} IN ({string.Join(", ", ids)})";
        }
        #endregion
    }

    [RoutePrefix("api/MeasurementType")]
    public class MeasurementTypeController : ModelController<MeasurementType> {}

    [RoutePrefix("api/MeasurementCharacteristic")]
    public class MeasurementCharacteristicController : ModelController<MeasurementCharacteristic> {}

    [RoutePrefix("api/ChannelGroup")]
    public class ChannelGroupController : ModelController<ChannelGroup> {}

    [RoutePrefix("api/ChannelGroupType")]
    public class ChannelGroupTypeController : ModelController<ChannelGroupType> {}

    [RoutePrefix("api/ChannelGroupDetails")]
    public class ChannelGroupDetailsController : ModelController<ChannelGroupDetails> {}

    [RoutePrefix("api/AlarmType")]
    public class AlarmTypeController : ModelController<AlarmType> {}

    [RoutePrefix("api/MeterDetail")]
    public class MeterDetailController : ModelController<MeterDetail> {}

    [RoutePrefix("api/AlarmDay")]
    public class AlarmDayController : ModelController<AlarmDay> {}

    [RoutePrefix("api/AlarmDayGroup")]
    public class AlarmDayGroupController : ModelController<AlarmDayGroupView> {}

    [RoutePrefix("api/SeriesType")]
    public class SeriesTypeController : ModelController<SeriesType> {}

    [RoutePrefix("api/Setting")]
    public class SettingController : ModelController<openXDA.Model.Setting>
    {

        [Route("Category/{category}")]
        public IHttpActionResult GetSettingCategory(string category)
        {
            using (AdoDataConnection connection = new AdoDataConnection(Connection))
            {
                return Ok(connection.RetrieveData("SELECT * FROM Setting WHERE Name LIKE {0}", category + "%"));
            }
        }
        [Route("{setting}")]
        public HttpResponseMessage GetSetting(string setting)
        {
            using (AdoDataConnection connection = new AdoDataConnection(Connection))
            {
                string value = connection.ExecuteScalar<string>(1000, "SELECT Value FROM Setting WHERE Name = {0}", setting);
                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                result.Content = new StringContent(value);
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("text/pain");
                return result;
            }
        }
    }

    [RoutePrefix("api/ValueList")]
    public class ValueListController : ModelController<ValueList>
    {
        [HttpGet, Route("Group/{groupName}")]
        public IHttpActionResult GetValueListForGroup(string groupName)
        {
            using (AdoDataConnection connection = new AdoDataConnection(Connection))
            {
                string tableName = new TableOperations<ValueListGroup>(connection).TableName;
                IEnumerable<ValueList> records = new TableOperations<ValueList>(connection).QueryRecordsWhere($"GroupID = ( SELECT ID FROM {tableName} WHERE Name = {{0}})", groupName);
                return Ok(records);
            }
        }
    }

    [RoutePrefix("api")]
    public class GeneralController : APIPingController
    {
        [HttpGet, Route("TestAuth")]
        public IHttpActionResult Alive()
        {
            return Ok(1);
        }

        [HttpGet, Route("TileList/GetAll")]
        public IHttpActionResult GetAllTiles()
        {
            string directoryPath = FilePath.GetAbsolutePath("wwwroot\\Images\\Tiles");
            string[] tileFiles = System.IO.Directory.GetFiles(directoryPath);
            IEnumerable<string> tileList = tileFiles.Select(file => ".\\" + file.Split(new string[] { "wwwroot\\" }, StringSplitOptions.None)[1]).ToList();
            return Ok(tileList);
        }
    }

    public class XDAEndController<T> : ModelController<T> where T: class, new()
    {
        public override IHttpActionResult GetSearchableList([FromBody] PostData postData)
        {
            if (GetAuthCheck() && !AllowSearch)
                return Unauthorized();

            try
            {
                DataTable table = GetSearchResults(postData);
                return Ok(table);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
    [RoutePrefix("api/Customer")]
    public class CustomerController : XDAEndController<SystemCenter.Model.Customer> { }

    [RoutePrefix("api/DetailedMeter")]
    public class DetailedMeterController : XDAEndController<SystemCenter.Model.DetailedMeter> { }


    [RoutePrefix("api/CustomerMeter")]
    public class CustomerMeterController : XDAEndController<SystemCenter.Model.CustomerMeter> { }

    [RoutePrefix("api/Console")]
    public class ConsoleController : APIConsoleController {
        protected override IAPIConsoleHost Host => ServiceConnection.Default;
    }

    [RoutePrefix("api/FTT")]
    public class FTTController : ApiController
    {
        [HttpGet, Route("XDAEventList")]
        public IHttpActionResult GetEventList([FromUri] string startdate, [FromUri] string enddate)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                string sqlQuery = @"SELECT     
                    Event.ID as eventID,
                    FaultSummary.Inception as event_datetime,
                    Location.ShortName as substation,
                    try_parse(Asset.AssetKey as int) as lineID,
                    Asset.AssetName as line,
                    CASE WHEN FaultSummary.Distance = '-1E308'
                        THEN 'NaN'
                    ELSE CAST(CAST(FaultSummary.Distance AS DECIMAL(16,2)) AS NVARCHAR(19))
                    END AS distance_miles
                FROM FaultSummary JOIN
                     Event ON FaultSummary.EventID = Event.ID JOIN
                     EventType ON Event.EventTypeID = EventType.ID JOIN
                     Meter ON Event.MeterID = Meter.ID JOIN
                     Location ON Meter.LocationID = Location.ID JOIN
                     Asset ON Event.AssetID = Asset.ID     
                WHERE      
                    IsSelectedAlgorithm = 1 AND  EventType.Name = 'Fault' AND Inception BETWEEN {0} AND {1}
                ";

                return Ok(connection.RetrieveData(sqlQuery, startdate, enddate));
            }
        }
    }
}