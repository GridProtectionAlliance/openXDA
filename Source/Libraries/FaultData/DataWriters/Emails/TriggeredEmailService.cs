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

        public bool SendEmail(EmailType email, List<int> eventIDs, Event evt, DateTime xdaNow)
        {
            if (eventIDs.Count == 0)
                return false;

            List<string> recipients = GetRecipients(email, eventIDs);

            if (recipients.Count == 0 && String.IsNullOrEmpty(email.FilePath))
                return false;

            SendEmail(email, evt, recipients, xdaNow, eventIDs, true);

            return true;
        }

        public void SendEmail(EmailType email, Event evt, List<string> recipients, bool saveToFile, out EmailResponse response) =>
            SendEmail(email, evt, recipients, new DateTime(), new List<int>(), saveToFile, out response);

        public void SendEmail(EmailType email, Event evt, List<string> recipients, out EmailResponse response) =>
            SendEmail(email, evt, recipients, new DateTime(), new List<int>(), false, out response);

        public void SendEmail(EmailType email, Event evt, List<string> recipients) =>
            SendEmail(email, evt, recipients, new DateTime(), new List<int>(), false, out EmailResponse _);

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

        private void SendEmail(EmailType email, Event evt, List<string> recipients, DateTime xdaNow, List<int> eventIDs, bool saveToFile) =>
            SendEmail(email, evt, recipients, xdaNow, eventIDs, saveToFile, out EmailResponse _);

        private void SendEmail(EmailType email, Event evt, List<string> recipients, DateTime xdaNow, List<int> eventIDs, bool saveToFile, out EmailResponse response)
        {
            List<Attachment> attachments = new List<Attachment>();

            response = new EmailResponse();

            try
            {
                EmailSection settings = QuerySettings();

                TriggeredDataSourceFactory factory = new TriggeredDataSourceFactory(ConnectionFactory);
                List<TriggeredDataSourceDefinition> definitions = factory.LoadDataSourceDefinitions(email);
                IEnumerable<DataSourceResponse> dataSourceResponses = definitions.Select(definition => definition.CreateAndProcess(factory, evt));
                response.DataSources.AddRange(dataSourceResponses);

                double chartSampleRate = settings.MinimumChartSamplesPerCycle;
                TemplateProcessor templateProcessor = new TemplateProcessor(ConnectionFactory);
                XElement templateData = new XElement("data", response.DataSources.Select(r => r.Data));
                XDocument htmlDocument = templateProcessor.ApplyTemplate(email, templateData.ToString());
                templateProcessor.ApplyChartTransform(attachments, htmlDocument, settings.MinimumChartSamplesPerCycle);
                templateProcessor.ApplyImageEmbedTransform(attachments, htmlDocument);

                SendEmail(recipients, htmlDocument, attachments, email, settings, response, (saveToFile ? email.FilePath : null));
                LoadSentEmail(email, xdaNow, recipients, htmlDocument, eventIDs);
            }
            finally
            {
                attachments?.ForEach(attachment => attachment.Dispose());
            }
        }

        private void LoadSentEmail(EmailType email, DateTime now, List<string> recipients, XDocument htmlDocument, List<int> eventIDs)
        {
            int sentEmailID = LoadSentEmail(email, now, recipients, htmlDocument);

            if (eventIDs.Count == 0)
                return;

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
