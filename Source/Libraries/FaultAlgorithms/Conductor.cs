//*********************************************************************************************************************
// Conductor.cs
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
using System.Linq;
using GSF;
using GSF.NumericalAnalysis;

namespace FaultAlgorithms
{
    /// <summary>
    /// Contains data for both the voltage
    /// and current on a conductor.
    /// </summary>
    public class Conductor
    {
        #region [ Members ]

        // Constants
        private const double PiOverTwo = Math.PI / 2.0D;

        // Fields

        /// <summary>
        /// One cycle of voltage data.
        /// </summary>
        public Cycle V;

        /// <summary>
        /// One cycle of current data.
        /// </summary>
        public Cycle I;

        /// <summary>
        /// The error in voltage between the sine fit and the given data values.
        /// </summary>
        public double VError;

        /// <summary>
        /// The error in current between the sine fit and the given data values.
        /// </summary>
        public double IError;

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Creates a new instance of the <see cref="Conductor"/> class.
        /// </summary>
        public Conductor()
        {
            V = new Cycle();
            I = new Cycle();
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Conductor"/> class.
        /// </summary>
        /// <param name="frequency">The frequency of the sine wave during this cycle.</param>
        /// <param name="timeData">The timestamps of the data points in the given data.</param>
        /// <param name="voltData">The voltage data points.</param>
        /// <param name="currentData">The current data points.</param>
        public Conductor(double frequency, long[] timeData, double[] voltData, double[] currentData)
        {
            long timeStart = timeData[0];
            double[] timeInSeconds = timeData.Select(ti => Ticks.ToSeconds(ti - timeStart)).ToArray();
            SineWave vFit = WaveFit.SineFit(voltData, timeInSeconds, frequency);
            SineWave iFit = WaveFit.SineFit(currentData, timeInSeconds, frequency);

            V = new Cycle()
            {
                RMS = Math.Sqrt(voltData.Select(vi => vi * vi).Average()),
                Phase = vFit.Phase - PiOverTwo,
                Peak = vFit.Amplitude,
                Frequency = frequency
            };

            I = new Cycle()
            {
                RMS = Math.Sqrt(currentData.Select(ci => ci * ci).Average()),
                Phase = iFit.Phase - PiOverTwo,
                Peak = iFit.Amplitude,
                Frequency = frequency
            };

            for (int i = 0; i < timeInSeconds.Length; i++)
            {
                double time = timeInSeconds[i];
                double voltage = voltData[i];
                double current = currentData[i];

                double vEst = vFit.CalculateY(time);
                double iEst = iFit.CalculateY(time);

                VError += Math.Abs(voltage - vEst);
                IError += Math.Abs(current - iEst);
            }
        }

        #endregion
    }
}
