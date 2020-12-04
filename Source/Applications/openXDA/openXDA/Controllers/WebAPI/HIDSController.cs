//******************************************************************************************************
//  HIDSController.cs - Gbtc
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


    public class HIDSPost {
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

    [RoutePrefix("api/HIDS")]
    public class HIDSController : ApiController {
        // Static Fields
        private static readonly ILog Log = LogManager.GetLogger(typeof(HIDSController));

        [HttpGet, Route("")]
        public IHttpActionResult Get() {
            return Ok("Hello!");
        }

        private class thingy { 
        
        }

        [HttpPost, Route("")]
        [ValidateRequestVerificationToken, SuppressMessage("Security", "SG0016", Justification = "CSRF vulnerability handled via ValidateRequestVerificationToken.")]
        public IHttpActionResult Post([FromBody] HIDSPost post, CancellationToken cancellationToken)
        {
            //Stopwatch stopwatch = new Stopwatch();
            //stopwatch.Start();
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings")) {
                DataTable ids = connection.RetrieveData(@"
                    SELECT 
	                    DISTINCT Channel.ID, Meter.Name as Meter, Asset.AssetName as Asset, Phase.Name as Phase,  
                        Channel.Name, MeasurementCharacteristic.Name as Characteristic, MeasurementType.Name as Type, 
                        Channel.HarmonicGroup as Harmonic, Location.Name as Station, Location.Latitude, Location.Longitude
                    FROM 
	                    Channel JOIN
	                    Meter ON Meter.ID = Channel.MeterID JOIN
	                    Asset ON Asset.ID = Channel.AssetID JOIN
	                    Phase ON Phase.ID = Channel.PhaseID JOIN
                        Location ON Location.ID = Meter.LocationID JOIN
	                    MeasurementCharacteristic ON MeasurementCharacteristic.ID = Channel.MeasurementCharacteristicID JOIN
	                    MeasurementType ON MeasurementType.ID = Channel.MeasurementTypeID JOIN
	                    ChannelGroupType ON ChannelGroupType.MeasurementCharacteristicID = Channel.MeasurementCharacteristicID AND ChannelGroupType.MeasurementTypeID = Channel.MeasurementTypeID
                    WHERE 
	                    " + post.By + @".ID IN (" + string.Join(",", post.IDs)+ @") AND
	                    Phase.ID IN (" + string.Join(",", post.Phases) + @") AND
	                    ChannelGroupType.ID IN (" + string.Join(",", post.Types) + @")
                ");
                //Log.Error($"Database Query Complete  = {stopwatch.Elapsed}");

                HIDS.TrendingDataQuery hids = new HIDS.TrendingDataQuery();
                List<Point> results = hids.Query(ids.Select().Select(x => int.Parse(x["ID"].ToString())).ToList(), post.StartTime, post.EndTime, post.Aggregate,  cancellationToken, post.Hours, post.Days, post.Weeks, post.Months).Result;
                //Log.Error($"HIDS Query Complete  = {stopwatch.Elapsed}");
                JArray json = JArray.FromObject(ids);

                var groupjoin = json.GroupJoin(results, row => int.Parse(row["ID"].ToString()), result => int.Parse(result.Tag, System.Globalization.NumberStyles.HexNumber), (row, resultcollection) => {
                    JObject jObject = JObject.FromObject(row);
                    jObject["Data"] = JArray.FromObject(resultcollection);
                    return jObject;
                });
                //Log.Error($"GroupJoin Complete  = {stopwatch.Elapsed}");

                return Ok(JsonConvert.SerializeObject(groupjoin));

                //HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                //response.Content = new PushStreamContent((stream, content, context) =>
                //{
                //    // write your output here
                //    SerializeJsonIntoStream(groupjoin, stream);
                //});

                //Log.Error($"Stream write Complete  = {stopwatch.Elapsed}");

                //return response;
            }          
        }

        private static void SerializeJsonIntoStream(object value, Stream stream)
        {
            using (var sw = new StreamWriter(stream, new UTF8Encoding(false), 1024, true))
            using (var jtw = new JsonTextWriter(sw) { Formatting = Formatting.None })
            {
                var js = new JsonSerializer();
                js.Serialize(jtw, value);
                jtw.Flush();
            }
        }

        private static HttpContent CreateHttpContent(object content)
        {
            HttpContent httpContent = null;

            if (content != null)
            {
                var ms = new MemoryStream();
                SerializeJsonIntoStream(content, ms);
                ms.Seek(0, SeekOrigin.Begin);
                httpContent = new StreamContent(ms);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }

            return httpContent;
        }
    }
}