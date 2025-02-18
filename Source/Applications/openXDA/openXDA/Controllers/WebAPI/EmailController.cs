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
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using FaultData;
using FaultData.DataWriters.Emails;
using GSF.Configuration;
using GSF.Data;
using GSF.Data.Model;
using GSF.Security;
using openXDA.Configuration;
using openXDA.Model;
using openXDA.Nodes;
using static FaultData.DataWriters.Emails.EmailService;
using ConfigurationLoader = openXDA.Nodes.ConfigurationLoader;
using Random = GSF.Security.Cryptography.Random;

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

        private class TestEmailResponse
        {
            public Exception Exception { get; set; }

            public List<DataSourceResponse> DataSourceResponses { get; set; }
                = new List<DataSourceResponse>();
        }

        private Host Host { get; }
        private TimeZoneConverter TimeZoneConverter => LazyTimeZoneConverter.Value;
        private Lazy<TimeZoneConverter> LazyTimeZoneConverter { get; }
        private DateTime XDANow => TimeZoneConverter.ToXDATimeZone(DateTime.UtcNow);

        #endregion

        #region [ Constructor ]

        public EmailController(Host host)
        {
            Host = host;

            LazyTimeZoneConverter = new Lazy<TimeZoneConverter>(() =>
            {
                ConfigurationLoader loader = new ConfigurationLoader(Host.ID, Host.CreateDbConnection);
                return new TimeZoneConverter(loader.Configure);
            });
        }

        #endregion

        #region [ HttpMethods ]

        [Route("sendVerification/{userID}"), HttpGet]
        public IHttpActionResult SendEmailVerification(string userID)
        {
            using (AdoDataConnection connection = CreateDbConnection())
            {
                ConfirmableUserAccount account = new TableOperations<ConfirmableUserAccount>(connection).QueryRecordWhere("ID = {0}", userID);
                if (account == null)
                    return BadRequest($"User with ID {userID} does not exists");
                int code = SendVerificationEmail(account.Email);
                return Ok(code);
            }
        }

        [Route("sendTextVerification/{userID}"), HttpGet]
        public IHttpActionResult SendTextVerification(string userID)
        {
            using (AdoDataConnection connection = CreateDbConnection())
            {
                ConfirmableUserAccount account = new TableOperations<ConfirmableUserAccount>(connection).QueryRecordWhere("ID = {0}", userID);
                if (account == null)
                    return BadRequest($"User with ID {userID} does not exists");
                CellCarrier cellCarrier = new TableOperations<CellCarrier>(connection).QueryRecordWhere("ID = (SELECT CarrierID FROM UserAccountCarrier WHERE UserAccountID = {0})", account.ID);
                if (cellCarrier == null)
                    return UnprocessibleEntity($"User has not specified a Cell Carrier");
                return Ok(SendVerificationText(string.Format(cellCarrier.Transform, account.Phone)));
            }
        }

        [Route("TestSMTPServer"), HttpGet]
        public IHttpActionResult SendTestEmail()
        {
            SecurityIdentity securityIdentity = User.Identity as SecurityIdentity;
            UserData userData = securityIdentity?.Provider.UserData;
            string email = userData?.EmailAddress;

            if (string.IsNullOrEmpty(email))
            {
                string username = securityIdentity?.Name ?? userData?.LoginID;

                return !string.IsNullOrEmpty(username)
                    ? UnprocessibleEntity($"User {username} has no email address configured.")
                    : UnprocessibleEntity("Cannot send test email to unknown user.");
            }

            EmailService emailService = new EmailService(CreateDbConnection, GetConfigurator());

            try
            {
                emailService.SendEmail(new List<string>() { email }, "Test Email", "This is a Test Email", new List<string>() { });
                return Ok(1);
            }
            catch (Exception ex)
            {
                return UnprocessibleEntity(ex);
            }
        }

        [Route("testEmail/{emailID:int}/{eventID:int}/{userID}"), HttpGet]
        public IHttpActionResult SendTestEmail(int emailID, string userID, int eventID)
        {
            Settings settings = new Settings(GetConfigurator());
            EventEmailSection eventEmailSettings = settings.EventEmailSettings;
            TriggeredEmailService emailService = new TriggeredEmailService(CreateDbConnection, GetConfigurator());

            using (AdoDataConnection connection = CreateDbConnection())
            {
                Event evt = new TableOperations<Event>(connection).QueryRecordWhere("ID = {0}", eventID);
                if (evt is null)
                    return BadRequest($"Event with ID {eventID} does not exists");

                EmailType email = new TableOperations<EmailType>(connection).QueryRecordWhere("ID = {0}", emailID);
                if (email is null)
                    return BadRequest($"Email type with ID {emailID} does not exists");

                ConfirmableUserAccount account = new TableOperations<ConfirmableUserAccount>(connection).QueryRecordWhere("ID = {0}", userID);
                if (account is null)
                    return BadRequest($"User with ID {userID} does not exists");

                TestEmailResponse response = new TestEmailResponse();

                try
                {
                    emailService.SendEmail(email, evt, XDANow, new List<string>() { account.Email }, false, out EmailResponse resultEmail);
                    response.DataSourceResponses = resultEmail.DataSources;
                    return Ok(response);
                }
                catch (Exception ex)
                {
                    response.Exception = ex;
                    return UnprocessibleEntity(response);
                }
            }
        }

        [Route("testFile/{emailID:int}/{eventID:int}/{save:bool}"), HttpGet]
        public async Task<HttpResponseMessage> TestEmailFile(int emailID, int eventID, bool save, CancellationToken cancellationToken)
        {
            string subject = null;

            IHttpActionResult RunTest()
            {
                Settings settings = new Settings(GetConfigurator());
                EventEmailSection eventEmailSettings = settings.EventEmailSettings;
                TriggeredEmailService emailService = new TriggeredEmailService(CreateDbConnection, GetConfigurator());

                using (AdoDataConnection connection = CreateDbConnection())
                {
                    Event evt = new TableOperations<Event>(connection).QueryRecordWhere("ID = {0}", eventID);
                    if (evt is null)
                        return BadRequest($"Event with ID {eventID} does not exists");


                    EmailType email = new TableOperations<EmailType>(connection).QueryRecordWhere("ID = {0}", emailID);
                    if (email is null)
                        return BadRequest($"Email type with ID {emailID} does not exists");

                    try
                    {
                        emailService.SendEmail(email, evt, XDANow, new List<string>() { }, save, out EmailResponse emailResponse);
                        subject = emailResponse.Subject;
                        return Ok(emailResponse.Body);
                    }
                    catch (Exception ex)
                    {
                        Exception wrapper = new Exception("Email type does not produce valid email please check the Template", ex);
                        return UnprocessibleEntity(wrapper.ToString());
                    }
                }
            }

            IHttpActionResult testResult = RunTest();
            HttpResponseMessage httpResponse = await testResult.ExecuteAsync(cancellationToken);

            if (!(subject is null))
            {
                httpResponse.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                {
                    FileName = subject
                };
            }

            return httpResponse;
        }

        [Route("testReport/{reportID:int}/{userID}/{current}"), HttpGet]
        public IHttpActionResult SendTestReport(int reportID, string userID, string current)
        {
            Action<object> configurator = GetConfigurator();
            Settings settings = new Settings(configurator);
            ScheduledEmailService emailService = new ScheduledEmailService(CreateDbConnection, configurator);

            using (AdoDataConnection connection = CreateDbConnection())
            {
                ScheduledEmailType report = new TableOperations<ScheduledEmailType>(connection).QueryRecordWhere("ID = {0}", reportID);
                if (report is null)
                    return BadRequest($"Scheduled Email type with ID {reportID} does not exists");

                ConfirmableUserAccount account = new TableOperations<ConfirmableUserAccount>(connection).QueryRecordWhere("ID = {0}", userID);
                if (account is null)
                    return BadRequest($"User with ID {userID} does not exists");

                TestEmailResponse response = new TestEmailResponse();

                try
                {
                    if (!DateTime.TryParse(current, out DateTime xdaNow))
                        xdaNow = DateTime.UtcNow;
                    
                    emailService.SendEmail(report, new List<string>() { account.Email }, out EmailResponse emailResponse, xdaNow);
                    response.DataSourceResponses = emailResponse.DataSources;
                    return Ok(response);
                }
                catch (Exception ex)
                {
                    response.Exception = ex;
                    return UnprocessibleEntity(response);
                }
            }
        }

        [Route("testData/{emailID:int}/{eventID:int}"), HttpGet]
        public IHttpActionResult TestDataSource(int emailID, int eventID)
        {
            TriggeredDataSourceFactory factory = new TriggeredDataSourceFactory(CreateDbConnection);

            using (AdoDataConnection connection = CreateDbConnection())
            {
                Event evt = new TableOperations<Event>(connection).QueryRecordWhere("ID = {0}", eventID);
                EmailType email = new TableOperations<EmailType>(connection).QueryRecordWhere("ID = {0}", emailID);
                List<TriggeredDataSourceDefinition> definitions = factory.LoadDataSourceDefinitions(email);

                List<DataSourceResponse> dataSourceResponses = definitions
                    .Select(definition => definition.CreateAndProcess(factory, evt))
                    .ToList();

                return Ok(dataSourceResponses);
            }
        }

        [Route("testReportData/{reportID:int}/{current}"), HttpGet]
        public IHttpActionResult TestReportDataSource(int reportID, string current)
        {
            ScheduledDataSourceFactory factory = new ScheduledDataSourceFactory(CreateDbConnection);

            using (AdoDataConnection connection = CreateDbConnection())
            {
                ScheduledEmailType report = new TableOperations<ScheduledEmailType>(connection).QueryRecordWhere("ID = {0}", reportID);

                if (!DateTime.TryParse(current, out DateTime xdaNow))
                    xdaNow = DateTime.UtcNow;

                List<ScheduledDataSourceDefinition> definitions = factory.LoadDataSourceDefinitions(report);

                List<DataSourceResponse> dataSourceResponses = definitions
                    .Select(definition => definition.CreateAndProcess(factory, xdaNow))
                    .ToList();

                return Ok(dataSourceResponses);
            }
        }

        #endregion

        #region [ Methods ]

        private int SendVerificationEmail(string recipient)
        {
            int code = GenerateCode();
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
                emailMessage.Subject = settings.SubscriptionSettings.ConfirmSubject;
                emailMessage.Body = string.Format(settings.SubscriptionSettings.ConfirmTemplate, code.ToString("D4"));
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
                return code;
            }
        }

        //# ToDo use proper Text Message logic for sending Text
        private int SendVerificationText(string recipient)
        {
            int code = GenerateCode();

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

        private int GenerateCode() =>
            Random.Int32Between(10000, 100000);

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

        private IHttpActionResult UnprocessibleEntity<T>(T value) =>
            Content((HttpStatusCode)422, value);

        #endregion
    }
}
