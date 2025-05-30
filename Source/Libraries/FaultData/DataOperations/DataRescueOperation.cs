//******************************************************************************************************
//  DataRescueOperation.cs - Gbtc
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
//  06/11/2021 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using FaultData.DataAnalysis;
using FaultData.DataSets;
using GSF.Data;
using openXDA.Model;

namespace FaultData.DataOperations
{
    public class DataRescueOperation : DataOperationBase<MeterDataSet>
    {
        #region [ Members ]

        // Nested Types
        private class ChannelAdjustment
        {
            public int ChannelID { get; }
            public double Multiplier { get; }
            public double Adder { get; }

            public ChannelAdjustment(DataRow row)
            {
                ChannelID = row.ConvertField<int>("ChannelID");
                Multiplier = row.ConvertField<int>("Multiplier");
                Adder = row.ConvertField<int>("Adder");
            }
        }

        #endregion

        #region [ Methods ]

        public override void Execute(MeterDataSet meterDataSet)
        {
            List<ChannelAdjustment> channelAdjustments;
            TimeSpan shift;

            using (AdoDataConnection connection = meterDataSet.CreateDbConnection())
            {
                channelAdjustments = QueryChannelAdjustments(connection, meterDataSet);
                shift = QueryTimeShift(connection, meterDataSet);
            }

            ApplyChannelAdjustments(meterDataSet, channelAdjustments);
            ApplyTimeShift(meterDataSet, shift);
        }

        private void ApplyChannelAdjustments(MeterDataSet meterDataSet, List<ChannelAdjustment> adjustments)
        {
            if (!adjustments.Any())
                return;

            ILookup<int, DataSeries> lookup = meterDataSet.DataSeries
                .ToLookup(dataSeries => dataSeries.SeriesInfo.ChannelID);

            foreach (ChannelAdjustment adjustment in adjustments)
            {
                double multiplier = adjustment.Multiplier;
                double adder = adjustment.Adder;

                foreach (DataSeries dataSeries in lookup[adjustment.ChannelID])
                {
                    foreach (DataPoint dataPoint in dataSeries.DataPoints)
                        dataPoint.Value = dataPoint.Value * multiplier + adder;
                }
            }
        }

        private void ApplyTimeShift(MeterDataSet meterDataSet, TimeSpan shift)
        {
            if (shift == TimeSpan.Zero)
                return;

            foreach (DataSeries dataSeries in meterDataSet.DataSeries)
            {
                foreach (DataPoint dataPoint in dataSeries.DataPoints)
                    dataPoint.Time += shift;
            }

            foreach (DataSeries dataSeries in meterDataSet.Digitals)
            {
                foreach (DataPoint dataPoint in dataSeries.DataPoints)
                    dataPoint.Time += shift;
            }

            for (int i = 0; i < meterDataSet.ReportedDisturbances.Count; i++)
            {
                ReportedDisturbance disturbance = meterDataSet.ReportedDisturbances[i];
                DateTime time = disturbance.Time + shift;
                meterDataSet.ReportedDisturbances[i] = disturbance.ShiftTimestampTo(time);
            }

            meterDataSet.FileGroup.DataStartTime += shift;
            meterDataSet.FileGroup.DataEndTime += shift;
        }

        private List<ChannelAdjustment> QueryChannelAdjustments(AdoDataConnection connection, MeterDataSet meterDataSet)
        {
            const string QueryFormat =
                "SELECT " +
                "    DataRescueChannelAdjustment.ChannelID, " +
                "    DataRescueChannelAdjustment.Multiplier, " +
                "    DataRescueChannelAdjustment.Adder " +
                "FROM " +
                "    DataRescueOperation JOIN " +
                "    DataRescueChannelAdjustment ON DataRescueChannelAdjustment.DataRescueOperationID = DataRescueOperation.ID JOIN " +
                "    DataRescueFileGroup ON DataRescueFileGroup.DataRescueOperationID = DataRescueOperation.ID " +
                "WHERE " +
                "    DataRescueOperation.MeterID = {0} AND " +
                "    DataRescueFileGroup.FileGroupID = {1} " +
                "ORDER BY " +
                "    DataRescueOperation.ID, " +
                "    DataRescueChannelAdjustment.ID";

            int meterID = meterDataSet.Meter.ID;
            int fileGroupID = meterDataSet.FileGroup.ID;

            using (DataTable table = connection.RetrieveData(QueryFormat, meterID, fileGroupID))
            {
                return table
                    .AsEnumerable()
                    .Select(row => new ChannelAdjustment(row))
                    .ToList();
            }
        }

        private TimeSpan QueryTimeShift(AdoDataConnection connection, MeterDataSet meterDataSet)
        {
            const string QueryFormat =
                "SELECT DataRescueTimeShift.Ticks " +
                "FROM " +
                "    DataRescueOperation JOIN " +
                "    DataRescueTimeShift ON DataRescueTimeShift.DataRescueOperationID = DataRescueOperation.ID JOIN " +
                "    DataRescueFileGroup ON DataRescueFileGroup.DataRescueOperationID = DataRescueOperation.ID " +
                "WHERE " +
                "    DataRescueOperation.MeterID = {0} AND " +
                "    DataRescueFileGroup.FileGroupID = {1} " +
                "ORDER BY " +
                "    DataRescueOperation.ID, " +
                "    DataRescueTimeShift.ID";

            int meterID = meterDataSet.Meter.ID;
            int fileGroupID = meterDataSet.FileGroup.ID;

            using (DataTable table = connection.RetrieveData(QueryFormat, meterID, fileGroupID))
            {
                long ticks = table
                    .AsEnumerable()
                    .Select(row => row.ConvertField<long>("Ticks"))
                    .DefaultIfEmpty(0L)
                    .Sum();

                return TimeSpan.FromTicks(ticks);
            }
        }

        #endregion
    }
}
