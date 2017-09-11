//******************************************************************************************************
//  MeterDataQualitySummary.cs - Gbtc
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
using GSF.Data.Model;

namespace openXDA.Model
{
    public class MeterDataQualitySummary
    {
        [PrimaryKey(true)]
        public int ID { get; set; }

        public int MeterID { get; set; }

        public DateTime Date { get; set; }

        public int ExpectedPoints { get; set; }

        public int GoodPoints { get; set; }

        public int LatchedPoints { get; set; }

        public int UnreasonablePoints { get; set; }

        public int NoncongruentPoints { get; set; }

        public int DuplicatePoints { get; set; }
    }

    public static partial class TableOperationsExtensions
    {
        public static void Upsert(this TableOperations<MeterDataQualitySummary> meterDataQualitySummaryTable, MeterDataQualitySummary meterDataQualitySummary)
        {
            const string UpsertQuery =
                "MERGE MeterDataQualitySummary WITH (HOLDLOCK) AS Target " +
                "USING (VALUES({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7})) AS Source(MeterID, Date, ExpectedPoints, GoodPoints, LatchedPoints, UnreasonablePoints, NoncongruentPoints, DuplicatePoints) " +
                "ON Target.MeterID = Source.MeterID AND Target.Date = Source.Date " +
                "WHEN MATCHED THEN " +
                "    UPDATE SET " +
                "        Target.ExpectedPoints = Source.ExpectedPoints, " +
                "        Target.GoodPoints = Target.GoodPoints + Source.GoodPoints, " +
                "        Target.LatchedPoints = Target.LatchedPoints + Source.LatchedPoints, " +
                "        Target.UnreasonablePoints = Target.UnreasonablePoints + Source.UnreasonablePoints, " +
                "        Target.NoncongruentPoints = Target.NoncongruentPoints + Source.NoncongruentPoints, " +
                "        Target.DuplicatePoints = Target.DuplicatePoints + Source.DuplicatePoints " +
                "WHEN NOT MATCHED THEN " +
                "    INSERT VALUES(Source.MeterID, Source.Date, Source.ExpectedPoints, Source.GoodPoints, Source.LatchedPoints, Source.UnreasonablePoints, Source.NoncongruentPoints, Source.DuplicatePoints);";

            int meterID = meterDataQualitySummary.MeterID;
            DateTime date = meterDataQualitySummary.Date;
            int expectedPoints = meterDataQualitySummary.ExpectedPoints;
            int goodPoints = meterDataQualitySummary.GoodPoints;
            int latchedPoints = meterDataQualitySummary.LatchedPoints;
            int unreasonablePoints = meterDataQualitySummary.UnreasonablePoints;
            int noncongruentPoints = meterDataQualitySummary.NoncongruentPoints;
            int duplicatePoints = meterDataQualitySummary.DuplicatePoints;

            meterDataQualitySummaryTable.Connection.ExecuteNonQuery(UpsertQuery, meterID, date, expectedPoints, goodPoints, latchedPoints, unreasonablePoints, noncongruentPoints, duplicatePoints);
        }
    }
}
