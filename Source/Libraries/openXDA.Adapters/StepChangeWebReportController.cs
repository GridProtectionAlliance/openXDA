//******************************************************************************************************
//  StepChangeWebReportController.cs - Gbtc
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
//  08/13/2018 - Billy Ernest
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
using GSF.Data.Model;
using GSF.Web;
using GSF.Web.Model;
using openXDA.Model;

namespace openXDA.Adapters
{
    public class StepChangeWebReportController : ApiController
    {
        #region [ Constructors ]

        public StepChangeWebReportController(Func<AdoDataConnection> connectionFactory) =>
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
            string sortField = query["sortField"];
            bool ascending = bool.Parse(query["ascending"]);
            DateTime startDate = DateTime.ParseExact(query["startDate"], "yyyy-MM-dd", CultureInfo.CurrentCulture).Date;
            DateTime endDate = DateTime.ParseExact(query["endDate"], "yyyy-MM-dd", CultureInfo.CurrentCulture).Date.AddDays(1).AddMinutes(-1);

            return Task.Factory.StartNew(() =>
            {
                using (AdoDataConnection connection = ConnectionFactory())
                {
                    return GetData(connection, startDate, endDate, sortField, ascending);
                }
            }, cancellationToken);

        }

        [HttpGet]
        public Task<Channel> GetChannel(CancellationToken cancellationToken)
        {
            Dictionary<string, string> query = Request.QueryParameters();
            int meterID = int.Parse(query["meterID"]);
            string measurementID = query["measurementID"];

            return Task.Factory.StartNew(() =>
            {
                using (AdoDataConnection connection = ConnectionFactory())
                {
                    TableOperations<PQMeasurement> pqMeasurementTable = new TableOperations<PQMeasurement>(connection);
                    PQMeasurement pQMeasurement = pqMeasurementTable.QueryRecordWhere("Name = {0}", measurementID);

                    TableOperations<Channel> channelTable = new TableOperations<Channel>(connection);
                    return channelTable.QueryRecordWhere(@"
                        MeterID = {0} AND
                        Channel.MeasurementTypeID = {1} AND
                        Channel.MeasurementCharacteristicID = {2} AND
                        Channel.PhaseID = {3} AND
                        Channel.HarmonicGroup = {4}
                    ", meterID, pQMeasurement.MeasurementTypeID, pQMeasurement.MeasurementCharacteristicID, pQMeasurement.PhaseID, pQMeasurement.HarmonicGroup);
                }
            }, cancellationToken);

        }


        private DataTable GetData(AdoDataConnection connection, DateTime startDate, DateTime endDate, string sortField, bool ascending)
        {
            DataTable table;
            string target = "Data" + startDate.ToString() + endDate.ToString();
            if (s_memoryCache.Contains(target))
            {
                table = (DataTable)s_memoryCache.Get(target);
            }
            else
            {
                table = connection.RetrieveData(@"
                    DECLARE @PivotColumns NVARCHAR(MAX) = N''
                    DECLARE @ReturnColumns NVARCHAR(MAX) = N''
                    DECLARE @SQLStatement NVARCHAR(MAX) = N''
                    DECLARE @StartDateParam Date = {0}
                    DECLARE @EndDateParam Date = {1}

                    SELECT @PivotColumns = @PivotColumns + '[' + t.Name + '],' 
                    FROM (SELECT 
	                            Name
                            FROM 
	                            StepChangeMeasurement JOIN
	                            PQMeasurement ON StepChangeMeasurement.PQMeasurementID = PQMeasurement.ID) AS t

                    SELECT @ReturnColumns = @ReturnColumns + ' COALESCE([' + t.Name + '], 0) AS [' + t.Name + '],' 
                    FROM (SELECT 
	                            Name
                            FROM 
	                            StepChangeMeasurement JOIN
	                            PQMeasurement ON StepChangeMeasurement.PQMeasurementID = PQMeasurement.ID) AS t


                    SET @SQLStatement =
                    ' 
                    DECLARE @startDate Date = @StartDateParam
                    DECLARE @endDate Date = @EndDateParam

                    SELECT MeterID, Name, ' + SUBSTRING(@ReturnColumns,0, LEN(@ReturnColumns)) + '
                    FROM (
	                    SELECT
                            Meter.ID as MeterID,
		                    Meter.Name,
		                    PQMeasurement.Name as Measurement,
		                    StepChangeStat.Value
	                    FROM
		                    Meter JOIN
		                    StepChangeStat ON Meter.ID = StepChangeStat.MeterID JOIN
		                    StepChangeMeasurement ON StepChangeStat.StepChangeMeasurementID = StepChangeMeasurement.ID JOIN
		                    PQMeasurement ON StepChangeMeasurement.PQMeasurementID = PQMeasurement.ID
	                    WHERE 
		                    Date BETWEEN @startDate AND @endDate
                            ) as ed 
                        PIVOT( 
		                    Avg(ed.Value)
		                    FOR ed.Measurement IN(' + SUBSTRING(@PivotColumns,0, LEN(@PivotColumns)) + ') 
                        ) as pvt 
                        ORDER BY Name '

                    print @sqlstatement

                    exec sp_executesql @SQLStatement, N'@StartDateParam Date, @EndDateParam Date', @StartDateParam = @StartDateParam, @EndDateParam = @EndDateParam                
                ", startDate, endDate);

                s_memoryCache.Add(target, table, new CacheItemPolicy { SlidingExpiration = TimeSpan.FromMinutes(10.0D) });

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

        private static MemoryCache s_memoryCache = new MemoryCache("StepChangeWebReport");

        #endregion
    }
}
