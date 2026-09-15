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

        public IEnumerable<LineSegment> GetSegments(AdoDataConnection connection, List<Asset> remoteAssets)
        {
            if (connection is null)
                return null;

            List<LineSegment> result = [];

            foreach (Asset remoteAsset in remoteAssets)
            {
                if (remoteAsset.AssetTypeID == (int)AssetType.LineSegement)
                    result.Add(LineSegment.DetailedLineSegment(remoteAsset, connection));
            }

            return result.AsEnumerable();
        }

        private List<LineSegment> QuerySegments()
        {
            List<LineSegment> lineSegments;
            List<Asset> remoteAssets = RemoteAssets;

            using (AdoDataConnection connection = ConnectionFactory?.Invoke())
            {
                lineSegments = GetSegments(connection, remoteAssets)?
                    .Select(LazyContext.GetLineSegment)
                    .ToList();
            }

            if (lineSegments is not null)
            {
                foreach (LineSegment segment in lineSegments)
                {
                    segment.Line = this;
                    segment.LazyContext = LazyContext;
                }
            }

            return lineSegments;
        }

        private List<TransmissionPath> QueryPath()
        {
            // Start by finding all the ends
            static bool IsEnd(LineSegment segment) =>
                segment.IsEnd ||
                segment.ConnectedSegments.Count == 0;

            List<LineSegment> lineEnds = [.. Segments
                .OrderBy(segment => segment.ID)
                .Where(IsEnd)];

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

            List<TransmissionPath> result = [];

            // start by creating all potential combinations

            for (int i = 0; i < lineEnds.Count; i++)
            {
                for (int j = i + 1; j < lineEnds.Count; j++)
                {
                    LineSegment start = lineEnds[i];
                    LineSegment end = lineEnds[j];
                    List<LineSegment> path = WalkTheLine(start, end);
                    if (path.Count == 0) continue;

                    result.Add(new TransmissionPath()
                    {
                        Length = path.Select(seg => seg.Length).Sum(),
                        X0 = path.Select(seg => seg.X0).Sum(),
                        R0 = path.Select(seg => seg.R0).Sum(),
                        X1 = path.Select(seg => seg.X1).Sum(),
                        R1 = path.Select(seg => seg.R1).Sum(),
                        Line = this,
                        Segments = path
                    });
                }
            }

            result.Sort((p1, p2) =>
            {
                if (p1.Length != p2.Length)
                    return p2.Length.CompareTo(p1.Length);

                return p1.Segments[0].ID.CompareTo(p2.Segments[0].ID);
            });

            return result;
        }

        /// <summary>
        /// Attempts to walk the line from start to end.
        /// </summary>
        private List<LineSegment> WalkTheLine(LineSegment start, LineSegment end, Stack<int> stack = null)
        {
            stack ??= new Stack<int>([start.ID]);

            List<LineSegment> nextSegments = [.. GetNextSegments(start)
                .Where(next => !stack.Contains(next.ID))
                .OrderBy(next => next.ID)];

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

            Line line = asset.LazyContext.GetLine(asset.ID);

            if (line is not null)
                return line;

            TableOperations<Line> lineTable = new(connection);
            line = lineTable.QueryRecordWhere("ID = {0}", asset.ID);

            if (line is null)
                return null;

            line = asset.LazyContext.GetLine(line);
            line.LazyContext = asset.LazyContext;
            line.ConnectionFactory = asset.ConnectionFactory;

            return line;
        }

        public static Line DetailedLine(Asset asset)
        {
            using AdoDataConnection connection = asset.ConnectionFactory();
            return DetailedLine(asset, connection);
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
