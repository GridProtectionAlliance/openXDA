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
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web.Http;
using GSF.Configuration;
using GSF.Data;
using GSF.Data.Model;
using GSF.Net.Security;
using GSF.Security.Model;
using GSF.Web;
using GSF.Web.Model;
using GSF.Web.Security;
using log4net;
using openXDA.Model;
using SystemCenter.Model;

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
    public class MeterController : ModelController<Meter> {
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
    public class ChannelController : ModelController<ChannelDetail> {}

    [RoutePrefix("api/Asset")]
    public class AssetController : ModelController<Asset> {}

    [RoutePrefix("api/Phase")]
    public class PhaseController : ModelController<Phase> {}

    [RoutePrefix("api/EventType")]
    public class EventTypeController : ModelController<EventType> {}

    [RoutePrefix("api/Event")]
    public class EventController : ModelController<Event>
    {
        [HttpPost, Route("TrenDAP")]
        public IHttpActionResult GetEventsForTrenDAP([FromBody] HIDSPost post)
        {
            string hours = string.Join(",", Enumerable.Range(0, 24).Where(index => (post.Hours & (1Lu << index)) > 0).Select(h => h.ToString()));
            string days = string.Join(",", Enumerable.Range(0, 7).Where(index => (post.Days & (1Lu << index)) > 0).Select(h => (h + 1).ToString()));
            string weeks = string.Join(",", Enumerable.Range(0, 53).Where(index => (post.Weeks & (1Lu << index)) > 0).Select(h => h.ToString()));
            string months = string.Join(",", Enumerable.Range(0, 12).Where(index => (post.Months & (1Lu << index)) > 0).Select(h => (h + 1).ToString()));
            string channels = string.Join(",", HIDSController.GetTable(post).Select().Select(row => row["ID"].ToString()));
            using (AdoDataConnection connection = new AdoDataConnection(Connection))
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

    [RoutePrefix("api/Customer")]
    public class CustomerController : ModelController<openXDA.Model.Customer> {}

}