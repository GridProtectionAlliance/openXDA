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
using System.Numerics;
using FaultData.DataAnalysis;
using FaultData.DataSets;
using GSF;
using GSF.Data;
using MathNet.Numerics.IntegralTransforms;
using Phase = GSF.PQDIF.Logical.Phase;
using Newtonsoft.Json;
using openXDA.Model;

namespace FaultData.DataResources
{
    public class SnapshotDataResource : DataResourceBase<MeterDataSet>
    {
        #region [ Members ]

        // Fields
        private class PhasorResult {
            public double Angle { get; set; }
            public double Magnitude { get; set; }
        }

        #endregion

        #region [ Properties ]
        public bool IsSnapshot { get; private set; } = false;
        public Dictionary<Channel, string> Snapshots { get; } = new Dictionary<Channel, string>();
        #endregion

        #region [ Methods ]

        public override void Initialize(MeterDataSet meterDataSet)
        {
            if (!meterDataSet.FilePath.ToLower().Contains("snapshot")) return;

            IsSnapshot = true;

            CycleDataResource cycleDataResource = meterDataSet.GetResource<CycleDataResource>();
            for (int i = 0; i < cycleDataResource.DataGroups.Count; i++)
            {
                ProcessSnapshots(cycleDataResource.SystemFrequency, cycleDataResource.VIDataGroups[i]);
            }
        }

        private void ProcessSnapshots(double systemFrequency, VIDataGroup viDataGroup)
        {
            ProcessSnapshots(systemFrequency, viDataGroup.VA, Phase.AN);
            ProcessSnapshots(systemFrequency, viDataGroup.IA, Phase.AN);
            ProcessSnapshots(systemFrequency, viDataGroup.VB, Phase.BN);
            ProcessSnapshots(systemFrequency, viDataGroup.IB, Phase.BN);
            ProcessSnapshots(systemFrequency, viDataGroup.VC, Phase.CN);
            ProcessSnapshots(systemFrequency, viDataGroup.IC, Phase.CN);
            ProcessSnapshots(systemFrequency, viDataGroup.VAB, Phase.AB);
            ProcessSnapshots(systemFrequency, viDataGroup.VBC, Phase.BC);
            ProcessSnapshots(systemFrequency, viDataGroup.VCA, Phase.CA);
        }

        private void ProcessSnapshots(double systemFrequency, DataSeries waveForm, Phase phase) {
            if (waveForm == null) return;

            waveForm.SeriesInfo.ConnectionFactory = () => new AdoDataConnection("systemSettings");
            int samplesPerCycle = Transform.CalculateSamplesPerCycle(waveForm.SampleRate, systemFrequency);
            int numCycles = (int)Math.Floor((double)waveForm.DataPoints.Count / samplesPerCycle);
            int numPoints = numCycles * samplesPerCycle;
            double[] points = waveForm.DataPoints.Select(x => x.Value/numPoints).Take(numPoints).ToArray();
            double[] frequencyScale = Fourier.FrequencyScale(points.Length, systemFrequency * samplesPerCycle);

            Complex[] result = FFT(points);

            Dictionary<string, PhasorResult> dict = new Dictionary<string, PhasorResult>();

            for (int i = 0; i < result.Length; i++)
            {
                if (frequencyScale[i] <= 0 || frequencyScale[i] % systemFrequency != 0) continue;

                int harmonic = (int)frequencyScale[i] / (int)systemFrequency;
                if (harmonic <= 0) continue;

                dict.Add($"H{harmonic}", new PhasorResult() { Angle = result[i].Phase * 180 / Math.PI, Magnitude = result[i].Magnitude*2 / Math.Sqrt(2) });
            }

            Snapshots.Add(waveForm.SeriesInfo.Channel, JsonConvert.SerializeObject(dict, Formatting.None));

            //using (StreamWriter sw = new StreamWriter(waveForm.SeriesInfo.Channel.Meter.Name + waveForm.SeriesInfo.Channel.MeasurementType.Name + phase.ToString() + "snapshottest.txt"))
            //{
            //    sw.Write(JsonConvert.SerializeObject(dict, Formatting.Indented));
            //}
        }

        private Complex[] FFT(double[] samples)
        {
            Complex[] complexSamples = samples
                .Select(sample => new Complex(sample, 0))
                .ToArray();

            Fourier.Forward(complexSamples, FourierOptions.NoScaling);

            return complexSamples;
        }


        #endregion
    }
}
