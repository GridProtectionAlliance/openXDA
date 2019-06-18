//******************************************************************************************************
//  BreakerRestrikeResource.cs - Gbtc
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
//  06/13/2019 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using FaultData.DataSets;

namespace FaultData.DataResources.GTC
{
    public class BreakerRestrikeResource : DataResourceBase<MeterDataSet>
    {
        #region [ Members ]

        // Constants
        private const string APPSectionName = "APP FaultSummary";
        private const string APPKey = "IsBreakerRestrike";
        private const string USISectionName = "USI Quicksummary";
        private const string USIKey = "BrkrRestrike";

        #endregion

        #region [ Constructors ]

        public BreakerRestrikeResource() { }

        public BreakerRestrikeResource(INFDataSet infDataSet)
        {
            if (infDataSet == null)
                return;

            string value =
                infDataSet.GetValue(APPSectionName, APPKey) ??
                infDataSet.GetValue(USISectionName, USIKey);

            BreakerRestrikeData = value;
        }

        #endregion

        #region [ Properties ]

        public string BreakerRestrikeData { get; }

        #endregion

        #region [ Methods ]

        public override void Initialize(MeterDataSet dataSet) { }

        #endregion
    }
}
