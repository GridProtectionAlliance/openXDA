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


using Gemstone;
using Gemstone.Data;
using Gemstone.Data.Model;
using System;
using System.Collections.Generic;
using System.Data;
using Ionic.Zlib;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace OpenXDA.Model
{
    [TableName("ChannelData")]
    public class ChannelData
    {
        #region [Private Class]
        private class DataPoint
        {
            public DateTime Time;
            public double Value;
        }

        #endregion
 
        #region [Properties]
        [PrimaryKey(true)]
        public int ID { get; set; }

        public int FileGroupID { get; set; }

        public int RunTimeID { get; set; }

        public byte[] TimeDomainData { get; set; }

        // Not sure we need this but it was in EventData
        public int MarkedForDeletion { get; set; }

        //This should be ChannelID not really....
        public int SeriesID { get; set; }

        public int EventID { get; set; }

        // This is for backwards compatibility so we can point to data that is still in a EventDataBlob.
        // As we pull up the data it will be moved out but if this is the first time Calling the ChannelData Blob
        // it just points back to the eventdata Blob.
        public int? EventDataID { get; set; }

        #endregion

        #region [Statics]

        // This is going through this function to migtrate all EventdataBlobs over to ChannelDataBlobs as they are read eventually removing the legacy table (eventData)
        public static List<byte[]> DataFromEvent(int eventID, AdoDataConnection connection)
        {
            List<byte[]> result = new List<byte[]>();
            List<int> directChannelIDs = new List<int>();

            //This Should start by getting multiple datasets
            string query = "SELECT SeriesID FROM ChannelData WHERE EventID = {0}";
            DataTable table = connection.RetrieveData(query, eventID);

            foreach(DataRow row in table.Rows)
            {
                int seriesID = row.Field<int>("SeriesID");
                ChannelData channelData = new TableOperations<ChannelData>(connection).QueryRecordWhere("SeriesID = {0} AND EventID = {1}", seriesID, eventID);
                if (channelData == null) continue;
                else if (channelData.TimeDomainData == null)
                {
                    channelData.TimeDomainData = ProcessLegacyBlob(eventID, seriesID, connection);
                    if (channelData.TimeDomainData == null) continue;
                }

                directChannelIDs.Add(connection.ExecuteScalar<int>("SELECT ChannelID FROM Series WHERE ID = {0}", seriesID));
                result.Add(channelData.TimeDomainData);
            }


            //This Will get the extended Data (throught connections)....
            Asset asset = new TableOperations<Asset>(connection).QueryRecordWhere("ID = (SELECT AssetID FROM Event WHERE ID = {0})", eventID);
            asset.ConnectionFactory = () => { return new AdoDataConnection(connection.Connection.ConnectionString, typeof(SqlConnection), typeof(SqlDataAdapter)); };
            List<int> channelIDs = asset.ConnectedChannels.Select(item => item.ID).ToList();

            foreach (int channelID in channelIDs)
            {
                if (directChannelIDs.Contains(channelID))
                    continue;

                //Find any Series where Event happens at the same time and ChannelID is ChannelID 
                //-> note that this assumes any Channel is only associated with a single event at a time
                
                int channelDataID = connection.ExecuteScalar<int>("SELECT COUNT(ChannelData.ID) FROM ChannelData LEFT JOIN Event ON ChannelData.EventID = Event.ID " +
                    "LEFT JOIN Series ON ChannelData.SeriesID = Series.ID " + 
                    "WHERE(Series.ChannelID = {0}) AND(Event.MeterID = (SELECT EV.MeterID FROM Event EV WHERE EV.ID = {1})) AND " +
                    "(Event.StartTime <= (SELECT EV.EndTime FROM Event EV WHERE EV.ID = {1})) AND " +
                    "(Event.EndTime >= (SELECT EV.StartTime FROM Event EV WHERE EV.ID = {1}))", channelID, eventID);

                if (channelDataID == 0)
                    continue;

                channelDataID = connection.ExecuteScalar<int>("SELECT ChannelData.ID FROM ChannelData LEFT JOIN Event ON ChannelData.EventID = Event.ID " +
                    "LEFT JOIN Series ON ChannelData.SeriesID = Series.ID " +
                    "WHERE(Series.ChannelID = {0}) AND(Event.MeterID = (SELECT EV.MeterID FROM Event EV WHERE EV.ID = {1})) AND " +
                    "(Event.StartTime <= (SELECT EV.EndTime FROM Event EV WHERE EV.ID = {1})) AND " +
                    "(Event.EndTime >= (SELECT EV.StartTime FROM Event EV WHERE EV.ID = {1}))", channelID, eventID);

                byte[] singleSeriesData = connection.ExecuteScalar<byte[]>("SELECT TimeDomainData FROM ChannelData WHERE ID = {0}"
                        , channelDataID);

                if (singleSeriesData == null)
                {
                    ChannelData channelData = new TableOperations<ChannelData>(connection).QueryRecordWhere("ID = {0}", channelDataID); 
                    singleSeriesData = ProcessLegacyBlob(channelData.EventID, channelData.SeriesID, connection);
                }

                result.Add(singleSeriesData);


            }
        

            return result;
        }

       /// <summary>
       /// Decompress a EventData Blob, add it as multiple CHannelData Blobs and remove it from EventData
       /// </summary>
       /// <param name="eventID"> The ID of the Event</param>
       /// <param name="requestedSeriesID"> The ID of the Series actually requested. </param>
       /// <param name="connection"> An <see cref="AdoDataConnection"/>.</param>
       /// <returns> A single Channel Data Blob for the  requested Series</returns>
        private static byte[] ProcessLegacyBlob(int eventID, int requestedSeriesID, AdoDataConnection connection)
        {
            byte[] resultData = null;

            byte[] timeDomainData = connection.ExecuteScalar<byte[]>("SELECT TimeDomainData FROM EventData WHERE ID = (SELECT EventDataID FROM ChannelData WHERE SeriesID = {0} AND EventID = {1})", requestedSeriesID, eventID);
            if (timeDomainData == null) return null;

            List<Tuple<int, List<DataPoint>>> data = Decompress(timeDomainData);

            foreach( var item in data)
            {
                if (item.Item1 == requestedSeriesID)
                {
                    resultData = ToData(item.Item2, item.Item1, item.Item2.Count);
                }
                // Insert into correct ChannelData.....
                connection.ExecuteNonQuery("UPDATE ChannelData SET TimeDomainData = {0} WHERE SeriesID = {1} AND EventID = {2}", ToData(item.Item2, item.Item1, item.Item2.Count), item.Item1, eventID);
            }

            connection.ExecuteNonQuery("DELETE FROM EventData WHERE ID = (SELECT EventDataID FROM ChannelData WHERE SeriesID = {0} AND EventID = {1})", requestedSeriesID, eventID);
            connection.ExecuteNonQuery("DELETE FROM ChannelData WHERE EventID = {0} AND TimeDomainData IS NULL", eventID);
            return resultData;
        }

        private static byte[] ToData(List<DataPoint> data, int seriesID, int samples)
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
            int dataSeriesByteLength = sizeof(int) + (2 * sizeof(double)) + (samples * sizeof(ushort));
            int totalByteLength = sizeof(int) + timeSeriesByteLength + dataSeriesByteLength;

           
            byte[] result = new byte[totalByteLength];
            int offset = 0;

            offset += LittleEndian.CopyBytes(samples, result, offset);

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

        /// <summary>
        /// This Decompresses the Time Domain Data into a List of DataPoints
        /// </summary>
        /// <param name="data"> Raw data to be decompressed</param>
        /// <returns> Returns a <see cref="Tuple"/> with the Series ID and the List of <see cref="DataPoint"/> </returns>
        private static List<Tuple<int, List<DataPoint>>> Decompress(byte[] data)
        {
            List<Tuple<int, List<DataPoint>>> result = new List<Tuple<int, List<DataPoint>>>();

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

        /// <summary>
        /// This Decompresses the Time Domain Data into a List of DataPoints based on the Legacy Compression Algorithm
        /// </summary>
        /// <param name="data"> Raw data to be decompressed</param>
        /// <returns> Returns a <see cref="Tuple"/> with the Series ID and the List of <see cref="DataPoint"/> </returns>
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

        
        /// <summary>
        /// Adjusts the TimeDomain Data by Moving it a certain ammount of Time
        /// </summary>
        /// <param name="ticks"> The number of Ticks the Data is moved. For moving it backwards in Time this needs to be < 0 </param>
        public void AdjustData(Ticks ticks)
        {
            // Initially we assume Data is already migrated...
            if (this.TimeDomainData == null)
                return;

            Tuple<int,List<DataPoint>> decompressed = Decompress(this.TimeDomainData)[0];
            List<DataPoint> data = decompressed.Item2;

            foreach (DataPoint dataPoint in data)
            {
                dataPoint.Time = dataPoint.Time.AddTicks(ticks);
            }

            this.TimeDomainData = ToData(data, decompressed.Item1, data.Count);

        }

        #endregion

    }
}
