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

        [HttpGet, Route("")]
        public IHttpActionResult Get()
        {
            return Ok("Hello!");
        }



        [HttpGet, Route("WaveformData/I/{eventId}")]
        [ValidateRequestVerificationToken, SuppressMessage("Security", "SG0016", Justification = "CSRF vulnerability handled via ValidateRequestVerificationToken.")]
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

                    if (dataGroup.IA != null)
                        list.Add(new SEBrowser.Model.D3Series()
                        {
                            ChartLabel = "A",
                            Color = "#FF0000",
                            DataPoints = dataGroup.IA.DataPoints.Select(dataPoint => new double[] {
                            dataPoint.Time.Subtract(s_epoch).TotalMilliseconds,
                            dataPoint.Value
                        }).ToList(),
                            Unit = "A"
                        });

                    if (dataGroup.IB != null)
                        list.Add(new SEBrowser.Model.D3Series()
                        {
                            ChartLabel = "B",
                            Color = "#0066CC",
                            DataPoints = dataGroup.IB.DataPoints.Select(dataPoint => new double[] {
                            dataPoint.Time.Subtract(s_epoch).TotalMilliseconds,
                            dataPoint.Value
                        }).ToList(),
                            Unit = "A"
                        });

                    if (dataGroup.IC != null)
                        list.Add(new SEBrowser.Model.D3Series()
                        {
                            ChartLabel = "C",
                            Color = "#33CC33",
                            DataPoints = dataGroup.IC.DataPoints.Select(dataPoint => new double[] {
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
        [ValidateRequestVerificationToken, SuppressMessage("Security", "SG0016", Justification = "CSRF vulnerability handled via ValidateRequestVerificationToken.")]
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

                    if (dataGroup.VA != null)
                        list.Add(new SEBrowser.Model.D3Series()
                        {
                            ChartLabel = "AN",
                            Color = "#A30000",
                            DataPoints = dataGroup.VA.DataPoints.Select(dataPoint => new double[] {
                            dataPoint.Time.Subtract(s_epoch).TotalMilliseconds,
                            dataPoint.Value
                        }).ToList(),
                            Unit = "kV"
                        });

                    if (dataGroup.VB != null)
                        list.Add(new SEBrowser.Model.D3Series()
                        {
                            ChartLabel = "BN",
                            Color = "#0029A3",
                            DataPoints = dataGroup.VB.DataPoints.Select(dataPoint => new double[] {
                            dataPoint.Time.Subtract(s_epoch).TotalMilliseconds,
                            dataPoint.Value
                        }).ToList(),
                            Unit = "kV"
                        });

                    if (dataGroup.VC != null)
                        list.Add(new SEBrowser.Model.D3Series()
                        {
                            ChartLabel = "CN",
                            Color = "#007A29",
                            DataPoints = dataGroup.VC.DataPoints.Select(dataPoint => new double[] {
                            dataPoint.Time.Subtract(s_epoch).TotalMilliseconds,
                            dataPoint.Value
                        }).ToList(),
                            Unit = "kV"
                        });

                    if (dataGroup.VAB != null)
                        list.Add(new SEBrowser.Model.D3Series()
                        {
                            ChartLabel = "AB",
                            Color = "#A30000",
                            DataPoints = dataGroup.VAB.DataPoints.Select(dataPoint => new double[] {
                            dataPoint.Time.Subtract(s_epoch).TotalMilliseconds,
                            dataPoint.Value
                        }).ToList(),
                            Unit = "kV"
                        });

                    if (dataGroup.VBC != null)
                        list.Add(new SEBrowser.Model.D3Series()
                        {
                            ChartLabel = "BC",
                            Color = "#0029A3",
                            DataPoints = dataGroup.VBC.DataPoints.Select(dataPoint => new double[] {
                            dataPoint.Time.Subtract(s_epoch).TotalMilliseconds,
                            dataPoint.Value
                        }).ToList(),
                            Unit = "kV"
                        });

                    if (dataGroup.VCA != null)
                        list.Add(new SEBrowser.Model.D3Series()
                        {
                            ChartLabel = "CA",
                            Color = "#007A29",
                            DataPoints = dataGroup.VCA.DataPoints.Select(dataPoint => new double[] {
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