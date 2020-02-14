//******************************************************************************************************
//  SagDataResource.cs - Gbtc
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
//  07/28/2015 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using FaultData.DataAnalysis;
using FaultData.DataSets;
using GSF;
using GSF.Data;
using Phase = GSF.PQDIF.Logical.Phase;

namespace FaultData.DataResources
{
    public class TransientDataResource : DataResourceBase<MeterDataSet>
    {
        #region [ Members ]

        // Fields
        private double m_systemFrequency;
        private double m_transientThreshold = 10.0D;

        private Dictionary<DataGroup, List<Disturbance>> m_transients = new Dictionary<DataGroup, List<Disturbance>>();

        #endregion

        #region [ Properties ]

        [Setting]
        public double SystemFrequency
        {
            get
            {
                return m_systemFrequency;
            }
            set
            {
                m_systemFrequency = value;
            }
        }

        public double TransientThreshold
        {
            get
            {
                return m_transientThreshold;
            }
            set
            {
                m_transientThreshold = value;
            }
        }

        public Dictionary<DataGroup, List<Disturbance>> Transients
        {
            get
            {
                return m_transients;
            }
        }

        #endregion

        #region [ Methods ]

        public override void Initialize(MeterDataSet meterDataSet)
        {
            DataGroup dataGroup;
            VIDataGroup viDataGroup;
            CycleDataResource cycleDataResource = meterDataSet.GetResource<CycleDataResource>();
            for (int i = 0; i < cycleDataResource.DataGroups.Count; i++)
            {
                dataGroup = cycleDataResource.DataGroups[i];
                viDataGroup = cycleDataResource.VIDataGroups[i];

                IEnumerable<Disturbance> transients = DetectTransients(viDataGroup);

                if(transients.Any())
                    Transients.Add(dataGroup, transients.ToList());
            }
        }

        private IEnumerable<Disturbance> DetectTransients(VIDataGroup viDataGroup)
        {
            IEnumerable<Disturbance> vaTransients = DetectTransients(viDataGroup.VA, Phase.AN);
            IEnumerable<Disturbance> vbTransients = DetectTransients(viDataGroup.VB, Phase.BN);
            IEnumerable<Disturbance> vcTransients = DetectTransients(viDataGroup.VC, Phase.CN);
            IEnumerable<Disturbance> vabTransients = DetectTransients(viDataGroup.VAB, Phase.AB);
            IEnumerable<Disturbance> vbcTransients = DetectTransients(viDataGroup.VBC, Phase.BC);
            IEnumerable<Disturbance> vcaTransients = DetectTransients(viDataGroup.VCA, Phase.CA);

            List<Disturbance> transientList = vaTransients.Concat(vbTransients).Concat(vcTransients).Concat(vabTransients).Concat(vbcTransients).Concat(vcaTransients).ToList();

            Disturbance worst = null;
            foreach (Disturbance transiet in transientList)
            {

                if ((object)worst == null || transiet.PerUnitMagnitude > worst.PerUnitMagnitude)
                    worst = transiet;

                worst = worst.Clone();
                worst.Phase = Phase.Worst;
            }

            if(worst != null)
                transientList.Add(worst);

            return transientList;
        }

        private IEnumerable<Disturbance> DetectTransients(DataSeries waveform, Phase phase) {
            if (waveform == null)
                return Enumerable.Empty<Disturbance>();

            // Obtain a list of time gaps in the waveform
            List<int> gapIndexes = Enumerable.Range(0, waveform.DataPoints.Count - 1)
                .Where(index =>
                {
                    DataPoint p1 = waveform[index];
                    DataPoint p2 = waveform[index + 1];
                    double cycleDiff = (p2.Time - p1.Time).TotalSeconds * SystemFrequency;

                    // Detect gaps larger than a quarter cycle.
                    // Tolerance of 0.000062 calculated
                    // assuming 3.999 samples per cycle
                    return (cycleDiff > 0.250062);
                })
                .ToList();

            double sampleRate = waveform.SampleRate;
            int samplesPerCycle = Transform.CalculateSamplesPerCycle(waveform.SampleRate, SystemFrequency);
            double nominalVoltage = waveform.SeriesInfo.Channel.Line.VoltageKV * 1000.0D;

            if (IsLineToNeutral(waveform.SeriesInfo.Channel.Phase.Name))
                nominalVoltage /= Math.Sqrt(3.0D);

            double thresholdValue = nominalVoltage * TransientThreshold / 100;
            List<Disturbance> disturbances = new List<Disturbance>();

            List<int> detections = Enumerable.Range(0, waveform.DataPoints.Count)
                .Select(index => new { P1 = index - samplesPerCycle, P2 = index, P3 = index + samplesPerCycle })
                .Where(obj => obj.P1 >= 0 && obj.P3 < waveform.DataPoints.Count)
                .Where(obj => !gapIndexes.Any(index => obj.P1 <= index && index < obj.P3))
                .Where(obj =>
                {
                    double p1 = waveform[obj.P1].Value;
                    double p2 = waveform[obj.P2].Value;
                    double p3 = waveform[obj.P3].Value;
                    double p1Diff = Math.Abs(p2 - p1);
                    double p3Diff = Math.Abs(p2 - p3);
                    return p1Diff >= thresholdValue && p3Diff >= thresholdValue;
                })
                .Select(obj => obj.P2)
                .ToList();

            IEnumerable<Range<int>> ranges = detections
                .Zip(detections.Skip(1), (p1, p2) => new Range<int>(p1, p2))
                .Where(range => range.End - range.Start < samplesPerCycle);

            double nominalPeakVoltage = nominalVoltage * Math.Sqrt(2.0D);

            return Range<int>.MergeAllOverlapping(ranges)
                .Select(range =>
                {
                    DataSeries subSeries = waveform.ToSubSeries(range.Start, range.End);
                    double max = Math.Abs(subSeries.Maximum);
                    double min = Math.Abs(subSeries.Minimum);
                    double peak = Math.Max(max, min);

                    return new Disturbance()
                    {
                        EventType = EventClassification.Transient,
                        Phase = phase,
                        StartIndex = range.Start,
                        EndIndex = range.End,
                        StartTime = waveform[range.Start].Time,
                        EndTime = waveform[range.End].Time,
                        Magnitude = peak,
                        PerUnitMagnitude = peak / nominalPeakVoltage
                    };
                })
                .ToList();
        }

        private bool IsLineToNeutral(string phase)
        {
            string[] lineToNeutralPhases = { "AN", "BN", "CN" };
            return lineToNeutralPhases.Contains(phase);
        }

        #endregion
    }
}
