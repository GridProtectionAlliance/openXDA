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
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Text;
using static System.Math;
using MathNet.Numerics.Statistics;
using Newtonsoft.Json;


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
        public const string postRoute = "api/OpenXDA/NewLCSVSEvent";
        //private static readonly HttpClient client = new HttpClient();

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
                TableOperations<Meter> meterTable = new TableOperations<Meter>(connection);
                TableOperations<Customer> customerTable = new TableOperations<Customer>(connection);
                TableOperations<StandardMagDurCurve> stdMagDurCurveTable = new TableOperations<StandardMagDurCurve>(connection);
                TableOperations<DbDisturbance> disturbanceTable = new TableOperations<DbDisturbance>(connection);

                IEnumerable<Event> fileGroupEvents = eventTable.QueryRecordsWhere("FileGroupID={0}", meterDataSet.FileGroup.ID);

                List<LSCVSEvent> lscvsEventList = new List<LSCVSEvent> ();

                foreach (Event evt in fileGroupEvents)
                {
                    EventStat evtSt = eventStatTable.QueryRecordWhere($"EventID = {evt.ID}");

                    if (evtSt.InitialMW is null || evtSt.FinalMW is null ||
                            (!(Abs((double) ((evtSt.InitialMW - evtSt.FinalMW) / evtSt.InitialMW)) > LSCVSSettings.ReportingThreshold))) continue;

                    IEnumerable <Customer> lscvsCustomers = customerTable.QueryRecordsWhere("LSCVS = 1 and ID in " +
                        $"(Select CustomerID from CustomerAsset where AssetID = {evt.AssetID})");

                    if (!lscvsCustomers.Any()) continue;

                    DbDisturbance worstDisturbance = disturbanceTable.QueryRecordWhere("ID=" +
                        $"(Select wd.WorstDisturbanceID from EventWorstDisturbance wd where wd.EventID = {evt.ID})");

                    if (evtSt.VAMin is null || evtSt.VBMin is null || evtSt.VCMin is null) continue;
                    List<double> lineToNeutralVoltages = new List<double>
                    {
                        (double)evtSt.VAMin,
                        (double)evtSt.VBMin,
                        (double)evtSt.VCMin
                    };

                    int curveStandard = 3;
                    if ((lineToNeutralVoltages.Min() / lineToNeutralVoltages.Median()) < LSCVSSettings.TypeThreshold) curveStandard = 1;


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
                    reportData.InsideCurve = (
                        stdMagDurCurveTable.QueryRecordWhere("Name='{0}' and " +
                        $"Area.STIntersects(geometry::Point({worstDisturbance.DurationSeconds},{worstDisturbance.PerUnitMagnitude},0)) = 1", curveStandardNames[curveStandard])
                        != null);

                    if (Min(Min((double)evtSt.VABMin, (double)evtSt.VABMin), (double)evtSt.VBCMin) < 80)
                        reportData.SARFI80Flag = true;
                    else
                        reportData.SARFI80Flag = false;

                    lscvsEventList.Add(reportData);

                    }
                }
                if (lscvsEventList.Count != 0) Post(postRoute, lscvsEventList, GenerateAntiForgeryToken()); //TODO: Add AF Token
            }
        }

        /// <summary>
        /// Gets AntiForgeryToken from openXDA
        /// TODO: Refactor this
        /// </summary>
        /// <returns>string token</returns>
        public string GenerateAntiForgeryToken() //Todo: When refactoring, change this to a static class...
        {
            using (HttpClient client = new HttpClient())
            {

                client.BaseAddress = new Uri(LSCVSSettings.URL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Remove("Authorization");

                if (!LSCVSSettings.UseCodeAuth)
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", 
                        Convert.ToBase64String(Encoding.ASCII.GetBytes($"{LSCVSSettings.Username}:{LSCVSSettings.Password}")));

                HttpResponseMessage response;
                if (LSCVSSettings.UseCodeAuth)
                    response = client.GetAsync($"api/rvhtcode={LSCVSSettings.AuthCode}").Result;
                else
                    response = client.GetAsync("api/rvht").Result;

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Unable to get Anti Forger Token: {response.StatusCode} {response.ReasonPhrase}");
                }

                Task<string> rsp = response.Content.ReadAsStringAsync();
                return response.Content.ReadAsStringAsync().Result;
            }

        }

        /// <summary>
        /// Processes Get request on openXDA + requestURI using provided credentials using Basic auth
        /// </summary>
        /// <param name="requestURI">Path to specific API request</param>
        /// <param name="token">anti forgery token, defaults to null</param>
        /// <returns>string</returns>
        public string Post(string requestURI, IEnumerable<LSCVSEvent> events, string token = null)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(LSCVSSettings.URL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Remove("Authorization");

                if (token != null)
                    client.DefaultRequestHeaders.Add("X-GSF-Verify", token);


                if (!LSCVSSettings.UseCodeAuth)
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($"{LSCVSSettings.Username}:{LSCVSSettings.Password}")));

                string jsonData = JsonConvert.SerializeObject(events);
                HttpContent contentData = new StringContent(jsonData, Encoding.UTF8, "application/json");

                HttpResponseMessage response;
                if (LSCVSSettings.UseCodeAuth)
                    response = client.PostAsync($"{requestURI}?code={LSCVSSettings.AuthCode}", contentData).Result;
                else
                    response = client.PostAsync(requestURI, contentData).Result;


                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception("Status code " + response.StatusCode + ": " + response.ReasonPhrase);
                }

                Task<string> rsp = response.Content.ReadAsStringAsync();
                return response.Content.ReadAsStringAsync().Result;
            }
        }


        #endregion
    }
}
