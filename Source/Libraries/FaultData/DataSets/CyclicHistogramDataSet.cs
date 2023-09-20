//******************************************************************************************************
//  CyclicHistogramDataSet.cs - Gbtc
//
//  Copyright © 2023, Grid Protection Alliance.  All Rights Reserved.
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
//  09/15/2023 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FaultData.DataSets
{
    public class CyclicHistogramDataSet
    {
        #region [ Members ]

        // Nested Types
        public class MetadataSet
        {
            public string DFRName { get; set; }
            public string ChannelName { get; set; }
            public string ChannelPhase { get; set; }
            public string ChannelLineName { get; set; }
            public int FundamentalFrequency { get; set; }
            public int SamplingRate { get; set; }
            public TimeSpan StartTime { get; set; }
            public TimeSpan EndTime { get; set; }
            public int TotalCapturedCycles { get; set; }
            public double CyclesMax { get; set; }
            public double CyclesMin { get; set; }
            public double ResidualMax { get; set; }
            public double ResidualMin { get; set; }
            public double FrequencyMax { get; set; }
            public double FrequencyMin { get; set; }
            public double RMSMax { get; set; }
            public double RMSMin { get; set; }
            public int CyclicHistogramBins { get; set; }
            public int ResidualHistogramBins { get; set; }
            public int FrequencyHistogramBins { get; set; }
            public string DataPrecision { get; set; }
            public DateTime RecordDate { get; set; }
            public string TimeZone { get; set; }
        }

        public class DataPoint
        {
            public int Bin { get; }
            public int Sample { get; }
            public float Value { get; }

            public DataPoint(int bin, int sample, float value)
            {
                Bin = bin;
                Sample = sample;
                Value = value;
            }
        }

        // Delegates
        private delegate void MetadataLoadAction(MetadataSet metadata, string fieldName, string value);

        #endregion

        #region [ Constructors ]

        private CyclicHistogramDataSet(MetadataSet metadata, List<DataPoint> cyclicHistogram, List<DataPoint> residualHistogram, List<DataPoint> frequencyHistogram, List<DataPoint> rmsHistogram, int rmsHistogramBins)
        {
            Metadata = metadata;
            CyclicHistogram = cyclicHistogram;
            ResidualHistogram = residualHistogram;
            FrequencyHistogram = frequencyHistogram;
            RMSHistogram = rmsHistogram;
            RMSHistogramBins = rmsHistogramBins;
        }

        #endregion

        #region [ Properties ]

        public MetadataSet Metadata { get; }
        public List<DataPoint> CyclicHistogram { get; }
        public List<DataPoint> ResidualHistogram { get; }
        public List<DataPoint> FrequencyHistogram { get; }
        public List<DataPoint> RMSHistogram { get; }
        public int RMSHistogramBins { get; }

        #endregion

        #region [ Static ]

        // Static Properties
        private static MetadataLoadAction MetadataLoader { get; }
            = InitializeMetadataLoader();

        // Static Methods
        public static MetadataSet LoadMetadata(string filePath)
        {
            using (Stream stream = File.OpenRead(filePath))
            {
                return LoadMetadata(stream);
            }
        }

        public static MetadataSet LoadMetadata(Stream stream)
        {
            Encoding encoding = new UTF8Encoding(false);

            using (TextReader reader = new StreamReader(stream, encoding, false, 4096, true))
            {
                return LoadMetadata(reader);
            }
        }

        public static MetadataSet LoadMetadata(TextReader reader)
        {
            MetadataSet metadata = new MetadataSet();

            while (true)
            {
                string line = reader.ReadLine();
                if (line is null)
                    return metadata;

                int commaIndex = line.IndexOf(',');
                if (commaIndex == -1)
                    continue;

                string fieldName = line.Substring(0, commaIndex);
                string value = line.Substring(commaIndex + 1);
                MetadataLoader.Invoke(metadata, fieldName, value);
            }
        }

        public static CyclicHistogramDataSet LoadAllHistograms(MetadataSet metadata, string filePath)
        {
            using (Stream stream = File.OpenRead(filePath))
            {
                return LoadAllHistograms(metadata, stream);
            }
        }

        public static CyclicHistogramDataSet LoadAllHistograms(MetadataSet metadata, Stream stream) =>
            LoadAllHistograms(metadata, stream, (int)stream.Length);

        public static CyclicHistogramDataSet LoadAllHistograms(MetadataSet metadata, Stream stream, int totalBytes)
        {
            using (BinaryReader reader = new BinaryReader(stream, Encoding.Default, true))
            {
                return LoadAllHistograms(metadata, reader, totalBytes);
            }
        }

        public static CyclicHistogramDataSet LoadAllHistograms(MetadataSet metadata, BinaryReader reader) =>
            LoadAllHistograms(metadata, reader, (int)reader.BaseStream.Length);

        public static CyclicHistogramDataSet LoadAllHistograms(MetadataSet metadata, BinaryReader reader, int totalBytes)
        {
            int GetValueSize(string dataType)
            {
                switch (dataType)
                {
                    case "float32": return sizeof(float);
                    default: throw new InvalidDataException($"Unknown data type: {dataType}");
                }
            }

            float LoadValue()
            {
                switch (metadata.DataPrecision)
                {
                    case "float32": return reader.ReadSingle();
                    default: throw new InvalidDataException($"Unknown data type: {metadata.DataPrecision}");
                }
            }

            int samplesPerCycle = metadata.SamplingRate / metadata.FundamentalFrequency;

            int valueSize = GetValueSize(metadata.DataPrecision);
            int cyclicHistogramByteSize = (samplesPerCycle + 1) * metadata.CyclicHistogramBins * valueSize;
            int residualHistogramByteSize = (samplesPerCycle + 1) * metadata.ResidualHistogramBins * valueSize;
            int frequencyHistogramByteSize = metadata.FrequencyHistogramBins * valueSize;
            int rmsHistogramByteSize = totalBytes - cyclicHistogramByteSize - residualHistogramByteSize - frequencyHistogramByteSize;
            int rmsHistogramBins = rmsHistogramByteSize / valueSize;

            List<DataPoint> cyclicHistogram = LoadHistogram(samplesPerCycle + 1, metadata.CyclicHistogramBins, LoadValue);
            List<DataPoint> residualHistogram = LoadHistogram(samplesPerCycle + 1, metadata.ResidualHistogramBins, LoadValue);
            List<DataPoint> frequencyHistogram = LoadHistogram(1, metadata.FrequencyHistogramBins, LoadValue);
            List<DataPoint> rmsHistogram = LoadHistogram(1, rmsHistogramBins, LoadValue);
            return new CyclicHistogramDataSet(metadata, cyclicHistogram, residualHistogram, frequencyHistogram, rmsHistogram, rmsHistogramBins);
        }

        private static List<DataPoint> LoadHistogram(int binSize, int binCount, Func<float> loadValueFunc)
        {
            List<DataPoint> histogram = new List<DataPoint>();

            for (int binIndex = 0; binIndex < binCount; binIndex++)
            {
                for (int sampleIndex = 0; sampleIndex < binSize; sampleIndex++)
                {
                    float value = loadValueFunc();
                    if (value == 0.0F)
                        continue;
                    DataPoint bin = new DataPoint(binIndex, sampleIndex, value);
                    histogram.Add(bin);
                }
            }

            return histogram;
        }

        private static MetadataLoadAction InitializeMetadataLoader()
        {
            const string DFRNameField = "DFR Name";
            const string ChannelNameField = "Channel Name";
            const string ChannelPhaseField = "Channel Phase";
            const string ChannelLineNameField = "Channel Line Name";
            const string FundamentalFrequencyField = "Fundamental Frequency";
            const string SamplingRateField = "Sampling Rate";
            const string StartTimeField = "Start Time";
            const string EndTimeField = "End Time";
            const string TotalCapturedCyclesField = "Total Captured Cycles";
            const string CyclesMaxField = "Cycles Max";
            const string CyclesMinField = "Cycles Min";
            const string ResidualMaxField = "Residual Max";
            const string ResidualMinField = "Residual Min";
            const string FrequencyMaxField = "Frequency Max";
            const string FrequencyMinField = "Frequency Min";
            const string RMSMaxField = "RMS Max";
            const string RMSMinField = "RMS Min";
            const string CyclicHistogramBinsField = "Cyclic Histogram Bins";
            const string ResidualHistogramBinsField = "Residual Histogram Bins";
            const string FrequencyHistogramBinsField = "Frequency Histogram Bins";
            const string DataPercisionField = "Data Percision";
            const string DataPrecisionField = "Data Precision";
            const string RecordDateField = "Record Date";
            const string TimeZoneField = "Time Zone";

            Action<MetadataSet, string> LoadTime(Action<MetadataSet, TimeSpan> loader) => (metadata, value) =>
            {
                if (TimeSpan.TryParse(value, out TimeSpan num))
                    loader(metadata, num);
            };

            Action<MetadataSet, string> LoadDate(Action<MetadataSet, DateTime> loader) => (metadata, value) =>
            {
                if (DateTime.TryParse(value, out DateTime num))
                    loader(metadata, num);
            };

            Action<MetadataSet, string> LoadInteger(Action<MetadataSet, int> loader) => (metadata, value) =>
            {
                if (int.TryParse(value, out int num))
                    loader(metadata, num);
            };

            Action<MetadataSet, string> LoadDouble(Action<MetadataSet, double> loader) => (metadata, value) =>
            {
                if (double.TryParse(value, out double num))
                    loader(metadata, num);
            };

            Dictionary<string, Action<MetadataSet, string>> lookup = new Dictionary<string, Action<MetadataSet, string>>()
            {
                { DFRNameField, (metadata, value) => metadata.DFRName = value },
                { ChannelNameField, (metadata, value) => metadata.ChannelName = value },
                { ChannelPhaseField, (metadata, value) => metadata.ChannelPhase = value },
                { ChannelLineNameField, (metadata, value) => metadata.ChannelLineName = value },
                { FundamentalFrequencyField, LoadInteger((metadata, value) => metadata.FundamentalFrequency = value) },
                { SamplingRateField, LoadInteger((metadata, value) => metadata.SamplingRate = value) },
                { StartTimeField, LoadTime((metadata, value) => metadata.StartTime = value) },
                { EndTimeField, LoadTime((metadata, value) => metadata.EndTime = value) },
                { TotalCapturedCyclesField, LoadInteger((metadata, value) => metadata.TotalCapturedCycles = value) },
                { CyclesMaxField, LoadDouble((metadata, value) => metadata.CyclesMax = value) },
                { CyclesMinField, LoadDouble((metadata, value) => metadata.CyclesMin = value) },
                { ResidualMaxField, LoadDouble((metadata, value) => metadata.ResidualMax = value) },
                { ResidualMinField, LoadDouble((metadata, value) => metadata.ResidualMin = value) },
                { FrequencyMaxField, LoadDouble((metadata, value) => metadata.FrequencyMax = value) },
                { FrequencyMinField, LoadDouble((metadata, value) => metadata.FrequencyMin = value) },
                { RMSMaxField, LoadDouble((metadata, value) => metadata.RMSMax = value) },
                { RMSMinField, LoadDouble((metadata, value) => metadata.RMSMin = value) },
                { CyclicHistogramBinsField, LoadInteger((metadata, value) => metadata.CyclicHistogramBins = value) },
                { ResidualHistogramBinsField, LoadInteger((metadata, value) => metadata.ResidualHistogramBins = value) },
                { FrequencyHistogramBinsField, LoadInteger((metadata, value) => metadata.FrequencyHistogramBins = value) },
                { DataPercisionField, (metadata, value) => metadata.DataPrecision = value },
                { DataPrecisionField, (metadata, value) => metadata.DataPrecision = value },
                { RecordDateField, LoadDate((metadata, value) => metadata.RecordDate = value) },
                { TimeZoneField, (metadata, value) => metadata.TimeZone = value },
            };

            return (metadata, fieldName, value) =>
            {
                if (lookup.TryGetValue(fieldName, out Action<MetadataSet, string> loader))
                    loader(metadata, value);
            };
        }

        #endregion
    }
}
