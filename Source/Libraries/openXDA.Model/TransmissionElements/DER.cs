//******************************************************************************************************
//  DER.cs - Gbtc
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
//  10/19/2021 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************

using System.ComponentModel.DataAnnotations;
using GSF.Data;
using GSF.Data.Model;

namespace openXDA.Model
{
    [MetadataType(typeof(Asset))]
    public class DER : Asset
    {
        #region [ Properties ]
        
        public double FullRatedOutputCurrent { get; set; }

        public string VoltageLevel { get; set; }

        #endregion

        #region [ Static ]

        // Static Methods

        public static DER DetailedDER(Asset asset, AdoDataConnection connection)
        {
            if (connection is null)
                return null;

            TableOperations<DER> table = new(connection);
            DER record = table.QueryRecordWhere("ID = {0}", asset.ID);
            if (record is null)
                return null;

            record.LazyContext = asset.LazyContext;
            return record;
        }

        public static DER DetailedDER(Asset asset)
        {
            using AdoDataConnection connection = asset.ConnectionFactory?.Invoke();
            return DetailedDER(asset, connection);
        }

        #endregion
    }
}
