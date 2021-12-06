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
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security;
using System.Text;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Xml.Linq;
using FaultData;
using FaultData.DataOperations;
using FaultData.DataWriters;
using FaultData.DataWriters.GTC;
using GSF.Configuration;
using GSF.Data;
using GSF.Data.Model;
using GSF.Xml;
using openXDA.Configuration;
using openXDA.Model;
using openXDA.Model.Emails;
using openXDA.NotificationDataSources;
using SystemCenter.Model;

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

        private EmailService EmailService { get; }

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
                foreach (EventEmailProcessor emailType in triggeredEmailTypes)
                    emailType.Process(evt);
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
            EmailType emailType;
            List<ITriggeredDataSource> dataSources;
            using (AdoDataConnection connection = CreateDbConnection())
            {
                TableOperations<EmailType> emailTypeTable = new TableOperations<EmailType>(connection);
                emailType = emailTypeTable.QueryRecordWhere("ID = {0}", parameters.ID);

                if ((object)emailType == null)
                    return;


                /* Load All DataSources */
                IEnumerable<TriggeredEmailDataSourceEmailType> ds = new TableOperations<TriggeredEmailDataSourceEmailType>(connection).QueryRecordsWhere("EmailTypeID = {0}", parameters.ID);
                if (ds.Count() == 0)
                    return;

                Dictionary<int, TriggeredEmailDataSource> dataSourceModels = new TableOperations<TriggeredEmailDataSource>(connection).QueryRecordsWhere("ID IN ({0})", parameters.ID).ToDictionary((m) => m.ID);
                dataSources = ds.Select(m => { dataSourceModels.TryGetValue(m.TriggeredEmailDataSourceID, out TriggeredEmailDataSource model); return model; })
                    .Where(i => i != null).Select(model => CreatedataSource(model, parameters)).ToList();

            }

            while (events.Any())
            {
                int nextEventID = events[0].ID;

                XElement data = new XElement("data");

                data.Add(dataSources.Select(d => d.Process(events[0])));

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
                  
                events.RemoveAll(evt => eventIDs.Contains(evt.ID));

                List<string> recipients = GetRecipients(emailType, eventIDs);

                XDocument htmlDocument = ApplyTemplate(emailType, data.ToString());

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

                   

                    ApplyChartTransform(attachments, htmlDocument);
                    ApplyFTTTransform(attachments, htmlDocument);
                    EmailService.SendEmail(recipients, htmlDocument, attachments);

                    DateTime now = DateTime.UtcNow;
                    TimeZoneConverter timeZoneConverter = new TimeZoneConverter(configurator);
                    DateTime xdaNow = timeZoneConverter.ToXDATimeZone(now);
                    LoadSentEmail(xdaNow, recipients, htmlDocument, eventIDs);
                    DailyStatisticOperation.UpdateEmailProcessingStatistic(eventIDs);
                }
                finally
                {
                    attachments?.ForEach(attachment => attachment.Dispose());
                }
            }
        }

        private ITriggeredDataSource CreatedataSource(TriggeredEmailDataSource model, EmailType emailModel)
        {
            try
            {

                string assemblyName = model.AssemblyName;
                string typeName = model.TypeName;
                PluginFactory<ITriggeredDataSource> pluginFactory = new PluginFactory<ITriggeredDataSource>();
                return pluginFactory.Create(assemblyName, typeName, this, model, emailModel);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private List<string> GetRecipients(EmailType emailType, List<int> eventIDs)
        {
            string emailAddressQuery;
            List<int> assetGroups = GetAssetgroups(eventIDs).Select(item => item.ID).ToList();

            if (assetGroups.Count == 0)
                return new List<string>();

            if (emailType.SMS)
            {

                Dictionary<int, string> cellCarrierTransforms = CellCarrierTransformations();

                emailAddressQuery =
                   $"SELECT DISTINCT UserAccount.Email AS Email " +
                   $"FROM " +
                   $"    UserAccountEmailType JOIN " +
                   $"    UserAccount ON UserAccountEmailType.UserAccountID = UserAccount.ID " +
                   $"WHERE " +
                   $"    UserAccountEmailType.EmailTypeID = {{0}} AND " +
                   $"    UserAccount.EmailConfirmed <> 0 AND " +
                   $"    UserAccount.Approved <> 0 AND " +
                   $"    UserAccountEmailType.AssetGroupID IN ({{1}}) ";

                // Needs to be implemented at some point
                return new List<string>();
            }
            else
            {
                emailAddressQuery =
                   $"SELECT DISTINCT UserAccount.Email AS Email,  " +
                   $"FROM " +
                   $"    UserAccountEmailType JOIN " +
                   $"    UserAccount ON UserAccountEmailType.UserAccountID = UserAccount.ID " +
                   $"WHERE " +
                   $"    UserAccountEmailType.EmailTypeID = {{0}} AND " +
                   $"    UserAccount.EmailConfirmed <> 0 AND " +
                   $"    UserAccount.Approved <> 0 AND " +
                   $"    UserAccountEmailType.AssetGroupID IN ({{1}}) ";

                using (AdoDataConnection connection = CreateDbConnection())
                    using (DataTable emailAddressTable = connection.RetrieveData(emailAddressQuery, emailType.ID, string.Join(",",assetGroups)))
                    {
                        return emailAddressTable
                            .Select()
                            .Select(row => row.ConvertField<string>("Email"))
                            .ToList();
                    }
            }
        }

        private Dictionary<int, string> CellCarrierTransformations()
        {
            Dictionary<int, string> cellCarrierTransforms = new Dictionary<int, string>();
            return cellCarrierTransforms;
        }

        private List<AssetGroup> GetAssetgroups(List<int> eventIDs)
        {
            if (eventIDs.Count == 0)
                return new List<AssetGroup>();

            using (AdoDataConnection connection = CreateDbConnection())
            {
                string assetCondition = "(SELECT COUNT(*) FROM AssetAssetGroup WHERE AssetID IN(SELECT AssetID FROM Event WHERE ID IN({0})) AND AssetGroupID = AssetGroup.ID) > 0";
                string meterCondition = "(SELECT COUNT(*) FROM MeterAssetGroup WHERE MeterID IN(SELECT MeterID FROM Event WHERE ID IN({0})) AND AssetGroupID = AssetGroup.ID) > 0";
                return new TableOperations<AssetGroup>(connection).QueryRecordsWhere($"{assetCondition} OR {meterCondition}", string.Join(",",eventIDs)).ToList();
            }
        }

        private XDocument ApplyTemplate(EmailType emailType, string templateData)
        {
            string htmlText = templateData.ApplyXSLTransform(emailType.Template);

            XDocument htmlDocument = XDocument.Parse(htmlText, LoadOptions.PreserveWhitespace);
            htmlDocument.TransformAll("format", element => element.Format());
            return htmlDocument;
        }

        public void ApplyChartTransform(List<Attachment> attachments, XDocument htmlDocument)
        {
            using (AdoDataConnection connection = ConnectionFactory())
            {
                htmlDocument.TransformAll("chart", (element, index) =>
                {
                    string chartEventID = (string)element.Attribute("eventID") ?? "-1";
                    string cid = $"event{chartEventID}_chart{index:00}.png";

                    Stream image = ChartGenerator.ConvertToChartImageStream(connection, element);
                    Attachment attachment = new Attachment(image, cid);
                    attachment.ContentId = attachment.Name;
                    attachments.Add(attachment);

                    return new XElement("img", new XAttribute("src", $"cid:{cid}"));
                });
            }
        }

        public void LoadSentEmail(DateTime now, List<string> recipients, XDocument htmlDocument, List<int> eventIDs)
        {
            int sentEmailID = EmailService.LoadSentEmail(now, recipients, htmlDocument);

            using (AdoDataConnection connection = CreateDbConnection())
            {
                TableOperations<EventSentEmail> eventSentEmailTable = new TableOperations<EventSentEmail>(connection);

                foreach (int eventID in eventIDs)
                {
                    if (eventSentEmailTable.QueryRecordCountWhere("EventID = {0} AND SentEmailID = {1}", eventID, sentEmailID) > 0)
                        continue;

                    EventSentEmail eventSentEmail = new EventSentEmail();
                    eventSentEmail.EventID = eventID;
                    eventSentEmail.SentEmailID = sentEmailID;
                    eventSentEmailTable.AddNewRecord(eventSentEmail);
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

        private void ApplyFTTTransform(List<Attachment> attachments, XDocument htmlDocument)
        {
            htmlDocument.TransformAll("ftt", (element, index) =>
            {
                string fttEventID = (string)element.Attribute("eventID") ?? "-1";
                string cid = $"event{fttEventID}_ftt{index:00}.jpg";

                try
                {
                    Stream image = FTTImageGenerator.ConvertToFTTImageStream(element);
                    Attachment attachment = new Attachment(image, cid);
                    attachment.ContentId = attachment.Name;
                    attachments.Add(attachment);

                    return new XElement("img", new XAttribute("src", $"cid:{cid}"));
                }
                catch (Exception ex)
                {
                    string text = new StringBuilder()
                        .AppendLine($"Error while querying {cid}:")
                        .Append(ex.ToString())
                        .ToString();

                    object[] content = text
                        .Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None)
                        .SelectMany(line => new object[] { new XElement("br"), new XText(line) })
                        .Skip(1)
                        .ToArray();

                    return new XElement("div", content);
                }
            });
        }


        #endregion
    }
}
