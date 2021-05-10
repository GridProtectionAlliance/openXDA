//******************************************************************************************************
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
using System.Text;
using System.Web.Http;
using GSF.Data;
using GSF.Web.Security;
using openXDA.Model;

namespace openXDA.Controllers.WebAPI
{
    [RoutePrefix("api/rvht")]
    public class RequestVerificationHeaderTokenController: ApiController
    {
        [HttpGet,Route("")]
        public HttpResponseMessage Get()
        {
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(Request.GenerateRequestVerficationHeaderToken(), Encoding.UTF8, "text/plain")
             };
        }
    }

    [RoutePrefix("api/Meter")]
    public class MeterController : ModelController<Meter>
    {
        public MeterController(Func<AdoDataConnection> connectionFactory)
            : base(connectionFactory)
        {
        }
        
        protected override bool AllowSearch => true;
    }
    
    [RoutePrefix("api/Channel")]
    public class ChannelController : ModelController<ChannelDetail> 
    {
        public ChannelController(Func<AdoDataConnection> connectionFactory)
            : base(connectionFactory)
        {
        }

        protected override bool HasParent => true;
        protected override string ParentKey => "MeterID";
        protected override bool AllowSearch => true;
    }

    [RoutePrefix("api/Asset")]
    public class AssetController : ModelController<Asset>
    {
        public AssetController(Func<AdoDataConnection> connectionFactory)
            : base(connectionFactory)
        {
        }
        
        protected override bool AllowSearch => true;
    }

    [RoutePrefix("api/Phase")]
    public class PhaseController : ModelController<Phase>
    {
        public PhaseController(Func<AdoDataConnection> connectionFactory)
            : base(connectionFactory)
        {
        }
        
        protected override bool AllowSearch => true;
    }

    [RoutePrefix("api/EventType")]
    public class EventTypeController : ModelController<EventType>
    {
        public EventTypeController(Func<AdoDataConnection> connectionFactory)
            : base(connectionFactory)
        {
        }
    }

    [RoutePrefix("api/Event")]
    public class EventController : ModelController<Event>
    {
        public EventController(Func<AdoDataConnection> connectionFactory)
            : base(connectionFactory)
        {
        }

        [HttpPost, Route("TrenDAP")]
        public IHttpActionResult GetEventsForTrenDAP([FromBody] HIDSPost post)
        {
            string hours = string.Join(",", Enumerable.Range(0, 24).Where(index => (post.Hours & (1Lu << index)) > 0).Select(h => h.ToString()));
            string days = string.Join(",", Enumerable.Range(0, 7).Where(index => (post.Days & (1Lu << index)) > 0).Select(h => (h + 1).ToString()));
            string weeks = string.Join(",", Enumerable.Range(0, 53).Where(index => (post.Weeks & (1Lu << index)) > 0).Select(h => h.ToString()));
            string months = string.Join(",", Enumerable.Range(0, 12).Where(index => (post.Months & (1Lu << index)) > 0).Select(h => (h + 1).ToString()));
            string channels = string.Join(",", HIDSController.GetTable(post).Select().Select(row => row["ID"].ToString()));
            using (AdoDataConnection connection = CreateDbConnection())
            {
                string sql = @"
                    SELECT DISTINCT Event.ID,StartTime,Channel.ID as ChannelID 
                    FROM Event JOIN
                         Channel ON Event.MeterID = channel.MeterID AND Event.AssetID = Channel.AssetID
                    WHERE 
                        (StartTime BETWEEN {0} AND {1} OR EndTime BETWEEN {0} AND {1}) AND
                        Channel.ID IN ("+ channels +@") AND
                        (DATEPART(hour,StartTime) IN ("+ hours + @") OR DATEPART(hour,EndTime) IN (" + hours + @")) AND
                        (DATEPART(day,StartTime) IN (" + days + @") OR DATEPART(day,EndTime) IN (" + days + @")) AND
                        (DATEPART(week,StartTime) IN (" + weeks + @") OR DATEPART(week,EndTime) IN (" + weeks + @")) AND
                        (DATEPART(month,StartTime) IN (" + months + @") OR DATEPART(month,EndTime) IN (" + months + @"))
                    ";

                DataTable table = connection.RetrieveData(sql, post.StartTime, post.EndTime);
                return Ok(table);
            }
        }

    }

    [RoutePrefix("api/MeasurementType")]
    public class MeasurementTypeController : ModelController<MeasurementType>
    {
        public MeasurementTypeController(Func<AdoDataConnection> connectionFactory)
            : base(connectionFactory)
        {
        }
    }

    [RoutePrefix("api/MeasurementCharacteristic")]
    public class MeasurementCharacteristicController : ModelController<MeasurementCharacteristic>
    {
        public MeasurementCharacteristicController(Func<AdoDataConnection> connectionFactory)
            : base(connectionFactory)
        {
        }
    }

    [RoutePrefix("api/ChannelGroup")]
    public class ChannelGroupController : ModelController<ChannelGroup>
    {
        public ChannelGroupController(Func<AdoDataConnection> connectionFactory)
            : base(connectionFactory)
        {
        }
    }

    [RoutePrefix("api/ChannelGroupType")]
    public class ChannelGroupTypeController : ModelController<ChannelGroupType>
    {
        public ChannelGroupTypeController(Func<AdoDataConnection> connectionFactory)
            : base(connectionFactory)
        {
        }
    }

    [RoutePrefix("api/AlarmType")]
    public class AlarmTypeController : ModelController<AlarmType>
    {
        public AlarmTypeController(Func<AdoDataConnection> connectionFactory)
            : base(connectionFactory)
        {
        }

        protected override bool AllowSearch => true;
    }

    [RoutePrefix("api/MeterDetail")]
    public class MeterDetailController : ModelController<MeterDetail>
    {
        public MeterDetailController(Func<AdoDataConnection> connectionFactory)
            : base(connectionFactory)
        {
        }

        protected override bool AllowSearch => true;
    }

    [RoutePrefix("api/AlarmDay")]
    public class AlarmDayController : ModelController<AlarmDay>
    {
        public AlarmDayController(Func<AdoDataConnection> connectionFactory)
            : base(connectionFactory)
        {
        }

        protected override bool ViewOnly => true;
    }

    [RoutePrefix("api/AlarmDayGroup")]
    public class AlarmDayGroupController : ModelController<AlarmDayGroupView>
    {
        public AlarmDayGroupController(Func<AdoDataConnection> connectionFactory)
            : base(connectionFactory)
        {
        }

        protected override bool ViewOnly => true;
    }

    [RoutePrefix("api/SeriesType")]
    public class SettingController : ModelController<SeriesType>
    {
        public SettingController(Func<AdoDataConnection> connectionFactory)
            : base(connectionFactory)
        {
        }

        protected override bool AllowSearch => true;
    }

    [RoutePrefix("api/Setting")]
    public class SeriesTypeController : ModelController<SeriesType>
    {
        public SeriesTypeController(Func<AdoDataConnection> connectionFactory)
            : base(connectionFactory)
        {
        }

        protected override bool AllowSearch => true;

        [Route("Category/{category}")]
        public IHttpActionResult GetSettingCategory(string category)
        {
            using (AdoDataConnection connection = CreateDbConnection())
            {
                return Ok(connection.RetrieveData("SELECT * FROM Setting WHERE Name LIKE {0}", category + "%"));
            }
        }
        [Route("{setting}")]
        public HttpResponseMessage GetSetting(string setting)
        {
            using (AdoDataConnection connection = CreateDbConnection())
            {
                string value = connection.ExecuteScalar<string>(1000, "SELECT Value FROM Setting WHERE Name = {0}", setting);
                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                result.Content = new StringContent(value);
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("text/pain");
                return result;
            }
        }
    }


}