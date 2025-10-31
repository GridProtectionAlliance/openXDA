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
        protected string SettingsCategory => "systemSettings";
        protected const int MAX_SAMPLE_COUNT = 1200;

        /// <summary>
        /// Endpoint that handles fetching openSEE event chart data.
        /// </summary>
        /// <remarks>
        /// This event relies on a query string with the following parameters:<br/>
        /// eventID that is an <see cref="int"/> that represents the ID of the event in the XDA database.<br/>
        /// type that is a <see cref="string"/> that represents the measurement type of the channels data is being pulled from.<br/>
        /// Note: supported type values are "Voltage", "Current", and "TripCoilCurrent".
        /// </remarks>
        [Route("GetData/{type}/{eventID:int}")]
        public IHttpActionResult GetOpenSEEData(string type, int eventID)
        {
            using (AdoDataConnection connection = new AdoDataConnection(SettingsCategory))
            {
                Dictionary<string, string> query = Request.QueryParameters();
                DateTime epoch = new DateTime(1970, 1, 1);

                Event evt = new TableOperations<Event>(connection).QueryRecordWhere("ID = {0}", eventID);
                if (evt is null)
                    throw new InvalidOperationException("Unable to find event with ID " + eventID);

                Meter meter = new TableOperations<Meter>(connection).QueryRecordWhere("ID = {0}", evt.MeterID);
                meter.ConnectionFactory = () => new AdoDataConnection(SettingsCategory);


                DateTime startTime = (query.ContainsKey("startDate") ? DateTime.Parse(query["startDate"]) : evt.StartTime);
                DateTime endTime = (query.ContainsKey("endDate") ? DateTime.Parse(query["endDate"]) : evt.EndTime);
                DataGroup dataGroup;
                dataGroup = QueryDataGroup(eventID, meter);
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
                List<byte[]> data = ChannelData.DataFromEvent(eventID, () => new AdoDataConnection(SettingsCategory));
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