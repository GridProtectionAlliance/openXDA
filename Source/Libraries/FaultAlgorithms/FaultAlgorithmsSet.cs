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
    /// <returns>Result of distance calculations for each cycle.</returns>
    public delegate double[] FaultLocationAlgorithm(FaultLocationDataSet faultDataSet, string parameters);

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
        public FaultLocationAlgorithm[] FaultLocationAlgorithms;

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
        public string[] FaultLocationParameters;
    }
}
