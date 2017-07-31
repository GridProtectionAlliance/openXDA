//******************************************************************************************************
//  DataHub.cs - Gbtc
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
//  05/23/2017 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************

using XDAAlarmCreationApp.Model;
using GSF;
using GSF.Data;
using GSF.Data.Model;
using GSF.IO;
using GSF.Web.Hubs;
using GSF.Web.Model.HubOperations;
using GSF.Web.Security;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Web.Script;
using CsvHelper;
using Microsoft.AspNet.SignalR;
using GSF.Web.Model;

namespace XDAAlarmCreationApp
{
    public class DataHub : RecordOperationsHub<DataHub>, IDirectoryBrowserOperations
    {
        #region [ Constructors ]

        public DataHub() : base(Program.LogStatusMessage, Program.LogExceptionMessage)
        {
            Action<string, UpdateType> logStatusMessage = (message, updateType) => LogStatusMessage(message, updateType);
            Action<Exception> logException = ex => LogException(ex);

        }

        #endregion

        #region [ Static ]

        // Static Properties

        /// <summary>
        /// Gets the hub connection ID for the current thread.
        /// </summary>
        public static string CurrentConnectionID => s_connectionID.Value;

        // Static Fields
        private static readonly ThreadLocal<string> s_connectionID;
        private static volatile int s_connectCount;
        private static bool s_massImportWaitFlag = false;


        // Static Constructor
        static DataHub()
        {
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string ecaClientDataPath = Path.Combine(appData, "Grid Protection Alliance", "XDA Alarm Client");
            s_connectionID = new ThreadLocal<string>();
        }

        public static void ProgressUpdate(string name, int update)
        {
            GlobalHost.ConnectionManager.GetHubContext<DataHub>().Clients.All.updateProgressBar(name, update);
        }


        #endregion

        #region [ Setting Table Operations ]

        [RecordOperation(typeof(Setting), RecordOperation.QueryRecordCount)]
        public int QuerySettingCount(string filterText)
        {
            return DataContext.Table<Setting>().QueryRecordCount(filterText);
        }

        [RecordOperation(typeof(Setting), RecordOperation.QueryRecords)]
        public IEnumerable<Setting> QuerySetting(string sortField, bool ascending, int page, int pageSize, string filterText)
        {
            return DataContext.Table<Setting>().QueryRecords(sortField, ascending, page, pageSize, filterText);
        }

        [AuthorizeHubRole("Administrator, Editor")]
        [RecordOperation(typeof(Setting), RecordOperation.DeleteRecord)]
        public void DeleteSetting(int id)
        {
            DataContext.Table<Setting>().DeleteRecord(id);
        }

        [RecordOperation(typeof(Setting), RecordOperation.CreateNewRecord)]
        public Setting NewSetting()
        {
            return DataContext.Table<Setting>().NewRecord();
        }

        [AuthorizeHubRole("Administrator, Editor")]
        [RecordOperation(typeof(Setting), RecordOperation.AddNewRecord)]
        public void AddNewSetting(Setting record)
        {
            DataContext.Table<Setting>().AddNewRecord(record);
        }

        [AuthorizeHubRole("Administrator, Editor")]
        [RecordOperation(typeof(Setting), RecordOperation.UpdateRecord)]
        public void UpdateSetting(Setting record)
        {
            DataContext.Table<Setting>().UpdateRecord(record);
        }

        [AuthorizeHubRole("Administrator, Editor")]
        public void AddNewOrUpdateSetting(Setting record)
        {
            DataContext.Table<Setting>().AddNewOrUpdateRecord(record);
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

        #region [ Security Table Operations ]

        [RecordOperation(typeof(Security), RecordOperation.QueryRecordCount)]
        public int QuerySecurityCount(string filterText)
        {
            IEnumerable<Security> table = new List<Security>
            {
                new Security() {
                ID = 1,
                Name = "Users"
                },
                new Security() {
                ID = 2,
                Name = "Groups"
                }
            };

            return table.Count();
        }

        [RecordOperation(typeof(Security), RecordOperation.QueryRecords)]
        public IEnumerable<Security> QuerySecurity(string sortField, bool ascending, int page, int pageSize, string filterText)
        {
            return new List<Security>
            {
                new Security() {
                ID = 1,
                Name = "Users"
                },
                new Security() {
                ID = 2,
                Name = "Groups"
                }
            };
        }

        [AuthorizeHubRole("Administrator, Editor")]
        [RecordOperation(typeof(Security), RecordOperation.DeleteRecord)]
        public void DeleteSecurity(int id)
        {
        }

        [RecordOperation(typeof(Security), RecordOperation.CreateNewRecord)]
        public Security NewSecurity()
        {
            return new Security();
        }

        [AuthorizeHubRole("Administrator, Editor")]
        [RecordOperation(typeof(Security), RecordOperation.AddNewRecord)]
        public void AddNewSecurity(Security record)
        {
        }

        [AuthorizeHubRole("Administrator, Editor")]
        [RecordOperation(typeof(Security), RecordOperation.UpdateRecord)]
        public void UpdateSecurity(Security record)
        {
        }

        [AuthorizeHubRole("Administrator, Editor")]
        public void AddNewOrUpdateSecurity(Security record)
        {
        }

        #endregion

        #region [ DirectoryBrowser Hub Operations ]

        public IEnumerable<string> LoadDirectories(string rootFolder, bool showHidden, bool showFiles)
        {
            if (string.IsNullOrWhiteSpace(rootFolder))
                return Directory.GetLogicalDrives();

            IEnumerable<string> directories = Directory.GetDirectories(rootFolder);
            IEnumerable<string> files = Directory.GetFiles(rootFolder);
            if (!showHidden)
                directories = directories.Where(path => !new DirectoryInfo(path).Attributes.HasFlag(FileAttributes.Hidden));

            IEnumerable<string> returnString = new[] { "..\\" }.Concat(directories.Select(path => FilePath.AddPathSuffix(FilePath.GetLastDirectoryName(path))));

            if (showFiles)
                returnString = returnString.Concat(files.Select(file => file.Split('\\')[file.Split('\\').Count() - 1]));

            return returnString;
        }

        public IEnumerable<string> LoadDirectories(string rootFolder, bool showHidden) => DirectoryBrowserOperations.LoadDirectories(rootFolder, showHidden);
        public bool IsLogicalDrive(string path) => DirectoryBrowserOperations.IsLogicalDrive(path);
        public string ResolvePath(string path) => DirectoryBrowserOperations.ResolvePath(path);
        public string CombinePath(string path1, string path2) => DirectoryBrowserOperations.CombinePath(path1, path2);
        public void CreatePath(string path) => DirectoryBrowserOperations.CreatePath(path);

        #endregion

        #region [ Miscellaneous Hub Operations ]

        /// <summary>
        /// Gets UserAccount table ID for current user.
        /// </summary>
        /// <returns>UserAccount.ID for current user.</returns>
        public Guid GetCurrentUserID()
        {
            Guid userID;
            AuthorizationCache.UserIDs.TryGetValue(Thread.CurrentPrincipal.Identity.Name, out userID);
            return userID;
        }

        /// <summary>
        /// Gets the current server time.
        /// </summary>
        /// <returns>Current server time.</returns>
        public DateTime GetServerTime() => DateTime.UtcNow;

        private void CascadeDelete(string tableName, string criterion)
        {

            using (IDbCommand sc = DataContext.Connection.Connection.CreateCommand())
            {

                //sc.CommandText = "DECLARE @context VARBINARY(128)\n SELECT @context = CONVERT(VARBINARY(128), CONVERT(VARCHAR(128), @userName))\n SET CONTEXT_INFO @context";
                //IDbDataParameter param = sc.CreateParameter();
                //param.ParameterName = "@userName";
                //param.Value = GetCurrentUserName();
                //sc.Parameters.Add(param);
                //sc.ExecuteNonQuery();
                //sc.Parameters.Clear();


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

        private DataTable CSVToDataTable(string fileName)
        {
            var streamReader = new StreamReader(fileName);
            var csv = new CsvParser(streamReader);
            DataTable dt = new DataTable();
            string[] headers = new string[0];
            while (true)
            {
                var row = csv.Read();
                if (row == null) break;

                if (csv.Context.Row == 1)
                {
                    foreach (string header in row)
                        dt.Columns.Add(header, typeof(string));
                    headers = row;
                }
                else
                {
                    var record = dt.NewRow();
                    for (int x = 0; x < row.Count(); ++x)
                    {
                        record[headers[x]] = row[x];
                    }
                    dt.Rows.Add(record);

                }

            }

            return dt;
        }
        #endregion


    }

    public class ServiceHub: Hub
    {
        public class DateTimeArg: EventArgs
        {
            public DateTime DateTime { get; set; }
        }

        #region [ Static Event Handlers ]
        public static event EventHandler<DateTimeArg> GetServerTimeEvent;
        #endregion

        public DateTime GetServerTime()
        {
            DateTimeArg dta = new DateTimeArg();
            GetServerTimeEvent?.Invoke(new object(), dta);
            return dta.DateTime;
        }

        public string GetPerformanceStatistics()
        {
            return null;
        }

        public void SendCommand(string command)
        {
            
        }

    }
}
