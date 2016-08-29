//******************************************************************************************************
//  DataHub.cs - Gbtc
//
//  Copyright © 2016, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the MIT License (MIT), the "License"; you may not use this
//  file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://opensource.org/licenses/MIT
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  06/01/2016 - Billy Ernest
//       Generated original version of source code.
//  08/18/2016 - J. Ritchie Carroll
//       Updated to use record operations hub base class.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using GSF.Data.Model;
using GSF.Web.Hubs;
using GSF.Web.Security;
using openXDA.Model;

namespace openXDA
{
    [AuthorizeHubRole]
    public class DataHub : RecordOperationsHub<DataHub>
    {
        // Client-side script functionality

        #region [ Setting Table Operations ]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Setting), RecordOperation.QueryRecordCount)]
        public int QuerySettingCount(string filterString)
        {
            return DataContext.Table<Setting>().QueryRecordCount();
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Setting), RecordOperation.QueryRecords)]
        public IEnumerable<Setting> QuerySettings(string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            return DataContext.Table<Setting>().QueryRecords(sortField, ascending, page, pageSize);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Setting), RecordOperation.DeleteRecord)]
        public void DeleteSetting(int id)
        {
            DataContext.Table<Setting>().DeleteRecord(id);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Setting), RecordOperation.CreateNewRecord)]
        public Setting NewSetting()
        {
            return new Setting();
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Setting), RecordOperation.AddNewRecord)]
        public void AddNewActionItem(Setting record)
        {
            DataContext.Table<Setting>().AddNewRecord(record);
        }

        [AuthorizeHubRole("Administrator, Owner")]
        [RecordOperation(typeof(Setting), RecordOperation.UpdateRecord)]
        public void UpdateActionItem(Setting record)
        {
            DataContext.Table<Setting>().UpdateRecord(record);
        }

        #endregion

        #region [ DashSettings Table Operations ]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(DashSettings), RecordOperation.QueryRecordCount)]
        public int QueryDashSettingsCount(string filterString)
        {
            return DataContext.Table<DashSettings>().QueryRecordCount();
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(DashSettings), RecordOperation.QueryRecords)]
        public IEnumerable<DashSettings> QueryDashSettingss(string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            return DataContext.Table<DashSettings>().QueryRecords(sortField, ascending, page, pageSize);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(DashSettings), RecordOperation.DeleteRecord)]
        public void DeleteDashSettings(int id)
        {
            DataContext.Table<DashSettings>().DeleteRecord(id);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(DashSettings), RecordOperation.CreateNewRecord)]
        public DashSettings NewDashSettings()
        {
            return new DashSettings();
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(DashSettings), RecordOperation.AddNewRecord)]
        public void AddNewActionItem(DashSettings record)
        {
            DataContext.Table<DashSettings>().AddNewRecord(record);
        }

        [AuthorizeHubRole("Administrator, Owner")]
        [RecordOperation(typeof(DashSettings), RecordOperation.UpdateRecord)]
        public void UpdateActionItem(DashSettings record)
        {
            DataContext.Table<DashSettings>().UpdateRecord(record);
        }

        #endregion

        #region [ Meter Table Operations ]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Meter), RecordOperation.QueryRecordCount)]
        public int QueryMeterCount(string filterString)
        {
            if (filterString == null) filterString = "%";
            else
            {
                // Build your filter string here!
                filterString += "%";
            }

            return DataContext.Table<Meter>().QueryRecordCount(new RecordRestriction("Name LIKE {0}", filterString));
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Meter), RecordOperation.QueryRecords)]
        public IEnumerable<Meter> QueryMeters(string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            if (filterString == null) filterString = "%";
            else
            {
                // Build your filter string here!
                filterString += "%";
            }

            return DataContext.Table<Meter>().QueryRecords(sortField, ascending, page, pageSize, new RecordRestriction("Name LIKE {0}", filterString));
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Meter), RecordOperation.DeleteRecord)]
        public void DeleteMeter(int id)
        {
            DataContext.Table<Meter>().DeleteRecord(id);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Meter), RecordOperation.CreateNewRecord)]
        public Meter NewMeter()
        {
            return new Meter();
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Meter), RecordOperation.AddNewRecord)]
        public void AddNewMeter(Meter record)
        {
            DataContext.Table<Meter>().AddNewRecord(record);
        }

        [AuthorizeHubRole("Administrator, Owner")]
        [RecordOperation(typeof(Meter), RecordOperation.UpdateRecord)]
        public void UpdateMeter(Meter record)
        {
            DataContext.Table<Meter>().UpdateRecord(record);
        }

        #endregion

        #region [ MeterLocation Table Operations ]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(MeterLocation), RecordOperation.QueryRecordCount)]
        public int QueryMeterLocationCount(string filterString)
        {
            if (filterString == null) filterString = "%";
            else
            {
                // Build your filter string here!
                filterString += "%";
            }


            return DataContext.Table<MeterLocation>().QueryRecordCount(new RecordRestriction("Name LIKE {0}", filterString));
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(MeterLocation), RecordOperation.QueryRecords)]
        public IEnumerable<MeterLocation> QueryMeterLocations(string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            if (filterString == null) filterString = "%";
            else
            {
                // Build your filter string here!
                filterString += "%";
            }


            return DataContext.Table<MeterLocation>().QueryRecords(sortField, ascending, page, pageSize, new RecordRestriction("Name LIKE {0}", filterString));
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(MeterLocation), RecordOperation.DeleteRecord)]
        public void DeleteMeterLocation(int id)
        {
            DataContext.Table<MeterLocation>().DeleteRecord(id);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(MeterLocation), RecordOperation.CreateNewRecord)]
        public MeterLocation NewMeterLocation()
        {
            return new MeterLocation();
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(MeterLocation), RecordOperation.AddNewRecord)]
        public void AddNewMeterLocation(MeterLocation record)
        {
            DataContext.Table<MeterLocation>().AddNewRecord(record);
        }

        [AuthorizeHubRole("Administrator, Owner")]
        [RecordOperation(typeof(MeterLocation), RecordOperation.UpdateRecord)]
        public void UpdateMeterLocation(MeterLocation record)
        {
            DataContext.Table<MeterLocation>().UpdateRecord(record);
        }

        #endregion

        #region [ Lines Table Operations ]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Lines), RecordOperation.QueryRecordCount)]
        public int QueryLinesCount(string filterString)
        {
            if (filterString == null) filterString = "%";
            else
            {
                // Build your filter string here!
                filterString += "%";
            }


            return DataContext.Table<Lines>().QueryRecordCount(new RecordRestriction("Name LIKE {0}", filterString));
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Lines), RecordOperation.QueryRecords)]
        public IEnumerable<Lines> QueryLines(string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            if (filterString == null) filterString = "%";
            else
            {
                // Build your filter string here!
                filterString += "%";
            }


            return DataContext.Table<Lines>().QueryRecords(sortField, ascending, page, pageSize, new RecordRestriction("Name LIKE {0}", filterString));
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Lines), RecordOperation.DeleteRecord)]
        public void DeleteLines(int id)
        {
            DataContext.Table<Lines>().DeleteRecord(id);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Lines), RecordOperation.CreateNewRecord)]
        public Lines NewLines()
        {
            return new Lines();
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Lines), RecordOperation.AddNewRecord)]
        public void AddNewLines(Lines record)
        {
            DataContext.Table<Lines>().AddNewRecord(record);
        }

        [AuthorizeHubRole("Administrator, Owner")]
        [RecordOperation(typeof(Lines), RecordOperation.UpdateRecord)]
        public void UpdateLines(Lines record)
        {
            DataContext.Table<Lines>().UpdateRecord(record);
        }

        #endregion

        #region [ LineView Table Operations ]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(LineView), RecordOperation.QueryRecordCount)]
        public int QueryLineViewCount(string filterString)
        {
            if (filterString == null) filterString = "%";
            else
            {
                // Build your filter string here!
                filterString += "%";
            }

            return DataContext.Table<LineView>().QueryRecordCount(new RecordRestriction("TopName LIKE {0}", filterString));
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(LineView), RecordOperation.QueryRecords)]
        public IEnumerable<LineView> QueryLineView(string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            if (filterString == null) filterString = "%";
            else
            {
                // Build your filter string here!
                filterString += "%";
            }

            return DataContext.Table<LineView>().QueryRecords(sortField, ascending, page, pageSize, new RecordRestriction("TopName LIKE {0}", filterString));
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(LineView), RecordOperation.DeleteRecord)]
        public void DeleteLineView(int id)
        {
            DataContext.Table<Lines>().DeleteRecord(id);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(LineView), RecordOperation.CreateNewRecord)]
        public LineView NewLineView()
        {
            return new LineView();
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(LineView), RecordOperation.AddNewRecord)]
        public void AddNewLineView(LineView record)
        {
            DataContext.Table<Lines>().AddNewRecord(CreateLines(record));
            DataContext.Table<LineImpedance>().AddNewRecord(CreateLineImpedance(record));
        }

        [AuthorizeHubRole("Administrator, Owner")]
        [RecordOperation(typeof(LineView), RecordOperation.UpdateRecord)]
        public void UpdateLineView(LineView record)
        {
            DataContext.Table<Lines>().UpdateRecord(CreateLines(record));
            DataContext.Table<LineImpedance>().UpdateRecord(CreateLineImpedance(record));

        }

        public Lines CreateLines(LineView record)
        {
            Lines line = NewLines();
            line.AssetKey = record.AssetKey;
            line.Description = record.Description;
            line.Length = record.Length;
            line.ThermalRating = record.ThermalRating;
            line.VoltageKV = record.VoltageKV;
            return line;
        }

        public LineImpedance CreateLineImpedance(LineView record)
        {
            LineImpedance li = new LineImpedance();
            li.R0 = record.R0;
            li.R1 = record.R1;
            li.X0 = record.X0;
            li.X1 = record.X1;
            li.LineID = record.ID;
            return li;
        }

        #endregion

        #region [ MeterLine Table Operations ]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(MeterLine), RecordOperation.QueryRecordCount)]
        public int QueryMeterLineCount(int lineID, int meterID, string filterString)
        {
            string restrictionString = "";
            if(lineID == -1 && meterID != -1)
            {
                restrictionString = $"MeterID = {meterID}";
            }
            else if (meterID == -1 && lineID != -1)
            {
                restrictionString = $"LineID = {lineID}";
            }
            else if(meterID != -1 && lineID != -1)
            {
                restrictionString = $"MeterID = {meterID} AND LineID = {lineID}";
            }

            return DataContext.Table<MeterLine>().QueryRecordCount(new RecordRestriction(restrictionString));
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(MeterLine), RecordOperation.QueryRecords)]
        public IEnumerable<MeterLine> QueryMeterLine(int lineID, int meterID, string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            string restrictionString = "";
            if (lineID == -1 && meterID != -1)
            {
                restrictionString = $"MeterID = {meterID}";
            }
            else if (meterID == -1 && lineID != -1)
            {
                restrictionString = $"LineID = {lineID}";
            }
            else if (meterID != -1 && lineID != -1)
            {
                restrictionString = $"MeterID = {meterID} AND LineID = {lineID}";
            }

            return DataContext.Table<MeterLine>().QueryRecords(sortField, ascending, page, pageSize, new RecordRestriction(restrictionString));
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(MeterLine), RecordOperation.DeleteRecord)]
        public void DeleteMeterLine(int id)
        {
            DataContext.Table<MeterLine>().DeleteRecord(id);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(MeterLine), RecordOperation.CreateNewRecord)]
        public MeterLine NewMeterLine()
        {
            return new MeterLine();
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(MeterLine), RecordOperation.AddNewRecord)]
        public void AddNewMeterLine(MeterLine record)
        {
            DataContext.Table<MeterLine>().AddNewRecord(record);
        }

        [AuthorizeHubRole("Administrator, Owner")]
        [RecordOperation(typeof(MeterLine), RecordOperation.UpdateRecord)]
        public void UpdateMeterLine(MeterLine record)
        {
            DataContext.Table<MeterLine>().UpdateRecord(record);
        }

        #endregion

        #region [ Channel Table Operations ]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Channel), RecordOperation.QueryRecordCount)]
        public int QueryChannelCount(int lineID, int meterID, string filterString = "%")
        {

            return DataContext.Table<Channel>().QueryRecordCount(new RecordRestriction($"MeterID = {meterID} AND LineID = {lineID}"));
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Channel), RecordOperation.QueryRecords)]
        public IEnumerable<Channel> QueryChannel(int lineID, int meterID, string sortField, bool ascending, int page, int pageSize, string filterString)
        {

            return DataContext.Table<Channel>().QueryRecords(sortField, ascending, page, pageSize, new RecordRestriction($"MeterID = {meterID} AND LineID = {lineID}"));
        }

        public IEnumerable<Channel> QueryChannelsForDropDown(string filterString)
        {
            return DataContext.Table<Channel>().QueryRecords(restriction: new RecordRestriction("Name LIKE {0}" ,filterString), limit: 50);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Channel), RecordOperation.DeleteRecord)]
        public void DeleteChannel(int id)
        {
            DataContext.Table<Channel>().DeleteRecord(id);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Channel), RecordOperation.CreateNewRecord)]
        public Channel NewChannel()
        {
            return new Channel();
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Channel), RecordOperation.AddNewRecord)]
        public void AddNewChannel(Channel record)
        {
            DataContext.Table<Channel>().AddNewRecord(record);
        }

        [AuthorizeHubRole("Administrator, Owner")]
        [RecordOperation(typeof(Channel), RecordOperation.UpdateRecord)]
        public void UpdateChannel(Channel record)
        {
            DataContext.Table<Channel>().UpdateRecord(record);
        }

        #endregion

        #region [ ChannelView Table Operations ]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(ChannelView), RecordOperation.QueryRecordCount)]
        public int QueryChannelViewCount(int lineID, int meterID, string filterString = "%")
        {
            string ChannelFilter = "%";
            string typeFilter = "%";
            string charFilter = "%";
            if (filterString != "%" && filterString != null)
            {
                string[] filters = filterString.Split(';');
                if (filters.Length == 3)
                {
                    ChannelFilter = filters[0] + '%';
                    typeFilter = filters[1] += '%';
                    charFilter = filters[2] += '%';
                }
            }

            return DataContext.Table<ChannelView>().QueryRecordCount(new RecordRestriction($"MeterID = {meterID} AND LineID = {lineID} AND Name LIKE '{ChannelFilter}' AND MeasurementType LIKE '{typeFilter}' AND MeasurementCharacteristic LIKE '{charFilter}'"));
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(ChannelView), RecordOperation.QueryRecords)]
        public IEnumerable<ChannelView> QueryChannelView(int lineID, int meterID, string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            string ChannelFilter = "%";
            string typeFilter = "%";
            string charFilter = "%";
            if (filterString != "%")
            {
                string[] filters = filterString.Split(';');
                if (filters.Length == 3)
                {
                    ChannelFilter = filters[0] + '%';
                    typeFilter = filters[1] += '%';
                    charFilter = filters[2] += '%';
                }
            }

            return DataContext.Table<ChannelView>().QueryRecords(sortField, ascending, page, pageSize, new RecordRestriction($"MeterID = {meterID} AND LineID = {lineID} AND Name LIKE '{ChannelFilter}' AND MeasurementType LIKE '{typeFilter}' AND MeasurementCharacteristic LIKE '{charFilter}'"));
        }

        public IEnumerable<ChannelView> QueryChannelViewsForDropDown(string filterString)
        {
            return DataContext.Table<ChannelView>().QueryRecords(restriction: new RecordRestriction("Name LIKE {0}", filterString), limit: 50);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(ChannelView), RecordOperation.DeleteRecord)]
        public void DeleteChannelView(int id)
        {
            DataContext.Table<Channel>().DeleteRecord(id);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(ChannelView), RecordOperation.CreateNewRecord)]
        public ChannelView NewChannelView()
        {
            return new ChannelView();
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(ChannelView), RecordOperation.AddNewRecord)]
        public void AddNewChannelView(ChannelView record)
        {
            DataContext.Table<Channel>().AddNewRecord(MakeChannel(record));
        }

        [AuthorizeHubRole("Administrator, Owner")]
        [RecordOperation(typeof(ChannelView), RecordOperation.UpdateRecord)]
        public void UpdateChannelView(ChannelView record)
        {
            DataContext.Table<Channel>().UpdateRecord(MakeChannel(record));
        }

        public Channel MakeChannel(ChannelView record)
        {
            Channel newRecord = new Channel();
            newRecord.ID = record.ID;
            newRecord.MeterID = record.MeterID;
            newRecord.MeasurementCharacteristicID = record.MeasurementCharacteristicID;
            newRecord.MeasurementTypeID = record.MeasurementTypeID;
            newRecord.PhaseID = record.PhaseID;
            newRecord.SamplesPerHour = record.SamplesPerHour;
            newRecord.PerUnitValue = record.PerUnitValue;
            newRecord.HarmonicGroup = record.HarmonicGroup;
            newRecord.Description = record.Description;
            newRecord.Enabled = record.Enabled;
            newRecord.Name = record.Name;
            newRecord.LineID = record.LineID;

            return newRecord;
        }
        #endregion
        #region [ Group Table Operations ]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Group), RecordOperation.QueryRecordCount)]
        public int QueryGroupCount(string filterString)
        {
            return DataContext.Table<Group>().QueryRecordCount();
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Group), RecordOperation.QueryRecords)]
        public IEnumerable<Group> QueryGroups(string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            return DataContext.Table<Group>().QueryRecords(sortField, ascending, page, pageSize);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Group), RecordOperation.DeleteRecord)]
        public void DeleteGroup(int id)
        {
            IEnumerable<GroupMeter> table = DataContext.Table<GroupMeter>().QueryRecords(restriction: new RecordRestriction("GroupID = {0}", id));
            foreach (GroupMeter gm in table)
            {
                DataContext.Table<GroupMeter>().DeleteRecord(gm.ID);
            }
            DataContext.Table<Group>().DeleteRecord(id);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Group), RecordOperation.CreateNewRecord)]
        public Group NewGroup()
        {
            return new Group();
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Group), RecordOperation.AddNewRecord)]
        public void AddNewGroup(Group record)
        {
            DataContext.Table<Group>().AddNewRecord(record);
        }

        [AuthorizeHubRole("Administrator, Owner")]
        [RecordOperation(typeof(Group), RecordOperation.UpdateRecord)]
        public void UpdateGroup(Group record)
        {
            DataContext.Table<Group>().UpdateRecord(record);
        }

        #endregion

        #region [ GroupMeterView Table Operations ]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(GroupMeterView), RecordOperation.QueryRecordCount)]
        public int QueryGroupMeterViewCount(int groupID, string filterString)
        {
            return DataContext.Table<GroupMeterView>().QueryRecordCount(new RecordRestriction("GroupID = {0}", groupID));
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(GroupMeterView), RecordOperation.QueryRecords)]
        public IEnumerable<GroupMeterView> QueryGroupMeterViews(int groupID, string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            return DataContext.Table<GroupMeterView>().QueryRecords(sortField, ascending, page, pageSize, new RecordRestriction("GroupID = {0}", groupID));
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(GroupMeterView), RecordOperation.DeleteRecord)]
        public void DeleteGroupMeterView(int id)
        {
            DataContext.Table<GroupMeter>().DeleteRecord(id);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(GroupMeterView), RecordOperation.CreateNewRecord)]
        public GroupMeterView NewGroupMeterView()
        {
            return new GroupMeterView();
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(GroupMeterView), RecordOperation.AddNewRecord)]
        public void AddNewGroupMeterView(GroupMeterView record)
        {
            DataContext.Table<GroupMeter>().AddNewRecord(CreateNewGroupMeter(record));
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(GroupMeterView), RecordOperation.UpdateRecord)]
        public void UpdateGroupMeterView(GroupMeterView record)
        {
            DataContext.Table<GroupMeter>().UpdateRecord(CreateNewGroupMeter(record));
        }

        public GroupMeter CreateNewGroupMeter(GroupMeterView record)
        {
            GroupMeter gm = new GroupMeter();
            gm.GroupID = record.GroupID;
            gm.MeterID = record.MeterID;
            return gm;
        }

        #endregion

        #region [ AlarmRangeLimitView Table Operations ]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(AlarmRangeLimitView), RecordOperation.QueryRecordCount)]
        public int QueryAlarmRangeLimitViewCount(string filterString)
        {
            string channelFilter = "%";
            string typeFilter = "%";
            string charFilter = "%";
            if (filterString != "%")
            {
                string[] filters = filterString.Split(';');
                if (filters.Length == 3)
                {
                    channelFilter = filters[0] + '%';
                    typeFilter = filters[1] += '%';
                    charFilter = filters[2] += '%';
                }
            }


            return DataContext.Table<AlarmRangeLimitView>().QueryRecordCount(new RecordRestriction("Name LIKE {0} AND MeasurementType LIKE {1} AND MeasurementCharacteristic LIKE {2}", channelFilter, typeFilter, charFilter));
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(AlarmRangeLimitView), RecordOperation.QueryRecords)]
        public IEnumerable<AlarmRangeLimitView> QueryAlarmRangeLimitViews( string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            string channelFilter = "%";
            string typeFilter = "%";
            string charFilter = "%";
            if (filterString != "%")
            {
                string[] filters = filterString.Split(';');
                if (filters.Length == 3)
                {
                    channelFilter = filters[0] + '%';
                    typeFilter = filters[1] += '%';
                    charFilter = filters[2] += '%';
                }
            }


            return DataContext.Table<AlarmRangeLimitView>().QueryRecords(sortField, ascending, page, pageSize, new RecordRestriction("Name LIKE {0} AND MeasurementType LIKE {1} AND MeasurementCharacteristic LIKE {2}", channelFilter, typeFilter, charFilter));
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(AlarmRangeLimitView), RecordOperation.DeleteRecord)]
        public void DeleteAlarmRangeLimitView(int id)
        {
            DataContext.Table<AlarmRangeLimit>().DeleteRecord(id);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(AlarmRangeLimitView), RecordOperation.CreateNewRecord)]
        public AlarmRangeLimitView NewAlarmRangeLimitView()
        {
            return new AlarmRangeLimitView();
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(AlarmRangeLimitView), RecordOperation.AddNewRecord)]
        public void AddNewAlarmRangeLimitView(AlarmRangeLimitView record)
        {
            if (record.IsDefault)
            {
                IEnumerable<DefaultAlarmRangeLimit> defaultLimits = DataContext.Table<DefaultAlarmRangeLimit>().QueryRecords(restriction: new RecordRestriction("MeasurementTypeID = {0} AND MeasurementCharacteristicID = {1}", record.MeasurementTypeID, record.MeasurementCharacteristicID));

                if (defaultLimits.Any())
                {
                    record.Severity = defaultLimits.First().Severity;
                    record.High = defaultLimits.First().High;
                    record.Low = defaultLimits.First().Low;
                    record.RangeInclusive = defaultLimits.First().RangeInclusive;
                    record.PerUnit = defaultLimits.First().PerUnit;
                }
            }


            DataContext.Table<AlarmRangeLimit>().AddNewRecord(CreateNewAlarmRangeLimit(record));
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(AlarmRangeLimitView), RecordOperation.UpdateRecord)]
        public void UpdateAlarmRangeLimitView(AlarmRangeLimitView record)
        {
            if (record.IsDefault)
            {
                IEnumerable<DefaultAlarmRangeLimit> defaultLimits = DataContext.Table<DefaultAlarmRangeLimit>().QueryRecords(restriction: new RecordRestriction("MeasurementTypeID = {0} AND MeasurementCharacteristicID = {1}", record.MeasurementTypeID, record.MeasurementCharacteristicID));

                if (defaultLimits.Any())
                {
                    record.Severity = defaultLimits.First().Severity;
                    record.High = defaultLimits.First().High;
                    record.Low = defaultLimits.First().Low;
                    record.RangeInclusive = defaultLimits.First().RangeInclusive;
                    record.PerUnit = defaultLimits.First().PerUnit;
                }
            }

            DataContext.Table<AlarmRangeLimit>().UpdateRecord(CreateNewAlarmRangeLimit(record));
        }

        public AlarmRangeLimit CreateNewAlarmRangeLimit(AlarmRangeLimitView record)
        {
            AlarmRangeLimit arl = new AlarmRangeLimit();
            arl.ID = record.ID;
            arl.ChannelID = record.ChannelID;
            arl.AlarmTypeID = record.AlarmTypeID;
            arl.Enabled = record.Enabled;


            arl.Severity = record.Severity;
            arl.High = record.High;
            arl.Low = record.Low;
            arl.RangeInclusive = record.RangeInclusive;
            arl.PerUnit = record.PerUnit;
            arl.IsDefault = record.IsDefault;

            return arl;
        }

        [AuthorizeHubRole("Administrator")]
        public void ResetAlarmToDefault(AlarmRangeLimitView record)
        {
            IEnumerable<DefaultAlarmRangeLimit> defaultLimits = DataContext.Table<DefaultAlarmRangeLimit>().QueryRecords(restriction: new RecordRestriction("MeasurementTypeID = {0} AND MeasurementCharacteristicID = {1}", record.MeasurementTypeID, record.MeasurementCharacteristicID));

            if(defaultLimits.Any())
            {
                record.Severity = defaultLimits.First().Severity;
                record.High = defaultLimits.First().High;
                record.Low = defaultLimits.First().Low;
                record.RangeInclusive = defaultLimits.First().RangeInclusive;
                record.PerUnit = defaultLimits.First().PerUnit;
                record.IsDefault = true;

                DataContext.Table<AlarmRangeLimit>().UpdateRecord(CreateNewAlarmRangeLimit(record));
            }
        }

        [AuthorizeHubRole("Administrator")]
        public string SendAlarmTableToCSV()
        {
            string csv = "";
            string[] headers = DataContext.Table<AlarmRangeLimitView>().GetFieldNames();

            foreach (string h in headers)
            {
                if (csv == "")
                    csv = '[' + h + ']';
                else
                    csv += ",[" + h + ']';
            }

            csv += "\n";
             
            IEnumerable<AlarmRangeLimitView> limits = DataContext.Table<AlarmRangeLimitView>().QueryRecords();

            foreach (AlarmRangeLimitView limit in limits)
            {
                csv += limit.csvString() + '\n';
                //csv += limit.ID.ToString() + ',' + limit.ChannelID.ToString() + ',' + limit.Name.ToString() + ',' + limit.AlarmTypeID.ToString() + ',' + limit.Severity.ToString() + ',' + limit.High.ToString() + ',' 
                //    + limit.Low.ToString() + ',' + limit.RangeInclusive.ToString() + ',' + limit.PerUnit.ToString() + ',' + limit.Enabled.ToString() + ',' + limit.MeasurementType.ToString() + ','
                //    + limit.MeasurementTypeID.ToString() + ',' + limit.MeasurementCharacteristic.ToString() + ',' + limit.MeasurementCharacteristicID.ToString() + ',' + limit.Phase.ToString() + ','
                //    + limit.PhaseID.ToString() + ',' + limit.HarmonicGroup.ToString() + ','  + limit.IsDefault.ToString() + "\n";
            }

            return csv;
        }

        public void ImportAlarmTableCSV(string csv)
        {
            string[] csvRows = csv.Split('\n');
            string[] tableFields = csvRows[0].Split(',');

            TableOperations<AlarmRangeLimit> table = DataContext.Table<AlarmRangeLimit>();

            if (table.GetFieldNames() == tableFields)
            {
                for (int i = 1; i < csvRows.Length; ++i)
                {
                    string[] row = csvRows[i].Split(',');

                    AlarmRangeLimit newRecord = DataContext.Connection.ExecuteScalar<AlarmRangeLimit>("Select * FROM AlarmRangeLimit WHERE ID ={0}", row[0]);

                    newRecord.Severity = int.Parse(row[4]);
                    newRecord.High = float.Parse(row[5]);
                    newRecord.Low = float.Parse(row[6]);
                    newRecord.RangeInclusive = int.Parse(row[7]);
                    newRecord.PerUnit = int.Parse(row[8]);
                    newRecord.Enabled = int.Parse(row[9]);
                    newRecord.IsDefault = bool.Parse(row[17]);

                    table.UpdateRecord(newRecord);
                }
            }
        }

        #endregion

        #region [ DefaultAlarmRangeLimit Table Operations ]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(DefaultAlarmRangeLimitView), RecordOperation.QueryRecordCount)]
        public int QueryDefaultAlarmRangeLimitViewCount(string filterString)
        {
            return DataContext.Table<DefaultAlarmRangeLimitView>().QueryRecordCount();
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(DefaultAlarmRangeLimitView), RecordOperation.QueryRecords)]
        public IEnumerable<DefaultAlarmRangeLimitView> QueryDefaultAlarmRangeLimitViews(string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            return DataContext.Table<DefaultAlarmRangeLimitView>().QueryRecords(sortField, ascending, page, pageSize);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(DefaultAlarmRangeLimitView), RecordOperation.DeleteRecord)]
        public void DeleteDefaultAlarmRangeLimitView(int id)
        {
            DataContext.Table<AlarmRangeLimit>().DeleteRecord(id);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(DefaultAlarmRangeLimitView), RecordOperation.CreateNewRecord)]
        public DefaultAlarmRangeLimitView NewDefaultAlarmRangeLimitView()
        {
            return new DefaultAlarmRangeLimitView();
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(DefaultAlarmRangeLimitView), RecordOperation.AddNewRecord)]
        public void AddNewDefaultAlarmRangeLimitView(DefaultAlarmRangeLimitView record)
        {
            DataContext.Table<DefaultAlarmRangeLimit>().AddNewRecord(CreateNewAlarmRangeLimit(record));
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(DefaultAlarmRangeLimitView), RecordOperation.UpdateRecord)]
        public void UpdateDefaultAlarmRangeLimitView(DefaultAlarmRangeLimitView record)
        {
            DataContext.Table<DefaultAlarmRangeLimit>().UpdateRecord(CreateNewAlarmRangeLimit(record));
        }

        public DefaultAlarmRangeLimit CreateNewAlarmRangeLimit(DefaultAlarmRangeLimitView record)
        {
            DefaultAlarmRangeLimit arl = new DefaultAlarmRangeLimit();
            arl.ID = record.ID;
            arl.AlarmTypeID = record.AlarmTypeID;
            arl.MeasurementTypeID = record.MeasurementTypeID;
            arl.MeasurementCharacteristicID = record.MeasurementCharacteristicID;
            arl.Severity = record.Severity;
            arl.High = record.High;
            arl.Low = record.Low;
            arl.RangeInclusive = record.RangeInclusive;
            arl.PerUnit = record.PerUnit;
            return arl;
        }


        [AuthorizeHubRole("Administrator")]
        public void ResetDefaultLimits(DefaultAlarmRangeLimitView record)
        {
            IEnumerable<Channel> channels = DataContext.Table<Channel>().QueryRecords(restriction: new RecordRestriction("MeasurementTypeID = {0} AND MeasurementCharacteristicID = {1}", record.MeasurementTypeID, record.MeasurementCharacteristicID));
            string channelIDs = "";
            foreach (Channel channel in channels)
            {
                if(channelIDs == "")
                    channelIDs += channel.ID.ToString();
                else
                    channelIDs += ',' + channel.ID.ToString();
            }
            IEnumerable<AlarmRangeLimit> limits = DataContext.Table<AlarmRangeLimit>().QueryRecords(restriction: new RecordRestriction($"ChannelID IN ({channelIDs})"));
            foreach (AlarmRangeLimit limit in limits)
            {
                limit.IsDefault = true;
                limit.High = record.High;
                limit.Low = record.Low;
                limit.Severity = record.Severity;
                limit.RangeInclusive = record.RangeInclusive;
                limit.PerUnit = record.PerUnit;
                DataContext.Table<AlarmRangeLimit>().UpdateRecord(limit);
            }
        }

        #endregion

        #region [ Stored Procedure Operations]

        public List<TrendingData> GetTrendingData(DateTime startDate, DateTime endDate, int channelId)
        {
            List<TrendingData> trendingData = new List<TrendingData>();

            using (IDbCommand cmd = DataContext.Connection.Connection.CreateCommand())
            {
                cmd.CommandText = "dbo.selectAlarmDataByChannelByDate";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 30000;
                IDbDataParameter StartDate = cmd.CreateParameter();
                IDbDataParameter EndDate = cmd.CreateParameter();
                IDbDataParameter ChannelID = cmd.CreateParameter();

                StartDate.ParameterName = "@StartDate";
                EndDate.ParameterName = "@EndDate";
                ChannelID.ParameterName = "@ChannelID";

                StartDate.Value = startDate;
                EndDate.Value = endDate;
                ChannelID.Value = channelId;

                cmd.Parameters.Add(StartDate);
                cmd.Parameters.Add(EndDate);
                cmd.Parameters.Add(ChannelID);

                using( IDataReader reader = cmd.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        TrendingData obj = new TrendingData();

                        obj.ChannelID = Convert.ToInt32(reader["ChannelID"]);
                        obj.SeriesType = Convert.ToString(reader["SeriesType"]);
                        obj.Time = Convert.ToDateTime(reader["Time"]);
                        obj.Value = Convert.ToDouble(reader["Value"]);
                        trendingData.Add(obj);
                    }

                }
            }

            return trendingData;
           
        }

        #endregion
    }
}