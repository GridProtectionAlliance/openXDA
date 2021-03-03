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

using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Runtime.Caching;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using GSF.Data;
using GSF.Web;

namespace openXDA.Adapters
{
    public class DataQualitySummaryController : ApiController
    {
        #region [ Constructors ]

        public DataQualitySummaryController(Func<AdoDataConnection> connectionFactory) =>
            ConnectionFactory = connectionFactory;

        #endregion

        #region [ Properties ]

        private Func<AdoDataConnection> ConnectionFactory { get; }

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
                using (AdoDataConnection connection = ConnectionFactory()) {
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
            if (MemoryCache.Contains("Meters" + date.ToString()))
            {
                table = (DataTable)MemoryCache.Get("Meters" + date.ToString());
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
		                    CAST(Channel.SamplesPerHour * 24 as int) as Expected,
		                    CAST((Channel.SamplesPerHour * 24) - COALESCE(ChannelDataQualitySummary.GoodPoints,0) - COALESCE(ChannelDataQualitySummary.LatchedPoints,0) - COALESCE(ChannelDataQualitySummary.UnreasonablePoints,0) - COALESCE(ChannelDataQualitySummary.NoncongruentPoints,0) as int) as Missing,
		                    CAST(COALESCE(ChannelDataQualitySummary.LatchedPoints, 0) as int) as Latched,
		                    CAST(COALESCE(ChannelDataQualitySummary.UnreasonablePoints, 0) as int) as Unreasonable
	                    FROM
		                    Channel JOIN
		                    ChannelDataQualitySummary ON Channel.ID = ChannelDataQualitySummary.ChannelID
	                    WHERE
		                    Channel.MeasurementTypeID IN (SELECT ID FROM MeasurementType WHERE Name = 'Voltage' OR Name = 'Current') AND
		                    Channel.MeasurementCharacteristicID IN (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'RMS')
	                    ) as tbl on Meter.ID = tbl.MeterID
                    WHERE Date = {0}
                    GROUP BY Meter.Name, Meter.ID
                ", date);

                MemoryCache.Add("Meters" + date.ToString(), table, new CacheItemPolicy { SlidingExpiration = TimeSpan.FromMinutes(10.0D) });

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
            if (MemoryCache.Contains("Channels" + meterID.ToString() + date.ToString()))
            {
                table = (DataTable)MemoryCache.Get("Channels" + meterID.ToString() + date.ToString());
            }
            else
            {
                table = connection.RetrieveData(@"
	                SELECT 
		                Channel.ID,
		                Channel.MeterId,
	                    MeasurementType.Name + ' ' + MeasurementCharacteristic.Name  + ' ' + Phase.Name as Name,
		                ChannelDataQualitySummary.Date,
		                CAST(Channel.SamplesPerHour * 24 as int) as Expected,
		                CAST((Channel.SamplesPerHour * 24) - COALESCE(ChannelDataQualitySummary.GoodPoints,0) - COALESCE(ChannelDataQualitySummary.LatchedPoints,0) - COALESCE(ChannelDataQualitySummary.UnreasonablePoints,0) - COALESCE(ChannelDataQualitySummary.NoncongruentPoints,0) as int) as Missing,
		                CAST(COALESCE(ChannelDataQualitySummary.LatchedPoints, 0) as int) as Latched,
		                CAST(COALESCE(ChannelDataQualitySummary.UnreasonablePoints, 0) as int) as Unreasonable
	                FROM
		                Channel JOIN
		                ChannelDataQualitySummary ON Channel.ID = ChannelDataQualitySummary.ChannelID  JOIN
	                    MeasurementType ON Channel.MeasurementTypeID = MeasurementType.ID JOIN
	                    MeasurementCharacteristic ON Channel.MeasurementCharacteristicID = MeasurementCharacteristic.ID JOIN
	                    Phase ON Channel.PhaseID = Phase.ID
	                WHERE
		                Channel.MeasurementTypeID IN (SELECT ID FROM MeasurementType WHERE Name = 'Voltage' OR Name = 'Current') AND
		                Channel.MeasurementCharacteristicID IN (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'RMS') AND
                        Date = {0} AND Channel.MeterID = {1}
                ", date , meterID);

                MemoryCache.Add("Channels" + meterID.ToString() + date.ToString(), table, new CacheItemPolicy { SlidingExpiration = TimeSpan.FromMinutes(10.0D) });
            }

            if (!table.Select().Any())
                return table;
            else if (ascending)
                return table.Select().OrderBy(row => row[sortField]).CopyToDataTable();
            else
                return table.Select().OrderByDescending(row => row[sortField]).CopyToDataTable();

        }

        #endregion

        #region [ Static ]

        // Static Properties
        private static MemoryCache MemoryCache { get; }
            = new MemoryCache("DataQualitySummary");

        #endregion
    }
}
