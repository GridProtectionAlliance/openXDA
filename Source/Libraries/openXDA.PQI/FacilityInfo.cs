//******************************************************************************************************
//  FacilityInfo.cs - Gbtc
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
//  07/22/2021 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

namespace openXDA.PQI
{
    public class FacilityInfo
    {
        /// <summary>
        /// Name of the facility
        /// </summary>
        public string FacilityName { get; set; }

        /// <summary>
        /// Voltage at the facility
        /// </summary>
        public string FacilityVoltage { get; set; }

        /// <summary>
        /// Utility voltage supplied to the facility
        /// </summary>
        public string UtilitySupplyVoltage { get; set; }

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
        /// Country in which the address is located.
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Name of the company
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// Industry the company is in
        /// </summary>
        public string Industry { get; set; }
    }
}
