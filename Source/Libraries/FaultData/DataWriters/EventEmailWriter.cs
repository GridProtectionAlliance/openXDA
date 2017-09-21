//******************************************************************************************************
//  FaultEmailWriter.cs - Gbtc
//
//  Copyright © 2014, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the Eclipse Public License -v 1.0 (the "License"); you may
//  not use this file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://www.opensource.org/licenses/eclipse-1.0.php
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  11/14/2014 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security;
using System.Threading;
using System.Xml.Linq;
using FaultData.Configuration;
using FaultData.DataOperations;
using FaultData.DataSets;
using GSF;
using GSF.Collections;
using GSF.Configuration;
using GSF.Data;
using GSF.Data.Model;
using GSF.Threading;
using GSF.Xml;
using log4net;
using openXDA.Model;
using Supremes;

namespace FaultData.DataWriters
{
    /// <summary>
    /// Writes the results of fault analysis to an email.
    /// </summary>
    public class EventEmailWriter : DataOperationBase<MeterDataSet>
    {
        #region [ Members ]

        // Nested Types
        private class LineEvent
        {
            public int LineID { get; }
            public Range<DateTime> TimeRange { get; }
            public DateTime TimeCreated { get; }
            public DateTime TimeUpdated { get; }

            public LineEvent(Event evt)
                : this(evt.LineID, new Range<DateTime>(evt.StartTime, evt.EndTime))
            {
            }

            public LineEvent(int lineID, Range<DateTime> timeRange)
                : this(lineID, timeRange, DateTime.UtcNow)
            {
            }

            private LineEvent(int lineID, Range<DateTime> timeRange, DateTime timeCreated)
                : this(lineID, timeRange, timeCreated, timeCreated)
            {
            }

            private LineEvent(int lineID, Range<DateTime> timeRange, DateTime timeCreated, DateTime timeUpdated)
            {
                LineID = lineID;
                TimeRange = timeRange;
                TimeCreated = timeCreated;
                TimeUpdated = timeUpdated;
            }

            public bool Overlaps(LineEvent other)
            {
                if (LineID != other.LineID)
                    return false;

                return TimeRange.Overlaps(other.TimeRange);
            }

            public LineEvent Merge(LineEvent other)
            {
                if (LineID != other.LineID)
                    throw new ArgumentException("Unable to merge line events on separate lines.", nameof(other));

                Range<DateTime> mergedTimeRange = TimeRange.Merge(other.TimeRange);
                DateTime timeCreated = Common.Min(TimeCreated, other.TimeCreated);
                DateTime timeUpdated = Common.Max(TimeUpdated, other.TimeUpdated);
                return new LineEvent(LineID, mergedTimeRange, timeCreated, timeUpdated);
            }
        }

        // Fields
        private string m_dbConnectionString;
        private double m_timeTolerance;
        private string m_xdaTimeZone;
        private string m_lengthUnits;
        private EmailSettings m_emailSettings;
        private EventEmailSettings m_eventEmailSettings;
        private FaultLocationSettings m_faultLocationSettings;

        #endregion

        #region [ Constructors ]

        public EventEmailWriter()
        {
            m_emailSettings = new EmailSettings();
            m_eventEmailSettings = new EventEmailSettings();
            m_faultLocationSettings = new FaultLocationSettings();
        }

        #endregion

        #region [ Properties ]

        [Setting]
        public string DbConnectionString
        {
            get
            {
                return m_dbConnectionString;
            }
            set
            {
                m_dbConnectionString = value;
            }
        }

        [Setting]
        public double TimeTolerance
        {
            get
            {
                return m_timeTolerance;
            }
            set
            {
                m_timeTolerance = value;
            }
        }

        [Setting]
        public string XDATimeZone
        {
            get
            {
                return m_xdaTimeZone;
            }
            set
            {
                m_xdaTimeZone = value;
            }
        }

        [Setting]
        public string LengthUnits
        {
            get
            {
                return m_lengthUnits;
            }
            set
            {
                m_lengthUnits = value;
            }
        }

        [Category]
        [SettingName("Email")]
        public EmailSettings EmailSettings
        {
            get
            {
                return m_emailSettings;
            }
        }

        [Category]
        [SettingName("EventEmail")]
        public EventEmailSettings FaultEmailSettings
        {
            get
            {
                return m_eventEmailSettings;
            }
        }

        [Category]
        [SettingName("FaultLocation")]
        public FaultLocationSettings FaultLocationSettings
        {
            get
            {
                return m_faultLocationSettings;
            }
        }

        #endregion

        #region [ Methods ]

        public override void Execute(MeterDataSet meterDataSet)
        {
            Initialize(this, meterDataSet);

            using (AdoDataConnection connection = meterDataSet.CreateDbConnection())
            {
                TableOperations<Event> eventTable = new TableOperations<Event>(connection);

                List<Event> events = eventTable
                    .QueryRecordsWhere("FileGroupID = {0}", meterDataSet.FileGroup.ID)
                    .ToList();

                foreach (Event evt in events)
                    QueueLineEvent(new LineEvent(evt));
            }
        }

        #endregion

        #region [ Static ]

        // Static Fields
        private static readonly HashSet<LineEvent> QueuedLineEvents;
        private static readonly LogicalThread EmailProcessingThread;

        private static string s_dbConnectionString;
        private static string s_smtpServer;
        private static string s_fromAddress;
        private static string s_username;
        private static SecureString s_password;
        private static bool s_enableSSL;
        private static double s_timeTolerance;
        private static TimeSpan s_minWaitPeriod;
        private static TimeSpan s_maxWaitPeriod;
        private static TimeZoneInfo s_timeZone;
        private static Func<AdoDataConnection> s_connectionFactory;

        private static readonly ILog Log = LogManager.GetLogger(typeof(EventEmailWriter));

        // Static Constructor
        static EventEmailWriter()
        {
            QueuedLineEvents = new HashSet<LineEvent>();
            EmailProcessingThread = new LogicalThread();
            EmailProcessingThread.UnhandledException += (sender, args) => Log.Error(args.Argument.Message, args.Argument);
        }

        // Static Methods
        private static void Initialize(EventEmailWriter writer, MeterDataSet meterDataSet)
        {
            bool configurationChanged =
                s_dbConnectionString != writer.DbConnectionString ||
                s_timeTolerance != writer.TimeTolerance ||
                s_smtpServer != writer.EmailSettings.SMTPServer ||
                s_fromAddress != writer.EmailSettings.FromAddress ||
                s_username != writer.EmailSettings.Username ||
                s_password != writer.EmailSettings.SecurePassword ||
                s_enableSSL != writer.EmailSettings.EnableSSL ||
                s_minWaitPeriod != TimeSpan.FromSeconds(writer.FaultEmailSettings.MinWaitPeriod) ||
                s_maxWaitPeriod != TimeSpan.FromSeconds(writer.FaultEmailSettings.MaxWaitPeriod) ||
                s_timeZone.Id != writer.XDATimeZone;

            if (configurationChanged)
            {
                EmailProcessingThread.Push(() =>
                {
                    s_dbConnectionString = writer.DbConnectionString;
                    s_timeTolerance = writer.TimeTolerance;
                    s_smtpServer = writer.EmailSettings.SMTPServer;
                    s_fromAddress = writer.EmailSettings.FromAddress;
                    s_username = writer.EmailSettings.Username;
                    s_password = writer.EmailSettings.SecurePassword;
                    s_enableSSL = writer.EmailSettings.EnableSSL;
                    s_minWaitPeriod = TimeSpan.FromSeconds(writer.FaultEmailSettings.MinWaitPeriod);
                    s_maxWaitPeriod = TimeSpan.FromSeconds(writer.FaultEmailSettings.MaxWaitPeriod);
                    s_timeZone = TimeZoneInfo.FindSystemTimeZoneById(writer.XDATimeZone);
                    s_connectionFactory = meterDataSet.CreateDbConnection;
                });
            }
        }

        private static void QueueLineEvent(LineEvent lineEvent)
        {
            EmailProcessingThread.Push(() =>
            {
                TimeSpan delaySpan = Common.Min(s_minWaitPeriod, s_maxWaitPeriod);
                int delay = (int)Math.Ceiling(delaySpan.TotalMilliseconds);
                QueuedLineEvents.Add(lineEvent);
                new Action(() => DequeueLineEvent(lineEvent)).DelayAndExecute(delay);
            });
        }

        private static void DequeueLineEvent(LineEvent lineEvent)
        {
            EmailProcessingThread.Push(() =>
            {
                if (!QueuedLineEvents.Remove(lineEvent))
                    return;

                using (AdoDataConnection connection = s_connectionFactory())
                {
                    TableOperations<Event> eventTable = new TableOperations<Event>(connection);

                    LineEvent newLineEvent = eventTable
                            .GetSystemEvent(lineEvent.TimeRange.Start, lineEvent.TimeRange.End, s_timeTolerance)
                            .Where(evt => evt.LineID == lineEvent.LineID)
                            .Select(evt => new LineEvent(evt))
                            .Aggregate(lineEvent, (mergedLineEvent, queuedLineEvent) => mergedLineEvent.Merge(queuedLineEvent));

                    List<LineEvent> queuedLineEvents = QueuedLineEvents
                        .Where(queuedLineEvent => newLineEvent.Overlaps(queuedLineEvent))
                        .ToList();

                    QueuedLineEvents.ExceptWith(queuedLineEvents);
                    newLineEvent = queuedLineEvents.Aggregate(lineEvent, (mergedLineEvent, queuedLineEvent) => mergedLineEvent.Merge(queuedLineEvent));

                    DateTime now = DateTime.UtcNow;
                    DateTime maxDequeueTime = newLineEvent.TimeCreated + s_maxWaitPeriod;
                    DateTime minDequeueTime = newLineEvent.TimeUpdated + s_minWaitPeriod;
                    DateTime dequeueTime = Common.Min(maxDequeueTime, minDequeueTime);

                    if (now < dequeueTime)
                    {
                        TimeSpan delaySpan = dequeueTime - now;
                        int delay = (int)Math.Ceiling(delaySpan.TotalMilliseconds);
                        QueuedLineEvents.Add(newLineEvent);
                        new Action(() => DequeueLineEvent(newLineEvent)).DelayAndExecute(delay);
                        return;
                    }

                    GenerateEmail(connection, newLineEvent);
                }
            });
        }

        private static void GenerateEmail(AdoDataConnection connection, LineEvent lineEvent)
        {
            TableOperations<Event> eventTable = new TableOperations<Event>(connection);
            Event evt;

            using (IDbCommand command = connection.Connection.CreateCommand())
            {
                IDbDataParameter startTimeParameter = command.CreateParameter();
                startTimeParameter.DbType = DbType.DateTime2;
                startTimeParameter.Value = lineEvent.TimeRange.Start;

                IDbDataParameter endTimeParameter = command.CreateParameter();
                endTimeParameter.DbType = DbType.DateTime2;
                endTimeParameter.Value = lineEvent.TimeRange.End;

                RecordRestriction recordRestriction =
                    new RecordRestriction("LineID = {0}", lineEvent.LineID) &
                    new RecordRestriction("StartTime >= {0}", startTimeParameter) &
                    new RecordRestriction("EndTime <= {0}", endTimeParameter);

                evt = eventTable.QueryRecord(recordRestriction);
            }

            GenerateEmail(connection, evt.ID);
        }

        private static void GenerateEmail(AdoDataConnection connection, int eventID)
        {
            XDocument htmlDocument;

            List<Attachment> attachments;
            string subject;
            string html;
            bool alreadySent;

            TableOperations<EventType> eventTypeTable = new TableOperations<EventType>(connection);
            EventType faultEventType = eventTypeTable.QueryRecordWhere("Name = 'Fault'");

            TableOperations<Event> eventTable = new TableOperations<Event>(connection);
            string eventDetail = connection.ExecuteScalar<string>("SELECT EventDetail FROM EventDetail WHERE EventID = {0}", eventID);

            List<IGrouping<int, Guid>> templateGroups;

            using (IDbCommand command = connection.Connection.CreateCommand())
            {
                Func<string, object, IDbDataParameter> createParameter = (name, value) =>
                {
                    IDbDataParameter parameter = command.CreateParameter();
                    parameter.ParameterName = name;
                    parameter.Value = value;
                    return parameter;
                };
                
                command.CommandText = "GetEventEmailRecipients";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(createParameter("@eventID", eventID));

                IDataAdapter adapter = (IDataAdapter)Activator.CreateInstance(connection.AdapterType, command);

                using (adapter as IDisposable)
                {
                    DataSet dataSet = new DataSet();

                    adapter.Fill(dataSet);

                    templateGroups = dataSet.Tables[0]
                        .Select()
                        .GroupBy(row => row.ConvertField<int>("TemplateID"), row => row.ConvertField<Guid>("UserAccountID"))
                        .ToList();
                }
            }

            foreach (IGrouping<int, Guid> templateGroup in templateGroups)
            {
                string template = connection.ExecuteScalar<string>("SELECT Template FROM XSLTemplate WHERE ID = {0}", templateGroup.Key);
                string paramString = string.Join(",", templateGroup.Select((userAccountID, index) => $"{{{index}}}"));
                string sql = $"SELECT Email FROM UserAccount WHERE Email IS NOT NULL AND Email <> '' AND ID IN ({paramString})";
                DataTable emailTable = connection.RetrieveData(sql, templateGroup.Cast<object>().ToArray());
                List<string> recipients = emailTable.Select().Select(row => row.ConvertField<string>("Email")).ToList();

                htmlDocument = XDocument.Parse(eventDetail.ApplyXSLTransform(template), LoadOptions.PreserveWhitespace);
                htmlDocument.TransformAll("format", element => element.Format());

                attachments = new List<Attachment>();

                try
                {
                    htmlDocument.TransformAll("chart", (element, index) =>
                    {
                        string cid = $"event{eventID}_chart{index:00}.png";

                        Stream image = ChartGenerator.ConvertToChartImageStream(connection, element);
                        Attachment attachment = new Attachment(image, cid);
                        attachment.ContentId = attachment.Name;
                        attachments.Add(attachment);

                        return new XElement("img", new XAttribute("src", $"cid:{cid}"));
                    });

                    htmlDocument.TransformAll("equipmentAndCustomersAffected", (element, index) =>
                    {
                        return PQIGenerator.GetPqiInformation(connection, element);
                    });

                    htmlDocument.TransformAll("structure", (element, index) =>
                    {
                        return StructureLocationGenerator.GetStructureLocationInformation(connection, element);
                    });

                    htmlDocument.TransformAll("lightning", (element, index) =>
                    {
                        return LightningGenerator.GetLightningInfo(connection, element);
                    });

                    htmlDocument.TransformAll("treeProbability", (element, index) =>
                    {
                        return TreeProbabilityGenerator.GetTreeProbability(connection, element);
                    });

                    subject = (string)htmlDocument.Descendants("title").FirstOrDefault() ?? "Fault detected by openXDA";
                    html = htmlDocument.ToString(SaveOptions.DisableFormatting).Replace("&amp;", "&");
                    alreadySent = false;

                    try
                    {
                        Event dequeuedEvent = eventTable.QueryRecordWhere("ID = {0}", eventID);

                        List<Event> systemEvent = eventTable
                            .GetSystemEvent(dequeuedEvent.StartTime, dequeuedEvent.EndTime, s_timeTolerance)
                            .Where(evt => dequeuedEvent.LineID == evt.LineID)
                            .ToList();

                        string systemEventIDs = string.Join(",", systemEvent.Where(row => row.LineID == dequeuedEvent.LineID).Select(row => row.ID));

                        string query =
                            $"SELECT SentEmail.ID " +
                            $"FROM " +
                            $"    SentEmail JOIN " +
                            $"    EventSentEmail ON EventSentEmail.SentEmailID = SentEmail.ID " +
                            $"WHERE " +
                            $"    EventSentEmail.EventID IN ({systemEventIDs}) AND " +
                            $"    SentEmail.Message = {{0}}";

                        int sentEmailID = connection.ExecuteScalar(-1, DataExtensions.DefaultTimeoutDuration, query, html);

                        alreadySent = (sentEmailID != -1);

                        if (!alreadySent)
                            sentEmailID = LoadSentEmail(connection, recipients, subject, html);

                        LoadEventSentEmail(connection, systemEvent, sentEmailID);
                    }
                    catch (Exception ex)
                    {
                        // Failure to load the email into the database should
                        // not prevent us from attempting to send the email
                        Log.Error(ex.Message, ex);
                    }

                    if (!alreadySent)
                        SendEmail(recipients, subject, html, attachments);
                }
                finally
                {
                    foreach (Attachment attachment in attachments)
                        attachment.Dispose();
                }
            }

            if (templateGroups.Any())
                Log.Info($"All emails sent for event ID {eventID}.");
        }

        private static int LoadSentEmail(AdoDataConnection connection, List<string> recipients, string subject, string body)
        {
            DateTime now = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, s_timeZone);
            string toLine = string.Join("; ", recipients.Select(recipient => recipient.Trim()));
            connection.ExecuteNonQuery("INSERT INTO SentEmail VALUES({0}, {1}, {2}, {3})", now, toLine, subject, body);
            return connection.ExecuteScalar<int>("SELECT @@IDENTITY");
        }

        private static void LoadEventSentEmail(AdoDataConnection connection, List<Event> systemEvent, int sentEmailID)
        {
            TableOperations<EventSentEmail> eventSentEmailTable = new TableOperations<EventSentEmail>(connection);

            foreach (Event evt in systemEvent)
            {
                EventSentEmail eventSentEmail = new EventSentEmail()
                {
                    EventID = evt.ID,
                    SentEmailID = sentEmailID
                };

                eventSentEmailTable.AddNewRecord(eventSentEmail);
            }
        }

        private static void SendEmail(List<string> recipients, string subject, string body, IEnumerable<Attachment> attachments)
        {
            const int DefaultSMTPPort = 25;

            string[] smtpServerParts = s_smtpServer.Split(':');
            string host = smtpServerParts[0];
            int port;

            if (string.IsNullOrEmpty(s_smtpServer))
                return;

            if (smtpServerParts.Length <= 1 || !int.TryParse(smtpServerParts[1], out port))
                port = DefaultSMTPPort;

            using (SmtpClient smtpClient = new SmtpClient(host, port))
            using (MailMessage emailMessage = new MailMessage())
            {
                if (!string.IsNullOrEmpty(s_username) && (object)s_password != null)
                    smtpClient.Credentials = new NetworkCredential(s_username, s_password);

                smtpClient.EnableSsl = s_enableSSL;

                emailMessage.From = new MailAddress(s_fromAddress);
                emailMessage.Subject = subject;
                emailMessage.Body = body;
                emailMessage.IsBodyHtml = true;

                // Add the specified To recipients for the email message
                foreach (string toRecipient in recipients)
                    emailMessage.To.Add(toRecipient.Trim());

                // Create the image attachment for the email message
                foreach (Attachment attachment in attachments)
                    emailMessage.Attachments.Add(attachment);

                // Send the email
                smtpClient.Send(emailMessage);
            }
        }

        #endregion
    }
}
