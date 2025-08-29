//******************************************************************************************************
//  Location.cs - Gbtc
//
//  Copyright © 2017, Grid Protection Alliance.  All Rights Reserved.
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
//  08/29/2017 - Billy Ernest
//       Generated original version of source code.
//  12/13/2019 - Christoph Lackner
//       Updated MeterLocation to more Generic Location.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
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
    public class Location
    {
        #region [ Members ]

        // Nested Types
        private delegate void ChannelConnector(int assetID, IEnumerable<Channel> channels);
        private delegate IEnumerable<AssetConnectionDetail> AssetConnectionLookup(int assetID);

        private class AssetConnectionDetail
        {
            #region [ Constructors ]

            public AssetConnectionDetail(int parentID, int childID, string jumpSQL, string passthroughSQL)
            {
                ParentID = parentID;
                ChildID = childID;
                JumpSQL = jumpSQL;
                PassthroughSQL = passthroughSQL;
            }

            #endregion

            #region [ Properties ]

            public int ParentID { get; }
            public int ChildID { get; }
            private string JumpSQL { get; }
            private string PassthroughSQL { get; }

            private HashSet<Channel> JumpChannels { get; } = new HashSet<Channel>();
            private HashSet<Channel> PassthroughChannels { get; } = new HashSet<Channel>();
            private bool Populated { get; set; }

            #endregion

            #region [ Methods ]

            public void PopulateChannelSets(AdoDataConnection connection, int locationID, Func<int, Channel> channelLookup)
            {
                if (Populated)
                    return;

                // lang=regex
                const string Pattern = @"\{(?:parentid|childid|channelid)\}";
                string jumpSQL = Regex.Replace(JumpSQL, Pattern, ReplaceFormatParameter, RegexOptions.IgnoreCase);
                string passthroughSQL = Regex.Replace(PassthroughSQL, Pattern, ReplaceFormatParameter, RegexOptions.IgnoreCase);

                string queryFormat =
                    $"SELECT " +
                    $"    SourceChannel.ID ChannelID, " +
                    $"    Jump.Value Jump, " +
                    $"    Passthrough.Value Passthrough " +
                    $"FROM " +
                    $"    Asset ParentAsset JOIN " +
                    $"    Asset ChildAsset ON " +
                    $"        ParentAsset.ID = {{0}} AND " +
                    $"        ChildAsset.ID = {{1}} JOIN " +
                    $"    Location ON Location.ID = {{2}} JOIN " +
                    $"    Meter ON Meter.LocationID = Location.ID JOIN " +
                    $"    Channel SourceChannel ON SourceChannel.MeterID = Meter.ID CROSS APPLY " +
                    $"    ({jumpSQL}) Jump(Value) CROSS APPLY " +
                    $"    ({passthroughSQL}) Passthrough(Value) " +
                    $"WHERE " +
                    $"    Jump.Value <> 0 OR " +
                    $"    Passthrough.Value <> 0";

                using (DataTable table = connection.RetrieveData(queryFormat, ParentID, ChildID, locationID))
                {
                    foreach (DataRow row in table.AsEnumerable())
                    {
                        int channelID = row.ConvertField<int>("ChannelID");
                        bool jump = row.ConvertField<bool>("Jump");
                        bool passthrough = row.ConvertField<bool>("Passthrough");
                        Channel channel = channelLookup(channelID);
                        if (channel is null) continue;
                        if (jump) JumpChannels.Add(channel);
                        if (passthrough) PassthroughChannels.Add(channel);
                    }
                }

                Populated = true;

                string ReplaceFormatParameter(Match match)
                {
                    switch (match.Value.ToLowerInvariant())
                    {
                        case "{parentid}": return "ParentAsset.ID";
                        case "{childid}": return "ChildAsset.ID";
                        case "{channelid}": return "SourceChannel.ID";
                        default: return match.Value;
                    }
                }
            }

            public bool CanJump(Channel channel) =>
                JumpChannels.Contains(channel);

            public bool CanPassThrough(Channel channel) =>
                PassthroughChannels.Contains(channel);

            #endregion
        }

        private class TraversalContext
        {
            public AdoDataConnection Connection { get; }
            public HashSet<int> VisitedAssets { get; }
            public AssetConnectionLookup FindAssetConnections { get; }
            public ChannelConnector ConnectChannels { get; }

            public TraversalContext(AdoDataConnection connection, HashSet<int> visitedAssets, AssetConnectionLookup findAssetConnections, ChannelConnector connectChannels)
            {
                Connection = connection;
                VisitedAssets = visitedAssets;
                FindAssetConnections = findAssetConnections;
                ConnectChannels = connectChannels;
            }
        }

        // Fields
        private List<Meter> m_meters;
        private List<AssetLocation> m_assetLocations;

        #endregion

        #region [ Properties ]

        [PrimaryKey(true)]
        public int ID { get; set; }

        [StringLength(50)]
        [Searchable]
        [Required]
        [DefaultSortOrder]
        public string LocationKey { get; set; }

        [StringLength(200)]
        [Searchable]
        [Required]
        public string Name { get; set; }

        [StringLength(200)]
        [Searchable]
        public string Alias { get; set; }

        [StringLength(50)]
        [Searchable]
        public string ShortName { get; set; }

        [Required]
        public double Latitude { get; set; }

        [Required]
        public double Longitude { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        [JsonIgnore]
        [NonRecordField]
        public List<Meter> Meters
        {
            get
            {
                return m_meters ?? (m_meters = QueryMeters());
            }
            set
            {
                m_meters = value;
            }
        }

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

        [JsonIgnore]
        [NonRecordField]
        internal LazyContext LazyContext { get; set; } = new LazyContext();

        #endregion

        #region [ Methods ]

        public IEnumerable<Meter> GetMeters(AdoDataConnection connection)
        {
            if ((object)connection == null)
                return null;

            TableOperations<Meter> meterTable = new TableOperations<Meter>(connection);
            return meterTable.QueryRecordsWhere("MeterLocationID = {0}", ID);
        }

        public IEnumerable<AssetLocation> GetAssetLocations(AdoDataConnection connection)
        {
            if ((object)connection == null)
                return null;

            TableOperations<AssetLocation> assetLocationTable = new TableOperations<AssetLocation>(connection);
            return assetLocationTable.QueryRecordsWhere("LocationID = {0}", ID);
        }

        private List<Meter> QueryMeters()
        {
            List<Meter> meters;

            using (AdoDataConnection connection = ConnectionFactory?.Invoke())
            {
                meters = GetMeters(connection)?
                    .Select(LazyContext.GetMeter)
                    .ToList();
            }

            if ((object)meters != null)
            {
                foreach (Meter meter in meters)
                {
                    meter.Location = this;
                    meter.LazyContext = LazyContext;
                }
            }

            return meters;
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

            if ((object)assetLocations != null)
            {
                foreach (AssetLocation assetLocation in assetLocations)
                {
                    assetLocation.Location = this;
                    assetLocation.LazyContext = LazyContext;
                }
            }

            return assetLocations;
        }

        public void ConnectAllChannels()
        {
            if (ConnectionFactory is null)
                return;

            Dictionary<int, HashSet<Channel>> connectedChannelLookup = new Dictionary<int, HashSet<Channel>>();
            ChannelConnector connectChannels = CreateChannelConnector(connectedChannelLookup);

            using (AdoDataConnection connection = ConnectionFactory())
            {
                TraverseAssetConnections(connection, connectChannels);

                TableOperations<Asset> assetTable = new TableOperations<Asset>(connection);

                foreach (KeyValuePair<int, HashSet<Channel>> kvp in connectedChannelLookup)
                {
                    int assetID = kvp.Key;
                    HashSet<Channel> connectedChannels = kvp.Value;
                    Asset asset = assetTable.QueryRecordWhere("ID = {0}", assetID);
                    asset = LazyContext.GetAsset(asset);
                    asset.ConnectedChannels = connectedChannels.ToList();
                    asset.LazyContext = LazyContext;
                }
            }

            // Assign empty lists to any assets that were missed by the recursive search
            foreach (Asset asset in AssetLocations.Select(al => al.Asset))
                EnsureConnectedChannels(asset);
        }

        private void TraverseAssetConnections(AdoDataConnection connection, ChannelConnector connectChannels)
        {
            List<Channel> allChannels = RetrieveAllChannels(connection).ToList();
            List<AssetConnectionDetail> allAssetConnections = RetrieveAllAssetConnections(connection).ToList();
            AssetConnectionLookup findAssetConnections = CreateAssetConnectionLookup(connection, allChannels, allAssetConnections);

            foreach (IGrouping<int, Channel> rootChannels in allChannels.GroupBy(channel => channel.AssetID))
            {
                int rootAssetID = rootChannels.Key;

                // Initialize an empty set of connected channels in case the root asset has no connected channels
                connectChannels(rootAssetID, Enumerable.Empty<Channel>());

                foreach (AssetConnectionDetail assetConnection in findAssetConnections(rootAssetID))
                {
                    List<Channel> jumpChannels = rootChannels
                        .Where(assetConnection.CanJump)
                        .ToList();

                    if (jumpChannels.Count == 0)
                        continue;

                    int connectedAssetID = assetConnection.ChildID;
                    connectChannels(connectedAssetID, jumpChannels);

                    HashSet<int> visitedAssets = new HashSet<int>() { rootAssetID };
                    TraversalContext context = new TraversalContext(connection, visitedAssets, findAssetConnections, connectChannels);
                    TraverseAssetConnections(context, jumpChannels, connectedAssetID);
                }
            }
        }

        private void TraverseAssetConnections(TraversalContext context, List<Channel> connectedChannels, int visitedAssetID)
        {
            AdoDataConnection connection = context.Connection;
            HashSet<int> visitedAssets = context.VisitedAssets;
            AssetConnectionLookup findAssetConnections = context.FindAssetConnections;
            ChannelConnector connectChannels = context.ConnectChannels;

            visitedAssets.Add(visitedAssetID);

            foreach (AssetConnectionDetail assetConnection in findAssetConnections(visitedAssetID))
            {
                int connectedAssetID = assetConnection.ChildID;

                if (visitedAssets.Contains(connectedAssetID))
                    continue;

                List<Channel> passthroughChannels = connectedChannels
                    .Where(assetConnection.CanPassThrough)
                    .ToList();

                if (passthroughChannels.Count == 0)
                    continue;

                connectChannels(connectedAssetID, passthroughChannels);
                TraverseAssetConnections(context, passthroughChannels, connectedAssetID);
            }

            visitedAssets.Remove(visitedAssetID);
        }

        private IEnumerable<Channel> RetrieveAllChannels(AdoDataConnection connection)
        {
            const string QueryFormat =
                "SELECT Channel.* " +
                "FROM " +
                "    Channel JOIN " +
                "    Meter ON Channel.MeterID = Meter.ID " +
                "WHERE Meter.LocationID = {0}";

            TableOperations<Channel> channelTable = new TableOperations<Channel>(connection);

            using (DataTable table = connection.RetrieveData(QueryFormat, ID))
            {
                foreach (DataRow row in table.AsEnumerable())
                {
                    Channel channel = channelTable.LoadRecord(row);
                    channel = LazyContext.GetChannel(channel);
                    yield return channel;
                }
            }
        }

        private IEnumerable<AssetConnectionDetail> RetrieveAllAssetConnections(AdoDataConnection connection)
        {
            const string QueryFormat =
                "SELECT " +
                "    AssetConnection.ParentID, " +
                "    AssetConnection.ChildID, " +
                "    AssetRelationshipType.JumpConnection JumpSQL, " +
                "    AssetRelationshipType.PassThrough PassthroughSQL " +
                "FROM " +
                "    Location JOIN " +
                "    AssetConnection ON Location.ID = {0} JOIN " +
                "    AssetRelationshipType ON AssetConnection.AssetRelationshipTypeID = AssetRelationshipType.ID JOIN " +
                "    AssetLocation ParentLocation ON " +
                "        ParentLocation.LocationID = Location.ID AND " +
                "        ParentLocation.AssetID = AssetConnection.ParentID JOIN " +
                "    AssetLocation ChildLocation ON " +
                "        ChildLocation.LocationID = Location.ID AND " +
                "        ChildLocation.AssetID = AssetConnection.ChildID";

            using (DataTable table = connection.RetrieveData(QueryFormat, ID))
            {
                foreach (DataRow row in table.AsEnumerable())
                {
                    int parentID = row.ConvertField<int>("ParentID");
                    int childID = row.ConvertField<int>("ChildID");
                    string jumpSQL = row.ConvertField<string>("JumpSQL");
                    string passthroughSQL = row.ConvertField<string>("PassthroughSQL");
                    yield return new AssetConnectionDetail(parentID, childID, jumpSQL, passthroughSQL);
                    yield return new AssetConnectionDetail(childID, parentID, jumpSQL, passthroughSQL);
                }
            }
        }

        private AssetConnectionLookup CreateAssetConnectionLookup(AdoDataConnection connection, List<Channel> allChannels, List<AssetConnectionDetail> allAssetConnections)
        {
            Dictionary<int, Channel> channelLookup = allChannels.ToDictionary(channel => channel.ID);
            ILookup<int, AssetConnectionDetail> assetConnectionLookup = allAssetConnections.ToLookup(conn => conn.ParentID);
            return findAssetConnection;

            Channel findChannel(int channelID) =>
                channelLookup.TryGetValue(channelID, out Channel channel)
                    ? channel
                    : null;

            IEnumerable<AssetConnectionDetail> findAssetConnection(int assetID)
            {
                foreach (AssetConnectionDetail assetConnection in assetConnectionLookup[assetID])
                {
                    assetConnection.PopulateChannelSets(connection, ID, findChannel);
                    yield return assetConnection;
                }
            }
        }

        #endregion

        #region [ Static ]

        // Static Methods
        private static ChannelConnector CreateChannelConnector(Dictionary<int, HashSet<Channel>> connectedChannelLookup)
        {
            return (assetID, channels) =>
            {
                HashSet<Channel> connectedChannels = connectedChannelLookup.GetOrAdd(assetID, _ => new HashSet<Channel>());
                connectedChannels.UnionWith(channels);
            };
        }

        private static void EnsureConnectedChannels(Asset asset)
        {
            Func<AdoDataConnection> connectionFactory = asset.ConnectionFactory;

            try
            {
                asset.ConnectionFactory = null;

                if (asset.ConnectedChannels is null)
                    asset.ConnectedChannels = new List<Channel>();
            }
            finally
            {
                asset.ConnectionFactory = connectionFactory;
            }
        }

        #endregion
    }
}
