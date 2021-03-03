//******************************************************************************************************
//  CycleDataResource.cs - Gbtc
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
//  02/20/2015 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using FaultData.DataAnalysis;
using FaultData.DataSets;
using GSF.Configuration;
using log4net;
using openXDA.Configuration;

namespace FaultData.DataResources
{
    public class CycleDataResource : DataResourceBase<MeterDataSet>
    {
        #region [ Properties ]

        [Category]
        [SettingName(DataAnalysisSection.CategoryName)]
        public DataAnalysisSection DataAnalysisSettings { get; }
            = new DataAnalysisSection();

        public List<DataGroup> DataGroups { get; private set; }
        public List<VIDataGroup> VIDataGroups { get; private set; }
        public List<VICycleDataGroup> VICycleDataGroups { get; private set; }

        private TimeSpan MaxEventDurationSpan =>
            TimeSpan.FromSeconds(DataAnalysisSettings.MaxEventDuration);

        #endregion

        #region [ Methods ]

        public override void Initialize(MeterDataSet meterDataSet)
        {
            double systemFrequency = DataAnalysisSettings.SystemFrequency;
            DataGroupsResource dataGroupsResource = meterDataSet.GetResource<DataGroupsResource>();
            Stopwatch stopwatch = new Stopwatch();

            DataGroups = dataGroupsResource.DataGroups
                .Where(dataGroup => dataGroup.Classification == DataClassification.Event)
                .Where(dataGroup => dataGroup.SamplesPerSecond / systemFrequency >= 3.999D)
                .Where(dataGroup => MaxEventDurationSpan <= TimeSpan.Zero || TimeSpan.FromSeconds(dataGroup.Duration) <= MaxEventDurationSpan)
                .ToList();

            Log.Info(string.Format("Found data for {0} events.", DataGroups.Count));

            VIDataGroups = DataGroups
                .Select(dataGroup => new VIDataGroup(dataGroup))
                .ToList();

            Log.Info(string.Format("Calculating cycle data for all {0} events.", DataGroups.Count));

            stopwatch.Start();

            VICycleDataGroups = VIDataGroups
                .Select(viDataGroup => Transform.ToVICycleDataGroup(viDataGroup, systemFrequency))
                .ToList();

            Log.Debug(string.Format("Cycle data calculated in {0}.", stopwatch.Elapsed));
        }

        #endregion

        #region [ Static ]

        // Static Fields
        private static readonly ILog Log = LogManager.GetLogger(typeof(CycleDataResource));

        #endregion
    }
}
