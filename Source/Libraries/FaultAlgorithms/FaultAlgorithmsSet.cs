//*********************************************************************************************************************
// FaultAlgorithms.cs
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
using System.Collections.Generic;
using System.Linq;
using GSF;

namespace FaultAlgorithms
{
    /// <summary>
    /// Function signature for fault trigger algorithms.
    /// </summary>
    /// <param name="faultDataSet">Full collection of voltage, current, and cycle data.</param>
    /// <param name="parameters">Custom parameters for algorithm.</param>
    /// <returns><c>true</c> if fault was found in dataset; otherwise <c>false</c>.</returns>
    public delegate bool FaultTriggerAlgorithm(FaultLocationDataSet faultDataSet, string parameters);

    /// <summary>
    /// Function signature for fault type algorithms.
    /// </summary>
    /// <param name="faultDataSet">Full collection of voltage, current, and cycle data.</param>
    /// <param name="parameters">Custom parameters for algorithm.</param>
    /// <returns>The type of the fault found in the fault data set.</returns>
    public delegate FaultType FaultTypeAlgorithm(FaultLocationDataSet faultDataSet, string parameters);

    /// <summary>
    /// Function signature for fault location algorithms.
    /// </summary>
    /// <param name="faultDataSet">Full collection of voltage, current, and cycle data.</param>
    /// <param name="parameters">Custom parameters for algorithm.</param>
    /// <returns>Percentage of distance down the line where fault occured.</returns>
    public delegate double FaultLocationAlgorithm(FaultLocationDataSet faultDataSet, string parameters);

    /// <summary>
    /// Attribute used to annotate fault trigger algorithms.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class FaultTriggerAlgorithmAttribute : Attribute
    {
    }

    /// <summary>
    /// Attribute used to annotate fault type algorithms.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class FaultTypeAlgorithmAttribute : Attribute
    {
    }

    /// <summary>
    /// Attribute used to annotate fault location algorithms.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class FaultLocationAlgorithmAttribute : Attribute
    {
    }

    /// <summary>
    /// Collection of fault algorithms and their parameters.
    /// </summary>
    public class FaultAlgorithmsSet
    {
        /// <summary>
        /// The fault trigger algorithm.
        /// </summary>
        public FaultTriggerAlgorithm FaultTriggerAlgorithm;

        /// <summary>
        /// The fault type algorithm.
        /// </summary>
        public FaultTypeAlgorithm FaultTypeAlgorithm;

        /// <summary>
        /// The fault location algorithm.
        /// </summary>
        public FaultLocationAlgorithm FaultLocationAlgorithm;

        /// <summary>
        /// The fault trigger parameters.
        /// </summary>
        public string FaultTriggerParameters;

        /// <summary>
        /// The fault type parameters.
        /// </summary>
        public string FaultTypeParameters;

        /// <summary>
        /// The fault location parameters.
        /// </summary>
        public string FaultLocationParameters;
    }

    /// <summary>
    /// Defines built-in fault detection and location algorithms
    /// </summary>
    public static class SimpleFaultAlgorithms
    {
        /// <summary>
        /// Simple algorithm that checks for faults in a <see cref="FaultLocationDataSet"/>.
        /// </summary>
        /// <param name="faultDataSet">The data set to check for faults.</param>
        /// <param name="parameters">Extra parameters to the algorithm.</param>
        /// <returns>The type of the fault found in the fault data set.</returns>
        [FaultTypeAlgorithm]
        private static FaultType SimpleFaultTypeAlgorithm(FaultLocationDataSet faultDataSet, string parameters)
        {
            Dictionary<string, string> parameterLookup;
            string parameterValue;

            CycleData bestFaultCycle;
            FaultType faultType;
            double anCurrentRMS;
            double bnCurrentRMS;
            double cnCurrentRMS;
            double anMultiplied;
            double bnMultiplied;
            double cnMultiplied;
            double multiplier;

            parameterLookup = parameters.ParseKeyValuePairs();
            bestFaultCycle = GetBestFaultCycle(faultDataSet);

            if ((object)bestFaultCycle == null)
                throw new InvalidOperationException("No cycles found in fault data set. Cannot calculate fault type.");

            if (!parameterLookup.TryGetValue("multiplier", out parameterValue) || !double.TryParse(parameterValue, out multiplier))
                multiplier = 5.0;

            anCurrentRMS = bestFaultCycle.AN.I.RMS;
            bnCurrentRMS = bestFaultCycle.BN.I.RMS;
            cnCurrentRMS = bestFaultCycle.CN.I.RMS;
            anMultiplied = anCurrentRMS * multiplier;
            bnMultiplied = bnCurrentRMS * multiplier;
            cnMultiplied = cnCurrentRMS * multiplier;

            if (anCurrentRMS >= bnMultiplied && anCurrentRMS >= cnMultiplied)
                faultType = FaultType.AN;
            else if (bnCurrentRMS >= anMultiplied && bnCurrentRMS >= cnMultiplied)
                faultType = FaultType.BN;
            else if (cnCurrentRMS >= anMultiplied && cnCurrentRMS >= bnMultiplied)
                faultType = FaultType.CN;
            else if (anCurrentRMS >= cnMultiplied && bnCurrentRMS >= cnMultiplied)
                faultType = FaultType.AB;
            else if (bnCurrentRMS >= anMultiplied && cnCurrentRMS >= anMultiplied)
                faultType = FaultType.BC;
            else if (anCurrentRMS >= bnMultiplied && cnCurrentRMS >= bnMultiplied)
                faultType = FaultType.CA;
            else
                faultType = FaultType.ABC;

            return faultType;
        }

        /// <summary>
        /// Simple algorithm that determines the distance to a fault that was found in the <see cref="FaultLocationDataSet"/>.
        /// </summary>
        /// <param name="faultDataSet">The data set to be used to find the distance to fault.</param>
        /// <param name="parameters">Extra parameters to the algorithm.</param>
        /// <returns>Distance to the fault.</returns>
        [FaultLocationAlgorithm]
        private static double SimpleFaultLocationAlgorithm(FaultLocationDataSet faultDataSet, string parameters)
        {
            CycleData bestFaultCycle;
            ComplexNumber v, i, z;

            // Get the cycle with the largest sum of currents
            bestFaultCycle = GetBestFaultCycle(faultDataSet);

            if ((object)bestFaultCycle == null)
                throw new InvalidOperationException("No cycles found in fault data set. Cannot calculate fault distance.");

            switch (faultDataSet.FaultType)
            {
                case FaultType.AN:
                    v = bestFaultCycle.AN.V.Complex;
                    i = bestFaultCycle.AN.I.Complex;
                    z = faultDataSet.Zs;
                    break;

                case FaultType.BN:
                    v = bestFaultCycle.BN.V.Complex;
                    i = bestFaultCycle.BN.I.Complex;
                    z = faultDataSet.Zs;
                    break;

                case FaultType.CN:
                    v = bestFaultCycle.CN.V.Complex;
                    i = bestFaultCycle.CN.I.Complex;
                    z = faultDataSet.Zs;
                    break;

                case FaultType.AB:
                    v = bestFaultCycle.AN.V.Complex - bestFaultCycle.BN.V.Complex;
                    i = bestFaultCycle.AN.I.Complex - bestFaultCycle.BN.I.Complex;
                    z = faultDataSet.Z1;
                    break;

                case FaultType.BC:
                    v = bestFaultCycle.BN.V.Complex - bestFaultCycle.CN.V.Complex;
                    i = bestFaultCycle.BN.I.Complex - bestFaultCycle.CN.I.Complex;
                    z = faultDataSet.Z1;
                    break;

                case FaultType.CA:
                    v = bestFaultCycle.CN.V.Complex - bestFaultCycle.AN.V.Complex;
                    i = bestFaultCycle.CN.I.Complex - bestFaultCycle.AN.I.Complex;
                    z = faultDataSet.Z1;
                    break;

                case FaultType.ABC:
                    Conductor[] conductors = { bestFaultCycle.AN, bestFaultCycle.BN, bestFaultCycle.CN };
                    Conductor bestConductor = conductors.OrderBy(conductor => conductor.IError).First();

                    v = bestConductor.V.Complex;
                    i = bestConductor.I.Complex;
                    z = faultDataSet.Z1;
                    break;

                default:
                    throw new ArgumentOutOfRangeException("Unknown fault type: " + faultDataSet.FaultType);
            }

            // Provide additional information about how the fault was calculated
            faultDataSet.FaultCalculationCycle = faultDataSet.Cycles
                .Select((cycle, index) => index)
                .Where(index => (object)faultDataSet.Cycles[index] == (object)bestFaultCycle)
                .DefaultIfEmpty(-1)
                .First();

            // Calculate fault distance
            return faultDataSet.LineDistance * (v.Magnitude / i.Magnitude) / z.Magnitude;
        }

        // Returns the cycle with the largest total current.
        private static CycleData GetBestFaultCycle(FaultLocationDataSet faultDataSet)
        {
            CycleData bestFaultCycle = null;
            double largestCurrent = 0.0D;

            foreach (CycleData cycle in faultDataSet.Cycles)
            {
                double totalCurrent = cycle.AN.I.RMS + cycle.BN.I.RMS + cycle.CN.I.RMS;

                if (totalCurrent > largestCurrent)
                {
                    bestFaultCycle = cycle;
                    largestCurrent = totalCurrent;
                }
            }

            return bestFaultCycle;
        }
    }
}
