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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using GSF.Data;
using GSF.Data.Model;
using HIDS;
using openXDA.HIDS;
using openXDA.HIDS.APIExtensions;
using openXDA.Model;
using openXDA.Nodes;

namespace SPCTools
{

    public class ParseResponse
    {
        public bool Valid { get; set; }
        public string Message { get; set; }
        public bool IsScalar { get; set; }
        public List<double> Value { get; set; }

        public ParseResponse(DataResponse response)
        {
            Valid = response.Valid;
            Message = response.Message;
            Value = response.Value;
            IsScalar = response.IsScalar;
        }
        public ParseResponse() { }
    }


    [RoutePrefix("api/SPCTools/Token")]
    public class TokenController : ApiController
    {
        /// <summary>
        /// Describes common arithmetic operands being Scalar, Slice, or Matrix. 
        /// </summary>

        #region [internal Classes]

        public class TokenParseRequest
        {
            public AlarmValue TokeValue { get; set; }
            public DataFilter StatisticsFilter { get; set; }
            public DateTime StatisticsStart { get; set; }
            public DateTime StatisticsEnd { get; set; }
            public List<int> StatisticsChannelID { get; set; }
        }

        #endregion

        #region [Properties]

        protected virtual string GetRoles { get; } = "Viewer,Administrator";

        private static DateTime s_epoch = new DateTime(1970, 1, 1);


        private Host Host { get; }

        #endregion

        #region [ Constructor ]

        public TokenController(Host host) =>
            Host = host;

        #endregion

        #region [HTTPRequests]

        /// <summary>
        /// Parses an <see cref="AlarmValue"/> based on the supplied Paramters
        /// </summary>
        /// <returns> Returns a Parsed AlarmValue </returns>
        [HttpPost, Route("Parse")]
        public IHttpActionResult ParseValue(TokenParseRequest request, CancellationToken cancellationToken)
        {
            if ((GetRoles != string.Empty && !User.IsInRole(GetRoles)))
                return Unauthorized();

            try
            {
                using (API hids = new API())
                {
                    Dictionary<int, IAsyncEnumerable<Point>> data = LoadChannel(hids, request.StatisticsChannelID, request.StatisticsStart, request.StatisticsEnd, cancellationToken);
                    ExpressionOperations root = new ExpressionOperations(request.TokeValue.Formula, data, request.StatisticsChannelID, request.StatisticsFilter, GetTimeFilter(request.TokeValue));

                    DataResponse resultant = root.Evaluate();
                    ParseResponse result = new ParseResponse(resultant);
                    return Ok(result);
                }
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
        private Dictionary<int, IAsyncEnumerable<Point>> LoadChannel(API hids, List<int> channelID, DateTime start, DateTime end, CancellationToken cancellationToken)
        {
            Dictionary<int, IAsyncEnumerable<Point>> result = new Dictionary<int, IAsyncEnumerable<Point>>();

            string cachTarget = start.Subtract(s_epoch).TotalMilliseconds + "-" + end.Subtract(s_epoch).TotalMilliseconds + "-";
            List<string> dataToGet = new List<string>();
            channelID.ForEach(item =>
            {
                dataToGet.Add(item.ToString("x8"));
            });

            if (dataToGet.Count == 0)
                return result;

            async Task<IAsyncEnumerable<Point>> QueryHIDSAsync()
            {
                HIDSSettings settings = SettingsHelper.GetHIDSSettings(Host);
                await hids.ConfigureAsync(settings);
                return hids.ReadPointsAsync(dataToGet, start, end, cancellationToken);
            }

            Task<IAsyncEnumerable<Point>> queryTask = QueryHIDSAsync();
            IAsyncEnumerable<Point> data = queryTask.GetAwaiter().GetResult();

            return channelID
                .ToAsyncEnumerable()
                .GroupJoin(data.GroupBy(pt => pt.Tag), item => item.ToString("x8"), grouping => grouping.Key, (item, grouping) => new { Key = item, Value = grouping.SelectMany(inner => inner) })
                .ToDictionaryAsync(obj => obj.Key, obj => obj.Value, cancellationToken)
                .GetAwaiter()
                .GetResult();
        }

        private Func<DateTime, bool> GetTimeFilter(AlarmValue alarmValue)
        {
            AlarmDay day;
            using (AdoDataConnection connection = Host.CreateDbConnection())
            {
                day = new TableOperations<AlarmDay>(connection).QueryRecordWhere("ID = {0}", alarmValue.AlarmDayID);
            }
            if (day == null)
                return (DateTime input) => { return (input.Hour >= alarmValue.StartHour && input.Hour < alarmValue.EndHour); };
            if (day.Name == "WeekEnd")
                return (DateTime input) => { return (input.Hour >= alarmValue.StartHour && input.Hour < alarmValue.EndHour && (input.DayOfWeek == DayOfWeek.Sunday || input.DayOfWeek == DayOfWeek.Saturday)); };
            if (day.Name == "WeekDay")
                return (DateTime input) => { return (input.Hour >= alarmValue.StartHour && input.Hour < alarmValue.EndHour && !(input.DayOfWeek == DayOfWeek.Sunday || input.DayOfWeek == DayOfWeek.Saturday)); };

            return (DateTime input) => { return (input.Hour >= alarmValue.StartHour && input.Hour < alarmValue.EndHour); };
        }

        #endregion
    }
}
