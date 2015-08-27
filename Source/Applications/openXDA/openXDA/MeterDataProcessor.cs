//******************************************************************************************************
//  MeterDataProcessor.cs - Gbtc
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
//  08/26/2015 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Transactions;
using FaultData.DataAnalysis;
using FaultData.Database;
using FaultData.DataOperations;
using FaultData.DataReaders;
using FaultData.DataSets;
using FaultData.DataWriters;
using GSF.Annotations;
using GSF.Collections;
using GSF.Configuration;
using GSF.IO;
using log4net;
using openXDA.Configuration;

namespace openXDA
{
    public class MeterDataProcessor
    {
        #region [ Members ]

        // Fields
        private string m_connectionString;
        private Dictionary<string, DateTime> m_fileCreationTimes;

        #endregion

        #region [ Constructors ]

        public MeterDataProcessor(string connectionString)
        {
            m_connectionString = connectionString;
            m_fileCreationTimes = new Dictionary<string, DateTime>();
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Processes the file at the given path.
        /// </summary>
        /// <param name="meterKey">The name of the meter that produced the file.</param>
        /// <param name="filePath">The path to the file to be processed.</param>
        /// <returns>False if the file was not able to be processed and needs to be processed again later.</returns>
        public bool ProcessFile(string meterKey, string filePath)
        {
            SystemSettings systemSettings;
            TimeZoneInfo defaultMeterTimeZone;
            TimeZoneInfo xdaTimeZone;

            FileInfoDataContext fileInfo;
            FileGroup fileGroup;

            IDataReader reader;

            List<MeterDataSet> meterDataSets;

            try
            {
                systemSettings = new SystemSettings(m_connectionString);

                using (DbAdapterContainer dbAdapterContainer = new DbAdapterContainer(systemSettings.DbConnectionString, systemSettings.DbTimeout))
                {
                    fileInfo = dbAdapterContainer.GetAdapter<FileInfoDataContext>();
                    defaultMeterTimeZone = systemSettings.DefaultMeterTimeZoneInfo;
                    xdaTimeZone = systemSettings.XDATimeZoneInfo;

                    // Validate that the creation time is within the user-defined tolerance
                    if (!ValidateFileCreationTime(filePath, systemSettings, fileInfo))
                        return true;

                    // Attempt to find a meter in the database matching the meter name parsed from the file path
                    if (!ValidateMeterKey(meterKey, filePath, systemSettings, dbAdapterContainer))
                        return true;

                    reader = GetDataReader(filePath, systemSettings, dbAdapterContainer);

                    if ((object)reader == null)
                        return true;

                    using (reader as IDisposable)
                    {
                        ConnectionStringParser.ParseConnectionString(m_connectionString, reader);

                        // Requeue the file if the reader is unable to process
                        if (!reader.CanParse(filePath, GetMaxFileCreationTime(filePath)))
                            return false;

                        // Create a file group for this file in the database
                        fileGroup = LoadFileGroup(fileInfo, filePath, xdaTimeZone);

                        // Parse the file
                        meterDataSets = reader.Parse(filePath);

                        // If reader is keeping the file open, go ahead and close it now
                        TryDispose(reader as IDisposable);

                        // Set properties on each of the meter data sets
                        foreach (MeterDataSet meterDataSet in meterDataSets)
                        {
                            meterDataSet.FilePath = filePath;
                            meterDataSet.FileGroup = fileGroup;
                            meterDataSet.Meter.AssetKey = meterKey;
                            ShiftTime(meterDataSet, defaultMeterTimeZone, xdaTimeZone);
                        }

                        // Set the DataStartTime and DataEndTime of the file group
                        SetDataTimeRange(meterDataSets, fileGroup, systemSettings, fileInfo);

                        // Determine if the duration of the file is reasonable
                        if (!ValidateFileDuration(filePath, fileGroup, systemSettings, fileInfo))
                            return true;

                        // Determine if the timestamps in the file are reasonable
                        if (!ValidateFileTimestamps(filePath, fileGroup, systemSettings, fileInfo))
                            return true;

                        // Process meter data sets
                        OnStatusMessage("Processing meter data from file \"{0}\"...", filePath);
                        ProcessMeterDataSets(meterDataSets, systemSettings, dbAdapterContainer);
                        OnStatusMessage("Finished processing data from file \"{0}\".", filePath);

                        ClearFileCreationTimes(filePath);
                    }
                }
            }
            catch (Exception ex)
            {
                string message = string.Format("Failed to process file \"{0}\" due to exception: {1}", filePath, ex.Message);
                OnHandleException(new InvalidOperationException(message, ex));
            }

            return true;
        }

        public void ProcessMeterDataSets(List<MeterDataSet> meterDataSets, SystemSettings systemSettings, DbAdapterContainer dbAdapterContainer)
        {
            TimeZoneInfo xdaTimeZone;
            DateTime processingEndTime;

            try
            {
                foreach (MeterDataSet meterDataSet in meterDataSets)
                    ProcessMeterData(meterDataSet, dbAdapterContainer);

                xdaTimeZone = systemSettings.XDATimeZoneInfo;
                processingEndTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, xdaTimeZone);

                foreach (MeterDataSet meterDataSet in meterDataSets)
                    meterDataSet.FileGroup.ProcessingEndTime = processingEndTime;

                dbAdapterContainer.GetAdapter<FileInfoDataContext>().SubmitChanges();
            }
            catch (Exception ex)
            {
                OnHandleException(ex);
            }
        }

        private void ProcessMeterData(MeterDataSet meterDataSet, DbAdapterContainer dbAdapterContainer)
        {
            try
            {
                meterDataSet.ConnectionString = m_connectionString;
                ExecuteDataOperations(meterDataSet, dbAdapterContainer);
                ExecuteDataWriters(meterDataSet, dbAdapterContainer);
            }
            catch (Exception ex)
            {
                try
                {
                    OnHandleException(ex);
                    meterDataSet.FileGroup.Error = 1;
                    dbAdapterContainer.GetAdapter<FileInfoDataContext>().SubmitChanges();
                }
                catch
                {
                    // Ignore errors here as they are most likely
                    // related to the error we originally caught
                }
            }
        }

        private IDataReader GetDataReader(string filePath, SystemSettings systemSettings, DbAdapterContainer dbAdapterContainer)
        {
            string extension;

            SystemInfoDataContext systemInfo;
            FileInfoDataContext fileInfo;

            Type readerType;
            IDataReader reader;

            TimeZoneInfo xdaTimeZone;
            FileGroup fileGroup;

            // Determine what data reader to use based on file extension
            extension = FilePath.GetExtension(filePath).Substring(1);

            // Get database adapters from the dbAdapterContainer
            systemInfo = dbAdapterContainer.GetAdapter<SystemInfoDataContext>();
            fileInfo = dbAdapterContainer.GetAdapter<FileInfoDataContext>();

            // Load type used to parse this file from the database
            readerType = systemInfo.DataReaders
                .Where(dataReader => dataReader.FileExtension.Equals(extension))
                .ToList()
                .Select(dataReader => LoadType(dataReader.AssemblyName, dataReader.TypeName))
                .Where(type => (object)type != null)
                .Where(type => typeof(IDataReader).IsAssignableFrom(type))
                .SingleOrDefault(type => (object)type.GetConstructor(Type.EmptyTypes) != null);

            if ((object)readerType == null)
            {
                OnStatusMessage("Skipped file \"{0}\" because no valid data reader was found for file extension '{1}'.", filePath, extension);

                xdaTimeZone = systemSettings.XDATimeZoneInfo;

                fileGroup = LoadFileGroup(fileInfo, filePath, xdaTimeZone);
                fileGroup.ProcessingEndTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, xdaTimeZone);
                fileGroup.Error = 1;
                fileInfo.SubmitChanges();

                return null;
            }

            // Create the data reader
            reader = (IDataReader)Activator.CreateInstance(readerType);

            return reader;
        }

        private void ExecuteDataOperations(MeterDataSet meterDataSet, DbAdapterContainer dbAdapterContainer)
        {
            IEnumerable<IEnumerable<DataOperation>> operationGroups;
            List<IDataOperation> dataOperations = null;

            try
            {
                // Load data operations from the database
                operationGroups = dbAdapterContainer.GetAdapter<SystemInfoDataContext>().DataOperations
                    .GroupBy(dataOperation => dataOperation.TransactionOrder)
                    .OrderBy(grouping => grouping.Key)
                    .Cast<IEnumerable<DataOperation>>();

                foreach (IEnumerable<DataOperation> operationGroup in operationGroups)
                {
                    dataOperations = operationGroup
                        .OrderBy(dataOperation => dataOperation.LoadOrder)
                        .Select(dataOperation => LoadType(dataOperation.AssemblyName, dataOperation.TypeName))
                        .Where(type => (object)type != null)
                        .Where(type => typeof(IDataOperation).IsAssignableFrom(type))
                        .Where(type => (object)type.GetConstructor(Type.EmptyTypes) != null)
                        .Select(Activator.CreateInstance)
                        .Cast<IDataOperation>()
                        .ToList();

                    foreach (IDataOperation dataOperation in dataOperations)
                    {
                        // Provide system settings to the data operation
                        ConnectionStringParser.ParseConnectionString(meterDataSet.ConnectionString, dataOperation);

                        // Prepare for execution of the data operation
                        dataOperation.Prepare(dbAdapterContainer);
                    }

                    // Execute the data operations
                    foreach (IDataOperation dataOperation in dataOperations)
                        dataOperation.Execute(meterDataSet);

                    // Load data from all data operations in a single transaction
                    using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, GetTransactionOptions()))
                    {
                        foreach (IDataOperation dataOperation in dataOperations)
                            dataOperation.Load(dbAdapterContainer);

                        transactionScope.Complete();
                    }
                }
            }
            finally
            {
                if ((object)dataOperations != null)
                {
                    // ReSharper disable once SuspiciousTypeConversion.Global
                    foreach (IDataOperation dataOperation in dataOperations)
                        TryDispose(dataOperation as IDisposable);
                }
            }
        }

        private void ExecuteDataWriters(MeterDataSet meterDataSet, DbAdapterContainer dbAdapterContainer)
        {
            List<IDataWriter> dataWriters = null;

            try
            {
                // Load data operations from the database
                dataWriters = dbAdapterContainer.GetAdapter<SystemInfoDataContext>().DataWriters
                    .ToList()
                    .Select(dataWriter => LoadType(dataWriter.AssemblyName, dataWriter.TypeName))
                    .Where(type => (object)type != null)
                    .Where(type => typeof(IDataWriter).IsAssignableFrom(type))
                    .Where(type => (object)type.GetConstructor(Type.EmptyTypes) != null)
                    .Select(Activator.CreateInstance)
                    .Cast<IDataWriter>()
                    .ToList();

                foreach (IDataWriter dataWriter in dataWriters)
                {
                    // Provide system settings to the data operation
                    ConnectionStringParser.ParseConnectionString(meterDataSet.ConnectionString, dataWriter);

                    // Prepare for execution of the data operation
                    dataWriter.WriteResults(dbAdapterContainer, meterDataSet);
                }
            }
            finally
            {
                if ((object)dataWriters != null)
                {
                    // ReSharper disable once SuspiciousTypeConversion.Global
                    foreach (IDataWriter dataWriter in dataWriters)
                        TryDispose(dataWriter as IDisposable);
                }
            }
        }

        private Type LoadType(string assemblyName, string typeName)
        {
            Assembly assembly;

            try
            {
                assembly = Assembly.LoadFrom(FilePath.GetAbsolutePath(assemblyName));

                if ((object)assembly != null)
                    return assembly.GetType(typeName);

                return null;
            }
            catch (Exception ex)
            {
                OnHandleException(ex);
                return null;
            }
        }

        // Loads a file group containing information about the file on the given
        // file path, as well as the files related to it, into the database.
        private FileGroup LoadFileGroup(FileInfoDataContext dataContext, string filePath, TimeZoneInfo xdaTimeZone)
        {
            string directory = FilePath.GetDirectoryName(filePath);
            string rootFileName = FilePath.GetFileNameWithoutExtension(filePath);

            FileInfo fileInfo;
            FileGroup fileGroup;
            DataFile dataFile;

            fileGroup = new FileGroup();
            fileGroup.ProcessingStartTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, xdaTimeZone);

            foreach (string file in FilePath.GetFileList(Path.Combine(directory, rootFileName + ".*")))
            {
                fileInfo = new FileInfo(file);

                dataFile = new DataFile();
                dataFile.FilePath = file;
                dataFile.FilePathHash = file.GetHashCode();
                dataFile.FileSize = fileInfo.Length;
                dataFile.CreationTime = TimeZoneInfo.ConvertTimeFromUtc(fileInfo.CreationTimeUtc, xdaTimeZone);
                dataFile.LastWriteTime = TimeZoneInfo.ConvertTimeFromUtc(fileInfo.LastWriteTimeUtc, xdaTimeZone);
                dataFile.LastAccessTime = TimeZoneInfo.ConvertTimeFromUtc(fileInfo.LastAccessTimeUtc, xdaTimeZone);
                dataFile.FileGroup = fileGroup;
            }

            dataContext.FileGroups.InsertOnSubmit(fileGroup);
            dataContext.SubmitChanges();

            return fileGroup;
        }

        // Determines the start time and end time of the given data and sets the properties on the given file group.
        private static void SetDataTimeRange(List<MeterDataSet> meterDataSets, FileGroup fileGroup, SystemSettings systemSettings, FileInfoDataContext fileInfo)
        {
            DateTime dataStartTime;
            DateTime dataEndTime;
            TimeZoneInfo xdaTimeZone;

            dataStartTime = meterDataSets
                .SelectMany(meterDataSet => meterDataSet.DataSeries)
                .Where(dataSeries => dataSeries.DataPoints.Any())
                .Select(dataSeries => dataSeries.DataPoints.First().Time)
                .DefaultIfEmpty()
                .Min();

            dataEndTime = meterDataSets
                .SelectMany(meterDataSet => meterDataSet.DataSeries)
                .Where(dataSeries => dataSeries.DataPoints.Any())
                .Select(dataSeries => dataSeries.DataPoints.Last().Time)
                .DefaultIfEmpty()
                .Max();

            xdaTimeZone = systemSettings.XDATimeZoneInfo;

            if (dataStartTime != default(DateTime))
                fileGroup.DataStartTime = TimeZoneInfo.ConvertTimeFromUtc(dataStartTime, xdaTimeZone);

            if (dataEndTime != default(DateTime))
                fileGroup.DataEndTime = TimeZoneInfo.ConvertTimeFromUtc(dataEndTime, xdaTimeZone);

            fileInfo.SubmitChanges();
        }

        // Adjusts the timestamps in the given data sets to the time zone of XDA.
        private void ShiftTime(MeterDataSet meterDataSet, TimeZoneInfo defaultMeterTimeZone, TimeZoneInfo xdaTimeZone)
        {
            TimeZoneInfo meterTimeZone;

            if (!string.IsNullOrEmpty(meterDataSet.Meter.TimeZone))
                meterTimeZone = TimeZoneInfo.FindSystemTimeZoneById(meterDataSet.Meter.TimeZone);
            else
                meterTimeZone = defaultMeterTimeZone;

            foreach (DataSeries dataSeries in meterDataSet.DataSeries)
            {
                foreach (DataPoint dataPoint in dataSeries.DataPoints)
                {
                    dataPoint.Time = TimeZoneInfo.ConvertTimeToUtc(dataPoint.Time, meterTimeZone);
                    dataPoint.Time = TimeZoneInfo.ConvertTimeFromUtc(dataPoint.Time, xdaTimeZone);
                }
            }
        }

        // Gets the creation time of the file with the given file name.
        private DateTime GetMaxFileCreationTime(string fileName)
        {
            string directory;
            string rootFileName;
            string[] fileList;

            directory = FilePath.GetDirectoryName(fileName);
            rootFileName = FilePath.GetFileNameWithoutExtension(fileName);
            fileList = FilePath.GetFileList(Path.Combine(directory, rootFileName + ".*"));

            return fileList.Max(file => m_fileCreationTimes.GetOrAdd(file, path => DateTime.Now));
        }

        // Removes the creation time of the file with the given
        // file name from the collection of file creation times.
        private void ClearFileCreationTimes(string fileName)
        {
            string directory;
            string rootFileName;
            string[] fileList;

            directory = FilePath.GetDirectoryName(fileName);
            rootFileName = FilePath.GetFileNameWithoutExtension(fileName);
            fileList = FilePath.GetFileList(Path.Combine(directory, rootFileName + ".*"));

            foreach (string file in fileList)
                m_fileCreationTimes.Remove(file);
        }

        private bool ValidateFileCreationTime(string filePath, SystemSettings systemSettings, FileInfoDataContext fileInfo)
        {
            FileGroup fileGroup;
            TimeZoneInfo xdaTimeZone;

            if (systemSettings.MaxFileCreationTimeOffset > 0.0D && DateTime.UtcNow.Subtract(File.GetCreationTimeUtc(filePath)).TotalHours > systemSettings.MaxFileCreationTimeOffset)
            {
                OnStatusMessage("Skipped file \"{0}\" because file creation time '{1}' is too old.", filePath, File.GetCreationTimeUtc(filePath));

                xdaTimeZone = systemSettings.XDATimeZoneInfo;

                fileGroup = LoadFileGroup(fileInfo, filePath, xdaTimeZone);
                fileGroup.ProcessingEndTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, xdaTimeZone);
                fileInfo.SubmitChanges();

                return false;
            }

            return true;
        }

        private bool ValidateMeterKey(string meterKey, string filePath, SystemSettings systemSettings, DbAdapterContainer dbAdapterContainer)
        {
            MeterInfoDataContext meterInfo;
            FileInfoDataContext fileInfo;
            TimeZoneInfo xdaTimeZone;
            FileGroup fileGroup;

            meterInfo = dbAdapterContainer.GetAdapter<MeterInfoDataContext>();

            if (!meterInfo.Meters.Any(m => m.AssetKey == meterKey))
            {
                OnStatusMessage("Skipped file \"{0}\" because no meter configuration was found for meter {1}.", filePath, meterKey);

                fileInfo = dbAdapterContainer.GetAdapter<FileInfoDataContext>();

                xdaTimeZone = systemSettings.XDATimeZoneInfo;

                fileGroup = LoadFileGroup(fileInfo, filePath, xdaTimeZone);
                fileGroup.ProcessingEndTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, xdaTimeZone);
                fileGroup.Error = 1;
                fileInfo.SubmitChanges();

                return false;
            }

            return true;
        }

        private bool ValidateFileDuration(string filePath, FileGroup fileGroup, SystemSettings systemSettings, FileInfoDataContext fileInfo)
        {
            TimeZoneInfo xdaTimeZone;
            double timeDifference;

            timeDifference = fileGroup.DataEndTime.Subtract(fileGroup.DataStartTime).TotalSeconds;

            if (systemSettings.MaxFileDuration > 0.0D && timeDifference > systemSettings.MaxFileDuration)
            {
                OnStatusMessage("Skipped file \"{0}\" because duration of the file ({1:0.##} seconds) is too long.", filePath, timeDifference);

                xdaTimeZone = systemSettings.XDATimeZoneInfo;

                fileGroup = LoadFileGroup(fileInfo, filePath, xdaTimeZone);
                fileGroup.ProcessingEndTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, xdaTimeZone);
                fileInfo.SubmitChanges();

                return false;
            }

            return true;
        }

        private bool ValidateFileTimestamps(string filePath, FileGroup fileGroup, SystemSettings systemSettings, FileInfoDataContext fileInfo)
        {
            TimeZoneInfo xdaTimeZone;
            double timeDifference;

            xdaTimeZone = systemSettings.XDATimeZoneInfo;
            timeDifference = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, xdaTimeZone).Subtract(fileGroup.DataStartTime).TotalHours;

            if (systemSettings.MinTimeOffset > 0.0D && timeDifference > systemSettings.MinTimeOffset)
            {
                OnStatusMessage("Skipped file \"{0}\" because data start time '{1}' is too old.", filePath, fileGroup.DataStartTime);

                fileGroup.ProcessingEndTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, xdaTimeZone);
                fileInfo.SubmitChanges();

                return true;
            }

            timeDifference = fileGroup.DataEndTime.Subtract(TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, xdaTimeZone)).TotalHours;

            if (systemSettings.MaxTimeOffset > 0.0D && timeDifference > systemSettings.MaxTimeOffset)
            {
                OnStatusMessage("Skipped file \"{0}\" because data end time '{1}' is too far in the future.", filePath, fileGroup.DataEndTime);
                fileGroup = LoadFileGroup(fileInfo, filePath, xdaTimeZone);
                fileGroup.ProcessingEndTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, xdaTimeZone);
                fileInfo.SubmitChanges();

                return false;
            }

            return true;
        }

        private void TryDispose(IDisposable obj)
        {
            try
            {
                if ((object)obj != null)
                    obj.Dispose();
            }
            catch (Exception ex)
            {
                OnHandleException(ex);
            }
        }

        [StringFormatMethod("format")]
        private void OnStatusMessage(string format, params object[] args)
        {
            Log.Info(string.Format(format, args));
        }

        private void OnHandleException(Exception ex)
        {
            Log.Error(ex.Message, ex);
        }

        #endregion

        #region [ Static ]

        // Static Fields
        private static readonly ConnectionStringParser<SettingAttribute, CategoryAttribute> ConnectionStringParser = new ConnectionStringParser<SettingAttribute, CategoryAttribute>();
        private static readonly ILog Log = LogManager.GetLogger(typeof(MeterDataProcessor));

        // Static Methods
        private static TransactionOptions GetTransactionOptions()
        {
            return new TransactionOptions()
            {
                IsolationLevel = IsolationLevel.ReadCommitted,
                Timeout = TransactionManager.MaximumTimeout
            };
        }

        #endregion
    }
}
