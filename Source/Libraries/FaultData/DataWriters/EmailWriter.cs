//******************************************************************************************************
//  HTMLWriter.cs - Gbtc
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
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Windows.Forms.DataVisualization.Charting;
using FaultData.DataAnalysis;
using FaultData.Database;
using FaultData.Database.FaultLocationDataTableAdapters;
using FaultData.Database.MeterDataTableAdapters;
using GSF;
using CycleDataTableAdapter = FaultData.Database.MeterDataTableAdapters.CycleDataTableAdapter;
using DataPoint = FaultData.DataAnalysis.DataPoint;
using Series = System.Windows.Forms.DataVisualization.Charting.Series;

namespace FaultData.DataWriters
{
    /// <summary>
    /// Writes the results of fault analysis to an email.
    /// </summary>
    public class EmailWriter
    {
        #region [ Members ]

        // Nested Types
        private class FaultRecordInfo
        {
            public Meter Meter;
            public Line Line;

            public MeterData.EventRow Event;
            public List<FaultLocationData.FaultCurveRow> FaultCurves;
            public List<FaultLocationData.FaultSummaryRow> FaultSummaries;

            public List<Recipient> Recipients;
        }

        private class NamedDataSeries
        {
            public string Name;
            public DataSeries Series;
        }

        // Fields
        private string m_connectionString;

        private string m_smtpServer;
        private string m_fromAddress;
        private string m_pqDashboardURL;
        private double m_maxFaultDistanceMultiplier;
        private double m_minFaultDistanceMultiplier;
        private string m_lengthUnits;

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Creates a new instance of the <see cref="XMLWriter"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string used to connect to the database containing fault location data.</param>
        public EmailWriter(string connectionString)
        {
            m_connectionString = connectionString;
        }

        #endregion

        #region [ Properties ]

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

        public string PQDashboardURL
        {
            get
            {
                return m_pqDashboardURL;
            }
            set
            {
                m_pqDashboardURL = value;
            }
        }

        public double MaxFaultDistanceMultiplier
        {
            get
            {
                return m_maxFaultDistanceMultiplier;
            }
            set
            {
                m_maxFaultDistanceMultiplier = value;
            }
        }

        public double MinFaultDistanceMultiplier
        {
            get
            {
                return m_minFaultDistanceMultiplier;
            }
            set
            {
                m_minFaultDistanceMultiplier = value;
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

        public void WriteResults(int eventID)
        {
            FaultRecordInfo faultRecordInfo = new FaultRecordInfo();

            using (SystemInfoDataContext systemInfo = new SystemInfoDataContext(m_connectionString))
            using (MeterInfoDataContext meterInfo = new MeterInfoDataContext(m_connectionString))
            using (EventTableAdapter eventAdapter = new EventTableAdapter())
            using (CycleDataTableAdapter cycleDataAdapter = new CycleDataTableAdapter())
            using (FaultCurveTableAdapter faultCurveAdapter = new FaultCurveTableAdapter())
            using (FaultSummaryTableAdapter faultSummaryAdapter = new FaultSummaryTableAdapter())
            {
                faultRecordInfo.Recipients = systemInfo.Recipients.ToList();

                if (faultRecordInfo.Recipients.Count > 0)
                {
                    eventAdapter.Connection.ConnectionString = m_connectionString;
                    cycleDataAdapter.Connection.ConnectionString = m_connectionString;
                    faultCurveAdapter.Connection.ConnectionString = m_connectionString;
                    faultSummaryAdapter.Connection.ConnectionString = m_connectionString;

                    faultRecordInfo.Event = eventAdapter.GetDataByID(eventID).First();

                    faultRecordInfo.Meter = meterInfo.Meters.Single(m => faultRecordInfo.Event.MeterID == m.ID);
                    faultRecordInfo.Line = meterInfo.Lines.Single(l => faultRecordInfo.Event.LineID == l.ID);

                    faultRecordInfo.FaultCurves = faultCurveAdapter.GetDataBy(eventID).ToList();
                    faultRecordInfo.FaultSummaries = faultSummaryAdapter.GetDataBy(eventID).ToList();

                    WriteResults(faultRecordInfo);
                }
            }
        }

        private void WriteResults(FaultRecordInfo faultRecordInfo)
        {
            string template;
            string subject;
            string body;

            List<FaultLocationData.FaultSummaryRow> faultSummaries;
            int faultCount;

            Lazy<string> faultCurvesHTML;
            List<NamedDataSeries> faultCurves;
            Attachment faultCurveAttachment;

            template = null;

            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("FaultData.DataWriters.FaultResultsTemplate.html"))
            {
                if ((object)stream != null)
                {
                    using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                    {
                        template = reader.ReadToEnd();
                    }
                }
            }

            if ((object)template != null)
            {
                faultSummaries = faultRecordInfo.FaultSummaries.Where(faultSummary => IsValid(faultSummary, faultRecordInfo.Line.Length)).ToList();
                faultCount = faultSummaries.Select(faultSummary => faultSummary.FaultNumber).Distinct().Count();

                if (faultCount > 0)
                {
                    subject = string.Format("Fault detected by openXDA  Line: {0}  Time: {1:MM/dd/yyyy HH:mm}", faultRecordInfo.Line.Name, ToDataSeries(faultRecordInfo.FaultCurves[0]).Series[0].Time);
                    body = template;

                    faultCurvesHTML = new Lazy<string>(() => "<img src=\"cid:FaultCurves.png\" width=\"650\" height=\"340\" />");

                    body = Replace(body, "{eventID}", faultRecordInfo.Event.ID.ToString());
                    body = Replace(body, "{meter}", faultRecordInfo.Meter.Name);
                    body = Replace(body, "{station}", faultRecordInfo.Meter.MeterLocation.Name);
                    body = Replace(body, "{line}", faultRecordInfo.Line.Name);
                    body = Replace(body, "{nFaults}", string.Format("{0} fault{1}", faultCount, (faultCount > 1) ? "s" : ""));
                    body = Replace(body, "{faultSummaries}", () => ToHTML(faultSummaries));
                    body = Replace(body, "{distanceSummaryTable}", () => string.Format("<table>{0}{1}</table>", GetTableHeader(), GetTableRows(faultRecordInfo.FaultSummaries, faultRecordInfo.Line)));
                    body = Replace(body, "{faultCurves}", () => faultCurvesHTML.Value);
                    body = Replace(body, "{PQDashboardURL}", m_pqDashboardURL);

                    if (faultCurvesHTML.IsValueCreated)
                    {
                        faultCurves = GetValidFaultCurves(faultSummaries, faultRecordInfo.FaultCurves);

                        foreach (NamedDataSeries faultCurve in faultCurves)
                            FixFaultCurve(faultCurve.Series, faultRecordInfo.Line);

                        faultCurveAttachment = new Attachment(ToImageStream(faultCurves), "FaultCurves.png");
                        faultCurveAttachment.ContentId = "FaultCurves.png";
                        SendEmail(faultRecordInfo.Recipients, subject, body, faultCurveAttachment);
                    }
                    else
                    {
                        SendEmail(faultRecordInfo.Recipients, subject, body);
                    }
                }
            }
        }

        private List<NamedDataSeries> GetValidFaultCurves(List<FaultLocationData.FaultSummaryRow> faultSummaries, List<FaultLocationData.FaultCurveRow> faultCurveRows)
        {
            HashSet<string> validAlgorithms = new HashSet<string>(faultSummaries.Select(faultSummary => faultSummary.Algorithm));

            return faultCurveRows
                .Where(faultCurveRow => validAlgorithms.Contains(faultCurveRow.Algorithm))
                .Select(ToDataSeries)
                .ToList();
        }

        private string ToHTML(List<FaultLocationData.FaultSummaryRow> faultSummaries)
        {
            string html = "";

            IEnumerable<FaultLocationData.FaultSummaryRow> faults = faultSummaries
                .GroupBy(faultSummary => faultSummary.FaultNumber)
                .Select(grouping => grouping.OrderBy(faultSummary => faultSummary.LargestCurrentDistance).ToList())
                .Select(list => list[list.Count / 2])
                .OrderBy(faultSummary => faultSummary.FaultNumber);

            foreach (FaultLocationData.FaultSummaryRow fault in faults)
            {
                html += string.Format("<p>" +
                                      "<span style=\"font-size: 20px; font-weight: bold; text-decoration: underline\">Fault {0}</span><br />" +
                                      "<span style=\"font-weight: bold\">Fault Type:</span> {1}<br />" +
                                      "<span style=\"font-weight: bold\">Inception:</span> {2:ss.ffffff} seconds (since {2:MM/dd/yyyy HH:mm})<br />" +
                                      "<span style=\"font-weight: bold\">Duration:</span> {3:0.000} milliseconds ({4:0.00} cycles)<br />" +
                                      "<span style=\"font-weight: bold\">Distance:</span> {5:0.000} {6} ({7})<br />" +
                                      "</p>", fault.FaultNumber, fault.FaultType, fault.Inception, fault.DurationSeconds * 1000,
                                      fault.DurationCycles, fault.LargestCurrentDistance, LengthUnits, fault.Algorithm);
            }

            return html;
        }

        private string GetTableHeader()
        {
            string header = string.Format("<tr><th colspan=\"3\"></th><th colspan=\"5\">Fault Distances ({0})</th></tr>", LengthUnits);

            header += string.Format("<tr><th>{0}</th><th>{1}</th><th>{2}</th><th>{3}</th><th>{4}</th><th>{5}</th><th>{6}</th><th>{7}</th></tr>",
                "Fault Number",
                "Algorithm",
                "Valid",
                "At Largest Current Cycle",
                "Maximum",
                "Minimum",
                "Average",
                "Standard Deviation");

            return header;
        }

        private string GetTableRows(List<FaultLocationData.FaultSummaryRow> faultSummaryRows, Line line)
        {
            IEnumerable<string> rows = faultSummaryRows
                .OrderBy(faultSummaryRow => faultSummaryRow.FaultNumber)
                .ThenBy(faultSummaryRow => faultSummaryRow.ID)
                .Select(faultSummaryRow => ToTableRow(faultSummaryRow, line));

            return string.Concat(rows);
        }

        private string ToTableRow(FaultLocationData.FaultSummaryRow faultSummaryRow, Line line)
        {
            return string.Format("<tr><td style=\"text-align: center\">{0}</td><td style=\"text-align: left\">{1}</td><td style=\"text-align: center\">{2}</td><td>{3:0.000}</td><td>{4:0.000}</td><td>{5:0.000}</td><td>{6:0.000}</td><td>{7:0.000}</td></tr>",
                faultSummaryRow.FaultNumber,
                faultSummaryRow.Algorithm,
                IsValid(faultSummaryRow, line.Length) ? "Yes" : "No",
                faultSummaryRow.LargestCurrentDistance,
                faultSummaryRow.MaximumDistance,
                faultSummaryRow.MinimumDistance,
                faultSummaryRow.AverageDistance,
                faultSummaryRow.DistanceDeviation);
        }

        private Stream ToImageStream(List<NamedDataSeries> faultCurves)
        {
            ChartArea area;
            Series series;

            DateTime startTime;
            DateTime endTime;
            double seconds;

            double maxSeconds;
            double minSeconds;

            using (Chart chart = new Chart())
            {
                startTime = faultCurves[0].Series[0].Time;
                endTime = faultCurves[0].Series.DataPoints.Last().Time;

                // Align startTime with top of the minute
                startTime = startTime.AddTicks(-(startTime.Ticks % Ticks.PerMinute));

                area = new ChartArea();
                area.AxisX.Title = string.Format("Seconds since {0:MM/dd/yyyy HH:mm}", startTime);
                area.AxisX.TitleFont = new Font(area.AxisX.TitleFont.FontFamily, 35.0F);
                area.AxisX.MajorGrid.Enabled = false;
                area.AxisX.LabelStyle.Format = "0.000";
                area.AxisX.LabelAutoFitMinFontSize = 35;
                area.AxisY.Title = string.Format("Distance ({0})", m_lengthUnits);
                area.AxisY.TitleFont = new Font(area.AxisY.TitleFont.FontFamily, 35.0F);
                area.AxisY.LabelAutoFitMinFontSize = 35;

                chart.Titles.Add("Calculated Fault Distance by Time of Calculation");
                chart.Titles[0].Font = new Font(chart.Titles[0].Font.FontFamily, 35.0F, FontStyle.Bold);
                chart.Legends.Add(new Legend());
                chart.Legends[0].Font = new Font(chart.Legends[0].Font.FontFamily, 35.0F, FontStyle.Regular);
                chart.Width = 2600;
                chart.Height = 1360;
                chart.ChartAreas.Add(area);

                maxSeconds = 0.0D;
                minSeconds = (endTime - startTime).TotalSeconds;

                foreach (NamedDataSeries dataSeries in faultCurves)
                {
                    series = new Series(dataSeries.Name);
                    series.ChartType = SeriesChartType.FastLine;
                    series.BorderWidth = 5;

                    foreach (DataPoint dataPoint in dataSeries.Series.DataPoints)
                    {
                        seconds = (dataPoint.Time - startTime).TotalSeconds;
                        series.Points.AddXY(seconds, dataPoint.Value);

                        if (dataPoint.Value != 0.0D)
                        {
                            if (seconds > maxSeconds)
                                maxSeconds = seconds;

                            if (seconds < minSeconds)
                                minSeconds = seconds;
                        }
                    }

                    chart.Series.Add(series);
                }

                if (maxSeconds < minSeconds)
                {
                    double temp = maxSeconds;
                    maxSeconds = minSeconds;
                    minSeconds = temp;
                }

                area.AxisX.Maximum = maxSeconds;
                area.AxisX.Minimum = minSeconds;

                return ChartToImage(chart);
            }
        }

        private Stream ChartToImage(Chart chart)
        {
            MemoryStream stream = new MemoryStream();
            chart.SaveImage(stream, ChartImageFormat.Bmp);
            stream.Position = 0;
            return BitmapToJpg(stream);
        }

        private Stream BitmapToJpg(Stream bitmapStream)
        {
            MemoryStream jpgStream = new MemoryStream();
            Bitmap bitmap = new Bitmap(bitmapStream);

            ImageCodecInfo encoder = ImageCodecInfo.GetImageEncoders()
                .SingleOrDefault(enc => enc.FormatID == ImageFormat.Jpeg.Guid);

            EncoderParameters parameters = new EncoderParameters(1);

            if ((object)encoder == null)
                throw new InvalidOperationException("Unable to convert bitmap to jpg.");

            parameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 500L);
            bitmap.Save(jpgStream, encoder, parameters);
            jpgStream.Position = 0;

            return jpgStream;
        }

        private void SendEmail(List<Recipient> recipients, string subject, string body, params Attachment[] attachments)
        {
            const int DefaultSMTPPort = 25;

            string[] smtpServerParts = m_smtpServer.Split(':');
            string host = smtpServerParts[0];
            int port;

            if (smtpServerParts.Length <= 1 || !int.TryParse(smtpServerParts[1], out port))
                port = DefaultSMTPPort;

            using (SmtpClient smtpClient = new SmtpClient(host, port))
            using (MailMessage emailMessage = new MailMessage())
            {
                emailMessage.From = new MailAddress(m_fromAddress);
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

        private NamedDataSeries ToDataSeries(FaultLocationData.FaultCurveRow faultCurve)
        {
            DataGroup dataGroup = new DataGroup();

            dataGroup.FromData(faultCurve.Data);

            return new NamedDataSeries()
            {
                Name = faultCurve.Algorithm,
                Series = dataGroup[0]
            };
        }

        private void FixFaultCurve(DataSeries faultCurve, Line line)
        {
            double maxFaultDistance = MaxFaultDistanceMultiplier * line.Length;
            double minFaultDistance = MinFaultDistanceMultiplier * line.Length;

            foreach (DataPoint dataPoint in faultCurve.DataPoints)
            {
                if (double.IsNaN(dataPoint.Value))
                    dataPoint.Value = 0.0D;
                else if (dataPoint.Value > maxFaultDistance)
                    dataPoint.Value = maxFaultDistance;
                else if (dataPoint.Value < minFaultDistance)
                    dataPoint.Value = minFaultDistance;
            }
        }

        private string Replace(string str, string oldValue, string newValue)
        {
            return str.Replace(oldValue, newValue);
        }

        private string Replace(string str, string oldValue, Func<string> newValueFactory)
        {
            Lazy<string> newValue = new Lazy<string>(newValueFactory);
            StringBuilder builder = new StringBuilder();
            int offset = 0;
            int index;

            while (offset < str.Length)
            {
                index = str.IndexOf(oldValue, offset, StringComparison.Ordinal);

                if (index >= 0)
                {
                    builder.Append(str.Substring(offset, index));
                    builder.Append(newValue.Value);
                    offset = index + oldValue.Length;
                }
                else
                {
                    builder.Append(str.Substring(offset, str.Length - offset));
                    offset = str.Length;
                }
            }

            return builder.ToString();
        }

        private bool IsValid(FaultLocationData.FaultSummaryRow faultSummary, double lineLength)
        {
            return (faultSummary.DistanceDeviation < 0.5D * lineLength)
                && (faultSummary.LargestCurrentDistance >= MinFaultDistanceMultiplier * lineLength)
                && (faultSummary.LargestCurrentDistance <= MaxFaultDistanceMultiplier * lineLength);
        }

        #endregion
    }
}
