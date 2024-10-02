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

using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using GSF;
using GSF.Configuration;
using GSF.Data;
using GSF.Data.Model;
using GSF.Net.Security;
using GSF.Security.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using openXDA.APIAuthentication;
using openXDA.Model;

namespace openXDA.DataPusher
{
    public class DataPusherRequester
    {
        private UserAccount m_userAccount = null;

        private string m_token = null;
        private string m_key = null;

        private string m_baseUrl = null;
        private bool m_useApiAuth;

        public static event EventHandler<EventArgs<string>> LogStatusMessageEvent;
        public static event EventHandler<EventArgs<string>> LogExceptionMessageEvent;
        private static readonly HttpClient staticClient = new HttpClient();
        private static WebRequestHandler WebRequestHandler { get; }
        private static HttpClient s_staticClient { get; }

        #region [ Constructor ]

        static DataPusherRequester()
        {
            WebRequestHandler = new WebRequestHandler();
            WebRequestHandler.ServerCertificateValidationCallback = HandleCertificateValidation;
            s_staticClient = new HttpClient(WebRequestHandler);
        }

        public DataPusherRequester(string instance, UserAccount userAccount)
        {
            m_baseUrl = instance.TrimEnd('/');
            m_userAccount = userAccount;
            m_useApiAuth = false;
        }

        public DataPusherRequester(string instance, string token, string key)
        {
            m_baseUrl = instance.TrimEnd('/');
            m_token = token;
            m_key = key;
            m_useApiAuth = true;
        }

        public DataPusherRequester(int instanceID, AdoDataConnection connection) : this(new TableOperations<RemoteXDAInstance>(connection).QueryRecordWhere("ID = {0}", instanceID), connection) { }

        public DataPusherRequester(RemoteXDAInstance instance, AdoDataConnection connection)
        {
            m_useApiAuth = instance.UseApiAuth;
            if (instance.UseApiAuth)
            {
                m_baseUrl = instance.Address.TrimEnd('/');
                m_token = instance.APIToken;
                m_key = instance.RegistrationKey;
            }
            else
            {
                UserAccount userAccount = new TableOperations<UserAccount>(connection).QueryRecordWhere("ID = {0}", instance.UserAccountID);
                m_baseUrl = instance.Address.TrimEnd('/');
                m_userAccount = userAccount;
            }
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
            if (m_useApiAuth)
                return await SendAPIQueryAsync(path, method, content, mediaResponseType);
            else
                return await SendLegacyRequestAsync(path, method, content, mediaResponseType);
        }

        private async Task<HttpResponseMessage> SendAPIQueryAsync(string path, HttpMethod method, HttpContent content, string mediaResponseType)
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

        private async Task<HttpResponseMessage> SendLegacyRequestAsync(string path, HttpMethod method, HttpContent content, string mediaResponseType)
        {
            string url = $"{m_baseUrl}/{path}";
            string antiForgeryToken = await GenerateAntiForgeryTokenAsync(m_baseUrl, m_userAccount);

            using (HttpRequestMessage request = new HttpRequestMessage(method, url))
            {
                ConfigureRequest(request, m_userAccount, mediaResponseType);
                request.Headers.Add("X-GSF-Verify", antiForgeryToken);

                if (!(content is null))
                    request.Content = content;

                HttpResponseMessage response = await s_staticClient.SendAsync(request);
                return response;
            }
        }

        #endregion

        #region [ Static ]
        private static void OnLogStatusMessage(string message)
        {
            LogStatusMessageEvent?.Invoke(new object(), new EventArgs<string>(message));
        }

        private static void OnLogExceptionMessage(string message)
        {
            LogExceptionMessageEvent?.Invoke(new object(), new EventArgs<string>(message));
        }

        private static bool HandleCertificateValidation(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            SimpleCertificateChecker simpleCertificateChecker = new SimpleCertificateChecker();

            CategorizedSettingsElementCollection systemSettings = ConfigurationFile.Current.Settings["systemSettings"];
            systemSettings.Add("CertFile", "", "Certificate for WebAPIHub used by PQMark/DataPusher.");
            systemSettings.Add("ValidPolicyErrors", "None", "Expected policy errors during remote server certificate validation for PQMark/DataPusher (self-signed: RemoteCertificateNameMismatch, RemoteCertificateChainErrors).");
            systemSettings.Add("ValidChainFlags", "NoError", "Expected chain flags set during remote server certificate validation for PQMark/DataPusher (self-signed: UntrustedRoot).");

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

        private static async Task<string> GenerateAntiForgeryTokenAsync(string instance, UserAccount userAccount)
        {
            string url = $"{instance}/api/PQMark/GenerateRequestVerficationToken";

            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url))
            {
                ConfigureRequest(request, userAccount, "application/json");

                using (HttpResponseMessage response = await s_staticClient.SendAsync(request))
                {
                    if (!response.IsSuccessStatusCode)
                        throw new InvalidOperationException($"Server returned status code {response.StatusCode}: {response.ReasonPhrase}");

                    return await response.Content.ReadAsStringAsync();
                }
            }
        }

        private static void ConfigureRequest(HttpRequestMessage request, UserAccount userAccount, string mediaResponseType)
        {
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaResponseType));

            Encoding utf8 = new UTF8Encoding(false);
            string credential = $"{userAccount.AccountName}:{userAccount.Password}";
            byte[] accountData = utf8.GetBytes(credential);
            string encodedCredential = Convert.ToBase64String(accountData);
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", encodedCredential);
        }

        #endregion
    }
}