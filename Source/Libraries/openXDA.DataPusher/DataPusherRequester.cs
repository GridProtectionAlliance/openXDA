//******************************************************************************************************
//  DataPusherRequester.cs - Gbtc
//
//  Copyright © 2024, Grid Protection Alliance.  All Rights Reserved.
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
//  09/11/2024 - Gabriel Santos
//       Generated original version of source code.
//
//******************************************************************************************************

using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using GSF.Data;
using GSF.Data.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using openXDA.APIAuthentication;
using openXDA.Model;

namespace openXDA.DataPusher
{
    public class DataPusherRequester
    {
        private string m_token = null;
        private string m_key = null;
        private string m_baseUrl = null;

        #region [ Constructor ]

        public DataPusherRequester(string instance, string token, string key)
        {
            m_baseUrl = instance.TrimEnd('/');
            m_token = token;
            m_key = key;
        }

        public DataPusherRequester(int instanceID, AdoDataConnection connection) : this(new TableOperations<RemoteXDAInstance>(connection).QueryRecordWhere("ID = {0}", instanceID)) { }

        public DataPusherRequester(RemoteXDAInstance instance)
        {
            m_baseUrl = instance.Address.TrimEnd('/');
            m_token = instance.APIToken;
            m_key = instance.RegistrationKey;
        }

        #endregion

        #region [ Methods ]

        public async Task<HttpResponseMessage> SendRequestAsync(string path, HttpMethod method, JObject json)
        {
            HttpContent content = new StringContent(json.ToString(Formatting.None), null, "application/json");
            return await SendRequestAsync(path, method, content);
        }

        public async Task<HttpResponseMessage> SendRequestAsync(string path, HttpMethod method, HttpContent content = null, string mediaResponseType = "application/json")
        {
            APIQuery query = new APIQuery(m_key, m_token, m_baseUrl.Split(';'));

            void ConfigureRequest(HttpRequestMessage request)
            {
                if (!(content is null))
                    request.Content = content;
                request.Method = method;
                request.Headers.Accept.Clear();
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaResponseType));
            }

            return await query.SendWebRequestAsync(ConfigureRequest, path);
        }

        #endregion
    }
}