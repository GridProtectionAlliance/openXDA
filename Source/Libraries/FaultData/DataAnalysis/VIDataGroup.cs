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
            public int Va { get; set; } = -1;
            public int Vb { get; set; } = -1;
            public int Vc { get; set; } = -1;

            public int Vab { get; set; } = -1;
            public int Vbc { get; set; } = -1;
            public int Vca { get; set; } = -1;

            public int Distance { get; set; } = -1;

            public int DefinedNeutralVoltages =>
                (Va >= 0 ? 1 : 0) +
                (Vb >= 0 ? 1 : 0) +
                (Vc >= 0 ? 1 : 0);

            public int DefinedLineVoltages =>
                (Vab >= 0 ? 1 : 0) +
                (Vbc >= 0 ? 1 : 0) +
                (Vca >= 0 ? 1 : 0);

            public bool AllVoltagesDefined =>
                (Va >= 0) && (Vb >= 0) && (Vc >= 0) &&
                (Vab >= 0) && (Vbc >= 0) && (Vca >= 0);
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

            HashSet<int> connectedAssets = new HashSet<int>(dataGroup.Asset.ConnectedAssets.Select(item => item.ID));

            var groupings = dataGroup.DataSeries
                .Select((DataSeries, Index) => new { DataSeries, Index })
                .Where(item => !(item.DataSeries.SeriesInfo is null))
                .Where(item => item.DataSeries.SeriesInfo.Channel.MeasurementCharacteristic.Name == "Instantaneous")
                .Where(item => new[] { "Instantaneous", "Values" }.Contains(item.DataSeries.SeriesInfo.SeriesType.Name))
                .GroupBy(item => item.DataSeries.SeriesInfo.Channel.AssetID)
                .OrderBy(grouping => grouping.Key == dataGroup.Asset.ID ? 0 : 1)
                .ThenBy(grouping => connectedAssets.Contains(grouping.Key) ? 0 : 1)
                .ToList();

            foreach (var grouping in groupings)
            {
                VIndices set = new VIndices() { Distance = 0 };

                int assetID = grouping.Key;

                if (assetID != dataGroup.Asset.ID)
                    set.Distance = dataGroup.Asset.DistanceToAsset(assetID);

                foreach (var item in grouping)
                {
                    string measurementType = item.DataSeries.SeriesInfo.Channel.MeasurementType.Name;
                    string phase = item.DataSeries.SeriesInfo.Channel.Phase.Name;

                    if (measurementType == "Voltage" && phase == "AN")
                        set.Va = item.Index;

                    if (measurementType == "Voltage" && phase == "BN")
                        set.Vb = item.Index;

                    if (measurementType == "Voltage" && phase == "CN")
                        set.Vc = item.Index;

                    if (measurementType == "Voltage" && phase == "AB")
                        set.Vab = item.Index;

                    if (measurementType == "Voltage" && phase == "BC")
                        set.Vbc = item.Index;

                    if (measurementType == "Voltage" && phase == "CA")
                        set.Vca = item.Index;

                    if (m_iaIndex < 0 && measurementType == "Current" && phase == "AN")
                        m_iaIndex = item.Index;

                    if (m_ibIndex < 0 && measurementType == "Current" && phase == "BN")
                        m_ibIndex = item.Index;

                    if (m_icIndex < 0 && measurementType == "Current" && phase == "CN")
                        m_icIndex = item.Index;

                    if (m_irIndex < 0 && measurementType == "Current" && phase == "RES")
                        m_irIndex = item.Index;
                }

                m_vIndices.Add(set);
            }

            if (m_vIndices.Count() == 0)
                m_vIndices.Add(new VIndices());

            CalculateMissingCurrentChannel();
            CalculateMissingLLVoltageChannels();

            m_vIndices.Sort((a, b) =>
            {
                if (b.AllVoltagesDefined && !a.AllVoltagesDefined)
                    return 1;
                if (a.AllVoltagesDefined && !b.AllVoltagesDefined)
                    return -1;
                return a.Distance.CompareTo(b.Distance);
            });
        }

        private VIDataGroup()
        {
        }

        #endregion

        #region [ Properties ]

        public DataSeries VA => (m_vIndices[0].Va >= 0)
            ? m_dataGroup[m_vIndices[0].Va]
            : null;

        public DataSeries VB => (m_vIndices[0].Vb >= 0)
            ? m_dataGroup[m_vIndices[0].Vb]
            : null;

        public DataSeries VC => (m_vIndices[0].Vc >= 0)
            ? m_dataGroup[m_vIndices[0].Vc]
            : null;

        public DataSeries VAB => (m_vIndices[0].Vab >= 0)
            ? m_dataGroup[m_vIndices[0].Vab]
            : null;

        public DataSeries VBC => (m_vIndices[0].Vbc >= 0)
            ? m_dataGroup[m_vIndices[0].Vbc]
            : null;

        public DataSeries VCA => (m_vIndices[0].Vca >= 0)
            ? m_dataGroup[m_vIndices[0].Vca]
            : null;

        public DataSeries IA => (m_iaIndex >= 0)
            ? m_dataGroup[m_iaIndex]
            : null;

        public DataSeries IB => (m_ibIndex >= 0)
            ? m_dataGroup[m_ibIndex]
            : null;

        public DataSeries IC => (m_icIndex >= 0)
            ? m_dataGroup[m_icIndex]
            : null;

        public DataSeries IR => (m_irIndex >= 0)
            ? m_dataGroup[m_irIndex]
            : null;

        public int DefinedNeutralVoltages =>
            m_vIndices.Sum(item => item.DefinedNeutralVoltages);

        public int DefinedLineVoltages =>
            m_vIndices.Sum(item => item.DefinedLineVoltages);

        public int DefinedCurrents =>
            CurrentIndexes.Count(index => index >= 0);

        public bool AllVIChannelsDefined =>
            m_vIndices[0].AllVoltagesDefined &&
            CurrentIndexes.All(index => index >= 0);

        private int[] CurrentIndexes =>
            new int[] { m_iaIndex, m_ibIndex, m_icIndex, m_irIndex };

        public Asset Asset => m_dataGroup.Asset;

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

        #endregion

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

                if (m_vIndices[i].Vab == -1 && !(VA is null) && !(VB is null))
                {
                    // Calculate VAB = VA - VB
                    missingSeries = VA.Add(VB.Negate());
                    missingSeries.SeriesInfo = GetSeriesInfo(meter, m_dataGroup, "Voltage", "AB", VA.SeriesInfo.Channel.AssetID);
                    missingSeries.Calculated = true;
                    m_vIndices[i].Vab = m_dataGroup.DataSeries.Count;
                    m_dataGroup.Add(missingSeries);
                }

                if (m_vIndices[i].Vbc == -1 && !(VB is null) && !(VC is null))
                {
                    // Calculate VBC = VB - VC
                    missingSeries = VB.Add(VC.Negate());
                    missingSeries.SeriesInfo = GetSeriesInfo(meter, m_dataGroup, "Voltage", "BC", VB.SeriesInfo.Channel.AssetID);
                    missingSeries.Calculated = true;
                    m_vIndices[i].Vbc = m_dataGroup.DataSeries.Count;
                    m_dataGroup.Add(missingSeries);
                }

                if (m_vIndices[i].Vca == -1 && !(VC is null) && !(VA is null))
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

            Channel dbChannel = meter.Channels
                .FirstOrDefault(channel => channelKey.Equals(new ChannelKey(channel)));

            Series dbSeries = dbChannel?.Series
                .FirstOrDefault(series => seriesKey.Equals(new SeriesKey(series)));

            if (dbSeries is null)
            {
                //need to get Asset based on the asset of the datagroup or the connected assets 
                Asset asset = dataGroup.Asset;
                if (asset.ID != assetID)
                {
                    List<Asset> connectedAssets = asset.ConnectedAssets.ToList();
                    asset = connectedAssets.Find(item => item.ID == assetID);
                }

                if (dbChannel is null)
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

        #endregion
    }
}
