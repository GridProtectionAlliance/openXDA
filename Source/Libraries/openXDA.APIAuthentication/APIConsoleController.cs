//******************************************************************************************************
//  APIConsoleController.cs - Gbtc
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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Security.Principal;
using System.Web.Http;
using GSF;
using GSF.ServiceProcess;

namespace openXDA.APIAuthentication
{
    /// <summary>
    /// Base Class that includes the endpoints needed for the API based Console.
    /// </summary>
    public abstract class  APIConsoleController : ApiController
    {
        #region [ Members ]

        internal class ConsoleState
        {
            public Guid SessionID { get; }
            public DateTime Established { get; }
            public DateTime LastUpdate { get; private set; }

            private bool m_isDisconnected;
            private IPrincipal m_user;
            private IAPIConsoleHost m_host;
            private List<ConsoleMessage> m_messages;
            private object m_messageLock = new object();

            public ConsoleState(IPrincipal User, IAPIConsoleHost Host)
            {
                Established = DateTime.UtcNow;
                LastUpdate = DateTime.UtcNow;
                SessionID = Guid.NewGuid();
                m_messages = new List<ConsoleMessage>();
                m_user = User;
                m_host = Host;
                m_isDisconnected = false;
                m_host.UpdatedStatus += m_serviceHost_UpdatedStatus;
                m_host.SendingClientResponse += m_serviceHost_SendingClientResponse;

                //send Filter as Initial Command
                SendCommand("Filter -Remove 0", User);
            }

            private void m_serviceHost_UpdatedStatus(object sender, EventArgs<Guid, string, UpdateType> e)
            {
                if ((DateTime.UtcNow - LastUpdate).TotalMinutes > 10)
                {
                    Disconnect();
                    return;
                }

                if (e.Argument1 != Guid.Empty && e.Argument1 != SessionID)
                    return;
                lock (m_messageLock)
                    m_messages.Add(new ConsoleMessage() { Message = e.Argument2, Type = e.Argument3 });
            }

            private void m_serviceHost_SendingClientResponse(object sender, EventArgs<Guid, ServiceResponse, bool> e)
            {
                if ((DateTime.UtcNow - LastUpdate).TotalMinutes > 10)
                {
                    Disconnect();
                    return;
                }

                if (e.Argument1 != Guid.Empty && e.Argument1 != SessionID)
                    return;

                // If actionable client response is successful and targeted for this hub client,
                // inform service helper that it does not need to broadcast a response
                if (e.Argument1 == SessionID)
                    e.Argument3 = false;

                lock (m_messageLock)
                    m_messages.Add(new ConsoleMessage() { Message = e.Argument2.Message, Type = UpdateType.Information });
            }

            public void SendCommand(string command, IPrincipal User)
            {
                if (User.Identity.Name != m_user.Identity.Name || !User.Identity.IsAuthenticated)
                    throw new UnauthorizedAccessException("You are not authorized to send commands to this session.");
                m_host.SendRequest(SessionID, User, command);
                LastUpdate = DateTime.UtcNow;
            }

            public List<ConsoleMessage> GetUpdates(IPrincipal User)
            {
                if (User.Identity.Name != m_user.Identity.Name || !User.Identity.IsAuthenticated)
                    throw new UnauthorizedAccessException("You are not authorized to connect to this session.");
                lock (m_messageLock)
                {
                    List<ConsoleMessage> messages = new List<ConsoleMessage>(m_messages);
                    m_messages.Clear();

                    LastUpdate = DateTime.UtcNow;
                    return messages;
                }
            }

            public bool CheckExpiration()
            {
                if (m_isDisconnected)
                    return true;

                if ((DateTime.UtcNow - LastUpdate).TotalMinutes > 10)
                {
                    Disconnect();
                    return true;
                }

                return false;
            }

            private void Disconnect()
            {
                m_isDisconnected = true;
                m_host.UpdatedStatus -= m_serviceHost_UpdatedStatus;
                m_host.SendingClientResponse -= m_serviceHost_SendingClientResponse;
                m_host.DisconnectClient(SessionID);
            }
        }

        /// <summary>
        /// Represents a Console Message and the corresponding <see cref="UpdateType"/>
        /// </summary>
        public class ConsoleMessage
        {
            /// <summary>
            /// Gets or sets the text of the message.
            /// </summary>
            public string Message { get; set; }

            /// <summary>
            /// Gets or sets the type of the message.
            /// </summary>
            public UpdateType Type { get; set; }
        }

        private static ConcurrentDictionary<Guid, ConsoleState> s_activeConnections = new ConcurrentDictionary<Guid, ConsoleState>();

        /// <summary>
        /// The <see cref="IAPIConsoleHost"/> that the console is attached to
        /// </summary>
        protected virtual IAPIConsoleHost Host { get; }

        #endregion

        #region [ HTTP Methods ]

        /// <summary>
        /// Connects a new Client to the console
        /// </summary>
        /// <returns>The Session ID of the new connection</returns>
        [Route("Connect"), HttpGet]
        public IHttpActionResult Connect()
        {
            try
            {
                ConsoleState connection = new ConsoleState(User, Host);
                s_activeConnections.TryAdd(connection.SessionID, connection);
                return Ok(connection.SessionID);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Retrieves the messages for this session
        /// </summary>
        [Route("Retrieve/{session}"), HttpGet]
        public IHttpActionResult Retrieve(string session)
        {
            foreach (ConsoleState connection in s_activeConnections.Values)
            {
                if (connection.CheckExpiration())
                    s_activeConnections.TryRemove(connection.SessionID, out ConsoleState state);
            }

            if (!Guid.TryParse(session, out Guid sessionID))
                return BadRequest("Invalid Session ID");
            if (!s_activeConnections.ContainsKey(sessionID))
                return BadRequest("Session not found");
            return Ok(s_activeConnections[sessionID].GetUpdates(User));
        }

        /// <summary>
        /// Send a command to the console
        /// </summary>
        [Route("Send/{session}"), HttpPost]
        public IHttpActionResult Send(string session, [FromBody] string command)
        {
            foreach (ConsoleState connection in s_activeConnections.Values)
            {
                if (connection.CheckExpiration())
                    s_activeConnections.TryRemove(connection.SessionID, out ConsoleState state);
            }

            if (!Guid.TryParse(session, out Guid sessionID))
                return BadRequest("Invalid Session ID");
            if (!s_activeConnections.ContainsKey(sessionID))
                return BadRequest("Session not found");
            try
            {
                s_activeConnections[sessionID].SendCommand(command, User);
                return Ok(1);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        #endregion
    }
}
