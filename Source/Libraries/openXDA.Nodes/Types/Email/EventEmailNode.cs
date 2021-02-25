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
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Xml.Linq;
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
            public void TriggerForFileGroup(int fileGroupID, int processingVersion, string triggerSource = "") =>
                Node.Process(fileGroupID, processingVersion, triggerSource);

            [HttpPost]
            public void TriggerForEvents([FromUri] List<int> eventIDs, string triggerSource = "") =>
                Node.Process(eventIDs, triggerSource);

            [HttpPost]
            public void RestoreEventEmails() =>
                Node.Restore();
        }

        // Fields
        private List<EventEmailType> m_emailTypes;
        private int m_tripped;

        #endregion

        #region [ Constructors ]

        public EventEmailNode(Host host, Node definition, NodeType type)
            : base(host, definition, type)
        {
        }

        #endregion

        #region [ Properties ]

        private EventEmailService EventEmailService { get; }

        private List<EventEmailType> EmailTypes
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

        private void Process(int fileGroupID, int processingVersion, string triggerSource)
        {
            using (AdoDataConnection connection = CreateDbConnection())
            {
                TableOperations<Event> eventTable = new TableOperations<Event>(connection);
                IEnumerable<Event> events = eventTable.QueryRecordsWhere("FileGroupID = {0} AND FileVersion = {1}", fileGroupID, processingVersion);
                Process(events, triggerSource);
            }
        }

        private void Process(List<int> eventIDs, string triggerSource)
        {
            using (AdoDataConnection connection = CreateDbConnection())
            {
                IEnumerable<string> formatParameters = eventIDs.Select((_, index) => $"{{{index}}}");
                string idList = string.Join(",", formatParameters);
                object[] queryParameters = eventIDs.Cast<object>().ToArray();
                TableOperations<Event> eventTable = new TableOperations<Event>(connection);
                IEnumerable<Event> events = eventTable.QueryRecordsWhere($"ID IN ({idList})", queryParameters);
                Process(events, triggerSource);
            }
        }

        private void Process(IEnumerable<Event> events, string triggerSource)
        {
            UpdateEmailTypes();

            List<EventEmailType> triggeredEmailTypes = EmailTypes
                .Where(emailType => string.Equals(emailType.Parameters.TriggerSource, triggerSource, StringComparison.OrdinalIgnoreCase))
                .ToList();

            foreach (Event evt in events)
            {
                foreach (EventEmailType emailType in triggeredEmailTypes)
                    emailType.Process(evt);
            }
        }

        public void StartTimer()
        {
            foreach (EventEmailType emailType in EmailTypes)
                emailType.StartTimer();
        }

        public void StopTimer()
        {
            foreach (EventEmailType emailType in EmailTypes)
                emailType.StopTimer();
        }

        private void UpdateEmailTypes()
        {
            List<EventEmailType> emailTypes = new List<EventEmailType>(EmailTypes);
            List<EventEmailParameters> parametersList;

            using (AdoDataConnection connection = CreateDbConnection())
            {
                TableOperations<EventEmailParameters> eventEmailParametersTable = new TableOperations<EventEmailParameters>(connection);
                parametersList = eventEmailParametersTable.QueryRecords().ToList();
                parametersList.ForEach(parameters => parameters.ConnectionFactory = CreateDbConnection);
            }

            List<EventEmailParameters> newParameters = parametersList
                .Where(parameters => !emailTypes.Any(emailType => emailType.EmailTypeID == parameters.EmailTypeID))
                .ToList();

            List<EventEmailType> newEmailTypes = newParameters
                .Select(parameters => new EventEmailType(SendEmail) { Parameters = parameters })
                .ToList();

            foreach (EventEmailType emailType in emailTypes)
            {
                EventEmailParameters parametersUpdate = parametersList
                    .Where(parameters => parameters.EmailTypeID == emailType.EmailTypeID)
                    .FirstOrDefault();

                if ((object)parametersUpdate == null)
                    continue;

                emailType.Parameters = parametersUpdate;
                newEmailTypes.Add(emailType);
            }

            EmailTypes = newEmailTypes;
        }

        private void SendEmail(EventEmailParameters parameters, List<Event> events)
        {
            EmailType emailType;

            using (AdoDataConnection connection = CreateDbConnection())
            {
                TableOperations<EmailType> emailTypeTable = new TableOperations<EmailType>(connection);
                emailType = emailTypeTable.QueryRecordWhere("ID = {0}", parameters.EmailTypeID);

                if ((object)emailType == null)
                    return;

                // Because we are able to rely on event email parameters,
                // we don't really need to check the email category,
                // but it should be validated for the sake of correctness
                TableOperations<EmailCategory> emailCategoryTable = new TableOperations<EmailCategory>(connection);
                EmailCategory emailCategory = emailCategoryTable.QueryRecordWhere("ID = {0}", emailType.EmailCategoryID);

                if (emailCategory.Name != "Event")
                    return;
            }

            while (events.Any())
            {
                int nextEventID = events[0].ID;
                string templateData = parameters.GetEventDetail(nextEventID);

                List<int> eventIDs = EventEmailService.GetEventIDs(templateData);
                eventIDs.Add(nextEventID);
                events.RemoveAll(evt => eventIDs.Contains(evt.ID));

                List<string> recipients = EventEmailService.GetRecipients(emailType, eventIDs);
                XDocument htmlDocument = EventEmailService.ApplyTemplate(emailType, templateData);
                List<Attachment> attachments = new List<Attachment>();

                try
                {
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

                    EventEmailService.ApplyChartTransform(attachments, htmlDocument);
                    EventEmailService.ApplyFTTTransform(attachments, htmlDocument);
                    EventEmailService.SendEmail(recipients, htmlDocument, attachments);

                    DateTime now = DateTime.UtcNow;
                    TimeZoneConverter timeZoneConverter = new TimeZoneConverter(configurator);
                    DateTime xdaNow = timeZoneConverter.ToXDATimeZone(now);
                    EventEmailService.LoadSentEmail(xdaNow, recipients, htmlDocument, eventIDs);
                }
                finally
                {
                    attachments?.ForEach(attachment => attachment.Dispose());
                }
            }
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
                    List<string> recipients = EventEmailService.GetAllRecipients(emailType);
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
            EventEmailService.SendAdminEmail(subject, message.ToString(), replyTo);
        }

        private void Restore()
        {
            while (!TaggedEmails.IsEmpty)
                TaggedEmails.TryDequeue(out _);

            Tripped = false;
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

        #endregion
    }
}
