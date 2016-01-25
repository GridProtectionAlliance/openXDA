//******************************************************************************************************
//  COMTRADEReader.cs - Gbtc
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
//  08/01/2014 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using GSF.COMTRADE;
using GSF.IO;
using FaultData.DataAnalysis;
using FaultData.Database;
using FaultData.DataSets;
using log4net;

namespace FaultData.DataReaders
{
    /// <summary>
    /// Reads a COMTRADE file to produce a <see cref="DataSets.MeterDataSet"/>.
    /// </summary>
    public class COMTRADEReader : IDataReader, IDisposable
    {
        #region [ Members ]

        // Fields
        private TimeSpan m_minWaitTime;
        private Parser m_parser;
        private MeterDataSet m_meterDataSet;

        private bool m_disposed;

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Creates a new instance of the <see cref="COMTRADEReader"/> class.
        /// </summary>
        public COMTRADEReader()
        {
            m_meterDataSet = new MeterDataSet();
        }

        #endregion

        #region [ Properties ]

        [Setting]
        public double COMTRADEMinWaitTime
        {
            get
            {
                return m_minWaitTime.TotalSeconds;
            }
            set
            {
                m_minWaitTime = TimeSpan.FromSeconds(value);
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

        public bool CanParse(string filePath, DateTime fileCreationTime)
        {
            string schemaFileName = Path.ChangeExtension(filePath, "cfg");
            string extension = FilePath.GetExtension(filePath);
            string[] fileList = FilePath.GetFileList(Path.ChangeExtension(filePath, "*"));
            bool multipleDataFiles = !extension.Equals(".dat", StringComparison.OrdinalIgnoreCase);

            if (!File.Exists(schemaFileName))
                return false;

            if (fileList.Any(file => !FilePath.TryGetReadLockExclusive(file)))
                return false;

            if (multipleDataFiles && DateTime.UtcNow - fileCreationTime < m_minWaitTime)
                return false;

            try
            {
                m_parser = new Parser();
                m_parser.Schema = new Schema(schemaFileName);
                m_parser.FileName = filePath;
                m_parser.InferTimeFromSampleRates = true;
                m_parser.OpenFiles();
            }
            catch (IOException)
            {
                return false;
            }

            return true;
        }

        public void Parse(string filePath)
        {
            Schema schema;
            Channel channel;
            DataSeries series;

            schema = m_parser.Schema;

            m_meterDataSet.Meter = new Meter();
            m_meterDataSet.Meter.AssetKey = schema.DeviceID;
            m_meterDataSet.Meter.Name = schema.DeviceID;
            m_meterDataSet.Meter.ShortName = schema.DeviceID.Substring(0, Math.Min(schema.DeviceID.Length, 50));

            m_meterDataSet.Meter.MeterLocation = new MeterLocation();
            m_meterDataSet.Meter.MeterLocation.AssetKey = schema.StationName;
            m_meterDataSet.Meter.MeterLocation.Name = schema.StationName;
            m_meterDataSet.Meter.MeterLocation.ShortName = schema.StationName.Substring(0, Math.Min(schema.StationName.Length, 50));
            m_meterDataSet.Meter.MeterLocation.Description = schema.StationName;

            foreach (AnalogChannel analogChannel in schema.AnalogChannels)
            {
                channel = ParseSeries(analogChannel);

                series = new DataSeries();
                series.SeriesInfo = channel.Series[0];

                m_meterDataSet.Meter.Channels.Add(channel);

                while (m_meterDataSet.DataSeries.Count <= analogChannel.Index)
                    m_meterDataSet.DataSeries.Add(new DataSeries());

                m_meterDataSet.DataSeries[analogChannel.Index] = series;
            }

            foreach (DigitalChannel digitalChannel in schema.DigitalChannels)
            {
                channel = ParseSeries(digitalChannel);

                series = new DataSeries();
                series.SeriesInfo = channel.Series[0];

                m_meterDataSet.Meter.Channels.Add(channel);

                while (m_meterDataSet.Digitals.Count <= digitalChannel.Index)
                    m_meterDataSet.Digitals.Add(new DataSeries());

                m_meterDataSet.Digitals[digitalChannel.Index] = series;
            }

            try
            {
                while (m_parser.ReadNext())
                {
                    for (int i = 0; i < schema.AnalogChannels.Length; i++)
                    {
                        int seriesIndex = schema.AnalogChannels[i].Index;
                        string units = schema.AnalogChannels[i].Units.ToUpper();
                        double multiplier = (units.Contains("KA") || units.Contains("KV")) ? 1000.0D : 1.0D;
                        m_meterDataSet.DataSeries[seriesIndex].DataPoints.Add(new DataPoint() { Time = m_parser.Timestamp, Value = multiplier * m_parser.PrimaryValues[i] });
                    }

                    for (int i = 0; i < schema.DigitalChannels.Length; i++)
                    {
                        int valuesIndex = schema.TotalAnalogChannels + i;
                        int seriesIndex = schema.DigitalChannels[i].Index;
                        m_meterDataSet.Digitals[seriesIndex].DataPoints.Add(new DataPoint() { Time = m_parser.Timestamp, Value = m_parser.Values[valuesIndex] });
                    }
                }
            }
            catch (InvalidOperationException ex)
            {
                Log.Warn(ex.Message, ex);
            }
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

        private Channel ParseSeries(AnalogChannel analogChannel)
        {
            Channel channel = new Channel();
            Series series = new Series();

            channel.Name = analogChannel.ChannelName;
            channel.HarmonicGroup = 0;
            channel.MeasurementType = new MeasurementType();
            channel.MeasurementType.Name = "Unknown";
            channel.MeasurementCharacteristic = new MeasurementCharacteristic();
            channel.MeasurementCharacteristic.Name = "Unknown";
            channel.Phase = new Phase();
            channel.Phase.Name = !string.IsNullOrEmpty(analogChannel.PhaseID) ? analogChannel.PhaseID : "Unknown";

            series.Channel = channel;
            series.SeriesType = new SeriesType();
            series.SeriesType.Name = "Values";
            series.SourceIndexes = analogChannel.Index.ToString();

            return channel;
        }

        private Channel ParseSeries(DigitalChannel digitalChannel)
        {
            Channel channel = new Channel();
            Series series = new Series();

            channel.Name = digitalChannel.ChannelName;
            channel.HarmonicGroup = 0;
            channel.MeasurementType = new MeasurementType();
            channel.MeasurementType.Name = "Digital";
            channel.MeasurementCharacteristic = new MeasurementCharacteristic();
            channel.MeasurementCharacteristic.Name = "Unknown";
            channel.Phase = new Phase();
            channel.Phase.Name = !string.IsNullOrEmpty(digitalChannel.PhaseID) ? digitalChannel.PhaseID : "Unknown";

            series.Channel = channel;
            series.SeriesType = new SeriesType();
            series.SeriesType.Name = "Values";
            series.SourceIndexes = digitalChannel.Index.ToString();

            return channel;
        }

        #endregion

        #region [ Static ]

        // Static Fields
        private static readonly ILog Log = LogManager.GetLogger(typeof(COMTRADEReader));

        #endregion
    }
}
