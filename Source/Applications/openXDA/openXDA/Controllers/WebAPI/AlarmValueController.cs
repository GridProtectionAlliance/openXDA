//******************************************************************************************************
//  AlarmValueController.cs - Gbtc
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
//  11/17/2020 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using GSF.Data;
using GSF.Data.Model;
using GSF.Web.Security;
using HIDS;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace openXDA.Controllers.WebAPI
{


    public class AlarmRequestPost {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string By { get; set; }
        public List<int> IDs { get; set; }
        public List<int> Phases { get; set; }
        public List<int> Groups { get; set; }
        public List<int> Types { get; set; }
        public ulong Hours { get; set; }
        public ulong Days { get; set; }
        public ulong Weeks { get; set; }
        public ulong Months { get; set; }
        public string Aggregate { get; set; }

    }

    public class AlarmRequestResponse
    {
        public string Label { get; set; }
        public int ID { get; set; }
        public string Color { get; set; }
        public List<double[]> ThreshHold { get; set; }
        public string Severity { get; set; }
    }

    [RoutePrefix("api/AlarmLimits")]
    public class AlarmLimitController : ApiController {
        // Static Fields
        private static readonly ILog Log = LogManager.GetLogger(typeof(AlarmLimitController));
        private static readonly DateTime m_epoch = new DateTime(1970, 1, 1);

        [HttpGet, Route("")]
        public IHttpActionResult Get() {
            return Ok("Hello!");
        }

     
        [HttpPost, Route("")]
        [ValidateRequestVerificationToken, SuppressMessage("Security", "SG0016", Justification = "CSRF vulnerability handled via ValidateRequestVerificationToken.")]
        public IHttpActionResult Post([FromBody] AlarmRequestPost post, CancellationToken cancellationToken)
        {

            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                DataTable alarms = connection.RetrieveData($@"
                    SELECT DISTINCT 
                        AlarmGroup.ID as ID,
                        AlarmSeverity.Name AS Severity,
                        AlarmSeverity.Color AS Color,
                        Alarm.Manual AS Manual,
                        AlarmGroup.Name AS Label
                    FROM AlarmGroup LEFT JOIN ALARM ON Alarm.AlarmgroupID = AlarmGroup.ID LEFT JOIN
					    Series ON Series.ID = Alarm.SeriesID LEFT JOIN 
	                    Channel ON Series.ChannelID = Channel.ID JOIN
	                    Meter ON Meter.ID = Channel.MeterID JOIN
	                    Asset ON Asset.ID = Channel.AssetID JOIN
	                    Phase ON Phase.ID = Channel.PhaseID JOIN
                        Location ON Location.ID = Meter.LocationID JOIN
	                    MeasurementCharacteristic ON MeasurementCharacteristic.ID = Channel.MeasurementCharacteristicID JOIN
	                    MeasurementType ON MeasurementType.ID = Channel.MeasurementTypeID JOIN
	                    ChannelGroupType ON ChannelGroupType.MeasurementCharacteristicID = Channel.MeasurementCharacteristicID AND ChannelGroupType.MeasurementTypeID = Channel.MeasurementTypeID JOIN
						AlarmSeverity ON AlarmGroup.SeverityID = AlarmSeverity.ID
                    WHERE 
	                    {post.By}.ID IN({string.Join(", ", post.IDs)}) AND
	                    Phase.ID IN ({string.Join(",", post.Phases)}) AND
	                    ChannelGroupType.ID IN {string.Join(",", post.Types)}) AND
						Alarm.Manual = 0
	                    ");

                IEnumerable<TimeFilter> hours = Enumerable.Range(0, 24).Where(index => (~post.Hours & (1Lu << index)) > 0).Select(h => TimeFilter.Hour00 + h);
                IEnumerable<TimeFilter> days = Enumerable.Range(0, 7).Where(index => (~post.Days & (1Lu << index)) > 0).Select(h => TimeFilter.Sunday + h);
                IEnumerable<TimeFilter> weeks = Enumerable.Range(0, 53).Where(index => (~post.Weeks & (1Lu << index)) > 0).Select(h => TimeFilter.Week00 + h);
                IEnumerable<TimeFilter> months = Enumerable.Range(0, 12).Where(index => (~post.Months & (1Lu << index)) > 0).Select(h => TimeFilter.January + h);

                string selectWeek = $@"
                    SELECT * FROM (
                    SELECT * FROM (SELECT StartHour, EndHour, Value FROM AlarmValue WHERE AlarmID = {{0}} AND (AlarmDayID IN (SELECT ID FROM AlarmDay WHERE Name IN ('Sunday','Weekend')) OR AlarmDayID IS NULL) AND 24 IN ({string.Join(",",days)})
                        UNION
                    SELECT StartHour + 24, EndHour + 24, Value FROM AlarmValue WHERE AlarmID = {{0}} AND(AlarmDayID IN(SELECT ID FROM AlarmDay WHERE Name IN('Monday', 'WorkDay')) OR AlarmDayID IS NULL) AND 25 IN ({string.Join(",", days)})
                        UNION
                    SELECT StartHour + 48, EndHour + 48, Value FROM AlarmValue WHERE AlarmID = {{0}} AND(AlarmDayID IN(SELECT ID FROM AlarmDay WHERE Name IN('Tuesday', 'WorkDay')) OR AlarmDayID IS NULL) AND 26 IN ({string.Join(",", days)})
                        UNION
                    SELECT StartHour + 72, EndHour + 72, Value FROM AlarmValue WHERE AlarmID = {{0}} AND(AlarmDayID IN(SELECT ID FROM AlarmDay WHERE Name IN('Wednesday', 'WorkDay')) OR AlarmDayID IS NULL) AND 27 IN ({string.Join(",", days)})
                        UNION
                    SELECT StartHour + 96, EndHour + 96, Value FROM AlarmValue WHERE AlarmID = {{0}} AND(AlarmDayID IN(SELECT ID FROM AlarmDay WHERE Name IN('Thursday', 'WorkDay')) OR AlarmDayID IS NULL) AND 28 IN ({string.Join(",", days)})
                        UNION
                    SELECT StartHour + 120, EndHour + 120, Value FROM AlarmValue WHERE AlarmID = {{0}} AND(AlarmDayID IN(SELECT ID FROM AlarmDay WHERE Name IN('Friday', 'WorkDay')) OR AlarmDayID IS NULL) AND 29 IN ({string.Join(",", days)})
                        UNION
                        SELECT StartHour + 144, EndHour + 144, Value FROM AlarmValue WHERE AlarmID = {{0}} AND(AlarmDayID IN(SELECT ID FROM AlarmDay WHERE Name IN('Saturday', 'WorkDay')) OR AlarmDayID IS NULL) AND 30 IN ({string.Join(",", days)})
                    ) W ORDER BY Starthour";

                DateTime startOfWeek = post.StartTime.Date.AddDays(-(int)post.StartTime.DayOfWeek);
                int nWeeks = (int)Math.Ceiling(post.EndTime.Subtract(startOfWeek).Days / 7.0);

                string weekTbl = $"( VALUES {string.Join(",",Enumerable.Repeat(0,nWeeks).Select(i => "(" + i.ToString() + ")"))}) TR (Weeks)";

                string sql = $@" SELECT 
	                DateAdd(week,Weeks,DateAdd(hour,StartHour,'2021-01-03')) AS StartTime,
	                DateAdd(week,Weeks,DateAdd(hour,EndHour,'2021-01-03')) AS EndTime
	                Value
	                From  ({selectWeek} CROSS JOIN {weekTbl}) C WHERE 
                    (DatePart(week,DateAdd(week,Weeks,DateAdd(hour,StartHour,{startOfWeek}))) + 30) IN ({string.Join(",", weeks)}) AND
                    (DatePart(month,DateAdd(week,Weeks,DateAdd(hour,StartHour,{startOfWeek}))) + 83) IN ({string.Join(",", months)}) AND
                    DateAdd(week,Weeks,DateAdd(hour,EndHour,{startOfWeek}))  > {{1}} AND
                    DateAdd(week,Weeks,DateAdd(hour,StartHour,{startOfWeek}))  < {{2}} AND
                    ORDER BY StartTime";

                IEnumerable<AlarmRequestResponse> result = alarms.Select().Select(item => {
                    int alarmID = connection.ExecuteScalar<int>("SELECT MAX(ID) FROM ALARM WHERE ALARMGROUPID = {0} AND Manual = 0", int.Parse(item["ID"].ToString()));
                    DataTable values = connection.RetrieveData(string.Format(string.Format(sql,alarmID,post.StartTime,post.EndTime)));
                    AlarmRequestResponse parsed = new AlarmRequestResponse() {
                        ID = int.Parse(item["ID"].ToString()),
                        Color = item["Color"].ToString(),
                        Severity = item["Severity"].ToString(),
                        Label = item["Label"].ToString(),
                        ThreshHold = values.Select().SelectMany(row => new List<double[]>() { 
                            new double[] { DateTime.Parse(row["StartTime"].ToString()).Subtract(m_epoch).TotalMilliseconds , double.Parse(row["Value"].ToString()) },
                            new double[] { DateTime.Parse(row["EndTime"].ToString()).Subtract(m_epoch).TotalMilliseconds , double.Parse(row["Value"].ToString()) },
                        }).ToList()
                    };

                    return parsed;
                });
              
                return Ok(result);

            }          
        }
    
    }
}