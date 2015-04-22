//******************************************************************************************************
//  HTMLWriter.cs - Gbtc
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
using System.Linq;
using System.Net.Mail;
using FaultData.Database;
using FaultData.DataResources;
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
        private string m_smtpServer;
        private string m_fromAddress;
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

        #endregion

        #region [ Methods ]

        public void WriteResults(DbAdapterContainer dbAdapterContainer, MeterDataSet meterDataSet)
        {
            List<SystemEventResource.SystemEvent> systemEvents;
            DateTime now;

            systemEvents = meterDataSet.GetResource<SystemEventResource>().SystemEvents;
            now = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, m_timeZone);

            foreach (SystemEventResource.SystemEvent systemEvent in systemEvents)
                dbAdapterContainer.EventAdapter.CreateEventSnapshots(systemEvent.StartTime, systemEvent.EndTime, m_timeTolerance, now);
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

        #endregion
    }
}
