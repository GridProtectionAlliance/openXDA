//******************************************************************************************************
//  Startup.cs - Gbtc
//
//  Copyright © 2016, Grid Protection Alliance.  All Rights Reserved.
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
//  01/12/2016 - J. Ritchie Carroll
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Routing;
using GSF.Configuration;
using GSF.Web.Hosting;
using GSF.Web.Model.Handlers;
using GSF.Web.Security;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Json;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Extensions;
using Newtonsoft.Json;
using openXDA.Adapters;
using openXDA.Model;
using openXDA.wwwroot.Config;
using Owin;

namespace openXDA
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.Use((context, next) =>
            {
                context.Response.Headers.Remove("Server");
                return next.Invoke();
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
            app.UseAuthentication(AuthenticationOptions);


            string allowedDomainList = ConfigurationFile.Current.Settings["systemSettings"]["AllowedDomainList"]?.Value;

            if (allowedDomainList == "*")
                app.UseCors(CorsOptions.AllowAll);
            else if ((object)allowedDomainList != null)
                httpConfig.EnableCors(new System.Web.Http.Cors.EnableCorsAttribute(allowedDomainList, "*", "*"));


            CsvDownloadHandler.LogExceptionHandler = Program.Host.HandleException;
            HowlCSVDownloadHandler.LogExceptionHandler = Program.Host.HandleException;
            HowlCSVUploadHandler.LogExceptionHandler = Program.Host.HandleException;

            // Load ServiceHub SignalR class
            app.MapSignalR("/signalr",hubConfig);

            // Set configuration to use reflection to setup routes
            httpConfig.MapHttpAttributeRoutes(new CustomDirectRouteProvider());

            // Set configuration to use reflection to setup routes
            ControllerConfig.Register(httpConfig);

            // Load new GSF web page controllers app.UseWebPageController

            // Load the WebPageController class and assign its routes
            app.UseWebApi(httpConfig);

            // Setup resolver for web page controller instances
            app.UseWebPageController(WebServer.Default, Program.Host.DefaultWebPage, Program.Host.Model, typeof(AppModel), AuthenticationOptions);

            httpConfig.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;

            // Check for configuration issues before first request
            httpConfig.EnsureInitialized();
        }

        /// <summary>
        /// Gets the authentication options used for the hosted web server.
        /// </summary>
        public static AuthenticationOptions AuthenticationOptions { get; } = new AuthenticationOptions();

    }

    public class CustomDirectRouteProvider : DefaultDirectRouteProvider
    {
        protected override IReadOnlyList<IDirectRouteFactory>
        GetActionRouteFactories(HttpActionDescriptor actionDescriptor)
        {
            return actionDescriptor.GetCustomAttributes<IDirectRouteFactory>(inherit: true);
        }
    }

}
