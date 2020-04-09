//******************************************************************************************************
//  MeterSettingsSheet.cs - Gbtc
//
//  Copyright © 2019, Grid Protection Alliance.  All Rights Reserved.
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
//  09/24/2019 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using GSF.Collections;
using GSF.Data.Model;
using openXDA.Model;

namespace FaultData.Configuration
{
    public class MeterSettingsSheet
    {
        public MeterSettingsSheet(Meter meter)
        {
            Meter = meter;
            Sheet = ToSheet(meter);
        }

        public MeterSettingsSheet(XDocument sheet)
        {
            Meter = ToMeter(sheet);
            Sheet = sheet;
        }

        public MeterSettingsSheet(TableOperations<MeterConfiguration> meterConfigurationTable, MeterConfiguration meterConfiguration)
        {
            string sheetText = meterConfigurationTable.Unpatch(meterConfiguration);
            XDocument sheet = XDocument.Parse(sheetText);
            Meter = ToMeter(sheet);
            Sheet = sheet;
        }

        public Meter Meter { get; }
        public XDocument Sheet { get; }

        public void UpdateConfiguration(TableOperations<MeterConfiguration> meterConfigurationTable, string configKey) =>
            meterConfigurationTable.PatchLatestConfiguration(Meter, configKey, Sheet.ToString());

        private XDocument ToSheet(Meter meter)
        {
            XDocument sheet = new XDocument();
            XElement rootElement = new XElement("meterSettingsSheet");
            sheet.Add(rootElement);

            XElement meterElement = new XElement("meter");
            meterElement.Add(new XElement(nameof(meter.ID), meter.ID));
            meterElement.Add(new XElement(nameof(meter.AssetKey), meter.AssetKey));
            meterElement.Add(new XElement(nameof(meter.LocationID), meter.LocationID));
            meterElement.Add(new XElement(nameof(meter.Name), meter.Name));
            meterElement.Add(new XElement(nameof(meter.Alias), meter.Alias));
            meterElement.Add(new XElement(nameof(meter.ShortName), meter.ShortName));
            meterElement.Add(new XElement(nameof(meter.Make), meter.Make));
            meterElement.Add(new XElement(nameof(meter.Model), meter.Model));
            meterElement.Add(new XElement(nameof(meter.TimeZone), meter.TimeZone));
            meterElement.Add(new XElement(nameof(meter.Description), meter.Description));
            rootElement.Add(meterElement);

            XElement locationElement = new XElement("location");
            locationElement.Add(new XElement(nameof(meter.Location.ID), meter.Location.ID));
            locationElement.Add(new XElement(nameof(meter.Location.LocationKey), meter.Location.LocationKey));
            locationElement.Add(new XElement(nameof(meter.Location.Name), meter.Location.Name));
            locationElement.Add(new XElement(nameof(meter.Location.Alias), meter.Location.Alias));
            locationElement.Add(new XElement(nameof(meter.Location.ShortName), meter.Location.ShortName));
            locationElement.Add(new XElement(nameof(meter.Location.Latitude), meter.Location.Latitude));
            locationElement.Add(new XElement(nameof(meter.Location.Longitude), meter.Location.Longitude));
            locationElement.Add(new XElement(nameof(meter.Location.Description), meter.Location.Description));
            rootElement.Add(locationElement);

            // Start With Assets
            IEnumerable<Asset> assets = meter.MeterAssets
                .Select(meterLine => meterLine.Asset)
                .OrderBy(asset => asset.ID);

            //Save individual Assets (Except LineSegments)
            List<LineSegment> lineSegments = new List<LineSegment>();
            List<AssetConnection> assetConnections = new List<AssetConnection>();

            foreach (Asset asset in assets)
            {
                XElement assetElement = new XElement("asset"); ;
               
                switch (asset.AssetTypeID)
                {
                    case ((int)AssetType.Breaker):
                        assetElement = new XElement("breaker");
                        Breaker breaker = Breaker.DetailedBreaker(asset, asset.ConnectionFactory?.Invoke());
                        assetElement.Add(new XElement(nameof(breaker.VoltageKV), breaker.VoltageKV));
                        assetElement.Add(new XElement(nameof(breaker.ThermalRating), breaker.ThermalRating));
                        assetElement.Add(new XElement(nameof(breaker.Speed), breaker.Speed));
                        assetElement.Add(new XElement(nameof(asset.ID), asset.ID));
                        assetElement.Add(new XElement(nameof(asset.AssetKey), asset.AssetKey));
                        assetElement.Add(new XElement(nameof(asset.Description), asset.Description));
                        assetElement.Add(new XElement(nameof(asset.AssetName), asset.AssetName));
                        break;

                    case ((int)AssetType.Bus):
                        assetElement = new XElement("bus");
                        Bus bus = Bus.DetailedBus(asset, asset.ConnectionFactory?.Invoke());
                        assetElement.Add(new XElement(nameof(bus.VoltageKV), bus.VoltageKV));
                        assetElement.Add(new XElement(nameof(asset.ID), asset.ID));
                        assetElement.Add(new XElement(nameof(asset.AssetKey), asset.AssetKey));
                        assetElement.Add(new XElement(nameof(asset.Description), asset.Description));
                        assetElement.Add(new XElement(nameof(asset.AssetName), asset.AssetName));
                        break;
                    case ((int)AssetType.CapacitorBank):
                        assetElement = new XElement("capacitorBank");
                        CapBank capBank = CapBank.DetailedCapBank(asset, asset.ConnectionFactory?.Invoke());
                        assetElement.Add(new XElement(nameof(capBank.VoltageKV), capBank.VoltageKV));
                        assetElement.Add(new XElement(nameof(capBank.NumberOfBanks), capBank.NumberOfBanks));
                        assetElement.Add(new XElement(nameof(capBank.CansPerBank), capBank.CansPerBank));
                        assetElement.Add(new XElement(nameof(capBank.CapacitancePerBank), capBank.CapacitancePerBank));
                        assetElement.Add(new XElement(nameof(asset.ID), asset.ID));
                        assetElement.Add(new XElement(nameof(asset.AssetKey), asset.AssetKey));
                        assetElement.Add(new XElement(nameof(asset.Description), asset.Description));
                        assetElement.Add(new XElement(nameof(asset.AssetName), asset.AssetName));
                        break;
                    case ((int)AssetType.Transformer):
                        assetElement = new XElement("transformer");
                        Transformer xfr = Transformer.DetailedTransformer(asset, asset.ConnectionFactory?.Invoke());
                        assetElement.Add(new XElement(nameof(xfr.ThermalRating), xfr.ThermalRating));
                        assetElement.Add(new XElement(nameof(xfr.SecondaryVoltageKV), xfr.SecondaryVoltageKV));
                        assetElement.Add(new XElement(nameof(xfr.PrimaryVoltageKV), xfr.PrimaryVoltageKV));
                        assetElement.Add(new XElement(nameof(xfr.Tap), xfr.Tap));
                        assetElement.Add(new XElement(nameof(xfr.X0), xfr.X0));
                        assetElement.Add(new XElement(nameof(xfr.R0), xfr.R0));
                        assetElement.Add(new XElement(nameof(xfr.X1), xfr.X1));
                        assetElement.Add(new XElement(nameof(xfr.R1), xfr.R1));
                        assetElement.Add(new XElement(nameof(asset.ID), asset.ID));
                        assetElement.Add(new XElement(nameof(asset.AssetKey), asset.AssetKey));
                        assetElement.Add(new XElement(nameof(asset.Description), asset.Description));
                        assetElement.Add(new XElement(nameof(asset.AssetName), asset.AssetName));
                        break;
                    case ((int)AssetType.Line):
                        assetElement = new XElement("line");
                        Line line = Line.DetailedLine(asset);
                        assetElement.Add(new XElement(nameof(line.VoltageKV), line.VoltageKV));
                        assetElement.Add(new XElement(nameof(line.MaxFaultDistance), line.MaxFaultDistance));
                        assetElement.Add(new XElement(nameof(line.MinFaultDistance), line.MinFaultDistance));
                        assetElement.Add(new XElement(nameof(asset.ID), asset.ID));
                        assetElement.Add(new XElement(nameof(asset.AssetKey), asset.AssetKey));
                        assetElement.Add(new XElement(nameof(asset.Description), asset.Description));
                        assetElement.Add(new XElement(nameof(asset.AssetName), asset.AssetName));
                        lineSegments.AddRange(line.Segments);
                        break;
                    case ((int)AssetType.LineSegement):
                        lineSegments.Add(LineSegment.DetailedLineSegment(asset, asset.ConnectionFactory?.Invoke()));
                        break;
                    default:
                        assetElement = new XElement("asset");
                        assetElement.Add(new XElement(nameof(asset.ID), asset.ID));
                        assetElement.Add(new XElement(nameof(asset.AssetKey), asset.AssetKey));
                        assetElement.Add(new XElement(nameof(asset.Description), asset.Description));
                        assetElement.Add(new XElement(nameof(asset.AssetName), asset.AssetName));
                        break;
                }

                assetConnections.AddRange(asset.Connections.Where(item => item.ParentID == asset.ID));
                rootElement.Add(assetElement);
            }

            //Deal with special Cases... If it is a line we need to keep the Line Segments...
            foreach (LineSegment lineSegment in lineSegments)
            {
                XElement lineSegmentElement = new XElement("lineSegment");
                lineSegmentElement.Add(new XElement(nameof(lineSegment.ThermalRating), lineSegment.ThermalRating));
                lineSegmentElement.Add(new XElement(nameof(lineSegment.X0), lineSegment.X0));
                lineSegmentElement.Add(new XElement(nameof(lineSegment.R0), lineSegment.R0));
                lineSegmentElement.Add(new XElement(nameof(lineSegment.X1), lineSegment.X1));
                lineSegmentElement.Add(new XElement(nameof(lineSegment.R1), lineSegment.R1));
                lineSegmentElement.Add(new XElement(nameof(lineSegment.ID), lineSegment.ID));
                lineSegmentElement.Add(new XElement(nameof(lineSegment.Length), lineSegment.Length));
                lineSegmentElement.Add(new XElement(nameof(lineSegment.AssetKey), lineSegment.AssetKey));
                lineSegmentElement.Add(new XElement(nameof(lineSegment.Description), lineSegment.Description));
                lineSegmentElement.Add(new XElement(nameof(lineSegment.AssetName), lineSegment.AssetName));
                assetConnections.AddRange(lineSegment.Connections.Where(item => item.ParentID == lineSegment.ID));
                rootElement.Add(lineSegmentElement);
            }

            //Deal with Asset -> Asset Connections.....
            foreach (AssetConnection assetConnection in assetConnections)
            {
                if (assets.Select(item => item.ID).Contains(assetConnection.ChildID)|| lineSegments.Select(item => item.ID).Contains(assetConnection.ChildID))
                {
                    XElement assetConnectionElement = new XElement("assetConnection");
                    assetConnectionElement.Add(new XElement(nameof(assetConnection.ID), assetConnection.ID));
                    assetConnectionElement.Add(new XElement(nameof(assetConnection.ChildID), assetConnection.ChildID));
                    assetConnectionElement.Add(new XElement(nameof(assetConnection.ParentID), assetConnection.ParentID));
                    assetConnectionElement.Add(new XElement(nameof(assetConnection.AssetRelationshipTypeID), assetConnection.AssetRelationshipTypeID));
                    rootElement.Add(assetConnectionElement);
                }
            }

            // That is it for Assets 

            IEnumerable<Channel> channels = meter.Channels
                .OrderBy(channel => channel.ID);

            foreach (Channel channel in channels)
            {
                if (channel.ID == 0)
                    continue;

                XElement channelElement = new XElement("channel");
                channelElement.Add(new XElement(nameof(channel.ID), channel.ID));
                channelElement.Add(new XElement(nameof(channel.MeterID), channel.MeterID));
                channelElement.Add(new XElement(nameof(channel.AssetID), channel.AssetID));
                channelElement.Add(new XElement(nameof(channel.MeasurementTypeID), channel.MeasurementTypeID));
                channelElement.Add(new XElement(nameof(channel.MeasurementType), channel.MeasurementType.Name));
                channelElement.Add(new XElement(nameof(channel.MeasurementCharacteristicID), channel.MeasurementCharacteristicID));
                channelElement.Add(new XElement(nameof(channel.MeasurementCharacteristic), channel.MeasurementCharacteristic.Name));
                channelElement.Add(new XElement(nameof(channel.PhaseID), channel.PhaseID));
                channelElement.Add(new XElement(nameof(channel.Phase), channel.Phase.Name));
                channelElement.Add(new XElement(nameof(channel.Name), channel.Name));
                channelElement.Add(new XElement(nameof(channel.SamplesPerHour), channel.SamplesPerHour));
                channelElement.Add(new XElement(nameof(channel.PerUnitValue), channel.PerUnitValue));
                channelElement.Add(new XElement(nameof(channel.HarmonicGroup), channel.HarmonicGroup));
                channelElement.Add(new XElement(nameof(channel.Description), channel.Description));
                channelElement.Add(new XElement(nameof(channel.Enabled), channel.Enabled));
                rootElement.Add(channelElement);
            }

            IEnumerable<Series> seriesList = meter.Channels
                .SelectMany(channel => channel.Series)
                .OrderBy(series => series.ID);

            foreach (Series series in seriesList)
            {
                if (series.ID == 0)
                    continue;

                XElement seriesElement = new XElement("series");
                seriesElement.Add(new XElement(nameof(series.ID), series.ID));
                seriesElement.Add(new XElement(nameof(series.ChannelID), series.ChannelID));
                seriesElement.Add(new XElement(nameof(series.SeriesTypeID), series.SeriesTypeID));
                seriesElement.Add(new XElement(nameof(series.SeriesType), series.SeriesType.Name));
                seriesElement.Add(new XElement(nameof(series.SourceIndexes), series.SourceIndexes));
                rootElement.Add(seriesElement);
            }

            IEnumerable<AssetLocation> assetLocations = meter.Location.AssetLocations
                .OrderBy(assetLocation => assetLocation.ID);

            foreach (AssetLocation assetLocation in assetLocations)
            {
                XElement meterLocationLineElement = new XElement("assetLocation");
                meterLocationLineElement.Add(new XElement(nameof(assetLocation.ID), assetLocation.ID));
                meterLocationLineElement.Add(new XElement(nameof(assetLocation.LocationID), assetLocation.LocationID));
                meterLocationLineElement.Add(new XElement(nameof(assetLocation.AssetID), assetLocation.AssetID));
                rootElement.Add(meterLocationLineElement);
            }

            IEnumerable<MeterAsset> meterAssets = meter.MeterAssets
                .OrderBy(meterAsset => meterAsset.ID);

            foreach (MeterAsset meterAsset in meterAssets)
            {
                XElement meterLineElement = new XElement("meterAsset");
                meterLineElement.Add(new XElement(nameof(meterAsset.ID), meterAsset.ID));
                meterLineElement.Add(new XElement(nameof(meterAsset.MeterID), meterAsset.MeterID));
                meterLineElement.Add(new XElement(nameof(meterAsset.AssetID), meterAsset.AssetID));
                rootElement.Add(meterLineElement);
            }

            return sheet;
        }

        private Meter ToMeter(XDocument sheet)
        {
            double? ToNullableDouble(string str)
            {
                if (string.IsNullOrEmpty(str))
                    return null;

                return Convert.ToDouble(str);
            }

            XElement rootElement = sheet.Root;

            Meter meter = new Meter()
            {
                MeterAssets = new List<MeterAsset>(),
                Channels = new List<Channel>()
            };

            XElement meterElement = rootElement.Element("meter");
            meter.ID = Convert.ToInt32((string)meterElement.Element(nameof(meter.ID)));
            meter.AssetKey = (string)meterElement.Element(nameof(meter.AssetKey));
            meter.LocationID = Convert.ToInt32((string)meterElement.Element(nameof(meter.LocationID)));
            meter.Name = (string)meterElement.Element(nameof(meter.Name));
            meter.Alias = (string)meterElement.Element(nameof(meter.Alias));
            meter.ShortName = (string)meterElement.Element(nameof(meter.ShortName));
            meter.Make = (string)meterElement.Element(nameof(meter.Make));
            meter.Model = (string)meterElement.Element(nameof(meter.Model));
            meter.TimeZone = (string)meterElement.Element(nameof(meter.TimeZone));
            meter.Description = (string)meterElement.Element(nameof(meter.Description));

            Location location = new Location()
            {
                Meters = new List<Meter>(),
                AssetLocations = new List<AssetLocation>()
            };

            XElement meterLocationElement = rootElement.Element("location");
            location.ID = Convert.ToInt32((string)meterLocationElement.Element(nameof(location.ID)));
            location.LocationKey = (string)meterLocationElement.Element(nameof(location.LocationKey));
            location.Name = (string)meterLocationElement.Element(nameof(location.Name));
            location.Alias = (string)meterLocationElement.Element(nameof(location.Alias));
            location.ShortName = (string)meterLocationElement.Element(nameof(location.ShortName));
            location.Latitude = Convert.ToDouble((string)meterLocationElement.Element(nameof(location.Latitude)));
            location.Longitude = Convert.ToDouble((string)meterLocationElement.Element(nameof(location.Longitude)));
            location.Description = (string)meterLocationElement.Element(nameof(location.Description));
            location.Meters.Add(meter);
            meter.Location = location;

            // This needs to be adjusted to work for Assets of all Types 
            Dictionary<int, Asset> assets = new Dictionary<int, Asset>();

            foreach (XElement breakerElement in rootElement.Elements("breaker"))
            {

                Breaker breaker = new Breaker()
                {
                    AssetLocations = new List<AssetLocation>(),
                    MeterAssets = new List<MeterAsset>(),
                    DirectChannels = new List<Channel>(),
                    Connections = new List<AssetConnection>(),
                    AssetTypeID = (int)AssetType.Breaker
                };
                breaker.VoltageKV = Convert.ToDouble((string)breakerElement.Element(nameof(breaker.VoltageKV)));
                breaker.ThermalRating = Convert.ToDouble((string)breakerElement.Element(nameof(breaker.ThermalRating)));
                breaker.Speed = Convert.ToDouble((string)breakerElement.Element(nameof(breaker.Speed)));
                breaker.ID = Convert.ToInt32((string)breakerElement.Element(nameof(breaker.ID)));
                breaker.AssetKey = (string)breakerElement.Element(nameof(breaker.AssetKey));
                breaker.Description = (string)breakerElement.Element(nameof(breaker.Description));
                breaker.AssetName = (string)breakerElement.Element(nameof(breaker.AssetName));
                assets.Add(breaker.ID, breaker);
            }

            foreach (XElement busElement in rootElement.Elements("bus"))
            {

                Bus bus = new Bus()
                {
                    AssetLocations = new List<AssetLocation>(),
                    MeterAssets = new List<MeterAsset>(),
                    DirectChannels = new List<Channel>(),
                    Connections = new List<AssetConnection>(),
                    AssetTypeID = (int)AssetType.Bus
                };
                bus.VoltageKV = Convert.ToDouble((string)busElement.Element(nameof(bus.VoltageKV)));
                bus.ID = Convert.ToInt32((string)busElement.Element(nameof(bus.ID)));
                bus.AssetKey = (string)busElement.Element(nameof(bus.AssetKey));
                bus.Description = (string)busElement.Element(nameof(bus.Description));
                bus.AssetName = (string)busElement.Element(nameof(bus.AssetName));

                assets.Add(bus.ID, bus);
            }

            foreach (XElement capacitorBankElement in rootElement.Elements("capacitorBank"))
            {

                CapBank capBank = new CapBank()
                {
                    AssetLocations = new List<AssetLocation>(),
                    MeterAssets = new List<MeterAsset>(),
                    DirectChannels = new List<Channel>(),
                    Connections = new List<AssetConnection>(),
                    AssetTypeID = (int)AssetType.CapacitorBank
                };
                capBank.VoltageKV = Convert.ToDouble((string)capacitorBankElement.Element(nameof(capBank.VoltageKV)));
                capBank.NumberOfBanks = Convert.ToInt32((string)capacitorBankElement.Element(nameof(capBank.NumberOfBanks)));
                capBank.CansPerBank = Convert.ToInt32((string)capacitorBankElement.Element(nameof(capBank.CansPerBank)));
                capBank.CapacitancePerBank = Convert.ToDouble((string)capacitorBankElement.Element(nameof(capBank.CapacitancePerBank)));

                capBank.ID = Convert.ToInt32((string)capacitorBankElement.Element(nameof(capBank.ID)));
                capBank.AssetKey = (string)capacitorBankElement.Element(nameof(capBank.AssetKey));
                capBank.Description = (string)capacitorBankElement.Element(nameof(capBank.Description));
                capBank.AssetName = (string)capacitorBankElement.Element(nameof(capBank.AssetName));

                assets.Add(capBank.ID, capBank);
            }

            foreach (XElement xfrElement in rootElement.Elements("transformer"))
            {

                Transformer xfr = new Transformer()
                {
                    AssetLocations = new List<AssetLocation>(),
                    MeterAssets = new List<MeterAsset>(),
                    DirectChannels = new List<Channel>(),
                    Connections = new List<AssetConnection>(),
                    AssetTypeID = (int)AssetType.Transformer
                };
                xfr.ThermalRating = Convert.ToDouble((string)xfrElement.Element(nameof(xfr.ThermalRating)));
                xfr.SecondaryVoltageKV = Convert.ToDouble((string)xfrElement.Element(nameof(xfr.SecondaryVoltageKV)));
                xfr.PrimaryVoltageKV = Convert.ToDouble((string)xfrElement.Element(nameof(xfr.PrimaryVoltageKV)));
                xfr.Tap = Convert.ToDouble((string)xfrElement.Element(nameof(xfr.Tap)));
                xfr.R0 = Convert.ToDouble((string)xfrElement.Element(nameof(xfr.R0)));
                xfr.X0 = Convert.ToDouble((string)xfrElement.Element(nameof(xfr.X0)));
                xfr.R1 = Convert.ToDouble((string)xfrElement.Element(nameof(xfr.R1)));
                xfr.X1 = Convert.ToDouble((string)xfrElement.Element(nameof(xfr.X1)));

                xfr.ID = Convert.ToInt32((string)xfrElement.Element(nameof(xfr.ID)));
                xfr.AssetKey = (string)xfrElement.Element(nameof(xfr.AssetKey));
                xfr.Description = (string)xfrElement.Element(nameof(xfr.Description));
                xfr.AssetName = (string)xfrElement.Element(nameof(xfr.AssetName));

                assets.Add(xfr.ID, xfr);
            }

            foreach (XElement lineElement in rootElement.Elements("line"))
            {

                Line line = new Line()
                {
                    AssetLocations = new List<AssetLocation>(),
                    MeterAssets = new List<MeterAsset>(),
                    DirectChannels = new List<Channel>(),
                    Connections = new List<AssetConnection>(),
                    AssetTypeID = (int)AssetType.Line,
                    Segments = new List<LineSegment>()
                };
                line.VoltageKV = Convert.ToDouble((string)lineElement.Element(nameof(line.VoltageKV)));
                line.MaxFaultDistance = Convert.ToInt32((string)lineElement.Element(nameof(line.MaxFaultDistance)));
                line.MinFaultDistance = Convert.ToInt32((string)lineElement.Element(nameof(line.MinFaultDistance)));

                line.ID = Convert.ToInt32((string)lineElement.Element(nameof(line.ID)));
                line.AssetKey = (string)lineElement.Element(nameof(line.AssetKey));
                line.Description = (string)lineElement.Element(nameof(line.Description));
                line.AssetName = (string)lineElement.Element(nameof(line.AssetName));

                assets.Add(line.ID, line);
            }

            foreach (XElement lineSegmentElement in rootElement.Elements("lineSegment"))
            {

                LineSegment lineSegment = new LineSegment()
                {
                    AssetLocations = new List<AssetLocation>(),
                    MeterAssets = new List<MeterAsset>(),
                    DirectChannels = new List<Channel>(),
                    Connections = new List<AssetConnection>(),
                    AssetTypeID = (int)AssetType.LineSegement
                };
                lineSegment.ThermalRating = Convert.ToDouble((string)lineSegmentElement.Element(nameof(lineSegment.ThermalRating)));
                lineSegment.R0 = Convert.ToDouble((string)lineSegmentElement.Element(nameof(lineSegment.R0)));
                lineSegment.X0 = Convert.ToDouble((string)lineSegmentElement.Element(nameof(lineSegment.X0)));
                lineSegment.R1 = Convert.ToDouble((string)lineSegmentElement.Element(nameof(lineSegment.R1)));
                lineSegment.X1 = Convert.ToDouble((string)lineSegmentElement.Element(nameof(lineSegment.X1)));
                lineSegment.Length = Convert.ToDouble((string)lineSegmentElement.Element(nameof(lineSegment.Length)));

                lineSegment.ID = Convert.ToInt32((string)lineSegmentElement.Element(nameof(lineSegment.ID)));
                lineSegment.AssetKey = (string)lineSegmentElement.Element(nameof(lineSegment.AssetKey));
                lineSegment.Description = (string)lineSegmentElement.Element(nameof(lineSegment.Description));
                lineSegment.AssetName = (string)lineSegmentElement.Element(nameof(lineSegment.AssetName));

                assets.Add(lineSegment.ID, lineSegment);
            }
            foreach (XElement assetConnectionElement in rootElement.Elements("assetConnection"))
            {
                AssetConnection assetConnection = new AssetConnection();

                assetConnection.ID = Convert.ToInt32((string)assetConnectionElement.Element(nameof(assetConnection.ID)));
                assetConnection.ChildID = Convert.ToInt32((string)assetConnectionElement.Element(nameof(assetConnection.ChildID)));
                assetConnection.ParentID = Convert.ToInt32((string)assetConnectionElement.Element(nameof(assetConnection.ParentID)));
                assetConnection.AssetRelationshipTypeID = Convert.ToInt32((string)assetConnectionElement.Element(nameof(assetConnection.AssetRelationshipTypeID)));
                
                assets[assetConnection.ChildID].Connections.Add(assetConnection);
                assets[assetConnection.ParentID].Connections.Add(assetConnection);

                if (assets[assetConnection.ChildID].AssetTypeID == (int)AssetType.Line && assets[assetConnection.ParentID].AssetTypeID == (int)AssetType.LineSegement)
                {
                    Line line = (Line)assets[assetConnection.ChildID];
                    LineSegment lineSegment = (LineSegment)assets[assetConnection.ParentID];
                    line.Segments.Add(lineSegment);
                    lineSegment.Line = line;
                    assets[assetConnection.ChildID] = line;
                    assets[assetConnection.ParentID] = lineSegment;
                }
                else if (assets[assetConnection.ParentID].AssetTypeID == (int)AssetType.Line && assets[assetConnection.ChildID].AssetTypeID == (int)AssetType.LineSegement)
                {
                    Line line = (Line)assets[assetConnection.ParentID];
                    LineSegment lineSegment = (LineSegment)assets[assetConnection.ChildID];
                    line.Segments.Add(lineSegment);
                    lineSegment.Line = line;
                    assets[assetConnection.ParentID] = line;
                    assets[assetConnection.ChildID] = lineSegment;
                }

                assetConnection.Child = assets[assetConnection.ChildID];
                assetConnection.Parent = assets[assetConnection.ParentID];
            }


               
            Dictionary<int, Channel> channels = new Dictionary<int, Channel>();
            Dictionary<int, MeasurementType> measurementTypes = new Dictionary<int, MeasurementType>();
            Dictionary<int, MeasurementCharacteristic> measurementCharacteristics = new Dictionary<int, MeasurementCharacteristic>();
            Dictionary<int, Phase> phases = new Dictionary<int, Phase>();

            foreach (XElement channelElement in rootElement.Elements("channel"))
            {
                Channel channel = new Channel() { Series = new List<Series>() };
                channel.ID = Convert.ToInt32((string)channelElement.Element(nameof(channel.ID)));
                channel.MeterID = Convert.ToInt32((string)channelElement.Element(nameof(channel.MeterID)));
                channel.AssetID = Convert.ToInt32((string)channelElement.Element(nameof(channel.AssetID)));
                channel.MeasurementTypeID = Convert.ToInt32((string)channelElement.Element(nameof(channel.MeasurementTypeID)));
                channel.MeasurementCharacteristicID = Convert.ToInt32((string)channelElement.Element(nameof(channel.ID)));
                channel.PhaseID = Convert.ToInt32((string)channelElement.Element(nameof(channel.PhaseID)));
                channel.Name = (string)channelElement.Element(nameof(channel.Name));
                channel.SamplesPerHour = Convert.ToDouble((string)channelElement.Element(nameof(channel.SamplesPerHour)));
                channel.PerUnitValue = ToNullableDouble((string)channelElement.Element(nameof(channel.PerUnitValue)));
                channel.HarmonicGroup = Convert.ToInt32((string)channelElement.Element(nameof(channel.HarmonicGroup)));
                channel.Description = (string)channelElement.Element(nameof(channel.Description));
                channel.Enabled = Convert.ToBoolean((string)channelElement.Element(nameof(channel.Enabled)));
                channels.Add(channel.ID, channel);

                channel.MeasurementType = measurementTypes.GetOrAdd(channel.MeasurementTypeID, id => new MeasurementType()
                {
                    ID = id,
                    Name = (string)channelElement.Element(nameof(channel.MeasurementType))
                });

                channel.MeasurementCharacteristic = measurementCharacteristics.GetOrAdd(channel.MeasurementCharacteristicID, id => new MeasurementCharacteristic()
                {
                    ID = id,
                    Name = (string)channelElement.Element(nameof(channel.MeasurementCharacteristic))
                });

                channel.Phase = phases.GetOrAdd(channel.PhaseID, id => new Phase()
                {
                    ID = id,
                    Name = (string)channelElement.Element(nameof(channel.Phase))
                });

                channel.Meter = meter;
                meter.Channels.Add(channel);

                channel.Asset = assets[channel.AssetID];
                channel.Asset.DirectChannels.Add(channel);
            }

            Dictionary<int, SeriesType> seriesTypes = new Dictionary<int, SeriesType>();

            foreach (XElement seriesElement in rootElement.Elements("series"))
            {
                Series series = new Series();
                series.ID = Convert.ToInt32((string)seriesElement.Element(nameof(series.ID)));
                series.ChannelID = Convert.ToInt32((string)seriesElement.Element(nameof(series.ChannelID)));
                series.SeriesTypeID = Convert.ToInt32((string)seriesElement.Element(nameof(series.SeriesTypeID)));
                series.SourceIndexes = (string)seriesElement.Element(nameof(series.SourceIndexes));

                series.SeriesType = seriesTypes.GetOrAdd(series.SeriesTypeID, id => new SeriesType()
                {
                    ID = id,
                    Name = (string)seriesElement.Element(nameof(series.SeriesType))
                });

                series.Channel = channels[series.ChannelID];
                series.Channel.Series.Add(series);
            }

            foreach (XElement assetLocationElement in rootElement.Elements("assetLocation"))
            {
                AssetLocation assetLocation = new AssetLocation();
                assetLocation.ID = Convert.ToInt32((string)assetLocationElement.Element(nameof(assetLocation.ID)));
                assetLocation.LocationID = Convert.ToInt32((string)assetLocationElement.Element(nameof(assetLocation.LocationID)));
                assetLocation.AssetID = Convert.ToInt32((string)assetLocationElement.Element(nameof(assetLocation.AssetID)));

                assetLocation.Location = location;
                location.AssetLocations.Add(assetLocation);

                assetLocation.Asset = assets[assetLocation.AssetID];
                assetLocation.Asset.AssetLocations.Add(assetLocation);
            }

            foreach (XElement meterAssetElement in rootElement.Elements("meterAsset"))
            {
                MeterAsset meterAsset= new MeterAsset();
                meterAsset.ID = Convert.ToInt32((string)meterAssetElement.Element(nameof(meterAsset.ID)));
                meterAsset.MeterID = Convert.ToInt32((string)meterAssetElement.Element(nameof(meterAsset.MeterID)));
                meterAsset.AssetID = Convert.ToInt32((string)meterAssetElement.Element(nameof(meterAsset.AssetID)));

                meterAsset.Meter = meter;
                meter.MeterAssets.Add(meterAsset);

                meterAsset.Asset = assets[meterAsset.AssetID];
                meterAsset.Asset.MeterAssets.Add(meterAsset);
            }

            return meter;
        }
    }
}
