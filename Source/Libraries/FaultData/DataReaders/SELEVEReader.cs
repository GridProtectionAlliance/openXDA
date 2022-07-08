﻿//******************************************************************************************************
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
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using FaultData.DataAnalysis;
using FaultData.DataSets;
using GSF;
using GSF.Configuration;
using GSF.SELEventParser;
using GSF.Units;
using openXDA.Configuration;
using openXDA.Model;

namespace FaultData.DataReaders
{
    /// <summary>
    /// Reads an SEL event file to produce a <see cref="MeterDataSet"/>.
    /// </summary>
    public class SELEVEReader : IDataReader
    {
        #region [ Properties ]

        [Category]
        [SettingName(DataAnalysisSection.CategoryName)]
        public DataAnalysisSection DataAnalysisSettings { get; }
            = new DataAnalysisSection();

        #endregion

        #region [ Methods ]

        public bool IsReadyForLoad(FileInfo[] fileList) =>
            fileList.All(fileInfo => fileInfo.Length > 0L);

        public DataFile GetPrimaryDataFile(FileGroup fileGroup) =>
            fileGroup.DataFiles.First();

        public MeterDataSet Parse(FileGroup fileGroup)
        {
            MeterDataSet meterDataSet = new MeterDataSet();
            meterDataSet.Meter = new Meter();

            DataFile dataFile = GetPrimaryDataFile(fileGroup);
            string tempDataFolderName = Path.GetFileNameWithoutExtension(dataFile.FilePath);

            // The temp data folder may be deeply nested;
            // long path syntax prevents errors due to large file names
            string tempDataFolderPath = @"\\?\" + Path.Combine(TempDataFolder, tempDataFolderName);

            string dataFileName = Path.GetFileName(dataFile.FilePath);
            string tempFilePath = Path.Combine(tempDataFolderPath, dataFileName);
            byte[] data = dataFile.FileBlob.Blob;

            try
            {
                if (Directory.Exists(tempDataFolderPath))
                    Directory.Delete(tempDataFolderPath, true);

                Directory.CreateDirectory(tempDataFolderPath);
                File.WriteAllBytes(tempFilePath, data);
                Parse(meterDataSet, tempFilePath);
            }
            finally
            {
                if (Directory.Exists(tempDataFolderPath))
                    Directory.Delete(tempDataFolderPath, true);
            }

            return meterDataSet;
        }

        private void Parse(MeterDataSet meterDataSet, string filePath)
        {
            double systemFrequency = DataAnalysisSettings.SystemFrequency;
            double maxEventDuration = DataAnalysisSettings.MaxEventDuration;
            EventFile eventFile = EventFile.Parse(filePath, systemFrequency, maxEventDuration);

            if (!eventFile.EventReports.Any() && !eventFile.CommaSeparatedEventReports.Any())
                return;

            Header header = eventFile.EventReports.FirstOrDefault()?.Header
                ?? eventFile.CommaSeparatedEventReports[0].Header;

            Meter meter = meterDataSet.Meter;
            meter.Location = new Location();
            meter.Channels = new List<Channel>();
            meter.AssetKey = header.RelayID;
            meter.Name = header.RelayID;
            meter.ShortName = new string(header.RelayID.ToNonNullString().Take(50).ToArray());

            Location meterLocation = meter.Location;
            meterLocation.Meters = new List<Meter>() { meter };
            meterLocation.LocationKey = header.StationID;
            meterLocation.Name = header.StationID;
            meterLocation.ShortName = new string(header.StationID.ToNonNullString().Take(50).ToArray());
            meterLocation.Description = header.StationID;

            foreach (EventReport report in eventFile.EventReports)
            {
                for (int i = 0; i < report.AnalogSection.AnalogChannels.Count; i++)
                {
                    Channel channel = MakeParsedAnalog(report, i);
                    channel.Meter = meter;
                    meter.Channels.Add(channel);

                    List<DateTime> timeSamples = report.AnalogSection.TimeChannel.Samples;
                    List<double> valueSamples = report.AnalogSection.AnalogChannels[i].Samples;
                    DataSeries dataSeries = new DataSeries();

                    dataSeries.DataPoints = timeSamples
                        .Zip(valueSamples, (time, value) => new DataPoint() { Time = time, Value = value })
                        .ToList();

                    if (new string[] { "VA", "VB", "VC", "VS" }.Contains(report.AnalogSection.AnalogChannels[i].Name))
                        dataSeries = dataSeries.Multiply(1000.0D);

                    dataSeries.SeriesInfo = channel.Series[0];
                    meterDataSet.DataSeries.Add(dataSeries);
                }

                for (int i = 0; i < report.AnalogSection.DigitalChannels.Count; i++)
                {
                    Channel channel = MakeParsedDigital(report, i);

                    if (channel.Name == "*")
                        continue;

                    channel.Meter = meter;
                    meter.Channels.Add(channel);

                    List<DateTime> timeSamples = report.AnalogSection.TimeChannel.Samples;
                    List<bool?> digitalSamples = report.AnalogSection.DigitalChannels[i].Samples;
                    DataSeries dataSeries = new DataSeries();

                    dataSeries.SeriesInfo = channel.Series[0];

                    dataSeries.DataPoints = timeSamples
                        .Zip(digitalSamples, (time, value) => new { Time = time, Value = value })
                        .Where(x => x.Value != null)
                        .Select(x => new DataPoint { Time = x.Time, Value = Convert.ToDouble(x.Value) })
                        .ToList();

                    meterDataSet.Digitals.Add(dataSeries);
                }

                ComplexNumber z1 = new ComplexNumber(0.0D, 0.0D);
                ComplexNumber z0 = new ComplexNumber(0.0D, 0.0D);
                double groupSetting;

                if (double.TryParse(report.GetGroupSettings("Z1MAG"), out groupSetting))
                    z1.Magnitude = groupSetting;

                if (double.TryParse(report.GetGroupSettings("Z1ANG"), out groupSetting))
                    z1.Angle = Angle.FromDegrees(groupSetting);

                if (double.TryParse(report.GetGroupSettings("Z0MAG"), out groupSetting))
                    z0.Magnitude = groupSetting;

                if (double.TryParse(report.GetGroupSettings("Z0ANG"), out groupSetting))
                    z0.Angle = Angle.FromDegrees(groupSetting);


            }

            foreach (CommaSeparatedEventReport report in eventFile.CommaSeparatedEventReports)
            {
                AnalysisDataSet analysisDataSet = new AnalysisDataSet();
                analysisDataSet.EventType = report.Event;
                meterDataSet.AnalysisDataSet = analysisDataSet;

                if (!double.IsNaN(report.Location))
                    analysisDataSet.FaultLocation = report.Location;

                for (int i = 0; i < report.AnalogSection.AnalogChannels.Count; i++)
                {
                    Channel channel = MakeParsedAnalog(report, i);
                    channel.Meter = meter;
                    meter.Channels.Add(channel);

                    List<DateTime> timeSamples = report.AnalogSection.TimeChannel.Samples;
                    List<double> valueSamples = report.AnalogSection.AnalogChannels[i].Samples;
                    DataSeries dataSeries = new DataSeries();

                    dataSeries.DataPoints = timeSamples
                        .Zip(valueSamples, (time, value) => new DataPoint() { Time = time, Value = value })
                        .ToList();

                    dataSeries.SeriesInfo = channel.Series[0];
                    meterDataSet.DataSeries.Add(dataSeries);
                }

                for (int i = 0; i < report.AnalogSection.DigitalChannels.Count; i++)
                {
                    Channel channel = MakeParsedDigital(report, i);

                    if (channel.Name == "*")
                        continue;

                    channel.Meter = meter;
                    meter.Channels.Add(channel);

                    List<DateTime> timeSamples = report.AnalogSection.TimeChannel.Samples;
                    List<bool?> digitalSamples = report.AnalogSection.DigitalChannels[i].Samples;
                    DataSeries dataSeries = new DataSeries();

                    dataSeries.SeriesInfo = channel.Series[0];

                    dataSeries.DataPoints = timeSamples
                        .Zip(digitalSamples, (time, value) => new { Time = time, Value = value })
                        .Where(x => x.Value != null)
                        .Select(x => new DataPoint { Time = x.Time, Value = Convert.ToDouble(x.Value) })
                        .ToList();

                    meterDataSet.Digitals.Add(dataSeries);
                }
            }
        }

        private Channel MakeParsedAnalog(EventReport report, int channelIndex)
        {
            Channel<double> analogChannel = report.AnalogSection.AnalogChannels[channelIndex];

            Series series = new Series();
            series.Channel = new Channel();
            series.SeriesType = new SeriesType();
            series.SeriesType.Name = "Values";
            series.SourceIndexes = channelIndex.ToString();

            Channel channel = series.Channel;
            channel.Series = new List<Series>() { series };
            channel.MeasurementType = new MeasurementType();
            channel.MeasurementCharacteristic = new MeasurementCharacteristic() { Name = "Instantaneous" };
            channel.Phase = new Phase();
            channel.Name = analogChannel.Name;
            channel.HarmonicGroup = 0;

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

            if (analogChannel.Name == "Freq")
                channel.MeasurementCharacteristic.Name = "Frequency";

            channel.MeasurementType.Description = channel.MeasurementType.Name;
            channel.MeasurementCharacteristic.Description = channel.MeasurementCharacteristic.Name;
            series.SeriesType.Description = series.SeriesType.Name;

            return channel;
        }

        private Channel MakeParsedDigital(EventReport report, int channelIndex)
        {
            Channel<bool?> digitalChannel = report.AnalogSection.DigitalChannels[channelIndex];

            Series series = new Series();
            series.Channel = new Channel();
            series.SeriesType = new SeriesType();
            series.SeriesType.Name = "Values";
            series.SourceIndexes = channelIndex.ToString();

            Channel channel = series.Channel;
            channel.Series = new List<Series>() { series };
            channel.MeasurementType = new MeasurementType() { Name = "Digital" };
            channel.MeasurementCharacteristic = new MeasurementCharacteristic() { Name = "Instantaneous" };
            channel.Phase = new Phase() { Name = "None" };
            channel.Name = digitalChannel.Name;
            channel.HarmonicGroup = 0;

            channel.MeasurementType.Description = channel.MeasurementType.Name;
            channel.MeasurementCharacteristic.Description = channel.MeasurementCharacteristic.Name;
            channel.Phase.Description = "No phase";
            series.SeriesType.Description = series.SeriesType.Name;

            return channel;
        }

        private Channel MakeParsedAnalog(CommaSeparatedEventReport report, int channelIndex)
        {
            const string ChannelWithUnitsPattern = @"(?<Name>\S+)\s*\((?<Units>\S+)\)";

            Channel<double> analogChannel = report.AnalogSection.AnalogChannels[channelIndex];

            Series series = new Series();
            series.Channel = new Channel();
            series.SeriesType = new SeriesType();
            series.SeriesType.Name = "Values";
            series.SourceIndexes = channelIndex.ToString();

            Channel channel = series.Channel;
            channel.Series = new List<Series>() { series };
            channel.MeasurementType = new MeasurementType();
            channel.MeasurementCharacteristic = new MeasurementCharacteristic() { Name = "Instantaneous" };
            channel.Phase = new Phase();
            channel.Name = analogChannel.Name;
            channel.HarmonicGroup = 0;

            Match regexMatch = Regex.Match(analogChannel.Name, ChannelWithUnitsPattern);
            string channelName = regexMatch.Success ? regexMatch.Groups["Name"].Value : analogChannel.Name;

            switch (channelName)
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

            switch (channelName)
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
            Channel<bool?> digitalChannel = report.AnalogSection.DigitalChannels[channelIndex];

            Series series = new Series();
            series.Channel = new Channel();
            series.SeriesType = new SeriesType();
            series.SeriesType.Name = "Values";
            series.SourceIndexes = channelIndex.ToString();

            Channel channel = series.Channel;
            channel.Series = new List<Series>() { series };
            channel.MeasurementType = new MeasurementType() { Name = "Digital" };
            channel.MeasurementType.Name = "Digital";
            channel.MeasurementCharacteristic = new MeasurementCharacteristic() { Name = "Instantaneous" };
            channel.Phase = new Phase() { Name = "None" };
            channel.Name = digitalChannel.Name;
            channel.HarmonicGroup = 0;

            channel.MeasurementType.Description = channel.MeasurementType.Name;
            channel.MeasurementCharacteristic.Description = channel.MeasurementCharacteristic.Name;
            channel.Phase.Description = "No phase";
            series.SeriesType.Description = series.SeriesType.Name;

            return channel;
        }

        #endregion

        #region [ Static ]

        // Static Properties
        private static string TempDataFolder => LazyTempDataFolder.Value;

        private static Lazy<string> LazyTempDataFolder => new Lazy<string>(() =>
        {
            string tempPath = Path.GetTempPath();
            return Path.Combine(tempPath, "openXDA", "SEL");
        });

        #endregion
    }
}
