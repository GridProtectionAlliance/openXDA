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


using GSF.Data;
using GSF.Data.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openXDA.Model
{
    public enum AssetType
    {
        Line = 1,
        Bus = 2,
        Breaker = 3,
        CapacitorBank = 4,
        LineSegement = 5,
        Transformer = 6
    }

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
        public string AssetKey { get; set; }

        public string Description { get; set; }

        public string AssetName { get; set; }

        [Required]
        public int AssetTypeID {get; set; }

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
                return m_connectedChannels ?? (m_connectedChannels = QueryChannels());
            }
            set
            {
                m_connectedChannels = value;
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
            if ((object)connection == null)
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

            if ((object)assetLocations != null)
            {
                foreach (AssetLocation assetLocation in assetLocations)
                {
                    assetLocation.Asset = this;
                    assetLocation.LazyContext = LazyContext;
                }
            }

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

            if ((object)meterAssets != null)
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
                channels = GetChannel(connection)?
                    .Select(LazyContext.GetChannel)
                    .ToList();
            }

            if ((object)channels != null)
            {
                foreach (Channel channel in channels)
                {
                    channel.Asset = this;
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
                connections = GetConnection(connection)?
                    .Select(LazyContext.GetAssetConnection)
                    .ToList();
            }

            if ((object)connections != null)
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
            if ((object)connection == null)
                return null;

            TableOperations<MeterAsset> meterAssetTable = new TableOperations<MeterAsset>(connection);
            return meterAssetTable.QueryRecordsWhere("AssetID = {0}", ID);
        }

        public IEnumerable<Channel> GetChannel(AdoDataConnection connection)
        {
            if ((object)connection == null)
                return null;

            TableOperations<Channel> channelTable = new TableOperations<Channel>(connection);
            return channelTable.QueryRecordsWhere("AssetID = {0}", ID);
        }

        public IEnumerable<AssetConnection> GetConnection(AdoDataConnection connection)
        {
            if ((object)connection == null)
                return null;

            TableOperations<AssetConnection> channelTable = new TableOperations<AssetConnection>(connection);
            return channelTable.QueryRecordsWhere("ParentID = {0} OR ChildID = {1}", ID, ID);
        }

        // Logic for Channels across Asset Connections
        public IEnumerable<Channel> GetConnectedChannel(AdoDataConnection connection, List<Asset> ignoredAssets = null, List<Location> alowedLocations = null)
        {
            ignoredAssets = ignoredAssets ?? new List<Asset>();
            alowedLocations = alowedLocations ?? QueryAssetLocations().Select(item => item.Location).ToList();


            List<Channel> result = new List<Channel>();

            ignoredAssets.Add(this);

            foreach (AssetConnection assetconnection in GetConnection(connection))
            {
                Asset remoteAsset = assetconnection.Child;
                if (assetconnection.ChildID == ID)
                    remoteAsset = assetconnection.Parent;

                if (ignoredAssets.Select(item => item.ID).Contains(remoteAsset.ID))
                    continue;

                bool locationAllowed = false;

                foreach (AssetLocation assetLocation in remoteAsset.AssetLocations)
                {
                    if (alowedLocations.Select(item =>item.ID).Contains(assetLocation.LocationID))
                    {
                        locationAllowed = true;
                        break;
                    }
                }
                if (locationAllowed == false)
                    continue;
                    
                IEnumerable<Channel> potentials = remoteAsset.GetChannel(connection);

                TableOperations<AssetConnectionType> assetConnectionTypeTbl = new TableOperations<AssetConnectionType>(connection);
                string jumpSQL = assetConnectionTypeTbl.QueryRecordWhere("ID = {0}",assetconnection.AssetRelationshipTypeID).JumpConnection;
                string passThrough = assetConnectionTypeTbl.QueryRecordWhere("ID = {0}", assetconnection.AssetRelationshipTypeID).PassThrough;

                foreach (Channel channel in potentials)
                {

                    if (Convert.ToBoolean(connection.ExecuteScalar(jumpSQL,channel.MeasurementTypeID)))
                    {
                        result.Add(channel);
                    }
                }

                potentials = remoteAsset.GetConnectedChannel(connection, ignoredAssets, alowedLocations);
                foreach (Channel channel in potentials)
                {

                    if (Convert.ToBoolean(connection.ExecuteScalar(passThrough, channel.MeasurementTypeID)))
                    {
                        result.Add(channel);
                    }
                }
            }

            return result;
        }

        #endregion
    }
}
