//******************************************************************************************************
//  TrendingData.cs - Gbtc
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
//******************************************************************************************************

using System;
using System.Collections.Generic;

namespace openXDA.Model
{
    public class TrendingData
    {
        public int ChannelID { get; set; }
        public string SeriesType { get; set; }
        public DateTime Time { get; set; }
        public double Value { get; set; }
    }

    public class TrendingDataSet
    {
        public List<TrendingDataPoint> ChannelData;
        public List<TrendingAlarmLimit> AlarmLimits;
        public List<TrendingAlarmLimit> OffNormalLimits;

        public TrendingDataSet()
        {
            ChannelData = new List<TrendingDataPoint>();
            AlarmLimits = new List<TrendingAlarmLimit>();
            OffNormalLimits = new List<TrendingAlarmLimit>();
        }
    }

    public class TrendingDataPoint
    {
        public double Time;
        public double Maximum;
        public double Minimum;
        public double Average;
    }

    public class TrendingAlarmLimit
    {
        public double TimeStart;
        public double TimeEnd;
        public double? High;
        public double? Low;
    }
}
