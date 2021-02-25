//******************************************************************************************************
//  WebHost.cs - Gbtc
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
//  01/18/2021 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using System.Web.Http.Routing;
using GSF.Configuration;
using GSF.Data;
using GSF.Data.Model;
using GSF.IO;
using GSF.Security;
using GSF.Security.Model;
using GSF.Web.Hosting;
using GSF.Web.Model;
using GSF.Web.Model.Handlers;
using GSF.Web.Security;
using log4net;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Json;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Extensions;
using Newtonsoft.Json;
using openXDA.Adapters;
using openXDA.Hubs;
using openXDA.Model;
using openXDA.Nodes;
using openXDA.WebHosting.Extensions;
using openXDA.wwwroot.Config;
using Owin;

#if DEBUG
using CSharp = GSF.Web.Model.CSharpDebug;
using VisualBasic = GSF.Web.Model.VisualBasicDebug;
#endif

namespace openXDA.WebHosting
{
    public class XDAWebHost : IDisposable
    {
        #region [ Members ]

        // Nested Types
        private static class DefaultSettings
        {
            public const string WebHostURL = "http://+:8989";
            public const string DefaultWebPage = "index.cshtml";
            public const string AllowedDomainList = "*";

            public const string AnonymousResourceExpression = "^/@|^/Scripts/|^/Content/|^/Images/|^/fonts/|^/favicon.ico$";
            public const string AuthFailureRedirectResourceExpression = AuthenticationOptions.DefaultAuthFailureRedirectResourceExpression + "|^/grafana(?!/api/).*$";
        }

        private class CustomDirectRouteProvider : DefaultDirectRouteProvider
        {
            protected override IReadOnlyList<IDirectRouteFactory> GetActionRouteFactories(HttpActionDescriptor actionDescriptor) =>
                actionDescriptor.GetCustomAttributes<IDirectRouteFactory>(inherit: true);
        }

        // Constants
        private const string SettingsCategory = "WebHost";

        #endregion

        #region [ Constructors ]

        public XDAWebHost(ConfigurationFile configurationFile, Host nodeHost)
        {
            CategorizedSettingsSection categorizedSettings = configurationFile.Settings;
            WebHostSettings = categorizedSettings[SettingsCategory];

            WebHostURL = RetrieveConfigurationFileSetting(
                name: nameof(WebHostURL),
                defaultValue: DefaultSettings.WebHostURL,
                description: "The web hosting URL for remote system management.");

            DefaultWebPage = RetrieveConfigurationFileSetting(
                name: nameof(DefaultWebPage),
                defaultValue: DefaultSettings.DefaultWebPage,
                description: "The default web page for the hosted web server.");

            AllowedDomainList = RetrieveConfigurationFileSetting(
                name: nameof(AllowedDomainList),
                defaultValue: DefaultSettings.AllowedDomainList,
                description: "Cross-domain access. Can have 1 domain or all domains. Use * for all domains and wildcards, e.g., *.consoto.com.");

            AppModel = InitializeAppModel();
            AuthenticationOptions = InitializeAuthenticationOptions();
            WebServer = InitializeWebServer();

            NodeHost = nodeHost;
            WebApp = Start();
        }

        #endregion

        #region [ Properties ]

        private CategorizedSettingsElementCollection WebHostSettings { get; }
        private string WebHostURL { get; }
        private string DefaultWebPage { get; }
        private string AllowedDomainList { get; }

        private AppModel AppModel { get; }
        private AuthenticationOptions AuthenticationOptions { get; }
        private WebServer WebServer { get; }

        private Host NodeHost { get; }

        private Func<AdoDataConnection> ConnectionFactory =>
            NodeHost.CreateDbConnection;

        private IDisposable WebApp { get; }
        private bool IsDisposed { get; set; }

        #endregion

        #region [ Methods ]

        public void Dispose()
        {
            if (IsDisposed)
                return;

            try { WebApp.Dispose(); }
            finally { IsDisposed = true; }
        }

        private AppModel InitializeAppModel()
        {
            AppModel appModel = new AppModel();

            appModel.Global.CompanyName = RetrieveConfigurationFileSetting(
                name: nameof(appModel.Global.CompanyName),
                defaultValue: "Eval(systemSettings.CompanyName)",
                description: "The name of the company who owns this instance of the openXDA.");

            appModel.Global.CompanyAcronym = RetrieveConfigurationFileSetting(
                name: nameof(appModel.Global.CompanyAcronym),
                defaultValue: "Eval(systemSettings.CompanyAcronym)",
                description: "The acronym representing the company who owns this instance of the openXDA.");

            appModel.Global.DateFormat = RetrieveConfigurationFileSetting(
                name: nameof(appModel.Global.DateFormat),
                defaultValue: "Eval(systemSettings.DateFormat)",
                description: "The default date format to use when rendering timestamps.");

            appModel.Global.TimeFormat = RetrieveConfigurationFileSetting(
                name: nameof(appModel.Global.TimeFormat),
                defaultValue: "Eval(systemSettings.TimeFormat)",
                description: "The default time format to use when rendering timestamps.");

            appModel.Global.BootstrapTheme = RetrieveConfigurationFileSetting(
                name: nameof(appModel.Global.BootstrapTheme),
                defaultValue: "Content/bootstrap.min.css",
                description: "Path to Bootstrap CSS to use for rendering styles.");

            appModel.Global.ApplicationName = "openXDA";
            appModel.Global.ApplicationDescription = "open eXtensible Disturbance Analytics";
            appModel.Global.ApplicationKeywords = "open source, utility, software, meter, interrogation";
            appModel.Global.DateTimeFormat = $"{appModel.Global.DateFormat} {appModel.Global.TimeFormat}";

            return appModel;
        }

        private AuthenticationOptions InitializeAuthenticationOptions()
        {
            AuthenticationOptions authenticationOptions = new AuthenticationOptions();

            // Authentication settings
            authenticationOptions.AuthenticationSchemes = RetrieveConfigurationFileSetting(
                name: nameof(authenticationOptions.AuthenticationSchemes),
                defaultValue: AuthenticationOptions.DefaultAuthenticationSchemes,
                description: "Comma separated list of authentication schemes to use for clients accessing the hosted web server, e.g., Basic or NTLM.");

            authenticationOptions.AuthFailureRedirectResourceExpression = RetrieveConfigurationFileSetting(
                name: nameof(authenticationOptions.AuthFailureRedirectResourceExpression),
                defaultValue: DefaultSettings.AuthFailureRedirectResourceExpression,
                description: "Expression that will match paths for the resources on the web server that should redirect to the LoginPage when authentication fails.");

            authenticationOptions.AnonymousResourceExpression = RetrieveConfigurationFileSetting(
                name: nameof(authenticationOptions.AnonymousResourceExpression),
                defaultValue: DefaultSettings.AnonymousResourceExpression,
                description: "Expression that will match paths for the resources on the web server that can be provided without checking credentials.");

            authenticationOptions.AuthenticationToken = RetrieveConfigurationFileSetting(
                name: nameof(authenticationOptions.AuthenticationToken),
                defaultValue: SessionHandler.DefaultAuthenticationToken,
                description: "Defines the token used for identifying the authentication token in cookie headers.");

            authenticationOptions.SessionToken = RetrieveConfigurationFileSetting(
                name: nameof(authenticationOptions.SessionToken),
                defaultValue: SessionHandler.DefaultSessionToken,
                description: "Defines the token used for identifying the session ID in cookie headers.");

            authenticationOptions.RequestVerificationToken = RetrieveConfigurationFileSetting(
                name: nameof(authenticationOptions.RequestVerificationToken),
                defaultValue: AuthenticationOptions.DefaultRequestVerificationToken,
                description: "Defines the token used for anti-forgery verification in HTTP request headers.");

            authenticationOptions.LoginPage = RetrieveConfigurationFileSetting(
                name: nameof(authenticationOptions.LoginPage),
                defaultValue: AuthenticationOptions.DefaultLoginPage,
                description: "Defines the login page used for redirects on authentication failure. Expects forward slash prefix.");

            authenticationOptions.AuthTestPage = RetrieveConfigurationFileSetting(
                name: nameof(authenticationOptions.AuthTestPage),
                defaultValue: AuthenticationOptions.DefaultAuthTestPage,
                description: "Defines the page name for the web server to test if a user is authenticated. Expects forward slash prefix.");

            authenticationOptions.Realm = RetrieveConfigurationFileSetting(
                name: nameof(authenticationOptions.Realm),
                defaultValue: string.Empty,
                description: "Case-sensitive identifier that defines the protection space for the web based authentication and is used to indicate a scope of protection.");

            authenticationOptions.LoginHeader = $"<h3><img src=\"/Images/{AppModel.Global.ApplicationName}.png\"/> {AppModel.Global.ApplicationName}</h3>";

            // Validate that configured authentication test page does not evaluate as an anonymous resource nor a authentication failure redirection resource
            string authTestPage = authenticationOptions.AuthTestPage;

            if (authenticationOptions.IsAnonymousResource(authTestPage))
                throw new SecurityException($"The configured authentication test page \"{authTestPage}\" evaluates as an anonymous resource. Modify \"AnonymousResourceExpression\" setting so that authorization test page is not a match.");

            if (authenticationOptions.IsAuthFailureRedirectResource(authTestPage))
                throw new SecurityException($"The configured authentication test page \"{authTestPage}\" evaluates as an authentication failure redirection resource. Modify \"AuthFailureRedirectResourceExpression\" setting so that authorization test page is not a match.");

            if (authenticationOptions.AuthenticationToken == authenticationOptions.SessionToken)
                throw new InvalidOperationException("Authentication token must be different from session token in order to differentiate the cookie values in the HTTP headers.");

            return authenticationOptions;
        }

        private WebServerOptions InitializeWebServerOptions()
        {
            WebServerOptions webServerOptions = new WebServerOptions();

            webServerOptions.WebRootPath = RetrieveConfigurationFileSetting(
                name: nameof(webServerOptions.WebRootPath),
                defaultValue: WebServerOptions.DefaultWebRootPath,
                description: "The root path for the hosted web server files. Location will be relative to install folder if full path is not specified.");

            webServerOptions.ClientCacheEnabled = RetrieveConfigurationFileSetting(
                name: nameof(webServerOptions.ClientCacheEnabled),
                defaultValue: WebServerOptions.DefaultClientCacheEnabled,
                description: "Determines if cache control is enabled for web server when rendering content to browser clients.");

            // Do not minify javascript,
            // Microsoft.Ajax minify return Null Reference exception on webpacked
            webServerOptions.MinifyJavascript = false;

            webServerOptions.MinifyStyleSheets = RetrieveConfigurationFileSetting(
                name: nameof(webServerOptions.MinifyStyleSheets),
                defaultValue: WebServerOptions.DefaultMinifyStyleSheets,
                description: "Determines if minification should be applied to rendered CSS files.");

            webServerOptions.UseMinifyInDebug = RetrieveConfigurationFileSetting(
                name: nameof(webServerOptions.UseMinifyInDebug),
                defaultValue: WebServerOptions.DefaultUseMinifyInDebug,
                description: "Determines if minification should be applied when running a Debug build.");

            webServerOptions.SessionToken = RetrieveConfigurationFileSetting(
                name: nameof(webServerOptions.SessionToken),
                defaultValue: SessionHandler.DefaultSessionToken,
                description: "Defines the token used for identifying the session ID in cookie headers.");

            webServerOptions.AuthTestPage = RetrieveConfigurationFileSetting(
                name: nameof(webServerOptions.AuthTestPage),
                defaultValue: AuthenticationOptions.DefaultAuthTestPage,
                description: "Defines the page name for the web server to test if a user is authenticated. Expects forward slash prefix.");

            return webServerOptions;
        }

        private RazorEngine<T> InitializeRazorEngine<T>() where T : LanguageConstraint, new()
        {
            string templatePath = RetrieveConfigurationFileSetting(
                name: "TemplatePath",
                defaultValue: "Eval(webHost.WebRootPath)",
                description: "Path for data context based razor field templates.");

            string absoluteTemplatePath = FilePath.GetAbsolutePath(templatePath);
            return new RazorEngine<T>(absoluteTemplatePath);
        }

        private WebServer InitializeWebServer()
        {
            WebServerOptions webServerOptions = InitializeWebServerOptions();
            IRazorEngine razorEngineCS = InitializeRazorEngine<CSharp>();
            IRazorEngine razorEngineVB = InitializeRazorEngine<VisualBasic>();
            WebServer webServer = new WebServer(webServerOptions, razorEngineCS, razorEngineVB);

            // Attach to default web server events
            webServer.StatusMessage += (sender, args) => Log.Debug($"[Status] WebHost: {args.Argument}");

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
            webServer.PagedViewModelTypes.TryAdd("Config/LineAssetGroupView.cshtml", new Tuple<Type, Type>(typeof(AssetAssetGroup), typeof(DataHub)));
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
            webServer.PagedViewModelTypes.TryAdd("Config/ExternalLinks/PQViewDataLoader.cshtml", new Tuple<Type, Type>(typeof(PQViewSite), typeof(DataHub)));

            webServer.PagedViewModelTypes.TryAdd("Assets/Asset.cshtml", new Tuple<Type, Type>(typeof(Asset), typeof(DataHub)));
            webServer.PagedViewModelTypes.TryAdd("Assets/Sites.cshtml", new Tuple<Type, Type>(typeof(Location), typeof(DataHub)));
            webServer.PagedViewModelTypes.TryAdd("Assets/Lines.cshtml", new Tuple<Type, Type>(typeof(LineView), typeof(DataHub)));
            webServer.PagedViewModelTypes.TryAdd("Assets/LineSegments.cshtml", new Tuple<Type, Type>(typeof(LineSegment), typeof(DataHub)));
            webServer.PagedViewModelTypes.TryAdd("Assets/Buss.cshtml", new Tuple<Type, Type>(typeof(Bus), typeof(DataHub)));
            webServer.PagedViewModelTypes.TryAdd("Assets/CapacitorBanks.cshtml", new Tuple<Type, Type>(typeof(CapBank), typeof(DataHub)));
            webServer.PagedViewModelTypes.TryAdd("Assets/CapacitorBankRelays.cshtml", new Tuple<Type, Type>(typeof(CapBankRelay), typeof(DataHub)));
            webServer.PagedViewModelTypes.TryAdd("Assets/Transformers.cshtml", new Tuple<Type, Type>(typeof(Transformer), typeof(DataHub)));
            webServer.PagedViewModelTypes.TryAdd("Assets/Breakers.cshtml", new Tuple<Type, Type>(typeof(Breaker), typeof(DataHub)));
            webServer.PagedViewModelTypes.TryAdd("Assets/MeterLines.cshtml", new Tuple<Type, Type>(typeof(MeterAsset), typeof(DataHub)));
            webServer.PagedViewModelTypes.TryAdd("Assets/Meters.cshtml", new Tuple<Type, Type>(typeof(Meter), typeof(DataHub)));
            webServer.PagedViewModelTypes.TryAdd("Assets/AssetConnections.cshtml", new Tuple<Type, Type>(typeof(AssetConnection), typeof(DataHub)));
            webServer.PagedViewModelTypes.TryAdd("Assets/Channels.cshtml", new Tuple<Type, Type>(typeof(Channel), typeof(DataHub)));
            webServer.PagedViewModelTypes.TryAdd("Assets/Customer.cshtml", new Tuple<Type, Type>(typeof(Customer), typeof(DataHub)));
            webServer.PagedViewModelTypes.TryAdd("Assets/CustomerAsset.cshtml", new Tuple<Type, Type>(typeof(CustomerAsset), typeof(DataHub)));
            webServer.PagedViewModelTypes.TryAdd("Assets/SpareAsset.cshtml", new Tuple<Type, Type>(typeof(AssetSpare), typeof(DataHub)));

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
            webServer.PagedViewModelTypes.TryAdd("Workbench/DataFiles.cshtml", new Tuple<Type, Type>(typeof(DataFile), typeof(DataHub)));
            webServer.PagedViewModelTypes.TryAdd("Workbench/StepChangeWebReportSettings.cshtml", new Tuple<Type, Type>(typeof(StepChangeMeasurement), typeof(DataHub)));
            webServer.PagedViewModelTypes.TryAdd("Workbench/PQTrendingWebReportSettings.cshtml", new Tuple<Type, Type>(typeof(PQMeasurement), typeof(DataHub)));

            webServer.PagedViewModelTypes.TryAdd("DataPusher/RemoteXDAInstances.cshtml", new Tuple<Type, Type>(typeof(RemoteXDAInstance), typeof(DataHub)));
            webServer.PagedViewModelTypes.TryAdd("DataPusher/MetersToDataPush.cshtml", new Tuple<Type, Type>(typeof(MetersToDataPush), typeof(DataHub)));

            return webServer;
        }

        private IDisposable Start()
        {
            void HandleException(Exception ex) =>
                Log.Error(ex.Message, ex);

            // Create new web application hosting environment
            IDisposable webApp = Microsoft.Owin.Hosting.WebApp.Start(WebHostURL, Configure);

            // Initiate pre-compile of base templates
            WebServer.RazorEngineCS.PreCompile(HandleException);
            WebServer.RazorEngineVB.PreCompile(HandleException);

            return webApp;
        }

        private void Configure(IAppBuilder app)
        {
            app.Use(async (context, next) =>
            {
                context.Request.Environment["AuthenticationOptions"] = AuthenticationOptions.Readonly;
                await next.Invoke();
                context.Response.Headers.Remove("Server");
            });

            app.UseStageMarker(PipelineStage.PostAcquireState);

            // Modify the JSON serializer to serialize dates as UTC - otherwise, timezone will not be appended
            // to date strings and browsers will select whatever timezone suits them
            JsonSerializerSettings settings = JsonUtility.CreateDefaultSerializerSettings();
            settings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;

            JsonSerializer serializer = JsonSerializer.Create(settings);

            GlobalHost.DependencyResolver.Register(typeof(JsonSerializer), () => serializer);

            // Load security hub in application domain before establishing SignalR hub configuration
            using (new SecurityHub()) { }

            HubConfiguration hubConfig = new HubConfiguration();
            HttpConfiguration httpConfig = new HttpConfiguration();

            // Enabled detailed client errors
            hubConfig.EnableDetailedErrors = true;

            // Enable GSF session management
            httpConfig.EnableSessions(AuthenticationOptions);

            // Enable GSF role-based security authentication
            app.UseHostAuthentication(ConnectionFactory);

            app.UseWhen(context => !(context.Request.User is SecurityPrincipal),
                branch => branch.UseAuthentication(AuthenticationOptions));

            if (AllowedDomainList == "*")
                app.UseCors(CorsOptions.AllowAll);
            else if (!(AllowedDomainList is null))
                httpConfig.EnableCors(new System.Web.Http.Cors.EnableCorsAttribute(AllowedDomainList, "*", "*"));

            // Load ServiceHub SignalR class
            app.MapSignalR(hubConfig);

            // Set configuration to use reflection to setup routes
            TryLoadExternalWebControllers();
            httpConfig.MapHttpAttributeRoutes(new CustomDirectRouteProvider());

            // Override controller selection
            XDAControllerSelector controllerSelector = new XDAControllerSelector(httpConfig, NodeHost.GetNode);
            httpConfig.Services.Replace(typeof(IHttpControllerSelector), controllerSelector);

            // Override controller activation
            XDAControllerActivator controllerActivator = new XDAControllerActivator(ConnectionFactory, NodeHost);
            httpConfig.Services.Replace(typeof(IHttpControllerActivator), controllerActivator);

            // Override action selection
            XDAActionSelector actionSelector = new XDAActionSelector();
            httpConfig.Services.Replace(typeof(IHttpActionSelector), actionSelector);

            // Set configuration to use reflection to setup routes
            ControllerConfig.Register(httpConfig);

            // Load the WebPageController class and assign its routes
            app.UseWebApi(httpConfig);

            // Setup resolver for web page controller instances
            app.UseWebPageController(WebServer, DefaultWebPage, AppModel, typeof(AppModel), AuthenticationOptions);

            httpConfig.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;

            // Check for configuration issues before first request
            httpConfig.EnsureInitialized();
        }

        private void TryLoadExternalWebControllers()
        {
            // Load external WebController Dll to make sure they are in the application Domain before loading the Web controller
            using (AdoDataConnection connection = ConnectionFactory())
            {
                try
                {
                    TableOperations<WebControllerExtension> extensionTable = new TableOperations<WebControllerExtension>(connection);

                    List<WebControllerExtension> webExtensionDefinitions = extensionTable
                        .QueryRecords("LoadOrder")
                        .ToList();

                    foreach (WebControllerExtension webExtensionDefinition in webExtensionDefinitions)
                    {
                        try
                        {
                            Assembly.Load(webExtensionDefinition.AssemblyName);

                            Log.Info($"[{webExtensionDefinition.AssemblyName}] Loading WebController...");
                        }
                        catch (Exception ex)
                        {
                            // Log the exception
                            string message = $"Failed to load web Controller {webExtensionDefinition.AssemblyName} due to exception: " + ex.InnerException.Message;
                            Exception wrap = new InvalidOperationException(message, ex);
                            Log.Error(wrap.Message, wrap);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex.Message, ex);
                }
            }
        }

        private T RetrieveConfigurationFileSetting<T>(string name, T defaultValue, string description)
        {
            WebHostSettings.Add(name, defaultValue, description);
            return WebHostSettings[name].ValueAs(defaultValue);
        }

        #endregion

        #region [ Static ]

        // Static Fields
        private static readonly ILog Log = LogManager.GetLogger(typeof(XDAWebHost));

        // Static Constructor
        static XDAWebHost()
        {
            CsvDownloadHandler.LogExceptionHandler = ex => Log.Error(ex.Message, ex);
            HowlCSVDownloadHandler.LogExceptionHandler = ex => Log.Error(ex.Message, ex);
            HowlCSVUploadHandler.LogExceptionHandler = ex => Log.Error(ex.Message, ex);
        }

        #endregion
    }
}