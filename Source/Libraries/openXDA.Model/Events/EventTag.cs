//******************************************************************************************************
//  EventTag.cs - Gbtc
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
//  02/10/2023 - C. Lackner
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.ComponentModel;
using GSF.Data.Model;

namespace openXDA.Model
{
    [SettingsCategory("systemSettings")]
    public class EventTag
    {
        [PrimaryKey(true)]
        public int ID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        [DefaultValue(true)]
        public bool ShowInFilter { get; set; }
    }

    public static partial class TableOperationsExtensions
    {
        public static EventTag GetOrAdd(this TableOperations<EventTag> eventTagTable, string name, string description = null)
        {
            EventTag eventTag = eventTagTable.QueryRecordWhere("Name = {0}", name);

            if (eventTag is null)
            {
                eventTag = eventTagTable.NewRecord();
                eventTag.Name = name;
                eventTag.Description = description ?? name;

                try
                {
                    eventTagTable.AddNewRecord(eventTag);
                }
                catch (Exception ex)
                {
                    // Ignore errors regarding unique key constraints
                    // which can occur as a result of a race condition
                    bool isUniqueViolation = ExceptionHandler.IsUniqueViolation(ex);

                    if (!isUniqueViolation)
                        throw;

                    return eventTagTable.QueryRecordWhere("Name = {0}", name);
                }

                eventTag.ID = eventTagTable.Connection.ExecuteScalar<int>("SELECT @@IDENTITY");
            }

            return eventTag;
        }
    }
}