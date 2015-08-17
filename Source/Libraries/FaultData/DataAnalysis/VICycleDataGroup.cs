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

using System;
using System.Collections.Generic;
using System.Linq;
using FaultAlgorithms;

namespace FaultData.DataAnalysis
{
    public class VICycleDataGroup
    {
        #region [ Members ]

        // Constants
        private const int VAIndex = 0;
        private const int VBIndex = 1;
        private const int VCIndex = 2;
        private const int IAIndex = 3;
        private const int IBIndex = 4;
        private const int ICIndex = 5;
        private const int IRIndex = 6;

        // Fields
        private List<CycleDataGroup> m_cycleDataGroups;

        #endregion

        #region [ Constructors ]

        public VICycleDataGroup(DataGroup dataGroup)
        {
            m_cycleDataGroups = dataGroup.DataSeries
                .Select((series, i) => Tuple.Create(i / 4, series))
                .GroupBy(tuple => tuple.Item1)
                .Select(grouping => new DataGroup(grouping.Select(tuple => tuple.Item2)))
                .Select(group => new CycleDataGroup(group))
                .ToList();
        }

        public VICycleDataGroup(CycleDataGroup va, CycleDataGroup vb, CycleDataGroup vc, CycleDataGroup ia, CycleDataGroup ib, CycleDataGroup ic, CycleDataGroup ir)
        {
            m_cycleDataGroups = new List<CycleDataGroup>() { va, vb, vc, ia, ib, ic, ir };
        }

        public VICycleDataGroup(List<CycleDataGroup> cycleDataGroups)
        {
            m_cycleDataGroups = cycleDataGroups;
        }

        #endregion

        #region [ Properties ]

        public CycleDataGroup VA
        {
            get
            {
                return m_cycleDataGroups[VAIndex];
            }
        }

        public CycleDataGroup VB
        {
            get
            {
                return m_cycleDataGroups[VBIndex];
            }
        }

        public CycleDataGroup VC
        {
            get
            {
                return m_cycleDataGroups[VCIndex];
            }
        }

        public CycleDataGroup IA
        {
            get
            {
                return m_cycleDataGroups[IAIndex];
            }
        }

        public CycleDataGroup IB
        {
            get
            {
                return m_cycleDataGroups[IBIndex];
            }
        }

        public CycleDataGroup IC
        {
            get
            {
                return m_cycleDataGroups[ICIndex];
            }
        }

        public CycleDataGroup IR
        {
            get
            {
                return m_cycleDataGroups[IRIndex];
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

        #endregion
    }
}
