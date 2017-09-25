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
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using FaultData.Configuration;
using FaultData.DataAnalysis;
using FaultData.DataSets;
using GSF.COMTRADE;
using GSF.Configuration;
using GSF.EMAX;
using GSF.IO;
using log4net;
using openXDA.Model;

namespace FaultData.DataReaders
{
    public class EMAXReader : IDataReader, IDisposable
    {
        #region [ Members ]

        // Fields
        private EMAXSettings m_emaxSettings;
        private string m_filePattern;

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
        /// Gets or sets the file pattern used to determine
        /// the asset key of the meter from the file path.
        /// </summary>
        [Setting]
        public string FilePattern
        {
            get
            {
                return m_filePattern;
            }
            set
            {
                m_filePattern = value;
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

            List<EVNT_CHNL_NEW> digitalChannels = controlFile.EventChannelSettings
                .OrderBy(kvp => kvp.Key)
                .Select(kvp => kvp.Value)
                .ToList();

            // Add an empty data series for 1-based indexing
            m_meterDataSet.DataSeries.Add(new DataSeries());
            m_meterDataSet.Digitals.Add(new DataSeries());

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

            foreach (EVNT_CHNL_NEW digitalChannel in digitalChannels)
            {
                Channel channel = ParseSeries(digitalChannel);
                channel.Series.Single().SourceIndexes = m_meterDataSet.Digitals.Count.ToString();

                DataSeries series = new DataSeries();
                series.SeriesInfo = channel.Series[0];

                meter.Channels.Add(channel);

                m_meterDataSet.Digitals.Add(series);
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

            if (!string.IsNullOrEmpty(m_emaxSettings.COMTRADEExportDirectory))
                ExportToCOMTRADE(filePath, controlFile, identityString, analogChannels, digitalChannels);

            m_meterDataSet.Meter = meter;
        }

        private void ExportToCOMTRADE(string filePath, ControlFile controlFile, string identityString, List<ANLG_CHNL_NEW> analogChannels, List<EVNT_CHNL_NEW> digitalChannels)
        {
            string assetKey = GetMeterKey(filePath, m_filePattern) ?? ThreadContext.Properties["Meter"].ToString();
            string rootFileName = FilePath.GetFileNameWithoutExtension(filePath);
            string directoryPath = Path.Combine(m_emaxSettings.COMTRADEExportDirectory, assetKey);
            string schemaFilePath = Path.Combine(directoryPath, $"{rootFileName}.cfg");
            string dataFilePath = Path.Combine(directoryPath, $"{rootFileName}.dat");
            Schema comtradeSchema = new Schema();

            if (File.Exists(dataFilePath))
                return;

            comtradeSchema.StationName = Regex.Replace(identityString, @"[\r\n]", "");
            comtradeSchema.Version = 2013;
            comtradeSchema.AnalogChannels = new AnalogChannel[controlFile.AnalogChannelCount];
            comtradeSchema.DigitalChannels = new DigitalChannel[controlFile.EventChannelSettings.Count];
            comtradeSchema.SampleRates = new SampleRate[1];
            comtradeSchema.SampleRates[0].Rate = controlFile.SystemParameters.samples_per_second;
            comtradeSchema.SampleRates[0].EndSample = controlFile.SystemParameters.rcd_sample_count - 1;
            comtradeSchema.StartTime = new Timestamp() { Value = m_meterDataSet.DataSeries[1][0].Time };

            int triggerIndex = controlFile.SystemParameters.start_offset_samples + controlFile.SystemParameters.prefault_samples;
            comtradeSchema.TriggerTime = new Timestamp() { Value = m_meterDataSet.DataSeries[1][triggerIndex].Time };

            for (int i = 0; i < analogChannels.Count; i++)
            {
                ANLG_CHNL_NEW analogChannel = analogChannels[i];
                AnalogChannel comtradeAnalog = new AnalogChannel();
                DataSeries channelData = m_meterDataSet.DataSeries[i + 1];

                double unitMultiplier = 1.0D;
                double max = channelData.Maximum;
                double min = channelData.Minimum;
                double num;

                comtradeAnalog.Index = i + 1;
                comtradeAnalog.Name = analogChannel.title;

                comtradeAnalog.Units = new Func<string, string>(type =>
                {
                    switch (type)
                    {
                        case "V": return "kVAC";
                        case "A": return "kAAC";
                        case "v": return " VDC";
                        default: return type;
                    }
                })(analogChannel.type);

                if (analogChannel.type.All(char.IsUpper))
                {
                    unitMultiplier = 0.001D;
                    max *= unitMultiplier;
                    min *= unitMultiplier;
                }

                comtradeAnalog.Multiplier = (max - min) / (2 * short.MaxValue);
                comtradeAnalog.Adder = (max + min) / 2.0D;
                comtradeAnalog.PrimaryRatio = double.TryParse(analogChannel.primary, out num) ? num * unitMultiplier : 0.0D;
                comtradeAnalog.SecondaryRatio = double.TryParse(analogChannel.secondary, out num) ? num * unitMultiplier : 0.0D;

                comtradeSchema.AnalogChannels[i] = comtradeAnalog;
            }

            for (int i = 0; i < digitalChannels.Count; i++)
            {
                EVNT_CHNL_NEW digitalChannel = digitalChannels[i];
                DigitalChannel comtradeDigital = new DigitalChannel();
                comtradeDigital.Index = i + 1;
                comtradeDigital.ChannelName = digitalChannel.e_title;
                comtradeSchema.DigitalChannels[i] = comtradeDigital;
            }

            Directory.CreateDirectory(directoryPath);
            File.WriteAllText(schemaFilePath, comtradeSchema.FileImage, Encoding.ASCII);

            using (FileStream stream = File.OpenWrite(dataFilePath))
            {
                const int DigitalSize = sizeof(ushort) * 8;

                IEnumerable<DataSeries> digitalWords = m_meterDataSet.Digitals.Skip(1)
                    .Select((dataSeries, index) => dataSeries.Multiply(Math.Pow(2.0D, DigitalSize - (index % DigitalSize) - 1)))
                    .Select((DataSeries, Index) => new { DataSeries, Index })
                    .GroupBy(obj => obj.Index / DigitalSize)
                    .Select(grouping => grouping.Select(obj => obj.DataSeries))
                    .Select(grouping => grouping.Aggregate((sum, series) => sum.Add(series)));

                List<DataSeries> allChannels = m_meterDataSet.DataSeries.Skip(1)
                    .Select((dataSeries, index) => analogChannels[index].type.All(char.IsUpper) ? dataSeries.Multiply(0.001D) : dataSeries)
                    .Concat(digitalWords)
                    .ToList();

                for (int i = 0; i < m_meterDataSet.DataSeries[1].DataPoints.Count; i++)
                {
                    DateTime timestamp = m_meterDataSet.DataSeries[1][i].Time;

                    double[] values = allChannels
                        .Select(dataSeries => dataSeries[i].Value)
                        .ToArray();

                    Writer.WriteNextRecordBinary(stream, comtradeSchema, timestamp, values, (uint)i, false);
                }
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

        private Channel ParseSeries(EVNT_CHNL_NEW digitalChannel)
        {
            Channel channel = new Channel();
            Series series = new Series();

            channel.Name = digitalChannel.e_title;
            channel.HarmonicGroup = 0;
            channel.MeasurementType = new MeasurementType();
            channel.MeasurementType.Name = "Digital";
            channel.MeasurementCharacteristic = new MeasurementCharacteristic();
            channel.MeasurementCharacteristic.Name = "Unknown";
            channel.Phase = new Phase();
            channel.Phase.Name = "Unknown";

            series.Channel = channel;
            series.SeriesType = new SeriesType();
            series.SeriesType.Name = "Instantaneous";
            series.SourceIndexes = digitalChannel.eventnum;

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

        // Uses regular expressions to read the meter's asset key from the file path.
        private static string GetMeterKey(string filePath, string filePattern)
        {
            Match match = Regex.Match(filePath, filePattern, RegexOptions.IgnoreCase);
            Group meterKeyGroup;

            if (!match.Success)
                return null;

            meterKeyGroup = match.Groups["AssetKey"];

            if ((object)meterKeyGroup == null)
                return null;

            return meterKeyGroup.Value;
        }

        #endregion
    }
}
