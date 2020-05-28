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
//  07/23/2019 - Christoph Lackner
//       Added RelayAlertSettings to LineView.
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
using Asset = openXDA.Model.Asset;
using Meter = openXDA.Model.Meter;
using MeterDetail = openXDA.Model.MeterDetail;
using MeterAsset = openXDA.Model.MeterAsset;
using Location = openXDA.Model.Location;
using MeterAssetGroup = openXDA.Model.MeterAssetGroup;
using Setting = openXDA.Model.Setting;
using GSF.Security;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using System.Collections;

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

        public static event EventHandler<EventArgs<int, int, bool>> ReprocessFilesEvent;

        private static void OnReprocessFile(int fileGroupId, int meterId, bool loadHistoricConfiguration)
        {
            ReprocessFilesEvent?.Invoke(new object(), new EventArgs<int, int, bool>(fileGroupId, meterId, loadHistoricConfiguration));
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
                using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
                {
                    return new TableOperations<Setting>(connection).QueryRecordWhere("Name = 'LocalXDAInstance'").Value ?? "http://127.0.0.1:8989";
                }
            }
        }

        public static string RemoteXDAInstance
        {
            get
            {
                using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
                {
                    return new TableOperations<Setting>(connection).QueryRecordWhere("Name = 'RemoteXDAInstance'").Value ?? "http://127.0.0.1:8989";
                }
            }
        }

        public static string CompanyName
        {
            get
            {
                using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
                {
                    return new TableOperations<Setting>(connection).QueryRecordWhere("Name = 'CompanyName'").Value;
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

        #region [ Settings ]
        #region [ Setting Table Operations ]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Setting), RecordOperation.QueryRecordCount)]
        public int QuerySettingCount(string filterString)
        {
            using(AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                return new TableOperations<Setting>(connection).QueryRecordCount(filterString);
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Setting), RecordOperation.QueryRecords)]
        public IEnumerable<Setting> QuerySettings(string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                return new TableOperations<Setting>(connection).QueryRecords(sortField, ascending, page, pageSize, filterString).ToList();
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Setting), RecordOperation.DeleteRecord)]
        public void DeleteSetting(int id)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                new TableOperations<Setting>(connection).DeleteRecord(id);
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Setting), RecordOperation.CreateNewRecord)]
        public Setting NewSetting()
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                return new TableOperations<Setting>(connection).NewRecord();
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Setting), RecordOperation.AddNewRecord)]
        public void AddNewSetting(Setting record)
        {
            using(AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                new TableOperations<Setting>(connection).AddNewRecord(record);
            }
        }

        [AuthorizeHubRole("Administrator, Owner")]
        [RecordOperation(typeof(Setting), RecordOperation.UpdateRecord)]
        public void UpdateSetting(Setting record)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                new TableOperations<Setting>(connection).UpdateRecord(record);
            }
        }

        #endregion

        #region [ DashSettings Table Operations ]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(DashSettings), RecordOperation.QueryRecordCount)]
        public int QueryDashSettingsCount(string filterString)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                return new TableOperations<DashSettings>(connection).QueryRecordCount(filterString);
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(DashSettings), RecordOperation.QueryRecords)]
        public IEnumerable<DashSettings> QueryDashSettingss(string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                return new TableOperations<DashSettings>(connection).QueryRecords(sortField, ascending, page, pageSize, filterString).ToList();
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(DashSettings), RecordOperation.DeleteRecord)]
        public void DeleteDashSettings(int id)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                new TableOperations<DashSettings>(connection).DeleteRecord(id);
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(DashSettings), RecordOperation.CreateNewRecord)]
        public DashSettings NewDashSettings()
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                return new TableOperations<DashSettings>(connection).NewRecord();
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(DashSettings), RecordOperation.AddNewRecord)]
        public void AddNewDashSetting(DashSettings record)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                new TableOperations<DashSettings>(connection).AddNewRecord(record);
            }
        }

        [AuthorizeHubRole("Administrator, Owner")]
        [RecordOperation(typeof(DashSettings), RecordOperation.UpdateRecord)]
        public void UpdateDashSetting(DashSettings record)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                new TableOperations<DashSettings>(connection).UpdateRecord(record);
            }
        }

        #endregion

        #region [ UserDashSettings Table Operations ]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(UserDashSettings), RecordOperation.QueryRecordCount)]
        public int QueryUserDashSettingsCount(string filterString)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                return new TableOperations<UserDashSettings>(connection).QueryRecordCount(filterString);
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(UserDashSettings), RecordOperation.QueryRecords)]
        public IEnumerable<UserDashSettings> QueryUserDashSettings(string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                return new TableOperations<UserDashSettings>(connection).QueryRecords(sortField, ascending, page, pageSize, filterString).ToList();
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(UserDashSettings), RecordOperation.DeleteRecord)]
        public void DeleteUserDashSettings(int id)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                new TableOperations<UserDashSettings>(connection).DeleteRecord(id);
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(UserDashSettings), RecordOperation.CreateNewRecord)]
        public UserDashSettings NewUserDashSettings()
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                return new TableOperations<UserDashSettings>(connection).NewRecord();
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(UserDashSettings), RecordOperation.AddNewRecord)]
        public void AddNewUSerDashSetting(UserDashSettings record)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                new TableOperations<UserDashSettings>(connection).AddNewRecord(record);
            }
        }

        [AuthorizeHubRole("Administrator, Owner")]
        [RecordOperation(typeof(UserDashSettings), RecordOperation.UpdateRecord)]
        public void UpdateUserDashSetting(UserDashSettings record)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                new TableOperations<UserDashSettings>(connection).UpdateRecord(record);
            }
        }

        #endregion

        #region [ UserAccount Operations]
        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(UserAccount), RecordOperation.QueryRecordCount)]
        public int QueryProblematicUserAccountCount(string filterString)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                return new TableOperations<UserAccount>(connection).QueryRecords(filterString).Where(x => x.UseADAuthentication && x.AccountName == x.Name).Count();
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(UserAccount), RecordOperation.QueryRecords)]
        public IEnumerable<UserAccount> QueryProblematicUserAccounts(string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                if (ascending)
                return new TableOperations<UserAccount>(connection).QueryRecords(filterString).Where(x => x.UseADAuthentication && x.AccountName == x.Name).OrderBy(x => sortField).Skip((page - 1)* pageSize).Take(pageSize).ToList();
            else
                return new TableOperations<UserAccount>(connection).QueryRecords(filterString).Where(x => x.UseADAuthentication && x.AccountName == x.Name).OrderByDescending(x => sortField).Skip((page - 1) * pageSize).Take(pageSize).ToList();
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(UserAccount), RecordOperation.DeleteRecord)]
        public void DeleteProblematicUserAccount(Guid id)
        {
            CascadeDelete("UserAccount", $"ID = '{id}'");
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(UserAccount), RecordOperation.CreateNewRecord)]
        public UserAccount NewProblematicUserAccount()
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                return new TableOperations<UserAccount>(connection).NewRecord();
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(UserAccount), RecordOperation.AddNewRecord)]
        public void AddNewProblematicUserAccount(UserAccount record)
        {
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(UserAccount), RecordOperation.UpdateRecord)]
        public void UpdateProblematicUserAccount(UserAccount record)
        {
        }

        #endregion

        #region [ UsersEmailTemplates Operations]

        const string usersEmailTemplatesQuery = @"
            SELECT 
	            UserAccount.ID,
	            UserAccount.Name,
	            UserAccount.FirstName + ' ' + UserAccount.LastName as FullName,
	            (SELECT COUNT(*) FROM UserAccountAssetGroup WHERE UserAccountAssetGroup.UserAccountID = UserAccount.ID) as AssetGroupCount,
	            (SELECT COUNT(*) FROM UserAccountEmailType WHERE UserAccountEmailType.UserAccountID = UserAccount.ID) as EmailTypeCount
            FROM
	            UserAccount
        ";

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(UserEmailTemplate), RecordOperation.QueryRecordCount)]
        public int QueryUserEmailTemplateCount(string filterString)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                IEnumerable<UserEmailTemplate> records = connection.RetrieveData(usersEmailTemplatesQuery).Select().Select(row => new TableOperations<UserEmailTemplate>(connection).LoadRecord(row)).Where(x => x.AccountName.ToLower().IndexOf(filterString.ToLower()) >= 0 || (x.FullName ?? "").ToLower().IndexOf(filterString.ToLower()) >= 0);
                return records.Count();
            }
        }

        public DataTable QueryAssetGroupsByUser(Guid userAccountID)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                return connection.RetrieveData("SELECT * FROM UserAccountAssetGroup WHERE UserAccountAssetGroup.UserAccountID = {0}", userAccountID);
            }
        }

        public DataTable QueryEmailTypesByUser(Guid userAccountID)
                {
                    using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
                    {

                        return connection.RetrieveData("SELECT * FROM UserAccountEmailType WHERE UserAccountEmailType.UserAccountID = {0}", userAccountID);
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(UserEmailTemplate), RecordOperation.QueryRecords)]
        public IEnumerable<UserEmailTemplate> QueryUserEmailTemplate(string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                IEnumerable<UserEmailTemplate> records = connection.RetrieveData(usersEmailTemplatesQuery).Select().Select(row => new TableOperations<UserEmailTemplate>(connection).LoadRecord(row));
            if (ascending)
                return records.Where(x=> x.AccountName.ToLower().IndexOf(filterString.ToLower()) >= 0 || (x.FullName ?? "").ToLower().IndexOf(filterString.ToLower()) >= 0).OrderBy(x => sortField).Skip((page - 1) * pageSize).Take(pageSize);
            else
                return records.Where(x => x.AccountName.ToLower().IndexOf(filterString.ToLower()) >= 0 || (x.FullName ?? "").ToLower().IndexOf(filterString.ToLower()) >= 0).OrderByDescending(x => sortField).Skip((page - 1) * pageSize).Take(pageSize);
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(UserEmailTemplate), RecordOperation.DeleteRecord)]
        public void DeleteUserEmailTemplate(Guid id)
        {
            //CascadeDelete("UserAccount", $"ID = '{id}'");
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(UserEmailTemplate), RecordOperation.CreateNewRecord)]
        public UserEmailTemplate NewUserEmailTemplate()
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                return new TableOperations<UserEmailTemplate>(connection).NewRecord();
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(UserEmailTemplate), RecordOperation.AddNewRecord)]
        public void AddNewUserEmailTemplate(UserEmailTemplate record)
        {
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(UserEmailTemplate), RecordOperation.UpdateRecord)]
        public void UpdateUserEmailTemplate(UserEmailTemplate record)
        {
        }

        public void UpdateAssetGroupsForEmailTypes(List<string> assetGroups, Guid userAccountID)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                IEnumerable<UserAccountAssetGroup> records = new TableOperations<UserAccountAssetGroup>(connection).QueryRecordsWhere("UserAccountID = {0}", userAccountID);

                foreach (UserAccountAssetGroup record in records)
                {
                    if (!assetGroups.Contains(record.AssetGroupID.ToString()))
                        new TableOperations<UserAccountAssetGroup>(connection).DeleteRecord(record.ID);
                }

                foreach (string assetGroup in assetGroups)
                {
                    if (!records.Any(record => record.AssetGroupID == int.Parse(assetGroup)))
                        new TableOperations<UserAccountAssetGroup>(connection).AddNewRecord(new UserAccountAssetGroup() { AssetGroupID = int.Parse(assetGroup), UserAccountID = userAccountID });
                }
            }
        }

        public void UpdateEmailTypesForEmailTypes(List<string> emailTypes, Guid userAccountID)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                IEnumerable<UserAccountEmailType> records = new TableOperations<UserAccountEmailType>(connection).QueryRecordsWhere("UserAccountID = {0}", userAccountID);

                foreach (UserAccountEmailType record in records)
                {
                    if (!emailTypes.Contains(record.EmailTypeID.ToString()))
                        new TableOperations<UserAccountEmailType>(connection).DeleteRecord(record.ID);
                }

                foreach (string emailType in emailTypes)
                {
                    if (!records.Any(record => record.EmailTypeID == int.Parse(emailType)))
                        new TableOperations<UserAccountEmailType>(connection).AddNewRecord(new UserAccountEmailType() { EmailTypeID = int.Parse(emailType), UserAccountID = userAccountID });
                }
            }
        }


        #endregion

        #region [ EmailTemplatesUsers Operations]

        const string EmailTemplatesUsersQuery = @"
            SELECT 
	            EmailType.ID as EmailTemplateID,
	            XSLTemplate.Name as Template,
	            (SELECT COUNT(*) FROM UserAccountEmailType WHERE UserAccountEmailType.EmailTypeID = EmailType.ID) as UserCount
 
            FROM 
	            EmailType JOIN
	            EmailCategory ON EmailType.EmailCategoryID = EmailCategory.ID JOIN
	            XSLTemplate ON EmailType.XSLTemplateID = XSLTemplate.ID
        ";

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(EmailTemplateUser), RecordOperation.QueryRecordCount)]
        public int QueryEmailTemplatesUsersCount(string filterString)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {


                IEnumerable<EmailTemplateUser> records = connection.RetrieveData(EmailTemplatesUsersQuery).Select().Select(row => new TableOperations<EmailTemplateUser>(connection).LoadRecord(row)).Where(x => x.Template.ToLower().IndexOf(filterString.ToLower()) >= 0);

                return records.Count();
            }
        }

        public DataTable QueryUsersByEmailType(int emailTypeID)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {


                return connection.RetrieveData("SELECT * FROM UserAccountEmailType WHERE UserAccountEmailType.EmailTypeID = {0}", emailTypeID);
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(EmailTemplateUser), RecordOperation.QueryRecords)]
        public IEnumerable<EmailTemplateUser> QueryEmailTemplatesUsers(string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                IEnumerable<EmailTemplateUser> records = connection.RetrieveData(EmailTemplatesUsersQuery).Select().Select(row => new TableOperations<EmailTemplateUser>(connection).LoadRecord(row));
                if (ascending)
                    return records.Where(x => x.Template.ToLower().IndexOf(filterString.ToLower()) >= 0).OrderBy(x => sortField).Skip((page - 1) * pageSize).Take(pageSize);
                else
                    return records.Where(x => x.Template.ToLower().IndexOf(filterString.ToLower()) >= 0).OrderByDescending(x => sortField).Skip((page - 1) * pageSize).Take(pageSize);
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(EmailTemplateUser), RecordOperation.DeleteRecord)]
        public void DeleteEmailTemplatesUsers(Guid id)
        {
            //CascadeDelete("UserAccount", $"ID = '{id}'");
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(EmailTemplateUser), RecordOperation.CreateNewRecord)]
        public EmailTemplateUser NewUEmailTemplatesUsers()
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                return new TableOperations<EmailTemplateUser>(connection).NewRecord();
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(EmailTemplateUser), RecordOperation.AddNewRecord)]
        public void AddNewEmailTemplatesUsers(EmailTemplateUser record)
        {
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(EmailTemplateUser), RecordOperation.UpdateRecord)]
        public void UpdateEmailTemplatesUsers(EmailTemplateUser record)
        {
        }

        public void UpdateUsersForEmailTypes(List<string> users, int emailTypeID)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {


                IEnumerable<UserAccountEmailType> records = new TableOperations<UserAccountEmailType>(connection).QueryRecordsWhere("EmailTypeID = {0}", emailTypeID);

                foreach (UserAccountEmailType record in records)
                {
                    if (!users.Contains(record.UserAccountID.ToString()))
                        new TableOperations<UserAccountEmailType>(connection).DeleteRecord(record.ID);
                }

                foreach (string assetGroup in users)
                {
                    if (!records.Any(record => record.UserAccountID == Guid.Parse(assetGroup)))
                        new TableOperations<UserAccountEmailType>(connection).AddNewRecord(new UserAccountEmailType() { UserAccountID = Guid.Parse(assetGroup), EmailTypeID = emailTypeID });
                }
            }
        }

        #endregion

        #region [ AssetGroup Table Operations ]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(AssetGroup), RecordOperation.QueryRecordCount)]
        public int QueryGroupCount(int assetGroupID, string filterString)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                TableOperations<AssetGroup> tableOperations = new TableOperations<AssetGroup>(connection);
                RecordRestriction restriction;

                if (assetGroupID > 0)
                    restriction = tableOperations.GetSearchRestriction(filterString) + new RecordRestriction("ID = {0}", assetGroupID);
                else
                    restriction = tableOperations.GetSearchRestriction(filterString);

                return tableOperations.QueryRecordCount(restriction);
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(AssetGroup), RecordOperation.QueryRecords)]
        public IEnumerable<AssetGroupView> QueryGroups(int assetGroupID, string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                TableOperations<AssetGroupView> tableOperations = new TableOperations<AssetGroupView>(connection);
                RecordRestriction restriction;

                if (assetGroupID > 0)
                    restriction = tableOperations.GetSearchRestriction(filterString) + new RecordRestriction("ID = {0}", assetGroupID);
                else
                    restriction = tableOperations.GetSearchRestriction(filterString);

                return tableOperations.QueryRecords(sortField, ascending, page, pageSize, restriction).ToList();
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(AssetGroup), RecordOperation.DeleteRecord)]
        public void DeleteGroup(int id)
        {
            CascadeDelete("AssetGroup", $"ID={id}");
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(AssetGroup), RecordOperation.CreateNewRecord)]
        public AssetGroup NewGroup()
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                return new TableOperations<AssetGroup>(connection).NewRecord();
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(AssetGroup), RecordOperation.AddNewRecord)]
        public void AddNewGroup(AssetGroup record)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                new TableOperations<AssetGroup>(connection).AddNewRecord(record);
            }
        }

        [AuthorizeHubRole("Administrator, Owner")]
        [RecordOperation(typeof(AssetGroup), RecordOperation.UpdateRecord)]
        public void UpdateGroup(AssetGroup record)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                new TableOperations<AssetGroup>(connection).UpdateRecord(record);
            }
        }

        public int GetLastAssetGroupID()
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                return connection.ExecuteScalar<int?>("SELECT IDENT_CURRENT('AssetGroup')") ?? 0;
            }
        }

        public void UpdateAssetGroups(List<string> assetGroups, int groupID)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                IEnumerable<AssetGroupAssetGroup> records = new TableOperations<AssetGroupAssetGroup>(connection).QueryRecords(restriction: new RecordRestriction("ParentAssetGroupID = {0}", groupID)).ToList();
           

                foreach (AssetGroupAssetGroup record in records)
                {
                    if (!assetGroups.Contains(record.ChildAssetGroupID.ToString()))
                        new TableOperations<AssetGroupAssetGroup>(connection).DeleteRecord(record.ID);
                }

                foreach (string assetGroup in assetGroups)
                {
                    if (!records.Any(record => record.ChildAssetGroupID == int.Parse(assetGroup)))
                        new TableOperations<AssetGroupAssetGroup>(connection).AddNewRecord(new AssetGroupAssetGroup() { ParentAssetGroupID = groupID, ChildAssetGroupID = int.Parse(assetGroup) });
                }
            }
        }
    

    public void UpdateMeters(List<string> meters, int groupID)
    {
        using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
        {

            IEnumerable<MeterAssetGroup> records = new TableOperations<MeterAssetGroup>(connection).QueryRecords(restriction: new RecordRestriction("AssetGroupID = {0}", groupID)).ToList();

            foreach (MeterAssetGroup record in records)
            {
                if (!meters.Contains(record.MeterID.ToString()))
                        new TableOperations<MeterAssetGroup>(connection).DeleteRecord(record.ID);
            }

            foreach (string meter in meters)
            {
                if (!records.Any(record => record.MeterID == int.Parse(meter)))
                        new TableOperations<MeterAssetGroup>(connection).AddNewRecord(new MeterAssetGroup() { AssetGroupID = groupID, MeterID = int.Parse(meter) });
            }
        }
    }

    public void UpdateLines(List<string> lines, int groupID)
    {
        using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
        {

            IEnumerable<LineAssetGroup> records = new TableOperations<LineAssetGroup>(connection).QueryRecords(restriction: new RecordRestriction("AssetGroupID = {0}", groupID)).ToList();

            foreach (LineAssetGroup record in records)
            {
                if (!lines.Contains(record.LineID.ToString()))
                        new TableOperations<LineAssetGroup>(connection).DeleteRecord(record.ID);
            }

            foreach (string line in lines)
            {
                if (!records.Any(record => record.LineID == int.Parse(line)))
                        new TableOperations<LineAssetGroup>(connection).AddNewRecord(new LineAssetGroup() { AssetGroupID = groupID, LineID = int.Parse(line) });
            }
        }
    }

    public void UpdateUsers(List<string> users, int groupID)
    {
        using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
        {

            IEnumerable<UserAccountAssetGroup> records = new TableOperations<UserAccountAssetGroup>(connection).QueryRecords(restriction: new RecordRestriction("AssetGroupID = {0}", groupID)).ToList();

            foreach (UserAccountAssetGroup record in records)
            {
                if (!users.Contains(record.UserAccountID.ToString()))
                        new TableOperations<UserAccountAssetGroup>(connection).DeleteRecord(record.ID);
            }

            foreach (string user in users)
            {
                if (!records.Any(record => record.UserAccountID == Guid.Parse(user)))
                        new TableOperations<UserAccountAssetGroup>(connection).AddNewRecord(new UserAccountAssetGroup() { AssetGroupID = groupID, UserAccountID = Guid.Parse(user) });
            }
        }
    }

        #endregion

        #region [ AssetGroupAssetGroup Table Operations ]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(AssetGroupAssetGroup), RecordOperation.QueryRecordCount)]
        public int QueryGroupAssetGroupAssetGroupCount(int groupID, string filterString)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                TableOperations<AssetGroupAssetGroupView> tableOperations = new TableOperations<AssetGroupAssetGroupView>(connection);
                RecordRestriction restriction;

                if (groupID > 0)
                    restriction = tableOperations.GetSearchRestriction(filterString) + new RecordRestriction("ParentAssetGroupID = {0}", groupID);
                else
                    restriction = tableOperations.GetSearchRestriction(filterString);

                return tableOperations.QueryRecordCount(restriction);
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(AssetGroupAssetGroup), RecordOperation.QueryRecords)]
        public IEnumerable<AssetGroupAssetGroupView> QueryAssetGroupAssetGroupViews(int groupID, string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                TableOperations<AssetGroupAssetGroupView> tableOperations = new TableOperations<AssetGroupAssetGroupView>(connection);
                RecordRestriction restriction;

                if (groupID > 0)
                    restriction = tableOperations.GetSearchRestriction(filterString) + new RecordRestriction("ParentAssetGroupID = {0}", groupID);
                else
                    restriction = tableOperations.GetSearchRestriction(filterString);

                return tableOperations.QueryRecords(sortField, ascending, page, pageSize, restriction).ToList();
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(AssetGroupAssetGroup), RecordOperation.DeleteRecord)]
        public void DeleteAssetGroupAssetGroupView(int id)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                new TableOperations<AssetGroupAssetGroup>(connection).DeleteRecord(id);
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(AssetGroupAssetGroup), RecordOperation.CreateNewRecord)]
        public AssetGroupAssetGroupView NewAssetGroupAssetGroupView()
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                return new TableOperations<AssetGroupAssetGroupView>(connection).NewRecord();
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(AssetGroupAssetGroup), RecordOperation.AddNewRecord)]
        public void AddNewAssetGroupAssetGroupView(AssetGroupAssetGroup record)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                new TableOperations<AssetGroupAssetGroup>(connection).AddNewRecord(record);
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(AssetGroupAssetGroup), RecordOperation.UpdateRecord)]
        public void UpdateAssetGroupAssetGroupView(AssetGroupAssetGroup record)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                new TableOperations<AssetGroupAssetGroup>(connection).UpdateRecord(record);
            }
        }

        [AuthorizeHubRole("Administrator")]
        public IEnumerable<IDLabel> SearchAssetGroupAssetGroupByGroup(int groupID, string searchText, int limit = -1)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                RecordRestriction restriction = new RecordRestriction("Name LIKE {0} AND ID NOT IN (SELECT MeterID FROM AssetGroupAssetGroup WHERE AssetGroupID = {1})", $"%{searchText}%", groupID);

                return new TableOperations<Meter>(connection).QueryRecords("Name", restriction, limit)
                    .Select(meter => new IDLabel(meter.ID.ToString(), meter.Name)).ToList();
            }
        }


        #endregion

        #region [ MeterAssetGroup Table Operations ]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(MeterAssetGroup), RecordOperation.QueryRecordCount)]
        public int QueryGroupMeterViewCount(int groupID, string filterString)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                TableOperations<MeterAssetGroupView> tableOperations = new TableOperations<MeterAssetGroupView>(connection);
                RecordRestriction restriction;

                if (groupID > 0)
                    restriction = tableOperations.GetSearchRestriction(filterString) + new RecordRestriction("AssetGroupID = {0}", groupID);
                else
                    restriction = tableOperations.GetSearchRestriction(filterString);

                return tableOperations.QueryRecordCount(restriction);
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(MeterAssetGroup), RecordOperation.QueryRecords)]
        public IEnumerable<MeterAssetGroupView> QueryGroupMeterViews(int groupID, string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                TableOperations<MeterAssetGroupView> tableOperations = new TableOperations<MeterAssetGroupView>(connection);
                RecordRestriction restriction;

                if (groupID > 0)
                    restriction = tableOperations.GetSearchRestriction(filterString) + new RecordRestriction("AssetGroupID = {0}", groupID);
                else
                    restriction = tableOperations.GetSearchRestriction(filterString);

                return tableOperations.QueryRecords(sortField, ascending, page, pageSize, restriction).ToList();
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(MeterAssetGroup), RecordOperation.DeleteRecord)]
        public void DeleteGroupMeterView(int id)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                new TableOperations<MeterAssetGroup>(connection).DeleteRecord(id);
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(MeterAssetGroup), RecordOperation.CreateNewRecord)]
        public MeterAssetGroupView NewGroupMeterView()
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                return new TableOperations<MeterAssetGroupView>(connection).NewRecord();
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(MeterAssetGroup), RecordOperation.AddNewRecord)]
        public void AddNewGroupMeterView(MeterAssetGroup record)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                new TableOperations<MeterAssetGroup>(connection).AddNewRecord(record);
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(MeterAssetGroup), RecordOperation.UpdateRecord)]
        public void UpdateGroupMeterView(MeterAssetGroup record)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                new TableOperations<MeterAssetGroup>(connection).UpdateRecord(record);
            }
        }

        [AuthorizeHubRole("Administrator")]
        public IEnumerable<IDLabel> SearchMeterAssetGroupByGroup(int groupID, string searchText, int limit = -1)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                RecordRestriction restriction = new RecordRestriction("Name LIKE {0} AND ID NOT IN (SELECT MeterID FROM MeterAssetGroup WHERE AssetGroupID = {1})", $"%{searchText}%", groupID);

                return new TableOperations<Meter>(connection).QueryRecords("Name", restriction, limit)
                    .Select(meter => new IDLabel(meter.ID.ToString(), meter.Name)).ToList();
            }
        }


        #endregion

        #region [ LineAssetGroup Table Operations ]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(LineAssetGroup), RecordOperation.QueryRecordCount)]
        public int QueryLineAssetGroupViewCount(int groupID, string filterString)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                TableOperations<LineAssetGroupView> tableOperations = new TableOperations<LineAssetGroupView>(connection);
                RecordRestriction restriction;

                if (groupID > 0)
                    restriction = tableOperations.GetSearchRestriction(filterString) + new RecordRestriction("AssetGroupID = {0}", groupID);
                else
                    restriction = tableOperations.GetSearchRestriction(filterString);

                return tableOperations.QueryRecordCount(restriction);
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(LineAssetGroup), RecordOperation.QueryRecords)]
        public IEnumerable<LineAssetGroupView> QueryLineAssetGroupViews(int groupID, string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                TableOperations<LineAssetGroupView> tableOperations = new TableOperations<LineAssetGroupView>(connection);
                RecordRestriction restriction;

                if (groupID > 0)
                    restriction = tableOperations.GetSearchRestriction(filterString) + new RecordRestriction("AssetGroupID = {0}", groupID);
                else
                    restriction = tableOperations.GetSearchRestriction(filterString);

                return tableOperations.QueryRecords(sortField, ascending, page, pageSize, restriction).ToList();
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(LineAssetGroup), RecordOperation.DeleteRecord)]
        public void DeleteLineAssetGroupView(int id)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                new TableOperations<LineAssetGroup>(connection).DeleteRecord(id);
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(LineAssetGroup), RecordOperation.CreateNewRecord)]
        public LineAssetGroupView NewLineAssetGroupView()
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                return new TableOperations<LineAssetGroupView>(connection).NewRecord();
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(LineAssetGroup), RecordOperation.AddNewRecord)]
        public void AddNewLineAssetGroupView(LineAssetGroup record)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                new TableOperations<LineAssetGroup>(connection).AddNewRecord(record);
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(LineAssetGroup), RecordOperation.UpdateRecord)]
        public void UpdateLineAssetGroupView(LineAssetGroup record)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                new TableOperations<LineAssetGroup>(connection).UpdateRecord(record);
            }
        }

        public IEnumerable<IDLabel> SearchLinesForLineAssetGroupByGroup(int groupID, string searchText, int limit = -1)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                RecordRestriction restriction = new RecordRestriction("AssetKey LIKE {0} AND ID NOT IN (SELECT LineID FROM LineAssetGroup WHERE AssetGroupID = {1})", $"%{searchText}%", groupID);

                return new TableOperations<Line>(connection).QueryRecords("AssetKey", restriction, limit)
                    .Select(line => new IDLabel(line.ID.ToString(), line.AssetKey)).ToList();
            }
        }

        #endregion

        #region [ UserAccount Table Operations ]

        /// <summary>
        /// Queries count of user accounts.
        /// </summary>
        /// <param name="filterText">Text to use for filtering.</param>
        /// <returns>Count of user accounts.</returns>
        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(ConfirmableUserAccount), RecordOperation.QueryRecordCount)]
        public int QueryUserAccountCount(string filterText)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                bool matchesFilter(ConfirmableUserAccount userAccount)
                {
                    string firstName = userAccount.FirstName.ToNonNullString().ToLower();
                    string lastName = userAccount.LastName.ToNonNullString().ToLower();
                    string userName = userAccount.AccountName.ToNonNullString().ToLower();
                    string email = userAccount.Email.ToNonNullString().ToLower();
                    string phone = userAccount.Phone.ToNonNullString().ToLower();
                    string lowFilterText = filterText.ToNonNullString().ToLower();

                    return
                        firstName.Contains(lowFilterText) ||
                        lastName.Contains(lowFilterText) ||
                        userName.Contains(lowFilterText) ||
                        email.Contains(lowFilterText) ||
                        phone.Contains(lowFilterText);
                }

                return new TableOperations<ConfirmableUserAccount>(connection).QueryRecords()
                    .Where(matchesFilter)
                    .Count();
            }
        }

        /// <summary>
        /// Queries page of user accounts.
        /// </summary>
        /// <param name="sortField">Current sort field.</param>
        /// <param name="ascending">Current sort direction.</param>
        /// <param name="page">Current page number.</param>
        /// <param name="pageSize">Current page size.</param>
        /// <param name="filterText">Text to use for filtering.</param>
        /// <returns>Page of user accounts.</returns>
        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(ConfirmableUserAccount), RecordOperation.QueryRecords)]
        public IEnumerable<ConfirmableUserAccount> QueryUserAccounts(string sortField, bool ascending, int page, int pageSize, string filterText)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                bool matchesFilter(ConfirmableUserAccount userAccount)
                {
                    string firstName = userAccount.FirstName.ToNonNullString().ToLower();
                    string lastName = userAccount.LastName.ToNonNullString().ToLower();
                    string userName = userAccount.AccountName.ToNonNullString().ToLower();
                    string email = userAccount.Email.ToNonNullString().ToLower();
                    string phone = userAccount.Phone.ToNonNullString().ToLower();
                    string lowFilterText = filterText.ToNonNullString().ToLower();

                    return
                        firstName.Contains(lowFilterText) ||
                        lastName.Contains(lowFilterText) ||
                        userName.Contains(lowFilterText) ||
                        email.Contains(lowFilterText) ||
                        phone.Contains(lowFilterText);
                }

                Func<ConfirmableUserAccount, IComparable> orderByFunc;

                switch (sortField)
                {
                    case "AccountName":
                        orderByFunc = (userAccount => userAccount.AccountName);
                        break;

                    case "Email":
                        orderByFunc = (userAccount => userAccount.Email);
                        break;

                    case "Phone":
                        orderByFunc = (userAccount => userAccount.Phone);
                        break;

                    case "UseADAuthentication":
                        orderByFunc = (userAccount => userAccount.UseADAuthentication);
                        break;

                    case "Approved":
                        orderByFunc = (userAccount => userAccount.Approved);
                        break;

                    default:
                    case "FullName":
                        orderByFunc = (userAccount => string.Concat(userAccount.LastName, " ", userAccount.FirstName));
                        break;
                }

                IEnumerable<ConfirmableUserAccount> accounts = new TableOperations<ConfirmableUserAccount>(connection).QueryRecords().ToList();
                accounts = accounts.Where(matchesFilter);
                accounts = ascending ? accounts.OrderBy(orderByFunc) : accounts.OrderByDescending(orderByFunc);

                int skipCount = (page - 1) * pageSize;
                accounts = accounts.Skip(skipCount);
                accounts = accounts.Take(pageSize);

                return accounts;
            }
        }

        /// <summary>
        /// Deletes user account.
        /// </summary>
        /// <param name="id">Unique ID of user account.</param>
        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(ConfirmableUserAccount), RecordOperation.DeleteRecord)]
        public void DeleteUserAccount(Guid id)
        {
            CascadeDelete("UserAccount", $"ID = '{id}'");
        }

        /// <summary>
        /// Creates a new user account model instance.
        /// </summary>
        /// <returns>A new user account model instance.</returns>
        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(ConfirmableUserAccount), RecordOperation.CreateNewRecord)]
        public ConfirmableUserAccount NewUserAccount()
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                return new TableOperations<ConfirmableUserAccount>(connection).NewRecord();
            }
        }

        /// <summary>
        /// Adds a new user account.
        /// </summary>
        /// <param name="record">User account model instance to add.</param>
        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(ConfirmableUserAccount), RecordOperation.AddNewRecord)]
        public void AddNewUserAccount(ConfirmableUserAccount record)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                if (!record.UseADAuthentication && !string.IsNullOrWhiteSpace(record.Password))
                    record.Password = SecurityProviderUtility.EncryptPassword(record.Password);

                new TableOperations<ConfirmableUserAccount>(connection).AddNewRecord(record);
            }
        }

        /// <summary>
        /// Updates user account.
        /// </summary>
        /// <param name="record">User account model instance to update.</param>
        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(ConfirmableUserAccount), RecordOperation.UpdateRecord)]
        public void UpdateUserAccount(ConfirmableUserAccount record)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                if (!record.UseADAuthentication && !string.IsNullOrWhiteSpace(record.Password))
                    record.Password = SecurityProviderUtility.EncryptPassword(record.Password);

                new TableOperations<ConfirmableUserAccount>(connection).UpdateRecord(record);
            }
        }

        #endregion

        #region [ UserAccountAssetGroup Table Operations ]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(UserAccountAssetGroup), RecordOperation.QueryRecordCount)]
        public int QueryUserAccountAssetGroupCount(int groupID, string filterString)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                if (filterString != null && filterString != string.Empty)
                    return new TableOperations<UserAccountAssetGroupView>(connection).QueryRecordsWhere("AssetGroupID = {0}", groupID).Where(x => UserInfo.SIDToAccountName(x.Username).ToLower().Contains(filterString.ToLower())).Count();
                else
                    return new TableOperations<UserAccountAssetGroupView>(connection).QueryRecordsWhere("AssetGroupID = {0}", groupID).Count();
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(UserAccountAssetGroup), RecordOperation.QueryRecords)]
        public IEnumerable<UserAccountAssetGroupView> QueryUserAccountAssetGroups(int groupID, string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                Func<UserAccountAssetGroupView, bool> whereFunc;

                if (filterString != null && filterString != string.Empty)
                    whereFunc = x => UserInfo.SIDToAccountName(x.Username).ToLower().Contains(filterString.ToLower());
                else
                    whereFunc = x => true;

                if (ascending)
                    return new TableOperations<UserAccountAssetGroupView>(connection).QueryRecords(sortField, ascending, page, pageSize, new RecordRestriction("AssetGroupID = {0}", groupID))
                        .Where(whereFunc).OrderBy(x => UserInfo.SIDToAccountName(x.Username), StringComparer.OrdinalIgnoreCase).ToList();
                else
                    return new TableOperations<UserAccountAssetGroupView>(connection).QueryRecords(sortField, ascending, page, pageSize, new RecordRestriction("AssetGroupID = {0}", groupID))
                        .Where(whereFunc).OrderByDescending(x => UserInfo.SIDToAccountName(x.Username), StringComparer.OrdinalIgnoreCase).ToList();
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(UserAccountAssetGroup), RecordOperation.DeleteRecord)]
        public void DeleteUserAccountAssetGroup(int id)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                new TableOperations<UserAccountAssetGroup>(connection).DeleteRecord(id);
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(UserAccountAssetGroup), RecordOperation.CreateNewRecord)]
        public UserAccountAssetGroupView NewUserAccountAssetGroup()
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                return new TableOperations<UserAccountAssetGroupView>(connection).NewRecord();
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(UserAccountAssetGroup), RecordOperation.AddNewRecord)]
        public void AddNewUserAccountAssetGroup(UserAccountAssetGroup record)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                new TableOperations<UserAccountAssetGroup>(connection).AddNewRecord(record);
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(UserAccountAssetGroup), RecordOperation.UpdateRecord)]
        public void UpdateUserAccountAssetGroup(UserAccountAssetGroup record)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                new TableOperations<UserAccountAssetGroup>(connection).UpdateRecord(record);
            }
        }

        [AuthorizeHubRole("Administrator")]
        public IEnumerable<IDLabel> SearchUsersByGroup(int groupID, string searchText, int limit = -1)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                RecordRestriction restriction = new RecordRestriction("ID NOT IN (SELECT UserAccountID FROM UserAccountAssetGroup WHERE AssetGroupID = {0})", groupID);
                if (limit < 1)
                    return new TableOperations<UserAccount>(connection)
                        .QueryRecords(restriction: restriction)
                        .Select(record =>
                        {
                            record.Name = UserInfo.SIDToAccountName(record.Name ?? "");
                            return record;
                        })
                        .Where(record => record.Name?.ToLower().Contains(searchText.ToLower()) ?? false)
                        .Select(record => IDLabel.Create(record.ID.ToString(), record.Name));

                return new TableOperations<UserAccount>(connection)
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
        }

        #endregion

        #region [ User Table Operations ]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(User), RecordOperation.QueryRecordCount)]
        public int QueryUserCount(string filterString)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                return new TableOperations<User>(connection).QueryRecordCount(filterString);
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(User), RecordOperation.QueryRecords)]
        public IEnumerable<User> QueryUsers(string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                return new TableOperations<User>(connection).QueryRecords(sortField, ascending, page, pageSize, filterString).ToList();
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(User), RecordOperation.DeleteRecord)]
        public void DeleteUser(int id)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                new TableOperations<User>(connection).DeleteRecord(id);
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(User), RecordOperation.CreateNewRecord)]
        public User NewUser()
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                return new TableOperations<User>(connection).NewRecord();
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(User), RecordOperation.AddNewRecord)]
        public void AddNewUser(User record)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                new TableOperations<User>(connection).AddNewRecord(record);
            }
        }

        [AuthorizeHubRole("Administrator, Owner")]
        [RecordOperation(typeof(User), RecordOperation.UpdateRecord)]
        public void UpdateUser(User record)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                new TableOperations<User>(connection).UpdateRecord(record);
            }
        }

        public IEnumerable<User> GetUsers()
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                return new TableOperations<User>(connection).QueryRecords(restriction: new RecordRestriction("Active <> 0")).ToList();
            }
        }
        #endregion

        #endregion

        #region [ Alarms ]
        #region [ AlarmRangeLimitView Table Operations ]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(AlarmRangeLimitView), RecordOperation.QueryRecordCount)]
        public int QueryAlarmRangeLimitViewCount(int meterID, int lineID, string filterString)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
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

                return new TableOperations<AlarmRangeLimitView>(connection).QueryRecordCount(new RecordRestriction("MeterName Like {0} AND Name LIKE {1} AND MeasurementType LIKE {2} AND MeasurementCharacteristic LIKE {3}" + (MeterID == "%" ? "" : " AND MeterID = {4} AND LineID = {5}"), meterFilter, channelFilter, typeFilter, charFilter, MeterID, LineID));
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(AlarmRangeLimitView), RecordOperation.QueryRecords)]
        public IEnumerable<AlarmRangeLimitView> QueryAlarmRangeLimitViews(int meterID, int lineID, string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
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


                return new TableOperations<AlarmRangeLimitView>(connection).QueryRecords(sortField, ascending, page, pageSize, new RecordRestriction("MeterName Like {0} AND Name LIKE {1} AND MeasurementType LIKE {2} AND MeasurementCharacteristic LIKE {3}" + (MeterID == "%" ? "" : " AND MeterID = {4} AND LineID = {5}"), meterFilter, channelFilter, typeFilter, charFilter, MeterID, LineID)).ToList();
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(AlarmRangeLimitView), RecordOperation.DeleteRecord)]
        public void DeleteAlarmRangeLimitView(int id)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                new TableOperations<AlarmRangeLimit>(connection).DeleteRecord(id);
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(AlarmRangeLimitView), RecordOperation.CreateNewRecord)]
        public AlarmRangeLimitView NewAlarmRangeLimitView()
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                return new TableOperations<AlarmRangeLimitView>(connection).NewRecord();
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(AlarmRangeLimitView), RecordOperation.AddNewRecord)]
        public void AddNewAlarmRangeLimitView(AlarmRangeLimitView record)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                if (record.IsDefault)
                {
                    IEnumerable<DefaultAlarmRangeLimit> defaultLimits = new TableOperations<DefaultAlarmRangeLimit>(connection).QueryRecords(restriction: new RecordRestriction("MeasurementTypeID = {0} AND MeasurementCharacteristicID = {1}", record.MeasurementTypeID, record.MeasurementCharacteristicID)).ToList();

                    if (defaultLimits.Any())
                    {
                        record.Severity = defaultLimits.First().Severity;
                        record.High = defaultLimits.First().High;
                        record.Low = defaultLimits.First().Low;
                        record.RangeInclusive = defaultLimits.First().RangeInclusive;
                        record.PerUnit = defaultLimits.First().PerUnit;
                    }
                }

                new TableOperations<AlarmRangeLimit>(connection).AddNewRecord(CreateNewAlarmRangeLimit(record));
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(AlarmRangeLimitView), RecordOperation.UpdateRecord)]
        public void UpdateAlarmRangeLimitView(AlarmRangeLimitView record)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                if (record.IsDefault)
                {
                    IEnumerable<DefaultAlarmRangeLimit> defaultLimits = new TableOperations<DefaultAlarmRangeLimit>(connection).QueryRecords(restriction: new RecordRestriction("MeasurementTypeID = {0} AND MeasurementCharacteristicID = {1}", record.MeasurementTypeID, record.MeasurementCharacteristicID)).ToList();

                    if (defaultLimits.Any())
                    {
                        record.Severity = defaultLimits.First().Severity;
                        record.High = defaultLimits.First().High;
                        record.Low = defaultLimits.First().Low;
                        record.RangeInclusive = defaultLimits.First().RangeInclusive;
                        record.PerUnit = defaultLimits.First().PerUnit;
                    }
                }

                new TableOperations<AlarmRangeLimit>(connection).UpdateRecord(CreateNewAlarmRangeLimit(record));
            }
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
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                IEnumerable<DefaultAlarmRangeLimit> defaultLimits = new TableOperations<DefaultAlarmRangeLimit>(connection).QueryRecords(restriction: new RecordRestriction("MeasurementTypeID = {0} AND MeasurementCharacteristicID = {1}", record.MeasurementTypeID, record.MeasurementCharacteristicID)).ToList();

                if (defaultLimits.Any())
                {
                    record.Severity = defaultLimits.First().Severity;
                    record.High = defaultLimits.First().High;
                    record.Low = defaultLimits.First().Low;
                    record.RangeInclusive = defaultLimits.First().RangeInclusive;
                    record.PerUnit = defaultLimits.First().PerUnit;
                    record.IsDefault = true;

                    new TableOperations<AlarmRangeLimit>(connection).UpdateRecord(CreateNewAlarmRangeLimit(record));
                }
            }
        }

        [AuthorizeHubRole("Administrator")]
        public string SendAlarmTableToCSV()
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                string csv = "";
                string[] headers = new TableOperations<AlarmRangeLimitView>(connection).GetFieldNames();

                foreach (string h in headers)
                {
                    if (csv == "")
                        csv = '[' + h + ']';
                    else
                        csv += ",[" + h + ']';
                }

                csv += "\n";

                IEnumerable<AlarmRangeLimitView> limits = new TableOperations<AlarmRangeLimitView>(connection).QueryRecords().ToList();

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
        }

        #endregion

        #region [ MetersWithHourlyLimits Table Operations ]

        [RecordOperation(typeof(MetersWithHourlyLimits), RecordOperation.QueryRecordCount)]
        public int QueryMetersWithHourlyLimitsCount(string filterText)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                return new TableOperations<MetersWithHourlyLimits>(connection).QueryRecordCount(filterText);
            }
        }

        [RecordOperation(typeof(MetersWithHourlyLimits), RecordOperation.QueryRecords)]
        public IEnumerable<MetersWithHourlyLimits> QueryMetersWithHourlyLimits(string sortField, bool ascending, int page, int pageSize, string filterText)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                return new TableOperations<MetersWithHourlyLimits>(connection).QueryRecords(sortField, ascending, page, pageSize, filterText).ToList();
            }
        }

        [AuthorizeHubRole("Administrator, Editor")]
        [RecordOperation(typeof(MetersWithHourlyLimits), RecordOperation.DeleteRecord)]
        public void DeleteMetersWithHourlyLimits(int id)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                new TableOperations<MetersWithHourlyLimits>(connection).DeleteRecord(id);
            }
        }

        [RecordOperation(typeof(MetersWithHourlyLimits), RecordOperation.CreateNewRecord)]
        public MetersWithHourlyLimits NewMetersWithHourlyLimits()
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                return new TableOperations<MetersWithHourlyLimits>(connection).NewRecord();
            }
        }

        [AuthorizeHubRole("Administrator, Editor")]
        [RecordOperation(typeof(MetersWithHourlyLimits), RecordOperation.AddNewRecord)]
        public void AddNewMetersWithHourlyLimits(MetersWithHourlyLimits record)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                new TableOperations<MetersWithHourlyLimits>(connection).AddNewRecord(record);
            }
        }

        [AuthorizeHubRole("Administrator, Editor")]
        [RecordOperation(typeof(MetersWithHourlyLimits), RecordOperation.UpdateRecord)]
        public void UpdateMetersWithHourlyLimits(MetersWithHourlyLimits record)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                new TableOperations<MetersWithHourlyLimits>(connection).UpdateRecord(record);
            }
        }

        [AuthorizeHubRole("Administrator, Editor")]
        public void AddNewOrUpdateMetersWithHourlyLimits(MetersWithHourlyLimits record)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                new TableOperations<MetersWithHourlyLimits>(connection).AddNewOrUpdateRecord(record);
            }
        }

        public IEnumerable<Meter> GetMetersForSelect()
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                return new TableOperations<Meter>(connection).QueryRecords("Name").ToList();
            }
        }

        public IEnumerable<MeasurementCharacteristic> GetCharacteristicsForSelect()
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                return new TableOperations<MeasurementCharacteristic>(connection).QueryRecords("Name").ToList();
            }
        }

        public void ProcessSmartAlarms(IEnumerable<int> meterIds, IEnumerable<int> typeIds, DateTime startDate, DateTime endDate, int sigmaLevel, int decimals, bool ignoreLargeValues, bool overwriteOldAlarms, int largeValueLevel)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                int progressTotal = (meterIds.Any() ? meterIds.Count() : 1);
                int progressCount = 0;
                ProgressUpdatedOverall("", (int)(100 * (progressCount) / progressTotal));

                string historianServer = connection.ExecuteScalar<string>("SELECT Value FROM Setting WHERE Name = 'Historian.Server'") ?? "127.0.0.1";
                string historianInstance = connection.ExecuteScalar<string>("SELECT Value FROM Setting WHERE Name = 'Historian.Instance'") ?? "XDA";

                foreach (int meterId in meterIds)
                {
                    string characteristicList = "(" + string.Join(",", typeIds) + ")";
                    IEnumerable<int> channelIds = new TableOperations<Channel>(connection).QueryRecordsWhere("MeterID = {0} AND MeasurementCharacteristicID IN " + characteristicList, meterId).Select(x => x.ID);
                    string meterName = connection.ExecuteScalar<string>("Select Name from Meter where ID = {0}", meterId);
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
                        string channelName = connection.ExecuteScalar<string>("Select Name from Channel where ID = {0}", channelId);
                        ProgressUpdatedByMeter(channelName, (int)(100 * (innerProgressCount) / innerProgressTotal));
                        foreach (RunningAvgStdDev data in runningData.Where(x => x.ChannelID == channelId))
                        {
                            double average = data.Sum / (data.Count != 0 ? data.Count : 1);

                            double stdDev = Math.Sqrt(Math.Abs((data.SumOfSquares - 2 * average * data.Sum + data.Count * average * average) / ((data.Count != 1 ? data.Count : 2) - 1)));
                            float high = (float)Math.Round(average + stdDev * sigmaLevel, decimals);
                            float low = (float)Math.Round(average - stdDev * sigmaLevel, decimals);


                            HourOfWeekLimit hwl = new TableOperations<HourOfWeekLimit>(connection).QueryRecordWhere("ChannelID = {0} AND HourOfWeek = {1}", data.ChannelID, data.HourOfWeek);

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
                                new TableOperations<HourOfWeekLimit>(connection).AddNewRecord(newrecord);
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
                                new TableOperations<HourOfWeekLimit>(connection).UpdateRecord(hwl);
                            }
                        }

                        ProgressUpdatedByMeter(channelName, (int)(100 * (++innerProgressCount) / innerProgressTotal));
                    }
                    ProgressUpdatedOverall(meterName, (int)(100 * (++progressCount) / progressTotal));
                }
            }
        }

        #endregion

        #region [ ChannelsWithHourlyLimits Table Operations ]

        [RecordOperation(typeof(ChannelsWithHourlyLimits), RecordOperation.QueryRecordCount)]
        public int QueryChannelsWithHourlyLimitsCount(int meterId, string filterText)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                TableOperations<ChannelsWithHourlyLimits> table = new TableOperations<ChannelsWithHourlyLimits>(connection);
                RecordRestriction restriction = table.GetSearchRestriction(filterText);

                return table.QueryRecordCount(new RecordRestriction("MeterID = {0}", meterId) + restriction);
            }
        }

        [RecordOperation(typeof(ChannelsWithHourlyLimits), RecordOperation.QueryRecords)]
        public IEnumerable<ChannelsWithHourlyLimits> QueryChannelsWithHourlyLimits(int meterId, string sortField, bool ascending, int page, int pageSize, string filterText)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                TableOperations<ChannelsWithHourlyLimits> table = new TableOperations<ChannelsWithHourlyLimits>(connection);
                RecordRestriction restriction = table.GetSearchRestriction(filterText);

                return table.QueryRecords(sortField, ascending, page, pageSize, new RecordRestriction("MeterID = {0}", meterId) + restriction).ToList();
            }
        }

        [AuthorizeHubRole("Administrator, Editor")]
        [RecordOperation(typeof(ChannelsWithHourlyLimits), RecordOperation.DeleteRecord)]
        public void DeleteChannelsWithHourlyLimits(int id)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                new TableOperations<ChannelsWithHourlyLimits>(connection).DeleteRecord(id);
            }
        }

        [RecordOperation(typeof(ChannelsWithHourlyLimits), RecordOperation.CreateNewRecord)]
        public ChannelsWithHourlyLimits NewChannelsWithHourlyLimits()
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                return new TableOperations<ChannelsWithHourlyLimits>(connection).NewRecord();
            }
        }

        [AuthorizeHubRole("Administrator, Editor")]
        [RecordOperation(typeof(ChannelsWithHourlyLimits), RecordOperation.AddNewRecord)]
        public void AddNewChannelsWithHourlyLimits(ChannelsWithHourlyLimits record)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                new TableOperations<ChannelsWithHourlyLimits>(connection).AddNewRecord(record);
            }
        }

        [AuthorizeHubRole("Administrator, Editor")]
        [RecordOperation(typeof(ChannelsWithHourlyLimits), RecordOperation.UpdateRecord)]
        public void UpdateChannelsWithHourlyLimits(ChannelsWithHourlyLimits record)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                new TableOperations<ChannelsWithHourlyLimits>(connection).UpdateRecord(record);
            }
        }

        [AuthorizeHubRole("Administrator, Editor")]
        public void AddNewOrUpdateChannelsWithHourlyLimits(ChannelsWithHourlyLimits record)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                new TableOperations<ChannelsWithHourlyLimits>(connection).AddNewOrUpdateRecord(record);
            }
        }

        #endregion

        #region [ HourOfWeekLimit Table Operations ]

        [RecordOperation(typeof(HourOfWeekLimit), RecordOperation.QueryRecordCount)]
        public int QueryHourOfWeekLimitCount(int channelId, string filterText)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                TableOperations<HourOfWeekLimit> table = new TableOperations<HourOfWeekLimit>(connection);
                RecordRestriction restriction = table.GetSearchRestriction(filterText);

                return table.QueryRecordCount(new RecordRestriction("ChannelID = {0}", channelId) + restriction);
            }
        }

        [RecordOperation(typeof(HourOfWeekLimit), RecordOperation.QueryRecords)]
        public IEnumerable<HourOfWeekLimitView> QueryHourOfWeekLimit(int channelId, string sortField, bool ascending, int page, int pageSize, string filterText)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                TableOperations<HourOfWeekLimitView> table = new TableOperations<HourOfWeekLimitView>(connection);
                RecordRestriction restriction = table.GetSearchRestriction(filterText);

                return table.QueryRecords(sortField, ascending, page, pageSize, new RecordRestriction("ChannelID = {0}", channelId) + restriction).ToList();
            }
        }

        [AuthorizeHubRole("Administrator, Editor")]
        [RecordOperation(typeof(HourOfWeekLimit), RecordOperation.DeleteRecord)]
        public void DeleteHourOfWeekLimit(int id)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                new TableOperations<HourOfWeekLimit>(connection).DeleteRecord(id);
            }
        }

        [RecordOperation(typeof(HourOfWeekLimit), RecordOperation.CreateNewRecord)]
        public HourOfWeekLimit NewHourOfWeekLimit()
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                return new TableOperations<HourOfWeekLimit>(connection).NewRecord();
            }
        }

        [AuthorizeHubRole("Administrator, Editor")]
        [RecordOperation(typeof(HourOfWeekLimit), RecordOperation.AddNewRecord)]
        public void AddNewHourOfWeekLimit(HourOfWeekLimit record)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                new TableOperations<HourOfWeekLimit>(connection).AddNewRecord(record);
            }
        }

        [AuthorizeHubRole("Administrator, Editor")]
        [RecordOperation(typeof(HourOfWeekLimit), RecordOperation.UpdateRecord)]
        public void UpdateHourOfWeekLimit(HourOfWeekLimit record)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                new TableOperations<HourOfWeekLimit>(connection).UpdateRecord(record);
            }
        }

        [AuthorizeHubRole("Administrator, Editor")]
        public void AddNewOrUpdateHourOfWeekLimit(HourOfWeekLimit record)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                new TableOperations<HourOfWeekLimit>(connection).AddNewOrUpdateRecord(record);
            }
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
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                return new TableOperations<MetersWithNormalLimits>(connection).QueryRecordCount(filterText);
            }
        }

        [RecordOperation(typeof(MetersWithNormalLimits), RecordOperation.QueryRecords)]
        public IEnumerable<MetersWithNormalLimits> QueryMetersWithNormalLimits(string sortField, bool ascending, int page, int pageSize, string filterText)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                return new TableOperations<MetersWithNormalLimits>(connection).QueryRecords(sortField, ascending, page, pageSize, filterText).ToList();
            }
        }

        [AuthorizeHubRole("Administrator, Editor")]
        [RecordOperation(typeof(MetersWithNormalLimits), RecordOperation.DeleteRecord)]
        public void DeleteMetersWithNormalLimits(int id)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                new TableOperations<MetersWithNormalLimits>(connection).DeleteRecord(id);
            }
        }

        [RecordOperation(typeof(MetersWithNormalLimits), RecordOperation.CreateNewRecord)]
        public MetersWithNormalLimits NewMetersWithNormalLimits()
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                return new TableOperations<MetersWithNormalLimits>(connection).NewRecord();
            }
        }

        [AuthorizeHubRole("Administrator, Editor")]
        [RecordOperation(typeof(MetersWithNormalLimits), RecordOperation.AddNewRecord)]
        public void AddNewMetersWithNormalLimits(MetersWithNormalLimits record)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                new TableOperations<MetersWithNormalLimits>(connection).AddNewRecord(record);
            }
        }

        [AuthorizeHubRole("Administrator, Editor")]
        [RecordOperation(typeof(MetersWithNormalLimits), RecordOperation.UpdateRecord)]
        public void UpdateMetersWithNormalLimits(MetersWithNormalLimits record)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                new TableOperations<MetersWithNormalLimits>(connection).UpdateRecord(record);
            }
        }

        [AuthorizeHubRole("Administrator, Editor")]
        public void AddNewOrUpdateMetersWithNormalLimits(MetersWithNormalLimits record)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                new TableOperations<MetersWithNormalLimits>(connection).AddNewOrUpdateRecord(record);
            }
        }

        public void ProcessSmartAlarmsNormal(IEnumerable<int> meterIds, IEnumerable<int> typeIds, DateTime startDate, DateTime endDate, int sigmaLevel, int decimals, bool ignoreLargeValues, bool overwriteOldAlarms, int largeValueLevel)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                int progressTotal = (meterIds.Any() ? meterIds.Count() : 1);
                int progressCount = 0;
                ProgressUpdatedOverall("", (int)(100 * (progressCount) / progressTotal));

                string historianServer = connection.ExecuteScalar<string>("SELECT Value FROM Setting WHERE Name = 'Historian.Server'") ?? "127.0.0.1";
                string historianInstance = connection.ExecuteScalar<string>("SELECT Value FROM Setting WHERE Name = 'Historian.Instance'") ?? "XDA";

                foreach (int meterId in meterIds)
                {
                    string characteristicList = "(" + string.Join(",", typeIds) + ")";
                    IEnumerable<int> channelIds = new TableOperations<Channel>(connection).QueryRecordsWhere("MeterID = {0} AND MeasurementCharacteristicID IN " + characteristicList, meterId).Select(x => x.ID);
                    string meterName = connection.ExecuteScalar<string>("Select Name from Meter where ID = {0}", meterId);
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
                        Channel channel = new TableOperations<Channel>(connection).QueryRecordWhere("ID = {0}", channelId);
                        ProgressUpdatedByMeter(channel.Name, (int)(100 * (innerProgressCount) / innerProgressTotal));
                        RunningAvgStdDev record = normalRunningData.Where(x => x.ChannelID == channelId).FirstOrDefault();
                        if (record != null)
                        {
                            double average = record.Sum / (record.Count != 0 ? record.Count : 1);

                            double stdDev = Math.Sqrt(Math.Abs((record.SumOfSquares - 2 * average * record.Sum + record.Count * average * average) / ((record.Count != 1 ? record.Count : 2) - 1)));
                            float high = (float)Math.Round(average + stdDev * sigmaLevel, decimals);
                            float low = (float)Math.Round(average - stdDev * sigmaLevel, decimals);

                            AlarmRangeLimit hwl = new TableOperations<AlarmRangeLimit>(connection).QueryRecordWhere("ChannelID = {0}", record.ChannelID);

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
                                new TableOperations<AlarmRangeLimit>(connection).AddNewRecord(newRecord);
                            }
                            else if (hwl != null && overwriteOldAlarms)
                            {
                                hwl.High = high;
                                hwl.Low = low;
                                new TableOperations<AlarmRangeLimit>(connection).UpdateRecord(hwl);
                            }

                        }

                        ProgressUpdatedByMeter(channel.Name, (int)(100 * (++innerProgressCount) / innerProgressTotal));
                    }
                    ProgressUpdatedOverall(meterName, (int)(100 * (++progressCount) / progressTotal));
                }
            }
        }

        #endregion

        #region [ ChannelsWithNormalLimits Table Operations ]

        [RecordOperation(typeof(ChannelsWithNormalLimits), RecordOperation.QueryRecordCount)]
        public int QueryChannelsWithNormalLimitsCount(int meterId, string filterText)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                TableOperations<ChannelsWithNormalLimits> table = new TableOperations<ChannelsWithNormalLimits>(connection);
                RecordRestriction restriction = table.GetSearchRestriction(filterText);

                return table.QueryRecordCount(new RecordRestriction("MeterID = {0}", meterId) + restriction);
            }
        }

        [RecordOperation(typeof(ChannelsWithNormalLimits), RecordOperation.QueryRecords)]
        public IEnumerable<ChannelsWithNormalLimits> QueryChannelsWithNormalLimits(int meterId, string sortField, bool ascending, int page, int pageSize, string filterText)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                TableOperations<ChannelsWithNormalLimits> table = new TableOperations<ChannelsWithNormalLimits>(connection);
                RecordRestriction restriction = table.GetSearchRestriction(filterText);

                return table.QueryRecords(sortField, ascending, page, pageSize, new RecordRestriction("MeterID = {0}", meterId) + restriction).ToList();
            }
        }

        [AuthorizeHubRole("Administrator, Editor")]
        [RecordOperation(typeof(ChannelsWithNormalLimits), RecordOperation.DeleteRecord)]
        public void DeleteChannelsWithNormalLimits(int id)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                new TableOperations<ChannelsWithNormalLimits>(connection).DeleteRecord(id);
            }
        }

        [RecordOperation(typeof(ChannelsWithNormalLimits), RecordOperation.CreateNewRecord)]
        public ChannelsWithNormalLimits NewChannelsWithNormalLimits()
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                return new TableOperations<ChannelsWithNormalLimits>(connection).NewRecord();
            }
        }

        [AuthorizeHubRole("Administrator, Editor")]
        [RecordOperation(typeof(ChannelsWithNormalLimits), RecordOperation.AddNewRecord)]
        public void AddNewChannelsWithNormalLimits(ChannelsWithNormalLimits record)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                new TableOperations<ChannelsWithNormalLimits>(connection).AddNewRecord(record);
            }
        }

        [AuthorizeHubRole("Administrator, Editor")]
        [RecordOperation(typeof(ChannelsWithNormalLimits), RecordOperation.UpdateRecord)]
        public void UpdateChannelsWithNormalLimits(ChannelsWithNormalLimits record)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                AlarmRangeLimit alarm = new TableOperations<AlarmRangeLimit>(connection).QueryRecordWhere("ChannelID = {0}", record.ID);

                alarm.High = record.High;
                alarm.Low = record.Low;
                alarm.RangeInclusive = record.RangeInclusive;
                alarm.PerUnit = record.PerUnit;
                alarm.IsDefault = record.IsDefault;
                alarm.Enabled = record.Enabled;

                new TableOperations<AlarmRangeLimit>(connection).UpdateRecord(alarm);

            }
        }

        [AuthorizeHubRole("Administrator, Editor")]
        public void AddNewOrUpdateChannelsWithNormalLimits(ChannelsWithNormalLimits record)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                AlarmRangeLimit alarm = new TableOperations<AlarmRangeLimit>(connection).QueryRecordWhere("ChannelID = {0}", record.ID);

                alarm.High = record.High;
                alarm.Low = record.Low;
                alarm.RangeInclusive = record.RangeInclusive;
                alarm.PerUnit = record.PerUnit;
                alarm.IsDefault = record.IsDefault;
                alarm.Enabled = record.Enabled;

                new TableOperations<AlarmRangeLimit>(connection).AddNewOrUpdateRecord(alarm);
            }
        }

        public void ResetAlarmToDefault2(int id)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                IEnumerable<DefaultAlarmRangeLimit> defaultLimits = new TableOperations<DefaultAlarmRangeLimit>(connection).QueryRecords(restriction: new RecordRestriction("MeasurementTypeID = (SELECT MeasurementTypeID FROM Channel WHERE ID = {0}) AND MeasurementCharacteristicID = (SELECT MeasurementCharacteristicID FROM Channel WHERE ID = {0})", id)).ToList();
                AlarmRangeLimit record = new TableOperations<AlarmRangeLimit>(connection).QueryRecordWhere("ChannelID = {0}", id);
                if (defaultLimits.Any())
                {
                    record.Severity = defaultLimits.First().Severity;
                    record.High = defaultLimits.First().High;
                    record.Low = defaultLimits.First().Low;
                    record.RangeInclusive = defaultLimits.First().RangeInclusive;
                    record.PerUnit = defaultLimits.First().PerUnit;
                    record.IsDefault = true;

                    new TableOperations<AlarmRangeLimit>(connection).UpdateRecord(record);
                }
            }
        }

        public void UpdateAlarmRangeLimit(int id, double high, double low)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                AlarmRangeLimit record = new TableOperations<AlarmRangeLimit>(connection).QueryRecordWhere("ChannelID = {0}", id);

                record.High = high;
                record.Low = low;
                record.IsDefault = false;
                new TableOperations<AlarmRangeLimit>(connection).UpdateRecord(record);
            }
        }

        #endregion

        #region [ DefaultAlarmRangeLimit Table Operations ]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(DefaultAlarmRangeLimitView), RecordOperation.QueryRecordCount)]
        public int QueryDefaultAlarmRangeLimitViewCount(string filterString)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                return new TableOperations<DefaultAlarmRangeLimitView>(connection).QueryRecordCount(filterString);
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(DefaultAlarmRangeLimitView), RecordOperation.QueryRecords)]
        public IEnumerable<DefaultAlarmRangeLimitView> QueryDefaultAlarmRangeLimitViews(string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                return new TableOperations<DefaultAlarmRangeLimitView>(connection).QueryRecords(sortField, ascending, page, pageSize, filterString).ToList();
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(DefaultAlarmRangeLimitView), RecordOperation.DeleteRecord)]
        public void DeleteDefaultAlarmRangeLimitView(int id)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                new TableOperations<DefaultAlarmRangeLimitView>(connection).DeleteRecord(id);
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(DefaultAlarmRangeLimitView), RecordOperation.CreateNewRecord)]
        public DefaultAlarmRangeLimitView NewDefaultAlarmRangeLimitView()
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                return new TableOperations<DefaultAlarmRangeLimitView>(connection).NewRecord();
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(DefaultAlarmRangeLimitView), RecordOperation.AddNewRecord)]
        public void AddNewDefaultAlarmRangeLimitView(DefaultAlarmRangeLimitView record)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                new TableOperations<DefaultAlarmRangeLimit>(connection).AddNewRecord(CreateNewAlarmRangeLimit(record));
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(DefaultAlarmRangeLimitView), RecordOperation.UpdateRecord)]
        public void UpdateDefaultAlarmRangeLimitView(DefaultAlarmRangeLimitView record)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                new TableOperations<DefaultAlarmRangeLimit>(connection).UpdateRecord(CreateNewAlarmRangeLimit(record));
            }
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
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                IEnumerable<Channel> channels = new TableOperations<Channel>(connection).QueryRecords(restriction: new RecordRestriction("MeasurementTypeID = {0} AND MeasurementCharacteristicID = {1}", record.MeasurementTypeID, record.MeasurementCharacteristicID)).ToList();
                string channelIDs = "";
                foreach (Channel channel in channels)
                {
                    if (channelIDs == "")
                        channelIDs += channel.ID.ToString();
                    else
                        channelIDs += ',' + channel.ID.ToString();
                }
                TableOperations<AlarmRangeLimit> table = new TableOperations<AlarmRangeLimit>(connection);

                IEnumerable<AlarmRangeLimit> limits = table.QueryRecords(restriction: new RecordRestriction($"ChannelID IN ({channelIDs})"));
                foreach (AlarmRangeLimit limit in limits)
                {
                    limit.IsDefault = true;
                    limit.High = record.High;
                    limit.Low = record.Low;
                    limit.Severity = record.Severity;
                    limit.RangeInclusive = record.RangeInclusive;
                    limit.PerUnit = record.PerUnit;
                    table.UpdateRecord(limit);
                }
            }
        }

        #endregion

        #endregion

        #region [ Emails ]
        #region [ EmailType Table Operations ]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(EmailType), RecordOperation.QueryRecordCount)]
        public int QueryEmailTypeCount(int emailTypeID, string filterString)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                TableOperations<EmailTypeView> tableOperations = new TableOperations<EmailTypeView>(connection);
                RecordRestriction restriction;

                if (emailTypeID > 0)
                    restriction = tableOperations.GetSearchRestriction(filterString) + new RecordRestriction("ID = {0}", emailTypeID);
                else
                    restriction = tableOperations.GetSearchRestriction(filterString);

                return tableOperations.QueryRecordCount(restriction);
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(EmailType), RecordOperation.QueryRecords)]
        public IEnumerable<EmailTypeView> QueryEmailType(int emailTypeID, string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                TableOperations<EmailTypeView> tableOperations = new TableOperations<EmailTypeView>(connection);
                RecordRestriction restriction;

                if (emailTypeID > 0)
                    restriction = tableOperations.GetSearchRestriction(filterString) + new RecordRestriction("ID = {0}", emailTypeID);
                else
                    restriction = tableOperations.GetSearchRestriction(filterString);

                return tableOperations.QueryRecords(sortField, ascending, page, pageSize, restriction).ToList();
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(EmailType), RecordOperation.DeleteRecord)]
        public void DeleteEmailType(int id)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                new TableOperations<EmailType>(connection).DeleteRecord(id);
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(EmailType), RecordOperation.CreateNewRecord)]
        public EmailType NewEmailType()
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                return new TableOperations<EmailType>(connection).NewRecord();
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(EmailType), RecordOperation.AddNewRecord)]
        public void AddNewEmailType(EmailTypeView record)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                new TableOperations<EmailType>(connection).AddNewRecord(CreateEmailTypeFromView(record));
            }
        }

        [AuthorizeHubRole("Administrator, Owner")]
        [RecordOperation(typeof(EmailType), RecordOperation.UpdateRecord)]
        public void UpdateEmailType(EmailTypeView record)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                new TableOperations<EmailType>(connection).UpdateRecord(CreateEmailTypeFromView(record));
            }
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

        #region [ XSLTemplate Table Operations ]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(XSLTemplate), RecordOperation.QueryRecordCount)]
        public int QueryXSLTemplateCount(string filterString)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                return new TableOperations<XSLTemplate>(connection).QueryRecordCount(filterString);
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(XSLTemplate), RecordOperation.QueryRecords)]
        public IEnumerable<XSLTemplate> QueryXSLTemplate(string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                return new TableOperations<XSLTemplate>(connection).QueryRecords(sortField, ascending, page, pageSize, filterString).ToList();
            }
        }

        public XSLTemplate QueryXSLTemplateByID(int id)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                return new TableOperations<XSLTemplate>(connection).QueryRecords(restriction: new RecordRestriction("ID = {0}", id)).ToList().First();
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(XSLTemplate), RecordOperation.DeleteRecord)]
        public void DeleteXSLTemplate(int id)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                new TableOperations<XSLTemplate>(connection).DeleteRecord(id);
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(XSLTemplate), RecordOperation.CreateNewRecord)]
        public XSLTemplate NewXSLTemplate()
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                return new TableOperations<XSLTemplate>(connection).NewRecord();
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(XSLTemplate), RecordOperation.AddNewRecord)]
        public void AddNewXSLTemplate(XSLTemplate record)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                new TableOperations<XSLTemplate>(connection).AddNewRecord(record);
            }
        }

        [AuthorizeHubRole("Administrator, Owner")]
        [RecordOperation(typeof(XSLTemplate), RecordOperation.UpdateRecord)]
        public void UpdateXSLTemplate(XSLTemplate record)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                new TableOperations<XSLTemplate>(connection).UpdateRecord(record);
            }
        }


        #endregion

        #region [ Event Email Configuration Table Operations ]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(EventEmailParameters), RecordOperation.QueryRecordCount)]
        public int QueryEventEmailConfigurationCount(string filterString)
        {
            return GetPagedQueryCount( @"
                SELECT
                    EventEmailParameters.ID,
	                XSLTemplate.Name,
                    SMS,
	                MinDelay,
	                MaxDelay
                FROM
	                EventEmailParameters JOIN
	                EmailType ON EventEmailParameters.EmailTypeID = EmailType.ID JOIN
	                XSLTemplate ON EmailType.XSLTemplateID = XSLTemplate.ID
                WHERE 
                    XSLTemplate.Name LIKE '%' + {0} + '%'
            ", filterString);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(EventEmailParameters), RecordOperation.QueryRecords)]
        public DataTable QueryEventEmailConfiguration(string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            return GetPagedQuery(@"
                SELECT
                    EventEmailParameters.ID,
	                XSLTemplate.Name,
                    SMS,
	                MinDelay,
	                MaxDelay
                FROM
	                EventEmailParameters JOIN
	                EmailType ON EventEmailParameters.EmailTypeID = EmailType.ID JOIN
	                XSLTemplate ON EmailType.XSLTemplateID = XSLTemplate.ID 
                WHERE 
                    XSLTemplate.Name LIKE '%' + {0} + '%'
            ", sortField, ascending, page, pageSize, filterString);
        }

        public EventEmailParameters QueryEventEmailConfigurationByID(int id)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                return new TableOperations<EventEmailParameters>(connection).QueryRecordWhere("ID = {0}", id);
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(EventEmailParameters), RecordOperation.DeleteRecord)]
        public void DeleteEventEmailConfiguration(int id)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                new TableOperations<EventEmailParameters>(connection).DeleteRecord(id);
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(EventEmailParameters), RecordOperation.CreateNewRecord)]
        public Dictionary<string, object> NewEventEmailConfiguration()
        {
            return new Dictionary<string, object>() {
                {"ID", 0 },
                { "Name", string.Empty },
                { "MinDelay", 0 },
                { "MaxDelay", 0 },
                { "SMS", false }
            };
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(EventEmailParameters), RecordOperation.AddNewRecord)]
        public void AddNewEventEmailConfiguration(Dictionary<string, object> record)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                new TableOperations<XSLTemplate>(connection).AddNewRecord(new XSLTemplate() { Name = record["Name"].ToString(), Template = "" });
                int emailCategoryID = connection.ExecuteScalar<int>("SELECT ID FROM EmailCategory WHERE Name = 'Event'");
                int xslTemplateID = connection.ExecuteScalar<int>("SELECT ID FROM XSLTemplate WHERE Name = {0}", record["Name"].ToString());
                new TableOperations<EmailType>(connection).AddNewRecord(new EmailType() { EmailCategoryID = emailCategoryID, XSLTemplateID = xslTemplateID, SMS = bool.Parse(record["SMS"].ToString()) });
                int emailTypeID = connection.ExecuteScalar<int>("SELECT ID FROM EmailType WHERE EmailCategoryID = {0} AND XSLTemplateID = {1}", emailCategoryID, xslTemplateID);
                new TableOperations<EventEmailParameters>(connection).AddNewRecord(new EventEmailParameters() { EmailTypeID = emailTypeID, TriggersEmailSQL = "", EventDetailSQL = "", MaxDelay = double.Parse(record["MaxDelay"].ToString()), MinDelay = double.Parse(record["MinDelay"].ToString()) });

            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(EventEmailParameters), RecordOperation.UpdateRecord)]
        public void UpdateEventEmailConfiguration(Dictionary<string, object> record)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                connection.ExecuteNonQuery("UPDATE XSLTemplate SET Name = {0} WHERE ID = (SELECT XSLTemplateID FROM EmailType WHERE ID = (SELECT EmailTypeID FROM EventEmailParameters WHERE ID = {1}))", record["Name"].ToString(), int.Parse(record["ID"].ToString()));
                connection.ExecuteNonQuery("UPDATE EventEmailParameters SET MinDelay = {0}, MaxDelay = {1} WHERE ID = {2}", double.Parse(record["MinDelay"].ToString()), double.Parse(record["MaxDelay"].ToString()), int.Parse(record["ID"].ToString()));
                connection.ExecuteNonQuery("UPDATE EmailType SET SMS = {0} WHERE ID = (SELECT EmailTypeID FROM EventEmailParameters WHERE ID = {1})", bool.Parse(record["SMS"].ToString()), int.Parse(record["ID"].ToString()));
            }
        }

        public void UpdateEventEmailTemplates(Dictionary<string, object> record)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                if (record.ContainsKey("template"))
                    connection.ExecuteNonQuery("UPDATE XSLTemplate SET Template = {0} WHERE ID = (SELECT XSLTemplateID FROM EmailType WHERE ID = (SELECT EmailTypeID FROM EventEmailParameters WHERE ID = {1}))", record["template"].ToString(), int.Parse(record["ID"].ToString()));
                else if (record.ContainsKey("trigger"))
                    connection.ExecuteNonQuery("UPDATE EventEmailParameters SET TriggersEmailSQL = {0} WHERE ID = {1}", record["trigger"].ToString(), int.Parse(record["ID"].ToString()));
                else if (record.ContainsKey("event"))
                    connection.ExecuteNonQuery("UPDATE EventEmailParameters SET EventDetailSQL = {0} WHERE ID = {1}", record["event"].ToString(), int.Parse(record["ID"].ToString()));
            }
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
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                EventCriterion ec = new EventCriterion();

                ec.five = connection.ExecuteScalar("SELECT ID FROM DisturbanceEmailCriterion WHERE EmailGroupID = {0} AND SeverityCode = 5", EmailGroupID) != null;
                ec.four = connection.ExecuteScalar("SELECT ID FROM DisturbanceEmailCriterion WHERE EmailGroupID = {0} AND SeverityCode = 4", EmailGroupID) != null;
                ec.three = connection.ExecuteScalar("SELECT ID FROM DisturbanceEmailCriterion WHERE EmailGroupID = {0} AND SeverityCode = 3", EmailGroupID) != null;
                ec.two = connection.ExecuteScalar("SELECT ID FROM DisturbanceEmailCriterion WHERE EmailGroupID = {0} AND SeverityCode = 2", EmailGroupID) != null;
                ec.one = connection.ExecuteScalar("SELECT ID FROM DisturbanceEmailCriterion WHERE EmailGroupID = {0} AND SeverityCode = 1", EmailGroupID) != null;
                ec.zero = connection.ExecuteScalar("SELECT ID FROM DisturbanceEmailCriterion WHERE EmailGroupID = {0} AND SeverityCode = 0", EmailGroupID) != null;
                ec.fault = connection.ExecuteScalar("SELECT ID FROM FaultEmailCriterion WHERE EmailGroupID = {0}", EmailGroupID) != null;
                ec.disturbances = ec.five || ec.four || ec.three || ec.two || ec.one || ec.zero;

                return ec;
            }
        }

        public void UpdateEventCriterion(EventCriterion record, int EmailGroupID)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                if (record.fault && connection.ExecuteScalar("SELECT ID FROM FaultEmailCriterion WHERE EmailGroupID = {0}", EmailGroupID) == null)
                    new TableOperations<FaultEmailCriterion>(connection).AddNewRecord(new FaultEmailCriterion() { EmailGroupID = EmailGroupID });
                else if (!record.fault && connection.ExecuteScalar("SELECT ID FROM FaultEmailCriterion WHERE EmailGroupID = {0}", EmailGroupID) != null)
                {
                    int index = connection.ExecuteScalar<int>("SELECT ID FROM FaultEmailCriterion WHERE EmailGroupID ={0}", EmailGroupID);
                    new TableOperations<FaultEmailCriterion>(connection).DeleteRecord(index);
                }

                if (record.five && connection.ExecuteScalar("SELECT ID FROM DisturbanceEmailCriterion WHERE EmailGroupID = {0} AND SeverityCode = 5", EmailGroupID) == null)
                    new TableOperations<DisturbanceEmailCriterion>(connection).AddNewRecord(new DisturbanceEmailCriterion() { EmailGroupID = EmailGroupID, SeverityCode = 5 });
                else if (!record.five && connection.ExecuteScalar("SELECT ID FROM DisturbanceEmailCriterion WHERE EmailGroupID = {0} AND SeverityCode = 5", EmailGroupID) != null)
                {
                    int index = connection.ExecuteScalar<int>("SELECT ID FROM DisturbanceEmailCriterion WHERE EmailGroupID = {0} AND SeverityCode = 5", EmailGroupID);
                    new TableOperations<DisturbanceEmailCriterion>(connection).DeleteRecord(index);
                }

                if (record.four && connection.ExecuteScalar("SELECT ID FROM DisturbanceEmailCriterion WHERE EmailGroupID = {0} AND SeverityCode = 4", EmailGroupID) == null)
                    new TableOperations<DisturbanceEmailCriterion>(connection).AddNewRecord(new DisturbanceEmailCriterion() { EmailGroupID = EmailGroupID, SeverityCode = 4 });
                else if (!record.five && connection.ExecuteScalar("SELECT ID FROM DisturbanceEmailCriterion WHERE EmailGroupID = {0} AND SeverityCode = 4", EmailGroupID) != null)
                {
                    int index = connection.ExecuteScalar<int>("SELECT ID FROM DisturbanceEmailCriterion WHERE EmailGroupID = {0} AND SeverityCode = 4", EmailGroupID);
                    new TableOperations<DisturbanceEmailCriterion>(connection).DeleteRecord(index);
                }

                if (record.three && connection.ExecuteScalar("SELECT ID FROM DisturbanceEmailCriterion WHERE EmailGroupID = {0} AND SeverityCode = 3", EmailGroupID) == null)
                    new TableOperations<DisturbanceEmailCriterion>(connection).AddNewRecord(new DisturbanceEmailCriterion() { EmailGroupID = EmailGroupID, SeverityCode = 3 });
                else if (!record.five && connection.ExecuteScalar("SELECT ID FROM DisturbanceEmailCriterion WHERE EmailGroupID = {0} AND SeverityCode = 3", EmailGroupID) != null)
                {
                    int index = connection.ExecuteScalar<int>("SELECT ID FROM DisturbanceEmailCriterion WHERE EmailGroupID = {0} AND SeverityCode = 3", EmailGroupID);
                    new TableOperations<DisturbanceEmailCriterion>(connection).DeleteRecord(index);
                }

                if (record.two && connection.ExecuteScalar("SELECT ID FROM DisturbanceEmailCriterion WHERE EmailGroupID = {0} AND SeverityCode = 2", EmailGroupID) == null)
                    new TableOperations<DisturbanceEmailCriterion>(connection).AddNewRecord(new DisturbanceEmailCriterion() { EmailGroupID = EmailGroupID, SeverityCode = 2 });
                else if (!record.five && connection.ExecuteScalar("SELECT ID FROM DisturbanceEmailCriterion WHERE EmailGroupID = {0} AND SeverityCode = 2", EmailGroupID) != null)
                {
                    int index = connection.ExecuteScalar<int>("SELECT ID FROM DisturbanceEmailCriterion WHERE EmailGroupID = {0} AND SeverityCode = 2", EmailGroupID);
                    new TableOperations<DisturbanceEmailCriterion>(connection).DeleteRecord(index);
                }

                if (record.one && connection.ExecuteScalar("SELECT ID FROM DisturbanceEmailCriterion WHERE EmailGroupID = {0} AND SeverityCode = 1", EmailGroupID) == null)
                    new TableOperations<DisturbanceEmailCriterion>(connection).AddNewRecord(new DisturbanceEmailCriterion() { EmailGroupID = EmailGroupID, SeverityCode = 1 });
                else if (!record.five && connection.ExecuteScalar("SELECT ID FROM DisturbanceEmailCriterion WHERE EmailGroupID = {0} AND SeverityCode = 1", EmailGroupID) != null)
                {
                    int index = connection.ExecuteScalar<int>("SELECT ID FROM DisturbanceEmailCriterion WHERE EmailGroupID = {0} AND SeverityCode = 1", EmailGroupID);
                    new TableOperations<DisturbanceEmailCriterion>(connection).DeleteRecord(index);
                }

                if (record.zero && connection.ExecuteScalar("SELECT ID FROM DisturbanceEmailCriterion WHERE EmailGroupID = {0} AND SeverityCode = 0", EmailGroupID) == null)
                    new TableOperations<DisturbanceEmailCriterion>(connection).AddNewRecord(new DisturbanceEmailCriterion() { EmailGroupID = EmailGroupID, SeverityCode = 0 });
                else if (!record.five && connection.ExecuteScalar("SELECT ID FROM DisturbanceEmailCriterion WHERE EmailGroupID = {0} AND SeverityCode = 0", EmailGroupID) != null)
                {
                    int index = connection.ExecuteScalar<int>("SELECT ID FROM DisturbanceEmailCriterion WHERE EmailGroupID = {0} AND SeverityCode = 0", EmailGroupID);
                    new TableOperations<DisturbanceEmailCriterion>(connection).DeleteRecord(index);
                }

            }
        }

        #endregion

        #endregion

        #region [ External Links]
        #region [ PQViewSite Table Operations ]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(PQViewSite), RecordOperation.QueryRecordCount)]
        public int QueryPQViewSiteCount(string filterString)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                return new TableOperations<PQViewSite>(connection).QueryRecordCount(filterString);
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(PQViewSite), RecordOperation.QueryRecords)]
        public IEnumerable<PQViewSite> QueryPQViewSites(string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                return new TableOperations<PQViewSite>(connection).QueryRecords(sortField, ascending, page, pageSize, filterString).ToList();
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(PQViewSite), RecordOperation.DeleteRecord)]
        public void DeletePQViewSite(int id)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                new TableOperations<PQViewSite>(connection).DeleteRecord(id);
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(PQViewSite), RecordOperation.CreateNewRecord)]
        public PQViewSite NewPQViewSite()
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                return new TableOperations<PQViewSite>(connection).NewRecord();
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(PQViewSite), RecordOperation.AddNewRecord)]
        public void AddNewPQViewSite(PQViewSite record)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                new TableOperations<PQViewSite>(connection).AddNewRecord(record);
            }
        }

        [AuthorizeHubRole("Administrator, Owner")]
        [RecordOperation(typeof(PQViewSite), RecordOperation.UpdateRecord)]
        public void UpdatePQViewSite(PQViewSite record)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                new TableOperations<PQViewSite>(connection).UpdateRecord(record);
            }
        }

        #endregion

        #endregion

        #region [ Stored Procedure Operations ]

        public List<TrendingData> GetTrendingData(DateTime startDate, DateTime endDate, int channelId)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                List<TrendingData> trendingData = new List<TrendingData>();

                string historianServer = connection.ExecuteScalar<string>("SELECT Value FROM Setting WHERE Name = 'Historian.Server'") ?? "127.0.0.1";
                string historianInstance = connection.ExecuteScalar<string>("SELECT Value FROM Setting WHERE Name = 'Historian.Instance'") ?? "XDA";
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
        }

        private void CascadeDelete(string tableName, string criterion)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            using (IDbCommand sc = connection.Connection.CreateCommand())
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


        #region [Asset Overview]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Asset), RecordOperation.QueryRecordCount)]
        public int QueryAssetCount(int locationID, string filterString)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                RecordRestriction restriction;
                TableOperations<AssetView> tableOperations = new TableOperations<AssetView>(connection);
                if (locationID > 0)
                    restriction = tableOperations.GetSearchRestriction(filterString) + AssetLocationRestriction(locationID,connection);
                else
                    restriction = tableOperations.GetSearchRestriction(filterString);

                return tableOperations.QueryRecordCount(restriction);
            }
        }

        private RecordRestriction AssetLocationRestriction(int locationID, AdoDataConnection connection)
        {
            List<int> assetIds = new TableOperations<AssetLocation>(connection).QueryRecordsWhere("LocationID = {0}",locationID).Select(item => item.AssetID).ToList();
            string restriction = string.Join(",", assetIds.Select(item => item.ToString()));
            return new RecordRestriction("ID in (" + restriction + ")");
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Asset), RecordOperation.QueryRecords)]
        public IEnumerable<AssetView> QueryAssets(int locationID, string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                RecordRestriction restriction;
                TableOperations<AssetView> tableOperations = new TableOperations<AssetView>(connection);

                if (locationID > 0)
                    restriction = tableOperations.GetSearchRestriction(filterString) + AssetLocationRestriction(locationID, connection);
                else
                    restriction = tableOperations.GetSearchRestriction(filterString);

                return tableOperations.QueryRecords(sortField, ascending, page, pageSize, restriction).ToList();
            }
        }

        [AuthorizeHubRole("Administrator, Owner")]
        [RecordOperation(typeof(Asset), RecordOperation.UpdateRecord)]
        public void UpdateAsset(AssetView record)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                
                new TableOperations<Asset>(connection).UpdateRecord(CreateAsset(record));
            }
        }

        private Asset CreateAsset(AssetView record)
        {
            Asset asset = NewAsset();
            asset.ID = record.ID;
            asset.AssetKey = record.AssetKey;
            asset.Description = record.Description;
            asset.Spare = record.Spare;

            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                asset.AssetTypeID = connection.ExecuteScalar<int>("SELECT ID From AssetType WHERE Name = {0}", record.AssetType);
            }

            asset.AssetName = record.AssetName;
            asset.VoltageKV = record.VoltageKV;
            return asset;
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Asset), RecordOperation.AddNewRecord)]
        public void AddNewAssetView(AssetView record)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                int added = new TableOperations<Asset>(connection).AddNewRecord(CreateAsset(record));
                if (added != 0)
                {
                    int index = connection.ExecuteScalar<int?>("SELECT IDENT_CURRENT('Asset')") ?? 0;
                    record.ID = index;
                    int assetTypeID = connection.ExecuteScalar<int>("SELECT ID From AssetType WHERE Name = {0}", record.AssetType);

                    switch (assetTypeID)
                    {
                        case ((int)AssetType.Breaker):
                            connection.ExecuteNonQuery("INSERT INTO BreakerAttributes (AssetID) VALUES ({0})", record.ID);
                            break;
                        case ((int)AssetType.Bus):
                            connection.ExecuteNonQuery("INSERT INTO BusAttributes (AssetID) VALUES ({0})", record.ID);
                            break;
                        case ((int)AssetType.CapacitorBank):
                            connection.ExecuteNonQuery("INSERT INTO CapacitorBankAttributes (AssetID) VALUES ({0})", record.ID);
                            break;
                        case ((int)AssetType.Line):
                            connection.ExecuteNonQuery("INSERT INTO LineAttributes (AssetID) VALUES ({0})", record.ID);
                            break;
                        case ((int)AssetType.Transformer):
                            connection.ExecuteNonQuery("INSERT INTO TransformerAttributes (AssetID) VALUES ({0})", record.ID);
                            break;
                    }
                }
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Asset), RecordOperation.DeleteRecord)]
        public void DeleteAsset(int id)
        {
            CascadeDelete("Asset", $"ID = {id}");
        }

        
        private Asset NewAsset()
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                return new TableOperations<Asset>(connection).NewRecord();
            }

        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Asset), RecordOperation.CreateNewRecord)]
        public AssetView NewAssetView()
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                return new TableOperations<AssetView>(connection).NewRecord();
            }

        }

        #endregion

        #region [Breaker Overview]

            [AuthorizeHubRole("Administrator")]
            [RecordOperation(typeof(Breaker), RecordOperation.QueryRecordCount)]
            public int QueryBreakerCount(string filterString)
            {
                using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
                {
                    return new TableOperations<Breaker>(connection).QueryRecordCount(filterString);
                }
            }

            [AuthorizeHubRole("Administrator")]
            [RecordOperation(typeof(Breaker), RecordOperation.QueryRecords)]
            public IEnumerable<Breaker> QueryBreaker(string sortField, bool ascending, int page, int pageSize, string filterString)
            {
                using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
                {
                    return new TableOperations<Breaker>(connection).QueryRecords(sortField, ascending, page, pageSize, filterString).ToList();
                }
            }

            [AuthorizeHubRole("Administrator")]
            [RecordOperation(typeof(Breaker), RecordOperation.CreateNewRecord)]
            public Breaker NewBreaker()
            {
                using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
                {
                    return new TableOperations<Breaker>(connection).NewRecord();
                }
            }

            [AuthorizeHubRole("Administrator")]
            [RecordOperation(typeof(Breaker), RecordOperation.AddNewRecord)]
            public void AddNewBreaker(Breaker record)
            {
                using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
                {
                    int added = new TableOperations<Breaker>(connection).AddNewRecord(record);
                    if (added != 0)
                    {
                        int index = connection.ExecuteScalar<int?>("SELECT IDENT_CURRENT('Asset')") ?? 0;
                        record.ID = index;
                    }
                }
            }

            [AuthorizeHubRole("Administrator, Owner")]
            [RecordOperation(typeof(Breaker), RecordOperation.UpdateRecord)]
            public void UpdateBreaker(Breaker record)
            {
                using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
                {
                    new TableOperations<Breaker>(connection).UpdateRecord(record);
                }
            }

            [AuthorizeHubRole("Administrator")]
            [RecordOperation(typeof(Breaker), RecordOperation.DeleteRecord)]
            public void DeleteBreaker(int id)
            {
                CascadeDelete("Asset", $"ID = {id}");
            }

            public int QueryLineBreakersCount(int lineID)
            {
                using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
                {
                    return new TableOperations<EDNAPoint>(connection).QueryRecordsWhere("LineID = {0}", lineID).ToList().Count();
                }
            }

            public IEnumerable<EDNAPoint> QueryLineBreakers(int lineID)
            {
                using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
                {
                    return new TableOperations<EDNAPoint>(connection).QueryRecordsWhere("LineID = {0}", lineID).ToList();
                }
            }

            public void DeleteLineBreaker(int id)
            {
                using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
                {
                    new TableOperations<EDNAPoint>(connection).DeleteRecord(new RecordRestriction("ID = {0}", id));
                }
            }

            public EDNAPoint AddNewLineBreaker(EDNAPoint record)
            {
                using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
                {
                    new TableOperations<EDNAPoint>(connection).AddNewRecord(record);
                    return new TableOperations<EDNAPoint>(connection).QueryRecordWhere("BreakerID = {0} AND Point = {1}", record.BreakerID, record.Point);
                }
            }

        #endregion

        #region [Line Overview]

            [RecordOperation(typeof(LineView), RecordOperation.QueryRecordCount)]
            public int QueryLineCount(string filterString)
            {
                using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
                {
                    return new TableOperations<LineView>(connection).QueryRecordCount(filterString);
                }
            }

            [AuthorizeHubRole("Administrator")]
            [RecordOperation(typeof(LineView), RecordOperation.QueryRecords)]
            public IEnumerable<LineView> QueryLine(string sortField, bool ascending, int page, int pageSize, string filterString)
            {
                using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
                {
                    return new TableOperations<LineView>(connection).QueryRecords(sortField, ascending, page, pageSize, filterString).ToList();
                }
            }

            [AuthorizeHubRole("Administrator")]
            [RecordOperation(typeof(LineView), RecordOperation.CreateNewRecord)]
            public LineView NewLine()
            {
                using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
                {
                    return new TableOperations<LineView>(connection).NewRecord();
                }
            }

            [AuthorizeHubRole("Administrator, Owner")]
            [RecordOperation(typeof(LineView), RecordOperation.UpdateRecord)]
            public void UpdateLine(LineView record)
            {
                using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
                {

                    new TableOperations<Line>(connection).UpdateRecord(CreateLine(record));
                }
            }

            private Line CreateLine(LineView record)
            {
                Line line = new Line();
                line.ID = record.ID;
                line.AssetKey = record.AssetKey;
                line.Description = record.Description;
                line.AssetTypeID = (int)AssetType.Line;

                line.AssetName = record.AssetName;
                line.VoltageKV = record.VoltageKV;
                line.MaxFaultDistance = record.MaxFaultDistance;
                line.MinFaultDistance = record.MinFaultDistance;

                return line;
            }

            [AuthorizeHubRole("Administrator")]
            [RecordOperation(typeof(LineView), RecordOperation.AddNewRecord)]
            public void AddNewLineView(LineView record)
            {
                using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
                {
                    int added = new TableOperations<Line>(connection).AddNewRecord(CreateLine(record));
                    if (added != 0)
                    {
                        int index = connection.ExecuteScalar<int?>("SELECT IDENT_CURRENT('Asset')") ?? 0;
                        record.ID = index;
                    }
                }
            }

            [AuthorizeHubRole("Administrator")]
            [RecordOperation(typeof(LineView), RecordOperation.DeleteRecord)]
            public void DeleteLine(int id)
            {
                CascadeDelete("Asset", $"ID = {id}");
            }

        #endregion

        #region [LineSegment Overview]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(LineSegment), RecordOperation.QueryRecordCount)]
        public int QueryLineSegmentCount(int lineID, string filterString)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                TableOperations<LineSegment> tableOperations = new TableOperations<LineSegment>(connection);
                RecordRestriction restriction = tableOperations.GetSearchRestriction(filterString);

                if (lineID > 0)
                {
                    List<int> connectionID = new TableOperations<AssetConnection>(connection).
                        QueryRecordsWhere("(ParentID = {0} OR ChildID = {0}) AND AssetRelationshipTypeID = 1", lineID).
                        Select(item =>
                        {
                            if (item.ParentID == lineID)
                                return item.ChildID;
                            return item.ParentID;
                        }).ToList();

                    //Because we are looking through the Line Segment View it does not matter if there are other
                    // Assets caught with this restriction.
                    restriction = restriction + new RecordRestriction("ID in ({0})", string.Join(",", connectionID));
                }
                return tableOperations.QueryRecordCount(restriction);
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(LineSegment), RecordOperation.QueryRecords)]
        public IEnumerable<LineSegment> QueryLineSegment(int lineID, string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                TableOperations<LineSegment> tableOperations = new TableOperations<LineSegment>(connection);
                RecordRestriction restriction = tableOperations.GetSearchRestriction(filterString);

                if (lineID > 0)
                {
                    List<int> connectionID = new TableOperations<AssetConnection>(connection).
                        QueryRecordsWhere("(ParentID = {0} OR ChildID = {0}) AND AssetRelationshipTypeID = 1", lineID).
                        Select(item =>
                        {
                            if (item.ParentID == lineID)
                                return item.ChildID;
                            return item.ParentID;
                        }).ToList();

                    restriction = restriction + new RecordRestriction("ID in ({0})", string.Join(",", connectionID));
                }

                return tableOperations.QueryRecords(sortField, ascending, page, pageSize, restriction).ToList();
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(LineSegment), RecordOperation.CreateNewRecord)]
        public LineSegment NewLineSegment()
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                return new TableOperations<LineSegment>(connection).NewRecord();
            }
        }

        [AuthorizeHubRole("Administrator, Owner")]
        [RecordOperation(typeof(LineSegment), RecordOperation.UpdateRecord)]
        public void UpdateLineSegment(LineSegment record)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                new TableOperations<LineSegment>(connection).UpdateRecord(record);
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(LineSegment), RecordOperation.AddNewRecord)]
        public void AddNewLineSegment(LineSegment record)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                int added = new TableOperations<LineSegment>(connection).AddNewRecord(record);
                if (added != 0)
                {
                    int index = connection.ExecuteScalar<int?>("SELECT IDENT_CURRENT('Asset')") ?? 0;
                    record.ID = index;

                }
            }
            
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(LineSegment), RecordOperation.DeleteRecord)]
        public void DeleteLineSegment(int id)
        {
            CascadeDelete("Asset", $"ID = {id}");
        }

        #endregion

        #region [Bus Overview]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Bus), RecordOperation.QueryRecordCount)]
        public int QueryBusCount(string filterString)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                return new TableOperations<Bus>(connection).QueryRecordCount(filterString);
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Bus), RecordOperation.QueryRecords)]
        public IEnumerable<Bus> QueryBus(string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                return new TableOperations<Bus>(connection).QueryRecords(sortField, ascending, page, pageSize, filterString).ToList();
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Bus), RecordOperation.CreateNewRecord)]
        public Bus NewBus()
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                return new TableOperations<Bus>(connection).NewRecord();
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Bus), RecordOperation.AddNewRecord)]
        public void AddNewBus(Bus record)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                int added = new TableOperations<Bus>(connection).AddNewRecord(record);
                if (added != 0)
                {
                    int index = connection.ExecuteScalar<int?>("SELECT IDENT_CURRENT('Asset')") ?? 0;
                    record.ID = index;
                }
            }
        }

        [AuthorizeHubRole("Administrator, Owner")]
        [RecordOperation(typeof(Bus), RecordOperation.UpdateRecord)]
        public void UpdateBus(Bus record)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                new TableOperations<Bus>(connection).UpdateRecord(record);
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Bus), RecordOperation.DeleteRecord)]
        public void DeleteBus(int id)
        {
            CascadeDelete("Asset", $"ID = {id}");
        }

        #endregion

        #region [CapBank Overview]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(CapBank), RecordOperation.QueryRecordCount)]
        public int QueryCapBankCount(string filterString)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                return new TableOperations<CapBank>(connection).QueryRecordCount(filterString);
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(CapBank), RecordOperation.QueryRecords)]
        public IEnumerable<CapBank> QueryCapBank(string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                return new TableOperations<CapBank>(connection).QueryRecords(sortField, ascending, page, pageSize, filterString).ToList();
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(CapBank), RecordOperation.CreateNewRecord)]
        public CapBank NewCapBank()
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                return new TableOperations<CapBank>(connection).NewRecord();
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(CapBank), RecordOperation.AddNewRecord)]
        public void AddNewCapBank(CapBank record)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                int added = new TableOperations<CapBank>(connection).AddNewRecord(record);
                if (added != 0)
                {
                    int index = connection.ExecuteScalar<int?>("SELECT IDENT_CURRENT('Asset')") ?? 0;
                    record.ID = index;
                }
            }
        }

        [AuthorizeHubRole("Administrator, Owner")]
        [RecordOperation(typeof(CapBank), RecordOperation.UpdateRecord)]
        public void UpdateCapBank(CapBank record)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                new TableOperations<CapBank>(connection).UpdateRecord(record);
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(CapBank), RecordOperation.DeleteRecord)]
        public void DeleteCapBank(int id)
        {
            CascadeDelete("Asset", $"ID = {id}");
        }
        #endregion

        #region [Transformer Overview]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Transformer), RecordOperation.QueryRecordCount)]
        public int QueryTransformerCount(string filterString)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                return new TableOperations<Transformer>(connection).QueryRecordCount(filterString);
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Transformer), RecordOperation.QueryRecords)]
        public IEnumerable<Transformer> QueryTransformer(string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                return new TableOperations<Transformer>(connection).QueryRecords(sortField, ascending, page, pageSize, filterString).ToList();
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Transformer), RecordOperation.CreateNewRecord)]
        public Transformer NewTransformer()
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                return new TableOperations<Transformer>(connection).NewRecord();
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Transformer), RecordOperation.AddNewRecord)]
        public void AddNewTransformer(Transformer record)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                int added = new TableOperations<Transformer>(connection).AddNewRecord(record);
                if (added != 0)
                {
                    int index = connection.ExecuteScalar<int?>("SELECT IDENT_CURRENT('Asset')") ?? 0;
                    record.ID = index;
                }
            }
        }

        [AuthorizeHubRole("Administrator, Owner")]
        [RecordOperation(typeof(Transformer), RecordOperation.UpdateRecord)]
        public void UpdateTransformer(Transformer record)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                new TableOperations<Transformer>(connection).UpdateRecord(record);
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Transformer), RecordOperation.DeleteRecord)]
        public void DeleteTransformer(int id)
        {
            CascadeDelete("Asset", $"ID = {id}");
        }

        #endregion

        #region [MeterAssets Overview]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(MeterAsset), RecordOperation.QueryRecordCount)]
        public int QueryMeterAssetCount(int assetID, int meterID, string filterString)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                TableOperations<MeterAsset> tableOperations = new TableOperations<MeterAsset>(connection);
                RecordRestriction restriction = tableOperations.GetSearchRestriction(filterString);

                if (assetID > 0)
                    restriction = restriction + new RecordRestriction("AssetID = {0}", assetID);
                if (meterID > 0)
                    restriction = restriction + new RecordRestriction("MeterID = {0}", meterID);

                return tableOperations.QueryRecordCount(restriction);
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(MeterAsset), RecordOperation.QueryRecords)]
        public IEnumerable<MeterAssetDetail> QueryMeterAsset(int assetID, int meterID, string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                TableOperations<MeterAssetDetail> tableOperations = new TableOperations<MeterAssetDetail>(connection);
                RecordRestriction restriction = tableOperations.GetSearchRestriction(filterString);

                if (assetID > 0)
                    restriction = restriction + new RecordRestriction("AssetID = {0}", assetID);
                if (meterID > 0)
                    restriction = restriction + new RecordRestriction("MeterID = {0}", meterID);

                return tableOperations.QueryRecords(sortField, ascending, page, pageSize, restriction).ToList();
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(MeterAsset), RecordOperation.DeleteRecord)]
        public void DeleteMeterAsset(int id)
        {
            CascadeDelete("MeterAsset", $"ID = {id}");
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(MeterAsset), RecordOperation.CreateNewRecord)]
        public MeterAssetDetail NewMeterAsset()
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                return new TableOperations<MeterAssetDetail>(connection).NewRecord();
            }
        }

        private MeterAsset ToMeterAsset(MeterAssetDetail record)
        {
            MeterAsset result = new MeterAsset();
            result.ID = record.ID;
            result.MeterID = record.MeterID;
            result.AssetID = record.AssetID;

            return result;
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(MeterAsset), RecordOperation.AddNewRecord)]
        public void AddNewMeterAsset(MeterAssetDetail record)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                new TableOperations<MeterAsset>(connection).AddNewRecord(ToMeterAsset(record));

                if (record.FaultDetectionLogic != null)
                {
                    int? id = connection.ExecuteScalar<int?>("SELECT @@IDENTITY");
                    if (id != null)
                        new TableOperations<FaultDetectionLogic>(connection).AddNewRecord(new FaultDetectionLogic() { MeterAssetID = (int)id, Expression = record.FaultDetectionLogic });
                }

            }

        }

        [AuthorizeHubRole("Administrator, Owner")]
        [RecordOperation(typeof(MeterAsset), RecordOperation.UpdateRecord)]
        public void UpdateMeterAsset(MeterAssetDetail record)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                new TableOperations<MeterAsset>(connection).UpdateRecord(ToMeterAsset(record));
                FaultDetectionLogic logic = new TableOperations<FaultDetectionLogic>(connection).QueryRecordWhere("MeterAssetID = {0}", record.ID);

                if (record.FaultDetectionLogic != null && record.FaultDetectionLogic != string.Empty && logic == null)
                {

                    new TableOperations<FaultDetectionLogic>(connection).AddNewRecord(new FaultDetectionLogic() { MeterAssetID = record.ID, Expression = record.FaultDetectionLogic });
                }
                else if (record.FaultDetectionLogic != null && record.FaultDetectionLogic != string.Empty && logic != null)
                {
                    logic.Expression = record.FaultDetectionLogic;
                    new TableOperations<FaultDetectionLogic>(connection).UpdateRecord(logic);
                }
                else if ((record.FaultDetectionLogic == null || record.FaultDetectionLogic == string.Empty) && logic != null)
                {
                    new TableOperations<FaultDetectionLogic>(connection).DeleteRecord(logic);
                }

            }
        }

        #endregion

        #region [AssetConnections Overview]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(AssetConnection), RecordOperation.QueryRecordCount)]
        public int QueryAssetConnectionCount(int assetID, string filterString)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                TableOperations<AssetConnection> tableOperations = new TableOperations<AssetConnection>(connection);
                RecordRestriction restriction = tableOperations.GetSearchRestriction(filterString);

                if (assetID > 0)
                    restriction = restriction + new RecordRestriction("ChildID = {0} OR ParentID = {0}", assetID);
                

                return tableOperations.QueryRecordCount(restriction);
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(AssetConnection), RecordOperation.QueryRecords)]
        public IEnumerable<AssetConnectionDetail> QueryMeterAssetConnection(int assetID, string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                TableOperations<AssetConnectionDetail> tableOperations = new TableOperations<AssetConnectionDetail>(connection);
                RecordRestriction restriction = tableOperations.GetSearchRestriction(filterString);

                if (assetID > 0)
                    restriction = restriction + new RecordRestriction("ChildID = {0} OR ParentID = {0}", assetID);


                return tableOperations.QueryRecords(sortField, ascending, page, pageSize, restriction).ToList();
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(AssetConnection), RecordOperation.DeleteRecord)]
        public void DeleteAssetConnection(int id)
        {
            CascadeDelete("AssetConnection", $"ID = {id}");
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(AssetConnection), RecordOperation.CreateNewRecord)]
        public AssetConnectionDetail NewAssetConnection()
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                return new TableOperations<AssetConnectionDetail>(connection).NewRecord();
            }
        }

        private AssetConnection ToAssetConnection(AssetConnectionDetail record)
        {
            AssetConnection result = new AssetConnection();
            result.ID = record.ID;
            result.ParentID = record.ParentID;
            result.ChildID = record.ChildID;
            result.AssetRelationshipTypeID = record.AssetRelationshipTypeID;

            return result;
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(AssetConnection), RecordOperation.AddNewRecord)]
        public void AddNewAssetConnection(AssetConnectionDetail record)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                new TableOperations<AssetConnection>(connection).AddNewRecord(ToAssetConnection(record));
            }

        }

        [AuthorizeHubRole("Administrator, Owner")]
        [RecordOperation(typeof(AssetConnection), RecordOperation.UpdateRecord)]
        public void UpdateAssetConnection(AssetConnectionDetail record)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                new TableOperations<AssetConnection>(connection).UpdateRecord(ToAssetConnection(record));
            }
        }
        #endregion

        #region [ Location  Overview ]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Location), RecordOperation.QueryRecordCount)]
        public int QueryLocationCount(string filterString)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                return new TableOperations<Location>(connection).QueryRecordCount(filterString);
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Location), RecordOperation.QueryRecords)]
        public IEnumerable<Location> QueryLocations(string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                return new TableOperations<Location>(connection).QueryRecords(sortField, ascending, page, pageSize, filterString).ToList();
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Location), RecordOperation.DeleteRecord)]
        public void DeleteLocation(int id)
        {
            CascadeDelete("Location", $"ID = {id}");
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Location), RecordOperation.CreateNewRecord)]
        public Location NewLocation()
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                return new TableOperations<Location>(connection).NewRecord();
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Location), RecordOperation.AddNewRecord)]
        public void AddNewLocation(Location record)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                new TableOperations<Location>(connection).AddNewRecord(record);
            }
        }

        [AuthorizeHubRole("Administrator, Owner")]
        [RecordOperation(typeof(Location), RecordOperation.UpdateRecord)]
        public void UpdateLocation(Location record)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                new TableOperations<Location>(connection).UpdateRecord(record);
            }
        }

        [AuthorizeHubRole("Administrator")]
        public Dictionary<string, string> SearchLocations(string searchText, int limit = -1)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                RecordRestriction restriction = new RecordRestriction("Name LIKE {0}", $"%{searchText}%");
                return new TableOperations<Location>(connection).QueryRecords("Name", restriction, limit).ToDictionary(x => x.ID.ToString(), x => x.Name);
            }
        }

        #endregion

        #region [ Meter Table Operations ]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Meter), RecordOperation.QueryRecordCount)]
        public int QueryMeterCount(int meterLocationID, string filterString)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                TableOperations<MeterDetail> tableOperations = new TableOperations<MeterDetail>(connection);
                RecordRestriction restriction;
                if (meterLocationID > 0)
                    restriction = tableOperations.GetSearchRestriction(filterString) + new RecordRestriction("LocationID = {0}", meterLocationID);
                else
                    restriction = tableOperations.GetSearchRestriction(filterString);

                return tableOperations.QueryRecordCount(restriction);
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Meter), RecordOperation.QueryRecords)]
        public IEnumerable QueryMeters(int meterLocationID, string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                TableOperations<MeterDetail> tableOperations = new TableOperations<MeterDetail>(connection);
                RecordRestriction restriction;
                if (meterLocationID > 0)
                    restriction = tableOperations.GetSearchRestriction(filterString) + new RecordRestriction("MeterLocationID = {0}", meterLocationID);
                else
                    restriction = tableOperations.GetSearchRestriction(filterString);

                IEnumerable<MeterDetail> meters = tableOperations.QueryRecords(sortField, ascending, page, pageSize, restriction);

                if (pageSize > 25)
                    return meters.ToList();

                List<JObject> jMeters = meters.Select(meter => JObject.FromObject(meter)).ToList();
                TableOperations<MaintenanceWindow> maintenanceWindowTable = new TableOperations<MaintenanceWindow>(connection);

                Dictionary<int, MaintenanceWindow> maintenanceWindows = maintenanceWindowTable
                    .QueryRecords()
                    .GroupBy(window => window.MeterID)
                    .ToDictionary(grouping => grouping.Key, grouping => grouping.First());

                foreach (dynamic meter in jMeters)
                {
                    int meterID = meter.ID;

                    if (maintenanceWindows.TryGetValue(meterID, out MaintenanceWindow window))
                        meter.MaintenanceWindow = JObject.FromObject(window);
                }

                return jMeters;
            }
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
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                return new TableOperations<MeterDetail>(connection).NewRecord();
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Meter), RecordOperation.AddNewRecord)]
        public void AddNewMeter(Meter record)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                new TableOperations<Meter>(connection).AddNewRecord(record);
            }
        }

        [AuthorizeHubRole("Administrator, Owner")]
        [RecordOperation(typeof(Meter), RecordOperation.UpdateRecord)]
        public void UpdateMeter(Meter record)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                new TableOperations<Meter>(connection).UpdateRecord(record);
            }
        }

        [AuthorizeHubRole("Administrator")]
        public IEnumerable<IDLabel> SearchMeters(string searchText, int limit = -1)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                RecordRestriction restriction = new RecordRestriction("Name LIKE {0}", $"%{searchText}%");

                return new TableOperations<Meter>(connection).QueryRecords("Name", restriction, limit)
                    .Select(meter => new IDLabel(meter.ID.ToString(), meter.Name)).ToList();
            }
        }

        [AuthorizeHubRole("Administrator")]
        public IEnumerable<IDLabel> SearchMetersByGroup(int groupID, string searchText, int limit = -1)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                RecordRestriction restriction = new RecordRestriction("Name LIKE {0} AND ID NOT IN (SELECT MeterID FROM GroupMeter WHERE GroupID = {1})", $"%{searchText}%", groupID);

                return new TableOperations<Meter>(connection).QueryRecords("Name", restriction, limit)
                    .Select(meter => new IDLabel(meter.ID.ToString(), meter.Name)).ToList();
            }
        }

        #endregion

        #region[Customer Table Operations]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Customer), RecordOperation.QueryRecordCount)]
        public int QueryCustomerCount(string filterString)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                return (new TableOperations<Customer>(connection)).QueryRecordCount(filterString);
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Customer), RecordOperation.QueryRecords)]
        public IEnumerable<Customer> QueryCustomer(string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                return new TableOperations<Customer>(connection).QueryRecords(sortField, ascending, page, pageSize, filterString).ToList();
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Customer), RecordOperation.DeleteRecord)]
        public void DeleteCustomer(int id)
        {
            CascadeDelete("Customer", $"ID = {id}");
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Customer), RecordOperation.CreateNewRecord)]
        public Customer NewCustomer()
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                return new TableOperations<Customer>(connection).NewRecord();
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Customer), RecordOperation.AddNewRecord)]
        public void AddNewCustomer(Customer record)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                new TableOperations<Customer>(connection).AddNewRecord(record);
            }
        }

        [AuthorizeHubRole("Administrator, Owner")]
        [RecordOperation(typeof(Customer), RecordOperation.UpdateRecord)]
        public void UpdateCustomer(Customer record)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                new TableOperations<Customer>(connection).UpdateRecord(record);
            }
        }

        #endregion

        #region[CustomerAsset Table Operations]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(CustomerAsset), RecordOperation.QueryRecordCount)]
        public int QueryCustomerAssetCount(int customerID, int assetID, string filterString)
        {

            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                RecordRestriction restriction;
                TableOperations<CustomerAsset> tableOperations = new TableOperations<CustomerAsset>(connection);

                restriction = tableOperations.GetSearchRestriction(filterString);

                if (customerID > 0)
                    restriction = restriction + new RecordRestriction("CustomerID = {0}", customerID);
                if (assetID > 0)
                    restriction = restriction + new RecordRestriction("AssetID = {0}", assetID);
                
                return (new TableOperations<CustomerAsset>(connection)).QueryRecordCount(restriction);
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(CustomerAsset), RecordOperation.QueryRecords)]
        public IEnumerable<CustomerAssetDetail> QueryCustomerAsset(int customerID, int assetID, string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                RecordRestriction restriction;
                TableOperations<CustomerAssetDetail> tableOperations = new TableOperations<CustomerAssetDetail>(connection);

                restriction = tableOperations.GetSearchRestriction(filterString);

                if (customerID > 0)
                    restriction = restriction + new RecordRestriction("CustomerID = {0}", customerID);
                if (assetID > 0)
                    restriction = restriction + new RecordRestriction("AssetID = {0}", assetID);

                return tableOperations.QueryRecords(sortField, ascending, page, pageSize, restriction).ToList();
            }
        }

        [AuthorizeHubRole("Administrator, Owner")]
        [RecordOperation(typeof(CustomerAsset), RecordOperation.UpdateRecord)]
        public void UpdateCustomerAsset(CustomerAssetDetail record)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                new TableOperations<CustomerAsset>(connection).UpdateRecord(CreateCustomerAsset(record));
            }
        }

        private CustomerAsset CreateCustomerAsset(CustomerAssetDetail record)
        {
            CustomerAsset customerAsset = NewCustomerAsset();
            customerAsset.ID = record.ID;
            customerAsset.AssetID = record.AssetID;
            customerAsset.CustomerID = record.CustomerID;

            return customerAsset;
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(CustomerAsset), RecordOperation.AddNewRecord)]
        public void AddNewCustomerAsset(CustomerAssetDetail record)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                new TableOperations<CustomerAsset>(connection).AddNewRecord(CreateCustomerAsset(record));
                
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(CustomerAsset), RecordOperation.DeleteRecord)]
        public void DeleteCustomerAsset(int id)
        {
            CascadeDelete("CustomerAsset", $"ID = {id}");
        }


        private CustomerAsset NewCustomerAsset()
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                return new TableOperations<CustomerAsset>(connection).NewRecord();
            }

        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(CustomerAsset), RecordOperation.CreateNewRecord)]
        public CustomerAssetDetail NewCustomerAssetView()
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                return new TableOperations<CustomerAssetDetail>(connection).NewRecord();
            }

        }


        #endregion

        #region[AssetSpare Table Operations]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(AssetSpare), RecordOperation.QueryRecordCount)]
        public int QueryAssetSpareCount(string filterString)
        {

            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                return (new TableOperations<AssetSpare>(connection)).QueryRecordCount(filterString);
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(AssetSpare), RecordOperation.QueryRecords)]
        public IEnumerable<AssetSpareView> QueryAssetSpare(string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
    
                return (new TableOperations<AssetSpareView>(connection)).QueryRecords(sortField, ascending, page, pageSize, filterString).ToList();
            }
        }

        [AuthorizeHubRole("Administrator, Owner")]
        [RecordOperation(typeof(AssetSpare), RecordOperation.UpdateRecord)]
        public void UpdateAssetSpare(AssetSpareView record)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                new TableOperations<AssetSpare>(connection).UpdateRecord(CreateAssetSpare(record));
            }
        }

        private AssetSpare CreateAssetSpare(AssetSpareView record)
        {
            AssetSpare customerAsset = NewAssetSpare();
            customerAsset.ID = record.ID;
            customerAsset.AssetID = record.AssetID;
            customerAsset.SpareAssetID = record.SpareAssetID;

            return customerAsset;
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(AssetSpare), RecordOperation.AddNewRecord)]
        public void AddNewAssetSpare(AssetSpareView record)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                new TableOperations<AssetSpare>(connection).AddNewRecord(CreateAssetSpare(record));

            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(AssetSpare), RecordOperation.DeleteRecord)]
        public void DeleteAssetSpare(int id)
        {
            CascadeDelete("AsssetSpare", $"ID = {id}");
        }


        private AssetSpare NewAssetSpare()
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                return new TableOperations<AssetSpare>(connection).NewRecord();
            }

        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(AssetSpare), RecordOperation.CreateNewRecord)]
        public AssetSpareView NewAssetSpareView()
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                return new TableOperations<AssetSpareView>(connection).NewRecord();
            }

        }


        #endregion

        // Legacy Line Table Operations
        // I am not sure these are necesarry but don't want to remove them
        // until checking with Steven
        #region [ Lines Table Operations ]


        [AuthorizeHubRole("Administrator")]
        public Dictionary<string,string> SearchLines(string searchText, int limit = -1)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                RecordRestriction restriction = new RecordRestriction("AssetKey LIKE {0}", $"%{searchText}%");
                return new TableOperations<Line>(connection).QueryRecords("AssetKey", restriction, limit).ToDictionary(x => x.ID.ToString(), x => x.AssetKey);
            }
        }

        [AuthorizeHubRole("Administrator")]
        public Dictionary<string, string> SearchLinesByGroup(int groupID, string searchText, int limit = -1)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                RecordRestriction restriction = new RecordRestriction("AssetKey LIKE {0} AND ID NOT IN (SELECT LineID FROM LineAssetGroup WHERE AssetGroupID = {1})", $"%{searchText}%", groupID);
                return new TableOperations<Meter>(connection).QueryRecords("AssetKey", restriction, limit).ToDictionary(x => x.ID.ToString(), x => x.AssetKey);
            }
        }


        #endregion



        #region [ Channel Table Operations ]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Channel), RecordOperation.QueryRecordCount)]
        public int QueryChannelCount(int meterID, int lineID, string filterString)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                TableOperations<ChannelDetail> tableOperations = new TableOperations<ChannelDetail>(connection);
                RecordRestriction restriction;

                if (meterID != -1 && lineID == -1)
                    restriction = tableOperations.GetSearchRestriction(filterString) + new RecordRestriction("MeterID = {0} ", meterID);
                else if (meterID == -1 && lineID != -1)
                    restriction = tableOperations.GetSearchRestriction(filterString) + new RecordRestriction("AssetID = {0}", lineID);
                else
                    restriction = tableOperations.GetSearchRestriction(filterString) + new RecordRestriction("MeterID = {0} AND AssetID = {1}", meterID, lineID);

                return tableOperations.QueryRecordCount(restriction);
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Channel), RecordOperation.QueryRecords)]
        public IEnumerable<ChannelDetail> QueryChannel(int meterID, int lineID, string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                TableOperations<ChannelDetail> tableOperations = new TableOperations<ChannelDetail>(connection);
                RecordRestriction restriction;

                if (meterID != -1 && lineID == -1)
                    restriction = tableOperations.GetSearchRestriction(filterString) + new RecordRestriction("MeterID = {0} ", meterID);
                else if (meterID == -1 && lineID != -1)
                    restriction = tableOperations.GetSearchRestriction(filterString) + new RecordRestriction("AssetID = {0}", lineID);
                else
                    restriction = tableOperations.GetSearchRestriction(filterString) + new RecordRestriction("MeterID = {0} AND AssetID = {1}", meterID, lineID);

                return tableOperations.QueryRecords(sortField, ascending, page, pageSize, restriction).ToList();
            }
        }

        public IEnumerable<Channel> QueryChannelsForDropDown(string filterString, int meterID)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                return new TableOperations<Channel>(connection).QueryRecords(restriction: new RecordRestriction("Name LIKE {0} AND MeterID = {1}", filterString, meterID), limit: 50).ToList();
            }
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
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                return new TableOperations<ChannelDetail>(connection).NewRecord();
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Channel), RecordOperation.AddNewRecord)]
        public void AddNewChannel(ChannelDetail record)
        {
            using(AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                new TableOperations<Channel>(connection).AddNewRecord(record);
                record.ID = connection.ExecuteScalar<int>("SELECT @@IDENTITY");

                if (!string.IsNullOrEmpty(record.Mapping))
                {
                    connection.ExecuteNonQuery("INSERT INTO Series VALUES({0}, {1}, {2})", record.ID, record.SeriesTypeID, record.Mapping);
                }
            }
        }

        [AuthorizeHubRole("Administrator, Owner")]
        [RecordOperation(typeof(Channel), RecordOperation.UpdateRecord)]
        public void UpdateChannel(ChannelDetail record)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                new TableOperations<Channel>(connection).UpdateRecord(record);

                if (!string.IsNullOrEmpty(record.Mapping))
                {
                    bool seriesExists = connection.ExecuteScalar<int>("SELECT COUNT(*) FROM Series WHERE ChannelID = {0} AND SeriesTypeID = {1}", record.ID, record.SeriesTypeID) > 0;

                    if (seriesExists)
                        connection.ExecuteNonQuery("UPDATE Series SET SourceIndexes = {0} WHERE ChannelID = {1} AND SeriesTypeID = {2}", record.Mapping, record.ID, record.SeriesTypeID);
                    else
                        connection.ExecuteNonQuery("INSERT INTO Series VALUES({0}, {1}, {2})", record.ID, record.SeriesTypeID, record.Mapping);
                }
                else
                {
                    connection.ExecuteNonQuery("UPDATE Series SET SourceIndexes = '' WHERE ChannelID = {0} AND SeriesTypeID = {1}", record.ID, record.SeriesTypeID);
                }
            }
        }

        [AuthorizeHubRole("Administrator")]
        public Dictionary<string,string> SearchMeasurementTypes(string searchText, int limit = -1)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                string limitText = (limit > 0) ? $"TOP {limit}" : "";

                return connection
                    .RetrieveData($"SELECT {limitText} ID, Name FROM MeasurementType WHERE Name LIKE {{0}}", $"%{searchText}%")
                    .Select()
                    .ToDictionary(row => row["ID"].ToString(), row => row["Name"].ToString());
            }
        }

        [AuthorizeHubRole("Administrator")]
        public Dictionary<string, string> SearchMeasurementCharacteristics(string searchText, int limit = -1)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                string limitText = (limit > 0) ? $"TOP {limit}" : "";

                return connection
                    .RetrieveData($"SELECT {limitText} ID, Name FROM MeasurementCharacteristic WHERE Name LIKE {{0}}", $"%{searchText}%")
                    .Select()
                    .ToDictionary(row => row["ID"].ToString(), row => row["Name"].ToString());
            }
        }

        [AuthorizeHubRole("Administrator")]
        public Dictionary<string, string> SearchPhases(string searchText, int limit = -1)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                string limitText = (limit > 0) ? $"TOP {limit}" : "";

                return connection
                    .RetrieveData($"SELECT {limitText} ID, Name FROM Phase WHERE Name LIKE {{0}}", $"%{searchText}%")
                    .Select()
                    .ToDictionary(row => row["ID"].ToString(), row => row["Name"].ToString());
            }
        }

        #endregion

        #region [ MaintenanceWindow Table Operations ]

        public void SetMaintenanceWindow(int meterID, DateTime? startTime, DateTime? endTime)
        {
            MaintenanceWindow maintenanceWindow = new MaintenanceWindow()
            {
                MeterID = meterID,
                StartTime = startTime,
                EndTime = endTime
            };

            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                TableOperations<MaintenanceWindow> maintenanceWindowTable = new TableOperations<MaintenanceWindow>(connection);
                maintenanceWindowTable.DeleteRecordWhere("MeterID = {0}", meterID);
                maintenanceWindowTable.AddNewRecord(maintenanceWindow);
            }
        }

        public void ClearMaintenanceWindow(int meterID)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                TableOperations<MaintenanceWindow> maintenanceWindowTable = new TableOperations<MaintenanceWindow>(connection);
                maintenanceWindowTable.DeleteRecordWhere("MeterID = {0}", meterID);
            }
        }

        #endregion

        #endregion

        #region [Workbench Page]

        #region [Filters Operations]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(WorkbenchFilter), RecordOperation.QueryRecordCount)]
        public int QueryWorkbenchFilterCount(string filterString)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                return new TableOperations<WorkbenchFilter>(connection).QueryRecordCount(new RecordRestriction("UserID = {0}", GetCurrentUserID()));
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(WorkbenchFilter), RecordOperation.QueryRecords)]
        public IEnumerable<WorkbenchFilter> QueryWorkbenchFilters(string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                return new TableOperations<WorkbenchFilter>(connection).QueryRecords(sortField, ascending, page, pageSize, new RecordRestriction("UserID = {0}", GetCurrentUserID())).ToList();
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(WorkbenchFilter), RecordOperation.DeleteRecord)]
        public void DeleteWorkbenchFilter(int id)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                TableOperations<WorkbenchFilter> table = new TableOperations<WorkbenchFilter>(connection);
                WorkbenchFilter record = table.QueryRecords(restriction: new RecordRestriction("ID = {0}", id)).First();

                table.DeleteRecord(id);
                if (record.IsDefault)
                {
                    IEnumerable<WorkbenchFilter> wbfs = table.QueryRecords(restriction: new RecordRestriction("UserID = {0}", GetCurrentUserID()));
                    if (wbfs.Any())
                    {
                        WorkbenchFilter wbf = wbfs.First();
                        wbf.IsDefault = !wbf.IsDefault;
                        table.UpdateRecord(wbf);

                    }
                }
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(WorkbenchFilter), RecordOperation.CreateNewRecord)]
        public WorkbenchFilter NewWorkbenchFilter()
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                return new TableOperations<WorkbenchFilter>(connection).NewRecord();
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(WorkbenchFilter), RecordOperation.AddNewRecord)]
        public void AddNewWorkbenchFilter(WorkbenchFilter record)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                TableOperations<WorkbenchFilter> table = new TableOperations<WorkbenchFilter>(connection);

                record.UserID = GetCurrentUserID();
                IEnumerable<WorkbenchFilter> wbfs = table.QueryRecords(restriction: new RecordRestriction("UserID = {0}", GetCurrentUserID()));
                if (!wbfs.Any())
                {
                    record.IsDefault = true;
                    table.AddNewRecord(record);
                    return;
                }

                if (record.IsDefault)
                {


                    foreach (WorkbenchFilter wbf in wbfs)
                    {
                        if (wbf.IsDefault)
                        {
                            wbf.IsDefault = !wbf.IsDefault;
                            table.UpdateRecord(wbf);
                        }
                    }
                }

                table.AddNewRecord(record);
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(WorkbenchFilter), RecordOperation.UpdateRecord)]
        public void UpdateWorkbenchFilter(WorkbenchFilter record)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                TableOperations<WorkbenchFilter> table = new TableOperations<WorkbenchFilter>(connection);

                if (record.IsDefault)
                {
                    IEnumerable<WorkbenchFilter> wbfs = table.QueryRecords(restriction: new RecordRestriction("UserID = {0}", GetCurrentUserID()));

                    foreach (WorkbenchFilter wbf in wbfs)
                    {
                        if (wbf.IsDefault)
                        {
                            wbf.IsDefault = !wbf.IsDefault;
                            table.UpdateRecord(wbf);
                        }
                    }
                }

                table.UpdateRecord(record);
            }
        }

        public IEnumerable<WorkbenchFilter> GetWorkbenchFiltersForSelect()
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                return new TableOperations<WorkbenchFilter>(connection).QueryRecords(restriction: new RecordRestriction("UserID = {0}", GetCurrentUserID())).ToList();
            }
        } 


        public IEnumerable<EventType> GetEventTypesForSelect()
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                return new TableOperations<EventType>(connection).QueryRecords().ToList();
            }
        }

        public IEnumerable<Line> GetLinesForSelect()
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                return new TableOperations<Line>(connection).QueryRecords().ToList();
            }
        }

        public DateTime GetOldestEventDateTime()
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                return connection.ExecuteScalar<DateTime>("SELECT TOP 1 StartTime FROM [Event] ORDER BY StartTime ASC");
            }
        }

        #endregion

        #region [Events Operations]
        public int GetEventCounts(int filterId, string filterString)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                Tuple<DateTime, DateTime> dTuple = GetTimeRange(filterId);
                DateTime startDate = dTuple.Item1;
                DateTime endDate = dTuple.Item2;

                TableOperations<EventView> tableOperations = new TableOperations<EventView>(connection);
                RecordRestriction restriction = tableOperations.GetSearchRestriction(filterString) +
                    new RecordRestriction("MeterID IN (Select * FROM String_To_Int_Table((Select Meters FROM WorkbenchFilter WHERE ID = {0}), ',')) AND EventTypeID IN (Select * FROM String_To_Int_Table((Select EventTypes FROM WorkbenchFilter WHERE ID = {0}), ',')) AND StartTime >= {1} AND StartTime <= {2} ", filterId, startDate, endDate);
                return tableOperations.QueryRecordCount(restriction);
            }
        }


        public IEnumerable<EventView> GetFilteredEvents(int filterId, string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                Tuple<DateTime, DateTime> dTuple = GetTimeRange(filterId);
                DateTime startDate = dTuple.Item1;
                DateTime endDate = dTuple.Item2;
                TableOperations<EventView> tableOperations = new TableOperations<EventView>(connection);
                RecordRestriction restriction = tableOperations.GetSearchRestriction(filterString) +
                    new RecordRestriction("MeterID IN (Select * FROM String_To_Int_Table((Select Meters FROM WorkbenchFilter WHERE ID = {0}), ',')) AND EventTypeID IN (Select * FROM String_To_Int_Table((Select EventTypes FROM WorkbenchFilter WHERE ID = {0}), ',')) AND StartTime >= {1} AND StartTime <= {2} ", filterId, startDate, endDate);

                return tableOperations.QueryRecords(sortField, ascending, page, pageSize, restriction).ToList();
            }
        }

        [AuthorizeHubRole("*")]
        [RecordOperation(typeof(Event), RecordOperation.QueryRecordCount)]
        public int QueryEventCount(int filterId, string filterString)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                Tuple<DateTime, DateTime> dTuple = GetTimeRange(filterId);
                DateTime startDate = dTuple.Item1;
                DateTime endDate = dTuple.Item2;

                TableOperations<EventView> tableOperations = new TableOperations<EventView>(connection);
                RecordRestriction restriction = tableOperations.GetSearchRestriction(filterString) +
                    new RecordRestriction("(MeterID IN (Select * FROM String_To_Int_Table((Select Meters FROM WorkbenchFilter WHERE ID = {0}), ',')) OR AssetID IN (Select * FROM String_To_Int_Table((Select Lines FROM WorkbenchFilter WHERE ID = {0}), ',')) ) AND " +
                                                                                             "EventTypeID IN (Select * FROM String_To_Int_Table((Select EventTypes FROM WorkbenchFilter WHERE ID = {0}), ',')) AND " +
                                                                                             "StartTime >= {1} AND " +
                                                                                             "StartTime <= {2} ",
                                                                                             filterId, startDate, endDate);

                return tableOperations.QueryRecordCount(restriction);
            }
        }

        [AuthorizeHubRole("*")]
        [RecordOperation(typeof(Event), RecordOperation.QueryRecords)]
        public IEnumerable<EventView> QueryEvents(int filterId, string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                Tuple<DateTime, DateTime> dTuple = GetTimeRange(filterId);
                DateTime startDate = dTuple.Item1;
                DateTime endDate = dTuple.Item2;

                TableOperations<EventView> tableOperations = new TableOperations<EventView>(connection);
                RecordRestriction restriction = tableOperations.GetSearchRestriction(filterString) +
                    new RecordRestriction("(MeterID IN (Select * FROM String_To_Int_Table((Select Meters FROM WorkbenchFilter WHERE ID = {0}), ',')) OR AssetID IN (Select * FROM String_To_Int_Table((Select Lines FROM WorkbenchFilter WHERE ID = {0}), ',')) ) AND " +
                                                                                             "EventTypeID IN (Select * FROM String_To_Int_Table((Select EventTypes FROM WorkbenchFilter WHERE ID = {0}), ',')) AND " +
                                                                                             "StartTime >= {1} AND " +
                                                                                             "StartTime <= {2} ",
                                                                                             filterId, startDate, endDate);

                return tableOperations.QueryRecords(sortField, ascending, page, pageSize, restriction).ToList();
            }
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
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                return new TableOperations<EventView>(connection).NewRecord();
            }
        }

        [AuthorizeHubRole("Administrator, Engineer")]
        [RecordOperation(typeof(Event), RecordOperation.AddNewRecord)]
        public void AddNewEvent(Event record)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                new TableOperations<Event>(connection).AddNewRecord(record);
            }
        }

        [AuthorizeHubRole("Administrator, Engineer")]
        [RecordOperation(typeof(Event), RecordOperation.UpdateRecord)]
        public bool UpdateEvent(EventView record, bool propagate)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                TableOperations<Event> eventTable = new TableOperations<Event>(connection);
                TableOperations<Disturbance> disturbanceTable = new TableOperations<Disturbance>(connection);
                TableOperations<Fault> faultTable = new TableOperations<Fault>(connection);

                DateTime oldStartTime = connection.ExecuteScalar<DateTime>($"SELECT StartTime FROM Event WHERE ID = {record.ID}");
                if (oldStartTime != record.StartTime)
                {
                    // Get Time Stamp shift
                    Ticks ticks = record.StartTime - oldStartTime;
                    record.EndTime = record.EndTime.AddTicks(ticks);
                    // Update event records
                    // IF propagate is true update all associated with the file

                    if (propagate)
                    {
                        IEnumerable<Event> events = eventTable.QueryRecords(restriction: new RecordRestriction("FileGroupID = {0} AND ID <> {1}", record.FileGroupID, record.ID));
                        foreach (var e in events)
                        {
                            e.StartTime = e.StartTime.AddTicks(ticks);
                            e.EndTime = e.EndTime.AddTicks(ticks);
                            e.UpdatedBy = GetCurrentUserName();
                            eventTable.UpdateRecord(e);
                        }
                    }


                    // Update disturbance records
                    // IF propagate is true update all associated with the file
                    // if propagate is false update all assocaited with the event id
                    IEnumerable<Disturbance> disturbances;

                    if (propagate)
                        disturbances = disturbanceTable.QueryRecords(restriction: new RecordRestriction("EventID IN (Select ID from Event WHERE FileGroupID = {0})", record.FileGroupID));
                    else
                        disturbances = disturbanceTable.QueryRecords(restriction: new RecordRestriction("EventID = {0}", record.ID));

                    foreach (var disturbance in disturbances)
                    {
                        disturbance.StartTime = disturbance.StartTime.AddTicks(ticks);
                        disturbance.EndTime = disturbance.EndTime.AddTicks(ticks);
                        disturbanceTable.UpdateRecord(disturbance);
                    }

                    // Update fault records
                    // IF propagate is true update all associated with the file
                    // if propagate is false update all assocaited with the event id
                    IEnumerable<Fault> faults;

                    if (propagate)
                        faults = faultTable.QueryRecords(restriction: new RecordRestriction("EventID IN (Select ID from Event WHERE FileGroupID = {0})", record.FileGroupID));
                    else
                        faults = faultTable.QueryRecords(restriction: new RecordRestriction("EventID = {0}", record.ID));

                    foreach (var fault in faults)
                    {
                        fault.Inception = fault.Inception.AddTicks(ticks);
                        faultTable.UpdateRecord(fault);
                    }

                    DataGroup dataTimeGroup = new DataGroup();
                    DataGroup dataFaultAlgo = new DataGroup();

                    Meter meter = new TableOperations<Meter>(connection).QueryRecordWhere("ID = {0}", record.MeterID);

                    //This is only to get old Data Migrated we will not be using the output, but calling 
                    // DataFromEvent will cause all of the data to be migrated to the new schema 
                    
                    List<byte[]> timeSeries = ChannelData.DataFromEvent(record.ID, connection);

                    if (propagate)
                    {
                        IEnumerable<Event> events = eventTable.QueryRecords(restriction: new RecordRestriction("FileGroupID = {0} AND ID <> {1}", record.FileGroupID, record.ID));
                        foreach (Event e in events)
                        {
                            timeSeries = ChannelData.DataFromEvent(e.ID, connection);
                        }
                    }


                    byte[] faultCurve = connection.ExecuteScalar<byte[]>("SELECT Data FROM FaultCurve WHERE EventID = {0}", record.ID);

                    meter.ConnectionFactory = () => new AdoDataConnection("systemSettings");

                    TableOperations<ChannelData> channelDataTbl = new TableOperations<ChannelData>(connection);
                    List<ChannelData> channelData = channelDataTbl.QueryRecordsWhere(
                        (propagate? "FileGroupID = (SELECT TOP 1 FileGroupID FROM ChannelData WHERE EventID = {0})" : "EventID = {0}"), record.ID).ToList();

                    try
                    {
                        if (channelData != null)
                        {
                          
                            foreach (ChannelData item in channelData)
                            {
                                item.AdjustData(ticks);
                                channelDataTbl.UpdateRecord(item);
                            }
                            
                        }

                        if (faultCurve != null)
                        {
                            dataFaultAlgo.FromData(meter, new List<byte[]>() { faultCurve });
                            foreach (var dataSeries in dataFaultAlgo.DataSeries)
                            {
                                foreach (var dataPoint in dataSeries.DataPoints)
                                {
                                    dataPoint.Time = dataPoint.Time.AddTicks(ticks);
                                }
                            }

                            byte[] newFaultAlgo = dataFaultAlgo.ToData()[dataFaultAlgo.DataSeries[0].SeriesInfo.ChannelID];

                            connection.ExecuteNonQuery("Update FaultCurve SET Data = {0} WHERE EventID = {1}", newFaultAlgo, record.ID);
                        }
                    }
                    catch (Exception ex)
                    {
                        OnLogStatusMessage(ex.ToString());
                    }
                }

                if (record.EventTypeID == 1)
                {
                    int rowCount = connection.Connection.ExecuteNonQuery($"UPDATE faultsummary SET IsSuppressed = 0 where eventid = {record.ID}");
                    if (rowCount == 0)
                    {
                        TableOperations<FaultLocationAlgorithm> flaTable = new TableOperations<FaultLocationAlgorithm>(connection);

                        IEnumerable<FaultLocationAlgorithm> fla = flaTable.QueryRecords();
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

                            faultTable.AddNewRecord(fault);

                        }
                    }
                }
                else
                {
                    connection.Connection.ExecuteNonQuery($"UPDATE faultsummary SET IsSuppressed = 1 where eventid = {record.ID}");
                }
                eventTable.UpdateRecord(MakeEventFromEventView(record));
                return true;
            }
        }

        private Event MakeEventFromEventView(EventView record)
        {
            Event newEvent = new Event();
            newEvent.ID = record.ID;
            newEvent.FileGroupID = record.FileGroupID;
            newEvent.MeterID = record.MeterID;
            newEvent.AssetID = record.AssetID;
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

        [AuthorizeHubRole("*")]
        [RecordOperation(typeof(EventView), RecordOperation.QueryRecordCount)]
        public int QueryEventViewCount(int filterId, string filterString) => QueryEventCount(filterId, filterString);

        [AuthorizeHubRole("*")]
        [RecordOperation(typeof(EventView), RecordOperation.QueryRecords)]
        public IEnumerable<EventView> QueryEventViews(int filterId, string sortField, bool ascending, int page, int pageSize, string filterString) =>
            QueryEvents(filterId, sortField, ascending, page, pageSize, filterString);

        [AuthorizeHubRole("Administrator, Engineer")]
        [RecordOperation(typeof(EventView), RecordOperation.DeleteRecord)]
        public void DeleteEventView(int id) => DeleteEvent(id);

        [AuthorizeHubRole("Administrator, Engineer")]
        [RecordOperation(typeof(EventView), RecordOperation.CreateNewRecord)]
        public EventView NewEventView() => NewEvent();

        [AuthorizeHubRole("Administrator, Engineer")]
        [RecordOperation(typeof(EventView), RecordOperation.AddNewRecord)]
        public void AddNewEventView(Event record) => AddNewEvent(record);

        [AuthorizeHubRole("Administrator, Engineer")]
        [RecordOperation(typeof(EventView), RecordOperation.UpdateRecord)]
        public bool UpdateEventView(EventView record, bool propagate) => UpdateEvent(record, propagate);

        #endregion

        #region [SingleEvent Operations]

        [AuthorizeHubRole("*")]
        [RecordOperation(typeof(SingleEvent), RecordOperation.QueryRecordCount)]
        public int QuerySingleEventCount(int eventId, string filterString)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                return new TableOperations<EventView>(connection).QueryRecordCount(new RecordRestriction("ID = {0}", eventId));
            }
        }

        [AuthorizeHubRole("*")]
        [RecordOperation(typeof(SingleEvent), RecordOperation.QueryRecords)]
        public IEnumerable<EventView> QuerySingleEvents(int eventId, string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                return new TableOperations<EventView>(connection).QueryRecords(sortField, ascending, page, pageSize, new RecordRestriction("ID = {0}", eventId)).ToList();
            }
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
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                return new EventView();
            }
        }

        [AuthorizeHubRole("Administrator, Engineer")]
        [RecordOperation(typeof(SingleEvent), RecordOperation.AddNewRecord)]
        public void AddNewSingleEvent(EventView record)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                new TableOperations<Event>(connection).AddNewRecord(record);
            }
        }

        [AuthorizeHubRole("Administrator, Engineer")]
        [RecordOperation(typeof(SingleEvent), RecordOperation.UpdateRecord)]
        public bool UpdateSingleEvent(EventView record, bool propagate) => UpdateEvent(record, propagate);
        #endregion

        #region [EventsForDate Operations]

        public IEnumerable<EventView> GetAllEventsForDate(int eventId, string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {


                DateTime date = connection.ExecuteScalar<DateTime>("Select StartTime FROM Event WHERE ID = {0}", eventId);
                DateTime startTime = date.AddMinutes(-5);
                DateTime endTime = date.AddMinutes(5);

                if (!filterString.EndsWith("%"))
                    filterString += "%";


                return new TableOperations<EventView>(connection).QueryRecords(sortField, ascending, page, pageSize, new RecordRestriction("StartTime >= {0} AND StartTime <= {1} AND (ID LIKE {2} OR MeterName LIKE {3} OR AssetName LIKE {4} OR EventTypeName LIKE {5})", startTime, endTime, filterString, filterString, filterString, filterString)).ToList();
            }
        }

        public int GetCountAllEventsForDate(int eventId, string filterString)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                int seconds = connection.ExecuteScalar<int>("Select Value FROM Setting WHERE Name = 'WorkbenchTimeRangeInSeconds'");
                DateTime date = connection.ExecuteScalar<DateTime>("Select StartTime FROM Event WHERE ID = {0}", eventId);
                DateTime startTime = date.AddSeconds(-1 * seconds);
                DateTime endTime = date.AddSeconds(seconds);

                if (!filterString.EndsWith("%"))
                    filterString += "%";


                return new TableOperations<EventView>(connection).QueryRecordCount(new RecordRestriction("StartTime >= {0} AND StartTime <= {1} AND (ID LIKE {2} OR MeterName LIKE {3} OR AssetName LIKE {4} OR EventTypeName Like {5})", startTime, endTime, filterString, filterString, filterString, filterString));
            }
        }

        [AuthorizeHubRole("*")]
        [RecordOperation(typeof(EventForDate), RecordOperation.QueryRecordCount)]
        public int QueryEventForDateCount(int eventId, int time, string filterString)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                int seconds = connection.ExecuteScalar<int>("Select Value FROM Setting WHERE Name = 'WorkbenchTimeRangeInSeconds'");
                DateTime date = connection.ExecuteScalar<DateTime>("Select " + (time == 1 ? "StartTime" : "EndTime") + " FROM Event WHERE ID = {0}", eventId);
                DateTime startTime = date.AddSeconds(-1 * seconds);
                DateTime endTime = date.AddSeconds(seconds);
                if (!filterString.EndsWith("%"))
                    filterString += "%";


                return new TableOperations<EventView>(connection).QueryRecordCount(new RecordRestriction("StartTime >= {0} AND StartTime <= {1} AND (ID LIKE {2} OR StartTime LIKE {3} OR EndTime LIKE {4} OR MeterName LIKE {5} OR AssetName LIKE {6})", startTime, endTime, filterString, filterString, filterString, filterString, filterString));
            }
        }

        [AuthorizeHubRole("*")]
        [RecordOperation(typeof(EventForDate), RecordOperation.QueryRecords)]
        public IEnumerable<EventView> QueryEventForDate(int eventId, int time, string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                string timeWord = (time == 1 ? "StartTime" : "EndTime");
                int seconds = connection.ExecuteScalar<int>("Select Value FROM Setting WHERE Name = 'WorkbenchTimeRangeInSeconds'");
                DateTime date = connection.ExecuteScalar<DateTime>("Select " + (time == 1 ? "StartTime" : "EndTime") + " FROM Event WHERE ID = {0}", eventId);
                DateTime startTime = date.AddSeconds(-1 * seconds);
                DateTime endTime = date.AddSeconds(seconds);
                if (!filterString.EndsWith("%"))
                    filterString += "%";


                return new TableOperations<EventView>(connection).QueryRecords(sortField, ascending, page, pageSize, new RecordRestriction("StartTime >= {0} AND StartTime <= {1} AND (ID LIKE {2} OR StartTime LIKE {3} OR EndTime LIKE {4} OR MeterName LIKE {5} OR AssetName LIKE {6})", startTime, endTime, filterString, filterString, filterString, filterString, filterString)).ToList();

            }
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
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                return new TableOperations<Event>(connection).NewRecord();
            }
        }

        [AuthorizeHubRole("Administrator, Engineer")]
        [RecordOperation(typeof(EventForDate), RecordOperation.AddNewRecord)]
        public void AddNewEventforDate(Event record)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                new TableOperations<Event>(connection).AddNewRecord(record);
            }
        }

        [AuthorizeHubRole("Administrator, Engineer")]
        [RecordOperation(typeof(EventForDate), RecordOperation.UpdateRecord)]
        public bool UpdateEventForRecord(EventView record, bool propagate) => UpdateEvent(record, propagate);
        #endregion

        #region [EventsForDay Operations]  
        [AuthorizeHubRole("*")]
        [RecordOperation(typeof(EventForDay), RecordOperation.QueryRecordCount)]
        public int QueryEventForDayCount(DateTime date, string eventTypes, int filterId, string filterString)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                DateTime startTime = new DateTime(date.Date.Ticks);
                DateTime endTime = startTime.AddDays(1).AddMilliseconds(-1);
                IEnumerable<EventType> types = new TableOperations<EventType>(connection).QueryRecords().ToList();
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

                TableOperations<EventView> table = new TableOperations<EventView>(connection);
                RecordRestriction restriction = table.GetSearchRestriction(filterString) + new RecordRestriction(sql);
                return table.QueryRecordCount(restriction);
            }
        }

        [AuthorizeHubRole("*")]
        [RecordOperation(typeof(EventForDay), RecordOperation.QueryRecords)]
        public IEnumerable<EventView> QueryEventForDay(DateTime date, string eventTypes, int filterId, string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                DateTime startTime = new DateTime(date.Date.Ticks);
                DateTime endTime = startTime.AddDays(1).AddMilliseconds(-1);
                IEnumerable<EventType> types = new TableOperations<EventType>(connection).QueryRecords().ToList();
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

                TableOperations<EventView> table = new TableOperations<EventView>(connection);
                RecordRestriction restriction = table.GetSearchRestriction(filterString) + new RecordRestriction(sql);

                return table.QueryRecords(sortField, ascending, page, pageSize, restriction).ToList();
            }
        }

        [AuthorizeHubRole("Administrator, Engineer")]
        [RecordOperation(typeof(EventForDay), RecordOperation.DeleteRecord)]
        public void DeleteEventForDay(int id)
        {
            CascadeDelete("Event", $"ID={id}");
        }

        [AuthorizeHubRole("Administrator, Engineer")]
        [RecordOperation(typeof(EventForDay), RecordOperation.CreateNewRecord)]
        public EventView NewEventForDay()
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                return new TableOperations<EventView>(connection).NewRecord();
            }
        }

        [AuthorizeHubRole("Administrator, Engineer")]
        [RecordOperation(typeof(EventForDay), RecordOperation.AddNewRecord)]
        public void AddNewEventForDay(EventView record)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                new TableOperations<Event>(connection).AddNewRecord(record);
            }
        }

        [AuthorizeHubRole("Administrator, Engineer")]
        [RecordOperation(typeof(EventForDay), RecordOperation.UpdateRecord)]
        public bool UpdateEventForDayRecord(EventView record, bool propagate) => UpdateEvent(record, propagate);

        #endregion

        #region [EventsForMeter Operations]

        public IEnumerable<EventView> GetEventsForMeter(int meterId, int filterId, string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                Tuple<DateTime, DateTime> dTuple = GetTimeRange(filterId);
                DateTime startDate = dTuple.Item1;
                DateTime endDate = dTuple.Item2;

                return new TableOperations<EventView>(connection).QueryRecords(sortField, ascending, page, pageSize, new RecordRestriction("MeterID = {0} AND StartTime >= {1} AND StartTime <= {2} AND EventTypeID IN (Select * FROM String_To_Int_Table((Select EventTypes FROM WorkbenchFilter WHERE ID = {3}), ',')) AND (ID LIKE {4} OR MeterName LIKE {5} OR LineName LIKE {6} OR EventTypeName LIKE {7})", meterId, startDate, endDate, filterId, filterString, filterString, filterString, filterString)).ToList();
            }
        }

        public int GetCountEventsForMeter(int meterId, int filterId, string filterString)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                Tuple<DateTime, DateTime> dTuple = GetTimeRange(filterId);
                DateTime startDate = dTuple.Item1;
                DateTime endDate = dTuple.Item2;

                return new TableOperations<EventView>(connection).QueryRecordCount(new RecordRestriction("MeterID = {0} AND StartTime >= {1} AND StartTime <= {2} AND EventTypeID IN (Select * FROM String_To_Int_Table((Select EventTypes FROM WorkbenchFilter WHERE ID = {3}), ',')) AND (ID LIKE {4} OR MeterName LIKE {5} OR LineName LIKE {6} OR EventTypeName LIKE {7})", meterId, startDate, endDate, filterId, filterString, filterString, filterString, filterString));
            }
        }

        [AuthorizeHubRole("*")]
        [RecordOperation(typeof(EventForMeter), RecordOperation.QueryRecordCount)]
        public int QueryEventsForMeterCount(int meterId, int filterId, string filterString)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                Tuple<DateTime, DateTime> dTuple = GetTimeRange(filterId);
                DateTime startDate = dTuple.Item1;
                DateTime endDate = dTuple.Item2;

                TableOperations<EventView> tableOperations = new TableOperations<EventView>(connection);
                RecordRestriction restriction = tableOperations.GetSearchRestriction(filterString) +
                    new RecordRestriction("MeterID = {0} AND " +
                                        "EventTypeID IN (Select * FROM String_To_Int_Table((Select EventTypes FROM WorkbenchFilter WHERE ID = {1}), ',')) AND " +
                                        "StartTime >= {2} AND " +
                                        "StartTime <= {3} ",
                                        meterId, filterId, startDate, endDate);

                return tableOperations.QueryRecordCount(restriction);
            }
        }

        [AuthorizeHubRole("*")]
        [RecordOperation(typeof(EventForMeter), RecordOperation.QueryRecords)]
        public IEnumerable<EventView> QueryEventsForMeters(int meterId, int filterId, string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                Tuple<DateTime, DateTime> dTuple = GetTimeRange(filterId);
                DateTime startDate = dTuple.Item1;
                DateTime endDate = dTuple.Item2;

                TableOperations<EventView> tableOperations = new TableOperations<EventView>(connection);
                RecordRestriction restriction = tableOperations.GetSearchRestriction(filterString) +
                    new RecordRestriction("MeterID = {0} AND " +
                                        "EventTypeID IN (Select * FROM String_To_Int_Table((Select EventTypes FROM WorkbenchFilter WHERE ID = {1}), ',')) AND " +
                                        "StartTime >= {2} AND " +
                                        "StartTime <= {3} ",
                                        meterId, filterId, startDate, endDate);

                return tableOperations.QueryRecords(sortField, ascending, page, pageSize, restriction).ToList();
            }
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
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                return new TableOperations<EventView>(connection).NewRecord();
            }
        }

        [AuthorizeHubRole("Administrator, Engineer")]
        [RecordOperation(typeof(EventForMeter), RecordOperation.AddNewRecord)]
        public void AddNewEventForMeter(EventView record)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                new TableOperations<Event>(connection).AddNewRecord(record);
            }
        }

        [AuthorizeHubRole("Administrator, Engineer")]
        [RecordOperation(typeof(EventForMeter), RecordOperation.UpdateRecord)]
        public bool UpdateEventForMeter(EventView record, bool propagate) => UpdateEvent(record, propagate);
        #endregion

        #region [ MeterEventsByLine Table Operations ]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(MeterEventsByLine), RecordOperation.QueryRecordCount)]
        public int QueryMeterEventsByLineCount(int siteID, DateTime targetDate, string context, string filterString )
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                DataTable table = new DataTable();
                using (IDbCommand sc = connection.Connection.CreateCommand())
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
                return table.Select(fe).Count();
            }
        }
    

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(MeterEventsByLine), RecordOperation.QueryRecords)]
        public IEnumerable<MeterEventsByLine> QueryMeterEventsByLines(int siteID, DateTime targetDate,string context,string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                DataTable table = new DataTable();
                using (IDbCommand sc = connection.Connection.CreateCommand())
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
                    return table.Select(fe).Select(row => new TableOperations<MeterEventsByLine>(connection).LoadRecord(row)).OrderBy(x => x.GetType().GetProperty(sortField).GetValue(x));
                else
                    return table.Select(fe).Select(row => new TableOperations<MeterEventsByLine>(connection).LoadRecord(row)).OrderByDescending(x => x.GetType().GetProperty(sortField).GetValue(x));

            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(MeterEventsByLine), RecordOperation.DeleteRecord)]
        public void DeleteMeterEventsByLine(int id)
        {
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(MeterEventsByLine), RecordOperation.CreateNewRecord)]
        public MeterEventsByLine NewMeterEventsByLine()
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                return new TableOperations<MeterEventsByLine>(connection).NewRecord();
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(MeterEventsByLine), RecordOperation.AddNewRecord)]
        public void AddNewMeterEventsByLine(MeterEventsByLine record)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                new TableOperations<MeterEventsByLine>(connection).AddNewRecord(record);
            }
        }

        [AuthorizeHubRole("Administrator, Owner")]
        [RecordOperation(typeof(MeterEventsByLine), RecordOperation.UpdateRecord)]
        public void UpdateMeterEventsByLine(MeterEventsByLine record)
        {
        }

        public void UpdateAllEventTypesForRange(List<int> eventIds, string eventType)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                foreach (var eventId in eventIds)
                {
                    connection.ExecuteNonQuery("Update Event SET EventTypeID = (SELECT ID FROM EventType WHERE Name = {0}), UpdatedBy = {1} WHERE ID = {2}", eventType, GetCurrentUserName(), eventId);
                }
            }
        }

        #endregion

        #region [ FaultDetailsByDate Table Operations ]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(FaultsDetailsByDate), RecordOperation.QueryRecordCount)]
        public int QueryFaultDetailsByDateCount(string siteID, DateTime targetDate, string context, string filterString)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                DataTable table = new DataTable();
                using (IDbCommand sc = connection.Connection.CreateCommand())
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
                return table.Select(fe).Count();
            }
        }


        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(FaultsDetailsByDate), RecordOperation.QueryRecords)]
        public IEnumerable<FaultsDetailsByDate> QueryFaultsDetailsByDate(string siteID, DateTime targetDate,string context, string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                DataTable table = new DataTable();
                using (IDbCommand sc = connection.Connection.CreateCommand())
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
                    return table.Select(fe).Select(row => new TableOperations<FaultsDetailsByDate>(connection).LoadRecord(row)).OrderBy(x => x.GetType().GetProperty(sortField).GetValue(x));
                else
                    return table.Select(fe).Select(row => new TableOperations<FaultsDetailsByDate>(connection).LoadRecord(row)).OrderByDescending(x => x.GetType().GetProperty(sortField).GetValue(x));
            }
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
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                return new TableOperations<FaultsDetailsByDate>(connection).NewRecord();
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(FaultsDetailsByDate), RecordOperation.AddNewRecord)]
        public void AddNewFaultsDetailsByDate(FaultsDetailsByDate record)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                new TableOperations<FaultsDetailsByDate>(connection).AddNewRecord(record);
            }
        }

        [AuthorizeHubRole("Administrator, Owner")]
        [RecordOperation(typeof(FaultsDetailsByDate), RecordOperation.UpdateRecord)]
        public void UpdateFaultsDetailsByDate(FaultsDetailsByDate record)
        {
        }

        public void UpdateFaultsDetailsByDate(List<int> eventIds, string eventType)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                foreach (var eventId in eventIds)
                {
                    connection.ExecuteNonQuery("Update Event SET EventTypeID = (SELECT ID FROM EventType WHERE Name = {0}), UpdatedBy = {1} WHERE ID ={2}", eventType, GetCurrentUserName(), eventId);
                    connection.ExecuteNonQuery("Update FaultSummary SET IsValid = {0} WHERE EventID = {1}", (eventType != "Fault" ? 0 : 1), eventId);
                }
            }
        }

        public void UndoChanges(List<int> eventIds)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                foreach (var eventId in eventIds)
                {
                    int eventTypeID = connection.ExecuteScalar<int>("Select OriginalValue FROM AuditLog WHERE PrimaryKeyValue = {0}", eventId);
                    connection.ExecuteNonQuery("Update Event SET EventTypeID = {0}, UpdatedBy = {1} WHERE ID = {2}", eventTypeID, GetCurrentUserName(), eventId);
                    connection.ExecuteNonQuery("Update FaultSummary SET IsValid = 1 WHERE EventID = {0}", eventId);
                }
            }
        }

        #endregion

        #region [DisturbancesForDay Operations]
        public IEnumerable<DisturbanceView> GetDisturbancesForDay(DateTime date, string eventTypes, int filterId, string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                DateTime startTime = new DateTime(date.Date.Ticks);
                DateTime endTime = startTime.AddDays(1).AddMilliseconds(-1);
                string eventTypeList = "" + (eventTypes.Contains("5") ? "'5'," : "") + (eventTypes.Contains("4") ? "'4'," : "") + (eventTypes.Contains("3") ? "'3'," : "") + (eventTypes.Contains("2") ? "'2'," : "") + (eventTypes.Contains("1") ? "'1'," : "") + (eventTypes.Contains("0") ? "'0'," : "");
                eventTypeList = eventTypeList.Remove(eventTypeList.Length - 1, 1);
                filterString += '%';
                return new TableOperations<DisturbanceView>(connection).QueryRecords(sortField, ascending, page, pageSize, new RecordRestriction(
                    " MeterID IN (Select * FROM String_To_Int_Table((Select Meters FROM WorkbenchFilter WHERE ID = {0}), ',')) AND " +
                    " StartTime >= {1} AND StartTime <= {2} AND " +
                    " (ID LIKE {3} OR StartTime LIKE {4} OR EndTime LIKE {5} OR MeterName LIKE {6} OR EventID LIKE {7} OR PhaseName Like {8}) AND " +
                    $" SeverityCode IN({eventTypeList}) ",
                    filterId, date, endTime, filterString, filterString, filterString, filterString, filterString, filterString)).ToList();
            }
        }

        public int GetCountDisturbancesForDay(DateTime date, string eventTypes, string filterString, int filterId)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                DateTime startTime = new DateTime(date.Date.Ticks);
                DateTime endTime = startTime.AddDays(1).AddMilliseconds(-1);
                string eventTypeList = "" + (eventTypes.Contains("5") ? "'5'," : "") + (eventTypes.Contains("4") ? "'4'," : "") + (eventTypes.Contains("3") ? "'3'," : "") + (eventTypes.Contains("2") ? "'2'," : "") + (eventTypes.Contains("1") ? "'1'," : "") + (eventTypes.Contains("0") ? "'0'," : "");
                eventTypeList = eventTypeList.Remove(eventTypeList.Length - 1, 1);
                filterString += '%';

                return new TableOperations<DisturbanceView>(connection).QueryRecordCount(new RecordRestriction(
                    " MeterID IN (Select * FROM String_To_Int_Table((Select Meters FROM WorkbenchFilter WHERE ID = {0}), ',')) AND " +
                    " StartTime >= {1} AND StartTime <= {2} AND " +
                    " (ID LIKE {3} OR StartTime LIKE {4} OR EndTime LIKE {5} OR MeterName LIKE {6} OR EventID LIKE {7} OR PhaseName Like {8}) AND " +
                    $" SeverityCode IN({eventTypeList}) ",
                    filterId, date, endTime, filterString, filterString, filterString, filterString, filterString, filterString));
            }
        }

        [AuthorizeHubRole("*")]
        [RecordOperation(typeof(DisturbancesForDay), RecordOperation.QueryRecordCount)]
        public int QueryDisturbancesForDayCount(DateTime date, string eventTypes, int filterId, string filterString)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                DateTime startTime = new DateTime(date.Date.Ticks);
                DateTime endTime = startTime.AddDays(1).AddMilliseconds(-1);
                string eventTypeList = "" + (eventTypes.Contains("5") ? "'5'," : "") + (eventTypes.Contains("4") ? "'4'," : "") + (eventTypes.Contains("3") ? "'3'," : "") + (eventTypes.Contains("2") ? "'2'," : "") + (eventTypes.Contains("1") ? "'1'," : "") + (eventTypes.Contains("0") ? "'0'," : "");
                eventTypeList = eventTypeList.Remove(eventTypeList.Length - 1, 1);
                filterString += '%';

                return new TableOperations<DisturbanceView>(connection).QueryRecordCount(new RecordRestriction(
                    " MeterID IN (Select * FROM String_To_Int_Table((Select Meters FROM WorkbenchFilter WHERE ID = {0}), ',')) AND " +
                    " StartTime >= {1} AND StartTime <= {2} AND " +
                    " (ID LIKE {3} OR MeterName LIKE {3} OR EventID LIKE {3} OR PhaseName Like {3}) AND " +
                    $" SeverityCode IN({eventTypeList}) ",
                    filterId, date, endTime, filterString));
            }
        }

        [AuthorizeHubRole("*")]
        [RecordOperation(typeof(DisturbancesForDay), RecordOperation.QueryRecords)]
        public IEnumerable<DisturbanceView> QueryDisturbancesForDay(DateTime date, string eventTypes, int filterId, string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                DateTime startTime = new DateTime(date.Date.Ticks);
                DateTime endTime = startTime.AddDays(1).AddMilliseconds(-1);
                string eventTypeList = "" + (eventTypes.Contains("5") ? "'5'," : "") + (eventTypes.Contains("4") ? "'4'," : "") + (eventTypes.Contains("3") ? "'3'," : "") + (eventTypes.Contains("2") ? "'2'," : "") + (eventTypes.Contains("1") ? "'1'," : "") + (eventTypes.Contains("0") ? "'0'," : "");
                eventTypeList = eventTypeList.Remove(eventTypeList.Length - 1, 1);
                if (!filterString.EndsWith("%"))
                    filterString += "%";

                return new TableOperations<DisturbanceView>(connection).QueryRecords(sortField, ascending, page, pageSize, new RecordRestriction(
                    " MeterID IN (Select * FROM String_To_Int_Table((Select Meters FROM WorkbenchFilter WHERE ID = {0}), ',')) AND " +
                    " StartTime >= {1} AND StartTime <= {2} AND " +
                    " (ID LIKE {3} OR MeterName LIKE {3} OR EventID LIKE {3} OR PhaseName Like {3}) AND " +
                    $" SeverityCode IN({eventTypeList}) ",
                    filterId, date, endTime, filterString)).ToList();
            }
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
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                return new TableOperations<DisturbanceView>(connection).NewRecord();
            }
        }

        [AuthorizeHubRole("Administrator, Engineer")]
        [RecordOperation(typeof(DisturbancesForDay), RecordOperation.AddNewRecord)]
        public void AddNewDisturbancesForDay(DisturbanceView record)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                new TableOperations<Disturbance>(connection).AddNewRecord(record);
            }
        }

        [AuthorizeHubRole("Administrator, Engineer")]
        [RecordOperation(typeof(DisturbancesForDay), RecordOperation.UpdateRecord)]
        public void UpdateDisturbancesForDayRecord(DisturbanceView record)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                new TableOperations<Disturbance>(connection).UpdateRecord(record);
            }
        }

        #endregion

        #region [DisturbancesForMeter Operations]

        public IEnumerable<DisturbanceView> GetDisturbancesForMeter(int meterId, int filterId, string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                Tuple<DateTime, DateTime> dTuple = GetTimeRange(filterId);
                DateTime startDate = dTuple.Item1;
                DateTime endDate = dTuple.Item2;
                return new TableOperations<DisturbanceView>(connection).QueryRecords(sortField, ascending, page, pageSize, new RecordRestriction("MeterID = {0} AND StartTime >= {1} AND StartTime <= {2} AND (ID LIKE {3} OR MeterName LIKE {4} OR PhaseName LIKE {5})", meterId, startDate, endDate, filterString, filterString, filterString)).ToList();
            }
        }

        public int GetCountDisturbancesForMeter(int meterId, int filterId, string filterString)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                Tuple<DateTime, DateTime> dTuple = GetTimeRange(filterId);
                DateTime startDate = dTuple.Item1;
                DateTime endDate = dTuple.Item2;
                return new TableOperations<DisturbanceView>(connection).QueryRecordCount(new RecordRestriction("MeterID = {0} AND StartTime >= {1} AND StartTime <= {2} AND (ID LIKE {3} OR MeterName LIKE {4} OR PhaseName LIKE {5})", meterId, startDate, endDate, filterString, filterString, filterString));
            }
        }

        [AuthorizeHubRole("*")]
        [RecordOperation(typeof(DisturbancesForMeter), RecordOperation.QueryRecordCount)]
        public int QueryDisturbancesForMeterCount(int meterId, int filterId, string filterString)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                Tuple<DateTime, DateTime> dTuple = GetTimeRange(filterId);
                DateTime startDate = dTuple.Item1;
                DateTime endDate = dTuple.Item2;
                TableOperations<DisturbanceView> tableOperations = new TableOperations<DisturbanceView>(connection);
                RecordRestriction restriction = tableOperations.GetSearchRestriction(filterString) + new RecordRestriction("MeterID = {0} AND StartTime >= {1} AND StartTime <= {2} AND PhaseName='Worst'",
                                                                                             meterId, startDate, endDate);

                return tableOperations.QueryRecordCount(restriction);
            }
        }

        [AuthorizeHubRole("*")]
        [RecordOperation(typeof(DisturbancesForMeter), RecordOperation.QueryRecords)]
        public IEnumerable<DisturbanceView> QueryDisturbancesForMeter(int meterId, int filterId, string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                Tuple<DateTime, DateTime> dTuple = GetTimeRange(filterId);
                DateTime startDate = dTuple.Item1;
                DateTime endDate = dTuple.Item2;

                TableOperations<DisturbanceView> tableOperations = new TableOperations<DisturbanceView>(connection);
                RecordRestriction restriction = tableOperations.GetSearchRestriction(filterString) + new RecordRestriction("MeterID = {0} AND StartTime >= {1} AND StartTime <= {2} AND PhaseName='Worst'",
                                                                                             meterId, startDate, endDate);

                return tableOperations.QueryRecords(sortField, ascending, page, pageSize, restriction).ToList();
            }
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
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                return new TableOperations<DisturbanceView>(connection).NewRecord();
            }
        }

        [AuthorizeHubRole("Administrator, Engineer")]
        [RecordOperation(typeof(DisturbancesForMeter), RecordOperation.AddNewRecord)]
        public void AddNewDisturbancesForMeter(DisturbanceView record)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                new TableOperations<Disturbance>(connection).AddNewRecord(record);
            }
        }

        [AuthorizeHubRole("Administrator, Engineer")]
        [RecordOperation(typeof(DisturbancesForMeter), RecordOperation.UpdateRecord)]
        public void UpdateDisturbancesForMeterRecord(DisturbanceView record)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                new TableOperations<Disturbance>(connection).UpdateRecord(record);
            }
        }

        #endregion

        #region [BreakerOperation Operations]

        [AuthorizeHubRole("*")]
        [RecordOperation(typeof(BreakerOperation), RecordOperation.QueryRecordCount)]
        public int QueryBreakerOperationCount(int eventId, string filterString)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                return new TableOperations<BreakerView>(connection).QueryRecordCount(new RecordRestriction("EventID = {0}", eventId));
            }
        }

        [AuthorizeHubRole("*")]
        [RecordOperation(typeof(BreakerOperation), RecordOperation.QueryRecords)]
        public IEnumerable<BreakerView> QueryBreakerOperations(int eventId, string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                return new TableOperations<BreakerView>(connection).QueryRecords(sortField, ascending, page, pageSize, new RecordRestriction("EventID = {0}", eventId)).ToList();
            }
        }

        [AuthorizeHubRole("Administrator, Engineer")]
        [RecordOperation(typeof(BreakerOperation), RecordOperation.DeleteRecord)]
        public void DeleteBreakerOperation(int id)
        {
            CascadeDelete("BreakerOperation", $"ID={id}");
        }

        [AuthorizeHubRole("Administrator, Engineer")]
        [RecordOperation(typeof(BreakerOperation), RecordOperation.CreateNewRecord)]
        public BreakerView NewBreakerOperation()
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                return new TableOperations<BreakerView>(connection).NewRecord();
            }
        }

        [AuthorizeHubRole("Administrator, Engineer")]
        [RecordOperation(typeof(BreakerOperation), RecordOperation.AddNewRecord)]
        public void AddNewBreakerOperation(BreakerOperation record)
        {
        }

        [AuthorizeHubRole("Administrator, Engineer")]
        [RecordOperation(typeof(BreakerOperation), RecordOperation.UpdateRecord)]
        public void UpdateBreakerOperation(BreakerView record)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                BreakerOperation bo = new TableOperations<BreakerOperation>(connection).QueryRecordWhere("ID = {0}", record.ID);
                bo.TripCoilEnergized = DateTime.Parse(record.Energized);
                bo.BreakerOperationTypeID = connection.ExecuteScalar<int>("SELECT ID FROM BreakerOperationType WHERE Name = {0}", record.OperationType);
                bo.UpdatedBy = GetCurrentUserName();
                new TableOperations<BreakerOperation>(connection).UpdateRecord(bo);
            }
        }

        #endregion

        #region [BreakersForDay Operations]  
        [AuthorizeHubRole("*")]
        [RecordOperation(typeof(BreakersForDay), RecordOperation.QueryRecordCount)]
        public int QueryBreakerForDayCount(DateTime date, string operationTypes, int filterId, string filterString)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                DateTime startTime = new DateTime(date.Date.Ticks);
                DateTime endTime = startTime.AddDays(1).AddMilliseconds(-1);
                IEnumerable<BreakerOperationType> types = new TableOperations<BreakerOperationType>(connection).QueryRecords().ToList();
                string operationTypeList = "";

                foreach (var type in types)
                {
                    if (operationTypes.Contains(type.Name))
                        operationTypeList += "'" + type.Name + "',";
                }

                operationTypeList = operationTypeList.Remove(operationTypeList.Length - 1, 1);
                if (!filterString.EndsWith("%"))
                    filterString += "%";

                TableOperations<BreakerView> table = new TableOperations<BreakerView>(connection);
                RecordRestriction restriction;
                restriction = table.GetSearchRestriction(filterString) + new RecordRestriction($"(MeterID IN (Select * FROM String_To_Int_Table((Select Meters FROM WorkbenchFilter WHERE ID = {filterId}), ','))) AND Energized >= '{startTime}' AND Energized <= '{endTime}' AND OperationType IN ({operationTypeList})");
                return table.QueryRecordCount(restriction);
            }
        }

        [AuthorizeHubRole("*")]
        [RecordOperation(typeof(BreakersForDay), RecordOperation.QueryRecords)]
        public IEnumerable<BreakerView> QueryBreakersForDay(DateTime date, string operationTypes, int filterId, string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                DateTime startTime = new DateTime(date.Date.Ticks);
                DateTime endTime = startTime.AddDays(1).AddMilliseconds(-1);
                IEnumerable<BreakerOperationType> types = new TableOperations<BreakerOperationType>(connection).QueryRecords().ToList();
                string operationTypeList = "";

                foreach (var type in types)
                {
                    if (operationTypes.Contains(type.Name))
                        operationTypeList += "'" + type.Name + "',";
                }

                operationTypeList = operationTypeList.Remove(operationTypeList.Length - 1, 1);
                if (!filterString.EndsWith("%"))
                    filterString += "%";

                TableOperations<BreakerView> table = new TableOperations<BreakerView>(connection);
                RecordRestriction restriction;
                restriction = table.GetSearchRestriction(filterString) + new RecordRestriction($"(MeterID IN (Select * FROM String_To_Int_Table((Select Meters FROM WorkbenchFilter WHERE ID = {filterId}), ','))) AND Energized >= '{startTime}' AND Energized <= '{endTime}' AND OperationType IN ({operationTypeList})");

                return table.QueryRecords(sortField, ascending, page, pageSize, restriction).ToList();
            }
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
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                return new TableOperations<BreakerView>(connection).NewRecord();
            }
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
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                BreakerOperation bo = new TableOperations<BreakerOperation>(connection).QueryRecordWhere("ID = {0}", record.ID);
                bo.TripCoilEnergized = DateTime.Parse(record.Energized);
                bo.BreakerOperationTypeID = connection.ExecuteScalar<int>("SELECT ID FROM BreakerOperationType WHERE Name = {0}", record.OperationType);
                bo.UpdatedBy = GetCurrentUserName();
                new TableOperations<BreakerOperation>(connection).UpdateRecord(bo);
            }
        }



        #endregion

        #region [FaultsForDay Operations]

        public IEnumerable<FaultView> GetFaultsForDay(DateTime date, string eventTypes, int filterId, string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                DateTime startTime = new DateTime(date.Date.Ticks);
                DateTime endTime = startTime.AddDays(1).AddMilliseconds(-1);
                string eventTypeList = "" + (eventTypes.Contains("500 kV") ? "'500'," : "") + (eventTypes.Contains("300 kV") ? "'300'," : "") + (eventTypes.Contains("230 kV") ? "'230'," : "") + (eventTypes.Contains("135 kV") ? "'135'," : "") + (eventTypes.Contains("115 kV") ? "'115'," : "") + (eventTypes.Contains("69 kV") ? "'69'," : "") + (eventTypes.Contains("46 kV") ? "'46'," : "") + (eventTypes.Contains("0 kV") ? "'0'," : "");
                eventTypeList = eventTypeList.Remove(eventTypeList.Length - 1, 1);
                filterString += '%';
                return new TableOperations<FaultView>(connection).QueryRecords(sortField, ascending, page, pageSize, new RecordRestriction(
                    " RK = 1 AND MeterID IN (Select * FROM String_To_Int_Table((Select Meters FROM WorkbenchFilter WHERE ID = {0}), ',')) AND " +
                    " InceptionTime >= {1} AND InceptionTime <= {2} AND " +
                    " (MeterName LIKE {3} OR EventID LIKE {4} OR LineName LIKE {5} OR Voltage LIKE {6} OR FaultType Like {7} OR CurrentDistance LIKE {8} OR InceptionTime LIKE {9}) AND " +
                    $" Voltage IN({eventTypeList}) ",
                    filterId, date, endTime, filterString, filterString, filterString, filterString, filterString, filterString, filterString)).ToList();
            }
        }

        public int GetCountFaultsForDay(DateTime date, string eventTypes, string filterString, int filterId)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                DateTime startTime = new DateTime(date.Date.Ticks);
                DateTime endTime = startTime.AddDays(1).AddMilliseconds(-1);
                string eventTypeList = "" + (eventTypes.Contains("500 kV") ? "'500'," : "") + (eventTypes.Contains("300 kV") ? "'300'," : "") + (eventTypes.Contains("230 kV") ? "'230'," : "") + (eventTypes.Contains("135 kV") ? "'135'," : "") + (eventTypes.Contains("115 kV") ? "'115'," : "") + (eventTypes.Contains("69 kV") ? "'69'," : "") + (eventTypes.Contains("46 kV") ? "'46'," : "") + (eventTypes.Contains("0 kV") ? "'0'," : "");
                eventTypeList = eventTypeList.Remove(eventTypeList.Length - 1, 1);
                filterString += '%';

                return new TableOperations<FaultView>(connection).QueryRecordCount(new RecordRestriction(
                    " RK = 1 AND MeterID IN (Select * FROM String_To_Int_Table((Select Meters FROM WorkbenchFilter WHERE ID = {0}), ',')) AND " +
                    " InceptionTime >= {1} AND InceptionTime <= {2} AND " +
                    " (MeterName LIKE {3} OR EventID LIKE {4} OR LineName LIKE {5} OR Voltage LIKE {6} OR FaultType Like {7} OR CurrentDistance LIKE {8} OR InceptionTime LIKE {9}) AND " +
                    $" Voltage IN({eventTypeList}) ",
                    filterId, date, endTime, filterString, filterString, filterString, filterString, filterString, filterString, filterString));
            }
        }

        #endregion

        #region [FaultForMeter Operations]
        [AuthorizeHubRole("*")]
        [RecordOperation(typeof(FaultForMeter), RecordOperation.QueryRecordCount)]
        public int QueryFaultorMeterCount(int meterId, int filterId, string filterString)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                Tuple<DateTime, DateTime> dTuple = GetTimeRange(filterId);
                DateTime startDate = dTuple.Item1;
                DateTime endDate = dTuple.Item2;

                TableOperations<FaultView> tableOperations = new TableOperations<FaultView>(connection);
                RecordRestriction restriction = tableOperations.GetSearchRestriction(filterString) + new RecordRestriction("MeterID = {0} AND InceptionTime >= {1} AND InceptionTime <= {2} AND RK=1",
                                                                                             meterId, startDate.Date, endDate.Date);

                return tableOperations.QueryRecordCount(restriction);
            }
        }

        [AuthorizeHubRole("*")]
        [RecordOperation(typeof(FaultForMeter), RecordOperation.QueryRecords)]
        public IEnumerable<FaultView> QueryFaultsForMeter(int meterId, int filterId, string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                Tuple<DateTime, DateTime> dTuple = GetTimeRange(filterId);
                DateTime startDate = dTuple.Item1;
                DateTime endDate = dTuple.Item2;

                TableOperations<FaultView> tableOperations = new TableOperations<FaultView>(connection);
                RecordRestriction restriction = tableOperations.GetSearchRestriction(filterString) + new RecordRestriction("MeterID = {0} AND InceptionTime >= {1} AND InceptionTime <= {2} AND RK=1",
                                                                                             meterId, startDate.Date, endDate.Date);

                return tableOperations.QueryRecords(sortField, ascending, page, pageSize, restriction).ToList();
            }
        }

        [AuthorizeHubRole("Administrator, Engineer")]
        [RecordOperation(typeof(FaultForMeter), RecordOperation.DeleteRecord)]
        public void DeleteFaultsForMeter(int id)
        {
            CascadeDelete("FaultSummary", $"ID={id}");
        }

        [AuthorizeHubRole("Administrator, Engineer")]
        [RecordOperation(typeof(FaultForMeter), RecordOperation.CreateNewRecord)]
        public FaultView NewFaultsForMeter()
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                return new TableOperations<FaultView>(connection).NewRecord();
            }
        }

        [AuthorizeHubRole("Administrator, Engineer")]
        [RecordOperation(typeof(FaultForMeter), RecordOperation.AddNewRecord)]
        public void AddNewFaultsForMeter(FaultView record)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                new TableOperations<FaultView>(connection).AddNewRecord(record);
            }
        }

        [AuthorizeHubRole("Administrator, Engineer")]
        [RecordOperation(typeof(FaultForMeter), RecordOperation.UpdateRecord)]
        public void UpdateFaultsForMeterRecord(FaultView record)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                new TableOperations<FaultView>(connection).UpdateRecord(record);
            }
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
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                TableOperations<DashSettings> dashSettingsTable = new TableOperations<DashSettings>(connection);
                TableOperations<UserDashSettings> userDashSettingsTable = new TableOperations<UserDashSettings>(connection);

                EventSet eventSet = new EventSet();

                string meters = connection.ExecuteScalar<string>("SELECT Meters FROM WorkbenchFilter WHERE ID ={0}", filterId);
                if (meters.IsNullOrWhiteSpace())
                {
                    meters = connection.ExecuteScalar<string>("Select * into #temp FROM (SELECT MeterID FROM MeterLine WHERE MeterID IN (Select * From String_To_Int_Table((Select Lines from WorkbenchFilter WHERE ID = {0}), ','))) As T " +
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

                IEnumerable<DashSettings> dashSettings = dashSettingsTable.QueryRecords(restriction: new RecordRestriction("Name = '" + tab + "Chart'"));
                List<UserDashSettings> userDashSettings = userDashSettingsTable.QueryRecords(restriction: new RecordRestriction("Name = '" + tab + "Chart' AND UserAccountID IN (SELECT ID FROM UserAccount WHERE Name = {0})", userName)).ToList();

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

                IEnumerable<DashSettings> colorSettings = dashSettingsTable.QueryRecords(restriction: new RecordRestriction("Name = '" + tab + "ChartColors' AND Enabled = 1"));
                List<UserDashSettings> userColorSettings = userDashSettingsTable.QueryRecords(restriction: new RecordRestriction("Name = '" + tab + "ChartColors' AND UserAccountID IN (SELECT ID FROM UserAccount WHERE Name = {0})", userName)).ToList();

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

                using (IDbCommand sc = connection.Connection.CreateCommand())
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
                                    dashSettingsTable.AddNewRecord(ds);

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
                                            dashSettingsTable.AddNewRecord(ds);
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
                                    dashSettingsTable.AddNewRecord(ds);

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
                                            dashSettingsTable.AddNewRecord(ds);

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
        }


        public EventSet GetEventsForPeriod(int filterId)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                EventSet eventSet = new EventSet();
                TableOperations<DashSettings> dashSettingsTable = new TableOperations<DashSettings>(connection);

                string meters = connection.ExecuteScalar<string>("SELECT Meters FROM WorkbenchFilter WHERE ID ={0}", filterId);
                if (meters.IsNullOrWhiteSpace())
                {
                    meters = connection.ExecuteScalar<string>("Select * into #temp FROM (SELECT MeterID FROM MeterLine WHERE MeterID IN (Select * From String_To_Int_Table((Select Lines from WorkbenchFilter WHERE ID = {0}), ','))) As T " +
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

                List<string> disabledFields = dashSettingsTable.QueryRecords(restriction: new RecordRestriction("Name = 'EventsChart' AND Enabled = 0")).Select(x => x.Value).ToList();
                IEnumerable<DashSettings> usersColors = dashSettingsTable.QueryRecords(restriction: new RecordRestriction("Name = 'EventsChartColors' AND Enabled = 1"));
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
                    conn = (SqlConnection)connection.Connection;
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
                                        dashSettingsTable.AddNewRecord(ds);
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
                                        dashSettingsTable.AddNewRecord(ds);

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
        }

        public EventSet GetDisturbancesForPeriod(int filterId)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                EventSet eventSet = new EventSet();
                TableOperations<DashSettings> dashSettingsTable = new TableOperations<DashSettings>(connection);

                string meters = connection.ExecuteScalar<string>("SELECT Meters FROM WorkbenchFilter WHERE ID ={0}", filterId);
                if (meters.IsNullOrWhiteSpace())
                {
                    meters = connection.ExecuteScalar<string>("Select * into #temp FROM (SELECT MeterID FROM MeterLine WHERE MeterID IN (Select * From String_To_Int_Table((Select Lines from WorkbenchFilter WHERE ID = {0}), ','))) As T " +
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

                List<string> disabledFields = dashSettingsTable.QueryRecords(restriction: new RecordRestriction("Name = 'DisturbanceChart' AND Enabled = 0")).Select(x => x.Value).ToList();
                IEnumerable<DashSettings> usersColors = dashSettingsTable.QueryRecords(restriction: new RecordRestriction("Name = 'DisturbanceChartColors' AND Enabled = 1"));

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
                    conn = (SqlConnection)connection.Connection;
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
                                        dashSettingsTable.AddNewRecord(ds);
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
                                        dashSettingsTable.AddNewRecord(ds);

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
        }

        public EventSet GetFaultsForPeriod(int filterId)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                TableOperations<DashSettings> dashSettingsTable = new TableOperations<DashSettings>(connection);

                string meters = connection.ExecuteScalar<string>("SELECT Meters FROM WorkbenchFilter WHERE ID ={0}", filterId);

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

                List<string> disabledFields = dashSettingsTable.QueryRecords(restriction: new RecordRestriction("Name = 'FaultsChart' AND Enabled = 0")).Select(x => x.Value).ToList();
                IEnumerable<DashSettings> usersColors = dashSettingsTable.QueryRecords(restriction: new RecordRestriction("Name = 'FaultsChartColors' AND Enabled = 1"));
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
                    conn = (SqlConnection)connection.Connection;
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
                                        dashSettingsTable.AddNewRecord(ds);
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
                                        dashSettingsTable.AddNewRecord(ds);
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
        }

        public EventSet GetBreakersForPeriod(int filterId)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                TableOperations<DashSettings> dashSettingsTable = new TableOperations<DashSettings>(connection);
                string meters = connection.ExecuteScalar<string>("SELECT Meters FROM WorkbenchFilter WHERE ID ={0}", filterId);

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

                List<string> disabledFields = dashSettingsTable.QueryRecords(restriction: new RecordRestriction("Name = 'BreakersChart' AND Enabled = 0")).Select(x => x.Value).ToList();
                IEnumerable<DashSettings> usersColors = dashSettingsTable.QueryRecords(restriction: new RecordRestriction("Name = 'BreakersChartColors' AND Enabled = 1"));
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
                    conn = (SqlConnection)connection.Connection;
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
                                        dashSettingsTable.AddNewRecord(ds);
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
                                        dashSettingsTable.AddNewRecord(ds);
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
        }

        public DataTable GetVoltageMagnitudeData(int filterId)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                Tuple<DateTime, DateTime> dTuple = GetTimeRange(filterId);
                DateTime startDate = dTuple.Item1;
                DateTime endDate = dTuple.Item2;

                DataTable table = connection.RetrieveData(
                    " SELECT * " +
                    " FROM DisturbanceView  " +
                    " WHERE (MeterID IN (Select * FROM String_To_Int_Table((Select Meters FROM WorkbenchFilter WHERE ID = {0}), ',')) OR LineID IN (Select * FROM String_To_Int_Table((Select Lines FROM WorkbenchFilter WHERE ID = {0}), ','))) " +
                    " AND EventID IN ( SELECT ID FROM Event WHERE EventTypeID IN (Select * FROM String_To_Int_Table((Select EventTypes FROM WorkbenchFilter WHERE ID = {0}), ','))) " +
                    " AND StartTime >= {1} AND StartTime <= {2}", filterId, startDate, endDate);
                return table;
            }
        }

        public IEnumerable<WorkbenchVoltageCurveView> GetCurves()
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                return new TableOperations<WorkbenchVoltageCurveView>(connection).QueryRecords("ID, LoadOrder").ToList();
            }
        }

        #endregion

        #region [Site Summary]
        [AuthorizeHubRole("*")]
        [RecordOperation(typeof(SiteSummary), RecordOperation.QueryRecordCount)]
        public int QuerySiteSummaryCount(int filterId, string filterString)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                Tuple<DateTime, DateTime> dTuple = GetTimeRange(filterId);
                DateTime startDate = dTuple.Item1;
                DateTime endDate = dTuple.Item2;

                DataTable table = connection.RetrieveData(
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
                  Meter.Name LIKE '%" + filterString + @"%'
            ", startDate, endDate, filterId);

                return table.Select().Count();
            }
        }

        [AuthorizeHubRole("*")]
        [RecordOperation(typeof(SiteSummary), RecordOperation.QueryRecords)]
        public DataTable QuerySiteSummaries(int filterId, string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                Tuple<DateTime, DateTime> dTuple = GetTimeRange(filterId);
                DateTime startDate = dTuple.Item1;
                DateTime endDate = dTuple.Item2;
                DataTable table = new DataTable();

                using (IDbCommand sc = connection.Connection.CreateCommand())
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
                    param6.Value = $"[{sortField}] {(ascending ? "ASC" : "DESC")}";
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

                return table;
            }
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
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                new TableOperations<SiteSummary>(connection).AddNewRecord(record);
            }
        }

        [AuthorizeHubRole("Administrator, Engineer")]
        [RecordOperation(typeof(SiteSummary), RecordOperation.UpdateRecord)]
        public void UpdateSiteSummary(SiteSummary record)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                new TableOperations<SiteSummary>(connection).UpdateRecord(record);
            }
        }


        #endregion

        #region [ AuditLog Table Operations ]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(AuditLog), RecordOperation.QueryRecordCount)]
        public int QueryAuditLogCount(string filterString)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                int auditLogMax = connection.ExecuteScalar<int>("SELECT Value FROM Setting WHERE Name = 'MaxAuditLogRecords'");
                TableOperations<AuditLog> tableOperations = new TableOperations<AuditLog>(connection);
                RecordRestriction restriction;
                restriction = tableOperations.GetSearchRestriction(filterString) + new RecordRestriction("UpdatedBy IS NOT NULL AND NewValue IS NOT NULL");
                int count = tableOperations.QueryRecordCount(restriction);
                return (count > auditLogMax ? auditLogMax : count);
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(AuditLog), RecordOperation.QueryRecords)]
        public IEnumerable<AuditLog> QueryAuditLogs(string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                int auditLogMax = connection.ExecuteScalar<int>("SELECT Value FROM Setting WHERE Name = 'MaxAuditLogRecords'");
                TableOperations<AuditLog> tableOperations = new TableOperations<AuditLog>(connection, new[] { new KeyValuePair<string, string>("{count}", auditLogMax.ToString()) });
                RecordRestriction restriction;
                restriction = tableOperations.GetSearchRestriction(filterString) + new RecordRestriction("UpdatedBy IS NOT NULL AND NewValue IS NOT NULL");
                return tableOperations.QueryRecords(sortField, ascending, page, pageSize, restriction).ToList();
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(AuditLog), RecordOperation.DeleteRecord)]
        public void DeleteAuditLog(int id)
        {
            CascadeDelete("AuditLog", $"ID = {id}");
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(AuditLog), RecordOperation.CreateNewRecord)]
        public AuditLog NewAuditLog()
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                return new TableOperations<AuditLog>(connection).NewRecord();
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(AuditLog), RecordOperation.AddNewRecord)]
        public void AddNewAuditLog(AuditLog record)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                new TableOperations<AuditLog>(connection).AddNewRecord(record);
            }
        }

        [AuthorizeHubRole("Administrator, Owner")]
        [RecordOperation(typeof(AuditLog), RecordOperation.UpdateRecord)]
        public void UpdateAuditLog(AuditLog record)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                new TableOperations<AuditLog>(connection).UpdateRecord(record);
            }
        }

        [AuthorizeHubRole("Administrator, Owner")]
        public void RestoreDataAuditLog(AuditLog record)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                connection.ExecuteNonQuery($"UPDATE {record.TableName} SET {record.ColumnName} = '{record.OriginalValue}' WHERE {record.PrimaryKeyColumn} = {record.PrimaryKeyValue}");
            }
        }

        [AuthorizeHubRole("Administrator, Owner")]
        public void RestoreMultipleDataAuditLog(List<int> IDs)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                foreach (int id in IDs)
                {
                    DataRow record = connection.RetrieveRow("SELECT * FROM AuditLog WHERE ID = {0}", id);
                    connection.ExecuteNonQuery($"UPDATE {record["TableName"]} SET {record["ColumnName"]} = '{record["OriginalValue"]}' WHERE {record["PrimaryKeyColumn"]} = {record["PrimaryKeyValue"]}");
                }
            }
        }


        #endregion

        #region [ DataFile Table Operations ]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(DataFile), RecordOperation.QueryRecordCount)]
        public int QueryDataFileCount(string filterString)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                const string QueryFormat =
                "SELECT COUNT(*) " +
                "FROM " +
                "( " +
                "    SELECT DISTINCT FileGroupID " +
                "    FROM DataFile " +
                "    WHERE {0} " +
                ") DataFile";

                TableOperations<DataFile> dataFileTable = new TableOperations<DataFile>(connection);
                RecordRestriction restriction = dataFileTable.GetSearchRestriction(filterString);
                string query = string.Format(QueryFormat, restriction?.FilterExpression ?? "1=1");
                return connection.ExecuteScalar<int>(query, restriction?.Parameters ?? new object[0]);
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(DataFile), RecordOperation.QueryRecords)]
        public IEnumerable<DataFile> QueryDataFiles(string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                const string QueryFormat =
                "WITH cte AS " +
                "( " +
                "    SELECT * " +
                "    FROM DataFile " +
                "    WHERE {0} " +
                ") " +
                "SELECT DataFile.* " +
                "FROM " +
                "( " +
                "    SELECT " +
                "        ROW_NUMBER() OVER(ORDER BY {2}) AS RowNumber, " +
                "        DataFile.* " +
                "    FROM (SELECT DISTINCT FileGroupID ID FROM cte) FileGroup CROSS APPLY " +
                "    ( " +
                "        SELECT TOP 1 * " +
                "        FROM cte DataFile " +
                "        WHERE DataFile.FileGroupID = FileGroup.ID " +
                "        ORDER BY FileSize DESC, FilePath " +
                "    ) DataFile " +
                ") DataFile " +
                "WHERE {1} " +
                "ORDER BY RowNumber";

                TableOperations<DataFile> dataFileTable = new TableOperations<DataFile>(connection);

                RecordRestriction searchRestriction = dataFileTable.GetSearchRestriction(filterString)
                    ?? new RecordRestriction("1=1");

                string searchClause = searchRestriction.FilterExpression;

                int paramIndex = searchRestriction.Parameters.Length;
                string pageClause = $"RowNumber BETWEEN {{{paramIndex}}} AND {{{paramIndex + 1}}}";

                string sortOrder = ascending ? "ASC" : "DESC";
                string orderByClause = $"{sortField} {sortOrder}";
                string query = string.Format(QueryFormat, searchClause, pageClause, orderByClause);

                int pageStart = (page - 1) * pageSize + 1;
                int pageEnd = pageStart + pageSize - 1;

                object[] parameters = searchRestriction.Parameters
                    .Concat(new object[] { pageStart, pageEnd })
                    .ToArray();

                return connection
                    .RetrieveData(query, parameters)
                    .AsEnumerable()
                    .Select(dataFileTable.LoadRecord);
            }
        }

        public IEnumerable<Event> GetEventsByDataFile (int dataFileId)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                return new TableOperations<Event>(connection).QueryRecords(restriction: new RecordRestriction("FileGroupID IN (SELECT FileGroupID FROM DataFile WHERE ID = {0})", dataFileId)).ToList();
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(DataFile), RecordOperation.DeleteRecord)]
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
        [RecordOperation(typeof(DataFile), RecordOperation.CreateNewRecord)]
        public DataFile NewDataFile()
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                return new TableOperations<DataFile>(connection).NewRecord();
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(DataFile), RecordOperation.AddNewRecord)]
        public void AddNewDataFile(DataFile record)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                new TableOperations<DataFile>(connection).AddNewRecord(record);
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(DataFile), RecordOperation.UpdateRecord)]
        public void UpdateDataFile(DataFile record)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                new TableOperations<DataFile>(connection).UpdateRecord(record);
            }
        }

        public IEnumerable<Event> GetEventsForFileGroup(int fileGroupId)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                return new TableOperations<Event>(connection).QueryRecords(restriction: new RecordRestriction("FileGroupID ={0}", fileGroupId)).ToList();
            }
        }

        public IEnumerable<Event> GetEventsForFileGroups(List<int> fileGroupIds)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                string ids = String.Join(",", fileGroupIds);
                return new TableOperations<Event>(connection).QueryRecords(restriction: new RecordRestriction($"FileGroupID IN ({ids})")).ToList();
            }
        }


        public void ReprocessFiles(List<int> meterIDs, Tuple<DateTime, DateTime> dateRange)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                string commaSeparatedMeters = string.Join(",", meterIDs);
                string eventFilter = $"MeterID IN ({commaSeparatedMeters}) AND StartTime >= '{dateRange.Item1}' AND StartTime <= '{dateRange.Item2}'";

                List<Event> events = new TableOperations<Event>(connection)
                    .QueryRecords(new RecordRestriction(eventFilter))
                    .ToList();

                List<Tuple<int, int>> fileGroupIDs = events
                    .Select(evt => Tuple.Create(evt.FileGroupID, evt.MeterID))
                    .Distinct()
                    .ToList();

                foreach (Tuple<int, int> fileGroupID in fileGroupIDs)
                {
                    CascadeDelete("Event", $"FileGroupID = {fileGroupID}");
                    CascadeDelete("EventData", $"FileGroupID = {fileGroupID}");
                    OnReprocessFile(fileGroupID.Item1, fileGroupID.Item2, true);
                }
            }
        }

        public void ReprocessFile(int fileGroupID, bool loadHistoricConfiguration)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                int? meterID = connection.ExecuteScalar<int?>("SELECT MeterID FROM Event WHERE FileGroupID = {0}", fileGroupID);

                if (meterID == null)
                {
                    string[] files = new TableOperations<DataFile>(connection)
                        .QueryRecordsWhere("FileGroupID = {0}", fileGroupID)
                        .Select(dataFile => dataFile.FilePath)
                        .ToArray();

                    string filePattern = connection.ExecuteScalar<string>("SELECT Value FROM Setting WHERE Name = 'FilePattern'")
                        ?? @"(?<AssetKey>[^\\]+)\\[^\\]+$";

                    string meterKey = files
                        .Select(file => Regex.Match(file, filePattern))
                        .Where(match => match.Success)
                        .Select(match => match.Groups["AssetKey"]?.Value)
                        .Where(assetKey => assetKey != null)
                        .FirstOrDefault();

                    if (meterKey != null)
                        meterID = connection.ExecuteScalar<int?>("SELECT ID FROM Meter WHERE AssetKey = {0}", meterKey);
                }

                if (meterID == null)
                    return;

                CascadeDelete("Event", $"FileGroupID = {fileGroupID}");
                CascadeDelete("EventData", $"FileGroupID = {fileGroupID}");

                OnReprocessFile(fileGroupID, meterID.GetValueOrDefault(), loadHistoricConfiguration);
            }
        }

        #endregion

        #region [ StepChangeWebReportSettings Operations]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(StepChangeMeasurement), RecordOperation.QueryRecordCount)]
        public int QueryStepChangeMeasurementCount(string filterString)
        {
            return GetPagedQueryCount(@"
            SELECT
                StepChangeMeasurement.ID,
                Name,
                Setting
            FROM
                StepChangeMeasurement JOIN
                PQMeasurement ON StepChangeMeasurement.PQMeasurementID = PQMeasurement.ID                
            WHERE 
                PQMeasurement.Name LIKE '%' + {0} + '%'
            ", filterString);
            
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(StepChangeMeasurement), RecordOperation.QueryRecords)]
        public DataTable QueryStepChangeMeasurements(string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            return GetPagedQuery(@"
            SELECT
                StepChangeMeasurement.ID,
                Name,
                Setting
            FROM
                StepChangeMeasurement JOIN
                PQMeasurement ON StepChangeMeasurement.PQMeasurementID = PQMeasurement.ID                
            WHERE 
                PQMeasurement.Name LIKE '%' + {0} + '%'
            ", sortField, ascending, page, pageSize, filterString);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(StepChangeMeasurement), RecordOperation.DeleteRecord)]
        public void DeleteStepChangeMeasurement(int id)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                new TableOperations<StepChangeMeasurement>(connection).DeleteRecord(id);
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(StepChangeMeasurement), RecordOperation.CreateNewRecord)]
        public Dictionary<string, object> NewStepChangeMeasurement()
        {
            return new Dictionary<string, object>() {
                {"ID", 0 },
                { "Name", string.Empty },
                { "Setting", 0 }
            };
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(StepChangeMeasurement), RecordOperation.AddNewRecord)]
        public void AddNewStepChangeMeasurement(Dictionary<string, object> record)
        {

        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(StepChangeMeasurement), RecordOperation.UpdateRecord)]
        public void UpdateStepChangeMeasurement(Dictionary<string, object> record)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                StepChangeMeasurement stepChangeMeasurement = new TableOperations<StepChangeMeasurement>(connection).QueryRecordWhere("ID = {0}", record["ID"]);
                stepChangeMeasurement.Setting = double.Parse(record["Setting"].ToString());
                new TableOperations<StepChangeMeasurement>(connection).UpdateRecord(stepChangeMeasurement);
            }
        }

        #endregion

        #region [ PQTrendingWebReportSettings Operations]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(PQMeasurement), RecordOperation.QueryRecordCount)]
        public int QueryPQMeasurementCount(string filterString)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                return new TableOperations<PQMeasurement>(connection).QueryRecordCount(filterString);
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(PQMeasurement), RecordOperation.QueryRecords)]
        public IEnumerable<PQMeasurement> QueryPQMeasurements(string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                return new TableOperations<PQMeasurement>(connection).QueryRecords(sortField, ascending, page, pageSize, filterString).ToList();
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(PQMeasurement), RecordOperation.DeleteRecord)]
        public void DeletePQMeasurement(int id)
        {
            CascadeDelete("PQMeasurement", $"ID = {id}");
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(PQMeasurement), RecordOperation.CreateNewRecord)]
        public PQMeasurement NewPQMeasurement()
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                return new TableOperations<PQMeasurement>(connection).NewRecord();
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(PQMeasurement), RecordOperation.AddNewRecord)]
        public void AddNewPQMeasurement(PQMeasurement record)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                new TableOperations<PQMeasurement>(connection).AddNewRecord(record);
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(PQMeasurement), RecordOperation.UpdateRecord)]
        public void UpdatePQMeasurement(PQMeasurement record)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                new TableOperations<PQMeasurement>(connection).UpdateRecord(record);
            }
        }

        #endregion

        #endregion

        #region [ DataPusher Operations ]

        #region [ MetersToDataPush Table Operations ]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(MetersToDataPush), RecordOperation.QueryRecordCount)]
        public int QueryMetersToDataPushCount(int remoteXDAInstanceId, string filterString)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                TableOperations<MetersToDataPush> table = new TableOperations<MetersToDataPush>(connection);
                RecordRestriction restriction = table.GetSearchRestriction(filterString) + new RecordRestriction("ID IN (SELECT MetersToDataPushID FROM RemoteXDAInstanceMeter WHERE RemoteXDAInstanceID = {0})", remoteXDAInstanceId);
                return table.QueryRecordCount(restriction);
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(MetersToDataPush), RecordOperation.QueryRecords)]
        public IEnumerable<MetersToDataPush> QueryMetersToDataPushs(int remoteXDAInstanceId, string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                TableOperations<MetersToDataPush> table = new TableOperations<MetersToDataPush>(connection);
                RecordRestriction restriction = table.GetSearchRestriction(filterString) + new RecordRestriction("ID IN (SELECT MetersToDataPushID FROM RemoteXDAInstanceMeter WHERE RemoteXDAInstanceID = {0})", remoteXDAInstanceId);
                return table.QueryRecords(sortField, ascending, page, pageSize, restriction).ToList();
            }
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
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                return new TableOperations<MetersToDataPush>(connection).NewRecord();
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(MetersToDataPush), RecordOperation.AddNewRecord)]
        public void AddNewMetersToDataPush(MetersToDataPush record)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                if (record.Obsfucate)
                    record.RemoteXDAAssetKey = Guid.NewGuid().ToString();
                else
                    record.RemoteXDAAssetKey = record.LocalXDAAssetKey;
                record.Synced = false;

                new TableOperations<MetersToDataPush>(connection).AddNewRecord(record);
                int meterId = connection.ExecuteScalar<int>("SELECT @@IDENTITY");
                new TableOperations<RemoteXDAInstanceMeter>(connection).AddNewRecord(new RemoteXDAInstanceMeter() { RemoteXDAInstanceID = record.RemoteXDAInstanceId, MetersToDataPushID = meterId });
            }
        }

        [AuthorizeHubRole("Administrator, Owner")]
        [RecordOperation(typeof(MetersToDataPush), RecordOperation.UpdateRecord)]
        public void UpdateMetersToDataPush(MetersToDataPush record)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                MetersToDataPush oldrecord = new TableOperations<MetersToDataPush>(connection).QueryRecordWhere("ID = {0}", record.ID);

                if (record.Obsfucate && !oldrecord.Obsfucate)
                    record.RemoteXDAAssetKey = Guid.NewGuid().ToString();
                else if (!record.Obsfucate && oldrecord.Obsfucate)
                    record.RemoteXDAAssetKey = record.LocalXDAAssetKey;

                new TableOperations<MetersToDataPush>(connection).UpdateRecord(record);
            }
        }


        [AuthorizeHubRole("Administrator")]
        public Dictionary<string,string> SearchMetersToDataPushs(string searchText, int limit = -1)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                RecordRestriction restriction = new RecordRestriction("Name LIKE {0}", $"%{searchText}%");

                return new TableOperations<Meter>(connection).QueryRecords("Name", restriction, limit)
                    .ToDictionary(meter => meter.ID.ToString(),meter => meter.AssetKey);
            }
        }

        public Meter GetMeterRecord(int id)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                return new TableOperations<Meter>(connection).QueryRecordWhere("ID = {0}", id);
            }
        }

        //public string GetConnectionId()
        //{
        //    return Context.ConnectionId;
        //}

        [AuthorizeHubRole("Administrator")]
        public void SyncMeterConfigurationForInstance(int instanceId, int meterId)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                string clientId = Context.ConnectionId;
                try
                {
                    // for now, create new instance of DataPusherEngine.  Later have one running in XDA ServiceHost and tie to it to ensure multiple updates arent happening simultaneously
                    DataPusherEngine engine = new DataPusherEngine();
                    RemoteXDAInstance instance = new TableOperations<RemoteXDAInstance>(connection).QueryRecordWhere("ID = {0}", instanceId);
                    MetersToDataPush meter = new TableOperations<MetersToDataPush>(connection).QueryRecordWhere("ID = {0}", meterId);
                    UserAccount userAccount = new TableOperations<UserAccount>(connection).QueryRecordWhere("ID = {0}", instance.UserAccountID);

                    CancellationTokenSource source = new CancellationTokenSource();
                    CancellationToken token = source.Token;

                    engine.SyncMeterConfigurationForInstance(clientId, instance, meter, userAccount, token);

                }
                catch (Exception ex)
                {
                    LogException(ex);
                }
            }
        }

        [AuthorizeHubRole("Administrator")]
        public void SyncMeterFilesForInstance(int instanceId, int meterId)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                string clientId = Context.ConnectionId;

                try
                {
                    // for now, create new instance of DataPusherEngine.  Later have one running in XDA ServiceHost and tie to it to ensure multiple updates arent happening simultaneously
                    DataPusherEngine engine = new DataPusherEngine();
                    RemoteXDAInstance instance = new TableOperations<RemoteXDAInstance>(connection).QueryRecordWhere("ID = {0}", instanceId);

                    CancellationTokenSource source = new CancellationTokenSource();
                    CancellationToken token = source.Token;

                    engine.SyncMeterFilesForInstance(clientId, instance, meterId, token);

                }
                catch (Exception ex)
                {
                    LogException(ex);
                }
            }
        }

        #endregion

        #region [ RemoteXDAInstance Table Operations ]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(RemoteXDAInstance), RecordOperation.QueryRecordCount)]
        public int QueryRemoteXDAInstanceCount(string filterString)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                return new TableOperations<RemoteXDAInstance>(connection).QueryRecordCount(filterString);
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(RemoteXDAInstance), RecordOperation.QueryRecords)]
        public IEnumerable<RemoteXDAInstance> QueryRemoteXDAInstances(string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                return new TableOperations<RemoteXDAInstance>(connection).QueryRecords(sortField, ascending, page, pageSize, filterString).ToList();
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(RemoteXDAInstance), RecordOperation.DeleteRecord)]
        public void DeleteRemoteXDAInstance(int id)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                new TableOperations<RemoteXDAInstance>(connection).DeleteRecord(id);
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(RemoteXDAInstance), RecordOperation.CreateNewRecord)]
        public RemoteXDAInstance NewRemoteXDAInstance()
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                return new TableOperations<RemoteXDAInstance>(connection).NewRecord();
            }
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(RemoteXDAInstance), RecordOperation.AddNewRecord)]
        public void AddNewRemoteXDAInstance(RemoteXDAInstance record)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                new TableOperations<RemoteXDAInstance>(connection).AddNewRecord(record);
            }
        }

        [AuthorizeHubRole("Administrator, Owner")]
        [RecordOperation(typeof(RemoteXDAInstance), RecordOperation.UpdateRecord)]
        public void UpdateRemoteXDAInstance(RemoteXDAInstance record)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                new TableOperations<RemoteXDAInstance>(connection).UpdateRecord(record);
            }
        }

        [AuthorizeHubRole("Administrator")]
        public void SyncInstanceConfiguration(int instanceId)
        {
            // for now, create new instance of DataPusherEngine.  Later have one running in XDA ServiceHost and tie to it to ensure multiple updates arent happening simultaneously
            DataPusherEngine engine = new DataPusherEngine();

            // Define the cancellation token.
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;

            engine.SyncInstanceConfiguration(Context.ConnectionId, instanceId, token);

        }

        [AuthorizeHubRole("Administrator")]
        public void SyncFilesForInstance(int instanceId)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                // for now, create new instance of DataPusherEngine.  Later have one running in XDA ServiceHost and tie to it to ensure multiple updates arent happening simultaneously
                DataPusherEngine engine = new DataPusherEngine();
                RemoteXDAInstance instance = new TableOperations<RemoteXDAInstance>(connection).QueryRecordWhere("ID = {0}", instanceId);

                CancellationTokenSource source = new CancellationTokenSource();
                CancellationToken token = source.Token;

                engine.SyncInstanceFiles(Context.ConnectionId, instance, token);
            }
        }

        #endregion

        #endregion

        #region [ Reports Operations ]

        public int FailedReportCount(int month, int year) {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                return new TableOperations<Report>(connection).QueryRecordCountWhere("Month = {0} AND Year = {1} AND Results = 'Fail'", month, year);
            }
        }

        public DataTable GetReports(int month, int year)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                return connection.RetrieveData(
                @"
                    SELECT
                        Report.ID as ReportID,
	                    Meter.AssetKey as Meter,
	                    MeterLocation.Name as Location,
	                    Line.VoltageKV * 1000 as Voltage,
	                    Report.Results as Result
                    FROM
	                    Report JOIN
	                    Meter ON Report.MeterID = Meter.ID JOIN
	                    MeterLocation ON Meter.MeterLocationID = MeterLocation.ID JOIN
	                    MeterLine ON Meter.ID = MeterLine.MeterID JOIN
	                    Line ON MeterLine.LineID = Line.ID 
                    WHERE
                        Report.Month = {0} AND Report.Year = {1}
                    ORDER BY Report.Results, Meter.AssetKey
                    ", month, year);
            }
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
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {

                string timeRange = connection.ExecuteScalar<string>("SELECT TimeRange FROM WorkbenchFilter WHERE ID ={0}", filterId);

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
        }

        private DataTable GetPagedQuery(string query, string sortField, bool ascending, int page, int pageSize, params object[] parameters) {
            List<object> list = new List<object>() { page, sortField};
            list.AddRange(parameters.AsEnumerable());
            DataTable table = new DataTable();
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            using (IDbCommand sc = connection.Connection.CreateCommand())
            {
                int num = 0;
                List<string> paramNames = new List<string>();
                foreach (object o in parameters)
                {
                    IDbDataParameter param = sc.CreateParameter();
                    param.ParameterName = "@p" + num.ToString();
                    paramNames.Add("@p" + num.ToString());
                    param.Value = o;
                    sc.Parameters.Add(param);
                    ++num;
                }
                query = string.Format(query, paramNames.ToArray());

                sc.CommandText = @"
                -- Determine the first record and last record 
                WITH TempResult as
                (
                " + query +
                @"
                ), 
                TempResult2 AS
                (
	                SELECT ROW_NUMBER() OVER(ORDER BY " + $"[{sortField}] {(ascending ? "ASC" : "DESC")}" + @") as RowNum, * FROM TempResult
                )
                SELECT top (@LastRec-1) *
                FROM TempResult2
                WHERE RowNum > @FirstRec 
                AND RowNum < @LastRec
                ";
                sc.CommandType = CommandType.Text;
                sc.CommandTimeout = 600;
                IDbDataParameter param1 = sc.CreateParameter();
                param1.ParameterName = "@FirstRec";
                param1.Value = (page - 1) * pageSize;
                IDbDataParameter param2 = sc.CreateParameter();
                param2.ParameterName = "@LastRec";
                param2.Value = page * pageSize + 1;

                sc.Parameters.Add(param1);
                sc.Parameters.Add(param2);


                IDataReader rdr = sc.ExecuteReader();
                table.Load(rdr);


            }

            return table;
        }

        private int GetPagedQueryCount(string query, params object[] parameters)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                return connection.RetrieveData(query, parameters).Select().Count();
            }
        }


        #endregion
    }
}