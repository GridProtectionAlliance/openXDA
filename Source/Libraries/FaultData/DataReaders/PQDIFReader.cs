//******************************************************************************************************
//  PQDIFLoader.cs - Gbtc
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
//  05/07/2014 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FaultData.DataAnalysis;
using FaultData.Database;
using FaultData.DataSets;
using GSF.PQDIF.Logical;
using Phase = GSF.PQDIF.Logical.Phase;

namespace FaultData.DataReaders
{
    public class PQDIFReader : IDataReader, IDisposable
    {
        #region [ Members ]

        // Fields
        private LogicalParser m_parser;
        private bool m_disposed;

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Determines whether the file can be parsed at this time.
        /// </summary>
        /// <param name="filePath">The path to the file to be parsed.</param>
        /// <param name="fileCreationTime">The time the file was created.</param>
        /// <returns>True if the file can be parsed; false otherwise.</returns>
        public bool CanParse(string filePath, DateTime fileCreationTime)
        {
            try
            {
                m_parser = new LogicalParser(filePath);
                m_parser.Open();
                return true;
            }
            catch (IOException)
            {
                return false;
            }
        }

        /// <summary>
        /// Parses the file into a meter data set per meter contained in the file.
        /// </summary>
        /// <param name="filePath">The path to the file to be parsed.</param>
        /// <returns>List of meter data sets, one per meter.</returns>
        public List<MeterDataSet> Parse(string filePath)
        {
            List<MeterDataSet> meterDataSets = new List<MeterDataSet>();

            DataSourceRecord dataSource = null;
            ObservationRecord observation;

            MeterDataSet meterDataSet = null;
            Meter meter = null;

            IEnumerable<ChannelInstance> channelInstances;
            Channel channel;
            DataSeries dataSeries;
            DateTime[] timeData;

            while (m_parser.HasNextObservationRecord())
            {
                observation = m_parser.NextObservationRecord();

                if ((object)observation.DataSource == null)
                    continue;

                if (!ReferenceEquals(dataSource, observation.DataSource))
                {
                    dataSource = observation.DataSource;
                    meterDataSet = new MeterDataSet();
                    meter = ParseDataSource(dataSource);

                    meterDataSet.Meter = meter;
                    meterDataSets.Add(meterDataSet);
                }

                if ((object)meter == null)
                    continue;

                channelInstances = observation.ChannelInstances
                    .Where(channelInstance => QuantityType.IsQuantityTypeID(channelInstance.Definition.QuantityTypeID))
                    .Where(channelInstance => channelInstance.SeriesInstances.Any())
                    .Where(channelInstance => channelInstance.SeriesInstances[0].Definition.ValueTypeID == SeriesValueType.Time);

                foreach (ChannelInstance channelInstance in channelInstances)
                {
                    timeData = ParseTimeData(channelInstance);

                    foreach (SeriesInstance seriesInstance in channelInstance.SeriesInstances.Skip(1))
                    {
                        channel = ParseSeries(seriesInstance);

                        dataSeries = new DataSeries();
                        dataSeries.DataPoints = timeData.Zip(ParseValueData(seriesInstance), (time, d) => new DataPoint() { Time = time, Value = d }).ToList();
                        dataSeries.SeriesInfo = channel.Series[0];

                        meter.Channels.Add(channel);
                        meterDataSet.DataSeries.Add(dataSeries);
                    }
                }
            }

            return meterDataSets;
        }

        public void Dispose()
        {
            if (!m_disposed)
            {
                try
                {
                    if ((object)m_parser != null)
                    {
                        m_parser.Dispose();
                        m_parser = null;
                    }
                }
                finally
                {
                    m_disposed = true;
                }
            }
        }

        #endregion

        #region [ Static ]

        // Static Methods

        public static List<MeterDataSet> ParseFile(string filePath)
        {
            List<MeterDataSet> meterDataSets = new List<MeterDataSet>();

            DataSourceRecord dataSource = null;
            ObservationRecord observation;

            MeterDataSet meterDataSet = null;
            Meter meter = null;

            IEnumerable<ChannelInstance> channelInstances;
            Channel channel;
            DataSeries dataSeries;
            DateTime[] timeData;

            using (LogicalParser parser = new LogicalParser(filePath))
            {
                parser.Open();

                while (parser.HasNextObservationRecord())
                {
                    observation = parser.NextObservationRecord();

                    if ((object)observation.DataSource == null)
                        continue;

                    if (!ReferenceEquals(dataSource, observation.DataSource))
                    {
                        dataSource = observation.DataSource;
                        meterDataSet = new MeterDataSet();
                        meter = ParseDataSource(dataSource);

                        meterDataSet.Meter = meter;
                        meterDataSets.Add(meterDataSet);
                    }

                    if ((object)meter == null)
                        continue;

                    channelInstances = observation.ChannelInstances
                        .Where(channelInstance => QuantityType.IsQuantityTypeID(channelInstance.Definition.QuantityTypeID))
                        .Where(channelInstance => channelInstance.SeriesInstances.Any())
                        .Where(channelInstance => channelInstance.SeriesInstances[0].Definition.ValueTypeID == SeriesValueType.Time);

                    foreach (ChannelInstance channelInstance in channelInstances)
                    {
                        timeData = ParseTimeData(channelInstance);

                        foreach (SeriesInstance seriesInstance in channelInstance.SeriesInstances.Skip(1))
                        {
                            channel = ParseSeries(seriesInstance);

                            dataSeries = new DataSeries();
                            dataSeries.DataPoints = timeData.Zip(ParseValueData(seriesInstance), (time, d) => new DataPoint() { Time = time, Value = d }).ToList();
                            dataSeries.SeriesInfo = channel.Series[0];

                            meter.Channels.Add(channel);
                            meterDataSet.DataSeries.Add(dataSeries);
                        }
                    }
                }
            }

            return meterDataSets;
        }

        private static Meter ParseDataSource(DataSourceRecord dataSource)
        {
            Meter meter;
            MeterLocation meterLocation;

            string name = dataSource.DataSourceName;
            Guid vendorID = dataSource.VendorID;
            Guid equipmentID = dataSource.EquipmentID;

            meter = new Meter();
            meter.Name = name;
            meter.AssetKey = name;
            meter.ShortName = name.Substring(0, Math.Min(name.Length, 50));

            meterLocation = new MeterLocation();
            meterLocation.AssetKey = meter.Name;
            meterLocation.Name = string.Format("{0} location", meter.Name);
            meterLocation.ShortName = meterLocation.Name.Substring(0, Math.Min(meterLocation.Name.Length, 50));
            meterLocation.Description = meterLocation.Name;

            if (vendorID != Vendor.None)
                meter.Make = Vendor.ToString(vendorID);

            if (equipmentID != Guid.Empty)
                meter.Model = Equipment.ToString(equipmentID);

            return meter;
        }

        private static Channel ParseSeries(SeriesInstance seriesInstance)
        {
            Channel channel = new Channel();
            Series series = new Series();

            ChannelInstance channelInstance = seriesInstance.Channel;
            ChannelDefinition channelDefinition = channelInstance.Definition;
            SeriesDefinition seriesDefinition = seriesInstance.Definition;
            QuantityMeasured quantityMeasured = channelDefinition.QuantityMeasured;
            Phase phase = channelDefinition.Phase;

            // Populate channel properties
            channel.Name = channelDefinition.ChannelName;
            channel.HarmonicGroup = channelInstance.ChannelGroupID;
            channel.MeasurementType = new MeasurementType();
            channel.MeasurementCharacteristic = new MeasurementCharacteristic();
            channel.Phase = new Database.Phase();

            if (seriesInstance.Definition.HasElement(SeriesDefinition.SeriesNominalQuantityTag))
                channel.PerUnitValue = seriesInstance.Definition.SeriesNominalQuantity;

            // Populate series properties
            series.SeriesType = new SeriesType();
            series.Channel = channel;
            series.SourceIndexes = string.Empty;

            // Populate measurement type properties
            channel.MeasurementType.Name = quantityMeasured.ToString();

            // Populate characteristic properties
            channel.MeasurementCharacteristic.Name = QuantityCharacteristic.ToName(seriesDefinition.QuantityCharacteristicID) ?? seriesDefinition.QuantityCharacteristicID.ToString();
            channel.MeasurementCharacteristic.Description = QuantityCharacteristic.ToString(seriesDefinition.QuantityCharacteristicID);

            // Popuplate phase properties
            channel.Phase.Name = phase.ToString();

            // Populate series type properties
            series.SeriesType.Name = SeriesValueType.ToString(seriesDefinition.ValueTypeID) ?? seriesDefinition.ValueTypeName ?? seriesDefinition.ValueTypeID.ToString();
            series.SeriesType.Description = seriesDefinition.ValueTypeName;

            return channel;
        }

        private static DateTime[] ParseTimeData(ChannelInstance channelInstance)
        {
            SeriesInstance timeSeries;
            SeriesDefinition timeSeriesDefinition;
            DateTime[] timeData;
            DateTime startTime;

            if (!channelInstance.SeriesInstances.Any())
                return null;

            timeSeries = channelInstance.SeriesInstances[0];
            timeSeriesDefinition = timeSeries.Definition;

            if (timeSeriesDefinition.ValueTypeID != SeriesValueType.Time)
                return null;

            if (timeSeriesDefinition.QuantityUnits == QuantityUnits.Timestamp)
            {
                timeData = timeSeries.OriginalValues
                    .Select(Convert.ToDateTime)
                    .ToArray();
            }
            else if (timeSeriesDefinition.QuantityUnits == QuantityUnits.Seconds)
            {
                startTime = channelInstance.ObservationRecord.StartTime;

                timeData = timeSeries.OriginalValues
                    .Select(Convert.ToDouble)
                    .Select(seconds => (long)(seconds * TimeSpan.TicksPerSecond))
                    .Select(TimeSpan.FromTicks)
                    .Select(timeSpan => startTime + timeSpan)
                    .ToArray();
            }
            else
            {
                return null;
            }

            return timeData;
        }

        private static double[] ParseValueData(SeriesInstance seriesInstance)
        {
            try
            {
                return seriesInstance.OriginalValues.Select(Convert.ToDouble).ToArray();
            }
            catch
            {
                return null;
            }
        }

        #endregion
    }
}
