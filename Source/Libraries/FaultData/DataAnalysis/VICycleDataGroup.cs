//******************************************************************************************************
//  VICycleDataGroup.cs - Gbtc
//
//  Copyright © 2014, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the Eclipse Public License -v 1.0 (the "License"); you may
//  not use this file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://www.opensource.org/licenses/eclipse-1.0.php
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  08/29/2014 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System.Collections.Generic;
using System.Linq;
using FaultAlgorithms;

namespace FaultData.DataAnalysis
{
    public class VICycleDataGroup
    {
        #region [ Members ]

        // Fields
        private int m_vaIndex;
        private int m_vbIndex;
        private int m_vcIndex;
        private int m_vabIndex;
        private int m_vbcIndex;
        private int m_vcaIndex;
        private int m_iaIndex;
        private int m_ibIndex;
        private int m_icIndex;
        private int m_irIndex;

        private List<CycleDataGroup> m_cycleDataGroups;

        #endregion

        #region [ Constructors ]

        public VICycleDataGroup(DataGroup dataGroup)
        {
            m_cycleDataGroups = dataGroup.DataSeries
                .Select((dataSeries, index) => new { DataSeries = dataSeries, Index = index })
                .GroupBy(obj => obj.Index / 4)
                .Where(grouping => grouping.Count() >= 4)
                .Select(grouping => grouping.Select(obj => obj.DataSeries))
                .Select(grouping => new CycleDataGroup(new DataGroup(grouping)))
                .ToList();

            MapIndexes();
        }

        public VICycleDataGroup(List<CycleDataGroup> cycleDataGroups)
        {
            m_cycleDataGroups = new List<CycleDataGroup>(cycleDataGroups);
            MapIndexes();
        }

        #endregion

        #region [ Properties ]

        public CycleDataGroup VA
        {
            get
            {
                return (m_vaIndex >= 0) ? m_cycleDataGroups[m_vaIndex] : null;
            }
        }

        public CycleDataGroup VB
        {
            get
            {
                return (m_vbIndex >= 0) ? m_cycleDataGroups[m_vbIndex] : null;
            }
        }

        public CycleDataGroup VC
        {
            get
            {
                return (m_vcIndex >= 0) ? m_cycleDataGroups[m_vcIndex] : null;
            }
        }

        public CycleDataGroup VAB
        {
            get
            {
                return (m_vabIndex >= 0) ? m_cycleDataGroups[m_vabIndex] : null;
            }
        }

        public CycleDataGroup VBC
        {
            get
            {
                return (m_vbcIndex >= 0) ? m_cycleDataGroups[m_vbcIndex] : null;
            }
        }

        public CycleDataGroup VCA
        {
            get
            {
                return (m_vcaIndex >= 0) ? m_cycleDataGroups[m_vcaIndex] : null;
            }
        }

        public CycleDataGroup IA
        {
            get
            {
                return (m_iaIndex >= 0) ? m_cycleDataGroups[m_iaIndex] : null;
            }
        }

        public CycleDataGroup IB
        {
            get
            {
                return (m_ibIndex >= 0) ? m_cycleDataGroups[m_ibIndex] : null;
            }
        }

        public CycleDataGroup IC
        {
            get
            {
                return (m_icIndex >= 0) ? m_cycleDataGroups[m_icIndex] : null;
            }
        }

        public CycleDataGroup IR
        {
            get
            {
                return (m_irIndex >= 0) ? m_cycleDataGroups[m_irIndex] : null;
            }
        }

        public List<CycleDataGroup> CycleDataGroups {
            get {
                return m_cycleDataGroups;
            }
        }

        #endregion

        #region [ Methods ]

        public DataGroup ToDataGroup()
        {
            return Transform.Combine(m_cycleDataGroups
                .Select(cycleDataGroup => cycleDataGroup.ToDataGroup())
                .ToArray());
        }

        public VICycleDataGroup ToSubSet(int startIndex, int endIndex)
        {
            return new VICycleDataGroup(m_cycleDataGroups
                .Select(cycleDataGroup => cycleDataGroup.ToSubGroup(startIndex, endIndex))
                .ToList());
        }

        public void PushDataTo(CycleDataSet cycleDataSet)
        {
            CycleData cycleData;
            Cycle[] cycles;
            CycleDataGroup[] cycleDataGroups;

            cycleDataGroups = new CycleDataGroup[] { VA, VB, VC, IA, IB, IC };
            cycles = new Cycle[cycleDataGroups.Length];

            for (int i = 0; i < VA.ToDataGroup().Samples; i++)
            {
                cycleData = new CycleData();

                cycles[0] = cycleData.AN.V;
                cycles[1] = cycleData.BN.V;
                cycles[2] = cycleData.CN.V;
                cycles[3] = cycleData.AN.I;
                cycles[4] = cycleData.BN.I;
                cycles[5] = cycleData.CN.I;

                for (int j = 0; j < cycles.Length; j++)
                {
                    cycles[j].RMS = cycleDataGroups[j].RMS[i].Value;
                    cycles[j].Phase = cycleDataGroups[j].Phase[i].Value;
                    cycles[j].Peak = cycleDataGroups[j].Peak[i].Value;
                    cycles[j].Error = cycleDataGroups[j].Error[i].Value;
                }

                cycleDataSet[i] = cycleData;
            }
        }

        private void MapIndexes()
        {
            m_vaIndex = -1;
            m_vbIndex = -1;
            m_vcIndex = -1;
            m_vabIndex = -1;
            m_vbcIndex = -1;
            m_vcaIndex = -1;
            m_iaIndex = -1;
            m_ibIndex = -1;
            m_icIndex = -1;
            m_irIndex = -1;

            for (int i = 0; i < m_cycleDataGroups.Count; i++)
            {
                string measurementType = m_cycleDataGroups[i].RMS.SeriesInfo.Channel.MeasurementType.Name;
                string phase = m_cycleDataGroups[i].RMS.SeriesInfo.Channel.Phase.Name;

                if (measurementType == "Voltage" && phase == "AN")
                    m_vaIndex = i;
                else if (measurementType == "Voltage" && phase == "BN")
                    m_vbIndex = i;
                else if (measurementType == "Voltage" && phase == "CN")
                    m_vcIndex = i;
                else if (measurementType == "Voltage" && phase == "AB")
                    m_vabIndex = i;
                else if (measurementType == "Voltage" && phase == "BC")
                    m_vbcIndex = i;
                else if (measurementType == "Voltage" && phase == "CA")
                    m_vcaIndex = i;
                else if (measurementType == "Current" && phase == "AN")
                    m_iaIndex = i;
                else if (measurementType == "Current" && phase == "BN")
                    m_ibIndex = i;
                else if (measurementType == "Current" && phase == "CN")
                    m_icIndex = i;
                else if (measurementType == "Current" && phase == "RES")
                    m_irIndex = i;
            }
        }

        #endregion
    }
}
