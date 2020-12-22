//******************************************************************************************************
//  EventEmailService.cs - Gbtc
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
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Xml.Linq;
using FaultData.DataWriters.GTC;
using GSF.Data;
using GSF.Data.Model;
using GSF.Xml;
using openXDA.Model;

namespace FaultData.DataWriters
{
    public class EventEmailService
    {
        #region [ Constructors ]

        public EventEmailService(Func<AdoDataConnection> connectionFactory, Action<object> configure)
        {
            EmailService = new EmailService(connectionFactory, configure);
            ConnectionFactory = connectionFactory;
        }

        #endregion

        #region [ Properties ]

        private EmailService EmailService { get; }
        private Func<AdoDataConnection> ConnectionFactory { get; }

        #endregion

        #region [ Methods ]

        public List<int> GetEventIDs(string templateData)
        {
            XDocument dataDocument = XDocument.Parse(templateData);

            IEnumerable<XElement> eventElements = dataDocument.Root
                .Elements("Events")
                .Elements("Event")
                .ToList();

            IEnumerable<string> idAttributes = eventElements
                .Attributes("id")
                .Select(attribute => (string)attribute);

            return eventElements
                .Elements("ID")
                .Select(element => (string)element)
                .Concat(idAttributes)
                .Where(text => int.TryParse(text, out int eventID))
                .Select(text => int.Parse(text))
                .ToList();
        }

        public List<string> GetRecipients(EmailType emailType, List<int> eventIDs)
        {
            using (AdoDataConnection connection = ConnectionFactory())
            {
                string eventIDList = string.Join(",", eventIDs);
                string meterIDList;
                string lineIDList;

                using (DataTable assetIDTable = connection.RetrieveData($"SELECT MeterID, LineID FROM Event WHERE ID IN ({eventIDList})"))
                {
                    DataRow[] assetIDs = assetIDTable.Select();
                    List<int> meterIDs = assetIDs.Select(row => row.ConvertField<int>("MeterID")).ToList();
                    List<int> lineIDs = assetIDs.Select(row => row.ConvertField<int>("LineID")).ToList();
                    meterIDList = string.Join(",", meterIDs);
                    lineIDList = string.Join(",", lineIDs);
                }

                string addressField = emailType.SMS ? "Phone" : "Email";

                string emailAddressQuery =
                    $"SELECT DISTINCT UserAccount.{addressField} AS Email " +
                    $"FROM " +
                    $"    UserAccount JOIN " +
                    $"    UserAccountEmailType ON UserAccountEmailType.UserAccountID = UserAccount.ID JOIN " +
                    $"    UserAccountAssetGroup ON UserAccountAssetGroup.UserAccountID = UserAccount.ID LEFT OUTER JOIN " +
                    $"    MeterAssetGroup ON " +
                    $"        MeterAssetGroup.AssetGroupID = UserAccountAssetGroup.AssetGroupID AND " +
                    $"        MeterAssetGroup.MeterID IN ({meterIDList}) LEFT OUTER JOIN " +
                    $"    LineAssetGroup ON " +
                    $"        LineAssetGroup.AssetGroupID = UserAccountAssetGroup.AssetGroupID AND " +
                    $"        LineAssetGroup.LineID IN ({lineIDList}) " +
                    $"WHERE " +
                    $"    UserAccountEmailType.EmailTypeID = {{0}} AND " +
                    $"    UserAccount.{addressField}Confirmed <> 0 AND " +
                    $"    UserAccount.Approved <> 0 AND " +
                    $"    UserAccountAssetGroup.Email <> 0 AND " +
                    $"    ( " +
                    $"        MeterAssetGroup.ID IS NOT NULL OR " +
                    $"        LineAssetGroup.ID IS NOT NULL " +
                    $"    )";

                using (DataTable emailAddressTable = connection.RetrieveData(emailAddressQuery, emailType.ID))
                {
                    return emailAddressTable
                        .Select()
                        .Select(row => row.ConvertField<string>("Email"))
                        .ToList();
                }
            }
        }

        public List<string> GetAllRecipients(EmailType emailType)
        {
            return EmailService.GetRecipients(emailType);
        }

        public XDocument ApplyTemplate(EmailType emailType, string templateData)
        {
            XDocument htmlDocument = EmailService.ApplyTemplate(emailType, templateData);
            TransformEventDataElements(htmlDocument);
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

        public int LoadSentEmail(DateTime now, List<string> recipients, XDocument htmlDocument, List<int> eventIDs)
        {
            int sentEmailID = EmailService.LoadSentEmail(now, recipients, htmlDocument);
            LoadEventSentEmail(sentEmailID, eventIDs);
            return sentEmailID;
        }

        public void SendEmail(List<string> recipients, XDocument htmlDocument, List<Attachment> attachments) =>
            EmailService.SendEmail(recipients, htmlDocument, attachments);

        public void SendAdminEmail(string subject, string message, List<string> replyTo) =>
            EmailService.SendAdminEmail(subject, message, replyTo);

        private void TransformEventDataElements(XDocument htmlDocument)
        {
            using (AdoDataConnection connection = ConnectionFactory())
            {
                htmlDocument.TransformAll("pqi", (element, index) => PQIGenerator.GetPqiInformation(connection, element));
                htmlDocument.TransformAll("structure", (element, index) => StructureLocationGenerator.GetStructureLocationInformation(element));
                htmlDocument.TransformAll("lightning", (element, index) => LightningGenerator.GetLightningInfo(connection, element));
                htmlDocument.TransformAll("faultType", (element, index) => FaultTypeGenerator.GetFaultType(element));
            }
        }

        private void LoadEventSentEmail(int sentEmailID, List<int> eventIDs)
        {
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

        #endregion
    }
}
