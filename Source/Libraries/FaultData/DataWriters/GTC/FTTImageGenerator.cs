//******************************************************************************************************
//  FTTImageGenerator.cs - Gbtc
//
//  Copyright © 2020, Grid Protection Alliance.  All Rights Reserved.
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
//  07/10/2020 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using FaultData.DataWriters.GTC.StringExtensions;
using log4net;

namespace FaultData.DataWriters.GTC
{
    public class FTTImageGenerator
    {
        #region [ Constructors ]

        public FTTImageGenerator(string urlFormat, int width, int height)
        {
            URLFormat = urlFormat;
            Width = width;
            Height = height;
        }

        #endregion

        #region [ Properties ]

        private string URLFormat { get; }
        private int Width { get; }
        private int Height { get; }

        #endregion

        #region [ Methods ]

        public Stream QueryToImageStream(string stationName, string lineKey, double distance, DateTime eventTime)
        {
            string url = GetURL(stationName, lineKey, distance, eventTime);
            return QueryToImageStreamAsync(url).GetAwaiter().GetResult();
        }

        private async Task<Stream> QueryToImageStreamAsync(string url)
        {
            TaskCompletionSource<Stream> queryTaskSource = new TaskCompletionSource<Stream>();

            Thread browserThread = new Thread(() =>
            {
                try { RunBrowser(url, queryTaskSource.SetResult); }
                catch (Exception ex) { queryTaskSource.SetException(ex); }
            });

            browserThread.SetApartmentState(ApartmentState.STA);
            browserThread.Start();
            return await queryTaskSource.Task;
        }

        private void RunBrowser(string url, Action<Stream> callback)
        {
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.ThrowException);

            using (WebBrowser webBrowser = new WebBrowser())
            {
                TaskCompletionSource<Stream> documentHandlerTaskSource = new TaskCompletionSource<Stream>();
                Task<Stream> documentHandlerTask = documentHandlerTaskSource.Task;

                webBrowser.DocumentCompleted += (_, __) =>
                    HandleDocumentCompleted(webBrowser.Document, documentHandlerTaskSource.SetResult);

                webBrowser.Width = Width;
                webBrowser.Height = Height;
                webBrowser.ScriptErrorsSuppressed = true;
                webBrowser.Navigate(url);

                while (!documentHandlerTask.IsCompleted)
                    Application.DoEvents();

                callback(documentHandlerTask.Result);
            }
        }

        private void HandleDocumentCompleted(HtmlDocument document, Action<Stream> callback)
        {
            document.Window.Error += (sender, args) =>
            {
                string message = new StringBuilder()
                    .AppendLine("FTT script error:")
                    .AppendLine($"    {args.Url}:{args.LineNumber}")
                    .AppendLine($"    {args.Description}")
                    .ToString();

                Log.Error(message);
                args.Handled = true;
            };

            Stopwatch stopwatch = Stopwatch.StartNew();
            var timer = new System.Windows.Forms.Timer();
            timer.Interval = 100;

            timer.Tick += (sender, args) =>
            {
                if (stopwatch.Elapsed > TimeSpan.FromMinutes(1))
                    throw new TimeoutException("Timeout waiting for output from webpage.");

                HtmlElement output = document.GetElementById("output");

                if (string.IsNullOrEmpty(output?.InnerHtml))
                    return;

                timer.Stop();
                timer.Dispose();

                Stream imageStream = GenerateImageStream(output);
                callback(imageStream);
            };

            timer.Start();
        }

        private Stream GenerateImageStream(HtmlElement output)
        {
            string imageData = output
                .GetElementsByTagName("a")
                .Cast<HtmlElement>()
                .Select(a => a.GetAttribute("href"))
                .Where(href => !string.IsNullOrEmpty(href))
                .Select(href => href.Substring(22))
                .FirstOrDefault();

            byte[] bytes = Convert.FromBase64String(imageData);
            return new MemoryStream(bytes);
        }

        private string GetURL(string station, string line, double distance, DateTime eventTime)
        {
            var parameters = new { station, line, distance, eventTime };
            string url = URLFormat.Interpolate(parameters);

            long now = DateTime.UtcNow.Ticks;
            string nocache = $"nocache={now:X}";
            UriBuilder builder = new UriBuilder(url);
            string query = builder.Query;

            builder.Query = !string.IsNullOrEmpty(query)
                ? string.Join("&", query, nocache)
                : nocache;

            return builder.ToString();
        }

        #endregion

        #region [ Static ]

        // Static Fields
        private static readonly ILog Log = LogManager.GetLogger(typeof(FTTImageGenerator));

        // Static Methods
        public static Stream ConvertToFTTImageStream(XElement fttElement)
        {
            string urlFormat = (string)fttElement.Attribute("url");
            int width = Convert.ToInt32((string)fttElement.Attribute("width") ?? "-1");
            int height = Convert.ToInt32((string)fttElement.Attribute("height") ?? "-1");

            string stationName = (string)fttElement.Attribute("stationName");
            string lineKey = (string)fttElement.Attribute("lineKey");
            double distance = Convert.ToDouble((string)fttElement.Attribute("distance") ?? "0.0D");
            DateTime eventTime = Convert.ToDateTime((string)fttElement.Attribute("eventTime") ?? default(DateTime).ToString());

            FTTImageGenerator generator = new FTTImageGenerator(urlFormat, width, height);
            return generator.QueryToImageStream(stationName, lineKey, distance, eventTime);
        }

        #endregion
    }
}
