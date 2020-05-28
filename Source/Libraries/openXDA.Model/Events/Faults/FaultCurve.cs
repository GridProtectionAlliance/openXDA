//******************************************************************************************************
//  FaultCurve.cs - Gbtc
//
//  Copyright © 2017, Grid Protection Alliance.  All Rights Reserved.
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
//  09/06/2017 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using GSF;
using GSF.Data.Model;
using Ionic.Zlib;

namespace openXDA.Model
{
    public class FaultCurve
    {
        [PrimaryKey(true)]
        public int ID { get; set; }

        public int EventID { get; set; }

        [StringLength(200)]
        public string Algorithm { get; set; }

        public byte[] Data { get; set; }

        public byte[] AngleData { get; set; }

        #region [Private Class]
        private class DataPoint
        {
            public DateTime Time;
            public double Value;
        }

        #endregion

        #region [Methods]

        public void Adjust(Ticks ticks)
        {
            // If the blob contains the GZip header,
            // move from Legacy Compression to normal Compression
            if (this.Data[0] == 0x1F && this.Data[1] == 0x8B)
            {
                this.Data = MigrateCompression(this.Data);
            }

            // If the blob contains the GZip header,
            // move from Legacy Compression to normal Compression
            if (this.AngleData[0] == 0x1F && this.AngleData[1] == 0x8B)
            {
                this.AngleData = MigrateCompression(this.AngleData)
            }

            this.Data = ChangeTS(this.Data, ticks);
            this.AngleData = ChangeTS(this.AngleData, ticks);


        }

        private static byte[] ChangeTS(byte[] data, Ticks ticks)
        {
            data[0] = 0x1F;
            data[1] = 0x8B;

            byte[] uncompressedData = GZipStream.UncompressBuffer(data);
            byte[] resultData = new byte[uncompressedData.Length];

            uncompressedData.CopyTo(resultData,0);

            int offset = 0;

            int m_samples = LittleEndian.ToInt32(uncompressedData, offset);
            offset += sizeof(int);

            int timeValues = LittleEndian.ToInt32(uncompressedData, offset);

            int startTS = offset;

            offset += sizeof(int);

            long currentValue = LittleEndian.ToInt64(uncompressedData, offset);

            DateTime startTime = new DateTime(currentValue);
            startTime = startTime.AddTicks(ticks);

            LittleEndian.CopyBytes(startTime.Ticks, resultData, startTS);

            resultData = GZipStream.CompressBuffer(resultData);
            resultData[0] = 0x44;
            resultData[1] = 0x33;
            return resultData;
        }

        private static byte[] MigrateCompression(byte[] data)
        {
            byte[] uncompressedData;
            int offset;
            DateTime[] times;
            List<DataPoint> series;
            int seriesID = 0;

            uncompressedData = GZipStream.UncompressBuffer(data);
            offset = 0;

            int m_samples = LittleEndian.ToInt32(uncompressedData, offset);
            offset += sizeof(int);

            times = new DateTime[m_samples];

            for (int i = 0; i < m_samples; i++)
            {
                times[i] = new DateTime(LittleEndian.ToInt64(uncompressedData, offset));
                offset += sizeof(long);
            }

            series = new List<DataPoint>();

            while (offset < uncompressedData.Length)
            {
                
                seriesID = LittleEndian.ToInt32(uncompressedData, offset);
                offset += sizeof(int);

              
                for (int i = 0; i < m_samples; i++)
                {
                    series.Add(new DataPoint()
                    {
                        Time = times[i],
                        Value = LittleEndian.ToDouble(uncompressedData, offset)
                    });

                    offset += sizeof(double);
                }
            }

            var timeSeries = series.Select(dataPoint => new { Time = dataPoint.Time.Ticks, Compressed = false }).ToList();

            for (int i = 1; i < timeSeries.Count; i++)
            {
                long previousTimestamp = series[i - 1].Time.Ticks;
                long timestamp = timeSeries[i].Time;
                long diff = timestamp - previousTimestamp;

                if (diff >= 0 && diff <= ushort.MaxValue)
                    timeSeries[i] = new { Time = diff, Compressed = true };


            }

            int timeSeriesByteLength = timeSeries.Sum(obj => obj.Compressed ? sizeof(ushort) : sizeof(int) + sizeof(long));
            int dataSeriesByteLength = sizeof(int) + (2 * sizeof(double)) + (m_samples * sizeof(ushort));
            int totalByteLength = sizeof(int) + timeSeriesByteLength + dataSeriesByteLength;


            byte[] result = new byte[totalByteLength];
            offset = 0;

            offset += LittleEndian.CopyBytes(m_samples, result, offset);

            List<int> uncompressedIndexes = timeSeries
                .Select((obj, Index) => new { obj.Compressed, Index })
                .Where(obj => !obj.Compressed)
                .Select(obj => obj.Index)
                .ToList();

            for (int i = 0; i < uncompressedIndexes.Count; i++)
            {
                int index = uncompressedIndexes[i];
                int nextIndex = (i + 1 < uncompressedIndexes.Count) ? uncompressedIndexes[i + 1] : timeSeries.Count;

                offset += LittleEndian.CopyBytes(nextIndex - index, result, offset);
                offset += LittleEndian.CopyBytes(timeSeries[index].Time, result, offset);

                for (int j = index + 1; j < nextIndex; j++)
                    offset += LittleEndian.CopyBytes((ushort)timeSeries[j].Time, result, offset);
            }

            const ushort NaNValue = ushort.MaxValue;
            const ushort MaxCompressedValue = ushort.MaxValue - 1;
            double range = series.Select(item => item.Value).Max() - series.Select(item => item.Value).Min();
            double decompressionOffset = series.Select(item => item.Value).Min();
            double decompressionScale = range / MaxCompressedValue;
            double compressionScale = (decompressionScale != 0.0D) ? 1.0D / decompressionScale : 0.0D;

            offset += LittleEndian.CopyBytes(seriesID, result, offset);
            offset += LittleEndian.CopyBytes(decompressionOffset, result, offset);
            offset += LittleEndian.CopyBytes(decompressionScale, result, offset);

            foreach (DataPoint dataPoint in series)
            {
                ushort compressedValue = (ushort)Math.Round((dataPoint.Value - decompressionOffset) * compressionScale);

                if (compressedValue == NaNValue)
                    compressedValue--;

                if (double.IsNaN(dataPoint.Value))
                    compressedValue = NaNValue;

                offset += LittleEndian.CopyBytes(compressedValue, result, offset);
            }

            byte[] returnArray = GZipStream.CompressBuffer(result);
            returnArray[0] = 0x44;
            returnArray[1] = 0x33;

            return returnArray;

        }
        #endregion
    }

    public class FaultCurveStatistic
    {
        [PrimaryKey(true)]
        public int ID { get; set; }

        public int FaultCurveID { get; set; }

        public int FaultNumber { get; set; }

        public double Maximum { get; set; }

        public double Minimum { get; set; }
        
        public double Average { get; set; }

        public double StandardDeviation { get; set; }
    }
}
