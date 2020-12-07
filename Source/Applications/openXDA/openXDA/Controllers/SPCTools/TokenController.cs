//******************************************************************************************************
//  SPCTools/DataController.cs - Gbtc
//
//  Copyright © 2018, Grid Protection Alliance.  All Rights Reserved.
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
//  10/23/2020 - C. Lackner
//       Generated original version of source code.
//
//******************************************************************************************************

using GSF.Data;
using GSF.Data.Model;
using Newtonsoft.Json.Linq;
using openXDA.Model;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using HIDS;
using openXDA.HIDS;
using openXDA.HIDS.APIExtensions;
using System.Runtime.Caching;

namespace openXDA.Controllers
{
    [RoutePrefix("api/SPCTools/Token")]
    public class TokenController : ApiController
    {
        #region [internal Classes]

        public class TokenParseRequest
        {
            public AlarmValue TokeValue { get; set; }
            public DataFilter StatisticsFilter { get; set; }
            public DateTime StatisticsStart { get; set; }
            public DateTime StatisticsEnd { get; set; }
            public List<int> StatisticsChannelID { get; set; }

        }

        public class TokenParseResponse
        {
            public bool Valid { get; set; }
            public string Message { get; set; }
            public bool IsScalar { get; set; }
            public List<double> Value { get; set; }

        }
        #endregion

        #region [Properties]

        protected virtual string Connection { get; } = "systemSettings";
        protected virtual string GetRoles { get; } = "Viewer,Administrator";

        private static DateTime s_epoch = new DateTime(1970, 1, 1);

        private static MemoryCache s_memoryCache;
        private static readonly double s_cacheExipry = 5; 

        #endregion

        #region [ Constructor ]

        static TokenController()
        {
            s_memoryCache = new MemoryCache("SPCToolsHIDSData");
        }
        #endregion
        #region [HTTPRequests]

        /// <summary>
        /// Parses an <see cref="AlarmValue"/> based on the supplied Paramters
        /// </summary>
        /// <returns> Returns a Parsed AlarmValue </returns>
        [HttpPost, Route("Parse")]
        public IHttpActionResult ParseValue(TokenParseRequest request)
        {
            if ((GetRoles != string.Empty && !User.IsInRole(GetRoles)))
                return Unauthorized();

            try
            {
                Dictionary<int, List<Point>> data = LoadChannel(request.StatisticsChannelID, request.StatisticsStart, request.StatisticsEnd );
                Token root = new Token(request.TokeValue.Formula, data, request.StatisticsChannelID, request.StatisticsFilter);

                TokenParseResponse result = new TokenParseResponse()
                {
                    IsScalar = root.isScalar,
                    Valid = root.Valid,
                    Message = root.Error,
                    Value = new List<double>()
                };

                // If it is not a scalar or a slice it is an error
                if (!root.isScalar && !root.isSlice && result.Valid)
                {
                    result.Valid = false;
                    result.Message = "The Expression needs to result in a static threshhold.";
                }

                if (result.Valid)
                    result.Value = root.ComputeSlice();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }




        #endregion

        #region [ HelperFunction ]

        // Note for now this only pulls Channel 3 because We are still looking at two different DataBases (one for HIDS and one local for testing).
        // This needs to change before TVA deployment
        private Dictionary<int,List<Point>> LoadChannel(List<int> channelID, DateTime start, DateTime end)
        {

            HIDSSettings settings = new HIDSSettings();
            settings.Load();

            Dictionary<int, List<Point>> result = new Dictionary<int, List<Point>>();

            string cachTarget = start.Subtract(s_epoch).TotalMilliseconds + "-" + end.Subtract(s_epoch).TotalMilliseconds + "-";
            List<string> dataToGet = new List<string>();
            channelID.ForEach(item =>
            {
                int tmp = 4;

                if (s_memoryCache.Contains(cachTarget + tmp.ToString("x8")))
                    result.Add(item, (List<Point>)s_memoryCache.Get(cachTarget + tmp.ToString("x8")));
                else
                    dataToGet.Add(tmp.ToString("x8"));

            });

            if (dataToGet.Count == 0)
                return result;

            dataToGet = new List<string>() { "00000003" };

            List <Point> data;
            using (API hids = new API())
            {
                hids.Configure(settings);
                data = hids.ReadPointsAsync(dataToGet, start,end).ToListAsync().Result;
            }

            dataToGet.ForEach(item => { s_memoryCache.Add(cachTarget + item, data, new CacheItemPolicy { SlidingExpiration = TimeSpan.FromMinutes(s_cacheExipry) }); });

            return channelID.ToDictionary(item => item, item => data);


        }

        #endregion
    }
}
