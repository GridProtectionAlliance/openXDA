//******************************************************************************************************
// EmailController.cs - Gbtc
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
//  05/25/2018 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using FaultData.DataWriters.Emails;
using GSF.Configuration;
using GSF.Data;
using GSF.Data.Model;
using GSF.Security;
using GSF.Web;
using Newtonsoft.Json.Linq;
using openXDA.Configuration;
using openXDA.Model;
using openXDA.Nodes;
using openXDA.Nodes.Types.Authentication;
using ConfigurationLoader = openXDA.Nodes.ConfigurationLoader;

namespace openXDA.Controllers.WebAPI
{
    [RoutePrefix("api/email")]
    public class EmailController : ApiController
    {
        #region [ Members ]
        private class Settings
        {
            public Settings(Action<object> configure) =>
                configure(this);

            [Category]
            [SettingName(EmailSection.CategoryName)]
            public EmailSection EmailSettings { get; } = new EmailSection();

            [Category]
            [SettingName(SubscriptionSection.CategoryName)]
            public SubscriptionSection SubscriptionSettings { get; } = new SubscriptionSection();

            [Category]
            [SettingName(EventEmailSection.CategoryName)]
            public EventEmailSection EventEmailSettings { get; } = new EventEmailSection();

        }

        private Host Host { get; }

        #endregion

        #region [ Constructor ]
        public EmailController(Host host)
        {
            Host = host;
        }

        #endregion

        #region [ HttpMethods ]

        [Route("sendVerification/{userID}"), HttpGet]
        public IHttpActionResult SendEmailVerification(string userID)
        {

            try 
            {
                using (AdoDataConnection connection = CreateDbConnection())
                {
                    ConfirmableUserAccount account = new TableOperations<ConfirmableUserAccount>(connection).QueryRecordWhere("ID = {0}", userID);
                    if (account == null)
                        return InternalServerError(new Exception($"User with ID {userID} does not exists"));
                    SendVerificationEmail(account.Email);
                    return Ok(1);
                }

            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("sendTextVerification/{userID}"), HttpGet]
        public IHttpActionResult SendTextVerification(string userID)
        {

            try
            {
                using (AdoDataConnection connection = CreateDbConnection())
                {
                    ConfirmableUserAccount account = new TableOperations<ConfirmableUserAccount>(connection).QueryRecordWhere("ID = {0}", userID);
                    if (account == null)
                        return InternalServerError(new Exception($"User with ID {userID} does not exists"));
                    CellCarrier cellCarrier = new TableOperations<CellCarrier>(connection).QueryRecordWhere("ID = (SELECT CarrierID FROM UserAccountCarrier WHERE UserAccountID = {0} )", account.ID);
                    if (cellCarrier == null)
                        return InternalServerError(new Exception($"User has not specified a Cell Carrier"));
                    return Ok(SendVerificationText(string.Format(cellCarrier.Transform,account.Phone)));
                }

            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("testEmail/{emailID:int}/{eventID:int}/{userID}"), HttpGet]
        public IHttpActionResult SendTestEmail(int emailID, string userID, int eventID)
        {
            Settings settings = new Settings(GetConfigurator());
            EventEmailSection eventEmailSettings = settings.EventEmailSettings;

            if (!eventEmailSettings.Enabled )
                return Ok(1);

            EmailService emailService = new EmailService(CreateDbConnection, GetConfigurator());

            using (AdoDataConnection connection = CreateDbConnection())
            {
                Event evt = new TableOperations<Event>(connection).QueryRecordWhere("ID = {0}", eventID);
                EmailType email = new TableOperations<EmailType>(connection).QueryRecordWhere("ID = {0}", emailID);

                ConfirmableUserAccount account = new TableOperations<ConfirmableUserAccount>(connection).QueryRecordWhere("ID = {0}", userID);
                if (account == null)
                    return InternalServerError(new Exception($"User with ID {userID} does not exists"));

                emailService.SendEmail(email, evt, new List<string>() { account.Email });
            }
            return Ok(1);
        }

        [Route("testData/{emailID:int}/{eventID:int}"), HttpGet]
        public IHttpActionResult TestDataSource(int emailID, int eventID)
        {
            try
            {
                EmailService emailService = new EmailService(CreateDbConnection, GetConfigurator());

                using (AdoDataConnection connection = CreateDbConnection())
                {
                    Event evt = new TableOperations<Event>(connection).QueryRecordWhere("ID = {0}", eventID);
                    EmailType email = new TableOperations<EmailType>(connection).QueryRecordWhere("ID = {0}", emailID);

                    return Ok(emailService.GetData(email, evt));
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        #endregion

        #region [ Methods ]
        private void SendVerificationEmail(string recipient)
        {
           
            Settings settings = new Settings(GetConfigurator());
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
                emailMessage.Subject = settings.SubscriptionSettings.ConfirmSubject;
                emailMessage.Body = settings.SubscriptionSettings.ConfirmTemplate;
                emailMessage.IsBodyHtml = true;

                string blindCopyAddress = emailSettings.BlindCopyAddress;

                if (string.IsNullOrEmpty(blindCopyAddress))
                {
                    emailMessage.To.Add(recipient);
                }
                else
                {
                    emailMessage.To.Add(blindCopyAddress);
                    emailMessage.Bcc.Add(recipient);
                }


                // Send the email
                smtpClient.Send(emailMessage);

            }
        }


        //# ToDo use proper Text Message logic for sending Text
        private int SendVerificationText(string recipient)
        {
            int code = -1;
            Random random = new Random();
            code = random.Next(0, 10) + random.Next(0, 10) * 10 + random.Next(0, 10) * 100 + random.Next(0, 10) * 1000 + random.Next(1, 10) * 10000;
            Settings settings = new Settings(GetConfigurator());
            EmailSection emailSettings = settings.EmailSettings;

            string smtpServer = emailSettings.SMTPServer;

            if (string.IsNullOrEmpty(smtpServer))
                return -1;

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
                emailMessage.Subject = "";
                emailMessage.Body = $"Your Activation Code is: {code.ToString("D4")}";
                emailMessage.IsBodyHtml = false;

                emailMessage.To.Add(recipient);
                // Send the email
                smtpClient.Send(emailMessage);

            }

            return code;
        }

        private SmtpClient CreateSmtpClient(string smtpServer)
        {
            string[] smtpServerParts = smtpServer.Split(':');
            string host = smtpServerParts[0];

            if (smtpServerParts.Length > 1 && int.TryParse(smtpServerParts[1], out int port))
                return new SmtpClient(host, port);

            return new SmtpClient(host);
        }

        private Action<object> GetConfigurator()
        {
            int hostID = Host.ID;
            ConfigurationLoader configurationLoader = new ConfigurationLoader(hostID, CreateDbConnection);
            return configurationLoader.Configure;
        }

        private AdoDataConnection CreateDbConnection()
        {
            AdoDataConnection connection = Host.CreateDbConnection();
            connection.DefaultTimeout = DataExtensions.DefaultTimeoutDuration;
            return connection;
        }

        #endregion

    }
}
