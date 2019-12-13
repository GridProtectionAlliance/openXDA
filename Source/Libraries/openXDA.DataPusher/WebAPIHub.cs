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
using System.Threading.Tasks;
using System.Web;
using GSF;
using GSF.Configuration;
using GSF.Net.Security;
using GSF.Security.Model;
using GSF.Web;
using Newtonsoft.Json.Linq;
using openXDA.Model;

namespace openXDA.DataPusher
{
    public class WebAPIHub
    {
        // Client-side script functionality
        #region [ Static ]

        public static event EventHandler<EventArgs<string>> LogStatusMessageEvent;

        private static void OnLogStatusMessage(string message)
        {
            LogStatusMessageEvent?.Invoke(new object(), new EventArgs<string>(message));
        }

        public static event EventHandler<EventArgs<string>> LogExceptionMessageEvent;

        private static void OnLogExceptionMessage(string message)
        {
            LogExceptionMessageEvent?.Invoke(new object(), new EventArgs<string>(message));
        }

        public static string CompanyName
        {
            get
            {
                return ConfigurationFile.Open("openXDA.exe.config").Settings["systemSettings"]["CompanyAcronym"].Value;
            }
        }


        #endregion

        #region [ RESTful API Handlers ]

        private static string GenerateAntiForgeryToken(string instance, UserAccount userAccount)
        {
            using (WebRequestHandler handler = new WebRequestHandler())
            using (HttpClient client = new HttpClient(handler))
            {
                handler.ServerCertificateValidationCallback += HandleCertificateValidation;

                client.BaseAddress = new Uri(instance);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.AddBasicAuthenticationHeader(userAccount.AccountName, userAccount.Password);

                HttpResponseMessage response = client.GetAsync("api/PQMark/GenerateRequestVerficationToken").Result;

                if (!response.IsSuccessStatusCode)
                    throw new InvalidOperationException($"Server returned status code {response.StatusCode}: {response.ReasonPhrase}");

                return response.Content.ReadAsStringAsync().Result;
            }
        }

        public T GetRecordIDWhereHub<T>(string instance, string whereClause, UserAccount userAccount) where T: class
        {
            return GetRecordIDWhere<T>(instance, whereClause, userAccount);
        }

        public static T GetRecordIDWhere<T>(string instance, string whereClause, UserAccount userAccount) where T : class
        {
            using (WebRequestHandler handler = new WebRequestHandler())
            using (HttpClient client = new HttpClient(handler))
            {
                handler.ServerCertificateValidationCallback += HandleCertificateValidation;

                client.BaseAddress = new Uri(instance);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.AddBasicAuthenticationHeader(userAccount.AccountName, userAccount.Password);
                
                string urlEncodedWhereClause = HttpUtility.UrlEncode(whereClause);
                HttpResponseMessage response = client.GetAsync($"api/PQMark/GetRecordIDWhere/{typeof(T).Name}?where={urlEncodedWhereClause}").Result;

                if (!response.IsSuccessStatusCode)
                    throw new InvalidOperationException($"Server returned status code {response.StatusCode}: {response.ReasonPhrase}");

                Task<T> record = response.Content.ReadAsAsync<T>();
                return record.Result;
            }
        }

        public IEnumerable<T> GetRecordIDsWhereHub<T>(string instance, string whereClause, UserAccount userAccount) where T : class
        {
            return GetRecordIDsWhere<T>(instance, whereClause, userAccount);
        }

        public static IEnumerable<T> GetRecordIDsWhere<T>(string instance, string whereClause, UserAccount userAccount) where T: class
        {
            using (WebRequestHandler handler = new WebRequestHandler())
            using (HttpClient client = new HttpClient(handler))
            {
                handler.ServerCertificateValidationCallback += HandleCertificateValidation;

                client.BaseAddress = new Uri(instance);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.AddBasicAuthenticationHeader(userAccount.AccountName, userAccount.Password);

                string urlEncodedWhereClause = HttpUtility.UrlEncode(whereClause);
                HttpResponseMessage response = client.GetAsync($"api/PQMark/GetRecordIDsWhere/{typeof(T).Name}?where={urlEncodedWhereClause}").Result;

                if (!response.IsSuccessStatusCode)
                    throw new InvalidOperationException($"Server returned status code {response.StatusCode}: {response.ReasonPhrase}");

                Task<IEnumerable<T>> record = response.Content.ReadAsAsync<IEnumerable<T>>();
                return record.Result;
            }
        }

        public T GetRecordHub<T>(string instance, int id, UserAccount userAccount) where T : class
        {
            return GetRecord<T>(instance, id, userAccount);
        }

        public static T GetRecord<T>(string instance, int id, UserAccount userAccount) where T : class
        {
            using (WebRequestHandler handler = new WebRequestHandler())
            using (HttpClient client = new HttpClient(handler))
            {
                handler.ServerCertificateValidationCallback += HandleCertificateValidation;

                client.BaseAddress = new Uri(instance);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.AddBasicAuthenticationHeader(userAccount.AccountName, userAccount.Password);

                HttpResponseMessage response = client.GetAsync($"api/PQMark/GetRecord/{typeof(T).Name}/{id}").Result;

                if (!response.IsSuccessStatusCode)
                    throw new InvalidOperationException($"Server returned status code {response.StatusCode}: {response.ReasonPhrase}");

                Task<T> record = response.Content.ReadAsAsync<T>();
                return record.Result;
            }
        }

        public IEnumerable<T> GetRecordsHub<T>(string instance, string ids, UserAccount userAccount) where T : class
        {
            return GetRecords<T>(instance, ids, userAccount);
        }

        public static IEnumerable<T> GetRecords<T>(string instance, string ids, UserAccount userAccount) where T : class
        {
            using (WebRequestHandler handler = new WebRequestHandler())
            using (HttpClient client = new HttpClient(handler))
            {
                handler.ServerCertificateValidationCallback += HandleCertificateValidation;

                client.BaseAddress = new Uri(instance);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.AddBasicAuthenticationHeader(userAccount.AccountName, userAccount.Password);

                HttpResponseMessage response = client.GetAsync($"api/PQMark/GetRecords/{typeof(T).Name}/{ids}").Result;

                if (!response.IsSuccessStatusCode)
                    throw new InvalidOperationException($"Server returned status code {response.StatusCode}: {response.ReasonPhrase}");

                Task<IEnumerable<T>> record = response.Content.ReadAsAsync<IEnumerable<T>>();
                return record.Result;
            }
        }

        public IEnumerable<T> GetRecordsWhereHub<T>(string instance, string tableName, string whereClause, UserAccount userAccount) where T : class
        {
            return GetRecordsWhere<T>(instance, whereClause, userAccount);
        }

        public static IEnumerable<T> GetRecordsWhere<T>(string instance, string whereClause, UserAccount userAccount) where T : class
        {
            using (WebRequestHandler handler = new WebRequestHandler())
            using (HttpClient client = new HttpClient(handler))
            {
                handler.ServerCertificateValidationCallback += HandleCertificateValidation;

                client.BaseAddress = new Uri(instance);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.AddBasicAuthenticationHeader(userAccount.AccountName, userAccount.Password);

                string urlEncodedWhereClause = HttpUtility.UrlEncode(whereClause);
                HttpResponseMessage response = client.GetAsync($"api/PQMark/GetRecordsWhere/{typeof(T).Name}?where={urlEncodedWhereClause}").Result;

                if (!response.IsSuccessStatusCode)
                    throw new InvalidOperationException($"Server returned status code {response.StatusCode}: {response.ReasonPhrase}");

                Task<IEnumerable<T>> record = response.Content.ReadAsAsync<IEnumerable<T>>();
                return record.Result;
            }
        }

        public T GetRecordWhereHub<T>(string instance, string whereClause, UserAccount userAccount) where T : class
        {
            return GetRecordWhere<T>(instance, whereClause, userAccount);
        }

        public static T GetRecordWhere<T>(string instance, string whereClause, UserAccount userAccount) where T: class
        {
            using (WebRequestHandler handler = new WebRequestHandler())
            using (HttpClient client = new HttpClient(handler))
            {
                handler.ServerCertificateValidationCallback += HandleCertificateValidation;

                client.BaseAddress = new Uri(instance);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.AddBasicAuthenticationHeader(userAccount.AccountName, userAccount.Password);

                string urlEncodedWhereClause = HttpUtility.UrlEncode(whereClause);
                HttpResponseMessage response = client.GetAsync($"api/PQMark/GetRecordWhere/{typeof(T).Name}?where={urlEncodedWhereClause}").Result;

                if (!response.IsSuccessStatusCode)
                    throw new InvalidOperationException($"Server returned status code {response.StatusCode}: {response.ReasonPhrase}");

                Task<T> record = response.Content.ReadAsAsync<T>();
                return record.Result;
            }
        }


        public dynamic GetChannelsHub(string instance, string ids, UserAccount userAccount)
        {
            return GetChannels(instance, ids, userAccount);
        }

        public static IEnumerable<ChannelDetail> GetChannels(string instance, string ids, UserAccount userAccount)
        {
            using (WebRequestHandler handler = new WebRequestHandler())
            using (HttpClient client = new HttpClient(handler))
            {
                handler.ServerCertificateValidationCallback += HandleCertificateValidation;

                client.BaseAddress = new Uri(instance);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.AddBasicAuthenticationHeader(userAccount.AccountName, userAccount.Password);
                
                HttpResponseMessage response = client.GetAsync($"api/PQMark/GetChannels/channel/{ids}").Result;

                if (!response.IsSuccessStatusCode)
                    throw new InvalidOperationException($"Server returned status code {response.StatusCode}: {response.ReasonPhrase}");

                dynamic record = response.Content.ReadAsAsync<dynamic>();
                return record.Result.ToObject(typeof(IEnumerable<ChannelDetail>));
            }
        }


        public HttpResponseMessage UpdateRecordHub<T>(string instance, T record, UserAccount userAccount) where T : class
        {
            return UpdateRecord<T>(instance, record, userAccount);
        }

        public static HttpResponseMessage UpdateRecord<T>(string instance, T record, UserAccount userAccount) where T : class
        {
            string antiForgeryToken = GenerateAntiForgeryToken(instance, userAccount);

            using (WebRequestHandler handler = new WebRequestHandler())
            using (HttpClient client = new HttpClient(handler))
            {
                handler.ServerCertificateValidationCallback += HandleCertificateValidation;

                client.BaseAddress = new Uri(instance);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("X-GSF-Verify", antiForgeryToken);
                client.AddBasicAuthenticationHeader(userAccount.AccountName, userAccount.Password);

                HttpResponseMessage response = client.PutAsJsonAsync($"api/PQMark/UpdateRecord/{typeof(T).Name}", record).Result;
                return response;
            }
        }

        public int CreateRecordHub<T>(string instance, T record, UserAccount userAccount) where T : class
        {
            return CreateRecord<T>(instance, record, userAccount);
        }

        public static int CreateRecord<T>(string instance, T record, UserAccount userAccount) where T: class
        {
            string antiForgeryToken = GenerateAntiForgeryToken(instance, userAccount);

            using (WebRequestHandler handler = new WebRequestHandler())
            using (HttpClient client = new HttpClient(handler))
            {
                handler.ServerCertificateValidationCallback += HandleCertificateValidation;

                client.BaseAddress = new Uri(instance);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("X-GSF-Verify", antiForgeryToken);
                client.AddBasicAuthenticationHeader(userAccount.AccountName, userAccount.Password);

                HttpResponseMessage response = client.PostAsJsonAsync($"api/PQMark/CreateRecord/{typeof(T).Name}", record).Result;

                if (!response.IsSuccessStatusCode)
                    throw new InvalidOperationException($"Server returned status code {response.StatusCode}: {response.ReasonPhrase}");

                dynamic r = response.Content.ReadAsAsync<dynamic>();
                return (int)r.Result;
            }
        }

        public int CreateChannelHub(string instance, JObject record, UserAccount userAccount)
        {
            return CreateChannel(instance, record, userAccount);
        }

        public static int CreateChannel(string instance, JObject record, UserAccount userAccount)
        {
            string antiForgeryToken = GenerateAntiForgeryToken(instance, userAccount);

            using (WebRequestHandler handler = new WebRequestHandler())
            using (HttpClient client = new HttpClient(handler))
            {
                handler.ServerCertificateValidationCallback += HandleCertificateValidation;

                client.BaseAddress = new Uri(instance);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("X-GSF-Verify", antiForgeryToken);
                client.AddBasicAuthenticationHeader(userAccount.AccountName, userAccount.Password);

                HttpResponseMessage response = client.PostAsJsonAsync("api/PQMark/CreateChannel", record).Result;

                if (!response.IsSuccessStatusCode)
                    throw new InvalidOperationException($"Server returned status code {response.StatusCode}: {response.ReasonPhrase}");

                dynamic r = response.Content.ReadAsAsync<dynamic>();
                return (int)r.Result;
            }
        }

        public HttpResponseMessage UpdateChannelHub(string instance, JObject record, UserAccount userAccount)
        {
            return UpdateChannel(instance, record, userAccount);
        }

        public static HttpResponseMessage UpdateChannel(string instance, JObject record, UserAccount userAccount)
        {
            string antiForgeryToken = GenerateAntiForgeryToken(instance, userAccount);

            using (WebRequestHandler handler = new WebRequestHandler())
            using (HttpClient client = new HttpClient(handler))
            {
                handler.ServerCertificateValidationCallback += HandleCertificateValidation;

                client.BaseAddress = new Uri(instance);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("X-GSF-Verify", antiForgeryToken);
                client.AddBasicAuthenticationHeader(userAccount.AccountName, userAccount.Password);

                HttpResponseMessage response = client.PostAsJsonAsync("api/PQMark/UpdateChannel", record).Result;

                if (!response.IsSuccessStatusCode)
                    throw new InvalidOperationException($"Server returned status code {response.StatusCode}: {response.ReasonPhrase}");

                return response;
            }
        }

        public HttpResponseMessage AppendToFileBlobHub(string instance, JObject record, UserAccount userAccount)
        {
            return AppendToFileBlob(instance, record, userAccount);
        }

        public static HttpResponseMessage AppendToFileBlob(string instance, JObject record, UserAccount userAccount)
        {
            string antiForgeryToken = GenerateAntiForgeryToken(instance, userAccount);

            using (WebRequestHandler handler = new WebRequestHandler())
            using (HttpClient client = new HttpClient(handler))
            {
                handler.ServerCertificateValidationCallback += HandleCertificateValidation;

                client.BaseAddress = new Uri(instance);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("X-GSF-Verify", antiForgeryToken);
                client.AddBasicAuthenticationHeader(userAccount.AccountName, userAccount.Password);

                HttpResponseMessage response = client.PutAsJsonAsync("api/PQMark/AppendToFileBlob", record).Result;

                if (!response.IsSuccessStatusCode)
                    throw new InvalidOperationException($"Server returned status code {response.StatusCode}: {response.ReasonPhrase}");

                return response;
            }
        }


        public HttpResponseMessage ProcessFileGroupHub(string instance, JObject record, UserAccount userAccount)
        {
            return ProcessFileGroup(instance, record, userAccount);
        }

        public static HttpResponseMessage ProcessFileGroup(string instance, JObject record, UserAccount userAccount)
        {
            string antiForgeryToken = GenerateAntiForgeryToken(instance, userAccount);

            using (WebRequestHandler handler = new WebRequestHandler())
            using (HttpClient client = new HttpClient(handler))
            {
                handler.ServerCertificateValidationCallback += HandleCertificateValidation;

                client.BaseAddress = new Uri(instance);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("X-GSF-Verify", antiForgeryToken);
                client.AddBasicAuthenticationHeader(userAccount.AccountName, userAccount.Password);

                HttpResponseMessage response = client.PostAsJsonAsync("api/PQMark/ProcessFileGroup", record).Result;
                return response;
            }
        }


        public HttpResponseMessage DeleteRecordHub(string instance, string tableName, int id, UserAccount userAccount)
        {
            return DeleteRecord(instance, tableName, id, userAccount);
        }

        public static HttpResponseMessage DeleteRecord(string instance, string tableName, int id, UserAccount userAccount)
        {
            string antiForgeryToken = GenerateAntiForgeryToken(instance, userAccount);

            using (WebRequestHandler handler = new WebRequestHandler())
            using (HttpClient client = new HttpClient(handler))
            {
                handler.ServerCertificateValidationCallback += HandleCertificateValidation;

                client.BaseAddress = new Uri(instance);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("X-GSF-Verify", antiForgeryToken);
                client.AddBasicAuthenticationHeader(userAccount.AccountName, userAccount.Password);

                HttpResponseMessage response = client.DeleteAsync($"api/PQMark/DeleteRecord/{tableName}/{id}").Result;
                return response;
            }
        }

        private static bool HandleCertificateValidation(Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            SimpleCertificateChecker simpleCertificateChecker = new SimpleCertificateChecker();

            CategorizedSettingsElementCollection systemSettings = ConfigurationFile.Current.Settings["systemSettings"];
            systemSettings.Add("CertFile", "", "This is a certfile.");
            systemSettings.Add("ValidPolicyErrors", "None", "Password for PQMarkWeb API access.");
            systemSettings.Add("ValidChainFlags", "NoError", "Password for PQMarkWeb API access.");

            try
            {
                simpleCertificateChecker.ValidPolicyErrors = (SslPolicyErrors)Enum.Parse(typeof(SslPolicyErrors), (systemSettings["ValidPolicyErrors"].Value != "All" ? systemSettings["ValidPolicyErrors"].Value : "7"));
                simpleCertificateChecker.ValidChainFlags = (X509ChainStatusFlags)Enum.Parse(typeof(X509ChainStatusFlags), (systemSettings["ValidChainFlags"].Value != "All"? systemSettings["ValidChainFlags"].Value : (~0).ToString()));
                simpleCertificateChecker.TrustedCertificates.Add((!string.IsNullOrEmpty(systemSettings["CertFile"].Value) ? new X509Certificate2(systemSettings["CertFile"].Value) : certificate));
            }
            catch (Exception ex)
            {
                OnLogExceptionMessage(ex.ToString());
            }

            return simpleCertificateChecker.ValidateRemoteCertificate(sender, certificate, chain, sslPolicyErrors);
        }

        #endregion

    }
}
