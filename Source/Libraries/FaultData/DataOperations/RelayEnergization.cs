﻿//******************************************************************************************************
//  RelayEnergization.cs - Gbtc
//
//  Copyright © 2021, Grid Protection Alliance.  All Rights Reserved.
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
//  08/20/2021 - Christoph Lackner
//       Added detection of additional Trip Coil Curve points.
//
//******************************************************************************************************


using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using FaultData.DataAnalysis;
using FaultData.DataResources;
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
                CycleDataResource cycleDataResource = meterDataSet.GetResource<CycleDataResource>();

                // Check if Channel is Relay Coil Current
                if (IsRelayEnergization(channel))
                {
                    Log.Info("Processing Trip Coil Energization Current....");

                    DateTime tripInitiate;
                    DateTime Imax1Time;
                    DateTime latchOff;
                    DateTime fingerOpen;
                    DateTime plungerLatch;
                    DateTime Tdrop;
                    DateTime Tend;
                    DateTime TTripCoilcondition;

                    double Imax1;
                    double Imax2;
                    double tripCoilCondition;
                    double IplungerLatch;
                    double pickupCurrent;
                    double Idrop;

                    // Determine P1 (Trip Initiate Time)
                    // find value above threshhold and walk backwards until I increases
                    int threshholdIndex = meterDataSet.DataSeries[i].Threshhold(0.5);

                    int minIndex = threshholdIndex - 1;


                    if (threshholdIndex < 1 || meterDataSet.DataSeries[i].Maximum < 1.0)
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
                    int Imax1Index = lowerIndex;

                    // Determine Slope of curve (Trip Coil Condition)
                    // Find middle between P1 and Imax1Time and compute average slope of 10% of the points in between
                    int center = minIndex + 1 + (lowerIndex - minIndex) / 2;
                    int bound = (lowerIndex - minIndex) / 5;

                    window = meterDataSet.DataSeries[i].ToSubSeries(center - bound, center + bound);
                    tripCoilCondition = LeastSquare(window);
                    TTripCoilcondition = new DateTime(meterDataSet.DataSeries[i][center - bound].Time.Ticks / 2 + meterDataSet.DataSeries[i][center + bound].Time.Ticks / 2);
                    
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
                    pickupCurrent = window[0].Value;

                    // Find point where Plunger hits latch by determining minimum dI/dt between Imax1Time and latchoff
                    if (lowerIndex - 21 < Imax1Index)
                    {
                        plungerLatch = meterDataSet.DataSeries[i][(lowerIndex + Imax1Index) / 2].Time;
                        IplungerLatch = meterDataSet.DataSeries[i][(lowerIndex + Imax1Index) / 2].Value;
                    }
                    else
                    {
                        window = meterDataSet.DataSeries[i].ToSubSeries(Imax1Index + 10, lowerIndex - 10);
                        List<double> dI = window.DataPoints.Skip(1).Select((p, j) => (window.DataPoints[j+1].Value - window.DataPoints[j].Value)).ToList();
                        double min = dI.Min();
                        int localMinIndex = dI.FindIndex(p => p == min);
                        plungerLatch = window[localMinIndex].Time;
                        IplungerLatch = window[localMinIndex].Value;
                    }

                    // Determine 
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

                    // Find point where dI < -70 A/s as Idrop/TiDrop
                    // and following find dI > 0 to determine Tend
                    if ((lowerIndex+ 10 ) > meterDataSet.DataSeries[i].Length)
                    {
                        Idrop = Imax2;
                        Tdrop = fingerOpen;
                        Tend = fingerOpen;
                    }
                    else
                    {
                        window = meterDataSet.DataSeries[i].ToSubSeries(lowerIndex + 10, meterDataSet.DataSeries[i].Length-1);
                        List<Tuple<double,double>> dI = window.DataPoints.Skip(1).Select((p, j) => new Tuple<double,double>((window.DataPoints[j+1].Value - window.DataPoints[j].Value)*window.SampleRate, window.DataPoints[j+1].Value)).ToList();
                        int dropIndex = dI.FindIndex(d => d.Item1 < -70);
                        int endIndex = dI.FindIndex(d => d.Item1 > 0 && d.Item2 < 0.2);
                        if (dropIndex < 0)
                        {
                            Idrop = Imax2;
                            Tdrop = fingerOpen;
                            Tend = fingerOpen;
                        }
                        else
                        {
                            Idrop = window[dropIndex].Value;
                            Tdrop = window[dropIndex].Time;
                            Tend = window[endIndex].Time;
                        } 
                    }

                    RelayPerformance result = new RelayPerformance()
                    {
                        ChannelID = channel.ID,
                        Imax1 = Imax1,
                        Imax2 = Imax2,
                        TripInitiate = tripInitiate,
                        PickupTime = (int)((latchOff - tripInitiate).TotalMilliseconds * GSF.Ticks.PerMillisecond),
                        TripTime = (int)((fingerOpen - tripInitiate).TotalMilliseconds * GSF.Ticks.PerMillisecond),
                        Tmax1 = (int)((Imax1Time - tripInitiate).TotalMilliseconds * GSF.Ticks.PerMillisecond),
                        PickupTimeCurrent = pickupCurrent,
                        TripTimeCurrent = Imax2,
                        TripCoilCondition = double.IsNaN(tripCoilCondition) ? (double?)null : tripCoilCondition,
                        TripCoilConditionTime = (int)((TTripCoilcondition - tripInitiate).TotalMilliseconds * GSF.Ticks.PerMillisecond),
                        TplungerLatch = (int)((plungerLatch - tripInitiate).TotalMilliseconds * GSF.Ticks.PerMillisecond),
                        IplungerLatch = IplungerLatch,
                        TiDrop = (int)((Tdrop - tripInitiate).TotalMilliseconds * GSF.Ticks.PerMillisecond),
                        Tend = (int)((Tend - tripInitiate).TotalMilliseconds * GSF.Ticks.PerMillisecond),
                        Idrop = Idrop
                    };

                    // look for Phase currents on that Breaker
                    
                    DataSeries Ia = meterDataSet.DataSeries.FirstOrDefault(
                        s => channel.Asset.DirectChannels.Select(ch => ch.ID).Contains(s.SeriesInfo.ChannelID) &&
                        IsPhaseCurrent("AN", s.SeriesInfo.Channel)
                        );
                    if (Ia == null)
                        Ia = meterDataSet.DataSeries.FirstOrDefault(
                            s => channel.Asset.ConnectedChannels.Select(ch => ch.ID).Contains(s.SeriesInfo.ChannelID) &&
                            IsPhaseCurrent("AN", s.SeriesInfo.Channel)
                            );

                    DataSeries Ib = meterDataSet.DataSeries.FirstOrDefault(
                            s => channel.Asset.DirectChannels.Select(ch => ch.ID).Contains(s.SeriesInfo.ChannelID) &&
                            IsPhaseCurrent("BN", s.SeriesInfo.Channel)
                            );
                    if (Ib == null)
                        Ib = meterDataSet.DataSeries.FirstOrDefault(
                            s => channel.Asset.ConnectedChannels.Select(ch => ch.ID).Contains(s.SeriesInfo.ChannelID) &&
                            IsPhaseCurrent("BN", s.SeriesInfo.Channel)
                            );

                    DataSeries Ic = meterDataSet.DataSeries.FirstOrDefault(
                            s => channel.Asset.DirectChannels.Select(ch => ch.ID).Contains(s.SeriesInfo.ChannelID) &&
                            IsPhaseCurrent("CN", s.SeriesInfo.Channel)
                            );
                    if (Ic == null)
                        Ic = meterDataSet.DataSeries.FirstOrDefault(
                            s => channel.Asset.ConnectedChannels.Select(ch => ch.ID).Contains(s.SeriesInfo.ChannelID) &&
                            IsPhaseCurrent("CN", s.SeriesInfo.Channel)
                            );

                    if (Ia != null)
                    {
                        Tuple<DateTime, double> phaseAnalysis = ProcessPhaseCurrents(Ia, tripInitiate, cycleDataResource);
                        if (!double.IsNaN(phaseAnalysis.Item2))
                        {
                            result.ExtinctionTimeA = (int)((phaseAnalysis.Item1 - tripInitiate).TotalMilliseconds * GSF.Ticks.PerMillisecond);
                            result.I2CA = phaseAnalysis.Item2;
                        }
                        else
                            Log.Info("Skipping A Phase analysis - RMS value too low.");
                    }
                    else
                        Log.Info("Skipping A Phase analysis - No Phase current found.");

                    if (Ib != null)
                    {
                        Tuple<DateTime, double> phaseAnalysis = ProcessPhaseCurrents(Ib, tripInitiate, cycleDataResource);
                        if (!double.IsNaN(phaseAnalysis.Item2))
                        {
                            result.ExtinctionTimeB = (int)((phaseAnalysis.Item1 - tripInitiate).TotalMilliseconds * GSF.Ticks.PerMillisecond);
                            result.I2CB = phaseAnalysis.Item2;
                        }
                        else
                            Log.Info("Skipping B Phase analysis - RMS value too low.");
                    }
                    else
                        Log.Info("Skipping B Phase analysis - No Phase current found.");

                    if (Ic != null)
                    {
                        Tuple<DateTime, double> phaseAnalysis = ProcessPhaseCurrents(Ic, tripInitiate, cycleDataResource);
                        if (!double.IsNaN(phaseAnalysis.Item2))
                        {
                            result.ExtinctionTimeC = (int)((phaseAnalysis.Item1 - tripInitiate).TotalMilliseconds * GSF.Ticks.PerMillisecond);
                            result.I2CC = phaseAnalysis.Item2;
                        }
                        else
                            Log.Info("Skipping C Phase analysis - RMS value too low.");
                    }
                    else
                        Log.Info("Skipping C Phase analysis - No Phase current found.");
                    
                    // Save Relay Characteristics
                    Log.Info("Saving Trip Coil Characteristics to DB.");

                    using (AdoDataConnection connection = meterDataSet.CreateDbConnection())
                    {
                        TableOperations<RelayPerformance> relayStatTable = new TableOperations<RelayPerformance>(connection);
                        Event evt = FindEvent(meterDataSet, channel);
                        if (evt == null)
                        {
                            Log.Error("Unable to identify Event");
                            continue;
                        }
                        result.EventID = evt.ID;
                        relayStatTable.AddNewRecord(result);
                    }
                    
                }
            }
        }

        private Tuple<DateTime,double> ProcessPhaseCurrents(DataSeries current, DateTime initialtrip, CycleDataResource cycleDataResource )
        {
            DataSeries Irms = cycleDataResource.VICycleDataGroups.SelectMany(item => item.CycleDataGroups).Where(cdg => cdg.RMS.SeriesInfo.ChannelID == current.SeriesInfo.ChannelID).FirstOrDefault()?.RMS;

            if (Irms == null)
                return new Tuple<DateTime, double>(initialtrip, double.NaN);

            DataSeries activeCurrent = current.ToSubSeries(initialtrip);
            double Imax = current.Maximum;
            double Imin = current.Minimum;
            DateTime Text;
            double I2C;

            if ((0.1 > Imax) || (-0.1 < Imin))
                return new Tuple<DateTime, double>(initialtrip,double.NaN);

            if (activeCurrent.Length < (0.5 * activeCurrent.SampleRate * 1.0 / 60.0))
                return new Tuple<DateTime, double>(initialtrip, double.NaN);

            if (Irms.Maximum < 40)
                return new Tuple<DateTime, double>(initialtrip, double.NaN);

            // Figure out Avg ABS(Current) before Trip
            double IpreTrip = Irms.ToSubSeries(0, current.Length - activeCurrent.Length).DataPoints.Average(p => Math.Abs(p.Value));

            // Threshold is what current should be at 1/32 of a cycle after zero crossing
            double Ithreshold = IpreTrip * Math.Sqrt(2) * Math.Sin(1.0 / 16.0 * Math.PI);

            //Find point where Current is below threshold for 1/16 of a cycle before and 1/16 of a cycle after.
            int l16 = (int)Math.Ceiling((1.0 / 16.0 * activeCurrent.SampleRate * 1.0 / 60.0));
            int i = l16;
            while (i < (activeCurrent.Length - l16))
            {
                DataSeries window = activeCurrent.ToSubSeries(i - l16, i + l16);
                if (window.Maximum < Ithreshold && window.Minimum > -Ithreshold)
                    break;
                i++;
            }

            Text = activeCurrent[i].Time;
            I2C = activeCurrent.DataPoints.Take(i).Sum(p => (p.Value*p.Value))*1.0/activeCurrent.SampleRate;

            return new Tuple<DateTime, double>(Text, I2C);
        }

        private bool IsPhaseCurrent(string phase, Channel channel)
        {
            return channel.MeasurementType.Name == "Current" &&
                   (channel.MeasurementCharacteristic.Name == "Instantaneous") &&
                   channel.Phase.Name == phase;
        }
        #region[ Static ]

        private static bool IsRelayEnergization(Channel channel)
        {
            return channel.MeasurementType.Name == "TripCoilCurrent" &&
                   (channel.MeasurementCharacteristic.Name == "Instantaneous");
        }

        private static double LeastSquare(DataSeries series)
        {
            // Assume that all points are uniformly sampled
                        
            // Calculate the Slope
            // Find the values S1, Sx, Sy, Sxx, and Sxy.
            double N = (double)series.Length;
            double Sy = series.DataPoints.Select(dataPoint => dataPoint.Value).Sum();
            double Sxx = 0;
            double Sx = 0;
            double Sxy = 0;

            for (int i = 0; i < N; i++)
            {
                Sxx += i * i;
                Sx += i;
                Sxy += series[i].Value * i;
            }

            double m = (Sxy * N - Sx * Sy) / (Sxx * N - Sx * Sx);

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

            if (dataGroups.Count == 0)
            {
                return null;
            }

            Event evt;

            const string Filter =
                    "FileGroupID = {0} AND " +
                    "AssetID = {1} AND " +
                    "StartTime = {2} AND " +
                    "EndTime = {3}";
            
            int fileGroupID = meterDataSet.FileGroup.ID;
            int lineID = dataGroups[0].Asset.ID;

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
