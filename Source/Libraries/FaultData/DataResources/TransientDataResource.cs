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
            IEnumerable<Disturbance> vaTransients = DetectTransients(viDataGroup.VA, Phase.AN, lineToGroundThreshold);
            IEnumerable<Disturbance> vbTransients = DetectTransients(viDataGroup.VB, Phase.BN, lineToGroundThreshold);
            IEnumerable<Disturbance> vcTransients = DetectTransients(viDataGroup.VC, Phase.CN, lineToGroundThreshold);
            IEnumerable<Disturbance> vabTransients = DetectTransients(viDataGroup.VAB, Phase.AB, lineToLineThreshold);
            IEnumerable<Disturbance> vbcTransients = DetectTransients(viDataGroup.VBC, Phase.BC, lineToLineThreshold);
            IEnumerable<Disturbance> vcaTransients = DetectTransients(viDataGroup.VCA, Phase.CA, lineToLineThreshold);

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

        private IEnumerable<Disturbance> DetectTransients(DataSeries waveForm, Phase phase, Func<double, double, double> thresholdFunc) {
            waveForm.SeriesInfo.ConnectionFactory = () => new AdoDataConnection("systemSettings");
            double sampleRate = waveForm.SampleRate;
            int samplesPerCycle = Transform.CalculateSamplesPerCycle(waveForm.SampleRate, SystemFrequency);
            double nominalVoltage = waveForm.SeriesInfo.Channel.Line.VoltageKV;
            double thresholdValue = thresholdFunc(nominalVoltage, TransientThreshold);
            List<Disturbance> disturbances = new List<Disturbance>();
            List<DataPoint> firstCycle = waveForm.DataPoints.Take(samplesPerCycle).ToList();
            List<DataPoint> fullWaveForm = waveForm.DataPoints.Select((dataPoint, index) => new DataPoint() { Time = dataPoint.Time, Value = Math.Abs(dataPoint.Value - firstCycle[index % samplesPerCycle].Value) }).ToList();
            
            IEnumerable<DataPoint> thresholdCrosses = fullWaveForm.Where(dataPoint => {
                int index = fullWaveForm.IndexOf(dataPoint);
                if (index == 0) return dataPoint.Value > thresholdValue;
                else if (fullWaveForm[index - 1].Value < thresholdValue) return dataPoint.Value >= thresholdValue;
                else if (fullWaveForm[index - 1].Value >= thresholdValue) return dataPoint.Value < thresholdValue;
                else return false;
            });

            //Use this to output data for excel analysis.

            //using (StreamWriter sw = new StreamWriter(waveForm.SeriesInfo.Channel.Meter.Name + phase.ToString() + "transtest.csv"))
            //{
            //    sw.WriteLine("ticks, waveform, 1 cycle, difference, threshold");

            //    waveForm.DataPoints.ForEach(dataPoint =>
            //    {
            //        int index = waveForm.DataPoints.IndexOf(dataPoint);
            //        sw.WriteLine($"{dataPoint.Time.Ticks},{dataPoint.Value}, {firstCycle[index % samplesPerCycle].Value},{Math.Abs(dataPoint.Value - firstCycle[index % samplesPerCycle].Value)}, {thresholdValue}");
            //    });
            //}

            if (thresholdCrosses.Any())
            {
                double magnitude = fullWaveForm.Select(dataPoint => dataPoint.Value).Max();
                Disturbance disturbance = new Disturbance()
                {
                    EventType = EventClassification.Transient,
                    Phase = phase,
                    StartIndex = fullWaveForm.IndexOf(thresholdCrosses.First()),
                    EndIndex = fullWaveForm.IndexOf(thresholdCrosses.Last()),
                    StartTime = thresholdCrosses.First().Time,
                    EndTime = thresholdCrosses.Last().Time,
                    Magnitude = magnitude,
                    PerUnitMagnitude = magnitude / (nominalVoltage * 1000)
                };
                disturbances.Add(disturbance);
            }
            return disturbances;
        }

        Func<double, double, double> lineToLineThreshold = (nominalKV, transientThreshold) =>
         {
             double nominalVoltage = nominalKV * 1000;
             return nominalVoltage * transientThreshold / 100;
         };

        Func<double, double, double> lineToGroundThreshold = (nominalKV, transientThreshold) =>
        {
            double nominalVoltage = nominalKV * 1000;
            nominalVoltage /= Math.Sqrt(3);
            return nominalVoltage * transientThreshold / 100;
        };

        #endregion
    }
}
