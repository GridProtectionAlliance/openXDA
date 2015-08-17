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

        public static VICycleDataGroup ToVICycleDataGroup(VIDataGroup dataGroup, double frequency)
        {
            return new VICycleDataGroup(dataGroup.ToDataGroup().DataSeries
                .Select(dataSeries => ToCycleDataGroup(dataSeries, frequency))
                .ToList());
        }

        public static CycleDataGroup ToCycleDataGroup(DataSeries dataSeries, double frequency)
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

            DateTime cycleTime;
            SineWave sineFit;

            if ((object)dataSeries == null)
                return new CycleDataGroup(dataGroup);

            // Set series info to the source series info
            rmsSeries.SeriesInfo = dataSeries.SeriesInfo;
            phaseSeries.SeriesInfo = dataSeries.SeriesInfo;
            peakSeries.SeriesInfo = dataSeries.SeriesInfo;
            errorSeries.SeriesInfo = dataSeries.SeriesInfo;

            // Get samples per cycle of the data series based on the given frequency
            samplesPerCycle = CalculateSamplesPerCycle(dataSeries, frequency);

            // Initialize arrays of y-values and t-values for calculating cycle data
            yValues = new double[samplesPerCycle];
            tValues = new double[samplesPerCycle];

            for (int i = 0; i <= dataSeries.DataPoints.Count - samplesPerCycle; i++)
            {
                // Use the time of the first data point in the cycle as the time of the cycle
                cycleTime = dataSeries.DataPoints[i].Time;
                sum = 0.0D;

                // Copy values from the original data series into the y-value and t-value arrays
                for (int j = 0; j < samplesPerCycle; j++)
                {
                    yValues[j] = dataSeries.DataPoints[i + j].Value;
                    tValues[j] = (dataSeries.DataPoints[i + j].Time - cycleTime).TotalSeconds;
                    sum += yValues[j] * yValues[j];
                }

                // Use a curve fitting algorithm to estimate the sine wave over this cycle
                sineFit = WaveFit.SineFit(yValues, tValues, frequency);

                // Add data points to each of the cycle data series
                rmsSeries.DataPoints.Add(new DataPoint()
                {
                    Time = cycleTime,
                    Value = Math.Sqrt(sum / samplesPerCycle)
                });

                phaseSeries.DataPoints.Add(new DataPoint()
                {
                    Time = cycleTime,
                    Value = sineFit.Phase
                });

                peakSeries.DataPoints.Add(new DataPoint()
                {
                    Time = cycleTime,
                    Value = sineFit.Amplitude
                });

                errorSeries.DataPoints.Add(new DataPoint()
                {
                    Time = cycleTime,

                    Value = tValues
                        .Select(sineFit.CalculateY)
                        .Zip(yValues, (estimate, value) => Math.Abs(estimate - value))
                        .Sum()
                });
            }

            // Add a series to the data group for each series of cycle data
            dataGroup.Add(rmsSeries);
            dataGroup.Add(phaseSeries);
            dataGroup.Add(peakSeries);
            dataGroup.Add(errorSeries);

            return new CycleDataGroup(dataGroup);
        }

        public static List<double> ToValues(DataSeries series)
        {
            return series.DataPoints
                .Select(dataPoint => dataPoint.Value)
                .ToList();
        }

        private static int CalculateSamplesPerCycle(DataSeries dataSeries, double frequency)
        {
            return (int)Math.Round(dataSeries.SampleRate / frequency);
        }
    }
}
