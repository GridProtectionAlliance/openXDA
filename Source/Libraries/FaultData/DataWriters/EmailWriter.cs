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
            public FaultLocationData.FaultSummaryRow FaultSummary;

            public List<Recipient> Recipients;
        }

        // Fields
        private string m_connectionString;

        private string m_smtpServer;
        private string m_fromAddress;
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
                eventAdapter.Connection.ConnectionString = m_connectionString;
                cycleDataAdapter.Connection.ConnectionString = m_connectionString;
                faultCurveAdapter.Connection.ConnectionString = m_connectionString;
                faultSummaryAdapter.Connection.ConnectionString = m_connectionString;

                faultRecordInfo.Event = eventAdapter.GetDataByID(eventID).First();

                faultRecordInfo.Meter = meterInfo.Meters.Single(m => faultRecordInfo.Event.MeterID == m.ID);
                faultRecordInfo.Line = meterInfo.Lines.Single(l => faultRecordInfo.Event.LineID == l.ID);

                faultRecordInfo.FaultCurves = faultCurveAdapter.GetDataBy(eventID).ToList();
                faultRecordInfo.FaultSummary = faultSummaryAdapter.GetDataBy(eventID).FirstOrDefault();

                faultRecordInfo.Recipients = systemInfo.Recipients.ToList();

                WriteResults(faultRecordInfo);
            }
        }

        private void WriteResults(FaultRecordInfo faultRecordInfo)
        {
            string template;
            string subject;
            string body;
            string table;

            List<DataSeries> faultCurves;

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
                subject = string.Format("Line faults detected on {0}", faultRecordInfo.Line.Name);
                body = template;
                table = string.Concat(faultRecordInfo.FaultCurves.Select(ToTableRow));

                table += ToTableRow(faultRecordInfo.FaultSummary);

                body = body.Replace("{meter}", faultRecordInfo.Meter.Name);
                body = body.Replace("{station}", faultRecordInfo.Meter.MeterLocation.Name);
                body = body.Replace("{line}", faultRecordInfo.Line.Name);
                body = body.Replace("{distance}", faultRecordInfo.FaultSummary.MedianDistance.ToString("0.##"));
                body = body.Replace("{confidence}", faultRecordInfo.FaultSummary.DistanceDeviation.ToString("0.##"));
                body = body.Replace("{lengthUnits}", m_lengthUnits);
                body = body.Replace("{distanceSummaryTable}", table);
                body = body.Replace("{faultType}", faultRecordInfo.FaultSummary.FaultType);
                body = body.Replace("{inception}", faultRecordInfo.FaultSummary.Inception.ToString("yyyy-MM-dd HH:mm:ss.fffffff"));
                body = body.Replace("{durationSeconds}", faultRecordInfo.FaultSummary.DurationSeconds.ToString("0.####"));
                body = body.Replace("{durationCycles}", faultRecordInfo.FaultSummary.DurationCycles.ToString("0.##"));
                body = body.Replace("{faultCount}", faultRecordInfo.FaultSummary.FaultCount.ToString());

                faultCurves = faultRecordInfo.FaultCurves
                    .Select(ToDataSeries)
                    .ToList();

                foreach (DataSeries faultCurve in faultCurves)
                    FixFaultCurve(faultCurve, faultRecordInfo.Line);

                SendEmail(faultRecordInfo.Recipients, subject, body, ToImageStream(faultCurves));
            }
        }

        private string ToTableRow(FaultLocationData.FaultCurveRow faultCurveRow)
        {
            return string.Format("<tr><td>{0}</td><td>{1:0.####}</td><td>{2:0.####}</td><td>{3:0.####}</td><td>{4:0.####}</td><td>{5:0.####}</td><td>{6:0.####}</td></tr>",
                faultCurveRow.Algorithm,
                faultCurveRow.LargestCurrentDistance,
                faultCurveRow.MedianDistance,
                faultCurveRow.MaximumDistance,
                faultCurveRow.MinimumDistance,
                faultCurveRow.AverageDistance,
                faultCurveRow.DistanceDeviation);
        }

        private string ToTableRow(FaultLocationData.FaultSummaryRow faultSummaryRow)
        {
            return string.Format("<tr><td>ALL</td><td>{0:0.####}</td><td>{1:0.####}</td><td>{2:0.####}</td><td>{3:0.####}</td><td>{4:0.####}</td><td>{5:0.####}</td></tr>",
                faultSummaryRow.LargestCurrentDistance,
                faultSummaryRow.MedianDistance,
                faultSummaryRow.MaximumDistance,
                faultSummaryRow.MinimumDistance,
                faultSummaryRow.AverageDistance,
                faultSummaryRow.DistanceDeviation);
        }

        private Stream ToImageStream(List<DataSeries> faultCurves)
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
                area = new ChartArea();
                area.AxisX.Title = "Time (seconds)";
                area.AxisX.TitleFont = new Font(area.AxisX.TitleFont.FontFamily, 35.0F);
                area.AxisX.MajorGrid.Enabled = false;
                area.AxisX.LabelStyle.Format = "0.####";
                area.AxisX.LabelAutoFitMinFontSize = 35;
                area.AxisY.Title = string.Format("Distance ({0})", m_lengthUnits);
                area.AxisY.TitleFont = new Font(area.AxisY.TitleFont.FontFamily, 35.0F);
                area.AxisY.LabelAutoFitMinFontSize = 35;
                
                startTime = faultCurves[0][0].Time;
                endTime = faultCurves[0].DataPoints.Last().Time;

                chart.Titles.Add(string.Format("Fault Curves ({0:yyyy-MM-dd HH:mm:ss.fffffff})", startTime));
                chart.Titles[0].Font = new Font(chart.Titles[0].Font.FontFamily, 35.0F, FontStyle.Bold);
                chart.Width = 2200;
                chart.Height = 1360;
                chart.ChartAreas.Add(area);

                maxSeconds = 0.0D;
                minSeconds = (endTime - startTime).TotalSeconds;

                foreach (DataSeries dataSeries in faultCurves)
                {
                    series = new Series();
                    series.ChartType = SeriesChartType.FastLine;

                    foreach (DataPoint dataPoint in dataSeries.DataPoints)
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

        private void SendEmail(List<Recipient> recipients, string subject, string body, Stream faultCurveStream)
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
                Attachment data = new Attachment(faultCurveStream, "FaultCurves.png");
                data.ContentId = "FaultCurves.png";
                emailMessage.Attachments.Add(data);

                // Send the email
                smtpClient.Send(emailMessage);
            }
        }

        private DataSeries ToDataSeries(FaultLocationData.FaultCurveRow faultCurve)
        {
            DataGroup dataGroup = new DataGroup();
            dataGroup.FromData(faultCurve.Data);
            return dataGroup[0];
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

        #endregion
    }
}
