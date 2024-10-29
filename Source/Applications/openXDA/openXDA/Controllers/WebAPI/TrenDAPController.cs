//******************************************************************************************************
//  TrenDAPController.cs - Gbtc
//
//  Copyright © 2024, Grid Protection Alliance.  All Rights Reserved.
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
//  10/29/2024 - Gabriel Santos
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Http;
using GSF.Data;
using HIDS;
using log4net;

namespace openXDA.Controllers.WebAPI
{

    [RoutePrefix("api/TrenDAP")]
    public class TrenDAPController : ApiController {
        // Static Fields
        private static readonly ILog Log = LogManager.GetLogger(typeof(TrenDAPController));
        protected string Connection { get; } = "systemSettings";

        #region [ Classes ]
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

        public class AlarmPost
        {
            public DateTime StartTime { get; set; }
            public DateTime EndTime { get; set; }
            public List<int> SevertiesIDs { get; set; }
            public List<int> AlarmGroupIDs { get; set; }
            public List<int> MeterIDs { get; set; }
            public List<int> AlarmTypeIDs { get; set; }
            public List<TimeFilter> TimeFilters { get; set; }
        }

        #endregion

        [HttpPost, Route("Event")]
        public DataTable GetEventsForTrenDAP([FromBody] EventPost postData)
        {
            using (AdoDataConnection connection = new AdoDataConnection(Connection))
            {
                string phaseFilter = getPhaseFilter(postData.EventCharacteristicFilter);
                string typeFilter = getEventTypeFilter(postData);
                string charFilter = getEventCharacteristicFilter(postData.EventCharacteristicFilter);
                string byFilter = getAssetFilters(postData);
                string timeFilter = getTimeExclusionFilter(postData.TimeFilters, "Event.StartTime");
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

        [Route("AlarmLog"), HttpPost]
        public DataTable GetTrendSearchData([FromBody] AlarmPost postData)
        {
            using (AdoDataConnection connection = new AdoDataConnection(Connection))
            {
                string severityFilter = getIDFilter(postData.SevertiesIDs, "AlarmLog.SeverityID");
                string alarmGroupFilter = getIDFilter(postData.AlarmGroupIDs, "AlarmGroup.ID");
                string meterFilter = getIDFilter(postData.MeterIDs, "Meter.ID");
                string alarmTypeFilter = getIDFilter(postData.MeterIDs, "AlarmType.ID");
                string timeFilter = getTimeExclusionFilter(postData.TimeFilters, "AlarmLog.StartTime");

                // Severities must be selected
                if (string.IsNullOrEmpty(severityFilter)) return new DataTable();

                string filters =
                    $@"AlarmLog.StartTime BETWEEN {{0}} AND {{1}}
                    {(string.IsNullOrEmpty(severityFilter) ? "" : $" AND {severityFilter}")}
                    {(string.IsNullOrEmpty(alarmGroupFilter) ? "" : $" AND {alarmGroupFilter}")}
                    {(string.IsNullOrEmpty(meterFilter) ? "" : $" AND {meterFilter}")}
                    {(string.IsNullOrEmpty(alarmTypeFilter) ? "" : $" AND {alarmTypeFilter}")}
                    {(string.IsNullOrEmpty(timeFilter) ? "" : $" AND {timeFilter}")}";

                string query =
                    $@"SELECT 
	                    Meter.Name as MeterName,
	                    AlarmLog.StartTime,
	                    AlarmLog.EndTime,
	                    AlarmSeverity.Name as AlarmSeverity,
	                    AlarmSeverity.Color as AlarmSeverityColor,
	                    AlarmGroup.Name as AlarmGroupName,
                        AlarmType.Name as AlarmTypeName
                    FROM
	                    AlarmLog LEFT JOIN 
	                    Alarm ON Alarm.ID = AlarmLog.AlarmID LEFT JOIN
	                    AlarmSeverity ON AlarmSeverity.ID = AlarmLog.SeverityID LEFT JOIN
	                    AlarmGroup ON AlarmGroup.ID = Alarm.AlarmGroupID LEFT JOIN
	                    Series ON Alarm.SeriesID = Series.ID LEFT JOIN
	                    Channel ON Series.ChannelID = Channel.ID LEFT JOIN
	                    Meter ON Channel.MeterID = Meter.ID LEFT JOIN
                        AlarmType ON AlarmType.ID = AlarmGroup.AlarmTypeID
                    WHERE
	                    {filters}";

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

        private string getTimeExclusionFilter(List<TimeFilter> timeFilters, string fieldName)
        {
            IEnumerable<int> hours = timeFilters.Where(filt => filt < TimeFilter.Sunday).Select(filt => (int)filt);
            // SQL dw is 1-7, starting on sunday
            IEnumerable<int> days = timeFilters.Where(filt => filt >= TimeFilter.Sunday && filt < TimeFilter.Week00).Select(filt => filt - TimeFilter.Sunday + 1);
            // Influx uses the iso week standard, i.e. 1-53
            IEnumerable<int> weeks = timeFilters.Where(filt => filt >= TimeFilter.Week00 && filt < TimeFilter.January).Select(filt => filt - TimeFilter.Week00 + 1);
            IEnumerable<int> months = timeFilters.Where(filt => filt >= TimeFilter.January).Select(filt => filt - TimeFilter.January + 1);

            string hourFilt = hours.Any() ? $"DATEPART(hh,{fieldName}) not in ({string.Join(",", hours)})" : null;
            string dayFilt = days.Any() ? $"DATEPART(dw, {fieldName}) not in ({string.Join(",", days)})" : null;
            string weekFilt = weeks.Any() ? $"DATEPART(isowk, {fieldName}) not in ({string.Join(",", weeks)})" : null;
            string monthFilt = months.Any() ? $"DATEPART(mm, {fieldName}) not in ({string.Join(",", months)})" : null;

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
}