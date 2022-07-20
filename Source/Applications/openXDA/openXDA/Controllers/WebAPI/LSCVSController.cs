//******************************************************************************************************
//  LSCVSController.cs - Gbtc
//
//  Copyright © 2022, Grid Protection Alliance.  All Rights Reserved.
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
//  04/07/2022 - Billy Ernest
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
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using FaultData.DataAnalysis;
using GSF.Data;
using GSF.Data.Model;
using GSF.Web.Security;
using HIDS;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using openXDA.Model;

namespace openXDA.Controllers.WebAPI
{



    [RoutePrefix("api/LSCVS")]
    public class LSCVSController : ApiController
    {
        // Static Fields
        private static readonly ILog Log = LogManager.GetLogger(typeof(LSCVSController));
        private static readonly DateTime s_epoch = new DateTime(1970, 1, 1);

        private double FBase
        { 
            get 
            {
                using (AdoDataConnection connection = new AdoDataConnection("systemSettigns"))
                    return double.Parse(connection.ExecuteScalar<string>("SELECT TOP 1 Value FROM Setting WHERE Name = 'DataAnalysis.SystemFrequency' UNION (SELECT '60.0' AS Value)"));
            }
        }

        [HttpGet, Route("")]
        public IHttpActionResult Get()
        {
            return Ok("Hello!");
        }



        [HttpGet, Route("WaveformData/I/{eventId}")]
        public IHttpActionResult GetCurrentData(int eventId)
        {
            try
            {

                List<SEBrowser.Model.D3Series> list = new List<SEBrowser.Model.D3Series>();

                using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
                {
                    Event evt = new TableOperations<Event>(connection).QueryRecordWhere("ID = {0}", eventId);
                    Meter meter = new TableOperations<Meter>(connection).QueryRecordWhere("ID = {0}", evt.MeterID);
                    meter.ConnectionFactory = () => new AdoDataConnection("systemSettings");


                    VIDataGroup dataGroup = GetDataGroup(meter, eventId);
                    VICycleDataGroup cycleData = Transform.ToVICycleDataGroup(dataGroup, FBase, true);

                    if (dataGroup.IA != null)
                        list.Add(new SEBrowser.Model.D3Series()
                        {
                            ChartLabel = "A WaveForm",
                            LegendGroup = "WaveForm",
                            Color = "#FF0000",
                            DataPoints = dataGroup.IA.DataPoints.Select(dataPoint => new double[] {
                            dataPoint.Time.Subtract(s_epoch).TotalMilliseconds,
                            dataPoint.Value
                        }).ToList(),
                            Unit = "A"
                        });

                    if (cycleData.IA != null)
                        list.Add(new SEBrowser.Model.D3Series()
                        {
                            ChartLabel = "A RMS",
                            Color = "#FF0000",
                            LegendGroup = "RMS",
                            DataPoints = cycleData.IA.RMS.DataPoints.Select(dataPoint => new double[] {
                            dataPoint.Time.Subtract(s_epoch).TotalMilliseconds,
                            dataPoint.Value
                        }).ToList(),
                            Unit = "A"
                        });

                    if (dataGroup.IB != null)
                        list.Add(new SEBrowser.Model.D3Series()
                        {
                            ChartLabel = "B WaveForm",
                            Color = "#0066CC",
                            LegendGroup = "WaveForm",
                            DataPoints = dataGroup.IB.DataPoints.Select(dataPoint => new double[] {
                            dataPoint.Time.Subtract(s_epoch).TotalMilliseconds,
                            dataPoint.Value
                        }).ToList(),
                            Unit = "A"
                        });

                    if (cycleData.IB != null)
                        list.Add(new SEBrowser.Model.D3Series()
                        {
                            ChartLabel = "B RMS",
                            Color = "#0066CC",
                            LegendGroup="RMS",
                            DataPoints = cycleData.IB.RMS.DataPoints.Select(dataPoint => new double[] {
                            dataPoint.Time.Subtract(s_epoch).TotalMilliseconds,
                            dataPoint.Value
                        }).ToList(),
                            Unit = "A"
                        });

                    if (dataGroup.IC != null)
                        list.Add(new SEBrowser.Model.D3Series()
                        {
                            ChartLabel = "C WaveForm",
                            Color = "#33CC33",
                            LegendGroup = "WaveForm",
                            DataPoints = dataGroup.IC.DataPoints.Select(dataPoint => new double[] {
                            dataPoint.Time.Subtract(s_epoch).TotalMilliseconds,
                            dataPoint.Value
                        }).ToList(),
                            Unit = "A"
                        });

                    if (cycleData.IC != null)
                        list.Add(new SEBrowser.Model.D3Series()
                        {
                            ChartLabel = "C RMS",
                            Color = "#33CC33",
                            LegendGroup = "RMS",
                            DataPoints = cycleData.IC.RMS.DataPoints.Select(dataPoint => new double[] {
                            dataPoint.Time.Subtract(s_epoch).TotalMilliseconds,
                            dataPoint.Value
                        }).ToList(),
                            Unit = "A"
                        });


                }
                return Ok(list);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet, Route("WaveformData/V/{eventId}")]
        public IHttpActionResult GetVoltageData(int eventId)
        {
            try
            {

                List<SEBrowser.Model.D3Series> list = new List<SEBrowser.Model.D3Series>();

                using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
                {
                    Event evt = new TableOperations<Event>(connection).QueryRecordWhere("ID = {0}", eventId);
                    Meter meter = new TableOperations<Meter>(connection).QueryRecordWhere("ID = {0}", evt.MeterID);
                    meter.ConnectionFactory = () => new AdoDataConnection("systemSettings");

                    VIDataGroup dataGroup = GetDataGroup(meter, eventId);
                    VICycleDataGroup cycleData = Transform.ToVICycleDataGroup(dataGroup, FBase, true);

                    if (dataGroup.VA != null)
                        list.Add(new SEBrowser.Model.D3Series()
                        {
                            ChartLabel = "AN WaveForm",
                            Color = "#A30000",
                            LegendGroup= "WaveForm",
                            DataPoints = dataGroup.VA.DataPoints.Select(dataPoint => new double[] {
                            dataPoint.Time.Subtract(s_epoch).TotalMilliseconds,
                            dataPoint.Value
                        }).ToList(),
                            Unit = "kV"
                        });

                    if (cycleData.VA != null)
                        list.Add(new SEBrowser.Model.D3Series()
                        {
                            ChartLabel = "AN RMS",
                            Color = "#A30000",
                            LegendGroup = "RMS",
                            DataPoints = cycleData.VA.RMS.DataPoints.Select(dataPoint => new double[] {
                            dataPoint.Time.Subtract(s_epoch).TotalMilliseconds,
                            dataPoint.Value
                        }).ToList(),
                            Unit = "kV"
                        });

                    if (dataGroup.VB != null)
                        list.Add(new SEBrowser.Model.D3Series()
                        {
                            ChartLabel = "BN WaveForm",
                            Color = "#0029A3",
                            LegendGroup = "WaveForm",
                            DataPoints = dataGroup.VB.DataPoints.Select(dataPoint => new double[] {
                            dataPoint.Time.Subtract(s_epoch).TotalMilliseconds,
                            dataPoint.Value
                        }).ToList(),
                            Unit = "kV"
                        });

                    if (cycleData.VB != null)
                        list.Add(new SEBrowser.Model.D3Series()
                        {
                            ChartLabel = "BN RMS",
                            Color = "#0029A3",
                            LegendGroup = "RMS",
                            DataPoints = cycleData.VB.RMS.DataPoints.Select(dataPoint => new double[] {
                            dataPoint.Time.Subtract(s_epoch).TotalMilliseconds,
                            dataPoint.Value
                        }).ToList(),
                            Unit = "kV"
                        });

                    if (dataGroup.VC != null)
                        list.Add(new SEBrowser.Model.D3Series()
                        {
                            ChartLabel = "CN WaveForm",
                            Color = "#007A29",
                            LegendGroup = "WaveForm",
                            DataPoints = dataGroup.VC.DataPoints.Select(dataPoint => new double[] {
                            dataPoint.Time.Subtract(s_epoch).TotalMilliseconds,
                            dataPoint.Value
                        }).ToList(),
                            Unit = "kV"
                        });

                    if (cycleData.VC != null)
                        list.Add(new SEBrowser.Model.D3Series()
                        {
                            ChartLabel = "CN RMS",
                            Color = "#007A29",
                            LegendGroup = "RMS",
                            DataPoints = cycleData.VC.RMS.DataPoints.Select(dataPoint => new double[] {
                            dataPoint.Time.Subtract(s_epoch).TotalMilliseconds,
                            dataPoint.Value
                        }).ToList(),
                            Unit = "kV"
                        });

                    if (dataGroup.VAB != null)
                        list.Add(new SEBrowser.Model.D3Series()
                        {
                            ChartLabel = "AB WaveForm",
                            Color = "#A30000",
                            LegendGroup = "WaveForm",
                            DataPoints = dataGroup.VAB.DataPoints.Select(dataPoint => new double[] {
                            dataPoint.Time.Subtract(s_epoch).TotalMilliseconds,
                            dataPoint.Value
                        }).ToList(),
                            Unit = "kV"
                        });

                    if (cycleData.VAB != null)
                        list.Add(new SEBrowser.Model.D3Series()
                        {
                            ChartLabel = "AB RMS",
                            Color = "#A30000",
                            LegendGroup = "RMS",
                            DataPoints = cycleData.VAB.RMS.DataPoints.Select(dataPoint => new double[] {
                            dataPoint.Time.Subtract(s_epoch).TotalMilliseconds,
                            dataPoint.Value
                        }).ToList(),
                            Unit = "kV"
                        });

                    if (dataGroup.VBC != null)
                        list.Add(new SEBrowser.Model.D3Series()
                        {
                            ChartLabel = "BC WaveForm",
                            Color = "#0029A3",
                            LegendGroup = "WaveForm",
                            DataPoints = dataGroup.VBC.DataPoints.Select(dataPoint => new double[] {
                            dataPoint.Time.Subtract(s_epoch).TotalMilliseconds,
                            dataPoint.Value
                        }).ToList(),
                            Unit = "kV"
                        });

                    if (cycleData.VBC != null)
                        list.Add(new SEBrowser.Model.D3Series()
                        {
                            ChartLabel = "BC RMS",
                            Color = "#0029A3",
                            LegendGroup = "RMS",
                            DataPoints = cycleData.VBC.RMS.DataPoints.Select(dataPoint => new double[] {
                            dataPoint.Time.Subtract(s_epoch).TotalMilliseconds,
                            dataPoint.Value
                        }).ToList(),
                            Unit = "kV"
                        });

                    if (dataGroup.VCA != null)
                        list.Add(new SEBrowser.Model.D3Series()
                        {
                            ChartLabel = "CA WaveForm",
                            Color = "#007A29",
                            LegendGroup = "WaveForm",
                            DataPoints = dataGroup.VCA.DataPoints.Select(dataPoint => new double[] {
                            dataPoint.Time.Subtract(s_epoch).TotalMilliseconds,
                            dataPoint.Value
                        }).ToList(),
                            Unit = "kV"
                        });

                    if (cycleData.VCA != null)
                        list.Add(new SEBrowser.Model.D3Series()
                        {
                            ChartLabel = "CA RMS",
                            Color = "#007A29",
                            LegendGroup = "RMS",
                            DataPoints = cycleData.VCA.RMS.DataPoints.Select(dataPoint => new double[] {
                            dataPoint.Time.Subtract(s_epoch).TotalMilliseconds,
                            dataPoint.Value
                        }).ToList(),
                            Unit = "kV"
                        });

                }
                return Ok(list);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        private VIDataGroup GetDataGroup(Meter meter, int eventID)
        {
            List<byte[]> data = ChannelData.DataFromEvent(eventID, () => new AdoDataConnection("systemSettings"));


            DataGroup dataGroup = new DataGroup();
            dataGroup.FromData(meter, data);
            return new VIDataGroup(dataGroup);
        }

    }

}