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

using GSF;
using GSF.Configuration;
using GSF.Web;
using GSF.Web.Model;
using Newtonsoft.Json.Linq;
using openXDA.Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;

namespace openXDA.DataPusher
{
    public class WebAPIHub
    {
        // Client-side script functionality
        #region [ Static ]

        public static event EventHandler<EventArgs<string>> LogStatusMessageEvent;

        private static readonly string s_userName;
        private static readonly string s_password;

        static WebAPIHub()
        {
            CategorizedSettingsElementCollection systemSettings = ConfigurationFile.Current.Settings["systemSettings"];

            systemSettings.Add("PQMarkUserName", "username", "User name for PQMarkWeb API access.");
            systemSettings.Add("PQMarkPassword", "password", "Password for PQMarkWeb API access.", true);

            s_userName = systemSettings["PQMarkUserName"].Value;
            s_password = systemSettings["PQMarkPassword"].Value;
        }

        private static void AddBasicAuthenticationHeader(HttpClient client)
        {
            client.AddBasicAuthenticationHeader(s_userName, s_password);
        }

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
                using (DataContext dataContext = new DataContext("systemSettings"))
                {
                    return dataContext.Table<Setting>().QueryRecordWhere("Name = 'CompanyName'")?.Value ?? "Synced Remote Devices";
                }
            }
        }


        #endregion

        #region [ RESTful API Handlers ]

        public dynamic GetRecordHub(string instance, string tableName, int id)
        {
            return GetRecord(instance, tableName, id);
        }

        public static dynamic GetRecord(string instance, string tableName, int id)
        {
            dynamic record = new object();
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(instance);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                AddBasicAuthenticationHeader(client);
                HttpResponseMessage response = client.GetAsync("api/PQMark/GetRecord/" + tableName + "/" + id).Result;
                if (response.IsSuccessStatusCode)
                {
                    record = response.Content.ReadAsAsync<dynamic>();
                }
            }
            return record.Result.ToObject(typeof(openXDA.Model.Meter).Assembly.GetType("openXDA.Model." + tableName));
        }

        public dynamic GetRecordsHub(string instance, string tableName, string ids)
        {
            return GetRecords(instance, tableName, ids);
        }

        public static IEnumerable<dynamic> GetRecords(string instance, string tableName, string ids)
        {
            dynamic record = new object();
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(instance);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                AddBasicAuthenticationHeader(client);
                HttpResponseMessage response = client.GetAsync("api/PQMark/GetRecords/" + tableName + "/" + ids).Result;
                if (response.IsSuccessStatusCode)
                {
                    record = response.Content.ReadAsAsync<dynamic>();
                }
            }

            Type type = typeof(openXDA.Model.Meter).Assembly.GetType("openXDA.Model." + tableName);
            Type listType = typeof(IEnumerable<>).MakeGenericType(type);
            return record.Result.ToObject(listType);
        }

        public dynamic GetRecordsWhereHub(string instance, string tableName, string ids)
        {
            return GetRecordsWhere(instance, tableName, ids);
        }

        public static IEnumerable<dynamic> GetRecordsWhere(string instance, string tableName, string ids)
        {
            dynamic record = new object();
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(instance);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                AddBasicAuthenticationHeader(client);
                HttpResponseMessage response = client.GetAsync("api/PQMark/GetRecordsWhere/" + tableName + "/" + ids).Result;
                if (response.IsSuccessStatusCode)
                {
                    record = response.Content.ReadAsAsync<dynamic>();
                }
            }

            Type type = typeof(openXDA.Model.Meter).Assembly.GetType("openXDA.Model." + tableName);
            Type listType = typeof(IEnumerable<>).MakeGenericType(type);
            return record.Result.ToObject(listType);
        }



        public dynamic GetChannelsHub(string instance, string ids)
        {
            return GetChannels(instance, ids);
        }

        public static IEnumerable<ChannelDetail> GetChannels(string instance, string ids)
        {
            dynamic record = new object();
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(instance);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                AddBasicAuthenticationHeader(client);
                HttpResponseMessage response = client.GetAsync("api/PQMark/GetChannels/channel/" + ids).Result;
                if (response.IsSuccessStatusCode)
                {
                    record = response.Content.ReadAsAsync<dynamic>();
                }
            }

            return record.Result.ToObject(typeof(IEnumerable<ChannelDetail>));
        }


        public HttpResponseMessage UpdateRecordHub(string instance, string tableName, JObject record)
        {
            return UpdateRecord(instance, tableName, record);
        }

        public static HttpResponseMessage UpdateRecord(string instance, string tableName, JObject record)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(instance);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                AddBasicAuthenticationHeader(client);
                HttpResponseMessage response = client.PutAsJsonAsync("api/PQMark/UpdateRecord/" + tableName, record).Result;
                return response;
            }

        }

        public int CreateRecordHub(string instance, string tableName, JObject record)
        {
            return CreateRecord(instance, tableName, record);
        }

        public static int CreateRecord(string instance, string tableName, JObject record)
        {
            dynamic r = new object();

            using (HttpClient client = new HttpClient())
            {

                client.BaseAddress = new Uri(instance);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                AddBasicAuthenticationHeader(client);
                HttpResponseMessage response = client.PostAsJsonAsync("api/PQMark/CreateRecord/" + tableName, record).Result;
                if (response.IsSuccessStatusCode)
                {
                    r = response.Content.ReadAsAsync<dynamic>();
                }

                return (int)r.Result;
            }
        }

        public int CreateChannelHub(string instance, JObject record)
        {
            return CreateChannel(instance, record);
        }

        public static int CreateChannel(string instance, JObject record)
        {
            dynamic r = new object();

            using (HttpClient client = new HttpClient())
            {

                client.BaseAddress = new Uri(instance);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                AddBasicAuthenticationHeader(client);
                HttpResponseMessage response = client.PostAsJsonAsync("api/PQMark/CreateChannel", record).Result;
                if (response.IsSuccessStatusCode)
                {
                    r = response.Content.ReadAsAsync<dynamic>();
                }

                return (int)r.Result;
            }
        }


        public HttpResponseMessage ProcessFileGroupHub(string instance, JObject record)
        {
            return ProcessFileGroup(instance, record);
        }

        public static HttpResponseMessage ProcessFileGroup(string instance, JObject record)
        {
            using (HttpClient client = new HttpClient())
            {

                client.BaseAddress = new Uri(instance);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                AddBasicAuthenticationHeader(client);
                HttpResponseMessage response = client.PostAsJsonAsync("api/PQMark/ProcessFileGroup", record).Result;
                return response;
            }
        }


        public HttpResponseMessage DeleteRecordHub(string instance, string tableName, int id)
        {
            return DeleteRecord(instance, tableName, id);
        }

        public static HttpResponseMessage DeleteRecord(string instance, string tableName, int id)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(instance);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                AddBasicAuthenticationHeader(client);
                HttpResponseMessage response = client.DeleteAsync("api/PQMark/DeleteRecord/" + tableName + "/" + id).Result;
                return response;
            }
        }

        #endregion

    }
}
