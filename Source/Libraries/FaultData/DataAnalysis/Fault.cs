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
using FaultAlgorithms;

namespace FaultData.DataAnalysis
{
    public class Fault
    {
        #region [ Members ]

        // Nested Types

        public class FaultInfo
        {
            #region [ Members ]

            // Fields
            private int m_calculationCycle;

            private FaultType m_type;

            private int m_startSample;
            private int m_endSample;
            private DateTime m_inceptionTime;
            private DateTime m_clearingTime;
            private TimeSpan m_duration;

            private int m_distanceAlgorithmIndex;
            private string m_distanceAlgorithm;
            private double m_distance;
            private double m_currentMagnitude;
            private int m_numberOfValidDistances;

            #endregion

            #region [ Properties ]

            public FaultType Type
            {
                get
                {
                    return m_type;
                }
                set
                {
                    m_type = value;
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

            public DateTime InceptionTime
            {
                get
                {
                    return m_inceptionTime;
                }
                set
                {
                    m_inceptionTime = value;
                }
            }

            public DateTime ClearingTime
            {
                get
                {
                    return m_clearingTime;
                }
                set
                {
                    m_clearingTime = value;
                }
            }

            public TimeSpan Duration
            {
                get
                {
                    return m_duration;
                }
                set
                {
                    m_duration = value;
                }
            }

            public int CalculationCycle
            {
                get
                {
                    return m_calculationCycle;
                }
                set
                {
                    m_calculationCycle = value;
                }
            }

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

            public double CurrentMagnitude
            {
                get
                {
                    return m_currentMagnitude;
                }
                set
                {
                    m_currentMagnitude = value;
                }
            }

            public int NumberOfValidDistances
            {
                get
                {
                    return m_numberOfValidDistances;
                }
                set
                {
                    m_numberOfValidDistances = value;
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

            public Curve(string algorithm)
            {
                m_algorithm = algorithm;
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

            #endregion

            #region [ Methods ]

            public bool HasData(int index)
            {
                return (index >= m_startIndex) &&
                       (index < m_startIndex + m_series.DataPoints.Count);
            }

            #endregion
        }

        // Fields
        private FaultInfo m_info;
        private List<Segment> m_segments;
        private List<Curve> m_curves;

        #endregion

        #region [ Constructors ]

        public Fault()
        {
            m_info = new FaultInfo();
            m_segments = new List<Segment>();
            m_curves = new List<Curve>();
        }

        #endregion

        #region [ Properties ]

        public FaultInfo Info
        {
            get
            {
                return m_info;
            }
        }

        public List<Segment> Segments
        {
            get
            {
                return m_segments;
            }
        }

        public List<Curve> Curves
        {
            get
            {
                return m_curves;
            }
        }

        #endregion
    }
}
