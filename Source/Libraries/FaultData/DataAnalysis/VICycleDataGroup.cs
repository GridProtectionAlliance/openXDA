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
using openXDA.Model;

namespace FaultData.DataAnalysis
{
    public class VICycleDataGroup
    {
        #region [ Members ]

        // Fields
        private List<VIndices> m_vIndices;
        private Asset m_asset;

        private int m_iaIndex;
        private int m_ibIndex;
        private int m_icIndex;
        private int m_irIndex;

        private List<CycleDataGroup> m_cycleDataGroups;

        private class VIndices
        {
            public int Va;
            public int Vb;
            public int Vc;

            public int Vab;
            public int Vbc;
            public int Vca;

            public int distance;
            public VIndices()
            {
                Va = -1;
                Vb = -1;
                Vc = -1;
                Vab = -1;
                Vbc = -1;
                Vca = -1;

                distance = -1;
            }

            public int DefinedNeutralVoltages
            {
                get
                {
                    return ((Va > -1) ? 1 : 0) + ((Vb > -1) ? 1 : 0) + ((Vc > -1) ? 1 : 0);
                }
            }

            public int DefinedLineVoltages
            {
                get
                {
                    return ((Vab > -1) ? 1 : 0) + ((Vbc > -1) ? 1 : 0) + ((Vca > -1) ? 1 : 0);
                }
            }

            public bool allVoltagesDefined
            {
                get
                {
                    return ((Vab > -1) && (Vbc > -1) && (Vca > -1) &&
                        (Va > -1) && (Vb > -1) && (Vc > -1));
                }
            }

        }


        #endregion

        #region [ Constructors ]

        public VICycleDataGroup(DataGroup dataGroup)
        {
            m_vIndices = new List<VIndices>();
            m_asset = dataGroup.Asset;

            m_cycleDataGroups = dataGroup.DataSeries
                .Select((dataSeries, index) => new { DataSeries = dataSeries, Index = index })
                .GroupBy(obj => obj.Index / 4)
                .Where(grouping => grouping.Count() >= 4)
                .Select(grouping => grouping.Select(obj => obj.DataSeries))
                .Select(grouping => new CycleDataGroup(new DataGroup(grouping, dataGroup.Asset), dataGroup.Asset))
                .ToList();

            MapIndexes();
        }

        public VICycleDataGroup(List<CycleDataGroup> cycleDataGroups, Asset asset)
        {
            m_vIndices = new List<VIndices>();
            m_cycleDataGroups = new List<CycleDataGroup>(cycleDataGroups);
            m_asset = asset;
            MapIndexes();
        }

        #endregion

        #region [ Properties ]

        public CycleDataGroup VA
        {
            get
            {
                return (m_vIndices[0].Va >= 0) ? m_cycleDataGroups[m_vIndices[0].Va] : null;
            }
        }

        public CycleDataGroup VB
        {
            get
            {
                return (m_vIndices[0].Vb >= 0) ? m_cycleDataGroups[m_vIndices[0].Vb] : null;
            }
        }

        public CycleDataGroup VC
        {
            get
            {
                return (m_vIndices[0].Vc >= 0) ? m_cycleDataGroups[m_vIndices[0].Vc] : null;
            }
        }

        public CycleDataGroup VAB
        {
            get
            {
                return (m_vIndices[0].Vab >= 0) ? m_cycleDataGroups[m_vIndices[0].Vab] : null;
            }
        }

        public CycleDataGroup VBC
        {
            get
            {
                return (m_vIndices[0].Vbc >= 0) ? m_cycleDataGroups[m_vIndices[0].Vbc] : null;
            }
        }

        public CycleDataGroup VCA
        {
            get
            {
                return (m_vIndices[0].Vca >= 0) ? m_cycleDataGroups[m_vIndices[0].Vca] : null;
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
                .ToList(), m_asset);
        }

        public VICycleDataGroup ToSubSet(DateTime startTime, DateTime endTime)
        {
            return new VICycleDataGroup(m_cycleDataGroups
                .Select(cycleDataGroup => cycleDataGroup.ToSubGroup(startTime, endTime))
                .ToList(), m_asset);
        }

        public void PushDataTo(CycleDataSet cycleDataSet)
        {
            FaultAlgorithms.CycleData cycleData;
            Cycle[] cycles;
            CycleDataGroup[] cycleDataGroups;

            cycleDataGroups = new CycleDataGroup[] { VA, VB, VC, IA, IB, IC };
            cycles = new Cycle[cycleDataGroups.Length];

            for (int i = 0; i < VA.ToDataGroup().Samples; i++)
            {
                cycleData = new FaultAlgorithms.CycleData();

                cycles[0] = cycleData.AN.V;
                cycles[1] = cycleData.BN.V;
                cycles[2] = cycleData.CN.V;
                cycles[3] = cycleData.AN.I;
                cycles[4] = cycleData.BN.I;
                cycles[5] = cycleData.CN.I;

                for (int j = 0; j < cycles.Length; j++)
                {
                    if (cycleDataGroups[j] == null)
                        continue;

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
            
            m_iaIndex = -1;
            m_ibIndex = -1;
            m_icIndex = -1;
            m_irIndex = -1;

            List<int> vaIndices = new List<int>();
            List<int> vbIndices = new List<int>();
            List<int> vcIndices = new List<int>();
            List<int> vabIndices = new List<int>();
            List<int> vbcIndices = new List<int>();
            List<int> vcaIndices = new List<int>();

            for (int i = 0; i < m_cycleDataGroups.Count; i++)
            {
                if (isVoltage("AN", m_cycleDataGroups[i]))
                    vaIndices.Add(i);
                else if (isVoltage("BN", m_cycleDataGroups[i]))
                    vbIndices.Add(i);
                else if (isVoltage("CN", m_cycleDataGroups[i]))
                    vcIndices.Add(i);
                else if (isVoltage("AB", m_cycleDataGroups[i]))
                    vabIndices.Add(i);
                else if (isVoltage("BC", m_cycleDataGroups[i]))
                    vbcIndices.Add(i);
                else if (isVoltage("CA", m_cycleDataGroups[i]))
                    vcaIndices.Add(i);
                
            }

            //Walk through all Va and try to get corresponding Vb and Vc...
            List<int?> ProcessedIndices = new List<int?>();
            foreach (int? VaIndex in vaIndices)
            {
                int assetID = m_cycleDataGroups[(int)VaIndex].Asset.ID;

                int VbIndex = vbIndices.Cast<int?>().FirstOrDefault(i => m_cycleDataGroups[(int)i].Asset.ID == assetID && !ProcessedIndices.Contains(i)) ?? -1;
                int VcIndex = vcIndices.Cast<int?>().FirstOrDefault(i => m_cycleDataGroups[(int)i].Asset.ID == assetID && !ProcessedIndices.Contains(i)) ?? -1;
                int VabIndex = vabIndices.Cast<int?>().FirstOrDefault(i => m_cycleDataGroups[(int)i].Asset.ID == assetID && !ProcessedIndices.Contains(i)) ?? -1;
                int VbcIndex = vbcIndices.Cast<int?>().FirstOrDefault(i => m_cycleDataGroups[(int)i].Asset.ID == assetID && !ProcessedIndices.Contains(i)) ?? -1;
                int VcaIndex = vcaIndices.Cast<int?>().FirstOrDefault(i => m_cycleDataGroups[(int)i].Asset.ID == assetID && !ProcessedIndices.Contains(i)) ?? -1;

                VIndices set = new VIndices();
                ProcessedIndices.Add(VaIndex);
                set.Va = (int)VaIndex;

                if (VbIndex > -1)
                {
                    ProcessedIndices.Add(VbIndex);
                    set.Vb = VbIndex;
                }
                if (VcIndex > -1)
                {
                    ProcessedIndices.Add(VcIndex);
                    set.Vc = VcIndex;
                }

                if (VabIndex > -1)
                {
                    ProcessedIndices.Add(VabIndex);
                    set.Vab = VabIndex;
                }
                if (VbcIndex > -1)
                {
                    ProcessedIndices.Add(VbcIndex);
                    set.Vbc = VbcIndex;
                }
                if (VcaIndex > -1)
                {
                    ProcessedIndices.Add(VcaIndex);
                    set.Vca = VcaIndex;
                }


                if (assetID == m_asset.ID)
                {
                    set.distance = 0;
                }
                else
                {
                    set.distance = m_asset.DistanceToAsset(assetID);
                }

                m_vIndices.Add(set);
            }

            // Also walk though all Vab to catch Leftover Cases where Va is not present
            foreach (int? VabIndex in vabIndices)
            {
                int assetID = m_cycleDataGroups[(int)VabIndex].Asset.ID;

                int VaIndex = vaIndices.Cast<int?>().FirstOrDefault(i => m_cycleDataGroups[(int)i].Asset.ID == assetID && !ProcessedIndices.Contains(i)) ?? -1;
                int VbIndex = vbIndices.Cast<int?>().FirstOrDefault(i => m_cycleDataGroups[(int)i].Asset.ID == assetID && !ProcessedIndices.Contains(i)) ?? -1;
                int VcIndex = vcIndices.Cast<int?>().FirstOrDefault(i => m_cycleDataGroups[(int)i].Asset.ID == assetID && !ProcessedIndices.Contains(i)) ?? -1;
                
                int VbcIndex = vbcIndices.Cast<int?>().FirstOrDefault(i => m_cycleDataGroups[(int)i].Asset.ID == assetID && !ProcessedIndices.Contains(i)) ?? -1;
                int VcaIndex = vcaIndices.Cast<int?>().FirstOrDefault(i => m_cycleDataGroups[(int)i].Asset.ID == assetID && !ProcessedIndices.Contains(i)) ?? -1;

                VIndices set = new VIndices();
                ProcessedIndices.Add(VabIndex);
                set.Vab = (int)VabIndex;

                if (VbIndex > -1)
                {
                    ProcessedIndices.Add(VbIndex);
                    set.Vb = VbIndex;
                }
                if (VcIndex > -1)
                {
                    ProcessedIndices.Add(VcIndex);
                    set.Vc = VcIndex;
                }

                if (VaIndex > -1)
                {
                    ProcessedIndices.Add(VaIndex);
                    set.Va = VaIndex;
                }
                if (VbcIndex > -1)
                {
                    ProcessedIndices.Add(VbcIndex);
                    set.Vbc = VbcIndex;
                }
                if (VcaIndex > -1)
                {
                    ProcessedIndices.Add(VcaIndex);
                    set.Vca = VcaIndex;
                }


                if (assetID == m_asset.ID)
                {
                    set.distance = 0;
                }
                else
                {
                    set.distance = m_asset.DistanceToAsset(assetID);
                }

                m_vIndices.Add(set);
            }

            for (int i = 0; i < m_cycleDataGroups.Count; i++)
            {
                string measurementType = m_cycleDataGroups[i].RMS.SeriesInfo.Channel.MeasurementType.Name;
                string phase = m_cycleDataGroups[i].RMS.SeriesInfo.Channel.Phase.Name;

                
                if (measurementType == "Current" && phase == "AN")
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

        #region [ Static ]

        // Static Methods
        
        private static bool isVoltage(string phase, CycleDataGroup dataGroup)
        {

            string measurementType = dataGroup.RMS.SeriesInfo.Channel.MeasurementType.Name;
            string seriesPhase = dataGroup.RMS.SeriesInfo.Channel.Phase.Name;

            if (measurementType != "Voltage")
                return false;

            if (seriesPhase != phase)
                return false;

            return true;

        }

        private static bool isCurrent(string phase, CycleDataGroup dataGroup)
        {
            string measurementType = dataGroup.RMS.SeriesInfo.Channel.MeasurementType.Name;
            string seriesPhase = dataGroup.RMS.SeriesInfo.Channel.Phase.Name;

            if (measurementType != "Current")
                return false;

            if (seriesPhase != phase)
                return false;

            return true;

        }

        #endregion

    }
}
