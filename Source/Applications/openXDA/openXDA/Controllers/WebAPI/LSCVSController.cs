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
using System.Linq;
using System.Web.Http;
using FaultData.DataAnalysis;
using GSF.Data;
using GSF.Data.Model;
using GSF.Web.Model;
using log4net;
using openXDA.Model;

namespace openXDA.Controllers.WebAPI
{

    [RoutePrefix("api/LSCVSAccount")]
    public class LSCVSAccountController : ModelController<SystemCenter.Model.LSCVSAccount> { }

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
                using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
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

                    double Vbase = cycleData.VBase;
                    double SBase = connection.ExecuteScalar<double?>("SELECT Value FROM Setting WHERE Name = 'SystemMVABase'") ?? 100.0;

                    if (cycleData.IA != null)
                        list.Add(new SEBrowser.Model.D3Series()
                        {
                            ChartLabel = "A RMS",
                            Color = "Ia",
                            LegendGroup = "RMS",
                            DataPoints = cycleData.IA.RMS.DataPoints.Select(dataPoint => new double[] {
                            dataPoint.Time.Subtract(s_epoch).TotalMilliseconds,
                            dataPoint.Value
                        }).ToList(),
                            Unit = "A",
                            BaseValue = (Sbase / (Math.Sqrt(3) * Vbase * 1000));
                        });

                 
                    if (cycleData.IB != null)
                        list.Add(new SEBrowser.Model.D3Series()
                        {
                            ChartLabel = "B RMS",
                            Color = "Ib",
                            LegendGroup="RMS",
                            DataPoints = cycleData.IB.RMS.DataPoints.Select(dataPoint => new double[] {
                            dataPoint.Time.Subtract(s_epoch).TotalMilliseconds,
                            dataPoint.Value
                        }).ToList(),
                            Unit = "A",
                            BaseValue = (Sbase / (Math.Sqrt(3) * Vbase * 1000));
                        });

                    if (cycleData.IC != null)
                        list.Add(new SEBrowser.Model.D3Series()
                        {
                            ChartLabel = "C RMS",
                            Color = "Ic",
                            LegendGroup = "RMS",
                            DataPoints = cycleData.IC.RMS.DataPoints.Select(dataPoint => new double[] {
                            dataPoint.Time.Subtract(s_epoch).TotalMilliseconds,
                            dataPoint.Value
                        }).ToList(),
                            Unit = "A",
                            BaseValue = (Sbase / (Math.Sqrt(3) * Vbase * 1000));
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

                   
                    if (cycleData.VA != null)
                        list.Add(new SEBrowser.Model.D3Series()
                        {
                            ChartLabel = "AN RMS",
                            Color = "Va",
                            LegendGroup = "RMS",
                            DataPoints = cycleData.VA.RMS.DataPoints.Select(dataPoint => new double[] {
                            dataPoint.Time.Subtract(s_epoch).TotalMilliseconds,
                            dataPoint.Value
                        }).ToList(),
                            Unit = "kV",
                            BaseValue = cycleData.Vbase /Math.Sqrt3
                        });

                    
                    if (cycleData.VB != null)
                        list.Add(new SEBrowser.Model.D3Series()
                        {
                            ChartLabel = "BN RMS",
                            Color = "Vb",
                            LegendGroup = "RMS",
                            DataPoints = cycleData.VB.RMS.DataPoints.Select(dataPoint => new double[] {
                            dataPoint.Time.Subtract(s_epoch).TotalMilliseconds,
                            dataPoint.Value
                        }).ToList(),
                            Unit = "kV",
                            BaseValue = cycleData.Vbase /Math.Sqrt3
                        });

                    if (cycleData.VC != null)
                        list.Add(new SEBrowser.Model.D3Series()
                        {
                            ChartLabel = "CN RMS",
                            Color = "Vc",
                            LegendGroup = "RMS",
                            DataPoints = cycleData.VC.RMS.DataPoints.Select(dataPoint => new double[] {
                            dataPoint.Time.Subtract(s_epoch).TotalMilliseconds,
                            dataPoint.Value
                        }).ToList(),
                            Unit = "kV",
                            BaseValue = cycleData.Vbase /Math.Sqrt3
                        });

                  

                    if (cycleData.VAB != null)
                        list.Add(new SEBrowser.Model.D3Series()
                        {
                            ChartLabel = "AB RMS",
                            Color = "Vab",
                            LegendGroup = "RMS",
                            DataPoints = cycleData.VAB.RMS.DataPoints.Select(dataPoint => new double[] {
                            dataPoint.Time.Subtract(s_epoch).TotalMilliseconds,
                            dataPoint.Value
                        }).ToList(),
                            Unit = "kV",
                            BaseValue = cycleData.Vbase
                        });

                    
                    if (cycleData.VBC != null)
                        list.Add(new SEBrowser.Model.D3Series()
                        {
                            ChartLabel = "BC RMS",
                            Color = "Vbc",
                            LegendGroup = "RMS",
                            DataPoints = cycleData.VBC.RMS.DataPoints.Select(dataPoint => new double[] {
                            dataPoint.Time.Subtract(s_epoch).TotalMilliseconds,
                            dataPoint.Value
                        }).ToList(),
                            Unit = "kV",
                            BaseValue = cycleData.Vbase
                        });

                   

                    if (cycleData.VCA != null)
                        list.Add(new SEBrowser.Model.D3Series()
                        {
                            ChartLabel = "CA RMS",
                            Color = "Vca",
                            LegendGroup = "RMS",
                            DataPoints = cycleData.VCA.RMS.DataPoints.Select(dataPoint => new double[] {
                            dataPoint.Time.Subtract(s_epoch).TotalMilliseconds,
                            dataPoint.Value
                        }).ToList(),
                            Unit = "kV",
                            BaseValue = cycleData.Vbase
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