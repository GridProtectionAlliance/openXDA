//******************************************************************************************************
//  ScheduledEmailNode.cs - Gbtc
//
//  Copyright © 2021, Grid Protection Alliance.  All Rights Reserved.
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
//  02/08/2021 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using FaultData;
using FaultData.DataWriters.Emails;
using GSF.Configuration;
using GSF.Data;
using GSF.Data.Model;
using GSF.Scheduling;
using log4net;
using openXDA.Configuration;
using openXDA.Model;

namespace openXDA.Nodes.Types.Email
{
    public class ScheduledEmailNode : NodeBase, IDisposable
    {
        #region [ Members ]

        // Nested Types
        private class Settings
        {
            public Settings(Action<object> configure) =>
                configure(this);

            [Category]
            [SettingName(SystemSection.CategoryName)]
            public SystemSection SystemSettings { get; } = new SystemSection();
        }

        private class ScheduledEmailWebController : ApiController
        {
            private ScheduledEmailNode Node { get; }

            public ScheduledEmailWebController(ScheduledEmailNode node) =>
                Node = node;

            [HttpGet]
            public HttpResponseMessage Schedules()
            {
                string status = Node.ScheduleManager.Status;
                HttpResponseMessage response = new HttpResponseMessage();
                response.Content = new StringContent(status);
                return response;
            }
        }

        #endregion

        #region [ Constructors ]

        public ScheduledEmailNode(Host host, Node definition, NodeType type)
            : base(host, definition, type)
        {
            ScheduleManager = new ScheduleManager();
            ScheduleManager.ScheduleDue += ScheduleManager_ScheduleDue;
            ScheduleManager.Start();
            Action<object> configurator = GetConfigurator();
            OnReconfigure(configurator);
        }

        #endregion

        #region [ Properties ]

        private ScheduleManager ScheduleManager { get; }
        private bool IsDisposed { get; set; }

        #endregion

        #region [ Methods ]

        public override IHttpController CreateWebController() =>
            new ScheduledEmailWebController(this);

        public void Dispose()
        {
            if (IsDisposed)
                return;

            ScheduleManager.Dispose();
            IsDisposed = true;
        }

        protected override void OnReconfigure(Action<object> configurator)
        {
            List<Schedule> schedules = ScheduleManager.Schedules;
            lock (schedules) { schedules.Clear(); }

            using (AdoDataConnection connection = CreateDbConnection())
            using (DataTable table = connection.RetrieveData("SELECT ID, Name, Schedule FROM ScheduledEmailType"))
            {
                foreach (DataRow row in table.Rows)
                {
                    int id = row.ConvertField<int>("ID");
                    string name = row.ConvertField<string>("Name");
                    string scheduleRule = row.ConvertField<string>("Schedule");
                    string scheduleName = $"[{id}] {name}";
                    ScheduleManager.AddSchedule(scheduleName, scheduleRule);
                }
            }
        }

        private void ScheduleManager_ScheduleDue(object sender, GSF.EventArgs<Schedule> e)
        {
            const string Pattern = @"\[(?<ID>\d+)\] (?<Name>.*)";
            string scheduleName = e.Argument.Name;
            Match match = Regex.Match(scheduleName, Pattern);

            if (!match.Success)
                return;

            string idMatch = match.Groups["ID"].Value;

            if (!int.TryParse(idMatch, out int id))
                return;

            DateTime currentScheduled = e.Argument.LastDueAt;

            // TODO: Think about tracking statistics on outstanding scheduled email tasks
            _ = Task.Run(() =>
            {
                try { ProcessScheduledEmail(id, currentScheduled); }
                catch (Exception ex) { LogException(ex); }
            });
        }

        private void ProcessScheduledEmail(int scheduledEmailID, DateTime now)
        {
            Action<object> configurator = GetConfigurator();
            Settings settings = new Settings(configurator);

            using (AdoDataConnection connection = CreateDbConnection())
            {
                TableOperations<ScheduledEmailType> scheduledEmailTypeTable = new TableOperations<ScheduledEmailType>(connection);
                ScheduledEmailType scheduledEmailType = scheduledEmailTypeTable.QueryRecordWhere("ID = {0}", scheduledEmailID);

                if (scheduledEmailType is null)
                    return;

                // Convert Times used to what ever the time zone is
                TimeZoneConverter timeZoneConverter = new TimeZoneConverter(configurator);
                DateTime xdaNow = timeZoneConverter.ToXDATimeZone(now);

                bool triggersEmail = connection.ExecuteScalar<bool>(scheduledEmailType.TriggerEmailSQL, xdaNow);

                if (!triggersEmail)
                    return;

                ScheduledEmailService emailService = new ScheduledEmailService(CreateDbConnection, configurator);

                emailService.SendEmail(scheduledEmailType, xdaNow);
            }
        }

        private void LogException(Exception ex) =>
            Log.Error(ex.Message, ex);

        #endregion

        #region [ Static ]

        // Static Fields
        private static readonly ILog Log = LogManager.GetLogger(typeof(ScheduledEmailNode));

        #endregion
    }
}
