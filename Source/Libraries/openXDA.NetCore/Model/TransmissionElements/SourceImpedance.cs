//******************************************************************************************************
//  SourceImpedance.cs - Gbtc
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
//
//******************************************************************************************************

using System;
using Gemstone.Data;
using Gemstone.Data.Model;
using Newtonsoft.Json;

namespace OpenXDA.Model
{
    public class SourceImpedance
    {
        #region [ Members ]

        // Fields
        private AssetLocation m_assetLocation;

        #endregion

        #region [ Properties ]

        [PrimaryKey(true)]
        public int ID { get; set; }

        public int AssetLocationID { get; set; }

        public double RSrc { get; set; }

        public double XSrc { get; set; }

        [JsonIgnore]
        [NonRecordField]
        public AssetLocation AssetLocation
        {
            get
            {
                return m_assetLocation ?? (m_assetLocation = QueryAssetLocation());
            }
            set
            {
                m_assetLocation = value;
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

        public AssetLocation GetAssetLocation(AdoDataConnection connection)
        {
            if ((object)connection == null)
                return null;

            TableOperations<AssetLocation> assetLocationTable = new TableOperations<AssetLocation>(connection);
            return assetLocationTable.QueryRecordWhere("ID = {0}", AssetLocationID);
        }

        private AssetLocation QueryAssetLocation()
        {
            AssetLocation assetLocation;

            using (AdoDataConnection connection = ConnectionFactory?.Invoke())
            {
                assetLocation = GetAssetLocation(connection);
            }

            if ((object)assetLocation != null)
                assetLocation.LazyContext = LazyContext;

            return LazyContext.GetAssetLocation(assetLocation);
        }

        #endregion
    }
}
