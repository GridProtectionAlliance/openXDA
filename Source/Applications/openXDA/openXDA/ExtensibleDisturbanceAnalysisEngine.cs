//*********************************************************************************************************************
// ExtensibleDisturbanceAnalysisEngine.cs
// Version 1.1 and subsequent releases
//
//  Copyright © 2013, Grid Protection Alliance.  All Rights Reserved.
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
// --------------------------------------------------------------------------------------------------------------------
//
// Version 1.0
//
// Copyright 2012 ELECTRIC POWER RESEARCH INSTITUTE, INC. All rights reserved.
//
// openXDA ("this software") is licensed under BSD 3-Clause license.
//
// Redistribution and use in source and binary forms, with or without modification, are permitted provided that the 
// following conditions are met:
//
// •    Redistributions of source code must retain the above copyright  notice, this list of conditions and 
//      the following disclaimer.
//
// •    Redistributions in binary form must reproduce the above copyright notice, this list of conditions and 
//      the following disclaimer in the documentation and/or other materials provided with the distribution.
//
// •    Neither the name of the Electric Power Research Institute, Inc. (“EPRI”) nor the names of its contributors 
//      may be used to endorse or promote products derived from this software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, 
// INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE 
// DISCLAIMED. IN NO EVENT SHALL EPRI BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL 
// DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; 
// OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, 
// OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
// POSSIBILITY OF SUCH DAMAGE.
//
//
// This software incorporates work covered by the following copyright and permission notice: 
//
// •    TVA Code Library 4.0.4.3 - Tennessee Valley Authority, tvainfo@tva.gov
//      No copyright is claimed pursuant to 17 USC § 105. All Other Rights Reserved.
//
//      Licensed under TVA Custom License based on NASA Open Source Agreement (TVA Custom NOSA); 
//      you may not use TVA Code Library except in compliance with the TVA Custom NOSA. You may  
//      obtain a copy of the TVA Custom NOSA at http://tvacodelibrary.codeplex.com/license.
//
//      TVA Code Library is provided by the copyright holders and contributors "as is" and any express 
//      or implied warranties, including, but not limited to, the implied warranties of merchantability 
//      and fitness for a particular purpose are disclaimed.
//
//*********************************************************************************************************************
//
//  Code Modification History:
//  -------------------------------------------------------------------------------------------------------------------
//  05/16/2012 - J. Ritchie Carroll, Grid Protection Alliance
//       Generated original version of source code.
//  10/02/2014 - Stephen C. Wills, Grid Protection Alliance
//       Adapted from the openFLE project to use the new fault location logic.
//
//*********************************************************************************************************************

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Transactions;
using FaultData.DataAnalysis;
using FaultData.Database;
using FaultData.DataOperations;
using FaultData.DataReaders;
using FaultData.DataSets;
using FaultData.DataWriters;
using GSF;
using GSF.Annotations;
using GSF.Collections;
using GSF.Configuration;
using GSF.IO;
using log4net;
using openXDA.Configuration;
using FileShare = openXDA.Configuration.FileShare;

namespace openXDA
{
    /// <summary>
    /// Represents an engine that processes power quality data
    /// to determine the locations of faults along power lines.
    /// </summary>
    public class ExtensibleDisturbanceAnalysisEngine : IDisposable
    {
        #region [ Members ]

        // Nested Types
        private class MeterDataProcessor
        {
            #region [ Members ]

            // Fields
            public string MeterDataFile;
            public SystemSettings SystemSettings;
            public DbAdapterContainer DbAdapterContainer;
            public List<MeterDataSet> MeterDataSets;

            #endregion

            #region [ Methods ]

            public void Process()
            {
                BlockingCollection<int> threadIDs;
                Thread processThread;
                int threadID;

                threadIDs = Interlocked.CompareExchange(ref s_threadIDs, null, null);
                threadID = threadIDs.Take();

                processThread = new Thread(() =>
                {
                    DateTime processingEndTime;

                    try
                    {
                        ThreadContext.Properties["ID"] = threadID;

                        using (DbAdapterContainer)
                        {
                            OnStatusMessage("Processing meter data from file \"{0}\"...", MeterDataFile);

                            foreach (MeterDataSet meterDataSet in MeterDataSets)
                                ProcessMeterData(meterDataSet);

                            processingEndTime = DateTime.UtcNow;

                            foreach (MeterDataSet meterDataSet in MeterDataSets)
                                meterDataSet.FileGroup.ProcessingEndTime = processingEndTime;

                            DbAdapterContainer.GetAdapter<FileInfoDataContext>().SubmitChanges();

                            OnStatusMessage("Finished processing data from file \"{0}\".", MeterDataFile);
                        }
                    }
                    catch (Exception ex)
                    {
                        OnHandleException(ex);
                    }

                    threadIDs.Add(threadID);
                });

                processThread.IsBackground = true;
                processThread.Start();
            }

            private void ProcessMeterData(MeterDataSet meterDataSet)
            {
                try
                {
                    meterDataSet.ConnectionString = SystemSettings.ToConnectionString();
                    ExecuteDataOperations(meterDataSet);
                    ExecuteDataWriters(meterDataSet);
                }
                catch (Exception ex)
                {
                    try
                    {
                        OnHandleException(ex);
                        meterDataSet.FileGroup.ProcessingEndTime = DateTime.UtcNow;
                        meterDataSet.FileGroup.Error = 1;
                        DbAdapterContainer.GetAdapter<FaultLocationInfoDataContext>().SubmitChanges();
                    }
                    catch
                    {
                        // Ignore errors here as they are most likely
                        // related to the error we originally caught
                    }
                }
            }

            private void ExecuteDataOperations(MeterDataSet meterDataSet)
            {
                IEnumerable<IEnumerable<DataOperation>> operationGroups;
                List<IDataOperation> dataOperations = null;
                ConnectionStringParser connectionStringParser;

                try
                {
                    // Load data operations from the database
                    operationGroups = DbAdapterContainer.GetAdapter<SystemInfoDataContext>().DataOperations
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

                        connectionStringParser = new ConnectionStringParser();
                        connectionStringParser.SerializeUnspecifiedProperties = true;

                        foreach (IDataOperation dataOperation in dataOperations)
                        {
                            // Provide system settings to the data operation
                            connectionStringParser.ParseConnectionString(meterDataSet.ConnectionString, dataOperation);

                            // Prepare for execution of the data operation
                            dataOperation.Prepare(DbAdapterContainer);
                        }

                        // Execute the data operations
                        foreach (IDataOperation dataOperation in dataOperations)
                            dataOperation.Execute(meterDataSet);

                        // Load data from all data operations in a single transaction
                        using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, GetTransactionOptions()))
                        {
                            foreach (IDataOperation dataOperation in dataOperations)
                                dataOperation.Load(DbAdapterContainer);

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

            private void ExecuteDataWriters(MeterDataSet meterDataSet)
            {
                List<IDataWriter> dataWriters = null;
                ConnectionStringParser connectionStringParser;

                try
                {
                    // Load data operations from the database
                    dataWriters = DbAdapterContainer.GetAdapter<SystemInfoDataContext>().DataWriters
                        .ToList()
                        .Select(dataWriter => LoadType(dataWriter.AssemblyName, dataWriter.TypeName))
                        .Where(type => (object)type != null)
                        .Where(type => typeof(IDataWriter).IsAssignableFrom(type))
                        .Where(type => (object)type.GetConstructor(Type.EmptyTypes) != null)
                        .Select(Activator.CreateInstance)
                        .Cast<IDataWriter>()
                        .ToList();

                    connectionStringParser = new ConnectionStringParser();
                    connectionStringParser.SerializeUnspecifiedProperties = true;

                    foreach (IDataWriter dataWriter in dataWriters)
                    {
                        // Provide system settings to the data operation
                        connectionStringParser.ParseConnectionString(meterDataSet.ConnectionString, dataWriter);

                        // Prepare for execution of the data operation
                        dataWriter.WriteResults(DbAdapterContainer, meterDataSet);
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
            private static BlockingCollection<int> s_threadIDs;

            // ReSharper disable once MemberHidesStaticFromOuterClass
            private static readonly ILog Log = LogManager.GetLogger(typeof(MeterDataProcessor));

            // Static Constructor
            static MeterDataProcessor()
            {
                SetThreadCount(Environment.ProcessorCount);
            }

            // Static Methods
            public static void SetThreadCount(int threadCount)
            {
                IEnumerable<int> threadIDs = Enumerable.Range(1, threadCount);
                ConcurrentQueue<int> threadIDQueue = new ConcurrentQueue<int>(threadIDs);
                Interlocked.Exchange(ref s_threadIDs, new BlockingCollection<int>(threadIDQueue));
            }

            #endregion
        }

        // Constants

        /// <summary>
        /// Globally unique identifier required by the file processor to identify its cached list of processed files.
        /// </summary>
        private static readonly Guid FileProcessorID = new Guid("4E3D3A90-6E7E-4AB7-96F3-3A5899081D0D");

        // Fields
        private SystemSettings m_systemSettings;
        private FileProcessor m_fileProcessor;

        private Dictionary<string, DateTime> m_fileCreationTimes = new Dictionary<string, DateTime>();

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Starts the fault location engine.
        /// </summary>
        public void Start()
        {
            ConfigurationFile configFile = ConfigurationFile.Current;
            CategorizedSettingsElementCollection category = configFile.Settings["systemSettings"];
            string connectionString;

            IEnumerable<string> validExtensions;

            // Retrieve the connection string from the config file
            category.Add("ConnectionString", "Data Source=localhost; Initial Catalog=openXDA; Integrated Security=SSPI", "Defines the connection to the openXDA database.");
            connectionString = category["ConnectionString"].Value;

            // Get system settings from the database
            using (SystemInfoDataContext systemInfo = new SystemInfoDataContext(connectionString))
            {
                m_systemSettings = new SystemSettings(LoadSystemSettings(systemInfo));

                validExtensions = systemInfo.DataReaders
                    .Select(reader => reader.FileExtension)
                    .Select(extension => string.Format("*.{0}", extension))
                    .ToList();
            }

            // Attempt to authenticate to configured file shares
            foreach (FileShare fileShare in m_systemSettings.FileShareList)
            {
                if (!fileShare.TryAuthenticate())
                    OnProcessException(fileShare.AuthenticationException);
            }

            // Make sure watch directories exist
            foreach (string path in m_systemSettings.WatchDirectoryList)
                TryCreateDirectory(path);

            // Make sure results directory exists
            TryCreateDirectory(m_systemSettings.ResultsPath);

            // Set the number of threads used for processing meter data
            MeterDataProcessor.SetThreadCount(m_systemSettings.ProcessingThreadCount);

            // Setup new file processor to monitor the watch directories
            if ((object)m_fileProcessor == null)
            {
                m_fileProcessor = new FileProcessor(FileProcessorID);
                m_fileProcessor.Filter = string.Join(Path.PathSeparator.ToString(), validExtensions);
                m_fileProcessor.Processing += FileProcessor_Processing;
                m_fileProcessor.Error += FileProcessor_Error;

                foreach (string path in m_systemSettings.WatchDirectoryList)
                    m_fileProcessor.AddTrackedDirectory(path);
            }
        }

        /// <summary>
        /// Reloads system settings from the database.
        /// </summary>
        public void ReloadSystemSettings()
        {
            using (SystemInfoDataContext systemInfo = new SystemInfoDataContext(m_systemSettings.DbConnectionString))
            {
                m_systemSettings = new SystemSettings(LoadSystemSettings(systemInfo));
            }
        }

        /// <summary>
        /// Stops the fault location engine.
        /// </summary>
        public void Stop()
        {
            m_fileProcessor.ClearTrackedDirectories();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            // Stop file monitor timer
            if ((object)m_fileProcessor != null)
            {
                m_fileProcessor.Processing -= FileProcessor_Processing;
                m_fileProcessor.Error -= FileProcessor_Error;
                m_fileProcessor.Dispose();
                m_fileProcessor = null;
            }
        }

        // Called when the file processor has picked up a file in one of the watch
        // directories. This handler validates the file and processes it if able.
        private void FileProcessor_Processing(object sender, FileProcessorEventArgs fileProcessorEventArgs)
        {
            string filePath;
            FileGroup fileGroup;
            DateTime dataStartTime;
            DateTime dataEndTime;

            string meterKey;
            string extension;
            Type readerType;

            IDataReader reader;
            ConnectionStringParser connectionStringParser;

            DbAdapterContainer dbAdapterContainer;
            FileInfoDataContext fileInfo;
            MeterInfoDataContext meterInfo;
            SystemInfoDataContext systemInfo;

            SystemSettings systemSettings;
            MeterDataProcessor meterDataProcessor;

            TimeZoneInfo xdaTimeZone;
            double timeDifference;

            dbAdapterContainer = null;
            filePath = fileProcessorEventArgs.FullPath;

            try
            {
                dbAdapterContainer = new DbAdapterContainer(m_systemSettings.DbConnectionString, m_systemSettings.DbTimeout);

                fileInfo = dbAdapterContainer.GetAdapter<FileInfoDataContext>();
                meterInfo = dbAdapterContainer.GetAdapter<MeterInfoDataContext>();
                systemInfo = dbAdapterContainer.GetAdapter<SystemInfoDataContext>();

                // Determine whether the file has already been processed
                if (fileProcessorEventArgs.AlreadyProcessed)
                {
                    if (fileInfo.DataFiles.Any(dataFile => dataFile.FilePath == filePath && dataFile.FileGroup.ProcessingEndTime > DateTime.MinValue))
                    {
                        OnStatusMessage("Skipped file \"{0}\" because it has already been processed.", filePath);
                        return;
                    }
                }

                xdaTimeZone = TimeZoneInfo.FindSystemTimeZoneById(m_systemSettings.XDATimeZone);

                // Validate that the creation time is within the user-defined tolerance
                if (m_systemSettings.MaxFileCreationTimeOffset > 0.0D && DateTime.UtcNow.Subtract(File.GetCreationTimeUtc(filePath)).TotalHours > m_systemSettings.MaxFileCreationTimeOffset)
                {
                    OnStatusMessage("Skipped file \"{0}\" because file creation time '{1}' is too old.", filePath, File.GetCreationTimeUtc(filePath));
                    fileGroup = LoadFileGroup(fileInfo, filePath, xdaTimeZone);
                    fileGroup.ProcessingEndTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, xdaTimeZone);
                    fileGroup.Error = 1;
                    fileInfo.SubmitChanges();
                    return;
                }

                // Try to parse the name of the meter from the file path to determine whether this file can be parsed
                if (string.IsNullOrEmpty(m_systemSettings.FilePattern) || !TryParseFilePath(filePath, out meterKey))
                {
                    OnStatusMessage("Skipped file \"{0}\" because no meter could not be determined based on the FilePattern system setting.", filePath);
                    fileGroup = LoadFileGroup(fileInfo, filePath, xdaTimeZone);
                    fileGroup.ProcessingEndTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, xdaTimeZone);
                    fileGroup.Error = 1;
                    fileInfo.SubmitChanges();
                    return;
                }

                // Attempt to find a meter in the database matching the meter name parsed from the file path
                if (!meterInfo.Meters.Any(m => m.AssetKey == meterKey))
                {
                    OnStatusMessage("Skipped file \"{0}\" because no meter configuration was found for meter {1}.", filePath, meterKey);
                    fileGroup = LoadFileGroup(fileInfo, filePath, xdaTimeZone);
                    fileGroup.ProcessingEndTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, xdaTimeZone);
                    fileGroup.Error = 1;
                    fileInfo.SubmitChanges();
                    return;
                }

                // Determine what data reader to use based on file extension
                extension = FilePath.GetExtension(filePath).Substring(1);

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
                    fileGroup = LoadFileGroup(fileInfo, filePath, xdaTimeZone);
                    fileGroup.ProcessingEndTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, xdaTimeZone);
                    fileGroup.Error = 1;
                    fileInfo.SubmitChanges();
                    return;
                }

                // Create the data reader
                reader = (IDataReader)Activator.CreateInstance(readerType);

                using (reader as IDisposable)
                {
                    systemSettings = new SystemSettings(LoadSystemSettings(systemInfo));

                    connectionStringParser = new ConnectionStringParser();
                    connectionStringParser.SerializeUnspecifiedProperties = true;
                    connectionStringParser.ParseConnectionString(systemSettings.ToConnectionString(), reader);

                    if (reader.CanParse(filePath, GetMaxFileCreationTime(filePath)))
                    {
                        // Create a file group for this file in the database
                        fileGroup = LoadFileGroup(fileInfo, filePath, xdaTimeZone);

                        // Create the meter data processor that will be processing this file
                        meterDataProcessor = new MeterDataProcessor();

                        // Initialize properties of the meter data processor
                        meterDataProcessor.SystemSettings = systemSettings;
                        meterDataProcessor.MeterDataFile = filePath;
                        meterDataProcessor.DbAdapterContainer = dbAdapterContainer;

                        // Parse the file
                        meterDataProcessor.MeterDataSets = reader.Parse(filePath);

                        // If reader is keeping the file open, go ahead and close it now
                        TryDispose(reader as IDisposable);

                        // Set properties on each of the meter data sets
                        foreach (MeterDataSet meterDataSet in meterDataProcessor.MeterDataSets)
                        {
                            meterDataSet.FilePath = filePath;
                            meterDataSet.FileGroup = fileGroup;
                            meterDataSet.Meter.AssetKey = meterKey;
                            ShiftTime(meterDataSet, xdaTimeZone);
                        }

                        dataStartTime = meterDataProcessor.MeterDataSets
                            .SelectMany(meterDataSet => meterDataSet.DataSeries)
                            .Where(dataSeries => dataSeries.DataPoints.Any())
                            .Select(dataSeries => dataSeries.DataPoints.First().Time)
                            .DefaultIfEmpty()
                            .Min();

                        dataEndTime = meterDataProcessor.MeterDataSets
                            .SelectMany(meterDataSet => meterDataSet.DataSeries)
                            .Where(dataSeries => dataSeries.DataPoints.Any())
                            .Select(dataSeries => dataSeries.DataPoints.Last().Time)
                            .DefaultIfEmpty()
                            .Max();

                        if (dataStartTime != default(DateTime))
                            fileGroup.DataStartTime = TimeZoneInfo.ConvertTimeFromUtc(dataStartTime, xdaTimeZone);

                        if (dataEndTime != default(DateTime))
                            fileGroup.DataEndTime = TimeZoneInfo.ConvertTimeFromUtc(dataEndTime, xdaTimeZone);

                        // Commit changes to DataStartTime and DataEndTime
                        fileInfo.SubmitChanges();

                        // Determine if the timestamps in the file are reasonable
                        timeDifference = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, xdaTimeZone).Subtract(dataStartTime).TotalHours;

                        if (m_systemSettings.MinTimeOffset > 0.0D && timeDifference > m_systemSettings.MinTimeOffset)
                        {
                            OnStatusMessage("Skipped file \"{0}\" because data start time '{1}' is too old.", filePath, dataStartTime);
                            fileGroup = LoadFileGroup(fileInfo, filePath, xdaTimeZone);
                            fileGroup.ProcessingEndTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, xdaTimeZone);
                            fileGroup.Error = 1;
                            fileInfo.SubmitChanges();
                            return;
                        }

                        timeDifference = dataEndTime.Subtract(TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, xdaTimeZone)).TotalHours;

                        if (m_systemSettings.MaxTimeOffset > 0.0D && timeDifference > m_systemSettings.MaxTimeOffset)
                        {
                            OnStatusMessage("Skipped file \"{0}\" because data end time '{1}' is too far in the future.", filePath, dataStartTime);
                            fileGroup = LoadFileGroup(fileInfo, filePath, xdaTimeZone);
                            fileGroup.ProcessingEndTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, xdaTimeZone);
                            fileGroup.Error = 1;
                            fileInfo.SubmitChanges();
                            return;
                        }

                        // Process meter data using the meter data processor
                        meterDataProcessor.Process();

                        // Ownership of the dbAdapterContainer has passed to the meterDataProcessor
                        dbAdapterContainer = null;

                        ClearFileCreationTimes(filePath);
                    }
                    else
                    {
                        // Requeue the file if the reader is unable to process
                        fileProcessorEventArgs.Requeue = true;
                    }
                }
            }
            catch (Exception ex)
            {
                string message = string.Format("Failed to process file \"{0}\" due to exception: {1}", filePath, ex.Message);
                OnProcessException(new InvalidOperationException(message, ex));
            }
            finally
            {
                if ((object)dbAdapterContainer != null)
                    dbAdapterContainer.Dispose();
            }
        }

        // Called when the file processor encounters an unexpected error.
        private void FileProcessor_Error(object sender, ErrorEventArgs args)
        {
            OnProcessException(args.GetException());
        }

        private string LoadSystemSettings(SystemInfoDataContext systemInfo)
        {
            Dictionary<string, string> settingsDictionary;
            string dbConnectionString;
            string fileShares;

            // Retrieve the connection string from the active database connection
            dbConnectionString = systemInfo.Connection.ConnectionString;

            // Convert the Setting table to a dictionary
            settingsDictionary = systemInfo.Settings
                .Where(setting => setting.Name.IndexOf('.') < 0)
                .ToDictionary(setting => setting.Name, setting => setting.Value, StringComparer.OrdinalIgnoreCase);

            // Add the database connection string if there is not
            // already one explicitly specified in the Setting table
            if (!settingsDictionary.ContainsKey("dbConnectionString"))
                settingsDictionary["dbConnectionString"] = dbConnectionString;

            // Convert settings prefixed with "FileShare." to a nested connection string
            if (!settingsDictionary.ContainsKey("fileShares"))
            {
                fileShares = systemInfo.Settings.ToList()
                    .Where(setting => setting.Name.StartsWith("FileShare.", StringComparison.OrdinalIgnoreCase))
                    .Select(setting => Tuple.Create(setting.Name.Split('.'), setting.Value))
                    .Where(tuple => tuple.Item1.Length == 3)
                    .GroupBy(tuple => tuple.Item1[1])
                    .Select(grouping => grouping.ToDictionary(tuple => tuple.Item1[2], tuple => tuple.Item2))
                    .Select(dict => dict.JoinKeyValuePairs().Trim(' ', ';'))
                    .Select((shareSettings, index) => Tuple.Create(index, shareSettings))
                    .ToDictionary(tuple => tuple.Item1.ToString(), tuple => tuple.Item2)
                    .JoinKeyValuePairs();

                if (!string.IsNullOrEmpty(fileShares))
                    settingsDictionary["fileShares"] = fileShares;
            }

            // Convert dictionary to a connection string and return it
            return settingsDictionary.JoinKeyValuePairs();
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

        private void ShiftTime(MeterDataSet meterDataSet, TimeZoneInfo xdaTimeZone)
        {
            TimeZoneInfo meterTimeZone;

            if (!string.IsNullOrEmpty(meterDataSet.Meter.TimeZone))
                meterTimeZone = TimeZoneInfo.FindSystemTimeZoneById(meterDataSet.Meter.TimeZone);
            else
                meterTimeZone = TimeZoneInfo.FindSystemTimeZoneById(m_systemSettings.DefaultMeterTimeZone);

            foreach (DataSeries dataSeries in meterDataSet.DataSeries)
            {
                foreach (DataPoint dataPoint in dataSeries.DataPoints)
                {
                    dataPoint.Time = TimeZoneInfo.ConvertTimeToUtc(dataPoint.Time, meterTimeZone);
                    dataPoint.Time = TimeZoneInfo.ConvertTimeFromUtc(dataPoint.Time, xdaTimeZone);
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
                OnProcessException(ex);
                return null;
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

        private static TransactionOptions GetTransactionOptions()
        {
            return new TransactionOptions()
            {
                IsolationLevel = IsolationLevel.ReadCommitted,
                Timeout = TransactionManager.MaximumTimeout
            };
        }

        // Attempts to create the directory at the given path.
        private void TryCreateDirectory(string path)
        {
            try
            {
                // Make sure results directory exists
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
            }
            catch (Exception ex)
            {
                OnProcessException(new InvalidOperationException(string.Format("Failed to create directory \"{0}\" due to exception: {1}", FilePath.GetAbsolutePath(path), ex.Message), ex));
            }
        }

        // Attempts to parse the meter's asset key from the file path
        private bool TryParseFilePath(string fileName, out string meterKey)
        {
            Match match = Regex.Match(fileName, m_systemSettings.FilePattern);
            Group meterKeyGroup;

            if (match.Success)
            {
                meterKeyGroup = match.Groups["AssetKey"];

                if ((object)meterKeyGroup != null)
                {
                    meterKey = meterKeyGroup.Value;
                    return true;
                }
            }

            meterKey = null;
            return false;
        }

        // Displays status message to the console - proxy method for service implementation
        [StringFormatMethod("format")]
        private void OnStatusMessage(string format, params object[] args)
        {
            Log.Info(string.Format(format, args));
        }

        // Displays exception message to the console - proxy method for service implmentation
        private void OnProcessException(Exception ex)
        {
            Log.Error(ex.Message, ex);
        }

        // Attempts to dispose of the object if it is not null
        private void TryDispose(IDisposable obj)
        {
            try
            {
                if ((object)obj != null)
                    obj.Dispose();
            }
            catch (Exception ex)
            {
                OnProcessException(ex);
            }
        }

        #endregion

        #region [ Static ]

        // Static Fields
        private static readonly ILog Log = LogManager.GetLogger(typeof(ExtensibleDisturbanceAnalysisEngine));

        #endregion
    }
}
