//*********************************************************************************************************************
// ServiceHost.cs
// Version 1.1 and subsequent releases
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
// --------------------------------------------------------------------------------------------------------------------
//
// Version 1.0
//
// Copyright 2012 ELECTRIC POWER RESEARCH INSTITUTE, INC. All rights reserved.
//
// openXDA ("this software") is licensed under BSD 3-Clause license.
//
// Redistribution and use in source and binary forms, with or without modification, are permitted provided that the 
// following conditions are met:
//
// •    Redistributions of source code must retain the above copyright  notice, this list of conditions and 
//      the following disclaimer.
//
// •    Redistributions in binary form must reproduce the above copyright notice, this list of conditions and 
//      the following disclaimer in the documentation and/or other materials provided with the distribution.
//
// •    Neither the name of the Electric Power Research Institute, Inc. (“EPRI”) nor the names of its contributors 
//      may be used to endorse or promote products derived from this software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, 
// INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE 
// DISCLAIMED. IN NO EVENT SHALL EPRI BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL 
// DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; 
// OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, 
// OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
// POSSIBILITY OF SUCH DAMAGE.
//
//
// This software incorporates work covered by the following copyright and permission notice: 
//
// •    TVA Code Library 4.0.4.3 - Tennessee Valley Authority, tvainfo@tva.gov
//      No copyright is claimed pursuant to 17 USC § 105. All Other Rights Reserved.
//
//      Licensed under TVA Custom License based on NASA Open Source Agreement (TVA Custom NOSA); 
//      you may not use TVA Code Library except in compliance with the TVA Custom NOSA. You may  
//      obtain a copy of the TVA Custom NOSA at http://tvacodelibrary.codeplex.com/license.
//
//      TVA Code Library is provided by the copyright holders and contributors "as is" and any express 
//      or implied warranties, including, but not limited to, the implied warranties of merchantability 
//      and fitness for a particular purpose are disclaimed.
//
//*********************************************************************************************************************
//
//  Code Modification History:
//  -------------------------------------------------------------------------------------------------------------------
//  09/10/2012 - Stephen C. Wills, Grid Protection Alliance
//       Generated original version of source code.
//
//*********************************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using GSF;
using GSF.Configuration;
using GSF.Console;
using GSF.Data;
using GSF.IO;
using GSF.ServiceProcess;
using log4net.Appender;
using log4net.Config;
using log4net.Layout;
using openXDA.Logging;
using openXDA.Nodes;
using openXDA.Nodes.Types.FileProcessing;
using openXDA.WebHosting;

namespace openXDA
{
    public partial class ServiceHost : ServiceBase
    {
        #region [ Members ]

        // Nested Types
        private class CLIRegistry : ICLIRegistry
        {
            private ServiceHelper ServiceHelper { get; }

            public CLIRegistry(ServiceHelper serviceHelper) =>
                ServiceHelper = serviceHelper;

            public void RegisterScheduledProcess(Action process, string name, string schedule)
            {
                void ProcessExecutionMethod(string processName, object[] processArgs) => process();
                ServiceHelper.RemoveScheduledProcess(name);

                if (ServiceHelper.AddProcess(ProcessExecutionMethod, name))
                    ServiceHelper.ProcessScheduler.AddSchedule(name, schedule, true);
            }

            public void DeregisterScheduledProcess(string name) =>
                ServiceHelper.RemoveScheduledProcess(name);
        }

        #endregion

        #region [ Constructors ]

        public ServiceHost()
        {
            InitializeCultureSettings();
            InitializeComponent();
        }

        public ServiceHost(IContainer container)
            : this()
        {
            if (container != null)
                container.Add(this);
        }

        #endregion

        #region [ Properties ]

        public ServiceHelper Helper => m_serviceHelper;
        private ConfigurationFile ConfigurationFile { get; set; }
        private DatabaseConnectionFactory DatabaseConnectionFactory { get; set; }
        private ServiceMonitors ServiceMonitors { get; set; }
        private Host NodeHost { get; set; }
        private XDAWebHost WebHost { get; set; }

        #endregion

        #region [ Methods ]

        public async Task<string> QueryEngineStatusAsync()
        {
            using (AdoDataConnection connection = DatabaseConnectionFactory.CreateDbConnection())
            {
                string nodeConfigurationStatus = QueryNodeConfigurationStatus(connection);
                string fileNodeStatus = await QueryFileNodeStatusAsync(connection);
                string analysisStatus = QueryAnalysisStatus(connection);

                return new StringBuilder()
                    .AppendLine("=== NODE CONFIGURATION ===")
                    .AppendLine(nodeConfigurationStatus)
                    .AppendLine()
                    .AppendLine("=== FILE PROCESSORS ===")
                    .AppendLine(fileNodeStatus)
                    .AppendLine()
                    .AppendLine("=== DATA PROCESSORS ===")
                    .Append(analysisStatus)
                    .ToString();
            }
        }

        private void ServiceHelper_ServiceStarted(object sender, EventArgs e)
        {
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;

            // Set current working directory to fix relative paths
            Directory.SetCurrentDirectory(FilePath.GetAbsolutePath(""));

            InitializeLogging();

            ConfigurationFile = ConfigurationFile.Current;
            InitializeConfigurationFile();

            DatabaseConnectionFactory = new DatabaseConnectionFactory(ConfigurationFile);

            // Set up heartbeat and client request handlers
            m_serviceHelper.AddProcess(UpdateConfigurationHandler, "UpdateConfiguration");
            m_serviceHelper.AddProcess(ScanFilesHandler, "ScanFiles");
            m_serviceHelper.AddScheduledProcess(ServiceHeartbeatHandler, "ServiceHeartbeat", "* * * * *");
            m_serviceHelper.ClientRequestHandlers.Add(new ClientRequestHandler("Reconfigure", "Force host to reconfigure on demand", ReconfigureHandler));
            m_serviceHelper.ClientRequestHandlers.Add(new ClientRequestHandler("ReloadWebHost", "Reloads web host with latest configuration", ReloadWebHostHandler));
            m_serviceHelper.ClientRequestHandlers.Add(new ClientRequestHandler("EngineStatus", "Displays status information about file/analysis nodes", EngineStatusHandler));
            m_serviceHelper.ClientRequestHandlers.Add(new ClientRequestHandler("MsgServiceMonitors", "Sends a message to all service monitors", MsgServiceMonitorsHandler));

            // Set up adapter loader to load service monitors
            ServiceMonitors = new ServiceMonitors();
            ServiceMonitors.AdapterCreated += ServiceMonitors_AdapterCreated;
            ServiceMonitors.AdapterLoaded += ServiceMonitors_AdapterLoaded;
            ServiceMonitors.AdapterUnloaded += ServiceMonitors_AdapterUnloaded;
            ServiceMonitors.AdapterLoadException += (obj, args) => HandleException(args.Argument);
            ServiceMonitors.Initialize();

            // Initialize node host and web host in async loops to
            // maintain a robust startup sequence and avoid timeouts
            _ = Task.Run(async () =>
            {
                Func<AdoDataConnection> connectionFactory = DatabaseConnectionFactory.CreateDbConnection;
                CLIRegistry cliRegistry = new CLIRegistry(m_serviceHelper);
                NodeHost = await InitializeNodeHostAsync(connectionFactory, cliRegistry);
                WebHost = await InitializeWebHostAsync(NodeHost);
            });
        }

        private void ServiceHelper_ServiceStopping(object sender, EventArgs e)
        {
            // Dispose of adapter loader for service monitors
            ServiceMonitors.AdapterLoaded -= ServiceMonitors_AdapterLoaded;
            ServiceMonitors.AdapterUnloaded -= ServiceMonitors_AdapterUnloaded;
            ServiceMonitors.Dispose();

            // Save updated settings to the configuration file
            ConfigurationFile.Save();

            NodeHost?.Dispose();
            WebHost?.Dispose();
            components.Dispose();
            Dispose();
        }

        private void InitializeCultureSettings()
        {
            // Make sure default service settings exist
            ConfigurationFile configFile = ConfigurationFile.Current;
            CategorizedSettingsElementCollection systemSettings = configFile.Settings["systemSettings"];
            systemSettings.Add("DefaultCulture", "en-US", "Default culture to use for language, country/region and calendar formats.");

            // Attempt to set default culture
            string defaultCulture = systemSettings["DefaultCulture"].ValueAs("en-US");
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.CreateSpecificCulture(defaultCulture);     // Defaults for date formatting, etc.
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.CreateSpecificCulture(defaultCulture);   // Culture for resource strings, etc.
        }

        private void InitializeConfigurationFile()
        {
            CategorizedSettingsSection categorizedSettings = ConfigurationFile.Settings;
            CategorizedSettingsElementCollection systemSettings = categorizedSettings["systemSettings"];
            systemSettings.Add("ConnectionString", "Data Source=localhost; Initial Catalog=openXDA; Integrated Security=SSPI", "Defines the connection to the openXDA database.");
            systemSettings.Add("DataProviderString", "AssemblyName={System.Data, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089}; ConnectionType=System.Data.SqlClient.SqlConnection; AdapterType=System.Data.SqlClient.SqlDataAdapter", "Configuration database ADO.NET data provider assembly type creation string used when ConfigurationType=Database");
            systemSettings.Add("NodeID", "00000000-0000-0000-0000-000000000000", "Unique Node ID");
            systemSettings.Add("CompanyName", "Grid Protection Alliance", "The acronym representing the company who owns this instance of openXDA.");
            systemSettings.Add("CompanyAcronym", "GPA", "Default culture to use for language, country/region and calendar formats.");
            systemSettings.Add("DateFormat", "MM/dd/yyyy", "The date format to use when rendering timestamps.");
            systemSettings.Add("TimeFormat", "HH:mm.ss.fff", "The time format to use when rendering timestamps.");
            systemSettings.Add("ConfigurationCachePath", string.Format("{0}{1}ConfigurationCache{1}", FilePath.GetAbsolutePath(""), Path.DirectorySeparatorChar), "Defines the path used to cache serialized configurations");

            CategorizedSettingsElementCollection securityProvider = categorizedSettings["securityProvider"];
            securityProvider.Add("ConnectionString", "Eval(systemSettings.ConnectionString)", "Connection connection string to be used for connection to the backend security datastore.");
            securityProvider.Add("DataProviderString", "Eval(systemSettings.DataProviderString)", "Configuration database ADO.NET data provider assembly type creation string to be used for connection to the backend security datastore.");
        }

        private void InitializeLogging()
        {
            ServiceHelperAppender serviceHelperAppender = new ServiceHelperAppender(m_serviceHelper);

            RollingFileAppender debugLogAppender = new RollingFileAppender();
            debugLogAppender.StaticLogFileName = false;
            debugLogAppender.AppendToFile = true;
            debugLogAppender.RollingStyle = RollingFileAppender.RollingMode.Composite;
            debugLogAppender.MaxSizeRollBackups = 10;
            debugLogAppender.PreserveLogFileNameExtension = true;
            debugLogAppender.MaximumFileSize = "1MB";
            debugLogAppender.Layout = new PatternLayout("%date [%thread] %-5level %logger - %message%newline");
            debugLogAppender.AddFilter(new FileSkippedExceptionFilter());

            RollingFileAppender skippedFilesAppender = new RollingFileAppender();
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
        }

        private async Task<Host> InitializeNodeHostAsync(Func<AdoDataConnection> connectionFactory, ICLIRegistry cliRegistry)
        {
            while (true)
            {
                try
                {
                    ReloadConfigurationFile();

                    CategorizedSettingsSection categorizedSettings = ConfigurationFile.Settings;
                    CategorizedSettingsElementCollection systemSettings = categorizedSettings["systemSettings"];
                    CategorizedSettingsElement hostRegistrationKeySetting = systemSettings["HostRegistrationKey"];
                    string hostRegistrationKey = hostRegistrationKeySetting?.Value;

                    if (string.IsNullOrEmpty(hostRegistrationKey))
                        hostRegistrationKey = Environment.MachineName;

                    DatabaseConnectionFactory.ReloadConfiguration();
                    return new Host(hostRegistrationKey, connectionFactory, cliRegistry);
                }
                catch (Exception ex)
                {
                    HandleException(ex);
                }

                await Task.Delay(5000);
            }
        }

        private async Task<XDAWebHost> InitializeWebHostAsync(Host nodeHost)
        {
            ServiceConnection.InitializeDefaultInstance(this);

            while (true)
            {
                try
                {
                    ReloadConfigurationFile();
                    DatabaseConnectionFactory.ReloadConfiguration();
                    return new XDAWebHost(ConfigurationFile, nodeHost);
                }
                catch (Exception ex)
                {
                    HandleException(ex);
                }

                await Task.Delay(5000);
            }
        }

        private void UpdateConfigurationHandler(string s, object[] args)
        {
            ReloadConfigurationFile();
            DatabaseConnectionFactory.ReloadConfiguration();
            NodeHost?.Reconfigure();
        }

        private void ScanFilesHandler(string s, object[] args)
        {
            if (NodeHost is null)
                return;

            void ConfigureRequest(HttpRequestMessage request)
            {
                Type fileProcessorType = typeof(FileProcessorNode);
                string url = NodeHost.BuildURL(fileProcessorType, "Enumerate");
                request.Method = HttpMethod.Post;
                request.RequestUri = new Uri(url);
            }

            async Task ScanFilesAsync()
            {
                using (HttpResponseMessage response = await NodeHost.SendWebRequestAsync(ConfigureRequest))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        UpdateType alarm = UpdateType.Alarm;
                        string errorMessage = $"ERROR: {response.StatusCode} {response.ReasonPhrase}";
                        m_serviceHelper.UpdateStatus(alarm, "{0}", errorMessage);
                    }
                }
            }

            _ = ScanFilesAsync();
        }

        private void ServiceHeartbeatHandler(string s, object[] args)
        {
            // Go through all service monitors to notify of the heartbeat
            foreach (IServiceMonitor serviceMonitor in ServiceMonitors.Adapters)
            {
                try
                {
                    // If the service monitor is enabled, notify it of the heartbeat
                    if (serviceMonitor.Enabled)
                        serviceMonitor.HandleServiceHeartbeat();
                }
                catch (Exception ex)
                {
                    // Handle each service monitor's exceptions individually
                    HandleException(ex);
                }
            }
        }

        // Force the host to reconfigure on demand.
        private void ReconfigureHandler(ClientRequestInfo requestInfo)
        {
            ReloadConfigurationFile();
            DatabaseConnectionFactory.ReloadConfiguration();
            NodeHost?.Reconfigure();
            SendResponse(requestInfo, true);
        }

        // Reloads web host from configuration in the config file.
        private void ReloadWebHostHandler(ClientRequestInfo requestInfo)
        {
            WebHost?.Dispose();
            ReloadConfigurationFile();
            DatabaseConnectionFactory.ReloadConfiguration();
            WebHost = new XDAWebHost(ConfigurationFile, NodeHost);
            SendResponse(requestInfo, true);
        }

        // Displays status information about the XDA engine.
        private void EngineStatusHandler(ClientRequestInfo requestInfo)
        {
            Task<string> engineStatusTask = QueryEngineStatusAsync();
            string engineStatus = engineStatusTask.GetAwaiter().GetResult();
            DisplayResponseMessage(requestInfo, engineStatus);
            SendResponse(requestInfo, true);
        }

        // Send a message to the service monitors on request.
        private void MsgServiceMonitorsHandler(ClientRequestInfo requestInfo)
        {
            Arguments arguments = requestInfo.Request.Arguments;

            if (arguments.ContainsHelpRequest)
            {
                StringBuilder helpMessage = new StringBuilder();

                helpMessage.Append("Sends a message to all service monitors.");
                helpMessage.AppendLine();
                helpMessage.AppendLine();
                helpMessage.Append("   Usage:");
                helpMessage.AppendLine();
                helpMessage.Append("       MsgServiceMonitors [Options] [Args...]");
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
                string[] args = Enumerable.Range(1, arguments.OrderedArgCount)
                    .Select(arg => arguments[arguments.OrderedArgID + arg])
                    .ToArray();

                // Go through all service monitors and handle the message
                foreach (IServiceMonitor serviceMonitor in ServiceMonitors.Adapters)
                {
                    try
                    {
                        // If the service monitor is enabled, notify it of the message
                        if (serviceMonitor.Enabled)
                            serviceMonitor.HandleClientMessage(args);
                    }
                    catch (Exception ex)
                    {
                        // Handle each service monitor's exceptions individually
                        HandleException(ex);
                    }
                }

                SendResponse(requestInfo, true);
            }
        }

        private void ReloadConfigurationFile()
        {
            ConfigurationFile.Reload();
            InitializeConfigurationFile();
        }

        // Send the error to the service helper, error logger, and each service monitor
        private void HandleException(Exception ex)
        {
            string newLines = string.Format("{0}{0}", Environment.NewLine);

            m_serviceHelper.ErrorLogger.Log(ex);
            m_serviceHelper.UpdateStatus(UpdateType.Alarm, "{0}", ex.Message + newLines);

            foreach (IServiceMonitor serviceMonitor in ServiceMonitors.Adapters)
            {
                try
                {
                    if (serviceMonitor.Enabled)
                        serviceMonitor.HandleServiceError(ex);
                }
                catch (Exception ex2)
                {
                    // Exceptions encountered while handling exceptions can be tricky,
                    // so we just log them rather than risk a recursive loop
                    m_serviceHelper.ErrorLogger.Log(ex2);
                    m_serviceHelper.UpdateStatus(UpdateType.Alarm, ex2.Message + newLines);
                }
            }
        }

        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            foreach (Exception ex in e.Exception.Flatten().InnerExceptions)
                HandleException(ex);

            e.SetObserved();
        }

        private string QueryNodeConfigurationStatus(AdoDataConnection connection)
        {
            const string Query =
                "SELECT " +
                "    ActiveHost.RegistrationKey HostKey, " +
                "    ActiveHost.URL HostURL, " +
                "    Node.ID NodeID, " +
                "    Node.Name NodeName, " +
                "    NodeType.Name NodeType " +
                "FROM " +
                "    ActiveHost JOIN " +
                "    Node ON Node.HostRegistrationID = ActiveHost.ID JOIN " +
                "    NodeType ON Node.NodeTypeID = NodeType.ID";

            using (DataTable result = connection.RetrieveData(Query))
            {
                IEnumerable<string> hostInfos = result
                    .AsEnumerable()
                    .Select(row => new
                    {
                        Host = new
                        {
                            Key = row.ConvertField<string>("HostKey"),
                            URL = row.ConvertField<string>("HostURL")
                        },
                        Node = new
                        {
                            ID = row.ConvertField<int>("NodeID"),
                            Name = row.ConvertField<string>("NodeName"),
                            Type = row.ConvertField<string>("NodeType")
                        }
                    })
                    .GroupBy(record => record.Host, record => record.Node)
                    .Select(grouping =>
                    {
                        var host = grouping.Key;

                        IEnumerable<string> lines = grouping
                            .Select(node => $"    {node.Name} <{node.Type}> ({node.ID})")
                            .Prepend($"{host.Key} ({host.URL})");

                        return string.Join(Environment.NewLine, lines);
                    });

                string doubleLineSeparator = string.Format("{0}{0}", Environment.NewLine);
                return string.Join(doubleLineSeparator, hostInfos);
            }
        }

        private async Task<string> QueryFileNodeStatusAsync(AdoDataConnection connection)
        {
            if (NodeHost is null)
                return string.Empty;

            const string QueryFormat =
                "SELECT " +
                "    ActiveHost.URL HostURL, " +
                "    Node.ID NodeID, " +
                "    Node.Name NodeName " +
                "FROM " +
                "    ActiveHost JOIN " +
                "    Node ON Node.HostRegistrationID = ActiveHost.ID JOIN " +
                "    NodeType ON Node.NodeTypeID = NodeType.ID " +
                "WHERE NodeType.TypeName = {0}";

            Type nodeType = typeof(FileProcessorNode);
            string nodeTypeName = nodeType.FullName;

            using (DataTable result = connection.RetrieveData(QueryFormat, nodeTypeName))
            {
                async Task<string> QueryAsync(string url)
                {
                    void ConfigureRequest(HttpRequestMessage request) =>
                        request.RequestUri = new Uri(url);

                    using (HttpResponseMessage response = await NodeHost.SendWebRequestAsync(ConfigureRequest))
                    {
                        if (!response.IsSuccessStatusCode)
                            return $"ERROR: [{response.StatusCode}] {response.ReasonPhrase}";

                        try { return await response.Content.ReadAsStringAsync(); }
                        catch (Exception ex) { return ex.ToString(); }
                    }
                }

                // Query the status of each active file node
                var nodeQueries = result
                    .AsEnumerable()
                    .Select(row =>
                    {
                        int nodeID = row.ConvertField<int>("NodeID");
                        string nodeName = row.ConvertField<string>("NodeName");

                        string hostURL = row.ConvertField<string>("HostURL");
                        string cleanHostURL = hostURL.Trim().TrimEnd('/');
                        string action = "Status";
                        string url = $"{cleanHostURL}/Node/{nodeID}/{action}";
                        Task<string> queryStatusTask = QueryAsync(url);

                        return new
                        {
                            NodeID = nodeID,
                            NodeName = nodeName,
                            QueryStatusTask = queryStatusTask
                        };
                    })
                    .ToList();

                // Wait for all queries to finish
                IEnumerable<Task<string>> queryTasks = nodeQueries.Select(obj => obj.QueryStatusTask);
                await Task.WhenAll(queryTasks);

                IEnumerable<string> fileNodeInfos = nodeQueries
                    .Select(query =>
                    {
                        int nodeID = query.NodeID;
                        string nodeName = query.NodeName;
                        Task<string> queryStatusTask = query.QueryStatusTask;
                        string header = $"{nodeName} ({nodeID}) Status:";
                        string status = queryStatusTask.GetAwaiter().GetResult();
                        return string.Join(Environment.NewLine, header, status);
                    });

                string doubleLineSeparator = string.Format("{0}{0}", Environment.NewLine);
                return string.Join(doubleLineSeparator, fileNodeInfos);
            }
        }

        private string QueryAnalysisStatus(AdoDataConnection connection)
        {
            if (NodeHost is null)
                return string.Empty;

            const string Query =
                "SELECT " +
                "    FileGroup.ProcessingStartTime, " +
                "    DataFile.FilePath, " +
                "    Meter.AssetKey MeterKey, " +
                "    Meter.Name MeterName, " +
                "    Node.ID NodeID, " +
                "    Node.Name NodeName " +
                "FROM " +
                "    AnalysisTask JOIN " +
                "    FileGroup ON AnalysisTask.FileGroupID = FileGroup.ID CROSS APPLY " +
                "    (SELECT TOP 1 * FROM DataFile WHERE FileGroupID = FileGroup.ID ORDER BY FileSize DESC, ID) DataFile JOIN " +
                "    Meter ON AnalysisTask.MeterID = Meter.ID JOIN " +
                "    Node ON AnalysisTask.NodeID = Node.ID";

            int totalCount = connection.ExecuteScalar<int>("SELECT COUNT(*) FROM AnalysisTask");

            using (DataTable result = connection.RetrieveData(Query))
            {
                int hostID = NodeHost.ID;
                Func<AdoDataConnection> connectionFactory = DatabaseConnectionFactory.CreateDbConnection;
                ConfigurationLoader configurationLoader = new ConfigurationLoader(hostID, connectionFactory);

                Action<object> configurator = configurationLoader.Configure;
                TimeZoneConverter timeZoneConverter = new TimeZoneConverter(configurator);
                DateTime now = DateTime.UtcNow;
                DateTime xdaNow = timeZoneConverter.ToXDATimeZone(now);

                IEnumerable<string> analysisNodeInfos = result
                    .AsEnumerable()
                    .Select(row => new
                    {
                        Node = new
                        {
                            ID = row.ConvertField<int>("NodeID"),
                            Name = row.ConvertField<string>("NodeName")
                        },
                        AnalysisTask = new
                        {
                            ProcessingStartTime = row.ConvertField<DateTime>("ProcessingStartTime"),
                            FilePath = row.ConvertField<string>("FilePath"),
                            MeterKey = row.ConvertField<string>("MeterKey"),
                            MeterName = row.ConvertField<string>("MeterName")
                        }
                    })
                    .GroupBy(record => record.Node, record => record.AnalysisTask)
                    .Select(grouping =>
                    {
                        string ToIdentification(string meterKey, string meterName) =>
                            meterKey != meterName
                                ? $"{meterName} ({meterKey})"
                                : meterName;

                        var node = grouping.Key;
                        int nodeID = node.ID;
                        string nodeName = node.Name;

                        IEnumerable<string> lines = grouping
                            .Select(analysisTask =>
                            {
                                DateTime processingStartTime = analysisTask.ProcessingStartTime;
                                string filePath = analysisTask.FilePath;
                                string meterKey = analysisTask.MeterKey;
                                string meterName = analysisTask.MeterName;

                                TimeSpan processingTime = xdaNow - processingStartTime;
                                string fileName = Path.GetFileName(filePath);
                                string meterIdentification = ToIdentification(meterKey, meterName);
                                return $"    [{processingTime}] from {meterIdentification} - {fileName}";
                            })
                            .Prepend($"Node {nodeName} ({nodeID}):");

                        return string.Join(Environment.NewLine, lines);
                    })
                    .Prepend($"Total queued analysis tasks: {totalCount}");

                string doubleLineSeparator = string.Format("{0}{0}", Environment.NewLine);
                return string.Join(doubleLineSeparator, analysisNodeInfos);
            }
        }

        #region [ Service Monitor Handlers ]

        // Ensure that service monitors save their settings to the configuration file
        private void ServiceMonitors_AdapterCreated(object sender, EventArgs<IServiceMonitor> e)
        {
            e.Argument.PersistSettings = true;
        }

        // Display a message when service monitors are loaded
        private void ServiceMonitors_AdapterLoaded(object sender, EventArgs<IServiceMonitor> e)
        {
            m_serviceHelper.UpdateStatusAppendLine(UpdateType.Information, "{0} has been loaded", e.Argument.GetType().Name);
            e.Argument.StatusUpdate += ServiceMonitor_StatusUpdate;
            e.Argument.ExecutionException += ServiceMonitor_ExecutionException;
        }

        // Display a message when service monitors are unloaded
        private void ServiceMonitors_AdapterUnloaded(object sender, EventArgs<IServiceMonitor> e)
        {
            m_serviceHelper.UpdateStatusAppendLine(UpdateType.Information, "{0} has been unloaded", e.Argument.GetType().Name);
        }

        // Handle updates from service monitors.
        private void ServiceMonitor_StatusUpdate(object sender, EventArgs<UpdateType, string> e)
        {
            IServiceMonitor serviceMonitor = sender as IServiceMonitor;

            if (serviceMonitor?.Enabled ?? false)
                m_serviceHelper.UpdateStatus(e.Argument1, "{0}", e.Argument2);
        }

        // Handle exceptions thrown by service monitors.
        private void ServiceMonitor_ExecutionException(object sender, EventArgs<string, Exception> e)
        {
            IServiceMonitor serviceMonitor = sender as IServiceMonitor;

            if (serviceMonitor?.Enabled ?? false)
            {
                Exception ex = e.Argument2;
                string newLines = string.Format("{0}{0}", Environment.NewLine);
                m_serviceHelper.ErrorLogger.Log(ex);
                m_serviceHelper.UpdateStatus(UpdateType.Alarm, "{0}: {1}", e.Argument1, ex.Message + newLines);
            }
        }

        #endregion

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

                    if (!(status is null))
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
