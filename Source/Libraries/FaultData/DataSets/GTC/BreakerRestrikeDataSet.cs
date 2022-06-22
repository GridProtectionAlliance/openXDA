//******************************************************************************************************
//  BreakerRestrikeDataSet.cs - Gbtc
//
//  Copyright © 2022, Grid Protection Alliance.  All Rights Reserved.
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
//  06/20/2022 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

namespace FaultData.DataSets.GTC
{
    public class BreakerRestrikeDataSet
    {
        private const string APPSectionName = "APP FaultSummary";
        private const string APPKey = "BreakerRestrike";
        private const string USISectionName = "USI QuickSummary";
        private const string USIKey = "BrkrRestrike";

        public string BreakerRestrikeData { get; }

        private BreakerRestrikeDataSet(string breakerRestrikeData) =>
            BreakerRestrikeData = breakerRestrikeData;

        public static BreakerRestrikeDataSet Create(INFDataSet infDataSet)
        {
            if (infDataSet is null)
                return null;

            string value =
                infDataSet.GetValue(APPSectionName, APPKey) ??
                infDataSet.GetValue(USISectionName, USIKey);

            if (value is null)
                return null;

            return new BreakerRestrikeDataSet(value);
        }
    }
}
