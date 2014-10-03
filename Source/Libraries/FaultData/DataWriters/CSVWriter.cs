//******************************************************************************************************
//  CSVWriter.cs - Gbtc
//
//  Copyright © 2014, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the Eclipse Public License -v 1.0 (the "License"); you may
//  not use this file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://www.opensource.org/licenses/eclipse-1.0.php
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  08/07/2014 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FaultData.DataAnalysis;
using FaultData.Database;
using FaultData.Database.FaultLocationDataTableAdapters;
using FaultData.Database.MeterDataTableAdapters;
using GSF.COMTRADE;
using GSF.IO;

namespace FaultData.DataWriters
{
    public class CSVWriter
    {
        public static void Write(string connectionString, int eventID, string originalFilePath, string filePath)
        {
            MeterInfoDataContext meterInfo = new MeterInfoDataContext(connectionString);
            FaultLocationInfoDataContext faultLocationInfo = new FaultLocationInfoDataContext(connectionString);
            MeterData.EventRow eventRow;

            Meter meter;
            DataGroup waveFormData;
            FaultLocationData.FaultCurveDataTable faultCurveTable;
            List<FaultSegment> segments;

            using (EventTableAdapter eventAdapter = new EventTableAdapter())
            {
                eventAdapter.Connection.ConnectionString = connectionString;
                eventRow = eventAdapter.GetDataByID(eventID).FirstOrDefault();
            }

            if ((object)eventRow == null)
                throw new InvalidOperationException(string.Format("Event with ID {0} not found", eventID));

            meter = meterInfo.Meters.FirstOrDefault(dbMeter => dbMeter.ID == eventRow.MeterID);

            waveFormData = new DataGroup();
            waveFormData.FromData(meter, eventRow.Data);

            using (FaultCurveTableAdapter faultCurveAdapter = new FaultCurveTableAdapter())
            {
                faultCurveAdapter.Connection.ConnectionString = connectionString;
                faultCurveTable = faultCurveAdapter.GetDataBy(eventID);
            }

            segments = faultLocationInfo.FaultSegments
                .Where(segment => segment.EventID == eventID)
                .OrderBy(segment => segment.StartSample)
                .ToList();

            Write(meter, waveFormData, faultCurveTable, segments, originalFilePath, filePath);
        }

        public static void Write(Meter meter, DataGroup waveFormData, FaultLocationData.FaultCurveDataTable faultCurveTable, List<FaultSegment> segments, string originalFilePath, string filePath)
        {
            List<DataSeries> waveFormSeriesList = GetWaveFormSeriesList(waveFormData);
            DataGroup faultLocationData = GetFaultLocationData(meter, faultCurveTable);

            string absoluteFilePath = FilePath.GetAbsolutePath(filePath);

            using (StreamWriter fileStream = new StreamWriter(File.OpenWrite(absoluteFilePath)))
            {
                string originalDirectory;
                string originalRootFileName;
                string originalSchemaFilePath;
                string absoluteOriginalFilePath;
                Schema originalSchema = null;

                string headerRow;

                absoluteOriginalFilePath = FilePath.GetAbsolutePath(originalFilePath);

                if (File.Exists(absoluteOriginalFilePath))
                {
                    originalDirectory = FilePath.GetDirectoryName(absoluteOriginalFilePath);
                    originalRootFileName = FilePath.GetFileNameWithoutExtension(originalFilePath);
                    originalSchemaFilePath = Path.Combine(originalDirectory, originalRootFileName + ".cfg");
                    originalSchema = new Schema(originalSchemaFilePath);
                }

                headerRow = waveFormData.DataSeries
                    .Select(series => GetOriginalChannelName(originalSchema, series))
                    .Concat(faultCurveTable.Select(row => string.Format("Fault Location ({0} Algorithm)", row.Algorithm)))
                    .Aggregate("Time", (s, s1) => s + "," + s1);

                fileStream.WriteLine(headerRow);

                for (int i = 0; i < waveFormData.Samples; i++)
                {
                    DateTime time = waveFormSeriesList[0].DataPoints[i].Time;

                    double[] values = waveFormSeriesList
                        .Select(series => series.DataPoints[i].Value)
                        .Concat(faultLocationData.DataSeries.Select(series => series.DataPoints.Count > i ? series.DataPoints[i].Value : 0.0D))
                        .ToArray();

                    fileStream.WriteLine(values.Aggregate(time.ToString("yyyy-MM-dd HH:mm:ss.ffffff"), (s, d) => s + "," + d));
                }
            }
        }

        private static List<DataSeries> GetWaveFormSeriesList(DataGroup waveFormData)
        {
            return GetWaveFormSeriesList(GetSeriesLookup(waveFormData));
        }

        private static List<DataSeries> GetWaveFormSeriesList(Dictionary<string, DataSeries> seriesLookup)
        {
            List<DataSeries> waveFormSeriesList = new List<DataSeries>()
            {
                seriesLookup["VA"],
                seriesLookup["VB"],
                seriesLookup["VC"],
                seriesLookup["IA"],
                seriesLookup["IB"],
                seriesLookup["IC"]
            };

            return waveFormSeriesList;
        }

        private static Dictionary<string, DataSeries> GetSeriesLookup(DataGroup waveFormData)
        {
            string[] measurementTypes = { "Voltage", "Current" };
            string[] phases = { "AN", "BN", "CN" };

            Func<DataSeries, string> typeSelector = series =>
            {
                string measurementType = series.SeriesInfo.Channel.MeasurementType.Name;
                string phase = series.SeriesInfo.Channel.Phase.Name;

                if (measurementType == "Current")
                    measurementType = "I";

                return string.Format("{0}{1}", measurementType[0], phase[0]);
            };

            return waveFormData.DataSeries
                .Where(series => series.SeriesInfo.SeriesType.Name == "Values")
                .Where(series => series.SeriesInfo.Channel.MeasurementCharacteristic.Name == "Instantaneous")
                .Where(series => measurementTypes.Contains(series.SeriesInfo.Channel.MeasurementType.Name))
                .Where(series => phases.Contains(series.SeriesInfo.Channel.Phase.Name))
                .ToDictionary(typeSelector);
        }

        private static string GetOriginalChannelName(Schema originalSchema, DataSeries series)
        {
            int index;

            return ((object)originalSchema != null && int.TryParse(series.SeriesInfo.SourceIndexes, out index))
                ? originalSchema.AnalogChannels[Math.Abs(index) - 1].Name
                : series.SeriesInfo.Channel.Name;
        }

        private static DataGroup GetFaultLocationData(Meter meter, FaultLocationData.FaultCurveDataTable faultCurveTable)
        {
            DataGroup faultLocationData = new DataGroup();
            DataGroup parsedGroup = new DataGroup();

            foreach (FaultLocationData.FaultCurveRow faultCurveRow in faultCurveTable)
            {
                parsedGroup.FromData(meter, faultCurveRow.Data);

                foreach (DataSeries series in parsedGroup.DataSeries)
                    faultLocationData.Add(series);
            }

            return faultLocationData;
        }
    }
}
