//******************************************************************************************************
//  ESRIMapController.cs - Gbtc
//
//  Copyright © 2023, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the MIT License (MIT), the "License"; you may not use this
//  file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://opensource.org/licenses/MIT
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  08/22/2023 - Preston Crawford
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Http;
using GSF.Data;

namespace openXDA.Controllers.Widgets
{
    /// <summary>
    /// Controller that handles fetching lightning, structure, and image data for the ESRI map widget.
    /// </summary>
    [RoutePrefix("api/Widgets/ESRIMap")]
    public class ESRIMapController : ApiController
    {
        private readonly Func<AdoDataConnection> m_connectionFactory;

        /// <summary>
        /// Constructor to pull a connection factory from the XDA controller activator.
        /// </summary>
        public ESRIMapController(Func<AdoDataConnection> connectionFactory)
        {
            m_connectionFactory = connectionFactory;
        }

        [Route("GetLightningInfo/{eventID:int}/{timeWindow:int}"), HttpGet]
        public IHttpActionResult GetLightningInfo(int eventID, int timeWindow)
        {
            using (AdoDataConnection connection = m_connectionFactory())
            {
                const string SQL = @"
                    SELECT
                        LightningStrike.ID, Service, DisplayTime, Amplitude, Latitude,Longitude
                    FROM
                        LightningStrike JOIN
                        Event ON LightningStrike.EventID = Event.ID JOIN
                        FaultSummary ON Event.ID = FaultSummary.EventID AND FaultSummary.IsSelectedAlgorithm = 1
                    WHERE
                        Event.ID = {0} AND CAST(LightningStrike.DisplayTime as datetime2) BETWEEN DateAdd(S,-{1}, FaultSummary.Inception) AND  DateAdd(S,{1}, FaultSummary.Inception)
                ";

                return Ok(connection.RetrieveData(SQL, eventID, timeWindow));
            }
        }

        [Route("NearestStructure/{station}/{line}"), HttpGet]
        public IHttpActionResult GetNearestStructure(string station, string line, string mileage)
        {
            string url = string.Format("http://opsptpsnet.cha.tva.gov:8025/TLI/StructureCrawler/FaultFinder.asp?Station={0}&Line={1}&Mileage={2}", station, line, mileage);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.UseDefaultCredentials = true;

            using (WebResponse response = request.GetResponse())
            using (Stream responseStream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(responseStream))
            {
                string html = reader.ReadToEnd();
                string text = WebUtility.HtmlDecode(Regex.Replace(html, "<.*?>", string.Empty)).Trim();
                return Ok(ToDataTable(text));
            }
        }

        [HttpGet, Route("Image/{base64Encoded}")]
        public HttpResponseMessage GetImage(string base64Encoded)
        {
            byte[] data = Convert.FromBase64String(base64Encoded);
            string filePath = Encoding.UTF8.GetString(data);

            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            FileStream fileStream = File.OpenRead(filePath);
            result.Content = new StreamContent(fileStream);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
            return result;
        }

        private DataTable ToDataTable(string csvInput)
        {
            char[] newLineChars = { '\r', '\n' };
            string[] lines = csvInput.Trim()
                .Split(newLineChars, StringSplitOptions.RemoveEmptyEntries)
                .Select(line => line.Trim())
                .Where(line => !string.IsNullOrEmpty(line))
                .ToArray();

            string[] fields = lines[0].Split(',');
            DataTable table = new DataTable();

            foreach (string field in fields)
                table.Columns.Add(field);

            foreach (string row in lines.Skip(1))
                table.Rows.Add(row.Split(','));

            return table;
        }
    }
}
