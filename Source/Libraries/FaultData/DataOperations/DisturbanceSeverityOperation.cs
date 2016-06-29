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
using FaultData.Database;
using FaultData.Database.MeterDataTableAdapters;
using FaultData.DataResources;
using FaultData.DataSets;
using static FaultData.Database.MeterData;

namespace FaultData.DataOperations
{
    public class DisturbanceSeverityOperation : DataOperationBase<MeterDataSet>
    {
        #region [ Members ]

        // Fields
        private DbAdapterContainer m_dbAdapterContainer;
        private DisturbanceSeverityDataTable m_disturbanceSeverityTable;

        #endregion

        #region [ Methods ]

        public override void Prepare(DbAdapterContainer dbAdapterContainer)
        {
            m_dbAdapterContainer = dbAdapterContainer;
        }

        public override void Execute(MeterDataSet meterDataSet)
        {
            CycleDataResource cycleDataResource = meterDataSet.GetResource<CycleDataResource>();
            SagDataResource sagDataResource = meterDataSet.GetResource<SagDataResource>();
            SwellDataResource swellDataResource = meterDataSet.GetResource<SwellDataResource>();
            InterruptionDataResource interruptionDataResource = meterDataSet.GetResource<InterruptionDataResource>();

            DisturbanceTableAdapter disturbanceAdapter = m_dbAdapterContainer.GetAdapter<DisturbanceTableAdapter>();
            DisturbanceDataTable disturbanceTable = disturbanceAdapter.GetDataByFileGroup(meterDataSet.FileGroup.ID);

            SystemInfoDataContext systemInfo = m_dbAdapterContainer.GetAdapter<SystemInfoDataContext>();

            m_disturbanceSeverityTable = new DisturbanceSeverityDataTable();

            foreach (VoltageEnvelope envelope in systemInfo.VoltageEnvelopes)
            {
                foreach (DisturbanceRow disturbance in disturbanceTable)
                {
                    IEnumerable<VoltageCurvePoint> points = envelope.VoltageCurves.Select(curve => curve.VoltageCurvePoints
                        .Where(p => p.DurationSeconds <= disturbance.DurationSeconds)
                        .OrderBy(p => p.LoadOrder)
                        .LastOrDefault());

                    IEnumerable<int> severityCodes = points.Select(point => ((object)point != null)
                        ? (int)((1.0D - disturbance.PerUnitMagnitude) / (1.0D - point.PerUnitMagnitude))
                        : 0);

                    int maxSeverityCode = severityCodes
                        .DefaultIfEmpty(0)
                        .Max();

                    if (maxSeverityCode < 0)
                        maxSeverityCode = 0;
                    else if (maxSeverityCode > 5)
                        maxSeverityCode = 5;

                    m_disturbanceSeverityTable.AddDisturbanceSeverityRow(envelope.ID, disturbance.ID, maxSeverityCode);
                }
            }
        }

        public override void Load(DbAdapterContainer dbAdapterContainer)
        {
            BulkLoader loader = new BulkLoader();
            loader.Connection = dbAdapterContainer.Connection;
            loader.CommandTimeout = dbAdapterContainer.CommandTimeout;
            loader.Load(m_disturbanceSeverityTable);
        }

        #endregion
    }
}
