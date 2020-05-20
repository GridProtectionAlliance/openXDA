//******************************************************************************************************
//  BreakerRestrikeOperation.cs - Gbtc
//
//  Copyright © 2019, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the MIT License (MIT), the "License"; you may not use this
//  file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://opensource.org/licenses/MIT
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  06/18/2019 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System.Collections.Generic;
using System.Linq;
using FaultData.DataAnalysis;
using FaultData.DataResources;
using FaultData.DataResources.GTC;
using FaultData.DataSets;
using GSF.Data;
using GSF.Data.Model;
using openXDA.Model;

namespace FaultData.DataOperations
{
    public class BreakerRestrikeOperation : DataOperationBase<MeterDataSet>
    {
        public override void Execute(MeterDataSet meterDataSet)
        {
            LoadRestrikeAnalysis(meterDataSet);
            LoadGTCRestrikeData(meterDataSet);
        }

        private void LoadRestrikeAnalysis(MeterDataSet meterDataSet)
        {
            BreakerDataResource breakerDataResource = meterDataSet.GetResource<BreakerDataResource>();

            foreach (var kvp in breakerDataResource.RestrikeLookup)
            {
                DataGroup dataGroup = kvp.Key;
                List<BreakerDataResource.Restrike> restrikes = kvp.Value;

                using (AdoDataConnection connection = meterDataSet.CreateDbConnection())
                {
                    TableOperations<Event> eventTable = new TableOperations<Event>(connection);
                    TableOperations<Phase> phaseTable = new TableOperations<Phase>(connection);
                    TableOperations<BreakerRestrike> breakerRestrikeTable = new TableOperations<BreakerRestrike>(connection);

                    Event evt = eventTable.GetEvent(meterDataSet.FileGroup, dataGroup);

                    foreach (BreakerDataResource.Restrike restrike in restrikes)
                    {
                        Phase phase = phaseTable.GetOrAdd(restrike.Phase.Name);
                        BreakerRestrike breakerRestrike = ProcessRestrike(restrike, phase, evt, new VIDataGroup(dataGroup));
                        breakerRestrikeTable.AddNewRecord(breakerRestrike);
                    }
                }
            }
        }

        private void LoadGTCRestrikeData(MeterDataSet meterDataSet)
        {
            string breakerRestrikeData = GetBreakerRestrikeData(meterDataSet);

            if (breakerRestrikeData == null)
                return;

            using (AdoDataConnection connection = meterDataSet.CreateDbConnection())
            {
                const string FieldName = "BreakerRestrike";
                const string Description = "Indicates whether the file captures a breaker restrike event.";
                meterDataSet.FileGroup.AddFieldValue(connection, FieldName, breakerRestrikeData, Description);
            }
        }

        private string GetBreakerRestrikeData(MeterDataSet meterDataSet)
        {
            BreakerRestrikeResource breakerRestrikeResource = meterDataSet.GetResource<BreakerRestrikeResource>();
            return breakerRestrikeResource.BreakerRestrikeData;
        }

        private BreakerRestrike ProcessRestrike(BreakerDataResource.Restrike restrike, Phase phase, Event evt, VIDataGroup dataGroup)
        {

            DataSeries voltage = (phase.Name == "AN"? dataGroup.VA: (phase.Name == "BN" ? dataGroup.VB : dataGroup.VC));
            DataSeries current = (phase.Name == "AN" ? dataGroup.IA : (phase.Name == "BN" ? dataGroup.IB : dataGroup.IC));

            BreakerRestrike result = new BreakerRestrike();

            result.EventID = evt.ID;
            result.PhaseID = phase.ID;

            result.InitialExtinguishSample = restrike.initialExtinction;
            result.InitialExtinguishTime = current[restrike.initialExtinction].Time;
            result.InitialExtinguishVoltage= voltage[restrike.initialExtinction].Value;

            result.RestrikeSample = restrike.restrike;
            result.RestrikeTime = current[restrike.restrike].Time;
            result.RestrikeVoltage = voltage[restrike.restrike].Value;

            result.RestrikeCurrentPeak = current[restrike.currentMaximum].Value;
            result.RestrikeVoltageDip = voltage[restrike.maximumVoltageSurpression].Value;

            result.TransientPeakSample = restrike.transientOverVoltage;


            result.TransientPeakTime = current[restrike.transientOverVoltage].Time;
            result.TransientPeakVoltage = voltage[restrike.transientOverVoltage].Value;
            result.PerUnitTransientPeakVoltage = voltage[restrike.transientOverVoltage].Value;

            result.FinalExtinguishSample = restrike.finalExtinction;
            result.FinalExtinguishTime = current[restrike.finalExtinction].Time;
            result.FinalExtinguishVoltage = voltage[restrike.finalExtinction].Value;
            result.I2t = 0.0;

            return result;

        }
    }
}
