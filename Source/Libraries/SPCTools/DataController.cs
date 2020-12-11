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
using System.Web.Http;
using HIDS;
using openXDA.HIDS;
using openXDA.HIDS.APIExtensions;
using System.Runtime.Caching;
using System.Net.Http;

namespace SPCTools
{
    
    [RoutePrefix("api/SPCTools/Data")]
    public class DataController : ApiController
    {
        #region [internal Classes]

        /// <summary>
        /// Result of Testing a single Channel
        /// </summary>
        public class ChannelTestResponse
        {
            public int ChannelID { get; set; }
            public double Threshhold { get; set; }
            public List<FactorTestResponse> FactorTests { get; set; }
        }

        /// <summary>
        /// Result of Testing a Factor associated with a single Channel (Since for Time in Alarm and Numebr Raised it is not simple multiplication anymore)
        /// </summary>
        public class FactorTestResponse
        {
            public double TimeInAlarm { get; set; }
            public int NumberRaised { get; set; }
            public double Factor { get; set; }
        }

        public class TestRequest
        {
            public List<double> AlarmFactors { get; set; }
            public DateTime Start { get; set; }
            public DateTime End { get; set; }
            public List<int> ChannelID { get; set; }
            public List<AlarmValue> TokenValues { get; set; }
            public DataFilter StatisticsFilter { get; set; }
            public DateTime StatisticsStart { get; set; }
            public DateTime StatisticsEnd { get; set; }
            public List<int> StatisticsChannelID { get; set; }
            public int AlarmTypeID { get; set; }
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

        static DataController()
        {
            s_memoryCache = new MemoryCache("SPCToolsHIDSTestData");
        }
        #endregion

        #region [HTTPRequests]

        /// <summary>
        /// Gets the Historical Data requested for a Channel and a SeriesType 
        /// </summary>
        /// <returns> List of DataPoints for the Channel  </returns>
        [HttpPost, Route("HistoryData/{ChannelId}/{SeriesTypeID}")]
        public IHttpActionResult getChannelData(int ChannelId, int SeriesTypeID, [FromBody] DataFilter postedFilter)
        {
            NameValueCollection queryParameters = Request.RequestUri.ParseQueryString();
            string startTime = queryParameters["start"];
            string endTime = queryParameters["end"];

            if ((GetRoles != string.Empty && !User.IsInRole(GetRoles)))
                return Unauthorized();

            try
            {
                DateTime start = DateTime.Parse(startTime);
                DateTime end = DateTime.Parse(endTime);

                Func<Point, double> flattenData = GetSeriesTypeFilter(SeriesTypeID);

                IEnumerable<double[]> data = LoadChannel(new List<int>() { ChannelId }, start, end)[ChannelId].Select(pt => new double[] { pt.Timestamp.Subtract(s_epoch).TotalMilliseconds, flattenData(pt) }); ;

                if (postedFilter != null)
                    data = data.Select(pt => {
                        if (postedFilter.FilterZero && pt[1] == 0.0D)
                            return new double[] { pt[0], double.NaN };
                        if (postedFilter.FilterLower && pt[1] < postedFilter.LowerLimit)
                            return new double[] { pt[0], double.NaN };
                        if (postedFilter.FilterUpper && pt[1] > postedFilter.UpperLimit)
                            return new double[] { pt[0], double.NaN };
                        return pt;
                    }).ToList();


                return Ok(data);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }


        }


        /// <summary>
        /// Gets the Historical Data requested for a Channel and a SeriesType 
        /// </summary>
        /// <returns> List of DataPoints for the Channel  </returns>
        [HttpPost, Route("Test/{SeriesTypeID}")]
        public IHttpActionResult TestAlarmGroup(int seriesTypeID, [FromBody] TestRequest request)
        {
            try
            {
                //Grab Data For Setpoint Computation
                Dictionary<int, List<Point>> statisticsData = LoadChannel(request.StatisticsChannelID, request.StatisticsStart, request.StatisticsEnd);
                List<Token> tokenList = request.TokenValues.Select(value => new Token(value.Formula, statisticsData, request.ChannelID, request.StatisticsFilter, GetTimeFilter(value))).ToList();

                Dictionary<int, List<Point>> testData;
                if (request.Start == request.StatisticsStart && request.End == request.StatisticsEnd && request.ChannelID.Count == request.StatisticsChannelID.Count)
                    testData = statisticsData;
                else
                    testData = LoadChannel(request.ChannelID, request.Start, request.End);

                bool isDynamic = !(tokenList.Count == 1);
                bool isScalar = tokenList.All(item => item.isScalar);

                Func<Point, double> seriesTypeFilter = GetSeriesTypeFilter(seriesTypeID);

                List<ChannelTestResponse> result = request.ChannelID.Select((id, index) =>
                {
                    ChannelTestResponse test = new ChannelTestResponse() { ChannelID = id, Threshhold = (isDynamic ? double.NaN : (isScalar ? tokenList[0].Scalar : tokenList[0].Slice[index])) };

                    return TestChannel(index, testData[id], tokenList, request.AlarmFactors, request.AlarmTypeID, test, seriesTypeFilter);

                }).ToList();

                return Ok(result);

            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }

        #endregion

        #region [ HelperFunction ]

        private Dictionary<int, List<Point>> LoadChannel(List<int> channelID, DateTime start, DateTime end)
        {

            HIDSSettings settings = new HIDSSettings();
            settings.Load();

            Dictionary<int, List<Point>> result = new Dictionary<int, List<Point>>();

            string cachTarget = start.Subtract(s_epoch).TotalMilliseconds + "-" + end.Subtract(s_epoch).TotalMilliseconds + "-";
            List<string> dataToGet = new List<string>();
            channelID.ForEach(item =>
            {
                if (s_memoryCache.Contains(cachTarget + item.ToString("x8")))
                    result.Add(item, (List<Point>)s_memoryCache.Get(cachTarget + item.ToString("x8")));
                else
                    dataToGet.Add(item.ToString("x8"));
            });

            if (dataToGet.Count == 0)
                return result;

         
            List<Point> data;
            using (API hids = new API())
            {
                hids.Configure(settings);
                data = hids.ReadPointsAsync(dataToGet, start, end).ToListAsync().Result;
            }

            dataToGet.ForEach(item => { s_memoryCache.Add(cachTarget + item, data.Where(pt => pt.Tag == item).ToList(), new CacheItemPolicy { SlidingExpiration = TimeSpan.FromMinutes(s_cacheExipry) }); });

            return channelID.ToDictionary(item => item, item => data.Where(pt => pt.Tag == item.ToString("x8")).ToList());


        }

        private ChannelTestResponse TestChannel(int channelIndex, List<Point> data, List<Token> tokenList, List<double> factors, int alarmtypeID, ChannelTestResponse result, Func<Point, double> getData)
        {

            bool upper = true;
            using (AdoDataConnection connection = new AdoDataConnection(Connection))
            {
                AlarmType alarmType = new TableOperations<AlarmType>(connection).QueryRecordWhere("ID = {0}", alarmtypeID);
                if (alarmType.Name == "Lower Limit")
                    upper = false;
            }

            result.FactorTests = factors.Select(f => new FactorTestResponse() { TimeInAlarm = 0, NumberRaised = 0, Factor = f }).ToList();
            result.FactorTests.Add(new FactorTestResponse() { TimeInAlarm = 0, NumberRaised = 0, Factor = 1.0 });


            // Create Hour of the week based Threshholds
            double[] threshholds = new double[168];

            // start with any Sunday 
            DateTime dt = new DateTime(2020, 11, 1);
            for (int i = 0; i < 168; i++)
            {
                int tokeIndex = tokenList.FindIndex(item => item.Applies(dt.AddHours(i)));
                if (tokeIndex == -1)
                    threshholds[i] = double.NaN;
                else
                    threshholds[i] = (tokenList[tokeIndex].isScalar ? tokenList[tokeIndex].Scalar : tokenList[tokeIndex].Slice[channelIndex]);
            }

            //Find number of alarms
            for (int i = 0; i < data.Count; i++)
            {
                int hourofWeek = (int)data[i].Timestamp.DayOfWeek * 24 + data[i].Timestamp.Hour;
                double threshold = threshholds[hourofWeek];

                if (!double.IsNaN(threshold))
                    for (int jF = 0; jF < result.FactorTests.Count; jF++)
                    {
                        double p1 = (i == 0 ? 0 : (getData(data[i - 1]) - threshold * result.FactorTests[jF].Factor) * (upper ? 1.0D : -1.0D));
                        double p2 = (getData(data[i]) - threshold * result.FactorTests[jF].Factor) * (upper ? 1.0D : -1.0D);

                        if (p2 > 0)
                            result.FactorTests[jF].TimeInAlarm++;
                        if ((p1 * p2) < 0 && p2 > 0 && i > 0)
                            result.FactorTests[jF].NumberRaised++;
                    }
            }



            result.FactorTests = result.FactorTests.Select(f => new FactorTestResponse() { TimeInAlarm = (data.Count > 0 ? f.TimeInAlarm / data.Count : 0.0D), NumberRaised = f.NumberRaised, Factor = f.Factor }).ToList();
            return result;

        }

        private Func<DateTime, bool> GetTimeFilter(AlarmValue alarmValue)
        {
            AlarmDay day;
            using (AdoDataConnection connection = new AdoDataConnection(Connection))
            {
                day = new TableOperations<AlarmDay>(connection).QueryRecordWhere("ID = {0}", alarmValue.AlarmdayID);
            }
            if (day == null)
                return (DateTime input) => { return (input.Hour >= alarmValue.StartHour && input.Hour < alarmValue.EndHour); };
            if (day.Name == "WeekEnd")
                return (DateTime input) => { return (input.Hour >= alarmValue.StartHour && input.Hour < alarmValue.EndHour && (input.DayOfWeek == DayOfWeek.Sunday || input.DayOfWeek == DayOfWeek.Saturday)); };
            if (day.Name == "WeekDay")
                return (DateTime input) => { return (input.Hour >= alarmValue.StartHour && input.Hour < alarmValue.EndHour && !(input.DayOfWeek == DayOfWeek.Sunday || input.DayOfWeek == DayOfWeek.Saturday)); };

            return (DateTime input) => { return (input.Hour >= alarmValue.StartHour && input.Hour < alarmValue.EndHour); };
        }

        private Func<Point, double> GetSeriesTypeFilter(int SeriesTypeID)
        {
            using (AdoDataConnection connection = new AdoDataConnection(Connection))
            {
                SeriesType seriesType = new TableOperations<SeriesType>(connection).QueryRecordWhere("ID={0}", SeriesTypeID);
                if (seriesType != null && seriesType.Name == "Minimum")
                    return (Point pt) => { return pt.Minimum; };
                else if (seriesType != null && seriesType.Name == "Maximum")
                    return (Point pt) => { return pt.Maximum; };
                else
                    return (Point pt) => { return pt.Average; };
            }
        }
        #endregion
    } 
}
