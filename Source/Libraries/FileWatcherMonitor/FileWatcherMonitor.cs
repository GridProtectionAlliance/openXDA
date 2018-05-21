﻿//******************************************************************************************************
//  FileWatcherMonitor.cs - Gbtc
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
//  05/11/2018 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Threading;
using GSF;
using GSF.Communication;
using GSF.Configuration;
using GSF.IO;
using GSF.ServiceProcess;
using GSF.Threading;

namespace FileWatcherMonitor
{
    public class FileWatcherMonitor : ServiceMonitorBase
    {
        #region [ Members ]

        // Nested Types

        private class DelayedSynchronizedOperation : SynchronizedOperationBase
        {
            private Action m_delayedAction;

            public DelayedSynchronizedOperation(Action action, Action<Exception> exceptionAction)
                : base(action, exceptionAction)
            {
                m_delayedAction = () =>
                {
                    if (ExecuteAction())
                        ExecuteActionAsync();
                };
            }

            public int Delay { get; set; }

            protected override void ExecuteActionAsync()
            {
                m_delayedAction.DelayAndExecute(Delay);
            }
        }

        // Fields
        private string m_engineStatus;
        private int m_enumeratingCount;
        private DelayedSynchronizedOperation m_checkEngineStatus;
        private Random m_random;

        #endregion

        #region [ Constructors ]

        public FileWatcherMonitor()
        {
            Action<Exception> exceptionHandler = ex => OnExecutionException("CheckEngineStatus", ex);
            m_checkEngineStatus = new DelayedSynchronizedOperation(TryCheckEngineStatus, exceptionHandler);
            m_random = new Random();
        }

        #endregion

        #region [ Properties ]

        public override bool Enabled
        {
            get
            {
                return base.Enabled;
            }
            set
            {
                base.Enabled = value;

                if (value)
                    Start();
            }
        }

        #endregion

        #region [ Methods ]

        public override void HandleClientMessage(string[] args)
        {
            if (args.Length == 0)
                return;

            if (string.Equals(args[0], "EngineStatus", StringComparison.OrdinalIgnoreCase))
                OnStatusUpdate(UpdateType.Information, "{0}", m_engineStatus);

            if (string.Equals(args[0], "RestartFileWatcher", StringComparison.OrdinalIgnoreCase))
                RestartFileWatcher();
        }

        private void Start()
        {
            QueueCheckEngineStatus();
        }

        private void QueueCheckEngineStatus()
        {
            const int MinDelay = 5 * 60 * 1000;     // 5 minutes in milliseconds
            const int MaxDelay = 7 * 60 * 1000;     // 7 minutes in milliseconds
            int delay = m_random.Next(MinDelay, MaxDelay);
            m_checkEngineStatus.Delay = delay;
            m_checkEngineStatus.RunOnceAsync();
        }

        private void TryCheckEngineStatus()
        {
            if (!Enabled)
                return;

            try { CheckEngineStatus(); }
            catch (Exception ex) { OnExecutionException(nameof(CheckEngineStatus), ex); }
            QueueCheckEngineStatus();
        }

        private void CheckEngineStatus()
        {
            string configurationPath = Path.Combine("FileWatcher", "openXDAFileWatcherConsole.exe.config");
            string absolutePath = FilePath.GetAbsolutePath(configurationPath);
            ConfigurationFile configurationFile = ConfigurationFile.Open(absolutePath);
            CategorizedSettingsElementCollection remotingClientSettings = configurationFile.Settings["remotingClient"];

            string connectionString = remotingClientSettings["ConnectionString"]?.Value ?? "Server=localhost:9999";
            string integratedSecurity = remotingClientSettings["IntegratedSecurity"]?.Value ?? "True";

            using (ClientHelper helper = new ClientHelper())
            using (TcpClient remotingClient = new TcpClient())
            {
                string engineStatus = "";
                ManualResetEvent waitHandle = new ManualResetEvent(false);

                remotingClient.ConnectionString = connectionString;
                remotingClient.IntegratedSecurity = integratedSecurity.ParseBoolean();
                remotingClient.PayloadAware = true;

                helper.StatusMessageFilter = "FILTER -Exclude Message \"\"";
                helper.RemotingClient = remotingClient;
                helper.AuthenticationFailure += (sender, args) => waitHandle.Set();

                helper.AuthenticationSuccess += (sender, args) =>
                {
                    EventHandler<EventArgs<UpdateType, string>> handler = null;

                    handler = (obj, e) =>
                    {
                        helper.ReceivedServiceUpdate -= handler;
                        engineStatus = e.Argument2;
                        waitHandle.Set();
                    };

                    helper.ReceivedServiceUpdate += handler;
                    helper.SendRequest("EngineStatus");
                };

                helper.ReceivedServiceResponse += (sender, args) =>
                {
                    engineStatus = args.Argument.Message;
                    waitHandle.Set();
                };

                helper.Connect();
                waitHandle.WaitOne();

                if (string.IsNullOrEmpty(engineStatus))
                    return;

                bool isEnumerating = engineStatus
                    .Split('\r', '\n')
                    .Where(line => line.Trim().StartsWith("Is Enumerating: "))
                    .Select(line => line.Split(':')[1].Trim())
                    .Select(value => value.ParseBoolean())
                    .DefaultIfEmpty(false)
                    .First();

                if (!isEnumerating)
                    m_enumeratingCount = 0;
                else if (engineStatus != m_engineStatus)
                    m_enumeratingCount = 1;
                else
                    m_enumeratingCount++;

                if (m_enumeratingCount >= 3)
                {
                    RestartFileWatcher();
                    m_enumeratingCount = 0;
                }

                m_engineStatus = engineStatus;
            }
        }

        private void RestartFileWatcher()
        {
            const string FileWatcherServiceName = "openXDAFileWatcher";

            // Attempt to access service controller for the specified Windows service
            ServiceController serviceController = ServiceController.GetServices()
                .SingleOrDefault(svc => string.Equals(svc.ServiceName, FileWatcherServiceName, StringComparison.OrdinalIgnoreCase));

            if (serviceController != null)
            {
                try
                {
                    if (serviceController.Status == ServiceControllerStatus.Running)
                    {
                        OnStatusUpdate(UpdateType.Information, "Attempting to stop the {0} Windows service...", FileWatcherServiceName);

                        serviceController.Stop();

                        // Can't wait forever for service to stop, so we time-out after 20 seconds
                        serviceController.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(20.0D));

                        if (serviceController.Status == ServiceControllerStatus.Stopped)
                            OnStatusUpdate(UpdateType.Information, "Successfully stopped the {0} Windows service.", FileWatcherServiceName);
                        else
                            OnStatusUpdate(UpdateType.Information, "Failed to stop the {0} Windows service after trying for 20 seconds...", FileWatcherServiceName);

                        // Add an extra line for visual separation of service termination status
                        OnStatusUpdate(UpdateType.Information, "");
                    }
                }
                catch (Exception ex)
                {
                    OnExecutionException($"Failed to stop the {FileWatcherServiceName} Windows service", ex);
                }
            }

            // If the service failed to stop or it is installed as stand-alone debug application, we try to forcibly stop any remaining running instances
            try
            {
                Process[] instances = Process.GetProcessesByName(FileWatcherServiceName);

                if (instances.Length > 0)
                {
                    int total = 0;
                    OnStatusUpdate(UpdateType.Information, "Attempting to stop running instances of the {0}...", FileWatcherServiceName);

                    // Terminate all instances of service running on the local computer
                    foreach (Process process in instances)
                    {
                        process.Kill();
                        total++;
                    }

                    if (total > 0)
                        OnStatusUpdate(UpdateType.Information, "Stopped {0} {1} instance{2}.", total, FileWatcherServiceName, total > 1 ? "s" : "");

                    // Add an extra line for visual separation of process termination status
                    OnStatusUpdate(UpdateType.Information, "");
                }
            }
            catch (Exception ex)
            {
                OnExecutionException($"Failed to terminate running instances of the {FileWatcherServiceName}", ex);
            }

            // Attempt to restart Windows service...
            if (serviceController != null)
            {
                try
                {
                    // Refresh state in case service process was forcibly stopped
                    serviceController.Refresh();

                    if (serviceController.Status != ServiceControllerStatus.Running)
                        serviceController.Start();
                }
                catch (Exception ex)
                {
                    OnExecutionException($"Failed to restart the {FileWatcherServiceName} Windows service", ex);
                }
            }
        }

        #endregion
    }
}
