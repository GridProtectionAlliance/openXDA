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
using GSF;
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
            SendEmail(recipients, htmlDocument, attachments, emailType, email, out _, filePath);

        public void SendEmail(List<string> recipients, XDocument htmlDocument, List<Attachment> attachments, EmailTypeBase emailType, EmailSection settings, EmailResponse email, string filePath = null) =>
            SendEmail(recipients, htmlDocument, attachments, emailType, settings, email, out _, filePath);

        public void SendEmail(List<string> recipients, XDocument htmlDocument, List<Attachment> attachments, EmailTypeBase emailType, EmailResponse email, out List<string> unreachedRecipients, string filePath = null) =>
            SendEmail(recipients, htmlDocument, attachments, emailType, QuerySettings(), email, out unreachedRecipients, filePath);

        public void SendEmail(List<string> recipients, XDocument htmlDocument, List<Attachment> attachments, EmailTypeBase emailType, EmailSection settings, EmailResponse email, out List<string> unreachedRecipients, string filePath = null)
        {
            string smtpServer = settings.SMTPServer;

            email.Body = !emailType.SMS ? GetBody(htmlDocument) : GetBodyNoHTML(htmlDocument);
            email.Subject = GetSubject(htmlDocument, emailType);
            unreachedRecipients = new List<string>();

            if (recipients.Count == 0)
            {
                WriteEmailToFile(filePath, email.Subject, email.Body);
                return;
            }

            if (string.IsNullOrEmpty(smtpServer))
                return;

            using (SmtpClient smtpClient = CreateSmtpClient(smtpServer))
            {
                // the blind copy address, when set, is added as the main recipient and the other recipients are added as blind copy, in order to avoid "reply all" shenanigans.
                string blindCopyAddress = settings.BlindCopyAddress;

                int nBcc = string.IsNullOrWhiteSpace(blindCopyAddress)
                    ? 0
                    : blindCopyAddress
                    .Split(',')
                    .Count(a => !string.IsNullOrWhiteSpace(a));

                int batchSize = settings.MaxRecipients - nBcc;
                if (batchSize <= 0)
                    throw new InvalidOperationException($"Email.MaxRecipients ({settings.MaxRecipients}) must exceed the number of blind copy addresses ({nBcc}).");

                // Split emails into max number of recipients per email
                int nEmails = (int)Math.Ceiling(recipients.Count / (double)batchSize);

                string username = settings.Username;
                SecureString password = settings.SecurePassword;

                if (!string.IsNullOrEmpty(username) && (object)password != null)
                    smtpClient.Credentials = new NetworkCredential(username, password);

                smtpClient.EnableSsl = settings.EnableSSL;

                List<Exception> errors = new List<Exception>();

                List<IGrouping<int, string>> batches = recipients
                    .Select((recipient, index) => (GroupIndex: index / batchSize, Address: recipient.Trim()))
                    .GroupBy(tuple => tuple.GroupIndex, tuple => tuple.Address)
                    .ToList();

                for (int i = 0; i < nEmails; i++)
                {
                    using (MailMessage emailMessage = new MailMessage())
                    {
                        string fromAddress = settings.FromAddress;
                        emailMessage.From = new MailAddress(fromAddress);
                        emailMessage.Subject = email.Subject;
                        emailMessage.Body = email.Body;
                        emailMessage.IsBodyHtml = !emailType.SMS;
                        string recipientList = string.Join(",", batches[i]);

                        try
                        {
                            if (string.IsNullOrEmpty(blindCopyAddress))
                            {
                                emailMessage.To.Add(recipientList);
                            }
                            else
                            {
                                emailMessage.To.Add(blindCopyAddress);
                                emailMessage.Bcc.Add(recipientList);
                            }

                            try
                            {
                                // Create the image attachment for the email message
                                foreach (Attachment attachment in attachments)
                                    emailMessage.Attachments.Add(attachment);

                                // Send the email
                                smtpClient.Send(emailMessage);
                            }
                            finally
                            {
                                emailMessage.Attachments.Clear();   // detach, so Dispose leaves them open
                            }

                        }
                        catch (Exception ex)
                        {
                            errors.Add(new InvalidOperationException($"Failed to send email batch {i + 1} of {nEmails}: {ex.Message}", ex));
                            unreachedRecipients.AddRange(batches[i]);
                        }
                    }
                }

                // Write the file once 
                WriteEmailToFile(filePath, email.Subject, email.Body);

                // Throw error if no email was successfully sent, otherwise allow execution to continue and create the email record.
                if (errors.Count == nEmails)
                    throw new AggregateException($"{errors.Count} of {nEmails} email batches failed to send.", errors);

                if (errors.Count > 0)
                    Log.Error($"{errors.Count} of {nEmails} email batches failed to send.", new AggregateException(errors));
            }
        }

        public void SendEmail(List<string> recipients, string subject, string message, List<string> replyToRecipients)
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
                emailMessage.From = new MailAddress(fromAddress);

                string recipientList = string.Join(",", recipients.Select(recipient => recipient.Trim()));
                emailMessage.To.Add(recipientList);
                emailMessage.Subject = subject;
                emailMessage.Body = message;

                // Add the specified To recipients for the email message
                foreach (string replyToRecipient in replyToRecipients)
                    emailMessage.ReplyToList.Add(replyToRecipient.Trim());

                // Send the email
                smtpClient.Send(emailMessage);
            }
        }

        public void SendAdminEmail(string subject, string message, List<string> replyToRecipients)
        {
            Settings settings = new Settings(Configure);
            EmailSection emailSettings = settings.EmailSettings;
            SendEmail(new List<string> { emailSettings.AdminAddress }, subject, message, replyToRecipients);
        }

        public List<string> GetRecipients(EmailType emailType)
        {
            string emailAddressQuery;
            Func<DataRow, string> processor;

            if (!emailType.SMS)
            {
                bool requireEmailConfirm;
                using (AdoDataConnection connection = ConnectionFactory())
                    requireEmailConfirm = connection.ExecuteScalar<bool>("SELECT Value From [Setting] Where Name = 'Subscription.RequireConfirmation'");

                emailAddressQuery =
                   "SELECT DISTINCT UserAccount.Email AS Email " +
                   "FROM " +
                   "    UserAccountEmailType JOIN " +
                   "    UserAccount ON UserAccountEmailType.UserAccountID = UserAccount.ID LEFT JOIN" +
                   "    EmailType ON UserAccountEmailType.EmailTypeID = EmailType.ID " +
                   "WHERE " +
                   "    UserAccountEmailType.EmailTypeID = {0} AND " +
                   (requireEmailConfirm ? "    UserAccount.EmailConfirmed <> 0 AND " : "") +
                   "    (EmailType.RequireApproval = 0 OR UserAccountEmailType.Approved <> 0) AND " +
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
                  "    EmailType ON UserAccountEmailType.EmailTypeID = EmailType.ID LEFT JOIN" +
                  "    CellCarrier ON UserAccountCarrier.CarrierID = CellCarrier.ID " +
                  "WHERE " +
                  "    UserAccountEmailType.EmailTypeID = {0} AND " +
                  "    UserAccount.PhoneConfirmed <> 0 AND " +
                  "    (EmailType.RequireApproval = 0 OR UserAccountEmailType.Approved <> 0) AND " +
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

        protected SentEmail CreateSentEmailRecord(EmailTypeBase email, DateTime now, List<string> recipients, XDocument htmlDocument)
        {
            SentEmail sentEmail = new SentEmail();
            sentEmail.EmailTypeID = email.ID;
            sentEmail.TimeSent = now;
            sentEmail.ToLine = string.Join(";", recipients.Select(recipient => recipient.Trim()));
            sentEmail.Subject = GetSubject(htmlDocument, email);
            sentEmail.Message = GetBody(htmlDocument);
            return sentEmail;
        }

        protected int LoadSentEmail(SentEmail sentEmail, AdoDataConnection connection)
        {
            TableOperations<SentEmail> sentEmailTable = new TableOperations<SentEmail>(connection);
            sentEmailTable.AddNewRecord(sentEmail);
            return connection.ExecuteScalar<int>("SELECT @@IDENTITY");
        }

        protected EmailSection QuerySettings() => new Settings(Configure).EmailSettings;

        private void WriteEmailToFile(string datafolder, string subject, string body)
        {
            if (string.IsNullOrEmpty(datafolder))
                return;

            Directory.CreateDirectory(datafolder);
            string dstFile = Path.Combine(datafolder, subject);

            if (File.Exists(dstFile))
                File.Delete(dstFile);

            using (StreamWriter fileWriter = File.CreateText(dstFile))
                fileWriter.Write(body);
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
            .Replace("&amp;", "&");

        private string GetBodyNoHTML(XDocument htmlDocument)
        {
            string body = (string)htmlDocument
                .Descendants("body")
                .FirstOrDefault();

            return body.Replace("&amp;", "&");
        }

        #endregion

        #region [ Static ]

        // Static Fields
        private static readonly ILog Log = LogManager.GetLogger(typeof(EmailService));

        #endregion
    }
}
