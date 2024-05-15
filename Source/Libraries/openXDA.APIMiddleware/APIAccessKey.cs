//******************************************************************************************************
//  APIAccessKey.cs - Gbtc
//
//  Copyright © 2020, Grid Protection Alliance.  All Rights Reserved.
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
//  07/01/2022 - Christoph Lackner
//       Generated original version of source code.
//
//******************************************************************************************************

using GSF.Data.Model;
using System;

namespace openXDA.APIMiddleware
{
    /// <summary>
    /// Model that represents a valid set of credentials to access the API.
    /// </summary>
    public class APIAccessKey
    {
        /// <summary>
        /// Unique identifier for the API access key.
        /// </summary>
        [PrimaryKey(true)]
        public int ID { get; set; }

        /// <summary>
        /// Unique registration key to identify the user.
        /// </summary>
        public string RegistrationKey { get; set; }

        /// <summary>
        /// The access token used to authenticate the user.
        /// </summary>
        public string APIToken { get; set; }

        /// <summary>
        /// The date and time at which the access key is no longer valid.
        /// </summary>
        public DateTime Expires { get; set; }

        /// <summary>
        /// A Flag indicating if this key is allowed to impersonate regular users.
        /// see <see cref="APIAuthenticationMiddleware"/> for how to impersonate a user in a request.
        /// </summary>
        public bool AllowImpersonation { get; set; }
    }
}
