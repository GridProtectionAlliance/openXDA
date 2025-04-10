//******************************************************************************************************
//  EDNADataResource.cs - Gbtc
//
//  Copyright © 2025, Grid Protection Alliance.  All Rights Reserved.
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
//  03/21/2025 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using FaultData.DataSets;
using InStep.eDNA.EzDNAApiNet;
using log4net;
using static FaultData.DataResources.SCADADataResource;

namespace FaultData.DataResources.eDNA
{
    public class EDNADataResource : DataResourceBase<MeterDataSet>, ISCADAHistorianResource
    {
        #region [ Properties ]

        private TimeZoneConverter XDATimeZoneConverter { get; set; }

        #endregion

        #region [ Methods ]

        public bool DidBreakerOpen(List<string> points, DateTime startTime, DateTime endTime, double breakerOpenValue)
        {
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

                DateTime localStartTime = XDATimeZoneConverter.ToUTCTimeZone(startTime).ToLocalTime();
                DateTime localEndTime = XDATimeZoneConverter.ToUTCTimeZone(endTime).ToLocalTime();
                int result = History.DnaGetHistRaw(point, localStartTime, localEndTime, out uint key);

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
                            time >= localStartTime &&
                            time <= localEndTime;

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

        public IEnumerable<string> QuerySCADADataPoints(string pointTag, int take)
        {
            return new string[] { };
            // Since we are not able to test at the moment, we leave this out,
            // but this untested code may be useful to they who implement this.
            // This requires additional conversation about how to do UI, since 
            // The tag needed is SITE.SERVICE.IDENTIFIER, as per docs, but searching
            // Would require us to split that...
            /*
            string[] parameters = pointTag.Split('.');
            int result = InStep.eDNA.EzDNAApiNet.Configuration.EzFindPoints(
                $"*{parameters[0]}*",
                $"*{parameters[1]}*",
                "*",
                $"*{parameters[2]}*",
                "*", "*", "*", "*", "*",
                out int key
            );
            int tableSize = InStep.eDNA.EzDNAApiNet.Configuration.EzFindPointsSize(key);
            int recordsReturned = take <= tableSize ? take : tableSize;
            string[] tagArray = new string[recordsReturned];
            tagArray.Select((_, index) =>
            {
                InStep.eDNA.EzDNAApiNet.Configuration.EzFindPointsRec(key, index, out string site, out string service, _, out string id);
                return $"{site}.{service}.{id}";
            });
            return tagArray;
            */
        }

        public override void Initialize(MeterDataSet meterDataSet) =>
            XDATimeZoneConverter = new TimeZoneConverter(meterDataSet.Configure);

        #endregion

        #region [ Static ]

        // Static Fields
        private static readonly ILog Log = LogManager.GetLogger(typeof(EDNADataResource));

        #endregion
    }
}
