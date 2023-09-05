//******************************************************************************************************
//  GrafanaHostingNode.cs - Gbtc
//
//  Copyright © 2023, Grid Protection Alliance.  All Rights Reserved.
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
//  08/18/2023 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using GSF;
using GSF.Configuration;
using GSF.Data;
using GSF.Data.Model;
using GSF.Diagnostics;
using GSF.IO;
using GSF.Security;
using GSF.Security.Model;
using GSF.Threading;
using log4net;
using Newtonsoft.Json.Linq;
using openXDA.Configuration;
using openXDA.Model;
using static openXDA.Nodes.Types.Grafana.GrafanaQueryHelper;
using CancellationToken = System.Threading.CancellationToken;
using Node = openXDA.Model.Node;

namespace openXDA.Nodes.Types.Grafana
{
    public class GrafanaHostingNode : NodeBase, IDisposable
    {
        #region [ Members ]

        // Nested Types

        private class NodeSettings
        {
            public NodeSettings(Action<object> configure) =>
                configure(this);

            [Category]
            [SettingName(GrafanaSection.CategoryName)]
            public GrafanaSection GrafanaSettings { get; } = new GrafanaSection();
        }

        // Constants

        /// <summary>
        /// Grafana admin role name.
        /// </summary>
        public const string GrafanaAdminRoleName = "GrafanaAdmin";

        // Fields
        private GrafanaSection m_serverSettings;
        private ChildProcessManager m_serverProcessManager;
        private Task m_serverReadyTask;
        private Action m_cancelAction;

        private Dictionary<string, string[]> m_lastSecurityContext;
        private Dictionary<string, string[]> m_latestSecurityContext;
        private int m_manualSynchronization;

        private int m_isDisposed;

        #endregion

        #region [ Constructors ]

        public GrafanaHostingNode(Host host, Node definition, NodeType type)
            : base(host, definition, type)
        {
            StartServerProcessOperation = new TaskSynchronizedOperation(StartServerProcessAsync, HandleException);
            SynchronizeUsersOperation = new TaskSynchronizedOperation(SynchronizeUsersAsync, HandleException);
            AdoSecurityProvider.SecurityContextRefreshed += AdoSecurityProvider_SecurityContextRefreshed;
            StartServerProcessOperation.RunOnceAsync();
            SpinWait.SpinUntil(() => !(WhenServerIsReady is null));
        }

        #endregion

        #region [ Properties ]

        public HttpClient GrafanaHttpClient => HttpClient;

        private TaskSynchronizedOperation StartServerProcessOperation { get; }
        private TaskSynchronizedOperation SynchronizeUsersOperation { get; }

        private ChildProcessManager ServerProcessManager
        {
            get => Interlocked.CompareExchange(ref m_serverProcessManager, null, null);
            set
            {
                ChildProcessManager serverProcessManager = Interlocked.Exchange(ref m_serverProcessManager, value);
                serverProcessManager?.Dispose();
            }
        }

        private Task WhenServerIsReady
        {
            get => Interlocked.CompareExchange(ref m_serverReadyTask, null, null);
            set => Interlocked.Exchange(ref m_serverReadyTask, value);
        }

        private Action CancelAction
        {
            get => Interlocked.CompareExchange(ref m_cancelAction, null, null);
            set
            {
                Action cancelAction = Interlocked.Exchange(ref m_cancelAction, value);
                cancelAction?.Invoke();
            }
        }

        private Dictionary<string, string[]> LastSecurityContext
        {
            get => Interlocked.CompareExchange(ref m_lastSecurityContext, null, null);
            set => Interlocked.Exchange(ref m_lastSecurityContext, value);
        }

        private Dictionary<string, string[]> LatestSecurityContext
        {
            get => Interlocked.CompareExchange(ref m_latestSecurityContext, null, null);
            set => Interlocked.Exchange(ref m_latestSecurityContext, value);
        }

        private bool ManualSynchronization
        {
            get => Interlocked.CompareExchange(ref m_manualSynchronization, 0, 0) != 0;
            set => Interlocked.Exchange(ref m_manualSynchronization, value ? 1 : 0);
        }

        private bool IsDisposed
        {
            get => Interlocked.CompareExchange(ref m_isDisposed, 0, 0) != 0;
            set => Interlocked.Exchange(ref m_isDisposed, value ? 1 : 0);
        }

        #endregion

        #region [ Methods ]

        protected override void OnReconfigure(Action<object> configurator)
        {
            NodeSettings nodeSettings = new NodeSettings(configurator);
            GrafanaSection newServerSettings = nodeSettings.GrafanaSettings;
            GrafanaSection oldServerSettings = Interlocked.CompareExchange(ref m_serverSettings, null, null);

            if (HasServerProcessChanged(oldServerSettings, newServerSettings))
                StartServerProcessOperation.RunOnceAsync();
        }

        public override IHttpController CreateWebController() =>
            new GrafanaAuthProxyController(this);

        public Dictionary<string, string[]> GetLatestSecurityContext(string username) =>
            LatestSecurityContext ?? StartUserSynchronization(username);

        public Dictionary<string, string[]> StartUserSynchronization(string currentUsername)
        {
            Dictionary<string, string[]> userRoles = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase);

            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            using (UserRoleCache userRoleCache = UserRoleCache.GetCurrentCache())
            {
                TableOperations<UserAccount> userAccountTable = new TableOperations<UserAccount>(connection);
                string[] roles;

                foreach (UserAccount user in userAccountTable.QueryRecords())
                {
                    string userName = user.AccountName;

                    if (userRoleCache.TryGetUserRole(userName, out roles))
                        userRoles[userName] = roles;
                }

                // Also make sure current user is added since user may have implicit rights based on group
                if (!string.IsNullOrEmpty(currentUsername))
                {
                    if (!userRoles.ContainsKey(currentUsername) && userRoleCache.TryGetUserRole(currentUsername, out roles))
                        userRoles[currentUsername] = roles;
                }

                if (userRoles.Count > 0)
                {
                    LatestSecurityContext = userRoles;
                    ManualSynchronization = true;
                    SynchronizeUsersOperation.RunOnceAsync();
                }

                return userRoles;
            }
        }

        public GrafanaSection QueryGrafanaSettings(Action<object> configurator = null)
        {
            NodeSettings settings = new NodeSettings(configurator ?? GetConfigurator());
            return settings.GrafanaSettings;
        }

        public void Dispose()
        {
            if (IsDisposed)
                return;

            try
            {
                AdoSecurityProvider.SecurityContextRefreshed -= AdoSecurityProvider_SecurityContextRefreshed;
                ServerProcessManager = null;
                CancelAction = null;
            }
            finally
            {
                IsDisposed = true;
            }
        }

        private async Task StartServerProcessAsync()
        {
            if (IsDisposed)
                return;

            GrafanaSection newServerSettings = QueryGrafanaSettings();
            GrafanaSection oldServerSettings = Interlocked.Exchange(ref m_serverSettings, newServerSettings);

            if (!HasServerProcessChanged(oldServerSettings, newServerSettings))
                return;

            CancellationToken cancellationToken = CreateCancellationToken();
            Task startTask = Task.Run(async () => await StartServerProcessAsync(newServerSettings, cancellationToken));
            WhenServerIsReady = startTask;
            await startTask;
        }

        private async Task StartServerProcessAsync(GrafanaSection settings, CancellationToken cancellationToken)
        {
            Process process = new Process();

            try
            {
                process.StartInfo.FileName = FilePath.GetAbsolutePath(settings.ServerPath);
                process.StartInfo.WorkingDirectory = FilePath.GetAbsolutePath(settings.BasePath);
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.UseShellExecute = false;

                process.OutputDataReceived += Process_OutputDataReceived;
                process.ErrorDataReceived += Process_ErrorDataReceived;
                process.EnableRaisingEvents = true;
                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                ChildProcessManager serverProcessManager = new ChildProcessManager();
                ServerProcessManager = serverProcessManager;
                serverProcessManager.AddProcess(process);
            }
            catch
            {
                process.Dispose();
                throw;
            }

            Stopwatch stopwatch = Stopwatch.StartNew();
            GrafanaQueryHelper queryHelper = new GrafanaQueryHelper(HttpClient, settings);
            int attempts = 0;

            while (!await queryHelper.IsServerRespondingAsync())
            {
                if (cancellationToken.IsCancellationRequested)
                    return;

                if (stopwatch.Elapsed.TotalSeconds > settings.InitializationTimeout)
                {
                    Ticks ticks = Ticks.FromSeconds(settings.InitializationTimeout);
                    Log.Warn($"Timeout awaiting Grafana initialization. Timeout configured as {ticks.ToElapsedTimeString()}.");
                    return;
                }

                Thread.Sleep(500);

                if (++attempts % 4 == 0)
                    Log.Debug($"Awaiting Grafana initialization, {attempts:N0} attempts so far...");
            }
        }

        private async Task SynchronizeUsersAsync()
        {
            using (Logger.SuppressFirstChanceExceptionLogMessages())
            {
                while (!WhenServerIsReady.IsCompleted)
                    await WhenServerIsReady;

                if (IsDisposed)
                    return;

                Dictionary<string, string[]> latestSecurityContext = LatestSecurityContext;

                if (latestSecurityContext is null)
                {
                    Log.Error("User synchronization should not be invoked before security context gets updated");
                    return;
                }

                // Skip user synchronization if security context has not changed
                if (!ManualSynchronization && SecurityContextsAreEqual(latestSecurityContext, LastSecurityContext))
                    return;

                GrafanaSection settings = QueryGrafanaSettings();
                GrafanaQueryHelper queryHelper = new GrafanaQueryHelper(HttpClient, settings);
                LastSecurityContext = latestSecurityContext;
                ManualSynchronization = false;

                // Set up handler for messages received during async operations
                string message = null;
                void HandleMessage(string m) => message = m;

                // Set up handler for user details received during async operations
                UserDetail userDetail = null;
                void HandleUserDetail(UserDetail detail, string m)
                {
                    userDetail = detail;
                    message = m;
                }

                // Lookup Grafana Administrative user
                if (!await queryHelper.LookupUserAsync(settings.AdminUser, HandleUserDetail))
                {
                    Log.Warn($"Failed to synchronize Grafana users, cannot find Grafana Administrator \"{settings.AdminUser}\": {message}");
                    return;
                }

                // Get user list for target organization
                OrgUserDetail[] organizationUsers = await queryHelper.GetOrganizationUsersAsync(settings.OrganizationID, HandleMessage);

                if (!string.IsNullOrEmpty(message))
                    Log.Info($"Issue retrieving user list for default organization: {message}");

                // Make sure Grafana Administrator has an admin role in the default organization
                bool success = organizationUsers.Any(user => user.userId == userDetail.id) ?
                    await queryHelper.UpdateUserOrganizationalRoleAsync(settings.OrganizationID, userDetail.id, "Admin", HandleMessage) :
                    await queryHelper.AddUserToOrganizationAsync(settings.OrganizationID, settings.AdminUser, "Admin", HandleMessage);

                if (!success)
                    Log.Info($"Issue validating organizational admin role for Grafana Administrator \"{settings.AdminUser}\" - Grafana user synchronization may not succeed: {message}");

                foreach (KeyValuePair<string, string[]> item in latestSecurityContext)
                {
                    string username = item.Key;
                    string[] roles = item.Value;
                    bool createdUser = false;

                    if (username.Equals("_logout", StringComparison.OrdinalIgnoreCase))
                        continue;

                    // Check if user exists
                    if (!await queryHelper.LookupUserAsync(username, HandleUserDetail))
                    {
                        createdUser = await queryHelper.CreateUserAsync(username, settings.OrganizationID, HandleUserDetail);
                        Log.Info($"Encountered new user \"{username}\": {message}");
                    }

                    if (userDetail.id == 0)
                        continue;

                    // Update user's Grafana admin role status if needed
                    bool userIsGrafanaAdmin = UserIsGrafanaAdmin(roles);

                    if (userDetail.isGrafanaAdmin != userIsGrafanaAdmin)
                    {
                        try
                        {
                            JObject content = JObject.FromObject(new
                            {
                                isGrafanaAdmin = userIsGrafanaAdmin
                            });

                            dynamic result = await queryHelper.CallAPIFunctionAsync(HttpMethod.Put, $"{settings.HostedURL}/api/admin/users/{userDetail.id}/permissions", content.ToString());
                            message = result.message;
                        }
                        catch (Exception ex)
                        {
                            message = ex.Message;
                        }

                        if (!message.Equals("User permissions updated", StringComparison.OrdinalIgnoreCase))
                            Log.Info($"Issue updating permissions for user \"{username}\": {message}");
                    }

                    // Attempt to lookup user in default organization
                    OrgUserDetail orgUserDetail = organizationUsers.FirstOrDefault(user => user.userId == userDetail.id);

                    // Get user's organizational role: Admin / Editor / Viewer
                    string organizationalRole = TranslateRole(roles);

                    // Update user's organizational status / role as needed
                    if (orgUserDetail is null && !createdUser)
                        success = await queryHelper.AddUserToOrganizationAsync(settings.OrganizationID, username, organizationalRole, HandleMessage);
                    else if (createdUser || !orgUserDetail.role.Equals(organizationalRole, StringComparison.OrdinalIgnoreCase))
                        success = await queryHelper.UpdateUserOrganizationalRoleAsync(settings.OrganizationID, userDetail.id, organizationalRole, HandleMessage);
                    else
                        success = true;

                    if (!success)
                        Log.Info($"Issue assigning organizational role \"{organizationalRole}\" for user \"{username}\": {message}");
                }

                Log.Info($"Synchronized security context with {latestSecurityContext.Count:N0} users to Grafana.");
            }
        }

        private bool HasServerProcessChanged(GrafanaSection oldServerSettings, GrafanaSection newServerSettings) =>
            oldServerSettings is null ||
            newServerSettings.ServerPath != oldServerSettings.ServerPath ||
            newServerSettings.BasePath != oldServerSettings.BasePath;

        private CancellationToken CreateCancellationToken()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            CancellationToken cancellationToken = cancellationTokenSource.Token;

            CancelAction = () =>
            {
                CancellationTokenSource interlockedRef = Interlocked.Exchange(ref cancellationTokenSource, null);
                interlockedRef?.Cancel();
                interlockedRef?.Dispose();
            };

            return cancellationToken;
        }

        private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Data))
                Log.Info(e.Data);
        }

        private void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Data))
                Log.Error(e.Data);
        }

        private void AdoSecurityProvider_SecurityContextRefreshed(object sender, EventArgs<Dictionary<string, string[]>> e)
        {
            LatestSecurityContext = e.Argument;
            SynchronizeUsersOperation.RunOnceAsync();
        }

        #endregion

        #region [ Static ]

        // Static Fields
        private static readonly ILog Log = LogManager.GetLogger(typeof(GrafanaHostingNode));

        // Static Properties
        private static HttpClient HttpClient { get; }
            = new HttpClient(new HttpClientHandler { UseCookies = false });

        // Static Methods
        private static bool UserIsGrafanaAdmin(string[] roles)
        {
            return roles.Any(role => role.Equals(GrafanaAdminRoleName, StringComparison.OrdinalIgnoreCase));
        }

        private static string TranslateRole(string[] roles)
        {
            if (roles.Any(role => role.Equals("Administrator", StringComparison.OrdinalIgnoreCase)))
                return "Admin";

            if (roles.Any(role => role.Equals("Engineer", StringComparison.OrdinalIgnoreCase)))
                return "Editor";

            return "Viewer";
        }

        private static bool SecurityContextsAreEqual(Dictionary<string, string[]> left, Dictionary<string, string[]> right)
        {
            if (left == right)
                return true;

            if (left is null || right is null)
                return false;

            if (left.Count != right.Count)
                return false;

            foreach (KeyValuePair<string, string[]> item in left)
            {
                if (!right.TryGetValue(item.Key, out string[] value))
                    return false;

                if (item.Value.CompareTo(value) != 0)
                    return false;
            }

            return true;
        }

        private static void HandleException(Exception ex) =>
            Log.Error(ex.Message, ex);

        #endregion
    }
}
