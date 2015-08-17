//******************************************************************************************************
//  DataSeries.cs - Gbtc
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
//  05/15/2014 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using FaultData.Database;

namespace FaultData.DataAnalysis
{
    /// <summary>
    /// Represents a series of data points.
    /// </summary>
    public class DataSeries
    {
        #region [ Members ]

        // Fields
        private Series m_seriesInfo;
        private List<DataPoint> m_dataPoints;

        private double? m_duration;
        private double? m_sampleRate;
        private double? m_minimum;
        private double? m_maximum;
        private double? m_average;

        #endregion

        #region [ Constructors ]

        public DataSeries()
        {
            m_dataPoints = new List<DataPoint>();
        }

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets or sets the configuration information
        /// that defines the data in this series.
        /// </summary>
        public Series SeriesInfo
        {
            get
            {
                return m_seriesInfo;
            }
            set
            {
                m_seriesInfo = value;
            }
        }

        /// <summary>
        /// Gets or sets the data points that make up the series.
        /// </summary>
        public List<DataPoint> DataPoints
        {
            get
            {
                return m_dataPoints;
            }
            set
            {
                m_dataPoints = value ?? new List<DataPoint>();
                m_duration = null;
                m_sampleRate = null;
                m_minimum = null;
                m_maximum = null;
                m_average = null;
            }
        }

        /// <summary>
        /// Gets the duration of the series, in seconds.
        /// </summary>
        public double Duration
        {
            get
            {
                if (m_duration.HasValue)
                    return m_duration.Value;

                if (!m_dataPoints.Any())
                    return double.NaN;

                m_duration = m_dataPoints.Last().Time.Subtract(m_dataPoints.First().Time).TotalSeconds;

                return m_duration.Value;
            }
        }

        /// <summary>
        /// Gets the sample rate of the series, in samples per second.
        /// </summary>
        public double SampleRate
        {
            get
            {
                if (m_sampleRate.HasValue)
                    return m_sampleRate.Value;

                if (!m_dataPoints.Any())
                    return double.NaN;

                m_sampleRate = (Duration != 0.0D) ? m_dataPoints.Count / Duration : double.NaN;

                return m_sampleRate.Value;
            }
        }

        /// <summary>
        /// Gets the maximum value in the series.
        /// </summary>
        public double Maximum
        {
            get
            {
                if (m_maximum.HasValue)
                    return m_maximum.Value;

                if (!m_dataPoints.Any())
                    return double.NaN;

                m_maximum = m_dataPoints.Max(point => point.Value);

                return m_maximum.Value;
            }
        }

        /// <summary>
        /// Gets the minimum value in the series.
        /// </summary>
        public double Minimum
        {
            get
            {
                if (m_minimum.HasValue)
                    return m_minimum.Value;

                if (!m_dataPoints.Any())
                    return double.NaN;

                m_minimum = m_dataPoints.Min(point => point.Value);

                return m_minimum.Value;
            }
        }

        /// <summary>
        /// Gets the average value in the series.
        /// </summary>
        public double Average
        {
            get
            {
                if (m_average.HasValue)
                    return m_average.Value;

                if (!m_dataPoints.Any())
                    return double.NaN;

                m_average = m_dataPoints.Average(point => point.Value);

                return m_average.Value;
            }
        }

        public DataPoint this[int index]
        {
            get
            {
                return m_dataPoints[index];
            }
        }

        #endregion

        #region [ Methods ]

        public DataSeries ToSubSeries(int startIndex, int endIndex)
        {
            DataSeries subSeries = new DataSeries();
            int count;

            subSeries.SeriesInfo = m_seriesInfo;

            if (startIndex < 0)
                startIndex = 0;

            if (endIndex >= m_dataPoints.Count)
                endIndex = m_dataPoints.Count - 1;

            count = endIndex - startIndex + 1;

            if (count > 0)
                subSeries.DataPoints = m_dataPoints.Skip(startIndex).Take(count).ToList();

            return subSeries;
        }

        public DataSeries Negate()
        {
            DataSeries negatedDataSeries = new DataSeries();

            negatedDataSeries.DataPoints = m_dataPoints
                .Select(point => point.Negate())
                .ToList();

            return negatedDataSeries;
        }

        public DataSeries Add(DataSeries operand)
        {
            DataSeries sum = new DataSeries();

            if (m_dataPoints.Count != operand.DataPoints.Count)
                throw new InvalidOperationException("Cannot take the sum of series with mismatched time values");

            sum.DataPoints = m_dataPoints
                .Zip(operand.DataPoints, Add)
                .ToList();

            return sum;
        }

        public DataSeries Subtract(DataSeries operand)
        {
            return Add(operand.Negate());
        }

        public DataSeries Multiply(double value)
        {
            DataSeries result = new DataSeries();

            result.DataPoints = m_dataPoints
                .Select(point => point.Multiply(value))
                .ToList();

            return result;
        }

        public DataSeries Copy()
        {
            return Multiply(1.0D);
        }

        #endregion

        #region [ Static ]

        // Static Methods

        private static DataPoint Add(DataPoint point1, DataPoint point2)
        {
            return point1.Add(point2);
        }

        #endregion
    }
}
