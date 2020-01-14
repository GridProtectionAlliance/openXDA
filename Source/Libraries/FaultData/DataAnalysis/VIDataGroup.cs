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

        private DataGroup m_dataGroup;

        #endregion

        #region [ Constructors ]

        public VIDataGroup(DataGroup dataGroup)
        {

            // Initialize each of
            // the indexes to -1
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

            // Initialize the data group
            m_dataGroup = new DataGroup(dataGroup.DataSeries);

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

            //Check if a voltage and current channnel exist on this asset
            if (vaIndices.Where(i => dataGroup[i].SeriesInfo.Channel.AssetID == dataGroup.Asset.ID).Count() > 0)
                m_vaIndex = vaIndices.Where(i => dataGroup[i].SeriesInfo.Channel.AssetID == dataGroup.Asset.ID).First();
            if (vbIndices.Where(i => dataGroup[i].SeriesInfo.Channel.AssetID == dataGroup.Asset.ID).Count() > 0)
                m_vbIndex = vbIndices.Where(i => dataGroup[i].SeriesInfo.Channel.AssetID == dataGroup.Asset.ID).First();
            if (vcIndices.Where(i => dataGroup[i].SeriesInfo.Channel.AssetID == dataGroup.Asset.ID).Count() > 0)
                m_vcIndex = vcIndices.Where(i => dataGroup[i].SeriesInfo.Channel.AssetID == dataGroup.Asset.ID).First();

            if (vabIndices.Where(i => dataGroup[i].SeriesInfo.Channel.AssetID == dataGroup.Asset.ID).Count() > 0)
                m_vabIndex = vabIndices.Where(i => dataGroup[i].SeriesInfo.Channel.AssetID == dataGroup.Asset.ID).First();
            if (vbcIndices.Where(i => dataGroup[i].SeriesInfo.Channel.AssetID == dataGroup.Asset.ID).Count() > 0)
                m_vbcIndex = vbcIndices.Where(i => dataGroup[i].SeriesInfo.Channel.AssetID == dataGroup.Asset.ID).First();
            if (vcaIndices.Where(i => dataGroup[i].SeriesInfo.Channel.AssetID == dataGroup.Asset.ID).Count() > 0)
                m_vcaIndex = vcaIndices.Where(i => dataGroup[i].SeriesInfo.Channel.AssetID == dataGroup.Asset.ID).First();

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

            if (m_vaIndex == -1 && vaIndices.Where(i => connectedAssets.Contains(dataGroup[i].SeriesInfo.Channel.AssetID)).Count() > 0)
                m_vaIndex = vaIndices.Where(i => connectedAssets.Contains(dataGroup[i].SeriesInfo.Channel.AssetID)).First();
            if (m_vbIndex == -1 && vbIndices.Where(i => connectedAssets.Contains(dataGroup[i].SeriesInfo.Channel.AssetID)).Count() > 0)
                m_vbIndex = vbIndices.Where(i => connectedAssets.Contains(dataGroup[i].SeriesInfo.Channel.AssetID)).First();
            if (m_vcIndex == -1 && vcIndices.Where(i => connectedAssets.Contains(dataGroup[i].SeriesInfo.Channel.AssetID)).Count() > 0)
                m_vcIndex = vcIndices.Where(i => connectedAssets.Contains(dataGroup[i].SeriesInfo.Channel.AssetID)).First();

            if (m_vabIndex == -1 && vabIndices.Where(i => connectedAssets.Contains(dataGroup[i].SeriesInfo.Channel.AssetID)).Count() > 0)
                m_vabIndex = vabIndices.Where(i => connectedAssets.Contains(dataGroup[i].SeriesInfo.Channel.AssetID)).First();
            if (m_vbcIndex == -1 && vbcIndices.Where(i => connectedAssets.Contains(dataGroup[i].SeriesInfo.Channel.AssetID)).Count() > 0)
                m_vbcIndex = vbcIndices.Where(i => connectedAssets.Contains(dataGroup[i].SeriesInfo.Channel.AssetID)).First();
            if (m_vcaIndex == -1 && vcaIndices.Where(i => connectedAssets.Contains(dataGroup[i].SeriesInfo.Channel.AssetID)).Count() > 0)
                m_vcaIndex = vcaIndices.Where(i => connectedAssets.Contains(dataGroup[i].SeriesInfo.Channel.AssetID)).First();

            if (m_iaIndex == -1 && iaIndices.Where(i => connectedAssets.Contains(dataGroup[i].SeriesInfo.Channel.AssetID)).Count() > 0)
                m_iaIndex = iaIndices.Where(i => connectedAssets.Contains(dataGroup[i].SeriesInfo.Channel.AssetID)).First();
            if (m_ibIndex == -1 && ibIndices.Where(i => connectedAssets.Contains(dataGroup[i].SeriesInfo.Channel.AssetID)).Count() > 0)
                m_ibIndex = ibIndices.Where(i => connectedAssets.Contains(dataGroup[i].SeriesInfo.Channel.AssetID)).First();
            if (m_icIndex == -1 && icIndices.Where(i => connectedAssets.Contains(dataGroup[i].SeriesInfo.Channel.AssetID)).Count() > 0)
                m_icIndex = icIndices.Where(i => connectedAssets.Contains(dataGroup[i].SeriesInfo.Channel.AssetID)).First();

            if (m_irIndex == -1 && iresIndices.Where(i => connectedAssets.Contains(dataGroup[i].SeriesInfo.Channel.AssetID)).Count() > 0)
                m_irIndex = iresIndices.Where(i => connectedAssets.Contains(dataGroup[i].SeriesInfo.Channel.AssetID)).First();

            // use any assets more than one hop away last
            if (m_vaIndex == -1 && vaIndices.Count() > 0)
                m_vaIndex = vaIndices.First();
            if (m_vbIndex == -1 && vbIndices.Count() > 0)
                m_vbIndex = vbIndices.First();
            if (m_vcIndex == -1 && vcIndices.Count() > 0)
                m_vcIndex = vcIndices.First();

            if (m_vabIndex == -1 && vabIndices.Count() > 0)
                m_vabIndex = vabIndices.First();
            if (m_vbcIndex == -1 && vbcIndices.Count() > 0)
                m_vbcIndex = vbcIndices.First();
            if (m_vcaIndex == -1 && vcaIndices.Count() > 0)
                m_vcaIndex = vcaIndices.First();

            if (m_iaIndex == -1 && iaIndices.Count() > 0)
                m_iaIndex = iaIndices.First();
            if (m_ibIndex == -1 && ibIndices.Count() > 0)
                m_ibIndex = ibIndices.First();
            if (m_ibIndex == -1 && ibIndices.Count() > 0)
                m_ibIndex = icIndices.First();

            if (m_irIndex == -1 && iresIndices.Count() > 0)
                m_irIndex = iresIndices.First();


            CalculateMissingCurrentChannel();
            CalculateMissingLLVoltageChannels();
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
                return (m_vaIndex >= 0) ? m_dataGroup[m_vaIndex] : null;
            }
        }

        public DataSeries VB
        {
            get
            {
                return (m_vbIndex >= 0) ? m_dataGroup[m_vbIndex] : null;
            }
        }

        public DataSeries VC
        {
            get
            {
                return (m_vcIndex >= 0) ? m_dataGroup[m_vcIndex] : null;
            }
        }

        public DataSeries VAB
        {
            get
            {
                return (m_vabIndex >= 0) ? m_dataGroup[m_vabIndex] : null;
            }
        }

        public DataSeries VBC
        {
            get
            {
                return (m_vbcIndex >= 0) ? m_dataGroup[m_vbcIndex] : null;
            }
        }

        public DataSeries VCA
        {
            get
            {
                return (m_vcaIndex >= 0) ? m_dataGroup[m_vcaIndex] : null;
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
                return NeutralVoltageIndexes.Count(index => index >= 0);
            }
        }

        public int DefinedLineVoltages
        {
            get
            {
                return LineVoltageIndexes.Count(index => index >= 0);
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
                return NeutralVoltageIndexes
                    .Concat(CurrentIndexes)
                    .All(index => index >= 0);
            }
        }

        private int[] NeutralVoltageIndexes
        {
            get
            {
                return new int[] { m_vaIndex, m_vbIndex, m_vcIndex };
            }
        }

        private int[] LineVoltageIndexes
        {
            get
            {
                return new int[] { m_vabIndex, m_vbcIndex, m_vcaIndex };
            }
        }

        private int[] CurrentIndexes
        {
            get
            {
                return new int[] { m_iaIndex, m_ibIndex, m_icIndex, m_irIndex };
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
                missingSeries.SeriesInfo = GetSeriesInfo(meter, m_dataGroup, "Current", "AN");
                missingSeries.Calculated = true;
                m_iaIndex = m_dataGroup.DataSeries.Count;
                m_dataGroup.Add(missingSeries);
            }
            else if (m_ibIndex == -1)
            {
                // Calculate IB = IR - IA - IC
                missingSeries = IR.Add(IA.Negate()).Add(IC.Negate());
                missingSeries.SeriesInfo = GetSeriesInfo(meter, m_dataGroup, "Current", "BN");
                missingSeries.Calculated = true;
                m_ibIndex = m_dataGroup.DataSeries.Count;
                m_dataGroup.Add(missingSeries);
            }
            else if (m_icIndex == -1)
            {
                // Calculate IC = IR - IA - IB
                missingSeries = IR.Add(IA.Negate()).Add(IB.Negate());
                missingSeries.SeriesInfo = GetSeriesInfo(meter, m_dataGroup, "Current", "CN");
                missingSeries.Calculated = true;
                m_icIndex = m_dataGroup.DataSeries.Count;
                m_dataGroup.Add(missingSeries);
            }
            else
            {
                // Calculate IR = IA + IB + IC
                missingSeries = IA.Add(IB).Add(IC);
                missingSeries.SeriesInfo = GetSeriesInfo(meter, m_dataGroup, "Current", "RES");
                missingSeries.Calculated = true;
                m_irIndex = m_dataGroup.DataSeries.Count;
                m_dataGroup.Add(missingSeries);
            }
        }

        private void CalculateMissingLLVoltageChannels()
        {
            Meter meter;
            DataSeries missingSeries;

            // If all line voltages are already present or there are not
            // atleast 2 lines we will not perform line to line calculations
            if ( DefinedLineVoltages == 3 || DefinedNeutralVoltages < 2 )
                return;

            // Get the meter associated with the channels in this data group
            meter = (VA ?? VB ?? VC).SeriesInfo.Channel.Meter;

            if (m_vabIndex == -1 && VA != null && VB != null)
            {
                // Calculate VAB = VA - VB
                missingSeries = VA.Add(VB.Negate());
                missingSeries.SeriesInfo = GetSeriesInfo(meter, m_dataGroup, "Voltage", "AB");
                missingSeries.Calculated = true;
                m_vabIndex = m_dataGroup.DataSeries.Count;
                m_dataGroup.Add(missingSeries);
            }

            if (m_vbcIndex == -1 && VB != null && VC != null)
            {
                // Calculate VBC = VB - VC
                missingSeries = VB.Add(VC.Negate());
                missingSeries.SeriesInfo = GetSeriesInfo(meter, m_dataGroup, "Voltage", "BC");
                missingSeries.Calculated = true;
                m_vbcIndex = m_dataGroup.DataSeries.Count;
                m_dataGroup.Add(missingSeries);
            }

            if (m_vcaIndex == -1 && VC != null && VA != null)
            {
                // Calculate VCA = VC - VA
                missingSeries = VC.Add(VA.Negate());
                missingSeries.SeriesInfo = GetSeriesInfo(meter, m_dataGroup, "Voltage", "CA");
                missingSeries.Calculated = true;
                m_vcaIndex = m_dataGroup.DataSeries.Count;
                m_dataGroup.Add(missingSeries);
            }

        }

        public DataGroup ToDataGroup()
        {
            return new DataGroup(m_dataGroup.DataSeries);
        }

        public VIDataGroup ToSubGroup(int startIndex, int endIndex)
        {
            VIDataGroup subGroup = new VIDataGroup();

            subGroup.m_vaIndex = m_vaIndex;
            subGroup.m_vbIndex = m_vbIndex;
            subGroup.m_vcIndex = m_vcIndex;
            subGroup.m_vabIndex = m_vabIndex;
            subGroup.m_vbcIndex = m_vbcIndex;
            subGroup.m_vcaIndex = m_vcaIndex;
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

            subGroup.m_vaIndex = m_vaIndex;
            subGroup.m_vbIndex = m_vbIndex;
            subGroup.m_vcIndex = m_vcIndex;
            subGroup.m_vabIndex = m_vabIndex;
            subGroup.m_vbcIndex = m_vbcIndex;
            subGroup.m_vcaIndex = m_vcaIndex;
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
        private static Series GetSeriesInfo(Meter meter, DataGroup dataGroup, string measurementTypeName, string phaseName)
        {
            int assetID = dataGroup.Asset.ID;
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
                        Asset = dataGroup.Asset,
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
