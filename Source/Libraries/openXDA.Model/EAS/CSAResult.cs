//******************************************************************************************************
//  CSAResult.cs - Gbtc
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
//  06/23/2017 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************

using GSF.Data.Model;

namespace openXDA.Model
{
    public class CSAResult
    {
        [PrimaryKey(true)]
        public int ID { get; set; }

        public int EventID { get; set; }

        public string IsDataError { get; set; }

        public string IsCapSwitch { get; set; }

        public string IsCapSwitchCondL { get; set; }

        public double OutFrequency { get; set; }

        public double OutVoltagesMax { get; set; }

        public double OutVoltagesMean { get; set; }

        public string OutQConditionRPBFlag { get; set; }

        public double OutQConditionMRPC { get; set; }

        public double OutQConditionRPCA { get; set; }

        public double OutQConditionRPCB { get; set; }

        public double OutQConditionRPCC { get; set; }

        public double OutQConditionMPFI { get; set; }

        public double OutQConditionPFA { get; set; }

        public double OutQConditionPFB { get; set; }

        public double OutQConditionPFC { get; set; }

        public string OutRestrikeFlag { get; set; }

        public int OutRestrikeNum { get; set; }

        public string OutRestrikePHA { get; set; }

        public string OutRestrikePHB { get; set; }

        public string OutRestrikePHC { get; set; }

        public string OutVTHDFlag { get; set; }

        public double OutVTHDBefore { get; set; }

        public double OutVTHDAfter { get; set; }

        public double OutVTHDIncrease { get; set; }
    }
}
