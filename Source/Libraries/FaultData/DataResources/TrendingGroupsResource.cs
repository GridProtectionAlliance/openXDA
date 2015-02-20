//******************************************************************************************************
//  TrendingGroupsResource.cs - Gbtc
//
//  Copyright © 2014, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the Eclipse Public License -v 1.0 (the "License"); you may
//  not use this file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://www.opensource.org/licenses/eclipse-1.0.php
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  07/22/2014 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using FaultData.DataAnalysis;
using FaultData.Database;
using FaultData.DataSets;

namespace FaultData.DataResources
{
    public class TrendingGroupsResource : DataResourceBase<MeterDataSet>
    {
        #region [ Members ]

        // Fields
        private Dictionary<Channel, List<DataGroup>> m_trendingGroups;

        #endregion

        #region [ Properties ]

        public Dictionary<Channel, List<DataGroup>> TrendingGroups
        {
            get
            {
                return m_trendingGroups;
            }
        }

        #endregion

        #region [ Methods ]

        public override void Initialize(MeterDataSet meterDataSet)
        {
            List<DataGroup> dataGroups = meterDataSet.GetResource<DataGroupsResource>().DataGroups;

            // All the data groups that classify as trend data must be split into multiple data groups by channel.
            // The collection is then flattened into a collection of all the (channel, dataGroup) pairs.
            // The flattened collection is then grouped by channel to get all groups for each channel.
            // The final result is a lookup table from channel to list of all of that channel's data groups
            m_trendingGroups = dataGroups
                .Where(dataGroup => dataGroup.Classification == DataClassification.Trend)
                .Select(dataGroup => dataGroup.DataSeries.GroupBy(dataSeries => dataSeries.SeriesInfo.Channel))
                .SelectMany(channelSeries => channelSeries.Select(grouping => Tuple.Create(grouping.Key, new DataGroup(grouping))))
                .GroupBy(tuple => tuple.Item1, tuple => tuple.Item2)
                .ToDictionary(grouping => grouping.Key, grouping => grouping.ToList());
        }

        #endregion
    }
}
