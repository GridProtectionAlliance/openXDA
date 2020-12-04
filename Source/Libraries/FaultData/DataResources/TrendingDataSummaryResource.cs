//******************************************************************************************************
//  TrendingDataSummaryResource.cs - Gbtc
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
using FaultData.DataSets;
using GSF.Collections;
using log4net;
using openXDA.Model;

namespace FaultData.DataResources
{
    public class TrendingDataSummaryResource : DataResourceBase<MeterDataSet>
    {
        #region [ Members ]

        // Nested Types
        public class TrendingDataSummary
        {
            #region [ Members ]

            // Fields
            public DateTime Time;
            public double Maximum;
            public double Minimum;
            public double Average;

            public bool Latched;
            public bool NonCongruent;
            public bool Unreasonable;

            public double UnreasonableValue;
            public double HighLimit;
            public double LowLimit;

            #endregion

            #region [ Properties ]

            public bool IsValid
            {
                get
                {
                    return !(Latched || NonCongruent || Unreasonable);
                }
            }

            #endregion
        }

        // Nested Types
        private class TrendingRange
        {
            #region [ Members ]

            // Fields
            private Channel m_channel;
            private DateTime m_startTime;
            private DateTime m_endTime;

            #endregion

            #region [ Constructors ]

            public TrendingRange(Channel channel, DataGroup dataGroup)
            {
                m_channel = channel;
                m_startTime = dataGroup.StartTime;
                m_endTime = dataGroup.EndTime;
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

            public DateTime StartTime
            {
                get
                {
                    return m_startTime;
                }
            }

            public DateTime EndTime
            {
                get
                {
                    return m_endTime;
                }
            }

            #endregion
        }

        #endregion

        #region [ Properties ]

        public Dictionary<Channel, List<TrendingDataSummary>> TrendingDataSummaries { get; private set; }

        #endregion

        #region [ Methods ]

        public override void Initialize(MeterDataSet meterDataSet)
        {
            TrendingDataSummaries = new Dictionary<Channel, List<TrendingDataSummary>>();

            TrendingGroupsResource trendingGroupsResource = meterDataSet.GetResource<TrendingGroupsResource>();
            Dictionary<Channel, List<DataGroup>> trendingGroups = trendingGroupsResource.TrendingGroups;

            foreach (KeyValuePair<Channel, List<DataGroup>> trendingGroup in trendingGroups)
            {
                List<TrendingDataSummary> summaries = TrendingDataSummaries.GetOrAdd(trendingGroup.Key, channel => new List<TrendingDataSummary>());

                foreach (DataGroup dataGroup in trendingGroup.Value)
                {
                    Dictionary<string, DataSeries> seriesLookup = dataGroup.DataSeries
                        .GroupBy(series => series.SeriesInfo.SeriesType.Name)
                        .ToDictionary(grouping => grouping.Key, grouping =>
                        {
                            if (grouping.Count() > 1)
                                Log.Warn($"Duplicate series type ({grouping.Key}) found while creating trending summaries for channel {trendingGroup.Key.ID}");

                            return grouping.First();
                        });

                    seriesLookup.TryGetValue("Minimum", out DataSeries minSeries);
                    seriesLookup.TryGetValue("Maximum", out DataSeries maxSeries);
                    seriesLookup.TryGetValue("Average", out DataSeries avgSeries);

                    // By the time we exit this if statement,
                    // the min series will have been defined
                    if ((object)minSeries == null)
                    {
                        if ((object)avgSeries != null)
                        {
                            // Use the average series as the min series.
                            // Max series may still be undefined
                            minSeries = avgSeries;
                        }
                        else if ((object)maxSeries != null)
                        {
                            // We know the min and average series are both undefined,
                            // so use the max series for all of them
                            minSeries = maxSeries;
                            avgSeries = maxSeries;
                        }
                        else if (seriesLookup.TryGetValue("Values", out minSeries) || seriesLookup.TryGetValue("Instantaneous", out minSeries))
                        {
                            // All series are undefined, but there exists an instantaneous series
                            // for this channel which can assume the role of min, max, and average
                            maxSeries = minSeries;
                            avgSeries = minSeries;
                        }
                        else
                        {
                            // There is no way to tell what series could reasonably
                            // represent the min, max, and average for this channel
                            continue;
                        }
                    }

                    // By the time we exit this if statement,
                    // the max series will have been defined
                    if ((object)maxSeries == null)
                    {
                        if ((object)avgSeries != null)
                        {
                            // Use the average series as the max series
                            maxSeries = avgSeries;
                        }
                        else
                        {
                            // Both the average and max series are undefined
                            // so use the min series for all of them
                            maxSeries = minSeries;
                            avgSeries = minSeries;
                        }
                    }

                    for (int i = 0; i < dataGroup.Samples; i++)
                    {
                        TrendingDataSummary summary = new TrendingDataSummary();

                        // Get the date-time of the summary
                        summary.Time = minSeries.DataPoints[i].Time;

                        // Get the min and the max of the current sample
                        summary.Minimum = minSeries.DataPoints[i].Value;
                        summary.Maximum = maxSeries.DataPoints[i].Value;

                        // If a series containing average values is not available,
                        // use the average of the max and the min
                        if ((object)avgSeries != null)
                            summary.Average = avgSeries.DataPoints[i].Value;
                        else
                            summary.Average = (summary.Minimum + summary.Maximum) / 2.0D;

                        if (double.IsNaN(summary.Minimum) || double.IsNaN(summary.Maximum) || double.IsNaN(summary.Average))
                            continue;

                        summaries.Add(summary);
                    }
                }
            }
        }

        #endregion

        #region [ Static ]

        // Static Fields
        private static readonly ILog Log = LogManager.GetLogger(typeof(TrendingDataSummaryResource));

        #endregion
    }
}
