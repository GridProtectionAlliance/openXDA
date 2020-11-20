//******************************************************************************************************
//  HIDSController.cs - Gbtc
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
//  11/17/2020 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using HIDS;
namespace openXDA.Controllers.WebAPI
{
    public class HIDSPost { 
        public string ChannelIDs { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
    [RoutePrefix("api/HIDS")]
    public class HIDSController : ApiController { 
        public IAsyncEnumerable<Point> Post([FromBody] HIDSPost post, CancellationToken cancellationToken)
        {
            HIDS.TrendingDataQuery hids = new HIDS.TrendingDataQuery();
            return hids.Query(post.ChannelIDs.Split(',').Select(x => int.Parse(x)).ToList(), post.StartTime, post.EndTime);
            
            
        }
    }
}