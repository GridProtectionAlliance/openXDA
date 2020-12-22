//******************************************************************************************************
//  AlarmOperation.cs - Gbtc
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
//  12/14/2020 - C. lackner
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
using HIDS;
using log4net;
using openXDA.HIDS.APIExtensions;
using openXDA.Model;

namespace openXDA.HIDS
{
    public class HIDSAlarmOperation : DataOperationBase<MeterDataSet>
    {
        #region [ Members ]

        // Fields
        [Category]
        [SettingName(HIDSSettings.CategoryName)]
        public HIDSSettings HIDSSettings { get; }

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Creates a new <see cref="HIDSAlarmOperation"/>.
        /// </summary>
        public HIDSAlarmOperation()
        {
            HIDSSettings = new HIDSSettings();
        }

        #endregion

        #region [ Properties ]

       

        #endregion

        #region [ Methods ]

        public override void Execute(MeterDataSet meterDataSet)
        {
            Log.Info("Executing operation to update trending summary alarms...");

            Dictionary<Channel, List<DataGroup>> trendingGroups = meterDataSet.GetResource<TrendingGroupsResource>().TrendingGroups;

            if (trendingGroups.Count == 0)
            {
                Log.Debug($"No trending data found; skipping {nameof(HIDSAlarmOperation)}.");
                return;
            }
            if (string.IsNullOrEmpty(HIDSSettings.Host))
            {
                Log.Debug($"No HIDS instance defined; skipping {nameof(HIDSAlarmOperation)}.");
                return;
            }

            using (API hids = new API())
            {
                hids.Configure(HIDSSettings);
                TrendingDataSummaryResource trendingDataSummaryResource = meterDataSet.GetResource<TrendingDataSummaryResource>();
                Dictionary<Channel, List<TrendingDataSummaryResource.TrendingDataSummary>> trendingDataSummaries = trendingDataSummaryResource.TrendingDataSummaries;

                foreach (KeyValuePair<Channel, List<TrendingDataSummaryResource.TrendingDataSummary>> channelSummaries in trendingDataSummaries)
                {
                    int channelID = channelSummaries.Key.ID;
                    IEnumerable<TrendingDataSummaryResource.TrendingDataSummary> summaries = channelSummaries.Value;

                    IEnumerable<Point> points = summaries.Select(summary => new Point()
                    {
                        Tag = hids.ToTag(channelID),
                        QualityFlags = 0u,
                        Timestamp = summary.Time,
                        Maximum = summary.Maximum,
                        Average = summary.Average,
                        Minimum = summary.Minimum
                    });

                    List<Point> data = points.ToList();
                    data.Sort((pt1, pt2) => pt1.Timestamp.CompareTo(pt2.Timestamp));

                    using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
                    {
                        IEnumerable<SeriesType> seriesTypes = new TableOperations<SeriesType>(connection).QueryRecordsWhere("Name = 'Average' OR Name = 'Maximum' OR Name = 'Minimum'");
                        TableOperations<AlarmLog> alarmLogTbl = new TableOperations<AlarmLog>(connection);
                        foreach (SeriesType sT in seriesTypes)
                        {
                            // Find Any Alarms for this channel
                            IEnumerable<ActiveAlarm> alarms = new TableOperations<ActiveAlarm>(connection).QueryRecordsWhere("SeriesID IN (SELECT ID FROM Series WHERE ChannelID = {0} AND SeriesTypeID = {1})", channelID, sT.ID);

                            if (alarms.Count() == 0)
                                continue;

                            Func<Point, double> pointProcessor = (Point pt) => pt.Average;

                            if (sT.Name == "Maximum")
                                pointProcessor = (Point pt) => pt.Maximum;
                            if (sT.Name == "Minimum")
                                pointProcessor = (Point pt) => pt.Minimum;

                            foreach (ActiveAlarm alarm in alarms)
                            {
                                ProcessSeriesAlarm(alarm,data, pointProcessor).ForEach(item => alarmLogTbl.AddNewRecord(item));
                            }
                        }
                    }
                }
            }
        }

        private List<AlarmLog> ProcessSeriesAlarm(ActiveAlarm alarm, List<Point> data, Func<Point,double> processPoint)
        {
            bool upper = true;
            IEnumerable<AlarmValue> alarmValues;
            using (AdoDataConnection connection = new AdoDataConnection("SystemSettings"))
            {
                AlarmType alarmType = new TableOperations<AlarmType>(connection).QueryRecordWhere("ID = {0}", alarm.AlarmTypeID);
                if (alarmType.Name == "Lower Limit")
                    upper = false;

                alarmValues = new TableOperations<AlarmValue>(connection).QueryRecordsWhere("AlarmID = {0}", alarm.AlarmID);
            }

            List<AlarmLog> result = new List<AlarmLog>();

            // Create Hour of the week based Threshholds
            bool isSingle = alarmValues.Count() == 1;

            int nHours = (data.Last().Timestamp- data.First().Timestamp).Hours + 1;

            double[] threshholds = new double[nHours];

            if (isSingle)
            {
                threshholds = Enumerable.Repeat(alarmValues.First().Value * alarm.Value, nHours).ToArray();
            }
            else
            {
                // start with First TS of the List
                DateTime dt = data.Min(pt => pt.Timestamp).Date;
                dt = dt.AddHours(data.Min(pt => pt.Timestamp).Hour);

                for (int i = 0; i < nHours; i++)
                {
                    AlarmValue current = alarmValues.Where(item => Applies(dt.AddHours(i),item)).FirstOrDefault();
                    if (current == null)
                        threshholds[i] = double.NaN;
                    else
                        threshholds[i] = current.Value * alarm.Value;
                }
            }

            DateTime startTime = data[0].Timestamp;
            //Find Threshholds Crossings
            List<DateTime> start = data.Skip(1).Where((pt, index) => {
                int hourofWeek = (data[index+1].Timestamp - startTime).Hours;
                double threshold = threshholds[hourofWeek];

                double p1 = (processPoint(data[index]) - threshold) * (upper ? 1.0D : -1.0D);
                double p2 = (processPoint(data[index + 1]) - threshold) * (upper ? 1.0D : -1.0D);

                return (p1 * p2) < 0 && p2 > 0;
            }).Select(pt => pt.Timestamp).ToList();

            List<DateTime> end = data.Take(data.Count - 1).Where((pt, index) => {
                int hourofWeek = (data[index].Timestamp - startTime).Hours;
                double threshold = threshholds[hourofWeek];

                double p1 = (processPoint(data[index]) - threshold) * (upper ? 1.0D : -1.0D);
                double p2 = (processPoint(data[index + 1]) - threshold) * (upper ? 1.0D : -1.0D);

                return (p1 * p2) < 0 && p2 < 0;
            }).Select(pt => pt.Timestamp).ToList();

            // Check if Alarm is on in first point
            double threshold = threshholds[0];

            double p1 = (processPoint(data[0]) - threshold) * (upper ? 1.0D : -1.0D);

            if (start.Count() == 0 && end.Count() == 0 && p1 < 0)
                return new List<AlarmLog>();

            //Special case if the whole Window is in Alarm
            if (start.Count() == 0 && end.Count() == 0 && p1 > 0)
                return new List<AlarmLog>() { 
                    new AlarmLog() {
                        AlarmID= alarm.AlarmID, 
                        AlarmFactorID = alarm.AlarmFactorID,
                        EndTime=data.Last().Timestamp,
                        StartTime = data.First().Timestamp
                    }
                };

            // case if new or old Window are bleeding over 
            // this needs to be handled seperately later...
            if (start.Count() > end.Count())
                end.Add(data.Last().Timestamp);
            if (start.Count() < end.Count())
                start = start.Prepend(data.First().Timestamp).ToList();

            return start.Select((item, index) => new AlarmLog() {
                AlarmID = alarm.AlarmID,
                AlarmFactorID = alarm.AlarmFactorID,
                EndTime = end[index],
                StartTime = item
            }).ToList();
        }

        private bool Applies(DateTime time, AlarmValue value)
        {
            AlarmDay day;
            using (AdoDataConnection connection = new AdoDataConnection("SystemSettings"))
            {
                day = new TableOperations<AlarmDay>(connection).QueryRecordWhere("ID = {0}", value.AlarmdayID);
            }
            if (day == null)
                return (time.Hour >= value.StartHour && time.Hour < value.EndHour);
            if (day.Name == "WeekEnd")
              return (time.Hour >= value.StartHour && time.Hour < value.EndHour && (time.DayOfWeek == DayOfWeek.Sunday || time.DayOfWeek == DayOfWeek.Saturday)); 
            if (day.Name == "WeekDay")
                 return (time.Hour >= value.StartHour && time.Hour < value.EndHour && !(time.DayOfWeek == DayOfWeek.Sunday || time.DayOfWeek == DayOfWeek.Saturday)); 

            return (time.Hour >= value.StartHour && time.Hour < value.EndHour); 
        }

        #endregion

        #region [ Static ]

        // Static Fields
        private static readonly ILog Log = LogManager.GetLogger(typeof(HIDSAlarmOperation));

        #endregion
    }
}
