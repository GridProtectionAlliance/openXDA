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

using GSF;
using GSF.Data;
using GSF.Data.Model;
using GSF.Web;
using GSF.Web.Model;
using openHistorian.XDALink;
using openXDA.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace openXDA.Adapters
{
    public class PeriodicDataDisplayController : ApiController
    {
        #region [ Members ]
        // Fields
        #endregion
        #region [ Properties ]
        #endregion

        #region [ Constructors ]
        #endregion

        #region [ Static ]
        private static MemoryCache s_memoryCache;

        static PeriodicDataDisplayController()
        {
            s_memoryCache = new MemoryCache("PeriodicDataDisplay");
        }
        #endregion


        #region [ Methods ]
        [HttpGet]
        public Task<Dictionary<string, List<double[]>>> GetData(CancellationToken cancellationToken)
        {
            Dictionary<string, string> query = Request.QueryParameters();
            int meterId = int.Parse(query["MeterID"]);
            int measurementCharacteristicId = int.Parse(query["MeasurementCharacteristicID"]);
            int pixels = int.Parse(query["pixels"]);
            string type = query["type"] ?? "Average";

            DateTime startDate = DateTime.Parse(query["StartDate"]);
            DateTime endDate = DateTime.Parse(query["EndDate"]);
            DateTime epoch = new DateTime(1970, 1, 1);

            return Task.Factory.StartNew(() =>
            {
                using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
                {
                    Stopwatch stopWatch = new Stopwatch();
                    stopWatch.Start();


                    string target = "PeriodicDataDisplay" + meterId.ToString() + measurementCharacteristicId.ToString() + startDate.Subtract(epoch).TotalMilliseconds.ToString() + endDate.Subtract(epoch).TotalMilliseconds.ToString() + type;
                    Dictionary<string, List<double[]>> data = (Dictionary<string, List<double[]>>)s_memoryCache.Get(target);
                    if(data == null)
                    {
                        data = new Dictionary<string, List<double[]>>();
                        IEnumerable<Channel> channels = (new TableOperations<Channel>(connection)).QueryRecordsWhere("MeterID = {0} AND MeasurementCharacteristicID = {1}", meterId, measurementCharacteristicId);

                        string historianServer = connection.ExecuteScalar<string>("SELECT Value FROM Setting WHERE Name = 'Historian.Server'") ?? "127.0.0.1";
                        string historianInstance = connection.ExecuteScalar<string>("SELECT Value FROM Setting WHERE Name = 'Historian.Instance'") ?? "XDA";

                        using (Historian historian = new Historian(historianServer, historianInstance))
                        {
                            foreach (openHistorian.XDALink.TrendingDataPoint point in historian.Read(channels.Select(x => x.ID), startDate, endDate))
                            {
                                if (point.SeriesID.ToString() == type)
                                {
                                    Channel channel = channels.First(x => x.ID == point.ChannelID);
                                    if (!data.ContainsKey(channel.Name))
                                        data.Add(channel.Name, new List<double[]>() { new[] { point.Timestamp.Subtract(epoch).TotalMilliseconds, point.Value } });
                                    else
                                        data[channel.Name].Add(new[] { point.Timestamp.Subtract(epoch).TotalMilliseconds, point.Value });
                                }
                            }
                        }

                        s_memoryCache.Add(target, data, new CacheItemPolicy { SlidingExpiration = TimeSpan.FromMinutes(0.5D) });

                    }
                    Console.WriteLine(stopWatch.Elapsed);
                    return Downsample(data, pixels, new Range<DateTime>(startDate, endDate));
                }

            }, cancellationToken);

        }

        [HttpGet]
        public Task<IOrderedEnumerable<Meter>> GetMeters(CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew(() =>
            {
                using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
                {
                    string target = "PeriodicDataDisplayMeters";
                    IOrderedEnumerable<Meter> data = (IOrderedEnumerable<Meter>)s_memoryCache.Get(target);
                    if (data == null)
                    {
                        data = (new TableOperations<Meter>(connection)).QueryRecords().OrderBy(x => x.AssetKey);
                        s_memoryCache.Add(target, data, new CacheItemPolicy { SlidingExpiration = TimeSpan.FromMinutes(1.0D) });
                    }

                    return data;
                }
            }, cancellationToken);

        }

        [HttpGet]
        public Task<IOrderedEnumerable<MeasurementCharacteristic>> GetMeasurementCharacteristics(CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew(() =>
            {
                using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
                {
                    string target = "PeriodicDataDisplayMeasurementCharacteristics";
                    IOrderedEnumerable<MeasurementCharacteristic> data = (IOrderedEnumerable<MeasurementCharacteristic>)s_memoryCache.Get(target);
                    if (data == null)
                    {
                        data = (new TableOperations<MeasurementCharacteristic>(connection)).QueryRecords().OrderBy(x => x.Name);
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




        #endregion

    }
}
