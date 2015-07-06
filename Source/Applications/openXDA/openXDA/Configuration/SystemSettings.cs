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
using System.Reflection;
using GSF;
using GSF.Configuration;

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
        private double m_maxFileCreationTimeOffset;

        private double m_maxVoltage;
        private double m_maxCurrent;
        private double m_prefaultTrigger;
        private double m_maxFaultDistanceMultiplier;
        private double m_minFaultDistanceMultiplier;

        private double m_openBreakerThreshold;
        private double m_lateBreakerThreshold;

        private string m_lengthUnits;
        private double m_comtradeMinWaitTime;
        private int m_processingThreadCount;
        private string m_fileShares;

        private string m_smtpServer;
        private string m_fromAddress;
        private string m_emailTemplate;

        private string m_pqDashboardUrl;
        private string m_structureQueryUrl;

        private string m_historianServer;
        private string m_historianInstanceName;

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
        /// Gets or sets the amount of time, in seconds,
        /// between the time a file is received and the time
        /// an email should be sent out by the system.
        /// </summary>
        [Setting]
        [DefaultValue(10.0D)]
        public double WaitPeriod
        {
            get
            {
                return m_waitPeriod;
            }
            set
            {
                m_waitPeriod = value;
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
        [DefaultValue(24.0D)]
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
        [DefaultValue(1440.0D)]
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
        /// Gets or sets the threshold at which the ratio between RMS
        /// current and prefault RMS current indicates faulted conditions.
        /// </summary>
        [Setting]
        [DefaultValue(5.0D)]
        public double PrefaultTrigger
        {
            get
            {
                return m_prefaultTrigger;
            }
            set
            {
                m_prefaultTrigger = value;
            }
        }

        /// <summary>
        /// Gets or sets the multiplier applied to the line length to determine
        /// the maximum value allowed for fault distance in the COMTRADE export.
        /// </summary>
        [Setting]
        [DefaultValue(1.05D)]
        public double MaxFaultDistanceMultiplier
        {
            get
            {
                return m_maxFaultDistanceMultiplier;
            }
            set
            {
                m_maxFaultDistanceMultiplier = value;
            }
        }

        /// <summary>
        /// Gets or sets the multiplier applied to the line length to determine
        /// the minimum value allowed for fault distance in the COMTRADE export.
        /// </summary>
        [Setting]
        [DefaultValue(-0.05D)]
        public double MinFaultDistanceMultiplier
        {
            get
            {
                return m_minFaultDistanceMultiplier;
            }
            set
            {
                m_minFaultDistanceMultiplier = value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum RMS current, in amps,
        /// at which the breaker can be considered open.
        /// </summary>
        [Setting]
        [DefaultValue(50.0D)]
        public double OpenBreakerThreshold
        {
            get
            {
                return m_openBreakerThreshold;
            }
            set
            {
                m_openBreakerThreshold = value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum number of cycles that a breaker
        /// operation's timing can exceed the configured breaker speed.
        /// </summary>
        [Setting]
        [DefaultValue(1.0D)]
        public double LateBreakerThreshold
        {
            get
            {
                return m_lateBreakerThreshold;
            }
            set
            {
                m_lateBreakerThreshold = value;
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

                if ((object)value != null)
                {
                    m_fileShareList = value.ParseKeyValuePairs()
                        .Select(kvp => kvp.Value)
                        .Select(fileShareString => new FileShare(fileShareString))
                        .ToList();
                }
            }
        }

        /// <summary>
        /// Gets or sets the hostname or IP address of the SMTP server to
        /// use for sending automated email notifications when faults occur.
        /// </summary>
        [Setting]
        [DefaultValue("")]
        public string SMTPServer
        {
            get
            {
                return m_smtpServer;
            }
            set
            {
                m_smtpServer = value;
            }
        }

        /// <summary>
        /// Gets or sets the email address used when sending automated email notifications.
        /// </summary>
        [Setting]
        [DefaultValue("openXDA@gridprotectionalliance.org")]
        public string FromAddress
        {
            get
            {
                return m_fromAddress;
            }
            set
            {
                m_fromAddress = value;
            }
        }

        /// <summary>
        /// Gets or sets the template used for sending emails to email recipients.
        /// </summary>
        [Setting]
        [DefaultValue(null)]
        public string EmailTemplate
        {
            get
            {
                return m_emailTemplate;
            }
            set
            {
                m_emailTemplate = value ?? GetDefaultEmailTemplate();
            }
        }

        /// <summary>
        /// Gets or sets the URL of the PQ Dashboard.
        /// </summary>
        [Setting]
        [DefaultValue("http://pqdashboard/")]
        public string PQDashboardURL
        {
            get
            {
                return m_pqDashboardUrl;
            }
            set
            {
                m_pqDashboardUrl = value;
            }
        }

        /// <summary>
        /// Gets or sets the URL of the web request to
        /// find the nearest structure to fault location.
        /// </summary>
        [Setting]
        [DefaultValue("http://localhost:6132/WebForm1.aspx?Station={0}&Line={1}&Mileage={2}")]
        public string StructureQueryURL
        {
            get
            {
                return m_structureQueryUrl;
            }
            set
            {
                m_structureQueryUrl = value;
            }
        }

        /// <summary>
        /// Gets or sets historian server, e.g., 127.0.0.1:38402
        /// </summary>
        [Setting]
        [DefaultValue("127.0.0.1")]
        public string HistorianServer
        {
            get
            {
                return m_historianServer;
            }
            set
            {
                m_historianServer = value;
            }
        }

        /// <summary>
        /// Gets or sets historian instance name.
        /// </summary>
        [Setting]
        [DefaultValue("XDA")]
        public string HistorianInstanceName
        {
            get
            {
                return m_historianInstanceName;
            }
            set
            {
                m_historianInstanceName = value;
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

        private string GetDefaultEmailTemplate()
        {
            IEnumerable<Assembly> assemblies = Assembly.GetExecutingAssembly().GetReferencedAssemblies()
                .Where(name => name.FullName.Contains("FaultData"))
                .Select(Assembly.Load);

            foreach (Assembly assembly in assemblies)
            {
                using (Stream resource = assembly.GetManifestResourceStream("FaultData.DataWriters.EmailTemplate.html"))
                {
                    if ((object)resource != null)
                    {
                        using (StreamReader reader = new StreamReader(resource))
                        {
                            return reader.ReadToEnd();
                        }
                    }
                }
            }

            return null;
        }

        #endregion
    }
}
