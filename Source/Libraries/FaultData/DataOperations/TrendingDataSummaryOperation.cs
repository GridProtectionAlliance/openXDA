//******************************************************************************************************
//  TrendingDataSummaryOperation.cs - Gbtc
//
//  Copyright © 2015, Grid Protection Alliance.  All Rights Reserved.
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
//  05/05/2015 - J. Ritchie Carroll
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using FaultData.Configuration;
using FaultData.DataResources;
using FaultData.DataSets;
using GSF.Configuration;
using log4net;
using openHistorian.XDALink;
using openXDA.Model;

namespace FaultData.DataOperations
{
    public class TrendingDataSummaryOperation : DataOperationBase<MeterDataSet>
    {
        #region [ Members ]

        // Fields
        private HistorianSettings m_historianSettings;

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Creates a new <see cref="TrendingDataSummaryOperation"/>.
        /// </summary>
        public TrendingDataSummaryOperation()
        {
            m_historianSettings = new HistorianSettings();
        }

        #endregion

        #region [ Properties ]

        [Category]
        [SettingName("Historian")]
        public HistorianSettings HistorianSettings
        {
            get
            {
                return m_historianSettings;
            }
        }

        #endregion

        #region [ Methods ]

        public override void Execute(MeterDataSet meterDataSet)
        {
            Log.Info("Executing operation to load trending summary data into the openHistorian...");

            ImportedMeasurementsTable importedMeasurementsTable = null;

            if ((m_historianSettings.SecurePassword?.Length ?? 0) > 0)
            {
                string url = m_historianSettings.URL;
                string username = m_historianSettings.InstanceName;
                string password = m_historianSettings.Password;
                importedMeasurementsTable = new ImportedMeasurementsTable(url, username, password);
            }

            using (Historian historian = new Historian(m_historianSettings.Server, m_historianSettings.InstanceName))
            {
                Dictionary<Channel, List<TrendingDataSummaryResource.TrendingDataSummary>> trendingDataSummaries = meterDataSet.GetResource<TrendingDataSummaryResource>().TrendingDataSummaries;

                // Import measurement metadata into the historian metadata container
                if ((object)importedMeasurementsTable != null)
                {
                    List<ImportedMeasurement> measurements = new List<ImportedMeasurement>();

                    foreach (Channel channel in trendingDataSummaries.Keys)
                    {
                        int channelID = channel.ID;

                        foreach (SeriesID seriesID in Enum.GetValues(typeof(SeriesID)))
                        {
                            long pointID = ToPointID(channelID, seriesID);
                            ImportedMeasurement measurement = importedMeasurementsTable.FindByID(pointID).FirstOrDefault();

                            if ((object)measurement == null)
                            {
                                Func<string, int, string> truncate = (str, len) => (str.Length > len) ? str.Substring(0, len) : str;

                                Meter meter = meterDataSet.Meter;
                                MeterLocation meterLocation = meter.MeterLocation;

                                string measurementType = channel.MeasurementType.Name;
                                string measurementCharacteristic = channel.MeasurementCharacteristic.Name;
                                string phase = channel.Phase.Name;
                                string seriesType = seriesID.ToString();

                                string pointTag = $"{meter.AssetKey}:{measurementCharacteristic}";
                                string alternateTag = channel.Name;
                                string signalTypeAcronym = measurementCharacteristic.ToUpper();
                                string signalReference = $"{meter.AssetKey}-{measurementCharacteristic}";

                                measurement = new ImportedMeasurement()
                                {
                                    SignalID = Guid.NewGuid(),
                                    PointID = pointID,
                                    PointTag = truncate($"{meter.AssetKey}:{measurementCharacteristic}", 200),
                                    AlternateTag = truncate(channel.Name, 200),
                                    SignalTypeAcronym = truncate(measurementCharacteristic, 4).ToUpper(),
                                    SignalReference = truncate($"{meter.AssetKey}-{measurementCharacteristic}", 200),
                                    ProtocolAcronym = "TREND",
                                    ProtocolType = "Trending",
                                    Phase = phase[0],
                                    Description = $"{seriesType} {measurementCharacteristic} ({phase} {measurementType})",
                                    Enabled = true
                                };

                                if (meterLocation.Latitude != 0.0D && meterLocation.Longitude != 0.0D)
                                {
                                    measurement.Longitude = meterLocation.Longitude;
                                    measurement.Latitude = meterLocation.Latitude;
                                }

                                measurements.Add(measurement);
                            }
                        }
                    }

                    importedMeasurementsTable.ImportMeasurements(measurements);
                }

                foreach (KeyValuePair<Channel, List<TrendingDataSummaryResource.TrendingDataSummary>> channelSummaries in trendingDataSummaries)
                {
                    // Reduce data set to valid summaries
                    IEnumerable<TrendingDataSummaryResource.TrendingDataSummary> validSummaries = channelSummaries.Value.Where(summary => summary.IsValid);
                    int channelID = channelSummaries.Key.ID;

                    foreach (TrendingDataSummaryResource.TrendingDataSummary summary in validSummaries)
                    {
                        // Write minimum series value
                        if (!summary.IsDuplicate)
                            historian.Write(channelID, SeriesID.Minimum, summary.Time, summary.Minimum);

                        // Write maximum series value
                        if (!summary.IsDuplicate)
                            historian.Write(channelID, SeriesID.Maximum, summary.Time, summary.Maximum);

                        // Write average series value
                        if (!summary.IsDuplicate)
                            historian.Write(channelID, SeriesID.Average, summary.Time, summary.Average);
                    }
                }

                // Wait for queue
                // to be processed
                historian.Flush();
            }
        }

        private long ToPointID(int channelID, SeriesID seriesID)
        {
            ulong pointID = Historian.ToPointID(channelID, (int)seriesID);
            return unchecked((long)pointID);
        }

        #endregion

        #region [ Static ]

        // Static Fields
        private static readonly ILog Log = LogManager.GetLogger(typeof(TrendingDataSummaryOperation));

        #endregion        
    }
}
