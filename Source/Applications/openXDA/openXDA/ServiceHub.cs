//******************************************************************************************************
//  ServiceHub.cs - Gbtc
//
//  Copyright © 2016, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the MIT License (MIT), the "License"; you may
//  not use this file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://opensource.org/licenses/MIT
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  09/13/2016 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Threading.Tasks;
using log4net;
using Microsoft.AspNet.SignalR;

namespace openXDA
{
    public class ServiceHub : Hub
    {
        #region [ Constructors ]

        public ServiceHub()
        {
            ServiceConnection = ServiceConnection.Default;
        }

        #endregion

        #region [ Properties ]

        private ServiceConnection ServiceConnection { get; }

        #endregion

        #region [ Methods ]

        public override Task OnConnected()
        {
            s_connectCount++;
            Log.Info($"ServiceHub connect by {Context.User?.Identity?.Name ?? "Undefined User"} [{Context.ConnectionId}] - count = {s_connectCount}");
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            if (stopCalled)
            {
                s_connectCount--;
                ServiceConnection.Disconnect(Context.ConnectionId);
                Log.Info($"ServiceHub disconnect by {Context.User?.Identity?.Name ?? "Undefined User"} [{Context.ConnectionId}] - count = {s_connectCount}");
            }

            return base.OnDisconnected(stopCalled);
        }

        /// <summary>
        /// Gets the current server time.
        /// </summary>
        /// <returns>Current server time.</returns>
        public DateTime GetServerTime() => DateTime.UtcNow;

        /// <summary>
        /// Gets current performance statistics for service.
        /// </summary>
        /// <returns>Current performance statistics for service.</returns>
        public Task<string> GetPerformanceStatistics() =>
            ServiceConnection.Host.QueryEngineStatusAsync();

        /// <summary>
        /// Sends a service command.
        /// </summary>
        /// <param name="command">Command string.</param>
        public void SendCommand(string command) =>
            ServiceConnection.SendCommand(Context.ConnectionId, Context.User, command);

        #endregion

        #region [ Static ]

        // Static Fields
        private static readonly ILog Log = LogManager.GetLogger(typeof(ServiceHub));
        private static volatile int s_connectCount;

        #endregion
    }
}
