//******************************************************************************************************
//  EmailWriter.cs - Gbtc
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
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Xsl;
using FaultData.Database;
using FaultData.Database.FaultLocationDataTableAdapters;
using FaultData.Database.MeterDataTableAdapters;
using FaultData.DataSets;
using GSF.Collections;

namespace FaultData.DataWriters
{
    /// <summary>
    /// Writes the results of fault analysis to an email.
    /// </summary>
    public class EmailWriter : IDataWriter
    {
        #region [ Members ]

        // Fields
        private string m_dbConnectionString;
        private string m_smtpServer;
        private string m_fromAddress;
        private string m_emailTemplate;
        private double m_timeTolerance;
        private double m_waitPeriod;
        private string m_xdaTimeZone;

        #endregion

        #region [ Properties ]

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

        public string SMTPServer
        {
            get
            {
                return m_smtpServer;
            }
            set
            {
                m_smtpServer = value;
            }
        }

        public string FromAddress
        {
            get
            {
                return m_fromAddress;
            }
            set
            {
                m_fromAddress = value;
            }
        }

        public string EmailTemplate
        {
            get
            {
                return m_emailTemplate;
            }
            set
            {
                m_emailTemplate = value;
            }
        }

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

        public double WaitPeriod
        {
            get
            {
                return m_waitPeriod;
            }
            set
            {
                m_waitPeriod = value;
            }
        }

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

        #endregion

        #region [ Methods ]

        public void WriteResults(DbAdapterContainer dbAdapterContainer, MeterDataSet meterDataSet)
        {
            Initialize(this);

            foreach (FaultLocationData.FaultSummaryRow faultSummary in dbAdapterContainer.GetAdapter<FaultSummaryTableAdapter>().GetDataByFileGroup(meterDataSet.FileGroup.ID))
            {
                if (faultSummary.IsSelectedAlgorithm != 0 && faultSummary.IsSuppressed == 0)
                {
                    if (dbAdapterContainer.GetAdapter<EventFaultEmailTableAdapter>().GetFaultEmailCount(faultSummary.EventID) == 0)
                        QueueEventID(faultSummary.EventID);
                }
            }
        }

        #endregion

        #region [ Static ]

        // Static Fields
        private static readonly HashSet<int> QueuedEventIDs;
        private static readonly ProcessQueue<Action> ProcessQueue;

        private static bool s_initialized;
        private static string s_smtpServer;
        private static string s_fromAddress;
        private static string s_emailTemplate;
        private static double s_timeTolerance;
        private static TimeSpan s_waitPeriod;
        private static TimeZoneInfo s_timeZone;
        private static DbAdapterContainer s_dbAdapterContainer;

        // Static Constructor
        static EmailWriter()
        {
            QueuedEventIDs = new HashSet<int>();
            ProcessQueue = ProcessQueue<Action>.CreateRealTimeQueue(action => action());
            ProcessQueue.Start();
        }

        // Static Methods
        private static void Initialize(EmailWriter writer)
        {
            if (s_initialized)
                return;

            ProcessQueue.Add(() =>
            {
                if (!s_initialized)
                {
                    s_timeTolerance = writer.TimeTolerance;
                    s_smtpServer = writer.SMTPServer;
                    s_fromAddress = writer.FromAddress;
                    s_emailTemplate = writer.EmailTemplate;
                    s_waitPeriod = TimeSpan.FromSeconds(writer.WaitPeriod);
                    s_timeZone = TimeZoneInfo.FindSystemTimeZoneById(writer.XDATimeZone);
                    s_dbAdapterContainer = new DbAdapterContainer(writer.DbConnectionString);
                    s_initialized = true;
                }
            });
        }

        private static void QueueEventID(int eventID)
        {
            ProcessQueue.Add(() =>
            {
                ManualResetEvent waitHandle;
                WaitOrTimerCallback callback;

                waitHandle = new ManualResetEvent(false);

                callback = (state, timedOut) =>
                {
                    waitHandle.Dispose();
                    DequeueEventID(eventID);
                };

                QueuedEventIDs.Add(eventID);

                ThreadPool.RegisterWaitForSingleObject(waitHandle, callback, null, s_waitPeriod, true);
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
            string eventDetail;

            XslCompiledTransform transform;
            XDocument htmlDocument;
            List<XElement> formatParents;

            List<Recipient> recipients;
            string subject;
            string html;

            eventDetail = s_dbAdapterContainer.GetAdapter<EventTableAdapter>().GetEventDetail(eventID);

            using (StringReader templateReader = new StringReader(s_emailTemplate))
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

            formatParents = htmlDocument
                .Descendants("format")
                .Select(element => element.Parent)
                .Distinct()
                .ToList();

            foreach (XElement parent in formatParents)
                parent.ReplaceNodes(parent.Nodes().Select(Format));

            recipients = s_dbAdapterContainer.GetAdapter<SystemInfoDataContext>().Recipients.ToList();
            subject = (string)htmlDocument.Descendants("title").FirstOrDefault() ?? "Fault detected by openXDA";
            html = htmlDocument.ToString(SaveOptions.DisableFormatting).Replace("&amp;", "&");

            SendEmail(recipients, subject, html);
            LoadEmail(eventID, recipients, subject, html);
        }

        private static void SendEmail(List<Recipient> recipients, string subject, string body, params Attachment[] attachments)
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

        private static object Format(XNode node)
        {
            XElement element;
            IFormattable formattable;

            Type elementType;
            string formatString;
            string value;

            element = node as XElement;

            if ((object)element != null)
            {
                if (element.Name.ToString().Equals("format", StringComparison.OrdinalIgnoreCase))
                {
                    elementType = Type.GetType((string)element.Attribute("type"), false);
                    formatString = (string)element.Attribute("spec");
                    value = (string)element;
                    formattable = (IFormattable)Convert.ChangeType(value, elementType);
                    return new XText(formattable.ToString(formatString, null));
                }
            }

            return node;
        }

        #endregion
    }
}
