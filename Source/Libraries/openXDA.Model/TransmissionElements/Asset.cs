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

        // Nested Types
        private delegate List<Channel> ConnectedChannelLookup(int parentID, int childID, string traversalSQL);

        private class TraversalContext
        {
            public AdoDataConnection Connection { get; }
            public int LocationID { get; }

            public ConnectedChannelLookup ConnectedChannelLookup { get; }
            public HashSet<int> VisitedAssets { get; }
            public HashSet<Channel> ConnectedChannels { get; }

            public TraversalContext(AdoDataConnection connection, int locationID)
            {
                Connection = connection;
                LocationID = locationID;

                ConnectedChannelLookup = GetConnectedChannelLookup(connection, locationID);
                VisitedAssets = new HashSet<int>();
                ConnectedChannels = new HashSet<Channel>();
            }
        }

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
            return AssetLocations
                .SelectMany(assetLocation => TraverseConnectedChannels(connection, assetLocation.LocationID, ID))
                .Distinct(new ChannelComparer());
        }

        private IEnumerable<Channel> TraverseConnectedChannels(AdoDataConnection connection, int locationID, int assetID)
        {
            TraversalContext context = new TraversalContext(connection, locationID);
            ConnectedChannelLookup channelLookup = context.ConnectedChannelLookup;
            context.VisitedAssets.Add(assetID);

            using (DataTable connectionTable = RetrieveConnectedAssets(connection, locationID, assetID))
            {
                foreach (DataRow traversalRow in connectionTable.AsEnumerable())
                {
                    int connectedAssetID = traversalRow.ConvertField<int>("ConnectedAssetID");
                    string jumpSQL = traversalRow.ConvertField<string>("JumpSQL");

                    IEnumerable<Channel> jumpChannels = channelLookup(connectedAssetID, assetID, jumpSQL)
                        .Where(channel => channel.AssetID == connectedAssetID);

                    context.ConnectedChannels.UnionWith(jumpChannels);
                }

                foreach (DataRow traversalRow in connectionTable.AsEnumerable())
                {
                    int connectedAssetID = traversalRow.ConvertField<int>("ConnectedAssetID");
                    string passthroughSQL = traversalRow.ConvertField<string>("PassthroughSQL");

                    List<Channel> pathChannels = channelLookup(connectedAssetID, assetID, passthroughSQL)
                        .Where(channel => channel.AssetID != assetID)
                        .Except(context.ConnectedChannels)
                        .ToList();

                    if (pathChannels.Count == 0)
                        continue;

                    TraverseConnectedChannels(context, pathChannels, connectedAssetID);
                }
            }

            return context.ConnectedChannels;
        }

        private void TraverseConnectedChannels(TraversalContext context, List<Channel> pathChannels, int visitedAssetID)
        {
            AdoDataConnection connection = context.Connection;
            ConnectedChannelLookup channelLookup = context.ConnectedChannelLookup;
            int locationID = context.LocationID;

            context.VisitedAssets.Add(visitedAssetID);

            using (DataTable connectionTable = RetrieveConnectedAssets(connection, locationID, visitedAssetID))
            {
                foreach (DataRow traversalRow in connectionTable.AsEnumerable())
                {
                    int connectedAssetID = traversalRow.ConvertField<int>("ConnectedAssetID");

                    if (context.VisitedAssets.Contains(connectedAssetID))
                        continue;

                    string jumpSQL = traversalRow.ConvertField<string>("JumpSQL");

                    IEnumerable<Channel> jumpChannels = channelLookup(connectedAssetID, visitedAssetID, jumpSQL)
                        .Where(channel => channel.AssetID == connectedAssetID)
                        .Intersect(pathChannels);

                    context.ConnectedChannels.UnionWith(jumpChannels);
                }

                HashSet<Channel> filteredPathChannels = new HashSet<Channel>(pathChannels);
                filteredPathChannels.ExceptWith(context.ConnectedChannels);

                foreach (DataRow traversalRow in connectionTable.AsEnumerable())
                {
                    if (filteredPathChannels.Count == 0)
                        break;

                    int connectedAssetID = traversalRow.ConvertField<int>("ConnectedAssetID");

                    if (context.VisitedAssets.Contains(connectedAssetID))
                        continue;

                    string passthroughSQL = traversalRow.ConvertField<string>("PassthroughSQL");

                    List<Channel> passthroughChannels = channelLookup(connectedAssetID, visitedAssetID, passthroughSQL)
                        .Intersect(filteredPathChannels)
                        .ToList();

                    if (passthroughChannels.Count == 0)
                        continue;

                    int connectedCount = context.ConnectedChannels.Count;
                    TraverseConnectedChannels(context, passthroughChannels, connectedAssetID);

                    if (context.ConnectedChannels.Count != connectedCount)
                        filteredPathChannels.ExceptWith(context.ConnectedChannels);
                }
            }

            context.VisitedAssets.Remove(visitedAssetID);
        }

        private DataTable RetrieveConnectedAssets(AdoDataConnection connection, int locationID, int visitedAssetID)
        {
            const string TraversalQueryFormat =
                "SELECT DISTINCT " +
                "    ConnectedAsset.ID ConnectedAssetID, " +
                "    AssetRelationshipType.JumpConnection JumpSQL, " +
                "    AssetRelationshipType.PassThrough PassthroughSQL " +
                "FROM " +
                "    Location JOIN " +
                "    Asset VisitedAsset ON " +
                "        Location.ID = {0} AND " +
                "        VisitedAsset.ID = {1} JOIN " +
                "    AssetConnection ON VisitedAsset.ID IN (AssetConnection.ParentID, AssetConnection.ChildID) JOIN " +
                "    Asset ConnectedAsset ON " +
                "        ConnectedAsset.ID IN (AssetConnection.ParentID, AssetConnection.ChildID) AND " +
                "        ConnectedAsset.ID <> VisitedAsset.ID JOIN " +
                "    AssetRelationshipType ON AssetConnection.AssetRelationshipTypeID = AssetRelationshipType.ID JOIN " +
                "    AssetLocation ON " +
                "        AssetLocation.AssetID = ConnectedAsset.ID AND " +
                "        AssetLocation.LocationID = Location.ID";

            return connection.RetrieveData(TraversalQueryFormat, locationID, visitedAssetID);
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

        #region [ Static ]

        // Static Methods
        private static ConnectedChannelLookup GetConnectedChannelLookup(AdoDataConnection connection, int locationID)
        {
            const string ChannelQueryFormat =
                "SELECT Channel.* " +
                "FROM " +
                "    Channel JOIN " +
                "    Meter ON " +
                "        Channel.MeterID = Meter.ID AND " +
                "        Meter.LocationID = {{0}} JOIN " +
                "    Asset ParentAsset ON ParentAsset.ID = {{1}} JOIN " +
                "    Asset ChildAsset ON ChildAsset.ID = {{2}} CROSS APPLY " +
                "    ({TraversalSQL}) Traversal(Traverse) " +
                "WHERE Traversal.Traverse <> 0";

            Dictionary<int, Channel> channelLookup = new Dictionary<int, Channel>();

            var connectedChannelLookup = Enumerable
                .Empty<List<Channel>>()
                .ToDictionary(_ => new { ParentID = 0, ChildID = 0, TraversalSQL = "" });

            return (parentID, childID, traversalSQL) =>
            {
                var key = new { ParentID = parentID, ChildID = childID, TraversalSQL = traversalSQL };
                return connectedChannelLookup.GetOrAdd(key, _ => RetrieveConnectedChannels(parentID, childID, traversalSQL));
            };

            List<Channel> RetrieveConnectedChannels(int parentID, int childID, string traversalSQL)
            {
                TableOperations<Channel> channelTable = new TableOperations<Channel>(connection);

                // lang=regex
                const string Pattern = @"\{(?:parentid|childid|channelid)\}";
                string replacedSQL = Regex.Replace(traversalSQL, Pattern, ReplaceFormatParameter, RegexOptions.IgnoreCase);

                string channelQuery = ChannelQueryFormat
                    .Interpolate(new { TraversalSQL = replacedSQL });

                return RetrieveRows(channelQuery, locationID, parentID, childID)
                    .Select(channelTable.LoadRecord)
                    .Select(LookUpChannel)
                    .ToList();
            }

            Channel LookUpChannel(Channel channel) =>
                channelLookup.GetOrAdd(channel.ID, _ => channel);

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

        #endregion
    }
}
