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
    public class PQTrendingWebReportController : ApiController
    {
        #region [ Static ]
        private static MemoryCache s_memoryCache;

        static PQTrendingWebReportController()
        {
            s_memoryCache = new MemoryCache("PQTrendingWebReport");
        }
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
                using (AdoDataConnection connection = new AdoDataConnection("systemSettings")) {
                    return GetData(connection, date, sortField, ascending, stat);
                }
            }, cancellationToken);

        }

        [HttpGet]
        public Task<DataTable> GetChart(CancellationToken cancellationToken) {
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
                else {
                    using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
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

            DataTable table;
            if (s_memoryCache.Contains("Data" +stat + date.ToString()))
            {
                table = (DataTable)s_memoryCache.Get("Data" + stat + date.ToString());
            }
            else {
                table = connection.RetrieveData(@"
                    DECLARE @PivotColumns NVARCHAR(MAX) = N''
                    DECLARE @SQLStatement NVARCHAR(MAX) = N''
                    DECLARE @DateParam Date = {0}

                    SELECT @PivotColumns = @PivotColumns + '[' + t.Name + '],' 
                    FROM (Select Name FROM PQMeasurement) AS t

                    SET @SQLStatement =
                    ' 
                    DECLARE @date Date = @DateParam
                    SELECT Name, ' + SUBSTRING(@PivotColumns,0, LEN(@PivotColumns)) + '
                    FROM (
	                    SELECT
		                    Meter.Name,
		                    PQMeasurement.Name as Measurement,
		                    PQTrendStat." + notSqlInjection + @"
	                    FROM
		                    Meter JOIN
		                    PQTrendStat ON Meter.ID = PQTrendStat.MeterID JOIN
		                    PQMeasurement ON PQTrendStat.PQMeasurementTypeID = PQMeasurement.ID
	                    WHERE 
		                    Date = @date
                           ) as ed 
                     PIVOT( 
		                    Avg(ed." + notSqlInjection + @")
		                    FOR ed.Measurement IN(' + SUBSTRING(@PivotColumns,0, LEN(@PivotColumns)) + ') 
                     ) as pvt 
                     ORDER BY Name '

                    print @sqlstatement

                    exec sp_executesql @SQLStatement, N'@DateParam Date', @DateParam
                ", date);

                s_memoryCache.Add("Data" + stat + date.ToString(), table, new CacheItemPolicy { SlidingExpiration = TimeSpan.FromMinutes(10.0D) });

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
