//******************************************************************************************************
//  SPCTools/AlarmGroupTestController.cs - Gbtc
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
//  12/01/2020 - C. Lackner
//       Generated original version of source code.
//
//******************************************************************************************************

using GSF;
using GSF.Data;
using GSF.Data.Model;
using GSF.Identity;
using GSF.Web;
using GSF.Web.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using openHistorian.XDALink;
using openXDA.Model;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace openXDA.Controllers
{
    [RoutePrefix("api/SPCTools/AlarmGroupTest")]
    public class AlarmGrouptestController : ApiController
    {
        #region [internal Classes]

        /// <summary>
        /// Request for testing an AlarmGroup Based on settings from the Wizzard
        /// There will be a different Request to Test Alarms from the DataBAse since they don't need to re-evaluate the setpoint
        /// </summary>
       public class WizzardTestRequest
        {
            public List<double> AlarmFactors { get; set; }
            public StaticAlarmCreationController.TokenizerRequest SetPointRequest { get; set; }
            public string Start { get; set; }
            public string End { get; set; }
            public List<int> ChannelID { get; set; }
        }

        /// <summary>
        /// Result of Testing a single Channel
        /// </summary>
        public class ChannelTestResponse
        {
            public int ChannelID { get; set; }
            public double Threshhold { get; set; }
            public List<FactorTest> FactorTests { get; set; }
        }

        /// <summary>
        /// Result of Testing a Factor associated with a single Channel (Since for Time in Alarm and Numebr Raised it is not simple multiplication anymore)
        /// </summary>
        public class FactorTest
        {
            public double TimeInAlarm { get; set; }
            public int NumberRaised { get; set; }
            public double Factor { get; set; }
        }

       



    #endregion

        #region [Properties]

        protected virtual string Connection { get; } = "systemSettings";
        protected virtual string GetRoles { get; } = "Viewer,Administrator";

        #endregion

        #region [HTTPRequests]

       
        /// <summary>
        /// Runs set of historic Data through a Wizzard Based AlarmGroup to evaluate Performance
        /// </summary>
        /// <returns>A List of Errors if applicaple</returns>
        [HttpPost, Route("TestWizzard")]
        public IHttpActionResult TestAlarmGroup([FromBody] WizzardTestRequest postedRequest)
        {
            try
            {
                // Start by Getting Setpoint
                Dictionary<int, List<Point>> Tokendata = StaticAlarmCreationController.createTokenData(postedRequest.SetPointRequest.Channels, DateTime.Parse(postedRequest.SetPointRequest.StartDate), DateTime.Parse(postedRequest.SetPointRequest.EndDate));
                Token root = new Token(postedRequest.SetPointRequest.Value, Tokendata, postedRequest.SetPointRequest.Channels);

                if (!root.Valid || (!root.isScalar && !root.isSlice))
                    return InternalServerError(new Exception("Setpoint Expression is not Valid"));

                if (postedRequest.ChannelID.Count != postedRequest.SetPointRequest.Channels.Count && root.isSlice)
                    return InternalServerError(new Exception("Unable to compute Setpoints for every Channel"));

                // get HIDDS Data (For now get data from ChannelSelectionController) and Evaluate Alarm

                List<double> setPoint = root.ComputeSlice();

                List<ChannelTestResponse> result = postedRequest.ChannelID.Select((id, index) =>
                {
                    double threshhold = (root.isScalar ? setPoint[0] : setPoint[index]);

                    ChannelTestResponse test = new ChannelTestResponse() { ChannelID = id, Threshhold = threshhold };
                    List<double[]> data = StaticAlarmCreationController.createData(id, DateTime.Parse(postedRequest.Start), DateTime.Parse(postedRequest.End));

                    test.FactorTests = postedRequest.AlarmFactors.Select(f => new FactorTest() { TimeInAlarm = 0, NumberRaised = 1, Factor = f }).ToList();
                    test.FactorTests.Add(new FactorTest() { TimeInAlarm = 0, NumberRaised = 1, Factor = 1.0 });

                    return test;
                }).ToList();

                return Ok(result);

            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }


        #endregion

    }
}
