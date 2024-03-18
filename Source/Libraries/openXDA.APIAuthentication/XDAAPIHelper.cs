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

using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace openXDA.APIAuthentication
{
    /// <summary>
    /// Helper class that provides openXDA API Calls
    /// </summary>
    public abstract class XDAAPIHelper
    {

        #region [Abstract]
        /// <summary>
        /// API Token used to access OpenXDA
        /// </summary>
        protected abstract string Token { get; }

        /// <summary>
        /// API Key used to access OpenXDA
        /// </summary>
        protected abstract string Key { get; }

        /// <summary>
        /// API Key used to access OpenXDA
        /// </summary>
        protected abstract string Host { get; }
        #endregion

        #region [Methods]
        /// <summary>
        /// Gets Response Task from XDA 
        /// </summary>
        /// <param name="requestURI">Path to specific API request</param>
        /// <param name="content"> The <see cref="HttpContent"/> of the request </param>
        /// <returns> response as a <see cref="Stream"/></returns>
        public Task<HttpResponseMessage> GetResponseTask(string requestURI, HttpContent content)
        {
            APIQuery query = new APIQuery(Key, Token, Host.Split(';'));

            void ConfigureRequest(HttpRequestMessage request)
            {
                request.Method = HttpMethod.Post;
                request.Content = content;
            }

            return query.SendWebRequestAsync(ConfigureRequest, requestURI);
        }

        /// <summary>
        /// Makes Post request on OpenXDA
        /// </summary>
        /// <param name="requestURI">Path to specific API request</param>
        /// <param name="content"> The <see cref="HttpContent"/> of the request </param>
        /// <returns> response as a <see cref="Stream"/></returns>
        public async Task<Stream> Post(string requestURI, HttpContent content)
        {
            HttpResponseMessage responseMessage = await GetResponseTask(requestURI, content).ConfigureAwait(false);
            return await responseMessage.Content.ReadAsStreamAsync().ConfigureAwait(false);
        }
        #endregion

    }
}
