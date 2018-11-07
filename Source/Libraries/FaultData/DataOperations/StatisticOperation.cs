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


using System.Collections.Generic;
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
            SagDataResource sagDataResource = meterDataSet.GetResource<SagDataResource>();
            SwellDataResource swellDataResource = meterDataSet.GetResource<SwellDataResource>();
            InterruptionDataResource interruptionDataResource = meterDataSet.GetResource<InterruptionDataResource>();
            FaultDataResource faultDataResource = meterDataSet.GetResource<FaultDataResource>();
            EventClassificationResource eventClassificationResource = meterDataSet.GetResource<EventClassificationResource>();
            using (AdoDataConnection connection = meterDataSet.CreateDbConnection())
            {
                TableOperations<Event> eventTable = new TableOperations<Event>(connection);

                List<Event> events = eventTable
                    .QueryRecordsWhere("FileGroupID = {0}", meterDataSet.FileGroup.ID)
                    .ToList();

                foreach (Event evt in events)
                {
                    // TODO: Calculate statistics here
                    double? iMin = meterDataSet.DataSeries.Where(ds => ds.SeriesInfo.Channel.MeasurementType.Name == "Current" && ds.SeriesInfo.Channel.MeasurementCharacteristic.Name == "Instantaneous")?.Select(ds => ds.DataPoints.Where(dp => dp.Time >= evt.StartTime && dp.Time <= evt.EndTime).Min(dp => dp.Value)).Min() ?? null;
                    double? iMax = meterDataSet.DataSeries.Where(ds => ds.SeriesInfo.Channel.MeasurementType.Name == "Current" && ds.SeriesInfo.Channel.MeasurementCharacteristic.Name == "Instantaneous")?.Select(ds => ds.DataPoints.Where(dp => dp.Time >= evt.StartTime && dp.Time <= evt.EndTime).Max(dp => dp.Value)).Max() ?? null;
                    double? vMin = meterDataSet.DataSeries.Where(ds => ds.SeriesInfo.Channel.MeasurementType.Name == "Voltage" && ds.SeriesInfo.Channel.MeasurementCharacteristic.Name == "Instantaneous")?.Select(ds => ds.DataPoints.Where(dp => dp.Time >= evt.StartTime && dp.Time <= evt.EndTime).Min(dp => dp.Value)).Min() ?? null;
                    double? vMax = meterDataSet.DataSeries.Where(ds => ds.SeriesInfo.Channel.MeasurementType.Name == "Voltage" && ds.SeriesInfo.Channel.MeasurementCharacteristic.Name == "Instantaneous")?.Select(ds => ds.DataPoints.Where(dp => dp.Time >= evt.StartTime && dp.Time <= evt.EndTime).Max(dp => dp.Value)).Max() ?? null;
                    double? i2t = CalcI2t(meterDataSet, faultDataResource, cycleDataResource);

                    // TODO: Push statistics to table here
                    (new TableOperations<EventStat>(connection)).AddNewRecord(new EventStat() {
                        EventID = evt.ID,
                        IMax = iMax,
                        IMin = iMin,
                        VMax = vMax,
                        VMin = vMin
                    });

                }
            }
        }
        private double? CalcI2t(MeterDataSet meterDataSet, FaultDataResource faultDataResource, CycleDataResource cycleDataResource)
        {
            double i2t = 0;
            foreach (KeyValuePair<DataGroup, DataAnalysis.FaultGroup> faultGroup in faultDataResource.FaultLookup)
            {
                foreach (DataAnalysis.Fault fault in faultGroup.Value.Faults)
                {
                    if (fault.IsSuppressed) continue;

                    foreach (DataAnalysis.Fault.Summary faultSummary in fault.Summaries) {
                        if (!faultSummary.IsValid || !faultSummary.IsSelectedAlgorithm) continue;

                         i2t += cycleDataResource
                            .VICycleDataGroups.Select( vcdg => (new List<double>() {
                                vcdg.IA.RMS.DataPoints.Where(dp => dp.Time >= fault.InceptionTime && dp.Time <= fault.ClearingTime).Select(dp => dp.Value * dp.Value).Sum(),
                                vcdg.IB.RMS.DataPoints.Where(dp => dp.Time >= fault.InceptionTime && dp.Time <= fault.ClearingTime).Select(dp => dp.Value * dp.Value).Sum(),
                                vcdg.IC.RMS.DataPoints.Where(dp => dp.Time >= fault.InceptionTime && dp.Time <= fault.ClearingTime).Select(dp => dp.Value * dp.Value).Sum() }).Sum()).Sum();
                    }
                }
            }

            return (i2t != 0 ? i2t : (double?)null);
        }

    }

}
