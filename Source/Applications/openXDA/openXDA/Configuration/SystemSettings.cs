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

        // Fields
        private string m_dbConnectionString;
        private int m_dbTimeout;

        private string m_watchDirectories;
        private string m_resultsPath;
        private string m_filePattern;

        private double m_waitPeriod;
        private double m_timeTolerance;
        private string m_defaultMeterTimeZone;
        private string m_xdaTimeZone;
        private double m_maxTimeOffset;
        private double m_minTimeOffset;
        private double m_maxFileDuration;
        private double m_maxFileCreationTimeOffset;

        private double m_systemFrequency;
        private double m_maxVoltage;
        private double m_maxCurrent;
        private double m_sagThreshold;
        private double m_swellThreshold;
        private double m_interruptionThreshold;

        private string m_lengthUnits;
        private double m_comtradeMinWaitTime;
        private int m_processingThreadCount;
        private FileEnumerationStrategy m_fileWatcherEnumerationStrategy;
        private int m_maxQueuedFileCount;
        private int m_fileWatcherMaxFragmentation;
        private int m_fileWatcherInternalThreadCount;
        private int m_fileWatcherBufferSize;
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
        public string DbConnectionString
        {
            get
            {
                return m_dbConnectionString;
            }
            set
            {
                m_dbConnectionString = value;
            }
        }

        /// <summary>
        /// Gets or sets the amount of time each database
        /// query is given to complete, in seconds.
        /// </summary>
        [Setting]
        [DefaultValue(120)]
        public int DbTimeout
        {
            get
            {
                return m_dbTimeout;
            }
            set
            {
                m_dbTimeout = value;
            }
        }

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
        /// Gets or sets the path to the fault location
        /// results generated by the fault location engine.
        /// </summary>
        [Setting]
        [DefaultValue("Results")]
        public string ResultsPath
        {
            get
            {
                return m_resultsPath;
            }
            set
            {
                m_resultsPath = value;
            }
        }

        /// <summary>
        /// Gets or sets the pattern used to parse file paths in
        /// order to identify the meter that the file came from.
        /// </summary>
        [Setting]
        [DefaultValue(@"(?<AssetKey>[^\\]+)\\[^\\]+$")]
        public string FilePattern
        {
            get
            {
                return m_filePattern;
            }
            set
            {
                m_filePattern = value;
            }
        }

        /// <summary>
        /// Gets or sets the time zone identifier for the time zone
        /// used by meters in the system unless configured otherwise.
        /// </summary>
        [Setting]
        [DefaultValue("UTC")]
        public string DefaultMeterTimeZone
        {
            get
            {
                return m_defaultMeterTimeZone;
            }
            set
            {
                m_defaultMeterTimeZone = value;
            }
        }

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
        [DefaultValue("")]
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
        public double TimeTolerance
        {
            get
            {
                return m_timeTolerance;
            }
            set
            {
                m_timeTolerance = value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum number of hours beyond the current system time
        /// before the time of the record indicates that the data is unreasonable.
        /// </summary>
        [Setting]
        [DefaultValue(0.0D)]
        public double MaxTimeOffset
        {
            get
            {
                return m_maxTimeOffset;
            }
            set
            {
                m_maxTimeOffset = value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum number of hours prior to the current system time
        /// before the time of the record indicates that the data is unreasonable.
        /// </summary>
        [Setting]
        [DefaultValue(0.0D)]
        public double MinTimeOffset
        {
            get
            {
                return m_minTimeOffset;
            }
            set
            {
                m_minTimeOffset = value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum duration, in seconds,
        /// of the files processed by openXDA.
        /// </summary>
        [Setting]
        [DefaultValue(0.0D)]
        public double MaxFileDuration
        {
            get
            {
                return m_maxFileDuration;
            }
            set
            {
                m_maxFileDuration = value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum number of hours prior to the current system time
        /// before the file creation time indicates that the data should not be processed.
        /// </summary>
        [Setting]
        [DefaultValue(0.0D)]
        public double MaxFileCreationTimeOffset
        {
            get
            {
                return m_maxFileCreationTimeOffset;
            }
            set
            {
                m_maxFileCreationTimeOffset = value;
            }
        }

        /// <summary>
        /// Gets or sets the system frequency.
        /// </summary>
        [Setting]
        [DefaultValue(60.0D)]
        public double SystemFrequency
        {
            get
            {
                return m_systemFrequency;
            }
            set
            {
                m_systemFrequency = value;
            }
        }

        /// <summary>
        /// Gets or sets the per-unit threshold at which the
        /// voltage exceeds engineering reasonableness.
        /// </summary>
        [Setting]
        [DefaultValue(2.0D)]
        public double MaxVoltage
        {
            get
            {
                return m_maxVoltage;
            }
            set
            {
                m_maxVoltage = value;
            }
        }

        /// <summary>
        /// Gets or sets the threshold, in amps, at which the
        /// current exceeds engineering reasonableness.
        /// </summary>
        [Setting]
        [DefaultValue(1000000.0D)]
        public double MaxCurrent
        {
            get
            {
                return m_maxCurrent;
            }
            set
            {
                m_maxCurrent = value;
            }
        }

        /// <summary>
        /// Gets or sets the threshold, in amps, at which the
        /// current exceeds engineering reasonableness.
        /// </summary>
        [Setting]
        [DefaultValue(0.9D)]
        public double SagThreshold
        {
            get
            {
                return m_sagThreshold;
            }
            set
            {
                m_sagThreshold = value;
            }
        }

        /// <summary>
        /// Gets or sets the threshold, in amps, at which the
        /// current exceeds engineering reasonableness.
        /// </summary>
        [Setting]
        [DefaultValue(1.1D)]
        public double SwellThreshold
        {
            get
            {
                return m_swellThreshold;
            }
            set
            {
                m_swellThreshold = value;
            }
        }

        /// <summary>
        /// Gets or sets the threshold, in amps, at which the
        /// current exceeds engineering reasonableness.
        /// </summary>
        [Setting]
        [DefaultValue(0.1D)]
        public double InterruptionThreshold
        {
            get
            {
                return m_interruptionThreshold;
            }
            set
            {
                m_interruptionThreshold = value;
            }
        }

        /// <summary>
        /// Gets or sets the units of measure to use
        /// for lengths (line length and fault distance).
        /// </summary>
        [Setting]
        [DefaultValue("miles")]
        public string LengthUnits
        {
            get
            {
                return m_lengthUnits;
            }
            set
            {
                m_lengthUnits = value;
            }
        }

        /// <summary>
        /// Gets or sets the minimum amount of time to wait for additional data
        /// files after the system detects the existence of a .D00 COMTRADE file.
        /// </summary>
        [Setting]
        [DefaultValue(15.0D)]
        public double COMTRADEMinWaitTime
        {
            get
            {
                return m_comtradeMinWaitTime;
            }
            set
            {
                m_comtradeMinWaitTime = value;
            }
        }

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
        /// Gets or sets the number of files that can be queued
        /// on meter data processing threads before the system
        /// starts blocking the file processing thread.
        /// </summary>
        /// <remarks>
        /// Values less than or equal to zero will be set to one.
        /// </remarks>
        [Setting]
        [DefaultValue(10)]
        public int MaxQueuedFileCount
        {
            get
            {
                return m_maxQueuedFileCount;
            }
            set
            {
                m_maxQueuedFileCount = value;

                if (m_maxQueuedFileCount <= 0)
                    m_maxQueuedFileCount = 1;
            }
        }

        /// <summary>
        /// Gets or sets the strategy used for enumeration of files in the file watcher.
        /// </summary>
        [Setting]
        [DefaultValue(FileProcessor.DefaultEnumerationStrategy)]
        public FileEnumerationStrategy FileWatcherEnumerationStrategy
        {
            get
            {
                return m_fileWatcherEnumerationStrategy;
            }
            set
            {
                m_fileWatcherEnumerationStrategy = value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum amount of fragmentation allowed before
        /// compacting the list of processed files in the file watcher.
        /// </summary>
        [Setting]
        [DefaultValue(FileProcessor.DefaultMaxFragmentation)]
        public int FileWatcherMaxFragmentation
        {
            get
            {
                return m_fileWatcherMaxFragmentation;
            }
            set
            {
                m_fileWatcherMaxFragmentation = value;
            }
        }

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
        [DefaultValue(8192)]
        public int FileWatcherBufferSize
        {
            get
            {
                return m_fileWatcherBufferSize;
            }
            set
            {
                m_fileWatcherBufferSize = value;
            }
        }

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
        /// Gets the <see cref="TimeZoneInfo"/> for the time zone
        /// used by meters in the system unless configured otherwise.
        /// </summary>
        public TimeZoneInfo DefaultMeterTimeZoneInfo
        {
            get
            {
                if ((object)m_defaultMeterTimeZoneInfo == null)
                    m_defaultMeterTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(m_defaultMeterTimeZone);

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
