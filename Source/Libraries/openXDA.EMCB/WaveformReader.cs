//******************************************************************************************************
//  WaveformReader.cs - Gbtc
//
//  Copyright © 2018, Grid Protection Alliance.  All Rights Reserved.
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
//  10/15/2018 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using FaultData.DataAnalysis;
using FaultData.DataReaders;
using FaultData.DataSets;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using openXDA.Model;

namespace openXDA.EMCB
{
    public class WaveformReader : IDataReader
    {
        #region [ Properties ]

        public MeterDataSet MeterDataSet { get; } = new MeterDataSet();
        private List<byte[]> WaveformData { get; set; } = new List<byte[]>();
        private List<byte[]> FastRMSData { get; set; } = new List<byte[]>();

        #endregion

        #region [ Methods ]

        public bool CanParse(string filePath, DateTime fileCreationTime)
        {
            try
            {
                string folderPath = Path.GetDirectoryName(filePath);

                foreach (string waveformDataFile in Directory.EnumerateFiles(folderPath))
                {
                    using (FileStream fileStream = File.OpenRead(waveformDataFile))
                    using (MemoryStream emcbStream = new MemoryStream())
                    {
                        fileStream.CopyTo(emcbStream);

                        if (waveformDataFile.EndsWith("waveform"))
                            WaveformData.Add(emcbStream.ToArray());
                        else if (waveformDataFile.EndsWith("fastRMS"))
                            FastRMSData.Add(emcbStream.ToArray());
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public void Parse(string filePath)
        {
            Meter meter = new Meter();
            meter.MeterLocation = new MeterLocation();
            meter.Channels = new List<Channel>();
            meter.AssetKey = "UNKNOWN";
            meter.Name = "Unknown";
            meter.Description = "Unknown";
            meter.Make = "Eaton";
            meter.Model = "EMCB";

            MeterLocation meterLocation = meter.MeterLocation;
            meterLocation.AssetKey = "UNKNOWN";
            meterLocation.Latitude = 0.0D;
            meterLocation.Longitude = 0.0D;
            meterLocation.Name = "Unknown";
            meterLocation.Description = "Unknown";

            DataSeries CreateDataSeries(string channelName, string type, string characteristic, string phase, DateTime[] timeSeries, JArray valueSeries)
            {
                if (valueSeries == null)
                    return null;

                const string SeriesType = "Values";

                DataSeries dataSeries = new DataSeries();

                dataSeries.DataPoints = timeSeries
                    .Zip(valueSeries, (time, token) => new DataPoint() { Time = time, Value = token.Value<double>() })
                    .ToList();

                // Convert from mV/mA to Volts/Amps
                dataSeries = dataSeries.Multiply(1.0D / 1000.0D);

                Series series = dataSeries.SeriesInfo = new Series();
                series.Channel = new Channel();
                series.SeriesType = new SeriesType() { Name = SeriesType };

                Channel channel = series.Channel;
                channel.Series = new List<Series>() { series };
                channel.MeasurementType = new MeasurementType() { Name = type };
                channel.MeasurementCharacteristic = new MeasurementCharacteristic() { Name = characteristic };
                channel.Phase = new Phase() { Name = phase };
                channel.Name = channelName;
                channel.Description = channelName;
                channel.HarmonicGroup = 0;

                if (type == "Voltage")
                    channel.PerUnitValue = 120.0D;

                return dataSeries;
            }

            void Parse(byte[] fileData, string characteristic)
            {
                using (MemoryStream fileStream = new MemoryStream(fileData))
                using (GZipStream decompressor = new GZipStream(fileStream, CompressionMode.Decompress))
                using (MemoryStream jsonStream = new MemoryStream())
                using (TextReader textReader = new StreamReader(jsonStream))
                using (JsonReader jsonReader = new JsonTextReader(textReader))
                {
                    decompressor.CopyTo(jsonStream);
                    decompressor.Dispose();
                    jsonStream.Position = 0;

                    dynamic obj = JObject.Load(jsonReader);
                    string meterKey = obj.idAgent;

                    meter.AssetKey = meterKey;
                    meter.Name = meterKey;
                    meter.Description = meterKey;
                    meterLocation.AssetKey = meterKey;
                    meterLocation.Name = meterKey;
                    meterLocation.Description = meterKey;

                    double startMilliseconds = obj.startTime;
                    double deltaTime = obj.deltaTime;
                    int samples = obj.numSamples;

                    DateTime epoch = new DateTime(1970, 1, 1);
                    DateTime startTime = epoch.AddMilliseconds(startMilliseconds);

                    DateTime[] timeSeries = Enumerable.Range(0, samples)
                        .Select(i => i * deltaTime)
                        .Select(startTime.AddMilliseconds)
                        .ToArray();

                    DataSeries[] allSeries =
                    {
                        CreateDataSeries("VAN", "Voltage", characteristic, "AN", timeSeries, obj.mVpA),
                        CreateDataSeries("VBN", "Voltage", characteristic, "BN", timeSeries, obj.mVpB),
                        CreateDataSeries("VCN", "Voltage", characteristic, "CN", timeSeries, obj.mVpC),
                        CreateDataSeries("IAN", "Current", characteristic, "AN", timeSeries, obj.mApA),
                        CreateDataSeries("IBN", "Current", characteristic, "BN", timeSeries, obj.mApB),
                        CreateDataSeries("ICN", "Current", characteristic, "CN", timeSeries, obj.mApC)
                    };

                    List<DataSeries> allValidSeries = allSeries
                        .Where(dataSeries => dataSeries != null)
                        .ToList();

                    MeterDataSet.Meter = meter;
                    MeterDataSet.DataSeries.AddRange(allValidSeries);

                    IEnumerable<Channel> allChannels = allValidSeries.Select(dataSeries => dataSeries.SeriesInfo.Channel);
                    meter.Channels.AddRange(allChannels);
                }
            }

            foreach (byte[] waveformData in WaveformData)
                Parse(waveformData, "Instantaneous");

            foreach (byte[] waveformData in FastRMSData)
                Parse(waveformData, "RMS");
        }

        #endregion
    }
}
