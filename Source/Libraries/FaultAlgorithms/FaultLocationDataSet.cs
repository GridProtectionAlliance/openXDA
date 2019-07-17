//*********************************************************************************************************************
// FaultLocationDataSet.cs
// Version 1.1 and subsequent releases
//
//  Copyright © 2013, Grid Protection Alliance.  All Rights Reserved.
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
// --------------------------------------------------------------------------------------------------------------------
//
// Version 1.0
//
// Copyright 2012 ELECTRIC POWER RESEARCH INSTITUTE, INC. All rights reserved.
//
// openFLE ("this software") is licensed under BSD 3-Clause license.
//
// Redistribution and use in source and binary forms, with or without modification, are permitted provided that the 
// following conditions are met:
//
// •    Redistributions of source code must retain the above copyright  notice, this list of conditions and 
//      the following disclaimer.
//
// •    Redistributions in binary form must reproduce the above copyright notice, this list of conditions and 
//      the following disclaimer in the documentation and/or other materials provided with the distribution.
//
// •    Neither the name of the Electric Power Research Institute, Inc. (“EPRI”) nor the names of its contributors 
//      may be used to endorse or promote products derived from this software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, 
// INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE 
// DISCLAIMED. IN NO EVENT SHALL EPRI BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL 
// DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; 
// OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, 
// OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
// POSSIBILITY OF SUCH DAMAGE.
//
//
// This software incorporates work covered by the following copyright and permission notice: 
//
// •    TVA Code Library 4.0.4.3 - Tennessee Valley Authority, tvainfo@tva.gov
//      No copyright is claimed pursuant to 17 USC § 105. All Other Rights Reserved.
//
//      Licensed under TVA Custom License based on NASA Open Source Agreement (TVA Custom NOSA); 
//      you may not use TVA Code Library except in compliance with the TVA Custom NOSA. You may  
//      obtain a copy of the TVA Custom NOSA at http://tvacodelibrary.codeplex.com/license.
//
//      TVA Code Library is provided by the copyright holders and contributors "as is" and any express 
//      or implied warranties, including, but not limited to, the implied warranties of merchantability 
//      and fitness for a particular purpose are disclaimed.
//
//*********************************************************************************************************************
//
//  Code Modification History:
//  -------------------------------------------------------------------------------------------------------------------
//  06/19/2012 - Stephen C. Wills, Grid Protection Alliance
//       Generated original version of source code.
//
//*********************************************************************************************************************

using System.Collections.Generic;
using GSF;

namespace FaultAlgorithms
{
    /// <summary>
    /// Represents possible types of a fault.
    /// </summary>
    public enum FaultType
    {
        /// <summary>
        /// A-phase to neutral fault type.
        /// </summary>
        AN = 1,

        /// <summary>
        /// B-phase to neutral fault type.
        /// </summary>
        BN = 2,
        
        /// <summary>
        /// C-phase to neutral fault type.
        /// </summary>
        CN = 3,

        /// <summary>
        /// A-phase to B-phase fault type.
        /// </summary>
        AB = 4,

        /// <summary>
        /// B-phase to C-phase fault type.
        /// </summary>
        BC = 5,

        /// <summary>
        /// C-phase to A-phase fault type.
        /// </summary>
        CA = 6,

        /// <summary>
        /// A-phase to B-phase to ground fault type.
        /// </summary>
        ABG = 7,
        
        /// <summary>
        /// B-phase to C-phase to ground fault type.
        /// </summary>
        BCG = 8,

        /// <summary>
        /// C-phase to A-phase to ground fault type.
        /// </summary>
        CAG = 9,
        
        /// <summary>
        /// Three-phase fault type.
        /// </summary>
        ABC = 10,

        /// <summary>
        /// Three-phase with ground fault type.
        /// </summary>
        ABCG = 11,

        /// <summary>
        /// No fault.
        /// </summary>
        None = 0
    }

    /// <summary>
    /// Represents a data set containing data about an event and the
    /// fault conditions that were detected when analyzing the data set.
    /// </summary>
    public class FaultLocationDataSet
    {
        #region [ Members ]

        // Fields
        private MeasurementDataSet m_voltages;
        private MeasurementDataSet m_currents;
        private CycleDataSet m_cycles;
        private CycleData m_prefaultCycle;

        private double m_frequency;
        private ComplexNumber m_positiveImpedance;
        private ComplexNumber m_zeroImpedance;
        private ComplexNumber m_localSourceImpedance;
        private ComplexNumber m_remoteSourceImpedance;

        private double m_ratedCurrent;
        private FaultType m_faultType;
        private IList<int> m_faultedCycles;
        private int m_faultCalculationCycle;
        private double m_lineDistance;
        private double m_faultDistance;
        private Dictionary<string, double[]> m_faultDistances;

        private Dictionary<string, object> m_values;

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Creates a new instance of the <see cref="FaultLocationDataSet"/> class.
        /// </summary>
        public FaultLocationDataSet()
        {
            m_voltages = new MeasurementDataSet();
            m_currents = new MeasurementDataSet();
            m_cycles = new CycleDataSet();
            m_frequency = 60.0D;

            m_values = new Dictionary<string, object>();
        }

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets the set of voltages used to initialize this <see cref="FaultLocationDataSet"/>.
        /// </summary>
        public MeasurementDataSet Voltages
        {
            get
            {
                return m_voltages;
            }
        }

        /// <summary>
        /// Gets the set of currents used to initialize this <see cref="FaultLocationDataSet"/>.
        /// </summary>
        public MeasurementDataSet Currents
        {
            get
            {
                return m_currents;
            }
        }

        /// <summary>
        /// Gets the set of cycles used to initialize this <see cref="FaultLocationDataSet"/>.
        /// </summary>
        public CycleDataSet Cycles
        {
            get
            {
                return m_cycles;
            }
        }

        /// <summary>
        /// Gets or sets the cycle of prefault data to be used in fault location calculations.
        /// </summary>
        public CycleData PrefaultCycle
        {
            get
            {
                return m_prefaultCycle;
            }
            set
            {
                m_prefaultCycle = value;
            }
        }

        /// <summary>
        /// Gets or sets the frequency of the measured system, in Hz.
        /// </summary>
        public double Frequency
        {
            get
            {
                return m_frequency;
            }
            set
            {
                m_frequency = value;
            }
        }

        /// <summary>
        /// Gets or sets the positive sequence impedance.
        /// </summary>
        public ComplexNumber PositiveImpedance
        {
            get
            {
                return m_positiveImpedance;
            }
            set
            {
                m_positiveImpedance = value;
            }
        }

        /// <summary>
        /// Gets or sets the zero sequence impedance.
        /// </summary>
        public ComplexNumber ZeroImpedance
        {
            get
            {
                return m_zeroImpedance;
            }
            set
            {
                m_zeroImpedance = value;
            }
        }

        /// <summary>
        /// Gets the loop impedance <c>[(Z0 + 2*Z1) / 3]</c>.
        /// </summary>
        public ComplexNumber LoopImpedance
        {
            get
            {
                return (m_zeroImpedance + 2.0D * m_positiveImpedance) / 3.0D;
            }
        }

        /// <summary>
        /// Gets or sets the impedance of the local voltage source.
        /// </summary>
        public ComplexNumber LocalSourceImpedance
        {
            get
            {
                return m_localSourceImpedance;
            }
            set
            {
                m_localSourceImpedance = value;
            }
        }

        /// <summary>
        /// Gets or sets the impedance of the remote voltage source.
        /// </summary>
        public ComplexNumber RemoteSourceImpedance
        {
            get
            {
                return m_remoteSourceImpedance;
            }
            set
            {
                m_remoteSourceImpedance = value;
            }
        }

        /// <summary>
        /// Gets or sets the positive sequence impedance.
        /// </summary>
        public ComplexNumber Z1
        {
            get
            {
                return m_positiveImpedance;
            }
            set
            {
                m_positiveImpedance = value;
            }
        }

        /// <summary>
        /// Gets or sets the zero sequence impedance.
        /// </summary>
        public ComplexNumber Z0
        {
            get
            {
                return m_zeroImpedance;
            }
            set
            {
                m_zeroImpedance = value;
            }
        }

        /// <summary>
        /// Gets the loop impedance <c>[(Z0 + 2*Z1) / 3]</c>.
        /// </summary>
        public ComplexNumber Zs
        {
            get
            {
                return LoopImpedance;
            }
        }

        /// <summary>
        /// Gets or sets the impedance of the local voltage source.
        /// </summary>
        public ComplexNumber ZSrc
        {
            get
            {
                return m_localSourceImpedance;
            }
            set
            {
                m_localSourceImpedance = value;
            }
        }

        /// <summary>
        /// Gets or sets the impedance of the remote voltage source.
        /// </summary>
        public ComplexNumber ZRem
        {
            get
            {
                return m_remoteSourceImpedance;
            }
            set
            {
                m_remoteSourceImpedance = value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum rated current on the line at 50 degrees fahrenheit.
        /// </summary>
        public double RatedCurrent
        {
            get
            {
                return m_ratedCurrent;
            }
            set
            {
                m_ratedCurrent = value;
            }
        }

        /// <summary>
        /// Gets or sets the fault type.
        /// </summary>
        public FaultType FaultType
        {
            get
            {
                return m_faultType;
            }
            set
            {
                m_faultType = value;
            }
        }

        /// <summary>
        /// Gets or sets the list of indexes into the collection of
        /// <see cref="Cycles"/> where fault conditions were detected.
        /// </summary>
        public IList<int> FaultedCycles
        {
            get
            {
                return m_faultedCycles;
            }
            set
            {
                m_faultedCycles = value;
            }
        }

        /// <summary>
        /// Gets or sets the number of cycles where fault conditions were detected.
        /// </summary>
        public int FaultCycleCount
        {
            get
            {
                return m_faultedCycles.Count;
            }
        }

        /// <summary>
        /// Gets or sets the index of the cycle used to calculate fault location.
        /// </summary>
        public int FaultCalculationCycle
        {
            get
            {
                return m_faultCalculationCycle;
            }
            set
            {
                m_faultCalculationCycle = value;
            }
        }

        /// <summary>
        /// Gets or sets the total distance across the line.
        /// </summary>
        public double LineDistance
        {
            get
            {
                return m_lineDistance;
            }
            set
            {
                m_lineDistance = value;
            }
        }

        /// <summary>
        /// Gets or sets the collection of fault distances,
        /// one distance per fault location algorithm.
        /// </summary>
        public Dictionary<string, double[]> FaultDistances
        {
            get
            {
                return m_faultDistances;
            }
            set
            {
                m_faultDistances = value;
            }
        }

        /// <summary>
        /// Gets or sets the total amount of distance from the power
        /// quality device to the fault along the faulted line.
        /// </summary>
        public double FaultDistance
        {
            get
            {
                return m_faultDistance;
            }
            set
            {
                m_faultDistance = value;
            }
        }

        /// <summary>
        /// Gets or sets the value of the parameter with the given name.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <returns>The value of the parameter.</returns>
        public object this[string name]
        {
            get
            {
                return m_values[name];
            }
            set
            {
                m_values[name] = value;
            }
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Uses system frequency to calculate the sample rate for each set of
        /// <see cref="MeasurementData"/> in the voltage and current data sets.
        /// </summary>
        public void CalculateSampleRates()
        {
            Voltages.CalculateSampleRates(m_frequency);
            Currents.CalculateSampleRates(m_frequency);
        }

        /// <summary>
        /// Sets the sample rate of the voltage and current data sets.
        /// </summary>
        /// <param name="sampleRate"></param>
        public void SetSampleRates(int sampleRate)
        {
            Voltages.SetSampleRate(sampleRate);
            Currents.SetSampleRate(sampleRate);
        }

        #endregion
    }
}
