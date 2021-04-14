//******************************************************************************************************
//  CapBankRelay..cs - Gbtc
//
//  Copyright © 2020, Grid Protection Alliance.  All Rights Reserved.
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
//  07/22/2020 - Christoph Lackner
//      Generated original version of source code.
//
//******************************************************************************************************

using System.ComponentModel.DataAnnotations;
using GSF.Data;
using GSF.Data.Model;

namespace openXDA.Model
{
    /// <summary>
    /// The following Lines are from Asset:
    /// 34 : VoltageKV
    /// 
    /// The following Lines are Systemwide Settings
    /// 33
    /// </summary>
    [MetadataType(typeof(Asset))]
    public class CapBankRelay : Asset
    {
        #region [ Members ]

        #endregion

        #region [ Properties ]


        /// <summary>
        /// Line 35
        /// </summary>
        public double OnVoltageThreshhold { get; set; }

        public int CapBankNumber { get; set; }
        #endregion

        #region [ Methods ]

        public static CapBankRelay DetailedCapBankRelay(Asset asset, AdoDataConnection connection)
        {
            if ((object)connection == null)
                return null;

            TableOperations<CapBankRelay> capBankTable = new TableOperations<CapBankRelay>(connection);
            CapBankRelay capBank = capBankTable.QueryRecordWhere("ID = {0}", asset.ID);
            capBank.LazyContext = asset.LazyContext;
            capBank.ConnectionFactory = asset.ConnectionFactory;

            return capBank;
        }

        public static CapBankRelay DetailedCapBankRelay(Asset asset)
        {
            return DetailedCapBankRelay(asset, asset.ConnectionFactory.Invoke());
        }

        #endregion
    }
}
