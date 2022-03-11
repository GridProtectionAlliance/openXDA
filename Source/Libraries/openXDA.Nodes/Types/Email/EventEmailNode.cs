//******************************************************************************************************
//  EventEmailNode.cs - Gbtc
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
//  02/12/2021 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Controllers;
using FaultData.DataOperations;
using FaultData.DataWriters;
using GSF.Configuration;
using GSF.Data;
using GSF.Data.Model;
using openXDA.Configuration;
using openXDA.Model;

namespace openXDA.Nodes.Types.Email
{
    public class EventEmailNode : NodeBase
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

            [Category]
            [SettingName(EmailSection.CategoryName)]
            public EmailSection EmailSettings { get; } = new EmailSection();

            [Category]
            [SettingName(EventEmailSection.CategoryName)]
            public EventEmailSection EventEmailSettings { get; } = new EventEmailSection();
        }

        private class EventEmailWebController : ApiController
        {
            private EventEmailNode Node { get; }

            public EventEmailWebController(EventEmailNode node) =>
                Node = node;

            [HttpPost]
            public void TriggerForFileGroup(int fileGroupID, int processingVersion) =>
                Node.Process(fileGroupID, processingVersion);

            [HttpPost]
            public void TriggerForEvents([FromUri] List<int> eventIDs) =>
                Node.Process(eventIDs);

            [HttpPost]
            public void RestoreEventEmails() =>
                Node.Restore();
        }

        // Fields
        private List<EventEmailProcessor> m_emailTypes;
        private int m_tripped;

        #endregion

        #region [ Constructors ]

        public EventEmailNode(Host host, Node definition, NodeType type)
            : base(host, definition, type)
        {
            m_emailTypes = new List<EventEmailProcessor>();
        }

        #endregion

        #region [ Properties ]

        private EventEmailService EmailService { get; }

        private List<EventEmailProcessor> EmailTypes
        {
            get => Interlocked.CompareExchange(ref m_emailTypes, null, null);
            set => Interlocked.Exchange(ref m_emailTypes, value);
        }

        private ConcurrentQueue<DateTime> TaggedEmails { get; }

        private bool Tripped
        {
            get => Interlocked.CompareExchange(ref m_tripped, 0, 0) != 0;
            set => Interlocked.Exchange(ref m_tripped, value ? 1 : 0);
        }

        #endregion

        #region [ Methods ]

        public override IHttpController CreateWebController() =>
            new EventEmailWebController(this);

        /// <summary>
        /// Process All Emails Belonging to a given FileGroup
        /// </summary>
        /// <param name="fileGroupID"></param>
        /// <param name="processingVersion"></param>
        private void Process(int fileGroupID, int processingVersion)
        {
            using (AdoDataConnection connection = CreateDbConnection())
            {
                TableOperations<Event> eventTable = new TableOperations<Event>(connection);
                IEnumerable<Event> events = eventTable.QueryRecordsWhere("FileGroupID = {0} AND FileVersion = {1}", fileGroupID, processingVersion);
                Process(events);
            }
        }

        /// <summary>
        /// Process all emails belonging to a given List of Event IDs
        /// </summary>
        /// <param name="eventIDs"></param>
        private void Process(List<int> eventIDs)
        {
            using (AdoDataConnection connection = CreateDbConnection())
            {
                IEnumerable<string> formatParameters = eventIDs.Select((_, index) => $"{{{index}}}");
                string idList = string.Join(",", formatParameters);
                object[] queryParameters = eventIDs.Cast<object>().ToArray();
                TableOperations<Event> eventTable = new TableOperations<Event>(connection);
                IEnumerable<Event> events = eventTable.QueryRecordsWhere($"ID IN ({idList})", queryParameters);
                Process(events);
            }
        }

        /// <summary>
        /// Process all emails belonging to a given List of Event
        /// </summary>
        private void Process(IEnumerable<Event> events)
        {
            UpdateEmailTypes();

            List<EventEmailProcessor> triggeredEmailTypes = EmailTypes.ToList();

            foreach (Event evt in events)
            {
                foreach (EventEmailProcessor emailProcessor in triggeredEmailTypes)
                    emailProcessor.Process(evt);
            }
        }

        public void StartTimer()
        {
            foreach (EventEmailProcessor emailType in EmailTypes)
                emailType.StartTimer();
        }

        public void StopTimer()
        {
            foreach (EventEmailProcessor emailType in EmailTypes)
                emailType.StopTimer();
        }

        private void UpdateEmailTypes()
        {
            List<EventEmailProcessor> emailTypes = new List<EventEmailProcessor>(EmailTypes);
            List<EmailType> parametersList;

            using (AdoDataConnection connection = CreateDbConnection())
            {
                TableOperations<EmailType> eventEmailTypeTable = new TableOperations<EmailType>(connection);
                parametersList = eventEmailTypeTable.QueryRecords().ToList();
            }

            List<EmailType> newParameters = parametersList
                .Where(parameters => !emailTypes.Any(emailType => emailType.EmailTypeID == parameters.ID))
                .ToList();

            List<EventEmailProcessor> newEmailTypes = newParameters
                .Select(parameters => new EventEmailProcessor(SendEmail) { EmailModel = parameters, ConnectionFactory = CreateDbConnection })
                .ToList();

            foreach (EventEmailProcessor emailType in emailTypes)
            {
                EmailType parametersUpdate = parametersList
                    .Where(parameters => parameters.ID == emailType.EmailTypeID)
                    .FirstOrDefault();

                if ((object)parametersUpdate == null)
                    continue;

                emailType.EmailModel = parametersUpdate;
                emailType.ConnectionFactory = CreateDbConnection;
                newEmailTypes.Add(emailType);
            }

            EmailTypes = newEmailTypes;
        }

        /// <summary>
        /// Logic to Send the Email.
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="events"></param>
        private void SendEmail(EmailType parameters, List<Event> events)
        {
            

            while (events.Any())
            {
                int nextEventID = events[0].ID;

                List<int> eventIDs = new List<int>(events[0].ID);
                try
                {
                    using (AdoDataConnection connection = CreateDbConnection())
                    {
                        DataTable dT = connection.RetrieveData(parameters.CombineEventsSQL, events[0].ID);

                        foreach (DataRow row in dT.Rows)
                            eventIDs.Add(row.Field<int>(0));
                    }
                }
                catch (Exception ex)
                {
                    continue;
                }

                Event evt = events[0];
                events.RemoveAll(e => eventIDs.Contains(e.ID));

               
                Action<object> configurator = GetConfigurator();
                Settings settings = new Settings(configurator);
                EventEmailSection eventEmailSettings = settings.EventEmailSettings;

                if (!eventEmailSettings.Enabled || Tripped)
                    return;

                if (TripsMaxEmailThreshold(eventEmailSettings))
                {
                    SendTripNotification(eventEmailSettings);
                    return;
                }

                DateTime now = DateTime.UtcNow;
                TimeZoneConverter timeZoneConverter = new TimeZoneConverter(configurator);
                DateTime xdaNow = timeZoneConverter.ToXDATimeZone(now);
                EmailService.SendEmail(parameters, eventIDs, evt, xdaNow );
                   
                DailyStatisticOperation.UpdateEmailProcessingStatistic(eventIDs);
                
                
            }
        }

        private bool TripsMaxEmailThreshold(EventEmailSection eventEmailSettings)
        {
            DateTime now = DateTime.UtcNow;
            int maxEmailCount = eventEmailSettings.MaxEmailCount;
            TimeSpan maxEmailSpan = eventEmailSettings.MaxEmailSpan;

            if (maxEmailCount <= 0 || maxEmailSpan <= TimeSpan.Zero)
                return false;

            DateTime oldestTag = now - maxEmailSpan;

            while (TaggedEmails.TryPeek(out DateTime tag) && tag < oldestTag)
                TaggedEmails.TryDequeue(out tag);

            if (TaggedEmails.Count < maxEmailCount)
            {
                TaggedEmails.Enqueue(now);
                return false;
            }

            Tripped = true;
            return true;
        }

        private void Restore()
        {
            while (!TaggedEmails.IsEmpty)
                TaggedEmails.TryDequeue(out _);

            Tripped = false;
        }

        private void SendTripNotification(EventEmailSection eventEmailSettings)
        {
            string subject = "openXDA email flooding detected";
            StringBuilder message = new StringBuilder();
            List<string> replyTo = new List<string>();
            string xdaInstance = "http://localhost:8989";

            using (AdoDataConnection connection = CreateDbConnection())
            {
                TableOperations<DashSettings> dashSettingsTable = new TableOperations<DashSettings>(connection);
                DashSettings xdaInstanceSetting = dashSettingsTable.QueryRecordWhere("Name = 'System.XDAInstance'");

                if ((object)xdaInstanceSetting != null)
                    xdaInstance = xdaInstanceSetting.Value.TrimEnd('/');

                TableOperations<EmailType> emailTypeTable = new TableOperations<EmailType>(connection);
                string emailTypeIDs = string.Join(",", m_emailTypes.Select(type => type.EmailTypeID));
                int eventEmailCategoryID = connection.ExecuteScalar<int>("SELECT ID FROM EmailCategory WHERE Name = 'Event'");
                IEnumerable<EmailType> emailTypes = emailTypeTable.QueryRecordsWhere($"ID IN ({emailTypeIDs}) AND EmailCategoryID = {eventEmailCategoryID}");

                foreach (EmailType emailType in emailTypes)
                {
                    emailType.SMS = false;
                    List<string> recipients = EmailService.GetRecipients(emailType);
                    replyTo.AddRange(recipients);
                }
            }

            int maxEmailCount = eventEmailSettings.MaxEmailCount;
            TimeSpan maxEmailSpan = eventEmailSettings.MaxEmailSpan;
            string maxEmailSpanText = maxEmailSpan.ToString();

            if (maxEmailSpan.Ticks % TimeSpan.TicksPerDay == 0)
                maxEmailSpanText = $"{maxEmailSpan.Days} {(maxEmailSpan.Days == 1 ? "day" : "days")}";
            else if (maxEmailSpan.Ticks % TimeSpan.TicksPerHour == 0)
                maxEmailSpanText = $"{maxEmailSpan.Hours} {(maxEmailSpan.Hours == 1 ? "hour" : "hours")}";
            else if (maxEmailSpan.Ticks % TimeSpan.TicksPerMinute == 0)
                maxEmailSpanText = $"{maxEmailSpan.Minutes} {(maxEmailSpan.Minutes == 1 ? "minute" : "minutes")}";
            else if (maxEmailSpan.Ticks % TimeSpan.TicksPerSecond == 0)
                maxEmailSpanText = $"{maxEmailSpan.Seconds} {(maxEmailSpan.Seconds == 1 ? "second" : "seconds")}";

            message.AppendLine($"openXDA has detected that over {maxEmailCount} emails were sent within {maxEmailSpanText}.");
            message.AppendLine();
            message.AppendLine($"Event email notifications have been disabled until further notice.");
            message.AppendLine($"Visit the following page to restore event email notifications.");
            message.AppendLine($"{xdaInstance}/RestoreEventEmail.cshtml");
            message.AppendLine();
            message.AppendLine($"Reply to this message to send a message to all event email subscribers.");
            EmailService.SendAdminEmail(subject, message.ToString(), replyTo);
        }


        #endregion
    }
}
