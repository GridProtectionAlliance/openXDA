//******************************************************************************************************
//  TrendingDataSummaryOperation.cs - Gbtc
//
//  Copyright © 2015, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the MIT License (MIT), the "License"; you may
//  not use this file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://opensource.org/licenses/MIT
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  05/05/2015 - J. Ritchie Carroll
//       Generated original version of source code.
//
//******************************************************************************************************

using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
    public class TrendingDataSummaryOperation : DataOperationBase<MeterDataSet>
    {
        #region [ Constructors ]

        /// <summary>
        /// Creates a new <see cref="TrendingDataSummaryOperation"/>.
        /// </summary>
        public TrendingDataSummaryOperation() =>
            HIDSSettings = new HIDSSettings();

        #endregion

        #region [ Properties ]

        [Category]
        [SettingName(HIDSSettings.CategoryName)]
        public HIDSSettings HIDSSettings { get; }

        #endregion

        #region [ Methods ]

        public override void Execute(MeterDataSet meterDataSet)
        {
            Log.Info("Executing operation to load trending summary data into HIDS...");

            Dictionary<Channel, List<DataGroup>> trendingGroups = meterDataSet.GetResource<TrendingGroupsResource>().TrendingGroups;

            if (trendingGroups.Count == 0)
            {
                Log.Debug($"No trending data found; skipping {nameof(TrendingDataSummaryOperation)}.");
                return;
            }

            if (string.IsNullOrEmpty(HIDSSettings.Host))
            {
                Log.Debug($"No HIDS instance defined; skipping {nameof(TrendingDataSummaryOperation)}.");
                return;
            }

            using API hids = new API();
            hids.Configure(HIDSSettings);

            TrendingDataSummaryResource trendingDataSummaryResource = meterDataSet.GetResource<TrendingDataSummaryResource>();
            Dictionary<Channel, List<TrendingDataSummaryResource.TrendingDataSummary>> trendingDataSummaries = trendingDataSummaryResource.TrendingDataSummaries;

            foreach (KeyValuePair<Channel, List<TrendingDataSummaryResource.TrendingDataSummary>> channelSummaries in trendingDataSummaries)
            {
                int channelID = channelSummaries.Key.ID;
                IEnumerable<TrendingDataSummaryResource.TrendingDataSummary> summaries = channelSummaries.Value;

                IEnumerable<Point> points = summaries.Select(summary => new Point()
                {
                    Tag = hids.ToTag(channelID),
                    QualityFlags = 0u,
                    Timestamp = summary.Time,
                    Maximum = summary.Maximum,
                    Average = summary.Average,
                    Minimum = summary.Minimum
                });

                hids.WritePoints(points);
            }
        }

        #endregion

        #region [ Static ]

        // Static Fields
        private static readonly ILog Log = LogManager.GetLogger(typeof(TrendingDataSummaryOperation));

        #endregion
    }
}
