//******************************************************************************************************
//  ServiceConnection.cs - Gbtc
//
//  Copyright © 2016, Grid Protection Alliance.  All Rights Reserved.
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
//  01/15/2016 - J. Ritchie Carroll
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Security.Principal;
using GSF;
using GSF.Security;
using GSF.ServiceProcess;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNet.SignalR.Infrastructure;

namespace openXDA
{
    public class ServiceConnection
    {
        #region [ Members ]

        // Fields
        private readonly IHubConnectionContext<dynamic> m_clients;

        #endregion

        #region [ Constructors ]

        private ServiceConnection(IHubConnectionContext<dynamic> clients)
        {
            m_clients = clients;
            ServiceHost.Helper.UpdatedStatus += ServiceHelper_UpdatedStatus;
        }

        #endregion

        #region [ Properties ]

        public ServiceHost Host => ServiceHost;

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Sends a service command.
        /// </summary>
        /// <param name="connectionID">Client connection ID.</param>
        /// <param name="principal">The principal used for role-based security.</param>
        /// <param name="command">Command string.</param>
        public void SendCommand(string connectionID, IPrincipal principal, string command)
        {
            if (Guid.TryParse(connectionID, out Guid clientID))
                SendRequest(clientID, principal, command);
        }

        public void Disconnect(string connectionID)
        {
            if (Guid.TryParse(connectionID, out Guid clientID))
                ServiceHost.Helper.DisconnectClient(clientID);
        }

        private void SendRequest(Guid clientID, IPrincipal principal, string userInput)
        {
            ClientRequest request = ClientRequest.Parse(userInput);

            if (request is null)
                return;

            if (SecurityProviderUtility.IsResourceSecurable(request.Command) && !SecurityProviderUtility.IsResourceAccessible(request.Command, principal))
            {
                ServiceHost.Helper.UpdateStatus(clientID, UpdateType.Alarm, $"Access to \"{request.Command}\" is denied.\r\n\r\n");
                return;
            }

            ClientRequestHandler requestHandler = ServiceHost.Helper.FindClientRequestHandler(request.Command);

            if (requestHandler is null)
            {
                ServiceHost.Helper.UpdateStatus(clientID, UpdateType.Alarm, $"Command \"{request.Command}\" is not supported.\r\n\r\n");
                return;
            }

            ClientInfo clientInfo = new ClientInfo();
            clientInfo.ClientID = clientID;
            clientInfo.SetClientUser(principal);

            ClientRequestInfo requestInfo = new ClientRequestInfo(clientInfo, request);
            requestHandler.HandlerMethod(requestInfo);
        }

        private void ServiceHelper_UpdatedStatus(object sender, EventArgs<Guid, string, UpdateType> e)
        {
            string color;

            switch (e.Argument3)
            {
                case UpdateType.Alarm:
                    color = "red";
                    break;
                case UpdateType.Warning:
                    color = "yellow";
                    break;
                default:
                    color = "white";
                    break;
            }

            BroadcastMessage(e.Argument1, e.Argument2, color);
        }

        private void BroadcastMessage(Guid clientID, string message, string color)
        {
            dynamic client = m_clients.Client(clientID.ToString());

            if (string.IsNullOrEmpty(color))
                color = "white";

            client.broadcastMessage(message, color);
        }

        #endregion

        #region [ Static ]

        // Static Constructor
        static ServiceConnection()
        {
            IConnectionManager connectionmanager = GlobalHost.ConnectionManager;
            IHubContext hubContext = connectionmanager.GetHubContext<ServiceHub>();
            IHubConnectionContext<dynamic> clients = hubContext.Clients;
            ServiceConnection CreateServiceConnection() => new ServiceConnection(clients);
            DefaultInstance = new Lazy<ServiceConnection>(CreateServiceConnection);
        }

        // Static Properties
        public static ServiceConnection Default => DefaultInstance.Value;
        private static ServiceHost ServiceHost { get; set; }
        private static Lazy<ServiceConnection> DefaultInstance { get; }

        // Static Methods
        public static void InitializeDefaultInstance(ServiceHost serviceHost)
        {
            if (DefaultInstance.IsValueCreated)
                throw new InvalidOperationException("Default ServiceConnection instance has already been initialized.");

            ServiceHost = serviceHost;
        }

        #endregion
    }
}
