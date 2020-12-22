//******************************************************************************************************
//  TimeZoneConverter.cs - Gbtc
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
using GSF.Configuration;
using openXDA.Configuration;

namespace openXDA.Nodes
{
    public class TimeZoneConverter
    {
        #region [ Members ]

        // Nested Types
        private class Settings
        {
            public Settings(Action<object> configure) =>
                configure(this);

            [Category]
            [SettingName(SystemSection.CategoryName)]
            public SystemSection SystemSettings { get; } = new SystemSection();
        }

        #endregion

        #region [ Constructors ]

        public TimeZoneConverter(Action<object> configurator) =>
            SettingsObject = new Settings(configurator);

        #endregion

        #region [ Properties ]

        private Settings SettingsObject { get; }

        private TimeZoneInfo XDATimeZoneInfo =>
            SettingsObject.SystemSettings.XDATimeZoneInfo;

        #endregion

        #region [ Methods ]

        public Func<DateTime, DateTime> GetMeterTimeZoneConverter(TimeZoneInfo meterTimeZoneInfo)
        {
            DateTime ToUTC(DateTime dateTime)
            {
                switch (dateTime.Kind)
                {
                    case DateTimeKind.Unspecified: return TimeZoneInfo.ConvertTimeToUtc(dateTime, meterTimeZoneInfo);
                    case DateTimeKind.Local: return TimeZoneInfo.ConvertTimeToUtc(dateTime);
                    default: return dateTime;
                }
            }

            return meterTime =>
            {
                DateTime utc = ToUTC(meterTime);
                return ToXDATimeZone(utc);
            };
        }

        public DateTime ToXDATimeZone(DateTime utc) =>
            TimeZoneInfo.ConvertTimeFromUtc(utc, XDATimeZoneInfo);

        #endregion
    }
}
