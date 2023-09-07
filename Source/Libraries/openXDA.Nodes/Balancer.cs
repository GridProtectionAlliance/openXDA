//******************************************************************************************************
//  Balancer.cs - Gbtc
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
//  08/28/2023 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using GSF.Collections;
using openXDA.Model;
using HostTypeKey = System.Tuple<int, int>;

namespace openXDA.Nodes
{
    public class Balancer
    {
        #region [ Members ]

        // Nested Types
        private class NodeWrapper
        {
            public Node Node { get; }
            public int? HostID { get; set; }
            public int? HostIndex { get; set; }
            public int? HostTypeIndex { get; set; }

            public NodeWrapper(Node node) =>
                Node = node;
        }

        private class NodeTypeBalancerInfo
        {
            #region [ Constructors ]

            public NodeTypeBalancerInfo(int nodeTypeID, int hostCount, IEnumerable<NodeWrapper> nodes)
            {
                List<Node> balancedNodes = nodes
                    .Select(assignment => assignment.Node)
                    .ToList();

                int balancedHostCount = RemoveUnbalancedNodes(balancedNodes, hostCount);
                int nodeCount = balancedNodes.Count;

                NodeTypeID = nodeTypeID;
                LoadFactor = nodeCount / balancedHostCount;
                Overflow = nodeCount % balancedHostCount;
                BalancedHostCount = balancedHostCount;
                Nodes = nodes.ToList().AsReadOnly();
            }

            #endregion

            #region [ Properties ]

            public int NodeTypeID { get; }
            public int LoadFactor { get; }
            public int Overflow { get; }
            public int BalancedHostCount { get; }
            public IReadOnlyList<NodeWrapper> Nodes { get; }

            #endregion

            #region [ Static ]

            // Static Methods
            private static int RemoveUnbalancedNodes(List<Node> nodes, int hostCount)
            {
                int runningHostCount = hostCount;

                while (true)
                {
                    int runningLoadFactor = nodes.Count / runningHostCount;
                    int runningOverflow = nodes.Count % runningHostCount;

                    int GetOverflowCount(int index) =>
                        (index < runningOverflow) ? 1 : 0;

                    int GetNodeCount(int index) =>
                        runningLoadFactor + GetOverflowCount(index);

                    int? overSubscribedHostID = nodes
                        .Where(node => !(node.AssignedHostRegistrationID is null))
                        .GroupBy(node => node.AssignedHostRegistrationID.GetValueOrDefault())
                        .Select(grouping => new { HostID = grouping.Key, AssignedCount = grouping.Count() })
                        .OrderByDescending(grouping => grouping.AssignedCount)
                        .ThenBy(grouping => grouping.HostID)
                        .Select((grouping, index) => new { grouping.HostID, grouping.AssignedCount, BalancedCount = GetNodeCount(index) })
                        .Where(grouping => grouping.AssignedCount > grouping.BalancedCount)
                        .Select(grouping => (int?)grouping.HostID)
                        .FirstOrDefault();

                    if (overSubscribedHostID is null)
                        break;

                    nodes.RemoveWhere(node => node.AssignedHostRegistrationID == overSubscribedHostID);
                    runningHostCount--;
                }

                return runningHostCount;
            }

            #endregion
        }

        #endregion

        #region [ Constructors ]

        public Balancer(List<HostRegistration> activeHosts, List<Node> activeNodes)
        {
            foreach (Node node in activeNodes)
                Wrappers.Add(new NodeWrapper(node));

            ApplyForcedAssignments();
            InitializeNodeTypeBalancerInfo(activeHosts.Count);
            AssignLoad(activeHosts);
            AssignOverflow(activeHosts);
            TradeAssignments();
        }

        #endregion

        #region [ Properties ]

        private List<NodeWrapper> Wrappers { get; }
            = new List<NodeWrapper>();

        private List<NodeTypeBalancerInfo> NodeTypeBalancerInfoList { get; }
            = new List<NodeTypeBalancerInfo>();

        private Dictionary<int, List<NodeWrapper>> HostLookup { get; }
            = new Dictionary<int, List<NodeWrapper>>();

        private Dictionary<HostTypeKey, List<NodeWrapper>> HostTypeLookup { get; }
            = new Dictionary<HostTypeKey, List<NodeWrapper>>();

        #endregion

        #region [ Methods ]

        public List<Node> GetEquilibrium(int hostID)
        {
            List<NodeWrapper> nodes = GetNodeList(hostID);

            return nodes
                .Select(wrapper => wrapper.Node)
                .ToList();
        }

        private void ApplyForcedAssignments()
        {
            foreach (NodeWrapper wrapper in Wrappers)
            {
                int? assignedHostID = wrapper.Node.AssignedHostRegistrationID;

                if (!(assignedHostID is null))
                    Assign(wrapper, assignedHostID.GetValueOrDefault());
            }
        }

        private void InitializeNodeTypeBalancerInfo(int hostCount)
        {
            IEnumerable<NodeTypeBalancerInfo> infos = Wrappers
                .GroupBy(wrapper => wrapper.Node.NodeTypeID)
                .Select(grouping => new NodeTypeBalancerInfo(grouping.Key, hostCount, grouping));

            NodeTypeBalancerInfoList.AddRange(infos);
        }

        private void AssignLoad(List<HostRegistration> activeHosts)
        {
            foreach (NodeTypeBalancerInfo info in NodeTypeBalancerInfoList)
            {
                int nodeTypeID = info.NodeTypeID;

                IEnumerable<NodeWrapper> unassignedNodes = info.Nodes
                    .Where(wrapper => wrapper.HostID is null);

                var loadMappings = Enumerable
                    .Range(0, info.LoadFactor)
                    .SelectMany(_ => activeHosts, (LoadIndex, Host) => new { Host, LoadIndex })
                    .Where(hostWrapper => GetNodeList(hostWrapper.Host.ID, nodeTypeID).Count <= hostWrapper.LoadIndex)
                    .Zip(unassignedNodes, (hostWrapper, Node) => new { hostWrapper.Host, Node })
                    .ToList();

                foreach (var mapping in loadMappings)
                    Assign(mapping.Node, mapping.Host.ID);
            }
        }

        private void AssignOverflow(List<HostRegistration> activeHosts)
        {
            IEnumerable<NodeTypeBalancerInfo> prioritizedNodeTypeBalancerInfo = NodeTypeBalancerInfoList
                .OrderBy(info => info.BalancedHostCount)
                .ThenByDescending(info => info.Overflow)
                .ThenBy(info => info.NodeTypeID);

            foreach (NodeTypeBalancerInfo info in prioritizedNodeTypeBalancerInfo)
            {
                int nodeTypeID = info.NodeTypeID;

                IEnumerable<NodeWrapper> unassignedNodes = info.Nodes
                    .Where(wrapper => wrapper.HostID is null);

                var overflowMappings = activeHosts
                    .Select(Host => new { Host, NodeTypeList = GetNodeList(Host.ID, nodeTypeID) })
                    .Where(hostWrapper => hostWrapper.NodeTypeList.Count <= info.LoadFactor)
                    .OrderBy(hostWrapper => hostWrapper.NodeTypeList.Count)
                    .ThenBy(hostWrapper => GetNodeList(hostWrapper.Host.ID).Count)
                    .Zip(unassignedNodes, (hostWrapper, Node) => new { hostWrapper.Host, Node })
                    .ToList();

                foreach (var mapping in overflowMappings)
                    Assign(mapping.Node, mapping.Host.ID);
            }
        }

        private void TradeAssignments()
        {
            foreach (NodeWrapper wrapper in Wrappers)
            {
                if (!(wrapper.Node.AssignedHostRegistrationID is null))
                    continue;

                if (wrapper.Node.HostRegistrationID is null)
                    continue;

                if (wrapper.Node.HostRegistrationID == wrapper.HostID)
                    continue;

                int sourceHostID = wrapper.HostID.GetValueOrDefault();
                int targetHostID = wrapper.Node.HostRegistrationID.GetValueOrDefault();
                int nodeTypeID = wrapper.Node.NodeTypeID;

                List<NodeWrapper> sourceHostList = GetNodeList(sourceHostID);
                List<NodeWrapper> sourceHostTypeList = GetNodeList(sourceHostID, nodeTypeID);
                List<NodeWrapper> targetHostList = GetNodeList(targetHostID);
                List<NodeWrapper> targetHostTypeList = GetNodeList(targetHostID, nodeTypeID);

                if (sourceHostList.Count > targetHostList.Count && sourceHostTypeList.Count > targetHostTypeList.Count)
                {
                    Assign(wrapper, targetHostID);
                    continue;
                }

                NodeWrapper trade = targetHostTypeList
                    .Where(tr => tr.Node.HostRegistrationID != tr.HostID)
                    .FirstOrDefault();

                if (!(trade is null))
                {
                    Assign(wrapper, targetHostID);
                    Assign(trade, sourceHostID);
                    continue;
                }

                if (sourceHostTypeList.Count <= targetHostTypeList.Count)
                    continue;

                bool CanTrade(NodeWrapper tr)
                {
                    int tradeNodeTypeID = tr.Node.NodeTypeID;
                    List<NodeWrapper> sourceTradeTypeList = GetNodeList(sourceHostID, tradeNodeTypeID);
                    List<NodeWrapper> targetTradeTypeList = GetNodeList(targetHostID, tradeNodeTypeID);
                    return targetTradeTypeList.Count > sourceTradeTypeList.Count;
                }

                trade = targetHostList
                    .Where(tr => tr.Node.NodeTypeID != nodeTypeID)
                    .Where(tr => tr.Node.HostRegistrationID != tr.HostID)
                    .Where(CanTrade)
                    .FirstOrDefault();

                if (!(trade is null))
                {
                    Assign(wrapper, targetHostID);
                    Assign(trade, sourceHostID);
                }
            }
        }

        private void Assign(NodeWrapper wrapper, int hostID)
        {
            if (wrapper.HostID == hostID)
                return;

            Unassign(wrapper);

            int nodeTypeID = wrapper.Node.NodeTypeID;
            List<NodeWrapper> newHostList = GetNodeList(hostID);
            List<NodeWrapper> newHostTypeList = GetNodeList(hostID, nodeTypeID);

            wrapper.HostID = hostID;
            wrapper.HostIndex = newHostList.Count;
            wrapper.HostTypeIndex = newHostTypeList.Count;

            newHostList.Add(wrapper);
            newHostTypeList.Add(wrapper);
        }

        private void Unassign(NodeWrapper wrapper)
        {
            if (wrapper.HostID is null)
                return;

            int hostID = wrapper.HostID.GetValueOrDefault();
            int hostIndex = wrapper.HostIndex.GetValueOrDefault();
            List<NodeWrapper> hostList = GetNodeList(hostID);
            NodeWrapper lastOnHost = hostList[hostList.Count - 1];
            lastOnHost.HostIndex = hostIndex;
            hostList[hostIndex] = lastOnHost;
            hostList.RemoveAt(hostList.Count - 1);

            int nodeTypeID = wrapper.Node.NodeTypeID;
            int hostTypeIndex = wrapper.HostTypeIndex.GetValueOrDefault();
            List<NodeWrapper> hostTypeList = GetNodeList(hostID, nodeTypeID);
            NodeWrapper lastForHostType = hostTypeList[hostTypeList.Count - 1];
            lastForHostType.HostTypeIndex = hostTypeIndex;
            hostTypeList[hostTypeIndex] = lastForHostType;
            hostTypeList.RemoveAt(hostTypeList.Count - 1);

            wrapper.HostID = null;
            wrapper.HostIndex = null;
            wrapper.HostTypeIndex = null;
        }

        private List<NodeWrapper> GetNodeList(int hostID) =>
            HostLookup.GetOrAdd(hostID, _ => new List<NodeWrapper>());

        private List<NodeWrapper> GetNodeList(int hostID, int nodeTypeID)
        {
            HostTypeKey hostTypeKey = GetHostTypeKey(hostID, nodeTypeID);
            return HostTypeLookup.GetOrAdd(hostTypeKey, _ => new List<NodeWrapper>());
        }

        private HostTypeKey GetHostTypeKey(int hostID, int nodeTypeID) =>
            Tuple.Create(hostID, nodeTypeID);

        #endregion
    }
}
