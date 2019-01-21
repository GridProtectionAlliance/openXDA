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

using FaultData.Configuration;
using FaultData.DataAnalysis;
using FaultData.DataOperations;
using FaultData.DataReaders;
using FaultData.DataSets;
using GSF.Annotations;
using GSF.Collections;
using GSF.Configuration;
using GSF.Data;
using GSF.Data.Model;
using GSF.IO;
using GSF.IO.Checksums;
using GSF.Threading;
using log4net;
using openXDA.Configuration;
using openXDA.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
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

        private class DataProcessorState
        {
            public string FilePath { get; set; }
            public string FileName => Path.GetFileName(FilePath);
            public FileWrapper FileWrapper { get; set; }
            public FileGroup FileGroup { get; set; }

            public string MeterKey { get; set; }
            public string ConnectionString { get; set; }
            public SystemSettings SystemSettings { get; set; }

            public Action<DataProcessorState> ProcessFileCallback { get; set; }
            public Action<DataProcessorState> ProcessFailureCallback { get; set; }
            public MeterDataSet MeterDataSet { get; set; }
            public bool Retry { get; set; }
        }

        private class FileWrapper
        {
            #region [ Members ]

            // Fields
            private string m_filePath;
            private string m_directory;
            private string m_filePathWithoutExtension;

            private DateTime? m_maxFileCreationTime;
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

            /// <summary>
            /// Creates a new instance of the <see cref="FileWrapper"/> class.
            /// </summary>
            /// <param name="filePath">The path to the wrapped file.</param>
            /// <param name="maxFileCreationTime">The max creation time of the file group.</param>
            public FileWrapper(string filePath, DateTime maxFileCreationTime)
                : this(filePath)
            {
                m_maxFileCreationTime = maxFileCreationTime;
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
            /// Creates new <see cref="DataFile"/> objects for each file in the file group.
            /// </summary>
            /// <param name="xdaTimeZone">The time zone used by openXDA.</param>
            /// <returns>The list of data files.</returns>
            public List<DataFile> CreateDataFiles(TimeZoneInfo xdaTimeZone)
            {
                List<DataFile> dataFiles = new List<DataFile>();
                DataFile dataFile;
                FileInfo fileInfo;

                foreach (string filePath in GSF.IO.FilePath.GetFileList($"{m_filePathWithoutExtension}.*"))
                {
                    fileInfo = new FileInfo(filePath);

                    dataFile = new DataFile();
                    dataFile.FilePath = filePath;
                    dataFile.FilePathHash = filePath.GetHashCode();
                    dataFile.FileSize = fileInfo.Length;
                    dataFile.CreationTime = TimeZoneInfo.ConvertTimeFromUtc(fileInfo.CreationTimeUtc, xdaTimeZone);
                    dataFile.LastWriteTime = TimeZoneInfo.ConvertTimeFromUtc(fileInfo.LastWriteTimeUtc, xdaTimeZone);
                    dataFile.LastAccessTime = TimeZoneInfo.ConvertTimeFromUtc(fileInfo.LastAccessTimeUtc, xdaTimeZone);

                    dataFiles.Add(dataFile);
                }

                return dataFiles;
            }

            /// <summary>
            /// Gets the maximum creation time of the files with the same root name as the wrapped file.
            /// </summary>
            /// <returns>The creation time of the file with the most recent creation time of the files with the same root name as the wrapped file.</returns>
            public DateTime GetMaxFileCreationTime()
            {
                if (m_maxFileCreationTime.HasValue)
                    return m_maxFileCreationTime.GetValueOrDefault();

                string[] fileList = GSF.IO.FilePath.GetFileList($"{m_filePathWithoutExtension}.*");
                return fileList.Max(file => m_fileCreationTimes.GetOrAdd(file, path => DateTime.UtcNow));
            }

            #endregion
        }

        private class FileSkippedException : Exception
        {
            public FileSkippedException()
            {
            }

            public FileSkippedException(string message)
                : base(message)
            {
            }

            public FileSkippedException(string message, Exception innerException)
                : base(message, innerException)
            {
            }
        }

        // Constants
        private const int FileEnumerationPriority = 1;
        private const int FileWatcherPriority = 2;
        private const int RequeuePriority = 3;

        // Fields
        private string m_dbConnectionString;
        private SystemSettings m_systemSettings;

        private FileProcessor m_fileProcessor;
        private ConcurrentDictionary<string, string> m_activeFiles;

        private LogicalThreadScheduler m_meterDataScheduler;
        private Dictionary<string, LogicalThread> m_meterDataThreadLookup;
        private LogicalThread m_noMeterThread;
        private int m_meterTaskCount;

        private EventEmailEngine m_eventEmailEngine;

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

                if (systemSettings.FileWatcherEnabled)
                {
                    statusBuilder.AppendLine("File Processor Status:");
                    statusBuilder.AppendLine(new string('=', 50));
                    statusBuilder.AppendLine($"                 Filter: {m_fileProcessor.Filter}");
                    statusBuilder.AppendLine($"   Internal buffer size: {m_fileProcessor.InternalBufferSize}");
                    statusBuilder.AppendLine($"   Max thread pool size: {m_fileProcessor.MaxThreadCount}");
                    statusBuilder.AppendLine($"      Max fragmentation: {m_fileProcessor.MaxFragmentation}");
                    statusBuilder.AppendLine($"   Enumeration strategy: {m_fileProcessor.EnumerationStrategy}");
                    statusBuilder.AppendLine($"         Is Enumerating: {m_fileProcessor.IsEnumerating}");
                    statusBuilder.AppendLine($"        Processed files: {m_fileProcessor.ProcessedFileCount}");
                    statusBuilder.AppendLine($"          Skipped files: {m_fileProcessor.SkippedFileCount}");
                    statusBuilder.AppendLine($"         Requeued files: {m_fileProcessor.RequeuedFileCount}");
                    statusBuilder.AppendLine($"            Is Cleaning: {m_fileProcessor.IsCleaning}");
                    statusBuilder.AppendLine($"      Last Compact Time: {m_fileProcessor.LastCompactTime}");
                    statusBuilder.AppendLine($"  Last Compact Duration: {m_fileProcessor.LastCompactDuration}");
                    statusBuilder.AppendLine();

                    if (m_fileProcessor.IsEnumerating)
                    {
                        IList<string> activelyEnumeratedPaths = m_fileProcessor.ActivelyEnumeratedPaths;

                        statusBuilder.AppendLine("  Actively enumerated paths:");

                        foreach (string path in activelyEnumeratedPaths.Take(5))
                            statusBuilder.AppendLine($"    {path}");

                        if (activelyEnumeratedPaths.Count > 5)
                            statusBuilder.AppendLine($"    {activelyEnumeratedPaths.Count - 5} more...");

                        statusBuilder.AppendLine();
                    }

                    statusBuilder.AppendLine("  Watch directories:");

                    foreach (string path in m_fileProcessor.TrackedDirectories)
                        statusBuilder.AppendLine($"    {path}");
                }

                if (m_eventEmailEngine.EmailServiceEnabled)
                {
                    statusBuilder.AppendLine();
                    statusBuilder.AppendLine("Event Email Status:");
                    statusBuilder.AppendLine(new string('=', 50));
                    statusBuilder.AppendLine($"       Tagged Emails: {m_eventEmailEngine.TaggedEmailCount}");
                    statusBuilder.AppendLine($"     Max Email Count: {m_eventEmailEngine.MaxEmailCount}");
                    statusBuilder.AppendLine($"      Max Email Span: {m_eventEmailEngine.MaxEmailSpan}");
                    statusBuilder.AppendLine($"             Tripped: {m_eventEmailEngine.EmailServiceTripped}");
                }

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
            // Get system settings from the database
            ReloadSystemSettings();

            // Reload configuration at startup
            ReloadConfiguration();

            // Make sure watch directories exist
            foreach (string path in m_systemSettings.WatchDirectoryList)
                TryCreateDirectory(path);

            // Create the lookup table used to track which files are being processed
            if ((object)m_activeFiles == null)
                m_activeFiles = new ConcurrentDictionary<string, string>();

            // Create the scheduler used to schedule when to process meter data
            if ((object)m_meterDataScheduler == null)
            {
                m_meterDataScheduler = new LogicalThreadScheduler(3);
                m_meterDataThreadLookup = new Dictionary<string, LogicalThread>();

                m_meterDataScheduler.UnhandledException += (sender, args) =>
                {
                    string message = $"Unhandled exception occurred while processing meter data: {args.Argument.Message}";
                    Exception ex = new Exception(message, args.Argument);
                    OnProcessException(ex);
                };
            }

            m_meterDataScheduler.MaxThreadCount = m_systemSettings.ProcessingThreadCount;

            // Setup new email engine to send emails when an event occurs
            if ((object)m_eventEmailEngine == null)
                m_eventEmailEngine = new EventEmailEngine();

            // Setup new file processor to monitor the watch directories
            if ((object)m_fileProcessor == null)
            {
                m_fileProcessor = new FileProcessor(m_systemSettings.FileProcessorID);
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

            if (m_systemSettings.FileWatcherEnabled)
            {
                foreach (string path in m_systemSettings.WatchDirectoryList)
                    m_fileProcessor.AddTrackedDirectory(path);
            }
        }

        /// <summary>
        /// Reloads system configuration from configuration sources.
        /// </summary>
        public void ReloadConfiguration()
        {
            // If system settings is null,
            // attempt to reload system settings
            if ((object)m_systemSettings == null)
                ReloadSystemSettings();

            // If system settings is still null, give up
            if ((object)m_systemSettings == null)
                return;

            using (AdoDataConnection connection = CreateDbConnection(m_systemSettings))
            {
                TableOperations<ConfigurationLoader> configurationLoaderTable = new TableOperations<ConfigurationLoader>(connection);

                List<ConfigurationLoader> configurationLoaderDefinitions = configurationLoaderTable
                    .QueryRecords("LoadOrder")
                    .ToList();

                string connectionString = LoadSystemSettings(connection);

                foreach (ConfigurationLoader configurationLoaderDefinition in configurationLoaderDefinitions)
                {
                    try
                    {
                        OnStatusMessage("[{0}] Loading configuration...", configurationLoaderDefinition.UnqualifiedTypeName);

                        using (ConfigurationLoaderWrapper wrapper = Wrap(configurationLoaderDefinition))
                        {
                            IConfigurationLoader configurationLoader = wrapper.DataObject;

                            // Use the connection string parser to load system settings into the configuration loader
                            ConnectionStringParser.ParseConnectionString(connectionString, configurationLoader);

                            // Update configuration by calling the configuration loader's UpdateConfiguration method
                            configurationLoader.UpdateConfiguration(connection);
                        }

                        OnStatusMessage("[{0}] Done loading configuration.", configurationLoaderDefinition.UnqualifiedTypeName);
                    }
                    catch (Exception ex)
                    {
                        string message = string.Format("[{0}] Unable to update configuration due to exception: {1}", configurationLoaderDefinition.UnqualifiedTypeName, ex.Message);
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

                if (m_systemSettings.FileWatcherEnabled)
                {
                    foreach (string directory in m_fileProcessor.TrackedDirectories.ToList())
                    {
                        if (!m_systemSettings.WatchDirectoryList.Contains(directory, StringComparer.OrdinalIgnoreCase))
                            m_fileProcessor.RemoveTrackedDirectory(directory);
                    }

                    foreach (string directory in m_systemSettings.WatchDirectoryList)
                        m_fileProcessor.AddTrackedDirectory(directory);
                }
                else
                {
                    foreach (string directory in m_fileProcessor.TrackedDirectories.ToList())
                        m_fileProcessor.RemoveTrackedDirectory(directory);
                }
            }
        }

        /// <summary>
        /// Auto deletes old files.
        /// </summary>
        /// <param name="args">The arguments supplied to the command to tweak the settings.</param>
        /// <returns></returns>
        public void AutoDeleteFiles()
        {
            using (AdoDataConnection connection = CreateDbConnection(m_systemSettings))
            {
                TableOperations<Setting> settingTable = new TableOperations<Setting>(connection);
                int days = int.Parse(settingTable.QueryRecordWhere("Name = 'AutoFileDeletionDays'")?.Value ?? "30");
                DateTime date = DateTime.UtcNow.AddDays(days * -1);

                foreach (string path in m_fileProcessor.TrackedDirectories)
                {
                    foreach(string filePath in FilePath.EnumerateFiles(path))
                    {
                        FileInfo file = new FileInfo(filePath);
                        List<DateTime> list = new List<DateTime>();
                        list.Add(file.CreationTimeUtc);
                        list.Add(file.LastAccessTimeUtc);
                        list.Add(file.LastWriteTimeUtc);
                        DateTime max = list.Max();

                        if (max < date)
                        {
                            OnStatusMessage("Deleting file - {0}", file.Name);
                            file.Delete();
                        }
                    }
                }
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
        /// Purges data from database beyond a specified date.
        /// </summary>
        /// <param name="args">The arguments supplied to the command to tweak the settings.</param>
        /// <returns>A message describing the change that was made.</returns>
        public string PurgeData(string[] args)
        {
            if (args.Length == 0 || args[0] == "-?")
            {
                StringBuilder helpMessage = new StringBuilder();

                helpMessage.Append("Purges data from database beyond a specified date.");
                helpMessage.AppendLine();
                helpMessage.AppendLine();
                helpMessage.Append("   Usage:");
                helpMessage.AppendLine();
                helpMessage.Append("       PurgeData {  MM/DD/YY }");
                helpMessage.AppendLine();
                helpMessage.AppendLine();
                helpMessage.AppendLine();
                helpMessage.Append("       PurgeData -?");
                helpMessage.AppendLine();
                helpMessage.AppendLine();
                return helpMessage.ToString();
            }
            else if (args.Length == 1) {
                DateTime parseDate;

                try
                {
                    parseDate = DateTime.Parse(args[0]);
                }
                catch (Exception)
                {
                    StringBuilder helpMessage = new StringBuilder();
                    helpMessage.Append("Argument not a valid date time object, please use MM/DD/YY");
                    return helpMessage.ToString();
                }

                using (AdoDataConnection connection = CreateDbConnection(m_systemSettings))
                {
                    TableOperations<Event> table = new TableOperations<Event>(connection);
                    int count = table.QueryRecordCountWhere("FileGroupID IN (SELECT ID FROM FileGroup WHERE ProcessingStartTime < {0})", parseDate);
                    StringBuilder helpMessage = new StringBuilder();
                    helpMessage.Append($"{count} events and subsequent data will be purged from the database...");
                    helpMessage.Append($"Run command PurgeData {args[0]} -verify to execute.");

                    return helpMessage.ToString();

                }
            }
            else if (args.Length == 2 && args[1] == "-verify") {
                DateTime parseDate;

                try
                {
                    parseDate = DateTime.Parse(args[0]);
                }
                catch (Exception)
                {
                    StringBuilder helpMessage = new StringBuilder();
                    helpMessage.Append("Argument not a valid date time object, please use MM/DD/YY");
                    return helpMessage.ToString();
                }

                using (AdoDataConnection connection = CreateDbConnection(m_systemSettings))
                {
                    TableOperations<FileGroup> table = new TableOperations<FileGroup>(connection);
                    IEnumerable<int> fileGroupIDs = table.QueryRecordsWhere($"ProcessingStartTime < '{parseDate.ToString()}'").Select(x => x.ID);
                    connection.ExecuteNonQuery($"EXEC UniversalCascadeDelete 'FileGroup','ProcessingStartTime < ''{parseDate.ToString()}'''");
                    StringBuilder helpMessage = new StringBuilder();
                    helpMessage.Append($"Preliminary data deleted.  Event data will continue deletion in the backgroup");

                    Task.Run(() => {
                        using(AdoDataConnection conn = CreateDbConnection(m_systemSettings))
                        {
                            while (fileGroupIDs.Any())
                            {
                                string stringOfIds = string.Join(",", fileGroupIDs.Take(50));
                                fileGroupIDs = fileGroupIDs.Skip(50);
                                conn.ExecuteNonQuery($"EXEC UniversalCascadeDelete 'EventData', 'FileGroupID IN ({stringOfIds})'");
                            }
                        }
                    });

                    return helpMessage.ToString();

                }
            }
            else {
                StringBuilder helpMessage = new StringBuilder();

                helpMessage.Append("Improper use of command - PurgeData.");
                helpMessage.AppendLine();
                helpMessage.AppendLine();
                helpMessage.Append("   Usage:");
                helpMessage.AppendLine();
                helpMessage.Append("       PurgeData {  MM/DD/YY }");
                helpMessage.AppendLine();
                helpMessage.AppendLine();
                helpMessage.AppendLine();
                helpMessage.Append("       PurgeData -?");
                helpMessage.AppendLine();
                helpMessage.AppendLine();
                return helpMessage.ToString();
            }
        }

        /// <summary>
        /// Issues command to the file processor to enumerate the watch directories.
        /// </summary>
        public void EnumerateWatchDirectories()
        {
            m_fileProcessor.EnumerateWatchDirectories();
        }

        /// <summary>
        /// Processes the given file on the appropriate meter thread with the given priority.
        /// </summary>
        /// <param name="filePath">The path to the file to be processed.</param>
        /// <param name="priority">The priority at which to process the file.</param>
        public void ProcessFile(string filePath, int priority, int crc)
        {
            string connectionString = LoadSystemSettings();
            SystemSettings systemSettings = new SystemSettings(connectionString);
            string meterKey = GetMeterKey(filePath, systemSettings.FilePattern);

            // Get the thread used to process this data
            QueueForMeter(meterKey, priority, () =>
            {
                if (m_stopped || m_disposed)
                    return;

                FileWrapper fileWrapper = new FileWrapper(filePath);
                FileGroup fileGroup = new FileGroup();
                fileGroup.FileHash = crc;

                Action<DataProcessorState> processFileCallback = state =>
                {
                    try
                    {
                        // Set up thread state for logging and reporting
                        ThreadContext.Properties["Meter"] = state.MeterKey;
                        m_activeFiles[state.MeterKey] = state.FilePath;

                        // Sets the processing start time of the file group
                        BeginProcessing(state);

                        // Create the appropriate data reader
                        // and use it to parse the file
                        ReadFile(state);

                        if (state.Retry)
                            return;

                        // Create the data file records to be saved to the database along with the file group
                        state.FileGroup.DataFiles = state.FileWrapper.CreateDataFiles(state.SystemSettings.XDATimeZoneInfo);

                        // Determine whether the file duration is within a user-defined maximum tolerance
                        ValidateFileDuration(state.FilePath, state.FileGroup, state.SystemSettings);

                        // Determine whether the timestamps in the file extend beyond user-defined thresholds
                        ValidateFileTimestamps(state.FilePath, state.FileGroup, state.SystemSettings);

                        // Save the file group in the database now that the data has been successfully parsed
                        using (AdoDataConnection connection = state.MeterDataSet.CreateDbConnection())
                        {
                            SaveFileGroup(connection, fileGroup);
                        }

                        // Process the data that was parsed from the file
                        ProcessFile(state);

                        // Set the processing end time of the file
                        // group and save it to the database
                        CompleteProcessing(state);
                    }
                    finally
                    {
                        // Restore thread state for logging and reporting
                        string removedFilePath;
                        m_activeFiles.TryRemove(state.MeterKey, out removedFilePath);
                        ThreadContext.Properties.Remove("Meter");
                    }
                };

                Action<DataProcessorState> processFailureCallback = state =>
                {
                    // Set the error flag on the file group,
                    // then set the processing end time
                    // and save the file group to the database
                    state.FileGroup.Error = 1;
                    CompleteProcessing(state);
                };

                // Set up the data processor state and enter the processing loop
                EnterProcessingLoop(new DataProcessorState()
                {
                    FilePath = filePath,
                    FileWrapper = fileWrapper,
                    FileGroup = fileGroup,

                    MeterKey = meterKey,
                    ConnectionString = connectionString,
                    SystemSettings = systemSettings,

                    ProcessFileCallback = processFileCallback,
                    ProcessFailureCallback = processFailureCallback
                });
            });
        }

        /// <summary>
        /// Processes a group of data files that have already been processed.
        /// </summary>
        /// <param name="fileGroupId">The ID of the file group representing the group of files to be processed again.</param>
        /// <param name="meterId">The ID of the meter representing the group of files to be processed again.</param>
        public void ReprocessFile(int fileGroupId, int meterId)
        {
            FileGroup fileGroup;
            string meterKey;
            string connectionString = LoadSystemSettings();
            SystemSettings systemSettings = new SystemSettings(connectionString);
            string filePath;

            try
            {
                // Get the list of data files in the file group
                using (AdoDataConnection connection = CreateDbConnection(systemSettings))
                {
                    TableOperations<Meter> meterTable = new TableOperations<Meter>(connection);
                    TableOperations<FileGroup> fileGroupTable = new TableOperations<FileGroup>(connection);
                    TableOperations<DataFile> dataFileTable = new TableOperations<DataFile>(connection);

                    fileGroup = fileGroupTable.QueryRecordWhere("ID = {0}", fileGroupId);

                    fileGroup.DataFiles = dataFileTable
                        .QueryRecordsWhere("FileGroupID = {0}", fileGroupId)
                        .ToList();

                    // Determine which meter the file is associated with
                    meterKey = meterTable.QueryRecordWhere("ID = {0}", meterId).AssetKey;
                }

                // Get the first file that matches the file processor's filter
                string[] filters = m_fileProcessor.Filter.Split(Path.PathSeparator);

                filePath = fileGroup.DataFiles
                    .Select(dataFile => dataFile.FilePath)
                    .FirstOrDefault(dataFile => FilePath.IsFilePatternMatch(filters, dataFile, true));

                if ((object)filePath == null)
                    throw new InvalidOperationException("There are no files in the file group that match the file processor's filter.");

            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return;
            }

            QueueForMeter(meterKey, RequeuePriority, () =>
            {
                if (m_stopped || m_disposed)
                    return;

                DateTime maxFileCreationTime = fileGroup.DataFiles.Max(dataFile => dataFile.CreationTime);
                FileWrapper fileWrapper = new FileWrapper(filePath, maxFileCreationTime);
                string tempFolderPath = Path.Combine(Path.GetTempPath(), "openXDA", "Reprocessor", meterKey);

                fileGroup.ProcessingEndTime = DateTime.MinValue;

                using (AdoDataConnection connection = CreateDbConnection(systemSettings))
                {
                    // If the data files don't exist at their original location,
                    // write them out to a temp directory so they can be processed
                    if (fileGroup.DataFiles.Any(dataFile => !File.Exists(dataFile.FilePath)))
                    {
                        string fileName = Path.GetFileName(filePath);
                        string tempFilePath = Path.Combine(tempFolderPath, fileName);
                        Directory.CreateDirectory(tempFolderPath);

                        foreach (DataFile dataFile in fileGroup.DataFiles)
                        {
                            byte[] dataFileBlob = connection.ExecuteScalar<byte[]>("SELECT Blob FROM FileBlob WHERE DataFileID = {0}", dataFile.ID);
                            string dataFileName = Path.GetFileName(dataFile.FilePath);
                            string tempDataFilePath = Path.Combine(tempFolderPath, dataFileName);
                            File.WriteAllBytes(tempDataFilePath, dataFileBlob);
                        }

                        filePath = tempFilePath;
                    }
                }

                Action<DataProcessorState> processFileCallback = state =>
                {
                    try
                    {
                        // Set up thread state for logging and reporting
                        ThreadContext.Properties["Meter"] = state.MeterKey;
                            m_activeFiles[state.MeterKey] = state.FilePath;

                        // Sets the processing start time of the file group
                        BeginProcessing(state);

                        // Create the appropriate data reader
                        // and use it to parse the file
                        ReadFile(state);

                        if (state.Retry)
                            return;

                        // Process the data that was parsed from the file
                        ProcessFile(state);

                        // Set the processing end time of the file
                        // group and save it to the database
                        CompleteProcessing(state);
                    }
                    finally
                    {
                        // Restore thread state for logging and reporting
                        string removedFilePath;
                        m_activeFiles.TryRemove(state.MeterKey, out removedFilePath);
                        ThreadContext.Properties.Remove("Meter");
                    }
                };

                Action<DataProcessorState> processFailureCallback = state =>
                {
                    // Set the error flag on the file group,
                    // then set the processing end time
                    // and save the file group to the database
                    state.FileGroup.Error = 1;
                        CompleteProcessing(state);
                };

                // Set up the data processor state and enter the processing loop
                EnterProcessingLoop(new DataProcessorState()
                {
                    FilePath = filePath,
                    FileWrapper = fileWrapper,
                    FileGroup = fileGroup,

                    MeterKey = meterKey,
                    ConnectionString = connectionString,
                    SystemSettings = systemSettings,

                    ProcessFileCallback = processFileCallback,
                    ProcessFailureCallback = processFailureCallback
                });
            });
        }

        /// <summary>
        /// Restores the event email engine to a working state after a trip has occurred.
        /// </summary>
        public void RestoreEventEmails()
        {
            m_eventEmailEngine.Restore();
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
                string connectionString = LoadSystemSettings();
                SystemSettings systemSettings = new SystemSettings(connectionString);
                ValidateFileCreationTime(filePath, systemSettings);

                using (AdoDataConnection connection = CreateDbConnection(systemSettings))
                {
                    TableOperations<Meter> meterTable = new TableOperations<Meter>(connection);
                    string meterKey = GetMeterKey(filePath, systemSettings.FilePattern);
                    ValidateMeterKey(filePath, meterKey, meterTable);
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
                    Log.Warn(ex.Message, ex);

                return false;
            }
        }

        // Called when the file processor has picked up a file in one of the watch
        // directories. This handler validates the file and processes it if able.
        private void FileProcessor_Processing(object sender, FileProcessorEventArgs fileProcessorEventArgs)
        {
            if (m_stopped || m_disposed)
                return;

            try
            {
                string filePath;
                int priority;

                filePath = fileProcessorEventArgs.FullPath;

                priority = fileProcessorEventArgs.RaisedByFileWatcher
                    ? FileWatcherPriority
                    : FileEnumerationPriority;

                // Determine whether the file has already been
                // processed or needs to be processed again
                if (fileProcessorEventArgs.AlreadyProcessed)
                {
                    using (AdoDataConnection connection = CreateDbConnection(m_systemSettings))
                    {
                        TableOperations<DataFile> dataFileTable = new TableOperations<DataFile>(connection);
                        TableOperations<FileGroup> fileGroupTable = new TableOperations<FileGroup>(connection);

                        DataFile dataFile = dataFileTable
                            .QueryRecordsWhere("FilePathHash = {0} AND FilePath = {1}", filePath.GetHashCode(), filePath)
                            .MaxBy(file => file.ID);

                        if ((object)dataFile != null)
                        {
                            FileGroup fileGroup = fileGroupTable.QueryRecordWhere("ID = {0}", dataFile.FileGroupID);

                            // This will tell us whether the service was stopped in the middle
                            // of processing the last time it attempted to process the file
                            if (fileGroup.ProcessingEndTime > DateTime.MinValue)
                            {
                                // Explicitly use Log.Debug() so that the message does not appear on the remote console,
                                // but include a FileSkippedException so that the message gets routed to the skipped files log
                                FileSkippedException ex = new FileSkippedException($"Skipped file \"{filePath}\" because it has already been processed.");
                                Log.Debug(ex.Message, ex);
                                return;
                            }
                        }
                    }
                }

                byte[] buffer;

                try
                {
                    buffer = File.ReadAllBytes(filePath);
                }
                catch (IOException)
                {
                    // Couldn't read from the file, likely because the
                    // process writing the file isn't finished writing
                    fileProcessorEventArgs.Requeue = true;
                    return;
                }

                // If the file is empty, we retry a number of times waiting
                // for the contents of the file before ultimately failing
                if (buffer.Length == 0)
                {
                    string message = $"Skipped file \"{filePath}\" because it is empty.";

                    if (fileProcessorEventArgs.AlreadyProcessed)
                        Log.Debug(message, new FileSkippedException(message));
                    else if (fileProcessorEventArgs.RetryCount < 30)
                        fileProcessorEventArgs.Requeue = true;
                    else
                        throw new FileSkippedException(message);

                    return;
                }

                Crc32 crc32 = new Crc32();
                crc32.Update(buffer);
                int crc = (int)crc32.Value;

                if (m_systemSettings.SkipOnCRCHashMatch)
                {
                    using (AdoDataConnection connection = CreateDbConnection(m_systemSettings))
                    {
                        TableOperations<FileGroup> to = new TableOperations<FileGroup>(connection);
                        int fileGroupCount = to.QueryRecordCountWhere("FileHash = {0}", crc);

                        if (fileGroupCount != 0)
                        {
                            TableOperations<FileBlob> to2 = new TableOperations<FileBlob>(connection);
                            int fileBlobCount = to2.QueryRecordCountWhere("DataFileID IN (SELECT ID FROM DataFile WHERE FileGroupID IN (SELECT ID FROM FileGroup WHERE FileHash = {0})) AND Blob = {1}", crc, buffer);

                            if (fileBlobCount != 0)
                            {
                                // Explicitly use Log.Debug() so that the message does not appear on the remote console,
                                // but include a FileSkippedException so that the message gets routed to the skipped files log
                                FileSkippedException ex = new FileSkippedException($"Skipped file \"{filePath}\" because it has already been processed.");
                                Log.Warn(ex.Message, ex);
                                return;
                            }
                        }
                    }
                }

                ProcessFile(filePath, priority, crc);
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
        }

        // Starts a loop to process the file.
        // Continues looping so long as the user continues requesting to requeue.
        private void EnterProcessingLoop(DataProcessorState state)
        {
            int retryCount = 0;
            Action delayAndProcess = null;

            // This method should always be called from the meter thread
            LogicalThread currentThread = LogicalThread.CurrentThread;
            LogicalThread meterThread = GetThread(state.MeterKey);

            if (!ReferenceEquals(currentThread, meterThread))
                throw new InvalidOperationException("EnterProcessingLoop must be called from the appropriate meter thread.");

            // Attempts to process the file and
            // returns the success indicator
            Func<bool> tryProcessFile = () =>
            {
                state.ProcessFileCallback(state);
                return !state.Retry;
            };

            // Always invoked on the meter thread -
            // attempts to process the file and kicks off
            // the delayAndProcess action on retry signal
            Action processFileTimer = () =>
            {
                if (m_stopped || m_disposed)
                    return;

                try
                {
                    int delay;

                    if (!tryProcessFile())
                    {
                        if (retryCount < 8)
                            delay = 250;
                        else if (retryCount < 21)
                            delay = 1000;
                        else if (retryCount < 30)
                            delay = 5000;
                        else if (retryCount < 34)
                            delay = 60000;
                        else
                            throw new FileSkippedException($"Skipped file \"{state.FilePath}\" because the system has been unable to process the file for the maximum allotted time.");

                        delayAndProcess.DelayAndExecute(delay);
                    }
                }
                catch (Exception ex)
                {
                    if (!IsFileSkippedException(ex))
                    {
                        try { state.ProcessFailureCallback(state); }
                        catch (Exception inner) { OnProcessException(inner); }
                    }

                    throw;
                }
            };

            // Always invoked after a delay -
            // pushes the process file timer onto the processing thread
            delayAndProcess = () => QueueForMeter(state.MeterKey, RequeuePriority, processFileTimer);

            // Execute the timer action
            // to kick off the async loop
            processFileTimer();
        }

        // Sets up state for logging and reporting.
        private void BeginProcessing(DataProcessorState state)
        {
            FileGroup fileGroup = state.FileGroup;
            SystemSettings systemSettings = state.SystemSettings;
            TimeZoneInfo xdaTimeZoneInfo = systemSettings.XDATimeZoneInfo;
            DateTime utcNow = DateTime.UtcNow;
            DateTime xdaNow = TimeZoneInfo.ConvertTimeFromUtc(utcNow, xdaTimeZoneInfo);
            fileGroup.ProcessingStartTime = xdaNow;
        }

        // Finds the data reader for the file and uses it to parse the file.
        private void ReadFile(DataProcessorState state)
        {
            string meterKey = state.MeterKey;
            string filePath = state.FilePath;

            // Get the data reader that will be used to parse the file
            SystemSettings systemSettings = state.SystemSettings;
            DataReader dataReaderDefinition;

            using (AdoDataConnection connection = CreateDbConnection(systemSettings))
            {
                TableOperations<DataReader> dataReaderTable = new TableOperations<DataReader>(connection);

                dataReaderDefinition = dataReaderTable
                    .QueryRecords("LoadOrder")
                    .FirstOrDefault(reader => FilePath.IsFilePatternMatch(reader.FilePattern, filePath, true));
            }

            if ((object)dataReaderDefinition == null)
            {
                // Because the file processor is filtering files based on the DataReader file patterns,
                // this should only ever occur if the configuration changes during runtime
                UpdateFileProcessorFilter(systemSettings);

                throw new FileSkippedException($"Skipped file \"{filePath}\" because no data reader could be found to process the file.");
            }

            // Instantiate the data reader definition and wrap the object in a
            // data reader wrapper so we can safely dispose in a using block
            using (DataReaderWrapper dataReaderWrapper = Wrap(dataReaderDefinition))
            {
                IDataReader dataReader = dataReaderWrapper.DataObject;

                // Apply connection string settings to the data reader
                ConnectionStringParser.ParseConnectionString(state.ConnectionString, dataReader);

                // Determine whether the dataReader can parse the file
                state.Retry = !dataReader.CanParse(filePath, state.FileWrapper.GetMaxFileCreationTime());

                if (state.Retry)
                    return;

                // Call the method to parse the file
                state.MeterDataSet = ParseFile(dataReader, filePath);

                if ((object)state.MeterDataSet != null)
                {
                    state.MeterDataSet.CreateDbConnection = () => CreateDbConnection(systemSettings);
                    state.MeterDataSet.FilePath = filePath;
                    state.MeterDataSet.FileGroup = state.FileGroup;
                    state.MeterDataSet.ConnectionString = state.ConnectionString;
                    state.MeterDataSet.Meter.AssetKey = meterKey;
                }
            }

            // Shift date/time values to the configured time zone and set the start and end time values on the file group
            MeterDataSet meterDataSet = state.MeterDataSet;
            ShiftTime(meterDataSet, meterDataSet.Meter.GetTimeZoneInfo(systemSettings.DefaultMeterTimeZoneInfo), systemSettings.XDATimeZoneInfo);
            SetDataTimeRange(meterDataSet);
        }

        // Processes the data that was parsed from the file.
        private void ProcessFile(DataProcessorState state)
        {
            string filePath = state.FilePath;
            FileGroup fileGroup = state.FileGroup;

            // Process the meter data set
            OnStatusMessage($"Processing file '{filePath}'...");
            ProcessMeterDataSet(state.MeterDataSet);
            OnStatusMessage($"Finished processing file '{filePath}'.");

            // Send data set to email engine for processing
            m_eventEmailEngine.Process(state.MeterDataSet);
        }

        // Saves the file group to the database and cleans up state for logging and reporting.
        private void CompleteProcessing(DataProcessorState state)
        {
            try
            {
                SystemSettings systemSettings = state.SystemSettings;
                FileGroup fileGroup = state.FileGroup;

                using (AdoDataConnection connection = CreateDbConnection(systemSettings))
                {
                    DateTime utcNow = DateTime.UtcNow;
                    TimeZoneInfo xdaTimeZone = systemSettings.XDATimeZoneInfo;
                    DateTime xdaNow = TimeZoneInfo.ConvertTimeFromUtc(utcNow, xdaTimeZone);
                    fileGroup.ProcessingEndTime = xdaNow;
                    SaveFileGroup(connection, fileGroup);
                }
            }
            catch (Exception ex)
            {
                OnProcessException(ex);
            }
        }

        // Parses the file on the meter's processing thread and kicks off processing of the meter data set.
        private MeterDataSet ParseFile(IDataReader dataReader, string filePath)
        {
            // Parse the file to turn it into a meter data set
            OnStatusMessage($"Parsing data from file \"{filePath}\"...");
            dataReader.Parse(filePath);
            OnStatusMessage($"Finished parsing data from file \"{filePath}\".");

            return dataReader.MeterDataSet;
        }

        // Instantiates and executes data operations and data writers to process the meter data set.
        private void ProcessMeterDataSet(MeterDataSet meterDataSet)
        {
            List<DataOperation> dataOperationDefinitions;

            using (AdoDataConnection connection = meterDataSet.CreateDbConnection())
            {
                TableOperations<DataOperation> dataOperationTable = new TableOperations<DataOperation>(connection);

                // Load the data operations from the database,
                // in descending order so we can remove records while we iterate
                dataOperationDefinitions = dataOperationTable
                    .QueryRecords("LoadOrder")
                    .ToList();
            }

            foreach (DataOperation dataOperationDefinition in dataOperationDefinitions)
            {
                try
                {
                    Log.Debug($"Executing data operation '{dataOperationDefinition.UnqualifiedTypeName}'...");

                    // Call the execute method on the data operation to perform in-memory data transformations
                    using (DataOperationWrapper wrapper = Wrap(dataOperationDefinition))
                    {
                        IDataOperation dataOperation = wrapper.DataObject;

                        ConnectionStringParser.ParseConnectionString(meterDataSet.ConnectionString, dataOperation);

                        dataOperation.Execute(meterDataSet);
                    }

                    Log.Debug($"Finished executing data operation '{dataOperationDefinition.UnqualifiedTypeName}'.");
                }
                catch (Exception ex)
                {
                    // Log the error and skip to the next data operation
                    string message = $"An error occurred while executing data operation of type '{dataOperationDefinition.TypeName}' on data from meter '{meterDataSet.Meter.AssetKey}': {ex.Message}";
                    OnProcessException(new Exception(message, ex));
                }
            }
        }

        // Saves the given file group to the database using the given connection.
        private void SaveFileGroup(AdoDataConnection connection, FileGroup fileGroup)
        {
            // Attempt to set the error flag on the file group
            TableOperations<FileGroup> fileGroupTable = new TableOperations<FileGroup>(connection);

            fileGroupTable.AddNewOrUpdateRecord(fileGroup);

            if (fileGroup.ID == 0)
                fileGroup.ID = connection.ExecuteScalar<int>("SELECT @@IDENTITY");

            TableOperations<DataFile> dataFileTable = new TableOperations<DataFile>(connection);
            TableOperations<FileBlob> fileBlobTable = new TableOperations<FileBlob>(connection);

            foreach (DataFile dataFile in fileGroup.DataFiles)
            {
                dataFile.FileGroupID = fileGroup.ID;
                dataFileTable.AddNewOrUpdateRecord(dataFile);

                if (dataFile.ID == 0)
                    dataFile.ID = connection.ExecuteScalar<int>("SELECT @@IDENTITY");

                if (File.Exists(dataFile.FilePath) && fileBlobTable.QueryRecordCountWhere("DataFileID = {0}", dataFile.ID) == 0)
                {
                    FileBlob fileBlob = new FileBlob();
                    fileBlob.DataFileID = dataFile.ID;
                    fileBlob.Blob = File.ReadAllBytes(dataFile.FilePath);
                    fileBlobTable.AddNewOrUpdateRecord(fileBlob);
                }
            }
        }

        // Updates the Filter property of the FileProcessor with the
        // latest collection of filters from the DataReader table.
        private void UpdateFileProcessorFilter(SystemSettings systemSettings)
        {
            List<string> filterPatterns;

            // Do not attempt to load filter patterns if file processor is not defined
            if ((object)m_fileProcessor == null)
                return;

            // Get the list of file extensions to be processed by openXDA
            using (AdoDataConnection connection = CreateDbConnection(systemSettings))
            {
                TableOperations<DataReader> dataReaderTable = new TableOperations<DataReader>(connection);

                filterPatterns = dataReaderTable
                    .QueryRecords()
                    .Select(reader => reader.FilePattern)
                    .ToList();
            }

            m_fileProcessor.Filter = string.Join(Path.PathSeparator.ToString(), filterPatterns);
        }

        private void QueueForMeter(string meterKey, int priority, Action action)
        {
            m_eventEmailEngine.StopTimer();
            LogicalThread meterThread = GetThread(meterKey);
            Interlocked.Increment(ref m_meterTaskCount);

            meterThread.Push(priority, () =>
            {
                try
                {
                    action();
                }
                finally
                {
                    int meterTaskCount = Interlocked.Decrement(ref m_meterTaskCount);

                    if (meterTaskCount == 0)
                        m_eventEmailEngine.StartTimer();
                }
            });
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
                        Log.Warn(ex.Message, ex);
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
            using (AdoDataConnection connection = new AdoDataConnection(m_dbConnectionString, typeof(SqlConnection), typeof(SqlDataAdapter)))
            {
                return LoadSystemSettings(connection);
            }
        }

        // Loads system settings from the database.
        private string LoadSystemSettings(AdoDataConnection connection)
        {
            TableOperations<Setting> settingTable = new TableOperations<Setting>(connection);
            List<Setting> settingList = settingTable.QueryRecords().ToList();

            foreach (IGrouping<string, Setting> grouping in settingList.GroupBy(setting => setting.Name))
            {
                if (grouping.Count() > 1)
                    Log.Warn($"Duplicate record for setting {grouping.Key} detected.");
            }

            // Convert the Setting table to a dictionary
            Dictionary<string, string> settings = settingList
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

        /// <summary>
        /// Determines if the given exception is a file skipped exception.
        /// </summary>
        /// <param name="ex">The exception to be tested.</param>
        /// <returns>True if it is a file skipped exception; false otherwise.</returns>
        public static bool IsFileSkippedException(Exception ex)
        {
            return ex is FileSkippedException;
        }

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
        private static void SetDataTimeRange(MeterDataSet meterDataSet)
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

            if (dataStartTime != default(DateTime) || dataEndTime != default(DateTime))
            {
                using (AdoDataConnection connection = meterDataSet.CreateDbConnection())
                {
                    TableOperations<FileGroup> fileGroupTable = new TableOperations<FileGroup>(connection);

                    if (dataStartTime != default(DateTime))
                        meterDataSet.FileGroup.DataStartTime = dataStartTime;

                    if (dataEndTime != default(DateTime))
                        meterDataSet.FileGroup.DataEndTime = dataEndTime;

                    fileGroupTable.UpdateRecord(meterDataSet.FileGroup);
                }
            }
        }

        // Instantiates the given data writer and wraps it in a disposable wrapper object.
        private static ConfigurationLoaderWrapper Wrap(ConfigurationLoader loader)
        {
            try
            {
                Assembly assembly = Assembly.LoadFrom(loader.AssemblyName);
                Type type = assembly.GetType(loader.TypeName);
                return new ConfigurationLoaderWrapper(loader.ID, type);
            }
            catch (Exception ex)
            {
                string message = $"Failed to create configuration loader of type {loader.TypeName}: {ex.Message}";
                throw new TypeLoadException(message, ex);
            }
        }

        // Instantiates the given data reader and wraps it in a disposable wrapper object.
        private static DataReaderWrapper Wrap(DataReader reader)
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
        private static DataOperationWrapper Wrap(DataOperation operation)
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

        // Determines whether the file creation time exceeds a user-defined threshold.
        private static void ValidateFileCreationTime(string filePath, SystemSettings systemSettings)
        {
            double maxFileCreationTimeOffset = systemSettings.MaxFileCreationTimeOffset;
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
        private static void ValidateMeterKey(string filePath, string meterKey, TableOperations<Meter> meterTable)
        {
            // Determine whether there exists a meter whose asset key matches the given meterKey
            if (meterTable.QueryRecordCountWhere("AssetKey = {0}", meterKey) == 0)
                throw new FileSkippedException($"Skipped file \"{filePath}\" because no meter configuration was found for meter {meterKey}.");
        }

        // Determines whether the duration of data in the file exceeds a user-defined threshold.
        private static void ValidateFileDuration(string filePath, FileGroup fileGroup, SystemSettings systemSettings)
        {
            double maxFileDuration = systemSettings.MaxFileDuration;
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

        // Determines whether the timestamps in the file extend beyond user-defined thresholds.
        private static void ValidateFileTimestamps(string filePath, FileGroup fileGroup, SystemSettings systemSettings)
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

        // Creates a new database connection based on the given system configuration.
        private static AdoDataConnection CreateDbConnection(SystemSettings systemSettings)
        {
            string connectionString = systemSettings.DbConnectionString;
            Type connectionType = typeof(SqlConnection);
            Type adapterType = typeof(SqlDataAdapter);

            return new AdoDataConnection(connectionString, connectionType, adapterType)
            {
                DefaultTimeout = systemSettings.DbTimeout
            };
        }

        // Called when the file processor encounters an unexpected error.
        private static void FileProcessor_Error(object sender, ErrorEventArgs args)
        {
            Exception ex = args.GetException();

            if (ex is FileSkippedException)
                Log.Warn(ex.Message, ex);
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
