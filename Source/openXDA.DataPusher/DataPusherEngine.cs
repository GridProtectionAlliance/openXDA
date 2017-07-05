//******************************************************************************************************
//  Class1.cs - Gbtc
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
//  06/30/2017 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using openXDA.Model;
using GSF.Configuration;
using GSF.Web.Model;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using GSF;

namespace openXDA.DataPusher
{
    public class DataPusherEngine : IDisposable
    {
        #region [ Members ]

        // Fields
        private string m_dbConnectionString;
        private DataContext m_dataContext;
        private bool m_dispoded;

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Creates a new instance of the <see cref="PusherEngine"/> class.
        /// </summary>
        public DataPusherEngine()
        {
            m_dbConnectionString = ConfigurationFile.Current.Settings["systemSettings"]["ConnectionString"].Value;
            m_dataContext = new DataContext("systemSettings");
        }

        #endregion

        // Client-side script functionality
        #region [ Static ]

        public static event EventHandler<EventArgs<string>> LogStatusMessageEvent;

        private static void OnLogStatusMessage(string message)
        {
            LogStatusMessageEvent?.Invoke(new object(), new EventArgs<string>(message));
        }
        #endregion

        #region [ Methods ]
        public void Dispose()
        {
            if (!m_dispoded)
            {
                try
                {
                    m_dataContext.Dispose();
                }
                catch (Exception ex)
                {
                    OnLogStatusMessage(ex.ToString());
                }
            }
        }


        // The process of syncing the local database with the remote database
        public void SyncDatabases()
        {
            //Stopwatch stopwatch = new Stopwatch();
            //stopwatch.Start();
            //int remoteMeterLocationId = 1;
            //int meterGroupId = AddMeterGroup();

            //IEnumerable<MetersToDataPush> selectedMeters = m_dataContext.Table<MetersToDataPush>().QueryRecords("LocalXDAAssetKey");

            //foreach (MetersToDataPush meter in selectedMeters)
            //{

            //    // if meter doesnt exist remotely add it
            //    //AddMeter(meter, remoteMeterLocationId);
            //    AddMeterMeterGroup(meterGroupId, meter.RemoteXDAMeterID);

            //    // get MeterLine table 
            //    List<MeterLine> localMeterLines = WebAPIHub.GetRecords("local", "MeterLine", "all").Where(x => x.MeterID == meter.LocalXDAMeterID).Select(x => (MeterLine)x).ToList();

            //    // if there is a line for the meter ensure that its data has been uploaded remotely
            //    foreach (MeterLine meterLine in localMeterLines)
            //    {

            //        LinesToDataPush selectedLine = AddLine(meterLine);

            //        // if MeterLine association has not been previously made, make it
            //        AddMeterLine(meter, selectedLine);

            //        // ensure remote and local line impedance matches
            //        SyncLineImpedances(selectedLine);

            //        // add line to meterlocationline table
            //        int meterLocationLineID = SyncMeterLocationLines(selectedLine.RemoteXDALineID, remoteMeterLocationId);

            //        // ensure remote and local Source Impedance records match for the current meter line location
            //        SyncSourceImpedance(meterLocationLineID);

            //        // Sync Channel and channel dependant data
            //        SyncChannel(meter, selectedLine);

            //        SyncEvents(meter.LocalXDAMeterID, meter.RemoteXDAMeterID, selectedLine.LocalXDALineID, selectedLine.RemoteXDALineID);
            //    }

            //    //Get Meter Alarm Summary Data
            //    SyncMeterAlarmSummary(meter);

            //    //Get Meter Data Quality Summary Data
            //    SyncMeterDataQualitySummary(meter);

            //    //Get Facility Data
            //    SyncMeterFacility(meter);

            //    Console.Write("Running Time: " + stopwatch.Elapsed);
            //    stopwatch.Stop();
            //}
        }

        public void SyncInstance(int instanceId) {

        }

        public void SyncMeterConfigurationForInstance(int instanceId, int meterId) {

            RemoteXDAInstance instance = m_dataContext.Table<RemoteXDAInstance>().QueryRecordWhere("ID = {0}", instanceId);
            MetersToDataPush meterToDataPush = m_dataContext.Table<MetersToDataPush>().QueryRecordWhere("ID = {0}", meterId);
            Meter localMeterRecord = m_dataContext.Table<Meter>().QueryRecordWhere("ID = {0}", meterToDataPush.LocalXDAMeterID);

            int remoteMeterLocationId = SyncMeterLocations(instance.Address, meterToDataPush, localMeterRecord);
            int meterGroupId = AddMeterGroup(instance.Address);

            // if meter doesnt exist remotely add it
            AddMeter(instance.Address, meterToDataPush, localMeterRecord, remoteMeterLocationId);
            AddMeterMeterGroup(instance.Address, meterGroupId, meterToDataPush.RemoteXDAMeterID);

            // get MeterLine table 
            IEnumerable<MeterLine> localMeterLines = m_dataContext.Table<MeterLine>().QueryRecordsWhere("MeterID = {0}", localMeterRecord.ID);

            // if there is a line for the meter ensure that its data has been uploaded remotely
            foreach (MeterLine meterLine in localMeterLines)
            {

                LinesToDataPush selectedLine = AddLine(instance.Address, meterLine, meterToDataPush.Obsfucate);

                // if MeterLine association has not been previously made, make it
                AddMeterLine(meterToDataPush, selectedLine);

                // ensure remote and local line impedance matches
                SyncLineImpedances(selectedLine);

                // add line to meterlocationline table
                int meterLocationLineID = SyncMeterLocationLines(selectedLine.RemoteXDALineID, remoteMeterLocationId);

                // ensure remote and local Source Impedance records match for the current meter line location
                SyncSourceImpedance(meterLocationLineID);

                // Sync Channel and channel dependant data
                SyncChannel(meterToDataPush, selectedLine);

            }
        }

        private void AddMeter(string address, MetersToDataPush meter, Meter localMeterRecord,int remoteMeterLocationId)
        {
            List<Meter> remoteMeters = WebAPIHub.GetRecords(address, "Meter", "all").Select(x => (Meter)x).ToList();

            // if meter doesnt exist remotely create the meter record
            if (!remoteMeters.Where(x => x.AssetKey.Equals(meter.RemoteXDAAssetKey.ToString())).Any())
            {
                if (meter.Obsfucate)
                {
                    Meter record = new Meter()
                    {
                        AssetKey = meter.RemoteXDAAssetKey.ToString(),
                        MeterLocationID = remoteMeterLocationId,
                        Name = meter.RemoteXDAName,
                        Alias = meter.RemoteXDAName,
                        ShortName = "",
                        Make = localMeterRecord.Make,
                        Model = localMeterRecord.Model,
                        Description = localMeterRecord.Description,
                        MeterTypeID = localMeterRecord.MeterTypeID,
                        TimeZone = localMeterRecord.TimeZone
                    };

                    meter.RemoteXDAMeterID = WebAPIHub.CreateRecord(address, "Meter", JObject.FromObject(record));
                }
                else
                {
                    localMeterRecord.MeterLocationID = remoteMeterLocationId; 
                    meter.RemoteXDAMeterID = WebAPIHub.CreateRecord(address, "Meter", JObject.FromObject(localMeterRecord));
                }

                m_dataContext.Table<MetersToDataPush>().UpdateRecord(meter);
            }
        }

        private LinesToDataPush AddLine(string address, MeterLine meterLine, bool obsfucate)
        {
            Line localLine = m_dataContext.Table<Line>().QueryRecordWhere("ID = {0}", meterLine.LineID);
            List<LinesToDataPush> selectedLines = m_dataContext.Table<LinesToDataPush>().QueryRecords("LocalXDAAssetKey").ToList();
            List<Line> remoteLines = WebAPIHub.GetRecords(address, "Line", "all").Select(x => (Line)x).ToList();

            //if the line does not exist in the PQMarkPusher Database to allow for obsfucation add it.
            if (!selectedLines.Where(x => x.LocalXDALineID == meterLine.LineID).Any())
            {
                LinesToDataPush record = new LinesToDataPush()
                {
                    LocalXDALineID = localLine.ID,
                    RemoteXDALineID = 0,
                    LocalXDAAssetKey = localLine.AssetKey,
                    RemoteXDAAssetKey = Guid.NewGuid()
                };

                Line newRecord = new Line()
                {
                    AssetKey = (obsfucate? record.RemoteXDAAssetKey.ToString() : localLine.AssetKey),
                    VoltageKV = localLine.VoltageKV,
                    ThermalRating = localLine.ThermalRating,
                    Length = localLine.Length,
                    Description = (obsfucate? "" : localLine.Description)
                };

                record.RemoteXDALineID = WebAPIHub.CreateRecord(address, "Line", JObject.FromObject(newRecord));
                m_dataContext.Table<LinesToDataPush>().AddNewRecord(record);
                return record;
            }
            else
            {
                return m_dataContext.Table<LinesToDataPush>().QueryRecordWhere("LocalXDALineID = {0}", meterLine.LineID);
            }

        }

        private int SyncMeterLocations(string address, MetersToDataPush meterToDataPush, Meter localMeterRecord)
        {
            List<MeterLocation> remoteMeterLocations = WebAPIHub.GetRecords(address, "MeterLocation", "all").Select(x => (MeterLocation)x).ToList();

            if (meterToDataPush.Obsfucate)
            {
                // if the company meter location does not exist, create it
                if (!remoteMeterLocations.Where(x => x.AssetKey == WebAPIHub.CompanyName).Any())
                {
                    MeterLocation record = new MeterLocation()
                    {
                        AssetKey = WebAPIHub.CompanyName,
                        Name = WebAPIHub.CompanyName,
                        Alias = WebAPIHub.CompanyName,
                        ShortName = "",
                        Description = "",
                        Latitude = 0.0F,
                        Longitude = 0.0F
                    };

                    return WebAPIHub.CreateRecord(address, "MeterLocation", JObject.FromObject(record));
                }
                else
                {
                    return remoteMeterLocations.Where(x => x.AssetKey == WebAPIHub.CompanyName).First().ID;
                }

            }
            else
            {
                MeterLocation meterLocation = m_dataContext.Table<MeterLocation>().QueryRecordWhere("ID = {0}", localMeterRecord.MeterLocationID);
                
                if(!remoteMeterLocations.Where(x => x.AssetKey == meterLocation.AssetKey).Any())
                    return WebAPIHub.CreateRecord(address, "MeterLocation", JObject.FromObject(meterLocation));
                else
                    return remoteMeterLocations.Where(x => x.AssetKey == meterLocation.AssetKey).First().ID;
            }
        }

        private int AddMeterGroup(string address)
        {
            List<MeterGroup> remote = WebAPIHub.GetRecords(address, "MeterGroup", "all").Select(x => (MeterGroup)x).ToList();
            // if the company meter location does not exist, create it
            if (!remote.Where(x => x.Name == WebAPIHub.CompanyName).Any())
            {
                MeterGroup record = new MeterGroup()
                {
                    Name = WebAPIHub.CompanyName
                };

                return WebAPIHub.CreateRecord(address, "MeterGroup", JObject.FromObject(record));
            }
            else
            {
                return remote.Where(x => x.Name == WebAPIHub.CompanyName).First().ID;
            }
        }

        private void AddMeterMeterGroup(string address, int meterGroupId, int meterId)
        {
            List<MeterMeterGroup> remote = WebAPIHub.GetRecordsWhere(address, "MeterMeterGroup", $"MeterID = {meterId} AND MeterGroupID = {meterGroupId}").Select(x => (MeterMeterGroup)x).ToList();

            // if MeterLine association has not been previously made, make it
            if (!remote.Any())
            {
                MeterMeterGroup record = new MeterMeterGroup()
                {
                    MeterID = meterId,
                    MeterGroupID = meterGroupId
                };

                WebAPIHub.CreateRecord(address, "MeterMeterGroup", JObject.FromObject(record));
            }
        }

        private void AddMeterLine(MetersToDataPush meter, LinesToDataPush selectedLine)
        {
            List<MeterLine> remoteMeterLines = WebAPIHub.GetRecordsWhere("remote", "MeterLine", $"MeterID = {meter.RemoteXDAMeterID} AND LineID = {selectedLine.RemoteXDALineID}").Select(x => (MeterLine)x).ToList();

            // if MeterLine association has not been previously made, make it
            if (!remoteMeterLines.Any())
            {
                MeterLine record = new MeterLine()
                {
                    MeterID = meter.RemoteXDAMeterID,
                    LineID = selectedLine.RemoteXDALineID,
                    LineName = selectedLine.RemoteXDAAssetKey.ToString()
                };

                WebAPIHub.CreateRecord("remote", "MeterLine", JObject.FromObject(record));
            }
        }

        private void SyncLineImpedances(LinesToDataPush selectedLine)
        {
            // ensure remote and local line impedance matches
            LineImpedance localLineImpedance = m_dataContext.Table<LineImpedance>().QueryRecordWhere("LineID = {0}",selectedLine.LocalXDALineID);
            LineImpedance remoteLineImpedance = (LineImpedance)WebAPIHub.GetRecordsWhere("remote", "LineImpedance", $"LineID = {selectedLine.RemoteXDALineID}").FirstOrDefault();

            // if there is a local record but not a remote record
            if (localLineImpedance != null && remoteLineImpedance == null)
            {
                JObject record = new JObject();
                record.Add("LineID", selectedLine.RemoteXDALineID);
                record.Add("R0", localLineImpedance.R0);
                record.Add("R1", localLineImpedance.R1);
                record.Add("X0", localLineImpedance.X0);
                record.Add("X1", localLineImpedance.X1);

                WebAPIHub.CreateRecord("remote", "LineImpedance", record);
            }
        }

        private int SyncMeterLocationLines(int lineID, int meterLocationID)
        {
            // add line to meterlocationline table
            dynamic remoteMeterLocationLine = WebAPIHub.GetRecordsWhere("remote", "MeterLocationLine", $"LineID = {lineID}").FirstOrDefault();
            if (remoteMeterLocationLine == null)
            {
                JObject record = new JObject();
                record.Add("LineID", lineID);
                record.Add("MeterLocationID", meterLocationID);

                WebAPIHub.CreateRecord("remote", "MeterLocationLine", record);
            }

            return WebAPIHub.GetRecords("remote", "MeterLocationLine", "all").Where(x => x.LineID == lineID).First().ID;
        }

        private void SyncSourceImpedance(int meterLocationLineID)
        {
            // ensure remote and local line impedance matches
            SourceImpedance local = m_dataContext.Table<SourceImpedance>().QueryRecordWhere("MeterLocationLineID = {0}", meterLocationLineID);
            SourceImpedance remote = (SourceImpedance)WebAPIHub.GetRecordsWhere("remote", "SourceImpedance", $"MeterLocationLineID = {meterLocationLineID}").FirstOrDefault();

            // if there is a local record but not a remote record
            if (local != null && remote == null)
            {
                JObject record = new JObject();
                record.Add("MeterLocationLineID", meterLocationLineID);
                record.Add("RSrc", local.RSrc);
                record.Add("XSrc", local.XSrc);

                WebAPIHub.CreateRecord("remote", "LineImpedance", record);
            }

        }

        private void SyncMeterAlarmSummary(MetersToDataPush meter)
        {
            // ensure remote and local line impedance matches
            List<MeterAlarmSummary> local = WebAPIHub.GetRecordsWhere("local", "MeterAlarmSummary", $"MeterID = {meter.LocalXDAMeterID}").Select(x => (MeterAlarmSummary)x).ToList();
            List<MeterAlarmSummary> remote = WebAPIHub.GetRecordsWhere("remote", "MeterAlarmSummary", $"MeterID = {meter.RemoteXDAMeterID}").Select(x => (MeterAlarmSummary)x).ToList();

            // if there is a local record but not a remote record
            foreach (MeterAlarmSummary summary in local)
            {
                if (!remote.Where(x => x.Date == summary.Date).Any())
                {
                    JObject record = new JObject();
                    record.Add("MeterID", meter.RemoteXDAMeterID);
                    record.Add("AlarmTypeID", summary.AlarmTypeID);
                    record.Add("Date", summary.Date);
                    record.Add("AlarmPoints", summary.AlarmPoints);

                    WebAPIHub.CreateRecord("remote", "MeterAlarmSummary", record);
                }
            }
        }

        private void SyncMeterDataQualitySummary(MetersToDataPush meter)
        {
            // ensure remote and local line impedance matches
            List<MeterDataQualitySummary> local = WebAPIHub.GetRecordsWhere("local", "MeterDataQualitySummary", $"MeterID = {meter.LocalXDAMeterID}").Select(x => (MeterDataQualitySummary)x).ToList();
            List<MeterDataQualitySummary> remote = WebAPIHub.GetRecordsWhere("remote", "MeterDataQualitySummary", $"MeterID = {meter.RemoteXDAMeterID}").Select(x => (MeterDataQualitySummary)x).ToList();

            // if there is a local record but not a remote record
            foreach (MeterDataQualitySummary summary in local)
            {
                if (!remote.Where(x => x.Date == summary.Date).Any())
                {
                    JObject record = new JObject();
                    record.Add("MeterID", meter.RemoteXDAMeterID);
                    record.Add("Date", summary.Date);
                    record.Add("ExpectedPoints", summary.ExpectedPoints);
                    record.Add("GoodPoints", summary.GoodPoints);
                    record.Add("LatchedPoints", summary.LatchedPoints);
                    record.Add("UnreasonablePoints", summary.UnreasonablePoints);
                    record.Add("NoncongruentPoints", summary.NoncongruentPoints);
                    record.Add("DuplicatePoints", summary.DuplicatePoints);

                    WebAPIHub.CreateRecord("remote", "MeterDataQualitySummary", record);
                }
            }

        }

        private void SyncMeterFacility(MetersToDataPush meter)
        {
            // ensure remote and local line impedance matches
            MeterFacility local = WebAPIHub.GetRecordsWhere("local", "MeterFacility", $"MeterID = {meter.LocalXDAMeterID}").Where(x => x.MeterID == meter.LocalXDAMeterID).Select(x => (MeterFacility)x).FirstOrDefault();
            MeterFacility remote = WebAPIHub.GetRecordsWhere("remote", "MeterFacility", $"MeterID = {meter.RemoteXDAMeterID}").Select(x => (MeterFacility)x).FirstOrDefault();

            // if there is a local record but not a remote record
            if (local != null && remote == null)
            {
                JObject record = new JObject();
                record.Add("MeterID", meter.RemoteXDAMeterID);
                record.Add("FacilityID", local.FacilityID);

                WebAPIHub.CreateRecord("remote", "MeterFacility", record);
            }
        }

        private void SyncChannel(MetersToDataPush meter, LinesToDataPush line)
        {
            // ensure remote and local line impedance matches
            IEnumerable<ChannelDetail> local = m_dataContext.Table<ChannelDetail>().QueryRecordsWhere("MeterID = {0} AND LineID = {1}",meter.LocalXDAMeterID,line.LocalXDALineID);
            List<ChannelDetail> remote = WebAPIHub.GetChannels("remote", $"MeterID = {meter.RemoteXDAMeterID} AND LineID = {line.RemoteXDALineID}").ToList();

            // if there is a local record but not a remote record
            foreach (ChannelDetail summary in local)
            {
                if (!remote.Where(x => x.MeasurementType == summary.MeasurementType && x.MeasurementCharacteristic == summary.MeasurementCharacteristic && x.Phase == summary.Phase && x.Name == summary.Name).Any())
                {
                    JObject record = new JObject();
                    record.Add("MeterID", meter.RemoteXDAMeterID);
                    record.Add("LineID", line.RemoteXDALineID);
                    record.Add("MeasurementType", summary.MeasurementType);
                    record.Add("MeasurementCharacteristic", summary.MeasurementCharacteristic);
                    record.Add("Phase", summary.Phase);
                    record.Add("Name", summary.Name);
                    record.Add("SamplesPerHour", summary.SamplesPerHour);
                    record.Add("PerUnitValue", summary.PerUnitValue);
                    record.Add("HarmonicGroup", summary.HarmonicGroup);
                    record.Add("Description", summary.Description);
                    record.Add("Enabled", summary.Enabled);

                    int remoteChannelId = WebAPIHub.CreateChannel("remote", record);

                    SyncSeries(summary.ID, remoteChannelId);
                    //SyncAlarmLogs(summary.ID, remoteChannelId);
                    SyncAlarmRangeLimit(summary.ID, remoteChannelId);
                    SyncBreakerChannel(summary.ID, remoteChannelId);
                    //SyncChannelAlarmSummary(summary.ID, remoteChannelId);
                    //SyncChannelDataQualitySummary(summary.ID, remoteChannelId);
                    //SyncDailyTrendingSummary(summary.ID, remoteChannelId);
                    SyncDailyQualityRangeLimit(summary.ID, remoteChannelId);
                }
            }
        }

        private void SyncSeries(int localChannelId, int remoteChannelId)
        {
            List<Series> local = WebAPIHub.GetRecordsWhere("local", "Series", $"ChannelID = {localChannelId}").Select(x => (Series)x).ToList();
            List<Series> remote = WebAPIHub.GetRecordsWhere("remote", "Series", $"ChannelID = {remoteChannelId}").Select(x => (Series)x).ToList();

            // if there is a local record but not a remote record
            foreach (Series localSeries in local)
            {
                if (!remote.Where(x => x.SeriesTypeID == localSeries.SeriesTypeID && x.SourceIndexes == localSeries.SourceIndexes).Any())
                {
                    Series record = new Series()
                    {
                        ChannelID = remoteChannelId,
                        SeriesTypeID = localSeries.SeriesTypeID,
                        SourceIndexes = localSeries.SourceIndexes
                    };

                    int remoteSeriesId = WebAPIHub.CreateRecord("remote", "Series", JObject.FromObject(record));
                    SynceOutputChannels(localSeries.ID, remoteSeriesId);

                }
                else
                {
                    int remoteSeriesId = remote.Where(x => x.SeriesTypeID == localSeries.SeriesTypeID && x.SourceIndexes == localSeries.SourceIndexes).FirstOrDefault().ID;
                    SynceOutputChannels(localSeries.ID, remoteSeriesId);
                }

            }

        }

        private void SynceOutputChannels(int localSeriesId, int remoteSeriesId)
        {
            List<OutputChannel> local = WebAPIHub.GetRecordsWhere("local", "OutputChannel", $"SeriesID = {localSeriesId}").Select(x => (OutputChannel)x).ToList();
            List<OutputChannel> remote = WebAPIHub.GetRecordsWhere("remote", "OutputChannel", $"SeriesID = {remoteSeriesId}").Select(x => (OutputChannel)x).ToList();

            // if there is a local record but not a remote record
            foreach (OutputChannel localOutputChannel in local)
            {
                if (!remote.Where(x => x.ChannelKey == localOutputChannel.ChannelKey && x.LoadOrder == localOutputChannel.LoadOrder).Any())
                {
                    OutputChannel record = new OutputChannel()
                    {
                        SeriesID = remoteSeriesId,
                        ChannelKey = localOutputChannel.ChannelKey,
                        LoadOrder = localOutputChannel.LoadOrder
                    };
                    WebAPIHub.CreateRecord("remote", "OutputChannel", JObject.FromObject(record));
                }
            }
        }

        private void SyncAlarmLogs(int localChannelId, int remoteChannelId)
        {
            List<AlarmLog> local = WebAPIHub.GetRecordsWhere("local", "AlarmLog", $"ChannelID = {localChannelId}").Select(x => (AlarmLog)x).ToList();
            List<AlarmLog> remote = WebAPIHub.GetRecordsWhere("remote", "AlarmLog", $"ChannelID = {remoteChannelId}").Select(x => (AlarmLog)x).ToList();

            // if there is a local record but not a remote record
            foreach (AlarmLog localRecord in local)
            {
                if (!remote.Where(x => x.Time == localRecord.Time).Any())
                {
                    AlarmLog record = new AlarmLog()
                    {
                        ChannelID = remoteChannelId,
                        AlarmTypeID = localRecord.AlarmTypeID,
                        Time = localRecord.Time,
                        Severity = localRecord.Severity,
                        LimitHigh = localRecord.LimitHigh,
                        LimitLow = localRecord.LimitLow,
                        Value = localRecord.Value,
                    };
                    WebAPIHub.CreateRecord("remote", "AlarmLog", JObject.FromObject(record));
                }
            }
        }

        private void SyncAlarmRangeLimit(int localChannelId, int remoteChannelId)
        {
            List<AlarmRangeLimit> local = WebAPIHub.GetRecordsWhere("local", "AlarmRangeLimit", $"ChannelID = {localChannelId}").Select(x => (AlarmRangeLimit)x).ToList();
            List<AlarmRangeLimit> remote = WebAPIHub.GetRecordsWhere("remote", "AlarmRangeLimit", $"ChannelID = {remoteChannelId}").Select(x => (AlarmRangeLimit)x).ToList();

            // if there is a local record but not a remote record
            foreach (AlarmRangeLimit localRecord in local)
            {
                if (!remote.Where(x => x.AlarmTypeID == localRecord.AlarmTypeID).Any())
                {
                    AlarmRangeLimit record = new AlarmRangeLimit()
                    {
                        ChannelID = remoteChannelId,
                        AlarmTypeID = localRecord.AlarmTypeID,
                        Severity = localRecord.Severity,
                        High = localRecord.High,
                        Low = localRecord.Low,
                        RangeInclusive = localRecord.RangeInclusive,
                        PerUnit = localRecord.PerUnit,
                        Enabled = localRecord.Enabled,
                        IsDefault = localRecord.IsDefault
                    };
                    WebAPIHub.CreateRecord("remote", "AlarmRangeLimit", JObject.FromObject(record));
                }
            }
        }

        private void SyncBreakerChannel(int localChannelId, int remoteChannelId)
        {
            List<BreakerChannel> local = WebAPIHub.GetRecordsWhere("local", "BreakerChannel", $"ChannelID = {localChannelId}").Select(x => (BreakerChannel)x).ToList();
            List<BreakerChannel> remote = WebAPIHub.GetRecordsWhere("remote", "BreakerChannel", $"ChannelID = {remoteChannelId}").Select(x => (BreakerChannel)x).ToList();

            // if there is a local record but not a remote record
            foreach (BreakerChannel localRecord in local)
            {
                if (!remote.Where(x => x.BreakerNumber == localRecord.BreakerNumber).Any())
                {
                    BreakerChannel record = new BreakerChannel()
                    {
                        ChannelID = remoteChannelId,
                        BreakerNumber = localRecord.BreakerNumber
                    };
                    WebAPIHub.CreateRecord("remote", "BreakerChannel", JObject.FromObject(record));
                }
            }
        }

        private void SyncChannelAlarmSummary(int localChannelId, int remoteChannelId)
        {
            List<ChannelAlarmSummary> local = WebAPIHub.GetRecordsWhere("local", "ChannelAlarmSummary", $"ChannelID = {localChannelId}").Select(x => (ChannelAlarmSummary)x).ToList();
            List<ChannelAlarmSummary> remote = WebAPIHub.GetRecordsWhere("remote", "ChannelAlarmSummary", $"ChannelID = {remoteChannelId}").Select(x => (ChannelAlarmSummary)x).ToList();

            // if there is a local record but not a remote record
            foreach (ChannelAlarmSummary localRecord in local)
            {
                if (!remote.Where(x => x.AlarmTypeID == localRecord.AlarmTypeID && x.Date == localRecord.Date).Any())
                {
                    ChannelAlarmSummary record = new ChannelAlarmSummary()
                    {
                        ChannelID = remoteChannelId,
                        AlarmTypeID = localRecord.AlarmTypeID,
                        Date = localRecord.Date,
                        AlarmPoints = localRecord.AlarmPoints
                    };
                    WebAPIHub.CreateRecord("remote", "ChannelAlarmSummary", JObject.FromObject(record));
                }
            }
        }

        private void SyncChannelDataQualitySummary(int localChannelId, int remoteChannelId)
        {
            List<ChannelDataQualitySummary> local = WebAPIHub.GetRecordsWhere("local", "ChannelDataQualitySummary", $"ChannelID = {localChannelId}").Select(x => (ChannelDataQualitySummary)x).ToList();
            List<ChannelDataQualitySummary> remote = WebAPIHub.GetRecordsWhere("remote", "ChannelDataQualitySummary", $"ChannelID = {remoteChannelId}").Select(x => (ChannelDataQualitySummary)x).ToList();

            // if there is a local record but not a remote record
            foreach (ChannelDataQualitySummary localRecord in local)
            {
                if (!remote.Where(x => x.Date == localRecord.Date).Any())
                {
                    ChannelDataQualitySummary record = new ChannelDataQualitySummary()
                    {
                        ChannelID = remoteChannelId,
                        Date = localRecord.Date,
                        ExpectedPoints = localRecord.ExpectedPoints,
                        GoodPoints = localRecord.GoodPoints,
                        LatchedPoints = localRecord.LatchedPoints,
                        UnreasonablePoints = localRecord.UnreasonablePoints,
                        NoncongruentPoints = localRecord.NoncongruentPoints,
                        DuplicatePoints = localRecord.DuplicatePoints,
                    };
                    WebAPIHub.CreateRecord("remote", "ChannelDataQualitySummary", JObject.FromObject(record));
                }
            }
        }

        private void SyncDailyTrendingSummary(int localChannelId, int remoteChannelId)
        {
            List<DailyTrendingSummary> local = WebAPIHub.GetRecordsWhere("local", "DailyTrendingSummary", $"ChannelID = {localChannelId}").Select(x => (DailyTrendingSummary)x).ToList();
            List<DailyTrendingSummary> remote = WebAPIHub.GetRecordsWhere("remote", "DailyTrendingSummary", $"ChannelID = {remoteChannelId}").Select(x => (DailyTrendingSummary)x).ToList();

            // if there is a local record but not a remote record
            foreach (DailyTrendingSummary localRecord in local)
            {
                if (!remote.Where(x => x.Date == localRecord.Date).Any())
                {
                    DailyTrendingSummary record = new DailyTrendingSummary()
                    {
                        ChannelID = remoteChannelId,
                        Date = localRecord.Date,
                        Minimum = localRecord.Minimum,
                        Average = localRecord.Average,
                        Maximum = localRecord.Maximum,
                        ValidCount = localRecord.ValidCount,
                        InvalidCount = localRecord.InvalidCount
                    };
                    WebAPIHub.CreateRecord("remote", "DailyTrendingSummary", JObject.FromObject(record));
                }
            }
        }

        private void SyncDailyQualityRangeLimit(int localChannelId, int remoteChannelId)
        {
            // ensure remote and local line impedance matches
            DataQualityRangeLimit local = WebAPIHub.GetRecordsWhere("local", "DataQualityRangeLimit", $"ChannelID = {localChannelId}").Select(x => (DataQualityRangeLimit)x).FirstOrDefault();
            DataQualityRangeLimit remote = WebAPIHub.GetRecordsWhere("remote", "DataQualityRangeLimit", $"ChannelID = {remoteChannelId}").Select(x => (DataQualityRangeLimit)x).FirstOrDefault();

            // if there is a local record but not a remote record
            if (local != null && remote == null)
            {
                DataQualityRangeLimit record = new DataQualityRangeLimit()
                {
                    ChannelID = remoteChannelId,
                    High = local.High,
                    Low = local.Low,
                    RangeInclusive = local.RangeInclusive,
                    PerUnit = local.PerUnit,
                    Enabled = local.Enabled
                };

                WebAPIHub.CreateRecord("remote", "DataQualityRangeLimit", JObject.FromObject(record));
            }
        }

        private void SyncEvents(int localMeterId, int remoteMeterId, int localLineId, int remoteLineId)
        {
            // ensure remote and local line impedance matches
            List<Event> local = WebAPIHub.GetRecordsWhere("local", "Event", $"MeterID = {localMeterId} AND LineID = {localLineId}").Select(x => (Event)x).ToList();
            List<Event> remote = WebAPIHub.GetRecordsWhere("remote", "Event", $"MeterID = {remoteMeterId} AND LineID = {remoteLineId}").Select(x => (Event)x).ToList();

            // if there is a local record but not a remote record
            foreach (Event summary in local)
            {
                int eventTypeId = GetRemoteEventTypeId(summary.EventTypeID);
                int fileGroupId = GetRemoteFileGroup(summary.FileGroupID);
                int eventDataId = GetRemoteEventData(summary.EventDataID, fileGroupId);

                if (!remote.Where(x => x.EventTypeID == eventTypeId && x.StartTime.Equals(summary.StartTime) && x.EndTime.Equals(summary.EndTime)).Any())
                {
                    Event record = new Event()
                    {
                        FileGroupID = fileGroupId,
                        MeterID = remoteMeterId,
                        LineID = remoteLineId,
                        EventTypeID = eventTypeId,
                        EventDataID = eventDataId,
                        Name = summary.Name,
                        Alias = summary.Alias,
                        ShortName = summary.ShortName,
                        StartTime = summary.StartTime,
                        EndTime = summary.EndTime,
                        Samples = summary.Samples,
                        TimeZoneOffset = summary.TimeZoneOffset,
                        SamplesPerSecond = summary.SamplesPerSecond,
                        SamplesPerCycle = summary.SamplesPerCycle,
                        Description = summary.Description,
                        UpdatedBy = summary.UpdatedBy
                    };

                    int eventId = WebAPIHub.CreateRecord("remote", "Event", JObject.FromObject(record));

                    GetBreakerOperation(summary.ID, eventId);
                    //GetCSAResult(summary.ID, eventId);
                    GetDisturbances(summary.ID, eventId);
                    GetFaultGroup(summary.ID, eventId);
                    GetFaultSegments(summary.ID, eventId);
                    GetFaultSummary(summary.ID, eventId);
                }
            }

        }

        private int GetRemoteEventTypeId(int localEventTypeId)
        {
            List<EventType> localTypes = WebAPIHub.GetRecords("local", "EventType", "all").Select(x => (EventType)x).ToList();
            List<EventType> remoteTypes = WebAPIHub.GetRecords("remote", "EventType", "all").Select(x => (EventType)x).ToList();

            if (!remoteTypes.Where(x => x.Name == localTypes.Where(y => y.ID == localEventTypeId).First().Name).Any())
            {
                EventType record = new EventType()
                {
                    Name = localTypes.Where(y => y.ID == localEventTypeId).First().Name,
                    Description = localTypes.Where(y => y.ID == localEventTypeId).First().Description
                };
                return WebAPIHub.CreateRecord("remote", "EventType", JObject.FromObject(record));
            }
            else
            {
                return remoteTypes.Where(x => x.Name == localTypes.Where(y => y.ID == localEventTypeId).First().Name).First().ID;
            }
        }

        private int GetRemoteFileGroup(int localFileGroupId)
        {
            FileGroup local = (FileGroup)WebAPIHub.GetRecordsWhere("local", "FileGroup", $"ID = {localFileGroupId}").FirstOrDefault();
            List<FileGroup> remote = WebAPIHub.GetRecords("remote", "FileGroup", "all").Select(x => (FileGroup)x).ToList();

            int id;
            if (local != null && !remote.Where(x => x.DataStartTime.Equals(local.DataStartTime) && x.DataEndTime.Equals(local.DataEndTime) && x.ProcessingStartTime.Equals(local.ProcessingStartTime) && x.ProcessingEndTime.Equals(local.ProcessingEndTime)).Any())
            {
                FileGroup record = new FileGroup()
                {
                    DataStartTime = local.DataStartTime,
                    DataEndTime = local.DataEndTime,
                    ProcessingStartTime = local.ProcessingStartTime,
                    ProcessingEndTime = local.ProcessingEndTime,
                    Error = local.Error
                };
                id = WebAPIHub.CreateRecord("remote", "FileGroup", JObject.FromObject(record));
            }
            else
            {
                id = remote.Where(x => x.DataStartTime.Equals(local.DataStartTime) && x.DataEndTime.Equals(local.DataEndTime) && x.ProcessingStartTime.Equals(local.ProcessingStartTime) && x.ProcessingEndTime.Equals(local.ProcessingEndTime)).First().ID;
            }

            SyncDataFiles(localFileGroupId, id);

            return id;
        }

        private void SyncDataFiles(int localFileGroupId, int remoteFileGroupId)
        {
            List<DataFile> local = WebAPIHub.GetRecordsWhere("local", "DataFile", $"FileGroupID = {localFileGroupId}").Select(x => (DataFile)x).ToList();
            List<DataFile> remote = WebAPIHub.GetRecordsWhere("remote", "DataFile", $"FileGroupID = {remoteFileGroupId}").Select(x => (DataFile)x).ToList();

            // if there is a local record but not a remote record
            foreach (DataFile localRecord in local)
            {
                if (!remote.Where(x => x.FilePath == localRecord.FilePath).Any())
                {
                    DataFile record = new DataFile()
                    {
                        FileGroupID = remoteFileGroupId,
                        FilePath = localRecord.FilePath,
                        FilePathHash = localRecord.FilePathHash,
                        FileSize = localRecord.FileSize,
                        CreationTime = localRecord.CreationTime,
                        LastAccessTime = localRecord.LastAccessTime,
                        LastWriteTime = localRecord.LastWriteTime
                    };
                    int id = WebAPIHub.CreateRecord("remote", "DataFile", JObject.FromObject(record));

                    SyncFileBlobs(localRecord.ID, id);
                }
            }
        }

        private void SyncFileBlobs(int localDataFileId, int remoteDataFileId)
        {
            FileBlob local = (FileBlob)WebAPIHub.GetRecordsWhere("local", "FileBlob", $"DataFileID = {localDataFileId}").FirstOrDefault();
            FileBlob remote = (FileBlob)WebAPIHub.GetRecordsWhere("remote", "FileBlob", $"DataFileID = {remoteDataFileId}").FirstOrDefault();

            if (local != null && remote == null)
            {
                FileBlob record = new FileBlob()
                {
                    DataFileID = remoteDataFileId,
                    Blob = local.Blob
                };
                WebAPIHub.CreateRecord("remote", "FileBlob", JObject.FromObject(record));
            }
        }

        private int GetRemoteEventData(int localEventDataId, int remoteFileGroup)
        {
            EventData local = (EventData)WebAPIHub.GetRecord("local", "EventData", localEventDataId);
            List<EventData> remote = WebAPIHub.GetRecordsWhere("remote", "EventData", $"FileGroupID = {remoteFileGroup}").Select(x => (EventData)x).ToList();


            if (local != null && !remote.Where(x => x.RunTimeID == local.RunTimeID).Any())
            {
                EventData record = new EventData()
                {
                    FileGroupID = remoteFileGroup,
                    RunTimeID = local.RunTimeID,
                    TimeDomainData = local.TimeDomainData,
                    FrequencyDomainData = local.FrequencyDomainData,
                    MarkedForDeletion = local.MarkedForDeletion
                };
                return WebAPIHub.CreateRecord("remote", "EventData", JObject.FromObject(record));
            }
            else
                return remote.Where(x => x.RunTimeID == local.RunTimeID).FirstOrDefault().ID;
        }

        private void GetBreakerOperation(int localEventId, int remoteEventId)
        {
            List<BreakerOperation> local = WebAPIHub.GetRecordsWhere("local", "BreakerOperation", $"EventID = {localEventId}").Select(x => (BreakerOperation)x).ToList();
            List<BreakerOperation> remote = WebAPIHub.GetRecordsWhere("remote", "BreakerOperation", $"EventID = {remoteEventId}").Select(x => (BreakerOperation)x).ToList();

            // if there is a local record but not a remote record
            foreach (BreakerOperation localRecord in local)
            {
                if (!remote.Where(x => x.TripCoilEnergized.Equals(localRecord.TripCoilEnergized)).Any())
                {
                    BreakerOperation record = new BreakerOperation()
                    {
                        EventID = remoteEventId,
                        PhaseID = GetRemotePhaseId(localRecord.PhaseID),
                        BreakerOperationTypeID = GetRemoteBreakerOperationType(localRecord.BreakerOperationTypeID),
                        BreakerNumber = localRecord.BreakerNumber,
                        TripCoilEnergized = localRecord.TripCoilEnergized,
                        StatusBitSet = localRecord.StatusBitSet,
                        APhaseCleared = localRecord.APhaseCleared,
                        BPhaseCleared = localRecord.BPhaseCleared,
                        CPhaseCleared = localRecord.CPhaseCleared,
                        BreakerTiming = localRecord.BreakerTiming,
                        StatusTiming = localRecord.StatusTiming,
                        APhaseBreakerTiming = localRecord.APhaseBreakerTiming,
                        BPhaseBreakerTiming = localRecord.BPhaseBreakerTiming,
                        CPhaseBreakerTiming = localRecord.CPhaseBreakerTiming,
                        BreakerSpeed = localRecord.BreakerSpeed,
                        UpdatedBy = localRecord.UpdatedBy,
                        StatusBitChatter = localRecord.StatusBitChatter
                    };
                    int id = WebAPIHub.CreateRecord("remote", "BreakerOperation", JObject.FromObject(record));

                }
            }

        }

        private int GetRemotePhaseId(int localPhaseId)
        {
            List<Phase> remote = WebAPIHub.GetRecords("remote", "Phase", "all").Select(x => (Phase)x).ToList();
            Phase local = (Phase)WebAPIHub.GetRecord("local", "Phase", localPhaseId);
            if (remote.Where(x => x.Name == local.Name).Any())
                return remote.Where(x => x.Name == local.Name).First().ID;
            else
            {
                Phase record = new Phase()
                {
                    Name = local.Name,
                    Description = local.Description
                };

                return WebAPIHub.CreateRecord("remote", "Phase", JObject.FromObject(record));
            }
        }

        private int GetRemoteBreakerOperationType(int localBreakerOperationTypeId)
        {
            List<BreakerOperationType> remote = WebAPIHub.GetRecords("remote", "BreakerOperationType", "all").Select(x => (BreakerOperationType)x).ToList();
            BreakerOperationType local = (BreakerOperationType)WebAPIHub.GetRecord("local", "BreakerOperationType", localBreakerOperationTypeId);
            if (remote.Where(x => x.Name == local.Name).Any())
                return remote.Where(x => x.Name == local.Name).First().ID;
            else
            {
                BreakerOperationType record = new BreakerOperationType()
                {
                    Name = local.Name,
                    Description = local.Description
                };

                return WebAPIHub.CreateRecord("remote", "BreakerOperationType", JObject.FromObject(record));
            }

        }

        private void GetCSAResult(int localEventId, int remoteEventId)
        {
            CSAResult local = (CSAResult)WebAPIHub.GetRecordsWhere("local", "CSAResult", $"EventID = {localEventId}").FirstOrDefault();
            CSAResult remote = (CSAResult)WebAPIHub.GetRecordsWhere("remote", "CSAResult", $"EventID = {remoteEventId}").FirstOrDefault();

            // if there is a local record but not a remote record
            if (remote == null && local != null)
            {
                CSAResult record = new CSAResult()
                {
                    EventID = remoteEventId,
                    IsDataError = local.IsDataError,
                    IsCapSwitch = local.IsCapSwitch,
                    IsCapSwitchCondL = local.IsCapSwitchCondL,
                    OutFrequency = local.OutFrequency,
                    OutVoltagesMax = local.OutVoltagesMax,
                    OutVoltagesMean = local.OutVoltagesMean,
                    OutQConditionRPBFlag = local.OutQConditionRPBFlag,
                    OutQConditionMRPC = local.OutQConditionMRPC,
                    OutQConditionRPCA = local.OutQConditionRPCA,
                    OutQConditionRPCB = local.OutQConditionRPCB,
                    OutQConditionRPCC = local.OutQConditionRPCC,
                    OutQConditionMPFI = local.OutQConditionMPFI,
                    OutQConditionPFA = local.OutQConditionPFA,
                    OutQConditionPFB = local.OutQConditionPFB,
                    OutQConditionPFC = local.OutQConditionPFC,
                    OutRestrikeFlag = local.OutRestrikeFlag,
                    OutRestrikeNum = local.OutRestrikeNum,
                    OutRestrikePHA = local.OutRestrikePHA,
                    OutRestrikePHB = local.OutRestrikePHB,
                    OutRestrikePHC = local.OutRestrikePHC,
                    OutVTHDFlag = local.OutVTHDFlag,
                    OutVTHDBefore = local.OutVTHDBefore,
                    OutVTHDAfter = local.OutVTHDAfter,
                    OutVTHDIncrease = local.OutVTHDIncrease
                };
                int id = WebAPIHub.CreateRecord("remote", "CSAResult", JObject.FromObject(record));

            }

        }

        private void GetDisturbances(int localEventId, int remoteEventId)
        {
            List<Disturbance> local = WebAPIHub.GetRecordsWhere("local", "Disturbance", $"EventID = {localEventId}").Select(x => (Disturbance)x).ToList();
            List<Disturbance> remote = WebAPIHub.GetRecordsWhere("remote", "Disturbance", $"EventID = {remoteEventId}").Select(x => (Disturbance)x).ToList();

            // if there is a local record but not a remote record
            foreach (Disturbance localRecord in local)
            {
                int remoteEventTypeId = GetRemoteEventTypeId(localRecord.EventTypeID);

                if (!remote.Where(x => x.StartTime.Equals(localRecord.StartTime) && x.EndTime.Equals(localRecord.EndTime) && x.EventTypeID == remoteEventTypeId).Any())
                {
                    Disturbance record = new Disturbance()
                    {
                        EventID = remoteEventId,
                        PhaseID = GetRemotePhaseId(localRecord.PhaseID),
                        EventTypeID = remoteEventTypeId,
                        Magnitude = localRecord.Magnitude,
                        PerUnitMagnitude = localRecord.PerUnitMagnitude,
                        StartTime = localRecord.StartTime,
                        EndTime = localRecord.EndTime,
                        DurationSeconds = localRecord.DurationSeconds,
                        DurationCycles = localRecord.DurationCycles,
                        StartIndex = localRecord.StartIndex,
                        EndIndex = localRecord.EndIndex,
                        UpdatedBy = ""
                    };
                    int id = WebAPIHub.CreateRecord("remote", "Disturbance", JObject.FromObject(record));
                    GetDisturbanceSeverity(localRecord.ID, id);
                }
            }
        }

        private void GetDisturbanceSeverity(int localDisturbanceId, int remoteDisturbanceId)
        {
            DisturbanceSeverity local = (DisturbanceSeverity)WebAPIHub.GetRecordsWhere("local", "DisturbanceSeverity", $"DisturbanceID = {localDisturbanceId}").FirstOrDefault();
            DisturbanceSeverity remote = (DisturbanceSeverity)WebAPIHub.GetRecordsWhere("remote", "DisturbanceSeverity", $"DisturbanceID = {remoteDisturbanceId}").FirstOrDefault();

            // if there is a local record but not a remote record
            if (remote == null && local != null)
            {
                DisturbanceSeverity record = new DisturbanceSeverity()
                {
                    VoltageEnvelopeID = GetRemoteVoltageEnvelopeId(local.VoltageEnvelopeID),
                    DisturbanceID = remoteDisturbanceId,
                    SeverityCode = local.SeverityCode
                };
                int id = WebAPIHub.CreateRecord("remote", "DisturbanceSeverity", JObject.FromObject(record));
            }
        }

        private int GetRemoteVoltageEnvelopeId(int id)
        {
            VoltageEnvelope local = (VoltageEnvelope)WebAPIHub.GetRecordsWhere("local", "VoltageEnvelope", $"ID = {id}").FirstOrDefault();
            VoltageEnvelope remote = (VoltageEnvelope)WebAPIHub.GetRecordsWhere("remote", "VoltageEnvelope", $"Name = {local.Name}").FirstOrDefault();

            if (remote == null)
            {
                VoltageEnvelope record = new VoltageEnvelope()
                {
                    Name = local.Name,
                    Description = local.Description
                };
                return WebAPIHub.CreateRecord("remote", "VoltageEnvelope", JObject.FromObject(record));

            }
            return remote.ID;
        }

        private void GetFaultGroup(int localEventId, int remoteEventId)
        {
            FaultGroup local = (FaultGroup)WebAPIHub.GetRecordsWhere("local", "FaultGroup", $"EventID = {localEventId}").FirstOrDefault();
            FaultGroup remote = (FaultGroup)WebAPIHub.GetRecordsWhere("remote", "FaultGroup", $"EventID = {remoteEventId}").FirstOrDefault();

            // if there is a local record but not a remote record
            if (remote == null && local != null)
            {
                FaultGroup record = new FaultGroup()
                {
                    EventID = remoteEventId,
                    FaultDetectionLogicResult = local.FaultDetectionLogicResult,
                    DefaultFaultDetectionLogicResult = local.DefaultFaultDetectionLogicResult,
                    FaultValidationLogicResult = local.FaultValidationLogicResult
                };
                int id = WebAPIHub.CreateRecord("remote", "FaultGroup", JObject.FromObject(record));

            }

        }

        private void GetFaultSegments(int localEventId, int remoteEventId)
        {
            List<FaultSegment> local = WebAPIHub.GetRecordsWhere("local", "FaultSegment", $"EventID = {localEventId}").Select(x => (FaultSegment)x).ToList();
            List<FaultSegment> remote = WebAPIHub.GetRecordsWhere("remote", "FaultSegment", $"EventID = {remoteEventId}").Select(x => (FaultSegment)x).ToList();

            // if there is a local record but not a remote record
            foreach (FaultSegment localRecord in local)
            {
                int remoteSegmentTypeId = GetRemoteSegmentTypeId(localRecord.SegmentTypeID);

                if (!remote.Where(x => x.StartTime.Equals(localRecord.StartTime) && x.EndTime.Equals(localRecord.EndTime) && x.SegmentTypeID == remoteSegmentTypeId).Any())
                {
                    FaultSegment record = new FaultSegment()
                    {
                        EventID = remoteEventId,
                        SegmentTypeID = remoteSegmentTypeId,
                        StartTime = localRecord.StartTime,
                        EndTime = localRecord.EndTime,
                        StartSample = localRecord.StartSample,
                        EndSample = localRecord.EndSample
                    };
                    int id = WebAPIHub.CreateRecord("remote", "FaultSegment", JObject.FromObject(record));
                }
            }
        }

        private int GetRemoteSegmentTypeId(int id)
        {
            SegmentType local = (SegmentType)WebAPIHub.GetRecordsWhere("local", "SegmentType", $"ID = {id}").FirstOrDefault();
            SegmentType remote = (SegmentType)WebAPIHub.GetRecordsWhere("remote", "SegmentType", $"Name = '{local.Name}'").FirstOrDefault();

            if (remote == null)
            {
                SegmentType record = new SegmentType()
                {
                    Name = local.Name,
                    Description = local.Description
                };
                return WebAPIHub.CreateRecord("remote", "SegmentType", JObject.FromObject(record));

            }
            return remote.ID;
        }

        private void GetFaultSummary(int localEventId, int remoteEventId)
        {
            List<FaultSummary> local = WebAPIHub.GetRecordsWhere("local", "FaultSummary", $"EventID = {localEventId}").Select(x => (FaultSummary)x).ToList();
            List<FaultSummary> remote = WebAPIHub.GetRecordsWhere("remote", "FaultSummary", $"EventID = {remoteEventId}").Select(x => (FaultSummary)x).ToList();

            // if there is a local record but not a remote record
            foreach (FaultSummary localRecord in local)
            {
                if (!remote.Where(x => x.Inception.Equals(localRecord.Inception) && x.FaultType == localRecord.FaultType).Any())
                {
                    FaultSummary record = new FaultSummary()
                    {
                        EventID = remoteEventId,
                        Algorithm = localRecord.Algorithm,
                        FaultNumber = localRecord.FaultNumber,
                        CalculationCycle = localRecord.CalculationCycle,
                        Distance = localRecord.Distance,
                        CurrentMagnitude = localRecord.CurrentMagnitude,
                        CurrentLag = localRecord.CurrentLag,
                        PrefaultCurrent = localRecord.PrefaultCurrent,
                        PostfaultCurrent = localRecord.PostfaultCurrent,
                        Inception = localRecord.Inception,
                        DurationSeconds = localRecord.DurationSeconds,
                        DurationCycles = localRecord.DurationCycles,
                        FaultType = localRecord.FaultType,
                        IsSelectedAlgorithm = localRecord.IsSelectedAlgorithm,
                        IsValid = localRecord.IsValid,
                        IsSuppressed = localRecord.IsSuppressed
                    };
                    int id = WebAPIHub.CreateRecord("remote", "FaultSummary", JObject.FromObject(record));
                }
            }
        }

        private void GetICFResult(int localEventId, int remoteEventId)
        {
            ICFResult local = (ICFResult)WebAPIHub.GetRecordsWhere("local", "ICFResult", $"EventID = {localEventId}").FirstOrDefault();
            ICFResult remote = (ICFResult)WebAPIHub.GetRecordsWhere("remote", "ICFResult", $"EventID = {remoteEventId}").FirstOrDefault();

            // if there is a local record but not a remote record
            if (remote == null && local != null)
            {
                ICFResult record = new ICFResult()
                {
                    EventID = remoteEventId,
                    IsDataError = local.IsDataError,
                    ICFState = local.ICFState,
                    ConfidenceLevel = local.ConfidenceLevel,
                    Phase = local.Phase,
                    EventCount = local.EventCount
                };
                int id = WebAPIHub.CreateRecord("remote", "ICFResult", JObject.FromObject(record));

            }

        }

        private void GetICFEvent(int localEventId, int remoteEventId)
        {
            ICFEvent local = (ICFEvent)WebAPIHub.GetRecordsWhere("local", "ICFEvent", $"ICFResultID = {localEventId}").FirstOrDefault();
            ICFEvent remote = (ICFEvent)WebAPIHub.GetRecordsWhere("remote", "ICFEvent", $"ICFResultID = {remoteEventId}").FirstOrDefault();

            // if there is a local record but not a remote record
            if (remote == null && local != null)
            {
                ICFEvent record = new ICFEvent()
                {
                    ICFResultID = remoteEventId,
                    PhaseAngle = local.PhaseAngle,
                    Time = local.Time,
                    DurationCycles = local.DurationCycles,
                    PeakFaultCurrent = local.PeakFaultCurrent
                };
                int id = WebAPIHub.CreateRecord("remote", "ICFEvent", JObject.FromObject(record));

            }

        }

        #endregion
    }
}
