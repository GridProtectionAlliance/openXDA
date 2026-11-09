//******************************************************************************************************
//  ScheduledEmailService.cs - Gbtc
//
//  Copyright © 2024, Grid Protection Alliance.  All Rights Reserved.
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
//  05/16/2024 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Xml.Linq;
using GSF.Data;
using openXDA.Configuration;
using openXDA.Model;

namespace FaultData.DataWriters.Emails
{
    public class ScheduledEmailService : EmailService
    {
        public ScheduledEmailService(Func<AdoDataConnection> connectionFactory, Action<object> configure)
            : base(connectionFactory, configure)
        {
        }

        public bool SendEmail(ScheduledEmailType email, DateTime xdaNow)
        {
            DataTable recipientInformation = GetRecipients(email);

            Func<DataRow, string> processor;

            if (email.SMS)
                processor = row => string.Format(row.ConvertField<string>("Transform"), row.ConvertField<string>("Phone"));
            else
                processor = row => row.ConvertField<string>("Email");

            List<string> recipients = recipientInformation
                .Select()
                .Select(processor)
                .Distinct()
                .ToList();

            List<int> subscriptionIDs = recipientInformation
                .Select()
                .Select(row => row.ConvertField<int>("SubscriptionID"))
                .ToList();

            if (recipients.Count == 0 && string.IsNullOrEmpty(email.FilePath))
                return false;

            SendEmail(email, recipients, subscriptionIDs, true, true, out EmailResponse _, xdaNow);

            return true;
        }

        /// <summary>
        /// Function that sends an email to a custom list of recipents instead of the database configured list.
        /// </summary>
        /// <remarks>Ignores the <see cref="EmailSection.BlindCopyAddress"/> setting and only sends to the list provided by this method.</remarks>
        /// <param name="email"><see cref="EmailType"/> email format to test.</param>
        /// <param name="xdaNow"><see cref="DateTime"/> that represents the current time on the current node.</param>
        /// <param name="recipients"><see cref="List{string}"/> list of recipents, either as emails or phone numbers.</param>
        /// <param name="response">Object that holds information on the email sent.</param>

        public void SendEmail(ScheduledEmailType email, List<string> recipients, out EmailResponse response, DateTime xdaNow) =>
            SendEmail(email, recipients, new List<int>(), false, false, out response, xdaNow);

        private void SendEmail(ScheduledEmailType email, List<string> recipients, List<int> subscriptionIDs, bool sendBcc, bool saveToFile, out EmailResponse response, DateTime xdaNow)
        {
            List<Attachment> attachments = new List<Attachment>();

            response = new EmailResponse();

            try
            {
                EmailSection settings = QuerySettings();
                if (!sendBcc)
                    settings.BlindCopyAddress = null;

                ScheduledDataSourceFactory factory = new ScheduledDataSourceFactory(ConnectionFactory);
                List<ScheduledDataSourceDefinition> definitions = factory.LoadDataSourceDefinitions(email);
                IEnumerable<DataSourceResponse> dataSourceResponses = definitions.Select(definition => definition.CreateAndProcess(factory, xdaNow));
                response.DataSources.AddRange(dataSourceResponses);

                TemplateProcessor templateProcessor = new TemplateProcessor(ConnectionFactory);
                XElement templateData = new XElement("data", response.DataSources.Select(r => r.Data));
                XDocument htmlDocument = templateProcessor.ApplyTemplate(email, templateData.ToString());
                templateProcessor.ApplyChartTransform(attachments, htmlDocument, settings.MinimumChartSamplesPerCycle);
                templateProcessor.ApplyImageEmbedTransform(attachments, htmlDocument);

                SendEmail(recipients, htmlDocument, attachments, email, settings, response, (saveToFile ? email.FilePath : null));

                SentEmail sentEmailRecord = CreateSentEmailRecord(email, xdaNow, recipients, htmlDocument);
                using (AdoDataConnection connection = ConnectionFactory())
                    LoadSentEmail(sentEmailRecord, subscriptionIDs, connection);
            }
            finally
            {
                attachments?.ForEach(attachment => attachment.Dispose());
            }
        }

        private void LoadSentEmail(SentEmail sentEmailRecord, List<int> subscriptionIDs, AdoDataConnection connection)
        {
            int sentEmailId = LoadSentEmail(sentEmailRecord, connection);

            if (subscriptionIDs.Count > 0)
            {
                connection.ExecuteNonQuery($"UPDATE UserAccountScheduledEmailType SET LastSentEmailID = {{0}} WHERE ID IN ({string.Join(",", subscriptionIDs)})", sentEmailId);
            }
        }

        public DataTable GetRecipients(ScheduledEmailType emailType)
        {
            string emailAddressQuery;

            if (!emailType.SMS)
            {
                bool requireEmailConfirm;
                using (AdoDataConnection connection = ConnectionFactory())
                    requireEmailConfirm = connection.ExecuteScalar<bool>("SELECT Value From [Setting] Where Name = 'Subscription.RequireConfirmation'");

                emailAddressQuery =
                   "SELECT UserAccountScheduledEmailType.ID AS SubscriptionID, UserAccount.Email AS Email " +
                   "FROM " +
                   "    UserAccountScheduledEmailType JOIN " +
                   "    UserAccount ON UserAccountScheduledEmailType.UserAccountID = UserAccount.ID LEFT JOIN " +
                   "    ScheduledEmailType ON UserAccountScheduledEmailType.ScheduledEmailTypeID = ScheduledEmailType.ID " +
                   "WHERE " +
                   "    UserAccountScheduledEmailType.ScheduledEmailTypeID = {0} AND " +
                   (requireEmailConfirm ? "    UserAccount.EmailConfirmed <> 0 AND " : "") +
                   "    UserAccount.Approved <> 0 AND " +
                   "(ScheduledEmailType.RequireApproval = 0 OR UserAccountScheduledEmailType.Approved <> 0)";
            }
            else
            {
                emailAddressQuery =
                  "SELECT UserAccountScheduledEmailType.ID AS SubscriptionID, UserAccount.Phone AS Phone, CellCarrier.Transform as Transform " +
                  "FROM " +
                  "    UserAccountScheduledEmailType JOIN " +
                  "    UserAccount ON UserAccountScheduledEmailType.UserAccountID = UserAccount.ID LEFT JOIN" +
                  "    UserAccountCarrier ON UserAccountCarrier.UserAccountID = UserAccount.ID LEFT JOIN " +
                  "    CellCarrier ON UserAccountCarrier.CarrierID = CellCarrier.ID LEFT JOIN " +
                  "    ScheduledEmailType ON UserAccountScheduledEmailType.ScheduledEmailTypeID = ScheduledEmailType.ID " +
                  "WHERE " +
                  "    UserAccountScheduledEmailType.ScheduledEmailTypeID = {0} AND " +
                  "    UserAccount.PhoneConfirmed <> 0 AND " +
                  "    UserAccount.Approved <> 0 AND " +
                   "   (ScheduledEmailType.RequireApproval = 0 OR UserAccountScheduledEmailType.Approved <> 0)";
            }

            using (AdoDataConnection connection = ConnectionFactory())
                return connection.RetrieveData(emailAddressQuery, emailType.ID);
        }
    }
}
