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
using System.Transactions;
using GSF;
using GSF.Data.Model;
using GSF.Web.Hubs;
using GSF.Web.Model;
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
        public int QueryMeterCount(int stationID, string filterString)
        {
            filterString = (filterString ?? "%").EnsureEnd("%");

            if (stationID > 0)
                return DataContext.Table<Meter>().QueryRecordCount(new RecordRestriction("MeterLocationID = {0} AND Name LIKE {1}", stationID, filterString));
            else
                return DataContext.Table<Meter>().QueryRecordCount(new RecordRestriction("Name LIKE {0}", filterString));
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Meter), RecordOperation.QueryRecords)]
        public IEnumerable<MeterDetail> QueryMeters(int stationID, string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            filterString = (filterString ?? "%").EnsureEnd("%");

            if (stationID > 0)
                return DataContext.Table<MeterDetail>().QueryRecords(sortField, ascending, page, pageSize, new RecordRestriction("MeterLocationID = {0} AND Name LIKE {1}", stationID, filterString));
            else
                return DataContext.Table<MeterDetail>().QueryRecords(sortField, ascending, page, pageSize, new RecordRestriction("Name LIKE {0}", filterString));
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Meter), RecordOperation.DeleteRecord)]
        public void DeleteMeter(int id)
        {
            DataContext.Table<Meter>().DeleteRecord(id);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Meter), RecordOperation.CreateNewRecord)]
        public MeterDetail NewMeter()
        {
            return new MeterDetail();
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

        [AuthorizeHubRole("Administrator")]
        public IEnumerable<IDLabel> SearchMeters(string searchText, int limit = -1)
        {
            RecordRestriction restriction = new RecordRestriction("Name LIKE {0}", $"%{searchText}%");

            return DataContext.Table<Meter>().QueryRecords("Name", restriction, limit)
                .Select(meter => new IDLabel(meter.ID.ToString(), meter.Name));
        }

        [AuthorizeHubRole("Administrator")]
        public IEnumerable<IDLabel> SearchMetersByGroup(int groupID, string searchText, int limit = -1)
        {
            RecordRestriction restriction = new RecordRestriction("Name LIKE {0} AND ID NOT IN (SELECT MeterID FROM GroupMeter WHERE GroupID = {1})", $"%{searchText}%", groupID);

            return DataContext.Table<Meter>().QueryRecords("Name", restriction, limit)
                .Select(meter => new IDLabel(meter.ID.ToString(), meter.Name));
        }

        #endregion

        #region [ MeterLocation Table Operations ]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(MeterLocation), RecordOperation.QueryRecordCount)]
        public int QueryMeterLocationCount(string filterString)
        {
            filterString = (filterString ?? "%").EnsureEnd("%");
            return DataContext.Table<MeterLocation>().QueryRecordCount(new RecordRestriction("Name LIKE {0}", filterString));
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(MeterLocation), RecordOperation.QueryRecords)]
        public IEnumerable<MeterLocation> QueryMeterLocations(string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            filterString = (filterString ?? "%").EnsureEnd("%");
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

        [AuthorizeHubRole("Administrator")]
        public IEnumerable<IDLabel> SearchMeterLocations(string searchText, int limit = -1)
        {
            RecordRestriction restriction = new RecordRestriction("Name LIKE {0}", $"%{searchText}%");

            return DataContext.Table<MeterLocation>().QueryRecords("Name", restriction, limit)
                .Select(meterLocation => new IDLabel(meterLocation.ID.ToString(), meterLocation.Name));
        }

        #endregion

        #region [ Lines Table Operations ]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Line), RecordOperation.QueryRecordCount)]
        public int QueryLinesCount(string filterString)
        {
            filterString = (filterString ?? "%").EnsureEnd("%");
            return DataContext.Table<Line>().QueryRecordCount(new RecordRestriction("Name LIKE {0}", filterString));
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Line), RecordOperation.QueryRecords)]
        public IEnumerable<Line> QueryLines(string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            filterString = (filterString ?? "%").EnsureEnd("%");
            return DataContext.Table<Line>().QueryRecords(sortField, ascending, page, pageSize, new RecordRestriction("AssetKey LIKE {0}", filterString));
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Line), RecordOperation.DeleteRecord)]
        public void DeleteLines(int id)
        {
            DataContext.Table<Line>().DeleteRecord(id);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Line), RecordOperation.CreateNewRecord)]
        public Line NewLine()
        {
            return new Line();
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Line), RecordOperation.AddNewRecord)]
        public void AddNewLines(Line record)
        {
            DataContext.Table<Line>().AddNewRecord(record);
        }

        [AuthorizeHubRole("Administrator, Owner")]
        [RecordOperation(typeof(Line), RecordOperation.UpdateRecord)]
        public void UpdateLines(Line record)
        {
            DataContext.Table<Line>().UpdateRecord(record);
        }

        [AuthorizeHubRole("Administrator")]
        public IEnumerable<IDLabel> SearchLines(string searchText, int limit = -1)
        {
            RecordRestriction restriction = new RecordRestriction("AssetKey LIKE {0}", $"%{searchText}%");

            return DataContext.Table<Line>().QueryRecords("AssetKey", restriction, limit)
                .Select(line => new IDLabel(line.ID.ToString(), line.AssetKey));
        }

        #endregion

        #region [ LineView Table Operations ]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(LineView), RecordOperation.QueryRecordCount)]
        public int QueryLineViewCount(string filterString)
        {
            filterString = (filterString ?? "%").EnsureEnd("%");
            return DataContext.Table<LineView>().QueryRecordCount(new RecordRestriction("TopName LIKE {0}", filterString));
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(LineView), RecordOperation.QueryRecords)]
        public IEnumerable<LineView> QueryLineView(string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            filterString = (filterString ?? "%").EnsureEnd("%");
            return DataContext.Table<LineView>().QueryRecords(sortField, ascending, page, pageSize, new RecordRestriction("AssetKey LIKE {0}", filterString));
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(LineView), RecordOperation.DeleteRecord)]
        public void DeleteLineView(int id)
        {
            DataContext.Table<Line>().DeleteRecord(id);
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
            DataContext.Table<Line>().AddNewRecord(CreateLine(record));
            DataContext.Table<LineImpedance>().AddNewRecord(CreateLineImpedance(record));
        }

        [AuthorizeHubRole("Administrator, Owner")]
        [RecordOperation(typeof(LineView), RecordOperation.UpdateRecord)]
        public void UpdateLineView(LineView record)
        {
            DataContext.Table<Line>().UpdateRecord(CreateLine(record));
            DataContext.Table<LineImpedance>().UpdateRecord(CreateLineImpedance(record));

        }

        public Line CreateLine(LineView record)
        {
            Line line = NewLine();
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
        public IEnumerable<MeterLineDetail> QueryMeterLine(int lineID, int meterID, string sortField, bool ascending, int page, int pageSize, string filterString)
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

            return DataContext.Table<MeterLineDetail>().QueryRecords(sortField, ascending, page, pageSize, new RecordRestriction(restrictionString));
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(MeterLine), RecordOperation.DeleteRecord)]
        public void DeleteMeterLine(int id)
        {
            DataContext.Table<MeterLine>().DeleteRecord(id);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(MeterLine), RecordOperation.CreateNewRecord)]
        public MeterLineDetail NewMeterLine()
        {
            return new MeterLineDetail();
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
        public int QueryChannelCount(int meterID, int lineID, string filterString)
        {
            var filters = (filterString ?? "").ParseKeyValuePairs()
                .Select(kvp => new { Field = kvp.Key, Operator = "LIKE", Value = kvp.Value.EnsureEnd("%") })
                .ToList();

            filters.Add(new { Field = "MeterID", Operator = "=", Value = meterID.ToString() });
            filters.Add(new { Field = "LineID", Operator = "=", Value = lineID.ToString() });

            string expression = string.Join(" AND ", filters.Select((filter, index) => $"{filter.Field} {filter.Operator} {{{index}}}"));
            object[] parameters = filters.Select(filter => (object)filter.Value).ToArray();

            return DataContext.Table<Channel>().QueryRecordCount(new RecordRestriction(expression, parameters));
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Channel), RecordOperation.QueryRecords)]
        public IEnumerable<ChannelDetail> QueryChannel(int meterID, int lineID, string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            var filters = (filterString ?? "").ParseKeyValuePairs()
                .Select(kvp => new { Field = kvp.Key, Operator = "LIKE", Value = kvp.Value.EnsureEnd("%") })
                .ToList();

            filters.Add(new { Field = "MeterID", Operator = "=", Value = meterID.ToString() });
            filters.Add(new { Field = "LineID", Operator = "=", Value = lineID.ToString() });

            string expression = string.Join(" AND ", filters.Select((filter, index) => $"{filter.Field} {filter.Operator} {{{index}}}"));
            object[] parameters = filters.Select(filter => (object)filter.Value).ToArray();

            return DataContext.Table<ChannelDetail>().QueryRecords(sortField, ascending, page, pageSize, new RecordRestriction(expression, parameters));
        }

        public IEnumerable<Channel> QueryChannelsForDropDown(string filterString)
        {
            return DataContext.Table<Channel>().QueryRecords(restriction: new RecordRestriction("Name LIKE {0}", filterString), limit: 50);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Channel), RecordOperation.DeleteRecord)]
        public void DeleteChannel(int id)
        {
            DataContext.Table<Channel>().DeleteRecord(id);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Channel), RecordOperation.CreateNewRecord)]
        public ChannelDetail NewChannel()
        {
            return new ChannelDetail();
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Channel), RecordOperation.AddNewRecord)]
        public void AddNewChannel(ChannelDetail record)
        {
            DataContext.Table<Channel>().AddNewRecord(record);
            record.ID = DataContext.Connection.ExecuteScalar<int>("SELECT @@IDENTITY");

            if (!string.IsNullOrEmpty(record.Mapping))
            {
                using (TransactionScope transaction = new TransactionScope())
                {
                    bool seriesTypeExists = DataContext.Connection.ExecuteScalar<int>("SELECT COUNT(*) FROM SeriesType WHERE Name = 'Values'") > 0;

                    if (!seriesTypeExists)
                        DataContext.Connection.ExecuteNonQuery("INSERT INTO SeriesType VALUES('Values', 'Values')");

                    transaction.Complete();
                }

                int seriesTypeID = DataContext.Connection.ExecuteScalar<int>("SELECT ID FROM SeriesType WHERE Name = 'Values'");
                DataContext.Connection.ExecuteNonQuery("INSERT INTO Series VALUES({0}, {1}, {2})", record.ID, seriesTypeID, record.Mapping);
            }
        }

        [AuthorizeHubRole("Administrator, Owner")]
        [RecordOperation(typeof(Channel), RecordOperation.UpdateRecord)]
        public void UpdateChannel(ChannelDetail record)
        {
            DataContext.Table<Channel>().UpdateRecord(record);

            if (!string.IsNullOrEmpty(record.Mapping))
            {
                using (TransactionScope transaction = new TransactionScope())
                {
                    bool seriesTypeExists = DataContext.Connection.ExecuteScalar<int>("SELECT COUNT(*) FROM SeriesType WHERE Name = 'Values'") > 0;

                    if (!seriesTypeExists)
                        DataContext.Connection.ExecuteNonQuery("INSERT INTO SeriesType VALUES('Values', 'Values')");

                    transaction.Complete();
                }

                int seriesTypeID = DataContext.Connection.ExecuteScalar<int>("SELECT ID FROM SeriesType WHERE Name = 'Values'");

                using (TransactionScope transaction = new TransactionScope())
                {
                    bool seriesExists = DataContext.Connection.ExecuteScalar<int>("SELECT COUNT(*) FROM Series WHERE ChannelID = {0} AND SeriesTypeID = {1}", record.ID, seriesTypeID) > 0;

                    if (seriesExists)
                        DataContext.Connection.ExecuteNonQuery("UPDATE Series SET SourceIndexes = {0} WHERE ChannelID = {1} AND SeriesTypeID = {2}", record.Mapping, record.ID, seriesTypeID);
                    else
                        DataContext.Connection.ExecuteNonQuery("INSERT INTO Series VALUES({0}, {1}, {2})", record.ID, seriesTypeID, record.Mapping);

                    transaction.Complete();
                }
            }
            else
            {
                int seriesTypeID = DataContext.Connection.ExecuteScalar(defaultValue: -1, sqlFormat: "SELECT ID FROM SeriesType WHERE Name = 'Values'");
                DataContext.Connection.ExecuteNonQuery("UPDATE Series SET SourceIndexes = '' WHERE ChannelID = {0} AND SeriesTypeID = {1}", record.ID, seriesTypeID);
            }
        }

        [AuthorizeHubRole("Administrator")]
        public IEnumerable<IDLabel> SearchMeasurementTypes(string searchText, int limit = -1)
        {
            string limitText = (limit > 0) ? $"TOP {limit}" : "";

            return DataContext.Connection
                .RetrieveData($"SELECT {limitText} ID, Name FROM MeasurementType WHERE Name LIKE {{0}}", $"%{searchText}%")
                .Select()
                .Select(row => new IDLabel(row["ID"].ToString(), row["Name"].ToString()));
        }

        [AuthorizeHubRole("Administrator")]
        public IEnumerable<IDLabel> SearchMeasurementCharacteristics(string searchText, int limit = -1)
        {
            string limitText = (limit > 0) ? $"TOP {limit}" : "";

            return DataContext.Connection
                .RetrieveData($"SELECT {limitText} ID, Name FROM MeasurementCharacteristic WHERE Name LIKE {{0}}", $"%{searchText}%")
                .Select()
                .Select(row => new IDLabel(row["ID"].ToString(), row["Name"].ToString()));
        }

        [AuthorizeHubRole("Administrator")]
        public IEnumerable<IDLabel> SearchPhases(string searchText, int limit = -1)
        {
            string limitText = (limit > 0) ? $"TOP {limit}" : "";

            return DataContext.Connection
                .RetrieveData($"SELECT {limitText} ID, Name FROM Phase WHERE Name LIKE {{0}}", $"%{searchText}%")
                .Select()
                .Select(row => new IDLabel(row["ID"].ToString(), row["Name"].ToString()));
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
            gm.ID = record.ID;
            gm.GroupID = record.GroupID;
            gm.MeterID = record.MeterID;
            return gm;
        }

        #endregion

        #region [ UserGroupView Table Operations ]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(UserGroupView), RecordOperation.QueryRecordCount)]
        public int QueryUserGroupViewCount(int groupID, string filterString)
        {
            return DataContext.Table<UserGroupView>().QueryRecordCount(new RecordRestriction("GroupID = {0}", groupID));
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(UserGroupView), RecordOperation.QueryRecords)]
        public IEnumerable<UserGroupView> QueryUserGroupViews(int groupID, string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            return DataContext.Table<UserGroupView>().QueryRecords(sortField, ascending, page, pageSize, new RecordRestriction("GroupID = {0}", groupID));
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(UserGroupView), RecordOperation.DeleteRecord)]
        public void DeleteUserGroupView(int id)
        {
            DataContext.Table<UserGroup>().DeleteRecord(id);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(UserGroupView), RecordOperation.CreateNewRecord)]
        public UserGroupView NewUserGroupView()
        {
            return new UserGroupView();
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(UserGroupView), RecordOperation.AddNewRecord)]
        public void AddNewUserGroupView(UserGroupView record)
        {
            DataContext.Table<UserGroup>().AddNewRecord(CreateNewUserGroup(record));
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(UserGroupView), RecordOperation.UpdateRecord)]
        public void UpdateUserGroupView(UserGroupView record)
        {
            DataContext.Table<UserGroup>().UpdateRecord(CreateNewUserGroup(record));
        }

        [AuthorizeHubRole("Administrator")]
        public IEnumerable<IDLabel> SearchUsersByGroup(int groupID, string searchText, int limit = -1)
        {
            RecordRestriction restriction = new RecordRestriction("Name LIKE {0} AND ID NOT IN (SELECT UserID FROM UserGroup WHERE GroupID = {1})", $"%{searchText}%", groupID);

            return DataContext.Table<User>().QueryRecords("Name", restriction, limit)
                .Select(user => new IDLabel(user.ID.ToString(), user.Name));
        }

        public UserGroup CreateNewUserGroup(UserGroupView record)
        {
            UserGroup gm = new UserGroup();
            gm.ID = record.ID;
            gm.GroupID = record.GroupID;
            gm.UserID = record.UserID;
            return gm;
        }

        #endregion

        #region [ User Table Operations ]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(User), RecordOperation.QueryRecordCount)]
        public int QueryUserCount(string filterString)
        {
            if (filterString == null) filterString = "%";
            else
            {
                // Build your filter string here!
                filterString += "%";
            }

            return DataContext.Table<User>().QueryRecordCount(new RecordRestriction("Name LIKE {0}", filterString));
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(User), RecordOperation.QueryRecords)]
        public IEnumerable<User> QueryUsers(string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            if (filterString == null) filterString = "%";
            else
            {
                // Build your filter string here!
                filterString += "%";
            }

            return DataContext.Table<User>().QueryRecords(sortField, ascending, page, pageSize, new RecordRestriction("Name LIKE {0}", filterString));
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(User), RecordOperation.DeleteRecord)]
        public void DeleteUser(int id)
        {
            DataContext.Table<User>().DeleteRecord(id);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(User), RecordOperation.CreateNewRecord)]
        public User NewUser()
        {
            return new User();
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(User), RecordOperation.AddNewRecord)]
        public void AddNewUser(User record)
        {
            DataContext.Table<User>().AddNewRecord(record);
        }

        [AuthorizeHubRole("Administrator, Owner")]
        [RecordOperation(typeof(User), RecordOperation.UpdateRecord)]
        public void UpdateUser(User record)
        {
            DataContext.Table<User>().UpdateRecord(record);
        }

        public IEnumerable<User> GetUsers()
        {
            return DataContext.Table<User>().QueryRecords(restriction: new RecordRestriction("Active <> 0"));
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

        #region [ Stored Procedure Operations ]

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