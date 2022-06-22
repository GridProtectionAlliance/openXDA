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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using FaultData.DataAnalysis;
using FaultData.DataOperations;
using FaultData.DataResources;
using FaultData.DataSets;
using GSF.Collections;
using GSF.Configuration;
using GSF.Data;
using GSF.Data.Model;
using HIDS;
using log4net;
using openXDA.HIDS.APIExtensions;
using openXDA.Model;
using TimeRange = GSF.Range<System.DateTime>;

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

            using AdoDataConnection connection = meterDataSet.CreateDbConnection();

            TrendingDataSummaryResource trendingDataSummaryResource = meterDataSet.GetResource<TrendingDataSummaryResource>();
            Dictionary<Channel, List<TrendingDataSummaryResource.TrendingDataSummary>> trendingDataSummaries = trendingDataSummaryResource.TrendingDataSummaries;

            foreach (KeyValuePair<Channel, List<TrendingDataSummaryResource.TrendingDataSummary>> channelSummaries in trendingDataSummaries)
            {
                Channel channel = channelSummaries.Key;
                List<DataQualityRangeLimit> dataQualityRangeLimits = InitializeRangeLimitTable(connection, channel);

                int channelID = channel.ID;
                double perUnitValue = channel.PerUnitValue ?? 1.0D;
                IEnumerable<TrendingDataSummaryResource.TrendingDataSummary> summaries = channelSummaries.Value;

                IEnumerable<Point> points = summaries.Select(summary => new Point()
                {
                    Tag = hids.ToTag(channelID),
                    QualityFlags = (uint)TestQuality(summary, dataQualityRangeLimits, perUnitValue),
                    Timestamp = summary.Time,
                    Maximum = summary.Maximum,
                    Average = summary.Average,
                    Minimum = summary.Minimum
                });

                Task writeTask = hids.WritePointsAsync(points);
                writeTask.GetAwaiter().GetResult();
            }

            Task updateTask = UpdateLatchedPoints(hids, meterDataSet);
            updateTask.GetAwaiter().GetResult();
        }

        private List<DataQualityRangeLimit> InitializeRangeLimitTable(AdoDataConnection connection, Channel channel)
        {
            // Query the range limits for the given channel
            TableOperations<DataQualityRangeLimit> dataQualityRangeLimitTable = new TableOperations<DataQualityRangeLimit>(connection);

            List<DataQualityRangeLimit> dataQualityRangeLimits = dataQualityRangeLimitTable
                .QueryRecordsWhere("ChannelID = {0}", channel.ID)
                .ToList();

            // If limits exist for the given channel,
            // range limit table has been successfully initialized
            if (dataQualityRangeLimits.Count != 0)
                return dataQualityRangeLimits;

            // Get the default range limits for the measurement type and characteristic of this channel
            TableOperations<DefaultDataQualityRangeLimit> defaultDataQualityRangeLimitTable = new TableOperations<DefaultDataQualityRangeLimit>(connection);
            int measurementTypeID = channel.MeasurementTypeID;
            int measurementCharacteristicID = channel.MeasurementCharacteristicID;

            List<DefaultDataQualityRangeLimit> defaultDataQualityRangeLimits = defaultDataQualityRangeLimitTable
                .QueryRecordsWhere("MeasurementTypeID = {0} AND MeasurementCharacteristicID = {1}", measurementTypeID, measurementCharacteristicID)
                .ToList();

            // If there are no default limits for the channel,
            // then the range limit table has been successfully initialized
            if (defaultDataQualityRangeLimits.Count == 0)
                return dataQualityRangeLimits;

            // Update the table to include this channel's default limits
            foreach (DefaultDataQualityRangeLimit defaultDataQualityRangeLimit in defaultDataQualityRangeLimits)
            {
                DataQualityRangeLimit dataQualityRangeLimit = new DataQualityRangeLimit()
                {
                    ChannelID = channel.ID,
                    High = defaultDataQualityRangeLimit.High,
                    Low = defaultDataQualityRangeLimit.Low,
                    RangeInclusive = defaultDataQualityRangeLimit.RangeInclusive,
                    PerUnit = defaultDataQualityRangeLimit.PerUnit,
                    Enabled = true
                };

                dataQualityRangeLimitTable.AddNewRecord(dataQualityRangeLimit);
            }

            return dataQualityRangeLimits;
        }

        private QualityFlags TestQuality(TrendingDataSummaryResource.TrendingDataSummary summary, List<DataQualityRangeLimit> dataQualityRangeLimits, double perUnitValue)
        {
            QualityFlags quality = QualityFlags.Empty;

            foreach (DataQualityRangeLimit dataQualityRangeLimit in dataQualityRangeLimits)
            {
                double factor = dataQualityRangeLimit.PerUnit ? perUnitValue : 1.0D;

                if (!(dataQualityRangeLimit.High is null))
                {
                    double highLimit = factor * dataQualityRangeLimit.High.GetValueOrDefault();
                    bool maxValid = Convert.ToBoolean(dataQualityRangeLimit.RangeInclusive) ^ (summary.Maximum < highLimit);

                    if (!maxValid)
                    {
                        quality |= QualityFlags.Unreasonable;
                        break;
                    }
                }

                if (!(dataQualityRangeLimit.Low is null))
                {
                    double lowLimit = factor * dataQualityRangeLimit.Low.GetValueOrDefault();
                    bool minValid = Convert.ToBoolean(dataQualityRangeLimit.RangeInclusive) ^ (summary.Minimum > lowLimit);

                    if (!minValid)
                    {
                        quality |= QualityFlags.Unreasonable;
                        break;
                    }
                }
            }

            if (!(summary.Maximum >= summary.Average) || !(summary.Average >= summary.Minimum))
                quality |= QualityFlags.Noncongruent;

            return quality;
        }

        private async Task UpdateLatchedPoints(API hids, MeterDataSet meterDataSet)
        {
            List<Point> updatedPoints = new List<Point>();
            Point previousPoint = null;

            await foreach (Point point in QueryPointsAsync(hids, meterDataSet))
            {
                try
                {
                    if (previousPoint is null)
                        continue;

                    if (previousPoint.Tag != point.Tag)
                        continue;

                    TimeSpan timeDiff = point.Timestamp - previousPoint.Timestamp;

                    bool wasLatched = (previousPoint.QualityFlags & (uint)QualityFlags.Latched) != 0;

                    bool isLatched =
                        previousPoint.Tag == point.Tag &&
                        timeDiff.TotalHours < 1.0D &&
                        previousPoint.Maximum == point.Maximum &&
                        previousPoint.Minimum == point.Minimum &&
                        previousPoint.Average == point.Average;

                    if (wasLatched == isLatched)
                        continue;

                    if (isLatched)
                        point.QualityFlags |= (uint)QualityFlags.Latched;
                    else
                        point.QualityFlags &= ~(uint)QualityFlags.Latched;

                    updatedPoints.Add(point);
                }
                finally
                {
                    previousPoint = point;
                }
            }

            if (updatedPoints.Any())
                await hids.WritePointsAsync(updatedPoints);
        }

        private async IAsyncEnumerable<Point> QueryPointsAsync(API hids, MeterDataSet meterDataSet)
        {
            static TimeRange TwoHourWindowAround(DateTime dt)
            {
                DateTime start = dt.AddHours(-1.0D);
                DateTime end = dt.AddHours(1.0D);
                return new TimeRange(start, end);
            }

            TrendingDataSummaryResource trendingDataSummaryResource = meterDataSet.GetResource<TrendingDataSummaryResource>();
            Dictionary<Channel, List<TrendingDataSummaryResource.TrendingDataSummary>> trendingDataSummaries = trendingDataSummaryResource.TrendingDataSummaries;

            IEnumerable<TimeRange> twoHourWindows = trendingDataSummaries
                .SelectMany(kvp => kvp.Value)
                .Select(summary => TwoHourWindowAround(summary.Time));

            IEnumerable<TimeRange> mergedWindows = TimeRange.MergeAllOverlapping(twoHourWindows);

            foreach (TimeRange timeRange in mergedWindows)
            {
                IEnumerable<string> tags = trendingDataSummaries.Keys
                    .Select(summary => summary.ID)
                    .Select(hids.ToTag);

                void BuildQuery(IQueryBuilder builder) => builder
                    .Range(timeRange.Start, timeRange.End)
                    .FilterTags(tags);

                await foreach (Point point in hids.ReadPointsAsync(BuildQuery))
                    yield return point;
            }
        }

        #endregion

        #region [ Static ]

        // Static Fields
        private static readonly ILog Log = LogManager.GetLogger(typeof(TrendingDataSummaryOperation));

        #endregion
    }
}
