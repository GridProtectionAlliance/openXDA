//******************************************************************************************************
//  EmailTemplateController.cs - Gbtc
//
//  Copyright © 2019, Grid Protection Alliance.  All Rights Reserved.
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
//  08/19/2019 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using GSF.Data;
using GSF.Data.Model;
using openXDA.Model;

namespace openXDA.Controllers.Config
{
    [RoutePrefix("api/Config/EmailTemplate")]
    public class EmailTemplateController : ApiController
    {
        [Route("{templateID}"), HttpGet]
        public List<EventView> Get(int templateID)
        {
            NameValueCollection queryParameters = Request.RequestUri.ParseQueryString();
            string countValue = queryParameters["count"];

            if (!int.TryParse(countValue, out int count))
                count = 50;

            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                string getTriggerSQL =
                    "SELECT TriggersEmailSQL " +
                    "FROM " +
                    "    EventEmailParameters JOIN " +
                    "    EmailType ON EventEmailParameters.EmailTypeID = EmailType.ID " +
                    "WHERE XSLTemplateID = {0}";

                string triggerFormat = connection.ExecuteScalar<string>(getTriggerSQL, templateID);
                string triggerSQL = string.Format(triggerFormat, "E.ID");

                string getEvents =
                    $"SELECT TOP {count} EventView.* " +
                    $"FROM " +
                    $"    Event as E CROSS APPLY " +
                    $"    ({triggerSQL}) EmailTrigger(Value) JOIN" +
                    $"    EventView ON E.ID = EventView.ID " +
                    $"WHERE EmailTrigger.Value <> 0 " +
                    $"ORDER BY EventView.StartTime DESC";

                TableOperations<EventView> eventTable = new TableOperations<EventView>(connection);

                return connection.RetrieveData(getEvents)
                    .AsEnumerable()
                    .Select(eventTable.LoadRecord)
                    .ToList();
            }

        }
    }
}