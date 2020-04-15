//******************************************************************************************************
//  Line.cs - Gbtc
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
//       Moved Base functionality to Asset.
//  04/15/2020 - Christoph Lackner
//       Added Transmission Path.
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
    public class Line: Asset
    {
        #region [ Members ]
       
        private List<LineSegment> m_lineSegment;

        #endregion

        #region [ Properties ]

        public double? MaxFaultDistance { get; set; }

        public double? MinFaultDistance { get; set; }

        [JsonIgnore]
        [NonRecordField]
        public List<LineSegment> Segments
        {
            get
            {
                return m_lineSegment ?? (m_lineSegment = QuerySegments());
            }
            set
            {
                m_lineSegment = value;
            }
        }

        [JsonIgnore]
        [NonRecordField]
        public List<TransmissionPath> Path
        {
            get
            {
                return new List<TransmissionPath>();
            }
            set
            {
                //m_lineSegment = value;
            }
        }

        #endregion

        #region [ Methods ]

        public IEnumerable<LineSegment> GetSegments(AdoDataConnection connection)
        {
            if ((object)connection == null)
                return null;

            List<LineSegment> result = new List<LineSegment>();

            foreach (AssetConnection assetConnection in Connections)
            {
                Asset remoteAsset = assetConnection.Child;
                if (assetConnection.ChildID == ID)
                    remoteAsset = assetConnection.Parent;

                if (remoteAsset.AssetTypeID == (int)AssetType.LineSegement)
                    result.Add(LineSegment.DetailedLineSegment(remoteAsset, connection));
            }

            return result.AsEnumerable();
        }

        private List<LineSegment> QuerySegments()
        {
            List<LineSegment> lineSegements;

            using (AdoDataConnection connection = ConnectionFactory?.Invoke())
            {
                lineSegements = GetSegments(connection)?
                    .Select(LazyContext.GetLineSegment)
                    .ToList();
            }

            if ((object)lineSegements != null)
            {
                foreach (LineSegment segment in lineSegements)
                {
                    segment.Line = this;
                    segment.LazyContext = LazyContext;
                }
            }

            return lineSegements;
        }


        public static Line DetailedLine(Asset asset, AdoDataConnection connection)
        {
           
            if ((object)connection == null)
                return null;

            TableOperations<Line> lineTable = new TableOperations<Line>(connection);
            Line line = lineTable.QueryRecordWhere("ID = {0}", asset.ID);

            if (line == null)
                return null;

            line.LazyContext = asset.LazyContext;
            line.ConnectionFactory = asset.ConnectionFactory;

            return line;
        }

        public static Line DetailedLine(Asset asset)
        {
            return DetailedLine(asset, asset.ConnectionFactory.Invoke());
        }
        #endregion
    }


    public class TransmissionPath
    {
        public double Length;
        public double X0;
        public double R0;
        public double X1;
        public double R1;

        public IEnumerable<LineSegment> Segments;
        public Line Line; 
    }

}
