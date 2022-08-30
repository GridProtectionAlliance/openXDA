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
using System.Reflection;
using System.Security;
using System.Text;
using System.Xml.Linq;
using FaultData.DataWriters.GTC;
using GSF.Configuration;
using GSF.Data;
using GSF.Data.Model;
using GSF.Xml;
using log4net;
using openXDA.Configuration;
using openXDA.Model;

namespace FaultData.DataWriters.Emails
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

        private class DataSourceWrapper
        {
            public string Name { get; }
            public ITriggeredDataSource DataSource { get; }

            public DataSourceWrapper(string name, ITriggeredDataSource dataSource)
            {
                Name = name;
                DataSource = dataSource;
            }

            public XElement TryProcess(Event evt)
            {
                if (DataSource is null)
                    return null;

                void HandleException(Exception ex) =>
                    Log.Error($"Email data source {Name} failed to process", ex);

                XElement element = null;
                try { element = DataSource.Process(evt); }
                catch (Exception ex) { HandleException(ex); }
                return element;
            }
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

        public void SendEmail(EmailType email, List<int> eventIDs, Event evt, DateTime xdaNow)
        {
            if (eventIDs.Count == 0)
                return;

            List<string> recipients = GetRecipients(email, eventIDs);

            if (recipients.Count == 0 && String.IsNullOrEmpty(email.FilePath))
                return;

            SendEmail(email, evt, recipients, xdaNow, eventIDs, true);
            
        }

        public void SendEmail(EmailType email, Event evt, List<string> recipients) => SendEmail(email, evt, recipients, new DateTime(), new List<int>(), false);

        private void SendEmail(EmailType email, Event evt, List<string> recipients, DateTime xdaNow, List<int> eventIDs, bool saveToFile)
        {
            List<Attachment> attachments = new List<Attachment>();

            try
            {
                
                XElement templateData = GetData(email,evt);
                if (templateData == null)
                    return;
                Settings settings = new Settings(Configure);
                XDocument htmlDocument = ApplyTemplate(email, templateData.ToString());
                ApplyChartTransform(attachments, htmlDocument, settings.EmailSettings.MinimumChartSamples);
                ApplyFTTTransform(attachments, htmlDocument);
                SendEmail(recipients, htmlDocument, attachments, email, settings, (saveToFile ? email.FilePath : null));
                if (eventIDs.Count() > 0)
                    LoadSentEmail(email, xdaNow, recipients, htmlDocument, eventIDs);
            }
            finally
            {
                attachments?.ForEach(attachment => attachment.Dispose());
            }
        }

        public XElement GetData(EmailType email, Event evt)
        {
            using (AdoDataConnection connection = ConnectionFactory())
            {
                /* Load All DataSources */
                const string DataSourceIDQueryFormat =
                    "SELECT TriggeredEmailDataSourceID " +
                    "FROM TriggeredEmailDataSourceEmailType " +
                    "WHERE EmailTypeID = {0}";

                TableOperations<TriggeredEmailDataSource> dataSourceTable = new TableOperations<TriggeredEmailDataSource>(connection);
                TableOperations<TriggeredEmailDataSourceEmailType> dataSourceEmailTypeTable = new TableOperations<TriggeredEmailDataSourceEmailType>(connection);

                List<DataSourceWrapper> dataSources = dataSourceEmailTypeTable
                    .QueryRecordsWhere("EmailTypeID = {0}", email.ID)
                    .Select((cm) => new Tuple<TriggeredEmailDataSourceEmailType, TriggeredEmailDataSource>(cm, dataSourceTable.QueryRecordWhere("ID = {0}", cm.TriggeredEmailDataSourceID)))
                    .Select((m) => CreateDataSource(m.Item2, m.Item1))
                    .ToList();

                if (dataSources.Any(wrapper => wrapper.DataSource is null))
                {
                    Log.Error("Failed to create one or more data sources for triggered email; check debug logs for details");
                    return null;
                }

                if (dataSources.Count == 0)
                    return null;

                IEnumerable<XElement> eventData = dataSources
                    .Select(dataSource => dataSource.TryProcess(evt));

                return new XElement("data", eventData);
            }
        }

        public List<string> GetRecipients(EmailType emailType, List<int> eventIDs)
        {
            List<int> assetGroups = GetAssetGroups(eventIDs)
                .Select(item => item.ID)
                .ToList();

            if (assetGroups.Count == 0)
                return new List<string>();

            string assetGroupFilter = string.Join(",", assetGroups);

            string emailAddressQuery;
            Func<DataRow, string> processor;

            if (!emailType.SMS)
            {

                emailAddressQuery =
                    $"SELECT DISTINCT UserAccount.Email AS Email " +
                    $"FROM " +
                    $"    UserAccountEmailType JOIN " +
                    $"    UserAccount ON UserAccountEmailType.UserAccountID = UserAccount.ID " +
                    $"WHERE " +
                    $"    UserAccountEmailType.EmailTypeID = {{0}} AND " +
                    $"    UserAccount.EmailConfirmed <> 0 AND " +
                    $"    UserAccount.Approved <> 0 AND " +
                    $"    UserAccountEmailType.AssetGroupID IN ({assetGroupFilter})";

                processor = row => row.ConvertField<string>("Email");
            }
            else
            {
                emailAddressQuery =
                     $"SELECT DISTINCT UserAccount.Phone AS Phone, CellCarrier.Transform as Transform " +
                     $"FROM " +
                     $"    UserAccountEmailType JOIN " +
                     $"    UserAccount ON UserAccountEmailType.UserAccountID = UserAccount.ID LEFT JOIN " +
                     $"    UserAccountCarrier ON UserAccountCarrier.UserAccountID = UserAccount.ID LEFT JOIN " +
                     $"    CellCarrier ON UserAccountCarrier.CarrierID = CellCarrier.ID " +
                     $"WHERE " +
                     $"    UserAccountEmailType.EmailTypeID = {{0}} AND " +
                     $"    UserAccount.PhoneConfirmed <> 0 AND " +
                     $"    UserAccount.Approved <> 0 AND " +
                     $"    UserAccountEmailType.AssetGroupID IN ({assetGroupFilter})";

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

        public XDocument ApplyTemplate(EmailType emailType, string templateData)
        {
            string htmlText = templateData.ApplyXSLTransform(emailType.Template);

            XDocument htmlDocument = XDocument.Parse(htmlText, LoadOptions.PreserveWhitespace);
            htmlDocument.TransformAll("format", element => {
                object f;
                try { f = element.Format(); }
                catch { f = ""; }
                return f;
                });
            return htmlDocument;
        }

        private DataSourceWrapper CreateDataSource(TriggeredEmailDataSource model, TriggeredEmailDataSourceEmailType connectionModel)
        {
            try
            {
                string assemblyName = model.AssemblyName;
                string typeName = model.TypeName;
                PluginFactory<ITriggeredDataSource> pluginFactory = new PluginFactory<ITriggeredDataSource>();
                Type pluginType = pluginFactory.GetPluginType(assemblyName, typeName);
                Type dbFactoryType = typeof(Func<AdoDataConnection>);
                ConstructorInfo constructor = pluginType.GetConstructor(new[] { dbFactoryType });
                object[] parameters = (constructor is null) ? Array.Empty<object>() : new object[] { ConnectionFactory };
                ITriggeredDataSource dataSource = pluginFactory.Create(assemblyName, typeName, parameters);
                ConfigurationLoader configurationLoader = new ConfigurationLoader(connectionModel.ID, ConnectionFactory);
                dataSource.Configure(configurationLoader.Configure);
                return new DataSourceWrapper(model.Name, dataSource);
            }
            catch (Exception ex)
            {
                Log.Debug($"Failed to create ITriggeredDataSource of type {model.TypeName}", ex);
                return new DataSourceWrapper(model.Name, null);
            }
        }

        private List<AssetGroup> GetAssetGroups(List<int> eventIDs)
        {
            if (eventIDs.Count == 0)
                return new List<AssetGroup>();

            string query =
                $"SELECT DISTINCT AssetGroup.* " +
                $"FROM " +
                $"    AssetGroup LEFT OUTER JOIN " +
                $"    MeterAssetGroup ON MeterAssetGroup.AssetGroupID = AssetGroup.ID LEFT OUTER JOIN " +
                $"    AssetAssetGroup ON AssetAssetGroup.AssetGroupID = AssetGroup.ID JOIN " +
                $"    Event ON " +
                $"        Event.ID IN ({string.Join(",", eventIDs)}) AND " +
                $"        (" +
                $"            Event.MeterID = MeterAssetGroup.MeterID OR " +
                $"            Event.AssetID = AssetAssetGroup.AssetID " +
                $"        )";

            using (AdoDataConnection connection = ConnectionFactory())
            using (DataTable table = connection.RetrieveData(query))
            {
                TableOperations<AssetGroup> assetGroupTable = new TableOperations<AssetGroup>(connection);

                return table
                    .AsEnumerable()
                    .Select(assetGroupTable.LoadRecord)
                    .ToList();
            }
        }

        private void LoadSentEmail(EmailType email, DateTime now, List<string> recipients, XDocument htmlDocument, List<int> eventIDs)
        {
            int sentEmailID = LoadSentEmail(email, now, recipients, htmlDocument);

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

        public void ApplyChartTransform(List<Attachment> attachments, XDocument htmlDocument, int minimumSamples = -1)
        {
            using (AdoDataConnection connection = ConnectionFactory())
            {
                htmlDocument.TransformAll("chart", (element, index) =>
                {
                    string chartEventID = (string)element.Attribute("eventID") ?? "-1";
                    string cid = $"event{chartEventID}_chart{index:00}.png";

                    string ignoreMin = ((string)element.Attribute("ignoreMin") ?? "false");
                    int passedMinimum = bool.Parse(ignoreMin) ? -1 : minimumSamples;

                    Stream image = ChartGenerator.ConvertToChartImageStream(connection, element, passedMinimum);
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

        private void SendEmail(List<string> recipients, XDocument htmlDocument, List<Attachment> attachments, EmailType emailType, Settings settings, string filePath=null)
        {
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
                emailMessage.Subject = GetSubject(htmlDocument,emailType);
                emailMessage.Body = GetBody(htmlDocument);
                emailMessage.IsBodyHtml = true;

                if (recipients.Count == 0)
                {
                    WriteEmailToFile(filePath, emailMessage);
                    return;
                }

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

                //Write the email to a File
                WriteEmailToFile(filePath,emailMessage);
            }
        }

        private void WriteEmailToFile(string path, MailMessage mail)
        {
            if (string.IsNullOrEmpty(path))
                return;

            string datafolder = Path.GetDirectoryName(path);
            Directory.CreateDirectory(datafolder);
            string dstFile = Path.Combine(datafolder, mail.Subject);

            if (File.Exists(dstFile))
                File.Delete(dstFile);
            using (StreamWriter fileWriter = new StreamWriter(File.OpenWrite(dstFile)))
                fileWriter.Write(mail.Body);
                
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

        private string GetSubject(XDocument htmlDocument, EmailType emailType)
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

        private SmtpClient CreateSmtpClient(string smtpServer)
        {
            string[] smtpServerParts = smtpServer.Split(':');
            string host = smtpServerParts[0];

            if (smtpServerParts.Length > 1 && int.TryParse(smtpServerParts[1], out int port))
                return new SmtpClient(host, port);

            return new SmtpClient(host);
        }

        private int LoadSentEmail(EmailType email, DateTime now, List<string> recipients, XDocument htmlDocument)
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

        #endregion

        #region [ Static ]

        // Static Fields
        private static readonly ILog Log = LogManager.GetLogger(typeof(EventEmailService));

        #endregion
    }
}
