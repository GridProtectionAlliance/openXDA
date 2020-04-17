//******************************************************************************************************
//  MeterDataQualityOverride.cs - Gbtc
//
//  Copyright © 2020, Grid Protection Alliance.  All Rights Reserved.
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
//  04/16/2020 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using FaultData.DataAnalysis;
using FaultData.DataResources;
using FaultData.DataSets;
using GSF.Configuration;

namespace FaultData.DataOperations.GTC
{
    public class MeterDataQualityOverride : DataOperationBase<MeterDataSet>
    {
        public class MeterDataQualitySettings
        {
            [Setting]
            [DefaultValue(15.0D)]
            public double MaxVoltageSlope { get; set; }
        }

        [Category]
        [SettingName("MeterDataQuality")]
        public MeterDataQualitySettings Settings { get; } = new MeterDataQualitySettings();

        [Setting]
        public double SystemFrequency { get; set; }

        public override void Execute(MeterDataSet meterDataSet)
        {
            // The following logic should only apply to NEXUS meters, not DFRs
            if (!meterDataSet.Meter.Model.StartsWith("NEXUS"))
                return;

            CycleDataResource cycleDataResource = meterDataSet.GetResource<CycleDataResource>();
            EventClassificationResource eventClassificationResource = meterDataSet.GetResource<EventClassificationResource>();

            for (int i = 0; i < cycleDataResource.DataGroups.Count; i++)
            {
                DataGroup dataGroup = cycleDataResource.DataGroups[i];
                VIDataGroup viDataGroup = cycleDataResource.VIDataGroups[i];
                double llPeakNominalVoltage = dataGroup.Line.VoltageKV * 1000.0D * Math.Sqrt(2.0D);
                double lnPeakNominalVoltage = llPeakNominalVoltage / Math.Sqrt(3.0D);

                // Nominal multipliers used for first derivative, based on:
                //   d/dt(A*sin(w*t)) = A*w*cos(w*t)
                double omega = 2.0D * Math.PI * SystemFrequency;
                double llFactor = 1.0D / (llPeakNominalVoltage * omega);
                double lnFactor = 1.0D / (lnPeakNominalVoltage * omega);

                DataSeries[] puVoltages =
                {
                    viDataGroup.VA?.Multiply(lnFactor),
                    viDataGroup.VB?.Multiply(lnFactor),
                    viDataGroup.VC?.Multiply(lnFactor),
                    viDataGroup.VAB?.Multiply(llFactor),
                    viDataGroup.VBC?.Multiply(llFactor),
                    viDataGroup.VCA?.Multiply(llFactor)
                };

                bool invalidVoltage = false;

                foreach (DataSeries voltage in puVoltages)
                {
                    if (voltage == null)
                        continue;

                    List<DataPoint> dataPoints = voltage.DataPoints;

                    invalidVoltage |= dataPoints
                        .Zip(dataPoints.Skip(1), (p1, p2) => (p2.Value - p1.Value) / (p2.Time - p1.Time).TotalSeconds)
                        .Any(slope => Math.Abs(slope) > Settings.MaxVoltageSlope);
                }

                if (invalidVoltage)
                    eventClassificationResource.Classifications[dataGroup] = EventClassification.MeterDataQuality;
            }
        }
    }
}
