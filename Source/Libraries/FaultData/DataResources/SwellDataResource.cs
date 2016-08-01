//******************************************************************************************************
//  SwellDataResource.cs - Gbtc
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
using System.Configuration;
using System.Linq;
using FaultData.DataAnalysis;
using FaultData.Database;
using FaultData.DataSets;
using GSF.Collections;

namespace FaultData.DataResources
{
    public class SwellDataResource : DataResourceBase<MeterDataSet>
    {
        #region [ Members ]

        // Fields
        private DbAdapterContainer m_dbAdapterContainer;

        private double m_systemFrequency;
        private double m_swellThreshold;
        private Dictionary<DataGroup, List<Disturbance>> m_swells;

        #endregion

        #region [ Constructors ]

        private SwellDataResource(DbAdapterContainer dbAdapterContainer)
        {
            m_dbAdapterContainer = dbAdapterContainer;
        }

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

        [Setting]
        public double SwellThreshold
        {
            get
            {
                return m_swellThreshold;
            }
            set
            {
                m_swellThreshold = value;
            }
        }

        public Dictionary<DataGroup, List<Disturbance>> Swells
        {
            get
            {
                return m_swells;
            }
        }

        #endregion

        #region [ Methods ]

        public override void Initialize(MeterDataSet meterDataSet)
        {
            VoltageDisturbanceAnalyzer voltageDisturbanceAnalyzer;

            voltageDisturbanceAnalyzer = new VoltageDisturbanceAnalyzer(IsSwell, IsMoreSevere, EventClassification.Swell);
            voltageDisturbanceAnalyzer.Initialize(meterDataSet, m_dbAdapterContainer);

            m_swells = voltageDisturbanceAnalyzer.Disturbances;
        }

        private bool IsSwell(DataPoint dataPoint)
        {
            return dataPoint.Value >= m_swellThreshold;
        }

        private bool IsMoreSevere(double mag1, double mag2)
        {
            return mag1 > mag2;
        }

        #endregion

        #region [ Static ]

        // Static Methods
        public static SwellDataResource GetResource(MeterDataSet meterDataSet, DbAdapterContainer dbAdapterContainer)
        {
            return meterDataSet.GetResource(() => new SwellDataResource(dbAdapterContainer));
        }

        #endregion
    }
}
