//******************************************************************************************************
//  IAPIConsoleHost.cs - Gbtc
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
//  06/27/2023 - Christoph Lackner
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Security.Principal;
using GSF;
using GSF.ServiceProcess;

namespace openXDA.APIMiddleware
{
    /// <summary>
    /// Interface for a Program.Host to include all required pieces for a <see cref="APIConsoleController"/> to hook into.
    /// </summary>
    public interface IAPIConsoleHost
    {
        /// <summary>
        /// Raised when there is a new status message reported to service.
        /// </summary>
        event EventHandler<EventArgs<Guid, string, UpdateType>> UpdatedStatus;

        /// <summary>
        /// Raise when a response is being sent to one or more clients.
        /// </summary>
        event EventHandler<EventArgs<Guid, ServiceResponse, bool>> SendingClientResponse;

        /// <summary>
        /// Sends a command request to the service.
        /// </summary>
        /// <param name="clientID">Client ID of sender.</param>
        /// <param name="principal">The principal used for role-based security.</param>
        /// <param name="userInput">Request string.</param>
        void SendRequest(Guid clientID, IPrincipal principal, string userInput);

        /// <summary>
        /// Disconnects a client from the service.
        /// </summary>
        /// <param name="clientID">Client ID</param>
        void DisconnectClient(Guid clientID);
    }
}
