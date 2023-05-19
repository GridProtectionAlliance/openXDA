//******************************************************************************************************
//  Company.cs - Gbtc
//
//  Copyright © 2023, Grid Protection Alliance.  All Rights Reserved.
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
//  05/19/2023 - C. Lackner
//       Generated original version of source code.
//
//******************************************************************************************************

using System.Diagnostics.Contracts;
using System;
using System.IO;
using System.Xml.Linq;

namespace openXDA.PQI
{
    /// <summary>
    /// Summary of the <see cref="Company"/>.
    /// </summary>
    public class Company
    {
        /// <summary>
        /// Path to query this company
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Path to query the addresses for this company
        /// </summary>
        public string Addresses { get; set; }

        /// <summary>
        /// Type of the company (EPRI, UTILITY, CONTRACTOR)
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Name of the company
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Name of the company
        /// </summary>
        public string Industry { get; set; }
    }
}
