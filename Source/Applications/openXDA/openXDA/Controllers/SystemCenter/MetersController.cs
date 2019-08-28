//******************************************************************************************************
//  Meters.cs - Gbtc
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
//  08/26/2019 - Billy Ernest
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

namespace openXDA.Controllers.SystemCenter
{
    [RoutePrefix("api/SystemCenter/Meters")]
    public class MetersController : ApiController
    {
        [Route(Name = "SystemCenter_Meters")]
        public DataTable Get()
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                return connection.RetrieveData(@"
                    SELECT
                        Meter.ID,
                        Meter.AssetKey,
                        Meter.Name,
                        Meter.Make,
                        Meter.Model,
                        MeterLocation.Name as Location,
                        CASE
                            WHEN COUNT(MeterLine.LineID) > 1 THEN 'DFR'
                            ELSE 'PQMeter'
                        END as Type
                    FROM 
                        Meter JOIN
                        MeterLocation ON Meter.MeterLocationID = MeterLocation.ID JOIN
                        MeterLine ON Meter.ID = MeterLine.ID
					GROUP BY
                        Meter.ID,
						Meter.AssetKey,
                        Meter.Name,
                        Meter.Make,
                        Meter.Model,
                        MeterLocation.Name
                ");
            }
        }
    }
}