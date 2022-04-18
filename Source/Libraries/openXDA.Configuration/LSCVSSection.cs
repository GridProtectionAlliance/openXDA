//******************************************************************************************************
//  LSCVSSection.cs - Gbtc
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
//  04/11/2022 - G. Santos
//       Generated original version of source code.
//
//******************************************************************************************************

using System.ComponentModel;
using System.Configuration;

namespace openXDA.Configuration
{
    public class LSCVSSection
    {
        public const string CategoryName = "LSCVS";

        /// <summary>
        /// URL string of the LSCVS report server name.
        /// </summary>
        [Setting]
        [DefaultValue("http://localhost/LSCVSReport")]
        public string URL { get; set; }

        /// <summary>
        /// Boolean to decide wether or not to use code authentication over username/password.
        /// </summary>
        [Setting]
        [DefaultValue(false)]
        public bool UseCodeAuth { get; set; }

        /// <summary>
        /// Login username to LSCVS Event Database.
        /// </summary>
        [Setting]
        [DefaultValue("")]
        public string Username { get; set; }

        /// <summary>
        /// Login hashed password to LSCVS Event Database.
        /// </summary>
        [Setting]
        [DefaultValue("")]
        public string Password { get; set; }

        /// <summary>
        /// Login code to LSCVS Event Databse.
        /// </summary>
        [Setting]
        [DefaultValue("")]
        public string AuthCode { get; set; }

        /// <summary>
        /// MW ratio threshold to report LSCVS event.
        /// </summary>
        [Setting]
        [DefaultValue(0.6D)]
        public double ReportingThreshold { get; set; }

        /// <summary>
        /// Ratio threshold for determining type III sag faults over type II or I.
        /// </summary>
        [Setting]
        [DefaultValue(0.95D)]
        public double TypeThreshold { get; set; }
    }
}
