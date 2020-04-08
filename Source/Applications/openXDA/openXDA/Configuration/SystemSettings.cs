//******************************************************************************************************
//  SystemSettings.cs - Gbtc
//
//  Copyright © 2014, Grid Protection Alliance.  All Rights Reserved.
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
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  09/30/2014 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using GSF;
using GSF.Configuration;
using GSF.IO;

namespace openXDA.Configuration
{
    /// <summary>
    /// Represents the system settings for openXDA.
    /// </summary>
    public class SystemSettings
    {
        #region [ Members ]

        private string m_watchDirectories;
        private string m_xdaTimeZone;
        private int m_processingThreadCount;
        private int m_fileWatcherInternalThreadCount;
        private string m_fileShares;

        private TimeZoneInfo m_defaultMeterTimeZoneInfo;
        private TimeZoneInfo m_xdaTimeZoneInfo;
        private List<string> m_watchDirectoryList;
        private List<FileShare> m_fileShareList;

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Creates a new instance of the <see cref="SystemSettings"/> class.
        /// </summary>
        public SystemSettings()
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="SystemSettings"/> class.
        /// </summary>
        /// <param name="connectionString">A string containing the system settings as key-value pairs.</param>
        public SystemSettings(string connectionString)
        {
            ConnectionStringParser<SettingAttribute> parser = new ConnectionStringParser<SettingAttribute>();
            parser.ParseConnectionString(connectionString, this);
        }

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets or sets the connection string to the database.
        /// </summary>
        [Setting]
        [DefaultValue("Data Source=localhost; Initial Catalog=openXDA; Integrated Security=SSPI")]
        public string DbConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the amount of time each database
        /// query is given to complete, in seconds.
        /// </summary>
        [Setting]
        [DefaultValue(120)]
        public int DbTimeout { get; set; }

        /// <summary>
        /// Gets or sets the list of directories to watch for files.
        /// </summary>
        [Setting]
        [DefaultValue("Watch")]
        public string WatchDirectories
        {
            get
            {
                return m_watchDirectories;
            }
            set
            {
                m_watchDirectories = value;

                if ((object)value != null)
                    m_watchDirectoryList = value.Split(Path.PathSeparator).ToList();
            }
        }

        /// <summary>
        /// Gets or sets the pattern used to parse file paths in
        /// order to identify the meter that the file came from.
        /// </summary>
        [Setting]
        [DefaultValue(@"(?<AssetKey>[^\\]+)\\[^\\]+$")]
        public string FilePattern { get; set; }

        /// <summary>
        /// Gets or sets the patterns used to determine which
        /// folders to skip when enumerating watch directories.
        /// </summary>
        [Setting]
        [DefaultValue("")]
        public string FolderExclusion { get; set; }

        /// <summary>
        /// Gets or sets the time zone identifier for the time zone
        /// used by meters in the system unless configured otherwise.
        /// </summary>
        [Setting]
        [DefaultValue("UTC")]
        public string DefaultMeterTimeZone { get; set; }

        /// <summary>
        /// Gets or sets the time zone identifier for the
        /// time zone used by openXDA to store data.
        /// </summary>
        /// <remarks>
        /// The default value for this setting (empty string)
        /// causes the setting to assume the value of the local
        /// time zone of the system openXDA is running on.
        /// </remarks>
        [Setting]
        [DefaultValue("UTC")]
        public string XDATimeZone
        {
            get
            {
                return m_xdaTimeZone;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    m_xdaTimeZone = TimeZoneInfo.Local.Id;
                else
                    m_xdaTimeZone = value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum distance, in seconds,
        /// between a meter's clock and real time.
        /// </summary>
        [Setting]
        [DefaultValue(0.5D)]
        public double TimeTolerance { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of hours beyond the current system time
        /// before the time of the record indicates that the data is unreasonable.
        /// </summary>
        [Setting]
        [DefaultValue(0.0D)]
        public double MaxTimeOffset { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of hours prior to the current system time
        /// before the time of the record indicates that the data is unreasonable.
        /// </summary>
        [Setting]
        [DefaultValue(0.0D)]
        public double MinTimeOffset { get; set; }

        /// <summary>
        /// Gets or sets the maximum duration, in seconds,
        /// of the files processed by openXDA.
        /// </summary>
        [Setting]
        [DefaultValue(0.0D)]
        public double MaxFileDuration { get; set; }

        /// <summary>
        /// Gets or sets the maximum duration, in seconds,
        /// of the events processed by openXDA.
        /// </summary>
        [Setting]
        [DefaultValue(0.0D)]
        public double MaxEventDuration { get; set; }

        /// <summary>
        /// Gets or sets the maximum file size, in MB,
        /// of the files processed by openXDA.
        /// </summary>
        [Setting]
        [DefaultValue(20.0D)]
        public double MaxFileSize { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of hours prior to the current system time
        /// before the file creation time indicates that the data should not be processed.
        /// </summary>
        [Setting]
        [DefaultValue(0.0D)]
        public double MaxFileCreationTimeOffset { get; set; }

        /// <summary>
        /// Gets or sets a boolean to skip on crc hash match.
        /// </summary>
        [Setting]
        [DefaultValue(true)]
        public bool SkipOnCRCHashMatch { get; set; }

        /// <summary>
        /// Gets or sets the system frequency.
        /// </summary>
        [Setting]
        [DefaultValue(60.0D)]
        public double SystemFrequency { get; set; }

        /// <summary>
        /// Gets or sets the per-unit threshold at which the
        /// voltage exceeds engineering reasonableness.
        /// </summary>
        [Setting]
        [DefaultValue(2.0D)]
        public double MaxVoltage { get; set; }

        /// <summary>
        /// Gets or sets the threshold, in amps, at which the
        /// current exceeds engineering reasonableness.
        /// </summary>
        [Setting]
        [DefaultValue(1000000.0D)]
        public double MaxCurrent { get; set; }

        /// <summary>
        /// Gets or sets the threshold, in amps, at which the
        /// current exceeds engineering reasonableness.
        /// </summary>
        [Setting]
        [DefaultValue(0.9D)]
        public double SagThreshold { get; set; }

        /// <summary>
        /// Gets or sets the threshold, in amps, at which the
        /// current exceeds engineering reasonableness.
        /// </summary>
        [Setting]
        [DefaultValue(1.1D)]
        public double SwellThreshold { get; set; }

        /// <summary>
        /// Gets or sets the threshold, in amps, at which the
        /// current exceeds engineering reasonableness.
        /// </summary>
        [Setting]
        [DefaultValue(0.1D)]
        public double InterruptionThreshold { get; set; }

        /// <summary>
        /// Gets or sets the units of measure to use
        /// for lengths (line length and fault distance).
        /// </summary>
        [Setting]
        [DefaultValue("miles")]
        public string LengthUnits { get; set; }

        /// <summary>
        /// Gets or sets the number of threads used
        /// for processing meter data concurrently.
        /// </summary>
        /// <remarks>
        /// Values less than or equal to zero will be set to the number of logical processors.
        /// </remarks>
        [Setting]
        [DefaultValue(0)]
        public int ProcessingThreadCount
        {
            get
            {
                return m_processingThreadCount;
            }
            set
            {
                m_processingThreadCount = value;

                if (m_processingThreadCount <= 0)
                    m_processingThreadCount = Environment.ProcessorCount;
            }
        }

        /// <summary>
        /// Gets or sets the ID of the file processor which determines
        /// the name of the file backed lookup table.
        /// </summary>
        [Setting]
        [SettingName("FileProcessorID")]
        [DefaultValue("4E3D3A90-6E7E-4AB7-96F3-3A5899081D0D")]
        public string FileProcessorIDSetting
        {
            get
            {
                return FileProcessorID.ToString();
            }
            set
            {
                FileProcessorID = Guid.Parse(value);
            }
        }

        /// <summary>
        /// Gets or sets the flag that indicates whether the local file watcher should be enabled.
        /// </summary>
        [Setting]
        [DefaultValue(true)]
        public bool FileWatcherEnabled { get; set; }

        /// <summary>
        /// Gets or sets the strategy used for enumeration of files in the file watcher.
        /// </summary>
        [Setting]
        [DefaultValue(FileProcessor.DefaultEnumerationStrategy)]
        public FileEnumerationStrategy FileWatcherEnumerationStrategy { get; set; }

        /// <summary>
        /// Gets or sets the flag that determines whether the file watcher
        /// should raise events for enumerated files in alphabetical order.
        /// </summary>
        [Setting]
        [DefaultValue(false)]
        public bool FileWatcherOrderedEnumeration { get; set; }

        /// <summary>
        /// Gets or sets the maximum amount of fragmentation allowed before
        /// compacting the list of processed files in the file watcher.
        /// </summary>
        [Setting]
        [DefaultValue(FileProcessor.DefaultMaxFragmentation)]
        public int FileWatcherMaxFragmentation { get; set; }

        /// <summary>
        /// Gets or sets the number of threads used
        /// internally to the file processor.
        /// </summary>
        /// <remarks>
        /// Values less than or equal to zero will be set to the number of logical processors.
        /// </remarks>
        [Setting]
        [DefaultValue(0)]
        public int FileWatcherInternalThreadCount
        {
            get
            {
                return m_fileWatcherInternalThreadCount;
            }
            set
            {
                m_fileWatcherInternalThreadCount = value;

                if (m_fileWatcherInternalThreadCount <= 0)
                    m_fileWatcherInternalThreadCount = Environment.ProcessorCount;
            }
        }

        /// <summary>
        /// Gets or sets the size of the
        /// <see cref="FileSystemWatcher"/>s' internal buffers.
        /// </summary>
        /// <seealso cref="FileSystemWatcher.InternalBufferSize"/>
        [Setting]
        [DefaultValue(65536)]
        public int FileWatcherBufferSize { get; set; }

        /// <summary>
        /// Gets or sets a list of parameters used
        /// to authenticate to multiple file shares.
        /// </summary>
        [Setting]
        [DefaultValue("")]
        public string FileShares
        {
            get
            {
                return m_fileShares;
            }
            set
            {
                m_fileShares = value;

                m_fileShareList = value.ToNonNullString().ParseKeyValuePairs()
                    .Select(kvp => kvp.Value)
                    .Select(fileShareString => new FileShare(fileShareString))
                    .ToList();
            }
        }

        /// <summary>
        /// Gets the ID of the file processor which determines
        /// the name of the file backed lookup table.
        /// </summary>
        public Guid FileProcessorID { get; private set; }

        /// <summary>
        /// Gets the <see cref="TimeZoneInfo"/> for the time zone
        /// used by meters in the system unless configured otherwise.
        /// </summary>
        public TimeZoneInfo DefaultMeterTimeZoneInfo
        {
            get
            {
                if ((object)m_defaultMeterTimeZoneInfo == null)
                    m_defaultMeterTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(DefaultMeterTimeZone);

                return m_defaultMeterTimeZoneInfo;
            }
        }

        /// <summary>
        /// Gets the <see cref="TimeZoneInfo"/> for the
        /// time zone used by openXDA to store data.
        /// </summary>
        public TimeZoneInfo XDATimeZoneInfo
        {
            get
            {
                if ((object)m_xdaTimeZoneInfo == null)
                    m_xdaTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(m_xdaTimeZone);

                return m_xdaTimeZoneInfo;
            }
        }

        /// <summary>
        /// Gets a list of directories to be watched
        /// for files containing fault records.
        /// </summary>
        public IReadOnlyCollection<string> WatchDirectoryList
        {
            get
            {
                return m_watchDirectoryList.AsReadOnly();
            }
        }

        /// <summary>
        /// Gets a list of file shares to be authenticated at startup.
        /// </summary>
        public IReadOnlyCollection<FileShare> FileShareList
        {
            get
            {
                return m_fileShareList.AsReadOnly();
            }
        }

        #endregion

        #region [ Methods ]

        public string ToConnectionString()
        {
            ConnectionStringParser<SettingAttribute> parser = new ConnectionStringParser<SettingAttribute>();
            parser.ExplicitlySpecifyDefaults = true;
            return parser.ComposeConnectionString(this);
        }

        #endregion

        #region [ Static ]

        // Static Fields
        private static readonly SystemSettings DefaultSystemSettings = new SystemSettings(string.Empty);
        private static readonly string DefaultConnectionString = DefaultSystemSettings.ToConnectionString();

        // Static Methods
        public static string ToConnectionString(Dictionary<string, string> settings)
        {
            List<CategorizedSetting> categorizedSettings = settings
                .Select(kvp => new CategorizedSetting(kvp.Key, kvp.Value))
                .ToList();

            string categorizedConnectionString = ToConnectionString(categorizedSettings, 0);

            return Merge(categorizedConnectionString, DefaultConnectionString);
        }

        private static string ToConnectionString(List<CategorizedSetting> settings, int level)
        {
            IEnumerable<string> connectionStrings;

            if (settings.Count == 1)
            {
                if (level < settings[0].Categories.Count)
                    return string.Format("{0}={{{1}}}", settings[0].Categories[level], ToConnectionString(settings, level + 1));

                return settings[0].Value;
            }

            connectionStrings = settings
                .Where(setting => level < setting.Categories.Count)
                .GroupBy(setting => setting.Categories[level])
                .Select(grouping => string.Format("{0}={{{1}}}", grouping.Key, ToConnectionString(grouping.ToList(), level + 1)));

            return string.Join(";", connectionStrings);
        }

        private static string Merge(string primaryConnectionString, string connectionString)
        {
            Dictionary<string, string> primarySettings = primaryConnectionString.ParseKeyValuePairs();
            Dictionary<string, string> settings = connectionString.ParseKeyValuePairs();

            foreach (KeyValuePair<string, string> kvp in primarySettings)
                settings[kvp.Key] = kvp.Value;

            return settings.JoinKeyValuePairs();
        }

        #endregion
    }
}
