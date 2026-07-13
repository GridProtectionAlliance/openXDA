//******************************************************************************************************
//  AssetHistoryStatsController.cs - Gbtc
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
using System.Web.Http;
using GSF.Data;

namespace openXDA.Controllers.Widgets
{
    /// <summary>
    /// Controller that handles fetching event statistics for the asset associated with an openXDA event.
    /// </summary>
    [RoutePrefix("api/Widgets/AssetHistoryStats")]
    public class AssetHistoryStatsController : ApiController
    {
        private readonly Func<AdoDataConnection> m_connectionFactory;

        /// <summary>
        /// Constructor to pull a connection factory from the XDA controller activator.
        /// </summary>
        public AssetHistoryStatsController(Func<AdoDataConnection> connectionFactory)
        {
            m_connectionFactory = connectionFactory;
        }

        [Route("{EventID:int}"), HttpGet]
        public IHttpActionResult GetAssetHistoryStats(int EventID)
        {
            using (AdoDataConnection connection = m_connectionFactory())
                return Ok(connection.RetrieveData(StatsSQL + @"
                    GROUP BY VoltageKV, Asset.AssetName
                ", EventID));
        }

        [Route("{EventID:int}/{months:int}"), HttpGet]
        public IHttpActionResult GetTimedAssetHistoryStats(int EventID, int months)
        {
            using (AdoDataConnection connection = m_connectionFactory())
                return Ok(connection.RetrieveData(StatsSQL + @"
                    AND Event.StartTime >= DATEADD(MONTH, -{1}, GETDATE())
                    GROUP BY VoltageKV, Asset.AssetName
                ", EventID, months));
        }

        private const string StatsSQL = @"
                    SELECT
                        ROUND(MAX(VPeak)/Asset.VoltageKV/1000, 3) as VPeakMax,
                        MAX(VMax) as VMax,
                        MIN(VMin) as VMin,
                        MAX(IMax) as IMax,
                        MAX(I2tMax) as I2tMax,
                        ROUND(MAX(IPeak),3) as IPeakMax,
                        ROUND(AVG(InitialMW),3) as AVGMW,
                        Asset.AssetName
                    FROM
                        Asset  JOIN
                        Event ON Event.AssetID = Asset.ID JOIN
                        EventStat ON EventStat.EventID = Event.ID  OUTER APPLY
                        (SELECT ROUND(MAX(VMax)/Asset.VoltageKV/1000,3) as VMax FROM (VALUES(VAMax), (VBMax), (VCMax), (VABMax), (VBCMax), (VCAMax)) AS VMaxView(VMax)) as VMax OUTER APPLY
                        (SELECT ROUND(MIN(VMin)/Asset.VoltageKV/1000,3) as VMin FROM (VALUES(VAMin), (VBMin), (VCMin), (VABMin), (VBCMin), (VCAMin)) AS VMinView(VMin)) as VMin OUTER APPLY
                        (SELECT ROUND(MAX(IMax),3) as IMax FROM (VALUES(IAMax), (IBMax), (ICMax)) AS IMaxView(IMax)) as IMax OUTER APPLY
                        (SELECT ROUND(MAX(I2tMax),3) as I2tMax FROM (VALUES(IA2t), (IB2t), (IC2t)) AS I2tView(I2tMax)) as I2tMax
                    WHERE Asset.ID = (SELECT AssetID FROM Event WHERE ID = {0})";
    }
}

