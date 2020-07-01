//******************************************************************************************************
//  SELLDPReader.cs - Gbtc
//
//  Copyright © 2020, Grid Protection Alliance.  All Rights Reserved.
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
//  06/26/2020 - C. Lackner
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using FaultData.Configuration;
using FaultData.DataAnalysis;
using FaultData.DataResources.GTC;
using FaultData.DataSets;
using GSF.COMTRADE;
using GSF.Configuration;
using GSF.Interop;
using GSF.IO;
using log4net;
using openXDA.Model;

namespace FaultData.DataReaders
{
    /// <summary>
    /// Reads a COMTRADE file to produce a <see cref="MeterDataSet"/>.
    /// </summary>
    public class SELLDPReader : IDataReader, IDisposable
    {
        #region [ Members ]

        // Fields
        private bool m_disposed;

        #endregion

        #region [ Member Classes ]

        private class Record
        {
            public RecordType Type { get; set; }
            public byte[] Data { get; set; }
            public int Length { get; set; }
            public int Checksum { get; set; }

            public static Record PareseNext(byte[] data)
            {
                ushort recordType = Parse16Int(data,0);
                Record result = new Record();
                result.Type = (RecordType)recordType;

                ushort size = Parse16Int(data, 2);
                result.Length = size;
                result.Data = new byte[(result.Length - 2)];
                Buffer.BlockCopy(data, 4, result.Data, 0, (result.Length - 2));

                ushort checkSum = Parse16Int(data, result.Length + 2);
                result.Checksum = checkSum;

                return result;
            }

            
        }
        
        enum RecordType: ushort
        { 
            MeterConfig = 0x0064,
            MeterStatus = 0x066,
            CurrentValues = 0x0065,
            LDPData = 0x0067,
            SERData = 0x0068
        }

        private class RecordChannel
        {
            public string Name { get; set; }
            public double Scalar { get; set; }
            public double Multiplier { get; set; }
            public string Function { get; set; }
            public ChannelType Type { get; set; }
        }

        enum ChannelType: uint
        {
            CurrentPhase =	0,
            CurrentNeutral = 1,
            Voltage	= 2,
            PowerReal = 50,
            PowerReactive = 51,
            PowerApparent = 52,
            EnergyReal = 100,
            EnergyReactive = 101,
            EnergyApparent = 102,
            AmpHoursPhase = 103,
            AmpHoursNeutral = 104,
            VoltHours = 105,
            Angle = 150,
            Frequency = 151,
            Percent = 152,
            Registers =	153,
            Miscellaneous = 200
        }

    #endregion
    #region [ Constructors ]

    /// <summary>
    /// Creates a new instance of the <see cref="SELLDPReader"/> class.
    /// </summary>
        public SELLDPReader()
        {
            MeterDataSet = new MeterDataSet();
        }

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets the data set produced by the Parse method of the data reader.
        /// </summary>
        public MeterDataSet MeterDataSet { get; }

        #endregion

        #region [ Methods ]

        public bool CanParse(string filePath, DateTime fileCreationTime)
        {
            return true;
        }

        public void Parse(string filePath)
        {
            List<Record> records = new List<Record>();
            byte[] data = ReadFile(filePath);

            while(data.Length > 0)
            {
                records.Add(Record.PareseNext(data));
                data = data.Skip(records.Last().Length + 4).ToArray();
            }


            Meter meter = new Meter();
            meter.Location = new Location();
            meter.Channels = new List<Channel>();
            meter.AssetKey = "";
            meter.Name = "";
            meter.ShortName = "";

            Location meterLocation = meter.Location;
            meterLocation.Meters = new List<Meter>() { meter };
            meterLocation.LocationKey = "";
            meterLocation.Name = "";
            meterLocation.ShortName = "";
            meterLocation.Description = "";

            List<RecordChannel> channels = ReadMeterConfiguration(records[0]);

            for (int i=0; i< channels.Count; i++)
            {
                Channel channel = ParseSeries(channels[i],i);
                channel.Meter = meter;

                DataSeries dataSeries = new DataSeries();
                dataSeries.SeriesInfo = channel.Series[0];

                meter.Channels.Add(channel);

                while (MeterDataSet.DataSeries.Count <= i)
                    MeterDataSet.DataSeries.Add(new DataSeries());

                MeterDataSet.DataSeries[i] = dataSeries;
            }

            try
            {
                foreach(Record record in records)
                {
                    if (record.Type != RecordType.LDPData)
                        continue;

                    int index = 0;
                    while (index < record.Data.Count())
                    {
                        DateTime TS;

                        index++;
                        int year = Parse16Int(record.Data, index);
                        index = index + 2;
                        int day = Parse16Int(record.Data, index);
                        index = index + 2;
                        long milliseconds = Parse32Int(record.Data, index);
                        index = index + 4;

                        TS = new DateTime(year, 1, 1);
                        TS = TS + (new TimeSpan(day, 0, 0, 0)) + (new TimeSpan(milliseconds * GSF.Ticks.PerMillisecond / 10));

                        

                        for (int i = 0; i < channels.Count; i++)
                        {
                            double value = Parse32Float(record.Data, index);
                            index = index + 4;
                            MeterDataSet.DataSeries[i].DataPoints.Add(new DataPoint() { Time = TS, Value = channels[i].Multiplier * channels[i].Scalar * value });
                        }
                    }

                }
                //Parse Values and TimeStamps


                // Process 

            }
            catch (InvalidOperationException ex)
            {
                Log.Warn(ex.Message, ex);
            }

            MeterDataSet.Meter = meter;
           
        }

        public void Dispose()
        {
            if (!m_disposed)
            {
                try
                {
                   
                }
                finally
                {
                    m_disposed = true;
                }
            }
        }

       
        private byte[] ReadFile(string fileName)
        {
            string hexData = "";
            try
            {   // Open the text file using a stream reader.
                using (StreamReader sr = new StreamReader(fileName))
                {
                    // Read the stream to a string, and write the string to the console.
                    hexData = sr.ReadToEnd();
                }
            }
            catch (IOException e)
            {
                throw new IOException("Unable to open SEL Trending Data File");
            }

            byte[] data = new byte[hexData.Length / 2];

            for (int index = 0; index < data.Length; index++)
            {
                string byteValue = hexData.Substring(index * 2, 2);
                data[index] = byte.Parse(byteValue, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            }

            return data;

        }

        private List<RecordChannel> ReadMeterConfiguration(Record meterConfiguration)
        {
            List<RecordChannel> channels = new List<RecordChannel>();

            if (meterConfiguration.Type != RecordType.MeterConfig)
                return channels;

            int nChannels = Parse16Int(meterConfiguration.Data, 151);
            int index = 153;
            
            //Get Names of Channels
            for (int i =0; i < nChannels; i++)
            {
                RecordChannel channel = new RecordChannel()
                {
                    Multiplier = 1.0,
                    Scalar = 1.0,
                    Name = "",
                    Function = ""
                };

                while(meterConfiguration.Data[index] != 0x00)
                {

                    channel.Name = channel.Name + Convert.ToChar(meterConfiguration.Data[index]);
                    index++;
                }

                channels.Add(channel);
                index++;
            }

            index++;

            //Get Channel Functions
            for (int i = 0; i < nChannels; i++)
            {
                while (meterConfiguration.Data[index] != 0x00)
                {

                    channels[i].Function = channels[i].Function + Convert.ToChar(meterConfiguration.Data[index]);
                    index++;
                }

                index++;
            }
            index++;

            //Get Channel Type
            for (int i = 0; i < nChannels; i++)
            {
                channels[i].Type = (ChannelType)Parse32Int(meterConfiguration.Data, index);
                index = index + 4;
            }

            // Get Channel Multiplier
            for (int i = 0; i < nChannels; i++)
            {
                channels[i].Multiplier = Parse32Float(meterConfiguration.Data, index);
                index = index + 4;
            }

            // Get Channel Scalar
            for (int i = 0; i < nChannels; i++)
            {
                channels[i].Scalar = Parse32Float(meterConfiguration.Data, index);
                index = index + 4;
            }

            return channels;

        }

        private static ushort Parse16Int(byte[] data, int start)
        {
            byte[] adjData = new byte[2];
            if (BitConverter.IsLittleEndian)
            {
                adjData[0] = data[start + 1];
                adjData[1] = data[start];
            }
            else
            {
                adjData[1] = data[start + 1];
                adjData[0] = data[start];
            }

            return BitConverter.ToUInt16(adjData, 0);

        }

        private static uint Parse32Int(byte[] data, int start)
        {
            byte[] adjData = new byte[4];
            if (BitConverter.IsLittleEndian)
            {
                adjData[0] = data[start + 3];
                adjData[1] = data[start + 2];
                adjData[2] = data[start + 1];
                adjData[3] = data[start];
            }
            else
            {
                adjData[0] = data[start];
                adjData[1] = data[start + 1];
                adjData[2] = data[start + 2];
                adjData[3] = data[start + 3];
            }

            return BitConverter.ToUInt32(adjData, 0);

        }

        private static float Parse32Float(byte[] data, int start)
        {
            byte[] adjData = new byte[4];
            if (BitConverter.IsLittleEndian)
            {
                adjData[0] = data[start + 3];
                adjData[1] = data[start + 2];
                adjData[2] = data[start + 1];
                adjData[3] = data[start];
            }
            else
            {
                adjData[0] = data[start];
                adjData[1] = data[start + 1];
                adjData[2] = data[start + 2];
                adjData[3] = data[start + 3];
            }

            return BitConverter.ToSingle(adjData, 0);

        }

        private Channel ParseSeries(RecordChannel record, int index)
        {

            Series series = new Series();
            series.Channel = new Channel();
            series.SeriesType = new SeriesType() { Name = "Values" };
            series.SourceIndexes = (index + 1).ToString();

            Channel channel = series.Channel;
            channel.Series = new List<Series>() { series };
            channel.MeasurementType = new MeasurementType() { Name = "Unknown" };
            channel.MeasurementCharacteristic = new MeasurementCharacteristic() { Name = "Unknown" };
            channel.Phase = new Phase() { Name = "Unknown" };
            channel.Name = record.Name;
            channel.Description = record.Name;
            channel.HarmonicGroup = 0;

            return channel;
        }

        #endregion

        #region [ Static ]

        // Static Fields
        private static readonly ILog Log = LogManager.GetLogger(typeof(SELLDPReader));

        #endregion
    }
}
