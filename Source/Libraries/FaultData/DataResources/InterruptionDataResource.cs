//******************************************************************************************************
//  InterruptionDataResource.cs - Gbtc
//
//  Copyright © 2015, Grid Protection Alliance.  All Rights Reserved.
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
//  08/29/2015 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System.Collections.Generic;
using System.ComponentModel;
using FaultData.DataAnalysis;
using FaultData.DataSets;
using GSF.Configuration;
using openXDA.Configuration;

namespace FaultData.DataResources
{
    public class InterruptionDataResource : DataResourceBase<MeterDataSet>
    {
        #region [ Properties ]

        [Category]
        [SettingName(DataAnalysisSection.CategoryName)]
        public DataAnalysisSection DataAnalysisSettings { get; }
            = new DataAnalysisSection();

        public Dictionary<DataGroup, List<Disturbance>> Interruptions { get; private set; }

        #endregion

        #region [ Methods ]

        public override void Initialize(MeterDataSet meterDataSet)
        {
            EventClassification classification = EventClassification.Interruption;
            VoltageDisturbanceAnalyzer voltageDisturbanceAnalyzer = new VoltageDisturbanceAnalyzer(IsInterruption, IsMoreSevere, classification);
            voltageDisturbanceAnalyzer.Initialize(meterDataSet);

            Interruptions = voltageDisturbanceAnalyzer.Disturbances;
        }

        private bool IsInterruption(DataPoint dataPoint) =>
            dataPoint.Value <= DataAnalysisSettings.InterruptionThreshold;

        private bool IsMoreSevere(double mag1, double mag2) =>
            mag1 < mag2;

        #endregion
    }
}
