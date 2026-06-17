//******************************************************************************************************
//  NoteController.cs - Gbtc
//
//  Copyright © 2020, Grid Protection Alliance.  All Rights Reserved.
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
//  03/26/2020 - Preston Crawford
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
    /// Controller that handles fetching the note types applicable to widget-relevant reference tables.
    /// </summary>
    [RoutePrefix("api/Widgets/Note/NoteType")]
    public class NoteTypeController : ModelController<LimitedNoteType> { }

    [RootQueryRestriction("[ReferenceTableName] IN ('Event', 'Meter', 'Asset', 'Location')")]
    public class LimitedNoteType : NoteType { }

    /// <summary>
    /// Controller that handles fetching note tags.
    /// </summary>
    [RoutePrefix("api/Widgets/Note/NoteTag")]
    public class NoteTagController : ModelController<NoteTag> { }

    /// <summary>
    /// Controller that handles fetching note applications.
    /// </summary>
    [RoutePrefix("api/Widgets/Note/NoteApp")]
    public class NoteAppController : ModelController<NoteApplication> { }

    /// <summary>
    /// Controller that handles fetching the Meter, Asset, and Location IDs associated with an openXDA event.
    /// </summary>
    [RoutePrefix("api/Widgets/Note/EventInformation")]
    public class EventInformationController : ApiController
    {
        private readonly Func<AdoDataConnection> m_connectionFactory;

        /// <summary>
        /// Constructor to pull a connection factory from the XDA controller activator.
        /// </summary>
        public EventInformationController(Func<AdoDataConnection> connectionFactory)
        {
            m_connectionFactory = connectionFactory;
        }

        [Route("{eventID:int}"), HttpGet]
        public IHttpActionResult GetEventInformation(int eventID)
        {
            using (AdoDataConnection connection = m_connectionFactory())
            {
                int meterID = connection.ExecuteScalar<int>("SELECT MeterID FROM Event WHERE ID = {0}", eventID);
                var ids = new
                {
                    Meter = meterID,
                    Asset = connection.ExecuteScalar<int>("SELECT AssetID FROM Event WHERE ID = {0}", eventID),
                    Location = connection.ExecuteScalar<int>("SELECT LocationID FROM Meter WHERE ID = {0}", meterID)
                };

                return Ok(ids);
            }
        }
    }
}

