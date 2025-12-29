//******************************************************************************************************
//  OpenSEEController.cs - Gbtc
//
//  Copyright © 2023, Grid Protection Alliance.  All Rights Reserved.
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
//  08/22/2023 - Preston Crawford
//       Generated original version of source code.
//  10/28/2025 - G. Santos
//      Migrated controller from EventWidgets to XDA (formerly Controllers/OpenSEE.cs)
//
//******************************************************************************************************

using FaultData.DataAnalysis;
using GSF.Data;
using GSF.Data.Model;
using GSF.Web;
using openXDA.Model;
using System.Collections.Generic;
using System;
using System.Data;
using System.Web.Http;
using System.Runtime.Caching;
using System.Threading.Tasks;
using System.Linq;

namespace openXDA.Controllers.Widgets
{
    /// <summary>
    /// Controller that handles fetching openSEE time event data for an openXDA <see cref="Event"/>
    /// </summary>
    [RoutePrefix("api/Widgets/OpenSEE")]
    public class OpenSEEController : ApiController
    {
        MemoryCache s_memoryCache = new MemoryCache("OpenXDA");
        protected const int MAX_SAMPLE_COUNT = 1200;
        Func<AdoDataConnection> m_connectionFactory;

        /// <summary>
        /// Constructor to pull  a connection factory from the XDA controller activator.
        /// </summary> 
        public OpenSEEController(Func<AdoDataConnection> connectionFactory)
        {
            m_connectionFactory = connectionFactory;
        }

        /// <summary>
        /// Endpoint that handles fetching openSEE event chart data.
        /// </summary>
        /// <param name="type"><see cref="string"/> that represents the measurement type of the channels data is being pulled from. ("Voltage", "Current", "TripCoilCurrent" are valid values)</param>
        /// <param name="postData"><see cref="EventPost"/> that contains query information.</param>
        [Route("GetData/{type}"), HttpPost]
        public IHttpActionResult GetOpenSEEData([FromBody] EventPost postData, string type)
        {
            using (AdoDataConnection connection = m_connectionFactory())
            {
                if (!postData.IsCustomerAuthorized(connection))
                    return Unauthorized();

                Dictionary<string, string> query = Request.QueryParameters();
                DateTime epoch = new DateTime(1970, 1, 1);

                Event evt = new TableOperations<Event>(connection).QueryRecordWhere("ID = {0}", postData.EventID);
                if (evt is null)
                    throw new InvalidOperationException("Unable to find event with ID " + postData.EventID);

                Meter meter = new TableOperations<Meter>(connection).QueryRecordWhere("ID = {0}", evt.MeterID);
                meter.ConnectionFactory = () => m_connectionFactory();

                DateTime startTime = (query.ContainsKey("startDate") ? DateTime.Parse(query["startDate"]) : evt.StartTime);
                DateTime endTime = (query.ContainsKey("endDate") ? DateTime.Parse(query["endDate"]) : evt.EndTime);
                DataGroup dataGroup;

                // No need to filter by customer: this is already constrained to a meter/asset on the event
                dataGroup = QueryDataGroup(postData.EventID, meter);
                Dictionary<string, IEnumerable<double[]>> returnData = new Dictionary<string, IEnumerable<double[]>>();
                bool hasVoltLN = dataGroup.DataSeries.Select(x => x.SeriesInfo.Channel.Phase.Name).Where(x => x.Contains("N")).Any();
                foreach (var series in dataGroup.DataSeries)
                {
                    List<double[]> data = series.DataPoints.Select(dp => new double[2] { (dp.Time - epoch).TotalMilliseconds, dp.Value }).ToList();
                    if (type == "Voltage")
                    {
                        if (series.SeriesInfo.Channel.MeasurementType.Name == "Voltage" && series.SeriesInfo.Channel.MeasurementCharacteristic.Name == "Instantaneous" && series.SeriesInfo.Channel.Phase.Name.Contains("N"))
                        {
                            if (!returnData.ContainsKey("V" + series.SeriesInfo.Channel.Phase.Name))
                                returnData.Add("V" + series.SeriesInfo.Channel.Phase.Name, Downsample(data));
                        }
                        else if (series.SeriesInfo.Channel.MeasurementType.Name == "Voltage" && series.SeriesInfo.Channel.MeasurementCharacteristic.Name == "Instantaneous" && !hasVoltLN)
                        {
                            if (!returnData.ContainsKey("V" + series.SeriesInfo.Channel.Phase.Name))
                                returnData.Add("V" + series.SeriesInfo.Channel.Phase.Name, Downsample(data));
                        }

                    }
                    else if (type == "TripCoilCurrent")
                    {
                        if (series.SeriesInfo.Channel.MeasurementType.Name == "TripCoilCurrent" && series.SeriesInfo.Channel.MeasurementCharacteristic.Name == "Instantaneous")
                        {
                            if (!returnData.ContainsKey("TCE" + series.SeriesInfo.Channel.Phase.Name))
                                returnData.Add("TCE" + series.SeriesInfo.Channel.Phase.Name, Downsample(data));
                        }
                    }
                    else
                    {
                        if (series.SeriesInfo.Channel.MeasurementType.Name == "Current" && series.SeriesInfo.Channel.MeasurementCharacteristic.Name == "Instantaneous")
                        {
                            if (!returnData.ContainsKey("I" + series.SeriesInfo.Channel.Phase.Name))
                                returnData.Add("I" + series.SeriesInfo.Channel.Phase.Name, Downsample(data));
                        }
                    }

                }

                return Ok(returnData);
            }
        }

        private DataGroup QueryDataGroup(int eventID, Meter meter)
        {
            string target = $"DataGroup-{eventID}";

            Task<DataGroup> dataGroupTask = new Task<DataGroup>(() =>
            {
                List<byte[]> data = ChannelData.DataFromEvent(eventID, () => m_connectionFactory());
                return ToDataGroup(meter, data);

            });

            if (s_memoryCache.Add(target, dataGroupTask, new CacheItemPolicy { SlidingExpiration = TimeSpan.FromMinutes(10.0D) }))
                dataGroupTask.Start();

            dataGroupTask = (Task<DataGroup>)s_memoryCache.Get(target);

            return dataGroupTask.Result;

        }

        private DataGroup ToDataGroup(Meter meter, List<byte[]> data)
        {
            DataGroup dataGroup = new DataGroup();
            dataGroup.FromData(meter, data);
            VIDataGroup vIDataGroup = new VIDataGroup(dataGroup);
            return vIDataGroup.ToDataGroup();
        }

        private List<double[]> Downsample(List<double[]> series)
        {
            List<double[]> data = new List<double[]>();
            DateTime epoch = new DateTime(1970, 1, 1);
            double startTime = series.First()[0];
            double endTime = series.Last()[0];
            int step = (int)(endTime * 1000 - startTime * 1000) / MAX_SAMPLE_COUNT;
            if (step < 1)
                step = 1;

            series = series.Where(x => x[0] >= startTime && x[0] <= endTime).ToList();

            int index = 0;

            for (double n = startTime * 1000; n <= endTime * 1000; n += 2 * step)
            {
                double[] min = null;
                double[] max = null;

                while (index < series.Count() && series[index][0] * 1000 < n + 2 * step)
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

            return data;

        }
    }
}