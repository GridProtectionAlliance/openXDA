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
using FaultData.DataResources.eDNA;
using FaultData.DataResources.OSIPI;
using FaultData.DataSets;
using GSF.Configuration;
using GSF.Data;
using openXDA.Configuration;
using openXDA.Model;

namespace FaultData.DataResources
{
    public class SCADADataResource : DataResourceBase<MeterDataSet>
    {
        #region [ Members ]

        // Nested Types
        public interface ISCADAHistorianResource
        {
            bool DidBreakerOpen(List<string> points, DateTime startTime, DateTime endTime, double breakerOpenValue);
        }

        public enum SCADASystem
        {
            None,
            EDNA,
            PI
        }

        #endregion

        #region [ Properties ]

        [Category]
        [SettingName(SystemSection.CategoryName)]
        public SystemSection SystemSettings { get; }
            = new SystemSection();

        [Category]
        [SettingName(SCADASection.CategoryName)]
        public SCADASection SCADASettings { get; }
            = new SCADASection();

        private Func<AdoDataConnection> ConnectionFactory { get; set; }
        private ISCADAHistorianResource SCADAHistorianResource { get; set; }

        #endregion

        #region [ Methods ]

        public bool DidBreakerOpen(Asset line, DateTime approximateTime)
        {
            // If no SCADA historian is configured,
            // trust the analysis results
            if (SCADAHistorianResource is null)
                return true;

            string pointQuery = SCADASettings.PointQuery;
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

            TimeSpan queryTolerance = SCADASettings.QueryToleranceSpan;
            DateTime startTime = localClearingTime - queryTolerance;
            DateTime endTime = localClearingTime + queryTolerance;

            double breakerOpenValue = SCADASettings.BreakerOpenValue;

            return SCADAHistorianResource.DidBreakerOpen(points, startTime, endTime, breakerOpenValue);
        }

        public override void Initialize(MeterDataSet meterDataSet)
        {
            ConnectionFactory = meterDataSet.CreateDbConnection;
            SCADAHistorianResource = GetSCADAHistorianResource(meterDataSet);
        }

        private ISCADAHistorianResource GetSCADAHistorianResource(MeterDataSet meterDataSet)
        {
            switch (SCADASettings.Historian)
            {
                case SCADASection.SCADAHistorian.eDNA:
                    return meterDataSet.GetResource<EDNADataResource>();

                case SCADASection.SCADAHistorian.OSIPI:
                    return meterDataSet.GetResource<OSIPIDataResource>();

                default:
                    return null;
            }
        }

        #endregion
    }
}
