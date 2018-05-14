//******************************************************************************************************
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
using System.IO;
using System.Linq;
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

            if (!string.Equals(args[0], "PrintEngineStatus", StringComparison.OrdinalIgnoreCase))
                return;

            OnStatusUpdate(UpdateType.Information, "{0}", m_engineStatus);
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
                    helper.SendRequest("Restart");
                    m_enumeratingCount = 0;
                }

                m_engineStatus = engineStatus;
            }
        }

        #endregion
    }
}
