﻿//******************************************************************************************************
//  RemoteXDAInstance.cs - Gbtc
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
//  07/03/2017 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.ComponentModel.DataAnnotations;
using GSF.ComponentModel.DataAnnotations;
using GSF.Data.Model;

namespace openXDA.Model
{
    [AllowSearch]
    [GetRoles("Administrator")]
    [DeleteRoles("Administrator")]
    public class RemoteXDAInstance
    {
        private const string MinutePattern = "[1-5]?[0-9]";
        private const string HourPattern = "[1]?[0-9]|2[0-3]";
        private const string DayOfMonthPattern = "[1-9]|[12][0-9]|3[01]";
        private const string MonthPattern = "[1-9]|1[0-2]";
        private const string DayOfWeekPattern = "[0-6]";

        private const string CronPattern = "(^[*]$)|(^" +
            @"(?:\*|(?:\*/)?0*(?:" + MinutePattern + ")) " +
            @"(?:\*|(?:\*/)?0*(?:" + HourPattern + ")) " +
            @"(?:\*|(?:\*/)?0*(?:" + DayOfMonthPattern + ")) " +
            @"(?:\*|(?:\*/)?0*(?:" + MonthPattern + ")) " +
            @"(?:\*|(?:\*/)?0*(?:" + DayOfWeekPattern + "))" +
            "$)";

        [PrimaryKey(true)]
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        public string Address { get; set; }

        
        [RegularExpression(CronPattern, ErrorMessage = "Please see syntax help for correct cron syntax.")]
        [InitialValueScript("''")]
        public string Frequency { get; set; }

        public Guid UserAccountID { get; set; }
    }
}
