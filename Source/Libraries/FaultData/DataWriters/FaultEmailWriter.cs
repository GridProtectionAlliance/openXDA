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
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security;
using System.Threading;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Xsl;
using FaultData.Configuration;
using FaultData.Database;
using FaultData.Database.FaultLocationDataTableAdapters;
using FaultData.Database.MeterDataTableAdapters;
using FaultData.DataSets;
using GSF;
using GSF.Collections;
using GSF.Configuration;
using GSF.Xml;
using log4net;

namespace FaultData.DataWriters
{
    /// <summary>
    /// Writes the results of fault analysis to an email.
    /// </summary>
    public class FaultEmailWriter : IDataWriter
    {
        #region [ Members ]

        // Fields
        private string m_dbConnectionString;
        private double m_timeTolerance;
        private string m_xdaTimeZone;
        private string m_lengthUnits;
        private EmailSettings m_emailSettings;
        private FaultEmailSettings m_faultEmailSettings;
        private FaultLocationSettings m_faultLocationSettings;

        #endregion

        #region [ Constructors ]

        public FaultEmailWriter()
        {
            m_emailSettings = new EmailSettings();
            m_faultEmailSettings = new FaultEmailSettings();
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
        [SettingName("FaultEmail")]
        public FaultEmailSettings FaultEmailSettings
        {
            get
            {
                return m_faultEmailSettings;
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
            bool? faultDetectionResult;
            bool faultValidationResult;

            Initialize(this);

            foreach (var faultGroup in dbAdapterContainer.GetAdapter<FaultGroupTableAdapter>().GetDataByFileGroup(meterDataSet.FileGroup.ID))
            {
                faultDetectionResult = !faultGroup.IsFaultDetectionLogicResultNull()
                    ? Convert.ToBoolean(faultGroup.FaultDetectionLogicResult)
                    : (bool?)null;

                faultValidationResult = Convert.ToBoolean(faultGroup.FaultValidationLogicResult);

                if (faultDetectionResult == true || (m_faultLocationSettings.UseDefaultFaultDetectionLogic && faultValidationResult))
                {
                    if (dbAdapterContainer.GetAdapter<EventFaultEmailTableAdapter>().GetFaultEmailCount(faultGroup.EventID) == 0)
                        QueueEventID(faultGroup.EventID);
                }
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

        private static readonly ILog Log = LogManager.GetLogger(typeof(FaultEmailWriter));

        // Static Constructor
        static FaultEmailWriter()
        {
            QueuedEventIDs = new HashSet<int>();
            ProcessQueue = ProcessQueue<Action>.CreateRealTimeQueue(action => action());
            ProcessQueue.ProcessException += (sender, args) => Log.Error(args.Argument.Message, args.Argument);
            ProcessQueue.Start();
        }

        // Static Methods
        private static void Initialize(FaultEmailWriter writer)
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

            if ((object)s_dbAdapterContainer == null)
            {
                ProcessQueue.Add(() =>
                {
                    if ((object)s_dbAdapterContainer == null)
                        s_dbAdapterContainer = new DbAdapterContainer(writer.DbConnectionString);
                });
            }
        }

        private static void QueueEventID(int eventID)
        {
            if (string.IsNullOrEmpty(s_smtpServer))
                return;

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
                MeterData.EventRow eventRow;
                MeterData.EventDataTable systemEvent;

                QueuedEventIDs.Remove(eventID);

                eventRow = s_dbAdapterContainer.GetAdapter<EventTableAdapter>().GetDataByID(eventID)[0];
                systemEvent = s_dbAdapterContainer.GetAdapter<EventTableAdapter>().GetSystemEvent(eventRow.StartTime, eventRow.EndTime, s_timeTolerance);

                if (systemEvent.Any(evt => evt.LineID == eventRow.LineID && QueuedEventIDs.Contains(evt.ID)))
                    return;

                if (s_dbAdapterContainer.GetAdapter<EventFaultEmailTableAdapter>().GetFaultEmailCount(eventID) == 0)
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

            int faultTypeID;
            string eventDetail;
            MeterData.EventRow eventRow;
            List<int> faultedMeters;
            List<int> meterGroups;
            List<Recipient> recipients;

            XslCompiledTransform transform;
            XDocument htmlDocument;

            List<Attachment> attachments;
            string subject;
            string html;

            systemInfo = s_dbAdapterContainer.GetAdapter<SystemInfoDataContext>();
            meterInfo = s_dbAdapterContainer.GetAdapter<MeterInfoDataContext>();
            faultInfo = s_dbAdapterContainer.GetAdapter<FaultLocationInfoDataContext>();
            eventAdapter = s_dbAdapterContainer.GetAdapter<EventTableAdapter>();
            eventTypeAdapter = s_dbAdapterContainer.GetAdapter<EventTypeTableAdapter>();

            faultTypeID = eventTypeAdapter.GetData()
                .Where(eventType => eventType.Name == "Fault")
                .Select(eventType => eventType.ID)
                .FirstOrDefault();

            eventDetail = eventAdapter.GetEventDetail(eventID);
            eventRow = eventAdapter.GetDataByID(eventID)[0];

            faultedMeters = eventAdapter.GetSystemEvent(eventRow.StartTime, eventRow.EndTime, s_timeTolerance)
                .Where(evt => evt.LineID == eventRow.LineID)
                .Where(evt => evt.EventTypeID == faultTypeID)
                .Select(evt => evt.MeterID)
                .ToList();

            meterGroups = meterInfo.GroupMeters
                .Where(groupMeter => faultedMeters.Contains(groupMeter.MeterID))
                .Select(groupMeter => groupMeter.GroupID)
                .ToList();

            foreach (FaultEmailTemplate template in faultInfo.FaultEmailTemplates.ToList())
            {
                recipients = template.GetRecipients(systemInfo.Recipients, meterGroups);

                if (recipients.Count == 0)
                    continue;

                using (StringReader templateReader = new StringReader(template.Template))
                using (StringReader dataReader = new StringReader(eventDetail))
                using (XmlReader xmlTemplateReader = XmlReader.Create(templateReader))
                using (XmlReader xmlDataReader = XmlReader.Create(dataReader))
                using (StringWriter transformWriter = new StringWriter())
                {
                    transform = new XslCompiledTransform();
                    transform.Load(xmlTemplateReader);
                    transform.Transform(xmlDataReader, null, transformWriter);
                    htmlDocument = XDocument.Parse(transformWriter.ToString(), LoadOptions.PreserveWhitespace);
                }

                htmlDocument.TransformAll("format", element => element.Format());
                attachments = new List<Attachment>();

                try
                {
                    htmlDocument.TransformAll("chart", (element, index) =>
                    {
                        string cid = $"chart{index:00}.png";

                        Stream image = ChartGenerator.ConvertToChartImageStream(s_dbAdapterContainer, element);
                        Attachment attachment = new Attachment(image, cid);
                        attachment.ContentId = attachment.Name;
                        attachments.Add(attachment);

                        return new XElement("img", new XAttribute("src", $"cid:{cid}"));
                    });

                    subject = (string)htmlDocument.Descendants("title").FirstOrDefault() ?? "Fault detected by openXDA";
                    html = htmlDocument.ToString(SaveOptions.DisableFormatting).Replace("&amp;", "&");
                    SendEmail(recipients, subject, html, attachments);
                    LoadEmail(eventID, recipients, subject, html);
                }
                finally
                {
                    foreach (Attachment attachment in attachments)
                        attachment.Dispose();
                }
            }
        }

        private static void SendEmail(List<Recipient> recipients, string subject, string body, IEnumerable<Attachment> attachments)
        {
            const int DefaultSMTPPort = 25;

            string[] smtpServerParts = s_smtpServer.Split(':');
            string host = smtpServerParts[0];
            int port;

            if (smtpServerParts.Length <= 1 || !int.TryParse(smtpServerParts[1], out port))
                port = DefaultSMTPPort;

            using (SmtpClient smtpClient = new SmtpClient(host, port))
            using (MailMessage emailMessage = new MailMessage())
            {
                if ((object)s_username != null && (object)s_password != null)
                    smtpClient.Credentials = new NetworkCredential(s_username, s_password);

                smtpClient.EnableSsl = s_enableSSL;

                emailMessage.From = new MailAddress(s_fromAddress);
                emailMessage.Subject = subject;
                emailMessage.Body = body;
                emailMessage.IsBodyHtml = true;

                // Add the specified To recipients for the email message
                foreach (string toRecipient in recipients.Select(recipient => recipient.Email))
                    emailMessage.To.Add(toRecipient.Trim());

                // Create the image attachment for the email message
                foreach (Attachment attachment in attachments)
                    emailMessage.Attachments.Add(attachment);

                // Send the email
                smtpClient.Send(emailMessage);
            }
        }

        private static void LoadEmail(int eventID, List<Recipient> recipients, string subject, string body)
        {
            EventTableAdapter eventAdapter = s_dbAdapterContainer.GetAdapter<EventTableAdapter>();
            MeterData.EventRow eventRow = eventAdapter.GetDataByID(eventID)[0];
            MeterData.EventDataTable systemEvent = eventAdapter.GetSystemEvent(eventRow.StartTime, eventRow.EndTime, s_timeTolerance);

            FaultEmailTableAdapter faultEmailAdapter = s_dbAdapterContainer.GetAdapter<FaultEmailTableAdapter>();
            FaultLocationData.FaultEmailDataTable faultEmailTable = new FaultLocationData.FaultEmailDataTable();
            FaultLocationData.EventFaultEmailDataTable eventFaultEmailTable = new FaultLocationData.EventFaultEmailDataTable();

            DateTime now = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, s_timeZone);
            string toLine = string.Join("; ", recipients.Select(recipient => recipient.Email));

            BulkLoader bulkLoader;

            faultEmailTable.AddFaultEmailRow(now, toLine, subject, body);
            faultEmailAdapter.Update(faultEmailTable);

            foreach (MeterData.EventRow evt in systemEvent)
            {
                if (eventRow.LineID == evt.LineID)
                    eventFaultEmailTable.AddEventFaultEmailRow(evt.ID, faultEmailTable[0].ID);
            }

            bulkLoader = new BulkLoader();
            bulkLoader.Connection = s_dbAdapterContainer.Connection;
            bulkLoader.CommandTimeout = s_dbAdapterContainer.CommandTimeout;
            bulkLoader.Load(eventFaultEmailTable);
        }

        #endregion
    }
}
