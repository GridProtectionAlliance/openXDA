//******************************************************************************************************
//  Program.cs - Gbtc
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
using System.Linq;
using System.Threading;
using System.Xml.Linq;
using FaultData.Database;
using GSF.Collections;

namespace DeviceDefinitionsMigrator
{
    class Program
    {
        private class ProgressTracker
        {
            private int m_progress;
            private int m_total;
            private ManualResetEvent m_pendingWaitHandle;
            private Thread m_pendingMessageThread;

            public ProgressTracker(int total)
            {
                m_total = total;
            }

            private double ProgressRatio
            {
                get
                {
                    return m_progress / (double)m_total;
                }
            }

            public void MakeProgress()
            {
                m_progress++;
            }

            public void WriteMessage(string message)
            {
                FinishPendingMessageLoop();
                Console.WriteLine("[{0:0.00%}] {1}", ProgressRatio, message);
            }

            public void StartPendingMessage(string message)
            {
                FinishPendingMessageLoop();
                Console.Write("[{0:0.00%}] {1}", ProgressRatio, message);
                StartPendingMessageLoop();
            }

            public void EndPendingMessage()
            {
                FinishPendingMessageLoop();
            }

            private void StartPendingMessageLoop()
            {
                m_pendingWaitHandle = new ManualResetEvent(false);
                m_pendingMessageThread = new Thread(ExecutePendingMessageLoop);
                m_pendingMessageThread.IsBackground = true;
                m_pendingMessageThread.Start();
            }

            private void ExecutePendingMessageLoop()
            {
                while (!m_pendingWaitHandle.WaitOne(TimeSpan.FromSeconds(2.0D)))
                {
                    Console.Write(".");
                }

                Console.WriteLine("done.");
            }

            private void FinishPendingMessageLoop()
            {
                if ((object)m_pendingWaitHandle != null)
                {
                    m_pendingWaitHandle.Set();
                    m_pendingMessageThread.Join();
                    m_pendingWaitHandle.Dispose();
                    m_pendingWaitHandle = null;
                }
            }
        }

        private class TupleIgnoreCase : IEqualityComparer<Tuple<string, string>>
        {
            public bool Equals(Tuple<string, string> x, Tuple<string, string> y)
            {
                return StringComparer.OrdinalIgnoreCase.Equals(x.Item1, y.Item1) &&
                       StringComparer.OrdinalIgnoreCase.Equals(x.Item2, y.Item2);
            }

            public int GetHashCode(Tuple<string, string> obj)
            {
                int hash = 17;
                hash = hash * 31 + StringComparer.OrdinalIgnoreCase.GetHashCode(obj.Item1);
                hash = hash * 31 + StringComparer.OrdinalIgnoreCase.GetHashCode(obj.Item2);
                return hash;
            }

            public static TupleIgnoreCase Default
            {
                get
                {
                    return s_default;
                }
            }

            private static readonly TupleIgnoreCase s_default = new TupleIgnoreCase();
        }

        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Usage:");
                Console.WriteLine("    DeviceDefinitionsMigrator <ConnectionString> <FilePath>");
                Console.WriteLine();
                Console.WriteLine("Example:");
                Console.WriteLine("    DeviceDefinitionsMigrator \"Data Source=localhost; Initial Catalog=openXDA; Integrated Security=SSPI\" \"C:\\Program Files\\openFLE\\DeviceDefinitions.xml\"");

                Environment.Exit(0);
            }

            try
            {
                string connectionString = args[0];
                string deviceDefinitionsFile = args[1];

                Migrate(connectionString, deviceDefinitionsFile);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("--- ERROR ---");
                Console.Error.WriteLine(ex.Message);
            }
        }

        private class LookupTables
        {
            private MeterInfoDataContext m_meterInfo;
            private FaultLocationInfoDataContext m_faultLocationInfo;

            public Dictionary<string, Meter> MeterLookup;
            public Dictionary<string, Line> LineLookup;
            public Dictionary<string, MeterLocation> MeterLocationLookup;
            public Dictionary<Tuple<string, string>, MeterLine> MeterLineLookup;
            public Dictionary<Tuple<string, string>, MeterLocationLine> MeterLocationLineLookup;
            public Dictionary<string, MeasurementType> MeasurementTypeLookup;
            public Dictionary<string, MeasurementCharacteristic> MeasurementCharacteristicLookup;
            public Dictionary<string, Phase> PhaseLookup;
            public Dictionary<string, SeriesType> SeriesTypeLookup;

            public Dictionary<Line, LineImpedance> LineImpedanceLookup;
            public Dictionary<MeterLocationLine, SourceImpedance> SourceImpedanceLookup;

            public LookupTables(MeterInfoDataContext meterInfo, FaultLocationInfoDataContext faultLocationInfo)
            {
                m_meterInfo = meterInfo;
                m_faultLocationInfo = faultLocationInfo;
            }

            public void CreateLookups(XDocument document)
            {
                List<XElement> deviceElements = document.Elements().Elements("device").ToList();
                List<XElement> lineElements = deviceElements.Elements("lines").Elements("line").ToList();

                MeterLookup = GetMeterLookup(deviceElements, m_meterInfo);
                LineLookup = GetLineLookup(lineElements, m_meterInfo);
                MeterLocationLookup = GetMeterLocationLookup(deviceElements, lineElements, m_meterInfo);
                MeterLineLookup = GetMeterLineLookup(MeterLookup.Values, LineLookup.Values, m_meterInfo);
                MeterLocationLineLookup = GetMeterLocationLineLookup(MeterLocationLookup.Values, LineLookup.Values, m_meterInfo);
                MeasurementTypeLookup = GetMeasurementTypeLookup(m_meterInfo);
                MeasurementCharacteristicLookup = GetMeasurementCharacteristicLookup(m_meterInfo);
                PhaseLookup = GetPhaseLookup(m_meterInfo);
                SeriesTypeLookup = GetSeriesTypeLookup(m_meterInfo);

                LineImpedanceLookup = GetLineImpedanceLookup(LineLookup.Values, m_faultLocationInfo);
                SourceImpedanceLookup = GetSourceImpedanceLookup(MeterLocationLineLookup.Values, m_faultLocationInfo);
            }

            public Dictionary<string, Tuple<Series, OutputChannel>> GetChannelLookup(Meter meter, Line line)
            {
                return meter.Channels
                    .Where(channel => channel.LineID == line.ID)
                    .SelectMany(channel => channel.Series)
                    .Join(m_faultLocationInfo.OutputChannels, series => series.ID, outputChannel => outputChannel.SeriesID, Tuple.Create)
                    .ToDictionary(tuple => tuple.Item2.ChannelKey, StringComparer.OrdinalIgnoreCase);
            }

            private Dictionary<string, Meter> GetMeterLookup(List<XElement> deviceElements, MeterInfoDataContext meterInfo)
            {
                List<string> deviceIDs = deviceElements
                    .Select(deviceElement => (string)deviceElement.Attribute("id"))
                    .Where(id => (object)id != null)
                    .Distinct()
                    .ToList();

                return meterInfo.Meters
                    .Where(meter => deviceIDs.Contains(meter.AssetKey))
                    .ToDictionary(meter => meter.AssetKey, StringComparer.OrdinalIgnoreCase);
            }

            private Dictionary<string, Line> GetLineLookup(List<XElement> lineElements, MeterInfoDataContext meterInfo)
            {
                List<string> lineIDs = lineElements
                    .Select(lineElement => (string)lineElement.Attribute("id"))
                    .Where(id => (object)id != null)
                    .Distinct()
                    .ToList();

                return meterInfo.Lines
                    .Where(line => lineIDs.Contains(line.AssetKey))
                    .ToDictionary(line => line.AssetKey, StringComparer.OrdinalIgnoreCase);
            }

            private Dictionary<string, MeterLocation> GetMeterLocationLookup(List<XElement> deviceElements, List<XElement> lineElements, MeterInfoDataContext meterInfo)
            {
                List<string> meterLocationIDs = deviceElements
                    .Select(deviceElement => deviceElement.Element("attributes") ?? new XElement("attributes"))
                    .Select(deviceAttributes => (string)deviceAttributes.Element("stationID"))
                    .Concat(lineElements.Select(lineElement => (string)lineElement.Element("endStationID")))
                    .Distinct()
                    .ToList();

                return meterInfo.MeterLocations
                    .Where(meterLocation => meterLocationIDs.Contains(meterLocation.AssetKey))
                    .ToDictionary(meterLocation => meterLocation.AssetKey, StringComparer.OrdinalIgnoreCase);
            }

            private Dictionary<Tuple<string, string>, MeterLine> GetMeterLineLookup(IEnumerable<Meter> meters, IEnumerable<Line> lines, MeterInfoDataContext meterInfo)
            {
                List<int> meterIDs = meters
                    .Select(meter => meter.ID)
                    .ToList();

                List<int> lineIDs = lines
                    .Select(line => line.ID)
                    .ToList();

                return meterInfo.MeterLines
                    .Where(meterLine => meterIDs.Contains(meterLine.MeterID))
                    .Where(meterLine => lineIDs.Contains(meterLine.LineID))
                    .ToDictionary(meterLine => Tuple.Create(meterLine.Meter.AssetKey, meterLine.Line.AssetKey), TupleIgnoreCase.Default);
            }

            private Dictionary<Tuple<string, string>, MeterLocationLine> GetMeterLocationLineLookup(IEnumerable<MeterLocation> meterLocations, IEnumerable<Line> lines, MeterInfoDataContext meterInfo)
            {
                List<int> meterLocationIDs = meterLocations
                    .Select(meterLocation => meterLocation.ID)
                    .ToList();

                List<int> lineIDs = lines
                    .Select(line => line.ID)
                    .ToList();

                return meterInfo.MeterLocationLines
                    .Where(meterLocationLine => meterLocationIDs.Contains(meterLocationLine.MeterLocationID))
                    .Where(meterLocationLine => lineIDs.Contains(meterLocationLine.LineID))
                    .ToDictionary(meterLocationLine => Tuple.Create(meterLocationLine.MeterLocation.AssetKey, meterLocationLine.Line.AssetKey), TupleIgnoreCase.Default);
            }

            private Dictionary<string, MeasurementType> GetMeasurementTypeLookup(MeterInfoDataContext meterInfo)
            {
                return meterInfo.MeasurementTypes.ToDictionary(measurementType => measurementType.Name, StringComparer.OrdinalIgnoreCase);
            }

            private Dictionary<string, MeasurementCharacteristic> GetMeasurementCharacteristicLookup(MeterInfoDataContext meterInfo)
            {
                return meterInfo.MeasurementCharacteristics.ToDictionary(measurementCharacteristic => measurementCharacteristic.Name, StringComparer.OrdinalIgnoreCase);
            }

            private Dictionary<string, Phase> GetPhaseLookup(MeterInfoDataContext meterInfo)
            {
                return meterInfo.Phases.ToDictionary(phase => phase.Name, StringComparer.OrdinalIgnoreCase);
            }

            private Dictionary<string, SeriesType> GetSeriesTypeLookup(MeterInfoDataContext meterInfo)
            {
                return meterInfo.SeriesTypes.ToDictionary(seriesType => seriesType.Name, StringComparer.OrdinalIgnoreCase);
            }

            private Dictionary<Line, LineImpedance> GetLineImpedanceLookup(IEnumerable<Line> lines, FaultLocationInfoDataContext faultLocationInfo)
            {
                return faultLocationInfo.LineImpedances
                    .Join(lines, impedance => impedance.LineID, line => line.ID, Tuple.Create)
                    .ToDictionary(tuple => tuple.Item2, tuple => tuple.Item1);
            }

            private Dictionary<MeterLocationLine, SourceImpedance> GetSourceImpedanceLookup(IEnumerable<MeterLocationLine> meterLocationLines, FaultLocationInfoDataContext faultLocationInfo)
            {
                return faultLocationInfo.SourceImpedances
                    .Join(meterLocationLines, impedance => impedance.MeterLocationLineID, meterLocationLine => meterLocationLine.ID, Tuple.Create)
                    .ToDictionary(tuple => tuple.Item2, tuple => tuple.Item1);
            }
        }

        private static void Migrate(string connectionString, string deviceDefinitionsFile)
        {
            LookupTables lookupTables;

            MeterLocation meterLocation;
            MeterLocation remoteMeterLocation;
            MeterLocationLine localLink;
            MeterLocationLine remoteLink;

            Meter meter;
            Line line;
            Series series;
            Channel channel;
            OutputChannel outputChannel;

            Dictionary<string, Tuple<Series, OutputChannel>> channelLookup;
            Tuple<Series, OutputChannel> tuple;
            string channelKey;
            int outputChannelIndex;

            LineImpedance lineImpedance;
            SourceImpedance localSourceImpedance;
            SourceImpedance remoteSourceImpedance;

            XDocument document = XDocument.Load(deviceDefinitionsFile);
            List<XElement> deviceElements = document.Elements().Elements("device").ToList();
            XElement deviceAttributes;
            XElement impedancesElement;

            List<Tuple<Line, LineImpedance>> lineImpedances = new List<Tuple<Line, LineImpedance>>();
            List<Tuple<MeterLocationLine, SourceImpedance>> sourceImpedances = new List<Tuple<MeterLocationLine, SourceImpedance>>();
            List<Tuple<Series, OutputChannel>> outputChannels = new List<Tuple<Series, OutputChannel>>();

            ProgressTracker progressTracker = new ProgressTracker(deviceElements.Count);

            using (MeterInfoDataContext meterInfo = new MeterInfoDataContext(connectionString))
            using (FaultLocationInfoDataContext faultLocationInfo = new FaultLocationInfoDataContext(connectionString))
            {
                // Load existing fault location configuration from the database
                progressTracker.StartPendingMessage("Loading existing fault location configuration from database...");
                lookupTables = new LookupTables(meterInfo, faultLocationInfo);
                lookupTables.CreateLookups(document);
                progressTracker.EndPendingMessage();

                // Load updates to fault location algorithms into the database
                progressTracker.StartPendingMessage("Loading updates to fault location algorithms into the database...");

                foreach (XElement analyticsElement in document.Elements().Elements("analytics"))
                    LoadFaultLocationAlgorithms(analyticsElement, faultLocationInfo);

                faultLocationInfo.SubmitChanges();

                progressTracker.EndPendingMessage();

                // Load updates to device configuration into the database
                progressTracker.WriteMessage(string.Format("Beginning migration of {0} device configurations...", deviceElements.Count));

                foreach (XElement deviceElement in deviceElements)
                {
                    lineImpedances.Clear();
                    sourceImpedances.Clear();
                    outputChannels.Clear();

                    // Get the element representing a device's attributes
                    deviceAttributes = deviceElement.Element("attributes") ?? new XElement("attributes");

                    // Attempt to find existing configuration for this device and update the meter with any changes to the device's attributes
                    meter = lookupTables.MeterLookup.GetOrAdd((string)deviceElement.Attribute("id"), assetKey => new Meter() { AssetKey = assetKey });
                    LoadMeterAttributes(meter, deviceAttributes);

                    // Now that we know what meter we are processing, display a message to indicate that we are parsing this meter's configuration
                    progressTracker.StartPendingMessage(string.Format("Loading configuration for meter {0} ({1})...", meter.Name, meter.AssetKey));

                    // Attempt to find existing configuration for the location of the meter and update with configuration changes
                    meterLocation = lookupTables.MeterLocationLookup.GetOrAdd((string)deviceAttributes.Element("stationID"), assetKey => new MeterLocation() { AssetKey = assetKey });
                    LoadMeterLocationAttributes(meterLocation, deviceAttributes);

                    // Link the meter location to the meter
                    meter.MeterLocation = meterLocation;

                    // Load updates to line configuration into the database
                    foreach (XElement lineElement in deviceElement.Elements("lines").Elements("line"))
                    {
                        // Attempt to find existing configuration for the line and update with configuration changes
                        line = lookupTables.LineLookup.GetOrAdd((string)lineElement.Attribute("id"), assetKey => new Line() { AssetKey = assetKey });
                        LoadLineAttributes(line, lineElement);

                        // Provide a link between this line and the location housing the meter
                        Link(meter, line, lineElement, lookupTables.MeterLineLookup);
                        localLink = Link(meterLocation, line, lookupTables.MeterLocationLineLookup);

                        if ((string)lineElement.Element("endStationID") != null)
                        {
                            // Attempt to find existing configuration for the location of the other end of the line and update with configuration changes
                            remoteMeterLocation = lookupTables.MeterLocationLookup.GetOrAdd((string)lineElement.Element("endStationID"), assetKey => new MeterLocation() { AssetKey = assetKey });
                            LoadRemoteMeterLocationAttributes(remoteMeterLocation, lineElement);

                            // Provide a link between this line and the remote location
                            remoteLink = Link(remoteMeterLocation, line, lookupTables.MeterLocationLineLookup);
                        }
                        else
                        {
                            // Set remote meter location to null so we
                            // know later that there isn't one defined
                            remoteMeterLocation = null;
                            remoteLink = null;
                        }

                        // Get a lookup table for the channels monitoring this line
                        channelLookup = lookupTables.GetChannelLookup(meter, line);
                        outputChannelIndex = 0;

                        foreach (XElement channelElement in lineElement.Elements("channels").Elements())
                        {
                            channelKey = channelElement.Name.LocalName;

                            // Attempt to find an existing channel corresponding to this element
                            if (channelLookup.TryGetValue(channelKey, out tuple))
                            {
                                series = tuple.Item1;
                                channel = series.Channel;
                                outputChannel = tuple.Item2;
                            }
                            else
                            {
                                channel = new Channel();
                                series = new Series();
                                outputChannel = new OutputChannel();

                                channelLookup.Add(channelKey, Tuple.Create(series, outputChannel));
                            }

                            // Load updates to channel configuration into the database
                            LoadChannelAttributes(meter, line, remoteMeterLocation, channel, channelKey, lookupTables);
                            LoadSeriesAttributes(channel, series, channelElement, lookupTables);

                            outputChannel.ChannelKey = channelKey;
                            outputChannel.LoadOrder = outputChannelIndex;
                            outputChannels.Add(Tuple.Create(series, outputChannel));

                            outputChannelIndex++;
                        }

                        impedancesElement = lineElement.Element("impedances") ?? new XElement("impedances");

                        // Attempt to find existing impedance configuration for the line and update with configuration changes
                        lineImpedance = lookupTables.LineImpedanceLookup.GetOrAdd(line, ln => new LineImpedance());
                        LoadLineImpedanceAttributes(lineImpedance, impedancesElement);
                        lineImpedances.Add(Tuple.Create(line, lineImpedance));

                        // Attempt to find existing impedance configuration for the meter's location and update with configuration changes
                        localSourceImpedance = lookupTables.SourceImpedanceLookup.GetOrAdd(localLink, location => new SourceImpedance());
                        LoadLocalSourceImpedanceAttributes(localSourceImpedance, impedancesElement);
                        sourceImpedances.Add(Tuple.Create(localLink, localSourceImpedance));

                        if ((object)remoteLink != null)
                        {
                            // Attempt to find existing impedance configuration for the remote location and update with configuration changes
                            remoteSourceImpedance = lookupTables.SourceImpedanceLookup.GetOrAdd(remoteLink, location => new SourceImpedance());
                            LoadRemoteSourceImpedanceAttributes(remoteSourceImpedance, impedancesElement);
                            sourceImpedances.Add(Tuple.Create(remoteLink, remoteSourceImpedance));
                        }
                    }

                    if (meter.ID == 0)
                        meterInfo.Meters.InsertOnSubmit(meter);

                    meterInfo.SubmitChanges();

                    // Load updates to line impedance configuration into the database
                    foreach (Tuple<Line, LineImpedance> mapping in lineImpedances)
                    {
                        line = mapping.Item1;
                        lineImpedance = mapping.Item2;
                        lineImpedance.LineID = line.ID;

                        if (lineImpedance.ID == 0)
                            faultLocationInfo.LineImpedances.InsertOnSubmit(lineImpedance);
                    }

                    // Load updates to source impedance configuration into the database
                    foreach (Tuple<MeterLocationLine, SourceImpedance> mapping in sourceImpedances)
                    {
                        localLink = mapping.Item1;
                        localSourceImpedance = mapping.Item2;
                        localSourceImpedance.MeterLocationLineID = localLink.ID;

                        if (localSourceImpedance.ID == 0 && (localSourceImpedance.RSrc != 0.0D || localSourceImpedance.XSrc != 0.0D))
                            faultLocationInfo.SourceImpedances.InsertOnSubmit(localSourceImpedance);
                    }

                    // Load updates to source impedance configuration into the database
                    foreach (Tuple<Series, OutputChannel> mapping in outputChannels)
                    {
                        series = mapping.Item1;
                        outputChannel = mapping.Item2;
                        outputChannel.SeriesID = series.ID;

                        if (outputChannel.ID == 0)
                            faultLocationInfo.OutputChannels.InsertOnSubmit(outputChannel);
                    }

                    faultLocationInfo.SubmitChanges();

                    progressTracker.EndPendingMessage();

                    // Increment the progress counter
                    progressTracker.MakeProgress();
                }
            }
        }

        private static void LoadFaultLocationAlgorithms(XElement analyticsElement, FaultLocationInfoDataContext faultLocationInfo)
        {
            List<FaultLocationAlgorithm> oldFaultLocationAlgorithms = faultLocationInfo.FaultLocationAlgorithms.ToList();

            List<FaultLocationAlgorithm> newFaultLocationAlgorithms = analyticsElement
                .Elements("faultLocation")
                .Select(ToFaultLocationAlgorithm)
                .ToList();

            faultLocationInfo.FaultLocationAlgorithms.InsertAllOnSubmit(newFaultLocationAlgorithms
                .Where(algorithm1 => !oldFaultLocationAlgorithms.Any(algorithm2 => IsMatch(algorithm1, algorithm2))));

            faultLocationInfo.FaultLocationAlgorithms.DeleteAllOnSubmit(oldFaultLocationAlgorithms
                .Where(algorithm1 => !newFaultLocationAlgorithms.Any(algorithm2 => IsMatch(algorithm1, algorithm2))));
        }

        private static FaultLocationAlgorithm ToFaultLocationAlgorithm(XElement faultLocationElement)
        {
            string assemblyName = (string)faultLocationElement.Attribute("assembly");
            string method = (string)faultLocationElement.Attribute("method");

            int index = method.LastIndexOf('.');
            string typeName = method.Substring(0, index);
            string methodName = method.Substring(index + 1);

            return new FaultLocationAlgorithm()
            {
                AssemblyName = assemblyName,
                TypeName = typeName,
                MethodName = methodName
            };
        }

        private static bool IsMatch(FaultLocationAlgorithm algorithm1, FaultLocationAlgorithm algorithm2)
        {
            return algorithm1.AssemblyName == algorithm2.AssemblyName
                && algorithm1.TypeName == algorithm2.TypeName
                && algorithm1.MethodName == algorithm2.MethodName;
        }

        private static void LoadMeterAttributes(Meter meter, XElement deviceAttributes)
        {
            string meterName = (string)deviceAttributes.Element("name") ?? (string)deviceAttributes.Element("stationName");

            if (meter.Name != meterName)
            {
                meter.Name = meterName;
                meter.ShortName = new string(meterName.Take(50).ToArray());
            }

            meter.Make = (string)deviceAttributes.Element("make") ?? string.Empty;
            meter.Model = (string)deviceAttributes.Element("model") ?? string.Empty;
        }

        private static void LoadMeterLocationAttributes(MeterLocation meterLocation, XElement deviceAttributes)
        {
            string meterLocationName = (string)deviceAttributes.Element("stationName");
            string latitude = (string)deviceAttributes.Element("stationLatitude");
            string longitude = (string)deviceAttributes.Element("stationLongitude");

            if (meterLocation.Name != meterLocationName)
            {
                meterLocation.Name = meterLocationName;
                meterLocation.ShortName = new string(meterLocationName.Take(50).ToArray());
            }

            if ((object)latitude != null)
                meterLocation.Latitude = Convert.ToDouble(latitude);

            if ((object)longitude != null)
                meterLocation.Longitude = Convert.ToDouble(longitude);
        }

        private static void LoadRemoteMeterLocationAttributes(MeterLocation meterLocation, XElement lineElement)
        {
            string meterLocationName = (string)lineElement.Element("endStationName");
            string latitude = (string)lineElement.Element("endStationLatitude");
            string longitude = (string)lineElement.Element("endStationLongitude");

            if (meterLocation.Name != meterLocationName)
            {
                meterLocation.Name = meterLocationName;
                meterLocation.ShortName = new string(meterLocationName.Take(50).ToArray());
            }

            if ((object)latitude != null)
                meterLocation.Latitude = Convert.ToDouble(latitude);

            if ((object)longitude != null)
                meterLocation.Longitude = Convert.ToDouble(longitude);
        }

        private static void LoadLineAttributes(Line line, XElement lineElement)
        {
            line.VoltageKV = Convert.ToDouble((string)lineElement.Element("voltage"));
            line.ThermalRating = Convert.ToDouble((string)lineElement.Element("rating50F") ?? "0.0");
            line.Length = Convert.ToDouble((string)lineElement.Element("length") ?? "0");
        }

        private static MeterLine Link(Meter meter, Line line, XElement lineElement, Dictionary<Tuple<string, string>, MeterLine> meterLineLookup)
        {
            Tuple<string, string> key = Tuple.Create(meter.AssetKey, line.AssetKey);
            MeterLine meterLine;

            if (!meterLineLookup.TryGetValue(key, out meterLine))
            {
                meterLine = new MeterLine()
                {
                    Meter = meter,
                    Line = line,
                    LineName = (string)lineElement.Element("name")
                };

                meterLineLookup.Add(key, meterLine);
            }

            return meterLine;
        }

        private static MeterLocationLine Link(MeterLocation meterLocation, Line line, Dictionary<Tuple<string, string>, MeterLocationLine> meterLocationLineLookup)
        {
            Tuple<string, string> key = Tuple.Create(meterLocation.AssetKey, line.AssetKey);
            MeterLocationLine meterLocationLine;

            if (!meterLocationLineLookup.TryGetValue(key, out meterLocationLine))
            {
                meterLocationLine = new MeterLocationLine()
                {
                    MeterLocation = meterLocation,
                    Line = line
                };

                meterLocationLineLookup.Add(key, meterLocationLine);
            }

            return meterLocationLine;
        }

        private static void LoadChannelAttributes(Meter meter, Line line, MeterLocation remoteMeterLocation, Channel channel, string channelKey, LookupTables lookupTables)
        {
            if ((object)remoteMeterLocation != null)
                channel.Name = string.Format("{0}({1}) {2}", remoteMeterLocation.Name, line.AssetKey, channelKey);
            else
                channel.Name = string.Format("({0}) {1}", line.AssetKey, channelKey);

            channel.HarmonicGroup = 0;
            channel.MeasurementType = lookupTables.MeasurementTypeLookup.GetOrAdd(GetMeasurementTypeName(channelKey), name => new MeasurementType() { Name = name, Description = name });
            channel.MeasurementCharacteristic = lookupTables.MeasurementCharacteristicLookup.GetOrAdd("Instantaneous", name => new MeasurementCharacteristic() { Name = name, Description = name });
            channel.Phase = lookupTables.PhaseLookup.GetOrAdd(GetPhaseName(channelKey), name => new Phase() { Name = name, Description = name });

            channel.Meter = meter;
            channel.Line = line;
        }

        private static void LoadSeriesAttributes(Channel channel, Series series, XElement channelElement, LookupTables lookupTables)
        {
            series.SeriesType = lookupTables.SeriesTypeLookup.GetOrAdd("Values", name => new SeriesType() { Name = name, Description = name });
            series.Channel = channel;
            series.SourceIndexes = (string)channelElement ?? string.Empty;
        }

        private static void LoadLineImpedanceAttributes(LineImpedance lineImpedance, XElement impedancesElement)
        {
            lineImpedance.R0 = Convert.ToDouble((string)impedancesElement.Element("R0"));
            lineImpedance.X0 = Convert.ToDouble((string)impedancesElement.Element("X0"));
            lineImpedance.R1 = Convert.ToDouble((string)impedancesElement.Element("R1"));
            lineImpedance.X1 = Convert.ToDouble((string)impedancesElement.Element("X1"));
        }

        private static void LoadLocalSourceImpedanceAttributes(SourceImpedance sourceImpedance, XElement impedancesElement)
        {
            object rSrc = (string)impedancesElement.Element("RSrc");
            object xSrc = (string)impedancesElement.Element("XSrc");

            if (rSrc != null && xSrc != null)
            {
                sourceImpedance.RSrc = Convert.ToDouble(rSrc);
                sourceImpedance.XSrc = Convert.ToDouble(xSrc);
            }
        }

        private static void LoadRemoteSourceImpedanceAttributes(SourceImpedance sourceImpedance, XElement impedancesElement)
        {
            object rSrc = (string)impedancesElement.Element("RRem");
            object xSrc = (string)impedancesElement.Element("XRem");

            if (rSrc != null && xSrc != null)
            {
                sourceImpedance.RSrc = Convert.ToDouble(rSrc);
                sourceImpedance.XSrc = Convert.ToDouble(xSrc);
            }
        }

        private static string GetMeasurementTypeName(string channelName)
        {
            string measurementTypeName;

            if (!MeasurementTypeNameLookup.TryGetValue(channelName, out measurementTypeName))
                measurementTypeName = "Unknown";

            return measurementTypeName;
        }

        private static string GetPhaseName(string channelName)
        {
            string phaseName;

            if (!PhaseNameLookup.TryGetValue(channelName, out phaseName))
                phaseName = "Unknown";

            return phaseName;
        }

        private static readonly Dictionary<string, string> MeasurementTypeNameLookup = new Dictionary<string, string>()
        {
            { "VA", "Voltage" },
            { "VB", "Voltage" },
            { "VC", "Voltage" },
            { "IA", "Current" },
            { "IB", "Current" },
            { "IC", "Current" },
            { "IR", "Current" },
            { "IN", "Current" },
            { "IG", "Current" }
        };

        private static readonly Dictionary<string, string> PhaseNameLookup = new Dictionary<string, string>()
        {
            { "VA", "AN" },
            { "VB", "BN" },
            { "VC", "CN" },
            { "IA", "AN" },
            { "IB", "BN" },
            { "IC", "CN" },
            { "IR", "RES" },
            { "IN", "NG" },
            { "IG", "NG" }
        };
    }
}
