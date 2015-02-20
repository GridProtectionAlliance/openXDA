//******************************************************************************************************
//  COMTRADEReader.cs - Gbtc
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
//  08/01/2014 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GSF.COMTRADE;
using GSF.IO;
using FaultData.DataAnalysis;
using FaultData.Database;
using FaultData.DataSets;

namespace FaultData.DataReaders
{
    public class COMTRADEReader : IDataReader
    {
        private TimeSpan m_minWaitTime;

        public COMTRADEReader()
        {
            // TODO: Hardcoded min wait time
            m_minWaitTime = TimeSpan.FromSeconds(15.0D);
        }

        public bool CanParse(string filePath)
        {
            string directory = FilePath.GetDirectoryName(filePath);
            string rootFileName = FilePath.GetFileNameWithoutExtension(filePath);
            string schemaFileName = Path.Combine(directory, rootFileName + ".cfg");
            string extension = FilePath.GetExtension(filePath);
            string[] fileList = FilePath.GetFileList(rootFileName + ".*");
            bool multipleDataFiles = !extension.Equals(".dat", StringComparison.OrdinalIgnoreCase);

            if (!File.Exists(schemaFileName))
                return false;

            if (fileList.Any(file => !FilePath.TryGetReadLockExclusive(file)))
                return false;

            if (multipleDataFiles && fileList.Any(file => DateTime.UtcNow - File.GetCreationTime(filePath) < m_minWaitTime))
                return false;

            return true;
        }

        public List<MeterDataSet> Parse(string filePath)
        {
            MeterDataSet meterDataSet = new MeterDataSet();

            string directory = FilePath.GetDirectoryName(filePath) ?? string.Empty;
            string rootFileName = FilePath.GetFileNameWithoutExtension(filePath);
            string configurationFileName = Path.Combine(directory, rootFileName + ".cfg");

            Schema schema = new Schema(configurationFileName);

            Channel channel;
            DataSeries series;

            using (Parser parser = new Parser())
            {
                parser.Schema = schema;
                parser.FileName = filePath;
                parser.InferTimeFromSampleRates = true;
                parser.OpenFiles();

                meterDataSet.Meter = new Meter();
                meterDataSet.Meter.AssetKey = schema.DeviceID;
                meterDataSet.Meter.Name = schema.DeviceID;
                meterDataSet.Meter.ShortName = schema.DeviceID.Substring(0, Math.Min(schema.DeviceID.Length, 50));

                meterDataSet.Meter.MeterLocation = new MeterLocation();
                meterDataSet.Meter.MeterLocation.AssetKey = schema.StationName;
                meterDataSet.Meter.MeterLocation.Name = schema.StationName;
                meterDataSet.Meter.MeterLocation.ShortName = schema.StationName.Substring(0, Math.Min(schema.StationName.Length, 50));
                meterDataSet.Meter.MeterLocation.Description = schema.StationName;

                foreach (AnalogChannel analogChannel in schema.AnalogChannels)
                {
                    channel = ParseSeries(analogChannel);

                    series = new DataSeries();
                    series.SeriesInfo = channel.Series[0];

                    meterDataSet.Meter.Channels.Add(channel);

                    while (meterDataSet.DataSeries.Count <= analogChannel.Index)
                        meterDataSet.DataSeries.Add(new DataSeries());

                    meterDataSet.DataSeries[analogChannel.Index] = series;
                }

                while (parser.ReadNext())
                {
                    for (int i = 0; i < schema.AnalogChannels.Length; i++)
                    {
                        int seriesIndex = schema.AnalogChannels[i].Index;
                        string units = schema.AnalogChannels[i].Units.ToUpper();
                        double multiplier = (units.Contains("KA") || units.Contains("KV")) ? 1000.0D : 1.0D;
                        meterDataSet.DataSeries[seriesIndex].DataPoints.Add(new DataPoint() { Time = parser.Timestamp, Value = multiplier * parser.Values[i] });
                    }
                }
            }

            return new List<MeterDataSet>() { meterDataSet };
        }

        private Channel ParseSeries(AnalogChannel analogChannel)
        {
            Channel channel = new Channel();
            Series series = new Series();

            channel.Name = analogChannel.ChannelName;
            channel.HarmonicGroup = 0;
            channel.MeasurementType = new MeasurementType();
            channel.MeasurementType.Name = "Unknown";
            channel.MeasurementCharacteristic = new MeasurementCharacteristic();
            channel.MeasurementCharacteristic.Name = "Unknown";
            channel.Phase = new Phase();
            channel.Phase.Name = !string.IsNullOrEmpty(analogChannel.PhaseID) ? analogChannel.PhaseID : "Unknown";

            series.Channel = channel;
            series.SeriesType = new SeriesType();
            series.SeriesType.Name = "Instantaneous";
            series.SourceIndexes = analogChannel.Index.ToString();

            return channel;
        }
    }
}
