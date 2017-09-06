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
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security;
using System.Threading;
using System.Xml.Linq;
using FaultData.Configuration;
using FaultData.Database;
using FaultData.Database.MeterDataTableAdapters;
using FaultData.DataSets;
using GSF;
using GSF.Collections;
using GSF.Configuration;
using GSF.Data;
using GSF.Xml;
using log4net;
using Supremes;
using static FaultData.Database.MeterData;

namespace FaultData.DataWriters
{
    /// <summary>
    /// Writes the results of fault analysis to an email.
    /// </summary>
    public class EventEmailWriter : IDataWriter
    {
        #region [ Members ]

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

        public void WriteResults(DbAdapterContainer dbAdapterContainer, MeterDataSet meterDataSet)
        {
            Initialize(this);

            foreach (EventRow evt in dbAdapterContainer.GetAdapter<EventTableAdapter>().GetDataByFileGroup(meterDataSet.FileGroup.ID))
            {
                if (GetEmailCount(dbAdapterContainer, evt.ID) == 0)
                    QueueEventID(evt.ID);
            }
        }

        #endregion

        #region [ Static ]

        // Static Fields
        private static readonly HashSet<int> QueuedEventIDs;
        private static readonly ProcessQueue<Action> ProcessQueue;

        private static string s_smtpServer;
        private static string s_fromAddress;
        private static string s_username;
        private static SecureString s_password;
        private static bool s_enableSSL;
        private static double s_timeTolerance;
        private static TimeSpan s_waitPeriod;
        private static TimeZoneInfo s_timeZone;
        private static DbAdapterContainer s_dbAdapterContainer;

        private static readonly ILog Log = LogManager.GetLogger(typeof(EventEmailWriter));

        // Static Constructor
        static EventEmailWriter()
        {
            QueuedEventIDs = new HashSet<int>();
            ProcessQueue = ProcessQueue<Action>.CreateRealTimeQueue(action => action());
            ProcessQueue.ProcessException += (sender, args) => Log.Error(args.Argument.Message, args.Argument);
            ProcessQueue.Start();
        }

        // Static Methods
        private static void Initialize(EventEmailWriter writer)
        {
            bool configurationChanged =
                s_timeTolerance != writer.TimeTolerance ||
                s_smtpServer != writer.EmailSettings.SMTPServer ||
                s_fromAddress != writer.EmailSettings.FromAddress ||
                s_username != writer.EmailSettings.Username ||
                s_password != writer.EmailSettings.SecurePassword ||
                s_enableSSL != writer.EmailSettings.EnableSSL ||
                s_waitPeriod != TimeSpan.FromSeconds(writer.FaultEmailSettings.WaitPeriod) ||
                s_timeZone.Id != writer.XDATimeZone;


            if (configurationChanged)
            {
                ProcessQueue.Add(() =>
                {
                    s_timeTolerance = writer.TimeTolerance;
                    s_smtpServer = writer.EmailSettings.SMTPServer;
                    s_fromAddress = writer.EmailSettings.FromAddress;
                    s_username = writer.EmailSettings.Username;
                    s_password = writer.EmailSettings.SecurePassword;
                    s_enableSSL = writer.EmailSettings.EnableSSL;
                    s_waitPeriod = TimeSpan.FromSeconds(writer.FaultEmailSettings.WaitPeriod);
                    s_timeZone = TimeZoneInfo.FindSystemTimeZoneById(writer.XDATimeZone);
                });
            }

            if ((object)s_dbAdapterContainer == null || !s_dbAdapterContainer.Connection.State.HasFlag(ConnectionState.Open))
            {
                ProcessQueue.Add(() =>
                {
                    if ((object)s_dbAdapterContainer != null && s_dbAdapterContainer.Connection.State.HasFlag(ConnectionState.Open))
                        return;

                    using (s_dbAdapterContainer)
                    {
                        s_dbAdapterContainer = new DbAdapterContainer(writer.DbConnectionString);
                    }
                });
            }
        }

        private static void QueueEventID(int eventID)
        {
            ProcessQueue.Add(() =>
            {
                EventArgs<RegisteredWaitHandle> args;
                ManualResetEvent waitHandle;
                WaitOrTimerCallback callback;

                args = new EventArgs<RegisteredWaitHandle>(null);
                waitHandle = new ManualResetEvent(false);

                callback = (state, timedOut) =>
                {
                    DequeueEventID(eventID);

                    if ((object)args.Argument != null)
                        args.Argument.Unregister(null);

                    waitHandle.Dispose();
                };

                QueuedEventIDs.Add(eventID);

                args.Argument = ThreadPool.RegisterWaitForSingleObject(waitHandle, callback, null, s_waitPeriod, true);
            });
        }

        private static void DequeueEventID(int eventID)
        {
            ProcessQueue.Add(() =>
            {
                EventRow eventRow;
                EventDataTable systemEvent;

                QueuedEventIDs.Remove(eventID);

                eventRow = s_dbAdapterContainer.GetAdapter<EventTableAdapter>().GetDataByID(eventID)[0];
                systemEvent = s_dbAdapterContainer.GetAdapter<EventTableAdapter>().GetSystemEvent(eventRow.StartTime, eventRow.EndTime, s_timeTolerance);

                if (systemEvent.Any(evt => evt.LineID == eventRow.LineID && QueuedEventIDs.Contains(evt.ID)))
                    return;

                if (GetEmailCount(s_dbAdapterContainer, eventID) == 0)
                    GenerateEmail(eventID);
            });
        }

        private static void GenerateEmail(int eventID)
        {
            SystemInfoDataContext systemInfo;
            MeterInfoDataContext meterInfo;
            FaultLocationInfoDataContext faultInfo;
            EventTableAdapter eventAdapter;
            EventTypeTableAdapter eventTypeAdapter;

            EventRow eventRow;
            EventDataTable systemEvent;

            int faultTypeID;
            string eventDetail;
            XDocument htmlDocument;

            List<Attachment> attachments;
            string subject;
            string html;
            bool alreadySent;

            systemInfo = s_dbAdapterContainer.GetAdapter<SystemInfoDataContext>();
            meterInfo = s_dbAdapterContainer.GetAdapter<MeterInfoDataContext>();
            faultInfo = s_dbAdapterContainer.GetAdapter<FaultLocationInfoDataContext>();
            eventAdapter = s_dbAdapterContainer.GetAdapter<EventTableAdapter>();
            eventTypeAdapter = s_dbAdapterContainer.GetAdapter<EventTypeTableAdapter>();

            faultTypeID = eventTypeAdapter.GetData()
                .Where(eventType => eventType.Name == "Fault")
                .Select(eventType => eventType.ID)
                .FirstOrDefault();

            // Load the system event before the eventDetail record to avoid race conditions causing missed emails
            eventRow = eventAdapter.GetDataByID(eventID)[0];
            systemEvent = eventAdapter.GetSystemEvent(eventRow.StartTime, eventRow.EndTime, s_timeTolerance);
            eventDetail = eventAdapter.GetEventDetail(eventID);

            List<IGrouping<int, Guid>> templateGroups;

            using (SqlCommand command = new SqlCommand("GetEventEmailRecipients", s_dbAdapterContainer.Connection))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                DataTable recipientTable = new DataTable();
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@eventID", eventID);
                adapter.Fill(recipientTable);

                templateGroups = recipientTable
                    .Select()
                    .GroupBy(row => row.ConvertField<int>("TemplateID"), row => row.ConvertField<Guid>("UserAccountID"))
                    .ToList();
            }

            foreach (IGrouping<int, Guid> templateGroup in templateGroups)
            {
                string template;
                List<string> recipients;

                using (AdoDataConnection connection = new AdoDataConnection(s_dbAdapterContainer.Connection, typeof(SqlDataAdapter), false))
                {
                    template = connection.ExecuteScalar<string>("SELECT Template FROM XSLTemplate WHERE ID = {0}", templateGroup.Key);

                    string paramString = string.Join(",", templateGroup.Select((userAccountID, index) => $"{{{index}}}"));
                    string sql = $"SELECT Email FROM UserAccount WHERE Email IS NOT NULL AND Email <> '' AND ID IN ({paramString})";
                    DataTable emailTable = connection.RetrieveData(sql, templateGroup.Cast<object>().ToArray());
                    recipients = emailTable.Select().Select(row => row.ConvertField<string>("Email")).ToList();
                }

                htmlDocument = XDocument.Parse(eventDetail.ApplyXSLTransform(template), LoadOptions.PreserveWhitespace);
                htmlDocument.TransformAll("format", element => element.Format());
                htmlDocument.TransformAll("structure", element =>
                {
                    string structureString = "";
                    string lat = "0";
                    string lng = "0";
                    try
                    {
                        var doc = Dcsoup.Parse(new Uri(element.Attribute("url").Value + $"?id={element.Value}"), 5000);
                        structureString = doc.Select("span[id=strno]").Text;
                        lat = structureString.Split('(', ',', ')')[1];
                        lng = structureString.Split('(', ',', ')')[2];

                    }
                    catch (Exception ex)
                    {
                        structureString = "Structure and location unavailable...";
                        return new XElement("span", structureString);
                    }
                    return new XElement(new XElement("a", new XAttribute("href", $"http://www.google.com/maps/place/{lat},{lng}"), new XElement("span", structureString)));

                });

                attachments = new List<Attachment>();

                try
                {
                    htmlDocument.TransformAll("chart", (element, index) =>
                    {
                        string cid = $"event{eventID}_chart{index:00}.png";

                        Stream image = ChartGenerator.ConvertToChartImageStream(s_dbAdapterContainer, element);
                        Attachment attachment = new Attachment(image, cid);
                        attachment.ContentId = attachment.Name;
                        attachments.Add(attachment);

                        return new XElement("img", new XAttribute("src", $"cid:{cid}"));
                    });

                    htmlDocument.TransformAll("equipmentAndCustomersAffected", (element, index) =>
                    {
                        return PQIEquipmentAffectedGenerator.GetEquipmentAffected(s_dbAdapterContainer, element);
                    });

                    subject = (string)htmlDocument.Descendants("title").FirstOrDefault() ?? "Fault detected by openXDA";
                    html = htmlDocument.ToString(SaveOptions.DisableFormatting).Replace("&amp;", "&");
                    alreadySent = false;

                    try
                    {
                        int sentEmailID;

                        using (AdoDataConnection connection = new AdoDataConnection(s_dbAdapterContainer.Connection, typeof(SqlDataAdapter), false))
                        {
                            string systemEventIDs = string.Join(",", systemEvent.Where(row => row.LineID == eventRow.LineID).Select(row => row.ID));

                            string query =
                                $"SELECT SentEmail.ID " +
                                $"FROM " +
                                $"    SentEmail JOIN " +
                                $"    EventSentEmail ON EventSentEmail.SentEmailID = SentEmail.ID " +
                                $"WHERE " +
                                $"    EventSentEmail.EventID IN ({systemEventIDs}) AND " +
                                $"    SentEmail.Message = {{0}}";

                            sentEmailID = connection.ExecuteScalar(-1, DataExtensions.DefaultTimeoutDuration, query, html);
                        }

                        alreadySent = (sentEmailID != -1);

                        if (!alreadySent)
                            sentEmailID = LoadSentEmail(recipients, subject, html);

                        LoadEventSentEmail(eventRow, systemEvent, sentEmailID);
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

        private static int LoadSentEmail(List<string> recipients, string subject, string body)
        {
            DateTime now = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, s_timeZone);
            string toLine = string.Join("; ", recipients.Select(recipient => recipient.Trim()));

            using (AdoDataConnection connection = new AdoDataConnection(s_dbAdapterContainer.Connection, typeof(SqlDataAdapter), false))
            {
                connection.ExecuteNonQuery("INSERT INTO SentEmail VALUES({0}, {1}, {2}, {3})", now, toLine, subject, body);
                return connection.ExecuteScalar<int>("SELECT @@IDENTITY");
            }
        }

        private static void LoadEventSentEmail(EventRow eventRow, EventDataTable systemEvent, int sentEmailID)
        {
            BulkLoader bulkLoader;
            DataTable eventSentEmailTable;

            using (AdoDataConnection connection = new AdoDataConnection(s_dbAdapterContainer.Connection, typeof(SqlDataAdapter), false))
            {
                // Query an empty table with matching schema --
                // union table to itself to eliminate unique key constraints
                eventSentEmailTable = connection.RetrieveData("SELECT * FROM EventSentEmail WHERE 1 IS NULL UNION ALL SELECT * FROM EventSentEmail WHERE 1 IS NULL");
                eventSentEmailTable.TableName = "EventSentEmail";
            }

            foreach (MeterData.EventRow evt in systemEvent)
            {
                if (eventRow.LineID == evt.LineID)
                    eventSentEmailTable.Rows.Add(0, evt.ID, sentEmailID);
            }

            bulkLoader = new BulkLoader();
            bulkLoader.Connection = s_dbAdapterContainer.Connection;
            bulkLoader.CommandTimeout = s_dbAdapterContainer.CommandTimeout;
            bulkLoader.Load(eventSentEmailTable);
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

        private static int GetEmailCount(DbAdapterContainer dbAdapterContainer, int eventID)
        {
            using (AdoDataConnection connection = new AdoDataConnection(dbAdapterContainer.Connection, typeof(SqlDataAdapter), false))
            {
                int timeout = dbAdapterContainer.CommandTimeout;
                string sql = "SELECT COUNT(*) FROM EventSentEmail WHERE EventID = {0}";
                object[] parameters = { eventID };
                return connection.ExecuteScalar<int>(timeout: timeout, sqlFormat: sql, parameters: parameters);
            }
        }

        #endregion
    }
}
