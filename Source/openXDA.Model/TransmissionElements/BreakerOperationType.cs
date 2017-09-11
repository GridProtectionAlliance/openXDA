//******************************************************************************************************
//  BreakerOperationType.cs - Gbtc
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
//  09/08/2017 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System.ComponentModel.DataAnnotations;
using System.Transactions;
using GSF.Data.Model;

namespace openXDA.Model
{
    public class BreakerOperationType
    {
        [PrimaryKey(true)]
        public int ID { get; set; }

        [StringLength(200)]
        public string Name { get; set; }

        public string Description { get; set; }
    }

    public static partial class TableOperationsExtensions
    {
        public static BreakerOperationType GetOrAdd(this TableOperations<BreakerOperationType> breakerOperationTypeTable, string name, string description = null)
        {
            TransactionScopeOption required = TransactionScopeOption.Required;

            TransactionOptions transactionOptions = new TransactionOptions()
            {
                IsolationLevel = IsolationLevel.ReadCommitted,
                Timeout = TransactionManager.MaximumTimeout
            };

            BreakerOperationType breakerOperationType;

            using (TransactionScope transactionScope = new TransactionScope(required, transactionOptions))
            {
                breakerOperationType = breakerOperationTypeTable.QueryRecordWhere("Name = {0}", name);

                if ((object)breakerOperationType == null)
                {
                    breakerOperationType = new BreakerOperationType();
                    breakerOperationType.Name = name;
                    breakerOperationType.Description = description ?? name;
                    breakerOperationTypeTable.AddNewRecord(breakerOperationType);

                    breakerOperationType.ID = breakerOperationTypeTable.Connection.ExecuteScalar<int>("SELECT @@IDENTITY");
                }
            }

            return breakerOperationType;
        }
    }
}
