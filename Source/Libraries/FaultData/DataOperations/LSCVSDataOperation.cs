//******************************************************************************************************
//  LSCVSDataOperation.cs - Gbtc
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
//  04/11/2022 - G. Santos
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using FaultData.DataSets;
using GSF.Configuration;
using GSF.Data;
using GSF.Data.Model;
using openXDA.Model;
using LSCVS.Model;
using log4net;
using openXDA.Configuration;
using DbDisturbance = openXDA.Model.Disturbance;
using Customer = openXDA.Model.Customer;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Text;
using static System.Math;
using MathNet.Numerics.Statistics;
using Newtonsoft.Json;
using openXDA.APIAuthentication;

namespace FaultData.DataOperations
{
    public class LSCVSDataOperation : DataOperationBase<MeterDataSet>
    {
        #region [ Members ]

        #endregion

        #region [ Static]

        private static readonly ILog Log = LogManager.GetLogger(typeof(LSCVSDataOperation));
        private static readonly Dictionary<int, string> curveStandardNames =
            new Dictionary<int, string>
            {
                {1, "IEEE 1668 Type I & II"},
                {3, "IEEE 1668 Type III"}
            };
        public const string postRoute = "/api/OpenXDA/NewLCSVSEvent";

       #endregion

        #region [ Properties ]

        [Category]
        [SettingName(LSCVSSection.CategoryName)]
        public LSCVSSection LSCVSSettings { get; }
            = new LSCVSSection();

        #endregion

        #region [ Methods ]

        public override void Execute(MeterDataSet meterDataSet)
        {
            using (AdoDataConnection connection = meterDataSet.CreateDbConnection())
            {
                TableOperations<Event> eventTable = new TableOperations<Event>(connection);
                TableOperations<EventStat> eventStatTable = new TableOperations<EventStat>(connection);
                TableOperations<Customer> customerTable = new TableOperations<Customer>(connection);
                TableOperations<DbDisturbance> disturbanceTable = new TableOperations<DbDisturbance>(connection);

                IEnumerable<Event> fileGroupEvents = eventTable.QueryRecordsWhere("FileGroupID={0}", meterDataSet.FileGroup.ID);
                IEnumerable<EventType> allowedEventTypes = new TableOperations<EventType>(connection).QueryRecordsWhere("Name={0} AND Category={1}", "Sag", "PQ");

                List<LSCVSEvent> lscvsEventList = new List<LSCVSEvent> ();

                int rejectionBadData = 0;
                int rejectionThreshold = 0;
                int rejectionNoCustomer = 0;

                foreach (Event evt in fileGroupEvents)
                {
                    EventStat evtSt = eventStatTable.QueryRecordWhere($"EventID = {evt.ID}");

                    if (evtSt.InitialMW is null || evtSt.FinalMW is null ||
                            (!(Abs((double)((evtSt.InitialMW - evtSt.FinalMW) / evtSt.InitialMW)) > LSCVSSettings.ReportingThreshold)))
                    {
                        rejectionThreshold++;
                        continue;
                    }

                    IEnumerable<Customer> lscvsCustomers = customerTable.QueryRecordsWhere("LSCVS = 1 AND (" +
                        "(ID in (Select CustomerID from CustomerAsset where AssetID = {0})) OR " +
                        "(ID in (Select CustomerID from CustomerMeter where MeterID = {1})))", evt.AssetID, evt.MeterID);

                    if (!lscvsCustomers.Any())
                    {
                        rejectionNoCustomer++;
                        continue;
                    }

                    IEnumerable<DbDisturbance> worstDisturbances = disturbanceTable.QueryRecordsWhere("ID in " +
                        $"(Select wd.WorstDisturbanceID from EventWorstDisturbance wd where wd.EventID = {evt.ID})");

                    if (evtSt.VAMin is null || evtSt.VBMin is null || evtSt.VCMin is null)
                    {
                        rejectionBadData++;
                        continue;
                    }

                    List<double> lineToNeutralVoltages = new List<double>
                    {
                        (double)evtSt.VAMin,
                        (double)evtSt.VBMin,
                        (double)evtSt.VCMin
                    };

                    int curveStandard = 3;
                    if ((lineToNeutralVoltages.Min() / lineToNeutralVoltages.Median()) < LSCVSSettings.TypeThreshold) curveStandard = 1;

                    foreach (DbDisturbance worstDisturbance in worstDisturbances)
                    {
                        // Skip non-sag events
                        if (!allowedEventTypes.Any(type => type.ID == worstDisturbance.EventTypeID)) continue;

                        foreach (Customer customer in lscvsCustomers)
                        {
                            LSCVSEvent reportData = new LSCVSEvent()
                            {
                                MeterID = evt.MeterID,
                                EventStart = worstDisturbance.StartTime,
                                Magnitude = worstDisturbance.PerUnitMagnitude,
                                Duration = worstDisturbance.DurationSeconds * 1000,
                                EventType = curveStandard,
                                IntialMW = (double)evtSt.InitialMW, // Should be a safe cast, null is checked for above
                                FinalMW = (double)evtSt.FinalMW,
                                OpenXDAID = evt.ID,
                                CustomerID = customer.ID
                            };

                            string curveCheck = $"Area.STIntersects(geometry::Point({worstDisturbance.DurationSeconds},{worstDisturbance.PerUnitMagnitude},0)) = 1";
                            if (connection.ExecuteScalar<int>("SELECT COUNT(ID) FROM StandardMagDurCurve WHERE Name={0}", curveStandardNames[curveStandard]) == 1)
                                reportData.InsideCurve = connection.ExecuteScalar<int>($"SELECT COUNT(ID) FROM StandardMagDurCurve WHERE Name={{0}} AND {curveCheck}", curveStandardNames[curveStandard]) == 1;
                            else
                                Log.Error($"Standard Mag-Dur Curve for {curveStandardNames[curveStandard]} was not found");

                            if (Min(Min((double)evtSt.VABMin, (double)evtSt.VABMin), (double)evtSt.VBCMin) < 80)
                                reportData.SARFI80Flag = true;
                            else
                                reportData.SARFI80Flag = false;

                            lscvsEventList.Add(reportData);

                        }
                    }
                }

                if (rejectionBadData + rejectionNoCustomer + rejectionThreshold > 0)
                {
                    Log.Info($"{rejectionBadData + rejectionNoCustomer + rejectionThreshold} lscvs events not sent for meter {meterDataSet.Meter.Name}:");
                    Log.Info($@"    {rejectionThreshold} due to not meeting MW ratio; {rejectionNoCustomer} due to not having customers assigned to relavent meters; {rejectionBadData} due to not having all line-neutral phase data.");
                }

                if (lscvsEventList.Count != 0)
                {
                    Log.Info($"Sending {lscvsEventList.Count} lscvs events found for meter {meterDataSet.Meter.Name}");
                    
                    APIQuery query = new APIQuery(LSCVSSettings.APIKey, LSCVSSettings.APIToken, LSCVSSettings.URL.Split(';'));

                    string jsonData = JsonConvert.SerializeObject(lscvsEventList);
                    HttpContent contentData = new StringContent(jsonData, Encoding.UTF8, "application/json");

                    void ConfigureRequest(HttpRequestMessage request)
                    {
                        request.Method = HttpMethod.Post;
                        request.Content = contentData;
                    }

                    HttpResponseMessage responseMessage = query.SendWebRequestAsync(ConfigureRequest, postRoute).Result;
                    if (!responseMessage.IsSuccessStatusCode)
                    {
                        throw new Exception("Status code " + responseMessage.StatusCode + ": " + responseMessage.ReasonPhrase);
                    }
                }
                
            }
        }

        #endregion
    }
}
