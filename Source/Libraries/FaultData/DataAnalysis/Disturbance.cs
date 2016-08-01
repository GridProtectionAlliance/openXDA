//******************************************************************************************************
//  Disturbance.cs - Gbtc
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
//  08/10/2015 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using FaultData.DataResources;
using GSF.PQDIF.Logical;

namespace FaultData.DataAnalysis
{
    public class Disturbance
    {
        #region [ Members ]

        // Fields
        private EventClassification m_eventType;
        private Phase m_phase;
        private int m_startIndex;
        private int m_endIndex;
        private DateTime m_startTime;
        private DateTime m_endTime;
        private double m_magnitude;
        private double m_perUnitMagnitude;

        #endregion

        #region [ Properties ]

        public EventClassification EventType
        {
            get
            {
                return m_eventType;
            }
            set
            {
                m_eventType = value;
            }
        }

        public Phase Phase
        {
            get
            {
                return m_phase;
            }
            set
            {
                m_phase = value;
            }
        }

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

        public int EndIndex
        {
            get
            {
                return m_endIndex;
            }
            set
            {
                m_endIndex = value;
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

        public double DurationSeconds
        {
            get
            {
                return (m_endTime - m_startTime).TotalSeconds;
            }
        }

        public double Magnitude
        {
            get
            {
                return m_magnitude;
            }
            set
            {
                m_magnitude = value;
            }
        }

        public double PerUnitMagnitude
        {
            get
            {
                return m_perUnitMagnitude;
            }
            set
            {
                m_perUnitMagnitude = value;
            }
        }

        #endregion

        #region [ Methods ]

        public double GetDurationCycles(double systemFrequency)
        {
            return DurationSeconds * systemFrequency;
        }

        public double GetPerUnitMagnitude(double nominalValue)
        {
            return m_magnitude / nominalValue;
        }

        public Disturbance Clone()
        {
            return (Disturbance)MemberwiseClone();
        }

        #endregion
    }
}
