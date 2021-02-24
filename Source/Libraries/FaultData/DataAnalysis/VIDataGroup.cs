//******************************************************************************************************
//  VIDataGroup.cs - Gbtc
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
using GSF.Data;
using GSF.Data.Model;
using openXDA.Model;

namespace FaultData.DataAnalysis
{
    public class VIDataGroup
    {
        #region [ Members ]

        // Fields
        private List<VIndices> m_vIndices;
       
        private int m_iaIndex;
        private int m_ibIndex;
        private int m_icIndex;
        private int m_irIndex;


        private DataGroup m_dataGroup;

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
                    return ((Va > -1)? 1: 0) + ((Vb > -1) ? 1 : 0) + ((Vc > -1) ? 1 : 0);
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

        public VIDataGroup(DataGroup dataGroup)
        {

            // Initialize each of
            // the indexes to -1
            m_vIndices = new List<VIndices>();

            m_iaIndex = -1;
            m_ibIndex = -1;
            m_icIndex = -1;
            m_irIndex = -1;

            // Initialize the data group
            m_dataGroup = new DataGroup(dataGroup.DataSeries, dataGroup.Asset);

           
            // List of Indices matching channel type
            List<int> vaIndices = new List<int>();
            List<int> vbIndices = new List<int>();
            List<int> vcIndices = new List<int>();
            List<int> vabIndices = new List<int>();
            List<int> vbcIndices = new List<int>();
            List<int> vcaIndices = new List<int>();

            List<int> iaIndices = new List<int>();
            List<int> ibIndices = new List<int>();
            List<int> icIndices = new List<int>();
            List<int> iresIndices = new List<int>();

            for (int i = 0; i < dataGroup.DataSeries.Count; i++)
            {
                if (isVoltage("AN", dataGroup[i]))
                    vaIndices.Add(i);
                else if (isVoltage("BN", dataGroup[i]))
                    vbIndices.Add(i);
                else if (isVoltage("CN", dataGroup[i]))
                    vcIndices.Add(i);
                else if (isVoltage("AB", dataGroup[i]))
                    vabIndices.Add(i);
                else if (isVoltage("BC", dataGroup[i]))
                    vbcIndices.Add(i);
                else if (isVoltage("CA", dataGroup[i]))
                    vcaIndices.Add(i);
                else if (isCurrent("AN", dataGroup[i]))
                    iaIndices.Add(i);
                else if (isCurrent("BN", dataGroup[i]))
                    ibIndices.Add(i);
                else if (isCurrent("CN", dataGroup[i]))
                    icIndices.Add(i);
                else if (isCurrent("RES", dataGroup[i]))
                    iresIndices.Add(i);
            }

            //Walk through all Va and try to get corresponding Vb and Vc...
            List<int?> ProcessedIndices = new List<int?>();
            foreach(int? VaIndex in vaIndices)
            {
                int assetID = dataGroup[(int)VaIndex].SeriesInfo.Channel.AssetID;
                int VbIndex = vbIndices.Cast<int?>().FirstOrDefault(i => dataGroup[(int)i].SeriesInfo.Channel.AssetID == assetID && !ProcessedIndices.Contains(i)) ?? -1;
                int VcIndex = vcIndices.Cast<int?>().FirstOrDefault(i => dataGroup[(int)i].SeriesInfo.Channel.AssetID == assetID && !ProcessedIndices.Contains(i)) ?? -1;
                int VabIndex = vabIndices.Cast<int?>().FirstOrDefault(i => dataGroup[(int)i].SeriesInfo.Channel.AssetID == assetID && !ProcessedIndices.Contains(i)) ?? -1;
                int VbcIndex = vbcIndices.Cast<int?>().FirstOrDefault(i => dataGroup[(int)i].SeriesInfo.Channel.AssetID == assetID && !ProcessedIndices.Contains(i)) ?? -1;
                int VcaIndex = vcaIndices.Cast<int?>().FirstOrDefault(i => dataGroup[(int)i].SeriesInfo.Channel.AssetID == assetID && !ProcessedIndices.Contains(i)) ?? -1;

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


                if (assetID == dataGroup.Asset.ID)
                {
                    set.distance = 0;
                }
                else
                {
                    set.distance = dataGroup.Asset.DistanceToAsset(assetID);
                }

                m_vIndices.Add(set);
            }

            // Also walk though all Vab to catch Leftover Cases where Va is not present
            foreach (int? VabIndex in vabIndices)
            {
                int assetID = dataGroup[(int)VabIndex].SeriesInfo.Channel.AssetID;

                int VaIndex = vaIndices.Cast<int?>().FirstOrDefault(i => dataGroup[(int)i].SeriesInfo.Channel.AssetID == assetID && !ProcessedIndices.Contains(i)) ?? -1;
                int VbIndex = vbIndices.Cast<int?>().FirstOrDefault(i => dataGroup[(int)i].SeriesInfo.Channel.AssetID == assetID && !ProcessedIndices.Contains(i)) ?? -1;
                int VcIndex = vcIndices.Cast<int?>().FirstOrDefault(i => dataGroup[(int)i].SeriesInfo.Channel.AssetID == assetID && !ProcessedIndices.Contains(i)) ?? -1;

                int VbcIndex = vbcIndices.Cast<int?>().FirstOrDefault(i => dataGroup[(int)i].SeriesInfo.Channel.AssetID == assetID && !ProcessedIndices.Contains(i)) ?? -1;
                int VcaIndex = vcaIndices.Cast<int?>().FirstOrDefault(i => dataGroup[(int)i].SeriesInfo.Channel.AssetID == assetID && !ProcessedIndices.Contains(i)) ?? -1;

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


                if (assetID == dataGroup.Asset.ID)
                {
                    set.distance = 0;
                }
                else
                {
                    set.distance = dataGroup.Asset.DistanceToAsset(assetID);
                }

                m_vIndices.Add(set);
            }

            // Start by matching 
            //Check if a voltage and current channnel exist on this asset

            if (iaIndices.Where(i => dataGroup[i].SeriesInfo.Channel.AssetID == dataGroup.Asset.ID).Count() > 0)
                m_iaIndex = iaIndices.Where(i => dataGroup[i].SeriesInfo.Channel.AssetID == dataGroup.Asset.ID).First();
            if (ibIndices.Where(i => dataGroup[i].SeriesInfo.Channel.AssetID == dataGroup.Asset.ID).Count() > 0)
                m_ibIndex = ibIndices.Where(i => dataGroup[i].SeriesInfo.Channel.AssetID == dataGroup.Asset.ID).First();
            if (icIndices.Where(i => dataGroup[i].SeriesInfo.Channel.AssetID == dataGroup.Asset.ID).Count() > 0)
                m_icIndex = icIndices.Where(i => dataGroup[i].SeriesInfo.Channel.AssetID == dataGroup.Asset.ID).First();

            if (iresIndices.Where(i => dataGroup[i].SeriesInfo.Channel.AssetID == dataGroup.Asset.ID).Count() > 0)
                m_irIndex = iresIndices.Where(i => dataGroup[i].SeriesInfo.Channel.AssetID == dataGroup.Asset.ID).First();

            // use one of the connected Assets
            List<int> connectedAssets = dataGroup.Asset.ConnectedAssets.Select(item => item.ID).ToList();

            

            if (m_iaIndex == -1 && iaIndices.Where(i => connectedAssets.Contains(dataGroup[i].SeriesInfo.Channel.AssetID)).Count() > 0)
                m_iaIndex = iaIndices.Where(i => connectedAssets.Contains(dataGroup[i].SeriesInfo.Channel.AssetID)).First();
            if (m_ibIndex == -1 && ibIndices.Where(i => connectedAssets.Contains(dataGroup[i].SeriesInfo.Channel.AssetID)).Count() > 0)
                m_ibIndex = ibIndices.Where(i => connectedAssets.Contains(dataGroup[i].SeriesInfo.Channel.AssetID)).First();
            if (m_icIndex == -1 && icIndices.Where(i => connectedAssets.Contains(dataGroup[i].SeriesInfo.Channel.AssetID)).Count() > 0)
                m_icIndex = icIndices.Where(i => connectedAssets.Contains(dataGroup[i].SeriesInfo.Channel.AssetID)).First();

            if (m_irIndex == -1 && iresIndices.Where(i => connectedAssets.Contains(dataGroup[i].SeriesInfo.Channel.AssetID)).Count() > 0)
                m_irIndex = iresIndices.Where(i => connectedAssets.Contains(dataGroup[i].SeriesInfo.Channel.AssetID)).First();

            
            if (m_iaIndex == -1 && iaIndices.Count() > 0)
                m_iaIndex = iaIndices.First();
            if (m_ibIndex == -1 && ibIndices.Count() > 0)
                m_ibIndex = ibIndices.First();
            if (m_ibIndex == -1 && ibIndices.Count() > 0)
                m_ibIndex = icIndices.First();

            if (m_irIndex == -1 && iresIndices.Count() > 0)
                m_irIndex = iresIndices.First();


            if (m_vIndices.Count() == 0)
                m_vIndices.Add(new VIndices());

            CalculateMissingCurrentChannel();
            CalculateMissingLLVoltageChannels();

            //sort by all available Channels and number of Hops to actual Asset
            m_vIndices.Sort((a, b) => 
                {
                    if (b.allVoltagesDefined && !a.allVoltagesDefined)
                        return 1;
                    if (a.allVoltagesDefined && !b.allVoltagesDefined)
                        return -1;
                    return a.distance.CompareTo(b.distance);
                });
        }

        private VIDataGroup()
        {
        }

        #endregion

        #region [ Properties ]

        public DataSeries VA
        {
            get
            {
                return (m_vIndices[0].Va >= 0) ? m_dataGroup[m_vIndices[0].Va] : null;
            }
        }

        public DataSeries VB
        {
            get
            {
                return (m_vIndices[0].Vb >= 0) ? m_dataGroup[m_vIndices[0].Vb] : null;
            }
        }

        public DataSeries VC
        {
            get
            {
                return (m_vIndices[0].Vc >= 0) ? m_dataGroup[m_vIndices[0].Vc] : null;
            }
        }

        public DataSeries VAB
        {
            get
            {
                return (m_vIndices[0].Vab >= 0) ? m_dataGroup[m_vIndices[0].Vab] : null;
            }
        }

        public DataSeries VBC
        {
            get
            {
                return (m_vIndices[0].Vbc >= 0) ? m_dataGroup[m_vIndices[0].Vbc] : null;
            }
        }

        public DataSeries VCA
        {
            get
            {
                return (m_vIndices[0].Vca >= 0) ? m_dataGroup[m_vIndices[0].Vca] : null;
            }
        }

        public DataSeries IA
        {
            get
            {
                return (m_iaIndex >= 0) ? m_dataGroup[m_iaIndex] : null;
            }
        }

        public DataSeries IB
        {
            get
            {
                return (m_ibIndex >= 0) ? m_dataGroup[m_ibIndex] : null;
            }
        }

        public DataSeries IC
        {
            get
            {
                return (m_icIndex >= 0) ? m_dataGroup[m_icIndex] : null;
            }
        }

        public DataSeries IR
        {
            get
            {
                return (m_irIndex >= 0) ? m_dataGroup[m_irIndex] : null;
            }
        }

        public int DefinedNeutralVoltages
        {
            get
            {
                return m_vIndices.Select(item => item.DefinedNeutralVoltages).Sum();
            }
        }

        public int DefinedLineVoltages
        {
            get
            {
                return m_vIndices.Select(item => item.DefinedLineVoltages).Sum();
            }
        }

        public int DefinedCurrents
        {
            get
            {
                return CurrentIndexes.Count(index => index >= 0);
            }
        }

        public bool AllVIChannelsDefined
        {
            get
            {
                return m_vIndices[0].allVoltagesDefined &&
                    CurrentIndexes.All(index => index >= 0);
            }
        }

        private int[] CurrentIndexes
        {
            get
            {
                return new int[] { m_iaIndex, m_ibIndex, m_icIndex, m_irIndex };
            }
        }

        public Asset Asset
        {
            get
            {
                return m_dataGroup.Asset;
            }
        }
        #endregion

        public DataSeries[] Data
        {
            get
            {
                List<DataSeries> result = new List<DataSeries>();
                foreach (VIndices Vindex in m_vIndices)
                {
                    if (Vindex.Va > -1)
                        result.Add(m_dataGroup[Vindex.Va]);
                    if (Vindex.Vb > -1)
                        result.Add(m_dataGroup[Vindex.Vb]);
                    if (Vindex.Vc > -1)
                        result.Add(m_dataGroup[Vindex.Vc]);

                    if (Vindex.Vab > -1)
                        result.Add(m_dataGroup[Vindex.Vab]);
                    if (Vindex.Vbc > -1)
                        result.Add(m_dataGroup[Vindex.Vbc]);
                    if (Vindex.Vca > -1)
                        result.Add(m_dataGroup[Vindex.Vca]);


                }
                
                if (m_iaIndex > -1)
                    result.Add(m_dataGroup[m_iaIndex]);
                if (m_ibIndex > -1)
                    result.Add(m_dataGroup[m_ibIndex]);
                if (m_icIndex > -1)
                    result.Add(m_dataGroup[m_icIndex]);
                if (m_irIndex > -1)
                    result.Add(m_dataGroup[m_irIndex]);
                return result.ToArray();

            }
        }
        #region [ Methods ]

        /// <summary>
        /// Given three of the four current channels, calculates the
        /// missing channel based on the relationship IR = IA + IB + IC.
        /// </summary>
        private void CalculateMissingCurrentChannel()
        {
            Meter meter;
            DataSeries missingSeries;

            // If the data group does not have exactly 3 channels,
            // then there is no missing channel or there is not
            // enough data to calculate the missing channel
            if (DefinedCurrents != 3)
                return;

            // Get the meter associated with the channels in this data group
            meter = (IA ?? IB).SeriesInfo.Channel.Meter;

            if (m_iaIndex == -1)
            {
                // Calculate IA = IR - IB - IC
                missingSeries = IR.Add(IB.Negate()).Add(IC.Negate());
                missingSeries.SeriesInfo = GetSeriesInfo(meter, m_dataGroup, "Current", "AN", IR.SeriesInfo.Channel.AssetID);
                missingSeries.Calculated = true;
                m_iaIndex = m_dataGroup.DataSeries.Count;
                m_dataGroup.Add(missingSeries);
            }
            else if (m_ibIndex == -1)
            {
                // Calculate IB = IR - IA - IC
                missingSeries = IR.Add(IA.Negate()).Add(IC.Negate());
                missingSeries.SeriesInfo = GetSeriesInfo(meter, m_dataGroup, "Current", "BN", IR.SeriesInfo.Channel.AssetID);
                missingSeries.Calculated = true;
                m_ibIndex = m_dataGroup.DataSeries.Count;
                m_dataGroup.Add(missingSeries);
            }
            else if (m_icIndex == -1)
            {
                // Calculate IC = IR - IA - IB
                missingSeries = IR.Add(IA.Negate()).Add(IB.Negate());
                missingSeries.SeriesInfo = GetSeriesInfo(meter, m_dataGroup, "Current", "CN", IR.SeriesInfo.Channel.AssetID);
                missingSeries.Calculated = true;
                m_icIndex = m_dataGroup.DataSeries.Count;
                m_dataGroup.Add(missingSeries);
            }
            else
            {
                // Calculate IR = IA + IB + IC
                missingSeries = IA.Add(IB).Add(IC);
                missingSeries.SeriesInfo = GetSeriesInfo(meter, m_dataGroup, "Current", "RES", IA.SeriesInfo.Channel.AssetID);
                missingSeries.Calculated = true;
                m_irIndex = m_dataGroup.DataSeries.Count;
                m_dataGroup.Add(missingSeries);
            }
        }

        private void CalculateMissingLLVoltageChannels()
        {
            Meter meter;
            DataSeries missingSeries;

            //Do this for every Voltage set
            for (int i = 0; i < m_vIndices.Count(); i++)
            {
                // If all line voltages are already present or there are not
                // at least 2 lines we will not perform line to line calculations
                if (m_vIndices[i].DefinedLineVoltages == 3 || m_vIndices[i].DefinedNeutralVoltages < 2)
                    return;

                // Get the meter associated with the channels in this data group
                DataSeries VA = null;
                DataSeries VB = null;
                DataSeries VC = null;

                if  (m_vIndices[i].Va  > -1)
                    VA = m_dataGroup[m_vIndices[i].Va];
                if (m_vIndices[i].Vb > -1)
                    VB = m_dataGroup[m_vIndices[i].Vb];
                if (m_vIndices[i].Vc > -1)
                    VC = m_dataGroup[m_vIndices[i].Vc];

                meter = (VA ?? VB ?? VC).SeriesInfo.Channel.Meter;

                if (m_vIndices[i].Vab == -1 && VA != null && VB != null)
                {
                    // Calculate VAB = VA - VB
                    missingSeries = VA.Add(VB.Negate());
                    missingSeries.SeriesInfo = GetSeriesInfo(meter, m_dataGroup, "Voltage", "AB", VA.SeriesInfo.Channel.AssetID);
                    missingSeries.Calculated = true;
                    m_vIndices[i].Vab = m_dataGroup.DataSeries.Count;
                    m_dataGroup.Add(missingSeries);
                }

                if (m_vIndices[i].Vbc == -1 && VB != null && VC != null)
                {
                    // Calculate VBC = VB - VC
                    missingSeries = VB.Add(VC.Negate());
                    missingSeries.SeriesInfo = GetSeriesInfo(meter, m_dataGroup, "Voltage", "BC", VB.SeriesInfo.Channel.AssetID);
                    missingSeries.Calculated = true;
                    m_vIndices[i].Vbc = m_dataGroup.DataSeries.Count;
                    m_dataGroup.Add(missingSeries);
                }

                if (m_vIndices[i].Vca == -1 && VC != null && VA != null)
                {
                    // Calculate VCA = VC - VA
                    missingSeries = VC.Add(VA.Negate());
                    missingSeries.SeriesInfo = GetSeriesInfo(meter, m_dataGroup, "Voltage", "CA", VC.SeriesInfo.Channel.AssetID);
                    missingSeries.Calculated = true;
                    m_vIndices[i].Vca = m_dataGroup.DataSeries.Count;
                    m_dataGroup.Add(missingSeries);
                }
            }

        }

        public DataGroup ToDataGroup()
        {
            return new DataGroup(m_dataGroup.DataSeries, m_dataGroup.Asset);
        }

        public VIDataGroup ToSubGroup(int startIndex, int endIndex)
        {
            VIDataGroup subGroup = new VIDataGroup();

            subGroup.m_vIndices = m_vIndices;
            subGroup.m_iaIndex = m_iaIndex;
            subGroup.m_ibIndex = m_ibIndex;
            subGroup.m_icIndex = m_icIndex;
            subGroup.m_irIndex = m_irIndex;

            subGroup.m_dataGroup = m_dataGroup.ToSubGroup(startIndex, endIndex);

            return subGroup;
        }

        public VIDataGroup ToSubGroup(DateTime startTime, DateTime endTime)
        {
            VIDataGroup subGroup = new VIDataGroup();

            subGroup.m_vIndices = m_vIndices;
            subGroup.m_iaIndex = m_iaIndex;
            subGroup.m_ibIndex = m_ibIndex;
            subGroup.m_icIndex = m_icIndex;
            subGroup.m_irIndex = m_irIndex;

            subGroup.m_dataGroup = m_dataGroup.ToSubGroup(startTime, endTime);

            return subGroup;
        }

        #endregion

        #region [ Static ]

        // Static Methods
        private static Series GetSeriesInfo(Meter meter, DataGroup dataGroup, string measurementTypeName, string phaseName, int assetID)
        {
            string measurementCharacteristicName = "Instantaneous";
            string seriesTypeName = "Values";

            char typeDesignation = (measurementTypeName == "Current") ? 'I' : measurementTypeName[0];
            string phaseDesignation = (phaseName == "RES") ? "R" : phaseName.TrimEnd('N');
            string channelName = string.Concat(typeDesignation, phaseDesignation);

            ChannelKey channelKey = new ChannelKey(assetID, 0, channelName, measurementTypeName, measurementCharacteristicName, phaseName);
            SeriesKey seriesKey = new SeriesKey(channelKey, seriesTypeName);

            Series dbSeries = meter.Channels
                .SelectMany(channel => channel.Series)
                .FirstOrDefault(series => seriesKey.Equals(new SeriesKey(series)));

            if ((object)dbSeries == null)
            {
                Channel dbChannel = meter.Channels
                    .FirstOrDefault(channel => channelKey.Equals(new ChannelKey(channel)));

                //need to get Asset based on the asset of the datagroup or the connected assets 
                Asset asset = dataGroup.Asset;
                if (asset.ID != assetID)
                {
                    List<Asset> connectedAssets = asset.ConnectedAssets.ToList();
                    asset = connectedAssets.Find(item => item.ID == assetID);
                }

                if ((object)dbChannel == null)
                {
                    MeasurementType measurementType = new MeasurementType() { Name = measurementTypeName };
                    MeasurementCharacteristic measurementCharacteristic = new MeasurementCharacteristic() { Name = measurementCharacteristicName };
                    Phase phase = new Phase() { Name = phaseName };

                    dbChannel = new Channel()
                    {
                        MeterID = meter.ID,
                        AssetID = assetID,
                        MeasurementTypeID = measurementType.ID,
                        MeasurementCharacteristicID = measurementCharacteristic.ID,
                        PhaseID = phase.ID,
                        Name = channelKey.Name,
                        SamplesPerHour = dataGroup.SamplesPerHour,
                        Description = string.Concat(measurementCharacteristicName, " ", measurementTypeName, " ", phaseName),
                        Enabled = true,

                        Meter = meter,
                        Asset = asset,
                        MeasurementType = measurementType,
                        MeasurementCharacteristic = measurementCharacteristic,
                        Phase = phase,
                        Series = new List<Series>()
                    };

                    meter.Channels.Add(dbChannel);
                }

                SeriesType seriesType = new SeriesType() { Name = seriesTypeName };

                dbSeries = new Series()
                {
                    ChannelID = dbChannel.ID,
                    SeriesTypeID = seriesType.ID,
                    SourceIndexes = string.Empty,

                    Channel = dbChannel,
                    SeriesType = seriesType
                };

                dbChannel.Series.Add(dbSeries);
            }

            return dbSeries;
        }

        private static bool isVoltage(string phase, DataSeries dataSeries)
        {
            string[] instantaneousSeriesTypes = { "Values", "Instantaneous" };

            string measurementType = dataSeries.SeriesInfo.Channel.MeasurementType.Name;
            string measurementCharacteristic = dataSeries.SeriesInfo.Channel.MeasurementCharacteristic.Name;
            string seriesType = dataSeries.SeriesInfo.SeriesType.Name;
            string seriesPhase = dataSeries.SeriesInfo.Channel.Phase.Name;

            if (measurementCharacteristic != "Instantaneous")
                return false;

            if (!instantaneousSeriesTypes.Contains(seriesType))
                return false;

            if (measurementType != "Voltage")
                return false;

            if (seriesPhase != phase)
                return false;

            return true;

        }

        private static bool isCurrent(string phase, DataSeries dataSeries)
        {
            string[] instantaneousSeriesTypes = { "Values", "Instantaneous" };

            string measurementType = dataSeries.SeriesInfo.Channel.MeasurementType.Name;
            string measurementCharacteristic = dataSeries.SeriesInfo.Channel.MeasurementCharacteristic.Name;
            string seriesType = dataSeries.SeriesInfo.SeriesType.Name;
            string seriesPhase = dataSeries.SeriesInfo.Channel.Phase.Name;

            if (measurementCharacteristic != "Instantaneous")
                return false;

            if (!instantaneousSeriesTypes.Contains(seriesType))
                return false;

            if (measurementType != "Current")
                return false;

            if (seriesPhase != phase)
                return false;

            return true;

        }

        #endregion
    }
}
