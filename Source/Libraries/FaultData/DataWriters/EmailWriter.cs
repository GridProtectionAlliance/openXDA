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
using System.Data;
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

namespace FaultData.DataWriters
{
    /// <summary>
    /// Writes the results of fault analysis to an email.
    /// </summary>
    public class EmailWriter : IDataWriter
    {
        #region [ Members ]

        // Fields
        private DbAdapterContainer m_dbAdapterContainer;

        private string m_smtpServer;
        private string m_fromAddress;
        private string m_emailTemplate;
        private double m_timeTolerance;
        private TimeZoneInfo m_timeZone;

        #endregion

        #region [ Properties ]

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

        public string XDATimeZone
        {
            get
            {
                return m_timeZone.Id;
            }
            set
            {
                m_timeZone = TimeZoneInfo.FindSystemTimeZoneById(value);
            }
        }

        private int FaultEventTypeID
        {
            get
            {
                if (s_faultEventTypeID == null)
                    Interlocked.Exchange(ref s_faultEventTypeID, GetFaultEventTypeID());

                return (int)s_faultEventTypeID;
            }
        }

        #endregion

        #region [ Methods ]

        public void WriteResults(DbAdapterContainer dbAdapterContainer, MeterDataSet meterDataSet)
        {
            m_dbAdapterContainer = dbAdapterContainer;

            //foreach (FaultLocationData.FaultSummaryRow faultSummary in dbAdapterContainer.GetAdapter<FaultSummaryTableAdapter>().GetDataByFileGroup(meterDataSet.FileGroup.ID))
            //{
            //    if ((faultSummary.IsSelectedAlgorithm != 0) && (faultSummary.IsSuppressed == 0))
            //        GenerateEmail(faultSummary.EventID);
            //}
        }

        private void GenerateEmail(int eventID)
        {
            string eventDetail;
            XslCompiledTransform transform;

            XDocument htmlDocument;
            List<XElement> formatParents;
            string html;

            eventDetail = m_dbAdapterContainer.GetAdapter<EventTableAdapter>().GetEventDetail(eventID);

            using (StringReader templateReader = new StringReader(m_emailTemplate))
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

            html = htmlDocument.ToString(SaveOptions.DisableFormatting).Replace("&amp;", "&");

            SendEmail(m_dbAdapterContainer.GetAdapter<SystemInfoDataContext>().Recipients.ToList(), "Fault email test", html);
        }

        private void SendEmail(List<Recipient> recipients, string subject, string body, params Attachment[] attachments)
        {
            const int DefaultSMTPPort = 25;

            string[] smtpServerParts = m_smtpServer.Split(':');
            string host = smtpServerParts[0];
            int port;

            if (smtpServerParts.Length <= 1 || !int.TryParse(smtpServerParts[1], out port))
                port = DefaultSMTPPort;

            using (SmtpClient smtpClient = new SmtpClient(host, port))
            using (MailMessage emailMessage = new MailMessage())
            {
                emailMessage.From = new MailAddress(m_fromAddress);
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

        private object Format(XNode node)
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

        private int GetFaultEventTypeID()
        {
            MeterData.EventTypeDataTable eventTypeTable;

            eventTypeTable = m_dbAdapterContainer.GetAdapter<EventTypeTableAdapter>().GetData();

            return Enumerable.Select(eventTypeTable
                    .Where(eventType => eventType.Name == "Fault"), eventType => eventType.ID)
                .FirstOrDefault();
        }

        #endregion

        #region [ Static ]

        // Static Fields
        private static object s_faultEventTypeID;

        // Static Constructor

        // Static Properties

        // Static Methods

        #endregion
    }
}
