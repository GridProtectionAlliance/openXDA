//******************************************************************************************************
//  ChannelData.cs - Gbtc
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
//  12/12/2019 - C. Lackner
//       Generated original version of source code.
//
//******************************************************************************************************


using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using GSF;
using GSF.Data;
using GSF.Data.Model;
using Ionic.Zlib;

namespace openXDA.Model
{
    [TableName("ChannelData")]
    public class ChannelData
    {
        #region [ Members ]

        // Nested Types
        private class DataPoint
        {
            public DateTime Time;
            public double Value;
        }

        #endregion
 
        #region [ Properties ]

        [PrimaryKey(true)]
        public int ID { get; set; }

        public int SeriesID { get; set; }

        public int EventID { get; set; }

        public byte[] TimeDomainData { get; set; }

        public int MarkedForDeletion { get; set; }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Adjusts the TimeDomain Data by Moving it a certain ammount of Time
        /// </summary>
        /// <param name="ticks"> The number of Ticks the Data is moved. For moving it backwards in Time this needs to be < 0 </param>
        public void AdjustData(Ticks ticks)
        {
            // Initially we assume Data is already migrated...
            if (TimeDomainData == null)
                return;

            Tuple<int, List<DataPoint>> decompressed = Decompress(TimeDomainData)[0];
            List<DataPoint> data = decompressed.Item2;

            foreach (DataPoint dataPoint in data)
                dataPoint.Time = dataPoint.Time.AddTicks(ticks);

            TimeDomainData = ToData(data, decompressed.Item1);
        }

        #endregion

        #region [ Static ]

        public static List<byte[]> DataFromEvent(int eventID, Func<AdoDataConnection> connectionFactory)
        {
            using (AdoDataConnection connection = connectionFactory())
            {
                TableOperations<Event> eventTable = new TableOperations<Event>(connection);
                Event evt = eventTable.QueryRecordWhere("ID = {0}", eventID);

                TableOperations<Asset> assetTable = new TableOperations<Asset>(connection);
                Asset asset = assetTable.QueryRecordWhere("ID = {0}", evt.AssetID);
                asset.ConnectionFactory = connectionFactory;

                List<Channel> channels = asset.DirectChannels
                    .Concat(asset.ConnectedChannels)
                    .ToList();

                if (!channels.Any())
                    return new List<byte[]>();

                IEnumerable<int> assetIDs = channels
                    .Select(channel => channel.AssetID)
                    .Distinct();

                foreach (int assetID in assetIDs)
                    MigrateLegacyBlob(connection, evt.FileGroupID, assetID, evt.StartTime);

                List<byte[]> eventData = new List<byte[]>();

                foreach (Channel channel in channels)
                {
                    const string DataQueryFormat =
                        "SELECT ChannelData.TimeDomainData " +
                        "FROM " +
                        "    ChannelData JOIN " +
                        "    Series ON ChannelData.SeriesID = Series.ID JOIN " +
                        "    Event ON ChannelData.EventID = Event.ID " +
                        " WHERE " +
                        "    Event.FileGroupID = {0} AND " +
                        "    Series.ChannelID = {1} AND " +
                        "    Event.StartTime = {2}";

                    object startTime2 = ToDateTime2(connection, evt.StartTime);
                    byte[] timeDomainData = connection.ExecuteScalar<byte[]>(DataQueryFormat, evt.FileGroupID, channel.ID, startTime2);

                    if (timeDomainData is null)
                        continue;

                    eventData.Add(timeDomainData);
                }

                return eventData;
            }
        }

        public static byte[] DataFromEvent(int eventID, int channelID, Func<AdoDataConnection> connectionFactory)
        {
            using (AdoDataConnection connection = connectionFactory())
            {
                TableOperations<Event> eventTable = new TableOperations<Event>(connection);
                Event evt = eventTable.QueryRecordWhere("ID = {0}", eventID);
                MigrateLegacyBlob(connection, evt);

                const string QueryFormat =
                    "SELECT ChannelData.TimeDomainData " +
                    "FROM " +
                    "    ChannelData JOIN " +
                    "    Series ON ChannelData.SeriesID = Series.ID " +
                    "WHERE " +
                    "    ChannelData.EventID = {0} AND " +
                    "    Series.ChannelID = {1}";

                return connection.ExecuteScalar<byte[]>(QueryFormat, eventID, channelID);
            }
        }

        private static void MigrateLegacyBlob(AdoDataConnection connection, int fileGroupID, int assetID, DateTime startTime)
        {
            const string AssetQueryFilter = "FileGroupID = {0} AND AssetID = {1} AND StartTime = {2}";
            object startTime2 = ToDateTime2(connection, startTime);

            TableOperations<Event> eventTable = new TableOperations<Event>(connection);
            Event evt = eventTable.QueryRecordWhere(AssetQueryFilter, fileGroupID, assetID, startTime2);
            MigrateLegacyBlob(connection, evt);
        }

        private static void MigrateLegacyBlob(AdoDataConnection connection, Event evt)
        {
            if (evt is null || evt.EventDataID is null)
                return;

            int eventDataID = evt.EventDataID.GetValueOrDefault();
            byte[] timeDomainData = connection.ExecuteScalar<byte[]>("SELECT TimeDomainData FROM EventData WHERE ID = {0}", eventDataID);
            List<Tuple<int, List<DataPoint>>> decompressedData = Decompress(timeDomainData);

            TableOperations<ChannelData> channelDataTable = new TableOperations<ChannelData>(connection);

            foreach (Tuple<int, List<DataPoint>> tuple in decompressedData)
            {
                int seriesID = tuple.Item1;
                List<DataPoint> data = tuple.Item2;

                ChannelData channelData = new ChannelData();
                channelData.SeriesID = seriesID;
                channelData.EventID = evt.ID;
                channelData.TimeDomainData = ToData(data, seriesID);

                try
                {
                    channelDataTable.AddNewRecord(channelData);
                }
                catch (SqlException ex)
                {
                    // Ignore errors regarding unique key constraints
                    // which can occur as a result of a race condition
                    bool isUniqueViolation = (ex.Number == 2601) || (ex.Number == 2627);

                    if (!isUniqueViolation)
                        throw;
                }
            }

            connection.ExecuteNonQuery("UPDATE Event SET EventDataID = NULL WHERE ID = {0}", evt.ID);
            connection.ExecuteNonQuery("DELETE FROM EventData WHERE ID = {0}", eventDataID);
        }

        private static byte[] ToData(List<DataPoint> data, int seriesID)
        {
            var timeSeries = data.Select(dataPoint => new { Time = dataPoint.Time.Ticks, Compressed = false }).ToList();

            for (int i = 1; i < timeSeries.Count; i++)
            {
                long previousTimestamp = data[i - 1].Time.Ticks;
                long timestamp = timeSeries[i].Time;
                long diff = timestamp - previousTimestamp;

                if (diff >= 0 && diff <= ushort.MaxValue)
                    timeSeries[i] = new { Time = diff, Compressed = true };
            }

            int timeSeriesByteLength = timeSeries.Sum(obj => obj.Compressed ? sizeof(ushort) : sizeof(int) + sizeof(long));
            int dataSeriesByteLength = sizeof(int) + (2 * sizeof(double)) + (data.Count * sizeof(ushort));
            int totalByteLength = sizeof(int) + timeSeriesByteLength + dataSeriesByteLength;
           
            byte[] result = new byte[totalByteLength];
            int offset = 0;

            offset += LittleEndian.CopyBytes(data.Count, result, offset);

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
            double range = data.Select(item => item.Value).Max() - data.Select(item => item.Value).Min();
            double decompressionOffset = data.Select(item => item.Value).Min();
            double decompressionScale = range / MaxCompressedValue;
            double compressionScale = (decompressionScale != 0.0D) ? 1.0D / decompressionScale : 0.0D;

            offset += LittleEndian.CopyBytes(seriesID, result, offset);
            offset += LittleEndian.CopyBytes(decompressionOffset, result, offset);
            offset += LittleEndian.CopyBytes(decompressionScale, result, offset);

            foreach (DataPoint dataPoint in data)
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

        private static List<Tuple<int, List<DataPoint>>> Decompress(byte[] data)
        {
            List<Tuple<int, List<DataPoint>>> result = new List<Tuple<int, List<DataPoint>>>();

            if (data == null)
                return result;
            // If the blob contains the GZip header,
            // use the legacy deserialization algorithm
            if (data[0] == 0x1F && data[1] == 0x8B)
            {
                return Decompress_Legacy(data);
            }

            // Restore the GZip header before uncompressing
            data[0] = 0x1F;
            data[1] = 0x8B;

            byte[] uncompressedData;
            int offset;

            uncompressedData = GZipStream.UncompressBuffer(data);
            offset = 0;

            int m_samples = LittleEndian.ToInt32(uncompressedData, offset);
            offset += sizeof(int);

            List<DateTime> times = new List<DateTime>();

            while (times.Count < m_samples)
            {
                int timeValues = LittleEndian.ToInt32(uncompressedData, offset);
                offset += sizeof(int);

                long currentValue = LittleEndian.ToInt64(uncompressedData, offset);
                offset += sizeof(long);
                times.Add(new DateTime(currentValue));

                for (int i = 1; i < timeValues; i++)
                {
                    currentValue += LittleEndian.ToUInt16(uncompressedData, offset);
                    offset += sizeof(ushort);
                    times.Add(new DateTime(currentValue));
                }
            }

            while (offset < uncompressedData.Length)
            {
                List<DataPoint> dataSeries = new List<DataPoint>();
                int seriesID = LittleEndian.ToInt32(uncompressedData, offset);
                offset += sizeof(int);


                const ushort NaNValue = ushort.MaxValue;
                double decompressionOffset = LittleEndian.ToDouble(uncompressedData, offset);
                double decompressionScale = LittleEndian.ToDouble(uncompressedData, offset + sizeof(double));
                offset += 2 * sizeof(double);

                for (int i = 0; i < m_samples; i++)
                {
                    ushort compressedValue = LittleEndian.ToUInt16(uncompressedData, offset);
                    offset += sizeof(ushort);

                    double decompressedValue = decompressionScale * compressedValue + decompressionOffset;

                    if (compressedValue == NaNValue)
                        decompressedValue = double.NaN;

                    dataSeries.Add(new DataPoint()
                    {
                        Time = times[i],
                        Value = decompressedValue
                    });
                }
               

                    result.Add(new Tuple<int, List<DataPoint>>(seriesID, dataSeries));
            }

            return result;
        }

        private static List<Tuple<int, List<DataPoint>>> Decompress_Legacy(byte[] data)
        {
            List<Tuple<int, List<DataPoint>>> result = new List<Tuple<int, List<DataPoint>>>();
            byte[] uncompressedData;
            int offset;
            DateTime[] times;
            int seriesID;

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

            while (offset < uncompressedData.Length)
            {
                seriesID = LittleEndian.ToInt32(uncompressedData, offset);
                offset += sizeof(int);

                List<DataPoint> points = new List<DataPoint>();

                for (int i = 0; i < m_samples; i++)
                {
                    points.Add(new DataPoint()
                    {
                        Time = times[i],
                        Value = LittleEndian.ToDouble(uncompressedData, offset)
                    });

                    offset += sizeof(double);
                }

                result.Add(new Tuple<int, List<DataPoint>>(seriesID, points));
            }
            return result;
        }

        private static object ToDateTime2(AdoDataConnection connection, DateTime dateTime)
        {
            using (IDbCommand command = connection.Connection.CreateCommand())
            {
                IDbDataParameter parameter = command.CreateParameter();
                parameter.DbType = DbType.DateTime2;
                parameter.Value = dateTime;
                return parameter;
            }
        }

        #endregion
    }
}
