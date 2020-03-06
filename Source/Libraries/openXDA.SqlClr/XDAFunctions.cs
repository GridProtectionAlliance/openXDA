//******************************************************************************************************
//  XDAFunctions.cs - Gbtc
//
//  Copyright © 2016, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the MIT License (MIT), the "License"; you may
//  not use this file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://opensource.org/licenses/MIT
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  09/01/2016 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using System.IO.Compression;
using System.Linq;
using GSF.NumericalAnalysis;
using Microsoft.SqlServer.Server;

namespace openXDA.SqlClr
{
    public class XDAFunctions
    {
        private class DataPoint
        {
            public int SeriesID;
            public string Characteristic;
            public DateTime Time;
            public double Value;
        }

        private class FaultDataPoint
        {
            public string Algorithm;
            public DateTime Time;
            public double Value;
        }

        [SqlFunction(
            DataAccess = DataAccessKind.Read,
            FillRowMethodName = "GetEventData_FillRow",
            TableDefinition = "[SeriesID] INT, [Characteristic] NVARCHAR(200), [Time] DATETIME2, [Value] FLOAT")
        ]
        public static IEnumerable GetEventData(SqlInt32 eventID)
        {
            const string Query =
                "SELECT TimeDomainData " +
                "FROM EventData " +
                "WHERE ID = " +
                "(" +
                "    SELECT EventDataID " +
                "    FROM Event " +
                "    WHERE ID = @id " +
                ")";

            DataSet eventDataSet = new DataSet();

            using (SqlConnection connection = new SqlConnection("context connection=true"))
            using (SqlCommand command = new SqlCommand(Query, connection))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                connection.Open();
                command.Parameters.AddWithValue("@id", eventID);
                adapter.Fill(eventDataSet);
            }

            DataRow row = eventDataSet.Tables[0].Rows[0];

            List<DataPoint> waveformData = ReadFrom((byte[])row["TimeDomainData"]).ToList();
            List<DataPoint> cycleData = CalculateCycleData(waveformData).ToList();
            return waveformData.Concat(cycleData).ToArray();
        }

        public static void GetEventData_FillRow(object source, out SqlInt32 seriesID, out SqlString characteristic, out DateTime time, out SqlDouble value)
        {
            DataPoint dataPoint = source as DataPoint;

            if ((object)dataPoint == null)
                throw new InvalidOperationException("FillRow source is not a DataPoint");

            seriesID = dataPoint.SeriesID;
            characteristic = dataPoint.Characteristic;
            time = dataPoint.Time;
            value = ToSqlDouble(dataPoint.Value);
        }

        [SqlFunction(
            DataAccess = DataAccessKind.Read,
            FillRowMethodName = "GetCycleData_FillRow",
            TableDefinition = "[SeriesID] INT, [Time] DATETIME2, [Value] FLOAT")
        ]
        public static IEnumerable GetCycleData(SqlInt32 eventID)
        {
            const string Query =
                "SELECT CycleData " +
                "FROM EventData " +
                "WHERE ID = " +
                "(" +
                "    SELECT EventDataID " +
                "    FROM Event " +
                "    WHERE ID = @id " +
                ")";

            DataSet eventDataSet = new DataSet();

            using (SqlConnection connection = new SqlConnection("context connection=true"))
            using (SqlCommand command = new SqlCommand(Query, connection))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                connection.Open();
                command.Parameters.AddWithValue("@id", eventID);
                adapter.Fill(eventDataSet);
            }

            DataRow row = eventDataSet.Tables[0].Rows[0];

            List<DataPoint> waveformData = ReadFrom((byte[])row["CycleData"]).ToList();
            return waveformData.ToArray();
        }

        public static void GetCycleData_FillRow(object source, out SqlInt32 seriesID, out DateTime time, out SqlDouble value)
        {
            DataPoint dataPoint = source as DataPoint;

            if ((object)dataPoint == null)
                throw new InvalidOperationException("FillRow source is not a DataPoint");

            seriesID = dataPoint.SeriesID;
            time = dataPoint.Time;
            value = ToSqlDouble(dataPoint.Value);
        }

        [SqlFunction(
            DataAccess = DataAccessKind.Read,
            FillRowMethodName = "GetFaultData_FillRow",
            TableDefinition = "[Algorithm] NVARCHAR(80), [Time] DATETIME2, [Value] FLOAT")
        ]
        public static IEnumerable GetFaultData(SqlInt32 eventID)
        {
            const string Query =
                "SELECT " +
                "    Algorithm, " +
                "    Data " +
                "FROM FaultCurve " +
                "WHERE EventID = @id";

            DataSet faultDataSet = new DataSet();

            using (SqlConnection connection = new SqlConnection("context connection=true"))
            using (SqlCommand command = new SqlCommand(Query, connection))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                connection.Open();
                command.Parameters.AddWithValue("@id", eventID);
                adapter.Fill(faultDataSet);
            }

            return faultDataSet.Tables[0].Select()
                .SelectMany(row => ReadFrom((byte[])row["Data"]).Select(dataPoint => new FaultDataPoint()
                {
                    Algorithm = row["Algorithm"].ToString(),
                    Time = dataPoint.Time,
                    Value = dataPoint.Value
                }))
                .ToArray();
        }

        public static void GetFaultData_FillRow(object source, out SqlString algorithm, out DateTime time, out SqlDouble value)
        {
            FaultDataPoint dataPoint = source as FaultDataPoint;

            if ((object)dataPoint == null)
                throw new InvalidOperationException("FillRow source is not a DataPoint");

            algorithm = dataPoint.Algorithm;
            time = dataPoint.Time;
            value = ToSqlDouble(dataPoint.Value);
        }

        private static IEnumerable<DataPoint> CalculateCycleData(IEnumerable<DataPoint> waveformData)
        {
            // Organize the data into individual series
            List<List<DataPoint>> seriesList = waveformData
                .GroupBy(dataPoint => dataPoint.SeriesID)
                .Where(grouping => IsVIWaveform(grouping.Key))
                .Select(grouping => grouping.OrderBy(dataPoint => dataPoint.Time))
                .Select(grouping => grouping.ToList())
                .ToList();

            double frequency = GetFrequency();

            foreach (List<DataPoint> dataSeries in seriesList)
            {
                // Get samples per cycle of the data series based on the given frequency
                int samplesPerCycle = CalculateSamplesPerCycle(dataSeries, frequency);

                // Initialize arrays of y-values and t-values for calculating cycle data
                double[] yValues = new double[samplesPerCycle];
                double[] tValues = new double[samplesPerCycle];

                // Obtain a list of time gaps in the data series
                List<int> gapIndexes = Enumerable.Range(0, dataSeries.Count - 1)
                    .Where(index =>
                    {
                        DataPoint p1 = dataSeries[index];
                        DataPoint p2 = dataSeries[index + 1];
                        double cycleDiff = (p2.Time - p1.Time).TotalSeconds * frequency;

                    // Detect gaps larger than a quarter cycle.
                    // Tolerance of 0.000062 calculated
                    // assuming 3.999 samples per cycle
                    return (cycleDiff > 0.250062);
                    })
                    .ToList();

                for (int i = 0; i <= dataSeries.Count - samplesPerCycle; i++)
                {
                    // If the cycle following i contains a data gap, do not calculate cycle data
                    if (gapIndexes.Any(index => i <= index && (i + samplesPerCycle - 1) > index))
                        continue;

                    // Use the time of the first data point in the cycle as the time of the cycle
                    DateTime cycleTime = dataSeries[i].Time;
                    double sum = 0.0D;

                    // Copy values from the original data series into the y-value and t-value arrays
                    for (int j = 0; j < samplesPerCycle; j++)
                    {
                        yValues[j] = dataSeries[i + j].Value;
                        tValues[j] = (dataSeries[i + j].Time - cycleTime).TotalSeconds;
                        sum += yValues[j] * yValues[j];
                    }

                    // Use a curve fitting algorithm to estimate the sine wave over this cycle
                    SineWave sineFit = WaveFit.SineFit(yValues, tValues, frequency);

                    // Add data points to each of the cycle data series
                    yield return new DataPoint()
                    {
                        SeriesID = dataSeries[0].SeriesID,
                        Characteristic = "RMS",
                        Time = cycleTime,
                        Value = Math.Sqrt(sum / samplesPerCycle)
                    };

                    yield return new DataPoint()
                    {
                        SeriesID = dataSeries[0].SeriesID,
                        Characteristic = "AngleFund",
                        Time = cycleTime,
                        Value = sineFit.Phase
                    };

                    yield return new DataPoint()
                    {
                        SeriesID = dataSeries[0].SeriesID,
                        Characteristic = "WaveAmplitude",
                        Time = cycleTime,
                        Value = sineFit.Amplitude
                    };

                    yield return new DataPoint()
                    {
                        SeriesID = dataSeries[0].SeriesID,
                        Characteristic = "WaveError",
                        Time = cycleTime,

                        Value = tValues
                            .Select(sineFit.CalculateY)
                            .Zip(yValues, (estimate, value) => Math.Abs(estimate - value))
                            .Sum()
                    };
                }
            }
        }

        private static double GetFrequency()
        {
            // Need to get the system frequency to calculate samples per cycle
            const string Query = "SELECT Value FROM Setting WHERE Name = 'SystemFrequency'";

            double frequency = 60.0D;

            using (SqlConnection connection = new SqlConnection("context connection=true"))
            using (SqlCommand command = new SqlCommand(Query, connection))
            {
                connection.Open();

                object systemFrequency = command.ExecuteScalar();

                try
                {
                    if (systemFrequency != null && !Convert.IsDBNull(systemFrequency))
                        frequency = Convert.ToDouble(systemFrequency);
                }
                catch (FormatException)
                {
                    // Ignore format exceptions and just use 60 Hz
                }
            }

            return frequency;
        }

        private static bool IsVIWaveform(int seriesID)
        {
            // Need to get the system frequency to calculate samples per cycle
            const string Query =
                "SELECT COUNT(*) " +
                "FROM " +
                "    Series JOIN " +
                "    Channel ON Series.ChannelID = Channel.ID JOIN " +
                "    MeasurementType ON Channel.MeasurementTypeID = MeasurementType.ID JOIN " +
                "    Phase ON Channel.PhaseID = Phase.ID " +
                "WHERE " +
                "    Series.ID = @seriesID AND " +
                "    MeasurementType.Name IN ('Voltage', 'Current') AND " +
                "    Phase.Name IN ('AN', 'BN', 'CN', 'AB', 'BC', 'CA')";

            using (SqlConnection connection = new SqlConnection("context connection=true"))
            using (SqlCommand command = new SqlCommand(Query, connection))
            {
                connection.Open();

                command.Parameters.AddWithValue("@seriesID", seriesID);
                int count = Convert.ToInt32(command.ExecuteScalar() ?? 0);
                return count > 0;
            }
        }

        private static int CalculateSamplesPerCycle(List<DataPoint> dataSeries, double frequency)
        {
            int[] commonSampleRates =
            {
                4, 8, 16, 32,
                80, 96, 100, 200,
                64, 128, 256, 512, 1024
            };

            int index = (dataSeries.Count > 128) ? 128 : dataSeries.Count - 1;

            double duration = (dataSeries[index].Time - dataSeries[0].Time).TotalSeconds;

            double samplesPerSecond = (duration != 0.0D)
                ? index / duration
                : double.NaN;

            int calculatedRate = (int)Math.Round(samplesPerSecond / frequency);
            int nearestCommonRate = commonSampleRates.Aggregate((min, rate) => Math.Abs(calculatedRate - rate) < Math.Abs(calculatedRate - min) ? rate : min);
            int diff = Math.Abs(calculatedRate - nearestCommonRate);
            return (diff < nearestCommonRate * 0.1D) ? nearestCommonRate : calculatedRate;
        }

        private static IEnumerable<DataPoint> ReadFrom(byte[] data)
        {
            // If the blob contains the GZip header,
            // use the legacy deserialization algorithm
            if (data[0] == 0x1F && data[1] == 0x8B)
                return ReadFrom_Legacy(data);

            return ReadFrom_Custom(data);
        }

        private static IEnumerable<DataPoint> ReadFrom_Custom(byte[] data)
        {
            // Restore the GZip header before uncompressing
            data[0] = 0x1F;
            data[1] = 0x8B;

            byte[] uncompressedData = Inflate(data);
            int offset = 0;

            int samples = BitConverter.ToInt32(uncompressedData, offset);
            offset += sizeof(int);

            List<DateTime> times = new List<DateTime>();

            while (times.Count < samples)
            {
                int timeValues = BitConverter.ToInt32(uncompressedData, offset);
                offset += sizeof(int);

                long currentValue = BitConverter.ToInt64(uncompressedData, offset);
                offset += sizeof(long);
                times.Add(new DateTime(currentValue));

                for (int i = 1; i < timeValues; i++)
                {
                    currentValue += BitConverter.ToUInt16(uncompressedData, offset);
                    offset += sizeof(ushort);
                    times.Add(new DateTime(currentValue));
                }
            }

            int seriesIndex = 0;

            while (offset < uncompressedData.Length)
            {
                int seriesID = BitConverter.ToInt32(uncompressedData, offset);
                offset += sizeof(int);

                const ushort NaNValue = ushort.MaxValue;
                double decompressionOffset = BitConverter.ToDouble(uncompressedData, offset);
                double decompressionScale = BitConverter.ToDouble(uncompressedData, offset + sizeof(double));
                offset += 2 * sizeof(double);

                for (int i = 0; i < samples; i++)
                {
                    ushort compressedValue = BitConverter.ToUInt16(uncompressedData, offset);
                    offset += sizeof(ushort);

                    double decompressedValue = decompressionScale * compressedValue + decompressionOffset;

                    if (compressedValue == NaNValue)
                        decompressedValue = double.NaN;

                    yield return new DataPoint()
                    {
                        SeriesID = seriesID,
                        Characteristic = "Instantaneous",
                        Time = times[i],
                        Value = decompressedValue
                    };
                }

                seriesIndex++;
            }
        }

        private static IEnumerable<DataPoint> ReadFrom_Legacy(byte[] data)
        {
            byte[] uncompressedData = Inflate(data);

            int offset = 0;
            int samples = BitConverter.ToInt32(uncompressedData, offset);
            DateTime[] times = new DateTime[samples];

            offset += sizeof(int);

            for (int i = 0; i < samples; i++)
            {
                times[i] = new DateTime(BitConverter.ToInt64(uncompressedData, offset));
                offset += sizeof(long);
            }

            int seriesIndex = 0;

            while (offset < uncompressedData.Length)
            {
                int seriesID = BitConverter.ToInt32(uncompressedData, offset);

                offset += sizeof(int);

                for (int i = 0; i < samples; i++)
                {
                    yield return new DataPoint()
                    {
                        SeriesID = seriesID,
                        Characteristic = "Instantaneous",
                        Time = times[i],
                        Value = BitConverter.ToDouble(uncompressedData, offset)
                    };

                    offset += sizeof(double);
                }

                seriesIndex++;
            }
        }

        private static byte[] Inflate(byte[] data)
        {
            using (MemoryStream stream = new MemoryStream(data))
            using (GZipStream inflater = new GZipStream(stream, CompressionMode.Decompress))
            using (MemoryStream result = new MemoryStream())
            {
                inflater.CopyTo(result);
                return result.ToArray();
            }
        }

        private static SqlDouble ToSqlDouble(double num)
        {
            const double Sentinel = -1.0e308;

            return (double.IsNaN(num) || double.IsInfinity(num))
                ? Sentinel
                : num;
        }
    }
}
