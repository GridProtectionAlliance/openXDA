//******************************************************************************************************
//  FileDownloadHandler.ashx.cs - Gbtc
//
//  Copyright © 2016, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the MIT License (MIT), the "License"; you may
//  not use this file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://opensource.org/licenses/MIT
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  07/29/2016 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************

using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using GSF.Security;
using GSF.Web.Hosting;
using GSF.Web.Model;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using FaultData.DataWriters;
using FaultData.Database;
using System.Data.SqlClient;
using System;
using System.Windows.Forms.DataVisualization.Charting;
using System.Xml.Linq;
using GSF.Xml;
using GSF.Data;
using System.Net;
using System.Drawing;

namespace openXDA
{
    /// <summary>
    /// Handles downloading chart data as an image.
    /// </summary>
    public class EmailTemplateHandler : IHostedHttpHandler
    {
        #region [ Members ]

        // Fields
        private int m_eventID;
        private int m_templateID;

        #endregion

        #region [ Properties ]

        public bool UseClientCache
        {
            get
            {
                return true;
            }
        }

        #endregion

        #region [ Methods ]

        public long GetContentHash(HttpRequestMessage request)
        {
            NameValueCollection parameters = request.RequestUri.ParseQueryString();
            int eventID = Convert.ToInt32(parameters["EventID"]);
            int templateID = Convert.ToInt32(parameters["TemplateID"]);

            using (DataContext context = new DataContext())
            {
                return context.Connection.ExecuteScalar<long>("SELECT dbo.ComputeHash({0}, {1})", eventID, templateID);
            }
        }

        /// <summary>
        /// Enables processing of HTTP web requests by a custom handler that implements the <see cref="T:GSF.Web.Hosting.IHostedHttpHandler"/> interface.
        /// </summary>
        /// <param name="request">HTTP request message.</param><param name="response">HTTP response message.</param>
        public Task ProcessRequestAsync(HttpRequestMessage request, HttpResponseMessage response)
        {
            return Task.Run(() =>
            {
                SecurityProviderCache.ValidateCurrentProvider();
                NameValueCollection parameters = request.RequestUri.ParseQueryString();

                m_eventID = Convert.ToInt32(parameters["EventID"]);
                m_templateID = Convert.ToInt32(parameters["TemplateID"]);

                if ((object)parameters["chartID"] == null)
                    ProcessEmailRequest(request, response);
                else
                    ProcessChartRequest(request, response);
            });
        }

        private void ProcessEmailRequest(HttpRequestMessage request, HttpResponseMessage response)
        {
            XDocument doc = XDocument.Parse(ApplyTemplate(request), LoadOptions.PreserveWhitespace);
            doc.TransformAll("format", element => element.Format());
            doc.TransformAll("chart", (element, index) => ToImgTag(index));

            string html = doc.ToString(SaveOptions.DisableFormatting).Replace("&amp;", "&");
            response.Content = new StringContent(html);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
        }

        private void ProcessChartRequest(HttpRequestMessage request, HttpResponseMessage response)
        {
            XDocument doc = XDocument.Parse(ApplyTemplate(request), LoadOptions.PreserveWhitespace);
            NameValueCollection parameters = request.RequestUri.ParseQueryString();
            int chartID = Convert.ToInt32(parameters["chartID"]);

            XElement chartElement = doc.Descendants("chart").Skip(chartID).FirstOrDefault();
            string title;

            if ((object)chartElement == null)
            {
                response.StatusCode = HttpStatusCode.NotFound;
                return;
            }

            title = (string)chartElement.Attribute("yAxisTitle");
            response.Content = new StreamContent(ConvertToChartImageStream(chartElement));
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            response.Content.Headers.ContentDisposition.FileName = title + ".png";
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/png");
        }

        private string ApplyTemplate(HttpRequestMessage request)
        {
            NameValueCollection parameters = request.RequestUri.ParseQueryString();

            string eventDetail;
            string emailTemplate;

            using (DataContext dataContext = new DataContext())
            {
                eventDetail = dataContext.Connection.ExecuteScalar<string>("SELECT EventDetail FROM EventDetail WHERE EventID = {0}", m_eventID);
                emailTemplate = dataContext.Connection.ExecuteScalar<string>("SELECT Template FROM FaultEmailTemplate WHERE ID = {0}", m_templateID);
            }

            return eventDetail.ApplyXSLTransform(emailTemplate);
        }

        private XElement ToImgTag(int chartID)
        {
            string url = $"EmailTemplateHandler.ashx?EventID={m_eventID}&TemplateID={m_templateID}&ChartID={chartID}";
            return new XElement("img", new XAttribute("src", url));
        }

        private Stream ConvertToChartImageStream(XElement chartElement)
        {
            Lazy<DataRow> faultSummary;
            Lazy<double> systemFrequency;
            DateTime inception;
            DateTime clearing;

            int width;
            int height;
            double prefaultCycles;
            double postfaultCycles;

            string title;
            List<string> keys;
            List<string> names;
            DateTime startTime;
            DateTime endTime;

            int eventID;
            int faultID;

            eventID = Convert.ToInt32((string)chartElement.Attribute("eventID") ?? "-1");
            faultID = Convert.ToInt32((string)chartElement.Attribute("faultID") ?? "-1");
            prefaultCycles = Convert.ToDouble((string)chartElement.Attribute("prefaultCycles") ?? "NaN");
            postfaultCycles = Convert.ToDouble((string)chartElement.Attribute("postfaultCycles") ?? "NaN");

            faultSummary = null;
            startTime = DateTime.MinValue;
            endTime = DateTime.MaxValue;

            using (DataContext dataContext = new DataContext())
            {
                faultSummary = new Lazy<DataRow>(() => dataContext.Connection.RetrieveData("SELECT * FROM FaultSummary WHERE ID = {0}", faultID).Select().FirstOrDefault());
                systemFrequency = new Lazy<double>(() => dataContext.Connection.ExecuteScalar(60.0D, "SELECT Value FROM Setting WHERE Name = 'SystemFrequency'"));

                // One way or another, we need to get the event ID in order to build the chart
                if (eventID == -1 && (object)faultSummary.Value != null)
                    eventID = faultSummary.Value.ConvertField<int>("EventID");

                if (eventID == -1)
                    eventID = m_eventID;

                if (!double.IsNaN(prefaultCycles) && (object)faultSummary.Value != null)
                {
                    inception = faultSummary.Value.ConvertField<DateTime>("Inception");
                    startTime = inception.AddSeconds(-prefaultCycles / systemFrequency.Value);
                }

                if (!double.IsNaN(postfaultCycles) && (object)faultSummary.Value != null)
                {
                    inception = faultSummary.Value.ConvertField<DateTime>("Inception");
                    clearing = inception.AddSeconds(faultSummary.Value.ConvertField<double>("DurationSeconds"));
                    endTime = clearing.AddSeconds(postfaultCycles / systemFrequency.Value);
                }
            }

            title = (string)chartElement.Attribute("yAxisTitle");
            keys = GetKeys(chartElement);
            names = GetNames(chartElement);

            width = Convert.ToInt32((string)chartElement.Attribute("width"));
            height = Convert.ToInt32((string)chartElement.Attribute("height"));

            using (DataContext dataContext = new DataContext())
            using (DbAdapterContainer dbAdapterContainer = new DbAdapterContainer((SqlConnection)dataContext.Connection.Connection))
            {
                ChartGenerator chartGenerator = new ChartGenerator(dbAdapterContainer, eventID);

                using (Chart chart = chartGenerator.GenerateChart(title, keys, names, startTime, endTime))
                {
                    SetChartSize(chart, width, height);

                    if ((object)chartElement.Attribute("yAxisMaximum") != null)
                        chart.ChartAreas[0].AxisY.Maximum = Convert.ToDouble((string)chartElement.Attribute("yAxisMaximum"));

                    if ((object)chartElement.Attribute("yAxisMinimum") != null)
                        chart.ChartAreas[0].AxisY.Minimum = Convert.ToDouble((string)chartElement.Attribute("yAxisMinimum"));

                    if (string.Equals((string)chartElement.Attribute("highlightCalculation"), "index", StringComparison.OrdinalIgnoreCase))
                    {
                        if ((object)faultSummary.Value != null)
                        {
                            int calculationCycle = faultSummary.Value.ConvertField<int>("CalculationCycle");
                            DateTime calculationTime = chartGenerator.ToDateTime(calculationCycle);
                            double calculationPosition = chart.ChartAreas[0].AxisX.Minimum + (calculationTime - startTime).TotalSeconds;
                            chart.ChartAreas[0].CursorX.Position = calculationPosition;
                        }
                    }
                    else if (string.Equals((string)chartElement.Attribute("highlightCalculation"), "cycle", StringComparison.OrdinalIgnoreCase))
                    {
                        if ((object)faultSummary.Value != null)
                        {
                            int calculationCycle = faultSummary.Value.ConvertField<int>("CalculationCycle");
                            DateTime calculationTime = chartGenerator.ToDateTime(calculationCycle);
                            double calculationPosition = chart.ChartAreas[0].AxisX.Minimum + (calculationTime - startTime).TotalSeconds;
                            chart.ChartAreas[0].CursorX.SelectionStart = calculationPosition;
                            chart.ChartAreas[0].CursorX.SelectionEnd = calculationPosition + 1.0D / 60.0D;
                        }
                    }

                    return ConvertToImageStream(chart, ChartImageFormat.Png);
                }
            }
        }

        #endregion

        #region [ Static ]

        // Static Methods
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

            foreach (System.Windows.Forms.DataVisualization.Charting.Series series in chart.Series)
                series.BorderWidth = borderWidth;
        }

        private static Stream ConvertToImageStream(Chart chart, ChartImageFormat format)
        {
            MemoryStream stream = new MemoryStream();
            chart.SaveImage(stream, format);
            stream.Position = 0;
            return stream;
        }

        #endregion
    }
}