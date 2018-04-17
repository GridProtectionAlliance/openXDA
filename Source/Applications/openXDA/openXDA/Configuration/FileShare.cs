//******************************************************************************************************
//  FileShare.cs - Gbtc
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
using System.ComponentModel;
using System.Configuration;
using System.Security;
using GSF;
using GSF.Configuration;
using GSF.IO;

namespace openXDA.Configuration
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
