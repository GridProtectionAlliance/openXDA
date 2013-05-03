//*********************************************************************************************************************
// ServiceClient.cs
// Version 1.1 and subsequent releases
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
// --------------------------------------------------------------------------------------------------------------------
//
// Version 1.0
//
// Copyright 2012 ELECTRIC POWER RESEARCH INSTITUTE, INC. All rights reserved.
//
// openFLE ("this software") is licensed under BSD 3-Clause license.
//
// Redistribution and use in source and binary forms, with or without modification, are permitted provided that the 
// following conditions are met:
//
// •    Redistributions of source code must retain the above copyright  notice, this list of conditions and 
//      the following disclaimer.
//
// •    Redistributions in binary form must reproduce the above copyright notice, this list of conditions and 
//      the following disclaimer in the documentation and/or other materials provided with the distribution.
//
// •    Neither the name of the Electric Power Research Institute, Inc. (“EPRI”) nor the names of its contributors 
//      may be used to endorse or promote products derived from this software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, 
// INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE 
// DISCLAIMED. IN NO EVENT SHALL EPRI BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL 
// DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; 
// OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, 
// OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
// POSSIBILITY OF SUCH DAMAGE.
//
//
// This software incorporates work covered by the following copyright and permission notice: 
//
// •    TVA Code Library 4.0.4.3 - Tennessee Valley Authority, tvainfo@tva.gov
//      No copyright is claimed pursuant to 17 USC § 105. All Other Rights Reserved.
//
//      Licensed under TVA Custom License based on NASA Open Source Agreement (TVA Custom NOSA); 
//      you may not use TVA Code Library except in compliance with the TVA Custom NOSA. You may  
//      obtain a copy of the TVA Custom NOSA at http://tvacodelibrary.codeplex.com/license.
//
//      TVA Code Library is provided by the copyright holders and contributors "as is" and any express 
//      or implied warranties, including, but not limited to, the implied warranties of merchantability 
//      and fitness for a particular purpose are disclaimed.
//
//*********************************************************************************************************************
//
//  Code Modification History:
//  -------------------------------------------------------------------------------------------------------------------
//  09/10/2012 - Stephen C. Wills, Grid Protection Alliance
//       Generated original version of source code.
//
//*********************************************************************************************************************

using System;
using System.ComponentModel;
using System.Text;
using GSF;
using GSF.Console;
using GSF.Reflection;
using GSF.ServiceProcess;

namespace openFLEConsole
{
    public partial class ServiceClient : Component
    {
        #region [ Members ]

        // Fields
        private bool m_telnetActive;
        private ConsoleColor m_originalBgColor;
        private ConsoleColor m_originalFgColor;

        #endregion

        #region [ Constructors ]

        public ServiceClient()
            : base()
        {
            InitializeComponent();

            // Save the color scheme.
            m_originalBgColor = Console.BackgroundColor;
            m_originalFgColor = Console.ForegroundColor;

            // Register event handlers.
            m_clientHelper.AuthenticationFailure += ClientHelper_AuthenticationFailure;
            m_clientHelper.ReceivedServiceUpdate += ClientHelper_ReceivedServiceUpdate;
            m_clientHelper.ReceivedServiceResponse += ClientHelper_ReceivedServiceResponse;
            m_clientHelper.TelnetSessionEstablished += ClientHelper_TelnetSessionEstablished;
            m_clientHelper.TelnetSessionTerminated += ClientHelper_TelnetSessionTerminated;
        }

        #endregion

        #region [ Methods ]

        public void Start(string[] args)
        {
            string userInput = null;
            Arguments arguments = new Arguments(string.Join(" ", args));

            if (arguments.Exists("server"))
            {
                // Override default settings with user provided input. 
                m_clientHelper.PersistSettings = false;
                m_remotingClient.PersistSettings = false;
                if (arguments.Exists("server"))
                    m_remotingClient.ConnectionString = string.Format("Server={0}", arguments["server"]);
            }

            // Connect to service and send commands. 
            m_clientHelper.Connect();
            while (m_clientHelper.Enabled &&
                   string.Compare(userInput, "Exit", true) != 0)
            {
                // Wait for a command from the user. 
                userInput = Console.ReadLine();
                // Write a blank line to the console.
                Console.WriteLine();

                if (!string.IsNullOrEmpty(userInput))
                {
                    // The user typed in a command and didn't just hit <ENTER>. 
                    switch (userInput.ToUpper())
                    {
                        case "CLS":
                            // User wants to clear the console window. 
                            Console.Clear();
                            break;
                        case "EXIT":
                            // User wants to exit the telnet session with the service. 
                            if (m_telnetActive)
                            {
                                userInput = string.Empty;
                                m_clientHelper.SendRequest("Telnet -disconnect");
                            }
                            break;
                        default:
                            // User wants to send a request to the service. 
                            m_clientHelper.SendRequest(userInput);
                            if (string.Compare(userInput, "Help", true) == 0)
                                DisplayHelp();

                            break;
                    }
                }
            }
        }

        private void DisplayHelp()
        {
            StringBuilder help = new StringBuilder();

            help.AppendFormat("Commands supported by {0}:", AssemblyInfo.EntryAssembly.Name);
            help.AppendLine();
            help.AppendLine();
            help.Append("Command".PadRight(20));
            help.Append(" ");
            help.Append("Description".PadRight(55));
            help.AppendLine();
            help.Append(new string('-', 20));
            help.Append(" ");
            help.Append(new string('-', 55));
            help.AppendLine();
            help.Append("Cls".PadRight(20));
            help.Append(" ");
            help.Append("Clears this console screen".PadRight(55));
            help.AppendLine();
            help.Append("Exit".PadRight(20));
            help.Append(" ");
            help.Append("Exits this console screen".PadRight(55));
            help.AppendLine();
            help.AppendLine();
            help.AppendLine();

            Console.Write(help.ToString());
        }

        private void ClientHelper_AuthenticationFailure(object sender, CancelEventArgs e)
        {
            // Prompt for credentials.
            StringBuilder prompt = new StringBuilder();
            prompt.AppendLine();
            prompt.AppendLine();
            prompt.Append("Connection to the service was rejected due to authentication failure. \r\n");
            prompt.Append("Enter the credentials to be used for authentication with the service.");
            prompt.AppendLine();
            prompt.AppendLine();
            Console.Write(prompt.ToString());

            // Capture the username.
            Console.Write("Enter username: ");
            m_clientHelper.Username = Console.ReadLine();

            // Capture the password.
            ConsoleKeyInfo key;
            Console.Write("Enter password: ");
            while ((key = Console.ReadKey(true)).KeyChar != '\r')
            {
                m_clientHelper.Password += key.KeyChar;
            }

            // Re-attempt connection with new credentials.
            e.Cancel = false;
            Console.WriteLine();
            Console.WriteLine();
        }

        private void ClientHelper_ReceivedServiceUpdate(object sender, EventArgs<UpdateType, string> e)
        {
            // Output status updates from the service to the console window.
            switch (e.Argument1)
            {
                case UpdateType.Alarm:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case UpdateType.Information:
                    Console.ForegroundColor = m_originalFgColor;
                    break;
                case UpdateType.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
            }
            Console.Write(e.Argument2);
            Console.ForegroundColor = m_originalFgColor;
        }

        private void ClientHelper_ReceivedServiceResponse(object sender, EventArgs<ServiceResponse> e)
        {
            string sourceCommand;
            bool responseSuccess;

            if (ClientHelper.TryParseActionableResponse(e.Argument, out sourceCommand, out responseSuccess))
            {
                string message = e.Argument.Message;
                string end = string.Format("{0}{0}", Environment.NewLine);

                if (responseSuccess)
                {
                    if (string.IsNullOrWhiteSpace(message))
                        Console.Write("{0} command processed successfully.{1}", sourceCommand, end);
                    else
                        Console.Write(message.EnsureEnd(end));
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;

                    if (string.IsNullOrWhiteSpace(message))
                        Console.Write("{0} failure.{1}", sourceCommand, end);
                    else
                        Console.Write("{0} failure: {1}", sourceCommand, message.EnsureEnd(end));

                    Console.ForegroundColor = m_originalFgColor;
                }
            }
        }

        private void ClientHelper_TelnetSessionEstablished(object sender, EventArgs e)
        {
            // Change the console color scheme to indicate active telnet session.
            m_telnetActive = true;
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Clear();
        }

        private void ClientHelper_TelnetSessionTerminated(object sender, EventArgs e)
        {
            // Revert to original color scheme to indicate end of telnet session.
            m_telnetActive = false;
            Console.BackgroundColor = m_originalBgColor;
            Console.ForegroundColor = m_originalFgColor;
            Console.Clear();
        }

        #endregion
    }
}
