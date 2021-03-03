//******************************************************************************************************
//  PQTrendingWebReportController.cs - Gbtc
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
using GSF.Web;

namespace openXDA.Adapters
{
    public class PQTrendingWebReportController : ApiController
    {
        #region [ Constructors ]

        public PQTrendingWebReportController(Func<AdoDataConnection> connectionFactory) =>
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
            string stat = query["stat"];
            string sortField = query["sortField"];
            bool ascending = bool.Parse(query["ascending"]);
            DateTime date = DateTime.ParseExact(query["date"], "yyyy-MM-dd", CultureInfo.CurrentCulture);

            return Task.Factory.StartNew(() =>
            {
                using (AdoDataConnection connection = ConnectionFactory())
                {
                    return GetData(connection, date, sortField, ascending, stat);
                }
            }, cancellationToken);

        }

        [HttpGet]
        public Task<DataTable> GetChart(CancellationToken cancellationToken)
        {
            Dictionary<string, string> query = Request.QueryParameters();
            string stat = query["stat"];
            string measurementId = query["measurementID"];
            string meterId = query["meterID"];

            DateTime toDate = DateTime.ParseExact(query["date"], "yyyy-MM-dd", CultureInfo.CurrentCulture);
            DateTime startDate = toDate.AddDays(-30);
            string[] stats = { "Max", "CP99", "CP95", "Avg", "CP05", "CP01", "Min" };
            string notSqlInjection = "Avg";
            if (stats.Contains(stat)) notSqlInjection = stat;

            return Task.Factory.StartNew(() =>
            {
                string target = "Chart" + meterId + measurementId + stat + startDate.ToString();
                if (s_memoryCache.Contains(target))
                {
                    return (DataTable)s_memoryCache.Get(target);
                }
                else
                {
                    using (AdoDataConnection connection = ConnectionFactory())
                    {
                        DataTable table = connection.RetrieveData("SELECT DISTINCT Date, " + notSqlInjection + " as Value FROM PQTrendStat WHERE MeterID = (SELECT TOP 1 ID FROM METER WHERE Name = {0}) AND PQMeasurementTypeID = (SELECT TOP 1 ID FROM PQMeasurement WHERE Name = {1}) AND Date BETWEEN {2} AND {3}", meterId, measurementId, startDate, toDate);
                        s_memoryCache.Add(target, table, new CacheItemPolicy { SlidingExpiration = TimeSpan.FromMinutes(10.0D) });
                        return table;
                    }
                }
            }, cancellationToken);
        }

        private DataTable GetData(AdoDataConnection connection, DateTime date, string sortField, bool ascending, string stat)
        {
            string[] stats = { "Max", "CP99", "CP95", "Avg", "CP05", "CP01", "Min" };
            string notSqlInjection = "Avg";
            if (stats.Contains(stat)) notSqlInjection = stat;

            string notSqlInjectionSort = connection.ExecuteScalar<string>(string.Empty, "SELECT Name FROM PQMeasurement WHERE Name = {0}", sortField);
            if (notSqlInjectionSort != sortField) notSqlInjectionSort = "Name";
            notSqlInjectionSort = (ascending ? "[" + notSqlInjectionSort + "]" + " ASC" : "[" + notSqlInjectionSort + "]" + " DESC");

            DataTable table;
            string target = "Data" + stat + date.ToString() + notSqlInjectionSort + ascending.ToString();
            if (s_memoryCache.Contains(target))
            {
                table = (DataTable)s_memoryCache.Get(target);
            }
            else
            {
                table = connection.RetrieveData(@"
                    DECLARE @PivotColumns NVARCHAR(MAX) = N''
                    DECLARE @SQLStatement NVARCHAR(MAX) = N''
                    DECLARE @DateParam Date = {0}

                    SELECT @PivotColumns = @PivotColumns + '[' + t.Name + '],' 
                    FROM (Select Name FROM PQMeasurement WHERE Enabled <> 0) AS t

                    SET @SQLStatement =
                    ' 
                    DECLARE @date Date = @DateParam
                    SELECT Name, ' + SUBSTRING(@PivotColumns,0, LEN(@PivotColumns)) + '
                    FROM (
	                    SELECT
		                    Meter.Name,
		                    t.Name as Measurement,
		                    t." + notSqlInjection + @"
	                    FROM
		                    Meter LEFT JOIN
                            (
                                SELECT 
                                    PQMeasurement.Name,
                                    PQTrendStat.MeterID,
                                    PQTrendStat.Date,
                                    PQTrendStat." + notSqlInjection + @"
                                FROM
		                            PQTrendStat  JOIN
		                            PQMeasurement ON PQTrendStat.PQMeasurementTypeID = PQMeasurement.ID
	                            WHERE 
		                            PQMeasurement.Enabled <>0 AND PQTrendStat.Date = @date

                            ) as t ON Meter.ID = t.MeterID
                           WHERE 
                                Meter.ID IN (SELECT DISTINCT ID FROM MeterDataQualitySummary)
                           ) as ed 
                     PIVOT( 
		                    Avg(ed." + notSqlInjection + @")
		                    FOR ed.Measurement IN(' + SUBSTRING(@PivotColumns,0, LEN(@PivotColumns)) + ') 
                     ) as pvt 
                     ORDER BY " + notSqlInjectionSort + @" '

                    print @sqlstatement

                    exec sp_executesql @SQLStatement, N'@DateParam Date', @DateParam
                ", date);

                s_memoryCache.Add(target, table, new CacheItemPolicy { SlidingExpiration = TimeSpan.FromMinutes(10.0D) });

            }
            return table;
        }

        #endregion

        #region [ Static ]

        private static MemoryCache s_memoryCache = new MemoryCache("PQTrendingWebReport");

        #endregion
    }
}
