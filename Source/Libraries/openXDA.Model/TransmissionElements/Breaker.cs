//******************************************************************************************************
//  Breaker.cs - Gbtc
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
//  12/13/2019 - Christoph Lackner
//      Generated original version of source code.
//
//******************************************************************************************************

using System.ComponentModel.DataAnnotations;
using GSF.Data;
using GSF.Data.Model;

namespace openXDA.Model
{
    [MetadataType(typeof(Asset))]
    public class Breaker : Asset
    {
        #region [ Properties ]
        
        public double ThermalRating { get; set; }

        public double Speed { get; set; }

        public int TripTime { get; set; } = 0;

        public int PickupTime { get; set; } = 0;

        public double TripCoilCondition { get; set; } = 0.0D;

        public bool AirGapResistor { get; set; } = false;

        #endregion

        #region [ Static ]

        // Static Methods

        public static Breaker DetailedBreaker(Asset asset, AdoDataConnection connection)
        {
            if (connection is null)
                return null;

            TableOperations<Breaker> breakerTable = new(connection);
            Breaker breaker = breakerTable.QueryRecordWhere("ID = {0}", asset.ID);
            if (breaker is null)
                return null;

            breaker.LazyContext = asset.LazyContext;
            return breaker;
        }

        public static Breaker DetailedBreaker(Asset asset)
        {
            using AdoDataConnection connection = asset.ConnectionFactory?.Invoke();
            return DetailedBreaker(asset, connection);
        }

        #endregion
    }
}
