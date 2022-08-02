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
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using GSF;
using GSF.Configuration;
using GSF.Net.Security;
using GSF.Security.Model;
using GSF.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using openXDA.Model;

namespace openXDA.DataPusher
{
    public class WebAPIHub
    {
        #region [ Client-side script functionality ]

        public static event EventHandler<EventArgs<string>> LogStatusMessageEvent;

        public static event EventHandler<EventArgs<string>> LogExceptionMessageEvent;
        
        private static readonly HttpClient staticClient = new HttpClient();

        private static void OnLogStatusMessage(string message)
        {
            LogStatusMessageEvent?.Invoke(new object(), new EventArgs<string>(message));
        }

        private static void OnLogExceptionMessage(string message)
        {
            LogExceptionMessageEvent?.Invoke(new object(), new EventArgs<string>(message));
        }

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

        #endregion

        #region [ Get ]

        public T GetRecordIDWhereHub<T>(string instance, string whereClause, UserAccount userAccount) where T : class =>
            GetRecordIDWhere<T>(instance, whereClause, userAccount);

        public static T GetRecordIDWhere<T>(string instance, string whereClause, UserAccount userAccount) where T : class
        {
            Task<T> task = GetRecordIDWhereAsync<T>(instance, whereClause, userAccount);
            return task.GetAwaiter().GetResult();
        }

        public static async Task<T> GetRecordIDWhereAsync<T>(string instance, string whereClause, UserAccount userAccount) where T : class
        {
            string url = BuildURL<T>(instance, $"api/PQMark/GetRecordIDWhere", whereClause);

            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url))
            {
                ConfigureRequest(request, userAccount);

                using (HttpResponseMessage response = await HttpClient.SendAsync(request))
                {
                    if (!response.IsSuccessStatusCode)
                        throw new InvalidOperationException($"Server returned status code {response.StatusCode}: {response.ReasonPhrase}");

                    return await response.Content.ReadAsAsync<T>();
                }
            }
        }



        public IEnumerable<T> GetRecordIDsWhereHub<T>(string instance, string whereClause, UserAccount userAccount) where T : class =>
            GetRecordIDsWhere<T>(instance, whereClause, userAccount);

        public static IEnumerable<T> GetRecordIDsWhere<T>(string instance, string whereClause, UserAccount userAccount) where T : class
        {
            Task<IEnumerable<T>> task = GetRecordIDsWhereAsync<T>(instance, whereClause, userAccount);
            return task.GetAwaiter().GetResult();
        }

        public static async Task<IEnumerable<T>> GetRecordIDsWhereAsync<T>(string instance, string whereClause, UserAccount userAccount) where T : class
        {
            string url = BuildURL<T>(instance, "api/PQMark/GetRecordIDsWhere", whereClause);

            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url))
            {
                ConfigureRequest(request, userAccount);

                using (HttpResponseMessage response = await HttpClient.SendAsync(request))
                {
                    if (!response.IsSuccessStatusCode)
                        throw new InvalidOperationException($"Server returned status code {response.StatusCode}: {response.ReasonPhrase}");

                    return await response.Content.ReadAsAsync<IEnumerable<T>>();
                }
            }
        }



        public T GetRecordHub<T>(string instance, int id, UserAccount userAccount) where T : class =>
            GetRecord<T>(instance, id, userAccount);

        public static T GetRecord<T>(string instance, int id, UserAccount userAccount) where T : class
        {
            Task<T> task = GetRecordAsync<T>(instance, id, userAccount);
            return task.GetAwaiter().GetResult();
        }

        public static async Task<T> GetRecordAsync<T>(string instance, int id, UserAccount userAccount) where T : class
        {
            string baseURL = BuildURL<T>(instance, "api/PQMark/GetRecord");
            string url = $"{baseURL}/{id}";

            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url))
            {
                ConfigureRequest(request, userAccount);

                using (HttpResponseMessage response = await HttpClient.SendAsync(request))
                {
                    if (!response.IsSuccessStatusCode)
                        throw new InvalidOperationException($"Server returned status code {response.StatusCode}: {response.ReasonPhrase}");

                    return await response.Content.ReadAsAsync<T>();
                }
            }
        }



        public IEnumerable<T> GetRecordsHub<T>(string instance, string ids, UserAccount userAccount) where T : class =>
            GetRecords<T>(instance, ids, userAccount);

        public static IEnumerable<T> GetRecords<T>(string instance, string ids, UserAccount userAccount) where T : class
        {
            Task<IEnumerable<T>> task = GetRecordsAsync<T>(instance, ids, userAccount);
            return task.GetAwaiter().GetResult();
        }

        public static async Task<IEnumerable<T>> GetRecordsAsync<T>(string instance, string ids, UserAccount userAccount) where T : class
        {
            string baseURL = BuildURL<T>(instance, "api/PQMark/GetRecords");
            string url = $"{baseURL}/{ids}";

            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url))
            {
                ConfigureRequest(request, userAccount);

                using (HttpResponseMessage response = await HttpClient.SendAsync(request))
                {
                    if (!response.IsSuccessStatusCode)
                        throw new InvalidOperationException($"Server returned status code {response.StatusCode}: {response.ReasonPhrase}");

                    return await response.Content.ReadAsAsync<IEnumerable<T>>();
                }
            }
        }



        public IEnumerable<T> GetRecordsWhereHub<T>(string instance, string tableName, string whereClause, UserAccount userAccount) where T : class =>
            GetRecordsWhere<T>(instance, whereClause, userAccount);

        public static IEnumerable<T> GetRecordsWhere<T>(string instance, string whereClause, UserAccount userAccount) where T : class
        {
            Task<IEnumerable<T>> task = GetRecordsWhereAsync<T>(instance, whereClause, userAccount);
            return task.GetAwaiter().GetResult();
        }

        public static async Task<IEnumerable<T>> GetRecordsWhereAsync<T>(string instance, string whereClause, UserAccount userAccount) where T : class
        {
            string url = BuildURL<T>(instance, "api/PQMark/GetRecordsWhere", whereClause);

            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url))
            {
                ConfigureRequest(request, userAccount);

                using (HttpResponseMessage response = await HttpClient.SendAsync(request))
                {
                    if (!response.IsSuccessStatusCode)
                        throw new InvalidOperationException($"Server returned status code {response.StatusCode}: {response.ReasonPhrase}");

                    return await response.Content.ReadAsAsync<IEnumerable<T>>();
                }
            }
        }



        public T GetRecordWhereHub<T>(string instance, string whereClause, UserAccount userAccount) where T : class =>
            GetRecordWhere<T>(instance, whereClause, userAccount);

        public static T GetRecordWhere<T>(string instance, string whereClause, UserAccount userAccount) where T : class
        {
            Task<T> task = GetRecordWhereAsync<T>(instance, whereClause, userAccount);
            return task.GetAwaiter().GetResult();
        }

        public static async Task<T> GetRecordWhereAsync<T>(string instance, string whereClause, UserAccount userAccount) where T : class
        {
            string url = BuildURL<T>(instance, "api/PQMark/GetRecordWhere", whereClause);

            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url))
            {
                ConfigureRequest(request, userAccount);

                using (HttpResponseMessage response = await HttpClient.SendAsync(request))
                {
                    if (!response.IsSuccessStatusCode)
                        throw new InvalidOperationException($"Server returned status code {response.StatusCode}: {response.ReasonPhrase}");

                    return await response.Content.ReadAsAsync<T>();
                }
            }
        }



        public dynamic GetChannelsHub(string instance, string ids, UserAccount userAccount) =>
            GetChannels(instance, ids, userAccount);

        public static IEnumerable<ChannelDetail> GetChannels(string instance, string ids, UserAccount userAccount)
        {
            Task<IEnumerable<ChannelDetail>> task = GetChannelsAsync(instance, ids, userAccount);
            return task.GetAwaiter().GetResult();
        }

        public static async Task<IEnumerable<ChannelDetail>> GetChannelsAsync(string instance, string ids, UserAccount userAccount)
        {
            string baseURL = BuildURL(instance, "api/PQMark/GetChannels/channel");
            string url = $"{baseURL}/{ids}";

            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url))
            {
                ConfigureRequest(request, userAccount);

                using (HttpResponseMessage response = await HttpClient.SendAsync(request))
                {
                    if (!response.IsSuccessStatusCode)
                        throw new InvalidOperationException($"Server returned status code {response.StatusCode}: {response.ReasonPhrase}");

                    return await response.Content.ReadAsAsync<IEnumerable<ChannelDetail>>();
                }
            }
        }

        private static bool HandleCertificateValidation(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            SimpleCertificateChecker simpleCertificateChecker = new SimpleCertificateChecker();

            CategorizedSettingsElementCollection systemSettings = ConfigurationFile.Current.Settings["systemSettings"];
            systemSettings.Add("CertFile", "", "This is a certfile.");
            systemSettings.Add("ValidPolicyErrors", "None", "Password for PQMarkWeb API access.");
            systemSettings.Add("ValidChainFlags", "NoError", "Password for PQMarkWeb API access.");

            try
            {
                simpleCertificateChecker.ValidPolicyErrors = (SslPolicyErrors)Enum.Parse(typeof(SslPolicyErrors), (systemSettings["ValidPolicyErrors"].Value != "All" ? systemSettings["ValidPolicyErrors"].Value : "7"));
                simpleCertificateChecker.ValidChainFlags = (X509ChainStatusFlags)Enum.Parse(typeof(X509ChainStatusFlags), (systemSettings["ValidChainFlags"].Value != "All" ? systemSettings["ValidChainFlags"].Value : (~0).ToString()));
                simpleCertificateChecker.TrustedCertificates.Add((!string.IsNullOrEmpty(systemSettings["CertFile"].Value) ? new X509Certificate2(systemSettings["CertFile"].Value) : certificate));
            }
            catch (Exception ex)
            {
                OnLogExceptionMessage(ex.ToString());
            }

            return simpleCertificateChecker.ValidateRemoteCertificate(sender, certificate, chain, sslPolicyErrors);
        }
        public static async Task<bool> TestConnection(string instance, UserAccount userAccount)
        {
            string url = BuildURL(instance, "api/PQMark/Alive");

            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url))
            {
                ConfigureRequest(request, userAccount);
                try
                {
                    using (HttpResponseMessage response = await HttpClient.SendAsync(request))
                    {
                        return response.IsSuccessStatusCode;
                    }
                }
                catch (Exception ex) //Exception here is a fail state, must be caught reported back so that testers know the failpoint is downstream
                {
                    return false;
                }
            }
        }

        static WebAPIHub()
        {
            WebRequestHandler = new WebRequestHandler();
            WebRequestHandler.ServerCertificateValidationCallback = HandleCertificateValidation;
            HttpClient = new HttpClient(WebRequestHandler);
        }

        private static WebRequestHandler WebRequestHandler { get; }
        private static HttpClient HttpClient { get; }

        #endregion

        #region [ Create ]

        public int CreateRecordHub<T>(string instance, T record, UserAccount userAccount) where T : class =>
            CreateRecord(instance, record, userAccount);

        public static int CreateRecord<T>(string instance, T record, UserAccount userAccount) where T : class
        {
            Task<int> task = CreateRecordAsync(instance, record, userAccount);
            return task.GetAwaiter().GetResult();
        }

        public static async Task<int> CreateRecordAsync<T>(string instance, T record, UserAccount userAccount) where T : class
        {
            string url = BuildURL<T>(instance, "api/PQMark/CreateRecord");
            string antiForgeryToken = await GenerateAntiForgeryTokenAsync(instance, userAccount);

            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url))
            {
                ConfigureRequest(request, userAccount);
                request.Headers.Add("X-GSF-Verify", antiForgeryToken);

                JObject jObject = JObject.FromObject(record);
                string json = jObject.ToString(Formatting.None);
                request.Content = new StringContent(json, null, "application/json");

                using (HttpResponseMessage response = await HttpClient.SendAsync(request))
                {
                    if (!response.IsSuccessStatusCode)
                        throw new InvalidOperationException($"Server returned status code {response.StatusCode}: {response.ReasonPhrase}");

                    dynamic r = response.Content.ReadAsAsync<dynamic>();
                    return (int)r.Result;
                }
            }
        }



        public int CreateChannelHub(string instance, JObject record, UserAccount userAccount) =>
            CreateChannel(instance, record, userAccount);

        public static int CreateChannel(string instance, JObject record, UserAccount userAccount)
        {
            Task<int> task = CreateChannelAsync(instance, record, userAccount);
            return task.GetAwaiter().GetResult();
        }

        public static async Task<int> CreateChannelAsync(string instance, JObject record, UserAccount userAccount)
        {
            string url = BuildURL(instance, "api/PQMark/CreateChannel");
            string antiForgeryToken = await GenerateAntiForgeryTokenAsync(instance, userAccount);

            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url))
            {
                ConfigureRequest(request, userAccount);
                request.Headers.Add("X-GSF-Verify", antiForgeryToken);

                string json = record.ToString(Formatting.None);
                request.Content = new StringContent(json, null, "application/json");

                using (HttpResponseMessage response = await HttpClient.SendAsync(request))
                {
                    if (!response.IsSuccessStatusCode)
                        throw new InvalidOperationException($"Server returned status code {response.StatusCode}: {response.ReasonPhrase}");

                    dynamic r = response.Content.ReadAsAsync<dynamic>();
                    return (int)r.Result;
                }
            }
        }

        #endregion

        #region [ Update ]

        public HttpResponseMessage UpdateRecordHub<T>(string instance, T record, UserAccount userAccount) where T : class =>
            UpdateRecord(instance, record, userAccount);

        public static HttpResponseMessage UpdateRecord<T>(string instance, T record, UserAccount userAccount) where T : class
        {
            Task<HttpResponseMessage> task = UpdateRecordAsync(instance, record, userAccount);
            return task.GetAwaiter().GetResult();
        }

        public static async Task<HttpResponseMessage> UpdateRecordAsync<T>(string instance, T record, UserAccount userAccount) where T : class
        {
            string url = BuildURL<T>(instance, "api/PQMark/UpdateRecord");
            string antiForgeryToken = await GenerateAntiForgeryTokenAsync(instance, userAccount);

            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, url))
            {
                ConfigureRequest(request, userAccount);
                request.Headers.Add("X-GSF-Verify", antiForgeryToken);

                JObject jObject = JObject.FromObject(record);
                string json = jObject.ToString(Formatting.None);
                request.Content = new StringContent(json, null, "application/json");

                HttpResponseMessage response = await HttpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    string message = $"Server returned status code {response.StatusCode}: {response.ReasonPhrase}";
                    response.Dispose();
                    throw new InvalidOperationException(message);
                }

                return response;
            }
        }



        public HttpResponseMessage UpdateChannelHub(string instance, JObject record, UserAccount userAccount) =>
            UpdateChannel(instance, record, userAccount);

        public static HttpResponseMessage UpdateChannel(string instance, JObject record, UserAccount userAccount)
        {
            Task<HttpResponseMessage> task = UpdateChannelAsync(instance, record, userAccount);
            return task.GetAwaiter().GetResult();
        }

        public static async Task<HttpResponseMessage> UpdateChannelAsync(string instance, JObject record, UserAccount userAccount)
        {
            string url = BuildURL(instance, "api/PQMark/UpdateChannel");
            string antiForgeryToken = await GenerateAntiForgeryTokenAsync(instance, userAccount);

            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url))
            {
                ConfigureRequest(request, userAccount);
                request.Headers.Add("X-GSF-Verify", antiForgeryToken);

                string json = record.ToString(Formatting.None);
                request.Content = new StringContent(json, null, "application/json");

                HttpResponseMessage response = await HttpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    string message = $"Server returned status code {response.StatusCode}: {response.ReasonPhrase}";
                    response.Dispose();
                    throw new InvalidOperationException(message);
                }

                return response;
            }
        }

        #endregion

        #region [ Delete ]

        public HttpResponseMessage DeleteRecordHub(string instance, string tableName, int id, UserAccount userAccount)
        {
            return DeleteRecord(instance, tableName, id, userAccount);
        }

        public static HttpResponseMessage DeleteRecord(string instance, string tableName, int id, UserAccount userAccount)
        {
            Task<HttpResponseMessage> task = DeleteRecordAsync(instance, tableName, id, userAccount);
            return task.GetAwaiter().GetResult();
        }

        public static async Task<HttpResponseMessage> DeleteRecordAsync(string instance, string tableName, int id, UserAccount userAccount)
        {
            string url = BuildURL(instance, $"api/PQMark/DeleteRecord/{tableName}/{id}");
            string antiForgeryToken = await GenerateAntiForgeryTokenAsync(instance, userAccount);

            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, url))
            {
                ConfigureRequest(request, userAccount);
                request.Headers.Add("X-GSF-Verify", antiForgeryToken);

                HttpResponseMessage response = await HttpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    string message = $"Server returned status code {response.StatusCode}: {response.ReasonPhrase}";
                    response.Dispose();
                    throw new InvalidOperationException(message);
                }

                return response;
            }
        }

        #endregion

        #region [ Files ]

        public HttpResponseMessage AppendToFileBlobHub(string instance, JObject record, UserAccount userAccount)
        {
            return AppendToFileBlob(instance, record, userAccount);
        }

        public static HttpResponseMessage AppendToFileBlob(string instance, JObject record, UserAccount userAccount)
        {
            Task<HttpResponseMessage> task = AppendToFileBlobAsync(instance, record, userAccount);
            return task.GetAwaiter().GetResult();
        }

        public static async Task<HttpResponseMessage> AppendToFileBlobAsync(string instance, JObject record, UserAccount userAccount)
        {
            string url = BuildURL(instance, "api/PQMark/AppendToFileBlob");
            string antiForgeryToken = await GenerateAntiForgeryTokenAsync(instance, userAccount);

            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, url))
            {
                ConfigureRequest(request, userAccount);
                request.Headers.Add("X-GSF-Verify", antiForgeryToken);

                string json = record.ToString(Formatting.None);
                request.Content = new StringContent(json, null, "application/json");

                HttpResponseMessage response = await HttpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    string message = $"Server returned status code {response.StatusCode}: {response.ReasonPhrase}";
                    response.Dispose();
                    throw new InvalidOperationException(message);
                }

                return response;
            }
        }



        public HttpResponseMessage ProcessFileGroupHub(string instance, JObject record, UserAccount userAccount)
        {
            return ProcessFileGroup(instance, record, userAccount);
        }

        public static HttpResponseMessage ProcessFileGroup(string instance, JObject record, UserAccount userAccount)
        {
            Task<HttpResponseMessage> task = ProcessFileGroupAsync(instance, record, userAccount);
            return task.GetAwaiter().GetResult();
        }

        public static async Task<HttpResponseMessage> ProcessFileGroupAsync(string instance, JObject record, UserAccount userAccount)
        {
            string url = BuildURL(instance, "api/PQMark/ProcessFileGroup");
            string antiForgeryToken = await GenerateAntiForgeryTokenAsync(instance, userAccount);

            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url))
            {
                ConfigureRequest(request, userAccount);
                request.Headers.Add("X-GSF-Verify", antiForgeryToken);

                string json = record.ToString(Formatting.None);
                request.Content = new StringContent(json, null, "application/json");

                HttpResponseMessage response = await HttpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    string message = $"Server returned status code {response.StatusCode}: {response.ReasonPhrase}";
                    response.Dispose();
                    throw new InvalidOperationException(message);
                }

                return response;
            }
        }

        #endregion

        #region [ Helpers ]

        private static string BuildURL<T>(string baseAddress, string basePath, string whereClause) where T : class
        {
            string baseURL = BuildURL<T>(baseAddress, basePath);
            string urlEncodedWhereClause = HttpUtility.UrlEncode(whereClause);
            return $"{baseURL}?where={urlEncodedWhereClause}";
        }

        private static string BuildURL<T>(string baseAddress, string basePath) where T : class
        {
            string tableName = typeof(T).Name;
            return BuildURL(baseAddress, $"{basePath}/{tableName}");
        }

        private static string BuildURL(string baseAddress, string path)
        {
            string trimmedAddress = baseAddress.TrimEnd('/');
            return $"{trimmedAddress}/{path}";
        }

        private static void ConfigureRequest(HttpRequestMessage request, UserAccount userAccount)
        {
            MediaTypeWithQualityHeaderValue jsonMediaType = new MediaTypeWithQualityHeaderValue("application/json");
            request.Headers.Accept.Add(jsonMediaType);

            Encoding utf8 = new UTF8Encoding(false);
            string credential = $"{userAccount.AccountName}:{userAccount.Password}";
            byte[] accountData = utf8.GetBytes(credential);
            string encodedCredential = Convert.ToBase64String(accountData);
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", encodedCredential);
        }

        private static async Task<string> GenerateAntiForgeryTokenAsync(string instance, UserAccount userAccount)
        {
            string url = BuildURL(instance, "api/PQMark/GenerateRequestVerficationToken");

            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url))
            {
                ConfigureRequest(request, userAccount);

                using (HttpResponseMessage response = await HttpClient.SendAsync(request))
                {
                    if (!response.IsSuccessStatusCode)
                        throw new InvalidOperationException($"Server returned status code {response.StatusCode}: {response.ReasonPhrase}");

                    return await response.Content.ReadAsStringAsync();
                }
            }
        }

        #endregion
    }
}