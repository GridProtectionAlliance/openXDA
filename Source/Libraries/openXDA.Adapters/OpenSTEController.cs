//******************************************************************************************************
//  OpenSEEController.cs - Gbtc
//
//  Copyright © 2018, Grid Protection Alliance.  All Rights Reserved.
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
//  04/17/2018 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Caching;
using System.Web.Http;
using GSF.Collections;
using GSF.Data;
using GSF.Web;
using openHistorian.XDALink;
using openXDA.Model;

namespace openXDA.Adapters
{
    public class OpenSTEController : ApiController
    {
        #region [ Constructors ]

        public OpenSTEController(Func<AdoDataConnection> connectionFactory) =>
            ConnectionFactory = connectionFactory;

        #endregion

        #region [ Properties ]

        private Func<AdoDataConnection> ConnectionFactory { get; }

        #endregion

        #region [ Methods ]

        [HttpGet]
        public TrendingDataSet GetData()
        {
            TrendingDataSet trendingDataSet = new TrendingDataSet();
            Dictionary<string, string> query = Request.QueryParameters();
            IEnumerable<int> channelIDs = new List<int>() { Convert.ToInt32(query["ChannelID"]) };
            string channelID = query["ChannelID"];
            DateTime startDate = Convert.ToDateTime(query["targetDate"]);

            string target = query["ChannelID"] + query["targetDate"];

            if (s_memoryCache.Contains(target))
                return (TrendingDataSet)s_memoryCache.Get(target);

            using (AdoDataConnection connection = ConnectionFactory())
            {
                string historianServer = connection.ExecuteScalar<string>("SELECT Value FROM Setting WHERE Name = 'Historian.Server'") ?? "127.0.0.1";
                string historianInstance = connection.ExecuteScalar<string>("SELECT Value FROM Setting WHERE Name = 'Historian.Instance'") ?? "XDA";
                DateTime endDate = startDate.AddDays(1);
                DateTime epoch = new DateTime(1970, 1, 1);

                using (Historian historian = new Historian(historianServer, historianInstance))
                {
                    foreach (openHistorian.XDALink.TrendingDataPoint point in historian.Read(channelIDs, startDate, endDate))
                    {
                        if (!trendingDataSet.ChannelData.Exists(x => x.Time == point.Timestamp.Subtract(epoch).TotalMilliseconds))
                        {
                            trendingDataSet.ChannelData.Add(new openXDA.Model.TrendingDataPoint());
                            trendingDataSet.ChannelData[trendingDataSet.ChannelData.Count - 1].Time = point.Timestamp.Subtract(epoch).TotalMilliseconds;
                        }

                        if (point.SeriesID.ToString() == "Average")
                            trendingDataSet.ChannelData[trendingDataSet.ChannelData.IndexOf(x => x.Time == point.Timestamp.Subtract(epoch).TotalMilliseconds)].Average = point.Value;
                        else if (point.SeriesID.ToString() == "Minimum")
                            trendingDataSet.ChannelData[trendingDataSet.ChannelData.IndexOf(x => x.Time == point.Timestamp.Subtract(epoch).TotalMilliseconds)].Minimum = point.Value;
                        else if (point.SeriesID.ToString() == "Maximum")
                            trendingDataSet.ChannelData[trendingDataSet.ChannelData.IndexOf(x => x.Time == point.Timestamp.Subtract(epoch).TotalMilliseconds)].Maximum = point.Value;

                    }
                }
                IEnumerable<DataRow> table = Enumerable.Empty<DataRow>();

                table = connection.RetrieveData(" Select {0} AS thedatefrom, " +
                                                            "        DATEADD(DAY, 1, {0}) AS thedateto, " +
                                                            "        CASE WHEN AlarmRangeLimit.PerUnit <> 0 AND Channel.PerUnitValue IS NOT NULL THEN AlarmRangeLimit.High * PerUnitValue ELSE AlarmRangeLimit.High END AS alarmlimithigh," +
                                                            "        CASE WHEN AlarmRangeLimit.PerUnit <> 0 AND Channel.PerUnitValue IS NOT NULL THEN AlarmRangeLimit.Low * PerUnitValue ELSE AlarmRangeLimit.Low END AS alarmlimitlow " +
                                                            " FROM   AlarmRangeLimit JOIN " +
                                                            "        Channel ON AlarmRangeLimit.ChannelID = Channel.ID " +
                                                            "WHERE   AlarmRangeLimit.AlarmTypeID = (SELECT ID FROM AlarmType where Name = 'Alarm') AND " +
                                                            "        AlarmRangeLimit.ChannelID = {1}", startDate, Convert.ToInt32(channelID)).Select();

                foreach (DataRow row in table)
                {
                    trendingDataSet.AlarmLimits.Add(new TrendingAlarmLimit() { High = row.Field<double?>("alarmlimithigh"), Low = row.Field<double?>("alarmlimitlow"), TimeEnd = row.Field<DateTime>("thedateto").Subtract(epoch).TotalMilliseconds, TimeStart = row.Field<DateTime>("thedatefrom").Subtract(epoch).TotalMilliseconds });
                }

                table = Enumerable.Empty<DataRow>();

                table = connection.RetrieveData(" DECLARE @dayOfWeek INT = DATEPART(DW, {0}) - 1 " +
                                                            " DECLARE @hourOfWeek INT = @dayOfWeek * 24 " +
                                                            " ; WITH HourlyIndex AS" +
                                                            " ( " +
                                                            "   SELECT @hourOfWeek AS HourOfWeek " +
                                                            "   UNION ALL " +
                                                            "   SELECT HourOfWeek + 1 " +
                                                            "   FROM HourlyIndex" +
                                                            "   WHERE (HourOfWeek + 1) < @hourOfWeek + 24" +
                                                            " ) " +
                                                            " SELECT " +
                                                            "        DATEADD(HOUR, HourlyIndex.HourOfWeek - @hourOfWeek, {0}) AS thedatefrom, " +
                                                            "        DATEADD(HOUR, HourlyIndex.HourOfWeek - @hourOfWeek + 1, {0}) AS thedateto, " +
                                                            "        HourOfWeekLimit.High AS offlimithigh, " +
                                                            "        HourOfWeekLimit.Low AS offlimitlow " +
                                                            " FROM " +
                                                            "        HourlyIndex LEFT OUTER JOIN " +
                                                            "        HourOfWeekLimit ON HourOfWeekLimit.HourOfWeek = HourlyIndex.HourOfWeek " +
                                                            " WHERE " +
                                                            "        HourOfWeekLimit.ChannelID IS NULL OR " +
                                                            "        HourOfWeekLimit.ChannelID = {1} ", startDate, Convert.ToInt32(channelID)).Select();

                foreach (DataRow row in table)
                {
                    trendingDataSet.OffNormalLimits.Add(new TrendingAlarmLimit() { High = row.Field<double?>("offlimithigh"), Low = row.Field<double?>("offlimitlow"), TimeEnd = row.Field<DateTime>("thedateto").Subtract(epoch).TotalMilliseconds, TimeStart = row.Field<DateTime>("thedatefrom").Subtract(epoch).TotalMilliseconds });
                }

                s_memoryCache.Add(target, trendingDataSet, new CacheItemPolicy() { SlidingExpiration = TimeSpan.FromMinutes(10.0D) });

                return trendingDataSet;
            }
        }

        #endregion

        #region [ Static ]

        private static MemoryCache s_memoryCache = new MemoryCache("openSTE");

        #endregion
    }
}