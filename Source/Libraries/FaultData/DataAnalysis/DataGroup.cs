//******************************************************************************************************
//  DataGroup.cs - Gbtc
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
//  05/19/2014 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using GSF;
using GSF.Data.Model;
using Ionic.Zlib;
using openXDA.Model;

namespace FaultData.DataAnalysis
{
    public enum DataClassification
    {
        Trend,
        Event,
        Unknown
    }

    public class DataGroup
    {
        #region [ Members ]

        // Constants
        
        /// <summary>
        /// Maximum sample rate, in samples per minute, of data classified as <see cref="DataClassification.Trend"/>.
        /// </summary>
        public const double TrendThreshold = 1.0D;

        // Fields
        private Line m_line;
        private DateTime m_startTime;
        private DateTime m_endTime;
        private int m_samples;

        private List<DataSeries> m_dataSeries;
        private DataClassification m_classification;

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Creates a new instance of the <see cref="DataGroup"/> class.
        /// </summary>
        public DataGroup()
        {
            m_dataSeries = new List<DataSeries>();
            m_classification = DataClassification.Unknown;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="DataGroup"/> class.
        /// </summary>
        /// <param name="dataSeries">Collection of data series to be added to the data group.</param>
        public DataGroup(IEnumerable<DataSeries> dataSeries)
            : this()
        {
            foreach (DataSeries series in dataSeries)
                Add(series);
        }

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets the line from which measurements were taken to create the group of data.
        /// </summary>
        public Line Line
        {
            get
            {
                return m_line;
            }
        }

        /// <summary>
        /// Gets the start time of the group of data.
        /// </summary>
        public DateTime StartTime
        {
            get
            {
                return m_startTime;
            }
        }

        /// <summary>
        /// Gets the end time of the group of data.
        /// </summary>
        public DateTime EndTime
        {
            get
            {
                return m_endTime;
            }
        }

        /// <summary>
        /// Gets the number of samples in each series.
        /// </summary>
        public int Samples
        {
            get
            {
                return m_samples;
            }
        }

        /// <summary>
        /// Gets the sample rate, in samples per second,
        /// of the data series in this data group.
        /// </summary>
        public double SamplesPerSecond
        {
            get
            {
                if (!m_dataSeries.Any())
                    return double.NaN;

                return m_dataSeries[0].SampleRate;
            }
        }

        /// <summary>
        /// Gets the sample rate, in samples per second,
        /// of the data series in this data group.
        /// </summary>
        public double Duration
        {
            get
            {
                if (!m_dataSeries.Any())
                    return double.NaN;

                return m_dataSeries[0].Duration;
            }
        }

        /// <summary>
        /// Gets the sample rate, in samples per hour,
        /// of the data series in this data group.
        /// </summary>
        public double SamplesPerHour
        {
            get
            {
                return (m_samples - 1) / (m_endTime - m_startTime).TotalHours;
            }
        }

        /// <summary>
        /// Gets the channels contained in this data group.
        /// </summary>
        public IReadOnlyList<DataSeries> DataSeries
        {
            get
            {
                return m_dataSeries.AsReadOnly();
            }
        }

        /// <summary>
        /// Gets the classification of this group of data as of the last call to <see cref="Classify()"/>.
        /// </summary>
        public DataClassification Classification
        {
            get
            {
                if (m_classification == DataClassification.Unknown)
                    Classify();

                return m_classification;
            }
        }

        public DataSeries this[int index]
        {
            get
            {
                return m_dataSeries[index];
            }
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Adds a channel to the group of data.
        /// </summary>
        /// <param name="dataSeries">The channel to be added to the group.</param>
        /// <returns>
        /// True if the channel was successfully added. False if the channel was excluded
        /// because the channel does not match the other channels already in the data group.
        /// </returns>
        public bool Add(DataSeries dataSeries)
        {
            Line line;
            DateTime startTime;
            DateTime endTime;
            int samples;

            if ((object)dataSeries == null)
                return false;

            if ((object)dataSeries.DataPoints == null)
                return false;

            if (!dataSeries.DataPoints.Any())
                return false;

            if (m_dataSeries.Contains(dataSeries))
                return false;

            if ((object)dataSeries.SeriesInfo != null)
                line = dataSeries.SeriesInfo.Channel.Line;
            else
                line = null;

            startTime = dataSeries.DataPoints[0].Time;
            endTime = dataSeries.DataPoints[dataSeries.DataPoints.Count - 1].Time;
            samples = dataSeries.DataPoints.Count;

            if (m_dataSeries.Count == 0)
            {
                m_line = line;
                m_startTime = startTime;
                m_endTime = endTime;
                m_samples = samples;

                m_dataSeries.Add(dataSeries);

                return true;
            }

            if (line == m_line && startTime == m_startTime && endTime == m_endTime && samples == m_samples)
            {
                m_dataSeries.Add(dataSeries);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Removes a channel from the data group.
        /// </summary>
        /// <param name="dataSeries">The channel to be removed from the data group.</param>
        /// <returns>True if the channel existed in the group and was removed; false otherwise.</returns>
        public bool Remove(DataSeries dataSeries)
        {
            if (m_dataSeries.Remove(dataSeries))
            {
                m_classification = DataClassification.Unknown;
                return true;
            }

            return false;
        }

        public DataGroup ToSubGroup(int startIndex, int endIndex)
        {
            DataGroup subGroup = new DataGroup();

            foreach (DataSeries dataSeries in m_dataSeries)
                subGroup.Add(dataSeries.ToSubSeries(startIndex, endIndex));

            return subGroup;
        }

        public byte[] ToData()
        {
            int timeSeriesByteLength;
            int dataSeriesByteLength;
            int totalByteLength;

            byte[] data;
            int offset;

            int seriesID;

            if (m_dataSeries.Count == 0)
                return new byte[0];

            timeSeriesByteLength = m_samples * sizeof(long);
            dataSeriesByteLength = sizeof(int) + (m_samples * sizeof(double));
            totalByteLength = sizeof(int) + timeSeriesByteLength + (dataSeriesByteLength * m_dataSeries.Count);
            data = new byte[totalByteLength];
            offset = 0;

            offset += LittleEndian.CopyBytes(m_samples, data, offset);

            foreach (DataPoint dataPoint in m_dataSeries[0].DataPoints)
                offset += LittleEndian.CopyBytes(dataPoint.Time.Ticks, data, offset);

            foreach (DataSeries series in m_dataSeries)
            {
                if ((object)series.SeriesInfo != null)
                    seriesID = series.SeriesInfo.ID;
                else
                    seriesID = 0;

                offset += LittleEndian.CopyBytes(seriesID, data, offset);

                foreach (DataPoint dataPoint in series.DataPoints)
                    offset += LittleEndian.CopyBytes(dataPoint.Value, data, offset);
            }

            return GZipStream.CompressBuffer(data);
        }

        public void FromData(byte[] data)
        {
            FromData(null, data);
        }

        public void FromData(Meter meter, byte[] data)
        {
            byte[] uncompressedData;
            int offset;
            DateTime[] times;
            DataSeries series;
            int seriesID;

            m_dataSeries.Clear();

            if (data.Length == 0)
                return;

            uncompressedData = GZipStream.UncompressBuffer(data);
            offset = 0;

            m_samples = LittleEndian.ToInt32(uncompressedData, offset);
            offset += sizeof(int);

            times = new DateTime[m_samples];

            for (int i = 0; i < m_samples; i++)
            {
                times[i] = new DateTime(LittleEndian.ToInt64(uncompressedData, offset));
                offset += sizeof(long);
            }

            while (offset < uncompressedData.Length)
            {
                series = new DataSeries();
                seriesID = LittleEndian.ToInt32(uncompressedData, offset);
                offset += sizeof(int);

                if (seriesID > 0 && (object)meter != null)
                {
                    series.SeriesInfo = meter.Channels
                        .SelectMany(channel => channel.Series)
                        .FirstOrDefault(seriesInfo => seriesInfo.ID == seriesID);
                }

                for (int i = 0; i < m_samples; i++)
                {
                    series.DataPoints.Add(new DataPoint()
                    {
                        Time = times[i],
                        Value = LittleEndian.ToDouble(uncompressedData, offset)
                    });

                    offset += sizeof(double);
                }

                Add(series);
            }
        }

        private void Classify()
        {
            if (IsTrend())
                m_classification = DataClassification.Trend;
            else if (IsEvent())
                m_classification = DataClassification.Event;
            else
                m_classification = DataClassification.Unknown;
        }

        private bool IsTrend()
        {
            double samplesPerMinute = (m_samples - 1) / (m_endTime - m_startTime).TotalMinutes;
            return samplesPerMinute <= TrendThreshold;
        }

        private bool IsEvent()
        {
            return m_dataSeries
                .Where(IsInstantaneous)
                .Where(dataSeries => (object)dataSeries.SeriesInfo != null)
                .Where(dataSeries => dataSeries.SeriesInfo.Channel.MeasurementType.Name != "Digital")
                .Any();
        }

        private bool IsInstantaneous(DataSeries dataSeries)
        {
            string characteristicName = dataSeries.SeriesInfo.Channel.MeasurementCharacteristic.Name;
            string seriesTypeName = dataSeries.SeriesInfo.SeriesType.Name;

            return (characteristicName == "Instantaneous") &&
                   (seriesTypeName == "Values" || seriesTypeName == "Instantaneous");
        }

        private string GetMeasurementType(DataSeries dataSeries)
        {
            return dataSeries.SeriesInfo.Channel.MeasurementType.Name;
        }

        private string GetPhase(DataSeries dataSeries)
        {
            return dataSeries.SeriesInfo.Channel.Phase.Name;
        }

        #endregion
    }

    public static partial class TableOperationsExtensions
    {
        public static Event GetEvent(this TableOperations<Event> eventTable, FileGroup fileGroup, DataGroup dataGroup)
        {
            int fileGroupID = fileGroup.ID;
            int lineID = dataGroup.Line.ID;
            DateTime startTime = dataGroup.StartTime;
            DateTime endTime = dataGroup.EndTime;
            int samples = dataGroup.Samples;

            IDbDataParameter startTimeParameter = new SqlParameter()
            {
                ParameterName = nameof(dataGroup.StartTime),
                DbType = DbType.DateTime2,
                Value = startTime
            };

            IDbDataParameter endTimeParameter = new SqlParameter()
            {
                ParameterName = nameof(dataGroup.EndTime),
                DbType = DbType.DateTime2,
                Value = endTime
            };

            RecordRestriction recordRestriction =
                new RecordRestriction("FileGroupID = {0}", fileGroupID) &
                new RecordRestriction("LineID = {0}", lineID) &
                new RecordRestriction("StartTime = {0}", startTimeParameter) &
                new RecordRestriction("EndTime = {0}", endTimeParameter) &
                new RecordRestriction("Samples = {0}", samples);

            return eventTable.QueryRecord(recordRestriction);
        }
    }
}
