//******************************************************************************************************
//  FileWatcherSection.cs - Gbtc
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
//  01/16/2021 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security;
using GSF;
using GSF.Configuration;
using GSF.IO;
using FileShare = openXDA.Configuration.FileWatcher.FileShare;

namespace openXDA.Configuration
{
    namespace FileWatcher
    {
        /// <summary>
        /// Represents a file share.
        /// </summary>
        public class FileShare
        {
            #region [ Members ]

            // Fields
            private string m_name;
            private string m_domain;
            private string m_username;
            private SecureString m_password;

            private Exception m_authenticationException;

            #endregion

            #region [ Constructors ]

            /// <summary>
            /// Creates a new instance of the <see cref="FileShare"/> class.
            /// </summary>
            public FileShare()
            {
            }

            /// <summary>
            /// Creates a new instance of the <see cref="FileShare"/> class.
            /// </summary>
            /// <param name="connectionString">A string containing the file share parameters as key-value pairs.</param>
            public FileShare(string connectionString)
            {
                ConnectionStringParser<SettingAttribute> parser = new ConnectionStringParser<SettingAttribute>();
                parser.ParseConnectionString(connectionString, this);
            }

            #endregion

            #region [ Properties ]

            /// <summary>
            /// Gets or sets the name of the file share (\\server\share).
            /// </summary>
            [Setting]
            [DefaultValue("")]
            public string Name
            {
                get
                {
                    return m_name;
                }
                set
                {
                    m_name = value;
                }
            }

            /// <summary>
            /// Gets or sets the domain of the user used to authenticate to the file share.
            /// </summary>
            [Setting]
            [DefaultValue("")]
            public string Domain
            {
                get
                {
                    return m_domain;
                }
                set
                {
                    m_domain = value;
                }
            }

            /// <summary>
            /// Gets or sets the username of the user used to authenticate to the file share.
            /// </summary>
            [Setting]
            [DefaultValue("")]
            public string Username
            {
                get
                {
                    return m_username;
                }
                set
                {
                    string[] splitValue;

                    m_username = value;

                    if ((object)value != null)
                    {
                        splitValue = value.Split('\\');

                        if (splitValue.Length == 2)
                        {
                            m_domain = splitValue[0];
                            m_username = splitValue[1];
                        }
                    }
                }
            }

            /// <summary>
            /// Gets or sets the password of the user used to authenticate to the file share.
            /// </summary>
            [Setting]
            [DefaultValue("")]
            public string Password
            {
                get
                {
                    return m_password.ToUnsecureString();
                }
                set
                {
                    m_password = value.ToSecureString();
                }
            }

            /// <summary>
            /// Gets the exception encountered during the most recent authentication attempt.
            /// </summary>
            public Exception AuthenticationException
            {
                get
                {
                    return m_authenticationException;
                }
            }

            #endregion

            #region [ Methods ]

            /// <summary>
            /// Attempts to authenticate to the file share.
            /// </summary>
            public void Authenticate()
            {
                try
                {
                    FilePath.ConnectToNetworkShare(m_name, m_username, Password, m_domain);
                    m_authenticationException = null;
                }
                catch (Exception ex)
                {
                    m_authenticationException = ex;
                    throw;
                }
            }

            /// <summary>
            /// Attempts to authenticate to the file share.
            /// </summary>
            /// <returns>True if successfully authenticated. False otherwise.</returns>
            public bool TryAuthenticate()
            {
                try
                {
                    Authenticate();
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            #endregion
        }
    }

    public class FileWatcherSection
    {
        public const string CategoryName = "FileWatcher";

        /// <summary>
        /// Gets or sets the size of the
        /// <see cref="FileSystemWatcher"/>s' internal buffers.
        /// </summary>
        /// <seealso cref="FileSystemWatcher.InternalBufferSize"/>
        [Setting]
        [DefaultValue(65536)]
        public int BufferSize { get; set; }

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
                return _FileShares;
            }
            set
            {
                _FileShares = value;

                _FileShareList = value.ToNonNullString().ParseKeyValuePairs()
                    .Select(kvp => kvp.Value)
                    .Select(fileShareString => new FileShare(fileShareString))
                    .ToList();
            }
        }

        /// <summary>
        /// Gets a list of file shares to be authenticated at startup.
        /// </summary>
        public IReadOnlyCollection<FileShare> FileShareList => _FileShareList.AsReadOnly();

        private string _FileShares { get; set; }
        private List<FileShare> _FileShareList { get; set; }

        /// <summary>
        /// Gets or sets the number of threads used
        /// internally to the file processor.
        /// </summary>
        /// <remarks>
        /// Values less than or equal to zero will be set to the number of logical processors.
        /// </remarks>
        [Setting]
        [DefaultValue(0)]
        public int InternalThreadCount
        {
            get => _InternalThreadCount;
            set => _InternalThreadCount = (value > 0)
                ? value
                : Environment.ProcessorCount;
        }

        private int _InternalThreadCount { get; set; }

        /// <summary>
        /// Gets or sets the list of directories to watch for files.
        /// </summary>
        [Setting]
        [DefaultValue("Watch")]
        public string WatchDirectories

        {
            get
            {
                return _WatchDirectories;
            }
            set
            {
                _WatchDirectories = value;

                if ((object)value != null)
                {
                    _WatchDirectoryList = value
                        .Split(Path.PathSeparator)
                        .Select(path => path.Trim())
                        .ToList();
                }
            }
        }

        /// <summary>
        /// Gets a list of directories to be watched
        /// for files containing fault records.
        /// </summary>
        public IReadOnlyCollection<string> WatchDirectoryList => _WatchDirectoryList.AsReadOnly();

        private string _WatchDirectories { get; set; }
        private List<string> _WatchDirectoryList { get; set; }
    }
}
