//*********************************************************************************************************************
// Cycle.cs
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
//  05/23/2012 - J. Ritchie Carroll, Grid Protection Alliance
//       Generated original version of source code.
//
//*********************************************************************************************************************

using System;
using System.Linq;
using System.Numerics;
using Gemstone;
using Gemstone.Numeric.Analysis;
using Gemstone.Units;

namespace FaultAlgorithms
{
    /// <summary>
    /// Represents a cycle of single phase power frequency-domain data.
    /// </summary>
    public class Cycle
    {
        #region [ Members ]

        // Constants
        private const double PiOverTwo = Math.PI / 2.0D;

        // Fields

        /// <summary>
        /// The actual frequency of the cycle in hertz.
        /// </summary>
        public double Frequency;

        /// <summary>
        /// Root-mean-square of the <see cref="MeasurementData.Measurements"/> in the cycle.
        /// </summary>
        public double RMS;

        /// <summary>
        /// Phase angle of the start of the cycle, relative to the reference angle.
        /// </summary>
        public Angle Phase;

        /// <summary>
        /// The most extreme data point in the cycle.
        /// </summary>
        public double Peak;

        /// <summary>
        /// The error between the sine fit and the given data values.
        /// </summary>
        public double Error;

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Creates a new instance of the <see cref="Cycle"/> class.
        /// </summary>
        public Cycle()
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Cycle"/> class.
        /// </summary>
        /// <param name="startSample">The index of the start of the cycle.</param>
        /// <param name="frequency">The frequency of the measured system, in Hz.</param>
        /// <param name="waveFormData">The time-domain data to be used to calculate frequency-domain values.</param>
        public Cycle(int startSample, double frequency, MeasurementData waveFormData)
        {
            long timeStart;
            double[] timeInSeconds;
            double[] measurements;
            SineWave sineFit;

            if (startSample < 0)
                throw new ArgumentOutOfRangeException("startSample");

            if (startSample + waveFormData.SampleRate > waveFormData.Times.Length)
                throw new ArgumentOutOfRangeException("startSample");

            if (startSample + waveFormData.SampleRate > waveFormData.Measurements.Length)
                throw new ArgumentOutOfRangeException("startSample");

            timeStart = waveFormData.Times[startSample];
            timeInSeconds = new double[waveFormData.SampleRate];
            measurements = new double[waveFormData.SampleRate];

            for (int i = 0; i < waveFormData.SampleRate; i++)
            {
                timeInSeconds[i] = Ticks.ToSeconds(waveFormData.Times[i + startSample] - timeStart);
                measurements[i] = waveFormData.Measurements[i + startSample];
            }

            sineFit = WaveFit.SineFit(measurements, timeInSeconds, frequency);

            RMS = Math.Sqrt(measurements.Select(vi => vi * vi).Average());
            Phase = sineFit.Phase - PiOverTwo;
            Peak = sineFit.Amplitude;
            Frequency = frequency;

            Error = timeInSeconds
                .Select(time => sineFit.CalculateY(time))
                .Zip(measurements, (calc, measurement) => Math.Abs(calc - measurement))
                .Sum();
        }

        #endregion

        #region [ Properties ]

        /// <summary>
        /// The complex number representation of the RMS phasor.
        /// </summary>
        public Complex Complex
        {
            get
            {
                return Complex.FromPolarCoordinates(RMS, Phase);
            }
            set
            {
                RMS = value.Magnitude;
                Phase = value.Phase;
            }
        }

        #endregion
    }
}