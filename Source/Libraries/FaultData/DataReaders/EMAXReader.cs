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
using System.Text;
using System.Text.RegularExpressions;
using FaultData.DataAnalysis;
using FaultData.DataSets;
using GSF.COMTRADE;
using GSF.Configuration;
using GSF.EMAX;
using log4net;
using openXDA.Configuration;
using openXDA.Model;

namespace FaultData.DataReaders
{
    public class EMAXReader : IDataReader
    {
        #region [ Members ]

        // Nested Types
        private class COMTRADEExporter
        {
            public string ExportDirectory { get; set; }

            public MeterDataSet MeterDataSet { get; set; }
            public string FilePath { get; set; }
            public string MeterKey { get; set; }
            public string IdentityString { get; set; }

            public ControlFile ControlFile { get; set; }
            public List<ANLG_CHNL_NEW> AnalogChannels { get; set; }
            public List<EVNT_CHNL_NEW> DigitalChannels { get; set; }

            public void ExportToCOMTRADE()
            {
                string rootFileName = GSF.IO.FilePath.GetFileNameWithoutExtension(FilePath);
                string directoryPath = Path.Combine(ExportDirectory, MeterKey);
                string schemaFilePath = Path.Combine(directoryPath, $"{rootFileName}.cfg");
                string dataFilePath = Path.Combine(directoryPath, $"{rootFileName}.dat");
                Schema comtradeSchema = new Schema();

                if (File.Exists(dataFilePath))
                    return;

                comtradeSchema.StationName = Regex.Replace(IdentityString, @"[\r\n]", "");
                comtradeSchema.Version = 2013;
                comtradeSchema.AnalogChannels = new AnalogChannel[ControlFile.AnalogChannelCount];
                comtradeSchema.DigitalChannels = new DigitalChannel[ControlFile.EventChannelSettings.Count];
                comtradeSchema.SampleRates = new SampleRate[1];
                comtradeSchema.SampleRates[0].Rate = ControlFile.SystemParameters.samples_per_second;
                comtradeSchema.SampleRates[0].EndSample = ControlFile.SystemParameters.rcd_sample_count - 1;
                comtradeSchema.StartTime = new Timestamp() { Value = MeterDataSet.DataSeries[1][0].Time };

                int triggerIndex = ControlFile.SystemParameters.start_offset_samples + ControlFile.SystemParameters.prefault_samples;
                comtradeSchema.TriggerTime = new Timestamp() { Value = MeterDataSet.DataSeries[1][triggerIndex].Time };

                for (int i = 0; i < AnalogChannels.Count; i++)
                {
                    ANLG_CHNL_NEW analogChannel = AnalogChannels[i];
                    AnalogChannel comtradeAnalog = new AnalogChannel();
                    DataSeries channelData = MeterDataSet.DataSeries[i + 1];

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

                for (int i = 0; i < DigitalChannels.Count; i++)
                {
                    EVNT_CHNL_NEW digitalChannel = DigitalChannels[i];
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

                    IEnumerable<DataSeries> digitalWords = MeterDataSet.Digitals.Skip(1)
                        .Select((dataSeries, index) => dataSeries.Multiply(Math.Pow(2.0D, DigitalSize - (index % DigitalSize) - 1)))
                        .Select((DataSeries, Index) => new { DataSeries, Index })
                        .GroupBy(obj => obj.Index / DigitalSize)
                        .Select(grouping => grouping.Select(obj => obj.DataSeries))
                        .Select(grouping => grouping.Aggregate((sum, series) => sum.Add(series)));

                    List<DataSeries> allChannels = MeterDataSet.DataSeries.Skip(1)
                        .Select((dataSeries, index) => AnalogChannels[index].type.All(char.IsUpper) ? dataSeries.Multiply(0.001D) : dataSeries)
                        .Concat(digitalWords)
                        .ToList();

                    for (int i = 0; i < MeterDataSet.DataSeries[1].DataPoints.Count; i++)
                    {
                        DateTime timestamp = MeterDataSet.DataSeries[1][i].Time;

                        double[] values = allChannels
                            .Select(dataSeries => dataSeries[i].Value)
                            .ToArray();

                        Writer.WriteNextRecordBinary(stream, comtradeSchema, timestamp, values, (uint)i, false);
                    }
                }
            }
        }

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Settings to configure native EMAX file format integration.
        /// </summary>
        [Category]
        [SettingName(EMAXSection.CategoryName)]
        public EMAXSection EMAXSettings { get; }
            = new EMAXSection();

        [Category]
        [SettingName(FileProcessorSection.CategoryName)]
        public FileProcessorSection FileProcessorSettings { get; }
            = new FileProcessorSection();

        #endregion

        #region [ Methods ]

        public bool IsReadyForLoad(FileInfo[] fileList)
        {
            FileInfo FromExtension(string extension) => fileList
                .FirstOrDefault(fileInfo => fileInfo.Extension.Equals(extension, StringComparison.OrdinalIgnoreCase));

            bool IsReady(FileInfo fileInfo) =>
                !(fileInfo is null) &&
                fileInfo.Length > 0L;

            FileInfo controlFile = FromExtension(".ctl");
            FileInfo rcdFile = FromExtension(".rcd");
            FileInfo rclFile = FromExtension(".rcl");
            FileInfo rcuFile = FromExtension(".rcu");

            if (!IsReady(controlFile))
                return false;

            if (!IsReady(rcdFile) && !(IsReady(rclFile) && IsReady(rcuFile)))
                return false;

            return true;
        }

        public DataFile GetPrimaryDataFile(FileGroup fileGroup)
        {
            bool IsPrimaryDataFile(DataFile dataFile) =>
                dataFile.FilePath.EndsWith(".rcd", StringComparison.OrdinalIgnoreCase) ||
                dataFile.FilePath.EndsWith(".rcl", StringComparison.OrdinalIgnoreCase);

            return fileGroup.DataFiles.First(IsPrimaryDataFile);
        }

        public MeterDataSet Parse(FileGroup fileGroup)
        {
            bool IsControlFile(DataFile dataFile) =>
                dataFile.FilePath.EndsWith(".ctl", StringComparison.OrdinalIgnoreCase);

            string firstFilePath = fileGroup.DataFiles.Select(dataFile => dataFile.FilePath).First();
            string tempDataFolderName = Path.GetFileNameWithoutExtension(firstFilePath);
            string tempDataFolderPath = Path.Combine(TempDataFolder, tempDataFolderName);

            string GetPathInTempDataFolder(DataFile dataFile)
            {
                string fileName = Path.GetFileName(dataFile.FilePath);
                return Path.Combine(tempDataFolderPath, fileName);
            }

            try
            {
                if (Directory.Exists(tempDataFolderPath))
                    Directory.Delete(tempDataFolderPath, true);

                Directory.CreateDirectory(tempDataFolderPath);
                Dump(fileGroup, tempDataFolderPath);

                string controlFilePath = fileGroup.DataFiles
                    .Where(IsControlFile)
                    .Select(GetPathInTempDataFolder)
                    .First();

                DataFile primaryDataFile = GetPrimaryDataFile(fileGroup);
                string primaryDataFilePath = GetPathInTempDataFolder(primaryDataFile);

                using (CorrectiveParser parser = new CorrectiveParser())
                {
                    parser.ControlFile = new ControlFile(controlFilePath);
                    parser.FileName = primaryDataFilePath;
                    parser.OpenFiles();
                    return Parse(parser);
                }
            }
            finally
            {
                if (Directory.Exists(tempDataFolderPath))
                    Directory.Delete(tempDataFolderPath, true);
            }
        }

        public MeterDataSet Parse(CorrectiveParser parser)
        {
            MeterDataSet meterDataSet = new MeterDataSet();
            meterDataSet.Meter = new Meter();

            ControlFile controlFile = parser.ControlFile;
            string identityString = controlFile.IdentityString.value;
            string deviceName = identityString.Substring(0, IndexOf(identityString, "\r\n", "\n", "\r"));

            Meter meter = meterDataSet.Meter;
            meter.Location = new Location();
            meter.Channels = new List<Channel>();
            meter.AssetKey = deviceName;
            meter.Name = deviceName;
            meter.ShortName = deviceName.Substring(0, Math.Min(deviceName.Length, 50));

            Location meterLocation = meter.Location;
            meterLocation.Meters = new List<Meter>() { meter };
            meterLocation.LocationKey = deviceName;
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
            meterDataSet.DataSeries.Add(new DataSeries());
            meterDataSet.Digitals.Add(new DataSeries());

            foreach (ANLG_CHNL_NEW analogChannel in analogChannels)
            {
                Channel channel = ParseSeries(analogChannel);
                channel.Meter = meter;
                channel.Series.Single().SourceIndexes = meterDataSet.DataSeries.Count.ToString();

                DataSeries dataSeries = new DataSeries();
                dataSeries.SeriesInfo = channel.Series[0];

                meter.Channels.Add(channel);

                meterDataSet.DataSeries.Add(dataSeries);
            }

            foreach (EVNT_CHNL_NEW digitalChannel in digitalChannels)
            {
                Channel channel = ParseSeries(digitalChannel);
                channel.Series.Single().SourceIndexes = meterDataSet.Digitals.Count.ToString();

                DataSeries series = new DataSeries();
                series.SeriesInfo = channel.Series[0];

                meter.Channels.Add(channel);

                meterDataSet.Digitals.Add(series);
            }

            while (parser.ReadNext())
            {
                DateTime timestamp = EMAXSettings.ApplyTimestampCorrection ? parser.CalculatedTimestamp : parser.ParsedTimestamp;

                for (int i = 0; i < analogChannels.Count; i++)
                {
                    double value = EMAXSettings.ApplyValueCorrection ? parser.CorrectedValues[i] : parser.Values[i];
                    meterDataSet.DataSeries[i + 1].DataPoints.Add(new DataPoint() { Time = timestamp, Value = value });
                }

                for (int i = 0; i < digitalChannels.Count; i++)
                {
                    int bitCount = sizeof(ushort) * 8;
                    int groupIndex = i / bitCount;
                    int bitIndex = i % bitCount;
                    ushort mask = (ushort)~(0x8000u >> bitIndex);
                    double value = parser.EventGroups[groupIndex] & mask;
                    meterDataSet.Digitals[i + 1].DataPoints.Add(new DataPoint() { Time = timestamp, Value = value });
                }
            }

            try
            {
                if (!string.IsNullOrEmpty(EMAXSettings.COMTRADEExportDirectory))
                {
                    string filePath = parser.FileName;

                    string meterKey = GetMeterKey(filePath, FileProcessorSettings.FilePattern)
                        ?? ThreadContext.Properties["Meter"].ToString();

                    COMTRADEExporter exporter = new COMTRADEExporter()
                    {
                        ExportDirectory = EMAXSettings.COMTRADEExportDirectory,

                        MeterDataSet = meterDataSet,
                        FilePath = filePath,
                        MeterKey = meterKey,
                        IdentityString = identityString,

                        ControlFile = controlFile,
                        AnalogChannels = analogChannels,
                        DigitalChannels = digitalChannels
                    };

                    exporter.ExportToCOMTRADE();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
            }

            return meterDataSet;
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
            Series series = new Series();
            series.Channel = new Channel();
            series.SeriesType = new SeriesType() { Name = "Instantaneous" };
            series.SourceIndexes = digitalChannel.eventnum;

            Channel channel = series.Channel;
            channel.Series = new List<Series>() { series };
            channel.MeasurementType = new MeasurementType() { Name = "Digital" };
            channel.MeasurementCharacteristic = new MeasurementCharacteristic() { Name = "Unknown" };
            channel.Phase = new Phase() { Name = "Unknown" };
            channel.Name = digitalChannel.e_title;
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

        #region [ Static ]

        // Static Fields
        private static readonly ILog Log = LogManager.GetLogger(typeof(EMAXReader));

        // Static Properties
        private static string TempDataFolder => LazyTempDataFolder.Value;

        private static Lazy<string> LazyTempDataFolder => new Lazy<string>(() =>
        {
            string tempPath = Path.GetTempPath();
            return Path.Combine(tempPath, "openXDA", "EMAX");
        });

        // Static Methods
        private static void Dump(FileGroup fileGroup, string path)
        {
            foreach (DataFile dataFile in fileGroup.DataFiles)
            {
                string fileName = Path.GetFileName(dataFile.FilePath);
                string destination = Path.Combine(path, fileName);
                byte[] data = dataFile.FileBlob.Blob;
                File.WriteAllBytes(destination, data);
            }
        }

        #endregion
    }
}
