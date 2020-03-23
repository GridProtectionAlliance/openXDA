//******************************************************************************************************
//  RelayEnergization.cs - Gbtc
//
//  Copyright © 2019, Grid Protection Alliance.  All Rights Reserved.
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
//  07/09/2019 - Christoph Lackner
//       Generated original version of source code.
//
//******************************************************************************************************


using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;
using FaultData.DataAnalysis;
using FaultData.DataResources;
using FaultData.DataOperations;
using FaultData.DataSets;
using GSF.Data;
using GSF.Data.Model;
using openXDA.Model;
using log4net;

namespace FaultData.DataOperations
{
    public class RelayEnergization : DataOperationBase<MeterDataSet>
    {
        public override void Execute(MeterDataSet meterDataSet)
        {
            for (int i = meterDataSet.DataSeries.Count - 1; i >= 0; i--)
            {
                Series seriesInfo = meterDataSet.DataSeries[i].SeriesInfo;
                Channel channel = seriesInfo.Channel;

                // Check if Channel is Relay Coil Current
                if (IsRelayEnergization(channel))
                {
                    Log.Info("Processing Trip Coil Energization Current....");

                    DateTime tripInitiate;
                    DateTime Imax1Time;
                    DateTime latchOff;
                    DateTime fingerOpen;

                    Double Imax1;
                    Double Imax2;
                    Double tripCoilCondition;

                    // Determine P1 (Trip Initiate Time)
                    // find value above threshhold and walk backwards until I increases
                    int threshholdIndex = meterDataSet.DataSeries[i].Threshhold(0.15);
                    int minIndex = threshholdIndex - 1;


                    if (threshholdIndex < 1)
                    {
                        Log.Info("Trip Coil Energization Current too low.");
                        continue;
                    }
                    
                    while (minIndex > 0)
                    {
                        if (meterDataSet.DataSeries[i][minIndex].LargerThan(meterDataSet.DataSeries[i][minIndex+1]))
                        {
                            break;
                        }
                        if (!meterDataSet.DataSeries[i][minIndex].LargerThan(0.0001))
                        { 
                            break;
                        }
                        minIndex--;
                    }
                    tripInitiate = meterDataSet.DataSeries[i][minIndex + 1].Time;
                    

                    // Determine First Peak Point (To get L1 and the Trip Coil Condition)
                    // Find point where the following 10 points are lower (I)
                    int lowerIndex = threshholdIndex;
                    DataSeries window = meterDataSet.DataSeries[i].ToSubSeries(lowerIndex, lowerIndex + 9);

                    while (window.Length == 10)
                    {
                        if (window.Maximum == window[0].Value)
                        {
                            break;
                        }
                        lowerIndex = lowerIndex + window.DataPoints.FindLastIndex(p => p.Value == window.Maximum);

                        window = meterDataSet.DataSeries[i].ToSubSeries(lowerIndex, lowerIndex + 9);
                    }

                    Imax1Time = window[0].Time;
                    Imax1 = window[0].Value;

                    // Determine Slope of curve (Trip Coil Condition)
                    // Find middle between P1 and Imax1Time and compute average slope of 10% of the points in between
                    int center = minIndex + 1 + (lowerIndex - minIndex) / 2;
                    int bound = (lowerIndex - minIndex) / 5;

                    window = meterDataSet.DataSeries[i].ToSubSeries(center - bound, center + bound);
                    tripCoilCondition = LeastSquare(window);

                    // Determine P2 (Latch Off)
                    // Find point where the following 10 points are higher (I)
                    window = meterDataSet.DataSeries[i].ToSubSeries(lowerIndex, lowerIndex + 9);
                    while (window.Length == 10)
                    {
                        if (window.Minimum == window[0].Value)
                        {
                            break;
                        }
                        lowerIndex = lowerIndex + window.DataPoints.FindLastIndex(p => p.Value == window.Minimum);

                        window = meterDataSet.DataSeries[i].ToSubSeries(lowerIndex, lowerIndex + 9);
                    }

                    latchOff = window[0].Time;

                    // Determine P3 (A Finger Open) and L2 (Maximum Current)
                    // Find point where the following 10 points are lower (I)
                    window = meterDataSet.DataSeries[i].ToSubSeries(lowerIndex, lowerIndex + 9);

                    while (window.Length == 10)
                    {
                        if (window.Maximum == window[0].Value)
                        {
                            break;
                        }
                        lowerIndex = lowerIndex + window.DataPoints.FindLastIndex(p => p.Value == window.Maximum);

                        window = meterDataSet.DataSeries[i].ToSubSeries(lowerIndex, lowerIndex + 9);
                    }

                    Imax2 = window[0].Value;
                    fingerOpen = window[0].Time;

                    // Save Relay Characteristics
                    Log.Info("Saving Trip Coil Characteristics to DB.");

                    using (AdoDataConnection connection = meterDataSet.CreateDbConnection())
                    {
                        TableOperations<RelayPerformance> relayStatTable = new TableOperations<RelayPerformance>(connection);
                        Event evt = FindEvent(meterDataSet, channel);
                        //IDbDataParameter tripTime = ToDateTime2(connection, tripInitiate);

                        relayStatTable.AddNewRecord(new RelayPerformance()
                        {
                            EventID = evt.ID,
                            ChannelID = channel.ID,
                            Imax1 = Imax1,
                            Imax2 = Imax2,
                            TripInitiate = tripInitiate,
                            PickupTimeTicks = (int)((latchOff - tripInitiate).TotalMilliseconds * 10000),
                            TripTimeTicks = (int)((fingerOpen - tripInitiate).TotalMilliseconds * 10000),
                            TripCoilCondition = double.IsNaN(tripCoilCondition) ? (double?)null : tripCoilCondition
                        });
                    }
                    
                }
            }
        }

        #region[ Static ]

        private static Boolean IsRelayEnergization(Channel channel)
        {
            return channel.MeasurementType.Name == "TripCoilCurrent" &&
                   (channel.MeasurementCharacteristic.Name == "Instantaneous");
        }

        private static Double LeastSquare(DataSeries series)
        {
            // Assume that all points are uniformly sampled
                        
            // Calculate the Slope
            // Find the values S1, Sx, Sy, Sxx, and Sxy.
            Double N = series.Length;
            Double Sy = series.DataPoints.Select(dataPoint => dataPoint.Value).Sum();
            Double Sxx = 0;
            Double Sx = 0;
            Double Sxy = 0;

            for (int i = 0; i < N; i++)
            {
                Sxx += i * i;
                Sx += i;
                Sxy += series[i].Value * i;
            }

            Double m = (Sxy * N - Sx * Sy) / (Sxx * N - Sx * Sx);

            return (m*series.SampleRate);
        }

        private static List<DataGroup> FindDataGroups(MeterDataSet meterDataSet)
        {
            DataGroupsResource dataGroupsResource = meterDataSet.GetResource<DataGroupsResource>();
            return dataGroupsResource.DataGroups.ToList();
        }

        private static Event FindEvent(MeterDataSet meterDataSet, Channel channel)
        {
            List<DataGroup> dataGroups = FindDataGroups(meterDataSet);

            dataGroups = dataGroups.Where(
                dataGroup => 
                dataGroup.DataSeries.Select(
                    dataSeries => 
                    dataSeries.SeriesInfo.Channel.ID)
                    .ToList().Contains(channel.ID)
                ).ToList();

            if (dataGroups.Count ==0)
            {
                return null;
            }

            Event evt;

            const string Filter =
                    "FileGroupID = {0} AND " +
                    "LineID = {1} AND " +
                    "StartTime = {2} AND " +
                    "EndTime = {3}";
            
            int fileGroupID = meterDataSet.FileGroup.ID;
            int lineID = dataGroups[0].Line.ID;

            using (AdoDataConnection connection = meterDataSet.CreateDbConnection())
            {


                IDbDataParameter startTime = ToDateTime2(connection, dataGroups[0].StartTime);
                IDbDataParameter endTime = ToDateTime2(connection, dataGroups[0].EndTime);

                TableOperations<Event> eventTable = new TableOperations<Event>(connection);
                evt = eventTable.QueryRecordWhere(Filter, fileGroupID, lineID, startTime, endTime);

            }
            return evt;
        }

        private static IDbDataParameter ToDateTime2(AdoDataConnection connection, DateTime dateTime)
        {
            using (IDbCommand command = connection.Connection.CreateCommand())
            {
                IDbDataParameter parameter = command.CreateParameter();
                parameter.DbType = DbType.DateTime2;
                parameter.Value = dateTime;
                return parameter;
            }
        }

        private static readonly ILog Log = LogManager.GetLogger(typeof(RelayEnergization));

        #endregion
    }

}
