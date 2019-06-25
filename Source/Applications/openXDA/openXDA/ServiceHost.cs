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
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GSF;
using GSF.Collections;
using GSF.Configuration;
using GSF.Console;
using GSF.Data;
using GSF.Data.Model;
using GSF.Identity;
using GSF.IO;
using GSF.Reflection;
using GSF.Security;
using GSF.Security.Model;
using GSF.ServiceProcess;
using GSF.Web.Hosting;
using GSF.Web.Model;
using GSF.Web.Security;
using log4net.Appender;
using log4net.Config;
using log4net.Layout;
using Microsoft.Owin.Hosting;
using openXDA.Adapters;
using openXDA.Configuration;
using openXDA.DataPusher;
using openXDA.Logging;
using openXDA.Model;
using openXDA.Reports;
using openXDA.PQTrendingWebReport;
using PQMark.DataAggregator;
using Channel = openXDA.Model.Channel;
using DataHub = openXDA.Hubs.DataHub;
using Meter = openXDA.Model.Meter;
using MeterLine = openXDA.Model.MeterLine;
using MeterLocation = openXDA.Model.MeterLocation;
using MeterAssetGroup = openXDA.Model.MeterAssetGroup;
using Setting = openXDA.Model.Setting;
using openXDA.StepChangeWebReport;
using System.Security;
using System.Net;

namespace openXDA
{
    public partial class ServiceHost : ServiceBase
    {
        #region [ Members ]

        // Events

        /// <summary>
        /// Raised when there is a new status message reported to service.
        /// </summary>
        public event EventHandler<EventArgs<Guid, string, UpdateType>> UpdatedStatus;

        /// <summary>
        /// Raised when there is a new exception logged to service.
        /// </summary>
        public event EventHandler<EventArgs<Exception>> LoggedException;

        // Fields
        private ServiceMonitors m_serviceMonitors;
        private ExtensibleDisturbanceAnalysisEngine m_extensibleDisturbanceAnalysisEngine;
        private DataPusherEngine m_dataPusherEngine;
        private DataAggregationEngine m_dataAggregationEngine;
        private ReportsEngine m_reportsEngine;
        private PQTrendingWebReportEngine m_pqTrendingWebReportEngine;
        private StepChangeWebReportEngine m_stepChangeWebReportEngine;
        private Thread m_startEngineThread;
        private bool m_serviceStopping;
        private IDisposable m_webAppHost;
        private bool m_disposed;
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

        public ServiceHost(IContainer container)
            : this()
        {
            if (container != null)
                container.Add(this);
        }

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets the configured default web page for the application.
        /// </summary>
        public string DefaultWebPage
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the model used for the application.
        /// </summary>
        public AppModel Model
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets current performance statistics.
        /// </summary>
        public string PerformanceStatistics => m_extensibleDisturbanceAnalysisEngine.Status;
        #endregion

        #region [ Methods ]

        private void WebServer_StatusMessage(object sender, EventArgs<string> e)
        {
            //DisplayStatusMessage(e.Argument, UpdateType.Information);
        }

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
            m_serviceHelper.AddScheduledProcess(ServiceHeartbeatHandler, "ServiceHeartbeat", "* * * * *");
            m_serviceHelper.AddScheduledProcess(ReloadConfigurationHandler, "ReloadConfiguration", "0 0 * * *");
            m_serviceHelper.AddProcess(EnumerateWatchDirectoriesHandler, "EnumWatchDirectories");
            m_serviceHelper.AddProcess(AutoFileDeletionHandler, "AutoFileDeletion");
            m_serviceHelper.ClientRequestHandlers.Add(new ClientRequestHandler("ReloadSystemSettings", "Reloads system settings from the database", ReloadSystemSettingsRequestHandler));
            m_serviceHelper.ClientRequestHandlers.Add(new ClientRequestHandler("EngineStatus", "Displays status information about the XDA engine", EngineStatusHandler));
            m_serviceHelper.ClientRequestHandlers.Add(new ClientRequestHandler("TweakFileProcessor", "Modifies the behavior of the file processor at runtime", TweakFileProcessorHandler));
            m_serviceHelper.ClientRequestHandlers.Add(new ClientRequestHandler("RestoreEventEmails", "Restores event email engine to a working state tripping", RestoreEventEmails));
            m_serviceHelper.ClientRequestHandlers.Add(new ClientRequestHandler("MsgServiceMonitors", "Sends a message to all service monitors", MsgServiceMonitorsRequestHandler));
            m_serviceHelper.ClientRequestHandlers.Add(new ClientRequestHandler("PurgeData", "Deletes data from database beyond a sepecified date", PurgeDataHandler));
            m_serviceHelper.ClientRequestHandlers.Add(new ClientRequestHandler("PQMarkProcAD", "Creates aggregates for all data", OnProcessAllData));
            m_serviceHelper.ClientRequestHandlers.Add(new ClientRequestHandler("PQMarkProcED", "Creates aggregates for missing monthly data", OnProcessAllEmptyData));
            m_serviceHelper.ClientRequestHandlers.Add(new ClientRequestHandler("PQMarkProcMTD", "Creates aggregates for month to date data", OnProcessMonthToDateData));
            m_serviceHelper.ClientRequestHandlers.Add(new ClientRequestHandler("PQTrendingProcess", "Processes data for PQTrending Web Report", OnProcessPQTrending));
            m_serviceHelper.ClientRequestHandlers.Add(new ClientRequestHandler("StepChangeProcess", "Processes data for Step Change Web Report", OnProcessStepChange));

            m_serviceHelper.UpdatedStatus += UpdatedStatusHandler;
            m_serviceHelper.LoggedException += LoggedExceptionHandler;

            // Set up adapter loader to load service monitors
            m_serviceMonitors = new ServiceMonitors();
            m_serviceMonitors.AdapterCreated += ServiceMonitors_AdapterCreated;
            m_serviceMonitors.AdapterLoaded += ServiceMonitors_AdapterLoaded;
            m_serviceMonitors.AdapterUnloaded += ServiceMonitors_AdapterUnloaded;
            m_serviceMonitors.AdapterLoadException += (obj, args) => HandleException(args.Argument);
            m_serviceMonitors.Initialize();

            string systemSettingsConnectionString = LoadSystemSettings();

            // Set up the analysis engine
            m_extensibleDisturbanceAnalysisEngine = new ExtensibleDisturbanceAnalysisEngine();

            // Set up data pusher engine
            m_dataPusherEngine = new DataPusherEngine();
            ConnectionStringParser.ParseConnectionString(systemSettingsConnectionString, m_dataPusherEngine);

            // Set up data aggregation engine
            m_dataAggregationEngine = new DataAggregationEngine();
            ConnectionStringParser.ParseConnectionString(systemSettingsConnectionString, m_dataAggregationEngine);

            // Set up data reports engine
            m_reportsEngine = new ReportsEngine();
            ConnectionStringParser.ParseConnectionString(systemSettingsConnectionString, m_reportsEngine);

            // Set up data reports engine
            m_pqTrendingWebReportEngine = new PQTrendingWebReportEngine();
            ConnectionStringParser.ParseConnectionString(systemSettingsConnectionString, m_pqTrendingWebReportEngine);

            // Set up data reports engine
            m_stepChangeWebReportEngine = new StepChangeWebReportEngine();
            ConnectionStringParser.ParseConnectionString(systemSettingsConnectionString, m_stepChangeWebReportEngine);


            //Set up datahub callbacks
            DataHub.LogStatusMessageEvent += (obj, Args) => LogStatusMessage(Args.Argument1, Args.Argument2);
            DataHub.ReprocessFilesEvent += (obj, Args) => ReprocessFile(Args.Argument1, Args.Argument2);
            DataHub.ReloadSystemSettingsEvent += (obj, Args) => OnReloadSystemSettingsRequestHandler();
            DataHub.LogExceptionMessage += (obj, Args) => LoggedExceptionHandler(obj, Args);

            //Set up DataPusherEngine callbacks
            DataPusherEngine.LogExceptionMessage += (obj, Args) => LoggedExceptionHandler(obj, Args);
            DataPusherEngine.LogStatusMessageEvent += (obj, Args) => LogStatusMessage(Args.Argument);
            DataPusherEngine.UpdateProgressForMeter += (obj, Args) => DataHub.ProgressUpdatedByMeter(obj, Args);
            DataPusherEngine.UpdateProgressForInstance += (obj, Args) => DataHub.ProgressUpdatedByInstance(obj, Args);

            //Set up PQMarkController callbacks
            PQMarkController.ReprocessFilesEvent += (obj, Args) => ReprocessFile(Args.Argument1, Args.Argument2);

            //Set up DataAggregationEngine callbacks
            DataAggregationEngine.LogExceptionMessage += (obj, Args) => LoggedExceptionHandler(obj, Args);
            DataAggregationEngine.LogStatusMessageEvent += (obj, Args) => LogStatusMessage(Args.Argument);

            // Set up separate thread to start the engine
            m_startEngineThread = new Thread(() =>
            {
                const int RetryDelay = 1000;
                const int SleepTime = 200;
                const int LoopCount = RetryDelay / SleepTime;

                bool engineStarted = false;
                bool webUIStarted = false;
                bool dataPusherEngineStarted = false;
                bool dataAggregationEngineStarted = false;
                bool reportsEngineStarted = false;
                bool pqTrendingWebReportsEngineStarted = false;
                bool statChangeWebReportsEngineStarted = false;

                while (true)
                {
                    engineStarted = engineStarted || TryStartEngine();
                    webUIStarted = webUIStarted || TryStartWebUI();
                    dataPusherEngineStarted = dataPusherEngineStarted || TryStartDataPusherEngine();
                    dataAggregationEngineStarted = dataAggregationEngineStarted || TryStartDataAggregationEngine();
                    reportsEngineStarted = m_reportsEngine.Start();
                    pqTrendingWebReportsEngineStarted = pqTrendingWebReportsEngineStarted || TryStartPQTrendingWebReportsEngine();
                    statChangeWebReportsEngineStarted = statChangeWebReportsEngineStarted || TryStartStepChangeWebReportsEngine();


                    if (engineStarted && webUIStarted && dataPusherEngineStarted && reportsEngineStarted && pqTrendingWebReportsEngineStarted && statChangeWebReportsEngineStarted)
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
            if (!m_disposed)
            {
                try
                {
                   m_webAppHost?.Dispose();
                }
                finally
                {
                    m_disposed = true;          // Prevent duplicate dispose.
                    base.Dispose();    // Call base class Dispose().
                }
            }

            // If the start engine thread is still
            // running, wait for it to stop
            m_serviceStopping = true;
            m_startEngineThread.Join();
            m_serviceHelper.UpdatedStatus -= UpdatedStatusHandler;
            m_serviceHelper.LoggedException -= LoggedExceptionHandler;

            // Dispose of adapter loader for service monitors
            m_serviceMonitors.AdapterLoaded -= ServiceMonitors_AdapterLoaded;
            m_serviceMonitors.AdapterUnloaded -= ServiceMonitors_AdapterUnloaded;
            m_serviceMonitors.Dispose();

            // Dispose of the analysis engine
            m_extensibleDisturbanceAnalysisEngine.Stop();
            m_extensibleDisturbanceAnalysisEngine.Dispose();

            // Dispose of the data pusher engine
            m_dataPusherEngine.Stop();
            m_dataPusherEngine.Dispose();

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
                m_extensibleDisturbanceAnalysisEngine.Start();
                return true;
            }
            catch (Exception ex)
            {
                string message;

                // Stop the analysis engine
                m_extensibleDisturbanceAnalysisEngine.Stop();

                // Log the exception
                message = "Failed to start XDA engine due to exception: " + ex.Message;
                HandleException(new InvalidOperationException(message, ex));

                return false;
            }
        }

        // Attempts to start the engine and logs startup errors.
        private bool TryStartDataPusherEngine()
        {
            try
            {
                if (m_dataPusherEngine.DataPusherSettings.Enabled)
                    m_dataPusherEngine.Start();
                return true;
            }
            catch (Exception ex)
            {
                string message;

                // Stop the analysis engine
                m_dataPusherEngine.Stop();

                // Log the exception
                message = "Failed to start DataPusher engine due to exception: " + ex.Message;
                HandleException(new InvalidOperationException(message, ex));

                return false;
            }
        }

        // Attempts to start the engine and logs startup errors.
        private bool TryStartDataAggregationEngine()
        {
            try
            {
                // Start the analysis engine
                if (m_dataAggregationEngine.PQMarkAggregationSettings.Enabled)
                    m_dataAggregationEngine.Start();
                return true;
            }
            catch (Exception ex)
            {
                string message;

                // Stop the analysis engine
                m_dataAggregationEngine.Stop();

                // Log the exception
                message = "Failed to start PQMark data aggregation engine due to exception: " + ex.Message;
                HandleException(new InvalidOperationException(message, ex));

                return false;
            }
        }

        // Attempts to start the engine and logs startup errors.
        private bool TryStartPQTrendingWebReportsEngine()
        {
            try
            {
                // Start the analysis engine
                if (m_pqTrendingWebReportEngine.PQTrendingWebReportSettings.Enabled)
                    m_pqTrendingWebReportEngine.Start();
                return true;
            }
            catch (Exception ex)
            {
                string message;

                // Stop the analysis engine
                m_pqTrendingWebReportEngine.Stop();

                // Log the exception
                message = "Failed to start reports engine due to exception: " + ex.Message;
                HandleException(new InvalidOperationException(message, ex));

                return false;
            }
        }

        // Attempts to start the engine and logs startup errors.
        private bool TryStartStepChangeWebReportsEngine()
        {
            try
            {
                // Start the analysis engine
                if (m_stepChangeWebReportEngine.StepChangeWebReportSettings.Enabled)
                    m_stepChangeWebReportEngine.Start();
                return true;
            }
            catch (Exception ex)
            {
                string message;

                // Stop the analysis engine
                m_stepChangeWebReportEngine.Stop();

                // Log the exception
                message = "Failed to start reports engine due to exception: " + ex.Message;
                HandleException(new InvalidOperationException(message, ex));

                return false;
            }
        }


        // Attempts to start the web UI and logs startup errors.
        private bool TryStartWebUI()
        {
            // Define set of default anonymous web resources for this site
            const string DefaultAnonymousResourceExpression = "^/@|^/Scripts/|^/Content/|^/Images/|^/fonts/|^/favicon.ico$";
            const string DefaultAuthFailureRedirectResourceExpression = AuthenticationOptions.DefaultAuthFailureRedirectResourceExpression + "|^/grafana(?!/api/).*$";

            try
            {
                ConfigurationFile.Current.Reload();
                AdoDataConnection.ReloadConfigurationSettings();

                CategorizedSettingsElementCollection systemSettings = ConfigurationFile.Current.Settings["systemSettings"];
                CategorizedSettingsElementCollection securityProvider = ConfigurationFile.Current.Settings["securityProvider"];

                systemSettings.Add("DataProviderString", "AssemblyName={System.Data, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089}; ConnectionType=System.Data.SqlClient.SqlConnection; AdapterType=System.Data.SqlClient.SqlDataAdapter", "Configuration database ADO.NET data provider assembly type creation string used when ConfigurationType=Database");
                systemSettings.Add("NodeID", "00000000-0000-0000-0000-000000000000", "Unique Node ID");
                systemSettings.Add("CompanyName", "Grid Protection Alliance", "The name of the company who owns this instance of the openXDA.");
                systemSettings.Add("CompanyAcronym", "GPA", "The acronym representing the company who owns this instance of the openXDA.");
                systemSettings.Add("WebHostURL", "http://+:8989", "The web hosting URL for remote system management.");
                systemSettings.Add("DefaultWebPage", "index.cshtml", "The default web page for the hosted web server.");
                systemSettings.Add("DateFormat", "MM/dd/yyyy", "The default date format to use when rendering timestamps.");
                systemSettings.Add("TimeFormat", "HH:mm.ss.fff", "The default time format to use when rendering timestamps.");
                systemSettings.Add("BootstrapTheme", "Content/bootstrap.min.css", "Path to Bootstrap CSS to use for rendering styles.");

                systemSettings.Add("AuthenticationSchemes", AuthenticationOptions.DefaultAuthenticationSchemes, "Comma separated list of authentication schemes to use for clients accessing the hosted web server, e.g., Basic or NTLM.");
                systemSettings.Add("AuthFailureRedirectResourceExpression", DefaultAuthFailureRedirectResourceExpression, "Expression that will match paths for the resources on the web server that should redirect to the LoginPage when authentication fails.");
                systemSettings.Add("AnonymousResourceExpression", DefaultAnonymousResourceExpression, "Expression that will match paths for the resources on the web server that can be provided without checking credentials.");
                systemSettings.Add("AuthenticationToken", SessionHandler.DefaultAuthenticationToken, "Defines the token used for identifying the authentication token in cookie headers.");
                systemSettings.Add("SessionToken", SessionHandler.DefaultSessionToken, "Defines the token used for identifying the session ID in cookie headers.");
                systemSettings.Add("RequestVerificationToken", AuthenticationOptions.DefaultRequestVerificationToken, "Defines the token used for anti-forgery verification in HTTP request headers.");
                systemSettings.Add("LoginPage", AuthenticationOptions.DefaultLoginPage, "Defines the login page used for redirects on authentication failure. Expects forward slash prefix.");
                systemSettings.Add("AuthTestPage", AuthenticationOptions.DefaultAuthTestPage, "Defines the page name for the web server to test if a user is authenticated. Expects forward slash prefix.");
                systemSettings.Add("Realm", "", "Case-sensitive identifier that defines the protection space for the web based authentication and is used to indicate a scope of protection.");
                systemSettings.Add("ConfigurationCachePath", string.Format("{0}{1}ConfigurationCache{1}", FilePath.GetAbsolutePath(""), Path.DirectorySeparatorChar), "Defines the path used to cache serialized configurations");

                securityProvider.Add("ConnectionString", "Eval(systemSettings.ConnectionString)", "Connection connection string to be used for connection to the backend security datastore.");
                securityProvider.Add("DataProviderString", "Eval(systemSettings.DataProviderString)", "Configuration database ADO.NET data provider assembly type creation string to be used for connection to the backend security datastore.");

                using (AdoDataConnection connection = new AdoDataConnection("securityProvider"))
                {
                    ValidateAccountsAndGroups(connection);
                }

                DefaultWebPage = systemSettings["DefaultWebPage"].Value;

                Model = new AppModel();
                Model.Global.CompanyName = systemSettings["CompanyName"].Value;
                Model.Global.CompanyAcronym = systemSettings["CompanyAcronym"].Value;
                Model.Global.ApplicationName = "openXDA";
                Model.Global.ApplicationDescription = "open eXtensible Disturbance Analytics";
                Model.Global.ApplicationKeywords = "open source, utility, software, meter, interrogation";
                Model.Global.DateFormat = systemSettings["DateFormat"].Value;
                Model.Global.TimeFormat = systemSettings["TimeFormat"].Value;
                Model.Global.DateTimeFormat = $"{Model.Global.DateFormat} {Model.Global.TimeFormat}";
                Model.Global.BootstrapTheme = systemSettings["BootstrapTheme"].Value;

                // Attach to default web server events
                WebServer webServer = WebServer.Default;
                webServer.StatusMessage += WebServer_StatusMessage;

                // Define types for Razor pages - self-hosted web service does not use view controllers so
                // we must define configuration types for all paged view model based Razor views here:
                webServer.PagedViewModelTypes.TryAdd("Config/Users.cshtml", new Tuple<Type, Type>(typeof(UserAccount), typeof(SecurityHub)));
                webServer.PagedViewModelTypes.TryAdd("Config/ProblematicUserAccounts.cshtml", new Tuple<Type, Type>(typeof(UserAccount), typeof(DataHub)));
                webServer.PagedViewModelTypes.TryAdd("Config/UsersEmailTemplates.cshtml", new Tuple<Type, Type>(typeof(UserEmailTemplate), typeof(DataHub)));
                webServer.PagedViewModelTypes.TryAdd("Config/EmailTemplatesUsers.cshtml", new Tuple<Type, Type>(typeof(EmailTemplateUser), typeof(DataHub)));
                webServer.PagedViewModelTypes.TryAdd("Config/Groups.cshtml", new Tuple<Type, Type>(typeof(SecurityGroup), typeof(SecurityHub)));
                webServer.PagedViewModelTypes.TryAdd("Config/Settings.cshtml", new Tuple<Type, Type>(typeof(Setting), typeof(DataHub)));
                webServer.PagedViewModelTypes.TryAdd("Config/XSLTemplate.cshtml", new Tuple<Type, Type>(typeof(XSLTemplate), typeof(DataHub)));
                webServer.PagedViewModelTypes.TryAdd("Config/AssetGroups.cshtml", new Tuple<Type, Type>(typeof(AssetGroup), typeof(DataHub)));
                webServer.PagedViewModelTypes.TryAdd("Config/AssetGroupAssetGroupView.cshtml", new Tuple<Type, Type>(typeof(AssetGroupAssetGroup), typeof(DataHub)));
                webServer.PagedViewModelTypes.TryAdd("Config/MeterAssetGroupView.cshtml", new Tuple<Type, Type>(typeof(MeterAssetGroup), typeof(DataHub)));
                webServer.PagedViewModelTypes.TryAdd("Config/LineAssetGroupView.cshtml", new Tuple<Type, Type>(typeof(LineAssetGroup), typeof(DataHub)));
                webServer.PagedViewModelTypes.TryAdd("Config/DashSettings.cshtml", new Tuple<Type, Type>(typeof(DashSettings), typeof(DataHub)));
                webServer.PagedViewModelTypes.TryAdd("Config/UserDashSettings.cshtml", new Tuple<Type, Type>(typeof(UserDashSettings), typeof(DataHub)));
                webServer.PagedViewModelTypes.TryAdd("Config/MetersWithHourlyLimits.cshtml", new Tuple<Type, Type>(typeof(MetersWithHourlyLimits), typeof(DataHub)));
                webServer.PagedViewModelTypes.TryAdd("Config/ChannelsWithHourlyLimits.cshtml", new Tuple<Type, Type>(typeof(ChannelsWithHourlyLimits), typeof(DataHub)));
                webServer.PagedViewModelTypes.TryAdd("Config/MetersWithNormalLimits.cshtml", new Tuple<Type, Type>(typeof(MetersWithNormalLimits), typeof(DataHub)));
                webServer.PagedViewModelTypes.TryAdd("Config/ChannelsWithNormalLimits.cshtml", new Tuple<Type, Type>(typeof(ChannelsWithNormalLimits), typeof(DataHub)));
                webServer.PagedViewModelTypes.TryAdd("Config/HourOfWeekLimits.cshtml", new Tuple<Type, Type>(typeof(HourOfWeekLimit), typeof(DataHub)));
                webServer.PagedViewModelTypes.TryAdd("Config/AlarmSettings.cshtml", new Tuple<Type, Type>(typeof(AlarmRangeLimitView), typeof(DataHub)));
                webServer.PagedViewModelTypes.TryAdd("Config/DefaultAlarmSettings.cshtml", new Tuple<Type, Type>(typeof(DefaultAlarmRangeLimitView), typeof(DataHub)));
                webServer.PagedViewModelTypes.TryAdd("Config/UserAccountAssetGroupView.cshtml", new Tuple<Type, Type>(typeof(UserAccountAssetGroup), typeof(DataHub)));
                webServer.PagedViewModelTypes.TryAdd("Config/EmailTypes.cshtml", new Tuple<Type, Type>(typeof(EmailType), typeof(DataHub)));
                webServer.PagedViewModelTypes.TryAdd("Config/EventEmailConfiguration.cshtml", new Tuple<Type, Type>(typeof(EventEmailParameters), typeof(DataHub)));

                webServer.PagedViewModelTypes.TryAdd("Assets/Lines.cshtml", new Tuple<Type, Type>(typeof(LineView), typeof(DataHub)));
                webServer.PagedViewModelTypes.TryAdd("Assets/MeterLines.cshtml", new Tuple<Type, Type>(typeof(MeterLine), typeof(DataHub)));
                webServer.PagedViewModelTypes.TryAdd("Assets/Channels.cshtml", new Tuple<Type, Type>(typeof(Channel), typeof(DataHub)));
                webServer.PagedViewModelTypes.TryAdd("Assets/Meters.cshtml", new Tuple<Type, Type>(typeof(Meter), typeof(DataHub)));
                webServer.PagedViewModelTypes.TryAdd("Assets/Sites.cshtml", new Tuple<Type, Type>(typeof(MeterLocation), typeof(DataHub)));

                webServer.PagedViewModelTypes.TryAdd("Workbench/Filters.cshtml", new Tuple<Type, Type>(typeof(WorkbenchFilter), typeof(DataHub)));
                webServer.PagedViewModelTypes.TryAdd("Workbench/Events.cshtml", new Tuple<Type, Type>(typeof(Event), typeof(DataHub)));
                webServer.PagedViewModelTypes.TryAdd("Workbench/Event.cshtml", new Tuple<Type, Type>(typeof(SingleEvent), typeof(DataHub)));
                webServer.PagedViewModelTypes.TryAdd("Workbench/Breaker.cshtml", new Tuple<Type, Type>(typeof(BreakerOperation), typeof(DataHub)));
                webServer.PagedViewModelTypes.TryAdd("Workbench/EventsForDate.cshtml", new Tuple<Type, Type>(typeof(EventForDate), typeof(DataHub)));
                webServer.PagedViewModelTypes.TryAdd("Workbench/EventsForDay.cshtml", new Tuple<Type, Type>(typeof(EventForDay), typeof(DataHub)));
                webServer.PagedViewModelTypes.TryAdd("Workbench/BreakersForDay.cshtml", new Tuple<Type, Type>(typeof(BreakersForDay), typeof(DataHub)));
                webServer.PagedViewModelTypes.TryAdd("Workbench/EventsForMeter.cshtml", new Tuple<Type, Type>(typeof(EventForMeter), typeof(DataHub)));
                webServer.PagedViewModelTypes.TryAdd("Workbench/MeterEventsByLine.cshtml", new Tuple<Type, Type>(typeof(MeterEventsByLine), typeof(DataHub)));
                webServer.PagedViewModelTypes.TryAdd("Workbench/FaultsDetailsByDate.cshtml", new Tuple<Type, Type>(typeof(FaultsDetailsByDate), typeof(DataHub)));
                webServer.PagedViewModelTypes.TryAdd("Workbench/DisturbancesForDay.cshtml", new Tuple<Type, Type>(typeof(DisturbancesForDay), typeof(DataHub)));
                webServer.PagedViewModelTypes.TryAdd("Workbench/DisturbancesForMeter.cshtml", new Tuple<Type, Type>(typeof(DisturbancesForMeter), typeof(DataHub)));
                webServer.PagedViewModelTypes.TryAdd("Workbench/FaultsForMeter.cshtml", new Tuple<Type, Type>(typeof(FaultForMeter), typeof(DataHub)));
                webServer.PagedViewModelTypes.TryAdd("Workbench/SiteSummaryPVM.cshtml", new Tuple<Type, Type>(typeof(SiteSummary), typeof(DataHub)));
                webServer.PagedViewModelTypes.TryAdd("Workbench/AuditLog.cshtml", new Tuple<Type, Type>(typeof(AuditLog), typeof(DataHub)));
                webServer.PagedViewModelTypes.TryAdd("Workbench/DataFiles.cshtml", new Tuple<Type, Type>(typeof(openXDA.Model.DataFile), typeof(DataHub)));
                webServer.PagedViewModelTypes.TryAdd("Workbench/StepChangeWebReportSettings.cshtml", new Tuple<Type, Type>(typeof(StepChangeMeasurement), typeof(DataHub)));
                webServer.PagedViewModelTypes.TryAdd("Workbench/PQTrendingWebReportSettings.cshtml", new Tuple<Type, Type>(typeof(PQMeasurement), typeof(DataHub)));

                webServer.PagedViewModelTypes.TryAdd("DataPusher/RemoteXDAInstances.cshtml", new Tuple<Type, Type>(typeof(RemoteXDAInstance), typeof(DataHub)));
                webServer.PagedViewModelTypes.TryAdd("DataPusher/MetersToDataPush.cshtml", new Tuple<Type, Type>(typeof(MetersToDataPush), typeof(DataHub)));

                // Parse configured authentication schemes
                if (!Enum.TryParse(systemSettings["AuthenticationSchemes"].ValueAs(AuthenticationOptions.DefaultAuthenticationSchemes.ToString()), true, out AuthenticationSchemes authenticationSchemes))
                    authenticationSchemes = AuthenticationOptions.DefaultAuthenticationSchemes;

                // Initialize web startup configuration
                Startup.AuthenticationOptions.AuthenticationSchemes = authenticationSchemes;
                Startup.AuthenticationOptions.AuthFailureRedirectResourceExpression = systemSettings["AuthFailureRedirectResourceExpression"].ValueAs(DefaultAuthFailureRedirectResourceExpression);
                Startup.AuthenticationOptions.AnonymousResourceExpression = systemSettings["AnonymousResourceExpression"].ValueAs(DefaultAnonymousResourceExpression);
                Startup.AuthenticationOptions.AuthenticationToken = systemSettings["AuthenticationToken"].ValueAs(SessionHandler.DefaultAuthenticationToken);
                Startup.AuthenticationOptions.SessionToken = systemSettings["SessionToken"].ValueAs(SessionHandler.DefaultSessionToken);
                Startup.AuthenticationOptions.RequestVerificationToken = systemSettings["RequestVerificationToken"].ValueAs(AuthenticationOptions.DefaultRequestVerificationToken);
                Startup.AuthenticationOptions.LoginPage = systemSettings["LoginPage"].ValueAs(AuthenticationOptions.DefaultLoginPage);
                Startup.AuthenticationOptions.AuthTestPage = systemSettings["AuthTestPage"].ValueAs(AuthenticationOptions.DefaultAuthTestPage);
                Startup.AuthenticationOptions.Realm = systemSettings["Realm"].ValueAs("");
                Startup.AuthenticationOptions.LoginHeader = $"<h3><img src=\"/Images/{Model.Global.ApplicationName}.png\"/> {Model.Global.ApplicationName}</h3>";

                // Validate that configured authentication test page does not evaluate as an anonymous resource nor a authentication failure redirection resource
                string authTestPage = Startup.AuthenticationOptions.AuthTestPage;

                if (Startup.AuthenticationOptions.IsAnonymousResource(authTestPage))
                    throw new SecurityException($"The configured authentication test page \"{authTestPage}\" evaluates as an anonymous resource. Modify \"AnonymousResourceExpression\" setting so that authorization test page is not a match.");

                if (Startup.AuthenticationOptions.IsAuthFailureRedirectResource(authTestPage))
                    throw new SecurityException($"The configured authentication test page \"{authTestPage}\" evaluates as an authentication failure redirection resource. Modify \"AuthFailureRedirectResourceExpression\" setting so that authorization test page is not a match.");

                if (Startup.AuthenticationOptions.AuthenticationToken == Startup.AuthenticationOptions.SessionToken)
                    throw new InvalidOperationException("Authentication token must be different from session token in order to differentiate the cookie values in the HTTP headers.");


                // Create new web application hosting environment
                m_webAppHost = WebApp.Start<Startup>(systemSettings["WebHostURL"].Value);

                // Initiate pre-compile of base templates
                if (AssemblyInfo.EntryAssembly.Debuggable)
                {
                    RazorEngine<CSharpDebug>.Default.PreCompile(HandleException);
                    RazorEngine<VisualBasicDebug>.Default.PreCompile(HandleException);
                }
                else
                {
                    RazorEngine<CSharp>.Default.PreCompile(HandleException);
                    RazorEngine<VisualBasic>.Default.PreCompile(HandleException);
                }

                return true;
            }
            catch (TargetInvocationException ex)
            {
                string message;

                // Log the exception
                message = "Failed to start web UI due to exception: " + ex.InnerException.Message;
                HandleException(new InvalidOperationException(message, ex));

                return false;
            }
            catch (Exception ex)
            {
                string message;

                // Log the exception
                message = "Failed to start web UI due to exception: " + ex.Message;
                HandleException(new InvalidOperationException(message, ex));

                return false;
            }
        }

        private void ServiceHeartbeatHandler(string s, object[] args)
        {
            // Go through all service monitors to notify of the heartbeat
            foreach (IServiceMonitor serviceMonitor in m_serviceMonitors.Adapters)
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

        private void ReloadConfigurationHandler(string s, object[] args)
        {
            m_extensibleDisturbanceAnalysisEngine.ReloadConfiguration();

            using (AdoDataConnection connection = new AdoDataConnection("securityProvider"))
            {
                ValidateAccountsAndGroups(connection);
            }
        }

        private void EnumerateWatchDirectoriesHandler(string s, object[] args)
        {
            m_extensibleDisturbanceAnalysisEngine.EnumerateWatchDirectories();
        }

        // Deletes old files from the XDA watch directories.
        private void AutoFileDeletionHandler(string s, object[] args)
        {
            m_extensibleDisturbanceAnalysisEngine.AutoDeleteFiles();
        }

        // Reloads system settings from the database.
        private void ReloadSystemSettingsRequestHandler(ClientRequestInfo requestInfo)
        {
            string connectionString = LoadSystemSettings();

            m_extensibleDisturbanceAnalysisEngine.ReloadSystemSettings();
            m_reportsEngine.ReloadSystemSettings(connectionString);

            if (m_dataPusherEngine.Running && m_dataPusherEngine.DataPusherSettings.Enabled)
                m_dataPusherEngine.ReloadSystemSettings();
            else if (!m_dataPusherEngine.DataPusherSettings.Enabled)
                m_dataPusherEngine.Stop();
            else if (!m_dataPusherEngine.Running && m_dataPusherEngine.DataPusherSettings.Enabled)
                m_dataPusherEngine.Start();
            else
                m_dataPusherEngine.Stop();

            SendResponse(requestInfo, true);
        }

        // Reloads system settings from the database.
        private void OnReloadSystemSettingsRequestHandler()
        {
            m_extensibleDisturbanceAnalysisEngine.ReloadSystemSettings();

            if (m_dataPusherEngine.Running && m_dataPusherEngine.DataPusherSettings.Enabled)
                m_dataPusherEngine.ReloadSystemSettings();
            else if (!m_dataPusherEngine.DataPusherSettings.Enabled)
                m_dataPusherEngine.Stop();
            else if (!m_dataPusherEngine.Running && m_dataPusherEngine.DataPusherSettings.Enabled)
                m_dataPusherEngine.Start();
            else
                m_dataPusherEngine.Stop();

            if (m_dataAggregationEngine.Running && m_dataAggregationEngine.PQMarkAggregationSettings.Enabled)
                m_dataAggregationEngine.ReloadSystemSettings();
            else if (!m_dataAggregationEngine.PQMarkAggregationSettings.Enabled)
                m_dataAggregationEngine.Stop();
            else if (!m_dataPusherEngine.Running && m_dataAggregationEngine.PQMarkAggregationSettings.Enabled)
                m_dataAggregationEngine.Start();
            else
                m_dataAggregationEngine.Stop();


            LogStatusMessage("Reload system settings complete...");
        }


        // Displays status information about the XDA engine.
        private void EngineStatusHandler(ClientRequestInfo requestInfo)
        {
            if (m_extensibleDisturbanceAnalysisEngine != null)
                DisplayResponseMessage(requestInfo, m_extensibleDisturbanceAnalysisEngine.Status);
            else
                SendResponseWithAttachment(requestInfo, false, null, "Engine is not ready.");
        }

        // Modifies the behavior of the file processor at runtime.
        private void TweakFileProcessorHandler(ClientRequestInfo requestInfo)
        {
            if (requestInfo.Request.Arguments.ContainsHelpRequest)
            {
                string helpMessage = m_extensibleDisturbanceAnalysisEngine.TweakFileProcessor(new string[] { "-?" });
                DisplayResponseMessage(requestInfo, helpMessage);
                return;
            }

            string[] args = Arguments.ToArgs(requestInfo.Request.Arguments.ToString());
            string message = m_extensibleDisturbanceAnalysisEngine.TweakFileProcessor(args);
            DisplayResponseMessage(requestInfo, message);
        }

        // Restores event email engine to a working state after a trip has occurred.
        private void RestoreEventEmails(ClientRequestInfo requestInfo)
        {
            m_extensibleDisturbanceAnalysisEngine.RestoreEventEmails();
        }

        private void PurgeDataHandler(ClientRequestInfo requestInfo)
        {
            if (requestInfo.Request.Arguments.ContainsHelpRequest)
            {
                string helpMessage = m_extensibleDisturbanceAnalysisEngine.PurgeData(new string[] { "-?" });
                DisplayResponseMessage(requestInfo, helpMessage);
                return;
            }

            string[] args = Arguments.ToArgs(requestInfo.Request.Arguments.ToString());
            string message = m_extensibleDisturbanceAnalysisEngine.PurgeData(args);
            DisplayResponseMessage(requestInfo, message);
        }


        public void OnProcessAllData(ClientRequestInfo requestInfo)
        {
            if (requestInfo.Request.Arguments.ContainsHelpRequest)
            {
                string helpMessage = m_dataAggregationEngine.GetHelpMessage("PQMarkProcessAllData");
                DisplayResponseMessage(requestInfo, helpMessage);
                return;
            }

            if (m_dataAggregationEngine.Running)
            {
                m_dataAggregationEngine.ProcessAllData();
                SendResponseWithAttachment(requestInfo, true, null, "PQMark data aggregation engine has completed aggregating all data.");
            }
            else
                SendResponseWithAttachment(requestInfo, false, null, "PQMark data aggregation engine is not current running.");
        }

        public void OnProcessAllEmptyData(ClientRequestInfo requestInfo)
        {
            if (requestInfo.Request.Arguments.ContainsHelpRequest)
            {
                string helpMessage = m_dataAggregationEngine.GetHelpMessage("PQMarkProcessEmptyData");
                DisplayResponseMessage(requestInfo, helpMessage);
                return;
            }

            if (m_dataAggregationEngine.Running)
            {
                m_dataAggregationEngine.ProcessAllEmptyData();
                SendResponseWithAttachment(requestInfo, true, null, "PQMark data aggregation engine has completed aggregating all empty data.");
            }
            else
                SendResponseWithAttachment(requestInfo, false, null, "PQMark data aggregation engine is not current running.");
        }

        public void OnProcessMonthToDateData(ClientRequestInfo requestInfo)
        {
            if (requestInfo.Request.Arguments.ContainsHelpRequest)
            {
                string helpMessage = m_dataAggregationEngine.GetHelpMessage("PQMarkProcessMonthToDateData");
                DisplayResponseMessage(requestInfo, helpMessage);
                return;
            }

            if (m_dataAggregationEngine.Running)
            {
                m_dataAggregationEngine.ProcessMonthToDateData();
                SendResponseWithAttachment(requestInfo, true, null, "PQMark data aggregation engine has completed aggregating month to date data.");
            }
            else
                SendResponseWithAttachment(requestInfo, false, null, "PQMark data aggregation engine is not current running.");
        }

        public void OnProcessPQTrending(ClientRequestInfo requestInfo)
        {
            DateTime? date = null;
            int? meter = null;
            if (requestInfo.Request.Arguments.ContainsHelpRequest)
            {
                string helpMessage = m_pqTrendingWebReportEngine.GetHelpMessage("PQTrendingProcess");
                DisplayResponseMessage(requestInfo, helpMessage);
                return;
            }

            if (requestInfo.Request.Arguments["date"] != null) {
                try
                {
                    date = DateTime.Parse(requestInfo.Request.Arguments["date"]);
                }
                catch (Exception ex) {
                    SendResponseWithAttachment(requestInfo, false, null, "Parameter is not a valid date string, use the following format - MM/DD/YYYY.");
                    return;
                }
            }

            if (requestInfo.Request.Arguments["meter"] != null)
            {
                try
                {
                    meter = int.Parse(requestInfo.Request.Arguments["meter"]);
                }
                catch (Exception ex)
                {
                    SendResponseWithAttachment(requestInfo, false, null, "Meter parameter is not a valid interger.");
                }
            }


            if (m_pqTrendingWebReportEngine.Running)
            {
                try
                {
                    m_pqTrendingWebReportEngine.ProcessPQWebReport(date, meter);
                    SendResponseWithAttachment(requestInfo, true, null, $"PQ Trending Web Report engine has begun aggregating data. {(date == null ? "" : $"-date={date.ToString()}")}{(meter == null ? "" : $"-meter={meter.ToString()}")}");

                }
                catch (Exception ex) {
                    SendResponseWithAttachment(requestInfo, false, null, "There was an error runnign the PQ Trending Web Report.");
                }
            }
            else
                SendResponseWithAttachment(requestInfo, false, null, "PQ Trending Web Report is not running.");
        }

        public void OnProcessStepChange(ClientRequestInfo requestInfo)
        {
            DateTime? date = null;
            int? meter = null;
            if (requestInfo.Request.Arguments.ContainsHelpRequest)
            {
                string helpMessage = m_stepChangeWebReportEngine.GetHelpMessage("StepChangeProcess");
                DisplayResponseMessage(requestInfo, helpMessage);
                return;
            }

            if (requestInfo.Request.Arguments["date"] != null)
            {
                try
                {
                    date = DateTime.Parse(requestInfo.Request.Arguments["date"]);
                }
                catch (Exception ex)
                {
                    SendResponseWithAttachment(requestInfo, false, null, "Parameter is not a valid date string, use the following format - MM/DD/YYYY.");
                    return;
                }
            }

            if (requestInfo.Request.Arguments["meter"] != null)
            {
                try
                {
                    meter = int.Parse(requestInfo.Request.Arguments["meter"]);
                }
                catch (Exception ex)
                {
                    SendResponseWithAttachment(requestInfo, false, null, "Meter parameter is not a valid interger.");
                }
            }


            if (m_pqTrendingWebReportEngine.Running)
            {
                try
                {
                    m_stepChangeWebReportEngine.ProcessStepChangeWebReport(date, meter);
                    SendResponseWithAttachment(requestInfo, true, null, $"Step Change Web Report engine has begun aggregating data. {(date == null ? "" : $"-date={date.ToString()}")}{(meter == null ? "" : $"-meter={meter.ToString()}")}");

                }
                catch (Exception ex)
                {
                    SendResponseWithAttachment(requestInfo, false, null, "There was an error runnign the Step Change Web Report.");
                }
            }
            else
                SendResponseWithAttachment(requestInfo, false, null, "Step Change Web Report is not running.");
        }


        // Send a message to the service monitors on request.
        private void MsgServiceMonitorsRequestHandler(ClientRequestInfo requestInfo)
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
                foreach (IServiceMonitor serviceMonitor in m_serviceMonitors.Adapters)
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

        // Send the error to the service helper, error logger, and each service monitor
        public void HandleException(Exception ex)
        {
            string newLines = string.Format("{0}{0}", Environment.NewLine);

            m_serviceHelper.ErrorLogger.Log(ex);
            m_serviceHelper.UpdateStatus(UpdateType.Alarm, "{0}", ex.Message + newLines);

            foreach (IServiceMonitor serviceMonitor in m_serviceMonitors.Adapters)
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

        /// <summary>
        /// Validate accounts and groups to ensure that account names and group names are converted to SIDs.
        /// </summary>
        /// <param name="database">Data connection to use for database operations.</param>
        private static void ValidateAccountsAndGroups(AdoDataConnection database)
        {
            const string SelectUserAccountQuery = "SELECT ID, Name, UseADAuthentication FROM UserAccount";
            const string SelectSecurityGroupQuery = "SELECT ID, Name FROM SecurityGroup";
            const string UpdateUserAccountFormat = "UPDATE UserAccount SET Name = '{0}' WHERE ID = '{1}'";
            const string UpdateSecurityGroupFormat = "UPDATE SecurityGroup SET Name = '{0}' WHERE ID = '{1}'";

            string id;
            string sid;
            string accountName;
            Dictionary<string, string> updateMap;

            updateMap = new Dictionary<string, string>();

            // Find user accounts that need to be updated
            using (IDataReader userAccountReader = database.Connection.ExecuteReader(SelectUserAccountQuery))
            {
                while (userAccountReader.Read())
                {
                    id = userAccountReader["ID"].ToNonNullString();
                    accountName = userAccountReader["Name"].ToNonNullString();

                    if (userAccountReader["UseADAuthentication"].ToNonNullString().ParseBoolean())
                    {
                        sid = UserInfo.UserNameToSID(accountName);

                        if (!ReferenceEquals(accountName, sid) && UserInfo.IsUserSID(sid))
                            updateMap.Add(id, sid);
                    }
                }
            }

            // Update user accounts
            foreach (KeyValuePair<string, string> pair in updateMap)
                database.Connection.ExecuteNonQuery(string.Format(UpdateUserAccountFormat, pair.Value, pair.Key));

            updateMap.Clear();

            // Find security groups that need to be updated
            using (IDataReader securityGroupReader = database.Connection.ExecuteReader(SelectSecurityGroupQuery))
            {
                while (securityGroupReader.Read())
                {
                    id = securityGroupReader["ID"].ToNonNullString();
                    accountName = securityGroupReader["Name"].ToNonNullString();

                    if (accountName.Contains('\\'))
                    {
                        sid = UserInfo.GroupNameToSID(accountName);

                        if (!ReferenceEquals(accountName, sid) && UserInfo.IsGroupSID(sid))
                            updateMap.Add(id, sid);
                    }
                }
            }

            // Update security groups
            foreach (KeyValuePair<string, string> pair in updateMap)
                database.Connection.ExecuteNonQuery(string.Format(UpdateSecurityGroupFormat, pair.Value, pair.Key));
        }


        /// <summary>
        /// Logs a status message to connected clients.
        /// </summary>
        /// <param name="message">Message to log.</param>
        /// <param name="type">Type of message to log.</param>
        public void LogStatusMessage(string message, UpdateType type = UpdateType.Information)
        {
            DisplayStatusMessage(message, type);
        }


        /// <summary>
        /// Displays a broadcast message to all subscribed clients.
        /// </summary>
        /// <param name="status">Status message to send to all clients.</param>
        /// <param name="type"><see cref="UpdateType"/> of message to send.</param>
        protected virtual void DisplayStatusMessage(string status, UpdateType type)
        {
            try
            {
                status = status.Replace("{", "{{").Replace("}", "}}");
                m_serviceHelper.UpdateStatus(type, string.Format("{0}\r\n\r\n", status));
            }
            catch (Exception ex)
            {
                HandleException(ex);
                m_serviceHelper.UpdateStatus(UpdateType.Alarm, "Failed to update client status \"" + status.ToNonNullString() + "\" due to an exception: " + ex.Message + "\r\n\r\n");
            }
        }

        /// <summary>
        /// Sends a command request to the service.
        /// </summary>
        /// <param name="clientID">Client ID of sender.</param>
        /// <param name="principal">The principal used for role-based security.</param>
        /// <param name="userInput">Request string.</param>
        public void SendRequest(Guid clientID, IPrincipal principal, string userInput)
        {
            ClientRequest request = ClientRequest.Parse(userInput);

            if ((object)request == null)
                return;

            if (SecurityProviderUtility.IsResourceSecurable(request.Command) && !SecurityProviderUtility.IsResourceAccessible(request.Command, principal))
            {
                m_serviceHelper.UpdateStatus(clientID, UpdateType.Alarm, $"Access to \"{request.Command}\" is denied.\r\n\r\n");
                return;
            }

            ClientRequestHandler requestHandler = m_serviceHelper.FindClientRequestHandler(request.Command);

            if ((object)requestHandler == null)
            {
                m_serviceHelper.UpdateStatus(clientID, UpdateType.Alarm, $"Command \"{request.Command}\" is not supported.\r\n\r\n");
                return;
            }

            ClientInfo clientInfo = new ClientInfo();
            clientInfo.ClientID = clientID;
            clientInfo.SetClientUser(principal);

            ClientRequestInfo requestInfo = new ClientRequestInfo(clientInfo, request);
            requestHandler.HandlerMethod(requestInfo);
        }

        /// <summary>
        /// Sends a command request to the service to reprocess files.
        /// </summary>
        /// <param name="dataFiles">Identifier for the file group to be reprocessed.</param>
        public void ReprocessFile(int fileGroupId, int meterId)
        {
            m_extensibleDisturbanceAnalysisEngine.ReprocessFile(fileGroupId, meterId);
        }

        public void DisconnectClient(Guid clientID)
        {
            m_serviceHelper.DisconnectClient(clientID);
        }

        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            foreach (Exception ex in e.Exception.Flatten().InnerExceptions)
                HandleException(ex);

            e.SetObserved();
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
                DisplayStatusMessage(e.Argument2, e.Argument1);
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


        private void UpdatedStatusHandler(object sender, EventArgs<Guid, string, UpdateType> e)
        {
            if ((object)UpdatedStatus != null)
                UpdatedStatus(sender, new EventArgs<Guid, string, UpdateType>(e.Argument1, e.Argument2, e.Argument3));
        }

        private void LoggedExceptionHandler(object sender, EventArgs<Exception> e)
        {
            if ((object)LoggedException != null)
                LoggedException(sender, new EventArgs<Exception>(e.Argument));
        }


        #endregion


        // Loads system settings from the database.
        private string LoadSystemSettings()
        {
            using(AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                TableOperations<Setting> settingTable = new TableOperations<Setting>(connection);
                List<Setting> settingList = settingTable.QueryRecords().ToList();

                foreach (IGrouping<string, Setting> grouping in settingList.GroupBy(setting => setting.Name))
                {
                    if (grouping.Count() > 1)
                        DisplayStatusMessage($"Duplicate record for setting {grouping.Key} detected.", UpdateType.Warning);
                }

                // Convert the Setting table to a dictionary
                Dictionary<string, string> settings = settingList
                    .DistinctBy(setting => setting.Name)
                    .ToDictionary(setting => setting.Name, setting => setting.Value, StringComparer.OrdinalIgnoreCase);

                // Convert dictionary to a connection string and return it
                return SystemSettings.ToConnectionString(settings);
            }
        }

        #endregion

        #region [ Static ]
        private static readonly ConnectionStringParser<SettingAttribute, CategoryAttribute> ConnectionStringParser = new ConnectionStringParser<SettingAttribute, CategoryAttribute>();
        #endregion
    }
}
