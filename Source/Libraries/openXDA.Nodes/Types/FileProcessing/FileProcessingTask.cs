//******************************************************************************************************
//  FileProcessingTask.cs - Gbtc
//
//  Copyright © 2021, Grid Protection Alliance.  All Rights Reserved.
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
//  01/17/2021 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using FaultData.DataReaders;
using GSF.Collections;
using GSF.Configuration;
using GSF.Data;
using GSF.Data.Model;
using log4net;
using openXDA.Configuration;
using openXDA.Model;
using openXDA.Nodes.Types.Analysis;
using AnalysisTask = openXDA.Nodes.Types.Analysis.AnalysisTask;
using IDataReader = FaultData.DataReaders.IDataReader;

namespace openXDA.Nodes.Types.FileProcessing
{
    internal class FileProcessingTask
    {
        #region [ Members ]

        // Nested Types
        private class TaskSettings
        {
            public TaskSettings(Action<object> configure) =>
                configure(this);

            [Category]
            [SettingName(FileProcessorSection.CategoryName)]
            public FileProcessorSection FileProcessorSettings { get; } = new FileProcessorSection();
        }

        #endregion

        #region [ Constructors ]

        public FileProcessingTask(FileInfo[] fileList, string filePath, int priority, Func<AdoDataConnection> connectionFactory, Action<object> configurator)
        {
            FileList = fileList;
            FilePath = filePath;
            Priority = priority;
            ConnectionFactory = connectionFactory;
            TimeZoneConverter = new TimeZoneConverter(configurator);
            Settings = new TaskSettings(configurator);
        }

        #endregion

        #region [ Properties ]

        private FileInfo[] FileList { get; }
        private string FilePath { get; }
        private int Priority { get; }
        private Func<AdoDataConnection> ConnectionFactory { get; }
        private TimeZoneConverter TimeZoneConverter { get; }
        private TaskSettings Settings { get; }

        #endregion

        #region [ Methods ]

        public void Execute()
        {
            ValidateFilesForProcessing();

            using (AdoDataConnection connection = ConnectionFactory())
            {
                bool hasChanges = CheckForChanges(connection);

                if (!hasChanges)
                    return;

                string filePattern = Settings.FileProcessorSettings.FilePattern;
                Meter meter = GetMeter(connection, FilePath, filePattern);
                FileGroup fileGroup = ToFileGroup(meter.ID);
                AnalysisTask analysisTask = new AnalysisTask(fileGroup, meter, Priority);
                AnalysisTaskPublisher taskPublisher = new AnalysisTaskPublisher(ConnectionFactory);
                taskPublisher.Publish(analysisTask);
            }
        }

        private void ValidateFilesForProcessing()
        {
            double maxFileCreationTimeOffset = Settings.FileProcessorSettings.MaxFileCreationTimeOffset;
            double maxFileSize = Settings.FileProcessorSettings.MaxFileSize;

            if (maxFileCreationTimeOffset > 0.0D)
            {
                DateTime now = DateTime.UtcNow;
                TimeSpan offset = TimeSpan.FromHours(maxFileCreationTimeOffset);
                DateTime threshold = now - offset;
                DateTime creationTime = FileList.Max(fileInfo => fileInfo.CreationTimeUtc);

                if (creationTime < threshold)
                {
                    string message = $"File creation time '{creationTime}' is too old.";
                    throw new FileSkippedException(false, message);
                }
            }

            if (maxFileSize > 0.0D)
            {
                double fileSizeMB = FileList
                    .Sum(fileInfo => fileInfo.Length / 1024.0D / 1024.0D);

                if (fileSizeMB > maxFileSize)
                {
                    string message = $"File size '{fileSizeMB} MB' is too large.";
                    throw new FileSkippedException(false, message);
                }
            }

            if (FileList.Any(fileInfo => !GSF.IO.FilePath.TryGetReadLockExclusive(fileInfo.FullName)))
            {
                string message = $"Exclusive lock could not be obtained.";
                throw new FileSkippedException(true, message);
            }

            DataReaderFactory readerFactory = new DataReaderFactory(ConnectionFactory);
            IDataReader reader = readerFactory.CreateDataReader(FileList);

            if (!reader.IsReadyForLoad(FileList))
            {
                string message = $"Reader is not ready to load the file group.";
                throw new FileSkippedException(true, message);
            }
        }

        private bool CheckForChanges(AdoDataConnection connection)
        {
            const string QueryFormat =
                "SELECT " +
                "    CASE WHEN LastWriteTime >= {0} " +
                "        THEN 1 " +
                "        ELSE 0 " +
                "    END AlreadyProcessed " +
                "FROM DataFile " +
                "WHERE " +
                "    FilePathHash = {1} AND " +
                "    FilePath = {2}";

            DateTime lastWriteTimeUTC = File.GetLastWriteTimeUtc(FilePath);
            DateTime lastWriteTime = TimeZoneConverter.ToXDATimeZone(lastWriteTimeUTC);
            int filePathHash = DataFile.GetHash(FilePath);
            bool alreadyProcessed = connection.ExecuteScalar<bool>(QueryFormat, lastWriteTime, filePathHash, FilePath);

            if (alreadyProcessed)
            {
                // Explicitly use Log.Debug() so that the message does not appear on the remote console,
                // but include a FileSkippedException so that the message gets routed to the skipped files log
                FileSkippedException ex = new FileSkippedException(false, $"Skipped file \"{FilePath}\" because path and timestamp indicate it has already been processed.");
                Log.Debug(ex.Message, ex);
            }

            return !alreadyProcessed;
        }

        private FileGroup ToFileGroup(int meterID)
        {
            FileBlob ToFileBlob(string blobPath) => new FileBlob()
            {
                Blob = File.ReadAllBytes(blobPath)
            };

            DataFile ToDataFile(FileInfo fileInfo) => new DataFile()
            {
                FilePath = fileInfo.FullName,
                FilePathHash = DataFile.GetHash(fileInfo.FullName),
                FileSize = fileInfo.Length,
                CreationTime = TimeZoneConverter.ToXDATimeZone(fileInfo.CreationTimeUtc),
                LastWriteTime = TimeZoneConverter.ToXDATimeZone(fileInfo.LastWriteTimeUtc),
                LastAccessTime = TimeZoneConverter.ToXDATimeZone(fileInfo.LastAccessTimeUtc),
                FileBlob = ToFileBlob(fileInfo.FullName)
            };

            try
            {
                string directory = Path.GetDirectoryName(FilePath);
                DirectoryInfo directoryInfo = new DirectoryInfo(directory);

                string fileName = Path.GetFileName(FilePath);
                string searchPattern = Path.ChangeExtension(fileName, ".*");
                FileInfo[] fileList = directoryInfo.GetFiles(searchPattern);

                FileGroup fileGroup = new FileGroup();
                fileGroup.MeterID = meterID;
                fileGroup.DataFiles = fileList.Select(ToDataFile).ToList();
                return fileGroup;
            }
            catch (IOException)
            {
                string message = $"Skipped file \"{FilePath}\" because data could not be retrieved for the whole file group.";
                throw new FileSkippedException(true, message);
            }
        }

        private Meter GetMeter(AdoDataConnection connection, string filePath, string filePattern)
        {
            TableOperations<Meter> meterTable = new TableOperations<Meter>(connection);
            string meterKey = GetMeterKey(filePath, filePattern);
            Meter meter = meterTable.QueryRecordWhere("AssetKey = {0}", meterKey);

            if (meter is null)
                throw new FileSkippedException(false, $"No meter configuration was found for meter {meterKey}.");

            return meter;
        }

        #endregion

        #region [ Static ]

        // Static Fields
        private static readonly ILog Log = LogManager.GetLogger(typeof(FileProcessingTask));

        // Static Methods
        private static string GetMeterKey(string filePath, string filePattern)
        {
            Match match = Regex.Match(filePath, filePattern, RegexOptions.IgnoreCase);
            Group meterKeyGroup;

            if (!match.Success)
                throw new FileSkippedException(false, $"Skipped file \"{filePath}\" because the file path did not match the file pattern: \"{filePattern}\".");

            meterKeyGroup = match.Groups["AssetKey"];

            if (meterKeyGroup is null)
                throw new FileSkippedException(false, $"Skipped file \"{filePath}\" because the AssetKey capture group was not found when matching to the file pattern: \"{filePattern}\".");

            return meterKeyGroup.Value;
        }

        #endregion
    }
}
