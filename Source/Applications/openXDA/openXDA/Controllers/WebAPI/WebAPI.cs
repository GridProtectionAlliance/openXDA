//******************************************************************************************************
//  WebAPI.cs - Gbtc
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
//  10/06/2020 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************

using GSF.Web.Security;
using openXDA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;

namespace openXDA.Controllers.WebAPI
{
    public class WebAPI
    {
    }

    [RoutePrefix("api/rvht")]
    public class RequestVerificationHeaderTokenController: ApiController
    {
        [HttpGet,Route("")]
        public HttpResponseMessage Get()
        {
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(Request.GenerateRequestVerficationHeaderToken(), Encoding.UTF8, "text/plain")
            };
        }

    }


    [RoutePrefix("api/Meter")]
    public class MeterController : ModelController<Meter> { }

    [RoutePrefix("api/Asset")]
    public class AssetController : ModelController<Asset> { }

    [RoutePrefix("api/Phase")]
    public class PhaseController : ModelController<Phase> { }

    [RoutePrefix("api/EventType")]
    public class EventTypeController : ModelController<EventType> { }

    [RoutePrefix("api/MeasurementType")]
    public class MeasurementTypeController : ModelController<MeasurementType> { }

    [RoutePrefix("api/MeasurementCharacteristic")]
    public class MeasurementCharacteristicController : ModelController<MeasurementCharacteristic> { }

    [RoutePrefix("api/ChannelGroup")]
    public class ChannelGroupController : ModelController<ChannelGroup> { }

    [RoutePrefix("api/ChannelGroupType")]
    public class ChannelGroupTypeController : ModelController<ChannelGroupType> { }

}