//******************************************************************************************************
//  FileInfoController.cs - Gbtc
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
//  08/08/2023 - Preston Crawford
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Data;
using System.Linq;
using System.Web.Http;
using GSF.Data;
using GSF.Data.Model;
using openXDA.Model;

namespace openXDA.Controllers.Widgets
{
    /// <summary>
    /// Controller that handles fetching file and channel mapping information for an openXDA event.
    /// </summary>
    [RoutePrefix("api/Widgets/FileInfo")]
    public class FileInfoController : ApiController
    {
        private readonly Func<AdoDataConnection> m_connectionFactory;

        /// <summary>
        /// Constructor to pull a connection factory from the XDA controller activator.
        /// </summary>
        public FileInfoController(Func<AdoDataConnection> connectionFactory)
        {
            m_connectionFactory = connectionFactory;
        }

        [Route("GetFileName/{eventID:int}"), HttpGet]
        public IHttpActionResult GetFileInfo(int eventID)
        {
            using (AdoDataConnection connection = m_connectionFactory())
            {
                DataFile dataFile = new TableOperations<DataFile>(connection).QueryRecord("FileSize DESC", new RecordRestriction("FileGroupID = (SELECT FileGroupID FROM Event WHERE ID = {0})", eventID));
                return Ok(dataFile.FilePath);
            }
        }

        [Route("GetMappedChannels/{eventID:int}"), HttpGet]
        public IHttpActionResult GetMappedChannels(int eventID)
        {
            using (AdoDataConnection connection = m_connectionFactory())
            {
                const string SQL = @"
                    SELECT
                        Channel.ID,
                        Channel.Name as Channel,
                        Series.SourceIndexes as Mapping
                    FROM
                        Event LEFT JOIN
                        Channel ON Event.MeterID = Channel.MeterID AND Event.AssetID = Channel.AssetID LEFT JOIN
                        Series ON Channel.ID = Series.ChannelID LEFT JOIN
                        MeasurementType ON MeasurementType.ID = Channel.MeasurementTypeID LEFT JOIN
                        Phase ON Channel.PhaseID = Phase.ID
                    WHERE
                        Event.ID = {0} AND Enabled = 1 AND Series.SourceIndexes !=''
                    ORDER BY
                        MeasurementType.Name DESC, Phase.Name ASC
                ";

                return Ok(connection.RetrieveData(SQL, eventID));
            }
        }

        [Route("GetMeterConfiguration/{eventID:int}"), HttpGet]
        public IHttpActionResult GetMeterConfiguration(int eventID)
        {
            using (AdoDataConnection connection = m_connectionFactory())
            {
                const string SQL = @"
                    SELECT
                        Meter.AssetKey as MeterKey,
                        FileGroupMeterConfiguration.MeterConfigurationID
                    FROM
                        FileGroup JOIN
                        Event ON Event.FileGroupID = FileGroup.ID LEFT JOIN
                        FileGroupMeterConfiguration ON FileGroup.ID = FileGroupMeterConfiguration.FileGroupID JOIN
                        Meter ON Event.MeterID = Meter.ID
                    WHERE
                        Event.ID = {0}
                ";

                DataTable dataTable = connection.RetrieveData(SQL, eventID);
                return Ok(dataTable.Select().First().ItemArray);
            }
        }
    }
}
