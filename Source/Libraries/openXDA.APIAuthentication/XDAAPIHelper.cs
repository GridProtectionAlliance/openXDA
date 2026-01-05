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
//  03/15/2024 - Gabriel Santos
//       Generated original version of source code.
//
//******************************************************************************************************

using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace openXDA.APIAuthentication
{
    /// <summary>
    /// Wrapper class that turns <see cref="XDAAPI"/> into a static for easy use across an application.
    /// </summary>
    /// <remarks>
    /// Must be initialized with the <see cref="InitializeHelper"/> method to fetch settings.
    /// </remarks>
    public static class XDAAPIHelper
    {
        #region [ Properties ]
        /// <summary>
        /// Tells users if the helper has been intialized or not.
        /// </summary>
        public static bool IsIntialized { 
            get 
            {
                return !(API is null);
            } 
        }

        /// <summary>
        /// API Token used to access OpenXDA
        /// </summary>
        public static string Token => API.Token;

        /// <summary>
        /// API Key used to access OpenXDA
        /// </summary>
        public static string Key => API.Key;

        /// <summary>
        /// API Key used to access OpenXDA
        /// </summary>
        public static string Host => API.Host;

        /// <summary>
        /// Object that retireves the token, key, and host of the helper.
        /// </summary>
        private static XDAAPI API { get; set; }

        #endregion

        #region [ Methods ]
        /// <summary>
        /// Function for setup of static helper. This must be ran once before using the helper.
        /// </summary>
        public static void InitializeHelper(IAPICredentialRetriever retriever)
        {
            API = new XDAAPI(retriever);
        }

        /// <summary>
        /// Recalls the setup settings function from <see cref="InitializeHelper"/> to refetch settings.
        /// </summary>
        public static bool TryRefreshSettings() => API.TryRefreshSettings();

        /// <summary>
        /// Retrieves <see href="https://github.com/GridProtectionAlliance/openXDA/blob/master/Source/Libraries/openXDA.Model/SystemCenter/Customer.cs">customer</see> key from the claims principle.
        /// </summary>
        /// <remarks>A value of <see langword="null"/> signifies the current user is authorized to view any object the controller may retrieve.</remarks>
        /// <returns> A flag indicating if the operation was successful. </returns>
        public static bool TryRetrieveCustomer(ClaimsPrincipal principal, out string customerKey) => API.TryRetrieveCustomer(principal, out customerKey);

        /// <summary>
        /// Gets Response Task from XDA 
        /// </summary>
        /// <param name="requestURI">Path to specific API request</param>
        /// <param name="content"> The <see cref="HttpContent"/> of the request </param>
        /// <returns> response as a <see cref="Stream"/></returns>
        public static Task<HttpResponseMessage> GetResponseTask(string requestURI, HttpContent content = null) => API.GetResponseTask(requestURI, content);

        /// <summary>
        /// Makes a Get Request to OpenXDA to obtain a string
        /// </summary>
        /// <param name="requestURI">Path to specific API request</param>
        /// <returns>string</returns>
        public static async Task<string> GetAsync(string requestURI) => await API.GetAsync(requestURI).ConfigureAwait(false);

        /// <summary>
        /// Makes a Get Request to OpenXDA to obtain a stream
        /// </summary>
        /// <param name="requestURI">Path to specific API request</param>
        /// <returns>stream</returns>
        public static async Task<Stream> GetStreamAsync(string requestURI) => await API.GetStreamAsync(requestURI).ConfigureAwait(false);

        /// <summary>
        /// Makes a Get Request to OpenXDA to obtain an object
        /// </summary>
        /// <param name="requestURI">Path to specific API request</param>
        /// <returns> a <see cref="Task{T}"/> object</returns>
        public static async Task<T> GetAsync<T>(string requestURI) => await API.GetAsync<T>(requestURI).ConfigureAwait(false);

        /// <summary>
        /// Makes Post request on OpenXDA
        /// </summary>
        /// <param name="requestURI">Path to specific API request</param>
        /// <param name="content"> The <see cref="HttpContent"/> of the request </param>
        /// <returns> response as a <see cref="string"/></returns>
        public static async Task<string> PostAsync(string requestURI, HttpContent content) => await API.PostAsync(requestURI, content).ConfigureAwait(false);

        /// <summary>
        /// Makes Post request on OpenXDA
        /// </summary>
        /// <param name="endpoint">Path to specific API request</param>
        /// <param name="content"> The <see cref="HttpContent"/> of the request </param>
        /// <returns> response as a <see cref="string"/></returns>
        public static async Task<string> PostAllAsync<T>(string endpoint, HttpContent content) => await API.PostAllAsync<T>(endpoint, content).ConfigureAwait(false);

        /// <summary>
        /// Makes a Get Request to OpenXDA to obtain a <see cref="List{T}"/> objects
        /// </summary>
        /// <param name="requestURI">Path to specific API request</param>
        /// <returns> a <see cref="List{T}"/> object</returns>
        public static async Task<List<T>> GetAllAsync<T>(string requestURI) => await API.GetAllAsync<T>(requestURI).ConfigureAwait(false);

        #endregion

    }
}
