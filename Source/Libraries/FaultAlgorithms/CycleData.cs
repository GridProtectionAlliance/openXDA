//*********************************************************************************************************************
// CycleData.cs
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
//  06/14/2012 - Stephen C. Wills, Grid Protection Alliance
//       Generated original version of source code.
//
//*********************************************************************************************************************

using System;
using GSF;

namespace FaultAlgorithms
{
    /// <summary>
    /// Contains data for a single cycle over all three line-to-neutral conductors.
    /// </summary>
    public class CycleData
    {
        #region [ Members ]

        // Constants

        /// <summary>
        /// 2 * pi
        /// </summary>
        public const double TwoPI = 2.0D * Math.PI;

        // a = e^((2/3) * pi * i)
        private const double Rad120 = TwoPI / 3.0D;
        private static readonly ComplexNumber a = new ComplexNumber(Math.Cos(Rad120), Math.Sin(Rad120));
        private static readonly ComplexNumber aSq = a * a;

        // Fields

        /// <summary>
        /// A-to-neutral conductor
        /// </summary>
        public Conductor AN;

        /// <summary>
        /// B-to-neutral conductor
        /// </summary>
        public Conductor BN;

        /// <summary>
        /// C-to-neutral conductor
        /// </summary>
        public Conductor CN;

        /// <summary>
        /// Timestamp of the start of the cycle.
        /// </summary>
        public DateTime StartTime;

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Creates a new instance of the <see cref="CycleData"/> class.
        /// </summary>
        public CycleData()
        {
            AN = new Conductor();
            BN = new Conductor();
            CN = new Conductor();
        }

        /// <summary>
        /// Creates a new instance of the <see cref="CycleData"/> class.
        /// </summary>
        /// <param name="cycleIndex">The index of the cycle being created.</param>
        /// <param name="sampleRateDivisor">The value to divide from the sample rate to determine the index of the sample at the start of the cycle.</param>
        /// <param name="frequency">The frequency of the measured system, in Hz.</param>
        /// <param name="voltageDataSet">The data set containing voltage measurements.</param>
        /// <param name="currentDataSet">The data set containing current measurements.</param>
        public CycleData(int cycleIndex, int sampleRateDivisor, double frequency, MeasurementDataSet voltageDataSet, MeasurementDataSet currentDataSet)
        {
            int sampleIndex;

            AN = new Conductor(cycleIndex, sampleRateDivisor, frequency, voltageDataSet.AN, currentDataSet.AN);
            BN = new Conductor(cycleIndex, sampleRateDivisor, frequency, voltageDataSet.BN, currentDataSet.BN);
            CN = new Conductor(cycleIndex, sampleRateDivisor, frequency, voltageDataSet.CN, currentDataSet.CN);

            sampleIndex = cycleIndex * (voltageDataSet.AN.SampleRate / sampleRateDivisor);
            StartTime = new DateTime(voltageDataSet.AN.Times[sampleIndex]);
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Calculates the positive, negative, and zero sequence components
        /// and returns them in an array with indexes 1, 2, and 0 respectively.
        /// </summary>
        /// <param name="anCycle">The cycle of A-to-neutral data to be used.</param>
        /// <param name="bnCycle">The cycle of B-to-neutral data to be used.</param>
        /// <param name="cnCycle">The cycle of C-to-neutral data to be used.</param>
        /// <returns>An array of size 3 containing the zero sequence, positive sequence, and negative sequence components in that order.</returns>
        public static ComplexNumber[] CalculateSequenceComponents(Cycle anCycle, Cycle bnCycle, Cycle cnCycle)
        {
            ComplexNumber an = anCycle.Complex;
            ComplexNumber bn = bnCycle.Complex;
            ComplexNumber cn = cnCycle.Complex;

            ComplexNumber[] sequenceComponents = new ComplexNumber[3];

            sequenceComponents[0] = (an + bn + cn) / 3.0D;
            sequenceComponents[1] = (an + a * bn + aSq * cn) / 3.0D;
            sequenceComponents[2] = (an + aSq * bn + a * cn) / 3.0D;

            return sequenceComponents;
        }

        #endregion
    }
}