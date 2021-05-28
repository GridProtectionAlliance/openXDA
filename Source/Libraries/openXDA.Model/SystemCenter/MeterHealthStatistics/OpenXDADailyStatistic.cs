//******************************************************************************************************
//  OpenXDADailyStatistic.cs - Gbtc
//
//  Copyright © 2021, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the MIT License (MIT), the "License"; you may not use this
//  file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://opensource.org/licenses/MIT
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  05/21/2021 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************

using GSF.Data.Model;
using System;

namespace SystemCenter.Model
{
    [TableName("SystemCenter.OpenXDADailyStatistic"), UseEscapedName]
    public class OpenXDADailyStatistic
    {
        [PrimaryKey(true)]
        public int ID { get; set; }

        [UseEscapedName]
        public string Date { get; set; }

        public string Meter { get; set; }
        public DateTime LastSuccessfulFileProcessed { get; set; }
        public DateTime LastUnsuccessfulFileProcessed { get; set; }

        public string LastUnsuccessfulFileProcessedExplanation { get; set; }
        public int TotalFilesProcessed { get; set; }
        public int TotalUnsuccessfulFilesProcessed { get; set; }
        public int TotalSuccessfulFilesProcessed { get; set; }
        public int TotalEmailsSent { get; set; }

        public double AverageDownloadLatency { get; set; }
        public double AverageProcessingStartLatency { get; set; }
        public double AverageProcessingEndLatency { get; set; }
        public double AverageEmailLatency { get; set; }
        public double AverageTotalProcessingLatency { get; set; }
        public double AverageTotalEmailLatency { get; set; }
    }
}