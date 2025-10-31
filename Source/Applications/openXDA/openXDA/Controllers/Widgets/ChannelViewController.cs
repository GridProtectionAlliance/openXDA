//******************************************************************************************************
//  ChannelViewController.cs - Gbtc
//
//  Copyright � 2020, Grid Protection Alliance.  All Rights Reserved.
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
//  10/28/2025 - G. Santos
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Web.Http;
using GSF.Data;
using GSF.Data.Model;
using GSF.Web.Model;
using openXDA.Model;

namespace openXDA.Controllers.Widgets
{
    /// <summary>
    /// Controller that handles fetching and searching of the <see cref="ChannelDetail"/> model.
    /// </summary> 
    [RoutePrefix("api/Widgets/ChannelView")]
    public class ChannelViewController : ApiController
    {
        Func<AdoDataConnection> m_connectionFactory;

        /// <summary>
        /// Constructor to pull  a connection factory from the XDA controller activator.
        /// </summary> 
        public ChannelViewController(Func<AdoDataConnection> connectionFactory)
        {
            m_connectionFactory = connectionFactory;
        }

        /// <summary>
        /// Searches <see cref="ChannelDetail"/> records for trend channels with an RMS <see cref="MeasurementCharacteristic"/>,
        /// line to neutral <see cref="Phase"/>, and part of the <see cref="Meter"/> associated with the <see cref="Event"/> provided.
        /// </summary>
        /// <param name="eventID">ID of the <see cref="Event"/>.</param>
        [Route("TrendChannels/{eventID:int}"), HttpGet]
        public virtual IHttpActionResult SearchByEvent(int eventID)
        {
            using (AdoDataConnection connection = m_connectionFactory())
            {

                string sql = @"
	                Trend = {0} AND
	                MeasurementCharacteristic = {1} AND
	                Phase like {2} AND
	                MeterID = (SELECT MeterID FROM [Event] WHERE [Event].ID = {3})
                ";

                return Ok(new TableOperations<ChannelDetail>(connection).QueryRecordsWhere(sql, 1, "RMS", "%N", eventID));
            }
        }
    }
}
