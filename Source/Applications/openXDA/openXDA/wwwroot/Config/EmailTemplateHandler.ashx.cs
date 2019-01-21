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

using System;
using System.Collections.Concurrent;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using FaultData.DataWriters;
using GSF.Threading;
using GSF.Web.Hosting;
using GSF.Web.Model;
using GSF.Xml;

namespace openXDA
{
    /// <summary>
    /// Handles downloading chart data as an image.
    /// </summary>
    public class EmailTemplateHandler : IHostedHttpHandler
    {
        #region [ Members ]

        // Nested Types
        private class ChartIdentity : IEquatable<ChartIdentity>
        {
            #region [ Members ]

            // Fields
            private readonly Tuple<int, int, int> m_chartTuple;

            #endregion

            #region [ Constructors ]

            public ChartIdentity(int eventID, int templateID, int chartID)
            {
                m_chartTuple = Tuple.Create(eventID, templateID, chartID);
            }

            #endregion

            #region [ Properties ]

            public int EventID
            {
                get
                {
                    return m_chartTuple.Item1;
                }
            }

            public int TemplateID
            {
                get
                {
                    return m_chartTuple.Item2;
                }
            }

            public int ChartID
            {
                get
                {
                    return m_chartTuple.Item3;
                }
            }

            #endregion

            #region [ Methods ]

            public bool Equals(ChartIdentity other)
            {
                return m_chartTuple.Equals(other.m_chartTuple);
            }

            public override bool Equals(object obj)
            {
                ChartIdentity other = obj as ChartIdentity;

                if ((object)other != null)
                    return Equals(other);

                return base.Equals(obj);
            }

            public override int GetHashCode()
            {
                return m_chartTuple.GetHashCode();
            }

            #endregion
        }

        private class ChartData
        {
            #region [ Members ]

            // Fields
            private readonly XElement m_chartElement;
            private readonly long m_contentHash;
            private ICancellationToken m_cancellationToken;

            #endregion

            #region [ Constructors ]

            public ChartData(XElement chartElement, long contentHash)
            {
                m_chartElement = chartElement;
                m_contentHash = contentHash;
            }

            #endregion

            #region [ Properties ]

            public XElement ChartElement
            {
                get
                {
                    return m_chartElement;
                }
            }

            public long ContentHash
            {
                get
                {
                    return m_contentHash;
                }
            }

            public ICancellationToken CancellationToken
            {
                get
                {
                    return Interlocked.CompareExchange(ref m_cancellationToken, null, null);
                }
                set
                {
                    ICancellationToken cancellationToken = Interlocked.Exchange(ref m_cancellationToken, value);

                    if ((object)cancellationToken != null)
                        cancellationToken.Cancel();
                }
            }

            #endregion
        }

        // Constants
        private const string GetEventDetailSQL =
            "SELECT EventDetailSQL " +
            "FROM " +
            "    EmailType JOIN " +
            "    EventEmailParameters ON EventEmailParameters.EmailTypeID = EmailType.ID " +
            "WHERE EmailType.XSLTemplateID = {0}";

        private const string GetTemplateSQL =
            "SELECT Template " +
            "FROM XSLTemplate " +
            "WHERE ID = {0}";

        // Fields
        private long m_contentHash;
        private int m_eventID;
        private int m_templateID;

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Determines if client cache should be enabled for rendered handler content.
        /// </summary>
        /// <remarks>
        /// If rendered handler content does not change often, the server and client will use the
        /// <see cref="GetContentHash"/> to determine if the client needs to refresh the content.
        /// </remarks>
        public bool UseClientCache
        {
            get
            {
                return true;
            }
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Gets hash of response content based on any <paramref name="request"/> parameters.
        /// </summary>
        /// <param name="request">HTTP request message.</param>
        /// <remarks>
        /// Value is only used when <see cref="UseClientCache"/> is <c>true</c>.
        /// </remarks>
        public long GetContentHash(HttpRequestMessage request)
        {
            NameValueCollection parameters = request.RequestUri.ParseQueryString();
            int eventID = Convert.ToInt32(parameters["EventID"]);
            int templateID = Convert.ToInt32(parameters["TemplateID"]);

            string eventDetail;
            string template;

            using (DataContext dataContext = new DataContext())
            {
                string eventDetailSQL = dataContext.Connection.ExecuteScalar<string>(GetEventDetailSQL, templateID);
                eventDetail = dataContext.Connection.ExecuteScalar<string>(eventDetailSQL, eventID);
                template = dataContext.Connection.ExecuteScalar<string>(GetTemplateSQL, templateID);
            }

            byte[] contentBytes = Encoding.UTF8.GetBytes(eventDetail + template);
            m_contentHash = ComputeHash(contentBytes);
            return m_contentHash;

            long ComputeHash(byte[] data)
            {
                long hash = 17L;

                foreach (byte b in data)
                    hash = hash * 23L + b;

                return hash;
            }
        }

        /// <summary>
        /// Enables processing of HTTP web requests by a custom handler that implements the <see cref="GSF.Web.Hosting.IHostedHttpHandler"/> interface.
        /// </summary>
        /// <param name="request">HTTP request message.</param>
        /// <param name="response">HTTP response message.</param>
        public Task ProcessRequestAsync(HttpRequestMessage request, HttpResponseMessage response, System.Threading.CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                try
                {
                    NameValueCollection parameters = request.RequestUri.ParseQueryString();
                    m_eventID = Convert.ToInt32(parameters["EventID"]);
                    m_templateID = Convert.ToInt32(parameters["TemplateID"]);

                    if ((object)parameters["chartID"] == null)
                        ProcessEmailRequest(request, response);
                    else
                        ProcessChartRequest(request, response);
                }
                catch (Exception ex)
                {
                    string html = $"<html><body><b>Internal server error:</b><br/><pre>{ex}</pre></body></html>";
                    response.Content = new StringContent(html);
                    response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
                }
            });
        }

        private void ProcessEmailRequest(HttpRequestMessage request, HttpResponseMessage response)
        {
            XDocument doc = XDocument.Parse(ApplyTemplate(request), LoadOptions.PreserveWhitespace);
            doc.TransformAll("format", element => element.Format());
            doc.TransformAll("chart", (element, index) => ToImgTag(element, index));

            using (DataContext dataContext = new DataContext())
            {
                doc.TransformAll("pqi", (element) => PQIGenerator.GetPqiInformation(dataContext.Connection, element));
                doc.TransformAll("structure", (element) => StructureLocationGenerator.GetStructureLocationInformation(element));
                doc.TransformAll("lightning", (element) => LightningGenerator.GetLightningInfo(dataContext.Connection, element));
                doc.TransformAll("faultType", (element) => FaultTypeGenerator.GetFaultType(element));
            }

            string html = doc.ToString(SaveOptions.DisableFormatting).Replace("&amp;", "&").Replace("&lt;", "<").Replace("&gt;", ">");
            response.Content = new StringContent(html);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
        }

        private void ProcessChartRequest(HttpRequestMessage request, HttpResponseMessage response)
        {
            ChartData chartData;
            XElement chartElement;
            string title;

            NameValueCollection parameters = request.RequestUri.ParseQueryString();
            int chartID = Convert.ToInt32(parameters["chartID"]);
            ChartIdentity chartIdentity = new ChartIdentity(m_eventID, m_templateID, chartID);

            if (s_chartLookup.TryGetValue(chartIdentity, out chartData))
            {
                chartElement = chartData.ChartElement;
            }
            else
            {
                XDocument doc = XDocument.Parse(ApplyTemplate(request), LoadOptions.PreserveWhitespace);
                chartElement = doc.Descendants("chart").Skip(chartID).FirstOrDefault();
            }

            if ((object)chartElement == null)
            {
                response.StatusCode = HttpStatusCode.NotFound;
                return;
            }

            title = (string)chartElement.Attribute("yAxisTitle");

            using (DataContext dataContext = new DataContext())
            {
                response.Content = new StreamContent(ChartGenerator.ConvertToChartImageStream(dataContext.Connection, chartElement));
                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                response.Content.Headers.ContentDisposition.FileName = title + ".png";
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/png");
            }
        }

        private string ApplyTemplate(HttpRequestMessage request)
        {
            string eventDetail;
            string emailTemplate;

            using (DataContext dataContext = new DataContext())
            {
                string eventDetailSQL = dataContext.Connection.ExecuteScalar<string>(GetEventDetailSQL, m_templateID);
                eventDetail = dataContext.Connection.ExecuteScalar<string>(eventDetailSQL, m_eventID);
                emailTemplate = dataContext.Connection.ExecuteScalar<string>(GetTemplateSQL, m_templateID);
            }

            return eventDetail.ApplyXSLTransform(emailTemplate);
        }

        private XElement ToImgTag(XElement chartElement, int chartID)
        {
            ChartIdentity chartIdentity = new ChartIdentity(m_eventID, m_templateID, chartID);
            ChartData chartData = s_chartLookup.GetOrAdd(chartIdentity, ident => new ChartData(chartElement, m_contentHash));

            // If the cancellation token has been initialized, but we are not able to cancel it,
            // it's possible that the action already executed so we attempt to add it back into the lookup
            if ((object)chartData.CancellationToken != null && chartData.CancellationToken.Cancel())
                chartData = s_chartLookup.GetOrAdd(chartIdentity, chartData);

            // If the content hash of the chart does not match the content hash of the email,
            // then the email has changed and we need to update the chart data in the cache
            if (chartData.ContentHash != m_contentHash)
                chartData = s_chartLookup[chartIdentity] = new ChartData(chartElement, m_contentHash);

            // Create a new cancellation token to remove the chart data from the cache in one minute
            chartData.CancellationToken = new Action(() => s_chartLookup.TryRemove(chartIdentity, out chartData)).DelayAndExecute(60 * 1000);

            string url = $"EmailTemplateHandler.ashx?EventID={m_eventID}&TemplateID={m_templateID}&ChartID={chartID}";
            return new XElement("img", new XAttribute("src", url));
        }

        #endregion

        #region [ Static ]

        // Static Fields
        private static readonly ConcurrentDictionary<ChartIdentity, ChartData> s_chartLookup = new ConcurrentDictionary<ChartIdentity, ChartData>();

        #endregion
    }
}