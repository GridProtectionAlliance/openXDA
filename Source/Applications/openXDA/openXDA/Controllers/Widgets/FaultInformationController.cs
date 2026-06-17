//******************************************************************************************************
//  FaultInformationController.cs - Gbtc
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
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Http;
using GSF.Data;

namespace openXDA.Controllers.Widgets
{
    /// <summary>
    /// Controller that handles fetching fault information for an openXDA event.
    /// </summary>
    [RoutePrefix("api/Widgets/FaultInformation")]
    public class FaultInformationController : ApiController
    {
        private readonly Func<AdoDataConnection> m_connectionFactory;

        /// <summary>
        /// Constructor to pull a connection factory from the XDA controller activator.
        /// </summary>
        public FaultInformationController(Func<AdoDataConnection> connectionFactory)
        {
            m_connectionFactory = connectionFactory;
        }

        [Route("{eventID:int}"), HttpGet]
        public IHttpActionResult GetFaultInformation(int eventID)
        {
            using (AdoDataConnection connection = m_connectionFactory())
            {
                DataTable dataTable = connection.RetrieveData(@"
                    SELECT
                        Event.ID,
                        FaultSummary.Inception as FaultTime,
                        Event.AssetID,
                        Meter.Name as StationName,
                        Location.LocationKey as StationID,
                        LineView.AssetKey as LineAssetKey,
                        LineView.AssetName as LineName,
                        LineView.Length,
                        ROUND(FaultSummary.Distance,2) as FaultDistance,
                        FaultSummary.FaultType,
                        ROUND(FaultSummary.DurationCycles,2) as FaultDuration,
                        FaultSummary.CurrentMagnitude,
                        FaultSummary.ID as FaultID,
                        DoubleEndedFaultDistance.Distance as DblDist
                    FROM
                        Event inner join
                        Meter on Event.MeterID = Meter.ID inner join
                        Location on Meter.LocationID = Location.ID inner join
                        LineView on Event.AssetID = LineView.ID inner join
                        FaultSummary on Event.ID = FaultSummary.EventID and [IsSelectedAlgorithm] = 1 AND IsSuppressed = 0 AND IsValid <> 0 left join
                        DoubleEndedFaultDistance on FaultSummary.ID = DoubleEndedFaultDistance.LocalFaultSummaryID
                    WHERE
                        Event.ID = {0}
                ", eventID);

                if (dataTable.Rows.Count == 0)
                    return Ok();

                DataRow row = dataTable.Rows[0];
                List<Dictionary<string, object>> result = new List<Dictionary<string, object>>();

                foreach (DataColumn col in dataTable.Columns)
                {
                    string key = col.ColumnName;
                    object value = row[col];
                    object formattedValue;

                    switch (key)
                    {
                        case "FaultTime":
                            formattedValue = value != null ? string.Format("{0:yyyy-MM-dd HH:mm:ss.fff} (Central Time)", Convert.ToDateTime(value)) : "";
                            break;
                        case "FaultDuration":
                            formattedValue = value != null ? string.Format("{0} cycles / {1:f2} ms", value, Convert.ToDouble(value) * 16.6D) : "";
                            break;
                        case "FaultType":
                            formattedValue = value ?? "";
                            break;
                        case "Location":
                            formattedValue = value != null && row["StationName"] != null && row["StationID"] != null && row["LineName"] != null && row["LineAssetKey"] != null
                                ? string.Format("{0} miles from {1} ({2}) on {3} ({4})", value, row["StationName"], row["StationID"], row["LineName"], row["LineAssetKey"])
                                : "";
                            break;
                        case "DoubleEndedLocation":
                            formattedValue = value != null && row["StationName"] != null ? string.Format("{0} miles from {1}", value, row["StationName"]) : "";
                            break;
                        case "FaultDistance":
                        case "DblDist":
                        case "Length":
                            formattedValue = value != null ? Convert.ToDouble(value) : (object)null;
                            break;
                        default:
                            formattedValue = value;
                            break;
                    }

                    result.Add(new Dictionary<string, object>
                    {
                        { "Key", key },
                        { "Value", formattedValue }
                    });
                }

                return Ok(result);
            }
        }

        [Route("GetLinks/{category}"), HttpGet]
        public IHttpActionResult GetLinks(string category)
        {
            using (AdoDataConnection connection = m_connectionFactory())
                return Ok(connection.RetrieveData("SELECT * FROM [SEBrowser.Links] WHERE Name LIKE {0} + '%'", category));
        }
    }
}
