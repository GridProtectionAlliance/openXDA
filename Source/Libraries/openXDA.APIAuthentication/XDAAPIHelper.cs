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
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

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
        public Task<HttpResponseMessage> GetResponseTask(string requestURI, HttpContent content = null)
        {
            APIQuery query = new APIQuery(Key, Token, Host.Split(';'));

            void ConfigureRequest(HttpRequestMessage request)
            {
                if (content == null)
                {
                    request.Method = HttpMethod.Get;
                    request.Headers.Accept.Clear();
                    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                }
                else
                {
                    request.Method = HttpMethod.Post;
                    request.Content = content;
                }
            }

            return query.SendWebRequestAsync(ConfigureRequest, requestURI);
        }

        /// <summary>
        /// Makes a Get Request to OpenXDA to obtain a string
        /// </summary>
        /// <param name="requestURI">Path to specific API request</param>
        /// <returns>string</returns>
        public async Task<string> Get(string requestURI)
        {
            APIQuery query = new APIQuery(Key, Token, Host.Split(';'));

            void ConfigureRequest(HttpRequestMessage request)
            {
                request.Method = HttpMethod.Get;
                request.Headers.Accept.Clear();
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }

            HttpResponseMessage responseMessage = await query.SendWebRequestAsync(ConfigureRequest, requestURI).ConfigureAwait(false);
            if (!responseMessage.IsSuccessStatusCode)
                throw new Exception(responseMessage.ReasonPhrase);
            return await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Makes a Get Request to OpenXDA to obtain a stream
        /// </summary>
        /// <param name="requestURI">Path to specific API request</param>
        /// <returns>stream</returns>
        public async Task<Stream> GetStream(string requestURI)
        {
            APIQuery query = new APIQuery(Key, Token, Host.Split(';'));

            void ConfigureRequest(HttpRequestMessage request)
            {
                request.Method = HttpMethod.Get;
                request.Headers.Accept.Clear();
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }

            HttpResponseMessage responseMessage = await query.SendWebRequestAsync(ConfigureRequest, requestURI).ConfigureAwait(false);
            if (!responseMessage.IsSuccessStatusCode)
                throw new Exception(responseMessage.ReasonPhrase);
            return await responseMessage.Content.ReadAsStreamAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Makes a Get Request to OpenXDA to obtain an object
        /// </summary>
        /// <param name="requestURI">Path to specific API request</param>
        /// <returns> a <see cref="Task{T}"/> object</returns>
        public async Task<T> Get<T>(string requestURI)
        {
            string result = await Get(requestURI).ConfigureAwait(false);
            T resultObject = JsonConvert.DeserializeObject<T>(result);
            return resultObject;
        }

        /// <summary>
        /// Makes Post request on OpenXDA
        /// </summary>
        /// <param name="requestURI">Path to specific API request</param>
        /// <param name="content"> The <see cref="HttpContent"/> of the request </param>
        /// <returns> response as a <see cref="string"/></returns>
        public async Task<string> Post(string requestURI, HttpContent content)
        {
            APIQuery query = new APIQuery(Key, Token, Host.Split(';'));

            void ConfigureRequest(HttpRequestMessage request)
            {
                request.Method = HttpMethod.Post;
                request.Content = content;
            }

            HttpResponseMessage responseMessage = await query.SendWebRequestAsync(ConfigureRequest, requestURI).ConfigureAwait(false);
            return await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Makes Post request on OpenXDA
        /// </summary>
        /// <param name="endpoint">Path to specific API request</param>
        /// <param name="content"> The <see cref="HttpContent"/> of the request </param>
        /// <returns> response as a <see cref="string"/></returns>
        public async Task<string> PostAll<T>(string endpoint, HttpContent content) => await Post(endpoint, content).ConfigureAwait(false);

        /// <summary>
        /// Makes a Get Request to OpenXDA to obtain a <see cref="List{T}"/> objects
        /// </summary>
        /// <param name="requestURI">Path to specific API request</param>
        /// <returns> a <see cref="List{T}"/> object</returns>
        public async Task<List<T>> GetAll<T>(string requestURI) => await Get<List<T>>(requestURI).ConfigureAwait(false);

        #endregion

    }
}
