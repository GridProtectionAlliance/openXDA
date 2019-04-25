//******************************************************************************************************
//  Fault.cs - Gbtc
//
//  Copyright © 2015, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the MIT License (MIT), the "License"; you may
//  not use this file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://opensource.org/licenses/MIT
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  02/20/2015 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using FaultAlgorithms;

namespace FaultData.DataAnalysis
{
    public class Fault
    {
        #region [ Members ]

        // Nested Types

        public class Summary
        {
            #region [ Members ]

            // Fields
            private int m_distanceAlgorithmIndex;
            private string m_distanceAlgorithm;
            private double m_distance;

            private bool m_isSelectedAlgorithm;
            private bool m_isValid;

            #endregion

            #region [ Properties ]

            public int DistanceAlgorithmIndex
            {
                get
                {
                    return m_distanceAlgorithmIndex;
                }
                set
                {
                    m_distanceAlgorithmIndex = value;
                }
            }

            public string DistanceAlgorithm
            {
                get
                {
                    return m_distanceAlgorithm;
                }
                set
                {
                    m_distanceAlgorithm = value;
                }
            }

            public double Distance
            {
                get
                {
                    return m_distance;
                }
                set
                {
                    m_distance = value;
                }
            }

            public bool IsSelectedAlgorithm
            {
                get
                {
                    return m_isSelectedAlgorithm;
                }
                set
                {
                    m_isSelectedAlgorithm = value;
                }
            }

            public bool IsValid
            {
                get
                {
                    return m_isValid;
                }
                set
                {
                    m_isValid = value;
                }
            }

            #endregion
        }

        public class Segment
        {
            #region [ Members ]

            // Fields
            private FaultType m_faultType;
            private DateTime m_startTime;
            private DateTime m_endTime;
            private int m_startSample;
            private int m_endSample;

            #endregion

            #region [ Constructors ]

            public Segment(FaultType faultType)
            {
                m_faultType = faultType;
            }

            #endregion

            #region [ Properties ]

            public FaultType FaultType
            {
                get
                {
                    return m_faultType;
                }
            }

            public DateTime StartTime
            {
                get
                {
                    return m_startTime;
                }
                set
                {
                    m_startTime = value;
                }
            }

            public DateTime EndTime
            {
                get
                {
                    return m_endTime;
                }
                set
                {
                    m_endTime = value;
                }
            }

            public int StartSample
            {
                get
                {
                    return m_startSample;
                }
                set
                {
                    m_startSample = value;
                }
            }

            public int EndSample
            {
                get
                {
                    return m_endSample;
                }
                set
                {
                    m_endSample = value;
                }
            }

            #endregion
        }

        public class Curve
        {
            #region [ Members ]

            // Fields
            private int m_startIndex;
            private string m_algorithm;
            private DataSeries m_series;

            #endregion

            #region [ Constructors ]

            public Curve()
            {
                m_series = new DataSeries();
            }

            #endregion

            #region [ Properties ]

            public int StartIndex
            {
                get
                {
                    return m_startIndex;
                }
                set
                {
                    m_startIndex = value;
                }
            }

            public string Algorithm
            {
                get
                {
                    return m_algorithm;
                }
                set
                {
                    m_algorithm = value;
                }
            }

            public DataSeries Series
            {
                get
                {
                    return m_series;
                }
            }

            public DataPoint this[int index]
            {
                get
                {
                    return m_series[index - m_startIndex];
                }
            }

            public double Maximum
            {
                get
                {
                    return m_series.Maximum;
                }
            }

            public double Minimum
            {
                get
                {
                    return m_series.Minimum;
                }
            }

            public double Average
            {
                get
                {
                    return m_series.Average;
                }
            }

            public double StandardDeviation
            {
                get
                {
                    double average = Average;

                    double numerator = m_series.DataPoints
                        .Select(dataPoint => dataPoint.Value)
                        .Select(value => value - average)
                        .Select(diff => diff * diff)
                        .Sum();

                    int denominator = m_series.DataPoints.Count;

                    return Math.Sqrt(numerator / denominator);
                }
            }

            #endregion

            #region [ Methods ]

            public bool HasData(int index)
            {
                return (index >= m_startIndex) &&
                       (index < m_startIndex + m_series.DataPoints.Count);
            }

            #endregion
        }

        #endregion

        #region [ Constructors ]

        public Fault()
        {
            Summaries = new List<Summary>();
            Segments = new List<Segment>();
            Curves = new List<Curve>();
        }

        #endregion

        #region [ Properties ]

        public int CalculationCycle { get; set; }

        public FaultType Type { get; set; }

        public int StartSample { get; set; }
        public int EndSample { get; set; }
        public DateTime InceptionTime { get; set; }
        public DateTime ClearingTime { get; set; }
        public TimeSpan Duration { get; set; }

        public double CurrentMagnitude { get; set; }
        public double CurrentLag { get; set; }
        public double PrefaultCurrent { get; set; }
        public double PostfaultCurrent { get; set; }
        public double ReactanceRatio { get; set; }

        public double TreeFaultResistance { get; set; }
        public double LightningMilliseconds { get; set; }
        public double InceptionDistanceFromPeak { get; set; }
        public double PrefaultThirdHarmonic { get; set; }
        public double GroundCurrentRatio { get; set; }
        public double LowPrefaultCurrentRatio { get; set; }

        public bool IsSuppressed { get; set; }
        public bool IsReclose { get; set; }

        public List<Summary> Summaries { get; }
        public List<Segment> Segments { get; }
        public List<Curve> Curves { get; }

        #endregion

        #region [ Methods ]

        public Curve CreateCurve(string algorithm)
        {
            return new Curve()
            {
                Algorithm = algorithm,
                StartIndex = StartSample
            };
        }

        #endregion
    }
}
