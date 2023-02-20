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
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using FaultData.DataAnalysis;
using FaultData.DataSets;
using FaultData.DataSets.GTC;
using GSF.COMTRADE;
using GSF.Configuration;
using GSF.Interop;
using log4net;
using openXDA.Configuration;
using openXDA.Model;

namespace FaultData.DataReaders
{
    /// <summary>
    /// Reads a COMTRADE file to produce a <see cref="MeterDataSet"/>.
    /// </summary>
    public class COMTRADEReader : IDataReader
    {
        #region [ Properties ]

        [Category]
        [SettingName(COMTRADESection.CategoryName)]
        public COMTRADESection Settings { get; }
            = new COMTRADESection();

        #endregion

        #region [ Methods ]

        public bool IsReadyForLoad(FileInfo[] fileList)
        {
            FileInfo FromExtension(string extension) => fileList
                .FirstOrDefault(fileInfo => fileInfo.Extension.Equals(extension, StringComparison.OrdinalIgnoreCase));

            bool IsDataFile(FileInfo fileInfo) =>
                fileInfo.Extension.StartsWith(".d", StringComparison.OrdinalIgnoreCase);

            bool IsDatFile(FileInfo fileInfo) =>
                fileInfo.Extension.Equals(".dat", StringComparison.OrdinalIgnoreCase);

            FileInfo schemaFile = FromExtension(".cfg");
            FileInfo infFile = FromExtension(".inf");
            FileInfo[] dataFiles = fileList.Where(IsDataFile).ToArray();
            bool multipleDataFiles = dataFiles.Any(fileInfo => !IsDatFile(fileInfo));

            DateTime fileCreationTime = fileList.Max(fileInfo => fileInfo.CreationTimeUtc);
            bool minWaitTimePassed = (DateTime.UtcNow - fileCreationTime) >= Settings.MinWaitTime;

            if (schemaFile is null || schemaFile.Length == 0L)
                return false;

            if (!dataFiles.Any())
                return false;

            if (dataFiles.Any(fileInfo => fileInfo.Length == 0L))
                return false;

            if (multipleDataFiles && !minWaitTimePassed)
                return false;

            if (Settings.WaitForINF && !minWaitTimePassed && infFile is null)
                return false;

            return true;
        }

        public DataFile GetPrimaryDataFile(FileGroup fileGroup)
        {
            bool IsPrimaryDataFile(DataFile dataFile) =>
                dataFile.FilePath.EndsWith(".dat", StringComparison.OrdinalIgnoreCase) ||
                dataFile.FilePath.EndsWith(".d00", StringComparison.OrdinalIgnoreCase);

            return fileGroup.DataFiles.First(IsPrimaryDataFile);
        }

        public MeterDataSet Parse(FileGroup fileGroup)
        {
            bool IsSchemaFile(DataFile dataFile) =>
                dataFile.FilePath.EndsWith(".cfg", StringComparison.OrdinalIgnoreCase);

            bool IsHeaderFile(DataFile dataFile) =>
                dataFile.FilePath.EndsWith(".hdr", StringComparison.OrdinalIgnoreCase);

            bool IsINFFile(DataFile dataFile) =>
                dataFile.FilePath.EndsWith(".inf", StringComparison.OrdinalIgnoreCase);

            string firstFilePath = fileGroup.DataFiles.Select(dataFile => dataFile.FilePath).First();
            string tempDataFolderName = Path.GetFileNameWithoutExtension(firstFilePath);

            // The temp data folder may be deeply nested;
            // long path syntax prevents errors due to large file names
            string tempDataFolderPath = @"\\?\" + Path.Combine(TempDataFolder, tempDataFolderName);

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

                string schemaFilePath = fileGroup.DataFiles
                    .Where(IsSchemaFile)
                    .Select(GetPathInTempDataFolder)
                    .First();

                DataFile primaryDataFile = GetPrimaryDataFile(fileGroup);
                string primaryDataFilePath = GetPathInTempDataFolder(primaryDataFile);
                MeterDataSet meterDataSet;

                using (Parser parser = new Parser())
                {
                    parser.Schema = new Schema(schemaFilePath, Settings.UseRelaxedValidation);
                    parser.FileName = primaryDataFilePath;
                    parser.InferTimeFromSampleRates = true;
                    parser.AdjustToUTC = false;
                    parser.OpenFiles();
                    meterDataSet = Parse(parser);
                }

                string infFilePath = fileGroup.DataFiles
                    .Where(IsINFFile)
                    .Select(GetPathInTempDataFolder)
                    .FirstOrDefault();

                if (File.Exists(infFilePath))
                {
                    IniFile infFile = new IniFile(infFilePath);
                    INFDataSet infDataSet = new INFDataSet(infFile);
                    BreakerRestrikeDataSet breakerRestrikeDataSet = BreakerRestrikeDataSet.Create(infDataSet);
                    meterDataSet.BreakerRestrikeDataSet = breakerRestrikeDataSet;
                }

                string headerData = fileGroup.DataFiles
                    .Where(IsHeaderFile)
                    .Select(file => file.FileBlob?.Blob ?? Array.Empty<byte>())
                    .Select(blob => Encoding.UTF8.GetString(blob))
                    .FirstOrDefault();

                if (!string.IsNullOrEmpty(headerData))
                    ParseSELHeader(meterDataSet, headerData);

                return meterDataSet;
            }
            finally
            {
                if (Directory.Exists(tempDataFolderPath))
                    Directory.Delete(tempDataFolderPath, true);
            }
        }

        private void ParseSELHeader(MeterDataSet meterDataSet, string headerData)
        {
            Lazy<AnalysisDataSet> lazyAnalysisDataSet = new Lazy<AnalysisDataSet>(() => meterDataSet.AnalysisDataSet = new AnalysisDataSet());

            string[] sections = headerData.Trim()
                .Replace("\r\n", "\n")
                .Replace('\r', '\n')
                .Split(new string[] { "\n\n" }, StringSplitOptions.None);

            if (sections.Length < 1 || !sections[0].Contains("SEL"))
                return;

            if (sections.Length < 2)
                return;

            MatchCollection eventSummaryMatches = Regex.Matches(sections[1], @"Event: (?<Type>\S+)|Location: (?<Location>\S+)");

            foreach (Match match in eventSummaryMatches)
            {
                Group typeGroup = match.Groups["Type"];
                if (typeGroup.Success)
                    lazyAnalysisDataSet.Value.EventType = typeGroup.Value;

                Group locationGroup = match.Groups["Location"];
                if (locationGroup.Success && double.TryParse(locationGroup.Value, out double location))
                    lazyAnalysisDataSet.Value.FaultLocation = location;
            }

            if (sections.Length < 3)
                return;

            string[] measurementSummaryLines = sections[2].Split('\n');
            MatchCollection measurementSummaryHeaders = Regex.Matches(measurementSummaryLines[0], @"I[ABCNG]|V[ABC]|3I2");

            IEnumerable<string> faultMeasurementLines = measurementSummaryLines
                .SkipWhile(line => !line.StartsWith("Fault:"))
                .Skip(1)
                .Take(2);

            foreach (string faultMeasurementLine in faultMeasurementLines)
            {
                if (!faultMeasurementLine.StartsWith("MAG"))
                    continue;

                foreach (Match header in measurementSummaryHeaders)
                {
                    int end = header.Index + header.Length;

                    string token = (end < faultMeasurementLine.Length)
                        ? faultMeasurementLine.Remove(end)
                        : faultMeasurementLine;

                    Match match = Regex.Match(token, @"-?[\d\.]+$");
                    if (!match.Success || !double.TryParse(match.Value, out double measurement))
                        continue;

                    switch (header.Value)
                    {
                        case "VA":
                            lazyAnalysisDataSet.Value.VA = measurement;
                            break;
                        case "VB":
                            lazyAnalysisDataSet.Value.VB = measurement;
                            break;
                        case "VC":
                            lazyAnalysisDataSet.Value.VC = measurement;
                            break;

                        case "IA":
                            lazyAnalysisDataSet.Value.IA = measurement;
                            break;
                        case "IB":
                            lazyAnalysisDataSet.Value.IB = measurement;
                            break;
                        case "IC":
                            lazyAnalysisDataSet.Value.IC = measurement;
                            break;
                        case "IN":
                            lazyAnalysisDataSet.Value.IN = measurement;
                            break;
                        case "IG":
                            lazyAnalysisDataSet.Value.IG = measurement;
                            break;

                        case "3I2":
                            lazyAnalysisDataSet.Value.INeg3 = measurement;
                            break;
                    }
                }
            }
        }

        private MeterDataSet Parse(Parser parser)
        {
            Schema schema = parser.Schema;

            MeterDataSet meterDataSet = new MeterDataSet();
            meterDataSet.Meter = new Meter();

            Meter meter = meterDataSet.Meter;
            meter.Location = new Location();
            meter.Channels = new List<Channel>();
            meter.AssetKey = schema.DeviceID;
            meter.Name = schema.DeviceID;
            meter.ShortName = schema.DeviceID.Substring(0, Math.Min(schema.DeviceID.Length, 50));

            Location meterLocation = meter.Location;
            meterLocation.Meters = new List<Meter>() { meter };
            meterLocation.LocationKey = schema.StationName;
            meterLocation.Name = schema.StationName;
            meterLocation.ShortName = schema.StationName.Substring(0, Math.Min(schema.StationName.Length, 50));
            meterLocation.Description = schema.StationName;

            foreach (AnalogChannel analogChannel in schema.AnalogChannels)
            {
                Channel channel = ParseSeries(analogChannel);
                channel.Meter = meter;

                DataSeries dataSeries = new DataSeries();
                dataSeries.SeriesInfo = channel.Series[0];

                meter.Channels.Add(channel);

                while (meterDataSet.DataSeries.Count <= analogChannel.Index)
                    meterDataSet.DataSeries.Add(new DataSeries());

                meterDataSet.DataSeries[analogChannel.Index] = dataSeries;
            }

            foreach (DigitalChannel digitalChannel in schema.DigitalChannels)
            {
                Channel channel = ParseSeries(digitalChannel);
                channel.Meter = meter;

                DataSeries dataSeries = new DataSeries();
                dataSeries.SeriesInfo = channel.Series[0];

                meter.Channels.Add(channel);

                while (meterDataSet.Digitals.Count <= digitalChannel.Index)
                    meterDataSet.Digitals.Add(new DataSeries());

                meterDataSet.Digitals[digitalChannel.Index] = dataSeries;
            }

            try
            {
                while (parser.ReadNext())
                {
                    for (int i = 0; i < schema.AnalogChannels.Length; i++)
                    {
                        int seriesIndex = schema.AnalogChannels[i].Index;
                        string units = schema.AnalogChannels[i].Units.ToUpper();
                        double multiplier = (units.Contains("KA") || units.Contains("KV")) ? 1000.0D : 1.0D;
                        meterDataSet.DataSeries[seriesIndex].DataPoints.Add(new DataPoint() { Time = parser.Timestamp, Value = multiplier * parser.PrimaryValues[i] });
                    }

                    for (int i = 0; i < schema.DigitalChannels.Length; i++)
                    {
                        int valuesIndex = schema.TotalAnalogChannels + i;
                        int seriesIndex = schema.DigitalChannels[i].Index;
                        meterDataSet.Digitals[seriesIndex].DataPoints.Add(new DataPoint() { Time = parser.Timestamp, Value = parser.Values[valuesIndex] });
                    }
                }
            }
            catch (InvalidOperationException ex)
            {
                Log.Warn(ex.Message, ex);
            }

            return meterDataSet;
        }

        private Channel ParseSeries(AnalogChannel analogChannel)
        {
            string phaseName = !string.IsNullOrEmpty(analogChannel.PhaseID) ? analogChannel.PhaseID : "Unknown";

            Series series = new Series();
            series.Channel = new Channel();
            series.SeriesType = new SeriesType() { Name = "Values" };
            series.SourceIndexes = analogChannel.Index.ToString();

            Channel channel = series.Channel;
            channel.Series = new List<Series>() { series };
            channel.MeasurementType = new MeasurementType() { Name = "Unknown" };
            channel.MeasurementCharacteristic = new MeasurementCharacteristic() { Name = "Unknown" };
            channel.Phase = new Phase() { Name = phaseName };
            channel.Name = analogChannel.Name;
            channel.Description = analogChannel.CircuitComponent;
            channel.HarmonicGroup = 0;

            return channel;
        }

        private Channel ParseSeries(DigitalChannel digitalChannel)
        {
            string phaseName = !string.IsNullOrEmpty(digitalChannel.PhaseID) ? digitalChannel.PhaseID : "Unknown";

            Series series = new Series();
            series.Channel = new Channel();
            series.SeriesType = new SeriesType() { Name = "Values" };
            series.SourceIndexes = digitalChannel.Index.ToString();

            Channel channel = series.Channel;
            channel.Series = new List<Series>() { series };
            channel.MeasurementType = new MeasurementType() { Name = "Digital" };
            channel.MeasurementCharacteristic = new MeasurementCharacteristic() { Name = "Unknown" };
            channel.Phase = new Phase() { Name = phaseName };
            channel.Name = digitalChannel.Name;
            channel.Description = digitalChannel.CircuitComponent;
            channel.HarmonicGroup = 0;

            return channel;
        }

        #endregion

        #region [ Static ]

        // Static Fields
        private static readonly ILog Log = LogManager.GetLogger(typeof(COMTRADEReader));

        // Static Properties
        private static string TempDataFolder => LazyTempDataFolder.Value;

        private static Lazy<string> LazyTempDataFolder => new Lazy<string>(() =>
        {
            string tempPath = Path.GetTempPath();
            return Path.Combine(tempPath, "openXDA", "COMTRADE");
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
