﻿//******************************************************************************************************
//  APIPingController.cs - Gbtc
//
//  Copyright © 2023, Grid Protection Alliance.  All Rights Reserved.
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
//  08/08/2024 - Christoph Lackner
//       Generated original version of source code.
//
//******************************************************************************************************

using System.Web.Http;

namespace openXDA.APIMiddleware
{
    /// <summary>
    /// Base Class that includes the endpoint needed for Pinging the service.
    /// </summary>
    public abstract class APIPingController : ApiController
    {
        /// <summary>
        /// Returns a 1 if the service is running
        /// </summary>
        /// <returns> a simple 1 </returns>
        [Route(""), HttpGet]
        public IHttpActionResult Connect()
        {
            return Ok(1);          
        }
    }
}
