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
using System.Linq;
using System.Threading;
using FaultData.DataSets;
using GSF.Data;
using GSF.Data.Model;
using log4net;
using openXDA.Model;
using SystemCenter.Model;

namespace FaultData.DataOperations
{
    public class DailyStatisticOperation
    {
        /// <summary>
        /// Update the <see cref="OpenXDADailyStatistic"/> for a list of <see cref="Event"/>s that have been included in an email.
        /// </summary>
        /// <param name="eventIDs">The IDs of the events that were included in the email</param>
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

                OpenXDADailyStatistic dailyStatistic = GetDailyStatistic(assetKey, out DateTime now);

                if (dailyStatistic.AverageTotalEmailLatency > 0)
                    dailyStatistic.AverageTotalEmailLatency = (dailyStatistic.AverageTotalEmailLatency * dailyStatistic.TotalEmailsSent + DateTime.Now.Subtract(fileGroup.DataStartTime).TotalSeconds) / (dailyStatistic.TotalEmailsSent + 1);
                else
                    dailyStatistic.AverageTotalEmailLatency = DateTime.Now.Subtract(fileGroup.DataStartTime).TotalSeconds;

                if (dailyStatistic.AverageEmailLatency > 0)
                    dailyStatistic.AverageEmailLatency = (dailyStatistic.AverageEmailLatency * dailyStatistic.TotalEmailsSent + now.Subtract(fileGroup.ProcessingEndTime).TotalSeconds) / (dailyStatistic.TotalEmailsSent + 1);
                else
                    dailyStatistic.AverageEmailLatency = now.Subtract(fileGroup.ProcessingEndTime).TotalSeconds;

                dailyStatistic.TotalEmailsSent += 1;
                using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
                {
                    new TableOperations<OpenXDADailyStatistic>(connection).AddNewOrUpdateRecord(dailyStatistic);
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

        /// <summary>
        /// Updated the <see cref="OpenXDADailyStatistic"/> for a file group that failed to process.
        /// </summary>
        /// <param name="assetKey">The asset key for the meter that produced the file group</param>
        /// <param name="fileGroup">The file group that failed to process</param>
        /// <param name="message">The error message that provides context for the failure</param>
        public static void UpdateFailureFileProcessingStatistic(string assetKey, FileGroup fileGroup, string message)
        {
            try
            {
                s_mutex.WaitOne();

                OpenXDADailyStatistic dailyStatistic = GetDailyStatistic(assetKey, out DateTime now);

                dailyStatistic.LastUnsuccessfulFileProcessed = now;
                dailyStatistic.LastUnsuccessfulFileProcessedExplanation = message;
                dailyStatistic.TotalUnsuccessfulFilesProcessed += 1;

                if (dailyStatistic.AverageDownloadLatency > 0)
                    dailyStatistic.AverageDownloadLatency = (dailyStatistic.AverageDownloadLatency * dailyStatistic.TotalFilesProcessed + fileGroup.DataFiles.First().LastWriteTime.Subtract(fileGroup.DataStartTime).TotalSeconds) / (dailyStatistic.TotalFilesProcessed + 1);
                else
                    dailyStatistic.AverageDownloadLatency = fileGroup.DataFiles.First().LastWriteTime.Subtract(fileGroup.DataStartTime).TotalSeconds;

                if (dailyStatistic.AverageProcessingStartLatency > 0)
                    dailyStatistic.AverageProcessingStartLatency = (dailyStatistic.AverageProcessingStartLatency * dailyStatistic.TotalFilesProcessed + fileGroup.ProcessingStartTime.Subtract(fileGroup.DataFiles.First().LastWriteTime).TotalSeconds) / (dailyStatistic.TotalFilesProcessed + 1);
                else
                    dailyStatistic.AverageProcessingStartLatency = fileGroup.ProcessingStartTime.Subtract(fileGroup.DataFiles.First().LastWriteTime).TotalSeconds;

                using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
                {
                    new TableOperations<OpenXDADailyStatistic>(connection).AddNewOrUpdateRecord(dailyStatistic);
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

        /// <summary>
        /// Updated the <see cref="OpenXDADailyStatistic"/> for a file group that successfully processed."/>
        /// </summary>
        public static void UpdateSuccessFileProcessingStatistic(MeterDataSet meterDataSet)
        {
            try
            {
                s_mutex.WaitOne();

                OpenXDADailyStatistic dailyStatistic = GetDailyStatistic(meterDataSet.Meter.AssetKey, out DateTime now);

                if (dailyStatistic.AverageDownloadLatency > 0)
                    dailyStatistic.AverageDownloadLatency = (dailyStatistic.AverageDownloadLatency * dailyStatistic.TotalFilesProcessed + meterDataSet.FileGroup.DataFiles.First().LastWriteTime.Subtract(meterDataSet.FileGroup.DataStartTime).TotalSeconds) / (dailyStatistic.TotalFilesProcessed + 1);
                else
                    dailyStatistic.AverageDownloadLatency = meterDataSet.FileGroup.DataFiles.First().LastWriteTime.Subtract(meterDataSet.FileGroup.DataStartTime).TotalSeconds;

                if (dailyStatistic.AverageProcessingStartLatency > 0)
                    dailyStatistic.AverageProcessingStartLatency = (dailyStatistic.AverageProcessingStartLatency * dailyStatistic.TotalFilesProcessed + meterDataSet.FileGroup.ProcessingStartTime.Subtract(meterDataSet.FileGroup.DataFiles.First().LastWriteTime).TotalSeconds) / (dailyStatistic.TotalFilesProcessed + 1);
                else
                    dailyStatistic.AverageProcessingStartLatency = meterDataSet.FileGroup.ProcessingStartTime.Subtract(meterDataSet.FileGroup.DataFiles.First().LastWriteTime).TotalSeconds;

                if (dailyStatistic.AverageProcessingEndLatency > 0)
                    dailyStatistic.AverageProcessingEndLatency = (dailyStatistic.AverageProcessingEndLatency * dailyStatistic.TotalFilesProcessed + now.Subtract(meterDataSet.FileGroup.ProcessingStartTime).TotalSeconds) / (dailyStatistic.TotalFilesProcessed + 1);
                else
                    dailyStatistic.AverageProcessingEndLatency = now.Subtract(meterDataSet.FileGroup.ProcessingStartTime).TotalSeconds;

                if (dailyStatistic.AverageTotalProcessingLatency > 0)
                    dailyStatistic.AverageTotalProcessingLatency = (dailyStatistic.AverageTotalProcessingLatency * dailyStatistic.TotalFilesProcessed + now.Subtract(meterDataSet.FileGroup.DataStartTime).TotalSeconds) / (dailyStatistic.TotalFilesProcessed + 1);
                else
                    dailyStatistic.AverageTotalProcessingLatency = now.Subtract(meterDataSet.FileGroup.DataStartTime).TotalSeconds;

                dailyStatistic.TotalFilesProcessed += 1;
                dailyStatistic.TotalSuccessfulFilesProcessed += 1;
                dailyStatistic.LastSuccessfulFileProcessed = now;

                using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
                {
                    MeterDataQualitySummary trendingSummary = new TableOperations<MeterDataQualitySummary>(connection).QueryRecordWhere("[Date] = {0} AND MeterID = (SELECT ID FROM Meter WHERE AssetKey = {1})", dailyStatistic.Date, meterDataSet.Meter.AssetKey);

                    int warningLevel = int.Parse(new TableOperations<SystemCenter.Model.Setting>(connection).QueryRecordWhere("Name = 'OpenXDA.WarningLevel'")?.Value ?? "50");
                    int errorLevel = int.Parse(new TableOperations<SystemCenter.Model.Setting>(connection).QueryRecordWhere("Name = 'OpenXDA.ErrorLevel'")?.Value ?? "100");

                    if (trendingSummary == null)
                        trendingSummary = new MeterDataQualitySummary() { ExpectedPoints = 0, GoodPoints = 0, LatchedPoints = 0, UnreasonablePoints = 0, NoncongruentPoints = 0 };

                    if (dailyStatistic.Status == "Error")
                    {
                        Log.Warn($"Failed to update daily statistic for {dailyStatistic.Meter} - a previous error was present");

                    } // already an error, do nothing
                    else if (dailyStatistic.TotalUnsuccessfulFilesProcessed > errorLevel)
                    {
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
                    else if (dailyStatistic.Status == "Warning")
                    {
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
            catch (Exception ex)
            {
                Log.Error($"Failed to create daily statistic for {meterDataSet.Meter.AssetKey} - {ex.Message}");
            }
            finally
            {
                s_mutex.ReleaseMutex();
            }
        }

        private static OpenXDADailyStatistic GetDailyStatistic(string meterKey, out DateTime now)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                string timeZone = connection.ExecuteScalar<string>("SELECT Value FROM Setting WHERE Name = 'System.XDATimeZone'");
                now = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById(timeZone));

                IEnumerable<OpenXDADailyStatistic> dailyStatistics = new TableOperations<OpenXDADailyStatistic>(connection).QueryRecordsWhere("Meter = {0} AND Date = {1}", meterKey, now.Date.ToString("MM/dd/yyyy"));

                if (!dailyStatistics.Any())
                    return new OpenXDADailyStatistic()
                    {
                        ID = 0,
                        Date = now.Date.ToString("MM/dd/yyyy"),
                        Meter = meterKey,

                        LastSuccessfulFileProcessed = null,
                        LastUnsuccessfulFileProcessed = null,
                        LastUnsuccessfulFileProcessedExplanation = null,

                        TotalFilesProcessed = 0,
                        TotalSuccessfulFilesProcessed = 0,
                        TotalUnsuccessfulFilesProcessed = 0,
                        TotalEmailsSent = 0,

                        AverageDownloadLatency = 0,
                        AverageProcessingStartLatency = 0,
                        AverageProcessingEndLatency = 0,
                        AverageTotalProcessingLatency = 0,
                        AverageTotalEmailLatency = 0,
                        AverageEmailLatency = 0
                    };

                return dailyStatistics.First();
            }
        }

        private static readonly Mutex s_mutex = new Mutex();
        private static readonly ILog Log = LogManager.GetLogger(typeof(DailyStatisticOperation));
    }
}
