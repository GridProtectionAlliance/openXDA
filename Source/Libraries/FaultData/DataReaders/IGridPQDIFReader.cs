//******************************************************************************************************
//  IGridPQIFReader.cs - Gbtc
//
//  Copyright © 2017, Grid Protection Alliance.  All Rights Reserved.
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
//  09/04/2017 - Stephen Jenks
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FaultData.DataAnalysis;
using FaultData.DataSets;
using openXDA.Model;

namespace FaultData.DataReaders
{
    public class IGridPQDIFReader : IDataReader
    {
        #region [ Constructors ]

        /// <summary>
        /// Creates a new instance of the <see cref="IGridPQDIFReader"/> class.
        /// </summary>
        public IGridPQDIFReader() =>
            PQDIFReader = new PQDIFReader();

        #endregion

        #region [ Properties ]

        public PQDIFReader PQDIFReader { get; }

        #endregion

        #region [ Methods ]

        public bool IsReadyForLoad(FileInfo[] fileList) =>
            PQDIFReader.IsReadyForLoad(fileList);

        public DataFile GetPrimaryDataFile(FileGroup fileGroup) =>
            PQDIFReader.GetPrimaryDataFile(fileGroup);

        public MeterDataSet Parse(FileGroup fileGroup)
        {
            MeterDataSet meterDataSet = PQDIFReader.Parse(fileGroup);

            List<DataSeries> startSeriesList = meterDataSet.DataSeries
                .Where(dataSeries => dataSeries.SeriesInfo.Channel.Name.ToLower().Contains("start"))
                .ToList();

            List<DataSeries> endSeriesList = meterDataSet.DataSeries
                .Where(dataSeries => dataSeries.SeriesInfo.Channel.Name.ToLower().Contains("end"))
                .ToList();

            string GetBaseChannelName(DataSeries dataSeries) => dataSeries
                .SeriesInfo.Channel.Name
                .ToLower()
                .Replace("start", "")
                .Replace("end", "");

            List<DataSeries> combinedSeriesList = startSeriesList
                .Join(endSeriesList, GetBaseChannelName, GetBaseChannelName, (Start, End) => new { Start, End })
                .Select(mapping =>
                {
                    DataSeries start = mapping.Start;
                    DataSeries end = mapping.End;
                    DataSeries combinedSeries = new DataSeries();
                    combinedSeries.SeriesInfo = start.SeriesInfo;
                    combinedSeries.DataPoints.AddRange(start.DataPoints);
                    combinedSeries.DataPoints.AddRange(end.DataPoints);

                    Channel channel = combinedSeries.SeriesInfo.Channel;
                    int index = channel.Name.IndexOf("start", StringComparison.OrdinalIgnoreCase);
                    channel.Name = channel.Name.Remove(index, 5).Insert(index, "Start + End");

                    return combinedSeries;
                })
                .ToList();

            meterDataSet.DataSeries = meterDataSet.DataSeries
                .Except(startSeriesList)
                .Except(endSeriesList)
                .Union(combinedSeriesList)
                .ToList();

            return meterDataSet;
        }

        #endregion
    }
}
