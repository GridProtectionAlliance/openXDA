//******************************************************************************************************
//  PQubeClassificationOverride.cs - Gbtc
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
//  02/17/2020 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FaultData.DataAnalysis;
using FaultData.DataResources;
using FaultData.DataSets;

namespace FaultData.DataOperations.PQube
{
    public class PQubeClassificationOverride : DataOperationBase<MeterDataSet>
    {
        public override void Execute(MeterDataSet meterDataSet)
        {
            EventClassificationResource eventClassificationResource = meterDataSet.GetResource<EventClassificationResource>();

            foreach (DataGroup dataGroup in eventClassificationResource.Classifications.Keys)
            {
                if (meterDataSet.Triggers.Contains("Voltage Sag"))
                    eventClassificationResource.Classifications[dataGroup] = EventClassification.Sag;

                if (meterDataSet.Triggers.Contains("Sag became Major Sag"))
                    eventClassificationResource.Classifications[dataGroup] = EventClassification.Sag;

                if (meterDataSet.Triggers.Contains("Voltage Swell"))
                    eventClassificationResource.Classifications[dataGroup] = EventClassification.Swell;

                if (meterDataSet.Triggers.Contains("Interruption"))
                    eventClassificationResource.Classifications[dataGroup] = EventClassification.Interruption;

                if (meterDataSet.Triggers.Contains("Waveshape Change"))
                    eventClassificationResource.Classifications[dataGroup] = EventClassification.Transient;

                if (meterDataSet.Triggers.Contains("Impulse"))
                    eventClassificationResource.Classifications[dataGroup] = EventClassification.Transient;

                if (meterDataSet.Triggers.Contains("High Frequency Impulse"))
                    eventClassificationResource.Classifications[dataGroup] = EventClassification.Transient;

                if (meterDataSet.Triggers.Contains("Snapshot"))
                    eventClassificationResource.Classifications[dataGroup] = EventClassification.Snapshot;

                if (meterDataSet.Triggers.Contains("Over-frequency"))
                    eventClassificationResource.Classifications[dataGroup] = EventClassification.Other;

                if (meterDataSet.Triggers.Contains("Under-frequency"))
                    eventClassificationResource.Classifications[dataGroup] = EventClassification.Other;

                if (meterDataSet.Triggers.Contains("RVC"))
                    eventClassificationResource.Classifications[dataGroup] = EventClassification.Other;

                if (meterDataSet.Triggers.Contains("QD"))
                    eventClassificationResource.Classifications[dataGroup] = EventClassification.Other;
            }
        }
    }
}
