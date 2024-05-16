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
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security;
using System.Xml.Linq;
using GSF.Configuration;
using GSF.Data;
using GSF.Data.Model;
using log4net;
using openXDA.Configuration;
using openXDA.Model;

namespace FaultData.DataWriters.Emails
{
    public class EmailService
    {
        #region [ Members ]

        // Nested Types
        private class Settings
        {
            public Settings(Action<object> configure) =>
                configure(this);

            [Category]
            [SettingName(EmailSection.CategoryName)]
            public EmailSection EmailSettings { get; } = new EmailSection();
        }

        private class DataSourceWrapper
        {
            public string Name { get; }
            public ITriggeredDataSource DataSourceTriggered { get; }
            public IScheduledDataSource DataSourceScheduled { get; }

            public DataSourceWrapper(string name, ITriggeredDataSource dataSourceTriggered = null, IScheduledDataSource dataSourceScheduled  = null)
            {
                Name = name;
                DataSourceTriggered = dataSourceTriggered;
                DataSourceScheduled = dataSourceScheduled;
            }
            public XElement TryProcess(DateTime xdaNow) => TryProcess(xdaNow, out _);

            public XElement TryProcess(DateTime xdaNow, out Exception exception)
            {
                if (DataSourceScheduled is null)
                {
                    exception = new NullReferenceException("DataSource was not created.");
                    Log.Debug($"Email data source {Name} was not created", exception);
                    return null;
                }

                XElement element = null;
                exception = null;
                try { element = DataSourceScheduled.Process(xdaNow); }
                catch (Exception ex) { exception = ex; }

                if (!(exception is null))
                    Log.Error($"Email data source {Name} failed to process", exception);

                return element;
            }
        }

        public class EmailResponse
        {
            public List<DataSourceResponse> DataSources { get; } = new List<DataSourceResponse>();
            public string Body { get; set; }
            public string Subject { get; set; }
        }

        #endregion

        #region [ Constructors ]

        public EmailService(Func<AdoDataConnection> connectionFactory, Action<object> configure)
        {
            ConnectionFactory = connectionFactory;
            Configure = configure;
        }

        #endregion

        #region [ Properties ]

        protected Func<AdoDataConnection> ConnectionFactory { get; }
        private Action<object> Configure { get; }

        #endregion

        #region [ Methods ]

        public void SendEmail(List<string> recipients, XDocument htmlDocument, List<Attachment> attachments, EmailTypeBase emailType, EmailResponse email, string filePath = null) =>
            SendEmail(recipients, htmlDocument, attachments, emailType, QuerySettings(), email, filePath);

        public void SendEmail(List<string> recipients, XDocument htmlDocument, List<Attachment> attachments, EmailTypeBase emailType, EmailSection settings, EmailResponse email, string filePath = null)
        {
            string smtpServer = settings.SMTPServer;

            email.Body = GetBody(htmlDocument);
            email.Subject = GetSubject(htmlDocument, emailType);

            if (string.IsNullOrEmpty(smtpServer))
                return;

            using (SmtpClient smtpClient = CreateSmtpClient(smtpServer))
            using (MailMessage emailMessage = new MailMessage())
            {
                string username = settings.Username;
                SecureString password = settings.SecurePassword;

                if (!string.IsNullOrEmpty(username) && (object)password != null)
                    smtpClient.Credentials = new NetworkCredential(username, password);

                smtpClient.EnableSsl = settings.EnableSSL;

                string fromAddress = settings.FromAddress;
                emailMessage.From = new MailAddress(fromAddress);
                emailMessage.Subject = email.Subject;
                emailMessage.Body = email.Body;
                emailMessage.IsBodyHtml = true;

                if (recipients.Count == 0)
                {
                    WriteEmailToFile(filePath, emailMessage);
                    return;
                }

                string blindCopyAddress = settings.BlindCopyAddress;
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

                //Write the email to a File
                WriteEmailToFile(filePath, emailMessage);
            }
        }

        public void SendAdminEmail(string subject, string message, List<string> replyToRecipients)
        {
            Settings settings = new Settings(Configure);
            EmailSection emailSettings = settings.EmailSettings;
            string smtpServer = emailSettings.SMTPServer;

            if (string.IsNullOrEmpty(smtpServer))
                return;

            using (SmtpClient smtpClient = CreateSmtpClient(smtpServer))
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

        public List<string> GetRecipients(EmailType emailType)
        {
            string emailAddressQuery;
            Func<DataRow, string> processor;

            if (!emailType.SMS)
            {
                bool requireEmailConfirm;
                using (AdoDataConnection connection = ConnectionFactory())
                    requireEmailConfirm  = connection.ExecuteScalar<bool>("SELECT Value From [Setting] Where Name = 'Subscription.RequireConfirmation'");

                emailAddressQuery =
                   "SELECT DISTINCT UserAccount.Email AS Email " +
                   "FROM " +
                   "    UserAccountEmailType JOIN " +
                   "    UserAccount ON UserAccountEmailType.UserAccountID = UserAccount.ID " +
                   "WHERE " +
                   "    UserAccountEmailType.EmailTypeID = {0} AND " +
                   (requireEmailConfirm ? "    UserAccount.EmailConfirmed <> 0 AND " : "") +
                   "    UserAccount.Approved <> 0";

                processor = row => row.ConvertField<string>("Email");
            }
            else
            {
                emailAddressQuery =
                  "SELECT DISTINCT UserAccount.Phone AS Phone, CellCarrier.Transform as Transform " +
                  "FROM " +
                  "    UserAccountEmailType JOIN " +
                  "    UserAccount ON UserAccountEmailType.UserAccountID = UserAccount.ID LEFT JOIN" +
                  "    UserAccountCarrier ON UserAccountCarrier.UserAccountID = UserAccount.ID LEFT JOIN " +
                  "    CellCarrier ON UserAccountCarrier.CarrierID = CellCarrier.ID " +
                  "WHERE " +
                  "    UserAccountEmailType.EmailTypeID = {0} AND " +
                  "    UserAccount.PhoneConfirmed <> 0 AND " +
                  "    UserAccount.Approved <> 0";

                processor = row => string.Format(row.ConvertField<string>("Transform"), row.ConvertField<string>("Phone"));
            }

            using (AdoDataConnection connection = ConnectionFactory())
            using (DataTable emailAddressTable = connection.RetrieveData(emailAddressQuery, emailType.ID))
            {
                return emailAddressTable
                    .Select()
                    .Select(processor)
                    .ToList();
            }
        }

        protected int LoadSentEmail(EmailTypeBase email, DateTime now, List<string> recipients, XDocument htmlDocument)
        {
            using (AdoDataConnection connection = ConnectionFactory())
            {
                SentEmail sentEmail = new SentEmail();
                sentEmail.EmailTypeID = email.ID;
                sentEmail.TimeSent = now;
                sentEmail.ToLine = string.Join(";", recipients.Select(recipient => recipient.Trim()));
                sentEmail.Subject = GetSubject(htmlDocument, email);
                sentEmail.Message = GetBody(htmlDocument);

                TableOperations<SentEmail> sentEmailTable = new TableOperations<SentEmail>(connection);
                sentEmailTable.AddNewRecord(sentEmail);
                return connection.ExecuteScalar<int>("SELECT @@IDENTITY");
            }
        }

        protected EmailSection QuerySettings() => new Settings(Configure).EmailSettings;

        private void WriteEmailToFile(string datafolder, MailMessage mail)
        {
            if (string.IsNullOrEmpty(datafolder))
                return;

            Directory.CreateDirectory(datafolder);
            string dstFile = Path.Combine(datafolder, mail.Subject);

            if (File.Exists(dstFile))
                File.Delete(dstFile);
            using (StreamWriter fileWriter = File.CreateText(dstFile))
                fileWriter.Write(mail.Body);
        }

        private SmtpClient CreateSmtpClient(string smtpServer)
        {
            string[] smtpServerParts = smtpServer.Split(':');
            string host = smtpServerParts[0];

            if (smtpServerParts.Length > 1 && int.TryParse(smtpServerParts[1], out int port))
                return new SmtpClient(host, port);

            return new SmtpClient(host);
        }

        private string GetSubject(XDocument htmlDocument, EmailTypeBase emailType)
        {
            string subject = (string)((string)htmlDocument
                .Descendants("title")
                .FirstOrDefault());

            return (subject ?? emailType.Name).Trim();
        }

        private string GetBody(XDocument htmlDocument) => htmlDocument
            .ToString(SaveOptions.DisableFormatting)
            .Replace("&amp;", "&")
            .Replace("&lt;", "<")
            .Replace("&gt;", ">");

        #endregion

        #region [ Static ]

        // Static Fields
        private static readonly ILog Log = LogManager.GetLogger(typeof(EmailService));

        #endregion
    }
}
