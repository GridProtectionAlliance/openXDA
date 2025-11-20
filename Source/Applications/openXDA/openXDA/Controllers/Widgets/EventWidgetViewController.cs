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
		                    CONVERT(varchar(3), DATENAME(month,Cast(Event.StartTime as Date))) as Month,
		                    Month(Event.StartTime) as MonthInt,
		                    Year(Event.StartTime) as Year
	                    FROM
		                    Event JOIN
		                    EventType ON Event.EventTypeID = EventType.ID	
	                    WHERE
		                    EventType.Name IN ({string.Join(",", types.Select((_type, index) => $"{{{index + 2}}}"))})
		                    AND Event.StartTime BETWEEN {{0}} AND {{1}} 
                            {meterFilter}
	                    GROUP BY
		                    CONVERT(varchar(3), DATENAME(month,Cast(Event.StartTime as Date))), EventType.Name, Month(Event.StartTime), Year(Event.StartTime)
                    ),
					DateTally AS (
					SELECT CONVERT(DATETIME,{{0}}) Dt
					UNION ALL
					SELECT DATEADD(MM,1,Dt) FROM DateTally WHERE Dt < CONVERT(DATETIME,{{1}})
					), 
					DateTallyR AS(
					SELECT 
					  Month(Dt) as MonthInt, LEFT(DATENAME(MM,Dt),3) as Month,CONVERT(VARCHAR,YEAR(Dt)) as Year 
					FROM 
					  DateTally 
					),
					Joined AS(
						SELECT
							DateTallyR.Month,
							DateTallyR.Year,
							COALESCE(EventCTE.EventType, 'None') EventType,
							COALESCE(EventCTE.Count, 0) Count,
							DateTallyR.MonthInt
						FROM
							DateTallyR LEFT JOIN
							EventCTE ON DateTallyR.Month = EventCTE.Month AND DateTallyR.Year = EventCTE.Year
					)
                    SELECT
					 --*
                        Year,
	                    Month, 
                        {string.Join(",", types.Select(type => $"COALESCE({type},0) as {type}"))}
                    FROM
	                    Joined
                    PIVOT
                    (
	                    SUM(Count) FOR EventType
	                    IN ({string.Join(",", types)})
                    ) pvt 
                    ORDER BY Year, MonthInt
                ";
                object[] paramsArray = new object[] { postData.StartTime, postData.EndTime }.Concat(types).ToArray();
                DataTable table = connection.RetrieveData(sql, paramsArray);

                return Ok(table);
            }
        }
    }
}
