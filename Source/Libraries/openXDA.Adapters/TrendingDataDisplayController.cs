﻿//******************************************************************************************************
//  PeriodicDataDisplay1.cs - Gbtc
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
//  05/25/2018 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Runtime.Caching;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using GSF;
using GSF.Data;
using GSF.Data.Model;
using GSF.Web;
using HIDS;
using openHistorian.XDALink;
using openXDA.Model;

namespace openXDA.Adapters
{
    public class TrendingDataDisplayController : ApiController
    {
        #region [ Members ]

        // Nested Types
        public class PeridoicMeasurements
        {
            public string MeasurementType { get; set; }
            public int MeasurementTypeID { get; set; }
            public string MeasurementCharacteristic { get; set; }
            public int MeasurementCharacteristicID { get; set; }
        }

        public class Post { 
            public string StartDate { get; set; }
            public string EndDate { get; set; }
            public int Pixels { get; set; }
            public List<int> Channels { get; set; }
        }

        public class ReturnPoint
        {
            public double Time { get; set; }
            public double Minimum { get; set; }
            public double Maximum { get; set; }
            public double Average { get; set; }
        }

        #endregion

        #region [ Constructors ]

        public TrendingDataDisplayController(Func<AdoDataConnection> connectionFactory) =>
            ConnectionFactory = connectionFactory;

        #endregion

        #region [ Properties ]

        private Func<AdoDataConnection> ConnectionFactory { get; }

        #endregion

        #region [ Methods ]

        [HttpGet]
        public Task<Dictionary<string, List<double[]>>> GetData(CancellationToken cancellationToken)
        {
            Dictionary<string, string> query = Request.QueryParameters();
            int channelID = int.Parse(query["ChannelID"]);
            int pixels = int.Parse(query["pixels"]);

            DateTime startDate = DateTime.ParseExact(query["StartDate"], "yyyy-MM-ddTHH:mm", CultureInfo.CurrentCulture);
            DateTime endDate = DateTime.ParseExact(query["EndDate"], "yyyy-MM-ddTHH:mm", CultureInfo.CurrentCulture);
            DateTime epoch = new DateTime(1970, 1, 1);

            return Task.Factory.StartNew(() =>
            {
                using (AdoDataConnection connection = ConnectionFactory())
                {
                    string target = "TrendingDataDisplayGetData" + channelID.ToString() + startDate.ToString("yyyyMMddHHmm") + endDate.ToString("yyyyMMddHHmm");
                    Dictionary<string, List<double[]>> data = (Dictionary<string, List<double[]>>)s_memoryCache.Get(target);
                    if (data == null)
                    {
                        data = new Dictionary<string, List<double[]>>();
                        data.Add("Minimum", new List<double[]>());
                        data.Add("Maximum", new List<double[]>());
                        data.Add("Average", new List<double[]>());

                        IEnumerable<Point> points = QueryHIDS(channelID, startDate, endDate);
                        foreach (Point point in points)
                        {
                            data["Minimum"].Add(new[] { point.Timestamp.Subtract(epoch).TotalMilliseconds, point.Minimum });
                            data["Maximum"].Add(new[] { point.Timestamp.Subtract(epoch).TotalMilliseconds, point.Maximum });
                            data["Average"].Add(new[] { point.Timestamp.Subtract(epoch).TotalMilliseconds, point.Average });
                        }



                        s_memoryCache.Add(target, data, new CacheItemPolicy { SlidingExpiration = TimeSpan.FromMinutes(0.5D) });

                    }
                    return Downsample(data, pixels, new Range<DateTime>(startDate, endDate));
                }

            }, cancellationToken);

        }

        [HttpPost]
        public Task<Dictionary<int, List<ReturnPoint>>> GetData([FromBody] Post post,CancellationToken cancellationToken)
        {
            DateTime epoch = new DateTime(1970, 1, 1);

            DateTime startDate = DateTime.ParseExact(post.StartDate, "yyyy-MM-ddTHH:mm", CultureInfo.CurrentCulture);
            DateTime endDate = DateTime.ParseExact(post.EndDate, "yyyy-MM-ddTHH:mm", CultureInfo.CurrentCulture);

            return Task.Factory.StartNew(() =>
            {
                using (AdoDataConnection connection = ConnectionFactory())
                {
                    Dictionary<int, List<ReturnPoint>> data = new Dictionary<int, List<ReturnPoint>>();

                    IEnumerable<Point> points = QueryHIDS(post.Channels, startDate, endDate);
                    foreach (Point point in points)
                    {
                        int channelID = int.Parse(point?.Tag ?? "0x0", System.Globalization.NumberStyles.HexNumber);
                        if (!data.ContainsKey(channelID))
                            data.Add(channelID, new List<ReturnPoint>() { new ReturnPoint { Time = point.Timestamp.Subtract(epoch).TotalMilliseconds, Minimum = point.Minimum, Average = point.Average, Maximum = point.Maximum } });
                        else
                            data[channelID].Add(new ReturnPoint { Time = point.Timestamp.Subtract(epoch).TotalMilliseconds, Minimum = point.Minimum, Average = point.Average, Maximum = point.Maximum });
                    }
                    return data;
                }

            }, cancellationToken);

        }


        [HttpGet]
        public Task<IOrderedEnumerable<Channel>> GetMeasurements(CancellationToken cancellationToken)
        {
            Dictionary<string, string> query = Request.QueryParameters();
            int meterId = int.Parse(query["MeterID"]);

            return Task.Factory.StartNew(() =>
            {
                using (AdoDataConnection connection = ConnectionFactory())
                {
                    string target = "TrendingDataDisplayMeasurements" + meterId.ToString();
                    IOrderedEnumerable<Channel> data = (IOrderedEnumerable<Channel>)s_memoryCache.Get(target);
                    if (data == null)
                    {
                        data = (new TableOperations<Channel>(connection)).QueryRecordsWhere("ID IN (SELECT DISTINCT ChannelID FROM DailyTrendingSummary) AND MeterID = {0}", meterId).OrderBy(x => x.Name);
                        s_memoryCache.Add(target, data, new CacheItemPolicy { SlidingExpiration = TimeSpan.FromMinutes(1.0D) });
                    }

                    return data;
                }
            }, cancellationToken);

        }

        [HttpGet]
        public Task<IOrderedEnumerable<PeridoicMeasurements>> GetMeasurementCharacteristics(CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew(() =>
            {
                using (AdoDataConnection connection = ConnectionFactory())
                {
                    string target = "PeriodicDataDisplayMeasurementCharacteristics";
                    IOrderedEnumerable<PeridoicMeasurements> data = (IOrderedEnumerable<PeridoicMeasurements>)s_memoryCache.Get(target);
                    if (data == null)
                    {
                        string query = @"
                            SELECT DISTINCT 
	                            MeasurementType.Name as MeasurementType,
                                MeasurementType.ID as MeasurementTypeID,
	                            MeasurementCharacteristic.Name as MeasurementCharacteristic,
	                            MeasurementCharacteristic.ID as MeasurementCharacteristicID
                            FROM 
	                            Channel JOIN
	                            MeasurementCharacteristic ON MeasurementCharacteristic.ID = Channel.MeasurementCharacteristicID JOIN
	                            MeasurementType ON MeasurementType.ID = Channel.MeasurementTypeID
                        ";

                        data = connection.RetrieveData(query).Select().Select(row => new PeridoicMeasurements() { MeasurementType = row["MeasurementType"].ToString(), MeasurementTypeID = int.Parse(row["MeasurementTypeID"].ToString()), MeasurementCharacteristic = row["MeasurementCharacteristic"].ToString(), MeasurementCharacteristicID = int.Parse(row["MeasurementCharacteristicID"].ToString()) }).OrderBy(x => x.MeasurementCharacteristic).ThenBy(x => x.MeasurementType);
                        s_memoryCache.Add(target, data, new CacheItemPolicy { SlidingExpiration = TimeSpan.FromMinutes(1.0D) });
                    }

                    return data;
                }

            }, cancellationToken);

        }

        private Dictionary<string, List<double[]>> Downsample(Dictionary<string, List<double[]>> dict, int maxSampleCount, Range<DateTime> range)
        {
            Dictionary<string, List<double[]>> returnData = new Dictionary<string, List<double[]>>();

            foreach (KeyValuePair<string, List<double[]>> kvp in dict)
            {
                List<double[]> data = new List<double[]>();
                DateTime epoch = new DateTime(1970, 1, 1);
                double startTime = range.Start.Subtract(epoch).TotalMilliseconds;
                double endTime = range.End.Subtract(epoch).TotalMilliseconds;
                long step = (long)(endTime - startTime) * 1000 / maxSampleCount;
                if (step < 1)
                    step = 1;

                List<double[]> series = kvp.Value;

                if (!returnData.ContainsKey(kvp.Key)) returnData.Add(kvp.Key, new List<double[]>());

                if (series.Count == 0) continue;

                int index = 0;

                for (double n = startTime * 1000; n <= endTime * 1000; n += 2 * step)
                {
                    double[] min = null;
                    double[] max = null;

                    while (index < series.Count && series[index][0] * 1000 < n + 2 * step)
                    {
                        if (min == null || min[1] > series[index][1])
                            min = series[index];

                        if (max == null || max[1] <= series[index][1])
                            max = series[index];

                        ++index;
                    }

                    if (min != null)
                    {
                        if (min[0] < max[0])
                        {
                            data.Add(min);
                            data.Add(max);
                        }
                        else if (min[0] > max[0])
                        {
                            data.Add(max);
                            data.Add(min);
                        }
                        else
                        {
                            data.Add(min);
                        }
                    }
                }

                returnData[kvp.Key] = data;
            }

            return returnData;

        }

        private IEnumerable<Point> QueryHIDS(int channelID, DateTime startDate, DateTime endDate)
        {
            using (AdoDataConnection connection = ConnectionFactory())
            using (API hids = new API())
            {

                string host = connection.ExecuteScalar<string>("SELECT Value FROM Setting WHERE Name = 'HIDS.Host'") ?? "127.0.0.1";
                string tokenID = connection.ExecuteScalar<string>("SELECT Value FROM Setting WHERE Name = 'HIDS.TokenID'") ?? "";
                string pointBucket = connection.ExecuteScalar<string>("SELECT Value FROM Setting WHERE Name = 'HIDS.PointBucket'") ?? "point_bucket";
                string orgID = connection.ExecuteScalar<string>("SELECT Value FROM Setting WHERE Name = 'HIDS.OrganizationID'") ?? "gpa";


                hids.TokenID = tokenID;
                hids.PointBucket = pointBucket;
                hids.OrganizationID = orgID;
                hids.Connect(host);


                List<Point> points = hids.ReadPointsAsync((t) =>
                {
                    t.FilterTags(channelID.ToString("x8"));
                    t.Range(startDate, endDate);
                }).ToListAsync().Result;

                return points;
            }

        }

        private IEnumerable<Point> QueryHIDS(IEnumerable<int> channels, DateTime startDate, DateTime endDate)
        {
            using (AdoDataConnection connection = ConnectionFactory())
            using (API hids = new API())
            {

                string host = connection.ExecuteScalar<string>("SELECT Value FROM Setting WHERE Name = 'HIDS.Host'") ?? "127.0.0.1";
                string tokenID = connection.ExecuteScalar<string>("SELECT Value FROM Setting WHERE Name = 'HIDS.TokenID'") ?? "";
                string pointBucket = connection.ExecuteScalar<string>("SELECT Value FROM Setting WHERE Name = 'HIDS.PointBucket'") ?? "point_bucket";
                string orgID = connection.ExecuteScalar<string>("SELECT Value FROM Setting WHERE Name = 'HIDS.OrganizationID'") ?? "gpa";


                hids.TokenID = tokenID;
                hids.PointBucket = pointBucket;
                hids.OrganizationID = orgID;
                hids.Connect(host);


                List<Point> points = hids.ReadPointsAsync((t) =>
                {
                    t.FilterTags(channels.Select(c => c.ToString("x8")));
                    t.Range(startDate, endDate);
                }).ToListAsync().Result;

                return points;
            }

        }

        #endregion

        #region [ Static ]

        private static MemoryCache s_memoryCache = new MemoryCache("TrendingDataDisplay");

        #endregion
    }
}
