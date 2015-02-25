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
using System.Linq;
using FaultData.DataAnalysis;
using FaultData.DataSets;

namespace FaultData.DataResources
{
    public class CycleDataResource : DataResourceBase<MeterDataSet>
    {
        #region [ Members ]

        // Constants
        // TODO: Hardcoded frequency
        public const double Frequency = 60.0D;

        // Fields
        private List<DataGroup> m_dataGroups;
        private List<VIDataGroup> m_viDataGroups;
        private List<VICycleDataGroup> m_viCycleDataGroups;

        #endregion

        #region [ Properties ]

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

        public override void Initialize(MeterDataSet dataSet)
        {
            DataGroupsResource dataGroupsResource = dataSet.GetResource<DataGroupsResource>();

            m_dataGroups = dataGroupsResource.DataGroups
                .Where(dataGroup => dataGroup.Classification == DataClassification.Event)
                .ToList();

            m_viDataGroups = m_dataGroups
                .Select(dataGroup => new VIDataGroup(dataGroup))
                .ToList();

            m_viCycleDataGroups = m_viDataGroups
                .Select(viDataGroup => Transform.ToVICycleDataSet(viDataGroup, Frequency))
                .ToList();
        }

        #endregion
    }
}
