//******************************************************************************************************
//  BreakerDataResource.cs - Gbtc
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
//  07/19/2019 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using FaultData.Configuration;
using FaultData.DataAnalysis;
using FaultData.DataSets;
using GSF.Collections;
using GSF.Configuration;
using GSF.NumericalAnalysis;
using GSF.PQDIF.Logical;

namespace FaultData.DataResources
{
    public class BreakerDataResource : DataResourceBase<MeterDataSet>
    {
        #region [ Members ]

        // Nested Types
        public class Trip
        {
            public Trip(Phase phase, DateTime timestamp)
            {
                Phase = phase;
                Timestamp = timestamp;
            }

            public Phase Phase { get; }
            public DateTime Timestamp { get; }
        }

        public class Restrike
        {
            public Restrike()
            { }
            public Restrike(Phase phase, int initial )
            {
                Phase = phase;
                initialExtinction = initial;
                restrike = initial;
                currentMaximum = initial;
                maximumVoltageSurpression = initial;
                transientOverVoltage = initial;
                finalExtinction = initial;

            }

            public Phase Phase { get; }
            public int initialExtinction { get; }
            public int restrike { get; }
            public int currentMaximum { get; }
            public int maximumVoltageSurpression { get; }
            public int transientOverVoltage { get; }
            public int finalExtinction { get; }
        }

        #endregion

        #region [ Properties ]

        [Setting]
        public double SystemFrequency { get; set; }

        [Category]
        [SettingName(BreakerSettings.CategoryName)]
        public BreakerSettings BreakerSettings { get; } = new BreakerSettings();

        public Dictionary<DataGroup, List<Trip>> TripLookup { get; } = new Dictionary<DataGroup, List<Trip>>();

        public Dictionary<DataGroup, List<Restrike>> RestrikeLookup { get; } = new Dictionary<DataGroup, List<Restrike>>();

        #endregion

        #region [ Methods ]

        public override void Initialize(MeterDataSet meterDataSet)
        {
            CycleDataResource cycleDataResource = meterDataSet.GetResource<CycleDataResource>();
            FindAllTrips(cycleDataResource);
            FindAllRestrikes(cycleDataResource);
        }

        private void FindAllTrips(CycleDataResource cycleDataResource)
        {
            for (int i = 0; i < cycleDataResource.DataGroups.Count; i++)
            {
                DataGroup dataGroup = cycleDataResource.DataGroups[i];
                VICycleDataGroup viCycleDataGroup = cycleDataResource.VICycleDataGroups[i];

                IEnumerable<Trip> iaTrips = FindTrips(viCycleDataGroup.IA?.Peak)
                    .Select(point => new Trip(Phase.AN, point.Time));

                IEnumerable<Trip> ibTrips = FindTrips(viCycleDataGroup.IB?.Peak)
                    .Select(point => new Trip(Phase.BN, point.Time));

                IEnumerable<Trip> icTrips = FindTrips(viCycleDataGroup.IC?.Peak)
                    .Select(point => new Trip(Phase.CN, point.Time));

                List<Trip> allTrips = Enumerable.Empty<Trip>()
                    .Concat(iaTrips)
                    .Concat(ibTrips)
                    .Concat(icTrips)
                    .ToList();

                if (allTrips.Any())
                    TripLookup.Add(dataGroup, allTrips);
            }
        }

        private IEnumerable<DataPoint> FindTrips(DataSeries peak)
        {
            if ((object)peak == null)
                return new List<DataPoint>();

            double threshold = BreakerSettings.OpenBreakerThreshold;

            return peak.DataPoints
                .Zip(peak.DataPoints.Skip(1), (P1, P2) => new { P1, P2 })
                .Where(obj => obj.P1.Value > threshold && obj.P2.Value <= threshold)
                .Select(obj => obj.P2);
        }

        private void FindAllRestrikes(CycleDataResource cycleDataResource)
        {
            for (int i = 0; i < cycleDataResource.DataGroups.Count; i++)
            {
                DataGroup dataGroup = cycleDataResource.DataGroups[i];
                VICycleDataGroup viCycleDataGroup = cycleDataResource.VICycleDataGroups[i];
                VIDataGroup viDataGroup = cycleDataResource.VIDataGroups[i];
    

                IEnumerable<Restrike> iaRestrikes = FindRestrikes(viCycleDataGroup.IA,viDataGroup.IA )
                    .Select(index => new Restrike(Phase.AN, index));

                IEnumerable<Restrike> ibRestrikes = FindRestrikes(viCycleDataGroup.IB, viDataGroup.IB)
                    .Select(index => new Restrike(Phase.BN, index));

                IEnumerable<Restrike> icRestrikes = FindRestrikes(viCycleDataGroup.IC, viDataGroup.IC)
                    .Select(index => new Restrike(Phase.CN, index));

                List<Restrike> allRestrikes = Enumerable.Empty<Restrike>()
                    .Concat(iaRestrikes)
                    .Concat(ibRestrikes)
                    .Concat(icRestrikes)
                    .ToList();

                if (allRestrikes.Any())
                    RestrikeLookup.Add(dataGroup, allRestrikes);
            }
        }

        private List<int> FindRestrikes(CycleDataGroup cycleData, DataSeries pointOnWaveData)
        {
            List<int> restrikes = new List<int>();

            if (cycleData == null || pointOnWaveData == null)
                return restrikes;

           
            int fullCycle = (int)Math.Ceiling(pointOnWaveData.SampleRate / SystemFrequency);
            // This needs to be tuned
            double deltaThreshhold = 8.5;

            // Step 1 -> Find changes in full cycle Peak as potential candidates for Breaker Changes.
            List<int> potentialPoints = new List<int>();

            for (int i = 1; i < cycleData.Peak.DataPoints.Count; i++)
            {
                DataPoint dataPoint = cycleData.Peak[i];
                
                bool deltaCloseToZero = Math.Abs(dataPoint.Value - cycleData.Peak[i-1].Value) < deltaThreshhold;

                if (!deltaCloseToZero)
                    potentialPoints.Add(i);
            }

            // Combine Potential Segments if they overlap...
            List<Tuple<int,int>> segments = new List<Tuple<int, int>>();
            foreach(int seg in potentialPoints)
            {
                if (segments.Count == 0)
                {
                    segments.Add(new Tuple<int, int>(seg, seg + fullCycle));
                    continue;
                }
                if (seg <= segments.Last().Item2)
                {
                    segments[segments.Count - 1] = new Tuple<int, int>(segments[segments.Count - 1].Item1, seg + fullCycle);

                }
                else
                    segments.Add(new Tuple<int, int>(seg, seg + fullCycle));
            }

            // Step 2 -> Narrow down based on 20 point peak data
            potentialPoints = new List<int>();
            bool closeToZero;

            
            for (int i =0; i < segments.Count(); i++)
            {
                closeToZero = true;
               
                
                for (int j = segments[i].Item1; j < (segments[i].Item2 - 20); j++)
                {
                    double amplitude = GetAmplitude(pointOnWaveData.ToSubSeries(j, j + 20).DataPoints);

                    if (j == segments[i].Item1)
                    {
                        closeToZero = amplitude < BreakerSettings.OpenBreakerThreshold;
                        continue;
                    }
                    if (closeToZero && (amplitude > BreakerSettings.OpenBreakerThreshold))
                        potentialPoints.Add(j);
                    
                    else if(!closeToZero && (amplitude < BreakerSettings.OpenBreakerThreshold))
                        potentialPoints.Add(j);

                    closeToZero = amplitude < BreakerSettings.OpenBreakerThreshold;
                    
                }
            }


            // Combine Potential Segments if they overlap and go 10 points in each direction (for combining)
            segments = new List<Tuple<int, int>>();
            foreach (int seg in potentialPoints)
            {
                if (segments.Count == 0)
                {
                    segments.Add(new Tuple<int, int>(seg -10, seg + 30));
                    continue;
                }
                if (seg <= segments.Last().Item2)
                {
                    segments[segments.Count - 1] = new Tuple<int, int>(segments[segments.Count - 1].Item1, seg + 30);

                }
                else
                    segments.Add(new Tuple<int, int>(seg - 10, seg + 30));
            }


            List<bool> breakerOpening = new List<bool>();
            List<bool> isValid = new List<bool>();

            //Step 3 -> Remove based on PoW average Distance to Mean across Segments
            //Step 4 -> Attempt to determine if it is breaker open or breaker close based avg across 3 points.

            for (int i = 0; i < segments.Count(); i++)
            {
                int startIndex = segments[i].Item1;
                if (startIndex < 0)
                    startIndex = 0;
                int endIndex = segments[i].Item2;

                double avgStart = pointOnWaveData.ToSubSeries(startIndex, startIndex + 3).DataPoints.Select(item => item.Value).Average(); 
                double avgEnd = pointOnWaveData.ToSubSeries(endIndex - 3, endIndex).DataPoints.Select(item => item.Value).Average();
                double avg = pointOnWaveData.ToSubSeries(startIndex, endIndex).DataPoints.Select(item => item.Value).Average();
                double avdDists = pointOnWaveData.ToSubSeries(startIndex, endIndex).DataPoints.Select(item => Math.Abs(item.Value - avg)).Average();


                startIndex = segments[i].Item2;

                double amplitudeEnd = GetAmplitude(pointOnWaveData.ToSubSeries(startIndex, startIndex + 20).DataPoints);

                if (avdDists < BreakerSettings.OpenBreakerThreshold)
                    isValid.Add(false);
                else
                    isValid.Add(true);

                if (avgEnd < avgStart)
                    breakerOpening.Add(true);
                else
                    breakerOpening.Add(false);
            }

            segments = segments.Where((item, index) => isValid[index]).ToList();
            breakerOpening = breakerOpening.Where((item, index) => isValid[index]).ToList();


            if (segments.Count < 2)
                return restrikes;

            // Remove potential issues where Breaker opening is not possible (Breaker already open)
            isValid = new List<bool>();
            isValid.Add(true);
            for (int i = 1; i < segments.Count(); i++)
            {
                isValid.Add(true);
                if (breakerOpening[i] == breakerOpening[i - 1])
                {
                    int startIndex = segments[i].Item1;
                    if (startIndex < 0)
                        startIndex = 0;

                    int endIndex = segments[i].Item2;
                    double avgStart = pointOnWaveData.ToSubSeries(startIndex, startIndex + 3).DataPoints.Select(item => item.Value).Average();
                    double avgEnd = pointOnWaveData.ToSubSeries(endIndex - 3, endIndex).DataPoints.Select(item => item.Value).Average();

                    double diff1 = Math.Abs(avgStart - avgEnd);

                    startIndex = segments[i-1].Item1;
                    if (startIndex < 0)
                        startIndex = 0;

                    endIndex = segments[i-1].Item2;
                    avgStart = pointOnWaveData.ToSubSeries(startIndex, startIndex + 3).DataPoints.Select(item => item.Value).Average();
                    avgEnd = pointOnWaveData.ToSubSeries(endIndex - 3, endIndex).DataPoints.Select(item => item.Value).Average();

                    double diff2 = Math.Abs(avgStart - avgEnd);

                    if (Math.Abs(diff1) > Math.Abs(diff2))
                        isValid[i - 1] = false;
                    else
                        isValid[i] = false;
                }

            }

            segments = segments.Where((item, index) => isValid[index]).ToList();
            breakerOpening = breakerOpening.Where((item, index) => isValid[index]).ToList();

            // Check if there are Breaker close movements that are restrikes
            //restrikes = segments.Where((item, index) => (index > 0) && (!breakerOpening[index])).Select(item => item.Item1).ToList();
            restrikes = segments.Select(item => item.Item1).ToList();

            //Determine excact point based on POW deviation (going forward for Breaker closing)


            return restrikes;
        }
        
        private double GetAmplitude(List<DataPoint> dataPoints)
        {

            if (dataPoints.Count < 15)
                return double.NaN;

            // Use the time of the first data point in the cycle as the time of the cycle
            double sum = 0.0D;
            double[] yValues = new double[dataPoints.Count];
            double[] tValues = new double[dataPoints.Count];
            // Copy values from the original data series into the y-value and t-value arrays
            for (int i = 0; i < dataPoints.Count; i++)
            {
                yValues[i] = dataPoints[i].Value;
                tValues[i] = (dataPoints[i].Time - dataPoints[0].Time).TotalSeconds;
                sum += yValues[i] * yValues[i];
            }

            // Use a curve fitting algorithm to estimate the sine wave over this cycle
            SineWave sineFit = WaveFit.SineFit(yValues, tValues, SystemFrequency);

            // Use a curve fitting algorithm to estimate the sine wave 
            return Math.Abs(sineFit.Amplitude);
        }
        #endregion
    }
}
