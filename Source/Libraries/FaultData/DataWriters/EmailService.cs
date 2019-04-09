//******************************************************************************************************
//  EmailService.cs - Gbtc
//
//  Copyright © 2018, Grid Protection Alliance.  All Rights Reserved.
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
//  07/31/2018 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security;
using System.Threading;
using System.Xml.Linq;
using FaultData.Configuration;
using GSF;
using GSF.Configuration;
using GSF.Data;
using GSF.Data.Model;
using GSF.Xml;
using openXDA.Model;

namespace FaultData.DataWriters
{
    public class EmailService
    {
        #region [ Members ]

        // Fields
        private Func<AdoDataConnection> m_connectionFactory;
        private string m_connectionString;

        #endregion

        #region [ Properties ]

        public Func<AdoDataConnection> ConnectionFactory
        {
            get => Interlocked.CompareExchange(ref m_connectionFactory, null, null);
            set => Interlocked.Exchange(ref m_connectionFactory, value);
        }

        public string ConnectionString
        {
            get => Interlocked.CompareExchange(ref m_connectionString, null, null);
            set => Interlocked.Exchange(ref m_connectionString, value);
        }

        private Dictionary<string, string> Settings =>
            (ConnectionString ?? "").ParseKeyValuePairs();

        private string TimeZoneID =>
            Settings.TryGetValue("XDATimeZone", out string timeZoneID)
                ? timeZoneID
                : "UTC";

        private string EmailConnectionString =>
            Settings.TryGetValue("Email", out string emailConnectionString)
                ? emailConnectionString
                : string.Empty;

        #endregion

        #region [ Methods ]

        public List<string> GetRecipients(EmailType emailType)
        {
            string addressField = emailType.SMS ? "Phone" : "Email";

            string emailAddressQuery =
                $"SELECT DISTINCT {addressField} AS Email " +
                $"FROM " +
                $"    UserAccount JOIN " +
                $"    UserAccountEmailType ON UserAccountEmailType.UserAccountID = UserAccount.ID " +
                $"WHERE " +
                $"    UserAccountEmailType.EmailTypeID = {{0}} AND " +
                $"    UserAccount.{addressField}Confirmed <> 0 AND " +
                $"    UserAccount.Approved <> 0";

            using (AdoDataConnection connection = ConnectionFactory())
            using (DataTable emailAddressTable = connection.RetrieveData(emailAddressQuery, emailType.ID))
            {
                return emailAddressTable
                    .Select()
                    .Select(row => row.ConvertField<string>("Email"))
                    .ToList();
            }
        }

        public XDocument ApplyTemplate(EmailType emailType, string templateData)
        {
            using (AdoDataConnection connection = ConnectionFactory())
            {
                TableOperations<XSLTemplate> templateTable = new TableOperations<XSLTemplate>(connection);
                XSLTemplate xslTemplate = templateTable.QueryRecordWhere("ID = {0}", emailType.XSLTemplateID);
                string htmlText = templateData.ApplyXSLTransform(xslTemplate.Template);

                XDocument htmlDocument = XDocument.Parse(htmlText, LoadOptions.PreserveWhitespace);
                htmlDocument.TransformAll("format", element => element.Format());
                return htmlDocument;
            }
        }

        public int LoadSentEmail(List<string> recipients, XDocument htmlDocument)
        {
            using (AdoDataConnection connection = ConnectionFactory())
            {
                TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById(TimeZoneID);
                DateTime now = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZone);
                string toLine = string.Join(";", recipients.Select(recipient => recipient.Trim()));
                string subject = GetSubject(htmlDocument);
                string body = GetBody(htmlDocument);
                connection.ExecuteNonQuery("INSERT INTO SentEmail VALUES({0}, {1}, {2}, {3})", now, toLine, subject, body);
                return connection.ExecuteScalar<int>("SELECT @@IDENTITY");
            }
        }

        public void SendEmail(List<string> recipients, XDocument htmlDocument, List<Attachment> attachments)
        {
            const int DefaultSMTPPort = 25;

            EmailSettings emailSettings = GetEmailSettings();
            string smtpServer = emailSettings.SMTPServer;

            if (string.IsNullOrEmpty(smtpServer))
                return;

            string[] smtpServerParts = smtpServer.Split(':');
            string host = smtpServerParts[0];
            int port;

            if (smtpServerParts.Length <= 1 || !int.TryParse(smtpServerParts[1], out port))
                port = DefaultSMTPPort;

            using (SmtpClient smtpClient = new SmtpClient(host, port))
            using (MailMessage emailMessage = new MailMessage())
            {
                string username = emailSettings.Username;
                SecureString password = emailSettings.SecurePassword;

                if (!string.IsNullOrEmpty(username) && (object)password != null)
                    smtpClient.Credentials = new NetworkCredential(username, password);

                smtpClient.EnableSsl = emailSettings.EnableSSL;

                string fromAddress = emailSettings.FromAddress;
                emailMessage.From = new MailAddress(fromAddress);
                emailMessage.Subject = GetSubject(htmlDocument);
                emailMessage.Body = GetBody(htmlDocument);
                emailMessage.IsBodyHtml = true;

                string blindCopyAddress = emailSettings.BlindCopyAddress;
                string recipientList = string.Join(",", recipients.Select(recipient => recipient.Trim()));

                if (string.IsNullOrEmpty(blindCopyAddress))
                {
                    emailMessage.To.Add(recipientList);
                }
                else
                {
                    emailMessage.To.Add(blindCopyAddress);
                    emailMessage.Bcc.Add(recipientList);
                }

                // Create the image attachment for the email message
                foreach (Attachment attachment in attachments)
                    emailMessage.Attachments.Add(attachment);

                // Send the email
                smtpClient.Send(emailMessage);
            }
        }

        public void SendAdminEmail(string subject, string message, List<string> replyToRecipients)
        {
            const int DefaultSMTPPort = 25;

            EmailSettings emailSettings = GetEmailSettings();
            string smtpServer = emailSettings.SMTPServer;

            if (string.IsNullOrEmpty(smtpServer))
                return;

            string[] smtpServerParts = smtpServer.Split(':');
            string host = smtpServerParts[0];
            int port;

            if (smtpServerParts.Length <= 1 || !int.TryParse(smtpServerParts[1], out port))
                port = DefaultSMTPPort;

            using (SmtpClient smtpClient = new SmtpClient(host, port))
            using (MailMessage emailMessage = new MailMessage())
            {
                string username = emailSettings.Username;
                SecureString password = emailSettings.SecurePassword;

                if (!string.IsNullOrEmpty(username) && (object)password != null)
                    smtpClient.Credentials = new NetworkCredential(username, password);

                smtpClient.EnableSsl = emailSettings.EnableSSL;

                string fromAddress = emailSettings.FromAddress;
                string toAddress = emailSettings.AdminAddress;
                emailMessage.From = new MailAddress(fromAddress);
                emailMessage.To.Add(toAddress);
                emailMessage.Subject = subject;
                emailMessage.Body = message;

                // Add the specified To recipients for the email message
                foreach (string replyToRecipient in replyToRecipients)
                    emailMessage.ReplyToList.Add(replyToRecipient.Trim());

                // Send the email
                smtpClient.Send(emailMessage);
            }
        }

        private string GetSubject(XDocument htmlDocument)
        {
            const string DefaultSubject = "Email sent by openXDA";

            string subject = (string)htmlDocument
                .Descendants("title")
                .FirstOrDefault();

            return subject ?? DefaultSubject;
        }

        private string GetBody(XDocument htmlDocument)
        {
            return htmlDocument
                .ToString(SaveOptions.DisableFormatting)
                .Replace("&amp;", "&")
                .Replace("&lt;", "<")
                .Replace("&gt;", ">");
        }

        private EmailSettings GetEmailSettings()
        {
            EmailSettings settings = new EmailSettings();
            ConnectionStringParser<SettingAttribute> parser = new ConnectionStringParser<SettingAttribute>();
            parser.ParseConnectionString(EmailConnectionString, settings);
            return settings;
        }

        #endregion
    }
}
