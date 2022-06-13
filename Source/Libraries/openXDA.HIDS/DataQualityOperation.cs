//******************************************************************************************************
//  DataQualityOperation.cs - Gbtc
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
//  08/25/2015 - Stephen C. Wills
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
using GSF.Data;
using GSF.Data.Model;
using log4net;
using openXDA.Model;

namespace openXDA.HIDS
{
    public class DataQualityOperation : DataOperationBase<MeterDataSet>
    {
        [Category]
        [SettingName(HIDSSettings.CategoryName)]
        public HIDSSettings HIDSSettings { get; } = new HIDSSettings();

        public override void Execute(MeterDataSet meterDataSet)
        {
            TrendingGroupsResource trendingGroupsResource = meterDataSet.GetResource<TrendingGroupsResource>();
            Dictionary<Channel, List<DataGroup>> trendingGroups = trendingGroupsResource.TrendingGroups;

            if (trendingGroups.Count == 0)
            {
                Log.Debug($"No trending data found; skipping {nameof(DataQualityOperation)}.");
                return;
            }

            if (string.IsNullOrEmpty(HIDSSettings.Host))
            {
                Log.Debug($"No HIDS instance defined; skipping {nameof(DataQualityOperation)}.");
                return;
            }

            PointAggregationResource pointCountResource = meterDataSet.GetResource<PointAggregationResource>();
            List<PointAggregate> pointAggregates = pointCountResource.PointAggregates;

            // Get the total cumulative samples per hour
            // of each of the enabled channels in the meter
            double meterSamplesPerHour = meterDataSet.Meter.Channels
                .Where(channel => channel.Enabled)
                .Where(channel => channel.SamplesPerHour > 0.0D)
                .Where(channel => channel.SamplesPerHour <= 60.0D)
                .Select(channel => channel.SamplesPerHour)
                .DefaultIfEmpty(0.0D)
                .Sum();

            using AdoDataConnection connection = meterDataSet.CreateDbConnection();
            TableOperations<ChannelDataQualitySummary> channelDataQualitySummaryTable = new TableOperations<ChannelDataQualitySummary>(connection);
            TableOperations<MeterDataQualitySummary> meterDataQualitySummaryTable = new TableOperations<MeterDataQualitySummary>(connection);

            foreach (var grouping in pointAggregates.GroupBy(pointCount => pointCount.Date))
            {
                MeterDataQualitySummary meterDataQualitySummary = new MeterDataQualitySummary()
                {
                    MeterID = meterDataSet.Meter.ID,
                    Date = grouping.Key,
                    ExpectedPoints = (int)(24.0D * meterSamplesPerHour)
                };

                foreach (PointAggregate pointCount in grouping)
                {
                    ChannelDataQualitySummary channelDataQualitySummary = new ChannelDataQualitySummary()
                    {
                        ChannelID = pointCount.Channel.ID,
                        Date = grouping.Key,
                        ExpectedPoints = (int)(24.0D * pointCount.Channel.SamplesPerHour)
                    };

                    meterDataQualitySummary.GoodPoints += pointCount.GoodPoints;
                    meterDataQualitySummary.UnreasonablePoints += pointCount.UnreasonablePoints;
                    meterDataQualitySummary.LatchedPoints += pointCount.LatchedPoints;
                    meterDataQualitySummary.NoncongruentPoints += pointCount.NoncongruentPoints;

                    channelDataQualitySummary.GoodPoints += pointCount.GoodPoints;
                    channelDataQualitySummary.UnreasonablePoints += pointCount.UnreasonablePoints;
                    channelDataQualitySummary.LatchedPoints += pointCount.LatchedPoints;
                    channelDataQualitySummary.NoncongruentPoints += pointCount.NoncongruentPoints;
                    channelDataQualitySummaryTable.Upsert(channelDataQualitySummary);
                }

                meterDataQualitySummaryTable.Upsert(meterDataQualitySummary);
            }
        }

        // Static Fields
        private static readonly ILog Log = LogManager.GetLogger(typeof(DataQualityOperation));
    }
}
