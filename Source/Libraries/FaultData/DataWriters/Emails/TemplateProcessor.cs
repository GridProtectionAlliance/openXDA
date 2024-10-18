//******************************************************************************************************
//  TemplateProcessor.cs - Gbtc
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
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Xml.Linq;
using GSF.Data;
using GSF.Xml;
using openXDA.Model;

namespace FaultData.DataWriters.Emails
{
    public class TemplateProcessor
    {
        private Func<AdoDataConnection> ConnectionFactory { get; }

        public TemplateProcessor(Func<AdoDataConnection> connectionFactory) =>
            ConnectionFactory = connectionFactory;

        public XDocument ApplyTemplate(EmailTypeBase emailType, string templateData)
        {
            string htmlText = templateData.ApplyXSLTransform(emailType.Template);

            XDocument htmlDocument = XDocument.Parse(htmlText, LoadOptions.PreserveWhitespace);

            htmlDocument.TransformAll("format", element => {
                try { return element.Format(); }
                catch { return string.Empty; }
            });

            return htmlDocument;
        }

        public void ApplyChartTransform(List<Attachment> attachments, XDocument htmlDocument, int minSamplesPerCycle = -1)
        {
            using (AdoDataConnection connection = ConnectionFactory())
            {
                htmlDocument.TransformAll("chart", (element, index) =>
                {
                    string chartEventID = (string)element.Attribute("eventID") ?? "-1";
                    string cid = $"event{chartEventID}_chart{index:00}.png";

                    string stringMinimum = (string)element.Attribute("minimumSamplesPerCycleOverride");
                    int passedMinimum = minSamplesPerCycle;
                    if (!(stringMinimum is null) && !int.TryParse(stringMinimum, out passedMinimum))
                        passedMinimum = -1;

                    Stream image = ChartGenerator.ConvertToChartImageStream(connection, element, passedMinimum);
                    Attachment attachment = new Attachment(image, cid);
                    attachment.ContentId = attachment.Name;
                    attachments.Add(attachment);

                    return new XElement("img", new XAttribute("src", $"cid:{cid}"));
                });
            }
        }

        public void ApplyImageEmbedTransform(List<Attachment> attachments, XDocument htmlDocument)
        {
            using (AdoDataConnection connection = ConnectionFactory())
            {
                htmlDocument.TransformAll("embed", (element, index) =>
                {
                    string cid = $"image{index:00}.jpg";

                    try
                    {
                        string base64 = (string)element;
                        byte[] imageData = Convert.FromBase64String(base64);
                        MemoryStream stream = new MemoryStream(imageData);
                        Attachment attachment = new Attachment(stream, cid);
                        attachment.ContentId = attachment.Name;
                        attachments.Add(attachment);
                        return new XElement("img", new XAttribute("src", $"cid:{cid}"));
                    }
                    catch (Exception ex)
                    {
                        string text = new StringBuilder()
                            .AppendLine($"Error while loading {cid}:")
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
        }
    }
}
