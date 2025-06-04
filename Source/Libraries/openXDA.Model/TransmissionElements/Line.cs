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
using log4net;
using Newtonsoft.Json;

namespace openXDA.Model
{
    [MetadataType(typeof(Asset))]
    public class Line: Asset
    {
        #region [ Members ]
       
        private List<LineSegment> m_lineSegment;
        private List<TransmissionPath> m_path;

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
                return m_path ?? (m_path = QueryPath().ToList());
            }
            set
            {
                m_path = value;
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

        private List<TransmissionPath> QueryPath()
        {
            
            //Start by finding all the ends
            List<LineSegment> lineEnds = Segments.Where(item => item.IsEnd).ToList();

            if (lineEnds.Count == 1)
                return new List<TransmissionPath>()
                {
                    new TransmissionPath()
                    {
                        Length = lineEnds[0].Length,
                        X0 = lineEnds[0].X0,
                        R0 = lineEnds[0].R0,
                        X1 = lineEnds[0].X1,
                        R1 = lineEnds[0].R1,
                        Line = this,
                        Segments = lineEnds
                    }
                };

            if (lineEnds.Count == 0)
                return new List<TransmissionPath>()
                {
                    new TransmissionPath()
                    {
                        Length = 0,
                        X0 = 0,
                        R0 = 0,
                        X1 = 0,
                        R1 = 0,
                        Line = this,
                        Segments = new List<LineSegment>()
                    }
                };


            Dictionary<Tuple<int, int>, List<LineSegment>> result = new Dictionary<Tuple<int, int>, List<LineSegment>>();

            // start by creating all potential combinations

            for (int i = 1; i < lineEnds.Count; i++)
            {
                for (int j = (i-1); j > -1; j--)
                {
                    result.Add(new Tuple<int, int>(lineEnds[i].ID, lineEnds[j].ID), WalkTheLine(lineEnds[i], lineEnds[j])); 
                }
            }

            return result.Where(item => item.Value.Count > 0).Select( item => new TransmissionPath()
            {
                Length = item.Value.Select(seg => seg.Length).Sum(),
                X0 = item.Value.Select(seg => seg.X0).Sum(),
                R0 = item.Value.Select(seg => seg.R0).Sum(),
                X1 = item.Value.Select(seg => seg.X1).Sum(),
                R1 = item.Value.Select(seg => seg.R1).Sum(),
                Line = this,
                Segments = item.Value
            }).OrderByDescending(item => item.Length).ToList();
        }

        /// <summary>
        /// Attempts to walk the Line from start to end.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        private List<LineSegment> WalkTheLine(LineSegment start, LineSegment end, Stack<int> stack = null)
        {
            if (stack is null)
                stack = new Stack<int>();

            foreach (LineSegment segment in GetNextSegement(start))
            {
                if (segment.ID == end.ID)
                {
                    // we found the end, so we can return the path
                    return new List<LineSegment>() { start, segment };
                }
                if (stack.Contains(segment.ID))
                {
                    if (stack.Peek() != segment.ID)
                        Log.Error($"Line {this.AssetKey} has a looped linesegement ({segment.AssetKey}). This is causing issues in fault distance computations.");
                    continue;
                }
                stack.Push(start.ID);
                List<LineSegment> follow = WalkTheLine(segment, end, stack);
                stack.Pop();
                if (follow.Count > 0)
                {
                    follow.Insert(0, start);
                    return follow;
                }
            }

           return new List<LineSegment>();          
        }

        /// <summary>
        /// Gets a list of segments connected to the current segment.
        /// </summary>
        /// <param name="current"></param>
        /// <returns></returns>
        private IEnumerable<LineSegment> GetNextSegement(LineSegment current)
        {
            return current.connectedSegments.Select(item =>
            {
                if (item.ChildSegment == current.ID)
                    return item.Parent;
                else
                    return item.Child;
            });
        }

        public static Line DetailedLine(Asset asset, AdoDataConnection connection)
        {
            if (connection is null)
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
            using (AdoDataConnection connection = asset.ConnectionFactory())
            {
                return DetailedLine(asset, connection);
            }
        }
        #endregion

        private static readonly ILog Log = LogManager.GetLogger(typeof(Line));
    }


    public class TransmissionPath
    {
        public double Length;
        public double X0;
        public double R0;
        public double X1;
        public double R1;

        public List<LineSegment> Segments;
        public Line Line; 
    }

}
