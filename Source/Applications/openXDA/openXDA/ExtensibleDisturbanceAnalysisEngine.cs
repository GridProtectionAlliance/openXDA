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
using System.ComponentModel;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Transactions;
using FaultData.Configuration;
using FaultData.DataAnalysis;
using FaultData.Database;
using FaultData.DataSets;
using GSF.Annotations;
using GSF.Collections;
using GSF.Configuration;
using GSF.Data;
using GSF.IO;
using GSF.Threading;
using log4net;
using openXDA.Configuration;
using openXDA.Model;
using DataContext = GSF.Web.Model.DataContext;
using FileShare = openXDA.Configuration.FileShare;
using Setting = FaultData.Database.Setting;

namespace openXDA
{
    /// <summary>
    /// Exception thrown when a file is skipped by the file processing engine.
    /// </summary>
    public class FileSkippedException : Exception
    {
        /// <summary>
        /// Creates a new instance of the <see cref="FileSkippedException"/> class.
        /// </summary>
        public FileSkippedException()
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="FileSkippedException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public FileSkippedException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="FileSkippedException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        public FileSkippedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }

    /// <summary>
    /// Represents an engine that processes power quality data
    /// to determine the locations of faults along power lines.
    /// </summary>
    public class ExtensibleDisturbanceAnalysisEngine : IDisposable
    {
        #region [ Members ]

        // Nested Types

        private class FileWrapper
        {
            #region [ Members ]

            // Fields
            private string m_filePath;
            private string m_directory;
            private string m_filePathWithoutExtension;
            private Dictionary<string, DateTime> m_fileCreationTimes;

            #endregion

            #region [ Constructors ]

            /// <summary>
            /// Creates a new instance of the <see cref="FileWrapper"/> class.
            /// </summary>
            /// <param name="filePath">The path to the wrapped file.</param>
            public FileWrapper(string filePath)
            {
                string rootFileName;

                m_filePath = filePath;
                m_directory = GSF.IO.FilePath.GetDirectoryName(filePath);
                rootFileName = GSF.IO.FilePath.GetFileNameWithoutExtension(filePath);
                m_filePathWithoutExtension = Path.Combine(m_directory, rootFileName);

                m_fileCreationTimes = new Dictionary<string, DateTime>();
            }

            #endregion

            #region [ Properties ]

            /// <summary>
            /// Gets the path to the wrapped file.
            /// </summary>
            public string FilePath
            {
                get
                {
                    return m_filePath;
                }
            }

            /// <summary>
            /// Gets the path to the directory containing the wrapped file.
            /// </summary>
            public string Directory
            {
                get
                {
                    return m_directory;
                }
            }

            #endregion

            #region [ Methods ]

            /// <summary>
            /// Gets the file group containing information about the file on
            /// the given file path, as well as the files related to it.
            /// </summary>
            /// <param name="dataContext">The data context used for database lookups.</param>
            /// <param name="xdaTimeZone">The time zone used by openXDA.</param>
            /// <returns></returns>
            public FaultData.Database.FileGroup GetFileGroup(FileInfoDataContext dataContext, TimeZoneInfo xdaTimeZone)
            {
                FileInfo fileInfo;
                FaultData.Database.FileGroup fileGroup;
                FaultData.Database.DataFile dataFile;

                fileGroup = new FaultData.Database.FileGroup();
                fileGroup.ProcessingStartTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, xdaTimeZone);

                foreach (string file in GSF.IO.FilePath.GetFileList($"{m_filePathWithoutExtension}.*"))
                {
                    fileInfo = new FileInfo(file);

                    dataFile = new FaultData.Database.DataFile();
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

            public openXDA.Model.FileGroup GetFileGroup(DataContext dataContext, TimeZoneInfo xdaTimeZone)
            {
                FileInfo fileInfo;
                Model.FileGroup fileGroup = new Model.FileGroup()
                {
                    ProcessingStartTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, xdaTimeZone),
                    ProcessingEndTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, xdaTimeZone),
                    DataEndTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, xdaTimeZone),
                    DataStartTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, xdaTimeZone),
                    Error = 0
                };

                dataContext.Table<Model.FileGroup>().AddNewRecord(fileGroup);
                fileGroup.ID = dataContext.Connection.ExecuteScalar<int>("SELECT @@IDENTITY");

                foreach (string file in GSF.IO.FilePath.GetFileList($"{m_filePathWithoutExtension}.*"))
                {
                    fileInfo = new FileInfo(file);

                    Model.DataFile dataFile = new Model.DataFile()
                    {
                        FilePath = file,
                        FilePathHash = file.GetHashCode(),
                        FileSize = fileInfo.Length,
                        CreationTime = TimeZoneInfo.ConvertTimeFromUtc(fileInfo.CreationTimeUtc, xdaTimeZone),
                        LastWriteTime = TimeZoneInfo.ConvertTimeFromUtc(fileInfo.LastWriteTimeUtc, xdaTimeZone),
                        LastAccessTime = TimeZoneInfo.ConvertTimeFromUtc(fileInfo.LastAccessTimeUtc, xdaTimeZone),
                        FileGroupID = fileGroup.ID
                    };
                    dataContext.Table<Model.DataFile>().AddNewRecord(dataFile);
                }

                return fileGroup;
            }

            /// <summary>
            /// Gets the maximum creation time of the files with the same root name as the wrapped file.
            /// </summary>
            /// <returns>The creation time of the file with the most recent creation time of the files with the same root name as the wrapped file.</returns>
            public DateTime GetMaxFileCreationTime()
            {
                string[] fileList = GSF.IO.FilePath.GetFileList($"{m_filePathWithoutExtension}.*");
                return fileList.Max(file => m_fileCreationTimes.GetOrAdd(file, path => DateTime.UtcNow));
            }

            #endregion
        }

        // Constants

        /// <summary>
        /// Globally unique identifier required by the file processor to identify its cached list of processed files.
        /// </summary>
        private static readonly Guid FileProcessorID = new Guid("4E3D3A90-6E7E-4AB7-96F3-3A5899081D0D");

        // Fields
        private string m_dbConnectionString;
        private SystemSettings m_systemSettings;

        private FileProcessor m_fileProcessor;
        private Dictionary<string, FileWrapper> m_fileWrapperLookup;
        private ConcurrentDictionary<string, string> m_activeFiles;

        private LogicalThreadScheduler m_meterDataScheduler;
        private Dictionary<string, LogicalThread> m_meterDataThreadLookup;
        private LogicalThread m_noMeterThread;

        private int m_queuedFileCount;

        private bool m_stopped;
        private bool m_disposed;

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Creates a new instance of the <see cref="ExtensibleDisturbanceAnalysisEngine"/> class.
        /// </summary>
        public ExtensibleDisturbanceAnalysisEngine()
        {
            m_stopped = true;
        }

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets the current status of the XDA engine.
        /// </summary>
        public string Status
        {
            get
            {
                SystemSettings systemSettings = m_systemSettings;
                StringBuilder statusBuilder = new StringBuilder();
                KeyValuePair<string, string>[] activeFiles;

                statusBuilder.AppendLine("Meter Data Status:");
                statusBuilder.AppendLine(new string('=', 50));
                statusBuilder.AppendLine($"          XDA Time Zone: {systemSettings.XDATimeZone}");
                statusBuilder.AppendLine($"       System frequency: {systemSettings.SystemFrequency} Hz");
                statusBuilder.AppendLine($"       Database Timeout: {systemSettings.DbTimeout} seconds");
                statusBuilder.AppendLine($"   Max thread pool size: {systemSettings.ProcessingThreadCount}");
                statusBuilder.AppendLine($"        Max Time Offset: {systemSettings.MaxTimeOffset} hours");
                statusBuilder.AppendLine($"        Min Time Offset: {systemSettings.MinTimeOffset} hours");
                statusBuilder.AppendLine($"      Max File Duration: {systemSettings.MaxFileDuration} seconds");
                statusBuilder.AppendLine($"   File Creation Offset: {systemSettings.MaxFileCreationTimeOffset} hours");
                statusBuilder.AppendLine($"      Queued file count: {Interlocked.CompareExchange(ref m_queuedFileCount, 0, 0)} / {systemSettings.MaxQueuedFileCount}");
                statusBuilder.AppendLine();

                activeFiles = m_activeFiles.ToArray();

                if (activeFiles.Any())
                {
                    statusBuilder.AppendLine("  Active Threads:");

                    foreach (KeyValuePair<string, string> kvp in m_activeFiles.ToArray())
                        statusBuilder.AppendLine($"    [{kvp.Key}] {kvp.Value}");

                    statusBuilder.AppendLine();
                }

                if (systemSettings.FileShareList.Any())
                {
                    statusBuilder.AppendLine("  File shares:");
                    
                    foreach (FileShare fileShare in systemSettings.FileShareList)
                    {
                        if ((object)fileShare.AuthenticationException == null)
                            statusBuilder.AppendLine($"    {fileShare.Name}");
                        else
                            statusBuilder.AppendLine($"    {fileShare.Name} [Exception: {fileShare.AuthenticationException.Message}]");
                    }

                    statusBuilder.AppendLine();
                }

                statusBuilder.AppendLine("File Processor Status:");
                statusBuilder.AppendLine(new string('=', 50));
                statusBuilder.AppendLine($"                 Filter: {m_fileProcessor.Filter}");
                statusBuilder.AppendLine($"   Internal buffer size: {m_fileProcessor.InternalBufferSize}");
                statusBuilder.AppendLine($"   Max thread pool size: {m_fileProcessor.MaxThreadCount}");
                statusBuilder.AppendLine($"      Max fragmentation: {m_fileProcessor.MaxFragmentation}");
                statusBuilder.AppendLine($"   Enumeration strategy: {m_fileProcessor.EnumerationStrategy}");
                statusBuilder.AppendLine($"    Enumeration threads: {m_fileProcessor.EnumerationThreads}");
                statusBuilder.AppendLine($"        Processed files: {m_fileProcessor.ProcessedFileCount}");
                statusBuilder.AppendLine($"          Skipped files: {m_fileProcessor.SkippedFileCount}");
                statusBuilder.AppendLine($"         Requeued files: {m_fileProcessor.RequeuedFileCount}");
                statusBuilder.AppendLine($"      Last Compact Time: {m_fileProcessor.LastCompactTime}");
                statusBuilder.AppendLine($"  Last Compact Duration: {m_fileProcessor.LastCompactDuration}");
                statusBuilder.AppendLine();

                statusBuilder.AppendLine("  Watch directories:");

                foreach (string path in m_fileProcessor.TrackedDirectories)
                    statusBuilder.AppendLine($"    {path}");

                return statusBuilder.ToString().TrimEnd();
            }
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Starts the fault location engine.
        /// </summary>
        public void Start()
        {
            IEnumerable<string> filterPatterns;

            // Get system settings from the database
            ReloadSystemSettings();

            // Get the list of file extensions to be processed by openXDA
            using (SystemInfoDataContext systemInfo = new SystemInfoDataContext(m_dbConnectionString))
            {
                filterPatterns = systemInfo.DataReaders
                    .Select(reader => reader.FilePattern)
                    .Select(extension => string.Format("*.{0}", extension))
                    .ToList();
            }

            // Reload configuration at startup
            ReloadConfiguration();

            // Make sure watch directories exist
            foreach (string path in m_systemSettings.WatchDirectoryList)
                TryCreateDirectory(path);

            // Make sure results directory exists
            TryCreateDirectory(m_systemSettings.ResultsPath);

            if ((object)m_fileWrapperLookup == null)
                m_fileWrapperLookup = new Dictionary<string, FileWrapper>();

            // Create the lookup table used to track which files are being processed
            if ((object)m_activeFiles == null)
                m_activeFiles = new ConcurrentDictionary<string, string>();

            // Create the scheduler used to schedule when to process meter data
            if ((object)m_meterDataScheduler == null)
            {
                m_meterDataScheduler = new LogicalThreadScheduler();
                m_meterDataThreadLookup = new Dictionary<string, LogicalThread>();

                m_meterDataScheduler.UnhandledException += (sender, args) =>
                {
                    string message = $"Unhandled exception occurred while processing meter data: {args.Argument.Message}";
                    Exception ex = new Exception(message, args.Argument);
                    OnProcessException(ex);
                };
            }

            m_meterDataScheduler.MaxThreadCount = m_systemSettings.ProcessingThreadCount;

            // Setup new file processor to monitor the watch directories
            if ((object)m_fileProcessor == null)
            {
                m_fileProcessor = new FileProcessor(FileProcessorID);
                m_fileProcessor.InternalBufferSize = m_systemSettings.FileWatcherBufferSize;
                m_fileProcessor.EnumerationStrategy = m_systemSettings.FileWatcherEnumerationStrategy;
                m_fileProcessor.MaxThreadCount = m_systemSettings.FileWatcherInternalThreadCount;
                m_fileProcessor.MaxFragmentation = m_systemSettings.FileWatcherMaxFragmentation;
                m_fileProcessor.FilterMethod = PrevalidateFile;
                m_fileProcessor.Processing += FileProcessor_Processing;
                m_fileProcessor.Error += FileProcessor_Error;

                UpdateFileProcessorFilter(m_systemSettings);
            }

            m_stopped = false;

            foreach (string path in m_systemSettings.WatchDirectoryList)
                m_fileProcessor.AddTrackedDirectory(path);
        }

        /// <summary>
        /// Reloads system configuration from configuration sources.
        /// </summary>
        public void ReloadConfiguration()
        {
            SystemInfoDataContext systemInfo;
            List<Type> types;
            string connectionString;

            IConfigurationLoader configurationLoader;

            // If system settings is null,
            // attempt to reload system settings
            if ((object)m_systemSettings == null)
                ReloadSystemSettings();

            // If system settings is still null, give up
            if ((object)m_systemSettings == null)
                return;

            using (DbAdapterContainer dbAdapterContainer = new DbAdapterContainer(m_systemSettings.DbConnectionString))
            {
                systemInfo = dbAdapterContainer.GetAdapter<SystemInfoDataContext>();

                types = systemInfo.ConfigurationLoaders
                    .OrderBy(configLoader => configLoader.LoadOrder)
                    .AsEnumerable()
                    .Select(configLoader => LoadType(configLoader.AssemblyName, configLoader.TypeName))
                    .Where(type => (object)type != null)
                    .Where(type => typeof(IConfigurationLoader).IsAssignableFrom(type))
                    .Where(type => (object)type.GetConstructor(Type.EmptyTypes) != null)
                    .ToList();

                connectionString = LoadSystemSettings(systemInfo);

                foreach (Type type in types)
                {
                    try
                    {
                        OnStatusMessage("[{0}] Loading configuration...", type.Name);

                        // Create an instance of the configuration loader
                        configurationLoader = (IConfigurationLoader)Activator.CreateInstance(type);

                        // Use the connection string parser to load system settings into the configuration loader
                        ConnectionStringParser.ParseConnectionString(connectionString, configurationLoader);

                        // Update configuration by calling the configuration loader's UpdateConfiguration method
                        configurationLoader.UpdateConfiguration(dbAdapterContainer);

                        OnStatusMessage("[{0}] Done loading configuration.", type.Name);
                    }
                    catch (Exception ex)
                    {
                        string message = string.Format("[{0}] Unable to update configuration due to exception: {1}", type.Name, ex.Message);
                        OnProcessException(new InvalidOperationException(message, ex));
                    }
                }
            }
        }

        /// <summary>
        /// Reloads system settings from the database.
        /// </summary>
        public void ReloadSystemSettings()
        {
            ConfigurationFile configurationFile;
            CategorizedSettingsElementCollection category;

            // Reload the configuration file
            configurationFile = ConfigurationFile.Current;
            configurationFile.Reload();
            AdoDataConnection.ReloadConfigurationSettings();

            // Retrieve the connection string from the config file
            category = configurationFile.Settings["systemSettings"];
            category.Add("ConnectionString", "Data Source=localhost; Initial Catalog=openXDA; Integrated Security=SSPI", "Defines the connection to the openXDA database.");
            m_dbConnectionString = category["ConnectionString"].Value;
            
            // Load system settings from the database
            m_systemSettings = new SystemSettings(LoadSystemSettings());

            // Update the limit on the number of processing threads
            if ((object)m_meterDataScheduler != null)
                m_meterDataScheduler.MaxThreadCount = m_systemSettings.ProcessingThreadCount;

            // Attempt to authenticate to configured file shares
            foreach (FileShare fileShare in m_systemSettings.FileShareList)
            {
                if (!fileShare.TryAuthenticate())
                    OnProcessException(fileShare.AuthenticationException);
            }

            // Update the FileProcessor with the latest system settings
            if ((object)m_fileProcessor != null)
            {
                m_fileProcessor.InternalBufferSize = m_systemSettings.FileWatcherBufferSize;
                m_fileProcessor.EnumerationStrategy = m_systemSettings.FileWatcherEnumerationStrategy;
                m_fileProcessor.MaxThreadCount = m_systemSettings.FileWatcherInternalThreadCount;
                m_fileProcessor.MaxFragmentation = m_systemSettings.FileWatcherMaxFragmentation;

                UpdateFileProcessorFilter(m_systemSettings);

                foreach (string directory in m_fileProcessor.TrackedDirectories.ToList())
                {
                    if (!m_systemSettings.WatchDirectoryList.Contains(directory, StringComparer.OrdinalIgnoreCase))
                        m_fileProcessor.RemoveTrackedDirectory(directory);
                }

                foreach (string directory in m_systemSettings.WatchDirectoryList)
                    m_fileProcessor.AddTrackedDirectory(directory);
            }
        }

        /// <summary>
        /// Tweaks the behavior of the file processor at runtime.
        /// </summary>
        /// <param name="args">The arguments supplied to the command to tweak the settings.</param>
        /// <returns>A message describing the change that was made.</returns>
        public string TweakFileProcessor(string[] args)
        {
            if (args.Length == 0 || args[0] == "-?")
            {
                StringBuilder helpMessage = new StringBuilder();

                helpMessage.Append("Modifies the behavior of the file processor at runtime.");
                helpMessage.AppendLine();
                helpMessage.AppendLine();
                helpMessage.Append("   Usage:");
                helpMessage.AppendLine();
                helpMessage.Append("       TweakFileProcessor { -Enumerate |");
                helpMessage.AppendLine();
                helpMessage.Append("                            -StopEnumeration |");
                helpMessage.AppendLine();
                helpMessage.Append("                            -Add <WatchDirectory> |");
                helpMessage.AppendLine();
                helpMessage.Append("                            -Remove <WatchDirectory> |");
                helpMessage.AppendLine();
                helpMessage.Append("                            -ListProperties |");
                helpMessage.AppendLine();
                helpMessage.Append("                            -Set <Property> <Value> }");
                helpMessage.AppendLine();
                helpMessage.AppendLine();
                helpMessage.Append("       TweakFileProcessor -?");
                helpMessage.AppendLine();
                helpMessage.AppendLine();
                helpMessage.Append("   Options:");
                helpMessage.AppendLine();
                helpMessage.Append("       -Enumerate".PadRight(25));
                helpMessage.Append("Initiates enumeration of the watch directories");
                helpMessage.AppendLine();
                helpMessage.Append("       -StopEnumeration".PadRight(25));
                helpMessage.Append("Stops enumeration of the watch directories");
                helpMessage.AppendLine();
                helpMessage.Append("       -Add".PadRight(25));
                helpMessage.Append("Adds a directory to the list of watch directories");
                helpMessage.AppendLine();
                helpMessage.Append("       -Remove".PadRight(25));
                helpMessage.Append("Removes a directory from the list of watch directories");
                helpMessage.AppendLine();
                helpMessage.Append("       -ListProperties".PadRight(25));
                helpMessage.Append("Lists properties that can be tweaked by this command");
                helpMessage.AppendLine();
                helpMessage.Append("       -Set".PadRight(25));
                helpMessage.Append("Tweaks a property of the file processor");
                helpMessage.AppendLine();
                helpMessage.Append("       -?".PadRight(25));
                helpMessage.Append("Displays this help message");

                return helpMessage.ToString();
            }

            if (args[0].Equals("-Enumerate", StringComparison.OrdinalIgnoreCase))
            {
                EnumerateWatchDirectories();
                return "Started enumeration of the watch directories.";
            }

            if (args[0].Equals("-StopEnumeration", StringComparison.OrdinalIgnoreCase))
            {
                m_fileProcessor.StopEnumeration();
                return "Stopped enumeration of the watch directories.";
            }

            if (args[0].Equals("-Add", StringComparison.OrdinalIgnoreCase))
            {
                if (args.Length < 2)
                    throw new FormatException("Malformed expression - Missing argument to 'TweakFileProcessor -Add <WatchDirectory>' command. Type 'TweakFileProcessor -?' to get help with this command.");

                m_fileProcessor.AddTrackedDirectory(args[1]);

                return $"Added directory '{args[1]}' to the file processor watch directories.";
            }

            if (args[0].Equals("-Remove", StringComparison.OrdinalIgnoreCase))
            {
                if (args.Length < 2)
                    throw new FormatException("Malformed expression - Missing argument to 'TweakFileProcessor -Remove <WatchDirectory>' command. Type 'TweakFileProcessor -?' to get help with this command.");

                m_fileProcessor.RemoveTrackedDirectory(args[1]);

                return $"Removed directory '{args[1]}' from the file processor watch directories.";
            }

            if (args[0].Equals("-ListProperties", StringComparison.OrdinalIgnoreCase))
            {
                StringBuilder propertyListBuilder = new StringBuilder();

                propertyListBuilder.AppendLine("File processor properties:");
                propertyListBuilder.AppendLine("  Filter");
                propertyListBuilder.AppendLine("  InternalBufferSize");
                propertyListBuilder.AppendLine("  MaxThreadCount");
                propertyListBuilder.AppendLine("  MaxFragmentation");
                propertyListBuilder.AppendLine("  EnumerationStrategy");

                return propertyListBuilder.ToString().TrimEnd();
            }

            if (args[0].Equals("-Set", StringComparison.OrdinalIgnoreCase))
            {
                string oldValue;

                if (args.Length < 3)
                    throw new FormatException("Malformed expression - Missing argument to 'TweakFileProcessor -Set <Property> <Value>' command. Type 'TweakFileProcessor -?' to get help with this command.");

                if (args[1].Equals("Filter", StringComparison.OrdinalIgnoreCase))
                {
                    oldValue = m_fileProcessor.Filter;
                    m_fileProcessor.Filter = args[2];
                }
                else if (args[1].Equals("InternalBufferSize", StringComparison.OrdinalIgnoreCase))
                {
                    int internalBufferSize;

                    if (!int.TryParse(args[2], out internalBufferSize))
                        throw new FormatException($"Malformed expression - Value for property 'InternalBufferSize' must be an integer. Type 'TweakFileProcessor -?' to get help with this command.");

                    oldValue = m_fileProcessor.InternalBufferSize.ToString();
                    m_fileProcessor.InternalBufferSize = internalBufferSize;
                }
                else if (args[1].Equals("MaxThreadCount", StringComparison.OrdinalIgnoreCase))
                {
                    int maxThreadCount;

                    if (!int.TryParse(args[2], out maxThreadCount))
                        throw new FormatException($"Malformed expression - Value for property 'MaxThreadCount' must be an integer. Type 'TweakFileProcessor -?' to get help with this command.");

                    oldValue = m_fileProcessor.MaxThreadCount.ToString();
                    m_fileProcessor.MaxThreadCount = maxThreadCount;
                }
                else if (args[1].Equals("MaxFragmentation", StringComparison.OrdinalIgnoreCase))
                {
                    int maxFragmentation;

                    if (!int.TryParse(args[2], out maxFragmentation))
                        throw new FormatException($"Malformed expression - Value for property 'MaxFragmentation' must be an integer. Type 'TweakFileProcessor -?' to get help with this command.");

                    oldValue = m_fileProcessor.MaxFragmentation.ToString();
                    m_fileProcessor.MaxFragmentation = maxFragmentation;
                }
                else if (args[1].Equals("EnumerationStrategy", StringComparison.OrdinalIgnoreCase))
                {
                    FileEnumerationStrategy enumerationStrategy;

                    if (!Enum.TryParse(args[2], out enumerationStrategy))
                        throw new FormatException($"Malformed expression - Unrecognized enumeration strategy '{args[2]}'. Type 'TweakFileProcessor -?' to get help with this command.");

                    oldValue = m_fileProcessor.EnumerationStrategy.ToString();
                    m_fileProcessor.EnumerationStrategy = enumerationStrategy;
                }
                else
                {
                    throw new FormatException($"Malformed expression - Unrecognized file processor property '{args[1]}'. Type 'TweakFileProcessor -?' to get help with this command.");
                }

                return $"Updated property '{args[1]}' from '{oldValue}' to '{args[2]}'.";
            }

            throw new FormatException($"Malformed expression - Unrecognized option '{args[1]}' supplied to the 'TweakFileProcessor' command. Type 'TweakFileProcessor -?' to get help with this command.");
        }

        /// <summary>
        /// Issues command to the file processor to enumerate the watch directories.
        /// </summary>
        public void EnumerateWatchDirectories()
        {
            if (m_fileProcessor.EnumerationThreads > 0)
                throw new InvalidOperationException("Enumeration of watch directories already in progress.");

            m_fileProcessor.EnumerateWatchDirectories();
        }

        /// <summary>
        /// Stops the fault location engine.
        /// </summary>
        public void Stop()
        {
            try
            {
                if ((object)m_fileProcessor != null)
                    m_fileProcessor.ClearTrackedDirectories();
            }
            finally
            {
                m_stopped = true;
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            if (m_disposed)
                return;

            try
            {
                Stop();

                // Stop file monitor timer
                if ((object)m_fileProcessor != null)
                {
                    m_fileProcessor.Processing -= FileProcessor_Processing;
                    m_fileProcessor.Error -= FileProcessor_Error;
                    m_fileProcessor.Dispose();
                    m_fileProcessor = null;
                }
            }
            finally
            {
                m_disposed = true;
            }
        }

        // Validates the file before invoking the file processing handler.
        // Improves file processor performance by executing the filter in
        // parallel and also by bypassing the set of processed files.
        private bool PrevalidateFile(string filePath)
        {
            try
            {
                string meterKey;
                string connectionString;
                SystemSettings systemSettings;

                connectionString = LoadSystemSettings();
                systemSettings = new SystemSettings(connectionString);
                ValidateFileCreationTime(filePath, systemSettings.MaxFileCreationTimeOffset);

                using (DbAdapterContainer dbAdapterContainer = new DbAdapterContainer(systemSettings.DbConnectionString, systemSettings.DbTimeout))
                {
                    meterKey = GetMeterKey(filePath, systemSettings.FilePattern);
                    ValidateMeterKey(filePath, meterKey, dbAdapterContainer.GetAdapter<MeterInfoDataContext>());
                }

                return true;
            }
            catch (FileSkippedException ex)
            {
                // This method may be called if the file was deleted,
                // in which case the user almost certainly doesn't care
                // why it was skipped for processing and logging the
                // error would only cause confusion
                if (File.Exists(filePath))
                    Log.Warn(ex.Message);

                return false;
            }
        }

        // Called when the file processor has picked up a file in one of the watch
        // directories. This handler validates the file and processes it if able.
        private void FileProcessor_Processing(object sender, FileProcessorEventArgs fileProcessorEventArgs)
        {
            if (m_disposed)
                return;

            try
            {
                string filePath;
                string connectionString;
                SystemSettings systemSettings;

                filePath = fileProcessorEventArgs.FullPath;

                if (!FilePath.TryGetReadLockExclusive(filePath))
                {
                    fileProcessorEventArgs.Requeue = true;
                    return;
                }

                connectionString = LoadSystemSettings();
                systemSettings = new SystemSettings(connectionString);

                using (DbAdapterContainer dbAdapterContainer = new DbAdapterContainer(systemSettings.DbConnectionString, systemSettings.DbTimeout))
                {
                    try
                    {
                        ProcessFile(
                            fileProcessorArgs: fileProcessorEventArgs,
                            connectionString: connectionString,
                            systemSettings: systemSettings,
                            dbAdapterContainer: dbAdapterContainer);
                    }
                    catch (Exception ex)
                    {
                        // There may be a problem here where the outer exception's call stack
                        // was overwritten by the call stack of the point where it was thrown
                        ExceptionDispatchInfo exInfo = ExceptionDispatchInfo.Capture(ex);

                        try
                        {
                            // Attempt to set the error flag on the file group
                            FileInfoDataContext fileInfo = dbAdapterContainer.GetAdapter<FileInfoDataContext>();
                            FileWrapper fileWrapper = m_fileWrapperLookup.GetOrAdd(filePath, path => new FileWrapper(path));
                            FaultData.Database.FileGroup fileGroup = fileWrapper.GetFileGroup(fileInfo, systemSettings.XDATimeZoneInfo);
                            fileGroup.ProcessingEndTime = fileGroup.ProcessingStartTime;
                            fileGroup.Error = 1;
                            fileInfo.SubmitChanges();
                        }
                        catch (Exception fileGroupError)
                        {
                            // Log exceptions that occur when setting the error flag on the file group
                            string message = $"Exception occurred setting error flag on file group: {fileGroupError.Message}";
                            OnProcessException(new Exception(message, fileGroupError));
                        }

                        // Throw the original exception
                        exInfo.Throw();
                    }
                }
            }
            catch (FileSkippedException)
            {
                // Do not wrap FileSkippedExceptions because
                // these only generate warning messages
                throw;
            }
            catch (Exception ex)
            {
                // Wrap all other exceptions to include the file path in the message
                string message = $"Exception occurred processing file \"{fileProcessorEventArgs.FullPath}\": {ex.Message}";
                throw new Exception(message, ex);
            }
            finally
            {
                // Make sure to clean up file wrappers from
                // the lookup table to prevent memory leaks
                if (!fileProcessorEventArgs.Requeue)
                    m_fileWrapperLookup.Remove(fileProcessorEventArgs.FullPath);
            }
        }

        // Processes the file to determine if it can be parsed and kicks off the meter's processing thread.
        private void ProcessFile(FileProcessorEventArgs fileProcessorArgs, string connectionString, SystemSettings systemSettings, DbAdapterContainer dbAdapterContainer)
        {
            string filePath;
            string meterKey;

            FileInfoDataContext fileInfo;
            SystemInfoDataContext systemInfo;

            FaultData.Database.DataReader dataReader;
            DataReaderWrapper dataReaderWrapper;
            FileWrapper fileWrapper;
            int queuedFileCount;

            filePath = fileProcessorArgs.FullPath;
            fileInfo = dbAdapterContainer.GetAdapter<FileInfoDataContext>();

            // Determine whether the file has already been
            // processed or needs to be processed again
            if (fileProcessorArgs.AlreadyProcessed)
            {
                FaultData.Database.DataFile dataFile = fileInfo.DataFiles
                    .Where(file => file.FilePathHash == filePath.GetHashCode())
                    .Where(file => file.FilePath == filePath)
                    .MaxBy(file => file.ID);

                // This will tell us whether the service was stopped in the middle
                // of processing the last time it attempted to process the file
                if ((object)dataFile != null && dataFile.FileGroup.ProcessingEndTime > DateTime.MinValue)
                {
                    Log.Debug($"Skipped file \"{filePath}\" because it has already been processed.");
                    return;
                }
            }

            // Get the data reader that will be used to parse the file
            systemInfo = dbAdapterContainer.GetAdapter<SystemInfoDataContext>();

            dataReader = systemInfo.DataReaders
                .OrderBy(reader => reader.LoadOrder)
                .AsEnumerable()
                .FirstOrDefault(reader => FilePath.IsFilePatternMatch(reader.FilePattern, filePath, true));

            if ((object)dataReader == null)
            {
                // Because the file processor is filtering files based on the DataReader file patterns,
                // this should only ever occur if the configuration changes during runtime
                UpdateFileProcessorFilter(systemSettings);

                throw new FileSkippedException($"Skipped file \"{filePath}\" because no data reader could be found to process the file.");
            }

            dataReaderWrapper = Wrap(dataReader);

            try
            {
                meterKey = null;

                // Determine whether the database contains configuration information for the meter that produced this file
                if ((object)dataReaderWrapper.DataObject.MeterDataSet != null)
                    meterKey = GetMeterKey(filePath, systemSettings.FilePattern);

                // Apply connection string settings to the data reader
                ConnectionStringParser.ParseConnectionString(connectionString, dataReaderWrapper.DataObject);

                // Get the file wrapper from the lookup table
                fileWrapper = m_fileWrapperLookup.GetOrAdd(filePath, path => new FileWrapper(path));

                // Determine whether the dataReader can parse the file
                if (!dataReaderWrapper.DataObject.CanParse(filePath, fileWrapper.GetMaxFileCreationTime()))
                {
                    fileProcessorArgs.Requeue = true;
                    dataReaderWrapper.Dispose();
                    return;
                }

                // Get the thread used to process this data
                GetThread(meterKey).Push(() =>
                {
                    ParseFile(connectionString, systemSettings, filePath, meterKey, dataReaderWrapper, fileWrapper);
                });

                // Keep track of the number of operations in thread queues
                queuedFileCount = Interlocked.Increment(ref m_queuedFileCount);

                while (!m_stopped && !m_disposed && m_queuedFileCount >= systemSettings.MaxQueuedFileCount)
                    Thread.Sleep(1000);
            }
            catch
            {
                // If an error occurs here, dispose of the data reader;
                // otherwise, the meter data thread will handle it
                dataReaderWrapper.Dispose();
                throw;
            }
        }

        // Parses the file on the meter's processing thread and kicks off processing of the meter data set.
        private void ParseFile(string connectionString, SystemSettings systemSettings, string filePath, string meterKey, DataReaderWrapper dataReaderWrapper, FileWrapper fileWrapper)
        {
            FaultData.Database.FileGroup fileGroup = null;
            MeterDataSet meterDataSet;
            int queuedFileCount;

            // Keep track of the number of operations in thread queues
            queuedFileCount = Interlocked.Decrement(ref m_queuedFileCount);

            if (m_stopped || m_disposed)
            {
                dataReaderWrapper.Dispose();
                return;
            }

            using (dataReaderWrapper)
            using (DbAdapterContainer dbAdapterContainer = new DbAdapterContainer(systemSettings.DbConnectionString, systemSettings.DbTimeout))
            {
                try
                {
                    // Keep track of the meters and files currently being processed
                    if ((object)meterKey != null)
                        m_activeFiles[meterKey] = filePath;

                    ThreadContext.Properties["Meter"] = meterKey;

                    // Create the file group
                    fileGroup = fileWrapper.GetFileGroup(dbAdapterContainer.GetAdapter<FileInfoDataContext>(), systemSettings.XDATimeZoneInfo);
                    LoadFileBlob(fileGroup.DataFiles.ToList());

                    // Parse the file to turn it into a meter data set
                    OnStatusMessage($"Parsing data from file \"{filePath}\"...");
                    dataReaderWrapper.DataObject.Parse(filePath);
                    OnStatusMessage($"Finished parsing data from file \"{filePath}\".");
                    meterDataSet = dataReaderWrapper.DataObject.MeterDataSet;

                    // If the data reader does not return a data set,
                    // there is nothing left to do
                    if ((object)meterDataSet == null)
                        return;

                    // Data reader has finally outlived its usefulness
                    dataReaderWrapper.Dispose();

                    // Set file path, file group, connection string,
                    // and meter asset key for the meter data set
                    meterDataSet.FilePath = filePath;
                    meterDataSet.FileGroup = fileGroup;
                    meterDataSet.ConnectionString = connectionString;
                    meterDataSet.Meter.AssetKey = meterKey;

                    // Shift date/time values to the configured time zone and set the start and end time values on the file group
                    ShiftTime(meterDataSet, meterDataSet.Meter.GetTimeZoneInfo(systemSettings.DefaultMeterTimeZoneInfo), systemSettings.XDATimeZoneInfo);
                    SetDataTimeRange(meterDataSet, dbAdapterContainer.GetAdapter<FileInfoDataContext>());

                    // Determine whether the file duration is within a user-defined maximum tolerance
                    ValidateFileDuration(meterDataSet.FilePath, systemSettings.MaxFileDuration, meterDataSet.FileGroup);

                    // Determine whether the timestamps in the file extend beyond user-defined thresholds
                    ValidateFileTimestamps(meterDataSet.FilePath, meterDataSet.FileGroup, systemSettings, dbAdapterContainer.GetAdapter<FileInfoDataContext>());

                    // Process the meter data set
                    OnStatusMessage($"Processing meter data from file \"{filePath}\"...");
                    ProcessMeterDataSet(meterDataSet, dbAdapterContainer);
                    OnStatusMessage($"Finished processing data from file \"{filePath}\".");
                }
                catch (Exception ex)
                {
                    // There seems to be a problem here where the outer exception's call stack
                    // was overwritten by the call stack of the point where it was thrown
                    ExceptionDispatchInfo exInfo = ExceptionDispatchInfo.Capture(ex);

                    try
                    {
                        // Attempt to set the error flag on the file group
                        if ((object)fileGroup != null)
                            fileGroup.Error = 1;
                    }
                    catch (Exception fileGroupError)
                    {
                        // Log any exceptions that occur when attempting to set the error flag on the file group
                        string message = $"Exception occurred setting error flag on file group: {fileGroupError.Message}";
                        OnProcessException(new Exception(message, fileGroupError));
                    }

                    // Throw the original exception
                    exInfo.Throw();
                }
                finally
                {
                    if ((object)fileGroup != null)
                    {
                        try
                        {
                            // Attempt to set the processing end time of the file group
                            fileGroup.ProcessingEndTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, systemSettings.XDATimeZoneInfo);
                            dbAdapterContainer.GetAdapter<FileInfoDataContext>().SubmitChanges();
                        }
                        catch (Exception ex)
                        {
                            // Log any exceptions that occur when attempting to set processing end time on the file group
                            string message = $"Exception occurred setting processing end time on file group: {ex.Message}";
                            OnProcessException(new Exception(message, ex));
                        }
                    }

                    // Keep track of the meters and files currently being processed
                    if ((object)meterKey != null)
                        m_activeFiles.TryRemove(meterKey, out filePath);

                    ThreadContext.Properties.Remove("Meter");
                }
            }
        }


        public void ReprocessFiles(Dictionary<int, int> fileGroups)
        {
            string connectionString = LoadSystemSettings();
            SystemSettings systemSettings = new SystemSettings(connectionString);

            foreach (var fileGroup in fileGroups)
            {
                string meterKey = "";
                using (AdoDataConnection adoDataConnection = new AdoDataConnection(systemSettings.DbConnectionString, typeof(SqlConnection),typeof(SqlDataAdapter)))
                {
                    meterKey = adoDataConnection.ExecuteScalar<string>("SELECT AssetKey FROM Meter WHERE ID = {0}", fileGroup.Value);
                }
                GetThread(meterKey).Push(() =>
                {
                    using (DbAdapterContainer dbAdapterContainer = new DbAdapterContainer(systemSettings.DbConnectionString, systemSettings.DbTimeout))
                    {

                        ReparseFiles(fileGroup.Key, meterKey, dbAdapterContainer, systemSettings);
                    }
                });
            }
            


        }

        public void ReprocessFile(int dataFileId, int fileGroupId, int meterId)
        {
            string connectionString = LoadSystemSettings();
            SystemSettings systemSettings = new SystemSettings(connectionString);

            string meterKey = "";
            using (AdoDataConnection adoDataConnection = new AdoDataConnection(systemSettings.DbConnectionString, typeof(SqlConnection), typeof(SqlDataAdapter)))
            {
                meterKey = adoDataConnection.ExecuteScalar<string>("SELECT AssetKey FROM METER WHERE ID = {0}", meterId);
            }
            GetThread(meterKey).Push(() =>
            {
                using (DbAdapterContainer dbAdapterContainer = new DbAdapterContainer(systemSettings.DbConnectionString, systemSettings.DbTimeout))
                {

                    ReparseFile(dataFileId, meterKey, dbAdapterContainer, systemSettings);
                }
            });
        }


        private void ReparseFiles(int fileGroupID, string meterKey, DbAdapterContainer dbAdapterContainer, SystemSettings systemSettings)
        {
            using (DataContext dataContext = new DataContext(new AdoDataConnection(dbAdapterContainer.Connection, typeof(SqlDataAdapter), false)))
            {
                SystemInfoDataContext systemInfo = dbAdapterContainer.GetAdapter<SystemInfoDataContext>();
                FileInfoDataContext fileInfoDataContext = dbAdapterContainer.GetAdapter<FileInfoDataContext>();
                MeterInfoDataContext meterInfoDataContext = dbAdapterContainer.GetAdapter<MeterInfoDataContext>();
                FaultData.Database.FileGroup fileGroup = fileInfoDataContext.FileGroups.Single(fg => fg.ID == fileGroupID);
                FaultData.Database.Meter meter = meterInfoDataContext.Meters.Single(m => m.AssetKey == meterKey);
                foreach (var dataFile in fileGroup.DataFiles)
                {
                    Byte[] blob = dataContext.Connection.ExecuteScalar<byte[]>("SELECT Blob FROM FileBlob WHERE DataFileID = {0}", dataFile.ID);
                    if (blob == null)
                    {
                        try
                        {
                            LoadFileBlob(fileGroup.DataFiles.ToList());
                        }
                        catch(Exception ex)
                        {
                            OnProcessException(ex);
                            return;
                        }
                        blob = dataContext.Connection.ExecuteScalar<byte[]>("SELECT Blob FROM FileBlob WHERE DataFileID = {0}", dataFile.ID);
                    }

                    DirectoryInfo directory = Directory.CreateDirectory(Path.Combine(Path.GetTempPath(), "openXDA"));
                    string filePath = Path.Combine(directory.FullName, dataFile.FilePath.Split('\\')[dataFile.FilePath.Split('\\').Length - 1]);
                    File.WriteAllBytes(filePath, blob);
                    try
                    {

                        FaultData.Database.DataReader dataReader = systemInfo.DataReaders
                            .OrderBy(reader => reader.LoadOrder)
                            .AsEnumerable()
                            .FirstOrDefault(reader => FilePath.IsFilePatternMatch(reader.FilePattern, filePath, true));

                        if ((object)dataReader == null)
                            continue;
                        // Keep track of the meters and files currently being processed
                        if ((object)meter.AssetKey != null)
                            m_activeFiles[meter.AssetKey] = filePath;

                        ThreadContext.Properties["Meter"] = meter.AssetKey;

                        // Create the file group
                        using (DataReaderWrapper dataReaderWrapper = Wrap(dataReader))
                        {
                            // Parse the file to turn it into a meter data set
                            OnStatusMessage($"Parsing data from file \"{filePath}\"...");
                            if (dataReaderWrapper.DataObject.CanParse(filePath, DateTime.UtcNow))
                                dataReaderWrapper.DataObject.Parse(filePath);
                            OnStatusMessage($"Finished parsing data from file \"{filePath}\".");
                            MeterDataSet meterDataSet = dataReaderWrapper.DataObject.MeterDataSet;

                            // If the data reader does not return a data set,
                            // there is nothing left to do
                            if ((object)meterDataSet == null)
                                return;

                            // Set file path, file group, connection string,
                            // and meter asset key for the meter data set
                            meterDataSet.FilePath = dataFile.FilePath;
                            meterDataSet.FileGroup = fileGroup;
                            meterDataSet.Meter = meter;
                            meterDataSet.ConnectionString = LoadSystemSettings();

                            // Data reader has finally outlived its usefulness
                            dataReaderWrapper.Dispose();

                            // Shift date/time values to the configured time zone and set the start and end time values on the file group
                            ShiftTime(meterDataSet, meterDataSet.Meter.GetTimeZoneInfo(systemSettings.DefaultMeterTimeZoneInfo), systemSettings.XDATimeZoneInfo);
                            SetDataTimeRange(meterDataSet.DataSeries, meterDataSet.Digitals, fileGroup.ID,dataContext);

                            // Determine whether the file duration is within a user-defined maximum tolerance
                            ValidateFileDuration(meterDataSet.FilePath, systemSettings.MaxFileDuration, meterDataSet.FileGroup.DataStartTime, meterDataSet.FileGroup.DataEndTime);

                            // Determine whether the timestamps in the file extend beyond user-defined thresholds
                            ValidateFileTimestamps(meterDataSet.FilePath, meterDataSet.FileGroup.DataStartTime, meterDataSet.FileGroup.DataEndTime, systemSettings);

                            // Process the meter data set
                            OnStatusMessage($"Processing meter data from file \"{filePath}\"...");
                            ProcessMeterDataSet(meterDataSet, dbAdapterContainer);
                            OnStatusMessage($"Finished processing data from file \"{filePath}\".");

                        }
                    }
                    catch (Exception ex)
                    {
                        // There seems to be a problem here where the outer exception's call stack
                        // was overwritten by the call stack of the point where it was thrown
                        ExceptionDispatchInfo exInfo = ExceptionDispatchInfo.Capture(ex);

                        try
                        {
                            // Attempt to set the error flag on the file group
                            dataContext.Connection.ExecuteNonQuery("UPDATE FileGroup SET Error = 1 WHERE ID = {0}", fileGroup.ID);
                        }
                        catch (Exception fileGroupError)
                        {
                            // Log any exceptions that occur when attempting to set the error flag on the file group
                            string message = $"Exception occurred setting error flag on file group: {fileGroupError.Message}";
                            OnProcessException(new Exception(message, fileGroupError));
                        }
                        OnProcessException(ex);

                        // Throw the original exception
                        exInfo.Throw();
                    }
                    finally
                    {
                        try
                        {
                            // Attempt to set the processing end time of the file group
                            dataContext.Connection.ExecuteNonQuery("UPDATE FileGroup SET ProcessingEndTime = {0} WHERE ID = {1}", TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, systemSettings.XDATimeZoneInfo), fileGroup.ID);

                        }
                        catch (Exception ex)
                        {
                            // Log any exceptions that occur when attempting to set processing end time on the file group
                            string message = $"Exception occurred setting processing end time on file group: {ex.Message}";
                            OnProcessException(new Exception(message, ex));
                        }

                        // Keep track of the meters and files currently being processed
                        if ((object)meter.AssetKey != null)
                            m_activeFiles.TryRemove(meter.AssetKey, out filePath);

                        ThreadContext.Properties.Remove("Meter");
                    }
                    File.Delete(filePath);
                }
            }
        }
        

        private void ReparseFile(int dataFileID, string meterKey, DbAdapterContainer dbAdapterContainer, SystemSettings systemSettings)
        {
            using (DataContext dataContext = new DataContext(new AdoDataConnection(dbAdapterContainer.Connection, typeof(SqlDataAdapter), false)))
            {
                SystemInfoDataContext systemInfo = dbAdapterContainer.GetAdapter<SystemInfoDataContext>();
                FileInfoDataContext fileInfoDataContext = dbAdapterContainer.GetAdapter<FileInfoDataContext>();
                MeterInfoDataContext meterInfoDataContext = dbAdapterContainer.GetAdapter<MeterInfoDataContext>();
                int fileGroupID = dataContext.Connection.ExecuteScalar<int>("SELECT FileGroupID FROM DataFile WHERE ID = {0}", dataFileID);
                FaultData.Database.FileGroup fileGroup = fileInfoDataContext.FileGroups.Single(fg => fg.ID == fileGroupID);
                FaultData.Database.Meter meter = meterInfoDataContext.Meters.Single(m => m.AssetKey == meterKey );
                FaultData.Database.DataFile dataFile = fileGroup.DataFiles.Single(df => df.ID == dataFileID);
                Byte[] blob = dataContext.Connection.ExecuteScalar<byte[]>("SELECT Blob FROM FileBlob WHERE DataFileID = {0}", dataFile.ID);

                if (blob == null)
                {
                    try
                    {
                        LoadFileBlob(fileGroup.DataFiles.ToList());
                    }
                    catch (Exception ex)
                    {
                        OnProcessException(ex);
                        return;
                    }
                    blob = dataContext.Connection.ExecuteScalar<byte[]>("SELECT Blob FROM FileBlob WHERE DataFileID = {0}", dataFile.ID);
                }

                DirectoryInfo directory = Directory.CreateDirectory(Path.Combine(Path.GetTempPath(), "openXDA"));
                string filePath = Path.Combine(directory.FullName, dataFile.FilePath.Split('\\')[dataFile.FilePath.Split('\\').Length - 1]);
                File.WriteAllBytes(filePath, blob);
                try
                {

                    FaultData.Database.DataReader dataReader = systemInfo.DataReaders
                        .OrderBy(reader => reader.LoadOrder)
                        .AsEnumerable()
                        .FirstOrDefault(reader => FilePath.IsFilePatternMatch(reader.FilePattern, filePath, true));

                    // Keep track of the meters and files currently being processed
                    if ((object)meter.AssetKey != null)
                        m_activeFiles[meter.AssetKey] = filePath;

                    ThreadContext.Properties["Meter"] = meter.AssetKey;

                    // Create the file group
                    using (DataReaderWrapper dataReaderWrapper = Wrap(dataReader))
                    {
                        // Parse the file to turn it into a meter data set
                        OnStatusMessage($"Parsing data from file \"{filePath}\"...");
                        if (dataReaderWrapper.DataObject.CanParse(filePath, DateTime.UtcNow))
                            dataReaderWrapper.DataObject.Parse(filePath);
                        OnStatusMessage($"Finished parsing data from file \"{filePath}\".");
                        MeterDataSet meterDataSet = dataReaderWrapper.DataObject.MeterDataSet;

                        // If the data reader does not return a data set,
                        // there is nothing left to do
                        if ((object)meterDataSet == null)
                            return;

                        // Set file path, file group, connection string,
                        // and meter asset key for the meter data set
                        meterDataSet.FilePath = dataFile.FilePath;
                        meterDataSet.FileGroup = fileGroup;
                        meterDataSet.Meter = meter;
                        meterDataSet.ConnectionString = LoadSystemSettings();

                        // Data reader has finally outlived its usefulness
                        dataReaderWrapper.Dispose();

                        // Shift date/time values to the configured time zone and set the start and end time values on the file group
                        ShiftTime(meterDataSet, meterDataSet.Meter.GetTimeZoneInfo(systemSettings.DefaultMeterTimeZoneInfo), systemSettings.XDATimeZoneInfo);
                        SetDataTimeRange(meterDataSet, dbAdapterContainer.GetAdapter<FileInfoDataContext>());

                        // Determine whether the file duration is within a user-defined maximum tolerance
                        ValidateFileDuration(meterDataSet.FilePath, systemSettings.MaxFileDuration, meterDataSet.FileGroup);

                        // Determine whether the timestamps in the file extend beyond user-defined thresholds
                        ValidateFileTimestamps(meterDataSet.FilePath, meterDataSet.FileGroup, systemSettings, dbAdapterContainer.GetAdapter<FileInfoDataContext>());

                        // Process the meter data set
                        OnStatusMessage($"Processing meter data from file \"{filePath}\"...");
                        ProcessMeterDataSet(meterDataSet, dbAdapterContainer);
                        OnStatusMessage($"Finished processing data from file \"{filePath}\".");

                    }
                }
                catch (Exception ex)
                {
                    // There seems to be a problem here where the outer exception's call stack
                    // was overwritten by the call stack of the point where it was thrown
                    ExceptionDispatchInfo exInfo = ExceptionDispatchInfo.Capture(ex);

                    try
                    {
                        // Attempt to set the error flag on the file group
                        dataContext.Connection.ExecuteNonQuery("UPDATE FileGroup SET Error = 1 WHERE ID = {0}", fileGroup.ID);
                    }
                    catch (Exception fileGroupError)
                    {
                        // Log any exceptions that occur when attempting to set the error flag on the file group
                        string message = $"Exception occurred setting error flag on file group: {fileGroupError.Message}";
                        OnProcessException(new Exception(message, fileGroupError));
                    }
                    OnProcessException(ex);

                    // Throw the original exception
                    exInfo.Throw();
                }
                finally
                {
                    try
                    {
                        // Attempt to set the processing end time of the file group
                        dataContext.Connection.ExecuteNonQuery("UPDATE FileGroup SET ProcessingEndTime = {0} WHERE ID = {1}", TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, systemSettings.XDATimeZoneInfo), fileGroup.ID);

                    }
                    catch (Exception ex)
                    {
                        // Log any exceptions that occur when attempting to set processing end time on the file group
                        string message = $"Exception occurred setting processing end time on file group: {ex.Message}";
                        OnProcessException(new Exception(message, ex));
                    }

                    // Keep track of the meters and files currently being processed
                    if ((object)meter.AssetKey != null)
                        m_activeFiles.TryRemove(meter.AssetKey, out filePath);

                    ThreadContext.Properties.Remove("Meter");
                }
                File.Delete(filePath);
            }
                
        }
        
        private void ParseFileWithoutLinq(string connectionString, SystemSettings systemSettings, string filePath, string meterKey, DataReaderWrapper dataReaderWrapper, FileWrapper fileWrapper)
        {
            MeterDataSet meterDataSet;
            Model.FileGroup fileGroup = null;
            using (DataContext dataContext = new DataContext("systemSettings"))
            {

                try
                {
                    fileGroup = fileWrapper.GetFileGroup(dataContext, systemSettings.XDATimeZoneInfo);
                    IEnumerable<Model.DataFile> dataFiles = dataContext.Table<Model.DataFile>().QueryRecordsWhere("FileGroupID = {0}", fileGroup.ID);
                    LoadFileBlob(dataFiles);

                    // Parse the file to turn it into a meter data set
                    OnStatusMessage($"Parsing data from file \"{filePath}\"...");
                    dataReaderWrapper.DataObject.Parse(filePath);
                    OnStatusMessage($"Finished parsing data from file \"{filePath}\".");
                    meterDataSet = dataReaderWrapper.DataObject.MeterDataSet;

                    // If the data reader does not return a data set,
                    // there is nothing left to do
                    if ((object)meterDataSet == null)
                        return;

                    // Data reader has finally outlived its usefulness
                    dataReaderWrapper.Dispose();

                    Model.Meter meter = dataContext.Table<Model.Meter>().QueryRecordWhere("AssetKey = {0}", meterKey);
                    ConfigurationDataSet configuration = meterDataSet.Configuration;
                    IEnumerable<DataSeries> dataSeries = meterDataSet.DataSeries;
                    IEnumerable<DataSeries> digitals = meterDataSet.Digitals;

                    // Shift date/time values to the configured time zone and set the start and end time values on the file group
                    ShiftTime(dataSeries, digitals, meterDataSet.Meter.GetTimeZoneInfo(systemSettings.DefaultMeterTimeZoneInfo), systemSettings.XDATimeZoneInfo);
                    SetDataTimeRange(dataSeries, digitals, fileGroup.ID, dataContext);

                    // Determine whether the file duration is within a user-defined maximum tolerance
                    ValidateFileDuration(meterDataSet.FilePath, systemSettings.MaxFileDuration, fileGroup.DataStartTime, fileGroup.DataEndTime);

                    // Determine whether the timestamps in the file extend beyond user-defined thresholds
                    ValidateFileTimestamps(meterDataSet.FilePath, fileGroup.DataStartTime,fileGroup.DataEndTime, systemSettings);

                    // Process the meter data set
                    OnStatusMessage($"Processing meter data from file \"{filePath}\"...");
                    using (DbAdapterContainer dbAdapterContainer = new DbAdapterContainer(systemSettings.DbConnectionString, systemSettings.DbTimeout))
                    {

                        ProcessMeterDataSet(meterDataSet, dbAdapterContainer);
                    }
                    OnStatusMessage($"Finished processing data from file \"{filePath}\".");

                }
                catch (Exception ex)
                {
                    // There seems to be a problem here where the outer exception's call stack
                    // was overwritten by the call stack of the point where it was thrown
                    ExceptionDispatchInfo exInfo = ExceptionDispatchInfo.Capture(ex);

                    try
                    {
                        // Attempt to set the error flag on the file group
                        if ((object)fileGroup != null)
                            fileGroup.Error = 1;
                    }
                    catch (Exception fileGroupError)
                    {
                        // Log any exceptions that occur when attempting to set the error flag on the file group
                        string message = $"Exception occurred setting error flag on file group: {fileGroupError.Message}";
                        OnProcessException(new Exception(message, fileGroupError));
                    }

                    // Throw the original exception
                    exInfo.Throw();

                }
                finally
                {
                    if ((object)fileGroup != null)
                    {
                        try
                        {
                            // Attempt to set the processing end time of the file group
                            fileGroup.ProcessingEndTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, systemSettings.XDATimeZoneInfo);
                            dataContext.Table<Model.FileGroup>().UpdateRecord(fileGroup);
                        }
                        catch (Exception ex)
                        {
                            // Log any exceptions that occur when attempting to set processing end time on the file group
                            string message = $"Exception occurred setting processing end time on file group: {ex.Message}";
                            OnProcessException(new Exception(message, ex));
                        }
                    }

                    // Keep track of the meters and files currently being processed
                    if ((object)meterKey != null)
                        m_activeFiles.TryRemove(meterKey, out filePath);

                    ThreadContext.Properties.Remove("Meter");

                }
            }

        }


        private void LoadFileBlob(IEnumerable<FaultData.Database.DataFile> dataFiles)
        {
            using (DataContext dataContext = new DataContext("systemSettings"))
            {
                foreach (FaultData.Database.DataFile dataFile in dataFiles)
                {
                    FileBlob file = new FileBlob() { DataFileID = dataFile.ID, Blob = File.ReadAllBytes(dataFile.FilePath) };
                    dataContext.Table<FileBlob>().AddNewRecord(file);
                }
            }
        }

        private void LoadFileBlob(IEnumerable<Model.DataFile> dataFiles)
        {
            using (DataContext dataContext = new DataContext("systemSettings"))
            {
                foreach (Model.DataFile dataFile in dataFiles)
                {
                    FileBlob file = new FileBlob() { DataFileID = dataFile.ID, Blob = File.ReadAllBytes(dataFile.FilePath) };
                    dataContext.Table<FileBlob>().AddNewRecord(file);
                }
            }
        }

        // Instantiates and executes data operations and data writers to process the meter data set.
        private void ProcessMeterDataSet(MeterDataSet meterDataSet, DbAdapterContainer dbAdapterContainer)
        {
            SystemInfoDataContext systemInfo;
            List<DataOperationWrapper> dataOperations = null;
            List<DataWriterWrapper> dataWriters = null;

            // Get the SystemInfoDataContext from the dbAdapterContainer
            systemInfo = dbAdapterContainer.GetAdapter<SystemInfoDataContext>();

            // Load the data operations from the database,
            // in descending order so we can remove records while we iterate
            dataOperations = systemInfo.DataOperations
                .OrderByDescending(dataOperation => dataOperation.LoadOrder)
                .Select(Wrap)
                .Where(wrapper => (object)wrapper != null)
                .ToList();

            for (int i = dataOperations.Count - 1; i >= 0; i--)
            {
                try
                {
                    Log.Debug($"Preparing data operation '{dataOperations[i].DataObject.GetType().Name}' for execution...");

                    // Load configuration parameters from the connection string into the data operation
                    ConnectionStringParser.ParseConnectionString(meterDataSet.ConnectionString, dataOperations[i].DataObject);

                    // Call the prepare method to allow the data operation to prepare any data it needs from the database
                    dataOperations[i].DataObject.Prepare(dbAdapterContainer);

                    Log.Debug($"Finished preparing data operation '{dataOperations[i].DataObject.GetType().Name}' for execution.");
                }
                catch (Exception ex)
                {
                    // Log the error and remove the data operation from the list
                    string message = $"An error occurred while preparing data from meter '{meterDataSet.Meter.AssetKey}' for data operation of type '{dataOperations[i].DataObject.GetType().FullName}': {ex.Message}";
                    OnProcessException(new Exception(message, ex));
                    dataOperations[i].Dispose();
                    dataOperations.RemoveAt(i);
                }
            }
            
            for (int i = dataOperations.Count - 1; i >= 0; i--)
            {
                try
                {
                    Log.Debug($"Executing data operation '{dataOperations[i].DataObject.GetType().Name}'...");

                    // Call the execute method on the data operation to perform in-memory data transformations
                    dataOperations[i].DataObject.Execute(meterDataSet);

                    Log.Debug($"Finished execurting data operation '{dataOperations[i].DataObject.GetType().Name}'.");
                }
                catch (Exception ex)
                {
                    // Log the error and skip to the next data operation
                    string message = $"An error occurred while executing data operation of type '{dataOperations[i].DataObject.GetType().FullName}' on data from meter '{meterDataSet.Meter.AssetKey}': {ex.Message}";
                    OnProcessException(new Exception(message, ex));
                    continue;
                }

                try
                {
                    Log.Debug($"Loading data from data operation '{dataOperations[i].DataObject.GetType().Name}' into database...");

                    // Call the load method inside a transaction to load data into from the data operation into the database
                    using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required, GetTransactionOptions()))
                    {
                        dataOperations[i].DataObject.Load(dbAdapterContainer);
                        transaction.Complete();
                    }

                    Log.Debug($"Finished loading data from data operation '{dataOperations[i].DataObject.GetType().Name}' into database.");
                }
                catch (Exception ex)
                {
                    // Log the error and move on to the next data operation
                    string message = $"An error occurred while loading data from data operation of type '{dataOperations[i].DataObject.GetType().FullName}' for data from meter '{meterDataSet.Meter.AssetKey}': {ex.Message}";
                    OnProcessException(new Exception(message, ex));
                }
            }

            // All data operations are complete, but we still need to clean up
            for (int i = dataOperations.Count - 1; i >= 0; i--)
                dataOperations[i].Dispose();

            // Load the data writers from the database
            dataWriters = systemInfo.DataWriters
                .OrderBy(dataWriter => dataWriter.LoadOrder)
                .Select(Wrap)
                .Where(wrapper => (object)wrapper != null)
                .ToList();

            foreach (DataWriterWrapper dataWriter in dataWriters)
            {
                try
                {
                    Log.Debug($"Writing results to external location with data writer '{dataWriter.DataObject.GetType().Name}'...");

                    // Load configuration parameters from the connection string into the data writer
                    ConnectionStringParser.ParseConnectionString(meterDataSet.ConnectionString, dataWriter.DataObject);

                    // Write the results to the data writer's destination by calling the WriteResults method
                    dataWriter.DataObject.WriteResults(dbAdapterContainer, meterDataSet);

                    Log.Debug($"Finished writing results with data writer '{dataWriter.DataObject.GetType().Name}'.");
                }
                catch (Exception ex)
                {
                    // Log the error and move on to the next data writer
                    string message = $"An error occurred while writing data from meter '{meterDataSet.Meter.AssetKey}' using data writer of type '{dataWriter.DataObject.GetType().FullName}': {ex.Message}";
                    OnProcessException(new Exception(message, ex));
                }
            }

            // All data writers are complete, but we still need to clean up
            foreach (DataWriterWrapper dataWriter in dataWriters)
                dataWriter.Dispose();
        }

        // Instantiates and executes data operations and data writers to process the meter data set.
        private void ProcessMeterDataSet(MeterDataSet meterDataSet, DataContext dataContext)
        {
            List<DataOperationWrapper> dataOperations = dataContext.Table<Model.DataOperation>().QueryRecords(orderByExpression: "LoadOrder DESC").Select(x => Wrap(x)).Where(x => (object)x != null).ToList();
            List<DataWriterWrapper> dataWriters = dataContext.Table<Model.DataWriter>().QueryRecords(orderByExpression: "LoadOrder DESC").Select(x => Wrap(x)).Where(x => (object)x != null).ToList(); ;

            for (int i = dataOperations.Count - 1; i >= 0; i--)
            {
                try
                {
                    Log.Debug($"Preparing data operation '{dataOperations[i].DataObject.GetType().Name}' for execution...");

                    // Load configuration parameters from the connection string into the data operation
                    ConnectionStringParser.ParseConnectionString(meterDataSet.ConnectionString, dataOperations[i].DataObject);

                    // Call the prepare method to allow the data operation to prepare any data it needs from the database
                    //dataOperations[i].DataObject.Prepare(dbAdapterContainer);

                    Log.Debug($"Finished preparing data operation '{dataOperations[i].DataObject.GetType().Name}' for execution.");
                }
                catch (Exception ex)
                {
                    // Log the error and remove the data operation from the list
                    string message = $"An error occurred while preparing data from meter '{meterDataSet.Meter.AssetKey}' for data operation of type '{dataOperations[i].DataObject.GetType().FullName}': {ex.Message}";
                    OnProcessException(new Exception(message, ex));
                    dataOperations[i].Dispose();
                    dataOperations.RemoveAt(i);
                }
            }

            for (int i = dataOperations.Count - 1; i >= 0; i--)
            {
                try
                {
                    Log.Debug($"Executing data operation '{dataOperations[i].DataObject.GetType().Name}'...");

                    // Call the execute method on the data operation to perform in-memory data transformations
                    //dataOperations[i].DataObject.Execute(meterDataSet);

                    Log.Debug($"Finished execurting data operation '{dataOperations[i].DataObject.GetType().Name}'.");
                }
                catch (Exception ex)
                {
                    // Log the error and skip to the next data operation
                    string message = $"An error occurred while executing data operation of type '{dataOperations[i].DataObject.GetType().FullName}' on data from meter '{meterDataSet.Meter.AssetKey}': {ex.Message}";
                    OnProcessException(new Exception(message, ex));
                    continue;
                }

                try
                {
                    Log.Debug($"Loading data from data operation '{dataOperations[i].DataObject.GetType().Name}' into database...");

                    // Call the load method inside a transaction to load data into from the data operation into the database
                    using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required, GetTransactionOptions()))
                    {
                        //dataOperations[i].DataObject.Load(dbAdapterContainer);
                        transaction.Complete();
                    }

                    Log.Debug($"Finished loading data from data operation '{dataOperations[i].DataObject.GetType().Name}' into database.");
                }
                catch (Exception ex)
                {
                    // Log the error and move on to the next data operation
                    string message = $"An error occurred while loading data from data operation of type '{dataOperations[i].DataObject.GetType().FullName}' for data from meter '{meterDataSet.Meter.AssetKey}': {ex.Message}";
                    OnProcessException(new Exception(message, ex));
                }
            }

            // All data operations are complete, but we still need to clean up
            for (int i = dataOperations.Count - 1; i >= 0; i--)
                dataOperations[i].Dispose();

            foreach (DataWriterWrapper dataWriter in dataWriters)
            {
                try
                {
                    Log.Debug($"Writing results to external location with data writer '{dataWriter.DataObject.GetType().Name}'...");

                    // Load configuration parameters from the connection string into the data writer
                    ConnectionStringParser.ParseConnectionString(meterDataSet.ConnectionString, dataWriter.DataObject);

                    // Write the results to the data writer's destination by calling the WriteResults method
                    //dataWriter.DataObject.WriteResults(dbAdapterContainer, meterDataSet);

                    Log.Debug($"Finished writing results with data writer '{dataWriter.DataObject.GetType().Name}'.");
                }
                catch (Exception ex)
                {
                    // Log the error and move on to the next data writer
                    string message = $"An error occurred while writing data from meter '{meterDataSet.Meter.AssetKey}' using data writer of type '{dataWriter.DataObject.GetType().FullName}': {ex.Message}";
                    OnProcessException(new Exception(message, ex));
                }
            }

            // All data writers are complete, but we still need to clean up
            foreach (DataWriterWrapper dataWriter in dataWriters)
                dataWriter.Dispose();
        }

        // Updates the Filter property of the FileProcessor with the
        // latest collection of filters from the DataReader table.
        private void UpdateFileProcessorFilter(SystemSettings systemSettings)
        {
            SystemInfoDataContext systemInfo;
            List<string> filterPatterns;

            // Do not attempt to load filter patterns if file processor is not defined
            if ((object)m_fileProcessor == null)
                return;

            // Get the list of file extensions to be processed by openXDA
            using (DbAdapterContainer dbAdapterContainer = new DbAdapterContainer(systemSettings.DbConnectionString, systemSettings.DbTimeout))
            {
                systemInfo = dbAdapterContainer.GetAdapter<SystemInfoDataContext>();

                filterPatterns = systemInfo.DataReaders
                    .Select(reader => reader.FilePattern)
                    .ToList();
            }

            m_fileProcessor.Filter = string.Join(Path.PathSeparator.ToString(), filterPatterns);
        }

        // Gets the thread used to process data for
        // the meter identified by the given asset key.
        private LogicalThread GetThread(string meterKey)
        {
            if ((object)meterKey == null)
                return m_noMeterThread ?? (m_noMeterThread = m_meterDataScheduler.CreateThread());

            return m_meterDataThreadLookup.GetOrAdd(meterKey, key =>
            {
                LogicalThread newThread = m_meterDataScheduler.CreateThread();

                newThread.UnhandledException += (sender, args) =>
                {
                    Exception ex = args.Argument;

                    if (ex is FileSkippedException)
                    {
                        Log.Warn(ex.Message);
                        return;
                    }

                    string message = $"Exception occurred processing data from meter \"{meterKey}\": {ex.Message}";
                    OnProcessException(new Exception(message, ex));
                };

                return newThread;
            });
        }

        // Loads system settings from the database.
        private string LoadSystemSettings()
        {
            using (SystemInfoDataContext systemInfo = new SystemInfoDataContext(m_dbConnectionString))
            {
                return LoadSystemSettings(systemInfo);
            }
        }

        // Loads system settings from the database.
        private string LoadSystemSettings(SystemInfoDataContext systemInfo)
        {
            foreach (IGrouping<string, Setting> grouping in systemInfo.Settings.GroupBy(setting => setting.Name))
            {
                if (grouping.Count() > 1)
                    Log.Warn($"Duplicate record for setting {grouping.Key} detected.");
            }

            // Convert the Setting table to a dictionary
            Dictionary<string, string> settings = systemInfo.Settings
                .DistinctBy(setting => setting.Name)
                .ToDictionary(setting => setting.Name, setting => setting.Value, StringComparer.OrdinalIgnoreCase);

            // Add the database connection string if there is not
            // already one explicitly specified in the Setting table
            if (!settings.ContainsKey("dbConnectionString"))
                settings.Add("dbConnectionString", m_dbConnectionString);

            // Convert dictionary to a connection string and return it
            return SystemSettings.ToConnectionString(settings);
        }

        #endregion

        #region [ Static ]

        // Static Fields
        private static readonly ConnectionStringParser<SettingAttribute, CategoryAttribute> ConnectionStringParser = new ConnectionStringParser<SettingAttribute, CategoryAttribute>();
        private static readonly ILog Log = LogManager.GetLogger(typeof(ExtensibleDisturbanceAnalysisEngine));

        // Static Methods

        // Attempts to create the directory at the given path.
        private static void TryCreateDirectory(string path)
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

        // Loads the type identified by the given type name from the assembly with the given assembly name.
        // Handles error logging so that this can be used in LINQ expressions.
        private static Type LoadType(string assemblyName, string typeName)
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

        // Uses regular expressions to read the meter's asset key from the file path.
        private static string GetMeterKey(string filePath, string filePattern)
        {
            Match match = Regex.Match(filePath, filePattern, RegexOptions.IgnoreCase);
            Group meterKeyGroup;

            if (!match.Success)
                throw new FileSkippedException($"Skipped file \"{filePath}\" because the file path did not match the file pattern: \"{filePattern}\".");

            meterKeyGroup = match.Groups["AssetKey"];

            if ((object)meterKeyGroup == null)
                throw new FileSkippedException($"Skipped file \"{filePath}\" because the AssetKey capture group was not found when matching to the file pattern: \"{filePattern}\".");

            return meterKeyGroup.Value;
        }

        // Adjusts the timestamps in the given data sets to the time zone of XDA.
        private static void ShiftTime(MeterDataSet meterDataSet, TimeZoneInfo meterTimeZone, TimeZoneInfo xdaTimeZone)
        {
            foreach (DataSeries dataSeries in meterDataSet.DataSeries)
            {
                foreach (DataPoint dataPoint in dataSeries.DataPoints)
                {
                    if (dataPoint.Time.Kind != DateTimeKind.Unspecified)
                        dataPoint.Time = TimeZoneInfo.ConvertTimeToUtc(dataPoint.Time);
                    else
                        dataPoint.Time = TimeZoneInfo.ConvertTimeToUtc(dataPoint.Time, meterTimeZone);

                    dataPoint.Time = TimeZoneInfo.ConvertTimeFromUtc(dataPoint.Time, xdaTimeZone);
                }
            }

            foreach (DataSeries dataSeries in meterDataSet.Digitals)
            {
                foreach (DataPoint dataPoint in dataSeries.DataPoints)
                {
                    if (dataPoint.Time.Kind != DateTimeKind.Unspecified)
                        dataPoint.Time = TimeZoneInfo.ConvertTimeToUtc(dataPoint.Time);
                    else
                        dataPoint.Time = TimeZoneInfo.ConvertTimeToUtc(dataPoint.Time, meterTimeZone);

                    dataPoint.Time = TimeZoneInfo.ConvertTimeFromUtc(dataPoint.Time, xdaTimeZone);
                }
            }
        }

        // Determines the start time and end time of the given data and sets the properties on the given file group.
        private static void SetDataTimeRange(MeterDataSet meterDataSet, FileInfoDataContext fileInfo)
        {
            DateTime dataStartTime;
            DateTime dataEndTime;

            dataStartTime = meterDataSet.DataSeries
                .Concat(meterDataSet.Digitals)
                .Where(dataSeries => dataSeries.DataPoints.Any())
                .Select(dataSeries => dataSeries.DataPoints.First().Time)
                .DefaultIfEmpty()
                .Min();

            dataEndTime = meterDataSet.DataSeries
                .Concat(meterDataSet.Digitals)
                .Where(dataSeries => dataSeries.DataPoints.Any())
                .Select(dataSeries => dataSeries.DataPoints.Last().Time)
                .DefaultIfEmpty()
                .Max();

            if (dataStartTime != default(DateTime))
                meterDataSet.FileGroup.DataStartTime = dataStartTime;

            if (dataEndTime != default(DateTime))
                meterDataSet.FileGroup.DataEndTime = dataEndTime;

            fileInfo.SubmitChanges();
        }

        // Adjusts the timestamps in the given data sets to the time zone of XDA.
        private static void ShiftTime(IEnumerable<DataSeries> dataSerieses, IEnumerable<DataSeries> digitals, TimeZoneInfo meterTimeZone, TimeZoneInfo xdaTimeZone)
        {
            foreach (DataSeries dataSeries in dataSerieses)
            {
                foreach (DataPoint dataPoint in dataSeries.DataPoints)
                {
                    if (dataPoint.Time.Kind != DateTimeKind.Unspecified)
                        dataPoint.Time = TimeZoneInfo.ConvertTimeToUtc(dataPoint.Time);
                    else
                        dataPoint.Time = TimeZoneInfo.ConvertTimeToUtc(dataPoint.Time, meterTimeZone);

                    dataPoint.Time = TimeZoneInfo.ConvertTimeFromUtc(dataPoint.Time, xdaTimeZone);
                }
            }

            foreach (DataSeries dataSeries in digitals)
            {
                foreach (DataPoint dataPoint in dataSeries.DataPoints)
                {
                    if (dataPoint.Time.Kind != DateTimeKind.Unspecified)
                        dataPoint.Time = TimeZoneInfo.ConvertTimeToUtc(dataPoint.Time);
                    else
                        dataPoint.Time = TimeZoneInfo.ConvertTimeToUtc(dataPoint.Time, meterTimeZone);

                    dataPoint.Time = TimeZoneInfo.ConvertTimeFromUtc(dataPoint.Time, xdaTimeZone);
                }
            }
        }

        // Determines the start time and end time of the given data and sets the properties on the given file group.
        private static void SetDataTimeRange(IEnumerable<DataSeries> dataSerieses, IEnumerable<DataSeries> digitals, int fileGroupId, DataContext dataContext)
        {
            DateTime dataStartTime;
            DateTime dataEndTime;

            Model.FileGroup fileGroup = dataContext.Table<Model.FileGroup>().QueryRecordWhere("ID = {0}", fileGroupId);
            dataStartTime = dataSerieses
                .Concat(digitals)
                .Where(dataSeries => dataSeries.DataPoints.Any())
                .Select(dataSeries => dataSeries.DataPoints.First().Time)
                .DefaultIfEmpty()
                .Min();

            dataEndTime = dataSerieses
                .Concat(digitals)
                .Where(dataSeries => dataSeries.DataPoints.Any())
                .Select(dataSeries => dataSeries.DataPoints.Last().Time)
                .DefaultIfEmpty()
                .Max();

            if (dataStartTime != default(DateTime))
                fileGroup.DataStartTime = dataStartTime;

            if (dataEndTime != default(DateTime))
                fileGroup.DataEndTime = dataEndTime;

            dataContext.Table<Model.FileGroup>().UpdateRecord(fileGroup);
        }



        // Instantiates the given data reader and wraps it in a disposable wrapper object.
        private static DataReaderWrapper Wrap(FaultData.Database.DataReader reader)
        {
            try
            {
                Assembly assembly = Assembly.LoadFrom(reader.AssemblyName);
                Type type = assembly.GetType(reader.TypeName);
                return new DataReaderWrapper(reader.ID, type);
            }
            catch (Exception ex)
            {
                string message = $"Failed to create data reader of type {reader.TypeName}: {ex.Message}";
                throw new TypeLoadException(message, ex);
            }
        }

        // Instantiates the given data operation and wraps it in a disposable wrapper object.
        private static DataOperationWrapper Wrap(FaultData.Database.DataOperation operation)
        {
            try
            {
                Assembly assembly = Assembly.LoadFrom(operation.AssemblyName);
                Type type = assembly.GetType(operation.TypeName);
                return new DataOperationWrapper(operation.ID, type);
            }
            catch (Exception ex)
            {
                string message = $"Failed to create data operation of type {operation.TypeName}: {ex.Message}";
                throw new TypeLoadException(message, ex);
            }
        }

        // Instantiates the given data operation and wraps it in a disposable wrapper object.
        private static DataOperationWrapper Wrap(Model.DataOperation operation)
        {
            try
            {
                Assembly assembly = Assembly.LoadFrom(operation.AssemblyName);
                Type type = assembly.GetType(operation.TypeName);
                return new DataOperationWrapper(operation.ID, type);
            }
            catch (Exception ex)
            {
                string message = $"Failed to create data operation of type {operation.TypeName}: {ex.Message}";
                throw new TypeLoadException(message, ex);
            }
        }


        // Instantiates the given data writer and wraps it in a disposable wrapper object.
        private static DataWriterWrapper Wrap(FaultData.Database.DataWriter writer)
        {
            try
            {
                Assembly assembly = Assembly.LoadFrom(writer.AssemblyName);
                Type type = assembly.GetType(writer.TypeName);
                return new DataWriterWrapper(writer.ID, type);
            }
            catch (Exception ex)
            {
                string message = $"Failed to create data writer of type {writer.TypeName}: {ex.Message}";
                throw new TypeLoadException(message, ex);
            }
        }

        // Instantiates the given data writer and wraps it in a disposable wrapper object.
        private static DataWriterWrapper Wrap(Model.DataWriter writer)
        {
            try
            {
                Assembly assembly = Assembly.LoadFrom(writer.AssemblyName);
                Type type = assembly.GetType(writer.TypeName);
                return new DataWriterWrapper(writer.ID, type);
            }
            catch (Exception ex)
            {
                string message = $"Failed to create data writer of type {writer.TypeName}: {ex.Message}";
                throw new TypeLoadException(message, ex);
            }
        }


        // Determines whether the file creation time exceeds a user-defined threshold.
        private static void ValidateFileCreationTime(string filePath, double maxFileCreationTimeOffset)
        {
            DateTime fileCreationTime;
            double hoursSinceCreation;

            // Determine whether file creation time validation is disabled
            if (maxFileCreationTimeOffset <= 0.0D)
                return;

            // This method may be called if the file was deleted,
            // in which case it should be impossible to get the
            // creation time of the file
            if (!File.Exists(filePath))
                return;

            // Determine the number of hours that have passed since the file was created
            fileCreationTime = File.GetCreationTimeUtc(filePath);
            hoursSinceCreation = DateTime.UtcNow.Subtract(fileCreationTime).TotalHours;

            // Determine whether the number of hours exceeds the maximum threshold
            if (hoursSinceCreation > maxFileCreationTimeOffset)
                throw new FileSkippedException($"Skipped file \"{filePath}\" because file creation time '{fileCreationTime}' is too old.");
        }

        // Determines whether configuration exists for the meter with the given asset key.
        private static void ValidateMeterKey(string filePath, string meterKey, MeterInfoDataContext meterInfo)
        {
            // Determine whether there exists a meter whose asset key matches the given meterKey
            if (!meterInfo.Meters.Any(m => m.AssetKey == meterKey))
                throw new FileSkippedException($"Skipped file \"{filePath}\" because no meter configuration was found for meter {meterKey}.");
        }

        // Determines whether the duration of data in the file exceeds a user-defined threshold.
        private static void ValidateFileDuration(string filePath, double maxFileDuration, FaultData.Database.FileGroup fileGroup)
        {
            double timeDifference;

            // Determine whether file duration validation is disabled
            if (maxFileDuration <= 0.0D)
                return;

            // Determine the number of seconds between the start and end time of the data in the file
            timeDifference = fileGroup.DataEndTime.Subtract(fileGroup.DataStartTime).TotalSeconds;

            // Determine whether the file duration exceeds the maximum threshold
            if (timeDifference > maxFileDuration)
                throw new FileSkippedException($"Skipped file \"{filePath}\" because duration of the file ({timeDifference:0.##} seconds) is too long.");
        }

        // Determines whether the duration of data in the file exceeds a user-defined threshold.
        private static void ValidateFileDuration(string filePath, double maxFileDuration, DateTime dataStartTime, DateTime dataEndTime)
        {
            double timeDifference;

            // Determine whether file duration validation is disabled
            if (maxFileDuration <= 0.0D)
                return;

            // Determine the number of seconds between the start and end time of the data in the file
            timeDifference = dataEndTime.Subtract(dataStartTime).TotalSeconds;

            // Determine whether the file duration exceeds the maximum threshold
            if (timeDifference > maxFileDuration)
                throw new FileSkippedException($"Skipped file \"{filePath}\" because duration of the file ({timeDifference:0.##} seconds) is too long.");
        }


        // Determines whether the timestamps in the file extend beyond user-defined thresholds.
        private static void ValidateFileTimestamps(string filePath, FaultData.Database.FileGroup fileGroup, SystemSettings systemSettings, FileInfoDataContext fileInfo)
        {
            DateTime now;
            double timeDifference;

            // Get the current time in XDA's time zone
            now = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, systemSettings.XDATimeZoneInfo);

            // Determine whether past timestamp validation is disabled
            if (systemSettings.MinTimeOffset > 0.0D)
            {
                // Get the total number of hours between the current time and the start time of the data in the file
                timeDifference = now.Subtract(fileGroup.DataStartTime).TotalHours;

                // Determine whether the number of hours exceeds the threshold
                if (timeDifference > systemSettings.MinTimeOffset)
                    throw new FileSkippedException($"Skipped file \"{filePath}\" because data start time '{fileGroup.DataStartTime}' is too old.");
            }

            // Determine whether future timestamp validation is disabled
            if (systemSettings.MaxTimeOffset > 0.0D)
            {
                // Get the total number of hours between the current time and the end time of the data in the file
                timeDifference = fileGroup.DataEndTime.Subtract(now).TotalHours;

                // Determine whether the number of hours exceeds the threshold
                if (timeDifference > systemSettings.MaxTimeOffset)
                    throw new FileSkippedException($"Skipped file \"{filePath}\" because data end time '{fileGroup.DataEndTime}' is too far in the future.");
            }
        }

        // Determines whether the timestamps in the file extend beyond user-defined thresholds.
        private static void ValidateFileTimestamps(string filePath, DateTime dataStartTime, DateTime dataEndTime, SystemSettings systemSettings)
        {
            DateTime now;
            double timeDifference;

            // Get the current time in XDA's time zone
            now = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, systemSettings.XDATimeZoneInfo);

            // Determine whether past timestamp validation is disabled
            if (systemSettings.MinTimeOffset > 0.0D)
            {
                // Get the total number of hours between the current time and the start time of the data in the file
                timeDifference = now.Subtract(dataStartTime).TotalHours;

                // Determine whether the number of hours exceeds the threshold
                if (timeDifference > systemSettings.MinTimeOffset)
                    throw new FileSkippedException($"Skipped file \"{filePath}\" because data start time '{dataStartTime}' is too old.");
            }

            // Determine whether future timestamp validation is disabled
            if (systemSettings.MaxTimeOffset > 0.0D)
            {
                // Get the total number of hours between the current time and the end time of the data in the file
                timeDifference = dataEndTime.Subtract(now).TotalHours;

                // Determine whether the number of hours exceeds the threshold
                if (timeDifference > systemSettings.MaxTimeOffset)
                    throw new FileSkippedException($"Skipped file \"{filePath}\" because data end time '{dataEndTime}' is too far in the future.");
            }
        }

        // Gets the default set of transaction options used for data operation transactions.
        private static TransactionOptions GetTransactionOptions()
        {
            return new TransactionOptions()
            {
                IsolationLevel = IsolationLevel.ReadCommitted,
                Timeout = TransactionManager.MaximumTimeout
            };
        }

        // Called when the file processor encounters an unexpected error.
        private static void FileProcessor_Error(object sender, ErrorEventArgs args)
        {
            Exception ex = args.GetException();

            if (ex is FileSkippedException)
                Log.Warn(ex.Message);
            else
                OnProcessException(args.GetException());
        }

        // Displays status message to the console - proxy method for service implementation
        [StringFormatMethod("format")]
        private static void OnStatusMessage(string format, params object[] args)
        {
            Log.Info(string.Format(format, args));
        }

        // Displays exception message to the console - proxy method for service implmentation
        private static void OnProcessException(Exception ex)
        {
            Log.Error(ex.Message, ex);
        }

        #endregion
    }
}
