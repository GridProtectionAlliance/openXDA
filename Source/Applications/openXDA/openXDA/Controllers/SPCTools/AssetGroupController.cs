//******************************************************************************************************
//  SPCTools/AssetGroupController.cs - Gbtc
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
//  10/26/2020 - C. Lackner
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
using System.Collections.Specialized;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace openXDA.Controllers
{
    [RoutePrefix("api/SPCTools/AssetGroupView")]
    public class AssetGroupViewController : ModelController<AssetGroupView>
    {
        protected override bool ViewOnly => true;
        protected override bool AllowSearch => true;

        [HttpGet, Route("{assetGroupID:int}/Meters")]
        public IHttpActionResult GetMeters(int assetGroupID)
        {
            using (AdoDataConnection connection = new AdoDataConnection(Connection))
            {
                try
                {
                    IEnumerable<MeterAssetGroup> connectionRecords = new TableOperations<MeterAssetGroup>(connection).QueryRecordsWhere("AssetGroupID = {0}", assetGroupID);

                    IEnumerable<MeterDetail> records = new TableOperations<MeterDetail>(connection).QueryRecordsWhere($"ID in ({string.Join(",", connectionRecords.Select(item => item.MeterID))})");

                    return Ok(records);
                }
                catch (Exception ex)
                {
                    return InternalServerError(ex);
                }
            }
        }
    }
}
