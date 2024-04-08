//******************************************************************************************************
//  FTTRecord.cs - Gbtc
//
//  Copyright © 2022, Grid Protection Alliance.  All Rights Reserved.
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
//  07/08/2022 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Data;
using GSF.Data;
using GSF.Data.Model;

namespace openXDA.NotificationDataSources.FaultTraceTool
{
    public class FTTRecord
    {
        public int FaultNumber { get; set; }
        public string StationName { get; set; }
        public string LineKey { get; set; }
        public double Distance { get; set; }

        [FieldDataType(DbType.DateTime2, DatabaseType.SQLServer)]
        public DateTime EventTime { get; set; }

        public double? EndLatitude { get; set; }
        public double? EndLongitude { get; set; }
    }
}
