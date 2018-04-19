//******************************************************************************************************
//  Program.cs - Gbtc
//
//  Copyright © 2018, Grid Protection Alliance.  All Rights Reserved.
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
//  04/15/2018 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

#if DEBUG
#define RunAsApp
#endif

#if RunAsApp
    using System.Windows.Forms;
#else
    using System.ServiceProcess;
#endif

namespace openXDAFileWatcher
{
    static class Program
    {
        /// <summary>
        /// The service host instance for the application.
        /// </summary>
        public static readonly ServiceHost Host = new ServiceHost();

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
#if RunAsApp
            // Run as Windows Application.
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new DebugHost(Host));
#else
            // Run as Windows Service.
            ServiceBase.Run(Host);
#endif
        }
    }
}
