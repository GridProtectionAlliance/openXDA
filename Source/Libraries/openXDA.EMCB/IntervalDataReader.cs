//******************************************************************************************************
//  IntervalDataReader.cs - Gbtc
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
    public class IntervalDataReader : IDataReader
    {
        #region [ Properties ]

        public MeterDataSet MeterDataSet { get; } = new MeterDataSet();
        private List<byte[]> FileData { get; } = new List<byte[]>();

        #endregion

        #region [ Methods ]

        public bool CanParse(string filePath, DateTime fileCreationTime)
        {
            try
            {
                string folderPath = Path.GetDirectoryName(filePath);

                foreach (string intervalDataFile in Directory.EnumerateFiles(folderPath))
                {
                    FileInfo fileInfo = new FileInfo(intervalDataFile);

                    if (fileInfo.Length == 0)
                        continue;

                    using (FileStream fileStream = File.OpenRead(intervalDataFile))
                    using (MemoryStream emcbStream = new MemoryStream())
                    {
                        fileStream.CopyTo(emcbStream);
                        FileData.Add(emcbStream.ToArray());
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
            meter.Location = new Location();
            meter.Channels = new List<Channel>();
            meter.AssetKey = "UNKNOWN";
            meter.Name = "Unknown";
            meter.Description = "Unknown EMCB";
            meter.Make = "Eaton";
            meter.Model = "EMCB";

            Location meterLocation = meter.Location;
            meterLocation.LocationKey = "UNKNOWN";
            meterLocation.Latitude = 0.0D;
            meterLocation.Longitude = 0.0D;
            meterLocation.Name = "Unknown";
            meterLocation.Description = "Unknown";

            Dictionary<string, DataSeries> dataSeriesLookup = new Dictionary<string, DataSeries>();

            void AddChannel(string channelName, string type, string characteristic, string phase, Dictionary<string, string> seriesMappings)
            {
                Channel channel = new Channel();
                channel.Series = new List<Series>();
                channel.Meter = meter;
                channel.Name = channelName;
                channel.MeasurementType = new MeasurementType() { Name = type };
                channel.MeasurementCharacteristic = new MeasurementCharacteristic() { Name = characteristic };
                channel.Phase = new Phase() { Name = phase };
                channel.HarmonicGroup = 0;

                if (type == "Voltage")
                    channel.PerUnitValue = 120.0D;

                foreach (KeyValuePair<string, string> mapping in seriesMappings)
                {
                    string path = mapping.Key;
                    string seriesType = mapping.Value;

                    Series series = new Series();
                    series.Channel = channel;
                    series.SeriesType = new SeriesType() { Name = seriesType };
                    channel.Series.Add(series);

                    DataSeries dataSeries = new DataSeries();
                    dataSeries.SeriesInfo = series;
                    dataSeriesLookup.Add(path, dataSeries);
                }

                meter.Channels.Add(channel);
            }

            AddChannel("VAN", "Voltage", "RMS", "AN", new Dictionary<string, string>()
            {
                { "statistics.voltage.LN.AN.avg", "Average" },
                { "statistics.voltage.LN.AN.min", "Minimum" },
                { "statistics.voltage.LN.AN.max", "Maxmimum" },
                { "voltage.LN.AN", "Values" }
            });

            AddChannel("VBN", "Voltage", "RMS", "BN", new Dictionary<string, string>()
            {
                { "statistics.voltage.LN.BN.avg", "Average" },
                { "statistics.voltage.LN.BN.min", "Minimum" },
                { "statistics.voltage.LN.BN.max", "Maxmimum" },
                { "voltage.LN.BN", "Values" }
            });

            AddChannel("VCN", "Voltage", "RMS", "CN", new Dictionary<string, string>()
            {
                { "statistics.voltage.LN.CN.avg", "Average" },
                { "statistics.voltage.LN.CN.min", "Minimum" },
                { "statistics.voltage.LN.CN.max", "Maxmimum" },
                { "voltage.LN.CN", "Values" }
            });

            AddChannel("VAB", "Voltage", "RMS", "AB", new Dictionary<string, string>()
            {
                { "statistics.voltage.LL.AB.avg", "Average" },
                { "statistics.voltage.LL.AB.min", "Minimum" },
                { "statistics.voltage.LL.AB.max", "Maxmimum" },
                { "voltage.LL.AB", "Values" }
            });

            AddChannel("VBC", "Voltage", "RMS", "BC", new Dictionary<string, string>()
            {
                { "statistics.voltage.LL.BC.avg", "Average" },
                { "statistics.voltage.LL.BC.min", "Minimum" },
                { "statistics.voltage.LL.BC.max", "Maxmimum" },
                { "voltage.LL.BC", "Values" }
            });

            AddChannel("VCA", "Voltage", "RMS", "CA", new Dictionary<string, string>()
            {
                { "statistics.voltage.LL.CA.avg", "Average" },
                { "statistics.voltage.LL.CA.min", "Minimum" },
                { "statistics.voltage.LL.CA.max", "Maxmimum" },
                { "voltage.LL.CA", "Values" }
            });

            AddChannel("IAN", "Current", "RMS", "AN", new Dictionary<string, string>()
            {
                { "statistics.current.A.avg", "Average" },
                { "statistics.current.A.min", "Minimum" },
                { "statistics.current.A.max", "Maxmimum" },
                { "current.A", "Values" }
            });

            AddChannel("IBN", "Current", "RMS", "BN", new Dictionary<string, string>()
            {
                { "statistics.current.B.avg", "Average" },
                { "statistics.current.B.min", "Minimum" },
                { "statistics.current.B.max", "Maxmimum" },
                { "current.B", "Values" }
            });

            AddChannel("ICN", "Current", "RMS", "CN", new Dictionary<string, string>()
            {
                { "statistics.current.C.avg", "Average" },
                { "statistics.current.C.min", "Minimum" },
                { "statistics.current.C.max", "Maxmimum" },
                { "current.C", "Values" }
            });

            AddChannel("FREQ", "Voltage", "Frequency", "AN", new Dictionary<string, string>()
            {
                { "statistics.frequency.avg", "Average" },
                { "statistics.frequency.min", "Minimum" },
                { "statistics.frequency.max", "Maxmimum" },
                { "frequency", "Values" }
            });

            AddChannel("Q1 Active Energy AN", "Energy", "PIntgPos", "AN", new Dictionary<string, string>() { { "energy.q1mJpA", "Total" } });
            AddChannel("Q2 Active Energy AN", "Energy", "PIntgNeg", "AN", new Dictionary<string, string>() { { "energy.q2mJpA", "Total" } });
            AddChannel("Q3 Active Energy AN", "Energy", "PIntgNeg", "AN", new Dictionary<string, string>() { { "energy.q3mJpA", "Total" } });
            AddChannel("Q4 Active Energy AN", "Energy", "PIntgPos", "AN", new Dictionary<string, string>() { { "energy.q4mJpA", "Total" } });
            AddChannel("Q1 Active Energy BN", "Energy", "PIntgPos", "BN", new Dictionary<string, string>() { { "energy.q1mJpB", "Total" } });
            AddChannel("Q2 Active Energy BN", "Energy", "PIntgNeg", "BN", new Dictionary<string, string>() { { "energy.q2mJpB", "Total" } });
            AddChannel("Q3 Active Energy BN", "Energy", "PIntgNeg", "BN", new Dictionary<string, string>() { { "energy.q3mJpB", "Total" } });
            AddChannel("Q4 Active Energy BN", "Energy", "PIntgPos", "BN", new Dictionary<string, string>() { { "energy.q4mJpB", "Total" } });
            AddChannel("Q1 Active Energy CN", "Energy", "PIntgPos", "CN", new Dictionary<string, string>() { { "energy.q1mJpC", "Total" } });
            AddChannel("Q2 Active Energy CN", "Energy", "PIntgNeg", "CN", new Dictionary<string, string>() { { "energy.q2mJpC", "Total" } });
            AddChannel("Q3 Active Energy CN", "Energy", "PIntgNeg", "CN", new Dictionary<string, string>() { { "energy.q3mJpC", "Total" } });
            AddChannel("Q4 Active Energy CN", "Energy", "PIntgPos", "CN", new Dictionary<string, string>() { { "energy.q4mJpC", "Total" } });

            AddChannel("Q1 Reactive Energy AN", "Energy", "QIntgPos", "AN", new Dictionary<string, string>() { { "energy.q1mVARspA", "Total" } });
            AddChannel("Q2 Reactive Energy AN", "Energy", "QIntgPos", "AN", new Dictionary<string, string>() { { "energy.q2mVARspA", "Total" } });
            AddChannel("Q3 Reactive Energy AN", "Energy", "QIntgNeg", "AN", new Dictionary<string, string>() { { "energy.q3mVARspA", "Total" } });
            AddChannel("Q4 Reactive Energy AN", "Energy", "QIntgNeg", "AN", new Dictionary<string, string>() { { "energy.q4mVARspA", "Total" } });
            AddChannel("Q1 Reactive Energy BN", "Energy", "QIntgPos", "BN", new Dictionary<string, string>() { { "energy.q1mVARspB", "Total" } });
            AddChannel("Q2 Reactive Energy BN", "Energy", "QIntgPos", "BN", new Dictionary<string, string>() { { "energy.q2mVARspB", "Total" } });
            AddChannel("Q3 Reactive Energy BN", "Energy", "QIntgNeg", "BN", new Dictionary<string, string>() { { "energy.q3mVARspB", "Total" } });
            AddChannel("Q4 Reactive Energy BN", "Energy", "QIntgNeg", "BN", new Dictionary<string, string>() { { "energy.q4mVARspB", "Total" } });
            AddChannel("Q1 Reactive Energy CN", "Energy", "QIntgPos", "CN", new Dictionary<string, string>() { { "energy.q1mVARspC", "Total" } });
            AddChannel("Q2 Reactive Energy CN", "Energy", "QIntgPos", "CN", new Dictionary<string, string>() { { "energy.q2mVARspC", "Total" } });
            AddChannel("Q3 Reactive Energy CN", "Energy", "QIntgNeg", "CN", new Dictionary<string, string>() { { "energy.q3mVARspC", "Total" } });
            AddChannel("Q4 Reactive Energy CN", "Energy", "QIntgNeg", "CN", new Dictionary<string, string>() { { "energy.q4mVARspC", "Total" } });

            JToken Get(JToken obj, string path) =>
                path.Split('.').Aggregate(obj, (token, identifier) => token?[identifier]);

            foreach (byte[] fileData in FileData)
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

                    JObject obj = JObject.Load(jsonReader);

                    DateTime epoch = new DateTime(1970, 1, 1);
                    double ts = obj["ts"].Value<double>();
                    DateTime timestamp = epoch.AddSeconds(ts);
                    
                    foreach (KeyValuePair<string, DataSeries> kvp in dataSeriesLookup)
                    {
                        string path = kvp.Key;
                        DataSeries dataSeries = kvp.Value;
                        JToken token = Get(obj, path);

                        if (token != null)
                        {
                            double value = token.Value<double>();
                            DataPoint dataPoint = new DataPoint() { Time = timestamp, Value = value };
                            dataSeries.DataPoints.Add(dataPoint);
                        }
                    }
                }
            }

            foreach (KeyValuePair<string, DataSeries> kvp in dataSeriesLookup)
            {
                DataSeries dataSeries = kvp.Value;

                if (dataSeries.DataPoints.Count > 0)
                {
                    string path = kvp.Key;

                    double adjustment = path.StartsWith("energy")
                        ? (1.0D / 1000.0D / 3600.0D)  // mJ -> Watt-hours || mVAR*s -> VAR-hours
                        : (1.0D / 1000.0D);           // mV -> Volts || mA -> Amps || mHz -> Hz

                    DataSeries adjustedData = dataSeries.Multiply(adjustment);
                    adjustedData.SeriesInfo = dataSeries.SeriesInfo;
                    MeterDataSet.DataSeries.Add(adjustedData);
                }
            }

            MeterDataSet.Meter = meter;
        }

        #endregion
    }
}
