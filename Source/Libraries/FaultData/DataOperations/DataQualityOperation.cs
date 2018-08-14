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
using System.Linq;
using FaultData.DataAnalysis;
using FaultData.DataResources;
using FaultData.DataSets;
using GSF.Data;
using GSF.Data.Model;
using log4net;
using openXDA.Model;

namespace FaultData.DataOperations
{
    public class DataQualityOperation : DataOperationBase<MeterDataSet>
    {
        #region [ Members ]

        // Nested Types
        private class ChannelSummary
        {
            #region [ Members ]

            // Fields
            private Channel m_channel;
            private DateTime m_date;
            private TrendingDataSummaryResource.TrendingDataSummary m_trendingSummary;

            #endregion

            #region [ Constructors ]

            public ChannelSummary(Channel channel, TrendingDataSummaryResource.TrendingDataSummary summary)
            {
                m_channel = channel;
                m_date = summary.Time.Date;
                m_trendingSummary = summary;
            }

            #endregion

            #region [ Properties ]

            public Channel Channel
            {
                get
                {
                    return m_channel;
                }
            }

            public DateTime Date
            {
                get
                {
                    return m_date;
                }
            }

            public TrendingDataSummaryResource.TrendingDataSummary TrendingSummary
            {
                get
                {
                    return m_trendingSummary;
                }
            }

            #endregion
        }

        #endregion

        #region [ Methods ]

        public override void Execute(MeterDataSet meterDataSet)
        {
            Dictionary<Channel, List<DataGroup>> trendingGroups = meterDataSet.GetResource<TrendingGroupsResource>().TrendingGroups;

            if (trendingGroups.Count == 0)
            {
                Log.Debug($"No trending data found; skipping {nameof(DataQualityOperation)}.");
                return;
            }

            // Process the data quality range limits to identify unreasonable values
            ProcessDataQualityRangeLimits(meterDataSet);

            // Get the total cumulative samples per hour
            // of each of the enabled channels in the meter
            double meterSamplesPerHour = meterDataSet.Meter.Channels
                .Where(channel => channel.Enabled)
                .Where(channel => channel.SamplesPerHour <= 60.0D)
                .Select(channel => channel.SamplesPerHour)
                .DefaultIfEmpty(0.0D)
                .Sum();

            // Convert trending data summaries to channel summaries
            // so that we can order by date to make processing easier
            List<ChannelSummary> channelSummaries = meterDataSet.GetResource<TrendingDataSummaryResource>().TrendingDataSummaries
                .SelectMany(kvp => kvp.Value.Select(summary => new ChannelSummary(kvp.Key, summary)))
                .Where(channelSummary => channelSummary.Channel.Enabled)
                .OrderBy(channelSummary => channelSummary.Date)
                .ThenBy(channelSummary => channelSummary.Channel.ID)
                .ToList();

            if (channelSummaries.Count == 0)
                return;

            using (AdoDataConnection connection = meterDataSet.CreateDbConnection())
            {
                TableOperations<ChannelDataQualitySummary> channelDataQualitySummaryTable = new TableOperations<ChannelDataQualitySummary>(connection);
                TableOperations<MeterDataQualitySummary> meterDataQualitySummaryTable = new TableOperations<MeterDataQualitySummary>(connection);

                ChannelDataQualitySummary channelDataQualitySummary = null;
                MeterDataQualitySummary meterDataQualitySummary = null;

                foreach (ChannelSummary channelSummary in channelSummaries)
                {
                    // If the current channel summary's data does not belong in the same aggregate as the previous channel summary,
                    // update the meter summary record, submit the current channel summary record, and create a new channel summary record
                    if ((object)channelDataQualitySummary != null && (channelSummary.Channel.ID != channelDataQualitySummary.ChannelID || channelSummary.Date != channelDataQualitySummary.Date))
                    {
                        meterDataQualitySummary.GoodPoints += channelDataQualitySummary.GoodPoints;
                        meterDataQualitySummary.LatchedPoints += channelDataQualitySummary.LatchedPoints;
                        meterDataQualitySummary.UnreasonablePoints += channelDataQualitySummary.UnreasonablePoints;
                        meterDataQualitySummary.NoncongruentPoints += channelDataQualitySummary.NoncongruentPoints;
                        meterDataQualitySummary.DuplicatePoints += channelDataQualitySummary.DuplicatePoints;

                        channelDataQualitySummaryTable.Upsert(channelDataQualitySummary);
                        channelDataQualitySummary = null;
                    }

                    // If the current channel summary's data does not fall on the same date as the previous
                    // channel summary's data, submit the current meter summary record and create a new one
                    if ((object)meterDataQualitySummary != null && channelSummary.Date != meterDataQualitySummary.Date)
                    {
                        meterDataQualitySummaryTable.Upsert(meterDataQualitySummary);
                        meterDataQualitySummary = null;
                    }

                    // If there is no existing record to aggregate
                    // channel summary data, create a new one
                    if ((object)channelDataQualitySummary == null)
                    {
                        channelDataQualitySummary = new ChannelDataQualitySummary();
                        channelDataQualitySummary.ChannelID = channelSummary.Channel.ID;
                        channelDataQualitySummary.Date = channelSummary.Date;
                        channelDataQualitySummary.ExpectedPoints = (int)Math.Round(24.0D * channelSummary.Channel.SamplesPerHour);
                        channelDataQualitySummary.GoodPoints = 0;
                        channelDataQualitySummary.LatchedPoints = 0;
                        channelDataQualitySummary.UnreasonablePoints = 0;
                        channelDataQualitySummary.NoncongruentPoints = 0;
                        channelDataQualitySummary.DuplicatePoints = 0;
                    }

                    // If there is no existing record to aggregate
                    // meter summary data, create a new one
                    if ((object)meterDataQualitySummary == null)
                    {
                        meterDataQualitySummary = new MeterDataQualitySummary();
                        meterDataQualitySummary.MeterID = meterDataSet.Meter.ID;
                        meterDataQualitySummary.Date = channelSummary.Date;
                        meterDataQualitySummary.ExpectedPoints = (int)Math.Round(24.0D * meterSamplesPerHour);
                        meterDataQualitySummary.GoodPoints = 0;
                        meterDataQualitySummary.LatchedPoints = 0;
                        meterDataQualitySummary.UnreasonablePoints = 0;
                        meterDataQualitySummary.NoncongruentPoints = 0;
                        meterDataQualitySummary.DuplicatePoints = 0;
                    }

                    // Update the channel summary aggregates
                    // based on the current channel summary
                    if (channelSummary.TrendingSummary.IsDuplicate)
                        channelDataQualitySummary.DuplicatePoints++;
                    else if (channelSummary.TrendingSummary.Latched)
                        channelDataQualitySummary.LatchedPoints++;
                    else if (channelSummary.TrendingSummary.Unreasonable)
                        channelDataQualitySummary.UnreasonablePoints++;
                    else if (channelSummary.TrendingSummary.NonCongruent)
                        channelDataQualitySummary.NoncongruentPoints++;
                    else
                        channelDataQualitySummary.GoodPoints++;
                }

                // Make sure the last channel and meter summary
                // records get submitted to the database
                if ((object)channelDataQualitySummary != null)
                {
                    meterDataQualitySummary.GoodPoints += channelDataQualitySummary.GoodPoints;
                    meterDataQualitySummary.LatchedPoints += channelDataQualitySummary.LatchedPoints;
                    meterDataQualitySummary.UnreasonablePoints += channelDataQualitySummary.UnreasonablePoints;
                    meterDataQualitySummary.NoncongruentPoints += channelDataQualitySummary.NoncongruentPoints;
                    meterDataQualitySummary.DuplicatePoints += channelDataQualitySummary.DuplicatePoints;

                    channelDataQualitySummaryTable.Upsert(channelDataQualitySummary);
                    meterDataQualitySummaryTable.Upsert(meterDataQualitySummary);
                }
            }
        }

        private void ProcessDataQualityRangeLimits(MeterDataSet meterDataSet)
        {
            TrendingDataSummaryResource trendingDataSummaryResource = meterDataSet.GetResource<TrendingDataSummaryResource>();
            Dictionary<Channel, List<TrendingDataSummaryResource.TrendingDataSummary>> trendingDataSummaries = trendingDataSummaryResource.TrendingDataSummaries;
            TrendingDataSummaryResource.TrendingDataSummary previousSummary = null;

            if (trendingDataSummaries.Count == 0)
                return;

            using (AdoDataConnection connection = meterDataSet.CreateDbConnection())
            {
                foreach (KeyValuePair<Channel, List<TrendingDataSummaryResource.TrendingDataSummary>> keyValuePair in trendingDataSummaries)
                {
                    Channel channel = keyValuePair.Key;
                    double perUnitValue = channel.PerUnitValue ?? 1.0D;

                    List<DataQualityRangeLimit> dataQualityRangeLimits = InitializeRangeLimitTable(connection, channel);

                    foreach (TrendingDataSummaryResource.TrendingDataSummary trendingDataSummary in keyValuePair.Value)
                    {
                        if ((object)previousSummary != null && trendingDataSummary.Minimum == previousSummary.Minimum && trendingDataSummary.Average == previousSummary.Average && trendingDataSummary.Maximum == previousSummary.Maximum)
                            trendingDataSummary.Latched = true;

                        if (trendingDataSummary.Average < trendingDataSummary.Minimum || trendingDataSummary.Average > trendingDataSummary.Maximum)
                            trendingDataSummary.NonCongruent = true;

                        foreach (DataQualityRangeLimit dataQualityRangeLimit in dataQualityRangeLimits.Where(row => row.Enabled))
                        {
                            double highLimit = 0.0D;
                            double lowLimit = 0.0D;
                            bool maxValid = true;
                            bool minValid = true;

                            double factor = dataQualityRangeLimit.PerUnit ? perUnitValue : 1.0D;

                            if ((object)dataQualityRangeLimit.High != null)
                            {
                                highLimit = factor * dataQualityRangeLimit.High.GetValueOrDefault();
                                maxValid = Convert.ToBoolean(dataQualityRangeLimit.RangeInclusive) ^ (trendingDataSummary.Maximum < highLimit);
                            }

                            if ((object)dataQualityRangeLimit.Low != null)
                            {
                                lowLimit = factor * dataQualityRangeLimit.Low.GetValueOrDefault();
                                minValid = Convert.ToBoolean(dataQualityRangeLimit.RangeInclusive) ^ (trendingDataSummary.Minimum > lowLimit);
                            }

                            if (!minValid || !maxValid)
                            {
                                trendingDataSummary.Unreasonable = true;
                                trendingDataSummary.HighLimit = highLimit;
                                trendingDataSummary.LowLimit = lowLimit;

                                if (!maxValid)
                                    trendingDataSummary.UnreasonableValue = trendingDataSummary.Maximum;
                                else
                                    trendingDataSummary.UnreasonableValue = trendingDataSummary.Minimum;

                                break;
                            }
                        }

                        previousSummary = trendingDataSummary;
                    }
                }
            }
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

        #endregion

        #region [ Static ]

        // Static Fields
        private static readonly ILog Log = LogManager.GetLogger(typeof(DataQualityOperation));

        #endregion
    }
}
