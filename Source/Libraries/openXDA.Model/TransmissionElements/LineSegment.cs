//******************************************************************************************************
//  LineSegment.cs - Gbtc
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
//  12/13/2019 - Christoph Lackner
//      Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using GSF.Data;
using GSF.Data.Model;
using Newtonsoft.Json;

namespace openXDA.Model
{
    [MetadataType(typeof(Asset))]
    public class LineSegment: Asset
    {
        #region [ Members ]

        // Fields
        private Line m_line;

        #endregion

        #region [ Properties ]

        public double R0 { get; set; }

        public double R1 { get; set; }

        public double X0 { get; set; }

        public double X1 { get; set; }

        public double ThermalRating { get; set; }

        public double Length { get; set; }

        [JsonIgnore]
        [NonRecordField]
        public Line Line
        {
            get
            {
                return m_line ?? (m_line = QueryLine());
            }
            set
            {
                m_line = value;
            }
        }

        #endregion

        #region [ Methods ]

        public static LineSegment DetailedLineSegment(Asset asset, AdoDataConnection connection)
        {
            if ((object)connection == null)
                return null;

            TableOperations<LineSegment> lineTable = new TableOperations<LineSegment>(connection);
            LineSegment line = lineTable.QueryRecordWhere("ID = {0}", asset.ID);
            line.LazyContext = asset.LazyContext;
            line.ConnectionFactory = asset.ConnectionFactory;

            return line;
        }

        public static LineSegment DetailedLineSegment(Asset asset)
        {
            return DetailedLineSegment(asset,asset.ConnectionFactory.Invoke());
        }

        public Line GetLine( AdoDataConnection connection)
        {
            if ((object)connection == null)
                return null;

            List<AssetConnection> connections = GetConnection(connection).ToList();

            int id = -1;

            foreach (AssetConnection assetConnection in connections)
            {
                Asset remoteAsset = assetConnection.Child;
                if (assetConnection.ChildID == ID)
                    remoteAsset = assetConnection.Parent;

                if (remoteAsset.AssetTypeID == (int)AssetType.Line)
                {
                    id = remoteAsset.ID;
                }
            }

            TableOperations<Line> lineTable = new TableOperations<Line>(connection);
            Line line = lineTable.QueryRecordWhere("AssetID = {0}", id);
            return line;
        }

        private Line QueryLine()
        {
            Line line;

            using (AdoDataConnection connection = ConnectionFactory?.Invoke())
            {
                line = LazyContext.GetLine(GetLine(connection));
            }

            if ((object)line != null)
            {
                line.LazyContext = LazyContext;
                
            }

            return line;
        }


        #endregion
    }
}
