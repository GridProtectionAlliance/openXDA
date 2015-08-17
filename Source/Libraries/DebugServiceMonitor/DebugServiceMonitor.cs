//******************************************************************************************************
//  DebugServiceMonitor.cs - Gbtc
//
//  Copyright © 2013, Grid Protection Alliance.  All Rights Reserved.
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
//  08/01/2013 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Diagnostics;
using GSF.ServiceProcess;

namespace DebugServiceMonitor
{
    /// <summary>
    /// Implementation of <see cref="IServiceMonitor"/> used for debugging purposes.
    /// </summary>
    public class DebugServiceMonitor : ServiceMonitorBase
    {
        /// <summary>
        /// Handles notifications from the service that occur
        /// on an interval to indicate that the service is
        /// still running.
        /// </summary>
        public override void HandleServiceHeartbeat()
        {
            Debug.WriteLine("...Heartbeat...");
        }

        /// <summary>
        /// Handles messages received by the service
        /// whenever the service encounters an error.
        /// </summary>
        /// <param name="ex">The error received from the service.</param>
        public override void HandleServiceError(Exception ex)
        {
            Debug.WriteLine("ERROR: {0}", (object)ex.Message);
        }

        /// <summary>
        /// Handles messages sent by a client.
        /// </summary>
        /// <param name="args">Arguments provided by the client.</param>
        public override void HandleClientMessage(string[] args)
        {
            Debug.WriteLine("Client says, \"{0}\"", (object)string.Join(" ", args));
        }
    }
}
