//******************************************************************************************************
//  MeasurementType.cs - Gbtc
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

using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using GSF.Data.Model;

namespace openXDA.Model
{
    public class MeasurementType
    {
        [PrimaryKey(true)]
        public int ID { get; set; }

        [StringLength(200)]
        public string Name { get; set; }

        public string Description { get; set; }
    }

    public static partial class TableOperationsExtensions
    {
        public static MeasurementType GetOrAdd(this TableOperations<MeasurementType> measurementTypeTable, string name, string description = null)
        {
            MeasurementType measurementType = measurementTypeTable.QueryRecordWhere("Name = {0}", name);

            if ((object)measurementType == null)
            {
                measurementType = new MeasurementType();
                measurementType.Name = name;
                measurementType.Description = description ?? name;

                try
                {
                    measurementTypeTable.AddNewRecord(measurementType);
                }
                catch (SqlException ex)
                {
                    // Ignore errors regarding unique key constraints
                    // which can occur as a result of a race condition
                    bool isUniqueViolation = (ex.Number == 2601) || (ex.Number == 2627);

                    if (!isUniqueViolation)
                        throw;

                    return measurementTypeTable.QueryRecordWhere("Name = {0}", name);
                }

                measurementType.ID = measurementTypeTable.Connection.ExecuteScalar<int>("SELECT @@IDENTITY");
            }

            return measurementType;
        }
    }
}
