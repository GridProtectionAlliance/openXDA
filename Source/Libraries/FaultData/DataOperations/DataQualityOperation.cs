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
using System.Data.SqlClient;
using System.Linq;
using FaultData.Database;
using FaultData.Database.DataQualityTableAdapters;
using FaultData.DataResources;
using FaultData.DataSets;

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

        // Fields
        private DbAdapterContainer m_dbAdapterContainer;

        #endregion

        #region [ Methods ]

        public override void Prepare(DbAdapterContainer dbAdapterContainer)
        {
            m_dbAdapterContainer = dbAdapterContainer;
        }

        public override void Execute(MeterDataSet meterDataSet)
        {
            List<ChannelSummary> channelSummaries;

            ChannelDataQualitySummaryTableAdapter channelSummaryAdapter = m_dbAdapterContainer.GetAdapter<ChannelDataQualitySummaryTableAdapter>();
            MeterDataQualitySummaryTableAdapter meterSummaryAdapter = m_dbAdapterContainer.GetAdapter<MeterDataQualitySummaryTableAdapter>();
            DataQuality.MeterDataQualitySummaryDataTable meterSummaryTable = new DataQuality.MeterDataQualitySummaryDataTable();
            DataQuality.ChannelDataQualitySummaryDataTable channelSummaryTable = new DataQuality.ChannelDataQualitySummaryDataTable();
            DataQuality.ChannelDataQualitySummaryRow channelSummaryRow = null;
            DataQuality.MeterDataQualitySummaryRow meterSummaryRow = null;

            double meterSamplesPerHour;

            // Process the data quality range limits to identify unreasonable values
            ProcessDataQualityRangeLimits(meterDataSet);

            // Get the total cumulative samples per hour
            // of each of the enabled channels in the meter
            meterSamplesPerHour = meterDataSet.Meter.Channels
                .Where(channel => channel.Enabled != 0)
                .Where(channel => channel.SamplesPerHour <= 60.0D)
                .Select(channel => channel.SamplesPerHour)
                .DefaultIfEmpty(0.0D)
                .Sum();

            // Convert trending data summaries to channel summaries
            // so that we can order by date to make processing easier
            channelSummaries = meterDataSet.GetResource<TrendingDataSummaryResource>().TrendingDataSummaries
                .SelectMany(kvp => kvp.Value.Select(summary => new ChannelSummary(kvp.Key, summary)))
                .Where(channelSummary => channelSummary.Channel.Enabled != 0)
                .OrderBy(channelSummary => channelSummary.Date)
                .ThenBy(channelSummary => channelSummary.Channel.ID)
                .ToList();

            foreach (ChannelSummary channelSummary in channelSummaries)
            {
                // If the current channel summary's data does not belong in the same aggregate as the previous channel summary,
                // update the meter summary record, submit the current channel summary record, and create a new channel summary record
                if ((object)channelSummaryRow != null && (channelSummary.Channel.ID != channelSummaryRow.ChannelID || channelSummary.Date != channelSummaryRow.Date))
                {
                    meterSummaryRow.GoodPoints += channelSummaryRow.GoodPoints;
                    meterSummaryRow.LatchedPoints += channelSummaryRow.LatchedPoints;
                    meterSummaryRow.UnreasonablePoints += channelSummaryRow.UnreasonablePoints;
                    meterSummaryRow.NoncongruentPoints += channelSummaryRow.NoncongruentPoints;
                    meterSummaryRow.DuplicatePoints += channelSummaryRow.DuplicatePoints;

                    channelSummaryRow.EndEdit();
                    channelSummaryAdapter.Upsert(channelSummaryRow);
                    channelSummaryRow = null;
                }

                // If the current channel summary's data does not fall on the same date as the previous
                // channel summary's data, submit the current meter summary record and create a new one
                if ((object)meterSummaryRow != null && channelSummary.Date != meterSummaryRow.Date)
                {
                    meterSummaryRow.EndEdit();
                    meterSummaryAdapter.Upsert(meterSummaryRow);
                    meterSummaryRow = null;
                }

                // If there is no existing record to aggregate
                // channel summary data, create a new one
                if ((object)channelSummaryRow == null)
                {
                    channelSummaryRow = channelSummaryTable.NewChannelDataQualitySummaryRow();

                    channelSummaryRow.BeginEdit();
                    channelSummaryRow.ChannelID = channelSummary.Channel.ID;
                    channelSummaryRow.Date = channelSummary.Date;
                    channelSummaryRow.ExpectedPoints = (int)Math.Round(24.0D * channelSummary.Channel.SamplesPerHour);
                    channelSummaryRow.GoodPoints = 0;
                    channelSummaryRow.LatchedPoints = 0;
                    channelSummaryRow.UnreasonablePoints = 0;
                    channelSummaryRow.NoncongruentPoints = 0;
                    channelSummaryRow.DuplicatePoints = 0;
                }

                // If there is no existing record to aggregate
                // meter summary data, create a new one
                if ((object)meterSummaryRow == null)
                {
                    meterSummaryRow = meterSummaryTable.NewMeterDataQualitySummaryRow();

                    meterSummaryRow.BeginEdit();
                    meterSummaryRow.MeterID = meterDataSet.Meter.ID;
                    meterSummaryRow.Date = channelSummary.Date;
                    meterSummaryRow.ExpectedPoints = (int)Math.Round(24.0D * meterSamplesPerHour);
                    meterSummaryRow.GoodPoints = 0;
                    meterSummaryRow.LatchedPoints = 0;
                    meterSummaryRow.UnreasonablePoints = 0;
                    meterSummaryRow.NoncongruentPoints = 0;
                    meterSummaryRow.DuplicatePoints = 0;
                }

                // Update the channel summary aggregates
                // based on the current channel summary
                if (channelSummary.TrendingSummary.IsDuplicate)
                    channelSummaryRow.DuplicatePoints++;
                else if (channelSummary.TrendingSummary.Latched)
                    channelSummaryRow.LatchedPoints++;
                else if (channelSummary.TrendingSummary.Unreasonable)
                    channelSummaryRow.UnreasonablePoints++;
                else if (channelSummary.TrendingSummary.NonCongruent)
                    channelSummaryRow.NoncongruentPoints++;
                else
                    channelSummaryRow.GoodPoints++;
            }

            // Make sure the last channel and meter summary
            // records get submitted to the database
            if ((object)channelSummaryRow != null)
            {
                meterSummaryRow.GoodPoints += channelSummaryRow.GoodPoints;
                meterSummaryRow.LatchedPoints += channelSummaryRow.LatchedPoints;
                meterSummaryRow.UnreasonablePoints += channelSummaryRow.UnreasonablePoints;
                meterSummaryRow.NoncongruentPoints += channelSummaryRow.NoncongruentPoints;
                meterSummaryRow.DuplicatePoints += channelSummaryRow.DuplicatePoints;

                channelSummaryRow.EndEdit();
                channelSummaryAdapter.Upsert(channelSummaryRow);

                meterSummaryRow.EndEdit();
                meterSummaryAdapter.Upsert(meterSummaryRow);
            }
        }

        public override void Load(DbAdapterContainer dbAdapterContainer)
        {
        }

        private void ProcessDataQualityRangeLimits(MeterDataSet meterDataSet)
        {
            DataQuality.DataQualityRangeLimitDataTable rangeLimitTable;

            Dictionary<Channel, List<TrendingDataSummaryResource.TrendingDataSummary>> trendingDataSummaries;
            Channel channel;

            TrendingDataSummaryResource.TrendingDataSummary previousSummary = null;

            double perUnitValue;
            double lowLimit;
            double highLimit;
            bool minValid;
            bool maxValid;

            trendingDataSummaries = meterDataSet.GetResource<TrendingDataSummaryResource>().TrendingDataSummaries;
            rangeLimitTable = new DataQuality.DataQualityRangeLimitDataTable();

            foreach (KeyValuePair<Channel, List<TrendingDataSummaryResource.TrendingDataSummary>> keyValuePair in trendingDataSummaries)
            {
                channel = keyValuePair.Key;
                perUnitValue = channel.PerUnitValue ?? 1.0D;
                InitializeRangeLimitTable(rangeLimitTable, channel);

                foreach (TrendingDataSummaryResource.TrendingDataSummary trendingDataSummary in keyValuePair.Value)
                {
                    if ((object)previousSummary != null && trendingDataSummary.Minimum == previousSummary.Minimum && trendingDataSummary.Average == previousSummary.Average && trendingDataSummary.Maximum == previousSummary.Maximum)
                        trendingDataSummary.Latched = true;

                    if (trendingDataSummary.Average < trendingDataSummary.Minimum || trendingDataSummary.Average > trendingDataSummary.Maximum)
                        trendingDataSummary.NonCongruent = true;

                    foreach (DataQuality.DataQualityRangeLimitRow row in rangeLimitTable.Where(row => row.Enabled != 0))
                    {
                        highLimit = 0.0D;
                        lowLimit = 0.0D;
                        maxValid = true;
                        minValid = true;

                        if (!row.IsHighNull())
                        {
                            highLimit = Convert.ToBoolean(row.PerUnit) ? row.High * perUnitValue : row.High;
                            maxValid = Convert.ToBoolean(row.RangeInclusive) ^ (trendingDataSummary.Maximum < highLimit);
                        }

                        if (!row.IsLowNull())
                        {
                            lowLimit = Convert.ToBoolean(row.PerUnit) ? row.Low * perUnitValue : row.Low;
                            minValid = Convert.ToBoolean(row.RangeInclusive) ^ (trendingDataSummary.Minimum > lowLimit);
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

        private void InitializeRangeLimitTable(DataQuality.DataQualityRangeLimitDataTable rangeLimitTable, Channel channel)
        {
            DataQualityRangeLimitTableAdapter rangeLimitAdapter;
            DefaultDataQualityRangeLimitTableAdapter defaultRangeLimitAdapter;
            DataQuality.DefaultDataQualityRangeLimitDataTable defaultRangeLimitTable;
            DataQuality.DataQualityRangeLimitRow rangeLimitRow;

            // Clear existing rows from the range limit table
            rangeLimitTable.Clear();

            // Fill the range limit table with range limits for the given channel
            rangeLimitAdapter = m_dbAdapterContainer.GetAdapter<DataQualityRangeLimitTableAdapter>();
            rangeLimitAdapter.FillBy(rangeLimitTable, channel.ID);

            // If limits exist for the given channel,
            // range limit table has been successfully initialized
            if (rangeLimitTable.Count != 0)
                return;

            // Get the default range limits for the measurement type and characteristic of this channel
            defaultRangeLimitAdapter = m_dbAdapterContainer.GetAdapter<DefaultDataQualityRangeLimitTableAdapter>();
            defaultRangeLimitTable = defaultRangeLimitAdapter.GetDataBy(channel.MeasurementTypeID, channel.MeasurementCharacteristicID);

            // If there are no default limits for the channel,
            // then the range limit table has been successfully initialized
            if (defaultRangeLimitTable.Count == 0)
                return;

            lock (RangeLimitLock)
            {
                // Fill the range limit table one more time inside the lock to
                // ensure that no other threads have written limits for this channel
                rangeLimitAdapter.FillBy(rangeLimitTable, channel.ID);

                // If there are still no limits defined for this channel,
                // update the table to include this channel's default limits
                if (rangeLimitTable.Count == 0)
                {
                    foreach (DataQuality.DefaultDataQualityRangeLimitRow row in defaultRangeLimitTable)
                    {
                        rangeLimitRow = rangeLimitTable.NewDataQualityRangeLimitRow();
                        rangeLimitRow.ChannelID = channel.ID;
                        rangeLimitRow.RangeInclusive = row.RangeInclusive;
                        rangeLimitRow.PerUnit = row.PerUnit;
                        rangeLimitRow.Enabled = 1;

                        if (!row.IsHighNull())
                            rangeLimitRow.High = row.High;

                        if (!row.IsLowNull())
                            rangeLimitRow.Low = row.Low;

                        rangeLimitTable.AddDataQualityRangeLimitRow(rangeLimitRow);
                    }

                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(m_dbAdapterContainer.Connection))
                    {
                        bulkCopy.BulkCopyTimeout = 0;
                        bulkCopy.DestinationTableName = rangeLimitTable.TableName;
                        bulkCopy.WriteToServer(rangeLimitTable);
                    }
                }
            }
        }

        #endregion

        #region [ Static ]

        // Static Fields
        private static readonly object RangeLimitLock = new object();

        #endregion
    }
}
