//******************************************************************************************************
//  Program.cs - Gbtc
//
//  Copyright © 2019, Grid Protection Alliance.  All Rights Reserved.
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
//  07/10/2019 - Christoph Lackner
//       Added trip coil currents.
//  12/15/2019 - Christoph Lackner
//       Overhaul to import data into asset centric models.
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Xml.Linq;
using GSF.Collections;
using GSF.Data;
using GSF.Data.Model;
using openXDA.Model;

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
                Console.Error.WriteLine(ex.ToString());
            }
        }

        private class LookupTables
        {
            public Dictionary<string, Meter> MeterLookup;
            public Dictionary<string, Asset> AssetLookup;
            public Dictionary<Tuple<string,string>, AssetConnection> AssetConnectionLookup;
            public Dictionary<string, Location> LocationLookup;
            public Dictionary<Tuple<string, string>, MeterAsset> MeterLineLookup;
            public Dictionary<Tuple<string, string>, AssetLocation> AssetLocationLookup;
            public Dictionary<string, MeasurementType> MeasurementTypeLookup;
            public Dictionary<string, MeasurementCharacteristic> MeasurementCharacteristicLookup;
            public Dictionary<string, Phase> PhaseLookup;
            public Dictionary<string, SeriesType> SeriesTypeLookup;

            public Dictionary<AssetLocation, SourceImpedance> SourceImpedanceLookup;

            public void CreateLookups(XDocument document, AdoDataConnection connection)
            {
                List<XElement> deviceElements = document.Elements().Elements("device").ToList();
                List<XElement> lineElements = deviceElements.Elements("lines").Elements("line").ToList();

                TableOperations<Meter> meterTable = new TableOperations<Meter>(connection);
                TableOperations<Asset> assetTable = new TableOperations<Asset>(connection);
                TableOperations<AssetConnection> assetConnectionTable = new TableOperations<AssetConnection>(connection);
                TableOperations<Location> meterLocationTable = new TableOperations<Location>(connection);
                TableOperations<MeterAsset> meterLineTable = new TableOperations<MeterAsset>(connection);
                TableOperations<AssetLocation> assetLocationTable = new TableOperations<AssetLocation>(connection);
                TableOperations<MeasurementType> measurementTypeTable = new TableOperations<MeasurementType>(connection);
                TableOperations<MeasurementCharacteristic> measurementCharacteristicTable = new TableOperations<MeasurementCharacteristic>(connection);
                TableOperations<Phase> phaseTable = new TableOperations<Phase>(connection);
                TableOperations<SeriesType> seriesTypeTable = new TableOperations<SeriesType>(connection);

                MeterLookup = meterTable.QueryRecords().ToDictionary(meter => meter.AssetKey, StringComparer.OrdinalIgnoreCase);
                AssetLookup = assetTable.QueryRecords().ToDictionary(asset => asset.AssetKey, StringComparer.OrdinalIgnoreCase);
                AssetConnectionLookup = assetConnectionTable.QueryRecords().ToDictionary(assetconection => Tuple.Create(AssetLookup.Values.Where(asset => asset.ID == assetconection.ChildID).First().AssetKey, AssetLookup.Values.Where(asset => asset.ID == assetconection.ParentID).First().AssetKey));
                LocationLookup = meterLocationTable.QueryRecords().ToDictionary(meterLocation => meterLocation.LocationKey, StringComparer.OrdinalIgnoreCase);
                MeterLineLookup = meterLineTable.QueryRecords().ToDictionary(meterLocation => Tuple.Create(MeterLookup.Values.Where(meter => meter.ID == meterLocation.MeterID).First().AssetKey, AssetLookup.Values.Where(asset => asset.ID == meterLocation.AssetID).First().AssetKey));
                AssetLocationLookup = assetLocationTable.QueryRecords().ToDictionary(meterLocationLine => Tuple.Create(LocationLookup.Values.Where(meterLocation => meterLocation.ID == meterLocationLine.LocationID).First().LocationKey, AssetLookup.Values.Where(line => line.ID == meterLocationLine.AssetID).First().AssetKey));
                MeasurementTypeLookup = measurementTypeTable.QueryRecords().ToDictionary(measurementType => measurementType.Name, StringComparer.OrdinalIgnoreCase);
                MeasurementCharacteristicLookup = measurementCharacteristicTable.QueryRecords().ToDictionary(measurementCharacteristic => measurementCharacteristic.Name, StringComparer.OrdinalIgnoreCase);
                PhaseLookup = phaseTable.QueryRecords().ToDictionary(phase => phase.Name, StringComparer.OrdinalIgnoreCase);
                SeriesTypeLookup = seriesTypeTable.QueryRecords().ToDictionary(seriesType => seriesType.Name, StringComparer.OrdinalIgnoreCase);

                SourceImpedanceLookup = GetSourceImpedanceLookup(AssetLocationLookup.Values, connection);
              

            }

            public Dictionary<string, Tuple<Series, OutputChannel>> GetChannelLookup(Meter meter, Asset asset, AdoDataConnection connection)
            {
                TableOperations<OutputChannel> outputChannelTable = new TableOperations<OutputChannel>(connection);

                meter.ConnectionFactory = () => new AdoDataConnection(connection.Connection, typeof(SqlDataAdapter), false);

                return meter.Channels
                    .Where(channel => channel.AssetID == asset.ID)
                    .SelectMany(channel => channel.Series)
                    .Join(outputChannelTable.QueryRecords(), series => series.ID, outputChannel => outputChannel.SeriesID, Tuple.Create)
                    .ToDictionary(tuple => tuple.Item2.ChannelKey, StringComparer.OrdinalIgnoreCase);
            }

            private Dictionary<AssetLocation, SourceImpedance> GetSourceImpedanceLookup(IEnumerable<AssetLocation> meterLocationLines, AdoDataConnection connection)
            {
                TableOperations<SourceImpedance> sourceImpedanceTable = new TableOperations<SourceImpedance>(connection);

                return sourceImpedanceTable.QueryRecords()
                    .Join(meterLocationLines, impedance => impedance.AssetLocationID, meterLocationLine => meterLocationLine.ID, Tuple.Create)
                    .ToDictionary(tuple => tuple.Item2, tuple => tuple.Item1);
            }
        }

        private static void Migrate(string connectionString, string deviceDefinitionsFile)
        {
            LookupTables lookupTables;

            Location location;
            Location remoteLocation;
            AssetLocation localLink;
            AssetLocation remoteLink;

            Meter meter;
            Asset asset;
            Series series;
            Channel channel;
            OutputChannel outputChannel;

            Dictionary<string, Tuple<Series, OutputChannel>> channelLookup;
            Tuple<Series, OutputChannel> tuple;
            string channelKey;
            int outputChannelIndex;

            SourceImpedance localSourceImpedance;
            SourceImpedance remoteSourceImpedance;

            XDocument document = XDocument.Load(deviceDefinitionsFile);
            List<XElement> deviceElements = document.Elements().Elements("device").ToList();
            XElement deviceAttributes;

            List<Tuple<AssetLocation, SourceImpedance>> sourceImpedances = new List<Tuple<AssetLocation, SourceImpedance>>();
            List<Tuple<Series, OutputChannel>> outputChannels = new List<Tuple<Series, OutputChannel>>();

            ProgressTracker progressTracker = new ProgressTracker(deviceElements.Count);

            using (AdoDataConnection connection = new AdoDataConnection(connectionString, "AssemblyName={System.Data, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089}; ConnectionType=System.Data.SqlClient.SqlConnection; AdapterType=System.Data.SqlClient.SqlDataAdapter"))
            {
                // Load existing fault location configuration from the database
                progressTracker.StartPendingMessage("Loading existing fault location configuration from database...");
                lookupTables = new LookupTables();
                lookupTables.CreateLookups(document, connection);
                progressTracker.EndPendingMessage();

                // Load updates to fault location algorithms into the database
                progressTracker.StartPendingMessage("Loading updates to fault location algorithms into the database...");

                foreach (XElement analyticsElement in document.Elements().Elements("analytics"))
                    LoadFaultLocationAlgorithms(analyticsElement, connection);

                progressTracker.EndPendingMessage();

                // Load updates to device configuration into the database
                progressTracker.WriteMessage(string.Format("Beginning migration of {0} device configurations...", deviceElements.Count));

                foreach (XElement deviceElement in deviceElements)
                {
                    sourceImpedances.Clear();
                    outputChannels.Clear();

                    // Get the element representing a device's attributes
                    deviceAttributes = deviceElement.Element("attributes") ?? new XElement("attributes");

                    // Attempt to find existing configuration for the location of the meter and update with configuration changes
                    location = lookupTables.LocationLookup.GetOrAdd((string)deviceAttributes.Element("stationID"), assetKey => new Location() { LocationKey = assetKey });
                    LoadMeterLocationAttributes(location, deviceAttributes, lookupTables, connection);

                    // Attempt to find existing configuration for this device and update the meter with any changes to the device's attributes
                    meter = lookupTables.MeterLookup.GetOrAdd((string)deviceElement.Attribute("id"), assetKey => new Meter() { AssetKey = assetKey });
                    // Link the meter location to the meter
                    meter.LocationID = location.ID;

                    LoadMeterAttributes(meter, deviceAttributes, lookupTables, connection);

                    // Now that we know what meter we are processing, display a message to indicate that we are parsing this meter's configuration
                    progressTracker.StartPendingMessage(string.Format("Loading configuration for meter {0} ({1})...", meter.Name, meter.AssetKey));

                    // ******* This is for backwards compatibility *******
                    List<XElement> assetElements = deviceElement.Elements("lines").Elements("line").ToList();
                    assetElements.AddRange(deviceElement.Elements("assets").Elements("asset").ToList());
                    // Assume any elemnt that does not have a type specified is a line
                    // not very intuitive but necesarry to make sure this works with fioles pre- model change 
                    // *********************************************************

                    // Load updates to Asset configuration into the database
                    foreach (XElement lineElement in assetElements)
                    {
                        // Attempt to find existing configuration for the asset and update with configuration changes
                        asset = lookupTables.AssetLookup.GetOrAdd((string)lineElement.Attribute("id"), assetKey => new Asset() { AssetKey = assetKey });

                        LoadAssetAttributes(asset, lineElement, lookupTables, connection);

                        //call corresponding Attribute Function
                        string assetType = (string)lineElement.Attribute("type") ?? "line";

                        switch(assetType)
                        {
                            case "breaker":
                                asset.AssetTypeID = (int)AssetType.Breaker;
                                lookupTables.AssetLookup[asset.AssetKey] = LoadBreakerAttributes(asset, lineElement, lookupTables, connection);
                                asset = lookupTables.AssetLookup[asset.AssetKey];
                                break;

                            case "bus":
                                asset.AssetTypeID = (int)AssetType.Bus;
                                lookupTables.AssetLookup[asset.AssetKey] = LoadBusAttributes(asset, lineElement, lookupTables, connection);
                                asset = lookupTables.AssetLookup[asset.AssetKey];
                                break;

                            case "transformer":
                                asset.AssetTypeID = (int)AssetType.Transformer;
                                lookupTables.AssetLookup[asset.AssetKey] = LoadXfrAttributes(asset, lineElement, lookupTables, connection);
                                asset = lookupTables.AssetLookup[asset.AssetKey];
                                break;
                            case "capacitorbank":
                                asset.AssetTypeID = (int)AssetType.CapacitorBank;
                                lookupTables.AssetLookup[asset.AssetKey] = LoadCapBankAttributes(asset, lineElement, lookupTables, connection);
                                asset = lookupTables.AssetLookup[asset.AssetKey];
                                break;
                            case "line":
                                asset.AssetTypeID = (int)AssetType.Line;
                                // This is special because we have to deal with the LineSegments
                                bool addConnection = false;
                                if (asset.ID == 0)
                                {
                                    //We add a single segment to the line and add it as a Line-Linesegment asset connection
                                    string assetkey = asset.AssetKey + "-Segment1";
                                    lookupTables.AssetLookup[assetkey] = LoadSegmentAttributes(asset, lineElement, lookupTables, connection);
                                    
                                    addConnection = true;
                                }
                                lookupTables.AssetLookup[asset.AssetKey] = LoadLineAttributes(asset, lineElement, lookupTables, connection);
                                asset = lookupTables.AssetLookup[asset.AssetKey];
                                if (addConnection)
                                {
                                    lookupTables.AssetConnectionLookup[new Tuple<string,string>(asset.AssetKey, asset.AssetKey + "-Segment1")] = LoadLinetoSegmentConnectionAttributes(asset, lookupTables, connection);
                                }

                                break;

                        }

                        // Provide a link between this line and the location housing the meter
                        Link(meter, asset, lineElement, lookupTables.MeterLineLookup, connection);
                        localLink = Link(location, asset, lookupTables.AssetLocationLookup, connection);

                        if ((string)lineElement.Element("endStationID") != null)
                        {
                            // Attempt to find existing configuration for the location of the other end of the line and update with configuration changes
                            remoteLocation = lookupTables.LocationLookup.GetOrAdd((string)lineElement.Element("endStationID"), assetKey => new Location() { LocationKey = assetKey });
                            LoadRemoteMeterLocationAttributes(remoteLocation, lineElement, lookupTables, connection);

                            // Provide a link between this line and the remote location
                            remoteLink = Link(remoteLocation, asset, lookupTables.AssetLocationLookup, connection);
                        }
                        else
                        {
                            // Set remote meter location to null so we
                            // know later that there isn't one defined
                            remoteLocation = null;
                            remoteLink = null;
                        }

                        // Get a lookup table for the channels monitoring this line
                        channelLookup = lookupTables.GetChannelLookup(meter, asset, connection);
                        List<String> channelAdded = new List<String>();
                        outputChannelIndex = 0;

                        foreach (XElement channelElement in lineElement.Elements("channels").Elements())
                        {
                            channelKey = channelElement.Name.LocalName;

                            // Don't allow same key twice
                            if (channelAdded.Contains(channelKey))
                            {
                                Console.WriteLine("Channel {0} already exists in xml file, skipping duplicate", channelKey);
                                continue;
                            }

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
                                channelAdded.Add(channelKey);
                            }

                            // Load updates to channel configuration into the database
                            LoadChannelAttributes(meter, asset, remoteLocation, channel, channelKey, lookupTables, connection);
                            LoadSeriesAttributes(channel, series, channelElement, lookupTables, connection);

                            outputChannel.ChannelKey = channelKey;
                            outputChannel.LoadOrder = outputChannelIndex;
                            outputChannels.Add(Tuple.Create(series, outputChannel));
                            
                            outputChannelIndex++;
                        }

                        XElement imepdanceElement = lineElement.Element("impedances");

                        if (imepdanceElement == null)
                        {
                            imepdanceElement = lineElement;
                        }

                        // Attempt to find existing impedance configuration for the meter's location and update with configuration changes
                        localSourceImpedance = lookupTables.SourceImpedanceLookup.GetOrAdd(localLink, sourceImpedance => new SourceImpedance());
                        LoadLocalSourceImpedanceAttributes(localSourceImpedance, imepdanceElement);
                        sourceImpedances.Add(Tuple.Create(localLink, localSourceImpedance));

                        if ((object)remoteLink != null)
                        {
                            // Attempt to find existing impedance configuration for the remote location and update with configuration changes
                            remoteSourceImpedance = lookupTables.SourceImpedanceLookup.GetOrAdd(remoteLink, sourceImpedance => new SourceImpedance());
                            LoadRemoteSourceImpedanceAttributes(remoteSourceImpedance, imepdanceElement);
                            sourceImpedances.Add(Tuple.Create(remoteLink, remoteSourceImpedance));
                        }
                    }
                    
                    // Load updates to source impedance configuration into the database
                    TableOperations<SourceImpedance> sourceImpedanceTable = new TableOperations<SourceImpedance>(connection);

                    foreach (Tuple<AssetLocation, SourceImpedance> mapping in sourceImpedances)
                    {
                        localLink = mapping.Item1;
                        localSourceImpedance = mapping.Item2;
                        localSourceImpedance.AssetLocationID = localLink.ID;

                        if (localSourceImpedance.ID != 0 || localSourceImpedance.RSrc != 0.0D || localSourceImpedance.XSrc != 0.0D)
                        {
                            sourceImpedanceTable.AddNewOrUpdateRecord(localSourceImpedance);

                            if (localSourceImpedance.ID == 0)
                                localSourceImpedance.ID = connection.ExecuteScalar<int>("SELECT ID FROM SourceImpedance WHERE AssetLocationID = {0}", localSourceImpedance.AssetLocationID);
                        }
                    }

                    // Load updates to output channel configuration into the database
                    TableOperations<OutputChannel> outputChannelTable = new TableOperations<OutputChannel>(connection);

                    foreach (Tuple<Series, OutputChannel> mapping in outputChannels)
                    {
                        series = mapping.Item1;
                        outputChannel = mapping.Item2;
                        outputChannel.SeriesID = series.ID;
                        outputChannelTable.AddNewOrUpdateRecord(outputChannel);
                    }

                    progressTracker.EndPendingMessage();

                    // Increment the progress counter
                    progressTracker.MakeProgress();
                }
            }
        }

        private static void LoadFaultLocationAlgorithms(XElement analyticsElement, AdoDataConnection connection)
        {
            TableOperations<FaultLocationAlgorithm> faultLocationAlgorithmTable = new TableOperations<FaultLocationAlgorithm>(connection);

            List<FaultLocationAlgorithm> oldFaultLocationAlgorithms = faultLocationAlgorithmTable.QueryRecords().ToList();

            List<FaultLocationAlgorithm> newFaultLocationAlgorithms = analyticsElement
                .Elements("faultLocation")
                .Select(ToFaultLocationAlgorithm)
                .ToList();

            List<FaultLocationAlgorithm> toDelete = oldFaultLocationAlgorithms
                .Where(oldAlg => !newFaultLocationAlgorithms.Any(newAlg => IsMatch(oldAlg, newAlg)))
                .ToList();

            foreach (FaultLocationAlgorithm faultLocationAlgorithm in toDelete)
                faultLocationAlgorithmTable.DeleteRecord(faultLocationAlgorithm);

            List<FaultLocationAlgorithm> toAdd = newFaultLocationAlgorithms
                .Where(newAlg => !oldFaultLocationAlgorithms.Any(oldAlg => IsMatch(newAlg, oldAlg)))
                .ToList();

            foreach (FaultLocationAlgorithm faultLocationAlgorithm in toAdd)
                faultLocationAlgorithmTable.AddNewRecord(faultLocationAlgorithm);

            foreach (FaultLocationAlgorithm newFaultLocationAlgorithm in newFaultLocationAlgorithms)
            {
                foreach (FaultLocationAlgorithm oldFaultLocationAlgorithm in oldFaultLocationAlgorithms)
                {
                    bool needsUpdate =
                        IsMatch(newFaultLocationAlgorithm, oldFaultLocationAlgorithm) &&
                        newFaultLocationAlgorithm.ExecutionOrder != oldFaultLocationAlgorithm.ExecutionOrder;

                    if (needsUpdate)
                    {
                        oldFaultLocationAlgorithm.ExecutionOrder = newFaultLocationAlgorithm.ExecutionOrder;
                        faultLocationAlgorithmTable.UpdateRecord(oldFaultLocationAlgorithm);
                    }
                }
            }
        }

        private static FaultLocationAlgorithm ToFaultLocationAlgorithm(XElement faultLocationElement)
        {
            string assemblyName = (string)faultLocationElement.Attribute("assembly");
            string method = (string)faultLocationElement.Attribute("method");

            int index = method.LastIndexOf('.');
            string typeName = method.Substring(0, index);
            string methodName = method.Substring(index + 1).Replace("NovoselEtAl", "Novosel");

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

        private static void LoadMeterAttributes(Meter meter, XElement deviceAttributes, LookupTables lookupTables, AdoDataConnection connection)
        {
            string meterName = (string)deviceAttributes.Element("name")
                ?? (string)deviceAttributes.Parent?.Attribute("id")
                ?? (string)deviceAttributes.Element("stationName");

            if (meter.Name != meterName)
            {
                meter.Name = meterName;
                meter.ShortName = new string(meterName.Take(50).ToArray());
            }

            meter.Make = (string)deviceAttributes.Element("make") ?? string.Empty;
            meter.Model = (string)deviceAttributes.Element("model") ?? string.Empty;

            TableOperations<Meter> meterTable = new TableOperations<Meter>(connection);
            meterTable.AddNewOrUpdateRecord(meter);

            if (meter.ID == 0)
                meter.ID = connection.ExecuteScalar<int>("SELECT ID FROM Meter WHERE AssetKey = {0}", meter.AssetKey);
        }

        private static void LoadMeterLocationAttributes(Location meterLocation, XElement deviceAttributes, LookupTables lookupTables, AdoDataConnection connection)
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

            TableOperations<Location> meterLocationTable = new TableOperations<Location>(connection);
            meterLocationTable.AddNewOrUpdateRecord(meterLocation);

            if (meterLocation.ID == 0)
                meterLocation.ID = connection.ExecuteScalar<int>("SELECT ID FROM Location WHERE LocationKey = {0}", meterLocation.LocationKey);
        }

        private static void LoadRemoteMeterLocationAttributes(Location meterLocation, XElement lineElement, LookupTables lookupTables, AdoDataConnection connection)
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

            TableOperations<Location> meterLocationTable = new TableOperations<Location>(connection);
            meterLocationTable.AddNewOrUpdateRecord(meterLocation);

            if (meterLocation.ID == 0)
                meterLocation.ID = connection.ExecuteScalar<int>("SELECT ID FROM Location WHERE LocationKey = {0}", meterLocation.LocationKey);
        }

        private static void LoadAssetAttributes(Asset asset, XElement assetElement, LookupTables lookupTables, AdoDataConnection connection)
        {
            asset.VoltageKV = Convert.ToDouble((string)assetElement.Element("voltage"));
            asset.AssetName = (string)assetElement.Element("name") ?? "Asset";

        }

        private static Asset LoadBreakerAttributes(Asset asset, XElement lineElement, LookupTables lookupTables, AdoDataConnection connection)
        {
            Breaker breaker = Breaker.DetailedBreaker(asset, connection) ?? new Breaker() {
                AssetKey = asset.AssetKey,
                AssetName = asset.AssetName,
                AssetTypeID = (int)AssetType.Breaker,
                VoltageKV = asset.VoltageKV
            };

            breaker.ThermalRating = Convert.ToDouble((string)lineElement.Element("rating50F") ?? "0.0");
            breaker.Speed = Convert.ToDouble((string)lineElement.Element("speed") ?? "0.0");

            TableOperations<Breaker> breakerTable = new TableOperations<Breaker>(connection);
            breakerTable.AddNewOrUpdateRecord(breaker);

            if (breaker.ID == 0)
                breaker.ID = connection.ExecuteScalar<int>("SELECT ID FROM Breaker WHERE AssetKey = {0}", breaker.AssetKey);

            return breaker;
        }

        private static Asset LoadBusAttributes(Asset asset, XElement lineElement, LookupTables lookupTables, AdoDataConnection connection)
        {
            Bus bus = Bus.DetailedBus(asset, connection) ?? new Bus()
            {
                AssetKey = asset.AssetKey,
                AssetName = asset.AssetName,
                AssetTypeID = (int)AssetType.Bus,
                VoltageKV = asset.VoltageKV
            };

            TableOperations<Bus> busTable = new TableOperations<Bus>(connection);
            busTable.AddNewOrUpdateRecord(bus);

            if (bus.ID == 0)
                bus.ID = connection.ExecuteScalar<int>("SELECT ID FROM Bus WHERE AssetKey = {0}", bus.AssetKey);

            return bus;
        }

        private static Asset LoadXfrAttributes(Asset asset, XElement lineElement, LookupTables lookupTables, AdoDataConnection connection)
        {
            Transformer xfr = Transformer.DetailedTransformer(asset, connection) ?? new Transformer()
            {
                AssetKey = asset.AssetKey,
                AssetName = asset.AssetName,
                AssetTypeID = (int)AssetType.Transformer,
                VoltageKV = asset.VoltageKV
            };

            xfr.PrimaryVoltageKV = Convert.ToDouble((string)lineElement.Element("primary") ?? "0.0");
            xfr.SecondaryVoltageKV = Convert.ToDouble((string)lineElement.Element("secondary") ?? "0.0");
            xfr.Tap = Convert.ToDouble((string)lineElement.Element("tap") ?? "1.0");
            xfr.ThermalRating = Convert.ToDouble((string)lineElement.Element("rating50F") ?? "0.0");
            xfr.R0 = Convert.ToDouble((string)lineElement.Element("R0") ?? "0.0");
            xfr.R1 = Convert.ToDouble((string)lineElement.Element("R1") ?? "0.0");
            xfr.X0 = Convert.ToDouble((string)lineElement.Element("X0") ?? "0.0");
            xfr.X1 = Convert.ToDouble((string)lineElement.Element("X1") ?? "0.0");    

            TableOperations<Transformer> xfrTable = new TableOperations<Transformer>(connection);
            xfrTable.AddNewOrUpdateRecord(xfr);

            if (xfr.ID == 0)
                xfr.ID = connection.ExecuteScalar<int>("SELECT ID FROM Transformer WHERE AssetKey = {0}", xfr.AssetKey);

            return xfr;
        }

        private static Asset LoadCapBankAttributes(Asset asset, XElement lineElement, LookupTables lookupTables, AdoDataConnection connection)
        {
            CapBank capbank = CapBank.DetailedCapBank(asset, connection) ?? new CapBank()
            {
                AssetKey = asset.AssetKey,
                AssetName = asset.AssetName,
                AssetTypeID = (int)AssetType.CapacitorBank,
                VoltageKV = asset.VoltageKV
            };

            capbank.NumberOfBanks = Convert.ToInt32((string)lineElement.Element("primary") ?? "0");
            capbank.CansPerBank = Convert.ToInt32((string)lineElement.Element("secondary") ?? "0");
            capbank.CapacitancePerBank = Convert.ToDouble((string)lineElement.Element("tap") ?? "0.0");
            

            TableOperations<CapBank> capBankTable = new TableOperations<CapBank>(connection);
            capBankTable.AddNewOrUpdateRecord(capbank);

            if (capbank.ID == 0)
                capbank.ID = connection.ExecuteScalar<int>("SELECT ID FROM CapBank WHERE AssetKey = {0}", capbank.AssetKey);

            return capbank;
        }

        private static Asset LoadLineAttributes(Asset asset, XElement lineElement, LookupTables lookupTables, AdoDataConnection connection)
        {
            Line line = Line.DetailedLine(asset, connection) ?? new Line()
            {
                AssetKey = asset.AssetKey,
                AssetName = asset.AssetName,
                AssetTypeID = (int)AssetType.CapacitorBank,
                VoltageKV = asset.VoltageKV
            };

            if (double.TryParse((string)lineElement.Element("maxfaultdistance"), out double maxFaultDistance))
                line.MaxFaultDistance = maxFaultDistance;

            if (double.TryParse((string)lineElement.Element("minfaultdistance"), out double minFaultDistance))
                line.MinFaultDistance = minFaultDistance;
            
            TableOperations<Line> lineTable = new TableOperations<Line>(connection);
            lineTable.AddNewOrUpdateRecord(line);

            if (line.ID == 0)
                line.ID = connection.ExecuteScalar<int>("SELECT ID FROM Line WHERE AssetKey = {0}", line.AssetKey);

            return line;
        }

        private static Asset LoadSegmentAttributes(Asset asset, XElement lineElement, LookupTables lookupTables, AdoDataConnection connection)
        {
            string assetkey = asset.AssetKey + "-Segment1";
            LineSegment segment = new LineSegment()
            {
                AssetKey = assetkey,
                AssetName = asset.AssetName,
                AssetTypeID = (int)AssetType.LineSegement,
                Length = Convert.ToDouble((string)lineElement.Element("length") ?? "0"),
                VoltageKV = asset.VoltageKV
            };

            XElement imepdanceElement = lineElement.Element("impedances");

            if (imepdanceElement == null)
            {
                imepdanceElement = lineElement;
            }

            segment.ThermalRating = Convert.ToDouble((string)lineElement.Element("rating50F") ?? "0.0");
            segment.R0 = Convert.ToDouble((string)imepdanceElement.Element("R0") ?? "0.0");
            segment.R1 = Convert.ToDouble((string)imepdanceElement.Element("R1") ?? "0.0");
            segment.X0 = Convert.ToDouble((string)imepdanceElement.Element("X0") ?? "0.0");
            segment.X1 = Convert.ToDouble((string)imepdanceElement.Element("X1") ?? "0.0");
            segment.Length = Convert.ToDouble((string)lineElement.Element("length") ?? "0.0");

            TableOperations<LineSegment> lineSegmentTable = new TableOperations<LineSegment>(connection);
            lineSegmentTable.AddNewOrUpdateRecord(segment);
            
            segment.ID = connection.ExecuteScalar<int>("SELECT ID FROM LineSegment WHERE AssetKey = {0}", segment.AssetKey);

            return segment;
        }

        private static AssetConnection LoadLinetoSegmentConnectionAttributes(Asset line, LookupTables lookupTables, AdoDataConnection connection)
        {
            string assetkey = line.AssetKey + "-Segment1";
            Asset segment = lookupTables.AssetLookup[assetkey];

            AssetConnection assetConnection = new AssetConnection()
            {
                ChildID = segment.ID,
                ParentID = line.ID,
                AssetRelationshipTypeID = 1,
            };


            TableOperations<AssetConnection> assetConnectiontTable = new TableOperations<AssetConnection>(connection);
            assetConnectiontTable.AddNewOrUpdateRecord(assetConnection);

            assetConnection.ID = connection.ExecuteScalar<int>("SELECT ID FROM AssetRelationship WHERE ParentID = {0} AND ChildID = {1}", line.ID, segment.ID);

            return assetConnection;
        }

        
        private static MeterAsset Link(Meter meter, Asset asset, XElement lineElement, Dictionary<Tuple<string, string>, MeterAsset> meterLineLookup, AdoDataConnection connection)
        {
            Tuple<string, string> key = Tuple.Create(meter.AssetKey, asset.AssetKey);
            MeterAsset meterLine;

            if (!meterLineLookup.TryGetValue(key, out meterLine))
            {
                meterLine = new MeterAsset()
                {
                    MeterID = meter.ID,
                    AssetID = asset.ID,
                };

                TableOperations<MeterAsset> meterLineTable = new TableOperations<MeterAsset>(connection);
                meterLineTable.AddNewRecord(meterLine);
                meterLine.ID = connection.ExecuteScalar<int>("SELECT ID FROM Meter WHERE AssetKey = {0}", meter.AssetKey);

                meterLineLookup.Add(key, meterLine);
            }

            return meterLine;
        }

        private static AssetLocation Link(Location location, Asset asset, Dictionary<Tuple<string, string>, AssetLocation> assetLocationLookup, AdoDataConnection connection)
        {
            Tuple<string, string> key = Tuple.Create(location.LocationKey, asset.AssetKey);
            AssetLocation assetLocation;

            if (!assetLocationLookup.TryGetValue(key, out assetLocation))
            {
                assetLocation = new AssetLocation()
                {
                    LocationID = location.ID,
                    AssetID = asset.ID
                };

                TableOperations<AssetLocation> meterLocationLineTable = new TableOperations<AssetLocation>(connection);
                meterLocationLineTable.AddNewRecord(assetLocation);
                assetLocation.ID = connection.ExecuteScalar<int>("SELECT ID FROM AssetLocation WHERE LocationID = {0} AND AssetID = {1}", location.ID, asset.ID);

                assetLocationLookup.Add(key, assetLocation);
            }

            return assetLocation;
        }

        private static void LoadChannelAttributes(Meter meter, Asset asset, Location remoteMeterLocation, Channel channel, string channelKey, LookupTables lookupTables, AdoDataConnection connection)
        {
           
            
            channel.Name = string.Format("({0}) {1}", asset.AssetKey, channelKey);

            channel.MeterID = meter.ID;
            channel.AssetID = asset.ID;
            channel.MeasurementTypeID = GetOrAddMeasurementType(GetMeasurementTypeName(channelKey), lookupTables, connection);
            channel.MeasurementCharacteristicID = GetOrAddMeasurementCharacteristic(GetCharacteristicName(channelKey), lookupTables, connection);

            channel.PhaseID = GetOrAddPhase(GetPhaseName(channelKey), lookupTables, connection);
            channel.HarmonicGroup = 0;

            TableOperations<Channel> channelTable = new TableOperations<Channel>(connection);
            channelTable.AddNewOrUpdateRecord(channel);

            if (channel.ID == 0)
                channel.ID = connection.ExecuteScalar<int>("SELECT ID FROM Channel WHERE MeterID = {0} AND AssetID = {1} AND MeasurementTypeID = {2} AND MeasurementCharacteristicID = {3} AND PhaseID = {4} AND HarmonicGroup={5}", channel.MeterID, channel.AssetID, channel.MeasurementTypeID, channel.MeasurementCharacteristicID, channel.PhaseID, channel.HarmonicGroup);
        }

        private static void LoadSeriesAttributes(Channel channel, Series series, XElement channelElement, LookupTables lookupTables, AdoDataConnection connection)
        {
            series.ChannelID = channel.ID;
            series.SeriesTypeID = GetOrAddSeriesType(lookupTables, connection);
            series.SourceIndexes = (string)channelElement ?? string.Empty;

            TableOperations<Series> seriesTable = new TableOperations<Series>(connection);
            seriesTable.AddNewOrUpdateRecord(series);

            if (series.ID == 0)
                series.ID = connection.ExecuteScalar<int>("SELECT ID FROM Series WHERE ChannelID = {0} AND SeriesTypeID = {1}", series.ChannelID, series.SeriesTypeID);
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

        private static string GetCharacteristicName(string channelName)
        {
            string characName;

            if (!MeasurementCharacteristicNameLookup.TryGetValue(channelName, out characName))
                characName = "Unknown";

            return characName;
        }

        private static readonly Dictionary<string, string> MeasurementCharacteristicNameLookup = new Dictionary<string, string>()
        {
            { "VA", "Instantaneous" },
            { "VB", "Instantaneous" },
            { "VC", "Instantaneous" },
            { "IA", "Instantaneous" },
            { "IB", "Instantaneous" },
            { "IC", "Instantaneous" },
            { "IR", "Instantaneous" },
            { "IN", "Instantaneous" },
            { "IG", "Instantaneous" },
            { "TCE", "Instantaneous" }
        };

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
            { "IG", "Current" },
            { "TCE", "TripCoilCurrent" }
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
            { "IG", "NG" },
            { "TCE", "None" }
        };

        private static int GetOrAddSeriesType(LookupTables lookupTables, AdoDataConnection connection)
        {
            if (lookupTables.SeriesTypeLookup.ContainsKey("Values"))
                return lookupTables.SeriesTypeLookup["Values"].ID;

            SeriesType seriesType = new SeriesType() { Name = "Values", Description = "Values" };
            lookupTables.SeriesTypeLookup.Add("Values", seriesType);

            TableOperations<SeriesType> seriesTypeTable = new TableOperations<SeriesType>(connection);
            seriesTypeTable.AddNewRecord(seriesType);
            seriesType.ID = connection.ExecuteScalar<int>("SELECT ID FROM SeriesType WHERE Name = {0}", seriesType.Name);

            return seriesType.ID;
        }

        private static int GetOrAddMeasurementType(string measurementTypeName, LookupTables lookupTables, AdoDataConnection connection)
        {
            if (lookupTables.MeasurementTypeLookup.ContainsKey(measurementTypeName))
                return lookupTables.MeasurementTypeLookup[measurementTypeName].ID;

            MeasurementType measurementType = new MeasurementType() { Name = measurementTypeName, Description = measurementTypeName };
            lookupTables.MeasurementTypeLookup.Add(measurementTypeName, measurementType);

            TableOperations<MeasurementType> measurementTypeTable = new TableOperations<MeasurementType>(connection);
            measurementTypeTable.AddNewRecord(measurementType);
            measurementType.ID = connection.ExecuteScalar<int>("SELECT ID FROM MeasurementType WHERE Name = {0}", measurementTypeName);

            return measurementType.ID;
        }

        private static int GetOrAddMeasurementCharacteristic(string measurementCharacteristicName, LookupTables lookupTables, AdoDataConnection connection)
        {
            if (lookupTables.MeasurementCharacteristicLookup.ContainsKey(measurementCharacteristicName))
                return lookupTables.MeasurementCharacteristicLookup[measurementCharacteristicName].ID;

            MeasurementCharacteristic measurementCharacteristic = new MeasurementCharacteristic() { Name = measurementCharacteristicName, Description = measurementCharacteristicName };
            lookupTables.MeasurementCharacteristicLookup.Add(measurementCharacteristicName, measurementCharacteristic);

            TableOperations<MeasurementCharacteristic> measurementCharacteristicTable = new TableOperations<MeasurementCharacteristic>(connection);
            measurementCharacteristicTable.AddNewRecord(measurementCharacteristic);
            measurementCharacteristic.ID = connection.ExecuteScalar<int>("SELECT ID FROM MeasurementCharacteristic WHERE Name = {0}", measurementCharacteristicName);

            return measurementCharacteristic.ID;
        }

        private static int GetOrAddPhase(string phaseName, LookupTables lookupTables, AdoDataConnection connection)
        {
            if (lookupTables.PhaseLookup.ContainsKey(phaseName))
                return lookupTables.PhaseLookup[phaseName].ID;

            Phase phase = new Phase() { Name = phaseName, Description = phaseName };
            lookupTables.PhaseLookup.Add(phaseName, phase);

            TableOperations<Phase> phaseTable = new TableOperations<Phase>(connection);
            phaseTable.AddNewRecord(phase);
            phase.ID = connection.ExecuteScalar<int>("SELECT ID FROM Phase WHERE Name = {0}", phaseName);

            return phase.ID;
        }
    }
}
