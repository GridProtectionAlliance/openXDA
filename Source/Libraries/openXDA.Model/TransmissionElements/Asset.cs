//******************************************************************************************************
//  Asset.cs - Gbtc
//
//  Copyright © 2019, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the MIT License (MIT), the "License"; you may
//  not use this file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://opensource.org/licenses/MIT
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  12/12/2019 - C. Lackner
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using GSF;
using GSF.Collections;
using GSF.Data;
using GSF.Data.Model;
using Newtonsoft.Json;

namespace openXDA.Model
{
    [AllowSearch]
    [PostRoles("Administrator, Transmission SME")]
    [DeleteRoles("Administrator, Transmission SME")]
    [PatchRoles("Administrator, Transmission SME")]
    [SettingsCategory("systemSettings")]
    public class Asset
    {
        #region [ Members ]

        // Fields
        private List<AssetLocation> m_assetLocations;
        private List<MeterAsset> m_meterAssets;
        private List<Channel> m_directChannels;
        private List<Channel> m_connectedChannels;
        private List<AssetConnection> m_connections;

        #endregion

        #region [ Properties ]

        [PrimaryKey(true)]
        public int ID { get; set; }

        [Required]
        public double VoltageKV { get; set; }

        [Required]
        [StringLength(50)]
        [Searchable]
        [DefaultSortOrder]
        public string AssetKey { get; set; }

        public string Description { get; set; }

        [DefaultValue("")]
        public string AssetName { get; set; }

        [Required]
        public int AssetTypeID {get; set; }

        public bool Spare { get; set; }

        [JsonIgnore]
        [NonRecordField]
        internal LazyContext LazyContext { get; set; } = new LazyContext();


        [JsonIgnore]
        [NonRecordField]
        public List<AssetLocation> AssetLocations
        {
            get
            {
                return m_assetLocations ?? (m_assetLocations = QueryAssetLocations());
            }
            set
            {
                m_assetLocations = value;
            }
        }

        [JsonIgnore]
        [NonRecordField]
        public List<MeterAsset> MeterAssets
        {
            get
            {
                return m_meterAssets ?? (m_meterAssets = QueryMeterAssets());
            }
            set
            {
                m_meterAssets = value;
            }
        }

        [JsonIgnore]
        [NonRecordField]
        public List<Channel> DirectChannels
        {
            get
            {
                return m_directChannels ?? (m_directChannels = QueryChannels());
            }
            set
            {
                m_directChannels = value;
            }
        }

        [JsonIgnore]
        [NonRecordField]
        public List<AssetConnection> Connections
        {
            get
            {
                return m_connections ?? (m_connections = QueryConnections());
            }
            set
            {
                m_connections = value;
            }
        }

        [JsonIgnore]
        [NonRecordField]
        public List<Channel> ConnectedChannels
        {
            get
            {
                return m_connectedChannels ?? (m_connectedChannels = QueryConnectedChannels());
            }
            set
            {
                m_connectedChannels = value;
            }
        }

        [JsonIgnore]
        [NonRecordField]
        public List<Asset> ConnectedAssets
        {
            get
            {
                return Connections.Where(item => item.ChildID == ID).Select(item => item.Parent).Concat(
                    Connections.Where(item => item.ParentID == ID).Select(item => item.Child)).ToList();
            }
            
        }

        [JsonIgnore]
        [NonRecordField]
        public Func<AdoDataConnection> ConnectionFactory
        {
            get
            {
                return LazyContext.ConnectionFactory;
            }
            set
            {
                LazyContext.ConnectionFactory = value;
            }
        }

        #endregion

        #region [ Methods ]

        public IEnumerable<AssetLocation> GetAssetLocations(AdoDataConnection connection)
        {
            if (connection is null)
                return null;

            TableOperations<AssetLocation> assetLocationTable = new TableOperations<AssetLocation>(connection);
            return assetLocationTable.QueryRecordsWhere("AssetID = {0}", ID);
        }

        private List<AssetLocation> QueryAssetLocations()
        {
            List<AssetLocation> assetLocations;

            using (AdoDataConnection connection = ConnectionFactory?.Invoke())
            {
                assetLocations = GetAssetLocations(connection)?
                    .Select(LazyContext.GetAssetLocation)
                    .ToList();
            }

            if (!(assetLocations is null))
            {
                foreach (AssetLocation assetLocation in assetLocations)
                {
                    assetLocation.Asset = this;
                    assetLocation.LazyContext = LazyContext;
                }
            }
            else
                return new List<AssetLocation>();

            return assetLocations;
        }

        private List<MeterAsset> QueryMeterAssets()
        {
            List<MeterAsset> meterAssets;

            using (AdoDataConnection connection = ConnectionFactory?.Invoke())
            {
                meterAssets = GetMeterAssets(connection)?
                    .Select(LazyContext.GetMeterAsset)
                    .ToList();
            }

            if (!(meterAssets is null))
            {
                foreach (MeterAsset meterAsset in meterAssets)
                {
                    meterAsset.Asset = this;
                    meterAsset.LazyContext = LazyContext;
                }
            }

            return meterAssets;
        }

        private List<Channel> QueryChannels()
        {
            List<Channel> channels;

            using (AdoDataConnection connection = ConnectionFactory?.Invoke())
            {
                channels = GetChannels(connection)?
                    .Select(LazyContext.GetChannel)
                    .ToList();
            }

            if (!(channels is null))
            {
                foreach (Channel channel in channels)
                {
                    channel.Asset = this;
                    channel.LazyContext = LazyContext;
                }
            }

            return channels;
        }

        private List<Channel> QueryConnectedChannels()
        {
            List<Channel> channels;

            using (AdoDataConnection connection = ConnectionFactory?.Invoke())
            {
                channels = GetConnectedChannels(connection)?
                    .Select(LazyContext.GetChannel)
                    .ToList();
            }

            if (!(channels is null))
            {
                foreach (Channel channel in channels)
                {
                    channel.LazyContext = LazyContext;
                }
            }

            return channels;
        }

        private List<AssetConnection> QueryConnections()
        {
            List<AssetConnection> connections;

            using (AdoDataConnection connection = ConnectionFactory?.Invoke())
            {
                connections = GetConnections(connection)?
                    .Select(LazyContext.GetAssetConnection)
                    .ToList();
            }

            if (!(connections is null))
            {
                foreach (AssetConnection connection in connections)
                {
                    connection.LazyContext = LazyContext;
                }
            }

            return connections;
        }

        public IEnumerable<MeterAsset> GetMeterAssets(AdoDataConnection connection)
        {
            if (connection is null)
                return null;

            TableOperations<MeterAsset> meterAssetTable = new TableOperations<MeterAsset>(connection);
            return meterAssetTable.QueryRecordsWhere("AssetID = {0}", ID);
        }

        public IEnumerable<Channel> GetChannels(AdoDataConnection connection)
        {
            if (connection is null)
                return null;

            TableOperations<Channel> channelTable = new TableOperations<Channel>(connection);
            return channelTable.QueryRecordsWhere("AssetID = {0}", ID);
        }

        public IEnumerable<AssetConnection> GetConnections(AdoDataConnection connection)
        {
            if (connection is null)
                return null;

            TableOperations<AssetConnection> channelTable = new TableOperations<AssetConnection>(connection);
            return channelTable.QueryRecordsWhere("ParentID = {0} OR ChildID = {1}", ID, ID);
        }

        // Logic for Channels across Asset Connections
        public IEnumerable<Channel> GetConnectedChannels(AdoDataConnection connection)
        {
            return TraverseConnectedChannels(connection, ID, ID, new Stack<int>())
                .Distinct(new ChannelComparer());
        }

        private IEnumerable<Channel> TraverseConnectedChannels(AdoDataConnection connection, int assetID, int visitedAssetID, Stack<int> visitedAssets)
        {
            const string TraversalQueryFormat =
                "SELECT DISTINCT " +
                "    ConnectedAsset.ID ConnectedAssetID, " +
                "    AssetConnection.ParentID, " +
                "    AssetConnection.ChildID, " +
                "    AssetRelationshipType.JumpConnection JumpSQL, " +
                "    AssetRelationshipType.PassThrough PassthroughSQL " +
                "FROM " +
                "    Asset JOIN " +
                "    Asset VisitedAsset ON " +
                "        Asset.ID = {0} AND " +
                "        VisitedAsset.ID = {1} JOIN " +
                "    AssetConnection ON VisitedAsset.ID IN (AssetConnection.ParentID, AssetConnection.ChildID) JOIN " +
                "    Asset ConnectedAsset ON " +
                "        ConnectedAsset.ID IN (AssetConnection.ParentID, AssetConnection.ChildID) AND " +
                "        ConnectedAsset.ID <> VisitedAsset.ID JOIN " +
                "    AssetRelationshipType ON AssetConnection.AssetRelationshipTypeID = AssetRelationshipType.ID JOIN " +
                "    MeterAsset ON MeterAsset.AssetID = Asset.ID JOIN " +
                "    MeterAsset ConnectedMeterAsset ON " +
                "        ConnectedMeterAsset.MeterID = MeterAsset.MeterID AND " +
                "        ConnectedMeterAsset.AssetID = ConnectedAsset.ID";

            const string JumpChannelsQueryFormat =
                "SELECT Channel.* " +
                "FROM " +
                "    Asset JOIN " +
                "    Asset ConnectedAsset ON " +
                "        Asset.ID = {{0}} AND " +
                "        ConnectedAsset.ID = {{1}} JOIN " +
                "    Asset ParentAsset ON ParentAsset.ID = {{2}} JOIN " +
                "    Asset ChildAsset ON ParentAsset.ID = {{3}} JOIN " +
                "    Channel ON Channel.AssetID = ConnectedAsset.ID JOIN " +
                "    MeterAsset ON " +
                "        MeterAsset.MeterID = Channel.MeterID AND " +
                "        MeterAsset.AssetID = Asset.ID CROSS APPLY " +
                "    ({JumpSQL}) Traverse(Jump) " +
                "WHERE Traverse.Jump <> 0";

            const string PassthroughChannelsQueryFormat =
                "SELECT Channel.* " +
                "FROM " +
                "    Channel JOIN " +
                "    Asset ParentAsset ON ParentAsset.ID = {{0}} JOIN " +
                "    Asset ChildAsset ON ParentAsset.ID = {{1}} CROSS APPLY " +
                "    ({PassthroughSQL}) Traverse(Passthrough) " +
                "WHERE " +
                "    Channel.ID IN ({DownstreamChannels}) " +
                "    Traverse.Passthrough <> 0";

            TableOperations<Channel> channelTable = new TableOperations<Channel>(connection);

            visitedAssets.Push(visitedAssetID);

            foreach (DataRow traversalRow in RetrieveRows(TraversalQueryFormat, assetID, visitedAssetID))
            {
                int connectedAssetID = traversalRow.ConvertField<int>("ConnectedAssetID");

                if (visitedAssets.Contains(connectedAssetID))
                    continue;

                int parentID = traversalRow.ConvertField<int>("ParentID");
                int childID = traversalRow.ConvertField<int>("ChildID");
                string jumpSQL = traversalRow.ConvertField<string>("JumpSQL");
                string passthroughSQL = traversalRow.ConvertField<string>("PassthroughSQL");

                // lang=regex
                const string Pattern = @"\{(?:parentid|childid|channelid)\}";
                jumpSQL = Regex.Replace(jumpSQL, Pattern, ReplaceFormatParameter, RegexOptions.IgnoreCase);
                passthroughSQL = Regex.Replace(passthroughSQL, Pattern, ReplaceFormatParameter, RegexOptions.IgnoreCase);

                string jumpChannelsQuery = JumpChannelsQueryFormat.Interpolate(new { JumpSQL = jumpSQL });

                foreach (DataRow channelRow in RetrieveRows(jumpChannelsQuery, assetID, connectedAssetID, parentID, childID))
                    yield return channelTable.LoadRecord(channelRow);

                IEnumerable<int> downstreamChannelIDs = TraverseConnectedChannels(connection, assetID, connectedAssetID, visitedAssets)
                    .Select(channel => channel.ID)
                    .OrderBy(id => id);

                string downstreamChannelList = string.Join(",", downstreamChannelIDs);

                if (downstreamChannelList.Length > 0)
                {
                    string passthroughChannelsQuery = PassthroughChannelsQueryFormat.Interpolate(new
                    {
                        PassthroughSQL = passthroughSQL,
                        DownstreamChannels = downstreamChannelList
                    });

                    foreach (DataRow channelRow in RetrieveRows(passthroughChannelsQuery, parentID, childID))
                        yield return channelTable.LoadRecord(channelRow);
                }
            }

            visitedAssets.Pop();

            string ReplaceFormatParameter(Match match)
            {
                switch (match.Value.ToLowerInvariant())
                {
                    case "{parentid}": return "ParentAsset.ID";
                    case "{childid}": return "ChildAsset.ID";
                    case "{channelid}": return "Channel.ID";
                    default: return match.Value;
                }
            }

            IEnumerable<DataRow> RetrieveRows(string query, params object[] args)
            {
                using (DataTable table = connection.RetrieveData(query, args))
                {
                    foreach (DataRow row in table.Rows)
                        yield return row;
                }
            }
        }

        // Logic to find distance between two Assets
        public int DistanceToAsset(int assetID)
        {
            int distance = 0;
            HashSet<int> visited = new HashSet<int>();
            List<Asset> next = new List<Asset>() { this };

            while (next.Count > 0)
            {
                if (next.Any(n => n.ID == assetID))
                    return distance;

                foreach (Asset n in next)
                    visited.Add(n.ID);

                next = next
                    .SelectMany(n => n.ConnectedAssets)
                    .DistinctBy(n => n.ID)
                    .Where(n => !visited.Contains(n.ID))
                    .ToList();

                distance++;
            }

            return -1;
        }

        public T QueryAs<T>(AdoDataConnection connection = null) where T : Asset, new()
        {
            if (this is T typedAsset)
                return typedAsset;

            if (connection is null && ConnectionFactory is null)
                throw new ArgumentNullException(nameof(connection));

            Lazy<AdoDataConnection> lazyConnection = new Lazy<AdoDataConnection>(ConnectionFactory);

            try
            {
                TableOperations<T> table = new TableOperations<T>(connection ?? lazyConnection.Value);
                return table.QueryRecordWhere("ID = {0}", ID);
            }
            finally
            {
                if (lazyConnection.IsValueCreated)
                    lazyConnection.Value.Dispose();
            }
        }

        [Obsolete("Replaced by GetChannels")]
        public IEnumerable<Channel> GetChannel(AdoDataConnection connection) => GetChannels(connection);

        [Obsolete("Replaced by GetConnections")]
        public IEnumerable<AssetConnection> GetConnection(AdoDataConnection connection) => GetConnections(connection);

        [Obsolete("Replaced by GetConnectedChannels")]
        public IEnumerable<Channel> GetConnectedChannel(AdoDataConnection connection) => GetConnectedChannels(connection);

        #endregion
    }
}
