//******************************************************************************************************
//  EMAXReader.cs - Gbtc
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
//  12/15/2014 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FaultData.DataAnalysis;
using FaultData.Database;
using FaultData.DataSets;
using GSF.EMAX;
using GSF.IO;

namespace FaultData.DataReaders
{
    public class EMAXReader : IDataReader, IDisposable
    {
        #region [ Members ]

        // Fields
        private CorrectiveParser m_parser;
        private bool m_disposed;

        #endregion

        #region [ Methods ]

        public bool CanParse(string filePath, DateTime fileCreationTime)
        {
            string directory = FilePath.GetDirectoryName(filePath);
            string rootFileName = FilePath.GetFileNameWithoutExtension(filePath);
            string controlFileName = Path.Combine(directory, rootFileName + ".ctl");

            if (!File.Exists(controlFileName))
                return false;

            try
            {
                m_parser = new CorrectiveParser();
                m_parser.ControlFile = new ControlFile(controlFileName);
                m_parser.FileName = filePath;
                m_parser.OpenFiles();
            }
            catch (IOException)
            {
                return false;
            }

            return true;
        }

        public MeterDataSet Parse(string filePath)
        {
            MeterDataSet meterDataSet;

            ControlFile controlFile;
            string identityString;
            string deviceName;

            Channel channel;
            DataSeries series;

            meterDataSet = new MeterDataSet();

            controlFile = m_parser.ControlFile;
            identityString = controlFile.IdentityString.value;
            deviceName = identityString.Substring(0, IndexOf(identityString, "\r\n", "\n", "\r"));

            meterDataSet.Meter = new Meter();
            meterDataSet.Meter.AssetKey = deviceName;
            meterDataSet.Meter.Name = deviceName;
            meterDataSet.Meter.ShortName = deviceName.Substring(0, Math.Min(deviceName.Length, 50));

            foreach (ANLG_CHNL_NEW analogChannel in controlFile.AnalogChannelSettings.Values)
            {
                channel = ParseSeries(analogChannel);

                series = new DataSeries();
                series.SeriesInfo = channel.Series[0];

                meterDataSet.Meter.Channels.Add(channel);

                while (meterDataSet.DataSeries.Count <= analogChannel.ChannelNumber)
                    meterDataSet.DataSeries.Add(new DataSeries());

                meterDataSet.DataSeries[analogChannel.ChannelNumber] = series;
            }

            while (m_parser.ReadNext())
            {
                int i = 0;

                foreach (int channelNumber in controlFile.AnalogChannelSettings.Keys.OrderBy(key => key))
                {
                    meterDataSet.DataSeries[channelNumber].DataPoints.Add(new DataPoint() { Time = m_parser.CalculatedTimestamp, Value = m_parser.CorrectedValues[i] });
                    i++;
                }
            }

            return meterDataSet;
        }

        public void Dispose()
        {
            if (!m_disposed)
            {
                try
                {
                    if ((object)m_parser != null)
                    {
                        m_parser.Dispose();
                        m_parser = null;
                    }
                }
                finally
                {
                    m_disposed = true;
                }
            }
        }

        private Channel ParseSeries(ANLG_CHNL_NEW analogChannel)
        {
            Channel channel = new Channel();
            Series series = new Series();

            channel.Name = analogChannel.title;
            channel.HarmonicGroup = 0;
            channel.MeasurementType = new MeasurementType();
            channel.MeasurementType.Name = "Unknown";
            channel.MeasurementCharacteristic = new MeasurementCharacteristic();
            channel.MeasurementCharacteristic.Name = "Unknown";
            channel.Phase = new Phase();
            channel.Phase.Name = "Unknown";

            series.Channel = channel;
            series.SeriesType = new SeriesType();
            series.SeriesType.Name = "Instantaneous";
            series.SourceIndexes = analogChannel.chanlnum;

            return channel;
        }

        private int IndexOf(string input, params string[] searchStrings)
        {
            int index = -1;
            int i = 0;

            while (index < 0 && i < searchStrings.Length)
                index = input.IndexOf(searchStrings[i++], StringComparison.Ordinal);

            return index;
        }

        #endregion
    }
}
