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
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using GSF.Annotations;
using GSF.Collections;
using GSF.Configuration;
using GSF.IO;
using GSF.IO.Checksums;
using GSF.Security.Model;
using log4net;
using Newtonsoft.Json.Linq;
using openXDA.DataPusher;
using openXDA.Model;
using openXDAFileWatcher.Configuration;
using FileShare = openXDAFileWatcher.Configuration.FileShare;

namespace openXDAFileWatcher
{
    /// <summary>
    /// Represents an engine that processes power quality data
    /// to determine the locations of faults along power lines.
    /// </summary>
    public class FileWatcherEngine : IDisposable
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
        private string m_xdaAddress;
        private UserAccount m_xdaUserAccount;
        private SystemSettings m_systemSettings;

        private FileProcessor m_fileProcessor;
        private ConcurrentDictionary<string, string> m_activeFiles;

        private bool m_stopped;
        private bool m_disposed;

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Creates a new instance of the <see cref="FileWatcherEngine"/> class.
        /// </summary>
        public FileWatcherEngine()
        {
            m_xdaUserAccount = new UserAccount();
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
                statusBuilder.AppendLine($"            XDA Address: {systemSettings.XDAAddress}");
                statusBuilder.AppendLine($"           XDA Username: {systemSettings.XDAUsername}");
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

            // Make sure watch directories exist
            foreach (string path in m_systemSettings.WatchDirectoryList)
                TryCreateDirectory(path);

            // Create the lookup table used to track which files are being processed
            if ((object)m_activeFiles == null)
                m_activeFiles = new ConcurrentDictionary<string, string>();

            // Setup new file processor to monitor the watch directories
            if ((object)m_fileProcessor == null)
            {
                m_fileProcessor = new FileProcessor(m_systemSettings.FileProcessorID);
                m_fileProcessor.FolderExclusion = m_systemSettings.FolderExclusion;
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
        /// Reloads system settings from the database.
        /// </summary>
        public void ReloadSystemSettings()
        {
            // Reload the configuration file
            ConfigurationFile configurationFile = ConfigurationFile.Current;
            configurationFile.Reload();

            // Retrieve the connection string from the config file
            CategorizedSettingsElementCollection category = configurationFile.Settings["systemSettings"];
            category.Add("XDAAddress", "http://localhost:8989/", "");
            category.Add("XDAUsername", "FileWatcher", "");
            category.Add("XDAPassword", "", "");
            m_xdaAddress = category["XDAAddress"].Value;
            m_xdaUserAccount.Name = category["XDAUsername"].Value;
            m_xdaUserAccount.Password = category["XDAPassword"].Value;

            // Load system settings from the database
            m_systemSettings = new SystemSettings(LoadSystemSettings());

            // Attempt to authenticate to configured file shares
            foreach (FileShare fileShare in m_systemSettings.FileShareList)
            {
                if (!fileShare.TryAuthenticate())
                    OnProcessException(fileShare.AuthenticationException);
            }

            // Update the FileProcessor with the latest system settings
            if ((object)m_fileProcessor != null)
            {
                m_fileProcessor.FolderExclusion = m_systemSettings.FolderExclusion;
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
                else if (args[1].Equals("FolderExclusion", StringComparison.OrdinalIgnoreCase))
                {
                    oldValue = m_fileProcessor.FolderExclusion;
                    m_fileProcessor.FolderExclusion = args[2];
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
                string connectionString = LoadSystemSettings();
                SystemSettings systemSettings = new SystemSettings(connectionString);
                ValidateFileCreationTime(filePath, systemSettings);

                string meterKey = GetMeterKey(filePath, systemSettings.FilePattern);
                ValidateMeterKey(systemSettings, filePath, meterKey);

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

                string connectionString = LoadSystemSettings();
                SystemSettings systemSettings = new SystemSettings(connectionString);

                UserAccount userAccount = new UserAccount();
                userAccount.Name = systemSettings.XDAUsername;
                userAccount.Password = systemSettings.XDAPassword;

                // Determine whether the file has already been
                // processed or needs to be processed again
                if (fileProcessorEventArgs.AlreadyProcessed)
                {
                    DataFile remoteDataFile = WebAPIHub.GetRecordsWhere<DataFile>(systemSettings.XDAAddress, $"FilePathHash = {filePath.GetHashCode()}", userAccount)
                        .Where(file => file.FilePath == filePath)
                        .MaxBy(file => file.ID);

                    if ((object)remoteDataFile != null)
                    {
                        FileGroup remoteFileGroup = WebAPIHub.GetRecordWhere<FileGroup>(systemSettings.XDAAddress, $"ID = {remoteDataFile.FileGroupID}", userAccount);

                        // This will tell us whether the service was stopped in the middle
                        // of processing the last time it attempted to process the file
                        if (remoteFileGroup.ProcessingEndTime > DateTime.MinValue)
                        {
                            // Explicitly use Log.Debug() so that the message does not appear on the remote console,
                            // but include a FileSkippedException so that the message gets routed to the skipped files log
                            FileSkippedException ex = new FileSkippedException($"Skipped file \"{filePath}\" because it has already been processed.");
                            Log.Debug(ex.Message, ex);
                            return;
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

                Crc32 crc32 = new Crc32();
                crc32.Update(buffer);
                int crc = (int)crc32.Value;

                if (systemSettings.SkipOnCRCHashMatch)
                {
                    List<FileGroup> fileGroups = WebAPIHub.GetRecordsWhere<FileGroup>(systemSettings.XDAAddress, $"FileHash = {crc}", userAccount)
                        .ToList();

                    if (fileGroups.Any())
                    {
                        string fileGroupIDs = string.Join(",", fileGroups.Select(fg => fg.ID));

                        List<DataFile> dataFiles = WebAPIHub.GetRecordsWhere<DataFile>(systemSettings.XDAAddress, $"FileGroupID IN ({fileGroupIDs})", userAccount)
                            .ToList();

                        if (dataFiles.Any())
                        {
                            string dataFileIDs = string.Join(",", dataFiles.Select(dataFile => dataFile.ID));

                            List<FileBlob> fileBlobs = WebAPIHub.GetRecordsWhere<FileBlob>(systemSettings.XDAAddress, $"DataFileID IN ({dataFileIDs})", userAccount)
                                .ToList();

                            if (fileBlobs.Any(fileBlob => buffer.SequenceEqual(fileBlob.Blob)))
                            {
                                FileSkippedException ex = new FileSkippedException($"Skipped file \"{filePath}\" because it has already been processed.");
                                Log.Warn(ex.Message, ex);
                                return;
                            }
                        }
                    }
                }

                Log.Info($"Pushing file {filePath} into openXDA...");

                FileGroup fileGroup = new FileGroup();
                fileGroup.FileHash = crc;
                fileGroup.DataFiles = CreateDataFiles(filePath, systemSettings.XDATimeZoneInfo);
                SaveFileGroup(fileGroup, systemSettings.XDAAddress, userAccount);

                string meterKey = GetMeterKey(filePath, systemSettings.FilePattern);
                Meter meter = WebAPIHub.GetRecordsWhere<Meter>(systemSettings.XDAAddress, $"AssetKey = '{meterKey}'", userAccount).FirstOrDefault();

                Dictionary<string, int> idObject = new Dictionary<string, int>();
                idObject.Add("FileGroupID", fileGroup.ID);
                idObject.Add("MeterID", meter.ID);
                WebAPIHub.ProcessFileGroup(systemSettings.XDAAddress, JObject.FromObject(idObject), userAccount);

                Log.Info($"Finished pushing file {filePath} into openXDA.");
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

        /// <summary>
        /// Creates new <see cref="DataFile"/> objects for each file in the file group.
        /// </summary>
        /// <param name="xdaTimeZone">The time zone used by openXDA.</param>
        /// <returns>The list of data files.</returns>
        private List<DataFile> CreateDataFiles(string filePath, TimeZoneInfo xdaTimeZone)
        {
            string directoryPath = FilePath.GetDirectoryName(filePath);
            string fileNameWithoutExtension = FilePath.GetFileNameWithoutExtension(filePath);
            string filePathWithoutExtension = Path.Combine(directoryPath, fileNameWithoutExtension);

            List<DataFile> dataFiles = new List<DataFile>();
            DataFile dataFile;
            FileInfo fileInfo;

            foreach (string dataFilePath in FilePath.GetFileList($"{filePathWithoutExtension}.*"))
            {
                fileInfo = new FileInfo(dataFilePath);

                dataFile = new DataFile();
                dataFile.FilePath = dataFilePath;
                dataFile.FilePathHash = dataFilePath.GetHashCode();
                dataFile.FileSize = fileInfo.Length;
                dataFile.CreationTime = TimeZoneInfo.ConvertTimeFromUtc(fileInfo.CreationTimeUtc, xdaTimeZone);
                dataFile.LastWriteTime = TimeZoneInfo.ConvertTimeFromUtc(fileInfo.LastWriteTimeUtc, xdaTimeZone);
                dataFile.LastAccessTime = TimeZoneInfo.ConvertTimeFromUtc(fileInfo.LastAccessTimeUtc, xdaTimeZone);

                dataFiles.Add(dataFile);
            }

            return dataFiles;
        }

        // Saves the given file group to the database using the given connection.
        private void SaveFileGroup(FileGroup fileGroup, string xdaAddress, UserAccount userAccount)
        {
            fileGroup.ID = WebAPIHub.CreateRecord(xdaAddress, fileGroup, userAccount);

            foreach (DataFile dataFile in fileGroup.DataFiles)
            {
                dataFile.FileGroupID = fileGroup.ID;
                dataFile.ID = WebAPIHub.CreateRecord(xdaAddress, dataFile, userAccount);

                FileBlob fileBlob = new FileBlob();
                fileBlob.DataFileID = dataFile.ID;
                fileBlob.Blob = File.ReadAllBytes(dataFile.FilePath);
                WebAPIHub.CreateRecord(xdaAddress, fileBlob, userAccount);

            }
        }

        // Updates the Filter property of the FileProcessor with the
        // latest collection of filters from the DataReader table.
        private void UpdateFileProcessorFilter(SystemSettings systemSettings)
        {
            // Do not attempt to load filter patterns if file processor is not defined
            if ((object)m_fileProcessor == null)
                return;

            UserAccount userAccount = new UserAccount();
            userAccount.Name = systemSettings.XDAUsername;
            userAccount.Password = systemSettings.XDAPassword;

            // Get the list of file extensions to be processed by openXDA
            List<string> filterPatterns = WebAPIHub.GetRecords<DataReader>(systemSettings.XDAAddress, "all", userAccount)
                .Select(reader => reader.FilePattern)
                .ToList();

            m_fileProcessor.Filter = string.Join(Path.PathSeparator.ToString(), filterPatterns);
        }

        // Loads system settings from the database.
        private string LoadSystemSettings()
        {
            List<Setting> settingList = WebAPIHub.GetRecords<Setting>(m_xdaAddress, "all", m_xdaUserAccount)
                .ToList();

            foreach (IGrouping<string, Setting> grouping in settingList.GroupBy(setting => setting.Name))
            {
                if (grouping.Count() > 1)
                    Log.Warn($"Duplicate record for setting {grouping.Key} detected.");
            }

            // Convert the Setting table to a dictionary
            Dictionary<string, string> settings = settingList
                .DistinctBy(setting => setting.Name)
                .ToDictionary(setting => setting.Name, setting => setting.Value, StringComparer.OrdinalIgnoreCase);

            // Add the configuration files settings if they are not
            // already explicitly specified in the Setting table
            if (!settings.ContainsKey("XDAAddress"))
                settings.Add("XDAAddress", m_xdaAddress);

            if (!settings.ContainsKey("XDAUsername"))
                settings.Add("XDAUsername", m_xdaUserAccount.Name);

            if (!settings.ContainsKey("XDAPassword"))
                settings.Add("XDAPassword", m_xdaUserAccount.Password);

            // Convert dictionary to a connection string and return it
            return SystemSettings.ToConnectionString(settings);
        }

        #endregion

        #region [ Static ]

        // Static Fields
        private static readonly ConnectionStringParser<SettingAttribute, CategoryAttribute> ConnectionStringParser = new ConnectionStringParser<SettingAttribute, CategoryAttribute>();
        private static readonly ILog Log = LogManager.GetLogger(typeof(FileWatcherEngine));

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
        private static void ValidateMeterKey(SystemSettings systemSettings, string filePath, string meterKey)
        {
            UserAccount xdaUserAccount = new UserAccount();
            xdaUserAccount.Name = systemSettings.XDAUsername;
            xdaUserAccount.Password = systemSettings.XDAPassword;

            // Determine whether there exists a meter whose asset key matches the given meterKey
            if (WebAPIHub.GetRecordsWhere<Meter>(systemSettings.XDAAddress, $"AssetKey = '{meterKey}'", xdaUserAccount).Count() == 0)
                throw new FileSkippedException($"Skipped file \"{filePath}\" because no meter configuration was found for meter {meterKey}.");
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