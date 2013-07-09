//******************************************************************************************************
//  ServiceHost.cs - Gbtc
//
//  Copyright © 2013, Grid Protection Alliance.  All Rights Reserved.
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
//  05/02/2013 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.ComponentModel;
using System.IO;
using System.ServiceProcess;
using System.Text;
using System.Windows.Forms;
using GSF;
using GSF.Adapters;
using GSF.IO;
using GSF.ServiceProcess;
using XDAServiceMonitor;

namespace XDAFileWatcher
{
    public partial class ServiceHost : ServiceBase
    {
        #region [ Members ]

        // Fields
        private string m_configFile;
        private AdapterLoader<IServiceMonitor> m_serviceMonitors;
        private XDAFileWatcher m_fileWatcher;

        #endregion

        #region [ Constructors ]

        public ServiceHost()
            : base()
        {
            InitializeComponent();

            // Register event handlers.
            m_serviceHelper.ServiceStarted += ServiceHelper_ServiceStarted;
            m_serviceHelper.ServiceStopping += ServiceHelper_ServiceStopping;
        }

        public ServiceHost(IContainer container)
            : this()
        {
            if (container != null)
                container.Add(this);
        }

        #endregion

        #region [ Methods ]

        private void ServiceHelper_ServiceStarted(object sender, EventArgs e)
        {
            m_serviceHelper.AddScheduledProcess(ServiceHeartbeatHandler, "ServiceHeartbeat", "* * * * *");
            m_serviceHelper.ClientRequestHandlers.Add(new ClientRequestHandler("ReloadConfig", "Reloads configuration from configuration file", ReloadConfigRequestHandler));
            m_serviceHelper.ClientRequestHandlers.Add(new ClientRequestHandler("ForceEvent", "Forces an event to be processed by the file watcher", ForceEventRequestHandler));

            m_serviceMonitors = new AdapterLoader<IServiceMonitor>();
            m_serviceMonitors.AdapterLoaded += ServiceMonitors_AdapterLoaded;
            m_serviceMonitors.AdapterUnloaded += ServiceMonitors_AdapterUnloaded;
            m_serviceMonitors.Initialize();

            m_configFile = Path.Combine(Application.StartupPath, "Filewatcher.config");
            m_fileWatcher = new XDAFileWatcher();
            m_fileWatcher.ReadConfigFile(m_configFile);
            m_fileWatcher.StartWatching();
        }

        private void ServiceHelper_ServiceStopping(object sender, EventArgs eventArgs)
        {
            m_serviceMonitors.AdapterLoaded -= ServiceMonitors_AdapterLoaded;
            m_serviceMonitors.AdapterUnloaded -= ServiceMonitors_AdapterUnloaded;
            m_serviceMonitors.Dispose();
        }

        private void ForceEventRequestHandler(ClientRequestInfo requestInfo)
        {
            if (requestInfo.Request.Arguments.ContainsHelpRequest)
            {
                StringBuilder helpMessage = new StringBuilder();

                helpMessage.Append("Forces an event to be processed by the file watcher on a set of files.");
                helpMessage.AppendLine();
                helpMessage.AppendLine();
                helpMessage.Append("   Usage:");
                helpMessage.AppendLine();
                helpMessage.Append("       ForceEvent <EventType> <File> [<File>...] [Options]");
                helpMessage.AppendLine();
                helpMessage.AppendLine();
                helpMessage.Append("   EventType:".PadRight(20));
                helpMessage.Append("Type of the event (Created, Changed, Deleted, Renamed)");
                helpMessage.AppendLine();
                helpMessage.Append("   File:".PadRight(20));
                helpMessage.Append("Name of the file to be processed (wildcards allowed)");
                helpMessage.AppendLine();
                helpMessage.AppendLine();
                helpMessage.Append("   Options:");
                helpMessage.AppendLine();
                helpMessage.Append("       -?".PadRight(20));
                helpMessage.Append("Displays this help message");

                DisplayResponseMessage(requestInfo, helpMessage.ToString());
            }
            else
            {
                FileEventType eventType;
                string filePattern;
                string orderedArg;

                if (!requestInfo.Request.Arguments.Exists("OrderedArg1"))
                {
                    SendResponse(requestInfo, false, "No event type was specified.");
                    return;
                }

                if (!Enum.TryParse(requestInfo.Request.Arguments["OrderedArg1"], true, out eventType))
                    eventType = FileEventType.None;

                for (int i = 2; i <= requestInfo.Request.Arguments.OrderedArgCount; i++)
                {
                    orderedArg = string.Format("OrderedArg{0}", i);
                    filePattern = requestInfo.Request.Arguments[orderedArg];

                    if (!Path.IsPathRooted(filePattern))
                        filePattern = Path.Combine(m_fileWatcher.RootPathToWatch, filePattern);

                    foreach (string filePath in FilePath.GetFileList(filePattern))
                        m_fileWatcher.ProcessEvent(filePath, eventType);
                }

                SendResponse(requestInfo, true, "Command {0} successfully invoked.", requestInfo.Request.Command.QuoteWrap());
            }
        }

        private void ReloadConfigRequestHandler(ClientRequestInfo requestInfo)
        {
            if (requestInfo.Request.Arguments.ContainsHelpRequest)
            {
                StringBuilder helpMessage = new StringBuilder();

                helpMessage.Append("Reloads the Filewatcher.config configuration file.");
                helpMessage.AppendLine();
                helpMessage.AppendLine();
                helpMessage.Append("   Usage:");
                helpMessage.AppendLine();
                helpMessage.Append("       ReloadConfig [Options]");
                helpMessage.AppendLine();
                helpMessage.AppendLine();
                helpMessage.Append("   Options:");
                helpMessage.AppendLine();
                helpMessage.Append("       -?".PadRight(20));
                helpMessage.Append("Displays this help message");

                DisplayResponseMessage(requestInfo, helpMessage.ToString());
            }
            else
            {
                m_fileWatcher.ReadConfigFile(m_configFile);
                SendResponse(requestInfo, true);
            }
        }

        private void ServiceHeartbeatHandler(string s, object[] args)
        {
            foreach (IServiceMonitor serviceMonitor in m_serviceMonitors.Adapters)
            {
                try
                {
                    if (serviceMonitor.Enabled)
                        serviceMonitor.HandleServiceHeartbeat();
                }
                catch (Exception ex)
                {
                    HandleException(ex);
                }
            }
        }

        private void HandleException(Exception ex)
        {
            string newLines = string.Format("{0}{0}", Environment.NewLine);

            m_serviceHelper.ErrorLogger.Log(ex);
            m_serviceHelper.UpdateStatus(UpdateType.Alarm, ex.Message + newLines);

            foreach (IServiceMonitor serviceMonitor in m_serviceMonitors.Adapters)
            {
                try
                {
                    serviceMonitor.HandleServiceErrorMessage(ex.Message);
                }
                catch (Exception ex2)
                {
                    m_serviceHelper.ErrorLogger.Log(ex2);
                    m_serviceHelper.UpdateStatus(UpdateType.Alarm, ex2.Message + newLines);
                }
            }
        }

        #region [ Service Monitor Handlers ]

        private void ServiceMonitors_AdapterLoaded(object sender, EventArgs<IServiceMonitor> e)
        {
            m_serviceHelper.UpdateStatusAppendLine(UpdateType.Information, "{0} has been loaded", e.Argument.GetType().Name);
        }

        private void ServiceMonitors_AdapterUnloaded(object sender, EventArgs<IServiceMonitor> e)
        {
            m_serviceHelper.UpdateStatusAppendLine(UpdateType.Information, "{0} has been unloaded", e.Argument.GetType().Name);
        }

        #endregion

        #region [ Broadcast Message Handling ]

        /// <summary>
        /// Sends a message to all connected clients.
        /// </summary>
        /// <param name="message">The message to be sent.</param>
        public void BroadcastMessage(string message)
        {
            m_serviceHelper.UpdateStatus(UpdateType.Information, "{0}{1}", message, Environment.NewLine);
        }

        /// <summary>
        /// Sends an error message to all connected clients.
        /// </summary>
        /// <param name="errorMessage">The error message to be sent.</param>
        public void BroadcastError(string errorMessage)
        {
            m_serviceHelper.UpdateStatus(UpdateType.Alarm, "{0}{1}", errorMessage, Environment.NewLine);

            foreach (IServiceMonitor serviceMonitor in m_serviceMonitors.Adapters)
            {
                try
                {
                    if (serviceMonitor.Enabled)
                        serviceMonitor.HandleServiceErrorMessage(errorMessage);
                }
                catch (Exception ex)
                {
                    m_serviceHelper.ErrorLogger.Log(ex);
                    m_serviceHelper.UpdateStatus(UpdateType.Alarm, ex.Message + string.Format("{0}{0}", Environment.NewLine));
                }
            }
        }

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
        /// Sends an actionable response to client with a formatted message.
        /// </summary>
        /// <param name="requestInfo"><see cref="ClientRequestInfo"/> instance containing the client request.</param>
        /// <param name="success">Flag that determines if this response to client request was a success.</param>
        /// <param name="status">Formatted status message to send with response.</param>
        /// <param name="args">Arguments of the formatted status message.</param>
        protected virtual void SendResponse(ClientRequestInfo requestInfo, bool success, string status, params object[] args)
        {
            SendResponseWithAttachment(requestInfo, success, null, status, args);
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
        /// <param name="args">Arguments of the formatted status message.</param>
        protected virtual void DisplayResponseMessage(ClientRequestInfo requestInfo, string status, params object[] args)
        {
            try
            {
                m_serviceHelper.UpdateStatus(requestInfo.Sender.ClientID, UpdateType.Information, string.Format("{0}\r\n\r\n", status), args);
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
