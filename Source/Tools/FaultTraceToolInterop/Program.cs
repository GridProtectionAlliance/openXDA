//******************************************************************************************************
//  Program.cs - Gbtc
//
//  Copyright © 2021, Grid Protection Alliance.  All Rights Reserved.
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
//  02/10/2021 - Theo Laughner
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using CefSharp;
using CefSharp.OffScreen;

namespace FaultTraceToolInterop
{
    class Program
    {
        static void Main()
        {
            bool done = false;
            ChromiumWebBrowser webView;
            string filename;
            TimeSpan timeout;

            void GenerateImage(string message)
            {
                if (!message.StartsWith("data:image"))
                {
                    Console.WriteLine(message);
                    return;
                }

                string imageData = message.Substring(22);
                byte[] bytes = Convert.FromBase64String(imageData);
                try
                {
                    File.WriteAllBytes(filename, bytes);
                }
                catch (Exception ex)
                {
                    PrintUsage();
                    Console.Error.WriteLine("ERROR");
                    Console.Error.WriteLine(ex);
                }
                finally
                {
                    done = true;
                }
            }

            async void BrowserLoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
            {
                if (!e.IsLoading)
                {
                    webView.LoadingStateChanged -= BrowserLoadingStateChanged;
                    JavascriptResponse jsResp;

                    do
                    {
                        jsResp = await webView.EvaluateScriptAsync("document.getElementById(\"output\").innerHTML;");
                        Thread.Sleep(100);
                    } while (jsResp.Result.ToString().Length < 22);

                    jsResp = await webView.EvaluateScriptAsync("document.getElementById(\"outputjpg\").href;");
                    string rslt = jsResp.Result.ToString();

                    GenerateImage(rslt);
                }
            }

            void PrintUsage()
            {
                Console.WriteLine("Usage: command url width height timeout filename");
            }

            string[] args = Environment.GetCommandLineArgs();
            UriBuilder builder;

            try
            {
                if (args.Length < 6)
                {
                    throw new Exception("Invalid number of arguments");
                }
                builder = new UriBuilder(args[1]);

                if (!Int32.TryParse(args[2], out Int32 width))
                {
                    throw new Exception("Invalid Width Parameter");
                }
                if (!Int32.TryParse(args[3], out Int32 height))
                {
                    throw new Exception("Invalid Height Parameter");
                }
                if (Int32.TryParse(args[4], out Int32 toSecs))
                {
                    timeout = new TimeSpan(0, 0, toSecs);
                }
                else
                {
                    throw new Exception("Invalid Timeout Parameter");
                }
                filename = args[5];

                long now = DateTime.UtcNow.Ticks;
                string nocache = $"nocache={now:X}";

                IEnumerable<string> queryParts = new[] { builder.Query, nocache }
                    .Where(str => !string.IsNullOrEmpty(str));

                builder.Query = string.Join("&", queryParts);
                var settings = new CefSettings()
                {
                    CachePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "FTTAPI", "Cache")
                };

                Cef.Initialize(settings, performDependencyCheck: true, browserProcessHandler: null);

                using (webView = new ChromiumWebBrowser(builder.ToString()))
                {
                    webView.Size = new System.Drawing.Size(width, height);
                    webView.LoadingStateChanged += BrowserLoadingStateChanged;

                    //TimeSpan timeout = TimeSpan.FromSeconds(10);
                    Stopwatch stopwatch = Stopwatch.StartNew();

                    while (true)
                    {
                        if (done)
                        {
                            Console.WriteLine(stopwatch.Elapsed);
                            Cef.Shutdown();
                            Console.WriteLine("Finished");
                            break;

                        }

                        if (stopwatch.Elapsed > timeout)
                            throw new TimeoutException("Timeout waiting for image data.");

                        Thread.Sleep(100);
                    }
                }
            }
            catch (Exception ex)
            {
                PrintUsage();
                Console.Error.WriteLine("ERROR");
                Console.Error.WriteLine(ex);
                Console.WriteLine();
            }

        }

    }
}
