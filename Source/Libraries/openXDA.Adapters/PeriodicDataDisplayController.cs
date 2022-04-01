//******************************************************************************************************
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
using System.Diagnostics;
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
    public class PeriodicDataDisplayController : ApiController
    {
        #region [ Members ]

        // Nested Types
        public class PeriodicMeasurements
        {
            public string MeasurementType { get; set; }
            public int MeasurementTypeID { get; set; }
            public string MeasurementCharacteristic { get; set; }
            public int MeasurementCharacteristicID { get; set; }
        }

        #endregion

        #region [ Constructors ]

        public PeriodicDataDisplayController(Func<AdoDataConnection> connectionFactory) =>
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
            int meterId = int.Parse(query["MeterID"]);
            int measurementCharacteristicId = int.Parse(query["MeasurementCharacteristicID"]);
            int measurementTypeId = int.Parse(query["MeasurementTypeID"]);
            int harmonicGroup = int.Parse(query["HarmonicGroup"]);

            int pixels = int.Parse(query["pixels"]);
            string type = query["type"] ?? "Average";

            DateTime startDate = DateTime.Parse(query["StartDate"]);
            DateTime endDate = DateTime.Parse(query["EndDate"]);
            DateTime epoch = new DateTime(1970, 1, 1);

            return Task.Factory.StartNew(() =>
            {
                using (AdoDataConnection connection = ConnectionFactory())
                {
                    Stopwatch stopWatch = new Stopwatch();
                    stopWatch.Start();


                    string target = "PeriodicDataDisplay" + meterId.ToString() + measurementTypeId.ToString()+ measurementCharacteristicId.ToString() + harmonicGroup.ToString() + startDate.Subtract(epoch).TotalMilliseconds.ToString() + endDate.Subtract(epoch).TotalMilliseconds.ToString() + type;
                    Dictionary<string, List<double[]>> data = (Dictionary<string, List<double[]>>)s_memoryCache.Get(target);
                    if (data == null)
                    {
                        data = new Dictionary<string, List<double[]>>();
                        IEnumerable<Channel> channels = (new TableOperations<Channel>(connection)).QueryRecordsWhere("MeterID = {0} AND MeasurementCharacteristicID = {1} AND MeasurementTypeID = {2} AND HarmonicGroup = {3}", meterId, measurementCharacteristicId, measurementTypeId, harmonicGroup);

                        IEnumerable<Point> points = QueryHIDS(channels, startDate, endDate);
                        foreach (Point point in points)
                        {
                            Channel channel = channels.First(x => x.ID.ToString("x8") == point.Tag);
                            if (!data.ContainsKey(channel.Name))
                                data.Add(channel.Name, new List<double[]>() { new[] { point.Timestamp.Subtract(epoch).TotalMilliseconds, GetValue(point, type) } });
                            else
                                data[channel.Name].Add(new[] { point.Timestamp.Subtract(epoch).TotalMilliseconds, GetValue(point, type) });
                        }

                        s_memoryCache.Add(target, data, new CacheItemPolicy { SlidingExpiration = TimeSpan.FromMinutes(0.5D) });


                    }
                    Console.WriteLine(stopWatch.Elapsed);
                    return Downsample(data, pixels, new Range<DateTime>(startDate, endDate));
                }

            }, cancellationToken);

        }

        private double GetValue(Point point, string type) {
            if (type == "Average") return point.Average;
            else if (type == "Minimum") return point.Minimum;
            else return point.Maximum;
        }

        private IEnumerable<Point> QueryHIDS(IEnumerable<Channel> channels, DateTime startDate, DateTime endDate) {
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
                    t.FilterTags(channels.Select(c => c.ID.ToString("x8")));
                    t.Range(startDate, endDate);
                }).ToListAsync().Result;

                return points;
            }

        }
        [HttpGet]
        public Task<IOrderedEnumerable<Meter>> GetMeters(CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew(() =>
            {
                using (AdoDataConnection connection = ConnectionFactory())
                {
                    string target = "PeriodicDataDisplayMeters";
                    IOrderedEnumerable<Meter> data = (IOrderedEnumerable<Meter>)s_memoryCache.Get(target);
                    if (data == null)
                    {
                        data = (new TableOperations<Meter>(connection)).QueryRecordsWhere("ID IN (select DISTINCT MeterID from MeterDataQualitySummary)").OrderBy(x => x.AssetKey);
                        s_memoryCache.Add(target, data, new CacheItemPolicy { SlidingExpiration = TimeSpan.FromMinutes(1.0D) });
                    }

                    return data;
                }
            }, cancellationToken);

        }

        [HttpGet]
        public IHttpActionResult GetMeasurementCharacteristics(CancellationToken cancellationToken)
        {
            Dictionary<string, string> query = Request.QueryParameters();
            if (query.ContainsKey("MeterID"))
            {
                int meterId = int.Parse(query["MeterID"]);
                return Ok(GetMeasurementCharacteristicsForWebReport());
            }
            else
                return Ok(GetAllMeasurementCharacteristics());

        }

        private DataTable GetAllMeasurementCharacteristics()
        {
            using (AdoDataConnection connection = ConnectionFactory())
            {
                    string query = @"
                            SELECT DISTINCT 
	                            MeasurementType.Name as MeasurementType,
                                MeasurementType.ID as MeasurementTypeID,
	                            MeasurementCharacteristic.Name as MeasurementCharacteristic,
	                            MeasurementCharacteristic.ID as MeasurementCharacteristicID,
                                HarmonicGroup
                            FROM 
	                            Channel JOIN
	                            MeasurementCharacteristic ON MeasurementCharacteristic.ID = Channel.MeasurementCharacteristicID JOIN
	                            MeasurementType ON MeasurementType.ID = Channel.MeasurementTypeID
                            ORDER BY MeasurementCharacteristic, MeasurementType
                        ";

                    return connection.RetrieveData(query);
            }
        }

        private DataTable GetMeasurementCharacteristicsForWebReport()
        {
            using (AdoDataConnection connection = ConnectionFactory())
            {
                    string query = @"
                            SELECT DISTINCT 
	                            MeasurementType.Name as MeasurementType,
                                MeasurementType.ID as MeasurementTypeID,
	                            MeasurementCharacteristic.Name as MeasurementCharacteristic,
	                            MeasurementCharacteristic.ID as MeasurementCharacteristicID,
                                HarmonicGroup
                            FROM 
	                            Channel JOIN
	                            MeasurementCharacteristic ON MeasurementCharacteristic.ID = Channel.MeasurementCharacteristicID JOIN
	                            MeasurementType ON MeasurementType.ID = Channel.MeasurementTypeID
                        WHERE 
	                        Channel.ID IN (
	                        SELECT 
		                        Channel.ID
	                        FROM 
		                        StepChangeMeasurement JOIN
		                        PQMeasurement ON StepChangeMeasurement.PQMeasurementID = PQMeasurement.ID LEFT JOIN
		                        Channel ON 
			                        PQMeasurement.MeasurementTypeID = Channel.MeasurementTypeID AND
			                        PQMeasurement.MeasurementCharacteristicID = Channel.MeasurementCharacteristicID
                         )
                        ORDER BY MeasurementCharacteristic, MeasurementType

                        ";

                    return connection.RetrieveData(query);
            }
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

        #endregion

        #region [ Static ]

        private static MemoryCache s_memoryCache = new MemoryCache("PeriodicDataDisplay");

        #endregion
    }
}
