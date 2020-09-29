//******************************************************************************************************
//  DisturbanceSeverityOperation.cs - Gbtc
//
//  Copyright © 2016, Grid Protection Alliance.  All Rights Reserved.
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
//  06/21/2016 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System.Collections.Generic;
using System.Linq;
using FaultData.DataResources;
using FaultData.DataSets;
using GSF.Data;
using GSF.Data.Model;
using openXDA.Model;

namespace FaultData.DataOperations
{
    public class DisturbanceSeverityOperation : DataOperationBase<MeterDataSet>
    {
        public override void Execute(MeterDataSet meterDataSet)
        {
            CycleDataResource cycleDataResource = meterDataSet.GetResource<CycleDataResource>();
            SagDataResource sagDataResource = meterDataSet.GetResource<SagDataResource>();
            SwellDataResource swellDataResource = meterDataSet.GetResource<SwellDataResource>();
            InterruptionDataResource interruptionDataResource = meterDataSet.GetResource<InterruptionDataResource>();

            using (AdoDataConnection connection = meterDataSet.CreateDbConnection())
            {
                TableOperations<Disturbance> disturbanceTable = new TableOperations<Disturbance>(connection);
                TableOperations<DisturbanceSeverity> disturbanceSeverityTable = new TableOperations<DisturbanceSeverity>(connection);
                TableOperations<VoltageEnvelope> voltageEnvelopeTable = new TableOperations<VoltageEnvelope>(connection);
                TableOperations<VoltageCurve> voltageCurveTable = new TableOperations<VoltageCurve>(connection);
                TableOperations<VoltageCurvePoint> voltageCurvePointTable = new TableOperations<VoltageCurvePoint>(connection);

                List<Disturbance> disturbances = disturbanceTable
                    .QueryRecordsWhere("EventID IN (SELECT ID FROM Event WHERE FileGroupID = {0} AND FileVersion = {1})", meterDataSet.FileGroup.ID, meterDataSet.FileGroup.ProcessingVersion)
                    .ToList();

                foreach (VoltageEnvelope voltageEnvelope in voltageEnvelopeTable.QueryRecords().ToList())
                {
                    List<VoltageCurve> voltageCurves = voltageCurveTable
                        .QueryRecordsWhere("ID IN (SELECT VoltageCurveID FROM VoltageEnvelopeCurve WHERE VoltageEnvelopeID = {0})", voltageEnvelope.ID)
                        .ToList();

                    foreach (VoltageCurve voltageCurve in voltageCurves)
                    {
                        voltageCurve.VoltageCurvePoints = voltageCurvePointTable
                            .QueryRecordsWhere("VoltageCurveID = {0}", voltageCurve.ID)
                            .ToList();
                    }

                    foreach (Disturbance disturbance in disturbances)
                    {
                        int maxSeverityCode = voltageCurves
                            .Select(voltageCurve => voltageCurve.GetMagnitude(disturbance.DurationSeconds))
                            .Select(curveMagnitude => (int)((1.0D - disturbance.PerUnitMagnitude) / (1.0D - curveMagnitude)))
                            .DefaultIfEmpty(0)
                            .Max();

                        if (maxSeverityCode < 0)
                            maxSeverityCode = 0;
                        else if (maxSeverityCode > 5)
                            maxSeverityCode = 5;

                        disturbanceSeverityTable.AddNewRecord(new DisturbanceSeverity()
                        {
                            VoltageEnvelopeID = voltageEnvelope.ID,
                            DisturbanceID = disturbance.ID,
                            SeverityCode = maxSeverityCode
                        });
                    }
                }
            }
        }
    }
}
