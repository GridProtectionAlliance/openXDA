﻿//******************************************************************************************************
//  SegmentType.cs - Gbtc
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
//  06/26/2017 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using GSF.Data.Model;

namespace openXDA.Model
{
    public class SegmentType
    {
        [PrimaryKey(true)]
        public int ID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }

    public static partial class TableOperationsExtensions
    {
        public static SegmentType GetOrAdd(this TableOperations<SegmentType> segmentTypeTable, string name, string description = null)
        {
            SegmentType segmentType = segmentTypeTable.QueryRecordWhere("Name = {0}", name);

            if ((object)segmentType == null)
            {
                segmentType = new SegmentType();
                segmentType.Name = name;
                segmentType.Description = description ?? name;

                try
                {
                    segmentTypeTable.AddNewRecord(segmentType);
                }
                catch (Exception ex)
                {
                    // Ignore errors regarding unique key constraints
                    // which can occur as a result of a race condition
                    bool isUniqueViolation = ExceptionHandler.IsUniqueViolation(ex);

                    if (!isUniqueViolation)
                        throw;

                    return segmentTypeTable.QueryRecordWhere("Name = {0}", name);
                }

                segmentType.ID = segmentTypeTable.Connection.ExecuteScalar<int>("SELECT @@IDENTITY");
            }

            return segmentType;
        }
    }
}
