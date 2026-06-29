//******************************************************************************************************
//  WatchDirectoryBalancerNode.cs - Gbtc
//
//  Copyright © 2026, Grid Protection Alliance.  All Rights Reserved.
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
//  06/26/2026 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using GSF.Configuration;
using GSF.Data;
using GSF.Data.Model;
using GSF.IO;
using GSF.Threading;
using log4net;
using openXDA.Configuration;
using openXDA.Model;
using openXDA.Nodes.Types.FileProcessing;

namespace openXDA.Nodes.Types.WatchDirectoryBalancing
{
    public class WatchDirectoryBalancerNode : NodeBase
    {
        #region [ Members ]

        // Nested Types
        private class Settings
        {
            public Settings(Action<object> configure) =>
                configure(this);

            [Category]
            [SettingName(WatchDirectoryBalancerSection.CategoryName)]
            public WatchDirectoryBalancerSection WatchDirectoryBalancerSettings { get; } = new WatchDirectoryBalancerSection();
        }

        private class WatchDirectoryBalancerWebController : ApiController
        {
            private WatchDirectoryBalancerNode Node { get; }

            public WatchDirectoryBalancerWebController(WatchDirectoryBalancerNode node) =>
                Node = node;

            [HttpPost]
            public void Balance() =>
                Node.BalanceOperation.RunOnceAsync();
        }

        private class NodeAssignment
        {
            public NodeAssignment(int nodeID, string watchDirectory)
            {
                NodeID = nodeID;
                WatchDirectory = watchDirectory;
            }

            public int NodeID { get; }
            public string WatchDirectory { get; }
        }

        #endregion

        #region [ Constructors ]

        public WatchDirectoryBalancerNode(Host host, Node definition, NodeType type)
            : base(host, definition, type)
        {
            BalanceOperation = new ShortSynchronizedOperation(Balance, HandleException);
            BalanceOperation.RunOnceAsync();
        }

        #endregion

        #region [ Properties ]

        private ISynchronizedOperation BalanceOperation { get; }

        #endregion

        #region [ Methods ]

        public override IHttpController CreateWebController() =>
            new WatchDirectoryBalancerWebController(this);

        protected override void OnReconfigure(Action<object> configurator) =>
            ScheduleAutoBalance(configurator);

        private void ScheduleAutoBalance(Action<object> configurator)
        {
            Settings settings = new Settings(configurator);
            string name = nameof(AutoBalance);
            string schedule = settings.WatchDirectoryBalancerSettings.Schedule;
            Host.RegisterScheduledProcess(this, AutoBalance, name, schedule);
        }

        private void AutoBalance() =>
            BalanceOperation.RunOnce();

        private void Balance()
        {
            Action<object> configurator = GetConfigurator();
            Settings settings = new Settings(configurator);
            string baseDirectory = settings.WatchDirectoryBalancerSettings.BaseDirectory;
            string searchPattern = settings.WatchDirectoryBalancerSettings.SearchPattern;
            int searchDepth = settings.WatchDirectoryBalancerSettings.SearchDepth;
            int balanceThreshold = settings.WatchDirectoryBalancerSettings.BalanceThreshold;

            if (string.IsNullOrEmpty(baseDirectory)) return;
            if (string.IsNullOrEmpty(searchPattern)) return;
            if (searchDepth <= 0) return;

            using (AdoDataConnection connection = CreateDbConnection())
            {
                List<NodeSetting> nodeSettings = QueryNodeSettings(connection);
                List<NodeAssignment> assignments = BuildWatchDirectoryAssignments(nodeSettings);
                List<string> watchDirectories = Search(baseDirectory, searchPattern, searchDepth);

                List<NodeAssignment> prunedAssignments = PruneStaleAssignments(assignments, watchDirectories);
                List<NodeAssignment> augmentedAssignments = AssignMissingDirectories(nodeSettings, prunedAssignments, watchDirectories);
                List<NodeAssignment> balancedAssignments = BalanceAssignments(nodeSettings, augmentedAssignments, balanceThreshold);

                // Prune, AssignMissing, and Balance operations must each return the
                // input list if no changes were made; otherwise, this comparison is invalid
                if (ReferenceEquals(assignments, balancedAssignments))
                    return;

                UploadWatchDirectoryAssignments(connection, nodeSettings, balancedAssignments);
                _ = TryNotifyFileProcessorNodesAsync();
            }
        }

        private List<NodeAssignment> PruneStaleAssignments(List<NodeAssignment> assignments, List<string> watchDirectories)
        {
            string FromWatchDirectory(string watchDirectory) => watchDirectory.ToLower();
            string FromAssignment(NodeAssignment assignment) => FromWatchDirectory(assignment.WatchDirectory);

            List<NodeAssignment> prunedAssignments = assignments
                .Join(watchDirectories, FromAssignment, FromWatchDirectory, (assignment, _) => assignment)
                .ToList();

            // If nothing was removed, return the original list to indicate no changes were made
            return (assignments.Count != prunedAssignments.Count)
                ? prunedAssignments
                : assignments;
        }

        private List<NodeAssignment> AssignMissingDirectories(List<NodeSetting> settings, List<NodeAssignment> assignments, List<string> watchDirectories)
        {
            List<(int NodeID, int Count)> assignmentCounts = settings
                .GroupJoin(assignments, setting => setting.NodeID, assignment => assignment.NodeID, (setting, grouping) => (setting.NodeID, Count: grouping.Count()))
                .OrderBy(tuple => tuple.Count)
                .ToList();

            IEnumerable<string> assignedDirectories = assignments
                .Select(assignment => assignment.WatchDirectory);

            List<string> missingDirectories = watchDirectories
                .Except(assignedDirectories)
                .ToList();

            if (missingDirectories.Count == 0)
                return assignments;

            using (IEnumerator<string> enumerator = missingDirectories.GetEnumerator())
            {
                // Perform the assignments in stages where each stage assigns A[s]
                // directories to the collection of N[s]=s+1 nodes with the fewest assignments.
                // * A[0] is the number of nodes needed for node 0 to have the same number of assignments as node 1.
                // * A[1] is the number of assignments needed for node 1 to have the same number of assignments as node 2.
                // * A[0]+A[1] is the number of assignments needed for node 0 to have the same number of assignments as node 2.
                //
                // At the end of stage 0, node 0 should have the same number of assignments as node 1.
                // At the end of stage 1, nodes 0 and 1 should have the same number of assignments as node 2.
                // At the end of stage s, nodes 0..s should all have the same number of assignments as node s+1.
                // Assigning directories in this manner keeps the system as balanced as possible.
                IEnumerable<NodeAssignment> missingAssignments = assignmentCounts
                    .Skip(1)

                    // Computes A[s] for each stage
                    .Zip(assignmentCounts, (right, left) => right.Count - left.Count)

                    // Computes T[s]=A[s]*N[s] as the total number of directories to be assigned in each stage
                    .Select((count, stage) => count * (stage + 1))

                    // Adds an additional stage to ensure that all directories get assigned
                    .Append(missingDirectories.Count)

                    // Break the list of directories into partitions of size T[s]
                    // and assign them to the appropriate nodes in each stage
                    .Select(count => ToPartition(enumerator, count))
                    .SelectMany(ToAssignments);

                return assignments
                    .Concat(missingAssignments)
                    .ToList();
            }

            IEnumerable<T> ToPartition<T>(IEnumerator<T> enumerator, int count)
            {
                for (int i = 0; i < count; i++)
                {
                    if (!enumerator.MoveNext())
                        break;

                    yield return enumerator.Current;
                }
            }

            IEnumerable<NodeAssignment> ToAssignments(IEnumerable<string> partition, int stage)
            {
                int nodes = stage + 1;
                int ToNodeID(int index) => assignmentCounts[index % nodes].NodeID;
                return partition.Select((directory, index) => new NodeAssignment(ToNodeID(index), directory));
            }
        }

        private List<NodeAssignment> BalanceAssignments(List<NodeSetting> settings, List<NodeAssignment> assignments, int balanceThreshold)
        {
            int nodes = settings.Count;
            int total = assignments.Count;
            int perNode = total / nodes;
            int overflow = total % nodes;

            // Equilibrium is the point where all nodes have the same number of assignments
            IEnumerable<int> equilibrium = Enumerable
                .Repeat(perNode, nodes)
                .Select((num, index) => index < overflow ? num + 1 : num);

            List<(int NodeID, int Count, int Equilibrium)> equilibriumMap = settings
                .GroupJoin(assignments, setting => setting.NodeID, assignment => assignment.NodeID, (setting, grouping) => (setting.NodeID, Count: grouping.Count()))
                .OrderByDescending(tuple => tuple.Count)
                .Zip(equilibrium, (tuple, Equilibrium) => (tuple.NodeID, tuple.Count, Equilibrium))
                .ToList();

            // The number of reassignments required to reach equilibrium can be determined by counting
            // the number of assignments each node has in excess of its equilibrium point, which should
            // be the same as the number of assignments each node needs to reach its equilibrium point
            int moves = equilibriumMap
                .Select(tuple => tuple.Count - tuple.Equilibrium)
                .Sum(diff => Math.Min(0, diff));

            if (moves < balanceThreshold)
                return assignments;

            // The node IDs in this enumerable indicate which nodes need directories assigned to them to reach equilibrium
            IEnumerable<int> assignmentsForBalance = equilibriumMap
                .Where(tuple => tuple.Count < tuple.Equilibrium)
                .SelectMany(tuple => Enumerable.Repeat(tuple.NodeID, tuple.Equilibrium - tuple.Count));

            // The countdown functions indicate whether a node has any assignments left in excess of equilibrium
            IEnumerable<(int NodeID, Func<bool> Countdown)> counters = equilibriumMap
                .Select(tuple => (tuple.NodeID, ToCountdown(tuple.Count - tuple.Equilibrium)));

            using (IEnumerator<int> enumerator = assignmentsForBalance.GetEnumerator())
            {
                // Join existing assignments with countdown functions, then reassign any
                // assignment whose countdown function indicates that its node still has excess
                return assignments
                    .Join(counters, assignment => assignment.NodeID, counter => counter.NodeID, (Assignment, counter) => (Assignment, counter.Countdown))
                    .Select(tuple => tuple.Countdown() && enumerator.MoveNext() ? Reassign(tuple.Assignment, enumerator.Current) : tuple.Assignment)
                    .ToList();
            }

            Func<bool> ToCountdown(int initialCount) =>
                () => initialCount > 0 && --initialCount >= 0;

            NodeAssignment Reassign(NodeAssignment assignment, int newNodeID) =>
                new NodeAssignment(newNodeID, assignment.WatchDirectory);
        }

        private List<string> Search(string baseDirectory, string searchPattern, int searchDepth)
        {
            string[] fileSpecs = searchPattern.Split(Path.PathSeparator);

            return EnumerateDirectories(baseDirectory, searchDepth)
                .Where(path => FilePath.IsFilePatternMatch(fileSpecs, path, true))
                .ToList();
        }

        private IEnumerable<string> EnumerateDirectories(string baseDirectory, int depth)
        {
            if (depth <= 0)
                yield break;

            IEnumerable<string> subdirectories = FilePath.EnumerateDirectories(
                baseDirectory,
                searchOption: SearchOption.TopDirectoryOnly,
                exceptionHandler: HandleException);

            foreach (string subdirectory in subdirectories)
            {
                yield return subdirectory;

                if (depth == 1)
                    continue;

                foreach (string directory in EnumerateDirectories(subdirectory, depth - 1))
                    yield return directory;
            }
        }

        private List<NodeSetting> QueryNodeSettings(AdoDataConnection connection)
        {
            TableOperations<NodeSetting> nodeSettingTable = new TableOperations<NodeSetting>(connection);

            return nodeSettingTable
                .QueryRecordsWhere("Name = 'FileWatcher.WatchDirectories'")
                .ToList();
        }

        private List<NodeAssignment> BuildWatchDirectoryAssignments(List<NodeSetting> settings)
        {
            IEnumerable<string> ToWatchDirectoryList(NodeSetting setting) => setting.Value
                .Split(Path.PathSeparator)
                .Select(path => path.Trim());

            NodeAssignment ToNodeAssignment(NodeSetting setting, string watchDirectory) =>
                new NodeAssignment(setting.NodeID, watchDirectory);

            return settings
                .SelectMany(ToWatchDirectoryList, ToNodeAssignment)
                .ToList();
        }

        private void UploadWatchDirectoryAssignments(AdoDataConnection connection, List<NodeSetting> settings, List<NodeAssignment> assignments)
        {
            TableOperations<NodeSetting> nodeSettingTable = new TableOperations<NodeSetting>(connection);
            string pathSeparator = Path.PathSeparator.ToString();

            ILookup<int, string> watchDirectoryLookup = assignments
                .ToLookup(assignment => assignment.NodeID, assignment => assignment.WatchDirectory);

            foreach (NodeSetting setting in settings)
            {
                int nodeID = setting.NodeID;
                IEnumerable<string> watchDirectories = watchDirectoryLookup[nodeID];
                setting.Value = string.Join(pathSeparator, watchDirectories);
                nodeSettingTable.UpdateRecord(setting);
            }
        }

        private async Task TryNotifyFileProcessorNodesAsync()
        {
            try { await NotifyFileProcessorNodesAsync(); }
            catch (Exception ex) { HandleException(ex); }
        }

        private async Task NotifyFileProcessorNodesAsync()
        {
            using (HttpResponseMessage response = await Host.SendWebRequestAsync(ConfigureRequest))
            {
                response.EnsureSuccessStatusCode();
            }

            void ConfigureRequest(HttpRequestMessage request)
            {
                string url = Host.BuildURL(typeof(FileProcessorNode), "ReconfigureWatchDirectories");
                request.Method = HttpMethod.Post;
                request.RequestUri = new Uri(url);
            }
        }

        #endregion

        #region [ Static ]

        // Static Fields
        private static readonly ILog Log = LogManager.GetLogger(typeof(WatchDirectoryBalancerNode));

        // Static Methods
        private static void HandleException(Exception ex) =>
            Log.Error(ex.Message, ex);

        #endregion
    }
}
