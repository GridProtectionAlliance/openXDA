//******************************************************************************************************
//  MeterLine.cs - Gbtc
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
//
//  12/13/2019 - C. Lackner
//       Modified to fit new Asset Model Structure.
//
//******************************************************************************************************

using System;
using System.ComponentModel.DataAnnotations;
using Gemstone.Data;
using Gemstone.Data.Model;
using Newtonsoft.Json;

namespace OpenXDA.Model
{
    public class MeterAsset
    {
        #region [ Members ]

        // Fields
        private Meter m_meter;
        private Asset m_asset;

        #endregion

        #region [ Properties ]

        [PrimaryKey(true)]
        public int ID { get; set; }

        public int MeterID { get; set; }

        public int AssetID { get; set; }

        [JsonIgnore]
        [NonRecordField]
        public Meter Meter
        {
            get
            {
                return m_meter ?? (m_meter = QueryMeter());
            }
            set
            {
                m_meter = value;
            }
        }

        [JsonIgnore]
        [NonRecordField]
        public Asset Asset
        {
            get
            {
                return m_asset ?? (m_asset = QueryAsset());
            }
            set
            {
                m_asset = value;
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

        public Meter GetMeter(AdoDataConnection connection)
        {
            if ((object)connection == null)
                return null;

            TableOperations<Meter> meterTable = new TableOperations<Meter>(connection);
            return meterTable.QueryRecordWhere("ID = {0}", MeterID);
        }

        public Asset GetAsset(AdoDataConnection connection)
        {
            if ((object)connection == null)
                return null;

            TableOperations<Asset> assetTable = new TableOperations<Asset>(connection);
            return assetTable.QueryRecordWhere("ID = {0}", AssetID);
        }

        public Meter QueryMeter()
        {
            Meter meter;

            using (AdoDataConnection connection = ConnectionFactory?.Invoke())
            {
                meter = GetMeter(connection);
            }

            if ((object)meter != null)
                meter.LazyContext = LazyContext;

            return LazyContext.GetMeter(meter);
        }

        public Asset QueryAsset()
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

        #endregion
    }

    public class MeterAssetDetail
    {
        [PrimaryKey(true)]
        public int ID { get; set; }

        public int MeterID { get; set; }

        public int AssetID { get; set; }

        [Searchable]
        public string MeterKey { get; set; }

        [Searchable]
        public string AssetKey { get; set; }

        [Searchable]
        public string AssetName { get; set; }

        [Searchable]
        public string AssetType { get; set; }

        public string FaultDetectionLogic { get; set; }
    }
}
