//******************************************************************************************************
//  CyclicHistogramOperation.cs - Gbtc
//
//  Copyright © 2023, Grid Protection Alliance.  All Rights Reserved.
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
//  10/05/2023 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using FaultData;
using FaultData.DataOperations;
using FaultData.DataSets;
using GSF.Configuration;
using HIDS;
using log4net;
using openXDA.HIDS.APIExtensions;
using static HIDS.Histogram;

namespace openXDA.HIDS
{
    public class CyclicHistogramOperation : DataOperationBase<MeterDataSet>
    {
        [Category]
        [SettingName(HIDSSettings.CategoryName)]
        public HIDSSettings HIDSSettings { get; } = new HIDSSettings();

        public override void Execute(MeterDataSet meterDataSet)
        {
            if (meterDataSet.CyclicHistogramDataSet is null)
            {
                Log.Debug($"No cyclic histogram data found; skipping {nameof(CyclicHistogramOperation)}.");
                return;
            }

            if (string.IsNullOrEmpty(HIDSSettings.Host))
            {
                Log.Debug($"No HIDS instance defined; skipping {nameof(CyclicHistogramOperation)}.");
                return;
            }

            string channelName = meterDataSet.CyclicHistogramDataSet.Metadata.ChannelLineName;

            Model.Channel channel = meterDataSet.Meter.Channels
                .FirstOrDefault(channel => channel.Name == channelName);

            if (channel is null)
                return;

            TimeZoneConverter timeZoneConverter = new TimeZoneConverter(meterDataSet.Configure);
            Task executeTask = ExecuteAsync(channel, meterDataSet.CyclicHistogramDataSet, timeZoneConverter);
            executeTask.GetAwaiter().GetResult();
        }

        public async Task ExecuteAsync(Model.Channel channel, CyclicHistogramDataSet cyclicHistogramDataSet, TimeZoneConverter timeZoneConverter)
        {
            using API hids = new API();
            await hids.ConfigureAsync(HIDSSettings);

            Histogram histogram = new Histogram();
            LoadMetadata(histogram.Info, cyclicHistogramDataSet);
            histogram.Info.Tag = hids.ToTag(channel.ID);
            histogram.Info.StartTime = timeZoneConverter.ToXDATimeZone(histogram.Info.StartTime);
            histogram.Info.EndTime = timeZoneConverter.ToXDATimeZone(histogram.Info.EndTime);

            LoadData(histogram.CyclicHistogramData, cyclicHistogramDataSet.CyclicHistogram);
            LoadData(histogram.ResidualHistogramData, cyclicHistogramDataSet.ResidualHistogram);
            LoadData(histogram.FrequencyHistogramData, cyclicHistogramDataSet.FrequencyHistogram);
            LoadData(histogram.RMSHistogramData, cyclicHistogramDataSet.RMSHistogram);

            await hids.WriteHistogramAsync(histogram);
        }

        private void LoadMetadata(Metadata destination, CyclicHistogramDataSet source)
        {
            CyclicHistogramDataSet.MetadataSet sourceMetadata = source.Metadata;
            TimeSpan timeZoneOffset = ParseTimeZoneOffset(sourceMetadata.TimeZone);
            destination.FundamentalFrequency = sourceMetadata.FundamentalFrequency;
            destination.SamplingRate = sourceMetadata.SamplingRate;
            destination.StartTime = sourceMetadata.RecordDate + sourceMetadata.StartTime - timeZoneOffset;
            destination.EndTime = sourceMetadata.RecordDate + sourceMetadata.EndTime - timeZoneOffset;
            destination.TotalCapturedCycles = sourceMetadata.TotalCapturedCycles;
            destination.CyclesMax = sourceMetadata.CyclesMax;
            destination.CyclesMin = sourceMetadata.CyclesMin;
            destination.ResidualMax = sourceMetadata.ResidualMax;
            destination.ResidualMin = sourceMetadata.ResidualMin;
            destination.FrequencyMax = sourceMetadata.FrequencyMax;
            destination.FrequencyMin = sourceMetadata.FrequencyMin;
            destination.RMSMax = sourceMetadata.RMSMax;
            destination.RMSMin = sourceMetadata.RMSMin;
            destination.CyclicHistogramBins = sourceMetadata.CyclicHistogramBins;
            destination.ResidualHistogramBins = sourceMetadata.ResidualHistogramBins;
            destination.FrequencyHistogramBins = sourceMetadata.FrequencyHistogramBins;
            destination.RMSHistogramBins = source.RMSHistogramBins;
        }

        private void LoadData(List<Histogram.Point> destination, List<CyclicHistogramDataSet.DataPoint> source)
        {
            foreach (CyclicHistogramDataSet.DataPoint dataPoint in source)
            {
                Histogram.Point point = new Histogram.Point();
                point.Bin = dataPoint.Bin;
                point.Sample = dataPoint.Sample;
                point.Value = dataPoint.Value;
                destination.Add(point);
            }
        }

        private TimeSpan ParseTimeZoneOffset(string timeZoneOffset)
        {
            string trimmedTimeZoneOffset = timeZoneOffset.EndsWith("t")
                ? timeZoneOffset.Substring(0, timeZoneOffset.Length - 1)
                : timeZoneOffset;

            return double.TryParse(trimmedTimeZoneOffset, out double offset)
                ? TimeSpan.FromHours(offset) : TimeSpan.Zero;
        }

        private static readonly ILog Log = LogManager.GetLogger(typeof(CyclicHistogramDataSet));
    }
}
