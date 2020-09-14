//******************************************************************************************************
//  LineSegmentConnections.cs - Gbtc
//
//  Copyright © 2020, Grid Protection Alliance.  All Rights Reserved.
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
//  04/14/2020 - C. Lackner
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.ComponentModel.DataAnnotations;
using Gemstone.Data;
using Gemstone.Data.Model;
using Newtonsoft.Json;

namespace OpenXDA.Model
{
    [TableName("LineSegmentConnections")]
    public class LineSegmentConnections
    {
        #region [ Members ]

        // Fields
        private LineSegment m_parent;
        private LineSegment m_child;

        #endregion

        #region [ Properties ]

        [PrimaryKey(true)]
        public int ID { get; set; }

        public int ParentSegment { get; set; }

        public int ChildSegment { get; set; }

        [JsonIgnore]
        [NonRecordField]
        public LineSegment Parent
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
        public LineSegment Child
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

        public LineSegment GetParent(AdoDataConnection connection)
        {
            if ((object)connection == null)
                return null;

            TableOperations<LineSegment> assetTable = new TableOperations<LineSegment>(connection);
            LineSegment parent = assetTable.QueryRecordWhere("ID = {0}", ParentSegment);
            return parent;
        }

        public LineSegment GetChild(AdoDataConnection connection)
        {
            if ((object)connection == null)
                return null;

            TableOperations<LineSegment> assetTable = new TableOperations<LineSegment>(connection);
            LineSegment child = assetTable.QueryRecordWhere("ID = {0}", ChildSegment);

            return child;
        }

        public LineSegment QueryParent()
        {
            LineSegment parent;

            using (AdoDataConnection connection = ConnectionFactory?.Invoke())
            {
                parent = GetParent(connection);
            }

            if ((object)parent != null)
                parent.LazyContext = LazyContext;

            return LazyContext.GetLineSegment(parent);
        }

        public LineSegment QueryChild()
        {
            LineSegment child;

            using (AdoDataConnection connection = ConnectionFactory?.Invoke())
            {
                child = GetChild(connection);
            }

            if ((object)child != null)
                child.LazyContext = LazyContext;

            return LazyContext.GetLineSegment(child);
        }

        #endregion
    }
}
