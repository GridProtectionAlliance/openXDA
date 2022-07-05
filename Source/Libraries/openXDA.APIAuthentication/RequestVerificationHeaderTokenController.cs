//******************************************************************************************************
//  RequestVerificationHeaderTokenController.cs - Gbtc
//
//  Copyright © 2022, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the Eclipse Public License -v 1.0 (the "License"); you may
//  not use this file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://www.opensource.org/licenses/eclipse-1.0.php
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  07/05/2022 - Christoph Lackner
//       Generated original version of source code.
//
//******************************************************************************************************

using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using GSF.Web.Security;

namespace openXDA.APIAuthentication
{
    /// <summary>
    /// Provides Endpoint to request the GSFRequestverification Token.
    /// This needs to be included and Loaded in all Apps that support APIAuthentication
    /// </summary>
    [RoutePrefix("api/rvht")]
    public class RequestVerificationHeaderTokenController : ApiController
    {

        [HttpGet, Route("")]
        public HttpResponseMessage Get()
        {
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(Request.GenerateRequestVerficationHeaderToken(), Encoding.UTF8, "text/plain")
            };
        }
    }

}
