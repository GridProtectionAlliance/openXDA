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
                    VIDataGroup viDataGroup = cycleDataResource.VIDataGroups[i];
                    VICycleDataGroup viCycleDataGroup = cycleDataResource.VICycleDataGroups[i];
                    int fileGroupID = meterDataSet.FileGroup.ID;
                    int lineID = dataGroup.Line.ID;
                    object startTime = ToDateTime2(dataGroup.StartTime);
                    object endTime = ToDateTime2(dataGroup.EndTime);

                    Event evt = eventTable.QueryRecordWhere(Filter, fileGroupID, lineID, startTime, endTime);

                    if (evt == null)
                        continue;

                    double? vPeak = CalcVoltagePeak(viDataGroup);
                    double? vaMax = CalcMax(viCycleDataGroup.VA?.RMS);
                    double? vbMax = CalcMax(viCycleDataGroup.VB?.RMS);
                    double? vcMax = CalcMax(viCycleDataGroup.VC?.RMS);
                    double? vabMax = CalcMax(viCycleDataGroup.VAB?.RMS);
                    double? vbcMax = CalcMax(viCycleDataGroup.VBC?.RMS);
                    double? vcaMax = CalcMax(viCycleDataGroup.VCA?.RMS);
                    double? vaMin = CalcMin(viCycleDataGroup.VA?.RMS);
                    double? vbMin = CalcMin(viCycleDataGroup.VB?.RMS);
                    double? vcMin = CalcMin(viCycleDataGroup.VC?.RMS);
                    double? vabMin = CalcMin(viCycleDataGroup.VAB?.RMS);
                    double? vbcMin = CalcMin(viCycleDataGroup.VBC?.RMS);
                    double? vcaMin = CalcMin(viCycleDataGroup.VCA?.RMS);

                    double? iPeak = CalcPeak(new[] { viDataGroup.IA, viDataGroup.IB, viDataGroup.IC });
                    double? iaMax = CalcMax(viCycleDataGroup.IA?.RMS);
                    double? ibMax = CalcMax(viCycleDataGroup.IB?.RMS);
                    double? icMax = CalcMax(viCycleDataGroup.IC?.RMS);

                    double? ia2t = null;
                    double? ib2t = null;
                    double? ic2t = null;

                    double? initialMW = CalcMW(viCycleDataGroup, true);
                    double? finalMW = CalcMW(viCycleDataGroup, false);

                    if (faultDataResource.FaultLookup.TryGetValue(dataGroup, out DataAnalysis.FaultGroup faultGroup))
                    {
                        ia2t = CalcI2t(faultGroup, viDataGroup.IA);
                        ib2t = CalcI2t(faultGroup, viDataGroup.IB);
                        ic2t = CalcI2t(faultGroup, viDataGroup.IC);
                    }

                    int? pqviewID = null;
                    if (meterDataSet.FilePath.Contains("\\pqview4\\events\\")) {
                        Regex pattern = new Regex(@"^\\pqview4\\events\\PQView4 \d+\\\d+T\d+-(?<PQViewID>\d+).pqd$");
                        Match match = pattern.Match(meterDataSet.FilePath);
                        string str = match.Groups["PQViewID"].Value;
                        if (str != null)
                            pqviewID = int.Parse(str);
                    }

                    eventStatTable.AddNewRecord(new EventStat()
                    {
                        EventID = evt.ID,
                        VPeak = vPeak,
                        VAMax = vaMax,
                        VBMax = vbMax,
                        VCMax = vcMax,
                        VABMax = vabMax,
                        VBCMax = vbcMax,
                        VCAMax = vcaMax,
                        VAMin = vaMin,
                        VBMin = vbMin,
                        VCMin = vcMin,
                        VABMin = vabMin,
                        VBCMin = vbcMin,
                        VCAMin = vcaMin,
                        IPeak = iPeak,
                        IAMax = iaMax,
                        IBMax = ibMax,
                        ICMax = icMax,
                        IA2t = ia2t,
                        IB2t = ib2t,
                        IC2t = ic2t,
                        InitialMW = initialMW,
                        FinalMW = finalMW,
                        PQViewID = pqviewID
                    });
                }
            }
        }

        private double? CalcVoltagePeak(VIDataGroup viDataGroup) => Enumerable.Empty<DataSeries[]>()
            .Concat(new[] { new[] { viDataGroup.VA, viDataGroup.VB, viDataGroup.VC } })
            .Concat(new[] { new[] { viDataGroup.VAB, viDataGroup.VBC, viDataGroup.VCA } })
            .Where(voltageGrouping => voltageGrouping.Any(waveform => waveform != null))
            .Select(CalcPeak)
            .FirstOrDefault();

        private double? CalcPeak(IEnumerable<DataSeries> waveforms) => waveforms
            .Where(waveform => waveform != null)
            .Select(CalcMax)
            .DefaultIfEmpty(null)
            .Max();

        private double? CalcMax(DataSeries rms) => rms?.DataPoints
            .Where(dataPoint => !double.IsNaN(dataPoint.Value))
            .DefaultIfEmpty(null)
            .Max(dataPoint => dataPoint?.Value);

        private double? CalcMin(DataSeries rms) => rms?.DataPoints
            .Where(dataPoint => !double.IsNaN(dataPoint.Value))
            .DefaultIfEmpty(null)
            .Min(dataPoint => dataPoint?.Value);

        private double? CalcI2t(DataAnalysis.FaultGroup faultGroup, DataSeries waveform)
        {
            if (waveform == null)
                return null;

            double samplingInterval = 1.0D / waveform.SampleRate;

            return faultGroup.Faults
                .Where(fault => !fault.IsSuppressed)
                .Where(fault => fault.Summaries.Any(summary => summary.IsValid))
                .Select(fault => waveform.ToSubSeries(fault.StartSample, fault.EndSample))
                .SelectMany(faultSamples => faultSamples.DataPoints)
                .Select(dataPoint => (double?)(dataPoint.Value * dataPoint.Value * samplingInterval))
                .DefaultIfEmpty(null)
                .Sum();
        }

        private double? CalcMW(VICycleDataGroup viCycleDataGroup, bool initial) {
            if (viCycleDataGroup.IA == null || viCycleDataGroup.IB == null || viCycleDataGroup.IC == null) return null;

            if (viCycleDataGroup.VA.RMS.DataPoints.Any() && initial) return CalcInitialLGMW(viCycleDataGroup);
            else if (!viCycleDataGroup.VA.RMS.DataPoints.Any() && initial) return CalcInitialLLMW(viCycleDataGroup);
            else if (viCycleDataGroup.VA.RMS.DataPoints.Any() && !initial) return CalcFinalLGMW(viCycleDataGroup);
            else return CalcFinalLLMW(viCycleDataGroup);
        }

        private double? CalcInitialLGMW(VICycleDataGroup viCycleDataGroup) {
            if (viCycleDataGroup.VA == null || viCycleDataGroup.VB == null || viCycleDataGroup.VC == null) return null;

            Complex va = Complex.FromPolarCoordinates(viCycleDataGroup.VA.RMS.DataPoints.First().Value, viCycleDataGroup.VA.Phase.DataPoints.First().Value);
            Complex ia = Complex.Conjugate(Complex.FromPolarCoordinates(viCycleDataGroup.IA.RMS.DataPoints.First().Value, viCycleDataGroup.IA.Phase.DataPoints.First().Value));
            Complex vb = Complex.FromPolarCoordinates(viCycleDataGroup.VB.RMS.DataPoints.First().Value, viCycleDataGroup.VB.Phase.DataPoints.First().Value);
            Complex ib = Complex.Conjugate(Complex.FromPolarCoordinates(viCycleDataGroup.IB.RMS.DataPoints.First().Value, viCycleDataGroup.IB.Phase.DataPoints.First().Value));
            Complex vc = Complex.FromPolarCoordinates(viCycleDataGroup.VC.RMS.DataPoints.First().Value, viCycleDataGroup.VC.Phase.DataPoints.First().Value);
            Complex ic = Complex.Conjugate(Complex.FromPolarCoordinates(viCycleDataGroup.IC.RMS.DataPoints.First().Value, viCycleDataGroup.IC.Phase.DataPoints.First().Value));

            return (va*ia + vb*ib  + vc*ic).Real / 1E6;
        }

        private double? CalcFinalLGMW(VICycleDataGroup viCycleDataGroup)
        {
            if (viCycleDataGroup.VA == null || viCycleDataGroup.VB == null || viCycleDataGroup.VC == null) return null;

            Complex va = Complex.FromPolarCoordinates(viCycleDataGroup.VA.RMS.DataPoints.Last().Value, viCycleDataGroup.VA.Phase.DataPoints.Last().Value);
            Complex ia = Complex.Conjugate(Complex.FromPolarCoordinates(viCycleDataGroup.IA.RMS.DataPoints.Last().Value, viCycleDataGroup.IA.Phase.DataPoints.Last().Value));
            Complex vb = Complex.FromPolarCoordinates(viCycleDataGroup.VB.RMS.DataPoints.Last().Value, viCycleDataGroup.VB.Phase.DataPoints.Last().Value);
            Complex ib = Complex.Conjugate(Complex.FromPolarCoordinates(viCycleDataGroup.IB.RMS.DataPoints.Last().Value, viCycleDataGroup.IB.Phase.DataPoints.Last().Value));
            Complex vc = Complex.FromPolarCoordinates(viCycleDataGroup.VC.RMS.DataPoints.Last().Value, viCycleDataGroup.VC.Phase.DataPoints.Last().Value);
            Complex ic = Complex.Conjugate(Complex.FromPolarCoordinates(viCycleDataGroup.IC.RMS.DataPoints.Last().Value, viCycleDataGroup.IC.Phase.DataPoints.Last().Value));

            return (va * ia + vb * ib + vc * ic).Real / 1E6;
        }

        private double? CalcInitialLLMW(VICycleDataGroup viCycleDataGroup)
        {
            if (viCycleDataGroup.VCA == null || viCycleDataGroup.VBC == null) return null;

            Complex vca = Complex.FromPolarCoordinates(viCycleDataGroup.VCA.RMS.DataPoints.First().Value, viCycleDataGroup.VCA.Phase.DataPoints.First().Value);
            Complex ia = Complex.Conjugate(Complex.FromPolarCoordinates(viCycleDataGroup.IA.RMS.DataPoints.First().Value, viCycleDataGroup.IA.Phase.DataPoints.First().Value));
            Complex vbc = Complex.FromPolarCoordinates(viCycleDataGroup.VBC.RMS.DataPoints.First().Value, viCycleDataGroup.VBC.Phase.DataPoints.First().Value);
            Complex ib = Complex.Conjugate(Complex.FromPolarCoordinates(viCycleDataGroup.IB.RMS.DataPoints.First().Value, viCycleDataGroup.IB.Phase.DataPoints.First().Value));

            return (-1*vca * ia + vbc * ib).Real / 1E6;
        }

        private double? CalcFinalLLMW(VICycleDataGroup viCycleDataGroup)
        {
            if (viCycleDataGroup.VCA == null || viCycleDataGroup.VBC == null) return null;

            Complex vca = Complex.FromPolarCoordinates(viCycleDataGroup.VCA.RMS.DataPoints.Last().Value, viCycleDataGroup.VCA.Phase.DataPoints.Last().Value);
            Complex ia = Complex.Conjugate(Complex.FromPolarCoordinates(viCycleDataGroup.IA.RMS.DataPoints.Last().Value, viCycleDataGroup.IA.Phase.DataPoints.Last().Value));
            Complex vbc = Complex.FromPolarCoordinates(viCycleDataGroup.VBC.RMS.DataPoints.Last().Value, viCycleDataGroup.VBC.Phase.DataPoints.Last().Value);
            Complex ib = Complex.Conjugate(Complex.FromPolarCoordinates(viCycleDataGroup.IB.RMS.DataPoints.Last().Value, viCycleDataGroup.IB.Phase.DataPoints.Last().Value));

            return (-1 * vca * ia + vbc * ib).Real / 1E6;
        }


    }

}
