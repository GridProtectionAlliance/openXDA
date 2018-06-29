﻿//******************************************************************************************************
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
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using FaultData.DataAnalysis;
using GSF;
using GSF.Collections;
using GSF.Data;
using GSF.Data.Model;
using GSF.Identity;
using GSF.Security.Model;
using GSF.Web.Hubs;
using GSF.Web.Model;
using GSF.Web.Security;
using Microsoft.AspNet.SignalR;
using openHistorian.XDALink;
using openXDA.DataPusher;
using openXDA.Model;
using Channel = openXDA.Model.Channel;
using ChannelDetail = openXDA.Model.ChannelDetail;
using Disturbance = openXDA.Model.Disturbance;
using Event = openXDA.Model.Event;
using Fault = openXDA.Model.Fault;
using Line = openXDA.Model.Line;
using LineImpedance = openXDA.Model.LineImpedance;
using Meter = openXDA.Model.Meter;
using MeterDetail = openXDA.Model.MeterDetail;
using MeterLine = openXDA.Model.MeterLine;
using MeterLocation = openXDA.Model.MeterLocation;
using MeterMeterGroup = openXDA.Model.MeterMeterGroup;
using Setting = openXDA.Model.Setting;

namespace openXDA.Hubs
{
    [AuthorizeHubRole]
    public class DataHub : RecordOperationsHub<DataHub>
    {
        // Client-side script functionality
        #region [ Constructor ]
        public DataHub() : base(OnLogStatusMessage, OnLogExceptionMessage) { }
        #endregion

        #region [ Static ]

        public static event EventHandler ReloadSystemSettingsEvent;

        private static void OnReloadSystemSettings()
        {
            ReloadSystemSettingsEvent?.Invoke(new object(), null);
        }


        public static event EventHandler<EventArgs<string, UpdateType>> LogStatusMessageEvent;
                
        private static void OnLogStatusMessage(string message, UpdateType updateType = UpdateType.Information)
        {
            LogStatusMessageEvent?.Invoke(new object(),new EventArgs<string, UpdateType>(message, updateType));
        }

        public static event EventHandler<EventArgs<int, int>> ReprocessFilesEvent;

        private static void OnReprocessFile(int fileGroupId, int meterId)
        {
            ReprocessFilesEvent?.Invoke(new object(), new EventArgs<int, int>(fileGroupId, meterId));
        }

        public static event EventHandler<EventArgs<Exception>> LogExceptionMessage;

        private static void OnLogExceptionMessage(Exception exception)
        {
            LogExceptionMessage?.Invoke(new object(), new EventArgs<Exception>(exception));
        }


        public static string LocalXDAInstance
        {
            get
            {
                using (DataContext dataContext = new DataContext("systemSettings"))
                {
                    return dataContext.Table<Setting>().QueryRecordWhere("Name = 'LocalXDAInstance'").Value ?? "http://127.0.0.1:8989";
                }
            }
        }

        public static string RemoteXDAInstance
        {
            get
            {
                using (DataContext dataContext = new DataContext("systemSettings"))
                {
                    return dataContext.Table<Setting>().QueryRecordWhere("Name = 'RemoteXDAInstance'").Value ?? "http://127.0.0.1:8989";
                }
            }
        }

        public static string CompanyName
        {
            get
            {
                using (DataContext dataContext = new DataContext("systemSettings"))
                {
                    return dataContext.Table<Setting>().QueryRecordWhere("Name = 'CompanyName'").Value;
                }
            }
        }

        public static void ProgressUpdatedByMeter(object sender, EventArgs<string, string, int> e)
        {
            string clientID = e.Argument1;

            if ((object)clientID != null)
                GlobalHost.ConnectionManager.GetHubContext<DataHub>().Clients.Client(clientID).updateProgressBarForMeter(e.Argument2, e.Argument3);
            else
                GlobalHost.ConnectionManager.GetHubContext<DataHub>().Clients.All.updateProgressBarForMeter(e.Argument2, e.Argument3);
        }

        public static void ProgressUpdatedByInstance(object sender, EventArgs<string, string, int> e)
        {
            string clientID = e.Argument1;

            if ((object)clientID != null)
                GlobalHost.ConnectionManager.GetHubContext<DataHub>().Clients.Client(clientID).updateProgressBarForInstance(e.Argument2, e.Argument3);
            else
                GlobalHost.ConnectionManager.GetHubContext<DataHub>().Clients.All.updateProgressBarForInstance(e.Argument2, e.Argument3);
        }

        public void ProgressUpdatedByMeter(string meterName, int value)
        {
            GlobalHost.ConnectionManager.GetHubContext<DataHub>().Clients.All.updateProgressBarForMeter(meterName, value);
        }

        public static void ProgressUpdatedOverall(string meterName, int value)
        {
            GlobalHost.ConnectionManager.GetHubContext<DataHub>().Clients.All.updateProgressBarForOverall(meterName, value);
        }


        #endregion

        #region [Config Page]

        #region [ Index Page ]
        public void ReloadSystemSetting()
        {
            OnReloadSystemSettings();
        }

        #endregion

        #region [ Setting Table Operations ]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Setting), RecordOperation.QueryRecordCount)]
        public int QuerySettingCount(string filterString)
        {
            return DataContext.Table<Setting>().QueryRecordCount(filterString);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Setting), RecordOperation.QueryRecords)]
        public IEnumerable<Setting> QuerySettings(string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            return DataContext.Table<Setting>().QueryRecords(sortField, ascending, page, pageSize, filterString);
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
        public void AddNewSetting(Setting record)
        {
            DataContext.Table<Setting>().AddNewRecord(record);
        }

        [AuthorizeHubRole("Administrator, Owner")]
        [RecordOperation(typeof(Setting), RecordOperation.UpdateRecord)]
        public void UpdateSetting(Setting record)
        {
            DataContext.Table<Setting>().UpdateRecord(record);
        }

        #endregion

        #region [ DashSettings Table Operations ]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(DashSettings), RecordOperation.QueryRecordCount)]
        public int QueryDashSettingsCount(string filterString)
        {
            return DataContext.Table<DashSettings>().QueryRecordCount(filterString);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(DashSettings), RecordOperation.QueryRecords)]
        public IEnumerable<DashSettings> QueryDashSettingss(string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            return DataContext.Table<DashSettings>().QueryRecords(sortField, ascending, page, pageSize, filterString);
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
        public void AddNewDashSetting(DashSettings record)
        {
            DataContext.Table<DashSettings>().AddNewRecord(record);
        }

        [AuthorizeHubRole("Administrator, Owner")]
        [RecordOperation(typeof(DashSettings), RecordOperation.UpdateRecord)]
        public void UpdateDashSetting(DashSettings record)
        {
            DataContext.Table<DashSettings>().UpdateRecord(record);
        }

        #endregion

        #region [ UserDashSettings Table Operations ]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(UserDashSettings), RecordOperation.QueryRecordCount)]
        public int QueryUserDashSettingsCount(string filterString)
        {
            return DataContext.Table<UserDashSettings>().QueryRecordCount(filterString);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(UserDashSettings), RecordOperation.QueryRecords)]
        public IEnumerable<UserDashSettings> QueryUserDashSettings(string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            return DataContext.Table<UserDashSettings>().QueryRecords(sortField, ascending, page, pageSize, filterString);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(UserDashSettings), RecordOperation.DeleteRecord)]
        public void DeleteUserDashSettings(int id)
        {
            DataContext.Table<UserDashSettings>().DeleteRecord(id);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(UserDashSettings), RecordOperation.CreateNewRecord)]
        public UserDashSettings NewUserDashSettings()
        {
            return new UserDashSettings();
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(UserDashSettings), RecordOperation.AddNewRecord)]
        public void AddNewUSerDashSetting(UserDashSettings record)
        {
            DataContext.Table<UserDashSettings>().AddNewRecord(record);
        }

        [AuthorizeHubRole("Administrator, Owner")]
        [RecordOperation(typeof(UserDashSettings), RecordOperation.UpdateRecord)]
        public void UpdateUserDashSetting(UserDashSettings record)
        {
            DataContext.Table<UserDashSettings>().UpdateRecord(record);
        }

        #endregion

        #region [ UserAccount Operations]
        public void DeleteUserAccount(Guid id)
        {
            CascadeDelete("UserAccount", $"ID = '{id}'");
        }

        #endregion

        #region [ MeterGroup Table Operations ]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(MeterGroup), RecordOperation.QueryRecordCount)]
        public int QueryGroupCount(int meterGroupID, string filterString)
        {
            TableOperations<MeterGroup> tableOperations = DataContext.Table<MeterGroup>();
            RecordRestriction restriction;

            if (meterGroupID > 0)
                restriction = tableOperations.GetSearchRestriction(filterString) + new RecordRestriction("ID = {0}", meterGroupID);
            else
                restriction = tableOperations.GetSearchRestriction(filterString);

            return tableOperations.QueryRecordCount(restriction);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(MeterGroup), RecordOperation.QueryRecords)]
        public IEnumerable<MeterGroup> QueryGroups(int meterGroupID, string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            TableOperations<MeterGroup> tableOperations = DataContext.Table<MeterGroup>();
            RecordRestriction restriction;

            if (meterGroupID > 0)
                restriction = tableOperations.GetSearchRestriction(filterString) + new RecordRestriction("ID = {0}", meterGroupID);
            else
                restriction = tableOperations.GetSearchRestriction(filterString);

            return tableOperations.QueryRecords(sortField, ascending, page, pageSize, restriction);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(MeterGroup), RecordOperation.DeleteRecord)]
        public void DeleteGroup(int id)
        {
            CascadeDelete("MeterGroup", $"ID={id}");
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(MeterGroup), RecordOperation.CreateNewRecord)]
        public MeterGroup NewGroup()
        {
            return new MeterGroup();
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(MeterGroup), RecordOperation.AddNewRecord)]
        public void AddNewGroup(MeterGroup record)
        {
            DataContext.Table<MeterGroup>().AddNewRecord(record);
        }

        [AuthorizeHubRole("Administrator, Owner")]
        [RecordOperation(typeof(MeterGroup), RecordOperation.UpdateRecord)]
        public void UpdateGroup(MeterGroup record)
        {
            DataContext.Table<MeterGroup>().UpdateRecord(record);
        }

        public int GetLastMeterGroupID()
        {
            return DataContext.Connection.ExecuteScalar<int?>("SELECT IDENT_CURRENT('MeterGroup')") ?? 0;
        }


        public void UpdateMeters(List<string> meters, int groupID )
        {
            IEnumerable<MeterMeterGroup> records = DataContext.Table<MeterMeterGroup>().QueryRecords(restriction: new RecordRestriction("MeterGroupID = {0}", groupID));
            foreach (MeterMeterGroup record in records)
            {
                if (!meters.Contains(record.MeterID.ToString()))
                    DataContext.Table<MeterMeterGroup>().DeleteRecord(record.ID);
            }

            foreach (string meter in meters)
            {
                if (!records.Any(record => record.MeterID == int.Parse(meter)))
                {
                    DataContext.Table<MeterMeterGroup>().AddNewRecord(new MeterMeterGroup() { MeterGroupID = groupID, MeterID = int.Parse(meter) });
                }
            }
        }

        public void UpdateUsers(List<string> users, int groupID)
        {
            IEnumerable<UserAccountMeterGroup> records = DataContext.Table<UserAccountMeterGroup>().QueryRecords(restriction: new RecordRestriction("MeterGroupID = {0}", groupID));
            foreach (UserAccountMeterGroup record in records)
            {
                if (!users.Contains(record.UserAccountID.ToString()))
                    DataContext.Table<UserAccountMeterGroup>().DeleteRecord(record.ID);
            }

            foreach (string user in users)
            {
                if (!records.Any(record => record.UserAccountID == Guid.Parse(user)))
                {
                    DataContext.Table<UserAccountMeterGroup>().AddNewRecord(new UserAccountMeterGroup() { MeterGroupID = groupID, UserAccountID = Guid.Parse(user) });
                }
            }
        }


        #endregion

        #region [ LineGroup Table Operations ]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(LineGroup), RecordOperation.QueryRecordCount)]
        public int QueryLineGroupCount(string filterString)
        {
            return DataContext.Table<LineGroup>().QueryRecordCount(filterString);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(LineGroup), RecordOperation.QueryRecords)]
        public IEnumerable<LineGroup> QueryLineGroups(string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            return DataContext.Table<LineGroup>().QueryRecords(sortField, ascending, page, pageSize, filterString);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(LineGroup), RecordOperation.DeleteRecord)]
        public void DeleteLineGroup(int id)
        {
            CascadeDelete("LineGroup", $"ID={id}");
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(LineGroup), RecordOperation.CreateNewRecord)]
        public LineGroup NewLineGroup()
        {
            return new LineGroup();
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(LineGroup), RecordOperation.AddNewRecord)]
        public void AddNewLineGroup(LineGroup record)
        {
            DataContext.Table<LineGroup>().AddNewRecord(record);
        }

        [AuthorizeHubRole("Administrator, Owner")]
        [RecordOperation(typeof(LineGroup), RecordOperation.UpdateRecord)]
        public void UpdateLineGroup(LineGroup record)
        {
            DataContext.Table<LineGroup>().UpdateRecord(record);
        }

        public int GetLastLineGroupID()
        {
            return DataContext.Connection.ExecuteScalar<int?>("SELECT IDENT_CURRENT('LineGroup')") ?? 0;
        }

        public void UpdateLines(List<string> lines, int groupID)
        {
            if(lines == null)
            {
                lines = new List<string>();
            }

            IEnumerable<LineLineGroup> records = DataContext.Table<LineLineGroup>().QueryRecords(restriction: new RecordRestriction("LineGroupID = {0}", groupID));
            foreach (LineLineGroup record in records)
            {
                if (!lines.Contains(record.LineID.ToString()))
                    DataContext.Table<LineLineGroup>().DeleteRecord(record.ID);
            }

            foreach (string line in lines)
            {
                if (!records.Any(record => record.LineID == int.Parse(line)))
                {
                    DataContext.Table<LineLineGroup>().AddNewRecord(new LineLineGroup() { LineGroupID = groupID, LineID = int.Parse(line) });
                }
            }
        }


        #endregion

        #region [ MeterMeterGroup Table Operations ]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(MeterMeterGroup), RecordOperation.QueryRecordCount)]
        public int QueryGroupMeterViewCount(int groupID, string filterString)
        {
            TableOperations<MeterMeterGroupView> tableOperations = DataContext.Table<MeterMeterGroupView>();
            RecordRestriction restriction;

            if (groupID > 0)
                restriction = tableOperations.GetSearchRestriction(filterString) + new RecordRestriction("MeterGroupID = {0}", groupID);
            else
                restriction = tableOperations.GetSearchRestriction(filterString);

            return tableOperations.QueryRecordCount(restriction);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(MeterMeterGroup), RecordOperation.QueryRecords)]
        public IEnumerable<MeterMeterGroupView> QueryGroupMeterViews(int groupID, string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            TableOperations<MeterMeterGroupView> tableOperations = DataContext.Table<MeterMeterGroupView>();
            RecordRestriction restriction;

            if (groupID > 0)
                restriction = tableOperations.GetSearchRestriction(filterString) + new RecordRestriction("MeterGroupID = {0}", groupID);
            else
                restriction = tableOperations.GetSearchRestriction(filterString);

            return tableOperations.QueryRecords(sortField, ascending, page, pageSize, restriction);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(MeterMeterGroup), RecordOperation.DeleteRecord)]
        public void DeleteGroupMeterView(int id)
        {
            DataContext.Table<MeterMeterGroup>().DeleteRecord(id);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(MeterMeterGroup), RecordOperation.CreateNewRecord)]
        public MeterMeterGroupView NewGroupMeterView()
        {
            return new MeterMeterGroupView();
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(MeterMeterGroup), RecordOperation.AddNewRecord)]
        public void AddNewGroupMeterView(MeterMeterGroup record)
        {
            DataContext.Table<MeterMeterGroup>().AddNewRecord(record);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(MeterMeterGroup), RecordOperation.UpdateRecord)]
        public void UpdateGroupMeterView(MeterMeterGroup record)
        {
            DataContext.Table<MeterMeterGroup>().UpdateRecord(record);
        }

        [AuthorizeHubRole("Administrator")]
        public IEnumerable<IDLabel> SearchMeterMeterGroupByGroup(int groupID, string searchText, int limit = -1)
        {
            RecordRestriction restriction = new RecordRestriction("Name LIKE {0} AND ID NOT IN (SELECT MeterID FROM MeterMeterGroup WHERE MeterGroupID = {1})", $"%{searchText}%", groupID);

            return DataContext.Table<Meter>().QueryRecords("Name", restriction, limit)
                .Select(meter => new IDLabel(meter.ID.ToString(), meter.Name));
        }


        #endregion

        #region [ LineLineGroup Table Operations ]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(LineLineGroup), RecordOperation.QueryRecordCount)]
        public int QueryLineLineGroupViewCount(int groupID, string filterString)
        {
            TableOperations<LineLineGroupView> tableOperations = DataContext.Table<LineLineGroupView>();
            RecordRestriction restriction;

            if (groupID > 0)
                restriction = tableOperations.GetSearchRestriction(filterString) + new RecordRestriction("LineGroupID = {0}", groupID);
            else
                restriction = tableOperations.GetSearchRestriction(filterString);

            return tableOperations.QueryRecordCount(restriction);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(LineLineGroup), RecordOperation.QueryRecords)]
        public IEnumerable<LineLineGroupView> QueryLineLineGroupViews(int groupID, string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            TableOperations<LineLineGroupView> tableOperations = DataContext.Table<LineLineGroupView>();
            RecordRestriction restriction;

            if (groupID > 0)
                restriction = tableOperations.GetSearchRestriction(filterString) + new RecordRestriction("LineGroupID = {0}", groupID);
            else
                restriction = tableOperations.GetSearchRestriction(filterString);

            return tableOperations.QueryRecords(sortField, ascending, page, pageSize, restriction);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(LineLineGroup), RecordOperation.DeleteRecord)]
        public void DeleteLineLineGroupView(int id)
        {
            DataContext.Table<LineLineGroup>().DeleteRecord(id);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(LineLineGroup), RecordOperation.CreateNewRecord)]
        public LineLineGroupView NewLineLineGroupView()
        {
            return new LineLineGroupView();
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(LineLineGroup), RecordOperation.AddNewRecord)]
        public void AddNewLineLineGroupView(LineLineGroup record)
        {
            DataContext.Table<LineLineGroup>().AddNewRecord(record);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(LineLineGroup), RecordOperation.UpdateRecord)]
        public void UpdateLineLineGroupView(LineLineGroup record)
        {
            DataContext.Table<LineLineGroup>().UpdateRecord(record);
        }

        public IEnumerable<IDLabel> SearchLinesForLineLineGroupByGroup(int groupID, string searchText, int limit = -1)
        {
            RecordRestriction restriction = new RecordRestriction("AssetKey LIKE {0} AND ID NOT IN (SELECT LineID FROM LineLineGroup WHERE LineGroupID = {1})", $"%{searchText}%", groupID);

            return DataContext.Table<Line>().QueryRecords("AssetKey", restriction, limit)
                .Select(line => new IDLabel(line.ID.ToString(), line.AssetKey));
        }

        #endregion

        #region [ UserAccountMeterGroup Table Operations ]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(UserAccountMeterGroup), RecordOperation.QueryRecordCount)]
        public int QueryUserAccountMeterGroupCount(int groupID, string filterString)
        {
            if (filterString != null && filterString != string.Empty)
                return DataContext.Table<UserAccountMeterGroupView>().QueryRecordsWhere("MeterGroupID = {0}", groupID).Where(x => UserInfo.SIDToAccountName(x.Username).ToLower().Contains(filterString.ToLower())).Count();
            else
                return DataContext.Table<UserAccountMeterGroupView>().QueryRecordsWhere("MeterGroupID = {0}", groupID).Count();

        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(UserAccountMeterGroup), RecordOperation.QueryRecords)]
        public IEnumerable<UserAccountMeterGroupView> QueryUserAccountMeterGroups(int groupID, string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            Func<UserAccountMeterGroupView, bool> whereFunc;

            if (filterString != null && filterString != string.Empty)
                whereFunc = x => UserInfo.SIDToAccountName(x.Username).ToLower().Contains(filterString.ToLower());
            else
                whereFunc = x => true;

            if (ascending)
                return DataContext.Table<UserAccountMeterGroupView>().QueryRecords(sortField, ascending, page, pageSize, new RecordRestriction("MeterGroupID = {0}", groupID))
                    .Where(whereFunc).OrderBy(x => UserInfo.SIDToAccountName(x.Username), StringComparer.OrdinalIgnoreCase);
            else
                return DataContext.Table<UserAccountMeterGroupView>().QueryRecords(sortField, ascending, page, pageSize, new RecordRestriction("MeterGroupID = {0}", groupID))
                    .Where(whereFunc).OrderByDescending(x => UserInfo.SIDToAccountName(x.Username), StringComparer.OrdinalIgnoreCase);

        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(UserAccountMeterGroup), RecordOperation.DeleteRecord)]
        public void DeleteUserAccountMeterGroup(int id)
        {
            DataContext.Table<UserAccountMeterGroup>().DeleteRecord(id);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(UserAccountMeterGroup), RecordOperation.CreateNewRecord)]
        public UserAccountMeterGroupView NewUserAccountMeterGroup()
        {
            return new UserAccountMeterGroupView();
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(UserAccountMeterGroup), RecordOperation.AddNewRecord)]
        public void AddNewUserAccountMeterGroup(UserAccountMeterGroup record)
        {
            DataContext.Table<UserAccountMeterGroup>().AddNewRecord(record);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(UserAccountMeterGroup), RecordOperation.UpdateRecord)]
        public void UpdateUserAccountMeterGroup(UserAccountMeterGroup record)
        {
            DataContext.Table<UserAccountMeterGroup>().UpdateRecord(record);
        }

        [AuthorizeHubRole("Administrator")]
        public IEnumerable<IDLabel> SearchUsersByGroup(int groupID, string searchText, int limit = -1)
        {
            RecordRestriction restriction = new RecordRestriction("ID NOT IN (SELECT UserAccountID FROM UserAccountMeterGroup WHERE MeterGroupID = {0})",  groupID);


            if (limit < 1)
                return DataContext
                    .Table<UserAccount>()
                    .QueryRecords(restriction: restriction)
                    .Select(record =>
                    {
                        record.Name = UserInfo.SIDToAccountName(record.Name ?? "");
                        return record;
                    })
                    .Where(record => record.Name?.ToLower().Contains(searchText.ToLower()) ?? false)
                    .Select(record => IDLabel.Create(record.ID.ToString(), record.Name));

            return DataContext
                .Table<UserAccount>()
                .QueryRecords(restriction: restriction)
                .Select(record =>
                {
                    record.Name = UserInfo.SIDToAccountName(record.Name ?? "");
                    return record;
                })
                .Where(record => record.Name?.ToLower().Contains(searchText.ToLower()) ?? false)
                .Take(limit)
                .Select(record => IDLabel.Create(record.ID.ToString(), record.Name));

        }

        #endregion

        #region [ User Table Operations ]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(User), RecordOperation.QueryRecordCount)]
        public int QueryUserCount(string filterString)
        {
            return DataContext.Table<User>().QueryRecordCount(filterString);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(User), RecordOperation.QueryRecords)]
        public IEnumerable<User> QueryUsers(string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            return DataContext.Table<User>().QueryRecords(sortField, ascending, page, pageSize, filterString);
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
        public int QueryAlarmRangeLimitViewCount(int meterID, int lineID, string filterString)
        {
            string meterFilter = "%";
            string channelFilter = "%";
            string typeFilter = "%";
            string charFilter = "%";

            if (filterString != "%")
            {
                string[] filters = filterString.Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]").Replace("*", "%").Split(';');
                if (filters.Length == 4)
                {
                    meterFilter = filters[0] + '%';
                    channelFilter = filters[1] + '%';
                    typeFilter = filters[2] += '%';
                    charFilter = filters[3] += '%';
                }
            }

            string MeterID = (meterID == -1 ? "%" : meterID.ToString());
            string LineID = (lineID == -1 ? "%" : lineID.ToString());

            return DataContext.Table<AlarmRangeLimitView>().QueryRecordCount(new RecordRestriction("MeterName Like {0} AND Name LIKE {1} AND MeasurementType LIKE {2} AND MeasurementCharacteristic LIKE {3}" + (MeterID == "%"? "": " AND MeterID = {4} AND LineID = {5}"), meterFilter, channelFilter, typeFilter, charFilter, MeterID, LineID));
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(AlarmRangeLimitView), RecordOperation.QueryRecords)]
        public IEnumerable<AlarmRangeLimitView> QueryAlarmRangeLimitViews(int meterID, int lineID, string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            string meterFilter = "%";
            string channelFilter = "%";
            string typeFilter = "%";
            string charFilter = "%";

            if (filterString != "%")
            {
                string[] filters = filterString.Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]").Replace("*", "%").Split(';');
                if (filters.Length == 4)
                {
                    meterFilter = filters[0] + '%';
                    channelFilter = filters[1] + '%';
                    typeFilter = filters[2] += '%';
                    charFilter = filters[3] += '%';
                }
            }

            string MeterID = (meterID == -1 ? "%" : meterID.ToString());
            string LineID = (lineID == -1 ? "%" : lineID.ToString());


            return DataContext.Table<AlarmRangeLimitView>().QueryRecords(sortField, ascending, page, pageSize, new RecordRestriction("MeterName Like {0} AND Name LIKE {1} AND MeasurementType LIKE {2} AND MeasurementCharacteristic LIKE {3}" + (MeterID == "%" ? "" : " AND MeterID = {4} AND LineID = {5}"), meterFilter, channelFilter, typeFilter, charFilter, MeterID, LineID));
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
                csv += limit.ToCSV() + '\n';
                //csv += limit.ID.ToString() + ',' + limit.ChannelID.ToString() + ',' + limit.Name.ToString() + ',' + limit.AlarmTypeID.ToString() + ',' + limit.Severity.ToString() + ',' + limit.High.ToString() + ',' 
                //    + limit.Low.ToString() + ',' + limit.RangeInclusive.ToString() + ',' + limit.PerUnit.ToString() + ',' + limit.Enabled.ToString() + ',' + limit.MeasurementType.ToString() + ','
                //    + limit.MeasurementTypeID.ToString() + ',' + limit.MeasurementCharacteristic.ToString() + ',' + limit.MeasurementCharacteristicID.ToString() + ',' + limit.Phase.ToString() + ','
                //    + limit.PhaseID.ToString() + ',' + limit.HarmonicGroup.ToString() + ','  + limit.IsDefault.ToString() + "\n";
            }

            return csv;
        }

        #endregion

        #region [ MetersWithHourlyLimits Table Operations ]

        [RecordOperation(typeof(MetersWithHourlyLimits), RecordOperation.QueryRecordCount)]
        public int QueryMetersWithHourlyLimitsCount(string filterText)
        {

            return DataContext.Table<MetersWithHourlyLimits>().QueryRecordCount(filterText);
        }

        [RecordOperation(typeof(MetersWithHourlyLimits), RecordOperation.QueryRecords)]
        public IEnumerable<MetersWithHourlyLimits> QueryMetersWithHourlyLimits(string sortField, bool ascending, int page, int pageSize, string filterText)
        {
            return DataContext.Table<MetersWithHourlyLimits>().QueryRecords(sortField, ascending, page, pageSize, filterText);
        }

        [AuthorizeHubRole("Administrator, Editor")]
        [RecordOperation(typeof(MetersWithHourlyLimits), RecordOperation.DeleteRecord)]
        public void DeleteMetersWithHourlyLimits(int id)
        {
            DataContext.Table<MetersWithHourlyLimits>().DeleteRecord(id);
        }

        [RecordOperation(typeof(MetersWithHourlyLimits), RecordOperation.CreateNewRecord)]
        public MetersWithHourlyLimits NewMetersWithHourlyLimits()
        {
            return DataContext.Table<MetersWithHourlyLimits>().NewRecord();
        }

        [AuthorizeHubRole("Administrator, Editor")]
        [RecordOperation(typeof(MetersWithHourlyLimits), RecordOperation.AddNewRecord)]
        public void AddNewMetersWithHourlyLimits(MetersWithHourlyLimits record)
        {
            DataContext.Table<MetersWithHourlyLimits>().AddNewRecord(record);
        }

        [AuthorizeHubRole("Administrator, Editor")]
        [RecordOperation(typeof(MetersWithHourlyLimits), RecordOperation.UpdateRecord)]
        public void UpdateMetersWithHourlyLimits(MetersWithHourlyLimits record)
        {
            DataContext.Table<MetersWithHourlyLimits>().UpdateRecord(record);
        }

        [AuthorizeHubRole("Administrator, Editor")]
        public void AddNewOrUpdateMetersWithHourlyLimits(MetersWithHourlyLimits record)
        {
            DataContext.Table<MetersWithHourlyLimits>().AddNewOrUpdateRecord(record);
        }

        public IEnumerable<Meter> GetMetersForSelect()
        {
            return DataContext.Table<Meter>().QueryRecords("Name", new RecordRestriction("ID IN (SELECT MeterID FROM UserMeter WHERE UserName = {0})", GetCurrentUserSID()));
        }

        public IEnumerable<MeasurementCharacteristic> GetCharacteristicsForSelect()
        {
            return DataContext.Table<MeasurementCharacteristic>().QueryRecords("Name");
        }

        public void ProcessSmartAlarms(IEnumerable<int> meterIds, IEnumerable<int> typeIds, DateTime startDate, DateTime endDate, int sigmaLevel, int decimals, bool ignoreLargeValues, bool overwriteOldAlarms, int largeValueLevel)
        {

            int progressTotal = (meterIds.Any() ? meterIds.Count() : 1);
            int progressCount = 0;
            ProgressUpdatedOverall("", (int)(100 * (progressCount) / progressTotal));

            string historianServer = DataContext.Connection.ExecuteScalar<string>("SELECT Value FROM Setting WHERE Name = 'Historian.Server'") ?? "127.0.0.1";
            string historianInstance = DataContext.Connection.ExecuteScalar<string>("SELECT Value FROM Setting WHERE Name = 'Historian.Instance'") ?? "XDA";

            foreach (int meterId in meterIds)
            {
                string characteristicList = "(" + string.Join(",", typeIds) + ")";
                IEnumerable<int> channelIds = DataContext.Table<Channel>().QueryRecordsWhere("MeterID = {0} AND MeasurementCharacteristicID IN " + characteristicList, meterId).Select(x => x.ID);
                string meterName = DataContext.Connection.ExecuteScalar<string>("Select Name from Meter where ID = {0}", meterId);
                ProgressUpdatedOverall(meterName, (int)(100 * (progressCount) / progressTotal));
                List<TrendingData> trendingData = new List<TrendingData>();
                List<RunningAvgStdDev> runningData = new List<RunningAvgStdDev>();
                ProgressUpdatedByMeter("Querying openHistorian...", 0);
                using (openHistorian.XDALink.Historian historian = new Historian(historianServer, historianInstance))
                {
                    foreach (openHistorian.XDALink.TrendingDataPoint point in historian.Read(channelIds, startDate, endDate))
                    {
                        int hourOfWeek = (int)point.Timestamp.DayOfWeek * 24 + point.Timestamp.Hour;
                        RunningAvgStdDev record = runningData.FirstOrDefault(x => x.ChannelID == point.ChannelID && x.HourOfWeek == hourOfWeek);
                        if (record == null)
                        {
                            record = new RunningAvgStdDev()
                            {
                                ChannelID = point.ChannelID,
                                HourOfWeek = hourOfWeek,
                                Count = 0,
                                Sum = 0,
                                SumOfSquares = 0
                            };
                            runningData.Add(record);
                        }

                        if (point.SeriesID.ToString() == "Average")
                        {
                            record.Sum += point.Value;
                            record.SumOfSquares += (point.Value * point.Value);
                            ++record.Count;

                        }
                    }

                    if (ignoreLargeValues)
                    {
                        runningData = runningData.Select(x =>
                        {
                            double average = x.Sum / (x.Count != 0 ? x.Count : 1);
                            double stdDev = Math.Sqrt(Math.Abs((x.SumOfSquares - 2 * average * x.Sum + x.Count * average * average) / ((x.Count != 1 ? x.Count : 2) - 1)));
                            x.FirstPassHigh = average + stdDev * largeValueLevel;
                            x.FirstPassLow = average - stdDev * largeValueLevel;
                            x.Sum = 0;
                            x.SumOfSquares = 0;
                            x.Count = 0;
                            return x;
                        }).ToList();

                        ProgressUpdatedByMeter("Querying openHistorian for second pass...", 0);
                        foreach (openHistorian.XDALink.TrendingDataPoint point in historian.Read(channelIds, startDate, endDate))
                        {
                            int hourOfWeek = (int)point.Timestamp.DayOfWeek * 24 + point.Timestamp.Hour;
                            RunningAvgStdDev record = runningData.FirstOrDefault(x => x.ChannelID == point.ChannelID && x.HourOfWeek == hourOfWeek);
                            if ((point.SeriesID.ToString() == "Average" && point.Value > record.FirstPassHigh) || (point.SeriesID.ToString() == "Average" && point.Value < record.FirstPassLow)) continue;
                            if (record == null)
                            {
                                record = new RunningAvgStdDev()
                                {
                                    ChannelID = point.ChannelID,
                                    Count = 0,
                                    Sum = 0,
                                    SumOfSquares = 0
                                };
                                runningData.Add(record);
                            }

                            if (point.SeriesID.ToString() == "Average")
                            {
                                record.Sum += point.Value;
                                record.SumOfSquares += (point.Value * point.Value);
                                ++record.Count;

                            }
                        }
                    }
                }


                int innerProgressTotal = (channelIds.Any() ? channelIds.Count() : 1);
                int innerProgressCount = 0;

                foreach (int channelId in channelIds)
                {
                    string channelName = DataContext.Connection.ExecuteScalar<string>("Select Name from Channel where ID = {0}", channelId);
                    ProgressUpdatedByMeter(channelName, (int)(100 * (innerProgressCount) / innerProgressTotal));
                    foreach (RunningAvgStdDev data in runningData.Where(x => x.ChannelID == channelId))
                    {
                        double average = data.Sum / (data.Count != 0 ? data.Count : 1);

                        double stdDev = Math.Sqrt(Math.Abs((data.SumOfSquares - 2 * average * data.Sum + data.Count * average * average) / ((data.Count != 1 ? data.Count : 2) - 1)));
                        float high = (float)Math.Round(average + stdDev * sigmaLevel, decimals);
                        float low = (float)Math.Round(average - stdDev * sigmaLevel, decimals);


                        HourOfWeekLimit hwl = DataContext.Table<HourOfWeekLimit>().QueryRecordWhere("ChannelID = {0} AND HourOfWeek = {1}", data.ChannelID, data.HourOfWeek);

                        if (hwl == null)
                        {
                            HourOfWeekLimit newrecord = new HourOfWeekLimit()
                            {
                                ChannelID = data.ChannelID,
                                AlarmTypeID = 4,
                                HourOfWeek = data.HourOfWeek,
                                Severity = 1,
                                High = high,
                                Low = low,
                                Enabled = 1
                            };
                            DataContext.Table<HourOfWeekLimit>().AddNewRecord(newrecord);
                        }
                        else if (hwl != null && overwriteOldAlarms)
                        {
                            hwl.ChannelID = data.ChannelID;
                            hwl.AlarmTypeID = 4;
                            hwl.HourOfWeek = data.HourOfWeek;
                            hwl.Severity = 1;
                            hwl.High = high;
                            hwl.Low = low;
                            hwl.Enabled = 1;
                            DataContext.Table<HourOfWeekLimit>().UpdateRecord(hwl);
                        }
                    }

                    ProgressUpdatedByMeter(channelName, (int)(100 * (++innerProgressCount) / innerProgressTotal));
                }
                ProgressUpdatedOverall(meterName, (int)(100 * (++progressCount) / progressTotal));
            }
        }

        #endregion

        #region [ ChannelsWithHourlyLimits Table Operations ]

        [RecordOperation(typeof(ChannelsWithHourlyLimits), RecordOperation.QueryRecordCount)]
        public int QueryChannelsWithHourlyLimitsCount(int meterId, string filterText)
        {
            TableOperations<ChannelsWithHourlyLimits> table = DataContext.Table<ChannelsWithHourlyLimits>();
            RecordRestriction restriction = table.GetSearchRestriction(filterText);

            return table.QueryRecordCount(new RecordRestriction("MeterID = {0}", meterId) + restriction);
        }

        [RecordOperation(typeof(ChannelsWithHourlyLimits), RecordOperation.QueryRecords)]
        public IEnumerable<ChannelsWithHourlyLimits> QueryChannelsWithHourlyLimits(int meterId, string sortField, bool ascending, int page, int pageSize, string filterText)
        {
            TableOperations<ChannelsWithHourlyLimits> table = DataContext.Table<ChannelsWithHourlyLimits>();
            RecordRestriction restriction = table.GetSearchRestriction(filterText);

            return table.QueryRecords(sortField, ascending, page, pageSize, new RecordRestriction("MeterID = {0}", meterId) + restriction);
        }

        [AuthorizeHubRole("Administrator, Editor")]
        [RecordOperation(typeof(ChannelsWithHourlyLimits), RecordOperation.DeleteRecord)]
        public void DeleteChannelsWithHourlyLimits(int id)
        {
            DataContext.Table<ChannelsWithHourlyLimits>().DeleteRecord(id);
        }

        [RecordOperation(typeof(ChannelsWithHourlyLimits), RecordOperation.CreateNewRecord)]
        public ChannelsWithHourlyLimits NewChannelsWithHourlyLimits()
        {
            return DataContext.Table<ChannelsWithHourlyLimits>().NewRecord();
        }

        [AuthorizeHubRole("Administrator, Editor")]
        [RecordOperation(typeof(ChannelsWithHourlyLimits), RecordOperation.AddNewRecord)]
        public void AddNewChannelsWithHourlyLimits(ChannelsWithHourlyLimits record)
        {
            DataContext.Table<ChannelsWithHourlyLimits>().AddNewRecord(record);
        }

        [AuthorizeHubRole("Administrator, Editor")]
        [RecordOperation(typeof(ChannelsWithHourlyLimits), RecordOperation.UpdateRecord)]
        public void UpdateChannelsWithHourlyLimits(ChannelsWithHourlyLimits record)
        {
            DataContext.Table<ChannelsWithHourlyLimits>().UpdateRecord(record);
        }

        [AuthorizeHubRole("Administrator, Editor")]
        public void AddNewOrUpdateChannelsWithHourlyLimits(ChannelsWithHourlyLimits record)
        {
            DataContext.Table<ChannelsWithHourlyLimits>().AddNewOrUpdateRecord(record);
        }

        #endregion

        #region [ HourOfWeekLimit Table Operations ]

        [RecordOperation(typeof(HourOfWeekLimit), RecordOperation.QueryRecordCount)]
        public int QueryHourOfWeekLimitCount(int channelId, string filterText)
        {
            TableOperations<HourOfWeekLimit> table = DataContext.Table<HourOfWeekLimit>();
            RecordRestriction restriction = table.GetSearchRestriction(filterText);

            return table.QueryRecordCount(new RecordRestriction("ChannelID = {0}", channelId) + restriction);
        }

        [RecordOperation(typeof(HourOfWeekLimit), RecordOperation.QueryRecords)]
        public IEnumerable<HourOfWeekLimitView> QueryHourOfWeekLimit(int channelId, string sortField, bool ascending, int page, int pageSize, string filterText)
        {
            TableOperations<HourOfWeekLimitView> table = DataContext.Table<HourOfWeekLimitView>();
            RecordRestriction restriction = table.GetSearchRestriction(filterText);

            return table.QueryRecords(sortField, ascending, page, pageSize, new RecordRestriction("ChannelID = {0}", channelId) + restriction);
        }

        [AuthorizeHubRole("Administrator, Editor")]
        [RecordOperation(typeof(HourOfWeekLimit), RecordOperation.DeleteRecord)]
        public void DeleteHourOfWeekLimit(int id)
        {
            DataContext.Table<HourOfWeekLimit>().DeleteRecord(id);
        }

        [RecordOperation(typeof(HourOfWeekLimit), RecordOperation.CreateNewRecord)]
        public HourOfWeekLimit NewHourOfWeekLimit()
        {
            return DataContext.Table<HourOfWeekLimit>().NewRecord();
        }

        [AuthorizeHubRole("Administrator, Editor")]
        [RecordOperation(typeof(HourOfWeekLimit), RecordOperation.AddNewRecord)]
        public void AddNewHourOfWeekLimit(HourOfWeekLimit record)
        {
            DataContext.Table<HourOfWeekLimit>().AddNewRecord(record);
        }

        [AuthorizeHubRole("Administrator, Editor")]
        [RecordOperation(typeof(HourOfWeekLimit), RecordOperation.UpdateRecord)]
        public void UpdateHourOfWeekLimit(HourOfWeekLimit record)
        {
            DataContext.Table<HourOfWeekLimit>().UpdateRecord(record);
        }

        [AuthorizeHubRole("Administrator, Editor")]
        public void AddNewOrUpdateHourOfWeekLimit(HourOfWeekLimit record)
        {
            DataContext.Table<HourOfWeekLimit>().AddNewOrUpdateRecord(record);
        }

        #endregion


        #region [ MetersWithNormalLimits Table Operations ]

        public class RunningAvgStdDev
        {
            public int ChannelID { get; set; }
            public int HourOfWeek { get; set; }
            public double Sum { get; set; }
            public double Count { get; set; }
            public double SumOfSquares { get; set; }
            public double FirstPassHigh { get; set; }
            public double FirstPassLow { get; set; }
        }

        [RecordOperation(typeof(MetersWithNormalLimits), RecordOperation.QueryRecordCount)]
        public int QueryMetersWithNormalLimitsCount(string filterText)
        {

            return DataContext.Table<MetersWithNormalLimits>().QueryRecordCount(filterText);
        }

        [RecordOperation(typeof(MetersWithNormalLimits), RecordOperation.QueryRecords)]
        public IEnumerable<MetersWithNormalLimits> QueryMetersWithNormalLimits(string sortField, bool ascending, int page, int pageSize, string filterText)
        {
            return DataContext.Table<MetersWithNormalLimits>().QueryRecords(sortField, ascending, page, pageSize, filterText);
        }

        [AuthorizeHubRole("Administrator, Editor")]
        [RecordOperation(typeof(MetersWithNormalLimits), RecordOperation.DeleteRecord)]
        public void DeleteMetersWithNormalLimits(int id)
        {
            DataContext.Table<MetersWithNormalLimits>().DeleteRecord(id);
        }

        [RecordOperation(typeof(MetersWithNormalLimits), RecordOperation.CreateNewRecord)]
        public MetersWithNormalLimits NewMetersWithNormalLimits()
        {
            return DataContext.Table<MetersWithNormalLimits>().NewRecord();
        }

        [AuthorizeHubRole("Administrator, Editor")]
        [RecordOperation(typeof(MetersWithNormalLimits), RecordOperation.AddNewRecord)]
        public void AddNewMetersWithNormalLimits(MetersWithNormalLimits record)
        {
            DataContext.Table<MetersWithNormalLimits>().AddNewRecord(record);
        }

        [AuthorizeHubRole("Administrator, Editor")]
        [RecordOperation(typeof(MetersWithNormalLimits), RecordOperation.UpdateRecord)]
        public void UpdateMetersWithNormalLimits(MetersWithNormalLimits record)
        {
            DataContext.Table<MetersWithNormalLimits>().UpdateRecord(record);
        }

        [AuthorizeHubRole("Administrator, Editor")]
        public void AddNewOrUpdateMetersWithNormalLimits(MetersWithNormalLimits record)
        {
            DataContext.Table<MetersWithNormalLimits>().AddNewOrUpdateRecord(record);
        }

        public void ProcessSmartAlarmsNormal(IEnumerable<int> meterIds, IEnumerable<int> typeIds, DateTime startDate, DateTime endDate, int sigmaLevel, int decimals, bool ignoreLargeValues, bool overwriteOldAlarms, int largeValueLevel)
        {

            int progressTotal = (meterIds.Any() ? meterIds.Count() : 1);
            int progressCount = 0;
            ProgressUpdatedOverall("", (int)(100 * (progressCount) / progressTotal));

            string historianServer = DataContext.Connection.ExecuteScalar<string>("SELECT Value FROM Setting WHERE Name = 'Historian.Server'") ?? "127.0.0.1";
            string historianInstance = DataContext.Connection.ExecuteScalar<string>("SELECT Value FROM Setting WHERE Name = 'Historian.Instance'") ?? "XDA";

            foreach (int meterId in meterIds)
            {
                string characteristicList = "(" + string.Join(",", typeIds) + ")";
                IEnumerable<int> channelIds = DataContext.Table<Channel>().QueryRecordsWhere("MeterID = {0} AND MeasurementCharacteristicID IN " + characteristicList, meterId).Select(x => x.ID);
                string meterName = DataContext.Connection.ExecuteScalar<string>("Select Name from Meter where ID = {0}", meterId);
                ProgressUpdatedOverall(meterName, (int)(100 * (progressCount) / progressTotal));
                List<TrendingData> trendingData = new List<TrendingData>();
                List<RunningAvgStdDev> normalRunningData = new List<RunningAvgStdDev>();
                ProgressUpdatedByMeter("Querying openHistorian...", 0);
                using (openHistorian.XDALink.Historian historian = new Historian(historianServer, historianInstance))
                {
                    foreach (openHistorian.XDALink.TrendingDataPoint point in historian.Read(channelIds, startDate, endDate))
                    {
                        RunningAvgStdDev normalRecord = normalRunningData.FirstOrDefault(x => x.ChannelID == point.ChannelID);
                        if (normalRecord == null)
                        {
                            normalRecord = new RunningAvgStdDev()
                            {
                                ChannelID = point.ChannelID,
                                Count = 0,
                                Sum = 0,
                                SumOfSquares = 0
                            };
                            normalRunningData.Add(normalRecord);
                        }

                        if (point.SeriesID.ToString() == "Average")
                        {
                            normalRecord.Sum += point.Value;
                            normalRecord.SumOfSquares += (point.Value * point.Value);
                            ++normalRecord.Count;

                        }
                    }

                    if (ignoreLargeValues)
                    {

                        normalRunningData = normalRunningData.Select(x =>
                        {
                            double average = x.Sum / (x.Count != 0 ? x.Count : 1);

                            double stddev = Math.Sqrt(Math.Abs((x.SumOfSquares - 2 * average * x.Sum + x.Count * average * average) / ((x.Count != 1 ? x.Count : 2) - 1)));
                            x.FirstPassHigh = average + stddev * largeValueLevel;
                            x.FirstPassLow = average - stddev * largeValueLevel; 
                            x.Count = 0;
                            x.Sum = 0;
                            x.SumOfSquares = 0;
                            return x;
                        }).ToList();



                        ProgressUpdatedByMeter("Querying openHistorian for second pass...", 0);
                        foreach (openHistorian.XDALink.TrendingDataPoint point in historian.Read(channelIds, startDate, endDate))
                        {
                            RunningAvgStdDev normalRecord = normalRunningData.FirstOrDefault(x => x.ChannelID == point.ChannelID);
                            if ((point.SeriesID.ToString() == "Average" && point.Value > normalRecord.FirstPassHigh) || (point.SeriesID.ToString() == "Average" && point.Value < normalRecord.FirstPassLow)) continue;
                            if (point.SeriesID.ToString() == "Average")
                            {
                                normalRecord.Sum += point.Value;
                                normalRecord.SumOfSquares += (point.Value * point.Value);
                                ++normalRecord.Count;
                            }
                        }
                    }
                }

                int innerProgressTotal = (channelIds.Any() ? channelIds.Count() : 1);
                int innerProgressCount = 0;

                foreach (int channelId in channelIds)
                {
                    Channel channel = DataContext.Table<Channel>().QueryRecordWhere("ID = {0}", channelId);
                    ProgressUpdatedByMeter(channel.Name, (int)(100 * (innerProgressCount) / innerProgressTotal));
                    RunningAvgStdDev record = normalRunningData.Where(x => x.ChannelID == channelId).FirstOrDefault();
                    if (record != null)
                    {
                        double average = record.Sum / (record.Count != 0 ? record.Count : 1);

                        double stdDev = Math.Sqrt(Math.Abs((record.SumOfSquares - 2 * average * record.Sum + record.Count * average * average) / ((record.Count != 1 ? record.Count : 2) - 1)));
                        float high = (float)Math.Round(average + stdDev * sigmaLevel, decimals);
                        float low = (float)Math.Round(average - stdDev * sigmaLevel, decimals);

                        AlarmRangeLimit hwl = DataContext.Table<AlarmRangeLimit>().QueryRecordWhere("ChannelID = {0}", record.ChannelID);

                        if (hwl == null)
                        {
                            AlarmRangeLimit newRecord = new AlarmRangeLimit()
                            {
                                ChannelID = record.ChannelID,
                                AlarmTypeID = 5,
                                Severity = 1,
                                High = high,
                                Low = low,
                                RangeInclusive = false,
                                PerUnit = false,
                                Enabled = true,
                                IsDefault = false
                            };
                            DataContext.Table<AlarmRangeLimit>().AddNewRecord(newRecord);
                        }
                        else if (hwl != null && overwriteOldAlarms)
                        {
                            hwl.High = high;
                            hwl.Low = low;
                            DataContext.Table<AlarmRangeLimit>().UpdateRecord(hwl);
                        }

                    }

                    ProgressUpdatedByMeter(channel.Name, (int)(100 * (++innerProgressCount) / innerProgressTotal));
                }
                ProgressUpdatedOverall(meterName, (int)(100 * (++progressCount) / progressTotal));
            }
        }

        #endregion

        #region [ ChannelsWithNormalLimits Table Operations ]

        [RecordOperation(typeof(ChannelsWithNormalLimits), RecordOperation.QueryRecordCount)]
        public int QueryChannelsWithNormalLimitsCount(int meterId, string filterText)
        {
            TableOperations<ChannelsWithNormalLimits> table = DataContext.Table<ChannelsWithNormalLimits>();
            RecordRestriction restriction = table.GetSearchRestriction(filterText);

            return table.QueryRecordCount(new RecordRestriction("MeterID = {0}", meterId) + restriction);
        }

        [RecordOperation(typeof(ChannelsWithNormalLimits), RecordOperation.QueryRecords)]
        public IEnumerable<ChannelsWithNormalLimits> QueryChannelsWithNormalLimits(int meterId, string sortField, bool ascending, int page, int pageSize, string filterText)
        {
            TableOperations<ChannelsWithNormalLimits> table = DataContext.Table<ChannelsWithNormalLimits>();
            RecordRestriction restriction = table.GetSearchRestriction(filterText);

            return table.QueryRecords(sortField, ascending, page, pageSize, new RecordRestriction("MeterID = {0}", meterId) + restriction);
        }

        [AuthorizeHubRole("Administrator, Editor")]
        [RecordOperation(typeof(ChannelsWithNormalLimits), RecordOperation.DeleteRecord)]
        public void DeleteChannelsWithNormalLimits(int id)
        {
            DataContext.Table<ChannelsWithNormalLimits>().DeleteRecord(id);
        }

        [RecordOperation(typeof(ChannelsWithNormalLimits), RecordOperation.CreateNewRecord)]
        public ChannelsWithNormalLimits NewChannelsWithNormalLimits()
        {
            return DataContext.Table<ChannelsWithNormalLimits>().NewRecord();
        }

        [AuthorizeHubRole("Administrator, Editor")]
        [RecordOperation(typeof(ChannelsWithNormalLimits), RecordOperation.AddNewRecord)]
        public void AddNewChannelsWithNormalLimits(ChannelsWithNormalLimits record)
        {
            DataContext.Table<ChannelsWithNormalLimits>().AddNewRecord(record);
        }

        [AuthorizeHubRole("Administrator, Editor")]
        [RecordOperation(typeof(ChannelsWithNormalLimits), RecordOperation.UpdateRecord)]
        public void UpdateChannelsWithNormalLimits(ChannelsWithNormalLimits record)
        {

            AlarmRangeLimit alarm = DataContext.Table<AlarmRangeLimit>().QueryRecordWhere("ChannelID = {0}", record.ID);

            alarm.High = record.High;
            alarm.Low = record.Low;
            alarm.RangeInclusive = record.RangeInclusive;
            alarm.PerUnit = record.PerUnit;
            alarm.IsDefault = record.IsDefault;
            alarm.Enabled = record.Enabled;

            DataContext.Table<AlarmRangeLimit>().UpdateRecord(alarm);

        }

        [AuthorizeHubRole("Administrator, Editor")]
        public void AddNewOrUpdateChannelsWithNormalLimits(ChannelsWithNormalLimits record)
        {
            AlarmRangeLimit alarm = DataContext.Table<AlarmRangeLimit>().QueryRecordWhere("ChannelID = {0}", record.ID);

            alarm.High = record.High;
            alarm.Low = record.Low;
            alarm.RangeInclusive = record.RangeInclusive;
            alarm.PerUnit = record.PerUnit;
            alarm.IsDefault = record.IsDefault;
            alarm.Enabled = record.Enabled;

            DataContext.Table<AlarmRangeLimit>().AddNewOrUpdateRecord(alarm);

        }

        public void ResetAlarmToDefault2(int id)
        {
            IEnumerable<DefaultAlarmRangeLimit> defaultLimits = DataContext.Table<DefaultAlarmRangeLimit>().QueryRecords(restriction: new RecordRestriction("MeasurementTypeID = (SELECT MeasurementTypeID FROM Channel WHERE ID = {0}) AND MeasurementCharacteristicID = (SELECT MeasurementCharacteristicID FROM Channel WHERE ID = {0})", id));
            AlarmRangeLimit record = DataContext.Table<AlarmRangeLimit>().QueryRecordWhere("ChannelID = {0}", id);
            if (defaultLimits.Any())
            {
                record.Severity = defaultLimits.First().Severity;
                record.High = defaultLimits.First().High;
                record.Low = defaultLimits.First().Low;
                record.RangeInclusive = defaultLimits.First().RangeInclusive;
                record.PerUnit = defaultLimits.First().PerUnit;
                record.IsDefault = true;

                DataContext.Table<AlarmRangeLimit>().UpdateRecord(record);
            }
        }

        public void UpdateAlarmRangeLimit(int id, double high, double low)
        {
            AlarmRangeLimit record = DataContext.Table<AlarmRangeLimit>().QueryRecordWhere("ChannelID = {0}", id);

            record.High = high;
            record.Low = low;
            record.IsDefault = false;
            DataContext.Table<AlarmRangeLimit>().UpdateRecord(record);
        }





        #endregion

        #region [ DefaultAlarmRangeLimit Table Operations ]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(DefaultAlarmRangeLimitView), RecordOperation.QueryRecordCount)]
        public int QueryDefaultAlarmRangeLimitViewCount(string filterString)
        {
            return DataContext.Table<DefaultAlarmRangeLimitView>().QueryRecordCount(filterString);

        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(DefaultAlarmRangeLimitView), RecordOperation.QueryRecords)]
        public IEnumerable<DefaultAlarmRangeLimitView> QueryDefaultAlarmRangeLimitViews(string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            return DataContext.Table<DefaultAlarmRangeLimitView>().QueryRecords(sortField, ascending, page, pageSize, filterString);
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

        #region [ EmailType Table Operations ]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(EmailType), RecordOperation.QueryRecordCount)]
        public int QueryEmailTypeCount(int emailTypeID, string filterString)
        {
            TableOperations<EmailTypeView> tableOperations = DataContext.Table<EmailTypeView>();
            RecordRestriction restriction;

            if (emailTypeID > 0)
                restriction = tableOperations.GetSearchRestriction(filterString) + new RecordRestriction("ID = {0}", emailTypeID);
            else
                restriction = tableOperations.GetSearchRestriction(filterString);

            return tableOperations.QueryRecordCount(restriction);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(EmailType), RecordOperation.QueryRecords)]
        public IEnumerable<EmailTypeView> QueryEmailType(int emailTypeID, string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            TableOperations<EmailTypeView> tableOperations = DataContext.Table<EmailTypeView>();
            RecordRestriction restriction;

            if (emailTypeID > 0)
                restriction = tableOperations.GetSearchRestriction(filterString) + new RecordRestriction("ID = {0}", emailTypeID);
            else
                restriction = tableOperations.GetSearchRestriction(filterString);

            return tableOperations.QueryRecords(sortField, ascending, page, pageSize, restriction);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(EmailType), RecordOperation.DeleteRecord)]
        public void DeleteEmailType(int id)
        {
            DataContext.Table<EmailType>().DeleteRecord(id);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(EmailType), RecordOperation.CreateNewRecord)]
        public EmailType NewEmailType()
        {
            return new EmailType();
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(EmailType), RecordOperation.AddNewRecord)]
        public void AddNewEmailType(EmailTypeView record)
        {
            DataContext.Table<EmailType>().AddNewRecord(CreateEmailTypeFromView(record));
        }

        [AuthorizeHubRole("Administrator, Owner")]
        [RecordOperation(typeof(EmailType), RecordOperation.UpdateRecord)]
        public void UpdateEmailType(EmailTypeView record)
        {
            DataContext.Table<EmailType>().UpdateRecord(CreateEmailTypeFromView(record));
        }

        private EmailType CreateEmailTypeFromView(EmailTypeView record)
        {
            return new EmailType()
            {
                EmailCategoryID = record.EmailCategoryID,
                ID = record.ID,
                XSLTemplateID = record.XSLTemplateID
            };
        }

        #endregion

        #region [ EmailGroup Table Operations ]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(EmailGroup), RecordOperation.QueryRecordCount)]
        public int QueryEmailGroupCount(int emailGroupID, string filterString)
        {
            TableOperations<EmailGroup> tableOperations = DataContext.Table<EmailGroup>();
            RecordRestriction restriction;

            if (emailGroupID > 0)
                restriction = tableOperations.GetSearchRestriction(filterString) + new RecordRestriction("ID = {0}", emailGroupID);
            else
                restriction = tableOperations.GetSearchRestriction(filterString);

            return tableOperations.QueryRecordCount(restriction);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(EmailGroup), RecordOperation.QueryRecords)]
        public IEnumerable<EmailGroup> QueryEmailGroup(int emailGroupID, string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            TableOperations<EmailGroup> tableOperations = DataContext.Table<EmailGroup>();
            RecordRestriction restriction;

            if (emailGroupID > 0)
                restriction = tableOperations.GetSearchRestriction(filterString) + new RecordRestriction("ID = {0}", emailGroupID);
            else
                restriction = tableOperations.GetSearchRestriction(filterString);

            return tableOperations.QueryRecords(sortField, ascending, page, pageSize, restriction);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(EmailGroup), RecordOperation.DeleteRecord)]
        public void DeleteEmailGroup(int id)
        {
            //DataContext.Table<EmailGroup>().DeleteRecord(id);
            CascadeDelete("EmailGroup", $"ID={id}");
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(EmailGroup), RecordOperation.CreateNewRecord)]
        public EmailGroup NewEmailGroup()
        {
            return new EmailGroup();
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(EmailGroup), RecordOperation.AddNewRecord)]
        public void AddNewEmailGroup(EmailGroup record)
        {
            DataContext.Table<EmailGroup>().AddNewRecord(record);
        }

        [AuthorizeHubRole("Administrator, Owner")]
        [RecordOperation(typeof(EmailGroup), RecordOperation.UpdateRecord)]
        public void UpdateEmailGroup(EmailGroup record)
        {
            DataContext.Table<EmailGroup>().UpdateRecord(record);
        }


        #endregion

        #region [ EmailGroupMeterGroup Table Operations ]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(EmailGroupMeterGroup), RecordOperation.QueryRecordCount)]
        public int QueryEmailGroupMeterGroupCount(int emailGroupId, string filterString)
        {
            TableOperations<EmailGroupMeterGroupView> tableOperations = DataContext.Table<EmailGroupMeterGroupView>();
            RecordRestriction restriction;
            if (emailGroupId > 0)
                restriction = tableOperations.GetSearchRestriction(filterString) + new RecordRestriction("EmailGroupID = {0}", emailGroupId);
            else
                restriction = tableOperations.GetSearchRestriction(filterString);

            return tableOperations.QueryRecordCount(restriction);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(EmailGroupMeterGroup), RecordOperation.QueryRecords)]
        public IEnumerable<EmailGroupMeterGroupView> QueryEmailGroupMeterGroup(int emailGroupId, string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            TableOperations<EmailGroupMeterGroupView> tableOperations = DataContext.Table<EmailGroupMeterGroupView>();
            RecordRestriction restriction;
            if (emailGroupId > 0)
                restriction = tableOperations.GetSearchRestriction(filterString) + new RecordRestriction("EmailGroupID = {0}", emailGroupId);
            else
                restriction = tableOperations.GetSearchRestriction(filterString);

            return tableOperations.QueryRecords(sortField, ascending, page, pageSize, restriction);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(EmailGroupMeterGroup), RecordOperation.DeleteRecord)]
        public void DeleteEmailGroupMeterGroup(int id)
        {
            DataContext.Table<EmailGroupMeterGroup>().DeleteRecord(id);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(EmailGroupMeterGroup), RecordOperation.CreateNewRecord)]
        public EmailGroupMeterGroup NewEmailGroupMeterGroup()
        {
            return new EmailGroupMeterGroup();
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(EmailGroupMeterGroup), RecordOperation.AddNewRecord)]
        public void AddNewEmailGroupMeterGroup(EmailGroupMeterGroup record)
        {
            DataContext.Table<EmailGroupMeterGroup>().AddNewRecord(record);
        }

        [AuthorizeHubRole("Administrator, Owner")]
        [RecordOperation(typeof(EmailGroupMeterGroup), RecordOperation.UpdateRecord)]
        public void UpdateEmailGroupMeterGroup(EmailGroupMeterGroup record)
        {
            DataContext.Table<EmailGroupMeterGroup>().UpdateRecord(record);
        }


        #endregion

        #region [ EmailGroupLineGroup Table Operations ]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(EmailGroupLineGroup), RecordOperation.QueryRecordCount)]
        public int QueryEmailGroupLineGroupCount(int emailGroupId, string filterString)
        {
            TableOperations<EmailGroupLineGroupView> tableOperations = DataContext.Table<EmailGroupLineGroupView>();
            RecordRestriction restriction;
            if (emailGroupId > 0)
                restriction = tableOperations.GetSearchRestriction(filterString) + new RecordRestriction("EmailGroupID = {0}", emailGroupId);
            else
                restriction = tableOperations.GetSearchRestriction(filterString);

            return tableOperations.QueryRecordCount(restriction);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(EmailGroupLineGroup), RecordOperation.QueryRecords)]
        public IEnumerable<EmailGroupLineGroupView> QueryEmailGroupLineGroup(int emailGroupId, string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            TableOperations<EmailGroupLineGroupView> tableOperations = DataContext.Table<EmailGroupLineGroupView>();
            RecordRestriction restriction;
            if (emailGroupId > 0)
                restriction = tableOperations.GetSearchRestriction(filterString) + new RecordRestriction("EmailGroupID = {0}", emailGroupId);
            else
                restriction = tableOperations.GetSearchRestriction(filterString);

            return tableOperations.QueryRecords(sortField, ascending, page, pageSize, restriction);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(EmailGroupLineGroup), RecordOperation.DeleteRecord)]
        public void DeleteEmailGroupLineGroup(int id)
        {
            DataContext.Table<EmailGroupLineGroup>().DeleteRecord(id);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(EmailGroupLineGroup), RecordOperation.CreateNewRecord)]
        public EmailGroupLineGroup NewEmailGroupLineGroup()
        {
            return new EmailGroupLineGroup();
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(EmailGroupLineGroup), RecordOperation.AddNewRecord)]
        public void AddNewEmailGroupLineGroup(EmailGroupLineGroup record)
        {
            DataContext.Table<EmailGroupLineGroup>().AddNewRecord(record);
        }

        [AuthorizeHubRole("Administrator, Owner")]
        [RecordOperation(typeof(EmailGroupLineGroup), RecordOperation.UpdateRecord)]
        public void UpdateEmailGroupLineGroup(EmailGroupLineGroup record)
        {
            DataContext.Table<EmailGroupLineGroup>().UpdateRecord(record);
        }


        #endregion

        #region [ EmailGroupUserAccount Table Operations ]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(EmailGroupUserAccount), RecordOperation.QueryRecordCount)]
        public int QueryEmailGroupUserAccountCount(int emailGroupId, string filterString)
        {
            TableOperations<EmailGroupUserAccountView> tableOperations = DataContext.Table<EmailGroupUserAccountView>();
            RecordRestriction restriction;
            if (emailGroupId > 0)
                restriction = tableOperations.GetSearchRestriction(filterString) + new RecordRestriction("EmailGroupID = {0}", emailGroupId);
            else
                restriction = tableOperations.GetSearchRestriction(filterString);

            return tableOperations.QueryRecordCount(restriction);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(EmailGroupUserAccount), RecordOperation.QueryRecords)]
        public IEnumerable<EmailGroupUserAccountView> QueryEmailGroupUserAccount(int emailGroupId, string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            TableOperations<EmailGroupUserAccountView> tableOperations = DataContext.Table<EmailGroupUserAccountView>();
            RecordRestriction restriction;
            if (emailGroupId > 0)
                restriction = tableOperations.GetSearchRestriction(filterString) + new RecordRestriction("EmailGroupID = {0}", emailGroupId);
            else
                restriction = tableOperations.GetSearchRestriction(filterString);

            return tableOperations.QueryRecords(sortField, ascending, page, pageSize, restriction);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(EmailGroupUserAccount), RecordOperation.DeleteRecord)]
        public void DeleteEmailGroupUserAccount(int id)
        {
            DataContext.Table<EmailGroupUserAccount>().DeleteRecord(id);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(EmailGroupUserAccount), RecordOperation.CreateNewRecord)]
        public EmailGroupUserAccount NewEmailGroupUserAccount()
        {
            return new EmailGroupUserAccount();
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(EmailGroupUserAccount), RecordOperation.AddNewRecord)]
        public void AddNewEmailGroupUserAccount(EmailGroupUserAccount record)
        {
            DataContext.Table<EmailGroupUserAccount>().AddNewRecord(record);
        }

        [AuthorizeHubRole("Administrator, Owner")]
        [RecordOperation(typeof(EmailGroupUserAccount), RecordOperation.UpdateRecord)]
        public void UpdateEmailGroupUserAccount(EmailGroupUserAccount record)
        {
            DataContext.Table<EmailGroupUserAccount>().UpdateRecord(record);
        }


        #endregion

        #region [ EmailGroupType Table Operations ]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(EmailGroupType), RecordOperation.QueryRecordCount)]
        public int QueryEmailGroupTypeCount(int emailGroupID, string filterString)
        {
            TableOperations<EmailGroupTypeView> tableOperations = DataContext.Table<EmailGroupTypeView>();
            RecordRestriction restriction;
            if (emailGroupID > 0)
                restriction = tableOperations.GetSearchRestriction(filterString) + new RecordRestriction("EmailGroupID = {0}", emailGroupID);
            else
                restriction = tableOperations.GetSearchRestriction(filterString);

            return tableOperations.QueryRecordCount(restriction);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(EmailGroupType), RecordOperation.QueryRecords)]
        public IEnumerable<EmailGroupType> QueryEmailGroupType(int emailGroupID, string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            TableOperations<EmailGroupTypeView> tableOperations = DataContext.Table<EmailGroupTypeView>();
            RecordRestriction restriction;
            if (emailGroupID > 0)
                restriction = tableOperations.GetSearchRestriction(filterString) + new RecordRestriction("EmailGroupID = {0}", emailGroupID);
            else
                restriction = tableOperations.GetSearchRestriction(filterString);

            return tableOperations.QueryRecords(sortField, ascending, page, pageSize, restriction);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(EmailGroupType), RecordOperation.DeleteRecord)]
        public void DeleteEmailGroupType(int id)
        {
            DataContext.Table<EmailGroupType>().DeleteRecord(id);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(EmailGroupType), RecordOperation.CreateNewRecord)]
        public EmailGroupType NewEmailGroupType()
        {
            return new EmailGroupType();
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(EmailGroupType), RecordOperation.AddNewRecord)]
        public void AddNewEmailGroupType(EmailGroupTypeView record)
        {
            DataContext.Table<EmailGroupType>().AddNewRecord(CreateEmailGroupTypeFromView(record));
        }

        [AuthorizeHubRole("Administrator, Owner")]
        [RecordOperation(typeof(EmailGroupType), RecordOperation.UpdateRecord)]
        public void UpdateEmailGroupType(EmailGroupTypeView record)
        {
            DataContext.Table<EmailGroupType>().UpdateRecord(CreateEmailGroupTypeFromView(record));
        }

        private EmailGroupType CreateEmailGroupTypeFromView(EmailGroupTypeView record)
        {
            return new EmailGroupType()
            {
                EmailGroupID = record.EmailGroupID,
                ID = record.ID,
                EmailTypeID = record.EmailTypeID
            };
        }


        #endregion

        #region [ XSLTemplate Table Operations ]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(XSLTemplate), RecordOperation.QueryRecordCount)]
        public int QueryXSLTemplateCount(string filterString)
        {
            return DataContext.Table<XSLTemplate>().QueryRecordCount(filterString);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(XSLTemplate), RecordOperation.QueryRecords)]
        public IEnumerable<XSLTemplate> QueryXSLTemplate(string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            return DataContext.Table<XSLTemplate>().QueryRecords(sortField, ascending, page, pageSize, filterString);
        }

        public XSLTemplate QueryXSLTemplateByID(int id)
        {
            return DataContext.Table<XSLTemplate>().QueryRecords(restriction: new RecordRestriction("ID = {0}", id)).First();
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(XSLTemplate), RecordOperation.DeleteRecord)]
        public void DeleteXSLTemplate(int id)
        {
            DataContext.Table<XSLTemplate>().DeleteRecord(id);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(XSLTemplate), RecordOperation.CreateNewRecord)]
        public XSLTemplate NewXSLTemplate()
        {
            return new XSLTemplate();
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(XSLTemplate), RecordOperation.AddNewRecord)]
        public void AddNewXSLTemplate(XSLTemplate record)
        {
            DataContext.Table<XSLTemplate>().AddNewRecord(record);
        }

        [AuthorizeHubRole("Administrator, Owner")]
        [RecordOperation(typeof(XSLTemplate), RecordOperation.UpdateRecord)]
        public void UpdateXSLTemplate(XSLTemplate record)
        {
            DataContext.Table<XSLTemplate>().UpdateRecord(record);
        }


        #endregion

        #region [Event Criterion]

        public class EventCriterion
        {
            public bool five;
            public bool four;
            public bool three;
            public bool two;
            public bool one;
            public bool zero;
            public bool disturbances;
            public bool fault;
        }

        public EventCriterion GetEventCriterion(int EmailGroupID)
        {
            EventCriterion ec = new EventCriterion();

            ec.five = DataContext.Connection.ExecuteScalar("SELECT ID FROM DisturbanceEmailCriterion WHERE EmailGroupID = {0} AND SeverityCode = 5", EmailGroupID) != null;
            ec.four = DataContext.Connection.ExecuteScalar("SELECT ID FROM DisturbanceEmailCriterion WHERE EmailGroupID = {0} AND SeverityCode = 4", EmailGroupID) != null;
            ec.three = DataContext.Connection.ExecuteScalar("SELECT ID FROM DisturbanceEmailCriterion WHERE EmailGroupID = {0} AND SeverityCode = 3", EmailGroupID) != null;
            ec.two = DataContext.Connection.ExecuteScalar("SELECT ID FROM DisturbanceEmailCriterion WHERE EmailGroupID = {0} AND SeverityCode = 2", EmailGroupID) != null;
            ec.one = DataContext.Connection.ExecuteScalar("SELECT ID FROM DisturbanceEmailCriterion WHERE EmailGroupID = {0} AND SeverityCode = 1", EmailGroupID) != null;
            ec.zero = DataContext.Connection.ExecuteScalar("SELECT ID FROM DisturbanceEmailCriterion WHERE EmailGroupID = {0} AND SeverityCode = 0", EmailGroupID) != null;
            ec.fault = DataContext.Connection.ExecuteScalar("SELECT ID FROM FaultEmailCriterion WHERE EmailGroupID = {0}", EmailGroupID) != null;
            ec.disturbances = ec.five || ec.four || ec.three || ec.two || ec.one || ec.zero;


            return ec;
        }

        public void UpdateEventCriterion(EventCriterion record, int EmailGroupID)
        {
            if (record.fault && DataContext.Connection.ExecuteScalar("SELECT ID FROM FaultEmailCriterion WHERE EmailGroupID = {0}", EmailGroupID) == null)
                DataContext.Table<FaultEmailCriterion>().AddNewRecord(new FaultEmailCriterion() { EmailGroupID = EmailGroupID });
            else if(!record.fault && DataContext.Connection.ExecuteScalar("SELECT ID FROM FaultEmailCriterion WHERE EmailGroupID = {0}", EmailGroupID) != null)
            {
                int index = DataContext.Connection.ExecuteScalar<int>("SELECT ID FROM FaultEmailCriterion WHERE EmailGroupID ={0}", EmailGroupID);
                DataContext.Table<FaultEmailCriterion>().DeleteRecord(index);
            }

            if (record.five && DataContext.Connection.ExecuteScalar("SELECT ID FROM DisturbanceEmailCriterion WHERE EmailGroupID = {0} AND SeverityCode = 5", EmailGroupID) == null)
                DataContext.Table<DisturbanceEmailCriterion>().AddNewRecord(new DisturbanceEmailCriterion() { EmailGroupID = EmailGroupID, SeverityCode = 5});
            else if (!record.five && DataContext.Connection.ExecuteScalar("SELECT ID FROM DisturbanceEmailCriterion WHERE EmailGroupID = {0} AND SeverityCode = 5", EmailGroupID) != null)
            {
                int index = DataContext.Connection.ExecuteScalar<int>("SELECT ID FROM DisturbanceEmailCriterion WHERE EmailGroupID = {0} AND SeverityCode = 5", EmailGroupID);
                DataContext.Table<DisturbanceEmailCriterion>().DeleteRecord(index);
            }

            if (record.four && DataContext.Connection.ExecuteScalar("SELECT ID FROM DisturbanceEmailCriterion WHERE EmailGroupID = {0} AND SeverityCode = 4", EmailGroupID) == null)
                DataContext.Table<DisturbanceEmailCriterion>().AddNewRecord(new DisturbanceEmailCriterion() { EmailGroupID = EmailGroupID, SeverityCode = 4 });
            else if (!record.five && DataContext.Connection.ExecuteScalar("SELECT ID FROM DisturbanceEmailCriterion WHERE EmailGroupID = {0} AND SeverityCode = 4", EmailGroupID) != null)
            {
                int index = DataContext.Connection.ExecuteScalar<int>("SELECT ID FROM DisturbanceEmailCriterion WHERE EmailGroupID = {0} AND SeverityCode = 4", EmailGroupID);
                DataContext.Table<DisturbanceEmailCriterion>().DeleteRecord(index);
            }

            if (record.three && DataContext.Connection.ExecuteScalar("SELECT ID FROM DisturbanceEmailCriterion WHERE EmailGroupID = {0} AND SeverityCode = 3", EmailGroupID) == null)
                DataContext.Table<DisturbanceEmailCriterion>().AddNewRecord(new DisturbanceEmailCriterion() { EmailGroupID = EmailGroupID, SeverityCode = 3 });
            else if (!record.five && DataContext.Connection.ExecuteScalar("SELECT ID FROM DisturbanceEmailCriterion WHERE EmailGroupID = {0} AND SeverityCode = 3", EmailGroupID) != null)
            {
                int index = DataContext.Connection.ExecuteScalar<int>("SELECT ID FROM DisturbanceEmailCriterion WHERE EmailGroupID = {0} AND SeverityCode = 3", EmailGroupID);
                DataContext.Table<DisturbanceEmailCriterion>().DeleteRecord(index);
            }

            if (record.two && DataContext.Connection.ExecuteScalar("SELECT ID FROM DisturbanceEmailCriterion WHERE EmailGroupID = {0} AND SeverityCode = 2", EmailGroupID) == null)
                DataContext.Table<DisturbanceEmailCriterion>().AddNewRecord(new DisturbanceEmailCriterion() { EmailGroupID = EmailGroupID, SeverityCode = 2 });
            else if (!record.five && DataContext.Connection.ExecuteScalar("SELECT ID FROM DisturbanceEmailCriterion WHERE EmailGroupID = {0} AND SeverityCode = 2", EmailGroupID) != null)
            {
                int index = DataContext.Connection.ExecuteScalar<int>("SELECT ID FROM DisturbanceEmailCriterion WHERE EmailGroupID = {0} AND SeverityCode = 2", EmailGroupID);
                DataContext.Table<DisturbanceEmailCriterion>().DeleteRecord(index);
            }

            if (record.one && DataContext.Connection.ExecuteScalar("SELECT ID FROM DisturbanceEmailCriterion WHERE EmailGroupID = {0} AND SeverityCode = 1", EmailGroupID) == null)
                DataContext.Table<DisturbanceEmailCriterion>().AddNewRecord(new DisturbanceEmailCriterion() { EmailGroupID = EmailGroupID, SeverityCode = 1 });
            else if (!record.five && DataContext.Connection.ExecuteScalar("SELECT ID FROM DisturbanceEmailCriterion WHERE EmailGroupID = {0} AND SeverityCode = 1", EmailGroupID) != null)
            {
                int index = DataContext.Connection.ExecuteScalar<int>("SELECT ID FROM DisturbanceEmailCriterion WHERE EmailGroupID = {0} AND SeverityCode = 1", EmailGroupID);
                DataContext.Table<DisturbanceEmailCriterion>().DeleteRecord(index);
            }

            if (record.zero && DataContext.Connection.ExecuteScalar("SELECT ID FROM DisturbanceEmailCriterion WHERE EmailGroupID = {0} AND SeverityCode = 0", EmailGroupID) == null)
                DataContext.Table<DisturbanceEmailCriterion>().AddNewRecord(new DisturbanceEmailCriterion() { EmailGroupID = EmailGroupID, SeverityCode = 0 });
            else if (!record.five && DataContext.Connection.ExecuteScalar("SELECT ID FROM DisturbanceEmailCriterion WHERE EmailGroupID = {0} AND SeverityCode = 0", EmailGroupID) != null)
            {
                int index = DataContext.Connection.ExecuteScalar<int>("SELECT ID FROM DisturbanceEmailCriterion WHERE EmailGroupID = {0} AND SeverityCode = 0", EmailGroupID);
                DataContext.Table<DisturbanceEmailCriterion>().DeleteRecord(index);
            }

        }

        #endregion

        #region [ Stored Procedure Operations ]

        public List<TrendingData> GetTrendingData(DateTime startDate, DateTime endDate, int channelId)
        {
            List<TrendingData> trendingData = new List<TrendingData>();

            string historianServer = DataContext.Connection.ExecuteScalar<string>("SELECT Value FROM Setting WHERE Name = 'Historian.Server'") ?? "127.0.0.1";
            string historianInstance = DataContext.Connection.ExecuteScalar<string>("SELECT Value FROM Setting WHERE Name = 'Historian.Instance'") ?? "XDA";
            IEnumerable<int> channelIDs = new List<int> { channelId };

            
            using (Historian historian = new Historian(historianServer, historianInstance))
            {
                foreach (openHistorian.XDALink.TrendingDataPoint point in historian.Read(channelIDs, startDate, endDate))
                {
                    TrendingData obj = new TrendingData();

                    obj.ChannelID = point.ChannelID;
                    obj.SeriesType = point.SeriesID.ToString();
                    obj.Time = point.Timestamp;
                    obj.Value = point.Value;
                    trendingData.Add(obj);
                }
            }


            return trendingData;
           
        }

        private void CascadeDelete(string tableName, string criterion)
        {

            using (IDbCommand sc = DataContext.Connection.Connection.CreateCommand())
            {

                sc.CommandText = "DECLARE @context VARBINARY(128)\n SELECT @context = CONVERT(VARBINARY(128), CONVERT(VARCHAR(128), @userName))\n SET CONTEXT_INFO @context";
                IDbDataParameter param = sc.CreateParameter();
                param.ParameterName = "@userName";
                param.Value = GetCurrentUserName();
                sc.Parameters.Add(param);
                sc.ExecuteNonQuery();
                sc.Parameters.Clear();


                sc.CommandText = "dbo.UniversalCascadeDelete";
                sc.CommandType = CommandType.StoredProcedure;
                IDbDataParameter param1 = sc.CreateParameter();
                param1.ParameterName = "@tableName";
                param1.Value = tableName;
                IDbDataParameter param2 = sc.CreateParameter();
                param2.ParameterName = "@baseCriteria";
                param2.Value = criterion;
                sc.Parameters.Add(param1);
                sc.Parameters.Add(param2);
                sc.ExecuteNonQuery();
            }
        }

        #endregion

        #endregion

        #region [Assets Operations]

        #region [ Meter Table Operations ]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Meter), RecordOperation.QueryRecordCount)]
        public int QueryMeterCount(int meterLocationID, string filterString)
        {
            TableOperations<MeterDetail> tableOperations = DataContext.Table<MeterDetail>();
            RecordRestriction restriction;
            if (meterLocationID > 0)
                restriction = tableOperations.GetSearchRestriction(filterString) + new RecordRestriction("MeterLocationID = {0}", meterLocationID);
            else
                restriction = tableOperations.GetSearchRestriction(filterString);

            return tableOperations.QueryRecordCount(restriction);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Meter), RecordOperation.QueryRecords)]
        public IEnumerable<MeterDetail> QueryMeters(int meterLocationID, string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            TableOperations<MeterDetail> tableOperations = DataContext.Table<MeterDetail>();
            RecordRestriction restriction;
            if (meterLocationID > 0)
                restriction = tableOperations.GetSearchRestriction(filterString) + new RecordRestriction("MeterLocationID = {0}", meterLocationID);
            else
                restriction = tableOperations.GetSearchRestriction(filterString);

            return tableOperations.QueryRecords(sortField, ascending, page, pageSize, restriction);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Meter), RecordOperation.DeleteRecord)]
        public void DeleteMeter(int id)
        {
            CascadeDelete("Meter", $"ID = {id}");

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
            return DataContext.Table<MeterLocation>().QueryRecordCount(filterString);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(MeterLocation), RecordOperation.QueryRecords)]
        public IEnumerable<MeterLocation> QueryMeterLocations(string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            return DataContext.Table<MeterLocation>().QueryRecords(sortField, ascending, page, pageSize, filterString);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(MeterLocation), RecordOperation.DeleteRecord)]
        public void DeleteMeterLocation(int id)
        {
            CascadeDelete("MeterLocation", $"ID = {id}");
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
            return DataContext.Table<Line>().QueryRecordCount(filterString);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Line), RecordOperation.QueryRecords)]
        public IEnumerable<Line> QueryLines(string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            return DataContext.Table<Line>().QueryRecords(sortField, ascending, page, pageSize, filterString);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Line), RecordOperation.DeleteRecord)]
        public void DeleteLines(int id)
        {
            //DataContext.Table<Line>().DeleteRecord(id);
            CascadeDelete("Line", $"ID = {id}");

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

        [AuthorizeHubRole("Administrator")]
        public IEnumerable<IDLabel> SearchLinesByGroup(int groupID, string searchText, int limit = -1)
        {
            RecordRestriction restriction = new RecordRestriction("AssetKey LIKE {0} AND ID NOT IN (SELECT LineID FROM LineLineGroup WHERE LineGroupID = {1})", $"%{searchText}%", groupID);

            return DataContext.Table<Meter>().QueryRecords("AssetKey", restriction, limit)
                .Select(line => new IDLabel(line.ID.ToString(), line.AssetKey));
        }


        #endregion

        #region [ LineView Table Operations ]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(LineView), RecordOperation.QueryRecordCount)]
        public int QueryLineViewCount(string filterString)
        {
            return DataContext.Table<LineView>().QueryRecordCount(filterString);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(LineView), RecordOperation.QueryRecords)]
        public IEnumerable<LineView> QueryLineView(string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            return DataContext.Table<LineView>().QueryRecords(sortField, ascending, page, pageSize, filterString);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(LineView), RecordOperation.DeleteRecord)]
        public void DeleteLineView(int id)
        {
            CascadeDelete("Line", $"ID = {id}");

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
            int index = DataContext.Connection.ExecuteScalar<int?>("SELECT IDENT_CURRENT('Line')") ?? 0;
            record.ID = index;
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
            line.ID = record.ID;
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
            li.ID = record.LineImpedanceID;
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
            TableOperations<MeterLineDetail> tableOperations = DataContext.Table<MeterLineDetail>();
            RecordRestriction restriction;

            if (lineID == -1 && meterID != -1)
                restriction = tableOperations.GetSearchRestriction(filterString) + new RecordRestriction("MeterID = {0}", meterID);
            else if (meterID == -1 && lineID != -1)
                restriction = tableOperations.GetSearchRestriction(filterString) + new RecordRestriction("LineID = {0}", lineID);
            else
                restriction = tableOperations.GetSearchRestriction(filterString);
            return tableOperations.QueryRecordCount(restriction);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(MeterLine), RecordOperation.QueryRecords)]
        public IEnumerable<MeterLineDetail> QueryMeterLine(int lineID, int meterID, string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            TableOperations<MeterLineDetail> tableOperations = DataContext.Table<MeterLineDetail>();
            RecordRestriction restriction;

            if (lineID == -1 && meterID != -1)
                restriction = tableOperations.GetSearchRestriction(filterString) + new RecordRestriction("MeterID = {0}", meterID);
            else if (meterID == -1 && lineID != -1)
                restriction = tableOperations.GetSearchRestriction(filterString) + new RecordRestriction("LineID = {0}", lineID);
            else
                restriction = tableOperations.GetSearchRestriction(filterString);
            return tableOperations.QueryRecords(sortField, ascending, page, pageSize, restriction);
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
            TableOperations<ChannelDetail> tableOperations = DataContext.Table<ChannelDetail>();
            RecordRestriction restriction;

            if (meterID != -1 && lineID == -1)
                restriction = tableOperations.GetSearchRestriction(filterString) + new RecordRestriction("MeterID = {0} ", meterID);
            else if (meterID == -1 && lineID != -1)
                restriction = tableOperations.GetSearchRestriction(filterString) + new RecordRestriction("LineID = {0}", lineID);
            else
                restriction = tableOperations.GetSearchRestriction(filterString) + new RecordRestriction("MeterID = {0} AND LineID = {1}", meterID, lineID);

            return tableOperations.QueryRecordCount(restriction);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Channel), RecordOperation.QueryRecords)]
        public IEnumerable<ChannelDetail> QueryChannel(int meterID, int lineID, string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            TableOperations<ChannelDetail> tableOperations = DataContext.Table<ChannelDetail>();
            RecordRestriction restriction;

            if (meterID != -1 && lineID == -1)
                restriction = tableOperations.GetSearchRestriction(filterString) + new RecordRestriction("MeterID = {0} ", meterID);
            else if (meterID == -1 && lineID != -1)
                restriction = tableOperations.GetSearchRestriction(filterString) + new RecordRestriction("LineID = {0}", lineID);
            else
                restriction = tableOperations.GetSearchRestriction(filterString) + new RecordRestriction("MeterID = {0} AND LineID = {1}", meterID, lineID);

            return tableOperations.QueryRecords(sortField, ascending, page, pageSize, restriction);
        }

        public IEnumerable<Channel> QueryChannelsForDropDown(string filterString, int meterID)
        {
            return DataContext.Table<Channel>().QueryRecords(restriction: new RecordRestriction("Name LIKE {0} AND MeterID = {1}", filterString, meterID), limit: 50);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Channel), RecordOperation.DeleteRecord)]
        public void DeleteChannel(int id)
        {
            CascadeDelete("Channel", $"ID = {id}");
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
                DataContext.Connection.ExecuteNonQuery("INSERT INTO Series VALUES({0}, {1}, {2})", record.ID, record.SeriesTypeID, record.Mapping);
            }
        }

        [AuthorizeHubRole("Administrator, Owner")]
        [RecordOperation(typeof(Channel), RecordOperation.UpdateRecord)]
        public void UpdateChannel(ChannelDetail record)
        {
            DataContext.Table<Channel>().UpdateRecord(record);

            if (!string.IsNullOrEmpty(record.Mapping))
            {
                bool seriesExists = DataContext.Connection.ExecuteScalar<int>("SELECT COUNT(*) FROM Series WHERE ChannelID = {0} AND SeriesTypeID = {1}", record.ID, record.SeriesTypeID) > 0;

                if (seriesExists)
                    DataContext.Connection.ExecuteNonQuery("UPDATE Series SET SourceIndexes = {0} WHERE ChannelID = {1} AND SeriesTypeID = {2}", record.Mapping, record.ID, record.SeriesTypeID);
                else
                    DataContext.Connection.ExecuteNonQuery("INSERT INTO Series VALUES({0}, {1}, {2})", record.ID, record.SeriesTypeID, record.Mapping);
            }
            else
            {
                DataContext.Connection.ExecuteNonQuery("UPDATE Series SET SourceIndexes = '' WHERE ChannelID = {0} AND SeriesTypeID = {1}", record.ID, record.SeriesTypeID);
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


        #endregion

        #region [Workbench Page]

        #region [Filters Operations]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(WorkbenchFilter), RecordOperation.QueryRecordCount)]
        public int QueryWorkbenchFilterCount(string filterString)
        {
            return DataContext.Table<WorkbenchFilter>().QueryRecordCount(new RecordRestriction("UserID = {0}", GetCurrentUserID()));
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(WorkbenchFilter), RecordOperation.QueryRecords)]
        public IEnumerable<WorkbenchFilter> QueryWorkbenchFilters(string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            return DataContext.Table<WorkbenchFilter>().QueryRecords(sortField, ascending, page, pageSize, new RecordRestriction("UserID = {0}", GetCurrentUserID()));
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(WorkbenchFilter), RecordOperation.DeleteRecord)]
        public void DeleteWorkbenchFilter(int id)
        {
            WorkbenchFilter record = DataContext.Table<WorkbenchFilter>().QueryRecords(restriction: new RecordRestriction("ID = {0}", id)).First();

            DataContext.Table<WorkbenchFilter>().DeleteRecord(id);
            if (record.IsDefault)
            {
                IEnumerable<WorkbenchFilter> wbfs = DataContext.Table<WorkbenchFilter>().QueryRecords(restriction: new RecordRestriction("UserID = {0}", GetCurrentUserID()));
                if (wbfs.Any())
                {
                    WorkbenchFilter wbf = wbfs.First();
                    wbf.IsDefault = !wbf.IsDefault;
                    DataContext.Table<WorkbenchFilter>().UpdateRecord(wbf);

                }
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(WorkbenchFilter), RecordOperation.CreateNewRecord)]
        public WorkbenchFilter NewWorkbenchFilter()
        {
            return new WorkbenchFilter();
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(WorkbenchFilter), RecordOperation.AddNewRecord)]
        public void AddNewWorkbenchFilter(WorkbenchFilter record)
        {
            record.UserID = GetCurrentUserID();
            IEnumerable<WorkbenchFilter> wbfs = DataContext.Table<WorkbenchFilter>().QueryRecords(restriction: new RecordRestriction("UserID = {0}", GetCurrentUserID()));
            if (!wbfs.Any())
            {
                record.IsDefault = true;
                DataContext.Table<WorkbenchFilter>().AddNewRecord(record);
                return;
            }

            if (record.IsDefault)
            {
                 
                
                foreach (WorkbenchFilter wbf in wbfs)
                {
                    if (wbf.IsDefault)
                    {
                        wbf.IsDefault = !wbf.IsDefault;
                        DataContext.Table<WorkbenchFilter>().UpdateRecord(wbf);
                    }
                }
            }

            DataContext.Table<WorkbenchFilter>().AddNewRecord(record);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(WorkbenchFilter), RecordOperation.UpdateRecord)]
        public void UpdateWorkbenchFilter(WorkbenchFilter record)
        {
            if (record.IsDefault)
            {
                IEnumerable<WorkbenchFilter> wbfs = DataContext.Table<WorkbenchFilter>().QueryRecords(restriction: new RecordRestriction("UserID = {0}", GetCurrentUserID()));

                foreach (WorkbenchFilter wbf in wbfs)
                {
                    if (wbf.IsDefault)
                    {
                        wbf.IsDefault = !wbf.IsDefault;
                        DataContext.Table<WorkbenchFilter>().UpdateRecord(wbf);
                    }
                }
            }

            DataContext.Table<WorkbenchFilter>().UpdateRecord(record);
        }

        public IEnumerable<WorkbenchFilter> GetWorkbenchFiltersForSelect()
        {
            return DataContext.Table<WorkbenchFilter>().QueryRecords(restriction: new RecordRestriction("UserID = {0}", GetCurrentUserID()));
        } 


        public IEnumerable<EventType> GetEventTypesForSelect()
        {
            return DataContext.Table<EventType>().QueryRecords();
        }

        public IEnumerable<Line> GetLinesForSelect()
        {
            return DataContext.Table<Line>().QueryRecords(restriction: new RecordRestriction( "ID IN (SELECT LineID FROM MeterLine WHERE MeterID IN (SELECT MeterID FROM UserMeter WHERE UserName = {0}))", GetCurrentUserSID()));
        }


        public DateTime GetOldestEventDateTime()
        {
            return DataContext.Connection.ExecuteScalar<DateTime>("SELECT TOP 1 StartTime FROM [Event] ORDER BY StartTime ASC");
        }

        #endregion

        #region [Events Operations]
        public int GetEventCounts(int filterId, string filterString)
        {
            Tuple<DateTime, DateTime> dTuple = GetTimeRange(filterId);
            DateTime startDate =  dTuple.Item1;
            DateTime endDate = dTuple.Item2;

            TableOperations<EventView> tableOperations = DataContext.Table<EventView>();
            RecordRestriction restriction = tableOperations.GetSearchRestriction(filterString) + 
                new RecordRestriction("MeterID IN (Select * FROM String_To_Int_Table((Select Meters FROM WorkbenchFilter WHERE ID = {0}), ',')) AND EventTypeID IN (Select * FROM String_To_Int_Table((Select EventTypes FROM WorkbenchFilter WHERE ID = {0}), ',')) AND StartTime >= {1} AND StartTime <= {2} ", filterId, startDate, endDate);
            return tableOperations.QueryRecordCount(restriction);
        }


        public IEnumerable<EventView> GetFilteredEvents(int filterId, string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            Tuple<DateTime, DateTime> dTuple = GetTimeRange(filterId);
            DateTime startDate = dTuple.Item1;
            DateTime endDate = dTuple.Item2;
            TableOperations<EventView> tableOperations = DataContext.Table<EventView>();
            RecordRestriction restriction = tableOperations.GetSearchRestriction(filterString) +
                new RecordRestriction("MeterID IN (Select * FROM String_To_Int_Table((Select Meters FROM WorkbenchFilter WHERE ID = {0}), ',')) AND EventTypeID IN (Select * FROM String_To_Int_Table((Select EventTypes FROM WorkbenchFilter WHERE ID = {0}), ',')) AND StartTime >= {1} AND StartTime <= {2} ", filterId, startDate, endDate);

            return tableOperations.QueryRecords(sortField, ascending, page, pageSize, restriction);
        }

        [AuthorizeHubRole("*")]
        [RecordOperation(typeof(Event), RecordOperation.QueryRecordCount)]
        public int QueryEventCount(int filterId, string filterString)
        {
            Tuple<DateTime, DateTime> dTuple = GetTimeRange(filterId);
            DateTime startDate = dTuple.Item1;
            DateTime endDate = dTuple.Item2;

            TableOperations<EventView> tableOperations = DataContext.Table<EventView>();
            RecordRestriction restriction = tableOperations.GetSearchRestriction(filterString) + 
                new RecordRestriction("(MeterID IN (Select * FROM String_To_Int_Table((Select Meters FROM WorkbenchFilter WHERE ID = {0}), ',')) OR LineID IN (Select * FROM String_To_Int_Table((Select Lines FROM WorkbenchFilter WHERE ID = {0}), ',')) ) AND " +
                                                                                         "EventTypeID IN (Select * FROM String_To_Int_Table((Select EventTypes FROM WorkbenchFilter WHERE ID = {0}), ',')) AND " +
                                                                                         "StartTime >= {1} AND " +
                                                                                         "StartTime <= {2} ",
                                                                                         filterId, startDate, endDate);

            return tableOperations.QueryRecordCount(restriction);
        }

        [AuthorizeHubRole("*")]
        [RecordOperation(typeof(Event), RecordOperation.QueryRecords)]
        public IEnumerable<EventView> QueryEvents(int filterId, string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            Tuple<DateTime, DateTime> dTuple = GetTimeRange(filterId);
            DateTime startDate = dTuple.Item1;
            DateTime endDate = dTuple.Item2;

            TableOperations<EventView> tableOperations = DataContext.Table<EventView>();
            RecordRestriction restriction = tableOperations.GetSearchRestriction(filterString) + 
                new RecordRestriction("(MeterID IN (Select * FROM String_To_Int_Table((Select Meters FROM WorkbenchFilter WHERE ID = {0}), ',')) OR LineID IN (Select * FROM String_To_Int_Table((Select Lines FROM WorkbenchFilter WHERE ID = {0}), ',')) ) AND " +
                                                                                         "EventTypeID IN (Select * FROM String_To_Int_Table((Select EventTypes FROM WorkbenchFilter WHERE ID = {0}), ',')) AND " +
                                                                                         "StartTime >= {1} AND " +
                                                                                         "StartTime <= {2} ",
                                                                                         filterId, startDate, endDate);

            return tableOperations.QueryRecords(sortField, ascending, page, pageSize, restriction);
        }

        [AuthorizeHubRole("Administrator, Engineer")]
        [RecordOperation(typeof(Event), RecordOperation.DeleteRecord)]
        public void DeleteEvent(int id)
        {
            CascadeDelete("Event", $"ID={id}");
        }

        [AuthorizeHubRole("Administrator, Engineer")]
        [RecordOperation(typeof(Event), RecordOperation.CreateNewRecord)]
        public EventView NewEvent()
        {
            return new EventView();
        }

        [AuthorizeHubRole("Administrator, Engineer")]
        [RecordOperation(typeof(Event), RecordOperation.AddNewRecord)]
        public void AddNewEvent(Event record)
        {
            DataContext.Table<Event>().AddNewRecord(record);
        }

        [AuthorizeHubRole("Administrator, Engineer")]
        [RecordOperation(typeof(Event), RecordOperation.UpdateRecord)]
        public bool UpdateEvent(EventView record, bool propagate)
        {
            DateTime oldStartTime = DataContext.Connection.ExecuteScalar<DateTime>($"SELECT StartTime FROM Event WHERE ID = {record.ID}");
            if (oldStartTime != record.StartTime)
            {
                // Get Time Stamp shift
                Ticks ticks = record.StartTime - oldStartTime;
                record.EndTime = record.EndTime.AddTicks(ticks);
                // Update event records
                // IF propagate is true update all associated with the file

                if (propagate)
                {
                    IEnumerable<Event> events = DataContext.Table<Event>().QueryRecords(restriction: new RecordRestriction("FileGroupID = {0} AND ID <> {1}", record.FileGroupID, record.ID));
                    foreach (var e in events)
                    {
                        e.StartTime = e.StartTime.AddTicks(ticks);
                        e.EndTime = e.EndTime.AddTicks(ticks);
                        e.UpdatedBy = GetCurrentUserName();
                        DataContext.Table<Event>().UpdateRecord(e);
                    }
                }


                // Update disturbance records
                // IF propagate is true update all associated with the file
                // if propagate is false update all assocaited with the event id
                IEnumerable<Disturbance> disturbances;

                if (propagate)
                    disturbances = DataContext.Table<Disturbance>().QueryRecords(restriction: new RecordRestriction("EventID IN (Select ID from Event WHERE FileGroupID = {0})", record.FileGroupID));
                else
                    disturbances = DataContext.Table<Disturbance>().QueryRecords(restriction: new RecordRestriction("EventID = {0}", record.ID));

                foreach (var disturbance in disturbances)
                {
                    disturbance.StartTime = disturbance.StartTime.AddTicks(ticks);
                    disturbance.EndTime = disturbance.EndTime.AddTicks(ticks);
                    DataContext.Table<Disturbance>().UpdateRecord(disturbance);
                }

                // Update fault records
                // IF propagate is true update all associated with the file
                // if propagate is false update all assocaited with the event id
                IEnumerable<Fault> faults;

                if (propagate)
                    faults = DataContext.Table<Fault>().QueryRecords(restriction: new RecordRestriction("EventID IN (Select ID from Event WHERE FileGroupID = {0})", record.FileGroupID));
                else
                    faults = DataContext.Table<Fault>().QueryRecords(restriction: new RecordRestriction("EventID = {0}", record.ID));

                foreach (var fault in faults)
                {
                    fault.Inception = fault.Inception.AddTicks(ticks);
                    DataContext.Table<Fault>().UpdateRecord(fault);
                }

                DataGroup dataTimeGroup = new DataGroup();
                DataGroup dataFaultAlgo = new DataGroup();

                Meter meter = DataContext.Table<Meter>().QueryRecordWhere("ID = {0}", record.MeterID);
                byte[] timeSeries = DataContext.Connection.ExecuteScalar<byte[]>("SELECT TimeDomainData FROM EventData WHERE ID = {0}", record.EventDataID);
                byte[] faultCurve = DataContext.Connection.ExecuteScalar<byte[]>("SELECT Data FROM FaultCurve WHERE EventID = {0}", record.ID);

                meter.ConnectionFactory = () => new AdoDataConnection(DataContext.Connection.Connection, DataContext.Connection.AdapterType, false);

                try
                {
                    if (timeSeries != null)
                    {
                        dataTimeGroup.FromData(meter, timeSeries);
                        foreach (var dataSeries in dataTimeGroup.DataSeries)
                        {
                            foreach (var dataPoint in dataSeries.DataPoints)
                            {
                                dataPoint.Time = dataPoint.Time.AddTicks(ticks);
                            }
                        }


                        byte[] newTimeData = dataTimeGroup.ToData();
                        DataContext.Connection.ExecuteNonQuery("Update EventData SET TimeDomainData = {0} WHERE ID = {2}", newTimeData, record.EventDataID);

                    }

                    if (faultCurve != null)
                    {
                        dataFaultAlgo.FromData(meter, faultCurve);
                        foreach (var dataSeries in dataFaultAlgo.DataSeries)
                        {
                            foreach (var dataPoint in dataSeries.DataPoints)
                            {
                                dataPoint.Time = dataPoint.Time.AddTicks(ticks);
                            }
                        }

                        byte[] newFaultAlgo = dataFaultAlgo.ToData();

                        DataContext.Connection.ExecuteNonQuery("Update FaultCurve SET Data = {0} WHERE EventID = {1}", newFaultAlgo, record.ID);
                    }
                }
                catch (Exception ex)
                {
                    OnLogStatusMessage(ex.ToString());
                }
            }

            if (record.EventTypeID == 1)
            {
                int rowCount = DataContext.Connection.Connection.ExecuteNonQuery($"UPDATE faultsummary SET IsSuppressed = 0 where eventid = {record.ID}");
                if(rowCount == 0)
                {
                    IEnumerable<FaultLocationAlgorithm> fla = DataContext.Table<FaultLocationAlgorithm>().QueryRecords();
                    foreach (var algo in fla)
                    {
                        Fault fault = new Fault()
                        {
                            EventID = record.ID,
                            Algorithm = algo.MethodName,
                            FaultNumber = 1,
                            CalculationCycle = 0,
                            Distance = -1E+308,
                            CurrentMagnitude = -1E+308,
                            CurrentLag = -1E+308,
                            PrefaultCurrent = -1E+308,
                            PostfaultCurrent = -1E+308,
                            Inception = record.StartTime,
                            DurationSeconds = (record.StartTime - record.EndTime).TotalSeconds,
                            DurationCycles = (record.StartTime - record.EndTime).TotalSeconds/60,
                            FaultType = "UNK",
                            IsSelectedAlgorithm = algo.MethodName == "Simple",
                            IsValid = true,
                            IsSuppressed = false                         
                        };

                        DataContext.Table<Fault>().AddNewRecord(fault);
                        
                    }
                }
            }
            else
            {
                DataContext.Connection.Connection.ExecuteNonQuery($"UPDATE faultsummary SET IsSuppressed = 1 where eventid = {record.ID}");
            }
            DataContext.Table<Event>().UpdateRecord(MakeEventFromEventView(record));
            return true;
        }

        private Event MakeEventFromEventView(EventView record)
        {
            Event newEvent = new Event();
            newEvent.ID = record.ID;
            newEvent.FileGroupID = record.FileGroupID;
            newEvent.MeterID = record.MeterID;
            newEvent.LineID = record.LineID;
            newEvent.EventTypeID = record.EventTypeID;
            newEvent.EventDataID = record.EventDataID;
            newEvent.Name = record.Name;
            newEvent.Alias = record.Alias;
            newEvent.ShortName = record.ShortName;
            newEvent.StartTime = record.StartTime;
            newEvent.EndTime = record.EndTime;
            newEvent.Samples = record.Samples;
            newEvent.TimeZoneOffset = record.TimeZoneOffset;
            newEvent.SamplesPerSecond = record.SamplesPerSecond; 
            newEvent.SamplesPerCycle = record.SamplesPerCycle;
            newEvent.Description = record.Description;
            newEvent.UpdatedBy = GetCurrentUserName();
            return newEvent;
        }





        #endregion

        #region [SingleEvent Operations]

        [AuthorizeHubRole("*")]
        [RecordOperation(typeof(SingleEvent), RecordOperation.QueryRecordCount)]
        public int QuerySingleEventCount(int eventId, string filterString)
        {
            return DataContext.Table<SingleEvent>().QueryRecordCount(new RecordRestriction("ID = {0}", eventId ));
        }

        [AuthorizeHubRole("*")]
        [RecordOperation(typeof(SingleEvent), RecordOperation.QueryRecords)]
        public IEnumerable<SingleEvent> QuerySingleEvents(int eventId, string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            return DataContext.Table<SingleEvent>().QueryRecords(sortField, ascending, page, pageSize, new RecordRestriction("ID = {0}", eventId));
        }

        [AuthorizeHubRole("Administrator, Engineer")]
        [RecordOperation(typeof(SingleEvent), RecordOperation.DeleteRecord)]
        public void DeleteSingleEvent(int id)
        {
            CascadeDelete("Event", $"ID={id}");
        }

        [AuthorizeHubRole("Administrator, Engineer")]
        [RecordOperation(typeof(SingleEvent), RecordOperation.CreateNewRecord)]
        public EventView NewSingleEvent()
        {
            return new EventView();
        }

        [AuthorizeHubRole("Administrator, Engineer")]
        [RecordOperation(typeof(SingleEvent), RecordOperation.AddNewRecord)]
        public void AddNewSingleEvent(SingleEvent record)
        {
            DataContext.Table<SingleEvent>().AddNewRecord(record);
        }

        [AuthorizeHubRole("Administrator, Engineer")]
        [RecordOperation(typeof(SingleEvent), RecordOperation.UpdateRecord)]
        public bool UpdateSingleEvent(SingleEvent record, bool propagate)
        {
            DateTime oldStartTime = DataContext.Connection.ExecuteScalar<DateTime>($"SELECT StartTime FROM Event WHERE ID = {record.ID}");
            if (oldStartTime != record.StartTime)
            {
                // Get Time Stamp shift
                Ticks ticks = record.StartTime - oldStartTime;
                record.EndTime = record.EndTime.AddTicks(ticks);

                // Update event records
                // IF propagate is true update all associated with the file

                if (propagate)
                {
                    IEnumerable<Event> events = DataContext.Table<Event>().QueryRecords(restriction: new RecordRestriction("FileGroupID = {0} AND ID <> {1}", record.FileGroupID, record.ID));
                    foreach (var e in events)
                    {
                        e.StartTime = e.StartTime.AddTicks(ticks);
                        e.EndTime = e.EndTime.AddTicks(ticks);
                        e.UpdatedBy = GetCurrentUserName();
                        DataContext.Table<Event>().UpdateRecord(e);
                    }
                }


                // Update disturbance records
                // IF propagate is true update all associated with the file
                // if propagate is false update all assocaited with the event id
                IEnumerable<Disturbance> disturbances;

                if (propagate)
                    disturbances = DataContext.Table<Disturbance>().QueryRecords(restriction: new RecordRestriction("EventID IN (Select ID from Event WHERE FileGroupID = {0})", record.FileGroupID));
                else
                    disturbances = DataContext.Table<Disturbance>().QueryRecords(restriction: new RecordRestriction("EventID = {0}", record.ID));

                foreach (var disturbance in disturbances)
                {
                    disturbance.StartTime = disturbance.StartTime.AddTicks(ticks);
                    disturbance.EndTime = disturbance.EndTime.AddTicks(ticks);
                    DataContext.Table<Disturbance>().UpdateRecord(disturbance);
                }

                // Update fault records
                // IF propagate is true update all associated with the file
                // if propagate is false update all assocaited with the event id
                IEnumerable<Fault> faults;

                if (propagate)
                    faults = DataContext.Table<Fault>().QueryRecords(restriction: new RecordRestriction("EventID IN (Select ID from Event WHERE FileGroupID = {0})", record.FileGroupID));
                else
                    faults = DataContext.Table<Fault>().QueryRecords(restriction: new RecordRestriction("EventID = {0}", record.ID));

                foreach (var fault in faults)
                {
                    fault.Inception = fault.Inception.AddTicks(ticks);
                    DataContext.Table<Fault>().UpdateRecord(fault);
                }

                DataGroup dataTimeGroup = new DataGroup();
                DataGroup dataFaultAlgo = new DataGroup();

                Meter meter = DataContext.Table<Meter>().QueryRecordWhere("ID = {0}", record.MeterID);
                byte[] timeSeries = DataContext.Connection.ExecuteScalar<byte[]>("SELECT TimeDomainData FROM EventData WHERE ID = {0}", record.EventDataID);
                byte[] faultCurve = DataContext.Connection.ExecuteScalar<byte[]>("SELECT Data FROM FaultCurve WHERE EventID = {0}", record.ID);

                meter.ConnectionFactory = () => new AdoDataConnection(DataContext.Connection.Connection, DataContext.Connection.AdapterType, false);

                try
                {
                    if (timeSeries != null)
                    {
                        dataTimeGroup.FromData(meter, timeSeries);
                        foreach (var dataSeries in dataTimeGroup.DataSeries)
                        {
                            foreach (var dataPoint in dataSeries.DataPoints)
                            {
                                dataPoint.Time = dataPoint.Time.AddTicks(ticks);
                            }
                        }

                        byte[] newTimeData = dataTimeGroup.ToData();
                        DataContext.Connection.ExecuteNonQuery("Update EventData SET TimeDomainData = {0} WHERE ID = {2}", newTimeData, record.EventDataID);

                    }

                    if (faultCurve != null)
                    {
                        dataFaultAlgo.FromData(meter, faultCurve);
                        foreach (var dataSeries in dataFaultAlgo.DataSeries)
                        {
                            foreach (var dataPoint in dataSeries.DataPoints)
                            {
                                dataPoint.Time = dataPoint.Time.AddTicks(ticks);
                            }
                        }

                        byte[] newFaultAlgo = dataFaultAlgo.ToData();

                        DataContext.Connection.ExecuteNonQuery("Update FaultCurve SET Data = {0} WHERE EventID = {1}", newFaultAlgo, record.ID);
                    }
                }
                catch (Exception ex)
                {
                    OnLogStatusMessage(ex.ToString());
                }
            }

            if (record.EventTypeID == 1)
            {
                int rowCount = DataContext.Connection.Connection.ExecuteNonQuery($"UPDATE faultsummary SET IsSuppressed = 0 where eventid = {record.ID}");
                if (rowCount == 0)
                {
                    IEnumerable<FaultLocationAlgorithm> fla = DataContext.Table<FaultLocationAlgorithm>().QueryRecords();
                    foreach (var algo in fla)
                    {
                        Fault fault = new Fault()
                        {
                            EventID = record.ID,
                            Algorithm = algo.MethodName,
                            FaultNumber = 1,
                            CalculationCycle = 0,
                            Distance = -1E+308,
                            CurrentMagnitude = -1E+308,
                            CurrentLag = -1E+308,
                            PrefaultCurrent = -1E+308,
                            PostfaultCurrent = -1E+308,
                            Inception = record.StartTime,
                            DurationSeconds = (record.StartTime - record.EndTime).TotalSeconds,
                            DurationCycles = (record.StartTime - record.EndTime).TotalSeconds / 60,
                            FaultType = "UNK",
                            IsSelectedAlgorithm = algo.MethodName == "Simple",
                            IsValid = true,
                            IsSuppressed = false
                        };

                        DataContext.Table<Fault>().AddNewRecord(fault);

                    }
                }
            }
            else
            {
                DataContext.Connection.Connection.ExecuteNonQuery($"UPDATE faultsummary SET IsSuppressed = 1 where eventid = {record.ID}");
            }
            DataContext.Table<Event>().UpdateRecord(MakeEventFromEventView(record));
            return true;
        }

        private Event MakeEventFromSingleEvent(SingleEvent record)
        {
            Event newEvent = new Event();
            newEvent.ID = record.ID;
            newEvent.FileGroupID = record.FileGroupID;
            newEvent.MeterID = record.MeterID;
            newEvent.LineID = record.LineID;
            newEvent.EventTypeID = record.EventTypeID;
            newEvent.EventDataID = record.EventDataID;
            newEvent.Name = record.Name;
            newEvent.Alias = record.Alias;
            newEvent.ShortName = record.ShortName;
            newEvent.StartTime = record.StartTime;
            newEvent.EndTime = record.EndTime;
            newEvent.Samples = record.Samples;
            newEvent.TimeZoneOffset = record.TimeZoneOffset;
            newEvent.SamplesPerSecond = record.SamplesPerSecond;
            newEvent.SamplesPerCycle = record.SamplesPerCycle;
            newEvent.Description = record.Description;
            newEvent.UpdatedBy = GetCurrentUserName();
            return newEvent;
        }

        #endregion

        #region [EventsForDate Operations]

        public IEnumerable<EventView> GetAllEventsForDate(int eventId, string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            DateTime date = DataContext.Connection.ExecuteScalar<DateTime>("Select StartTime FROM Event WHERE ID = {0}", eventId);
            DateTime startTime = date.AddMinutes(-5);
            DateTime endTime = date.AddMinutes(5);

            if (!filterString.EndsWith("%"))
                filterString += "%";


            return DataContext.Table<EventView>().QueryRecords(sortField, ascending, page, pageSize, new RecordRestriction("StartTime >= {0} AND StartTime <= {1} AND (ID LIKE {2} OR MeterName LIKE {3} OR LineName LIKE {4} OR EventTypeName LIKE {5})", startTime, endTime, filterString, filterString, filterString, filterString));
        }

        public int GetCountAllEventsForDate(int eventId, string filterString)
        {
            int seconds = DataContext.Connection.ExecuteScalar<int>("Select Value FROM Setting WHERE Name = 'WorkbenchTimeRangeInSeconds'");
            DateTime date = DataContext.Connection.ExecuteScalar<DateTime>("Select StartTime FROM Event WHERE ID = {0}", eventId);
            DateTime startTime = date.AddSeconds(-1*seconds);
            DateTime endTime = date.AddSeconds(seconds);

            if (!filterString.EndsWith("%"))
                filterString += "%";


            return DataContext.Table<EventView>().QueryRecordCount( new RecordRestriction("StartTime >= {0} AND StartTime <= {1} AND (ID LIKE {2} OR MeterName LIKE {3} OR LineName LIKE {4} OR EventTypeName Like {5})", startTime, endTime, filterString, filterString, filterString, filterString));
        }

        [AuthorizeHubRole("*")]
        [RecordOperation(typeof(EventForDate), RecordOperation.QueryRecordCount)]
        public int QueryEventForDateCount(int eventId, int time, string filterString)
        {
            int seconds = DataContext.Connection.ExecuteScalar<int>("Select Value FROM Setting WHERE Name = 'WorkbenchTimeRangeInSeconds'");
            DateTime date = DataContext.Connection.ExecuteScalar<DateTime>("Select "+ (time == 1 ? "StartTime" : "EndTime") + " FROM Event WHERE ID = {0}", eventId);
            DateTime startTime = date.AddSeconds(-1 * seconds);
            DateTime endTime = date.AddSeconds(seconds);
            if (!filterString.EndsWith("%"))
                filterString += "%";


            return DataContext.Table<EventView>().QueryRecordCount(new RecordRestriction("StartTime >= {0} AND StartTime <= {1} AND (ID LIKE {2} OR StartTime LIKE {3} OR EndTime LIKE {4} OR MeterName LIKE {5} OR LineName LIKE {6})", startTime, endTime, filterString, filterString, filterString, filterString, filterString));
        }

        [AuthorizeHubRole("*")]
        [RecordOperation(typeof(EventForDate), RecordOperation.QueryRecords)]
        public IEnumerable<EventView> QueryEventForDate(int eventId, int time, string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            string timeWord = (time == 1 ? "StartTime" : "EndTime");
            int seconds = DataContext.Connection.ExecuteScalar<int>("Select Value FROM Setting WHERE Name = 'WorkbenchTimeRangeInSeconds'");
            DateTime date = DataContext.Connection.ExecuteScalar<DateTime>("Select " + (time == 1 ? "StartTime" : "EndTime") + " FROM Event WHERE ID = {0}", eventId);
            DateTime startTime = date.AddSeconds(-1 * seconds);
            DateTime endTime = date.AddSeconds(seconds);
            if (!filterString.EndsWith("%"))
                filterString += "%";


            return DataContext.Table<EventView>().QueryRecords(sortField, ascending, page, pageSize, new RecordRestriction("StartTime >= {0} AND StartTime <= {1} AND (ID LIKE {2} OR StartTime LIKE {3} OR EndTime LIKE {4} OR MeterName LIKE {5} OR LineName LIKE {6})", startTime, endTime, filterString, filterString, filterString, filterString, filterString));

        }

        [AuthorizeHubRole("Administrator, Engineer")]
        [RecordOperation(typeof(EventForDate), RecordOperation.DeleteRecord)]
        public void DeleteEventForDate(int id)
        {
            CascadeDelete("Event", $"ID={id}");
        }

        [AuthorizeHubRole("Administrator, Engineer")]
        [RecordOperation(typeof(EventForDate), RecordOperation.CreateNewRecord)]
        public Event NewEventForDate()
        {
            return new Event();
        }

        [AuthorizeHubRole("Administrator, Engineer")]
        [RecordOperation(typeof(EventForDate), RecordOperation.AddNewRecord)]
        public void AddNewEventforDate(Event record)
        {
            DataContext.Table<Event>().AddNewRecord(record);
        }

        [AuthorizeHubRole("Administrator, Engineer")]
        [RecordOperation(typeof(EventForDate), RecordOperation.UpdateRecord)]
        public bool UpdateEventForRecord(EventView record, bool propagate)
        {
            DateTime oldStartTime = DataContext.Connection.ExecuteScalar<DateTime>($"SELECT StartTime FROM Event WHERE ID = {record.ID}");
            if (oldStartTime != record.StartTime)
            {
                // Get Time Stamp shift
                Ticks ticks = record.StartTime - oldStartTime;
                record.EndTime = record.EndTime.AddTicks(ticks);

                // Update event records
                // IF propagate is true update all associated with the file

                if (propagate)
                {
                    IEnumerable<Event> events = DataContext.Table<Event>().QueryRecords(restriction: new RecordRestriction("FileGroupID = {0} AND ID <> {1}", record.FileGroupID, record.ID));
                    foreach (var e in events)
                    {
                        e.StartTime = e.StartTime.AddTicks(ticks);
                        e.EndTime = e.EndTime.AddTicks(ticks);
                        e.UpdatedBy = GetCurrentUserName();
                        DataContext.Table<Event>().UpdateRecord(e);
                    }
                }


                // Update disturbance records
                // IF propagate is true update all associated with the file
                // if propagate is false update all assocaited with the event id
                IEnumerable<Disturbance> disturbances;

                if (propagate)
                    disturbances = DataContext.Table<Disturbance>().QueryRecords(restriction: new RecordRestriction("EventID IN (Select ID from Event WHERE FileGroupID = {0})", record.FileGroupID));
                else
                    disturbances = DataContext.Table<Disturbance>().QueryRecords(restriction: new RecordRestriction("EventID = {0}", record.ID));

                foreach (var disturbance in disturbances)
                {
                    disturbance.StartTime = disturbance.StartTime.AddTicks(ticks);
                    disturbance.EndTime = disturbance.EndTime.AddTicks(ticks);
                    DataContext.Table<Disturbance>().UpdateRecord(disturbance);
                }

                // Update fault records
                // IF propagate is true update all associated with the file
                // if propagate is false update all assocaited with the event id
                IEnumerable<Fault> faults;

                if (propagate)
                    faults = DataContext.Table<Fault>().QueryRecords(restriction: new RecordRestriction("EventID IN (Select ID from Event WHERE FileGroupID = {0})", record.FileGroupID));
                else
                    faults = DataContext.Table<Fault>().QueryRecords(restriction: new RecordRestriction("EventID = {0}", record.ID));

                foreach (var fault in faults)
                {
                    fault.Inception = fault.Inception.AddTicks(ticks);
                    DataContext.Table<Fault>().UpdateRecord(fault);
                }

                DataGroup dataTimeGroup = new DataGroup();
                DataGroup dataFaultAlgo = new DataGroup();

                Meter meter = DataContext.Table<Meter>().QueryRecordWhere("ID = {0}", record.MeterID);
                byte[] timeSeries = DataContext.Connection.ExecuteScalar<byte[]>("SELECT TimeDomainData FROM EventData WHERE ID = {0}", record.EventDataID);
                byte[] faultCurve = DataContext.Connection.ExecuteScalar<byte[]>("SELECT Data FROM FaultCurve WHERE EventID = {0}", record.ID);

                meter.ConnectionFactory = () => new AdoDataConnection(DataContext.Connection.Connection, DataContext.Connection.AdapterType, false);

                try
                {
                    if (timeSeries != null)
                    {
                        dataTimeGroup.FromData(meter, timeSeries);
                        foreach (var dataSeries in dataTimeGroup.DataSeries)
                        {
                            foreach (var dataPoint in dataSeries.DataPoints)
                            {
                                dataPoint.Time = dataPoint.Time.AddTicks(ticks);
                            }
                        }

                        byte[] newTimeData = dataTimeGroup.ToData();
                        DataContext.Connection.ExecuteNonQuery("Update EventData SET TimeDomainData = {0} WHERE ID = {2}", newTimeData, record.EventDataID);

                    }

                    if (faultCurve != null)
                    {
                        dataFaultAlgo.FromData(meter, faultCurve);
                        foreach (var dataSeries in dataFaultAlgo.DataSeries)
                        {
                            foreach (var dataPoint in dataSeries.DataPoints)
                            {
                                dataPoint.Time = dataPoint.Time.AddTicks(ticks);
                            }
                        }

                        byte[] newFaultAlgo = dataFaultAlgo.ToData();

                        DataContext.Connection.ExecuteNonQuery("Update FaultCurve SET Data = {0} WHERE EventID = {1}", newFaultAlgo, record.ID);
                    }
                }
                catch (Exception ex)
                {
                    OnLogStatusMessage(ex.ToString());
                }
            }

            if (record.EventTypeID == 1)
            {
                int rowCount = DataContext.Connection.Connection.ExecuteNonQuery($"UPDATE faultsummary SET IsSuppressed = 0 where eventid = {record.ID}");
                if (rowCount == 0)
                {
                    IEnumerable<FaultLocationAlgorithm> fla = DataContext.Table<FaultLocationAlgorithm>().QueryRecords();
                    foreach (var algo in fla)
                    {
                        Fault fault = new Fault()
                        {
                            EventID = record.ID,
                            Algorithm = algo.MethodName,
                            FaultNumber = 1,
                            CalculationCycle = 0,
                            Distance = -1E+308,
                            CurrentMagnitude = -1E+308,
                            CurrentLag = -1E+308,
                            PrefaultCurrent = -1E+308,
                            PostfaultCurrent = -1E+308,
                            Inception = record.StartTime,
                            DurationSeconds = (record.StartTime - record.EndTime).TotalSeconds,
                            DurationCycles = (record.StartTime - record.EndTime).TotalSeconds / 60,
                            FaultType = "UNK",
                            IsSelectedAlgorithm = algo.MethodName == "Simple",
                            IsValid = true,
                            IsSuppressed = false
                        };

                        DataContext.Table<Fault>().AddNewRecord(fault);

                    }
                }
            }
            else
            {
                DataContext.Connection.Connection.ExecuteNonQuery($"UPDATE faultsummary SET IsSuppressed = 1 where eventid = {record.ID}");
            }
            DataContext.Table<Event>().UpdateRecord(MakeEventFromEventView(record));
            return true;
        }



        #endregion

        #region [EventsForDay Operations]  
        [AuthorizeHubRole("*")]
        [RecordOperation(typeof(EventForDay), RecordOperation.QueryRecordCount)]
        public int QueryEventForDayCount(DateTime date, string eventTypes, int filterId, string filterString)
        {
            DateTime startTime = new DateTime(date.Date.Ticks);
            DateTime endTime = startTime.AddDays(1).AddMilliseconds(-1);
            IEnumerable<EventType> types = DataContext.Table<EventType>().QueryRecords();
            string eventTypeList = "";
            string sql = "";

            if (filterId > -1)
            {
                sql += $"(MeterID IN (Select * FROM String_To_Int_Table((Select Meters FROM WorkbenchFilter WHERE ID = {filterId}), ',')) OR LineID IN (Select * FROM String_To_Int_Table((Select Lines FROM WorkbenchFilter WHERE ID = {filterId}), ',')) ) AND";
            }

            sql += $" StartTime >= '{startTime}' AND StartTime <= '{endTime}' ";
            if (eventTypes != "")
            {
                foreach (var type in types)
                {
                    if (eventTypes.Contains(type.Name))
                        eventTypeList += "'" + type.Name + "',";
                }

                eventTypeList = eventTypeList.Remove(eventTypeList.Length - 1, 1);
                if (!filterString.EndsWith("%"))
                    filterString += "%";

                sql += $" AND EventTypeID IN (SELECT ID FROM EventType WHERE Name IN ({eventTypeList})) ";
            }

            TableOperations<EventView> table = DataContext.Table<EventView>();
            RecordRestriction restriction = table.GetSearchRestriction(filterString) + new RecordRestriction(sql);
            return table.QueryRecordCount(restriction);
        }

        [AuthorizeHubRole("*")]
        [RecordOperation(typeof(EventForDay), RecordOperation.QueryRecords)]
        public IEnumerable<EventView> QueryEventForDay(DateTime date, string eventTypes, int filterId, string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            DateTime startTime = new DateTime(date.Date.Ticks);
            DateTime endTime = startTime.AddDays(1).AddMilliseconds(-1);
            IEnumerable<EventType> types = DataContext.Table<EventType>().QueryRecords();
            string eventTypeList = "";
            string sql = "";

            if (filterId > -1)
            {
                sql += $"(MeterID IN (Select * FROM String_To_Int_Table((Select Meters FROM WorkbenchFilter WHERE ID = {filterId}), ',')) OR LineID IN (Select * FROM String_To_Int_Table((Select Lines FROM WorkbenchFilter WHERE ID = {filterId}), ',')) ) AND";
            }

            sql += $" StartTime >= '{startTime}' AND StartTime <= '{endTime}' ";
            if (eventTypes != "")
            {
                foreach (var type in types)
                {
                    if (eventTypes.Contains(type.Name))
                        eventTypeList += "'" + type.Name + "',";
                }

                eventTypeList = eventTypeList.Remove(eventTypeList.Length - 1, 1);
                if (!filterString.EndsWith("%"))
                    filterString += "%";

                sql += $" AND EventTypeID IN (SELECT ID FROM EventType WHERE Name IN ({eventTypeList})) ";
            }

            TableOperations<EventView> table = DataContext.Table<EventView>();
            RecordRestriction restriction = table.GetSearchRestriction(filterString) + new RecordRestriction(sql);

            return table.QueryRecords(sortField, ascending, page, pageSize, restriction);
        }

        [AuthorizeHubRole("Administrator, Engineer")]
        [RecordOperation(typeof(EventForDay), RecordOperation.DeleteRecord)]
        public void DeleteEventForDay(int id)
        {
            CascadeDelete("Event", $"ID={id}");
        }

        [AuthorizeHubRole("Administrator, Engineer")]
        [RecordOperation(typeof(EventForDay), RecordOperation.CreateNewRecord)]
        public Event NewEventForDay()
        {
            return new Event();
        }

        [AuthorizeHubRole("Administrator, Engineer")]
        [RecordOperation(typeof(EventForDay), RecordOperation.AddNewRecord)]
        public void AddNewEventForDay(Event record)
        {
            DataContext.Table<Event>().AddNewRecord(record);
        }

        [AuthorizeHubRole("Administrator, Engineer")]
        [RecordOperation(typeof(EventForDay), RecordOperation.UpdateRecord)]
        public bool UpdateEventForDayRecord(EventView record, bool propagate)
        {
            DateTime oldStartTime = DataContext.Connection.ExecuteScalar<DateTime>($"SELECT StartTime FROM Event WHERE ID = {record.ID}");
            if (oldStartTime != record.StartTime)
            {
                // Get Time Stamp shift
                Ticks ticks = record.StartTime - oldStartTime;
                record.EndTime = record.EndTime.AddTicks(ticks);

                // Update event records
                // IF propagate is true update all associated with the file

                if (propagate)
                {
                    IEnumerable<Event> events = DataContext.Table<Event>().QueryRecords(restriction: new RecordRestriction("FileGroupID = {0} AND ID <> {1}", record.FileGroupID, record.ID));
                    foreach (var e in events)
                    {
                        e.StartTime = e.StartTime.AddTicks(ticks);
                        e.EndTime = e.EndTime.AddTicks(ticks);
                        e.UpdatedBy = GetCurrentUserName();
                        DataContext.Table<Event>().UpdateRecord(e);
                    }
                }


                // Update disturbance records
                // IF propagate is true update all associated with the file
                // if propagate is false update all assocaited with the event id
                IEnumerable<Disturbance> disturbances;

                if (propagate)
                    disturbances = DataContext.Table<Disturbance>().QueryRecords(restriction: new RecordRestriction("EventID IN (Select ID from Event WHERE FileGroupID = {0})", record.FileGroupID));
                else
                    disturbances = DataContext.Table<Disturbance>().QueryRecords(restriction: new RecordRestriction("EventID = {0}", record.ID));

                foreach (var disturbance in disturbances)
                {
                    disturbance.StartTime = disturbance.StartTime.AddTicks(ticks);
                    disturbance.EndTime = disturbance.EndTime.AddTicks(ticks);
                    DataContext.Table<Disturbance>().UpdateRecord(disturbance);
                }

                // Update fault records
                // IF propagate is true update all associated with the file
                // if propagate is false update all assocaited with the event id
                IEnumerable<Fault> faults;

                if (propagate)
                    faults = DataContext.Table<Fault>().QueryRecords(restriction: new RecordRestriction("EventID IN (Select ID from Event WHERE FileGroupID = {0})", record.FileGroupID));
                else
                    faults = DataContext.Table<Fault>().QueryRecords(restriction: new RecordRestriction("EventID = {0}", record.ID));

                foreach (var fault in faults)
                {
                    fault.Inception = fault.Inception.AddTicks(ticks);
                    DataContext.Table<Fault>().UpdateRecord(fault);
                }


                DataGroup dataTimeGroup = new DataGroup();
                DataGroup dataFaultAlgo = new DataGroup();

                Meter meter = DataContext.Table<Meter>().QueryRecordWhere("ID = {0}", record.MeterID);
                byte[] timeSeries = DataContext.Connection.ExecuteScalar<byte[]>("SELECT TimeDomainData FROM EventData WHERE ID = {0}", record.EventDataID);
                byte[] faultCurve = DataContext.Connection.ExecuteScalar<byte[]>("SELECT Data FROM FaultCurve WHERE EventID = {0}", record.ID);

                meter.ConnectionFactory = () => new AdoDataConnection(DataContext.Connection.Connection, DataContext.Connection.AdapterType, false);

                try
                {
                    if (timeSeries != null)
                    {
                        dataTimeGroup.FromData(meter, timeSeries);
                        foreach (var dataSeries in dataTimeGroup.DataSeries)
                        {
                            foreach (var dataPoint in dataSeries.DataPoints)
                            {
                                dataPoint.Time = dataPoint.Time.AddTicks(ticks);
                            }
                        }


                        byte[] newTimeData = dataTimeGroup.ToData();
                        DataContext.Connection.ExecuteNonQuery("Update EventData SET TimeDomainData = {0} WHERE ID = {2}", newTimeData, record.EventDataID);

                    }

                    if (faultCurve != null)
                    {
                        dataFaultAlgo.FromData(meter, faultCurve);
                        foreach (var dataSeries in dataFaultAlgo.DataSeries)
                        {
                            foreach (var dataPoint in dataSeries.DataPoints)
                            {
                                dataPoint.Time = dataPoint.Time.AddTicks(ticks);
                            }
                        }

                        byte[] newFaultAlgo = dataFaultAlgo.ToData();

                        DataContext.Connection.ExecuteNonQuery("Update FaultCurve SET Data = {0} WHERE EventID = {1}", newFaultAlgo, record.ID);
                    }
                }
                catch (Exception ex)
                {
                    OnLogStatusMessage(ex.ToString());
                }
            }

            if (record.EventTypeID == 1)
            {
                int rowCount = DataContext.Connection.Connection.ExecuteNonQuery($"UPDATE faultsummary SET IsSuppressed = 0 where eventid = {record.ID}");
                if (rowCount == 0)
                {
                    IEnumerable<FaultLocationAlgorithm> fla = DataContext.Table<FaultLocationAlgorithm>().QueryRecords();
                    foreach (var algo in fla)
                    {
                        Fault fault = new Fault()
                        {
                            EventID = record.ID,
                            Algorithm = algo.MethodName,
                            FaultNumber = 1,
                            CalculationCycle = 0,
                            Distance = -1E+308,
                            CurrentMagnitude = -1E+308,
                            CurrentLag = -1E+308,
                            PrefaultCurrent = -1E+308,
                            PostfaultCurrent = -1E+308,
                            Inception = record.StartTime,
                            DurationSeconds = (record.StartTime - record.EndTime).TotalSeconds,
                            DurationCycles = (record.StartTime - record.EndTime).TotalSeconds / 60,
                            FaultType = "UNK",
                            IsSelectedAlgorithm = algo.MethodName == "Simple",
                            IsValid = true,
                            IsSuppressed = false
                        };

                        DataContext.Table<Fault>().AddNewRecord(fault);

                    }
                }
            }
            else
            {
                DataContext.Connection.Connection.ExecuteNonQuery($"UPDATE faultsummary SET IsSuppressed = 1 where eventid = {record.ID}");
            }
            DataContext.Table<Event>().UpdateRecord(MakeEventFromEventView(record));
            return true;
        }



        #endregion

        #region [EventsForMeter Operations]

        public IEnumerable<EventView> GetEventsForMeter(int meterId, int filterId, string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            Tuple<DateTime, DateTime> dTuple = GetTimeRange(filterId);
            DateTime startDate = dTuple.Item1;
            DateTime endDate = dTuple.Item2;

            return DataContext.Table<EventView>().QueryRecords(sortField, ascending, page, pageSize, new RecordRestriction("MeterID = {0} AND StartTime >= {1} AND StartTime <= {2} AND EventTypeID IN (Select * FROM String_To_Int_Table((Select EventTypes FROM WorkbenchFilter WHERE ID = {3}), ',')) AND (ID LIKE {4} OR MeterName LIKE {5} OR LineName LIKE {6} OR EventTypeName LIKE {7})", meterId, startDate, endDate, filterId, filterString, filterString, filterString, filterString));
        }

        public int GetCountEventsForMeter(int meterId, int filterId, string filterString)
        {
            Tuple<DateTime, DateTime> dTuple = GetTimeRange(filterId);
            DateTime startDate = dTuple.Item1;
            DateTime endDate = dTuple.Item2;

            return DataContext.Table<EventView>().QueryRecordCount(new RecordRestriction("MeterID = {0} AND StartTime >= {1} AND StartTime <= {2} AND EventTypeID IN (Select * FROM String_To_Int_Table((Select EventTypes FROM WorkbenchFilter WHERE ID = {3}), ',')) AND (ID LIKE {4} OR MeterName LIKE {5} OR LineName LIKE {6} OR EventTypeName LIKE {7})", meterId, startDate, endDate, filterId, filterString, filterString, filterString, filterString));
        }

        [AuthorizeHubRole("*")]
        [RecordOperation(typeof(EventForMeter), RecordOperation.QueryRecordCount)]
        public int QueryEventsForMeterCount(int meterId, int filterId, string filterString)
        {
            Tuple<DateTime, DateTime> dTuple = GetTimeRange(filterId);
            DateTime startDate = dTuple.Item1;
            DateTime endDate = dTuple.Item2;

            TableOperations<EventView> tableOperations = DataContext.Table<EventView>();
            RecordRestriction restriction = tableOperations.GetSearchRestriction(filterString) + 
                new RecordRestriction("MeterID = {0} AND " +
                                    "EventTypeID IN (Select * FROM String_To_Int_Table((Select EventTypes FROM WorkbenchFilter WHERE ID = {1}), ',')) AND " +
                                    "StartTime >= {2} AND " +
                                    "StartTime <= {3} ",
                                    meterId,filterId, startDate, endDate);

            return tableOperations.QueryRecordCount(restriction);
        }

        [AuthorizeHubRole("*")]
        [RecordOperation(typeof(EventForMeter), RecordOperation.QueryRecords)]
        public IEnumerable<EventView> QueryEventsForMeters(int meterId, int filterId, string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            Tuple<DateTime, DateTime> dTuple = GetTimeRange(filterId);
            DateTime startDate = dTuple.Item1;
            DateTime endDate = dTuple.Item2;

            TableOperations<EventView> tableOperations = DataContext.Table<EventView>();
            RecordRestriction restriction = tableOperations.GetSearchRestriction(filterString) + 
                new RecordRestriction("MeterID = {0} AND " +
                                    "EventTypeID IN (Select * FROM String_To_Int_Table((Select EventTypes FROM WorkbenchFilter WHERE ID = {1}), ',')) AND " +
                                    "StartTime >= {2} AND " +
                                    "StartTime <= {3} ",
                                    meterId, filterId, startDate, endDate);

            return tableOperations.QueryRecords(sortField, ascending, page, pageSize, restriction);
        }

        [AuthorizeHubRole("Administrator, Engineer")]
        [RecordOperation(typeof(EventForMeter), RecordOperation.DeleteRecord)]
        public void DeleteEventForMeter(int id)
        {
            CascadeDelete("Event", $"ID={id}");
        }

        [AuthorizeHubRole("Administrator, Engineer")]
        [RecordOperation(typeof(EventForMeter), RecordOperation.CreateNewRecord)]
        public EventView NewEventForMeter()
        {
            return new EventView();
        }

        [AuthorizeHubRole("Administrator, Engineer")]
        [RecordOperation(typeof(EventForMeter), RecordOperation.AddNewRecord)]
        public void AddNewEventForMeter(Event record)
        {
            DataContext.Table<Event>().AddNewRecord(record);
        }

        [AuthorizeHubRole("Administrator, Engineer")]
        [RecordOperation(typeof(EventForMeter), RecordOperation.UpdateRecord)]
        public bool UpdateEventForMeter(EventView record, bool propagate)
        {
            DateTime oldStartTime = DataContext.Connection.ExecuteScalar<DateTime>($"SELECT StartTime FROM Event WHERE ID = {record.ID}");
            if (oldStartTime != record.StartTime)
            {
                // Get Time Stamp shift
                Ticks ticks = record.StartTime - oldStartTime;
                record.EndTime = record.EndTime.AddTicks(ticks);

                // Update event records
                // IF propagate is true update all associated with the file

                if (propagate)
                {
                    IEnumerable<Event> events = DataContext.Table<Event>().QueryRecords(restriction: new RecordRestriction("FileGroupID = {0} AND ID <> {1}", record.FileGroupID, record.ID));
                    foreach (var e in events)
                    {
                        e.StartTime = e.StartTime.AddTicks(ticks);
                        e.EndTime = e.EndTime.AddTicks(ticks);
                        e.UpdatedBy = GetCurrentUserName();
                        DataContext.Table<Event>().UpdateRecord(e);
                    }
                }


                // Update disturbance records
                // IF propagate is true update all associated with the file
                // if propagate is false update all assocaited with the event id
                IEnumerable<Disturbance> disturbances;

                if (propagate)
                    disturbances = DataContext.Table<Disturbance>().QueryRecords(restriction: new RecordRestriction("EventID IN (Select ID from Event WHERE FileGroupID = {0})", record.FileGroupID));
                else
                    disturbances = DataContext.Table<Disturbance>().QueryRecords(restriction: new RecordRestriction("EventID = {0}", record.ID));

                foreach (var disturbance in disturbances)
                {
                    disturbance.StartTime = disturbance.StartTime.AddTicks(ticks);
                    disturbance.EndTime = disturbance.EndTime.AddTicks(ticks);
                    DataContext.Table<Disturbance>().UpdateRecord(disturbance);
                }

                // Update fault records
                // IF propagate is true update all associated with the file
                // if propagate is false update all assocaited with the event id
                IEnumerable<Fault> faults;

                if (propagate)
                    faults = DataContext.Table<Fault>().QueryRecords(restriction: new RecordRestriction("EventID IN (Select ID from Event WHERE FileGroupID = {0})", record.FileGroupID));
                else
                    faults = DataContext.Table<Fault>().QueryRecords(restriction: new RecordRestriction("EventID = {0}", record.ID));

                foreach (var fault in faults)
                {
                    fault.Inception = fault.Inception.AddTicks(ticks);
                    DataContext.Table<Fault>().UpdateRecord(fault);
                }

                DataGroup dataTimeGroup = new DataGroup();
                DataGroup dataFaultAlgo = new DataGroup();

                Meter meter = DataContext.Table<Meter>().QueryRecordWhere("ID = {0}", record.MeterID);
                byte[] timeSeries = DataContext.Connection.ExecuteScalar<byte[]>("SELECT TimeDomainData FROM EventData WHERE ID = {0}", record.EventDataID);
                byte[] faultCurve = DataContext.Connection.ExecuteScalar<byte[]>("SELECT Data FROM FaultCurve WHERE EventID = {0}", record.ID);

                meter.ConnectionFactory = () => new AdoDataConnection(DataContext.Connection.Connection, DataContext.Connection.AdapterType, false);

                try
                {
                    if (timeSeries != null)
                    {
                        dataTimeGroup.FromData(meter, timeSeries);
                        foreach (var dataSeries in dataTimeGroup.DataSeries)
                        {
                            foreach (var dataPoint in dataSeries.DataPoints)
                            {
                                dataPoint.Time = dataPoint.Time.AddTicks(ticks);
                            }
                        }

                        byte[] newTimeData = dataTimeGroup.ToData();
                        DataContext.Connection.ExecuteNonQuery("Update EventData SET TimeDomainData = {0} WHERE ID = {2}", newTimeData, record.EventDataID);

                    }

                    if (faultCurve != null)
                    {
                        dataFaultAlgo.FromData(meter, faultCurve);
                        foreach (var dataSeries in dataFaultAlgo.DataSeries)
                        {
                            foreach (var dataPoint in dataSeries.DataPoints)
                            {
                                dataPoint.Time = dataPoint.Time.AddTicks(ticks);
                            }
                        }

                        byte[] newFaultAlgo = dataFaultAlgo.ToData();

                        DataContext.Connection.ExecuteNonQuery("Update FaultCurve SET Data = {0} WHERE EventID = {1}", newFaultAlgo, record.ID);
                    }
                }
                catch (Exception ex)
                {
                    OnLogStatusMessage(ex.ToString());
                }
            }

            if (record.EventTypeID == 1)
            {
                int rowCount = DataContext.Connection.Connection.ExecuteNonQuery($"UPDATE faultsummary SET IsSuppressed = 0 where eventid = {record.ID}");
                if (rowCount == 0)
                {
                    IEnumerable<FaultLocationAlgorithm> fla = DataContext.Table<FaultLocationAlgorithm>().QueryRecords();
                    foreach (var algo in fla)
                    {
                        Fault fault = new Fault()
                        {
                            EventID = record.ID,
                            Algorithm = algo.MethodName,
                            FaultNumber = 1,
                            CalculationCycle = 0,
                            Distance = -1E+308,
                            CurrentMagnitude = -1E+308,
                            CurrentLag = -1E+308,
                            PrefaultCurrent = -1E+308,
                            PostfaultCurrent = -1E+308,
                            Inception = record.StartTime,
                            DurationSeconds = (record.StartTime - record.EndTime).TotalSeconds,
                            DurationCycles = (record.StartTime - record.EndTime).TotalSeconds / 60,
                            FaultType = "UNK",
                            IsSelectedAlgorithm = algo.MethodName == "Simple",
                            IsValid = true,
                            IsSuppressed = false
                        };

                        DataContext.Table<Fault>().AddNewRecord(fault);

                    }
                }
            }
            else
            {
                DataContext.Connection.Connection.ExecuteNonQuery($"UPDATE faultsummary SET IsSuppressed = 1 where eventid = {record.ID}");
            }
            DataContext.Table<Event>().UpdateRecord(MakeEventFromEventView(record));
            return true;
        }

        #endregion

        #region [ MeterEventsByLine Table Operations ]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(MeterEventsByLine), RecordOperation.QueryRecordCount)]
        public int QueryMeterEventsByLineCount(int siteID, DateTime targetDate, string context, string filterString )
        {
            DataTable table = new DataTable();
            using (IDbCommand sc = DataContext.Connection.Connection.CreateCommand())
            {
                sc.CommandText = "dbo.selectSiteLinesDetailsByDate";
                sc.CommandType = CommandType.StoredProcedure;


                IDbDataParameter date = sc.CreateParameter();
                date.ParameterName = "@EventDate";
                date.Value = targetDate;
                sc.Parameters.Add(date);

                IDbDataParameter meter = sc.CreateParameter();
                meter.ParameterName = "@MeterID";
                meter.Value = siteID;
                sc.Parameters.Add(meter);


                IDbDataParameter window = sc.CreateParameter();
                window.ParameterName = "@context";
                window.Value = context;
                sc.Parameters.Add(window);
                IDataReader rdr = sc.ExecuteReader();
                table.Load(rdr);

            }

            string fe = $"theeventtype LIKE '%{filterString}%' OR thelinename LIKE '%{filterString}%' OR thefaulttype LIKE '%{filterString}%'";
            return table.Select(fe).Select(row => DataContext.Table<MeterEventsByLine>().LoadRecord(row)).Count();
        }
    

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(MeterEventsByLine), RecordOperation.QueryRecords)]
        public IEnumerable<openXDA.Model.MeterEventsByLine> QueryMeterEventsByLines(int siteID, DateTime targetDate,string context,string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            DataTable table = new DataTable();
            using (IDbCommand sc = DataContext.Connection.Connection.CreateCommand())
            {
                sc.CommandText = "dbo.selectSiteLinesDetailsByDate";
                sc.CommandType = CommandType.StoredProcedure;


                IDbDataParameter date = sc.CreateParameter();
                date.ParameterName = "@EventDate";
                date.Value = targetDate;
                sc.Parameters.Add(date);

                IDbDataParameter meter = sc.CreateParameter();
                meter.ParameterName = "@MeterID";
                meter.Value = siteID;
                sc.Parameters.Add(meter);


                IDbDataParameter window = sc.CreateParameter();
                window.ParameterName = "@context";
                window.Value = context;
                sc.Parameters.Add(window);
                IDataReader rdr = sc.ExecuteReader();
                table.Load(rdr);

            }
            string fe = $"theeventtype LIKE '%{filterString}%' OR thelinename LIKE '%{filterString}%' OR thefaulttype LIKE '%{filterString}%'";
            if (ascending)
                return table.Select(fe).Select(row => DataContext.Table<MeterEventsByLine>().LoadRecord(row)).OrderBy(x => x.GetType().GetProperty(sortField).GetValue(x));
            else
                return table.Select(fe).Select(row => DataContext.Table<MeterEventsByLine>().LoadRecord(row)).OrderByDescending(x => x.GetType().GetProperty(sortField).GetValue(x));

        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(openXDA.Model.MeterEventsByLine), RecordOperation.DeleteRecord)]
        public void DeleteMeterEventsByLine(int id)
        {
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(openXDA.Model.MeterEventsByLine), RecordOperation.CreateNewRecord)]
        public openXDA.Model.MeterEventsByLine NewMeterEventsByLine()
        {
            return new openXDA.Model.MeterEventsByLine();
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(openXDA.Model.MeterEventsByLine), RecordOperation.AddNewRecord)]
        public void AddNewMeterEventsByLine(openXDA.Model.MeterEventsByLine record)
        {
            DataContext.Table<openXDA.Model.MeterEventsByLine>().AddNewRecord(record);
        }

        [AuthorizeHubRole("Administrator, Owner")]
        [RecordOperation(typeof(openXDA.Model.MeterEventsByLine), RecordOperation.UpdateRecord)]
        public void UpdateMeterEventsByLine(openXDA.Model.MeterEventsByLine record)
        {
        }

        public void UpdateAllEventTypesForRange(List<int> eventIds, string eventType)
        {
            foreach (var eventId in eventIds)
            {
                DataContext.Connection.ExecuteNonQuery("Update Event SET EventTypeID = (SELECT ID FROM EventType WHERE Name = {0}), UpdatedBy = {1} WHERE ID ={2}", eventType, GetCurrentUserName(),eventId);
            }
        }

        #endregion

        #region [ FaultDetailsByDate Table Operations ]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(FaultsDetailsByDate), RecordOperation.QueryRecordCount)]
        public int QueryFaultDetailsByDateCount(string siteID, DateTime targetDate, string context, string filterString)
        {
            DataTable table = new DataTable();


            using (IDbCommand sc = DataContext.Connection.Connection.CreateCommand())
            {
                sc.CommandText = "dbo.selectSitesFaultsDetailsByDate";
                sc.CommandType = CommandType.StoredProcedure;
                IDbDataParameter eventDateFrom = sc.CreateParameter();
                eventDateFrom.ParameterName = "@EventDate";
                eventDateFrom.Value = targetDate;
                sc.Parameters.Add(eventDateFrom);
                IDbDataParameter param3 = sc.CreateParameter();
                param3.ParameterName = "@meterID";
                param3.Value = siteID;
                IDbDataParameter param4 = sc.CreateParameter();
                param4.ParameterName = "@username";
                param4.Value = GetCurrentUserSID();
                sc.Parameters.Add(param3);
                sc.Parameters.Add(param4);
                IDbDataParameter param5 = sc.CreateParameter();
                param5.ParameterName = "@context";
                param5.Value = context;
                sc.Parameters.Add(param5);

                IDataReader rdr = sc.ExecuteReader();
                table.Load(rdr);
            }
            string fe = $"locationname LIKE '%{filterString}%' OR thelinename LIKE '%{filterString}%' OR thefaulttype LIKE '%{filterString}%'";
            return table.Select(fe).Select(row => DataContext.Table<FaultsDetailsByDate>().LoadRecord(row)).Count();
        }


        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(FaultsDetailsByDate), RecordOperation.QueryRecords)]
        public IEnumerable<FaultsDetailsByDate> QueryFaultsDetailsByDate(string siteID, DateTime targetDate,string context, string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            DataTable table = new DataTable();


            using (IDbCommand sc = DataContext.Connection.Connection.CreateCommand())
            {
                sc.CommandText = "dbo.selectSitesFaultsDetailsByDate";
                sc.CommandType = CommandType.StoredProcedure;
                IDbDataParameter eventDateFrom = sc.CreateParameter();
                eventDateFrom.ParameterName = "@EventDate";
                eventDateFrom.Value = targetDate;
                sc.Parameters.Add(eventDateFrom);
                IDbDataParameter param3 = sc.CreateParameter();
                param3.ParameterName = "@meterID";
                param3.Value = siteID;
                IDbDataParameter param4 = sc.CreateParameter();
                param4.ParameterName = "@username";
                param4.Value = GetCurrentUserSID();
                sc.Parameters.Add(param3);
                sc.Parameters.Add(param4);
                IDbDataParameter param5 = sc.CreateParameter();
                param5.ParameterName = "@context";
                param5.Value = context;
                sc.Parameters.Add(param5);

                IDataReader rdr = sc.ExecuteReader();
                table.Load(rdr);
            }

            string fe = $"locationname LIKE '%{filterString}%' OR thelinename LIKE '%{filterString}%' OR thefaulttype LIKE '%{filterString}%'";
            if (ascending)
                return table.Select(fe).Select(row => DataContext.Table<FaultsDetailsByDate>().LoadRecord(row)).OrderBy(x => x.GetType().GetProperty(sortField).GetValue(x));
            else
                return table.Select(fe).Select(row => DataContext.Table<FaultsDetailsByDate>().LoadRecord(row)).OrderByDescending(x => x.GetType().GetProperty(sortField).GetValue(x));

        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(FaultsDetailsByDate), RecordOperation.DeleteRecord)]
        public void DeleteFaultsDetailsByDate(int id)
        {
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(FaultsDetailsByDate), RecordOperation.CreateNewRecord)]
        public FaultsDetailsByDate NewFaultsDetailsByDate()
        {
            return new FaultsDetailsByDate();
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(FaultsDetailsByDate), RecordOperation.AddNewRecord)]
        public void AddNewFaultsDetailsByDate(FaultsDetailsByDate record)
        {
            DataContext.Table<FaultsDetailsByDate>().AddNewRecord(record);
        }

        [AuthorizeHubRole("Administrator, Owner")]
        [RecordOperation(typeof(FaultsDetailsByDate), RecordOperation.UpdateRecord)]
        public void UpdateFaultsDetailsByDate(FaultsDetailsByDate record)
        {
        }

        public void UpdateFaultsDetailsByDate(List<int> eventIds, string eventType)
        {
            foreach (var eventId in eventIds)
            {
                DataContext.Connection.ExecuteNonQuery("Update Event SET EventTypeID = (SELECT ID FROM EventType WHERE Name = {0}), UpdatedBy = {1} WHERE ID ={2}", eventType, GetCurrentUserName(), eventId);
                DataContext.Connection.ExecuteNonQuery("Update FaultSummary SET IsValid = {0} WHERE EventID = {1}", (eventType != "Fault"? 0: 1), eventId);
            }
        }

        public void UndoChanges(List<int> eventIds)
        {
            foreach (var eventId in eventIds)
            {
                int eventTypeID = DataContext.Connection.ExecuteScalar<int>("Select OriginalValue FROM AuditLog WHERE PrimaryKeyValue = {0}", eventId);
                DataContext.Connection.ExecuteNonQuery("Update Event SET EventTypeID = {0}, UpdatedBy = {1} WHERE ID = {2}", eventTypeID, GetCurrentUserName(), eventId);
                DataContext.Connection.ExecuteNonQuery("Update FaultSummary SET IsValid = 1 WHERE EventID = {0}", eventId);
            }
        }

        #endregion

        #region [DisturbancesForDay Operations]
        public IEnumerable<DisturbanceView> GetDisturbancesForDay(DateTime date, string eventTypes, int filterId, string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            DateTime startTime = new DateTime(date.Date.Ticks);
            DateTime endTime = startTime.AddDays(1).AddMilliseconds(-1);
            string eventTypeList = "" + (eventTypes.Contains("5") ? "'5'," : "") + (eventTypes.Contains("4") ? "'4'," : "") + (eventTypes.Contains("3") ? "'3'," : "") + (eventTypes.Contains("2") ? "'2'," : "") + (eventTypes.Contains("1") ? "'1'," : "") + (eventTypes.Contains("0") ? "'0'," : "");
            eventTypeList = eventTypeList.Remove(eventTypeList.Length - 1, 1);
            filterString += '%';
            return DataContext.Table<DisturbanceView>().QueryRecords(sortField, ascending, page, pageSize, new RecordRestriction(
                " MeterID IN (Select * FROM String_To_Int_Table((Select Meters FROM WorkbenchFilter WHERE ID = {0}), ',')) AND " +
                " StartTime >= {1} AND StartTime <= {2} AND " +
                " (ID LIKE {3} OR StartTime LIKE {4} OR EndTime LIKE {5} OR MeterName LIKE {6} OR EventID LIKE {7} OR PhaseName Like {8}) AND " +
                $" SeverityCode IN({eventTypeList}) ",
                filterId, date, endTime, filterString, filterString, filterString, filterString, filterString,filterString));
        }

        public int GetCountDisturbancesForDay(DateTime date, string eventTypes, string filterString, int filterId)
        {
            DateTime startTime = new DateTime(date.Date.Ticks);
            DateTime endTime = startTime.AddDays(1).AddMilliseconds(-1);
            string eventTypeList = "" + (eventTypes.Contains("5") ? "'5'," : "") + (eventTypes.Contains("4") ? "'4'," : "") + (eventTypes.Contains("3") ? "'3'," : "") + (eventTypes.Contains("2") ? "'2'," : "") + (eventTypes.Contains("1") ? "'1'," : "") + (eventTypes.Contains("0") ? "'0'," : "");
            eventTypeList = eventTypeList.Remove(eventTypeList.Length - 1, 1);
            filterString += '%';

            return DataContext.Table<DisturbanceView>().QueryRecordCount(new RecordRestriction(
                " MeterID IN (Select * FROM String_To_Int_Table((Select Meters FROM WorkbenchFilter WHERE ID = {0}), ',')) AND " +
                " StartTime >= {1} AND StartTime <= {2} AND "+
                " (ID LIKE {3} OR StartTime LIKE {4} OR EndTime LIKE {5} OR MeterName LIKE {6} OR EventID LIKE {7} OR PhaseName Like {8}) AND " +
                $" SeverityCode IN({eventTypeList}) ", 
                filterId, date, endTime, filterString, filterString, filterString, filterString, filterString, filterString));
        }

        [AuthorizeHubRole("*")]
        [RecordOperation(typeof(DisturbancesForDay), RecordOperation.QueryRecordCount)]
        public int QueryDisturbancesForDayCount(DateTime date, string eventTypes, int filterId, string filterString)
        {
            DateTime startTime = new DateTime(date.Date.Ticks);
            DateTime endTime = startTime.AddDays(1).AddMilliseconds(-1);
            string eventTypeList = "" + (eventTypes.Contains("5") ? "'5'," : "") + (eventTypes.Contains("4") ? "'4'," : "") + (eventTypes.Contains("3") ? "'3'," : "") + (eventTypes.Contains("2") ? "'2'," : "") + (eventTypes.Contains("1") ? "'1'," : "") + (eventTypes.Contains("0") ? "'0'," : "");
            eventTypeList = eventTypeList.Remove(eventTypeList.Length - 1, 1);
            filterString += '%';

            return DataContext.Table<DisturbanceView>().QueryRecordCount(new RecordRestriction(
                " MeterID IN (Select * FROM String_To_Int_Table((Select Meters FROM WorkbenchFilter WHERE ID = {0}), ',')) AND " +
                " StartTime >= {1} AND StartTime <= {2} AND " +
                " (ID LIKE {3} OR MeterName LIKE {3} OR EventID LIKE {3} OR PhaseName Like {3}) AND " +
                $" SeverityCode IN({eventTypeList}) ",
                filterId, date, endTime, filterString));
        }

        [AuthorizeHubRole("*")]
        [RecordOperation(typeof(DisturbancesForDay), RecordOperation.QueryRecords)]
        public IEnumerable<DisturbanceView> QueryDisturbancesForDay(DateTime date, string eventTypes, int filterId, string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            DateTime startTime = new DateTime(date.Date.Ticks);
            DateTime endTime = startTime.AddDays(1).AddMilliseconds(-1);
            string eventTypeList = "" + (eventTypes.Contains("5") ? "'5'," : "") + (eventTypes.Contains("4") ? "'4'," : "") + (eventTypes.Contains("3") ? "'3'," : "") + (eventTypes.Contains("2") ? "'2'," : "") + (eventTypes.Contains("1") ? "'1'," : "") + (eventTypes.Contains("0") ? "'0'," : "");
            eventTypeList = eventTypeList.Remove(eventTypeList.Length - 1, 1);
            if (!filterString.EndsWith("%"))
                filterString += "%";

            return DataContext.Table<DisturbanceView>().QueryRecords(sortField, ascending, page, pageSize, new RecordRestriction(
                " MeterID IN (Select * FROM String_To_Int_Table((Select Meters FROM WorkbenchFilter WHERE ID = {0}), ',')) AND " +
                " StartTime >= {1} AND StartTime <= {2} AND " +
                " (ID LIKE {3} OR MeterName LIKE {3} OR EventID LIKE {3} OR PhaseName Like {3}) AND " +
                $" SeverityCode IN({eventTypeList}) ",
                filterId, date, endTime, filterString));
        }

        [AuthorizeHubRole("Administrator, Engineer")]
        [RecordOperation(typeof(DisturbancesForDay), RecordOperation.DeleteRecord)]
        public void DeleteDisturbancesForDay(int id)
        {
            CascadeDelete("Distrubance", $"ID={id}");
        }

        [AuthorizeHubRole("Administrator, Engineer")]
        [RecordOperation(typeof(DisturbancesForDay), RecordOperation.CreateNewRecord)]
        public DisturbanceView NewDisturbancesForDay()
        {
            return new DisturbanceView();
        }

        [AuthorizeHubRole("Administrator, Engineer")]
        [RecordOperation(typeof(DisturbancesForDay), RecordOperation.AddNewRecord)]
        public void AddNewDisturbancesForDay(DisturbanceView record)
        {
            DataContext.Table<Disturbance>().AddNewRecord(MakeDisturbanceFromDisturbanceView(record));
        }

        [AuthorizeHubRole("Administrator, Engineer")]
        [RecordOperation(typeof(DisturbancesForDay), RecordOperation.UpdateRecord)]
        public void UpdateDisturbancesForDayRecord(DisturbanceView record)
        {
            DataContext.Table<Disturbance>().UpdateRecord(MakeDisturbanceFromDisturbanceView(record));
        }

        private Disturbance MakeDisturbanceFromDisturbanceView(DisturbanceView record)
        {
            Disturbance nd = new Disturbance();
            nd.ID = record.ID;
            nd.EventID = record.EventID;
            nd.EventTypeID = record.EventTypeID;
            nd.PhaseID = record.PhaseID;
            nd.Magnitude = record.Magnitude;
            nd.PerUnitMagnitude = record.PerUnitMagnitude;
            nd.StartTime = record.StartTime;
            nd.EndTime = record.EndTime;
            nd.DurationSeconds = record.DurationSeconds;
            nd.DurationCycles = record.DurationCycles;
            nd.StartIndex = record.StartIndex;
            nd.EndIndex = record.EndIndex;
            nd.UpdatedBy = GetCurrentUserName();
            return nd;
        }

        #endregion

        #region [DisturbancesForMeter Operations]

        public IEnumerable<DisturbanceView> GetDisturbancesForMeter(int meterId, int filterId, string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            Tuple<DateTime, DateTime> dTuple = GetTimeRange(filterId);
            DateTime startDate = dTuple.Item1;
            DateTime endDate = dTuple.Item2;
            return DataContext.Table<DisturbanceView>().QueryRecords(sortField, ascending, page, pageSize, new RecordRestriction("MeterID = {0} AND StartTime >= {1} AND StartTime <= {2} AND (ID LIKE {3} OR MeterName LIKE {4} OR PhaseName LIKE {5})", meterId, startDate, endDate, filterString, filterString, filterString));
        }

        public int GetCountDisturbancesForMeter(int meterId, int filterId, string filterString)
        {
            Tuple<DateTime, DateTime> dTuple = GetTimeRange(filterId);
            DateTime startDate = dTuple.Item1;
            DateTime endDate = dTuple.Item2;
            return DataContext.Table<DisturbanceView>().QueryRecordCount(new RecordRestriction("MeterID = {0} AND StartTime >= {1} AND StartTime <= {2} AND (ID LIKE {3} OR MeterName LIKE {4} OR PhaseName LIKE {5})", meterId, startDate, endDate, filterString, filterString, filterString));
        }

        [AuthorizeHubRole("*")]
        [RecordOperation(typeof(DisturbancesForMeter), RecordOperation.QueryRecordCount)]
        public int QueryDisturbancesForMeterCount(int meterId, int filterId, string filterString)
        {
            Tuple<DateTime, DateTime> dTuple = GetTimeRange(filterId);
            DateTime startDate = dTuple.Item1;
            DateTime endDate = dTuple.Item2;
            TableOperations<DisturbanceView> tableOperations = DataContext.Table<DisturbanceView>();
            RecordRestriction restriction = tableOperations.GetSearchRestriction(filterString) + new RecordRestriction("MeterID = {0} AND StartTime >= {1} AND StartTime <= {2} AND PhaseName='Worst'",
                                                                                         meterId, startDate, endDate);

            return tableOperations.QueryRecordCount(restriction);
        }

        [AuthorizeHubRole("*")]
        [RecordOperation(typeof(DisturbancesForMeter), RecordOperation.QueryRecords)]
        public IEnumerable<DisturbanceView> QueryDisturbancesForMeter(int meterId, int filterId, string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            Tuple<DateTime, DateTime> dTuple = GetTimeRange(filterId);
            DateTime startDate = dTuple.Item1;
            DateTime endDate = dTuple.Item2;

            TableOperations<DisturbanceView> tableOperations = DataContext.Table<DisturbanceView>();
            RecordRestriction restriction = tableOperations.GetSearchRestriction(filterString) + new RecordRestriction("MeterID = {0} AND StartTime >= {1} AND StartTime <= {2} AND PhaseName='Worst'",
                                                                                         meterId, startDate, endDate);

            return tableOperations.QueryRecords(sortField, ascending, page, pageSize, restriction);
        }

        [AuthorizeHubRole("Administrator, Engineer")]
        [RecordOperation(typeof(DisturbancesForMeter), RecordOperation.DeleteRecord)]
        public void DeleteDisturbancesForMeter(int id)
        {
            CascadeDelete("Distrubance", $"ID={id}");
        }

        [AuthorizeHubRole("Administrator, Engineer")]
        [RecordOperation(typeof(DisturbancesForMeter), RecordOperation.CreateNewRecord)]
        public DisturbanceView NewDisturbancesForMeter()
        {
            return new DisturbanceView();
        }

        [AuthorizeHubRole("Administrator, Engineer")]
        [RecordOperation(typeof(DisturbancesForMeter), RecordOperation.AddNewRecord)]
        public void AddNewDisturbancesForMeter(DisturbanceView record)
        {
            DataContext.Table<Disturbance>().AddNewRecord(MakeDisturbanceFromDisturbanceView(record));
        }

        [AuthorizeHubRole("Administrator, Engineer")]
        [RecordOperation(typeof(DisturbancesForMeter), RecordOperation.UpdateRecord)]
        public void UpdateDisturbancesForMeterRecord(DisturbanceView record)
        {
            DataContext.Table<Disturbance>().UpdateRecord(MakeDisturbanceFromDisturbanceView(record));
        }

        #endregion

        #region [BreakerOperation Operations]

        [AuthorizeHubRole("*")]
        [RecordOperation(typeof(openXDA.Model.BreakerOperation), RecordOperation.QueryRecordCount)]
        public int QueryBreakerOperationCount(int eventId, string filterString)
        {
            return DataContext.Table<BreakerView>().QueryRecordCount(new RecordRestriction("EventID = {0}", eventId));
        }

        [AuthorizeHubRole("*")]
        [RecordOperation(typeof(openXDA.Model.BreakerOperation), RecordOperation.QueryRecords)]
        public IEnumerable<BreakerView> QueryBreakerOperations(int eventId, string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            return DataContext.Table<BreakerView>().QueryRecords(sortField, ascending, page, pageSize, new RecordRestriction("EventID = {0}", eventId));
        }

        [AuthorizeHubRole("Administrator, Engineer")]
        [RecordOperation(typeof(openXDA.Model.BreakerOperation), RecordOperation.DeleteRecord)]
        public void DeleteBreakerOperation(int id)
        {
            CascadeDelete("BreakerOperation", $"ID={id}");
        }

        [AuthorizeHubRole("Administrator, Engineer")]
        [RecordOperation(typeof(openXDA.Model.BreakerOperation), RecordOperation.CreateNewRecord)]
        public BreakerView NewBreakerOperation()
        {
            return new BreakerView();
        }

        [AuthorizeHubRole("Administrator, Engineer")]
        [RecordOperation(typeof(openXDA.Model.BreakerOperation), RecordOperation.AddNewRecord)]
        public void AddNewBreakerOperation(openXDA.Model.BreakerOperation record)
        {
        }

        [AuthorizeHubRole("Administrator, Engineer")]
        [RecordOperation(typeof(openXDA.Model.BreakerOperation), RecordOperation.UpdateRecord)]
        public void UpdateBreakerOperation(BreakerView record)
        {
            openXDA.Model.BreakerOperation bo = DataContext.Table<openXDA.Model.BreakerOperation>().QueryRecords(restriction: new RecordRestriction("ID = {0}", record.ID)).FirstOrDefault();
            bo.TripCoilEnergized = DateTime.Parse(record.Energized);
            bo.BreakerOperationTypeID = DataContext.Connection.ExecuteScalar<int>("SELECT ID FROM BreakerOperationType WHERE Name = {0}", record.OperationType);
            bo.UpdatedBy = GetCurrentUserName();
            DataContext.Table<openXDA.Model.BreakerOperation>().UpdateRecord(bo);
        }

        #endregion

        #region [BreakersForDay Operations]  
        [AuthorizeHubRole("*")]
        [RecordOperation(typeof(BreakersForDay), RecordOperation.QueryRecordCount)]
        public int QueryBreakerForDayCount(DateTime date, string operationTypes, int filterId, string filterString)
        {
            DateTime startTime = new DateTime(date.Date.Ticks);
            DateTime endTime = startTime.AddDays(1).AddMilliseconds(-1);
            IEnumerable<BreakerOperationType> types = DataContext.Table<BreakerOperationType>().QueryRecords();
            string operationTypeList = "";

            foreach (var type in types)
            {
                if (operationTypes.Contains(type.Name))
                    operationTypeList += "'" + type.Name + "',"; 
            }

            operationTypeList = operationTypeList.Remove(operationTypeList.Length - 1, 1);
            if (!filterString.EndsWith("%"))
                filterString += "%";

            TableOperations<BreakerView> table = DataContext.Table<BreakerView>();
            RecordRestriction restriction ;
            restriction = table.GetSearchRestriction(filterString) + new RecordRestriction($"(MeterID IN (Select * FROM String_To_Int_Table((Select Meters FROM WorkbenchFilter WHERE ID = {filterId}), ','))) AND Energized >= '{startTime}' AND Energized <= '{endTime}' AND OperationType IN ({operationTypeList})");
            return table.QueryRecordCount(restriction);
        }

        [AuthorizeHubRole("*")]
        [RecordOperation(typeof(BreakersForDay), RecordOperation.QueryRecords)]
        public IEnumerable<BreakerView> QueryBreakersForDay(DateTime date, string operationTypes, int filterId, string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            DateTime startTime = new DateTime(date.Date.Ticks);
            DateTime endTime = startTime.AddDays(1).AddMilliseconds(-1);
            IEnumerable<BreakerOperationType> types = DataContext.Table<BreakerOperationType>().QueryRecords();
            string operationTypeList = "";

            foreach (var type in types)
            {
                if (operationTypes.Contains(type.Name))
                    operationTypeList += "'" + type.Name + "',";
            }

            operationTypeList = operationTypeList.Remove(operationTypeList.Length - 1, 1);
            if (!filterString.EndsWith("%"))
                filterString += "%";

            TableOperations<BreakerView> table = DataContext.Table<BreakerView>();
            RecordRestriction restriction;
            restriction = table.GetSearchRestriction(filterString) + new RecordRestriction($"(MeterID IN (Select * FROM String_To_Int_Table((Select Meters FROM WorkbenchFilter WHERE ID = {filterId}), ','))) AND Energized >= '{startTime}' AND Energized <= '{endTime}' AND OperationType IN ({operationTypeList})");

            return table.QueryRecords(sortField, ascending, page, pageSize, restriction);
        }

        [AuthorizeHubRole("Administrator, Engineer")]
        [RecordOperation(typeof(BreakersForDay), RecordOperation.DeleteRecord)]
        public void DeleteBreakersForDay(int id)
        {
            CascadeDelete("BreakerOperation", $"ID={id}");
        }

        [AuthorizeHubRole("Administrator, Engineer")]
        [RecordOperation(typeof(BreakersForDay), RecordOperation.CreateNewRecord)]
        public BreakerView NewBreakersForDay()
        {
            return new BreakerView();
        }

        [AuthorizeHubRole("Administrator, Engineer")]
        [RecordOperation(typeof(BreakersForDay), RecordOperation.AddNewRecord)]
        public void AddNewBreakersForDay(Event record)
        {
        }

        [AuthorizeHubRole("Administrator, Engineer")]
        [RecordOperation(typeof(BreakersForDay), RecordOperation.UpdateRecord)]
        public void UpdateBreakersForDayRecord(BreakerView record)
        {
            openXDA.Model.BreakerOperation bo = DataContext.Table<openXDA.Model.BreakerOperation>().QueryRecords(restriction: new RecordRestriction("ID = {0}", record.ID)).FirstOrDefault();
            bo.TripCoilEnergized = DateTime.Parse(record.Energized);
            bo.BreakerOperationTypeID = DataContext.Connection.ExecuteScalar<int>("SELECT ID FROM BreakerOperationType WHERE Name = {0}", record.OperationType);
            bo.UpdatedBy = GetCurrentUserName();
            DataContext.Table<openXDA.Model.BreakerOperation>().UpdateRecord(bo);
        }



        #endregion

        #region [FaultsForDay Operations]

        public IEnumerable<FaultView> GetFaultsForDay(DateTime date, string eventTypes, int filterId, string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            DateTime startTime = new DateTime(date.Date.Ticks);
            DateTime endTime = startTime.AddDays(1).AddMilliseconds(-1);
            string eventTypeList = "" + (eventTypes.Contains("500 kV") ? "'500'," : "") + (eventTypes.Contains("300 kV") ? "'300'," : "") + (eventTypes.Contains("230 kV") ? "'230'," : "") + (eventTypes.Contains("135 kV") ? "'135'," : "") + (eventTypes.Contains("115 kV") ? "'115'," : "") + (eventTypes.Contains("69 kV") ? "'69'," : "") + (eventTypes.Contains("46 kV") ? "'46'," : "") + (eventTypes.Contains("0 kV") ? "'0'," : "");
            eventTypeList = eventTypeList.Remove(eventTypeList.Length - 1, 1);
            filterString += '%';
            return DataContext.Table<FaultView>().QueryRecords(sortField, ascending, page, pageSize, new RecordRestriction(
                " RK = 1 AND MeterID IN (Select * FROM String_To_Int_Table((Select Meters FROM WorkbenchFilter WHERE ID = {0}), ',')) AND " +
                " InceptionTime >= {1} AND InceptionTime <= {2} AND " +
                " (MeterName LIKE {3} OR EventID LIKE {4} OR LineName LIKE {5} OR Voltage LIKE {6} OR FaultType Like {7} OR CurrentDistance LIKE {8} OR InceptionTime LIKE {9}) AND " +
                $" Voltage IN({eventTypeList}) ",
                filterId, date, endTime, filterString, filterString, filterString, filterString, filterString, filterString, filterString));
        }

        public int GetCountFaultsForDay(DateTime date, string eventTypes, string filterString, int filterId)
        {
            DateTime startTime = new DateTime(date.Date.Ticks);
            DateTime endTime = startTime.AddDays(1).AddMilliseconds(-1);
            string eventTypeList = "" + (eventTypes.Contains("500 kV") ? "'500'," : "") + (eventTypes.Contains("300 kV") ? "'300'," : "") + (eventTypes.Contains("230 kV") ? "'230'," : "") + (eventTypes.Contains("135 kV") ? "'135'," : "") + (eventTypes.Contains("115 kV") ? "'115'," : "") + (eventTypes.Contains("69 kV") ? "'69'," : "") + (eventTypes.Contains("46 kV") ? "'46'," : "") + (eventTypes.Contains("0 kV") ? "'0'," : "");
            eventTypeList = eventTypeList.Remove(eventTypeList.Length - 1, 1);
            filterString += '%';

            return DataContext.Table<FaultView>().QueryRecordCount(new RecordRestriction(
                " RK = 1 AND MeterID IN (Select * FROM String_To_Int_Table((Select Meters FROM WorkbenchFilter WHERE ID = {0}), ',')) AND " +
                " InceptionTime >= {1} AND InceptionTime <= {2} AND " +
                " (MeterName LIKE {3} OR EventID LIKE {4} OR LineName LIKE {5} OR Voltage LIKE {6} OR FaultType Like {7} OR CurrentDistance LIKE {8} OR InceptionTime LIKE {9}) AND " +
                $" Voltage IN({eventTypeList}) ",
                filterId, date, endTime, filterString, filterString, filterString, filterString, filterString, filterString, filterString));
        }

        #endregion

        #region [FaultForMeter Operations]
        [AuthorizeHubRole("*")]
        [RecordOperation(typeof(FaultForMeter), RecordOperation.QueryRecordCount)]
        public int QueryFaultorMeterCount(int meterId, int filterId, string filterString)
        {
            Tuple<DateTime, DateTime> dTuple = GetTimeRange(filterId);
            DateTime startDate = dTuple.Item1;
            DateTime endDate = dTuple.Item2;

            TableOperations<FaultForMeter> tableOperations = DataContext.Table<FaultForMeter>();
            RecordRestriction restriction = tableOperations.GetSearchRestriction(filterString) + new RecordRestriction("MeterID = {0} AND InceptionTime >= {1} AND InceptionTime <= {2} AND RK=1",
                                                                                         meterId, startDate.Date, endDate.Date);

            return tableOperations.QueryRecordCount(restriction);
        }

        [AuthorizeHubRole("*")]
        [RecordOperation(typeof(FaultForMeter), RecordOperation.QueryRecords)]
        public IEnumerable<FaultForMeter> QueryFaultsForMeter(int meterId, int filterId, string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            Tuple<DateTime, DateTime> dTuple = GetTimeRange(filterId);
            DateTime startDate = dTuple.Item1;
            DateTime endDate = dTuple.Item2;

            TableOperations<FaultForMeter> tableOperations = DataContext.Table<FaultForMeter>();
            RecordRestriction restriction = tableOperations.GetSearchRestriction(filterString) + new RecordRestriction("MeterID = {0} AND InceptionTime >= {1} AND InceptionTime <= {2} AND RK=1",
                                                                                         meterId, startDate.Date, endDate.Date);

            return tableOperations.QueryRecords(sortField, ascending, page, pageSize, restriction);
        }

        [AuthorizeHubRole("Administrator, Engineer")]
        [RecordOperation(typeof(FaultForMeter), RecordOperation.DeleteRecord)]
        public void DeleteFaultsForMeter(int id)
        {
            CascadeDelete("FaultSummary", $"ID={id}");
        }

        [AuthorizeHubRole("Administrator, Engineer")]
        [RecordOperation(typeof(FaultForMeter), RecordOperation.CreateNewRecord)]
        public DisturbanceView NewFaultsForMeter()
        {
            return new DisturbanceView();
        }

        [AuthorizeHubRole("Administrator, Engineer")]
        [RecordOperation(typeof(FaultForMeter), RecordOperation.AddNewRecord)]
        public void AddNewFaultsForMeter(FaultView record)
        {
            DataContext.Table<Fault>().AddNewRecord(MakeFaultFromFaultView(record));
        }

        [AuthorizeHubRole("Administrator, Engineer")]
        [RecordOperation(typeof(FaultForMeter), RecordOperation.UpdateRecord)]
        public void UpdateFaultsForMeterRecord(FaultView record)
        {
            DataContext.Table<Fault>().UpdateRecord(MakeFaultFromFaultView(record));
        }

        public Fault MakeFaultFromFaultView(FaultView record)
        {
            Fault fault = new Fault();
            fault.ID = record.ID;
            fault.EventID = record.EventID;
            fault.Algorithm = record.Algorithm;
            fault.CalculationCycle = record.CalculationCycle;
            fault.Distance = record.Distance;
            fault.CurrentMagnitude = record.CurrentMagnitude;
            fault.CurrentLag = record.CurrentLag;
            fault.PrefaultCurrent = record.PrefaultCurrent;
            fault.PostfaultCurrent = record.PostfaultCurrent;
            fault.Inception = record.InceptionTime;
            fault.DurationSeconds = record.DurationSeconds;
            fault.DurationCycles = record.DurationCycles;
            fault.FaultType = record.FaultType;
            fault.IsSelectedAlgorithm = record.IsSelectedAlgorithm;
            fault.IsValid = record.IsSelectedAlgorithm;
            fault.IsSuppressed = record.IsSuppressed;
            return fault;
        }


        #endregion

        #region [Chart Operations]

        public class EventSet
        {
            public DateTime StartDate;
            public DateTime EndDate;
            public class EventDetail
            {
                public string Name;
                public List<Tuple<DateTime, int>> Data;
                public string Color;
                public EventDetail()
                {
                    Data = new List<Tuple<DateTime, int>>();
                }
            }
            public List<EventDetail> Types;

            public EventSet()
            {
                Types = new List<EventDetail>();
            }
        }

        public EventSet GetDataForPeriod(int filterId, string tab)
        {
            EventSet eventSet = new EventSet();

            string meters = DataContext.Connection.ExecuteScalar<string>("SELECT Meters FROM WorkbenchFilter WHERE ID ={0}", filterId);
            if (meters.IsNullOrWhiteSpace())
            {
                meters = DataContext.Connection.ExecuteScalar<string>("Select * into #temp FROM (SELECT MeterID FROM MeterLine WHERE MeterID IN (Select * From String_To_Int_Table((Select Lines from WorkbenchFilter WHERE ID = {0}), ','))) As T " +
                                                                      "declare @results varchar(max) " +
                                                                      "Select @results = coalesce(@results + ',', '') + convert(varchar(12), MeterID) from #temp order by MeterID " +
                                                                      "select @results as results " +
                                                                      "DROP TABLE #temp ", filterId);
            }

            Tuple<DateTime, DateTime> dTuple = GetTimeRange(filterId);
            DateTime startDate = eventSet.StartDate = dTuple.Item1;
            DateTime endDate = eventSet.EndDate = dTuple.Item2;
            string userName = UserInfo.UserNameToSID(Thread.CurrentPrincipal.Identity.Name);

            Dictionary<string, string> colors = new Dictionary<string, string>();

            IEnumerable<DashSettings> dashSettings = DataContext.Table<DashSettings>().QueryRecords(restriction: new RecordRestriction("Name = '" + tab + "Chart'"));
            List<UserDashSettings> userDashSettings = DataContext.Table<UserDashSettings>().QueryRecords(restriction: new RecordRestriction("Name = '" + tab + "Chart' AND UserAccountID IN (SELECT ID FROM UserAccount WHERE Name = {0})", userName)).ToList();

            Dictionary<string, bool> disabledFileds = new Dictionary<string, bool>();
            foreach (DashSettings setting in dashSettings)
            {
                var index = userDashSettings.IndexOf(x => x.Name == setting.Name && x.Value == setting.Value);
                if (index >= 0)
                {
                    setting.Enabled = userDashSettings[index].Enabled;
                }

                if (!disabledFileds.ContainsKey(setting.Value))
                    disabledFileds.Add(setting.Value, setting.Enabled);

            }

            IEnumerable<DashSettings> colorSettings = DataContext.Table<DashSettings>().QueryRecords(restriction: new RecordRestriction("Name = '" + tab + "ChartColors' AND Enabled = 1"));
            List<UserDashSettings> userColorSettings = DataContext.Table<UserDashSettings>().QueryRecords(restriction: new RecordRestriction("Name = '" + tab + "ChartColors' AND UserAccountID IN (SELECT ID FROM UserAccount WHERE Name = {0})", userName)).ToList();

            foreach (var color in colorSettings)
            {
                var index = userColorSettings.IndexOf(x => x.Name == color.Name && x.Value.Split(',')?[0] == color.Value.Split(',')?[0]);
                if (index >= 0)
                {
                    color.Value = userColorSettings[index].Value;
                }

                if (colors.ContainsKey(color.Value.Split(',')[0]))
                    colors[color.Value.Split(',')[0]] = color.Value.Split(',')[1];
                else
                    colors.Add(color.Value.Split(',')[0], color.Value.Split(',')[1]);
            }

            using (IDbCommand sc = DataContext.Connection.Connection.CreateCommand())
            {
                sc.CommandText = "dbo.select" + tab + "ForMeterIDbyDateRange";
                sc.CommandType = CommandType.StoredProcedure;
                IDbDataParameter param1 = sc.CreateParameter();
                param1.ParameterName = "@EventDateFrom";
                param1.Value = startDate;
                IDbDataParameter param2 = sc.CreateParameter();
                param2.ParameterName = "@EventDateTo";
                param2.Value = endDate;
                IDbDataParameter param3 = sc.CreateParameter();
                param3.ParameterName = "@MeterID";
                param3.Value = meters;
                IDbDataParameter param4 = sc.CreateParameter();
                param4.ParameterName = "@username";
                param4.Value = userName;
                IDbDataParameter param5 = sc.CreateParameter();
                param5.ParameterName = "@context";
                param5.Value = "day";

                sc.Parameters.Add(param1);
                sc.Parameters.Add(param2);
                sc.Parameters.Add(param3);
                sc.Parameters.Add(param4);
                sc.Parameters.Add(param5);

                IDataReader rdr = sc.ExecuteReader();
                DataTable table = new DataTable();
                table.Load(rdr);

                try
                {
                    foreach (DataRow row in table.Rows)
                    {
                        foreach (DataColumn column in table.Columns)
                        {
                            if (column.ColumnName != "thedate" && !disabledFileds.ContainsKey(column.ColumnName))
                            {
                                disabledFileds.Add(column.ColumnName, true);
                                DashSettings ds = new DashSettings()
                                {
                                    Name = "" + tab + "Chart",
                                    Value = column.ColumnName,
                                    Enabled = true
                                };
                                DataContext.Table<DashSettings>().AddNewRecord(ds);

                            }

                            if (column.ColumnName != "thedate" && disabledFileds[column.ColumnName])
                            {
                                if (eventSet.Types.All(x => x.Name != column.ColumnName))
                                {
                                    eventSet.Types.Add(new EventSet.EventDetail());
                                    eventSet.Types[eventSet.Types.Count - 1].Name = column.ColumnName;
                                    if (colors.ContainsKey(column.ColumnName))
                                        eventSet.Types[eventSet.Types.Count - 1].Color = colors[column.ColumnName];
                                    else
                                    {
                                        Random r = new Random();
                                        eventSet.Types[eventSet.Types.Count - 1].Color = "#" + r.Next(256).ToString("X2") + r.Next(256).ToString("X2") + r.Next(256).ToString("X2");
                                        DashSettings ds = new DashSettings()
                                        {
                                            Name = "" + tab + "ChartColors",
                                            Value = column.ColumnName + "," + eventSet.Types[eventSet.Types.Count - 1].Color,
                                            Enabled = true
                                        };
                                        DataContext.Table<DashSettings>().AddNewRecord(ds);
                                    }
                                }
                                eventSet.Types[eventSet.Types.IndexOf(x => x.Name == column.ColumnName)].Data.Add(Tuple.Create(Convert.ToDateTime(row["thedate"]), Convert.ToInt32(row[column.ColumnName])));
                            }
                        }
                    }

                    if (!eventSet.Types.Any())
                    {
                        foreach (DataColumn column in table.Columns)
                        {
                            if (column.ColumnName != "thedate" && !disabledFileds.ContainsKey(column.ColumnName))
                            {
                                disabledFileds.Add(column.ColumnName, true);
                                DashSettings ds = new DashSettings()
                                {
                                    Name = "" + tab + "Chart",
                                    Value = column.ColumnName,
                                    Enabled = true
                                };
                                DataContext.Table<DashSettings>().AddNewRecord(ds);

                            }

                            if (column.ColumnName != "thedate" && disabledFileds[column.ColumnName])
                            {
                                if (eventSet.Types.All(x => x.Name != column.ColumnName))
                                {
                                    eventSet.Types.Add(new EventSet.EventDetail());
                                    eventSet.Types[eventSet.Types.Count - 1].Name = column.ColumnName;
                                    if (colors.ContainsKey(column.ColumnName))
                                        eventSet.Types[eventSet.Types.Count - 1].Color = colors[column.ColumnName];
                                    else
                                    {
                                        Random r = new Random();
                                        eventSet.Types[eventSet.Types.Count - 1].Color = "#" + r.Next(256).ToString("X2") + r.Next(256).ToString("X2") + r.Next(256).ToString("X2");
                                        DashSettings ds = new DashSettings()
                                        {
                                            Name = "" + tab + "ChartColors",
                                            Value = column.ColumnName + "," + eventSet.Types[eventSet.Types.Count - 1].Color,
                                            Enabled = true
                                        };
                                        DataContext.Table<DashSettings>().AddNewRecord(ds);

                                    }
                                }
                            }
                        }

                    }

                }
                finally
                {
                    if (!rdr.IsClosed)
                    {
                        rdr.Close();
                    }
                }
            }
            return eventSet;
        }


        public EventSet GetEventsForPeriod(int filterId)
        {
            EventSet eventSet = new EventSet();

            string meters = DataContext.Connection.ExecuteScalar<string>("SELECT Meters FROM WorkbenchFilter WHERE ID ={0}", filterId);
            if (meters.IsNullOrWhiteSpace())
            {
                meters = DataContext.Connection.ExecuteScalar<string>("Select * into #temp FROM (SELECT MeterID FROM MeterLine WHERE MeterID IN (Select * From String_To_Int_Table((Select Lines from WorkbenchFilter WHERE ID = {0}), ','))) As T " +
                                                                      "declare @results varchar(max) " +
                                                                      "Select @results = coalesce(@results + ',', '') + convert(varchar(12), MeterID) from #temp order by MeterID " +
                                                                      "select @results as results " +
                                                                      "DROP TABLE #temp ", filterId);
            }

            Tuple<DateTime, DateTime> dTuple = GetTimeRange(filterId);
            DateTime startDate = eventSet.StartDate = dTuple.Item1;
            DateTime endDate = eventSet.EndDate = dTuple.Item2;


            SqlConnection conn = null;
            SqlDataReader rdr = null;
            Dictionary<string, string> colors = new Dictionary<string, string>()
            {
                { "Interruption", "#C00000" },
                { "Fault", "#FF2800" },
                { "Sag", "#FF9600" },
                { "Swell", "#00FFF4" },
                { "Transient", "#FFFF00" },
                { "Other", "#0000FF" },
                { "Test", "#A9A9A9" },
                { "Breaker", "#A500FF" },
            };

            List<string> disabledFields = DataContext.Table<DashSettings>().QueryRecords(restriction: new RecordRestriction("Name = 'EventsChart' AND Enabled = 0")).Select(x => x.Value).ToList();
            IEnumerable<DashSettings> usersColors = DataContext.Table<DashSettings>().QueryRecords(restriction: new RecordRestriction("Name = 'EventsChartColors' AND Enabled = 1"));
            DataTable table = new DataTable();

            foreach (var color in usersColors)
            {
                if (colors.ContainsKey(color.Value.Split(',')[0]))
                    colors[color.Value.Split(',')[0]] = color.Value.Split(',')[1];
                else
                    colors.Add(color.Value.Split(',')[0], color.Value.Split(',')[1]);
            }


            try
            {
                conn = (SqlConnection)DataContext.Connection.Connection;
                SqlCommand cmd = new SqlCommand("dbo.selectEventsForMeterIDByDateRange", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@EventDateFrom", startDate));
                cmd.Parameters.Add(new SqlParameter("@EventDateTo", endDate));
                cmd.Parameters.Add(new SqlParameter("@MeterID", meters));
                cmd.Parameters.Add(new SqlParameter("@username", UserInfo.UserNameToSID(Thread.CurrentPrincipal.Identity.Name)));
                cmd.CommandTimeout = 300;

                rdr = cmd.ExecuteReader();
                table.Load(rdr);


                foreach (DataRow row in table.Rows)
                {
                    foreach (DataColumn column in table.Columns)
                    {
                        if (column.ColumnName != "thedate" && !disabledFields.Contains(column.ColumnName))
                        {
                            if (eventSet.Types.All(x => x.Name != column.ColumnName))
                            {
                                eventSet.Types.Add(new EventSet.EventDetail());
                                eventSet.Types[eventSet.Types.Count - 1].Name = column.ColumnName;
                                if (colors.ContainsKey(column.ColumnName))
                                    eventSet.Types[eventSet.Types.Count - 1].Color = colors[column.ColumnName];
                                else
                                {
                                    Random r = new Random();
                                    eventSet.Types[eventSet.Types.Count - 1].Color = "#" + r.Next(256).ToString("X2") + r.Next(256).ToString("X2") + r.Next(256).ToString("X2");
                                    DashSettings ds = new DashSettings()
                                    {
                                        Name = "EventsChartColors",
                                        Value = column.ColumnName + "," + eventSet.Types[eventSet.Types.Count - 1].Color,
                                        Enabled = true
                                    };
                                    DataContext.Table<DashSettings>().AddNewRecord(ds);
                                }
                            }
                            eventSet.Types[eventSet.Types.IndexOf(x => x.Name == column.ColumnName)].Data.Add(Tuple.Create(Convert.ToDateTime(row["thedate"]), Convert.ToInt32(row[column.ColumnName])));
                        }
                    }
                }

                if (!eventSet.Types.Any())
                {
                    foreach (DataColumn column in table.Columns)
                    {
                        if (column.ColumnName != "thedate" && !disabledFields.Contains(column.ColumnName))
                        {
                            if (eventSet.Types.All(x => x.Name != column.ColumnName))
                            {
                                eventSet.Types.Add(new EventSet.EventDetail());
                                eventSet.Types[eventSet.Types.Count - 1].Name = column.ColumnName;
                                if (colors.ContainsKey(column.ColumnName))
                                    eventSet.Types[eventSet.Types.Count - 1].Color = colors[column.ColumnName];
                                else
                                {
                                    Random r = new Random();
                                    eventSet.Types[eventSet.Types.Count - 1].Color = "#" + r.Next(256).ToString("X2") + r.Next(256).ToString("X2") + r.Next(256).ToString("X2");
                                    DashSettings ds = new DashSettings()
                                    {
                                        Name = "EventsChartColors",
                                        Value = column.ColumnName + "," + eventSet.Types[eventSet.Types.Count - 1].Color,
                                        Enabled = true
                                    };
                                    DataContext.Table<DashSettings>().AddNewRecord(ds);

                                }
                            }
                        }
                    }

                }


            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
                if (rdr != null)
                {
                    rdr.Close();
                }
            }

            return eventSet;
        }

        public EventSet GetDisturbancesForPeriod(int filterId)
        {
            EventSet eventSet = new EventSet();

            string meters = DataContext.Connection.ExecuteScalar<string>("SELECT Meters FROM WorkbenchFilter WHERE ID ={0}", filterId);
            if (meters.IsNullOrWhiteSpace())
            {
                meters = DataContext.Connection.ExecuteScalar<string>("Select * into #temp FROM (SELECT MeterID FROM MeterLine WHERE MeterID IN (Select * From String_To_Int_Table((Select Lines from WorkbenchFilter WHERE ID = {0}), ','))) As T " +
                                                                      "declare @results varchar(max) " +
                                                                      "Select @results = coalesce(@results + ',', '') + convert(varchar(12), MeterID) from #temp order by MeterID " +
                                                                      "select @results as results " +
                                                                      "DROP TABLE #temp ", filterId);
            }

            Tuple<DateTime, DateTime> dTuple = GetTimeRange(filterId);
            DateTime startDate = eventSet.StartDate = dTuple.Item1;
            DateTime endDate = eventSet.EndDate = dTuple.Item2;

            Dictionary<string, string> colors = new Dictionary<string, string>()
            {
                { "5", "#C00000" },
                { "4", "#FF2800" },
                { "3", "#FF9600" },
                { "2", "#00FFF4" },
                { "1", "#FFFF00" },
                { "0", "#0000FF" },
            };

            List<string> disabledFields = DataContext.Table<DashSettings>().QueryRecords(restriction: new RecordRestriction("Name = 'DisturbanceChart' AND Enabled = 0")).Select(x => x.Value).ToList();
            IEnumerable<DashSettings> usersColors = DataContext.Table<DashSettings>().QueryRecords(restriction: new RecordRestriction("Name = 'DisturbanceChartColors' AND Enabled = 1"));

            foreach (var color in usersColors)
            {
                if (colors.ContainsKey(color.Value.Split(',')[0]))
                    colors[color.Value.Split(',')[0]] = color.Value.Split(',')[1];
                else
                    colors.Add(color.Value.Split(',')[0], color.Value.Split(',')[1]);
            }


            SqlConnection conn = null;
            SqlDataReader rdr = null;

            try
            {
                conn = (SqlConnection)DataContext.Connection.Connection;
                SqlCommand cmd = new SqlCommand("dbo.selectDisturbancesForMeterIDByDateRange", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@EventDateFrom", startDate));
                cmd.Parameters.Add(new SqlParameter("@EventDateTo", endDate));
                cmd.Parameters.Add(new SqlParameter("@MeterID", meters));
                cmd.Parameters.Add(new SqlParameter("@username", UserInfo.UserNameToSID(Thread.CurrentPrincipal.Identity.Name)));
                cmd.CommandTimeout = 300;

                rdr = cmd.ExecuteReader();
                DataTable table = new DataTable();
                table.Load(rdr);

                foreach (DataRow row in table.Rows)
                {
                    foreach (DataColumn column in table.Columns)
                    {
                        if (column.ColumnName != "thedate" && !disabledFields.Contains(column.ColumnName))
                        {
                            if (eventSet.Types.All(x => x.Name != column.ColumnName))
                            {
                                eventSet.Types.Add(new EventSet.EventDetail());
                                eventSet.Types[eventSet.Types.Count - 1].Name = column.ColumnName;
                                if (colors.ContainsKey(column.ColumnName))
                                    eventSet.Types[eventSet.Types.Count - 1].Color = colors[column.ColumnName];
                                else
                                {
                                    Random r = new Random();
                                    eventSet.Types[eventSet.Types.Count - 1].Color = "#" + r.Next(256).ToString("X2") + r.Next(256).ToString("X2") + r.Next(256).ToString("X2");
                                    DashSettings ds = new DashSettings()
                                    {
                                        Name = "DisturbanceChartColors",
                                        Value = column.ColumnName + "," + eventSet.Types[eventSet.Types.Count - 1].Color,
                                        Enabled = true
                                    };
                                    DataContext.Table<DashSettings>().AddNewRecord(ds);
                                }
                            }
                            eventSet.Types[eventSet.Types.IndexOf(x => x.Name == column.ColumnName)].Data.Add(Tuple.Create(Convert.ToDateTime(row["thedate"]), Convert.ToInt32(row[column.ColumnName])));
                        }
                    }
                }

                if (!eventSet.Types.Any())
                {
                    foreach (DataColumn column in table.Columns)
                    {
                        if (column.ColumnName != "thedate" && !disabledFields.Contains(column.ColumnName))
                        {
                            if (eventSet.Types.All(x => x.Name != column.ColumnName))
                            {
                                eventSet.Types.Add(new EventSet.EventDetail());
                                eventSet.Types[eventSet.Types.Count - 1].Name = column.ColumnName;
                                if (colors.ContainsKey(column.ColumnName))
                                    eventSet.Types[eventSet.Types.Count - 1].Color = colors[column.ColumnName];
                                else
                                {
                                    Random r = new Random();
                                    eventSet.Types[eventSet.Types.Count - 1].Color = "#" + r.Next(256).ToString("X2") + r.Next(256).ToString("X2") + r.Next(256).ToString("X2");
                                    DashSettings ds = new DashSettings()
                                    {
                                        Name = "DisturbanceChartColors",
                                        Value = column.ColumnName + "," + eventSet.Types[eventSet.Types.Count - 1].Color,
                                        Enabled = true
                                    };
                                    DataContext.Table<DashSettings>().AddNewRecord(ds);

                                }
                            }
                        }
                    }

                }


            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
                if (rdr != null)
                {
                    rdr.Close();
                }
            }

            return eventSet;

        }

        public EventSet GetFaultsForPeriod(int filterId)
        {
            string meters = DataContext.Connection.ExecuteScalar<string>("SELECT Meters FROM WorkbenchFilter WHERE ID ={0}", filterId);

            EventSet eventSet = new EventSet();

            Tuple<DateTime, DateTime> dTuple = GetTimeRange(filterId);
            DateTime startDate = eventSet.StartDate = dTuple.Item1;
            DateTime endDate = eventSet.EndDate = dTuple.Item2;

            SqlConnection conn = null;
            SqlDataReader rdr = null;

            Dictionary<string, string> colors = new Dictionary<string, string>()
            {
                { "500", "#91e8e1" },
                { "300", "#f45b5b" },
                { "230", "#2b908f" },
                { "200", "#e4d354" },
                { "161", "#f15c80" },
                { "135", "#8085e9" },
                { "115", "#f7a35c" },
                { "69", "#ff0000" },
                { "46", "#434348" },
                { "0", "#90ed7d" },

            };

            List<string> disabledFields = DataContext.Table<DashSettings>().QueryRecords(restriction: new RecordRestriction("Name = 'FaultsChart' AND Enabled = 0")).Select(x => x.Value).ToList();
            IEnumerable<DashSettings> usersColors = DataContext.Table<DashSettings>().QueryRecords(restriction: new RecordRestriction("Name = 'FaultsChartColors' AND Enabled = 1"));
            DataTable table = new DataTable();

            foreach (var color in usersColors)
            {
                if (colors.ContainsKey(color.Value.Split(',')[0]))
                    colors[color.Value.Split(',')[0]] = color.Value.Split(',')[1];
                else
                    colors.Add(color.Value.Split(',')[0], color.Value.Split(',')[1]);
            }

            try
            {
                conn = (SqlConnection)DataContext.Connection.Connection;
                SqlCommand cmd = new SqlCommand("dbo.selectFaultsForMeterIDByDateRange", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@EventDateFrom", startDate));
                cmd.Parameters.Add(new SqlParameter("@EventDateTo", endDate));
                cmd.Parameters.Add(new SqlParameter("@MeterID", meters));
                cmd.Parameters.Add(new SqlParameter("@username", UserInfo.UserNameToSID(Thread.CurrentPrincipal.Identity.Name)));
                cmd.CommandTimeout = 300;

                rdr = cmd.ExecuteReader();
                table.Load(rdr);

                foreach (DataRow row in table.Rows)
                {
                    foreach (DataColumn column in table.Columns)
                    {
                        if (column.ColumnName != "thedate")
                        {
                            if (eventSet.Types.All(x => x.Name != column.ColumnName))
                            {
                                eventSet.Types.Add(new EventSet.EventDetail());
                                eventSet.Types[eventSet.Types.Count - 1].Name = column.ColumnName;
                                if (colors.ContainsKey(column.ColumnName))
                                    eventSet.Types[eventSet.Types.Count - 1].Color = colors[column.ColumnName];
                                else
                                {
                                    Random r = new Random();
                                    eventSet.Types[eventSet.Types.Count - 1].Color = "#" + r.Next(256).ToString("X2") + r.Next(256).ToString("X2") + r.Next(256).ToString("X2");
                                    DashSettings ds = new DashSettings()
                                    {
                                        Name = "FaultsChartColors",
                                        Value = column.ColumnName + "," + eventSet.Types[eventSet.Types.Count - 1].Color,
                                        Enabled = true
                                    };
                                    DataContext.Table<DashSettings>().AddNewRecord(ds);
                                }
                            }
                            eventSet.Types[eventSet.Types.IndexOf(x => x.Name == column.ColumnName)].Data.Add(Tuple.Create(Convert.ToDateTime(row["thedate"]), Convert.ToInt32(row[column.ColumnName])));
                        }
                    }
                }

                if (!eventSet.Types.Any())
                {
                    foreach (DataColumn column in table.Columns)
                    {
                        if (column.ColumnName != "thedate")
                        {
                            if (eventSet.Types.All(x => x.Name != column.ColumnName))
                            {
                                eventSet.Types.Add(new EventSet.EventDetail());
                                eventSet.Types[eventSet.Types.Count - 1].Name = column.ColumnName;
                                if (colors.ContainsKey(column.ColumnName))
                                    eventSet.Types[eventSet.Types.Count - 1].Color = colors[column.ColumnName];
                                else
                                {
                                    Random r = new Random();
                                    eventSet.Types[eventSet.Types.Count - 1].Color = "#" + r.Next(256).ToString("X2") + r.Next(256).ToString("X2") + r.Next(256).ToString("X2");
                                    DashSettings ds = new DashSettings()
                                    {
                                        Name = "FaultsChartColors",
                                        Value = column.ColumnName + "," + eventSet.Types[eventSet.Types.Count - 1].Color,
                                        Enabled = true
                                    };
                                    DataContext.Table<DashSettings>().AddNewRecord(ds);
                                }
                            }
                        }
                    }

                }


            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
                if (rdr != null)
                {
                    rdr.Close();
                }
            }

            return eventSet;

        }

        public EventSet GetBreakersForPeriod(int filterId)
        {
            string meters = DataContext.Connection.ExecuteScalar<string>("SELECT Meters FROM WorkbenchFilter WHERE ID ={0}", filterId);

            EventSet eventSet = new EventSet();
            Tuple<DateTime, DateTime> dTuple = GetTimeRange(filterId);
            DateTime startDate = eventSet.StartDate = dTuple.Item1;
            DateTime endDate = eventSet.EndDate = dTuple.Item2;



            Dictionary<string, string> colors = new Dictionary<string, string>()
            {
                { "Normal", "#ff0000" },
                { "Late", "#434348" },
                { "Indeterminate", "#90ed7d" }
            };

            List<string> disabledFields = DataContext.Table<DashSettings>().QueryRecords(restriction: new RecordRestriction("Name = 'BreakersChart' AND Enabled = 0")).Select(x => x.Value).ToList();
            IEnumerable<DashSettings> usersColors = DataContext.Table<DashSettings>().QueryRecords(restriction: new RecordRestriction("Name = 'BreakersChartColors' AND Enabled = 1"));
            DataTable table = new DataTable();

            foreach (var color in usersColors)
            {
                if (colors.ContainsKey(color.Value.Split(',')[0]))
                    colors[color.Value.Split(',')[0]] = color.Value.Split(',')[1];
                else
                    colors.Add(color.Value.Split(',')[0], color.Value.Split(',')[1]);
            }

            SqlConnection conn = null;
            SqlDataReader rdr = null;

            try
            {
                conn = (SqlConnection)DataContext.Connection.Connection;
                SqlCommand cmd = new SqlCommand("dbo.selectBreakersForMeterIDByDateRange", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@EventDateFrom", startDate));
                cmd.Parameters.Add(new SqlParameter("@EventDateTo", endDate));
                cmd.Parameters.Add(new SqlParameter("@MeterID", meters));
                cmd.Parameters.Add(new SqlParameter("@username", UserInfo.UserNameToSID(Thread.CurrentPrincipal.Identity.Name)));
                cmd.CommandTimeout = 300;

                rdr = cmd.ExecuteReader();
                table.Load(rdr);

                foreach (DataRow row in table.Rows)
                {
                    foreach (DataColumn column in table.Columns)
                    {
                        if (column.ColumnName != "thedate")
                        {
                            if (eventSet.Types.All(x => x.Name != column.ColumnName))
                            {
                                eventSet.Types.Add(new EventSet.EventDetail());
                                eventSet.Types[eventSet.Types.Count - 1].Name = column.ColumnName;
                                if (colors.ContainsKey(column.ColumnName))
                                    eventSet.Types[eventSet.Types.Count - 1].Color = colors[column.ColumnName];
                                else
                                {
                                    Random r = new Random();
                                    eventSet.Types[eventSet.Types.Count - 1].Color = "#" + r.Next(256).ToString("X2") + r.Next(256).ToString("X2") + r.Next(256).ToString("X2");
                                    DashSettings ds = new DashSettings()
                                    {
                                        Name = "BreakersChartColors",
                                        Value = column.ColumnName + "," + eventSet.Types[eventSet.Types.Count - 1].Color,
                                        Enabled = true
                                    };
                                    DataContext.Table<DashSettings>().AddNewRecord(ds);
                                }
                            }
                            eventSet.Types[eventSet.Types.IndexOf(x => x.Name == column.ColumnName)].Data.Add(Tuple.Create(Convert.ToDateTime(row["thedate"]), Convert.ToInt32(row[column.ColumnName])));
                        }
                    }
                }

                if (!eventSet.Types.Any())
                {
                    foreach (DataColumn column in table.Columns)
                    {
                        if (column.ColumnName != "thedate")
                        {
                            if (eventSet.Types.All(x => x.Name != column.ColumnName))
                            {
                                eventSet.Types.Add(new EventSet.EventDetail());
                                eventSet.Types[eventSet.Types.Count - 1].Name = column.ColumnName;
                                if (colors.ContainsKey(column.ColumnName))
                                    eventSet.Types[eventSet.Types.Count - 1].Color = colors[column.ColumnName];
                                else
                                {
                                    Random r = new Random();
                                    eventSet.Types[eventSet.Types.Count - 1].Color = "#" + r.Next(256).ToString("X2") + r.Next(256).ToString("X2") + r.Next(256).ToString("X2");
                                    DashSettings ds = new DashSettings()
                                    {
                                        Name = "BreakersChartColors",
                                        Value = column.ColumnName + "," + eventSet.Types[eventSet.Types.Count - 1].Color,
                                        Enabled = true
                                    };
                                    DataContext.Table<DashSettings>().AddNewRecord(ds);
                                }
                            }
                        }
                    }

                }

            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
                if (rdr != null)
                {
                    rdr.Close();
                }
            }

            return eventSet;

        }

        public IEnumerable<DisturbanceView> GetVoltageMagnitudeData(int filterId)
        {
            Tuple<DateTime, DateTime> dTuple = GetTimeRange(filterId);
            DateTime startDate = dTuple.Item1;
            DateTime endDate = dTuple.Item2;

            DataTable table = DataContext.Connection.RetrieveData(
                " SELECT * " + 
                " FROM DisturbanceView  " +
                " WHERE (MeterID IN (Select * FROM String_To_Int_Table((Select Meters FROM WorkbenchFilter WHERE ID = {0}), ',')) OR LineID IN (Select * FROM String_To_Int_Table((Select Lines FROM WorkbenchFilter WHERE ID = {0}), ','))) " + 
                " AND EventID IN ( SELECT ID FROM Event WHERE EventTypeID IN (Select * FROM String_To_Int_Table((Select EventTypes FROM WorkbenchFilter WHERE ID = {0}), ','))) " + 
                " AND StartTime >= {1} AND StartTime <= {2}", filterId, startDate, endDate);
            return table.Select().Select(row => DataContext.Table<DisturbanceView>().LoadRecord(row));
        }

        public IEnumerable<WorkbenchVoltageCurveView> GetCurves()
        {
            return DataContext.Table<WorkbenchVoltageCurveView>().QueryRecords("ID, LoadOrder");
        }

        #endregion

        #region [Site Summary]
        [AuthorizeHubRole("*")]
        [RecordOperation(typeof(SiteSummary), RecordOperation.QueryRecordCount)]
        public int QuerySiteSummaryCount(int filterId, string filterString)
        {
            Tuple<DateTime, DateTime> dTuple = GetTimeRange(filterId);
            DateTime startDate = dTuple.Item1;
            DateTime endDate = dTuple.Item2;

            DataTable table = DataContext.Connection.RetrieveData(
            @"SELECT
                   Meter.Name AS MeterID,
                   0 as Completeness,
                   0 as Correctness,
	               0 AS[Events],
                   0 AS Disturbances,
                   0 AS Faults,
                   0 AS MaxCurrent,
                   0 AS MinVoltage
            FROM Meter
            WHERE Meter.ID IN (Select * FROM String_To_Int_Table((Select Meters FROM WorkbenchFilter WHERE ID = {2}), ',')) AND
                  Meter.Name LIKE '%"+ filterString + @"%'
            ", startDate, endDate, filterId);

            return table.Select().Select(x => DataContext.Table<SiteSummary>().LoadRecord(x)).Count();
        }

        [AuthorizeHubRole("*")]
        [RecordOperation(typeof(SiteSummary), RecordOperation.QueryRecords)]
        public IEnumerable<SiteSummary> QuerySiteSummaries(int filterId, string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            Tuple<DateTime, DateTime> dTuple = GetTimeRange(filterId);
            DateTime startDate = dTuple.Item1;
            DateTime endDate = dTuple.Item2;
            DataTable table = new DataTable();

            using (IDbCommand sc = DataContext.Connection.Connection.CreateCommand())
            {
                sc.CommandText = "dbo.GetSiteSummaries";
                sc.CommandType = CommandType.StoredProcedure;
                sc.CommandTimeout = 600;
                IDbDataParameter param1 = sc.CreateParameter();
                param1.ParameterName = "@startDate";
                param1.Value = startDate;
                IDbDataParameter param2 = sc.CreateParameter();
                param2.ParameterName = "@endDate";
                param2.Value = endDate;
                IDbDataParameter param3 = sc.CreateParameter();
                param3.ParameterName = "@filterID";
                param3.Value = filterId;
                IDbDataParameter param4 = sc.CreateParameter();
                param4.ParameterName = "@Page";
                param4.Value = page;
                IDbDataParameter param5 = sc.CreateParameter();
                param5.ParameterName = "@RecsPerPage";
                param5.Value = pageSize;
                IDbDataParameter param6 = sc.CreateParameter();
                param6.ParameterName = "@orderBy";
                param6.Value = $"[{sortField}] {(ascending? "ASC":"DESC")}";
                IDbDataParameter param7 = sc.CreateParameter();
                param7.ParameterName = "@filterString";
                param7.Value = filterString;


                sc.Parameters.Add(param1);
                sc.Parameters.Add(param2);
                sc.Parameters.Add(param3);
                sc.Parameters.Add(param4);
                sc.Parameters.Add(param5);
                sc.Parameters.Add(param6);
                sc.Parameters.Add(param7);

                IDataReader rdr = sc.ExecuteReader();
                table.Load(rdr);


            }

            return table.Select().OrderBy(x => x["RowNum"]).Select(x => DataContext.Table<SiteSummary>().LoadRecord(x));
        }

        [AuthorizeHubRole("Administrator, Engineer")]
        [RecordOperation(typeof(SiteSummary), RecordOperation.DeleteRecord)]
        public void DeleteSiteSummary(int id)
        {
        }

        [AuthorizeHubRole("Administrator, Engineer")]
        [RecordOperation(typeof(SiteSummary), RecordOperation.CreateNewRecord)]
        public Event NewSummary()
        {
            return new Event();
        }

        [AuthorizeHubRole("Administrator, Engineer")]
        [RecordOperation(typeof(SiteSummary), RecordOperation.AddNewRecord)]
        public void AddNewSiteSummary(SiteSummary record)
        {
            DataContext.Table<SiteSummary>().AddNewRecord(record);
        }

        [AuthorizeHubRole("Administrator, Engineer")]
        [RecordOperation(typeof(SiteSummary), RecordOperation.UpdateRecord)]
        public void UpdateSiteSummary(SiteSummary record)
        {
            DataContext.Table<SiteSummary>().UpdateRecord(record);
        }


        #endregion

        #region [ AuditLog Table Operations ]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(AuditLog), RecordOperation.QueryRecordCount)]
        public int QueryAuditLogCount(string filterString)
        {
            int auditLogMax = DataContext.Connection.ExecuteScalar<int>("SELECT Value FROM Setting WHERE Name = 'MaxAuditLogRecords'");
            TableOperations<AuditLog> tableOperations = DataContext.Table<AuditLog>();
            RecordRestriction restriction;
            restriction = tableOperations.GetSearchRestriction(filterString) + new RecordRestriction("UpdatedBy IS NOT NULL AND NewValue IS NOT NULL");
            int count = tableOperations.QueryRecordCount(restriction);
            return (count > auditLogMax ? auditLogMax : count);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(AuditLog), RecordOperation.QueryRecords)]
        public IEnumerable<AuditLog> QueryAuditLogs(string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            int auditLogMax = DataContext.Connection.ExecuteScalar<int>("SELECT Value FROM Setting WHERE Name = 'MaxAuditLogRecords'");
            DataContext.CustomTableOperationTokens[typeof(AuditLog)] = new[] { new KeyValuePair<string, string>("{count}", auditLogMax.ToString()) };
            TableOperations<AuditLog> tableOperations = DataContext.Table<AuditLog>();
            RecordRestriction restriction;
            restriction = tableOperations.GetSearchRestriction(filterString) + new RecordRestriction("UpdatedBy IS NOT NULL AND NewValue IS NOT NULL");
            return tableOperations.QueryRecords(sortField, ascending, page, pageSize, restriction);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(AuditLog), RecordOperation.DeleteRecord)]
        public void DeleteAuditLog(int id)
        {
            //DataContext.Table<AuditLog>().DeleteRecord(id);
            CascadeDelete("AuditLog", $"ID = {id}");
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(AuditLog), RecordOperation.CreateNewRecord)]
        public AuditLog NewAuditLog()
        {
            return new AuditLog();
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(AuditLog), RecordOperation.AddNewRecord)]
        public void AddNewAuditLog(AuditLog record)
        {
            DataContext.Table<AuditLog>().AddNewRecord(record);
        }

        [AuthorizeHubRole("Administrator, Owner")]
        [RecordOperation(typeof(AuditLog), RecordOperation.UpdateRecord)]
        public void UpdateAuditLog(AuditLog record)
        {
            DataContext.Table<AuditLog>().UpdateRecord(record);
        }

        [AuthorizeHubRole("Administrator, Owner")]
        public void RestoreDataAuditLog(AuditLog record)
        {
            DataContext.Connection.ExecuteNonQuery($"UPDATE {record.TableName} SET {record.ColumnName} = '{record.OriginalValue}' WHERE {record.PrimaryKeyColumn} = {record.PrimaryKeyValue}");
        }

        [AuthorizeHubRole("Administrator, Owner")]
        public void RestoreMultipleDataAuditLog(List<int> IDs)
        {
            foreach(int id in IDs)
            {
                DataRow record = DataContext.Connection.RetrieveRow("SELECT * FROM AuditLog WHERE ID = {0}",id);
                DataContext.Connection.ExecuteNonQuery($"UPDATE {record["TableName"]} SET {record["ColumnName"]} = '{record["OriginalValue"]}' WHERE {record["PrimaryKeyColumn"]} = {record["PrimaryKeyValue"]}");
            }
        }


        #endregion

        #region [ DataFile Table Operations ]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(openXDA.Model.DataFile), RecordOperation.QueryRecordCount)]
        public int QueryDataFileCount(string filterString)
        {
            IEnumerable<openXDA.Model.DataReader> dataReaders = DataContext.Table<openXDA.Model.DataReader>().QueryRecords();
            TableOperations<openXDA.Model.DataFile> tableOperations = DataContext.Table<openXDA.Model.DataFile>();
            RecordRestriction restriction;
            restriction = tableOperations.GetSearchRestriction(filterString);
            RecordRestriction innerRestriction = null;
            foreach (var dataReader in dataReaders)
            {
                if(innerRestriction == null)
                    innerRestriction = new RecordRestriction($"FilePath LIKE '%.{dataReader.FilePattern.Split('.')[dataReader.FilePattern.Split('.').Length - 1]}'");
                else
                    innerRestriction |= new RecordRestriction($"FilePath LIKE '%.{dataReader.FilePattern.Split('.')[dataReader.FilePattern.Split('.').Length - 1]}'");
            }
            restriction += innerRestriction;
            return tableOperations.QueryRecordCount(restriction);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(openXDA.Model.DataFile), RecordOperation.QueryRecords)]
        public IEnumerable<openXDA.Model.DataFile> QueryDataFiles(string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            IEnumerable<openXDA.Model.DataReader> dataReaders = DataContext.Table<openXDA.Model.DataReader>().QueryRecords();
            TableOperations<openXDA.Model.DataFile> tableOperations = DataContext.Table<openXDA.Model.DataFile>();
            RecordRestriction restriction;
            restriction = tableOperations.GetSearchRestriction(filterString);
            RecordRestriction innerRestriction = null;
            foreach (var dataReader in dataReaders)
            {
                if (innerRestriction == null)
                    innerRestriction = new RecordRestriction($"FilePath LIKE '%.{dataReader.FilePattern.Split('.')[dataReader.FilePattern.Split('.').Length - 1]}'");
                else
                    innerRestriction |= new RecordRestriction($"FilePath LIKE '%.{dataReader.FilePattern.Split('.')[dataReader.FilePattern.Split('.').Length - 1]}'");
            }
            restriction += innerRestriction;
            return tableOperations.QueryRecords(sortField, ascending, page, pageSize, restriction);
        }

        public IEnumerable<Event> GetEventsByDataFile (int dataFileId)
        {
            return DataContext.Table<Event>().QueryRecords(restriction: new RecordRestriction("FileGroupID IN (SELECT FileGroupID FROM DataFile WHERE ID = {0})", dataFileId));
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(openXDA.Model.DataFile), RecordOperation.DeleteRecord)]
        public void DeleteDataFile(int id)
        {
            CascadeDelete("FileGroup", $"ID = {id}");
        }

        public void DeleteMultipleDataFiles(List<int> ids)
        {
            string idString = String.Join(",", ids);
            CascadeDelete("FileGroup", $"ID IN({idString})");
        }


        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(openXDA.Model.DataFile), RecordOperation.CreateNewRecord)]
        public openXDA.Model.DataFile NewDataFile()
        {
            return new openXDA.Model.DataFile();
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(openXDA.Model.DataFile), RecordOperation.AddNewRecord)]
        public void AddNewDataFile(openXDA.Model.DataFile record)
        {
            DataContext.Table<openXDA.Model.DataFile>().AddNewRecord(record);
        }

        [AuthorizeHubRole("Administrator, Owner")]
        [RecordOperation(typeof(openXDA.Model.DataFile), RecordOperation.UpdateRecord)]
        public void UpdateDataFile(openXDA.Model.DataFile record)
        {
            DataContext.Table<openXDA.Model.DataFile>().UpdateRecord(record);
        }

        public IEnumerable<Event> GetEventsForFileGroup(int fileGroupId)
        {
            return DataContext.Table<Event>().QueryRecords(restriction: new RecordRestriction("FileGroupID ={0}", fileGroupId));
        }

        public IEnumerable<Event> GetEventsForFileGroups(List<int> fileGroupIds)
        {
            string ids = String.Join(",",fileGroupIds);
            return DataContext.Table<Event>().QueryRecords(restriction: new RecordRestriction($"FileGroupID IN ({ids})"));
        }


        public void ReprocessFiles(List<int> meterIDs, Tuple<DateTime, DateTime> dateRange)
        {
            string commaSeparatedMeters = string.Join(",", meterIDs);
            string eventFilter = $"MeterID IN ({commaSeparatedMeters}) AND StartTime >= '{dateRange.Item1}' AND StartTime <= '{dateRange.Item2}'";

            List<Event> events = DataContext
                .Table<Event>()
                .QueryRecords(new RecordRestriction(eventFilter))
                .ToList();

            List<Tuple<int,int>> fileGroupIDs = events
                .Select(evt => Tuple.Create(evt.FileGroupID, evt.MeterID))
                .Distinct()
                .ToList();

            foreach (Tuple<int,int> fileGroupID in fileGroupIDs)
            {
                CascadeDelete("Event", $"FileGroupID = {fileGroupID}");
                CascadeDelete("EventData", $"FileGroupID = {fileGroupID}");
                OnReprocessFile(fileGroupID.Item1, fileGroupID.Item2);
            }
        }

        public void ReprocessFile(int dataFileID, int fileGroupID, int meterID, bool useFile)
        { 
            CascadeDelete("Event", $"FileGroupID = {fileGroupID}");
            CascadeDelete("EventData", $"FileGroupID = {fileGroupID}");
            if (useFile)
                CascadeDelete("FileBlob", $"DataFileID IN (SELECT ID FROM DataFile WHERE FileGroupID = {fileGroupID})");

            OnReprocessFile(fileGroupID, meterID);
        }

        #endregion

        #endregion

        #region [ DataPusher Operations ]

        #region [ MetersToDataPush Table Operations ]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(MetersToDataPush), RecordOperation.QueryRecordCount)]
        public int QueryMetersToDataPushCount(int remoteXDAInstanceId, string filterString)
        {
            TableOperations<MetersToDataPush> table = DataContext.Table<MetersToDataPush>();
            RecordRestriction restriction = table.GetSearchRestriction(filterString) + new RecordRestriction("ID IN (SELECT MetersToDataPushID FROM RemoteXDAInstanceMeter WHERE RemoteXDAInstanceID = {0})", remoteXDAInstanceId);
            return  table.QueryRecordCount(restriction);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(MetersToDataPush), RecordOperation.QueryRecords)]
        public IEnumerable<MetersToDataPush> QueryMetersToDataPushs(int remoteXDAInstanceId, string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            TableOperations<MetersToDataPush> table = DataContext.Table<MetersToDataPush>();
            RecordRestriction restriction = table.GetSearchRestriction(filterString) + new RecordRestriction("ID IN (SELECT MetersToDataPushID FROM RemoteXDAInstanceMeter WHERE RemoteXDAInstanceID = {0})", remoteXDAInstanceId);
            return table.QueryRecords(sortField, ascending, page, pageSize, restriction);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(MetersToDataPush), RecordOperation.DeleteRecord)]
        public void DeleteMetersToDataPush(int id)
        {
            CascadeDelete("MetersToDataPush", $"ID = {id}");
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(MetersToDataPush), RecordOperation.CreateNewRecord)]
        public MetersToDataPush NewMetersToDataPush()
        {
            return new MetersToDataPush();
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(MetersToDataPush), RecordOperation.AddNewRecord)]
        public void AddNewMetersToDataPush(MetersToDataPush record)
        {
            if (record.Obsfucate)
                record.RemoteXDAAssetKey = Guid.NewGuid().ToString();
            else
                record.RemoteXDAAssetKey = record.LocalXDAAssetKey;
            record.Synced = false;

            DataContext.Table<MetersToDataPush>().AddNewRecord(record);
            int meterId = DataContext.Connection.ExecuteScalar<int>("SELECT @@IDENTITY");
            DataContext.Table<RemoteXDAInstanceMeter>().AddNewRecord(new RemoteXDAInstanceMeter(){ RemoteXDAInstanceID = record.RemoteXDAInstanceId, MetersToDataPushID = meterId});
        }

        [AuthorizeHubRole("Administrator, Owner")]
        [RecordOperation(typeof(MetersToDataPush), RecordOperation.UpdateRecord)]
        public void UpdateMetersToDataPush(MetersToDataPush record)
        {
            MetersToDataPush oldrecord = DataContext.Table<MetersToDataPush>().QueryRecordWhere("ID = {0}", record.ID);

            if (record.Obsfucate && !oldrecord.Obsfucate)
                record.RemoteXDAAssetKey = Guid.NewGuid().ToString();
            else if(!record.Obsfucate && oldrecord.Obsfucate)
                record.RemoteXDAAssetKey = record.LocalXDAAssetKey;

            DataContext.Table<MetersToDataPush>().UpdateRecord(record);
        }


        [AuthorizeHubRole("Administrator")]
        public IEnumerable<IDLabel> SearchMetersToDataPushs(string searchText, int limit = -1)
        {
            RecordRestriction restriction = new RecordRestriction("Name LIKE {0}", $"%{searchText}%");

            return DataContext.Table<Meter>().QueryRecords("Name", restriction, limit)
                .Select(meter => new IDLabel(meter.ID.ToString(), meter.AssetKey));
        }

        public Meter GetMeterRecord(int id)
        {
            return DataContext.Table<Meter>().QueryRecordWhere("ID = {0}",id);
        }

        [AuthorizeHubRole("Administrator")]
        public void SyncMeterConfigurationForInstance(int instanceId, int meterId)
        {
            string clientId = Context.ConnectionId;
            try
            {
                // for now, create new instance of DataPusherEngine.  Later have one running in XDA ServiceHost and tie to it to ensure multiple updates arent happening simultaneously
                DataPusherEngine engine = new DataPusherEngine();
                RemoteXDAInstance instance = DataContext.Table<RemoteXDAInstance>().QueryRecordWhere("ID = {0}", instanceId);
                UserAccount userAccount = DataContext.Table<UserAccount>().QueryRecordWhere("ID = {0}", instance.UserAccountID);
                engine.SyncMeterConfigurationForInstance(clientId, instance, meterId, userAccount);
                DataContext.Connection.ExecuteNonQuery("UPDATE MetersToDataPush SET Synced = 1 WHERE ID ={0}", meterId);
            }
            catch (Exception ex) {
                LogException(ex);
            }
        }

        [AuthorizeHubRole("Administrator")]
        public void SyncMeterFilesForInstance(int instanceId, int meterId)
        {
            string clientId = Context.ConnectionId;

            try
            {
                // for now, create new instance of DataPusherEngine.  Later have one running in XDA ServiceHost and tie to it to ensure multiple updates arent happening simultaneously
                DataPusherEngine engine = new DataPusherEngine();
                RemoteXDAInstance instance = DataContext.Table<RemoteXDAInstance>().QueryRecordWhere("ID = {0}", instanceId);
                engine.SyncMeterFilesForInstance( clientId, instance, meterId);
            }
            catch (Exception ex)
            {
                LogException(ex);
            }
        }

        #endregion

        #region [ RemoteXDAInstance Table Operations ]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(RemoteXDAInstance), RecordOperation.QueryRecordCount)]
        public int QueryRemoteXDAInstanceCount(string filterString)
        {
            return DataContext.Table<RemoteXDAInstance>().QueryRecordCount(filterString);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(RemoteXDAInstance), RecordOperation.QueryRecords)]
        public IEnumerable<RemoteXDAInstance> QueryRemoteXDAInstances(string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            return DataContext.Table<RemoteXDAInstance>().QueryRecords(sortField, ascending, page, pageSize, filterString);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(RemoteXDAInstance), RecordOperation.DeleteRecord)]
        public void DeleteRemoteXDAInstance(int id)
        {
            DataContext.Table<RemoteXDAInstance>().DeleteRecord(id);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(RemoteXDAInstance), RecordOperation.CreateNewRecord)]
        public RemoteXDAInstance NewRemoteXDAInstance()
        {
            return new RemoteXDAInstance();
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(RemoteXDAInstance), RecordOperation.AddNewRecord)]
        public void AddNewRemoteXDAInstance(RemoteXDAInstance record)
        {
            DataContext.Table<RemoteXDAInstance>().AddNewRecord(record);
        }

        [AuthorizeHubRole("Administrator, Owner")]
        [RecordOperation(typeof(RemoteXDAInstance), RecordOperation.UpdateRecord)]
        public void UpdateRemoteXDAInstance(RemoteXDAInstance record)
        {
            DataContext.Table<RemoteXDAInstance>().UpdateRecord(record);
        }

        [AuthorizeHubRole("Administrator")]
        public void SyncInstanceConfiguration(int instanceId)
        {
            // for now, create new instance of DataPusherEngine.  Later have one running in XDA ServiceHost and tie to it to ensure multiple updates arent happening simultaneously
            DataPusherEngine engine = new DataPusherEngine();
            engine.SyncInstanceConfiguration(Context.ConnectionId, instanceId);
        }

        [AuthorizeHubRole("Administrator")]
        public void SyncFilesForInstance(int instanceId)
        {
            // for now, create new instance of DataPusherEngine.  Later have one running in XDA ServiceHost and tie to it to ensure multiple updates arent happening simultaneously
            DataPusherEngine engine = new DataPusherEngine();
            RemoteXDAInstance instance = DataContext.Table<RemoteXDAInstance>().QueryRecordWhere("ID = {0}", instanceId);
            engine.SyncInstanceFiles(Context.ConnectionId, instance);
        }



        #endregion

        #endregion

        #region [OpenSEE Operations]
        public List<SignalCode.FlotSeries> GetFlotData(int eventID, List<int> seriesIndexes)
        {
            SignalCode sc = new SignalCode();
            return sc.GetFlotData(eventID, seriesIndexes);
        }
        #endregion

        #region [OpenSTE Operations]

        public TrendingDataSet GetTrendsForChannelIDDate(string ChannelID, string targetDate)
        {
            string historianServer = DataContext.Connection.ExecuteScalar<string>("SELECT Value FROM Setting WHERE Name = 'Historian.Server'") ?? "127.0.0.1";
            string historianInstance = DataContext.Connection.ExecuteScalar<string>("SELECT Value FROM Setting WHERE Name = 'Historian.Instance'") ?? "XDA";
            IEnumerable<int> channelIDs = new List<int>() { Convert.ToInt32(ChannelID) };
            DateTime startDate = Convert.ToDateTime(targetDate);
            DateTime endDate = startDate.AddDays(1);
            TrendingDataSet trendingDataSet = new TrendingDataSet();
            DateTime epoch = new DateTime(1970, 1, 1);

            using (Historian historian = new Historian(historianServer, historianInstance))
            {
                foreach (openHistorian.XDALink.TrendingDataPoint point in historian.Read(channelIDs, startDate, endDate))
                {
                    if (!trendingDataSet.ChannelData.Exists( x => x.Time == point.Timestamp.Subtract(epoch).TotalMilliseconds))
                    {
                        trendingDataSet.ChannelData.Add(new openXDA.Model.TrendingDataPoint());
                        trendingDataSet.ChannelData[trendingDataSet.ChannelData.Count - 1].Time = point.Timestamp.Subtract(epoch).TotalMilliseconds;
                    }

                    if (point.SeriesID.ToString() == "Average")
                        trendingDataSet.ChannelData[trendingDataSet.ChannelData.IndexOf(x => x.Time == point.Timestamp.Subtract(epoch).TotalMilliseconds)].Average = point.Value;
                    else if (point.SeriesID.ToString() == "Minimum")
                        trendingDataSet.ChannelData[trendingDataSet.ChannelData.IndexOf(x => x.Time == point.Timestamp.Subtract(epoch).TotalMilliseconds)].Minimum = point.Value;
                    else if (point.SeriesID.ToString() == "Maximum")
                        trendingDataSet.ChannelData[trendingDataSet.ChannelData.IndexOf(x => x.Time == point.Timestamp.Subtract(epoch).TotalMilliseconds)].Maximum = point.Value;

                }
            }
            IEnumerable<DataRow> table = Enumerable.Empty<DataRow>();

            table = DataContext.Connection.RetrieveData(" Select {0} AS thedatefrom, " +
                                                        "        DATEADD(DAY, 1, {0}) AS thedateto, " +
                                                        "        CASE WHEN AlarmRangeLimit.PerUnit <> 0 AND Channel.PerUnitValue IS NOT NULL THEN AlarmRangeLimit.High * PerUnitValue ELSE AlarmRangeLimit.High END AS alarmlimithigh," +
                                                        "        CASE WHEN AlarmRangeLimit.PerUnit <> 0 AND Channel.PerUnitValue IS NOT NULL THEN AlarmRangeLimit.Low * PerUnitValue ELSE AlarmRangeLimit.Low END AS alarmlimitlow " +
                                                        " FROM   AlarmRangeLimit JOIN " +
                                                        "        Channel ON AlarmRangeLimit.ChannelID = Channel.ID " +
                                                        "WHERE   AlarmRangeLimit.AlarmTypeID = (SELECT ID FROM AlarmType where Name = 'Alarm') AND " +
                                                        "        AlarmRangeLimit.ChannelID = {1}", startDate, Convert.ToInt32(ChannelID)).Select();

            foreach (DataRow row in table)
            {
                trendingDataSet.AlarmLimits.Add(new TrendingAlarmLimit() { High = row.Field<double?>("alarmlimithigh"), Low = row.Field<double?>("alarmlimitlow"), TimeEnd = row.Field<DateTime>("thedateto").Subtract(epoch).TotalMilliseconds, TimeStart = row.Field<DateTime>("thedatefrom").Subtract(epoch).TotalMilliseconds });
            }

            table = Enumerable.Empty<DataRow>();

            table = DataContext.Connection.RetrieveData(" DECLARE @dayOfWeek INT = DATEPART(DW, {0}) - 1 " +
                                                        " DECLARE @hourOfWeek INT = @dayOfWeek * 24 " +
                                                        " ; WITH HourlyIndex AS" +
                                                        " ( " +
                                                        "   SELECT @hourOfWeek AS HourOfWeek " +
                                                        "   UNION ALL " +
                                                        "   SELECT HourOfWeek + 1 " +
                                                        "   FROM HourlyIndex" +
                                                        "   WHERE (HourOfWeek + 1) < @hourOfWeek + 24" +
                                                        " ) " +
                                                        " SELECT " +
                                                        "        DATEADD(HOUR, HourlyIndex.HourOfWeek - @hourOfWeek, {0}) AS thedatefrom, " +
                                                        "        DATEADD(HOUR, HourlyIndex.HourOfWeek - @hourOfWeek + 1, {0}) AS thedateto, " +
                                                        "        HourOfWeekLimit.High AS offlimithigh, " +
                                                        "        HourOfWeekLimit.Low AS offlimitlow " +
                                                        " FROM " +
                                                        "        HourlyIndex LEFT OUTER JOIN " +
                                                        "        HourOfWeekLimit ON HourOfWeekLimit.HourOfWeek = HourlyIndex.HourOfWeek " +
                                                        " WHERE " +
                                                        "        HourOfWeekLimit.ChannelID IS NULL OR " +
                                                        "        HourOfWeekLimit.ChannelID = {1} ", startDate, Convert.ToInt32(ChannelID)).Select();

            foreach (DataRow row in table)
            {
                trendingDataSet.OffNormalLimits.Add(new TrendingAlarmLimit() { High = row.Field<double?>("offlimithigh"), Low = row.Field<double?>("offlimitlow"), TimeEnd = row.Field<DateTime>("thedateto").Subtract(epoch).TotalMilliseconds, TimeStart = row.Field<DateTime>("thedatefrom").Subtract(epoch).TotalMilliseconds });
            }

            return trendingDataSet;
        }

        #endregion

        #region [ Misc ]

        public IEnumerable<IDLabel> SearchTimeZones(string searchText , int limit)
        {
            IReadOnlyCollection<TimeZoneInfo> tzi = TimeZoneInfo.GetSystemTimeZones();

            return tzi
                .Select(row => new IDLabel(row.Id, row.ToString()))
                .Where(row => row.label.ToLower().Contains(searchText.ToLower()));
        }


        /// <summary>
        /// Gets UserAccount table ID for current user.
        /// </summary>
        /// <returns>UserAccount.ID for current user.</returns>
        public static Guid GetCurrentUserID()
        {
            Guid userID;
            AuthorizationCache.UserIDs.TryGetValue(Thread.CurrentPrincipal.Identity.Name, out userID);
            return userID;
        }

        /// <summary>
        /// Gets UserAccount table SID for current user.
        /// </summary>
        /// <returns>UserAccount.ID for current user.</returns>
        public static string GetCurrentUserSID()
        {
            return UserInfo.UserNameToSID(Thread.CurrentPrincipal.Identity.Name);
        }

        /// <summary>
        /// Gets UserAccount table name for current user.
        /// </summary>
        /// <returns>User name for current user.</returns>
        public static string GetCurrentUserName()
        {
            return Thread.CurrentPrincipal.Identity.Name;
        }

        public Tuple<DateTime, DateTime> GetTimeRange(int filterId)
        {
            string timeRange = DataContext.Connection.ExecuteScalar<string>("SELECT TimeRange FROM WorkbenchFilter WHERE ID ={0}", filterId);

            string[] timeRangeSplit = timeRange.Split(';');
            DateTime startDate;
            DateTime endDate;
            if (timeRangeSplit[0] == "-1")
            {
                startDate = DateTime.Parse(timeRangeSplit[1]);
                endDate = DateTime.Parse(timeRangeSplit[2]);
            }
            else if (timeRangeSplit[0] == "0") // 1 day time range
            {
                endDate = DateTime.UtcNow;
                startDate = endDate.AddDays(-1);
            }
            else if (timeRangeSplit[0] == "1") // 3 day time range
            {
                endDate = DateTime.UtcNow;
                startDate = endDate.AddDays(-3);
            }
            else if (timeRangeSplit[0] == "2") // 7 day time range
            {
                endDate = DateTime.UtcNow;
                startDate = endDate.AddDays(-7);
            }
            else if (timeRangeSplit[0] == "3") // 30 day time range
            {
                endDate = DateTime.UtcNow;
                startDate = endDate.AddDays(-30);
            }
            else if (timeRangeSplit[0] == "4") // 30 day time range
            {
                endDate = DateTime.UtcNow;
                startDate = endDate.AddDays(-90);
            }

            else if (timeRangeSplit[0] == "5") // 30 day time range
            {
                endDate = DateTime.UtcNow;
                startDate = endDate.AddDays(-365);
            }



            else // default to 2 weeks
            {
                endDate = DateTime.UtcNow;
                startDate = endDate.AddDays(-14);
            }

            return Tuple.Create(startDate, endDate);
        }

        #endregion
    }
}