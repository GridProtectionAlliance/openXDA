//******************************************************************************************************
//  PeriodicDataDisplay1.cs - Gbtc
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
//  05/25/2018 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************

using GSF;
using GSF.Data;
using GSF.Data.Model;
using GSF.Web;
using GSF.Web.Model;
using openHistorian.XDALink;
using openXDA.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace openXDA.Adapters
{
    public class DataQualitySummaryController : ApiController
    {
        #region [ Static ]
        private static MemoryCache s_memoryCache;

        static DataQualitySummaryController()
        {
            s_memoryCache = new MemoryCache("DataQualitySummary");
        }
        #endregion


        #region [ Methods ]
        [HttpGet]
        public Task<DataTable> GetData(CancellationToken cancellationToken)
        {
            Dictionary<string, string> query = Request.QueryParameters();
            int meterID = int.Parse(query["meterID"]);
            string level = query["level"];
            string sortField = query["sortField"];
            bool ascending = bool.Parse(query["ascending"]);
            DateTime date = DateTime.ParseExact(query["date"], "yyyy-MM-dd", CultureInfo.CurrentCulture);

            return Task.Factory.StartNew(() =>
            {
                using (AdoDataConnection connection = new AdoDataConnection("systemSettings")) {
                    if (level == "Meter")
                        return GetDataMeters(connection, date, sortField, ascending);
                    else
                        return GetDataChannels(connection, date, meterID, sortField, ascending);

                }
            }, cancellationToken);

        }


        private DataTable GetDataMeters(AdoDataConnection connection, DateTime date, string sortField, bool ascending)
        {
            DataTable table;
            if (s_memoryCache.Contains("Meters" + date.ToString()))
            {
                table = (DataTable)s_memoryCache.Get("Meters" + date.ToString());
            }
            else {
                table = connection.RetrieveData(@"
                    SELECT 
                        Meter.ID,
	                    Meter.Name,
	                    SUM(COALESCE(tbl.Expected,0)) as Expected,
	                    SUM(COALESCE(tbl.Missing,0)) as Missing,
	                    SUM(COALESCE(tbl.Latched,0)) as Latched,
	                    SUM(COALESCE(tbl.Unreasonable,0)) as Unreasonable
                    FROM
	                    Meter JOIN
	                    (
	                    SELECT 
		                    Channel.ID,
		                    Channel.MeterId,
		                    Channel.Name,
		                    ChannelDataQualitySummary.Date,
		                    Channel.SamplesPerHour * 24 as Expected,
		                    (Channel.SamplesPerHour * 24) - COALESCE(ChannelDataQualitySummary.GoodPoints,0) - COALESCE(ChannelDataQualitySummary.LatchedPoints,0) - COALESCE(ChannelDataQualitySummary.UnreasonablePoints,0) - COALESCE(ChannelDataQualitySummary.NoncongruentPoints,0) as Missing,
		                    COALESCE(ChannelDataQualitySummary.LatchedPoints, 0) as Latched,
		                    COALESCE(ChannelDataQualitySummary.UnreasonablePoints, 0) as Unreasonable
	                    FROM
		                    Channel JOIN
		                    ChannelDataQualitySummary ON Channel.ID = ChannelDataQualitySummary.ChannelID
	                    ) as tbl on Meter.ID = tbl.MeterID
                    WHERE Date BETWEEN {0} AND {1}
                    GROUP BY Meter.Name, Meter.ID
                ", date, date.AddDays(1).AddMilliseconds(-1));

                s_memoryCache.Add("Meters" + date.ToString(), table, new CacheItemPolicy { SlidingExpiration = TimeSpan.FromMinutes(10.0D) });

            }
            if (!table.Select().Any())
                return table;
            else if (ascending)
                return table.Select().OrderBy(row => row[sortField]).CopyToDataTable();
            else
                return table.Select().OrderByDescending(row => row[sortField]).CopyToDataTable();
        }

        private DataTable GetDataChannels(AdoDataConnection connection, DateTime date, int meterID, string sortField, bool ascending)
        {
            DataTable table;
            if (s_memoryCache.Contains("Channels" + meterID.ToString() + date.ToString()))
            {
                table = (DataTable)s_memoryCache.Get("Channels" + meterID.ToString() + date.ToString());
            }
            else
            {
                table = connection.RetrieveData(@"
	                SELECT 
		                Channel.ID,
		                Channel.MeterId,
		                Channel.Name,
		                ChannelDataQualitySummary.Date,
		                Channel.SamplesPerHour * 24 as Expected,
		                (Channel.SamplesPerHour * 24) - COALESCE(ChannelDataQualitySummary.GoodPoints,0) - COALESCE(ChannelDataQualitySummary.LatchedPoints,0) - COALESCE(ChannelDataQualitySummary.UnreasonablePoints,0) - COALESCE(ChannelDataQualitySummary.NoncongruentPoints,0) as Missing,
		                COALESCE(ChannelDataQualitySummary.LatchedPoints, 0) as Latched,
		                COALESCE(ChannelDataQualitySummary.UnreasonablePoints, 0) as Unreasonable
	                FROM
		                Channel JOIN
		                ChannelDataQualitySummary ON Channel.ID = ChannelDataQualitySummary.ChannelID
                    WHERE Date BETWEEN {0} AND {1} AND Channel.MeterID = {2}
                ", date, date.AddDays(1).AddMilliseconds(-1), meterID);

                s_memoryCache.Add("Channels" + meterID.ToString() + date.ToString(), table, new CacheItemPolicy { SlidingExpiration = TimeSpan.FromMinutes(10.0D) });
            }

            if (!table.Select().Any())
                return table;
            else if (ascending)
                return table.Select().OrderBy(row => row[sortField]).CopyToDataTable();
            else
                return table.Select().OrderByDescending(row => row[sortField]).CopyToDataTable();

        }

        #endregion

    }
}
