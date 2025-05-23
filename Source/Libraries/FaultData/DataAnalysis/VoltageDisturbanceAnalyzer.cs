﻿//******************************************************************************************************
//  VoltageDisturbanceAnalyzer.cs - Gbtc
//
//  Copyright © 2015, Grid Protection Alliance.  All Rights Reserved.
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
//  08/10/2015 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using FaultData.DataResources;
using FaultData.DataSets;
using GSF;
using GSF.Configuration;
using GSF.PQDIF.Logical;
using openXDA.Configuration;
using openXDA.Model;
using Phase = GSF.PQDIF.Logical.Phase;

namespace FaultData.DataAnalysis
{
    public class VoltageDisturbanceAnalyzer
    {
        #region [ Members ]

        // Fields
        private Func<DataPoint, bool> m_isDisturbed;
        private Func<double, double, bool> m_isMoreSevere;
        private EventClassification m_eventType;

        private Dictionary<DataGroup, List<Disturbance>> m_disturbances;

        #endregion

        #region [ Constructors ]

        public VoltageDisturbanceAnalyzer(Func<DataPoint, bool> isDisturbed, Func<double, double, bool> isMoreSevere, EventClassification eventType)
        {
            if (isDisturbed is null)
                throw new ArgumentNullException(nameof(isDisturbed));

            if (isMoreSevere is null)
                throw new ArgumentNullException(nameof(isMoreSevere));

            m_isDisturbed = isDisturbed;
            m_isMoreSevere = isMoreSevere;
            m_eventType = eventType;
        }

        #endregion

        #region [ Properties ]

        [Category]
        [SettingName(DataAnalysisSection.CategoryName)]
        public DataAnalysisSection DataAnalysisSettings { get; }
            = new DataAnalysisSection();

        public Dictionary<DataGroup, List<Disturbance>> Disturbances
        {
            get
            {
                return m_disturbances;
            }
        }

        #endregion

        #region [ Methods ]

        public void Initialize(MeterDataSet meterDataSet)
        {
            meterDataSet.Configure(this);

            m_disturbances = new Dictionary<DataGroup, List<Disturbance>>();

            CycleDataResource cycleDataResource = meterDataSet.GetResource<CycleDataResource>();

            int assetCount = meterDataSet.Meter.MeterAssets.Count;

            for (int i = 0; i < cycleDataResource.DataGroups.Count; i++)
            {
                DataGroup dataGroup = cycleDataResource.DataGroups[i];
                VICycleDataGroup viCycleDataGroup = cycleDataResource.VICycleDataGroups[i];
                FastRMSDataResource fastRMSDataResource = meterDataSet.GetResource<FastRMSDataResource>();

                if (assetCount == 1 && dataGroup.Disturbances.Count > 0)
                    ProcessReportedDisturbances(meterDataSet.Meter, dataGroup);
                else if (fastRMSDataResource.FastRMSLookup.TryGetValue(dataGroup, out DataGroup fastRMS))
                    DetectDisturbances(dataGroup, fastRMS);
                else
                    DetectDisturbances(dataGroup, viCycleDataGroup);
            }

            DataGroupsResource dataGroupsResource = meterDataSet.GetResource<DataGroupsResource>();

            foreach (DataGroup dataGroup in dataGroupsResource.DataGroups)
            {
                if (dataGroup.DataSeries.Count > 0)
                    continue;

                if (assetCount == 1 && dataGroup.Disturbances.Count > 0)
                    ProcessReportedDisturbances(meterDataSet.Meter, dataGroup);
            }
        }

        private void ProcessReportedDisturbances(Meter meter, DataGroup dataGroup)
        {
            Asset asset = dataGroup.Asset;

            List<Disturbance> disturbanceList = dataGroup.Disturbances
                .Select(disturbance => ToDisturbance(asset, disturbance))
                .Where(IsDisturbed)
                .ToList();

            if (dataGroup.Samples > 0)
            {
                DateTime startTime = dataGroup.StartTime;
                DateTime endTime = dataGroup.EndTime;
                TimeSpan duration = (endTime - startTime);

                foreach (Disturbance disturbance in disturbanceList)
                {
                    TimeSpan startDuration = disturbance.StartTime - startTime;
                    TimeSpan endDuration = disturbance.EndTime - startTime;
                    disturbance.StartIndex = (int)((startDuration.TotalSeconds / duration.TotalSeconds) * dataGroup.Samples);
                    disturbance.EndIndex = (int)((endDuration.TotalSeconds / duration.TotalSeconds) * dataGroup.Samples);
                }
            }

            IEnumerable<Range<DateTime>> allDisturbanceRanges = disturbanceList
                .Select(ToDateRange);

            int distGroupIndex = 1;
            IEnumerable<Disturbance> worstDisturbances = Range<DateTime>.MergeAllOverlapping(allDisturbanceRanges)
                .Select(range =>
                {
                    Disturbance worst = null;
                    Disturbance worstLL = null;
                    Disturbance worstLN = null;

                    foreach (Disturbance disturbance in disturbanceList.Where(disturbance => ToDateRange(disturbance).Overlaps(range)))
                    {
                        if ((object)worst == null || m_isMoreSevere(disturbance.PerUnitMagnitude, worst.PerUnitMagnitude))
                            worst = disturbance;
                        if (disturbance.IsLLDisturbance && ((object)worstLL == null || m_isMoreSevere(disturbance.PerUnitMagnitude, worstLL.PerUnitMagnitude)))
                            worstLL = disturbance;
                        if (disturbance.IsLNDisturbance && ((object)worstLN == null || m_isMoreSevere(disturbance.PerUnitMagnitude, worstLN.PerUnitMagnitude)))
                            worstLN = disturbance;
                    }

                    if (worstLL != null)
                    {
                        worstLL.IsWorstDisturbance = true;
                        worstLL.WorstDisturbanceGrouping = distGroupIndex;
                    }

                    if (worstLN != null)
                    {
                        worstLN.IsWorstDisturbance = true;
                        worstLN.WorstDisturbanceGrouping = distGroupIndex;
                    }

                    worst.IsWorstDisturbance = true;
                    worst.IsTotalWorstDisturbance = true;
                    worst.WorstDisturbanceGrouping = distGroupIndex;
                    worst = worst.Clone();
                    worst.Phase = Phase.Worst;
                    distGroupIndex++;
                    return worst;
                });

            disturbanceList.AddRange(worstDisturbances);

            if (disturbanceList.Any())
                m_disturbances.Add(dataGroup, disturbanceList);
        }

        private void DetectDisturbances(DataGroup dataGroup, VICycleDataGroup viCycleDataGroup)
        {
            DataSeries va = viCycleDataGroup.VA?.RMS;
            DataSeries vb = viCycleDataGroup.VB?.RMS;
            DataSeries vc = viCycleDataGroup.VC?.RMS;
            DataSeries vab = viCycleDataGroup.VAB?.RMS;
            DataSeries vbc = viCycleDataGroup.VBC?.RMS;
            DataSeries vca = viCycleDataGroup.VCA?.RMS;

            List<Disturbance> disturbanceList = GetDisturbanceList(va, vb, vc, vab, vbc, vca);

            if (disturbanceList.Any())
                m_disturbances.Add(dataGroup, disturbanceList);
        }

        private void DetectDisturbances(DataGroup dataGroup, DataGroup fastRMS)
        {
            DataSeries GetVoltageRMS(string phase) => fastRMS.DataSeries
                .Where(dataSeries => dataSeries.SeriesInfo.Channel.MeasurementType.Name == "Voltage")
                .Where(dataSeries => dataSeries.SeriesInfo.Channel.Phase.Name == phase)
                .Where(dataSeries => dataSeries.SeriesInfo.Channel.MeasurementCharacteristic.Name == "RMS")
                .Where(dataSeries => new[] { "Values", "Instantaneous" }.Contains(dataSeries.SeriesInfo.SeriesType.Name))
                .FirstOrDefault();

            DataSeries va = GetVoltageRMS("AN");
            DataSeries vb = GetVoltageRMS("BN");
            DataSeries vc = GetVoltageRMS("CN");
            DataSeries vab = GetVoltageRMS("AB");
            DataSeries vbc = GetVoltageRMS("BC");
            DataSeries vca = GetVoltageRMS("CA");

            List<Disturbance> disturbanceList = GetDisturbanceList(va, vb, vc, vab, vbc, vca);

            if (disturbanceList.Any())
                m_disturbances.Add(dataGroup, disturbanceList);
        }

        private List<Disturbance> GetDisturbanceList(DataSeries va, DataSeries vb, DataSeries vc, DataSeries vab, DataSeries vbc, DataSeries vca)
        {
            List<Range<int>> aPhaseDisturbanceRanges = DetectDisturbanceRanges(ToPerUnit(va));
            List<Range<int>> bPhaseDisturbanceRanges = DetectDisturbanceRanges(ToPerUnit(vb));
            List<Range<int>> cPhaseDisturbanceRanges = DetectDisturbanceRanges(ToPerUnit(vc));
            List<Range<int>> abPhaseDisturbanceRanges = DetectDisturbanceRanges(ToPerUnit(vab));
            List<Range<int>> bcPhaseDisturbanceRanges = DetectDisturbanceRanges(ToPerUnit(vbc));
            List<Range<int>> caPhaseDisturbanceRanges = DetectDisturbanceRanges(ToPerUnit(vca));

            List<Disturbance> disturbanceList = aPhaseDisturbanceRanges.Select(range => ToDisturbance(va, range, Phase.AN))
                .Concat(bPhaseDisturbanceRanges.Select(range => ToDisturbance(vb, range, Phase.BN)))
                .Concat(cPhaseDisturbanceRanges.Select(range => ToDisturbance(vc, range, Phase.CN)))
                .Concat(abPhaseDisturbanceRanges.Select(range => ToDisturbance(vab, range, Phase.AB)))
                .Concat(bcPhaseDisturbanceRanges.Select(range => ToDisturbance(vbc, range, Phase.BC)))
                .Concat(caPhaseDisturbanceRanges.Select(range => ToDisturbance(vca, range, Phase.CA)))
                .ToList();

            IEnumerable<Range<int>> allDisturbanceRanges = aPhaseDisturbanceRanges
                .Concat(bPhaseDisturbanceRanges)
                .Concat(cPhaseDisturbanceRanges)
                .Concat(abPhaseDisturbanceRanges)
                .Concat(bcPhaseDisturbanceRanges)
                .Concat(caPhaseDisturbanceRanges);

            int distGroupIndex = 1;
            IEnumerable<Disturbance> worstDisturbances = Range<int>.MergeAllOverlapping(allDisturbanceRanges)
                .Select(range =>
                {
                    Disturbance worst = null;
                    Disturbance worstLL = null;
                    Disturbance worstLN = null;

                    foreach (Disturbance disturbance in disturbanceList.Where(disturbance => ToRange(disturbance).Overlaps(range)))
                    {
                        if ((object)worst == null || m_isMoreSevere(disturbance.PerUnitMagnitude, worst.PerUnitMagnitude))
                            worst = disturbance;
                        if (disturbance.IsLLDisturbance && ((object)worstLL == null || m_isMoreSevere(disturbance.PerUnitMagnitude, worstLL.PerUnitMagnitude)))
                            worstLL = disturbance;
                        if (disturbance.IsLNDisturbance &&  ((object)worstLN == null || m_isMoreSevere(disturbance.PerUnitMagnitude, worstLN.PerUnitMagnitude)))
                            worstLN = disturbance;
                    }

                    if (worstLL != null)
                    {
                        worstLL.IsWorstDisturbance = true;
                        worstLL.WorstDisturbanceGrouping = distGroupIndex;
                    }
                    if (worstLN != null)
                    {
                        worstLN.IsWorstDisturbance = true;
                        worstLN.WorstDisturbanceGrouping = distGroupIndex;
                    }

                    worst.IsWorstDisturbance = true;
                    worst.IsTotalWorstDisturbance = true;
                    worst.WorstDisturbanceGrouping = distGroupIndex;
                    worst = worst.Clone();
                    worst.Phase = Phase.Worst;
                    distGroupIndex++;
                    return worst;
                });

            disturbanceList.AddRange(worstDisturbances);

            return disturbanceList;
        }

        private DataSeries ToPerUnit(DataSeries rms)
        {
            double nominalValue = rms?.SeriesInfo.Channel.PerUnitValue ?? GetAssetVoltage(rms);

            if (nominalValue == 0.0D)
                return null;

            return rms?.Multiply(1.0D / nominalValue);
        }

        private double GetAssetVoltage(DataSeries rms)
        {
            if (rms is null)
                return 0.0D;

            Channel channel = rms.SeriesInfo.Channel;

            double assetVoltage = channel.Asset.AssetTypeID == (int)AssetType.Transformer
                ? GetTransformerVoltage(channel)
                : channel.Asset.VoltageKV;

            if (new string[] { "AN", "BN", "CN" }.Contains(channel.Phase.Name))
                assetVoltage /= Math.Sqrt(3.0D);

            return assetVoltage * 1000.0D;
        }

        private double GetTransformerVoltage(Channel channel)
        {
            int connectionPriority = channel.ConnectionPriority;
            Transformer transformer = channel.Asset.QueryAs<Transformer>();

            switch (connectionPriority)
            {
                default: return transformer.PrimaryVoltageKV;
                case 2: return transformer.SecondaryVoltageKV;
                case 3: return transformer.TertiaryVoltageKV;
            }
        }

        private List<Range<int>> DetectDisturbanceRanges(DataSeries rms)
        {
            int samplesPerCycle;
            List<Range<int>> disturbanceRanges;
            int start;
            int end;

            if ((object)rms == null)
                return new List<Range<int>>();

            // Determine the sample rate so that we can exclude
            // disturbances that are shorter than half a cycle
            samplesPerCycle = Transform.CalculateSamplesPerCycle(rms, DataAnalysisSettings.SystemFrequency);

            // Sample rate of zero is invalid
            if (samplesPerCycle == 0)
                return new List<Range<int>>();

            // Initialize disturbanceRanges and the start index
            disturbanceRanges = new List<Range<int>>();
            start = 0;

            // Iterate over all data points in the RMS series
            while (start < rms.DataPoints.Count)
            {
                // If the data point at the start index was measured during
                // disturbed conditions, attempt to locate the end of the disturbance
                if (m_isDisturbed(rms[start]))
                {
                    // Initialize the end index to the data
                    // point just past the start of the disturbance
                    end = start + 1;

                    // Don't iterate beyond the end of the data series
                    while (end < rms.DataPoints.Count)
                    {
                        // If the data point at the end index was not measured during
                        // the disturbance, the end of the disturbance has been found
                        if (!m_isDisturbed(rms[end]))
                            break;

                        // Increment the end index
                        end++;
                    }

                    // If the disturbance lasted for at least half a cycle,
                    // store it in the list of disturbance ranges
                    if ((end - start) > (samplesPerCycle / 2))
                        disturbanceRanges.Add(new Range<int>(start, end - 1));

                    // Move the start index up
                    // to the end of the disturbance
                    start = end;
                }

                // Increment the start index
                start++;
            }

            return disturbanceRanges;
        }

        private Disturbance ToDisturbance(Asset asset, ReportedDisturbance reportedDisturbance)
        {
            double nominalValue = asset.VoltageKV * 1000.0D;

            if (asset.AssetTypeID == (int)AssetType.Transformer)
            {
                //Special Case that needs to be treated seperately No Solution Yet
                nominalValue = 0 * 1000.0D;
            }

            if (new[] { Phase.AN, Phase.BN, Phase.CN }.Contains(reportedDisturbance.Phase))
                nominalValue /= Math.Sqrt(3.0D);

            Disturbance disturbance = new Disturbance();

            double value = m_isMoreSevere(reportedDisturbance.Maximum, reportedDisturbance.Minimum)
                ? reportedDisturbance.Maximum
                : reportedDisturbance.Minimum;

            if (reportedDisturbance.Units == QuantityUnits.Percent)
                value *= nominalValue / 100.0D;
            else if (reportedDisturbance.Units == QuantityUnits.PerUnit)
                value *= nominalValue;

            disturbance.EventType = m_eventType;
            disturbance.Phase = reportedDisturbance.Phase;
            disturbance.StartTime = reportedDisturbance.Time;
            disturbance.EndTime = reportedDisturbance.Time + reportedDisturbance.Duration;
            disturbance.Magnitude = value;
            disturbance.PerUnitMagnitude = value / nominalValue;

            return disturbance;
        }

        private Disturbance ToDisturbance(DataSeries rms, Range<int> range, Phase phase)
        {
            double nominalValue = rms.SeriesInfo.Channel.PerUnitValue ?? GetAssetVoltage(rms);
            Disturbance disturbance = new Disturbance();

            disturbance.EventType = m_eventType;
            disturbance.Phase = phase;
            disturbance.StartIndex = range.Start;
            disturbance.EndIndex = range.End;
            disturbance.StartTime = rms[range.Start].Time;
            disturbance.EndTime = rms[range.End].Time;
            disturbance.Magnitude = GetMagnitude(rms.ToSubSeries(range.Start, range.End));
            disturbance.PerUnitMagnitude = disturbance.Magnitude / nominalValue;

            return disturbance;
        }

        private Range<int> ToRange(Disturbance disturbance)
        {
            return new Range<int>(disturbance.StartIndex, disturbance.EndIndex);
        }

        private Range<DateTime> ToDateRange(Disturbance disturbance)
        {
            return new Range<DateTime>(disturbance.StartTime, disturbance.EndTime);
        }

        private double GetMagnitude(DataSeries dataSeries)
        {
            double magnitude = double.NaN;

            foreach (DataPoint dataPoint in dataSeries.DataPoints)
            {
                if (double.IsNaN(magnitude) || m_isMoreSevere(dataPoint.Value, magnitude))
                    magnitude = dataPoint.Value;
            }

            return magnitude;
        }

        private bool IsDisturbed(Disturbance disturbance)
        {
            DataPoint dataPoint = new DataPoint();
            dataPoint.Time = disturbance.StartTime;
            dataPoint.Value = disturbance.PerUnitMagnitude;
            return m_isDisturbed(dataPoint);
        }

        #endregion
    }
}
