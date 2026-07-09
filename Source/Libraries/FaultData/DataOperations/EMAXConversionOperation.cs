//******************************************************************************************************
//  EMAXConversionOperation.cs - Gbtc
//
//  Copyright © 2026, Grid Protection Alliance.  All Rights Reserved.
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
//  06/09/2026 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using FaultData.DataAnalysis;
using FaultData.DataSets;
using GSF;
using GSF.COMTRADE;
using GSF.Configuration;
using GSF.EMAX;
using openXDA.Configuration;
using openXDA.Model;

namespace FaultData.DataOperations
{
    public class EMAXConversionOperation : DataOperationBase<MeterDataSet>
    {
        [Category]
        [SettingName(EMAXSection.CategoryName)]
        public EMAXSection EMAXSettings { get; }
            = new EMAXSection();

        private string ExportDirectory =>
            EMAXSettings.COMTRADEExportDirectory;

        public override void Execute(MeterDataSet meterDataSet)
        {
            if (string.IsNullOrEmpty(ExportDirectory))
                return;

            EMAXConversionDataSet emaxConversionDataSet = meterDataSet.EMAXConversionDataSet;

            if (emaxConversionDataSet is null)
                return;

            ControlFile controlFile = emaxConversionDataSet.ControlFile;
            List<DataSeries> analogData = emaxConversionDataSet.AnalogData;
            List<DataSeries> digitalData = emaxConversionDataSet.DigitalData;

            string rootFileName = GSF.IO.FilePath.GetFileNameWithoutExtension(controlFile.FileName);
            string directoryPath = GetExportDirectory(meterDataSet.Meter);
            string schemaFilePath = Path.Combine(directoryPath, $"{rootFileName}.cfg");
            string dataFilePath = Path.Combine(directoryPath, $"{rootFileName}.dat");

            if (File.Exists(dataFilePath))
                return;

            List<ANLG_CHNL_NEW> analogChannels = [.. controlFile.AnalogChannelSettings
                .OrderBy(kvp => kvp.Key)
                .Select(kvp => kvp.Value)];

            List<EVNT_CHNL_NEW> digitalChannels = [.. controlFile.EventChannelSettings
                .OrderBy(kvp => kvp.Key)
                .Select(kvp => kvp.Value)];

            Schema comtradeSchema = CreateSchema(controlFile, analogData);
            PopulateAnalogs(comtradeSchema, analogChannels, analogData);
            PopulateDigitals(comtradeSchema, digitalChannels);

            Directory.CreateDirectory(directoryPath);
            File.WriteAllText(schemaFilePath, comtradeSchema.FileImage, Encoding.ASCII);

            const int DigitalSize = sizeof(ushort) * 8;
            using FileStream stream = File.OpenWrite(dataFilePath);

            IEnumerable<DataSeries> digitalWords = digitalData.Skip(1)
                .Select((dataSeries, index) => dataSeries.Multiply(Math.Pow(2.0D, DigitalSize - (index % DigitalSize) - 1)))
                .Select((DataSeries, Index) => new { DataSeries, Index })
                .GroupBy(obj => obj.Index / DigitalSize)
                .Select(grouping => grouping.Select(obj => obj.DataSeries))
                .Select(grouping => grouping.Aggregate((sum, series) => sum.Add(series)));

            List<DataSeries> allChannels = [.. analogData.Skip(1)
                .Select((dataSeries, index) => analogChannels[index].type.All(char.IsUpper) ? dataSeries.Multiply(0.001D) : dataSeries)
                .Concat(digitalWords)];

            for (int i = 0; i < analogData[1].DataPoints.Count; i++)
            {
                DateTime timestamp = analogData[1][i].Time;
                double[] values = [.. allChannels.Select(dataSeries => dataSeries[i].Value)];
                Writer.WriteNextRecordBinary(stream, comtradeSchema, timestamp, values, (uint)i, false);
            }
        }

        private Schema CreateSchema(ControlFile controlFile, List<DataSeries> analogData)
        {
            Schema comtradeSchema = new();

            comtradeSchema.StationName = Regex.Replace(controlFile.IdentityString.value, @"[\r\n]", "");
            comtradeSchema.Version = 2013;
            comtradeSchema.AnalogChannels = new AnalogChannel[controlFile.AnalogChannelCount];
            comtradeSchema.DigitalChannels = new DigitalChannel[controlFile.EventChannelSettings.Count];
            comtradeSchema.SampleRates = new SampleRate[1];
            comtradeSchema.SampleRates[0].Rate = controlFile.SystemParameters.samples_per_second;
            comtradeSchema.SampleRates[0].EndSample = controlFile.SystemParameters.rcd_sample_count - 1;
            comtradeSchema.StartTime = new Timestamp() { Value = analogData[1][0].Time };

            int triggerIndex = controlFile.SystemParameters.start_offset_samples + controlFile.SystemParameters.prefault_samples;
            comtradeSchema.TriggerTime = new Timestamp() { Value = analogData[1][triggerIndex].Time };

            return comtradeSchema;
        }

        private void PopulateAnalogs(Schema comtradeSchema, List<ANLG_CHNL_NEW> analogChannels, List<DataSeries> analogData)
        {
            for (int i = 0; i < analogChannels.Count; i++)
            {
                ANLG_CHNL_NEW analogChannel = analogChannels[i];
                AnalogChannel comtradeAnalog = new();
                DataSeries channelData = analogData[i + 1];

                double unitMultiplier = 1.0D;
                double max = channelData.Maximum;
                double min = channelData.Minimum;

                comtradeAnalog.Index = i + 1;
                comtradeAnalog.CircuitComponent = analogChannel.title;
                comtradeAnalog.Name = "A" + (i + 1);

                comtradeAnalog.Units = analogChannel.type switch
                {
                    "V" => "kVAC",
                    "A" => "kAAC",
                    "v" => " VDC",
                    string type => type,
                };

                if (analogChannel.type.All(char.IsUpper))
                {
                    unitMultiplier = 0.001D;
                    max *= unitMultiplier;
                    min *= unitMultiplier;
                }

                comtradeAnalog.Multiplier = (max - min) / (2 * short.MaxValue);
                comtradeAnalog.Adder = (max + min) / 2.0D;

                // Setting Secondary to 1 and adjusting primary accordingly (for human readability)
                double basePrimary = double.TryParse(analogChannel.primary, out double num) ? num * unitMultiplier : 0.0D;
                double inverseSecondaryMultiplier = double.TryParse(analogChannel.secondary, out num) ? num * unitMultiplier : 0.0D;

                if (inverseSecondaryMultiplier != 0.0D)
                {
                    comtradeAnalog.PrimaryRatio = basePrimary / inverseSecondaryMultiplier;
                    comtradeAnalog.SecondaryRatio = 1.0D;
                }
                else
                {
                    comtradeAnalog.PrimaryRatio = basePrimary;
                    comtradeAnalog.SecondaryRatio = 0.0D;
                }

                comtradeSchema.AnalogChannels[i] = comtradeAnalog;
            }
        }

        private void PopulateDigitals(Schema comtradeSchema, List<EVNT_CHNL_NEW> digitalChannels)
        {
            for (int i = 0; i < digitalChannels.Count; i++)
            {
                EVNT_CHNL_NEW digitalChannel = digitalChannels[i];
                DigitalChannel comtradeDigital = new();
                comtradeDigital.Index = i + 1;
                comtradeDigital.CircuitComponent = digitalChannel.e_title;
                comtradeDigital.ChannelName = "E" + (i + 1);
                comtradeDigital.PhaseID = "?";
                comtradeSchema.DigitalChannels[i] = comtradeDigital;
            }
        }

        private string GetExportDirectory(Meter meter)
        {
            var templateData = new
            {
                meter.Location.LocationKey,
                LocationName = meter.Location.Name,
                LocationShortName = meter.Location.ShortName,

                meter.AssetKey,
                meter.Name,
                meter.Make,
                meter.Model,
                meter.Alias,
                meter.ShortName
            };

            return ExportDirectory.Interpolate(templateData);
        }
    }
}
