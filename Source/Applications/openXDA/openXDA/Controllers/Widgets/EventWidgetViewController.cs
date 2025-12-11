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
using System.Web.Http;
using GSF.Data;
using GSF.Data.Model;
using GSF.Web.Model;
using Newtonsoft.Json.Linq;
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
        public class AggregateCountQuery
        {
            public DateTime StartTime { get; set; }
            public DateTime EndTime { get; set; }
            public IEnumerable<int> MeterIDs { get; set; }
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

        [HttpPost, Route("EventCountAggregate")]
        public IHttpActionResult EventCountByAggregate([FromBody] JObject query)
        {
            AggregateCountQuery postData = query.ToObject<AggregateCountQuery>();
            using (AdoDataConnection connection = ConnectionFactory())
            {
                IEnumerable<string> types = new TableOperations<EventType>(connection)
                    .QueryRecords()
                    .Select(type => type.Name);

                string meterFilter = postData.MeterIDs.Any() ? $" AND Meter.ID IN ({string.Join(",", postData.MeterIDs)})" : "";

                string timeSpan;
                switch (postData.Granularity.ToUpper())
                {
                    case "HOURLY":
                        timeSpan = "HOUR";
                        break;
                    case "WEEKLY":
                        timeSpan = "WEEK";
                        break;
                    default:
                    case "MONTHLY":
                        timeSpan = "MONTH";
                        break;
                    case "DAILY":
                        timeSpan = "DAY";
                        break;
                    case "YEARLY":
                        timeSpan = "YEAR";
                        break;
                }

                string sql = $@"
                    WITH EventCTE AS (
	                    SELECT
		                    COUNT(Event.ID) as Count,
		                    EventType.Name as EventType,
		                    DATETRUNC({timeSpan}, Event.StartTime) as aggTime
	                    FROM
		                    Event JOIN
		                    EventType ON Event.EventTypeID = EventType.ID	
	                    WHERE
		                    EventType.Name IN ({string.Join(",", types.Select((_type, index) => $"{{{index + 2}}}"))})
                            AND Event.StartTime BETWEEN {{0}} AND {{1}} 
                            {meterFilter}
	                    GROUP BY
		                    EventType.Name,
		                    DATETRUNC({timeSpan}, Event.StartTime)
                    ),
	                Joined AS(
		                SELECT
                            EventCTE.aggTime,
			                COALESCE(EventCTE.EventType, 'None') EventType,
			                COALESCE(EventCTE.Count, 0) Count
		                FROM
			                EventCTE
	                )
                    SELECT
                        aggTime,
                        {string.Join(",", types.Select(type => $"COALESCE({type},0) as {type}"))}
                    FROM
	                    Joined
                    PIVOT
                    (
	                    SUM(Count) FOR EventType
                        IN ({string.Join(",", types)})
                    ) pvt 
                    ORDER BY aggTime ASC";

                object[] paramsArray = new object[] { postData.StartTime, postData.EndTime }.Concat(types).ToArray();
                DataTable table = connection.RetrieveData(sql, paramsArray);

                return Ok(table);
            }
        }
    }
}
