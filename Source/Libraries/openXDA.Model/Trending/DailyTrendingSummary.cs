//******************************************************************************************************
//  DailyTrendingSummary.cs - Gbtc
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
//  06/22/2017 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using GSF.Data.Model;

namespace openXDA.Model
{
    public class DailyTrendingSummary
    {
        [PrimaryKey(true)]
        public int ID { get; set; }

        public int ChannelID { get; set; }

        public DateTime Date { get; set; }

        public double Minimum { get; set; }

        public double Average { get; set; }

        public double Maximum { get; set; }

        public int ValidCount { get; set; }

        public int InvalidCount { get; set; }
    }

    public static partial class TableOperationsExtensions
    {
        public static void Upsert(this TableOperations<DailyTrendingSummary> dailyTrendingSummaryTable, DailyTrendingSummary dailyTrendingSummary)
        {
            const string UpsertQuery =
                "MERGE INTO DailyTrendingSummary WITH (TABLOCK) AS Target " +
                "USING (VALUES({0}, {1}, {2}, {3}, {4}, {5}, {6})) AS Source([ChannelID], [Date], [Minimum], [Maximum], [Average], [ValidCount], [InvalidCount]) " +
                "ON Source.ChannelID = Target.ChannelID AND Source.Date = Target.Date " +
                "WHEN MATCHED THEN " +
                "    UPDATE SET " +
                "        Maximum = CASE WHEN Target.ValidCount = 0 OR Source.Maximum > Target.Maximum THEN Source.Maximum ELSE Target.Maximum END, " +
                "        Minimum = CASE WHEN Target.ValidCount = 0 OR Source.Minimum < Target.Minimum THEN Source.Minimum ELSE Target.Minimum END, " +
                "        Average = CASE WHEN Target.ValidCount = 0 THEN Source.Average ELSE Target.Average * (CAST(Target.ValidCount AS FLOAT) / (Target.ValidCount + Source.ValidCount)) + Source.Average * (CAST(Source.ValidCount AS FLOAT) / (Target.ValidCount + Source.ValidCount)) END, " +
                "        ValidCount = Source.ValidCount + Target.ValidCount, " +
                "        InvalidCount = Source.InvalidCount + Target.InvalidCount " +
                "WHEN NOT MATCHED THEN " +
                "    INSERT (ChannelID, Date, Maximum, Minimum, Average, ValidCount, InvalidCount) " +
                "    VALUES (Source.ChannelID, Source.Date, Source.Maximum, Source.Minimum, Source.Average, Source.ValidCount, Source.InvalidCount);";

            int channelID = dailyTrendingSummary.ChannelID;
            DateTime date = dailyTrendingSummary.Date;
            double minimum = dailyTrendingSummary.Minimum;
            double maximum = dailyTrendingSummary.Maximum;
            double average = dailyTrendingSummary.Average;
            int validCount = dailyTrendingSummary.ValidCount;
            int invalidCount = dailyTrendingSummary.InvalidCount;

            dailyTrendingSummaryTable.Connection.ExecuteNonQuery(UpsertQuery, channelID, date, minimum, maximum, average, validCount, invalidCount);
        }
    }

}
