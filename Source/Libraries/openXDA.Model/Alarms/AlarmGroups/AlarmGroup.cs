﻿//******************************************************************************************************
//  AlarmGroup.cs - Gbtc
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
//  10/23/2020 - Christoph Lackner
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.ComponentModel.DataAnnotations;
using GSF.Data.Model;

namespace openXDA.Model
{
    /// <summary>
    /// Represents a Group of Alarms set up in SPC Tools
    /// </summary>
    public class AlarmGroup
    {
        [PrimaryKey(true)]
        public int ID { get; set; }

        [StringLength(200)]
        public string Name { get; set; }

        public int AlarmTypeID { get; set; }

        public int SeverityID { get; set; }
    }

    /// <summary>
    /// Represents a detailed view of <see cref="AlarmGroup"/> inlcuding some summary Numbers
    /// </summary>
    [SettingsCategory("systemSettings"), AllowSearch, ViewOnly]
    public class AlarmGroupView 
    {
        [PrimaryKey(true)]
        public int ID { get; set; }
        public string Name { get; set; }
        public string AlarmSeverity { get; set; }
        public int Channels { get; set; }
        public int Meters { get; set; }
        public DateTime LastAlarmStart { get; set; }
        public DateTime LastAlarmEnd { get; set; }
        public string LastChannel { get; set; }
        public string LastMeter { get; set; }
        public string AlarmType { get; set; }
    }

}
