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
            // Start by finding all the ends
            bool IsEnd(LineSegment segment) =>
                segment.IsEnd ||
                segment.ConnectedSegments.Count <= 1 ||
                IsForkedEnd(segment);

            List<LineSegment> lineEnds = [.. Segments.Where(IsEnd)];

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

            return result.Where(item => item.Value.Count > 0).Select(item => new TransmissionPath()
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
        /// Determines if all pairs of segments connected to the given segment are connected to each other.
        /// </summary>
        private bool IsForkedEnd(LineSegment segment)
        {
            bool AreDirectConnected(LineSegment s1, LineSegment s2) => s1.ConnectedSegments
                .Any(connection => connection.ParentSegment == s2.ID || connection.ChildSegment == s2.ID);

            List<LineSegment> nextSegments = [.. GetNextSegments(segment)];

            return nextSegments
                .SelectMany(_ => nextSegments, (S1, S2) => (S1, S2))
                .Where(tuple => tuple.S1.ID < tuple.S2.ID)
                .All(tuple => AreDirectConnected(tuple.S1, tuple.S2));
        }

        /// <summary>
        /// Attempts to walk the line from start to end.
        /// </summary>
        private List<LineSegment> WalkTheLine(LineSegment start, LineSegment end, Stack<int> stack = null)
        {
            stack ??= new Stack<int>([start.ID]);

            List<LineSegment> nextSegments = [.. GetNextSegments(start)
                .Where(next => !stack.Contains(next.ID))];

            // Don't treat taps as bridges; if start, F1, and F2 are all connected
            // to each other, do not allow the F1 path to visit F2 or vice-versa
            foreach (LineSegment next in nextSegments)
                stack.Push(next.ID);

            try
            {
                foreach (LineSegment next in nextSegments)
                {
                    if (next.ID == end.ID)
                        return [start, next];
                }

                foreach (LineSegment next in nextSegments)
                {
                    List<LineSegment> follow = WalkTheLine(next, end, stack);

                    if (follow.Count == 0)
                        continue;

                    follow.Insert(0, start);
                    return follow;
                }

                return [];
            }
            finally
            {
                for (int i = 0; i < nextSegments.Count; i++)
                    stack.Pop();
            }
        }

        /// <summary>
        /// Gets a list of segments connected to the current segment.
        /// </summary>
        /// <param name="current"></param>
        /// <returns></returns>
        private IEnumerable<LineSegment> GetNextSegments(LineSegment current)
        {
            return current.ConnectedSegments.Select(item =>
            {
                if (item.ChildSegment == current.ID)
                    return item.Parent;
                else
                    return item.Child;
            });
        }

        #endregion

        #region [ Static ]

        // Static Fields
        private static readonly ILog Log = LogManager.GetLogger(typeof(Line));

        // Static Methods
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
    }


    public class TransmissionPath
    {
        public double Length;
        public double X0;
        public double R0;
        public double X1;
        public double R1;

        public Line Line;
        public List<LineSegment> Segments;
        public LineSegment Start => Segments?.FirstOrDefault();
        public LineSegment End => Segments?.LastOrDefault();
    }
}
