//******************************************************************************************************
//  VoltageCurve.cs - Gbtc
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
//  09/07/2017 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using GSF.Data.Model;
using Newtonsoft.Json;

namespace openXDA.Model
{
    public class VoltageCurve
    {
        [PrimaryKey(true)]
        public int ID { get; set; }

        [StringLength(200)]
        public string Name { get; set; }

        [JsonIgnore]
        [NonRecordField]
        public List<VoltageCurvePoint> VoltageCurvePoints { get; set; }

        public double GetMagnitude(double duration)
        {
            var voltageSegments = VoltageCurvePoints
                .Zip(VoltageCurvePoints.Skip(1), (P1, P2) => new { P1, P2 })
                .ToList();

            int voltageSegmentIndex = voltageSegments
                .TakeWhile(segment => segment.P2.DurationSeconds <= duration)
                .Count();

            // The last data point in the curve extends to infinite duration
            if (voltageSegmentIndex == voltageSegments.Count)
                return voltageSegments.Last().P2.PerUnitMagnitude;

            var voltageSegment = voltageSegments[voltageSegmentIndex];

            // The first data point in the curve extends to negative inifinite magnitude
            if (duration < voltageSegment.P1.DurationSeconds)
                return double.NegativeInfinity;

            // Linear interpolation
            double y2 = voltageSegment.P2.PerUnitMagnitude;
            double y1 = voltageSegment.P1.PerUnitMagnitude;
            double x2 = voltageSegment.P2.DurationSeconds;
            double x1 = voltageSegment.P1.DurationSeconds;
            double x = duration;

            double m = (y2 - y1) / (x2 - x1);
            double y = m * (x - x1) + y1;

            return y;
        }
    }
}
