//*********************************************************************************************************************
// FaultLocationEngine.cs
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
using FaultData.Database;
using FaultData.DataOperations;
using FaultData.DataReaders;
using FaultData.DataSets;
using FaultData.DataWriters;
using GSF;
using GSF.Annotations;
using GSF.Configuration;
using GSF.IO;
using openXDA.Configuration;
using FileShare = openXDA.Configuration.FileShare;

namespace openXDA
{
    /// <summary>
    /// Represents an engine that processes power quality data
    /// to determine the locations of faults along power lines.
    /// </summary>
    public class FaultLocationEngine : IDisposable
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
            public Logger Logger;

            public Action<string> StatusMessageHandler;
            public Action<Exception> ExceptionHandler;

            private int m_threadID;

            #endregion

            #region [ Methods ]

            public void Process()
            {
                BlockingCollection<int> threadIDs;
                Thread processThread;

                threadIDs = Interlocked.CompareExchange(ref s_threadIDs, null, null);
                m_threadID = threadIDs.Take();

                processThread = new Thread(() =>
                {
                    using (Logger)
                    {
                        OnStatusMessage("Processing meter data from file \"{0}\"...", MeterDataFile);

                        foreach (MeterDataSet meterDataSet in MeterDataSets)
                            ProcessMeterData(meterDataSet);

                        OnStatusMessage("Finished processing data from file \"{0}\".", MeterDataFile);
                    }

                    threadIDs.Add(m_threadID);
                });

                processThread.IsBackground = true;
                processThread.Start();
            }

            private void ProcessMeterData(MeterDataSet meterDataSet)
            {
                List<IDataOperation> dataOperations;
                ConnectionStringParser connectionStringParser;
                string systemSettings;

                try
                {
                    // Load data operations from the database
                    dataOperations = DbAdapterContainer.SystemInfoAdapter.DataOperations
                        .OrderBy(dataOperation => dataOperation.LoadOrder)
                        .Select(dataOperation => LoadType(dataOperation.AssemblyName, dataOperation.TypeName))
                        .Where(type => (object)type != null)
                        .Where(type => typeof(IDataOperation).IsAssignableFrom(type))
                        .Where(type => (object)type.GetConstructor(Type.EmptyTypes) != null)
                        .Select(Activator.CreateInstance)
                        .Cast<IDataOperation>()
                        .ToList();

                    systemSettings = SystemSettings.ToConnectionString();
                    connectionStringParser = new ConnectionStringParser();
                    connectionStringParser.SerializeUnspecifiedProperties = true;

                    foreach (IDataOperation dataOperation in dataOperations)
                    {
                        // Attach to messaging events
                        dataOperation.StatusMessage += (sender, args) => OnStatusMessage("{0}", args.Argument);
                        dataOperation.ProcessException += (sender, args) => OnHandleException(args.Argument);

                        // Provide system settings to the data operation
                        connectionStringParser.ParseConnectionString(systemSettings, dataOperation);

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

                    // Export fault results to results files
                    WriteResults(meterDataSet);
                }
                catch (Exception ex)
                {
                    try
                    {
                        OnHandleException(ex);
                        meterDataSet.FileGroup.ProcessingEndTime = DateTime.UtcNow;
                        meterDataSet.FileGroup.Error = 1;
                        DbAdapterContainer.FileInfoAdapter.SubmitChanges();
                    }
                    catch
                    {
                        // Ignore errors here as they are most likely
                        // related to the error we originally caught
                    }
                }

                OnStatusMessage("");
            }

            private void WriteResults(MeterDataSet meterDataSet)
            {
                COMTRADEWriter comtradeWriter = new COMTRADEWriter(SystemSettings.DbConnectionString);
                XMLWriter xmlWriter = new XMLWriter(SystemSettings.DbConnectionString);
                EmailWriter emailWriter = new EmailWriter(SystemSettings.DbConnectionString);

                int faultEventTypeID;
                MeterData.EventDataTable events;
                MeterData.EventTypeDataTable eventTypes;

                comtradeWriter.MaxFaultDistanceMultiplier = SystemSettings.MaxFaultDistanceMultiplier;
                comtradeWriter.MinFaultDistanceMultiplier = SystemSettings.MinFaultDistanceMultiplier;
                comtradeWriter.LengthUnits = SystemSettings.LengthUnits;

                xmlWriter.MaxFaultDistanceMultiplier = SystemSettings.MaxFaultDistanceMultiplier;
                xmlWriter.MinFaultDistanceMultiplier = SystemSettings.MinFaultDistanceMultiplier;

                emailWriter.SMTPServer = SystemSettings.SMTPServer;
                emailWriter.FromAddress = SystemSettings.FromAddress;
                emailWriter.PQDashboardURL = SystemSettings.PQDashboardURL;
                emailWriter.MaxFaultDistanceMultiplier = SystemSettings.MaxFaultDistanceMultiplier;
                emailWriter.MinFaultDistanceMultiplier = SystemSettings.MinFaultDistanceMultiplier;
                emailWriter.LengthUnits = SystemSettings.LengthUnits;

                events = DbAdapterContainer.EventAdapter.GetDataByFileGroup(meterDataSet.FileGroup.ID);
                eventTypes = DbAdapterContainer.EventTypeAdapter.GetData();

                faultEventTypeID = eventTypes
                    .Where(row => row.Name == "Fault")
                    .Select(row => row.ID)
                    .DefaultIfEmpty(0)
                    .Single();

                foreach (MeterData.EventRow evt in events.Where(row => row.EventTypeID == faultEventTypeID))
                {
                    Meter meter = DbAdapterContainer.MeterInfoAdapter.Meters.Single(m => m.ID == evt.MeterID);
                    Line line = DbAdapterContainer.MeterInfoAdapter.Lines.Single(l => l.ID == evt.LineID);
                    FaultLocationData.FaultSummaryDataTable faultSummaries = DbAdapterContainer.FaultSummaryAdapter.GetDataBy(evt.ID);

                    string resultsDir;
                    string comtradeFilePath;
                    string xmlFilePath;

                    if (faultSummaries.Count > 0)
                    {
                        OnStatusMessage("Fault found on line {0} at {1} {2}", line.Name, faultSummaries.First().LargestCurrentDistance, SystemSettings.LengthUnits);

                        resultsDir = Path.Combine(SystemSettings.ResultsPath, meter.AssetKey);
                        TryCreateDirectory(resultsDir);

                        comtradeFilePath = Path.Combine(resultsDir, evt.ID.ToString("000000") + "_" + FilePath.GetFileNameWithoutExtension(meterDataSet.FilePath) + ".dat");
                        comtradeWriter.WriteResults(evt.ID, comtradeFilePath);
                        OnStatusMessage("Results written to {0}", comtradeFilePath);

                        xmlFilePath = Path.Combine(resultsDir, evt.ID.ToString("000000") + "_" + FilePath.GetFileNameWithoutExtension(meterDataSet.FilePath) + ".xml");
                        xmlWriter.WriteResults(evt.ID, xmlFilePath);
                        OnStatusMessage("Summary of results written to {0}", xmlFilePath);

                        emailWriter.WriteResults(evt.ID);
                        OnStatusMessage("Summary of results sent by email");
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
                    OnHandleException(new InvalidOperationException(string.Format("Failed to create directory \"{0}\" due to exception: {1}", FilePath.GetAbsolutePath(path), ex.Message), ex));
                }
            }

            [StringFormatMethod("format")]
            private void OnStatusMessage(string format, params object[] args)
            {
                string message = string.Format(format, args);

                if ((object)Logger != null)
                    Logger.WriteLine(message);

                if ((object)StatusMessageHandler != null)
                    StatusMessageHandler(string.Format("[{0}] {1}", m_threadID, message));
            }

            private void OnHandleException(Exception ex)
            {
                if ((object)Logger != null)
                    Logger.WriteException(ex);

                if ((object)StatusMessageHandler != null)
                    StatusMessageHandler(string.Format("[{0}] ERROR: {1}", m_threadID, ex.Message));

                if ((object)ExceptionHandler != null)
                    ExceptionHandler(ex);
            }

            #endregion

            #region [ Static ]

            // Static Fields
            private static BlockingCollection<int> s_threadIDs;

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

        // Events

        /// <summary>
        /// Triggered when a message concerning the status
        /// of the fault location engine is encountered.
        /// </summary>
        public event EventHandler<EventArgs<string>> StatusMessage;

        /// <summary>
        /// Triggered when an exception is handled by the fault location engine.
        /// </summary>
        public event EventHandler<EventArgs<Exception>> ProcessException;

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
                    .Select(extension => string.Format("*.{0}", extension));
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

            // Make sure debug directory exists
            TryCreateDirectory(m_systemSettings.DebugPath);

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

            string meterKey;
            string extension;
            Type readerType;

            IDataReader reader;
            ConnectionStringParser connectionStringParser;

            SystemSettings systemSettings;
            MeterDataProcessor meterDataProcessor;

            filePath = fileProcessorEventArgs.FullPath;

            try
            {
                using (DbAdapterContainer dbAdapterContainer = new DbAdapterContainer(m_systemSettings.DbConnectionString))
                {
                    // Determine whether the file has already been processed
                    if (fileProcessorEventArgs.AlreadyProcessed)
                    {
                        if (dbAdapterContainer.FileInfoAdapter.DataFiles.Any(dataFile => dataFile.FilePath == filePath))
                        {
                            OnStatusMessage("Skipped file \"{0}\" because it has already been processed.", filePath);
                            return;
                        }
                    }

                    // Try to parse the name of the meter from the file path to determine whether this file can be parsed
                    if (string.IsNullOrEmpty(m_systemSettings.FilePattern) || !TryParseFilePath(filePath, out meterKey))
                    {
                        OnStatusMessage("Skipped file \"{0}\" because no meter could not be determined based on the FilePattern system setting.", filePath);
                        fileGroup = LoadFileGroup(dbAdapterContainer.FileInfoAdapter, filePath);
                        fileGroup.ProcessingEndTime = DateTime.UtcNow;
                        fileGroup.Error = 1;
                        dbAdapterContainer.FileInfoAdapter.SubmitChanges();
                        return;
                    }

                    // Attempt to find a meter in the database matching the meter name parsed from the file path
                    if (!dbAdapterContainer.MeterInfoAdapter.Meters.Any(m => m.AssetKey == meterKey))
                    {
                        OnStatusMessage("Skipped file \"{0}\" because no meter configuration was found for meter {1}.", filePath, meterKey);
                        fileGroup = LoadFileGroup(dbAdapterContainer.FileInfoAdapter, filePath);
                        fileGroup.ProcessingEndTime = DateTime.UtcNow;
                        fileGroup.Error = 1;
                        dbAdapterContainer.FileInfoAdapter.SubmitChanges();
                        return;
                    }

                    // Determine what data reader to use based on file extension
                    extension = FilePath.GetExtension(filePath);

                    // Load type used to parse this file from the database
                    readerType = dbAdapterContainer.SystemInfoAdapter.DataReaders
                        .Where(dataReader => extension.EndsWith(dataReader.FileExtension, StringComparison.OrdinalIgnoreCase))
                        .Select(dataReader => LoadType(dataReader.AssemblyName, dataReader.TypeName))
                        .Where(type => (object)type != null)
                        .Where(type => typeof(IDataReader).IsAssignableFrom(type))
                        .SingleOrDefault(type => (object)type.GetConstructor(Type.EmptyTypes) != null);

                    if ((object)readerType == null)
                    {
                        OnStatusMessage("Skipped file \"{0}\" because no valid data reader was found for file extension '{1}'.", filePath, extension);
                        fileGroup = LoadFileGroup(dbAdapterContainer.FileInfoAdapter, filePath);
                        fileGroup.ProcessingEndTime = DateTime.UtcNow;
                        fileGroup.Error = 1;
                        dbAdapterContainer.FileInfoAdapter.SubmitChanges();
                        return;
                    }

                    // Create the data reader
                    reader = (IDataReader)Activator.CreateInstance(readerType);
                    systemSettings = new SystemSettings(LoadSystemSettings(dbAdapterContainer.SystemInfoAdapter));

                    connectionStringParser = new ConnectionStringParser();
                    connectionStringParser.SerializeUnspecifiedProperties = true;
                    connectionStringParser.ParseConnectionString(systemSettings.ToConnectionString(), reader);

                    if (reader.CanParse(filePath, GetFileCreationTime(filePath)))
                    {
                        // Create a file group for this file in the database
                        fileGroup = LoadFileGroup(dbAdapterContainer.FileInfoAdapter, filePath);

                        // Create the meter data processor that will be processing this file
                        meterDataProcessor = new MeterDataProcessor();

                        // Initialize properties of the meter data processor
                        meterDataProcessor.SystemSettings = systemSettings;
                        meterDataProcessor.MeterDataFile = filePath;
                        meterDataProcessor.DbAdapterContainer = dbAdapterContainer;
                        meterDataProcessor.StatusMessageHandler = OnStatusMessage;
                        meterDataProcessor.ExceptionHandler = OnProcessException;

                        // Create the logger used to log messages from the meter data processor
                        if (systemSettings.DebugLevel > 0)
                            meterDataProcessor.Logger = CreateLogger(filePath, meterKey);

                        // Parse the file
                        meterDataProcessor.MeterDataSets = reader.Parse(filePath);

                        // Set properties on each of the meter data sets
                        foreach (MeterDataSet meterDataSet in meterDataProcessor.MeterDataSets)
                        {
                            meterDataSet.FilePath = filePath;
                            meterDataSet.FileGroup = fileGroup;
                            meterDataSet.Meter.AssetKey = meterKey;
                        }

                        // Process meter data using the meter data processor
                        meterDataProcessor.Process();

                        m_fileCreationTimes.Remove(filePath);
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
        }

        // Called when the file processor encounters an unexpected error.
        private void FileProcessor_Error(object sender, ErrorEventArgs args)
        {
            OnProcessException(args.GetException());
        }

        private Logger CreateLogger(string filePath, string meterKey)
        {
            string debugDir;
            string logFilePath;
            string logFileFullPath = null;
            Logger logger = null;

            try
            {
                while ((object)logger == null)
                {
                    try
                    {
                        debugDir = Path.Combine(m_systemSettings.DebugPath, meterKey);
                        logFilePath = Path.Combine(debugDir, FilePath.GetFileNameWithoutExtension(filePath) + ".log");
                        logFileFullPath = FilePath.GetUniqueFilePathWithBinarySearch(logFilePath);
                        logger = Logger.Open(logFileFullPath);
                    }
                    catch (IOException)
                    {
                        // Exceptions can occur if two threads are vying for the same file
                        // path where both will attempt to open the log and one will fail.
                        // If this happens, we can simply try again getting a different file path
                        if (!File.Exists(logFileFullPath))
                            throw;
                    }
                }
            }
            catch (Exception ex)
            {
                string message = string.Format("Unable to open log file due to exception: {0}", ex.Message);
                OnProcessException(new InvalidOperationException(message, ex));
            }

            return logger;
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
        private FileGroup LoadFileGroup(FileInfoDataContext dataContext, string filePath)
        {
            string directory = FilePath.GetDirectoryName(filePath);
            string rootFileName = FilePath.GetFileNameWithoutExtension(filePath);

            FileInfo fileInfo;
            FileGroup fileGroup;
            DataFile dataFile;

            fileGroup = new FileGroup();
            fileGroup.ProcessingStartTime = DateTime.UtcNow;

            foreach (string file in FilePath.GetFileList(Path.Combine(directory, rootFileName + ".*")))
            {
                fileInfo = new FileInfo(file);

                dataFile = new DataFile();
                dataFile.FilePath = file;
                dataFile.FileSize = fileInfo.Length;
                dataFile.CreationTime = fileInfo.CreationTimeUtc;
                dataFile.LastWriteTime = fileInfo.LastWriteTimeUtc;
                dataFile.LastAccessTime = fileInfo.LastAccessTimeUtc;
                dataFile.FileGroup = fileGroup;
            }

            dataContext.FileGroups.InsertOnSubmit(fileGroup);
            dataContext.SubmitChanges();

            return fileGroup;
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
        private DateTime GetFileCreationTime(string fileName)
        {
            DateTime creationTime;

            if (!m_fileCreationTimes.TryGetValue(fileName, out creationTime))
            {
                creationTime = DateTime.Now;
                m_fileCreationTimes.Add(fileName, creationTime);
            }

            return creationTime;
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
        private void OnStatusMessage(string message)
        {
            OnStatusMessage("{0}", message);
        }

        // Displays status message to the console - proxy method for service implementation
        [StringFormatMethod("format")]
        private void OnStatusMessage(string format, params object[] args)
        {
            string message = string.Format(format, args);

            if ((object)StatusMessage != null)
                StatusMessage(this, new EventArgs<string>(message));
        }

        // Displays exception message to the console - proxy method for service implmentation
        private void OnProcessException(Exception ex)
        {
            if ((object)ProcessException != null)
                ProcessException(this, new EventArgs<Exception>(ex));
        }

        #endregion
    }
}
