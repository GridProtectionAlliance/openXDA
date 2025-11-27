//******************************************************************************************************
//  RabbitMQSection.cs - Gbtc
//
//  Copyright © 2025, Grid Protection Alliance.  All Rights Reserved.
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
//  11/01/2025 - C. Lackner
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.ComponentModel;
using System.Configuration;
using GSF.Configuration;

namespace openXDA.Configuration
{
    public class RabbitMQSection
    {
        public const string CategoryName = "RabbitMQ";


        /// <summary>
        /// The RabbbitMQ Server Hostname or IP Address.
        /// </summary>
        [Setting]
        [DefaultValue("localhost")]
        public string Hostname { get; set; } = "localhost";

        /// <summary>
        /// Defines the port RabbitMQ is listening on.
        /// </summary>
        [Setting]
        [DefaultValue(5672)]
        public int Port { get; set; } = 5672;

        /// <summary>
        /// Defines the name of the exchange to listen to.
        /// </summary>
        [Setting]
        [DefaultValue("openxda")]
        public string ExchangeName { get; set; } = "openxda";

        /// <summary>
        /// Defines the routing key used to listen for messages.
        /// </summary>
        [Setting]
        [DefaultValue("openxda")]
        public string RoutingKey { get; set; } = "openxda";

        /// <summary>
        /// Defines a flag that determines if RabbitMQ integration is enabled.
        /// </summary>
        [Setting]
        [DefaultValue(false)]
        public bool Enabled { get; set; } = false;
    }
}
