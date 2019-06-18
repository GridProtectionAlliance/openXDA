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
using System.ComponentModel;
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
    public class COMTRADEReader : IDataReader, IDisposable
    {
        #region [ Members ]

        // Fields
        private bool m_disposed;

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Creates a new instance of the <see cref="COMTRADEReader"/> class.
        /// </summary>
        public COMTRADEReader()
        {
            Settings = new COMTRADESettings();
            MeterDataSet = new MeterDataSet();
        }

        #endregion

        #region [ Properties ]

        [Category]
        [SettingName("COMTRADE")]
        public COMTRADESettings Settings { get; }

        /// <summary>
        /// Gets the data set produced by the Parse method of the data reader.
        /// </summary>
        public MeterDataSet MeterDataSet { get; }

        private Parser Parser { get; set; }

        #endregion

        #region [ Methods ]

        public bool CanParse(string filePath, DateTime fileCreationTime)
        {
            string schemaFilePath = Path.ChangeExtension(filePath, "cfg");
            string infFilePath = Path.ChangeExtension(filePath, "inf");
            string extension = FilePath.GetExtension(filePath);
            string[] fileList = FilePath.GetFileList(Path.ChangeExtension(filePath, "*"));
            bool multipleDataFiles = !extension.Equals(".dat", StringComparison.OrdinalIgnoreCase);
            bool minWaitTimePassed = (DateTime.UtcNow - fileCreationTime) >= Settings.MinWaitTime;

            if (!File.Exists(schemaFilePath))
                return false;

            if (Settings.WaitForINF && !minWaitTimePassed && !File.Exists(infFilePath))
                return false;

            if (fileList.Any(file => !FilePath.TryGetReadLockExclusive(file)))
                return false;

            if (multipleDataFiles && !minWaitTimePassed)
                return false;

            try
            {
                Parser = new Parser();
                Parser.Schema = new Schema(schemaFilePath, Settings.UseRelaxedValidation);
                Parser.FileName = filePath;
                Parser.InferTimeFromSampleRates = true;
                Parser.AdjustToUTC = false;
                Parser.OpenFiles();
            }
            catch (IOException)
            {
                return false;
            }

            return true;
        }

        public void Parse(string filePath)
        {
            Schema schema = Parser.Schema;

            Meter meter = new Meter();
            meter.MeterLocation = new MeterLocation();
            meter.Channels = new List<Channel>();
            meter.AssetKey = schema.DeviceID;
            meter.Name = schema.DeviceID;
            meter.ShortName = schema.DeviceID.Substring(0, Math.Min(schema.DeviceID.Length, 50));

            MeterLocation meterLocation = meter.MeterLocation;
            meterLocation.Meters = new List<Meter>() { meter };
            meterLocation.AssetKey = schema.StationName;
            meterLocation.Name = schema.StationName;
            meterLocation.ShortName = schema.StationName.Substring(0, Math.Min(schema.StationName.Length, 50));
            meterLocation.Description = schema.StationName;

            foreach (AnalogChannel analogChannel in schema.AnalogChannels)
            {
                Channel channel = ParseSeries(analogChannel);
                channel.Meter = meter;

                DataSeries dataSeries = new DataSeries();
                dataSeries.SeriesInfo = channel.Series[0];

                meter.Channels.Add(channel);

                while (MeterDataSet.DataSeries.Count <= analogChannel.Index)
                    MeterDataSet.DataSeries.Add(new DataSeries());

                MeterDataSet.DataSeries[analogChannel.Index] = dataSeries;
            }

            foreach (DigitalChannel digitalChannel in schema.DigitalChannels)
            {
                Channel channel = ParseSeries(digitalChannel);
                channel.Meter = meter;

                DataSeries dataSeries = new DataSeries();
                dataSeries.SeriesInfo = channel.Series[0];

                meter.Channels.Add(channel);

                while (MeterDataSet.Digitals.Count <= digitalChannel.Index)
                    MeterDataSet.Digitals.Add(new DataSeries());

                MeterDataSet.Digitals[digitalChannel.Index] = dataSeries;
            }

            try
            {
                while (Parser.ReadNext())
                {
                    for (int i = 0; i < schema.AnalogChannels.Length; i++)
                    {
                        int seriesIndex = schema.AnalogChannels[i].Index;
                        string units = schema.AnalogChannels[i].Units.ToUpper();
                        double multiplier = (units.Contains("KA") || units.Contains("KV")) ? 1000.0D : 1.0D;
                        MeterDataSet.DataSeries[seriesIndex].DataPoints.Add(new DataPoint() { Time = Parser.Timestamp, Value = multiplier * Parser.PrimaryValues[i] });
                    }

                    for (int i = 0; i < schema.DigitalChannels.Length; i++)
                    {
                        int valuesIndex = schema.TotalAnalogChannels + i;
                        int seriesIndex = schema.DigitalChannels[i].Index;
                        MeterDataSet.Digitals[seriesIndex].DataPoints.Add(new DataPoint() { Time = Parser.Timestamp, Value = Parser.Values[valuesIndex] });
                    }
                }
            }
            catch (InvalidOperationException ex)
            {
                Log.Warn(ex.Message, ex);
            }

            MeterDataSet.Meter = meter;

            string infFilePath = Path.ChangeExtension(filePath, "inf");

            if (File.Exists(infFilePath))
            {
                IniFile infFile = new IniFile(infFilePath);
                INFDataSet infDataSet = new INFDataSet(infFile);
                MeterDataSet.GetResource(() => new BreakerRestrikeResource(infDataSet));
            }
        }

        public void Dispose()
        {
            if (!m_disposed)
            {
                try
                {
                    if ((object)Parser != null)
                    {
                        Parser.Dispose();
                        Parser = null;
                    }
                }
                finally
                {
                    m_disposed = true;
                }
            }
        }

        private Channel ParseSeries(AnalogChannel analogChannel)
        {
            string phaseName = !string.IsNullOrEmpty(analogChannel.PhaseID) ? analogChannel.PhaseID : "Unknown";

            Series series = new Series();
            series.Channel = new Channel();
            series.SeriesType = new SeriesType() { Name = "Values" };
            series.SourceIndexes = analogChannel.Index.ToString();

            Channel channel = series.Channel;
            channel.Series = new List<Series>() { series };
            channel.MeasurementType = new MeasurementType() { Name = "Unknown" };
            channel.MeasurementCharacteristic = new MeasurementCharacteristic() { Name = "Unknown" };
            channel.Phase = new Phase() { Name = phaseName };
            channel.Name = analogChannel.Name;
            channel.Description = analogChannel.CircuitComponent;
            channel.HarmonicGroup = 0;

            return channel;
        }

        private Channel ParseSeries(DigitalChannel digitalChannel)
        {
            string phaseName = !string.IsNullOrEmpty(digitalChannel.PhaseID) ? digitalChannel.PhaseID : "Unknown";

            Series series = new Series();
            series.Channel = new Channel();
            series.SeriesType = new SeriesType() { Name = "Values" };
            series.SourceIndexes = digitalChannel.Index.ToString();

            Channel channel = series.Channel;
            channel.Series = new List<Series>() { series };
            channel.MeasurementType = new MeasurementType() { Name = "Digital" };
            channel.MeasurementCharacteristic = new MeasurementCharacteristic() { Name = "Unknown" };
            channel.Phase = new Phase() { Name = phaseName };
            channel.Name = digitalChannel.Name;
            channel.Description = digitalChannel.CircuitComponent;
            channel.HarmonicGroup = 0;

            return channel;
        }

        #endregion

        #region [ Static ]

        // Static Fields
        private static readonly ILog Log = LogManager.GetLogger(typeof(COMTRADEReader));

        #endregion
    }
}
