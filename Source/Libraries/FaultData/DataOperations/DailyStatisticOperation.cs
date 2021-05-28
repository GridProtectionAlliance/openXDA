//******************************************************************************************************
//  DailyStatisticOperation.cs - Gbtc
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
using System.Threading;
using FaultData.DataAnalysis;
using FaultData.DataResources;
using FaultData.DataSets;
using GSF.Data;
using GSF.Data.Model;
using log4net;
using openXDA.Model;
using SystemCenter.Model;

namespace FaultData.DataOperations
{
    public class DailyStatisticOperation : DataOperationBase<MeterDataSet>
    {
        private static Mutex s_mutex = new Mutex();
        private static readonly ILog Log = LogManager.GetLogger(typeof(DailyStatisticOperation));

        public override void Execute(MeterDataSet meterDataSet)
        {
            try
            {
                s_mutex.WaitOne();

                using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
                {

                    OpenXDADailyStatistic dailyStatistic = new TableOperations<OpenXDADailyStatistic>(connection).QueryRecordsWhere("Meter = {0}", meterDataSet.Meter.AssetKey).OrderBy(x => x.Date).Last();

                    if (dailyStatistic == null)
                    {

                        dailyStatistic = new OpenXDADailyStatistic();
                        dailyStatistic.ID = 0;
                        dailyStatistic.Date = DateTime.Now.Date.ToString("MM/dd/yyyy");
                        dailyStatistic.Meter = meterDataSet.Meter.AssetKey;

                        dailyStatistic.LastSuccessfulFileProcessed = DateTime.Now;
                        dailyStatistic.LastUnsuccessfulFileProcessed = DateTime.MinValue;
                        dailyStatistic.LastUnsuccessfulFileProcessedExplanation = null;

                        dailyStatistic.TotalFilesProcessed = 1;
                        dailyStatistic.TotalSuccessfulFilesProcessed = 1;
                        dailyStatistic.TotalUnsuccessfulFilesProcessed = 0;
                        dailyStatistic.TotalEmailsSent = 0;

                        dailyStatistic.AverageDownloadLatency = meterDataSet.FileGroup.DataFiles.First().LastWriteTime.Subtract(meterDataSet.FileGroup.DataStartTime).TotalSeconds;
                        dailyStatistic.AverageProcessingStartLatency = meterDataSet.FileGroup.ProcessingStartTime.Subtract(meterDataSet.FileGroup.DataFiles.First().LastWriteTime).TotalSeconds;
                        dailyStatistic.AverageProcessingEndLatency = meterDataSet.FileGroup.ProcessingEndTime.Subtract(meterDataSet.FileGroup.ProcessingStartTime).TotalSeconds;
                        dailyStatistic.AverageTotalProcessingLatency = meterDataSet.FileGroup.ProcessingEndTime.Subtract(meterDataSet.FileGroup.DataStartTime).TotalSeconds;
                        dailyStatistic.AverageTotalEmailLatency = 0;
                        dailyStatistic.AverageEmailLatency = 0;

                    }
                    else if (dailyStatistic.Date != DateTime.Now.Date.ToString("MM/dd/yyyy"))
                    {
                        dailyStatistic.ID = 0;
                        dailyStatistic.Date = DateTime.Now.Date.ToString("MM/dd/yyyy");

                        dailyStatistic.LastSuccessfulFileProcessed = DateTime.Now;

                        dailyStatistic.TotalFilesProcessed = 1;
                        dailyStatistic.TotalSuccessfulFilesProcessed = 1;
                        dailyStatistic.TotalUnsuccessfulFilesProcessed = 0;
                        dailyStatistic.TotalEmailsSent = 0;

                        dailyStatistic.AverageDownloadLatency = meterDataSet.FileGroup.DataFiles.First().LastWriteTime.Subtract(meterDataSet.FileGroup.DataStartTime).TotalSeconds;
                        dailyStatistic.AverageProcessingStartLatency = meterDataSet.FileGroup.ProcessingStartTime.Subtract(meterDataSet.FileGroup.DataFiles.First().LastWriteTime).TotalSeconds;
                        dailyStatistic.AverageProcessingEndLatency = meterDataSet.FileGroup.ProcessingEndTime.Subtract(meterDataSet.FileGroup.ProcessingStartTime).TotalSeconds;
                        dailyStatistic.AverageTotalProcessingLatency = meterDataSet.FileGroup.ProcessingEndTime.Subtract(meterDataSet.FileGroup.DataStartTime).TotalSeconds;
                        dailyStatistic.AverageTotalEmailLatency = 0;
                        dailyStatistic.AverageEmailLatency = 0;
                    }
                    else
                    {
                        dailyStatistic.LastSuccessfulFileProcessed = DateTime.Now;
                        dailyStatistic.TotalUnsuccessfulFilesProcessed += 1;

                        dailyStatistic.AverageDownloadLatency = (dailyStatistic.AverageDownloadLatency * dailyStatistic.TotalFilesProcessed + meterDataSet.FileGroup.DataFiles.First().LastWriteTime.Subtract(meterDataSet.FileGroup.DataStartTime).TotalSeconds) / (dailyStatistic.TotalFilesProcessed + 1);
                        dailyStatistic.AverageProcessingStartLatency = (dailyStatistic.AverageProcessingStartLatency * dailyStatistic.TotalFilesProcessed + meterDataSet.FileGroup.ProcessingStartTime.Subtract(meterDataSet.FileGroup.DataFiles.First().LastWriteTime).TotalSeconds) / (dailyStatistic.TotalFilesProcessed + 1);
                        dailyStatistic.AverageProcessingEndLatency = (dailyStatistic.AverageProcessingEndLatency * dailyStatistic.TotalFilesProcessed + meterDataSet.FileGroup.ProcessingEndTime.Subtract(meterDataSet.FileGroup.ProcessingStartTime).TotalSeconds) / (dailyStatistic.TotalFilesProcessed + 1);
                        dailyStatistic.AverageProcessingStartLatency = (dailyStatistic.AverageProcessingStartLatency * dailyStatistic.TotalFilesProcessed + meterDataSet.FileGroup.ProcessingEndTime.Subtract(meterDataSet.FileGroup.DataStartTime).TotalSeconds) / (dailyStatistic.TotalFilesProcessed + 1);

                        dailyStatistic.TotalFilesProcessed += 1;
                    }

                    new TableOperations<OpenXDADailyStatistic>(connection).AddNewOrUpdateRecord(dailyStatistic);
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Failed to create daily statistic for {meterDataSet.Meter.AssetKey} - {ex.Message}");
            }
            finally
            {
                s_mutex.ReleaseMutex();
            }
        }
    }
}
