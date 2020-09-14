//******************************************************************************************************
//  ReportedDisturbance.cs - Gbtc
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
//  12/06/2017 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using Gemstone;
using Gemstone.PQDIF.Logical;

namespace FaultData.DataAnalysis
{
    public class ReportedDisturbance
    {
        public ReportedDisturbance(Phase phase, DateTime time, double max, double min, double avg, TimeSpan duration, QuantityUnits units)
        {
            Phase = phase;
            Time = time;
            Maximum = max;
            Minimum = min;
            Average = avg;
            Duration = duration;
            Units = units;
        }

        public Phase Phase { get; }
        public DateTime Time { get; }
        public double Maximum { get; }
        public double Minimum { get; }
        public double Average { get; }
        public TimeSpan Duration { get; }
        public QuantityUnits Units { get; }

        public Range<DateTime> ToRange()
        {
            return new Range<DateTime>(Time, Time + Duration);
        }
    }
}
