//******************************************************************************************************
//  SCADADataResource.cs - Gbtc
//
//  Copyright © 2019, Grid Protection Alliance.  All Rights Reserved.
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
//  07/18/2019 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using FaultData.DataSets;
using GSF.Configuration;
using GSF.Data;
using InStep.eDNA.EzDNAApiNet;
using log4net;
using openXDA.Configuration;
using openXDA.Model;

namespace FaultData.DataResources
{
    public class SCADADataResource : DataResourceBase<MeterDataSet>
    {
        #region [ Properties ]

        [Category]
        [SettingName(SystemSection.CategoryName)]
        public SystemSection SystemSettings { get; }
            = new SystemSection();

        [Category]
        [SettingName(EDNASection.CategoryName)]
        public EDNASection EDNASettings { get; }
            = new EDNASection();

        private Func<AdoDataConnection> ConnectionFactory { get; set; }

        #endregion

        #region [ Methods ]

        public bool DidBreakerOpen(Asset line, DateTime approximateTime)
        {
            string pointQuery = EDNASettings.PointQuery;
            List<string> points;

            using (AdoDataConnection connection = ConnectionFactory())
            using (DataTable pointTable = connection.RetrieveData(pointQuery, line.ID))
            {
                points = pointTable
                    .Select()
                    .Select(row => row.ConvertField<string>("Point"))
                    .ToList();
            }

            // If there are no SCADA points configured,
            // trust the analysis results
            if (!points.Any())
                return true;

            DateTime utcClearingTime = TimeZoneInfo.ConvertTimeToUtc(approximateTime, SystemSettings.XDATimeZoneInfo);
            DateTime localClearingTime = utcClearingTime.ToLocalTime();

            TimeSpan queryTolerance = EDNASettings.QueryToleranceSpan;
            DateTime startTime = localClearingTime - queryTolerance;
            DateTime endTime = localClearingTime + queryTolerance;

            double breakerOpenValue = EDNASettings.BreakerOpenValue;

            foreach (string point in points)
            {
                var previousPoint = new
                {
                    Value = default(double),
                    Time = default(DateTime),
                    Status = default(string),
                    Valid = false
                };

                int[] expectedResults =
                {
                    (int)eDNAHistoryReturnStatus.END_OF_HISTORY,
                    (int)eDNAHistoryReturnStatus.NO_HISTORY_FOR_TIME
                };

                int result = History.DnaGetHistRaw(point, startTime, endTime, out uint key);

                while (result == 0)
                {
                    result = History.DnaGetNextHist(key, out double value, out DateTime time, out string status);

                    if (result == 0)
                    {
                        // Verify that the data point represents a change
                        // from closed to open within the queried time range
                        bool trip =
                            previousPoint.Valid &&
                            previousPoint.Value != breakerOpenValue &&
                            value == breakerOpenValue &&
                            time >= startTime &&
                            time <= endTime;

                        if (trip)
                            return true;

                        previousPoint = new
                        {
                            Value = value,
                            Time = time,
                            Status = status,
                            Valid = true
                        };
                    }
                }

                // Assume that unexpected return status indicates an error
                // and therefore the analysis results should be trusted
                if (!expectedResults.Contains(result))
                {
                    Log.Debug($"Unexpected eDNA return code: {result}");
                    return true;
                }
            }

            return false;
        }

        public override void Initialize(MeterDataSet meterDataSet) =>
            ConnectionFactory = meterDataSet.CreateDbConnection;

        #endregion

        #region [ Static ]

        // Static Fields
        private static readonly ILog Log = LogManager.GetLogger(typeof(SCADADataResource));

        #endregion
    }
}
