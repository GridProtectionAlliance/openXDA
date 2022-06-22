//******************************************************************************************************
//  SettingsHelper.cs - Gbtc
//
//  Copyright © 2022, Grid Protection Alliance.  All Rights Reserved.
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
//  06/08/2022 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System.ComponentModel;
using GSF.Configuration;
using openXDA.HIDS;
using openXDA.Nodes;

namespace SPCTools
{
    internal static class SettingsHelper
    {
        private class Settings
        {
            [Category]
            [SettingName(HIDSSettings.CategoryName)]
            public HIDSSettings HIDSSettings { get; }
                = new HIDSSettings();
        }

        public static HIDSSettings GetHIDSSettings(Host host)
        {
            ConfigurationLoader configurationLoader = new ConfigurationLoader(host.ID, host.CreateDbConnection);
            Settings settings = new Settings();
            configurationLoader.Configure(settings);
            return settings.HIDSSettings;
        }
    }
}
