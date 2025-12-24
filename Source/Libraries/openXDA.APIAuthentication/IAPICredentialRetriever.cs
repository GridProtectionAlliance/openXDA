//******************************************************************************************************
//  XDAAPIHelper.cs - Gbtc
//
//  Copyright © 2023, Grid Protection Alliance.  All Rights Reserved.
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
//  10/17/2025 - Gabriel Santos
//       Generated original version of source code.
//
//******************************************************************************************************

using System.Security.Claims;

namespace openXDA.APIAuthentication
{
    /// <summary>
    /// Interface of credential retriever object for <see cref="XDAAPI"/>.
    /// </summary>
    public interface IAPICredentialRetriever
    {
        #region [ Properties ]

        /// <summary>
        /// API Token used to access OpenXDA
        /// </summary>
        string Token { get; }

        /// <summary>
        /// API Key used to access OpenXDA
        /// </summary>
        string Key { get; }

        /// <summary>
        /// API Key used to access OpenXDA
        /// </summary>
        string Host { get; }

        /// <summary>
        /// Refreshes the settings from the original source.
        /// </summary>
        /// <returns> A flag indicating if the operation was successful. </returns>
        bool TryRefreshSettings();

        /// <summary>
        /// Retrieves <see href="https://github.com/GridProtectionAlliance/openXDA/blob/master/Source/Libraries/openXDA.Model/SystemCenter/Customer.cs">customer</see> ID from a claims principle.
        /// </summary>
        /// <remarks>A value of -1 signifies the current user is authorized to view any object the controller may retrieve.</remarks>
        /// <returns> A flag indicating if the operation was successful. </returns>
        bool TryRetrieveCustomer(ClaimsPrincipal principal, out int CustomerID);

        #endregion
    }
}
