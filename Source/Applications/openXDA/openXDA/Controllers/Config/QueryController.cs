//******************************************************************************************************
//  Config/QueryController.cs - Gbtc
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
//  05/25/2018 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************

using GSF;
using GSF.Data;
using GSF.Data.Model;
using GSF.Identity;
using GSF.Web;
using GSF.Web.Model;
using Newtonsoft.Json;
using openHistorian.XDALink;
using openXDA.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace openXDA.Controllers
{
    [RoutePrefix("api/Config/Query")]
    public class QueryController : ApiController
    {
        public class PostQueryForm
        {
            public string Query { get; set; }
            public string SettingsCategory { get; set; }
            public int Timeout { get; set; }
        }

        [Route, HttpPost]
        public HttpResponseMessage Post(PostQueryForm form, CancellationToken token)
        {
            if (!AppModel.ValidateAdminRequestForRole("Developer", User.Identity.Name)) return Request.CreateResponse(HttpStatusCode.Unauthorized);

            try
            {
                using (AdoDataConnection connection = new AdoDataConnection(form.SettingsCategory))
                using (IDbCommand command = connection.Connection.CreateCommand())
                {
                    var task = Task.Run(() =>
                    {
                        command.CommandTimeout = form.Timeout;
                        // I know... And I like it...
                        command.CommandText = form.Query;
                        IDataAdapter dataAdapter = (IDataAdapter)Activator.CreateInstance(connection.AdapterType, command);
                        DataSet data = new DataSet("Temp");
                        dataAdapter.Fill(data);

                        return JsonConvert.SerializeObject(data.Tables[0].Select().Take(100).CopyToDataTable());
                    });

                    while (!task.Wait(100))
                    {
                        if (token.IsCancellationRequested)
                        {
                            command.Cancel();
                            return Request.CreateResponse(HttpStatusCode.OK, "Query canceled by you.");
                        }
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, task.Result);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.InnerException.Message);
            }
        }
    }
}
