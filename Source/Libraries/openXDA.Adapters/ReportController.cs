//******************************************************************************************************
//  ReportController.cs - Gbtc
//
//  Copyright © 2018, Grid Protection Alliance.  All Rights Reserved.
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
//  07/25/2018 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************

using GSF.Data;
using GSF.Data.Model;
using openXDA.Model;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace openXDA.Adapters
{
    public class ReportController: ApiController
    {
        [HttpGet]
        public IHttpActionResult Get(int id, string name) {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            using(MemoryStream stream = new MemoryStream())
            {
               
                Report report = (new TableOperations<Report>(connection)).QueryRecordWhere("ID = {0}", id);
                Meter meter = (new TableOperations<Meter>(connection)).QueryRecordWhere("ID = {0}", report.MeterID);

                if (report == null) return BadRequest();

                stream.WriteAsync(report.PDF, 0, report.PDF.Length);
                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ByteArrayContent(stream.ToArray()),
                };
                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue($"inline")
                {
                    FileName = name
                };

                result.Content.Headers.ContentType =
                    new MediaTypeHeaderValue("application/pdf");

                return ResponseMessage(result);

            }

        }
    }
}
