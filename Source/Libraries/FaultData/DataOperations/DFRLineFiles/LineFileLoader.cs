//******************************************************************************************************
//  LineFileLoader.cs - Gbtc
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
//  03/08/2017 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using FaultData.DataSets;
using GSF.Data;
using GSF.Data.Model;
using log4net;
using openXDA.Model;

namespace FaultData.DataOperations.DFRLineFiles
{
    public class LineFileLoader : DataOperationBase<MeterDataSet>
    {
        #region [ Members ]

        // Fields
        private List<int> m_updatedSeriesIDs;
        private AdoDataConnection m_connection;

        #endregion

        #region [ Properties ]

        [Setting]
        [DefaultValue("LineConfiguration")]
        public string LineConfigurationDirectories { get; set; }

        [Setting]
        [DefaultValue("")]
        public string LineConfigurationIgnoreDirectories { get; set; }

        #endregion

        #region [ Constructors ]

        public LineFileLoader()
        {
            m_updatedSeriesIDs = new List<int>();
        }

        #endregion

        #region [ Methods ]

        public override void Execute(MeterDataSet meterDataSet)
        {
            LineConfiguration lineConfiguration;
            BreakerConfiguration breakerConfiguration;
            string[] lineConfigurationDirectories;
            string[] lineConfigurationIgnoreDirectories;

            using (m_connection = meterDataSet.CreateDbConnection())
            {
                TableOperations<Meter> meterTable = new TableOperations<Meter>(m_connection);
                Meter meter = meterTable.QueryRecordWhere("AssetKey = {0}", meterDataSet.Meter.AssetKey);

                if ((object)meter == null)
                {
                    Log.Warn($"No configuration found for meter with asset key {meterDataSet.Meter.AssetKey} -- exiting LineFileLoader.");
                    return;
                }

                meter.ConnectionFactory = () => new AdoDataConnection(m_connection.Connection, m_connection.AdapterType, false);

                lineConfigurationDirectories = LineConfigurationDirectories
                    .Split(Path.PathSeparator)
                    .Select(dir => dir.Trim())
                    .ToArray();

                lineConfigurationIgnoreDirectories = LineConfigurationIgnoreDirectories
                    .Split(Path.PathSeparator)
                    .Select(dir => dir.Trim())
                    .ToArray();

                try
                {
                    lineConfiguration = new LineConfiguration(meterDataSet, lineConfigurationDirectories, lineConfigurationIgnoreDirectories);
                }
                catch (InvalidOperationException ex)
                {
                    Log.Error($"Error occurred processing line configuration file for meter '{meter.AssetKey}': {ex.Message}", ex);
                    return;
                }

                foreach (LineConfiguration.LineSection lineSection in lineConfiguration.LineSections)
                {
                    string lineID = lineSection.LineID;
                    string breaker1 = lineSection.Breaker1;
                    string breaker2 = lineSection.Breaker2;
                    bool lineIDValid = !string.IsNullOrEmpty(lineID) && !lineID.All(c => c == '0');
                    bool breaker1Valid = !string.IsNullOrEmpty(breaker1) && !breaker1.All(c => c == '0');
                    bool breaker2Valid = !string.IsNullOrEmpty(breaker2) && !breaker2.All(c => c == '0');

                    if (!lineIDValid && (!breaker1Valid || breaker2Valid))
                        continue;

                    if (!lineIDValid)
                        lineID = string.Concat("BR", breaker1);

                    UpdateLine(meter, lineID, lineSection);

                    foreach (LineConfiguration.ChannelInfo channelInfo in lineSection.Channels)
                        UpdateChannel(meter, lineID, channelInfo);

                    foreach (LineConfiguration.DigitalInfo digitalInfo in lineSection.Digitals)
                        UpdateDigital(meter, lineID, digitalInfo, null);

                    if (!string.IsNullOrEmpty(lineSection.Breaker1))
                    {
                        breakerConfiguration = new BreakerConfiguration(meterDataSet, lineSection.Breaker1);

                        foreach (LineConfiguration.DigitalInfo digitalInfo in breakerConfiguration.Channels)
                            UpdateDigital(meter, lineID, digitalInfo, lineSection.Breaker1);
                    }

                    if (!string.IsNullOrEmpty(lineSection.Breaker2))
                    {
                        breakerConfiguration = new BreakerConfiguration(meterDataSet, lineSection.Breaker2);

                        foreach (LineConfiguration.DigitalInfo digitalInfo in breakerConfiguration.Channels)
                            UpdateDigital(meter, lineID, digitalInfo, lineSection.Breaker2);
                    }
                }

                if (m_updatedSeriesIDs.Count > 0)
                {
                    string seriesIDList = string.Join(",", m_updatedSeriesIDs);
                    m_connection.ExecuteNonQuery($"UPDATE Series SET SourceIndexes = 'NONE' WHERE (SELECT MeterID FROM Channel WHERE ID = ChannelID) = {{0}} AND ID NOT IN ({seriesIDList})", meter.ID);
                }
            }
        }

        private void UpdateLine(Meter meter, string lineID, LineConfiguration.LineSection lineSection)
        {
            TableOperations<Line> lineTable = new TableOperations<Line>(m_connection);
            Line line = lineTable.QueryRecordWhere("AssetKey = {0}", lineID);
            double voltageKV = GetVoltageKV(lineSection.LineName);

            if (line is null)
            {
                line = new Line()
                {
                    AssetKey = lineID,
                    AssetName = lineSection.LineName,
                    VoltageKV = voltageKV
                };

                try
                {
                    lineTable.AddNewRecord(line);
                    line.ID = m_connection.ExecuteScalar<int>("SELECT ID FROM Line WHERE AssetKey = {0}", lineID);
                }
                catch (SqlException ex)
                {
                    // Ignore errors regarding unique key constraints
                    // which can occur as a result of a race condition
                    bool isUniqueViolation = (ex.Number == 2601) || (ex.Number == 2627);

                    if (!isUniqueViolation)
                        throw;

                    line = lineTable.QueryRecordWhere("AssetKey = {0}", lineID);
                }
            }
            else if (line.VoltageKV != voltageKV)
            {
                line.VoltageKV = voltageKV;
                lineTable.UpdateRecord(line);
            }

            int assetRelationshipType = m_connection.ExecuteScalar<int>("SELECT ID FROM AssetRelationshipType WHERE Name = 'Line-LineSegment'");
            int lineSegmentID;

            using (DataTable table = m_connection.RetrieveData("SELECT ChildID FROM AssetRelationship WHERE AssetRelationshipTypeID = {0} AND ParentID = {1}", assetRelationshipType, line.ID))
            {
                if (table.Rows.Count > 1)
                    throw new InvalidOperationException("Detected multiple line segments on line managed by DFR line file");

                lineSegmentID = table
                    .AsEnumerable()
                    .Select(row => row.ConvertField<int>("ChildID"))
                    .SingleOrDefault();
            }

            TableOperations <LineSegment> lineSegmentTable = new TableOperations<LineSegment>(m_connection);
            LineSegment lineSegment = lineSegmentTable.QueryRecordWhere("ID = {0}", lineSegmentID);
            MeterAsset meterAsset = null;

            if (lineSegment is null)
            {
                string assetKey = $"{line.AssetKey}-Segment1";

                lineSegment = new LineSegment()
                {
                    AssetKey = assetKey,
                    AssetName = $"{lineSection.LineName} - Segment 1",
                    Length = lineSection.Length,
                    R0 = lineSection.R0,
                    X0 = lineSection.X0,
                    R1 = lineSection.R1,
                    X1 = lineSection.X1,
                    IsEnd = true
                };

                try
                {
                    lineSegmentTable.AddNewRecord(lineSegment);
                    lineSegment.ID = m_connection.ExecuteScalar<int>("SELECT ID FROM Asset WHERE AssetKey = {0}", assetKey);

                    AssetConnection assetConnection = new AssetConnection()
                    {
                        AssetRelationshipTypeID = assetRelationshipType,
                        ParentID = line.ID,
                        ChildID = lineSegment.ID
                    };

                    TableOperations<AssetConnection> assetConnectionTable = new TableOperations<AssetConnection>(m_connection);
                    assetConnectionTable.AddNewRecord(assetConnection);

                    meterAsset = new MeterAsset()
                    {
                        MeterID = meter.ID,
                        AssetID = line.ID
                    };

                    TableOperations<MeterAsset> meterAssetTable = new TableOperations<MeterAsset>(m_connection);
                    meterAssetTable.AddNewRecord(meterAsset);
                    meterAsset.ID = m_connection.ExecuteScalar<int>("SELECT ID FROM MeterAsset WHERE MeterID = {0} AND AssetID = {1}", meter.ID, line.ID);
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
            else if (lineSegment.Length != lineSection.Length || lineSegment.R0 != lineSection.R0 || lineSegment.X0 != lineSection.X0 || lineSegment.R1 != lineSection.R1 || lineSegment.X1 != lineSection.X1)
            {
                lineSegment.Length = lineSection.Length;
                lineSegment.R0 = lineSection.R0;
                lineSegment.X0 = lineSection.X0;
                lineSegment.R1 = lineSection.R1;
                lineSegment.X1 = lineSection.X1;
                lineSegmentTable.UpdateRecord(lineSegment);
            }

            if (meterAsset is null)
            {
                TableOperations<MeterAsset> meterAssetTable = new TableOperations<MeterAsset>(m_connection);
                meterAsset = meterAssetTable.QueryRecordWhere("MeterID = {0} AND AssetID = {1}", meter.ID, line.ID);
            }

            TableOperations<FaultDetectionLogic> faultDetectionLogicTable = new TableOperations<FaultDetectionLogic>(m_connection);
            FaultDetectionLogic faultDetectionLogic = faultDetectionLogicTable.QueryRecordWhere("MeterAssetID = {0}", meterAsset.ID);

            if (faultDetectionLogic is null)
            {
                faultDetectionLogic = new FaultDetectionLogic()
                {
                    MeterAssetID = meterAsset.ID,
                    Expression = lineSection.FaultLogic
                };

                faultDetectionLogicTable.AddNewRecord(faultDetectionLogic);
            }
            else if (faultDetectionLogic.Expression != lineSection.FaultLogic)
            {
                faultDetectionLogic.Expression = lineSection.FaultLogic;
                faultDetectionLogicTable.UpdateRecord(faultDetectionLogic);
            }
        }

        private void UpdateChannel(Meter meter, string lineID, LineConfiguration.ChannelInfo channelInfo)
        {
            TableOperations<MeasurementType> measurementTypeTable = new TableOperations<MeasurementType>(m_connection);
            MeasurementType measurementType = measurementTypeTable.QueryRecordWhere("Name = {0}", channelInfo.MeasurementType);

            if ((object)measurementType == null)
            {
                measurementType = new MeasurementType()
                {
                    Name = channelInfo.MeasurementType,
                    Description = channelInfo.MeasurementType
                };

                try
                {
                    measurementTypeTable.AddNewRecord(measurementType);
                    measurementType.ID = m_connection.ExecuteScalar<int>("SELECT ID FROM MeasurementType WHERE Name = {0}", channelInfo.MeasurementType);
                }
                catch (SqlException ex)
                {
                    // Ignore errors regarding unique key constraints
                    // which can occur as a result of a race condition
                    bool isUniqueViolation = (ex.Number == 2601) || (ex.Number == 2627);

                    if (!isUniqueViolation)
                        throw;

                    measurementType = measurementTypeTable.QueryRecordWhere("Name = {0}", channelInfo.MeasurementType);
                }
            }

            TableOperations<MeasurementCharacteristic> measurementCharacteristicTable = new TableOperations<MeasurementCharacteristic>(m_connection);
            MeasurementCharacteristic measurementCharacteristic = measurementCharacteristicTable.QueryRecordWhere("Name = 'Instantaneous'");

            if ((object)measurementCharacteristic == null)
            {
                measurementCharacteristic = new MeasurementCharacteristic()
                {
                    Name = "Instantaneous",
                    Description = "Instantaneous"
                };

                try
                {
                    measurementCharacteristicTable.AddNewRecord(measurementCharacteristic);
                    measurementCharacteristic.ID = m_connection.ExecuteScalar<int>("SELECT ID FROM MeasurementCharacteristic WHERE Name = 'Instantaneous'");
                }
                catch (SqlException ex)
                {
                    // Ignore errors regarding unique key constraints
                    // which can occur as a result of a race condition
                    bool isUniqueViolation = (ex.Number == 2601) || (ex.Number == 2627);

                    if (!isUniqueViolation)
                        throw;

                    measurementCharacteristic = measurementCharacteristicTable.QueryRecordWhere("Name = 'Instantaneous'");
                }
            }

            TableOperations<Phase> phaseTable = new TableOperations<Phase>(m_connection);
            Phase phase = phaseTable.QueryRecordWhere("Name = {0}", channelInfo.Phase);

            if ((object)phase == null)
            {
                phase = new Phase()
                {
                    Name = channelInfo.Phase,
                    Description = channelInfo.Phase
                };

                try
                {
                    phaseTable.AddNewRecord(phase);
                    phase.ID = m_connection.ExecuteScalar<int>("SELECT ID FROM Phase WHERE Name = {0}", channelInfo.Phase);
                }
                catch (SqlException ex)
                {
                    // Ignore errors regarding unique key constraints
                    // which can occur as a result of a race condition
                    bool isUniqueViolation = (ex.Number == 2601) || (ex.Number == 2627);

                    if (!isUniqueViolation)
                        throw;

                    phase = phaseTable.QueryRecordWhere("Name = {0}", channelInfo.Phase);
                }
            }

            TableOperations<SeriesType> seriesTypeTable = new TableOperations<SeriesType>(m_connection);
            SeriesType seriesType = seriesTypeTable.QueryRecordWhere("Name = 'Values'");

            if ((object)seriesType == null)
            {
                seriesType = new SeriesType()
                {
                    Name = "Values",
                    Description = "Values"
                };

                try
                {
                    seriesTypeTable.AddNewRecord(seriesType);
                    seriesType.ID = m_connection.ExecuteScalar<int>("SELECT ID FROM SeriesType WHERE Name = 'Values'");
                }
                catch (SqlException ex)
                {
                    // Ignore errors regarding unique key constraints
                    // which can occur as a result of a race condition
                    bool isUniqueViolation = (ex.Number == 2601) || (ex.Number == 2627);

                    if (!isUniqueViolation)
                        throw;

                    seriesType = seriesTypeTable.QueryRecordWhere("Name = 'Values'");
                }
            }

            TableOperations<Channel> channelTable = new TableOperations<Channel>(m_connection);
            TableOperations<Series> seriesTable = new TableOperations<Series>(m_connection);

            Series series = meter.Channels
                .Where(ch => ch.Asset.AssetKey == lineID)
                .Where(ch => ch.MeasurementTypeID == measurementType.ID)
                .Where(ch => ch.MeasurementCharacteristicID == measurementCharacteristic.ID)
                .Where(ch => ch.PhaseID == phase.ID)
                .SelectMany(channel => channel.Series)
                .Where(s => s.SeriesTypeID == seriesType.ID)
                .FirstOrDefault();

            if ((object)series == null)
            {
                Channel channel = new Channel()
                {
                    MeterID = meter.ID,
                    AssetID = m_connection.ExecuteScalar<int>("SELECT ID FROM Asset WHERE AssetKey = {0}", lineID),
                    MeasurementTypeID = measurementType.ID,
                    MeasurementCharacteristicID = measurementCharacteristic.ID,
                    PhaseID = phase.ID,
                    Name = channelInfo.Name,
                    Description = channelInfo.Description,
                    HarmonicGroup = 0
                };

                channelTable.AddNewRecord(channel);
                channel.ID = m_connection.ExecuteScalar<int>("SELECT @@IDENTITY");

                series = new Series()
                {
                    ChannelID = channel.ID,
                    SeriesTypeID = seriesType.ID,
                    SourceIndexes = channelInfo.Index.ToString()
                };

                seriesTable.AddNewRecord(series);
                series.ID = m_connection.ExecuteScalar<int>("SELECT @@IDENTITY");

                // Set collection of channels to null so
                // it can be recomputed on next access
                meter.Channels = null;
            }
            else if (series.Channel.Name != channelInfo.Name || series.Channel.Description != channelInfo.Description || series.SourceIndexes != channelInfo.Index.ToString())
            {
                Channel channel = series.Channel;
                channel.Name = channelInfo.Name;
                channel.Description = channelInfo.Description;
                channelTable.UpdateRecord(channel);

                series.SourceIndexes = channelInfo.Index.ToString();
                seriesTable.UpdateRecord(series);
            }

            m_updatedSeriesIDs.Add(series.ID);
        }

        private void UpdateDigital(Meter meter, string lineID, LineConfiguration.DigitalInfo digitalInfo, string breakerNumber)
        {
            TableOperations<MeasurementType> measurementTypeTable = new TableOperations<MeasurementType>(m_connection);
            MeasurementType measurementType = measurementTypeTable.QueryRecordWhere("Name = 'Digital'");

            if ((object)measurementType == null)
            {
                measurementType = new MeasurementType()
                {
                    Name = "Digital",
                    Description = "Digital"
                };

                try
                {
                    measurementTypeTable.AddNewRecord(measurementType);
                    measurementType.ID = m_connection.ExecuteScalar<int>("SELECT ID FROM MeasurementType WHERE Name = 'Digital'");
                }
                catch (SqlException ex)
                {
                    // Ignore errors regarding unique key constraints
                    // which can occur as a result of a race condition
                    bool isUniqueViolation = (ex.Number == 2601) || (ex.Number == 2627);

                    if (!isUniqueViolation)
                        throw;

                    measurementType = measurementTypeTable.QueryRecordWhere("Name = 'Digital'");
                }
            }

            TableOperations<MeasurementCharacteristic> measurementCharacteristicTable = new TableOperations<MeasurementCharacteristic>(m_connection);
            MeasurementCharacteristic measurementCharacteristic = measurementCharacteristicTable.QueryRecordWhere("Name = {0}", digitalInfo.MeasurementCharacteristic);

            if ((object)measurementCharacteristic == null)
            {
                measurementCharacteristic = new MeasurementCharacteristic()
                {
                    Name = digitalInfo.MeasurementCharacteristic,
                    Description = digitalInfo.MeasurementCharacteristicDescription
                };

                try
                {
                    measurementCharacteristicTable.AddNewRecord(measurementCharacteristic);
                    measurementCharacteristic.ID = m_connection.ExecuteScalar<int>("SELECT ID FROM MeasurementCharacteristic WHERE Name = {0}", digitalInfo.MeasurementCharacteristic);
                }
                catch (SqlException ex)
                {
                    // Ignore errors regarding unique key constraints
                    // which can occur as a result of a race condition
                    bool isUniqueViolation = (ex.Number == 2601) || (ex.Number == 2627);

                    if (!isUniqueViolation)
                        throw;

                    measurementCharacteristic = measurementCharacteristicTable.QueryRecordWhere("Name = {0}", digitalInfo.MeasurementCharacteristic);
                }
            }

            TableOperations<Phase> phaseTable = new TableOperations<Phase>(m_connection);
            Phase phase = phaseTable.QueryRecordWhere("Name = 'None'");

            if ((object)phase == null)
            {
                phase = new Phase()
                {
                    Name = "None",
                    Description = "None"
                };

                try
                {
                    phaseTable.AddNewRecord(phase);
                    phase.ID = m_connection.ExecuteScalar<int>("SELECT ID FROM Phase WHERE Name = 'None'");
                }
                catch (SqlException ex)
                {
                    // Ignore errors regarding unique key constraints
                    // which can occur as a result of a race condition
                    bool isUniqueViolation = (ex.Number == 2601) || (ex.Number == 2627);

                    if (!isUniqueViolation)
                        throw;

                    phase = phaseTable.QueryRecordWhere("Name = 'None'");
                }
            }

            TableOperations<SeriesType> seriesTypeTable = new TableOperations<SeriesType>(m_connection);
            SeriesType seriesType = seriesTypeTable.QueryRecordWhere("Name = 'Values'");

            if ((object)seriesType == null)
            {
                seriesType = new SeriesType()
                {
                    Name = "Values",
                    Description = "Values"
                };

                try
                {
                    seriesTypeTable.AddNewRecord(seriesType);
                    seriesType.ID = m_connection.ExecuteScalar<int>("SELECT ID FROM SeriesType WHERE Name = 'Values'");
                }
                catch (SqlException ex)
                {
                    // Ignore errors regarding unique key constraints
                    // which can occur as a result of a race condition
                    bool isUniqueViolation = (ex.Number == 2601) || (ex.Number == 2627);

                    if (!isUniqueViolation)
                        throw;

                    seriesType = seriesTypeTable.QueryRecordWhere("Name = 'Values'");
                }
            }

            TableOperations<Channel> channelTable = new TableOperations<Channel>(m_connection);
            TableOperations<Series> seriesTable = new TableOperations<Series>(m_connection);

            Series series = meter.Channels
                .Where(ch => ch.Asset.AssetKey == lineID)
                .Where(ch => ch.MeasurementTypeID == measurementType.ID)
                .SelectMany(channel => channel.Series)
                .Where(s => s.SourceIndexes == digitalInfo.Index.ToString())
                .FirstOrDefault();

            if ((object)series == null)
            {
                Channel channel = new Channel()
                {
                    MeterID = meter.ID,
                    AssetID = m_connection.ExecuteScalar<int>("SELECT ID FROM Asset WHERE AssetKey = {0}", lineID),
                    MeasurementTypeID = measurementType.ID,
                    MeasurementCharacteristicID = measurementCharacteristic.ID,
                    PhaseID = phase.ID,
                    Name = digitalInfo.Name,
                    Description = digitalInfo.Description,
                    HarmonicGroup = 0
                };

                channelTable.AddNewRecord(channel);
                channel.ID = m_connection.ExecuteScalar<int>("SELECT @@IDENTITY");

                series = new Series()
                {
                    ChannelID = channel.ID,
                    SeriesTypeID = seriesType.ID,
                    SourceIndexes = digitalInfo.Index.ToString(),
                };

                seriesTable.AddNewRecord(series);
                series.ID = m_connection.ExecuteScalar<int>("SELECT @@IDENTITY");

                // Set collection of channels to null so
                // it can be recomputed on next access
                meter.Channels = null;
            }
            else if (series.Channel.Name != digitalInfo.Name || series.Channel.Description != digitalInfo.Description || series.Channel.MeasurementType.ID != measurementType.ID || series.Channel.MeasurementCharacteristic.ID != measurementCharacteristic.ID)
            {
                Channel channel = series.Channel;
                channel.MeasurementTypeID = measurementType.ID;
                channel.MeasurementCharacteristicID = measurementCharacteristic.ID;
                channel.Name = digitalInfo.Name;
                channel.Description = digitalInfo.Description;
                channelTable.UpdateRecord(channel);
            }

            if (!string.IsNullOrEmpty(breakerNumber))
            {
                TableOperations<BreakerChannel> breakerChannelTable = new TableOperations<BreakerChannel>(m_connection);
                BreakerChannel breakerChannel = breakerChannelTable.QueryRecordWhere("ChannelID = {0}", series.ChannelID);

                if ((object)breakerChannel == null)
                {
                    breakerChannel = new BreakerChannel()
                    {
                        ChannelID = series.ChannelID,
                        BreakerNumber = breakerNumber
                    };

                    breakerChannelTable.AddNewRecord(breakerChannel);
                }
                else if (breakerChannel.BreakerNumber != breakerNumber)
                {
                    breakerChannel.BreakerNumber = breakerNumber;
                    breakerChannelTable.UpdateRecord(breakerChannel);
                }
            }

            m_updatedSeriesIDs.Add(series.ID);
        }

        private double GetVoltageKV(string lineName)
        {
            const string LineNamePattern = @"(?<VoltageKV>[0-9]+)\s*KV";
            Match match = Regex.Match(lineName, LineNamePattern, RegexOptions.IgnoreCase);
            double voltageKV;

            if (match.Success && double.TryParse(match.Groups["VoltageKV"].Value, out voltageKV))
                return voltageKV;

            return 0.0D;
        }

        #endregion

        #region [ Static ]

        // Static Fields
        private readonly ILog Log = LogManager.GetLogger(typeof(LineFileLoader));

        #endregion
    }
}
