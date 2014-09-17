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
using System.IO;
using System.Linq;
using System.Text;
using FaultAlgorithms;
using GSF;
using GSF.COMTRADE;
using GSF.IO;
using FaultData.DataAnalysis;
using FaultData.Database;
using FaultData.Database.FaultLocationDataTableAdapters;
using FaultData.Database.MeterDataTableAdapters;
using Line = FaultData.Database.Line;

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

        private class FaultRecordInfo
        {
            public FileGroup FileGroup;
            public Meter Meter;
            public Line Line;

            public MeterData.EventRow Event;
            public FaultLocationData.CycleDataRow CycleData;
            public List<FaultSegment> FaultSegments;
            public List<FaultLocationData.FaultCurveRow> FaultCurves;

            public List<Tuple<OutputChannel, DataSeries>> WaveformChannels;
            public VICycleDataSet CycleDataSet;
        }

        // Constants
        // TODO: Hardcoded frequency
        private const double Frequency = 60.0D;

        // TODO: Hardcoded maximum fault distance multiplier
        private const double MaxFaultDistanceMultiplier = 1.25D;

        // TODO: Hardcoded minimum fault distance multiplier
        private const double MinFaultDistanceMultiplier = -0.1D;

        // Fields
        private string m_connectionString;

        #endregion

        #region [ Constructors ]

        public COMTRADEWriter(string connectionString)
        {
            m_connectionString = connectionString;
        }

        #endregion

        #region [ Methods ]

        public void WriteResults(int eventID, string resultsFilePath)
        {
            FaultRecordInfo faultRecordInfo = new FaultRecordInfo();
            DataGroup eventDataGroup = new DataGroup();
            DataGroup cycleDataGroup = new DataGroup();

            // Populate faultRecordInfo with all the information from the database needed to create the COMTRADE fault record
            using (FileInfoDataContext fileInfo = new FileInfoDataContext(m_connectionString))
            using (MeterInfoDataContext meterInfo = new MeterInfoDataContext(m_connectionString))
            using (FaultLocationInfoDataContext faultLocationInfo = new FaultLocationInfoDataContext(m_connectionString))
            using (EventTableAdapter eventAdapter = new EventTableAdapter())
            using (CycleDataTableAdapter cycleDataAdapter = new CycleDataTableAdapter())
            using (FaultCurveTableAdapter faultCurveAdapter = new FaultCurveTableAdapter())
            {
                // Set the connection strings of the LINQ-to-DataSet adapters
                eventAdapter.Connection.ConnectionString = m_connectionString;
                cycleDataAdapter.Connection.ConnectionString = m_connectionString;
                faultCurveAdapter.Connection.ConnectionString = m_connectionString;

                // Get the event for the given event ID
                faultRecordInfo.Event = eventAdapter.GetDataBy(eventID).Single();

                // Get the group of files that contained the event data as well as the
                // meter that monitored the event and the line on which the event occurred
                faultRecordInfo.FileGroup = fileInfo.FileGroups.Single(fg => faultRecordInfo.Event.FileGroupID == fg.ID);
                faultRecordInfo.Meter = meterInfo.Meters.Single(m => faultRecordInfo.Event.MeterID == m.ID);
                faultRecordInfo.Line = meterInfo.Lines.Single(l => faultRecordInfo.Event.LineID == l.ID);

                // Get the cycle data, fault segments, and fault curves calculated by the fault location engine
                faultRecordInfo.CycleData = cycleDataAdapter.GetDataBy(eventID).Single();
                faultRecordInfo.FaultSegments = faultLocationInfo.FaultSegments.Where(segment => segment.EventID == eventID).ToList();
                faultRecordInfo.FaultCurves = faultCurveAdapter.GetDataBy(eventID).Where(curve => curve.EventID == eventID).ToList();

                // Extract the waveform and cycle data from the blobs stored in the database
                eventDataGroup.FromData(faultRecordInfo.Meter, faultRecordInfo.Event.Data);
                cycleDataGroup.FromData(faultRecordInfo.Meter, faultRecordInfo.CycleData.Data);

                // Get the "output channels" that define the order in which the
                // waveforms should be written to the COMTRADE fault record
                faultRecordInfo.WaveformChannels = faultLocationInfo.OutputChannels
                    .Join(eventDataGroup.DataSeries.Where(series => (object)series.SeriesInfo != null), channel => channel.SeriesID, series => series.SeriesInfo.ID, Tuple.Create)
                    .OrderBy(tuple => tuple.Item1.LoadOrder)
                    .ToList();

                // Create a cycle data set to identify what each series is in the cycleDataGroup
                faultRecordInfo.CycleDataSet = new VICycleDataSet(cycleDataGroup);

                // Use the fault record info to write the results to the COMTRADE files
                WriteResults(faultRecordInfo, resultsFilePath);
            }
        }

        private void WriteResults(FaultRecordInfo faultRecordInfo, string resultsFilePath)
        {
            const StringComparison IgnoreCase = StringComparison.OrdinalIgnoreCase;

            // Get the path to the original data file
            string dataFilePath = faultRecordInfo.FileGroup.DataFiles
                .Select(dataFile => dataFile.FilePath)
                .FirstOrDefault(path => path.EndsWith(".dat", IgnoreCase) || path.EndsWith(".d00", IgnoreCase));

            string directory;
            string rootFileName;
            string schemaFilePath;

            Parser originalFileParser = null;

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
                WriteResults(faultRecordInfo, originalFileParser, resultsFilePath);
            }
        }

        private void WriteResults(FaultRecordInfo faultRecordInfo, Parser originalFileParser, string resultsFilePath)
        {
            // Assume the data file path is the same as the results file path
            // and determine the path to the header and schema files
            string directory = FilePath.GetDirectoryName(resultsFilePath);
            string rootFileName = FilePath.GetFileNameWithoutExtension(resultsFilePath);
            string headerFilePath = Path.Combine(directory, rootFileName + ".hdr");
            string schemaFilePath = Path.Combine(directory, rootFileName + ".cfg");
            string dataFilePath = resultsFilePath;
            Schema schema;

            // Get structures containing the data to be written to the results files
            COMTRADEData comtradeData = new COMTRADEData();
            DataGroup eventDataGroup = new DataGroup();

            eventDataGroup.FromData(faultRecordInfo.Event.Data);

            comtradeData.StationName = faultRecordInfo.Meter.Name;
            comtradeData.DeviceID = faultRecordInfo.Meter.AssetKey;
            comtradeData.DataStartTime = eventDataGroup.StartTime;
            comtradeData.SampleCount = eventDataGroup.Samples;
            comtradeData.SamplingRate = eventDataGroup.Samples / (eventDataGroup.EndTime - eventDataGroup.StartTime).TotalSeconds;

            comtradeData.AnalogChannelData = GetAnalogChannelData(faultRecordInfo);
            comtradeData.DigitalChannelData = GetDigitalChannelData(faultRecordInfo);

            // If the original file is available, use data from the original file
            // in order to update the data to be written to the results files
            if ((object)originalFileParser != null)
                FixCOMTRADEData(comtradeData, originalFileParser);

            // Write data to the header file
            WriteHeaderFile(faultRecordInfo, headerFilePath);

            // Write data to the schema file
            schema = WriteSchemaFile(comtradeData, schemaFilePath);

            // Write data to the data file
            WriteDataFile(comtradeData, schema, dataFilePath);
        }

        private void WriteHeaderFile(FaultRecordInfo faultRecordInfo, string headerFilePath)
        {
            // Date-time format used in the header file
            const string DateTimeFormat = "yyyy-MM-dd HH:mm:ss.ffffff";

            // TODO: Hardcoded units
            const string LengthUnits = "miles";

            // Get the list of faults, which are the defined as a list of consecutive segments
            // where a fault was detected (an evolving fault is a fault with multiple segments)
            List<List<FaultSegment>> faults = faultRecordInfo.FaultSegments
                .Aggregate(new List<List<FaultSegment>>(), (list, segment) =>
                {
                    if (segment.SegmentType.Name == "Prefault" || segment.SegmentType.Name == "Postfault")
                    {
                        list.Add(new List<FaultSegment>());
                    }
                    else
                    {
                        if (list.Count == 0)
                            list.Add(new List<FaultSegment>());

                        list.Last().Add(segment);
                    }

                    return list;
                });

            // Get the fault curves calculated by the fault location engine
            List<DataSeries> faultCurves = faultRecordInfo.FaultCurves
                .Select(faultCurve => ToDataSeries(faultCurve))
                .ToList();

            using (FileStream stream = File.Create(FilePath.GetAbsolutePath(headerFilePath)))
            using (StreamWriter writer = new StreamWriter(stream, Encoding.ASCII))
            {
                // Write out the original list of files containing
                // the data used to generate this fault record
                writer.WriteLine("Disturbance files:");

                foreach (DataFile dataFile in faultRecordInfo.FileGroup.DataFiles)
                    writer.WriteLine(dataFile.FilePath);

                writer.WriteLine();

                // Write the meter name to the header file
                writer.WriteLine("Meter: " + faultRecordInfo.Meter.Name);

                // Write out the name and length of the line on which the fault(s) occurred
                writer.WriteLine("Line: {0} ({1} {2})", faultRecordInfo.Line.Name, faultRecordInfo.Line.Length, LengthUnits);
                writer.WriteLine();

                // For each fault, write the distance, the most prominent fault type, the time of
                // inception, the time of clearing, and the duration of the fault to the header file
                for (int i = 0; i < faults.Count; i++)
                {
                    int faultNumber = i + 1;
                    List<FaultSegment> fault = faults[i];

                    if (fault.Count == 0)
                        continue;

                    DateTime startTime = fault.First().StartTime;
                    DateTime endTime = fault.Last().EndTime;
                    int startSample = fault.First().StartSample;
                    int endSample = fault.Last().EndSample;
                    double duration = (endTime - startTime).TotalSeconds;

                    List<double> validDistances = faultCurves
                        .Select(series => series.ToSubSeries(startSample, endSample))
                        .SelectMany(series => series.DataPoints)
                        .Select(dataPoint => dataPoint.Value)
                        .Where(value => value >= MinFaultDistanceMultiplier * faultRecordInfo.Line.Length)
                        .Where(value => value <= MaxFaultDistanceMultiplier * faultRecordInfo.Line.Length)
                        .OrderBy(value => value)
                        .ToList();

                    if (!validDistances.Any())
                        continue;

                    writer.WriteLine("Fault {0}:", faultNumber);
                    writer.WriteLine("[{0}]       Fault distance: {1:0.00} {2}", faultNumber, validDistances[validDistances.Count / 2], LengthUnits);
                    writer.WriteLine("[{0}] Prominent fault type: {1}", faultNumber, GetFaultType(fault.OrderByDescending(segment => segment.EndSample - segment.StartSample).First().SegmentType));
                    writer.WriteLine("[{0}]      Fault inception: {1} (sample #{2})", faultNumber, startTime.ToString(DateTimeFormat), startSample);
                    writer.WriteLine("[{0}]       Fault clearing: {1} (sample #{2})", faultNumber, endTime.ToString(DateTimeFormat), endSample);
                    writer.WriteLine("[{0}]       Fault duration: {1:0.0000} seconds ({2:0.00} cycles)", faultNumber, duration, duration * Frequency);
                    writer.WriteLine();
                }
            }
        }

        private Schema WriteSchemaFile(COMTRADEData comtradeData, string schemaFilePath)
        {
            Schema schema = Writer.CreateSchema(new List<ChannelMetadata>(), comtradeData.StationName, comtradeData.DeviceID, comtradeData.DataStartTime, comtradeData.SampleCount, samplingRate: comtradeData.SamplingRate, includeFracSecDefinition: false);
            List<AnalogChannel> analogChannels = new List<AnalogChannel>();
            List<DigitalChannel> digitalChannels = new List<DigitalChannel>();
            int i = 1;

            // Populate the analog channel list with analog channel metadata
            foreach (COMTRADEChannelData channelData in comtradeData.AnalogChannelData)
            {
                AnalogChannel analogChannel = new AnalogChannel();

                analogChannel.Index = i;
                analogChannel.Name = channelData.Name;
                analogChannel.Multiplier = (channelData.Data.Maximum - channelData.Data.Minimum) / (2 * short.MaxValue);
                analogChannel.Adder = channelData.Data.Minimum + analogChannel.Multiplier * short.MaxValue;
                analogChannel.Units = channelData.Units;

                if ((object)channelData.OriginalAnalogChannel != null)
                {
                    analogChannel.PhaseID = channelData.OriginalAnalogChannel.PhaseID;
                    analogChannel.CircuitComponent = channelData.OriginalAnalogChannel.CircuitComponent;
                    analogChannel.Units = channelData.OriginalAnalogChannel.Units;
                    analogChannel.Multiplier = channelData.OriginalAnalogChannel.Multiplier;
                    analogChannel.Adder = channelData.OriginalAnalogChannel.Adder;
                    analogChannel.Skew = channelData.OriginalAnalogChannel.Skew;
                    analogChannel.MinValue = channelData.OriginalAnalogChannel.MinValue;
                    analogChannel.MaxValue = channelData.OriginalAnalogChannel.MaxValue;
                    analogChannel.PrimaryRatio = channelData.OriginalAnalogChannel.PrimaryRatio;
                    analogChannel.SecondaryRatio = channelData.OriginalAnalogChannel.SecondaryRatio;
                    analogChannel.ScalingIdentifier = channelData.OriginalAnalogChannel.ScalingIdentifier;

                    if (analogChannel.Units.ToUpper().Contains("KA") || analogChannel.Units.ToUpper().Contains("KV"))
                        channelData.Data = channelData.Data.Multiply(0.001);
                }

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

        private List<COMTRADEChannelData> GetAnalogChannelData(FaultRecordInfo faultRecordInfo)
        {
            int sourceIndex;

            // Get COMTRADE channel information for each waveform to be output to the COMTRADE file
            IEnumerable<COMTRADEChannelData> waveformChannelData = faultRecordInfo.WaveformChannels
                .Select(tuple => new { OutputChannel = tuple.Item1, DataSeries = tuple.Item2 })
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
            IEnumerable<COMTRADEChannelData> faultCurveChannelData = faultRecordInfo.FaultCurves
                .Select(faultCurve => new COMTRADEChannelData()
                {
                    GroupOrder = 1,
                    LoadOrder = faultCurve.ID,
                    OriginalChannelIndex = -1,
                    Name = faultCurve.Algorithm,
                    Units = string.Empty,
                    Data = ToDataSeries(faultCurve, faultRecordInfo.Line.Length)
                });

            // Get COMTRADE channel information for each cycle data series to be output to the file
            IEnumerable<COMTRADEChannelData> cycleChannelData = GetCycleChannelData(faultRecordInfo.CycleDataSet, faultRecordInfo.WaveformChannels.Select(tuple => tuple.Item1));

            // Return all the analog channel data in one list
            return waveformChannelData
                .Concat(faultCurveChannelData)
                .Concat(cycleChannelData)
                .OrderBy(channelData => channelData.GroupOrder)
                .ThenBy(channelData => channelData.LoadOrder)
                .ToList();
        }

        private List<COMTRADEChannelData> GetDigitalChannelData(FaultRecordInfo faultRecordInfo)
        {
            // Get an arbitrary data series from the collection of waveforms so
            // that we can use it to determine the timestamp of each data point
            DataSeries series = faultRecordInfo.WaveformChannels.First().Item2;

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
            foreach (FaultSegment segment in faultRecordInfo.FaultSegments)
            {
                // Get the fault type of the segment
                FaultType faultType = GetFaultType(segment.SegmentType);

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

        private void FixCOMTRADEData(COMTRADEData comtradeData, Parser parser)
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
                        Time = parser.Timestamp,
                        Value = parser.Values[analogChannels + channelData.OriginalChannelIndex]
                    });
                }
            }

            // Add the new digital channels to the digitalChannelData collection
            comtradeData.DigitalChannelData.AddRange(originalDigitalChannelData);
        }

        private IEnumerable<COMTRADEChannelData> GetCycleChannelData(VICycleDataSet cycleDataSet, IEnumerable<OutputChannel> outputChannels)
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
                Tuple.Create("VA", cycleDataSet.VA),
                Tuple.Create("VB", cycleDataSet.VB),
                Tuple.Create("VC", cycleDataSet.VC),
                Tuple.Create("IA", cycleDataSet.IA),
                Tuple.Create("IB", cycleDataSet.IB),
                Tuple.Create("IC", cycleDataSet.IC)
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

        // Get the fault type of the given segment.
        private static FaultType GetFaultType(SegmentType segmentType)
        {
            switch (segmentType.Name)
            {
                case "AN Fault": return FaultType.AN;
                case "BN Fault": return FaultType.BN;
                case "CN Fault": return FaultType.CN;
                case "AB Fault": return FaultType.AB;
                case "BC Fault": return FaultType.BC;
                case "CA Fault": return FaultType.CA;
                case "ABG Fault": return FaultType.ABG;
                case "BCG Fault": return FaultType.BCG;
                case "CAG Fault": return FaultType.CAG;
                case "3-Phase Fault": return FaultType.ABC;
                default: return FaultType.None;
            }
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

        // Convert the given fault curve to a data series capped at the given maximum fault distance.
        private DataSeries ToDataSeries(FaultLocationData.FaultCurveRow faultCurve, double lineLength)
        {
            DataSeries series = ToDataSeries(faultCurve);
            double maxFaultDistance = lineLength * MaxFaultDistanceMultiplier;
            double minFaultDistance = lineLength * MinFaultDistanceMultiplier;

            foreach (DataPoint dataPoint in series.DataPoints)
            {
                if (double.IsNaN(dataPoint.Value))
                    dataPoint.Value = maxFaultDistance;
                else if (dataPoint.Value > maxFaultDistance)
                    dataPoint.Value = maxFaultDistance;
                else if (dataPoint.Value < minFaultDistance)
                    dataPoint.Value = minFaultDistance;
            }

            return series;
        }

        // Convert the given fault curve to a data series.
        private DataSeries ToDataSeries(FaultLocationData.FaultCurveRow faultCurve)
        {
            DataGroup dataGroup = new DataGroup();
            dataGroup.FromData(faultCurve.Data);
            return dataGroup[0];
        }

        #endregion
    }
}
