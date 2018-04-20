//*********************************************************************************************************************
// FaultTriggerAlgorithms.cs
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
//  -------------------------------------------------------------------------------------------------------------------
//  03/08/2013 - Stephen C. Wills
//       Generated original version of source code.
//
//*********************************************************************************************************************


using System;
using System.Collections.Generic;
using GSF;

namespace FaultAlgorithms
{
    internal class FaultTriggerAlgorithms
    {
        [FaultTriggerAlgorithm]
        private static bool PrefaultMultiplierAndRatedCurrentTrigger(FaultLocationDataSet faultDataSet, string parameters)
        {
            Dictionary<string, string> parameterLookup;
            string parameterValue;
            double prefaultMultiplier;
            double ratingMultiplier;

            double anFaultLimit;
            double bnFaultLimit;
            double cnFaultLimit;

            List<int> faultedCycles;
            bool anFaultCycle;
            bool bnFaultCycle;
            bool cnFaultCycle;

            // If no cycles exist in the data set, there is no fault
            if (faultDataSet.Cycles.Count <= 0)
                return false;

            // Get parameters required for determining the existence of the fault
            parameterLookup = parameters.ParseKeyValuePairs();

            if (!parameterLookup.TryGetValue("prefaultMultiplier", out parameterValue) || !double.TryParse(parameterValue, out prefaultMultiplier))
                prefaultMultiplier = 5.0D;

            if (!parameterLookup.TryGetValue("ratingMultiplier", out parameterValue) || !double.TryParse(parameterValue, out ratingMultiplier))
                ratingMultiplier = 2.0D;

            // Determine the upper limit of the current during normal conditions
            anFaultLimit = Math.Max(faultDataSet.Cycles[0].AN.I.RMS * prefaultMultiplier, faultDataSet.RatedCurrent * ratingMultiplier);
            bnFaultLimit = Math.Max(faultDataSet.Cycles[0].BN.I.RMS * prefaultMultiplier, faultDataSet.RatedCurrent * ratingMultiplier);
            cnFaultLimit = Math.Max(faultDataSet.Cycles[0].CN.I.RMS * prefaultMultiplier, faultDataSet.RatedCurrent * ratingMultiplier);

            // Build a list of faulted cycles
            faultedCycles = new List<int>();

            for (int i = 0; i < faultDataSet.Cycles.Count; i++)
            {
                CycleData cycle = faultDataSet.Cycles[i];

                anFaultCycle = (cycle.AN.I.RMS >= anFaultLimit);
                bnFaultCycle = (cycle.BN.I.RMS >= bnFaultLimit);
                cnFaultCycle = (cycle.CN.I.RMS >= cnFaultLimit);

                if (anFaultCycle || bnFaultCycle || cnFaultCycle)
                    faultedCycles.Add(i);
            }

            // Provide additional information about which
            // cycles were found to contain fault conditions
            faultDataSet.FaultedCycles = faultedCycles;

            return faultedCycles.Count > 0;
        }

        private static bool PrefaultMultiplierOrRatedCurrentTrigger(FaultLocationDataSet faultDataSet, string parameters)
        {
            Dictionary<string, string> parameterLookup;
            string parameterValue;
            double prefaultMultiplier;
            double ratingMultiplier;

            double anFaultLimit;
            double bnFaultLimit;
            double cnFaultLimit;

            List<int> faultedCycles;
            bool anFaultCycle;
            bool bnFaultCycle;
            bool cnFaultCycle;

            // If no cycles exist in the data set, there is no fault
            if (faultDataSet.Cycles.Count <= 0)
                return false;

            // Get parameters required for determining the existence of the fault
            parameterLookup = parameters.ParseKeyValuePairs();

            if (!parameterLookup.TryGetValue("prefaultMultiplier", out parameterValue) || !double.TryParse(parameterValue, out prefaultMultiplier))
                prefaultMultiplier = 5.0D;

            if (!parameterLookup.TryGetValue("ratingMultiplier", out parameterValue) || !double.TryParse(parameterValue, out ratingMultiplier))
                ratingMultiplier = 2.0D;

            // Determine the upper limit of the current during normal conditions
            anFaultLimit = Math.Max(Math.Min(faultDataSet.Cycles[0].AN.I.RMS * prefaultMultiplier, faultDataSet.RatedCurrent * ratingMultiplier), faultDataSet.RatedCurrent);
            bnFaultLimit = Math.Max(Math.Min(faultDataSet.Cycles[0].BN.I.RMS * prefaultMultiplier, faultDataSet.RatedCurrent * ratingMultiplier), faultDataSet.RatedCurrent);
            cnFaultLimit = Math.Max(Math.Min(faultDataSet.Cycles[0].CN.I.RMS * prefaultMultiplier, faultDataSet.RatedCurrent * ratingMultiplier), faultDataSet.RatedCurrent);

            // Build a list of faulted cycles
            faultedCycles = new List<int>();

            for (int i = 0; i < faultDataSet.Cycles.Count; i++)
            {
                CycleData cycle = faultDataSet.Cycles[i];

                anFaultCycle = (cycle.AN.I.RMS >= anFaultLimit);
                bnFaultCycle = (cycle.BN.I.RMS >= bnFaultLimit);
                cnFaultCycle = (cycle.CN.I.RMS >= cnFaultLimit);

                if (anFaultCycle || bnFaultCycle || cnFaultCycle)
                    faultedCycles.Add(i);
            }

            // Provide additional information about which
            // cycles were found to contain fault conditions
            faultDataSet.FaultedCycles = faultedCycles;

            return faultedCycles.Count > 0;
        }

        private static bool PrefaultMultiplierTrigger(FaultLocationDataSet faultDataSet, string parameters)
        {
            Dictionary<string, string> parameterLookup;
            string parameterValue;
            double prefaultMultiplier;

            double anFaultLimit;
            double bnFaultLimit;
            double cnFaultLimit;

            List<int> faultedCycles;
            bool anFaultCycle;
            bool bnFaultCycle;
            bool cnFaultCycle;

            // If no cycles exist in the data set, there is no fault
            if (faultDataSet.Cycles.Count <= 0)
                return false;

            // Get parameters required for determining the existence of the fault
            parameterLookup = parameters.ParseKeyValuePairs();

            if (!parameterLookup.TryGetValue("prefaultMultiplier", out parameterValue) || !double.TryParse(parameterValue, out prefaultMultiplier))
                prefaultMultiplier = 5.0D;

            // Determine the upper limit of the current during normal conditions
            anFaultLimit = faultDataSet.Cycles[0].AN.I.RMS * prefaultMultiplier;
            bnFaultLimit = faultDataSet.Cycles[0].BN.I.RMS * prefaultMultiplier;
            cnFaultLimit = faultDataSet.Cycles[0].CN.I.RMS * prefaultMultiplier;

            // Build a list of faulted cycles
            faultedCycles = new List<int>();

            for (int i = 0; i < faultDataSet.Cycles.Count; i++)
            {
                CycleData cycle = faultDataSet.Cycles[i];

                anFaultCycle = (cycle.AN.I.RMS >= anFaultLimit);
                bnFaultCycle = (cycle.BN.I.RMS >= bnFaultLimit);
                cnFaultCycle = (cycle.CN.I.RMS >= cnFaultLimit);

                if (anFaultCycle || bnFaultCycle || cnFaultCycle)
                    faultedCycles.Add(i);
            }

            // Provide additional information about which
            // cycles were found to contain fault conditions
            faultDataSet.FaultedCycles = faultedCycles;

            return faultedCycles.Count > 0;
        }
        private static bool RatedCurrentTrigger(FaultLocationDataSet faultDataSet, string parameters)
        {
            Dictionary<string, string> parameterLookup;
            string parameterValue;
            double ratingMultiplier;

            double anFaultLimit;
            double bnFaultLimit;
            double cnFaultLimit;

            List<int> faultedCycles;
            bool anFaultCycle;
            bool bnFaultCycle;
            bool cnFaultCycle;

            // If no cycles exist in the data set, there is no fault
            if (faultDataSet.Cycles.Count <= 0)
                return false;

            // Get parameters required for determining the existence of the fault
            parameterLookup = parameters.ParseKeyValuePairs();

            if (!parameterLookup.TryGetValue("ratingMultiplier", out parameterValue) || !double.TryParse(parameterValue, out ratingMultiplier))
                ratingMultiplier = 2.0D;

            // Determine the upper limit of the current during normal conditions
            anFaultLimit = faultDataSet.RatedCurrent * ratingMultiplier;
            bnFaultLimit = faultDataSet.RatedCurrent * ratingMultiplier;
            cnFaultLimit = faultDataSet.RatedCurrent * ratingMultiplier;

            // Build a list of faulted cycles
            faultedCycles = new List<int>();

            for (int i = 0; i < faultDataSet.Cycles.Count; i++)
            {
                CycleData cycle = faultDataSet.Cycles[i];

                anFaultCycle = (cycle.AN.I.RMS >= anFaultLimit);
                bnFaultCycle = (cycle.BN.I.RMS >= bnFaultLimit);
                cnFaultCycle = (cycle.CN.I.RMS >= cnFaultLimit);

                if (anFaultCycle || bnFaultCycle || cnFaultCycle)
                    faultedCycles.Add(i);
            }

            // Provide additional information about which
            // cycles were found to contain fault conditions
            faultDataSet.FaultedCycles = faultedCycles;

            return faultedCycles.Count > 0;
        }

        // True if current spikes upward by 300amps within a cycle while in a range between the 
        //rated current and a user defined maximum current. 
        private static bool RangeSpikeTrigger(FaultLocationDataSet faultDataSet, string parameters)
        {
            Dictionary<string, string> parameterLookup;
            string parameterValue;
            double ratingMultiplier;

            double anFaultLimit;
            double bnFaultLimit;
            double cnFaultLimit;

            // If no cycles exist in the data set, there is no fault
            if (faultDataSet.Cycles.Count <= 0)
                return false;

            // Get parameters required for determining the existence of the fault
            parameterLookup = parameters.ParseKeyValuePairs();

            if (!parameterLookup.TryGetValue("ratingMultiplier", out parameterValue) || !double.TryParse(parameterValue, out ratingMultiplier))
                ratingMultiplier = 0.50D;

            // Determine the upper limit of the current range during normal conditions
            anFaultLimit =  faultDataSet.RatedCurrent * ratingMultiplier;
            bnFaultLimit =  faultDataSet.RatedCurrent * ratingMultiplier;
            cnFaultLimit =  faultDataSet.RatedCurrent * ratingMultiplier;

            for (int i = 0; i < faultDataSet.Cycles.Count; i++)
            {
                CycleData cycle = faultDataSet.Cycles[i];

                if ((cycle.AN.I.RMS >= faultDataSet.RatedCurrent && cycle.AN.I.RMS <= anFaultLimit ||
                     cycle.BN.I.RMS >= faultDataSet.RatedCurrent && cycle.BN.I.RMS <= bnFaultLimit ||
                     cycle.CN.I.RMS >= faultDataSet.RatedCurrent && cycle.CN.I.RMS <= cnFaultLimit) &&

                     faultDataSet.Cycles[i + 1].AN.I.RMS > faultDataSet.Cycles[i].AN.I.RMS + 300D ||
                     faultDataSet.Cycles[i + 1].BN.I.RMS > faultDataSet.Cycles[i].BN.I.RMS + 300D ||
                     faultDataSet.Cycles[i + 1].CN.I.RMS > faultDataSet.Cycles[i].CN.I.RMS + 300D)

                    return true;
            }
            return false;
        }
    }
}
