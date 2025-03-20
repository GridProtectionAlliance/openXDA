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
using System.IO;
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
                    string timeZone = connection.ExecuteScalar<string>("SELECT Value FROM Setting WHERE Name = 'System.XDATimeZone'");
                    DateTime now = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById(timeZone));

                    IEnumerable<OpenXDADailyStatistic> dailyStatistics = new TableOperations<OpenXDADailyStatistic>(connection).QueryRecordsWhere("Meter = {0}", meterDataSet.Meter.AssetKey);

                    if (!dailyStatistics.Any())
                    {

                        OpenXDADailyStatistic dailyStatistic = new OpenXDADailyStatistic();
                        dailyStatistic.ID = 0;
                        dailyStatistic.Date = now.Date.ToString("MM/dd/yyyy");
                        dailyStatistic.Meter = meterDataSet.Meter.AssetKey;

                        dailyStatistic.LastSuccessfulFileProcessed = now;
                        dailyStatistic.LastUnsuccessfulFileProcessed = null;
                        dailyStatistic.LastUnsuccessfulFileProcessedExplanation = null;

                        dailyStatistic.TotalFilesProcessed = 1;
                        dailyStatistic.TotalSuccessfulFilesProcessed = 1;
                        dailyStatistic.TotalUnsuccessfulFilesProcessed = 0;
                        dailyStatistic.TotalEmailsSent = 0;

                        dailyStatistic.AverageDownloadLatency = meterDataSet.FileGroup.DataFiles.First().LastWriteTime.Subtract(meterDataSet.FileGroup.DataStartTime).TotalSeconds;
                        dailyStatistic.AverageProcessingStartLatency = meterDataSet.FileGroup.ProcessingStartTime.Subtract(meterDataSet.FileGroup.DataFiles.First().LastWriteTime).TotalSeconds;
                        dailyStatistic.AverageProcessingEndLatency = now.Subtract(meterDataSet.FileGroup.ProcessingStartTime).TotalSeconds;
                        dailyStatistic.AverageTotalProcessingLatency = now.Subtract(meterDataSet.FileGroup.DataStartTime).TotalSeconds;
                        dailyStatistic.AverageTotalEmailLatency = 0;
                        dailyStatistic.AverageEmailLatency = 0;

                        new TableOperations<OpenXDADailyStatistic>(connection).AddNewOrUpdateRecord(dailyStatistic);

                        return;
                    }
                    else
                    {
                        OpenXDADailyStatistic dailyStatistic = dailyStatistics.OrderBy(x => x.Date).Last();

                        if (dailyStatistic.Date != now.Date.ToString("MM/dd/yyyy"))
                        {
                            dailyStatistic.ID = 0;
                            dailyStatistic.Date = now.Date.ToString("MM/dd/yyyy");

                            dailyStatistic.LastSuccessfulFileProcessed = now;

                            dailyStatistic.TotalFilesProcessed = 1;
                            dailyStatistic.TotalSuccessfulFilesProcessed = 1;
                            dailyStatistic.TotalUnsuccessfulFilesProcessed = 0;
                            dailyStatistic.TotalEmailsSent = 0;

                            dailyStatistic.AverageDownloadLatency = meterDataSet.FileGroup.DataFiles.First().LastWriteTime.Subtract(meterDataSet.FileGroup.DataStartTime).TotalSeconds;
                            dailyStatistic.AverageProcessingStartLatency = meterDataSet.FileGroup.ProcessingStartTime.Subtract(meterDataSet.FileGroup.DataFiles.First().LastWriteTime).TotalSeconds;
                            dailyStatistic.AverageProcessingEndLatency = now.Subtract(meterDataSet.FileGroup.ProcessingStartTime).TotalSeconds;
                            dailyStatistic.AverageTotalProcessingLatency = now.Subtract(meterDataSet.FileGroup.DataStartTime).TotalSeconds;
                            dailyStatistic.AverageTotalEmailLatency = 0;
                            dailyStatistic.AverageEmailLatency = 0;
                        }
                        else
                        {
                            dailyStatistic.LastSuccessfulFileProcessed = now;
                            dailyStatistic.TotalSuccessfulFilesProcessed += 1;

                            dailyStatistic.AverageDownloadLatency = (dailyStatistic.AverageDownloadLatency * dailyStatistic.TotalFilesProcessed + meterDataSet.FileGroup.DataFiles.First().LastWriteTime.Subtract(meterDataSet.FileGroup.DataStartTime).TotalSeconds) / (dailyStatistic.TotalFilesProcessed + 1);
                            dailyStatistic.AverageProcessingStartLatency = (dailyStatistic.AverageProcessingStartLatency * dailyStatistic.TotalFilesProcessed + meterDataSet.FileGroup.ProcessingStartTime.Subtract(meterDataSet.FileGroup.DataFiles.First().LastWriteTime).TotalSeconds) / (dailyStatistic.TotalFilesProcessed + 1);
                            dailyStatistic.AverageProcessingEndLatency = (dailyStatistic.AverageProcessingEndLatency * dailyStatistic.TotalFilesProcessed + now.Subtract(meterDataSet.FileGroup.ProcessingStartTime).TotalSeconds) / (dailyStatistic.TotalFilesProcessed + 1);
                            dailyStatistic.AverageTotalProcessingLatency = (dailyStatistic.AverageTotalProcessingLatency * dailyStatistic.TotalFilesProcessed + now.Subtract(meterDataSet.FileGroup.DataStartTime).TotalSeconds) / (dailyStatistic.TotalFilesProcessed + 1);

                            dailyStatistic.TotalFilesProcessed += 1;
                        }

                        new TableOperations<OpenXDADailyStatistic>(connection).AddNewOrUpdateRecord(dailyStatistic);

                        return;

                    }
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

        public static void UpdateEmailProcessingStatistic(List<int> eventIDs)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                foreach (int eventID in eventIDs)
                {
                    Event xdaEvent = new TableOperations<Event>(connection).QueryRecordWhere("ID = {0}", eventID);
                    Meter meter = new TableOperations<Meter>(connection).QueryRecordWhere("ID = {0}", xdaEvent.MeterID);
                    FileGroup fileGroup = new TableOperations<FileGroup>(connection).QueryRecordWhere("ID = {0}", xdaEvent.FileGroupID);
                    fileGroup.DataFiles = new TableOperations<DataFile>(connection).QueryRecordsWhere("FileGroupID = {0}", xdaEvent.FileGroupID).ToList();
                    UpdateEmailProcessingStatistic(meter.AssetKey, fileGroup);
                }
            }
        }

        private static void UpdateEmailProcessingStatistic(string assetKey, FileGroup fileGroup)
        {
            try
            {
                s_mutex.WaitOne();

                using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
                {

                    string timeZone = connection.ExecuteScalar<string>("SELECT Value FROM Setting WHERE Name = 'System.XDATimeZone'");
                    DateTime now = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById(timeZone));

                    IEnumerable<OpenXDADailyStatistic> dailyStatistics = new TableOperations<OpenXDADailyStatistic>(connection).QueryRecordsWhere("Meter = {0}", assetKey);

                    if (!dailyStatistics.Any())
                    {

                        OpenXDADailyStatistic dailyStatistic = new OpenXDADailyStatistic();
                        dailyStatistic.ID = 0;
                        dailyStatistic.Date = now.Date.ToString("MM/dd/yyyy");
                        dailyStatistic.Meter = assetKey;

                        dailyStatistic.LastSuccessfulFileProcessed = null;
                        dailyStatistic.LastUnsuccessfulFileProcessed = null;
                        dailyStatistic.LastUnsuccessfulFileProcessedExplanation = null;

                        dailyStatistic.TotalFilesProcessed = 0;
                        dailyStatistic.TotalSuccessfulFilesProcessed = 0;
                        dailyStatistic.TotalUnsuccessfulFilesProcessed = 0;
                        dailyStatistic.TotalEmailsSent = 0;

                        dailyStatistic.AverageDownloadLatency = 0;
                        dailyStatistic.AverageProcessingStartLatency = 0;
                        dailyStatistic.AverageProcessingEndLatency = 0;
                        dailyStatistic.AverageTotalProcessingLatency = 0;
                        dailyStatistic.AverageEmailLatency = now.Subtract(fileGroup.ProcessingEndTime).TotalSeconds;
                        dailyStatistic.AverageTotalEmailLatency = now.Subtract(fileGroup.DataStartTime).TotalSeconds;
                        dailyStatistic.BadDays = new TableOperations<OpenXDADailyStatistic>(connection).QueryRecords("[DATE] DESC", new RecordRestriction("Meter = {0}", assetKey)).FirstOrDefault()?.BadDays ?? 0;
                        new TableOperations<OpenXDADailyStatistic>(connection).AddNewOrUpdateRecord(dailyStatistic);

                        return;
                    }
                    else
                    {
                        OpenXDADailyStatistic dailyStatistic = dailyStatistics.OrderBy(x => x.Date).Last();


                        if (dailyStatistic.Date != now.Date.ToString("MM/dd/yyyy"))
                        {
                            dailyStatistic.ID = 0;
                            dailyStatistic.Date = now.Date.ToString("MM/dd/yyyy");

                            dailyStatistic.LastSuccessfulFileProcessed = null;

                            dailyStatistic.TotalFilesProcessed = 0;
                            dailyStatistic.TotalSuccessfulFilesProcessed = 0;
                            dailyStatistic.TotalUnsuccessfulFilesProcessed = 0;
                            dailyStatistic.TotalEmailsSent = 0;

                            dailyStatistic.AverageDownloadLatency = 0;
                            dailyStatistic.AverageProcessingStartLatency = 0;
                            dailyStatistic.AverageProcessingEndLatency = 0;
                            dailyStatistic.AverageTotalProcessingLatency = 0;
                            dailyStatistic.AverageEmailLatency = now.Subtract(fileGroup.ProcessingEndTime).TotalSeconds;
                            dailyStatistic.AverageTotalEmailLatency = now.Subtract(fileGroup.DataStartTime).TotalSeconds;


                        }
                        else
                        {
                            dailyStatistic.AverageTotalEmailLatency = (dailyStatistic.AverageTotalEmailLatency * dailyStatistic.TotalEmailsSent + DateTime.Now.Subtract(fileGroup.DataStartTime).TotalSeconds) / (dailyStatistic.TotalEmailsSent + 1);
                            dailyStatistic.TotalEmailsSent += 1;
                        }

                        MeterDataQualitySummary trendingSummary = new TableOperations<MeterDataQualitySummary>(connection).QueryRecordWhere("[Date] = {0} AND MeterID = (SELECT ID FROM Meter WHERE AssetKey = {1})", dailyStatistic.Date, assetKey);
                        int warningLevel = int.Parse(new TableOperations<SystemCenter.Model.Setting>(connection).QueryRecordWhere("Name = 'OpenXDA.WarningLevel'")?.Value ?? "50");
                        int errorLevel = int.Parse(new TableOperations<SystemCenter.Model.Setting>(connection).QueryRecordWhere("Name = 'OpenXDA.ErrorLevel'")?.Value ?? "100");

                        if (trendingSummary == null) trendingSummary = new MeterDataQualitySummary() { ExpectedPoints = 0, GoodPoints = 0, LatchedPoints = 0, UnreasonablePoints = 0, NoncongruentPoints = 0 };

                        if (dailyStatistic.Status == "Error") { 
                            Log.Warn($"Failed to update daily statistic for {dailyStatistic.Meter} - a previous error was present");

                        } // already an error, do nothing
                        else if (dailyStatistic.TotalUnsuccessfulFilesProcessed > errorLevel) {
                            dailyStatistic.Status = "Error";
                            dailyStatistic.BadDays++;
                        }
                        else if ((trendingSummary.ExpectedPoints - trendingSummary.GoodPoints) > errorLevel)
                        {
                            dailyStatistic.Status = "Error";
                            dailyStatistic.BadDays++;
                            dailyStatistic.LastUnsuccessfulFileProcessed = now;
                            dailyStatistic.LastUnsuccessfulFileProcessedExplanation = $"Expected {trendingSummary.ExpectedPoints} points but only have {trendingSummary.GoodPoints} good data points.  Exceeds error threshold of {errorLevel}.";
                        }
                        else if (trendingSummary.LatchedPoints > errorLevel)
                        {
                            dailyStatistic.Status = "Error";
                            dailyStatistic.BadDays++;
                            dailyStatistic.LastUnsuccessfulFileProcessed = now;
                            dailyStatistic.LastUnsuccessfulFileProcessedExplanation = $"Expected {trendingSummary.ExpectedPoints} points but have {trendingSummary.LatchedPoints} latched data points.  Exceeds error threshold of {errorLevel}.";

                        }
                        else if (trendingSummary.UnreasonablePoints > errorLevel)
                        {
                            dailyStatistic.Status = "Error";
                            dailyStatistic.BadDays++;
                            dailyStatistic.LastUnsuccessfulFileProcessed = now;
                            dailyStatistic.LastUnsuccessfulFileProcessedExplanation = $"Expected {trendingSummary.ExpectedPoints} points but have {trendingSummary.UnreasonablePoints} unreasonable data points.  Exceeds error threshold of {errorLevel}.";

                        }
                        else if (trendingSummary.NoncongruentPoints > errorLevel)
                        {
                            dailyStatistic.Status = "Error";
                            dailyStatistic.BadDays++;
                            dailyStatistic.LastUnsuccessfulFileProcessed = now;
                            dailyStatistic.LastUnsuccessfulFileProcessedExplanation = $"Expected {trendingSummary.ExpectedPoints} points but only have {trendingSummary.NoncongruentPoints} non-congruent data points.  Exceeds error threshold of {errorLevel}.";

                        }
                        else if (dailyStatistic.Status == "Warning") {
                            Log.Warn($"Failed to update daily statistic for {dailyStatistic.Meter} - a previous warning was present");
                         } // already a warning, do nothing
                        else if (dailyStatistic.TotalUnsuccessfulFilesProcessed > warningLevel)
                        {
                            dailyStatistic.Status = "Warning";
                        }
                        else if ((trendingSummary.ExpectedPoints - trendingSummary.GoodPoints) > warningLevel)
                        {
                            dailyStatistic.Status = "Warning";
                            dailyStatistic.LastUnsuccessfulFileProcessed = now;
                            dailyStatistic.LastUnsuccessfulFileProcessedExplanation = $"Expected {trendingSummary.ExpectedPoints} points but only have {trendingSummary.GoodPoints} good data points.  Exceeds warning threshold of {warningLevel}.";
                        }
                        else if (trendingSummary.LatchedPoints > warningLevel)
                        {
                            dailyStatistic.Status = "Warning";
                            dailyStatistic.LastUnsuccessfulFileProcessed = now;
                            dailyStatistic.LastUnsuccessfulFileProcessedExplanation = $"Expected {trendingSummary.ExpectedPoints} points but have {trendingSummary.LatchedPoints} latched data points.  Exceeds warning threshold of {warningLevel}.";

                        }
                        else if (trendingSummary.UnreasonablePoints > warningLevel)
                        {
                            dailyStatistic.Status = "Warning";
                            dailyStatistic.LastUnsuccessfulFileProcessed = now;
                            dailyStatistic.LastUnsuccessfulFileProcessedExplanation = $"Expected {trendingSummary.ExpectedPoints} points but have {trendingSummary.UnreasonablePoints} unreasonable data points. Exceeds warning error threshold of {warningLevel}.";

                        }
                        else if (trendingSummary.NoncongruentPoints > warningLevel)
                        {
                            dailyStatistic.Status = "Warning";
                            dailyStatistic.LastUnsuccessfulFileProcessed = now;
                            dailyStatistic.LastUnsuccessfulFileProcessedExplanation = $"Expected {trendingSummary.ExpectedPoints} points but only have {trendingSummary.NoncongruentPoints} non-congruent data points. Exceeds warning error threshold of {warningLevel}.";

                        }

                        new TableOperations<OpenXDADailyStatistic>(connection).AddNewOrUpdateRecord(dailyStatistic);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Failed to create daily statistic for {assetKey} - {ex.Message}");
            }
            finally
            {
                s_mutex.ReleaseMutex();
            }

        }

        public static void UpdateFileProcessingStatistic(string assetKey, FileGroup fileGroup, string message)
        {
            try
            {
                s_mutex.WaitOne();

                using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
                {
                    string timeZone = connection.ExecuteScalar<string>("SELECT Value FROM Setting WHERE Name = 'System.XDATimeZone'");
                    DateTime now = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById(timeZone));

                    IEnumerable<OpenXDADailyStatistic> dailyStatistics = new TableOperations<OpenXDADailyStatistic>(connection).QueryRecordsWhere("Meter = {0}", assetKey);

                    if (!dailyStatistics.Any())
                    {

                        OpenXDADailyStatistic dailyStatistic = new OpenXDADailyStatistic();
                        dailyStatistic.ID = 0;
                        dailyStatistic.Date = now.Date.ToString("MM/dd/yyyy");
                        dailyStatistic.Meter = assetKey;

                        dailyStatistic.LastSuccessfulFileProcessed = null;
                        dailyStatistic.LastUnsuccessfulFileProcessed = now;
                        dailyStatistic.LastUnsuccessfulFileProcessedExplanation = message;

                        dailyStatistic.TotalFilesProcessed = 1;
                        dailyStatistic.TotalSuccessfulFilesProcessed = 0;
                        dailyStatistic.TotalUnsuccessfulFilesProcessed = 1;
                        dailyStatistic.TotalEmailsSent = 0;

                        dailyStatistic.AverageDownloadLatency = fileGroup.DataFiles.First().LastWriteTime.Subtract(fileGroup.DataStartTime).TotalSeconds;
                        dailyStatistic.AverageProcessingStartLatency = fileGroup.ProcessingStartTime.Subtract(fileGroup.DataFiles.First().LastWriteTime).TotalSeconds;
                        dailyStatistic.AverageProcessingEndLatency = 0;
                        dailyStatistic.AverageEmailLatency = 0;
                        dailyStatistic.AverageTotalProcessingLatency = 0;
                        dailyStatistic.AverageTotalEmailLatency = 0;
                        new TableOperations<OpenXDADailyStatistic>(connection).AddNewOrUpdateRecord(dailyStatistic);
                        return;
                    }
                    else
                    {
                        OpenXDADailyStatistic dailyStatistic = dailyStatistics.OrderBy(x => x.Date).Last();

                        if (dailyStatistic.Date != now.Date.ToString("MM/dd/yyyy"))
                        {
                            dailyStatistic.ID = 0;
                            dailyStatistic.Date = now.Date.ToString("MM/dd/yyyy");

                            dailyStatistic.LastUnsuccessfulFileProcessed = now;
                            dailyStatistic.LastUnsuccessfulFileProcessedExplanation = message;

                            dailyStatistic.TotalFilesProcessed = 1;
                            dailyStatistic.TotalSuccessfulFilesProcessed = 0;
                            dailyStatistic.TotalUnsuccessfulFilesProcessed = 1;
                            dailyStatistic.TotalEmailsSent = 0;

                            dailyStatistic.AverageDownloadLatency = fileGroup.DataFiles.First().LastWriteTime.Subtract(fileGroup.DataStartTime).TotalSeconds;
                            dailyStatistic.AverageProcessingStartLatency = fileGroup.ProcessingStartTime.Subtract(fileGroup.DataFiles.First().LastWriteTime).TotalSeconds;
                            dailyStatistic.AverageProcessingEndLatency = 0;
                            dailyStatistic.AverageTotalProcessingLatency = 0;
                            dailyStatistic.AverageTotalEmailLatency = 0;
                            dailyStatistic.AverageEmailLatency = 0;

                        }
                        else
                        {
                            dailyStatistic.LastUnsuccessfulFileProcessed = now;
                            dailyStatistic.LastUnsuccessfulFileProcessedExplanation = message;
                            dailyStatistic.TotalUnsuccessfulFilesProcessed += 1;

                            dailyStatistic.AverageDownloadLatency = (dailyStatistic.AverageDownloadLatency * dailyStatistic.TotalFilesProcessed + fileGroup.DataFiles.First().LastWriteTime.Subtract(fileGroup.DataStartTime).TotalSeconds) / (dailyStatistic.TotalFilesProcessed + 1);
                            dailyStatistic.AverageProcessingStartLatency = (dailyStatistic.AverageProcessingStartLatency * dailyStatistic.TotalFilesProcessed + fileGroup.ProcessingStartTime.Subtract(fileGroup.DataFiles.First().LastWriteTime).TotalSeconds) / (dailyStatistic.TotalFilesProcessed + 1);

                            dailyStatistic.TotalFilesProcessed += 1;
                        }

                        new TableOperations<OpenXDADailyStatistic>(connection).AddNewOrUpdateRecord(dailyStatistic);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Failed to create daily statistic for {assetKey} - {ex.Message}");
            }
            finally
            {
                s_mutex.ReleaseMutex();
            }


        }

    }
}
