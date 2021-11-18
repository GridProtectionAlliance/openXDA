﻿//******************************************************************************************************
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
            DataGroup dataGroup = new DataGroup();

            DataSeries rmsSeries = new DataSeries();
            DataSeries phaseSeries = new DataSeries();
            DataSeries peakSeries = new DataSeries();
            DataSeries errorSeries = new DataSeries();

            int samplesPerCycle;
            double[] yValues;
            double[] tValues;
            double sum;
            double sinError;
            double prevSinError = 0.0D;
            double phase;

            DateTime cycleTime;
            SineWave sineFit;

            if ((object)dataSeries == null)
                return null;

            // Set series info to the source series info
            rmsSeries.SeriesInfo = dataSeries.SeriesInfo;
            phaseSeries.SeriesInfo = dataSeries.SeriesInfo;
            peakSeries.SeriesInfo = dataSeries.SeriesInfo;
            errorSeries.SeriesInfo = dataSeries.SeriesInfo;


            // Get samples per cycle of the data series based on the given frequency
            samplesPerCycle = Transform.CalculateSamplesPerCycle(dataSeries, frequency);

            //preinitialize size of SeriesInfo
            int ncycleData = dataSeries.DataPoints.Count - samplesPerCycle;
            rmsSeries.DataPoints = new List<DataPoint>(ncycleData);
            phaseSeries.DataPoints = new List<DataPoint>(ncycleData);
            peakSeries.DataPoints = new List<DataPoint>(ncycleData);
            errorSeries.DataPoints = new List<DataPoint>(ncycleData);



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
                yValues = dataSeries.DataPoints.Take(samplesPerCycle).Select(item => item.Value).ToArray();
                tValues = dataSeries.DataPoints.Take(samplesPerCycle).Select(item => (item.Time - dataSeries.DataPoints[0].Time).TotalSeconds).ToArray();
                sineFit = WaveFit.SineFit(yValues, tValues, frequency);
                sinError = tValues
                            .Select(sineFit.CalculateY)
                            .Zip(yValues, (estimate, value) => Math.Abs(estimate - value))
                            .Sum();
                phase = sineFit.Phase;

                rmsSeries.DataPoints.Add(new DataPoint()
                {
                    Time = dataSeries.DataPoints[0].Time,
                    Value = Math.Sqrt(sum / samplesPerCycle)
                });
                phaseSeries.DataPoints.Add(new DataPoint()
                {
                    Time = dataSeries.DataPoints[0].Time,
                    Value = phase
                });

                peakSeries.DataPoints.Add(new DataPoint()
                {
                    Time = dataSeries.DataPoints[0].Time,
                    Value = sineFit.Amplitude
                });

                errorSeries.DataPoints.Add(new DataPoint()
                {
                    Time = dataSeries.DataPoints[0].Time,
                    Value = sinError
                });

                prevSinError = sinError;
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

                    phase = phase + 2 * Math.PI * frequency * (dataSeries.DataPoints[i].Time - cycleTime).TotalSeconds;

                    // Use the time of the first data point in the cycle as the time of the cycle
                    cycleTime = dataSeries.DataPoints[i].Time;

                    yValues = dataSeries.DataPoints.Skip(i).Take(samplesPerCycle).Select(item => item.Value).ToArray();
                    tValues = dataSeries.DataPoints.Skip(i).Take(samplesPerCycle).Select(item => (item.Time - dataSeries.DataPoints[i].Time).TotalSeconds).ToArray();
                    sinError = tValues
                                .Select(sineFit.CalculateY)
                                .Zip(yValues, (estimate, value) => Math.Abs(estimate - value))
                                .Sum();


                    if (Math.Abs(prevSinError - sinError) > sinError * 0.0001 || !compress)
                    {
                        sineFit = WaveFit.SineFit(yValues, tValues, frequency);
                        sinError = tValues
                                    .Select(sineFit.CalculateY)
                                    .Zip(yValues, (estimate, value) => Math.Abs(estimate - value))
                                    .Sum();
                        phase = sineFit.Phase;
                    }

                    prevSinError = sinError;
                    //sum = 0.0D;

                    //remove Previous and add new one

                    // Copy values from the original data series into the y-value and t-value arrays
                    //for (int j = 0; j < samplesPerCycle; j++)
                    //{
                    //    yValues[j] = dataSeries.DataPoints[i + j].Value;
                    //    tValues[j] = (dataSeries.DataPoints[i + j].Time - cycleTime).TotalSeconds;
                    //    sum += yValues[j] * yValues[j];
                    //}

                    // Use a curve fitting algorithm to estimate the sine wave over this cycle
                    //sineFit = WaveFit.SineFit(yValues, tValues, frequency);

                    // Add data points to each of the cycle data series
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
                        Value = prevSinError
                    });


                }
            }

            // Add a series to the data group for each series of cycle data
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
