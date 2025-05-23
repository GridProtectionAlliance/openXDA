﻿//******************************************************************************************************
//  OSIPIDataResource.cs - Gbtc
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
using System.ComponentModel;
using System.Linq;
using FaultData.DataSets;
using GSF.Configuration;
using log4net;
using openXDA.Configuration;
using OSIsoft.AF.Asset;
using OSIsoft.AF.Data;
using OSIsoft.AF.PI;
using OSIsoft.AF.Time;
using static FaultData.DataResources.SCADADataResource;

namespace FaultData.DataResources.OSIPI
{
    public class OSIPIDataResource : DataResourceBase<MeterDataSet>, ISCADAHistorianResource
    {
        #region [ Properties ]

        [Category]
        [SettingName(OSIPISection.CategoryName)]
        public OSIPISection Settings { get; }
            = new OSIPISection();

        private TimeZoneConverter XDATimeZoneConverter { get; set; }

        #endregion

        #region [ Methods ]

        public bool DidBreakerOpen(List<string> points, DateTime startTime, DateTime endTime, double breakerOpenValue)
        {
            try
            {
                using (PIConnection connection = CreateConnection())
                {
                    connection.Open();
                    return DidBreakerOpen(connection, points, startTime, endTime, breakerOpenValue);
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Error while connecting to OSI-PI historian: {ex.Message}", ex);
                return true;
            }
        }

        public IEnumerable<string> QuerySCADADataPoints(string pointTag, int take)
        {
            try
            {
                using (PIConnection connection = CreateConnection())
                {
                    connection.Open();
                    string searchText = $"*{pointTag}*";
                    return PIPoint.FindPIPoints(connection.Server, searchText, null, new List<string> { PICommonPointAttributes.Tag })
                        .Select(point => {
                            point.LoadAttributes();
                            return (string) point.GetAttribute(PICommonPointAttributes.Tag);
                        }).Take(take);
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Error while connecting to OSI-PI historian: {ex.Message}", ex);
                return new string[] { };
            }
        }

        public override void Initialize(MeterDataSet meterDataSet) =>
            XDATimeZoneConverter = new TimeZoneConverter(meterDataSet.Configure);

        private bool DidBreakerOpen(PIConnection connection, List<string> points, DateTime startTime, DateTime endTime, double breakerOpenValue)
        {
            PIPointList pointList = new PIPointList();
            DateTime utcStartTime = XDATimeZoneConverter.ToUTCTimeZone(startTime);
            DateTime utcEndTime = XDATimeZoneConverter.ToUTCTimeZone(endTime);
            AFTimeRange timeRange = new AFTimeRange(utcStartTime, utcEndTime);
            PIPagingConfiguration pagingConfiguration = new PIPagingConfiguration(PIPageType.EventCount, 100);

            if (!PopulatePointList(connection, points, pointList))
            {
                Log.Error("Ignoring SCADA breaker status due to misconfiguration.");
                return true;
            }

            try
            {
                IEnumerable<AFValue> afValues = pointList
                    .RecordedValues(timeRange, AFBoundaryType.Outside, null, false, pagingConfiguration)
                    .SelectMany(scanner => scanner);

                AFValue previousValue = null;

                foreach (AFValue afValue in afValues)
                {
                    if (!afValue.IsGood)
                        continue;

                    // Verify that the data point represents a change
                    // from closed to open within the queried time range
                    bool trip =
                        !(previousValue is null) &&
                        previousValue.ValueAsDouble() != breakerOpenValue &&
                        afValue.ValueAsDouble() == breakerOpenValue &&
                        afValue.Timestamp.UtcTime >= utcStartTime &&
                        afValue.Timestamp.UtcTime <= utcEndTime;

                    if (trip)
                        return true;

                    previousValue = afValue;
                }

                return false;
            }
            catch (OperationCanceledException)
            {
                Exception ex = pagingConfiguration.Error;
                Log.Error($"Error while reading breaker state from OSI-PI: {ex.Message}", ex);
                return true;
            }
            catch (Exception ex)
            {
                Log.Error($"Error while reading breaker state from OSI-PI: {ex.Message}", ex);
                return true;
            }
        }

        private bool PopulatePointList(PIConnection connection, List<string> points, PIPointList pointList)
        {
            bool encounteredError = false;

            foreach (string piTag in points)
            {
                if (!PIPoint.TryFindPIPoint(connection.Server, piTag, out PIPoint point))
                {
                    encounteredError = true;
                    Log.Error($"Unable to find PI point '{piTag}'.");

                    // Even if an error occurs,
                    // continue processing to log additional errors
                    continue;
                }

                pointList.Add(point);
            }

            return !encounteredError;
        }

        private PIConnection CreateConnection() => new PIConnection
        {
            ServerName = Settings.ServerName,
            UserName = Settings.UserName,
            Password = Settings.Password,
            ConnectTimeout = Settings.ConnectTimeout
        };

        #endregion

        #region [ Static ]

        // Static Fields
        private static readonly ILog Log = LogManager.GetLogger(typeof(OSIPIDataResource));

        #endregion
    }
}
