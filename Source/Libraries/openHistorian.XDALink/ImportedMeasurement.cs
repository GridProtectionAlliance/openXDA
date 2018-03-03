//******************************************************************************************************
//  ImportedMeasurement.cs - Gbtc
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
//  09/27/2017 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace openHistorian.XDALink
{
    public class ImportedMeasurement
    {
        public Guid? NodeID { get; set; }

        public Guid? SourceNodeID { get; set; }

        public Guid? SignalID { get; set; }

        [StringLength(200)]
        public string Source { get; set; }

        public long PointID { get; set; }

        [StringLength(200)]
        public string PointTag { get; set; }

        [StringLength(200)]
        public string AlternateTag { get; set; }

        [StringLength(4)]
        public string SignalTypeAcronym { get; set; }

        [StringLength(200)]
        public string SignalReference { get; set; }

        public int? FramesPerSecond { get; set; }

        [StringLength(200)]
        public string ProtocolAcronym { get; set; }

        [StringLength(200)]
        [DefaultValue("Frame")]
        public string ProtocolType { get; set; }

        public int? PhasorID { get; set; }

        public char? PhasorType { get; set; }

        public char? Phase { get; set; }

        [DefaultValue(0.0D)]
        public double Adder { get; set; }

        [DefaultValue(1.0D)]
        public double Multiplier { get; set; }

        [StringLength(200)]
        public string CompanyAcronym { get; set; }

        public double? Longitude { get; set; }

        public double? Latitude { get; set; }

        public string Description { get; set; }

        [DefaultValue(false)]
        public bool Enabled { get; set; }
    }

    public class ImportedMeasurementsTable
    {
        #region [ Members ]

        // Fields
        private string m_historianURL;
        private string m_authString;

        #endregion

        #region [ Constructors ]

        public ImportedMeasurementsTable(string historianURL, string username, string password)
        {
            m_historianURL = historianURL;

            string unencodedAuthString = $"{username}:{password}";
            byte[] authBytes = Encoding.UTF8.GetBytes(unencodedAuthString);
            m_authString = Convert.ToBase64String(authBytes);
        }

        #endregion

        #region [ Methods ]

        public IEnumerable<ImportedMeasurement> FindAll()
        {
            string url = GetURL("/api/importedmeasurements/findall");
            return RequestMeasurements(url);
        }

        public IEnumerable<ImportedMeasurement> FindByID(long pointID)
        {
            string url = GetURL($"/api/importedmeasurements/findbyid/{pointID}");
            return RequestMeasurements(url);
        }

        public IEnumerable<ImportedMeasurement> FindByPointTag(string pointTag)
        {
            string url = GetURL($"/api/importedmeasurements/findbypointtag/{pointTag}");
            return RequestMeasurements(url);
        }

        public IEnumerable<ImportedMeasurement> FindByAlternateTag(string alternateTag)
        {
            string url = GetURL($"/api/importedmeasurements/findbyalternatetag/{alternateTag}");
            return RequestMeasurements(url);
        }

        public void ImportMeasurements(IEnumerable<ImportedMeasurement> measurements)
        {
            string antiForgeryToken = GenerateAntiForgeryToken();
            string url = GetURL("/api/importedmeasurements/importmeasurements");

            using (MemoryStream stream = new MemoryStream())
            using (StreamWriter writer = new StreamWriter(stream))
            using (JsonTextWriter jsonWriter = new JsonTextWriter(writer))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(jsonWriter, measurements);
                jsonWriter.Flush();

                using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url))
                using (ByteArrayContent content = new ByteArrayContent(stream.ToArray()))
                using (HttpClient client = CreateClient())
                {
                    request.Content = content;
                    request.Headers.Add("X-GSF-Verify", antiForgeryToken);
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    client.SendAsync(request).Result.Dispose();
                }
            }
        }

        public void DeleteMeasurement(long pointID)
        {
            string antiForgeryToken = GenerateAntiForgeryToken();
            string url = GetURL($"/api/importedmeasurements/deletemeasurement/{pointID}");

            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, url))
            using (HttpClient client = CreateClient())
            {
                request.Headers.Add("X-GSF-Verify", antiForgeryToken);
                client.SendAsync(request).Result.Dispose();
            }
        }

        private IEnumerable<ImportedMeasurement> RequestMeasurements(string url)
        {
            using (HttpClient client = CreateClient())
            using (Stream response = client.GetStreamAsync(url).Result)
            using (StreamReader streamReader = new StreamReader(response))
            using (JsonTextReader jsonReader = new JsonTextReader(streamReader))
            {
                JsonSerializer serializer = new JsonSerializer();
                return serializer.Deserialize<IEnumerable<ImportedMeasurement>>(jsonReader);
            }
        }

        private string GenerateAntiForgeryToken()
        {
            string url = GetURL($"/api/importedmeasurements/generaterequestverficationtoken");

            using (HttpClient client = CreateClient())
            {
                return client.GetStringAsync(url).Result;
            }
        }

        private string GetURL(string relativePath)
        {
            string baseURL = m_historianURL.TrimEnd('/');
            string subURL = relativePath.TrimStart('/');
            return $"{baseURL}/{subURL}";
        }

        private HttpClient CreateClient()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", m_authString);
            return client;
        }

        #endregion
    }
}
