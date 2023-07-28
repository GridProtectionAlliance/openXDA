//******************************************************************************************************
//  Transform.cs - Gbtc
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
//  08/28/2014 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using GSF.Collections;
using GSF.NumericalAnalysis;

namespace FaultData.DataAnalysis
{
    public static class Transform
    {
        public static DataGroup Combine(params DataGroup[] dataGroups)
        {
            DataGroup combination = new DataGroup();

            foreach (DataGroup dataGroup in dataGroups)
            {
                foreach (DataSeries dataSeries in dataGroup.DataSeries)
                    combination.Add(dataSeries);
            }

            return combination;
        }

        public static VICycleDataGroup ToVICycleDataGroup(VIDataGroup dataGroup, double frequency, bool compress = false)
        {
            DataSeries[] cycleSeries = dataGroup.Data;
            
            return new VICycleDataGroup(cycleSeries
                .Where(dataSeries => (object)dataSeries != null)
                .Select(dataSeries =>  ToCycleDataGroup(dataSeries, frequency, compress))            
                .ToList(), dataGroup.Asset);
        }

        public static CycleDataGroup ToCycleDataGroup(DataSeries dataSeries, double frequency, bool compress=false)
        {
            if (dataSeries is null)
                return null;

            DataSeries rmsSeries = new DataSeries();
            DataSeries phaseSeries = new DataSeries();
            DataSeries peakSeries = new DataSeries();
            DataSeries errorSeries = new DataSeries();

            // Set series info to the source series info
            rmsSeries.SeriesInfo = dataSeries.SeriesInfo;
            phaseSeries.SeriesInfo = dataSeries.SeriesInfo;
            peakSeries.SeriesInfo = dataSeries.SeriesInfo;
            errorSeries.SeriesInfo = dataSeries.SeriesInfo;

            // Get samples per cycle of the data series based on the given frequency
            int samplesPerCycle = CalculateSamplesPerCycle(dataSeries, frequency);

            //preinitialize size of SeriesInfo
            int ncycleData = dataSeries.DataPoints.Count - samplesPerCycle + 1;

            if (ncycleData <= 0)
                return null;

            rmsSeries.DataPoints.Capacity = ncycleData;
            phaseSeries.DataPoints.Capacity = ncycleData;
            peakSeries.DataPoints.Capacity = ncycleData;
            errorSeries.DataPoints.Capacity = ncycleData;

            // Initialize arrays of y-values and t-values for calculating cycle data as necessary
            double[] yValues = new double[samplesPerCycle];
            double[] tValues = new double[samplesPerCycle];

            void CaptureCycle(int cycleIndex)
            {
                DateTime startTime = dataSeries.DataPoints[0].Time;

                for (int i = 0; i < samplesPerCycle; i++)
                {
                    DateTime time = dataSeries.DataPoints[cycleIndex + i].Time;
                    double value = dataSeries.DataPoints[cycleIndex + i].Value;
                    tValues[i] = time.Subtract(startTime).TotalSeconds;
                    yValues[i] = value;
                }
            }

            // Obtain a list of time gaps in the data series
            List<int> gapIndexes = Enumerable.Range(0, dataSeries.DataPoints.Count - 1)
                .Where(index =>
                {
                    DataPoint p1 = dataSeries[index];
                    DataPoint p2 = dataSeries[index + 1];
                    double cycleDiff = (p2.Time - p1.Time).TotalSeconds * frequency;

                    // Detect gaps larger than a quarter cycle.
                    // Tolerance of 0.000062 calculated
                    // assuming 3.999 samples per cycle
                    return (cycleDiff > 0.250062);
                })
                .ToList();

            double sum = 0;

            if (dataSeries.DataPoints.Count >= samplesPerCycle)
            {
                CaptureCycle(0);
                sum = yValues.Sum(y => y * y);

                DateTime cycleTime = dataSeries.DataPoints[0].Time;
                SineWave sineFit = WaveFit.SineFit(yValues, tValues, frequency);
                double phase = sineFit.Phase;

                double ComputeSineError() => tValues
                    .Select(sineFit.CalculateY)
                    .Zip(yValues, (estimate, value) => Math.Abs(estimate - value))
                    .Sum();

                double sineError = ComputeSineError();
                double previousSineError = sineError;

                rmsSeries.DataPoints.Add(new DataPoint()
                {
                    Time = cycleTime,
                    Value = Math.Sqrt(sum / samplesPerCycle)
                });

                phaseSeries.DataPoints.Add(new DataPoint()
                {
                    Time = cycleTime,
                    Value = phase
                });

                peakSeries.DataPoints.Add(new DataPoint()
                {
                    Time = cycleTime,
                    Value = sineFit.Amplitude
                });

                errorSeries.DataPoints.Add(new DataPoint()
                {
                    Time = cycleTime,
                    Value = sineError
                });

                // Reduce RMS to max 2 pt per cycle to get half cycle RMS
                int step = 1;
                if (compress)
                    step = (int)Math.Floor(samplesPerCycle / 2.0D);
                if (step == 0)
                    step = 1;

                for (int cycleIndex = step; cycleIndex < dataSeries.DataPoints.Count - samplesPerCycle + 1; cycleIndex += step)
                {
                    for (int j = 0; j < step; j++)
                    {
                        int oldIndex = cycleIndex - step + j;
                        int newIndex = oldIndex + samplesPerCycle;
                        double oldValue = dataSeries.DataPoints[oldIndex].Value;
                        double newValue = dataSeries.DataPoints[newIndex].Value;
                        sum += newValue * newValue - oldValue * oldValue;
                    }

                    // If the cycle following i contains a data gap, do not calculate cycle data
                    if (gapIndexes.Any(index => cycleIndex <= index && (cycleIndex + samplesPerCycle - 1) > index))
                        continue;

                    phase += 2 * Math.PI * frequency * (dataSeries.DataPoints[cycleIndex].Time - cycleTime).TotalSeconds;

                    // Use the time of the first data point in the cycle as the time of the cycle
                    cycleTime = dataSeries.DataPoints[cycleIndex].Time;

                    CaptureCycle(cycleIndex);

                    if (compress)
                        sineError = ComputeSineError();

                    if (!compress || Math.Abs(previousSineError - sineError) > sineError * 0.0001)
                    {
                        sineFit = WaveFit.SineFit(yValues, tValues, frequency);
                        phase = sineFit.Phase;
                        sineError = ComputeSineError();
                    }

                    previousSineError = sineError;

                    rmsSeries.DataPoints.Add(new DataPoint()
                    {
                        Time = cycleTime,
                        Value = Math.Sqrt(sum / samplesPerCycle)
                    });

                    phaseSeries.DataPoints.Add(new DataPoint()
                    {
                        Time = cycleTime,
                        Value = phase
                    });

                    peakSeries.DataPoints.Add(new DataPoint()
                    {
                        Time = cycleTime,
                        Value = sineFit.Amplitude
                    });

                    errorSeries.DataPoints.Add(new DataPoint()
                    {
                        Time = cycleTime,
                        Value = sineError
                    });
                }
            }

            // Add a series to the data group for each series of cycle data
            DataGroup dataGroup = new DataGroup();
            dataGroup.Add(rmsSeries);
            dataGroup.Add(phaseSeries);
            dataGroup.Add(peakSeries);
            dataGroup.Add(errorSeries);

            return new CycleDataGroup(dataGroup, dataSeries.SeriesInfo.Channel.Asset);
        }

        public static DataSeries ToRMS(DataSeries dataSeries, double frequency, bool compress = false)
        {
            DataSeries rmsSeries = new DataSeries();

            int samplesPerCycle;
            double[] yValues;
            double[] tValues;
            double sum;

            DateTime cycleTime;

            if ((object)dataSeries == null)
                return null;

            // Set series info to the source series info
            rmsSeries.SeriesInfo = dataSeries.SeriesInfo;


            // Get samples per cycle of the data series based on the given frequency
            samplesPerCycle = Transform.CalculateSamplesPerCycle(dataSeries, frequency);

            //preinitialize size of SeriesInfo
            int ncycleData = dataSeries.DataPoints.Count - samplesPerCycle;
            rmsSeries.DataPoints = new List<DataPoint>(ncycleData);



            // Initialize arrays of y-values and t-values for calculating cycle data as necessary
            yValues = new double[samplesPerCycle];
            tValues = new double[samplesPerCycle];

            // Obtain a list of time gaps in the data series
            List<int> gapIndexes = Enumerable.Range(0, dataSeries.DataPoints.Count - 1)
                .Where(index =>
                {
                    DataPoint p1 = dataSeries[index];
                    DataPoint p2 = dataSeries[index + 1];
                    double cycleDiff = (p2.Time - p1.Time).TotalSeconds * frequency;

                    // Detect gaps larger than a quarter cycle.
                    // Tolerance of 0.000062 calculated
                    // assuming 3.999 samples per cycle
                    return (cycleDiff > 0.250062);
                })
                .ToList();

            sum = 0;

            if (dataSeries.DataPoints.Count > samplesPerCycle)
            {
                sum = dataSeries.DataPoints.Take(samplesPerCycle).Sum(pt => pt.Value * pt.Value);

                rmsSeries.DataPoints.Add(new DataPoint()
                {
                    Time = dataSeries.DataPoints[0].Time,
                    Value = Math.Sqrt(sum / samplesPerCycle)
                });

                cycleTime = dataSeries.DataPoints[0].Time;

                // Reduce RMS to max 2 pt per cycle to get half cycle RMS
                int step = 1;
                if (compress)
                    step = (int)Math.Floor(samplesPerCycle / 2.0D);
                if (step == 0)
                    step = 1;

                for (int i = step; i < dataSeries.DataPoints.Count - samplesPerCycle; i = i + step)
                {

                    for (int j = 0; j < step; j++)
                    {
                        sum = sum - dataSeries.DataPoints[i - step + j].Value * dataSeries.DataPoints[i - step + j].Value;
                        sum = sum + dataSeries.DataPoints[i - step + j + samplesPerCycle].Value * dataSeries.DataPoints[i - step + j + samplesPerCycle].Value;
                    }

                    // If the cycle following i contains a data gap, do not calculate cycle data
                    if (gapIndexes.Any(index => i <= index && (i + samplesPerCycle - 1) > index))
                        continue;

                    // Use the time of the first data point in the cycle as the time of the cycle
                    cycleTime = dataSeries.DataPoints[i].Time;

                    rmsSeries.DataPoints.Add(new DataPoint()
                    {
                        Time = cycleTime,
                        Value = Math.Sqrt(sum / samplesPerCycle)
                    });

                }
            }

            return rmsSeries;
        }

        public static List<double> ToValues(DataSeries series)
        {
            return series.DataPoints
                .Select(dataPoint => dataPoint.Value)
                .ToList();
        }

        public static int CalculateSamplesPerCycle(DataSeries dataSeries, double frequency)
        {
            return CalculateSamplesPerCycle(dataSeries.SampleRate, frequency);
        }

        public static int CalculateSamplesPerCycle(double samplesPerSecond, double frequency)
        {
            int[] commonSampleRates =
            {
                4, 8, 16, 32,
                80, 96, 100, 200,
                64, 128, 256, 512, 1024
            };

            int calculatedRate = (int)Math.Round(samplesPerSecond / frequency);
            int nearestCommonRate = commonSampleRates.MinBy(rate => Math.Abs(calculatedRate - rate));
            int diff = Math.Abs(calculatedRate - nearestCommonRate);
            return (diff < nearestCommonRate * 0.1D) ? nearestCommonRate : calculatedRate;
        }
    }
}
