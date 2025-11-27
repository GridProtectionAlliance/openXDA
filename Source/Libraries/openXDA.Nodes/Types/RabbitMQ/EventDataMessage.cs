//******************************************************************************************************
//  EventDataMessage.cs - Gbtc
//
//  Copyright © 2021, Grid Protection Alliance.  All Rights Reserved.
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
//  11/20/2025 - C. Lackner
//       Generated original version of source code.
//
//******************************************************************************************************

using GSF;
using GSF.Configuration;
using GSF.Data;
using GSF.IO;
using GSF.Parsing;
using log4net;
using Newtonsoft.Json.Linq;
using openXDA.Configuration;
using openXDA.Model;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace openXDA.Nodes.Types.RabbitMQ
{
    public class EventDataMessage 
    {
        /// <summary>
        /// Unique ebvent identifier used in response
        /// </summary>
        public int event_id { get; set; }
        public double[] Va { get; set; } 
        public double[] Vb { get; set; }
        public double[] Vc { get; set; }
        /// <summary>
        /// Samples Per Cycle
        /// </summary>
        public int sample_rate { get; set; } 
        /// <summary>
        /// Samples per second
        /// </summary>
        public double sample_frequency { get; set; }
        public int event_start_idx { get; set; }
        public string event_type { get; set; }
    }
}
