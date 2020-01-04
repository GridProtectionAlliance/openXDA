//******************************************************************************************************
//  MeterLocationLine.cs - Gbtc
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
//  06/19/2017 - Billy Ernest
//       Generated original version of source code.
//  12/13/2019 - C. Lackner
//       Update to reflect changes in Location and move from Line to Asset.
//
//******************************************************************************************************

using System;
using GSF.Data;
using GSF.Data.Model;
using Newtonsoft.Json;

namespace openXDA.Model
{
    public class AssetLocation
    {
        #region [ Members ]

        // Fields
        private Location m_Location;
        private Asset m_asset;
        private SourceImpedance m_sourceImpedance;

        #endregion

        #region [ Properties ]

        [PrimaryKey(true)]
        public int ID { get; set; }

        public int LocationID { get; set; }

        public int AssetID { get; set; }

        [JsonIgnore]
        [NonRecordField]
        public Location Location
        {
            get
            {
                return m_Location ?? (m_Location = QueryLocation());
            }
            set
            {
                m_Location = value;
            }
        }

        [JsonIgnore]
        [NonRecordField]
        public Asset Asset
        {
            get
            {
                return m_asset ?? (m_asset ?? QueryAsset());
            }
            set
            {
                m_asset = value;
            }
        }

        [JsonIgnore]
        [NonRecordField]
        public SourceImpedance SourceImpedance
        {
            get
            {
                return m_sourceImpedance ?? (m_sourceImpedance ?? QuerySourceImpedance());
            }
            set
            {
                m_sourceImpedance = value;
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

        public Location GetLocation(AdoDataConnection connection)
        {
            if ((object)connection == null)
                return null;

            TableOperations<Location> locationTable = new TableOperations<Location>(connection);

            try
            {
                return locationTable.QueryRecordWhere("ID = {0}", LocationID);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public Asset GetAsset(AdoDataConnection connection)
        {
            if ((object)connection == null)
                return null;

            TableOperations<Asset> assetTable = new TableOperations<Asset>(connection);
            return assetTable.QueryRecordWhere("ID = {0}", AssetID);
        }

        public SourceImpedance GetSourceImpedance(AdoDataConnection connection)
        {
            if ((object)connection == null)
                return null;

            TableOperations<SourceImpedance> sourceImpedanceTable = new TableOperations<SourceImpedance>(connection);
            return sourceImpedanceTable.QueryRecordWhere("MeterLocationLineID = {0}", ID);
        }

        private Location QueryLocation()
        {
            Location location;

            using (AdoDataConnection connection = ConnectionFactory?.Invoke())
            {
                location = GetLocation(connection);
            }

            if ((object)location != null)
                location.LazyContext = LazyContext;

            return LazyContext.GetLocation(location);
        }

        private Asset QueryAsset()
        {
            Asset asset;

            using (AdoDataConnection connection = ConnectionFactory?.Invoke())
            {
                asset = GetAsset(connection);
            }

            if ((object)asset != null)
                asset.LazyContext = LazyContext;

            return LazyContext.GetAsset(asset);
        }

        private SourceImpedance QuerySourceImpedance()
        {
            SourceImpedance sourceImpedance;

            using (AdoDataConnection connection = ConnectionFactory?.Invoke())
            {
                sourceImpedance = GetSourceImpedance(connection);
            }

            if ((object)sourceImpedance != null)
                sourceImpedance.LazyContext = LazyContext;

            return LazyContext.GetSourceImpedance(sourceImpedance);
        }

        #endregion
    }
}
