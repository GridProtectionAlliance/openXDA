//******************************************************************************************************
//  StatisticOperation.cs - Gbtc
//
//  Copyright © 2018, Grid Protection Alliance.  All Rights Reserved.
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
//  11/07/2018 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************


using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using FaultData.DataAnalysis;
using FaultData.DataResources;
using FaultData.DataSets;
using GSF.Data;
using GSF.Data.Model;
using openXDA.Model;

namespace FaultData.DataOperations
{
    public class StatisticOperation : DataOperationBase<MeterDataSet>
    {
        public override void Execute(MeterDataSet meterDataSet)
        {
            CycleDataResource cycleDataResource = meterDataSet.GetResource<CycleDataResource>();
            FaultDataResource faultDataResource = meterDataSet.GetResource<FaultDataResource>();

            using (AdoDataConnection connection = meterDataSet.CreateDbConnection())
            {
                object ToDateTime2(DateTime dateTime)
                {
                    using (IDbCommand command = connection.Connection.CreateCommand())
                    {
                        IDbDataParameter parameter = command.CreateParameter();
                        parameter.DbType = DbType.DateTime2;
                        parameter.Value = dateTime;
                        return parameter;
                    }
                }

                TableOperations<Event> eventTable = new TableOperations<Event>(connection);
                TableOperations<EventStat> eventStatTable = new TableOperations<EventStat>(connection);

                for (int i = 0; i < cycleDataResource.DataGroups.Count; i++)
                {
                    const string Filter =
                        "FileGroupID = {0} AND " +
                        "LineID = {1} AND " +
                        "StartTime = {2} AND " +
                        "EndTime = {3}";

                    DataGroup dataGroup = cycleDataResource.DataGroups[i];
                    VICycleDataGroup viCycleDataGroup = cycleDataResource.VICycleDataGroups[i];
                    int fileGroupID = meterDataSet.FileGroup.ID;
                    int lineID = dataGroup.Line.ID;
                    object startTime = ToDateTime2(dataGroup.StartTime);
                    object endTime = ToDateTime2(dataGroup.EndTime);

                    Event evt = eventTable.QueryRecordWhere(Filter, fileGroupID, lineID, startTime, endTime);

                    if (evt == null)
                        continue;

                    DataSeries[] currents = { viCycleDataGroup.IA?.RMS, viCycleDataGroup.IB?.RMS, viCycleDataGroup.IC?.RMS };
                    DataSeries[] voltages = { viCycleDataGroup.VA?.RMS, viCycleDataGroup.VB?.RMS, viCycleDataGroup.VC?.RMS };

                    if (voltages.All(voltage => voltage == null))
                        voltages = new[] { viCycleDataGroup.VAB?.RMS, viCycleDataGroup.VBC?.RMS, viCycleDataGroup.VCA?.RMS };

                    List<DataPoint> currentDataPoints = currents
                        .Where(ds => ds != null)
                        .SelectMany(ds => ds.DataPoints)
                        .DefaultIfEmpty(null)
                        .ToList();

                    List<DataPoint> voltageDataPoints = voltages
                        .Where(ds => ds != null)
                        .SelectMany(ds => ds.DataPoints)
                        .DefaultIfEmpty(null)
                        .ToList();

                    double? iMin = currentDataPoints.Min(dp => dp?.Value);
                    double? iMax = currentDataPoints.Max(dp => dp?.Value);
                    double? vMin = voltageDataPoints.Min(dp => dp?.Value);
                    double? vMax = voltageDataPoints.Max(dp => dp?.Value);
                    double? i2t = null;

                    if (faultDataResource.FaultLookup.TryGetValue(dataGroup, out DataAnalysis.FaultGroup faultGroup))
                        i2t = CalcI2t(faultGroup, viCycleDataGroup);

                    eventStatTable.AddNewRecord(new EventStat()
                    {
                        EventID = evt.ID,
                        IMax = iMax,
                        IMin = iMin,
                        VMax = vMax,
                        VMin = vMin,
                        I2t = i2t
                    });
                }
            }
        }

        private double? CalcI2t(DataAnalysis.FaultGroup faultGroup, VICycleDataGroup viCycleDataGroup)
        {
            double i2t = 0;

            foreach (DataAnalysis.Fault fault in faultGroup.Faults)
            {
                if (fault.IsSuppressed)
                    continue;

                foreach (DataAnalysis.Fault.Summary faultSummary in fault.Summaries)
                {
                    if (!faultSummary.IsValid || !faultSummary.IsSelectedAlgorithm)
                        continue;

                    double samplingInterval = 1.0D / viCycleDataGroup.IA.RMS.SampleRate;

                    i2t += new[] { viCycleDataGroup.IA?.RMS, viCycleDataGroup.IB?.RMS, viCycleDataGroup.IC?.RMS }
                        .Where(ds => ds != null)
                        .SelectMany(ds => ds.DataPoints)
                        .Where(dp => dp.Time >= fault.InceptionTime && dp.Time <= fault.ClearingTime)
                        .Select(dp => dp.Value * dp.Value * samplingInterval)
                        .Sum();
                }
            }

            return (i2t != 0 ? i2t : (double?)null);
        }
    }

}
