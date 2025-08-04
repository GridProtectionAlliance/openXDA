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

        private class TraversalContext
        {
            public AdoDataConnection Connection { get; }
            public ChannelConnector ConnectChannels { get; }
            public HashSet<int> VisitedAssets { get; }
            public int RootAssetID { get; }

            public TraversalContext(AdoDataConnection connection, ChannelConnector connectChannels, HashSet<int> visitedAssets, int rootAssetID)
            {
                Connection = connection;
                ConnectChannels = connectChannels;
                VisitedAssets = visitedAssets;
                RootAssetID = rootAssetID;
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
                }
            }
        }

        private void TraverseAssetConnections(AdoDataConnection connection, ChannelConnector connectChannels)
        {
            IEnumerable<IGrouping<int, Channel>> groupings = RetrieveAllChannels(connection)
                .GroupBy(channel => channel.AssetID);

            foreach (IGrouping<int, Channel> grouping in groupings)
            {
                int rootAssetID = grouping.Key;

                // Initialize an empty set of connected channels in case the root asset has no connected channels
                connectChannels(rootAssetID, Enumerable.Empty<Channel>());

                foreach (DataRow row in RetrieveJumpConnections(connection, rootAssetID))
                {
                    int connectedAssetID = row.ConvertField<int>("ConnectedAssetID");
                    string jumpSQL = row.ConvertField<string>("ChannelFilter");

                    List<Channel> jumpChannels = FilterChannels(connection, rootAssetID, rootAssetID, connectedAssetID, jumpSQL)
                        .ToList();

                    if (jumpChannels.Count == 0)
                        continue;

                    connectChannels(connectedAssetID, jumpChannels);

                    HashSet<int> visitedAssets = new HashSet<int>() { rootAssetID };
                    TraversalContext context = new TraversalContext(connection, connectChannels, visitedAssets, rootAssetID);
                    TraverseAssetConnections(context, jumpChannels, connectedAssetID);
                }
            }
        }

        private void TraverseAssetConnections(TraversalContext context, List<Channel> connectedChannels, int visitedAssetID)
        {
            AdoDataConnection connection = context.Connection;
            ChannelConnector connectChannels = context.ConnectChannels;
            HashSet<int> visitedAssets = context.VisitedAssets;
            int rootAssetID = context.RootAssetID;

            visitedAssets.Add(visitedAssetID);

            foreach (DataRow row in RetrievePassthroughConnections(connection, visitedAssetID))
            {
                int connectedAssetID = row.ConvertField<int>("ConnectedAssetID");

                if (visitedAssets.Contains(connectedAssetID))
                    continue;

                string passthroughSQL = row.ConvertField<string>("ChannelFilter");

                List<Channel> passthroughChannels = FilterChannels(connection, rootAssetID, visitedAssetID, connectedAssetID, passthroughSQL)
                    .Intersect(connectedChannels)
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
                    yield return channelTable.LoadRecord(row);
            }
        }

        private IEnumerable<Channel> FilterChannels(AdoDataConnection connection, int rootAssetID, int parentAssetID, int childAssetID, string filterSQL)
        {
            // lang=regex
            const string Pattern = @"\{(?:parentid|childid|channelid)\}";
            string replacedSQL = Regex.Replace(filterSQL, Pattern, ReplaceFormatParameter, RegexOptions.IgnoreCase);

            string queryFormat =
                $"SELECT SourceChannel.* " +
                $"FROM " +
                $"    Asset ParentAsset JOIN " +
                $"    Asset ChildAsset ON " +
                $"        ParentAsset.ID = {{0}} AND " +
                $"        ChildAsset.ID = {{1}} JOIN " +
                $"    Channel SourceChannel ON SourceChannel.AssetID = {{2}} CROSS APPLY " +
                $"    ({replacedSQL}) Traversal(Traverse) " +
                $"WHERE Traversal.Traverse <> 0";

            TableOperations<Channel> channelTable = new TableOperations<Channel>(connection);

            using (DataTable table = connection.RetrieveData(queryFormat, parentAssetID, childAssetID, rootAssetID))
            {
                foreach (DataRow row in table.AsEnumerable())
                {
                    Channel channel = channelTable.LoadRecord(row);
                    yield return LazyContext.GetChannel(channel);
                }
            }

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

        #endregion

        #region [ Static ]

        // Static Methods
        private static IEnumerable<DataRow> RetrieveJumpConnections(AdoDataConnection connection, int assetID)
        {
            return RetrieveAssetConnections(connection, assetID, "JumpConnection");
        }

        private static IEnumerable<DataRow> RetrievePassthroughConnections(AdoDataConnection connection, int assetID)
        {
            return RetrieveAssetConnections(connection, assetID, "PassThrough");
        }

        private static IEnumerable<DataRow> RetrieveAssetConnections(AdoDataConnection connection, int assetID, string filterField)
        {
            string queryFormat =
                $"SELECT " +
                $"    ConnectedAsset.ID ConnectedAssetID, " +
                $"    AssetRelationshipType.{filterField} ChannelFilter " +
                $"FROM " +
                $"    Asset JOIN " +
                $"    AssetConnection ON " +
                $"        Asset.ID = {{0}} AND " +
                $"        Asset.ID IN (AssetConnection.ParentID, AssetConnection.ChildID) JOIN " +
                $"    Asset ConnectedAsset ON " +
                $"        ConnectedAsset.ID IN (AssetConnection.ParentID, AssetConnection.ChildID) AND " +
                $"        ConnectedAsset.ID <> Asset.ID JOIN " +
                $"    AssetRelationshipType ON AssetConnection.AssetRelationshipTypeID = AssetRelationshipType.ID";

            using (DataTable table = connection.RetrieveData(queryFormat, assetID))
            {
                foreach (DataRow row in table.AsEnumerable())
                    yield return row;
            }
        }

        private static ChannelConnector CreateChannelConnector(Dictionary<int, HashSet<Channel>> connectedChannelLookup)
        {
            return (int assetID, IEnumerable<Channel> channels) =>
            {
                HashSet<Channel> connectedChannels = connectedChannelLookup.GetOrAdd(assetID, _ => new HashSet<Channel>());
                connectedChannels.UnionWith(channels);
            };
        }

        #endregion
    }
}
