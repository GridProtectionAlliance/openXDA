﻿//******************************************************************************************************
//  DataGroups.cs - Gbtc
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
//  07/21/2014 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using FaultData.DataAnalysis;
using FaultData.DataSets;
using openXDA.Model;
using DataGroupKey = System.Tuple<System.DateTime, System.DateTime, int, bool>;

namespace FaultData.DataResources
{
    public class DataGroupsResource : DataResourceBase<MeterDataSet>
    {
        #region [ Members ]

        // Fields
        private List<DataGroup> m_dataGroups;

        #endregion

        #region [ Properties ]

        public List<DataGroup> DataGroups
        {
            get
            {
                return m_dataGroups;
            }
        }

        #endregion

        #region [ Methods ]

        public override void Initialize(MeterDataSet meterDataSet)
        {
            List<DataGroup> dataGroups = new List<DataGroup>();

            foreach (IGrouping<DataGroupKey, DataSeries> dataGrouping in meterDataSet.DataSeries.Concat(meterDataSet.Digitals).GroupBy(GetKey))
            {
                HashSet<int> completedAsset = new HashSet<int>();

                foreach (IGrouping<Asset, DataSeries> assetGroup in dataGrouping.GroupBy(GetAsset))
                {
                    completedAsset.Add(assetGroup.Key.ID);

                    DataGroup dataGroup = new DataGroup();

                    foreach (DataSeries dataSeries in assetGroup)
                        dataGroup.Add(dataSeries);

                    foreach (DataSeries dataSeries in GetConnectedSeries(dataGrouping, assetGroup.Key))
                        dataGroup.Add(dataSeries);

                    dataGroups.Add(dataGroup);
                }

                //Add Any Datagroups for Assets that have no directly connected Assets
                foreach (Asset asset in meterDataSet.Meter.MeterAssets.Select(item => item.Asset))
                {
                    if (completedAsset.Contains(asset.ID))
                        continue;

                    DataGroup dataGroup = new DataGroup(asset);
                    foreach (DataSeries dataSeries in GetConnectedSeries(dataGrouping, asset))
                        dataGroup.Add(dataSeries);

                    dataGroups.Add(dataGroup);
                }
            }

            if (meterDataSet.Meter.MeterAssets.Count == 1)
            {
                Asset asset = meterDataSet.Meter.MeterAssets[0].Asset;

                foreach (ReportedDisturbance disturbance in meterDataSet.ReportedDisturbances.OrderBy(dist => dist.Time))
                {
                    DataGroup dataGroup = dataGroups.FirstOrDefault(dg => dg.Add(disturbance));

                    if ((object)dataGroup == null)
                    {
                        dataGroup = new DataGroup(asset);
                        dataGroup.Add(disturbance);
                        dataGroups.Add(dataGroup);
                    }
                }
            }

            m_dataGroups = dataGroups;
        }

        private Asset GetAsset(DataSeries dataSeries)
        {
            if ((object)dataSeries.SeriesInfo != null)
                return dataSeries.SeriesInfo.Channel.Asset;

            return null;
        }

        private IEnumerable<DataSeries> GetConnectedSeries(IEnumerable<DataSeries> groupedSeries, Asset asset)
        {
            return groupedSeries.Join(asset.ConnectedChannels,
                dataSeries => dataSeries.SeriesInfo.ChannelID,
                channel => channel.ID,
                (dataSeries, _) => dataSeries);
        }

        private DataGroupKey GetKey(DataSeries dataSeries)
        {
            DateTime startTime = dataSeries.StartTime;
            DateTime endTime = dataSeries.EndTime;
            int sampleCount = dataSeries.DataPoints.Count;
            bool trend = dataSeries.SeriesInfo?.Channel.Trend == true;
            return Tuple.Create(startTime, endTime, sampleCount, trend);
        }

        #endregion
    }
}
