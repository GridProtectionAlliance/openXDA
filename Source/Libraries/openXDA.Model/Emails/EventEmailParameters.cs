//******************************************************************************************************
//  EventEmailParameters.cs - Gbtc
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
//  07/24/2018 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using GSF.Data;
using GSF.Data.Model;
using Newtonsoft.Json;

namespace openXDA.Model
{
    public class EventEmailParameters
    {
        [PrimaryKey(true)]
        public int ID { get; set; }

        public int EmailTypeID { get; set; }

        public string TriggersEmailSQL { get; set; }

        public string EventDetailSQL { get; set; }

        public string TriggerSource { get; set; }

        public double MinDelay { get; set; }

        public double MaxDelay { get; set; }

        [JsonIgnore]
        [NonRecordField]
        public TimeSpan MinDelaySpan => TimeSpan.FromSeconds(MinDelay);

        [JsonIgnore]
        [NonRecordField]
        public TimeSpan MaxDelaySpan => TimeSpan.FromSeconds(MaxDelay);

        [JsonIgnore]
        [NonRecordField]
        public Func<AdoDataConnection> ConnectionFactory { get; set; }

        public bool TriggersEmail(int eventID)
        {
            Func<AdoDataConnection> connectionFactory = ConnectionFactory;

            if ((object)connectionFactory == null)
                throw new InvalidOperationException("ConnectionFactory is undefined");

            using (AdoDataConnection connection = connectionFactory())
            {
                return connection.ExecuteScalar<bool>(TriggersEmailSQL, eventID);
            }
        }

        public string GetEventDetail(int eventID)
        {
            Func<AdoDataConnection> connectionFactory = ConnectionFactory;

            if ((object)connectionFactory == null)
                throw new InvalidOperationException("ConnectionFactory is undefined");

            using (AdoDataConnection connection = connectionFactory())
            {
                return connection.ExecuteScalar<string>(EventDetailSQL, eventID);
            }
        }
    }
}
