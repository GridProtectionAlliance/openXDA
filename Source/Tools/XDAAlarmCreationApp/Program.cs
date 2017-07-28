//******************************************************************************************************
//  Program.cs - Gbtc
//
//  Copyright © 2017, Grid Protection Alliance.  All Rights Reserved.
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
//  05/02/2017 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Windows.Forms;
using GSF.Diagnostics;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using GSF.Threading;
using System.Threading;
using System.Diagnostics;
using GSF;

namespace XDAAlarmCreationApp
{
    static class Program
    {
        /// <summary>
        /// Defines common global settings for the application.
        /// </summary>
        public static readonly GlobalSettings Global;

        /// <summary>
        /// Gets the list of currently connected hub clients.
        /// </summary>
        public static IHubConnectionContext<dynamic> HubClients => s_clients.Value;

        private static readonly MainWindow s_mainWindow;
        private static readonly Mutex s_singleInstanceMutex;
        private static readonly Lazy<IHubConnectionContext<dynamic>> s_clients;

        static Program()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            s_mainWindow = new MainWindow();
            s_singleInstanceMutex = InterprocessLock.GetNamedMutex();
            s_clients = new Lazy<IHubConnectionContext<dynamic>>(() => GlobalHost.ConnectionManager.GetHubContext<DataHub>().Clients);
            Global = MainWindow.Model.Global;
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                if (s_singleInstanceMutex.WaitOne(0, true))
                    Application.Run(s_mainWindow);
                else
                    using (Process.Start(Global.WebHostURL)) { }
            }catch(Exception ex)
            {
                System.IO.File.WriteAllText("ErrorLog.txt", ex.ToString());
            }
        }

        /// <summary>
        /// Common status message logging function for the application.
        /// </summary>
        public static void LogStatus(string message, bool pushToHubClients = false)
        {
            s_mainWindow.LogStatus(message, pushToHubClients);
        }

        /// <summary>
        /// Logs a status message to connected clients.
        /// </summary>
        /// <param name="message">Message to log.</param>
        /// <param name="type">Type of message to log.</param>
        public static void LogStatusMessage(string message, UpdateType type = UpdateType.Information)
        {
            s_mainWindow.LogStatus(message, true);
        }

        /// <summary>
        /// Common exception logging function for the application.
        /// </summary>
        public static void LogException(Exception ex, bool pushToHubClients = false)
        {
            s_mainWindow.LogException(ex, pushToHubClients);
        }

        /// <summary>
        /// Logs an exception to the service.
        /// </summary>
        /// <param name="ex">Exception to log.</param>
        public static void LogExceptionMessage(Exception ex)
        {
            s_mainWindow.LogException(ex, true);
        }

    }
}
