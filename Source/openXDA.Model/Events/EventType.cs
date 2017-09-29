//******************************************************************************************************
//  EventType.cs - Gbtc
//
//  Copyright © 2017, Grid Protection Alliance.  All Rights Reserved.
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
//  08/29/2017 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************

using System.Transactions;
using GSF.Data.Model;

namespace openXDA.Model
{
    public class EventType
    {
        [PrimaryKey(true)]
        public int ID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }

    public static partial class TableOperationsExtensions
    {
        public static EventType GetOrAdd(this TableOperations<EventType> eventTypeTable, string name, string description = null)
        {
            TransactionScopeOption required = TransactionScopeOption.Required;

            TransactionOptions transactionOptions = new TransactionOptions()
            {
                IsolationLevel = IsolationLevel.ReadCommitted,
                Timeout = TransactionManager.MaximumTimeout
            };

            EventType eventType;

            using (TransactionScope transactionScope = new TransactionScope(required, transactionOptions))
            {
                eventType = eventTypeTable.QueryRecordWhere("Name = {0}", name);

                if ((object)eventType == null)
                {
                    eventType = new EventType();
                    eventType.Name = name;
                    eventType.Description = description ?? name;
                    eventTypeTable.AddNewRecord(eventType);

                    eventType.ID = eventTypeTable.Connection.ExecuteScalar<int>("SELECT @@IDENTITY");
                }

                transactionScope.Complete();
            }

            return eventType;
        }
    }
}