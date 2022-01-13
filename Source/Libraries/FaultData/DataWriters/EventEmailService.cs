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
using System.Text;
using System.Xml.Linq;
using FaultData.DataWriters.GTC;
using GSF.Configuration;
using GSF.Data;
using GSF.Data.Model;
using GSF.Xml;
using openXDA.Configuration;
using openXDA.Model;
using openXDA.Model.Emails;

namespace FaultData.DataWriters
{
    public class EventEmailService
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

        #endregion

        #region [ Constructors ]

        public EventEmailService(Func<AdoDataConnection> connectionFactory, Action<object> configure)
        {
            ConnectionFactory = connectionFactory;
            Configure = configure;
        }

        #endregion

        #region [ Properties ]

        private Func<AdoDataConnection> ConnectionFactory { get; }
        private Action<object> Configure { get; }
        #endregion

        #region [ Methods ]

        // #Todo Implement SMS based on userAdditionalFields
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

                using (AdoDataConnection connection = ConnectionFactory())
                using (DataTable emailAddressTable = connection.RetrieveData(emailAddressQuery, emailType.ID, string.Join(",", assetGroups)))
                {
                    return emailAddressTable
                        .Select()
                        .Select(row => row.ConvertField<string>("Email"))
                        .ToList();
                }
            }
        }

        // #ToDo Implement SMS based on userAddditionlFields
        public List<string> GetRecipients(EmailType emailType)
        {
            string emailAddressQuery;
          
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

            using (AdoDataConnection connection = ConnectionFactory())
            {
                string assetCondition = "(SELECT COUNT(*) FROM AssetAssetGroup WHERE AssetID IN(SELECT AssetID FROM Event WHERE ID IN({0})) AND AssetGroupID = AssetGroup.ID) > 0";
                string meterCondition = "(SELECT COUNT(*) FROM MeterAssetGroup WHERE MeterID IN(SELECT MeterID FROM Event WHERE ID IN({0})) AND AssetGroupID = AssetGroup.ID) > 0";
                return new TableOperations<AssetGroup>(connection).QueryRecordsWhere($"{assetCondition} OR {meterCondition}", string.Join(",", eventIDs)).ToList();
            }
        }

        public void SendEmail(EmailType email, List<int> eventIDs, Event evt, DateTime xdaNow)
        {
            if (eventIDs.Count == 0)
                return;

            List<string> recipients = GetRecipients(email, eventIDs);

            if (recipients.Count == 0)
                return;

            List<Attachment> attachments = new List<Attachment>();

            try
            {
                using (AdoDataConnection connection = ConnectionFactory())
                {
                    /* Load All DataSources */
                    IEnumerable<TriggeredEmailDataSourceEmailType> ds = new TableOperations<TriggeredEmailDataSourceEmailType>(connection).QueryRecordsWhere("EmailTypeID = {0}", email.ID);

                    if (ds.Count() == 0)
                        return;

                    Dictionary<int, TriggeredEmailDataSource> dataSourceModels = new TableOperations<TriggeredEmailDataSource>(connection).QueryRecordsWhere("ID IN ({0})", email.ID).ToDictionary((m) => m.ID);

                    List<ITriggeredDataSource> dataSources = ds.Select(m => { dataSourceModels.TryGetValue(m.TriggeredEmailDataSourceID, out TriggeredEmailDataSource model); return model; })
                        .Where(i => i != null).Select(model => CreatedataSource(model, email)).ToList();

                    XElement data = new XElement("data");

                    data.Add(dataSources.Select(d => d.Process(evt)));

                    XDocument htmlDocument = ApplyTemplate(email, data.ToString());

                    

                    ApplyChartTransform(attachments, htmlDocument);
                    ApplyFTTTransform(attachments, htmlDocument);
                    SendEmail(recipients, htmlDocument, attachments);
               
                    LoadSentEmail(xdaNow, recipients, htmlDocument, eventIDs);

                }
            }
            finally
            {
                attachments?.ForEach(attachment => attachment.Dispose());
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


        public void ApplyFTTTransform(List<Attachment> attachments, XDocument htmlDocument)
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
        private void LoadSentEmail(DateTime now, List<string> recipients, XDocument htmlDocument, List<int> eventIDs)
        {
            int sentEmailID = LoadSentEmail(now, recipients, htmlDocument);

            using (AdoDataConnection connection = ConnectionFactory())
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


        public void SendEmail(List<string> recipients, XDocument htmlDocument, List<Attachment> attachments)
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

        private string GetSubject(XDocument htmlDocument)
        {
            const string DefaultSubject = "Email sent by openXDA";

            string subject = (string)htmlDocument
                .Descendants("title")
                .FirstOrDefault();

            return subject ?? DefaultSubject;
        }

        private string GetBody(XDocument htmlDocument) => htmlDocument
            .ToString(SaveOptions.DisableFormatting)
            .Replace("&amp;", "&")
            .Replace("&lt;", "<")
            .Replace("&gt;", ">");

        private SmtpClient CreateSmtpClient(string smtpServer)
        {
            string[] smtpServerParts = smtpServer.Split(':');
            string host = smtpServerParts[0];

            if (smtpServerParts.Length > 1 && int.TryParse(smtpServerParts[1], out int port))
                return new SmtpClient(host, port);

            return new SmtpClient(host);
        }

        private int LoadSentEmail(DateTime now, List<string> recipients, XDocument htmlDocument)
        {
            using (AdoDataConnection connection = ConnectionFactory())
            {
                string toLine = string.Join(";", recipients.Select(recipient => recipient.Trim()));
                string subject = GetSubject(htmlDocument);
                string body = GetBody(htmlDocument);
                connection.ExecuteNonQuery("INSERT INTO SentEmail VALUES({0}, {1}, {2}, {3})", now, toLine, subject, body);
                return connection.ExecuteScalar<int>("SELECT @@IDENTITY");
            }
        }
        #endregion
    }
}
