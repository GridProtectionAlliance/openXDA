//******************************************************************************************************
//  EMAXReader.cs - Gbtc
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
//  12/15/2014 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using FaultData.Configuration;
using FaultData.DataAnalysis;
using FaultData.DataSets;
using GSF.Configuration;
using GSF.EMAX;
using GSF.IO;
using openXDA.Model;

namespace FaultData.DataReaders
{
    public class EMAXReader : IDataReader, IDisposable
    {
        #region [ Members ]

        // Fields
        private EMAXSettings m_emaxSettings;
        private CorrectiveParser m_parser;
        private MeterDataSet m_meterDataSet;
        private bool m_disposed;

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Creates a new instance of the <see cref="EMAXReader"/> class.
        /// </summary>
        public EMAXReader()
        {
            m_emaxSettings = new EMAXSettings();
            m_meterDataSet = new MeterDataSet();
        }

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Settings to configure native EMAX file format integration.
        /// </summary>
        [Category]
        [SettingName("EMAX")]
        public EMAXSettings EMAXSettings
        {
            get
            {
                return m_emaxSettings;
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
            string directory = FilePath.GetDirectoryName(filePath);
            string rootFileName = FilePath.GetFileNameWithoutExtension(filePath);
            string controlFileName = Path.Combine(directory, rootFileName + ".ctl");

            if (!File.Exists(controlFileName))
                return false;

            try
            {
                m_parser = new CorrectiveParser();
                m_parser.ControlFile = new ControlFile(controlFileName);
                m_parser.FileName = filePath;
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
            ControlFile controlFile = m_parser.ControlFile;
            string identityString = controlFile.IdentityString.value;
            string deviceName = identityString.Substring(0, IndexOf(identityString, "\r\n", "\n", "\r"));

            Meter meter = new Meter();
            meter.MeterLocation = new MeterLocation();
            meter.Channels = new List<Channel>();
            meter.AssetKey = deviceName;
            meter.Name = deviceName;
            meter.ShortName = deviceName.Substring(0, Math.Min(deviceName.Length, 50));

            MeterLocation meterLocation = meter.MeterLocation;
            meterLocation.Meters = new List<Meter>() { meter };
            meterLocation.AssetKey = deviceName;
            meterLocation.Name = deviceName;
            meterLocation.ShortName = meter.ShortName;
            meterLocation.Description = deviceName;

            List<ANLG_CHNL_NEW> analogChannels = controlFile.AnalogChannelSettings
                .OrderBy(kvp => kvp.Key)
                .Select(kvp => kvp.Value)
                .ToList();

            // Add an empty data series for 1-based indexing
            m_meterDataSet.DataSeries.Add(new DataSeries());

            foreach (ANLG_CHNL_NEW analogChannel in analogChannels)
            {
                Channel channel = ParseSeries(analogChannel);
                channel.Meter = meter;
                channel.Series.Single().SourceIndexes = m_meterDataSet.DataSeries.Count.ToString();

                DataSeries dataSeries = new DataSeries();
                dataSeries.SeriesInfo = channel.Series[0];

                meter.Channels.Add(channel);

                m_meterDataSet.DataSeries.Add(dataSeries);
            }

            while (m_parser.ReadNext())
            {
                for (int i = 0; i < analogChannels.Count; i++)
                {
                    DateTime timestamp = m_emaxSettings.ApplyTimestampCorrection ? m_parser.CalculatedTimestamp : m_parser.ParsedTimestamp;
                    double value = m_emaxSettings.ApplyValueCorrection ? m_parser.CorrectedValues[i] : m_parser.Values[i];
                    m_meterDataSet.DataSeries[i + 1].DataPoints.Add(new DataPoint() { Time = timestamp, Value = value });
                }
            }

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

        private Channel ParseSeries(ANLG_CHNL_NEW analogChannel)
        {
            Series series = new Series();
            series.Channel = new Channel();
            series.SeriesType = new SeriesType() { Name = "Instantaneous" };
            series.SourceIndexes = analogChannel.chanlnum;

            Channel channel = series.Channel;
            channel.Series = new List<Series>() { series };
            channel.MeasurementType = new MeasurementType() { Name = "Unknown" };
            channel.MeasurementCharacteristic = new MeasurementCharacteristic() { Name = "Unknown" };
            channel.Phase = new Phase() { Name = "Unknown" };
            channel.Name = analogChannel.title;
            channel.HarmonicGroup = 0;

            return channel;
        }

        private int IndexOf(string input, params string[] searchStrings)
        {
            int index = -1;
            int i = 0;

            while (index < 0 && i < searchStrings.Length)
                index = input.IndexOf(searchStrings[i++], StringComparison.Ordinal);

            return index;
        }

        #endregion
    }
}
