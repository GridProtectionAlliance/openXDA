﻿//******************************************************************************************************
//  ActiveAlarm.cs - Gbtc
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
//  12/01/2020 - Christoph Lackner
//       Generated original version of source code.
//
//******************************************************************************************************

using GSF.Data.Model;

namespace openXDA.Model
{
    /// <summary>
    /// Represents an Alarm with Threshhold to trigger based on trendData
    /// </summary>
    [TableName("ActiveAlarmView")]
    public class ActiveAlarm
    {
        public int AlarmID { get; set; }
        public int AlarmGroupID { get; set; }
        public int AlarmTypeID { get; set; }
        public int? AlarmFactorID { get; set; }
        public int SeriesID { get; set; }
        public double Value { get; set; }
    }
}
