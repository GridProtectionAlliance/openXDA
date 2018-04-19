//******************************************************************************************************
//  ServiceHost.cs - Gbtc
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
//  04/18/2018 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Globalization;
using System.IO;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;
using GSF;
using GSF.Configuration;
using GSF.Console;
using GSF.IO;
using GSF.ServiceProcess;
using log4net.Appender;
using log4net.Config;
using log4net.Layout;
using openXDAFileWatcher.Logging;

namespace openXDAFileWatcher
{
    public partial class ServiceHost : ServiceBase
    {
        #region [ Members ]

        // Fields
        private FileWatcherEngine m_fileWatcherEngine;
        private Thread m_startEngineThread;
        private bool m_serviceStopping;

        #endregion

        #region [ Constructors ]

        public ServiceHost()
        {
            // Make sure default service settings exist
            ConfigurationFile configFile = ConfigurationFile.Current;
            CategorizedSettingsElementCollection systemSettings = configFile.Settings["systemSettings"];
            systemSettings.Add("DefaultCulture", "en-US", "Default culture to use for language, country/region and calendar formats.");

            // Attempt to set default culture
            string defaultCulture = systemSettings["DefaultCulture"].ValueAs("en-US");
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.CreateSpecificCulture(defaultCulture);     // Defaults for date formatting, etc.
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.CreateSpecificCulture(defaultCulture);   // Culture for resource strings, etc.

            InitializeComponent();

            // Register event handlers.
            m_serviceHelper.ServiceStarted += ServiceHelper_ServiceStarted;
            m_serviceHelper.ServiceStopping += ServiceHelper_ServiceStopping;
        }

        #endregion

        #region [ Methods ]

        private void ServiceHelper_ServiceStarted(object sender, EventArgs e)
        {
            ServiceHelperAppender serviceHelperAppender;
            RollingFileAppender debugLogAppender;
            RollingFileAppender skippedFilesAppender;

            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;

            // Set current working directory to fix relative paths
            Directory.SetCurrentDirectory(FilePath.GetAbsolutePath(""));

            // Set up logging
            serviceHelperAppender = new ServiceHelperAppender(m_serviceHelper);

            debugLogAppender = new RollingFileAppender();
            debugLogAppender.StaticLogFileName = false;
            debugLogAppender.AppendToFile = true;
            debugLogAppender.RollingStyle = RollingFileAppender.RollingMode.Composite;
            debugLogAppender.MaxSizeRollBackups = 10;
            debugLogAppender.PreserveLogFileNameExtension = true;
            debugLogAppender.MaximumFileSize = "1MB";
            debugLogAppender.Layout = new PatternLayout("%date [%thread] %-5level %logger - %message%newline");
            debugLogAppender.AddFilter(new FileSkippedExceptionFilter());

            skippedFilesAppender = new RollingFileAppender();
            skippedFilesAppender.StaticLogFileName = false;
            skippedFilesAppender.AppendToFile = true;
            skippedFilesAppender.RollingStyle = RollingFileAppender.RollingMode.Composite;
            skippedFilesAppender.MaxSizeRollBackups = 10;
            skippedFilesAppender.PreserveLogFileNameExtension = true;
            skippedFilesAppender.MaximumFileSize = "1MB";
            skippedFilesAppender.Layout = new PatternLayout("%date [%thread] %-5level %logger - %message%newline");
            skippedFilesAppender.AddFilter(new FileSkippedExceptionFilter(false));

            try
            {
                if (!Directory.Exists("Debug"))
                    Directory.CreateDirectory("Debug");

                debugLogAppender.File = @"Debug\openXDA.log";
                skippedFilesAppender.File = @"Debug\SkippedFiles.log";
            }
            catch (Exception ex)
            {
                debugLogAppender.File = "openXDA.log";
                skippedFilesAppender.File = "SkippedFiles.log";
                m_serviceHelper.ErrorLogger.Log(ex);
            }

            debugLogAppender.ActivateOptions();
            skippedFilesAppender.ActivateOptions();
            BasicConfigurator.Configure(serviceHelperAppender, debugLogAppender, skippedFilesAppender);

            // Set up heartbeat and client request handlers
            m_serviceHelper.AddProcess(EnumerateWatchDirectoriesHandler, "EnumWatchDirectories");
            m_serviceHelper.ClientRequestHandlers.Add(new ClientRequestHandler("ReloadSystemSettings", "Reloads system settings from the database", ReloadSystemSettingsRequestHandler));
            m_serviceHelper.ClientRequestHandlers.Add(new ClientRequestHandler("EngineStatus", "Displays status information about the XDA engine", EngineStatusHandler));
            m_serviceHelper.ClientRequestHandlers.Add(new ClientRequestHandler("TweakFileProcessor", "Modifies the behavior of the file processor at runtime", TweakFileProcessorHandler));

            // Set up the file watcher engine
            m_fileWatcherEngine = new FileWatcherEngine();

            // Set up separate thread to start the engine
            m_startEngineThread = new Thread(() =>
            {
                const int RetryDelay = 1000;
                const int SleepTime = 200;
                const int LoopCount = RetryDelay / SleepTime;

                bool engineStarted = false;

                while (true)
                {
                    engineStarted = engineStarted || TryStartEngine();

                    if (engineStarted)
                        break;

                    for (int i = 0; i < LoopCount; i++)
                    {
                        if (m_serviceStopping)
                            return;

                        Thread.Sleep(SleepTime);
                    }
                }
            });

            m_startEngineThread.Start();
        }

        private void ServiceHelper_ServiceStopping(object sender, EventArgs e)
        {
            // If the start engine thread is still
            // running, wait for it to stop
            m_serviceStopping = true;
            m_startEngineThread.Join();

            // Dispose of the watcher engine
            m_fileWatcherEngine.Stop();
            m_fileWatcherEngine.Dispose();

            // Save updated settings to the configuration file
            ConfigurationFile.Current.Save();

            Dispose();
        }

        // Attempts to start the engine and logs startup errors.
        private bool TryStartEngine()
        {
            try
            {
                // Start the analysis engine
                m_fileWatcherEngine.Start();
                return true;
            }
            catch (Exception ex)
            {
                string message;

                // Stop the analysis engine
                m_fileWatcherEngine.Stop();

                // Log the exception
                message = "Failed to start XDA engine due to exception: " + ex.Message;
                HandleException(new InvalidOperationException(message, ex));

                return false;
            }
        }

        private void EnumerateWatchDirectoriesHandler(string s, object[] args)
        {
            m_fileWatcherEngine.EnumerateWatchDirectories();
        }

        // Reloads system settings from the database.
        private void ReloadSystemSettingsRequestHandler(ClientRequestInfo requestInfo)
        {
            m_fileWatcherEngine.ReloadSystemSettings();
            SendResponse(requestInfo, true);
        }

        // Displays status information about the XDA engine.
        private void EngineStatusHandler(ClientRequestInfo requestInfo)
        {
            if (m_fileWatcherEngine != null)
                DisplayResponseMessage(requestInfo, m_fileWatcherEngine.Status);
            else
                SendResponseWithAttachment(requestInfo, false, null, "Engine is not ready.");
        }

        // Modifies the behavior of the file processor at runtime.
        private void TweakFileProcessorHandler(ClientRequestInfo requestInfo)
        {
            if (requestInfo.Request.Arguments.ContainsHelpRequest)
            {
                string helpMessage = m_fileWatcherEngine.TweakFileProcessor(new string[] { "-?" });
                DisplayResponseMessage(requestInfo, helpMessage);
                return;
            }

            string[] args = Arguments.ToArgs(requestInfo.Request.Arguments.ToString());
            string message = m_fileWatcherEngine.TweakFileProcessor(args);
            DisplayResponseMessage(requestInfo, message);
        }

        // Send the error to the service helper, error logger, and each service monitor
        public void HandleException(Exception ex)
        {
            string newLines = string.Format("{0}{0}", Environment.NewLine);
            m_serviceHelper.ErrorLogger.Log(ex);
            m_serviceHelper.UpdateStatus(UpdateType.Alarm, "{0}", ex.Message + newLines);
        }

        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            foreach (Exception ex in e.Exception.Flatten().InnerExceptions)
                HandleException(ex);

            e.SetObserved();
        }

        #region [ Broadcast Message Handling ]

        /// <summary>
        /// Sends an actionable response to client.
        /// </summary>
        /// <param name="requestInfo"><see cref="ClientRequestInfo"/> instance containing the client request.</param>
        /// <param name="success">Flag that determines if this response to client request was a success.</param>
        protected virtual void SendResponse(ClientRequestInfo requestInfo, bool success)
        {
            SendResponseWithAttachment(requestInfo, success, null, null);
        }

        /// <summary>
        /// Sends an actionable response to client with a formatted message and attachment.
        /// </summary>
        /// <param name="requestInfo"><see cref="ClientRequestInfo"/> instance containing the client request.</param>
        /// <param name="success">Flag that determines if this response to client request was a success.</param>
        /// <param name="attachment">Attachment to send with response.</param>
        /// <param name="status">Formatted status message to send with response.</param>
        /// <param name="args">Arguments of the formatted status message.</param>
        protected virtual void SendResponseWithAttachment(ClientRequestInfo requestInfo, bool success, object attachment, string status, params object[] args)
        {
            try
            {
                // Send actionable response
                m_serviceHelper.SendActionableResponse(requestInfo, success, attachment, status, args);

                // Log details of client request as well as response
                if (m_serviceHelper.LogStatusUpdates && m_serviceHelper.StatusLog.IsOpen)
                {
                    string responseType = requestInfo.Request.Command + (success ? ":Success" : ":Failure");
                    string arguments = requestInfo.Request.Arguments.ToString();
                    string message = responseType + (string.IsNullOrWhiteSpace(arguments) ? "" : "(" + arguments + ")");

                    if (status != null)
                    {
                        if (args.Length == 0)
                            message += " - " + status;
                        else
                            message += " - " + string.Format(status, args);
                    }

                    m_serviceHelper.StatusLog.WriteTimestampedLine(message);
                }
            }
            catch (Exception ex)
            {
                string message = string.Format("Failed to send client response due to an exception: {0}", ex.Message);
                HandleException(new InvalidOperationException(message, ex));
            }
        }

        /// <summary>
        /// Displays a response message to client requestor.
        /// </summary>
        /// <param name="requestInfo"><see cref="ClientRequestInfo"/> instance containing the client request.</param>
        /// <param name="status">Formatted status message to send to client.</param>
        protected virtual void DisplayResponseMessage(ClientRequestInfo requestInfo, string status)
        {
            DisplayResponseMessage(requestInfo, "{0}", status);
        }

        /// <summary>
        /// Displays a response message to client requestor.
        /// </summary>
        /// <param name="requestInfo"><see cref="ClientRequestInfo"/> instance containing the client request.</param>
        /// <param name="status">Formatted status message to send to client.</param>
        /// <param name="args">Arguments of the formatted status message.</param>
        protected virtual void DisplayResponseMessage(ClientRequestInfo requestInfo, string status, params object[] args)
        {
            try
            {
                m_serviceHelper.UpdateStatus(requestInfo.Sender.ClientID, UpdateType.Information, string.Format("{0}{1}{1}", status, Environment.NewLine), args);
            }
            catch (Exception ex)
            {
                string message = string.Format("Failed to update client status \"{0}\" due to an exception: {1}", status.ToNonNullString(), ex.Message);
                HandleException(new InvalidOperationException(message, ex));
            }
        }

        #endregion

        #endregion
    }
}
