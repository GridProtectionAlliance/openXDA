//******************************************************************************************************
//  XMLConfigLoader.cs - Gbtc
//
//  Copyright © 2021, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the MIT License (MIT), the "License"; you may not use this
//  file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://opensource.org/licenses/MIT
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  10/22/2021 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using GSF.Data;
using GSF.Data.Model;
using log4net;
using openXDA.Model;

namespace openXDA.XMLConfigLoader
{
    public class XMLConfigLoader
    {
        #region [ Static ]
        // Static Fields
        private static readonly ILog Log = LogManager.GetLogger(typeof(XMLConfigLoader));
        #endregion

        #region [ Properties ]
        private string ConnectionString { get;  set; }
        private string DataProvider { get; set; }
        #endregion

        #region [ Constructor ]
        public XMLConfigLoader(string connectionString, string dataProvider)
        {
            ConnectionString = connectionString;
            DataProvider = dataProvider;
        }
        #endregion

        /// <summary>
        /// Loads config data in XDA database based on information from a config file
        /// </summary>
        /// <param name="file">Path to the xml file to be loaded into the XDA database</param>
        public void Load(string file) {
            XmlDocument document = new XmlDocument();
            document.Load(file);
            Load(document);

        }

        /// <summary>
        /// Loads config data in XDA database based on information from a config file
        /// </summary>
        /// <param name="file"><seealso cref="Stream"/> containing the xml file to be loaded into the XDA database</param>
        public void Load(Stream file)
        {
            XmlDocument document = new XmlDocument();
            document.Load(file);
            Load(document);

        }

        private void Load(XmlDocument document) {
            XmlNode xdaNode = document.SelectSingleNode("OpenXDA");

            // First pass load all stations
            foreach (XmlNode node in xdaNode.ChildNodes)
                LoadStation(node);

            // Second pass load all meters
            foreach (XmlNode stationNode in xdaNode.ChildNodes)
                foreach (XmlNode deviceNode in stationNode.SelectNodes("Device"))
                {
                    LoadMeter(stationNode, deviceNode);
                }

            // Third pass load all assets, meterasset links and Channels now that all the meters are defined and loaded into the database
            foreach (XmlNode stationNode in xdaNode.ChildNodes)
                foreach (XmlNode deviceNode in stationNode.SelectNodes("Device"))
                {
                    foreach (XmlNode assetNode in deviceNode.ChildNodes)
                    {
                        // Load the bus data in database
                        string tagName = assetNode.Name;
                        switch (tagName)
                        {
                            case "Bus":
                                LoadBus(assetNode);
                                break;
                            case "Breaker":
                                LoadBreaker(assetNode);
                                break;
                            case "CapacitorBank":
                                LoadCapacitorBank(assetNode);
                                break;
                            case "Transformer":
                                LoadTransformer(assetNode);
                                break;
                            case "CapacitorBankRelay":
                                LoadCapacitorBankRelay(assetNode);
                                break;
                            case "DER":
                                LoadDER(assetNode);
                                break;
                            case "Line":
                                LoadLine(assetNode);
                                // Load Line Segments and Channels 
                                foreach (XmlNode lineSegmentNode in assetNode.SelectNodes("Segment"))
                                {
                                    LoadLineSegment(lineSegmentNode);
                                    // Check Meter Asset Link
                                    LinkMeterAsset(deviceNode, lineSegmentNode);

                                    // Add end stations
                                    foreach (XmlNode endStationNode in lineSegmentNode.SelectNodes("Station"))
                                    {
                                        LinkAssetLocation(endStationNode, lineSegmentNode);
                                    }
                                }
                                // Load Line Sements links
                                foreach (XmlNode lineSegmentNode in assetNode.SelectNodes("Segment"))
                                    foreach (XmlNode childSegmentNode in assetNode.SelectNodes("ChildSegment"))
                                        LinkLineSegments(lineSegmentNode, childSegmentNode);
                                break;

                            default:
                                Console.WriteLine("Unsupported asset type.");
                                continue;  // Unsupported type  skip the rest of the stuff


                        }

                        // Check Meter Asset Link
                        LinkMeterAsset(deviceNode, assetNode);
                        // Load Asset Location data
                        LinkAssetLocation(stationNode, assetNode);
                        // Load Channels and Series
                        foreach (XmlNode channelNode in assetNode.SelectNodes("Channel"))
                        {
                            LoadChannel(deviceNode, assetNode, channelNode);
                            foreach (XmlNode seriesNode in channelNode.SelectNodes("Series"))
                            {
                                LoadSeries(deviceNode, assetNode, channelNode, seriesNode);
                            }

                        }
                    }
                }


            // Fourth pass load all Asset Connections now that all the assets are defined and loaded into the database
            foreach (XmlNode stationNode in xdaNode.ChildNodes)
                foreach (XmlNode deviceNode in stationNode.SelectNodes("Device"))
                    foreach (XmlNode assetNode in deviceNode.ChildNodes)
                        foreach (XmlNode connectiontNode in assetNode.SelectNodes("Connection"))
                            LinkAssetConnections(assetNode, connectiontNode);


        }

        private void LoadStation(XmlNode node)
        {
            Location station = new Location()
            {
                LocationKey = node.Attributes["LocationKey"].Value,
                Name = node.Attributes["Name"].Value,
                Longitude = double.Parse(node.Attributes["Longitude"].Value),
                Latitude = double.Parse(node.Attributes["Latitude"].Value),

            };

            using (AdoDataConnection connection = new AdoDataConnection(ConnectionString, DataProvider))
            {
                Location record = new TableOperations<Location>(connection).QueryRecordWhere("LocationKey = {0}", station.LocationKey);
                if (record == null) record = station;
                else
                {
                    record.Name = station.Name;
                    record.Longitude = station.Longitude;
                    record.Latitude = station.Latitude;
                }

                new TableOperations<Location>(connection).AddNewOrUpdateRecord(record);
            }
            Log.Info($"Loaded Station {station.LocationKey}");

        }

        private void LoadMeter(XmlNode stationNode, XmlNode meterNode)
        {
            string locationKey = stationNode.Attributes["LocationKey"].Value;

            Meter meter = new Meter()
            {
                AssetKey = meterNode.Attributes["AssetKey"].Value,
                Name = meterNode.Attributes["Name"].Value,
                Make = meterNode.Attributes["Make"].Value,
                Model = meterNode.Attributes["Model"].Value,
            };

            // Query from database 
            using (AdoDataConnection connection = new AdoDataConnection(ConnectionString, DataProvider))
            {
                Location station = new TableOperations<Location>(connection).QueryRecordWhere("LocationKey = {0}", locationKey);
                meter.LocationID = station.ID;

                Meter record = new TableOperations<Meter>(connection).QueryRecordWhere("AssetKey = {0}", meter.AssetKey);

                if (record == null) record = meter;
                else
                {
                    record.Name = meter.Name;
                    record.Make = meter.Make;
                    record.Model = meter.Model;
                }

                new TableOperations<Meter>(connection).AddNewOrUpdateRecord(record);
            }
            Log.Info($"Loaded Device {meter.AssetKey}");

        }

        private void LoadBus(XmlNode assetNode)
        {
            Bus bus = new Bus()
            {
                AssetKey = assetNode.Attributes["AssetKey"].Value,
                AssetName = assetNode.Attributes["AssetName"].Value,
                VoltageKV = float.Parse(assetNode.Attributes["Make"]?.Value ?? "0.0"),
                Spare = (assetNode.Attributes["Model"]?.Value ?? "0") == "1",
                Description = assetNode.Attributes["Description"]?.Value
            };

            // Query from database 
            using (AdoDataConnection connection = new AdoDataConnection(ConnectionString, DataProvider))
            {
                int assetTypeID = connection.ExecuteScalar<int>("SELECT ID FROM AssetType WHERE Name = 'Bus'");
                bus.AssetTypeID = assetTypeID;

                Bus record = new TableOperations<Bus>(connection).QueryRecordWhere("AssetKey = {0}", bus.AssetKey);

                if (record == null) record = bus;
                else
                {
                    record.AssetName = bus.AssetName;
                    record.VoltageKV = bus.VoltageKV;
                    record.Spare = bus.Spare;
                    record.Description = bus.Description;
                }

                new TableOperations<Bus>(connection).AddNewOrUpdateRecord(record);
            }
            Log.Info($"Loaded Bus {bus.AssetKey}");

        }

        private void LoadBreaker(XmlNode assetNode)
        {

            // Query from database 
            using (AdoDataConnection connection = new AdoDataConnection(ConnectionString, DataProvider))
            {
                string assetKey = assetNode.Attributes["AssetKey"].Value;
                int assetTypeID = connection.ExecuteScalar<int>("SELECT ID FROM AssetType WHERE Name = 'Breaker'");

                Breaker record = new TableOperations<Breaker>(connection).QueryRecordWhere("AssetKey = {0}", assetKey);

                if (record == null)
                {
                    record = new Breaker()
                    {
                        AssetKey = assetNode.Attributes["AssetKey"].Value,
                        AssetTypeID = assetTypeID
                    };
                }

                record.AssetName = assetNode.Attributes["AssetName"].Value;
                record.VoltageKV = double.Parse(assetNode.Attributes["Make"]?.Value ?? "0.0");
                record.Spare = (assetNode.Attributes["Model"]?.Value ?? "0") == "1";
                record.Description = assetNode.Attributes["Description"]?.Value;

                record.ThermalRating = double.Parse(assetNode.Attributes["ThermalRating"]?.Value ?? "0.0");
                record.Speed = double.Parse(assetNode.Attributes["Speed"]?.Value ?? "0.0");
                record.PickupTime = int.Parse(assetNode.Attributes["PickupTime"]?.Value ?? "0");
                record.TripTime = int.Parse(assetNode.Attributes["TripTime"]?.Value ?? "0");
                record.TripCoilCondition = double.Parse(assetNode.Attributes["TripCoilCondition"]?.Value ?? "0.0");

                new TableOperations<Breaker>(connection).AddNewOrUpdateRecord(record);
                Log.Info($"Loaded Breaker {record.AssetKey}");

            }

        }

        private void LoadCapacitorBank(XmlNode assetNode)
        {

            // Query from database 
            using (AdoDataConnection connection = new AdoDataConnection(ConnectionString, DataProvider))
            {
                string assetKey = assetNode.Attributes["AssetKey"].Value;
                int assetTypeID = connection.ExecuteScalar<int>("SELECT ID FROM AssetType WHERE Name = 'CapacitorBank'");

                CapBank record = new TableOperations<CapBank>(connection).QueryRecordWhere("AssetKey = {0}", assetKey);

                if (record == null)
                {
                    record = new CapBank()
                    {
                        AssetKey = assetNode.Attributes["AssetKey"].Value,
                        AssetTypeID = assetTypeID
                    };
                }

                record.AssetName = assetNode.Attributes["AssetName"].Value;
                record.VoltageKV = double.Parse(assetNode.Attributes["Make"]?.Value ?? "0.0");
                record.Spare = (assetNode.Attributes["Model"]?.Value ?? "0") == "1";
                record.Description = assetNode.Attributes["Description"]?.Value;

                record.NumberOfBanks = int.Parse(assetNode.Attributes["NumberOfBanks"]?.Value ?? "0");
                record.CapacitancePerBank = double.Parse(assetNode.Attributes["Speed"]?.Value ?? "0.0");
                record.CktSwitcher = assetNode.Attributes["CktSwitcher"]?.Value ?? "";
                record.MaxKV = double.Parse(assetNode.Attributes["MaxKV"]?.Value ?? "0.0");
                record.UnitKV = double.Parse(assetNode.Attributes["UnitKV"]?.Value ?? "0.0");
                record.UnitKVAr = double.Parse(assetNode.Attributes["UnitKVAr"]?.Value ?? "0.0");
                record.NegReactanceTol = double.Parse(assetNode.Attributes["NegReactanceTol"]?.Value ?? "0.0");
                record.PosReactanceTol = double.Parse(assetNode.Attributes["PosReactanceTol"]?.Value ?? "0.0");
                record.Nparalell = int.Parse(assetNode.Attributes["Nparalell"]?.Value ?? "0");
                record.NSeriesGroup = int.Parse(assetNode.Attributes["NSeriesGroup"]?.Value ?? "0");
                record.NParalellGroup = int.Parse(assetNode.Attributes["NParalellGroup"]?.Value ?? "0");
                record.Fused = int.Parse(assetNode.Attributes["Fused"]?.Value ?? "0") == 1;
                record.VTratioBus = double.Parse(assetNode.Attributes["VTratioBus"]?.Value ?? "0.0");
                record.NumberLVCaps = int.Parse(assetNode.Attributes["NumberLVCaps"]?.Value ?? "0");
                record.NumberLVUnits = int.Parse(assetNode.Attributes["NumberLVUnits"]?.Value ?? "0");
                record.LVKVAr = double.Parse(assetNode.Attributes["LVKVAr"]?.Value ?? "0.0");
                record.LVKV = double.Parse(assetNode.Attributes["LVKV"]?.Value ?? "0.0");
                record.LVNegReactanceTol = double.Parse(assetNode.Attributes["LVNegReactanceTol"]?.Value ?? "0.0");
                record.LVPosReactanceTol = double.Parse(assetNode.Attributes["LVPosReactanceTol"]?.Value ?? "0.0");
                record.LowerXFRRatio = double.Parse(assetNode.Attributes["LowerXFRRatio"]?.Value ?? "0.0");
                record.Nshorted = double.Parse(assetNode.Attributes["Nshorted"]?.Value ?? "0.0");
                record.BlownFuses = int.Parse(assetNode.Attributes["BlownFuses"]?.Value ?? "0");
                record.BlownGroups = int.Parse(assetNode.Attributes["BlownGroups"]?.Value ?? "0");
                record.RelayPTRatioPrimary = int.Parse(assetNode.Attributes["RelayPTRatioPrimary"]?.Value ?? "0");
                record.RelayPTRatioSecondary = int.Parse(assetNode.Attributes["RelayPTRatioSecondary"]?.Value ?? "0");
                record.Sh = double.Parse(assetNode.Attributes["Sh"]?.Value ?? "0.0");
                record.Rv = double.Parse(assetNode.Attributes["Rv"]?.Value ?? "0.0");
                record.Rh = double.Parse(assetNode.Attributes["Rh"]?.Value ?? "0.0");
                record.Compensated = int.Parse(assetNode.Attributes["Compensated"]?.Value ?? "0") == 1;
                record.NLowerGroups = int.Parse(assetNode.Attributes["NLowerGroups"]?.Value ?? "0");
                record.ShortedGroups = int.Parse(assetNode.Attributes["ShortedGroups"]?.Value ?? "0");

                new TableOperations<CapBank>(connection).AddNewOrUpdateRecord(record);
                Log.Info($"Loaded CapBank {record.AssetKey}");

            }

        }

        private void LoadTransformer(XmlNode assetNode)
        {

            // Query from database 
            using (AdoDataConnection connection = new AdoDataConnection(ConnectionString, DataProvider))
            {
                string assetKey = assetNode.Attributes["AssetKey"].Value;
                int assetTypeID = connection.ExecuteScalar<int>("SELECT ID FROM AssetType WHERE Name = 'Transformer'");

                Transformer record = new TableOperations<Transformer>(connection).QueryRecordWhere("AssetKey = {0}", assetKey);

                if (record == null)
                {
                    record = new Transformer()
                    {
                        AssetKey = assetNode.Attributes["AssetKey"].Value,
                        AssetTypeID = assetTypeID
                    };
                }

                record.AssetName = assetNode.Attributes["AssetName"].Value;
                record.VoltageKV = double.Parse(assetNode.Attributes["Make"]?.Value ?? "0.0");
                record.Spare = (assetNode.Attributes["Model"]?.Value ?? "0") == "1";
                record.Description = assetNode.Attributes["Description"]?.Value;

                record.R0 = double.Parse(assetNode.Attributes["R0"]?.Value ?? "0.0");
                record.X0 = double.Parse(assetNode.Attributes["X0"]?.Value ?? "0.0");
                record.R1 = int.Parse(assetNode.Attributes["R1"]?.Value ?? "0");
                record.X1 = int.Parse(assetNode.Attributes["X1"]?.Value ?? "0");
                record.ThermalRating = double.Parse(assetNode.Attributes["ThermalRating"]?.Value ?? "0.0");
                record.SecondaryVoltageKV = double.Parse(assetNode.Attributes["SecondaryVoltageKV"]?.Value ?? "0.0");
                record.PrimaryVoltageKV = double.Parse(assetNode.Attributes["PrimaryVoltageKV"]?.Value ?? "0.0");
                record.TertiaryVoltageKV = double.Parse(assetNode.Attributes["TertiaryVoltageKV"]?.Value ?? "0.0");
                record.SecondaryWinding = double.Parse(assetNode.Attributes["SecondaryWinding"]?.Value ?? "0.0");
                record.PrimaryWinding = double.Parse(assetNode.Attributes["PrimaryWinding"]?.Value ?? "0.0");
                record.TertiaryWinding = double.Parse(assetNode.Attributes["TertiaryWinding"]?.Value ?? "0.0");
                record.Tap = double.Parse(assetNode.Attributes["Tap"]?.Value ?? "0.0");

                new TableOperations<Transformer>(connection).AddNewOrUpdateRecord(record);
                Log.Info($"Loaded Transformer {record.AssetKey}");

            }

        }

        private void LoadCapacitorBankRelay(XmlNode assetNode)
        {

            // Query from database 
            using (AdoDataConnection connection = new AdoDataConnection(ConnectionString, DataProvider))
            {
                string assetKey = assetNode.Attributes["AssetKey"].Value;
                int assetTypeID = connection.ExecuteScalar<int>("SELECT ID FROM AssetType WHERE Name = 'CapacitorBankRelay'");

                CapBankRelay record = new TableOperations<CapBankRelay>(connection).QueryRecordWhere("AssetKey = {0}", assetKey);

                if (record == null)
                {
                    record = new CapBankRelay()
                    {
                        AssetKey = assetNode.Attributes["AssetKey"].Value,
                        AssetTypeID = assetTypeID
                    };
                }

                record.AssetName = assetNode.Attributes["AssetName"].Value;
                record.VoltageKV = double.Parse(assetNode.Attributes["Make"]?.Value ?? "0.0");
                record.Spare = (assetNode.Attributes["Model"]?.Value ?? "0") == "1";
                record.Description = assetNode.Attributes["Description"]?.Value;

                record.OnVoltageThreshhold = double.Parse(assetNode.Attributes["OnVoltageThreshhold"]?.Value ?? "0.0");
                record.CapBankNumber = int.Parse(assetNode.Attributes["CapBankNumber"]?.Value ?? "0");

                new TableOperations<CapBankRelay>(connection).AddNewOrUpdateRecord(record);
                Log.Info($"Loaded CapBankRelay {record.AssetKey}");

            }

        }

        private void LoadDER(XmlNode assetNode)
        {

            // Query from database 
            using (AdoDataConnection connection = new AdoDataConnection(ConnectionString, DataProvider))
            {
                string assetKey = assetNode.Attributes["AssetKey"].Value;
                int assetTypeID = connection.ExecuteScalar<int>("SELECT ID FROM AssetType WHERE Name = 'DER'");

                DER record = new TableOperations<DER>(connection).QueryRecordWhere("AssetKey = {0}", assetKey);

                if (record == null)
                {
                    record = new DER()
                    {
                        AssetKey = assetNode.Attributes["AssetKey"].Value,
                        AssetTypeID = assetTypeID
                    };
                }

                record.AssetName = assetNode.Attributes["AssetName"].Value;
                record.VoltageKV = double.Parse(assetNode.Attributes["Make"]?.Value ?? "0.0");
                record.Spare = (assetNode.Attributes["Model"]?.Value ?? "0") == "1";
                record.Description = assetNode.Attributes["Description"]?.Value;

                record.VoltageLevel = assetNode.Attributes["VoltageLevel"]?.Value ?? "Low";
                record.FullRatedOutputCurrent = double.Parse(assetNode.Attributes["FullRatedOutputCurrent"]?.Value ?? "0.0");

                new TableOperations<DER>(connection).AddNewOrUpdateRecord(record);
                Log.Info($"Loaded DER {record.AssetKey}");

            }

        }

        private void LoadLine(XmlNode assetNode)
        {

            // Query from database 
            using (AdoDataConnection connection = new AdoDataConnection(ConnectionString, DataProvider))
            {
                string assetKey = assetNode.Attributes["AssetKey"].Value;
                int assetTypeID = connection.ExecuteScalar<int>("SELECT ID FROM AssetType WHERE Name = 'Line'");

                Line record = new TableOperations<Line>(connection).QueryRecordWhere("AssetKey = {0}", assetKey);

                if (record == null)
                {
                    record = new Line()
                    {
                        AssetKey = assetNode.Attributes["AssetKey"].Value,
                        AssetTypeID = assetTypeID
                    };
                }

                record.AssetName = assetNode.Attributes["AssetName"].Value;
                record.VoltageKV = double.Parse(assetNode.Attributes["Make"]?.Value ?? "0.0");
                record.Spare = (assetNode.Attributes["Model"]?.Value ?? "0") == "1";
                record.Description = assetNode.Attributes["Description"]?.Value;

                record.MaxFaultDistance = double.Parse(assetNode.Attributes["MaxFaultDistance"]?.Value ?? "0.0");
                record.MinFaultDistance = double.Parse(assetNode.Attributes["MinFaultDistance"]?.Value ?? "0.0");

                new TableOperations<Line>(connection).AddNewOrUpdateRecord(record);
                Log.Info($"Loaded Line {record.AssetKey}");

            }

        }

        private void LoadLineSegment(XmlNode lineSegmentNode)
        {

            using (AdoDataConnection connection = new AdoDataConnection(ConnectionString, DataProvider))
            {
                string lineSegmentKey = lineSegmentNode.Attributes["AssetKey"].Value;

                int assetTypeID = connection.ExecuteScalar<int>("SELECT ID FROM AssetType WHERE Name = 'LineSegment'");

                LineSegment record = new TableOperations<LineSegment>(connection).QueryRecordWhere("AssetKey = {0}", lineSegmentKey);

                if (record == null)
                {
                    record = new LineSegment()
                    {
                        AssetKey = lineSegmentKey,
                        AssetTypeID = assetTypeID
                    };
                }

                record.AssetName = lineSegmentNode.Attributes["AssetName"].Value;
                record.VoltageKV = double.Parse(lineSegmentNode.Attributes["Make"]?.Value ?? "0.0");
                record.Spare = (lineSegmentNode.Attributes["Model"]?.Value ?? "0") == "1";
                record.Description = lineSegmentNode.Attributes["Description"]?.Value;

                record.R0 = double.Parse(lineSegmentNode.Attributes["R0"]?.Value ?? "0.0");
                record.X0 = double.Parse(lineSegmentNode.Attributes["X0"]?.Value ?? "0.0");
                record.R1 = int.Parse(lineSegmentNode.Attributes["R1"]?.Value ?? "0");
                record.X1 = int.Parse(lineSegmentNode.Attributes["X1"]?.Value ?? "0");
                record.ThermalRating = double.Parse(lineSegmentNode.Attributes["ThermalRating"]?.Value ?? "0.0");
                record.Length = double.Parse(lineSegmentNode.Attributes["ThermalRating"]?.Value ?? "0.0");

                new TableOperations<LineSegment>(connection).AddNewOrUpdateRecord(record);

            }

        }

        private void LinkMeterAsset(XmlNode meterNode, XmlNode assetNode)
        {
            string assetKey = assetNode.Attributes["AssetKey"].Value;
            string meterKey = meterNode.Attributes["AssetKey"].Value;

            using (AdoDataConnection connection = new AdoDataConnection(ConnectionString, DataProvider))
            {
                int meterID = connection.ExecuteScalar<int>("SELECT ID FROM Meter WHERE AssetKey = {0}", meterKey);
                int assetID = connection.ExecuteScalar<int>("SELECT ID FROM Asset WHERE AssetKey = {0}", assetKey);
                MeterAsset record = new TableOperations<MeterAsset>(connection).QueryRecordWhere("MeterID = {0} AND AssetID = {1}", meterID, assetID);

                if (record == null)
                {
                    record = new MeterAsset();
                    record.MeterID = meterID;
                    record.AssetID = assetID;
                }

                new TableOperations<MeterAsset>(connection).AddNewRecord(record);
                Log.Info($"Loaded Meter Asset Link between {meterKey} AND {assetKey}");

            }

        }

        private void LinkAssetLocation(XmlNode stationNode, XmlNode assetNode)
        {
            string assetKey = assetNode.Attributes["AssetKey"].Value;
            string locationKey = stationNode.Attributes["LocationKey"].Value;

            using (AdoDataConnection connection = new AdoDataConnection(ConnectionString, DataProvider))
            {
                int locationID = connection.ExecuteScalar<int>("SELECT ID FROM Location WHERE LocationKey = {0}", locationKey);
                int assetID = connection.ExecuteScalar<int>("SELECT ID FROM Asset WHERE AssetKey = {0}", assetKey);
                AssetLocation record = new TableOperations<AssetLocation>(connection).QueryRecordWhere("LocationID = {0} AND AssetID = {1}", locationID, assetID);

                if (record == null)
                {
                    record = new AssetLocation();
                    record.LocationID = locationID;
                    record.AssetID = assetID;
                }

                new TableOperations<AssetLocation>(connection).AddNewRecord(record);
                Log.Info($"Loaded Meter Asset Link between {locationKey} AND {assetKey}");

            }

        }

        private void LinkLineSegments(XmlNode parentNode, XmlNode childNode)
        {

            using (AdoDataConnection connection = new AdoDataConnection(ConnectionString, DataProvider))
            {
                string parentKey = parentNode.Attributes["AssetKey"].Value;
                int parentID = connection.ExecuteScalar<int>("SELECT ID FROM Asset WHERE AssetKey = {0}", parentKey);
                string childKey = childNode.Attributes["Key"].Value;
                int childID = connection.ExecuteScalar<int>("SELECT ID FROM Asset WHERE AssetKey = {0}", childKey);

                LineSegmentConnections record = new TableOperations<LineSegmentConnections>(connection).QueryRecordWhere("ParentSegment = {0} AND ChildSegment = {1}", parentID, childID);

                if (record == null)
                {
                    record = new LineSegmentConnections();
                    record.ParentSegment = parentID;
                    record.ChildSegment = childID;

                    new TableOperations<LineSegmentConnections>(connection).AddNewRecord(record);

                }


            }

        }

        private void LinkAssetConnections(XmlNode parentNode, XmlNode connectionNode)
        {

            using (AdoDataConnection connection = new AdoDataConnection(ConnectionString, DataProvider))
            {
                string parentKey = parentNode.Attributes["AssetKey"].Value;
                int parentID = connection.ExecuteScalar<int>("SELECT ID FROM Asset WHERE AssetKey = {0}", parentKey);
                string childKey = connectionNode.Attributes["Child"].Value;
                int childID = connection.ExecuteScalar<int>("SELECT ID FROM Asset WHERE AssetKey = {0}", childKey);
                string assetConnectionType = connectionNode.Attributes["Type"].Value;
                int assetConnectionTypeID = connection.ExecuteScalar<int>("SELECT ID FROM AssetRelationshipType WHERE Name = {0}", assetConnectionType);

                AssetConnection record = new TableOperations<AssetConnection>(connection).QueryRecordWhere("ParentID = {0} AND ChildID = {1} AND AssetRelationshipTypeID = {2}", parentID, childID, assetConnectionTypeID);

                if (record == null)
                {
                    record = new AssetConnection();
                    record.ParentID = parentID;
                    record.ChildID = childID;
                    record.AssetRelationshipTypeID = assetConnectionTypeID;
                    new TableOperations<AssetConnection>(connection).AddNewRecord(record);

                }


            }

        }

        private void LoadChannel(XmlNode meterNode, XmlNode assetNode, XmlNode channelNode)
        {
            using (AdoDataConnection connection = new AdoDataConnection(ConnectionString, DataProvider))
            {
                string assetKey = assetNode.Attributes["AssetKey"].Value;
                int assetID = connection.ExecuteScalar<int>("SELECT ID FROM Asset WHERE AssetKey = {0}", assetKey);

                string meterKey = meterNode.Attributes["AssetKey"].Value;
                int meterID = connection.ExecuteScalar<int>("SELECT ID FROM Meter WHERE AssetKey = {0}", meterKey);

                string measurementType = channelNode.Attributes["MeasurementType"].Value;
                int measurementTypeID = connection.ExecuteScalar<int>("SELECT ID FROM MeasurementType WHERE Name = {0}", measurementType);

                string measurementCharacteristic = channelNode.Attributes["MeasurementCharacteristic"].Value;
                int measurementCharacteristicID = connection.ExecuteScalar<int>("SELECT ID FROM MeasurementCharacteristic WHERE Name = {0}", measurementCharacteristic);

                string phase = channelNode.Attributes["Phase"].Value;
                int phaseID = connection.ExecuteScalar<int>("SELECT ID FROM Phase WHERE Name = {0}", phase);

                Channel channel = new Channel()
                {
                    MeterID = meterID,
                    AssetID = assetID,
                    Name = channelNode.Attributes["Name"].Value,
                    Description = channelNode.Attributes["Description"].Value,
                    Enabled = channelNode.Attributes["Description"].Value == "1",
                    SamplesPerHour = double.Parse(channelNode.Attributes["SamplesPerHour"]?.Value ?? "0.0"),
                    Adder = double.Parse(channelNode.Attributes["Adder"]?.Value ?? "0.0"),
                    Multiplier = double.Parse(channelNode.Attributes["Multiplier"]?.Value ?? "1.0"),
                    HarmonicGroup = int.Parse(channelNode.Attributes["HarmonicGroup"]?.Value ?? "0"),
                    ConnectionPriority = int.Parse(channelNode.Attributes["ConnectionPriority"]?.Value ?? "0"),

                    MeasurementTypeID = measurementTypeID,
                    MeasurementCharacteristicID = measurementCharacteristicID,
                    PhaseID = phaseID
                };

                Channel record = new TableOperations<Channel>(connection).QueryRecordWhere("MeterID = {0} AND AssetID = {1} AND MeasurementTypeID = {2} AND MeasurementCharacteristicID = {3} AND PhaseID = {4} AND HarmonicGroup = {5}", channel.MeterID, channel.AssetID, channel.MeasurementTypeID, channel.MeasurementCharacteristicID, channel.PhaseID, channel.HarmonicGroup);

                if (record == null) record = channel;
                else
                {
                    record.Name = channel.Name;
                    record.Description = channel.Description;
                    record.Enabled = channel.Enabled;
                    record.Adder = channel.Adder;
                    record.Multiplier = channel.Multiplier;
                    record.ConnectionPriority = channel.ConnectionPriority;

                }

                new TableOperations<Channel>(connection).AddNewOrUpdateRecord(record);


            }

        }

        private void LoadSeries(XmlNode meterNode, XmlNode assetNode, XmlNode channelNode, XmlNode seriesNode)
        {
            using (AdoDataConnection connection = new AdoDataConnection(ConnectionString, DataProvider))
            {
                string seriesType = seriesNode.Attributes["Type"].Value;
                int seriesTypeID = connection.ExecuteScalar<int>("SELECT ID FROM SeriesType WHERE Name = {0}", seriesType);

                string sourceIndexes = seriesNode.Attributes["SourceIndexes"].Value;

                string assetKey = assetNode.Attributes["AssetKey"].Value;
                int assetID = connection.ExecuteScalar<int>("SELECT ID FROM Asset WHERE AssetKey = {0}", assetKey);

                string meterKey = meterNode.Attributes["AssetKey"].Value;
                int meterID = connection.ExecuteScalar<int>("SELECT ID FROM Meter WHERE AssetKey = {0}", meterKey);

                string measurementType = channelNode.Attributes["MeasurementType"].Value;
                int measurementTypeID = connection.ExecuteScalar<int>("SELECT ID FROM MeasurementType WHERE Name = {0}", measurementType);

                string measurementCharacteristic = channelNode.Attributes["MeasurementCharacteristic"].Value;
                int measurementCharacteristicID = connection.ExecuteScalar<int>("SELECT ID FROM MeasurementCharacteristic WHERE Name = {0}", measurementCharacteristic);

                string phase = channelNode.Attributes["Phase"].Value;
                int phaseID = connection.ExecuteScalar<int>("SELECT ID FROM Phase WHERE Name = {0}", phase);
                int harmonicGroup = int.Parse(channelNode.Attributes["HarmonicGroup"]?.Value ?? "0");

                Channel channel = new TableOperations<Channel>(connection).QueryRecordWhere("MeterID = {0} AND AssetID = {1} AND MeasurementTypeID = {2} AND MeasurementCharacteristicID = {3} AND PhaseID = {4} AND HarmonicGroup = {5}", meterID, assetID, measurementTypeID, measurementCharacteristicID, phaseID, harmonicGroup);
                Series record = new TableOperations<Series>(connection).QueryRecordWhere("ChannelID = {0} AND SeriesTypeID = {1} ", channel.ID, seriesTypeID);
                if (record == null)
                {
                    record = new Series();
                    record.ChannelID = channel.ID;
                    record.SeriesTypeID = seriesTypeID;
                }

                record.SourceIndexes = sourceIndexes;

                new TableOperations<Series>(connection).AddNewOrUpdateRecord(record);


            }
        }

    }
}
