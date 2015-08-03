//******************************************************************************************************
//  HistorianSettings.cs - Gbtc
//
//  Copyright © 2015, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the MIT License (MIT), the "License"; you may
//  not use this file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://opensource.org/licenses/MIT
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  08/02/2015 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.ComponentModel;
using System.Configuration;
using GSF;

namespace openHistorian.XDALink
{
    public class HistorianSettings
    {
        #region [ Members ]

        // Constants

        /// <summary>
        /// Default historian server port number.
        /// </summary>
        public const int DefaultHistorianPort = 38402;

        // Fields
        private string m_hostName;
        private int m_port;
        private string m_instanceName;

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets or sets historian server, e.g., 127.0.0.1:38402
        /// </summary>
        [Setting]
        [DefaultValue("127.0.0.1")]
        public string Server
        {
            get
            {
                return string.Format("{0}:{1}", m_hostName.ToNonNullNorEmptyString("127.0.0.1"), m_port);
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentNullException("value", "historian server null or empty");

                string[] parts = value.Split(':');
                m_hostName = parts[0];

                if (parts.Length < 2 || !int.TryParse(parts[1], out m_port))
                    m_port = DefaultHistorianPort;
            }
        }

        /// <summary>
        /// Gets or sets historian instance name.
        /// </summary>
        [Setting]
        [DefaultValue("XDA")]
        public string InstanceName
        {
            get
            {
                return m_instanceName;
            }
            set
            {
                m_instanceName = value;
            }
        }

        /// <summary>
        /// Gets the host name of the historian server.
        /// </summary>
        public string HostName
        {
            get
            {
                return m_hostName;
            }
        }

        /// <summary>
        /// Gets the port on which the historian server is listening.
        /// </summary>
        public int Port
        {
            get
            {
                return m_port;
            }
        }

        #endregion
    }
}
