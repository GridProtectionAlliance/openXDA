//******************************************************************************************************
//  AssetConnection.cs - Gbtc
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
//  12/13/2019 - C. Lackner
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.ComponentModel.DataAnnotations;
using GSF.Data;
using GSF.Data.Model;
using Newtonsoft.Json;

namespace openXDA.Model
{
    [TableName("AssetRelationship")]
    public class AssetConnection
    {
        #region [ Members ]

        // Fields
        private Asset m_parent;
        private Asset m_child;

        #endregion

        #region [ Properties ]

        [PrimaryKey(true)]
        public int ID { get; set; }

        public int AssetRelationshipTypeID { get; set; }

        public int ParentID { get; set; }

        public int ChildID { get; set; }

        [JsonIgnore]
        [NonRecordField]
        public Asset Parent
        {
            get
            {
                return m_parent ?? (m_parent = QueryParent());
            }
            set
            {
                m_parent = value;
            }
        }

        [JsonIgnore]
        [NonRecordField]
        public Asset Child
        {
            get
            {
                return m_child ?? (m_child = QueryChild());
            }
            set
            {
                m_child = value;
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

        public Asset GetParent(AdoDataConnection connection)
        {
            if ((object)connection == null)
                return null;

            TableOperations<Asset> assetTable = new TableOperations<Asset>(connection);
            Asset parent = assetTable.QueryRecordWhere("ID = {0}", ParentID);
            return parent;
        }

        public Asset GetChild(AdoDataConnection connection)
        {
            if ((object)connection == null)
                return null;

            TableOperations<Asset> assetTable = new TableOperations<Asset>(connection);
            Asset child = assetTable.QueryRecordWhere("ID = {0}", ChildID);

            return child;
        }

        public Asset QueryParent()
        {
            Asset parent;

            using (AdoDataConnection connection = ConnectionFactory?.Invoke())
            {
                parent = GetParent(connection);
            }

            if ((object)parent != null)
                parent.LazyContext = LazyContext;

            return LazyContext.GetAsset(parent);
        }

        public Asset QueryChild()
        {
            Asset child;

            using (AdoDataConnection connection = ConnectionFactory?.Invoke())
            {
                child = GetChild(connection);
            }

            if ((object)child != null)
                child.LazyContext = LazyContext;

            return LazyContext.GetAsset(child);
        }

        #endregion
    }

    public class AssetConnectionDetail
    {
        [PrimaryKey(true)]
        public int ID { get; set; }

        public int AssetRelationshipTypeID { get; set; }

        public int ParentID { get; set; }

        public int ChildID { get; set; }

        public string ChildKey { get; set; }

        public string ParentKey { get; set; }

        public string AssetRelationshipType { get; set; }
    }
}
