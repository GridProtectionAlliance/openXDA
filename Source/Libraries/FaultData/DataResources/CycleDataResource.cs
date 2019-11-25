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

using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using FaultData.DataAnalysis;
using FaultData.DataSets;
using log4net;

namespace FaultData.DataResources
{
    public class CycleDataResource : DataResourceBase<MeterDataSet>
    {
        #region [ Members ]

        // Fields
        private double m_systemFrequency;
        private List<DataGroup> m_dataGroups;
        private List<VIDataGroup> m_viDataGroups;
        private List<VICycleDataGroup> m_viCycleDataGroups;

        #endregion

        #region [ Properties ]

        [Setting]
        public double SystemFrequency
        {
            get
            {
                return m_systemFrequency;
            }
            set
            {
                m_systemFrequency = value;
            }
        }

        public List<DataGroup> DataGroups
        {
            get
            {
                return m_dataGroups;
            }
        }

        public List<VIDataGroup> VIDataGroups
        {
            get
            {
                return m_viDataGroups;
            }
        }

        public List<VICycleDataGroup> VICycleDataGroups
        {
            get
            {
                return m_viCycleDataGroups;
            }
        }

        #endregion

        #region [ Methods ]

        public override void Initialize(MeterDataSet meterDataSet)
        {
            DataGroupsResource dataGroupsResource = meterDataSet.GetResource<DataGroupsResource>();
            Stopwatch stopwatch = new Stopwatch();

            m_dataGroups = dataGroupsResource.DataGroups
                .Where(dataGroup => dataGroup.Classification == DataClassification.Event)
                .Where(dataGroup => dataGroup.SamplesPerSecond / m_systemFrequency >= 3.999D)
                .ToList();

            Log.Info(string.Format("Found data for {0} events.", m_dataGroups.Count));

            m_viDataGroups = m_dataGroups
                .Select(dataGroup => new VIDataGroup(dataGroup))
                .ToList();

            Log.Info(string.Format("Calculating cycle data for all {0} events.", m_dataGroups.Count));

            stopwatch.Start();

            m_viCycleDataGroups = m_viDataGroups
                .Select(viDataGroup => Transform.ToVICycleDataGroup(viDataGroup, m_systemFrequency))
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
