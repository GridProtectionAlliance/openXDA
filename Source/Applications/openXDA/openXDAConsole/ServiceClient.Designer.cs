//*********************************************************************************************************************
// ServiceClient.Designer.cs
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
// openXDA ("this software") is licensed under BSD 3-Clause license.
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

namespace openXDAConsole
{
    partial class ServiceClient
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                m_clientHelper.Disconnect();
                if (components != null)
                    components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.m_clientHelper = new GSF.ServiceProcess.ClientHelper(this.components);
            this.m_remotingClient = new GSF.Communication.TcpClient(this.components);
            this.m_errorLogger = new GSF.ErrorManagement.ErrorLogger(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.m_clientHelper)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_remotingClient)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_errorLogger)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_errorLogger.ErrorLog)).BeginInit();
            // 
            // m_clientHelper
            // 
            this.m_clientHelper.PersistSettings = true;
            this.m_clientHelper.RemotingClient = this.m_remotingClient;
            // 
            // m_remotingClient
            // 
            this.m_remotingClient.ConnectionString = "Server=localhost:8888";
            this.m_remotingClient.IntegratedSecurity = true;
            this.m_remotingClient.PayloadAware = true;
            this.m_remotingClient.PersistSettings = true;
            this.m_remotingClient.SettingsCategory = "RemotingClient";
            // 
            // m_errorLogger
            // 
            // 
            // 
            // 
            this.m_errorLogger.ErrorLog.FileName = "openXDAConsole.ErrorLog.txt";
            this.m_errorLogger.LogToUI = true;
            ((System.ComponentModel.ISupportInitialize)(this.m_clientHelper)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_remotingClient)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_errorLogger.ErrorLog)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_errorLogger)).EndInit();

        }

        #endregion

        private GSF.ServiceProcess.ClientHelper m_clientHelper;
        private GSF.Communication.TcpClient m_remotingClient;
        private GSF.ErrorManagement.ErrorLogger m_errorLogger;


    }
}
