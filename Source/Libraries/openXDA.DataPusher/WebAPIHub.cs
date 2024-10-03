//******************************************************************************************************
//  WebAPIHub.cs - Gbtc
//
//  Copyright © 2017, Grid Protection Alliance.  All Rights Reserved.
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
//  07/05/2017 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using GSF.Configuration;
using GSF.Security.Model;
using Newtonsoft.Json.Linq;
using openXDA.Model;

namespace openXDA.DataPusher
{
    public class WebAPIHub
    {
        public static string CompanyName
        {
            get
            {
                ConfigurationFile xdaConfig = ConfigurationFile.Open("openXDA.exe.config");
                CategorizedSettingsElementCollection systemSettings = xdaConfig.Settings["systemSettings"];
                CategorizedSettingsElement companyAcronymSetting = systemSettings["CompanyAcronym"];
                return companyAcronymSetting.Value;
            }
        }

        #region [ Get ]

        public static T GetRecordIDWhere<T>(string whereClause, DataPusherRequester requester) where T : class
        {
            Task<T> task = GetRecordIDWhereAsync<T>(whereClause, requester);
            return task.GetAwaiter().GetResult();
        }

        public static async Task<T> GetRecordIDWhereAsync<T>(string whereClause, DataPusherRequester requester) where T : class
        {
            string path = BuildPath<T>($"api/PQMark/GetRecordIDWhere", whereClause);

            using (HttpResponseMessage response = await requester.SendRequestAsync(path, HttpMethod.Get))
            {
                if (!response.IsSuccessStatusCode)
                    throw new InvalidOperationException($"Server returned status code {response.StatusCode}: {response.ReasonPhrase}");

                return await response.Content.ReadAsAsync<T>();
            }
        }

        public static IEnumerable<T> GetRecordIDsWhere<T>(string whereClause, DataPusherRequester requester) where T : class
        {
            Task<IEnumerable<T>> task = GetRecordIDsWhereAsync<T>(whereClause, requester);
            return task.GetAwaiter().GetResult();
        }

        public static async Task<IEnumerable<T>> GetRecordIDsWhereAsync<T>(string whereClause, DataPusherRequester requester) where T : class
        {
            string path = BuildPath<T>("api/PQMark/GetRecordIDsWhere", whereClause);

            using (HttpResponseMessage response = await requester.SendRequestAsync(path, HttpMethod.Get))
            {
                if (!response.IsSuccessStatusCode)
                    throw new InvalidOperationException($"Server returned status code {response.StatusCode}: {response.ReasonPhrase}");

                return await response.Content.ReadAsAsync<IEnumerable<T>>();
            }
        } 

        public static T GetRecord<T>(int id, DataPusherRequester requester) where T : class
        {
            Task<T> task = GetRecordAsync<T>(id, requester);
            return task.GetAwaiter().GetResult();
        }

        public static async Task<T> GetRecordAsync<T>(int id, DataPusherRequester requester) where T : class
        {
            string basepath = BuildPath<T>("api/PQMark/GetRecord");
            string path = $"{basepath}/{id}";

            using (HttpResponseMessage response = await requester.SendRequestAsync(path, HttpMethod.Get))
            {
                if (!response.IsSuccessStatusCode)
                    throw new InvalidOperationException($"Server returned status code {response.StatusCode}: {response.ReasonPhrase}");

                return await response.Content.ReadAsAsync<T>();
            }
        }

        public static IEnumerable<T> GetRecords<T>(string ids, DataPusherRequester requester) where T : class
        {
            Task<IEnumerable<T>> task = GetRecordsAsync<T>(ids, requester);
            return task.GetAwaiter().GetResult();
        }

        public static async Task<IEnumerable<T>> GetRecordsAsync<T>(string ids, DataPusherRequester requester) where T : class
        {
            string basepath = BuildPath<T>("api/PQMark/GetRecords");
            string path = $"{basepath}/{ids}";

            using (HttpResponseMessage response = await requester.SendRequestAsync(path, HttpMethod.Get))
            {
                if (!response.IsSuccessStatusCode)
                    throw new InvalidOperationException($"Server returned status code {response.StatusCode}: {response.ReasonPhrase}");

                return await response.Content.ReadAsAsync<IEnumerable<T>>();
            }
        }

        public static IEnumerable<T> GetRecordsWhere<T>(string whereClause, DataPusherRequester requester) where T : class
        {
            Task<IEnumerable<T>> task = GetRecordsWhereAsync<T>(whereClause, requester);
            return task.GetAwaiter().GetResult();
        }

        public static async Task<IEnumerable<T>> GetRecordsWhereAsync<T>(string whereClause, DataPusherRequester requester) where T : class
        {
            string path = BuildPath<T>("api/PQMark/GetRecordsWhere", whereClause);

            using (HttpResponseMessage response = await requester.SendRequestAsync(path, HttpMethod.Get))
            {
                if (!response.IsSuccessStatusCode)
                    throw new InvalidOperationException($"Server returned status code {response.StatusCode}: {response.ReasonPhrase}");

                return await response.Content.ReadAsAsync<IEnumerable<T>>();
            }
        }

        public static T GetRecordWhere<T>(string whereClause, DataPusherRequester requester) where T : class
        {
            Task<T> task = GetRecordWhereAsync<T>(whereClause, requester);
            return task.GetAwaiter().GetResult();
        }

        public static async Task<T> GetRecordWhereAsync<T>(string whereClause, DataPusherRequester requester) where T : class
        {
            string path = BuildPath<T>("api/PQMark/GetRecordWhere", whereClause);

            using (HttpResponseMessage response = await requester.SendRequestAsync(path, HttpMethod.Get))
            {
                if (!response.IsSuccessStatusCode)
                    throw new InvalidOperationException($"Server returned status code {response.StatusCode}: {response.ReasonPhrase}");

                return await response.Content.ReadAsAsync<T>();
            }
        }

        public static IEnumerable<ChannelDetail> GetChannels(string ids, DataPusherRequester requester)
        {
            Task<IEnumerable<ChannelDetail>> task = GetChannelsAsync(ids, requester);
            return task.GetAwaiter().GetResult();
        }

        public static async Task<IEnumerable<ChannelDetail>> GetChannelsAsync(string ids, DataPusherRequester requester)
        {
            using (HttpResponseMessage response = await requester.SendRequestAsync($"api/PQMark/GetChannels/channel/{ids}", HttpMethod.Get))
            {
                if (!response.IsSuccessStatusCode)
                    throw new InvalidOperationException($"Server returned status code {response.StatusCode}: {response.ReasonPhrase}");

                return await response.Content.ReadAsAsync<IEnumerable<ChannelDetail>>();
            }
        }
        public static async Task<(bool, Exception)> TestConnection(DataPusherRequester requester)
        {
            try
            {
                using (HttpResponseMessage response = await requester.SendRequestAsync("api/PQMark/Alive", HttpMethod.Get))
                {
                    return (response.IsSuccessStatusCode, null);
                }
            }
            catch (Exception ex)//Exception here is a fail state, must be caught reported back so that testers know the failpoint is downstream
            {
                return (false, ex);
            }
        }

        #endregion

        #region [ Create ]

        public static int CreateRecord<T>(T record, DataPusherRequester requester) where T : class
        {
            Task<int> task = CreateRecordAsync(record, requester);
            return task.GetAwaiter().GetResult();
        }

        public static async Task<int> CreateRecordAsync<T>(T record, DataPusherRequester requester) where T : class
        {
            JObject jObject = JObject.FromObject(record);
            string path = BuildPath<T>("api/PQMark/CreateRecord");

            using (HttpResponseMessage response = await requester.SendRequestAsync(path, HttpMethod.Post, jObject))
            {
                if (!response.IsSuccessStatusCode)
                    throw new InvalidOperationException($"Server returned status code {response.StatusCode}: {response.ReasonPhrase}");

                dynamic r = response.Content.ReadAsAsync<dynamic>();
                return (int)r.Result;
            }
        }

        public static int CreateChannel(JObject record, DataPusherRequester requester)
        {
            Task<int> task = CreateChannelAsync(record, requester);
            return task.GetAwaiter().GetResult();
        }

        public static async Task<int> CreateChannelAsync(JObject record, DataPusherRequester requester)
        {
            using (HttpResponseMessage response = await requester.SendRequestAsync("api/PQMark/CreateChannel", HttpMethod.Post, record))
            {
                if (!response.IsSuccessStatusCode)
                    throw new InvalidOperationException($"Server returned status code {response.StatusCode}: {response.ReasonPhrase}");

                dynamic r = response.Content.ReadAsAsync<dynamic>();
                return (int)r.Result;
            }
        }

        #endregion

        #region [ Update ]

        public static HttpResponseMessage UpdateRecord<T>(T record, DataPusherRequester requester) where T : class
        {
            Task<HttpResponseMessage> task = UpdateRecordAsync(record, requester);
            return task.GetAwaiter().GetResult();
        }

        public static async Task<HttpResponseMessage> UpdateRecordAsync<T>(T record, DataPusherRequester requester) where T : class
        {
            string path = BuildPath<T>("api/PQMark/UpdateRecord");
            JObject jObject = JObject.FromObject(record);

            HttpResponseMessage response = await requester.SendRequestAsync(path, HttpMethod.Put, jObject);
            if (!response.IsSuccessStatusCode)
            {
                string message = $"Server returned status code {response.StatusCode}: {response.ReasonPhrase}";
                response.Dispose();
                throw new InvalidOperationException(message);
            }
            return response;
        }

        public static HttpResponseMessage UpdateChannel(JObject record, DataPusherRequester requester)
        {
            Task<HttpResponseMessage> task = UpdateChannelAsync(record, requester);
            return task.GetAwaiter().GetResult();
        }

        public static async Task<HttpResponseMessage> UpdateChannelAsync(JObject record, DataPusherRequester requester)
        {
            HttpResponseMessage response = await requester.SendRequestAsync("api/PQMark/UpdateChannel", HttpMethod.Post, record);
            if (!response.IsSuccessStatusCode)
            {
                string message = $"Server returned status code {response.StatusCode}: {response.ReasonPhrase}";
                response.Dispose();
                throw new InvalidOperationException(message);
            }
            return response;
        }

        #endregion

        #region [ Delete ]

        public static HttpResponseMessage DeleteRecord(string tableName, int id, DataPusherRequester requester)
        {
            Task<HttpResponseMessage> task = DeleteRecordAsync(tableName, id, requester);
            return task.GetAwaiter().GetResult();
        }

        public static async Task<HttpResponseMessage> DeleteRecordAsync(string tableName, int id, DataPusherRequester requester)
        {
            HttpResponseMessage response = await requester.SendRequestAsync($"api/PQMark/DeleteRecord/{tableName}/{id}", HttpMethod.Delete);
            if (!response.IsSuccessStatusCode)
            {
                string message = $"Server returned status code {response.StatusCode}: {response.ReasonPhrase}";
                response.Dispose();
                throw new InvalidOperationException(message);
            }
            return response;
        }

        #endregion

        #region [ Files ]

        public static HttpResponseMessage AppendToFileBlob(JObject record, DataPusherRequester requester)
        {
            Task<HttpResponseMessage> task = AppendToFileBlobAsync(record, requester);
            return task.GetAwaiter().GetResult();
        }

        public static async Task<HttpResponseMessage> AppendToFileBlobAsync(JObject record, DataPusherRequester requester)
        {
            HttpResponseMessage response = await requester.SendRequestAsync("api/PQMark/AppendToFileBlob", HttpMethod.Put, record);
            if (!response.IsSuccessStatusCode)
            {
                string message = $"Server returned status code {response.StatusCode}: {response.ReasonPhrase}";
                response.Dispose();
                throw new InvalidOperationException(message);
            }
            return response;
        }

        public static HttpResponseMessage ProcessFileGroup(JObject record, DataPusherRequester requester)
        {
            Task<HttpResponseMessage> task = ProcessFileGroupAsync(record, requester);
            return task.GetAwaiter().GetResult();
        }

        public static async Task<HttpResponseMessage> ProcessFileGroupAsync(JObject record, DataPusherRequester requester)
        {
            HttpResponseMessage response = await requester.SendRequestAsync("api/PQMark/ProcessFileGroup", HttpMethod.Post, record);
            if (!response.IsSuccessStatusCode)
            {
                string message = $"Server returned status code {response.StatusCode}: {response.ReasonPhrase}";
                response.Dispose();
                throw new InvalidOperationException(message);
            }
            return response;
        }

        #endregion

        #region [ UserAccount Helpers ]

        private static string BuildPath<T>(string basePath, string whereClause) where T : class
        {
            string baseURL = BuildPath<T>(basePath);
            string urlEncodedWhereClause = HttpUtility.UrlEncode(whereClause);
            return $"{baseURL}?where={urlEncodedWhereClause}";
        }

        private static string BuildPath<T>(string basePath) where T : class
        {
            string tableName = typeof(T).Name;
            return $"{basePath}/{tableName}";
        }

        #endregion

    }
}