//******************************************************************************************************
//  EmailWriter.cs - Gbtc
//
//  Copyright © 2014, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the Eclipse Public License -v 1.0 (the "License"); you may
//  not use this file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://www.opensource.org/licenses/eclipse-1.0.php
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  11/14/2014 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading;
using System.Windows.Forms.DataVisualization.Charting;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Xsl;
using FaultData.Database;
using FaultData.Database.FaultLocationDataTableAdapters;
using FaultData.Database.MeterDataTableAdapters;
using FaultData.DataSets;
using GSF.Collections;
using log4net;
using Series = System.Windows.Forms.DataVisualization.Charting.Series;

namespace FaultData.DataWriters
{
    /// <summary>
    /// Writes the results of fault analysis to an email.
    /// </summary>
    public class EmailWriter : IDataWriter
    {
        #region [ Members ]

        // Fields
        private string m_dbConnectionString;
        private string m_smtpServer;
        private string m_fromAddress;
        private string m_emailTemplate;
        private double m_timeTolerance;
        private double m_waitPeriod;
        private string m_xdaTimeZone;
        private string m_lengthUnits;

        #endregion

        #region [ Properties ]

        public string DbConnectionString
        {
            get
            {
                return m_dbConnectionString;
            }
            set
            {
                m_dbConnectionString = value;
            }
        }

        public string SMTPServer
        {
            get
            {
                return m_smtpServer;
            }
            set
            {
                m_smtpServer = value;
            }
        }

        public string FromAddress
        {
            get
            {
                return m_fromAddress;
            }
            set
            {
                m_fromAddress = value;
            }
        }

        public string EmailTemplate
        {
            get
            {
                return m_emailTemplate;
            }
            set
            {
                m_emailTemplate = value;
            }
        }

        public double TimeTolerance
        {
            get
            {
                return m_timeTolerance;
            }
            set
            {
                m_timeTolerance = value;
            }
        }

        public double WaitPeriod
        {
            get
            {
                return m_waitPeriod;
            }
            set
            {
                m_waitPeriod = value;
            }
        }

        public string XDATimeZone
        {
            get
            {
                return m_xdaTimeZone;
            }
            set
            {
                m_xdaTimeZone = value;
            }
        }

        public string LengthUnits
        {
            get
            {
                return m_lengthUnits;
            }
            set
            {
                m_lengthUnits = value;
            }
        }

        #endregion

        #region [ Methods ]

        public void WriteResults(DbAdapterContainer dbAdapterContainer, MeterDataSet meterDataSet)
        {
            Initialize(this);

            foreach (FaultLocationData.FaultSummaryRow faultSummary in dbAdapterContainer.GetAdapter<FaultSummaryTableAdapter>().GetDataByFileGroup(meterDataSet.FileGroup.ID))
            {
                if (faultSummary.IsSelectedAlgorithm != 0 && faultSummary.IsSuppressed == 0)
                {
                    if (dbAdapterContainer.GetAdapter<EventFaultEmailTableAdapter>().GetFaultEmailCount(faultSummary.EventID) == 0)
                        QueueEventID(faultSummary.EventID);
                }
            }
        }

        #endregion

        #region [ Static ]

        // Static Fields
        private static readonly HashSet<int> QueuedEventIDs;
        private static readonly ProcessQueue<Action> ProcessQueue;

        private static string s_smtpServer;
        private static string s_fromAddress;
        private static string s_emailTemplate;
        private static double s_timeTolerance;
        private static TimeSpan s_waitPeriod;
        private static TimeZoneInfo s_timeZone;
        private static DbAdapterContainer s_dbAdapterContainer;

        private static readonly ILog Log = LogManager.GetLogger(typeof(EmailWriter));

        // Static Constructor
        static EmailWriter()
        {
            QueuedEventIDs = new HashSet<int>();
            ProcessQueue = ProcessQueue<Action>.CreateRealTimeQueue(action => action());
            ProcessQueue.ProcessException += (sender, args) => Log.Error(args.Argument.Message, args.Argument);
            ProcessQueue.Start();
        }

        // Static Methods
        private static void Initialize(EmailWriter writer)
        {
            bool configurationChanged =
                s_timeTolerance != writer.TimeTolerance ||
                s_smtpServer != writer.SMTPServer ||
                s_fromAddress != writer.FromAddress ||
                s_emailTemplate != writer.EmailTemplate ||
                s_waitPeriod != TimeSpan.FromSeconds(writer.WaitPeriod) ||
                s_timeZone.Id != writer.XDATimeZone;


            if (configurationChanged)
            {
                ProcessQueue.Add(() =>
                {
                    s_timeTolerance = writer.TimeTolerance;
                    s_smtpServer = writer.SMTPServer;
                    s_fromAddress = writer.FromAddress;
                    s_emailTemplate = writer.EmailTemplate;
                    s_waitPeriod = TimeSpan.FromSeconds(writer.WaitPeriod);
                    s_timeZone = TimeZoneInfo.FindSystemTimeZoneById(writer.XDATimeZone);
                });
            }

            if ((object)s_dbAdapterContainer == null)
            {
                ProcessQueue.Add(() =>
                {
                    if ((object)s_dbAdapterContainer == null)
                        s_dbAdapterContainer = new DbAdapterContainer(writer.DbConnectionString);
                });
            }
        }

        private static void QueueEventID(int eventID)
        {
            if (string.IsNullOrEmpty(s_smtpServer))
                return;

            ProcessQueue.Add(() =>
            {
                ManualResetEvent waitHandle;
                WaitOrTimerCallback callback;

                waitHandle = new ManualResetEvent(false);

                callback = (state, timedOut) =>
                {
                    waitHandle.Dispose();
                    DequeueEventID(eventID);
                };

                QueuedEventIDs.Add(eventID);

                ThreadPool.RegisterWaitForSingleObject(waitHandle, callback, null, s_waitPeriod, true);
            });
        }

        private static void DequeueEventID(int eventID)
        {
            ProcessQueue.Add(() =>
            {
                MeterData.EventRow eventRow;
                MeterData.EventDataTable systemEvent;

                QueuedEventIDs.Remove(eventID);

                eventRow = s_dbAdapterContainer.GetAdapter<EventTableAdapter>().GetDataByID(eventID)[0];
                systemEvent = s_dbAdapterContainer.GetAdapter<EventTableAdapter>().GetSystemEvent(eventRow.StartTime, eventRow.EndTime, s_timeTolerance);

                if (systemEvent.Any(evt => evt.LineID == eventRow.LineID && QueuedEventIDs.Contains(evt.ID)))
                    return;

                if (s_dbAdapterContainer.GetAdapter<EventFaultEmailTableAdapter>().GetFaultEmailCount(eventID) == 0)
                    GenerateEmail(eventID);
            });
        }

        private static void GenerateEmail(int eventID)
        {
            Dictionary<int, ChartGenerator> generators;

            string eventDetail;
            XslCompiledTransform transform;
            XDocument htmlDocument;
            List<XElement> formatParents;
            List<XElement> chartElements;
            List<XElement> chartParents;

            List<Recipient> recipients;
            Attachment[] attachments;
            string subject;
            string html;

            eventDetail = s_dbAdapterContainer.GetAdapter<EventTableAdapter>().GetEventDetail(eventID);

            using (StringReader templateReader = new StringReader(s_emailTemplate))
            using (StringReader dataReader = new StringReader(eventDetail))
            using (XmlReader xmlTemplateReader = XmlReader.Create(templateReader))
            using (XmlReader xmlDataReader = XmlReader.Create(dataReader))
            using (StringWriter transformWriter = new StringWriter())
            {
                transform = new XslCompiledTransform();
                transform.Load(xmlTemplateReader);
                transform.Transform(xmlDataReader, null, transformWriter);
                htmlDocument = XDocument.Parse(transformWriter.ToString(), LoadOptions.PreserveWhitespace);
            }

            formatParents = htmlDocument
                .Descendants("format")
                .Select(element => element.Parent)
                .Distinct()
                .ToList();

            foreach (XElement parent in formatParents)
                parent.ReplaceNodes(parent.Nodes().Select(Format));

            chartElements = htmlDocument
                .Descendants("chart")
                .ToList();

            for (int i = 0; i < chartElements.Count; i++)
                chartElements[i].SetAttributeValue("cid", string.Format("chart{0:00}.png", i));

            chartParents = chartElements
                .Select(element => element.Parent)
                .Distinct()
                .ToList();

            generators = new Dictionary<int, ChartGenerator>();

            foreach (XElement parent in chartParents)
                parent.ReplaceNodes(parent.Nodes().Select(ToImageElement));

            recipients = s_dbAdapterContainer.GetAdapter<SystemInfoDataContext>().Recipients.ToList();
            subject = (string)htmlDocument.Descendants("title").FirstOrDefault() ?? "Fault detected by openXDA";
            html = htmlDocument.ToString(SaveOptions.DisableFormatting).Replace("&amp;", "&");
            attachments = null;

            try
            {
                attachments = chartElements
                    .Select(element =>
                    {
                        int chartEvent = Convert.ToInt32((string)element.Attribute("eventID"));
                        ChartGenerator generator = generators.GetOrAdd(chartEvent, id => new ChartGenerator(s_dbAdapterContainer, id));

                        Stream image;
                        Attachment attachment;

                        using (Chart chart = GenerateChart(generator, element))
                        {
                            image = ConvertToImage(chart, ChartImageFormat.Png);
                            attachment = new Attachment(image, (string)element.Attribute("cid"));
                            attachment.ContentId = attachment.Name;
                            return attachment;
                        }
                    })
                    .ToArray();

                SendEmail(recipients, subject, html, attachments);
                LoadEmail(eventID, recipients, subject, html);
            }
            finally
            {
                if ((object)attachments != null)
                {
                    foreach (Attachment attachment in attachments)
                        attachment.Dispose();
                }
            }
        }

        private static void SendEmail(List<Recipient> recipients, string subject, string body, params Attachment[] attachments)
        {
            const int DefaultSMTPPort = 25;

            string[] smtpServerParts = s_smtpServer.Split(':');
            string host = smtpServerParts[0];
            int port;

            if (smtpServerParts.Length <= 1 || !int.TryParse(smtpServerParts[1], out port))
                port = DefaultSMTPPort;

            using (SmtpClient smtpClient = new SmtpClient(host, port))
            using (MailMessage emailMessage = new MailMessage())
            {
                emailMessage.From = new MailAddress(s_fromAddress);
                emailMessage.Subject = subject;
                emailMessage.Body = body;
                emailMessage.IsBodyHtml = true;

                // Add the specified To recipients for the email message
                foreach (string toRecipient in recipients.Select(recipient => recipient.Email))
                    emailMessage.To.Add(toRecipient.Trim());

                // Create the image attachment for the email message
                foreach (Attachment attachment in attachments)
                    emailMessage.Attachments.Add(attachment);

                // Send the email
                smtpClient.Send(emailMessage);
            }
        }

        private static void LoadEmail(int eventID, List<Recipient> recipients, string subject, string body)
        {
            EventTableAdapter eventAdapter = s_dbAdapterContainer.GetAdapter<EventTableAdapter>();
            MeterData.EventRow eventRow = eventAdapter.GetDataByID(eventID)[0];
            MeterData.EventDataTable systemEvent = eventAdapter.GetSystemEvent(eventRow.StartTime, eventRow.EndTime, s_timeTolerance);

            FaultEmailTableAdapter faultEmailAdapter = s_dbAdapterContainer.GetAdapter<FaultEmailTableAdapter>();
            FaultLocationData.FaultEmailDataTable faultEmailTable = new FaultLocationData.FaultEmailDataTable();
            FaultLocationData.EventFaultEmailDataTable eventFaultEmailTable = new FaultLocationData.EventFaultEmailDataTable();

            DateTime now = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, s_timeZone);
            string toLine = string.Join("; ", recipients.Select(recipient => recipient.Email));

            BulkLoader bulkLoader;

            faultEmailTable.AddFaultEmailRow(now, toLine, subject, body);
            faultEmailAdapter.Update(faultEmailTable);

            foreach (MeterData.EventRow evt in systemEvent)
            {
                if (eventRow.LineID == evt.LineID)
                    eventFaultEmailTable.AddEventFaultEmailRow(evt.ID, faultEmailTable[0].ID);
            }

            bulkLoader = new BulkLoader();
            bulkLoader.Connection = s_dbAdapterContainer.Connection;
            bulkLoader.CommandTimeout = s_dbAdapterContainer.CommandTimeout;
            bulkLoader.Load(eventFaultEmailTable);
        }

        private static object Format(XNode node)
        {
            XElement element;
            IFormattable formattable;

            Type elementType;
            string formatString;
            string value;

            element = node as XElement;

            if ((object)element != null)
            {
                if (element.Name.ToString().Equals("format", StringComparison.OrdinalIgnoreCase))
                {
                    elementType = Type.GetType((string)element.Attribute("type"), false);
                    formatString = (string)element.Attribute("spec");
                    value = (string)element;
                    formattable = (IFormattable)Convert.ChangeType(value, elementType);
                    return new XText(formattable.ToString(formatString, null));
                }
            }

            return node;
        }

        private static XNode ToImageElement(XNode node)
        {
            XElement element;
            string cid;

            element = node as XElement;

            if ((object)element == null || element.Name != "chart")
                return node;

            cid = string.Concat("cid:", (string)element.Attribute("cid"));

            return new XElement("img", new XAttribute("src", cid));
        }

        private static Chart GenerateChart(ChartGenerator generator, XElement chartElement)
        {
            Chart chart;

            FaultSummaryTableAdapter faultSummaryAdapter;
            FaultLocationData.FaultSummaryDataTable faultSummaries;
            FaultLocationData.FaultSummaryRow faultSummary;

            int width;
            int height;
            double prefaultCycles;
            double postfaultCycles;

            string title;
            List<string> keys;
            List<string> names;
            DateTime startTime;
            DateTime endTime;

            int faultID;

            faultSummaryAdapter = s_dbAdapterContainer.GetAdapter<FaultSummaryTableAdapter>();
            faultSummaries = faultSummaryAdapter.GetDataBy(generator.EventID);
            faultID = Convert.ToInt32((string)chartElement.Attribute("faultID"));

            faultSummary = faultSummaries
                .Where(row => row.ID == faultID)
                .FirstOrDefault(row => row.IsSelectedAlgorithm != 0);

            if ((object)faultSummary == null)
                return null;

            prefaultCycles = Convert.ToDouble((string)chartElement.Attribute("prefaultCycles"));
            postfaultCycles = Convert.ToDouble((string)chartElement.Attribute("postfaultCycles"));

            title = (string)chartElement.Attribute("yAxisTitle");
            keys = GetKeys(chartElement);
            names = GetNames(chartElement);
            startTime = faultSummary.Inception.AddSeconds(-prefaultCycles / 60.0D);
            endTime = faultSummary.Inception.AddSeconds(faultSummary.DurationSeconds).AddSeconds(postfaultCycles / 60.0D);
            chart = generator.GenerateChart(title, keys, names, startTime, endTime);

            width = Convert.ToInt32((string)chartElement.Attribute("width"));
            height = Convert.ToInt32((string)chartElement.Attribute("height"));
            SetChartSize(chart, width, height);

            if ((object)chartElement.Attribute("yAxisMaximum") != null)
                chart.ChartAreas[0].AxisY.Maximum = Convert.ToDouble((string)chartElement.Attribute("yAxisMaximum"));

            if ((object)chartElement.Attribute("yAxisMinimum") != null)
                chart.ChartAreas[0].AxisY.Minimum = Convert.ToDouble((string)chartElement.Attribute("yAxisMinimum"));

            if (string.Equals((string)chartElement.Attribute("highlightCalculation"), "index", StringComparison.OrdinalIgnoreCase))
            {
                DateTime calculationTime = generator.ToDateTime(faultSummary.CalculationCycle);
                double calculationPosition = chart.ChartAreas[0].AxisX.Minimum + (calculationTime - startTime).TotalSeconds;
                chart.ChartAreas[0].CursorX.Position = calculationPosition;
            }
            else if (string.Equals((string)chartElement.Attribute("highlightCalculation"), "cycle", StringComparison.OrdinalIgnoreCase))
            {
                DateTime calculationTime = generator.ToDateTime(faultSummary.CalculationCycle);
                double calculationPosition = chart.ChartAreas[0].AxisX.Minimum + (calculationTime - startTime).TotalSeconds;
                chart.ChartAreas[0].CursorX.SelectionStart = calculationPosition;
                chart.ChartAreas[0].CursorX.SelectionEnd = calculationPosition + 1.0D / 60.0D;
            }

            return chart;
        }

        private static List<string> GetKeys(XElement chartElement)
        {
            return chartElement
                .Elements()
                .Select(childElement => (string)childElement.Attribute("key"))
                .ToList();
        }

        private static List<string> GetNames(XElement chartElement)
        {
            return chartElement
                .Elements()
                .Select(childElement => (string)childElement)
                .ToList();
        }

        private static void SetChartSize(Chart chart, int width, int height)
        {
            int fontSize = (int)Math.Round(height / 37.0D);
            int borderWidth = (int)Math.Round(height / 480.0D);

            chart.Width = width;
            chart.Height = height;

            chart.ChartAreas[0].AxisX.LabelAutoFitMaxFontSize = fontSize;
            chart.ChartAreas[0].AxisY.LabelAutoFitMaxFontSize = fontSize;
            chart.ChartAreas[0].AxisX.LabelAutoFitMinFontSize = fontSize;
            chart.ChartAreas[0].AxisY.LabelAutoFitMinFontSize = fontSize;
            chart.ChartAreas[0].AxisX.TitleFont = new Font(chart.ChartAreas[0].AxisX.TitleFont.FontFamily, fontSize);
            chart.ChartAreas[0].AxisY.TitleFont = new Font(chart.ChartAreas[0].AxisY.TitleFont.FontFamily, fontSize);
            chart.Legends[0].Font = new Font(chart.Legends[0].Font.FontFamily, fontSize, FontStyle.Regular);

            foreach (Series series in chart.Series)
                series.BorderWidth = borderWidth;
        }

        private static Stream ConvertToImage(Chart chart, ChartImageFormat format)
        {
            MemoryStream stream = new MemoryStream();
            chart.SaveImage(stream, format);
            stream.Position = 0;
            return stream;
        }

        #endregion
    }
}
