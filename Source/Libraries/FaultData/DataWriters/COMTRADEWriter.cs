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

namespace FaultData.DataWriters
{
    public static class COMTRADEWriter
    {
        public static void Write(string connectionString, int eventID, string originalFilePath, string filePath)
        {
            MeterInfoDataContext meterInfo = new MeterInfoDataContext(connectionString);
            FaultLocationInfoDataContext faultLocationInfo = new FaultLocationInfoDataContext(connectionString);
            MeterData.EventRow eventRow;

            Meter meter;
            DataGroup waveFormData;
            FaultLocationData.FaultCurveDataTable faultCurveTable;
            List<FaultSegment> segments;

            using (EventTableAdapter eventAdapter = new EventTableAdapter())
            {
                eventAdapter.Connection.ConnectionString = connectionString;
                eventRow = eventAdapter.GetDataBy(eventID).FirstOrDefault();
            }

            if ((object)eventRow == null)
                throw new InvalidOperationException(string.Format("Event with ID {0} not found", eventID));

            meter = meterInfo.Meters.FirstOrDefault(dbMeter => dbMeter.ID == eventRow.MeterID);

            waveFormData = new DataGroup();
            waveFormData.FromData(meter, eventRow.Data);

            using (FaultCurveTableAdapter faultCurveAdapter = new FaultCurveTableAdapter())
            {
                faultCurveAdapter.Connection.ConnectionString = connectionString;
                faultCurveTable = faultCurveAdapter.GetDataBy(eventID);
            }

            segments = faultLocationInfo.FaultSegments
                .Where(segment => segment.EventID == eventID)
                .OrderBy(segment => segment.StartSample)
                .ToList();

            Write(meter, waveFormData, faultCurveTable, segments, originalFilePath, filePath);
        }

        public static void Write(Meter meter, DataGroup waveFormData, FaultLocationData.FaultCurveDataTable faultCurveTable, List<FaultSegment> segments, string originalFilePath, string filePath)
        {
            List<DataSeries> waveFormSeriesList = GetWaveFormSeriesList(waveFormData);
            DataGroup faultLocationData = GetFaultLocationData(meter, faultCurveTable);
            Schema schema = BuildSchema(meter, waveFormData, waveFormSeriesList, faultCurveTable, originalFilePath);

            string absoluteFilePath = FilePath.GetAbsolutePath(filePath);
            string directory = FilePath.GetDirectoryName(absoluteFilePath);
            string rootFileName = FilePath.GetFileNameWithoutExtension(filePath);
            string schemaFilePath = Path.Combine(directory, rootFileName + ".cfg");

            File.WriteAllText(schemaFilePath, schema.FileImage, Encoding.ASCII);

            using (FileStream fileStream = File.OpenWrite(absoluteFilePath))
            {
                FaultType faultType;
                FaultSegment currentSegment = null;
                int nextSegmentIndex = 0;
                bool[] digitals = null;

                for (int i = 0; i < waveFormData.Samples; i++)
                {
                    DateTime time = waveFormSeriesList[0].DataPoints[i].Time;

                    if ((object)currentSegment == null || i > currentSegment.EndSample)
                    {
                        if (nextSegmentIndex < segments.Count)
                        {
                            currentSegment = segments[nextSegmentIndex++];
                            faultType = GetFaultType(currentSegment.SegmentType);
                        }
                        else
                        {
                            currentSegment = new FaultSegment()
                            {
                                StartSample = i,
                                EndSample = waveFormData.Samples - 1
                            };

                            faultType = FaultType.None;
                        }

                        digitals = new bool[]
                        {
                            faultType != FaultType.None,
                            faultType == FaultType.AN,
                            faultType == FaultType.BN,
                            faultType == FaultType.CN,
                            faultType == FaultType.AB,
                            faultType == FaultType.BC,
                            faultType == FaultType.CA,
                            faultType == FaultType.ABC
                        };
                    }

                    double[] values = waveFormSeriesList
                        .Select(series => series.DataPoints[i].Value)
                        .Concat(faultLocationData.DataSeries.Select(series => series.DataPoints.Count > i ? series.DataPoints[i].Value : 0.0D))
                        .Concat(digitals.Select(Convert.ToDouble))
                        .ToArray();

                    Writer.WriteNextRecordBinary(fileStream, schema, time, values, (uint)i);
                }
            }
        }

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
                case "3-Phase Fault": return FaultType.ABC;
                default: return FaultType.None;
            }
        }

        private static Schema BuildSchema(Meter meter, DataGroup waveFormData, List<DataSeries> waveFormSeriesList, FaultLocationData.FaultCurveDataTable faultCurveTable, string originalFilePath)
        {
            Schema schema;
            ChannelMetadata channelMetadata;
            AnalogChannel originalSchemaChannel;
            AnalogChannel schemaChannel;

            string originalDirectory;
            string originalRootFileName;
            string originalSchemaFilePath;
            Schema originalSchema = null;

            List<ChannelMetadata> metadata;
            string stationName;
            string deviceID;
            Ticks dataStartTime;
            int sampleCount;
            double samplingRate;

            string absoluteOriginalFilePath = FilePath.GetAbsolutePath(originalFilePath);

            if (File.Exists(absoluteOriginalFilePath))
            {
                originalDirectory = FilePath.GetDirectoryName(absoluteOriginalFilePath);
                originalRootFileName = FilePath.GetFileNameWithoutExtension(originalFilePath);
                originalSchemaFilePath = Path.Combine(originalDirectory, originalRootFileName + ".cfg");
                originalSchema = new Schema(originalSchemaFilePath);
            }

            metadata = new List<ChannelMetadata>();
            stationName = ((object)originalSchema != null) ? originalSchema.StationName : meter.Name;
            deviceID = ((object)originalSchema != null) ? originalSchema.DeviceID : meter.AssetKey;
            dataStartTime = waveFormData.StartTime;
            sampleCount = waveFormData.Samples;
            samplingRate = waveFormData.DataSeries.First().SampleRate;

            foreach (DataSeries series in waveFormSeriesList)
            {
                channelMetadata = new ChannelMetadata();
                channelMetadata.Name = GetOriginalChannelName(originalSchema, series);
                metadata.Add(channelMetadata);
            }

            foreach (FaultLocationData.FaultCurveRow faultCurveRow in faultCurveTable)
            {
                channelMetadata = new ChannelMetadata();
                channelMetadata.Name = string.Format("Fault Location ({0} Algorithm)", faultCurveRow.Algorithm);
                metadata.Add(channelMetadata);
            }

            channelMetadata = new ChannelMetadata();
            channelMetadata.Name = "Fault Detected";
            channelMetadata.IsDigital = true;
            metadata.Add(channelMetadata);

            foreach (FaultType faultType in GetFaultTypeChannels())
            {
                channelMetadata = new ChannelMetadata();
                channelMetadata.Name = string.Format("{0} Fault Type", faultType);
                channelMetadata.IsDigital = true;
                metadata.Add(channelMetadata);
            }

            schema = Writer.CreateSchema(metadata, stationName, deviceID, dataStartTime, sampleCount, samplingRate: samplingRate);

            for (int i = 0; i < waveFormSeriesList.Count; i++)
            {
                originalSchemaChannel = GetOriginalSchemaChannel(originalSchema, waveFormSeriesList[i]);
                schemaChannel = schema.AnalogChannels[i];

                if ((object)originalSchemaChannel != null)
                {
                    schemaChannel.PhaseID = originalSchemaChannel.PhaseID;
                    schemaChannel.CircuitComponent = originalSchemaChannel.CircuitComponent;
                    schemaChannel.Units = originalSchemaChannel.Units;
                    schemaChannel.Multiplier = originalSchemaChannel.Multiplier;
                    schemaChannel.Adder = originalSchemaChannel.Adder;
                    schemaChannel.Skew = originalSchemaChannel.Skew;
                    schemaChannel.MinValue = originalSchemaChannel.MinValue;
                    schemaChannel.MaxValue = originalSchemaChannel.MaxValue;
                    schemaChannel.PrimaryRatio = originalSchemaChannel.PrimaryRatio;
                    schemaChannel.SecondaryRatio = originalSchemaChannel.SecondaryRatio;
                    schemaChannel.ScalingIdentifier = originalSchemaChannel.ScalingIdentifier;

                    if (schemaChannel.Units.ToUpper().Contains("KA") || schemaChannel.Units.ToUpper().Contains("KV"))
                        waveFormSeriesList[i] = waveFormSeriesList[i].Multiply(0.001);
                }
            }

            return schema;
        }

        private static List<DataSeries> GetWaveFormSeriesList(DataGroup waveFormData)
        {
            return GetWaveFormSeriesList(GetSeriesLookup(waveFormData));
        }

        private static List<DataSeries> GetWaveFormSeriesList(Dictionary<string, DataSeries> seriesLookup)
        {
            List<DataSeries> waveFormSeriesList = new List<DataSeries>()
            {
                seriesLookup["VA"],
                seriesLookup["VB"],
                seriesLookup["VC"],
                seriesLookup["IA"],
                seriesLookup["IB"],
                seriesLookup["IC"]
            };

            return waveFormSeriesList;
        }

        private static List<FaultType> GetFaultTypeChannels()
        {
            return new List<FaultType>()
            {
                FaultType.AN,
                FaultType.BN,
                FaultType.CN,
                FaultType.AB,
                FaultType.BC,
                FaultType.CA,
                FaultType.ABC
            };
        }

        private static Dictionary<string, DataSeries> GetSeriesLookup(DataGroup waveFormData)
        {
            string[] measurementTypes = { "Voltage", "Current" };
            string[] phases = { "AN", "BN", "CN" };

            Func<DataSeries, string> typeSelector = series =>
            {
                string measurementType = series.SeriesInfo.Channel.MeasurementType.Name;
                string phase = series.SeriesInfo.Channel.Phase.Name;

                if (measurementType == "Current")
                    measurementType = "I";

                return string.Format("{0}{1}", measurementType[0], phase[0]);
            };

            return waveFormData.DataSeries
                .Where(series => series.SeriesInfo.SeriesType.Name == "Values")
                .Where(series => series.SeriesInfo.Channel.MeasurementCharacteristic.Name == "Instantaneous")
                .Where(series => measurementTypes.Contains(series.SeriesInfo.Channel.MeasurementType.Name))
                .Where(series => phases.Contains(series.SeriesInfo.Channel.Phase.Name))
                .ToDictionary(typeSelector);
        }

        private static string GetOriginalChannelName(Schema originalSchema, DataSeries series)
        {
            int index;

            return ((object)originalSchema != null && int.TryParse(series.SeriesInfo.SourceIndexes, out index))
                ? originalSchema.AnalogChannels[Math.Abs(index) - 1].Name
                : series.SeriesInfo.Channel.Name;
        }

        private static AnalogChannel GetOriginalSchemaChannel(Schema originalSchema, DataSeries series)
        {
            int index;

            return ((object)originalSchema != null && int.TryParse(series.SeriesInfo.SourceIndexes, out index))
                ? originalSchema.AnalogChannels[Math.Abs(index) - 1]
                : null;
        }

        private static DataGroup GetFaultLocationData(Meter meter, FaultLocationData.FaultCurveDataTable faultCurveTable)
        {
            DataGroup faultLocationData = new DataGroup();
            DataGroup parsedGroup = new DataGroup();

            foreach (FaultLocationData.FaultCurveRow faultCurveRow in faultCurveTable)
            {
                parsedGroup.FromData(meter, faultCurveRow.Data);

                foreach (DataSeries series in parsedGroup.DataSeries)
                    faultLocationData.Add(series);
            }

            return faultLocationData;
        }
    }
}
