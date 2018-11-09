//******************************************************************************************************
//  COMTRADEWriter.cs - Gbtc
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
//  08/06/2014 - Stephen C. Wills
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
using FaultAlgorithms;
using FaultData.Configuration;
using FaultData.DataAnalysis;
using FaultData.DataResources;
using FaultData.DataSets;
using GSF;
using GSF.Configuration;
using GSF.COMTRADE;
using GSF.IO;
using openXDA.Model;
using FaultGroup = FaultData.DataAnalysis.FaultGroup;
using Fault = FaultData.DataAnalysis.Fault;
using GSF.Data;
using GSF.Data.Model;
using System.Data;
using System.IO.Compression;

namespace FaultData.DataWriters
{
    public class COMTRADEWriter
    {
        #region [ Members ]

        // Nested Types
        private class COMTRADEData
        {
            public string StationName;
            public string DeviceID;
            public Ticks DataStartTime;
            public int SampleCount;
            public double SamplingRate;

            public List<COMTRADEChannelData> AnalogChannelData;
            public List<COMTRADEChannelData> DigitalChannelData;
        }

        private class COMTRADEChannelData
        {
            public int GroupOrder;
            public int LoadOrder;
            public int OriginalChannelIndex;
            public string Name;
            public string Units;
            public DataSeries Data;

            public AnalogChannel OriginalAnalogChannel;
            public DigitalChannel OriginalDigitalChannel;
        }

        private class EventDataSet
        {
            public string ResultsPath;
            public MeterDataSet MeterDataSet;
            public TimeSpan TimeZoneOffset;
            public DataGroup DataGroup;
            public VICycleDataGroup VICycleDataGroup;
            public List<Fault> Faults;
            public List<OutputChannel> OutputChannels;
        }

        // Fields
        private string m_resultsPath;
        private double m_systemFrequency;
        private string m_lengthUnits;
        private string m_defaultMeterTimeZone;
        private TimeZoneInfo m_xdaTimeZone;
        private FaultLocationSettings m_faultLocationSettings;

        #endregion

        #region [ Constructors ]

        public COMTRADEWriter()
        {
            m_faultLocationSettings = new FaultLocationSettings();
        }

        #endregion

        #region [ Properties ]

        [Setting]
        public string ResultsPath
        {
            get
            {
                return m_resultsPath;
            }
            set
            {
                m_resultsPath = value;
            }
        }

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
        /// Gets or sets the unit of measure for length used by the fault location system.
        /// </summary>
        [Setting]
        public string LengthUnits
        {
            get
            {
                return m_lengthUnits;
            }
            set
            {
                m_lengthUnits = value;
            }
        }

        [Setting]
        public string DefaultMeterTimeZone
        {
            get
            {
                return m_defaultMeterTimeZone;
            }
            set
            {
                m_defaultMeterTimeZone = value;
            }
        }

        [Setting]
        public string XDATimeZone
        {
            get
            {
                return m_xdaTimeZone.Id;
            }
            set
            {
                m_xdaTimeZone = TimeZoneInfo.FindSystemTimeZoneById(value);
            }
        }

        [Category]
        [SettingName("FaultLocation")]
        public FaultLocationSettings FaultLocationSettings
        {
            get
            {
                return m_faultLocationSettings;
            }
        }

        #endregion

        #region [ Methods ]

        public void WriteResults(MeterDataSet meterDataSet)
        {
            CycleDataResource cycleDataResource;
            FaultDataResource faultDataResource;

            DataGroup dataGroup;
            FaultGroup faultGroup;
            List<int> seriesIDs;
            EventDataSet eventDataSet;

            string rootFileName;
            string fileName;

            cycleDataResource = meterDataSet.GetResource<CycleDataResource>();
            faultDataResource = meterDataSet.GetResource<FaultDataResource>();

            if (!Directory.Exists(m_resultsPath))
                Directory.CreateDirectory(m_resultsPath);

            for (int i = 0; i < cycleDataResource.DataGroups.Count; i++)
            {
                dataGroup = cycleDataResource.DataGroups[i];

                if (faultDataResource.FaultLookup.TryGetValue(dataGroup, out faultGroup))
                {
                    rootFileName = FilePath.GetFileNameWithoutExtension(meterDataSet.FilePath);
                    fileName = string.Format("{0},{1:000},Line{2}.dat", rootFileName, i, dataGroup.Line.AssetKey);

                    seriesIDs = dataGroup.DataSeries
                        .Select(series => series.SeriesInfo.ID)
                        .ToList();
                    using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
                    {
                        eventDataSet = new EventDataSet()
                        {
                            ResultsPath = Path.Combine(m_resultsPath, fileName),
                            MeterDataSet = meterDataSet,
                            TimeZoneOffset = GetTimeZoneOffset(meterDataSet.Meter.TimeZone, dataGroup.StartTime),
                            DataGroup = dataGroup,
                            VICycleDataGroup = cycleDataResource.VICycleDataGroups[i],
                            Faults = faultGroup.Faults,
                            OutputChannels = (new TableOperations<OutputChannel>(connection)).QueryRecordsWhere($"SeriesID IN ({string.Join(",",seriesIDs)})").ToList()
                        };
                    }
                    WriteResults(eventDataSet);
                }
            }
        }

        private void WriteResults(EventDataSet eventDataSet)
        {
            const StringComparison IgnoreCase = StringComparison.OrdinalIgnoreCase;

            // Get the path to the original data file
            string dataFilePath;
            string directory;
            string rootFileName;
            string schemaFilePath;

            Parser originalFileParser = null;

            dataFilePath = eventDataSet.MeterDataSet.FileGroup.DataFiles
                .Select(dataFile => dataFile.FilePath)
                .FirstOrDefault(path => path.EndsWith(".dat", IgnoreCase) || path.EndsWith(".d00", IgnoreCase));

            if (File.Exists(dataFilePath))
            {
                // Get the path to the original schema file
                directory = FilePath.GetDirectoryName(dataFilePath);
                rootFileName = FilePath.GetFileNameWithoutExtension(dataFilePath);
                schemaFilePath = Path.Combine(directory, rootFileName + ".cfg");

                if (File.Exists(schemaFilePath))
                {
                    // Create a parser to parse the original COMTRADE files
                    originalFileParser = new Parser()
                    {
                        FileName = dataFilePath,
                        InferTimeFromSampleRates = true,
                        Schema = new Schema(schemaFilePath)
                    };
                }
            }

            using (originalFileParser)
            {
                if ((object)originalFileParser != null)
                    originalFileParser.OpenFiles();

                // Write results to the COMTRADE fault record using the parser
                WriteResults(eventDataSet, originalFileParser);
            }
        }

        private void WriteResults(EventDataSet eventDataSet, Parser originalFileParser)
        {
            // Assume the data file path is the same as the results file path
            // and determine the path to the header and schema files
            string directory = FilePath.GetDirectoryName(eventDataSet.ResultsPath);
            string rootFileName = FilePath.GetFileNameWithoutExtension(eventDataSet.ResultsPath);
            string headerFilePath = Path.Combine(directory, rootFileName + ".hdr");
            string schemaFilePath = Path.Combine(directory, rootFileName + ".cfg");
            string dataFilePath = eventDataSet.ResultsPath;
            Schema schema;

            // Get structures containing the data to be written to the results files
            COMTRADEData comtradeData = new COMTRADEData();

            // If there are no waveforms to be written to the file, give up
            if (!eventDataSet.OutputChannels.Any())
                return;

            comtradeData.StationName = eventDataSet.MeterDataSet.Meter.Name;
            comtradeData.DeviceID = eventDataSet.MeterDataSet.Meter.AssetKey;
            comtradeData.DataStartTime = eventDataSet.DataGroup.StartTime;
            comtradeData.SampleCount = eventDataSet.DataGroup.Samples;
            comtradeData.SamplingRate = eventDataSet.DataGroup.Samples / (eventDataSet.DataGroup.EndTime - eventDataSet.DataGroup.StartTime).TotalSeconds;

            comtradeData.AnalogChannelData = GetAnalogChannelData(eventDataSet);
            comtradeData.DigitalChannelData = GetDigitalChannelData(eventDataSet);

            // If the original file is available, use data from the original file
            // in order to update the data to be written to the results files
            if ((object)originalFileParser != null)
                FixCOMTRADEData(comtradeData, originalFileParser, eventDataSet.TimeZoneOffset);

            // Write data to the header file
            WriteHeaderFile(eventDataSet, headerFilePath);

            // Write data to the schema file
            schema = WriteSchemaFile(comtradeData, schemaFilePath);

            // Write data to the data file
            WriteDataFile(comtradeData, schema, dataFilePath);
        }

        private void WriteHeaderFile(EventDataSet eventDataSet, string headerFilePath)
        {
            // Date-time format used in the header file
            const string DateTimeFormat = "yyyy-MM-dd HH:mm:ss.ffffff";

            string lineName = eventDataSet.MeterDataSet.Meter.MeterLines
                .Where(ml => ml.LineID == eventDataSet.DataGroup.Line.ID)
                .Select(ml => ml.LineName)
                .FirstOrDefault() ?? eventDataSet.DataGroup.Line.AssetKey;

            using (FileStream stream = File.Create(FilePath.GetAbsolutePath(headerFilePath)))
            using (StreamWriter writer = new StreamWriter(stream, Encoding.ASCII))
            {
                // Write out the original list of files containing
                // the data used to generate this fault record
                writer.WriteLine("Disturbance files:");

                foreach (DataFile dataFile in eventDataSet.MeterDataSet.FileGroup.DataFiles)
                    writer.WriteLine(dataFile.FilePath);

                writer.WriteLine();

                // Write the meter name to the header file
                writer.WriteLine("Meter: " + eventDataSet.MeterDataSet.Meter.Name);

                // Write out the name and length of the line on which the fault(s) occurred
                writer.WriteLine("Line: {0} ({1} {2})", lineName, eventDataSet.DataGroup.Line.Length, LengthUnits);
                writer.WriteLine();

                // For each fault, write the distance, the most prominent fault type, the time of
                // inception, the time of clearing, and the duration of the fault to the header file
                for (int i = 0; i < eventDataSet.Faults.Count; i++)
                {
                    int faultNumber = i + 1;
                    Fault fault = eventDataSet.Faults[i];

                    if (fault.IsSuppressed || fault.Summaries.All(summary => !summary.IsValid))
                        continue;

                    DateTime startTime = fault.InceptionTime;
                    DateTime endTime = fault.ClearingTime;
                    int startSample = fault.StartSample;
                    int endSample = fault.EndSample;
                    double duration = fault.Duration.TotalSeconds;

                    double distance = fault.Summaries
                        .Where(summary => summary.IsSelectedAlgorithm)
                        .Select(summary => summary.Distance)
                        .DefaultIfEmpty(double.NaN)
                        .First();

                    writer.WriteLine("Fault {0}:", faultNumber);
                    writer.WriteLine("[{0}]       Fault distance: {1:0.00} {2}", faultNumber, distance, LengthUnits);
                    writer.WriteLine("[{0}] Prominent fault type: {1}", faultNumber, fault.Type);
                    writer.WriteLine("[{0}]      Fault inception: {1} (sample #{2})", faultNumber, startTime.ToString(DateTimeFormat), startSample);
                    writer.WriteLine("[{0}]       Fault clearing: {1} (sample #{2})", faultNumber, endTime.ToString(DateTimeFormat), endSample);
                    writer.WriteLine("[{0}]       Fault duration: {1:0.0000} seconds ({2:0.00} cycles)", faultNumber, duration, duration * m_systemFrequency);
                    writer.WriteLine();
                }
            }
        }

        private Schema WriteSchemaFile(COMTRADEData comtradeData, string schemaFilePath)
        {
            Schema schema = Writer.CreateSchema(new List<ChannelMetadata>(), comtradeData.StationName, comtradeData.DeviceID, comtradeData.DataStartTime, comtradeData.SampleCount, 1999, FileType.Binary, 1, comtradeData.SamplingRate, m_systemFrequency,false);
            List<AnalogChannel> analogChannels = new List<AnalogChannel>();
            List<DigitalChannel> digitalChannels = new List<DigitalChannel>();
            int i = 1;

            // Populate the analog channel list with analog channel metadata
            foreach (COMTRADEChannelData channelData in comtradeData.AnalogChannelData)
            {
                AnalogChannel analogChannel = new AnalogChannel();

                analogChannel.Index = i;
                analogChannel.Name = channelData.Name;
                analogChannel.MinValue = -short.MaxValue;
                analogChannel.MaxValue = short.MaxValue;
                analogChannel.Units = channelData.Units;

                if ((object)channelData.OriginalAnalogChannel != null)
                {
                    analogChannel.PhaseID = channelData.OriginalAnalogChannel.PhaseID;
                    analogChannel.CircuitComponent = channelData.OriginalAnalogChannel.CircuitComponent;
                    analogChannel.Units = channelData.OriginalAnalogChannel.Units;
                    analogChannel.Skew = channelData.OriginalAnalogChannel.Skew;
                    analogChannel.PrimaryRatio = channelData.OriginalAnalogChannel.PrimaryRatio;
                    analogChannel.SecondaryRatio = channelData.OriginalAnalogChannel.SecondaryRatio;
                    analogChannel.ScalingIdentifier = channelData.OriginalAnalogChannel.ScalingIdentifier;

                    if (analogChannel.Units.ToUpper().Contains("KA") || analogChannel.Units.ToUpper().Contains("KV"))
                        channelData.Data = channelData.Data.Multiply(0.001);
                }

                analogChannel.Multiplier = (channelData.Data.Maximum - channelData.Data.Minimum) / (2 * short.MaxValue);
                analogChannel.Adder = (channelData.Data.Maximum + channelData.Data.Minimum) / 2.0D;

                analogChannels.Add(analogChannel);

                i++;
            }

            i = 1;

            // Populate the digital channel list with digital channel metadata
            foreach (COMTRADEChannelData channelData in comtradeData.DigitalChannelData)
            {
                DigitalChannel digitalChannel = new DigitalChannel();

                digitalChannel.Index = i;
                digitalChannel.Name = channelData.Name;

                if ((object)channelData.OriginalDigitalChannel != null)
                {
                    digitalChannel.PhaseID = channelData.OriginalDigitalChannel.PhaseID;
                    digitalChannel.CircuitComponent = channelData.OriginalDigitalChannel.CircuitComponent;
                    digitalChannel.NormalState = channelData.OriginalDigitalChannel.NormalState;
                }

                digitalChannels.Add(digitalChannel);

                i++;
            }

            schema.AnalogChannels = analogChannels.ToArray();
            schema.DigitalChannels = digitalChannels.ToArray();

            // Dump the generated schema to the schema file
            File.WriteAllText(FilePath.GetAbsolutePath(schemaFilePath), schema.FileImage, Encoding.ASCII);

            // Return the schema
            return schema;
        }

        private void WriteDataFile(COMTRADEData comtradeData, Schema schema, string dataFilePath)
        {
            // Function to get the value at a given index from a data series
            Func<DataSeries, int, double> valueAt = (series, i) =>
                (i < series.DataPoints.Count)
                ? series.DataPoints[i].Value
                : series.DataPoints.Last().Value;

            // Get the list of data series for every channel in the COMTRADE file
            IEnumerable<DataSeries> digitalSeriesList = comtradeData.DigitalChannelData
                .Select(channelData => channelData.Data)
                .Select((series, index) => series.Multiply(Math.Pow(2.0D, index % 16)))
                .Select((series, index) => Tuple.Create(index / 16, series))
                .GroupBy(tuple => tuple.Item1)
                .Select(group => group.Select(tuple => tuple.Item2))
                .Select(group => group.Aggregate((sum, series) => sum.Add(series)));

            List<DataSeries> allChannels = comtradeData.AnalogChannelData
                .Select(channelData => channelData.Data)
                .Concat(digitalSeriesList)
                .ToList();

            // Use the longest data series as the series
            // from which time values will be used
            DataSeries timeSeries = allChannels
                .OrderByDescending(series => series.DataPoints.Count)
                .First();

            // Open the data file for writing
            using (FileStream fileStream = File.Create(FilePath.GetAbsolutePath(dataFilePath)))
            {
                // Write the timestamp and values to each line of the data file
                for (int i = 0; i < comtradeData.SampleCount; i++)
                {
                    Ticks timestamp = timeSeries.DataPoints[i].Time;

                    double[] values = allChannels
                        .Select(series => valueAt(series, i))
                        .ToArray();

                    Writer.WriteNextRecordBinary(fileStream, schema, timestamp, values, (uint)i, false);
                }
            }
        }

        private List<Tuple<string, DataSeries>> ToFaultCurves(DataGroup dataGroup, List<Fault> faults)
        {
            double maxDistance;
            double minDistance;

            List<Tuple<string, DataSeries>> faultCurves;

            string algorithm;
            DataSeries faultCurve;

            maxDistance = dataGroup.Line.Length * m_faultLocationSettings.MaxFaultDistanceMultiplier;
            minDistance = dataGroup.Line.Length * m_faultLocationSettings.MinFaultDistanceMultiplier;
            faultCurves = new List<Tuple<string, DataSeries>>();

            for (int c = 0; c < faults[0].Curves.Count; c++)
            {
                algorithm = faults[0].Curves[c].Algorithm;
                faultCurve = dataGroup[0].Multiply(0.0D);

                foreach (Fault fault in faults)
                {
                    for (int i = fault.StartSample; fault.Curves[c].HasData(i); i++)
                        faultCurve[i].Value = Common.Mid(minDistance, fault.Curves[c][i].Value, maxDistance);
                }

                faultCurves.Add(Tuple.Create(algorithm, faultCurve));
            }

            return faultCurves;
        }

        private List<COMTRADEChannelData> GetAnalogChannelData(EventDataSet eventDataSet)
        {
            int sourceIndex;

            // Get COMTRADE channel information for each waveform to be output to the COMTRADE file
            IEnumerable<COMTRADEChannelData> waveformChannelData = eventDataSet.OutputChannels
                .Join(eventDataSet.DataGroup.DataSeries, channel => channel.SeriesID, series => series.SeriesInfo.ID, (channel, series) => new { OutputChannel = channel, DataSeries = series })
                .Select(waveformChannel => new COMTRADEChannelData()
                {
                    GroupOrder = 0,
                    LoadOrder = waveformChannel.OutputChannel.LoadOrder,
                    OriginalChannelIndex = int.TryParse(waveformChannel.DataSeries.SeriesInfo.SourceIndexes, out sourceIndex) ? sourceIndex : -1,
                    Name = waveformChannel.OutputChannel.ChannelKey,
                    Units = GetUnits(waveformChannel.OutputChannel.ChannelKey),
                    Data = waveformChannel.DataSeries
                });

            // Get COMTRADE channel information for each fault curve
            IEnumerable<COMTRADEChannelData> faultCurveChannelData = new List<COMTRADEChannelData>();
            if (eventDataSet.Faults != null && eventDataSet.Faults.Any())
            {
                faultCurveChannelData = ToFaultCurves(eventDataSet.DataGroup, eventDataSet.Faults)
                    .Select(tuple => new { Algorithm = tuple.Item1, DataSeries = tuple.Item2 })
                    .Select((faultCurve, i) => new COMTRADEChannelData()
                    {
                        GroupOrder = 1,
                        LoadOrder = i,
                        OriginalChannelIndex = -1,
                        Name = faultCurve.Algorithm,
                        Units = string.Empty,
                        Data = faultCurve.DataSeries
                    });
            }
            // Get COMTRADE channel information for each cycle data series to be output to the file
            IEnumerable<COMTRADEChannelData> cycleChannelData = GetCycleChannelData(eventDataSet.VICycleDataGroup, eventDataSet.OutputChannels);

            // Return all the analog channel data in one list
            return waveformChannelData
                .Concat(faultCurveChannelData)
                .Concat(cycleChannelData)
                .OrderBy(channelData => channelData.GroupOrder)
                .ThenBy(channelData => channelData.LoadOrder)
                .ToList();
        }

        private List<COMTRADEChannelData> GetDigitalChannelData(EventDataSet eventDataSet)
        {
            // Get an arbitrary data series from the collection of waveforms so
            // that we can use it to determine the timestamp of each data point
            DataSeries series = eventDataSet.DataGroup[0];

            // Create the function to convert a name and an index to a COMTRADEChannelData
            Func<string, int, COMTRADEChannelData> toChannelData = (name, index) =>
                new COMTRADEChannelData()
                {
                    GroupOrder = 0,
                    LoadOrder = index,
                    OriginalChannelIndex = index,
                    Name = name,
                    Data = series.Multiply(0.0D)
                };

            // Get the collection of COMTRADEChannelData for the fault-related digitals
            List<COMTRADEChannelData> digitalChannelData = new string[] { "Fault Detected" }
                .Concat(GetAllFaultTypes().Select(faultType => string.Format("{0} Fault Type", faultType)))
                .Select(toChannelData)
                .ToList();

            // Populate the data points for each digital channel
            // based on the fault type of each segment of fault data
            foreach (Fault.Segment segment in eventDataSet.Faults.SelectMany(fault => fault.Segments))
            {
                // Get the fault type of the segment
                FaultType faultType = segment.FaultType;

                // Get the value of each digital in this segment
                bool[] digitals =
                {
                    faultType != FaultType.None,
                    faultType == FaultType.AN,
                    faultType == FaultType.BN,
                    faultType == FaultType.CN,
                    faultType == FaultType.AB,
                    faultType == FaultType.BC,
                    faultType == FaultType.CA,
                    faultType == FaultType.ABG,
                    faultType == FaultType.BCG,
                    faultType == FaultType.CAG,
                    faultType == FaultType.ABC
                };

                // Populate the data points for this segment with the proper values based on fault type
                for (int i = 0; i < digitalChannelData.Count; i++)
                {
                    double value = Convert.ToDouble(digitals[i]);

                    for (int j = segment.StartSample; j <= segment.EndSample; j++)
                        digitalChannelData[i].Data.DataPoints[j].Value = value;
                }
            }

            return digitalChannelData;
        }

        private void FixCOMTRADEData(COMTRADEData comtradeData, Parser parser, TimeSpan timeZoneOffset)
        {
            // Function to convert a digital channel from the COMTRADE files to COMTRADEChannelData
            Func<DigitalChannel, int, COMTRADEChannelData> toChannelData = (digitalChannel, index) =>
                new COMTRADEChannelData()
                {
                    GroupOrder = 1,
                    LoadOrder = index,
                    OriginalChannelIndex = index,
                    Name = digitalChannel.Name,
                    Data = new DataSeries(),
                    OriginalDigitalChannel = digitalChannel
                };

            // List of COMTRADEChannelData containing data about the digital channels in the COMTRADE file
            List<COMTRADEChannelData> originalDigitalChannelData = parser.Schema.DigitalChannels
                .Select(toChannelData)
                .ToList();

            // The number of analog channels in the COMTRADE file
            int analogChannels = parser.Schema.AnalogChannels.Length;

            // Set COMTRADE schema information to match that of the original file
            comtradeData.StationName = parser.Schema.StationName;
            comtradeData.DeviceID = parser.Schema.DeviceID;

            // First map the analog channel data to the analog channels in the file, then update the analog channel data to match the information in the file
            foreach (var mapping in comtradeData.AnalogChannelData.Join(parser.Schema.AnalogChannels, channelData => channelData.OriginalChannelIndex, channel => channel.Index, (channelData, channel) => new { ChannelData = channelData, Channel = channel }))
            {
                mapping.ChannelData.Name = mapping.Channel.Name;
                mapping.ChannelData.OriginalAnalogChannel = mapping.Channel;
            }

            // Populate the new digital channels with data from the COMTRADE file
            while (parser.ReadNext())
            {
                foreach (COMTRADEChannelData channelData in originalDigitalChannelData)
                {
                    channelData.Data.DataPoints.Add(new DataPoint()
                    {
                        Time = parser.Timestamp + timeZoneOffset,
                        Value = parser.Values[analogChannels + channelData.OriginalChannelIndex]
                    });
                }
            }

            // Add the new digital channels to the digitalChannelData collection
            comtradeData.DigitalChannelData.AddRange(originalDigitalChannelData);
        }

        private IEnumerable<COMTRADEChannelData> GetCycleChannelData(VICycleDataGroup cycleDataGroup, IEnumerable<OutputChannel> outputChannels)
        {
            // Create a function to get the load order of a collection of
            // channels where collections should have no more than 1 channel
            Func<IEnumerable<OutputChannel>, int> getLoadOrder = channels => channels
                .Select(channel => channel.LoadOrder)
                .DefaultIfEmpty(int.MaxValue)
                .First();

            // Convert the cycle data set to a collection of cycle data groups
            List<Tuple<string, CycleDataGroup>> cycleDataGroups = new List<Tuple<string, CycleDataGroup>>
            {
                Tuple.Create("VA", cycleDataGroup.VA),
                Tuple.Create("VB", cycleDataGroup.VB),
                Tuple.Create("VC", cycleDataGroup.VC),
                Tuple.Create("IA", cycleDataGroup.IA),
                Tuple.Create("IB", cycleDataGroup.IB),
                Tuple.Create("IC", cycleDataGroup.IC),
                Tuple.Create("IR", cycleDataGroup.IR)
            };

            // Join the output channels to the cycle data groups, order them by LoadOrder, and then
            // return a collection containing only the RMS and Phase series from each cycle data group
            return cycleDataGroups
                .GroupJoin(outputChannels, tuple => tuple.Item1, outputChannel => outputChannel.ChannelKey, (tuple, channels) => new { LoadOrder = getLoadOrder(channels), ChannelKey = tuple.Item1, CycleDataGroup = tuple.Item2 })
                .OrderBy(cycleDataGroupInfo => cycleDataGroupInfo.LoadOrder)
                .ThenBy(cycleDataGroupInfo => cycleDataGroupInfo.ChannelKey)
                .SelectMany((cycleDataGroupInfo, index) => ToAnalogChannelCollection(index, cycleDataGroupInfo.ChannelKey, cycleDataGroupInfo.CycleDataGroup));
        }

        private IEnumerable<COMTRADEChannelData> ToAnalogChannelCollection(int cycleDataGroupIndex, string channelKey, CycleDataGroup cycleDataGroup)
        {
            // Get the RMS channel data from the given cycle data group
            COMTRADEChannelData rmsChannelData = new COMTRADEChannelData()
            {
                GroupOrder = 2,
                LoadOrder = cycleDataGroupIndex,
                OriginalChannelIndex = -1,
                Name = channelKey + " RMS",
                Units = GetUnits(channelKey),
                Data = cycleDataGroup.RMS
            };

            // Get the phase angle channel data from the given cycle group
            COMTRADEChannelData phaseChannelData = new COMTRADEChannelData()
            {
                GroupOrder = 3,
                LoadOrder = cycleDataGroupIndex,
                OriginalChannelIndex = -1,
                Name = channelKey + " phase angle",
                Units = string.Empty,
                Data = cycleDataGroup.Phase
            };

            // Return the channel data obtained from the given cycle group
            return new COMTRADEChannelData[] { rmsChannelData, phaseChannelData };
        }

        // Get all possible fault types (except for None).
        private static List<FaultType> GetAllFaultTypes()
        {
            return new List<FaultType>()
            {
                FaultType.AN,
                FaultType.BN,
                FaultType.CN,
                FaultType.AB,
                FaultType.BC,
                FaultType.CA,
                FaultType.ABG,
                FaultType.BCG,
                FaultType.CAG,
                FaultType.ABC
            };
        }

        // Get the units for the given channel.
        private string GetUnits(string channelKey)
        {
            Dictionary<string, string> unitsLookup = new Dictionary<string, string>()
            {
                { "VA", "volts" },
                { "VB", "volts" },
                { "VC", "volts" },
                { "IA", "amps" },
                { "IB", "amps" },
                { "IC", "amps" }
            };

            string units;

            if (!unitsLookup.TryGetValue(channelKey, out units))
                units = string.Empty;

            return units;
        }

        public void WriteResults(int meterID, int lineID, DateTime startTime, DateTime endTime, Stream returnStream)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                IEnumerable<Event> events = (new TableOperations<Event>(connection)).QueryRecordsWhere("StartTime <= {0} AND EndTime >= {1} and MeterID = {2} AND LineID = {3}", ToDateTime2(connection, endTime), ToDateTime2(connection, startTime), meterID, lineID);
                Dictionary<string, byte[]> dict = new Dictionary<string, byte[]>();
                foreach (Event evt in events)
                {
                    MeterDataSet meterDataSet = new MeterDataSet(evt);
                    var returnDict = WriteResultsToStream(evt, meterDataSet);
                    foreach(var kvp in returnDict)
                    {
                        if (!dict.ContainsKey(kvp.Key))
                            dict.Add(kvp.Key, kvp.Value);
                    };

                }

                using(MemoryStream memoryStream = new MemoryStream())
                {
                    using (ZipArchive zip = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                    {
                        foreach (KeyValuePair<string, byte[]> kvp in dict)
                        {
                            ZipArchiveEntry zipArchiveEntry = zip.CreateEntry(kvp.Key);
                            using (Stream stream = zipArchiveEntry.Open())
                            {
                                (new MemoryStream(kvp.Value)).CopyTo(stream);
                            }
                        }
                    }
                    memoryStream.WriteTo(returnStream);
                }

            }
        }

        public Dictionary<string, byte[]> WriteResultsToStream(Event evt, MeterDataSet meterDataSet)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                double freq = connection.ExecuteScalar<double?>("SELECT Value FROM Setting WHERE Name = 'SystemFrequency'") ?? 60.0D;
                DefaultMeterTimeZone = connection.ExecuteScalar<string>("SELECT Value FROM Setting WHERE Name = 'DefaultMeterTimeZone'") ?? "UTC";
                XDATimeZone = connection.ExecuteScalar<string>("SELECT Value FROM Setting WHERE Name = 'XDATimeZone'") ?? "UTC";
                DataGroup dataGroup = ToDataGroup(meterDataSet.Meter, (new TableOperations<EventData>(connection)).QueryRecordWhere("ID = {0}", evt.EventDataID).TimeDomainData);
                VICycleDataGroup viCycleDataGroup = Transform.ToVICycleDataGroup(new VIDataGroup(dataGroup), freq);

                List<int> seriesIDs = dataGroup.DataSeries
                .Select(series => series.SeriesInfo.ID)
                .ToList();

                EventDataSet eventDataSet = new EventDataSet()
                {
                    ResultsPath = string.Empty,
                    MeterDataSet = meterDataSet,
                    TimeZoneOffset = GetTimeZoneOffset(meterDataSet.Meter.TimeZone, dataGroup.StartTime),
                    DataGroup = dataGroup,
                    VICycleDataGroup = viCycleDataGroup,
                    OutputChannels = (new TableOperations<OutputChannel>(connection)).QueryRecordsWhere($"SeriesID IN ({string.Join(",", seriesIDs)})").ToList(), 
                    Faults = new List<Fault>()
                };

                return WriteResultsToStream(eventDataSet);
            }
        }

        private Dictionary<string, byte[]> WriteResultsToStream(EventDataSet eventDataSet)
        {
            Dictionary<string, byte[]> dict = new Dictionary<string, byte[]>();

            // Get structures containing the data to be written to the results files
            COMTRADEData comtradeData = new COMTRADEData();

            // If there are no waveforms to be written to the file, give up
            if (!eventDataSet.OutputChannels.Any())
                return new Dictionary<string, byte[]>();

            comtradeData.StationName = eventDataSet.MeterDataSet.Meter.Name;
            comtradeData.DeviceID = eventDataSet.MeterDataSet.Meter.AssetKey;
            comtradeData.DataStartTime = eventDataSet.DataGroup.StartTime;
            comtradeData.SampleCount = eventDataSet.DataGroup.Samples;
            comtradeData.SamplingRate = eventDataSet.DataGroup.Samples / (eventDataSet.DataGroup.EndTime - eventDataSet.DataGroup.StartTime).TotalSeconds;

            comtradeData.AnalogChannelData = GetAnalogChannelData(eventDataSet);
            comtradeData.DigitalChannelData = GetDigitalChannelData(eventDataSet);

            // Write data to the header file
            WriteHeaderFile(eventDataSet, dict);

            // Write data to the schema file
            Schema schema = WriteSchemaFile(comtradeData, eventDataSet, dict);

            // Write data to the data file
            WriteDataFile(comtradeData, schema, eventDataSet, dict);

            return dict;
        }

        private void WriteHeaderFile(EventDataSet eventDataSet, Dictionary<string, byte[]> dict)
        {
            // Date-time format used in the header file
            const string DateTimeFormat = "yyyy-MM-dd HH:mm:ss.ffffff";

            string lineName = eventDataSet.MeterDataSet.Meter.MeterLines
                .Where(ml => ml.LineID == eventDataSet.DataGroup.Line.ID)
                .Select(ml => ml.LineName)
                .FirstOrDefault() ?? eventDataSet.DataGroup.Line.AssetKey;
            using (MemoryStream memoryStream = new MemoryStream())
            using (StreamWriter writer = new StreamWriter(memoryStream, Encoding.ASCII))
            {
                // Write out the original list of files containing
                // the data used to generate this fault record
                writer.WriteLine("Disturbance files:");

                foreach (DataFile dataFile in eventDataSet.MeterDataSet.FileGroup.DataFiles)
                    writer.WriteLine(dataFile.FilePath);

                writer.WriteLine();

                // Write the meter name to the header file
                writer.WriteLine("Meter: " + eventDataSet.MeterDataSet.Meter.Name);

                // Write out the name and length of the line on which the fault(s) occurred
                writer.WriteLine("Line: {0} ({1} {2})", lineName, eventDataSet.DataGroup.Line.Length, LengthUnits);
                writer.WriteLine();

                // For each fault, write the distance, the most prominent fault type, the time of
                // inception, the time of clearing, and the duration of the fault to the header file
                for (int i = 0; i < eventDataSet.Faults.Count; i++)
                {
                    int faultNumber = i + 1;
                    Fault fault = eventDataSet.Faults[i];

                    if (fault.IsSuppressed || fault.Summaries.All(summary => !summary.IsValid))
                        continue;

                    DateTime startTime = fault.InceptionTime;
                    DateTime endTime = fault.ClearingTime;
                    int startSample = fault.StartSample;
                    int endSample = fault.EndSample;
                    double duration = fault.Duration.TotalSeconds;

                    double distance = fault.Summaries
                        .Where(summary => summary.IsSelectedAlgorithm)
                        .Select(summary => summary.Distance)
                        .DefaultIfEmpty(double.NaN)
                        .First();

                    writer.WriteLine("Fault {0}:", faultNumber);
                    writer.WriteLine("[{0}]       Fault distance: {1:0.00} {2}", faultNumber, distance, LengthUnits);
                    writer.WriteLine("[{0}] Prominent fault type: {1}", faultNumber, fault.Type);
                    writer.WriteLine("[{0}]      Fault inception: {1} (sample #{2})", faultNumber, startTime.ToString(DateTimeFormat), startSample);
                    writer.WriteLine("[{0}]       Fault clearing: {1} (sample #{2})", faultNumber, endTime.ToString(DateTimeFormat), endSample);
                    writer.WriteLine("[{0}]       Fault duration: {1:0.0000} seconds ({2:0.00} cycles)", faultNumber, duration, duration * m_systemFrequency);
                    writer.WriteLine();



                }
                writer.Flush();
                dict.Add(Path.GetFileNameWithoutExtension(eventDataSet.MeterDataSet.FilePath) + ".hdr", memoryStream.ToArray());
            }
        }

        private Schema WriteSchemaFile(COMTRADEData comtradeData, EventDataSet eventDataSet, Dictionary<string, byte[]> dict)
        {
            Schema schema = Writer.CreateSchema(new List<ChannelMetadata>(), comtradeData.StationName, comtradeData.DeviceID, comtradeData.DataStartTime, comtradeData.SampleCount, 1999, FileType.Binary, 1, comtradeData.SamplingRate, m_systemFrequency, false);
            List<AnalogChannel> analogChannels = new List<AnalogChannel>();
            List<DigitalChannel> digitalChannels = new List<DigitalChannel>();
            int i = 1;

            // Populate the analog channel list with analog channel metadata
            foreach (COMTRADEChannelData channelData in comtradeData.AnalogChannelData)
            {
                AnalogChannel analogChannel = new AnalogChannel();

                analogChannel.Index = i;
                analogChannel.Name = channelData.Name;
                analogChannel.MinValue = -short.MaxValue;
                analogChannel.MaxValue = short.MaxValue;
                analogChannel.Units = channelData.Units;

                if ((object)channelData.OriginalAnalogChannel != null)
                {
                    analogChannel.PhaseID = channelData.OriginalAnalogChannel.PhaseID;
                    analogChannel.CircuitComponent = channelData.OriginalAnalogChannel.CircuitComponent;
                    analogChannel.Units = channelData.OriginalAnalogChannel.Units;
                    analogChannel.Skew = channelData.OriginalAnalogChannel.Skew;
                    analogChannel.PrimaryRatio = channelData.OriginalAnalogChannel.PrimaryRatio;
                    analogChannel.SecondaryRatio = channelData.OriginalAnalogChannel.SecondaryRatio;
                    analogChannel.ScalingIdentifier = channelData.OriginalAnalogChannel.ScalingIdentifier;

                    if (analogChannel.Units.ToUpper().Contains("KA") || analogChannel.Units.ToUpper().Contains("KV"))
                        channelData.Data = channelData.Data.Multiply(0.001);
                }

                analogChannel.Multiplier = (channelData.Data.Maximum - channelData.Data.Minimum) / (2 * short.MaxValue);
                analogChannel.Adder = (channelData.Data.Maximum + channelData.Data.Minimum) / 2.0D;

                analogChannels.Add(analogChannel);

                i++;
            }

            i = 1;

            // Populate the digital channel list with digital channel metadata
            foreach (COMTRADEChannelData channelData in comtradeData.DigitalChannelData)
            {
                DigitalChannel digitalChannel = new DigitalChannel();

                digitalChannel.Index = i;
                digitalChannel.Name = channelData.Name;

                if ((object)channelData.OriginalDigitalChannel != null)
                {
                    digitalChannel.PhaseID = channelData.OriginalDigitalChannel.PhaseID;
                    digitalChannel.CircuitComponent = channelData.OriginalDigitalChannel.CircuitComponent;
                    digitalChannel.NormalState = channelData.OriginalDigitalChannel.NormalState;
                }

                digitalChannels.Add(digitalChannel);

                i++;
            }

            schema.AnalogChannels = analogChannels.ToArray();
            schema.DigitalChannels = digitalChannels.ToArray();

            using (MemoryStream memoryStream = new MemoryStream())
            using (StreamWriter writer = new StreamWriter(memoryStream, Encoding.ASCII))
            {
                writer.Write(schema.FileImage);
                writer.Flush();
                dict.Add(Path.GetFileNameWithoutExtension(eventDataSet.MeterDataSet.FilePath) + ".cfg", memoryStream.ToArray());
            }

            // Return the schema
            return schema;
        }

        private void WriteDataFile(COMTRADEData comtradeData, Schema schema, EventDataSet eventDataSet, Dictionary<string, byte[]> dict)
        {
            // Function to get the value at a given index from a data series
            Func<DataSeries, int, double> valueAt = (series, i) =>
                (i < series.DataPoints.Count)
                ? series.DataPoints[i].Value
                : series.DataPoints.Last().Value;

            // Get the list of data series for every channel in the COMTRADE file
            IEnumerable<DataSeries> digitalSeriesList = comtradeData.DigitalChannelData
                .Select(channelData => channelData.Data)
                .Select((series, index) => series.Multiply(Math.Pow(2.0D, index % 16)))
                .Select((series, index) => Tuple.Create(index / 16, series))
                .GroupBy(tuple => tuple.Item1)
                .Select(group => group.Select(tuple => tuple.Item2))
                .Select(group => group.Aggregate((sum, series) => sum.Add(series)));

            List<DataSeries> allChannels = comtradeData.AnalogChannelData
                .Select(channelData => channelData.Data)
                .Concat(digitalSeriesList)
                .ToList();

            // Use the longest data series as the series
            // from which time values will be used
            DataSeries timeSeries = allChannels
                .OrderByDescending(series => series.DataPoints.Count)
                .First();

            // Open the data file for writing
            using (MemoryStream memoryStream = new MemoryStream())
            {
                // Write the timestamp and values to each line of the data file
                for (int i = 0; i < comtradeData.SampleCount; i++)
                {
                    Ticks timestamp = timeSeries.DataPoints[i].Time;

                    double[] values = allChannels
                        .Select(series => valueAt(series, i))
                        .ToArray();

                    Writer.WriteNextRecordBinary(memoryStream, schema, timestamp, values, (uint)i, false);
                }
                dict.Add(Path.GetFileNameWithoutExtension(eventDataSet.MeterDataSet.FilePath) + ".dat", memoryStream.ToArray());
            }
        }

        private TimeSpan GetTimeZoneOffset(string meterTimeZoneID, DateTime startTime)
        {
            TimeZoneInfo meterTimeZone;

            if (string.IsNullOrEmpty(meterTimeZoneID))
                meterTimeZoneID = m_defaultMeterTimeZone;

            meterTimeZone = TimeZoneInfo.FindSystemTimeZoneById(meterTimeZoneID);

            return m_xdaTimeZone.GetUtcOffset(startTime) - meterTimeZone.GetUtcOffset(startTime);
        }

        private IDbDataParameter ToDateTime2(AdoDataConnection connection, DateTime dateTime)
        {
            using (IDbCommand command = connection.Connection.CreateCommand())
            {
                IDbDataParameter parameter = command.CreateParameter();
                parameter.DbType = DbType.DateTime2;
                parameter.Value = dateTime;
                return parameter;
            }
        }

        private DataGroup ToDataGroup(Meter meter, byte[] data)
        {
            DataGroup dataGroup = new DataGroup();
            dataGroup.FromData(meter, data);
            return dataGroup;
        }

        #endregion
    }
}
