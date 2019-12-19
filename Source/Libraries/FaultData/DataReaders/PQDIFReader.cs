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
using System.Configuration;
using System.IO;
using System.Linq;
using FaultData.DataAnalysis;
using FaultData.DataSets;
using GSF;
using GSF.PQDIF.Logical;
using GSF.PQDIF.Physical;
using log4net;
using openXDA.Model;
using Phase = GSF.PQDIF.Logical.Phase;

namespace FaultData.DataReaders
{
    public class PQDIFReader : IDataReader, IDisposable
    {
        #region [ Members ]

        // Fields
        private double m_systemFrequency;

        private LogicalParser m_parser;
        private MeterDataSet m_meterDataSet;
        private bool m_disposed;

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Creates a new instance of the <see cref="PQDIFReader"/> class.
        /// </summary>
        public PQDIFReader()
        {
            m_meterDataSet = new MeterDataSet();
        }

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets or sets the system frequency.
        /// </summary>
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

        /// <summary>
        /// Gets the data set produced by the Parse method of the data reader.
        /// </summary>
        public MeterDataSet MeterDataSet
        {
            get
            {
                return m_meterDataSet;
            }
        }

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
        public void Parse(string filePath)
        {
            List<DataSourceRecord> dataSources;
            List<ObservationRecord> observationRecords;
            List<ChannelInstance> channelInstances;
            List<SeriesInstance> seriesInstances;
            List<SeriesDefinition> seriesDefinitions;

            Meter meter;
            Channel channel;
            DataSeries dataSeries;
            DateTime[] timeData;

            // Build the list of observation records in the PQDIF file
            observationRecords = new List<ObservationRecord>();

            while (m_parser.HasNextObservationRecord())
                observationRecords.Add(m_parser.NextObservationRecord());

            // Build the list of all data source records in the PQDIF file
            dataSources = observationRecords
                .Select(observation => observation.DataSource)
                .Distinct()
                .ToList();

            // If there are no data sources, there is no
            // need to go any further because we won't be
            // able to interpret any of the channel data
            if (!dataSources.Any())
                return;

            // Validate data sources to make sure there is only one data source defined in the file
            if (!dataSources.Zip(dataSources.Skip(1), (ds1, ds2) => AreEquivalent(ds1, ds2)).All(b => b))
                throw new InvalidDataException($"PQDIF file \"{filePath}\" defines too many data sources.");

            // Create a meter from the parsed data source
            meter = ParseDataSource(dataSources.First());

            // Build the list of all channel instances in the PQDIF file
            channelInstances = observationRecords
                .SelectMany(observation => observation.ChannelInstances)
                .Where(channelInstance => QuantityType.IsQuantityTypeID(channelInstance.Definition.QuantityTypeID))
                .Where(channelInstance => channelInstance.SeriesInstances.Any())
                .Where(channelInstance => channelInstance.SeriesInstances[0].Definition.ValueTypeID == SeriesValueType.Time)
                .ToList();

            // Create the list of series instances so we can
            // build it as we process each channel instance
            seriesInstances = new List<SeriesInstance>();

            foreach (ChannelInstance channelInstance in channelInstances)
            {
                bool timeValueChannel =
                    channelInstance.Definition.QuantityTypeID == QuantityType.WaveForm ||
                    channelInstance.Definition.QuantityTypeID == QuantityType.ValueLog ||
                    channelInstance.Definition.QuantityTypeID == QuantityType.Phasor;

                if (!timeValueChannel)
                    continue;

                // Parse time data from the channel instance
                timeData = ParseTimeData(channelInstance, m_systemFrequency);

                foreach (SeriesInstance seriesInstance in channelInstance.SeriesInstances.Skip(1))
                {
                    // Create a channel from the parsed series instance
                    seriesInstances.Add(seriesInstance);
                    channel = ParseSeries(seriesInstance);

                    // Parse the values and zip them with time data to create data points
                    dataSeries = new DataSeries();
                    dataSeries.DataPoints = timeData.Zip(ParseValueData(seriesInstance), (time, d) => new DataPoint() { Time = time, Value = d }).ToList();
                    dataSeries.SeriesInfo = channel.Series[0];

                    // Add the new channel to the meter's channel list
                    channel.Meter = meter;
                    meter.Channels.Add(channel);
                    m_meterDataSet.DataSeries.Add(dataSeries);
                }
            }

            foreach (ChannelInstance channelInstance in channelInstances)
            {
                bool magDurChannel =
                    channelInstance.Definition.QuantityTypeID == QuantityType.MagDur ||
                    channelInstance.Definition.QuantityTypeID == QuantityType.MagDurCount ||
                    channelInstance.Definition.QuantityTypeID == QuantityType.MagDurTime;

                if (!magDurChannel)
                    continue;

                timeData = channelInstance.SeriesInstances
                    .Where(seriesInstance => seriesInstance.Definition.ValueTypeID == SeriesValueType.Time)
                    .Select(seriesInstance => ParseTimeData(seriesInstance, m_systemFrequency))
                    .FirstOrDefault();

                Guid valType = SeriesValueType.Val;
                Guid maxType = SeriesValueType.Max;
                Guid minType = SeriesValueType.Min;
                Guid avgType = SeriesValueType.Avg;

                double[] valData = channelInstance.SeriesInstances
                    .Where(seriesInstance => seriesInstance.Definition.ValueTypeID == valType)
                    .Select(seriesInstance => ParseValueData(seriesInstance))
                    .FirstOrDefault();

                double[] maxData = channelInstance.SeriesInstances
                    .Where(seriesInstance => seriesInstance.Definition.ValueTypeID == maxType)
                    .Select(seriesInstance => ParseValueData(seriesInstance))
                    .FirstOrDefault() ?? valData;

                double[] minData = channelInstance.SeriesInstances
                    .Where(seriesInstance => seriesInstance.Definition.ValueTypeID == minType)
                    .Select(seriesInstance => ParseValueData(seriesInstance))
                    .FirstOrDefault() ?? valData;

                double[] avgData = channelInstance.SeriesInstances
                    .Where(seriesInstance => seriesInstance.Definition.ValueTypeID == avgType)
                    .Select(seriesInstance => ParseValueData(seriesInstance))
                    .FirstOrDefault() ?? valData;

                TimeSpan[] durData = channelInstance.SeriesInstances
                    .Where(seriesInstance => seriesInstance.Definition.ValueTypeID == SeriesValueType.Duration)
                    .Select(seriesInstance => ParseTimeSpanData(seriesInstance, m_systemFrequency))
                    .FirstOrDefault();

                int minLength = Common.Min(timeData.Length, maxData.Length, minData.Length, avgData.Length, durData.Length);

                QuantityUnits units = channelInstance.SeriesInstances
                    .Where(seriesInstance => new[] { valType, maxType, minType, avgType }.Contains(seriesInstance.Definition.ValueTypeID))
                    .Select(seriesInstance => seriesInstance.Definition.QuantityUnits)
                    .FirstOrDefault();

                for (int i = 0; i < minLength; i++)
                {
                    DateTime time = ((object)timeData != null)
                        ? timeData[i]
                        : channelInstance.ObservationRecord.StartTime;

                    double max = maxData[i];
                    double min = minData[i];
                    double avg = avgData[i];
                    TimeSpan dur = durData[i];

                    m_meterDataSet.ReportedDisturbances.Add(new ReportedDisturbance(channelInstance.Definition.Phase, time, max, min, avg, dur, units));
                }
            }

            // Build a list of series definitions that were not instanced by this PQDIF file
            seriesDefinitions = dataSources
                .SelectMany(dataSource => dataSource.ChannelDefinitions)
                .SelectMany(channelDefinition => channelDefinition.SeriesDefinitions)
                .Distinct()
                .Except(seriesInstances.Select(seriesInstance => seriesInstance.Definition))
                .ToList();

            // Add each of the series definitions which were not instanced to the meter's list of channels
            foreach (SeriesDefinition seriesDefinition in seriesDefinitions)
                meter.Channels.Add(ParseSeries(seriesDefinition));

            m_meterDataSet.Meter = meter;
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

        // Static Fields
        private static readonly ILog Log = LogManager.GetLogger(typeof(PQDIFReader));

        // Static Methods

        private static bool AreEquivalent(DataSourceRecord dataSource1, DataSourceRecord dataSource2)
        {
            if (ReferenceEquals(dataSource1, dataSource2))
                return true;

            if ((object)dataSource1 == null)
                return false;

            if ((object)dataSource2 == null)
                return false;

            return dataSource1.DataSourceName == dataSource2.DataSourceName &&
                   dataSource1.VendorID == dataSource2.VendorID &&
                   dataSource1.EquipmentID == dataSource2.EquipmentID;
        }

        private static Meter ParseDataSource(DataSourceRecord dataSource)
        {
            string name = dataSource.DataSourceName;
            Guid vendorID = dataSource.VendorID;
            Guid equipmentID = dataSource.EquipmentID;

            Meter meter = new Meter();
            meter.Location = new Location();
            meter.Channels = new List<Channel>();
            meter.Name = name;
            meter.AssetKey = name;
            meter.ShortName = name.Substring(0, Math.Min(name.Length, 50));

            Location meterLocation = meter.Location;
            meterLocation.Meters = new List<Meter>() { meter };
            meterLocation.LocationKey = name;
            meterLocation.Name = string.Format("{0} location", name);
            meterLocation.ShortName = meterLocation.Name.Substring(0, Math.Min(meterLocation.Name.Length, 50));
            meterLocation.Description = meterLocation.Name;

            if (vendorID != Vendor.None)
                meter.Make = Vendor.ToString(vendorID);

            if (equipmentID != Guid.Empty)
                meter.Model = Equipment.ToString(equipmentID);

            return meter;
        }

        private static Channel ParseSeries(SeriesDefinition seriesDefinition)
        {
            ChannelDefinition channelDefinition = seriesDefinition.ChannelDefinition;
            QuantityMeasured quantityMeasured = channelDefinition.QuantityMeasured;
            Phase phase = channelDefinition.Phase;

            // Populate series properties
            Series series = new Series();
            series.Channel = new Channel();
            series.SeriesType = new SeriesType();
            series.SourceIndexes = string.Empty;

            // Populate channel properties
            Channel channel = series.Channel;
            channel.Series = new List<Series>() { series };
            channel.MeasurementType = new MeasurementType();
            channel.MeasurementCharacteristic = new MeasurementCharacteristic();
            channel.Phase = new openXDA.Model.Phase();
            channel.Name = channelDefinition.ChannelName;
            channel.HarmonicGroup = 0;
            channel.Series.Add(series);

            if (seriesDefinition.HasElement(SeriesDefinition.SeriesNominalQuantityTag))
                channel.PerUnitValue = seriesDefinition.SeriesNominalQuantity;

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

        private static Channel ParseSeries(SeriesInstance seriesInstance)
        {
            Channel channel = ParseSeries(seriesInstance.Definition);
            channel.HarmonicGroup = seriesInstance.Channel.ChannelGroupID;
            return channel;
        }

        private static DateTime[] ParseTimeData(ChannelInstance channelInstance, double systemFrequency)
        {
            SeriesInstance timeSeries = channelInstance.SeriesInstances
                .FirstOrDefault(seriesInstance => seriesInstance.Definition.ValueTypeID == SeriesValueType.Time);

            if ((object)timeSeries == null)
                return null;

            return ParseTimeData(timeSeries, systemFrequency);
        }
        
        private static DateTime[] ParseTimeData(SeriesInstance seriesInstance, double systemFrequency)
        {
            DateTime[] timeData;

            SeriesDefinition timeSeriesDefinition = seriesInstance.Definition;

            if (timeSeriesDefinition.ValueTypeID != SeriesValueType.Time)
                return null;

            VectorElement seriesValues = seriesInstance.SeriesValues;

            if (seriesValues.TypeOfValue == PhysicalType.Timestamp)
            {
                timeData = seriesInstance.OriginalValues
                    .Select(Convert.ToDateTime)
                    .ToArray();
            }
            else if (timeSeriesDefinition.QuantityUnits == QuantityUnits.Cycles)
            {
                ChannelInstance channelInstance = seriesInstance.Channel;
                DateTime startTime = channelInstance.ObservationRecord.StartTime;
                double nominalFrequency = channelInstance.ObservationRecord?.Settings.NominalFrequency ?? systemFrequency;

                timeData = seriesInstance.OriginalValues
                    .Select(Convert.ToDouble)
                    .Select(cycles => cycles / nominalFrequency)
                    .Select(seconds => (long)(seconds * TimeSpan.TicksPerSecond))
                    .Select(TimeSpan.FromTicks)
                    .Select(timeSpan => startTime + timeSpan)
                    .ToArray();
            }
            else
            {
                ChannelInstance channelInstance = seriesInstance.Channel;
                DateTime startTime = channelInstance.ObservationRecord.StartTime;

                timeData = seriesInstance.OriginalValues
                    .Select(Convert.ToDouble)
                    .Select(seconds => (long)(seconds * TimeSpan.TicksPerSecond))
                    .Select(TimeSpan.FromTicks)
                    .Select(timeSpan => startTime + timeSpan)
                    .ToArray();
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

        private static TimeSpan[] ParseTimeSpanData(SeriesInstance seriesInstance, double systemFrequency)
        {
            TimeSpan[] timeSpanData;

            SeriesDefinition timeSeriesDefinition = seriesInstance.Definition;

            if (timeSeriesDefinition.ValueTypeID != SeriesValueType.Duration)
                return null;

            VectorElement seriesValues = seriesInstance.SeriesValues;

            if (timeSeriesDefinition.QuantityUnits == QuantityUnits.Cycles)
            {
                ChannelInstance channelInstance = seriesInstance.Channel;
                double nominalFrequency = channelInstance.ObservationRecord?.Settings.NominalFrequency ?? systemFrequency;

                timeSpanData = seriesInstance.OriginalValues
                    .Select(Convert.ToDouble)
                    .Select(cycles => cycles / nominalFrequency)
                    .Select(seconds => (long)(seconds * TimeSpan.TicksPerSecond))
                    .Select(TimeSpan.FromTicks)
                    .ToArray();
            }
            else
            {
                timeSpanData = seriesInstance.OriginalValues
                    .Select(Convert.ToDouble)
                    .Select(seconds => (long)(seconds * TimeSpan.TicksPerSecond))
                    .Select(TimeSpan.FromTicks)
                    .ToArray();
            }

            return timeSpanData;
        }

        #endregion
    }
}
