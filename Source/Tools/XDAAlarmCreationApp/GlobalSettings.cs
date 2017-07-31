//******************************************************************************************************
//  GlobalSettings.cs - Gbtc
//
//  Copyright © 2017, Grid Protection Alliance.  All Rights Reserved.
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
//  05/02/2017 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************

using System;

namespace XDAAlarmCreationApp
{
    public class GlobalSettings
    {
        public string WebHostURL
        {
            get;
            set;
        }

        public string WebHostPortRange
        {
            get;
            set;
        }

        public string DefaultWebPage
        {
            get;
            set;
        }

        public string CompanyName
        {
            get;
            set;
        }

        public string CompanyAcronym
        {
            get;
            set;
        }

        public string ProjectName
        {
            get;
            set;
        }

        public Guid NodeID
        {
            get;
            set;
        }

        public string ApplicationName
        {
            get;
            set;
        }

        public string ApplicationDescription
        {
            get;
            set;
        }

        public string ApplicationKeywords
        {
            get;
            set;
        }

        public string DateFormat
        {
            get;
            set;
        }

        public string TimeFormat
        {
            get;
            set;
        }

        public string DateTimeFormat
        {
            get;
            set;
        }

        public string BootstrapTheme
        {
            get;
            set;
        }

        public string SubscriptionConnectionString
        {
            get;
            set;
        }

        public string DefaultProjectPath
        {
            get;
            set;
        }
    }
}
