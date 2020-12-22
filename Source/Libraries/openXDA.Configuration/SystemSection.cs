//******************************************************************************************************
//  SystemSection.cs - Gbtc
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

namespace openXDA.Configuration
{
    /// <summary>
    /// Represents the system settings for openXDA.
    /// </summary>
    public class SystemSection
    {
        public const string CategoryName = "System";

        /// <summary>
        /// Gets or sets the time zone identifier for the time zone
        /// used by meters in the system unless configured otherwise.
        /// </summary>
        [Setting]
        [DefaultValue("UTC")]
        public string DefaultMeterTimeZone
        {
            get => DefaultMeterTimeZoneInfo.Id;
            set => DefaultMeterTimeZoneInfo = string.IsNullOrEmpty(value)
                ? TimeZoneInfo.Local
                : TimeZoneInfo.FindSystemTimeZoneById(value);
        }

        /// <summary>
        /// Gets the <see cref="TimeZoneInfo"/> used by meters
        /// in the system unless configured otherwise.
        /// </summary>
        public TimeZoneInfo DefaultMeterTimeZoneInfo { get; private set; }

        /// <summary>
        /// Gets or sets the amount of time each database
        /// query is given to complete, in seconds.
        /// </summary>
        [Setting]
        [DefaultValue(120)]
        public int DbTimeout { get; set; }

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
                return _XDATimeZone;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    _XDATimeZone = TimeZoneInfo.Local.Id;
                else
                    _XDATimeZone = value;
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
                if ((object)_XDATimeZoneInfo == null)
                    _XDATimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(_XDATimeZone);

                return _XDATimeZoneInfo;
            }
        }

        private string _XDATimeZone { get; set; }
        private TimeZoneInfo _XDATimeZoneInfo { get; set; }
    }
}
