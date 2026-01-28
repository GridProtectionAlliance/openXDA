//******************************************************************************************************
//  TriggeredEmailService.cs - Gbtc
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
//  05/15/2024 - Stephen C. Wills
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
using GSF.Data.Model;
using openXDA.Configuration;
using openXDA.Model;

namespace FaultData.DataWriters.Emails
{
    public class TriggeredEmailService : EmailService
    {
        public TriggeredEmailService(Func<AdoDataConnection> connectionFactory, Action<object> configure)
            : base(connectionFactory, configure)
        {
        }

        public bool SendEmail(EmailType email, List<int> eventIDs, Event evt, DateTime xdaNow, TimeSpan disallowedWindow)
        {
            if (eventIDs.Count == 0)
                return false;

            List<string> recipients = GetRecipients(email, eventIDs);

            if (recipients.Count == 0 && String.IsNullOrEmpty(email.FilePath))
                return false;

            SendEmail(email, evt, recipients, xdaNow, disallowedWindow, eventIDs, true, true, out EmailResponse _);

            return true;
        }

        /// <summary>
        /// Function that sends an email to a custom list of recipents instead of the database configured list.
        /// </summary>
        /// <remarks>Ignores the <see cref="EmailSection.BlindCopyAddress"/> setting and only sends to the list provided by this method.</remarks>
        /// <param name="email"><see cref="EmailType"/> email format to test.</param>
        /// <param name="evt"><see cref="Event"/> event data to use when creating the email.</param>
        /// <param name="xdaNow"><see cref="DateTime"/> that represents the current time on the current node.</param>
        /// <param name="recipients"><see cref="List{string}"/> list of recipents, either as emails or phone numbers.</param>
        /// <param name="saveToFile"><see cref="bool"/> flag that requests XDA save the email.</param>
        /// <param name="response">Object that holds information on the email sent.</param>
        public void SendEmail(EmailType email, Event evt, DateTime xdaNow, List<string> recipients, bool saveToFile, out EmailResponse response) =>
            SendEmail(email, evt, recipients, xdaNow, new TimeSpan(0), new List<int>(), false, saveToFile, out response);

        private void SendEmail(EmailType email, Event evt, List<string> recipients, DateTime xdaNow, TimeSpan disallowedWindow, List<int> eventIDs, bool sendBcc, bool saveToFile, out EmailResponse response)
        {
            List<Attachment> attachments = new List<Attachment>();

            response = new EmailResponse();

            try
            {
                EmailSection settings = QuerySettings();
                if (!sendBcc)
                    settings.BlindCopyAddress = null;

                TriggeredDataSourceFactory factory = new TriggeredDataSourceFactory(ConnectionFactory);
                List<TriggeredDataSourceDefinition> definitions = factory.LoadDataSourceDefinitions(email);
                IEnumerable<DataSourceResponse> dataSourceResponses = definitions.Select(definition => definition.CreateAndProcess(factory, evt));
                response.DataSources.AddRange(dataSourceResponses);

                TemplateProcessor templateProcessor = new TemplateProcessor(ConnectionFactory);
                XElement templateData = new XElement("data", response.DataSources.Select(r => r.Data));
                XDocument htmlDocument = templateProcessor.ApplyTemplate(email, templateData.ToString());
                templateProcessor.ApplyChartTransform(attachments, htmlDocument, settings.MinimumChartSamplesPerCycle);
                templateProcessor.ApplyImageEmbedTransform(attachments, htmlDocument);

                SentEmail sentEmailRecord = CreateSentEmailRecord(email, xdaNow, recipients, htmlDocument);
                using (AdoDataConnection connection = ConnectionFactory())
                {
                    // Find duplicate email messages within the time frame specified, if any exist, don't send email
                    TableOperations<SentEmail> sentEmailTable = new TableOperations<SentEmail>(connection);
                    DateTime timeThreshold = xdaNow.Subtract(disallowedWindow);

                    int duplicateEmailCount = sentEmailTable
                        .QueryRecordCountWhere("EmailTypeID = {0} AND TimeSent >= {1} AND Message = {2}", sentEmailRecord.EmailTypeID, timeThreshold, sentEmailRecord.Message);

                    if (duplicateEmailCount == 0)
                    {
                        SendEmail(recipients, htmlDocument, attachments, email, settings, response, (saveToFile ? email.FilePath : null));
                        LoadSentEmail(sentEmailRecord, eventIDs, connection);
                    }
                }
            }
            finally
            {
                attachments?.ForEach(attachment => attachment.Dispose());
            }
        }

        private void LoadSentEmail(SentEmail sentEmail, List<int> eventIDs, AdoDataConnection connection)
        {
            int sentEmailId = LoadSentEmail(sentEmail, connection);

            if (eventIDs.Count == 0)
                return;

            TableOperations<EventSentEmail> eventSentEmailTable = new TableOperations<EventSentEmail>(connection);

            foreach (int eventID in eventIDs)
            {
                if (eventSentEmailTable.QueryRecordCountWhere("EventID = {0} AND SentEmailID = {1}", eventID, sentEmailId) > 0)
                    continue;

                EventSentEmail eventSentEmail = new EventSentEmail();
                eventSentEmail.EventID = eventID;
                eventSentEmail.SentEmailID = sentEmailId;
                eventSentEmailTable.AddNewRecord(eventSentEmail);
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
    }
}
