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
using System.IO;
using System.Linq;
using System.Xml.Linq;
using FaultData.Database;

namespace DeviceDefinitionsMigrator
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = args[0];
            string deviceDefinitionsFile = args[1];

            Migrate(connectionString, deviceDefinitionsFile);
        }

        private static void Migrate(string connectionString, string deviceDefinitionsFile)
        {
            MeterInfoDataContext meterInfo = new MeterInfoDataContext(connectionString);
            Meter meter;
            MeterLocation endMeterLocation;

            List<Tuple<XElement, Line>> lineMappings = new List<Tuple<XElement, Line>>();
            FaultLocationInfoDataContext faultLocationInfo = new FaultLocationInfoDataContext(connectionString);
            Impedance impedance;

            FaultLocationAlgorithm algorithm;

            XElement root = GetRoot(deviceDefinitionsFile);
            XElement analyticsElement = root.Element("analytics") ?? new XElement("analytics");

            foreach (XElement faultLocationElement in analyticsElement.Elements("faultLocation"))
            {
                int index;
                string algorithmName;

                algorithmName = (string)faultLocationElement.Attribute("method");
                index = algorithmName.LastIndexOf('.');

                algorithm = new FaultLocationAlgorithm();
                algorithm.AssemblyName = (string)faultLocationElement.Attribute("assembly");
                algorithm.TypeName = algorithmName.Substring(0, index);
                algorithm.MethodName = algorithmName.Substring(index + 1);
                faultLocationInfo.FaultLocationAlgorithms.InsertOnSubmit(algorithm);
            }

            int i = 0;
            List<XElement> deviceElements = root.Elements("device").ToList();
            WriteProgressMessage(string.Format("Beginning migration of {0} device elements...", deviceElements.Count), i, deviceElements.Count);

            foreach (XElement deviceElement in deviceElements)
            {
                XElement deviceAttributes = deviceElement.Element("attributes") ?? new XElement("attributes");
                XElement linesElement = deviceElement.Element("lines") ?? new XElement("lines");

                meter = new Meter();

                meter.AssetKey = (string)deviceElement.Attribute("id");
                meter.Name = (string)deviceAttributes.Element("stationName");
                meter.Make = (string)deviceAttributes.Element("make");
                meter.Model = (string)deviceAttributes.Element("model");
                meter.ShortName = new string(meter.Name.Take(50).ToArray());
                meter.MeterLocation = GetMeterLocation(meterInfo, (string)deviceAttributes.Element("stationID"), meter.Name);

                WriteProgressMessage(string.Format("Loading meter {0} ({1})...", meter.Name, meter.AssetKey), i, deviceElements.Count);

                foreach (XElement lineElement in linesElement.Elements("line"))
                {
                    Line line = GetLine(meterInfo, lineElement);

                    WriteProgressMessage(string.Format("Loading line {0} ({1}-{2})...", line.AssetKey, meter.MeterLocation.Name, (string)lineElement.Element("endStationName")), i, deviceElements.Count);

                    if (line.ID == 0)
                    {
                        endMeterLocation = GetMeterLocation(meterInfo, (string)lineElement.Element("endStationID"), (string)lineElement.Element("endStationName"));

                        meterInfo.MeterLocationLines.InsertOnSubmit(new MeterLocationLine()
                        {
                            Line = line,
                            MeterLocation = meter.MeterLocation
                        });

                        meterInfo.MeterLocationLines.InsertOnSubmit(new MeterLocationLine()
                        {
                            Line = line,
                            MeterLocation = endMeterLocation
                        });
                    }

                    XElement channelsElement = lineElement.Element("channels") ?? new XElement("channels");
                    AddChannel(meter, line, meterInfo, "Voltage", "AN", channelsElement.Element("VA"));
                    AddChannel(meter, line, meterInfo, "Voltage", "BN", channelsElement.Element("VB"));
                    AddChannel(meter, line, meterInfo, "Voltage", "CN", channelsElement.Element("VC"));
                    AddChannel(meter, line, meterInfo, "Current", "AN", channelsElement.Element("IA"));
                    AddChannel(meter, line, meterInfo, "Current", "BN", channelsElement.Element("IB"));
                    AddChannel(meter, line, meterInfo, "Current", "CN", channelsElement.Element("IC"));

                    lineMappings.Add(Tuple.Create(lineElement, line));
                }

                meterInfo.Meters.InsertOnSubmit(meter);
                meterInfo.SubmitChanges();

                foreach (Tuple<XElement, Line> mapping in lineMappings)
                {
                    XElement lineElement = mapping.Item1;
                    XElement impedanceElement = lineElement.Element("impedances") ?? new XElement("impedances");

                    impedance = new Impedance();
                    impedance.LineID = mapping.Item2.ID;
                    impedance.R0 = Convert.ToDouble((string)impedanceElement.Element("R0"));
                    impedance.X0 = Convert.ToDouble((string)impedanceElement.Element("X0"));
                    impedance.R1 = Convert.ToDouble((string)impedanceElement.Element("R1"));
                    impedance.X1 = Convert.ToDouble((string)impedanceElement.Element("X1"));
                    faultLocationInfo.Impedances.InsertOnSubmit(impedance);

                    foreach (XElement faultLocationElement in lineElement.Elements("faultLocation"))
                    {
                        int index;
                        string algorithmName;

                        algorithmName = (string)faultLocationElement.Attribute("method");
                        index = algorithmName.LastIndexOf('.');

                        algorithm = new FaultLocationAlgorithm();
                        algorithm.LineID = mapping.Item2.ID;
                        algorithm.AssemblyName = (string)faultLocationElement.Attribute("assembly");
                        algorithm.TypeName = algorithmName.Substring(0, index);
                        algorithm.MethodName = algorithmName.Substring(index + 1);
                        faultLocationInfo.FaultLocationAlgorithms.InsertOnSubmit(algorithm);
                    }
                }

                faultLocationInfo.SubmitChanges();

                WriteProgressMessage(string.Format("Committing data for meter {0} to the database...", meter.Name), i, deviceElements.Count);

                i++;
            }
        }

        private static XElement GetRoot(string deviceDefinitionsFile)
        {
            XDocument deviceDefinitionsDocument;
            XElement root;

            if (!File.Exists(deviceDefinitionsFile))
                throw new FileNotFoundException(string.Format("Device definitions file \"{0}\" not found.", deviceDefinitionsFile));

            deviceDefinitionsDocument = XDocument.Load(deviceDefinitionsFile);
            root = deviceDefinitionsDocument.Root ?? new XElement("openFLE");

            return root;
        }

        private static MeterLocation GetMeterLocation(MeterInfoDataContext meterInfo, string assetKey, string name)
        {
            MeterLocation meterLocation;

            meterLocation = meterInfo.GetChangeSet().Inserts.OfType<MeterLocation>().FirstOrDefault(location => location.AssetKey == assetKey);

            if ((object)meterLocation == null)
                meterLocation = meterInfo.MeterLocations.FirstOrDefault(location => location.AssetKey == assetKey);

            if ((object)meterLocation == null)
            {
                meterLocation = new MeterLocation();
                meterLocation.AssetKey = assetKey;
                meterLocation.Name = name;
            }

            return meterLocation;
        }

        private static Line GetLine(MeterInfoDataContext meterInfo, XElement lineElement)
        {
            Line line;
            string assetKey = (string)lineElement.Attribute("id");

            line = meterInfo.GetChangeSet().Inserts.OfType<Line>().FirstOrDefault(dbLine => dbLine.AssetKey == assetKey);

            if ((object)line == null)
                line = meterInfo.Lines.FirstOrDefault(dbLine => dbLine.AssetKey == assetKey);

            if ((object)line == null)
            {
                line = new Line();
                line.AssetKey = (string)lineElement.Attribute("id");
                line.Name = (string)lineElement.Element("name");
                line.ShortName = new string(line.Name.Take(50).ToArray());
                line.Length = Convert.ToDouble((string)lineElement.Element("length"));
                line.ThermalRating = Convert.ToDouble((string)lineElement.Element("rating50F"));
                line.VoltageKV = Convert.ToDouble((string)lineElement.Element("voltage"));
            }

            return line;
        }

        private static void AddChannel(Meter meter, Line line, MeterInfoDataContext meterInfo, string measurementTypeName, string phaseName, XElement channelElement)
        {
            Channel channel;
            Series series;

            channel = new Channel();
            channel.MeasurementType = GetMeasurementType(meterInfo, measurementTypeName);
            channel.MeasurementCharacteristic = GetMeasurementCharacteristic(meterInfo, "Instantaneous");
            channel.Name = string.Format("{0} {1}", line.Name, GetShortMeasurementTypeName(measurementTypeName, phaseName));
            channel.Phase = GetPhase(meterInfo, phaseName);
            channel.HarmonicGroup = 0;

            channel.Meter = meter;
            channel.Line = line;

            series = new Series();
            series.SeriesType = GetSeriesType(meterInfo, "Values");
            series.Channel = channel;
            series.SourceIndexes = channelElement.Value;
        }

        private static MeasurementType GetMeasurementType(MeterInfoDataContext meterInfo, string name)
        {
            MeasurementType measurementType;

            measurementType = meterInfo.GetChangeSet().Inserts.OfType<MeasurementType>().FirstOrDefault(type => type.Name == name);

            if ((object)measurementType == null)
                measurementType = meterInfo.MeasurementTypes.FirstOrDefault(type => type.Name == name);

            if ((object)measurementType == null)
            {
                measurementType = new MeasurementType();
                measurementType.Name = name;
                measurementType.Description = name;
            }

            return measurementType;
        }

        private static MeasurementCharacteristic GetMeasurementCharacteristic(MeterInfoDataContext meterInfo, string name)
        {
            MeasurementCharacteristic measurementCharacteristic;

            measurementCharacteristic = meterInfo.GetChangeSet().Inserts.OfType<MeasurementCharacteristic>().FirstOrDefault(characteristic => characteristic.Name == name);

            if ((object)measurementCharacteristic == null)
                measurementCharacteristic = meterInfo.MeasurementCharacteristics.FirstOrDefault(characteristic => characteristic.Name == name);

            if ((object)measurementCharacteristic == null)
            {
                measurementCharacteristic = new MeasurementCharacteristic();
                measurementCharacteristic.Name = name;
                measurementCharacteristic.Description = name;
            }

            return measurementCharacteristic;
        }

        private static Phase GetPhase(MeterInfoDataContext meterInfo, string name)
        {
            Phase phase;

            phase = meterInfo.GetChangeSet().Inserts.OfType<Phase>().FirstOrDefault(type => type.Name == name);

            if ((object)phase == null)
                phase = meterInfo.Phases.FirstOrDefault(type => type.Name == name);

            if ((object)phase == null)
            {
                phase = new Phase();
                phase.Name = name;
                phase.Description = name;
            }

            return phase;
        }

        private static SeriesType GetSeriesType(MeterInfoDataContext meterInfo, string name)
        {
            SeriesType seriesType;

            seriesType = meterInfo.GetChangeSet().Inserts.OfType<SeriesType>().FirstOrDefault(type => type.Name == name);

            if ((object)seriesType == null)
                seriesType = meterInfo.SeriesTypes.FirstOrDefault(type => type.Name == name);

            if ((object)seriesType == null)
            {
                seriesType = new SeriesType();
                seriesType.Name = name;
                seriesType.Description = name;
            }

            return seriesType;
        }

        private static string GetShortMeasurementTypeName(string measurementTypeName, string phaseName)
        {
            if (measurementTypeName == "Current")
                measurementTypeName = "I";

            return string.Format("{0}{1}", measurementTypeName[0], phaseName[0]);
        }

        private static void WriteProgressMessage(string message, int progress, int total)
        {
            Console.WriteLine("[{0:0.00%}] {1}", progress / (double)total, message);
        }
    }
}
