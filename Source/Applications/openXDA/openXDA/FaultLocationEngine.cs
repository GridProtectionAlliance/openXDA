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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
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

        // Constants

        /// <summary>
        /// Globally unique identifier required by the file processor to identify its cached list of processed files.
        /// </summary>
        private static readonly Guid FileProcessorID = new Guid("4E3D3A90-6E7E-4AB7-96F3-3A5899081D0D");

        /// <summary>
        /// List of file extensions that can trigger file processing events.
        /// </summary>
        private static readonly string[] ValidExtensions = { ".pqd", ".dat", ".d00", ".rcd", ".rcl" };

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

            Dictionary<string, string> settingsDictionary;
            string fileShares;

            // Retrieve the connection string from the config file
            category.Add("ConnectionString", "Data Source=localhost; Initial Catalog=openXDA; Integrated Security=SSPI", "Defines the connection to the openXDA database.");
            connectionString = category["ConnectionString"].Value;

            // Get system settings from the database
            using (SystemInfoDataContext systemInfo = new SystemInfoDataContext(connectionString))
            {
                settingsDictionary = systemInfo.Settings
                    .Where(setting => setting.Name.IndexOf('.') < 0)
                    .ToDictionary(setting => setting.Name, setting => setting.Value);

                if (!settingsDictionary.ContainsKey("dbConnectionString"))
                    settingsDictionary["dbConnectionString"] = connectionString;

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
            }

            m_systemSettings = new SystemSettings(settingsDictionary.JoinKeyValuePairs());

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

            // Setup new file processor to monitor the watch directories
            if ((object)m_fileProcessor == null)
            {
                m_fileProcessor = new FileProcessor(FileProcessorID);
                m_fileProcessor.Filter = string.Join(Path.PathSeparator.ToString(), ValidExtensions.Select(ext => "*" + ext));
                m_fileProcessor.Processing += FileProcessor_Processing;
                m_fileProcessor.Error += FileProcessor_Error;

                foreach (string path in m_systemSettings.WatchDirectoryList)
                    m_fileProcessor.AddTrackedDirectory(path);
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

        // Attempts to create the directory at the given path.
        private void TryCreateDirectory(string path)
        {
            TryCreateDirectory(null, path);
        }

        // Attempts to create the directory at the given path.
        private void TryCreateDirectory(Logger logger, string path)
        {
            try
            {
                // Make sure results directory exists
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
            }
            catch (Exception ex)
            {
                OnProcessException(logger, new InvalidOperationException(string.Format("Failed to create directory \"{0}\" due to exception: {1}", FilePath.GetAbsolutePath(path), ex.Message), ex));
            }
        }

        // Determines whether the file on the given file path
        // can be processed at the time this method is called.
        private bool CanProcessFile(string filePath)
        {
            string directory;
            string rootFileName;
            string extension;

            string cfgFileName;
            TimeSpan timeSinceCreation;

            if ((object)filePath == null || !File.Exists(filePath))
                return false;

            rootFileName = FilePath.GetFileNameWithoutExtension(filePath);
            extension = FilePath.GetExtension(filePath).ToLowerInvariant().Trim();

            // If the data file is COMTRADE and the schema file does not exist, the file cannot be processed
            if (extension.Equals(".dat", StringComparison.OrdinalIgnoreCase) || extension.Equals(".d00", StringComparison.OrdinalIgnoreCase))
            {
                directory = FilePath.GetDirectoryName(filePath);
                cfgFileName = Path.Combine(directory, rootFileName + ".cfg");

                if (!File.Exists(cfgFileName))
                    return false;

                if (!FilePath.TryGetReadLockExclusive(cfgFileName))
                    return false;

                // If the extension for the data file is .d00, inject a delay before allowing the file to be processed
                if (extension.Equals(".d00", StringComparison.OrdinalIgnoreCase))
                {
                    timeSinceCreation = DateTime.Now - GetFileCreationTime(filePath);

                    if (timeSinceCreation.TotalSeconds < m_systemSettings.COMTRADEMinWaitTime)
                        return false;

                    foreach (string file in FilePath.GetFileList(rootFileName + ".d*"))
                    {
                        if (!FilePath.TryGetReadLockExclusive(file))
                            return false;
                    }
                }
            }

            // if the data file is EMAX and the control file does not exist, the file cannot be processed
            if (extension.Equals(".rcd", StringComparison.OrdinalIgnoreCase) || extension.Equals(".rcl", StringComparison.OrdinalIgnoreCase))
            {
                directory = FilePath.GetDirectoryName(filePath);
                cfgFileName = Path.Combine(directory, rootFileName + ".ctl");

                if (!File.Exists(cfgFileName))
                    return false;

                if (!FilePath.TryGetReadLockExclusive(cfgFileName))
                    return false;
            }

            return true;
        }

        // Processes the file on the given file path.
        private void ProcessFile(string filePath)
        {
            string meterKey;

            DbAdapterContainer dbAdapterContainer = null;

            string logFilePath;
            Logger logger = null;

            string extension;
            FileGroup fileGroup = null;

            IDataReader reader;
            List<MeterDataSet> meterDataSets;

            List<Type> dataOperationTypes;
            List<IDataOperation> dataOperations;

            ConnectionStringParser connectionStringParser;

            try
            {
                dbAdapterContainer = new DbAdapterContainer(m_systemSettings.DbConnectionString);
                fileGroup = LoadFileGroup(dbAdapterContainer.FileInfoAdapter, filePath);

                connectionStringParser = new ConnectionStringParser();
                connectionStringParser.SerializeUnspecifiedProperties = true;

                // Open a temp file to write log entries
                logFilePath = Path.GetTempFileName();

                if (m_systemSettings.DebugLevel > 0)
                    logger = Logger.Open(logFilePath);

                // Try to parse the name of the meter from the file path to determine whether this file needs to be parsed
                if (!string.IsNullOrEmpty(m_systemSettings.FilePattern) && TryParseFilePath(filePath, out meterKey))
                {
                    using (MeterInfoDataContext meterInfo = new MeterInfoDataContext(m_systemSettings.DbConnectionString))
                    {
                        if (!meterInfo.Meters.Any(m => m.AssetKey == meterKey))
                        {
                            OnStatusMessage(logger, "Skipped file \"{0}\" because no meter configuration was found for meter {1}.", filePath, meterKey);
                            fileGroup.ProcessingEndTime = DateTime.UtcNow;
                            dbAdapterContainer.FileInfoAdapter.SubmitChanges();
                            return;
                        }
                    }
                }

                OnStatusMessage(logger, "Found fault record \"{0}\".", filePath);

                // Determine whether the file is PQDIF, COMTRADE, or EMAX
                extension = FilePath.GetExtension(filePath);

                if (extension.Equals(".pqd", StringComparison.OrdinalIgnoreCase))
                    reader = new PQDIFReader();
                else if (extension.Equals(".dat", StringComparison.OrdinalIgnoreCase))
                    reader = new COMTRADEReader();
                else if (extension.Equals(".d00", StringComparison.OrdinalIgnoreCase))
                    reader = new COMTRADEReader();
                else if (extension.Equals(".rcd", StringComparison.OrdinalIgnoreCase))
                    reader = new EMAXReader();
                else if (extension.Equals(".rcl", StringComparison.OrdinalIgnoreCase))
                    reader = new EMAXReader();
                else
                    throw new InvalidOperationException(string.Format("Unable to parse file with unrecognized file extension '{0}'", extension));

                // Parse the file into a meter data set
                meterDataSets = reader.Parse(filePath);

                dataOperationTypes = dbAdapterContainer.SystemInfoAdapter.DataOperations
                    .OrderBy(dataOperation => dataOperation.LoadOrder)
                    .Select(dataOperation => LoadType(dataOperation.AssemblyName, dataOperation.TypeName))
                    .Where(type => (object)type != null)
                    .Where(type => typeof(IDataOperation).IsAssignableFrom(type))
                    .Where(type => (object)type.GetConstructor(Type.EmptyTypes) != null)
                    .ToList();

                foreach (MeterDataSet meterDataSet in meterDataSets)
                {
                    meterDataSet.FilePath = filePath;
                    meterDataSet.FileGroup = fileGroup;

                    dataOperations = dataOperationTypes
                        .Select(Activator.CreateInstance)
                        .Cast<IDataOperation>()
                        .ToList();

                    foreach (IDataOperation dataOperation in dataOperations)
                    {
                        // Attach to messaging events
                        dataOperation.StatusMessage += CreateMessageHandler(logger);
                        dataOperation.ProcessException += CreateExceptionHandler(logger);

                        // Provide system settings to the data operation
                        connectionStringParser.ParseConnectionString(m_systemSettings.ToConnectionString(), dataOperation);

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

                    // Export fault results to results files
                    WriteResults(dbAdapterContainer, meterDataSet, logger);
                }

                if ((object)logger != null)
                {
                    // Close the log file so we can
                    // move it to the debug directory
                    logger.Close();

                    // Move the file to the debug directory
                    string debugDir = Path.Combine(m_systemSettings.DebugPath, meterDataSets.First().Meter.AssetKey);
                    string debugPath = Path.Combine(debugDir, FilePath.GetFileNameWithoutExtension(filePath) + ".log");
                    TryCreateDirectory(debugDir);
                    File.Copy(logFilePath, FilePath.GetUniqueFilePathWithBinarySearch(debugPath), true);
                    File.Delete(logFilePath);
                }
            }
            catch (Exception ex)
            {
                try
                {
                    OnProcessException(logger, ex);

                    using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, GetTransactionOptions()))
                    using (FileInfoDataContext fileInfoDataContext = new FileInfoDataContext(m_systemSettings.DbConnectionString))
                    {
                        if ((object)fileGroup == null)
                            fileGroup = LoadFileGroup(fileInfoDataContext, filePath);

                        fileGroup.ProcessingEndTime = DateTime.UtcNow;
                        fileGroup.Error = 1;
                        fileInfoDataContext.SubmitChanges();
                        transactionScope.Complete();
                    }
                }
                catch
                {
                    // Ignore errors here as they are most likely
                    // related to the error we originally caught
                }
            }
            finally
            {
                if ((object)dbAdapterContainer != null)
                    dbAdapterContainer.Dispose();

                // Clean up the current log file
                if ((object)logger != null)
                {
                    try
                    {
                        logger.Close();

                        if (File.Exists(logger.LogFile))
                            File.Delete(logger.LogFile);
                    }
                    catch (Exception ex)
                    {
                        // The logger has already been closed so this
                        // error will go only to the status log and console
                        OnProcessException(ex);
                    }

                    logger.Dispose();
                }
            }

            OnStatusMessage("");
        }

        private void WriteResults(DbAdapterContainer dbAdapterContainer, MeterDataSet meterDataSet, Logger logger)
        {
            COMTRADEWriter comtradeWriter = new COMTRADEWriter(m_systemSettings.DbConnectionString);
            XMLWriter xmlWriter = new XMLWriter(m_systemSettings.DbConnectionString);
            EmailWriter emailWriter = new EmailWriter(m_systemSettings.DbConnectionString);

            int faultEventTypeID;
            MeterData.EventDataTable events;
            MeterData.EventTypeDataTable eventTypes;

            comtradeWriter.MaxFaultDistanceMultiplier = m_systemSettings.MaxFaultDistanceMultiplier;
            comtradeWriter.MinFaultDistanceMultiplier = m_systemSettings.MinFaultDistanceMultiplier;
            comtradeWriter.LengthUnits = m_systemSettings.LengthUnits;

            xmlWriter.MaxFaultDistanceMultiplier = m_systemSettings.MaxFaultDistanceMultiplier;
            xmlWriter.MinFaultDistanceMultiplier = m_systemSettings.MinFaultDistanceMultiplier;

            emailWriter.SMTPServer = m_systemSettings.SMTPServer;
            emailWriter.FromAddress = m_systemSettings.FromAddress;
            emailWriter.PQDashboardURL = m_systemSettings.PQDashboardURL;
            emailWriter.MaxFaultDistanceMultiplier = m_systemSettings.MaxFaultDistanceMultiplier;
            emailWriter.MinFaultDistanceMultiplier = m_systemSettings.MinFaultDistanceMultiplier;
            emailWriter.LengthUnits = m_systemSettings.LengthUnits;

            events = dbAdapterContainer.EventAdapter.GetDataByFileGroup(meterDataSet.FileGroup.ID);
            eventTypes = dbAdapterContainer.EventTypeAdapter.GetData();

            faultEventTypeID = eventTypes
                .Where(row => row.Name == "Fault")
                .Select(row => row.ID)
                .DefaultIfEmpty(0)
                .Single();

            foreach (MeterData.EventRow evt in events.Where(row => row.EventTypeID == faultEventTypeID))
            {
                Meter meter = dbAdapterContainer.MeterInfoAdapter.Meters.Single(m => m.ID == evt.MeterID);
                Line line = dbAdapterContainer.MeterInfoAdapter.Lines.Single(l => l.ID == evt.LineID);
                FaultLocationData.FaultSummaryDataTable faultSummaries = dbAdapterContainer.FaultSummaryAdapter.GetDataBy(evt.ID);

                string resultsDir;
                string comtradeFilePath;
                string xmlFilePath;

                if (faultSummaries.Count > 0)
                {
                    OnStatusMessage(logger, "Fault found on line {0} at {1} {2}", line.Name, faultSummaries.First().LargestCurrentDistance, m_systemSettings.LengthUnits);

                    resultsDir = Path.Combine(m_systemSettings.ResultsPath, meter.AssetKey);
                    TryCreateDirectory(resultsDir);

                    comtradeFilePath = Path.Combine(resultsDir, evt.ID.ToString("000000") + "_" + FilePath.GetFileNameWithoutExtension(meterDataSet.FilePath) + ".dat");
                    comtradeWriter.WriteResults(evt.ID, comtradeFilePath);
                    OnStatusMessage(logger, "Results written to {0}", comtradeFilePath);

                    xmlFilePath = Path.Combine(resultsDir, evt.ID.ToString("000000") + "_" + FilePath.GetFileNameWithoutExtension(meterDataSet.FilePath) + ".xml");
                    xmlWriter.WriteResults(evt.ID, xmlFilePath);
                    OnStatusMessage(logger, "Summary of results written to {0}", xmlFilePath);

                    emailWriter.WriteResults(evt.ID);
                    OnStatusMessage(logger, "Summary of results sent by email");
                }
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

        // Called when the file processor has picked up a file in one of the watch
        // directories. This handler validates the file and processes it if able.
        private void FileProcessor_Processing(object sender, FileProcessorEventArgs fileProcessorEventArgs)
        {
            string filePath = fileProcessorEventArgs.FullPath;

            try
            {
                if (fileProcessorEventArgs.AlreadyProcessed)
                {
                    using (FileInfoDataContext fileInfoDataContext = new FileInfoDataContext(m_systemSettings.DbConnectionString))
                    {
                        // Check that this file has not already been processed
                        if (fileInfoDataContext.DataFiles.Any(dataFile => dataFile.FilePath == filePath))
                        {
                            OnStatusMessage("Skipped file \"{0}\" because it has already been processed.", filePath);
                            return;
                        }
                    }
                }

                if (CanProcessFile(filePath))
                {
                    ProcessFile(filePath);
                    m_fileCreationTimes.Remove(filePath);
                }
                else
                {
                    fileProcessorEventArgs.Requeue = true;
                }
            }
            catch (Exception ex)
            {
                string message = string.Format("Unable to process file \"{0}\" due to exception: {1}", filePath, ex.Message);
                OnProcessException(new InvalidOperationException(message, ex));
            }
        }

        // Called when the file processor encounters an unexpected error.
        private void FileProcessor_Error(object sender, ErrorEventArgs args)
        {
            OnProcessException(args.GetException());
        }

        // Displays status message to the console - proxy method for service implementation
        [StringFormatMethod("format")]
        private void OnStatusMessage(string format, params object[] args)
        {
            OnStatusMessage(null, format, args);
        }

        // Displays status message to the console - proxy method for service implementation
        [StringFormatMethod("format")]
        private void OnStatusMessage(Logger logger, string format, params object[] args)
        {
            string message = string.Format(format, args);

            if ((object)logger != null)
                logger.WriteLine(message);

            if ((object)StatusMessage != null)
                StatusMessage(this, new EventArgs<string>(message));
        }

        // Displays exception message to the console - proxy method for service implmentation
        private void OnProcessException(Exception ex)
        {
            OnProcessException(null, ex);
        }

        // Displays exception message to the console - proxy method for service implmentation
        private void OnProcessException(Logger logger, Exception ex)
        {
            StringBuilder stackTrace;
            Exception inner;

            if ((object)logger != null)
            {
                stackTrace = new StringBuilder();
                inner = ex;

                while ((object)inner != null)
                {
                    stackTrace.AppendLine(inner.StackTrace);
                    inner = inner.InnerException;
                }

                logger.WriteLine(string.Empty);
                logger.WriteLine(string.Format("ERROR: {0}", ex.Message));
                logger.WriteLine(stackTrace.ToString());
                logger.WriteLine(string.Empty);
            }

            if ((object)ProcessException != null)
                ProcessException(this, new EventArgs<Exception>(ex));
        }

        private EventHandler<EventArgs<string>> CreateMessageHandler(Logger logger)
        {
            return (sender, args) => OnStatusMessage(logger, args.Argument);
        }

        private EventHandler<EventArgs<Exception>> CreateExceptionHandler(Logger logger)
        {
            return (sender, args) => OnProcessException(logger, args.Argument);
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

        private TransactionOptions GetTransactionOptions()
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
