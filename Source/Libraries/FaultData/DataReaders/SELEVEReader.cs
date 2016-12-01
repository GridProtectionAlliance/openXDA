//******************************************************************************************************
//  SELEVEReader.cs - Gbtc
//
//  Copyright © 2015, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the MIT License (MIT), the "License"; you may
//  not use this file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://opensource.org/licenses/MIT
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  10/29/2015 - Stephen C. Wills
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
using GSF;
using GSF.SELEventParser;

namespace FaultData.DataReaders
{
    /// <summary>
    /// Reads an SEL event file to produce a <see cref="MeterDataSet"/>.
    /// </summary>
    public class SELEVEReader : IDataReader
    {
        #region [ Members ]

        // Fields
        private EventFile m_eventFile;
        private MeterDataSet m_meterDataSet;

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Creates a new instance of the <see cref="SELEVEReader"/> class.
        /// </summary>
        public SELEVEReader()
        {
            m_meterDataSet = new MeterDataSet();
        }

        #endregion

        #region [ Properties ]

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
                m_eventFile = EventFile.Parse(filePath);
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
            Channel channel;
            DataSeries series;
            List<DateTime> timeSamples;
            List<double> valueSamples;

            if ((object)m_eventFile == null)
                m_eventFile = EventFile.Parse(filePath);

            if (!m_eventFile.EventReports.Any() && !m_eventFile.CommaSeparatedEventReports.Any())
                return;

            m_meterDataSet.Meter = new Meter();
            m_meterDataSet.Meter.AssetKey = m_eventFile.EventReports[0].Header.RelayID;
            m_meterDataSet.Meter.Name = m_eventFile.EventReports[0].Header.RelayID;
            m_meterDataSet.Meter.ShortName = new string(m_eventFile.EventReports[0].Header.RelayID.ToNonNullString().Take(50).ToArray());

            m_meterDataSet.Meter.MeterLocation = new MeterLocation();
            m_meterDataSet.Meter.MeterLocation.AssetKey = m_eventFile.EventReports[0].Header.StationID;
            m_meterDataSet.Meter.MeterLocation.Name = m_eventFile.EventReports[0].Header.StationID;
            m_meterDataSet.Meter.MeterLocation.ShortName = new string(m_eventFile.EventReports[0].Header.StationID.ToNonNullString().Take(50).ToArray());
            m_meterDataSet.Meter.MeterLocation.Description = m_eventFile.EventReports[0].Header.StationID;

            foreach (EventReport report in m_eventFile.EventReports)
            {
                for (int i = 0; i < report.AnalogSection.AnalogChannels.Count; i++)
                {
                    channel = MakeParsedAnalog(report, i);
                    series = new DataSeries();

                    timeSamples = report.AnalogSection.TimeChannel.Samples;
                    valueSamples = report.AnalogSection.AnalogChannels[i].Samples;

                    series.SeriesInfo = channel.Series[0];

                    series.DataPoints = timeSamples
                        .Zip(valueSamples, (time, value) => new DataPoint() { Time = time, Value = value })
                        .ToList();

                    if (new string[] { "VA", "VB", "VC", "VS" }.Contains(report.AnalogSection.AnalogChannels[i].Name))
                        m_meterDataSet.DataSeries.Add(series.Multiply(1000.0));
                    else
                        m_meterDataSet.DataSeries.Add(series);
                }

                for (int i = 0; i < report.AnalogSection.DigitalChannels.Count; i++)
                {
                    channel = MakeParsedDigital(report, i);
                    series = new DataSeries();

                    timeSamples = report.AnalogSection.TimeChannel.Samples;
                    valueSamples = report.AnalogSection.DigitalChannels[i].Samples.Select(Convert.ToDouble).ToList();

                    series.SeriesInfo = channel.Series[0];

                    series.DataPoints = timeSamples
                        .Zip(valueSamples, (time, value) => new DataPoint() { Time = time, Value = value })
                        .ToList();

                    m_meterDataSet.Digitals.Add(series);
                }
            }

            foreach (CommaSeparatedEventReport report in m_eventFile.CommaSeparatedEventReports)
            {
                for (int i = 0; i < report.AnalogSection.AnalogChannels.Count; i++)
                {
                    channel = MakeParsedAnalog(report, i);
                    series = new DataSeries();

                    timeSamples = report.AnalogSection.TimeChannel.Samples;
                    valueSamples = report.AnalogSection.AnalogChannels[i].Samples;

                    series.SeriesInfo = channel.Series[0];

                    series.DataPoints = timeSamples
                        .Zip(valueSamples, (time, value) => new DataPoint() { Time = time, Value = value })
                        .ToList();

                    if (new string[] { "VA", "VB", "VC", "VS" }.Contains(report.AnalogSection.AnalogChannels[i].Name))
                        m_meterDataSet.DataSeries.Add(series.Multiply(1000.0));
                    else
                        m_meterDataSet.DataSeries.Add(series);
                }

                for (int i = 0; i < report.AnalogSection.DigitalChannels.Count; i++)
                {
                    channel = MakeParsedDigital(report, i);
                    series = new DataSeries();

                    timeSamples = report.AnalogSection.TimeChannel.Samples;
                    valueSamples = report.AnalogSection.DigitalChannels[i].Samples.Select(Convert.ToDouble).ToList();

                    series.SeriesInfo = channel.Series[0];

                    series.DataPoints = timeSamples
                        .Zip(valueSamples, (time, value) => new DataPoint() { Time = time, Value = value })
                        .ToList();

                    m_meterDataSet.Digitals.Add(series);
                }
            }
        }

        private Channel MakeParsedAnalog(EventReport report, int channelIndex)
        {
            Channel channel = new Channel();
            Series series = new Series();
            Channel<double> analogChannel = report.AnalogSection.AnalogChannels[channelIndex];

            channel.Name = analogChannel.Name;
            channel.HarmonicGroup = 0;
            channel.MeasurementType = new MeasurementType();
            channel.MeasurementCharacteristic = new MeasurementCharacteristic();
            channel.MeasurementCharacteristic.Name = "Instantaneous";
            channel.Phase = new Phase();

            series.Channel = channel;
            series.SeriesType = new SeriesType();
            series.SeriesType.Name = "Values";
            series.SourceIndexes = channelIndex.ToString();

            switch (analogChannel.Name)
            {
                case "VA": case "VB": case "VC":
                case "VS": case "VDC": case "Freq":
                    channel.MeasurementType.Name = "Voltage";
                    break;

                case "IA": case "IB": case "IC":
                case "IN": case "IG": case "IR":
                    channel.MeasurementType.Name = "Current";
                    break;

                default:
                    channel.MeasurementType.Name = "Unknown";
                    break;
            }

            switch (analogChannel.Name)
            {
                case "VA": case "IA": case "Freq":
                    channel.Phase.Name = "AN";
                    channel.Phase.Description = "A-phase to neutral";
                    break;

                case "VB": case "IB":
                    channel.Phase.Name = "BN";
                    channel.Phase.Description = "B-phase to neutral";
                    break;

                case "VC": case "IC":
                    channel.Phase.Name = "CN";
                    channel.Phase.Description = "C-phase to neutral";
                    break;

                case "IN":
                    channel.Phase.Name = "NG";
                    channel.Phase.Description = "Neutral to ground";
                    break;

                case "IG":
                    channel.Phase.Name = "Ground";
                    channel.Phase.Description = "Ground";
                    break;

                case "IR":
                    channel.Phase.Name = "RES";
                    channel.Phase.Description = "Residual";
                    break;

                default: case "VS": case "VDC":
                    channel.Phase.Name = "Unknown";
                    channel.Phase.Description = "Unknown";
                    break;
            }

            channel.MeasurementType.Description = channel.MeasurementType.Name;
            channel.MeasurementCharacteristic.Description = channel.MeasurementCharacteristic.Name;
            series.SeriesType.Description = series.SeriesType.Name;

            return channel;
        }

        private Channel MakeParsedDigital(EventReport report, int channelIndex)
        {
            Channel channel = new Channel();
            Series series = new Series();
            Channel<bool> digitalChannel = report.AnalogSection.DigitalChannels[channelIndex];

            channel.Name = digitalChannel.Name;
            channel.HarmonicGroup = 0;
            channel.MeasurementType = new MeasurementType();
            channel.MeasurementType.Name = "Digital";
            channel.MeasurementCharacteristic = new MeasurementCharacteristic();
            channel.MeasurementCharacteristic.Name = "Instantaneous";
            channel.Phase = new Phase();
            channel.Phase.Name = "None";

            series.Channel = channel;
            series.SeriesType = new SeriesType();
            series.SeriesType.Name = "Values";
            series.SourceIndexes = channelIndex.ToString();

            channel.MeasurementType.Description = channel.MeasurementType.Name;
            channel.MeasurementCharacteristic.Description = channel.MeasurementCharacteristic.Name;
            channel.Phase.Description = "No phase";
            series.SeriesType.Description = series.SeriesType.Name;

            return channel;
        }

        private Channel MakeParsedAnalog(CommaSeparatedEventReport report, int channelIndex)
        {
            Channel channel = new Channel();
            Series series = new Series();
            Channel<double> analogChannel = report.AnalogSection.AnalogChannels[channelIndex];

            channel.Name = analogChannel.Name;
            channel.HarmonicGroup = 0;
            channel.MeasurementType = new MeasurementType();
            channel.MeasurementCharacteristic = new MeasurementCharacteristic();
            channel.MeasurementCharacteristic.Name = "Instantaneous";
            channel.Phase = new Phase();

            series.Channel = channel;
            series.SeriesType = new SeriesType();
            series.SeriesType.Name = "Values";
            series.SourceIndexes = channelIndex.ToString();

            switch (analogChannel.Name)
            {
                case "VA": case "VB": case "VC":
                case "VS": case "VDC": case "Freq":
                    channel.MeasurementType.Name = "Voltage";
                    break;

                case "IA": case "IB": case "IC":
                case "IN": case "IG": case "IR":
                    channel.MeasurementType.Name = "Current";
                    break;

                default:
                    channel.MeasurementType.Name = "Unknown";
                    break;
            }

            switch (analogChannel.Name)
            {
                case "VA": case "IA": case "Freq":
                    channel.Phase.Name = "AN";
                    channel.Phase.Description = "A-phase to neutral";
                    break;

                case "VB": case "IB":
                    channel.Phase.Name = "BN";
                    channel.Phase.Description = "B-phase to neutral";
                    break;

                case "VC": case "IC":
                    channel.Phase.Name = "CN";
                    channel.Phase.Description = "C-phase to neutral";
                    break;

                case "IN":
                    channel.Phase.Name = "NG";
                    channel.Phase.Description = "Neutral to ground";
                    break;

                case "IG":
                    channel.Phase.Name = "Ground";
                    channel.Phase.Description = "Ground";
                    break;

                case "IR":
                    channel.Phase.Name = "RES";
                    channel.Phase.Description = "Residual";
                    break;

                default: case "VS": case "VDC":
                    channel.Phase.Name = "Unknown";
                    channel.Phase.Description = "Unknown";
                    break;
            }

            channel.MeasurementType.Description = channel.MeasurementType.Name;
            channel.MeasurementCharacteristic.Description = channel.MeasurementCharacteristic.Name;
            series.SeriesType.Description = series.SeriesType.Name;

            return channel;
        }

        private Channel MakeParsedDigital(CommaSeparatedEventReport report, int channelIndex)
        {
            Channel channel = new Channel();
            Series series = new Series();
            Channel<bool> digitalChannel = report.AnalogSection.DigitalChannels[channelIndex];

            channel.Name = digitalChannel.Name;
            channel.HarmonicGroup = 0;
            channel.MeasurementType = new MeasurementType();
            channel.MeasurementType.Name = "Digital";
            channel.MeasurementCharacteristic = new MeasurementCharacteristic();
            channel.MeasurementCharacteristic.Name = "Instantaneous";
            channel.Phase = new Phase();
            channel.Phase.Name = "None";

            series.Channel = channel;
            series.SeriesType = new SeriesType();
            series.SeriesType.Name = "Values";
            series.SourceIndexes = channelIndex.ToString();

            channel.MeasurementType.Description = channel.MeasurementType.Name;
            channel.MeasurementCharacteristic.Description = channel.MeasurementCharacteristic.Name;
            channel.Phase.Description = "No phase";
            series.SeriesType.Description = series.SeriesType.Name;

            return channel;
        }

        #endregion
    }
}
