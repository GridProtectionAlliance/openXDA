//******************************************************************************************************
//  PQubeTrendingDataCSVReader.cs - Gbtc
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
using System.Text;
using System.Text.RegularExpressions;
using FaultData.DataAnalysis;
using FaultData.DataSets;
using GSF;
using GSF.IO;
using log4net;
using openXDA.Model;

namespace FaultData.DataReaders
{
    /// <summary>
    /// Reads a PQubeTrendingDataCSVReader file to produce a <see cref="MeterDataSet"/>.
    /// </summary>
    public class PQubeTrendingDataCSVReader : IDataReader, IDisposable
    {
        #region [ Members ]

        // Fields
        private MeterDataSet m_meterDataSet;
        private bool m_disposed;

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Creates a new instance of the <see cref="PQubeTrendingDataCSVReader"/> class.
        /// </summary>
        public PQubeTrendingDataCSVReader()
        {
            m_meterDataSet = new MeterDataSet();
            Channels = new Dictionary<string, int>();
        }

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets the data set produced by the Parse method of the data reader.
        /// </summary>
        public MeterDataSet MeterDataSet
        {
            get
            {
                return m_meterDataSet;
            }
        }

        private Dictionary<string, int> Channels { get; set; }
        #endregion

        #region [ Methods ]

        public bool CanParse(string filePath, DateTime fileCreationTime)
        {
            if (!FilePath.TryGetReadLockExclusive(filePath))
                return false;

            return true;
        }

        public void Parse(string filePath)
        {
            Meter meter = new Meter();
            meter.MeterLocation = new MeterLocation();
            meter.Channels = new List<Channel>();

            MeterLocation meterLocation = meter.MeterLocation;
            meterLocation.Meters = new List<Meter>() { meter };      

            try
            {
                // open file and read first line
                StreamReader file = new StreamReader(filePath);
                string line = file.ReadLine();

                // if first line is not header columns move down until you read the header columns
                while (!line.Contains(","))
                    line = file.ReadLine();

                // build channels and dataseries from header columns
                string[] headers = CSVDecode(line);
                for(int i = 1; i < headers.Length; ++i)
                {
                    if (headers[i].ToLower() == "flag"|| headers[i].ToLower() == "nothing") continue;

                    Channel channel = ParseSeries(headers[i]);
                    channel.Meter = meter;

                    DataSeries dataSeries = new DataSeries();
                    dataSeries.SeriesInfo = channel.Series[0];
                    meter.Channels.Add(channel);
                    m_meterDataSet.DataSeries.Add(dataSeries);
                    Channels.Add(headers[i], m_meterDataSet.DataSeries.Count - 1);
                }

                while ((line = file.ReadLine()) != null)
                {
                    string[] data = CSVDecode(line);
                    string timeStamp = data[0];

                    for (int i = 1; i < headers.Length; i++)
                    {
                        if (headers[i].ToLower() == "flag" || headers[i].ToLower() == "nothing") continue;
                        m_meterDataSet.DataSeries[Channels[headers[i]]].DataPoints.Add(new DataPoint() { Time = DateTime.Parse(timeStamp), Value = double.Parse(data[i]) });
                    }

                }
            }
            catch (InvalidOperationException ex)
            {
                Log.Warn(ex.Message, ex);
            }

            m_meterDataSet.Meter = meter;
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

        private Channel ParseSeries(string channelName)
        {
            string phaseName = ParsePhase(channelName);

            Series series = new Series();
            series.Channel = new Channel();
            series.SeriesType = new SeriesType() { Name = ParseSeriesType(channelName) };

            Channel channel = series.Channel;
            channel.Series = new List<Series>() { series };
            channel.MeasurementType = new MeasurementType() { Name = ParseMeasurementType(channelName)};
            channel.MeasurementCharacteristic = new MeasurementCharacteristic() { Name = ParseMeasurementCharacteristic(channelName)};
            channel.Phase = new Phase() { Name = phaseName };
            channel.Name = FixChannelName(channelName);
            channel.Description = channelName;
            channel.HarmonicGroup = ParseHarmonicGroup(channel);

            return channel;
        }

        private string FixChannelName(string channelName) {
            channelName = channelName.Replace("-", " ");
            if (channelName.ToLower().Contains("min"))
                channelName = channelName.Replace("min", "").Replace("Min", "");
            else if (channelName.ToLower().Contains("max"))
                channelName = channelName.Replace("max", "").Replace("Max", "");
            else if (channelName.ToLower().Contains("avg"))
                channelName = channelName.Replace("avg", "").Replace("Avg", "");
            channelName = channelName.Replace("  ", " ");
            return channelName.Trim();
        }

        private string ParseSeriesType(string channelName) {
            if (channelName.ToLower().Contains("min"))
                return "Minimum";
            else if (channelName.ToLower().Contains("max"))
                return "Maximum";
            else 
                return "Average";
        }

        private string ParsePhase(string channelName)
        {
            if (channelName.Contains("L1")) return "1";
            else if (channelName.Contains("L2")) return "2";
            else if (channelName.Contains("L3")) return "3";
            else if (channelName.Contains("L4")) return "4";
            else if (channelName.Contains("LA")) return "AN";
            else if (channelName.Contains("LB")) return "BN";
            else if (channelName.Contains("LC")) return "CN";
            else return "Unknown";
        }

        private string ParseMeasurementType(string channelName) {
            if (channelName.Contains("(A)") || channelName.Contains("Current")) return "Current";
            else return "Voltage";
        }

        private string ParseMeasurementCharacteristic(string channelName)
        {
            if (channelName.Contains("Interharmonic")) return "IHRMS";
            else if (channelName.Contains("Negative Sequence Unblanace")) return "S2S1";
            else if (channelName.Contains("Zero Sequence Unblanace")) return "S0S1";
            else if (channelName.Contains("Uover")) return "Uover";
            else if (channelName.Contains("Uunder")) return "Uunder";
            else if (channelName.Contains("Flicker Pst")) return "FlkrPST";
            else if (channelName.Contains("Flicker Pinst")) return "FlkrPinst";
            else if (channelName.Contains("Flicker Plt")) return "FlkrPLT";
            else if (channelName.Contains("THD")) return "THD";
            else if (channelName.Contains("TDD")) return "TDD";

            else return "HRMS";
        }

        private int ParseHarmonicGroup(Channel channel) {
            if(!channel.MeasurementCharacteristic.Name.Contains("HRMS"))
                return 0;

            int harmonicGroup = 0;

            if (channel.Name.ToLower().Contains("harmonic"))
                harmonicGroup = int.Parse(Regex.Matches(channel.Name, "([H][0-9]+)")[0].Value.RemoveCharacter('H'));

            return harmonicGroup;
        }

        private string[] CSVDecode(string line)
        {
            List<string> fields = new List<string>();
            int i = 0;


            while (i < line.Length)
            {
                if (line[i] == '"')
                    fields.Add(ReadEscaped(ref i, line));
                else
                    fields.Add(ReadToComma(ref i, line));

                i++;
            }

            return fields.ToArray();
        }

        string ReadToComma(ref int i, string line)
        {
            StringBuilder token = new StringBuilder();

            while (i < line.Length && line[i] != ',' )
                token.Append(line[i++]);

            return token.ToString();
        }

        string ReadEscaped(ref int i, string line)
        {
            StringBuilder token = new StringBuilder();
            i++;
            while (true)
            {
                while (i < line.Length && line[i] != '"')
                    token.Append(line[i++]);

                i++;

                if (i < line.Length && line[i] == '"')
                    token.Append(line[i++]);
                else
                    break;
            }

            token.Append(ReadToComma(ref i, line));
            return token.ToString();
        }

        #endregion

        #region [ Static ]

        // Static Fields
        private static readonly ILog Log = LogManager.GetLogger(typeof(PQubeTrendingDataCSVReader));

        #endregion
    }
}
