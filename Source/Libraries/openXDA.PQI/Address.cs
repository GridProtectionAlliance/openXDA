//******************************************************************************************************
//  Address.cs - Gbtc
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

using System.IO;

namespace openXDA.PQI
{
    /// <summary>
    /// Summary of the <see cref="Address"/>.
    /// </summary>
    public class Address
    {
        /// <summary>
        /// Path to query this address
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        ///  Path to query the company at this address
        /// </summary>
        public string Company { get; set; }
       
        /// <summary>
        /// Path to query facilities at this address
        /// </summary>
        public string Facilities { get; set; }
        
        /// <summary>
        /// First line of the address (street address)
        /// </summary>
        public string AddressLine1 { get; set; }
        
         /// <summary>
         /// Second line of the address (building number, etc.)
         /// </summary>
        public string AddressLine2 { get; set; }
        
        /// <summary>
        /// City in which the address is located
        /// </summary>
        public string City { get; set; }
        
        /// <summary>
        /// State or province in which the address is located
        /// </summary>
        public string StateOrProvince { get; set; }

        /// <summary>
        /// Zip code/postal code
        /// </summary>
        public string PostalCode { get; set; }

        /// <summary>
        /// Country in which the address is located
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// True/false to indicate whether the address is primary
        /// </summary>
        public bool Primary { get; set; }
    }
}
