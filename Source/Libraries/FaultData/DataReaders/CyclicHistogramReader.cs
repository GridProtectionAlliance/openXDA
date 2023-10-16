//******************************************************************************************************
//  CyclicHistogramReader.cs - Gbtc
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
//  09/15/2023 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.IO;
using System.Linq;
using FaultData.DataSets;
using openXDA.Model;

namespace FaultData.DataReaders
{
    public class CyclicHistogramReader : IDataReader
    {
        public bool IsReadyForLoad(FileInfo[] fileList) =>
            fileList.Any(fileInfo => IsMetadataFile(fileInfo.Name)) &&
            fileList.Any(fileInfo => IsDataFile(fileInfo.Name));

        public DataFile GetMetadataFile(FileGroup fileGroup) =>
            fileGroup.DataFiles.FirstOrDefault(dataFile => IsMetadataFile(dataFile.FilePath));

        public DataFile GetPrimaryDataFile(FileGroup fileGroup) =>
            fileGroup.DataFiles.FirstOrDefault(dataFile => IsDataFile(dataFile.FilePath));

        public MeterDataSet Parse(FileGroup fileGroup)
        {
            DataFile metadataFile = GetMetadataFile(fileGroup);
            DataFile dataFile = GetPrimaryDataFile(fileGroup);
            CyclicHistogramDataSet.MetadataSet metadata = ParseMetadata(metadataFile);
            CyclicHistogramDataSet histogramDataSet = ParseHistogramData(metadata, dataFile);
            return CreateMeterDataSet(histogramDataSet);
        }

        private CyclicHistogramDataSet.MetadataSet ParseMetadata(DataFile metadataFile)
        {
            using (MemoryStream metadataStream = new MemoryStream(metadataFile.FileBlob.Blob))
                return CyclicHistogramDataSet.LoadMetadata(metadataStream);
        }

        private CyclicHistogramDataSet ParseHistogramData(CyclicHistogramDataSet.MetadataSet metadata, DataFile dataFile)
        {
            using (MemoryStream metadataStream = new MemoryStream(dataFile.FileBlob.Blob))
                return CyclicHistogramDataSet.LoadAllHistograms(metadata, metadataStream);
        }

        private MeterDataSet CreateMeterDataSet(CyclicHistogramDataSet histogramDataSet) => new MeterDataSet()
        {
            CyclicHistogramDataSet = histogramDataSet,
            Meter = new Meter()
        };

        private bool IsMetadataFile(string filePath) =>
            filePath.EndsWith("MD.csv", StringComparison.OrdinalIgnoreCase);

        private bool IsDataFile(string filePath) =>
            filePath.EndsWith(".bin", StringComparison.OrdinalIgnoreCase);
    }
}
