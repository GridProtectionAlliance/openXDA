//******************************************************************************************************
//  FaultTypeAlgorithms.cs - Gbtc
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
//  --------------------------------------------------------------------------------------------------- 
//  Portions of this work are derived from "openFLE" which is an Electric Power Research Institute, Inc.
//  (EPRI) copyrighted open source software product released under the BSD license.  openFLE carries
//  the following copyright notice: Version 1.0 - Copyright 2012 ELECTRIC POWER RESEARCH INSTITUTE, INC.
//  All rights reserved.
//  ---------------------------------------------------------------------------------------------------
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  11/12/2013 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using GSF;

namespace FaultAlgorithms
{
    internal class FaultTypeAlgorithms
    {
        /// <summary>
        /// Simple algorithm that checks for faults in a <see cref="FaultLocationDataSet"/>.
        /// </summary>
        /// <param name="faultDataSet">The data set to check for faults.</param>
        /// <param name="parameters">Extra parameters to the algorithm.</param>
        /// <returns>The type of the fault found in the fault data set.</returns>
        [FaultTypeAlgorithm]
        private static FaultType Simple(FaultLocationDataSet faultDataSet, string parameters)
        {
            Dictionary<string, string> parameterLookup;
            string parameterValue;

            int largestCurrentIndex;
            CycleData largestCurrentCycle;
            FaultType faultType;

            double anCurrentRMS;
            double bnCurrentRMS;
            double cnCurrentRMS;
            double anMultiplied;
            double bnMultiplied;
            double cnMultiplied;
            double multiplier;

            parameterLookup = parameters.ParseKeyValuePairs();
            largestCurrentIndex = faultDataSet.Cycles.GetLargestCurrentIndex();
            largestCurrentCycle = faultDataSet.Cycles[largestCurrentIndex];

            if ((object)largestCurrentCycle == null)
                throw new InvalidOperationException("No cycles found in fault data set. Cannot calculate fault type.");

            if (!parameterLookup.TryGetValue("multiplier", out parameterValue) || !double.TryParse(parameterValue, out multiplier))
                multiplier = 5.0;

            anCurrentRMS = largestCurrentCycle.AN.I.RMS;
            bnCurrentRMS = largestCurrentCycle.BN.I.RMS;
            cnCurrentRMS = largestCurrentCycle.CN.I.RMS;
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
    }
}
