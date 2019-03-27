﻿//******************************************************************************************************
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
            string[] instantaneousSeriesTypes = { "Values", "Instantaneous" };

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

            for (int i = 0; i < dataGroup.DataSeries.Count; i++)
            {
                string measurementType = dataGroup[i].SeriesInfo.Channel.MeasurementType.Name;
                string measurementCharacteristic = dataGroup[i].SeriesInfo.Channel.MeasurementCharacteristic.Name;
                string seriesType = dataGroup[i].SeriesInfo.SeriesType.Name;
                string phase = dataGroup[i].SeriesInfo.Channel.Phase.Name;

                // If the data group is not instantaneous, do not use it in the VIDataGroup
                if (measurementCharacteristic != "Instantaneous")
                    continue;

                // If the data group is not instantaneous, do not use it in the VIDataGroup
                if (!instantaneousSeriesTypes.Contains(seriesType))
                    continue;

                // Assign the proper indexes for the seven VIDataGroup
                // channels by checking the name of the channel
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
        /// <param name="meterInfo">Data context for accessing configuration tables in the database.</param>
        public DataSeries CalculateMissingCurrentChannel()
        {
            Meter meter;
            DataSeries missingSeries;

            // If the data group does not have exactly 3 channels,
            // then there is no missing channel or there is not
            // enough data to calculate the missing channel
            if (DefinedCurrents != 3)
                return null;

            // Get the meter associated with the channels in this data group
            meter = (IA ?? IB).SeriesInfo.Channel.Meter;

            if (m_iaIndex == -1)
            {
                // Calculate IA = IR - IB - IC
                missingSeries = IR.Add(IB.Negate()).Add(IC.Negate());
                missingSeries.SeriesInfo = GetSeriesInfo(meter, m_dataGroup, "Current", "AN");
                m_iaIndex = m_dataGroup.DataSeries.Count;
                m_dataGroup.Add(missingSeries);
            }
            else if (m_ibIndex == -1)
            {
                // Calculate IB = IR - IA - IC
                missingSeries = IR.Add(IA.Negate()).Add(IC.Negate());
                missingSeries.SeriesInfo = GetSeriesInfo(meter, m_dataGroup, "Current", "BN");
                m_ibIndex = m_dataGroup.DataSeries.Count;
                m_dataGroup.Add(missingSeries);
            }
            else if (m_icIndex == -1)
            {
                // Calculate IC = IR - IA - IB
                missingSeries = IR.Add(IA.Negate()).Add(IB.Negate());
                missingSeries.SeriesInfo = GetSeriesInfo(meter, m_dataGroup, "Current", "CN");
                m_icIndex = m_dataGroup.DataSeries.Count;
                m_dataGroup.Add(missingSeries);
            }
            else
            {
                // Calculate IR = IA + IB + IC
                missingSeries = IA.Add(IB).Add(IC);
                missingSeries.SeriesInfo = GetSeriesInfo(meter, m_dataGroup, "Current", "RES");
                m_irIndex = m_dataGroup.DataSeries.Count;
                m_dataGroup.Add(missingSeries);
            }

            return missingSeries;
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
            int lineID = dataGroup.Line.ID;
            string measurementCharacteristicName = "Instantaneous";
            string seriesTypeName = "Values";

            char typeDesignation = (measurementTypeName == "Current") ? 'I' : measurementTypeName[0];
            char phaseDesignation = (phaseName == "RES") ? 'R' : phaseName[0];
            string channelName = string.Concat(typeDesignation, phaseDesignation);

            ChannelKey channelKey = new ChannelKey(lineID, 0, channelName, measurementTypeName, measurementCharacteristicName, phaseName);
            SeriesKey seriesKey = new SeriesKey(channelKey, seriesTypeName);

            Series dbSeries = meter.Channels
                .SelectMany(channel => channel.Series)
                .FirstOrDefault(series => seriesKey.Equals(new SeriesKey(series)));

            if ((object)dbSeries == null)
            {
                using (AdoDataConnection connection = meter.ConnectionFactory())
                {
                    Channel dbChannel = meter.Channels
                        .FirstOrDefault(channel => channelKey.Equals(new ChannelKey(channel)));

                    if ((object)dbChannel == null)
                    {
                        TableOperations<Channel> channelTable = new TableOperations<Channel>(connection);
                        TableOperations<MeasurementType> measurementTypeTable = new TableOperations<MeasurementType>(connection);
                        TableOperations<MeasurementCharacteristic> measurementCharacteristicTable = new TableOperations<MeasurementCharacteristic>(connection);
                        TableOperations<Phase> phaseTable = new TableOperations<Phase>(connection);

                        MeasurementType measurementType = measurementTypeTable.GetOrAdd(measurementTypeName);
                        MeasurementCharacteristic measurementCharacteristic = measurementCharacteristicTable.GetOrAdd(measurementCharacteristicName);
                        Phase phase = phaseTable.GetOrAdd(phaseName);

                        dbChannel = new Channel()
                        {
                            MeterID = meter.ID,
                            LineID = lineID,
                            MeasurementTypeID = measurementType.ID,
                            MeasurementCharacteristicID = measurementCharacteristic.ID,
                            PhaseID = phase.ID,
                            Name = channelKey.Name,
                            SamplesPerHour = dataGroup.SamplesPerHour,
                            Description = string.Concat(measurementCharacteristicName, " ", measurementTypeName, " ", phaseName),
                            Enabled = true
                        };

                        channelTable.AddNewRecord(dbChannel);
                        dbChannel.ID = connection.ExecuteScalar<int>("SELECT @@IDENTITY");
                        meter.Channels = null;
                    }

                    TableOperations<Series> seriesTable = new TableOperations<Series>(connection);
                    TableOperations<SeriesType> seriesTypeTable = new TableOperations<SeriesType>(connection);
                    SeriesType seriesType = seriesTypeTable.GetOrAdd(seriesTypeName);

                    dbSeries = new Series()
                    {
                        ChannelID = dbChannel.ID,
                        SeriesTypeID = seriesType.ID,
                        SourceIndexes = string.Empty
                    };

                    seriesTable.AddNewRecord(dbSeries);
                    dbSeries.ID = connection.ExecuteScalar<int>("SELECT @@IDENTITY");
                    dbChannel.Series = null;

                    dbSeries = meter.Channels
                        .SelectMany(channel => channel.Series)
                        .First(series => seriesKey.Equals(new SeriesKey(series)));
                }
            }

            return dbSeries;
        }

        #endregion
    }
}
