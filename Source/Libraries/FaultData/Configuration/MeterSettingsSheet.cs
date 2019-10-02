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

using GSF.Collections;
using GSF.Data.Model;
using GSF.Text;
using openXDA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

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

        public MeterSettingsSheet(TableOperations<MeterConfiguration> meterConfigurationTable, MeterConfiguration configuration)
        {
            DiffMatchPatch patchProvider = new DiffMatchPatch();

            string ToSheetText(MeterConfiguration config)
            {
                if (config.DiffID == null)
                    return config.ConfigText;

                MeterConfiguration configToPatch = meterConfigurationTable.QueryRecordWhere("ID = {0}", config.DiffID);
                List<Patch> patches = patchProvider.PatchFromText(config.ConfigText);
                string sheetToPatch = ToSheetText(configToPatch);
                return (string)patchProvider.PatchApply(patches, sheetToPatch)[0];
            }

            string sheetText = ToSheetText(configuration);
            XDocument sheet = XDocument.Parse(sheetText);
            Meter = ToMeter(sheet);
            Sheet = sheet;
        }

        public Meter Meter { get; }
        public XDocument Sheet { get; }

        public void UpdateConfiguration(TableOperations<MeterConfiguration> meterConfigurationTable, string configKey)
        {
            MeterConfiguration newConfiguration = new MeterConfiguration();
            newConfiguration.MeterID = Meter.ID;
            newConfiguration.ConfigKey = configKey;
            newConfiguration.ConfigText = Sheet.ToString();

            MeterConfiguration oldConfiguration = meterConfigurationTable.QueryRecordWhere("MeterID = {0} AND ConfigKey = {1} AND DiffID IS NULL", Meter.ID, configKey);

            if (oldConfiguration == null)
            {
                meterConfigurationTable.AddNewRecord(newConfiguration);
                return;
            }

            DiffMatchPatch patchProvider = new DiffMatchPatch();
            List<Patch> patches = patchProvider.PatchMake(newConfiguration.ConfigText, oldConfiguration.ConfigText);

            if (patches.Count > 0)
            {
                meterConfigurationTable.AddNewRecord(newConfiguration);
                oldConfiguration.DiffID = meterConfigurationTable.Connection.ExecuteScalar<int>("SELECT @@IDENTITY");
                oldConfiguration.ConfigText = patchProvider.PatchToText(patches);
                meterConfigurationTable.UpdateRecord(oldConfiguration);
            }
        }

        private XDocument ToSheet(Meter meter)
        {
            XDocument sheet = new XDocument();
            XElement rootElement = new XElement("meterSettingsSheet");
            sheet.Add(rootElement);

            XElement meterElement = new XElement("meter");
            meterElement.Add(new XElement(nameof(meter.ID), meter.ID));
            meterElement.Add(new XElement(nameof(meter.AssetKey), meter.AssetKey));
            meterElement.Add(new XElement(nameof(meter.MeterLocationID), meter.MeterLocationID));
            meterElement.Add(new XElement(nameof(meter.Name), meter.Name));
            meterElement.Add(new XElement(nameof(meter.Alias), meter.Alias));
            meterElement.Add(new XElement(nameof(meter.ShortName), meter.ShortName));
            meterElement.Add(new XElement(nameof(meter.Make), meter.Make));
            meterElement.Add(new XElement(nameof(meter.Model), meter.Model));
            meterElement.Add(new XElement(nameof(meter.TimeZone), meter.TimeZone));
            meterElement.Add(new XElement(nameof(meter.Description), meter.Description));
            rootElement.Add(meterElement);

            XElement meterLocationElement = new XElement("meterLocation");
            meterLocationElement.Add(new XElement(nameof(meter.MeterLocation.ID), meter.MeterLocation.ID));
            meterLocationElement.Add(new XElement(nameof(meter.MeterLocation.AssetKey), meter.MeterLocation.AssetKey));
            meterLocationElement.Add(new XElement(nameof(meter.MeterLocation.Name), meter.MeterLocation.Name));
            meterLocationElement.Add(new XElement(nameof(meter.MeterLocation.Alias), meter.MeterLocation.Alias));
            meterLocationElement.Add(new XElement(nameof(meter.MeterLocation.ShortName), meter.MeterLocation.ShortName));
            meterLocationElement.Add(new XElement(nameof(meter.MeterLocation.Latitude), meter.MeterLocation.Latitude));
            meterLocationElement.Add(new XElement(nameof(meter.MeterLocation.Longitude), meter.MeterLocation.Longitude));
            meterLocationElement.Add(new XElement(nameof(meter.MeterLocation.Description), meter.MeterLocation.Description));
            rootElement.Add(meterLocationElement);

            IEnumerable<Line> lines = meter.MeterLines
                .Select(meterLine => meterLine.Line)
                .OrderBy(line => line.ID);

            foreach (Line line in lines)
            {
                XElement lineElement = new XElement("line");
                lineElement.Add(new XElement(nameof(line.ID), line.ID));
                lineElement.Add(new XElement(nameof(line.AssetKey), line.AssetKey));
                lineElement.Add(new XElement(nameof(line.VoltageKV), line.VoltageKV));
                lineElement.Add(new XElement(nameof(line.ThermalRating), line.ThermalRating));
                lineElement.Add(new XElement(nameof(line.Length), line.Length));
                lineElement.Add(new XElement(nameof(line.MaxFaultDistance), line.MaxFaultDistance));
                lineElement.Add(new XElement(nameof(line.MinFaultDistance), line.MinFaultDistance));
                lineElement.Add(new XElement(nameof(line.Description), line.Description));

                if (line.LineImpedance != null)
                {
                    lineElement.Add(new XElement(nameof(line.LineImpedance), line.LineImpedance.ID));
                    lineElement.Add(new XElement(nameof(line.LineImpedance.R0), line.LineImpedance.R0));
                    lineElement.Add(new XElement(nameof(line.LineImpedance.X0), line.LineImpedance.X0));
                    lineElement.Add(new XElement(nameof(line.LineImpedance.R1), line.LineImpedance.R1));
                    lineElement.Add(new XElement(nameof(line.LineImpedance.X1), line.LineImpedance.X1));
                }

                rootElement.Add(lineElement);
            }

            IEnumerable<Channel> channels = meter.Channels
                .OrderBy(channel => channel.ID);

            foreach (Channel channel in channels)
            {
                if (channel.ID == 0)
                    continue;

                XElement channelElement = new XElement("channel");
                channelElement.Add(new XElement(nameof(channel.ID), channel.ID));
                channelElement.Add(new XElement(nameof(channel.MeterID), channel.MeterID));
                channelElement.Add(new XElement(nameof(channel.LineID), channel.LineID));
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

            IEnumerable<MeterLocationLine> meterLocationLines = meter.MeterLocation.MeterLocationLines
                .OrderBy(meterLocationLine => meterLocationLine.ID);

            foreach (MeterLocationLine meterLocationLine in meterLocationLines)
            {
                XElement meterLocationLineElement = new XElement("meterLocationLine");
                meterLocationLineElement.Add(new XElement(nameof(meterLocationLine.ID), meterLocationLine.ID));
                meterLocationLineElement.Add(new XElement(nameof(meterLocationLine.MeterLocationID), meterLocationLine.MeterLocationID));
                meterLocationLineElement.Add(new XElement(nameof(meterLocationLine.LineID), meterLocationLine.LineID));
                rootElement.Add(meterLocationLineElement);
            }

            IEnumerable<MeterLine> meterLines = meter.MeterLines
                .OrderBy(meterLine => meterLine.ID);

            foreach (MeterLine meterLine in meterLines)
            {
                XElement meterLineElement = new XElement("meterLine");
                meterLineElement.Add(new XElement(nameof(meterLine.ID), meterLine.ID));
                meterLineElement.Add(new XElement(nameof(meterLine.MeterID), meterLine.MeterID));
                meterLineElement.Add(new XElement(nameof(meterLine.LineID), meterLine.LineID));
                meterLineElement.Add(new XElement(nameof(meterLine.LineName), meterLine.LineName));
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
                MeterLines = new List<MeterLine>(),
                Channels = new List<Channel>()
            };

            XElement meterElement = rootElement.Element("meter");
            meter.ID = Convert.ToInt32((string)meterElement.Element(nameof(meter.ID)));
            meter.AssetKey = (string)meterElement.Element(nameof(meter.AssetKey));
            meter.MeterLocationID = Convert.ToInt32((string)meterElement.Element(nameof(meter.MeterLocationID)));
            meter.Name = (string)meterElement.Element(nameof(meter.Name));
            meter.Alias = (string)meterElement.Element(nameof(meter.Alias));
            meter.ShortName = (string)meterElement.Element(nameof(meter.ShortName));
            meter.Make = (string)meterElement.Element(nameof(meter.Make));
            meter.Model = (string)meterElement.Element(nameof(meter.Model));
            meter.TimeZone = (string)meterElement.Element(nameof(meter.TimeZone));
            meter.Description = (string)meterElement.Element(nameof(meter.Description));

            MeterLocation meterLocation = new MeterLocation()
            {
                Meters = new List<Meter>(),
                MeterLocationLines = new List<MeterLocationLine>()
            };

            XElement meterLocationElement = rootElement.Element("meterLocation");
            meterLocation.ID = Convert.ToInt32((string)meterLocationElement.Element(nameof(meterLocation.ID)));
            meterLocation.AssetKey = (string)meterLocationElement.Element(nameof(meterLocation.AssetKey));
            meterLocation.Name = (string)meterLocationElement.Element(nameof(meterLocation.Name));
            meterLocation.Alias = (string)meterLocationElement.Element(nameof(meterLocation.Alias));
            meterLocation.ShortName = (string)meterLocationElement.Element(nameof(meterLocation.ShortName));
            meterLocation.Latitude = Convert.ToDouble((string)meterLocationElement.Element(nameof(meterLocation.Latitude)));
            meterLocation.Longitude = Convert.ToDouble((string)meterLocationElement.Element(nameof(meterLocation.Longitude)));
            meterLocation.Description = (string)meterLocationElement.Element(nameof(meterLocation.Description));
            meterLocation.Meters.Add(meter);
            meter.MeterLocation = meterLocation;

            Dictionary<int, Line> lines = new Dictionary<int, Line>();

            foreach (XElement lineElement in rootElement.Elements("line"))
            {
                Line line = new Line()
                {
                    MeterLocationLines = new List<MeterLocationLine>(),
                    MeterLines = new List<MeterLine>(),
                    Channels = new List<Channel>()
                };

                line.ID = Convert.ToInt32((string)lineElement.Element(nameof(line.ID)));
                line.AssetKey = (string)lineElement.Element(nameof(line.AssetKey));
                line.VoltageKV = Convert.ToDouble((string)lineElement.Element(nameof(line.VoltageKV)));
                line.ThermalRating = Convert.ToDouble((string)lineElement.Element(nameof(line.ThermalRating)));
                line.Length = Convert.ToDouble((string)lineElement.Element(nameof(line.Length)));
                line.MaxFaultDistance = ToNullableDouble((string)lineElement.Element(nameof(line.MaxFaultDistance)));
                line.MinFaultDistance = ToNullableDouble((string)lineElement.Element(nameof(line.MinFaultDistance)));
                line.Description = (string)lineElement.Element(nameof(line.Description));

                line.LineImpedance = new LineImpedance()
                {
                    ID = Convert.ToInt32((string)lineElement.Element(nameof(line.LineImpedance))),
                    R0 = Convert.ToDouble((string)lineElement.Element(nameof(line.LineImpedance.R0))),
                    X0 = Convert.ToDouble((string)lineElement.Element(nameof(line.LineImpedance.X0))),
                    R1 = Convert.ToDouble((string)lineElement.Element(nameof(line.LineImpedance.R1))),
                    X1 = Convert.ToDouble((string)lineElement.Element(nameof(line.LineImpedance.X1)))
                };

                lines.Add(line.ID, line);
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
                channel.LineID = Convert.ToInt32((string)channelElement.Element(nameof(channel.LineID)));
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

                channel.Line = lines[channel.LineID];
                channel.Line.Channels.Add(channel);
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

            foreach (XElement meterLocationLineElement in rootElement.Elements("meterLocationLine"))
            {
                MeterLocationLine meterLocationLine = new MeterLocationLine();
                meterLocationLine.ID = Convert.ToInt32((string)meterLocationLineElement.Element(nameof(meterLocationLine.ID)));
                meterLocationLine.MeterLocationID = Convert.ToInt32((string)meterLocationLineElement.Element(nameof(meterLocationLine.MeterLocationID)));
                meterLocationLine.LineID = Convert.ToInt32((string)meterLocationLineElement.Element(nameof(meterLocationLine.LineID)));

                meterLocationLine.MeterLocation = meterLocation;
                meterLocation.MeterLocationLines.Add(meterLocationLine);

                meterLocationLine.Line = lines[meterLocationLine.LineID];
                meterLocationLine.Line.MeterLocationLines.Add(meterLocationLine);
            }

            foreach (XElement meterLineElement in rootElement.Elements("meterLine"))
            {
                MeterLine meterLine = new MeterLine();
                meterLine.ID = Convert.ToInt32((string)meterLineElement.Element(nameof(meterLine.ID)));
                meterLine.MeterID = Convert.ToInt32((string)meterLineElement.Element(nameof(meterLine.MeterID)));
                meterLine.LineID = Convert.ToInt32((string)meterLineElement.Element(nameof(meterLine.LineID)));
                meterLine.LineName = (string)meterLineElement.Element(nameof(meterLine.LineName));

                meterLine.Meter = meter;
                meter.MeterLines.Add(meterLine);

                meterLine.Line = lines[meterLine.LineID];
                meterLine.Line.MeterLines.Add(meterLine);
            }

            return meter;
        }
    }
}
