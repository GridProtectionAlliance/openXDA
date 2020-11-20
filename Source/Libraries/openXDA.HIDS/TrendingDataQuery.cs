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

        public IAsyncEnumerable<Point> Query(List<int> channelIDs, DateTime startTime, DateTime endTime )
        {
            using (API hids = new API())
            {
                hids.Configure(HIDSSettings);

                return hids.ReadPointsAsync((t) => {
                    t.FilterTags(channelIDs.Select(cid => cid.ToString()));
                    t.Range(startTime, endTime);
                 });
            }
        }

        #endregion

        #region [ Static ]

        // Static Fields
        private static readonly ILog Log = LogManager.GetLogger(typeof(TrendingDataQuery));

        #endregion
    }
}
