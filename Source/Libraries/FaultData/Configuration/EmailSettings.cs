﻿//******************************************************************************************************
//  EmailSettings.cs - Gbtc
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

using System.ComponentModel;
using System.Configuration;
using System.Security;
using GSF;

namespace FaultData.Configuration
{
    public class EmailSettings
    {
        /// <summary>
        /// Gets or sets the address used by the administrator of the email system.
        /// </summary>
        [Setting]
        [DefaultValue("xda-admin@gridprotectionalliance.org")]
        public string AdminAddress { get; set; }

        /// <summary>
        /// Gets or sets the address used in the To line when blind-copying recipients.
        /// </summary>
        [Setting]
        [DefaultValue("")]
        public string BlindCopyAddress { get; set; }

        /// <summary>
        /// Gets or sets the hostname or IP address of the SMTP server to
        /// use for sending automated email notifications when faults occur.
        /// </summary>
        [Setting]
        [DefaultValue("")]
        public string SMTPServer { get; set; }

        /// <summary>
        /// Gets or sets the email address used when sending automated email notifications.
        /// </summary>
        [Setting]
        [DefaultValue("openXDA@gridprotectionalliance.org")]
        public string FromAddress { get; set; }

        /// <summary>
        /// Gets or sets the username used to authenticate to the SMTP server.
        /// </summary>
        [Setting]
        [DefaultValue("")]
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the password used to authenticate to the SMTP server.
        /// </summary>
        [Setting]
        [DefaultValue(null)]
        public string Password
        {
            get => SecurePassword.ToUnsecureString();
            set => SecurePassword = value.ToSecureString();
        }

        /// <summary>
        /// Gets or sets the flag that determines whether to enable
        /// SSL when establishing communications with the SMTP server.
        /// </summary>
        [Setting]
        [DefaultValue(false)]
        public bool EnableSSL { get; set; }

        /// <summary>
        /// Gets the password as a <see cref="SecureString"/>.
        /// </summary>
        public SecureString SecurePassword { get; private set; }
    }
}
