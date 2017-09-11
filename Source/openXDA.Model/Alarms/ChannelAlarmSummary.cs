//******************************************************************************************************
//  MeterAlarmSummary.cs - Gbtc
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
//  06/20/2017 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using GSF.Data;
using GSF.Data.Model;

namespace openXDA.Model
{
    public class ChannelAlarmSummary
    {
        [PrimaryKey(true)]
        public int ID { get; set; }

        public int ChannelID { get; set; }

        public int AlarmTypeID { get; set; }

        public DateTime Date { get; set; } 

        public int AlarmPoints { get; set; }
    }

    public static partial class TableOperationsExtensions
    {
        public static void Upsert(this TableOperations<ChannelAlarmSummary> channelAlarmSummaryTable, ChannelAlarmSummary channelAlarmSummary)
        {
            const string UpsertQuery =
                "MERGE ChannelAlarmSummary WITH (HOLDLOCK) AS Target " +
                "USING (VALUES({0}, {1}, {2}, {3})) AS Source(ChannelID, AlarmTypeID, Date, AlarmPoints) " +
                "ON Target.ChannelID = Source.ChannelID AND Target.AlarmTypeID = Source.AlarmTypeID AND Target.Date = Source.Date " +
                "WHEN MATCHED THEN " +
                "    UPDATE SET " +
                "        Target.AlarmPoints = Source.AlarmPoints " +
                "WHEN NOT MATCHED THEN " +
                "    INSERT VALUES(Source.ChannelID, Source.AlarmTypeID, Source.Date, Source.AlarmPoints);";

            int channelID = channelAlarmSummary.ChannelID;
            int alarmTypeID = channelAlarmSummary.AlarmTypeID;
            DateTime date = channelAlarmSummary.Date;
            int alarmPoints = channelAlarmSummary.AlarmPoints;

            channelAlarmSummaryTable.Connection.ExecuteNonQuery(UpsertQuery, channelID, alarmTypeID, date, alarmPoints);
        }
    }
}
