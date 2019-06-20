﻿//******************************************************************************************************
//  BreakerRestrikeOperation.cs - Gbtc
//
//  Copyright © 2019, Grid Protection Alliance.  All Rights Reserved.
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
//  06/18/2019 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using FaultData.DataResources.GTC;
using FaultData.DataSets;
using GSF.Data;

namespace FaultData.DataOperations
{
    public class BreakerRestrikeOperation : DataOperationBase<MeterDataSet>
    {
        public override void Execute(MeterDataSet meterDataSet)
        {
            string breakerRestrikeData = GetBreakerRestrikeData(meterDataSet);

            if (breakerRestrikeData == null)
                return;

            using (AdoDataConnection connection = meterDataSet.CreateDbConnection())
            {
                const string FieldName = "BreakerRestrike";
                const string Description = "Indicates whether the file captures a breaker restrike event.";
                meterDataSet.FileGroup.AddFieldValue(connection, FieldName, breakerRestrikeData, Description);
            }
        }

        private string GetBreakerRestrikeData(MeterDataSet meterDataSet)
        {
            BreakerRestrikeResource breakerRestrikeResource = meterDataSet.GetResource<BreakerRestrikeResource>();
            return breakerRestrikeResource.BreakerRestrikeData;
        }
    }
}
