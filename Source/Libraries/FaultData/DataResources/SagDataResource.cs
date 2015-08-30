//******************************************************************************************************
//  SagDataResource.cs - Gbtc
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
//  07/28/2015 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using FaultData.DataAnalysis;
using FaultData.DataSets;

namespace FaultData.DataResources
{
    public class SagDataResource : DataResourceBase<MeterDataSet>
    {
        #region [ Members ]

        // Fields
        private double m_systemFrequency;
        private Dictionary<DataGroup, List<Disturbance>> m_sags;

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

        public Dictionary<DataGroup, List<Disturbance>> Sags
        {
            get
            {
                return m_sags;
            }
        }

        #endregion

        #region [ Methods ]

        public override void Initialize(MeterDataSet meterDataSet)
        {
            VoltageDisturbanceAnalyzer voltageDisturbanceAnalyzer;

            voltageDisturbanceAnalyzer = new VoltageDisturbanceAnalyzer(IsSag, GetMagnitude, GetVAllPoint, EventClassification.Sag);
            voltageDisturbanceAnalyzer.Initialize(meterDataSet);

            m_sags = voltageDisturbanceAnalyzer.Disturbances;
        }

        private bool IsSag(DataPoint dataPoint)
        {
            return 0.1D < dataPoint.Value && dataPoint.Value <= 0.9D;
        }

        private double GetMagnitude(DataSeries rms)
        {
            return rms.DataPoints.Min(dataPoint => dataPoint.Value);
        }

        private DataPoint GetVAllPoint(DataPoint va, DataPoint vb, DataPoint vc)
        {
            if (va.Value < vb.Value && va.Value < vc.Value)
                return va;

            if (vb.Value < vc.Value)
                return vb;

            return vc;
        }

        #endregion
    }
}
