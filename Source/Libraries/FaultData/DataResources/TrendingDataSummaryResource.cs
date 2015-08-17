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
using FaultData.Database;
using FaultData.DataSets;
using GSF.Collections;

namespace FaultData.DataResources
{
    public class TrendingDataSummaryResource : DataResourceBase<MeterDataSet>
    {
        #region [ Members ]

        // Nested Types
        public class TrendingDataSummary
        {
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

            public bool IsValid
            {
                get
                {
                    return !(Latched || NonCongruent || Unreasonable);
                }
            }
        }

        // Constants
        private const int SignificantDigits = 5;

        // Fields
        private Dictionary<Channel, List<TrendingDataSummary>> m_trendingDataSummaries;

        #endregion

        #region [ Properties ]

        public Dictionary<Channel, List<TrendingDataSummary>> TrendingDataSummaries
        {
            get
            {
                return m_trendingDataSummaries;
            }
        }

        #endregion

        #region [ Methods ]

        public override void Initialize(MeterDataSet meterDataSet)
        {
            Dictionary<Channel, List<DataGroup>> trendingGroups = meterDataSet.GetResource<TrendingGroupsResource>().TrendingGroups;

            Dictionary<string, DataSeries> seriesLookup;
            DataSeries minSeries;
            DataSeries maxSeries;
            DataSeries avgSeries;

            List<TrendingDataSummary> summaries;
            TrendingDataSummary summary;

            m_trendingDataSummaries = new Dictionary<Channel, List<TrendingDataSummary>>();

            foreach (KeyValuePair<Channel, List<DataGroup>> trendingGroup in trendingGroups)
            {
                summaries = m_trendingDataSummaries.GetOrAdd(trendingGroup.Key, channel => new List<TrendingDataSummary>());

                foreach (DataGroup dataGroup in trendingGroup.Value)
                {
                    seriesLookup = dataGroup.DataSeries.ToDictionary(series => series.SeriesInfo.SeriesType.Name);
                    seriesLookup.TryGetValue("Minimum", out minSeries);
                    seriesLookup.TryGetValue("Maximum", out maxSeries);
                    seriesLookup.TryGetValue("Average", out avgSeries);

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
                        summary = new TrendingDataSummary();

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

                        summaries.Add(summary);
                    }
                }
            }
        }

        // TODO: Determine if this helps or hurts
        private double Round(double value, int significantDigits)
        {
            return Math.Round(value, significantDigits - ((int)Math.Log10(value) + 1));
        }

        #endregion
    }
}
