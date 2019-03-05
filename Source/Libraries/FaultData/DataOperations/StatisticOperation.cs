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
using System.Numerics;
using System.Text.RegularExpressions;
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
                    double initialMW = CalcMW(viCycleDataGroup, true);
                    double finalMW = CalcMW(viCycleDataGroup, false);

                    if (faultDataResource.FaultLookup.TryGetValue(dataGroup, out DataAnalysis.FaultGroup faultGroup))
                        i2t = CalcI2t(faultGroup, viCycleDataGroup);


                    int? pqviewID = null;
                    if (meterDataSet.FilePath.Contains("\\pqview4\\events\\")) {
                        Regex pattern = new Regex(@"^\\pqview4\\events\\PQView4 \d+\\\d+T\d+-(?<PQViewID>\d+).pqd$");
                        Match match = pattern.Match(meterDataSet.FilePath);
                        string str = match.Groups["PQViewID"].Value;
                        if(str != null)
                            pqviewID = int.Parse(str);
                    }

                    eventStatTable.AddNewRecord(new EventStat()
                    {
                        EventID = evt.ID,
                        IMax = iMax,
                        IMin = iMin,
                        VMax = vMax,
                        VMin = vMin,
                        I2t = i2t,
                        InitialMW = initialMW,
                        FinalMW = finalMW,
                        PQViewID = pqviewID
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

        private double CalcMW(VICycleDataGroup viCycleDataGroup, bool initial) {
            if (viCycleDataGroup.VA.RMS.DataPoints.Any() && initial) return CalcInitialLGMW(viCycleDataGroup);
            else if (!viCycleDataGroup.VA.RMS.DataPoints.Any() && initial) return CalcInitialLLMW(viCycleDataGroup);
            else if (viCycleDataGroup.VA.RMS.DataPoints.Any() && !initial) return CalcFinalLGMW(viCycleDataGroup);
            else return CalcFinalLLMW(viCycleDataGroup);
        }

        private double CalcInitialLGMW(VICycleDataGroup viCycleDataGroup) {
            Complex va = Complex.FromPolarCoordinates(viCycleDataGroup.VA.RMS.DataPoints.First().Value, viCycleDataGroup.VA.Phase.DataPoints.First().Value);
            Complex ia = Complex.Conjugate(Complex.FromPolarCoordinates(viCycleDataGroup.IA.RMS.DataPoints.First().Value, viCycleDataGroup.IA.Phase.DataPoints.First().Value));
            Complex vb = Complex.FromPolarCoordinates(viCycleDataGroup.VB.RMS.DataPoints.First().Value, viCycleDataGroup.VB.Phase.DataPoints.First().Value);
            Complex ib = Complex.Conjugate(Complex.FromPolarCoordinates(viCycleDataGroup.IB.RMS.DataPoints.First().Value, viCycleDataGroup.IB.Phase.DataPoints.First().Value));
            Complex vc = Complex.FromPolarCoordinates(viCycleDataGroup.VC.RMS.DataPoints.First().Value, viCycleDataGroup.VC.Phase.DataPoints.First().Value);
            Complex ic = Complex.Conjugate(Complex.FromPolarCoordinates(viCycleDataGroup.IC.RMS.DataPoints.First().Value, viCycleDataGroup.IC.Phase.DataPoints.First().Value));

            return (va*ia + vb*ib  + vc*ic).Real / 1E6;
        }

        private double CalcFinalLGMW(VICycleDataGroup viCycleDataGroup)
        {
            Complex va = Complex.FromPolarCoordinates(viCycleDataGroup.VA.RMS.DataPoints.Last().Value, viCycleDataGroup.VA.Phase.DataPoints.Last().Value);
            Complex ia = Complex.Conjugate(Complex.FromPolarCoordinates(viCycleDataGroup.IA.RMS.DataPoints.Last().Value, viCycleDataGroup.IA.Phase.DataPoints.Last().Value));
            Complex vb = Complex.FromPolarCoordinates(viCycleDataGroup.VB.RMS.DataPoints.Last().Value, viCycleDataGroup.VB.Phase.DataPoints.Last().Value);
            Complex ib = Complex.Conjugate(Complex.FromPolarCoordinates(viCycleDataGroup.IB.RMS.DataPoints.Last().Value, viCycleDataGroup.IB.Phase.DataPoints.Last().Value));
            Complex vc = Complex.FromPolarCoordinates(viCycleDataGroup.VC.RMS.DataPoints.Last().Value, viCycleDataGroup.VC.Phase.DataPoints.Last().Value);
            Complex ic = Complex.Conjugate(Complex.FromPolarCoordinates(viCycleDataGroup.IC.RMS.DataPoints.Last().Value, viCycleDataGroup.IC.Phase.DataPoints.Last().Value));

            return (va * ia + vb * ib + vc * ic).Real / 1E6;
        }

        private double CalcInitialLLMW(VICycleDataGroup viCycleDataGroup)
        {
            Complex vca = Complex.FromPolarCoordinates(viCycleDataGroup.VCA.RMS.DataPoints.First().Value, viCycleDataGroup.VCA.Phase.DataPoints.First().Value);
            Complex ia = Complex.Conjugate(Complex.FromPolarCoordinates(viCycleDataGroup.IA.RMS.DataPoints.First().Value, viCycleDataGroup.IA.Phase.DataPoints.First().Value));
            Complex vbc = Complex.FromPolarCoordinates(viCycleDataGroup.VBC.RMS.DataPoints.First().Value, viCycleDataGroup.VBC.Phase.DataPoints.First().Value);
            Complex ib = Complex.Conjugate(Complex.FromPolarCoordinates(viCycleDataGroup.IB.RMS.DataPoints.First().Value, viCycleDataGroup.IB.Phase.DataPoints.First().Value));

            return (-1*vca * ia + vbc * ib).Real / 1E6;
        }

        private double CalcFinalLLMW(VICycleDataGroup viCycleDataGroup)
        {
            Complex vca = Complex.FromPolarCoordinates(viCycleDataGroup.VCA.RMS.DataPoints.Last().Value, viCycleDataGroup.VCA.Phase.DataPoints.Last().Value);
            Complex ia = Complex.Conjugate(Complex.FromPolarCoordinates(viCycleDataGroup.IA.RMS.DataPoints.Last().Value, viCycleDataGroup.IA.Phase.DataPoints.Last().Value));
            Complex vbc = Complex.FromPolarCoordinates(viCycleDataGroup.VBC.RMS.DataPoints.Last().Value, viCycleDataGroup.VBC.Phase.DataPoints.Last().Value);
            Complex ib = Complex.Conjugate(Complex.FromPolarCoordinates(viCycleDataGroup.IB.RMS.DataPoints.Last().Value, viCycleDataGroup.IB.Phase.DataPoints.Last().Value));

            return (-1 * vca * ia + vbc * ib).Real / 1E6;
        }


    }

}
