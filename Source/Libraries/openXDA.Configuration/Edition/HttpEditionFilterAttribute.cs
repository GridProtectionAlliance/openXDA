//******************************************************************************************************
//  HttpEditionFilterAttirbute.cs - Gbtc
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
//  10/07/2024 - Gabriel Santos
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Net.Http;
using System.Net;

namespace openXDA.Configuration
{
    /// <summary>
    /// Defines an attribute that will disallow an http method if it does not pass the edition requirement.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class HttpEditionFilterAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Gets the edition needed specified by attribute construction.
        /// </summary>
        public Edition EditionRequred { get; }
        public HttpEditionFilterAttribute(Edition edition)
        {
            EditionRequred = edition;
        }

        /// <summary>
        /// Creates a new <see cref="HttpEditionFilterAttribute"/>.
        /// </summary>
        /// <param name="EditionRequred">Edition this http method requires.</param>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            // Wronge edition means we should skip the method and return forbidden
            if (!EditionChecker.CheckEdition(EditionRequred))
                actionContext.Response = new HttpResponseMessage(HttpStatusCode.Forbidden);
            base.OnActionExecuting(actionContext);
        }
    }
}
