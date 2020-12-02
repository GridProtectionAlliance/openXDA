//******************************************************************************************************
//  TrendingDataQuery.cs - Gbtc
//
//  Copyright © 2020, Grid Protection Alliance.  All Rights Reserved.
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
//  11/16/2020 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FaultData.DataAnalysis;
using FaultData.DataOperations;
using FaultData.DataResources;
using FaultData.DataSets;
using GSF.Configuration;
using HIDS;
using log4net;
using openXDA.HIDS.APIExtensions;
using openXDA.Model;

namespace openXDA.HIDS
{
    public class TrendingDataQuery 
    {
        #region [ Members ]
        const ulong ValidHours = 16777215; // Math.Pow(2, 24) - 1
        const ulong ValidDays = 127; //  (int)(Math.Pow(2, 7) - 1);
        const ulong ValidWeeks = 9007199254740991; // (int)(Math.Pow(2, 53) - 1);
        const ulong ValidMonths = 4095;//  (int)(Math.Pow(2, 12) - 1);

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Creates a new <see cref="TrendingDataSummaryOperation"/>.
        /// </summary>
        public TrendingDataQuery()
        {
            HIDSSettings = new HIDSSettings();
            HIDSSettings.Load();
        }

        #endregion

        #region [ Properties ]

        [Category]
        [SettingName(HIDSSettings.CategoryName)]
        public HIDSSettings HIDSSettings { get; }

        #endregion

        #region [ Methods ]

        public async Task<List<Point>> Query(List<int> channelIDs, DateTime startTime, DateTime endTime, CancellationToken cancellationToken, ulong validHours = ValidHours, ulong validDays = ValidDays, ulong validWeeks = ValidWeeks, ulong validMonths = ValidMonths)
        {
            IEnumerable<TimeFilter> hours = Enumerable.Range(0, 24).Where(index => (~validHours & (1Lu << index)) > 0).Select(h => TimeFilter.Hour00 + h);
            IEnumerable<TimeFilter> days = Enumerable.Range(0, 7).Where(index => (~validDays & (1Lu << index)) > 0).Select(h => TimeFilter.Sunday + h);
            IEnumerable<TimeFilter> weeks = Enumerable.Range(0, 53).Where(index => (~validWeeks & (1Lu << index)) > 0).Select(h => TimeFilter.Week00 + h);
            IEnumerable<TimeFilter> months = Enumerable.Range(0, 12).Where(index => (~validMonths & (1Lu << index)) > 0).Select(h => TimeFilter.January + h);

            using (API hids = new API())
            {
                hids.Configure(HIDSSettings);
                return await hids.ReadPointsAsync((t) => {
                    t.FilterTags(channelIDs.Select(cid => cid.ToString("x8")));
                    t.Range(startTime, endTime);
                    t.FilterTime(hours.Concat(days).Concat(weeks).Concat(months));

                 }, cancellationToken).ToListAsync();
            }
        }

        #endregion

        #region [ Static ]

        // Static Fields
        private static readonly ILog Log = LogManager.GetLogger(typeof(TrendingDataQuery));

        #endregion
    }
}
