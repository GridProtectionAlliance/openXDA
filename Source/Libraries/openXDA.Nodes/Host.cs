//******************************************************************************************************
//  Host.cs - Gbtc
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
//  01/17/2021 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FaultData;
using GSF.Configuration;
using GSF.Data;
using GSF.Data.Model;
using GSF.Security.Cryptography;
using log4net;
using openXDA.Configuration;
using openXDA.Model;
using AssemblyInfo = GSF.Reflection.AssemblyInfo;

namespace openXDA.Nodes
{
    public class Host : IDisposable
    {
        #region [ Members ]

        // Nested Types
        private class Settings
        {
            public Settings(Action<object> configure) =>
                configure(this);

            [Category]
            [SettingName(SystemSection.CategoryName)]
            public SystemSection SystemSettings { get; } = new SystemSection();
        }

        // Fields
        private IReadOnlyDictionary<int, INode> m_nodes;

        #endregion

        #region [ Constructors ]

        public Host(string registrationKey, Func<AdoDataConnection> connectionFactory, ICLIRegistry cliRegistry)
        {
            Nodes = new Dictionary<int, INode>();
            ConnectionFactory = connectionFactory;
            CLIRegistry = cliRegistry;
            ID = Register(registrationKey);

            Action<object> configurator = GetConfigurator();
            UpdateDbTimeout(configurator);

            // Start the async loop to check in with
            // the database for configuration updates
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            Task startTask = RunTimerAsync(cancellationTokenSource.Token);
            int isCancelled = 0;

            StopTimerAction = () =>
            {
                if (Interlocked.CompareExchange(ref isCancelled, 1, 0) != 0)
                {
                    startTask.GetAwaiter().GetResult();
                    return;
                }

                cancellationTokenSource.Cancel();
                startTask.GetAwaiter().GetResult();
                cancellationTokenSource.Dispose();
            };
        }

        #endregion

        #region [ Properties ]

        public int ID { get; }

        private Func<AdoDataConnection> ConnectionFactory { get; }
        private ICLIRegistry CLIRegistry { get; }

        private int DbTimeout { get; set; } =
            DataExtensions.DefaultTimeoutDuration;

        private IReadOnlyDictionary<int, INode> Nodes
        {
            get => Interlocked.CompareExchange(ref m_nodes, null, null);
            set => Interlocked.Exchange(ref m_nodes, value);
        }

        private Action StopTimerAction { get; }
        private bool IsDisposed { get; set; }

        #endregion

        #region [ Methods ]

        public void Reconfigure()
        {
            void Reconfigure(INode node)
            {
                try { node.Reconfigure(); }
                catch (Exception ex) { Log.Error(ex.Message, ex); }
            }

            Action<object> configurator = GetConfigurator();
            UpdateDbTimeout(configurator);

            foreach (INode node in Nodes.Values)
                Reconfigure(node);
        }

        public INode GetNode(int nodeID) =>
            Nodes.TryGetValue(nodeID, out INode node)
                ? node
                : null;

        public void RegisterScheduledProcess(INode node, Action process, string name, string schedule)
        {
            int nodeID = node.Definition.ID;

            if (!DisposeTasks.TryGetValue(nodeID, out TaskCompletionSource<object> disposeTaskSource))
                return;

            string nodeName = node.Definition.Name;
            string processName = $"[{nodeName}] {name}";
            CLIRegistry.RegisterScheduledProcess(process, processName, schedule);
            disposeTaskSource.Task.ContinueWith(_ => CLIRegistry.DeregisterScheduledProcess(processName));
        }

        public string BuildURL(int nodeID, string action) =>
            BuildURL(nodeID, action, null);

        public string BuildURL(int nodeID, string action, NameValueCollection queryParameters)
        {
            const string NodeQueryFormat =
                "SELECT ActiveHost.URL " +
                "FROM " +
                "    ActiveHost JOIN " +
                "    Node ON Node.HostRegistrationID = ActiveHost.ID " +
                "WHERE Node.ID = {0}";

            string url;

            using (AdoDataConnection connection = CreateDbConnection())
            {
                string hostURL = connection.ExecuteScalar<string>(NodeQueryFormat, nodeID);

                if (hostURL is null)
                    throw new InvalidOperationException($"Node {nodeID} is not active.");

                string cleanHostURL = hostURL.Trim().TrimEnd('/');
                url = $"{cleanHostURL}/Node/{nodeID}/{action}";
            }

            if (!(queryParameters is null) && queryParameters.Count > 0)
            {
                IEnumerable<string> queryStringParameters = queryParameters.AllKeys
                    .SelectMany(key =>
                    {
                        return queryParameters
                            .GetValues(key)
                            .Select(Uri.EscapeDataString)
                            .Select(value => $"{key}={value}");
                    });

                string queryString = string.Join("&", queryParameters);
                url += $"?{queryString}";
            }

            return url;
        }

        public async Task<HttpResponseMessage> SendWebRequestAsync(Action<HttpRequestMessage> configure, CancellationToken cancellationToken = default)
        {
            const string HostQueryFormat =
                "SELECT " +
                "    ActiveHost.RegistrationKey, " +
                "    ActiveHost.APIToken " +
                "FROM ActiveHost " +
                "WHERE ID = {0}";

            string registrationKey;
            string apiToken;

            using (AdoDataConnection connection = CreateDbConnection())
            using (DataTable hostResult = connection.RetrieveData(HostQueryFormat, ID))
            {
                if (hostResult.Rows.Count == 0)
                    throw new InvalidOperationException($"Host {ID} is not active.");

                DataRow hostRow = hostResult.Rows[0];
                registrationKey = hostRow.ConvertField<string>("RegistrationKey");
                apiToken = hostRow.ConvertField<string>("APIToken");
            }

            using (HttpRequestMessage request = new HttpRequestMessage())
            {
                configure(request);

                const string type = "XDA-Host";
                string decode = $"{registrationKey}:{apiToken}";
                Encoding utf8 = new UTF8Encoding(false);
                byte[] credentialData = utf8.GetBytes(decode);
                string credentials = Convert.ToBase64String(credentialData);
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(type, credentials);

                return await HttpClient.SendAsync(request, cancellationToken);
            }
        }

        public AdoDataConnection CreateDbConnection()
        {
            AdoDataConnection connection = ConnectionFactory();
            connection.DefaultTimeout = DbTimeout;
            return connection;
        }

        public void Dispose()
        {
            void TryDeactivate()
            {
                try
                {
                    using (AdoDataConnection connection = CreateDbConnection())
                    {
                        const string QueryFormat =
                            "UPDATE HostRegistration " +
                            "SET CheckedIn = CONVERT(DATETIME, 0) " +
                            "WHERE ID = {0} " +
                            "" +
                            "UPDATE Node " +
                            "SET HostRegistrationID = NULL " +
                            "WHERE HostRegistrationID = {0}";

                        connection.ExecuteNonQuery(QueryFormat, ID);
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex.Message, ex);
                }
            }

            if (IsDisposed)
                return;

            try
            {
                StopTimerAction();

                foreach (INode node in Nodes.Values)
                    TryDispose(node);

                Nodes = new Dictionary<int, INode>();
                TryDeactivate();
            }
            finally
            {
                IsDisposed = true;
            }
        }

        private int Register(string registrationKey)
        {
            string GetHostName()
            {
                AssemblyInfo entryAssembly = AssemblyInfo.EntryAssembly;

                return !entryAssembly.Debuggable
                    ? Environment.MachineName.ToLowerInvariant()
                    : "localhost";
            }

            string GenerateAPIToken()
            {
                List<CharacterGroup> characterGroups = new List<CharacterGroup>()
                {
                    new CharacterGroup() { Characters = "abcdefghijklmnopqrstuvwxyz", MinOccurrence = 1 },
                    new CharacterGroup() { Characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ", MinOccurrence = 1 },
                    new CharacterGroup() { Characters = "0123456789", MinOccurrence = 1 },
                    new CharacterGroup() { Characters = "~!@#$%^&*-_+,.?", MinOccurrence = 1 }
                };

                PasswordGenerator passwordGenerator = new PasswordGenerator(characterGroups);
                return passwordGenerator.GeneratePassword(50);
            }

            using (AdoDataConnection connection = CreateDbConnection())
            {
                const string QueryFormat =
                    "INSERT INTO HostRegistration " +
                    "SELECT {0}, {1}, {2}, GETUTCDATE() " +
                    "WHERE NOT EXISTS " +
                    "( " +
                    "    SELECT * " +
                    "    FROM HostRegistration " +
                    "    WHERE RegistrationKey = {0} " +
                    ") " +
                    "" +
                    "UPDATE HostRegistration " +
                    "SET APIToken = {1} " +
                    "WHERE RegistrationKey = {0} " +
                    "" +
                    "SELECT ID " +
                    "FROM HostRegistration " +
                    "WHERE RegistrationKey = {0}";

                string hostname = GetHostName();
                string apiToken = GenerateAPIToken();
                string url = $"http://{hostname}:8989";
                return connection.ExecuteScalar<int>(QueryFormat, registrationKey, apiToken, url);
            }
        }

        private Action<object> GetConfigurator()
        {
            ConfigurationLoader configurationLoader = new ConfigurationLoader(ID, CreateDbConnection);
            return configurationLoader.Configure;
        }

        private void UpdateDbTimeout(Action<object> configurator)
        {
            Settings settings = new Settings(configurator);
            DbTimeout = settings.SystemSettings.DbTimeout;
        }

        private async Task RunTimerAsync(CancellationToken cancellationToken)
        {
            void CheckIn(AdoDataConnection connection)
            {
                const string QueryFormat =
                    "UPDATE HostRegistration " +
                    "SET CheckedIn = GETUTCDATE() " +
                    "WHERE ID = {0} " +
                    "" +
                    "UPDATE Node " +
                    "SET HostRegistrationID = NULL " +
                    "WHERE HostRegistrationID NOT IN " +
                    "( " +
                    "    SELECT ID " +
                    "    FROM ActiveHost " +
                    ")";

                connection.ExecuteNonQuery(QueryFormat, ID);
            }

            bool PollConfiguration()
            {
                List<Node> equilibrium;
                List<Node> activeConfiguration;

                using (AdoDataConnection connection = CreateDbConnection())
                {
                    CheckIn(connection);

                    List<HostRegistration> activeHosts = QueryActiveHosts(connection);
                    List<Node> activeNodes = QueryActiveNodes(connection, activeHosts.Count);
                    equilibrium = GetEquilibrium(activeHosts, activeNodes);
                    RetireNodes(connection, equilibrium);

                    activeConfiguration = UpdateNodeConfiguration(connection, equilibrium);
                    ActivateNodes(connection, activeConfiguration);
                }

                IEnumerable<int> equilibriumIDs = equilibrium.Select(node => node.ID);
                IEnumerable<int> activeIDs = activeConfiguration.Select(node => node.ID);
                return !equilibriumIDs.Except(activeIDs).Any();
            }

            await Task.Run(async () =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    Stopwatch stopwatch = Stopwatch.StartNew();
                    bool atEquilibrium = false;

                    try { atEquilibrium = PollConfiguration(); }
                    catch (Exception ex) { Log.Error(ex.Message, ex); }

                    TimeSpan totalDelay = atEquilibrium
                        ? TimeSpan.FromMinutes(1)
                        : TimeSpan.FromSeconds(10);

                    TimeSpan remainingDelay = totalDelay - stopwatch.Elapsed;

                    if (remainingDelay > TimeSpan.Zero)
                    {
                        try { await Task.Delay(remainingDelay, cancellationToken); }
                        catch (TaskCanceledException) { }
                    }
                }
            });
        }

        private List<HostRegistration> QueryActiveHosts(AdoDataConnection connection)
        {
            const string Query = "SELECT * FROM ActiveHost ORDER BY ID";
            TableOperations<HostRegistration> hostRegistrationTable = new TableOperations<HostRegistration>(connection);

            using (DataTable result = connection.RetrieveData(Query))
            {
                return result
                    .AsEnumerable()
                    .Select(hostRegistrationTable.LoadRecord)
                    .ToList();
            }
        }

        private List<Node> QueryActiveNodes(AdoDataConnection connection, int hostCount)
        {
            TableOperations<Node> nodeTable = new TableOperations<Node>(connection);
            RecordRestriction restriction = new RecordRestriction("MinimumHostCount <= {0}", hostCount);

            return nodeTable
                .QueryRecords("MinimumHostCount ASC, ID ASC", restriction)
                .ToList();
        }

        private List<Node> GetEquilibrium(List<HostRegistration> activeHosts, List<Node> activeNodes)
        {
            List<Node> localNodes = new List<Node>();

            int hostLoadFactor = activeNodes.Count / activeHosts.Count;
            int hostOverflow = activeNodes.Count % activeHosts.Count;

            var nodeTypeAssignments = activeNodes
                .GroupBy(node => node.NodeTypeID)
                .ToDictionary(grouping => grouping.Key, grouping =>
                {
                    int nodeTypeID = grouping.Key;
                    int nodeCount = grouping.Count();
                    int loadFactor = nodeCount / activeHosts.Count;
                    int overflow = nodeCount % activeHosts.Count;

                    return new
                    {
                        NodeTypeID = nodeTypeID,
                        LoadFactor = loadFactor,
                        HasOverflow = new Func<bool>(() => overflow > 0),
                        AssignOverflow = new Action(() => overflow--)
                    };
                });

            var hostAssignments = activeHosts
                .Select(host =>
                {
                    HashSet<int> fullSlotTypes = new HashSet<int>();

                    Dictionary<int, int> typedSlots = nodeTypeAssignments.Values
                        .ToDictionary(assignment => assignment.NodeTypeID, assignment => assignment.LoadFactor);

                    int freeSlots = hostLoadFactor - typedSlots.Values.Sum();
                    int overflowSlots = 1;
                    int totalAssigned = 0;

                    int GetTotalAssigned() => totalAssigned;
                    bool CanAssign(int nodeType) => !fullSlotTypes.Contains(nodeType);
                    bool HasTypedSlot(int nodeType) => typedSlots[nodeType] > 0;
                    bool HasFreeSlot() => freeSlots > 0;
                    bool HasOverflowSlot() => overflowSlots > 0;

                    void AssignTyped(Node node)
                    {
                        if (host.ID == ID)
                            localNodes.Add(node);

                        int nodeTypeID = node.NodeTypeID;
                        typedSlots[nodeTypeID]--;
                        totalAssigned++;
                    }

                    void AssignFree(Node node)
                    {
                        if (host.ID == ID)
                            localNodes.Add(node);

                        int nodeTypeID = node.NodeTypeID;
                        fullSlotTypes.Add(nodeTypeID);
                        freeSlots--;
                        totalAssigned++;
                    }

                    void AssignOverflow(Node node)
                    {
                        if (host.ID == ID)
                            localNodes.Add(node);

                        int nodeTypeID = node.NodeTypeID;
                        fullSlotTypes.Add(nodeTypeID);
                        overflowSlots--;
                        totalAssigned++;
                    }

                    return new
                    {
                        HostID = host.ID,
                        GetTotalAssigned = new Func<int>(GetTotalAssigned),
                        CanAssignNodeType = new Func<int, bool>(CanAssign),
                        HasTypedSlot = new Func<int, bool>(HasTypedSlot),
                        HasFreeSlot = new Func<bool>(HasFreeSlot),
                        HasOverflowSlot = new Func<bool>(HasOverflowSlot),
                        AssignTyped = new Action<Node>(AssignTyped),
                        AssignFree = new Action<Node>(AssignFree),
                        AssignOverflow = new Action<Node>(AssignOverflow)
                    };
                })
                .ToList();

            foreach (Node node in activeNodes)
            {
                var prioritizedHostAssignments = hostAssignments
                    .OrderBy(hostAssignment => hostAssignment.HostID == node.HostRegistrationID ? 0 : 1)
                    .ThenBy(hostAssignment => hostAssignment.GetTotalAssigned())
                    .ThenBy(hostAssignment => hostAssignment.HostID);

                foreach (var hostAssignment in prioritizedHostAssignments)
                {
                    int nodeTypeID = node.NodeTypeID;

                    if (!hostAssignment.CanAssignNodeType(nodeTypeID))
                        continue;

                    if (hostAssignment.HasTypedSlot(nodeTypeID))
                    {
                        hostAssignment.AssignTyped(node);
                        break;
                    }

                    var nodeTypeAssignment = nodeTypeAssignments[nodeTypeID];

                    if (!nodeTypeAssignment.HasOverflow())
                        continue;

                    if (hostAssignment.HasFreeSlot())
                    {
                        nodeTypeAssignment.AssignOverflow();
                        hostAssignment.AssignFree(node);
                        break;
                    }

                    if (hostOverflow <= 0)
                        continue;

                    if (!hostAssignment.HasOverflowSlot())
                        continue;

                    hostOverflow--;
                    nodeTypeAssignment.AssignOverflow();
                    hostAssignment.AssignOverflow(node);
                    break;
                }
            }

            return localNodes;
        }

        private List<Node> UpdateNodeConfiguration(AdoDataConnection connection, List<Node> equilibrium)
        {
            List<int> nodeIDs = equilibrium.Select(node => node.ID).ToList();
            IEnumerable<string> parameterFormats = nodeIDs.Select((id, index) => $"{{{index + 1}}}");
            string idParameters = string.Join(",", parameterFormats);

            string queryFormat =
                $"UPDATE Node " +
                $"SET HostRegistrationID = {{0}} " +
                $"WHERE " +
                $"    ID IN ({idParameters}) AND " +
                $"    HostRegistrationID IS NULL " +
                $"" +
                $"SELECT * " +
                $"FROM Node " +
                $"WHERE HostRegistrationID = {{0}}";

            object[] parameters = new object[] { ID }
                .Concat(nodeIDs.Cast<object>())
                .ToArray();

            using (DataTable assignedNodes = connection.RetrieveData(queryFormat, parameters))
            {
                TableOperations<Node> nodeTable = new TableOperations<Node>(connection);

                return assignedNodes
                    .AsEnumerable()
                    .Select(nodeTable.LoadRecord)
                    .ToList();
            }
        }

        private void ActivateNodes(AdoDataConnection connection, List<Node> activeConfiguration)
        {
            NodeType QueryNodeType(int nodeTypeID)
            {
                TableOperations<NodeType> nodeTypeTable = new TableOperations<NodeType>(connection);
                return nodeTypeTable.QueryRecordWhere("ID = {0}", nodeTypeID);
            }

            bool IsMatch(Node definition1, Node definition2) =>
                definition1.ID == definition2.ID &&
                definition1.Name == definition2.Name &&
                definition1.NodeTypeID == definition2.NodeTypeID;

            int GetDefinitionID(Node definition) => definition.ID;
            int GetNodeID(INode node) => node.Definition.ID;

            List<INode> nodes = Nodes.Values.ToList();

            List<INode> activeNodes = nodes
                .Join(activeConfiguration, GetNodeID, GetDefinitionID, (Node, Definition) => new { Node, Definition })
                .Where(join => IsMatch(join.Node.Definition, join.Definition))
                .Select(join => join.Node)
                .ToList();

            INode ToNode(Node definition)
            {
                int typeID = definition.NodeTypeID;
                NodeType type = QueryNodeType(typeID);
                return CreateNodeInstance(definition, type);
            }

            IEnumerable<INode> inactiveNodes = activeConfiguration
                .GroupJoin(activeNodes, GetDefinitionID, GetNodeID, (definition, grouping) => !grouping.Any() ? definition : null)
                .Where(definition => !(definition is null))
                .Select(ToNode);

            Nodes = activeNodes
                .Concat(inactiveNodes)
                .ToDictionary(GetNodeID);
        }

        private void RetireNodes(AdoDataConnection connection, List<Node> equilibrium)
        {
            bool IsMatch(Node definition1, Node definition2) =>
                definition1.ID == definition2.ID &&
                definition1.Name == definition2.Name &&
                definition1.NodeTypeID == definition2.NodeTypeID;

            int GetDefinitionID(Node definition) => definition.ID;
            int GetNodeID(INode node) => node.Definition.ID;

            List<INode> nodes = Nodes.Values.ToList();

            List<INode> activeNodes = nodes
                .Join(equilibrium, GetNodeID, GetDefinitionID, (Node, Definition) => new { Node, Definition })
                .Where(join => IsMatch(join.Node.Definition, join.Definition))
                .Select(join => join.Node)
                .ToList();

            List<INode> retiredNodes = nodes
                .Except(activeNodes)
                .ToList();

            if (!retiredNodes.Any())
                return;

            Nodes = activeNodes.ToDictionary(GetNodeID);

            foreach (INode node in retiredNodes)
                TryDispose(node);

            List<int> nodeIDs = retiredNodes.Select(node => node.Definition.ID).ToList();
            IEnumerable<string> parameterFormats = nodeIDs.Select((id, index) => $"{{{index}}}");
            string idParameters = string.Join(",", parameterFormats);

            string queryFormat =
                $"UPDATE Node " +
                $"SET HostRegistrationID = NULL " +
                $"WHERE ID IN ({idParameters})";

            object[] parameters = nodeIDs
                .Cast<object>()
                .ToArray();

            connection.ExecuteNonQuery(queryFormat, parameters);
        }

        private INode CreateNodeInstance(Node definition, NodeType type)
        {
            try
            {
                // Since this handles dispose logic, it does not need to execute continuations asynchronously
                TaskCompletionSource<object> disposeTaskSource = new TaskCompletionSource<object>();
                DisposeTasks.TryAdd(definition.ID, disposeTaskSource);

                string assemblyName = type.AssemblyName;
                string typeName = type.TypeName;
                PluginFactory<INode> pluginFactory = new PluginFactory<INode>();
                return pluginFactory.Create(assemblyName, typeName, this, definition, type);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                return null;
            }
        }

        private void TryDispose(INode node)
        {
            try
            {
                using (node as IDisposable)
                {
                    int nodeID = node.Definition.ID;

                    if (DisposeTasks.TryRemove(nodeID, out TaskCompletionSource<object> disposeTaskSource))
                        disposeTaskSource.SetResult(null);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
            }
        }

        #endregion

        #region [ Static ]

        // Static Fields
        private static readonly ILog Log = LogManager.GetLogger(typeof(Host));

        // Static Properties
        private static HttpClient HttpClient { get; }
            = new HttpClient();

        private static ConcurrentDictionary<int, TaskCompletionSource<object>> DisposeTasks { get; }
            = new ConcurrentDictionary<int, TaskCompletionSource<object>>();

        #endregion
    }
}
