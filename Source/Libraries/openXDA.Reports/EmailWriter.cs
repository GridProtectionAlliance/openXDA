//******************************************************************************************************
//  EmailWriter.cs - Gbtc
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
//  07/13/2018 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************

using GSF;
using GSF.Configuration;
using GSF.Data;
using GSF.Data.Model;
using GSF.Threading;
using GSF.Web.Model;
using GSF.Xml;
using log4net;
using openXDA.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace openXDA.Reports
{
    public class EmailWriter
    {
        #region [ Members ]


        // Constants

        // Fields

        #endregion

        #region [ Constructors ]

        public EmailWriter(DataContext dataContext, PQReportsSettings reportsSettings, EmailSettings emailSettings)
        {
            ReportSettings = reportsSettings;
            DataContext = dataContext;
            EmailSettings = emailSettings;
            XDATimeZone = TimeZoneInfo.FindSystemTimeZoneById(dataContext.Connection.ExecuteScalar<string>("SELECT Value FROM Setting WHERE Name = 'XDATimeZone'"));
        }

        #endregion

        #region [ Properties ]
        public EmailSettings EmailSettings { get; }
        public PQReportsSettings ReportSettings { get; }
        public DataContext DataContext { get; }
        public TimeZoneInfo XDATimeZone { get; }
        #endregion

        #region [ Methods ]

        public void Execute(int month, int year)
        {
            GenerateEmail(month,  year);
        }

        private void GenerateEmail(int month, int year)
        {
            string sql = $"SELECT Email FROM UserAccount WHERE Email IS NOT NULL AND Email <> '' AND ID IN (SELECT UserAccountID FROM EmailGroupUserAccount WHERE EmailGroupID IN (SELECT EmailGroupID FROM EmailGroupType WHERE EmailTypeID IN (SELECT ID FROM EmailType WHERE  EmailCategoryID = (SELECT ID FROM EmailCategory WHERE Name = 'PQReport'))))";
            DataTable emailTable = DataContext.Connection.RetrieveData(sql);
            List<string> recipients = emailTable.Select().Select(row => row.ConvertField<string>("Email")).ToList();
            string template = DataContext.Connection.ExecuteScalar<string>("SELECT Template FROM XSLTemplate WHERE Name = 'PQReport'");
            string data = DataContext.Connection.ExecuteScalar<string>(@"
            SELECT
	            {0} as [Month],
	            {1} as [Year],
	            (select value from DashSettings where Name = 'System.XDAInstance') as [XDALink],
	            (SELECT
		            Meter.AssetKey as [Meter],
		            Report.Results as [Result],
		            Report.ID as [ReportID]
	            FROM 
		            Report JOIN
		            Meter ON Report.MeterID = Meter.ID
	            WHERE 
		            Month = {0} AND
		            Year = {1}
	            FOR XML RAW('Report') ,TYPE, ELEMENTS) as [Reports]
	            FOR XML RAW ('PQReport'),TYPE, ELEMENTS
            ", month, year);

            XDocument htmlDocument = XDocument.Parse(data.ApplyXSLTransform(template), LoadOptions.PreserveWhitespace);

            try
            {
                string subject = (string)htmlDocument.Descendants("title").FirstOrDefault() ?? "Fault detected by openXDA";
                string html = htmlDocument.ToString(SaveOptions.DisableFormatting).Replace("&amp;", "&").Replace("&lt;", "<").Replace("&gt;", ">");


                SendEmail(recipients, subject, html);
                LoadSentEmail(recipients, subject, html);

            }
            catch(Exception ex)
            {
                Log.Error(ex.ToString());
            }
        }


        private int LoadSentEmail(List<string> recipients, string subject, string body)
        {
            DateTime now = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, XDATimeZone);
            string toLine = string.Join("; ", recipients.Select(recipient => recipient.Trim()));
            DataContext.Connection.ExecuteNonQuery("INSERT INTO SentEmail VALUES({0}, {1}, {2}, {3})", now, toLine, subject, body);
            return DataContext.Connection.ExecuteScalar<int>("SELECT @@IDENTITY");
        }

        private void SendEmail(List<string> recipients, string subject, string body)
        {
            const int DefaultSMTPPort = 25;

            if (string.IsNullOrEmpty(EmailSettings.SMTPServer))
                return;

            string[] smtpServerParts = EmailSettings.SMTPServer.Split(':');
            string host = smtpServerParts[0];
            int port;

            if (smtpServerParts.Length <= 1 || !int.TryParse(smtpServerParts[1], out port))
                port = DefaultSMTPPort;

            using (SmtpClient smtpClient = new SmtpClient(host, port))
            using (MailMessage emailMessage = new MailMessage())
            {
                if (!string.IsNullOrEmpty(EmailSettings.Username) && (object)EmailSettings.SecurePassword != null)
                    smtpClient.Credentials = new NetworkCredential(EmailSettings.Username, EmailSettings.SecurePassword);

                smtpClient.EnableSsl = EmailSettings.EnableSSL;

                emailMessage.From = new MailAddress(EmailSettings.FromAddress);
                emailMessage.Subject = subject;
                emailMessage.Body = body;
                emailMessage.IsBodyHtml = true;

                // Add the specified To recipients for the email message
                foreach (string toRecipient in recipients)
                    emailMessage.To.Add(toRecipient.Trim());

                // Send the email
                smtpClient.Send(emailMessage);
            }
        }

        public void SendEmailWithAttachment(List<string> recipients, string subject, string body, MemoryStream memoryStream, string name, string mediaType)
        {
            const int DefaultSMTPPort = 25;

            if (string.IsNullOrEmpty(EmailSettings.SMTPServer))
                return;

            string[] smtpServerParts = EmailSettings.SMTPServer.Split(':');
            string host = smtpServerParts[0];
            int port;

            if (smtpServerParts.Length <= 1 || !int.TryParse(smtpServerParts[1], out port))
                port = DefaultSMTPPort;

            using (SmtpClient smtpClient = new SmtpClient(host, port))
            using (MailMessage emailMessage = new MailMessage())
            {
                if (!string.IsNullOrEmpty(EmailSettings.Username) && (object)EmailSettings.SecurePassword != null)
                    smtpClient.Credentials = new NetworkCredential(EmailSettings.Username, EmailSettings.SecurePassword);

                smtpClient.EnableSsl = EmailSettings.EnableSSL;

                emailMessage.From = new MailAddress(EmailSettings.FromAddress);
                emailMessage.Subject = subject;
                emailMessage.Body = body;
                emailMessage.IsBodyHtml = true;
                emailMessage.Attachments.Add(new Attachment(memoryStream, name, mediaType));

                // Add the specified To recipients for the email message
                foreach (string toRecipient in recipients)
                    emailMessage.To.Add(toRecipient.Trim());

                // Send the email
                smtpClient.Send(emailMessage);
            }
        }

        #endregion

        #region [ Static ]
        // Static Fields
        private static readonly ILog Log = LogManager.GetLogger(typeof(EmailWriter));
        #endregion

    }
}
