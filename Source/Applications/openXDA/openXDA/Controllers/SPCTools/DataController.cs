//******************************************************************************************************
//  SPCTools/DataController.cs - Gbtc
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
//  10/23/2020 - C. Lackner
//       Generated original version of source code.
//
//******************************************************************************************************

using GSF.Data;
using GSF.Data.Model;
using Newtonsoft.Json.Linq;
using openXDA.Model;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using HIDS;
using openXDA.HIDS;
using openXDA.HIDS.APIExtensions;

namespace openXDA.Controllers
{
    [RoutePrefix("api/SPCTools/Data")]
    public class DataController : ApiController
    {
        #region [internal Classes]

        

        #endregion

        #region [Properties]

        protected virtual string Connection { get; } = "systemSettings";
        protected virtual string GetRoles { get; } = "Viewer,Administrator";

        private static DateTime m_epoch = new DateTime(1970, 1, 1);

        #endregion

        #region [HTTPRequests]

        /// <summary>
        /// Gets the Historical Data requested for a Channel and a SeriesType 
        /// </summary>
        /// <returns> List of DataPoints for the Channel  </returns>
        [HttpPost, Route("HistoryData/{ChannelId}/{SeriesTypeID}")]
        public IHttpActionResult getChannelData(int ChannelId, int SeriesTypeID, [FromBody] DataFilter postedFilter)
        {
            NameValueCollection queryParameters = Request.RequestUri.ParseQueryString();
            string startTime = queryParameters["start"];
            string endTime = queryParameters["end"];

            if ((GetRoles != string.Empty && !User.IsInRole(GetRoles)))
                return Unauthorized();

            try
            {
                DateTime start = DateTime.Parse(startTime);
                DateTime end = DateTime.Parse(endTime);

                IEnumerable<double[]> data;
                using (AdoDataConnection connection = new AdoDataConnection(Connection))
                {
                    SeriesType seriesType = new TableOperations<SeriesType>(connection).QueryRecordWhere("ID={0}", SeriesTypeID);
                    if (seriesType != null && seriesType.Name == "Minimum")
                        data = LoadChannel(ChannelId, start, end).Select(pt => new double[] { pt.Timestamp.Subtract(m_epoch).TotalMilliseconds, pt.Minimum });
                    else if (seriesType != null && seriesType.Name == "Maximum")
                        data = LoadChannel(ChannelId, start, end).Select(pt => new double[] { pt.Timestamp.Subtract(m_epoch).TotalMilliseconds, pt.Maximum });
                    else
                        data = LoadChannel(ChannelId, start, end).Select(pt => new double[] { pt.Timestamp.Subtract(m_epoch).TotalMilliseconds, pt.Average });
                }

                if (postedFilter != null)
                    data = data.Select(pt => {
                        if (postedFilter.FilterZero && pt[1] == 0.0D)
                            return new double[] { pt[0], double.NaN };
                        if (postedFilter.FilterLower && pt[1] < postedFilter.LowerLimit)
                            return new double[] { pt[0], double.NaN };
                        if (postedFilter.FilterUpper && pt[1] > postedFilter.UpperLimit)
                            return new double[] { pt[0], double.NaN };
                        return pt;
                    }).ToList();


                return Ok(data);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }


        }




        #endregion

        #region [ HelperFunction ]
        private List<Point> LoadChannel(int channelID, DateTime start, DateTime end)
        {
            HIDSSettings settings = new HIDSSettings();
            settings.Load();

            
            using (API hids = new API())
            {
                hids.Configure(settings);
                return hids.ReadPointsAsync(new List<string>() { "00000003" },start,end).ToListAsync().Result;
            }

        }
        #endregion
    }
}
