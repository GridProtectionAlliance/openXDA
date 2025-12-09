//******************************************************************************************************
//  EventWidgetViewController.cs - Gbtc
//
//  Copyright � 2020, Grid Protection Alliance.  All Rights Reserved.
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
//  10/28/2025 - G. Santos
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;
using GSF.Data;
using GSF.Data.Model;
using GSF.Web.Model;
using Newtonsoft.Json.Linq;
using openXDA.HIDS.APIExtensions;
using openXDA.Model;

namespace openXDA.Controllers.Widgets
{
    /// <summary>
    /// Controller that handles fetching and searching of the <see cref="EventWidgetsEventView"/> model.
    /// </summary>
    [RoutePrefix("api/Widgets/EventView")]
    public class EventWidgetViewController : ModelController<EventWidgetsEventView> 
    {
        public class CountQuery
        {
            public DateTime StartTime { get; set; }
            public DateTime EndTime { get; set; }
            public IEnumerable<int> MeterIDs { get; set; }
        }
        public class AggregateCountQuery : CountQuery
        {
            public string Granularity { get; set; }
        }

        [HttpPost, Route("EventCount")]
        public IHttpActionResult EventCount([FromBody] JObject query)
        {
            CountQuery postData = query.ToObject<CountQuery>();
            using (AdoDataConnection connection = ConnectionFactory())
            {
                IEnumerable<string> types = new TableOperations<EventType>(connection)
                    .QueryRecords()
                    .Select(type => type.Name);

                string meterFilter = postData.MeterIDs.Any() ? $" AND Meter.ID IN ({string.Join(",", postData.MeterIDs)})" : "";

                string sql = $@"
                    WITH EventCTE AS (
	                    SELECT
		                    COUNT(Event.ID) as Count,
		                    EventType.Name as EventType,
                            Meter.ID as ID,
		                    Meter.Name as Meter
	                    FROM
		                    Meter CROSS JOIN
		                    EventType LEFT JOIN
		                    Event ON Event.MeterID =  Meter.ID AND  Event.EventTypeID = EventType.ID AND Event.StartTime BETWEEN {{0}} AND {{1}}
	                    WHERE
		                    EventType.Name IN ({string.Join(",", types.Select((_type, index) => $"{{{index + 2}}}"))})
                            {meterFilter}
	                    GROUP BY
		                    Meter.Name, EventType.Name, Meter.ID
                    )
                    SELECT
                        ID,
	                    Meter as Name,
                        {string.Join(",", types.Select(type => $"COALESCE({type},0) as {type}"))}
                    FROM
	                    EventCTE
                    PIVOT
                    (
	                    SUM(EventCTE.Count) FOR EventType
	                    IN ({string.Join(",", types)})
                    ) pvt
                    ORDER BY Meter
                ";
                object[] paramsArray = new object[]{ postData.StartTime, postData.EndTime }.Concat(types).ToArray();
                DataTable table = connection.RetrieveData(sql, paramsArray);

                return Ok(table);
            }
        }

        [HttpPost, Route("EventCountByMonth")]
        public IHttpActionResult EventCountByMonth([FromBody] JObject query)
        {
            AggregateCountQuery postData = query.ToObject<AggregateCountQuery>();
            using (AdoDataConnection connection = ConnectionFactory())
            {
                IEnumerable<string> types = new TableOperations<EventType>(connection)
                    .QueryRecords()
                    .Select(type => type.Name);

                string meterFilter = postData.MeterIDs.Any() ? $" AND Meter.ID IN ({string.Join(",", postData.MeterIDs)})" : "";

                string aggUnit;
                string timeSpan;
                switch (postData.Granularity.ToUpper())
                {
                    // ToDo: Hourly is a special case that will require a bit more work
                    case "HOURLY":
                    default:
                    case "MONTHLY":
                        aggUnit = "MONTH";
                        timeSpan = "MONTH";
                        break;
                    // Special notes about weekly, the first week and last week of the year may be less than 7 days due to ISO week numbering specifics
                    case "WEEKLY":
                        aggUnit = "ISOWK";
                        timeSpan = "WEEK";
                        break;
                    case "DAILY":
                        aggUnit = "DAYOFYEAR";
                        timeSpan = "DAY";
                        break;
                    case "YEARLY":
                        timeSpan = "YEAR";
                        aggUnit = null;
                        break;
                }

                string sql = $@"
                    WITH EventCTE AS (
	                    SELECT
		                    COUNT(Event.ID) as Count,
		                    EventType.Name as EventType,
		                    {(aggUnit is null ? "" : $"DATEPART({aggUnit}, Event.StartTime) as SInt,")}
		                    DATEPART(YEAR, Event.StartTime) as YInt
	                    FROM
		                    Event JOIN
		                    EventType ON Event.EventTypeID = EventType.ID	
	                    WHERE
		                    EventType.Name IN ('Sag','Fault','RecloseIntoFault','BreakerOpen','Interruption','Swell','Transient','Test','Snapshot','Other')
		                    AND Event.StartTime BETWEEN '12/1/2024 12:00:00 AM' AND '12/1/2025 12:00:00 AM'
                            
	                    GROUP BY
		                    EventType.Name, {(aggUnit is null ? "" : $"DATEPART({aggUnit}, Event.StartTime),")} DATEPART(YEAR, Event.StartTime)
                    ),
	                DateTally AS (
	                SELECT CONVERT(DATETIME,'12/1/2024 12:00:00 AM') Dt
	                UNION ALL
	                SELECT DATEADD({timeSpan},1,Dt) FROM DateTally WHERE Dt < CONVERT(DATETIME,'12/1/2025 12:00:00 AM')
	                ), 
	                DateTallyR AS(
	                SELECT 
		                {(aggUnit is null ? "" : $"DATEPART({aggUnit},Dt) as SInt,")} DATEPART(YEAR,Dt) as YInt 
	                FROM 
		                DateTally 
	                ),
	                Joined AS(
		                SELECT
			                DateTallyR.YInt,
			                COALESCE(EventCTE.EventType, 'None') EventType,
			                COALESCE(EventCTE.Count, 0) Count,
			                {(aggUnit is null ? "" : "DateTallyR.SInt")}
		                FROM
			                DateTallyR LEFT JOIN
			                {(aggUnit is null ? "" : "EventCTE ON DateTallyR.SInt = EventCTE.SInt AND ")}DateTallyR.YInt = EventCTE.YInt
	                )
                    SELECT
                        YInt,
	                    {(aggUnit is null ? "" : "SInt, ")}
                        COALESCE(Fault,0) as Fault,COALESCE(RecloseIntoFault,0) as RecloseIntoFault,COALESCE(BreakerOpen,0) as BreakerOpen,COALESCE(Interruption,0) as Interruption,COALESCE(Sag,0) as Sag,COALESCE(Swell,0) as Swell,COALESCE(Transient,0) as Transient,COALESCE(Other,0) as Other,COALESCE(Test,0) as Test,COALESCE(Snapshot,0) as Snapshot
                    FROM
	                    Joined
                    PIVOT
                    (
	                    SUM(Count) FOR EventType
	                    IN (Fault,RecloseIntoFault,BreakerOpen,Interruption,Sag,Swell,Transient,Other,Test,Snapshot)
                    ) pvt 
                    ORDER BY YInt{(aggUnit is null ? "" : ", SInt")}";

                object[] paramsArray = new object[] { postData.StartTime, postData.EndTime }.Concat(types).ToArray();
                DataTable table = connection.RetrieveData(sql, paramsArray);

                return Ok(table);
            }
        }
    }
}
