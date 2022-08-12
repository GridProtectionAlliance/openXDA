//******************************************************************************************************
//  SSAMSSection.cs - Gbtc
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
//  02/08/2021 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.ComponentModel;
using System.Configuration;
using GSF.Configuration;

namespace openXDA.Configuration
{
    public class SSAMSSection
    {
        public const string CategoryName = "SSAMS";

        private const string DefaultDataProviderString = 
            "AssemblyName={System.Data, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089}; " +
            "ConnectionType=System.Data.SqlClient.SqlConnection; " +
            "AdapterType=System.Data.SqlClient.SqlDataAdapter;";

        /// <summary>
        /// Cron string frequency at which the SSAMS process is scheduled.
        /// </summary>
        [Setting]
        [DefaultValue("* 0 * * *")]
        public string Schedule { get; set; }

        /// <summary>
        /// Defines the connection string of the SSAMS DB.
        /// </summary>
        [Setting]
        [DefaultValue("Data Source=localhost; Initial Catalog=openXDA; Integrated Security=SSPI")]
        public string ConnectionString { get; set; }

        /// <summary>
        /// Defines the connection string of the SSAMS DB.
        /// </summary>
        [Setting]
        [DefaultValue(DefaultDataProviderString)]
        public string DataProviderString { get; set; }

        /// <summary>
        /// Command or procedure name that defines the command executed by the external DB.
        /// </summary>
        [Setting]
        [DefaultValue("sp_LogSsamEvent")]
        public string DatabaseCommand { get; set; }

        /// <summary>
        /// Parameters for the external DB procedure.
        /// </summary>
        [Setting]
        [DefaultValue("1,1,'FL_PMU_{Acronym}_HEARTBEAT','','{Acronym} adapter heartbeat at {Timestamp} UTC',''")]
        public string CommandParameters { get; set; }
    }
}
