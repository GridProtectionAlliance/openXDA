//******************************************************************************************************
//  ChannelViewController.cs - Gbtc
//
//  Copyright � 2020, Grid Protection Alliance.  All Rights Reserved.
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
//  10/28/2025 - G. Santos
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Web.Http;
using GSF.Data;
using GSF.Data.Model;
using GSF.Web.Model;
using openXDA.Model;

namespace openXDA.Controllers.Widgets
{
    /// <summary>
    /// Controller that handles fetching and searching of the <see cref="ChannelDetail"/> model.
    /// </summary>
    [RoutePrefix("api/Widgets/ChannelView")]
    public class ChannelViewController : ModelController<ChannelDetail>
    {
        /// <summary>
        /// Endpoint that mimics ChannelView searching with a <see cref="ParentKeyAttribute"/>,
        /// but replacing the normal <see cref="Meter"/> marked <see cref="ParentKeyAttribute"/> with <see cref="Event"/> .
        /// </summary>
        /// <param name="postData">Filtering <see cref="FromBodyAttribute"/> information used to filter results.</param>
        /// <param name="eventID"><see cref="Event"/> ID that is to be used as the <see cref="ParentKeyAttribute"/></param>
        [Route("ByParentEvent/{eventID:int}/SearchableList"), HttpPost]
        public virtual IHttpActionResult GetSearchableList([FromBody] PostData postData, int eventID)
        {
            if (!GetAuthCheck() || !AllowSearch)
            {
                return Unauthorized();
            }

            using (AdoDataConnection connection = ConnectionFactory())
            {
                Event evt = new TableOperations<Event>(connection).QueryRecordWhere("ID = {0}", eventID);
                if (evt is null)
                    return Ok();

                // Schema changed, assumption of MeterID being the ParentID of the model is no longer valid.
                if (ParentKey != "MeterID")
                    throw new InvalidOperationException("Underlying schema has changed. MeterID is no longer the parent key of channels." +
                        "Method must be corrected in code to utilize the new parent key.");

                return GetSearchableList(postData, evt.MeterID.ToString());
            }
        }
    }
}
